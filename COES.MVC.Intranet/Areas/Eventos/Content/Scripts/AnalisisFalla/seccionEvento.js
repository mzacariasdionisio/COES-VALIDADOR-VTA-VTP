var controlador = siteRoot + 'Eventos/AnalisisFallas/';
var columnaChecksSecEvent = 4;
var comboBoxCC;
var indexListaSEcuenciaEvento = 0;
let tipoCondPrevia = "";
var comboBoxNumeral;
var comboBoxNumeroDefaultValue;
let indexListaAnalisisEvento = 0;
var hotAnalisisEvento = null;
var hotLinea = null;
var hotCentral = null;
var hotTransformadores = null;
var hotHistoricoScadaAnalisis = null;
var hotSecuenciaEvento = null;

$(function () {
    $('#tab_container_Sco').easytabs(
        { animate: false }
    );
    
    new SlimSelect({
        select: '#cbZona'
    });

    new SlimSelect({
        select: '#cbCanal'
    });

    new SlimSelect({
        select: '#cbZonaInvolucrados'
    });

    new SlimSelect({
        select: '#cbCanalInvolucrados'
    });

    new SlimSelect({
        select: '#cbZonaTransformadores'
    });

    new SlimSelect({
        select: '#cbCanalTransformadores'
    });

    new SlimSelect({
        select: '#cbZonaAnalisis'
    });

    new SlimSelect({
        select: '#cbCanalAnalisis'
    });

    llenarCombo();
    llenarComboNumeral();

    ListadoCondPreviaLinea();
    ListadoCondPreviaCentral();
    ListadoCondPreviaTransformadores();
    ListadoSecuenciasEvento();
    ListadoInterruptores();
    ListadoDescargadores();
    ListadoHistoricoScadaAnalisis();
    ListadoAnalisisEvento();
    
    document.getElementById("cbCanal").style.fontFamily = 'Courier New';

    $('#cbZona').change(function () {
        $('#cbCanal').empty();
        cargarCanalPorZona($('#cbZona').val(), 1);
    });

    $('#cbZona').val(0);

    document.getElementById("cbCanalInvolucrados").style.fontFamily = 'Courier New';

    $('#cbZonaInvolucrados').change(function () {
        $('#cbCanalInvolucrados').empty();
        cargarCanalPorZona($('#cbZonaInvolucrados').val(), 2);
    });

    $('#cbZonaInvolucrados').val(0);

    document.getElementById("cbCanalTransformadores").style.fontFamily = 'Courier New';

    $('#cbZonaTransformadores').change(function () {
        $('#cbCanalTransformadores').empty();
        cargarCanalPorZona($('#cbZonaTransformadores').val(), 3);
    });

    $('#cbZonaTransformadores').val(0);

    document.getElementById("cbCanalAnalisis").style.fontFamily = 'Courier New';

    $('#cbZonaAnalisis').val(0);

    $('#cbZonaAnalisis').change(function () {
        $('#cbCanalAnalisis').empty();
        cargarCanalPorZona($('#cbZonaAnalisis').val(), 4);
    });

    $("#btngrabarInterruptoresDescargadores").click(function () {
        GrabarInterruptoresDescargadores();
    });

    $('#txtFechaDesdeAnalisis').Zebra_DatePicker({
        readonly_element: false,
        onSelect: function (date) {
            $('#txtFechaDesdeAnalisis').val(date);
        }
    });

    $('#txtFechaDesdeAnalisis').inputmask({
        mask: "1/2/y",
        placeholder: "dd/mm/yyyy",
        alias: "datetime"

    });

    $('#txtFechaHastaAnalisis').Zebra_DatePicker({
        readonly_element: false,
        onSelect: function (date) {
            $('#txtFechaHastaAnalisis').val(date);
        }
    });

    $('#txtFechaHastaAnalisis').inputmask({
        mask: "1/2/y",
        placeholder: "dd/mm/yyyy",
        alias: "datetime"

    });

    $("#txtHoraIniAnalisis").inputmask("h:s", { "placeholder": "hh/mm" });
    $("#txtHoraFinAnalisis").inputmask("h:s", { "placeholder": "hh/mm" });

    $("#btnBuscarCondicionesPrevias").click(function () {
        ListarScada("L");
        tipoCondPrevia = "L";
    });
    $("#btnBuscarCentrales").click(function () {
        ListarScada("C");
        tipoCondPrevia = "C";
    });
    $("#btnBuscarTransformadores").click(function () {
        ListarScada("T");
        tipoCondPrevia = "T";
    });
    
    $("#btnGrabarScada").click(function () {
        if (tipoCondPrevia != "")
            InsertarCondicionPrevia(tipoCondPrevia, 1);
    });

    $("#btnAgregarAnalisis").click(function () {
        InsertarHistScadaAnalisisEvento(1);
    });

    $("#confirmarCerrarPopupGenerarAnalisis").click(function (event) {
        $("#GenerarAnalisisModal").css("display", "none");
    });

    $("#btnGenerarAnalisis").click(function (event) {
        event.preventDefault();
        $("#GenerarAnalisisModal").css("display", "flex");
    });

    $("#confirmarGenerarAnalisis").click(function (event) {
        event.preventDefault();
        $("#GenerarAnalisisModal").css("display", "none");
        InsertarAnalisisEvento();
    });

    $("#btnAgregarRecomendacionObs").click(function () {
        InsertarRecomendacionObservacion(1);
    });

    $("#btnAgregarObservacionobs").click(function () {
        InsertarRecomendacionObservacion(2);
    });



    $("#btngrabareventoSCO").click(function () {
        GrabarEtapasEventoSCO();
    });
    
    
});

cargarCanalPorZona = function (zonacodi, tipo) {

    $.ajax({
        type: 'POST',
        url: controlador + "listacanalporzona",
        data: {
            zonaCodi: zonacodi, tipo: tipo
        },
        dataType: 'json',
        cache: false,
        success: function (evt) {
            var _len = evt.length;
            //var cad1 = _len + "\r\n";
            for (i = 0; i < _len; i++) {

                post = evt[i];
                if (tipo == 1) {                   
                    var select = document.getElementById("cbCanal");
                    select.options[select.options.length] = new Option(post.Canalnomb, post.Canalcodi);
                }
                else if (tipo == 2) {                  
                    var select = document.getElementById("cbCanalInvolucrados");
                    select.options[select.options.length] = new Option(post.Canalnomb, post.Canalcodi);
                }
                else if (tipo == 3) {                   
                    var select = document.getElementById("cbCanalTransformadores");
                    select.options[select.options.length] = new Option(post.Canalnomb, post.Canalcodi);
                }
                else if (tipo == 4) {                  
                    var select = document.getElementById("cbCanalAnalisis");
                    select.options[select.options.length] = new Option(post.Canalnomb, post.Canalcodi);
                }
            }
        },
        error: function (xhr, textStatus, exceptionThrown) {
            mostrarError();
        }
    });

}

function ListadoInterruptores() {
    var controlador = siteRoot + 'Eventos/AnalisisFallas/';
    var evencodi = $("#hfEvencodicheck").val();
    $.ajax({
        type: 'POST',
        url: controlador + 'ListadoInterruptoresDescargadores',
        data: { evencodi: evencodi, tipo: 1 },
        success: function (resultado) {
            $("#InterruptoresTable").html('');
            if (resultado.Resultado != '-1') {
                crearHandsonTableInterruptores(resultado);
            }
        },
        error: function (err) {
            alert("Ingresa al error " + err);
        }
    });
}

function ListadoDescargadores() {
    var controlador = siteRoot + 'Eventos/AnalisisFallas/';
    var evencodi = $("#hfEvencodicheck").val();
    $.ajax({
        type: 'POST',
        url: controlador + 'ListadoInterruptoresDescargadores',
        data: { evencodi: evencodi, tipo: 2 },
        success: function (resultado) {
            $("#DescargadoresTable").html('');
            if (resultado.Resultado != '-1') {
                crearHandsonTableDescargadores(resultado);
            }
        },
        error: function (err) {
            alert("Ingresa al error " + err);
        }
    });
}

function ListadoSecuenciasEvento() {
    if (!!hotSecuenciaEvento && hotSecuenciaEvento != null) {
        hotSecuenciaEvento.destroy();
    }
    var controlador = siteRoot + 'Eventos/AnalisisFallas/';
    var evencodi = $("#hfEvencodicheck").val();
    $.ajax({
        type: 'POST',
        url: controlador + 'ListadoSecuenciasEventoSco',
        data: { evencodi: evencodi },
        success: function (resultado) {
            $("#SecuenciasEventoTable").html('');
            if (resultado.Resultado != '-1') {
                crearHandsonTableSecuenciaEvento(resultado);
            }
        },
        error: function (err) {
            alert("Ingresa al error " + err);
        }
    });
}

function llenarCombo() {
    $.ajax({
        type: 'POST',
        url: controlador + "ListaEmpresaConfiguracion",
        dataType: 'json',
        cache: false,
        success: function (evt) {
            comboBoxCC = evt;
            var _len = evt.length;
            //comboBoxCC = '<select name="select" id="btnSelectEmpresa{0}">';
            //comboBoxCC = comboBoxCC + '<option value=""></option>';
            //comboBoxCC = comboBoxCC + '<option value="' + 1 + '">CCO-COES</option>';
            //for (i = 0; i < _len; i++) {
            //    post = evt[i];
            //    comboBoxCC = comboBoxCC + '<option value="' + post.Emprcodi + '">' + post.EmpresaSICCOES + '</option>';
            //}
            //comboBoxCC = comboBoxCC + '</select >';
        },
        error: function (xhr, textStatus, exceptionThrown) {
            mostrarError();
        }

    });

}

var containerSecuenciaEvento
function crearHandsonTableSecuenciaEvento(evtHot) {

    function firstRowRenderer(instance, td, row, col, prop, value, cellProperties) {
        Handsontable.renderers.TextRenderer.apply(this, arguments);
        td.style.fontWeight = 'bold';
        td.style.fontSize = '10px';
        td.style.color = 'White';
        td.style.background = '#2980B9';
        td.style.textAlign = 'center';
    }

    function descripRowRenderer2(instance, td, row, col, prop, value, cellProperties) {
        Handsontable.renderers.TextRenderer.apply(this, arguments);
        td.style.fontSize = '10px';
        td.style.color = 'MidnightBlue';
        td.style.textAlign = 'center';

        if (row > 0 && col == 0) {
            const RegexFecha = /^(0?[1-9]|[12][0-9]|3[01])[\/](0?[1-9]|1[012])[\/]\d{4}$/;
            const isValidFecha = RegexFecha.test(value);
            if (!isValidFecha) {
                evtHot.Handson.ListaExcelData[row][col] = "";
                $(td).html("");
            }
        }

        if (row > 0 && col == 1) {
            const RegexHoramin = /^([0-1]?[0-9]|2[0-3]):[0-5][0-9]$/gm;
            const RegexHorminseg = /^([0-1]?[0-9]|2[0-3]):([0-5][0-9])(:[0-5][0-9])?$/gm;
            const timeRegex = /^([0-1]?[0-9]|2[0-3]):([0-5][0-9]):([0-5][0-9])(\.)(\d{3})$/gm;
            const isValidTime = timeRegex.test(value);
            const isValiHorMin = RegexHoramin.test(value);
            const isValiHorMinSeg = RegexHorminseg.test(value);

            if (!isValiHorMin) {
                if (!isValiHorMinSeg) {
                    if (!isValidTime) {
                        evtHot.Handson.ListaExcelData[row][col] = "";
                        $(td).html("");
                    }
                }
            }
        }
    }

    function cambiosCellRenderer(instance, td, row, col, prop, value, cellProperties) {
        Handsontable.renderers.TextRenderer.apply(this, arguments);
        td.style.fontSize = '12px';
        td.style.textAlign = 'right';
        td.style.background = '#FFFFD7';
        $(td).html(formatFloat(parseFloat(value), 3, '.', ','));
    }

    function negativeValueRenderer(instance, td, row, col, prop, value, cellProperties) {
        Handsontable.renderers.TextRenderer.apply(this, arguments);

        if (parseInt(value, 10) < 0) {
            td.style.color = 'orange';
            td.style.fontStyle = 'italic';
        }
    }

    function checkRenderer(instance, td, row, col, prop, value, cellProperties) {
        Handsontable.renderers.CheckboxRenderer.apply(this, arguments);
        td.style.fontSize = '12px';
        td.style.textAlign = 'center';
        td.style.color = 'DimGray';
        td.style.background = 'silver';
    }

    function calculateSize() {
        var offset = Handsontable.Dom.offset(containerSecuenciaEvento);

        if (offset.top == 222) {
            availableHeight = $(window).height() - 140 - offset.top - 20;
        }
        else {
            availableHeight = $(window).height() - 140 - offset.top - 20;
        }

        if (offset.left > 0)
            availableWidth = $(window).width() - 2 * offset.left;
        if (offset.top > 0) {
            availableHeight = availableHeight < HEIGHT_MINIMO ? HEIGHT_MINIMO : availableHeight;
            containerSecuenciaEvento.style.height = availableHeight + 'px';
        }
    }

    function llenarComboLista(row, col, prop, evtHot) {

        var NroEntrada = indexListaSEcuenciaEvento;

        if (evtHot.Handson.ListaExcelData[row][1] == null || evtHot.Handson.ListaExcelData[row][1] == 0) {

            var id = "btnSelectEmpresa" + row;
            var comboSeleccionado = $("#" + id).val();

            if (row != 0) {
                evtHot.Handson.ListaExcelData[row][1] = comboSeleccionado;
            }

        } else {

            $("#btnSelectEmpresa" + row).on('change', function (event) {
                evtHot.Handson.ListaExcelData[row][1] = $(this).val();
            });

        }

        indexListaSEcuenciaEvento = indexListaSEcuenciaEvento + 1;
    }

    function CboComboBox(instance, td, row, col, prop, value, cellProperties) {
        Handsontable.renderers.TextRenderer.apply(this, arguments);
        var comboBox = comboBoxCC.replace('{0}', row);
        var id = "btnSelectEmpresa" + row;
        var comboSeleccionado = evtHot.Handson.ListaExcelData[row][1] == null ? 0 : evtHot.Handson.ListaExcelData[row][1];
        td.innerHTML = comboBox;
        if (comboSeleccionado != 0) {
            $("#" + id).val(comboSeleccionado);
        }

    }

    var contexMenuSecuencia = {
        items: {
            'row_below': { name: "Insertar fila abajo" },
            'remove_row': { name: "Eliminar fila" }
        }
    };

    containerSecuenciaEvento = document.getElementById('SecuenciasEventoTable');
    Handsontable.renderers.registerRenderer('negativeValueRenderer', negativeValueRenderer);

    for (var p = 1; p < evtHot.Handson.ListaExcelData.length; p++) {
        if (evtHot.Handson.ListaExcelData[p][4] == "1")
            evtHot.Handson.ListaExcelData[p][4] = true;
        else
            evtHot.Handson.ListaExcelData[p][4] = false;
    }

    hotOptions = {
        data: evtHot.Handson.ListaExcelData,
        maxCols: evtHot.Handson.ListaExcelData[0].length,
        colHeaders: false,
        rowHeaders: false,
        contextMenu: contexMenuSecuencia,
        fillHandle: true,
        columnSorting: false,
        className: "htCenter",
        readOnly: evtHot.Handson.ReadOnly,
        fixedRowsTop: 1,
        fixedColumnsLeft: evtHot.Handson.ColCabecera,
        mergeCells: evtHot.Handson.ListaMerge,
        colWidths: evtHot.Handson.ListaColWidth,
        columns: [
            {
                data: 0,
                type: "date",
                allowInvalid: false
            },
            {
                data: 1,
                type: "text"
            },
            {
                data: 2,
                type: "autocomplete",
                strict: true,
                filter: false,
                source: comboBoxCC
            },
            {
                data: 3,
                type: "text"
            },
            {
                data: 4,
                type: "checkbox",
                className: "htCenter"
            }
        ],
        afterLoadData: function () {
            this.render();
        },
        cells: function (row, col, prop) {

            var cellProperties = {};
            var formato = "";
            var render;
            var readOnly = true;
            var tipo;

            if (row == 0) {
                render = firstRowRenderer;
                readOnly = true;
            }

            if (row >= evtHot.Handson.FilasCabecera && row <= evtHot.Handson.ListaExcelData.length) {
                //llenarComboLista(row, col, prop, evtHot);
                //if (col == 2)
                //    render = CboComboBox;
                if (col != 2) {
                    if (col == columnaChecksSecEvent) {
                        tipo = 'checkbox';
                        formato = '';
                        render = checkRenderer;
                        readOnly = false;
                    }

                    else
                        render = descripRowRenderer2;
                }

                //if (col == 2)
                //    readOnly = true;
                //else
                readOnly = false;
            }

            for (var i in evtHot.ListaCambios) {
                if ((row == evtHot.ListaCambios[i].Row) && (col == evtHot.ListaCambios[i].Col)) {
                    render = cambiosCellRenderer;
                }
            }

            cellProperties = {
                renderer: render,
                format: formato,
                type: tipo,
                readOnly: readOnly
            }

            return cellProperties;
        }
    };

    hotSecuenciaEvento = new Handsontable(containerSecuenciaEvento, hotOptions);
    hotSecuenciaEvento.render();
    calculateSize(1);
}

var containerInterruptores
function crearHandsonTableInterruptores(evtHot) {

    function firstRowRenderer(instance, td, row, col, prop, value, cellProperties) {
        Handsontable.renderers.TextRenderer.apply(this, arguments);
        td.style.fontWeight = 'bold';
        td.style.fontSize = '10px';
        td.style.color = 'White';
        td.style.background = '#2980B9';
        td.style.textAlign = 'center';
    }

    function descripRowRenderer2(instance, td, row, col, prop, value, cellProperties) {
        Handsontable.renderers.TextRenderer.apply(this, arguments);
        td.style.fontSize = '10px';
        td.style.color = 'MidnightBlue';
        //td.style.background = '#E8F6FF';
        td.style.textAlign = 'center';
    }

    function negativeValueRenderer(instance, td, row, col, prop, value, cellProperties) {
        Handsontable.renderers.TextRenderer.apply(this, arguments);

        if (parseInt(value, 10) < 0) {
            td.style.color = 'orange';
            td.style.fontStyle = 'italic';
        }
    }

    function calculateSize() {
        var offset = Handsontable.Dom.offset(containerInterruptores);
        if (offset.top == 222) {
            availableHeight = $(window).height() - 140 - offset.top - 20;
        }
        else {
            availableHeight = $(window).height() - 140 - offset.top - 20;
        }

        if (offset.left > 0)
            availableWidth = $(window).width() - 2 * offset.left;
        if (offset.top > 0) {
            availableHeight = availableHeight < HEIGHT_MINIMO ? HEIGHT_MINIMO : availableHeight;
        }
    }
    var contexMenuInterruptores = {
        items: {
            'row_below': { name: "Insertar fila abajo" },
            'remove_row': { name: "Eliminar fila" }
        }
    };
    containerInterruptores = document.getElementById('InterruptoresTable');
    Handsontable.renderers.registerRenderer('negativeValueRenderer', negativeValueRenderer);

    hotOptions = {
        data: evtHot.Handson.ListaExcelData,
        maxCols: evtHot.Handson.ListaExcelData[0].length,
        colHeaders: false,
        rowHeaders: false,
        fillHandle: true,
        columnSorting: false,
        contextMenu: contexMenuInterruptores,
        className: "htCenter",
        readOnly: evtHot.Handson.ReadOnly,
        fixedRowsTop: 2,
        fixedColumnsLeft: evtHot.Handson.ColCabecera,
        colWidths: evtHot.Handson.ListaColWidth,
        afterLoadData: function () {
            this.render();
        },
        cells: function (row, col, prop) {
            var cellProperties = {};
            var formato = "";
            var render;
            var readOnly = true;
            var tipo;

            if (row == 0 || row == 1) {
                render = firstRowRenderer;
                readOnly = true;
            }
            if (row >= evtHot.Handson.FilasCabecera + 1 && row <= evtHot.Handson.ListaExcelData.length) {
                render = descripRowRenderer2;
                readOnly = false;
            }

            cellProperties = {
                renderer: render,
                format: formato,
                type: tipo,
                readOnly: readOnly
            }

            return cellProperties;
        },
        mergeCells: [
            { row: 0, col: 0, rowspan: 2, colspan: 1 },
            { row: 0, col: 1, rowspan: 2, colspan: 1 },
            { row: 0, col: 2, rowspan: 2, colspan: 1 },
            { row: 0, col: 3, rowspan: 1, colspan: 3 },
            { row: 0, col: 6, rowspan: 1, colspan: 3 }
        ]
    };

    hotInterruptores = new Handsontable(containerInterruptores, hotOptions);
}

var containerDescargadores
function crearHandsonTableDescargadores(evtHot) {

    function firstRowRenderer(instance, td, row, col, prop, value, cellProperties) {
        Handsontable.renderers.TextRenderer.apply(this, arguments);
        td.style.fontWeight = 'bold';
        td.style.fontSize = '10px';
        td.style.color = 'White';
        td.style.background = '#2980B9';
        td.style.textAlign = 'center';
    }

    function descripRowRenderer2(instance, td, row, col, prop, value, cellProperties) {
        Handsontable.renderers.TextRenderer.apply(this, arguments);
        td.style.fontSize = '10px';
        td.style.color = 'MidnightBlue';
        //td.style.background = '#E8F6FF';
        td.style.textAlign = 'center';
    }

    function negativeValueRenderer(instance, td, row, col, prop, value, cellProperties) {
        Handsontable.renderers.TextRenderer.apply(this, arguments);

        if (parseInt(value, 10) < 0) {
            td.style.color = 'orange';
            td.style.fontStyle = 'italic';
        }
    }

    function calculateSize() {
        var offset = Handsontable.Dom.offset(containerDescargadores);
        if (offset.top == 222) {
            availableHeight = $(window).height() - 140 - offset.top - 20;
        }
        else {
            availableHeight = $(window).height() - 140 - offset.top - 20;
        }

        if (offset.left > 0)
            availableWidth = $(window).width() - 2 * offset.left;
        if (offset.top > 0) {
            availableHeight = availableHeight < HEIGHT_MINIMO ? HEIGHT_MINIMO : availableHeight;
        }
    }
    var contexMenuDescargadores = {
        items: {
            'row_below': { name: "Insertar fila abajo" },
            'remove_row': { name: "Eliminar fila" }
        }
    };

    containerDescargadores = document.getElementById('DescargadoresTable');
    Handsontable.renderers.registerRenderer('negativeValueRenderer', negativeValueRenderer);

    hotOptions = {
        data: evtHot.Handson.ListaExcelData,
        maxCols: evtHot.Handson.ListaExcelData[0].length,
        colHeaders: false,
        rowHeaders: false,
        fillHandle: true,
        columnSorting: false,
        contextMenu: contexMenuDescargadores,
        className: "htCenter",
        readOnly: evtHot.Handson.ReadOnly,
        fixedRowsTop: 2,
        fixedColumnsLeft: evtHot.Handson.ColCabecera,
        colWidths: evtHot.Handson.ListaColWidth,
        afterLoadData: function () {
            this.render();
        },
        cells: function (row, col, prop) {
            var cellProperties = {};
            var formato = "";
            var render;
            var readOnly = true;
            var tipo;

            if (row == 0|| row == 1) {
                render = firstRowRenderer;
                readOnly = true;
            }

            if (row >= evtHot.Handson.FilasCabecera + 1 && row <= evtHot.Handson.ListaExcelData.length) {
                render = descripRowRenderer2;
                readOnly = false;
            }
            
            cellProperties = {
                renderer: render,
                format: formato,
                type: tipo,
                readOnly: readOnly
            }

            return cellProperties;
        },
        mergeCells: [
            { row: 0, col: 0, rowspan: 2, colspan: 1 },
            { row: 0, col: 1, rowspan: 2, colspan: 1 },
            { row: 0, col: 2, rowspan: 2, colspan: 1 },
            { row: 0, col: 3, rowspan: 1, colspan: 3 },
            { row: 0, col: 6, rowspan: 1, colspan: 3 }
        ]
    };

    hotDescargadores = new Handsontable(containerDescargadores, hotOptions);

}

ListarScada = function (Tipo) {
    
    let canalCodi;
    let controlador = siteRoot + 'Eventos/AnalisisFallas/';
    if (Tipo == "L")
        canalCodi = $('#cbCanal').val();
    else if (Tipo == "C")
        canalCodi = $('#cbCanalInvolucrados').val();
    else if (Tipo == "T")
        canalCodi = $('#cbCanalTransformadores').val();

    var evencodi = $("#hfEvencodicheck").val();

    $.ajax({
        type: 'POST',
        url: controlador + "ListadoScada",
        traditional: true,
        data: { listCanalCodi: canalCodi, evencodi: evencodi },
        success: function (evt) {
            $('#contenidoScada').html(evt);
            $('#popupScada').bPopup({
                follow: [true, true],
                position: ['auto', 'auto'],
                positionStyle: 'fixed',
            });
        },
        error: function () {
        }
    });
}

InsertarCondicionPrevia = function (tipo, tipoProceso) {
    
    var zona;
    var fecha = "";
    var canal = "";
    var valor = "";
    var dataExcel;
    var canalScada = "";
    var cantScada;
    if (tipo == "L") {
        dataExcel = hotLinea.getData();
        zona = $('#cbZona').val();
    }
    else if (tipo == "C") {
        dataExcel = hotCentral.getData();
        zona = $('#cbZonaInvolucrados').val();
    }
    else if (tipo == "T") {
        dataExcel = hotTransformadores.getData();       
        zona = $('#cbZonaTransformadores').val();
    }
        
    var evencodi = $("#hfEvencodicheck").val();

    if (tipoProceso == 1) 
        cantScada = $("#hfCantScada").val();
    else if (tipoProceso == 2)
        cantScada = 1;

    for (var x = 1; x <= cantScada; x++) {
         
        var nomcheck = 'chkScada' + x;
        var ValoresScada = document.querySelectorAll('.' + nomcheck);
        
        for (var k = 0; k < ValoresScada.length; k++) {
            if (ValoresScada[k].checked == true) {
                var parametros = ValoresScada[k].value.split('|');
                fecha = parametros[1];
                canal = parametros[2];
                valor = parametros[3];
                canalScada = parametros[4];
            }          
        }

        $.ajax({
            type: 'POST',
            url: controlador + "InsertarCondicionPrevia",
            contentType: 'application/json',
            dataType: 'json',
            data: JSON.stringify({
                dataExcel: dataExcel,
                tipo: tipo,
                zona: zona,
                evencodi: evencodi,
                fecha: fecha,
                canal: canal,
                valor: valor,
                tipoProceso: tipoProceso,
                canalScada: canalScada
            }),
            success: function (resultado) {
                console.log('Ingreso Success CondPrevia:' + resultado);
                if (resultado.result == 1) {
                    if (tipo == "L" && tipoProceso == 1) {
                        ListadoCondPreviaLinea();                        
                        var bPopup = $('#popupScada').bPopup();
                        bPopup.close();                                                
                    } else if (tipo == "C" && tipoProceso == 1) {
                        ListadoCondPreviaCentral();                       
                        var bPopup = $('#popupScada').bPopup();
                        bPopup.close();                        
                    } else if (tipo == "T" && tipoProceso == 1) {
                        ListadoCondPreviaTransformadores();
                        var bPopup = $('#popupScada').bPopup();
                        bPopup.close();
                    }

                    if (tipo == "L" && tipoProceso == 2) {
                        ListadoCondPreviaLinea();
                        InsertarCondicionPrevia('C', 2); //Inserta Condición Previa Central    
                    }
                    if (tipo == "C" && tipoProceso == 2) {
                        ListadoCondPreviaCentral();
                        InsertarCondicionPrevia('T', 2); //Inserta Condición Previa Transformadores    
                    }
                    if (tipo == "T" && tipoProceso == 2) {
                        ListadoCondPreviaTransformadores();
                        InsertarHistScadaAnalisisEvento(2); //Inserta Histórico Scada                   
                    }
                }
                else {
                    alert("Ocurrión un error al insertar Condiciones Previas");
                }
            },
            error: function () {
                alert("Ocurrión un error al insertar Condiciones Previas");
            }
        });
    }

    
}

var containerHistoricoScadaAnalisis
function crearHandsonTableHistoricoScadaAnalisis(evtHot) {

    function firstRowRenderer(instance, td, row, col, prop, value, cellProperties) {
        Handsontable.renderers.TextRenderer.apply(this, arguments);
        td.style.fontWeight = 'bold';
        td.style.fontSize = '10px';
        td.style.color = 'White';
        td.style.background = '#2980B9';
        td.style.textAlign = 'center';
    }

    function descripRowRenderer2(instance, td, row, col, prop, value, cellProperties) {
        Handsontable.renderers.TextRenderer.apply(this, arguments);
        td.style.fontSize = '10px';
        td.style.color = 'MidnightBlue';
        //td.style.background = '#FFFFFF';
        td.style.textAlign = 'center';

        if (row > 0 && col == 4) {
            const regex = /^(0?[1-9]|[12][0-9]|3[01])\/(0?[1-9]|1[0-2])\/\d{4} (0?[0-9]|1[0-9]|2[0-3]):([0-5][0-9]):([0-5][0-9].([0-9]{3}))$/;
            const isValidTime = regex.test(value);

            if (!isValidTime) {
                evtHot.Handson.ListaExcelData[row][col] = "";
                $(td).html("");
            }

        }
    }

    function negativeValueRenderer(instance, td, row, col, prop, value, cellProperties) {
        Handsontable.renderers.TextRenderer.apply(this, arguments);

        if (parseInt(value, 10) < 0) {
            td.style.color = 'orange';
            td.style.fontStyle = 'italic';
        }
    }
    var contexMenuHistoricoScada = {
        items: {
            'row_below': { name: "Insertar fila abajo" },
            'remove_row': { name: "Eliminar fila" }
        }
    };
    containerHistoricoScadaAnalisis = document.getElementById('HistoricoScadaAnalisisTable');
    //Handsontable.renderers.registerRenderer('negativeValueRenderer', negativeValueRenderer);

    hotOptions = {
        data: evtHot.Handson.ListaExcelData,
        maxCols: evtHot.Handson.ListaExcelData[0].length,
        height: 180,
        colHeaders: false,
        rowHeaders: false,
        fillHandle: true,
        columnSorting: false,
        contextMenu: contexMenuHistoricoScada,
        className: "htCenter",
        readOnly: evtHot.Handson.ReadOnly,
        fixedRowsTop: 1,
        fixedColumnsLeft: evtHot.Handson.ColCabecera,
        mergeCells: evtHot.Handson.ListaMerge,
        colWidths: evtHot.Handson.ListaColWidth,
        hiddenColumns: {
            columns: [0],
            indicators: true,
        },
        cells: function (row, col, prop) {
            var cellProperties = {};
            var formato = "";
            var render;
            var readOnly = true;
            var tipo;

            if (row == 0) {
                render = firstRowRenderer;
                readOnly = true;
            }
            if (row >= evtHot.Handson.FilasCabecera && row <= evtHot.Handson.ListaExcelData.length) {
                render = descripRowRenderer2;
                readOnly = false;
            }

            cellProperties = {
                renderer: render,
                format: formato,
                type: tipo,
                readOnly: readOnly
            }

            return cellProperties;
        },
        afterLoadData: function () {
            this.render();
        },
    };

    hotHistoricoScadaAnalisis = new Handsontable(containerHistoricoScadaAnalisis, hotOptions);
    hotHistoricoScadaAnalisis.getPlugin('hiddenColumns').hideColumns([0]);
    hotHistoricoScadaAnalisis.render();
}

function ListadoHistoricoScadaAnalisis() {
    if (!!hotHistoricoScadaAnalisis && hotHistoricoScadaAnalisis != null) {
        hotHistoricoScadaAnalisis.destroy();
    }
    var controlador = siteRoot + 'Eventos/AnalisisFallas/';
    var evencodi = $("#hfEvencodicheck").val();
    $.ajax({
        type: 'POST',
        url: controlador + 'MostrarHojaExcelWeb',
        data: { evencodi: evencodi, tipo: 4 },
        success: function (resultado) {
            
            $("#HistoricoScadaAnalisisTable").html('');
            
            if (resultado.Resultado != '-1') {
                crearHandsonTableHistoricoScadaAnalisis(resultado);
            }
        },
        error: function (err) {
            alert("Ingresa al error " + err);
        }
    });
}

const InsertarHistScadaAnalisisEvento = async (tipo) => {

    var controlador = siteRoot + 'Eventos/AnalisisFallas/';
    var evencodi = $("#hfEvencodicheck").val();
    var dataScadaAnalisis = hotHistoricoScadaAnalisis.getData();
    var tempUrl = controlador + 'InsertarHistScadaAnalisisEvento';

    try {

        const requestOptions = {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json',
            },
            body: JSON.stringify({
                dataExcel: dataScadaAnalisis,
                evencodi: evencodi,
                listCanalCodi: $('#cbCanalAnalisis').val(),
                fechaHoraIni: $('#txtFechaDesdeAnalisis').val() + ' ' + $('#txtHoraIniAnalisis').val(),
                fechaHoraFin: $('#txtFechaHastaAnalisis').val() + ' ' + $('#txtHoraFinAnalisis').val(),
                filtro: $('#cbCalidad').val(),
                tipo: tipo
            }),
        };

        const response = await fetch(tempUrl, requestOptions);

        if (!response.ok) {
            throw new Error('Error en la solicitud');
        }

        const data = await response.json();

        if (data.result == '1') {

            ListadoHistoricoScadaAnalisis();

            if (tipo == 2) {
                await SubirFileAnalisis();
            }

        }

        if (data.result == '2') {
            alert(data.responseText);
        }

    } catch (error) {
        console.error('Error al recuperar los datos:', error);
    }

}

function llenarComboNumeral() {
    $.ajax({
        type: 'POST',
        url: controlador + "ListaTipoNumeral",
        dataType: 'json',
        cache: false,
        success: function (evt) {
            
            var _len = evt.length;
            comboBoxNumeral = '<select name="selectNumeral" id="btnSelectNumeral{0}" style="width: 290px;">';
            for (i = 0; i < _len; i++) {
                post = evt[i];
                if (i === 0) {
                    comboBoxNumeroDefaultValue = post.EVETIPNUMCODI;
                    comboBoxNumeral = comboBoxNumeral + '<option selected="selected" value="' + post.EVETIPNUMCODI + '">' + post.EVETIPNUMDESCRIPCION + '</option>';
                } else {
                    comboBoxNumeral = comboBoxNumeral + '<option value="' + post.EVETIPNUMCODI + '">' + post.EVETIPNUMDESCRIPCION + '</option>';
                }
                
            }
            comboBoxNumeral = comboBoxNumeral + '</select >';

        },
        error: function (xhr, textStatus, exceptionThrown) {

            mostrarError();
        }
    });
}

var containerAnalisisEvento
function crearHandsonTableAnalisisEvento(evtHot) {
            
    function firstRowRenderer(instance, td, row, col, prop, value, cellProperties) {
        Handsontable.renderers.TextRenderer.apply(this, arguments);
        td.style.fontWeight = 'bold';
        td.style.fontSize = '10px';
        td.style.color = 'White';
        td.style.background = '#2980B9';
        td.style.textAlign = 'center';
    }

    function descripRowRenderer1(instance, td, row, col, prop, value, cellProperties) {
        Handsontable.renderers.TextRenderer.apply(this, arguments);
        td.style.fontSize = '10px';
        td.style.color = 'MidnightBlue';
        td.style.textAlign = 'left';
    }

    function descripRowRenderer2(instance, td, row, col, prop, value, cellProperties) {
        Handsontable.renderers.TextRenderer.apply(this, arguments);
        td.style.fontSize = '10px';
        td.style.color = 'MidnightBlue';
        td.style.textAlign = 'center';
    }

    function generarUUID() {
        return 'xxxxxxxx-xxxx-4xxx-yxxx-xxxxxxxxxxxx'.replace(/[xy]/g, function (c) {
            var r = Math.random() * 16 | 0,
                v = c === 'x' ? r : (r & 0x3 | 0x8);
            return v.toString(16);
        });
    }

    function LinkRowRender(instance, td, row, col, prop, value, cellProperties) {
        Handsontable.renderers.TextRenderer.apply(this, arguments);
        td.style.textAlign = 'center';

        var valor = evtHot.Handson.ListaExcelData[row][3];

        if (valor === undefined || valor === '') {
            valor = generarUUID();
            evtHot.Handson.ListaExcelData[row][3] = valor;
        }
        
        var seleccionador = '';
        seleccionador = '<input type="file" id="fileAnalisis_' + valor + '"accept="image/png, image/jpeg" />';
        td.innerHTML = seleccionador;
    }

    function DescargarImagenRowRender(instance, td, row, col, prop, value, cellProperties) {
        
        Handsontable.renderers.TextRenderer.apply(this, arguments);
        td.style.textAlign = 'center';
        var nombre = '';
        var seleccionador = '';
        var id = evtHot.Handson.ListaExcelData[row][3];
        if (evtHot.Handson.ListaExcelData[row][2] != null) {
            nombre = evtHot.Handson.ListaExcelData[row][2];
            seleccionador = '<a href="JavaScript:VerImagenAnalisis(' + id + ')">' + nombre + '</a >';
            td.innerHTML = seleccionador;
        }        
    }

    function llenarComboListaNumeral(row, col, prop, evtHot) {

        if (evtHot.Handson.ListaExcelData[row][4] != null && evtHot.Handson.ListaExcelData[row][4].split('|')[0] == null) {

            var eveanacoodi = evtHot.Handson.ListaExcelData[row][4].split('|')[1];

            if (evtHot.Handson.ListaExcelData[row][4].split('|')[0] == 0) {

                var id = "btnSelectNumeral" + row;
                var comboSeleccionadoNumeral = $("#" + id).val();

                if (row != 0) {
                    evtHot.Handson.ListaExcelData[row][4] = comboSeleccionadoNumeral + '|' + eveanacoodi;
                }
            }
        }
        indexListaAnalisisEvento = indexListaAnalisisEvento + 1;
    }

    function CboComboBoxNumeral(instance, td, row, col, prop, value, cellProperties) {      
        Handsontable.renderers.TextRenderer.apply(this, arguments);
        var _comboBoxNumeral = comboBoxNumeral.replace('{0}', row);
        var id = "btnSelectNumeral" + row;
        var comboSeleccionadoNumeral = evtHot.Handson.ListaExcelData[row][4] == null ? 0 : evtHot.Handson.ListaExcelData[row][4].split('|')[0];
        td.innerHTML = _comboBoxNumeral;
        if (comboSeleccionadoNumeral != 0) {
            $("#" + id).val(comboSeleccionadoNumeral);
        } else {
            $("#" + id).val(comboBoxNumeroDefaultValue);
        }
    }

    function UpdateComboNumeral(row, col, prop, evtHot) {
        $("#btnSelectNumeral" + row).change(function () {

            var valorCombo = $("#btnSelectNumeral" + row).val();

            if (valorCombo != undefined) {
                var eveanacoodi = evtHot.Handson.ListaExcelData[row][4].split('|')[1];
                var evencodi = evtHot.Handson.ListaExcelData[row][4].split('|')[2];
                evtHot.Handson.ListaExcelData[row][4] = valorCombo + '|' + eveanacoodi + '|' + evencodi;
            }

        });
    }

    function UpdateEvencodi(row, col, evtHot) {

        var evencodi = evtHot.Evencodi;

        if (evtHot.Handson.ListaExcelData[row][4] == null) {
            evtHot.Handson.ListaExcelData[row][0] = '';
            evtHot.Handson.ListaExcelData[row][1] = '';
            evtHot.Handson.ListaExcelData[row][2] = '';
            evtHot.Handson.ListaExcelData[row][3] = '';
            evtHot.Handson.ListaExcelData[row][4] = '|' + 'undefined|' + evencodi;
        }
            
        var eveanacoodi = evtHot.Handson.ListaExcelData[row][4].split('|')[0];
        var evenumcodi = evtHot.Handson.ListaExcelData[row][4].split('|')[0];
        var eveanacoodi = evtHot.Handson.ListaExcelData[row][4].split('|')[1];

        if (evenumcodi === '') {
            evtHot.Handson.ListaExcelData[row][4] = comboBoxNumeroDefaultValue + '|' + eveanacoodi + '|' + evencodi;
        } else {
            evtHot.Handson.ListaExcelData[row][4] = evenumcodi + '|' + eveanacoodi + '|' + evencodi;
        }
        
    }

    var contexMenuAnalisisEvento = {
        items: {
            'row_above': { name: "Insertar fila arriba" },
            'row_below': { name: "Insertar fila abajo" },
            'remove_row': { name: "Eliminar fila" }
        }
    };

    containerAnalisisEvento = document.getElementById('AnalisisEventoTable');

    hotOptions = {
        data: evtHot.Handson.ListaExcelData,
        maxCols: evtHot.Handson.ListaExcelData[0].length,
        colHeaders: false,
        rowHeaders: false,
        fillHandle: true,
        columnSorting: false,
        contextMenu: contexMenuAnalisisEvento,
        className: "htCenter",
        readOnly: evtHot.Handson.ReadOnly,
        fixedRowsTop: 1,
        fixedColumnsLeft: evtHot.Handson.ColCabecera,
        mergeCells: evtHot.Handson.ListaMerge,
        colWidths: evtHot.Handson.ListaColWidth,
        
        cells: function (row, col, prop) {
            var cellProperties = {};
            var formato = "";
            var render;
            var readOnly = true;
            var tipo;

            if (row == 0) {
                render = firstRowRenderer;
                readOnly = true;
            }
            if (row >= evtHot.Handson.FilasCabecera && row <= evtHot.Handson.ListaExcelData.length) {
                llenarComboListaNumeral(row, col, prop, evtHot);
                UpdateEvencodi(row, col, evtHot);
                if (col == 4) {
                    render = CboComboBoxNumeral;
                    UpdateComboNumeral(row, col, prop, evtHot);
                    readOnly = true;
                    formato = '';
                    tipo = 'dropdown';
                } else if (col == 3) {
                    render = LinkRowRender;
                } else if (col == 2) {
                    render = DescargarImagenRowRender;
                    readOnly = true;
                } else {
                    render = descripRowRenderer1;
                    readOnly = false;
                }
            }

            cellProperties = {
                renderer: render,
                format: formato,
                type: tipo,
                readOnly: readOnly
            }

            return cellProperties;
        },
        afterLoadData: function () {
            this.render();
        },
    };

    hotAnalisisEvento = new Handsontable(containerAnalisisEvento, hotOptions);
    //hotAnalisisEvento.getPlugin('hiddenColumns').hideColumns([0]);
    hotAnalisisEvento.render();
    console.log('En hansontable: ');
    console.log(hotAnalisisEvento);
}

function ListadoAnalisisEvento() {
    
    if (!!hotAnalisisEvento && hotAnalisisEvento != null) {
        console.log('Valor de ListadoAnalisisEvento: ');
        console.log(hotAnalisisEvento.getData());
        hotAnalisisEvento.destroy();
    }
    var controlador = siteRoot + 'Eventos/AnalisisFallas/';
    var evencodi = $("#hfEvencodicheck").val();
    $.ajax({
        type: 'POST',
        url: controlador + 'MostrarHojaExcelWeb',
        data: { evencodi: evencodi, tipo: 5 },
        success: function (resultado) {
            $("#AnalisisEventoTable").html('');
            if (resultado.Resultado != '-1') {
                crearHandsonTableAnalisisEvento(resultado);
            }
        },
        error: function (err) {
            alert("Ingresa al error " + err);
        }
    });
}

function InsertarAnalisisEvento() {
    
    var controlador = siteRoot + 'Eventos/AnalisisFallas/';
    var evencodi = $("#hfEvencodicheck").val();
    var dataScadaAnalisis = hotHistoricoScadaAnalisis.getData();

    //debugger;

    $.ajax({
        type: 'POST',
        url: controlador + 'InsertarAnalisisEvento',
        contentType: 'application/json',
        dataType: 'json',
        traditional: true,
        data: JSON.stringify({
            dataExcel: dataScadaAnalisis,
            evencodi: evencodi,
            listCanalCodi: $('#cbCanalAnalisis').val(),
            fechaHoraIni: $('#txtFechaDesdeAnalisis').val() + ' ' + $('#txtHoraIniAnalisis').val(),
            fechaHoraFin: $('#txtFechaHastaAnalisis').val() + ' ' + $('#txtHoraFinAnalisis').val(),
            filtro: $('#cbCalidad').val()
        }),
        //beforeSend: function () {
        //    $(".modal-loading-background").css("visibility", "visible");
        //},
        success: function (resultado) {
            if (resultado.result != '-1') {
                alert('Análisis de Evento generado');
                ListadoAnalisisEvento();
            }
        },
        error: function (err) {
            alert("Ingresa al error " + err);
        }//,
        //complete: function () {
        //    $(".modal-loading-background").css("visibility", "hidden");
        //}
    });
}

InsertarRecomendacionObservacion = function (tipo) {
    
    var txtDescripcion;
    var codigoEmpresa;
    if (tipo == 1) {
        txtDescripcion = $("#txtRecomendacionObs").val();
        codigoEmpresa = $('#hfIdEmpresaRecomendacion').val();
    }
    else if (tipo == 2) {
        txtDescripcion = $("#txtObservacionobs").val();
        codigoEmpresa = $('#hfIdEmpresaObservacion').val();
    }
    
    var evencodi = $("#hfEvencodicheck").val();
    
    var Mensaje = "";
    if (codigoEmpresa == "") {
        Mensaje += "<li>Por favor busque una empresa</li>";
    }

    if (txtDescripcion == "") {
        Mensaje += "<li>Por favor ingrese una recomendacion</li>";
    }


    if (Mensaje != "") {
        $("#ulMensajeObs").empty();
        $("#ulMensajeObs").append(Mensaje);

        $('#divMensajeObs').bPopup({
            follow: [true, true],
            position: ['auto', 'auto'],
            positionStyle: 'fixed',
        });
        return;
    }

    $.ajax({
        type: 'POST',
        url: controlador + "InsertarRecomendacionObservacion",
        dataType: 'json',
        data: { evencodi: evencodi, descripcion: txtDescripcion, codigoEmpresa: codigoEmpresa, tipo: tipo },
        success: function (result) {
            $("#txtEmpresaObs").val("");
            $("#txtRecomendacionObs").val("");
            if(tipo == 1)
                TraerRecomendacionObservacion(1);
            else if (tipo == 2)
                TraerRecomendacionObservacion(2);
        }
    });

}

TraerRecomendacionObservacion = function (tipo) {
    
    var controlador = siteRoot + 'Eventos/AnalisisFallas/';
    var evencodi = $("#hfEvencodicheck").val();
    $.ajax({
        type: 'POST',
        url: controlador + "TraerRecomendacionObservacion",
        dataType: 'json',
        data: { evencodi: evencodi, tipo: tipo},
        success: function (json) {
            var html = "";
            if (json != "") {
                var njson = json.length;
                
                for (var i = 0; i < njson; i++) {
                    html += "<tr>";
                    if(tipo == 1)
                        html += "<td style='border: hidden'><a href='#' onclick='EliminarObservacionRecomendacion(" + json[i].EVERECOMOBSERVCODI + "," + tipo + ")' id='btnEliminarReco'><img src='" + siteRoot + "Content/Images/btn-cancel.png' style='margin-top: 2px;'/></a></td>";
                    else if(tipo == 2)
                        html += "<td style='border: hidden'><a href='#' onclick='EliminarObservacionRecomendacion(" + json[i].EVERECOMOBSERVCODI + "," + tipo + ")' id='btnEliminarObs'><img src='" + siteRoot + "Content/Images/btn-cancel.png' style='margin-top: 2px;'/></a></td>";

                    html += "<td>" + json[i].EMPRNOMB + "</td>";
                    html += "<td>" + json[i].EVERECOMOBSERVDESC + "</td>";
                    html += "<td>" + json[i].LASTUSER + "</td>";
                    html += "<td>" + json[i].LASTDATEstr + "</td>";
                    html += "</tr>";
                }
                if (tipo == 1) {
                    $("#tbodyRecomendacionObs").empty();
                    $("#tbodyRecomendacionObs").append(html);
                }
                else if (tipo == 2) {
                    $("#tbodyObservacionObs").empty();
                    $("#tbodyObservacionObs").append(html);
                }

            } else {
                if (tipo == 1)
                    $("#tbodyRecomendacionObs").empty();
                else if (tipo == 2)
                    $("#tbodyObservacionObs").empty();
            }
        }
    });
}

EliminarObservacionRecomendacion = function (everecomobservcodi, tipo) {
     
    if (confirm("Esta seguro que desea eliminar el registro?")) {
        var controlador = siteRoot + 'Eventos/AnalisisFallas/';

        $.ajax({
            type: 'POST',
            url: controlador + "EliminarObservacionRecomendacion",
            dataType: 'json',
            data: { everecomobservcodi: everecomobservcodi },
            success: function (resultado) {
                if (resultado.result != '-1') {
                    alert('Se eliminó correctamente');
                    if (tipo == 1)
                        TraerRecomendacionObservacion(tipo);
                    else if (tipo == 2)
                        TraerRecomendacionObservacion(tipo);
                }
            }
        });
    }

}



function ListadoCondPreviaLinea() {

    if (!!hotLinea && hotLinea != null) {
        hotLinea.destroy();
    }
    var controlador = siteRoot + 'Eventos/AnalisisFallas/';
    var evencodi = $("#hfEvencodicheck").val();
    $.ajax({
        type: 'POST',
        url: controlador + 'MostrarHojaExcelWeb',
        data: { evencodi: evencodi, tipo: 1 },
        success: function (resultadoLinea) {
            if (resultadoLinea.Resultado != '-1') {
                $("#CondicionesPreviasLinea").html('');
                crearHandsonTableLinea(resultadoLinea);
            }
        },
        error: function (err) {
            alert("Ingresa al error " + err);
        }
    });
}

var containerLinea
function crearHandsonTableLinea(evtHot) {

    function firstRowRenderer(instance, td, row, col, prop, value, cellProperties) {
        Handsontable.renderers.TextRenderer.apply(this, arguments);
        td.style.fontWeight = 'bold';
        td.style.fontSize = '11px';
        td.style.color = 'White';
        td.style.background = '#2980B9';
        td.style.textAlign = 'center';
    }

    function descripRowRenderer2(instance, td, row, col, prop, value, cellProperties) {
        Handsontable.renderers.TextRenderer.apply(this, arguments);
        td.style.fontSize = '11px';
        td.style.color = 'MidnightBlue';
        td.style.textAlign = 'center';
    }

    function descripRowRenderer3(instance, td, row, col, prop, value, cellProperties) {
        debugger;
        Handsontable.renderers.TextRenderer.apply(this, arguments);
        td.style.fontSize = '11px';
        td.style.color = 'MidnightBlue';
        td.style.textAlign = 'center';
        if (row > 1 && col >= 5) {
            //const regex = /^[0-9]*(\.[0-9]+)?$/;
            //const isValidTime = regex.test(value);

            //if (!isValidTime) {
            //    evtHot.Handson.ListaExcelData[row][col] = "";
            //    $(td).html("");
            //}
            if (value != null && value.length > 20) {
                evtHot.Handson.ListaExcelData[row][col] = "";
                $(td).html("");
            }
        }
    }

    function negativeValueRenderer(instance, td, row, col, prop, value, cellProperties) {
        Handsontable.renderers.TextRenderer.apply(this, arguments);

        if (parseInt(value, 10) < 0) {
            td.style.color = 'orange';
            td.style.fontStyle = 'italic';
        }
    }

    function readOnlyValueRenderer(instance, td, row, col, prop, value, cellProperties) {
        Handsontable.renderers.TextRenderer.apply(this, arguments);
        td.style.fontSize = '11px';
        td.style.textAlign = 'center';
        td.style.color = 'MidnightBlue';
    }

    function calculateSize() {
        var offset = Handsontable.Dom.offset(containerLinea);

        //Verificar en que ventana esta, expandida -> 2 o extranet normal -> 1
        if (offset.top == 222) {
            availableHeight = $(window).height() - 140 - offset.top - 20;
        }
        else {
            availableHeight = $(window).height() - 140 - offset.top - 20;
        }

        if (offset.left > 0)
            availableWidth = $(window).width() - 2 * offset.left;
        if (offset.top > 0) {
            availableHeight = availableHeight < HEIGHT_MINIMO ? HEIGHT_MINIMO : availableHeight;
            container.style.height = availableHeight + 'px';
        }
    }
    var contexMenuCPLinea = {
        items: {
            'row_below': { name: "Insertar fila abajo" },
            'remove_row': { name: "Eliminar fila" }
        }
    };

    containerLinea = document.getElementById('CondicionesPreviasLinea');

    hotOptions = {
        data: evtHot.Handson.ListaExcelData,
        height: 180,
        //maxRows: evtHot.Handson.ListaExcelData.length,
        maxCols: evtHot.Handson.ListaExcelData[0].length,
        colHeaders: false,
        rowHeaders: false,
        fillHandle: true,
        contextMenu: contexMenuCPLinea,
        columnSorting: false,
        className: "htCenter",
        readOnly: evtHot.Handson.ReadOnly,
        fixedRowsTop: 2,
        fixedColumnsLeft: evtHot.Handson.ColCabecera,
        mergeCells: evtHot.Handson.ListaMerge,
        colWidths: evtHot.Handson.ListaColWidth,
        hiddenColumns: {
            columns: [0],
            indicators: true,
        },       
        cells: function (row, col, prop) {
            var cellProperties = {};
            var formato = "";
            var render;
            var readOnly = true;
            var tipo;

            
            if (row == 0 || row == 1) {
                render = firstRowRenderer;
                readOnly = true;
            }
            if (row > evtHot.Handson.FilasCabecera && row <= evtHot.Handson.ListaExcelData.length) {

                if (col == 1) 
                    render = readOnlyValueRenderer;
                if (col >= 5) {
                    render = descripRowRenderer3;
                    readOnly = false;
                }                 
                else {
                    render = descripRowRenderer2;
                    readOnly = false;
                }              
            }

            cellProperties = {
                renderer: render,
                format: formato,
                type: tipo,
                readOnly: readOnly
            }

            return cellProperties;
        },
        afterLoadData: function () {
            this.render();
        },
        mergeCells: [
            { row: 0, col: 1, rowspan: 2, colspan: 1 },
            { row: 0, col: 2, rowspan: 2, colspan: 1 },
            { row: 0, col: 3, rowspan: 1, colspan: 2 },
            { row: 1, col: 3, rowspan: 1, colspan: 1 },
            { row: 0, col: 5, rowspan: 2, colspan: 1 },
            { row: 0, col: 6, rowspan: 2, colspan: 1 },
        ]
    };

    hotLinea = new Handsontable(containerLinea, hotOptions);
    hotLinea.getPlugin('hiddenColumns').hideColumns([0]);
    hotLinea.render();
}

function ListadoCondPreviaCentral() {
    if (!!hotCentral && hotCentral != null) {
        hotCentral.destroy();
    }
    
    var controlador = siteRoot + 'Eventos/AnalisisFallas/';
    var evencodi = $("#hfEvencodicheck").val();
    $.ajax({
        type: 'POST',
        url: controlador + 'MostrarHojaExcelWeb',
        data: { evencodi: evencodi, tipo: 2 },
        success: function (resultadoLinea) {
            if (resultadoLinea.Resultado != '-1') {
                $("#CondicionesPreviasCentral").html('');
                crearHandsonTableCentral(resultadoLinea);
            }
        },
        error: function (err) {
            alert("Ingresa al error " + err);
        }
    });
}

var containerCentral
function crearHandsonTableCentral(evtHot) {

    function firstRowRenderer(instance, td, row, col, prop, value, cellProperties) {
        Handsontable.renderers.TextRenderer.apply(this, arguments);
        td.style.fontWeight = 'bold';
        td.style.fontSize = '11px';
        td.style.color = 'White';
        td.style.background = '#2980B9';
        td.style.textAlign = 'center';
    }

    function descripRowRenderer2(instance, td, row, col, prop, value, cellProperties) {
        Handsontable.renderers.TextRenderer.apply(this, arguments);
        td.style.fontSize = '11px';
        td.style.color = 'MidnightBlue';
        td.style.textAlign = 'center';
    }

    function descripRowRenderer3(instance, td, row, col, prop, value, cellProperties) {
        debugger;
        Handsontable.renderers.TextRenderer.apply(this, arguments);
        td.style.fontSize = '11px';
        td.style.color = 'MidnightBlue';
        td.style.textAlign = 'center';
        if (row > 1 && col >= 4) {
            if (value != null && value.length > 20) {
                evtHot.Handson.ListaExcelData[row][col] = "";
                $(td).html("");
            }
            //const regex = /^[0-9]*(\.[0-9]+)?$/;
            //const isValidTime = regex.test(value);

            //if (!isValidTime) {
            //    evtHot.Handson.ListaExcelData[row][col] = "";
            //    $(td).html("");
            //}
        }
    }

    function readOnlyValueRenderer(instance, td, row, col, prop, value, cellProperties) {
        Handsontable.renderers.TextRenderer.apply(this, arguments);
        td.style.fontSize = '11px';
        td.style.textAlign = 'center';
        td.style.color = 'MidnightBlue';
    }

    var contexMenuCPCentral = {
        items: {
            'row_below': { name: "Insertar fila abajo" },
            'remove_row': { name: "Eliminar fila" }
        }
    };

    containerCentral = document.getElementById('CondicionesPreviasCentral');

    hotOptions = {
        data: evtHot.Handson.ListaExcelData,
        maxCols: evtHot.Handson.ListaExcelData[0].length,
        height: 180,
        colHeaders: false,
        rowHeaders: false,
        fillHandle: true,
        contextMenu: contexMenuCPCentral,
        columnSorting: false,
        className: "htCenter",
        readOnly: evtHot.Handson.ReadOnly,
        fixedRowsTop: 2,
        fixedColumnsLeft: evtHot.Handson.ColCabecera,
        mergeCells: evtHot.Handson.ListaMerge,
        colWidths: evtHot.Handson.ListaColWidth,
        hiddenColumns: {
            columns: [0],
            indicators: true,
        },        
        cells: function (row, col, prop) {
            var cellProperties = {};
            var formato = "";
            var render;
            var readOnly = true;
            var tipo;

            if (row == 0 || row == 1) {
                render = firstRowRenderer;
                readOnly = true;
            }
            if (row > evtHot.Handson.FilasCabecera && row <= evtHot.Handson.ListaExcelData.length) {
                if (col == 1)
                    render = readOnlyValueRenderer;
                else if (col >= 4) {
                    render = descripRowRenderer3;
                    readOnly = false;
                }
                else {
                    render = descripRowRenderer2;
                    readOnly = false;
                }
                    
            }

            cellProperties = {
                renderer: render,
                format: formato,
                type: tipo,
                readOnly: readOnly
            }

            return cellProperties;
        },
        afterLoadData: function () {
            this.render();
        },
        mergeCells: [
            { row: 0, col: 1, rowspan: 2, colspan: 1 },
            { row: 0, col: 2, rowspan: 2, colspan: 1 },
            { row: 0, col: 3, rowspan: 2, colspan: 1 },
            { row: 0, col: 4, rowspan: 1, colspan: 2 },
        ],
    };

    hotCentral = new Handsontable(containerCentral, hotOptions);
    hotCentral.getPlugin('hiddenColumns').hideColumns([0]);
    hotCentral.render();
}

function ListadoCondPreviaTransformadores() {
    if (!!hotTransformadores && hotTransformadores != null) {
        hotTransformadores.destroy();
    }
    
    var controlador = siteRoot + 'Eventos/AnalisisFallas/';
    var evencodi = $("#hfEvencodicheck").val();
    $.ajax({
        type: 'POST',
        url: controlador + 'MostrarHojaExcelWeb',
        data: { evencodi: evencodi, tipo: 3 },
        success: function (resultadoLinea) {
            
            if (resultadoLinea.Resultado != '-1') {
                $("#CondicionesPreviasTransformadores").html('');
                crearHandsonTableTransformadores(resultadoLinea);
            }
        },
        error: function (err) {
            alert("Ingresa al error " + err);
        }
    });
}

var containerTransformadores
function crearHandsonTableTransformadores(evtHot) {

    function firstRowRenderer(instance, td, row, col, prop, value, cellProperties) {
        Handsontable.renderers.TextRenderer.apply(this, arguments);
        td.style.fontWeight = 'bold';
        td.style.fontSize = '11px';
        td.style.color = 'White';
        td.style.background = '#2980B9';
        td.style.textAlign = 'center';
    }

    function descripRowRenderer2(instance, td, row, col, prop, value, cellProperties) {
        Handsontable.renderers.TextRenderer.apply(this, arguments);
        td.style.fontSize = '11px';
        td.style.color = 'MidnightBlue';
        td.style.textAlign = 'center';
    }

    function descripRowRenderer3(instance, td, row, col, prop, value, cellProperties) {
        debugger;
        Handsontable.renderers.TextRenderer.apply(this, arguments);
        td.style.fontSize = '11px';
        td.style.color = 'MidnightBlue';
        td.style.textAlign = 'center';
        if (row > 0 && col == 4) {
            const regex = /^([0-9]{1,10}(\.[0-9]{1,5})?)$/;
            const isValidTime = regex.test(value);

            if (!isValidTime) {
                evtHot.Handson.ListaExcelData[row][col] = "";
                $(td).html("");
            }
            
        }
        if (row > 0 && col > 4) {
            if (value != null && value.length > 20) {
                evtHot.Handson.ListaExcelData[row][col] = "";
                $(td).html("");
            }
            //const regex = /^[0-9]*(\.[0-9]+)?$/;
            //const isValidTime = regex.test(value);

            //if (!isValidTime) {
            //    evtHot.Handson.ListaExcelData[row][col] = "";
            //    $(td).html("");
            //}
        }
    }

    function readOnlyValueRenderer(instance, td, row, col, prop, value, cellProperties) {
        Handsontable.renderers.TextRenderer.apply(this, arguments);
        td.style.fontSize = '11px';
        td.style.textAlign = 'center';
        td.style.color = 'MidnightBlue';
    }

    var contexMenuCPTransformadores = {
        items: {
            'row_below': {
                name: "Insertar fila abajo",
            },
            'remove_row': {
                name: "Eliminar fila"
            }
        }
    };
    containerTransformadores = document.getElementById('CondicionesPreviasTransformadores');

    hotOptions = {
        data: evtHot.Handson.ListaExcelData,
        maxCols: evtHot.Handson.ListaExcelData[0].length,
        height: 180,
        colHeaders: false,
        rowHeaders: false,
        fillHandle: true,
        contextMenu: contexMenuCPTransformadores,
        columnSorting: false,
        className: "htCenter",
        readOnly: evtHot.Handson.ReadOnly,
        fixedRowsTop: 1,
        fixedColumnsLeft: evtHot.Handson.ColCabecera,
        mergeCells: evtHot.Handson.ListaMerge,
        colWidths: evtHot.Handson.ListaColWidth,
        hiddenColumns: {
            columns: [0],
            indicators: true,
        },
        cells: function (row, col, prop) {
            var cellProperties = {};
            var formato = "";
            var render;
            var readOnly = true;
            var tipo;

            if (row == 0) {
                render = firstRowRenderer;
                readOnly = true;
            }
            if (row >= evtHot.Handson.FilasCabecera && row <= evtHot.Handson.ListaExcelData.length) {
                if (col == 1)
                    render = readOnlyValueRenderer;
                else if (col >= 4) {
                    render = descripRowRenderer3;
                    readOnly = false;
                }
                else {
                    render = descripRowRenderer2;
                    readOnly = false;
                }                         
            }

            cellProperties = {
                renderer: render,
                format: formato,
                type: tipo,
                readOnly: readOnly
            }

            return cellProperties;
        },
        afterLoadData: function () {
            this.render();
        }
    };

    hotTransformadores = new Handsontable(containerTransformadores, hotOptions);
    hotTransformadores.getPlugin('hiddenColumns').hideColumns([0]);
    hotTransformadores.render();
}

const SubirFileAnalisis = async () => {

    var dataExcel = hotAnalisisEvento.getData();

    for (var z = 1; z < dataExcel.length; z++) {

        if (dataExcel[z][4] !== '' && dataExcel[z][4] !== null) {

            if (dataExcel[z][4].split('|')[1] != "") {

                var id;

                if (dataExcel[z][4].split('|')[1] != 'undefined') {

                    id = "fileAnalisis_" + dataExcel[z][4].split('|')[1];

                    var Eveanaevecodi = dataExcel[z][4].split('|')[1];

                    if ($('#' + id)[0] != undefined) {

                        var Archivo = $('#' + id)[0].files[0];
                        var frm = new FormData();

                        if (Archivo != 'undefined') {

                            frm.append("Eveanaevecodi", Eveanaevecodi);
                            frm.append("Archivo", Archivo);

                            const tempSubirFileAnalisisUrl = controlador + "SubirFileAnalisis";

                            const requestOptions = {
                                method: 'POST',
                                body: frm,
                            };

                            const response = await fetch(tempSubirFileAnalisisUrl, requestOptions);

                            if (!response.ok) {
                                alert("Error al cargar archivo en Análisis de Evento");
                            }

                            //const responseData = await response.text();

                            //if (responseData !== 'True') {
                            //    alert("Error al cargar archivo en Análisis de Evento");
                            //}

                        }

                    }
                }                 
            }
        }

        //if (z == dataExcel.length - 1) {
        //    UpdateAnalisisEvento();
        //}
    }

    await UpdateAnalisisEvento();
    
}

const toBase64 = file => new Promise((resolve, reject) => {
    const reader = new FileReader();
    reader.readAsDataURL(file);
    reader.onload = () => resolve(reader.result);
    reader.onerror = reject;
});

const convertirABase64 = async (input,arrayImagenes,i) => {

    if (input.files && input.files[0]) {
        arrayImagenes.push({
            indice: i,
            imagenBase64: await toBase64(input.files[0]),
        });
    }

}

const UpdateAnalisisEvento = async () => {
    
    var controlador = siteRoot + 'Eventos/AnalisisFallas/';
    var dataAnalisisEvento = hotAnalisisEvento.getData();
    var arrayImagenes = [];

    var error = false;
    var fila = 0;

    if (dataAnalisisEvento.length > 1) {

        for (var i = 1; i < dataAnalisisEvento.length; i++) {

            const contenidoDescripcionNumeral = dataAnalisisEvento[i][0];

            if (contenidoDescripcionNumeral !== null) {

                console.log("CANTIDAD DE CARACTERES: ", contenidoDescripcionNumeral.length);

                if (contenidoDescripcionNumeral.length > 4000) {
                    error = true;
                    fila = i;
                }
            }

            var codigRegistro = dataAnalisisEvento[i][4].split('|')[1];

            var id = "fileAnalisis_" + codigRegistro;

            if (codigRegistro == "undefined") {
                id = 'fileAnalisis_' + dataAnalisisEvento[i][3];
            }

            if ($('#' + id)[0] != undefined) {
                await convertirABase64($('#' + id)[0], arrayImagenes, i);
            }

        }

        for (var i = 0; i < dataAnalisisEvento.length; i++) {
            
            var item = arrayImagenes.find(x => x.indice === i);

            if (item !== undefined) {
                dataAnalisisEvento[i].push(item.imagenBase64);
            } else {
                dataAnalisisEvento[i].push('');
            }

        }
    }

    if (error) {

        alert(`Análisis del Evento: La fila ${fila} campo 'Descripción del Numeral' tiene mas de 4000 caracteres.`);
        return;

    }

    debugger;

    $.ajax({
        type: 'POST',
        url: controlador + 'UpdateAnalisisEvento',
        contentType: 'application/json',
        dataType: 'json',
        traditional: true,
        data: JSON.stringify({
            dataExcel: dataAnalisisEvento
        }),
        success: function (resultado) {
            if (resultado.result != '-1') {
                //alert("Información análisis de eventos actualizada.");
                ListadoAnalisisEvento();
                GrabarSenializacionProteccion(); //Inserta Señalización de Protecciones
            }
        },
        error: function (err) {

            if (err.status === 400) {
                alert(err.statusText);
                return;
            }

            alert("Ingresa al error " + err);
        }
    });

}

function InsertarSecuenciaEvento() {

    var objetoSecuenciaEvento = [];
    var dataSecuenciaEvento = hotSecuenciaEvento.getData();
    var eventcodi = $("#hfEvencodicheck").val();

    for (var d = 1; d < dataSecuenciaEvento.length; d++) {
        console.log('Fila:' + d + ' Fecha: ' + dataSecuenciaEvento[d][0]);
        console.log('Fila:' + d + ' Hora: ' + dataSecuenciaEvento[d][1])
        if (dataSecuenciaEvento[d][0] != "") {
            for (var x = 1; x < dataSecuenciaEvento.length; x++) {
                if (dataSecuenciaEvento[x][1] != "" && dataSecuenciaEvento[x][0] == "") {
                    console.log(dataSecuenciaEvento[x][1]);
                    return alert('Debe ingresar todas las fechas.');
                }
                    
            }
            if (dataSecuenciaEvento[d][1] == "") {
                return alert('Debe ingresar hora.');
            }
        }

        objetoSecuenciaEvento.push({ EVENCODI: eventcodi, EVESECFECHA: dataSecuenciaEvento[d][0], EVESECHORA: dataSecuenciaEvento[d][1], EVESECSECC: dataSecuenciaEvento[d][2], EVESECDESCRIPCION: dataSecuenciaEvento[d][3], EVESECINCMANIOB: dataSecuenciaEvento[d][4] });
    }

    //for (var d = 1; d < dataSecuenciaEvento.length; d++) {
    //    var valorCombo = document.getElementById("btnSelectEmpresa" + d);
    //    var eventcodi = $("#hfEvencodicheck").val();

    //    if (valorCombo != null) {
    //        objetoSecuenciaEvento.push({ EVENCODI: eventcodi, EVESECFECHA: dataSecuenciaEvento[d][0], EVESECHORA: dataSecuenciaEvento[d][1], EVESECSECC: valorCombo.value, EVESECDESCRIPCION: dataSecuenciaEvento[d][3], EVESECINCMANIOB: dataSecuenciaEvento[d][4] });
    //    }
    //}

    $.ajax({
        type: 'POST',
        url: controlador + "ActualizarSecuenciaEvento",
        contentType: 'application/json',
        dataType: 'json',
        traditional: true,
        data: JSON.stringify(objetoSecuenciaEvento),
        success: function (resultado) {
            if (resultado == 1) {              
                InsertarCondicionPrevia('L', 2); //Inserta Condición Previa Línea                        
            }
        },
        error: function () {
            alert("Ocurrión un error al insertar Secuencias de Evento");
        }
    });

}

function GrabarSenializacionProteccion() {
    let controlador = siteRoot + 'Eventos/AnalisisFallas/';
    let codigoEvento = $("#hfEvencodicheck").val();

    if (typeof hotProteccion !== 'undefined') {

        let data = hotProteccion.getData();

        $.ajax({
            type: 'POST',
            url: controlador + 'GrabarSenializacionProteccion',
            dataType: 'json',
            contentType: 'application/json',
            data: JSON.stringify({
                dataExcel: data,
                codigoEvento: codigoEvento
            }),
            success: function (result) {
                if (result.Resultado == "1") {
                    InsertarInterruptoresDescargadores(); //Inserta Interruptores y Descargadores
                }
                else if (result.Resultado == "-1") {
                    //mostrarMensajeEval('mensaje', 'alert', result.StrMensaje);
                }
            },
            error: function () {
                alert("Ocurrión un error al insertar Señalización de Protecciones");
            }
        });
    }

}

InsertarInterruptoresDescargadores = function () {
    var dataInterruptores = hotInterruptores.getData();
    var dataDescargadores = hotDescargadores.getData();
    var evencodi = $("#hfEvencodicheck").val();
    $.ajax({
        type: 'POST',
        url: controlador + "InsertarInterruptoresDescargadores",
        contentType: 'application/json',
        dataType: 'json',
        traditional: true,
        data: JSON.stringify({
            dataExcelInterruptores: dataInterruptores,
            dataExcelDescargadores: dataDescargadores,
            evencodi: evencodi
        }),
        success: function (resultado) {
            if (resultado.result == 1) {
                ListadoInterruptores();
                ListadoDescargadores();
                alert('Los Datos de Evento SCO se actualizaron correctamente.');
            }
            else if (resultado.result != 1) {
                alert("Ocurrió un error al insertar Interruptores y Descargadores");
            }

        },
        error: function () {
        }
    });
}

function GrabarEtapasEventoSCO() {
    var evendesc = $("#txtDescripcionEvento").val();
    var evencodi = $("#hfEvencodicheck").val();
    ActualizarDesEventoAF(evencodi, evendesc);
    InsertarSecuenciaEvento();    
}

function CheckScada(checkbox) {
    
    var nomcheck = checkbox.className;
    var inputs = document.querySelectorAll('.' + nomcheck);
    
    for (var k = 0; k < inputs.length; k++) {
        if (inputs[k].id != checkbox.id)
            inputs[k].checked = false;
    }
}

function ActualizarDesEventoAF(evencodi, evendesc) {
    $.ajax({
        type: 'POST',
        url: controlador + 'ActualizarDesEventoAF',
        dataType: 'json',
        data: {
            evencodi: evencodi,
            evendescctaf: evendesc
        },
        success: function (evt) {
            if (evt.result != "1") {
                alert("Error actualizar Descripción evento Sco");
            }

        }
    });
}