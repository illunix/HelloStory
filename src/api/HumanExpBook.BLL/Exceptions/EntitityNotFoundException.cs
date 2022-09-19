namespace HumanExpBook.BLL.Exceptions;

public sealed class EntitityNotFoundException : Exception
{
    public EntitityNotFoundException(
        string name,
        int id
    ) : base($"Entity {name} with id ({id}) was not found.") { }

    public EntitityNotFoundException(string name) : base($"Entity {name} was not found.") { }
}