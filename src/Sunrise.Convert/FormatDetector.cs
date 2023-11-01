using Sunrise.Types.Enums;

namespace Sunrise.Convert;

//этот класс проверяет массив байтов(файл) на соответствие какому-либо формату
public static class FormatDetector{
    public static Format GetFileFormat(this byte[] arr){
        //jpg
        if(arr.Length>2 && arr[0]==0xff && arr[1]==0xd8){
            return new Format(PostType.Image, "jpg");
        }

        //png (header is 0x89 50 4e 47 0d 0a 1a 0a)
        if(arr.Length>8 && arr[0]==0x89 && arr[1]==0x50 && arr[2]==0x4e && arr[3]==0x47 && arr[4]==0x0d && arr[5]==0x0a && arr[6]==0x1a && arr[7]==0x0a){
            return new Format(PostType.Image, "png");
        }

        //RIFF media container  https://ru.wikipedia.org/wiki/RIFF (header 0x52 49 46 46)
        if(arr.Length > 12 && arr[0]==0x52 && arr[1]==0x49 && arr[2]==0x46 && arr[3]==0x46)
        {
            //webp (header is 0x 52 49 46 46 X X X X 57 45 42 50 where X X X X is file size)
            if(arr[8]==0x57 && arr[9]==0x45 && arr[10]==0x42 && arr[11]==0x50){
                return new Format(PostType.Image, "webp");
            }

            //avi (header is 0x 52 49 46 46 X X X X 41 56 49 20 where X X X X is file size)
            if(arr[8]==0x41 && arr[9]==0x56 && arr[10]==0x49 && arr[11]==0x20){
                return new Format(PostType.Video, "avi");
            }

            //wav (header is 0x 52 49 46 46 X X X X 57 41 56 45 where X X X X is file size)
            if(arr[8]==0x57 && arr[9]==0x41 && arr[10]==0x56 && arr[11]==0x45){
                return new Format(PostType.Unknown, "wav");
            }
        }


        //gif (header is 0x47 49 46 38 39 61 OR 0x47 49 46 38 37 61)
        if(arr.Length>6 && arr[0]==0x47 && arr[1]==0x49 && arr[2]==0x46 && arr[3]==0x38 && (arr[4]==0x39 || arr[4]==0x37) && arr[5]==0x61){
            return new Format(PostType.Gif, "gif");
        }

        //mp4 (header is 0xX X X X 66 74 79 70 where X X X X is block offset)
        if(arr.Length>8 && arr[4]==0x66 && arr[5]==0x74 && arr[6]==0x79 && arr[7]==0x70){
            return new Format(PostType.Video, "mp4");
        }

        //matroska media container(mkv, webm, mka, mks, mk3d)
        //header is 0x1A 45 DF A3
        if(arr.Length>4 && arr[0]==0x1A && arr[1]==0x45 && arr[2]==0xDF && arr[3]==0xA3){
            return new Format(PostType.Video, "mkv");
        }

        return new Format(PostType.Unknown, "");
    }
}