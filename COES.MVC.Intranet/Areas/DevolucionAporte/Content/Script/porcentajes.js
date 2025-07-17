var controlador = siteRoot + 'devolucionaporte/porcentajes/';
var archivo = 0;

$(document).ready(function () {
    pintarBusqueda(1);

    $("#cboAnioInversion").change(function () {
        $("#btnImportar").unbind("click");

        pintarBusqueda(1);
        $('#mensaje').hide();
        ObtenerPresupuesto($(this).val());
    });

    $("#btnImportar").click(function () {
        if ($("#cboAnioInversion").val() == 0) {
            mostrarError("Debe seleccionar Año Inversión");
            return;
        }
    });

    $("#fuImportar").change(function () {
        $('#mensaje').hide();
        ImportarAportantes();
    });
});

function subirEmpresas() {
    $('#mensaje').hide();
    $("#fuImportar").click();
}

function pintarBusqueda(nroPagina) {
    //$('#hfNroPagina').val(nroPagina);

    $.ajax({
        type: 'POST',
        url: controlador + "ListadoPorcentajes",
        data: { prescodi: $("#cboAnioInversion").val() },
        success: function (evt) {
            $('#listado').css("width", $('#mainLayout').width() + "px");

            $('#listado').html(evt);
            $('#tabla').dataTable({
                "scrollY": 430,
                "scrollX": false,
                'searching': false,
                "ordering": false,
                "pagingType": "full_numbers",
                "iDisplayLength": 50,
                "createdRow": function (row, data, index) {
                    //
                    // if the second column cell is blank apply special formatting
                    //
                    if (data[6] == 0) {
                        $(row).addClass('label-warning');
                    }

                    $('td', row).eq(4).html(Number(data[4]).toFixed(6));

                    $('td', row).eq(0).addClass('text-left');
                    $('td', row).eq(0).width('23%');

                    $('td', row).eq(1).width('10%');
                    $('td', row).eq(2).width('10%');

                    $('td', row).eq(3).addClass('text-left');
                    $('td', row).eq(3).width('23%');

                    $('td', row).eq(4).addClass('text-right');
                    $('td', row).eq(4).width('10%');

                    $('td', row).eq(5).addClass('text-right');
                    $('td', row).eq(5).width('10%');
                },
                "columnDefs": [
                    {
                        "targets": [6],
                        "visible": false,
                    }
                ]
            });

            //if (dataFiltros.Audiactivo > -1 || dataFiltros.Aniovigencia > 0 || dataFiltros.Audinombre.length > 0) {
            //    var filas = $('#listado tbody tr.fila').length;
            //    if (filas == 0) {
            //        mostrarMensajePopup("No se encontro data, con los parametros ingresados.", 3);
            //    }
            //}
        },
        error: function () {
            //mostrarError();
        }
    });
}

var aportantes = [];
function ImportarAportantes() {
    $('#mensaje').hide();
    aportantes = [];

    $("#lblmensaje").html("");
    $("#lblmensajeregistros").html("");

    if ($("#cboAnioInversion").val() == 0) {
        mostrarError("Debe seleccionar Año Inversión");
        return;
    }

    var fileUpload = $("#fuImportar").get(0);
    var files = fileUpload.files;

    // Create FormData object  
    var fileData = new FormData();

    // Looping over all files and add it to FormData object  
    for (var i = 0; i < files.length; i++) {
        fileData.append(files[i].name, files[i]);
    }

    fileData.append('prescodi', $("#cboAnioInversion").val());

    var table = $('#tabla').DataTable();
    table.clear().draw();

    $.ajax({
        url: controlador + 'ImportarAportantes',
        type: "POST",
        contentType: false, // Not to set any content header  
        processData: false, // Not to process data  
        data: fileData,
        success: function (data) {

            if (data != "0") {
                $.each(data, function (key, item) {
                    if (item.Porcentaje != "") {
                        aportantes.push({
                            Emprcodi: item.Emprcodi,
                            Porcentaje: item.Porcentaje,
                            Prescodi: Number($("#cboAnioInversion").val()),
                            EstadoImportado: item.Estado
                        });

                        table.row.add([
                            item.Emprnomb,
                            item.Tipoemprdesc,
                            item.Emprruc,
                            item.Emprrazsocial,
                            item.Porcentaje,
                            item.Apormontoparticipacion,
                            item.Estado
                        ]).draw();
                    }
                });

                var sum = aportantes.map(a => a.Porcentaje).reduce(function (a, b) { return Number(a) + Number(b); }, 0);

                var aport = aportantes.filter(obj => {
                    return obj.EstadoImportado == 0
                })

                if (Math.round(sum) < 100) {
                    $("#lblmensaje").html("El total Porcentaje(%) no suma 100%");
                }

                if (aport.length > 0) {
                    $("#lblmensajeregistros").html("Hay registros con errores");
                }

                $("#fuImportar").val("");
            }
        },
        error: function (err) {
            //alert(err.statusText);
        }
    });
}

function countDecimals(value) {
    if (Math.floor(value) !== value) {
        return value.toString().split(".")[1].length || 0;
    }
        
    return 0;
}

function guardar() {
    if ($("#cboAnioInversion").val() == 0) {
        mostrarError("Debe seleccionar Año Inversión");
        return;
    }

    if (aportantes.length == 0) {
        mostrarError("No tiene data para importar");
        return;
    }

    //var sum = aportantes.map(a => countDecimals(a.Porcentaje) > 2 ? a.Porcentaje * 100 : a.Porcentaje).reduce(function (a, b) { return Number(a) + Number(b); }, 0);
    var sum = aportantes.map(a => a.Porcentaje).reduce(function (a, b) { return Number(a) + Number(b); }, 0);
    var aport = aportantes.filter(obj => {
        return obj.EstadoImportado == 0
    })

    var mensaje = "";

    if (aport.length > 0) {
        $("#lblmensajeregistros").html("Hay registros con errores");
    }
    
    sum = Math.round(sum)
    
    if (sum < 100) {
        mostrarError("El total Porcentaje(%) no suma 100%");
    } else if (sum > 100) {
        mostrarError("El total Porcentaje(%) no debe exceder el 100%");
    } else if (sum == 100) {
        mostrarConfirmacion("", guardarAportantes, "");
    }
}

function guardarAportantes() {
    var x = 0;

    aportantes = aportantes.filter(obj => {
        return obj.EstadoImportado == 1
    })

    var aaportantes = [];

    for (var i = 0; i < aportantes.length; i++) {
        var aportante = aportantes[i];

        aaportantes.push({
            Emprcodi: aportante.Emprcodi,
            Porcentaje: aportante.Porcentaje,
            Prescodi: Number($("#cboAnioInversion").val())
        });
    }

    $.ajax({
        url: controlador + 'SubirAportantes',
        contentType: 'application/json',
        type: "POST",
        data: JSON.stringify(aaportantes),
        success: function (respuesta) {
            if (respuesta.resultado == 1) {
                $("#mensaje").hide();
                $('#popupConfirmarOperacion').bPopup().close();
                mostrarMensajePopup("Se realizo con exito la operación", "");
                pintarBusqueda(1);
                aportantes = [];
            }
        },
        error: function (err) {
            //alert(err.statusText);
        }
    });

    //for (var i = 0; i < aportantes.length; i++) {
    //    $.ajax({
    //        url: controlador + 'SubirAportantes',
    //        type: "POST",
    //        data: { emprcodi: aportantes[i].emprcodi, porcentaje: aportantes[i].porcentaje, prescodi: $("#cboAnioInversion").val() },
    //        success: function (respuesta) {
    //            if (respuesta.resultado == 1)
    //            {
    //                x++;

    //                if (aportantes.length == x) {
    //                    $("#mensaje").hide();
    //                    $('#popupConfirmarOperacion').bPopup().close();
    //                    mostrarMensajePopup("Se realizo con exito la operación", "");
    //                    pintarBusqueda(1);
    //                    aportantes = [];
    //                }
    //            }
    //        },
    //        error: function (err) {
    //            //alert(err.statusText);
    //        }
    //    });
    //}
}

function ObtenerPresupuesto(prescodi) {
    if (prescodi == "") {
        $("#btnImportar").click(function () {
            if ($("#cboAnioInversion").val() == 0) {
                mostrarError("Debe seleccionar Año Inversión");
                return;
            }
            subirEmpresas();
        });

        return;
    }
    
    $.ajax({
        url: controlador + 'ObtenerPresupuesto',
        type: "POST",
        data: { prescodi: prescodi },
        success: function (respuesta) {
            

            if (respuesta.presprocesada == "1") {
                $("#btnImportar").click(function () {
                    mostrarError("El año de inversion seleccionada ya se encuentra procesada.");
                });
                $("#btnConsultar").removeAttr("onclick");
            } else {

                $("#btnImportar").click(function () {
                    subirEmpresas();
                });
            }
        },
        error: function (err) {
            //alert(err.statusText);
        }
    });
}

function descargarEmpresas() {
    window.location = controlador + "DescargarEmpresas?prescodi=" + $("#cboAnioInversion").val();
}

mostrarExito = function () {
    $('#mensaje').show();
    $('#mensaje').removeClass();
    $('#mensaje').addClass('action-exito');
    $('#mensaje').html("La operación se realizó correctamente");
}

mostrarError = function (mensaje) {
    $('#mensaje').show();
    $('#mensaje').removeClass();
    $('#mensaje').addClass('action-error');
    $('#mensaje').html(mensaje);
}

mostrarInformacion = function (mensaje) {
    $('#mensaje').show();
    $('#mensaje').removeClass();
    $('#mensaje').addClass('action-message');
    $('#mensaje').html(mensaje);
}