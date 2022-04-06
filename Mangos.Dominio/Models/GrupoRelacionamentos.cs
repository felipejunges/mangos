namespace Mangos.Dominio.Models
{
    public class GrupoRelacionamentos
    {
        public int Id { get; set; }

        public string Descricao { get; set; }

        public int Pessoas { get; set; }

        public int Usuarios { get; set; }

        public int Categorias { get; set; }

        public int ContasBancarias { get; set; }

        public int CartoesCredito { get; set; }

        public int Receitas { get; set; }

        public int Despesas { get; set; }

        public int LancamentosCartao { get; set; }

        public int TransferenciasConta { get; set; }

        public int LancamentosFixos { get; set; }

        public GrupoRelacionamentos(int id, string descricao, int pessoas, int usuarios, int categorias, int contasBancarias, int cartoesCredito, int receitas, int despesas, int lancamentosCartao, int transferenciasConta, int lancamentosFixos)
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