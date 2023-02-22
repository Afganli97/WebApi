using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApi.Data.DAL;
using WebApi.DTOs.Category;
using WebApi.DTOs.Product;
using WebApi.Models;

namespace WebApi.Controllers;

[ApiController]
[Route("[controller]")]
public class ProductController : ControllerBase
{
    private readonly DataBase _context;
    public readonly IWebHostEnvironment _env;

    public ProductController(DataBase context, IWebHostEnvironment env)
    {
        _context = context;
        _env = env;
    }

    [HttpGet]
    [Route("{id}")]
    public IActionResult Get(int? id)
    {
        if (id == null) return NotFound("No Id");
        var productDto = _context.Products.Include(x=>x.Category).Where(x=>x.Id== id).Select(x => new ProductReturnDto{
            Name = x.Name,
            Price = x.Price,
            Discount = x.Discount,
            SalePrice = x.Price - (x.Price * x.Discount / 100),
            CategoryName = x.Category.Name,
            FullPath = "https://localhost:7038/" + x.ImageUrl
        }).FirstOrDefault();

        if (productDto == null) return NotFound("No product");
        
        return Ok(productDto);
    }
    
    [HttpGet]
    public IActionResult GetAll()
    {
        var products = _context.Products.Include(x=>x.Category).Select(x => new ProductReturnDto{
            Name = x.Name,
            Price = x.Price,
            Discount = x.Discount,
            SalePrice = x.Price - (x.Price * x.Discount / 100),
            CategoryName = x.Category.Name,
            FullPath = "https://localhost:7038/" + x.ImageUrl
        }).ToList();

        if (products.Count == 0) return NotFound("No Products");
        
        return Ok(products);
    }
    
    [HttpPost]
    public IActionResult Create(ProductCreateDto productDto)
    {
        if (productDto == null) return BadRequest("Not data to create!");

        Product product = new(){
            Name = productDto.Name,
            Price = productDto.Price,
            Discount = productDto.Discount,
            SalePrice = productDto.Price - (productDto.Price * productDto.Discount / 100),
            ImageUrl = "img/product/" + productDto.Photo.FileName,
            CategoryId = productDto.CategoryId
        };
        _context.Products.Add(product);
        _context.SaveChanges();
        
        return Ok("Product created!");
    }
    
    [HttpPut]
    public IActionResult Update(ProductUpdateDto productDto)
    {
        if (productDto == null) return BadRequest("Not data to update!");
        var existProduct = _context.Products.Find(productDto.Id);
        if (existProduct == null) return NotFound();
        
        existProduct.Name = productDto.Name;
        existProduct.Price = productDto.Price;
        existProduct.Discount = productDto.Discount;
        existProduct.CategoryId = productDto.CategoryId;
        _context.SaveChanges();
        
        return Ok("Product updated!");
    }
    
    [HttpPatch]
    public IActionResult UpdateStatus(int? id, bool status)
    {
        if (id == null) return NotFound();
        var product = _context.Products.Find(id);
        if (product == null) return NotFound();
        product.IsDeleted = status;
        _context.SaveChanges();
        
        return Ok("Product updated!");
    }

    [HttpDelete]
    public IActionResult Delete(int? id)
    {
        if (id == null) return NotFound();
        var product = _context.Products.Find(id);
        if (product == null) return NotFound();
        _context.Products.Remove(product);
        _context.SaveChanges();
        
        return Ok("Product deleted!");
    }
}