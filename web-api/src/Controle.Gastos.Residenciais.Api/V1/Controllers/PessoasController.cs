using Controle.Gastos.Residenciais.Api.Controllers;
using Controle.Gastos.Residenciais.Api.ViewModels;
using Controle.Gastos.Residenciais.Business.Interfaces;
using Controle.Gastos.Residenciais.Business.Interfaces.Repositories;
using Controle.Gastos.Residenciais.Business.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Controle.Gastos.Residenciais.Api.V1.Controllers
{
    [AllowAnonymous]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/pessoas")]
    public class PessoasController : MainController
    {
        private readonly IPessoaRepository _pessoaRepository;
        private readonly IPessoaService _pessoaService;

        public PessoasController(IPessoaRepository pessoaRepository,
                                 IPessoaService pessoaService,
                                 INotificador notificador,
                                 IUser user) : base(notificador, user)
        {
            _pessoaRepository = pessoaRepository;
            _pessoaService = pessoaService;
        }

        [HttpGet]
        public async Task<IEnumerable<PessoaVm>> Listar([FromQuery] PaginacaoVm paginacao)
        {
            var pessoas = await _pessoaRepository.ListarPaginado(paginacao.Pagina, paginacao.QtdItensPorPagina);

            return pessoas?.Select(p => new PessoaVm(p));
        }

        [HttpGet("{id:guid}")]
        public async Task<ActionResult<PessoaVm>> ObterPorId(Guid id)
        {
            var pessoa = await _pessoaRepository.ObterPorId(id);

            if (pessoa == null) return NotFound();

            return new PessoaVm(pessoa);
        }

        [HttpPost]
        public async Task<ActionResult<PessoaVm>> Adicionar(PessoaVm pessoaVm)
        {
            if (!ModelState.IsValid) return CustomResponse(ModelState);

            await _pessoaService.Adicionar(pessoaVm.Map());

            return CustomResponse(pessoaVm);
        }

        [HttpPut("{id:guid}")]
        public async Task<ActionResult<PessoaVm>> Atualizar(Guid id, PessoaVm pessoaVm)
        {
            if (id != pessoaVm.Id)
            {
                NotificarErro("O id informado não é o mesmo que foi passado na query");
                return CustomResponse(pessoaVm);
            }

            if (!ModelState.IsValid) return CustomResponse(ModelState);

            var pessoa = await _pessoaRepository.ObterPorId(id);

            pessoaVm.Atualizar(pessoa);

            await _pessoaService.Atualizar(pessoa);

            return CustomResponse(pessoaVm);
        }

        [HttpDelete("{id:guid}")]
        public async Task<ActionResult<PessoaVm>> Excluir(Guid id)
        {
            var pessoa = await _pessoaRepository.ObterPorId(id);

            if (pessoa is null) return NotFound();

            await _pessoaService.Remover(id);

            return CustomResponse(new PessoaVm(pessoa));
        }
    }
}