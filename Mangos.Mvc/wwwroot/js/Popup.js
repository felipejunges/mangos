function executeAction(controller, action, id, refreshAfter) {
    if (refreshAfter === undefined)
        refreshAfter = true;

    var url = fullUrl + controller + "/" + action + (id !== 0 ? "/" + id : '');

    $.ajax({
        url: url,
        dataType: "json",
        success: function (data) {
            if (data !== null && data.mensagem !== '')
                mensagemTopo(data.sucesso, data.mensagem);

            if (refreshAfter && data.sucesso) {
                if ($('#form-busca').length)
                    $('#form-busca').noCacheSubmit();
                else
                    location.reload();
            }
        }
    });
}

function openModal(controller, action, id, queryName, queryValue) {
    var url = fullUrl + controller + "/" + action + (id !== 0 ? "/" + id : '') + (queryName !== undefined && queryName !== '' ? '?' + queryName + '=' + queryValue : '');

    $.ajax({
        url: url,
        dataType: "text",
        success: function (data) {
            $('#modal-principal').html(data);
            $('#modal-principal').modal('show');
        }
    });
}

function refreshModal() {
    $('#modal-principal').modal('handleUpdate');
}

function closeModal() {
    $('#modal-principal').modal('hide');
}

function excludeItem(controller, id) {
    var url = fullUrl + controller + "/Excluir/" + id;

    $.ajax({
        url: url,
        dataType: "json",
        success: function (data) {

            if (data !== null && data.mensagem !== '')
                mensagemTopo(data.sucesso, data.mensagem);

            if (data.sucesso) {
                if ($('#form-busca-modal').length)
                    $('#form-busca-modal').noCacheSubmit();
                else if ($('#form-busca').length)
                    $('#form-busca').noCacheSubmit();
                else
                    location.reload();
            }
        }
    });
}

function setaFormSubmit() {
    $('#modal-principal').find('form').on('submit', function (e) {
        e.preventDefault();

        var action = $(this).attr('action');

        $.ajax({
            url: action,
            dataType: "text",
            type: "POST",
            data: $(this).serialize(),
            success: function (data) {
                if (data !== "OK") {
                    $('#modal-principal').html(data);
                } else {
                    closeModal();

                    if ($('#form-busca').length)
                        $('#form-busca').noCacheSubmit();
                }
            }
        });
    });
}
