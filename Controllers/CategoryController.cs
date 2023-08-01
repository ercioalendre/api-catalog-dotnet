using APICatalog.DTOs;
using APICatalog.Models;
using APICatalog.Pagination;
using APICatalog.Repository;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace APICatalog.Controllers
{
    [Authorize(AuthenticationSchemes = "Bearer")]
    [Route("category")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;

        private readonly IMapper _mapper;

        public CategoryController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;

            _mapper = mapper;
        }

        [HttpPost("create-one")]
        public async Task<ActionResult<CategoryOutputDTO>> CreateOne(CategoryInputDTO categoryInput)
        {
            var category = _mapper.Map<CategoryModel>(categoryInput);

            category.Create(Guid.NewGuid());

            _unitOfWork.CategoryRepository.CreateOne(category);

            await _unitOfWork.CommitAsync();

            return _mapper.Map<CategoryOutputDTO>(category);
        }

        [HttpGet("list-one/{id}")]
        public async Task<ActionResult<CategoryOutputDTO>> ListOne(Guid id)
        {
            var category = await _unitOfWork.CategoryRepository.GetOneByIdAsync(p => p.Id == id);

            if (category == null)
            {
                return NotFound(new
                {
                    message = "Category not found."
                });
            }
            else
            {
                return Ok(_mapper.Map<CategoryOutputDTO>(category));
            }
        }

        [HttpGet("list-many")]
        public async Task<ActionResult<IEnumerable<CategoryOutputDTO>>> ListMany([FromQuery] QueryStringParameters queryStringParameters)
        {
            var categoryList = await _unitOfWork.CategoryRepository.GetManyAsync(queryStringParameters);

            var metadata = new
            {
                categoryList.TotalCount,
                categoryList.PageSize,
                categoryList.CurrentPage,
                categoryList.TotalPages,
                categoryList.HasNext,
                categoryList.HasPrevious,
            };

            Response.Headers.Add("X-Pagination", JsonSerializer.Serialize(metadata));

            return Ok(_mapper.Map<IEnumerable<CategoryOutputDTO>>(categoryList));
        }

        [HttpPut("update-one/{id}")]
        public async Task<ActionResult<CategoryOutputDTO>> UpdateOneAsync(Guid id, CategoryInputDTO categoryInput)
        {
            var category = _mapper.Map<CategoryModel>(categoryInput);

            category.Id = id;

            category.Update(Guid.NewGuid());

            _unitOfWork.CategoryRepository.UpdateOne(category);

            await _unitOfWork.CommitAsync();

            return Ok(_mapper.Map<CategoryOutputDTO>(category));
        }

        [HttpDelete("delete-one/{id}")]
        public async Task<ActionResult<object>> DeleteOne(Guid id)
        {
            var category = await _unitOfWork.CategoryRepository.GetOneByIdAsync(p => p.Id == id);

            if (category == null)
            {
                return NotFound(new
                {
                    message = "Category not found."
                });
            }
            else
            {
                _unitOfWork.CategoryRepository.DeleteOne(category);

                await _unitOfWork.CommitAsync();

                return Ok(new
                {
                    message = "Category deleted successfully!"
                });
            }
        }
    }
}
