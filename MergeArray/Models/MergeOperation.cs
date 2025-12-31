using System;

namespace MergeArraysApi.Models;

public class MergeOperation
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public DateTime CreatedAtUtc { get; set; } = DateTime.UtcNow;

    public string Array1Json { get; set; } = "[]";
    public string Array2Json { get; set; } = "[]";
    public string ResultJson { get; set; } = "[]";
}
