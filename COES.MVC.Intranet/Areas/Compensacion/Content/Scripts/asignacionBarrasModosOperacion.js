var controlador = siteRoot + "compensacion/general/";

$( /**
   * Llamadas iniciales
   * @returns {} 
   */
   $(document).ready(function () {
       //mostrarMensaje("Selecionar el periodo y la versión");    
       pintarBusqueda();
       $('#btnAgregar').click(function () {
           agregar();
       });      

       $('#btnRegresar').click(function () {
           regresar();
       });

       $('#fechaVigencia').Zebra_DatePicker({
           format: 'd/m/Y'
       });

       $('#btnGuadarEdicion').click(function () {
           guardarEdicion();
       });

       $('#btnCancelarEdicion').click(function () {
           cancelarEdicion();
       });

   }));


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

function agregar()
{
    $('#esNuevo').val(1);
    $('#barrcodi').val('');
    $('#fechaVigencia').val('');

    $("#popupEdicion").bPopup({
        autoClose: false
    });
}

function regresar() {
    window.location.href = controlador + "ModosOperacion";
}

function pintarBusqueda() {
    $.ajax({
        type: "POST",
        url: controlador + "listarAsignacionBarras",
        data: {
            grupocodi: $("#hdnGrupocodi").val()
        },
        success: function (evt) {

            $('#listado').css("width", $('#mainLayout').width() + "px");
            $('#listado').html(evt);
            $('#tabla').dataTable({
                "ordering": false,
                "paging": false,
                "scrollY": 340,
                "scrollX": true,
                "bDestroy": true
            });
            mostrarMensaje("Consulta generada");
        },
        error: function () {
            mostrarError('Opcion Consultar: Ha ocurrido un error');
        }
    });
}

function modificarAsignacion(formuladat, fechadat) {
    $('#esNuevo').val(0);
    $('#barrcodi').val(formuladat);
    $('#fechaVigencia').val(fechadat);

    $("#popupEdicion").bPopup({
        autoClose: false
    });
}

function guardarEdicion() {

    var fechaVigencia = $('#fechaVigencia').val();
    if (fechaVigencia == "") {
        alert('Debe ingresar una fecha correcta');
        return;
    }

    var grupocodi = $('#hdnGrupocodi').val();
    if (grupocodi == "" || grupocodi == null) {
        alert('El modo de operación no es válido');
        return;
    }

    var barrcodi = $('#barrcodi').val();
    if (barrcodi == "" || barrcodi == null) {
        alert('Debe seleccionar una barra');
        return;
    }
    var esNuevo = false;

    if ($('#esNuevo').val() == 1) {
        esNuevo = true;
    }

    $.ajax({
        type: 'POST',
        url: controlador + 'GuardarAsignacionBarra',
        data: {
            fechaVigencia: fechaVigencia,
            grupocodi: grupocodi,
            barrcodi: barrcodi,            
            esNuevo: esNuevo
        },
        dataType: 'json',
        success: function (result) {

            if (result.success) {
                $('#popupEdicion').bPopup().close();
                pintarBusqueda();
                if (esNuevo) {
                    alert("Se ha creado la asignación correctamente");
                }
                else {
                    alert("Se ha modificado la asignación correctamente");
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
