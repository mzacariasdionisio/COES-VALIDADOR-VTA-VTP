var ancho = 1200;

$(function () {

    ancho = $("#mainLayout").width() > 900 ? $("#mainLayout").width() - 30 : 900;
    $('#cbEmpresa').multipleSelect({
        width: '150px',
        filter: true,
        onClose: function (view) {
        }
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
    $.ajax({
        type: 'POST',
        url: controlador + 'CargarListaTensionBarrasSEIN',
        data: {
            idEmpresa: getEmpresa(),
            idSubEstacion: getSubestacion(),
            fechaIni: getFechaInicio(),
            fechaFin: getFechaFin()
        },
        success: function (aData) {
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
