var controlador = siteRoot + 'tiemporeal/lecturasp7/';
var plot = null;

$(function () {

    $('#tab-container').easytabs({
        animate: false
    });

    $('#tab-container-principal').easytabs({
        animate: false
    });




    $('#txtFechaDesde').Zebra_DatePicker({
        readonly_element: false,
        onSelect: function (date) {
            $('#txtFechaDesde').val(date);
        }
    });


    $('#txtFechaDesde').inputmask({
        mask: "1/2/y h:s",
        placeholder: "dd/mm/yyyy hh:mm",
        alias: "datetime",
        hourFormat: "24"
    });



    $('#txtFechaHasta').Zebra_DatePicker({
        readonly_element: false,
        onSelect: function (date) {
            $('#txtFechaHasta').val(date);
        }
    });


    $('#txtFechaHasta').inputmask({
        mask: "1/2/y h:s",
        placeholder: "dd/mm/yyyy hh:mm",
        alias: "datetime",
        hourFormat: "24"
    });




    $('#cbGps').val(1);




    $('#btnBuscar').click(function () {
        buscar();
    });


    $('#btnExportar').click(function () {
        exportarFrecuencia();

    });




});



exportarFrecuencia = function () {

    $('#hfFechaDesde').val($('#txtFechaDesde').val()),
    $('#hfFechaHasta').val($('#txtFechaHasta').val()),


    $.ajax({
        type: 'POST',
        url: controlador + "exportarFrecuencia",
        data: {
            gpsNombre: $('#hfGpsNombre').val(),
            fechaIni: $('#hfFechaDesde').val(),
            fechaFin: $('#hfFechaHasta').val()
        },
        dataType: 'json',
        success: function (resultado) {

            if (resultado != 1) {
                mostrarError();
            }
            else {
                window.location = controlador + "descargar";
            }

        },
        error: function () {
            mostrarError();
        }
    });
}
obtenerNombreGps = function () {

    var e = document.getElementById("cbGps");
    var nombreGps = e.options[e.selectedIndex].text;


    $('#hfGpsNombre').val(nombreGps);



}






buscar = function () {



    $('#tab-container').easytabs('disable', '#paso2');


    if ($('#cbReporte').val() != "frecuencia") {
        if (diferenciaHoras() > 24) {
            alert("Máximo tiempo de consulta: 24 horas.\r\nNo se puede continuar...");
            return;
        }
    }



    ($('#cbReporte').val() == "frecuencia" ? $('#tab1Paso1').show() : $('#tab1Paso1').hide());


    switch ($('#cbReporte').val()) {
        case "frecuencia":

            if (diferenciaHoras() > 6) {
                alert("Máximo tiempo de consulta: 6 horas.\r\nNo se puede continuar...");
                return;
            }

            $('#tab-container').easytabs('select', '#paso1');
            generarGrafico();

            break;
        case "maximofrecuencia": //maxima frecuencia
            $('#tab-container').easytabs('select', '#paso2');

            obtenerListado(1, "");
            break;
        case "minimofrecuencia": //minima frecuencia
            $('#tab-container').easytabs('select', '#paso2');
            obtenerListado(2, "");
            break;
        case "subita": //subita
            $('#tab-container').easytabs('select', '#paso2');

            var transgresion = ($('#cbxTransgresiones').is(':checked') ? "S" : "T");

            obtenerListado(3, transgresion);

            break;
        case "sostenida": //sostenida

            $('#tab-container').easytabs('select', '#paso2');
            var transgresion = ($('#cbxTransgresiones').is(':checked') ? "S" : "T");
            obtenerListado(4, transgresion);

            break;
        default:
            $('#tab-container').easytabs('select', '#paso2');
            break

    }




}

cargaropcion = function () {

    ($('#cbReporte').val() == "subita" || $('#cbReporte').val() == "sostenida" ? $('#DivTransgresiones').show() : $('#DivTransgresiones').hide());
    ($('#cbReporte').val() == "frecuencia" ? $('#btnExportar').show() : $('#btnExportar').hide());


}


diferenciaHoras = function () {



    var fechaIni = obtenerFecha('txtFechaDesde');
    var fechaFin = obtenerFecha('txtFechaHasta');

    var dif = fechaFin.getTime() - fechaIni.getTime();



    var horasTranscurridas = dif / (1000 * 60 * 60);


    return horasTranscurridas;
}



obtenerFecha = function (id) {

    var valor = $('#' + id).val() + " ";



    var date = valor.split(' ')[0].split('/');
    var time = valor.split(' ')[1].split(':');


    var date0 = (validarNumero(date[0])) ? date[0] : "00";
    var date1 = (validarNumero(date[1])) ? date[1] : "00";
    var date2 = (validarNumero(date[2])) ? date[2] : "0000";

    var time0 = (validarNumero(time[0])) ? time[0] : "00";
    var time1 = (validarNumero(time[1])) ? time[1] : "00";
    var time2 = "00";//(validarNumero(time[2])) ? time[2] : "00";


    $('#' + id).val(date0 + "/" + date1 + "/" + date2 + " " + time0 + " " + time1);

    var d = new Date(date2, parseInt(date1) - 1, date0, time0, time1, time2, 0);


    return d;
}


validarNumero = function (valor) {
    return !isNaN(parseFloat(valor)) && isFinite(valor);
}

pintarPaginado = function () {

    $.ajax({
        type: 'POST',
        url: controlador + "paginado",
        data: {
            gpsCodi: $('#cbGps').val(),
            fechaHoraIni: $('#txtFechaDesde').val(),
            fechaHoraFin: $('#txtFechaHasta').val()
        },
        success: function (evt) {
            $('#paginado').html(evt);
            mostrarPaginado();
        },
        error: function () {
            mostrarError();
        }
    });
}

pintarBusqueda = function (nroPagina) {
    mostrarListado(nroPagina);
}



mostrarListado = function (nroPagina) {



    $('#hfNroPagina').val(nroPagina);
    $.ajax({
        type: 'POST',
        url: controlador + "lista",
        data: {
            gpsCodi: $('#cbGps').val(),
            fechaHoraIni: $('#txtFechaDesde').val(),
            fechaHoraFin: $('#txtFechaHasta').val(),
            nroPage: nroPagina
        },
        success: function (evt) {
            $('#listado').css("width", $('#mainLayout').width() + "px");
            $('#listado').html(evt);
            $('#tabla').dataTable({
                "scrollY": 430,
                "scrollX": true,
                "sDom": 't',
                "ordering": false,
                "iDisplayLength": 50
            });

        },
        error: function () {
            mostrarError();
        }
    });
}


mostrarListaGrafica = function () {


    $.ajax({
        type: 'POST',
        url: controlador + "listagrafica",
        data: {
            gpsCodi: $('#cbGps').val(),
            fechaHoraIni: $('#txtFechaDesde').val(),
            fechaHoraFin: $('#txtFechaHasta').val()
        },
        success: function (evt) {

            $('#listado').css("width", $('#mainLayout').width() + "px");
            $('#listado').html(evt);
            $('#tabla').dataTable({
                "scrollY": 430,
                "scrollX": false,
                "sDom": 't',
                "ordering": false,
                "iDisplayLength": -1
            });

            generarGrafico();

        },
        error: function () {
            mostrarError();
        }
    });
}


mostrarListaGraficaSinParametro = function () {


    $.ajax({
        type: 'POST',
        url: controlador + "listagrafica",
        data: {
            gpsCodi: $('#cbGps').val()
        },
        success: function (evt) {
            $('#listado').css("width", "20 " + "px");
            $('#listado').html(evt);
            $('#tabla').dataTable({
                "scrollY": 430,
                "scrollX": false,
                "sDom": 't',
                "ordering": false,
                "iDisplayLength": -1
            });



        },
        error: function () {
            mostrarError();
        }
    });


}


function generarGrafico() {
    obtenerNombreGps();

    if ($('#cbGps').val() == "0" || $('#cbGps').val() == null || $('#cbGps').val() == undefined) {
        alert("Seleccione un GPS");
        return;
    }

    $.ajax({
        type: 'POST',
        url: controlador + "GraficoFrecuencia",
        data: {
            gpsCodi: $('#cbGps').val(),
            fechaHoraIni: $('#txtFechaDesde').val(),
            fechaHoraFin: $('#txtFechaHasta').val(),
            gpsNombre: $('#hfGpsNombre').val()
        },
        dataType: 'json',
        success: function (result) {
            if (result.Grafico.SerieDataS[0].length > 0) {
                graficoFrecuencia(result);
            }
            else {
                graficoFrecuencia(result);
            }



            mostrarListaGraficaSinParametro();





        },
        error: function () {
            alert("Ha ocurrido un error en generar grafico");
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
            fechaIni: $('#txtFechaDesde').val(),
            fechaFin: $('#txtFechaHasta').val(),
            gpsNombre: $('#hfGpsNombre').val()
        },
        success: function () {

            mostrarListaGraficaSinParametro();
        },
        error: function () {
            mostrarError();
        }
    });

}





graficoFrecuencia = function (result) {
    var series = [];
    var series1 = [];

    for (k = 0; k < result.Grafico.SerieDataS[0].length; k++) {
        var seriePoint = [];
        var now = parseJsonDate(result.Grafico.SerieDataS[0][k].X);
        var nowUTC = Date.UTC(now.getFullYear(), now.getMonth(), now.getDate(), now.getHours(), now.getMinutes(), now.getSeconds());
        seriePoint.push(nowUTC);
        seriePoint.push(result.Grafico.SerieDataS[0][k].Y);
        series.push(seriePoint);
    }

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
        yAxis: [{
            title: {
                text: result.Grafico.YAxixTitle[0]
            }
        }],
        legend: {
            layout: 'horizontal',
            align: 'center',
            verticalAlign: 'bottom',
            borderWidth: 0,
            enabled: true
        },
        series: []
    };
    opcion.series.push({
        name: result.Grafico.Series[0].Name,
        color: result.Grafico.Series[0].Color,
        data: series,
        type: result.Grafico.Series[0].Type
    });


    $('#graficos').highcharts('StockChart', opcion);
}







mostrarError = function () {

    alert('Ha ocurrido un error.');
}

