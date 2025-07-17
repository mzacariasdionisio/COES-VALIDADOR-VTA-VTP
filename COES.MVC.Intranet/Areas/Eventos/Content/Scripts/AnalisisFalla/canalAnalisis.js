$(function () {
    $("#btnAgregarNumeral").click(function () {
        ListadoNumeral();
    });
    $("#btnGrabarNumeral").click(function () {
        InsertarTipoNumeral();
    });
});


function ListadoNumeral() {
    //debugger;
    var controlador = siteRoot + 'Eventos/AnalisisFallas/';
    var evencodi = $("#hfEvencodicheck").val();
    $.ajax({
        type: 'POST',
        url: controlador + 'ListadoNumeral',
        traditional: true,
        success: function (resultado) {
            //debugger;

            //console.log("Paso 3: " + resultado);
            ListadoNumeralHandson();
            //console.log("Paso 3: ListadoNumeralHandson");
            $("#contenidoNumeral").show();
            contadorSlim = 1;
            $("#contenidoNumeral").html(resultado);
            //console.log("Paso 1: contenidoNumeral");
            $('#popupNumeral').bPopup({
                follow: [true, true],
                position: ['auto', 'auto'],
                positionStyle: 'fixed',
            });
            //console.log("Paso 2: popupNumeral");

        },
        error: function (err) {

            alert("Ingresa al error " + err);
        }
    });
}

function ListadoNumeralHandson() {
    var controlador = siteRoot + 'Eventos/AnalisisFallas/';
    var evencodi = $("#hfEvencodicheck").val();
    $.ajax({
        type: 'POST',
        url: controlador + 'MostrarHojaExcelWeb',
        data: { evencodi: evencodi, tipo: 6 },
        success: function (resultado) {
            $("#NumeralTable").html('');
            if (resultado.Resultado != '-1') {
                console.log(resultado);
                crearHandsonTableNumeral(resultado);
            }
        },
        error: function (err) {
            alert("Ingresa al error " + err);
        }
    });
}

var containerNumeral
function crearHandsonTableNumeral(evtHot) {

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
        td.style.background = '#E8F6FF';
        td.style.textAlign = 'center';
    }

    function negativeValueRenderer(instance, td, row, col, prop, value, cellProperties) {
        Handsontable.renderers.TextRenderer.apply(this, arguments);

        if (parseInt(value, 10) < 0) {
            td.style.color = 'orange';
            td.style.fontStyle = 'italic';
        }
    }

    function LinkRowRender(instance, td, row, col, prop, value, cellProperties) {
        Handsontable.renderers.TextRenderer.apply(this, arguments);
        td.style.background = '#E8F6FF';
        td.style.textAlign = 'center';
        var valor = evtHot.Handson.ListaExcelData[row][0];

        var seleccionador = '';
        /*seleccionador = '<a href="JavaScript:VerEmpresaInvolucradaSco(' + valor + ')">Seleccionar archivo' + '</a >';*/
        seleccionador = '<input type="file" id="fileAnalisis_' + valor + '"accept="image/png, image/jpeg" />';
        td.innerHTML = seleccionador;
    }

    var contexMenuConfig = {
        items: {
            'row_below': { name: "Insertar fila abajo" },
            'remove_row': { name: "Eliminar fila" }
        }
    };

    containerNumeral = document.getElementById('NumeralTable');
    Handsontable.renderers.registerRenderer('negativeValueRenderer', negativeValueRenderer);

    hotOptions = {
        data: evtHot.Handson.ListaExcelData,
        maxCols: evtHot.Handson.ListaExcelData[0].length,
        colHeaders: false,
        rowHeaders: false,
        fillHandle: true,
        columnSorting: false,
        contextMenu: contexMenuConfig,
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
                if (col == 3 && evtHot.Handson.ListaExcelData[row][0] != "")
                    readOnly = true;
                else
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
        }
    };

    hotNumeral = new Handsontable(containerNumeral, hotOptions);
    hotNumeral.getPlugin('hiddenColumns').hideColumns([0]);
    hotNumeral.render();
}

function InsertarTipoNumeral() {
    var controlador = siteRoot + 'Eventos/AnalisisFallas/';
    var dataNumeral = hotNumeral.getData();
    $.ajax({
        type: 'POST',
        url: controlador + 'InsertarTipoNumeral',
        contentType: 'application/json',
        dataType: 'json',
        traditional: true,
        data: JSON.stringify({
            dataExcel: dataNumeral
        }),
        success: function (resultado) {
            if (resultado.result != '-1') {
                alert('Registro de Tipo de Numeral');
                llenarComboNumeral();
                ListadoAnalisisEvento();
                ListadoNumeral();
                //var bPopup = $('#popupNumeral').bPopup();
                //bPopup.close();
            }
        },
        error: function (err) {
            alert("Ingresa al error " + err);
        }
    });
}