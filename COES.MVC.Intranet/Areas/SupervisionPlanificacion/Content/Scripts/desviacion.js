var controler = siteRoot + "supervisionPlanificacion/desviacion/";

$(document).ready(function () {
   
    $('#buscar').click(function () {
        buscar();
    });

    $('#txtFecha').Zebra_DatePicker({
        onSelect: function () {
            buscar();
        }
    });

    buscar();
});

function buscar() {
    $.ajax({
        type: 'POST',
        url: controler + "grilla",
        data: { fecha: $('#txtFecha').val() },
        success: function (evt) {
            $('#listado').html(evt);
            $('#tabla').dataTable({ 
            });
        },
        error: function () {
            mostrarErrorFormato();
        }
    });
}

mostrarErrorFormato = function () {
    alert("Error en el formato.");
}

function loadInfoFile(fileName, fileSize) {
    $('#fileInfo').html(fileName + " (" + fileSize + ")");
    $('#fileInfo').removeClass();
    $('#fileInfo').addClass('action-exito');
    $('#fileInfo').css('margin', '0px');
    $('#fileInfo').css('padding', '5px');
}

function loadValidacionFile(mensaje) {
    $('#fileInfo').html(mensaje);
    $('#fileInfo').removeClass();
    $('#fileInfo').addClass('action-alert');
    $('#fileInfo').css('margin', '0px');
    $('#fileInfo').css('padding', '5px');
}

function mostrarProgreso(porcentaje) {
    $('#fileInfo').text(porcentaje + "%");
    $('#fileInfo').removeClass();
    $('#fileInfo').addClass('action-exito');
    $('#fileInfo').css('margin', '0px');
    $('#fileInfo').css('padding', '5px');
}

function mostrarErrorFile(mensaje)
{
    $('#fileInfo').text(mensaje);
    $('#fileInfo').removeClass();
    $('#fileInfo').addClass('action-error');
    $('#fileInfo').css('margin', '0px');
    $('#fileInfo').css('padding', '5px');
}

function procesarArchivo() {
    
    var flag = true;
    if ($('#hfIndicador').val() == "S")
    {
        if (!confirm('¿Existen datos, desea reemplazar?'))
        {
            flag = false;
        }
    }

    if (flag) {
        $.ajax({
            type: 'POST',
            url: controler + 'procesararchivo',
            dataType: 'json',
            data: { fecha: $('#txtFecha').val() },
            cache: false,
            success: function (resultado) {
                if (resultado == 1) {
                    buscar();
                    $('#mensaje').removeClass();
                    $('#mensaje').text("");
                    $('#fileInfo').text("Archivo procesado correctamente.");
                    $('#fileInfo').removeClass();
                    $('#fileInfo').addClass('action-exito');
                    $('#fileInfo').css('margin', '0px');
                    $('#fileInfo').css('padding', '5px');
                }
                else if(resultado == 0){
                    mostrarError("No existen datos en el formato");
                }
                else if (resultado == 2) {
                    mostrarMensaje("Existen datos incorrectos en el formato");
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

mostrarMensaje = function (mensaje) {
    $('#mensaje').removeClass();
    $('#mensaje').text(mensaje);
    $('#mensaje').addClass('action-alert');
}

mostrarError = function () {
    $('#mensaje').removeClass();
    $('#mensaje').text("Ha ocurrido un error");
    $('#mensaje').addClass('action-error');
}