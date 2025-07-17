var numColumnasResizables = 4; //solo las 3 primeras son resizables

var MODELO_FICHA = null;
var COL_ADICIONAL;
var mostrarColumnaValorConfidencial = false;
var mostrarColumnaInstructivoLlenado = false;
var mostrarColumnaSustento = false;

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

function generarModeloFicha() {
    var lista = [];

    //generar objetos cuando se haya seleccionado una ficha
    if (MODELO_FICHA  != null && MODELO_FICHA.ListaTreeData !== undefined) {
        actualizarListaConCheckConfidencial(TIPO_ARCHIVO_VALOR_DATO);
        actualizarListaConCheckConfidencial(TIPO_ARCHIVO_SUSTENTO_DATO);

        for (var i = 0; i < MODELO_FICHA.ListaTreeData.length; i++) {
            var regData = MODELO_FICHA.ListaTreeData[i];
            if (regData.Ftedatcodi > 0) {
                var valorCelda = $(`#campo_input_${regData.Ftitcodi}`).val();
                regData.Valor = valorCelda;

                //Anotacion
                var anotacionFila = $("#campo_input_anotacion_" + regData.Ftitcodi).val();

                //ValConfidencial
                var seleccionado = "N";
                if ($(`#chk_ValConfidencial_${regData.Ftitcodi}`).is(':checked')) seleccionado = "S";

                //Archivos
                var seccion = obtenerSeccionXcnp(MODELO_FICHA.ListaAllItems, regData.Ftitcodi);
                var listaArchivoValor = getListaArchivoXTipo(seccion, TIPO_ARCHIVO_VALOR_DATO);
                var listaArchivoAdjunto = getListaArchivoXTipo(seccion, TIPO_ARCHIVO_SUSTENTO_DATO);

                lista.push({
                    Ftitcodi: regData.Ftitcodi,
                    Ftedatcodi: regData.Ftedatcodi,
                    FtitcodiDependiente: regData.FtitcodiDependiente,
                    EsFilaEditableExtranet: regData.EsFilaEditableExtranet,
                    EsFilaRevisableIntranet: regData.EsFilaRevisableIntranet,
                    Valor: regData.Valor,
                    Itemcomentario: anotacionFila ?? '',
                    ItemValConfidencial: seleccionado,
                    ListaArchivoValor: listaArchivoValor,
                    ListaArchivoAdjunto: listaArchivoAdjunto,
                });
            }
        }
    }

    return lista;
}

//////////////////////////////////////////////////////////
//// btnExpandirRestaurar
//////////////////////////////////////////////////////////
function expandirRestaurar() {
    if ($('#hfExpandirContraer').val() == "E") {
        expandirExcel();

        $('#hfExpandirContraer').val("C");
        $('#spanExpandirContraer').text('Restaurar');

        var img = $('#imgExpandirContraer').attr('src');
        var newImg = img.replace('expandir.png', 'contraer.png');
        $('#imgExpandirContraer').attr('src', newImg);

    }
    else {
        restaurarExcel();

        $('#hfExpandirContraer').val("E");
        $('#spanExpandirContraer').text('Expandir');

        var img = $('#imgExpandirContraer').attr('src');
        var newImg = img.replace('contraer.png', 'expandir.png');
        $('#imgExpandirContraer').attr('src', newImg);
    }

    //cambiar ancho de handson
    for (var i = 0; i < MODELO_LISTA_CENTRAL.length; i++) {
        var objTabCentral = MODELO_LISTA_CENTRAL[i];
        updateDimensionHandson(objTabCentral.Equicodi);
    }

    //
    updateDimensionHandsonSustentatorio();
}

function expandirExcel() {
    $('#mainLayout').addClass("divexcel");

    for (var i = 0; i < MODELO_LISTA_CENTRAL.length; i++) {
        var objTabCentral = MODELO_LISTA_CENTRAL[i];
        if (LISTA_OBJETO_HOJA[objTabCentral.Equicodi].hot != null) {
            LISTA_OBJETO_HOJA[objTabCentral.Equicodi].hot.render();
        }
    }
}

function restaurarExcel() {
    $('#tophead').css("display", "none");
    $('#detExcel').css("display", "block");
    $('#mainLayout').removeClass("divexcel");
    $('#itemExpandir').css("display", "block");
    $('#itemRestaurar').css("display", "none");
}

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

////////////////////////////////////////////////
// Util
////////////////////////////////////////////////

function esString(valor) {
    if (typeof valor == "string")
        return true;
    else
        return false;
}
