namespace SurveyHub.UnitTests;

using SurveyHub.Application.Common;

public class PagedResultTests
{
    [Theory]
    [InlineData(10, 25, 3)]
    [InlineData(10, 20, 2)]
    [InlineData(10, 0, 0)]
    [InlineData(10, 1, 1)]
    public void TotalPages_ShouldBeCalculatedCorrectly(int pageSize, int totalCount, int expectedTotalPages)
    {
        // Arrange
        var pagedResult = new PagedResult<string>([], Page: 1, pageSize, totalCount);

        // Assert
        Assert.Equal(expectedTotalPages, pagedResult.TotalPages);
    }
}