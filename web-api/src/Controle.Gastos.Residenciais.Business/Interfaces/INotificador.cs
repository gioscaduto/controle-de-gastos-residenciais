using Controle.Gastos.Residenciais.Business.Notificacoes;

namespace Controle.Gastos.Residenciais.Business.Interfaces
{
    public interface INotificador
    {
        bool TemNotificacao();
        List<Notificacao> ObterNotificacoes();
        void Handle(Notificacao notificacao);
    }
}