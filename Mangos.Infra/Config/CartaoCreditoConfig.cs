namespace Mangos.Infra.Config
{
    using Mangos.Dominio.Entities;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    public class CartaoCreditoConfig : IEntityTypeConfiguration<CartaoCredito>
    {
        public void Configure(EntityTypeBuilder<CartaoCredito> builder)
        {
            builder.ToTable("CartaoCredito");

            builder.HasKey(c => c.Id);

            builder.HasMany(c => c.LancamentosCartao)
                   .WithOne(c => c.CartaoCredito)
                   .HasForeignKey(c => c.CartaoCreditoId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(c => c.LancamentosFixos)
                   .WithOne(c => c.CartaoCredito)
                   .HasForeignKey(c => c.CartaoCreditoId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.Property(p => p.Descricao)
                .HasMaxLength(100)
                .IsRequired();

            builder.Property(p => p.ValorLimite)
                .HasColumnType("money");
        }
    }
}