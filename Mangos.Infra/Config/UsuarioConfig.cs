namespace Mangos.Infra.Config
{
    using Mangos.Dominio.Entities;
    using Mangos.Dominio.ValueObjects;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    public class UsuarioConfig : IEntityTypeConfiguration<Usuario>
    {
        public void Configure(EntityTypeBuilder<Usuario> builder)
        {
            builder.ToTable("Usuario");

            builder.HasKey(u => u.Id);

            builder.HasMany(c => c.SessoesAcesso)
                .WithOne(c => c.Usuario)
                .HasForeignKey(c => c.UsuarioId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(c => c.DispositivosConectados)
                .WithOne(c => c.Usuario)
                .HasForeignKey(c => c.UsuarioId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Property(c => c.Senha)
                .HasColumnName(nameof(Usuario.Senha))
                .HasMaxLength(100)
                .HasConversion(
                    x => x.Senha,
                    x => UsuarioSenhaVO.FromHashed(x)
                )
                .IsRequired();

            builder.Property(c => c.Nome)
                .HasMaxLength(100)
                .IsRequired();

            builder.Property(c => c.Email)
                .HasMaxLength(255)
                .IsRequired();

            builder.Property(c => c.Senha)
                .HasMaxLength(100)
                .IsRequired();

            builder.Property(c => c.TokenSenha)
                .HasMaxLength(12);
        }
    }
}