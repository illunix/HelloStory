﻿using HumanExpBook.DAL.Entities;
using Microsoft.EntityFrameworkCore;

namespace HumanExpBook.DAL.Context;

public sealed class InternalDbContext : DbContext
{
    public DbSet<User> Users { get; init; }
    public DbSet<UserFollow> UserFollows { get; init; }
    public DbSet<Post> Posts { get; init; }
    public DbSet<Comment> Comments { get; init; }

    public InternalDbContext(DbContextOptions<InternalDbContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
    }
}