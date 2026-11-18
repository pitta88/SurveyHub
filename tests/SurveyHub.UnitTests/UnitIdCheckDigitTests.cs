using System.Runtime.CompilerServices;
using SurveyHub.Domain.Institutions;

namespace SurveyHub.UnitTests;

public class UnitIdCheckDigitTests
{
    [Theory]
    [InlineData (10065, 100654)]
    [InlineData (10066, 100663)]
    public void Matches_real_ipeds_unit_ids(int baseNumber, int expctedUnitId)
    {
        Assert.Equal(expctedUnitId, UnitIdCheckDigit.Append(baseNumber));
        Assert.True(UnitIdCheckDigit.IsValid(expctedUnitId));
    }

    [Fact]
    public void Detects_wrong_check_digit()
    {
        var valid = UnitIdCheckDigit.Append(10065);

        var wrong = valid / 10 * 10 + (valid % 10 + 1) % 10;   // 체크 디지트만 바꿈

        Assert.False(UnitIdCheckDigit.IsValid(wrong));
    }

    [Fact]
    public void Detects_adjacent_transposition()
    {
        Assert.True(UnitIdCheckDigit.IsValid(734910));
        Assert.False(UnitIdCheckDigit.IsValid(374910));   // 앞 두 자리를 바꿈
    }

    [Fact]
    public void Known_limitation_digits_differing_by_five()
    {
        // 16000과 61000은 같은 체크 디지트 → 자리 바꿈을 못 잡는 경우
        Assert.Equal(UnitIdCheckDigit.Append(16000) % 10, UnitIdCheckDigit.Append(61000) % 10);
    }
    
}