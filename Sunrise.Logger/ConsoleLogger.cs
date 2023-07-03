namespace Sunrise.Logger;

public class ConsoleLogger : ILogger
{
    public void Initialize()
    {
    }

    public void Write(StringBuilder message)
    {
        Console.WriteLine(message.ToString());
    }
}