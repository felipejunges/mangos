namespace Mangos.Infra.Config
{
    using Mangos.Dominio.Entities;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    public class ContaBancariaConfig : IEntityTypeConfiguration<ContaBancaria>
    {
        public void Configure(EntityTypeBuilder<ContaBancaria> builder)
        {
            builder.ToTable("ContaBancaria");

            builder.HasKey(c => c.Id);

            builder
                .HasMany(c => c.Lancamentos)
                .WithOne(c => c.ContaBancaria)
                .HasForeignKey(c => c.ContaBancariaId)
                .OnDelete(DeleteBehavior.Restrict);

            builder
                .HasMany(c => c.RendimentosMensalConta)
                .WithOne(c => c.ContaBancaria)
                .HasForeignKey(c => c.ContaBancariaId)
                .OnDelete(DeleteBehavior.Restrict);

            builder
                .HasMany(c => c.SaldosContasBancarias)
                .WithOne(c => c.ContaBancaria)
                .HasForeignKey(c => c.ContaBancariaId)
                .OnDelete(DeleteBehavior.Restrict);

            builder
                .HasMany(c => c.TransferenciasContaDestino)
                .WithOne(c => c.ContaBancariaDestino)
                .HasForeignKey(c => c.ContaBancariaDestinoId)
                .OnDelete(DeleteBehavior.Restrict);

            builder
                .HasMany(c => c.TransferenciasContaOrigem)
                .WithOne(c => c.ContaBancariaOrigem)
                .HasForeignKey(c => c.ContaBancariaOrigemId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Property(p => p.Descricao)
                .HasMaxLength(100)
                .IsRequired();

            builder.Property(p => p.NumeroBanco)
                .HasMaxLength(4);

            builder.Property(p => p.Agencia)
                .HasMaxLength(8);

            builder.Property(p => p.NumeroConta)
                .HasMaxLength(20);

            builder.Property(p => p.SaldoInicial)
                .HasColumnType("money");
        }
    }
}