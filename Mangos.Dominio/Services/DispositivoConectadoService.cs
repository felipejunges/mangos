using Mangos.Dominio.Entities;
using Mangos.Dominio.Interfaces.Repository;
using System;
using System.Threading.Tasks;

namespace Mangos.Dominio.Services
{
    public class DispositivoConectadoService
    {
        private readonly IDispositivoConectadoRepository _dispositivoConectadoRepository;

        public DispositivoConectadoService(IDispositivoConectadoRepository dispositivoConectadoRepository)
        {
            _dispositivoConectadoRepository = dispositivoConectadoRepository;
        }

        public async Task Incluir(int usuarioId, string identificador, string refreshToken, DateTime dataHoraExpiracao, string ip, string sistema)
        {
            var dispositivoConectado = new DispositivoConectado()
            {
                Id = 0,
                UsuarioId = usuarioId,
                Identificador = identificador,
                RefreshToken = refreshToken,
                DataHoraCriacao = DateTime.Now,
                DataHoraUltimoRefresh = DateTime.Now,
                DataHoraExpiracaoRefreshToken = dataHoraExpiracao,
                IP = ip,
                Sistema = sistema,
                Expirado = false
            };

            await _dispositivoConectadoRepository.IncluirAsync(dispositivoConectado);
        }

        public async Task Alterar(int usuarioId, string identificador, string refreshToken, DateTime dataHoraExpiracao)
        {
            var dispositivoConectado = await _dispositivoConectadoRepository.ObterDispositivoConectadoAsync(identificador, usuarioId);

            if (dispositivoConectado != null)
            {
                dispositivoConectado.RefreshToken = refreshToken;
                dispositivoConectado.DataHoraUltimoRefresh = DateTime.Now;
                dispositivoConectado.DataHoraExpiracaoRefreshToken = dataHoraExpiracao;

                await _dispositivoConectadoRepository.AlterarAsync(dispositivoConectado);
            }
        }
    }
}