using Controle.Gastos.Residenciais.Business.Entities;
using Controle.Gastos.Residenciais.Business.Interfaces;
using Controle.Gastos.Residenciais.Business.Interfaces.Repositories;
using Controle.Gastos.Residenciais.Business.Interfaces.Services;
using Controle.Gastos.Residenciais.Business.Notificacoes;
using Controle.Gastos.Residenciais.Business.Services;
using Moq;
using Moq.AutoMock;
using System.Net.Sockets;
using System.Threading;

namespace Controle.Gastos.Residenciais.Business.Tests
{
    public class TransacaoServiceTests
    {
        private readonly AutoMocker _mocker;
        private readonly ITransacaoService _transacaoService;

        public TransacaoServiceTests()
        {
            _mocker = new AutoMocker();
            _transacaoService = _mocker.CreateInstance<TransacaoService>();
        }

        [Fact(DisplayName = "Adicionar Transacao com Falha")]
        [Trait("Categoria", "Transacao Service")]
        public void TransacaoService_Adicionar_DeveFalharDevidoTransacaoInvalida()
        {
            // Arrange
            var transacao = new Transacao("a", 123, Guid.NewGuid(), Guid.NewGuid(), Enums.TransacaoTipo.Receita);

            // Act
            _transacaoService.Adicionar(transacao);

            // Assert
            _mocker.GetMock<ITransacaoRepository>()
                .Verify(r => r.Adicionar(transacao), Times.Never);
        }
    }
}
