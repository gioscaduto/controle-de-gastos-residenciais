using Controle.Gastos.Residenciais.Business.Entities;
using Controle.Gastos.Residenciais.Business.Entities.Validations;
using Controle.Gastos.Residenciais.Business.Interfaces;
using Controle.Gastos.Residenciais.Business.Interfaces.Repositories;
using Controle.Gastos.Residenciais.Business.Interfaces.Services;

namespace Controle.Gastos.Residenciais.Business.Services
{
    public class PessoaService : BaseService, IPessoaService
    {
        private const string MSG_PESSOA_MESMO_NOME = "Já existe uma pessoa com este nome";
        private readonly IPessoaRepository _pessoaRepository;

        public PessoaService(IPessoaRepository pessoaRepository,
                             INotificador notificador) : base(notificador)
        {
            _pessoaRepository = pessoaRepository;
        }

        public async Task Adicionar(Pessoa pessoa)
        {
            if (!ExecutarValidacao(new PessoaValidation(), pessoa)) return;

            if (await _pessoaRepository.Existe(d => d.Nome.Trim() == pessoa.Nome.Trim()))
            {
                Notificar(MSG_PESSOA_MESMO_NOME);
                return;
            }

            await _pessoaRepository.Adicionar(pessoa);
        }

        public async Task Atualizar(Pessoa pessoa)
        {
            if (!ExecutarValidacao(new PessoaValidation(), pessoa)) return;

            if (!await ExistePessoa(pessoa.Id)) return;

            if (await _pessoaRepository.Existe(d =>
                    d.Id != pessoa.Id && d.Nome.Trim() == pessoa.Nome.Trim()))
            {
                Notificar(MSG_PESSOA_MESMO_NOME);
                return;
            }

            await _pessoaRepository.Atualizar(pessoa);
        }

        public async Task Remover(Guid id)
        {
            if (!await ExistePessoa(id)) return;

            await _pessoaRepository.Remover(id);
        }

        private async Task<bool> ExistePessoa(Guid id)
        {
            var existe = await _pessoaRepository.Existe(d => d.Id == id);

            if (!existe)
            {
                Notificar("Pessoa não encontrada");
                return false;
            }

            return true;
        }

        public void Dispose()
        {
            _pessoaRepository?.Dispose();
        }
    }
}