namespace Sunrise.Storage.Items;

public class ImageItem : Item
{
    MemoryStream previewImage, baseImage, originalImage;
    bool previewReady = false, baseReady=false, originalReady=false;
    public MemoryStream Preview{
        get{
            while(!previewReady){
                Task.Delay(50).Wait();
            }
            return previewImage;
        }
    }
    public MemoryStream Base{
        get{
            while(!baseReady){
                Task.Delay(50).Wait();
            }
            return baseImage;
        }
    }
    public MemoryStream Original{
        get{
            while(!originalReady){
                Task.Delay(50).Wait();
            }
            return originalImage;
        }
    }
    public override async Task Load(Guid id, Storage source)
    {
        #pragma warning disable CS4014
        Task.Run(async ()=>{
            originalImage = await source.LoadFile(id, "original");
            originalReady = true;
        });
        Task.Run(async ()=>{
            baseImage = await source.LoadFile(id, "base");
            baseReady = true;
        });
        await Task.Run(async ()=>{
            previewImage = await source.LoadFile(id, "preview");
            previewReady = true;
        });
        
    }
    
}