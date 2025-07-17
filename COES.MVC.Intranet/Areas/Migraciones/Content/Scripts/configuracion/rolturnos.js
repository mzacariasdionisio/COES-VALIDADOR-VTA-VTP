var controlador = siteRoot + 'Migraciones/Configuraciones/';
var ALTURA_HANDSON = 900;
//opciones
var OPCION_CONSULTAR = 1;
var OPCION_ENVIAR_DATOS = 2;
var OPCION_ENVIO_ANTERIOR = 3;
var OPCION_IMPORTAR_DATOS = 5;

$(function () {
    $("#cbArea").val("3");

    $("#cbArea").change(function () {
        limpiar();
    });
    $("#cbTipo").change(function () {
        limpiar();
    });
    $('#txtMesAnio').Zebra_DatePicker({
        format: 'm-Y',
        onSelect: function () {
            limpiar();
        }
    });

    $("#btnSave").click(function () {
        saveRolturnos();
    });
    $("#btnExportar").click(function () {
        exportarRolturnos();
    });
    $('#btnMostrarErrores').click(function () {
        mostrarDetalleErrores();
    });
    $("#btnConsultar").click(function () {
        consultar();
    });


    crearPupload();
});

function consultar() {
    limpiar();
    switch ($('#cbTipo').val()) {
        case "1": cargarRolturnos(); break;
        case "2": cargarMovimientos(); break;
    }
}

function crearPupload() {
    var msjOpc = "", msjCarga = "";
    var uploader = new plupload.Uploader({
        runtimes: 'html5,flash,silverlight,html4',
        browse_button: "btnImportar",
        url: controlador + "Upload",
        flash_swf_url: 'Scripts/Moxie.swf',
        silverlight_xap_url: 'Scripts/Moxie.xap',
        multi_selection: false,
        filters: {
            max_file_size: 0,
            mime_types: [
                { title: "Archivos xls", extensions: "xls,xlsx" },
            ]
        },
        init: {
            FilesAdded: function (up, files) {
                if (uploader.files.length == 2) {
                    uploader.removeFile(uploader.files[0]);
                }
                uploader.start();
                up.refresh();
            },
            UploadProgress: function (up, file) {
                mostrarAlerta("Por favor espere ...(<strong>" + file.percent + "%</strong>)");
            },
            UploadComplete: function (up, file) {

                mostrarExito("Subida completada. <strong>Por favor espere.</strong>");

                $.ajax({
                    type: 'POST',
                    url: controlador + "ImportarDatosRolTurnos",
                    dataType: 'json',
                    async: false,
                    data: {
                        areacodi: $('#cbArea').val(),
                        mesanio: $('#txtMesAnio').val()
                    },
                    success: function (evt) {
                        //cargarRolturnos(OPCION_IMPORTAR_DATOS);
                        mostrarImportacion(evt, OPCION_IMPORTAR_DATOS);
                    },
                    error: function () {
                        mostrarError("Ha ocurrido un error");
                    }
                });
            },
            Error: function (up, err) {
                mostrarError(err.code + "-" + err.message);
                if (err.code == -600) {
                    alert("La capacidad máxima de Zip es de 30Mb..... \nEliminar carpetas o archivos que no son parte del contenido del archivo ZIP."); return;
                }
            }
        }
    });

    uploader.init();
}

function saveRolturnos() {
    if (confirm("¿Desea enviar información a COES?")) {
        if (validarEnvio()) {
            $.ajax({
                type: 'POST',
                url: controlador + "SaveRolturnos",
                dataType: 'json',
                contentType: 'application/json',
                traditional: true,
                data: JSON.stringify({
                    areacodi: $('#cbArea').val(),
                    mesanio: $('#txtMesAnio').val(),
                    data: hotRol.getData()
                }),
                success: function (evt) {
                    if (evt.nRegistros == 1) {
                        cargarRolturnos(OPCION_ENVIAR_DATOS);
                        if (evt.Administrador == 1) { $("#btnReplicar").show(); }
                    }
                    else {
                        if (evt.nRegistros == -2) { alert("Todo responsable debe tener actividades asignadas en el mes"); $("#btnReplicar").hide(); }
                        else { alert("Ha ocurrido un error."); }
                    }
                },
                error: function (err) { alert("Error al cargar Excel Web"); }
            });
        }
    }
}

function validarEnvio() {
    generarListaErroresFromData();
    var existeErrores = existeListaErrores();

    //valida si existen errores
    if (existeErrores) {
        mostrarError("Existen errores en las celdas, favor de corregir y vuelva a envíar");
        mostrarDetalleErrores();
        return false;
    }

    return true;
}

function cargarRolturnos(opcion) {
    var accion = parseInt(opcion) || 0;

    if (typeof hotRol != 'undefined') {
        hotRol.destroy();
    }

    ALTURA_HANDSON = parseInt(650);

    $.ajax({
        type: 'POST',
        url: controlador + "CargarRolturnos",
        dataType: 'json',
        data: {
            areacodi: $('#cbArea').val(),
            mesanio: $('#txtMesAnio').val()
        },
        success: function (evt) {
            evtHot = evt;
            switch (accion) {
                case OPCION_ENVIAR_DATOS:
                    mostrarExito("Los datos se enviaron correctamente.");
                    break;
                case OPCION_IMPORTAR_DATOS:
                    mostrarExito("Los datos se cargaron correctamente. <strong>Por favor presione el botón Grabar para guardar los cambios.</strong>");
                    break;
            }

            $('#excelweb').show();
            crearExcelRolturnos(evt, ALTURA_HANDSON);

            if (evt.nRegistros > 0) {
                if (evt.Administrador == 1) { $("#btnSave").show(); $("#btnExportar").show(); $("#btnImportar").show(); $("#btnMostrarErrores").show(); }
                else { $("#btnSave").hide(); $("#btnExportar").hide(); $("#btnReplicar").hide(); $("#btnImportar").hide(); $("#btnMostrarErrores").show(); }
            }
            else { $("#btnSave").show(); $("#btnExportar").show(); $("#btnReplicar").hide(); $("#btnImportar").show(); $("#btnMostrarErrores").show(); }

            $("#roles").html(evt.Comentario);
            $("#tb_leyenda").dataTable({
                "ordering": false
            });
            $("#roles").show();
        },
        error: function (err) { alert("Error..!!"); }
    });
}

function mostrarImportacion(evt, opcion) {
    var accion = parseInt(opcion) || 0;

    if (typeof hotRol != 'undefined') {
        hotRol.destroy();
    }

    ALTURA_HANDSON = parseInt(650);

    switch (accion) {
        case OPCION_ENVIAR_DATOS:
            mostrarExito("Los datos se enviaron correctamente.");
            break;
        case OPCION_IMPORTAR_DATOS:
            mostrarExito("Los datos se cargaron correctamente. <strong>Por favor presione el botón Grabar para guardar los cambios.</strong>");
            break;
    }

    $('#excelweb').show();
    crearExcelRolturnos(evt, ALTURA_HANDSON);

    if (evt.nRegistros > 0) {
        if (evt.Administrador == 1) { $("#btnSave").show(); $("#btnExportar").show(); $("#btnImportar").show(); $("#btnMostrarErrores").show(); }
        else { $("#btnSave").hide(); $("#btnExportar").hide(); $("#btnReplicar").hide(); $("#btnImportar").hide(); $("#btnMostrarErrores").show(); }
    }
    else { $("#btnSave").show(); $("#btnExportar").show(); $("#btnReplicar").hide(); $("#btnImportar").show(); $("#btnMostrarErrores").show(); }

    $("#roles").html(evt.Comentario);
    $("#tb_leyenda").dataTable({
        "ordering": false
    });
    $("#roles").show();
}

function cargarMovimientos() {
    $("#div_movimientos").hide();
    $.ajax({
        type: 'POST',
        url: controlador + "CargarMovimientos",
        dataType: 'json',
        data: {
            areacodi: $('#cbArea').val(),
            mesanio: $('#txtMesAnio').val()
        },
        success: function (evt) {
            if (evt.nRegistros > 0) {
                $('#div_movimientos').show();
                $("#btnSave").hide(); $("#btnExportar").hide(); $("#btnReplicar").hide();
                $('#div_movimientos').html(evt.Resultado);

                $('#0').html('<input type="text" placeholder="Buscar Responsable" data-index="0" />');
                var table = $('#tb_movimientos').DataTable({
                    "sPaginationType": "full_numbers",
                    "destroy": "true",
                    "bInfo": true,
                    //"bLengthChange": false,
                    //"sDom": 'fpt',
                    "ordering": true,
                    "iDisplayLength": 15,
                    "order": [[5, "desc"]]
                });

                // Filter event handler
                $(table.table().container()).on('keyup', 'tfoot input', function () {
                    table
                        .column($(this).data('index'))
                        .search(this.value)
                        .draw();
                });
            }
        },
        error: function (err) { alert("Error..!!"); }
    });
}

function exportarRolturnos() {
    $.ajax({
        type: 'POST',
        url: controlador + 'GenerarReporteXls',
        data: {
            areacodi: $('#cbArea').val(),
            areanomb: $('#cbArea option:selected').text(),
            mesanio: $('#txtMesAnio').val()
        },
        dataType: 'json',
        success: function (result) {
            switch (result.nRegistros) {
                case 1: window.location = controlador + "Exportar?fi=" + result.Resultado; break;// si hay elementos
                case 2: alert("No existen registros !"); break;// sino hay elementos
                case -1: alert("Error en reporte result"); break;// Error en C#
            }
        },
        error: function (err) {
            alert("Error en reporte");;
        }
    });
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

function mostrarInfo(mensaje) {
    $('#mensaje').removeClass();
    $('#mensaje').html(mensaje);
    $('#mensaje').addClass('action-message');
}

function limpiar() {
    mostrarInfo('Por favor complete los datos.');
    $("#roles").hide();
    $("#excelweb").hide();
    $("#div_movimientos").hide();
    $("#btnSave").hide(); $("#btnExportar").hide(); $("#btnImportar").hide(); $("#btnMostrarErrores").hide(); $('#excelweb').hide();
}

//////////////////////////////////////////////////////////
//// btnMostrarErrores
//////////////////////////////////////////////////////////
function mostrarDetalleErrores() {
    $('#idTerrores').html('');
    $('#validaciones').bPopup({
        easing: 'easeOutBack',
        speed: 450,
        transition: 'slideDown',
        modalClose: false
    });

    setTimeout(function () {
        $('#idTerrores').html(dibujarTablaError());

        $('#tablaError').dataTable({
            "scrollY": 330,
            "scrollX": true,
            "sDom": 't',
            "ordering": false,
            "bPaginate": false,
            "iDisplayLength": -1
        });
    }, 200);
}

function dibujarTablaError() {
    var cadena = "<div style='clear:both; height:5px'></div> ";
    cadena += "<table id='tablaError' border='1' class='pretty tabla-adicional' cellspacing='0'>";
    cadena += "<thead><tr><th>Celda</th><th>Valor</th><th>Tipo Error</th></tr></thead>";
    cadena += "<tbody>";

    generarListaErroresFromData();
    var listErrores = getListaErrores();

    var errores = ERROR_GLOBAL;
    var len = listErrores.length;
    for (var i = 0 ; i < len ; i++) {
        cadena += "<tr><td style='text-align: center;'>" + listErrores[i].Celda + "</td>";
        cadena += "<td style='text-align: center;'>" + listErrores[i].Valor + "</td>";
        cadena += "<td>" + errores[listErrores[i].Tipo].Descripcion + "</td></tr>";
    }
    cadena += "</tbody></table>";

    return cadena;
}

function generarListaErroresFromData() {
    var evt = evtHot;
    var readOnly;

    var numCol = evt.Handson.ListaExcelData[0].length;
    var numFil = evt.Handson.ListaExcelData.length;
    var numFilCabecera = 2;
    var colIniData = 3;
    var listErrores = [];
    var totalError = 0;
    var errores = ERROR_GLOBAL;
    var fin = false;

    for (var col = 0; col < numCol && !fin; col++) {
        for (var row = 0; row < numFil && !fin; row++) {
            readOnly = true;

            if (row >= numFilCabecera && row <= numFil && col >= colIniData && col <= numCol) {
                if (evt.Handson.ReadOnly) {
                    readOnly = true;
                }
                else {
                    readOnly = false;
                }
            }

            if (!readOnly) {
                var value = hotRol.view.instance.getDataAtCell(row, col);
                var regError = obtenerErrorGlobal(value, row, col, errores, numFilCabecera);
                if (regError != null) {
                    totalError++;
                    listErrores.push(regError);
                }
            }

            if ((numCol <= col && numFil <= row) || totalError == 100) {
                fin = true;
            }
        }
    }
    evt.listErrores = listErrores;
}

function getListaErrores() {
    if (evtHot != null && evtHot != undefined && evtHot.listErrores != undefined && evtHot.listErrores != null) {
        return evtHot.listErrores;
    }

    return [];
}

function existeListaErrores() {
    return getListaErrores().length > 0;
}

function obtenerErrorGlobal(value, row, col, errores, numFilCabecera) {
    var evt = evtHot;
    var errorReg = null;
    var cont = 0;
    if (value != null) {
        celda = getExcelColumnName(parseInt(col) + 1) + (parseInt(row) + 1).toString();

        if (value.trim() != "") {
            //Es actividad válida
            var activid = evt.ListaString;
            value = value.toUpperCase();
            if (!activid[col - 1].includes("&" + value + "&")) {
                return obtenerError(celda, value, ERROR_NO_ACTIVIDAD);
            }

            //Actividad se encuentra repetida
            if (esValidable(value)) {
                var result = "";
                var listaTmp = [];
                for (var x = 2; x < grillaBD.length; x++) {
                    result += grillaBD[x][col] + ",";
                    listaTmp.push(grillaBD[x][col]);
                }

                var firstVal = getIndicesOf(value, result, listaTmp);
                var secondVal = esActividadNoRepetible(value);
                var thirdVal = esActividadNoRepetible2Veces(value, listaTmp);

                if (firstVal && secondVal) {
                    return obtenerError(celda, value, ERROR_ACTIVIDAD_REPETIDA);
                }
                if (firstVal && thirdVal) {
                    return obtenerError(celda, value, ERROR_ACTIVIDAD_REPETIDA);
                }
            }
        }
        else {
        }
    }

    return errorReg;
}

function obtenerError(celda, valor, tipo) {
    var regError = null;
    regError = {
        Celda: celda,
        Valor: valor,
        Tipo: tipo
    };
    return regError;
}