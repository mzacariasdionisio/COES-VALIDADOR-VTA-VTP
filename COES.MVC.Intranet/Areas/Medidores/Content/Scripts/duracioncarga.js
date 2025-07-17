var controlador = siteRoot + 'Medidores/duracioncarga/'

$(function () {

    $('#tab-container').easytabs({
        animate: false
    });

    $('#txtFechaDesde').Zebra_DatePicker({
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
        direction: true
    });

    $('#cbTipoGeneracion').multipleSelect({
        width: '100%',
        filter: true
    });

    $('#btnConsultar').click(function () {
        consultar();
    });

    $('#btnExportar').click(function () {
        exportar();
    });

    $('#cbTipoEmpresa').multipleSelect({
        width: '100%',
        filter: true,
        onClick: function (view) {
            cargarEmpresas();
        },
        onClose: function (view) {
            cargarEmpresas();
        }
    });

    $('#cbMeses').change(function () {
        cargarDatosMes();
        cargarPaginas();
        pintarPaginado(1);
    });

    cargarPrevio();
    cargarEmpresas();
    consultar();
});

cargarPrevio = function () {
    $('#cbTipoGeneracion').multipleSelect('checkAll');
    $('#cbTipoEmpresa').multipleSelect('checkAll');
}

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
        error: function () {
            mostrarError();
        }
    });
}

consultar = function () {
    var empresa = $('#cbEmpresas').multipleSelect('getSelects');
    var tipoGeneracion = $('#cbTipoGeneracion').multipleSelect('getSelects');
    var tipoEmpresa = $('#cbTipoEmpresa').multipleSelect('getSelects');
    if (empresa == "[object Object]") empresa = "";
    $('#hfEmpresa').val(empresa);
    $('#hfTipoGeneracion').val(tipoGeneracion);
    $('#hfTipoEmpresa').val(tipoEmpresa);

    if ($('#txtFechaDesde').val() != "" && $('#txtFechaHasta').val() != "") {
        $.ajax({
            type: 'POST',
            dataType: 'json',
            url: controlador + "consulta",
            data: {
                fechaDesde: $('#txtFechaDesde').val(),
                fechaHasta: $('#txtFechaHasta').val(),
                tiposEmpresa: $('#hfTipoEmpresa').val(),
                empresas: $('#hfEmpresa').val(), tiposGeneracion: $('#hfTipoGeneracion').val(),
                central: $('#cbCentral').val()
            },
            success: function (result) {               
                if (result != "-1" && result!= "-2" && result != "-3") {                   
                    cargarMeses(result);
                    cargarDatosMes();
                    cargarPaginas();
                    pintarPaginado(1);
                }
                else if(result == "-1") {
                    mostrarError();
                }
                else if (result == "-2") {
                    mostrarAlerta("No puede seleccionar más de tres meses");
                }
                else if (result == "-3")
                {
                    $('#tabla').html('');
                    $('#potenciaMaxima').html('');
                    $('#potenciaMinima').html('');
                    $('#energiaAcumulada').html('');
                    $('#factorCarga').html('');
                    $('#tituloGrafico').html('');
                    $('#grafico').html('');
                    $('#paginado').html('');
                }
            },
            error: function () {
                mostrarError();
            }
        });
    }
    else {
        alert("Por favor seleccione mes.");
    }
}


cargarPaginas = function ()
{
    $.ajax({
        type: 'POST',
        dataType: 'json',
        cache: false,
        url: controlador + "obtenerpaginas",       
        success: function (result) {
            $('#paginado').html(result);
        },
        error: function () {
            mostrarError();
        }
    });
}


cargarMeses = function (result)
{
    $('#cbMeses').get(0).options.length = 0;    
    $.each(result, function (i, item) {
        $('#cbMeses').get(0).options[$('#cbMeses').get(0).options.length] = new Option(item.Nombre, item.Valor);
    });
  
}

cargarDatosMes = function ()
{
    var mes = $('#cbMeses').val();
    $.ajax({
        type: 'POST',
        dataType: 'json',
        cache: false,
        url: controlador + "obtenerdatospormes",
        data: {
            indicador: mes,
            fechaInicio: $('#txtFechaDesde').val(),
            fechaFin: $('#txtFechaHasta').val(),
            texto: $('#cbMeses option:selected').text()
        },
        success: function (result) {
            if (result != "-1") {
                plotearDiagramaCarga(result.ListaGrafico);
                $('#potenciaMaxima').text($.number(result.Maximo, 3, ',', ' '));
                $('#potenciaMinima').text($.number(result.Minimo, 3, ',', ' '));
                $('#energiaAcumulada').text($.number(result.Gwh, 3, ',', ' '));
                $('#factorCarga').text($.number(result.Fc, 3, ',', ' '));
                $('#tituloGrafico').text(result.Titulo);
            }
            else {
                mostrarError();
            }
        },
        error: function () {
            mostrarError();
        }
    });
}

plotearDiagramaCarga = function (result) {
    var json = result;   
    var jsondata = [];
    var indice =8;
    for (var i in json) {
        var jsonValor = [];
        var jsonLista = json[i].ListaValores;
        for (var j in jsonLista) {
            jsonValor.push(jsonLista[j]);
        }
        jsondata.push({
            name: json[i].SerieName,
            data: jsonValor,
            color: json[i].SerieColor,
            index: indice
        });
        indice--;
    }

    var opciones = {
        chart: {
            type: 'area'
        },
        title: {
            text: ''
        },
        xAxis: {
            allowDecimals: false,
            title: {
                text: 'Tiempo'
            },
            labels: {
                formatter: function () {
                    return this.value;
                }
            }
        },
        yAxis: {
            title: {
                text: 'Potencia (MW)'
            },
            labels: {
                formatter: function () {
                    return this.value;
                }
            }
        },
        tooltip: {
            pointFormat: '{series.name} Potencia <b>{point.y:,.0f}</b><br/> en la hora {point.x}'
        },
        plotOptions: {
            area: {
                stacking: 'normal',
                pointStart: 1,
                marker: {
                    enabled: false,
                    symbol: 'circle',
                    radius: 2,
                    states: {
                        hover: {
                            enabled: true
                        }
                    }
                }
            }
        },
        series: jsondata
    };

    $('#grafico').highcharts(opciones);
}

pintarPaginado = function (index)
{
    $.ajax({
        type: 'POST',
        dataType: 'json',
        cache: false,
        url: controlador + "paginado",
        data: {
            index: index
        },
        success: function (result) {
            $('.page-item').removeClass("page-active");
            $('#page-item' + index).addClass("page-active");
            
            $('#tabla').html(result);
        },
        error: function () {
            mostrarError();
        }
    });
}

exportar = function () {
    var empresa = $('#cbEmpresas').multipleSelect('getSelects');
    var tipoGeneracion = $('#cbTipoGeneracion').multipleSelect('getSelects');
    var tipoEmpresa = $('#cbTipoEmpresa').multipleSelect('getSelects');
    if (empresa == "[object Object]") empresa = "";
    $('#hfEmpresa').val(empresa);
    $('#hfTipoGeneracion').val(tipoGeneracion);
    $('#hfTipoEmpresa').val(tipoEmpresa);

    if ($('#txtFechaDesde').val() != "" && $('#txtFechaHasta').val() != "") {
        $.ajax({
            type: 'POST',
            url: controlador + "exportar",
            dataType: 'json',
            data: {
                fechaDesde: $('#txtFechaDesde').val(),
                fechaHasta: $('#txtFechaHasta').val(),
                tiposEmpresa: $('#hfTipoEmpresa').val(),
                empresas: $('#hfEmpresa').val(), tiposGeneracion: $('#hfTipoGeneracion').val(),
                central: $('#cbCentral').val()
            },
            success: function (result) {
                if (result == "1") {
                    window.location = controlador + 'descargar'
                }
                else if (result == "-2") {
                    mostrarAlerta("No puede seleccionar más de un año.");
                }
                else if (result == "-3") {
                    mostrarAlerta("No existen datos.");
                }
                else {
                    mostrarError();
                }
            },
            error: function () {
                mostrarError();
            }
        });
    }
    else {
        alert("Por favor seleccione mes.");
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