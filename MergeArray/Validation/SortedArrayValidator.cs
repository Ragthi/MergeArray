using System.Collections.Generic;

namespace MergeArraysApi.Validation;

public static class SortedArrayValidator
{
    public static List<string> ValidateSortedAscending(int[]? arr, string paramName)
    {
        var errors = new List<string>();

        if (arr is null)
        {
            errors.Add($"{paramName} must not be null.");
            return errors;
        }

        for (int i = 1; i < arr.Length; i++)
        {
            if (arr[i] < arr[i - 1])
            {
                errors.Add($"{paramName} must be sorted ascending.");
                break;
            }
        }

        return errors;
    }
}
