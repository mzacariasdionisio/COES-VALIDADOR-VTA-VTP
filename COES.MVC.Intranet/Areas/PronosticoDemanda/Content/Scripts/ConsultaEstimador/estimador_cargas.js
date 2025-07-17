var objHt, objHc, objPopup;
var dtSeleccionados, dtDisponibles, dtGrupos;
var datosUnidadesDisponibles = [];
var datosPrevFiltros = {};

$(document).ready(function () {
    $('.toolHistorico').Zebra_DatePicker({
        onSelect: function () {
            const m = {
                id: this.attr('id'),
                fecha: this.val()
            };
            if (verificarFiltros()) {
                actualizarMedicion(m);
            }
            else {
                SetMessage('#message',
                    'Falta seleccionar algun filtro...',
                    'error', true);
            } 
        }
    });

    $('.filtroSimple').each(function () {
        let element = this;
        $(element).multipleSelect({
            name: element.id,
            filter: true,
            single: true,
            placeholder: 'Seleccione',
            onOpen: function () {
                datosPrevFiltros['idUnidad'] = $('#idUnidad').val();
                datosPrevFiltros['idCentral'] = $('#idCentral').val();
                datosPrevFiltros['idVariable'] = $('#idVariable').val();
                datosPrevFiltros['idFuente'] = $('#idFuente').val();
            },
            onClose: function () {
                const e = document.getElementById(this.name);
                if ($(`#${e.id}`).val() == datosPrevFiltros[e.id]) return false;

                if (e.id == 'idUnidad')
                    $('#idCentral').multipleSelect('uncheckAll');
                if (e.id == 'idCentral')
                    $('#idUnidad').multipleSelect('uncheckAll');

                obtenerMediciones();
            }
        });
    });

    $('.filtroSimple2').each(function () {
        let element = this;
        $(element).multipleSelect({
            name: element.id,
            filter: false,
            single: true,
            onOpen: function () {
                datosPrevFiltros['idUnidad'] = $('#idUnidad').val();
                datosPrevFiltros['idCentral'] = $('#idCentral').val();
                datosPrevFiltros['idVariable'] = $('#idVariable').val();
                datosPrevFiltros['idFuente'] = $('#idFuente').val();
            },
            onClose: function () {
                const e = document.getElementById(this.name);
                if ($(`#${e.id}`).val() == datosPrevFiltros[e.id]) return false;

                obtenerMediciones();
            }
        });
    });

    $('#btnAgruparCargas').on('click', function () {
        objPopup = $('#popup-cargas').bPopup({
            easing: 'easeOutBack',
            speed: 350,
            transition: 'fadeIn',
            modalClose: false,
            onClose: function () {
                $('#txtPopNombre').val('');

                dtGrupos.data()
                    .clear()
                    .draw();
                dtSeleccionados.data()
                    .clear()
                    .draw();
                dtDisponibles.data()
                    .clear()
                    .draw();
            }
        }, function () {
            generarAsociacion();
        });
    });

    $('#btnExportar').on('click', function () {
        const myData = objHt.getData();
        const myHeaders = objHt.getColHeader();

        Exportar(myData, myHeaders);
    });

    $('#btnRegistrarAsociacion').on('click', function () {
        const data = dtGrupos.rows()
            .data()
            .toArray();
        registrarAsociacion(data);
    });

    $('#btnPopNuevo').on('click', function () {
        const nombre = $('#txtPopNombre').val();
        if (!nombre) {
            SetMessage('#pop-alert',
                'El campo nombre es obligatorio',
                'warning', true);
            return false;
        }

        const seleccionados = dtSeleccionados.rows()
            .data()
            .map(e => e.Ptomedicodi)
            .toArray();
        if (seleccionados.length == 0) {
            SetMessage('#pop-alert',
                'Debe seleccionar al menos una unidad ',
                'warning', true);
            return false;
        }

        //validacion para que no se repita el nombre
        let existe = false;
        dtGrupos.rows()
            .data()
            .map(e => {
                if (e.Asocianom == nombre.trim()) {
                    existe = true;;
                }
            });

        if (existe) {
            SetMessage('#pop-alert',
                'Ya existe un registro con ese nombre!',
                'warning', true);
            return false;
        }

        dtGrupos.rows
            .add([{
                Asociacodi: -1,
                Asocianom: nombre,
                Detalle: seleccionados
            }])
            .draw();

        SetMessage('#pop-alert',
            'Registro creado',
            'success', true);
    });

    $('#btnPopActualizar').on('click', function () {
        const row = $('#dtGrupos .row-selected').toArray();
        if (row.length == 0) return false;

        const seleccionados = dtSeleccionados.rows()
            .data()
            .map(e => e.Ptomedicodi)
            .toArray();
        if (seleccionados.length == 0) {
            SetMessage('#pop-alert',
                'Debe seleccionar al menos una unidad ',
                'warning', true);
            return false;
        }

        dtGrupos.row(row)
            .data()
            .Detalle = seleccionados;

        SetMessage('#pop-alert',
            'Registro actualizado',
            'success', true);
    });

    $('#btnPopEliminar').on('click', function () {
        const row = $('#dtGrupos .row-selected').toArray();
        if (row.length == 0) return false;

        dtGrupos.row(row)
            .remove()
            .draw();
        dtSeleccionados.data()
            .clear()
            .draw();
        datosUnidadesDisponibles.forEach(e => e['selected'] = false);
        dtDisponibles.data()
            .clear();
        dtDisponibles.rows
            .add(datosUnidadesDisponibles)
            .draw();

        SetMessage('#pop-alert',
            'Registro eliminado',
            'success', true);
    });

    obtenerMediciones();
});

function Exportar(datos, headers) {
    let val = nombreUnidadAsociacion();
    if (val == true) {
        SetMessage('#message', 'Debe seleccionar una agrupacion o carga...', 'error', true);
    } else {  
        $.ajax({
        type: 'POST',
        url: controller + 'Exportar',
        contentType: 'application/json; charset=utf-8',
        data: JSON.stringify({
            form: datos,
            header: headers,
            nombre: val
        }),
        datatype: 'json',
        traditional: true,
        success: function (result) {
            if (result != -1) {
                window.location = controller + 'abrirarchivo?formato=' + 1 + '&file=' + result;
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

function registrarAsociacion(datos) {
    $.ajax({
        type: 'POST',
        url: controller + 'RegistrarAsociacion',
        contentType: 'application/json; charset=utf-8',
        data: JSON.stringify({
            modulo: $('#main').data('mdl'),
            datos: datos
        }),
        datatype: 'json',
        traditional: true,
        success: function (result) {
            RefillDropDownList($('#idCentral'), result, 'Asociacodi', 'Asocianom');
            SetMessage('#message',
                'Las agrupaciones se registraron exitosamente!',
                'success', true);
        },
        error: function () {
            alert('Ha ocurrido un error...');
        },
        complete: function () {
            objPopup.close();
        }
    });
}

function generarAsociacion() {
    $.ajax({
        type: 'POST',
        url: controller + 'GenerarAsociacion',
        contentType: 'application/json; charset=utf-8',
        data: JSON.stringify({
            modulo: 'carga'
        }),
        datatype: 'json',
        traditional: true,
        success: function (modelo) {
            datosUnidadesDisponibles = modelo.ListaUnidadesEstimador;

            iniciarTablaGrupos(modelo);
            iniciarTablaDisponibles(modelo);
            iniciarTablaSeleccionados();
        },
        error: function () {
            alert('Ha ocurrido un error...');
        }
    });
}

function obtenerMediciones() {
    const unidad = $('#idUnidad').val();
    const asociacion = $('#idCentral').val();
    const variable = $('#idVariable').val();

    const clasHistorico = $('.toolHistorico')
        .toArray()
        .filter(e => (e.value == "") ? false : true);
    const selHistoricos = clasHistorico
        .map(e => {
            const [name, index] = e.id.split('med');
            return `${parseInt(index) - 1}:${e.value}`;
        });

    $.ajax({
        type: 'POST',
        url: controller + 'ObtenerMediciones',
        contentType: 'application/json; charset=utf-8',
        data: JSON.stringify({
            idUnidad: (unidad) ? unidad : -1,
            idAsociacion: (asociacion) ? asociacion : -1,
            idVariable: (variable) ? variable : -1,
            idFuente: $('#idFuente').val(),
            selHistoricos: selHistoricos,
            modulo: $('#main').data('mdl')
        }),
        datatype: 'json',
        traditional: true,
        success: function (result) {
            console.log(result, '_result');

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
            alert('Ha ocurrido un error...');
        }
    });
}

function actualizarMedicion(medicion) {
    const unidad = $('#idUnidad').val();
    const asociacion = $('#idCentral').val();

    $.ajax({
        type: 'POST',
        url: controller + 'ActualizarMedicion',
        contentType: 'application/json; charset=utf-8',
        data: JSON.stringify({
            idUnidad: (unidad) ? unidad : -1,
            idAsociacion: (asociacion) ? asociacion : -1,
            idVariable: $('#idVariable').val(),
            idFuente: $('#idFuente').val(),
            fecha: medicion.fecha,
            modulo: $('#main').data('mdl')
        }),
        datatype: 'json',
        traditional: true,
        success: function (modelo) {
            const test = modelo;

            objHc.get(medicion.id).
                setData(test);
            objHc.get(medicion.id)
                .update({ name: medicion.fecha });

            const htData = HtGetDataAsProp();
            htData.forEach((e, index) => {
                e[medicion.id] = test[index];
            });

            let htHeader = objHt.getColHeader();
            const colIndex = objHt.propToCol(medicion.id);
            htHeader[colIndex] = medicion.fecha;
            objHt.updateSettings({
                data: htData,
                colHeaders: htHeader
            });
        },
        error: function () {
            alert('Ha ocurrido un error...');
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
        colHeaders: model.colHeaders
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

function iniciarTablaGrupos(modelo) {
    dtGrupos = $('#dtGrupos').DataTable({
        data: modelo.ListaAsociacionDetalle,
        columns: [
            { title: '', data: null, width: '20px' },
            { title: 'Agrupaciones', data: 'Asocianom' }
        ],
        columnDefs: [
            {
                targets: 0,
                defaultContent: '<input type="checkbox" class="chkSeleccion" />'
            }
        ],
        createdRow: function (row, data, index) {
            if (data.Asociacodi === -1) {
                const ary = dtGrupos.rows()
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
                    $(row).removeClass('row-selected').
                        find('.chkSeleccion')
                        .prop('checked', false);

                    dtSeleccionados.data().clear()
                        .draw();
                    dtDisponibles.data().clear();
                    datosUnidadesDisponibles.forEach(e => e['selected'] = false);
                    dtDisponibles.rows
                        .add(datosUnidadesDisponibles)
                        .draw();
                }
                else {
                    $(row).addClass('row-selected')
                        .find('.chkSeleccion')
                        .prop('checked', true);

                    dtSeleccionados.data().clear();
                    const grupoRowData = dtGrupos.row(row).data();
                    const seleccionadosData = datosUnidadesDisponibles
                        .filter(e => grupoRowData.Detalle.includes(e.Ptomedicodi));
                    seleccionadosData.forEach(e => e['selected'] = true);
                    dtSeleccionados.rows
                        .add(seleccionadosData)
                        .draw();

                    dtDisponibles.data().clear();
                    const seleccionadosIds = seleccionadosData
                        .map(e => e.Ptomedicodi);

                    const disponiblesData = datosUnidadesDisponibles
                        .filter(e => (seleccionadosIds.includes(e.Ptomedicodi)) ? false : true);
                    disponiblesData.forEach(e => e['selected'] = false);
                    dtDisponibles.rows
                        .add(disponiblesData)
                        .draw();
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
        searching: false
    });
}

function iniciarTablaSeleccionados() {
    dtSeleccionados = $('#dtSeleccionados').DataTable({
        data: [],
        columns: [
            {
                title: '',
                data: null,
                width: '20px'
            },
            { title: 'Cargas Seleccionadas', data: 'Ptomedidesc' },
        ],
        columnDefs: [
            {
                targets: 0,
                createdCell: (td, cellData, rowData, row, col) => {
                    if (rowData.selected)
                        return $(td).html(
                            '<input type="checkbox" class="chkSeleccion" checked />'
                        );
                    else
                        return $(td).html(
                            '<input type="checkbox" class="chkSeleccion" />'
                        );
                }
            }
        ],
        createdRow: function (row, data, index) {
            const inputElement = $(row).find('input')[0];
            $(inputElement).on('click', function () {
                dtDisponibles.rows
                    .add([{
                        Ptomedicodi: data.Ptomedicodi,
                        Ptomedidesc: data.Ptomedidesc,
                        selected: false
                    }])
                    .draw();

                dtSeleccionados.row(row)
                    .remove()
                    .draw();
            });
        },
        bLengthChange: false,
        bSort: false,
        destroy: true,
        paging: true,
        pageLength: 10,
        info: false
    });
}

function iniciarTablaDisponibles(modelo) {
    dtDisponibles = $('#dtDisponibles').DataTable({
        data: modelo.ListaUnidadesEstimador,
        columns: [
            {
                title: '',
                data: null,
                width: '20px'
            },
            { title: 'Cargas Disponibles', data: 'Ptomedidesc' },
        ],
        columnDefs: [
            {
                targets: 0,
                createdCell: (td, cellData, rowData, row, col) => {
                    if (rowData.selected)
                        return $(td).html(
                            '<input type="checkbox" class="chkSeleccion" checked />'
                        );
                    else
                        return $(td).html(
                            '<input type="checkbox" class="chkSeleccion" />'
                        );
                }
            }
        ],
        createdRow: function (row, data, index) {
            const inputElement = $(row).find('input')[0];
            $(inputElement).on('click', function () {
                dtSeleccionados.rows
                    .add([{
                        Ptomedicodi: data.Ptomedicodi,
                        Ptomedidesc: data.Ptomedidesc,
                        selected: true
                    }])
                    .draw();

                dtDisponibles.row(row)
                    .remove()
                    .draw();
            });
        },
        bLengthChange: false,
        bSort: false,
        destroy: true,
        paging: true,
        pageLength: 10,
        info: false
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

function verificarFiltros() {
    const unidad = $('#idUnidad').val();
    const asociacion = $('#idCentral').val();
    const variable = $('#idVariable').val();

    let valUnidad = (unidad) ? true : false;
    let valAsociacion = (asociacion) ? true : false;
    let valVariable = (variable) ? true : false;
    console.log(valUnidad, 'unidad');
    console.log(valAsociacion, 'asociacion');
    console.log(valVariable, 'variable');
    if ((valUnidad || valAsociacion) && valVariable) {
        return true;
    }
    else {
        return false;
    }
}

function nombreUnidadAsociacion() {
    let nUnidad, nAsociacion, nombre;
    $("#idUnidad :selected").each(function (i, sel) {
        nUnidad = $(sel).text().trim();
    });
    $("#idCentral :selected").each(function (i, sel) {
        nAsociacion = $(sel).text().trim();
    });

    nombre = (nUnidad) ? nUnidad : nAsociacion;
    let val = (nombre) ? nombre : true;

    return val;
}
