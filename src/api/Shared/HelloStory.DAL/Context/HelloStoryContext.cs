using HelloStory.DAL.Enities;
using Microsoft.EntityFrameworkCore;

namespace HelloStory.DAL.Context;

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
