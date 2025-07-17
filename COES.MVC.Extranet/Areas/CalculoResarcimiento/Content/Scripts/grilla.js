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
var arregloNivelTension = [];
var arregloTipoInterrupcion = [];
var arregloCausaInterrupcion = [];
var arregloEmpresa = [];
var arregloIndicadores = [];
var hot = null;
var newHeaders = [];
var modoCorrecion = false;

var colwidths = [
    1,      //- Id
    40,     //- Eliminar
    60,     //- Correlativo
    90,     //- Tipo cliente
    300,    //- Cliente
    300,    //- Punto Entrega
    120,     //- Nro de suministro
    70,     //- Nivel de tension
    80,     //- Aplicación Literal
    120,     //- Energia semestral
    80,     //- Incremento tolerancia
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
    120,     //- Resarcmiento
    80      //- Evidencia
];

var camposTabla = [
    { id: 0, required: false },     //- Id
    { id: 1, required: false },     //- Eliminar
    { id: 2, required: true, correlativo: true, validarMaximo: true },     //- Correlativo (Obligatorio)
    { id: 3, required: true },     //- Tipo cliente (Obligatorio)
    { id: 4, required : true, existe: true },     //- Cliente (Obligatorio)
    { id: 5, required : true },    //- Punto Entrega (Obligatorio)
    { id: 6, required : false, longitud: true, maxlen: 30  },    //- Nro de suministro (Opcional)
    { id: 7, required : true },    //- Nivel de tension (Obligatorio)
    { id: 8, required : true, esEntero: true, rango: true  },     //- Aplicación Literal (Obligatorio)
    { id: 9, required: false, esDecimal: true, countDecimals: true, decimales: 10, countEnteros: true, enteros: 23, esNegativo: true },     //- Energia semestral (Opcional)
    { id: 10, required : true },    //- Incremento tolerancia (Obligatorio)
    { id: 11, required : true },    //- Tipo interrupcion (Obligatorio)
    { id: 12, required : true },    //- Causa interrupcion (Obligatorio)
    { id: 13, required : true, esDecimal: true, rango: true},    //- Ni (Obligatorio)
    { id: 14, required : true, esDecimal: true, rango: true},    //- Ki (Obligatorio)
    { id: 15, required : true, esDate: true },    //- Fecha ini   (Obligatorio)
    { id: 16, required : true, esDate: true },    //- Fecha fin (Obligatorio)
    { id: 17, required : false, esDate: true },   //- Fecha ini   (Opcional)
    { id: 18, required : false, esDate: true },   //- Fecha fin  (Opcional)
    { id: 19, required : true },    //- Empresa    (Obligatorio)
    { id: 20, required : true, esPorcentaje: true },    //- Porcentaje (Obligatorio)
    { id: 21, required : false },    //- Empresa 2   (Opcional)
    { id: 22, required : false, esPorcentaje: true },   //- Porcentaje 2 (Opcional)
    { id: 23, required : false },   //- Empresa 3   (Opcional)
    { id: 24, required : false, esPorcentaje: true },    //- Porcentaje 3 (Opcional)
    { id: 25, required : false },   //- Empresa  4   (Opcional)
    { id: 26, required : false, esPorcentaje: true },   //- Porcentaje 4 (Opcional)
    { id: 27, required : false },    //- Empresa 5   (Opcional)
    { id: 28, required : false, esPorcentaje: true },   //- Porcentaje 5 (Opcional)
    { id: 29, required : false },    //-Causa resuminda (Opcional)
    { id: 30, required: true, esDecimal: true, countDecimals: true, decimales: 4, countEnteros: true, enteros: 14, esNegativo: true},   //- Ei/E (Obligatorio)
    { id: 31, required: true, esDecimal: true, countDecimals: true, decimales: 4, countEnteros: true, enteros: 14, esNegativo: true },   //- Resarcmiento (Obligatorio)
    { id: 32, required : false, esFile: true }    //- Evidencia (Obligatorio dependiendo)
];

var columnDoble = [15, 17, 19, 21, 23, 25, 27];

var headers = ['', '', 'Correl. P.E.', 'Tipo de Cliente', 'Cliente', 'Punto de Entrega / \n Barra', 'N° de suministro \n cliente libre', 'Niv. Tensión', 'Meses de sum. en el sem.', 'Energía Semestral(kWh)', 'Incr. de Toler.', 'Tipo', 'Causa', 'Ni', 'Ki', 'Tiempo Ejecutado', '', 'Tiempo Programado', '', 'Responsable 1', '', 'Responsable 2', '', 'Responsable 3', '', 'Responsable 4', '', 'Responsable 5', '', 'Causa resumida de interrupción', 'Ei / E', 'Resarcimiento(US$)', 'Evidencia'];
var headersAlterno = ['', '', 'Correlativo por \n Punto de Entrega', 'Tipo de Cliente', 'Cliente', 'Punto de Entrega / \n Barra', 'N° de suministro \n cliente libre', 'Nivel de Tensión', 'Aplicación literal \n e) de numeral 5.2.4 \n Base Metodológica \n (meses de suministro en el semestre)', 'Energía Semestral(kWh)', 'Incremento de tolerancias \n Sector Distribución Típico \n 2(Mercado regulado)', 'Tipo', 'Causa', 'Ni', 'Ki', 'Tiempo Ejecutado - Inicio', 'Tiempo Ejecutado - Fin', 'Tiempo Programado - Inicio', 'Tiempo Programado - Fin', 'Responsable 1', '% Responsable 1', 'Responsable 2', '% Responsable 2', 'Responsable 3', '% Responsable 3', 'Responsable 4', '% Responsable 4', 'Responsable 5', '% Responsable 5', 'Causa resumida de interrupción', 'Ei / E', 'Resarcimiento(US$)', 'Evidencia'];

var dataObj = [
    headers,
    ['', '', '', '', '', '', '', '', '', '', '', '', '', '', '', 'Fecha Hora Inicio', 'Fecha Hora Fin', 'Fecha Hora Inicio', 'Fecha Hora Fin', 'Empresa', '%', 'Empresa', '%', 'Empresa', '%', 'Empresa', '%', 'Empresa', '%', '', '', '', ''],
    ['', '', '', '', '', '', '', '', '', '', '', '', '', '', '', '', '', '', '', '', '', '', '', '', '', '', '', '', '', '', '', '', ''],
    ['', '', '', '', '', '', '', '', '', '', '', '', '', '', '', '(dd/mm/yyyy hh24:mm:ss)', '', '(dd/mm/yyyy hh24:mm:ss)', '', '', '', '', '', '', '', '', '', '', '', '', '', '', ''],
];

function cargarGrillaInterrupcion(result, soloLectura) {
    
    if (hot != null) hot.destroy();

    arregloCliente = [];
    arregloPtoEntrega = [];
    arregloNivelTension = [];
    arregloTipoInterrupcion = [];
    arregloCausaInterrupcion = [];
    arregloEmpresa = [];
    arregloIndicadores = [];
   
    for (var k in result.ListaCliente) {
        arregloCliente.push({ id: result.ListaCliente[k].Emprcodi, text: result.ListaCliente[k].Emprnomb });
    }

    for (var k in result.ListaEmpresa) {
        arregloEmpresa.push({ id: result.ListaEmpresa[k].Emprcodi, text: result.ListaEmpresa[k].Emprnomb });
    }

    for (var k in result.ListaPuntoEntrega) {
        arregloPtoEntrega.push({ id: result.ListaPuntoEntrega[k].Repentcodi, text: result.ListaPuntoEntrega[k].Repentnombre, idNT: result.ListaPuntoEntrega[k].Rentcodi });
    }

    for (var k in result.ListaNivelTension) {
        arregloNivelTension.push({ id: result.ListaNivelTension[k].Rentcodi, text: result.ListaNivelTension[k].Rentabrev });
    }

    for (var k in result.ListaTipoInterrupcion) {
        arregloTipoInterrupcion.push({ id: result.ListaTipoInterrupcion[k].Retintcodi, text: result.ListaTipoInterrupcion[k].Retintnombre });
    }

    for (var k in result.ListaCausaInterrupcion) {
        arregloCausaInterrupcion.push({ id: result.ListaCausaInterrupcion[k].Recintcodi, text: result.ListaCausaInterrupcion[k].Recintnombre, idTipo: result.ListaCausaInterrupcion[k].Retintcodi });
    }

    $('#hfPlazoEnvio').val(result.PlazoEnvio);

    arregloIndicadores = result.Indicadores;

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

    for (var i = 29; i <= 32; i++) {
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
        rowHeaders: true,
        //viewportRowRenderingOffset: 1000,
        cells: function (row, col, prop) {
            var cellProperties = {};

            // var data = this.instance.getData();

            if (row < 4) {
                cellProperties.renderer = tituloRenderer;
                cellProperties.readOnly = true;
            }

            if (row > 3) {
                //- Eliminar interrupcion
                if (col == 1) {
                    cellProperties.renderer =  deleteRenderer;
                    cellProperties.readOnly = false;
                }
                
                //- Correlativo
                if (col == 2) {
                    
                    if (validarExcelCorrelativoRango(this.instance.getDataAtCell(row,col))) {
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
                   
                    if (this.instance.getDataAtCell(row, col -2) == "R") {
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

                //- Numero de suministro
                if (col == 6) {                    
                    if (validarTexto(this.instance.getDataAtCell(row, col), 30)) {
                        cellProperties.renderer = defaultRenderer;
                        cellProperties.readOnly = false;
                    }
                    else {
                        cellProperties.renderer = errorRenderer;
                        cellProperties.readOnly = false;
                    }
                }
                
                //- Nivel de tension
                if (col == 7) {
                    cellProperties.editor = 'select2';
                    cellProperties.renderer = dropdownNivelTensionRenderer;
                    cellProperties.select2Options = {
                        data: arregloNivelTension,
                        dropdownAutoWidth: true,
                        width: 'resolve',
                        allowClear: false
                    }
                }
                
                //- Aplicacion literal
                if (col == 8) {
                    if (validarExcelEnteroRango(this.instance.getDataAtCell(row, col))) {
                        cellProperties.renderer = defaultRenderer;
                        cellProperties.readOnly = false;
                    }
                    else {
                        cellProperties.renderer = errorRenderer;
                        cellProperties.readOnly = false;
                    }
                }

                //- Energia semestral
                if (col == 9) {
                    if (validarExcelNumeroConDecimalesEnteroPositivo(this.instance.getDataAtCell(row, col), 12, 10)) {
                        cellProperties.renderer = defaultRenderer;
                        cellProperties.readOnly = false;
                    }
                    else {
                        cellProperties.renderer = errorRenderer;
                        cellProperties.readOnly = false;
                    }
                    if (result.PlazoEnergia == "S") {
                        cellProperties.renderer = disabledRenderer;
                        cellProperties.readOnly = true;
                    }
                }

                //- Incremento de tolerancias
                if (col == 10) {
                    cellProperties.editor = 'select2';
                    cellProperties.renderer = dropdownSiNoRenderer;
                    cellProperties.select2Options = {
                        data: arregloSiNo,
                        dropdownAutoWidth: true,
                        width: 'resolve',
                        allowClear: false
                    }
                }

                //- Tipo interrupcion
                if (col == 11) {
                    cellProperties.editor = 'select2';
                    cellProperties.renderer = dropdownTipoInterrupcionRenderer;
                    cellProperties.select2Options = {
                        data: arregloTipoInterrupcion,
                        dropdownAutoWidth: true,
                        width: 'resolve',
                        allowClear: false
                    }
                }
                
                //- Causa interrupcion
                if (col == 12) {
                    var arregloCausa = arregloCausaInterrupcion;
                    if (this.instance.getDataAtCell(row, col - 1) != "") {
                        arregloCausa = obtenerCausasPorTipo(this.instance.getDataAtCell(row, col - 1));
                    }

                    cellProperties.editor = 'select2';
                    cellProperties.renderer = dropdownCausaInterrupcionRenderer;
                    cellProperties.select2Options = {
                        data: arregloCausa,
                        dropdownAutoWidth: true,
                        width: 'resolve',
                        allowClear: false
                    }
                }

                //- Indicador Ni y Ki
                if (col == 13 || col == 14) {
                    if (validarExcelDecimalRango(this.instance.getDataAtCell(row, col))) {
                        cellProperties.renderer = defaultRenderer;
                        cellProperties.readOnly = false;
                    }
                    else {
                        cellProperties.renderer = errorRenderer;
                        cellProperties.readOnly = false;
                    }
                }

                //- Fechas
                if (col == 15 || col == 16 || col == 17 || col == 18) {
                    if (validarExcelFecha(this.instance.getDataAtCell(row, col), 3)) {
                        cellProperties.renderer = defaultRenderer;
                        cellProperties.readOnly = false;
                    }
                    else {
                        cellProperties.renderer = errorRenderer;
                        cellProperties.readOnly = false;
                    }
                }

                //- Responsables
                if (col == 19 || col == 21 || col == 23 || col == 25 || col == 27) {
                    cellProperties.editor = 'select2';
                    cellProperties.renderer = dropdownEmpresaRenderer;
                    cellProperties.select2Options = {
                        data: arregloEmpresa,
                        dropdownAutoWidth: true,
                        width: 'resolve',
                        allowClear: false
                    }
                }

                //- Porcentajes de responsabilidad
                if (col == 20 || col == 22 || col == 24 || col == 26 || col == 28) {
                    if (validarExcelPorcentaje(this.instance.getDataAtCell(row, col))) {
                        cellProperties.renderer = defaultRenderer;
                        cellProperties.readOnly = false;
                    }
                    else {
                        cellProperties.renderer = errorRenderer;
                        cellProperties.readOnly = false;
                    }
                }

                //- Causa resumida
                if (col == 29) {
                    if (validarTexto(this.instance.getDataAtCell(row, col), 300)) {
                        cellProperties.renderer = defaultRenderer;
                        cellProperties.readOnly = false;
                    }
                    else {
                        cellProperties.renderer = errorRenderer;
                        cellProperties.readOnly = false;
                    }
                }

                //- Ei/E, Resarcimiento
                if (col == 30 || col == 31) {
                    if (validarExcelNumeroConDecimalesPositivo(this.instance.getDataAtCell(row, col), 4)) {
                        cellProperties.renderer = defaultRenderer;
                        cellProperties.readOnly = false;
                    }
                    else {
                        cellProperties.renderer = errorRenderer;
                        cellProperties.readOnly = false;
                    }
                    if (result.PlazoEnergia == "S") {
                        cellProperties.renderer = disabledRenderer;
                        cellProperties.readOnly = true;
                    }
                }
                //- Evidencia
                if (col == 32) {
                    if (this.instance.getDataAtCell(row, col) != null && this.instance.getDataAtCell(row, col) != "") {
                        cellProperties.renderer = openFileRenderer;
                        cellProperties.readOnly = false;
                    }
                    else {
                        cellProperties.renderer = cargarFileRenderer;
                        cellProperties.readOnly = false;
                    }
                }
                
                if (soloLectura) {
                    cellProperties.readOnly = true; //Todas las celdas a solo lectura
                    if (col == 1) {
                        cellProperties.renderer = defaultRenderer; //Quito boton cancelar
                        cellProperties.readOnly = true;
                    }
                    if (col == 32) {
                        cellProperties.renderer = defaultRenderer; //Quito boton archivos
                        cellProperties.readOnly = true;
                    }
                }
                
            }

            return cellProperties;
        },
        afterLoadData: function () {
            this.render();
        },
        afterChange: function (changes, source) {
            if (source == "edit") {

                var filaCambiada = (changes[0])[0];
                var propiedadCambiada = ((changes[0])[1]);
                var valorActual = (changes[0])[2];
                var valorNuevo = (changes[0])[3];
                //this.render();
                if (valorActual != valorNuevo) {
                    if (propiedadCambiada == 5 && hot.getDataAtCell(filaCambiada, 3) == "R") {
                        var idNT = obtenerNivelTension(valorNuevo);
                        hot.setDataAtCell(filaCambiada, 7, idNT);
                    }

                    //- Pintamos los valores de ni y ki
                    if (propiedadCambiada == 12) {
                        setearIndicadores(valorNuevo, filaCambiada, propiedadCambiada);
                    }
                }

                if (modoCorrecion === true) {
                    var validaciones = validarDatos();
                    $('#contenidoError').html(obtenerErroresInterrupciones(validaciones));

                    if (validaciones.length > 0) {
                        mostrarMensaje('mensaje', 'alert', 'Se encontraron errores en los datos ingresados');
                    }
                    else {
                        mostrarMensaje('mensaje', 'exito', 'No se encontraron errores en los datos ingresados');
                        $('#popupErrores').bPopup().close();
                    }
                }

                edicionDatos = true;
            }
        }

    });
    hot.render();
   
    cargarColumnasGrilla(headers);
}

function cargarColumnasGrilla(headers) {
    var html = obtenerColumnas(headers);
    $('#contenedorColumnas').html(html);
    $('#cbSelectAll').click(function (e) {
        var table = $(e.target).closest('table');
        $('td input:checkbox', table).prop('checked', this.checked);
    });
}

function obtenerNivelTension(idPtoEntrega) {
    for (var index = 0; index < arregloPtoEntrega.length; index++) {
        if (parseInt(idPtoEntrega) === arregloPtoEntrega[index].id) {
            return arregloPtoEntrega[index].idNT;
        }
    }
}

function obtenerCausasPorTipo(idTipoInterrupcion) {
    var arreglo = [];
    for (var index = 0; index < arregloCausaInterrupcion.length; index++) {
        if (parseInt(idTipoInterrupcion) === arregloCausaInterrupcion[index].idTipo) {
            arreglo.push(arregloCausaInterrupcion[index]);
        }
    }
    return arreglo;
} 

function setearIndicadores(idCausaInterrupcion, row, col) {   
    for (var index = 0; index < arregloIndicadores.length; index++) {
        if (parseInt(idCausaInterrupcion) === parseInt(arregloIndicadores[index][0])) {
            var ni = arregloIndicadores[index][3];
            var ki = arregloIndicadores[index][4];
            hot.setDataAtCell(row, col + 1, ni);
            hot.setDataAtCell(row, col + 2, ki);
        }
    }
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

var cargarFileRenderer = function (instance, td, row, col, prop, value, cellProperties) {
    var div;
    $(td).children('.btn').remove();
    $(td).html('');
    div = document.createElement('div');
    div.className = 'btn';
    td.style.color = '#fff';
    div.appendChild(document.createTextNode("."));
    td.appendChild(div);
    $(div).on('mouseup', function () {
        showFile(hot.getData()[row][0], row, col, hot.getData()[row][col]);
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
    $(div).on('mouseup', function () {
        showFile(hot.getData()[row][0], row, col, hot.getData()[row][col]);
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
    td.innerHTML = `<div class="truncated">${value}</div>`;
};

var dropdownNivelTensionRenderer = function (instance, td, row, col, prop, value, cellProperties) {
    var selectedId;

    for (var index = 0; index < arregloNivelTension.length; index++) {
        if (parseInt(value) === arregloNivelTension[index].id) {
            selectedId = arregloNivelTension[index].id;
            value = arregloNivelTension[index].text;
        }
    }
    $(td).addClass("estilocombo");
    Handsontable.TextCell.renderer.apply(this, arguments);
    $('#selectedId').text(selectedId);
};

var dropdownTipoInterrupcionRenderer = function (instance, td, row, col, prop, value, cellProperties) {
    var selectedId;

    for (var index = 0; index < arregloTipoInterrupcion.length; index++) {
        if (parseInt(value) === arregloTipoInterrupcion[index].id) {
            selectedId = arregloTipoInterrupcion[index].id;
            value = arregloTipoInterrupcion[index].text;
        }
    }
    $(td).addClass("estilocombo");
    Handsontable.TextCell.renderer.apply(this, arguments);
    $('#selectedId').text(selectedId);
};

var dropdownCausaInterrupcionRenderer = function (instance, td, row, col, prop, value, cellProperties) {
    var selectedId;

    for (var index = 0; index < arregloCausaInterrupcion.length; index++) {
        if (parseInt(value) === arregloCausaInterrupcion[index].id) {
            selectedId = arregloCausaInterrupcion[index].id;
            value = arregloCausaInterrupcion[index].text;
        }
    }
    $(td).addClass("estilocombo");
    Handsontable.TextCell.renderer.apply(this, arguments);
    $('#selectedId').text(selectedId);
};

var dropdownEmpresaRenderer = function (instance, td, row, col, prop, value, cellProperties) {
    var selectedId;

    for (var index = 0; index < arregloEmpresa.length; index++) {
        if (parseInt(value) === arregloEmpresa[index].id) {
            selectedId = arregloEmpresa[index].id;
            value = arregloEmpresa[index].text;
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

    for (var i = 4; i < data.length; i++) {

        if (true) {
           
            var porcentaje = 0;
            var flag = true;
            for (var j in camposTabla) {
                //- Validacion de campos obligatorios
                if (camposTabla[j].required) {
                    if (data[i][j] == "" && !camposTabla[j].existe) {
                        validaciones.push({ row: i, col: j, validation: Validacion.DatoObligatorio });
                        flag = false;
                    }
                    if (data[i][j] == "" && camposTabla[j].existe) {
                        validaciones.push({ row: i, col: j, validation: Validacion.DatoObligatorioCliente });
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
                if (data[i][j] != "" || j == 32) {
                    //- Validamos enteros
                    if (camposTabla[j].esEntero) {
                        if (!validarEntero(data[i][j])) {
                            validaciones.push({ row: i, col: j, validation: Validacion.FormatoEntero });
                            flag = false;
                        }
                        else {
                            if (camposTabla[j].rango) {
                                if (!validarEnteroRango(data[i][j])) {
                                    validaciones.push({ row: i, col: j, validation: Validacion.FormatoEnteroRango });
                                    flag = false;
                                }
                            }
                            if (camposTabla[j].longitud) {
                                if (data[i][j].length > camposTabla[j].enteros) {
                                    validaciones.push({ row: i, col: j, validation: Validacion.LongitudEntero });
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
                            if (camposTabla[j].esNegativo) {
                                if (!validarNegativo(data[i][j])) {
                                    validaciones.push({ row: i, col: j, validation: Validacion.FormatoDecimalNegativo });
                                    flag = false;
                                }
                            }
                            if (camposTabla[j].countEnteros) {
                                if (data[i][j].length > camposTabla[j].enteros) {
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

                    //- Validamos porcentajes
                    if (camposTabla[j].esPorcentaje) {
                        if (!validarPorcentaje(data[i][j])) {
                            validaciones.push({ row: i, col: j, validation: Validacion.FormatoPorcentaje });
                            flag = false;
                        }
                        else {
                            porcentaje = porcentaje + parseFloat(data[i][j]);
                        }
                    }

                    //- Validamos archivos adjuntos
                    if (camposTabla[j].esFile) {

                        if (data[i][12] != "") {
                            
                            if (parseInt(data[i][12]) == 7 || parseInt(data[i][12]) == 8 || parseInt(data[i][12]) == 4) {
                                
                                if (data[i][32] == null || data[i][32] == "") {
                                    validaciones.push({ row: i, col: j, validation: Validacion.ArchivoSustentoObligatorio });
                                }
                            }
                        }
                    }
                }
            }

            //- Validamos los valores de Ni y Ki
           
            for (var index = 0; index < arregloIndicadores.length; index++) {
                
                if (parseInt(data[i][12]) === parseInt(arregloIndicadores[index][0])) {
                    var ni = arregloIndicadores[index][3];
                    var ki = arregloIndicadores[index][4];
                    if (parseFloat(ni) != parseFloat(data[i][13])) {
                        validaciones.push({ row: i, col: 13, validation: Validacion.ValorNiIncorrecto });
                    }
                    if (parseFloat(ki) != parseFloat(data[i][14])) {
                        validaciones.push({ row: i, col: 14, validation: Validacion.ValorKiIncorrecto });
                    }
                }
            }

            //- Validamos combinacion de tipo con causa
            if (data[i][11] != "" && data[i][12] != "") {
                for (var k = 0; k < arregloCausaInterrupcion.length; k++) {
                    if (parseInt(data[i][12]) == parseInt(arregloCausaInterrupcion[k].id)) {
                        if (parseInt(data[i][11]) != parseInt(arregloCausaInterrupcion[k].idTipo)){
                            validaciones.push({ row: i, col: 12, validation: Validacion.TipoCausaInterrupcion });
                        }
                    }
                }
            }

            //- Nivel de tensión
            if (data[i][3] == "R") {
                var idNT = obtenerNivelTension(data[i][5]);                
                if (parseInt(idNT) != parseInt(data[i][7])) {
                    validaciones.push({ row: i, col: 7, validation: Validacion.ValorNTIncorrecto });
                }
            }

            if (data[i][3] == "L") {
                if (data[i][5].length > 100) {
                    validaciones.push({ row: i, col: 5, validation: Validacion.TextoPuntoEntrega });
                }
            }

            //- Causa resumida
            if (data[i][29].length > 300) {
                validaciones.push({ row: i, col: 29, validation: Validacion.TextoCausaResumida });
            }

            //- Nro suministro
            if (data[i][6].length > 30) {
                validaciones.push({ row: i, col: 6, validation: Validacion.TextoNroSuministro });
            }

            //- Validamos suma de porcentajes
            if (porcentaje != 100) {
                validaciones.push({ row: i, col: -1, validation: Validacion.SumaPorcentajes });
            }

            //- Validamos que se hayan ingresado porcentajes y empresas
            for (var t = 0; t < 4; t++) {
                if ((data[i][21 + t * 2] == "" && data[i][21 + t * 2 + 1] != "") || (data[i][21 + t * 2] != "" && data[i][21 + t * 2 + 1] == "")) {
                    validaciones.push({ row: i, col: 21 + t * 2, validation: Validacion.IngresoReponsablePorcentaje });
                }
            }

            var ordenResponsable = [];
            //- validamos que los porcentajes se hayan llenado en orden
            for (var t = 0; t < 4; t++) {
                ordenResponsable[t] = 0;
                if ((data[i][21 + t * 2] != "" && data[i][21 + t * 2 + 1] != "")) {
                    ordenResponsable[t] = 1;
                }
            }
            var flagCero = false;
            var flagEmpresaError = false;
            for (var t = 0; t < 4; t++) {
                if (ordenResponsable[t] == 0) {
                    flagCero = true;
                }
                if (flagCero && ordenResponsable[t] == 1) {
                    flagEmpresaError = true;
                }
            }

            if (flagEmpresaError) {
                validaciones.push({ row: i, col: -1, validation: Validacion.IngresoResponsablesOrden });
            }

            //- Validacion de fechas mantenimiento programado
            if (data[i][12] != "") {

                if ((parseInt(data[i][12]) == 1 || parseInt(data[i][12]) == 3 || parseInt(data[i][12]) == 4)) {

                    if (data[i][17] == "" || data[i][18] == "") {
                        validaciones.push({ row: i, col: -1, validation: Validacion.ValidacionFechaProgramada });
                    }
                    else {
                        if (data[i][16] != "") {
                            if (validarFechaHora(data[i][16]) && validarFechaHora(data[i][18])) {
                                if (getDateTime(data[i][16]) > getDateTime(data[i][18])) {
                                    validaciones.push({ row: i, col: 16, validation: Validacion.FechaEjecutadaFinal });
                                }
                            }
                        }
                        if (data[i][15] != "") {
                            if (validarFechaHora(data[i][15]) && validarFechaHora(data[i][17])) {
                                if (getDateTime(data[i][15]) < getDateTime(data[i][17])) {
                                    validaciones.push({ row: i, col: 15, validation: Validacion.FechaEjecutadaInicial });
                                }
                            }
                        }
                    }
                }
                else {
                    if (data[i][15] != "" && data[i][16] != "" && data[i][17] != "" && data[i][18] != "") {
                        if (validarFechaHora(data[i][16]) && validarFechaHora(data[i][18])) {
                            if (getDateTime(data[i][16]) > getDateTime(data[i][18])) {
                                validaciones.push({ row: i, col: 16, validation: Validacion.FechaEjecutadaFinal });
                            }
                        }
                        if (validarFechaHora(data[i][15]) && validarFechaHora(data[i][17])) {
                            if (getDateTime(data[i][15]) < getDateTime(data[i][17])) {
                                validaciones.push({ row: i, col: 15, validation: Validacion.FechaEjecutadaInicial });
                            }
                        }
                    }
                }
            }


            //- Validamos la duración de la interrupcion
            if (data[i][15] != "" && data[i][16] != "") {
                if (validarFechaHora(data[i][15]) && validarFechaHora(data[i][16])) {

                    
                    if (data[i][12] != "" && parseInt(data[i][12]) == 10) {
                        if ((getDateTime(data[i][16]) - getDateTime(data[i][15])) / (1000 * 60) <= 0) {
                            validaciones.push({ row: i, col: 16, validation: Validacion.DuracionEventoPE });
                        }
                    }
                    else {
                        if ((getDateTime(data[i][16]) - getDateTime(data[i][15])) / (1000 * 60) < 3) {
                            validaciones.push({ row: i, col: 16, validation: Validacion.DuracionEvento });
                        }
                    }

                    
                }
            }

            //- Fechas programadas
            if (data[i][17] == "" && data[i][18] != "") {
                validaciones.push({ row: i, col: 17, validation: Validacion.FechaProgramadaInicio });
            }
            if ( data[i][17] != "" && data[i][18] == "") {
                validaciones.push({ row: i, col: 18, validation: Validacion.FechaProgramadaFin });
            }
            if (data[i][17] != "" && data[i][18] != "") {
                if (validarFechaHora(data[i][17]) && validarFechaHora(data[i][18])) {
                    if (getDateTime(data[i][18]) - getDateTime(data[i][17]) <= 0) {
                        validaciones.push({ row: i, col: 18, validation: Validacion.DuracionProgramada });
                    }
                }
            }

            arreglo.push(data[i]);
        }

    }

    //- Validar registros duplicados
    var index = 4;
    var arregloPto = [];
    for (var k = 4; k < data.length; k++) {
        if (data[k][2] != "" && data[k][3] != "" && data[k][4] != "") {
            index = k;
            if (data[k][5] != "") {
                var itemPto = { cliente: data[k][4], pto: data[k][5] };
                //if (!arregloPto.includes(data[k][5])) arregloPto.push(data[k][5]);

                var flagExiste = false;
                for (var t = 0; t < arregloPto.length; t++) {
                    if (itemPto.cliente == arregloPto[t].cliente && itemPto.pto == arregloPto[t].pto) {
                        flagExiste = true;
                    }
                }
                if (!flagExiste) arregloPto.push(itemPto);
            }
        }
    }

   
    for (var i = 4; i < data.length - 1; i++) {
        for (var j = i + 1; j < data.length; j++) {
            if (compararArreglo(data[i], data[j])) {
                validaciones.push({ row: i, col: -1, validation: Validacion.RegistroDuplicado + (j + 1) });
            }
        }
    }

    //- Validar datos por punto de entrega
    for (var t = 0; t < arregloPto.length; t++) {
        //7, 8, 9, 10
        var nt = "";
        var aplicacion = "";
        var energia = "";
        var incremento = "";
        var bandera = false;
        var modelo = 0;

        for (var i = 4; i < data.length; i++) {
            if (arregloPto[t].cliente == data[i][4] && arregloPto[t].pto == data[i][5]) { //- Mismo punto de entrega y cliente

                if (!bandera) {
                    nt = data[i][7];
                    aplicacion = data[i][8];
                    energia = data[i][9];
                    incremento = data[i][10];
                    bandera = true;
                    modelo = i;
                }
                else {

                    if (nt != data[i][7] || aplicacion != data[i][8] || energia != data[i][9] || incremento != data[i][10]) {
                        validaciones.push({ row: i, col: -1, validation: Validacion.ValidacionPtoEntrega + (modelo + 1) });
                    }
                }
            }
        }
    }

    //- Validación de traslapes
    for (var t = 0; t < arregloPto.length; t++) {
        var arregloTiempo = [];
        for (var i = 4; i < data.length; i++) {
            if (arregloPto[t].cliente == data[i][4] && arregloPto[t].pto == data[i][5]) { //- Mismo punto de entrega y cliente
                var itemTiempo = { Inicio: data[i][15], Fin: data[i][16], Indice: i + 1 };
                arregloTiempo.push(itemTiempo);
            }
        }
        

        for (var k = 0; k < arregloTiempo.length - 1; k++) {
            for (var y = k + 1; y < arregloTiempo.length; y++) {
                if (validarFechaHora(arregloTiempo[k].Inicio) && validarFechaHora(arregloTiempo[k].Fin) &&
                    validarFechaHora(arregloTiempo[y].Inicio) && validarFechaHora(arregloTiempo[y].Fin)) {

                    if (!(getDateTime(arregloTiempo[k].Inicio) - getDateTime(arregloTiempo[y].Fin) >= 0 ||
                        getDateTime(arregloTiempo[y].Inicio) - getDateTime(arregloTiempo[k].Fin) >= 0)) {
                        validaciones.push({ row: arregloTiempo[y].Indice - 1, col: -1, validation: Validacion.TraslapeInterrupcion + arregloTiempo[k].Indice });
                    }
                }
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
        html = html + "     <tr style='cursor:pointer' onclick='irCeldaError(" + data[k].row + "," + data[k].col + ")'>";
        html = html + "         <td>" + (data[k].row  + 1)+ "</td>";
        if (data[k].col != -1) {
            html = html + "         <td>" + headersAlterno[data[k].col]+ "</td>";
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

function obtenerErroresExportacion(data) {
    var result = [];
    for (var k in data) {
        var itemData = [];
        itemData.push((data[k].row + 1));
        itemData.push(headersAlterno[data[k].col]);
        itemData.push(data[k].validation);
        result.push(itemData);
    }
    return result;
}

function irCeldaError(row, col) {  
    hot.selectCell(row, col)
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
    for (var i = 0; i <= 32; i++) {
        newRow.push("");
    }
    data.push(newRow);

    hot.updateSettings({
        data: data
    });

    hot.render();
}

function actualizarFile(row, extension) {
    hot.setDataAtCell(row, 32, extension);
    hot.render();   
}

function mostrarErroresCalculo(data) {
    $('#btnExportarErrores').hide();
    var dataGrilla = hot.getData();
    var arreglo = [];
    for (var i = 0; i < data.length; i++) {
        if (data[i][3] == "S") {
            var original = dataGrilla[i + 4];
            arreglo.push({ Fila: i + 4 + 1, EiReporte: original[30], ResarcimientoReporte: original[31], EiSistema: data[i][1], ResarcimientoSistema: data[i][2] });
        }
    }
    var html = "";
    if (arreglo.length > 0) {

        html = "<table class='pretty tabla-adicional' id='tablaErrores'>";
        html = html + " <thead>";
        html = html + "     <tr>";
        html = html + "         <th>Fila</th>";
        html = html + "         <th>Error</th>";
        html = html + "         <th>Ei/E Reportado</th>";
        html = html + "         <th>Resarcimiento Reportado</th>";
        html = html + "         <th>Ei/E Sistema</th>";
        html = html + "         <th>Resarcimiento Sistema</th>";
        html = html + "     </tr>";
        html = html + " </thead>";
        html = html + " <tbody>";
        for (var k in arreglo) {
            html = html + "     <tr style='cursor:pointer' onclick='irCeldaError(" + (parseInt(arreglo[k].Fila) - 1) + "," + 31 + ")'>";
            html = html + "         <td>" + (arreglo[k].Fila) + "</td>";
            html = html + "         <td> Los valores no coinciden</td>";           
            html = html + "         <td>" + arreglo[k].EiReporte + "</td>";
            html = html + "         <td>" + arreglo[k].ResarcimientoReporte + "</td>";
            html = html + "         <td>" + arreglo[k].EiSistema + "</td>";
            html = html + "         <td>" + arreglo[k].ResarcimientoSistema + "</td>";
            html = html + "     </tr>";
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
       
    for (var i = 4; i < data.length; i++) {
        if (data[i][0] == idInterrupcion) {
            cliente = data[i][4];
            ptoEntrega = data[i][5];
            break;
        }
    }

    for (var i = 4; i < data.length; i++) {
        if (
            data[i][4] == cliente &&
            data[i][5] == ptoEntrega &&
            data[i][0] != idInterrupcion
        ) {
            data[i][1] = i;
            arreglo.push(data[i]);
        }
    }

    const columnas = data[0].length;
    for (let i = 0; i < 4; i++) {
        arreglo.unshift(new Array(columnas).fill("")); 
    }

    return arreglo;
}



function mostrarErroresCalculoAnulacion(data) {
    $('#btnExportarErrores').hide();
    var dataGrilla = hot.getData();
    var arreglo = [];
    for (var i = 0; i < data.length; i++) {
        var original = dataGrilla[data[i][3]];
        arreglo.push({ Fila: parseInt(data[i][3]) + 1, EiReporte: original[30], ResarcimientoReporte: original[31], EiSistema: data[i][1], ResarcimientoSistema: data[i][2] });
    }
    var html = "";
    if (arreglo.length > 0) {
        html = "<strong>!Al anular la interrupción se modifican los cálculos de resarcimientos!</strong>";
        html = html + "<div class='popup-title'>Diferencias en los cálculos</div>";
        html = html + "<table class='pretty tabla-adicional' id='tablaRecalculoAnulacion'>";
        html = html + " <thead>";
        html = html + "     <tr>";
        html = html + "         <th>Fila</th>";
        html = html + "         <th>Mensaje</th>";
        html = html + "         <th>Ei/E Reportado</th>";
        html = html + "         <th>Resarcimiento Reportado</th>";
        html = html + "         <th>Ei/E Sistema</th>";
        html = html + "         <th>Resarcimiento Sistema</th>";
        html = html + "     </tr>";
        html = html + " </thead>";
        html = html + " <tbody>";
        for (var k in arreglo) {
            html = html + "     <tr style='cursor:pointer' onclick='irCeldaError(" + (parseInt(arreglo[k].Fila) - 1) + "," + 31 + ")'>";
            html = html + "         <td>" + (arreglo[k].Fila) + "</td>";
            html = html + "         <td> Los valores no coinciden</td>";
            html = html + "         <td>" + arreglo[k].EiReporte + "</td>";
            html = html + "         <td>" + arreglo[k].ResarcimientoReporte + "</td>";
            html = html + "         <td>" + arreglo[k].EiSistema + "</td>";
            html = html + "         <td>" + arreglo[k].ResarcimientoSistema + "</td>";
            html = html + "     </tr>";
        }
        html = html + " </tbody>";
        html = html + "</table>";
    }

    return html;
}