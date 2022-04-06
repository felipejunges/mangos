
function gerarSaldos() {
    var url = fullUrl + "SaldoConta/GerarSaldos";

    $.ajax({
        url: url,
        dataType: "json",
        data: "contaBancariaId=" + jQuery("#Busca_ContaBancariaId").val() + "&mesInicial=" + jQuery("#Busca_MesInicial").val(),
        success: function (data) {

            if (data !== null && data.mensagem !== '')
                mensagemTopo(data.sucesso, data.mensagem);

            if (data.sucesso) {
                if ($('#form-busca').length)
                    $('#form-busca').noCacheSubmit();
                else
                    location.reload();
            }
        }
    });
}
