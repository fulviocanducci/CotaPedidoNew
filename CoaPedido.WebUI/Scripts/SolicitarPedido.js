function Chosen(target, func) {
    if (typeof func === 'function') {
      $(target).chosen({ no_results_text: "Sem registros para:", width: "100%" }).change(func);
    }
    else {
      $(target).chosen({ no_results_text: "Sem registros para:", width: "100%" });
    }
}

function CarregarCidades() {
    var val = $('#regiao').val();
    if (typeof val !== 'undefined') {
        //$('#cidade option:gt(0)').attr('style', 'display:none');
        //$('#cidade option[data-regiao="' + val + '"]').removeAttr('style');
    }
}

function CarregarSubCategorias() {
    var val = $('#categoria').val();
    if (typeof val !== 'undefined') {
        //$('#subcategoria option:gt(0)').attr('style', 'display:none');
        //$('#subcategoria option[data-categoria="' + val + '"]').removeAttr('style');
    }
}



$(document).ready(function () {
  $('#dataLimiteProposta').mask('99/99/9999');

  //var options = $('option[value*="data-regiao"]');
  //options.each(function () {
  //    var val = $(this).val().split(' ');
  //    $(this).val(val[0].replace('\"', ''));
  //    $(this).attr('data-regiao', val[1].replace('data-regiao="', '').replace('\"', ''));
  //});

  //var idCidade = $('#idCidade').val();
  //if (idCidade !== '0') {
  //    $('#cidade').val(idCidade);
  //}

  //CarregarCidades();

  //$('#regiao').change(function () {
  //    CarregarCidades();
  //    $('#cidade').val('');
  //})

  var optionsSub = $('option[value*="data-categoria"]');
  optionsSub.each(function () {
    var val = $(this).val().split(' ');
    $(this).val(val[0].replace('\"', ''));
    $(this).attr('data-categoria', val[1].replace('data-categoria="', '').replace('\"', ''));
  });

  var idSubCategoria = $('#idSubCategoria').val();
  if (idSubCategoria !== '0') {
    $('#idSubCategoria').val(idSubCategoria);
  }

  CarregarSubCategorias();

  $('#categoria').change(function () {
    CarregarSubCategorias();
    $('#subcategoria').val('');
  });

  //Chosen('#categoria');
});