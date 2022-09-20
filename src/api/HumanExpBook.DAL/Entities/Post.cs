using HumanExpBook.DAL.Entities.Abstract;

namespace HumanExpBook.DAL.Entities;

public sealed record Post(
    Guid UserId,
    string Content
) : EntityBase
{
    public int Likes { get; private set; }
    public bool IsEdited { get; set; }
    public DateTime CreatedAt { get; }
    public User? User { get; init; }

    public void Like()
        => Likes += 1;
}