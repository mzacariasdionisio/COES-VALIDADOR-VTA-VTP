var controlador = siteRoot + 'seguimientorecomendacion/';

var ordenamiento = [
    { Campo: "EMPRNOMB", Orden: "asc" },
    { Campo: "AREANOMB", Orden: "asc" },
    { Campo: "EQUIABREV", Orden: "asc" },
    { Campo: "SRMRECTITULO", Orden: "asc" },
    { Campo: "USERNAME", Orden: "asc" },
    { Campo: "SRMRECFECHAVENCIM", Orden: "asc" },
    { Campo: "SRMRECDIANOTIFPLAZO", Orden: "asc" },
    { Campo: "SRMCRTDESRIP", Orden: "asc" },
    { Campo: "SRMSTDDESRIP", Orden: "asc" }
];

$(function () {

    $('#cbFamilia').val($('#hfFamilia').val());
    $('#cbTipoEmpresa').val($('#hfTipoEmpresa').val());
    $('#cbEmpresa').val($('#hfEmpresa').val());

    $('#cbEstado').val($('#hfEstado').val());
    $('#cbCriticidad').val($('#hfCriticidad').val());

    
    $('#FechaDesde').Zebra_DatePicker({
        onSelect: function () {
            buscarEvento();
        }
    });

    $('#FechaHasta').Zebra_DatePicker({
        onSelect: function () {
            buscarEvento();
        }
    });

    $('#cbEstado').on("change", function () {
        buscarEvento();
    });

    $('#cbCriticidad').on("change", function () {
        buscarEvento();
    });


    $('#cbFamilia').on("change", function () {
        buscarEvento();
    });
    $('#cbTipoEmpresa').change(function () {
        cargarEmpresas();
        buscarEvento();
    });
    $('#cbEmpresa').on("change", function () {
        buscarEvento();
    });

    $('#cbResponsable').on("change", function () {
        buscarEvento();
    });


    buscarEvento();
    
    $('#btnBuscar').click(function () {
        buscarEvento();
    });



    $('#btnExportar').click(function () {
        exportar(0);
    });


    $(document).ready(function () {

       
    });

    $('#tab-container').easytabs({
        animate: false
    });
});

buscarEvento = function()
{
    pintarPaginado();
    $('#hfCampo').val('SRMRECFECHAVENCIM');
    $('#hfOrden').val('desc');
    mostrarListado(1);

}

closeImportar = function ()
{
    $('#divLeyenda').css('display', 'none');
}

pintarPaginado = function()
{  
    $.ajax({
        type: 'POST',
        url: controlador + "reporte/paginado",
        data: $('#frmBusqueda').serialize(),
        success: function (evt) {
            $('#paginado').html(evt);
            mostrarPaginado();
        },
        error: function () {
            mostrarError();
        }
    });
}

cargarEmpresas = function() {
    var controlador2 = siteRoot + 'eventos/';
    $.ajax({
        type: 'POST',
        url: controlador2 + 'evento/cargarempresas',
        dataType: 'json',
        data: { idTipoEmpresa: $('#cbTipoEmpresa').val() },
        cache: false,
        success: function (aData) {
            $('#cbEmpresa').get(0).options.length = 0;
            $('#cbEmpresa').get(0).options[0] = new Option("--SELECCIONE--", "");
            $.each(aData, function (i, item) {
                $('#cbEmpresa').get(0).options[$('#cbEmpresa').get(0).options.length] = new Option(item.Text, item.Value);
            });
        },
        error: function () {
            mostrarError();
        }
    });
}

function ordenar(elemento) {
    order = $.grep(ordenamiento, function (e) { return e.Campo == elemento; })[0].Orden;
    $('#hfCampo').val(elemento);
    $('#hfOrden').val(order);
    $.each(ordenamiento, function () {
        if (this.Campo == elemento) {
            this.Orden = (order == "asc") ? "desc" : "asc";
        }
    });
    mostrarListado(1);
}

mostrarListado = function(nroPagina)
{

    $('#hfNroPagina').val(nroPagina);
    $.ajax({
        type: 'POST',
        url: controlador + "reporte/lista",
        data:  $('#frmBusqueda').serialize(),
        success: function (evt) {

            $('#listado').html(evt);
            
            //grafica
            cargarEmpresaCriticidad();
        },
        error: function () {
            mostrarError();
        }
    });
}

exportar = function(indicador)
{
    $('#hfIndicador').val(indicador);

    $.ajax({
        type: 'POST',
        url: controlador + "reporte/exportarreporte",
        dataType: 'json',
        cache: false,
        data: $('#frmBusqueda').serialize(),
        success: function (resultado) {
            if (resultado == 1) {
                location.href = controlador + "reporte/descargarreporte";
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

pintarBusqueda = function(nroPagina)
{
    mostrarListado(nroPagina);
}

//carga grafico Empresa Criticidad
cargarEmpresaCriticidad = function () {

    $.ajax({
        type: 'POST',
        url: controlador + "reporte/ListadoEmpresaCriticidad",
        data:  $('#frmBusqueda').serialize() ,
        dataType: 'json',
        success: function (result) {
            graficoEmpresaCriticidad(result);
            cargarEmpresaEstado();
        },
        error: function () {

        }
    });
}

graficoEmpresaCriticidad = function (result) {
    var json = result;
    var jsoncriticidad = []; //criticidad
    var jsonempresa = []; //empresa
    var jsoncriticidadcolor = []; //color criticidad

    //lista de criticidad
    var listCrit = json.ListaCriticidad;
    for (var i in listCrit) {
        jsoncriticidad.push(listCrit[i].Srmcrtdescrip);
        jsoncriticidadcolor.push('#'+listCrit[i].Srmcrtcolor);
    }

    //lista de empresa
    var listEmp = json.listaRecomendacion;
    var jsonRec = json.listaRecomendacion;
    for (var i in listEmp) {

        if (jsonempresa.indexOf(listEmp[i].Emprnomb) < 0) {
            jsonempresa.push((listEmp[i].Emprnomb + "").trim());

        }
    }

    var maximo = 0;

    //lista de recomendaciones por criticidad
    var jsonmanto = [];
    for (var i in jsoncriticidad) {

        jsonmanto[i] = [];
        for (var j in jsonempresa) {
            jsonmanto[i].push(null);
        }
    }


    var k;
    var j;
    for (var i in jsonRec) {

        var txt = "Empresa: " + jsonRec[i].Emprnomb + ". Criticidad: " + jsonRec[i].Srmcrtdescrip + ". Reg: " + listEmp[i].Registros;
        
        k = jsonempresa.indexOf((jsonRec[i].Emprnomb + "").trim());
        j = jsoncriticidad.indexOf(jsonRec[i].Srmcrtdescrip);

        var reg =jsonRec[i].Registros;
        jsonmanto[j][k] = reg;
        
        
    }

    var opcion = {
        chart: {
            type: 'bar',
            options3d: {
                enabled: true,
                alpha: 10,
                beta: 25,
                depth: 70
            }
        },
        title: {
            text: 'Recomendación por Empresa y Criticidad'
        },
        xAxis: {
            categories: jsonempresa,
            style: {

                fontSize: '5'
            },
            
        },
        yAxis: {
            min: 0,
            endOnTick:false,
            allowDecimals: false,
            title: {
                text: ''
            },
            stackLabels: {
                enabled: true,
                style: {
                    fontWeight: 'bold',
                    color:  'gray'
                }
            }
        },
        legend: {
            reversed: true
        },
        tooltip: {
            formatter: function () {
                return '<b>' + this.x + '</b><br/>' +
                    this.series.name + ': ' + this.y + '<br/>' +
                    'Total: ' + this.point.stackTotal;
            }
        },
        plotOptions: {

            series: {
        stacking: 'normal',
        dataLabels: {
            enabled: true
            
            
        }
    }
        },
        colors:jsoncriticidadcolor,
        series: []
    };
   
    for (var i in jsonmanto) {
        opcion.series.push({
            name: jsoncriticidad[i],
            data: jsonmanto[i]
        });
    }
    

    $('#GrafEmpresa').highcharts(opcion);
}

//carga grafico Empresa Estado
cargarEmpresaEstado = function () {
    $.ajax({
        type: 'POST',
        url: controlador + "reporte/ListadoEmpresaEstado",
        data: $('#frmBusqueda').serialize(),
        dataType: 'json',
        success: function (result) {
            graficoEmpresaEstado(result);
            if ($('#hfConsultar').val() == 1) {
                cargarTipoEquipoCriticidad();
            }
        },
        error: function () {

        }
    });
}

graficoEmpresaEstado = function (result) {
    var json = result;
    var jsonestado = []; //estado
    var jsonempresa = []; //empresa
    var jsonestadocolor = []; //color estado

    //lista de estado
    var listEst = json.ListaEstado;
    for (var i in listEst) {
        jsonestado.push(listEst[i].Srmstddescrip);
        jsonestadocolor.push('#' + listEst[i].Srmstdcolor);
    }

    //lista de empresa
    var listEmp = json.listaRecomendacion;
    var jsonRec = json.listaRecomendacion;
    for (var i in listEmp) {

        if (jsonempresa.indexOf(listEmp[i].Emprnomb) < 0) {
            jsonempresa.push((listEmp[i].Emprnomb + "").trim());

        }
    }

    var maximo = 0;

    //lista de recomendaciones por estado
    var jsonmanto = [];
    for (var i in jsonestado) {

        jsonmanto[i] = [];
        for (var j in jsonempresa) {
            jsonmanto[i].push(null);
        }
    }


    var k;
    var j;
    for (var i in jsonRec) {

        var txt = "Empresa: " + jsonRec[i].Emprnomb + ". Criticidad: " + jsonRec[i].Srmcrtdescrip + ". Reg: " + listEmp[i].Registros;

        k = jsonempresa.indexOf((jsonRec[i].Emprnomb + "").trim());
        j = jsonestado.indexOf(jsonRec[i].Srmstddescrip);

        var reg = jsonRec[i].Registros;
        jsonmanto[j][k] = reg;


    }

    var opcion = {
        chart: {
            type: 'bar',
            options3d: {
                enabled: true,
                alpha: 10,
                beta: 25,
                depth: 70
            }
        },
        title: {
            text: 'Recomendación por Empresa y Estado'
        },
        xAxis: {
            categories: jsonempresa,
            style: {

                fontSize: '5'
            },

        },
        yAxis: {
            min: 0,
            endOnTick: false,
            allowDecimals: false,
            title: {
                text: ''
            },
            stackLabels: {
                enabled: true,
                style: {
                    fontWeight: 'bold',
                    color: 'gray'
                }
            }
        },
        legend: {
            reversed: true
        },
        tooltip: {
            formatter: function () {
                return '<b>' + this.x + '</b><br/>' +
                    this.series.name + ': ' + this.y + '<br/>' +
                    'Total: ' + this.point.stackTotal;
            }
        },
        plotOptions: {
            series: {
                stacking: 'normal',
                dataLabels: {
                    enabled: true                    
                }
            }
        },
        colors: jsonestadocolor,
        series: []
    };

    for (var i in jsonmanto) {
        opcion.series.push({
            name: jsonestado[i],
            data: jsonmanto[i]
        });
    }
        
    $('#GrafEmpresaEstado').highcharts(opcion);
}

//carga grafico Tipo de equipo Criticidad
cargarTipoEquipoCriticidad = function () {

    $.ajax({
        type: 'POST',
        url: controlador + "reporte/ListadoTipoEquipoCriticidad",
        data: $('#frmBusqueda').serialize(),
        dataType: 'json',
        success: function (result) {
            graficoTipoEquipoCriticidad(result);
            cargarTipoEquipoEstado();
        },
        error: function () {

        }
    });
}

graficoTipoEquipoCriticidad = function (result) {
    var json = result;
    var jsoncriticidad = []; //criticidad
    var jsontipoequipo = []; //tipo de equipo
    var jsoncriticidadcolor = []; //color criticidad

    //lista de criticidad
    var listCrit = json.ListaCriticidad;
    for (var i in listCrit) {
        jsoncriticidad.push(listCrit[i].Srmcrtdescrip);
        jsoncriticidadcolor.push('#' + listCrit[i].Srmcrtcolor);
    }

    //lista de tipo de equipo
    var listTequipo = json.listaRecomendacion;
    var jsonRec = json.listaRecomendacion;
    for (var i in listTequipo) {

        if (jsontipoequipo.indexOf(listTequipo[i].Famnomb) < 0) {
            jsontipoequipo.push((listTequipo[i].Famnomb + "").trim());

        }
    }

    var maximo = 0;

    //lista de recomendaciones por criticidad
    var jsonmanto = [];
    for (var i in jsoncriticidad) {

        jsonmanto[i] = [];
        for (var j in jsontipoequipo) {
            jsonmanto[i].push(null);
        }
    }


    var k;
    var j;
    for (var i in jsonRec) {

        var txt = "Empresa: " + jsonRec[i].Famnomb + ". Criticidad: " + jsonRec[i].Srmcrtdescrip + ". Reg: " + listTequipo[i].Registros;

        k = jsontipoequipo.indexOf((jsonRec[i].Famnomb + "").trim());
        j = jsoncriticidad.indexOf(jsonRec[i].Srmcrtdescrip);


        var reg = jsonRec[i].Registros;
        jsonmanto[j][k] = reg;


    }

    var opcion = {
        chart: {
            type: 'bar',
            options3d: {
                enabled: true,
                alpha: 10,
                beta: 25,
                depth: 70
            }
        },
        title: {
            text: 'Recomendación por Tipo de Equipo y Criticidad'
        },
        xAxis: {
            categories: jsontipoequipo,
            style: {

                fontSize: '5'
            },

        },
        yAxis: {
            min: 0,
            endOnTick: false,
            allowDecimals: false,
            title: {
                text: ''
            },
            stackLabels: {
                enabled: true,
                style: {
                    fontWeight: 'bold',
                    color: 'gray'
                }
            }
        },
        legend: {
            reversed: true
        },
        tooltip: {
            formatter: function () {
                return '<b>' + this.x + '</b><br/>' +
                    this.series.name + ': ' + this.y + '<br/>' +
                    'Total: ' + this.point.stackTotal;
            }
        },
        plotOptions: {
            series: {
                stacking: 'normal',
                dataLabels: {
                    enabled: true                   
                }
            }
        },
        colors: jsoncriticidadcolor,
        series: []
    };

    for (var i in jsonmanto) {
        opcion.series.push({
            name: jsoncriticidad[i],
            data: jsonmanto[i]
        });
    }



    $('#GrafTipoEquipo').highcharts(opcion);
}

//carga grafico Tipo de equipo Estado
cargarTipoEquipoEstado = function () {
    $.ajax({
        type: 'POST',
        url: controlador + "reporte/ListadoTipoEquipoEstado",
        data: $('#frmBusqueda').serialize(),
        dataType: 'json',
        success: function (result) {
            graficoTipoEquipoEstado(result);
            cargarEstado();
        },
        error: function () {

        }
    });
}

graficoTipoEquipoEstado = function (result) {
    var json = result;
    var jsonestado = []; //estado
    var jsontipoequipo = []; //empresa
    var jsonestadocolor = []; //color estado

    //lista de estado
    var listEst = json.ListaEstado;
    for (var i in listEst) {
        jsonestado.push(listEst[i].Srmstddescrip);
        jsonestadocolor.push('#' + listEst[i].Srmstdcolor);
    }

    //lista de tipo de equipo
    var listTequipo = json.listaRecomendacion;
    var jsonRec = json.listaRecomendacion;
    for (var i in listTequipo) {

        if (jsontipoequipo.indexOf(listTequipo[i].Famnomb) < 0) {
            jsontipoequipo.push((listTequipo[i].Famnomb + "").trim());

        }
    }

    var maximo = 0;

    //lista de recomendaciones por criticidad
    var jsonmanto = [];
    for (var i in jsonestado) {

        jsonmanto[i] = [];
        for (var j in jsontipoequipo) {
            jsonmanto[i].push(null);
        }
    }


    var k;
    var j;
    for (var i in jsonRec) {

        var txt = "Empresa: " + jsonRec[i].Famnomb + ". Criticidad: " + jsonRec[i].Srmcrtdescrip + ". Reg: " + listTequipo[i].Registros;

        k = jsontipoequipo.indexOf((jsonRec[i].Famnomb + "").trim());
        j = jsonestado.indexOf(jsonRec[i].Srmstddescrip);

        var reg = jsonRec[i].Registros;
        jsonmanto[j][k] = reg;

    }

    var opcion = {
        chart: {
            type: 'bar',
            options3d: {
                enabled: true,
                alpha: 10,
                beta: 25,
                depth: 70
            }
        },
        title: {
            text: 'Recomendación por Tipo de Equipo y Estado'
        },
        xAxis: {
            categories: jsontipoequipo,
            style: {

                fontSize: '5'
            },

        },
        yAxis: {
            min: 0,
            endOnTick: false,
            allowDecimals: false,
            title: {
                text: ''
            },
            stackLabels: {
                enabled: true,
                style: {
                    fontWeight: 'bold',
                    color:  'gray'
                }
            }
        },
        legend: {
            reversed: true
        },
        tooltip: {
            formatter: function () {
                return '<b>' + this.x + '</b><br/>' +
                    this.series.name + ': ' + this.y + '<br/>' +
                    'Total: ' + this.point.stackTotal;
            }
        },
        plotOptions: {
            series: {
                stacking: 'normal',
                dataLabels: {
                    enabled: true
                }
            }
        },
        colors: jsonestadocolor,
        series: []
    };

    for (var i in jsonmanto) {
        opcion.series.push({
            name: jsonestado[i],
            data: jsonmanto[i]
        });
    }


    $('#GrafTipoEquipoEstado').highcharts(opcion);
}

//carga grafico Estado
cargarEstado = function () {
    $.ajax({
        type: 'POST',
        url: controlador + "reporte/ListadoEstado",
        data: $('#frmBusqueda').serialize(),
        dataType: 'json',
        success: function (result) {
            graficoEstado(result);
            cargarEstadoCriticidad();
        },
        error: function () {
            
        }
    });
}

graficoEstado = function (result) {

    var json = result;
    var jsondata = [];    
    var color1 = [];

    for (var i in json) {
        jsondata.push([json[i].Srmstddescrip, json[i].Registros]);
        color1.push('#' +json[i].Srmstdcolor);
    }


    $('#GrafEstado').highcharts({
        chart: {
            type: 'pie',
            options3d: {
                enabled: true,
                alpha: 45,
                beta: 0
            }
        },
        title: {
            text: 'Recomendación por Estado'
        },
        tooltip: {
            pointFormat: '{series.name}: <b>{point.percentage:.1f}%</b><br>Registros:<b>{point.y}</b>',
            percentageDecimals: 1
        },
        plotOptions: {
            pie: {
                allowPointSelect: true,
                cursor: 'pointer',
                depth: 35,
                dataLabels: {
                    enabled: true,
                    format: '<b>{point.name}</b>:<br>{point.percentage:.1f} %<br>Registros:{point.y}'
                }
                
            }
        },
        series: [{
            type: 'pie',
            name: 'Porcentaje',
            colorByPoint: true,
            data: jsondata
        }],
        colors: color1
    });
     
    
}

cargarEstadoCriticidad = function () {
    $.ajax({
        type: 'POST',
        url: controlador + "reporte/ListadoEstadoCriticidad",
        data: $('#frmBusqueda').serialize(),
        dataType: 'json',
        success: function (result) {
            graficoEstadoCriticidad(result);
            cargarCriticidad();
        },
        error: function () {

        }
    });
}

graficoEstadoCriticidad = function (result) {
    var json = result;
    var jsoncriticidad = []; //criticidad
    var jsonestado = []; //estado
    var jsoncriticidadcolor = []; //color criticidad

    //lista de criticidad
    var listCrit = json.ListaCriticidad;
    for (var i in listCrit) {
        jsoncriticidad.push(listCrit[i].Srmcrtdescrip);
        jsoncriticidadcolor.push('#' + listCrit[i].Srmcrtcolor);
    }

    //lista de empresa
    var listEmp = json.listaRecomendacion;
    var jsonRec = json.listaRecomendacion;
    for (var i in listEmp) {

        if (jsonestado.indexOf(listEmp[i].Srmstddescrip) < 0) {
            jsonestado.push((listEmp[i].Srmstddescrip + "").trim());

        }
    }

    var maximo = 0;

    //lista de recomendaciones por criticidad
    var jsonmanto = [];
    for (var i in jsoncriticidad) {

        jsonmanto[i] = [];
        for (var j in jsonestado) {
            jsonmanto[i].push(null);
        }
    }


    var k;
    var j;
    for (var i in jsonRec) {

        var txt = "Estado: " + jsonRec[i].Srmstddescrip + ". Criticidad: " + jsonRec[i].Srmcrtdescrip + ". Reg: " + listEmp[i].Registros;

        k = jsonestado.indexOf((jsonRec[i].Srmstddescrip + "").trim());
        j = jsoncriticidad.indexOf(jsonRec[i].Srmcrtdescrip);



        var reg = jsonRec[i].Registros;
        jsonmanto[j][k] = reg;


    }

    var opcion = {
        chart: {
            type: 'column',
            options3d: {
                enabled: true,
                alpha: 10,
                beta: 25,
                depth: 70
            }
        },
        title: {
            text: 'Recomendación por Estado y Criticidad'
        },
        xAxis: {
            categories: jsonestado,
            style: {

                fontSize: '5'
            },

        },
        yAxis: {
            min: 0,
            endOnTick: false,
            allowDecimals: false,
            title: {
                text: ''
            },
            stackLabels: {
                enabled: true,
                style: {
                    fontWeight: 'bold',
                    color: (Highcharts.theme && Highcharts.theme.textColor) || 'gray'
                }
            }
        },
        legend: {
            reversed: true
        },
        tooltip: {
            formatter: function () {
                return '<b>' + this.x + '</b><br/>' +
                    this.series.name + ': ' + this.y + '<br/>'
            }
        },
        plotOptions: {
            series: {
                dataLabels: {
                    enabled: true,
                    color: (Highcharts.theme && Highcharts.theme.dataLabelsColor) || 'white',
                    style: {
                        textShadow: '0 0 3px black, 0 0 3px black'
                    }
                }
            }
        },
        colors: jsoncriticidadcolor,
        series: []
    };

    for (var i in jsonmanto) {
        opcion.series.push({
            name: jsoncriticidad[i],
            data: jsonmanto[i]
        });
    }


    $('#GrafEstadoCriticidad').highcharts(opcion);
}

//carga grafico Criticidad
cargarCriticidad = function () {
    $.ajax({
        type: 'POST',
        url: controlador + "reporte/ListadoCriticidad",
        data: $('#frmBusqueda').serialize(),
        dataType: 'json',
        success: function (result) {
            graficoCriticidad(result);
        },
        error: function () {

        }
    });
}

graficoCriticidad = function (result) {

    var json = result;
    var jsondata = [];
    var color1 = [];

    for (var i in json) {
        jsondata.push([json[i].Srmcrtdescrip, json[i].Registros]);
        color1.push('#' + json[i].Srmcrtcolor);
    }


    $('#GrafCriticidad').highcharts({
        chart: {
            type: 'pie',
            options3d: {
                enabled: true,
                alpha: 45,
                beta: 0
            }
        },
        title: {
            text: 'Recomendación por Criticidad'
        },
        tooltip: {
            pointFormat: '{series.name}: <b>{point.percentage:.1f}%</b><br>Registros:<b>{point.y}</b>',
            percentageDecimals: 1
        },
        plotOptions: {
            pie: {
                allowPointSelect: true,
                cursor: 'pointer',
                depth: 35,
                dataLabels: {
                    enabled: true,
                    format: '<b>{point.name}</b>:<br>{point.percentage:.1f} %<br>Registros:{point.y}'                    
                }

            }
        },
        series: [{
            type: 'pie',
            name: 'Porcentaje',
            colorByPoint: true,
            data: jsondata
        }],
        colors: color1
    });
    
}

mostrarError = function ()
{
    alert('Ha ocurrido un error.');
}



