using MemoryPack;

namespace Sunrise.Storage;
//структура которая описывает обьект в contentserver
[MemoryPackable]
public struct Item
{
    public Sunrise.Types.ContentType content = Sunrise.Types.ContentType.Unknown;
    public Guid Id = Guid.Empty;
    public bool IsProcessed = false;
    public Item()
    {
        
    }
}