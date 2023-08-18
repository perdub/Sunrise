namespace Sunrise.Types.Exceptions;

[System.Serializable]
public class InvalidObjectTypeException : System.Exception
{
    public InvalidObjectTypeException() { }
    public InvalidObjectTypeException(string message) : base(message) { }
    public InvalidObjectTypeException(string message, System.Exception inner) : base(message, inner) { }
    protected InvalidObjectTypeException(
        System.Runtime.Serialization.SerializationInfo info,
        System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
}