namespace Sunrise.Logger;

public class Logger
{
    //ревлизация сингелтона
    static Logger _s;
    public static Logger Singelton {get{return _s;}}

    //класс который собирает и шлёт информацию в логгеры
    ILogger[] _loggers;
    DateTime _loggerInitTime;
    public Logger(params ILogger[] loggers)
    {
        _s = this;
        _loggerInitTime = DateTime.Now;
        _loggers = loggers;
        foreach(var q in _loggers)
            q.Initialize();
    }

    StringBuilder getLine(string m){
        StringBuilder bld = new StringBuilder();
        bld.Append('[');
        bld.Append((DateTime.Now - _loggerInitTime).ToString());
        bld.Append(']');
        bld.Append(' ');
        bld.Append(m);
        return bld;
    }

    public void Write(string message)
    {
        var r = getLine(message);
        foreach(var q in _loggers)
            q.Write(r);
    }
}