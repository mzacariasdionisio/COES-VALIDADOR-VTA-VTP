var controlador = siteRoot + 'regionesdeseguridad/regionseguridad/';

$(function () {
    var estado = $('#cboEstadoCoord').find('option:selected').val()
    $('#btnConsultar').on('click', function () {
        consultar(estado);
    });

    $('#btnNuevaRestriccion').on('click', function () {
        var idregion = $("#hfRegion").val();
        var idTipo = $("#hfTipo").val(); 
        editarRestriccion(0, idregion,idTipo);
    });

    $('#btnRegresar').click(function () {
        verRegion();
    });

    $('#cboEstadoCoord').on('change', function () {
        var value = $(this).find('option:selected').val();
        consultar(value);
    });

    consultar(estado);
});

function verRegion() {
    location.href = siteRoot + "regionesdeseguridad/regionseguridad/Index";
};

consultar = function (estado) {
    $.ajax({
        type: 'POST',
        url: controlador + 'RestriccionList',
        data: {
            regcodi: $('#hfRegion').val(),
            idtipo: $('#hfTipo').val(),
            estado: estado
        },
        success: function (evt) {
            $('#listado').html(evt);
            $('#tablaRestriccion').dataTable({
                bJQueryUI: true,
                "scrollY": 440,
                "scrollX": false,
                "sDom": 'ft',
                "ordering": true,
                "iDisplayLength": -1
            });
        },
        error: function () {
            mostrarMensaje('mensaje', 'error', 'Ha ocurrido un error.');
        }
    });
}


editarRestriccion = function (id, regcodi,tipo) {
    
    $.ajax({
        type: 'POST',
        url: controlador + 'RestriccionEdit',
        data: {
            segcocodi: id,
            regcodi: regcodi
        },
        success: function (evt) {
            $('#contenidoEdicion').html(evt);
            setTimeout(function () {
                $('#popupEdicion').bPopup({
                    autoClose: false
                });
            }, 50);
            if (id > 0) {
                $('#cboTipoDemanda').val($('#hfTipoDemanda').val());
                $('#cboZona').val($('#hfTipoZona').val());
            }
            else
            {
                $('#cboTipoDemanda').val(tipo);
                $('#cboZona').val(0);
            }
            
            $('#cboTipoDemanda').prop('disabled', true);

            
            $('#btnGrabarRestriccion').on("click", function () {
                $('#cboTipoDemanda').prop('disabled', false);
                grabar();
            });

            $('#btnCancelarRestriccion').on("click", function () {
                $('#popupEdicion').bPopup().close();
            });

        },
        error: function () {
            mostrarMensaje('mensaje', 'error', 'Se ha producido un error.');
        }
    });
};

eliminarRestriccion = function (id, estado) {

    if (confirm('¿Está seguro de realizar esta operación?')) {

        $.ajax({
            type: 'POST',
            url: controlador + 'RestriccionDelete',
            data: {
                segcocodi: id,
                estado : estado
            },
            dataType: 'json',
            success: function (result) {
                if (result == 1) {
                    alert("El registro se eliminó correctamente");
                    var estadol = $("#cboEstadoCoord").val(); 
                    consultar(estadol);
                }
                else {
                    mostrarMensaje('mensaje', 'error', 'Se ha producido un error.');
                }
            },
            error: function () {
                mostrarMensaje('mensaje', 'error', 'Se ha producido un error.');
            }
        });
    }
}

grabar = function () {
    var validacion = validar();
    alert(validacion);
    if (validacion == "") {
        $.ajax({
            type: 'POST',
            url: controlador + 'RestriccionSave',
            data: $('#frmRegistro').serialize(),
            dataType: 'json',
            global: false,
            success: function (result) {
                if (result != -1) {
                    alert("La Restricción se grabó correctamente");
                    $('#popupEdicion').bPopup().close();
                    var estadol = $("#cboEstadoCoord").val(); 
                    consultar(estadol);
                }
                else {
                    mostrarMensaje('mensajeEdicion', 'error', 'Se ha producido un error.');
                }
            },
            error: function () {
                mostrarMensaje('mensajeEdicion', 'error', 'Se ha producido un error.');
            }
        });
    }
    else {
        mostrarMensaje('mensajeEdicion', 'alert', validacion);
    }
}

validar = function () {
    
    var mensaje = "<ul>";
    var flag = true;

    if ($('#cboTipo').val() == "") {
        mensaje = mensaje + "<li>Seleccione el tipo demanda.</li>";
        flag = false;
    }

    if ($('#cboZona').val() == "") {
        mensaje = mensaje + "<li>Seleccione Zona de seguridad.</li>";
        flag = false;
    }

    if ($('#txtFlujo1').val() == "") {
        mensaje = mensaje + "<li>Ingrese Flujo Inicio</li>";
        flag = false;
    }

    if ($('#txtGeneracion1').val() == "") {
        mensaje = mensaje + "<li>Ingrese Generación Inicio</li>";
        flag = false;
    }

    if ($('#txtFlujo2').val() == "") {
        mensaje = mensaje + "<li>Ingrese Flujo Final</li>";
        flag = false;
    }

    if ($('#txtGeneracion2').val() == "") {
        mensaje = mensaje + "<li>Ingrese Generación Final</li>";
        flag = false;
    }

    mensaje = mensaje + "</ul>";

    if (flag) mensaje = "";

    return mensaje;
}

mostrarMensaje = function (id, tipo, mensaje) {
    $('#' + id).removeClass();
    $('#' + id).addClass('action-' + tipo);
    $('#' + id).html(mensaje);
}