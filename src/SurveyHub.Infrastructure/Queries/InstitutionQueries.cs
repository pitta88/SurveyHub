using Dapper;
using SurveyHub.Application.Common;
using SurveyHub.Application.Institutions;
using SurveyHub.Infrastructure.Persistence;

namespace SurveyHub.Infrastructure.Queries;

internal sealed class InstitutionQueries(SqlConnectionFactory sqlConnectionFactory) : IInstitutionQueries
{
    public async Task<PagedResult<InstitutionDto>> GetPageAsync(PageRequest pageRequest, CancellationToken ct)
    {
        const string sql = """
            SELECT UnitID, Name, State
            FROM Institutions
            ORDER BY UnitID
            OFFSET @Skip ROWS FETCH NEXT @PageSize ROWS ONLY;

            SELECT COUNT(*) FROM Institutions;
            """;

        await using var connection = sqlConnectionFactory.Create();

        var command = new CommandDefinition(
            sql,
            new { pageRequest.Skip, pageRequest.PageSize },
            cancellationToken: ct);

        await using var multi = await connection.QueryMultipleAsync(command);
        var items = (await multi.ReadAsync<InstitutionDto>()).AsList();
        var totalCount = await multi.ReadSingleAsync<int>();

        return new PagedResult<InstitutionDto>(items, pageRequest.Page, pageRequest.PageSize, totalCount);
    }

    public async Task<InstitutionDto?> GetByUnitIdAsync(int unitId, CancellationToken ct)
    {
        const string sql = """
            SELECT UnitId, Name, State
            FROM Institutions
            WHERE UnitId = @unitId;
            """;

        await using var connection = sqlConnectionFactory.Create();

        return await connection.QuerySingleOrDefaultAsync<InstitutionDto>(
            new CommandDefinition(sql, new { unitId }, cancellationToken: ct));
    }
}
