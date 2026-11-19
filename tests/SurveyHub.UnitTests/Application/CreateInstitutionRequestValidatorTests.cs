using FluentValidation.TestHelper;
using SurveyHub.Application.Institutions;
using SurveyHub.Domain.Institutions;

namespace SurveyHub.UnitTests.Application;

public class CreateInstitutionRequestValidatorTests
{
    private readonly CreateInstitutionRequestValidator _validator = new();

    [Fact]
    public void Valid_request_process() => _validator.TestValidate(new CreateInstitutionRequest("New University", "MD")).ShouldNotHaveAnyValidationErrors();

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    public void Empty_name_fails(string name)
        => _validator.TestValidate(new CreateInstitutionRequest(name, "MD"))
                     .ShouldHaveValidationErrorFor(x => x.Name);

    [Fact]
    public void Name_longer_than_max_fails()
        => _validator.TestValidate(new CreateInstitutionRequest(new string('a', Institution.NameMaxLength + 1), "MD"))
                     .ShouldHaveValidationErrorFor(x => x.Name);

    [Theory]
    [InlineData("md")]
    [InlineData("M")]
    [InlineData("MDX")]
    [InlineData("12")]
    public void Invalid_state_fails(string state)
        => _validator.TestValidate(new CreateInstitutionRequest("New University", state))
                     .ShouldHaveValidationErrorFor(x => x.State);
}