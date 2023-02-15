using Microsoft.AspNetCore.Mvc;
using WebApi.Data.DAL;
using WebApi.Models;

namespace WebApi.Controllers;

[ApiController]
[Route("[controller]")]
public class ProductController : ControllerBase
{
    private readonly DataBase _context;

    public ProductController(DataBase context)
    {
        _context = context;
    }

    [HttpGet]
    [Route("{id}")]
    public IActionResult Get(int? id)
    {
        if (id == null) return NotFound();
        var product = _context.Products.FirstOrDefault(x=>x.Id== id);
        if (product == null) return NotFound();
        
        return Ok(product);
    }
    
    [HttpGet]
    public IActionResult GetAll()
    {
        var products = _context.Products.Where(x=>!x.IsDeleted).ToList();
        if (products.Count == 0) return NotFound();
        
        return Ok(products);
    }
    
    [HttpPost]
    public IActionResult Create(Product product)
    {
        if (product == null) return BadRequest("Not data to create!");
        _context.Products.Add(product);
        _context.SaveChanges();
        
        return Ok("Product created!");
    }
    
    [HttpPut]
    public IActionResult Update(Product product)
    {
        if (product == null) return BadRequest("Not data to update!");
        var existProduct = _context.Products.Find(product.Id);
        if (existProduct == null) return NotFound();
        
        existProduct.Name = product.Name;
        existProduct.CategoryId = product.CategoryId;
        existProduct.IsDeleted = product.IsDeleted;
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