using System.ComponentModel.DataAnnotations;
using System.Text;
using Microsoft.EntityFrameworkCore;

namespace StaticDataLibrary.ValidationLibrary;

public static class RangeChecker
{
    public static async Task CheckAsync<T>(string staticDataPath, StringBuilder? errors = null) where T : DbContext
    {
        await ValidationAttributeChecker<T, RangeAttribute>.CheckAsync(
            staticDataPath,
            (attribute, value, location) =>
            {
                var message = string.Empty;
                
                if (value.GetType() == typeof(DateTime))
                {
                    var val = (DateTime)value;
                    var min = (DateTime)attribute.Minimum;
                    var max = (DateTime)attribute.Maximum;

                    message = $"Range Error. {location}({val.ToString("s")}) 반드시 {min.ToString("s")} ~ {max.ToString("s")} 사이여야 합니다.";
                }
                else
                {
                    message = $"Range Error. {location}({value}) 반드시 {attribute.Minimum} ~ {attribute.Maximum} 사이여야 합니다.";
                }
                
                if (errors is null)
                {
                    throw new ValidationException(message);
                }
                else
                {
                    errors.AppendLine(message);
                }
            });
    }
}