namespace SurveyHub.UnitTests;

using SurveyHub.Application.Common;

public class PageRequestTests
{
    private static readonly PaginationOptions Options = new() { DefaultPageSize = 20, MaxPageSize = 100 };

    [Theory]
    [InlineData(null, null, 1, 20, 0)]
    [InlineData(2, null, 2, 20, 20)]
    [InlineData(2, 10, 2, 10, 10)]
    [InlineData(2, 500, 2, 100, 100)]
    public void PageRequest_Should_Have_Default_Values(int? page, int? pageSize, int expectedPage, int expectedPageSize, int expectedSkipped)
    {
        // Arrange
        var pageRequest = PageRequest.Create(page, pageSize, Options);

        // Assert
        Assert.True(pageRequest.IsSuccess);
        Assert.Equal(expectedPage, pageRequest.Value.Page);
        Assert.Equal(expectedPageSize, pageRequest.Value.PageSize);
        Assert.Equal(expectedSkipped, pageRequest.Value.Skip);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    public void Create_Should_Return_Error_When_Page_Is_Less_Than_One(int page)
    {
        var request = PageRequest.Create(page, 10, Options);

        Assert.False(request.IsSuccess);
        Assert.Equal("Page must be a positive integer.", request.Error.Description);
    }

    [Theory]
    [InlineData(0, 10, "Paging.InvalidPage")]
    [InlineData(-1, 10, "Paging.InvalidPage")]
    [InlineData(1, 0, "Paging.InvalidPageSize")]
    [InlineData(1, -1, "Paging.InvalidPageSize")]
    public void Create_Rejects_Values_Below_One(int page, int pageSize, string expectedCode)
    {
        var request = PageRequest.Create(page, pageSize, Options);

        Assert.True(request.IsFailure);
        Assert.Equal(ErrorType.Validation, request.Error.Type);
        Assert.Equal(expectedCode, request.Error.Code);
    }
}