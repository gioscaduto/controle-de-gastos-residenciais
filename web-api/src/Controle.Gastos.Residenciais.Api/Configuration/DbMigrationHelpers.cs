using Controle.Gastos.Residenciais.Api.Data;
using Controle.Gastos.Residenciais.Business.Entities;
using Controle.Gastos.Residenciais.Data.Context;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Controle.Gastos.Residenciais.Api.Configuration
{
    public static class DbMigrationHelpers
    {
        public static async Task EnsureSeedData(WebApplication serviceScope)
        {
            var services = serviceScope.Services.CreateScope().ServiceProvider;
            await EnsureSeedData(services);
        }

        private static async Task EnsureSeedData(IServiceProvider serviceProvider)
        {
            using var scope = serviceProvider.GetRequiredService<IServiceScopeFactory>().CreateScope();
            var env = scope.ServiceProvider.GetRequiredService<IWebHostEnvironment>();

            if (env.IsDevelopment() || env.IsStaging() || env.IsEnvironment("Docker"))
            {
                var context = scope.ServiceProvider.GetRequiredService<MeuDbContext>();
                var contextIdentity = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

                await context.Database.MigrateAsync();
                await contextIdentity.Database.MigrateAsync();

                await EnsureSeedData(context, contextIdentity);
            }
        }

        private static async Task EnsureSeedData(MeuDbContext context, ApplicationDbContext contextIdentity)
        {
            if (contextIdentity.Users.Any())
                return;

            var userId = Guid.NewGuid();

            var user = new IdentityUser
            {
                Id = userId.ToString(),
                UserName = "giovani.scaduto@gmail.com",
                NormalizedUserName = "GIOVANI.SCADUTO@GMAIL.COM",
                Email = "giovani.scaduto@gmail.com",
                NormalizedEmail = "GIOVANI.SCADUTO@GMAIL.COM",
                AccessFailedCount = 0,
                LockoutEnabled = false,
                TwoFactorEnabled = false,
                ConcurrencyStamp = Guid.NewGuid().ToString(),
                EmailConfirmed = true,
                SecurityStamp = Guid.NewGuid().ToString()
            };

            var hasher = new PasswordHasher<IdentityUser>();
            user.PasswordHash = hasher.HashPassword(user, "Giovani@123");

            await contextIdentity.Users.AddAsync(user);

            await contextIdentity.UserClaims.AddRangeAsync(
                new IdentityUserClaim<string>
                {
                    UserId = userId.ToString(),
                    ClaimType = "Fornecedor",
                    ClaimValue = "Adicionar,Atualizar,Excluir"
                },
                new IdentityUserClaim<string>
                {
                    UserId = userId.ToString(),
                    ClaimType = "Produto",
                    ClaimValue = "Adicionar,Atualizar,Excluir"
                });

            await contextIdentity.SaveChangesAsync();

            if (context.Categorias.Any() || context.Pessoas.Any())
                return;

            await context.Categorias.AddRangeAsync(
                new Categoria("Contas domésticas", Business.Enums.CategoriaFinalidade.Despesa),
                new Categoria("Salário", Business.Enums.CategoriaFinalidade.Receita),
                new Categoria("Investimento", Business.Enums.CategoriaFinalidade.Ambas));

            await context.Pessoas.AddRangeAsync(
                new Pessoa("Giovani", 35),
                new Pessoa("Maria", 17));

            await context.SaveChangesAsync();
        }
    }
}