﻿// <auto-generated />
using System;
using Mangos.Infra;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Mangos.Infra.Migrations
{
    [DbContext(typeof(MangosDb))]
    [Migration("20201030232113_Grupo-MesesAntecedencia-LancamentoFixo")]
    partial class GrupoMesesAntecedenciaLancamentoFixo
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "3.1.7")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("Mangos.Dominio.Entities.CartaoCredito", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<bool>("Ativo")
                        .HasColumnType("bit");

                    b.Property<int?>("CategoriaId")
                        .HasColumnType("int");

                    b.Property<string>("Descricao")
                        .IsRequired()
                        .HasColumnType("nvarchar(100)")
                        .HasMaxLength(100);

                    b.Property<int>("DiaFechamento")
                        .HasColumnType("int");

                    b.Property<int>("DiaVencimento")
                        .HasColumnType("int");

                    b.Property<bool>("ExibirDadosRelatorio")
                        .HasColumnType("bit");

                    b.Property<bool>("GerarLancamentoFecharMes")
                        .HasColumnType("bit");

                    b.Property<int>("GrupoId")
                        .HasColumnType("int");

                    b.Property<int>("OffsetReferenciaVencimento")
                        .HasColumnType("int");

                    b.Property<decimal>("ValorLimite")
                        .HasColumnType("money");

                    b.HasKey("Id");

                    b.HasIndex("CategoriaId");

                    b.HasIndex("GrupoId");

                    b.ToTable("CartaoCredito");
                });

            modelBuilder.Entity("Mangos.Dominio.Entities.Categoria", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<bool>("Ativo")
                        .HasColumnType("bit");

                    b.Property<int?>("CategoriaSuperiorId")
                        .HasColumnType("int");

                    b.Property<string>("Descricao")
                        .IsRequired()
                        .HasColumnType("nvarchar(100)")
                        .HasMaxLength(100);

                    b.Property<bool>("Despesa")
                        .HasColumnType("bit");

                    b.Property<int>("GrupoId")
                        .HasColumnType("int");

                    b.Property<bool>("Receita")
                        .HasColumnType("bit");

                    b.Property<bool>("RelatorioTotal")
                        .HasColumnType("bit");

                    b.HasKey("Id");

                    b.HasIndex("CategoriaSuperiorId");

                    b.HasIndex("GrupoId");

                    b.ToTable("Categoria");
                });

            modelBuilder.Entity("Mangos.Dominio.Entities.ContaBancaria", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Agencia")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("Ativo")
                        .HasColumnType("bit");

                    b.Property<int?>("CategoriaRendimentoMensalId")
                        .HasColumnType("int");

                    b.Property<string>("Descricao")
                        .IsRequired()
                        .HasColumnType("nvarchar(100)")
                        .HasMaxLength(100);

                    b.Property<int>("GrupoId")
                        .HasColumnType("int");

                    b.Property<string>("NumeroBanco")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("NumeroConta")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("PularFinaisSemanaLancamentoRapido")
                        .HasColumnType("bit");

                    b.Property<bool>("RelatorioProjecaoSaldo")
                        .HasColumnType("bit");

                    b.Property<decimal>("SaldoInicial")
                        .HasColumnType("money");

                    b.HasKey("Id");

                    b.HasIndex("CategoriaRendimentoMensalId");

                    b.HasIndex("GrupoId");

                    b.ToTable("ContaBancaria");
                });

            modelBuilder.Entity("Mangos.Dominio.Entities.Grupo", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Descricao")
                        .IsRequired()
                        .HasColumnType("nvarchar(100)")
                        .HasMaxLength(100);

                    b.Property<int>("MesesAntecedenciaGerarLancamento")
                        .HasColumnType("int");

                    b.Property<int>("MesesAntecedenciaGerarLancamentoCartao")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.ToTable("Grupo");
                });

            modelBuilder.Entity("Mangos.Dominio.Entities.Lancamento", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int?>("CategoriaId")
                        .HasColumnType("int");

                    b.Property<int?>("ContaBancariaId")
                        .HasColumnType("int");

                    b.Property<DateTime?>("DataContaBancaria")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("DataHoraCadastro")
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("DataPagamento")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("DataVencimento")
                        .HasColumnType("datetime2");

                    b.Property<string>("Descricao")
                        .IsRequired()
                        .HasColumnType("nvarchar(255)")
                        .HasMaxLength(255);

                    b.Property<int>("GrupoId")
                        .HasColumnType("int");

                    b.Property<int?>("LancamentoFixoOrigemId")
                        .HasColumnType("int");

                    b.Property<int>("NumeroParcela")
                        .HasColumnType("int");

                    b.Property<string>("Observacoes")
                        .HasColumnType("varchar(MAX)");

                    b.Property<bool>("Pago")
                        .HasColumnType("bit");

                    b.Property<int?>("PessoaId")
                        .HasColumnType("int");

                    b.Property<int>("Tipo")
                        .HasColumnType("int");

                    b.Property<int>("TotalParcelas")
                        .HasColumnType("int");

                    b.Property<decimal>("Valor")
                        .HasColumnType("money");

                    b.Property<decimal?>("ValorPago")
                        .HasColumnType("money");

                    b.HasKey("Id");

                    b.HasIndex("CategoriaId");

                    b.HasIndex("ContaBancariaId");

                    b.HasIndex("GrupoId");

                    b.HasIndex("LancamentoFixoOrigemId");

                    b.HasIndex("PessoaId");

                    b.ToTable("Lancamento");
                });

            modelBuilder.Entity("Mangos.Dominio.Entities.LancamentoCartao", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("CartaoCreditoId")
                        .HasColumnType("int");

                    b.Property<int?>("CategoriaId")
                        .HasColumnType("int");

                    b.Property<DateTime>("DataHoraCadastro")
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("DataHoraGeracaoLancamento")
                        .HasColumnType("datetime2");

                    b.Property<string>("Descricao")
                        .IsRequired()
                        .HasColumnType("nvarchar(255)")
                        .HasMaxLength(255);

                    b.Property<bool>("GeradoLancamento")
                        .HasColumnType("bit");

                    b.Property<int>("GrupoId")
                        .HasColumnType("int");

                    b.Property<int?>("LancamentoFixoOrigemId")
                        .HasColumnType("int");

                    b.Property<int?>("LancamentoId")
                        .HasColumnType("int");

                    b.Property<DateTime>("MesReferencia")
                        .HasColumnType("datetime2");

                    b.Property<int>("NumeroParcela")
                        .HasColumnType("int");

                    b.Property<string>("Observacoes")
                        .HasColumnType("varchar(MAX)");

                    b.Property<int?>("PessoaId")
                        .HasColumnType("int");

                    b.Property<int>("TipoLancamento")
                        .HasColumnType("int");

                    b.Property<int>("TotalParcelas")
                        .HasColumnType("int");

                    b.Property<decimal>("Valor")
                        .HasColumnType("money");

                    b.HasKey("Id");

                    b.HasIndex("CartaoCreditoId");

                    b.HasIndex("CategoriaId");

                    b.HasIndex("GrupoId");

                    b.HasIndex("LancamentoFixoOrigemId");

                    b.HasIndex("LancamentoId");

                    b.HasIndex("PessoaId");

                    b.ToTable("LancamentoCartao");
                });

            modelBuilder.Entity("Mangos.Dominio.Entities.LancamentoFixo", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<bool>("Ativo")
                        .HasColumnType("bit");

                    b.Property<int?>("CartaoCreditoId")
                        .HasColumnType("int");

                    b.Property<int?>("CategoriaId")
                        .HasColumnType("int");

                    b.Property<DateTime?>("DataHoraUltimaGeracao")
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("DataUltimoVencimento")
                        .HasColumnType("datetime2");

                    b.Property<string>("Descricao")
                        .IsRequired()
                        .HasColumnType("nvarchar(255)")
                        .HasMaxLength(255);

                    b.Property<int>("DiaVencimento")
                        .HasColumnType("int");

                    b.Property<int>("GrupoId")
                        .HasColumnType("int");

                    b.Property<int?>("MesVencimento")
                        .HasColumnType("int");

                    b.Property<int>("Periodicidade")
                        .HasColumnType("int");

                    b.Property<int?>("PessoaId")
                        .HasColumnType("int");

                    b.Property<int>("Tipo")
                        .HasColumnType("int");

                    b.Property<decimal>("Valor")
                        .HasColumnType("money");

                    b.HasKey("Id");

                    b.HasIndex("CartaoCreditoId");

                    b.HasIndex("CategoriaId");

                    b.HasIndex("GrupoId");

                    b.HasIndex("PessoaId");

                    b.ToTable("LancamentoFixo");
                });

            modelBuilder.Entity("Mangos.Dominio.Entities.Log", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("CategoryName")
                        .HasColumnType("nvarchar(100)")
                        .HasMaxLength(100);

                    b.Property<DateTime>("DataHora")
                        .HasColumnType("datetime2");

                    b.Property<string>("Exception")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("LogLevel")
                        .IsRequired()
                        .HasColumnType("nvarchar(25)")
                        .HasMaxLength(25);

                    b.Property<string>("Mensagem")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Log");
                });

            modelBuilder.Entity("Mangos.Dominio.Entities.MetaMovimentacao", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("GrupoId")
                        .HasColumnType("int");

                    b.Property<DateTime>("MesFinal")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("MesInicial")
                        .HasColumnType("datetime2");

                    b.Property<decimal>("ValorMensal")
                        .HasColumnType("money");

                    b.HasKey("Id");

                    b.HasIndex("GrupoId");

                    b.ToTable("MetaMovimentacao");
                });

            modelBuilder.Entity("Mangos.Dominio.Entities.MetaSaldo", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("GrupoId")
                        .HasColumnType("int");

                    b.Property<DateTime>("Mes")
                        .HasColumnType("datetime2");

                    b.Property<decimal>("Valor")
                        .HasColumnType("money");

                    b.HasKey("Id");

                    b.HasIndex("GrupoId");

                    b.ToTable("MetaSaldo");
                });

            modelBuilder.Entity("Mangos.Dominio.Entities.Pessoa", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<bool>("Ativo")
                        .HasColumnType("bit");

                    b.Property<int?>("CategoriaPadraoDespesaId")
                        .HasColumnType("int");

                    b.Property<int?>("CategoriaPadraoReceitaId")
                        .HasColumnType("int");

                    b.Property<string>("Email")
                        .HasColumnType("nvarchar(100)")
                        .HasMaxLength(100);

                    b.Property<int>("GrupoId")
                        .HasColumnType("int");

                    b.Property<string>("Nome")
                        .IsRequired()
                        .HasColumnType("nvarchar(100)")
                        .HasMaxLength(100);

                    b.Property<string>("Site")
                        .HasColumnType("nvarchar(100)")
                        .HasMaxLength(100);

                    b.Property<string>("Telefone1")
                        .HasColumnType("nvarchar(20)")
                        .HasMaxLength(20);

                    b.Property<string>("Telefone2")
                        .HasColumnType("nvarchar(20)")
                        .HasMaxLength(20);

                    b.Property<string>("Telefone3")
                        .HasColumnType("nvarchar(20)")
                        .HasMaxLength(20);

                    b.Property<int>("Tipo")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("CategoriaPadraoDespesaId");

                    b.HasIndex("CategoriaPadraoReceitaId");

                    b.HasIndex("GrupoId");

                    b.ToTable("Pessoa");
                });

            modelBuilder.Entity("Mangos.Dominio.Entities.PessoaCoordenada", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Descricao")
                        .HasColumnType("nvarchar(max)");

                    b.Property<decimal>("Latitude")
                        .HasColumnType("decimal(11,8)");

                    b.Property<decimal>("Longitude")
                        .HasColumnType("decimal(11,8)");

                    b.Property<int>("PessoaId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("PessoaId");

                    b.ToTable("PessoaCoordenada");
                });

            modelBuilder.Entity("Mangos.Dominio.Entities.RendimentoMensalConta", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("ContaBancariaId")
                        .HasColumnType("int");

                    b.Property<DateTime>("DataHoraCadastro")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("MesReferencia")
                        .HasColumnType("datetime2");

                    b.Property<decimal>("Valor")
                        .HasColumnType("money");

                    b.HasKey("Id");

                    b.HasIndex("ContaBancariaId");

                    b.ToTable("RendimentoMensalConta");
                });

            modelBuilder.Entity("Mangos.Dominio.Entities.SaldoContaBancaria", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("ContaBancariaId")
                        .HasColumnType("int");

                    b.Property<DateTime>("Data")
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("DataHoraConferenciaSaldo")
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("DataHoraFechamento")
                        .HasColumnType("datetime2");

                    b.Property<bool>("Fechado")
                        .HasColumnType("bit");

                    b.Property<decimal?>("ValorConferenciaSaldo")
                        .HasColumnType("money");

                    b.Property<decimal>("ValorMovimentacoes")
                        .HasColumnType("money");

                    b.Property<decimal>("ValorSaldo")
                        .HasColumnType("money");

                    b.HasKey("Id");

                    b.HasIndex("ContaBancariaId");

                    b.HasIndex("Data");

                    b.ToTable("SaldoContaBancaria");
                });

            modelBuilder.Entity("Mangos.Dominio.Entities.SessaoAcesso", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Browser")
                        .HasColumnType("nvarchar(100)")
                        .HasMaxLength(100);

                    b.Property<string>("Chave")
                        .IsRequired()
                        .HasColumnType("nvarchar(100)")
                        .HasMaxLength(100);

                    b.Property<DateTime>("DataHoraAtualizacao")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("DataHoraCriacao")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("DataHoraExpiracao")
                        .HasColumnType("datetime2");

                    b.Property<string>("IP")
                        .HasColumnType("nvarchar(50)")
                        .HasMaxLength(50);

                    b.Property<bool>("IsMobile")
                        .HasColumnType("bit");

                    b.Property<bool>("Logout")
                        .HasColumnType("bit");

                    b.Property<bool>("Persistente")
                        .HasColumnType("bit");

                    b.Property<string>("UserAgent")
                        .HasColumnType("nvarchar(255)")
                        .HasMaxLength(255);

                    b.Property<int>("UsuarioId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("Chave");

                    b.HasIndex("UsuarioId");

                    b.ToTable("SessaoAcesso");
                });

            modelBuilder.Entity("Mangos.Dominio.Entities.TransferenciaConta", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int?>("ContaBancariaDestinoId")
                        .HasColumnType("int");

                    b.Property<int?>("ContaBancariaOrigemId")
                        .HasColumnType("int");

                    b.Property<DateTime?>("DataCredito")
                        .HasColumnType("datetime");

                    b.Property<DateTime?>("DataDebito")
                        .HasColumnType("datetime");

                    b.Property<DateTime>("DataHoraCadastro")
                        .HasColumnType("datetime2");

                    b.Property<string>("Descricao")
                        .IsRequired()
                        .HasColumnType("nvarchar(255)")
                        .HasMaxLength(255);

                    b.Property<int>("GrupoId")
                        .HasColumnType("int");

                    b.Property<decimal>("Valor")
                        .HasColumnType("money");

                    b.HasKey("Id");

                    b.HasIndex("ContaBancariaDestinoId");

                    b.HasIndex("ContaBancariaOrigemId");

                    b.HasIndex("GrupoId");

                    b.ToTable("TransferenciaConta");
                });

            modelBuilder.Entity("Mangos.Dominio.Entities.Usuario", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<bool>("Admin")
                        .HasColumnType("bit");

                    b.Property<bool>("Ativo")
                        .HasColumnType("bit");

                    b.Property<int>("DiasAlertaVencimentos")
                        .HasColumnType("int");

                    b.Property<int>("DiasEmailVencimentos")
                        .HasColumnType("int");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("nvarchar(255)")
                        .HasMaxLength(255);

                    b.Property<int>("GrupoId")
                        .HasColumnType("int");

                    b.Property<string>("Nome")
                        .IsRequired()
                        .HasColumnType("nvarchar(100)")
                        .HasMaxLength(100);

                    b.Property<int>("TipoAlertaVencimentos")
                        .HasColumnType("int");

                    b.Property<int>("TipoEmailVencimentos")
                        .HasColumnType("int");

                    b.Property<string>("TokenSenha")
                        .HasColumnType("nvarchar(12)")
                        .HasMaxLength(12);

                    b.Property<DateTime?>("ValidadeTokenSenha")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.HasIndex("GrupoId");

                    b.ToTable("Usuario");
                });

            modelBuilder.Entity("Mangos.Dominio.Entities.CartaoCredito", b =>
                {
                    b.HasOne("Mangos.Dominio.Entities.Categoria", "Categoria")
                        .WithMany("CartoesCredito")
                        .HasForeignKey("CategoriaId")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.HasOne("Mangos.Dominio.Entities.Grupo", "Grupo")
                        .WithMany("CartoesCredito")
                        .HasForeignKey("GrupoId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();
                });

            modelBuilder.Entity("Mangos.Dominio.Entities.Categoria", b =>
                {
                    b.HasOne("Mangos.Dominio.Entities.Categoria", "CategoriaSuperior")
                        .WithMany("CategoriasFilhas")
                        .HasForeignKey("CategoriaSuperiorId")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.HasOne("Mangos.Dominio.Entities.Grupo", "Grupo")
                        .WithMany("Categorias")
                        .HasForeignKey("GrupoId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();
                });

            modelBuilder.Entity("Mangos.Dominio.Entities.ContaBancaria", b =>
                {
                    b.HasOne("Mangos.Dominio.Entities.Categoria", "CategoriaRendimentoMensal")
                        .WithMany("ContasBancariasRendimentoMensal")
                        .HasForeignKey("CategoriaRendimentoMensalId")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.HasOne("Mangos.Dominio.Entities.Grupo", "Grupo")
                        .WithMany("ContasBancarias")
                        .HasForeignKey("GrupoId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();
                });

            modelBuilder.Entity("Mangos.Dominio.Entities.Lancamento", b =>
                {
                    b.HasOne("Mangos.Dominio.Entities.Categoria", "Categoria")
                        .WithMany("Lancamentos")
                        .HasForeignKey("CategoriaId")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.HasOne("Mangos.Dominio.Entities.ContaBancaria", "ContaBancaria")
                        .WithMany("Lancamentos")
                        .HasForeignKey("ContaBancariaId")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.HasOne("Mangos.Dominio.Entities.Grupo", "Grupo")
                        .WithMany("Lancamentos")
                        .HasForeignKey("GrupoId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("Mangos.Dominio.Entities.LancamentoFixo", "LancamentoFixoOrigem")
                        .WithMany("LancamentosOrigem")
                        .HasForeignKey("LancamentoFixoOrigemId")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.HasOne("Mangos.Dominio.Entities.Pessoa", "Pessoa")
                        .WithMany("Lancamentos")
                        .HasForeignKey("PessoaId")
                        .OnDelete(DeleteBehavior.Restrict);
                });

            modelBuilder.Entity("Mangos.Dominio.Entities.LancamentoCartao", b =>
                {
                    b.HasOne("Mangos.Dominio.Entities.CartaoCredito", "CartaoCredito")
                        .WithMany("LancamentosCartao")
                        .HasForeignKey("CartaoCreditoId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("Mangos.Dominio.Entities.Categoria", "Categoria")
                        .WithMany("LancamentosCartao")
                        .HasForeignKey("CategoriaId")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.HasOne("Mangos.Dominio.Entities.Grupo", "Grupo")
                        .WithMany("LancamentosCartao")
                        .HasForeignKey("GrupoId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("Mangos.Dominio.Entities.LancamentoFixo", "LancamentoFixoOrigem")
                        .WithMany("LancamentosCartaoOrigem")
                        .HasForeignKey("LancamentoFixoOrigemId")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.HasOne("Mangos.Dominio.Entities.Lancamento", "Lancamento")
                        .WithMany("LancamentosCartao")
                        .HasForeignKey("LancamentoId")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.HasOne("Mangos.Dominio.Entities.Pessoa", "Pessoa")
                        .WithMany("LancamentosCartao")
                        .HasForeignKey("PessoaId")
                        .OnDelete(DeleteBehavior.Restrict);
                });

            modelBuilder.Entity("Mangos.Dominio.Entities.LancamentoFixo", b =>
                {
                    b.HasOne("Mangos.Dominio.Entities.CartaoCredito", "CartaoCredito")
                        .WithMany("LancamentosFixos")
                        .HasForeignKey("CartaoCreditoId")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.HasOne("Mangos.Dominio.Entities.Categoria", "Categoria")
                        .WithMany("LancamentosFixos")
                        .HasForeignKey("CategoriaId")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.HasOne("Mangos.Dominio.Entities.Grupo", "Grupo")
                        .WithMany("LancamentosFixos")
                        .HasForeignKey("GrupoId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("Mangos.Dominio.Entities.Pessoa", "Pessoa")
                        .WithMany("LancamentosFixos")
                        .HasForeignKey("PessoaId")
                        .OnDelete(DeleteBehavior.Restrict);
                });

            modelBuilder.Entity("Mangos.Dominio.Entities.MetaMovimentacao", b =>
                {
                    b.HasOne("Mangos.Dominio.Entities.Grupo", "Grupo")
                        .WithMany("MetasMovimentacao")
                        .HasForeignKey("GrupoId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();
                });

            modelBuilder.Entity("Mangos.Dominio.Entities.MetaSaldo", b =>
                {
                    b.HasOne("Mangos.Dominio.Entities.Grupo", "Grupo")
                        .WithMany("MetasSaldo")
                        .HasForeignKey("GrupoId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();
                });

            modelBuilder.Entity("Mangos.Dominio.Entities.Pessoa", b =>
                {
                    b.HasOne("Mangos.Dominio.Entities.Categoria", "CategoriaPadraoDespesa")
                        .WithMany("PessoasPadraoDespesa")
                        .HasForeignKey("CategoriaPadraoDespesaId")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.HasOne("Mangos.Dominio.Entities.Categoria", "CategoriaPadraoReceita")
                        .WithMany("PessoasPadraoReceita")
                        .HasForeignKey("CategoriaPadraoReceitaId")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.HasOne("Mangos.Dominio.Entities.Grupo", "Grupo")
                        .WithMany("Pessoas")
                        .HasForeignKey("GrupoId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();
                });

            modelBuilder.Entity("Mangos.Dominio.Entities.PessoaCoordenada", b =>
                {
                    b.HasOne("Mangos.Dominio.Entities.Pessoa", "Pessoa")
                        .WithMany("PessoaCoordenadas")
                        .HasForeignKey("PessoaId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();
                });

            modelBuilder.Entity("Mangos.Dominio.Entities.RendimentoMensalConta", b =>
                {
                    b.HasOne("Mangos.Dominio.Entities.ContaBancaria", "ContaBancaria")
                        .WithMany("RendimentosMensalConta")
                        .HasForeignKey("ContaBancariaId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();
                });

            modelBuilder.Entity("Mangos.Dominio.Entities.SaldoContaBancaria", b =>
                {
                    b.HasOne("Mangos.Dominio.Entities.ContaBancaria", "ContaBancaria")
                        .WithMany("SaldosContasBancarias")
                        .HasForeignKey("ContaBancariaId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();
                });

            modelBuilder.Entity("Mangos.Dominio.Entities.SessaoAcesso", b =>
                {
                    b.HasOne("Mangos.Dominio.Entities.Usuario", "Usuario")
                        .WithMany("SessoesAcesso")
                        .HasForeignKey("UsuarioId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Mangos.Dominio.Entities.TransferenciaConta", b =>
                {
                    b.HasOne("Mangos.Dominio.Entities.ContaBancaria", "ContaBancariaDestino")
                        .WithMany("TransferenciasContaDestino")
                        .HasForeignKey("ContaBancariaDestinoId")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.HasOne("Mangos.Dominio.Entities.ContaBancaria", "ContaBancariaOrigem")
                        .WithMany("TransferenciasContaOrigem")
                        .HasForeignKey("ContaBancariaOrigemId")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.HasOne("Mangos.Dominio.Entities.Grupo", "Grupo")
                        .WithMany("TransferenciasConta")
                        .HasForeignKey("GrupoId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();
                });

            modelBuilder.Entity("Mangos.Dominio.Entities.Usuario", b =>
                {
                    b.HasOne("Mangos.Dominio.Entities.Grupo", "Grupo")
                        .WithMany("Usuarios")
                        .HasForeignKey("GrupoId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.OwnsOne("Mangos.Dominio.ValueObjects.UsuarioSenhaVO", "Senha", b1 =>
                        {
                            b1.Property<int>("UsuarioId")
                                .ValueGeneratedOnAdd()
                                .HasColumnType("int")
                                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                            b1.Property<string>("Senha")
                                .IsRequired()
                                .HasColumnName("Senha")
                                .HasColumnType("nvarchar(100)");

                            b1.HasKey("UsuarioId");

                            b1.ToTable("Usuario");

                            b1.WithOwner()
                                .HasForeignKey("UsuarioId");
                        });
                });
#pragma warning restore 612, 618
        }
    }
}
