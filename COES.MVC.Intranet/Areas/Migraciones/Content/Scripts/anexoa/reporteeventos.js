var controlador = siteRoot + 'Migraciones/AnexoA/'
$(function () {

    $('#cbEmpresa').multipleSelect({
        width: '150px',
        filter: true,
        onClose: function (view) {
            cargarUbicacion();
        }
    });

    $('#btnExportar').click(function () {
        exportarReporte();
    });

    $('#txtFechaInicio').Zebra_DatePicker({
        //direction: -1
    });

    $('#txtFechaFin').Zebra_DatePicker({
        //direction: -1
    });

    cargarValoresIniciales();


});

function cargarValoresIniciales() {
    $('#cbEmpresa').multipleSelect('checkAll');
    cargarUbicacion();
}

function buscarEventos() {
    cargarListaEventos(1);
}

function cargarUbicacion() {
    var empresa = $('#cbEmpresa').multipleSelect('getSelects');
    if (empresa == "[object Object]") empresa = "-1";
    $('#hfEmpresa').val(empresa);
    var idEmpresa = $('#hfEmpresa').val();

    if (idEmpresa != "") {
        $.ajax({
            type: 'POST',

            url: controlador + 'CargarUbicacion',
            data: { idEmpresa: idEmpresa },
            success: function (aData) {
                $('#ubicacion').html(aData);
                $('#cbUbicacion').multipleSelect({
                    width: '150px',
                    filter: true,
                    onClose: function (view) {
                        buscarEventos();
                    }
                });
                $('#cbUbicacion').multipleSelect('checkAll');
                buscarEventos();
            },
            error: function () {
                alert("Ha ocurrido un error");
            }
        });
    } else {
        alert("Debe seleccionar al menos una empresa");
        $('#cbEmpresa').multipleSelect('checkAll');
    }

}

function cargarListaEventos(nroPagina) {

    var empresa = $('#cbEmpresa').multipleSelect('getSelects');
    if (empresa == "[object Object]") empresa = "-1";
    $('#hfEmpresa').val(empresa);
    var idEmpresa = $('#hfEmpresa').val();

    var ubicacion = $('#cbUbicacion').multipleSelect('getSelects');
    if (ubicacion == "[object Object]") ubicacion = "-1";
    $('#hfUbicacion').val(ubicacion);
    var idUbicacion = $('#hfUbicacion').val();

    var fechaInicio = $('#txtFechaInicio').val();
    var fechaFin = $('#txtFechaFin').val();

    $.ajax({
        type: 'POST',
        async: false,
        url: controlador + 'CargarListaEventos',
        data: { idEmpresa: idEmpresa, idUbicacion: idUbicacion, fechaInicio: fechaInicio, fechaFin: fechaFin, nroPagina: nroPagina },
        success: function (aData) {
            $('#listado').html(aData.Resultado);
            $('#idGraficoContainer').html('');
        },
        error: function () {
            alert("Ha ocurrido un error");
        }
    });
}

function exportarReporte() {
    var empresa = $('#cbEmpresa').multipleSelect('getSelects');
    if (empresa == "[object Object]") empresa = "-1";
    $('#hfEmpresa').val(empresa);
    var idEmpresa = $('#hfEmpresa').val();

    var ubicacion = $('#cbUbicacion').multipleSelect('getSelects');
    if (ubicacion == "[object Object]") ubicacion = "-1";
    $('#hfUbicacion').val(ubicacion);
    var idUbicacion = $('#hfUbicacion').val();


    var fechaInicio = $('#txtFechaInicio').val();
    var fechaFin = $('#txtFechaFin').val();

    $.ajax({
        type: 'POST',
        url: controlador + 'GenerarArchivoListaEventos',
        data: { idEmpresa: idEmpresa, idUbicacion: idUbicacion, fechaInicio: fechaInicio, fechaFin: fechaFin },
        dataType: 'json',
        success: function (result) {
            if (result == 1)
                window.location = controlador + "ExportarReporteEnvio";
            if (result == -1)
                alert("Error en exportar reporte...");
        },
        error: function () {
            alert("Error en reporte...");
        }
    });

}
