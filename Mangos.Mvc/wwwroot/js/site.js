var timerFecharMensagem = null;

function mensagemTopo(sucesso, mensagem) {
    $('#mensagem-topo-texto').html(mensagem);

    if (sucesso) {
        $('.alert-topo').removeClass('alert-danger');
        $('.alert-topo').addClass('alert-success');
    } else {
        $('.alert-topo').removeClass('alert-success');
        $('.alert-topo').addClass('alert-danger');
    }

    $('.alert-topo').show();

    //
    if (timerFecharMensagem !== null)
        clearInterval(timerFecharMensagem);

    timerFecharMensagem = setTimeout(() => $('.alert-topo').hide(), 7500);
}
