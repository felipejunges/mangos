namespace Mangos.Dominio.Services.User
{
    public interface IUserResolverService
    {
        bool Logado { get; }
        int UsuarioId { get; }
        int GrupoId { get; }
        string Nome { get; }
        string ChaveSessao { get; }
    }
}