using SurveyHub.Domain.Institutions;

namespace SurveyHub.UnitTests.Domain;

public class InstitutionTests
{
    private static readonly int validUnitId = UnitIdCheckDigit.Append(10000);

    [Theory]
    [InlineData("MD")]
    [InlineData("VA")]
    [InlineData("DC")]
    public void Accepts_valid_state(string state)
    {
        var institution = new Institution(validUnitId, "Test University", state);

        Assert.Equal(state, institution.State);
    }

    [Theory]
    [InlineData("md")]     // 소문자
    [InlineData("M")]      // 1글자
    [InlineData("MDX")]    // 3글자
    [InlineData("12")]     // 숫자
    [InlineData("A1")]     // 섞임
    public void Rejects_invalid_state(string state)
    {
        Assert.Throws<ArgumentException>(() => new Institution(100001, "Test University", state));
    }

    [Fact]
    public void Rename_rejects_name_longer_than_max()
    {
        var institution = new Institution(102368, "Test University", "MD");
        var tooLong = new string('a', Institution.NameMaxLength + 1);

        Assert.Throws<ArgumentException>(() => institution.Rename(tooLong));
    }

    [Theory]
    [InlineData(60000)]
    [InlineData(73491)]
    [InlineData(99999)]
    public void Appended_unit_id_is_valid(int baseNumber)
    {
        var unitId = UnitIdCheckDigit.Append(baseNumber);

        Assert.InRange(unitId, Institution.UnitIdMin, Institution.UnitIdMax);
        Assert.True(UnitIdCheckDigit.IsValid(unitId));
    }

    [Fact]
    public void Changing_one_digit_makes_it_invalid()
    {
        var unitId = UnitIdCheckDigit.Append(73491);
        Assert.False(UnitIdCheckDigit.IsValid(unitId + 1));   // 체크 디지트를 하나 바꿈
        Assert.False(UnitIdCheckDigit.IsValid(unitId + 9));   // 체크 디지트를 하나 바꿈
        Assert.False(UnitIdCheckDigit.IsValid(unitId + 2));   // 체크 디지트를 하나 바꿈
    }

    [Theory]
    [InlineData(99999)]       // 5자리
    [InlineData(1000000)]     // 7자리
    [InlineData(0)]
    [InlineData(-1)]
    public void Rejects_out_of_range_unit_id(int unitId)
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => new Institution(unitId, "Test University", "MD"));
    }

    [Fact]
    public void Rejects_unit_id_with_wrong_check_digit()
    {
        var valid = UnitIdCheckDigit.Append(10000);
        var wrongCheckDigit = valid / 10 * 10 + (valid % 10 + 1) % 10;   // 체크 디지트만 하나 바꿈

        Assert.Throws<ArgumentException>(() => new Institution(wrongCheckDigit, "Test University", "MD"));
    }
}