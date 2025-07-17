var controlador = siteRoot + 'Siosein/TablasPrieDeclaracionMen/'

$(function () {
    $('#cbEmpresa').multipleSelect({
        width: '150px',
        filter: true,
        onClose: function (view) {
            cargarListaDifusionRecaudacionPeajesConexionTransmision();
        }
    });

    $('#cbEmpresa').multipleSelect('checkAll');

    cargarListaDifusionRecaudacionPeajesConexionTransmision();
    
});

function cargarListaDifusionRecaudacionPeajesConexionTransmision() {
    var empresa = $('#cbEmpresa').multipleSelect('getSelects');
    var contar = $("#cbEmpresa option:not(:selected)").length;
    if (empresa == "") { empresa = "-1"; }
    if (contar == 0) { empresa = "-1"; }
    $('#hfEmpresa').val(empresa);
    var mesAnio = $('#txtFecha').val();

    $.ajax({
        type: 'POST',
        url: controlador + 'CargarListaDifusionRecaudacionPeajesConexionTransmision',
        data: { mesAnio: mesAnio, idEmpresa: $('#hfEmpresa').val() },
        success: function (aData) {
            $('#listado1').html(aData.Resultado);
            $('#tabla_dif').dataTable();
            cargarGraficoDifusionRecaudacionPeajesConexionTransmision();
        },
        error: function () {
            alert("Ha ocurrido un error");
        }
    });
}

function cargarGraficoDifusionRecaudacionPeajesConexionTransmision() {
    var empresa = $('#cbEmpresa').multipleSelect('getSelects');
    var contar = $("#cbEmpresa option:not(:selected)").length;
    if (empresa == "") { empresa = "-1"; }
    if (contar == 0) { empresa = "-1"; }
    $('#hfEmpresa').val(empresa);
    var mesAnio = $('#txtFecha').val();

    $.ajax({
        type: 'POST',
        url: controlador + 'CargarGraficoDifusionRecaudacionPeajesConexionTransmision',
        data: { mesAnio: mesAnio, idEmpresa: $('#hfEmpresa').val() },
        dataType: 'json',
        success: function (aData) {
            if (aData.Serie1 != null) {
                disenioGrafico(aData, 'idGrafico1', 'MONTOS RECAUDADOS POR PEAJES CALCULADOS POR CONEXION Y TRANSMISION EN EL SEIN (S/.)', 1);
            }
            
            //if (aData.Lista1.length > 0) {
            //    diseñoGrafico(aData.Lista1, 'idGrafico1', 'POTENCIA EFECTIVA POR EMPRESAS');
            //} else $('#idGrafico1').html('Sin Grafico - No existen registros a mostrar!');
        },
        error: function () {
            alert('Ha ocurrido un error');
        }
    });
}

function disenioGrafico(data, grafico, texto, tip) {
    if (tip == 1) {
        var Categorias = [];

        var Name1 = "";
        var Name2 = "";
        var Name3 = "";
        var Name4 = "";

        var Serie1 = [];
        var Serie2 = [];
        var Serie3 = [];
        var Serie4 = [];
        var Serie5 = [];
        var Serie6 = [];
        var Serie7 = [];
        var Serie8 = [];
        var Serie9 = [];

        for (j = 0; j < data.Categoria.length; j++) {
            Categorias[j] = data.Categoria[j];
        }
        for (j = 0; j < data.Serie1.length; j++) {
            Serie1[j] = data.Serie1[j];
        }
        for (j = 0; j < data.Serie2.length; j++) {
            Serie2[j] = data.Serie2[j];
        }
        for (j = 0; j < data.Serie3.length; j++) {
            Serie3[j] = data.Serie3[j];
        }
        for (j = 0; j < data.Serie4.length; j++) {
            Serie4[j] = data.Serie4[j];
        }
        for (j = 0; j < data.Serie5.length; j++) {
            Serie5[j] = data.Serie5[j];
        }
        for (j = 0; j < data.Serie6.length; j++) {
            Serie6[j] = data.Serie6[j];
        }
        for (j = 0; j < data.Serie7.length; j++) {
            Serie7[j] = data.Serie7[j];
        }
        for (j = 0; j < data.Serie8.length; j++) {
            Serie8[j] = data.Serie8[j];
        }
        for (j = 0; j < data.Serie9.length; j++) {
            Serie8[j] = data.Serie9[j];
        }




        Highcharts.chart(grafico, {
            title: {
                text: 'PROYECCION DE MEDIANO PLAZO DE LA PRODUCCION DE ENERGIA EN EL SEIN (MWh)'
            },
            xAxis: {
                categories: Categorias
            },
            yAxis: {
                title: {
                    text: 'MWh'
                },
            },
            labels: {
                items: [{
                    html: '',
                    style: {
                        left: '50px',
                        top: '18px',
                        color: (Highcharts.theme && Highcharts.theme.textColor) || 'black'
                    }
                }]
            },
            plotOptions: {
                column: {
                    stacking: 'normal',
                    dataLabels: {
                        enabled: true,
                        color: (Highcharts.theme && Highcharts.theme.dataLabelsColor) || 'white'
                    }
                },
                series: {
                    label: {
                        connectorAllowed: false
                    }
                }
            },
            series: [{
                type: 'column',
                name: 'RECUADACION TRANSMISION',
               /* color: '#356CAF',*/
                data: Serie1
            }, {
                type: 'column',
                name: 'RECAUACION GENERACION ADICIONAL',
               /* color: '#AD3330',*/
                data: Serie2
            },  {
                type: 'column',
                name: 'RECAUDACION SEG SUMNRF',
               /* color: '#85A83D',*/
                data: Serie3
            },{
                type: 'column',
                name: 'RECAUDACION SEG SUM RESERVA FRIA',
                /*color: '#356CAF',*/
                data: Serie4
            }, {
                type: 'column',
                name: 'RECAUDACION PRIMA RER',
               /* color: '#356CAF',*/
                data: Serie5
            }, {
                type: 'column',
                name: 'RECAUDACION PRIMA FISE',
               /* color: '#356CAF',*/
                data: Serie6
            }, {
                type: 'column',
                name: 'RECAUDACION PRIMA CASE',
                /*color: '#356CAF',*/
                data: Serie7
            }, {
                type: 'column',
                name: 'RECAUDACION CONFIABILIDAD',
                /*color: '#356CAF',*/
                data: Serie8
            }, {
                type: 'column',
                name: 'RECAUDACION OTROS CARGOS',
                /*color: '#356CAF',*/
                data: Serie9
            }
            ]
        });
    }
}