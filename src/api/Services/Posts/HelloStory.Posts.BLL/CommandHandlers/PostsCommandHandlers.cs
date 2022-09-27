using HelloStory.Posts.BLL.Commands;
using HelloStory.Shared.BLL.Exceptions;
using HelloStory.Shared.BLL.Interfaces;
using HelloStory.Shared.DAL.Context;
using HelloStory.Shared.DAL.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace HelloStory.Posts.BLL.CommandHandlers;

public sealed partial class PostsCommandHandlers :
    IHttpRequestHandler<CreatePostCommand>,
    IHttpRequestHandler<UpdatePostCommand>,
    IHttpRequestHandler<DeletePostCommand>,
    IHttpRequestHandler<LikePostCommand>
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

    public async Task<IResult> Handle(
        LikePostCommand req,
        CancellationToken ct
    )
    {
        if (await _ctx.Posts.AnyAsync(q => q.Id == req.PostId))
            throw new EntityNotFoundException(nameof(PostEntity));

        if (await _ctx.Users.AnyAsync(q => q.Id == req.UserId))
            throw new EntityNotFoundException(nameof(PostEntity));

        _ctx.Add(new LikedPostEntity(
            req.PostId,
            req.UserId
        ));

        await _ctx.SaveChangesAsync();

        return Results.Ok();
    }
}
