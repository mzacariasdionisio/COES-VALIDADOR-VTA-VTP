var controlador = siteRoot + 'devolucionaporte/cuadrodevoluciones/';

var aportantesTodos = [];
var datax = [];
$(document).ready(function () {
    pintarBusqueda(1);

    $("#cboAnioInversion").change(function () {
        pintarBusqueda(1);
    });

});

GenerarCarta = function () {
    $.ajax({
        type: 'POST',
        url: controlador + 'parametros',
        //data: {
        //    idGrupo: id
        //},
        global: false,
        success: function (evt) {
            $('#contenidoParametros').html(evt);
            setTimeout(function () {
                $('#popupParametros').bPopup({
                    autoClose: false
                });
            }, 50);

        },
        error: function () {
            mostrarMensaje('mensajeGrupo', 'error', 'Se ha producido un error');
        }
    });
}

function pintarBusqueda(nroPagina) {
    //$('#hfNroPagina').val(nroPagina);
    
    $.ajax({
        type: 'POST',
        url: controlador + "Listado",
        success: function (evt) {
            $('#listado').css("width", $('#mainLayout').width() + "px");

            $('#listado').html(evt);

            var table = $('#tabla').DataTable({
                "scrollY": 430,
                "scrollX": false,
                'searching': false,
                "ordering": false,
                "pagingType": "full_numbers",
                "iDisplayLength": 50,
                "createdRow": function (row, data, index) {
                    $('td', row).eq(5).html('<input type="checkbox" value="' + data[0] + '" />');
                }
            });

            $("#chkTodos").click(function () {
                var checked = $(this).is(":checked");
                var rows = table.rows({ 'search': 'applied' }).nodes();

                $('input[type="checkbox"]', rows).prop('checked', checked);
            });
        },
        error: function () {
            mostrarError();
        }
    });
}

function obtenerAportantes() {
    var table = $('#tabla').DataTable();

    var aportantes = [];
    table.$('input[type="checkbox"]').each(function () {
        var aporcodi = $(this).val();
        if ($(this).is(":checked")) {
            aportantes.push(aporcodi)
        }
    });

    return aportantes;
}

function procesar() {
    if ($("#cboAnio").val() == "0") {
        mostrarError("Debe seleccionar un Año");
        return;
    }

    mostrarConfirmacion("", generarreporte, "");
}

function generarreporte() {
    var aportantes = obtenerAportantes();

    if (aportantes.length == 0) {
        mostrarError("Debe seleccionar al menos un aportante");
        return;
    }

    $('#popupConfirmarOperacion').bPopup().close();

    window.location = controlador + "GenerarReporte?anio=" + $("#cboAnio").val() + "&aports=" + aportantes.join(",")
    
}

mostrarExito = function () {
    $('#mensaje').show();
    $('#mensaje').removeClass();
    $('#mensaje').addClass('action-exito');
    $('#mensaje').html("La operación se realizó correctamente");
}

mostrarError = function (mensaje) {
    if (mensaje == null) mensaje = "";
    if (mensaje.length == 0) mensaje = "Ha ocurrido un error";

    $('#mensaje').show();
    $('#mensaje').removeClass();
    $('#mensaje').addClass('action-error');
    $('#mensaje').html(mensaje);
}