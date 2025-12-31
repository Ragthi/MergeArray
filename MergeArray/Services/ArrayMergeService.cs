namespace MergeArraysApi.Services;

public sealed class ArrayMergeService : IArrayMergeService
{
    public int[] MergeSorted(int[] a, int[] b)
    {
        var result = new int[a.Length + b.Length];
        int i = 0, j = 0, k = 0;

        while (i < a.Length && j < b.Length)
        {
            if (a[i] <= b[j]) result[k++] =a[i++];
            else result[k++] = b[j++];
        }

        while (i < a.Length) result[k++] = a[i++];
        while (j < b.Length) result[k++] = b[j++];

        return result;
    }
}
