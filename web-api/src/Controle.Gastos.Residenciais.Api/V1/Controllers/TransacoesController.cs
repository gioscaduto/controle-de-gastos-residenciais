using Controle.Gastos.Residenciais.Api.Controllers;
using Controle.Gastos.Residenciais.Api.ViewModels;
using Controle.Gastos.Residenciais.Business.Interfaces;
using Controle.Gastos.Residenciais.Business.Interfaces.Repositories;
using Controle.Gastos.Residenciais.Business.Interfaces.Services;
using Controle.Gastos.Residenciais.Data.Dtos;
using Controle.Gastos.Residenciais.Data.Queries;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Controle.Gastos.Residenciais.Api.V1.Controllers
{
    [AllowAnonymous]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/transacoes")]
    public class TransacoesController : MainController
    {
        private readonly ITransacaoRepository _transacaoRepository;
        private readonly ITransacaoService _transacaoService;
        private readonly ITransacaoQuery _transacaoQuery;

        public TransacoesController(ITransacaoRepository transacaoRepository,
                                    ITransacaoService transacaoService,
                                    INotificador notificador,
                                    IUser user,
                                    ITransacaoQuery transacaoQuery) : base(notificador, user)
        {
            _transacaoRepository = transacaoRepository;
            _transacaoService = transacaoService;
            _transacaoQuery = transacaoQuery;
        }

        [HttpGet]
        public async Task<IEnumerable<TransacaoVm>> Listar([FromQuery] PaginacaoVm paginacao)
        {
            var transacoes = await _transacaoRepository.ListarPaginado(paginacao.Pagina, paginacao.QtdItensPorPagina);

            return transacoes?.Select(t => new TransacaoVm(t));
        }

        [HttpGet("pessoa")]
        public async Task<IEnumerable<TransacaoPessoaDto>> ListarPorPessoa()
        {
            return await _transacaoQuery.ListarPorPessoa();
        }

        [HttpGet("categoria")]
        public async Task<IEnumerable<TransacaoCategoriaDto>> ListarPorCategoria()
        {
            return await _transacaoQuery.ListarPorCategoria();
        }

        [HttpPost]
        public async Task<ActionResult<TransacaoVm>> Adicionar(TransacaoVm transacaoVm)
        {
            if (!ModelState.IsValid) return CustomResponse(ModelState);

            await _transacaoService.Adicionar(transacaoVm.Map());

            return CustomResponse(transacaoVm);
        }
    }
}