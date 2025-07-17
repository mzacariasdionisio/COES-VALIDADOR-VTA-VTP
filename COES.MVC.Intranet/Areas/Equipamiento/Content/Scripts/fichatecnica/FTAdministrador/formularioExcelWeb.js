var controlador = siteRoot + 'Equipamiento/FTAdministrador/';

var COL_ADICIONAL;
var mostrarColumnaValorConfidencial = false;
var mostrarColumnaInstructivoLlenado = false;
var mostrarColumnaSustento = false;
var numColumnasResizables = 9; //solo las 3 primeras son resizables

var MODELO_FICHA = null;
//var OPCION_GLOBAL_EDITAR = false;

$(function () {
    //$('#btnInicio').click(function () {
    //    regresarPrincipal();
    //});
    //$('#btnEnviarDatos').click(function () {
    //    enviarExcelWeb();
    //});
    //$('#btnMostrarErrores').click(function () {
    //    mostrarDetalleErrores();
    //});
    //$('#btnGuardarAnotacion').click(function () {
    //    guardarTemporalmenteAnotacion();
    //});

    //listarFicha();
});



function createResizableColumn(col, resizer, mitabla) {
    let x = 0;
    let w = 0;
    let wT = 0;

    const mouseDownHandler = function (e) {
        x = e.clientX;

        const styles = window.getComputedStyle(col);
        w = parseInt(styles.width, 10);

        const stylesT = window.getComputedStyle(mitabla);
        wT = parseInt(stylesT.width, 10);

        document.addEventListener('mousemove', mouseMoveHandler);
        document.addEventListener('mouseup', mouseUpHandler);

        resizer.classList.add('resizing');
    };

    const mouseMoveHandler = function (e) {
        const dx = e.clientX - x;
        col.style.width = `${w + dx}px`;
        wT = 1405;
        //mitabla.style.width = `${wT + dx}px`;
    };

    const mouseUpHandler = function () {
        resizer.classList.remove('resizing');
        document.removeEventListener('mousemove', mouseMoveHandler);
        document.removeEventListener('mouseup', mouseUpHandler);
    };

    resizer.addEventListener('mousedown', mouseDownHandler);
};


function createResizableTable(table) {
    const cols = table.querySelectorAll('th');
    var i = 0;
    [].forEach.call(cols, function (col) {
        if (i < numColumnasResizables) {
            // Add a resizer element to the column
            const resizer = document.createElement('div');
            resizer.classList.add('resizer');

            // Set the height
            resizer.style.height = `${table.offsetHeight}px`;

            col.appendChild(resizer);

            createResizableColumn(col, resizer, table);
        }
        i++;
    });
}


function getObjetoFiltroFT() {
    var filtro = {};

    filtro.codigoEnvio = parseInt($("#hfIdEnvio").val()) || 0;
    filtro.fteeqcodi = parseInt($("#hfIdEquipoEnvio").val()) || 0;
    //filtro.codigoEnvioTemporal = parseInt($("#hfIdEnvioTemporal").val()) || 0;

    return filtro;
}

function _determinarColumnasAdicionales(lstConfiguraciones) {
    COL_ADICIONAL = 0;
    mostrarColumnaValorConfidencial = false;
    mostrarColumnaInstructivoLlenado = false;
    mostrarColumnaSustento = false;

    //Seteo si debe mostrar Columna VALOR CONFIDENCIAL
    var conFlagConfidencial = lstConfiguraciones.find(x => x.Fitcfgflagvalorconf === "S");
    if (conFlagConfidencial != null) {
        mostrarColumnaValorConfidencial = true;
        COL_ADICIONAL++;
    }

    //Seteo si debe mostrar Columna INSTRUCTIVO DE LLENADO
    var conInstructivoLlenado = lstConfiguraciones.find(x => x.Fitcfgflaginstructivo === "S");
    if (conInstructivoLlenado != null) {
        mostrarColumnaInstructivoLlenado = true;
        COL_ADICIONAL++;
    }

    //Seteo si debe mostrar Columna SUSTENTO
    var conSustento = lstConfiguraciones.find(x => x.Fitcfgflagsustento === "S");
    if (conSustento != null) {
        mostrarColumnaSustento = true;
        COL_ADICIONAL++;
    }
}