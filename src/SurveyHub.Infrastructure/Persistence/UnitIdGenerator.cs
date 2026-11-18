using Dapper;
using SurveyHub.Domain.Institutions;
using SurveyHub.Infrastructure.Persistence;

namespace SurveyHub.Application.Institutions;

internal sealed class UnitIdGenerator(SqlConnectionFactory connectionFactory) : IUnitIdGenerator
{
    public async Task<int> NextAsync(CancellationToken ct)
    {
        await using var connection = connectionFactory.Create();
        var baseNumber =  await connection.ExecuteScalarAsync<int>(new CommandDefinition("SELECT NEXT VALUE For dbo.UnitIdBaseSequence", cancellationToken: ct));

        return UnitIdCheckDigit.Append(baseNumber);
    }
}