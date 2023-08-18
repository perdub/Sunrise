namespace Sunrise.Utilities;
//этот класс выполняет преоброзование тега, а именно убирает и заменяет различные знаки
public static class TagProcess{
    public static string Process(this string s){
        return s.ToLowerInvariant()
            .Replace("!","")
            .Replace("?","")
            .Replace('-','_')
            ;
    }
}