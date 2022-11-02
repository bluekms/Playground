using System.ComponentModel.DataAnnotations;
using System.Text;
using Microsoft.EntityFrameworkCore;

namespace StaticDataLibrary.ValidationLibrary;

public static class RegexChecker
{
    public static async Task CheckAsync<T>(string staticDataPath, StringBuilder? errors = null) where T : DbContext
    {
        await ValidationAttributeChecker<T, RegularExpressionAttribute>.CheckAsync(
            staticDataPath,
            (_, value, location) =>
            {
                var message = $"Regex Error. {location}({value}) 패턴이 맞지 않습니다.";
                if (errors is null)
                {
                    throw new ValidationException(message);
                }
                else
                {
                    errors.AppendLine($"Regex Error. {location}({value}) 패턴이 맞지 않습니다.");
                }
            });
    }
}