using Microsoft.EntityFrameworkCore;
using MergeArraysApi.Models;

namespace MergeArraysApi.Data;

public class MergeDbContext : DbContext
{
    public MergeDbContext(DbContextOptions<MergeDbContext> options) : base(options) { }

    public DbSet<MergeOperation> MergeOperations => Set<MergeOperation>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        var op = modelBuilder.Entity<MergeOperation>();
        op.HasKey(x => x.Id);
        op.Property(x => x.Array1Json).IsRequired();
        op.Property(x => x.Array2Json).IsRequired();
        op.Property(x => x.ResultJson).IsRequired();
        op.Property(x => x.CreatedAtUtc).IsRequired();
    }
}
