
using FoodLabelManager.API.DTOs;
using FoodLabelManager.API.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace FoodLabelManager.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class FoodLabelsController : ControllerBase
    {
        private readonly IFoodLabelService _foodLabelService;

        public FoodLabelsController(IFoodLabelService foodLabelService)
        {
            _foodLabelService = foodLabelService;
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> GetAll(
            [FromQuery] string? category,
            [FromQuery] string? search,
            [FromQuery] bool includeInactive = false,
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 10)
        {
            var foodLabels = await _foodLabelService.GetAllFoodLabelsAsync(category, search, includeInactive, page, pageSize);
            return Ok(foodLabels);
        }

        [HttpGet("{id}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetById(int id)
        {
            var foodLabel = await _foodLabelService.GetFoodLabelByIdAsync(id);
            if (foodLabel == null)
            {
                return NotFound();
            }
            return Ok(foodLabel);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateFoodLabelDto model)
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim == null || !int.TryParse(userIdClaim.Value, out int userId))
            {
                return Unauthorized("المستخدم غير مصادق.");
            }

            var foodLabel = await _foodLabelService.CreateFoodLabelAsync(model, userId);
            return CreatedAtAction(nameof(GetById), new { id = foodLabel.Id }, foodLabel);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateFoodLabelDto model)
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim == null || !int.TryParse(userIdClaim.Value, out int userId))
            {
                return Unauthorized("المستخدم غير مصادق.");
            }

            var foodLabel = await _foodLabelService.UpdateFoodLabelAsync(id, model, userId);
            if (foodLabel == null)
            {
                return NotFound();
            }
            return Ok(foodLabel);
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")] // Only admins can delete
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _foodLabelService.DeleteFoodLabelAsync(id);
            if (!result)
            {
                return NotFound();
            }
            return NoContent();
        }

        [HttpGet("categories")]
        [AllowAnonymous]
        public async Task<IActionResult> GetCategories()
        {
            var categories = await _foodLabelService.GetAllCategoriesAsync();
            return Ok(categories);
        }
    }
}

namespace FoodLabelManager.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class FoodLabelController : ControllerBase
    {
        private readonly IFoodLabelService _foodLabelService;

        public FoodLabelController(IFoodLabelService foodLabelService)
        {
            _foodLabelService = foodLabelService;
        }

        [HttpGet]
        [AllowAnonymous] // Allow unauthenticated access for listing food labels
        public async Task<IActionResult> GetAll()
        {
            var foodLabels = await _foodLabelService.GetAllFoodLabels();
            return Ok(foodLabels);
        }

        [HttpGet("{id}")]
        [AllowAnonymous] // Allow unauthenticated access for viewing single food label
        public async Task<IActionResult> GetById(int id)
        {
            var foodLabel = await _foodLabelService.GetFoodLabelById(id);
            if (foodLabel == null)
            {
                return NotFound();
            }
            return Ok(foodLabel);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")] // Only Admin can create
        public async Task<IActionResult> Create([FromBody] CreateFoodLabelDto dto)
        {
            var foodLabel = await _foodLabelService.CreateFoodLabel(dto);
            return CreatedAtAction(nameof(GetById), new { id = foodLabel.Id }, foodLabel);
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")] // Only Admin can update
        public async Task<IActionResult> Update(int id, [FromBody] UpdateFoodLabelDto dto)
        {
            if (id != dto.Id)
            {
                return BadRequest("ID mismatch");
            }
            var foodLabel = await _foodLabelService.UpdateFoodLabel(dto);
            if (foodLabel == null)
            {
                return NotFound();
            }
            return Ok(foodLabel);
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")] // Only Admin can delete
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _foodLabelService.DeleteFoodLabel(id);
            if (!result)
            {
                return NotFound();
            }
            return NoContent();
        }
    }
}


