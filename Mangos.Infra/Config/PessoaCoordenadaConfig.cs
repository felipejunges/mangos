namespace Mangos.Infra.Config
{
    using Mangos.Dominio.Entities;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    public class PessoaCoordenadaConfig : IEntityTypeConfiguration<PessoaCoordenada>
    {
        public void Configure(EntityTypeBuilder<PessoaCoordenada> builder)
        {
            builder.ToTable("PessoaCoordenada");

            builder.HasKey(p => p.Id);

            builder.Property(c => c.Latitude)
                .HasColumnType("decimal(11,8)");

            builder.Property(c => c.Longitude)
                .HasColumnType("decimal(11,8)");

            builder.Property(c => c.Observacao)
                .HasMaxLength(100);
        }   
    }
}