function GerarIndex() {
    var indexes = $('input.index');
    var index = -1;
    var valid = true;
    for (var i = 0; i < 300; i++) {
        for (var j = 0; j < indexes.length ; j++) {
            if (i == $(indexes[j]).val()) {
                valid = false;
                break;
            }
        }
        if (valid)
            return i;
        else
            valid = true;
    }
    return 0;
}

function CarregarSubcategorias() {
    var value = $('#categoria').val();
    value = parseInt(value);

    if (!isNaN(value) && value > 0) {
        $.ajax({
            type: 'POST',
            url: '/Compras/GetSubCategorias',
            data: { idCategoria: value },
            success: function (subcategorias) {
                $('#subcategoria option:gt(0)').remove();
                subcategorias.forEach(function (subcat, i) {
                    $('#subcategoria').append('<option value="' + subcat.Id + '">' + subcat.Nome + '</option>');
                });
                //$('#subcategoria').trigger('chosen:updated');
            },
            error: function () {

            }
        });
    } else {
        $('#subcategoria option:gt(0)').remove();
        $('#subcategoria').trigger('chosen:updated');
    }
}

function BuscarInserircategoria() {
    var value = $('#categoria').val();
    console.log('Evento para inserir ou selecionar categoria ID = ' + value);
}

function Chosen(target, func) {
    if (typeof func == 'function') {
        $(target).chosen({ no_results_text: "Sem registros para:", width: "100%" }).change(func)
    }
    else {
        $(target).chosen({ no_results_text: "Sem registros para:", width: "100%" })
    }
}

function IncluirProdutoPedido() {
    var $categoria = $('#categoria option:selected');
    var $subcategoria = $('#subcategoria option:selected');
    var $unidade = $('#unidade option:selected');
    var produto = $('#produto').val();
    var quantidade = $('#quantidade').val();
    var erro = '';

    quantidade = typeof quantidade == 'undefined' ? NaN : parseInt(quantidade);
    var idcategoria = $categoria.val();
    var nomecategoria = $categoria.text();

    var idSubcategoria = $subcategoria.val();
    var nomeSubcategoria = $subcategoria.text();

    var idUnidade = $unidade.val();
    var nomeUnidade = $unidade.text();

    if (typeof idUnidade == 'undefined' || isNaN(parseInt(idUnidade))) {
        erro += '<li>O campo unidade é obrigatório</li>';
    }
    if (isNaN(quantidade) || !isNaN(quantidade) && quantidade > 1000 || !isNaN(quantidade) && quantidade < 1) {
        erro += '<li>O campo Quantidade é obrigatório e deve estar entre 1 e 1000</li>';
    }
    if (typeof idcategoria == 'undefined' || isNaN(parseInt(idcategoria))) {
        erro += '<li>O campo categoria é obrigatório</li>';
    }
    if (typeof idSubcategoria == 'undefined' || isNaN(parseInt(idSubcategoria))) {
        erro += '<li>O campo Subcategoria é obrigatório</li>';
    }
    if (typeof produto == 'undefined' || produto == '') {
        erro += '<li>O campo Produto é obrigatório</li>';
    } else {
        $('[name*="NomeProduto"]').each(function () {
            if ($(this).val() == produto) {
                erro += "<li>Este Produto já está incluido em sua lista</li>";
            }
        })
    }

    if (erro != '') {
        $('#modalAlert .modal-body').html('<h3>Os seguintes erros foram encontrados:</h3><ul class="has-error">' + erro + '</ul>');
        $('#modalAlert').modal('show');
    } else {
        produtosInseridos = GerarIndex();
        var templateProduto = '<tr><td>{NomeProduto}<input type="hidden" value="{index}" class="index" disabled><input type="hidden" name="Itens[{index}].Excluido" value="false" /><input type="hidden" name="Itens[{index}].NomeProduto" value="{NomeProduto}" /></td><td>{Nomecategoria}<input type="hidden" name="Itens[{index}].Idcategoria" value="{Idcategoria}" /></td><td>{NomeSubcategoria}<input type="hidden" name="Itens[{index}].IdSubcategoria" value="{IdSubcategoria}" /></td><td>{NomeUnidade}<input type="hidden" name="Itens[{index}].IdUnidade" value="{IdUnidade}" /></td><td><input type="text" name="Itens[{index}].Quantidade" value="{Quantidade}" class="input-qty form-control text-center" /></td><td class="text-center content-middle"><a href="#" class="remove_cart" data-index="{index}" rel="2"><i class="fa fa-trash-o fa-2x"></i></a></td></tr>';
        templateProduto = templateProduto.replace('{NomeProduto}', produto)
            .replace('{NomeProduto}', produto)
            .replace('{NomeSubcategoria}', nomeSubcategoria)
            .replace('{IdSubcategoria}', idSubcategoria)
            .replace('{Idcategoria}', idcategoria)
            .replace('{Nomecategoria}', nomecategoria)
            .replace('{Quantidade}', quantidade)
            .replace('{NomeUnidade}', nomeUnidade)
            .replace('{IdUnidade}', idUnidade)
            .replace('{index}', produtosInseridos)
            .replace('{index}', produtosInseridos)
            .replace('{index}', produtosInseridos)
            .replace('{index}', produtosInseridos)
            .replace('{index}', produtosInseridos)
            .replace('{index}', produtosInseridos)
            .replace('{index}', produtosInseridos)
            .replace('{index}', produtosInseridos)
        produtosInseridos++;

        templateProduto = $(templateProduto);
        RemovePedido(templateProduto.find('.remove_cart'));
        $(templateProduto.find('.input-qty')).TouchSpin();
        $('#produtos').prepend(templateProduto);

        //$('#categoria option[value=""]').attr('selected', ' ');
        //$('#subcategoria option[value=""]').attr('selected', ' ');
        //$('#categoria').trigger('chosen:updated');
        //$('#subcategoria').trigger('chosen:updated');
        $('#produto').val('');
        $('#quantidade').val('');
    }
}

function RemovePedido(target) {
    $(target).click(function () {
        var index = $(this).attr("data-index");
        var tr = $(this).parentsUntil('tbody');
        $("input[name='Itens[" + index + "].Excluido']").val(true);
        $(tr).css("display", "none");
        return false;
    })
}

$(document).ready(function () {
    //Chosen('#categoria', CarregarSubcategorias);
    //Chosen('#subcategoria');
    CarregarSubcategorias();

    $('.input-qty').TouchSpin();

    $('#categoria').change(function () {
        CarregarSubcategorias();
    });

    $('#quantidade').mask('9xxx', { translation: { 'x': { pattern: /[0-9]/, optional: true } } });

    $('#btnIncluir').click(IncluirProdutoPedido);

    RemovePedido('.remove_cart');

    $('#btnNovaCategoria').click(function () {
        $('#newSubcategory').modal('show');
        return false;
    });

    $("#newSubcategory").on('shown.bs.modal', function () {
        $('#novaSubcategoria').focus();
    });

    $('#newSubcategory').on('hidden.bs.modal', function () {
        $('#errorNovaSubcategoria').html('');
        $('#novaSubcategoria').val('');
        $('#btnIncluirNovaSubcategoria').html('Gravar');
        $('#btnIncluirNovaSubcategoria').removeAttr('disabled');
    })

    $('#btnIncluirNovaSubcategoria').click(function () {
        $(this).attr('disabled', '');
        $(this).html('Aguarde...');
        var $erro = $('#errorNovaSubcategoria');
        var nome = $('#novaSubcategoria').val();
        var idCategoria = parseInt($('#categoria').val());
        nome = nome.trim();
        if (nome == '') {
            $erro.html('O campo Nome é obrigatório');
        } else if (isNaN(idCategoria)) {
            $erro.html('Selecione a categoria na tela de pedido');
        } else {
            $erro.html('');
            $.ajax({
                type: 'POST',
                url: '/Compras/AdicionarSubcategoria',
                data: { nome: nome, idCategoria: idCategoria },
                success: function (callback) {
                    if (callback.Success) {
                        $('#subcategoria').html(callback.Select);
                        //$('#subcategoria').trigger('chosen:updated');
                        $('#newSubcategory').modal('hide');
                        $('#produto').focus();
                    } else {
                        $erro.html(callback.Message);
                    }
                },
                error: function (callback) {
                    $erro.html('Algo de errado aconteceu. Tente novamente mais tarde.');
                }
            });
        }
    });

    $('#btnImportar').click(function () {
        var formData = new FormData();
        var idPedido = $('#inputId').val();
        var fileInput = $('#inputFile');

        if (fileInput.length > 0) {
            var file = fileInput[0].files[0];
            formData.append("file", file);
        }
        formData.append("id", idPedido);

        $.ajax({
            type: 'POST',
            url: '/Compras/ImportarItens',
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

    $('#btnCancelar').click(function () {
        var idPedido = $('#inputId').val();

        $.ajax({
            type: 'POST',
            url: '/Compras/CancelarPedido',
            data: {idPedido: idPedido},
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

    $('#novaSubcategoria').change(function () {
        var $erro = $('#errorNovaSubcategoria');
        var nome = $(this).val();
        nome = nome.trim();
        if (nome == '') {
            $erro.html('O campo Nome é obrigatório');
        } else {
            $erro.html('');
        }
    });

    $('#btnPublicar').click(function () {
        var idPedido = $('#inputId').val();

        $.ajax({
            type: 'POST',
            url: '/Compras/PublicarPedido',
            data: { idPedido: idPedido },
            success: function (response) {
                if (response.result === 'Erro') {
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