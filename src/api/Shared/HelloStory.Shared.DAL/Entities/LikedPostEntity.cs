using HelloStory.Shared.DAL.Enities;
using HelloStory.Shared.DAL.Entities.Abstract;

namespace HelloStory.Shared.DAL.Entities;

public sealed record LikedPostEntity(
    Guid PostId,
    Guid UserId
) : EntityBase
{
    public PostEntity? Post { get; init; }
    public UserEntity? User { get; init; }
}