var controlador = siteRoot + 'Migraciones/AnexoA/'
$(function () {

    $('#cbEmpresa').multipleSelect({
        width: '150px',
        filter: true,
        onClose: function (view) {
            cargarGPS();
        }
    });

    $('#btnBuscar').click(function () {
        cargarLista();
    });

    $('#txtFechaInicio').Zebra_DatePicker({
        //direction: -1
    });

    $('#txtFechaFin').Zebra_DatePicker({
        //direction: -1
    });
    cargarValoresIniciales();
});

function cargarValoresIniciales() {
    $('#cbEmpresa').multipleSelect('checkAll');
    cargarGPS();
}

function cargarGPS() {
    $.ajax({
        type: 'POST',
        url: controlador + 'CargarGPS',
        data: {
            idEmpresa: getEmpresa()
        },
        success: function (aData) {
            $('#gps').html(aData);
            $('#cbGPS').multipleSelect({
                width: '150px',
                filter: true,
                onClose: function (view) {
                    cargarLista();
                }
            });
            $('#cbGPS').multipleSelect('checkAll');

            cargarLista();
        },
        error: function () {
            alert("Ha ocurrido un error");
        }
    });
}

function cargarLista() {
    $.ajax({
        type: 'POST',
        url: controlador + 'CargarListaVariacionesSostenidasSubitas',
        data: {
            empresas: getEmpresa(),
            fechaInicio: getFechaInicio(),
            fechaFin: getFechaFin(),
            gps: getGPS()
        },
        success: function (aData) {
            $('#listado').html(aData.Resultado);
            $('#idGraficoContainer').html('');
        },
        error: function () {
            alert("Ha ocurrido un error");
        }
    });
}

function fnClickFrecuencia(x) {
    $.ajax({
        type: 'POST',
        url: controlador + 'CargarGraficoVariacionesSostenidasSubitas',
        data: { empresas: '', gps: x, fechaInicio: getFechaInicio() },
        dataType: 'json',
        success: function (aData) {
            if (aData.nRegistros > 0) {
                DiseñoGrafico(aData);
                $('#idVistaGrafica2').html(aData.Resultado);
            }
            else { alert('Sin informacion!'); }
        },
        error: function () {
            alert("Ha ocurrido un error");
        }
    });
}

function DiseñoGrafico(result) {
    var opcion;

    var json = result.ListaGrafico;
    var jsondata = [];
    var indice = 3;
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
            //index: indice
        });
        indice--;
    }

    opcion = {
        chart: {
            type: 'spline'
        },
        title: {
            text: result.Grafico.TitleText
        },
        xAxis: {
            title: {
                text: ''
            },
            categories: result.Grafico.XAxisCategories
        },
        credits: {
            enabled: false
        },
        yAxis: {
            title: { text: '' },
            labels: {
                formatter: function () {
                    return this.value + '%';
                }
            }/*,
            categories: [0,10,20,30,40,50,60,70]*/
        },
        legend: {
            reversed: true
        },
        plotOptions: {
            bar: {
                dataLabels: {
                    enabled: false
                }
            }
        },
        series: jsondata
    };

    $('#idVistaGrafica').highcharts(opcion);

    mostrarGrafico();
}

function mostrarGrafico() {

    setTimeout(function () {
        $('#idGrafico2').bPopup({
            easing: 'easeOutBack',
            speed: 450,
            transition: 'slideDown',
            modalClose: false
        });
    }, 50);
}