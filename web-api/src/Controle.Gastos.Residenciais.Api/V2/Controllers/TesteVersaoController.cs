using Controle.Gastos.Residenciais.Api.Controllers;
using Controle.Gastos.Residenciais.Business.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Controle.Gastos.Residenciais.Api.V2.Controllers
{
    [ApiVersion("2.0")]
    [Route("api/v{version:apiVersion}/teste-versao")]
    public class TesteVersaoController : MainController
    {
        private readonly ILogger _logger;

        public TesteVersaoController(INotificador notificador, 
            IUser appUser, 
            ILogger<TesteVersaoController> logger) : base(notificador, appUser)
        {
            _logger = logger;
        }

        [HttpGet]
        public string Valor()
        {
            _logger.LogTrace("Log de Trace");
            _logger.LogDebug("Log de Debug");
            _logger.LogInformation("Log de Informação");
            _logger.LogWarning("Log de Aviso");
            _logger.LogError("Log de Erro");
            _logger.LogCritical("Log de Problema Critico");

            return "Sou a V2, sou a nova versão";
        }
    }
}