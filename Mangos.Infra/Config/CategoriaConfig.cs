namespace Mangos.Infra.Config
{
    using Mangos.Dominio.Entities;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    public class CategoriaConfig : IEntityTypeConfiguration<Categoria>
    {
        public void Configure(EntityTypeBuilder<Categoria> builder)
        {
            builder.ToTable("Categoria");

            builder.HasKey(c => c.Id);

            builder.HasMany(c => c.CartoesCredito)
                .WithOne(c => c.Categoria)
                .HasForeignKey(c => c.CategoriaId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(c => c.CategoriasFilhas)
                .WithOne(c => c.CategoriaSuperior)
                .HasForeignKey(c => c.CategoriaSuperiorId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(c => c.ContasBancariasRendimentoMensal)
                .WithOne(c => c.CategoriaRendimentoMensal)
                .HasForeignKey(c => c.CategoriaRendimentoMensalId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(c => c.Lancamentos)
                .WithOne(c => c.Categoria)
                .HasForeignKey(c => c.CategoriaId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(c => c.LancamentosCartao)
                .WithOne(c => c.Categoria)
                .HasForeignKey(c => c.CategoriaId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(c => c.LancamentosFixos)
                .WithOne(c => c.Categoria)
                .HasForeignKey(c => c.CategoriaId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(c => c.PessoasPadraoDespesa)
                .WithOne(c => c.CategoriaPadraoDespesa)
                .HasForeignKey(c => c.CategoriaPadraoDespesaId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(c => c.PessoasPadraoReceita)
                .WithOne(c => c.CategoriaPadraoReceita)
                .HasForeignKey(c => c.CategoriaPadraoReceitaId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Property(p => p.Descricao)
                .HasMaxLength(100)
                .IsRequired();
        }
    }
}