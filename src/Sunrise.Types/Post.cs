namespace Sunrise.Types;

public class Post
{
    public Guid Id {get;private set;}
    public Guid AuthorId{get;private set;}
    public DateTime PostCreationTime {get;private set;}
    public List<Tag> Tags {get; set;}
    public FileInfo File {get;private set;}
    public PostStatus Status {get; set;}
    public Rating Rating {get;set;}
    private Post(){}

    public Post(Guid authorId, FileInfo fileId, PostStatus waitForReview = PostStatus.WaitForReview){
        PostCreationTime = DateTime.UtcNow;
        Tags = new List<Tag>();
        Id = Guid.NewGuid();
        this.AuthorId = authorId;
        File=fileId;
        Status = waitForReview;
        Rating = Rating.Unset;
    }
}

//представляет один обьект для ленты, содержит айди поста, ссылку на базовую версию и тип контента
public record class ScrollItem(Guid postId, string baseLink, int contentType);