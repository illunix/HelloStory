namespace HelloStory.Shared.BLL.Exceptions;

public sealed class EntityNotFoundException : Exception
{
    public EntityNotFoundException(
        string name,
        int id
    ) : base($"{name} with id ({id}) was not found.") { }

    public EntityNotFoundException(string name) : base($"{name} was not found.") { }
}