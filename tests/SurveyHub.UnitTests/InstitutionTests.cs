using SurveyHub.Domain.Institutions;

namespace SurveyHub.UnitTests.Domain;

public class InstitutionTests
{
    [Theory]
    [InlineData("MD")]
    [InlineData("VA")]
    [InlineData("DC")]
    public void Accepts_valid_state(string state)
    {
        var institution = new Institution(100001, "Test University", state);

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
        var institution = new Institution(100001, "Test University", "MD");
        var tooLong = new string('a', Institution.NameMaxLength + 1);

        Assert.Throws<ArgumentException>(() => institution.Rename(tooLong));
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
}