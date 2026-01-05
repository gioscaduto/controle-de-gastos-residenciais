namespace Controle.Gastos.Residenciais.Business.Entities
{
    public abstract class Entidade
    {
        public Entidade()
        {
            Id = Guid.NewGuid();
        }

        public Guid Id { get; set; }
        public bool Removido { get; protected set; }

        public void Remover() => Removido = true;
    }
}