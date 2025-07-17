var controlador = siteRoot + 'ParametrosTecnicos/';
$(function () {
    $('#tab-container').easytabs({ animate: false });
    $('#txtFechaini').Zebra_DatePicker({
        pair: $('#txtFechafin'),
        onSelect: function (date) {
            var date1 = getFecha(date);
            var date2 = getFecha($('#txtFechafin').val());
            $('#txtFechafin').val(date);
        }
    });
    $('#txtFechafin').Zebra_DatePicker({
        direction: true
    });

    $('#btnConsultar').click(function () {
        $("#exp").hide();
        $("#tab-container").hide();
        buscar(1);
    });

    $('#btnExportar').click(function () {
        buscar(2);
    });
});

function buscar(x) {
    $.ajax({
        type: 'POST',
        url: controlador + "GetParametrosTecnicos",
        data: { fechaini: $("#txtFechaini").val(), fechafin: $("#txtFechafin").val(), tip: x },
        success: function (evt) {
            if (x == 2) {
                window.location = controlador + "ExportarReporteXls?nameFile=" + evt.Resultado; // si hay elementos
            } else {
                if (evt.Count > 0) {
                    $("#exp").show();
                    $("#tab-container").show();
                    $("#listado1").html(evt.ListaResultado[0]);
                    $("#listado2").html(evt.ListaResultado[1]);
                    $("#listado3").html(evt.ListaResultado[2]);
                    $("#listado4").html(evt.ListaResultado[3]);
                    $("#listado5").html(evt.ListaResultado[4]);
                    $("#listado6").html(evt.ListaResultado[5]);

                    if (evt.ListaCount[0] > 0) { $("#tb01").dataTable(); }
                    if (evt.ListaCount[1] > 0) { $("#tb02").dataTable(); }
                    if (evt.ListaCount[2] > 0) { $("#tb03").dataTable(); }
                    if (evt.ListaCount[3] > 0) { $("#tb04").dataTable(); }
                    if (evt.ListaCount[4] > 0) { $("#tb05").dataTable(); }
                    if (evt.ListaCount[5] > 0) { $("#tb06").dataTable(); }
                } else { alert("No existe informacion en el rango de fecha de consulta"); }
            }
        },
        error: function (err) { alert("Error inconsistente"); }
    });
}