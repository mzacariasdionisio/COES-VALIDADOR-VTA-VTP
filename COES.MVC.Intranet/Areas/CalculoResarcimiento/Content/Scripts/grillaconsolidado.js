var arregloSiNo = [
    { id: 'S', text: 'Si' },
    { id: 'N', text: 'No' }
];
var newHeaders = [];
var newHeadersRC = [];
var hot = null;
var hotTrazabilidad = null;

var colwidths = [
    60,      //- Id
    40,     //- Eliminar
    280,    //- Suministrador
    120,    //- Correlativo
    120,    //- Tipo cliente
    500,    //- Cliente
    480,    //- Punto Entrega
    80,     //- Nro de suministro
    80,     //- Nivel de tension
    250,    //- Aplicación Literal
    80,     //- Energia semestral
    160,    //- Incremento tolerancia
    250,    //- Tipo interrupcion
    350,    //- Causa interrupcion
    60,     //- Ni
    60,     //- Ki
    140,    //- Fecha ini  --
    140,    //- Fecha fin
    140,    //- Fecha ini  --
    140,    //- Fecha fin
    300,    //- Empresa 1   --/
    60,     //- Porcentaje
    80,     //- Aplicacion ntcse
    90,     //- Compensacion 0
    300,    //- Empresa 2   --
    60,     //- Porcentaje 
    80,     //- Aplicacion ntcse
    90,     //- Compensacion 0
    300,    //- Empresa 3   --
    60,     //- Porcentaje
    80,     //- Aplicacion ntcse
    90,     //- Compensacion 0
    300,    //- Empresa  4  --
    60,     //- Porcentaje
    80,     //- Aplicacion ntcse
    90,     //- Compensacion 0
    300,    //- Empresa  5  --
    60,     //- Aplicacion ntcse
    90,     //- Compensacion 0
    80,     //- Porcentaje
    300,    //- Causa resumida
    80,     //- Ei/E
    120,    //- Resarcmiento
    80      //- Evidencia
];

var colwidthsTrazabilidad = [
    180,    //- Usuario
    200,    //- Fecha
    200,    //- Suministrador
    120,    //- Correlativo
    120,    //- Tipo cliente
    220,    //- Cliente
    160,    //- Punto entrega
    80,     //- Nro de suministro
    80,     //- Nivel de tension
    250,    //- Aplicación Literal
    80,     //- Energia semestral
    160,    //- Incremento tolerancia
    150,    //- Tipo interrupcion
    180,    //- Causa interrupcion
    60,     //- Ni
    60,     //- Ki
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
    300,    //- Causa resumida
    80,     //- Ei/E
    120     //- Resarcmiento
];

var colrespuesta = [
    120,   //- Conformidad responsable
    200,   //- Observacion responsable
    140,   //- Detalle campo observado responsable
    140,   //- Comentarios responsable  
    80,    //- Evidencia responsable
    120,   //- Conformidad suministrador
    120,   //- Comentarios suministrador    
    80     //- Evidencia
];

var colrespuestaTrazabilidad = [
    120,   //- Conformidad responsable
    200,   //- Observacion responsable
    140,   //- Detalle campo observado responsable
    140,   //- Comentarios responsable   
    120,   //- Conformidad suministrador
    120   //- Comentarios suministrador    
];

var colwidthsRechazo = [
    60,      //- Id
    40,     //- Eliminar
    280,    //- Suministrador
    120,    //- Correlativo
    120,    //- Tipo cliente
    350,    //- Cliente
    300,    //- Punto Entrega
    520,     //- Alimentador SED
    120,     //- ENST
    250,    //- Evento COES
    200,    //- Comentario
    140,    //- Fecha ini  --
    140,    //- Fecha fin
    140,    //- Pk
    140,    //- Compensable
    140,    //- ENS
    140,     //- Resarcimiento,
    220,    //- Empresa 1   --
    60,     //- Porcentaje
    180,     //- Aplicacion ntcse
    220,    //- Empresa 2   --
    60,     //- Porcentaje
    180,     //- Aplicacion ntcse
    220,    //- Empresa 3   --
    60,     //- Porcentaje
    180,     //- Aplicacion ntcse
    220,    //- Empresa 4   --
    60,     //- Porcentaje
    180,     //- Aplicacion ntcse
    220,    //- Empresa 5   --
    60,     //- Porcentaje
    180,     //- Aplicacion ntcse
];

var colwidthsRechazoTrazabilidad = [
    180,      //- Id
    200,     //- Eliminar
    200,    //- Suministrador
    120,    //- Correlativo
    120,    //- Tipo cliente
    220,    //- Cliente
    160,    //- Punto Entrega
    160,     //- Alimentador SED
    120,     //- ENST
    250,    //- Evento COES
    200,    //- Comentario
    140,    //- Fecha ini  --
    140,    //- Fecha fin
    140,    //- Pk
    140,    //- Compensable
    140,    //- ENS
    140     //- Resarcimiento
];

var headers = ['ID', '', 'Suministrador', 'Correlativo por \n Punto de Entrega', 'Tipo de Cliente', 'Cliente', 'Punto de Entrega / \n Barra', 'N° de suministro \n cliente libre', 'Nivel de Tensión', 'Aplicación literal \n e) de numeral 5.2.4 \n Base Metodológica \n (meses de suministro en el semestre)', 'Energía Semestral(kWh)', 'Incremento de tolerancias \n Sector Distribución Típico \n 2(Mercado regulado)', 'Tipo', 'Causa', 'Ni', 'Ki', 'Tiempo Ejecutado', '', 'Tiempo Programado', '', 'Responsable 1', '', '', '', 'Responsable 2', '', '', '', 'Responsable 3', '', '', '', 'Responsable 4', '', '', '', 'Responsable 5', '', '', '', 'Causa resumida de interrupción', 'Ei / E', 'Resarcimiento(US$)', 'Evidencia', 'Observación Responsable 1', '', '', '', '', 'Conformidad Responsable 1', '', '', 'Observación Responsable 2', '', '', '', '', 'Conformidad Responsable 2', '', '', 'Observación Responsable 3', '', '', '', '', 'Conformidad Responsable 3', '', '', 'Observación Responsable 4', '', '', '', '', 'Conformidad Responsable 4', '', '', 'Observación Responsable 5', '', '', '', '', 'Conformidad Responsable 5', '', '', 'Decisión controversia', 'Comentario'];
var headersTrazabilidad = ['Usuario', 'Fecha', 'Suministrador', 'Correlativo por \n Punto de Entrega', 'Tipo de Cliente', 'Cliente', 'Punto de Entrega / \n Barra', 'N° de suministro \n cliente libre', 'Nivel de Tensión', 'Aplicación literal \n e) de numeral 5.2.4 \n Base Metodológica \n (meses de suministro en el semestre)', 'Energía Semestral(kWh)', 'Incremento de tolerancias \n Sector Distribución Típico \n 2(Mercado regulado)', 'Tipo', 'Causa', 'Ni', 'Ki', 'Tiempo Ejecutado', '', 'Tiempo Programado', '', 'Responsable 1', '', 'Responsable 2', '', 'Responsable 3', '', 'Responsable 4', '', 'Responsable 5', '', 'Causa resumida de interrupción', 'Ei / E', 'Resarcimiento(US$)', 'Observación Responsable 1', '', '', '', 'Conformidad Responsable 1', '', 'Observación Responsable 2', '', '', '', 'Conformidad Responsable 2', '', 'Observación Responsable 3', '', '', '', 'Conformidad Responsable 3', '', 'Observación Responsable 4', '', '', '', 'Conformidad Responsable 4', '', 'Observación Responsable 5', '', '', '', 'Conformidad Responsable 5', '', 'Decisión controversia', 'Comentario'];

var headersAlterno = ['', '', 'Correlativo por \n Punto de Entrega', 'Tipo de Cliente', 'Cliente', 'Punto de Entrega / \n Barra', 'N° de suministro \n cliente libre', 'Nivel de Tensión', 'Aplicación literal \n e) de numeral 5.2.4 \n Base Metodológica \n (meses de suministro en el semestre)', 'Energía Semestral(kWh)', 'Incremento de tolerancias \n Sector Distribución Típico \n 2(Mercado regulado)', 'Tipo', 'Causa', 'Ni', 'Ki', 'Tiempo Ejecutado - Inicio', 'Tiempo Ejecutado - Fin', 'Tiempo Programado - Inicio', 'Tiempo Programado - Fin', 'Responsable 1', '% Responsable 1', 'Responsable 2', '% Responsable 2', 'Responsable 3', '% Responsable 3', 'Responsable 4', '% Responsable 4', 'Responsable 5', '% Responsable 5', 'Causa resumida de interrupción', 'Ei / E', 'Resarcimiento(US$)', 'Evidencia',
    'Conformidad Responsable 1', 'Observación Responsable 1', ' Detalle de campo observado Responsable 1', 'Comentarios Responsable 1', 'Sustento Responsable 1', 'Conformidad Suministrador - Responsable 1', 'Comentarios Suministrador - Responsable 1', 'Sustento Suministrador - Responsable 1',
    'Conformidad Responsable 2', 'Observación Responsable 2', ' Detalle de campo observado Responsable 2', 'Comentarios Responsable 2', 'Sustento Responsable 2', 'Conformidad Suministrador - Responsable 2', 'Comentarios Suministrador - Responsable 2', 'Sustento Suministrador - Responsable 2',
    'Conformidad Responsable 3', 'Observación Responsable 3', ' Detalle de campo observado Responsable 3', 'Comentarios Responsable 3', 'Sustento Responsable 3', 'Conformidad Suministrador - Responsable 3', 'Comentarios Suministrador - Responsable 3', 'Sustento Suministrador - Responsable 3',
    'Conformidad Responsable 4', 'Observación Responsable 4', ' Detalle de campo observado Responsable 4', 'Comentarios Responsable 4', 'Sustento Responsable 4', 'Conformidad Suministrador - Responsable 4', 'Comentarios Suministrador - Responsable 4', 'Sustento Suministrador - Responsable 4',
    'Conformidad Responsable 5', 'Observación Responsable 5', ' Detalle de campo observado Responsable 5', 'Comentarios Responsable 5', 'Sustento Responsable 5', 'Conformidad Suministrador - Responsable 5', 'Comentarios Suministrador - Responsable 5', 'Sustento Suministrador - Responsable 5', 'Decisión controversia', 'Comentario'];

var dataObj = [
    headers,
    ['', '', '', '', '', '', '', '', '', '', '', '', '', '', '', '', 'Fecha Hora Inicio', 'Fecha Hora Fin', 'Fecha Hora Inicio', 'Fecha Hora Fin', 'Empresa', '%', 'Aplicación 1DF NTCSE', 'Compensación Cero', 'Empresa', '%', 'Aplicación 1DF NTCSE', 'Compensación Cero', 'Empresa', '%', 'Aplicación 1DF NTCSE', 'Compensación Cero', 'Empresa', '%', 'Aplicación 1DF NTCSE', 'Compensación Cero', 'Empresa', '%', 'Aplicación 1DF NTCSE', 'Compensación Cero', '', '', '', '', 'Conformidad Responsable', 'Observación', 'Detalle campo observado', 'Comentarios', 'Sustento', 'Conformidad Suministrador', 'Comentarios', 'Sustento', 'Conformidad Responsable', 'Observación', 'Detalle campo observado', 'Comentarios', 'Sustento', 'Conformidad Suministrador', 'Comentarios', 'Sustento', 'Conformidad Responsable', 'Observación', 'Detalle campo observado', 'Comentarios', 'Sustento', 'Conformidad Suministrador', 'Comentarios', 'Sustento', 'Conformidad Responsable', 'Observación', 'Detalle campo observado', 'Comentarios', 'Sustento', 'Conformidad Suministrador', 'Comentarios', 'Sustento', 'Conformidad Responsable', 'Observación', 'Detalle campo observado', 'Comentarios', 'Sustento', 'Conformidad Suministrador', 'Comentarios', 'Sustento','',''],
    ['', '', '', '', '', '', '', '', '', '', '', '', '', '', '', '', '', '', '', '', '', '', '', '', '', '', '', '', '', '', '', '', '', '', '', '', '', '', '', '', '', '', '', '', '', '', '', '', '', '', '', '', '', '', '', '', '', '', '', '', '', '', '', '', '', '', '', '', '', '', '', '', '', '', '', '', '', '', '', '', '', '', '', '', '', ''],
    ['', '', '', '', '', '', '', '', '', '', '', '', '', '', '', '', '(dd/mm/yyyy hh24:mm:ss)', '', '(dd/mm/yyyy hh24:mm:ss)', '', '', '', '', '', '', '', '', '', '', '', '', '', '', '', '', '', '', '', '', '', '', '', '', '', '', '', '', '', '', '', '', '', '', '', '', '', '', '', '', '', '', '', '', '', '', '', '', '', '', '', '', '', '', '', '', '', '', '', '', '', '', '', '', '', '', ''],
];

var dataObjTrazabilidad = [
    headersTrazabilidad,
    ['', '', '', '', '', '', '', '', '', '', '', '', '', '', '', '', 'Fecha Hora Inicio', 'Fecha Hora Fin', 'Fecha Hora Inicio', 'Fecha Hora Fin', 'Empresa', '%', 'Empresa', '%', 'Empresa', '%', 'Empresa', '%', 'Empresa', '%',  '', '', '', 'Conformidad Responsable', 'Observación', 'Detalle campo observado', 'Comentarios', 'Conformidad Suministrador', 'Comentarios', 'Conformidad Responsable', 'Observación', 'Detalle campo observado', 'Comentarios', 'Conformidad Suministrador', 'Comentarios', 'Conformidad Responsable', 'Observación', 'Detalle campo observado', 'Comentarios', 'Conformidad Suministrador', 'Comentarios', 'Conformidad Responsable', 'Observación', 'Detalle campo observado', 'Comentarios', 'Conformidad Suministrador', 'Comentarios', 'Conformidad Responsable', 'Observación', 'Detalle campo observado', 'Comentarios', 'Conformidad Suministrador', 'Comentarios', '', ''],
    ['', '', '', '', '', '', '', '', '', '', '', '', '', '', '', '', '', '', '', '', '', '', '', '', '', '', '', '', '', '', '', '', '', '', '', '', '', '', '', '', '', '', '', '', '', '', '', '', '', '', '', '', '', '', '', '', '', '', '', '', '', '', '', ''],
    ['', '', '', '', '', '', '', '', '', '', '', '', '', '', '', '', '(dd/mm/yyyy hh24:mm:ss)', '', '(dd/mm/yyyy hh24:mm:ss)', '', '', '', '', '', '', '', '', '', '', '', '', '', '', '', '', '', '', '', '', '', '', '', '', '', '', '', '', '', '', '', '', '', '', '', '', '', '', '', '', '', '', '', '', ''],
];

var columnDoble = [15, 17, 19, 21, 23, 25, 27];
var columnTriple = [38, 46, 55, 63, 71];
var columnQuintuple = [34, 45, 50, 58, 66];
var columnDobleRC = [10];

var headersRC = ['', '', 'Suministrador', 'Correlativo RC', 'Tipo de Cliente', 'Cliente', 'Barra', 'Alimentador/SED', 'ENST f,k(kWh)', 'Evento COES', 'Comentario', 'Tiempo Ejecutado', '', 'Pk (kW)', 'Compensable', 'ENS f, k', 'Resarcimiento', 'Responsable 1', '', '', 'Responsable 2', '', '', 'Responsable 3', '', '', 'Responsable 4', '', '', 'Responsable 5', '', ''];
var headersAlternoRC = ['', '', 'Suministrador', 'Correlativo por rechazo de Carga', 'Tipo de Cliente', 'Cliente', 'Barra', 'Alimentador/SED', 'ENST f,k(kWh)', 'Evento COES', 'Comentario', 'Tiempo Ejecutado - Inicio', 'Tiempo Ejecutado - Fin', 'Pk (kW)', 'Compensable', 'ENS f, k', 'Resarcimiento'];

var dataObjRC = [
    headersRC,
    ['', '', '', '', '', '', '', '', '', '', '', 'Inicio', 'Fin', '', '', '', '', 'Empresa', '%', 'Aplicación 1DF NTCSE', 'Empresa', '%', 'Aplicación 1DF NTCSE', 'Empresa', '%', 'Aplicación 1DF NTCSE', 'Empresa', '%', 'Aplicación 1DF NTCSE', 'Empresa', '%', 'Aplicación 1DF NTCSE']
];

var headersRCTrazabilidad = ['Usuario', 'Fecha', 'Suministrador', 'Correlativo por rechazo de Carga', 'Tipo de Cliente', 'Cliente', 'Barra', 'Alimentador/SED', 'ENST f,k(kWh)', 'Evento COES', 'Comentario', 'Tiempo Ejecutado', '', 'Pk (kW)', 'Compensable', 'ENS f, k', 'Resarcimiento'];

var dataObjRCTrazabilidad = [
    headersRCTrazabilidad,
    ['', '', '', '', '', '', '', '', '', '', '', 'Inicio', 'Fin', '', '', '', '']
];

function cargarGrillaInterrupciones(result) {

    for (var k = 0; k < 5; k++) {
        for (var t in colrespuesta) {
            colwidths.push(colrespuesta[t]);
        }
    }
    colwidths.push(200);
    colwidths.push(200);

    var merge = [
        { row: 0, col: 16, rowspan: 1, colspan: 2 },
        { row: 0, col: 18, rowspan: 1, colspan: 2 },
        { row: 0, col: 20, rowspan: 1, colspan: 4 },
        { row: 0, col: 24, rowspan: 1, colspan: 4 },
        { row: 0, col: 28, rowspan: 1, colspan: 4 },
        { row: 0, col: 32, rowspan: 1, colspan: 4 },
        { row: 0, col: 36, rowspan: 1, colspan: 4 },
        { row: 3, col: 16, rowspan: 1, colspan: 2 },
        { row: 3, col: 16, rowspan: 1, colspan: 2 },
        { row: 1, col: 16, rowspan: 2, colspan: 1 },
        { row: 1, col: 17, rowspan: 2, colspan: 1 },
        { row: 3, col: 18, rowspan: 1, colspan: 2 },
        { row: 1, col: 18, rowspan: 2, colspan: 1 },
        { row: 1, col: 19, rowspan: 2, colspan: 1 },
        { row: 1, col: 20, rowspan: 3, colspan: 1 },
        { row: 1, col: 21, rowspan: 3, colspan: 1 },
        { row: 1, col: 22, rowspan: 3, colspan: 1 },
        { row: 1, col: 23, rowspan: 3, colspan: 1 },
        { row: 1, col: 24, rowspan: 3, colspan: 1 },
        { row: 1, col: 25, rowspan: 3, colspan: 1 },
        { row: 1, col: 26, rowspan: 3, colspan: 1 },
        { row: 1, col: 27, rowspan: 3, colspan: 1 },
        { row: 1, col: 28, rowspan: 3, colspan: 1 },
        { row: 1, col: 29, rowspan: 3, colspan: 1 },
        { row: 1, col: 30, rowspan: 3, colspan: 1 },
        { row: 1, col: 31, rowspan: 3, colspan: 1 },
        { row: 1, col: 32, rowspan: 3, colspan: 1 },
        { row: 1, col: 33, rowspan: 3, colspan: 1 },
        { row: 1, col: 34, rowspan: 3, colspan: 1 },
        { row: 1, col: 35, rowspan: 3, colspan: 1 },
        { row: 1, col: 36, rowspan: 3, colspan: 1 },
        { row: 1, col: 37, rowspan: 3, colspan: 1 },
        { row: 1, col: 38, rowspan: 3, colspan: 1 },
        { row: 1, col: 39, rowspan: 3, colspan: 1 },
        { row: 0, col: 44, rowspan: 1, colspan: 5 },
        { row: 0, col: 49, rowspan: 1, colspan: 3 },
        { row: 0, col: 52, rowspan: 1, colspan: 5 },
        { row: 0, col: 57, rowspan: 1, colspan: 3 },
        { row: 0, col: 60, rowspan: 1, colspan: 5 },
        { row: 0, col: 65, rowspan: 1, colspan: 3 },
        { row: 0, col: 68, rowspan: 1, colspan: 5 },
        { row: 0, col: 73, rowspan: 1, colspan: 3 },
        { row: 0, col: 76, rowspan: 1, colspan: 5 },
        { row: 0, col: 81, rowspan: 1, colspan: 3 }

    ];

    for (var i = 0; i <= 15; i++) {
        merge.push({
            row: 0, col: i, rowspan: 4, colspan: 1
        })
    }

    for (var i = 40; i <= 43; i++) {
        merge.push({
            row: 0, col: i, rowspan: 4, colspan: 1
        })
    }

    for (var i = 44; i <= 83; i++) {
        merge.push({
            row: 1, col: i, rowspan: 3, colspan: 1
        })
    }
    for (var i = 84; i <= 85; i++) {
        merge.push({
            row: 0, col: i, rowspan: 4, colspan: 1
        })
    }

    //if (hot != null) hot.destroy();
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
        maxRows: dataGrilla.length,
        colWidths: colwidths,
        rowHeaders: true,
        cells: function (row, col, prop) {
            var cellProperties = {};

            // var data = this.instance.getData();

            if (row < 4) {
                cellProperties.renderer = tituloRenderer;
                cellProperties.readOnly = true;

                //- Alternamos con los titulos de observaciones
                if ((col >= 44 && col <= 48) || (col >= 52 && col <= 56) || (col >= 60 && col <= 64) || (col >= 68 && col <= 72) || (col >= 76 && col <= 80)) {
                    cellProperties.renderer = tituloObservacionRenderer;
                    cellProperties.readOnly = true;
                }

                if (col == 22 || col == 23 || col==26 || col ==27 || col ==30 || col == 31 || col == 34 || col ==35 || col > 83) {
                    cellProperties.renderer = tituloDecisionRenderer;
                    cellProperties.readOnly = true;
                }
            }

            if (row > 3) {
                //- Eliminar interrupcion
                if (this.instance.getDataAtCell(row, 0).split("-")[1] == "B") {

                    if (this.instance.getDataAtCell(row, 0).split("-")[1] == "B") {
                        cellProperties.renderer = eliminadoRenderer;
                        cellProperties.readOnly = true;

                        if (col == 1) {
                            cellProperties.renderer = motivoAnulacionRenderer;
                            cellProperties.readOnly = false;
                        }
                    }
                    
                }
                else {

                    if (col < 84) {
                        cellProperties.renderer = disabledRenderer;
                        cellProperties.readOnly = true;

                        if (this.instance.getDataAtCell(row, 0).split("-")[2] == "S") {
                            cellProperties.renderer = trimestralRenderer;
                            cellProperties.readOnly = true;
                        }
                    }

                    if (col == 1) {
                        cellProperties.renderer = trazabilidadRenderer;
                        cellProperties.readOnly = true;

                        if (this.instance.getDataAtCell(row, 0).split("-")[2] == "S") {
                            cellProperties.renderer = trazabilidadRenderer1;
                            cellProperties.readOnly = true;
                        }
                    }

                    //- Evidencia
                    if (col == 43) {
                        if (this.instance.getDataAtCell(row, col) != null && this.instance.getDataAtCell(row, col) != "") {
                            cellProperties.renderer = showFileRenderer;
                            cellProperties.readOnly = true;

                            if (this.instance.getDataAtCell(row, 0).split("-")[2] == "S") {
                                cellProperties.renderer = showFileRenderer1;
                                cellProperties.readOnly = true;
                            }
                        }
                    }

                    //- Evidencia de responsable
                    if (col == 48 || col == 56 || col == 64 || col == 72 || col == 80) {
                        if (this.instance.getDataAtCell(row, col) != null && this.instance.getDataAtCell(row, col) != "") {
                            cellProperties.renderer = showFileRendererObservacion;
                            cellProperties.readOnly = true;

                            if (this.instance.getDataAtCell(row, 0).split("-")[2] == "S") {
                                cellProperties.renderer = showFileRendererObservacion1;
                                cellProperties.readOnly = true;
                            }
                        }
                    }

                    //- Sustento suministrador
                    if (col == 51 || col == 59 || col == 67 || col == 75 || col == 83) {
                        if (this.instance.getDataAtCell(row, col) != null && this.instance.getDataAtCell(row, col)!= "") {
                            cellProperties.renderer = showFileRendererRespuesta;
                            cellProperties.readOnly = true;

                            if (this.instance.getDataAtCell(row, 0).split("-")[2] == "S") {
                                cellProperties.renderer = showFileRendererRespuesta1;
                                cellProperties.readOnly = true;
                            }
                        }
                    }

                    //- Celdas editables
                    if (col == 22 || col == 23 || col == 26 || col == 27 || col == 30 || col == 31 || col == 34 || col == 35 || col == 38 || col == 39) {
                        cellProperties.editor = 'select2';
                        cellProperties.renderer = dropdownSiNoRenderer;
                        cellProperties.select2Options = {
                            data: arregloSiNo,
                            dropdownAutoWidth: true,
                            width: 'resolve',
                            allowClear: false
                        };
                        cellProperties.readOnly = false;
                    }

                    if (col == 84 || col == 85) {
                        if (this.instance.getDataAtCell(row, 0).split("-")[2] == "S") {
                            cellProperties.renderer = trimestralRenderer;
                            cellProperties.readOnly = true;
                        }
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
    cargarColumnasGrilla(headers, "S");
}

function cargarGrillaRechazoCarga(result) {     

    var merge = [
        { row: 0, col: 11, rowspan: 1, colspan: 2 },
        { row: 0, col: 17, rowspan: 1, colspan: 3 },
        { row: 0, col: 20, rowspan: 1, colspan: 3 },
        { row: 0, col: 23, rowspan: 1, colspan: 3 },
        { row: 0, col: 26, rowspan: 1, colspan: 3 },
        { row: 0, col: 29, rowspan: 1, colspan: 3 },
        
    ];

    for (var i = 0; i <= 10; i++) {
        merge.push({
            row: 0, col: i, rowspan: 2, colspan: 1
        })
    }

    for (var i = 13; i <= 16; i++) {
        merge.push({
            row: 0, col: i, rowspan: 2, colspan: 1
        })
    }

    var grilla = document.getElementById('detalleFormato');

    newHeadersRC = dataObjRC[0].slice();
    var dataGrilla = dataObjRC.slice();
    for (var i in result.Data) {
        dataGrilla.push(result.Data[i]);
    }

    hot = new Handsontable(grilla, {
        data: dataGrilla,
        mergeCells: merge,
        fixedRowsTop: 2,
        height: 400,
        colWidths: colwidthsRechazo,
        rowHeaders: true,
        cells: function (row, col, prop) {
            var cellProperties = {};

            //var data = this.instance.getData();

            if (row < 2) {
                cellProperties.renderer = tituloRenderer;
                cellProperties.readOnly = true;

                if (row == 1) {
                    if (col == 18 || col == 19 || col == 21 || col == 22 || col == 24 || col == 25 || col == 27 || col == 28 || col == 30 || col == 31) {
                        cellProperties.renderer = tituloDecisionRenderer;
                        cellProperties.readOnly = true;
                    }
                }
            }

            if (row > 1) {
                if (this.instance.getDataAtCell(row, 0).split("-")[1] == "B") {
                    cellProperties.renderer = eliminadoRenderer;
                    cellProperties.readOnly = true;

                    if (col == 1) {
                        cellProperties.renderer = motivoAnulacionRenderer;
                        cellProperties.readOnly = false;
                    }

                   
                }
                else {
                    cellProperties.renderer = disabledRenderer;
                    cellProperties.readOnly = true;

                    if (col == 1) {
                        cellProperties.renderer = trazabilidadRenderer;
                        cellProperties.readOnly = false;
                    }

                    //- Celdas editables
                    if (col == 18 || col == 21 || col == 24 || col == 27 || col == 30) {
                        if (validarExcelPorcentaje(this.instance.getDataAtCell(row, col))) {
                            cellProperties.renderer = defaultRenderer;
                            cellProperties.readOnly = false;
                        }
                        else {
                            cellProperties.renderer = errorRenderer;
                            cellProperties.readOnly = false;
                        }
                    }

                    //- Celdas editables
                    if (col == 19 || col == 22 || col == 25 || col == 28 || col == 31) {
                        cellProperties.editor = 'select2';
                        cellProperties.renderer = dropdownSiNoRenderer;
                        cellProperties.select2Options = {
                            data: arregloSiNo,
                            dropdownAutoWidth: true,
                            width: 'resolve',
                            allowClear: false
                        };
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
    hot.render();
    cargarColumnasGrilla(headersRC, "R");
}

function actualizarDataGrilla(data, tipo) {
    var dataGrilla = (tipo == "S") ? dataObj.slice() : dataObjRC.slice();
    for (var i in data) {
        dataGrilla.push(data[i]);
    }

    hot.updateSettings({
        data: dataGrilla
    });
    hot.render();
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

var tituloDecisionRenderer = function (instance, td, row, col, prop, value, cellProperties) {
    Handsontable.renderers.TextRenderer.apply(this, arguments);
    td.style.fontWeight = 'bold';
    td.style.fontSize = '11px';
    td.style.textAlign = 'center';
    td.style.verticalAlign = 'middle';
    td.style.color = '#fff';
    td.style.backgroundColor = '#009933';
};

var eliminadoRenderer = function (instance, td, row, col, prop, value, cellProperties) {
    Handsontable.renderers.TextRenderer.apply(this, arguments);
    td.style.backgroundColor = '#FFB0B0';
    td.style.fontSize = '11px';
    td.style.verticalAlign = 'middle';
    td.title = value;
    td.innerHTML = `<div class="truncated">${value}</div>`;
};

var trimestralRenderer = function (instance, td, row, col, prop, value, cellProperties) {
    Handsontable.renderers.TextRenderer.apply(this, arguments);
    td.style.backgroundColor = 'yellow';
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

var motivoAnulacionRenderer = function (instance, td, row, col, prop, value, cellProperties) {
    var div;
    $(td).children('.btn').remove();
    $(td).html('');
    div = document.createElement('div');
    div.className = 'btn';
    td.style.color = '#fff';
    td.style.backgroundColor = '#FFB0B0';
    div.appendChild(document.createTextNode("."));
    td.appendChild(div);
    $(div).on('mouseup', function () {
        showMotivoAnulacion(hot.getData()[row][0].split("-")[0]);
    });
    $(td).addClass("anulacion-renderer");
    return td;
}

var disabledRenderer = function (instance, td, row, col, prop, value, cellProperties) {
    Handsontable.renderers.TextRenderer.apply(this, arguments);
    td.style.backgroundColor = '#F2F2F2';
    td.style.fontSize = '11px';
    td.style.verticalAlign = 'middle';
    td.title = value;
    td.innerHTML = `<div class="truncated">${value}</div>`;
};

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

var trazabilidadRenderer = function (instance, td, row, col, prop, value, cellProperties) {
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
        showTrazabilidad(hot.getData()[row][0].split("-")[0]);
    });
    $(td).addClass("trazabilidad-renderer");
    return td;
}

var trazabilidadRenderer1 = function (instance, td, row, col, prop, value, cellProperties) {
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
        showTrazabilidad(hot.getData()[row][0].split("-")[0]);
    });
    $(td).addClass("trazabilidad-renderer");
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
        showFileInterrupcion(hot.getData()[row][0].split("-")[0], hot.getData()[row][col]);
    });
    $(td).addClass("file-renderer");
    return td;
}

var showFileRendererObservacion = function (instance, td, row, col, prop, value, cellProperties) {
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
        var idDetalle = -1;
        if (col > 32) idDetalle = (col) / 8 - 5;
        showFileObservacion(hot.getData()[row][0].split("-")[0], idDetalle, hot.getData()[row][col]);
    });
    $(td).addClass("file-renderer");
    return td;
}

var showFileRendererRespuesta = function (instance, td, row, col, prop, value, cellProperties) {
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
        var idDetalle = -1;
        if (col > 32) idDetalle = (col - 3) / 8 - 5;
        showFileRespuesta(hot.getData()[row][0].split("-")[0], idDetalle, hot.getData()[row][col]);
    });
    $(td).addClass("file-renderer");
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
        showFileInterrupcion(hot.getData()[row][0].split("-")[0], hot.getData()[row][col]);
    });
    $(td).addClass("file-renderer");
    return td;
}

var showFileRendererObservacion1 = function (instance, td, row, col, prop, value, cellProperties) {
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
        var idDetalle = -1;
        if (col > 32) idDetalle = (col) / 8 - 5;
        showFileObservacion(hot.getData()[row][0].split("-")[0], idDetalle, hot.getData()[row][col]);
    });
    $(td).addClass("file-renderer");
    return td;
}

var showFileRendererRespuesta1 = function (instance, td, row, col, prop, value, cellProperties) {
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
        var idDetalle = -1;
        if (col > 32) idDetalle = (col - 3) / 8 - 5;
        showFileRespuesta(hot.getData()[row][0].split("-")[0], idDetalle, hot.getData()[row][col]);
    });
    $(td).addClass("file-renderer");
    return td;
}

var defaultRenderer = function (instance, td, row, col, prop, value, cellProperties) {
    Handsontable.renderers.TextRenderer.apply(this, arguments);
    td.style.fontSize = '11px';
    td.style.verticalAlign = 'middle';
};

function obtenerDataInterrupcion(tipo) {
    var data = hot.getData();
    var arreglo = [];
    var filaInicio = (tipo == "S") ? 4 : 2;
    var posicionesWeb = (tipo == "S") ? [22, 23, 26, 27, 30, 31, 34, 35, 38, 39, 84, 85] : [18, 19, 21, 22, 24, 25, 27, 28, 30, 31];
    
    for (var i = filaInicio; i < data.length; i++) {
        var id = data[i][0].split("-")[0];
        var estado = data[i][0].split("-")[1];
        var copia = data[i][0].split("-")[2];
        if (estado == "A" && copia != "S") {
            var subData = [];
            subData[0] = id;
            var k = 1;
            for (var j in posicionesWeb) {                
                subData[k] = data[i][posicionesWeb[j]]
                k++;
            }         
            arreglo.push(subData);            
        }
    }    
    return arreglo;
}

function validarDatos(data, tipo) {
    var validaciones = [];   
    var filaInicial = (tipo == "S") ? 5 : 3;
    for (var i in data) {
        if (tipo == "S") {
            if (data[i][11] != null && data[i][11] != "" && data[i][11].length > 300) {
                validaciones.push({ row: (parseInt(i) + filaInicial), col: "Decisión controversia", validation: "Supera los 300 caracteres" });
            }
            if (data[i][12] != null && data[i][12] != "" && data[i][12].length > 300) {
                validaciones.push({ row: (parseInt(i) + filaInicial), col: "Comentario", validation: "Supera los 300 caracteres" });
            }
        }
        else {
            if (!validarExcelPorcentaje(data[i][1])) {
                validaciones.push({ row: (parseInt(i) + filaInicial), col: "Porcentaje responsable 1", validation: "Formato de porcentaje no válido." });
            }
            if (!validarExcelPorcentaje(data[i][3])) {
                validaciones.push({ row: (parseInt(i) + filaInicial), col: "Porcentaje responsable 2", validation: "Formato de porcentaje no válido." });
            }
            if (!validarExcelPorcentaje(data[i][5])) {
                validaciones.push({ row: (parseInt(i) + filaInicial), col: "Porcentaje responsable 3", validation: "Formato de porcentaje no válido." });
            }
            if (!validarExcelPorcentaje(data[i][7])) {
                validaciones.push({ row: (parseInt(i) + filaInicial), col: "Porcentaje responsable 4", validation: "Formato de porcentaje no válido." });
            }
            if (!validarExcelPorcentaje(data[i][9])) {
                validaciones.push({ row: (parseInt(i) + filaInicial), col: "Porcentaje responsable 5", validation: "Formato de porcentaje no válido." });
            }
        }
    }

    return validaciones;
}

function cargarTrazabilidadInterrupciones(result) {

    for (var k = 0; k < 5; k++) {
        for (var t in colrespuestaTrazabilidad) {
            colwidthsTrazabilidad.push(colrespuestaTrazabilidad[t]);
        }
    }
    colwidthsTrazabilidad.push(200);
    colwidthsTrazabilidad.push(200);

    var merge = [
        { row: 0, col: 16, rowspan: 1, colspan: 2 },
        { row: 0, col: 18, rowspan: 1, colspan: 2 },
        { row: 0, col: 20, rowspan: 1, colspan: 2 },
        { row: 0, col: 22, rowspan: 1, colspan: 2 },
        { row: 0, col: 24, rowspan: 1, colspan: 2 },
        { row: 0, col: 26, rowspan: 1, colspan: 2 },
        { row: 0, col: 28, rowspan: 1, colspan: 2 },
        { row: 3, col: 16, rowspan: 1, colspan: 2 },
        { row: 3, col: 16, rowspan: 1, colspan: 2 },
        { row: 1, col: 16, rowspan: 2, colspan: 1 },
        { row: 1, col: 17, rowspan: 2, colspan: 1 },
        { row: 3, col: 18, rowspan: 1, colspan: 2 },
        { row: 1, col: 18, rowspan: 2, colspan: 1 },
        { row: 1, col: 19, rowspan: 2, colspan: 1 },
        { row: 1, col: 20, rowspan: 3, colspan: 1 },
        { row: 1, col: 21, rowspan: 3, colspan: 1 },
        { row: 1, col: 22, rowspan: 3, colspan: 1 },
        { row: 1, col: 23, rowspan: 3, colspan: 1 },
        { row: 1, col: 24, rowspan: 3, colspan: 1 },
        { row: 1, col: 25, rowspan: 3, colspan: 1 },
        { row: 1, col: 26, rowspan: 3, colspan: 1 },
        { row: 1, col: 27, rowspan: 3, colspan: 1 },
        { row: 1, col: 28, rowspan: 3, colspan: 1 },
        { row: 1, col: 29, rowspan: 3, colspan: 1 },
        { row: 0, col: 33, rowspan: 1, colspan: 4 },
        { row: 0, col: 37, rowspan: 1, colspan: 2 },
        { row: 0, col: 39, rowspan: 1, colspan: 4 },
        { row: 0, col: 43, rowspan: 1, colspan: 2 },
        { row: 0, col: 45, rowspan: 1, colspan: 4 },
        { row: 0, col: 49, rowspan: 1, colspan: 2 },
        { row: 0, col: 51, rowspan: 1, colspan: 4 },
        { row: 0, col: 55, rowspan: 1, colspan: 2 },
        { row: 0, col: 57, rowspan: 1, colspan: 4 },
        { row: 0, col: 61, rowspan: 1, colspan: 2 }
    ];

    for (var i = 0; i <= 15; i++) {
        merge.push({
            row: 0, col: i, rowspan: 4, colspan: 1
        })
    }

    for (var i = 30; i <= 32; i++) {
        merge.push({
            row: 0, col: i, rowspan: 4, colspan: 1
        })
    }

    for (var i = 33; i <= 62; i++) {
        merge.push({
            row: 1, col: i, rowspan: 3, colspan: 1
        })
    }
    for (var i = 63; i <= 64; i++) {
        merge.push({
            row: 0, col: i, rowspan: 4, colspan: 1
        })
    }
    var dataGrilla = dataObjTrazabilidad.slice();
    for (var i in result) {
        dataGrilla.push(result[i]);
    }

    if (hotTrazabilidad != null) hotTrazabilidad.destroy();
    var grilla = document.getElementById('excelTrazabilidad');

    var optionsTrazabilidad = {
        data: dataGrilla,
        mergeCells: merge,
        fixedRowsTop: 4,
        height: 400,       
        maxRows: dataGrilla.length,
        colWidths: colwidthsTrazabilidad,
        rowHeaders: true,
        cells: function (row, col, prop) {
            var cellProperties = {};

            var data = this.instance.getData();

            if (row < 4) {
                cellProperties.renderer = tituloRenderer;
                cellProperties.readOnly = true;

                //- Alternamos con los titulos de observaciones
                if ((col >= 33 && col <= 36) || (col >= 39 && col <= 42) || (col >= 45 && col <= 48) || (col >= 51 && col <= 54) || (col >= 57 && col <= 60)) {
                    cellProperties.renderer = tituloObservacionRenderer;
                    cellProperties.readOnly = true;
                }

                if (col > 62) {
                    cellProperties.renderer = tituloDecisionRenderer;
                    cellProperties.readOnly = true;
                }
            }

            if (row > 3) {
                cellProperties.renderer = disabledRenderer;
                cellProperties.readOnly = false;

                if (row < data.length - 1) {
                    if (data[row][col] != data[row + 1][col]) {                        
                        cellProperties.renderer = eliminadoRenderer;
                        cellProperties.readOnly = true;
                    }
                }
            }

            return cellProperties;
        },
        afterLoadData: function () {
            this.render();
        }
    };
    
    hotTrazabilidad = new Handsontable(grilla, optionsTrazabilidad);
    hotTrazabilidad.render();
    $('#excelTrazabilidad').show();
    $('#excelTrazabilidad').scroll();
    $('#excelTrazabilidad').click();
}

function cargarTrazabilidadRechazoCarga(result) {
    var merge = [
        { row: 0, col: 11, rowspan: 1, colspan: 2 }
    ];

    for (var i = 0; i <= 10; i++) {
        merge.push({
            row: 0, col: i, rowspan: 2, colspan: 1
        })
    }

    for (var i = 13; i <= 16; i++) {
        merge.push({
            row: 0, col: i, rowspan: 2, colspan: 1
        })
    }

    var grilla = document.getElementById('excelTrazabilidad');
       
    var dataGrilla = dataObjRCTrazabilidad.slice();
    for (var i in result) {
        dataGrilla.push(result[i]);
    }

    hotTrazabilidad = new Handsontable(grilla, {
        data: dataGrilla,
        mergeCells: merge,
        fixedRowsTop: 2,
        height: 400,
        colWidths: colwidthsRechazoTrazabilidad,
        rowHeaders: true,
        cells: function (row, col, prop) {
            var cellProperties = {};

            var data = this.instance.getData();

            if (row < 2) {
                cellProperties.renderer = tituloRenderer;
                cellProperties.readOnly = true;
            }

            if (row > 1) {
                cellProperties.renderer = disabledRenderer;
                cellProperties.readOnly = true;

                if (row < data.length - 1) {
                    if (data[row][col] != data[row + 1][col]) {
                        cellProperties.renderer = eliminadoRenderer;
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
    hotTrazabilidad.render();
}

function cargarColumnasGrilla(cabeceras, tipo) {
    if(tipo == "S")$('#contenedorColumnasR').html("");
    if(tipo == "R")$('#contenedorColumnasS').html("");
    var html = obtenerColumnas(cabeceras, tipo);
    $('#contenedorColumnas' + tipo).html(html);
    $('#cbSelectAll').click(function (e) {
        var table = $(e.target).closest('table');
        $('td input:checkbox', table).prop('checked', this.checked);
    });
}

function obtenerColumnas(data, tipo) {
    var html = "<table class='pretty tabla-adicional' id='tablaColumnas" + tipo + "'>";
    html = html + " <thead>";
    html = html + "     <tr>";
    html = html + "         <th><input type='checkbox' id='cbSelectAll' checked='checked'></th>";
    html = html + "         <th>Columna</th>";
    html = html + "     </tr>";
    html = html + " </thead>";
    html = html + " <tbody>";
    for (var k in data) {
        if (k > 1 && data[k] != '') {
            html = html + "     <tr>";
            html = html + "         <td><input type='checkbox' checked='checked' value='" + k + "' /></td>";
            html = html + "         <td>" + data[k] + "</td>";
            html = html + "     </tr>";
        }
    }
    html = html + " </tbody>";
    html = html + "</table>";

    return html;
}

function aplicarOcultado() {
    limpiarMensaje('mensajeColumna');
    var newWidts = colwidths.slice();

    var count = 0;
    var countHidden = 0;
    $('#tablaColumnasS tbody input').each(function () {
        if (!$(this).is(':checked')) {
            newWidts[parseInt($(this).val())] = 1;
            countHidden++;
            if (columnDoble.includes(parseInt($(this).val()))) {
                newWidts[parseInt($(this).val()) + 1] = 1;
            }
            if (columnTriple.includes(parseInt($(this).val()))) {
                newWidts[parseInt($(this).val()) + 1] = 1;
                newWidts[parseInt($(this).val()) + 2] = 1;
            }
            if (columnQuintuple.includes(parseInt($(this).val()))) {
                newWidts[parseInt($(this).val()) + 1] = 1;
                newWidts[parseInt($(this).val()) + 2] = 1;
                newWidts[parseInt($(this).val()) + 3] = 1;
                newWidts[parseInt($(this).val()) + 4] = 1;
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

function aplicarOcultadoRC() {
    limpiarMensaje('mensajeColumna');
    var newWidts = colwidthsRechazo.slice();

    var count = 0;
    var countHidden = 0;
    $('#tablaColumnasR tbody input').each(function () {
        if (!$(this).is(':checked')) {
            newWidts[parseInt($(this).val())] = 1;
            countHidden++;
            if (columnDobleRC.includes(parseInt($(this).val()))) {
                newWidts[parseInt($(this).val()) + 1] = 1;
            }
            hot.setDataAtCell(0, parseInt($(this).val()), '');
        }
        else {
            hot.setDataAtCell(0, parseInt($(this).val()), newHeadersRC[parseInt($(this).val())]);
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

function refreshGrilla(result) {
    cargarTrazabilidadInterrupciones(result);    
}

function refreshGrillaRC(result) {
    cargarTrazabilidadRechazoCarga(result);
}