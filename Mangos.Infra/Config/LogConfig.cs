namespace Mangos.Infra.Config
{
    using Mangos.Dominio.Entities;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    public class LogConfig : IEntityTypeConfiguration<Log>
    {
        public void Configure(EntityTypeBuilder<Log> builder)
        {
            builder.ToTable("Log");

            builder.HasKey(l => l.Id);

            builder.Property(p => p.LogLevel)
                   .IsRequired()
                   .HasMaxLength(25);

            builder.Property(p => p.Aplicacao)
                   .HasMaxLength(100);

            builder.Property(p => p.CategoryName)
                   .HasMaxLength(100);

            builder.Property(p => p.Mensagem)
                   .IsRequired()
                   .HasColumnType("nvarchar(max)");

            builder.Property(p => p.Exception)
                   .HasColumnType("nvarchar(max)");
        }
    }
}