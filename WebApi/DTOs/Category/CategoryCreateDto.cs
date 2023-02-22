namespace WebApi.DTOs.Category;

public class CategoryCreateDto
{
    public string Name { get; set; }
    public IFormFile Photo { get; set; }
}