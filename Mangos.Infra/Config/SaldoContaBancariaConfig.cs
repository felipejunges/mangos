namespace Mangos.Infra.Config
{
    using Mangos.Dominio.Entities;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    public class SaldoContaBancariaConfig : IEntityTypeConfiguration<SaldoContaBancaria>
    {
        public void Configure(EntityTypeBuilder<SaldoContaBancaria> builder)
        {
            builder.ToTable("SaldoContaBancaria");

            builder.HasKey(s => s.Id);

            builder.Property(p => p.ValorMovimentacoes)
                .HasColumnType("money");

            builder.Property(p => p.ValorSaldo)
                .HasColumnType("money");

            builder.Property(p => p.ValorConferenciaSaldo)
                .HasColumnType("money");

            builder.HasIndex(p => p.Data);
        }
    }
}