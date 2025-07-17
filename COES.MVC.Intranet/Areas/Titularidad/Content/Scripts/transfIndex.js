var controlador = siteRoot + 'Titularidad/Transferencia/';
var ancho = 1180;

$(function () {
    $('#cbTipoEmpresa').val(-2);

    $('#btnConsultarMigracion').on('click', function () {
        consultarListado();
    });

    $('#cbTipoEmpresa').change(function () {
        cargarEmpresas();
    });

    $('#btnNuevo').on('click', function () {
        document.location.href = controlador + 'Transferencia';
    });

    $('#btnExportar').on('click', function () {
        exportarReporte();
    });

    $('#btnIrAlListado').click(function () {
        document.location.href = siteRoot + 'Titularidad/Transferencia/Index';
    });

    ancho = $("#mainLayout").width() > 1180 ? $("#mainLayout").width() - 10 : 1180;

    $('#cbEmpresasOfIndex').multipleSelect({
        width: '250px',
        filter: true,
        single: true,
        onClose: function () {
        }
    });
    $('#cbEmpresasDOfIndex').multipleSelect({
        width: '250px',
        filter: true,
        single: true,
        onClose: function () {
        }
    });

    $('#cbEmpresasOfIndex').val(-2);
    $('#cbEmpresasDOfIndex').val(-2);
    $("#cbEmpresasOfIndex").multipleSelect("setSelects", [-2]);
    $("#cbEmpresasDOfIndex").multipleSelect("setSelects", [-2]);

    $('input[name=check_estado_todos]:checkbox').change(function (e) {
        consultarListado();
        view_checkMostrarAnulado();
    });
    view_checkMostrarAnulado();

    consultarListado();
});

function consultarListado() {
    $.ajax({
        type: 'POST',
        url: controlador + 'ListadoTransferencias',
        data: {
            empresaOrigen: $('#cbEmpresasOfIndex').val(),
            empresaDestino: $('#cbEmpresasDOfIndex').val(),
            descripcionMigracion: $('#txtDescripcion').val(),
            estadoAnulado: getEstado()
        },
        success: function (result) {
            $('#listado').html(result);

            view_checkMostrarAnulado();

            var anchoReporte = $('#resultado').width();
            $("#listado").css("width", (anchoReporte > ancho ? ancho : anchoReporte) + "px");

            $('#resultado').dataTable({
                "sPaginationType": "full_numbers",
                "scrollX": true,
                "scrollY": "780px",
                "scrollCollapse": true,
                "iDisplayLength": 25,
                "ordering": false,
                fixedColumns: {
                    leftColumns: 2
                }
            });
        },
        error: function (err) {
            alert("Ha ocurrido un error");
        }
    });
}

function exportarReporte() {
    $.ajax({
        type: 'POST',
        url: controlador + 'GenerarExcelListadoTransferencias',
        success: function (evt) {
            if (evt.Error == undefined) {
                window.location.href = controlador + 'DescargarReporte';
            }
            else {
                alert("Error:" + evt.Descripcion);
            }
        },
        error: function (result) {
            alert('ha ocurrido un error al descargar el archivo excel. ' + result.status + ' - ' + result.statusText + '.');
        }
    });
}

function view_checkMostrarAnulado() {
    var checked = $('input[name=check_estado_todos]:checkbox').prop("checked");
    $("#btnExportar").show();
    $("#leyenda").hide();

    if (checked) {
        $("#btnExportar").hide();
        $("#leyenda").show();
    }
}

function getEstado() {
    var estado = "0";
    if ($('#check_estado_todos').is(':checked')) {
        estado = '-1';
    }
    return estado;
}