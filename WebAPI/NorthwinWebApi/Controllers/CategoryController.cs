using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Northwind.Contracts;
using System;

namespace NorthwinWebApi.Controllers
{
    [Route("api/categories")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly IRepositoryManager _repository;
        private readonly ILoggerManager _logger;

        public CategoryController(IRepositoryManager repository, ILoggerManager logger)
        {
            _repository = repository;
            _logger = logger;
        }

        [HttpGet]
        public IActionResult GetCategories()
        {
            try
            {
                var categories = _repository.Category.GetAllCategory(trackChanges: false);
                return Ok(categories);
            }
            catch (Exception ex)
            {
                _logger.LogError($"{nameof(GetCategories)} message : {ex}");
                return StatusCode(500, "Internal Server Error");
            }
        }
    }
}
