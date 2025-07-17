var controlador = siteRoot + 'operacion/transferencias/';

$(function () {
    $('#cbAnio').val($('#hfAnio').val());
    $('#cbMes').val($('#hfMes').val());

    $('#btnExportar').click(function () {
        exportar();
    });

    $('#cbAnio').change(function () {
        cargarBarra();
    });

    $('#cbMes').change(function () {
        cargarBarra();
    });

    cargarBarra();
});

cargarBarra = function () {
    var flag = validarFecha();

    $('#labelBarra').hide();
    $('#controlBarra').hide();
    $('#labelBarraDTR').hide();
    $('#controlBarraDTR').hide();

    if (flag) {
        $('#labelBarra').show();
        $('#controlBarra').show();
    }
    else {
        $('#labelBarraDTR').show();
        $('#controlBarraDTR').show();
    }
}

exportar = function () {

    var flag = validarFecha();

    if (($('#cbBarra').val() != "" && flag) || ($('#cbBarraDTR').val() != "" && !flag)) {

        var barra = (flag) ? $('#cbBarra').val() : $('#cbBarraDTR').val();

        let dataPost = { anio: $('#cbAnio').val(), mes: $('#cbMes').val(), barra: barra };

        $.ajax({
            type: 'POST',
            url: controlador + 'exportar',
            data: dataPost,
            dataType: 'json',
            success: function (result) {                
                if (result == 1) {
                    location.href = controlador + 'descargar';
                    showMensajeExito('Archivo generado correctamente');
                }
                else {
                    showMensajeError('Se ha producido un error de datos.');
                }
            },
            error: function () {
                showMensajeError('Se ha producido un error.');
            }
        });
    }
    else {
        showMensajeAlerta('Seleccione la barra');
    }
}

validarFecha = function () {
    var anio = parseInt($('#cbAnio').val());
    var mes = parseInt($('#cbMes').val());

    if (anio === 2015 && mes <= 7) {
        return false;
    }
    if (anio < 2015) {
        return false;
    }
    return true;
}

//mostrarError = function () {
//    $('#mensaje').removeClass();
//    $('#mensaje').addClass('action-error');
//    $('#mensaje').text('Se ha producido un error.');
//}

//mostrarAlerta = function (mensaje) {
//    $('#mensaje').removeClass();
//    $('#mensaje').addClass('action-alert');
//    $('#mensaje').html(mensaje);
//}

//mostrarExito = function (mensaje) {
//    $('#mensaje').removeClass();
//    $('#mensaje').addClass('action-exito');
//    $('#mensaje').html(mensaje);
//}

showMensajeAlerta = function (mensaje) {
    $('#mensaje').removeClass();
    $("#mensaje").css("display", "flex");
    $('#mensaje').addClass('coes-form-item--info coes-form-item coes-box coes-box--content pt-2 pe-3 ps-3 pb-2 mt-3');
    $('#mensaje').html(mensaje);
}

showMensajeError = function (mensaje) {
    $('#mensaje').removeClass();
    $("#mensaje").css("display", "flex");
    $('#mensaje').addClass('coes-form-item--error coes-form-item coes-box coes-box--content pt-2 pe-3 ps-3 pb-2 mt-3');
    $('#mensaje').html(mensaje);
}

showMensajeExito = function (mensaje) {
    $('#mensaje').removeClass();
    $("#mensaje").css("display", "flex");
    $('#mensaje').addClass('coes-form-item--info coes-form-item coes-box coes-box--content pt-2 pe-3 ps-3 pb-2 mt-3');
    $('#mensaje').html(mensaje);
}
