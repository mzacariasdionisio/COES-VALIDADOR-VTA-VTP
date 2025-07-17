var _ht;
$(document).ready(function () {
    $('#btnEnviarDatos').on('click', function () {
        const _grilla = formatoEnvioGrilla();
        const _modelo = formatoEnvioModelo();

        const msg = "Se enviará la información de la versión" +
            " mostrada al aplicativo Yupana";
        if (confirm(msg)) {
            enviarDatosVeg(_grilla, _modelo);
        }
    });

    obtenerDatosTotalBarras();
});

function obtenerDatosTotalBarras() {
    const [_id] = $('#repSelVersion')
        .multipleSelect('getSelects');
    const [_name] = $('#repSelVersion')
        .multipleSelect('getSelects', 'text');

    $.ajax({
        type: 'POST',
        url: `${controlador}ObtenerDatosTotalBarras`,
        contentType: 'application/json; charset=utf-8',
        data: JSON.stringify({
            idVersion: (_id) ? _id : -1,
            nomVersion: (_name) ? _name : 'none',
            fecha: $('#repSelFecha').val(),
        }),
        datatype: 'json',
        traditional: true,
        success: function (model) {
            iniciarHandson(model);
            SetMessage(
                '#message',
                model.msgVegtativa + '<br>' + model.msgUsuLibres,
                'info');
        },
        error: function () {
            alert("Ha ocurrido un error...");
        }
    });
}

function enviarDatosVeg(_dataExcel, _dataExcel2) {
    const [_name] = $('#repSelVersion')
        .multipleSelect('getSelects', 'text');

    $.ajax({
        type: 'POST',
        url: `${controlador}EnviarDatosVeg`,
        contentType: 'application/json; charset=utf-8',
        data: JSON.stringify({
            dataExcel: _dataExcel,
            dataExcel2: _dataExcel2,
            fecha: $('#repSelFecha').val(),
            nomVersion: _name,
        }),
        datatype: 'json',
        traditional: true,
        success: function (res) {
            if (res.Resultado == 1)
                alert('Se enviaron los datos');
            if (res.Resultado == -1)
                alert('Ha acurrido un error durante el envio');
        },
        error: function () {
            alert("Ha ocurrido un error...");
        },
    });
}

function iniciarHandson(model) {
    $('#ht').html('');
    const footer = model.datos.length - 1;
    const container = document.getElementById('ht');
    _ht = new Handsontable(container, {
        data: model.datos,
        colHeaders: true,
        rowHeaders: true,
        fillHandle: true,
        minSpareCols: 0,
        minSpareRows: 0,
        fixedRowsTop: 1,
        fixedColumnsLeft: 1,
        cell: formatoCells(model.datos),
        columns: formatoColumns(model),
        hiddenRows: {
            rows: [footer],
        },
    });
}

function formatoColumns(model) {
    let res = [];
    res.push({
        data: 0,
        type: 'text',
        className: 'htCenter',
        readOnly: true,
        width: '150px',
        renderer: HoraColumnRenderer,
    });

    const _columns = model.datos[model.datos.length - 1];
    _columns.forEach((item, index) => {
        if (index == 0) return;

        let e = {
            data: index,
            type: 'numeric',
            format: '0.00',
            readOnly: true,
        }
        res.push(e);
    });

    return res;
}

function formatoCells(data) {
    let res = [];
    //Formato de la cabecera
    const _header = data[0];
    _header.forEach((item, index) => {
        res.push({
            row: 0,
            col: index,
            readOnly: true,
            className: 'htCenter',
            renderer: HeaderCellRenderer,
        });
    });

    //Formato condicional de celda
    data.forEach((row, rowIndex) => {
        if (rowIndex == 0) return;

        row.forEach((col, colIndex) => {
            if (colIndex == 0) return;

            res.push({
                row: rowIndex,
                col: colIndex,
                renderer: CeroColumnRenderer,
            });
        });
    });

    return res;
}

function formatoMedicion(data) {
    let res = [];
    const _count = _ht.countCols();

    let i = 1;
    while (i < _count) {
        const col = _ht.getDataAtCol(i);

        let entity = {};
        col.forEach((item, index) => {
            if (index == 0) return;
            if (index == col.length - 1) {
                entity['Dpomedcodi'] = item;
                return;
            }

            entity[`H${index}`] = item;
        });
        res.push(entity);
        i++;
    }

    return res;
}

function formatoEnvioGrilla() {
    let _data = _ht.getData().slice(0, -1);
    return _data.toString();
}

function formatoEnvioModelo() {
    let _data = _ht.getData();
    const _ids = _data.pop();

    const res = [];
    _ids.forEach((item, index) => {
        if (index == 0) return;

        let entity = {
            Ptomedicodi: parseInt(item),
            Meditotal: 0,
            Tipoinfocodi: 1,
            Emprcodi: 0,
            MedifechaPto: _data[1][0],
        };

        let i = 0;
        while (i < 48) {
            const value = _data[(i + 1)][index];
            entity[`H${(i + 1)}`] = parseFloat(value);
            i++;
        }

        res.push(entity);
    });

    return JSON.stringify(res);
}

function CeroColumnRenderer(instance, td, row, col, prop, value, cellProperties) {
    Handsontable.renderers.NumericRenderer.apply(this, arguments);
    
    if (parseFloat(value) == parseFloat(0)) {
        td.style.background = '#F6F6F6';
        td.style.color = '#A0A0A0';
    }
}
