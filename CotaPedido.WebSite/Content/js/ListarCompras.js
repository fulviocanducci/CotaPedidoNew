function CalculaPrecos(target) {
    var tr = $(target).parentsUntil('tbody');

    var subtotal = $(tr).find('td[id*="subtotal"]');

    var valorAntigo = parseFloat($(subtotal).html().replace('R$ ', '').replace(',', '.'));

    var total = parseFloat($('#total b').html().replace('R$ ', '').replace(',', '.'));

    total = total - valorAntigo;

    var qtde = $(tr).find('input[id*="qtde"]');
    var preco = $(tr).find('input[id*="preco"]');

    qtde = parseInt($(qtde).val());
    preco = parseFloat($(preco).val());

    var valorNovo = (qtde * preco);

    $(subtotal).html("R$ " + valorNovo.toFixed(2));

    total = total + valorNovo;

    $('#total b').html('R$ ' + total.toFixed(2).toLocaleString());
}

$(document).ready(function () {
    $('button[class*="bootstrap-touchspin"]').click(function () {
        CalculaPrecos(this);
    });

    $('.remove_cart').click(function () {
        var tr = $(this).parentsUntil('tbody');

        var subtotal = $(tr).find('td[id*="subtotal"]');

        var valorAntigo = parseFloat($(subtotal).html().replace('R$ ', '').replace(',', '.'));

        var total = parseFloat($('#total b').html().replace('R$ ', '').replace(',', '.'));

        total = total - valorAntigo;

        $('#total b').html('R$ ' + total.toFixed(2).toLocaleString());

        $(tr).remove();
        return false;
    })

    $('input[name="Quantidade"]').focusout(function () {
        CalculaPrecos(this);
    })
})