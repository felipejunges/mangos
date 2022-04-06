namespace Mangos.Dominio.Models.Relatorios
{
    public class ItemRelatorioBurnupModel
    {
        public string Legenda { get; set; }
        public double ValorProjecao { get; set; }
        public double ValorRealizacao { get; set; }

        public ItemRelatorioBurnupModel(string legenda, double valorProjecao, double valorRealizacao)
        {
            Legenda = legenda;
            ValorProjecao = valorProjecao;
            ValorRealizacao = valorRealizacao;
        }
    }
}