using HelloStory.Shared.DAL.Enities;
using HelloStory.Shared.DAL.Entities;
using Microsoft.EntityFrameworkCore;

namespace HelloStory.Shared.DAL.Context;

public sealed class HelloStoryContext : DbContext
{
    public DbSet<UserEntity> Users { get; init; } = null!;
    public DbSet<LikedPostEntity> LikedPosts { get; init; } = null!;
    public DbSet<PostEntity> Posts { get; init; } = null!;

    public HelloStoryContext(DbContextOptions<HelloStoryContext> options)
    : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
    }
}
