var controlador = siteRoot + 'serviciorpf/comparativo/';

$(function () {

    $('#txtFecha').Zebra_DatePicker({
        direction: -1,
        onSelect: function () {
            cargarCentrales();
        },
    });

    $('#cbEmpresa').on('change', function () {
        cargarCentrales();
    });

    $('#btnConfiguracion').on('click', function () {
        document.location.href = controlador + 'equivalencia';
    });

    $('#btnConsultar').on('click', function () {
        consultar();
    });

    $('#btnExportar').on('click', function () {
        exportar();
    });

    $('#btnExportarRPF').on('click', function () {
        exportarRPF();
    });

    consultar();
});

cargarCentrales = function () {
    var fecha = $('#txtFecha').val();

    var empresa = $('#cbEmpresa').val();
    if (empresa == "") empresa = "-1";

    $("#cbCentral").empty();
    $('#cbCentral').append($('<option></option>').val("").html("TODOS"));
    $.ajax({
        type: 'POST',
        url: controlador + 'obtenercentrales',
        data: {
            fecha: fecha,
            idEmpresa: empresa
        },
        dataType: 'json',
        success: function (result) {
            for (var item in result) {
                $('#cbCentral').append($('<option></option>').val(result[item].Equicodi).html(result[item].Equinomb));
            }
        },
        error: function () {
            mostrarMensaje('mensaje', 'error', 'Ha ocurrido un error.');
        }
    });
}

consultar = function () {
    $.ajax({
        type: 'POST',
        url: controlador + 'comparativo',
        data: {
            idEmpresa: $('#cbEmpresa').val(),
            idCentral: $('#cbCentral').val(),
            fecha: $('#txtFecha').val()
        },
        dataType: 'json',
        success: function (result) {
            pintarTabla(result);
            pintarGrafico(result);
            $('#mensaje').removeClass();
            $('#mensaje').html('');
        },
        error: function () {
            mostrarMensaje('mensaje', 'error', 'Ha ocurrido un error.');
        }
    });
}

pintarTabla = function (result) {

    var html = '<table class="pretty tabla-adicional">';
    html = html + ' <thead>';
    html = html + '     <tr>';
    html = html + '         <th>HORA</th>';
    html = html + '         <th>RPF</th>';
    html = html + '         <th>DESPACHO</th>';
    html = html + '         <th>DESVIACIÓN</th>';
    html = html + '     </tr>';
    html = html + ' </thead>';
    html = html + ' <tbody>';
    var i = 0;
    var sumRPF = 0;
    var sumDespacho = 0;
    for (var i in result) {

        var style = '';
        if (i % 2 == 0) style = 'background-color: #f2f5f7';

        if (result[i].Desviacion * 100 > 5) {
            style = 'background-color: #ffb4b4';
        }

        html = html + '     <tr>';
        html = html + '         <td style="text-align:center;' + style + '">' + result[i].Hora + '</td>';
        html = html + '         <td style="text-align:right;' + style + '">' + $.number(result[i].ValorRPF, 2, '.', '') + '</td>';
        html = html + '         <td style="text-align:right;' + style + '">' + $.number(result[i].ValorDespacho, 2, '.', '') + '</td>';
        html = html + '         <td style="text-align:right;' + style + '">' + $.number(result[i].Desviacion * 100, 2, '.', '') + '% </td>';
        html = html + '     </tr>';

        sumRPF = sumRPF + result[i].ValorRPF;
        sumDespacho = sumDespacho + result[i].ValorDespacho;
        i++;
    }

    var desviacion = (sumRPF != 0) ? (sumDespacho - sumRPF) / sumRPF : 0;
    html = html + ' </tbody>';
    html = html + ' <thead>';
    html = html + '     <tr>';
    html = html + '         <th>TOTAL</th>';
    html = html + '         <th style="text-align:right;">' + $.number(sumRPF, 2, '.', '') + '</th>';
    html = html + '         <th style="text-align:right;">' + $.number(sumDespacho, 2, '.', '') + '</th>';
    html = html + '         <th style="text-align:right;">' + $.number(desviacion * 100, 2, '.', '') + '% </th>';
    html = html + '     </tr>';
    html = html + ' </thead>';
    html = html + '</table>';

    $('#contentTabla').html(html);
}

pintarGrafico = function (result) {

    var categorias = [];
    var series = [];
    var dataRPF = [];
    var dataDespacho = [];

    for (var i in result) {
        categorias.push(result[i].Hora);
        dataRPF.push(result[i].ValorRPF);
        dataDespacho.push(result[i].ValorDespacho);
    }

    series.push({ name: 'RPF', data: dataRPF, color: '#7CB5EC' });
    series.push({ name: 'Despacho Ejecutado', data: dataDespacho, color: '#90ED7D' });

    $('#contentGrafico').highcharts({
        title: {
            text: 'Comparativo RPF VS Despacho Ejecutado',
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
                text: 'Potencia (MW)'
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
}

exportar = function () {
    $.ajax({
        type: 'POST',
        url: controlador + 'exportar',
        data: {
            idEmpresa: $('#cbEmpresa').val(),
            idCentral: $('#cbCentral').val(),
            fecha: $('#txtFecha').val()
        },
        dataType: 'json',
        success: function (result) {
            if (result == 1) {
                document.location.href = controlador + 'descargar';
            }
            else {
                mostrarMensaje('mensaje', 'error', 'Ha ocurrido un error.');
            }
        },
        error: function () {
            mostrarMensaje('mensaje', 'error', 'Ha ocurrido un error.');
        }
    });
}

exportarRPF = function () {
    $.ajax({
        type: 'POST',
        url: controlador + 'exportarrpf',
        data: {
            fecha: $('#txtFecha').val()
        },
        dataType: 'json',
        success: function (result) {
            if (result == 1) {
                document.location.href = controlador + 'descargarrpf';
            }
            else {
                mostrarMensaje('mensaje', 'error', 'Ha ocurrido un error.');
            }
        },
        error: function () {
            mostrarMensaje('mensaje', 'error', 'Ha ocurrido un error.');
        }
    });
}

mostrarMensaje = function (id, tipo, mensaje) {
    $('#' + id).removeClass();
    $('#' + id).addClass('action-' + tipo);
    $('#' + id).html(mensaje);
}