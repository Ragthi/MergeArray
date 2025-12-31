namespace MergeArray.DTOs
{
    public class MergeOperationDto
    {
        public Guid Id { get; set; }
        public DateTime CreatedAtUtc { get; set; }

        public int[] Array1 { get; set; } = Array.Empty<int>();
        public int[] Array2 { get; set; } = Array.Empty<int>();

        public int[] Result { get; set; } = Array.Empty<int>();
    }
}
