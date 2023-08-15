namespace Sunrise.Types;
//возможные состояния поста
public enum PostStatus
{
    //ждёт проверки от модерации, НЕ МОЖЕТ БЫТЬ ПОКАЗАН
    WaitForReview = 0,
    //ждёт проверки от модерации, может быть показан(с плашкой о том что пост не проверен лол)
    WaitForModerate = 1,
    //пост проверен и полностью готов к показу
    ReadyToShow = Int32.MaxValue
}