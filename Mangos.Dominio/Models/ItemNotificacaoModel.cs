namespace Mangos.Dominio.Models
{
    public class ItemNotificacaoModel
    {
        public string Descricao { get; private set; }
        public string LinkMvc { get; private set; }

        public ItemNotificacaoModel(string descricao, string linkMvc)
        {
            Descricao = descricao;
            LinkMvc = linkMvc;
        }
    }
}