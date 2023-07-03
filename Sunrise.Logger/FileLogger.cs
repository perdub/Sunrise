namespace Sunrise.Logger;

public class FileLogger : ILogger
{
    StreamWriter _writer;
    public void Initialize()
    {
        //создание папки для логов и выбор имени файла
        Directory.CreateDirectory("logs");
        string logFileName = "logs/";
        #if DEBUG
logFileName+=DateTime.Now.ToString("yyyyMMddhhmmss")+".log";
        #endif
        #if RELEASE
logFileName += "lastlog.log"
        #endif

        _writer = new StreamWriter(logFileName);
    }

    public void Write(StringBuilder message)
    {
        _writer.WriteLine(message);
        _writer.Flush();
    }
}
