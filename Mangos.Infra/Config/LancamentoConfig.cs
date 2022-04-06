namespace Mangos.Infra.Config
{
    using Mangos.Dominio.Entities;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    public class LancamentoConfig : IEntityTypeConfiguration<Lancamento>
    {
        public void Configure(EntityTypeBuilder<Lancamento> builder)
        {
            builder.ToTable("Lancamento");

            builder.HasKey(l => l.Id);

            builder.HasMany(c => c.LancamentosCartao)
                .WithOne(c => c.Lancamento)
                .HasForeignKey(c => c.LancamentoId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Property(p => p.Descricao)
                .HasMaxLength(255)
                .IsRequired();

            builder.Property(p => p.Valor)
                .HasColumnType("money");

            builder.Property(p => p.ValorPago)
                .HasColumnType("money");

            builder.Property(p => p.Observacoes)
                .HasColumnType("varchar(MAX)");
        }
    }
}