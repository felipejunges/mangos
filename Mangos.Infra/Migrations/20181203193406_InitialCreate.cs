using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Mangos.Infra.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Grupo",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Descricao = table.Column<string>(maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Grupo", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ParametrosSistema",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false),
                    QuantidadeBackupsManter = table.Column<int>(nullable: false),
                    ExcluirBackupsDesatualizados = table.Column<bool>(nullable: false),
                    QuantidadeMesesBackupManter = table.Column<int>(nullable: false),
                    QuantidadeSegundasBackupManter = table.Column<int>(nullable: false),
                    QuantidadeDiasBackupManter = table.Column<int>(nullable: false),
                    DistanciaMaximaFornecedorDespesaRapida = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ParametrosSistema", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Categoria",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    GrupoId = table.Column<int>(nullable: false),
                    CategoriaSuperiorId = table.Column<int>(nullable: true),
                    Descricao = table.Column<string>(maxLength: 100, nullable: false),
                    Receita = table.Column<bool>(nullable: false),
                    Despesa = table.Column<bool>(nullable: false),
                    RelatorioTotal = table.Column<bool>(nullable: false),
                    Ativo = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Categoria", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Categoria_Categoria_CategoriaSuperiorId",
                        column: x => x.CategoriaSuperiorId,
                        principalTable: "Categoria",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Categoria_Grupo_GrupoId",
                        column: x => x.GrupoId,
                        principalTable: "Grupo",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ContaBancaria",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    GrupoId = table.Column<int>(nullable: false),
                    Descricao = table.Column<string>(maxLength: 100, nullable: false),
                    NumeroBanco = table.Column<string>(nullable: true),
                    Agencia = table.Column<string>(nullable: true),
                    NumeroConta = table.Column<string>(nullable: true),
                    SaldoInicial = table.Column<decimal>(type: "money", nullable: false),
                    RelatorioProjecaoSaldo = table.Column<bool>(nullable: false),
                    Ativo = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ContaBancaria", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ContaBancaria_Grupo_GrupoId",
                        column: x => x.GrupoId,
                        principalTable: "Grupo",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "MetaMovimentacao",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    GrupoId = table.Column<int>(nullable: false),
                    ValorMensal = table.Column<decimal>(type: "money", nullable: false),
                    MesInicial = table.Column<DateTime>(nullable: false),
                    MesFinal = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MetaMovimentacao", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MetaMovimentacao_Grupo_GrupoId",
                        column: x => x.GrupoId,
                        principalTable: "Grupo",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Usuario",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    GrupoId = table.Column<int>(nullable: false),
                    Nome = table.Column<string>(maxLength: 100, nullable: false),
                    Email = table.Column<string>(maxLength: 255, nullable: false),
                    Senha = table.Column<string>(maxLength: 100, nullable: false),
                    DataHoraUltimaAtualizacaoSessao = table.Column<DateTime>(nullable: true),
                    TipoAlertaVencimentos = table.Column<int>(nullable: false),
                    DiasAlertaVencimentos = table.Column<int>(nullable: false),
                    TipoEmailVencimentos = table.Column<int>(nullable: false),
                    DiasEmailVencimentos = table.Column<int>(nullable: false),
                    Admin = table.Column<bool>(nullable: false),
                    Ativo = table.Column<bool>(nullable: false),
                    TokenSenha = table.Column<string>(maxLength: 12, nullable: true),
                    ValidadeTokenSenha = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Usuario", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Usuario_Grupo_GrupoId",
                        column: x => x.GrupoId,
                        principalTable: "Grupo",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "CartaoCredito",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    GrupoId = table.Column<int>(nullable: false),
                    Descricao = table.Column<string>(maxLength: 100, nullable: false),
                    CategoriaId = table.Column<int>(nullable: true),
                    DiaFechamento = table.Column<int>(nullable: false),
                    DiaVencimento = table.Column<int>(nullable: false),
                    DataUltimoFechamento = table.Column<DateTime>(nullable: true),
                    Ativo = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CartaoCredito", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CartaoCredito_Categoria_CategoriaId",
                        column: x => x.CategoriaId,
                        principalTable: "Categoria",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CartaoCredito_Grupo_GrupoId",
                        column: x => x.GrupoId,
                        principalTable: "Grupo",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Pessoa",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    GrupoId = table.Column<int>(nullable: false),
                    Nome = table.Column<string>(maxLength: 100, nullable: false),
                    Telefone1 = table.Column<string>(maxLength: 20, nullable: true),
                    Telefone2 = table.Column<string>(maxLength: 20, nullable: true),
                    Telefone3 = table.Column<string>(maxLength: 20, nullable: true),
                    Site = table.Column<string>(maxLength: 100, nullable: true),
                    Email = table.Column<string>(maxLength: 100, nullable: true),
                    Tipo = table.Column<int>(nullable: false),
                    CategoriaPadraoReceitaId = table.Column<int>(nullable: true),
                    CategoriaPadraoDespesaId = table.Column<int>(nullable: true),
                    Latitude = table.Column<decimal>(type: "decimal(11,8)", nullable: true),
                    Longitude = table.Column<decimal>(type: "decimal(11,8)", nullable: true),
                    Ativo = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Pessoa", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Pessoa_Categoria_CategoriaPadraoDespesaId",
                        column: x => x.CategoriaPadraoDespesaId,
                        principalTable: "Categoria",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Pessoa_Categoria_CategoriaPadraoReceitaId",
                        column: x => x.CategoriaPadraoReceitaId,
                        principalTable: "Categoria",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Pessoa_Grupo_GrupoId",
                        column: x => x.GrupoId,
                        principalTable: "Grupo",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "MetaSaldo",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    GrupoId = table.Column<int>(nullable: false),
                    ValorFinal = table.Column<decimal>(nullable: false),
                    MesInicial = table.Column<DateTime>(nullable: false),
                    MesFinal = table.Column<DateTime>(nullable: false),
                    ContaBancariaId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MetaSaldo", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MetaSaldo_ContaBancaria_ContaBancariaId",
                        column: x => x.ContaBancariaId,
                        principalTable: "ContaBancaria",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_MetaSaldo_Grupo_GrupoId",
                        column: x => x.GrupoId,
                        principalTable: "Grupo",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "SaldoContaBancaria",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    GrupoId = table.Column<int>(nullable: false),
                    ContaBancariaId = table.Column<int>(nullable: false),
                    Data = table.Column<DateTime>(nullable: false),
                    ValorMovimentacoes = table.Column<decimal>(type: "money", nullable: false),
                    ValorSaldo = table.Column<decimal>(type: "money", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SaldoContaBancaria", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SaldoContaBancaria_ContaBancaria_ContaBancariaId",
                        column: x => x.ContaBancariaId,
                        principalTable: "ContaBancaria",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_SaldoContaBancaria_Grupo_GrupoId",
                        column: x => x.GrupoId,
                        principalTable: "Grupo",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "TransferenciaConta",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    GrupoId = table.Column<int>(nullable: false),
                    DataHoraCadastro = table.Column<DateTime>(nullable: false),
                    Valor = table.Column<decimal>(type: "money", nullable: false),
                    Data = table.Column<DateTime>(nullable: false),
                    Descricao = table.Column<string>(maxLength: 255, nullable: false),
                    ContaBancariaOrigemId = table.Column<int>(nullable: true),
                    ContaBancariaDestinoId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TransferenciaConta", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TransferenciaConta_ContaBancaria_ContaBancariaDestinoId",
                        column: x => x.ContaBancariaDestinoId,
                        principalTable: "ContaBancaria",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TransferenciaConta_ContaBancaria_ContaBancariaOrigemId",
                        column: x => x.ContaBancariaOrigemId,
                        principalTable: "ContaBancaria",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TransferenciaConta_Grupo_GrupoId",
                        column: x => x.GrupoId,
                        principalTable: "Grupo",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Lancamento",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    GrupoId = table.Column<int>(nullable: false),
                    DataHoraCadastro = table.Column<DateTime>(nullable: false),
                    Tipo = table.Column<int>(nullable: false),
                    Valor = table.Column<decimal>(type: "money", nullable: false),
                    DataVencimento = table.Column<DateTime>(nullable: false),
                    Descricao = table.Column<string>(maxLength: 255, nullable: false),
                    NumeroParcela = table.Column<int>(nullable: false),
                    TotalParcelas = table.Column<int>(nullable: false),
                    PessoaId = table.Column<int>(nullable: true),
                    CategoriaId = table.Column<int>(nullable: true),
                    Observacoes = table.Column<string>(type: "varchar(MAX)", nullable: true),
                    Pago = table.Column<bool>(nullable: false),
                    DataPagamento = table.Column<DateTime>(nullable: true),
                    ValorPago = table.Column<decimal>(type: "money", nullable: true),
                    ContaBancariaId = table.Column<int>(nullable: true),
                    DataContaBancaria = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Lancamento", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Lancamento_Categoria_CategoriaId",
                        column: x => x.CategoriaId,
                        principalTable: "Categoria",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Lancamento_ContaBancaria_ContaBancariaId",
                        column: x => x.ContaBancariaId,
                        principalTable: "ContaBancaria",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Lancamento_Grupo_GrupoId",
                        column: x => x.GrupoId,
                        principalTable: "Grupo",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Lancamento_Pessoa_PessoaId",
                        column: x => x.PessoaId,
                        principalTable: "Pessoa",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "LancamentoFixo",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    GrupoId = table.Column<int>(nullable: false),
                    Tipo = table.Column<int>(nullable: false),
                    Periodicidade = table.Column<int>(nullable: false),
                    Valor = table.Column<decimal>(type: "money", nullable: false),
                    DiaVencimento = table.Column<int>(nullable: true),
                    MesVencimento = table.Column<int>(nullable: true),
                    CartaoCreditoId = table.Column<int>(nullable: true),
                    Descricao = table.Column<string>(maxLength: 255, nullable: false),
                    PessoaId = table.Column<int>(nullable: true),
                    CategoriaId = table.Column<int>(nullable: true),
                    MesesAntecedenciaGerar = table.Column<int>(nullable: false),
                    UltimoMesGerado = table.Column<DateTime>(nullable: true),
                    Ativo = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LancamentoFixo", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LancamentoFixo_CartaoCredito_CartaoCreditoId",
                        column: x => x.CartaoCreditoId,
                        principalTable: "CartaoCredito",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_LancamentoFixo_Categoria_CategoriaId",
                        column: x => x.CategoriaId,
                        principalTable: "Categoria",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_LancamentoFixo_Grupo_GrupoId",
                        column: x => x.GrupoId,
                        principalTable: "Grupo",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_LancamentoFixo_Pessoa_PessoaId",
                        column: x => x.PessoaId,
                        principalTable: "Pessoa",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "LancamentoCartao",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    GrupoId = table.Column<int>(nullable: false),
                    DataHoraCadastro = table.Column<DateTime>(nullable: false),
                    CartaoCreditoId = table.Column<int>(nullable: false),
                    Valor = table.Column<decimal>(type: "money", nullable: false),
                    MesVencimento = table.Column<DateTime>(nullable: false),
                    Descricao = table.Column<string>(maxLength: 255, nullable: false),
                    NumeroParcela = table.Column<int>(nullable: false),
                    TotalParcelas = table.Column<int>(nullable: false),
                    PessoaId = table.Column<int>(nullable: true),
                    CategoriaId = table.Column<int>(nullable: true),
                    Observacoes = table.Column<string>(type: "varchar(MAX)", nullable: true),
                    GeradoLancamento = table.Column<bool>(nullable: false),
                    LancamentoId = table.Column<int>(nullable: true),
                    DataHoraGeracaoLancamento = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LancamentoCartao", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LancamentoCartao_CartaoCredito_CartaoCreditoId",
                        column: x => x.CartaoCreditoId,
                        principalTable: "CartaoCredito",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_LancamentoCartao_Categoria_CategoriaId",
                        column: x => x.CategoriaId,
                        principalTable: "Categoria",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_LancamentoCartao_Grupo_GrupoId",
                        column: x => x.GrupoId,
                        principalTable: "Grupo",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_LancamentoCartao_Lancamento_LancamentoId",
                        column: x => x.LancamentoId,
                        principalTable: "Lancamento",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_LancamentoCartao_Pessoa_PessoaId",
                        column: x => x.PessoaId,
                        principalTable: "Pessoa",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CartaoCredito_CategoriaId",
                table: "CartaoCredito",
                column: "CategoriaId");

            migrationBuilder.CreateIndex(
                name: "IX_CartaoCredito_GrupoId",
                table: "CartaoCredito",
                column: "GrupoId");

            migrationBuilder.CreateIndex(
                name: "IX_Categoria_CategoriaSuperiorId",
                table: "Categoria",
                column: "CategoriaSuperiorId");

            migrationBuilder.CreateIndex(
                name: "IX_Categoria_GrupoId",
                table: "Categoria",
                column: "GrupoId");

            migrationBuilder.CreateIndex(
                name: "IX_ContaBancaria_GrupoId",
                table: "ContaBancaria",
                column: "GrupoId");

            migrationBuilder.CreateIndex(
                name: "IX_Lancamento_CategoriaId",
                table: "Lancamento",
                column: "CategoriaId");

            migrationBuilder.CreateIndex(
                name: "IX_Lancamento_ContaBancariaId",
                table: "Lancamento",
                column: "ContaBancariaId");

            migrationBuilder.CreateIndex(
                name: "IX_Lancamento_GrupoId",
                table: "Lancamento",
                column: "GrupoId");

            migrationBuilder.CreateIndex(
                name: "IX_Lancamento_PessoaId",
                table: "Lancamento",
                column: "PessoaId");

            migrationBuilder.CreateIndex(
                name: "IX_LancamentoCartao_CartaoCreditoId",
                table: "LancamentoCartao",
                column: "CartaoCreditoId");

            migrationBuilder.CreateIndex(
                name: "IX_LancamentoCartao_CategoriaId",
                table: "LancamentoCartao",
                column: "CategoriaId");

            migrationBuilder.CreateIndex(
                name: "IX_LancamentoCartao_GrupoId",
                table: "LancamentoCartao",
                column: "GrupoId");

            migrationBuilder.CreateIndex(
                name: "IX_LancamentoCartao_LancamentoId",
                table: "LancamentoCartao",
                column: "LancamentoId");

            migrationBuilder.CreateIndex(
                name: "IX_LancamentoCartao_PessoaId",
                table: "LancamentoCartao",
                column: "PessoaId");

            migrationBuilder.CreateIndex(
                name: "IX_LancamentoFixo_CartaoCreditoId",
                table: "LancamentoFixo",
                column: "CartaoCreditoId");

            migrationBuilder.CreateIndex(
                name: "IX_LancamentoFixo_CategoriaId",
                table: "LancamentoFixo",
                column: "CategoriaId");

            migrationBuilder.CreateIndex(
                name: "IX_LancamentoFixo_GrupoId",
                table: "LancamentoFixo",
                column: "GrupoId");

            migrationBuilder.CreateIndex(
                name: "IX_LancamentoFixo_PessoaId",
                table: "LancamentoFixo",
                column: "PessoaId");

            migrationBuilder.CreateIndex(
                name: "IX_MetaMovimentacao_GrupoId",
                table: "MetaMovimentacao",
                column: "GrupoId");

            migrationBuilder.CreateIndex(
                name: "IX_MetaSaldo_ContaBancariaId",
                table: "MetaSaldo",
                column: "ContaBancariaId");

            migrationBuilder.CreateIndex(
                name: "IX_MetaSaldo_GrupoId",
                table: "MetaSaldo",
                column: "GrupoId");

            migrationBuilder.CreateIndex(
                name: "IX_Pessoa_CategoriaPadraoDespesaId",
                table: "Pessoa",
                column: "CategoriaPadraoDespesaId");

            migrationBuilder.CreateIndex(
                name: "IX_Pessoa_CategoriaPadraoReceitaId",
                table: "Pessoa",
                column: "CategoriaPadraoReceitaId");

            migrationBuilder.CreateIndex(
                name: "IX_Pessoa_GrupoId",
                table: "Pessoa",
                column: "GrupoId");

            migrationBuilder.CreateIndex(
                name: "IX_SaldoContaBancaria_ContaBancariaId",
                table: "SaldoContaBancaria",
                column: "ContaBancariaId");

            migrationBuilder.CreateIndex(
                name: "IX_SaldoContaBancaria_Data",
                table: "SaldoContaBancaria",
                column: "Data");

            migrationBuilder.CreateIndex(
                name: "IX_SaldoContaBancaria_GrupoId",
                table: "SaldoContaBancaria",
                column: "GrupoId");

            migrationBuilder.CreateIndex(
                name: "IX_TransferenciaConta_ContaBancariaDestinoId",
                table: "TransferenciaConta",
                column: "ContaBancariaDestinoId");

            migrationBuilder.CreateIndex(
                name: "IX_TransferenciaConta_ContaBancariaOrigemId",
                table: "TransferenciaConta",
                column: "ContaBancariaOrigemId");

            migrationBuilder.CreateIndex(
                name: "IX_TransferenciaConta_GrupoId",
                table: "TransferenciaConta",
                column: "GrupoId");

            migrationBuilder.CreateIndex(
                name: "IX_Usuario_GrupoId",
                table: "Usuario",
                column: "GrupoId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "LancamentoCartao");

            migrationBuilder.DropTable(
                name: "LancamentoFixo");

            migrationBuilder.DropTable(
                name: "MetaMovimentacao");

            migrationBuilder.DropTable(
                name: "MetaSaldo");

            migrationBuilder.DropTable(
                name: "ParametrosSistema");

            migrationBuilder.DropTable(
                name: "SaldoContaBancaria");

            migrationBuilder.DropTable(
                name: "TransferenciaConta");

            migrationBuilder.DropTable(
                name: "Usuario");

            migrationBuilder.DropTable(
                name: "Lancamento");

            migrationBuilder.DropTable(
                name: "CartaoCredito");

            migrationBuilder.DropTable(
                name: "ContaBancaria");

            migrationBuilder.DropTable(
                name: "Pessoa");

            migrationBuilder.DropTable(
                name: "Categoria");

            migrationBuilder.DropTable(
                name: "Grupo");
        }
    }
}
