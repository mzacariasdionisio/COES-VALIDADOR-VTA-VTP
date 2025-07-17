var ancho = 900;

$(function () {
    $('#cbTipoOperacion').multipleSelect({
        width: '150px',
        filter: true,
        onClose: function (view) {
            cargarLista();
        }
    });

    $('#cbTipoCentral').multipleSelect({
        width: '150px',
        filter: true,
        onClose: function (view) {
            cargarListaEmpresa();
        }
    });
    ancho = $("#mainLayout").width() > 900 ? $("#mainLayout").width() - 30 : 900;

    cargarValoresIniciales();
});

function cargarValoresIniciales() {
    $('#cbTipoOperacion').multipleSelect('checkAll');
    $('#cbTipoCentral').multipleSelect('checkAll');

    cargarListaEmpresa();
}

function mostrarReporteByFiltros() {
    cargarLista();
}

function cargarListaEmpresa() {
    var idTipoCentral = getTipocentral();

    $.ajax({
        type: 'POST',
        url: controlador + 'CargarEmpresaxTipoCentral',
        data: { idTipoCentral: idTipoCentral },
        cache: false,
        success: function (aData) {
            $('#empresas').html(aData);
            $('#cbEmpresa').multipleSelect({
                width: '150px',
                filter: true,
                onClose: function (view) {
                    cargarTipoCombustible();
                }
            });
            $('#cbEmpresa').multipleSelect('checkAll');
            cargarTipoCombustible();
        },
        error: function () {
            alert("Ha ocurrido un error");
        }
    });
}

function cargarTipoCombustible() {
    var idEmpresa = getEmpresa();
    var idTipoCentral = getTipocentral();

    $.ajax({
        type: 'POST',
        url: controlador + 'CargarTipoCombustibleXTipoCentral',
        data: { idEmpresa: idEmpresa, idTipoCentral: idTipoCentral },
        success: function (aData) {
            $('#tipocombustible').html(aData);
            $('#cbTipoCombustibleXTipoCentral').multipleSelect({
                width: '150px',
                filter: true,
                onClose: function (view) {
                    cargarLista();
                }
            });
            $('#cbTipoCombustibleXTipoCentral').multipleSelect('checkAll');
            cargarLista();
        },
        error: function (err) {
            alert("Ha ocurrido un error");
        }
    });
}

function cargarLista() {
    $('#listado').html('');
    ancho = $("#mainLayout").width() > 900 ? $("#mainLayout").width() - 30 : 900;

    $.ajax({
        type: 'POST',
        url: controlador + 'CargarCalificacionReporte',
        data: {
            idEmpresa: getEmpresa(),
            fechaInicio: getFechaInicio(),
            fechaFin: getFechaFin(),
            idTOperacion: getTipoOperacion(),
            idTCentral: getTipocentral(),
            idTCombustible: getTipoCombustibleXTipoCentral()
        },
        success: function (aData) {
            $('.filtro_fecha_desc').html(aData.FiltroFechaDesc);

            $('#listado').html(aData.Resultado);

            var anchoReporte = $('#tablaHOP').width() - 30;
            $("#listado").css("width", (anchoReporte > ancho ? ancho : anchoReporte) + "px");

            $('#tablaHOP').dataTable({
                "destroy": "true",
                "info": false,
                "searching": true,
                "paging": false,
                "scrollX": true,
                "scrollY": $('#listado').height() > 200 ? 500 + "px" : "100%"
            });

        },
        error: function (err) {
            alert("Ha ocurrido un error");
        }
    });
}
