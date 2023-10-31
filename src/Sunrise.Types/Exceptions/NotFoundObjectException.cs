namespace Sunrise.Types.Exceptions;

[System.Serializable]
public class NotFoundObjectException : System.Exception
{
    public NotFoundObjectException() { }
    public NotFoundObjectException(string message) : base(message) { }
    public NotFoundObjectException(string message, System.Exception inner) : base(message, inner) { }
    protected NotFoundObjectException(
        System.Runtime.Serialization.SerializationInfo info,
        System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
}