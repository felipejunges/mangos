namespace Mangos.Infra.Config
{
    using Mangos.Dominio.Entities;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    public class SessaoAcessoConfig : IEntityTypeConfiguration<SessaoAcesso>
    {
        public void Configure(EntityTypeBuilder<SessaoAcesso> builder)
        {
            builder.ToTable("SessaoAcesso");

            builder.HasKey(s => s.Id);

            builder.Property(p => p.Chave)
                .HasMaxLength(100)
                .IsRequired();

            builder.Property(p => p.Ip)
                .HasColumnName("IP")
                .HasMaxLength(50);

            builder.Property(p => p.Browser)
                .HasMaxLength(100);

            builder.Property(p => p.UserAgent)
                .HasMaxLength(255);

            builder.HasIndex(p => p.Chave);
        }
    }
}