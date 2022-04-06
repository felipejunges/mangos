namespace Mangos.Infra.Config
{
    using Mangos.Dominio.Entities;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    public class MetaSaldoConfig : IEntityTypeConfiguration<MetaSaldo>
    {
        public void Configure(EntityTypeBuilder<MetaSaldo> builder)
        {
            builder.ToTable("MetaSaldo");

            builder.HasKey(m => m.Id);

            builder.Property(p => p.Valor)
                .HasColumnType("money");
        }
    }
}