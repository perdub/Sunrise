namespace Sunrise.Types.Exceptions;

[System.Serializable]
public class InvalidParamsException : System.Exception
{
    public InvalidParamsException() { }
    public InvalidParamsException(string message) : base("Value can`t be a empty, null or invalid.\n"+message) { }
    public InvalidParamsException(string message, System.Exception inner) : base(message, inner) { }
    protected InvalidParamsException(
        System.Runtime.Serialization.SerializationInfo info,
        System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
}