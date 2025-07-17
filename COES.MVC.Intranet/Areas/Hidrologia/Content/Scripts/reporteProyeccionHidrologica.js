var controlador = siteRoot + 'hidrologia/';
var tipoInformacion = 0;
var opc = 0;
var listLectPeriodo = [];
var listLectCodi = [];
var currentValue = 0;
var flag = 0;
var flag2 = 0;

$(function () {
    $('#cbFormato').change(function () {
        cambiarFormatoFecha();
    });

    $('#FechaDesde').Zebra_DatePicker({
    });
    $('#FechaHasta').Zebra_DatePicker({

    });
    $('#Anho').Zebra_DatePicker({
        format: 'Y',
        onSelect: function () {
            cargarSemanaAnho()
        }
    });
    $('#AnhoIni').Zebra_DatePicker({
        format: 'Y',
        onSelect: function () {
            cargarSemanaAnho(1)
        }
    });
    $('#AnhoFin').Zebra_DatePicker({
        format: 'Y',
        onSelect: function () {
            cargarSemanaAnho(2)
        }
    });

    $('#btnExpotar').click(function () {
        exportarExcel();
    });

    cargarSemanaAnho(1);
    cargarSemanaAnho(2);
    cambiarFormatoFecha();
});

function exportarExcel() {
    var formatcodi = parseInt($("#cbFormato").val()) || 0;
    var fechaInicial = $("#FechaDesde").val();
    var fechaFinal = $("#FechaHasta").val();
    var anhoIni = parseInt($("#AnhoIni").val()) || 0;
    var semanaIni = parseInt($("#cbSemanaIni").val()) || 0;
    var anhoFin = parseInt($("#AnhoFin").val()) || 0;
    var semanaFin = parseInt($("#cbSemanaFin").val()) || 0;

    $.ajax({
        type: 'POST',
        url: controlador + 'reporte/GenerarArchivoReporteProyeccionHidrologica',
        data: {
            formatcodi: formatcodi,
            fechaInicial: fechaInicial,
            fechaFinal: fechaFinal,
            anhoIni: anhoIni,
            semanaIni: semanaIni,
            anhoFin: anhoFin,
            semanaFin: semanaFin
        },
        dataType: 'json',
        success: function (evt) {
            if (evt.Resultado != "-1") {
                window.location = controlador + "reporte/ExportarProyeccionHidrologica?file_name=" + evt.Resultado;
            } else {
                alert("Error: "+evt.Mensaje);
            }
        },
        error: function () {
            alert("Ha ocurrido un error");
        }
    });
}

//filtros
function cambiarFormatoFecha() {
    var formato = $('#cbFormato').val();
    var tipo = '0';
    if (formato == 35 || formato == 36) tipo = '4';
    if (formato == 30) tipo = '2';
    if (formato == 31) tipo = '1';

    $(".fila_resol_semanal").hide();
    $(".fila_resol_diario").hide();

    switch (tipo) {
        case '0': case '1':// Horas / diario
            $('#FechaDesde').Zebra_DatePicker({
            });
            $('#FechaHasta').Zebra_DatePicker({
            });

            //inicializar filtros
            var fecha = new Date();
            var dia = "0" + fecha.getDate().toString();
            dia = dia.substr(dia.length - 2, dia.length);
            var mes = "0" + (fecha.getMonth() + 1).toString();
            mes = mes.substr(mes.length - 2, mes.length);
            var stFecha = dia + "/" + mes + "/" + fecha.getFullYear();
            $('#FechaDesde').val(stFecha);
            $('#FechaHasta').val(stFecha)

            $(".fila_resol_diario").show();

            break;
        case '2': case '3':
            $(".fila_resol_semanal").show();
            break;
        case '4':// Reporte mensual
            $('#FechaDesde').Zebra_DatePicker({
                format: 'm Y'
            });
            $('#FechaHasta').Zebra_DatePicker({
                format: 'm Y'
            });

            //inicializar filtros
            var fecha = new Date();
            var mes = "0" + (fecha.getMonth() + 1).toString();
            mes = mes.substr(mes.length - 2, mes.length);
            var stFecha = mes + " " + fecha.getFullYear();
            $('#FechaDesde').val(stFecha);
            $('#FechaHasta').val(stFecha);

            $(".fila_resol_diario").show();

            break;
    }

}

function cargarSemanaAnho(tipo) {
    var idAnio = tipo == 1 ? "#AnhoIni" : "#AnhoFin";
    var idHfAnio = tipo == 1 ? "#hfAnhoIni" : "#hfAnhoFin";
    var idSem = tipo == 1 ? "cbSemanaIni" : "cbSemanaFin";
    var idDivSem = tipo == 1 ? "#divSemanaIni" : "#divSemanaFin";

    var anho = $(idAnio).val();
    $(idHfAnio).val(anho);

    $.ajax({
        type: 'POST',
        url: controlador + 'reporte/CargarSemanas',
        data: {
            idAnho: anho
        },
        success: function (aData) {
            aData = aData.replace('cbSemanas', idSem);

            $(idDivSem).html(aData);
        },
        error: function () {
            alert("Ha ocurrido un error");
        }
    });

}