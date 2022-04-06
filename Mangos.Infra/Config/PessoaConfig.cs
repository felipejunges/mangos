namespace Mangos.Infra.Config
{
    using Mangos.Dominio.Entities;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    public class PessoaConfig : IEntityTypeConfiguration<Pessoa>
    {
        public void Configure(EntityTypeBuilder<Pessoa> builder)
        {
            builder.ToTable("Pessoa");

            builder.HasKey(p => p.Id);

            builder.HasMany(c => c.Lancamentos)
                .WithOne(c => c.Pessoa)
                .HasForeignKey(c => c.PessoaId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(c => c.LancamentosCartao)
                .WithOne(c => c.Pessoa)
                .HasForeignKey(c => c.PessoaId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(c => c.LancamentosFixos)
                .WithOne(c => c.Pessoa)
                .HasForeignKey(c => c.PessoaId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(c => c.PessoaCoordenadas)
                .WithOne(c => c.Pessoa)
                .HasForeignKey(c => c.PessoaId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Property(p => p.Nome)
                .HasMaxLength(100)
                .IsRequired();

            builder.Property(p => p.Telefone1)
                .HasMaxLength(20);

            builder.Property(p => p.Telefone2)
                .HasMaxLength(20);

            builder.Property(p => p.Telefone3)
                .HasMaxLength(20);

            builder.Property(p => p.Site)
                .HasMaxLength(100);

            builder.Property(p => p.Email)
                .HasMaxLength(100);
        }
    }
}