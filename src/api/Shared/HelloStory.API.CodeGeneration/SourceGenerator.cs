using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Text;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Text.RegularExpressions;

namespace HelloStory.Shared.API.CodeGeneration;

[Generator]
internal class SourceGenerator : ISourceGenerator
{
    public void Initialize(GeneratorInitializationContext ctx)
        => ctx.RegisterForSyntaxNotifications(() => new SyntaxReceiver());

    public void Execute(GeneratorExecutionContext ctx)
    {
        if (ctx.SyntaxReceiver is not SyntaxReceiver syntaxReceiver)
        {
            return;
        }

        var methods = GetMethods(
            ctx,
            syntaxReceiver
        );

        ctx.AddSource(
            "HelloStory.API.Extensions.g.cs",
            SourceText.From(
                GenerateExtensions(methods),
                Encoding.UTF8
            )
        );
    }

    private static string GenerateExtensions(IEnumerable<IMethodSymbol> methods)
    {
        var generateMethods = () =>
        {
            var pascalToKebabCase = (string value) =>
            {
                if (string.IsNullOrEmpty(value))
                    return value;

                return Regex.Replace(
                    value,
                    "(?<!^)([A-Z][a-z]|(?<=[a-z])[A-Z])",
                    "-$1",
                    RegexOptions.Compiled
                )
                    .Trim()
                    .ToLower();
            };

            var sb = new StringBuilder();

            foreach (var method in methods)
            {
                var httpAttr = method.GetAttributes().Where(q => q.AttributeClass!.Name.StartsWith("Http")).FirstOrDefault()!;
                var httpMethod = httpAttr.AttributeClass?.Name
                    .Replace(
                        "Http",
                        ""
                    )
                    .Replace(
                        "Attribute",
                        ""
                    );
                var requestType = method.Parameters.FirstOrDefault()!;
                var requestTypeType = requestType.Type;
                var validate = requestTypeType.GetMembers().Any();

                var handlerName = method.ContainingType.Name
                    .Replace(
                        "CommandHandlers",
                        ""
                    )
                    .Replace(
                        "QueryHandlers",
                        ""
                    );

                var requestName = "";

                if (httpAttr.ConstructorArguments.FirstOrDefault().Value is not null)
                {
                    requestName = httpAttr.ConstructorArguments.FirstOrDefault().Value!.ToString();
                }
                else if (
                    requestTypeType.Name.StartsWith("Get") ||
                    requestTypeType.Name.StartsWith("GetAll") ||
                    requestTypeType.Name.StartsWith("Create") ||
                    requestTypeType.Name.StartsWith("Update") ||
                    requestTypeType.Name.StartsWith("Delete")
                )
                {
                    requestName = "";
                }
                else
                {
                    requestName = pascalToKebabCase(requestTypeType.Name
                        .Replace(
                            "Command",
                            ""
                        )
                        .Replace(
                            "Query",
                            ""
                        ));
                }

                var route = $"{pascalToKebabCase(handlerName)}/{requestName}";

                var getCurrentUser = requestTypeType.GetMembers().Any(q => q.Name == "CurrentUserId");

                sb.AppendLine(
    @$"app.Map{httpMethod}(
            ""{route}"",
            async (
                {(getCurrentUser ? "ClaimsPrincipal user,\n\t\t\t\t" : "")}[AsParameters] {requestTypeType} req,
                IMediator mediator
            )
                => await mediator.Send(req{(getCurrentUser ? @" with { CurrentUserId = Guid.Parse(user.Claims.FirstOrDefault(q => q.Type == ClaimTypes.NameIdentifier).Value)}" : "")})
            ){(validate ? "" : $".AddEndpointFilter<ValidationFilter<{requestTypeType}>>()")}.WithTags(""{handlerName}"");"
                );
            }

            return sb.ToString();
        };

        var serviceName = methods.First().ContainingNamespace.ContainingNamespace.ContainingNamespace.Name;

        var sb = new StringBuilder();

        sb.AppendLine(
@$"using System.Security.Claims;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MediatR;
using HelloStory.Shared.API.Filters;

namespace HelloStory.{serviceName}.API.Extensions;

public static class ApiExtensions
{{
    public static WebApplication MapEndpoints(this WebApplication app)
    {{
        {generateMethods()}
        return app;
    }}
}}
"
        );

        return sb.ToString();
    }

    private static IEnumerable<IMethodSymbol> GetMethods(
       GeneratorExecutionContext context,
       SyntaxReceiver receiver
    )
    {
        var compilation = context.Compilation;

        foreach (var @method in receiver.CandidateMethods)
        {
            var model = compilation.GetSemanticModel(@method.SyntaxTree);
            var methodSymbol = (IMethodSymbol)model.GetDeclaredSymbol(@method)!;
            if (methodSymbol is null)
            {
                break;
            }

            if (methodSymbol.GetAttributes()
                .Select(q => q.AttributeClass?.Name)
                .Any(q =>
                    q == nameof(HttpGetAttribute) ||
                    q == nameof(HttpPostAttribute) ||
                    q == nameof(HttpPutAttribute) ||
                    q == nameof(HttpDeleteAttribute)
                )
            )
            {
                yield return methodSymbol;
            }
        }
    }
}
