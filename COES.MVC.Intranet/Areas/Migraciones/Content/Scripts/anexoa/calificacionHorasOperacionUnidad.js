var controlador = siteRoot + 'Migraciones/AnexoA/'

$(function () {
    $('#cbTipoOperacion').multipleSelect({
        width: '150px',
        filter: true,
        onClose: function (view) { //Cambio
            cargarLista(1);
            //pintarPaginado();
        }
    });

    $('#cbEmpresa').multipleSelect({
        width: '150px',
        filter: true,
        onClose: function (view) {
            cargarTipoCentral();
        }
    });

    
    cargarValoresIniciales();
});

function cargarValoresIniciales() {
    $('#cbTipoOperacion').multipleSelect('checkAll');
    $('#cbEmpresa').multipleSelect('checkAll');

    cargarTipoCentral();
}



function cargarTipoCentral() {
    $.ajax({
        type: 'POST',
        url: controlador + 'CargarTipoCentral',
        data: { idEmpresa: getEmpresa() },
        success: function (aData) {

            $('#tipocentral').html(aData);
            $('#cbTipoCentral').multipleSelect({
                width: '150px',
                filter: true,
                onClose: function (view) {
                    cargarTipoCombustible();
                }
            });
            cargarTipoCombustible();

        },
        error: function () {
            alert("Ha ocurrido un error");
        }
    });
}

function cargarTipoCombustible() {
    var tipocentral = $('#cbTipoCentral').multipleSelect('getSelects');

    if (tipocentral == "[object Object]") central = "-1";
    $('#hfTipoCentral').val(tipocentral);

    $.ajax({
        type: 'POST',
        url: controlador + 'CargarTipoCombustible',
        data: { idTipoCentral: $('#hfTipoCentral').val() },
        success: function (aData) {

            $('#tipocombustible').html(aData);
            $('#cbTipoCombustible').multipleSelect({
                width: '150px',
                filter: true,
                onClose: function (view) {
                    cargarLista(1);
                    //pintarPaginado();
                }
            });
            $('#cbTipoCombustible').multipleSelect('checkAll');
            cargarLista(1);
        },
        error: function () {
            alert("Ha ocurrido un error");
        }
    });

}


function cargarClasificacion() {
    var tipocentral = $('#cbTipoCentral').multipleSelect('getSelects');

    if (tipocentral == "[object Object]") central = "-1";
    $('#hfTipoCentral').val(tipocentral);

    $.ajax({
        type: 'POST',
        url: controlador + 'CargarClasificacion',
        data: { idTipoCentral: $('#hfTipoCentral').val() },
        success: function (aData) {

            $('#clasificacion').html(aData);
            $('#cbClasificacion').multipleSelect({
                width: '150px',
                filter: true
            });

        },
        error: function () {
            alert("Ha ocurrido un error");
        }
    });

}

function cargarLista(nroPagina) {

    var modo = $('#cbModo').multipleSelect('getSelects');
    if (modo == "[object Object]") modo = "-1";
    $('#hfModo').val(modo);
    var idModo = $('#hfModo').val();

    var unidad = $('#cbUnidad').multipleSelect('getSelects');
    if (unidad == "[object Object]") unidad = "-1";
    $('#hfUnidad').val(unidad);
    var idUnidad = $('#hfUnidad').val();

    var tipoOpe = $('#cbTipoOpe').multipleSelect('getSelects');
    if (tipoOpe == "[object Object]") tipoOpe = "-1";
    $('#hfTipoOpe').val(tipoOpe);
    var idtipoOpe = $('#hfTipoOpe').val();


    var semanaI = $('#hfcboSemanaI').val();
    var semanaF = $('#hfcboSemanaF').val();


    $.ajax({
        type: 'POST',
        url: controlador + 'CargarCalificacionReporte',
        data: {
            idEmpresa: getEmpresa(),
            fechaInicio: getFechaInicio(),
            fechaFin: getFechaFin(),
            idTOperacion: getTipoOperacion(),
            idTCentral: getTipocentral(),
            idTCombustible: getTipoCombustible(),
            idSistemaA: getSistemaA(),
            idOtraClasificacion: getOtraClasificacion(),
            nroPagina: nroPagina
        },
        success: function (aData) {
            var anchoActual = $(".search-content").width();
            var altoRep = $("#tablaHOP").height();

            $("#reporte").css({
                width: anchoActual, height: altoRep
            });

            $('#listado').html(aData.Resultado);
            $('#idGraficoContainer').html('');

            $("#tablaHOP").css({
                width: anchoActual - 15
            });
            $('#tablaHOP').dataTable({
                //"scrollY": 500,
                "scrollX": true,
                "sDom": 't',
                "ordering": false,
                "iDisplayLength": 500
            });

            actualizarPantalla();
        },
        error: function () {
            alert("Ha ocurrido un error");
        }
    });
}


function actualizarPantalla() {
    $(window).on('resize', function () {
        var anchoActual = $(".search-content").width();
        var altoRep = $("#tablaHOP").height();

        $("#reporte").css({
            width: anchoActual, height: altoRep
        });

        $("#tablaHOP").css({
            width: anchoActual - 15
        });
    });
}