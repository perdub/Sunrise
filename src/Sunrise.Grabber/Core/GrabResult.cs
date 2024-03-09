namespace Sunrise.Grabber;

public class GrabResult{
    public bool Success { get; set; }
    public byte[] Data { get; set; }
    public string? Tags { get; set; } = null;
    public GrabResult()
    {
        Success = false;
        Data = null;
    }
    public GrabResult(bool success, byte[] data, string tags)
    {
        Success = success;
        Data = data;
        Tags = tags;
    }
}