// =======================================================================================
//
// (c) COES Sinac. Todos los derechos reservados. 
//
// Autor: Henry Manuel Díaz Tuesta
// Acronimo: HDT
// Requerimiento: compensaciones
//
// Fecha creacion: 29/03/2017
// Descripcion: Archivo para la atencion del requerimiento.
//
// Historial de cambios:
// 
// Correlativo	Fecha		Requerimiento		Comentario
//
// =======================================================================================

var controlador = siteRoot + "compensacion/general/";

$( /**
   * Llamadas iniciales
   * @returns {} 
   */
   $(document).ready(function () {

       mostrarExito("Registre los cálculos manuales");

       $('#btnNuevoRegistro').click(function () {
           nuevoRegistro();
       });

       $('#btnRegresar').click(function () {
           regresar();
       });

       $('#btnGuadarEdicion').click(function () {
           guardarEdicion();
       });

       $('#btnCancelarEdicion').click(function () {
           cancelarEdicion();
       });       

       pintarBusqueda();

   }));

var pintarBusqueda =
    /**
    * Pinta el listado
    * @returns {} 
    */
    function () {


        $.ajax({
            type: "POST",
            url: controlador + "listarCalculosManuales",
            data: {
                periodo: $.get("periodo")
            },
            success: function (evt) {

                $('#listado').css("width", $('#mainLayout').width() + "px");
                $('#listado').html(evt);
                $('#tabla').dataTable({
                    "ordering": false,
                    "paging": false,
                    "scrollY": 350,
                    "scrollX": true,
                    "bDestroy": true
                });

            },
            error: function () {
                mostrarMensaje("mensaje", $mensajeErrorGeneral, $tipoMensajeError, $modoMensajeCuadro);
            }
        });
    };

var regresar = function () {
    parent.history.back();
};

var nuevoRegistro = function () {
    $("#modoOperacion").prop("disabled", false);

    $('#esNuevo').val(1);
    $('#modoOperacion').val([]);
    $('#apgcfccodi').val("G1");
    $('#apgcabccbef').val(0);
    
    $("#popupEdicion").bPopup({
        autoClose: false
    });
};

function modificarRegistroManual(pecaCodi, grupoCodi, apgcfccodi) {
    
    $('#esNuevo').val(0);

    $("#modoOperacion").prop("disabled", true);

    $.ajax({
        type: 'POST',
        url: controlador + 'ObtenerCalculoManual',
        data: {
            PecaCodi: pecaCodi, 
            GrupoCodi: grupoCodi,
            Apgcfccodi: apgcfccodi
        },
        dataType: 'json',
        success: function (data) {
            var jsonData = JSON.parse(data);

            $('#modoOperacion').val(jsonData.Grupocodi);
            $('#apgcfccodi').val(jsonData.Apgcfccodi);
            $('#apgcabccbef').val(jsonData.Apgcabccbef);

            $("#popupEdicion").bPopup({
                autoClose: false
            });
        },
        error: function () {
            mostrarMensaje('mensaje', 'Ha ocurrido un error', $tipoMensajeError, $modoMensajeCuadro);
        }
    });

}

/*
* Guarda la edición del cálculo manual
*/
function guardarEdicion() {

    var pecacodi = $.get("periodo");
    if (pecacodi == "" || pecacodi == null) {
        alert('No se ha podido identificar el periodo');
        return;
    }

    var grupocodi = $('#modoOperacion').val();
    if (grupocodi == "" || grupocodi == null) {
        alert('Por favor selecciones el modo de operación');
        return;
    }

    var apgcfccodi = $('#apgcfccodi').val();
    if (apgcfccodi == "" || apgcfccodi == null) {
        alert('Por favor selecciones el valor del Ccbef');
        return;
    }

    var apgcabccbef = $('#apgcabccbef').val();
    if (apgcabccbef == "" || apgcabccbef == null) {
        alert('Por favor proporcione el valor del Ccbef');
        return;
    }

    var esNuevo = false;

    if ($('#esNuevo').val() == 1) {
        esNuevo = true;
    }

    $.ajax({
        type: 'POST',
        url: controlador + 'GuardarCalculoManual',
        data: {
            Pecacodi: pecacodi,
            Grupocodi: grupocodi,
            Apgcfccodi: apgcfccodi,
            Apgcabccbef: apgcabccbef,
            EsNuevo: esNuevo
        },
        dataType: 'json',
        success: function (result) {

            if (result.success) {
                $('#popupEdicion').bPopup().close();
                pintarBusqueda();
                if (esNuevo) {
                    mostrarExito("Se ha creado el cálculo manual");
                }
                else {
                    mostrarExito("Se ha modificado el cálculo manual");
                }
            }
            else {
                alert(result.message);
            }

        },
        error: function () {
            mostrarMensaje('mensaje', 'Ha ocurrido un error', $tipoMensajeError, $modoMensajeCuadro);
        }
    });
}

function cancelarEdicion() {

    $('#popupEdicion').bPopup().close();

}

function eliminarRegistroManual(pecaCodi, grupoCodi, apgcfccodi) {
    if (confirm("¿Desea eliminar el cálculo manual?")) {
        $.ajax({
            type: 'POST',
            dataType: 'json',
            url: controlador + "EliminarCalculoManual",
            data: {
                PecaCodi: pecaCodi,
                GrupoCodi: grupoCodi,
                Apgcfccodi: apgcfccodi
            },
            success: function (result) {

                if (result.success) {
                    mostrarExito("Se ha eliminado el cálculo manual")
                    pintarBusqueda();
                }
                else {
                    alert(result.message);
                }

            },            
            error: function () {
                mostrarError();
            }
        });
    }
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

//- Permite obtener el valor del parámetro GET.
$.get = function (key) {
    key = key.replace(/[\[]/, '\\[');
    key = key.replace(/[\]]/, '\\]');
    var pattern = "[\\?&]" + key + "=([^&#]*)";
    var regex = new RegExp(pattern);
    var url = unescape(window.location.href);
    var results = regex.exec(url);
    if (results === null) {
        return null;
    } else {
        return results[1];
    }
}

function soloNumeros(e) {
    var key = window.Event ? e.which : e.keyCode
    return (key <= 13 || (key >= 48 && key <= 57) || key == 46);
}

String.prototype.lpad = function (padString, length) {
    var str = this;
    while (str.length < length)
        str = padString + str;
    return str;
}