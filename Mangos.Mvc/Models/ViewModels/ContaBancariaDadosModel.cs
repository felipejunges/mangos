using Mangos.Dominio.Entities;
using Mangos.Mvc.Models.ViewModels.Abstract;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace Mangos.Mvc.Models.ViewModels
{
    public class ContaBancariaDadosModel : HashedModel
    {
        [Display(Name = "Descrição")]
        public string? Descricao { get; set; }

        [Display(Name = "Banco")]
        public string? NumeroBanco { get; set; }

        [Display(Name = "Agência")]
        public string? Agencia { get; set; }

        [Display(Name = "Conta")]
        public string? NumeroConta { get; set; }

        [Display(Name = "Saldo inicial")]
        public decimal SaldoInicial { get; set; }

        [Display(Name = "Categoria rendimento mensal")]
        public int? CategoriaRendimentoMensalId { get; set; }

        [Display(Name = "Relatório projeção de saldo")]
        public bool RelatorioProjecaoSaldo { get; set; }

        [Display(Name = "Pular finais semana Lançamento Rápido")]
        public bool PularFinaisSemanaLancamentoRapido { get; set; }

        public bool Ativo { get; set; }

        public IEnumerable<Categoria> CategoriasRendimentoMensal { get; set; }

        public ContaBancariaDadosModel()
        {
            CategoriasRendimentoMensal = Enumerable.Empty<Categoria>();
        }

        public ContaBancariaDadosModel(int id, int grupoId, string? descricao, string? numeroBanco, string? agencia, string? numeroConta, decimal saldoInicial, int? categoriaRendimentoMensalId, bool relatorioProjecaoSaldo, bool pularFinaisSemanaLancamentoRapido, bool ativo, IEnumerable<Categoria> categoriasRendimentoMensal)
        {
            Id = id;
            GrupoId = grupoId;
            Descricao = descricao;
            NumeroBanco = numeroBanco;
            Agencia = agencia;
            NumeroConta = numeroConta;
            SaldoInicial = saldoInicial;
            CategoriaRendimentoMensalId = categoriaRendimentoMensalId;
            RelatorioProjecaoSaldo = relatorioProjecaoSaldo;
            PularFinaisSemanaLancamentoRapido = pularFinaisSemanaLancamentoRapido;
            Ativo = ativo;
            CategoriasRendimentoMensal = categoriasRendimentoMensal;

            SetValidationHash();
        }

        public static ContaBancariaDadosModel Novo(int grupoId, IEnumerable<Categoria> categoriasRendimentoMensal)
        {
            return new ContaBancariaDadosModel(
                id: 0,
                grupoId: grupoId,
                descricao: null,
                numeroBanco: null,
                agencia: null,
                numeroConta: null,
                saldoInicial: 0,
                categoriaRendimentoMensalId: null,
                relatorioProjecaoSaldo: true,
                pularFinaisSemanaLancamentoRapido: false,
                ativo: true,
                categoriasRendimentoMensal: categoriasRendimentoMensal
            );
        }

        public void AtualizarCategorias(IEnumerable<Categoria> categoriasRendimentoMensal)
        {
            CategoriasRendimentoMensal = categoriasRendimentoMensal;
        }
    }
}