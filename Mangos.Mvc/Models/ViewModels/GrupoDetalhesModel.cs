using System.ComponentModel.DataAnnotations;

namespace Mangos.Mvc.Models.ViewModels
{
    public class GrupoDetalhesModel
    {
        public int Id { get; set; }

        [Display(Name = "Descrição")]
        public string Descricao { get; set; }

        public int Pessoas { get; set; }

        [Display(Name = "Usuários")]
        public int Usuarios { get; set; }

        public int Categorias { get; set; }

        [Display(Name = "Contas bancárias")]
        public int ContasBancarias { get; set; }

        [Display(Name = "Cartões crédito")]
        public int CartoesCredito { get; set; }

        public int Receitas { get; set; }

        public int Despesas { get; set; }

        [Display(Name = "Lançamentos cartão")]
        public int LancamentosCartao { get; set; }

        [Display(Name = "Transferências conta")]
        public int TransferenciasConta { get; set; }

        [Display(Name = "Lançamentos fixos")]
        public int LancamentosFixos { get; set; }

        public GrupoDetalhesModel(int id, string descricao, int pessoas, int usuarios, int categorias, int contasBancarias, int cartoesCredito, int receitas, int despesas, int lancamentosCartao, int transferenciasConta, int lancamentosFixos)
        {
            Id = id;
            Descricao = descricao;
            Pessoas = pessoas;
            Usuarios = usuarios;
            Categorias = categorias;
            ContasBancarias = contasBancarias;
            CartoesCredito = cartoesCredito;
            Receitas = receitas;
            Despesas = despesas;
            LancamentosCartao = lancamentosCartao;
            TransferenciasConta = transferenciasConta;
            LancamentosFixos = lancamentosFixos;
        }
    }
}