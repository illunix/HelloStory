using HelloStory.Shared.DAL.Enities;
using Microsoft.EntityFrameworkCore;

namespace HelloStory.Shared.DAL.Context;

public sealed class HelloStoryContext : DbContext
{
    public DbSet<UserEntity> Users { get; init; } = null!;

    public HelloStoryContext(DbContextOptions<HelloStoryContext> options)
    : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
    }
}
