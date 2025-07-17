var controlador = siteRoot + 'cortoplazo/comparativo/';
var numeroTotal = 31;

$(function () {

    $('#txtFecha').Zebra_DatePicker({
        direction: -1,
        onSelect: function (date) {
            $('#txtExportarDesde').val(date);
            $('#txtExportarHasta').val(date);
        }
    });

    $('#txtExportarDesde').Zebra_DatePicker({        
        direction: 0,
        onSelect: function (date) {
            var date1 = getFecha(date);
            var date2 = getFecha($('#txtExportarHasta').val());

            if (date1 > date2) {
                $('#txtExportarHasta').val(date);
            }
        }
    });

    $('#txtExportarHasta').Zebra_DatePicker({
       
    });

    $('#btnConsultar').on('click', function () {
        consultar('N');
    });

    $('#btnExportar').on('click', function () {
        consultar('S');
    });

    $('#btnExportarMasivo').on('click', function () {
        openExportarMasivo();
    });

    $('#btnOkExportarMasivo').on('click', function () {
        exportarMasivo();
    });    
});

consultar = function (option) {

    var mensaje = validacion();

    if (mensaje == '') {

        $.ajax({
            type: 'POST',
            url: controlador + 'comparativodemandaems',
            data: {
                idBarra: $('#cbBarra').val(),
                fecha: $('#txtFecha').val(),
                option: option
            },
            dataType: 'json',
            success: function (result) {

                if (option == 'N') {
                    $('#contentGrafico').hide();
                    $('#contentTabla').html('');
                }

                if (result.Resultado == 1) {

                    if (option == 'N') {
                        pintarTabla(result.ListaDatos);
                        pintarGrafico(result.ListaDatos, result.Descripcion);
                        $('#mensaje').removeClass();
                        $('#mensaje').html('');
                    }
                    else {
                        document.location.href = controlador + 'descargardemandaems?fecha=' + result.Fecha;
                    }
                }
                else if (result.Resultado == 2) {
                    mostrarMensaje('mensaje', 'alert', 'No existe equivalencia de código SCADA para la barra seleccionada.');
                }
                else if (result.Resultado == 3) {
                    mostrarMensaje('mensaje', 'alert', 'No existen datos completos en las fuentes de datos.');
                }
                else if (result.Resultado == 4) {
                    if(option == 'N')
                        mostrarMensaje('mensaje', 'alert', 'No se encontraron resultados para los filtros seleccionados.');
                    else
                        mostrarMensaje('mensaje', 'alert', 'No se puede generar el reporte ya que no existen datos del comparativo.');
                }
                else if (result.Resultado == 5) {
                    mostrarMensaje('mensaje', 'alert', 'La barra seleccionada no tiene configurado equivalencia con los códigos SCADA.');
                }
                else if (result.Resultado == -1) {
                    mostrarMensaje('mensaje', 'error', 'Ha ocurrido un error.');
                }
            },
            error: function () {
                mostrarMensaje('mensaje', 'error', 'Ha ocurrido un error.');
            }
        });
    }
    else {
        mostrarMensaje('mensaje', 'alert', mensaje);
        $('#contentGrafico').hide();
        $('#contentTabla').html('');
    }
}

pintarTabla = function (result) {

    var html = '<table class="pretty tabla-adicional">';
    html = html + ' <thead>';
    html = html + '     <tr>';
    html = html + '         <th>Hora </th>';
    html = html + '         <th>Demanda EMS (MW)</th>';
    html = html + '         <th>Demanda Ejecutada (MW)</th>';
    html = html + '         <th>Diferencia (MW)</th>';
    html = html + '         <th>Desviación (%)</th>';   
    html = html + '     </tr>';
    html = html + ' </thead>';
    html = html + ' <tbody>';
    var i = 0;
   
    for (var i in result) {

        var style = '';
        if (i % 2 == 0) style = 'background-color: #f2f5f7';

        if (result[i].Indicador == 'S') {
            style = 'background-color: #ffb4b4';
        }


        html = html + '     <tr>';
        html = html + '         <td style="text-align:center;' + style + '">' + result[i].Hora + '</td>';
        html = html + '         <td style="text-align:right;' + style + '">' + $.number(result[i].DemandaEMS, 2, '.', '') + '</td>';
        html = html + '         <td style="text-align:right;' + style + '">' + $.number(result[i].DemandaEjecutada, 2, '.', '') + '</td>';
        html = html + '         <td style="text-align:right;' + style + '">' + $.number(result[i].Diferencia, 2, '.', '') + '</td>';
        html = html + '         <td style="text-align:right;' + style + '">' + $.number(result[i].Desviacion * 100, 2, '.', '') + '</td>';

        html = html + '     </tr>';
       
        i++;
    }
   
    html = html + ' </tbody>';   
    html = html + '</table>';

    $('#contentTabla').html(html);
}

pintarGrafico = function (result, barra) {

    $('#contentGrafico').show();
    var categorias = [];
    var series = [];
    var dataDemandaEMS = [];
    var dataDemandaEjecutada = [];

    for (var i in result) {
        categorias.push(result[i].Hora);
        dataDemandaEMS.push(result[i].DemandaEMS);
        dataDemandaEjecutada.push(result[i].DemandaEjecutada);
    }

    series.push({ name: 'Demanda EMS', data: dataDemandaEMS, color: '#7CB5EC' });
    series.push({ name: 'Demanda Ejecutada', data: dataDemandaEjecutada, color: '#90ED7D' });


    $('#contentGrafico').highcharts({
        title: {
            text: 'Comparativo Demanda EMS vs Demanda Ejecutada - ' + barra,
            x: -20
        },
        xAxis: {
            categories: categorias,
            labels: {
                rotation: -90
            }
        },
        yAxis: {
            title: {
                text: '(MW)'
            },
            plotLines: [{
                value: 0,
                width: 1,
                color: '#808080'
            }]
        },
        tooltip: {
            valueSuffix: 'MW'
        },
        legend: {
            borderWidth: 0
        },
        series: series
    });
};

validacion = function () {
    var mensaje = "<ul>";
    var flag = true;

    if ($('#cbBarra').val() == "") {
        mensaje = mensaje + "<li>Por favor seleccione una barra.</li>";
        flag = false;
    }

    if ($('#txtFecha').val() == "") {
        mensaje = mensaje + "<li>Por favor seleccione una fecha.</li>";
        flag = false;
    }

    mensaje = mensaje + "</ul>";

    if (flag) mensaje = "";

    return mensaje;
};

openExportarMasivo = function () {
    $('#divExportar').show();
};

closeExportar = function () {
    $('#divExportar').hide();
};

exportarMasivo = function () {

    if ($('#txtExportarDesde').val() != "" && $('#txtExportarDesde').val() != "") {
        var date1 = getFecha($('#txtExportarDesde').val());
        var date2 = getFecha($('#txtExportarHasta').val());

        if (date1 > date2) {
            mostrarMensaje('mensajeExportar', 'alert', 'La fecha inicial no debe mayor ser a la fecha final.');
        }
        else {                     

            if (date2 < getFecha($('#hfFechaActual').val())) {
               
                var diferencia = numeroDias(date1, date2);

                if (diferencia <= numeroTotal) {

                    $.ajax({
                        type: 'POST',
                        url: controlador + 'exportardemandaemsmasivo',
                        data: {
                            fechaInicio: $('#txtExportarDesde').val(),
                            fechaFin: $('#txtExportarHasta').val()
                        },
                        dataType: 'json',
                        success: function (result) {
                            if (result.result == 1) {
                                document.location.href = controlador + 'descargardemandaemsmasivo?fecha=' + result.fecha;
                            }
                            else {
                                mostrarMensaje('mensajeExportar', 'error', 'Ha ocurrido un error.');
                            }
                        },
                        error: function () {
                            mostrarMensaje('mensajeExportar', 'error', 'Ha ocurrido un error.');
                        }
                    });
                }
                else {
                    mostrarMensaje('mensajeExportar', 'alert', 'Solo puede seleccionar cómo máximo 31 días.');
                }
            }
            else {
                mostrarMensaje('mensajeExportar', 'alert', 'La fecha final debe ser menor a la fecha actual.');
            }
        }
    }
    else {
        mostrarMensaje('mensajeExportar', 'alert', 'Debe seleccionar fecha inicial y fecha final');
    }
}

mostrarMensaje = function (id, tipo, mensaje) {
    $('#' + id).removeClass();
    $('#' + id).addClass('action-' + tipo);
    $('#' + id).html(mensaje);
}

getFecha = function (date) {
    var parts = date.split("/");
    var date = new Date(parts[1] + "/" + parts[0] + "/" + parts[2]);
    return date.getTime();
};

numeroDias = function (inicio, final) {
    return Math.round((final - inicio) / (1000 * 60 * 60 * 24));
};