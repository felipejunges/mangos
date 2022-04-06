﻿// <auto-generated />
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using System;

namespace Mangos.Infra.Migrations
{
    [DbContext(typeof(MangosDb))]
    [Migration("20181229005156_Removao-ParametrosSistema")]
    partial class RemovaoParametrosSistema
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.1.4-rtm-31024")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("Mangos.Dominio.Entities.CartaoCredito", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<bool>("Ativo");

                    b.Property<int?>("CategoriaId");

                    b.Property<DateTime?>("DataUltimoFechamento");

                    b.Property<string>("Descricao")
                        .IsRequired()
                        .HasMaxLength(100);

                    b.Property<int>("DiaFechamento");

                    b.Property<int>("DiaVencimento");

                    b.Property<int>("GrupoId");

                    b.HasKey("Id");

                    b.HasIndex("CategoriaId");

                    b.HasIndex("GrupoId");

                    b.ToTable("CartaoCredito");
                });

            modelBuilder.Entity("Mangos.Dominio.Entities.Categoria", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<bool>("Ativo");

                    b.Property<int?>("CategoriaSuperiorId");

                    b.Property<string>("Descricao")
                        .IsRequired()
                        .HasMaxLength(100);

                    b.Property<bool>("Despesa");

                    b.Property<int>("GrupoId");

                    b.Property<bool>("Receita");

                    b.Property<bool>("RelatorioTotal");

                    b.HasKey("Id");

                    b.HasIndex("CategoriaSuperiorId");

                    b.HasIndex("GrupoId");

                    b.ToTable("Categoria");
                });

            modelBuilder.Entity("Mangos.Dominio.Entities.ContaBancaria", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Agencia");

                    b.Property<bool>("Ativo");

                    b.Property<string>("Descricao")
                        .IsRequired()
                        .HasMaxLength(100);

                    b.Property<int>("GrupoId");

                    b.Property<string>("NumeroBanco");

                    b.Property<string>("NumeroConta");

                    b.Property<bool>("RelatorioProjecaoSaldo");

                    b.Property<decimal>("SaldoInicial")
                        .HasColumnType("money");

                    b.HasKey("Id");

                    b.HasIndex("GrupoId");

                    b.ToTable("ContaBancaria");
                });

            modelBuilder.Entity("Mangos.Dominio.Entities.Grupo", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Descricao")
                        .IsRequired()
                        .HasMaxLength(100);

                    b.HasKey("Id");

                    b.ToTable("Grupo");
                });

            modelBuilder.Entity("Mangos.Dominio.Entities.Lancamento", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int?>("CategoriaId");

                    b.Property<int?>("ContaBancariaId");

                    b.Property<DateTime?>("DataContaBancaria");

                    b.Property<DateTime>("DataHoraCadastro");

                    b.Property<DateTime?>("DataPagamento");

                    b.Property<DateTime>("DataVencimento");

                    b.Property<string>("Descricao")
                        .IsRequired()
                        .HasMaxLength(255);

                    b.Property<int>("GrupoId");

                    b.Property<int>("NumeroParcela");

                    b.Property<string>("Observacoes")
                        .HasColumnType("varchar(MAX)");

                    b.Property<bool>("Pago");

                    b.Property<int?>("PessoaId");

                    b.Property<int>("Tipo");

                    b.Property<int>("TotalParcelas");

                    b.Property<decimal>("Valor")
                        .HasColumnType("money");

                    b.Property<decimal?>("ValorPago")
                        .HasColumnType("money");

                    b.HasKey("Id");

                    b.HasIndex("CategoriaId");

                    b.HasIndex("ContaBancariaId");

                    b.HasIndex("GrupoId");

                    b.HasIndex("PessoaId");

                    b.ToTable("Lancamento");
                });

            modelBuilder.Entity("Mangos.Dominio.Entities.LancamentoCartao", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("CartaoCreditoId");

                    b.Property<int?>("CategoriaId");

                    b.Property<DateTime>("DataHoraCadastro");

                    b.Property<DateTime?>("DataHoraGeracaoLancamento");

                    b.Property<string>("Descricao")
                        .IsRequired()
                        .HasMaxLength(255);

                    b.Property<bool>("GeradoLancamento");

                    b.Property<int>("GrupoId");

                    b.Property<int?>("LancamentoId");

                    b.Property<DateTime>("MesVencimento");

                    b.Property<int>("NumeroParcela");

                    b.Property<string>("Observacoes")
                        .HasColumnType("varchar(MAX)");

                    b.Property<int?>("PessoaId");

                    b.Property<int>("TotalParcelas");

                    b.Property<decimal>("Valor")
                        .HasColumnType("money");

                    b.HasKey("Id");

                    b.HasIndex("CartaoCreditoId");

                    b.HasIndex("CategoriaId");

                    b.HasIndex("GrupoId");

                    b.HasIndex("LancamentoId");

                    b.HasIndex("PessoaId");

                    b.ToTable("LancamentoCartao");
                });

            modelBuilder.Entity("Mangos.Dominio.Entities.LancamentoFixo", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<bool>("Ativo");

                    b.Property<int?>("CartaoCreditoId");

                    b.Property<int?>("CategoriaId");

                    b.Property<string>("Descricao")
                        .IsRequired()
                        .HasMaxLength(255);

                    b.Property<int?>("DiaVencimento");

                    b.Property<int>("GrupoId");

                    b.Property<int?>("MesVencimento");

                    b.Property<int>("MesesAntecedenciaGerar");

                    b.Property<int>("Periodicidade");

                    b.Property<int?>("PessoaId");

                    b.Property<int>("Tipo");

                    b.Property<DateTime?>("UltimoMesGerado");

                    b.Property<decimal>("Valor")
                        .HasColumnType("money");

                    b.HasKey("Id");

                    b.HasIndex("CartaoCreditoId");

                    b.HasIndex("CategoriaId");

                    b.HasIndex("GrupoId");

                    b.HasIndex("PessoaId");

                    b.ToTable("LancamentoFixo");
                });

            modelBuilder.Entity("Mangos.Dominio.Entities.MetaMovimentacao", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("GrupoId");

                    b.Property<DateTime>("MesFinal");

                    b.Property<DateTime>("MesInicial");

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
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("GrupoId");

                    b.Property<DateTime>("Mes");

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
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<bool>("Ativo");

                    b.Property<int?>("CategoriaPadraoDespesaId");

                    b.Property<int?>("CategoriaPadraoReceitaId");

                    b.Property<string>("Email")
                        .HasMaxLength(100);

                    b.Property<int>("GrupoId");

                    b.Property<decimal?>("Latitude")
                        .HasConversion(new ValueConverter<decimal, decimal>(v => default(decimal), v => default(decimal), new ConverterMappingHints(precision: 38, scale: 17)))
                        .HasColumnType("decimal(11,8)");

                    b.Property<decimal?>("Longitude")
                        .HasConversion(new ValueConverter<decimal, decimal>(v => default(decimal), v => default(decimal), new ConverterMappingHints(precision: 38, scale: 17)))
                        .HasColumnType("decimal(11,8)");

                    b.Property<string>("Nome")
                        .IsRequired()
                        .HasMaxLength(100);

                    b.Property<string>("Site")
                        .HasMaxLength(100);

                    b.Property<string>("Telefone1")
                        .HasMaxLength(20);

                    b.Property<string>("Telefone2")
                        .HasMaxLength(20);

                    b.Property<string>("Telefone3")
                        .HasMaxLength(20);

                    b.Property<int>("Tipo");

                    b.HasKey("Id");

                    b.HasIndex("CategoriaPadraoDespesaId");

                    b.HasIndex("CategoriaPadraoReceitaId");

                    b.HasIndex("GrupoId");

                    b.ToTable("Pessoa");
                });

            modelBuilder.Entity("Mangos.Dominio.Entities.SaldoContaBancaria", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("ContaBancariaId");

                    b.Property<DateTime>("Data");

                    b.Property<int>("GrupoId");

                    b.Property<decimal>("ValorMovimentacoes")
                        .HasColumnType("money");

                    b.Property<decimal>("ValorSaldo")
                        .HasColumnType("money");

                    b.HasKey("Id");

                    b.HasIndex("ContaBancariaId");

                    b.HasIndex("Data");

                    b.HasIndex("GrupoId");

                    b.ToTable("SaldoContaBancaria");
                });

            modelBuilder.Entity("Mangos.Dominio.Entities.TransferenciaConta", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int?>("ContaBancariaDestinoId");

                    b.Property<int?>("ContaBancariaOrigemId");

                    b.Property<DateTime>("Data");

                    b.Property<DateTime>("DataHoraCadastro");

                    b.Property<string>("Descricao")
                        .IsRequired()
                        .HasMaxLength(255);

                    b.Property<int>("GrupoId");

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
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<bool>("Admin");

                    b.Property<bool>("Ativo");

                    b.Property<DateTime?>("DataHoraUltimaAtualizacaoSessao");

                    b.Property<int>("DiasAlertaVencimentos");

                    b.Property<int>("DiasEmailVencimentos");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasMaxLength(255);

                    b.Property<int>("GrupoId");

                    b.Property<string>("Nome")
                        .IsRequired()
                        .HasMaxLength(100);

                    b.Property<string>("Senha")
                        .IsRequired()
                        .HasMaxLength(100);

                    b.Property<int>("TipoAlertaVencimentos");

                    b.Property<int>("TipoEmailVencimentos");

                    b.Property<string>("TokenSenha")
                        .HasMaxLength(12);

                    b.Property<DateTime?>("ValidadeTokenSenha");

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
                        .OnDelete(DeleteBehavior.Restrict);
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
                        .OnDelete(DeleteBehavior.Restrict);
                });

            modelBuilder.Entity("Mangos.Dominio.Entities.ContaBancaria", b =>
                {
                    b.HasOne("Mangos.Dominio.Entities.Grupo", "Grupo")
                        .WithMany("ContasBancarias")
                        .HasForeignKey("GrupoId")
                        .OnDelete(DeleteBehavior.Restrict);
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
                        .OnDelete(DeleteBehavior.Restrict);

                    b.HasOne("Mangos.Dominio.Entities.Categoria", "Categoria")
                        .WithMany("LancamentosCartao")
                        .HasForeignKey("CategoriaId")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.HasOne("Mangos.Dominio.Entities.Grupo", "Grupo")
                        .WithMany("LancamentosCartao")
                        .HasForeignKey("GrupoId")
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
                        .OnDelete(DeleteBehavior.Restrict);

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
                        .OnDelete(DeleteBehavior.Restrict);
                });

            modelBuilder.Entity("Mangos.Dominio.Entities.MetaSaldo", b =>
                {
                    b.HasOne("Mangos.Dominio.Entities.Grupo", "Grupo")
                        .WithMany("MetasSaldo")
                        .HasForeignKey("GrupoId")
                        .OnDelete(DeleteBehavior.Restrict);
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
                        .OnDelete(DeleteBehavior.Restrict);
                });

            modelBuilder.Entity("Mangos.Dominio.Entities.SaldoContaBancaria", b =>
                {
                    b.HasOne("Mangos.Dominio.Entities.ContaBancaria", "ContaBancaria")
                        .WithMany("SaldosContasBancarias")
                        .HasForeignKey("ContaBancariaId")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.HasOne("Mangos.Dominio.Entities.Grupo", "Grupo")
                        .WithMany("SaldosContasBancarias")
                        .HasForeignKey("GrupoId")
                        .OnDelete(DeleteBehavior.Restrict);
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
                        .OnDelete(DeleteBehavior.Restrict);
                });

            modelBuilder.Entity("Mangos.Dominio.Entities.Usuario", b =>
                {
                    b.HasOne("Mangos.Dominio.Entities.Grupo", "Grupo")
                        .WithMany("Usuarios")
                        .HasForeignKey("GrupoId")
                        .OnDelete(DeleteBehavior.Restrict);
                });
#pragma warning restore 612, 618
        }
    }
}
