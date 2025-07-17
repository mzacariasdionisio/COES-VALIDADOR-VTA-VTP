var controlador = siteRoot + 'Medidores/'

$(function () {



    $('#FechaDesde').change({
        format: 'Y-m'
    });

    $('#FechaHasta').change({
        format: 'Y-m'
    });

    $('#btnBuscar').click(function () {
        buscar();
    });

    $('#btnExportar').click(function () {
        exportar();
    });

    $('#cbTipoEmpresa').multipleSelect({
        width: '100%',
        filter: true,
        onClick: function (view) {
            cargarEmpresas();
        },
        onClose: function (view) {
            cargarEmpresas();
        }
    });

    $('#cbTipoGeneracion').multipleSelect({
        width: '100%',
        filter: true
    });

    $('#divReporteMaxDemanda').on('click', function () {
        $("#btnReporteMaximaDemanda").css("background-color", "var(--bs-gray-500)");
        $("#btnReporteDiagramaCarga").css("background-color", "");
        $("#btnReporteRecursoEnergetico").css("background-color", "");
        $("#btnReporteRankingDemandaPotencia").css("background-color", "");
    });

    $('#divDiagramaCarga').on('click', function () {
        $("#btnReporteMaximaDemanda").css("background-color", "");
        $("#btnReporteDiagramaCarga").css("background-color", "var(--bs-gray-500)");
        $("#btnReporteRecursoEnergetico").css("background-color", "");
        $("#btnReporteRankingDemandaPotencia").css("background-color", "");
    });

    $('#divRecursoEnergetico').on('click', function () {
        $("#btnReporteMaximaDemanda").css("background-color", "");
        $("#btnReporteDiagramaCarga").css("background-color", "");
        $("#btnReporteRecursoEnergetico").css("background-color", "var(--bs-gray-500)");
        $("#btnReporteRankingDemandaPotencia").css("background-color", "");
    });

    $('#divRankingDemandaPotencia').on('click', function () {
        $("#btnReporteMaximaDemanda").css("background-color", "");
        $("#btnReporteDiagramaCarga").css("background-color", "");
        $("#btnReporteRecursoEnergetico").css("background-color", "");
        $("#btnReporteRankingDemandaPotencia").css("background-color", "var(--bs-gray-500)");
    });

    $(window).resize(function () {
        $('#listado').css("width", $('#mainLayout').width() + "px");
    });

    cargarPrevio();
    cargarEmpresas();
    buscar();
});


cargarPrevio = function () {
    $('#cbTipoGeneracion').multipleSelect('checkAll');
    $('#cbTipoEmpresa').multipleSelect('checkAll');
}

buscar = function (nroPagina) {
    var empresa = $('#cbEmpresas').multipleSelect('getSelects');
    var tipoGeneracion = $('#cbTipoGeneracion').multipleSelect('getSelects');
    var tipoEmpresa = $('#cbTipoEmpresa').multipleSelect('getSelects');
    if (empresa == "[object Object]") empresa = "";
    $('#hfEmpresa').val(empresa);
    $('#hfTipoGeneracion').val(tipoGeneracion);
    $('#hfTipoEmpresa').val(tipoEmpresa);

    if ($('#FechaDesde').val() != "") {
        $.ajax({
            type: 'POST',
            url: controlador + "Reportes/MaximaDemandaDiaria",
            data: {
                fecha: $('#FechaDesde').val(),
                tiposEmpresa: $('#hfTipoEmpresa').val(),
                empresas: $('#hfEmpresa').val(), tiposGeneracion: $('#hfTipoGeneracion').val(),
                central: $('#cbCentral').val()
            },
            success: function (evt) {
                $('#listado').css("width", $('#mainLayout').width() + "px");
                $('#listado').html(evt);
                $('#tabla').dataTable({
                    "scrollY": 430,
                    "scrollX": true,
                    "sDom": 't',
                    "ordering": false,
                    "bPaginate": false,
                    "iDisplayLength": -1
                });
            },
            error: function () {
                mostrarError();
            }
        });
    }
    else {
        alert("Por favor seleccione mes.");
    }
}

cargarEmpresas = function () {
    var tipoEmpresa = $('#cbTipoEmpresa').multipleSelect('getSelects');
    $('#hfTipoEmpresa').val(tipoEmpresa);
    $.ajax({
        type: 'POST',
        url: controlador + "Reportes/empresas",
        data: {
            tiposEmpresa: $('#hfTipoEmpresa').val()
        },
        success: function (evt) {
            $('#empresas').html(evt);
        },
        error: function () {
            mostrarError();
        }
    });
}



exportar = function () {
    var empresa = $('#cbEmpresas').multipleSelect('getSelects');
    var tipoGeneracion = $('#cbTipoGeneracion').multipleSelect('getSelects');
    var tipoEmpresa = $('#cbTipoEmpresa').multipleSelect('getSelects');

    if (empresa == "[object Object]") empresa = "";

    $('#hfEmpresa').val(empresa);
    $('#hfTipoGeneracion').val(tipoGeneracion);
    $('#hfTipoEmpresa').val(tipoEmpresa);

    if ($('#FechaDesde').val() != "") {

        $.ajax({
            type: 'POST',
            url: controlador + "Reportes/exportar",
            dataType: 'json',
            data: {
                fecha: $('#FechaDesde').val(),
                tiposEmpresa: $('#hfTipoEmpresa').val(),
                empresas: $('#hfEmpresa').val(), tiposGeneracion: $('#hfTipoGeneracion').val(),
                central: $('#cbCentral').val()
            },
            success: function (result) {
                if (result == "1") {
                    window.location = controlador + 'Reportes/descargar'
                }
                else {
                    alert(result);
                }
            },
            error: function () {
                mostrarError();
            }
        });
    }
    else {
        alert("Por favor seleccione mes.")
    }
}

mostrarError = function () {
    alert("Ha ocurrido un error.");
}