namespace Sunrise.Utilities;

public static class StreamExtensions{
    public static byte[] ToByteArray(this Stream s){
        if(!s.CanRead){
            throw new Exception("Fall to read stream");
        }
        byte[] res = new byte[s.Length];
        s.Read(res,0,res.Length);
        return res;
    }
}