var controlador = siteRoot + 'Migraciones/AnexoA/'
$(function () {

    $('#cbEmpresa').multipleSelect({
        width: '150px',
        filter: true,
        onClose: function (view) {
            cargarListaVertimientosPeriodoVolumen()
        }
    });

    $('#btnBuscar').click(function () {
        cargarListaVertimientosPeriodoVolumen();
    });
    cargarValoresIniciales();

});

function cargarValoresIniciales() {
    $('#cbEmpresa').multipleSelect('checkAll');
    cargarListaVertimientosPeriodoVolumen()
}

function cargarListaVertimientosPeriodoVolumen() {
    var empresa = $('#cbEmpresa').multipleSelect('getSelects');
    if (empresa == "[object Object]") empresa = "-1";
    $('#hfEmpresa').val(empresa);

    var idEmpresa = $('#hfEmpresa').val();

    var fechaInicio = $('#txtFechaInicio').val();
    var fechaFin = $('#txtFechaFin').val();

    $.ajax({
        type: 'POST',
        url: controlador + 'CargarListaVertimientosPeriodoVolumen',
        data: { idEmpresa: idEmpresa, fechaInicio: fechaInicio, fechaFin: fechaFin },
        success: function (aData) {
            $('#listado').html(aData.Resultado);
        },
        error: function () {
            alert("Ha ocurrido un error");
        }
    });
}