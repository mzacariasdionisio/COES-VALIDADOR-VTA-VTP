var controlador = siteRoot + 'cortoplazo/comparativo/';

$(function () {

    $('#txtFecha').Zebra_DatePicker({
        direction: -1
    });     

    $('#btnConsultar').on('click', function () {
        consultar('N');
    });

    $('#btnExportar').on('click', function () {
        consultar('S');
    });
   
});

consultar = function (option) {

    var mensaje = validacion();

    if (mensaje == '') {

        $.ajax({
            type: 'POST',
            url: controlador + 'comparativocostosmarginales',
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

                if (result.Resultado == 1 || result.Resultado == 5) {
                    $('#mensaje').removeClass();
                    $('#mensaje').html('');

                    if (option == 'N') {
                        pintarTabla(result.ListaDatos);
                        pintarGrafico(result.ListaDatos, result.Descripcion);
                        $('#mensaje').removeClass();
                        $('#mensaje').html('');
                    }
                    else {
                        document.location.href = controlador + 'descargarcostosmarginales?fecha=' + result.Fecha;
                    }

                    if (result.Resultado == 5) {
                        mostrarMensaje('mensaje', 'alert', 'La barra seleccionada no tiene configurado su equivalencia con la barra YUPANA.');
                    }
                }
                else if (result.Resultado == 2) {
                    mostrarMensaje('mensaje', 'alert', 'No existe equivalencia de código SCADA para la barra seleccionada.');
                }
                else if (result.Resultado == 3) {
                    mostrarMensaje('mensaje', 'alert', 'No existen datos completos en las fuentes de datos.');
                }
                else if (result.Resultado == 4) {
                    mostrarMensaje('mensaje', 'alert', 'No existen datos para los filtros seleccionados.');
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
    html = html + '         <th>CM <br />Aplicativo (S/.)</th>';
    html = html + '         <th>CM <br />Programado (S/.)</th>';
    html = html + '         <th>Costo <br />Incremental (S/.)</th>';
    html = html + '         <th>CM App - CM Prog</th>';
    html = html + '         <th>CM App - CI</th>';
    html = html + '     </tr>';
    html = html + ' </thead>';
    html = html + ' <tbody>';
    var i = 0;

    for (var i in result) {
       
        var style = '';
        if (i % 2 == 0) style = 'background-color: #f2f5f7';
        var cprog = '';
        var diferenciaAB = '';
        if (result[i].CmProgramado != null) cprog = $.number(result[i].CmProgramado, 2, '.', '');
        if (result[i].DiferenciaAB != null) diferenciaAB = $.number(result[i].DiferenciaAB, 2, '.', '');


        html = html + '     <tr>';
        html = html + '         <td style="text-align:center;' + style + '">' + result[i].Hora + '</td>';
        html = html + '         <td style="text-align:right;' + style + '">' + $.number(result[i].CmAplicativo, 2, '.', '') + '</td>';
        html = html + '         <td style="text-align:right;' + style + '">' + cprog + '</td>';
        html = html + '         <td style="text-align:right;' + style + '">' + $.number(result[i].CostoIncremental, 2, '.', '') + '</td>';
        html = html + '         <td style="text-align:right;' + style + '">' + diferenciaAB + '</td>';
        html = html + '         <td style="text-align:right;' + style + '">' + $.number(result[i].DiferenciaAC, 2, '.', '') + '</td>';
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
    var dataCMAplicativo = [];
    var dataCMProgramado = [];
    var dataCostoIncremental = [];

    for (var i in result) {
        categorias.push(result[i].Hora);
        dataCMAplicativo.push(result[i].CmAplicativo);
        dataCMProgramado.push(result[i].CmProgramado);
        dataCostoIncremental.push(result[i].CostoIncremental);
    }

    series.push({ name: 'CM Aplicativo', data: dataCMAplicativo, color: '#7CB5EC' });
    series.push({ name: 'CM Programado', data: dataCMProgramado, color: '#90ED7D' });
    series.push({ name: 'Costo Incremental', data: dataCostoIncremental, color: 'red' });


    $('#contentGrafico').highcharts({
        title: {
            text: 'Comparativo CM vs Costos Incrementales - ' + barra,
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
                text: '(S/.)'
            },
            plotLines: [{
                value: 0,
                width: 1,
                color: '#808080'
            }]
        },
        tooltip: {
            valueSuffix: 'S/.'
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
