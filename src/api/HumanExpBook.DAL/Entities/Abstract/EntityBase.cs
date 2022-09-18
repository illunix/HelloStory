namespace HumanExpBook.DAL.Entities.Abstract;

public record EntityBase
{
    public Guid Id { get; } = Guid.NewGuid();
}