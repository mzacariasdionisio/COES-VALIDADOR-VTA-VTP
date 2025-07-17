var controlador = siteRoot + 'tiemporeal/circularsp7/'
var plot = null;

$(function() {

    //document.getElementById("cbZona").options[0].text = "SELECCIONAR UBICACIÓN";
    //$("#cbZona").multipleSelect({});
    //$("#cbCanal").multipleSelect({});
    $('#txtFechaDesde').Zebra_DatePicker({
        format: 'Y-m-d',
        pair: $('#txtFechaHasta'),
        onSelect: function (date) {
            var date1 = getFecha(date);
            var date2 = getFecha($('#txtFechaHasta').val());
            if (date1 > date2) {
                $('#txtFechaHasta').val(date);
            }
        }
    });

    $('#txtFechaHasta').Zebra_DatePicker({
        format: 'Y-m-d',
        direction: true // solo fechas futuras
    });
    new SlimSelect({
        select: '#cbZona'
    });

    new SlimSelect({
        select: '#cbCanal'
    });

    document.getElementById("cbCanal").style.fontFamily = 'Courier New';

    $('#tab-container').easytabs({
        animate: false
    });

    $('#tab-container-principal').easytabs({
        animate: false
    });

    $('#cbZona').change(function() {
        cargarCanalPorZona($('#cbZona').val());
    });


    $('#cbZona').val(0);


    $('#btnBuscar').click(function() {
        buscar();
    });


    $('#btnExportar').click(function () {
        exportar();

    });

    $('#btnExportarCSV').click(function () {
        exportarCSV();

    });


    $('#btnDia').click(function() {
        configuraHoraDiario();

    });

    $("#txtHoraIni").inputmask("h:s", { "placeholder": "hh/mm" });
    $("#txtHoraFin").inputmask("h:s", { "placeholder": "hh/mm" });


});


obtenerHora = function(id) {

    var valor = $('#' + id).val();
    var time = valor.split(':');
    var time0 = (validarNumero(time[0])) ? time[0] : "00";
    var time1 = (validarNumero(time[1])) ? time[1] : "00";


    $('#' + id).val(time0 + ":" + time1);


}


validarNumero = function(valor) {
    return !isNaN(parseFloat(valor)) && isFinite(valor);
}


configuraHoraDiario = function () {

    $('#txtHoraIni').val("00:00");
    $('#txtHoraFin').val("23:59");
}

cargarCanalPorZona = function(zonacodi) {


    $.ajax({
        type: 'POST',
        url: controlador + "listacanalporzona",
        data: {
            zonaCodi: $('#cbZona').val()
        },
        dataType: 'json',
        cache: false,
        success: function(evt) {

            var _len = evt.length;

            var cad1 = _len + "\r\n";

            $('#cbCanal').empty();

            for (i = 0; i < _len; i++) {

                post = evt[i];

                var select = document.getElementById("cbCanal");
                select.options[select.options.length] = new Option(post.Canalnomb, post.Canalcodi);

            }

            //$("#cbCanal").multipleSelect("refresh");
        },
        error: function(xhr, textStatus, exceptionThrown) {
            mostrarError();
        }
    });

}

exportar = function () {

    $('#hfFechaDesde').val(getFechaDesde()),
        $('#hfFechaHasta').val(getFechaHasta()),
        $.ajax({
            type: 'POST',
            url: controlador + "exportar",
            traditional: true,
            data: {
                listCanalCodi: $('#cbCanal').val(),
                canalNombre: $('#hfCanal').val(),
                fechaIni: getFechaDesde() + ' ' + $('#txtHoraIni').val(),
                fechaFin: getFechaHasta() + ' ' + $('#txtHoraFin').val(),
                filtro: $('#cbCalidad').val()
            },
            dataType: 'json',
            success: function (resultado) {

                if (resultado != 1) {
                    mostrarError();
                } else {
                    window.location = controlador + "descargar";
                }

            },
            error: function () {
                mostrarError();
            }
        });
}

exportarCSV = function () {

    $('#hfFechaDesde').val(getFechaDesde()),
        $('#hfFechaHasta').val(getFechaHasta()),
        $.ajax({
            type: 'POST',
            url: controlador + "exportarCSV",
            traditional: true,
            data: {
                listCanalCodi: $('#cbCanal').val(),
                canalNombre: $('#hfCanal').val(),
                fechaIni: getFechaDesde() + ' ' + $('#txtHoraIni').val(),
                fechaFin: getFechaHasta() + ' ' + $('#txtHoraFin').val(),
                filtro: $('#cbCalidad').val()
            },
            dataType: 'json',
            success: function (resultado) {

                if (resultado != 1) {
                    mostrarError();
                } else {
                    window.location = controlador + "DescargarCSV";
                }

            },
            error: function () {
                mostrarError();
            }
        });
}

obtenerNombreGps = function() {

    var e = document.getElementById("cbCanal");
    var nombreCanal = e.options[e.selectedIndex].text;


    $('#hfCanal').val(nombreCanal);
    
}


buscar = function() {

    var canalcodi = $('#cbCanal').val();

    obtenerHora('txtHoraIni');
    obtenerHora('txtHoraFin');

    if ($('#txtHoraIni').val() == $('#txtHoraFin').val()) {
        alert("Hora inicial y final no pueden ser iguales.")
        return;
    }

    var fechaIni = Date.parse('2016-01-01 ' + $('#txtHoraIni').val() + ':00');
    var fechaFin = Date.parse('2016-01-01 ' + $('#txtHoraFin').val() + ':00');


    if (fechaIni >= fechaFin) {
        alert("Revisar Hora inicial y final");
        return;
    }


    if (canalcodi == null) {
        alert("Debe elegir una señal");
        return;
    }


    $('#tab-container').easytabs('select', '#paso1');
    generarGrafico();

}

cargaropcion = function() {

    ($('#cbReporte').val() == "subita" || $('#cbReporte').val() == "sostenida"
        ? $('#DivTransgresiones').show()
        : $('#DivTransgresiones').hide());
    ($('#cbReporte').val() == "frecuencia" ? $('#btnExportar').show() : $('#btnExportar').hide());

}


mostrarListaGrafica = function() {


    $.ajax({
        type: 'POST',
        url: controlador + "ListaGraficaHistorial",
        data: {
            canalCodi: $('#cbCanal').val()
        },
        success: function(evt) {

            $('#listado').css("width", $('#mainLayout').width() + "px");
            $('#listado').html(evt);
            //$('#tabla').dataTable({
            //    "scrollY": 430,
            //    "scrollX": false,
            //    "sDom": 't',
            //    "ordering": false,
            //    "iDisplayLength": -1
            //});

            generarGrafico();

        },
        error: function() {
            mostrarError();
        }
    });
}


mostrarListaGraficaSinParametro = function() {


    $.ajax({
        type: 'POST',
        url: controlador + "ListaGraficaHistorial",
        traditional: true,
        data: {
            canalCodi: $('#cbCanal').val()
        },
        success: function(evt) {
            $('#listado').css("width", "20 " + "px");
            $('#listado').html(evt);
            //$('#tabla').dataTable({
            //    "scrollY": 430,
            //    "scrollX": false,
            //    "sDom": 't',
            //    "ordering": false,
            //    "iDisplayLength": -1
            //});


        },
        error: function() {
            mostrarError();
        }
    });


}


function generarGrafico() {

    obtenerNombreGps();

    $.ajax({
        type: 'POST',
        url: controlador + "Grafico",
        traditional: true,
        data: {
            canalCodi: $('#cbCanal').val(),
            fechaHoraIni: getFechaDesde() + ' ' + $('#txtHoraIni').val(),
            fechaHoraFin: getFechaHasta() + ' ' + $('#txtHoraFin').val(),
            canalNombre: $('#hfCanal').val(),
            filtro: $('#cbCalidad').val()
        },
        dataType: 'json',
        cache: false,
        success: function(result) {
            if (result.Grafico != null) {
                if (result.Grafico.SerieDataS[0].length > 0) {
                    grafico(result);
                } else {
                    grafico(result);
                }
            }

            mostrarListaGraficaSinParametro();


        },
        error: function(jqXHR, exception) {
            alert("Ha ocurrido un error en generar grafico: " + exception + " - " + jqXHR.status);
        }
    });

}


function obtenerListado(opcion, transgresion) {

    obtenerNombreGps();

    $.ajax({
        type: 'POST',
        url: controlador + "Listado",
        data: {
            opcion: opcion,
            transgresion: transgresion,
            gpsCodi: $('#cbGps').val(),
            fechaIni: getFechaDesde(),
            fechaFin: getFechaHasta(),
            gpsNombre: $('#hfGpsNombre').val()
        },
        success: function() {

            mostrarListaGraficaSinParametro();
        },
        error: function() {
            mostrarError();
        }
    });

}


grafico = function(result) {
    var series = [];
    var series1 = [];

    for (var pos = 0; pos < result.Grafico.SerieDataS.length; pos++) {
        series[pos] = [];
        for (k = 0; k < result.Grafico.SerieDataS[pos].length; k++) {
            var seriePoint = [];
            var now = parseJsonDate(result.Grafico.SerieDataS[pos][k].X);
            var nowUTC = Date.UTC(now.getFullYear(),
                now.getMonth(),
                now.getDate(),
                now.getHours(),
                now.getMinutes(),
                now.getSeconds());
            seriePoint.push(nowUTC);
            seriePoint.push(result.Grafico.SerieDataS[pos][k].Y);
            series[pos].push(seriePoint);
        }
    }

    /*
        for (k = 0; k < result.Grafico.SerieDataS[0].length; k++) {
            var seriePoint = [];
            var now = parseJsonDate(result.Grafico.SerieDataS[0][k].X);
            var nowUTC = Date.UTC(now.getFullYear(),
                now.getMonth(),
                now.getDate(),
                now.getHours(),
                now.getMinutes(),
                now.getSeconds());
            seriePoint.push(nowUTC);
            seriePoint.push(result.Grafico.SerieDataS[0][k].Y);
            series.push(seriePoint);
        }*/

    console.log(series);


    var opcion = {
        rangeSelector: {
            selected: 1
        },

        title: {
            text: result.Grafico.titleText,
            style: {
                fontSize: '8'
            }
        },
        yAxis: [
            {
                title: {
                    text: result.Grafico.YAxixTitle[0]
                }
            }
        ],
        legend: {
            layout: 'horizontal',
            align: 'center',
            verticalAlign: 'bottom',
            borderWidth: 0,
            enabled: true
        },
        series: []
    };
    
    for (var pos = 0; pos < result.Grafico.SerieDataS.length; pos++) {
        opcion.series.push({
            name: result.Grafico.Series[pos].Name,
            //color: result.Grafico.Series[pos].Color,
            data: series[pos],
            type: result.Grafico.Series[pos].Type
        });
    }

    /*
        opcion.series.push({
            name: result.Grafico.Series[0].Name,
            color: result.Grafico.Series[0].Color,
            data: series,
            type: result.Grafico.Series[0].Type
        });*/


    $('#graficos').highcharts('StockChart', opcion);
    /*
    Highcharts.chart('graficos', {

        title: {
            text: 'Solar Employment Growth by Sector, 2010-2016'
        },

        subtitle: {
            text: 'Source: thesolarfoundation.com'
        },

        yAxis: {
            title: {
                text: 'Number of Employees'
            }
        },
        legend: {
            layout: 'vertical',
            align: 'right',
            verticalAlign: 'middle'
        },

        plotOptions: {
            series: {
                pointStart: 2010
            }
        },

        series: opcion.series

    });*/
}


mostrarError = function() {

    alert('Ha ocurrido un error.');
}

function getFechaDesde() {
    return convertirFecha($('#txtFechaDesde').val());
}
function getFechaHasta() {
    return convertirFecha($('#txtFechaHasta').val());
}

function convertirFecha(fecha) {
    const partes = fecha.split('-');
    const año = partes[0];
    const mes = partes[1];
    const día = partes[2];
    return `${día}/${mes}/${año}`;
}
