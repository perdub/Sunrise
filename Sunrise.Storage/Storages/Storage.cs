namespace Sunrise.Storage;
using Sunrise.Storage.Types;
//базовый класс для хранилища
public abstract class Storage
{
    public virtual void Initialize()
    {
        Sunrise.Logger.Logger.Singelton.Write("New storage initialize.");
    }
    public abstract Task<string> SaveAsync(Guid id, byte[] f, string fileName);
    public abstract Task<MemoryStream> LoadFile(Guid id, string fileName);
}