namespace Sunrise.Types;

public class EndpointError{
    public string ErrorText{get;set;}
    public string ErrorDescription{get;set;}
    public bool HasError{get;set;}

    public static EndpointError BuildError(string errorText = "", string errorDescription = ""){
        return new EndpointError{
            HasError = true,
            ErrorDescription = errorDescription,
            ErrorText = errorText
        };
    }
}