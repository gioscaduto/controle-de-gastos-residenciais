using Controle.Gastos.Residenciais.Business.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Controle.Gastos.Residenciais.Data.Mappings
{
    public class TransacaoMapping : IEntityTypeConfiguration<Transacao>
    {
        public void Configure(EntityTypeBuilder<Transacao> builder)
        {
            builder.HasKey(t => t.Id);

            builder.Property(t => t.Descricao)
                .IsRequired()
                .HasColumnType("varchar(1000)");

            builder.Property(t => t.Valor)
                .HasPrecision(10, 2)
                .IsRequired();

            builder.Property(t => t.Tipo)
                .IsRequired();

            builder.Property(t => t.Removido)
                .IsRequired();

            builder.HasOne(t => t.Categoria)
                .WithMany()
                .HasForeignKey(t => t.CategoriaId);

            builder.HasIndex(t => t.CategoriaId)
                .IsUnique(false)
                .HasDatabaseName("IX_Transacao_CategoriaId");

            builder.HasOne(t => t.Pessoa)
                .WithMany()
                .HasForeignKey(t => t.PessoaId);

            builder.HasIndex(t => t.PessoaId)
                .IsUnique(false)
                .HasDatabaseName("IX_Transacao_PessoaId");

            builder.ToTable("Transacoes");
        }
    }
}
