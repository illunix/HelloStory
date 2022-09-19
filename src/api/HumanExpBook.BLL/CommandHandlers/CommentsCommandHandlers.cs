using HumanExpBook.BLL.Commands.Comments;
using HumanExpBook.BLL.Exceptions;
using HumanExpBook.BLL.Interfaces;
using HumanExpBook.DAL.Context;
using HumanExpBook.DAL.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HumanExpBook.BLL.CommandHandlers;

public sealed partial class CommentsCommandHandlers :
    IHttpRequestHandler<CreateCommentCommand>,
    IHttpRequestHandler<UpdateCommentCommand>,
    IHttpRequestHandler<DeleteCommentCommand>
{
    private readonly InternalDbContext _ctx;

    [HttpPost]
    public async Task<IResult> Handle(
        CreateCommentCommand req,
        CancellationToken ct
    )
    {
        _ctx.Add(new Comment(
            req.PostId,
            req.CurrentUserId,
            req.ParentCommentId,
            req.Content
        ));

        await _ctx.SaveChangesAsync();

        return Results.Ok();
    }

    [HttpPut]
    public async Task<IResult> Handle(
        UpdateCommentCommand req,
        CancellationToken ct
    )
    {
        var comment = await _ctx.Comments.FindAsync(req.Id);
        if (comment is null)
            throw new EntitityNotFoundException(nameof(Comment));

        _ctx.Update(comment with
        {
            Content = req.Content,
            IsEdited = true
        });

        await _ctx.SaveChangesAsync();

        return Results.Ok();
    }

    [HttpDelete]
    public async Task<IResult> Handle(
        DeleteCommentCommand req,
        CancellationToken ct
    )
    {
        var comment = await _ctx.Comments.FindAsync(req.Id);
        if (comment is null)
            throw new EntitityNotFoundException(nameof(Comment));

        _ctx.Remove(comment);

        await _ctx.SaveChangesAsync();

        return Results.Ok();
    }
}