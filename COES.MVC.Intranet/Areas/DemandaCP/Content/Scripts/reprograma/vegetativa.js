var _ht;
var _popRvcg, _popRvcv, _popRve;

$(document).ready(function () {
    $('#rpvcgHistorico').multipleSelect({
        filter: true,
        single: true,
        placeholder: 'Seleccione...',
    });
    $('#rpvcvHistorico').multipleSelect({
        filter: true,
        single: true,
        placeholder: 'Seleccione...',        
    });
    $('#rpvcgFechaHora').Zebra_DatePicker({
        format: 'H:i',
    });
    $('#rpvcvFechaHora').Zebra_DatePicker({
        format: 'H:i',
    });

    //$('#btnConsultar').on('click', function () {
    //    obtenerDatosVeg();
    //});
    $('#btnGuardar').on('click', function () {
        const _data = _ht.getData();
        const _res = formatoMedicion(_data);
        registrarDatosVeg(_res);
    });
    $('#btnEnviarDatos').on('click', function () {
        const _grilla = formatoEnvioGrilla();
        const _modelo = formatoEnvioModelo();

        const msg = "Se enviará la información de la versión" +
            " mostrada al aplicativo Yupana";
        if (confirm(msg)) {
            enviarDatosVeg(_grilla, _modelo);
        }
    });

    $('#repVegConfGeneral').click(function () {        
        _popRvcg = $('#repPopVegConfGeneral').bPopup({
            easing: 'easeOutBack',
            speed: 350,
            transition: 'fadeIn',
            modalClose: false,
            onClose: function () {
                const r = {
                    Dpocngdias: [],
                    Dpocngpromedio: 0,
                    Dpocngtendencia: 0,
                    Dpocnggaussiano: 0,
                }
                setearEntidadConfGeneral(r);                
            }
        }, function () {
            obtenerConfiguracion("general");
        });
    });

    $('#repVegConfVersion').click(function () {
        const [_id] = $('#repSelVersion')
            .multipleSelect('getSelects');
        if (!_id) {
            alert("Debe seleccionar una versión a configurar");
            return;
        }

        _popRvcv = $('#repPopVegConfVersion').bPopup({
            easing: 'easeOutBack',
            speed: 350,
            transition: 'fadeIn',
            modalClose: false,
            onClose: function () {
                const r = {
                    Dpocngdias: [],
                    Dpocngpromedio: 0,
                    Dpocngtendencia: 0,
                    Dpocnggaussiano: 0,
                }
                setearEntidadConfVersion(r);
            }
        }, function () {
            obtenerConfiguracion("version");
        });
    });

    $('#rpvcgGuardar').click(function () {
        const res = obtenerEntidadConfGeneral();     
        registrarConfiguracion(res);
        _popRvcg.close();
    });

    $('#rpvcvGuardar').click(function () {
        const res = obtenerEntidadConfVersion();
        registrarConfiguracion(res);
        _popRvcv.close();
    });

    $('#repVegEjecutarPronostico').on('click', function () {
        _popRve = $('#repPopVegEjecutar').bPopup({
            easing: 'easeOutBack',
            speed: 350,
            transition: 'fadeIn',
            modalClose: false,
            onOpen: function () {
                SetMessage('#rpveMensaje',
                    '• Verifique la configuración para el cálculo del pronóstico antes de "Ejecutar".<br>' +
                    '• Se ejecutará el pronóstico para la fecha seleccionada en el filtro "Fecha consultada".<br>' +
                    '• El pronóstico se ejecutará para todas las "Barras CP" registradas.<br>' +
                    '• El proceso puede tardar varios minutos.',
                    'warning');
            },            
        });
    });

    $('#rpveEjecutar').click(function () {
        ejecutarPronostico();
    });

    $('#btnPrueba').click(function () {
        ejecutarPruebaDemVeg();
    });

    //Validaciones
    $('.validSoloEnteros').change(function () {
        let valor = this.value;
        $(this).val(Math.round(valor));
    });

    obtenerDatosVeg();
});

function obtenerDatosVeg() {
    const [_id] = $('#repSelVersion')
        .multipleSelect('getSelects');
    const [_name] = $('#repSelVersion')
        .multipleSelect('getSelects', 'text');

    $.ajax({
        type: 'POST',
        url: `${controlador}ObtenerDatosVeg`,
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

function registrarDatosVeg(_datos) {
    const _fecha = $('#repSelFecha').val();
    const [_version] = $('#repSelVersion')
        .multipleSelect('getSelects');

    $.ajax({
        type: 'POST',
        url: `${controlador}RegistrarDatosVeg`,
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

function obtenerConfiguracion(tipo) {
    const [_id] = $('#repSelVersion')
        .multipleSelect('getSelects');

    $.ajax({
        type: 'POST',
        url: `${controlador}ObtenerDatosConf`,
        contentType: 'application/json; charset=utf-8',
        data: JSON.stringify({
            idVersion: (tipo == "version") ? _id : -1,            
        }),
        datatype: 'json',
        traditional: true,
        success: function (res) {
            if (tipo === "general")
                setearEntidadConfGeneral(res);
            if (tipo === "version")
                setearEntidadConfVersion(res);
        },
        error: function () {
            alert("Ha ocurrido un error...");
        }
    });
}

function registrarConfiguracion(_datos) {
    $.ajax({
        type: 'POST',
        url: `${controlador}RegistrarConfiguracionVeg`,
        contentType: 'application/json; charset=utf-8',
        data: JSON.stringify({
            datos: _datos,
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

function ejecutarPronostico() {
    const [_id] = $('#repSelVersion')
        .multipleSelect('getSelects');
    const [_name] = $('#repSelVersion')
        .multipleSelect('getSelects', 'text');
    const _fecha = $('#repSelFecha').val();
    const [_idVersionAnte] = $('#repSelVersionAnte')
        .multipleSelect('getSelects');
    const _fechaAnterior = $('#repSelFechaAnte').val();
    const [_idVersionComp] = $('#repSelVersionComp')
        .multipleSelect('getSelects');
    const _fechaComparacion = $('#repSelFechaComp').val();

    $.ajax({
        type: 'POST',
        url: `${controlador}EjecutarPronosticoVeg`,
        contentType: 'application/json; charset=utf-8',
        data: JSON.stringify({
            idVersion: (_id) ? _id : -1,
            nomVersion: (_name) ? _name : 'none',
            fecha: _fecha,
            idVersionAnterior: (_idVersionAnte) ? _idVersionAnte : -1,
            fechaAnterior: _fechaAnterior,
            idVersionComparacion: (_idVersionComp) ? _idVersionComp : -1,
            fechaComparacion: _fechaComparacion,
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
        },
        complete: function () {
            _popRve.close();
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
            allowInvalid: false,
            allowEmpty: false,
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

            entity[`H${(index * 2)}`] = item;
        });
        res.push(entity);
        i++;
    }

    return res;
}

function obtenerEntidadConfGeneral() {
    const _id = $('#repPopVegConfGeneral')
        .data('codigo');

    const [_cgHistorico] = $('#rpvcgHistorico')
        .multipleSelect('getSelects');
    const _cgFechaHora = $('#rpvcgFechaHora').val();
    const _cgPromedio = $('#rpvcgPromedio').val();
    const _cgTendencia = $('#rpvcgTendencia').val();
    const _cgGaussiano = $('#rpvcgGaussiano').val();

    const _cgUmbral = $('#rpvcgUmbral').val();
    const _cgVmg = $('#rpvcgVmg').val();
    const _cgStd = $('#rpvcgStd').val();

    return {
        Dpocngcodi: _id,
        Dpocngdias: _cgHistorico,
        Dpocngpromedio: _cgPromedio,
        Dpocngtendencia: _cgTendencia,
        Dpocnggaussiano: _cgGaussiano,
        Dpocngumbral: _cgUmbral,
        Dpocngvmg: _cgVmg,
        Dpocngstd: _cgStd,
        Dpocngfechora: _cgFechaHora,
    };
}

function obtenerEntidadConfVersion() {
    const _id = $('#repPopVegConfVersion')
        .data('codigo');

    const _cvVersion = $('#repSelVersion').val();
    const [_cvHistorico] = $('#rpvcvHistorico')
        .multipleSelect('getSelects');
    const _cvFechaHora = $('#rpvcvFechaHora').val();
    const _cvPromedio = $('#rpvcvPromedio').val();
    const _cvTendencia = $('#rpvcvTendencia').val();
    const _cvGaussiano = $('#rpvcvGaussiano').val();

    const _cvUmbral = $('#rpvcvUmbral').val();
    const _cvVmg = $('#rpvcvVmg').val();
    const _cvStd = $('#rpvcvStd').val();

    return {
        Dpocngcodi: _id,
        Vergrpcodi: _cvVersion,
        Dpocngdias: _cvHistorico,
        Dpocngpromedio: _cvPromedio,
        Dpocngtendencia: _cvTendencia,
        Dpocnggaussiano: _cvGaussiano,
        Dpocngumbral: _cvUmbral,
        Dpocngvmg: _cvVmg,
        Dpocngstd: _cvStd,
        Dpocngfechora: _cvFechaHora,
    };
}

function setearEntidadConfGeneral(data) {   
    console.log(data, '_general');
    $('#repPopVegConfGeneral').data('codigo', -1);

    const _dias = (data != null) ? [data.Dpocngdias] : [];
    $('#rpvcgHistorico').multipleSelect('setSelects', _dias);

    $('#rpvcgPromedio').val(data.Dpocngpromedio);
    $('#rpvcgTendencia').val(data.Dpocngtendencia);
    $('#rpvcgGaussiano').val(data.Dpocnggaussiano);

    $('#rpvcgUmbral').val(data.Dpocngumbral);
    $('#rpvcgVmg').val(data.Dpocngvmg);
    $('#rpvcgStd').val(data.Dpocngstd);
    $('#rpvcgFechaHora').val(data.Dpocngfechora);
}

function setearEntidadConfVersion(data) {
    console.log(data, '_version');
    $('#repPopVegConfVersion').data('codigo', data.Dpocngcodi);

    const _dias = (data != null) ? [data.Dpocngdias] : [];
    $('#rpvcvHistorico').multipleSelect('setSelects', _dias);

    $('#rpvcvPromedio').val(data.Dpocngpromedio);
    $('#rpvcvTendencia').val(data.Dpocngtendencia);
    $('#rpvcvGaussiano').val(data.Dpocnggaussiano);

    $('#rpvcvUmbral').val(data.Dpocngumbral);
    $('#rpvcvVmg').val(data.Dpocngvmg);
    $('#rpvcvStd').val(data.Dpocngstd);
    $('#rpvcvFechaHora').val(data.Dpocngfechora);
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
