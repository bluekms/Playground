using StaticDataLibrary;

namespace ExcelToCsv.Test;

public sealed class CsvUtilityTest
{
    [Theory]
    [InlineData("\"버린 카드\"를 섞은 후 3장 뽑음. 그중 1장 가지고 나머지 카드 버림.", "\"버린 카드\"를 섞은 후 3장 뽑음. 그중 1장 가지고 나머지 카드 버림.")]
    [InlineData("덱에서 카드 3장 뽑음.", "덱에서 카드 3장 뽑음.")]
    [InlineData("이번 행동이 끝난 후, 당신은 원하는 \"행동 카드\"를 1에 놓을 수 있음.", "\"이번 행동이 끝난 후, 당신은 원하는 \"행동 카드\"를 1에 놓을 수 있음.\"")]
    public void ToCsvTest(string normalStr, string csvStr)
    {
        Assert.Equal(csvStr, CsvUtility.ToCsv(normalStr));
    }

    [Theory]
    [InlineData("\"버린 카드\"를 섞은 후 3장 뽑음. 그중 1장 가지고 나머지 카드 버림.", "\"버린 카드\"를 섞은 후 3장 뽑음. 그중 1장 가지고 나머지 카드 버림.")]
    [InlineData("덱에서 카드 3장 뽑음.", "덱에서 카드 3장 뽑음.")]
    [InlineData("이번 행동이 끝난 후, 당신은 원하는 \"행동 카드\"를 1에 놓을 수 있음.", "\"이번 행동이 끝난 후, 당신은 원하는 \"행동 카드\"를 1에 놓을 수 있음.\"")]
    public void ToNormalTest(string normalStr, string csvStr)
    {
        Assert.Equal(normalStr, CsvUtility.ToNormal(csvStr));
    }
}