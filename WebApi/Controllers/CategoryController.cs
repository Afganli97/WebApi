using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WebApi.Data.DAL;
using WebApi.DTOs.Category;
using WebApi.Models;

namespace WebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CategoryController : ControllerBase
    {
        private readonly DataBase _context;

        public CategoryController(DataBase context)
        {
            _context = context;
        }

        [HttpGet]
        [Route("{id}")]
        public IActionResult Get(int? id)
        {
            if (id == null) return NotFound();
            var category = _context.Categories.Where(x=>x.Id == id).Select(x=>new CategoryGetDto
            {
                Name = x.Name,
                FullPath = "https://localhost:7038/wwwroot/img/2.jpg"
            }).FirstOrDefault();
            if (category == null) return NotFound();
            
            return Ok(category);
        }
        
        [HttpGet]
        public IActionResult GetAll()
        {
            var categories = _context.Categories.Select(x=>new CategoryGetDto
            {
                Name = x.Name
            }).ToList();
            if (categories.Count == 0) return NotFound();
            
            return Ok(categories);
        }
        
        [HttpPost]
        public IActionResult Create(Category category)
        {
            if (category == null) return BadRequest("Not data to create!");
            _context.Categories.Add(category);
            _context.SaveChanges();
            
            return Ok("Category created!");
        }
        
        [HttpPut]
        public IActionResult Update(Category category)
        {
            if (category == null) return BadRequest("Not data to update!");
            var existCategory = _context.Categories.Find(category.Id);
            if (existCategory == null) return NotFound();
            
            existCategory.Name = category.Name;
            existCategory.IsDeleted = category.IsDeleted;
            
            _context.SaveChanges();
            
            return Ok("Category updated!");
        }
        
        [HttpPatch]
        public IActionResult UpdateStatus(int? id, bool status)
        {
            if (id == null) return NotFound();
            var category = _context.Categories.Find(id);
            if (category == null) return NotFound();
            category.IsDeleted = status;
            _context.SaveChanges();
            
            return Ok("Category updated!");
        }

        [HttpDelete]
        public IActionResult Delete(int? id)
        {
            if (id == null) return NotFound();
            var category = _context.Categories.Find(id);
            if (category == null) return NotFound();
            _context.Categories.Remove(category);
            _context.SaveChanges();
            
            return Ok("Category deleted!");
        }
    }
}