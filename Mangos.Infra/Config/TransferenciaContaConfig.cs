namespace Mangos.Infra.Config
{
    using Mangos.Dominio.Entities;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    public class TransferenciaContaConfig : IEntityTypeConfiguration<TransferenciaConta>
    {
        public void Configure(EntityTypeBuilder<TransferenciaConta> builder)
        {
            builder.ToTable("TransferenciaConta");

            builder.HasKey(t => t.Id);

            builder.Property(p => p.Descricao)
                .HasMaxLength(255)
                .IsRequired();

            builder.Property(p => p.Valor)
                .HasColumnType("money");

            builder.Property(p => p.DataDebito)
                .HasColumnType("datetime");

            builder.Property(p => p.DataCredito)
                .HasColumnType("datetime");
        }
    }
}