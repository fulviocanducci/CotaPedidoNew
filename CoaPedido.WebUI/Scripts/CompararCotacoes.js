function PrintDiv() {
    var divToPrint = $('#divToPrint');
    var divOriginal = $('#divToPrint').html();

   // divToPrint.find('.col-categoria').remove();
   // divToPrint.find('.col-subcategoria').remove();

    var popupWin = window.open('', '_blank', 'width=900,height=600,location=no,left=200px');
    popupWin.document.write('<html><head>' +
        '<link href="/Content/css/bootstrap.min.css" rel="stylesheet"/>' +
        '<link href="/Content/css/font-awesome.min.css" rel="stylesheet"/>' +
        '<link href="/Content/css/prettyPhoto.css" rel="stylesheet"/>' +
        '<link href="/Content/css/price-range.css" rel="stylesheet"/>' +
        '<link href="/Content/css/animate.css" rel="stylesheet"/>' +
        '<link href="/Content/css/main.css" rel="stylesheet"/>' +
        '<link href="/Content/css/responsive.css" rel="stylesheet"/>' +
        '</head><body onload="window.print()">' + divToPrint.html() + '</body></html>');
    popupWin.document.close();
    popupWin.onfocus = function () {
        setTimeout(function () {
            popupWin.focus();
            popupWin.close();
        }, 200);
    };

    $('#divToPrint').html(divOriginal);
}

$(document).ready(function () {

    $('.checkboxEscolhido').change(function () {
        var idCotacao = parseInt($(this).val());

        $('#cotacaoEscolhida').val(idCotacao);
    });



    
})