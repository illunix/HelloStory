using HelloStory.Shared.DAL.Entities.Abstract;

namespace HelloStory.Shared.DAL.Enities;

public sealed record UserEntity(
    string Email,
    string Username,
    string Password,
    string Salt
) : EntityBase
{
    public DateTime CreatedAt { get; }
}