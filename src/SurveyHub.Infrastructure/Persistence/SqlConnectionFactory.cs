using Microsoft.Data.SqlClient;

namespace SurveyHub.Infrastructure.Persistence;

public sealed class SqlConnectionFactory(string connectionString)
{
    public SqlConnection Create() => new(connectionString);
}