function buscarPessoaPeloGps() {
    getPosition(10, 10, true, function (position) {

        jQuery('#Latitude').val(position.coords.latitude.toString().replace(".", ","));
        jQuery('#Longitude').val(position.coords.longitude.toString().replace(".", ","));

        var url = fullUrl + "Geo/BuscaFornecedorMaisProximo?latitude=" + jQuery('#Latitude').val() + "&longitude=" + jQuery('#Longitude').val();

        $.ajax({
            url: url,
            dataType: "json",
            success: function (data) {
                if (data !== null) {
                    jQuery('#PessoaCoordenadaIdAtualizar').val(data.id);
                    jQuery('#PessoaId').val(data.pessoaId);
                    jQuery('#ProcuraPessoa').val(data.pessoaNome);
                    jQuery('#Descricao').val(data.ultimaDescricaoDespesa);

                    if (jQuery("#ContaBancariaId option[value='" + data.contaBancariaId + "']").length > 0)
                        jQuery('#ContaBancariaId').val(data.contaBancariaId);

                    if (jQuery("#CartaoCreditoId option[value='" + data.CartaoCreditoId + "']").length > 0)
                        jQuery('#CartaoCreditoId').val(data.CartaoCreditoId);
                }

                if (jQuery('#ProcuraPessoa').val() === '')
                    jQuery('#ProcuraPessoa').focus();
                else if (jQuery('#Descricao').val() === '')
                    jQuery('#Descricao').focus();
                else
                    jQuery('#Valor').focus();
            }
        });

    });
}

jQuery(document).ready(function () {
    buscarPessoaPeloGps();
});
