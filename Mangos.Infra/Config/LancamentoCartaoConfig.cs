namespace Mangos.Infra.Config
{
    using Mangos.Dominio.Entities;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    public class LancamentoCartaoConfig : IEntityTypeConfiguration<LancamentoCartao>
    {
        public void Configure(EntityTypeBuilder<LancamentoCartao> builder)
        {
            builder.ToTable("LancamentoCartao");

            builder.HasKey(l => l.Id);

            builder.Property(p => p.Descricao)
                .HasMaxLength(255)
                .IsRequired();

            builder.Property(p => p.Valor)
                .HasColumnType("money");

            builder.Property(p => p.Observacoes)
                .HasColumnType("varchar(MAX)");
        }
    }
}