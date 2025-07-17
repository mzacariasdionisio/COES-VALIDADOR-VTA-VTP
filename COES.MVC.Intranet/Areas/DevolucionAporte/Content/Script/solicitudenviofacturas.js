var controlador = siteRoot + 'devolucionaporte/solicitudenviofacturas/';

var aportantesTodos = [];
var datax = [];
$(document).ready(function () {
    pintarBusqueda(1);

    $("#cboAnioInversion").change(function () {
        $('#mensaje').hide();
        pintarBusqueda(1);
    });

});

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

GenerarCarta = function () {
    var aportantes = obtenerAportantes();
    
    $("#mensaje").hide();

    if (aportantes.length == 0) {
        mostrarError("mensaje", "Debe seleccionar al menos un aportante");
        return;
    }
    
    $.ajax({
        type: 'POST',
        url: controlador + 'parametros',
        data: {
            anio: $("#cboAnioInversion").val()
        },
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
        data: { anio: $("#cboAnioInversion").val() },
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
            mostrarError("mensaje");
        }
    });
}

function procesar() {
    
    if ($("#txtCarta").val() == "") {
        mostrarError("modalmensaje", "Debe ingresar N° de carta.")
        return;
    }
    
    mostrarConfirmacion("", generarcarta, "");
}

function generarcarta() {
    var aportantes = obtenerAportantes();

    $.ajax({
        type: 'POST',
        url: controlador + "GenerarCarta",
        data: { nrocarta: $("#txtCarta").val(), anio: $("#txtAnioPago").val(), aports: aportantes.join(",") },
        success: function (evt) {
            $('#popupConfirmarOperacion').bPopup().close();

            if (evt > "0") {
                $('#popupParametros').bPopup().close();
                mostrarMensajePopup("Se realizo con exito la operación", "");
                pintarBusqueda(1);
                $("#chkTodos").prop("checked", false);

                window.setTimeout(function () {
                    descargarZip($("#txtAnioPago").val());
                }, 100)
            }
        },
        error: function () {
            mostrarError();
        }
    });
}

function descargarZip(anio) {
    window.location = controlador + "Descargarzip?anio=" + anio;
}

mostrarExito = function () {
    $('#modalmensaje').removeClass();
    $('#modalmensaje').show();
    $('#modalmensaje').addClass('action-exito');
    $('#modalmensaje').html("La operación se realizó correctamente");
}

mostrarError = function (div, mensaje) {
    if (mensaje == null) mensaje = "";
    if (mensaje.length == 0) mensaje = "Ha ocurrido un error";
    $("#" + div).show();
    $("#" + div).removeClass();
    $("#" + div).addClass('action-error');
    $("#" + div).html(mensaje);
}
