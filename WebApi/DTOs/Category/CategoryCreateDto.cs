namespace WebApi.DTOs.Category;

public class CategoryCreateDto
{
    public string Name { get; set; }
    public List<IFormFile> Photos { get; set; }
}