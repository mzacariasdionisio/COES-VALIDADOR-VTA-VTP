var controlador = siteRoot + 'Migraciones/Reporte/'

$(function () {
    $('#txtFechaInicial').Zebra_DatePicker({
        pair: $('#txtFechaFinal'),
        onSelect: function (date) {
            var date1 = getFecha(date);
            var date2 = getFecha($('#txtFechaFinal').val());

            if (date1 > date2) {
                $('#txtFechaFinal').val(date);
            }
        }
    });

    $('#txtFechaFinal').Zebra_DatePicker({
        direction: true
    });
     
    $('#btnConsultar').click(function(){
        mostrarReporte(1);       
    });

    $('#btnExportar').click(function () {
        mostrarReporte(2);
    });

    $("#cbProg").val(6);
});

mostrarReporte = function (x)
{
    if ($('#txtFechaInicial').val() != "" && $('#txtFechaFinal').val() != "")
    {
        $.ajax({
            type: 'POST',
            url: controlador + "ReporteProduccyProgramado",
            data: {
                fechaInicial: $('#txtFechaInicial').val(), fechaFinal: $('#txtFechaFinal').val(),
                lectcodi: $('#cbProg').val(), xx: x
            },
            success: function (evt) {     
                if (x == 1) {          
                    $('#listado').html(evt.Resultado);
                } else {
                    switch (evt.nRegistros) {
                        case 1: window.location = controlador + "Exportar?fi=" + evt.Resultado; break;// si hay elementos
                        case 2: alert("No existen registros !"); break;// sino hay elementos
                        case -1: alert("Error en reporte result"); break;// Error en C#
                    }
                }
            },
            error: function (err) {
                mostrarError();
            }
        });
    }
    else
    {
        mostrarAlerta("Seleccione rango de fechas.");
    }
}

mostrarAlerta = function (mensaje) {
    alert(mensaje);
}

mostrarError = function () {
    alert("Ha ocurrido un error.");
}

getFecha = function (date) {
    var parts = date.split("/");
    var date = new Date(parts[1] + "/" + parts[0] + "/" + parts[2]);
    return date.getTime();
}

