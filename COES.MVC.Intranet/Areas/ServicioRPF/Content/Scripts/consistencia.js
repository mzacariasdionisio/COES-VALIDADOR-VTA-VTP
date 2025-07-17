var controlador = siteRoot + 'serviciorpf/'

$(function () {

    $('#FechaConsulta').Zebra_DatePicker({
    });

    $('#btnConsultar').click(function () {
        verificar();
    });

    $('#btnExportar').click(function () {
        exportar();
    });

    $('#btnReProcesar').click(function () {
        mostrar(1);
        $('#popupUnidad').bPopup().close();
    });

    $('#btnNoReProcesar').click(function () {
        mostrar(0);
        $('#popupUnidad').bPopup().close();
    });

    $('#tab-container').easytabs({
        animate: false
    });

    cargarParametros();
});

verificar = function (){
    $.ajax({
        type: 'POST',
        global: false,
        url: controlador + 'consistencia/consultarexistencia',
        dataType: 'json',
        data: { fecha: $('#FechaConsulta').val() },
        cache: false,
        success: function (resultado) {
            if (resultado == 1) {               
                $('#popupUnidad').bPopup({
                    easing: 'easeOutBack',
                    speed: 450,
                    transition: 'slideDown'
                });
            }
            else if (resultado == 0) {
                mostrar(1);
            }
            else mostrarError();
        },
        error: function () {
            mostrarError();
        }
    });
}

mostrar = function (indicador) {
    $.ajax({
        type: "POST",
        url: controlador + "consistencia/consulta",
        data: { fecha: $('#FechaConsulta').val(), indicador: indicador },
        success: function (evt) {
            $('#listado').html(evt);
            oTable = $('#tabla').dataTable({
                "sPaginationType": "full_numbers",
                "aaSorting": [[0, "asc"]],
                "destroy": "true",
                "aoColumnDefs": [
                     { 'bSortable': false, 'aTargets': [5] }
                ]
            });
        },
        error: function () {
            mostrarError();
        }
    });
}

exportar= function(){
    $.ajax({
        type: 'POST',
        url: controlador + 'consistencia/exportar',     
        dataType: 'json',
        success: function (result) {
            if (result == 1) {
                window.location = controlador + "consistencia/descargar";
            }
            if (result == -1) {
                mostrarError();
            }
        },
        error: function () {
            mostrarError();
        }
    });
}

cargarParametros = function () {
    $.ajax({
        type: "POST",
        url: controlador + "consistencia/configuracion",
        success: function (evt) {
            $('#parametros').html(evt);
        },
        error: function () {
            mostrarError();
        }
    });
}

grabarParametro = function () {

    if ($('#txtPorcentaje').val() == "" || $('#txtPercentil').val() == "" ||
        $('#txtPotencia').val() == "" ) {
        mostrarMensaje("Debe ingresar todo los parámetros.");
    }
    else {
        $.ajax({
            type: 'POST',
            url: controlador + 'consistencia/grabarparametro',
            data: {
                porcentaje: $('#txtPorcentaje').val(), percentil: $('#txtPercentil').val(),
                potencia: $('#txtPotencia').val()
            },
            dataType: 'json',
            cache: false,
            success: function (result) {
                if (result == 1) {
                    mostrarExito("Los parámetros fueron modificados correctamente.");
                }
                else {
                    mostrarError();
                }
            },
            error: function () {
                mostrarError();
            }
        });
    }
}


validarNumero = function (item, evt) {
    evt = (evt) ? evt : window.event;
    var charCode = (evt.which) ? evt.which : evt.keyCode;

    if (charCode == 46) {
        var regex = new RegExp(/\./g)
        var count = $(item).val().match(regex).length;
        if (count > 1) {
            return false;
        }
    }

    if (charCode > 31 && (charCode < 48 || charCode > 57)) {
        return false;
    }

    return true;
}

mostrarError = function () {
    alert("Ha ocurrido un error.");
}

mostrarExito = function (mensaje) {
    alert(mensaje);
}