using Controle.Gastos.Residenciais.Business.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Controle.Gastos.Residenciais.Data.Mappings
{
    public class PessoaMapping : IEntityTypeConfiguration<Pessoa>
    {
        public void Configure(EntityTypeBuilder<Pessoa> builder)
        {
            builder.HasKey(p => p.Id);

            builder.Property(p => p.Nome)
                .IsRequired()
                .HasColumnType("varchar(255)");

            builder.Property(p => p.Idade)
                .IsRequired();

            builder.Property(p => p.Removido)
                .IsRequired();

            builder.ToTable("Pessoas");
        }
    }
}
