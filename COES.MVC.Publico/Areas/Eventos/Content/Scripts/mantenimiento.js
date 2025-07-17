var controlador = siteRoot + 'eventos/'

$(function () {

    $('#tab-grafico').css("display", "none");

    $('#FechaDesde').change({
    });


    $('#FechaHasta').change({
    });

    $('#cbEmpresa').multipleSelect({
        filter: true
    });

    $('#cbEmpresa').multipleSelect('checkAll');


    $('#btnBuscar').click(function () {

        var fini = GetDateNormalFormat($('#FechaDesde').val()).split('/');
        var ffin = GetDateNormalFormat($('#FechaHasta').val()).split('/');
        var fechaInicial = new Date(fini[2], fini[1], fini[0]);
        var fechaFinal = new Date(ffin[2], ffin[1], ffin[0]);
        var ndias = ((((fechaFinal - fechaInicial) / 1000) / 60) / 60) / 24;

        if (ndias < 91)
            buscarEvento();
        else
            alert("El rango fecha debe ser de tres meses como máximo ");
    });

    $('#btnGrafico').click(function () {
        var fini = GetDateNormalFormat($('#FechaDesde').val()).split('/');
        var ffin = GetDateNormalFormat($('#FechaHasta').val()).split('/');
        var fechaInicial = new Date(fini[2], fini[1], fini[0]);
        var fechaFinal = new Date(ffin[2], ffin[1], ffin[0]);
        var ndias = ((((fechaFinal - fechaInicial) / 1000) / 60) / 60) / 24;

        if (ndias < 91)
            generarGrafico();
        else
            alert("El rango fecha debe ser de tres meses como máximo ");
    });

    $('#btnExpotar').click(function () {
        var fini = GetDateNormalFormat($('#FechaDesde').val()).split('/');
        var ffin = GetDateNormalFormat($('#FechaHasta').val()).split('/');
        var fechaInicial = new Date(fini[2], fini[1], fini[0]);
        var fechaFinal = new Date(ffin[2], ffin[1], ffin[0]);
        var ndias = ((((fechaFinal - fechaInicial) / 1000) / 60) / 60) / 24;

        if (ndias < 91)
            exportar();
        else
            alert("El rango fecha debe ser de tres meses como máximo ");
    });

    $('#excelManttoEmpresa').click(function () {
        exportar_ManttoEmpresa();
    });


    $('#cbTipoEmpresa').multipleSelect({
        filter: true,
        onClick: function (view) {
            cargarEmpresas(1);
        },
        onClose: function (view) {
            cargarEmpresas(1);
        }
    });

    $('#cbTipoMantenimiento').multipleSelect({
        width: '100%',
        filter: true
    });

    $('#cbTipoMantto').multipleSelect({
        width: '100%',
        filter: true
    });

    $('#cbFamilia').multipleSelect({
        width: '100%',
        filter: true
    });

    cargarPrevio();
    cargarEmpresas(0);
   
    $(window).resize(function () {
        $('#listado').css("width", "100%");
    });


});

function buscarEvento() {

    if ($('#cbEmpresa').multipleSelect('rowCountSelected') <= 100 || $('#cbEmpresa').multipleSelect('isAllSelected') == "S") {
        pintarPaginado();
        mostrarListado(1);
    }
    else {
        alert("No puede seleccionar más de 100 empresas.");
    }
}

function pintarPaginado() {
    var empresa = $('#cbEmpresa').multipleSelect('getSelects');
    var tipoMantenimiento = $('#cbTipoMantenimiento').multipleSelect('getSelects');
    var tipoEmpresa = $('#cbTipoEmpresa').multipleSelect('getSelects');
    var tipoEquipo = $('#cbFamilia').multipleSelect('getSelects');
    var tipoMantto = $('#cbTipoMantto').multipleSelect('getSelects');

    if (empresa == "[object Object]") empresa = "-1";
    

    //$('#hfEmpresa').val(empresa);
    $('#hfEmpresa').val($('#cbEmpresa').multipleSelect('isAllSelected') == "S" ? "-1" : empresa);
    $('#hfTipoMantenimiento').val(tipoMantenimiento);
    $('#hfTipoEmpresa').val(tipoEmpresa);
    $('#hfTipoEquipo').val(tipoEquipo);
    $('#hfTipoMantto').val(tipoMantto)

    $.ajax({
        type: 'POST',
        url: controlador + "mantenimiento/paginado",
        data: {
            tiposMantenimiento: $('#hfTipoMantenimiento').val(),
            fechaInicial: GetDateNormalFormat($('#FechaDesde').val()),
            fechaFinal: GetDateNormalFormat($('#FechaHasta').val()),
            indispo: $('#cbIndispo').val(),
            tiposEmpresa: $('#hfTipoEmpresa').val(), empresas: $('#hfEmpresa').val(),
            tiposEquipo: $('#hfTipoEquipo').val(), interrupcion: $('#cbInterrupcion').val(),
            tiposMantto: $('#hfTipoMantto').val(),
            nroPagina: $('#hfPaginaActual').val()
        },
        success: function (evt) {
            $('#paginado').html(evt);
            mostrarPaginado();
        },
        error: function () {
            alert("Ha ocurrido un error");
        }
    });
}

function cargarEmpresas(opcion) {
    var tipoEmpresa = $('#cbTipoEmpresa').multipleSelect('getSelects');
    $('#hfTipoEmpresa').val(tipoEmpresa);
    $.ajax({
        type: 'POST',
        url: controlador + 'mantenimiento/Empresas',

        data: { tiposEmpresa: $('#hfTipoEmpresa').val() },

        success: function (aData) {
            $('#empresas').html(aData);

            if (opcion == 0)
            {
                buscarEvento();
            }
        },
        error: function () {
            alert("Ha ocurrido un error ");
        }
    });
}

cargarPrevio = function () {
    $('#cbTipoMantenimiento').multipleSelect('checkAll');
    $('#cbTipoEmpresa').multipleSelect('checkAll');
    $('#cbFamilia').multipleSelect('checkAll');
    $('#cbTipoMantto').multipleSelect('checkAll');

}
// carga grafico 01
cargarManttoEmpresa = function (idmantto) {

    $.ajax({
        type: 'POST',
        url: controlador + "mantenimiento/GraficoManntoEmpresa",
        data: { idmanto: idmantto },
        dataType: 'json',
        success: function (result) {
            graficoManttoEmpresa(result);
        },
        error: function () {
            mostrarError();
        }
    });
}
// carga grafico 02
cargarManttoEquipo = function (idmantto) {

    $.ajax({
        type: 'POST',
        url: controlador + "mantenimiento/GraficoManntoEquipo",
        data: { idmanto: idmantto },
        dataType: 'json',
        success: function (result) {
            graficoManttoEquipo(result);
        },
        error: function () {
            mostrarError();
        }
    });
}
// carga grafico 03
cargarManttoEmpresaEquipo1 = function (idmantto) {
    $.ajax({
        type: 'POST',
        url: controlador + "mantenimiento/GraficoManntoEmpresaEquipo1",
        data: { idmanto: idmantto },
        dataType: 'json',
        success: function (result) {
            graficoManttoEmpresaEquipo1(result);
        },
        error: function () {
            mostrarError();
        }
    });
}
// carga grafico 04
cargarManttoEmpresaEquipo2 = function (idmantto) {
    $.ajax({
        type: 'POST',
        url: controlador + "mantenimiento/GraficoManntoEmpresaEquipo2",
        data: { idmanto: idmantto },
        dataType: 'json',
        success: function (result) {
            graficoManttoEmpresaEquipo2(result);
        },
        error: function () {
            mostrarError();
        }
    });
}
// carga grafico 05
cargarManttoEquipo2 = function (idmantto) {
    $.ajax({
        type: 'POST',
        url: controlador + "mantenimiento/GraficoManntoEquipo2",
        data: { idmanto: idmantto },
        dataType: 'json',
        success: function (result) {
            graficoManttoEquipo2(result);
        },
        error: function () {
            mostrarError();
        }
    });
}
// carga grafico 06
cargarTipoManttoTipoEmpresa = function (idmantto) {
    $.ajax({
        type: 'POST',
        url: controlador + "mantenimiento/GraficoTipoMantoTipoEmpresa",
        data: { idmanto: idmantto },
        dataType: 'json',
        success: function (result) {
            graficoTipoManttoTipoEmpresa(result);
        },
        error: function () {
            mostrarError();
        }
    });
}

obtenerTipoMantto = function (chart) {
    var txt = "";
    var manttos = [];
    var manttosNombre = [];
    txt = txt + '<table class="tabla-formulario">';
    txt = txt + '<thead>';
    txt = txt + '<tr>';
    txt = txt + '<th colspan="2" >Mantemiento</th>';
    txt = txt + '</tr>';
    txt = txt + '</thead>';
    txt = txt + '<tbody>';
    for (var i = 0; i < chart.length; i++) {

        txt = txt + '<tr style="cursor:pointer" class="itemMantto"><td><div class="ideven" id="' + chart[i].Evenclasecodi + '"></div>' + chart[i].Evenclasecodi + '</td><td>' + chart[i].Evenclasedesc + '</td></tr>';
        manttos.push(chart[i].Evenclasecodi);
        manttosNombre.push(chart[i].Evenclasedesc);
        if (i == 0) {
            $('#tituloManttoEmpresa').html("PARTICIPACIÓN DE NÚMERO DE MANTENIMIENTOS " + manttosNombre[0] + " POR EMPRESAS");
            $('#tituloManttoEquipo').html("PARTICIPACIÓN DE NÚMERO DE MANTENIMIENTOS " + manttosNombre[0] + " POR TIPO DE EQUIPO");
            $('#tituloManttoEmpresaEquipo1').html("PARTICIPACIÓN DE NÚMERO DE MANTENIMIENTOS " + manttosNombre[0] + " POR EMPRESAS Y TIPO DE MANTENIMIENTO");
            $('#tituloManttoEmpresaEquipo2').html("EVOLUCIÓN DEL NÚMERO DE MANTENIMIENTOS " + manttosNombre[0] + " POR EMPRESA Y TIPO DE EQUIPO");
            $('#tituloManttoEquipo2').html("EVOLUCIÓN DEL NÚMERO DE MANTENIMIENTOS " + manttosNombre[0] + " POR TIPO DE MANTENIMIENTO Y EQUIPO");
            $('#tituloTipoManttoTipoEmpresa').html("EVOLUCIÓN DEL NÚMERO DE MANTENIMIENTOS " + manttosNombre[0] + " POR TIPO DE MANTENIMIENTO Y EMPRESAS");
        }
    }

    txt = txt + '</tbody>';
    txt = txt + '</table>';


    //grafico 01
    $('#legendManttoEmpresa').html(txt);
    $('#excelManttoEmpresa').html("<img onclick='exportar_ManttoEmpresa();' width='32px' style='cursor: pointer; display: inline;' src='" + siteRoot + "Content/Images/ExportExcel.png' />");
    $('#legendManttoEmpresa .itemMantto').click(function () {
        var inx = $(this).index(),
           point = manttos[inx];
        cargarManttoEmpresa(point);
        $('#tituloManttoEmpresa').html("PARTICIPACIÓN DE NÚMERO DE MANTENIMIENTOS " + manttosNombre[inx] + " POR EMPRESAS");
    });
    //grafico 02
    $('#legendManttoEquipo').html(txt);
    $('#excelManttoEquipo').html("<img onclick='exportar_ManttoEquipo();' width='32px' style='cursor: pointer; display: inline;' src='" + siteRoot + "Content/Images/ExportExcel.png' />");
    $('#legendManttoEquipo .itemMantto').click(function () {
        var inx = $(this).index(),
           point = manttos[inx];
        cargarManttoEquipo(point);
        $('#tituloManttoEquipo').html("PARTICIPACIÓN DE NÚMERO DE MANTENIMIENTOS " + manttosNombre[inx] + " POR TIPO DE EQUIPO");
    });
    // grafico 03
    $('#legendManttoEmpresaEquipo1').html(txt);
    $('#excelManttoEmpresaEquipo1').html("<img onclick='exportar_ManttoEmpresaEquipo1();' width='32px' style='cursor: pointer; display: inline;' src='" + siteRoot + "Content/Images/ExportExcel.png' />");
    $('#legendManttoEmpresaEquipo1 .itemMantto').click(function () {
        var inx = $(this).index(),
           point = manttos[inx];
        cargarManttoEmpresaEquipo1(point);
        $('#tituloManttoEmpresaEquipo1').html("PARTICIPACIÓN DE NÚMERO DE MANTENIMIENTOS " + manttosNombre[inx] + " POR EMPRESAS Y TIPO DE MANTENIMIENTO");
    });
    // grafico 04
    $('#legendManttoEmpresaEquipo2').html(txt);
    $('#excelManttoEmpresaEquipo2').html("<img onclick='exportar_ManttoEmpresaEquipo2();' width='32px' style='cursor: pointer; display: inline;' src='" + siteRoot + "Content/Images/ExportExcel.png' />");
    $('#legendManttoEmpresaEquipo2 .itemMantto').click(function () {
        var inx = $(this).index(),
           point = manttos[inx];
        cargarManttoEmpresaEquipo2(point);
        $('#tituloManttoEmpresaEquipo2').html("EVOLUCIÓN DEL NÚMERO DE MANTENIMIENTOS " + manttosNombre[inx] + " POR EMPRESA Y TIPO DE EQUIPO");
    });
    // grafico 05
    $('#legendManttoEquipo2').html(txt);
    $('#excelManttoEquipo2').html("<img onclick='exportar_ManttoEquipo2();' width='32px' style='cursor: pointer; display: inline;' src='" + siteRoot + "Content/Images/ExportExcel.png' />");
    $('#legendManttoEquipo2 .itemMantto').click(function () {
        var inx = $(this).index(),
           point = manttos[inx];
        cargarManttoEquipo2(point);
        $('#tituloManttoEquipo2').html("EVOLUCIÓN DEL NÚMERO DE MANTENIMIENTOS " + manttosNombre[inx] + " POR TIPO DE MANTENIMIENTO Y EQUIPO");
    });
    // grafico 06
    $('#legendTipoManttoTipoEmpresa').html(txt);
    $('#excelTipoManttoTipoEmpresa').html("<img onclick='exportar_TipoManttoTipoEmpresa();' width='32px' style='cursor: pointer; display: inline;' src='" + siteRoot + "Content/Images/ExportExcel.png' />");
    $('#legendTipoManttoTipoEmpresa .itemMantto').click(function () {
        var inx = $(this).index(),
           point = manttos[inx];
        cargarTipoManttoTipoEmpresa(point);
        $('#tituloTipoManttoTipoEmpresa').html("EVOLUCIÓN DEL NÚMERO DE MANTENIMIENTOS " + manttosNombre[inx] + " POR TIPO DE MANTENIMIENTO Y EMPRESAS");
    });
}

graficoManttoEmpresa = function (result) {
    var json = result;
    var jsondata = [];
    var tiposmanto = [];
    var idsmantto = [];

    for (var i in json) {
        jsondata.push([json[i].Emprnomb, json[i].Porcentajemantto]);
    }

    $('#graficoManttoEmpresa').highcharts({
        chart: {
            type: 'pie',
            options3d: {
                enabled: true,
                alpha: 45,
                beta: 0
            }
        },
        title: {
            text: ''
        },
        tooltip: {
            pointFormat: '{series.name}: <b>{point.percentage}%</b>',
            percentageDecimals: 1
        },
        plotOptions: {
            pie: {
                allowPointSelect: true,
                cursor: 'pointer',
                depth: 35,
                dataLabels: {
                    enabled: true,
                    formatter: function () {
                        return '<b>' + this.point.name + '</b>: ' + $.number(this.percentage, 3, ',', ' ') + ' %';
                    }
                }
            }
        },
        series: [{
            type: 'pie',
            name: 'Participación',
            data: jsondata
        }]
    });
}

graficoManttoEquipo = function (result) {
    var json = result;
    var jsondata = [];

    for (var i in json) {
        jsondata.push([json[i].Famnomb, json[i].Porcentajemantto]);
    }

    $('#graficoManttoEquipo').highcharts({
        chart: {
            type: 'pie',
            options3d: {
                enabled: true,
                alpha: 45,
                beta: 0
            }
        },
        title: {
            text: ''
        },
        tooltip: {
            pointFormat: '{series.name}: <b>{point.percentage}%</b>',
            percentageDecimals: 1
        },
        plotOptions: {
            pie: {
                allowPointSelect: true,
                cursor: 'pointer',
                depth: 35,
                dataLabels: {
                    enabled: true,
                    formatter: function () {
                        return '<b>' + this.point.name + '</b>: ' + $.number(this.percentage, 3, ',', ' ') + ' %';
                    }
                }
            }
        },
        series: [{
            type: 'pie',
            name: 'Participación',
            data: jsondata
        }]
    });
}

function encontrarorden(matriz) {

    var totales = [];
    var orden = [];
    for (var i in matriz) {
        if (i == 0) {
            totales = new Array(matriz[i].length);
            for (z = 0; z < matriz[i].length; z++)
                totales[z] = 0;
        }
        for (var j in matriz[i]) {
            if (matriz[i][j] != null)
                totales[j] += matriz[i][j];
        }
    }

    for (var i in totales) {
        orden.push(i);
    }
    var piv = 0;
    var indice = 0;

    for (var i in totales) {
        for (j = 0; j < totales.length - 1 - i; j++) {
            if (totales[j] < totales[j + 1]) {
                piv = totales[j + 1];
                totales[j + 1] = totales[j];
                totales[j] = piv;
                piv = orden[j + 1];
                orden[j + 1] = orden[j];
                orden[j] = piv;
            }

        }

    }

    return orden;
}

function ordernarmatriz(orden, matriz) {
    var matrizaux = [];
    for (var i in matriz) {

        matrizaux = new Array(matriz[i].length);

        for (z = 0; z < matrizaux.length; z++)
            matrizaux[z] = matriz[i][z];

        for (var j in matriz[i]) {
            matriz[i][j] = matrizaux[orden[j]];
        }
    }

    return matriz;
}

function ordenaraxisx(orden, vector) {
    var vectoraux = [];
    var piv = 0;
    vectoraux = new Array(vector.length);
    for (var i in vector) {
        vectoraux[i] = vector[i];
    }
    for (var i in vector) {

        vector[i] = vectoraux[orden[i]];
    }
    return vector;
}

graficoManttoEmpresaEquipo1 = function (result) {
    var json = result;
    var jsontipomanto = [];
    var jsonemp = [];
    var totemp = 0;
    var nomempr = "";
    for (var i in json) {
        if (json[i].Emprabrev != nomempr) {
            jsonemp.push(json[i].Emprabrev);
        }
        nomempr = json[i].Emprabrev;
        //lleno arreglo con los tipos de MANTTOS
        if (jsontipomanto.indexOf(json[i].Tipoevendesc) < 0) {
            jsontipomanto.push(json[i].Tipoevendesc);
        }
    }

    var jsonmanto = [];
    for (var i in jsontipomanto) {
        jsonmanto[i] = [];
        for (var j in jsonemp) {
            jsonmanto[i].push(null);
        }
    }

    var k;
    var j;
    for (var i in json) {
        k = jsontipomanto.indexOf(json[i].Tipoevendesc);
        j = jsonemp.indexOf(json[i].Emprabrev);
        jsonmanto[k][j] = json[i].Subtotal;
    }
    var orden = encontrarorden(jsonmanto);
    jsonmanto = ordernarmatriz(orden, jsonmanto);
    jsonemp = ordenaraxisx(orden, jsonemp);
    var opcion = {
        chart: {
            type: 'column'
        },
        title: {
            text: ''
        },
        xAxis: {
            categories: jsonemp,
            style: {

                fontSize: '5'
            }

        },
        yAxis: {
            min: 0,
            title: {
                text: 'NÚMERO DE MANTENIMIENTOS'
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
                    this.series.name + ': ' + this.y + '<br/>' +
                    'Total: ' + this.point.stackTotal;
            }
        },
        plotOptions: {
            column: {
                stacking: 'normal',
                dataLabels: {
                    enabled: true,
                    color: (Highcharts.theme && Highcharts.theme.dataLabelsColor) || 'white',
                    style: {
                        textShadow: '0 0 3px black, 0 0 3px black'
                    }
                }
            }
        },
        series: []

    };


    for (var i in jsonmanto) {
        opcion.series.push({
            name: jsontipomanto[i],
            data: jsonmanto[i]
        });
    }


    $('#graficoManttoEmpresaEquipo1').highcharts(opcion);
}

graficoManttoEmpresaEquipo2 = function (result) {
    var json = result;
    var jsonempresa = [];
    var jsondata = [];
    var jsontipoequipo = [];
    var totemp = 0;
    var nomempr = "";
    var nomtipoequip = "";

    for (var i in json) {
        // lleno arreglo con nombres de los equipos y otro arreglo con tipo de mantenimiento
        if (json[i].Emprabrev != nomempr) {
            jsonempresa.push(json[i].Emprabrev);
        }
        nomempr = json[i].Emprabrev;
        //lleno arreglo con los tipos de equipos
        if (jsontipoequipo.indexOf(json[i].Famnomb) < 0) {
            jsontipoequipo.push(json[i].Famnomb);
        }
    }
    var jsonmanto = [];
    for (var i in jsontipoequipo) {
        jsonmanto[i] = [];
        for (var j in jsonempresa) {
            jsonmanto[i].push(null);
        }
    }

    var k;
    var j;
    for (var i in json) {
        k = jsontipoequipo.indexOf(json[i].Famnomb);
        j = jsonempresa.indexOf(json[i].Emprabrev);
        jsonmanto[k][j] = json[i].Subtotal;
    }
    var orden = encontrarorden(jsonmanto);
    jsonmanto = ordernarmatriz(orden, jsonmanto);
    jsonempresa = ordenaraxisx(orden, jsonempresa);

    var opcion = {
        chart: {
            type: 'column'
        },
        title: {
            text: ''
        },
        xAxis: {
            categories: jsonempresa,
            style: {

                fontSize: '5'
            }

        },
        yAxis: {
            min: 0,
            title: {
                text: 'NÙMERO DE MANTENIMIENTOS'
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
                    this.series.name + ': ' + this.y + '<br/>' +
                    'Total: ' + this.point.stackTotal;
            }
        },
        plotOptions: {
            column: {
                stacking: 'normal',
                dataLabels: {
                    enabled: true,
                    color: (Highcharts.theme && Highcharts.theme.dataLabelsColor) || 'white',
                    style: {
                        textShadow: '0 0 3px black, 0 0 3px black'
                    }
                }
            }
        },
        series: []

    };


    for (var i in jsonmanto) {
        opcion.series.push({
            name: jsontipoequipo[i],
            data: jsonmanto[i]
        });
    }


    $('#graficoManttoEmpresaEquipo2').highcharts(opcion);

}

graficoManttoEquipo2 = function (result) {
    var json = result;
    var jsontipoequip = [];
    var jsondata = [];
    var jsontipomanto = [];
    var nomtipoequip = "";

    for (var i in json) {
        // lleno arreglo con nombres de los equipos y otro arreglo con tipo de mantenimiento
        //if (json[i].Famnomb != nomtipoequip) {
        //    jsontipoequip.push(json[i].Famnomb);
        //}
        if (jsontipoequip.indexOf(json[i].Famnomb) < 0) {
            jsontipoequip.push(json[i].Famnomb);
        }
        //nomtipoequip = json[i].Famnomb;
        //lleno arreglo con los tipos de MANTTOS
        if (jsontipomanto.indexOf(json[i].Tipoevendesc) < 0) {
            jsontipomanto.push(json[i].Tipoevendesc);
        }
    }
    var jsonmanto = [];
    for (var i in jsontipomanto) {
        jsonmanto[i] = [];
        for (var j in jsontipoequip) {
            jsonmanto[i].push(null);
        }
    }

    var k;
    var j;
    for (var i in json) {
        k = jsontipomanto.indexOf(json[i].Tipoevendesc);
        j = jsontipoequip.indexOf(json[i].Famnomb);
        jsonmanto[k][j] = json[i].Subtotal;
    }
    var orden = encontrarorden(jsonmanto);
    jsonmanto = ordernarmatriz(orden, jsonmanto);
    jsontipoequip = ordenaraxisx(orden, jsontipoequip);
    var opcion = {
        chart: {
            type: 'bar'
        },
        title: {
            text: ''
        },
        xAxis: {
            categories: jsontipoequip,
            style: {

                fontSize: '5'
            }

        },
        yAxis: {
            min: 0,
            title: {
                text: 'NÚMERO DE MANTENIMIENTOS'
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
                    this.series.name + ': ' + this.y + '<br/>' +
                    'Total: ' + this.point.stackTotal;
            }
        },
        plotOptions: {
            series: {
                stacking: 'normal'
            }
        },
        series: []

    };

    for (var i in jsonmanto) {
        opcion.series.push({
            name: jsontipomanto[i],
            data: jsonmanto[i]
        });
    }

    $('#graficoManttoEquipo2').highcharts(opcion);

}

graficoTipoManttoTipoEmpresa = function (result) {
    var json = result;
    var jsontipomanto = [];
    var jsontipoemp = [];
    var tipomanto = "";
    for (var i in json) {
        if (json[i].Tipoevendesc != tipomanto) {
            jsontipomanto.push(json[i].Tipoevendesc);
        }
        tipomanto = json[i].Tipoevendesc;
        //lleno arreglo con los tipos de MANTTOS

        if (jsontipoemp.indexOf(json[i].Tipoemprdesc) < 0) {
            jsontipoemp.push(json[i].Tipoemprdesc);

        }
    }

    var jsonmanto = [];
    for (var i in jsontipoemp) {

        jsonmanto[i] = [];
        for (var j in jsontipomanto) {
            jsonmanto[i].push(null);
        }
    }

    var k;
    var j;
    for (var i in json) {
        k = jsontipoemp.indexOf(json[i].Tipoemprdesc);
        j = jsontipomanto.indexOf(json[i].Tipoevendesc);
        jsonmanto[k][j] = json[i].Subtotal;
    }
    var orden = encontrarorden(jsonmanto);
    jsonmanto = ordernarmatriz(orden, jsonmanto);
    jsontipomanto = ordenaraxisx(orden, jsontipomanto);

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
            text: ''
        },
        xAxis: {
            categories: jsontipomanto,
            style: {

                fontSize: '5'
            }
        },
        yAxis: {
            min: 0,
            title: {
                text: 'NÚMERO DE MANTENIMIENTOS'
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
                    this.series.name + ': ' + this.y + '<br/>' +
                    'Total: ' + this.point.stackTotal;
            }
        },
        plotOptions: {
            column: {
                stacking: 'normal',
                dataLabels: {
                    enabled: true,
                    color: (Highcharts.theme && Highcharts.theme.dataLabelsColor) || 'white',
                    style: {
                        textShadow: '0 0 3px black, 0 0 3px black'
                    }
                }
            }
        },
        series: []
    };

    for (var i in jsonmanto) {
        opcion.series.push({
            name: jsontipoemp[i],
            data: jsonmanto[i]
        });
    }

    $('#graficoTipoMantoTipoEmpresa').highcharts(opcion);
}

function generarGrafico() {
    if ($('#cbEmpresa').multipleSelect('rowCountSelected') <= 100 || $('#cbEmpresa').multipleSelect('isAllSelected') == "S") {

        var empresa = $('#cbEmpresa').multipleSelect('getSelects');
        var tipoMantenimiento = $('#cbTipoMantenimiento').multipleSelect('getSelects');
        var tipoEmpresa = $('#cbTipoEmpresa').multipleSelect('getSelects');
        var tipoEquipo = $('#cbFamilia').multipleSelect('getSelects');
        var tipoMantto = $('#cbTipoMantto').multipleSelect('getSelects');

        if (empresa == "[object Object]") empresa = "-1";

        //$('#hfEmpresa').val(empresa);
        $('#hfEmpresa').val($('#cbEmpresa').multipleSelect('isAllSelected') == "S" ? "-1" : empresa);
        $('#hfTipoMantenimiento').val(tipoMantenimiento);
        $('#hfTipoEmpresa').val(tipoEmpresa);
        $('#hfTipoEquipo').val(tipoEquipo);
        $('#hfTipoMantto').val(tipoMantto);

        $('#reporte').css("display", "none");
        $('#paginado').css("display", "none");
        $('#tab-grafico').css("display", "block");

        $.ajax({
            type: 'POST',
            url: controlador + "mantenimiento/graficoreporte",
            data: {
                tiposMantenimiento: $('#hfTipoMantenimiento').val(),
                fechaInicial: GetDateNormalFormat($('#FechaDesde').val()),
                fechaFinal: GetDateNormalFormat($('#FechaHasta').val()),
                indispo: $('#cbIndispo').val(),
                tiposEmpresa: $('#hfTipoEmpresa').val(), empresas: $('#hfEmpresa').val(),
                tiposEquipo: $('#hfTipoEquipo').val(), interrupcion: $('#cbInterrupcion').val(),
                tiposMantto: $('#hfTipoMantto').val()
            },
            success: function (evt) {

                $.ajax({
                    type: 'POST',
                    url: controlador + "mantenimiento/ObtenerTipoManttos",
                    success: function (evt) {
                        obtenerTipoMantto(evt);
                    },
                    error: function () {
                        alert("Ha ocurrido un error");
                    }
                });

                cargarManttoEmpresa(0);
                cargarManttoEquipo(0);
                cargarManttoEmpresaEquipo1(0);
                cargarManttoEmpresaEquipo2(0);
                cargarManttoEquipo2(0);
                cargarTipoManttoTipoEmpresa(0);
            },
            error: function () {
                alert("Ha ocurrido un error");
            }
        });

    }
    else {
        alert("No puede seleccionar más de 100 empresas.");
    }
}

function exportar_ManttoEmpresa() {  // Genera Reporte excel de Mantenimiento por empresas - Grafico 01
    $.ajax({
        type: 'POST',
        url: controlador + "mantenimiento/GenerarArchivoManttoEmpresa",
        data: {
            fechaInicial: GetDateNormalFormat($('#FechaDesde').val()),
            fechaFinal: GetDateNormalFormat($('#FechaHasta').val())
        },
        dataType: 'json',
        success: function (result) {
            if (result == 1) {
                window.location = controlador + "mantenimiento/ExportarReporte?tipo=1";
            }
            if (result == -1) {
                mostrarError();
            }
        },
        error: function () {
            alert("Error en Ajax Exportar Empresa");
        }
    });
}

function exportar_ManttoEquipo() {
    // Excel grafico 02
    $.ajax({
        type: 'POST',
        url: controlador + "mantenimiento/GenerarArchivoManttoEquipo",
        data: {
            fechaInicial: GetDateNormalFormat($('#FechaDesde').val()),
            fechaFinal: GetDateNormalFormat($('#FechaHasta').val())
        },
        dataType: 'json',
        success: function (result) {
            if (result == 1) {
                window.location = controlador + "mantenimiento/ExportarReporte?tipo=2";
            }
            if (result == -1) {
                mostrarError();
            }
        },
        error: function () {
            mostrarError();
        }
    });
}

function exportar_ManttoEmpresaEquipo1() {
    //Excel grafico 03
    $.ajax({
        type: 'POST',
        url: controlador + "mantenimiento/GenerarArchivoManttoEmpresaEquipo1",
        data: {
            fechaInicial: GetDateNormalFormat($('#FechaDesde').val()),
            fechaFinal: GetDateNormalFormat($('#FechaHasta').val())
        },
        dataType: 'json',
        success: function (result) {
            if (result == 1) {
                window.location = controlador + "mantenimiento/ExportarReporte?tipo=3";
            }
            if (result == -1) {
                mostrarError();
            }
        },
        error: function () {
            mostrarError();
        }
    });
}

function exportar_ManttoEmpresaEquipo2() {
    //EXcel grafico 04
    $.ajax({
        type: 'POST',
        url: controlador + "mantenimiento/GenerarArchivoManttoEmpresaEquipo2",
        data: {
            fechaInicial: GetDateNormalFormat($('#FechaDesde').val()),
            fechaFinal: GetDateNormalFormat($('#FechaHasta').val())
        },
        dataType: 'json',
        success: function (result) {
            if (result == 1) {
                window.location = controlador + "mantenimiento/ExportarReporte?tipo=4";
            }
            if (result == -1) {
                mostrarError();
            }
        },
        error: function () {
            mostrarError();
        }
    });
}

function exportar_ManttoEquipo2() {
    //Excel grafico 05
    $.ajax({
        type: 'POST',
        url: controlador + "mantenimiento/GenerarArchivoManttoEquipo2",
        data: {
            fechaInicial: GetDateNormalFormat($('#FechaDesde').val()),
            fechaFinal: GetDateNormalFormat($('#FechaHasta').val())
        },
        dataType: 'json',
        success: function (result) {
            if (result == 1) {
                window.location = controlador + "mantenimiento/ExportarReporte?tipo=5";
            }
            if (result == -1) {
                mostrarError();
            }
        },
        error: function () {
            mostrarError();
        }
    });
}

function exportar_TipoManttoTipoEmpresa() {
    //Excel grafico 06
    $.ajax({
        type: 'POST',
        url: controlador + "mantenimiento/GenerarArchivoTipoMantoTipoEmpresa",
        data: {
            fechaInicial: GetDateNormalFormat($('#FechaDesde').val()),
            fechaFinal: GetDateNormalFormat($('#FechaHasta').val())
        },
        dataType: 'json',
        success: function (result) {
            if (result == 1) {
                window.location = controlador + "mantenimiento/ExportarReporte?tipo=6";
            }
            if (result == -1) {
                mostrarError();
            }
        },
        error: function () {
            mostrarError();
        }
    });
}

function mostrarListado(nroPagina) {
    var empresa = $('#cbEmpresa').multipleSelect('getSelects');

    var tipoMantenimiento = $('#cbTipoMantenimiento').multipleSelect('getSelects');
    var tipoEmpresa = $('#cbTipoEmpresa').multipleSelect('getSelects');
    var tipoEquipo = $('#cbFamilia').multipleSelect('getSelects');
    var tipoMantto = $('#cbTipoMantto').multipleSelect('getSelects');

    if (empresa == "[object Object]") empresa = "-1";
    if (tipoMantto == "[object Object]") tipoMantto = "-1";
    if (tipoMantto == "") tipoMantto = "-1";

    //$('#hfEmpresa').val(empresa);
    $('#hfEmpresa').val($('#cbEmpresa').multipleSelect('isAllSelected') == "S" ? "-1" : empresa);
    $('#hfTipoMantenimiento').val(tipoMantenimiento);
    $('#hfTipoEmpresa').val(tipoEmpresa);
    $('#hfTipoEquipo').val(tipoEquipo);
    $('#hfTipoMantto').val(tipoMantto);

    $('#reporte').css("display", "block");
    $('#paginado').css("display", "block");
    $('#tab-grafico').css("display", "none");

    $.ajax({
        type: 'POST',
        url: controlador + "mantenimiento/lista",
        data: {
            tiposMantenimiento: $('#hfTipoMantenimiento').val(),
            fechaInicial: GetDateNormalFormat($('#FechaDesde').val()),
            fechaFinal: GetDateNormalFormat($('#FechaHasta').val()),
            indispo: $('#cbIndispo').val(),
            tiposEmpresa: $('#hfTipoEmpresa').val(), empresas: $('#hfEmpresa').val(),
            tiposEquipo: $('#hfTipoEquipo').val(), interrupcion: $('#cbInterrupcion').val(),
            tiposMantto: $('#hfTipoMantto').val(),
            nroPagina: nroPagina
        },
        success: function (evt) {
           /* $('#listado').css("width", $('#mainLayout').width() + "px");*/

            $('#listado').html(evt);
          
            $('#tabla').dataTable({
                "scrollY": 430,
                "scrollX": true,
                "sDom": 'pt',
                "ordering": false,
                "iDisplayLength": 50
            });
            pintarPaginado();
            $('#tab-grafico').css("display", "none");
        },
        error: function () {
            alert("Ha ocurrido un error");
        }
    });
}

function mostrarLeyenda() {
    if ($('#leyenda').css("display") == "none") {
        $('#leyenda').css("display", "block");
        $('#spanLeyenda').text("Ocultar leyenda CIER");
    }
    else if ($('#leyenda').css("display") == "block") {
        $('#leyenda').css("display", "none");
        $('#spanLeyenda').text("Mostrar leyenda CIER");
    }
}

function pintarBusqueda(nroPagina) {
    mostrarListado(nroPagina);
}

function mostrarDetalle(id) {
    location.href = controlador + "evento/detalle?id=" + id;
}
// exporta el reporte general consultado a archivo excel
function exportar() {

    if ($('#cbEmpresa').multipleSelect('rowCountSelected') <= 100 || $('#cbEmpresa').multipleSelect('isAllSelected') == "S") {

        var empresa = $('#cbEmpresa').multipleSelect('getSelects');
        var tipoMantenimiento = $('#cbTipoMantenimiento').multipleSelect('getSelects');
        var tipoEmpresa = $('#cbTipoEmpresa').multipleSelect('getSelects');
        var tipoEquipo = $('#cbFamilia').multipleSelect('getSelects');
        var tipoMantto = $('#cbTipoMantto').multipleSelect('getSelects');

        if (empresa == "[object Object]") empresa = "-1";

        //$('#hfEmpresa').val(empresa);
        $('#hfEmpresa').val($('#cbEmpresa').multipleSelect('isAllSelected') == "S" ? "-1" : empresa);
        $('#hfTipoMantenimiento').val(tipoMantenimiento);
        $('#hfTipoEmpresa').val(tipoEmpresa);
        $('#hfTipoEquipo').val(tipoEquipo);
        $('#hfTipoMantto').val(tipoMantto);

        $.ajax({
            type: 'POST',
            url: controlador + 'mantenimiento/GenerarArchivoReporte',
            data: {
                tiposMantenimiento: $('#hfTipoMantenimiento').val(),
                fechaInicial: GetDateNormalFormat($('#FechaDesde').val()),
                fechaFinal: GetDateNormalFormat($('#FechaHasta').val()),
                indispo: $('#cbIndispo').val(),
                tiposEmpresa: $('#hfTipoEmpresa').val(), empresas: $('#hfEmpresa').val(),
                tiposEquipo: $('#hfTipoEquipo').val(), interrupcion: $('#cbInterrupcion').val(),
                tiposMantto: $('#hfTipoMantto').val()
            },
            dataType: 'json',
            success: function (result) {
                if (result == 1) {
                    window.location = controlador + "mantenimiento/ExportarReporte?tipo=0";
                }
                if (result == -1) {
                    mostrarError();
                }
            },
            error: function () {
                mostrarError();
            }
        });
    }
    else {
        alert("No puede seleccionar más de 100 empresas.");
    }
}

function exportar2() {
    $.ajax({
        type: 'POST',
        url: controlador + "mantenimiento/Paginado2",
        data: $('#frmBusqueda').serialize(),
        success: function (evt) {
            $('#paginado').html(evt);
            mostrarPaginado();
        },
        error: function () {
            alert("Ha ocurrido un error");
        }
    });

}

mostrarError = function () {
    alert("Ha ocurrido un error ");
}

// Convierte formato yyyy-mm-dd a dd/mm/yyyy
GetDateNormalFormat = function (fechaFormatoISO) {
    let fechaNormal = fechaFormatoISO.substring(8, 10) + "/" + fechaFormatoISO.substring(5, 7) + "/" + fechaFormatoISO.substring(0, 4);
    return fechaNormal;
}