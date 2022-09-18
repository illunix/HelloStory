using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace HumanExpBook.API.CodeGeneration.Extensions;

internal static class StringExtensions
{
    public static string PascalToKebabCase(this string value)
    {
        if (string.IsNullOrEmpty(value))
            return value;

        return Regex.Replace(
            value,
            "(?<!^)([A-Z][a-z]|(?<=[a-z])[A-Z])",
            "-$1",
            RegexOptions.Compiled)
            .Trim()
            .ToLower();
    }
}