namespace Sunrise.Storage.Items;

public abstract class Item
{
    string path;
    public abstract Task Load(Guid id, Storage source);
}