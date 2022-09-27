using HelloStory.Shared.DAL.Entities.Abstract;

namespace HelloStory.Shared.DAL.Entities;

public sealed record PostEntity(
    Guid UserId,
    string Content
) : EntityBase
{
    public DateTime CreatedAt { get; } = DateTime.Now;
    public ICollection<LikedPostEntity>? Likes { get; set; }
}