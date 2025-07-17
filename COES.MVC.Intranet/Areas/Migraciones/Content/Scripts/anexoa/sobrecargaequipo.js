var controlador = siteRoot + 'Migraciones/AnexoA/'
$(function () {
    $('#btnBuscar').click(function () {
        cargarLista();
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
    cargarLista();
}

function cargarLista() {
    var fechaInicio = $('#txtFechaInicio').val();
    var fechaFin = $('#txtFechaFin').val();

    $.ajax({
        type: 'POST',
        url: controlador + 'CargarListaSobreCargaEquipo',
        data: { fechaInicio: fechaInicio, fechaFin: fechaFin },
        success: function (aData) {
            $('#listado').html(aData.Resultado);
            $('#reporte').dataTable({
                "bAutoWidth": false,
                "bSort": false,
                "scrollY": 500,
                "scrollX": true,
                "sDom": 't',
                "iDisplayLength": -1,
                "language": {
                    "emptyTable": "No Existen registros..!"
                }
            });
            $('#idGraficoContainer').html('');
        },
        error: function () {
            alert("Ha ocurrido un error");
        }
    });
}