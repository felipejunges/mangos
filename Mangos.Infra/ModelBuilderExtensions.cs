using Mangos.Dominio.Entities;
using Microsoft.EntityFrameworkCore;

namespace Mangos.Infra
{
    public static class ModelBuilderExtensions
    {
        public static void Seed(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Grupo>().HasData(
                new Grupo() { Id = 1, Descricao = "Teste", MesesAntecedenciaGerarLancamento = 6, MesesAntecedenciaGerarLancamentoCartao = 0, MesesGraficosDashboard = 12 }
            );

            modelBuilder.Entity<Usuario>().HasData(
                new Usuario() {
                    Id = 1,
                    GrupoId = 1,
                    Nome = "Felipe Junges",
                    Email = "felipejunges@yahoo.com.br",
                    Senha = "123",
                    Admin = true,
                    Ativo = true
                }
            );
        }
    }
}