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
    [Route("product")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;

        private readonly IMapper _mapper;

        public ProductController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;

            _mapper = mapper;
        }

        [HttpPost("create-one")]
        public async Task<ActionResult<ProductOutputDTO>> CreateOne(ProductInputDTO productInput)
        {
            var product = _mapper.Map<ProductModel>(productInput);

            product.Create(Guid.NewGuid());

            _unitOfWork.ProductRepository.CreateOne(product);

            await _unitOfWork.CommitAsync();

            return _mapper.Map<ProductOutputDTO>(product);
        }

        [HttpGet("list-one/{id}")]
        public async Task<ActionResult<ProductOutputDTO>> ListOne(Guid id)
        {
            var product = await _unitOfWork.ProductRepository.GetOneByIdAsync(p => p.Id == id);

            if (product == null)
            {
                return NotFound(new
                {
                    message = "Product not found."
                });
            }
            else
            {
                return Ok(_mapper.Map<ProductOutputDTO>(product));
            }
        }

        [HttpGet("list-many")]
        public async Task<ActionResult<IEnumerable<ProductOutputDTO>>> ListMany([FromQuery] QueryStringParameters queryStringParameters)
        {
            var productList = await _unitOfWork.ProductRepository.GetManyAsync(queryStringParameters);

            var metadata = new
            {
                productList.TotalCount,
                productList.PageSize,
                productList.CurrentPage,
                productList.TotalPages,
                productList.HasNext,
                productList.HasPrevious,
            };

            Response.Headers.Add("X-Pagination", JsonSerializer.Serialize(metadata));

            return Ok(_mapper.Map<IEnumerable<ProductOutputDTO>>(productList));
        }

        [HttpPut("update-one/{id}")]
        public async Task<ActionResult<ProductOutputDTO>> UpdateOneAsync(Guid id, ProductInputDTO productInput)
        {
            var product = _mapper.Map<ProductModel>(productInput);

            product.Id = id;

            product.Update(Guid.NewGuid());

            _unitOfWork.ProductRepository.UpdateOne(product);

            await _unitOfWork.CommitAsync();

            return Ok(_mapper.Map<ProductOutputDTO>(product));
        }

        [HttpDelete("delete-one/{id}")]
        public async Task<ActionResult<object>> DeleteOne(Guid id)
        {
            var product = await _unitOfWork.ProductRepository.GetOneByIdAsync(p => p.Id == id);

            if (product == null)
            {
                return NotFound(new
                {
                    message = "Product not found."
                });
            }
            else
            {
                _unitOfWork.ProductRepository.DeleteOne(product);

                await _unitOfWork.CommitAsync();

                return Ok(new
                {
                    message = "Product deleted successfully!"
                });
            }
        }
    }
}
