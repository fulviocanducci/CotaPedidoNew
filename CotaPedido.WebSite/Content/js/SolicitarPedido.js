function Chosen(target, func) {
    if (typeof func == 'function') {
        $(target).chosen({ no_results_text: "Sem registros para:", width: "100%" }).change(func)
    }
    else {
        $(target).chosen({ no_results_text: "Sem registros para:", width: "100%" })
    }
}

$(document).ready(function () {
    $('#dataLimiteProposta').mask('99/99/9999');
    Chosen('#categoria');
})