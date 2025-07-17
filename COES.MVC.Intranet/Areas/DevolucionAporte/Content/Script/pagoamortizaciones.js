var controlador = siteRoot + 'devolucionaporte/pagoamortizaciones/';

var ingresarCheques = 0;
var aportantesCheques = [];
$(document).ready(function () {
    pintarBusqueda(1);

    $("#cboAnioInversion").change(function () {
        $('#mensaje').hide();
        pintarBusqueda(1);
    });

    $("#fuImportar").change(function () {
        aportantesCheques = [];
        ImportarNroCheque();
    });
});

eliminarArchivo = function () {
    $("#txtNrocheque").val("");
    $("#fuImportar").val("");
    ingresarCheques = 0;
}

GenerarCarta = function () {
    $('#mensaje').hide();

    var aportantes = obtenerAportantes();
    
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

                //ListarAnios();
            }, 50);

        },
        error: function () {
            mostrarError('mensaje', 'error', 'Se ha producido un error');
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
                'searching': true,
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

//function ListarAnios() {
//    //var aportantes = obtenerAportantes();
//    var aportantes = obtenerAportantes();
    
//    $.ajax({
//        url: controlador + "ListarAnios",
//        contentType: 'application/json',
//        type: "POST",
//        data: { prescodi: $("#cboAnioInversion").val(), apors: 1 },
//        success: function (evt) {
//            console.log(evt);
//        },
//        error: function () {
//            mostrarError();
//        }
//    });
//}

function obtenerAportantes() {
    var table = $('#tabla').DataTable();

    var aportantes = [];
    table.$('input[type="checkbox"]').each(function () {
        var aporcodi = $(this).val();
        if ($(this).is(":checked")) {
            aportantes.push({
                Aporcodi: aporcodi,
                NroCarta: "",
                Anio: 0,
                Pago: 0
            })
        }
    });

    return aportantes;
}

function procesar() {
    if ($("#txtAnioPago").val() == "") {
        mostrarError("modalmensaje", "Debe ingresar año de pago.")
        return;
    }

    if ($("#txtCarta").val() == "") {
        mostrarError("modalmensaje", "Debe ingresar N° de carta.")
        return;
    }
    
    if (ingresarCheques == 0) {
        mostrarError("modalmensaje", "Debe cargar N° cheques.")
        return;
    }
    
    mostrarConfirmacion("", carta, "");
}

function carta() {
    var nrocarta = $("#txtCarta").val();
    
    //var aportantes = obtenerAportantes();
    aportantesCheques.forEach(function (item, index) {
        item.NroCarta = $("#txtCarta").val();
        item.Anio = $("#txtAnioPago").val();
        item.Pago = $("#chkConfirmarpago").is(":checked") ? 1 : 0
    });
    
    $.ajax({
        url: controlador + "GenerarCarta",
        contentType: 'application/json',
        type: "POST",
        data: JSON.stringify(aportantesCheques),
        success: function (evt) {
            $('#popupConfirmarOperacion').bPopup().close();

            if (evt == "1") {
                $('#popupParametros').bPopup().close();
                mostrarMensajePopup("Se realizo con exito la operación", "");
                pintarBusqueda(1);
                $("#chkTodos").prop("checked", false);

                window.setTimeout(function () {
                    descargarZip($("#txtAnioPago").val());
                }, 100);
            } else {
                console.log(evt);
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

function descargarAportantes() {
    var aportantes = obtenerAportantes();
    aportantes = aportantes.map(a => a.Aporcodi);

    window.location = controlador + "DescargarAportrantes?aports=" + aportantes.join(",")
}

function ImportarNroCheque() {
    $('#modalmensaje').hide();
    
    var fileUpload = $("#fuImportar").get(0);
    var files = fileUpload.files;

    // Create FormData object  
    var fileData = new FormData();

    // Looping over all files and add it to FormData object  
    for (var i = 0; i < files.length; i++) {
        fileData.append(files[i].name, files[i]);
        $("#txtNrocheque").val(files[i].name);
    }
    
    $.ajax({
        url: controlador + 'ImportarNroCheque',
        type: "POST",
        contentType: false, // Not to set any content header  
        processData: false, // Not to process data  
        data: fileData,
        success: function (data) {

            if (data != "0") {
                $.each(data, function (key, item) {
                    if (item.Porcentaje != "") {
                        ingresarCheques = 1;

                        aportantesCheques.push({
                            Emprcodi: item.Emprcodi,
                            NroChequeEgreso: item.NroChequeEgreso,
                            NroReciboEgreso: item.NroReciboEgreso,
                            NroChequeInteres: item.NroChequeInteres,
                            NroCarta: "",
                            Anio: 0,
                            Pago: 0
                        });
                    }
                });

                $("#fuImportar").val("");
            }
        },
        error: function (err) {
            //alert(err.statusText);
        }
    });
}

mostrarExito = function (div) {
    $('#' + div).show();
    $('#' + div).removeClass();
    $('#' + div).addClass('action-exito');
    $('#' + div).html("La operación se realizó correctamente");
}

mostrarError = function (div, mensaje) {
    if (mensaje == null) mensaje = "";
    if (mensaje.length == 0) mensaje = "Ha ocurrido un error";

    $('#' + div).show();
    $('#' + div).removeClass();
    $('#' + div).addClass('action-error');
    $('#' + div).html(mensaje);
}