using MergeArray.DTOs;
using MergeArraysApi.Data;
using MergeArraysApi.Models;
using MergeArraysApi.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

namespace MergeArray.Controllers
{
    [ApiController]
    [Route("api/merge")]
    public class ArrayMergeController : ControllerBase
    {
        private readonly ILogger<ArrayMergeController> _logger;
        private readonly IArrayMergeService _merger;
        private readonly MergeDbContext _db;

        public ArrayMergeController(ILogger<ArrayMergeController> logger, IArrayMergeService merge, MergeDbContext db)
        {
            _logger = logger;
            _merger = merge;
            _db = db;
        }

        [HttpPost]
        [ProducesResponseType(typeof(MergeOperationDto), 201)]
        [ProducesResponseType(typeof(object), 400)]
        public async Task<IActionResult> Merge([FromBody] MergeRequestDto request)
        {

            var merged = _merger.MergeSorted(request.Array1, request.Array2);

            var op = new MergeOperation
            {
                Id = Guid.NewGuid(),
                CreatedAtUtc = DateTime.UtcNow,
                Array1Json = JsonSerializer.Serialize(request.Array1),
                Array2Json = JsonSerializer.Serialize(request.Array2),
                ResultJson = JsonSerializer.Serialize(merged)
            };

            _db.MergeOperations.Add(op);
            await _db.SaveChangesAsync();

            var dto = new MergeOperationDto { Id = op.Id, CreatedAtUtc = op.CreatedAtUtc, Array1 = request.Array1, Array2 = request.Array2, Result = merged };
            return CreatedAtAction(nameof(GetById), new { id = dto.Id }, dto);
        }

        /// <summary>
        /// Fetches a stored merge operation by id.
        /// </summary>
        [HttpGet("{id:guid}")]
        [ProducesResponseType(typeof(MergeOperationDto), 200)]
        [ProducesResponseType(typeof(object), 404)]
        public async Task<IActionResult> GetById([FromRoute] Guid id)
        {
            var op = await _db.MergeOperations.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);
            if (op is null) return NotFound(new { message = "Not found." });

            var dto = new MergeOperationDto
            {
                Id = op.Id,
                CreatedAtUtc = op.CreatedAtUtc,
                Array1 = JsonSerializer.Deserialize<int[]>(op.Array1Json) ?? Array.Empty<int>(),
                Array2 = JsonSerializer.Deserialize<int[]>(op.Array2Json) ?? Array.Empty<int>(),
                Result = JsonSerializer.Deserialize<int[]>(op.ResultJson) ?? Array.Empty<int>()
            };

            return Ok(dto);
        }
    }
}
