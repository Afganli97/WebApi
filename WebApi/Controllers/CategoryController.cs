using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WebApi.Data.DAL;
using WebApi.DTOs.Category;
using WebApi.Helpers.Extensions;
using WebApi.Models;

namespace WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CategoryController : ControllerBase
    {
        private readonly DataBase _context;
        public readonly IWebHostEnvironment _env;

        public CategoryController(DataBase context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }

        [HttpGet]
        [Route("{id}")]
        public IActionResult Get(int? id)
        {
            if (id == null) return NotFound("Id was not found!");
            var category = _context.Categories.Where(x=>x.Id == id).Select(x=>new CategoryReturnDto
            {
                Name = x.Name,
                FullPath = "https://localhost:7038/" + x.ImageUrl
            }).FirstOrDefault();
            if (category == null) return NotFound("Category was not found!");
            
            return Ok(category);
        }
        
        [HttpGet]
        public IActionResult GetAll()
        {
            var categories = _context.Categories.Select(x=>new CategoryReturnDto
            {
                Name = x.Name,
                FullPath = "https://localhost:7038/" + x.ImageUrl
            }).ToList();
            if (categories.Count == 0) return NotFound("No Category to return!");
            
            return Ok(categories);
        }
        
        [HttpPost]
        public IActionResult Create([FromForm]CategoryCreateDto categoryDto)
        {
            if (categoryDto == null) return BadRequest("Not data to create!");

            if(!categoryDto.Photo.CheckFile("image")) return BadRequest("Select Photo");

            if(categoryDto.Photo.CheckFileLength(1000)) return BadRequest("Selected photo length is so much");

            categoryDto.Photo.SaveFile(_env, "img/category");

            Category category =new(){
                Name = categoryDto.Name,
                ImageUrl = "img/category/" + categoryDto.Photo.FileName
            };
            _context.Categories.Add(category);
            _context.SaveChanges();
            
            return Ok("Category created!");
        }
        
        [HttpPut]
        public IActionResult Update(CategoryUpdateDto categoryDto)
        {
            if (categoryDto == null) return BadRequest("Not data to update!");
            var existCategory = _context.Categories.Find(categoryDto.Id);
            if (existCategory == null) return NotFound();
            
            existCategory.Name = categoryDto.Name;
            
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