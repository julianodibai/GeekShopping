using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProductAPI.Infra.Repository;
using ProductAPI.Services.DTOs;
using ProductAPI.Services.Utils;

namespace ProductAPI.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private IProductRepository _repository;

        public ProductController(IProductRepository repository)
        {
            _repository = repository ?? throw new
                ArgumentNullException(nameof(repository));
        }

        [Authorize]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProductDTO>>> FindAll()
        {
            var products = await _repository.FindAll();

            return Ok(products);
        }

        [Authorize]
        [HttpGet("{id}")]
        public async Task<ActionResult<ProductDTO>> FindById(long id)
        {
            var product = await _repository.FindById(id);

            if (product == null)
                return NotFound();

            return Ok(product);
        }

        [Authorize]
        [HttpPost]
        public async Task<ActionResult<ProductAddDTO>> Create([FromBody] ProductAddDTO dto)
        {
            if (dto == null)
                return BadRequest();

            var product = await _repository.Create(dto);

            return Ok(product);
        }

        [Authorize]
        [HttpPut]
        public async Task<ActionResult<ProductDTO>> Update([FromBody] ProductDTO dto)
        {
            if (dto == null)
                return BadRequest();

            var product = await _repository.Update(dto);

            return Ok(product);
        }

        [Authorize(Roles = Role.Admin)]
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(long id)
        {
            var status = await _repository.Delete(id);

            if (!status)
                return BadRequest();

            return Ok(status);
        }
    }
}