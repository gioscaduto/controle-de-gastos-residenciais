using Controle.Gastos.Residenciais.Business.Entities;
using Controle.Gastos.Residenciais.Business.Entities.Validations;
using Controle.Gastos.Residenciais.Business.Enums;
using Controle.Gastos.Residenciais.Business.Interfaces;
using Controle.Gastos.Residenciais.Business.Interfaces.Repositories;
using Controle.Gastos.Residenciais.Business.Interfaces.Services;

namespace Controle.Gastos.Residenciais.Business.Services
{
    public class TransacaoService : BaseService, ITransacaoService
    {
        private const string MSG_PESSOA_NAO_ENCONTRADA = "Pessoa não encontrada";
        private const string MSG_CATEGORIA_NAO_ENCONTRADA = "Categoria não encontrada";
        private const int IDADE_MAIORIDADE = 18;
        private readonly ITransacaoRepository _transacaoRepository;
        private readonly IPessoaRepository _pessoaRepository;
        private readonly ICategoriaRepository _categoriaRepository;

        public TransacaoService(ITransacaoRepository transacaoRepository,
                                IPessoaRepository pessoaRepository,
                                ICategoriaRepository categoriaRepository,
                                INotificador notificador) : base(notificador)
        {
            _transacaoRepository = transacaoRepository;
            _pessoaRepository = pessoaRepository;
            _categoriaRepository = categoriaRepository;
        }

        public async Task Adicionar(Transacao transacao)
        {
             if(!await Validar(transacao)) return;

            await _transacaoRepository.Adicionar(transacao);
        }

        private async Task<bool> Validar(Transacao transacao)
        {
            if (!ExecutarValidacao(new TransacaoValidation(), transacao)) return false;

            var pessoa = await _pessoaRepository.ObterPorId(transacao.PessoaId);

            if (pessoa is null)
            {
                Notificar(MSG_PESSOA_NAO_ENCONTRADA);
                return false;
            }

            var categoria = await _categoriaRepository.ObterPorId(transacao.CategoriaId);

            if (categoria is null)
            {
                Notificar(MSG_CATEGORIA_NAO_ENCONTRADA);
                return false;
            }

            if (pessoa.Idade < IDADE_MAIORIDADE && transacao.Tipo != TransacaoTipo.Despesa)
            {
                Notificar("Para pessoa menor de idade (menor de 18 anos), apenas despesas são aceitas.");
                return false;
            }

            if (categoria.Finalidade == CategoriaFinalidade.Despesa && transacao.Tipo == TransacaoTipo.Receita)
            {
                Notificar("Categoria destinada para despesas não pode ser utilizada em uma transação do tipo receita.");
                return false;
            }

            if (categoria.Finalidade == CategoriaFinalidade.Receita && transacao.Tipo == TransacaoTipo.Despesa)
            {
                Notificar("Categoria destinada para receitas não pode ser utilizada em uma transação do tipo despesa.");
                return false ;
            }

            return true;
        }
 
        public void Dispose()
        {
            _transacaoRepository?.Dispose();
        }
    }
}
