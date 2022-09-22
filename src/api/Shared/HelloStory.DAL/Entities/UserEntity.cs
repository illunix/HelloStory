using HelloStory.DAL.Entities.Abstract;

namespace HelloStory.DAL.Enities;

public sealed record UserEntity(
    string Email,
    string Username,
    string Password,
    string Salt
) : EntityBase
{
    public DateTime CreatedAt { get; }
}