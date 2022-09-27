using HelloStory.Posts.BLL.Commands;
using HelloStory.Shared.BLL.Exceptions;
using HelloStory.Shared.BLL.Interfaces;
using HelloStory.Shared.DAL.Context;
using HelloStory.Shared.DAL.Entities;
using Microsoft.AspNetCore.Http;

namespace HelloStory.Posts.BLL.CommandHandlers;

public sealed partial class PostsCommandHandlers :
    IHttpRequestHandler<CreatePostCommand>,
    IHttpRequestHandler<UpdatePostCommand>,
    IHttpRequestHandler<DeletePostCommand>
{
    private readonly HelloStoryContext _ctx;

    public async Task<IResult> Handle(
        CreatePostCommand req,
        CancellationToken ct
    )
    {
        _ctx.Add(new PostEntity(
            req.UserId,
            req.Content
        ));

        await _ctx.SaveChangesAsync();

        return Results.Ok();
    }

    public async Task<IResult> Handle(
        UpdatePostCommand req,
        CancellationToken ct
    )
    {
        var post = await _ctx.Posts.FindAsync(req.Id);
        if (post is null)
            throw new EntityNotFoundException(nameof(PostEntity));

        _ctx.Update(post with
        {
            Content = req.Content
        });

        await _ctx.SaveChangesAsync();

        return Results.Ok();
    }

    public async Task<IResult> Handle(
        DeletePostCommand req,
        CancellationToken ct
    )
    {
        var post = await _ctx.Posts.FindAsync(req.Id);
        if (post is null)
            throw new EntityNotFoundException(nameof(PostEntity));

        _ctx.Remove(post);

        await _ctx.SaveChangesAsync();

        return Results.Ok();
    }
}
