$(document).ready(function () {
    $('.itemValorUnitario').mask('9xxx,99', { translation: { 'x': { pattern: /[0-9]/, optional: true } } });

    $('.itemValorTotal').mask('9xxx,99', { translation: { 'x': { pattern: /[0-9]/, optional: true } } });

    $('.itemValorUnitario').change(function () {
        var quantidade = parseFloat($(this).closest('tr').find("input[id$='Quantidade']").val().replace(',', '.'));
        var valorUnitario = parseFloat($(this).val().replace(',', '.'));
        var valorTotal = quantidade * valorUnitario;

        if (isNaN(valorTotal)) {
            $(this).closest('tr').find("input[id$='ValorTotal']").val('0,00');
        }
        else {
            $(this).closest('tr').find("input[id$='ValorTotal']").val(valorTotal.toFixed(2).replace('.', ','));
        }
    });

    $(document).on('click', '#btnExportar', function (e) {
        var table = $('#produtos').html();

        $('.tdItens').remove();

        $(".valorUnitario").each(function (index) {
            $(this).html($(this).children().val());
        });

        $('#produtos').tableExport({ type: 'excel', escape: 'false', tableName:'Itens' });

        $('#produtos').html(table);
    });

    $(document).on('click', '#btnImportar', function (e) {
        var formData = new FormData($('#formCotacao')[0]);

        $.ajax({
            type: 'POST',
            url: '/Vendas/ImportarItens',
            data: formData,
            cache: false,
            contentType: false,
            processData: false,
            success: function (response) {
                if (response.result == 'Erro') {
                    $('#modalAlertBody').html(response.erro);
                    $('#modalAlert').modal('show');
                }
                else if (response.result == 'Redirect') {
                    //redirecting to main page from here for the time being.
                    window.location = response.url;
                }
            },
            error: function (response) {
                $('#modalAlertBody').html(response.erro);
                $('#modalAlert').modal('show');
            }
        });
    });
})