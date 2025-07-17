var controler = siteRoot + "transferencias/infodesviacion/";

$(document).ready(function () {
    $('#btnBuscar').click(function () {
        buscar();
    });

    $('#btnGenerarExcel').click(function () {
        generarExcel();
    });

    buscar();
});

function buscar() {
    $.ajax({
        type: 'POST',
        url: controler + "Lista",
        data: { iPeriCodi: $("#iPeriCodi").val(), iVersion: $("#iVersion").val(), iDesbalance: $("#iDesbalance").val(), iBarrCodi: $("#iBarrCodi").val(), iValorizacion: $("#iValorizacion").val() },
        success: function (evt) {
            $('#listado').html(evt);
            oTable = $('#tabla').dataTable({
                "sPaginationType": "full_numbers",
                "destroy": "true"
            });
        },
        error: function () {
            alert("error en Buscar");
        }
    });
}

generarExcel = function () {
    if ($("#iPeriCodi").val() == "") {
        alert("Por favor, seleccione un periodo");
    }
    else {
        var iPericodi = $("#iPeriCodi").val();
        var iVersion = $("#iVersion").val();
        var iDesbalance = $("#iDesbalance").val();
        var iBarrCodi = $("#iBarrCodi").val();
        var iValorizacion = $("#iValorizacion").val();
        $.ajax({
            type: 'POST',
            url: controler + 'generarexcel',
            data: { iPericodi: iPericodi, iVersion: iVersion, iDesbalance: iDesbalance, iBarrCodi: iBarrCodi, iValorizacion: iValorizacion },
            dataType: 'json',
            success: function (result) {
                if (result == 1) {
                    window.location = controler + 'abrirexcel';
                }
                if (result == -1) {
                    alert("Lo sentimos, se ha producido un error");
                }
            },
            error: function () {
                alert("Lo sentimos, se ha producido un error");
            }
        });
    }
}