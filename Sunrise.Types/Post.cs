namespace Sunrise.Types;

public class Post
{
    public Guid Id {get;private set;}
    public Guid AuthorId{get;private set;}
    public DateTime PostCreationTime {get;private set;}
    public List<Tag> Tags {get;private set;}
    public Guid FileId {get;private set;}
    public bool WaitForReview {get;private set;}
    private Post(){}

    public Post(Guid authorId, Guid fileId){
        PostCreationTime = DateTime.UtcNow;
        Tags = new List<Tag>();
        Id = Guid.NewGuid();
        this.AuthorId = authorId;
        FileId=fileId;
        WaitForReview = true;
    }
}