var controlador = siteRoot + 'IND/Combustible/';
var LISTA_UNIDAD = [];
var ancho = 1000;
var ALTURA_HANDSON = 600;
var LISTA_HoT = [null];

$(function () {
    $('#cntMenu').css("display", "none");

    //#region IND.PR25.2022
    $('#tab-container').easytabs({
        animate: false
    });

    $('#tab-container').bind('easytabs:after', function () {
        refrehDatatable();
    });

    $('#tab-container').easytabs('select', '#vistaConsulta');

    $('#cbAnio2').change(function () {
        listadoPeriodo("#cbAnio2", "#cbPeriodo2", "#vistaImportacion");
    });

    $('#cbPeriodo2').change(function () {
        $("#dtFechaInicio2").val('');
        $("#dtFechaFin2").val('');
    });

    $('#dtFechaInicio2').Zebra_DatePicker();

    $('#dtFechaFin2').Zebra_DatePicker();

    $('#cbAnio').change(function () {
        listadoPeriodo("#cbAnio", "#cbPeriodo", "#vistaConsulta");
    });
    //#endregion

    $('#cbPeriodo').change(function () {
        //mostrarListado();
    });

    $('#cbEmpresa').multipleSelect({
        width: '250px',
        filter: true,
        single: false,
        onClose: function () {
            //mostrarListado();
        }
    });

    $('#cbEmpresa').multipleSelect('checkAll');

    $('#btnBuscar').click(function () {
        mostrarListado();
    });

    $("#btnExportarExcel").click(function () {
        exportarExcel();
    });

    $("#btnFormato").click(function () {
        var urlFmt = siteRoot + "Hidrologia/FormatoMedicion/Index?app=" + 4;
        window.open(urlFmt, '_blank');
    });

    $("#btnAmpliacion").click(function () {
        var urlFmt = siteRoot + "IEOD/Ampliacion/Index?app=" + 4;
        window.open(urlFmt, '_blank');
    });

    //#region IND.PR25.2022
    $('#btnImportar').click(function () {
        importarStock();
    });

    $('#popupEditar').bPopup().close();

    $('#cbNumeroDias').change(function () {
        setearStock();
    });

    $('#btnGuardar').click(function () {
        guardarStock();
    });
    //#endregion

    mostrarListado();
});

function exportarExcel() {
    var pericodi = parseInt($("#cbPeriodo").val()) || 0;

    $.ajax({
        type: 'POST',
        url: controlador + "GenerarArchivoExcelCumplimiento",
        data: {
            pericodi: pericodi,
            empresa: getEmpresa(),
        },
        success: function (evt) {
            if (evt.Resultado != "-1") {
                window.location = controlador + "Exportar?file_name=" + evt.Resultado;
            } else {
                alert("Ha ocurrido un error: " + evt.Mensaje);
            }
        },
        error: function (err) {
            alert('Ha ocurrido un error.');
        }
    });

}

function getEmpresa() {
    var empresa = $('#cbEmpresa').multipleSelect('getSelects');
    if (empresa == "[object Object]" || empresa.length == 0) empresa = "-1";
    $('#hfEmpresa').val(empresa);
    var idEmpresa = $('#hfEmpresa').val();

    return idEmpresa;
}

//#region IND.PR25.2022
function mostrarListado()
{
    ALTURA_HANDSON = parseInt($(".listado1").height());
    ancho = $("#mainLayout").width() > 1100 ? $("#mainLayout").width() - 30 : 800;

    $('#tab-container').easytabs('select', '#vistaConsulta');
    $("#mensajeListado1").html('');
    $("#listado1").html('');
    $("#div_cambios_app").hide();
    $("#div_tabla_cambios").html('');
    var container1 = document.getElementById('listado1');
    var ipericodi = parseInt($("#cbPeriodo").val()) || 0;

    $.ajax({
        type: 'POST',
        url: controlador + "CargarHtmlStockCombustibleDetalle",
        data: {
            ipericodi: ipericodi,
            emprcodi: getEmpresa(),
        },
        success: function (evt) {
            if (evt.Resultado == "1") {
                crearGrillaExcelStkCmtDet(0, container1, evt.HandsonStkCmtDet, ALTURA_HANDSON);

                if (evt.RptHtmlCambios != null && evt.RptHtmlCambios != "") {

                    $("#div_cambios_app").show();
                    $("#div_tabla_cambios").html(evt.RptHtmlCambios);

                    $('#tabla_cambios').dataTable({
                        "sPaginationType": "full_numbers",
                        "destroy": "true",
                        "ordering": false,
                        "searching": false,
                        "iDisplayLength": 15,
                        "info": false,
                        "paging": false,
                        "scrollX": true,
                        "scrollY": $('#div_tabla_cambios').height() > 300 ? 300 + "px" : "100%"
                    });
                }
            } else if (evt.Resultado == "-2") {
                $("#mensajeListado1").html(evt.Mensaje);
                $("#listado1").html('');
            } else {
                alert(evt.Mensaje);
            }
        },
        error: function (err) {
            alert('Ha ocurrido un error.');
        }
    });
}

function crearGrillaExcelStkCmtDet(tab, container, handson, heightHansonTab)
{
    if (typeof LISTA_HoT[tab] != 'undefined' && LISTA_HoT[tab] !== null) {
        LISTA_HoT[tab].destroy();
    }

    var ColorEmpresa = function (instance, td, row, col, prop, value, cellProperties) {
        Handsontable.renderers.TextRenderer.apply(this, arguments);
        td.style.backgroundColor = '#4F81BD';
        td.style.fontSize = '11px';
        td.style.color = 'white'
    };

    var LateralIzq = function (instance, td, row, col, prop, value, cellProperties) {
        Handsontable.renderers.TextRenderer.apply(this, arguments);
        td.style.backgroundColor = '#DCE6F1';
        td.style.fontSize = '11px';
        td.style.color = 'black';
    };

    var ColorDiferenteOriginal = function (instance, td, row, col, prop, value, cellProperties) {
        Handsontable.renderers.TextRenderer.apply(this, arguments);
        td.style.backgroundColor = '#EAFF05';
        td.style.fontSize = '11px';
        td.style.color = 'black';
    };

    var columns = handson.Columnas;
    var headers = handson.Headers;
    var widths = handson.ListaColWidth;
    var excelData = handson.ListaExcelData;
    var dataDescrip = handson.ListaExcelDescripcion;
    var dataFormato = handson.ListaExcelFormatoHtml;
    var arrMergeCells = handson.ListaMerge;
    var listaCambios = handson.ListaCambios;

    LISTA_HoT[tab] = new Handsontable(container, {
        data: excelData,
        stretchH: "all",
        observeChanges: true,
        colHeaders: headers,
        colWidths: widths,
        rowHeaders: true,
        columnSorting: false,
        minSpareRows: 0,
        readOnly: true,
        columns: columns,
        height: heightHansonTab,
        mergeCells: arrMergeCells,
        fixedColumnsLeft: 0,
        contextMenu: {
            items: {
                "Modificar": {
                    name: 'Modificar',
                    callback: function (key, selection, clickEvent) {
                        initPopupStkCmtDet(this, selection);
                    }
                }
            }
        },
        cells: function (row, col, prop) {
            var cellProperties = {
            };

            if (col == 0 && row >= 0) {
                cellProperties.renderer = ColorEmpresa;
            }
            if (col > 0 && col < 5 && row >= 0) {
                cellProperties.renderer = LateralIzq;
            }

            if (row >= 0) {
                switch (true) {
                    case (col >= 0 && col <=6):
                        break;
                    case (col > 6):
                        cellProperties.readOnly = true;
                        cellProperties.className = "htRight htMiddle";
                        cellProperties.type = 'numeric';
                        cellProperties.format = '0,0.00000';
                        var orig = excelData[row][col - 1];
                        var modi = excelData[row][col];
                        if (orig != modi)
                        {
                            cellProperties.renderer = ColorDiferenteOriginal;
                        }
                        break;
                }
            }
            return cellProperties;
        }
    });
}

function initPopupStkCmtDet(handson, selection)
{
    var lastStaticIndex = 5;
    var startIndexDayOrig = lastStaticIndex + 1;
    var col = selection.start.col;

    if (col <= startIndexDayOrig)
    {
        alert("No es una celda válida de stock de combustible.");
        return;
    }

    var row = selection.start.row;
    var filaData = handson.getDataAtRow(row);

    $("#txtEmprnomb").val(filaData[0]);
    $("#txtEquinombcentral").val(filaData[1]);
    $("#txtEquinombunidad").val(filaData[2]);
    $("#txtTipoinfodesc").val(filaData[3]);
    $("#txtStkDetCodi").val(filaData[4]);
    $("#txtNumDias").val(filaData[5]);
    var numDias = parseInt(filaData[5]);
    var index = lastStaticIndex;
    if (numDias > 0)
    {
        $('#cbNumeroDias').get(0).options[0] = new Option("", "0");
        $('#cbStockOrigDias').get(0).options[0] = new Option("", "0");
        $('#cbStockModiDias').get(0).options[0] = new Option("", "0");

        for (i = 1; i <= numDias; i++)
        {
            $('#cbNumeroDias').get(0).options[i] = new Option(i, i);
            index++;
            $('#cbStockOrigDias').get(0).options[i] = new Option(filaData[index], i);
            index++;
            $('#cbStockModiDias').get(0).options[i] = new Option(filaData[index], i);
        }
    }
    else
    {
        $('#cbNumeroDias').get(0).options[0] = new Option("", "0");
    }
    $("#txtRow").val(row);
    $("#txtStockOrigDia").val("");
    $("#txtStockModiDia").val("");

    if (col > startIndexDayOrig)
    {
        var dia = 0;
        var j = 1;
        for (i = 1; i <= 31; i++)
        {
            if (col == (startIndexDayOrig + j))
            {
                dia = i;
                break;
            }
            j = j + 2;
        }

        $("#cbNumeroDias option[value=" + dia + "]").prop("selected", true);
        setearStock();
    }

    $('#popupEditar').bPopup({
        easing: 'easeOutBack',
        speed: 450,
        transition: 'slideDown'
    });
}

function setearStock()
{
    var dia = parseInt($("#cbNumeroDias").val()) || 0;

    if (dia == 0)
    {
        $("#txtStockOrigDia").val("");
        $("#txtStockModiDia").val("");
    }
    else if (dia > 0)
    {
        $("#txtStockOrigDia").val($('#cbStockOrigDias').get(0).options[dia].text);
        if ($('#cbStockModiDias').get(0).options[dia].text == "No informó.")
        {
            $("#txtStockModiDia").val(0);
        }
        else
        {
            $("#txtStockModiDia").val($('#cbStockModiDias').get(0).options[dia].text);
        }
        
    }
}

function listadoPeriodo(idCbAnio, idCbPeriodo, idVista)
{
    var anio = parseInt($(idCbAnio).val()) || 0;

    $(idCbPeriodo).empty();

    $.ajax({
        type: 'POST',
        url: controlador + "PeriodoListado",
        data: {
            anio: anio,
        },
        success: function (evt) {
            if (evt.Resultado != "-1") {
                if (evt.ListaPeriodo.length > 0) {
                    $.each(evt.ListaPeriodo, function (i, item) {
                        $(idCbPeriodo).get(0).options[$(idCbPeriodo).get(0).options.length] = new Option(item.Iperinombre, item.Ipericodi);
                    });
                } else {
                    $(idCbPeriodo).get(0).options[0] = new Option("--", "0");
                }

                if (idVista == "#vistaConsulta") {
                    mostrarListado();
                }
            } else {
                alert("Ha ocurrido un error: " + evt.Mensaje);
            }
        },
        error: function (err) {
            alert('Ha ocurrido un error.');
        }
    });
}

function importarStock()
{
    if (!confirm("¿Está seguro que desea ejecutar la importación para el rango de fechas especificado?")) {
        return;
    }

    var ipericodi = parseInt($("#cbPeriodo2").val()) || 0;
    var fechainicio = $("#dtFechaInicio2").val() || '';
    var fechafin = $("#dtFechaFin2").val() || '';
    var empresa = "-1";
    var historico = $("#cbHistorico").is(":checked");

    $.ajax({
        type: 'POST',
        url: controlador + "ImportarStock",
        data: {
            ipericodi: ipericodi,
            fechainicio: fechainicio,
            fechafin: fechafin,
            empresa: empresa,
            historico: historico
        },
        success: function (evt) {
            if (evt.Resultado != "-1") {
                $("#mensajeListado1").html('');
                $("#listado1").html('');
                $("#div_cambios_app").hide();
                $("#div_tabla_cambios").html('');
                alert(evt.Mensaje);
            } else {
                alert("Ha ocurrido un error: " + evt.Mensaje);
            }
        },
        error: function (err) {
            alert("Ha ocurrido un error");
        }
    });
}

function guardarStock()
{
    var dia = parseInt($("#cbNumeroDias").val()) || 0;
    if (dia == 0) { alert("Debe seleccionar el día de stock que desea actualizar"); return; }

    var stkdetcodiModi = parseInt($("#txtStkDetCodi").val()) || 0;
    if (stkdetcodiModi == 0) { alert("No existe un id de Stock de Detalle. Revisar"); return; }

    var stockOrig = $("#txtStockOrigDia").val();
    var stockModi = $("#txtStockModiDia").val();
    if (stockModi == null || stockModi.trim() == "") { alert("El stock modificado debe tener un valor numérico"); return; }
    if (!validarNumero(stockModi)) { alert("El stock modificado debe ser un valor numérico"); return; }

    $.ajax({
        type: 'POST',
        url: controlador + "ActualizarStock",
        data: {
            stkdetcodiModi: stkdetcodiModi,
            dia: dia,
            stockOrig: stockOrig,
            stockModi: stockModi,
        },
        success: function (evt) {
            if (evt.Resultado != "-1") {
                alert(evt.Mensaje);
                $('#popupEditar').bPopup().close();
                mostrarListado();
            } else {
                alert("Ha ocurrido un error: " + evt.Mensaje);
            }
        },
        error: function (err) {
            alert("Ha ocurrido un error");
        }
    });
}

function validarNumero(item, evt)
{
    evt = (evt) ? evt : window.event;
    var charCode = (evt.which) ? evt.which : evt.keyCode;

    if (charCode == 46) {
        var regex = new RegExp(/\./g)
        var count = $(item).val().match(regex).length;
        if (count > 1) {
            return false;
        }
    }

    if (charCode > 31 && (charCode < 48 || charCode > 57)) {
        return false;
    }

    return true;
}

function refrehDatatable()
{
    $('#tabla_cambios').dataTable({
        "sPaginationType": "full_numbers",
        "destroy": "true",
        "ordering": false,
        "searching": false,
        "iDisplayLength": 15,
        "info": false,
        "paging": false,
        "scrollX": true,
        "scrollY": $('#div_tabla_cambios').height() > 300 ? 300 + "px" : "100%"
    });
}
//#endregion