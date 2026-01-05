using Controle.Gastos.Residenciais.Api.Extensions;

namespace Controle.Gastos.Residenciais.Api.Configuration
{
    public static class LoggerConfig
    {
        public static IServiceCollection AddLoggingConfig(this IServiceCollection services, WebApplicationBuilder builder)
        {
            // Add Log - Examples: ElmahIo, Serilog, NLog

            var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

            services.AddHealthChecks()
                .AddCheck("Categorias", new SqlLiteHealthCheck(connectionString))
                .AddSqlite(connectionString, name: "BancoSQLite");

            // Add UI com interface amigável 

            //services.AddHealthChecksUI()
            //    .AddSqliteStorage(connectionString);

            return services;
        }

        public static IApplicationBuilder UseLoggingConfiguration(this IApplicationBuilder app)
        {
            // Use Log - Examples: ElmahIo, Serilog, NLog

            return app;
        }
    }
}