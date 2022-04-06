using Mangos.Dominio.Entities;
using Mangos.Infra.Config;
using Microsoft.EntityFrameworkCore;

namespace Mangos.Infra
{
    public class MangosDb : DbContext
    {
        public MangosDb(DbContextOptions<MangosDb> options) : base(options) { }

        public DbSet<Categoria> Categorias => Set<Categoria>();
        public DbSet<CartaoCredito> CartoesCredito => Set<CartaoCredito>();
        public DbSet<ContaBancaria> ContasBancarias => Set<ContaBancaria>();
        public DbSet<DispositivoConectado> DispositivosConectados => Set<DispositivoConectado>();
        public DbSet<Grupo> Grupos => Set<Grupo>();
        public DbSet<Lancamento> Lancamentos => Set<Lancamento>();
        public DbSet<LancamentoCartao> LancamentosCartao => Set<LancamentoCartao>();
        public DbSet<LancamentoFixo> LancamentosFixos => Set<LancamentoFixo>();
        public DbSet<Log> Logs => Set<Log>();
        public DbSet<MetaMovimentacao> MetasMovimentacao => Set<MetaMovimentacao>();
        public DbSet<MetaSaldo> MetasSaldo => Set<MetaSaldo>();
        public DbSet<Pessoa> Pessoas => Set<Pessoa>();
        public DbSet<PessoaCoordenada> PessoasCoordenadas => Set<PessoaCoordenada>();
        public DbSet<RendimentoMensalConta> RendimentosMensaisContas => Set<RendimentoMensalConta>();
        public DbSet<SaldoContaBancaria> SaldosContasBancarias => Set<SaldoContaBancaria>();
        public DbSet<SessaoAcesso> SessoesAcesso => Set<SessaoAcesso>();
        public DbSet<TransferenciaConta> TransferenciasContas => Set<TransferenciaConta>();
        public DbSet<Usuario> Usuarios => Set<Usuario>();

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new CartaoCreditoConfig());
            modelBuilder.ApplyConfiguration(new CategoriaConfig());
            modelBuilder.ApplyConfiguration(new ContaBancariaConfig());
            modelBuilder.ApplyConfiguration(new DispositivoConectadoConfig());
            modelBuilder.ApplyConfiguration(new GrupoConfig());
            modelBuilder.ApplyConfiguration(new LancamentoCartaoConfig());
            modelBuilder.ApplyConfiguration(new LancamentoConfig());
            modelBuilder.ApplyConfiguration(new LancamentoFixoConfig());
            modelBuilder.ApplyConfiguration(new LogConfig());
            modelBuilder.ApplyConfiguration(new MetaMovimentacaoConfig());
            modelBuilder.ApplyConfiguration(new MetaSaldoConfig());
            modelBuilder.ApplyConfiguration(new PessoaConfig());
            modelBuilder.ApplyConfiguration(new PessoaCoordenadaConfig());
            modelBuilder.ApplyConfiguration(new RendimentoMensalContaConfig());
            modelBuilder.ApplyConfiguration(new SaldoContaBancariaConfig());
            modelBuilder.ApplyConfiguration(new SessaoAcessoConfig());
            modelBuilder.ApplyConfiguration(new TransferenciaContaConfig());
            modelBuilder.ApplyConfiguration(new UsuarioConfig());

            base.OnModelCreating(modelBuilder);
        }
    }
}