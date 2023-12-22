namespace Sunrise.Types.Enums;

//перечисление возможного типа содержания поста
public enum PostType: Int32{
    Image = 0,
    Video = 1,
    Gif = 2,

    Unknown = Int32.MaxValue
}