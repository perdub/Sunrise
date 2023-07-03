namespace Sunrise.Storage;

public class Server
{
    #region Singelton
    static Server _singelton;
    public static Server Singelton {get{return _singelton;}}
    
    public Server()
    {
        _singelton = this;
    }
#endregion
    public List<Storage> Storages {get;private set;}= new List<Storage>();

    public Server(params Storage[] storage) : this()
    {
        Storages = storage.ToList();
    }

    public async Task<Types.FileInfo> Save(Guid id, byte[] f, string fileName)
    {
        Types.FileInfo res = new Types.FileInfo();
        res.Id = id;
        res.Paths = new string[Storages.Count];
        for(int i = 0; i< res.Paths.Length; i++){
            res.Paths[i] = await Storages[i].SaveAsync(id, f, fileName);
        }
        return res;
    }

    public IEnumerator<Item> GetItems(Guid id){
        yield return new ImageItem();
    }
}
