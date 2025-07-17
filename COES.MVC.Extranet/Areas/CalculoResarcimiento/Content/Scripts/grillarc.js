var arregloSiNo = [
    { id: 'S', text: 'Si' },
    { id: 'N', text: 'No' }
];

var arregloTipoCliente = [
    { id: 'L', text: 'Libre' },
    { id: 'R', text: 'Regulado' }
];

var arregloCliente = [];
var arregloPtoEntrega = [];
var arregloEvento = [];
var hot = null;
var newHeaders = [];

var colwidths = [
    1,      //- Id
    40,     //- Eliminar
    80,    //- Correlativo
    120,    //- Tipo cliente
    300,    //- Cliente
    200,    //- Punto Entrega
    220,     //- Alimentador SED
    120,     //- ENST
    200,    //- Evento COES
    200,    //- Comentario
    140,    //- Fecha ini  --
    140,    //- Fecha fin
    140,    //- Pk
    140,    //- Compensable
    140,    //- ENS
    140     //- Resarcimiento
];

var camposTabla = [
    { id: 0, required: false },     //- Id
    { id: 1, required: false },     //- Eliminar
    { id: 2, required: true, correlativo: true, validarMaximo: true },     //- Correlativo (Obligatorio)
    { id: 3, required: true },     //- Tipo cliente (Obligatorio)
    { id: 4, required: true },     //- Cliente (Obligatorio)
    { id: 5, required: true },    //- Punto Entrega (Obligatorio)
    { id: 6, required: true },    //- Alimentador SED
    { id: 7, required: true, esDecimal: true, countDecimals: true, decimales: 10, countEnteros: true, enteros: 23, esNegativo: true },    //-ENST
    { id: 8, required: true },     //- Evento
    { id: 9, required: false },     //- Comentario
    { id: 10, required: true, esDate: true },    //- Fecha ini   (Obligatorio)
    { id: 11, required: true, esDate: true },    //- Fecha fin (Obligatorio)
    { id: 12, required: true, esDecimal: true, countDecimals: true, decimales: 4, countEnteros: true, enteros: 10,  esNegativo: true },   //- Pk
    { id: 13, required: true },     //- Compensable
    { id: 14, required: true, esDecimal: true, countDecimals: true, decimales: 4, countEnteros: true, enteros: 10,  esNegativo: true },   //- Ens
    { id: 15, required: true, esDecimal: true, countDecimals: true, decimales: 4, countEnteros: true, enteros: 17,  esNegativo: true },   //- Resarcmiento
];

var columnDoble = [10
];

var headers = ['', '', 'Correl. R.C', 'Tipo de Cliente', 'Cliente', 'Barra', 'Alimentador/SED', 'ENST f,k(kWh)', 'Evento COES', 'Comentario', 'Tiempo Ejecutado', '', 'Pk (kW)', 'Compensable', 'ENS f, k', 'Resarcimiento'];
var headersAlterno = ['', '', 'Correl. R.C', 'Tipo de Cliente', 'Cliente', 'Barra', 'Alimentador/SED', 'ENST f,k(kWh)', 'Evento COES', 'Comentario', 'Tiempo Ejecutado - Inicio', 'Tiempo Ejecutado - Fin', 'Pk (kW)', 'Compensable', 'ENS f, k', 'Resarcimiento'];

var dataObj = [
    headers,
    ['', '', '', '', '', '', '', '', '', '', 'Fecha Hora Inicio', 'Fecha Hora Fin', '', '', '', '']
   ];

function cargarGrillaInterrupcion(result, soloLectura) {

    arregloCliente = [];
    arregloPtoEntrega = [];
    arregloEvento = [];

    if (hot != null) hot.destroy();

    for (var k in result.ListaCliente) {
        arregloCliente.push({ id: result.ListaCliente[k].Emprcodi, text: result.ListaCliente[k].Emprnomb });
    }

    for (var k in result.ListaPuntoEntrega) {
        arregloPtoEntrega.push({ id: result.ListaPuntoEntrega[k].Repentcodi, text: result.ListaPuntoEntrega[k].Repentnombre});
    }

    for (var k in result.ListaEvento) {
        arregloEvento.push({ id: result.ListaEvento[k].Reevecodi, text: result.ListaEvento[k].Reevedescripcion, fecha: result.ListaEvento[k].FechaEvento });
    }

    $('#hfPlazoEnvio').val(result.PlazoEnvio);

    var merge = [
        { row: 0, col: 10, rowspan: 1, colspan: 2 },

    ];

    for (var i = 0; i <= 9; i++) {
        merge.push({
            row: 0, col: i, rowspan: 2, colspan: 1
        })
    }

    for (var i = 12; i <= 15; i++) {
        merge.push({
            row: 0, col: i, rowspan: 2, colspan: 1
        })
    }

    var grilla = document.getElementById('detalleFormato');

    newHeaders = dataObj[0].slice();
    var dataGrilla = dataObj.slice();
    for (var i in result.Data) {
        dataGrilla.push(result.Data[i]);
    }

    hot = new Handsontable(grilla, {
        data: dataGrilla,
        mergeCells: merge,
        fixedRowsTop: 2,
        height: 400,
        colWidths: colwidths,
        rowHeaders: true,
        cells: function (row, col, prop) {
            var cellProperties = {};

            // var data = this.instance.getData();

            if (row < 2) {
                cellProperties.renderer = tituloRenderer;
                cellProperties.readOnly = true;
            }

            if (row > 1) {
                //- Eliminar interrupcion
                if (col == 1) {
                    cellProperties.renderer = deleteRenderer;
                    cellProperties.readOnly = false;
                }

                //- Correlativo
                if (col == 2) {
                    if (validarExcelCorrelativoRango(this.instance.getDataAtCell(row, col))) {
                        cellProperties.renderer = defaultRenderer;
                        cellProperties.readOnly = false;
                    }
                    else {
                        cellProperties.renderer = errorRenderer;
                        cellProperties.readOnly = false;
                    }
                }

                //- Tipo de cliente
                if (col == 3) {
                    cellProperties.editor = 'select2';
                    cellProperties.renderer = dropdownTipoClienteRenderer;
                    cellProperties.select2Options = {
                        data: arregloTipoCliente,
                        dropdownAutoWidth: true,
                        width: 'resolve',
                        allowClear: false
                    }
                }

                //- Cliente
                if (col == 4) {
                    cellProperties.editor = 'select2';
                    cellProperties.renderer = dropdownClienteRenderer;
                    cellProperties.select2Options = {
                        data: arregloCliente,
                        dropdownAutoWidth: true,
                        width: 'resolve',
                        allowClear: false
                    }
                }

                //- Tipo de entrega
                if (col == 5) {

                    if (this.instance.getDataAtCell(row, col - 2) == "R") {
                        cellProperties.editor = 'select2';
                        cellProperties.renderer = dropdownPuntoEntregaRenderer;
                        cellProperties.select2Options = {
                            data: arregloPtoEntrega,
                            dropdownAutoWidth: true,
                            width: 'resolve',
                            allowClear: false
                        }
                    }
                    else {                        
                        cellProperties.editor = 'text';
                        cellProperties.readOnly = false;

                        if (validarTexto(this.instance.getDataAtCell(row, col), 100)) {
                            cellProperties.renderer = defaultRenderer;
                        }
                        else {
                            cellProperties.renderer = errorRenderer;
                        }
                    }
                }

                //- Alimentador SED
                if (col == 6) {
                    if (validarTexto(this.instance.getDataAtCell(row, col), 300)) {
                        cellProperties.renderer = defaultRenderer;
                        cellProperties.readOnly = false;
                    }
                    else {
                        cellProperties.renderer = errorRenderer;
                        cellProperties.readOnly = false;
                    }
                }

                //- ENST
                if (col == 7) {
                    if (validarExcelNumeroConDecimalesPositivo(this.instance.getDataAtCell(row, col), 10)) {
                        cellProperties.renderer = defaultRenderer;
                        cellProperties.readOnly = false;
                    }
                    else {
                        cellProperties.renderer = errorRenderer;
                        cellProperties.readOnly = false;
                    }                   
                }

                //- Evento COES
                if (col == 8) {
                    cellProperties.editor = 'select2';
                    cellProperties.renderer = dropdownEventoCOESRenderer;
                    cellProperties.select2Options = {
                        data: arregloEvento,
                        dropdownAutoWidth: true,
                        width: 'resolve',
                        allowClear: false
                    }
                }

                //- Comentario
                if (col == 9) {
                    if (validarTexto(this.instance.getDataAtCell(row, col), 300)) {
                        cellProperties.renderer = defaultRenderer;
                        cellProperties.readOnly = false;
                    }
                    else {
                        cellProperties.renderer = errorRenderer;
                        cellProperties.readOnly = false;
                    }
                }

                //- Fechas
                if (col == 10 || col == 11 ) {
                    if (validarExcelFecha(this.instance.getDataAtCell(row, col))) {
                        cellProperties.renderer = defaultRenderer;
                        cellProperties.readOnly = false;
                    }
                    else {
                        cellProperties.renderer = errorRenderer;
                        cellProperties.readOnly = false;
                    }
                }

                //- Compensable
                if (col == 13) {
                    cellProperties.editor = 'select2';
                    cellProperties.renderer = dropdownSiNoRenderer;
                    cellProperties.select2Options = {
                        data: arregloSiNo,
                        dropdownAutoWidth: true,
                        width: 'resolve',
                        allowClear: false
                    }
                }

                //- PK, ENS, Resarcimiento
                if (col == 12 || col == 14 || col == 15) {
                    if (validarExcelNumeroConDecimalesPositivo(this.instance.getDataAtCell(row, col), 4)) {
                        cellProperties.renderer = defaultRenderer;
                        cellProperties.readOnly = false;
                    }
                    else {
                        cellProperties.renderer = errorRenderer;
                        cellProperties.readOnly = false;
                    }
                    if (result.PlazoEnergia == "S") {
                        cellProperties.readOnly = true;
                    }
                }

                if (soloLectura) {
                    cellProperties.readOnly = true; //Todas las celdas a solo lectura
                    if (col == 1) {
                        cellProperties.renderer = defaultRenderer; //Quito boton cancelar
                        cellProperties.readOnly = true;
                    }
                    
                }
            }

            return cellProperties;
        },
        afterLoadData: function () {
            this.render();
        }
    });
    hot.render();
    cargarColumnasGrilla(headers);
}

function getFechaEvento(id) {
    for (var index = 0; index < arregloEvento.length; index++) {
        if (parseInt(id) === arregloEvento[index].id) {
            return arregloEvento[index].fecha;
        }
    }
}

function cargarColumnasGrilla(headers) {
    var html = obtenerColumnas(headers);
    $('#contenedorColumnas').html(html);
    $('#cbSelectAll').click(function (e) {
        var table = $(e.target).closest('table');
        $('td input:checkbox', table).prop('checked', this.checked);
    });
}

var tituloRenderer = function (instance, td, row, col, prop, value, cellProperties) {
    Handsontable.renderers.TextRenderer.apply(this, arguments);
    td.style.fontWeight = 'bold';
    td.style.fontSize = '11px';
    td.style.textAlign = 'center';
    td.style.verticalAlign = 'middle';
    td.style.color = '#fff';
    td.style.backgroundColor = '#4C97C3';
};

var disabledRenderer = function (instance, td, row, col, prop, value, cellProperties) {
    Handsontable.renderers.TextRenderer.apply(this, arguments);
    td.style.backgroundColor = '#F2F2F2';
    td.style.fontSize = '11px';
    td.style.verticalAlign = 'middle';
};

var defaultRenderer = function (instance, td, row, col, prop, value, cellProperties) {
    Handsontable.renderers.TextRenderer.apply(this, arguments);
    td.style.fontSize = '11px';
    td.style.verticalAlign = 'middle';
    td.innerHTML = `<div class="truncated">${value}</div>`;
};

var errorRenderer = function (instance, td, row, col, prop, value, cellProperties) {
    Handsontable.renderers.TextRenderer.apply(this, arguments);
    td.style.fontSize = '11px';
    td.style.backgroundColor = '#FF0000';
    td.style.color = '#fff';
    td.style.verticalAlign = 'middle';
};

var deleteRenderer = function (instance, td, row, col, prop, value, cellProperties) {
    var div;
    $(td).children('.btn').remove();
    $(td).html('');
    div = document.createElement('div');
    div.className = 'btn';
    td.style.color = '#fff';
    div.appendChild(document.createTextNode("."));
    td.appendChild(div);
    $(div).on('mouseup', function () {
        var id = hot.getData()[row][col - 1];

        if (id == "") {
            return instance.alter("remove_row", row);
        }
        else {
            anularInterrupcion(id, row);
        }
    });
    $(td).addClass("estilodelete");
    return td;
}

var dropdownSiNoRenderer = function (instance, td, row, col, prop, value, cellProperties) {
    var selectedId;

    for (var index = 0; index < arregloSiNo.length; index++) {
        if (value === arregloSiNo[index].id) {
            selectedId = arregloSiNo[index].id;
            value = arregloSiNo[index].text;
        }
    }
    $(td).addClass("estilocombo");
    Handsontable.TextCell.renderer.apply(this, arguments);
    $('#selectedId').text(selectedId);
};

var dropdownTipoClienteRenderer = function (instance, td, row, col, prop, value, cellProperties) {
    var selectedId;

    for (var index = 0; index < arregloTipoCliente.length; index++) {
        if (value === arregloTipoCliente[index].id) {
            selectedId = arregloTipoCliente[index].id;
            value = arregloTipoCliente[index].text;
        }
    }
    $(td).addClass("estilocombo");
    Handsontable.TextCell.renderer.apply(this, arguments);
    $('#selectedId').text(selectedId);
};

var dropdownClienteRenderer = function (instance, td, row, col, prop, value, cellProperties) {
    var selectedId;

    for (var index = 0; index < arregloCliente.length; index++) {
        if (parseInt(value) === arregloCliente[index].id) {
            selectedId = arregloCliente[index].id;
            value = arregloCliente[index].text;
        }
    }
    $(td).addClass("estilocombo");
    Handsontable.TextCell.renderer.apply(this, arguments);
    $('#selectedId').text(selectedId);
    td.innerHTML = `<div class="truncated">${value}</div>`;
};

var dropdownPuntoEntregaRenderer = function (instance, td, row, col, prop, value, cellProperties) {
    var selectedId;

    for (var index = 0; index < arregloPtoEntrega.length; index++) {
        if (parseInt(value) === arregloPtoEntrega[index].id) {
            selectedId = arregloPtoEntrega[index].id;
            value = arregloPtoEntrega[index].text;
        }
    }
    $(td).addClass("estilocombo");
    Handsontable.TextCell.renderer.apply(this, arguments);
    $('#selectedId').text(selectedId);
};

var dropdownEventoCOESRenderer = function (instance, td, row, col, prop, value, cellProperties) {
    var selectedId;

    for (var index = 0; index < arregloEvento.length; index++) {
        if (parseInt(value) === arregloEvento[index].id) {
            selectedId = arregloEvento[index].id;
            value = arregloEvento[index].text;
        }
    }
    $(td).addClass("estilocombo");
    Handsontable.TextCell.renderer.apply(this, arguments);
    $('#selectedId').text(selectedId);
};


function aplicarOcultado() {
    limpiarMensaje('mensajeColumna');
    var newWidts = colwidths.slice();

    var count = 0;
    var countHidden = 0;
    $('#tablaColumnas tbody input').each(function () {
        if (!$(this).is(':checked')) {
            newWidts[parseInt($(this).val())] = 1;
            countHidden++;
            if (columnDoble.includes(parseInt($(this).val()))) {
                newWidts[parseInt($(this).val()) + 1] = 1;
            }
            hot.setDataAtCell(0, parseInt($(this).val()), '');
        }
        else {
            hot.setDataAtCell(0, parseInt($(this).val()), newHeaders[parseInt($(this).val())]);
        }
        count++;
    });
    if (count == countHidden) {
        mostrarMensaje('mensajeColumna', 'alert', 'No puede ocultar todas las columnas.');
    }
    else {
        hot.updateSettings({
            colWidths: newWidts,
        });

        $('#popupColumna').bPopup().close();
    }
}

function actualizarDataGrilla(data) {
    var dataGrilla = dataObj.slice();
    for (var i in data) {
        dataGrilla.push(data[i]);
    }
    hot.updateSettings({
        data: dataGrilla
    });
    hot.render();
}

function validarDatos() {
    var data = hot.getData();
    var validaciones = [];
    var arreglo = [];

    for (var i = 2; i < data.length; i++) {

        if (true) {
            var porcentaje = 0;
            var flag = true;
            for (var j in camposTabla) {
                //- Validacion de campos obligatorios
                if (camposTabla[j].required) {
                    if (data[i][j] == "") {
                        validaciones.push({ row: i, col: j, validation: Validacion.DatoObligatorio });
                        flag = false;
                    }
                }

                if (camposTabla[j].correlativo) {
                    if (!validarCorrelativoRango(data[i][j])) {
                        validaciones.push({ row: i, col: j, validation: Validacion.ValidarCorrelativo });
                        flag = false;
                    }
                }

                //- Validacion de formatos
                if (data[i][j] != "") {
                    //- Validamos enteros
                    if (camposTabla[j].esEntero) {
                        if (!validarEntero(data[i][j])) {
                            validaciones.push({ row: i, col: j, validation: Validacion.FormatoEntero });
                            flag = false;
                        }
                        else {
                            if (camposTabla[j].rango) {
                                if (!validarEnteroRango(data[i][j])) {
                                    validaciones.push({ row: i, col: j, validation: Validacion.FormatoEntero });
                                    flag = false;
                                }
                            }
                        }
                    }
                    //- Validamos decimales
                    if (camposTabla[j].esDecimal) {
                        if (!validarNumero(data[i][j])) {
                            validaciones.push({ row: i, col: j, validation: Validacion.FormatoNumero });
                            flag = false;
                        }
                        else {
                            if (camposTabla[j].rango) {
                                if (!validarRangoDecimal(data[i][j])) {
                                    validaciones.push({ row: i, col: j, validation: Validacion.FormatoDecimalRango });
                                    flag = false;
                                }
                            }
                            if (camposTabla[j].countDecimals) {
                                if (!validarCantidadDecimales(data[i][j], camposTabla[j].decimales)) {
                                    validaciones.push({ row: i, col: j, validation: Validacion.FormatoDecimal + camposTabla[j].decimales });
                                    flag = false;
                                }
                            }
                            if (camposTabla[j].countEnteros) {
                                if (contarEnteros(data[i][j]) > camposTabla[j].enteros) {
                                    validaciones.push({ row: i, col: j, validation: Validacion.LongitudDecimal });
                                    flag = false;
                                }
                            }
                        }
                    }

                    //- Validamos fechas
                    if (camposTabla[j].esDate) {
                        if (!validarFechaHora(data[i][j])) {
                            validaciones.push({ row: i, col: j, validation: Validacion.FormatoFechaHora });
                            flag = false;
                        }
                    }

                }
            }

            if (data[i][3] == "L") {
                if (data[i][5].length > 100) {
                    validaciones.push({ row: i, col: 5, validation: Validacion.TextoPuntoEntrega });
                }
            }


            //- Alimentador SED
            if (data[i][6] != "" && data[i][6].length > 200) {
                validaciones.push({ row: i, col: 6, validation: Validacion.TextoLongitudAlimentadorSED });
            }

            //- Comentario
            if (data[i][9] != "" && data[i][9].length > 300) {
                validaciones.push({ row: i, col: 9, validation: Validacion.TextoLongitudComentario });
            }
                      

            //- Validamos la duración de la interrupcion
            if (data[i][10] != "" || data[i][11] != "") {
                if (validarFechaHora(data[i][11]) && validarFechaHora(data[i][10])) {
                    if ((getDateTime(data[i][11]) - getDateTime(data[i][10])) / (1000 * 60) <= 0) {
                        validaciones.push({ row: i, col: 11, validation: Validacion.DuracionEventoRC });
                    }
                    else if ((getDateTime(data[i][11]) - getDateTime(data[i][10])) / (1000 * 60) < 3) {
                        if (parseFloat(data[i][15]) != 0) {
                            validaciones.push({ row: i, col: 15, validation: Validacion.ResarcimimientoRCCero });
                        }
                    }

                }
            }

            //- Valimos que la fecha con fecha de evento
            if (data[i][10] != "" && data[i][8] != "") {
                if (validarFechaHora(data[i][10])) {
                    var fechaEvento = getFechaEvento(data[i][8]);
                    var fechaRechazo = getDatePart(data[i][10]);

                    if (getFecha(fechaEvento) != getFecha(fechaRechazo)) {
                        validaciones.push({ row: i, col: 10, validation: Validacion.FechaInicioEvento });
                    }
                }
            }

            arreglo.push(data[i]);
        }

    }

    //- Validar registros duplicados
    var index = 2;
    for (var k = 2; k < data.length; k++) {
        if (data[k][2] != "" && data[k][3] != "" && data[k][4] != "") {
            index = k;            
        }
    }
    index = index + 1;
    for (var i = 2; i < index - 1; i++) {
        for (var j = i + 1; j < index; j++) {
            if (compararArreglo(data[i], data[j])) {
                validaciones.push({ row: i, col: -1, validation: Validacion.RegistroDuplicado + (j + 1) });
            }
        }
    }

    return validaciones;
}

function obtenerErroresInterrupciones(data) {
    var html = "<table class='pretty tabla-adicional' id='tablaErrores'>";
    html = html + " <thead>";
    html = html + "     <tr>";
    html = html + "         <th>Fila</th>";
    html = html + "         <th>Columna</th>";
    html = html + "         <th>Error</th>";
    html = html + "     </tr>";
    html = html + " </thead>";
    html = html + " <tbody>";
    for (var k in data) {
        html = html + "     <tr>";
        html = html + "         <td>" + (data[k].row + 1) + "</td>";
        if (data[k].col != -1) {
            html = html + "         <td>" + headersAlterno[data[k].col] + "</td>";
        }
        else {
            html = html + "         <td></td>";
        }
        html = html + "         <td>" + data[k].validation + "</td>";
        html = html + "     </tr>";
    }
    html = html + " </tbody>";
    html = html + "</table>";

    return html;
}

function getData() {
    return hot.getData();
}

function getSize() {
    return hot.getData().length;
}

function eliminarFilaGrilla(row) {
    hot.alter('remove_row', row);
}

function agregarFilaGrilla() {
    var data = hot.getData();
    var newRow = [];
    for (var i = 0; i <= 15; i++) {
        newRow.push("");
    }
    data.push(newRow);

    hot.updateSettings({
        data: data
    });

    hot.render();
}



function mostrarErroresCalculo(arregloData) {
   
    var html = "";
    if (arregloData.length > 0) {

        html = "<table class='pretty tabla-adicional' id='tablaErrores'>";
        html = html + " <thead>";
        html = html + "     <tr>";
        html = html + "         <th>Fila</th>";
        html = html + "         <th>Error</th>";
        html = html + "         <th>Resarcimiento Reportado</th>";      
        html = html + "         <th>Resarcimiento Sistema</th>";
        html = html + "     </tr>";
        html = html + " </thead>";
        html = html + " <tbody>";
        var index = 3;
        for (var k = 0; k < arregloData.length; k++) {

            if (arregloData[k][3] == "S") {
                html = html + "     <tr>";
                html = html + "         <td>" + index + "</td>";
                html = html + "         <td> Los valores no coinciden</td>";
                html = html + "         <td>" + arregloData[k][1] +  "</td>";
                html = html + "         <td>" + arregloData[k][2] + "</td>";
                html = html + "     </tr>";
            }
            if (arregloData[k][4] == "S") {
                html = html + "     <tr>";
                html = html + "         <td>" + index + "</td>";
                html = html + "         <td>Los valores de ENTS deben ser iguales si cliente, barra y evento son iguales.</td>";
                html = html + "         <td>-</td>";
                html = html + "         <td>-</td>";
                html = html + "     </tr>";
            }

            index = index + 1;
        }
        html = html + " </tbody>";
        html = html + "</table>";
    }

    return html;
}



function obtenerDatosAnulacion(idInterrupcion) {

    var data = hot.getData();
    var arreglo = [];
    var cliente = "";
    var ptoEntrega = "";
    var evencodi = "";
    var alimentadorSED = "";

    for (var i = 2; i < data.length; i++) {
        if (data[i][0] == idInterrupcion) {
            cliente = data[i][4];
            ptoEntrega = data[i][5];
            evencodi = data[i][8];
            alimentadorSED = data[i][6];
            break;
        }
    }

    for (var i = 2; i < data.length; i++) {
        if (
            (
                (data[i][4] == cliente && data[i][5] == ptoEntrega && data[i][8] == evencodi) ||
                (data[i][4] == cliente && data[i][6] == alimentadorSED)
            )
            && data[i][0] != idInterrupcion
        ) {
            data[i][1] = i;
            arreglo.push(data[i]);
        }
    }

    const columnas = data[0].length;
    for (let i = 0; i < 2; i++) {
        arreglo.unshift(new Array(columnas).fill(""));
    }

    return arreglo;
}



function mostrarErroresCalculoAnulacion(arregloData) {

    var html = "";    
    if (arregloData.length > 0) {
        html = "<strong>!Al anular la interrupción se modifican los cálculos de resarcimientos!</strong>";
        html = html + "<div class='popup-title'>Diferencias en los cálculos</div>";
        html = html + "<table class='pretty tabla-adicional' id='tablaRecalculoAnulacion'>";
        html = html + " <thead>";
        html = html + "     <tr>";
        html = html + "         <th>Fila</th>";
        html = html + "         <th>Mensaje</th>";
        html = html + "         <th>Resarcimiento Reportado</th>";
        html = html + "         <th>Resarcimiento Sistema</th>";
        html = html + "     </tr>";
        html = html + " </thead>";
        html = html + " <tbody>";
        var index = 3;
        for (var k = 0; k < arregloData.length; k++) {            
            html = html + "     <tr>";
            html = html + "         <td>" + (parseInt(arregloData[k][3]) + 1) + "</td>";
            html = html + "         <td> Los valores no coinciden</td>";
            html = html + "         <td>" + arregloData[k][1] + "</td>";
            html = html + "         <td>" + arregloData[k][2] + "</td>";
            html = html + "     </tr>";           

            index = index + 1;
        }
        html = html + " </tbody>";
        html = html + "</table>";
    }

    return html;

}