var controler = siteRoot + "transfpotencia/actualizartrasempfusion/";

$(document).ready(function () {
    $('#tab-container').easytabs({
        animate: false
    });

    $("#mensaje").hide();

    $("#aPaso1").click();

    $('#btnConsultar').click(function () {
        mostrarListado();
    });
    $('#btnTrasladarSaldos').click(function () {
        trasladarSaldos();
    });

});


trasladarSaldos = function () {

    var items = checkMark();
    if (items == "0") {
        mostrarMensaje('Debe seleccionar al menor un registro.');
        return;
    }

    var pericodi = $('#pericodi').val();

    $.ajax({
        type: 'POST',
        url: controler + "trasladarSaldosVTP",
        data: { pericodi: pericodi, items: items },
        dataType: 'json',
        success: function (result) {
            mostrarMensaje("Se trasladó con éxito el saldo seleccionado.");
            mostrarListado();
        },
        error: function () {
            mostrarMensaje('Lo sentimos, ha ocurrido un error inesperado');
        }
    });
}


function mostrarListado() {
    $.ajax({
        type: 'POST',
        url: controler + "ListaSaldosSobrantes",
        data: {
            pericodi: $('#pericodi').val()
        },
        success: function (evt) {
            $('#grillaExcelSaldosSobrantes').html(evt);
            $('#tabla').dataTable({
                "filter": false,
                "ordering": true,
                "paging": false,
                "scrollY": 300,
                "scrollX": true,
                "bDestroy": true,
                "autoWidth": false,
                "columnDefs": [
                    { "width": "20%", "targets": 0 },
                    { "width": "80%", "targets": 1 }
                ]
            });
            mostrarListadoNoIdentificadas();
        },
        error: function () {
            alert("Lo sentimos, ha ocurrido un error inesperado");
        }
    });
}

function mostrarListadoNoIdentificadas() {
    $.ajax({
        type: 'POST',
        url: controler + "ListaSaldosNoIdentificados",
        data: {
            pericodi: $('#pericodi').val()
        },
        success: function (evt) {
            $('#grillaExcelCasosNoMigrados').html(evt);
            $('#tabla2').dataTable({
                "filter": false,
                "ordering": true,
                "paging": false,
                "scrollY": 300,
                "scrollX": true,
                "bDestroy": true,
                "autoWidth": false,
                "columnDefs": [
                    { "width": "20%", "targets": 0 },
                    { "width": "80%", "targets": 1 }
                ]
            });
        },
        error: function () {
            alert("Lo sentimos, ha ocurrido un error inesperado");
        }
    });
}
