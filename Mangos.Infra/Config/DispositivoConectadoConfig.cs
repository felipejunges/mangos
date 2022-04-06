namespace Mangos.Infra.Config
{
    using Mangos.Dominio.Entities;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    public class DispositivoConectadoConfig : IEntityTypeConfiguration<DispositivoConectado>
    {
        public void Configure(EntityTypeBuilder<DispositivoConectado> builder)
        {
            builder.ToTable("DispositivoConectado");

            builder.HasKey(d => d.Id);

            builder.Property(p => p.Identificador)
                .HasMaxLength(100)
                .IsRequired();

            builder.Property(p => p.RefreshToken)
                .HasMaxLength(100)
                .IsRequired();

            builder.Property(p => p.IP)
                .HasMaxLength(50);

            builder.Property(p => p.Sistema)
                .HasMaxLength(100);
            
            builder.HasIndex(p => p.Identificador);
        }
    }
}