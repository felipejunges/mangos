String.prototype.endsWith = function (suffix) {
    return this.indexOf(suffix, this.length - suffix.length) !== -1;
};

$(document).ready(function () {
    $('#btnFiltroSimplesAvancado').on('click', function (e) {
        var form = $(this).closest('form');
        var tipoBuscaDom = $(form).find('#TipoBusca');

        if (tipoBuscaDom.val() === 'S') {
            tipoBuscaDom.val('C');
            $(form).find('.group-busca-padrao').css('display', 'none');
            $(form).find('.group-busca-avancada').css('display', 'flex');

            $(this).text('[-] Filtro padrão');
        } else {
            tipoBuscaDom.val('S');
            $(form).find('.group-busca-padrao').css('display', 'flex');
            $(form).find('.group-busca-avancada').css('display', 'none');

            $(this).text('[+] Filtro avançado');
        }
    });
});
