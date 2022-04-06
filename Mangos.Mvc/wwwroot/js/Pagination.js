
function carregarPagina(pagina) {
    $('#form-busca').find('#Pagina').val(pagina);
    $('#form-busca').submit();
}

$(document).ready(function () {
    $('button[type="submit"]').on('click', function () {
        $('#form-busca').find('#Pagina').val('1');
    });
});
