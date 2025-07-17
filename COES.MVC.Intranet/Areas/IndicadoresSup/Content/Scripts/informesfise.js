var controlador = siteRoot + 'IndicadoresSup/InformesFISE/';

var TipoEstado = Object.freeze(
    {
        OK: 1,
        ERROR: 0
    }
);

$(function () {


    $('#tab-container,#tab-container2').easytabs({
        animate: false
    });

    $('#cboElemento').multipleSelect({
        width: '400px',
        filter: true,
        single: true,
        onClose: function () {

            var value = $('#cboElemento').multipleSelect('getSelects');
            if (value === "") return;
            if (value.length === 0) return;
            var numAnexo = $("#tab-container2 .tab.active").data("value");
            CargarAnexo(value[0], numAnexo);
        }
    });

    $('#tab-container2').bind('easytabs:midTransition', function () {

        var value = $('#cboElemento').multipleSelect('getSelects');
        if (value === "") return;
        if (value.length === 0) return;
        var numAnexo = $("#tab-container2 .tab.active").data("value");
        CargarAnexo(value[0], numAnexo);

    });

    $('.pickermes').Zebra_DatePicker({
        format: 'm Y',
        onSelect: function () {
            CargarInformesFise();
            CargarListaCompensacion();
        }
    });

    CargarInformesFise();
});

function CargarInformesFise() {
    $.ajax({
        type: 'POST',
        url: controlador + 'CargarInformesFise',
        data: { mesanio: $("#txtMesAnio").val() },
        success: function (aData) {
            $("#listado1").html(aData.Resultados[0]);

            var table2 = $('#tab-container').width() - 50 + "px";
            $('.table-list2').css("width", table2);

            $("#listado2_1").html(aData.Resultados[1]);
            $("#listado2_2").html(aData.Resultados[2]);
            $("#listado2_3").html(aData.Resultados[3]);

            $("#listado3_1").html(aData.Resultados[4]);
            $("#listado3_2").html(aData.Resultados[5]);
            $("#listado3_3").html(aData.Resultados[6]);

            $("#tabla_1").dataTable({
                "pageLength": 50
            });

        },
        error: function (err) { alert("Ha ocurrido un error"); }
    });
}

function CargarListaCompensacion() {

    $.ajax({
        type: 'POST',
        url: controlador + 'CargarListaCompensacion',
        data: { mesanio: $("#txtMesAnio").val() },
        success: function (aData) {
            $("#cboElemento option").remove();
            $.each(aData, function (i, val) {
                $('#cboElemento').append(new Option(val.Text, val.Value));
            });
            $('#cboElemento').multipleSelect("refresh");
            Limpiar();
        },
        error: function (err) { alert("Ha ocurrido un error"); }
    });
}

function CargarAnexo(stcompcodi, numeroAnexo) {

    $.ajax({
        type: 'POST',
        url: controlador + 'CargarAnexo',
        data: {
            mesanio: $("#txtMesAnio").val(),
            stcompcodi: parseInt(stcompcodi),
            numeroAnexo: numeroAnexo
        },
        success: function (aData) {
            if (aData.TipoEstado === TipoEstado.OK) {
                $("#listado" + numeroAnexo).html(aData.Resultado);
            } else {
                alert("Ha ocurrido un error");
            }
        },
        error: function (err) { alert("Ha ocurrido un error"); }
    });

}

function Limpiar() {
    $("#listado4").empty();
    $("#listado5").empty();
    $("#listado6").empty();
}

 
function ExportarInformeFise() {
    $.ajax({
        type: 'POST',
        url: controlador + 'ExportarFise',
        data: {
            mesanio: $("#txtMesAnio").val()
        },
        success: function (aData) {
            switch (aData.TipoEstado) {
                case TipoEstado.OK: window.location = controlador + "Exportar?fi=" + aData.Resultado; break;// si hay elementos
                //case 2: alert("No existen registros !"); break;// sino hay elementos
                case TipoEstado.ERROR: alert("Error en reporte result"); break;// Error en C#
            }
        },
        error: function (err) { alert("Ha ocurrido un error: " + err.ResponseText); }
    });
}



