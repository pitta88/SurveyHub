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
        var (pageRequest, error) = PageRequest.Create(page, pageSize, Options);

        // Assert
        Assert.Null(error);
        Assert.NotNull(pageRequest);
        Assert.Equal(expectedPage, pageRequest.Page);
        Assert.Equal(expectedPageSize, pageRequest.PageSize);
        Assert.Equal(expectedSkipped, pageRequest.Skip);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    public void Create_Should_Return_Error_When_Page_Is_Less_Than_One(int page)
    {
        var (request, error) = PageRequest.Create(page, 10, Options);

        Assert.Null(request);
        Assert.Equal("Page must be a positive integer.", error);
    }

    [Theory]
    [InlineData(0, 10)]
    [InlineData(-1, 10)]
    [InlineData(1, 0)]
    [InlineData(1, -1)]
    public void Create_Should_Return_Error_When_PageSize_Is_Less_Than_One(int page, int pageSize)
    {
        var (request, error) = PageRequest.Create(page, pageSize, Options);

        Assert.Null(request);
        Assert.NotNull(error);
    }   
}