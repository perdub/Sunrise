namespace Sunrise;

public static class StreamExtensions{
    public static byte[] ToByteArray(this Stream s){
        if(s.CanSeek){
            s.Position = 0;
        }
        byte[] arr = new byte[s.Length];
        s.Read(arr, 0, arr.Length);
        return arr;
    }
}