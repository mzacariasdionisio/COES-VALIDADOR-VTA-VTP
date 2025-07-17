var ht;
$(document).ready(function () {

    $('.f-fecha').Zebra_DatePicker({
        onSelect: function () {
            $.ajax({
                url: actualizarVersion(),
                success: function () {
                    obtenerMedicionesBarras();
                }
            });
        }
    });

    $('#idVersion').multipleSelect({
        filter: true,
        single: true,
        placeholder: 'Seleccione',
        onClose: function () {
            cargaFiltrosBarras();
            obtenerMedicionesBarras();
        }
    })

    $('#idBarraOrigen').multipleSelect({
        filter: true,
        single: true,
        placeholder: 'Seleccione',
        onClose: function () {
            obtenerMedicionesBarras();
        },
        styler: function (row) {
            let r = document.getElementById('idBarraOrigen');
            for (var i = 0; i < r.length; i++) {
                let d = document.getElementById('idBarraOrigen')[i];
                if (d.value == row && d.dataset.contador >= 1) {
                    return 'background-color: #ffee00; color: #ff0000;'
                }
            }
        }
    })

    $('#idBarraDestino').multipleSelect({
        filter: true,
        single: true,
        placeholder: 'Seleccione',
        onClose: function () {
            obtenerMedicionesBarras();
        },
        styler: function (row) {
            const r = document.getElementById('idBarraDestino');
            for (var i = 0; i < r.length; i++) {
                const d = document.getElementById('idBarraDestino')[i];
                if (d.value == row && d.dataset.contador >= 1) {
                    return 'background-color: #ffee00; color: #ff0000;'
                }
            }
        }
    })

    $('#btn-guardar').on('click', function () {
        actualizarMedicionTraslado();
    });

    obtenerMedicionesBarras();
});

function actualizarMedicionTraslado() {
    const version = $('#idVersion').val();
    const barraOrigen = $('#idBarraOrigen').val();
    const barraDestino = $('#idBarraDestino').val();

    $.ajax({
        type: 'POST',
        url: controller + 'ActualizarMedicionTraslado',
        contentType: 'application/json; charset=utf-8',
        data: JSON.stringify({
            fecha: $('#idFecha').val(),
            idVersion: (version) ? version : -1,
            idOrigen: (barraOrigen) ? barraOrigen : -1,
            idDestino: (barraDestino) ? barraDestino : -1,
            data: ht.getData()
        }),
        datatype: 'json',
        traditional: true,
        success: function (result) {
            SetMessage('#message', result.dataMsg, result.typeMsg, true);
            cargaFiltrosBarras();
            
        },
        error: function () {
            alert('Ha ocurrido un error...');
        }
    });
}

function cargaFiltrosBarras() {
    const version = $('#idVersion').val();
    $.ajax({
        type: 'POST',
        url: controller + 'cargaFiltrosBarras',
        contentType: 'application/json; charset=utf-8',
        data: JSON.stringify({
            idVersion: (version) ? version : -1
        }),
        datatype: 'json',        
        traditional: true,
        success: function (result) {
            RefillDropDownListTraslado($('#idBarraOrigen'), result.ListBarrasOrigen, 'Grupocodi', 'Gruponomb');        
            RefillDropDownListTraslado($('#idBarraDestino'), result.ListBarrasDestino, 'Grupocodi', 'Gruponomb');
        },
        error: function () {
            alert('Ha ocurrido un error...');
        }
    });
}

function RefillDropDownListTraslado(element, data, data_id, data_name) {
    //Vacia el elemento
    element.empty();
    //Carga el elemento
    $.each(data, function (i, item) {
        var n_value = i, n_html = item;
        if (data_id) n_value = item[data_id];
        if (data_name) n_html = item[data_name];
        element.append($(`<option data-contador="${item.PrnmgrTrasladoCount}"></option>`).val(n_value).html(n_html));
    });
    //Actualiza la lib.multipleselect
    element.multipleSelect('refresh');
}

function obtenerMedicionesBarras() {
    const version = $('#idVersion').val();
    const barraOrigen = $('#idBarraOrigen').val();
    const barraDestino = $('#idBarraDestino').val();

    $.ajax({
        type: 'POST',
        url: controller + 'ObtenerMedicionesBarras',
        contentType: 'application/json; charset=utf-8',
        data: JSON.stringify({
            fecha: $('#idFecha').val(),
            idVersion: (version) ? version : -1,
            idOrigen: (barraOrigen) ? barraOrigen : -1,
            idDestino: (barraDestino) ? barraDestino : -1
        }),
        datatype: 'json',
        traditional: true,
        success: function (result) {

            //Crea el handsontable
            const ht_model = FormatHandson(result.data);
            iniciarHandson(ht_model);

        },
        error: function () {
            alert('Ha ocurrido un error...');
        }
    });
}

function actualizarVersion() {
    $.ajax({
        type: 'POST',
        url: controller + 'PronosticoPorBarrasActualizarVersion',
        contentType: 'application/json; charset=utf-8',
        data: JSON.stringify({
            fechaIni: $('#idFecha').val(),
            fechaFin: $('#idFecha').val(),
        }),
        datatype: 'json',
        traditional: true,
        success: function (result) {
            RefillDropDowList($('#idVersion'),
                result,
                'Vergrpcodi',
                'Vergrpnomb');
        },
        error: function () {
            alert("Ha ocurrido un problema...");
        }
    });
}

function iniciarHandson(model) {
    const contenedor = document.getElementById('ht');
    contenedor.innerHTML = '';
    ht = new Handsontable(contenedor, {
        data: model.data,
        fillHandle: true,
        stretchH: 'all',
        maxCols: model.maxCols,
        maxRows: model.maxRows,
        minSpareCols: 0,
        minSpareRows: 0,
        columns: model.columns,
        colHeaders: model.colHeaders,
        afterChange: function (changes, source) {
            if (ValidarEventosHandsontable(source)) {
                htBaseIndexOrigen = ht.propToCol('origenInicial');//Setea la columna base (Handsontable)
                htBaseIndexDestino = ht.propToCol('destinoInicial');//Setea la columna base (Handsontable)
                htFinalIndexOrigen = ht.propToCol('origenFinal');//Setea la columna final (Handsontable)
                htFinalIndexDestino = ht.propToCol('destinoFinal');//Setea la columna final (Handsontable)

                for (var i = 0; i < changes.length; i++) {
                    htRowIndex = changes[i][0]; htRowData = ht.getDataAtRow(htRowIndex);

                    //Agrega el ajuste realizado
                    htSumaOrigen = parseFloat(changes[i][3]) * -1;
                    htSumaDestino = parseFloat(changes[i][3]);

                    //Suma el valor de "Entrada"
                    if (htRowData[htBaseIndexOrigen] != null) {
                        htSumaOrigen += parseFloat(htRowData[htBaseIndexOrigen]);
                    }
                    if (htRowData[htBaseIndexDestino] != null) {
                        htSumaDestino += parseFloat(htRowData[htBaseIndexDestino]);
                    }

                    //Realiza la modificación
                    ht.setDataAtCell(htRowIndex, htFinalIndexOrigen, htSumaOrigen, 'sumOrigen');
                    ht.setDataAtCell(htRowIndex, htFinalIndexDestino, htSumaDestino, 'sumDestino');
                }
            }
        }
    });
}

function FormatHandson(model) {
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

function ValidarEventosHandsontable(evento) {
    var valid = false;
    var ListEventos = ['edit', 'autofill', 'paste', 'grafico'];

    for (var i = 0; i < ListEventos.length; i++) {
        if (evento == ListEventos[i]) valid = true;
    }
    return valid;
}

function HoraColumnRenderer(instance, td, row, col, prop, value, cellProperties) {
    Handsontable.renderers.TextRenderer.apply(this, arguments);
    td.style.fontWeight = 'bold';
    td.style.background = '#F2F2F2';
}

function PatronColumnRenderer(instance, td, row, col, prop, value, cellProperties) {
    Handsontable.renderers.NumericRenderer.apply(this, arguments);
    td.style.background = '#edf5fc';
}
