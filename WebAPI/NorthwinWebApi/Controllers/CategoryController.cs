using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Northwind.Contracts;
using System;
using System.Linq;
using Northwind.Entities.DataTransferObject;
using AutoMapper;
using System.Collections.Generic;
using Northwind.Entities.Models;
using System.Threading.Tasks;
using System.IO;

namespace NorthwinWebApi.Controllers
{
    [Route("api/categories")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly IRepositoryManager _repository;
        private readonly ILoggerManager _logger;
        private readonly IMapper _mapper;

        public CategoryController(IRepositoryManager repository, ILoggerManager logger, IMapper mapper)
        {
            _repository = repository;
            _logger = logger;
            _mapper = mapper;
        }

        [HttpGet]
        public IActionResult GetCategories()
        {

            var categories = _repository.Category.GetAllCategory(trackChanges: false);
            var categoryDto = _mapper.Map<IEnumerable<CategoryDto>>(categories);

            //only test for simulate error, when succeed, please remove
            //throw new Exception("Exception");

            return Ok(categoryDto);

     
 

            /* try
            {
                var categories = _repository.Category.GetAllCategory(trackChanges: false);

                // replace by categoryDto
                *//*var categoryDto = categories.Select(c => new CategoryDto
                {
                    Id = c.CategoryId,
                    categoryName = c.CategoryName,
                    description = c.Description
                }).ToList();*//*

                var categoryDto = _mapper.Map<IEnumerable<CategoryDto>>(categories);

                return Ok(categoryDto);
            }
            catch (Exception ex)
            {
                _logger.LogError($"{nameof(GetCategories)} message : {ex}");
                return StatusCode(500, "Internal Server Error");
            }*/
        }

        [HttpGet("{id}",Name = "CategoryById")]
        public IActionResult GetCategory(int id)
        {
            var category = _repository.Category.GetCategory(id, trackChanges: false);
            if (category == null)
            {
                _logger.LogInfo($"Category with Id : {id} doesn't exist");
                return NotFound();
            }
            else
            {
                var categoryDto = _mapper.Map<CategoryDto>(category);
                return Ok(categoryDto);
            }
        }

        [HttpPost]
        public IActionResult CreateCategory([FromBody] CategoryDto categoryDto)
        {
            if (categoryDto == null)
            {
                _logger.LogError("Category object is null");
                return BadRequest("Category object is null");
            }

            var categoryEntity = _mapper.Map<Category>(categoryDto);
            _repository.Category.CreateCategory(categoryEntity);
            _repository.Save();

            var categoryResult = _mapper.Map<CategoryDto>(categoryEntity);

            return CreatedAtRoute("CategoryById", new { id = categoryResult.categoryId }, categoryResult);


        }

        [HttpDelete("{id}")]
        public IActionResult DeleteCategory(int id)
        {
            var category = _repository.Category.GetCategory(id, trackChanges: false);
            if(category == null)
            {
                _logger.LogInfo($"Category with Id : {id} not found");
                return NotFound();
            }

            _repository.Category.DeleteCategory(category);
            _repository.Save();

            return NoContent();
        }

        [HttpPut("{id}")]
        public IActionResult UpdateCategory(int id, [FromBody] CategoryDto categoryDto)
        {
            if (categoryDto == null)
            {
                _logger.LogError($"Category must not be null");
                return BadRequest("Category must not be null");
            }

            //find category by id
            var categoryEntity = _repository.Category.GetCategory(id, trackChanges: true);
            if(categoryEntity == null)
            {
                _logger.LogInfo($"Category with id : {id} not found");
                return NotFound();
            }

            _mapper.Map(categoryDto, categoryEntity);
           // _repository.Category.UpdateCategory(categoryEntity);

            _repository.Save();
            return NoContent();
        }


        [HttpPost("form"), DisableRequestSizeLimit]
        public async Task<IActionResult> PostCategory()
        {
            //use try-catch to prevent error when read Request.ReadFormAsync()
            
            try
            {
                // hold data multipart from client-side
                var formCollection = await Request.ReadFormAsync();
                // hold data File
                var file = formCollection.Files.First();

                // hold data attribute
                formCollection.TryGetValue("Description", out var description).ToString();
                formCollection.TryGetValue("CategoryName", out var categoryName).ToString();

                // use object MemoryStream to hold stream binary
                using (var memoryStream = new MemoryStream())
                {
                    //convert file stream to object MemoryStream
                    await file.CopyToAsync(memoryStream);

                    // hold file stream & attribute in CategoryDto
                    var categoryDto = new CategoryDto()
                    {
                        categoryName = categoryName,
                        description = description,
                        Picture = memoryStream.ToArray()
                    };

                    //map categoryDto and CategoryModel
                    var categoryEntity = _mapper.Map<Category>(categoryDto);
                    _repository.Category.CreateCategory(categoryEntity);
                    _repository.Save();

                    // we want result category post then send to client side
                    var categoryToReturn = _mapper.Map<CategoryDto>(categoryEntity);
                    return CreatedAtRoute("CategoryById", new { id = categoryToReturn.categoryId }, categoryToReturn);

                }

            }
            catch (Exception ex)
            {
                return BadRequest($"Content-Type is null {ex}");
            }

        }

        [HttpGet("getPhoto/{id}"), DisableRequestSizeLimit]
        public async Task<IActionResult> GetPhoto(int id)
        {
            var categoryEntity = await _repository.Category.GetCategoryAsycn(id, trackChanges: false);
            if (categoryEntity == null)
            {
                _logger.LogInfo($"Category with id :{id} doesn't exist in the database");
                return NotFound();
            }

            byte[] picture = categoryEntity.Picture;
            return base.File(picture, "image/jpeg");
        }
    }
}
