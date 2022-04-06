namespace Mangos.Infra.Config
{
    using Mangos.Dominio.Entities;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    public class GrupoConfig : IEntityTypeConfiguration<Grupo>
    {
        public void Configure(EntityTypeBuilder<Grupo> builder)
        {
            builder.ToTable("Grupo");

            builder.HasKey(g => g.Id);

            builder.HasMany(c => c.CartoesCredito)
                .WithOne(c => c.Grupo)
                .HasForeignKey(c => c.GrupoId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(c => c.Categorias)
                .WithOne(c => c.Grupo)
                .HasForeignKey(c => c.GrupoId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(c => c.ContasBancarias).WithOne(c => c.Grupo).HasForeignKey(c => c.GrupoId).OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(c => c.Lancamentos).WithOne(c => c.Grupo).HasForeignKey(c => c.GrupoId).OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(c => c.LancamentosCartao).WithOne(c => c.Grupo).HasForeignKey(c => c.GrupoId).OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(c => c.LancamentosFixos).WithOne(c => c.Grupo).HasForeignKey(c => c.GrupoId).OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(c => c.MetasMovimentacao)
                .WithOne(c => c.Grupo)
                .HasForeignKey(c => c.GrupoId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(c => c.MetasSaldo)
                .WithOne(c => c.Grupo)
                .HasForeignKey(c => c.GrupoId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(c => c.Pessoas).WithOne(c => c.Grupo).HasForeignKey(c => c.GrupoId).OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(c => c.TransferenciasConta).WithOne(c => c.Grupo).HasForeignKey(c => c.GrupoId).OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(c => c.Usuarios).WithOne(c => c.Grupo).HasForeignKey(c => c.GrupoId).OnDelete(DeleteBehavior.Restrict);

            builder.Property(p => p.Descricao)
                .HasMaxLength(100)
                .IsRequired();
        }
    }
}