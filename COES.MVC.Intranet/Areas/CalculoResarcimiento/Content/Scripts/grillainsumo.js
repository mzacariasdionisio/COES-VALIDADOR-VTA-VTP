var arregloCliente = [];
var arregloPtoEntrega = [];
var arregloTipoInterrupcion = [];
var arregloCausaInterrupcion = [];
var arregloSuministradores = [];
var arregloEmpresa = [];
var hotInterrupciones = null;
var newHeaders = [];

var colwidths = [
    60,     //-0 Id    
    300,    //-1 Punto Entrega
    140,    //-2 Fecha ejecutado ini  --
    140,    //-3 Fecha ejecutado fin
    180,    //-4 Tipo de Interrupción
    180,    //-5 Causa Interrupcion
    180,    //-6 Codigo OSI
    300,    //-7 Cliente
    300,    //-8 Suministrador
    250,    //-9 Observacion
    220,    //-10 Empresa 1  --
    60,     //-11 Porcentaje 1
    220,    //-12 Empresa 2   --
    60,     //-13 Porcentaje 2 
    220,    //-14 Empresa 3   --
    60,     //-15 Porcentaje 3
    220,    //-16 Empresa 4   --
    60,     //-17 Porcentaje 4
    220,    //-18 Empresa 5   --
    60,     //-19 Porcentaje 5
    140,    //-20 Fecha programado ini  --
    140,    //-21 Fecha programado fin
    250,    //-22 Tipo interrupcion
    350     //-23 Causa interrupcion
];

var camposTabla = [
    { id: 0, required: true },                          //-1 Id (Obligatorio)
    { id: 5, required: true },                          //-2 Punto Entrega (Obligatorio)
    { id: 1, required: true, esDate: true },            //-3 Fecha ini   (Obligatorio)
    { id: 2, required: true, esDate: true },            //-4 Fecha ini   (Obligatorio)
    { id: 0, required: true },                          //-5 Tipo de Interrupción (Obligatorio)
    { id: 0, required: true },                          //-5 Causa Interrupcion (Obligatorio)
    { id: 0, required: false },                         //-6 Codigo OSI
    { id: 4, required: true },            //-7 Cliente
    { id: 0, required: true },                          //-8 Suministrador
    { id: 29, required: false },                        //-9 Observacion
    { id: 19, required: false },                         //- Empresa    (Obligatorio)
    { id: 20, required: false, esPorcentaje: true },     //- Porcentaje (Obligatorio)
    { id: 21, required: false },                        //- Empresa 2   (Opcional)
    { id: 22, required: false, esPorcentaje: true },    //- Porcentaje 2 (Opcional)
    { id: 23, required: false },                        //- Empresa 3   (Opcional)
    { id: 24, required: false, esPorcentaje: true },    //- Porcentaje 3 (Opcional)
    { id: 25, required: false },                        //- Empresa  4   (Opcional)
    { id: 26, required: false, esPorcentaje: true },    //- Porcentaje 4 (Opcional)
    { id: 27, required: false },                        //- Empresa 5   (Opcional)
    { id: 28, required: false, esPorcentaje: true },    //- Porcentaje 5 (Opcional)
    { id: 17, required: false, esDate: true },          //- Fecha ini   (Opcional)
    { id: 18, required: false, esDate: true },          //- Fecha fin  (Opcional)
    { id: 11, required: true },                         //- Tipo interrupcion (Obligatorio)
    { id: 12, required: true }                          //- Causa interrupcion (Obligatorio)
];

var columnDoble = [10, 12, 14, 16, 18];

var headers = ['Id', ' Punto de Entrega', 'Inicio de interrupción', 'Fin de interrupción', 'Tipo de Interrupción', 'Causa de Interrupción', 'Codosi', 'Cliente', 'Suministrador', 'Observación', 'Responsable 1', '', 'Responsable 2', '', 'Responsable 3', '', 'Responsable 4', '', 'Responsable 5', '', 'Tiempo Programado Inicio', 'Tiempo Programado Fin', 'Tipo de Interrupción Aplicativo', 'Causa de Interrupción Aplicativo'];
var headersAlterno = ['Id', ' Punto de Entrega', 'Inicio de interrupción', 'Fin de interrupción', 'Tipo de Interrupción', 'Causa de Interrupción', 'Codosi', 'Cliente', 'Suministrador', 'Observación', 'Responsable 1', '% Responsable 1', 'Responsable 2', '% Responsable 2', 'Responsable 3', '% Responsable 3', 'Responsable 4', '% Responsable 4', 'Responsable 5', '% Responsable 5', 'Tiempo Programado Inicio', 'Tiempo Programado Fin', 'Tipo de Interrupción Aplicativo', 'Causa de Interrupción Aplicativo'];

var dataObj = [
    headers,
    ['', '', '', '', '', '', '', '', '', '', 'Empresa', '%', 'Empresa', '%', 'Empresa', '%', 'Empresa', '%', 'Empresa', '%', '', '', '', '']
  ];

function cargarGrillaInterrupcion(result) {

    if (hotInterrupciones != null) hotInterrupciones.destroy();

    arregloCliente = [];
    arregloPtoEntrega = [];    
    arregloTipoInterrupcion = [];
    arregloCausaInterrupcion = [];
    arregloEmpresa = [];
    arregloSuministradores = [];

    for (var k in result.ListaCliente) {
        arregloCliente.push({ id: result.ListaCliente[k].Emprcodi, text: result.ListaCliente[k].Emprnomb });
    }

    for (var k in result.ListaEmpresa) {
        arregloEmpresa.push({ id: result.ListaEmpresa[k].Emprcodi, text: result.ListaEmpresa[k].Emprnomb });
    }

    for (var k in result.ListaPuntoEntrega) {
        arregloPtoEntrega.push({ id: result.ListaPuntoEntrega[k].Repentcodi, text: result.ListaPuntoEntrega[k].Repentnombre, idNT: result.ListaPuntoEntrega[k].Rentcodi });
    }       

    for (var k in result.ListaTipoInterrupcion) {
        arregloTipoInterrupcion.push({ id: result.ListaTipoInterrupcion[k].Retintcodi, text: result.ListaTipoInterrupcion[k].Retintnombre });
    }

    for (var k in result.ListaCausaInterrupcion) {
        arregloCausaInterrupcion.push({ id: result.ListaCausaInterrupcion[k].Recintcodi, text: result.ListaCausaInterrupcion[k].Recintnombre, idTipo: result.ListaCausaInterrupcion[k].Retintcodi });
    }

    for (var k in result.ListaSuministradores) {
        arregloSuministradores.push({ id: result.ListaSuministradores[k].Emprcodi, text: result.ListaSuministradores[k].Emprnomb });
    }

    var merge = [
        { row: 0, col: 10, rowspan: 1, colspan: 2 },
        { row: 0, col: 12, rowspan: 1, colspan: 2 },
        { row: 0, col: 14, rowspan: 1, colspan: 2 },
        { row: 0, col: 16, rowspan: 1, colspan: 2 },
        { row: 0, col: 18, rowspan: 1, colspan: 2 }        
    ];

    for (var i = 0; i <= 9; i++) {
        merge.push({
            row: 0, col: i, rowspan: 2, colspan: 1
        })
    }

    for (var i = 20; i <= 23; i++) {
        merge.push({
            row: 0, col: i, rowspan: 2, colspan: 1
        })
    }

    var grilla = document.getElementById('contenedorGrillaInterrupcion');

    newHeaders = dataObj[0].slice();
    var dataGrilla = dataObj.slice();
    for (var i in result.Data) {
        dataGrilla.push(result.Data[i]);
    }

    hotInterrupciones = new Handsontable(grilla, {
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
                
                //- Correlativo
                if (col == 0) {

                    if (validarExcelEntero(this.instance.getDataAtCell(row, col))) {
                        cellProperties.renderer = defaultRenderer;
                        cellProperties.readOnly = false;
                    }
                    else {
                        cellProperties.renderer = errorRenderer;
                        cellProperties.readOnly = false;
                    }
                }

                //- punto de entrega
                if (col == 1) {
                    cellProperties.editor = 'select2';
                    cellProperties.renderer = dropdownPuntoEntregaRenderer;
                    cellProperties.select2Options = {
                        data: arregloPtoEntrega,
                        dropdownAutoWidth: true,
                        width: 'resolve',
                        allowClear: false
                    }
                }

                //- Fechas
                if (col == 2 || col == 3 || col == 20 || col == 21) {
                    if (validarExcelFecha(this.instance.getDataAtCell(row, col), 3)) {
                        cellProperties.renderer = defaultRenderer;
                        cellProperties.readOnly = false;
                    }
                    else {
                        cellProperties.renderer = errorRenderer;
                        cellProperties.readOnly = false;
                    }
                }


                //- Cliente
                if (col == 7) {
                    cellProperties.editor = 'select2';
                    cellProperties.renderer = dropdownClienteRenderer;
                    cellProperties.select2Options = {
                        data: arregloCliente,
                        dropdownAutoWidth: true,
                        width: 'resolve',
                        allowClear: false
                    }
                }

                //- suministrador
                if (col == 8) {
                    cellProperties.editor = 'select2';
                    cellProperties.renderer = dropdownSuministradorRenderer;
                    cellProperties.select2Options = {
                        data: arregloSuministradores,
                        dropdownAutoWidth: true,
                        width: 'resolve',
                        allowClear: false
                    }
                }

                //- Tipo, causa
                if (col == 4 || col == 5) {
                    if (validarTextoInsumo(this.instance.getDataAtCell(row, col), 100)) {
                        cellProperties.renderer = defaultRenderer;
                        cellProperties.readOnly = false;
                    }
                    else {
                        cellProperties.renderer = errorRenderer;
                        cellProperties.readOnly = false;
                    }
                }

                //- codiosi
                if (col == 6) {
                    if (validarTextoInsumo(this.instance.getDataAtCell(row, col), 20)) {
                        cellProperties.renderer = defaultRenderer;
                        cellProperties.readOnly = false;
                    }
                    else {
                        cellProperties.renderer = errorRenderer;
                        cellProperties.readOnly = false;
                    }
                }

                //- observacion
                if (col == 9) {
                    if (validarTextoInsumo(this.instance.getDataAtCell(row, col), 100)) {
                        cellProperties.renderer = defaultRenderer;
                        cellProperties.readOnly = false;
                    }
                    else {
                        cellProperties.renderer = errorRenderer;
                        cellProperties.readOnly = false;
                    }
                }

                //- Tipo interrupcion
                if (col == 22) {
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
                if (col == 23) {
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

                //- Responsables
                if (col == 10 || col == 12 || col == 14 || col == 16 || col == 18) {
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
                if (col == 11 || col == 13 || col == 15 || col == 17 || col == 19) {
                    if (validarExcelPorcentaje(this.instance.getDataAtCell(row, col))) {
                        cellProperties.renderer = defaultRenderer;
                        cellProperties.readOnly = false;
                    }
                    else {
                        cellProperties.renderer = errorRenderer;
                        cellProperties.readOnly = false;
                    }
                }
                
            }

            return cellProperties;
        },
        afterLoadData: function () {
            this.render();
        }
    });
    hotInterrupciones.render();
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
};

var errorRenderer = function (instance, td, row, col, prop, value, cellProperties) {
    Handsontable.renderers.TextRenderer.apply(this, arguments);
    td.style.fontSize = '11px';
    td.style.backgroundColor = '#FF0000';
    td.style.color = '#fff';
    td.style.verticalAlign = 'middle';
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

var dropdownSuministradorRenderer = function (instance, td, row, col, prop, value, cellProperties) {
    var selectedId;

    for (var index = 0; index < arregloSuministradores.length; index++) {
        if (parseInt(value) === arregloSuministradores[index].id) {
            selectedId = arregloSuministradores[index].id;
            value = arregloSuministradores[index].text;
        }
    }
    $(td).addClass("estilocombo");
    Handsontable.TextCell.renderer.apply(this, arguments);
    $('#selectedId').text(selectedId);
};

function actualizarDataGrilla(data) {
    var dataGrilla = dataObj.slice();
    for (var i in data) {
        dataGrilla.push(data[i]);
    }
    hotInterrupciones.updateSettings({
        data: dataGrilla
    });
    hotInterrupciones.render();
}

function validarDatosInterrupcion() {
    var data = hotInterrupciones.getData();
    var validaciones = [];

    var arreglo = [];

    for (var i = 2; i < data.length; i++) {

        if (true) {
            var porcentaje = 0;
            var validaPorcentaje = false;
            var flag = true;
            for (var j in camposTabla) {
                //- Validacion de campos obligatorios
                if (camposTabla[j].required) {
                    if (data[i][j] == "") {
                        validaciones.push({ row: i, col: j, validation: Validacion.DatoObligatorio });
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
                            validaPorcentaje = true;
                        }
                    }                   
                }
            }

            //- Validamos combinacion de tipo con causa
            if (data[i][22] != "" && data[i][23] != "") {
                for (var k = 0; k < arregloCausaInterrupcion.length; k++) {
                    if (parseInt(data[i][23]) == parseInt(arregloCausaInterrupcion[k].id)) {
                        if (parseInt(data[i][22]) != parseInt(arregloCausaInterrupcion[k].idTipo)) {
                            validaciones.push({ row: i, col: 23, validation: Validacion.TipoCausaInterrupcion });
                        }
                    }
                }
            }
                       
            //- Tipo
            if (data[i][4]!= null && data[i][4] !="" && data[i][4].length > 50) {
                validaciones.push({ row: i, col: 4, validation: ValidacionAdicional.TextoTipoInterrupcion });
            }

            //- Causa
            if (data[i][5] != null && data[i][5] != "" &&  data[i][5].length > 50) {
                validaciones.push({ row: i, col: 5, validation: ValidacionAdicional.TextoCausaInterrupcion });
            }

            //- Codigo osi
            if (data[i][6] != null && data[i][6] != "" && data[i][6].length > 20) {
                validaciones.push({ row: i, col: 6, validation: ValidacionAdicional.TextoCodigoOsi });
            }

            //- observacion
            if (data[i][9] != null && data[i][9] != "" && data[i][9].length > 100) {
                validaciones.push({ row: i, col: 9, validation: ValidacionAdicional.TextoObservacion });
            }

            if (validaPorcentaje) {
                //- Validamos suma de porcentajes
                if (porcentaje != 100) {
                    validaciones.push({ row: i, col: -1, validation: Validacion.SumaPorcentajes });
                }
            }

            //- Validamos que se hayan ingresado porcentajes y empresas
            for (var t = 0; t < 4; t++) {
                if ((data[i][10 + t * 2] == "" && data[i][10 + t * 2 + 1] != "") || (data[i][10 + t * 2] != "" && data[i][10 + t * 2 + 1] == "")) {
                    validaciones.push({ row: i, col: 10 + t * 2, validation: Validacion.IngresoReponsablePorcentaje });
                }
            }

            var ordenResponsable = [];
            //- validamos que los porcentajes se hayan llenado en orden
            for (var t = 0; t < 4; t++) {
                ordenResponsable[t] = 0;
                if ((data[i][10 + t * 2] != "" && data[i][10 + t * 2 + 1] != "")) {
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

            //- Validamos la duración de la interrupcion
            if (data[i][2] != "" && data[i][3] != "") {
                if (validarFechaHora(data[i][2]) && validarFechaHora(data[i][3])) {
                    if ((getDateTime(data[i][3]) - getDateTime(data[i][2])) / (1000 * 60) < 3) {
                        validaciones.push({ row: i, col: 3, validation: Validacion.DuracionEvento });
                    }
                }
            }

            //- Fechas programadas
            if (data[i][20] == "" && data[i][21] != "") {
                validaciones.push({ row: i, col: 20, validation: Validacion.FechaProgramadaInicio });
            }
            if (data[i][20] != "" && data[i][21] == "") {
                validaciones.push({ row: i, col: 21, validation: Validacion.FechaProgramadaFin });
            }
            if (data[i][20] != "" && data[i][21] != "") {
                if (validarFechaHora(data[i][20]) && validarFechaHora(data[i][21])) {
                    if (getDateTime(data[i][21]) - getDateTime(data[i][20]) <= 0) {
                        validaciones.push({ row: i, col: 21, validation: Validacion.DuracionProgramada });
                    }
                }
            }

            arreglo.push(data[i]);
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

function getDataInterrupciones() {
    return hotInterrupciones.getData();
}

function getSize() {
    return hotInterrupciones.getData().length;
}

var ValidacionAdicional = {
    TextoTipoInterrupcion: "El tipo de interrupción no puede superar los 50 caracteres",
    TextoCausaInterrupcion: "La causa de interrupción no puede superar los 50 caracteres",
    TextoCodigoOsi: "El código OSI no puede superar los 20 caracteres",
    TextoObservacion: "La observación no puede superar los 100 caracteres",
};