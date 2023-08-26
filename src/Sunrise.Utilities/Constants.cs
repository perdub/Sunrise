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
    //количество постов для подзагрузки ленты с постами
    public const int POST_PER_SCROLL = 5;
    //сколько символов будет убиратся из начала имени файла для соответствия структуре файловой системы
    public const int FILE_NAME_SYMBOL_SLICE_COUNT = 4;
}