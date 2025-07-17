var controlador = siteRoot + 'Migraciones/AnexoA/';
var ancho = 1200;

$(function () {
    $('#cbEmpresa').multipleSelect({
        width: '150px',
        filter: true,
        onClose: function (view) {
            cargarSubEstacion();
        }
    });

    ancho = $("#mainLayout").width() > 1200 ? $("#mainLayout").width() : 1200;

    cargarValoresIniciales();
});

function cargarValoresIniciales() {
    $('#cbEmpresa').multipleSelect('checkAll');
    cargarSubEstacion();
}

function cargarSubEstacion() {
    $.ajax({
        type: 'POST',
        url: controlador + 'CargarSubEstacionFlujoPotencia',
        data: { idEmpresa: getEmpresa() },
        success: function (aData) {
            $('#subestacion').html(aData);
            $('#cbSubEstacion').multipleSelect({
                width: '150px',
                filter: true
            });
            cargarLista();
        },
        error: function () {
            alert("Ha ocurrido un error");
        }
    });
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

            $("#resultado").css("width", ancho + "px");
            $('#reporte').dataTable({
                "scrollX": true,
                "scrollCollapse": true,
                "sDom": 't',
                "ordering": false,
                paging: false,
                fixedColumns: {
                    leftColumns: 1
                }
            });
        },
        error: function () {
            alert("Ha ocurrido un error");
        }
    });
}
