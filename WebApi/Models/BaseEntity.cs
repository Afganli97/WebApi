namespace WebApi.Models;

public class BaseEntity
{
    public BaseEntity()
    {
        CreatedTime = DateTime.Now;
        LastModifiedTime = DateTime.Now;
    }
    public bool IsDeleted { get; set; }
    public DateTime CreatedTime { get; set; }
    public DateTime LastModifiedTime { get; set; }
    public DateTime DeletedTime { get; set; }
}