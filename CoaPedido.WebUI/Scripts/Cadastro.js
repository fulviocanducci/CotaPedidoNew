//function CarregarCidades() {
//  var val = $('#inputUF').val();

//  alert(val);
//    if (typeof val !== 'undefined') {
//        $('#inputCidades option:gt(0)').attr('style', 'display:none');
//        $('#inputCidades option[data-uf="' + val + '"]').removeAttr('style');
//    }
//}

//function CarregarCidadesCobranca() {
//    var val = $('#inputUF2').val();
//    if (typeof val !== 'undefined') {
//        $('#inputCidades2 option:gt(0)').attr('style', 'display:none');
//        $('#inputCidades2 option[data-uf="' + val + '"]').removeAttr('style');
//    }
//}

$(document).ready(function () {
  //var options = $('option[value*="data-uf"]');
  //options.each(function () {
  //  var val = $(this).val().split(' ');
  //  $(this).val(val[0].replace('\"', ''));
  //  $(this).attr('data-uf', val[1].replace('data-uf="', '').replace('\"', ''));
  //});

  //var idCidade = $('#inputIdCidade').val();
  //if (idCidade !== '0') {
  //  $('#inputCidades').val(idCidade);
  //}

  //idCidade = $('#inputIdCidadeCobranca').val();
  //if (idCidade !== '0') {
  //  $('#inputCidades2').val(idCidade);
  //}

  //CarregarCidades();
  //CarregarCidadesCobranca();

  $('#inputCNPJ').mask('999.999.999-99');
  $('#inputCNPJ').keydown(function (e) {
    if (e.which !== 9) {
      var value = "";
      if ($(this).val().length >= 14) {
        value = $(this).cleanVal();
        $(this).unmask();
        $(this).mask('99.999.999/9999-99');
        $(this).val(value);
      }
      else {
        value = $(this).cleanVal();
        $(this).unmask();
        $(this).mask('999.999.999-99');
        $(this).val(value);
      }
    }
  });
  $('#inputCelular').mask('(99) 99999-9999');
  $('#inputTelefone').mask('(99) 9999-9999');
  $('#inputCEP').mask('19.999-999');

  //$('#inputUF').change(function () {
  //  CarregarCidades();
  //  $('#inputCidades').val('');
  //});

  //////////////////////////////////////////////////////////////////////////////////////
  //                              ENDEREÇO DE COBRANÇA                                //
  //////////////////////////////////////////////////////////////////////////////////////

  $('#inputCelular2').mask('(99) 99999-9999');
  $('#inputTelefone2').mask('(99) 9999-9999');
  $('#inputCEP2').mask('19.999-999');
  //$('#inputUF2').change(function () {
  //  CarregarCidadesCobranca();
  //  $('#inputCidades2').val('');
  //});
  $('#btnCobranca').click(function () {
    var inputUF = '#inputUF2';
    var inputCidade = '#inputCidades2';
    var inputEndereco = '#inputEndereco2';
    var inputNumero = '#inputNumero2';
    var inputComplemento = '#inputComplemento2';
    var inputCEP = '#inputCEP2';
    var inputBairro = '#inputBairro2';
    var inputTelefone = '#inputTelefone2';
    var inputCelular = '#inputCelular2';
    if ($(this).prop('checked') === true) {

      var valUF = $('#inputUF').val();
      if (typeof valUF !== 'undefined')
        $(inputUF).val(valUF);
      var valCidade = $('#inputCidades').val();
      if (typeof valCidade !== 'undefined')
        $(inputCidade).val(valCidade);

      $(inputEndereco).val($('#inputEndereco').val());
      $(inputNumero).val($('#inputNumero').val());
      $(inputComplemento).val($('#inputComplemento').val());
      $(inputCEP).val($('#inputCEP').val());
      $(inputBairro).val($('#inputBairro').val());
      $(inputTelefone).val($('#inputTelefone').val());
      $(inputCelular).val($('#inputCelular').val());
      $(inputEndereco).trigger('focusout');
      $(inputNumero).trigger('focusout');
      $(inputComplemento).trigger('focusout');
      $(inputCEP).trigger('focusout');
      $(inputBairro).trigger('focusout');
      $(inputTelefone).trigger('focusout');
      $(inputCelular).trigger('focusout');
    } else {
      $(inputUF).val($($(inputUF + ' option')[0]).val());
      $(inputCidade).val('');
      $(inputEndereco).val('');
      $(inputNumero).val('');
      $(inputComplemento).val('');
      $(inputCEP).val('');
      $(inputBairro).val('');
      $(inputTelefone).val('');
      $(inputCelular).val('');
    }
  });
});