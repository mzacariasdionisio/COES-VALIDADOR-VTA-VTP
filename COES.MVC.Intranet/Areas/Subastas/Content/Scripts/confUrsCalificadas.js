var controlador = siteRoot + 'Subastas/Configuracion/';
var OPCION_VER = 1;
var OPCION_EDITAR = 2;
var OPCION_NUEVO = 3;
var OPCION_ACTUAL = 0;
var OPCION_LISTA = 0;
var TIPO_ACTA = 1;

$(document).ready(function () {

    $('#txtFechaData').Zebra_DatePicker({
        onSelect: function () {
            listarURSCalificadas();
        }
    });

    $('#btnExportar').click(function () {
        exportarReporte();
    });

    listarURSCalificadas();

    adjuntarArchivo1();
});

function listarURSCalificadas() {
    var fechaConsulta = $('#txtFechaData').val();
    $.ajax({
        type: 'POST',
        url: controlador + 'ListadoURSCalificadas',
        data: {
            fechaConsulta: fechaConsulta
        },
        dataType: 'json',
        success: function (result) {
            $('#listado_URSCalificadas').html(result.Resultado);
        },
        error: function (err) {
            alert('Ha ocurrido un error.');
        }
    });
}




function verHistoricoURS(grupocodi, ursDesc) {
    OPCION_ACTUAL = OPCION_VER;
    OPCION_LISTA = OPCION_VER;
    inicializarGrupoDat(OPCION_ACTUAL, grupocodi, ursDesc);
}

function editarURS(grupocodi, ursDesc) {
    OPCION_ACTUAL = OPCION_EDITAR;
    OPCION_LISTA = OPCION_EDITAR;
    inicializarGrupoDat(OPCION_ACTUAL, grupocodi, ursDesc);
}

function inicializarGrupoDat(opcion, grupocodi, ursDesc) {
    $("#hfGrupocodiDat").val(grupocodi);
    //$("#hfConcepcodiDat").val(concepcodi);
    $("#idURS").html(ursDesc);
    $('#listadoGrupoDat').html('');

    configurarFormularioGrupodatNuevo(grupocodi);

    switch (opcion) {
        case OPCION_VER:
            $("#popupHistoricoConcepto .content-botonera").hide();
            $("#popupHistoricoConcepto .titulo_listado").hide();
            break;
        case OPCION_EDITAR:
            $("#popupHistoricoConcepto .content-botonera").show();
            break;
        case OPCION_NUEVO:
            $("#popupHistoricoConcepto .content-botonera").hide()
            break;
    }

    $("#btnGrupodatNuevo").unbind();
    $('#btnGrupodatNuevo').click(function () {
        $("#btnGrupodatNuevo").hide();
        configurarFormularioGrupodatNuevo(grupocodi);
    });

    $("#btnGrupodatGuardar").unbind();
    $('#btnGrupodatGuardar').click(function () {
        guardarGrupodat();
    });

    listarHistoricos(grupocodi);

    $('#fechaURSIni').Zebra_DatePicker({
    });
    $('#fechaURSFin').Zebra_DatePicker({
    });
}

function configurarFormularioGrupodatNuevo(grupocodi) {
    OPCION_ACTUAL = OPCION_NUEVO;
    $("#formularioGrupodat").show();
    $("#fechaURSIni").val($("#hfFechaAct").val());
    $("#fechaURSFin").val($("#hfFechaAct").val());
    $("#actaCalificada").val('');
    $("#valorBanda").val('');
    $("#hfDeleted").val(0);
    $("#btnGrupodatGuardar").val("Registrar");
    $("#formularioGrupodat .popup-title span").html("Nuevo Registro");
    $("#fechaURSIni").removeAttr('disabled');
    $("#fechaURSIni").parent().find("button").prop("class", "Zebra_DatePicker_Icon Zebra_DatePicker_Icon_Inside");
    $("#fechaURSFin").removeAttr('disabled');
    $("#fechaURSFin").parent().find("button").prop("class", "Zebra_DatePicker_Icon Zebra_DatePicker_Icon_Inside");

    var fecconsult = $('#txtFechaData').val();
    $("#filelist").html('');
    listaredicionURS(fecconsult, grupocodi, OPCION_ACTUAL);
}

function editarGrupodat(Grupocodi, FechaInicio, FechaFin, BandaURS, Acta, deleted) {
    OPCION_ACTUAL = OPCION_EDITAR;
    $("#formularioGrupodat").show();
    $("#fechaURSIni").val(FechaInicio);
    $("#fechaURSFin").val(FechaFin);
    $("#actaCalificada").val(Acta);
    $("#valorBanda").val(BandaURS);
    $("#hfDeleted").val(deleted);
    $("#btnGrupodatGuardar").val("Actualizar");
    $("#formularioGrupodat .popup-title span").html("Modificar Registro");
    $("#fechaURSIni").prop('disabled', 'disabled');
    $("#fechaURSIni").parent().find("button").prop("class", "Zebra_DatePicker_Icon Zebra_DatePicker_Icon_Disabled Zebra_DatePicker_Icon_Inside");
    $("#fechaURSFin").prop('disabled', 'disabled');
    $("#fechaURSFin").parent().find("button").prop("class", "Zebra_DatePicker_Icon Zebra_DatePicker_Icon_Disabled Zebra_DatePicker_Icon_Inside");

    var fecconsult = $('#fechaURSIni').val();
    $("#filelist").html(Acta);
    listaredicionURS(fecconsult, Grupocodi, OPCION_ACTUAL);
}


function listaredicionURS(fecha, urs, opActual) {
    $.ajax({
        type: 'POST',
        url: controlador + "listarEdicionURS",
        data: {
            fechaConsulta: fecha,
            ursCodi: urs,
            opcionActual: opActual
        },
        dataType: 'json',
        success: function (result) {
            $('#listadoFormulario').html(result.Resultado);

            eventoTabla();
        },
        error: function (err) {
            alert('Ha ocurrido un error.');
        }
    });
}

function listarHistoricos(grupocodi) {
    $("#btnGrupodatNuevo").show();
    $("#formularioGrupodat").hide();

    $.ajax({
        type: 'POST',
        url: controlador + 'ListarHistoricoURSCalificadas',
        data: {
            grupocodi: grupocodi,
            //concepcodi: concepcodi,
            opedicion: OPCION_LISTA
        },
        success: function (result) {
            $('#listadoGrupoDat').html(result);

            $('#tablaListadoGrupodat').dataTable({
                "sPaginationType": "full_numbers",
                "destroy": "true",
                "bInfo": false,
                "ordering": false,
                "order": [[2, "asc"]],
                "iDisplayLength": 15
            });

            setTimeout(function () {
                $('#popupHistoricoConcepto').bPopup({
                    easing: 'easeOutBack',
                    speed: 450,
                    transition: 'slideDown',
                    follow: [false, false], //x, y
                    position: [15, 15] //x, y
                });
            }, 250);
        },
        error: function (err) {
            alert('Ha ocurrido un error.');
        }
    });
}

function guardarGrupodat() {
    var entity = getObjetoGrupodatJson();

    if (confirm('¿Desea guardar el registro?')) {
        var msj = validarData(entity);
        var jsonResult = "GuardarURSGrupodat";

        if (msj == "") {
            var obj = JSON.stringify(entity);
            $.ajax({
                type: 'POST',
                dataType: 'json',
                traditional: true,
                url: controlador + jsonResult,
                data: {
                    strJsonData: obj
                },
                cache: false,
                success: function (result) {
                    if (result.Resultado == '-1') {
                        alert('Ha ocurrido un error:' + result.Mensaje);
                    } else {
                        alert("Se guard\u00F3 correctamente el registro");
                        listarURSCalificadas();
                        listarHistoricos(entity.Grupocodi);
                    }
                },
                error: function (err) {
                    alert('Ha ocurrido un error.');
                }
            });
        } else {
            alert(msj);
        }
    }
}

function eliminarGrupodat(grupocodi, FechadatDesc, deleted) {

    var entityDeleted = getObjetoGrupodatJson();
    entityDeleted.FechaData = FechadatDesc;
    entityDeleted.deleted = deleted;
    entityDeleted.Grupocodi = grupocodi;

    if (confirm("\u00BFDesea eliminar el registro?")) {

        var objDeleted = JSON.stringify(entityDeleted);
        $.ajax({
            type: 'POST',
            dataType: 'json',
            traditional: true,
            url: controlador + 'URSEliminar',
            data: {
                strJsonDataDeleted: objDeleted
            },
            cache: false,
            success: function (result) {
                if (result.Resultado == '-1') {
                    alert('Ha ocurrido un error:' + result.Mensaje);
                } else {
                    alert("Se elimin\u00F3 correctamente el registro");
                    listarURSCalificadas();
                    listarHistoricos(grupocodi);
                }
            },
            error: function (err) {
                alert('Ha ocurrido un error.');
            }
        });
    }
}

function exportarReporte() {

    var fechaconsultaEXport = $('#txtFechaData').val();
    $.ajax({
        type: 'POST',
        url: controlador + "ExportarReporteURSCalificadas",
        data: {
            fechaConsulta: fechaconsultaEXport
        },
        dataType: 'json',
        cache: false,
        success: function (model) {
            if (model.Resultado != "-1") {
                location.href = controlador + "DescargarReporte";
            } else {
                alert("Ha ocurrido un error: " + model.Mensaje);
            }
        },
        error: function (err) {
            alert("Ha ocurrido un error.");
        }
    });
}

function getObjetoGrupodatJson() {
    var obj = {};

    obj.TipoAccion = OPCION_ACTUAL;
    obj.FechaData = $("#fechaURSIni").val();

    obj.FechaInicio = $("#fechaURSIni").val();
    obj.FechaFin = $("#fechaURSFin").val();
    obj.BandaURS = parseFloat($("#valorBanda").val()) || 0;
    obj.Acta = $("#actaCalificada").val();
    obj.Grupocodi = parseInt($("#hfGrupocodiDat").val()) || 0; //grupocodi de la URS
    obj.Concepcodi = parseInt($("#hfConcepcodiDat").val()) || 0;
    obj.Deleted = parseInt($("#hfDeleted").val()) || 0;

    obj.ModosOpList = listarDetalleModosOp();
    return obj;
}


function listarDetalleModosOp() {
    var listaDet = [];
    var idFila = 0;
    $('#tabla_agrupacion2 tbody').find('tr').each(function () {

        var id_grupocodiModo = "#" + 'grupocodiModo' + idFila.toString();
        var id_catecodiModo = "#" + 'catecodiModo' + idFila.toString();
        var id_Pmin = "#" + 'valorPmin' + idFila.toString();
        var id_Pmax = "#" + 'valorPmax' + idFila.toString();
        var id_Banda = "#" + 'valorBanda' + idFila.toString();
        var id_Cometario = "#" + 'valorComent' + idFila.toString();

        fechaDat = $("#fechaURSIni").val();
        var objDet =
        {
            FechaData: fechaDat,
            PMinDesc: parseFloat($(id_Pmin).val()) || 0.0,
            PMaxDesc: parseFloat($(id_Pmax).val()) || 0.0,
            BandaCalifDesc: parseFloat($(id_Banda).val()) || 0.0,
            Comentario: $(id_Cometario).val(),
            Grupocodi: $(id_grupocodiModo).val(),
            Catecodi: $(id_catecodiModo).val(),
            Deleted: parseInt($("#hfDeleted").val()) || 0
        };
        listaDet.push(objDet);
        idFila = idFila + 1;
    });

    return listaDet;
}

function validarData(obj) {
    var msj = "";

    if (obj.FechaInicio == null || obj.FechaInicio == '') {
        msj += "Debe seleccionar una fecha Inicio" + "\n";
    }
    if (obj.FechaFin == null || obj.FechaFin == '') {
        msj += "Debe seleccionar una fecha Fin" + "\n";
    }
    if (obj.BandaURS == null || obj.BandaURS == '') {
        msj += "Debe ingresar un valor para Banda de la URS" + "\n";
    }

    if (obj.ModosOpList == null || obj.ModosOpList.length == 0) {
        msj += "Debe registrar datos correctos para cada modo de operación." + "\n";
    } else {
        for (var i = 0; i < obj.ModosOpList.length; i++) {
            var objFila = obj.ModosOpList[i];

            var PMinDesc = objFila.PMinDesc + '';
            if (PMinDesc == '') {
                msj += "Debe registrar 'Pminrsf' para el modo de operación." + "\n";
            }

            var PMaxDesc = objFila.PMaxDesc + '';
            if (PMaxDesc == '') {
                msj += "Debe registrar 'Pmxrsf' para el modo de operación." + "\n";
            }

            if (objFila.BandaCalifDesc <= 0) {
                msj += "Debe registrar 'Banda' mayor o igual a cero para el modo de operación." + "\n";
            }
        }
    }


    return msj;
}

function eventoTabla() {

    $('input[name=valorPmin]').unbind();
    $('input[name=valorPmin]').change(function () {
        actualizarFilaTabla(this.id + '');
    });
    $('input[name=valorPmax]').on('focusout', function (e) {
        actualizarFilaTabla(this.id + '');
    });

    $('input[name=valorPmax]').unbind();
    $('input[name=valorPmax]').change(function () {
        actualizarFilaTabla(this.id + '');
    });
    $('input[name=valorPmax]').on('focusout', function (e) {
        actualizarFilaTabla(this.id + '');
    });
}

function actualizarFilaTabla(idinput) {
    var posFila = idinput.slice(idinput.length - 1);

    var valorMin = $("#valorPmin" + posFila).val();
    var valorMax = $("#valorPmax" + posFila).val();

    valorMin = parseFloat(valorMin) || 0.0;
    valorMax = parseFloat(valorMax) || 0.0;
    var banda = valorMax - valorMin;

    if (banda >= 0)
        $("#valorBanda" + posFila).val(banda);
    else
        $("#valorBanda" + posFila).val('');
}



function adjuntarArchivo1() {
    var nombreModulo = document.getElementById('NombreModulo').value;
    var tamanioMaxActa = document.getElementById('tamanioMaxActa').value;

    var fullDate = new Date();
    var twoDigitMonth = ((fullDate.getMonth().length) == 1) ? '0' + (fullDate.getMonth() + 1) : (fullDate.getMonth() + 1);
    var sFecha = fullDate.getFullYear().toString() + twoDigitMonth.toString() + fullDate.getDate().toString() + fullDate.getHours().toString() + fullDate.getMinutes().toString() + fullDate.getSeconds().toString();
    var intArchivo = 1;

    uploaderN = new plupload.Uploader({
        runtimes: 'html5,flash,silverlight,html4',
        browse_button: "btnSelectFile1",
        url: controlador + 'UploadActa?sFecha=' + sFecha + '&sModulo=' + nombreModulo + '&tipo=' + TIPO_ACTA,
        multi_selection: false,
        filters: {
            max_file_size: tamanioMaxActa,
            mime_types: [
                { title: "Archivos pdf .pdf", extensions: "pdf" },
                { title: "Archivos Word .doc", extensions: "doc" },
                { title: "Archivos Word .docx", extensions: "docx" },
                { title: "Archivos Zip .zip", extensions: "zip" },
                { title: "Archivos Rar .rar", extensions: "rar" },
            ]
        },
        init: {
            FilesAdded: function (up, files) {
                document.getElementById('filelist').innerHTML = '';
                $('#container').css('display', 'block');

                if (uploaderN.files.length === 2) {
                    uploaderN.removeFile(uploaderN.files[0]);
                }
                for (i = 0; i < uploaderN.files.length; i++) {
                    var file = uploaderN.files[i];
                }

                uploaderN.start();
            },
            UploadProgress: function (up, file) {
            },
            UploadComplete: function (up, file) {
                $('#container').css('display', 'block');
                mostrarListaArchivos();
            },
            Error: function (up, err) {
                if (err.code === -600) {
                    alert("La capacidad máxima del archivo es de " + tamanioMaxActa + ". \nSeleccionar archivo con el tamaño adecuado."); return;
                }
            }
        }
    });
    uploaderN.init();
}

function mostrarListaArchivos() {
    var autoId = 0;
    var nombreModulo = document.getElementById('NombreModulo').value;

    $.ajax({
        type: 'POST',
        url: controlador + 'ListaArchivosNuevo',
        data: { sModulo: nombreModulo, tipo: TIPO_ACTA },
        success: function (result) {
            var listaArchivos = result.ListaDocumentosFiltrado;

            $.each(listaArchivos, function (index, value) {
                autoId++;

                document.getElementById('filelist').innerHTML += '<div class="file-name" id="' + autoId + '">' + value.FileName + ' (' + value.FileSize + ') <a class="remove-item" href="JavaScript:eliminarFile(\'' + autoId + "@" + value.FileName + '\');">X</a> <b></b></div>';

            })

        },
        error: function () {
        }
    });
}

function eliminarFile(id) {
    var string = id.split("@");
    var idInter = string[0];
    var nombreArchivo = string[1];
    var orden = string[2];

    uploaderN.removeFile(idInter);

    $.ajax({
        type: 'POST',
        url: controlador + 'EliminarArchivosNuevo',
        data: { nombreArchivo: nombreArchivo, nroOrden: orden },
        success: function (result) {
            if (result == -1) {
                $('#' + idInter).remove();
            } else {
                alert("Algo salió mal");
            }
        },
        error: function () {
        }
    });
}

function descargarActa(nombreArchivo, grupocodi, tipo) {
    window.location = controlador + 'DescargarActa?nameArchivo=' + nombreArchivo + '&grupocodi=' + grupocodi + '&tipo=' + tipo;
}