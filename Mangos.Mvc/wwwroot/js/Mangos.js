var $loading = $('#loading').hide();
$(document)
  .ajaxStart(function () {
      $loading.show();
  })
  .ajaxStop(function () {
      $loading.hide();
  });

$.fn.noCacheSubmit = function () {
    let reativaApos = false;

    if ($(this).attr('data-ajax-cache') !== undefined && $(this).attr('data-ajax-cache') === "true") {
        reativaApos = true;
        $(this).attr('data-ajax-cache', "false");
    }

    $(this).submit();

    if (reativaApos)
        $(this).attr('data-ajax-cache', "true");
};

function setaCamposMascaras() {
    jQuery(".campoValor").maskMoney({ allowNegative: true, thousands: '.', decimal: ',', allowZero: true });

    jQuery('.campoData').mask('00/00/0000');
    jQuery('.campoData').datepicker({
        format: 'dd/mm/yyyy',
        language: "pt-BR",
        orientation: "auto left",
        autoclose: true,
        todayBtn: "linked",
        todayHighlight: true
    });

    jQuery('.campoMesAno').mask('00/0000');
    jQuery('.campoMesAno').datepicker({
        format: 'mm/yyyy',
        language: "pt-BR",
        orientation: "auto left",
        minViewMode: 1,
        autoclose: true,
        todayBtn: "linked",
        todayHighlight: true
    });

    jQuery('.campo-ano').mask('0000');
    jQuery('.campo-ano').datepicker({
        format: 'yyyy',
        language: "pt-BR",
        orientation: "auto left",
        minViewMode: 2,
        autoclose: true,
        todayBtn: "linked",
        todayHighlight: true
    });

    $('.input-daterange').datepicker({
        format: 'dd/mm/yyyy',
        language: "pt-BR",
        orientation: "auto left",
        autoclose: true,
        todayBtn: "linked",
        todayHighlight: true
    });

    /*$('.input-daterange-mes-ano').datepicker({
        format: 'mm/yyyy',
        language: "pt-BR",
        orientation: "auto left",
        autoclose: true,
        todayBtn: "linked",
        todayHighlight: true
    });*/

    var TelMaskBehavior = function (val) {
        return val.replace(/\D/g, '').length === 11 ? '(00) 0 0000-0000' : '(00) 0000-00009';
    },
        spOptions = {
            onKeyPress: function (val, e, field, options) {
                field.mask(TelMaskBehavior.apply({}, arguments), options);
            }
        };

    $('.campoTelefone').mask(TelMaskBehavior, spOptions);

    //
    $('.campoData').attr('autocomplete', 'off');
    $('.campoMesAno').attr('autocomplete', 'off');
    $('.campoValor').attr('autocomplete', 'off');
    $('.campoTelefone').attr('autocomplete', 'off');
}