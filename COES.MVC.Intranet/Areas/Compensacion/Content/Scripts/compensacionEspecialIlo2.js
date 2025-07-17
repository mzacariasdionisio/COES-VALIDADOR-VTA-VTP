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

       mostrarExito("Registre las compensaciones especiales Ilo 2");

       $('#fecha').Zebra_DatePicker({
           format: 'd/m/Y'
       });

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

       $("#aptopcodi").change(function ()
       {
           ObtenerSubTipo(this.value, '');
           asignarTituloCarga(this.value);
       }
       );

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
            url: controlador + "listarCompensacionesEspecialesIlo2",
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
    $("#fecha").prop("disabled", false);

    $('#modoOperacion').val("");
    var currentdate = new Date();
    var ahora = currentdate.getDate().toString().lpad("0", 2) + "/" + (currentdate.getMonth() + 1).toString().lpad("0", 2) + "/" + currentdate.getFullYear();
    $('#fecha').val(ahora);
    $('#aptopcodi').val("");
    $('#apstocodi').val("");
    $('#apespcargafinal').val(0);    
    
    $("#popupEdicion").bPopup({
        autoClose: false
    });
};

function eliminarCompensacionEspecialIlo2(pecacodi, grupoCodi, apespFechadesc, apstoCodi) {

    if (confirm("¿Desea eliminar la compensación especial Ilo 2?")) {
        $.ajax({
            type: 'POST',
            dataType: 'json',
            url: controlador + "EliminarCompensacionEspecialIlo2",
            data: {
                pecacodi: pecacodi,
                grupoCodi: grupoCodi,
                apespFechadesc: apespFechadesc,
                apstoCodi: apstoCodi
            },
            success: function (result) {

                if (result.success) {
                    mostrarExito("Se ha eliminado la compensación especial Ilo 2")
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

/*
* Guarda la edición de la compensación especial Ilo 2
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

    var fecha = $('#fecha').val();
    if (fecha == "" || fecha == null) {
        alert('Por favor seleccione la fecha');
        return;
    }

    var aptopcodi = $('#aptopcodi').val();
    if (aptopcodi == "" || aptopcodi == null) {
        alert('Por favor seleccione el tipo');
        return;
    }

    var apstocodi = $('#apstocodi').val();
    if (apstocodi == "" || apstocodi == null) {
        alert('Por favor seleccione el sub tipo');
        return;
    }

    var apespcargafinal = $('#apespcargafinal').val();
    if (apespcargafinal == "" || apespcargafinal == null) {
        alert('Por favor proporcione la carga final');
        return;
    }

    $.ajax({
        type: 'POST',
        url: controlador + 'GuardarCompensacionEspecialIlo2',
        data: {
            pecacodi: pecacodi,
            grupoCodi: grupocodi,
            apespFechaDesc: fecha,
            apespTipo: aptopcodi,
            aptopSubtipo: apstocodi,
            apespCargaFinal: apespcargafinal
        },
        dataType: 'json',
        success: function (result) {

            if (result.success) {
                $('#popupEdicion').bPopup().close();
                pintarBusqueda();
                mostrarExito("Se ha creado la compensación especial Ilo 2");
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

function ObtenerSubTipo(valor, selected) {

    if (valor != "") {
        $.ajax({
            type: 'POST',
            url: controlador + 'obtenerListaTipoOpera',
            data: {
                tipo: valor
            },
            dataType: 'json',
            success: function (data) {
                var jsonData = JSON.parse(data);
                dwr.util.removeAllOptions("apstocodi");
                dwr.util.addOptions("apstocodi", jsonData, 'id', 'name');
                dwr.util.setValue("apstocodi", selected);
            },
            error: function () {
                mostrarMensaje('mensaje', 'Ha ocurrido un error', $tipoMensajeError, $modoMensajeCuadro);
            }
        });
    }
    else {
        dwr.util.removeAllOptions("apstocodi");
    }
}

function asignarTituloCarga(valor) {
    var titulo;
     
    switch(valor){
        case "A":
            titulo = "Carga final*:";
            break;
 
        case "P":
            titulo = "Carga inicial*:";
            break;
    }
 
    $('#tituloCarga').html(titulo);    
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