var controlador = siteRoot + 'informesosinergmin/informe/';
$(function(){

    $('#txtFecha').Zebra_DatePicker({
        format: 'm Y',
        direction:-1
    });
    
    $('#btnConsultar').on('click', function () {
        consultar();
    })
});

consultar = function () {
    
    $.ajax({
        type:'POST',
        url: controlador + 'detalle',
        data: {
            fecha: $('#txtFecha').val()
        },
        success: function (evt) {
            $('#listado').html(evt);
            $('#tab-container').easytabs({
                animate: false
            });

            $('#tabla1').dataTable({
                "iDisplayLength": 50
            });
            $('#tabla2').dataTable({
                "iDisplayLength": 50
            });
            $('#tabla3').dataTable({
                "iDisplayLength": 50
            });

            $('#cbDiaFin').val($('#hfDiaFin').val());

            $('#btnComparativo').on('click', function () {
                comparativo();
            });

            comparativo();
        },
        error: function () {
            mostrarMensaje('mensaje', 'error', 'Se ha producido un error.');
        }
    });
}

comparativo = function () {
    var inicio = $('#cbDiaInicio').val();
    var fin = $('#cbDiaFin').val();

    if (inicio <= fin) {
        
        $.ajax({
            type: 'POST',
            url: controlador + 'comparativo',
            data: {
                fecha: $('#txtFecha').val(),
                diaInicio: inicio,
                diaFin: fin
            },            
            success: function (evt) {
                $('#comparativo').html(evt);
                $('#tablaComparitivo').dataTable({
                    "iDisplayLength": 50
                });
            },
            error: function () {
                mostrarMensaje('mensaje', 'error', 'Se ha producido un error.');
            }
        });
    }
    else {
        mostrarMensaje('mensaje', 'alert', 'El día final no puede ser menor que el inicial');
    }
}

mostrarMensaje = function (id, tipo, mensaje) {
    $('#' + id).removeClass();
    $('#' + id).addClass('action-' + tipo);
    $('#' + id).html(mensaje);
}