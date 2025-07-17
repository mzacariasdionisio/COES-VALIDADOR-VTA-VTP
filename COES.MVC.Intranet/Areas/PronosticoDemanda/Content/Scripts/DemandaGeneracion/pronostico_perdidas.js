var pop, objHt;

$(document).ready(function () {
    $('.f-fecha').Zebra_DatePicker();
    $('#btn-importar').on('click', function () {
        pop = $('#popup').bPopup({
            easing: 'easeOutBack',
            speed: 350,
            transition: 'fadeIn',
            modalClose: false,
            onOpen: function () {
                SetMessage('#pop-mensaje',
                    '• Seleccione el archivo a cargar.<br>' +
                    '• El archivo debe ser de formato Excel.<br>' +
                    '• Finalmente presione el boton Importar',
                    'info');
            }
        });
    });
    $('#btn-ejecutar').on('click', function () {
        pop = $('#popupEjecutar').bPopup({
            easing: 'easeOutBack',
            speed: 350,
            transition: 'fadeIn',
            modalClose: false,
            onOpen: function () {
                SetMessage('#pop-ejecutar-mensaje',
                    '• Se ejecutara el proceso de traslado de Pronóstico por "Áreas" a "Barras"<br>' +
                    '• Es necesario haber ejecutado el Pronóstico por áreas para el día seleccionado.<br>' +
                    '• Presione "Ejecutar" para seguir con el proceso, de lo contrario presione "X" para cancelar.',
                    'info');
            }
        });
    });
    $('#btn-pop-ejecutar').on('click', function () {
        ejecutar();
    });
    $('#btn-pop-importar').on('click', function () {
        importarArchivo();
    });

    obtenerDatos();
});

function obtenerDatos() {
    $.ajax({
        type: 'POST',
        url: controller + 'PerdidasPR03Datos',
        contentType: 'application/json; charset=utf-8',
        data: JSON.stringify({
            regFecha: $('#id-fecha').val()
        }),
        datatype: 'json',
        traditional: true,
        success: function (result) {
            var ht_model = FormatoHandson(result.data);
            obtenerHandson(ht_model);
        },
        error: function () {
            alert("Ha ocurrido un problema...");
        }
    });
}

function obtenerHandson(model) {
    $('#ht').html('');
    var container = document.getElementById('ht');
    objHt = new Handsontable(container, {
        data: model.data,
        fillHandle: true,
        stretchH: 'all',
        maxCols: model.maxCols,
        maxRows: model.maxRows,
        minSpareCols: 0,
        minSpareRows: 0,
        columns: model.columns,
        nestedHeaders: [
            ['', { label: 'PERDIDAS BASE(%)', colspan: 4 }, ' '],
            ['Hora', 'Area Norte', 'Area Sur', 'Area Centro', 'Area Sierra Centro', 'RECALCULO(%)']
        ],
    });
}

function ejecutar() {
    const perdidasNorte = objHt.getDataAtProp('norte');
    const perdidasSur = objHt.getDataAtProp('sur');
    const perdidasCentro = objHt.getDataAtProp('centro');
    const perdidasSCentro = objHt.getDataAtProp('scentro');
    const recalculo = objHt.getDataAtProp('recalculo');

    $.ajax({
        type: 'POST',
        url: controller + 'PerdidasPR03Ejecutar',
        contentType: 'application/json; charset=utf-8',
        data: JSON.stringify({
            perdidasNorte: perdidasNorte,
            perdidasSur: perdidasSur,
            perdidasCentro: perdidasCentro,
            perdidasSCentro: perdidasSCentro,
            recalculo: recalculo,
            regFecha: $('#id-fecha').val()
        }),
        datatype: 'json',
        traditional: true,
        success: function (result) {
            SetMessage('#message', result.dataMsg, result.typeMsg, true);
        },
        error: function () {
            alert("Ha ocurrido un problema...");
        }
    });
}

function importarArchivo() {
    const archivoSeleccionado = ($('#id-archivo'))[0].files[0];
    const fechaRegistro = $('#id-fecha').val();
    let archivoData = new FormData();
    archivoData.append('archivo', archivoSeleccionado);
    archivoData.append('regFecha', fechaRegistro);

    $.ajax({
        type: 'POST',
        url: controller + 'PerdidasPR03Importar',
        contentType: false,
        processData: false,
        data: archivoData,
        success: function (result) {
            SetMessage('#message', result.dataMsg, result.typeMsg, true);
            if (result.typeMsg == 'error') return false;
            
            const dataRecalculo = result.recalculo;
            const htData = HtGetDataAsProp();
            htData.forEach((e, index) => {
                e['recalculo'] = dataRecalculo[index];
            });

            objHt.updateSettings({ data: htData });
        },
        error: function () {
            alert("Ha ocurrido un problema...");
        },
        complete: function () {
            pop.close();
        }
    });
}

function HtGetDataAsProp() {
    var res = [];
    var s_max = objHt.countRows();
    var s_schema = objHt.getSchema();
    for (var i = 0; i < s_max; i++) {
        var s_row = {};
        $.each(s_schema, function (prop, value) {
            s_row[prop] = objHt.getDataAtRowProp(i, prop);
        });
        res.push(s_row);
    }
    return res;
}

//Formatea el modelo de datos para el Handsontable
function FormatoHandson(model) {
    var handson = {};
    var data = [], columns = [], headers = [];
    var rule = ['no'];

    $.each(model, function (i, item) {
        //crea la propiedad "colHeaders"
        if (!(rule.includes(item.htrender))) {
            headers.push(item.label);
        }

        //Crea la propiedad "columns"
        var col = {};
        col['data'] = item.id;

        switch (item.htrender) {
            case 'hora':
                col['type'] = 'text';
                col['className'] = 'htCenter';
                col['readOnly'] = true;
                col['renderer'] = HoraColumnRenderer;
                break;
            case 'normal':
                col['type'] = 'numeric';
                col['format'] = '0.00';
                col['readOnly'] = true;
                break;
            case 'patron':
                col['type'] = 'numeric';
                col['format'] = '0.00';
                col['readOnly'] = true;
                col['renderer'] = PatronColumnRenderer;
                break;
            case 'edit':
                col['type'] = 'numeric';
                col['format'] = '0.00';
                col['readOnly'] = false;
                col['allowInvalid'] = false;
                col['allowEmpty'] = false;
                break;
            case 'final':
                col['type'] = 'numeric';
                col['format'] = '0.00';
                col['readOnly'] = true;
                col['renderer'] = PatronColumnRenderer;
        }

        if (!(rule.includes(item.htrender))) {
            columns.push(col);

            //Crea la propiedad "data"
            $.each(item.data, function (j, value) {
                var row = {};
                if (data[j]) {
                    data[j][item.id] = value;
                }
                else {
                    row[item.id] = value;
                    data.push(row);
                }
            });
        }
    });

    handson['colHeaders'] = headers;
    handson['columns'] = columns;
    handson['data'] = data;
    handson['maxCols'] = columns.length;
    handson['maxRows'] = data.length;
    return handson;
}

//Estilos de las columnas del Handsontable
function HoraColumnRenderer(instance, td, row, col, prop, value, cellProperties) {
    Handsontable.renderers.TextRenderer.apply(this, arguments);
    td.style.fontWeight = 'bold';
    td.style.background = '#F2F2F2';
}
