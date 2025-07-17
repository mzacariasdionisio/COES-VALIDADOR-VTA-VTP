var controlador = siteRoot + 'IndicadoresSup/reporteejecutivo/';

$(function () {

    $('.pickermes').Zebra_DatePicker({
        format: 'm Y',
        onSelect: function () {
            //setearFechas();
            listaVersionesReporte();
        }
    });

    $('#btnGenerar').click(function () {
        saveGenerarVersion();
    });

    listaVersionesReporte();

});

function saveGenerarVersion() {
    var mes = $('#txtMesAnio').val().replace(" ", "/");
    if (confirm("Se creara version del mes " + mes + ". ¿Desea continuar?")) {
        $.ajax({
            type: 'POST',
            url: controlador + 'SaveGenerarVersion',
            data: { fecha: $('#txtMesAnio').val() },
            success: function (aData) {
                if (aData.Total > 0) {
                    alert("Version registrada correctamente..");
                    listaVersionesReporte();
                }
            },
            error: function (err) {
                alert("Ha ocurrido un error");
            }
        });
    }
}


function setearFechas() {
    $.ajax({
        type: 'POST',
        url: controlador + 'SetearFechaFilter',
        data: { fec1: $('#txtMesAnio').val(), fec2: $('#txtMesAnio').val(), versi: $("#cboVersiones").val()  },
        dataType: 'json',
        success: function (e) {
            if (e.Total > 0) {
                $("#viewMenu").show();
            }
        },
        error: function (err) {
            alert("Ha ocurrido un error");
        }
    });
}

function listaVersionesReporte() {
    $("#viewMenu").hide();
    fec1 = $("#txtMesAnio").val();
    $.ajax({
        type: 'POST',
        url: controlador + 'ListaVersionesReporte',
        data: { fecha: fec1 },
        success: function (e) {
            $('#cbVersion').html(e);
        },
        error: function (err) {
            alert("Ha ocurrido un error");
        }
    });
}