var controlador = siteRoot + 'Titularidad/Transferencia/';
var ancho = 1180;

$(function () {

    $('#tab-container').easytabs({
        animate: false
    });

    $('#tab-container').bind('easytabs:after', function () {
        refrehDatatable();
    });

    $('#btnIrAlListado').click(function () {
        document.location.href = siteRoot + 'Titularidad/Transferencia/Index';
    });

    $('#btnAnular').click(function () {
        if (confirm("¿Desea anular la transferencia?")) {
            anularTransferencia();
        }
    });

    $('#btnProcesarStr').click(function () {
        if (confirm("¿Desea procesar la información asociada a los aplicativos de STR?")) {
            procesarStr();
        }
    });

    refrehDatatable();
});

function anularTransferencia() {
    var migracodi = parseInt($("#hdnMigracodi").val());
    $.ajax({
        type: 'POST',
        url: controlador + 'AnularTransferencia',
        data: {
            migracodi: migracodi
        },
        dataType: 'json',
        cache: false,
        async: true,
        success: function (result) {
            var bRes = result.Resultado == "1";
            if (bRes) {
                alert('La transferencia ha sido anulada correctamente');
                mostrarDetalleTransferencia(migracodi);
            } else {
                alert('Ha ocurrido un error: ' + result.Mensaje);
            }
        },
        error: function (err) {
            alert('Ha ocurrido un error');
        }
    });
}

function procesarStr() {
    var migracodi = parseInt($("#hdnMigracodi").val());
    $.ajax({
        type: 'POST',
        url: controlador + 'ProcesarStr',
        data: {
            migracodi: migracodi
        },
        dataType: 'json',
        cache: false,
        async: true,
        success: function (result) {
            var bRes = result.Resultado == "1";
            if (bRes) {
                alert('La transferencia ha sido actualizada correctamente');
                mostrarDetalleTransferencia(migracodi);
            } else {
                alert('Ha ocurrido un error: ' + result.Mensaje);
            }
        },
        error: function (err) {
            alert('Ha ocurrido un error');
        }
    });
}


function refrehDatatable() {

    $("#reporteEquipo").dataTable({
        "scrollY": "500px",
        "destroy": "true",
        "scrollCollapse": true,
        "paging": false,
        "oLanguage": {
            "sEmptyTable": "No existen registros"
        }
    });

    $("#reportePto").dataTable({
        "scrollY": "500px",
        "destroy": "true",
        "scrollCollapse": true,
        "paging": false,
        "oLanguage": {
            "sEmptyTable": "No existen registros"
        }
    });

    $("#reporteGrupo").dataTable({
        "scrollY": "500px",
        "destroy": "true",
        "scrollCollapse": true,
        "paging": false,
        "oLanguage": {
            "sEmptyTable": "No existen registros"
        }
    });

    $("#reporteLog").dataTable({
        "scrollY": "500px",
        "ordering": false,
        "destroy": "true",
        "scrollCollapse": true,
        "paging": false,
        "oLanguage": {
            "sEmptyTable": "No existen registros"
        }
    });

    var tieneLogDBA = $("#hdnPermisoDBA").val();
    if (tieneLogDBA)
        $("#reporteDba").dataTable({
            "scrollY": "500px",
            "ordering": false,
            "destroy": "true",
            "scrollCollapse": true,
            "paging": false,
            "oLanguage": {
                "sEmptyTable": "No existen registros"
            }
        });

    $("#listadoDba").css("width", (ancho) + "px");
}