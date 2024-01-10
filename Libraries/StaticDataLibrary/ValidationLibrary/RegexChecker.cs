using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace StaticDataLibrary.ValidationLibrary;

public static class RegexChecker
{
    public static async Task CheckAsync<T>(string staticDataPath, List<string>? errors = null)
        where T : DbContext
    {
        var checker = new ValidationAttributeChecker<T, RegularExpressionAttribute>();
        await checker.CheckAsync(
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
                    errors.Add(message);
                }
            });
    }
}
