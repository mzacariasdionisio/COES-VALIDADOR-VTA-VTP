var controler = siteRoot + "transferencias/ComparativoEntregaRetiro/";

//Funciones de busqueda
$(document).ready(function () {
    var $chart = null;

    oTable = $('#tabla').dataTable({
        "sPaginationType": "full_numbers",
        "destroy": "true"
    });

    $("#tabla_length").css("display", "none");

    var _arrayCodigos = [];
    $(document).off('click', '#tabla input[type="checkbox"]');
    $(document).on('click', '#tabla input[type="checkbox"]', (event) => {
        var item = $(event.currentTarget);
        var codigos = "'" + item.data("valotrancodentret") + "'";

        var chk = $(event.currentTarget);
        if (chk.prop('checked')) {
            var findIndex = _arrayCodigos.findIndex(x => x == codigos);
            if (findIndex < 0) {
                _arrayCodigos.push(codigos);
            }
        } else {

            for (var i = _arrayCodigos.length - 1; i >= 0; i--) {
                if (_arrayCodigos[i].trim() == codigos) {
                    _arrayCodigos.splice(i, 1);
                }
            }
        }

    })

    $('#grafico1').click(function () {
        $('#grafico2').removeClass('active');
        $('#grafico1').addClass('active');

        var codigos = _arrayCodigos.join(',');

        metodo.dibujarGrafica(codigos, 0);

    });

    $('#grafico2').click(function () {
        $('#grafico1').removeClass('active');
        $('#grafico2').addClass('active');

        var codigos = "";

        $("#tabla").find('input[type="checkbox"]:checked').each(function (i, k) {

            var item = $(this);
            if (codigos == "") {
                codigos = "'" + item.data("valotrancodentret") + "'";
            } else {
                codigos = codigos + "," + "'" + item.data("valotrancodentret") + "'";
            }

        });

        metodo.dibujarGrafica(codigos, 1);

    });

    var metodo = {
        cargarDatosIniciales: function () {

            const timezone = new Date().getTimezoneOffset()
            Highcharts.setOptions({
                lang: {
                    loading: 'Cargando...',
                    months: ['Enero', 'Febrero', 'Marzo', 'Abril', 'Mayo', 'Junio', 'Julio', 'Agosto', 'Septiembre', 'Octubre', 'Noviembre', 'Diciembre'],
                    weekdays: ['Domingo', 'Lunes', 'Martes', 'Miércoles', 'Jueves', 'Viernes', 'Sábado'],
                    shortMonths: ['Ene', 'Feb', 'Mar', 'Abr', 'May', 'Jun', 'Jul', 'Ago', 'Sep', 'Oct', 'Nov', 'Dic'],
                    exportButtonTitle: "Exportar",
                    printButtonTitle: "Importar",
                    rangeSelectorFrom: "Desde",
                    rangeSelectorTo: "Hasta",
                    rangeSelectorZoom: "Período",
                    downloadPNG: 'Descargar imagen PNG',
                    downloadJPEG: 'Descargar imagen JPEG',
                    downloadPDF: 'Descargar imagen PDF',
                    downloadSVG: 'Descargar imagen SVG',
                    printChart: 'Imprimir',
                    resetZoom: 'Reiniciar zoom',
                    resetZoomTitle: 'Reiniciar zoom',
                    thousandsSep: ",",
                    decimalPoint: '.'
                },
                global: {
                    timezoneOffset: timezone
                },
                exporting: {
                    enabled: true
                }
            });
            metodo.chartJS.init(null);
        },
        chartJS: {
            init: (series) => {

                var id = $('#tab-container li.active a').attr('data-id')

                $chart = Highcharts.chart('contenedor1', {
                    chart: {
                        zoomType: 'x'
                    },
                    title: {
                        text: 'Consulta de Entregas y Retiro'
                    },
                    xAxis: {
                        type: 'datetime',

                    },
                    yAxis: {
                        title: {
                            text: 'MWh'
                        }
                    },
                    legend: {
                        enabled: true
                    },
                    plotOptions: {
                        area: {
                            fillColor: {

                                stops: [
                                    [0, Highcharts.getOptions().colors[0]],
                                    [1, Highcharts.color(Highcharts.getOptions().colors[0]).setOpacity(0).get('rgba')]
                                ]
                            },
                            marker: {
                                radius: 2
                            },
                            lineWidth: 1,
                            states: {
                                hover: {
                                    lineWidth: 1
                                }
                            },
                            threshold: null
                        }
                    },
                    series: series
                });

            },
            update: (series) => {
                chart.series[0].update({ series: series });
            }
        }
        ,
        dibujarGrafica: (codigos, tipo) => {
            var pericodi = $("#pericodi option:selected").val();
            var version = $("#recacodi option:selected").val();
            if (pericodi == "" || version == "") {
                alert('Por favor, seleccione el Periodo y la versión');
            }
            else {
                var trnenvtipinf = $("#trnenvtipinf option:selected").val();
                var empcodi = $("#emprcodi option:selected").val();
                var cliemprcodi = $("#cliemprcodi option:selected").val();
                var barrcodi = $("#barrcodi option:selected").val();
                var flag = $("#flag option:selected").val();
                if (empcodi == '')
                    empcodi = null;
                if (barrcodi == '')
                    barrcodi = null;
                if (pericodi == '')
                    pericodi = null;

                $.ajax({
                    type: 'POST',
                    url: controler + "MostrarGrafico",
                    data: {
                        trnenvtipinf: trnenvtipinf, pericodi: pericodi, version: version,
                        empcodi: empcodi, codigos: codigos, tipo: tipo,
                        FechaInicio: $('#txtfechaIni').val() == '' ? '01/01/1900' : $('#txtfechaIni').val(),
                        FechaFin: $('#txtfechaFin').val() == '' ? '01/01/2900' : $('#txtfechaFin').val(),
                    },
                    success: function (evt) {


                        if (evt.dataER == null)
                            return false;
                        arrayGrafica = [];
                        evt.dataER.map((itemParent, index, array) => {
                            let barra = {
                                type: 'area',
                                name: itemParent.Codigo,
                                data: []
                            };

                            itemParent.entregaRetiros.map((value, index, array) => {
                                barra.data.push([
                                    parseInt(value.FechaIntervalo.replace("/Date(", "").replace(")/", ""), 10),
                                    value.CMGREnergia
                                ])
                            })
                            arrayGrafica.push(barra);
                        })

                        metodo.chartJS.init(arrayGrafica);


                    },
                    error: function () {
                        mostrarError();
                    }
                });
            }

        }

    }

    var arrayData = [];

    armarData = function (dataER, codigos, tipo) {
        var lista = [];
        var cont = 1440;

        if (tipo == 0) {
            var aCodigos = codigos.split(',');

            $.each(aCodigos, function (i, el) {
                cont = 1440;
                var cod = el.replace("'", "").replace("'", "");
                var obj = {
                    name: cod,
                    data: []
                };

                $.each(dataER, function (j, item) {

                    if (cod == item.ValoTranCodEntRet) {

                        for (var prop in item) {

                            if (prop == 'VT1') {
                                var contDia = cont / 1440; obj.data.push([contDia, item[prop]]);
                                cont += 15;
                            }
                            if (prop == 'VT2') {

                                var contDia = cont / 1440; obj.data.push([contDia, item[prop]]);
                                cont += 15;

                            }
                            if (prop == 'VT3') {

                                var contDia = cont / 1440; obj.data.push([contDia, item[prop]]);
                                cont += 15;

                            }
                            if (prop == 'VT4') {
                                var contDia = cont / 1440; obj.data.push([contDia, item[prop]]);
                                cont += 15;
                            }
                            if (prop == 'VT5') {
                                var contDia = cont / 1440; obj.data.push([contDia, item[prop]]);
                                cont += 15;
                            }
                            if (prop == 'VT6') {
                                var contDia = cont / 1440; obj.data.push([contDia, item[prop]]);
                                cont += 15;
                            }
                            if (prop == 'VT7') {
                                var contDia = cont / 1440; obj.data.push([contDia, item[prop]]);
                                cont += 15;
                            }
                            if (prop == 'VT8') {
                                var contDia = cont / 1440; obj.data.push([contDia, item[prop]]);
                                cont += 15;
                            }
                            if (prop == 'VT9') {
                                var contDia = cont / 1440; obj.data.push([contDia, item[prop]]);
                                cont += 15;
                            }

                            if (prop == 'VT10') {
                                var contDia = cont / 1440; obj.data.push([contDia, item[prop]]);
                                cont += 15;
                            }
                            if (prop == 'VT11') {

                                var contDia = cont / 1440; obj.data.push([contDia, item[prop]])
                                cont += 15;
                            }
                            if (prop == 'VT12') {

                                var contDia = cont / 1440; obj.data.push([contDia, item[prop]])
                                cont += 15;
                            }
                            if (prop == 'VT13') {

                                var contDia = cont / 1440; obj.data.push([contDia, item[prop]])
                                cont += 15;
                            }
                            if (prop == 'VT14') {

                                var contDia = cont / 1440; obj.data.push([contDia, item[prop]])
                                cont += 15;
                            }
                            if (prop == 'VT15') {

                                var contDia = cont / 1440; obj.data.push([contDia, item[prop]])
                                cont += 15;
                            }
                            if (prop == 'VT16') {

                                var contDia = cont / 1440; obj.data.push([contDia, item[prop]])
                                cont += 15;
                            }
                            if (prop == 'VT17') {

                                var contDia = cont / 1440; obj.data.push([contDia, item[prop]])
                                cont += 15;
                            }
                            if (prop == 'VT18') {

                                var contDia = cont / 1440; obj.data.push([contDia, item[prop]])
                                cont += 15;
                            }
                            if (prop == 'VT19') {

                                var contDia = cont / 1440; obj.data.push([contDia, item[prop]])
                                cont += 15;
                            }
                            if (prop == 'VT20') {

                                var contDia = cont / 1440; obj.data.push([contDia, item[prop]])
                                cont += 15;
                            }
                            if (prop == 'VT21') {

                                var contDia = cont / 1440; obj.data.push([contDia, item[prop]])
                                cont += 15;
                            }
                            if (prop == 'VT22') {

                                var contDia = cont / 1440; obj.data.push([contDia, item[prop]])
                                cont += 15;
                            }
                            if (prop == 'VT23') {

                                var contDia = cont / 1440; obj.data.push([contDia, item[prop]])
                                cont += 15;
                            }
                            if (prop == 'VT24') {

                                var contDia = cont / 1440; obj.data.push([contDia, item[prop]])
                                cont += 15;
                            }
                            if (prop == 'VT25') {

                                var contDia = cont / 1440; obj.data.push([contDia, item[prop]])
                                cont += 15;
                            }
                            if (prop == 'VT26') {

                                var contDia = cont / 1440; obj.data.push([contDia, item[prop]])
                                cont += 15;
                            }
                            if (prop == 'VT27') {

                                var contDia = cont / 1440; obj.data.push([contDia, item[prop]])
                                cont += 15;
                            }
                            if (prop == 'VT28') {

                                var contDia = cont / 1440; obj.data.push([contDia, item[prop]])
                                cont += 15;
                            }
                            if (prop == 'VT29') {

                                var contDia = cont / 1440; obj.data.push([contDia, item[prop]])
                                cont += 15;
                            }
                            if (prop == 'VT30') {

                                var contDia = cont / 1440; obj.data.push([contDia, item[prop]])
                                cont += 15;
                            }
                            if (prop == 'VT31') {

                                var contDia = cont / 1440; obj.data.push([contDia, item[prop]])
                                cont += 15;
                            }
                            if (prop == 'VT32') {

                                var contDia = cont / 1440; obj.data.push([contDia, item[prop]])
                                cont += 15;
                            }
                            if (prop == 'VT33') {

                                var contDia = cont / 1440; obj.data.push([contDia, item[prop]])
                                cont += 15;
                            }
                            if (prop == 'VT34') {

                                var contDia = cont / 1440; obj.data.push([contDia, item[prop]])
                                cont += 15;
                            }
                            if (prop == 'VT35') {

                                var contDia = cont / 1440; obj.data.push([contDia, item[prop]])
                                cont += 15;
                            }
                            if (prop == 'VT36') {

                                var contDia = cont / 1440; obj.data.push([contDia, item[prop]])
                                cont += 15;
                            }
                            if (prop == 'VT37') {

                                var contDia = cont / 1440; obj.data.push([contDia, item[prop]])
                                cont += 15;
                            }
                            if (prop == 'VT38') {

                                var contDia = cont / 1440; obj.data.push([contDia, item[prop]])
                                cont += 15;
                            }
                            if (prop == 'VT39') {

                                var contDia = cont / 1440; obj.data.push([contDia, item[prop]])
                                cont += 15;
                            }
                            if (prop == 'VT40') {

                                var contDia = cont / 1440; obj.data.push([contDia, item[prop]])
                                cont += 15;
                            }
                            if (prop == 'VT41') {

                                var contDia = cont / 1440; obj.data.push([contDia, item[prop]])
                                cont += 15;
                            }
                            if (prop == 'VT42') {

                                var contDia = cont / 1440; obj.data.push([contDia, item[prop]])
                                cont += 15;
                            }
                            if (prop == 'VT43') {

                                var contDia = cont / 1440; obj.data.push([contDia, item[prop]])
                                cont += 15;
                            }
                            if (prop == 'VT44') {

                                var contDia = cont / 1440; obj.data.push([contDia, item[prop]])
                                cont += 15;
                            }
                            if (prop == 'VT45') {

                                var contDia = cont / 1440; obj.data.push([contDia, item[prop]])
                                cont += 15;
                            }
                            if (prop == 'VT46') {

                                var contDia = cont / 1440; obj.data.push([contDia, item[prop]])
                                cont += 15;
                            }
                            if (prop == 'VT47') {

                                var contDia = cont / 1440; obj.data.push([contDia, item[prop]])
                                cont += 15;
                            }
                            if (prop == 'VT48') {

                                var contDia = cont / 1440; obj.data.push([contDia, item[prop]])
                                cont += 15;
                            }
                            if (prop == 'VT49') {

                                var contDia = cont / 1440; obj.data.push([contDia, item[prop]])
                                cont += 15;
                            }
                            if (prop == 'VT50') {

                                var contDia = cont / 1440; obj.data.push([contDia, item[prop]])
                                cont += 15;
                            }
                            if (prop == 'VT51') {

                                var contDia = cont / 1440; obj.data.push([contDia, item[prop]])
                                cont += 15;
                            }
                            if (prop == 'VT52') {

                                var contDia = cont / 1440; obj.data.push([contDia, item[prop]])
                                cont += 15;
                            }
                            if (prop == 'VT53') {

                                var contDia = cont / 1440; obj.data.push([contDia, item[prop]])
                                cont += 15;
                            }
                            if (prop == 'VT54') {

                                var contDia = cont / 1440; obj.data.push([contDia, item[prop]])
                                cont += 15;
                            }
                            if (prop == 'VT55') {

                                var contDia = cont / 1440; obj.data.push([contDia, item[prop]])
                                cont += 15;
                            }
                            if (prop == 'VT56') {

                                var contDia = cont / 1440; obj.data.push([contDia, item[prop]])
                                cont += 15;
                            }
                            if (prop == 'VT57') {

                                var contDia = cont / 1440; obj.data.push([contDia, item[prop]])
                                cont += 15;
                            }
                            if (prop == 'VT58') {

                                var contDia = cont / 1440; obj.data.push([contDia, item[prop]])
                                cont += 15;
                            }
                            if (prop == 'VT59') {

                                var contDia = cont / 1440; obj.data.push([contDia, item[prop]])
                                cont += 15;
                            }
                            if (prop == 'VT60') {

                                var contDia = cont / 1440; obj.data.push([contDia, item[prop]])
                                cont += 15;
                            }
                            if (prop == 'VT61') {

                                var contDia = cont / 1440; obj.data.push([contDia, item[prop]])
                                cont += 15;
                            }
                            if (prop == 'VT62') {

                                var contDia = cont / 1440; obj.data.push([contDia, item[prop]])
                                cont += 15;
                            }
                            if (prop == 'VT63') {

                                var contDia = cont / 1440; obj.data.push([contDia, item[prop]])
                                cont += 15;
                            }
                            if (prop == 'VT64') {

                                var contDia = cont / 1440; obj.data.push([contDia, item[prop]])
                                cont += 15;
                            }
                            if (prop == 'VT65') {

                                var contDia = cont / 1440; obj.data.push([contDia, item[prop]])
                                cont += 15;
                            }
                            if (prop == 'VT66') {

                                var contDia = cont / 1440; obj.data.push([contDia, item[prop]])
                                cont += 15;
                            }
                            if (prop == 'VT67') {

                                var contDia = cont / 1440; obj.data.push([contDia, item[prop]])
                                cont += 15;
                            }
                            if (prop == 'VT68') {

                                var contDia = cont / 1440; obj.data.push([contDia, item[prop]])
                                cont += 15;
                            }
                            if (prop == 'VT69') {

                                var contDia = cont / 1440; obj.data.push([contDia, item[prop]])
                                cont += 15;
                            }
                            if (prop == 'VT70') {

                                var contDia = cont / 1440; obj.data.push([contDia, item[prop]])
                                cont += 15;
                            }
                            if (prop == 'VT71') {

                                var contDia = cont / 1440; obj.data.push([contDia, item[prop]])
                                cont += 15;
                            }
                            if (prop == 'VT72') {

                                var contDia = cont / 1440; obj.data.push([contDia, item[prop]])
                                cont += 15;
                            }
                            if (prop == 'VT73') {

                                var contDia = cont / 1440; obj.data.push([contDia, item[prop]])
                                cont += 15;
                            }
                            if (prop == 'VT74') {

                                var contDia = cont / 1440; obj.data.push([contDia, item[prop]])
                                cont += 15;
                            }
                            if (prop == 'VT75') {

                                var contDia = cont / 1440; obj.data.push([contDia, item[prop]])
                                cont += 15;
                            }
                            if (prop == 'VT76') {

                                var contDia = cont / 1440; obj.data.push([contDia, item[prop]])
                                cont += 15;
                            }
                            if (prop == 'VT77') {

                                var contDia = cont / 1440; obj.data.push([contDia, item[prop]])
                                cont += 15;
                            }
                            if (prop == 'VT78') {

                                var contDia = cont / 1440; obj.data.push([contDia, item[prop]])
                                cont += 15;
                            }
                            if (prop == 'VT79') {

                                var contDia = cont / 1440; obj.data.push([contDia, item[prop]])
                                cont += 15;
                            }
                            if (prop == 'VT80') {

                                var contDia = cont / 1440; obj.data.push([contDia, item[prop]])
                                cont += 15;
                            }
                            if (prop == 'VT81') {

                                var contDia = cont / 1440; obj.data.push([contDia, item[prop]])
                                cont += 15;
                            }
                            if (prop == 'VT82') {

                                var contDia = cont / 1440; obj.data.push([contDia, item[prop]])
                                cont += 15;
                            }
                            if (prop == 'VT83') {

                                var contDia = cont / 1440; obj.data.push([contDia, item[prop]])
                                cont += 15;
                            }
                            if (prop == 'VT84') {

                                var contDia = cont / 1440; obj.data.push([contDia, item[prop]])
                                cont += 15;
                            }
                            if (prop == 'VT85') {

                                var contDia = cont / 1440; obj.data.push([contDia, item[prop]])
                                cont += 15;
                            }
                            if (prop == 'VT86') {

                                var contDia = cont / 1440; obj.data.push([contDia, item[prop]])
                                cont += 15;
                            }
                            if (prop == 'VT87') {

                                var contDia = cont / 1440; obj.data.push([contDia, item[prop]])
                                cont += 15;
                            }
                            if (prop == 'VT88') {

                                var contDia = cont / 1440; obj.data.push([contDia, item[prop]])
                                cont += 15;
                            }
                            if (prop == 'VT89') {

                                var contDia = cont / 1440; obj.data.push([contDia, item[prop]])
                                cont += 15;
                            }
                            if (prop == 'VT90') {

                                var contDia = cont / 1440; obj.data.push([contDia, item[prop]])
                                cont += 15;
                            }
                            if (prop == 'VT91') {

                                var contDia = cont / 1440; obj.data.push([contDia, item[prop]])
                                cont += 15;
                            }
                            if (prop == 'VT92') {

                                var contDia = cont / 1440; obj.data.push([contDia, item[prop]])
                                cont += 15;
                            }
                            if (prop == 'VT93') {

                                var contDia = cont / 1440; obj.data.push([contDia, item[prop]])
                                cont += 15;
                            }
                            if (prop == 'VT94') {

                                var contDia = cont / 1440; obj.data.push([contDia, item[prop]])
                                cont += 15;
                            }
                            if (prop == 'VT95') {

                                var contDia = cont / 1440; obj.data.push([contDia, item[prop]])
                                cont += 15;
                            }
                            if (prop == 'VT96') {

                                var contDia = cont / 1440; obj.data.push([contDia, item[prop]])
                                cont += 15;
                            }

                        }
                    }
                });

                lista.push(obj);

            });
        } else {

            var obj = {
                name: codigos,
                data: []
            };

            $.each(dataER, function (j, item) {

                for (var prop in item) {

                    if (prop == 'VT1') {
                        var contDia = cont / 1440; obj.data.push([contDia, item[prop]]);
                        cont += 15;
                    }
                    if (prop == 'VT2') {

                        var contDia = cont / 1440; obj.data.push([contDia, item[prop]]);
                        cont += 15;

                    }
                    if (prop == 'VT3') {

                        var contDia = cont / 1440; obj.data.push([contDia, item[prop]]);
                        cont += 15;

                    }
                    if (prop == 'VT4') {
                        var contDia = cont / 1440; obj.data.push([contDia, item[prop]]);
                        cont += 15;
                    }
                    if (prop == 'VT5') {
                        var contDia = cont / 1440; obj.data.push([contDia, item[prop]]);
                        cont += 15;
                    }
                    if (prop == 'VT6') {
                        var contDia = cont / 1440; obj.data.push([contDia, item[prop]]);
                        cont += 15;
                    }
                    if (prop == 'VT7') {
                        var contDia = cont / 1440; obj.data.push([contDia, item[prop]]);
                        cont += 15;
                    }
                    if (prop == 'VT8') {
                        var contDia = cont / 1440; obj.data.push([contDia, item[prop]]);
                        cont += 15;
                    }
                    if (prop == 'VT9') {
                        var contDia = cont / 1440; obj.data.push([contDia, item[prop]]);
                        cont += 15;
                    }

                    if (prop == 'VT10') {
                        var contDia = cont / 1440; obj.data.push([contDia, item[prop]]);
                        cont += 15;
                    }
                    if (prop == 'VT11') {

                        var contDia = cont / 1440; obj.data.push([contDia, item[prop]])
                        cont += 15;
                    }
                    if (prop == 'VT12') {

                        var contDia = cont / 1440; obj.data.push([contDia, item[prop]])
                        cont += 15;
                    }
                    if (prop == 'VT13') {

                        var contDia = cont / 1440; obj.data.push([contDia, item[prop]])
                        cont += 15;
                    }
                    if (prop == 'VT14') {

                        var contDia = cont / 1440; obj.data.push([contDia, item[prop]])
                        cont += 15;
                    }
                    if (prop == 'VT15') {

                        var contDia = cont / 1440; obj.data.push([contDia, item[prop]])
                        cont += 15;
                    }
                    if (prop == 'VT16') {

                        var contDia = cont / 1440; obj.data.push([contDia, item[prop]])
                        cont += 15;
                    }
                    if (prop == 'VT17') {

                        var contDia = cont / 1440; obj.data.push([contDia, item[prop]])
                        cont += 15;
                    }
                    if (prop == 'VT18') {

                        var contDia = cont / 1440; obj.data.push([contDia, item[prop]])
                        cont += 15;
                    }
                    if (prop == 'VT19') {

                        var contDia = cont / 1440; obj.data.push([contDia, item[prop]])
                        cont += 15;
                    }
                    if (prop == 'VT20') {

                        var contDia = cont / 1440; obj.data.push([contDia, item[prop]])
                        cont += 15;
                    }
                    if (prop == 'VT21') {

                        var contDia = cont / 1440; obj.data.push([contDia, item[prop]])
                        cont += 15;
                    }
                    if (prop == 'VT22') {

                        var contDia = cont / 1440; obj.data.push([contDia, item[prop]])
                        cont += 15;
                    }
                    if (prop == 'VT23') {

                        var contDia = cont / 1440; obj.data.push([contDia, item[prop]])
                        cont += 15;
                    }
                    if (prop == 'VT24') {

                        var contDia = cont / 1440; obj.data.push([contDia, item[prop]])
                        cont += 15;
                    }
                    if (prop == 'VT25') {

                        var contDia = cont / 1440; obj.data.push([contDia, item[prop]])
                        cont += 15;
                    }
                    if (prop == 'VT26') {

                        var contDia = cont / 1440; obj.data.push([contDia, item[prop]])
                        cont += 15;
                    }
                    if (prop == 'VT27') {

                        var contDia = cont / 1440; obj.data.push([contDia, item[prop]])
                        cont += 15;
                    }
                    if (prop == 'VT28') {

                        var contDia = cont / 1440; obj.data.push([contDia, item[prop]])
                        cont += 15;
                    }
                    if (prop == 'VT29') {

                        var contDia = cont / 1440; obj.data.push([contDia, item[prop]])
                        cont += 15;
                    }
                    if (prop == 'VT30') {

                        var contDia = cont / 1440; obj.data.push([contDia, item[prop]])
                        cont += 15;
                    }
                    if (prop == 'VT31') {

                        var contDia = cont / 1440; obj.data.push([contDia, item[prop]])
                        cont += 15;
                    }
                    if (prop == 'VT32') {

                        var contDia = cont / 1440; obj.data.push([contDia, item[prop]])
                        cont += 15;
                    }
                    if (prop == 'VT33') {

                        var contDia = cont / 1440; obj.data.push([contDia, item[prop]])
                        cont += 15;
                    }
                    if (prop == 'VT34') {

                        var contDia = cont / 1440; obj.data.push([contDia, item[prop]])
                        cont += 15;
                    }
                    if (prop == 'VT35') {

                        var contDia = cont / 1440; obj.data.push([contDia, item[prop]])
                        cont += 15;
                    }
                    if (prop == 'VT36') {

                        var contDia = cont / 1440; obj.data.push([contDia, item[prop]])
                        cont += 15;
                    }
                    if (prop == 'VT37') {

                        var contDia = cont / 1440; obj.data.push([contDia, item[prop]])
                        cont += 15;
                    }
                    if (prop == 'VT38') {

                        var contDia = cont / 1440; obj.data.push([contDia, item[prop]])
                        cont += 15;
                    }
                    if (prop == 'VT39') {

                        var contDia = cont / 1440; obj.data.push([contDia, item[prop]])
                        cont += 15;
                    }
                    if (prop == 'VT40') {

                        var contDia = cont / 1440; obj.data.push([contDia, item[prop]])
                        cont += 15;
                    }
                    if (prop == 'VT41') {

                        var contDia = cont / 1440; obj.data.push([contDia, item[prop]])
                        cont += 15;
                    }
                    if (prop == 'VT42') {

                        var contDia = cont / 1440; obj.data.push([contDia, item[prop]])
                        cont += 15;
                    }
                    if (prop == 'VT43') {

                        var contDia = cont / 1440; obj.data.push([contDia, item[prop]])
                        cont += 15;
                    }
                    if (prop == 'VT44') {

                        var contDia = cont / 1440; obj.data.push([contDia, item[prop]])
                        cont += 15;
                    }
                    if (prop == 'VT45') {

                        var contDia = cont / 1440; obj.data.push([contDia, item[prop]])
                        cont += 15;
                    }
                    if (prop == 'VT46') {

                        var contDia = cont / 1440; obj.data.push([contDia, item[prop]])
                        cont += 15;
                    }
                    if (prop == 'VT47') {

                        var contDia = cont / 1440; obj.data.push([contDia, item[prop]])
                        cont += 15;
                    }
                    if (prop == 'VT48') {

                        var contDia = cont / 1440; obj.data.push([contDia, item[prop]])
                        cont += 15;
                    }
                    if (prop == 'VT49') {

                        var contDia = cont / 1440; obj.data.push([contDia, item[prop]])
                        cont += 15;
                    }
                    if (prop == 'VT50') {

                        var contDia = cont / 1440; obj.data.push([contDia, item[prop]])
                        cont += 15;
                    }
                    if (prop == 'VT51') {

                        var contDia = cont / 1440; obj.data.push([contDia, item[prop]])
                        cont += 15;
                    }
                    if (prop == 'VT52') {

                        var contDia = cont / 1440; obj.data.push([contDia, item[prop]])
                        cont += 15;
                    }
                    if (prop == 'VT53') {

                        var contDia = cont / 1440; obj.data.push([contDia, item[prop]])
                        cont += 15;
                    }
                    if (prop == 'VT54') {

                        var contDia = cont / 1440; obj.data.push([contDia, item[prop]])
                        cont += 15;
                    }
                    if (prop == 'VT55') {

                        var contDia = cont / 1440; obj.data.push([contDia, item[prop]])
                        cont += 15;
                    }
                    if (prop == 'VT56') {

                        var contDia = cont / 1440; obj.data.push([contDia, item[prop]])
                        cont += 15;
                    }
                    if (prop == 'VT57') {

                        var contDia = cont / 1440; obj.data.push([contDia, item[prop]])
                        cont += 15;
                    }
                    if (prop == 'VT58') {

                        var contDia = cont / 1440; obj.data.push([contDia, item[prop]])
                        cont += 15;
                    }
                    if (prop == 'VT59') {

                        var contDia = cont / 1440; obj.data.push([contDia, item[prop]])
                        cont += 15;
                    }
                    if (prop == 'VT60') {

                        var contDia = cont / 1440; obj.data.push([contDia, item[prop]])
                        cont += 15;
                    }
                    if (prop == 'VT61') {

                        var contDia = cont / 1440; obj.data.push([contDia, item[prop]])
                        cont += 15;
                    }
                    if (prop == 'VT62') {

                        var contDia = cont / 1440; obj.data.push([contDia, item[prop]])
                        cont += 15;
                    }
                    if (prop == 'VT63') {

                        var contDia = cont / 1440; obj.data.push([contDia, item[prop]])
                        cont += 15;
                    }
                    if (prop == 'VT64') {

                        var contDia = cont / 1440; obj.data.push([contDia, item[prop]])
                        cont += 15;
                    }
                    if (prop == 'VT65') {

                        var contDia = cont / 1440; obj.data.push([contDia, item[prop]])
                        cont += 15;
                    }
                    if (prop == 'VT66') {

                        var contDia = cont / 1440; obj.data.push([contDia, item[prop]])
                        cont += 15;
                    }
                    if (prop == 'VT67') {

                        var contDia = cont / 1440; obj.data.push([contDia, item[prop]])
                        cont += 15;
                    }
                    if (prop == 'VT68') {

                        var contDia = cont / 1440; obj.data.push([contDia, item[prop]])
                        cont += 15;
                    }
                    if (prop == 'VT69') {

                        var contDia = cont / 1440; obj.data.push([contDia, item[prop]])
                        cont += 15;
                    }
                    if (prop == 'VT70') {

                        var contDia = cont / 1440; obj.data.push([contDia, item[prop]])
                        cont += 15;
                    }
                    if (prop == 'VT71') {

                        var contDia = cont / 1440; obj.data.push([contDia, item[prop]])
                        cont += 15;
                    }
                    if (prop == 'VT72') {

                        var contDia = cont / 1440; obj.data.push([contDia, item[prop]])
                        cont += 15;
                    }
                    if (prop == 'VT73') {

                        var contDia = cont / 1440; obj.data.push([contDia, item[prop]])
                        cont += 15;
                    }
                    if (prop == 'VT74') {

                        var contDia = cont / 1440; obj.data.push([contDia, item[prop]])
                        cont += 15;
                    }
                    if (prop == 'VT75') {

                        var contDia = cont / 1440; obj.data.push([contDia, item[prop]])
                        cont += 15;
                    }
                    if (prop == 'VT76') {

                        var contDia = cont / 1440; obj.data.push([contDia, item[prop]])
                        cont += 15;
                    }
                    if (prop == 'VT77') {

                        var contDia = cont / 1440; obj.data.push([contDia, item[prop]])
                        cont += 15;
                    }
                    if (prop == 'VT78') {

                        var contDia = cont / 1440; obj.data.push([contDia, item[prop]])
                        cont += 15;
                    }
                    if (prop == 'VT79') {

                        var contDia = cont / 1440; obj.data.push([contDia, item[prop]])
                        cont += 15;
                    }
                    if (prop == 'VT80') {

                        var contDia = cont / 1440; obj.data.push([contDia, item[prop]])
                        cont += 15;
                    }
                    if (prop == 'VT81') {

                        var contDia = cont / 1440; obj.data.push([contDia, item[prop]])
                        cont += 15;
                    }
                    if (prop == 'VT82') {

                        var contDia = cont / 1440; obj.data.push([contDia, item[prop]])
                        cont += 15;
                    }
                    if (prop == 'VT83') {

                        var contDia = cont / 1440; obj.data.push([contDia, item[prop]])
                        cont += 15;
                    }
                    if (prop == 'VT84') {

                        var contDia = cont / 1440; obj.data.push([contDia, item[prop]])
                        cont += 15;
                    }
                    if (prop == 'VT85') {

                        var contDia = cont / 1440; obj.data.push([contDia, item[prop]])
                        cont += 15;
                    }
                    if (prop == 'VT86') {

                        var contDia = cont / 1440; obj.data.push([contDia, item[prop]])
                        cont += 15;
                    }
                    if (prop == 'VT87') {

                        var contDia = cont / 1440; obj.data.push([contDia, item[prop]])
                        cont += 15;
                    }
                    if (prop == 'VT88') {

                        var contDia = cont / 1440; obj.data.push([contDia, item[prop]])
                        cont += 15;
                    }
                    if (prop == 'VT89') {

                        var contDia = cont / 1440; obj.data.push([contDia, item[prop]])
                        cont += 15;
                    }
                    if (prop == 'VT90') {

                        var contDia = cont / 1440; obj.data.push([contDia, item[prop]])
                        cont += 15;
                    }
                    if (prop == 'VT91') {

                        var contDia = cont / 1440; obj.data.push([contDia, item[prop]])
                        cont += 15;
                    }
                    if (prop == 'VT92') {

                        var contDia = cont / 1440; obj.data.push([contDia, item[prop]])
                        cont += 15;
                    }
                    if (prop == 'VT93') {

                        var contDia = cont / 1440; obj.data.push([contDia, item[prop]])
                        cont += 15;
                    }
                    if (prop == 'VT94') {

                        var contDia = cont / 1440; obj.data.push([contDia, item[prop]])
                        cont += 15;
                    }
                    if (prop == 'VT95') {

                        var contDia = cont / 1440; obj.data.push([contDia, item[prop]])
                        cont += 15;
                    }
                    if (prop == 'VT96') {

                        var contDia = cont / 1440; obj.data.push([contDia, item[prop]])
                        cont += 15;
                    }

                }
            });

            lista.push(obj);
        }

        metodo.chartJS.init(lista);
        return lista;
    }

    metodo.cargarDatosIniciales();
});




