var controlador = siteRoot + 'medidores/validacionregistro/'

$(function () {

    $('#txtFechaInicial').Zebra_DatePicker({
        format: 'm Y'
    });

   
    $('#cbEmpresa').on('change', function () {
        cargarGrupos();
    });

    $('#btnConsultar').on('click', function () {

        if ($('#cbGraficoCentral').prop('checked') == true) {
            if ($('#cbEmpresa').val() != "") {
               
                graficarMasivo();
                $('#mensaje').removeClass();
                $('#mensaje').html('');
            }
            else {
                mostrarMensaje('mensaje', 'alert', 'Seleccione una empresa.');
            }
        }
        else {
            graficar();
        }
    });

    cargarGrupos();
   
});

cargarGrupos = function () {
    $.ajax({
        type: 'POST',
        url: controlador + 'obtenercentrales',
        data: {
            empresa: $('#cbEmpresa').val()
        },
        dataType: 'json',
        global: false,
        success: function (result) {
            if (result != -1) {

                $('#cbGrupo').get(0).options.length = 0;
                $('#cbGrupo').get(0).options[0] = new Option("--TODOS--", "");
                $.each(result, function (i, item) {
                    $('#cbGrupo').get(0).options[$('#cbGrupo').get(0).options.length] = new Option(item, item);
                });
            }
            else {
                mostrarMensaje('mensaje', 'error', 'Se ha producido un error.');
            }
        },
        error: function () {
            mostrarMensaje('mensaje', 'error', 'Se ha producido un error.');
        }
    });
};

cargarPrevio = function () {

    $('#cbTipoEmpresa').multipleSelect('checkAll');

};


cargarEmpresas = function () {
    var tipoEmpresa = $('#cbTipoEmpresa').multipleSelect('getSelects');
    $('#hfTipoEmpresa').val(tipoEmpresa);

    $.ajax({
        type: 'POST',
        url: controlador + "empresas",
        data: {
            tiposEmpresa: $('#hfTipoEmpresa').val()
        },
        success: function (evt) {
            $('#empresas').html(evt);
        },
        error: function (err) {
            mostrarError();
        }
    });
};


graficar = function () {
    $.ajax({
        type: 'POST',
        url: controlador + 'ObtenerDatosGrafico',
        data: {
            empresa: $('#cbEmpresa').val(),
            central: $('#cbGrupo').val(),
            mes: $('#txtFechaInicial').val()
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
};

graficarMasivo = function () {
    $.ajax({
        type: 'POST',
        url: controlador + 'ObtenerDatosGraficoMasivo',
        data: {
            empresa: $('#cbEmpresa').val(),            
            mes: $('#txtFechaInicial').val()
        },
        dataType: 'json',
        success: function (result) {           
            pintarGraficoMasivo(result);
            $('#mensaje').removeClass();
            $('#mensaje').html('');
        },
        error: function () {
            mostrarMensaje('mensaje', 'error', 'Ha ocurrido un error.');
        }
    });
};

pintarTabla = function (result) {

    var html = '<table class="pretty tabla-adicional">';
    html = html + ' <thead>';
    html = html + '     <tr>';
    html = html + '         <th>HORA</th>';
    html = html + '         <th>MEDIDOR</th>';
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

        var style2 = '';
        if (result[i].ValorMedidor > 0 && result[i].ValorMedidor < 1) {
            style2 = 'background-color: skyblue';
        } else {
            style2 = style;
        }

        var style3 = '';
        if (result[i].ValorDespacho > 0 && result[i].ValorDespacho < 1) {
            style3 = 'background-color: skyblue';
        } else {
            style3 = style;
        }

        html = html + '     <tr>';
        html = html + '         <td style="text-align:center;' + style + '">' + result[i].Hora + '</td>';
        html = html + '         <td style="text-align:right;' + style2 + '">' + $.number(result[i].ValorMedidor, 10, '.', '') + '</td>';
        html = html + '         <td style="text-align:right;' + style3 + '">' + $.number(result[i].ValorDespacho, 10, '.', '') + '</td>';
        html = html + '         <td style="text-align:right;' + style + '">' + $.number(result[i].Desviacion * 100, 2, '.', '') + '% </td>';
        html = html + '     </tr>';

        sumRPF = sumRPF + result[i].ValorMedidor;
        sumDespacho = sumDespacho + result[i].ValorDespacho;
        i++;
    }

    var desviacion = (sumRPF != 0) ? (sumDespacho - sumRPF) / sumRPF : 0;
    html = html + ' </tbody>';
    html = html + ' <thead>';
    html = html + '     <tr>';
    html = html + '         <th>TOTAL</th>';
    html = html + '         <th style="text-align:right;">' + $.number(sumRPF, 10, '.', '') + '</th>';
    html = html + '         <th style="text-align:right;">' + $.number(sumDespacho, 10, '.', '') + '</th>';
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
        dataRPF.push(result[i].ValorMedidor);
        dataDespacho.push(result[i].ValorDespacho);
    }

    series.push({ name: 'Medidores', data: dataRPF, color: '#7CB5EC' });
    series.push({ name: 'Despacho Ejecutado', data: dataDespacho, color: '#90ED7D' });

    $('#contentGrafico').highcharts({
        title: {
            text: 'Comparativo Medidores VS Despacho Ejecutado',
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

pintarGraficoMasivo = function (result) {
    $('#contentTabla').html('');
    $('#contentGrafico').html('');
    
    for (var i in result) {
       
        var categorias = [];
        var series = [];
        var dataRPF = [];
        var dataDespacho = [];
        var central = result[i].Central;
        var datos = result[i].ListaGrafico;       

        for (var j in datos) {
            categorias.push(datos[j].Hora);
            dataRPF.push(datos[j].ValorMedidor);
            dataDespacho.push(datos[j].ValorDespacho);
        }
        
        series.push({ name: 'Medidores', data: dataRPF, color: '#7CB5EC' });
        series.push({ name: 'Despacho Ejecutado', data: dataDespacho, color: '#90ED7D' });

        var contenedor = $("<div class='contenedor-main'></div>");
        var chartContent = $("<div class='contenedor-grafico'></div>");
        contenedor.append(chartContent);

        $('#contentGrafico').append(contenedor);


        $(chartContent).highcharts({
            title: {
                text: 'Comparativo Medidores VS Despacho Ejecutado - ' + central,
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
};

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


mostrarMensaje = function (id, tipo, mensaje) {
    $('#' + id).removeClass();
    $('#' + id).addClass('action-' + tipo);
    $('#' + id).html(mensaje);
};

mostrarReporte = function () {
};

exportar = function () {

    var empresa = $('#cbEmpresas').multipleSelect('getSelects');
    var tipoGeneracion = $('#cbTipoGeneracion').multipleSelect('getSelects');
    var fuenteEnergia = $('#cbRecursoEnergetico').multipleSelect('getSelects');
    var tipoEmpresa = $('#cbTipoEmpresa').multipleSelect('getSelects');
    if (empresa == "[object Object]") empresa = "";
    $('#hfEmpresa').val(empresa);
    $('#hfTipoGeneracion').val(tipoGeneracion);
    $('#hfFuenteEnergia').val(fuenteEnergia);
    $('#hfTipoEmpresa').val(tipoEmpresa);

    if ($('#txtFechaInicial').val() != "" && $('#txtFechaFinal').val() != "") {
        $.ajax({
            type: 'POST',
            url: controlador + "exportar",
            dataType: 'json',
            data: {
                fechaInicial: $('#txtFechaInicial').val(), fechaFinal: $('#txtFechaFinal').val(),
                tiposEmpresa: $('#hfTipoEmpresa').val(),
                empresas: $('#hfEmpresa').val(), tiposGeneracion: $('#hfTipoGeneracion').val(),
                fuentesEnergia: $('#hfFuenteEnergia').val(), central: $('#cbCentral').val(),
                id: $('#cbFiltro').val()
            },
            success: function (result) {
                if (result == 1) {
                    window.location = controlador + 'descargar';
                }
                if (result == -1) {
                    mostrarError();
                }
            },
            error: function (err) {
                mostrarError()
            }
        });
    }
    else {
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

function generarTablaHtml(evt) {

    $('#reporte').css("width", $('#mainLayout').width() - 30 + "px");
    $('#reporte').html(evt);
    $('#tabla').dataTable({
        "scrollY": $('#reporte').height() > 400 ? 400 + "px" : "100%",
        "scrollX": false,
        "sDom": 't',
        "ordering": false,
        "bPaginate": false,
        "iDisplayLength": -1
    });
}
