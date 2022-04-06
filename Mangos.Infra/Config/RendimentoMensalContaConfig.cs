namespace Mangos.Infra.Config
{
    using Mangos.Dominio.Entities;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    public class RendimentoMensalContaConfig : IEntityTypeConfiguration<RendimentoMensalConta>
    {
        public void Configure(EntityTypeBuilder<RendimentoMensalConta> builder)
        {
            builder.ToTable("RendimentoMensalConta");

            builder.HasKey(r => r.Id);

            builder.Property(p => p.Valor)
                .HasColumnType("money");
        }
    }
}