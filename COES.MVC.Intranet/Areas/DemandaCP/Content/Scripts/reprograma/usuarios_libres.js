var _ht;
$(document).ready(function () {
    $('#btnActualizarULExtranet').on('click', function () {
        const msg = "Se actualizarán los datos reportados" +
            " por los agentes desde extranet";
        if (confirm(msg)) {
            actualizarDatosUL();
        }
    });

    //$('#btnConsultar').on('click', function () {
    //    obtenerDatosUL();
    //});

    $('#btnGuardar').click(function () {
        const _data = _ht.getData();
        const _res = formatoMedicion(_data);
        registrarDatosUL(_res);
    });

    obtenerDatosUL();
});

function obtenerDatosUL() {
    const [_id] = $('#repSelVersion')
        .multipleSelect('getSelects');
    const [_name] = $('#repSelVersion')
        .multipleSelect('getSelects', 'text');
    
    $.ajax({
        type: 'POST',
        url: `${controlador}ObtenerDatosUL`,
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

function actualizarDatosUL() {
    const [_id] = $('#repSelVersion')
        .multipleSelect('getSelects');
    const [_name] = $('#repSelVersion')
        .multipleSelect('getSelects', 'text');

    $.ajax({
        type: 'POST',
        url: `${controlador}ActualizarDatosUL`,
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

function registrarDatosUL(_datos) {  
    const _fecha = $('#repSelFecha').val();
    const [_version] = $('#repSelVersion')
        .multipleSelect('getSelects');

    $.ajax({
        type: 'POST',
        url: `${controlador}RegistrarDatosUL`,
        contentType: 'application/json; charset=utf-8',
        data: JSON.stringify({
            datos: _datos,
            version: _version,  
            fecha: _fecha,
        }),
        datatype: 'json',
        traditional: true,
        success: function (res) {
            alert(res.dataMsg);
        },
        error: function () {
            alert("Ha ocurrido un error...");
        }
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
        cell: formatoCells(model),
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
            allowInvalid: false,
            allowEmpty: false,
        }
        
        res.push(e);
    });

    return res;
}

function formatoCells(model) {
    let res = [];
    //Formato de la cabecera
    const _header = model.datos[0];
    _header.forEach((item, index) => {
        res.push({
            row: 0,
            col: index,
            readOnly: true,
            className: 'htCenter',
            renderer: HeaderCellRenderer,
        });
    });

    //Formato condicional de celda "cero"
    model.datos.forEach((row, rowIndex) => {
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

    //Formato condicional de celda "actualización"
    const _modData = model.datosModificados;
    const _ids = model.datos[model.datos.length - 1];
    _modData.forEach((item, index) => {
        const strId = item.id.toString();
        const colIndex = _ids.indexOf(strId);
        item.intervalos.forEach((itv, itvIndex) => {
            const rowIndex = itv + 1;
            res.push({
                row: rowIndex,
                col: colIndex,
                renderer: UpdateColumnRenderer,
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

            entity[`H${(index * 2)}`] = item;
        });
        res.push(entity);
        i++;
    }

    return res;
}

function UpdateColumnRenderer(instance, td, row, col, prop, value, cellProperties) {
    Handsontable.renderers.NumericRenderer.apply(this, arguments);
    td.style.background = '#DFF3D9';
}

function CeroColumnRenderer(instance, td, row, col, prop, value, cellProperties) {
    Handsontable.renderers.NumericRenderer.apply(this, arguments);
    if (parseFloat(value) == parseFloat(0)) {
        td.style.background = '#F6F6F6';
        td.style.color = '#A0A0A0';
    }
}