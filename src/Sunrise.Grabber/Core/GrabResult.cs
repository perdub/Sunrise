namespace Sunrise.Grabber;

public class GrabResult{
    public bool Success { get; set; }
    public byte[] Data { get; set; }
    public GrabResult()
    {
        Success = false;
        Data = null;
    }
    public GrabResult(bool success, byte[] data)
    {
        Success = success;
        Data = data;
    }
}