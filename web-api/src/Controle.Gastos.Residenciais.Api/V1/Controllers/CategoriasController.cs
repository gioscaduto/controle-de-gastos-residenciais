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
    [Route("api/v{version:apiVersion}/categorias")]
    public class CategoriasController : MainController
    {
        private readonly ICategoriaRepository _categoriaRepository;
        private readonly ICategoriaService _categoriaService;

        public CategoriasController(ICategoriaRepository categoriaRepository,
                                    ICategoriaService categoriaService,
                                    INotificador notificador,
                                    IUser user) : base(notificador, user)
        {
            _categoriaRepository = categoriaRepository;
            _categoriaService = categoriaService;
        }

        [HttpGet]
        public async Task<IEnumerable<CategoriaVm>> Listar([FromQuery] PaginacaoVm paginacao)
        {
            var categorias = await _categoriaRepository.ListarPaginado(paginacao.Pagina, paginacao.QtdItensPorPagina);

            return categorias?.Select(c => new CategoriaVm(c));
        }

        [HttpGet("{id:guid}")]
        public async Task<ActionResult<CategoriaVm>> ObterPorId(Guid id)
        {
            var categoria = await _categoriaRepository.ObterPorId(id);

            if (categoria == null) return NotFound();

            return new CategoriaVm(categoria);
        }

        [HttpPost]
        public async Task<ActionResult<CategoriaVm>> Adicionar(CategoriaVm categoriaVm)
        {
            if (!ModelState.IsValid) return CustomResponse(ModelState);

            await _categoriaService.Adicionar(categoriaVm.Map());

            return CustomResponse(categoriaVm);
        }

        [HttpPut("{id:guid}")]
        public async Task<ActionResult<CategoriaVm>> Atualizar(Guid id, CategoriaVm categoriaVm)
        {
            if (id != categoriaVm.Id)
            {
                NotificarErro("O id informado não é o mesmo que foi passado na query");
                return CustomResponse(categoriaVm);
            }

            if (!ModelState.IsValid) return CustomResponse(ModelState);

            var categoria = await _categoriaRepository.ObterPorId(id);

            if (categoria == null) return NotFound();

            categoriaVm.Atualizar(categoria);

            await _categoriaService.Atualizar(categoria);

            return CustomResponse(categoriaVm);
        }

        [HttpDelete("{id:guid}")]
        public async Task<ActionResult<CategoriaVm>> Excluir(Guid id)
        {
            var categoria = await _categoriaRepository.ObterPorId(id);

            if (categoria is null) return NotFound();

            await _categoriaService.Remover(id);

            return CustomResponse(new CategoriaVm(categoria));
        }
    }
}
