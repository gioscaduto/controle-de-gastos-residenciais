using Controle.Gastos.Residenciais.Api.Extensions;
using Controle.Gastos.Residenciais.Business.Interfaces;
using Controle.Gastos.Residenciais.Business.Interfaces.Repositories;
using Controle.Gastos.Residenciais.Business.Interfaces.Services;
using Controle.Gastos.Residenciais.Business.Notificacoes;
using Controle.Gastos.Residenciais.Business.Services;
using Controle.Gastos.Residenciais.Data.Context;
using Controle.Gastos.Residenciais.Data.Queries;
using Controle.Gastos.Residenciais.Data.Repository;
using Microsoft.Extensions.Options;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Controle.Gastos.Residenciais.Api.Configuration
{
    public static class DependencyInjectionConfig
    {
        public static IServiceCollection ResolveDependencies(this IServiceCollection services)
        {
            services.AddScoped<MeuDbContext>();
            services.AddScoped<ICategoriaRepository, CategoriaRepository>();
            services.AddScoped<IPessoaRepository, PessoaRepository>();
            services.AddScoped<ITransacaoRepository, TransacaoRepository>();

            services.AddScoped<ITransacaoQuery, TransacaoQuery>();

            services.AddScoped<INotificador, Notificador>();
            services.AddScoped<ICategoriaService, CategoriaService>();
            services.AddScoped<IPessoaService, PessoaService>();
            services.AddScoped<ITransacaoService, TransacaoService>();

            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddScoped<IUser, AspNetUser>();

            services.AddTransient<IConfigureOptions<SwaggerGenOptions>, ConfigureSwaggerOptions>();

            return services;
        }
    }
}