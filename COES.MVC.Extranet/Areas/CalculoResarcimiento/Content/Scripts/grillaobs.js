var arregloSiNo = [
    { id: 'S', text: 'Si' },
    { id: 'N', text: 'No' }
];

var arregloTipoObservacion = [];
var hot = null;
var newHeaders = [];

var colwidths = [
    80,      //- Id
    250,    //- Suministrador
    80,    //- Correlativo
    100,    //- Tipo cliente
    300,    //- Cliente
    300,    //- Punto Entrega
    120,     //- Nro de suministro
    70,     //- Nivel de tension
    80,    //- Aplicación Literal
    120,     //- Energia semestral
    80,    //- Incremento tolerancia
    210,    //- Tipo interrupcion
    350,    //- Causa interrupcion
    40,     //- Ni
    40,     //- Ki
    140,    //- Fecha ini  --
    140,    //- Fecha fin
    140,    //- Fecha ini  --
    140,    //- Fecha fin
    220,    //- Empresa    --
    60,     //- Porcentaje
    220,    //- Empresa    --
    60,     //- Porcentaje 
    220,    //- Empresa    --
    60,     //- Porcentaje
    220,    //- Empresa    --
    60,     //- Porcentaje
    220,    //- Empresa    --
    60,     //- Porcentaje
    300,    //-Causa resuminda
    80,     //- Ei/E
    120,    //- Resarcmiento
    80,     //- Evidencia
    120,    //- Conformidad responsable
    300,    //- Observacion responsable
    150,    //- Detalle campo observado responsable
    150,    //- Comentaro responsable
    80      //- Evidencia responsable 
];

var camposTabla = [
    { id: 0, required: false },     //- Id
    { id: 1, required: false },     //- Suministrador
    { id: 2, required: false },     //- Correlativo (Obligatorio)
    { id: 3, required: false },     //- Tipo cliente (Obligatorio)
    { id: 4, required: false },     //- Cliente (Obligatorio)
    { id: 5, required: false },     //- Punto Entrega (Obligatorio)
    { id: 6, required: false },     //- Nro de suministro (Opcional)
    { id: 7, required: false },     //- Nivel de tension (Obligatorio)
    { id: 8, required: false },     //- Aplicación Literal (Obligatorio)
    { id: 9, required: false },     //- Energia semestral (Opcional)
    { id: 10, required: false },    //- Incremento tolerancia (Obligatorio)
    { id: 11, required: false },    //- Tipo interrupcion (Obligatorio)
    { id: 12, required: false },    //- Causa interrupcion (Obligatorio)
    { id: 13, required: false },    //- Ni (Obligatorio)
    { id: 14, required: false },    //- Ki (Obligatorio)
    { id: 15, required: false },    //- Fecha ini   (Obligatorio)
    { id: 16, required: false },    //- Fecha fin (Obligatorio)
    { id: 17, required: false },    //- Fecha ini   (Opcional)
    { id: 18, required: false },    //- Fecha fin  (Opcional)
    { id: 19, required: false },    //- Empresa    (Obligatorio)
    { id: 20, required: false },    //- Porcentaje (Obligatorio)
    { id: 21, required: false },    //- Empresa 2   (Opcional)
    { id: 22, required: false },    //- Porcentaje 2 (Opcional)
    { id: 23, required: false },    //- Empresa 3   (Opcional)
    { id: 24, required: false },    //- Porcentaje 3 (Opcional)
    { id: 25, required: false },    //- Empresa  4   (Opcional)
    { id: 26, required: false },    //- Porcentaje 4 (Opcional)
    { id: 27, required: false },    //- Empresa 5   (Opcional)
    { id: 28, required: false },    //- Porcentaje 5 (Opcional)
    { id: 29, required: false },    //-Causa resuminda (Opcional)
    { id: 30, required: false },    //- Ei/E (Obligatorio)
    { id: 31, required: false },    //- Resarcmiento (Obligatorio)
    { id: 32, required: false },     //- Evidencia (Obligatorio dependiendo)
    { id: 33, required: false },     //- Conformidad responsable
    { id: 34, required: false },    //- Observacion responsable
    { id: 35, required: false },    //- Detalle campo observado
    { id: 36, required: false },    //- Comentario reponsable
    { id: 37, required: false }     //- Evidencia responsable
];

var columnDoble = [15, 17, 19, 21, 23, 25, 27];

var headers = ['ID', 'Suministrador', 'Correl. P.E', 'Tipo de Cliente', 'Cliente', 'Punto de Entrega / \n Barra', 'N° de suministro \n cliente libre', 'Niv. Tensión', 'Meses de sum.  en el sem.', 'Energía Semestral(kWh)', 'Incr. de Toler.', 'Tipo', 'Causa', 'Ni', 'Ki', 'Tiempo Ejecutado', '', 'Tiempo Programado', '', 'Responsable 1', '', 'Responsable 2', '', 'Responsable 3', '', 'Responsable 4', '', 'Responsable 5', '', 'Causa resumida de interrupción', 'Ei / E', 'Resarcimiento(US$)', 'Evidencia', 'Conformidad responsable', 'Observación', 'Detalle del campo observado', 'Comentarios', 'Evidencia'];
var headersAlterno = ['', 'Suministrador', 'Correlativo por \n Punto de Entrega', 'Tipo de Cliente', 'Cliente', 'Punto de Entrega / \n Barra', 'N° de suministro \n cliente libre', 'Nivel de Tensión', 'Aplicación literal \n e) de numeral 5.2.4 \n Base Metodológica \n (meses de suministro en el semestre)', 'Energía Semestral(kWh)', 'Incremento de tolerancias \n Sector Distribución Típico \n 2(Mercado regulado)', 'Tipo', 'Causa', 'Ni', 'Ki', 'Tiempo Ejecutado - Inicio', 'Tiempo Ejecutado - Fin', 'Tiempo Programado - Inicio', 'Tiempo Programado - Fin', 'Responsable 1', '% Responsable 1', 'Responsable 2', '% Responsable 2', 'Responsable 3', '% Responsable 3', 'Responsable 4', '% Responsable 4', 'Responsable 5', '% Responsable 5', 'Causa resumida de interrupción', 'Ei / E', 'Resarcimiento(US$)', 'Evidencia', 'Conformidad responsable', 'Observación', 'Detalle del campo observado', 'Comentarios', 'Evidencia'];

var dataObj = [
    headers,
    ['', '', '', '', '', '', '', '', '', '', '', '', '', '', '', 'Fecha Hora Inicio', 'Fecha Hora Fin', 'Fecha Hora Inicio', 'Fecha Hora Fin', 'Empresa', '%', 'Empresa', '%', 'Empresa', '%', 'Empresa', '%', 'Empresa', '%', '', '', '', '', '','','','',''],
    ['', '', '', '', '', '', '', '', '', '', '', '', '', '', '', '', '', '', '', '', '', '', '', '', '', '', '', '', '', '', '', '', '', '', '', '', '', ''],
    ['', '', '', '', '', '', '', '', '', '', '', '', '', '', '', '(dd/mm/yyyy hh24:mm:ss)', '', '(dd/mm/yyyy hh24:mm:ss)', '', '', '', '', '', '', '', '', '', '', '', '', '', '', '', '', '', '', '', ''],
];

function cargarGrillaInterrupcion(result) {

    arregloTipoObservacion = [];

    if (hot != null) hot.destroy();

    for (var k in result.ListaTiposObservacion) {
        arregloTipoObservacion.push({ id: result.ListaTiposObservacion[k].Id, label: result.ListaTiposObservacion[k].Texto });
    }     

    $('#hfPlazoEnvio').val(result.PlazoEnvio);

    var merge = [
        { row: 0, col: 15, rowspan: 1, colspan: 2 },
        { row: 0, col: 17, rowspan: 1, colspan: 2 },
        { row: 0, col: 19, rowspan: 1, colspan: 2 },
        { row: 0, col: 21, rowspan: 1, colspan: 2 },
        { row: 0, col: 23, rowspan: 1, colspan: 2 },
        { row: 0, col: 25, rowspan: 1, colspan: 2 },
        { row: 0, col: 27, rowspan: 1, colspan: 2 },
        { row: 3, col: 15, rowspan: 1, colspan: 2 },
        { row: 3, col: 15, rowspan: 1, colspan: 2 },
        { row: 1, col: 15, rowspan: 2, colspan: 1 },
        { row: 1, col: 16, rowspan: 2, colspan: 1 },
        { row: 3, col: 17, rowspan: 1, colspan: 2 },
        { row: 1, col: 17, rowspan: 2, colspan: 1 },
        { row: 1, col: 18, rowspan: 2, colspan: 1 },
        { row: 1, col: 19, rowspan: 3, colspan: 1 },
        { row: 1, col: 20, rowspan: 3, colspan: 1 },
        { row: 1, col: 21, rowspan: 3, colspan: 1 },
        { row: 1, col: 22, rowspan: 3, colspan: 1 },
        { row: 1, col: 23, rowspan: 3, colspan: 1 },
        { row: 1, col: 24, rowspan: 3, colspan: 1 },
        { row: 1, col: 25, rowspan: 3, colspan: 1 },
        { row: 1, col: 26, rowspan: 3, colspan: 1 },
        { row: 1, col: 27, rowspan: 3, colspan: 1 },
        { row: 1, col: 28, rowspan: 3, colspan: 1 }
    ];

    for (var i = 0; i <= 14; i++) {
        merge.push({
            row: 0, col: i, rowspan: 4, colspan: 1
        })
    }

    for (var i = 29; i <= 37; i++) {
        merge.push({
            row: 0, col: i, rowspan: 4, colspan: 1
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
        fixedRowsTop: 4,
        height: 400,
        colWidths: colwidths,
        maxRows: dataGrilla.length,
        rowHeaders: true,
        cells: function (row, col, prop) {
            var cellProperties = {};

            //var data = this.instance.getData();

            if (row < 4) {
                if (col < 33) {
                    cellProperties.renderer = tituloRenderer;
                    cellProperties.readOnly = true;
                }
                else {
                    cellProperties.renderer = tituloObservacionRenderer;
                    cellProperties.readOnly = true;
                }
            }

            if (row > 3) {

                var trimestral = false;
                if (this.instance.getDataAtCell(row, 0).split("-")[2] == "S") {
                    trimestral = true;
                }

                //- Evidencia interrupcion
                if (col == 32) {
                    if (this.instance.getDataAtCell(row, col) != null && this.instance.getDataAtCell(row, col) != "") {
                        cellProperties.renderer = showFileRenderer;
                        cellProperties.readOnly = false;

                        if (trimestral == true) {
                            cellProperties.renderer = showFileRenderer1;
                            cellProperties.readOnly = false;
                        }
                    }
                    else {
                        cellProperties.renderer = disabledRendererTexto;
                        cellProperties.readOnly = true;

                        if (trimestral == true) {
                            cellProperties.renderer = trimestralRenderer;
                            cellProperties.readOnly = true;
                        }
                    }
                }
                
                //- Conformidad responsable
                if (col == 33) {
                    cellProperties.editor = 'select2';
                    cellProperties.renderer = dropdownSiNoRenderer;
                    cellProperties.select2Options = {
                        data: arregloSiNo,
                        dropdownAutoWidth: true,
                        width: 'resolve',
                        allowClear: false
                    }

                    if (trimestral == true) {
                        hot.getCell(row, col).style.backgroundColor = "yellow";
                        cellProperties.readOnly = true;
                    }
                }

                //- Observacion responsable
                if (col == 34 && this.instance.getDataAtCell(row, col -1) == "N") {
                    cellProperties.renderer = customDropdownRenderer;
                    cellProperties.editor = "chosen";
                    cellProperties.chosenOptions = {};
                    cellProperties.chosenOptions.multiple = true;
                    cellProperties.chosenOptions.data = arregloTipoObservacion;
                    cellProperties.readOnly = false;


                    if (trimestral == true) {
                        hot.getCell(row, col).style.backgroundColor = "yellow";
                        cellProperties.readOnly = true;
                    }
                }
                else {
                    if (col == 34) {
                        cellProperties.renderer = disabledRenderer;
                        cellProperties.readOnly = true;
                        dataGrilla[row][col] = '';
                    }
                }

                //- Detalle campo observador, Comentario responsable
                if ((col == 35 && this.instance.getDataAtCell(row, col - 2) == "N") || (col == 36 && this.instance.getDataAtCell(row, col-3) == "N")) {
                    if (validarTexto(this.instance.getDataAtCell(row, col), 300)) {
                        cellProperties.renderer = defaultRenderer;
                        cellProperties.readOnly = false;
                    }
                    else {
                        cellProperties.renderer = errorRenderer;
                        cellProperties.readOnly = false;
                    }

                    if (trimestral == true) {
                        hot.getCell(row, col).style.backgroundColor = "yellow";
                        cellProperties.readOnly = true;
                    }
                }
                else {
                    if (col == 35 || col == 36) {
                        cellProperties.renderer = disabledRenderer;
                        cellProperties.readOnly = true;
                        dataGrilla[row][col] = '';
                    }
                }

                //- Evidencia responsable
                if (col == 37 && this.instance.getDataAtCell(row, col - 4) == "N") {
                    if (this.instance.getDataAtCell(row, col) != null && this.instance.getDataAtCell(row, col) != "") {
                        if (trimestral != true) {
                            cellProperties.renderer = openFileRenderer;
                            cellProperties.readOnly = false;
                        }
                        else {
                            cellProperties.renderer = showFileDetalleRenderer;
                            cellProperties.readOnly = false;
                        }
                    }
                    else {
                        cellProperties.renderer = cargarFileRenderer;
                        cellProperties.readOnly = false;

                        if (trimestral == true) {
                            cellProperties.renderer = trimestralRenderer;
                            cellProperties.readOnly = true;
                        }
                    }
                   
                }
                else {
                    if (col == 37) {
                        cellProperties.renderer = disabledRenderer;
                        cellProperties.readOnly = true;
                    }
                }
             

                if (col < 32) {
                    cellProperties.renderer = disabledRenderer;
                    cellProperties.readOnly = true;

                    if (trimestral == true) {
                        cellProperties.renderer = trimestralRenderer;
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

       hot.addHook('afterOnCellMouseDown', function (row, col) {
        const dropdown = document.querySelector('.chosen-container');
        const handsontableContainer = document.querySelector('.handsontable-container');

        if (dropdown && handsontableContainer) {
            // Ajustar la posición del dropdown dinámicamente
            const rect = handsontableContainer.getBoundingClientRect();
            dropdown.style.top = `${rect.top}px`;
            dropdown.style.left = `${rect.left}px`;
            dropdown.style.zIndex = '9999';
        }
    });
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

var tituloObservacionRenderer = function (instance, td, row, col, prop, value, cellProperties) {
    Handsontable.renderers.TextRenderer.apply(this, arguments);
    td.style.fontWeight = 'bold';
    td.style.fontSize = '11px';
    td.style.textAlign = 'center';
    td.style.verticalAlign = 'middle';
    td.style.color = '#fff';
    td.style.backgroundColor = '#FF9900';
};

var disabledRenderer = function (instance, td, row, col, prop, value, cellProperties) {
    Handsontable.renderers.TextRenderer.apply(this, arguments);
    td.style.backgroundColor = '#F2F2F2';
    td.style.fontSize = '11px';
    td.style.verticalAlign = 'middle';
    td.title = value;
    td.innerHTML = `<div class="truncated">${value}</div>`;
};

var disabledRendererTexto = function (instance, td, row, col, prop, value, cellProperties) {
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

var trimestralRenderer = function (instance, td, row, col, prop, value, cellProperties) {
    Handsontable.renderers.TextRenderer.apply(this, arguments);
    td.style.backgroundColor = 'yellow';
    td.style.fontSize = '11px';
    td.style.verticalAlign = 'middle';
};

var cargarFileRenderer = function (instance, td, row, col, prop, value, cellProperties) {
    var div;
    $(td).children('.btn').remove();
    $(td).html('');
    div = document.createElement('div');
    div.className = 'btn';
    td.style.color = '#fff';
    div.appendChild(document.createTextNode("."));
    td.appendChild(div);
    $(td).on('mouseup', function () {
        showFile(hot.getData()[row][0].split("-")[1], row, col, hot.getData()[row][col]);
    });
    $(td).addClass("estilofolder");
    return td;
}

var openFileRenderer = function (instance, td, row, col, prop, value, cellProperties) {
    var div;
    $(td).children('.btn').remove();
    $(td).html('');
    div = document.createElement('div');
    div.className = 'btn';
    td.style.color = '#fff';
    div.appendChild(document.createTextNode("."));
    td.appendChild(div);
    $(td).on('mouseup', function () {
        showFile(hot.getData()[row][0].split("-")[1], row, col, hot.getData()[row][col]);
    });
    $(td).addClass("openfolder");
    return td;
}

var showFileRenderer = function (instance, td, row, col, prop, value, cellProperties) {
    var div;
    $(td).children('.btn').remove();
    $(td).html('');
    div = document.createElement('div');
    div.className = 'btn';
    td.style.color = '#fff';
    td.style.backgroundColor = '#F2F2F2';
    div.appendChild(document.createTextNode("."));
    td.appendChild(div);
    $(div).on('mouseup', function () {
        showFileInterrupcion(hot.getData()[row][0].split("-")[0], row, col, hot.getData()[row][col]);
    });
    $(td).addClass("openfolder");
    return td;
}

var showFileRenderer1 = function (instance, td, row, col, prop, value, cellProperties) {
    var div;
    $(td).children('.btn').remove();
    $(td).html('');
    div = document.createElement('div');
    div.className = 'btn';
    td.style.color = '#fff';
    td.style.backgroundColor = 'yellow';
    div.appendChild(document.createTextNode("."));
    td.appendChild(div);
    $(div).on('mouseup', function () {
        showFileInterrupcion(hot.getData()[row][0].split("-")[0], row, col, hot.getData()[row][col]);
    });
    $(td).addClass("openfolder");
    return td;
}

var showFileDetalleRenderer = function (instance, td, row, col, prop, value, cellProperties) {
    var div;
    $(td).children('.btn').remove();
    $(td).html('');
    div = document.createElement('div');
    div.className = 'btn';
    td.style.color = '#fff';
    td.style.backgroundColor = 'yellow';
    div.appendChild(document.createTextNode("."));
    td.appendChild(div);
    $(td).on('mouseup', function () {
        showFileInterrupcionDetalle(hot.getData()[row][0].split("-")[1], row, col, hot.getData()[row][col]);
    });
    $(td).addClass("openfolder");
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

var dropdownTipoObservacionRenderer = function (instance, td, row, col, prop, value, cellProperties) {
    var selectedId;

    for (var index = 0; index < arregloTipoObservacion.length; index++) {
        if (value === arregloTipoObservacion[index].id) {
            selectedId = arregloTipoObservacion[index].id;
            value = arregloTipoObservacion[index].text;
        }
    }
    $(td).addClass("estilocombo");
    Handsontable.TextCell.renderer.apply(this, arguments);
    $('#selectedId').text(selectedId);
    td.style.fontSize = '11px';
    td.innerHTML = `<div class="truncated">${value}</div>`;
};


function customDropdownRenderer(instance, td, row, col, prop, value, cellProperties) {
    var selectedId;
    var optionsList = cellProperties.chosenOptions.data;

    if (typeof optionsList === "undefined" || typeof optionsList.length === "undefined" || !optionsList.length) {
        Handsontable.TextCell.renderer(instance, td, row, col, prop, value, cellProperties);
        return td;
    }

    var values = (value + "").split(",");
    value = [];
    for (var index = 0; index < optionsList.length; index++) {

        if (values.indexOf(optionsList[index].id + "") > -1) {
            selectedId = optionsList[index].id;
            value.push(optionsList[index].label);
        }
    }
    value = value.join(", ");
    Handsontable.TextCell.renderer(instance, td, row, col, prop, value, cellProperties);
    td.style.fontSize = '11px';
    td.innerHTML = `<div class="truncated">${value}</div>`;

    return td;
}

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
    var hotData = hot.getData();
    
    for (var i = 0; i < data.length; i++) {
        for (j = 0; j < 4; j++) {
            hotData[4 + i][ 33 + j] = data[i][j + 1];
        }
    }

    hot.updateSettings({
        data: hotData
    });
    hot.render();

}

function validarDatos() {
    var data = hot.getData();
    var validaciones = [];

    var arreglo = [];

    for (var i = 4; i < data.length; i++) {

        if (data[i][0] != "") {
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
            }

            if (data[i][33] != "" && data[i][33] == "N" ) {
                if (data[i][34] == "") {
                    validaciones.push({ row: i, col: 34, validation: Validacion.DatoObligatorio });
                }

                if (data[i][35] == "") {
                    validaciones.push({ row: i, col: 35, validation: Validacion.DatoObligatorio });
                }                
            }

            //- Detalle de campo observado
            if (data[i][35] != "") {
                if (data[i][35].length > 300) {
                    validaciones.push({ row: i, col: 35, validation: Validacion.TextoCausaResumida });
                }
            }

            //- Comentarios
            if (data[i][36] != "") {
                if (data[i][36].length > 300) {
                    validaciones.push({ row: i, col: 36, validation: Validacion.TextoCausaResumida });
                }
            }

            //- Validamos archivos adjuntos
            if (data[i][34] != "") {

                const numeros = [1, 2];
                console.log(data[i][34]);
                const resultado = contieneNumeros(data[i][34], numeros);

                if ((resultado === true) && data[i][37] == "") {
                    validaciones.push({ row: i, col: 37, validation: Validacion.ArchivoSustentoObligatorio });
                }
            }

            if (data[i][33] == "S") {
                if (data[i][34] != "") {
                    validaciones.push({ row: i, col: 34, validation: Validacion.DatosInnecesarios });
                }
                if (data[i][35] != "") {
                    validaciones.push({ row: i, col: 35, validation: Validacion.DatosInnecesarios });
                }
                if (data[i][36] != "") {                   
                    validaciones.push({ row: i, col: 36, validation: Validacion.DatosInnecesarios });
                }
            }

            arreglo.push(data[i]);
        }
    }

    return validaciones;
}

function contieneNumeros(stringNumeros, numerosABuscar) {  
    const numerosArray = stringNumeros.split(',').map(num => parseInt(num.trim(), 10));
    return numerosABuscar.some(numero => numerosArray.includes(numero));
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

function actualizarFile(row, extension) {
    hot.setDataAtCell(row, 37, extension);
    hot.render();
}