namespace Mangos.Infra.Config
{
    using Mangos.Dominio.Entities;
    using Mangos.Dominio.Enums;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    public class LancamentoFixoConfig : IEntityTypeConfiguration<LancamentoFixo>
    {
        public void Configure(EntityTypeBuilder<LancamentoFixo> builder)
        {
            builder.ToTable("LancamentoFixo");

            builder.HasKey(l => l.Id);

            builder.HasMany(c => c.LancamentosOrigem)
                .WithOne(c => c.LancamentoFixoOrigem)
                .HasForeignKey(c => c.LancamentoFixoOrigemId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(c => c.LancamentosCartaoOrigem)
                .WithOne(c => c.LancamentoFixoOrigem)
                .HasForeignKey(c => c.LancamentoFixoOrigemId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Property(p => p.Descricao)
                .HasMaxLength(255)
                .IsRequired();

            builder.Property(p => p.Valor)
                .HasColumnType("money");

            builder.Property(l => l.Tipo)
                .IsRequired()
                .HasConversion(
                    t => (int)t,
                    t => (TipoLancamentoFixo)t
                );

            builder.Property(l => l.Periodicidade)
                .IsRequired()
                .HasConversion(
                    t => (int)t,
                    t => (PeriodicidadeLancamentoFixo)t
                );
        }
    }
}