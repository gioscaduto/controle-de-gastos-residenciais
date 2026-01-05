using Controle.Gastos.Residenciais.Business.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Controle.Gastos.Residenciais.Data.Mappings
{
    public class CategoriaMapping : IEntityTypeConfiguration<Categoria>
    {
        public void Configure(EntityTypeBuilder<Categoria> builder)
        {
            builder.HasKey(c => c.Id);

            builder.Property(c => c.Descricao)
                .IsRequired()
                .HasColumnType("varchar(255)");

            builder.Property(c => c.Finalidade)
                .IsRequired();

            builder.Property(c => c.Removido)
                .IsRequired();

            builder.ToTable("Categorias");
        }
    }
}