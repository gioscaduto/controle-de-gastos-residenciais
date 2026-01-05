using Microsoft.Data.Sqlite;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace Controle.Gastos.Residenciais.Api.Extensions
{
    public class SqlLiteHealthCheck : IHealthCheck
    {
        readonly string _connection;

        public SqlLiteHealthCheck(string connection)
        {
            _connection = connection;
        }

        public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = new CancellationToken())
        {
            try
            {
                using (var connection = new SqliteConnection(_connection))
                {
                    await connection.OpenAsync(cancellationToken);

                    var command = connection.CreateCommand();
                    command.CommandText = "SELECT COUNT(id) FROM categorias";

                    return Convert.ToInt32(await command.ExecuteScalarAsync(cancellationToken)) > 0
                        ? HealthCheckResult.Healthy()
                        : HealthCheckResult.Unhealthy();
                }
            }
            catch (Exception)
            {
                return HealthCheckResult.Unhealthy();
            }
        }
    }
}