namespace Sunrise;
//этот класс содержит все константы в приложении для того что бы их менять было удобней
public static class Constants
{
    //название куки для сохранения сессии
    public const string SESSION_COOKIE_NAME = "suntoken";
    //количество постов на одной странице
    public const int POST_PER_PAGE = 50;
    //размер большей стороны превью изображения
    public const int PREVIEW_SIZE = 200;
    //потоков для ffmpeg
    public const int FFMPEG_THREADS_COUNT = 2;

    
}