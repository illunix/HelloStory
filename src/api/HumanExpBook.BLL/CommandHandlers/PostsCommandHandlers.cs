﻿using HumanExpBook.BLL.Commands.Posts;
using HumanExpBook.BLL.Exceptions;
using HumanExpBook.BLL.Interfaces;
using HumanExpBook.DAL.Context;
using HumanExpBook.DAL.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HumanExpBook.BLL.CommandHandlers;

public sealed partial class PostsCommandHandlers :
    IHttpRequestHandler<CreatePostCommand>,
    IHttpRequestHandler<UpdatePostCommand>,
    IHttpRequestHandler<DeletePostCommand>
{
    private readonly InternalDbContext _ctx;

    [HttpPost]
    public async Task<IResult> Handle(
        CreatePostCommand req,
        CancellationToken ct
    )
    {
        _ctx.Add(new Post(
            req.CurrentUserId,
            req.Content
        ));

        return await Task.FromResult(Results.Ok());
    }

    [HttpPut]
    public async Task<IResult> Handle(
        UpdatePostCommand req,
        CancellationToken ct
    )
    {
        var post = await _ctx.Posts.FindAsync(req.Id);
        if (post is null)
            throw new NotFoundException(nameof(Post));

        _ctx.Update(post with 
        { 
            Content = req.Content,
            Edited = true 
        });

        return Results.Ok();
    }

    [HttpDelete]
    public async Task<IResult> Handle(
        DeletePostCommand req,
        CancellationToken ct
    )
    {
        var post = await _ctx.Posts.FindAsync(req.Id);
        if (post is null)
            throw new NotFoundException(nameof(Post));

        _ctx.Remove(post);

        return Results.Ok();
    }
}