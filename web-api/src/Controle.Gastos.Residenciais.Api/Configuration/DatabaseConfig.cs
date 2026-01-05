using Controle.Gastos.Residenciais.Api.Data;
using Controle.Gastos.Residenciais.Data.Context;
using Microsoft.EntityFrameworkCore;

namespace Controle.Gastos.Residenciais.Api.Configuration
{
    public static class DatabaseConfig
    {
        public static WebApplicationBuilder AddDatabaseConfiguration(this WebApplicationBuilder builder)
        {
            if (builder == null) throw new ArgumentNullException(nameof(builder));

            var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

            if (string.IsNullOrEmpty(connectionString)) throw new ArgumentNullException(nameof(connectionString));

            builder.Services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlite(connectionString));

            builder.Services.AddDbContext<MeuDbContext>(options =>
                options.UseSqlite(connectionString));

            return builder;
        }

        public static WebApplication UseDbSeed(this WebApplication app)
        {
            ArgumentNullException.ThrowIfNull(app);

            DbMigrationHelpers.EnsureSeedData(app).Wait();

            return app;
        }
    }
}