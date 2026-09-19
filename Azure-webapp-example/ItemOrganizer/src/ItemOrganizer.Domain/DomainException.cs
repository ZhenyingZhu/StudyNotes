namespace ItemOrganizer.Domain;

public sealed class DomainException : InvalidOperationException
{
    public DomainException(string message)
        : base(message)
    {
    }
}
