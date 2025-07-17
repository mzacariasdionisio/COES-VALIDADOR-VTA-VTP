var controlador = siteRoot + 'IEOD/EnergiaPrimaria/';
var HOJA_DATA = [];
var hojaPrincipal = '';

$(function () {
    var data = $("#hfHojaJson").val();
    HOJA_DATA = JSON.parse(data);

    //Generar html de los tabs
    var prefijo = 'view';
    var idTabContainer = '#tab-container';
    cargarListaTabsFromHojas(HOJA_DATA, hojaPrincipal, idTabContainer, prefijo, true);

    //Carga tab por tab del formato
    for (var i = 0; i < HOJA_DATA.length; i++) {
        var numHoja = HOJA_DATA[i].Hojacodi;
        var formato = HOJA_DATA[i].Formatcodi;
        var numHojaStr = numHoja + '';
        var idHoja = prefijo + numHoja;

        LISTA_OBJETO_HOJA[numHojaStr] = crearObjetoHoja(hojaPrincipal, idHoja, idTabContainer);

        getErrores(numHojaStr)[ERROR_NO_NUMERO].validar = true;
        getErrores(numHojaStr)[ERROR_LIM_INFERIOR].validar = true;
        getErrores(numHojaStr)[ERROR_LIM_SUPERIOR].validar = true;
        getErrores(numHojaStr)[ERROR_BLANCO].validar = true;
        getErrores(numHojaStr)[ERROR_NUMERO_NEGATIVO].validar = true;

        setTieneGrafico(true, numHojaStr);

        cargarHoja(numHojaStr, formato, idHoja);
    }
});

function generaFiltroGrafico(numHoja) { }

function mostrarGrafico(numHoja) {
    generarFiltroGraficoFormatoIEOD(numHoja);
    graficoFormatoCentralUnidadMedidaEje(numHoja);

    setTimeout(function () {
        $(getIdGrafico(numHoja)).bPopup({
            easing: 'easeOutBack',
            speed: 450,
            transition: 'slideDown',
            modalClose: false
        });
    }, 50);
}