namespace Mangos.Infra.Config
{
    using Mangos.Dominio.Entities;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    public class MetaMovimentacaoConfig : IEntityTypeConfiguration<MetaMovimentacao>
    {
        public void Configure(EntityTypeBuilder<MetaMovimentacao> builder)
        {
            builder.ToTable("MetaMovimentacao");

            builder.HasKey(m => m.Id);

            builder.Property(p => p.ValorMensal)
                .HasColumnType("money");
        }
    }
}