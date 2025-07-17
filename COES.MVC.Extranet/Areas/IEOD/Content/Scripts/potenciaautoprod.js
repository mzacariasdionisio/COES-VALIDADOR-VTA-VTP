var controlador = siteRoot + 'IEOD/PotAutoProd/';

$(function () {
    var hoja1 = '1';
    //inicializar variables
    LISTA_OBJETO_HOJA = { '1': crearObjetoHoja() };

    getErrores(hoja1)[ERROR_NO_NUMERO].validar = true;
    getErrores(hoja1)[ERROR_LIM_INFERIOR].validar = true;
    getErrores(hoja1)[ERROR_LIM_SUPERIOR].validar = true;

    setTieneGrafico(true, hoja1);

    var fmt1 = $("#hfFormato1").val();

    //crear vistas
    cargarHoja(hoja1, fmt1, 'viewHojaFormato');
});

function mostrarGrafico(numHoja) {
    /*
    strHtml = "<div id ='panelGrafico' style='display: block; width: 1250px; height:650px;'></div>";
    getIdElementoGrafico(numHoja, '#idVistaGrafica').html(strHtml);

    setTimeout(function () {
        getIdElementoGrafico(numHoja, '').bPopup({
            easing: 'easeOutBack',
            speed: 450,
            transition: 'slideDown',
            modalClose: false
        });
    }, 50);

    generarFiltroGraficoFormatoIEOD();
    generarGraficoFormatoIEOD();
    */
}

function generaFiltroGrafico() {
}