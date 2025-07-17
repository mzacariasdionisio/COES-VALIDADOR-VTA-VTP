var controlador = siteRoot + "compensacion/general/";

$( /**
   * Llamadas iniciales
   * @returns {} 
   */
   $( document ).ready(function() {
        
       $('#btnCancelar').click(function () {
           cancelar();
       });

       $('#btnGrabar').click(function () {
           grabarMesValorizacion();
       });

       pintarBusqueda();

   }));

var pintarBusqueda =
    /**
    * Pinta el listado de periodos según el año seleccionado
    * @returns {} 
    */
    function () {


        $.ajax({
            type: "POST",
            url: controlador + "ListarEntidades",
            data: {
                pecacodi: $("#pecacodi").val()
            },
            success: function(evt) {
                $("#listado").html(evt);
                $("#tabla").dataTable({
                    "scrollY": 314,
                    "scrollX": false,
                    "sDom": "t",
                    "ordering": false,
                    "bDestroy": true,
                    "bPaginate": false,
                    "iDisplayLength": 50
                });
            },
            error: function() {
                mostrarMensaje("mensaje", $mensajeErrorGeneral, $tipoMensajeError, $modoMensajeCuadro);
            }
        });
    };

function obtenerData(id, proceso) {

    var metodo = "";

    if (proceso == "VCE_ENERGIA")
    {
        metodo = "obtenerDataEnergia";
    }

    if (proceso == "VCE_HORA_OPERACION")
    {
        metodo = "obtenerDataHorasOperacion";
    }

    if (proceso == "TRN_COSTO_MARGINAL") {
        metodo = "";
    }

    if (proceso == "VCE_DATCALCULO") {
        metodo = "obtenerDataCalculo";
    }

    if (metodo == "")
    {
        alert("Proceso no registrado");
        return false;
    }

    $.ajax({
        type: 'POST',
        url: controlador + metodo,
        data: {
            id: id,
            pecacodi: $("#pecacodi").val()
        },
        dataType: 'json',
        beforeSend: function () {
            mostrarAlerta("Obteniendo Información ...");
        },
        success: function (result) {
            mostrarExito("Registros obtenidos.");
            pintarBusqueda();
        },
        error: function () {
            mostrarError();
        }
    });
}

function grabarMesValorizacion() {

    if ($('#version').val() == "" || $('#version').val() == null) {
        alert("Debe seleccionar una versión de costo marginal.");
        return false;
    }

    if ($('#tc').val() == "" || $('#tc').val() == null) {
        alert("Debe ingresar el tipo de cambio.");
        return false;
    }

    if (confirm("¿Desea guardar la información?")) {

        $.ajax({
            type: 'POST',
            dataType: 'json',
            url: controlador + "GrabarMesValorizacion",
            data: {
                pecacodi: $('#pecacodi').val(),
                tc: $('#tc').val(),
                version: $('#version').val()
            },
            beforeSend: function () {
                mostrarAlerta("Guardando Información ..");
            },
            success: function (evt) {
                mostrarExito("Registro actualizado correctamente.");
                pintarBusqueda();
            },
            error: function () {
                mostrarError();
            }
        });
    }
}

function soloNumeros(e) {
    var key = window.Event ? e.which : e.keyCode
    return (key <= 13 || (key >= 48 && key <= 57) || key == 46);
}

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

var editarRegistro = function (id) {
    window.location.href = controlador + "EditMesValorizacion?periodo=" + id;
}

function cancelar() {
    window.location.href = controlador + "MesValorizacion";
}