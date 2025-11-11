using AutoMapper;
using EcommerceWarehouse.Server.DTOs;
using EcommerceWarehouse.Server.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EcommerceWarehouse.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Policy = "AdminOnly")]
    public class CategoriesController : ControllerBase
    {
        private readonly ICategoryService _service;
        private readonly ICategoryQueryService _queryService;
        public CategoriesController(ICategoryService service, ICategoryQueryService queryService)
        {
            _service = service; _queryService = queryService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] BasicQuery q)
        {
            if (q.PageSize > 0) return Ok(await _queryService.QueryAsync(q));
            var result = await _service.GetAllAsync();
            return Ok(result);
        }

        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var dto = await _service.GetByIdAsync(id);
            return dto == null ? NotFound() : Ok(dto);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateCategoryRequest request)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var created = await _service.CreateAsync(request);
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
        }

        [HttpPut("{id:guid}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] CreateCategoryRequest request)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var ok = await _service.UpdateAsync(id, request);
            return ok ? NoContent() : NotFound();
        }

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var ok = await _service.DeleteAsync(id);
            return ok ? NoContent() : NotFound();
        }
    }
}
