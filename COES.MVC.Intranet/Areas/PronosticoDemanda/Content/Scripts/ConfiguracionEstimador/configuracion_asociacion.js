var objHt, objHc, objPopup;
var htItems, dtRelaciones;
var formulasPorNombre = [];

$(document).ready(function () {
    $('#selFecha').Zebra_DatePicker({
        onSelect: function () {
            obtenerMediciones();
        }
    });

    $('.toolHistorico').Zebra_DatePicker({
        onSelect: function () {
            const m = {
                id: this.attr('id'),
                fecha: this.val()
            };

            actualizarMedicion(m);
        }
    });

    $('#selTipos').multipleSelect({
        filter: false,
        single: true,
        placeholder: 'Seleccione',
        onClose: function () {
            $('#selBarras').multipleSelect('uncheckAll');
            FiltrarRelaciones();
        }
    });

    $('#selBarras').multipleSelect({
        filter: false,
        single: true,
        placeholder: 'Seleccione',
        onClose: function () {
            obtenerMediciones();
        }
    });

    $('#btnExportar').on('click', function () {
        const myData = objHt.getData();
        const myHeaders = objHt.getColHeader();

        Exportar(myData, myHeaders);
    });

    $('.filtroSimple').each(function () {
        let element = this;
        $(element).multipleSelect({
            filter: false,
            single: true            
        });
    });

    $('.filtroMultiple').each(function () {
        let element = this;
        $(element).multipleSelect({
            filter: true,
            selectAll: false,
            onClose: function () {
                const ids = $(element).multipleSelect('getSelects');
                const nombs = $(element).multipleSelect('getSelects', 'text');
                if (ids.length == 0) {
                    htItems.updateSettings({
                        data: [],
                        columns: ColumnsHandsonBarras()
                    });
                    return false;
                }

                let newData = ids.map((e, index) => [`${nombs[index].trim()}[${e}]`, ""]);
                
                const htData = htItems.getData();
                if (htData.length > 0) {
                    newData.forEach(e => {
                        htData.forEach(f => {
                            if (f[0] == e[0]) e[1] = f[1];
                        });
                    });
                }
                htItems.updateSettings({
                    data: newData,
                    columns: ColumnsHandsonBarras()
                }); 
            }
        });
    });

    $('#btnCrearRelacion').on('click', function () {
        objPopup = $('#popup-relacion').bPopup({
            easing: 'easeOutBack',
            speed: 350,
            transition: 'fadeIn',
            modalClose: false,
            onClose: function () {
                $('#txtPopNombre').val('');
                dtRelaciones.data()
                    .clear()
                    .draw();
                htItems.updateSettings({
                    data: [],
                    columns: ColumnsHandsonBarras()
                });
            }
        }, function () {
            generarRelacion();
        });
    });

    $('#btnPopNuevo').on('click', function () {
        const nombre = $('#txtPopNombre').val();
        if (!nombre) {
            SetMessage('#pop-alert',
                'El campo nombre es obligatorio',
                'warning', true);
            return false;
        }

        const vegetativa = $('#selComponenteVegetativa').val();
        if (!vegetativa) {
            SetMessage('#pop-alert',
                'Debe agregar una fórmula para la componente vegetativa',
                'warning', true);
            return false;
        }

        const seleccionados = $('#selPopBarrasCP')
            .multipleSelect('getSelects');
        if (seleccionados.length == 0) {
            SetMessage('#pop-alert',
                'Debe agregar al menos una barra CP',
                'warning', true);
            return false;
        }

        //validacion para que no se repita el nombre
        let existe = false;
        dtRelaciones.rows()
            .data()
            .map(e => {
                if (e.Reltnanom == nombre.trim()) {
                    existe = true;;
                }
            });

        if (existe) {
            SetMessage('#pop-alert',
                'Ya existe un registro con ese nombre!',
                'warning', true);
            return false;
        }

        const itemsData = htItems.getData();
        if (itemsData.find(e => e[1] == "")) {
            SetMessage('#pop-alert',
                'Debe asignar una fórmula a todas las barras CP',
                'warning', true);
            return false;
        }
        const detalle = itemsData
            .map(e => {
                let [nomBarra, idBarra] = e[0].split('[');
                idBarra = parseInt(idBarra.split(']'));
                let [nomFormula, idFormula] = e[1].split('[');
                idFormula = parseInt(idFormula.split(']'));
                return [idBarra, nomBarra, idFormula, nomFormula];
            });
        
        dtRelaciones.rows
            .add([{
                Reltnacodi: -1,
                Reltnaformula: vegetativa,
                Reltnanom: nombre,
                Detalle: detalle
            }])
            .draw();

        SetMessage('#pop-alert',
            'Registro creado',
            'success', true);
    });

    $('#btnPopActualizar').on('click', function () {
        const row = $('#dtRelaciones .row-selected').toArray();
        if (row.length == 0) return false;

        const vegetativa = $('#selComponenteVegetativa').val();

        const seleccionados = $('#selPopBarrasCP')
            .multipleSelect('getSelects');
        if (seleccionados.length == 0) {
            SetMessage('#pop-alert',
                'Debe agregar al menos una barra CP',
                'warning', true);
            return false;
        }

        const itemsData = htItems.getData();
        if (itemsData.find(e => e[1] == "")) {
            SetMessage('#pop-alert',
                'Debe asignar una fórmula a todas las barras CP',
                'warning', true);
            return false;
        }
        const detalle = itemsData
            .map(e => {
                let [nomBarra, idBarra] = e[0].split('[');
                idBarra = parseInt(idBarra.split(']'));
                let [nomFormula, idFormula] = e[1].split('[');
                idFormula = parseInt(idFormula.split(']'));
                return [idBarra, nomBarra, idFormula, nomFormula];
            });

        dtRelaciones.row(row)
            .data()
            .Reltnaformula = vegetativa;
        dtRelaciones.row(row)
            .data()
            .Detalle = detalle;

        SetMessage('#pop-alert',
            'Registro actualizado',
            'success', true);
    });

    $('#btnPopEliminar').on('click', function () {
        const row = $('#dtRelaciones .row-selected').toArray();
        if (row.length == 0) return false;

        dtRelaciones.row(row)
            .remove()
            .draw();
        $('#selComponenteVegetativa')
            .multipleSelect('uncheckAll');
        $('#selPopBarrasCP')
            .multipleSelect('uncheckAll');
        htItems.updateSettings({
            data: [],
            columns: ColumnsHandsonBarras()
        });

        SetMessage('#pop-alert',
            'Registro eliminado',
            'success', true);
    });

    $('#btnRegistrarAsociacion').on('click', function () {
        const data = dtRelaciones.rows()
            .data()
            .toArray();
        registrarRelacion(data);
    });

    $('#btnRegistrarDefecto').on('click', function () {        
        const data = objHt.getDataAtProp('final');
        registrarDefecto(data);
    });

    FiltrarRelaciones();
    obtenerMediciones();
    iniciarHandsonBarras();
});

function registrarDefecto(datosMedicion) {
    const id = $('#selBarras').val();
    $.ajax({
        type: 'POST',
        url: controller + 'RegistrarPerfilDefecto',
        contentType: 'application/json; charset=utf-8',
        data: JSON.stringify({
            idRelacion: (id) ? id : -1,
            regFecha: $('#selFecha').val(),
            datosMedicion: datosMedicion,
        }),
        datatype: 'json',
        traditional: true,
        success: function (result) {
            objHc.get("defecto").
                setData(datosMedicion);

            const _data = HtGetDataAsProp();
            _data.forEach((e, index) => {
                e['defecto'] = datosMedicion[index];
            });
            objHt.updateSettings({
                data: _data,
            });

            SetMessage('#message',
                result.dataMsg,
                result.typeMsg,
                true);
        },
        error: function () {
            SetMessage('#message',
                'Ha ocurrido un problema...',
                'error',
                true);            
        }
    });
}

function FiltrarRelaciones() {
    const id = $('#selTipos').val();
    $.ajax({
        type: 'POST',
        url: controller + 'FiltrarRelaciones',
        contentType: 'application/json; charset=utf-8',
        data: JSON.stringify({
            idTipo: id
        }),
        datatype: 'json',
        traditional: true,
        success: function (result) {
            RefillDropDownList($('#selBarras'), result, 'Reltnacodi', 'Reltnanom');
        },
        error: function () {
            alert("Ha ocurrido un problema...");
        }
    });
}

function obtenerMediciones() {
    const id = $('#selBarras').val();    
    $.ajax({
        type: 'POST',
        url: controller + 'ObtenerDatosRelacion',
        contentType: 'application/json; charset=utf-8',
        data: JSON.stringify({
            idRelacion: (id) ? id : -1,
            regFecha: $('#selFecha').val(),
        }),
        datatype: 'json',
        traditional: true,
        success: function (result) {            
            //Crea el handsontable
            const ht_model = FormatHandson(result.data);
            iniciarHandson(ht_model);

            //Crea el highchart
            const hc_model = FormatHighchart(result.data);
            iniciarHighchart(hc_model);

            //Llena calendarios
            iniciarCalendarios(result.data);
        },
        error: function () {
            alert("Ha ocurrido un problema...");
        }
    });
}

function actualizarMedicion(medicion) {
    const id = $('#selBarras').val();
    $.ajax({
        type: 'POST',
        url: controller + 'ActualizarMedicionRelacion',
        contentType: 'application/json; charset=utf-8',
        data: JSON.stringify({
            idRelacion: (id) ? id : -1,
            fecha: medicion.fecha
        }),
        datatype: 'json',
        traditional: true,
        success: function (modelo) { 
            objHc.get(medicion.id).
                setData(modelo);            
            objHc.get(medicion.id)
                .update({ name: medicion.fecha });

            const htData = HtGetDataAsProp();
            htData.forEach((e, index) => {
                e[medicion.id] = modelo[index];
            });

            let htHeader = objHt.getColHeader();
            const colIndex = objHt.propToCol(medicion.id);
            htHeader[colIndex] = medicion.fecha;
            objHt.updateSettings({
                data: htData,
                colHeaders: htHeader
            });

            recalcularPerfilPatron();
        },
        error: function () {
            alert('Ha ocurrido un error...');
        }
    });
}

function Exportar(datos, headers) {
    let val = nombreUnidadAsociacion();
    if (val == true) {
        SetMessage('#message', 'Falta seleccionar una Barra...', 'error', true);
    } else {
        $.ajax({
            type: 'POST',
            url: controller + 'Exportar',
            contentType: 'application/json; charset=utf-8',
            data: JSON.stringify({
                form: datos,
                header: headers,
                modulo: 2,
                nombre: val
            }),
            datatype: 'json',
            traditional: true,
            success: function (result) {
                if (result != -1) {
                    window.location = controller + 'abrirarchivo?formato=' + 2 + '&file=' + result;
                    SetMessage('#message', 'Felicidades, el archivo se descargo correctamente...!', 'success', true);
                }
                else {
                    SetMessage('#message', 'Lo sentimos, ha ocurrido un error inesperado', 'error', true);
                }
            },
            error: function () {
                alert("Ha ocurrido un problema...");
            }
        });
    }
}

function generarRelacion() {
    $.ajax({
        type: 'POST',
        url: controller + 'GenerarRelacion',
        contentType: 'application/json; charset=utf-8',        
        datatype: 'json',
        traditional: true,
        success: function (model) {
            formulasPorNombre = model.ListFormulasEstimador
                .map(e => {
                    return `${e.Prruabrev}[${e.Prrucodi}]`
                });
            RefillDropDownList($('#selComponenteVegetativa'), model.ListFormulasEstimador,
                'Prrucodi', 'Prruabrev');
            RefillDropDownList($('#selPopBarrasCP'), model.ListBarrasCP,
                'Grupocodi', 'Gruponomb');

            iniciarTablaRelaciones(model.ListRelacion);
        },
        error: function () {
            alert("Ha ocurrido un problema...");
        }
    });
}

function registrarRelacion(datosRelacion) {
    $.ajax({
        type: 'POST',
        url: controller + 'RegistrarRelacion',
        contentType: 'application/json; charset=utf-8',
        data: JSON.stringify({
            dataRelaciones: datosRelacion
        }),
        datatype: 'json',
        traditional: true,
        success: function (result) {            
            SetMessage('#message', result, 'success', true);
            FiltrarRelaciones();
        },
        error: function () {
            alert("Ha ocurrido un problema...");
        },
        complete: function () {
            objPopup.close();
        }
    });
}

function iniciarHandson(model) {
    const contenedor = document.getElementById('ht');
    contenedor.innerHTML = '';
    objHt = new Handsontable(contenedor, {
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
                const htBaseIndex = objHt.propToCol('base');//Setea la columna base (Handsontable)
                const htDestinoIndex = objHt.propToCol('final');//Setea la columna final (Handsontable)                
                const hcSerieIndex = objHc.get('final').index;//Setea el indice de la linea que se modificara (Highchart)

                for (var i = 0; i < changes.length; i++) {
                    const htRowIndex = changes[i][0]; let htRowData = objHt.getDataAtRow(htRowIndex);

                    htSuma = parseFloat(changes[i][3]);//Agrega el ajuste realizado

                    if (htRowData[htBaseIndex] != null) {
                        htSuma += parseFloat(htRowData[htBaseIndex]);//Suma el valor de "Entrada"
                    }

                    objHt.setDataAtCell(htRowIndex, htDestinoIndex, htSuma, 'sum');//Realiza la modificación

                    if (source != 'grafico') {//El evento "grafico" modifica la grafica previamente
                        objHc.series[hcSerieIndex].data[htRowIndex].update(htSuma);
                    }
                }
            }
        },
    });
}

//Formatea el modelo de datos para el Handsontable
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

function iniciarHighchart(model) {
    objHc = Highcharts.chart('hc', {
        chart: {
            type: 'spline',
            backgroundColor: 'transparent',
            plotShadow: false,
            zoomType: 'xy',
        },
        title: {
            text: ''
        },
        credits: {
            enabled: false
        },
        line: {
            cursor: 'ns-resize'
        },
        legend: {
            itemDistance: 15,
            verticalAlign: 'top',
            symbolWidth: 10,
            itemStyle: {
                fontWeight: 'normal'
            },
            y: 0
        },
        tooltip: { enabled: true },
        plotOptions: {
            series: {
                stickyTracking: false,
                marker: { enabled: true, radius: 3 }
            }
        },
        xAxis: {
            tickInterval: 1,
            categories: model.categories,
            labels: { rotation: 90 }
        },
        yAxis: {
            title: ''
        },
        tooltip: {
            crosshairs: true,
            shared: true
        },
        series: model.series
    });
}

//Formatea el modelo de datos para el Highchart
function FormatHighchart(model) {
    var highchart = {};
    var categories;
    var series = [];
    var rule = ['categoria', 'no'];
    $.each(model, function (i, item) {
        var serie = {};
        switch (item.hcrender) {
            case 'categoria':
                categories = item.data;
                break;
            case 'normal':
                serie['id'] = item.id;
                serie['name'] = item.label;
                serie['data'] = item.data;
                serie['marker'] = { enabled: true, symbol: 'circle' };
                break;
            case 'patron':
                serie['id'] = item.id;
                serie['name'] = item.label;
                serie['data'] = item.data;
                serie['lineWidth'] = 6;
                serie['marker'] = { enabled: true, symbol: 'circle' };
                break;
            case 'margen':
                serie['id'] = item.id;
                serie['name'] = item.label;
                serie['data'] = item.data;
                serie['color'] = '#000000';
                serie['dashStyle'] = 'dot';
                serie['marker'] = { enabled: false, symbol: 'circle' };
                break;
            case 'final':
                serie['id'] = item.id;
                serie['name'] = item.label;
                serie['data'] = item.data;
                serie['lineWidth'] = 6;
                serie['marker'] = { enabled: true, symbol: 'circle' };
                serie['draggableY'] = true;
                break;
            case 'defecto':
                serie['id'] = item.id;
                serie['name'] = item.label;
                serie['data'] = item.data;
                serie['lineWidth'] = 6;
                serie['marker'] = { enabled: true, symbol: 'circle' };
                break;
        }

        if (!(rule.includes(item.hcrender))) series.push(serie);
    });

    highchart['categories'] = categories;
    highchart['series'] = series;
    return highchart;
}

function iniciarCalendarios(model) {
    model.forEach(e => {
        if (e.id == 'intervalos') return;
        const dateValue = (e.label == 'No encontrada')
            ? '' : e.label;
        $(`#${e.id}`).val(dateValue);
    });
}

function iniciarTablaRelaciones(relaciones) {
    dtRelaciones = $('#dtRelaciones').DataTable({
        data: relaciones,
        columns: [
            { title: '', data: null, width: '20px' },
            { title: 'Nombre', data: 'Reltnanom' }
        ],
        columnDefs: [
            {
                targets: 0,
                defaultContent: '<input type="checkbox" class="chkSeleccion" />'
            },            
        ],
        createdRow: function (row, data, index) {
            if (data.Reltnacodi === -1) {//Al crear nueva fila
                const ary = dtRelaciones.rows()
                    .nodes()
                    .toArray();
                ary.forEach(e => {
                    $(e).removeClass('row-selected')
                        .find('.chkSeleccion')
                        .prop('checked', false);
                });

                $(row).addClass('row-selected')
                    .find('.chkSeleccion')
                    .prop('checked', true);

                $('#txtPopNombre').val("");
            }

            $(row).on('click', function () {
                if ($(row).hasClass('row-selected')) {
                    $(row).removeClass('row-selected')
                        .find('.chkSeleccion')
                        .prop('checked', false);
                    $('#selComponenteVegetativa')
                        .multipleSelect('uncheckAll');
                    $('#selPopBarrasCP')
                        .multipleSelect('uncheckAll');
                    htItems.updateSettings({ data: []});
                }
                else {
                    $(row).addClass('row-selected')
                        .find('.chkSeleccion')
                        .prop('checked', true);

                    const relRowData = dtRelaciones.row(row).data();
                    const idFormula = relRowData.Reltnaformula;

                    //Detalle
                    //[0]:IdBarra, [1]:NomBarra, [2]:idFormula, [3]:NomFormula
                    const arrayBarras = relRowData.Detalle.map(e => e[0]);
                    
                    $('#selComponenteVegetativa')
                        .multipleSelect('setSelects', [idFormula]);
                    $('#selPopBarrasCP')
                        .multipleSelect('setSelects', arrayBarras);

                    const dataHtBarras = relRowData.Detalle
                        .map(e => [`${e[1]}[${e[0]}]`, `${e[3]}[${e[2]}]`]);
                    htItems.updateSettings({
                        data: dataHtBarras,
                        columns: ColumnsHandsonBarras()
                    });
                }

                $(row).siblings()
                    .removeClass('row-selected')
                    .find('.chkSeleccion')
                    .prop('checked', false);
            });
        },
        bLengthChange: false,
        bSort: false,
        destroy: true,
        paging: true,
        pageLength: 10,
        info: false,
    });
}

function iniciarHandsonBarras() {
    const contenedor = document.getElementById('htBarras');
    htItems = new Handsontable(contenedor, {
        data: [],
        fillHandle: true,
        width: '100%',
        colWidths: 250,
        minSpareCols: 0,
        minSpareRows: 0,
        columns: ColumnsHandsonBarras()
    });
}

function ColumnsHandsonBarras() {
    return [
        {
            type: 'text',
            title: 'Barras CP',
            readOnly: true,
            className: 'htCenter'
        },
        {
            title: 'Formulas',
            editor: 'dropdown',
            source: formulasPorNombre
        }
    ];
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

function RefillDropDownList(element, data, data_id, data_name) {
    //Vacia el elemento
    element.empty();
    //Carga el elemento
    $.each(data, function (i, item) {
        var n_value = i, n_html = item;
        if (data_id) n_value = item[data_id];
        if (data_name) n_html = item[data_name];
        element.append($('<option></option>').val(n_value).html(n_html));
    });
    //Actualiza la lib.multipleselect
    element.multipleSelect('refresh');
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

function nombreUnidadAsociacion() {
    let nBarra;
    $("#selBarras :selected").each(function (i, sel) {
        nBarra = $(sel).text().trim();
    });

    let val = (nBarra) ? nBarra : true;

    return val;
}

function ValidarEventosHandsontable(evento) {
    var valid = false;
    var ListEventos = ['edit', 'autofill', 'paste', 'grafico'];

    for (var i = 0; i < ListEventos.length; i++) {
        if (evento == ListEventos[i]) valid = true;
    }
    return valid;
}

function recalcularPerfilPatron() {
    const ids = $('.toolHistorico')
        .toArray()
        .map(x => x.id);

    let mediciones = [];
    ids.forEach(x => {
        mediciones
            .push(objHt.getDataAtProp(x));
    });
    
    const numIntervalos = 48;
    let nuevoPatron = Array(numIntervalos)
        .fill(0);

    mediciones.forEach(x => {
        let i = 0;
        while (i < numIntervalos) {
            nuevoPatron[i] += x[i];
            i++;
        }
    });

    const ajuste = objHt.getDataAtProp('ajuste');
    const _base = nuevoPatron
        .map(x => {
            return parseFloat((x / ids.length)
                .toFixed(2));
        });
    const _final = _base
        .map((x, index) => {
            return parseFloat((x + ajuste[index])
                .toFixed(2));
        });

    const _data = HtGetDataAsProp();
    _data.forEach((e, index) => {
        e['base'] = _base[index];
        e['final'] = _final[index];
    });
    objHt.updateSettings({
        data: _data,
    });

    objHc.get('base').setData(_base);
    objHc.get('final').setData(_final);
}