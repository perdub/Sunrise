using System.Text;
namespace Sunrise.Types;
//в теории можно добавть кучу всего: аудио, текст, файлы и так далее и тому подобное
public enum ContentType
{
    Image = 0,
    Video = 1,
    Gif=2,
    Audio=3,
    Unknown = Int32.MaxValue
}
//класс который пытается определить тип загружаемого файла по заголовкам
public static class ContentTypeChecker
{
    public static ContentType CheckType(this byte[] arr)
    {
        //jpg/jpeg (header is 0xff d8)
        if(arr.Length>2 && arr[0]==0xff && arr[1]==0xd8){
            return ContentType.Image;
        }

        //png (header is 0x89 50 4e 47 0d 0a 1a 0a)
        if(arr.Length>8 && arr[0]==0x89 && arr[1]==0x50 && arr[2]==0x4e && arr[3]==0x47 && arr[4]==0x0d && arr[5]==0x0a && arr[6]==0x1a && arr[7]==0x0a){
            return ContentType.Image;
        }

        //RIFF media container  https://ru.wikipedia.org/wiki/RIFF (header 0x52 49 46 46)
        if(arr.Length > 12 && arr[0]==0x52 && arr[1]==0x49 && arr[2]==0x46 && arr[3]==0x46)
        {
            //webp (header is 0x 52 49 46 46 X X X X 57 45 42 50 where X X X X is file size)
            if(arr[8]==0x57 && arr[9]==0x45 && arr[10]==0x42 && arr[11]==0x50){
                return ContentType.Image;
            }

            //avi (header is 0x 52 49 46 46 X X X X 41 56 49 20 where X X X X is file size)
            if(arr[8]==0x41 && arr[9]==0x56 && arr[10]==0x49 && arr[11]==0x20){
                return ContentType.Video;
            }

            //wav (header is 0x 52 49 46 46 X X X X 57 41 56 45 where X X X X is file size)
            if(arr[8]==0x57 && arr[9]==0x41 && arr[10]==0x56 && arr[11]==0x45){
                return ContentType.Audio;
            }
        }


        //gif (header is 0x47 49 46 38 39 61 OR 0x47 49 46 38 37 61)
        if(arr.Length>6 && arr[0]==0x47 && arr[1]==0x49 && arr[2]==0x46 && arr[3]==0x38 && (arr[4]==0x39 || arr[4]==0x37) && arr[5]==0x61){
            return ContentType.Gif;
        }

        return ContentType.Unknown;
    }

    public static string TryGrabHeader(this byte[] arr, int headerSize = 16){
        StringBuilder sb = new StringBuilder();
        for(int i = 0; i<Math.Min(headerSize,arr.Length);i++){
            sb.Append(arr[i].ToString("X2"));
            sb.Append(' ');
        }
        return sb.ToString();
    }
}