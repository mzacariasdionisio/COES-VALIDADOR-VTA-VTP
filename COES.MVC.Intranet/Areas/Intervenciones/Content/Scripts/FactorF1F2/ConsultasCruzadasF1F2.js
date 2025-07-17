var controladorF1F2 = siteRoot + 'Intervenciones/FactorF1F2/';

var APP_MOSTRAR_MODAL_EQUIPO_YA_CARGADO = 0;
var INTERVENCION_WEB = null;
var INTERVENCION_GLOBAL = null;
var INTERVENCION_GLOBAL2 = null;
var ANCHO_LISTADO_EMS = 1200;
var OPCION_VER = 1;
var OPCION_EDITAR = 2;
var OPCION_NUEVO = 3;
var OPCION_ACTUAL = 0;

var hot;
var MODELO_GRID = null;

// btn expandir / contraer;
var ocultar = 0;


$(function () {
    $('#cboF1F2Disponibilidad').change(function () {
        cargarListaEquipoF1F2();
    });
    $('#cbEmpresaF1F2').change(function () {
        cargarListaUbicacionF1F2();
    });
    $('#cbUbicacionF1F2').change(function () {
        cargarListaEquipoF1F2();
    });
    $('#cbEquipoF1F2').change(function () {
        actualizarGrilla();
    });

    $('#cboMantenimiento').multipleSelect({
        filter: true,
        placeholder: "SELECCIONE",
        onClose: function (view) {
            cambiarMantenimiento();
        }
    });

    $('#cboFrecuencia').multipleSelect({
        filter: true,
        placeholder: "SELECCIONE",
        onClose: function (view) {
            actualizarGrilla();
        }
    });

    $('#btnGenerarReporte').click(function () {
        GenerarReporte();
    });

    $('#btnCancelar').click(function () {
        $('#idPopupReporte').bPopup().close();
    });

    $('#btnExportarCruzadas').click(function () {
        setTimeout(function () {
            $('#idPopupReporte').bPopup({
                easing: 'easeOutBack',
                speed: 450,
                transition: 'slideDown',
                modalClose: false
            });
        }, 50);
    });

    $('#cboEmpresaFiltroF1F2').multipleSelect('checkAll');
    $('#cboMantenimiento').multipleSelect('checkAll');
    $('#cboFrecuencia').multipleSelect('checkAll');

    ANCHO_LISTADO_EMS = ANCHO_LISTADO_EMS > 1200 ? ANCHO_LISTADO_EMS : 1200;
    $(window).resize(function () {
        $('#listado').css("width", ANCHO_LISTADO_EMS + "px");
    });

    actualizarGrilla();

    $("#btnRegresarCruzada").click(function () {
        var fecha = $("#infverfechaperiodo").val();
        window.location.href = siteRoot + "Intervenciones/FactorF1F2/Index?fechaPeriodo=" + fecha;
    });

    $(document).off('click', '.intervencion');
    $(document).on('click', '.intervencion', function (e) {
        // your function here
        e.preventDefault();
        var interCodi = $(this).attr("class").split(" ")[1].split("_")[1];
        editarCeldaIntervencionF1F2(interCodi);
    });
});

actualizarGrilla = function () {
    $('#mensaje').css("display", "none");
    mostrarGrillaExcelF1F2()
};

function editarCeldaIntervencionF1F2(vinterCodi) {
    objPosSelecOrigen = objPosSelecTmp; //guardar en memoria la celda seleccionada

    $('#popupFormIntervencion').html('');
    $("#busquedaEquipo").remove();

    if (APP_MOSTRAR_MODAL_EQUIPO_YA_CARGADO === undefined) { } else {
        APP_MOSTRAR_MODAL_EQUIPO_YA_CARGADO = 0;
    }

    var objParam = {
        interCodi: vinterCodi,
        progrCodi: 0,
        tipoProgramacion: 0,
        escruzadas: true,
        esFlotante: true,
        lecturaF1F2: 1
    };

    //IntervencionesFormulario.js
    mostrarIntervencion(objParam);
};

function cambiarMantenimiento() {
    var seleccionMantenimiento = $('#cboMantenimiento').multipleSelect('getSelects'); // Obtiene la selección de cboMantenimiento
    var cboFrecuencia = $('#cboFrecuencia');

    // Remueve todas las opciones actuales de cboFrecuencia
    cboFrecuencia.empty();

    // Agrega las opciones en función de la selección en cboMantenimiento
    if (seleccionMantenimiento.includes("1")) {
        cboFrecuencia.append('<option value="Mmay" id="option1">Mant. Mensual – M</option>');
        cboFrecuencia.append('<option value="Smay" id="option2">Mant. Semanal – S</option>');
        cboFrecuencia.append('<option value="Dmay" id="option3">Mant. Diario -D</option>');
        cboFrecuencia.append('<option value="Emay" id="option4">Mant. Ejecutado Mayor – E</option>');
    }

    if (seleccionMantenimiento.includes("2")) {
        cboFrecuencia.append('<option value="emen" id="option5">Mant. Ejecutado Menor del mensual (<24h) - em</option>');
        cboFrecuencia.append('<option value="etr" id="option6">Mant. Ejecutado Menor (<24h) - e</option>');
    }

    // Refresca cboFrecuencia para que se muestren las nuevas opciones
    cboFrecuencia.multipleSelect('refresh');
    $('#cboFrecuencia').multipleSelect('checkAll');

    actualizarGrilla();
};

function mostrarGrillaExcelF1F2(loadData) {
    var msj = "";

    var objData = getObjetoFiltroF1F2();

    if (msj == "") {
        $.ajax({
            type: 'POST',
            url: controladorF1F2 + "GrillaExcel",
            data: objData,
            dataType: 'json',
            success: function (result) {
                if (result.Resultado != "-1") {
                    if (loadData) {
                        refrescarCeldasModificadas(result.GridExcel);
                    } else {
                        $("#grillaExcel").hide();
                        $("#alerta").hide();

                        if (result.Resultado == '-1') {
                            $("#grillaExcel").hide();
                            alert(result.Mensaje);
                            return;
                        }

                        MODELO_GRID = result.GridExcel;
                        generarHoTwebF1F2();
                    }

                } else {
                    alert(result.StrMensaje);
                }
            },
            error: function (err) {
                alert('Lo sentimos no se puede mostrar la consulta . *Revise que el rango de fechas no debe de sobrepasar el año')
            }
        });
    } else {
        alert(msj);
    }
}

function GenerarReporte() {
    var idEmpresa = getObjetoFiltroF1F2().Emprcodi;
    var ubicacion = getObjetoFiltroF1F2().Areacodi;
    var equipo = getObjetoFiltroF1F2().Equicodi;
    var tipoReporte = parseInt($("input[name='rbMantenimiento']:checked").val()) || 0;
    var infvercodi = parseInt($('#infvercodi').val()) || 0;

    $.ajax({
        type: 'POST',
        url: controladorF1F2 + "GenerarArchivoExcelReporteCruzado",
        data: {
            tipoReporte: tipoReporte,
            infvercodi: infvercodi,
            idEmpresa: idEmpresa,
            ubicacion: ubicacion,
            equipo: equipo,
        },
        success: function (evt) {
            if (evt.Resultado != "-1") {
                window.location = controladorF1F2 + "ExportarReporte?nameFile=" + evt.Resultado;
            } else {
                alert("Ha ocurrido un error: " + evt.Mensaje);
            }
        },
        error: function (err) {
            alert('Ha ocurrido un error.');
        }
    });
}

function generarHoTwebF1F2() {
    if (typeof hot != 'undefined') {
        hot.destroy();
    }

    var nuevoTamanioTabla = getHeightTablaListado();
    $("#grillaExcel").show();
    nuevoTamanioTabla = nuevoTamanioTabla > 250 ? nuevoTamanioTabla : 250;

    var container = document.getElementById('grillaExcel');
    hot = new Handsontable(container, {
        data: MODELO_GRID.Data,
        maxCols: MODELO_GRID.Columnas.length,
        colHeaders: MODELO_GRID.Headers,
        colWidths: MODELO_GRID.Widths,
        height: nuevoTamanioTabla,
        fixedColumnsLeft: MODELO_GRID.FixedColumnsLeft,
        columnSorting: true,
        manualColumnResize: true,
        sortIndicator: true,
        rowHeaders: false,
        columns: MODELO_GRID.Columnas,
        //hiddenColumns: { //no ocultar columnas porque luego no se renderizan bien algunas celdas
        //    // specify columns hidden by default
        //    columns: [4, 5, 6]
        //},
        outsideClickDeselects: false,
        copyPaste: false,
        autoRowSize: { syncLimit: 30000 },
        cells: function (row, col, prop) {
            var cellProperties = {};

            var columnsColor = MODELO_GRID.ListaColumnasColor;
            for (var i = 0; i < columnsColor.length; i++) {
                if (col == columnsColor[i].indexcabecera && columnsColor[i].isendofweek) {
                    if (columnsColor[i].itypeendofweek == 1) //sabado
                        cellProperties.renderer = firstRowRendererColorSabadoAndSafeHtml;
                    if (columnsColor[i].itypeendofweek == 2) //domingo / feriado
                        cellProperties.renderer = firstRowRendererColorDomingoAndSafeHtml;
                } else if (col == columnsColor[i].indexcabecera && columnsColor[i].isendofweek == false) {
                    cellProperties.renderer = safeHtmlRenderer;
                }
            }

            if (col < MODELO_GRID.FixedColumnsLeft) {
                cellProperties.renderer = titleHtmlRenderer;
            }

            return cellProperties;
        }
    });

    hot.addHook('afterOnCellMouseDown', function (event, coords, TD) {
        if (coords.row == -1 && (coords.col >= MODELO_GRID.FixedColumnsLeft && coords.col < MODELO_GRID.FixedColumnsLeft + MODELO_GRID.ListaFecha.length)) {
            var posCol = coords.col - MODELO_GRID.FixedColumnsLeft;
            generarVentanaFlotanteDetalle(MODELO_GRID.ListaFecha[posCol]);
        }
    });

    container.addEventListener('dblclick', function (e) {
        hot.getActiveEditor().close();
    });

    hot.render();
    updateDimensionHandson(hot);
}

function getObjetoFiltroF1F2() {
    var infvercodi = parseInt($('#infvercodi').val()) || 0;
    var infverfechaperiodo = $('#infverfechaperiodo').val();
    var Frecuenciacodi = $('#cboFrecuencia').multipleSelect('getSelects').join(',');
    var disponibilidad = $('#cboF1F2Disponibilidad').val();
    
    var obj = {
        Infverfechaperiodo: infverfechaperiodo,
        Frecuenciacodi: Frecuenciacodi,
        Infvercodi: infvercodi,
        Emprcodi: getEmpresaF1F2(),
        Areacodi: getUbicacionF1F2(),
        Equicodi: getEquipoF1F2(),
        Disponibilidad: disponibilidad
    };

    return obj;
}

///////////////////////////
/// Listar filtros 
///////////////////////////

function cargarListaUbicacionF1F2() {
    $("#div_ubicacion_filtro").html('');

    $.ajax({
        type: 'POST',
        url: controladorF1F2 + 'ViewCargarFiltros',
        dataType: 'json',
        data: {
            infvercodi: getVersionF1F2(),
            infmmhoja: getHojaF1F2(),
            empresa: getEmpresaF1F2(),
            ubicacion: getUbicacionF1F2(),
        },
        cache: false,
        success: function (data) {
            //ubicacion
            $("#div_ubicacion_filtroF1F2").html('<select id="cbUbicacionF1F2"><option value="-1">--TODOS--</option></select>');

            if (data.ListaUbicacion.length > 0) {
                $.each(data.ListaUbicacion, function (i, item) {
                    $('#cbUbicacionF1F2').get(0).options[$('#cbUbicacionF1F2').get(0).options.length] = new Option(item.Areanomb, item.Areacodi);
                });
            }

            $('#cbUbicacionF1F2').unbind();
            $('#cbUbicacionF1F2').change(function () {
                cargarListaEquipoF1F2();
            });

            //equipo
            $("#div_equipo_filtroF1F2").html('<select id="cbEquipoF1F2"><option value="-1">--TODOS--</option></select>');

            if (data.ListaEquipo.length > 0) {
                $.each(data.ListaEquipo, function (i, item) {
                    $('#cbEquipoF1F2').get(0).options[$('#cbEquipoF1F2').get(0).options.length] = new Option(item.Equiabrev, item.Equicodi);
                });
            }

            $('#cbEquipoF1F2').unbind();
            $('#cbEquipoF1F2').change(function () {
                actualizarGrilla();
            });

            actualizarGrilla();
        },
        error: function (err) {
            alert('Ha ocurrido un error.');
        }
    });
}

function cargarListaEquipoF1F2() {
    $("#div_equipo_filtroF1F2").html('');

    $.ajax({
        type: 'POST',
        url: controladorF1F2 + 'ViewCargarFiltros',
        dataType: 'json',
        data: {
            infvercodi: getVersionF1F2(),
            infmmhoja: getHojaF1F2(),
            empresa: getEmpresaF1F2(),
            ubicacion: getUbicacionF1F2(),
        },
        cache: false,
        success: function (data) {
            //equipo
            $("#div_equipo_filtroF1F2").html('<select id="cbEquipoF1F2"><option value="-1">--TODOS--</option></select>');

            if (data.ListaEquipo.length > 0) {
                $.each(data.ListaEquipo, function (i, item) {
                    $('#cbEquipoF1F2').get(0).options[$('#cbEquipoF1F2').get(0).options.length] = new Option(item.Equiabrev, item.Equicodi);
                });
            }

            $('#cbEquipoF1F2').unbind();
            $('#cbEquipoF1F2').change(function () {
                actualizarGrilla();
            });

            actualizarGrilla();
        },
        error: function (err) {
            alert('Ha ocurrido un error.');
        }
    });
}

function getEmpresaF1F2() {
    var item = $('#cbEmpresaF1F2').val();

    return item;
}

function getUbicacionF1F2() {
    var item = $('#cbUbicacionF1F2').val();

    return item;
}

function getEquipoF1F2() {
    var item = $('#cbEquipoF1F2').val();

    return item;
}

function getVersionF1F2() {
    return $("#hfVersionF1F2").val();
}

function getHojaF1F2() {
    return $("#hfHojaF1F2").val();
}