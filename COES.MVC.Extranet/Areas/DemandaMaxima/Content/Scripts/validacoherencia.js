var controlador = siteRoot + 'demandaMaxima/validacion/';
var uploader;
var totNoNumero = 0;
var totLimSup = 0;
var totLimInf = 0;
var validaInicial = true;
var hot;
var hotOptions;
var evtHot;

$(function () {

    $('#txtMes').Zebra_DatePicker({
        format: 'm Y'
    });
    
    $(document).keypress(function (event) {
        tecla = (document.all) ? event.keyCode : event.which;
        //Tecla de retroceso para borrar, siempre la permite
        if (tecla == 8) {
            return true;
        }
        // Patron de entrada, en este caso solo acepta numeros
        patron = /[0-9]/;
        tecla_final = String.fromCharCode(tecla);
        return patron.test(tecla_final);
    });

    $('#btnCerrar').click(function () {
        OpenUrl('', 'F', 506, 'demandaMaxima/envio', 'index');
    });

    $('#btnValidar').click(function () {

        if ($('#txtVariacion').val() == "")
        {
            alert("Debe colocar el porcentaje de Variación.");
            return false;
        }

        if (!isNaN($('#txtVariacion').val()) == false) {
            alert("Debe colocar un número correcto.");
            $('#txtVariacion').val("")
            $('#txtVariacion').focus();
            return false;
        }

        $.ajax({
            type: 'POST',
            url: controlador + 'ListarObservaciones',
            data: {
                idEmpresa: $('#cbEmpresa').val(),
                mes: $('#txtMes').val(),
                variacion: $('#txtVariacion').val(),
                consumo: $('#cbConsumo').val()
            },
            success: function (evt) {
                $('#listado').html(evt);
                $('#tabla').dataTable({
                    "scrollY": 314,
                    "scrollX": false,
                    "sDom": 't',
                    "ordering": false,
                    "bDestroy": true,
                    "bPaginate": false,
                    "iDisplayLength": 50
                });
            },
            error: function () {
                alert("Error al obtener la consulta");
            }
        });  
    });

    $('#btnExportar').click(function () {
        exportar();
    });
});

function mostrarExito(mensaje) {
    $('#mensaje').removeClass();
    $('#mensaje').html(mensaje);
    $('#mensaje').addClass('action-exito');
}

function mostrarError(mensaje) {
    $('#mensaje').removeClass();
    $('#mensaje').html(mensaje);
    $('#mensaje').addClass('action-error');
}

function mostrarAlerta(mensaje) {
    $('#mensaje').removeClass();
    $('#mensaje').html(mensaje);
    $('#mensaje').addClass('action-alert');
}

function mostrarMensaje(mensaje) {
    $('#mensaje').removeClass();
    $('#mensaje').html(mensaje);
    $('#mensaje').addClass('action-message');
}


exportar = function () {
    //formato = 1 [Excel]
    var formato = "1";
    if ($('#txtVariacion').val() == "") {
        alert("Debe colocar el porcentaje de Variación");
        return false;
    }

    $.ajax({
        type: 'POST',
        url: controlador + 'exportar',
        data: {
            idEmpresa: $('#cbEmpresa').val(),
            mes: $('#txtMes').val(),
            variacion: $('#txtVariacion').val(),
            consumo: $('#cbConsumo').val()
        },
        dataType: 'json',
        success: function (result) {
            if (result != -1) {
                document.location.href = controlador + 'descargar?formato=' + formato + '&file=' + result
            }
            else {
                mostrarMensaje('mensaje', 'Ha ocurrido un error', $tipoMensajeError, $modoMensajeCuadro);
            }
        },
        error: function () {
            mostrarMensaje('mensaje', 'Ha ocurrido un error', $tipoMensajeError, $modoMensajeCuadro);
        }
    });
}
