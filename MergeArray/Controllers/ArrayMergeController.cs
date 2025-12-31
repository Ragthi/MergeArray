using MergeArray.DTOs;
using MergeArraysApi.Data;
using MergeArraysApi.Models;
using MergeArraysApi.Services;
using MergeArraysApi.Validation;
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
            var errors = new List<string>();
            errors.AddRange(SortedArrayValidator.ValidateSortedAscending(request.Array1, nameof(request.Array1)));
            errors.AddRange(SortedArrayValidator.ValidateSortedAscending(request.Array2, nameof(request.Array2)));

            if (errors.Count > 0)
                return BadRequest(new { message = "Validation failed.", errors });

            var merged = _merger.MergeSorted(request.Array1, request.Array2);

            var op = new MergeOperation
            {
                Id = Guid.NewGuid(),
                CreatedAtUtc = DateTime.UtcNow,
                Array1Json = JsonSerializer.Serialize(request.Array1),
                Array2Json = JsonSerializer.Serialize(request.Array2),
                ResultJson = JsonSerializer.Serialize(merged),
                ResultLength = merged.Length
            };

            _db.MergeOperations.Add(op);
            await _db.SaveChangesAsync();

            var dto = new MergeOperationDto { Id = op.Id, CreatedAtUtc = op.CreatedAtUtc, Array1 = request.Array1, Array2 = request.Array2, Result = merged, ResultLength = merged.Length };
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
                Result = JsonSerializer.Deserialize<int[]>(op.ResultJson) ?? Array.Empty<int>(),
                ResultLength = op.ResultLength
            };

            return Ok(dto);
        }

        [HttpGet("by-result-length/{length:int}")]
        [ProducesResponseType(typeof(object), 200)]
        public async Task<IActionResult> GetByResultLength([FromRoute] int length, [FromQuery] int? skip, [FromQuery] int? take)
        {
            var s = Math.Max(skip ?? 0, 0);
            var t = Math.Clamp(take ?? 50, 1, 200);

            var ops = await _db.MergeOperations
                .AsNoTracking()
                .Where(x => x.ResultLength == length)
                .OrderByDescending(x => x.CreatedAtUtc)
                .Skip(s)
                .Take(t)
                .ToListAsync();

            var items = ops.Select(op => new MergeOperationDto
            {
                Id = op.Id,
                CreatedAtUtc = op.CreatedAtUtc,
                Array1 = JsonSerializer.Deserialize<int[]>(op.Array1Json) ?? Array.Empty<int>(),
                Array2 = JsonSerializer.Deserialize<int[]>(op.Array2Json) ?? Array.Empty<int>(),
                Result = JsonSerializer.Deserialize<int[]>(op.ResultJson) ?? Array.Empty<int>(),
                ResultLength = op.ResultLength
            }).ToList();

            return Ok(new
            {
                length,
                skip = s,
                take = t,
                count = items.Count,
                items
            });
        }
    }
}
