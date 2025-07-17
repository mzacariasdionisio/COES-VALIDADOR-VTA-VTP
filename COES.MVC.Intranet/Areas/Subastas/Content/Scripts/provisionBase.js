var controlador = siteRoot + 'Subastas/Configuracion/'
var OPCION_VER = 1;
var OPCION_EDITAR = 2;
var OPCION_NUEVO = 3;
var OPCION_ACTUAL = 0;
var OPCION_LISTA = 0;
var TIPO_ACTA = 2;

$(document).ready(function () {

    $('#txtFechaData').Zebra_DatePicker({
        onSelect: function () {
            listadoReporte();
        }
    });

    $('#btnExportar').click(function () {
        exportarReporte();
    });

    listadoReporte();

    adjuntarArchivo1();
});

function listadoReporte() {
    var fechaConsulta = $('#txtFechaData').val();
    $.ajax({
        type: 'POST',
        url: controlador + 'ListadoProvisionBase',
        data: {
            fechaConsulta: fechaConsulta,
            estadoFiltro: getValorCheckProv(),
        },
        dataType: 'json',
        success: function (result) {
            $('#listado_ProvisionBase').html(result.Resultado);
        },
        error: function (err) {
            alert('Ha ocurrido un error.');
        }
    });
}

function getValorCheckProv() {
    var check = document.getElementById("chkTieneProv").checked;
    return check == 1 ? "2" : "-1";
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
            $(".filaUrsCabecera").hide();
            break;
        case OPCION_EDITAR:
            $("#popupHistoricoConcepto .content-botonera").show();
            $(".filaUrsCabecera").hide();
            break;
        case OPCION_NUEVO:
            $("#popupHistoricoConcepto .content-botonera").hide();
            $(".filaUrsCabecera").hide();
            break;
    }

    $("#btnGrupodatNuevo").unbind();
    $('#btnGrupodatNuevo').click(function () {
        $("#btnGrupodatNuevo").hide();
        $(".filaUrsCabecera").show();
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

    $("#agregar_fila").unbind();
    $("#agregar_fila").click(function () {
        CONTADOR_FILA += 1;

        var grupoSeleccionado = ($("#cbModoxUrs").val()).split("|");
        var grupocodi = grupoSeleccionado[0];
        var catecodi = grupoSeleccionado[1];
        var nomEmp = grupoSeleccionado[2];
        var nomCentral = grupoSeleccionado[3];
        var nomGrupo = grupoSeleccionado[4];

        var htmlXUnidad = `<tr id="tr_pos_${CONTADOR_FILA}">
                                <td><input type="button" id="quitar_fila${CONTADOR_FILA}" onclick="quitarFilaTabla(${CONTADOR_FILA})" value="-"></td>
                                <td>${nomEmp}</td>
                                <td>${nomCentral}</td>
                                <td>${nomGrupo}</td>
                                <td style="background-color: yellowgreen;">
                                            <input type="hidden" name="posicionFila" value="${CONTADOR_FILA}">
                                            <input id="grupocodiModo${CONTADOR_FILA}" type="hidden" name="grupocodiModo" value="${grupocodi}">
                                            <input id="catecodiModo${CONTADOR_FILA}" type="hidden" name="catecodiModo" value="${catecodi}">
                                            <input id="flagValidateFecha${CONTADOR_FILA}" type="hidden" name="flagValidateFecha" value="${0}">
                                            <input id="valorPotBajar${CONTADOR_FILA}" type="text" name="valorPotBajar" value="" style="width: 40px; text-indent: 3px;"></td>
                                <td style="background-color: yellowgreen;"><input id="valorPotSubir${CONTADOR_FILA}" type="text" name="valorPotSubir" value="" style="width: 40px; text-indent: 3px;"></td>
                                <td><input id="valorBanda${CONTADOR_FILA}" type="text" name="valorBanda" value="" style="width: 40px; text-indent: 3px;"></td>
                                <td><input id="valorPrecioBajar${CONTADOR_FILA}" type="text" name="valorPrecioBajar" value="" style="width: 40px; text-indent: 3px;"></td>
                                <td><input id="valorPrecioSubir${CONTADOR_FILA}" type="text" name="valorPrecioSubir" value="" style="width: 40px; text-indent: 3px;"></td>
                                <td><input id="fechaModoInicio${CONTADOR_FILA}" type="text" name="fechaModoInicio" value="" style="width: 90px;"</td>
                                <td><input id="fechaModoFin${CONTADOR_FILA}" type="text" name="fechaModoFin" value="" style="width: 90px"></td>
                                <td><input id="comentario${CONTADOR_FILA}" type="text" name="comentario" value="" style="WIDTH: 250px;"></td>
                        </tr>
                    `;

        $("#tabla_agrupacion2").find('tbody').append(htmlXUnidad);
        $('input[name=fechaModoInicio]').Zebra_DatePicker({});
        $('input[name=fechaModoFin]').Zebra_DatePicker({});
        setTimeout(function () {
            eventoTabla();
        }, 250);
    });
}

function quitarFilaTabla(pos) {
    $('#tabla_agrupacion2 #tr_pos_' + pos).remove();
}

function configurarFormularioGrupodatNuevo(grupocodi) {
    OPCION_ACTUAL = OPCION_NUEVO;
    $("#formularioGrupodat").show();
    $("#fechaURSIni").val($("#hfFechaAct").val());
    $("#fechaURSFin").val($("#hfFechaAct").val());
    $("#actaCalificada").val('');
    $("#hfDeleted").val(0);
    $("#btnGrupodatGuardar").val("Registrar");
    $("#formularioGrupodat .popup-title span").html("Nuevo Registro");
    $("#fechaURSIni").removeAttr('disabled');
    $("#fechaURSIni").parent().find("button").prop("class", "Zebra_DatePicker_Icon Zebra_DatePicker_Icon_Inside");
    $("#fechaURSFin").removeAttr('disabled');
    $("#fechaURSFin").parent().find("button").prop("class", "Zebra_DatePicker_Icon Zebra_DatePicker_Icon_Inside");
    $("#div_combo_modo").hide();

    var fecconsult = $('#txtFechaData').val();
    $("#filelist").html('');
    listaredicionURS(fecconsult, grupocodi, OPCION_ACTUAL);
}

function editarGrupodat(Grupocodi, FechaInicio, FechaFin, Acta, deleted) {
    OPCION_ACTUAL = OPCION_EDITAR;
    $("#formularioGrupodat").show();
    $("#fechaURSIni").val(FechaInicio);
    $("#fechaURSFin").val(FechaFin);
    $("#actaCalificada").val(Acta);
    $("#hfDeleted").val(deleted);
    $("#btnGrupodatGuardar").val("Actualizar");
    $("#formularioGrupodat .popup-title span").html("Modificar Registro");
    $("#fechaURSIni").prop('disabled', 'disabled');
    $("#fechaURSIni").parent().find("button").prop("class", "Zebra_DatePicker_Icon Zebra_DatePicker_Icon_Disabled Zebra_DatePicker_Icon_Inside");
    $("#fechaURSFin").prop('disabled', 'disabled');
    $("#fechaURSFin").parent().find("button").prop("class", "Zebra_DatePicker_Icon Zebra_DatePicker_Icon_Disabled Zebra_DatePicker_Icon_Inside");
    $("#div_combo_modo").hide();
    $(".filaUrsCabecera").show();

    var fecconsult = $('#fechaURSIni').val();
    $("#filelist").html(Acta);
    listaredicionURS(fecconsult, Grupocodi, OPCION_ACTUAL);
}

var CONTADOR_FILA = 1000;
function listaredicionURS(fecha, urs, opActual) {

    $.ajax({
        type: 'POST',
        url: controlador + "ListarEdicionProvisionBase",
        data: {
            fechaConsulta: fecha,
            ursCodi: urs,
            opcionActual: opActual
        },
        dataType: 'json',
        success: function (result) {
            $('#listadoFormulario').html(result.Resultado);

            if (opActual == OPCION_NUEVO || opActual == OPCION_EDITAR) {
                $("#cbModoxUrs").empty();
                for (var i = 0; i < result.ListaModo.length; i++) {
                    var reg = result.ListaModo[i];
                    var valueFila = reg.Grupocodi + "|" + reg.Catecodi + "|" + reg.Emprnomb + "|" + reg.Central + "|" + reg.Gruponomb;
                    $("#cbModoxUrs").append("<option value='" + valueFila + "'>" + reg.Emprnomb + " / " + reg.Central + " / " + reg.Gruponomb + '</option>');
                }
                $("#div_combo_modo").show();

                $('input[name=fechaModoInicio]').Zebra_DatePicker({});
                $('input[name=fechaModoFin]').Zebra_DatePicker({});

                eventoTabla();
            }

        },
        error: function (err) {
            alert('Ha ocurrido un error.');
        }
    });
}

function listarHistoricos(grupocodi) {
    $("#btnGrupodatNuevo").show();
    $("#formularioGrupodat").hide();
    $(".filaUrsCabecera").hide();

    $.ajax({
        type: 'POST',
        url: controlador + 'ListarHistoricoProvisionBase',
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
                    position: [2, 15] //x, y
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

        if (msj == "") {
            var obj = JSON.stringify(entity);
            $.ajax({
                type: 'POST',
                dataType: 'json',
                traditional: true,
                url: controlador + 'ProvisionBaseGuardarGrupodat',
                data: {
                    strJsonData: obj
                },
                cache: false,
                success: function (result) {
                    if (result.Resultado == '-1') {
                        alert('Ha ocurrido un error:' + result.Mensaje);
                    } else {
                        alert("Se guardó correctamente el registro");
                        listadoReporte();
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

    if (confirm("¿Desea eliminar el registro?")) {

        var objDeleted = JSON.stringify(entityDeleted);
        $.ajax({
            type: 'POST',
            dataType: 'json',
            traditional: true,
            url: controlador + 'ProvisionBaseEliminar',
            data: {
                strJsonDataDeleted: objDeleted
            },
            cache: false,
            success: function (result) {
                if (result.Resultado == '-1') {
                    alert('Ha ocurrido un error:' + result.Mensaje);
                } else {
                    alert("Se elimin\u00F3 correctamente el registro");
                    listadoReporte();
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
        url: controlador + "ExportarReporteURSBase",
        data: {
            fechaConsulta: fechaconsultaEXport,
            estadoFiltro: getValorCheckProv(),
        },
        dataType: 'json',
        cache: false,
        success: function (model) {
            if (model.Resultado != "-1") {
                location.href = controlador + "DescargarReporteURSBase";
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

    obj.TipoAccion = OPCION_ACTUAL,
        obj.FechaData = $("#fechaURSIni").val();

    obj.FechaInicio = $("#fechaURSIni").val();
    obj.FechaFin = $("#fechaURSFin").val();
    obj.Acta = $("#actaCalificada").val();
    obj.Grupocodi = parseInt($("#hfGrupocodiDat").val()) || 0; //grupocodi de la URS
    obj.Deleted = parseInt($("#hfDeleted").val()) || 0;

    obj.FechaConsulta = $("#txtFechaData").val();
    obj.ModosOpList = listarDetalleModosOp();
    return obj;
}


function listarDetalleModosOp() {
    var listaDet = [];

    $('#tabla_agrupacion2 tbody').find('tr').each(function () {
        var idFila = parseInt($(this).find("input[name=posicionFila]").val()) || 0;

        var id_grupocodiModo = "#" + 'grupocodiModo' + idFila.toString();
        var id_catecodiModo = "#" + 'catecodiModo' + idFila.toString();
        var id_Pmin = "#" + 'valorPotBajar' + idFila.toString();
        var id_Pmax = "#" + 'valorPotSubir' + idFila.toString();
        var id_Banda = "#" + 'valorBanda' + idFila.toString();
        var id_PrecioBajar = "#" + 'valorPrecioBajar' + idFila.toString();
        var id_PrecioSubir = "#" + 'valorPrecioSubir' + idFila.toString();
        var id_ModoInicio = "#" + 'fechaModoInicio' + idFila.toString();
        var id_ModoFin = "#" + 'fechaModoFin' + idFila.toString();
        var id_Cometario = "#" + 'comentario' + idFila.toString();
        var id_FlagValidateFecha = "#" + 'flagValidateFecha' + idFila.toString();

        fechaDat = $("#fechaURSIni").val();
        var objDet =
        {
            FechaData: fechaDat,
            PMinDesc: $(id_Pmin).val(),
            PMaxDesc: $(id_Pmax).val(),
            BandaAdjudDesc: $(id_Banda).val(),
            PrecMinDesc: $(id_PrecioBajar).val(),
            PrecMaxDesc: $(id_PrecioSubir).val(),
            ModFechIni: $(id_ModoInicio).val(),
            ModFechFin: $(id_ModoFin).val(),
            Comentario: $(id_Cometario).val(),
            Grupocodi: $(id_grupocodiModo).val(),
            Catecodi: $(id_catecodiModo).val(),
            FlagValidateFecha: $(id_FlagValidateFecha).val(),
            Deleted: parseInt($("#hfDeleted").val()) || 0
        };
        listaDet.push(objDet);

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

    if (obj.ModosOpList == null || obj.ModosOpList.length == 0) {
        msj += "Debe registrar datos correctos para cada modo de operación.";
    } else {
        for (var i = 0; i < obj.ModosOpList.length; i++) {
            var objFila = obj.ModosOpList[i];
            var PMinDesc = objFila.PMinDesc + '';
            if (PMinDesc == '') {
                msj += "Debe registrar 'Potencia para bajar' para el modo de operación." + "\n";
            }
            var PMaxDesc = objFila.PMaxDesc + '';
            if (PMaxDesc == '') {
                msj += "Debe registrar 'Potencia para subir' para el modo de operación." + "\n";
            }
            var BandaAdjudDesc = objFila.BandaAdjudDesc + '';
            if (BandaAdjudDesc == '') {
                msj += "Debe registrar 'Banda' para el modo de operación." + "\n";
            }
            var PrecMinDesc = objFila.PrecMinDesc + '';
            if (PrecMinDesc == '') {
                msj += "Debe registrar 'Precio para bajar' para el modo de operación." + "\n";
            }
            var PrecMaxDesc = objFila.PrecMaxDesc + '';
            if (PrecMaxDesc == '') {
                msj += "Debe registrar 'Precio para subir' para el modo de operación." + "\n";
            }
            var ModFechIni = objFila.ModFechIni + '';
            if (ModFechIni == '') {
                msj += "Debe registrar 'Fecha Inicio' para el modo de operación." + "\n";
            }
            var ModFechFin = objFila.ModFechFin + '';
            if (ModFechFin == '') {
                msj += "Debe registrar 'Fecha Fin' para el modo de operación." + "\n";
            }
        }
    }

    return msj;
}

function eventoTabla() {

    $('input[name=valorPotBajar]').unbind();
    $('input[name=valorPotBajar]').change(function () {
        actualizarFilaTabla(this.id + '');
    });
    $('input[name=valorPotBajar]').on('focusout', function (e) {
        actualizarFilaTabla(this.id + '');
    });

    $('input[name=valorPotSubir]').unbind();
    $('input[name=valorPotSubir]').change(function () {
        actualizarFilaTabla(this.id + '');
    });
    $('input[name=valorPotSubir]').on('focusout', function (e) {
        actualizarFilaTabla(this.id + '');
    });
}

function actualizarFilaTabla(idinput) {
    var posFila = idinput.substring(13, idinput.length)

    var valorMin = $("#valorPotBajar" + posFila).val();
    var valorMax = $("#valorPotSubir" + posFila).val();

    valorMin = parseFloat(valorMin) || 0.0;
    valorMax = parseFloat(valorMax) || 0.0;
    var banda = valorMax + valorMin;

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