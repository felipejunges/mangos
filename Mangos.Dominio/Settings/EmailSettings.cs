namespace Mangos.Dominio.Settings
{
    public class EmailSettings
    {
        public bool Ativo { get; set; }
        public string ServidorSMTP { get; set; }
        public string UsuarioSMTP { get; set; }
        public string SenhaSMTP { get; set; }
        public int PortaSMTP { get; set; }
        public int Timeout { get; set; }
        public bool EnableSSL { get; set; }

        public EmailSettings()
        {
            ServidorSMTP = string.Empty;
            UsuarioSMTP = string.Empty;
            SenhaSMTP = string.Empty;
        }
    }
}