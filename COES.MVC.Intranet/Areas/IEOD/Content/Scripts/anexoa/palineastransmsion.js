var ancho = 1200;
$(function () {
    ancho = $("#mainLayout").width() > 900 ? $("#mainLayout").width() - 40 : 900;

    $('#cbEmpresa').multipleSelect({
        width: '150px',
        filter: true,
        onClose: function (view) {
        }
    });

    parametro1 = $("#cbTPotencia").val();
    $('#cbTPotencia').change(function () {
        parametro1 = $("#cbTPotencia").val();
        cargarLista();
    });

    cargarValoresIniciales();
});

function cargarValoresIniciales() {
    $('#cbEmpresa').multipleSelect('checkAll');
    cargarLista();
}

function mostrarReporteByFiltros() {
    cargarLista();
}

function cargarLista() {
    var potencia = $('#cbTPotencia').val();

    $.ajax({
        type: 'POST',
        url: controlador + 'CargarListaPALineasTransmision',
        data: {
            idEmpresa: getEmpresa(),
            fechaIni: getFechaInicio(),
            fechaFin: getFechaFin(),
            idPotencia: potencia
        },
        success: function (aData) {
            $('.filtro_fecha_desc').html(aData.FiltroFechaDesc);
            $('#listado').html(aData.Resultado);

            var anchoReporte = $('#reporte').width();
            $("#resultado").css("width", (anchoReporte > ancho ? ancho : anchoReporte) + "px");

            $('#reporte').dataTable({
                "scrollX": true,
                "scrollY": 550,
                "scrollCollapse": true,
                "sDom": 't',
                "ordering": false,
                paging: false,
                fixedColumns: {
                    leftColumns: 1
                }
            });
        },
        error: function (err) {
            alert("Ha ocurrido un error");
        }
    });
}