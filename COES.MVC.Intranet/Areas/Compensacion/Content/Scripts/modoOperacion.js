var controlador = siteRoot + "compensacion/general/";

$( /**
   * Llamadas iniciales
   * @returns {} 
   */
   $(document).ready(function () {

       $('#btnCancelar').click(function () {
           cancelar();
       });

       $('#btnAgregar').click(function () {
           grabar();
       });

       pintarBusqueda();
   }));

function grabar() {

    if (confirm("¿Desea guardar la información?")) {
        $.ajax({
            type: 'POST',
            dataType: 'json',
            url: controlador + "SaveVcePtomedModope",
            data: {
                pecacodi: $("#pecacodi").val(),
                ptomedicodi: $('#ptomedicodi').val(),
                grupocodi: $('#grupocodi').val(),
            },
            beforeSend: function () {
                mostrarMensaje("Guardando Información ..");
            },
            success: function (evt) {

                if (evt.mensaje != "") {
                    mostrarExito(evt.mensaje);

                    if ($('#medicodi').val() == "0") {
                        $('#ptomedicodi').attr('disabled', 'disabled');
                    }
                }
                else {
                    mostrarError("El punto ya se encuentra registrado.");
                }

                pintarBusqueda();
            },
            error: function () {
                mostrarError();
            }
        });
    }

}

function eliminar(grupocodi) {

    if (confirm("¿Desea eliminar el registro?")) {
        $.ajax({
            type: 'POST',
            dataType: 'json',
            url: controlador + "DeleteVcePtomedModope",
            data: {
                pecacodi: $("#pecacodi").val(),
                ptomedicodi: $('#ptomedicodi').val(),
                grupocodi: grupocodi,
            },
            beforeSend: function () {
                mostrarAlerta("Eliminando el registro ..");
            },
            success: function (evt) {
                mostrarExito("Registro eliminado correctamente.");
                pintarBusqueda();
            },
            error: function () {
                mostrarError();
            }
        });
    }

}

var pintarBusqueda =
    /**
    * Pinta el listado de periodos según el año seleccionado
    * @returns {} 
    */
    function () {

        $.ajax({
            type: "POST",
            url: controlador + "listarModosOperacion",
            data: {
                pecacodi: $("#pecacodi").val(),
                ptoMedicodi: $("#ptomedicodi").val()
            },
            success: function (evt) {
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

var editarRegistro = function (id) {
    window.location.href = controlador + "EditPtoGrupo?id="+id;
}

function cancelar() {
    window.location.href = controlador + "GrillaPtoGrupo";
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

function mostrarMensajeFecha(mensaje) {
    $('#mensajefecha').removeClass();
    $('#mensajefecha').html(mensaje);
    $('#mensajefecha').addClass('action-message');
}