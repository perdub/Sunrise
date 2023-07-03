namespace Sunrise.Logger;

public interface ILogger
{
    void Write(StringBuilder message);
    void Initialize();
}