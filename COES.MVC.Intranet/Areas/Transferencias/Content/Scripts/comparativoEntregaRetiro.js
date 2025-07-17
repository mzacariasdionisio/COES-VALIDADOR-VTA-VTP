var controler = siteRoot + "transferencias/ComparativoEntregaRetiro/";

var error = [];
$(document).ready(function () {
    $('#tab-container').easytabs({
        animate: false
    });

    $('.txtFecha').Zebra_DatePicker({
    });

    $('#emprcodi').multipleSelect({
        width: '189px',
        filter: true,
        selectAll: false,
        single: true,
        onClick: function (view) {
            var emprcodi = $("#emprcodi").multipleSelect('getSelects');
            $('#hfComboEmpresa').val(emprcodi);
        },
        onClose: function (view) {
            var emprcodi = $("#emprcodi").multipleSelect('getSelects');
            $('#hfComboEmpresa').val(emprcodi);
        }
    });

    $('#cliemprcodi').multipleSelect({
        width: '189px',
        filter: true,
        selectAll: false,
        single: true,
        onClick: function (view) {
            var cliemprcodi = $("#cliemprcodi").multipleSelect('getSelects');
            $('#hfComboCliente').val(cliemprcodi);
        },
        onClose: function (view) {
            var cliemprcodi = $("#cliemprcodi").multipleSelect('getSelects');
            $('#hfComboCliente').val(cliemprcodi);
        }
    });

    //$('#barrcodi').multipleSelect({
    //    width: '189px',
    //    filter: true,
    //    selectAll: false,
    //    single: true,
    //    onClick: function (view) {
    //        var barrcodi = $("#barrcodi").multipleSelect('getSelects');
    //        $('#hfComboBarra').val(barrcodi);
    //    },
    //    onClose: function (view) {
    //        var barrcodi = $("#barrcodi").multipleSelect('getSelects');
    //        $('#hfComboBarra').val(barrcodi);
    //    }
    //});

    $('#emprcodi1').multipleSelect({
        width: '189px',
        filter: true,
        selectAll: false,
        single: true,
        onClick: function (view) {
            var emprcodi = $("#emprcodi1").multipleSelect('getSelects');
            $('#hfComboEmpresa').val(emprcodi);
        },
        onClose: function (view) {
            var emprcodi = $("#emprcodi1").multipleSelect('getSelects');
            $('#hfComboEmpresa').val(emprcodi);
        }
    });

    $('#cliemprcodi1').multipleSelect({
        width: '189px',
        filter: true,
        selectAll: false,
        single: true,
        onClick: function (view) {
            var cliemprcodi = $("#cliemprcodi1").multipleSelect('getSelects');
            $('#hfComboCliente').val(cliemprcodi);
        },
        onClose: function (view) {
            var cliemprcodi = $("#cliemprcodi1").multipleSelect('getSelects');
            $('#hfComboCliente').val(cliemprcodi);
        }
    });

    $('#barrcodi1').multipleSelect({
        width: '189px',
        filter: true,
        selectAll: false,
        single: true,
        onClick: function (view) {
            var barrcodi = $("#barrcodi1").multipleSelect('getSelects');
            $('#hfComboBarra').val(barrcodi);
        },
        onClose: function (view) {
            var barrcodi = $("#barrcodi1").multipleSelect('getSelects');
            $('#hfComboBarra').val(barrcodi);
        }
    });

    $('#dia').multipleSelect({
        width: '150px',
        filter: true,
        selectAll: true,
        single: false
    });

    $('#codigo').multipleSelect({
        width: '100%',
        filter: true
    });

    $('#btnConsultar').click(function () {
        //mostrarAlerta("Espere un momento, se esta procesando su solicitud");
        buscar();
    });

    $('#btnConsultar2').click(function () {
        //mostrarAlerta("Espere un momento, se esta procesando su solicitud");
        buscar2();
    });

})




cargarComboboxCliente = (param) => {

    var pro = new Promise((resolve, reject) => {

        $.ajax({
            type: 'POST',
            url: controler + "ListaInterCoReSoCliPorEmpresa",
            data: { emprcodi: parseInt(param) },
            success: function (evt) {
                resolve(evt)
            },
            error: function () {

                reject("Lo sentimos, ha ocurrido un error inesperado");
            }
        });
    })
    return pro;
}

cargarBarrasClientes = (tipoInfoCodigo, periCodi, emprCodi, cliCodi, enumEntregaRetiro) => {
    var pro = new Promise((resolve, reject) => {
        $.ajax({
            type: 'POST',
            url: controler + "ListarTodasLasBarras",
            data: {
                tipoInfoCodigo: tipoInfoCodigo,
                periCodi: periCodi,
                emprcodi: emprCodi,
                cliCodi: cliCodi,
                enumComparativoEntregaRetiros: enumEntregaRetiro
            },
            success: function (evt) {
                resolve(evt)
            },
            error: function () {

                reject("Lo sentimos, ha ocurrido un error inesperado");
            }
        });
    })
    return pro;

}

RecargarConsulta = function () {
    var cmbPericodi = document.getElementById('pericodi');
    window.location.href = controler + "index?pericodi=" + cmbPericodi.value + "#paso1";
}

RecargarConsulta2 = function () {
    var cmbPericodi1 = document.getElementById('pericodi1');
    var cmbPericodi2 = document.getElementById('pericodi2');
    var version = document.getElementById('recacodi1');
    window.location.href = controler + "index?pericodi=" + cmbPericodi1.value + "&pericodi2=" + cmbPericodi2.value + "&recacodi=" + version + "#paso2";
}


buscar = function () {
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
        if (cliemprcodi == '' || cliemprcodi == 'TODOS')
            cliemprcodi = null;

        if (trnenvtipinf == '1') {
            if (flag == 'E')
                flag = 'E';
            else if (flag == 'E')
                cliemprcodi = null;
        }

        $.ajax({
            type: 'POST',
            url: controler + "ListaCodigo",
            data: {
                trnenvtipinf: trnenvtipinf, pericodi: pericodi, version: version,
                empcodi: empcodi, cliemprcodi: cliemprcodi, barrcodi: barrcodi, flagEntrReti: flag,
                fechaInicio: $('#txtfechaIni').val() == '' ? '01/01/1900' : $('#txtfechaIni').val(),
                fechaFin: $('#txtfechaFin').val() == '' ? '01/01/2900' : $('#txtfechaFin').val()
            },
            success: function (evt) {
                $('#listado').html(evt);

            },
            error: function () {
                mostrarError();
            }
        });
    }



}

var desviacion = {
    chartJS: {
        init: (id, series) => {

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

            if (document.querySelectorAll('.grafica-detalle').length > 0) {
                document.querySelectorAll('.grafica-detalle').forEach((index, key, target) => {
                    index.remove();
                })
            }

            var chartype = {};
            chartype = {
                zoomType: 'x'
            }

            $chart = Highcharts.chart(id, {
                chart: chartype,
                title: {
                    text: 'Comparativo Entrega y Retiros'
                },
                //subtitle: {
                //    text: document.ontouchstart === undefined ?
                //        'Click and drag in the plot area to zoom in' : 'Pinch the chart to zoom in'
                //},
                xAxis: {
                    type: 'datetime'
                },
                tooltip: {
                    formatter: function () {
                        return Highcharts.dateFormat('%H:%M', new Date(this.x)) + '<br>' +
                            '<b>' + this.series.name + ' : ' + this.y + '</b>';
                    }
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
    },

    parameter: () => {
        var diaInicioArray = [];
        var codigoArray = [];
        var completo = false;
        var error = false;
        var mensaje = "";
        $('#dia').next().find('li.selected input[type="checkbox"]').each((index, target) => {
            if ($(target).val() != "TODOS") {
                diaInicioArray.push($(target).val())
            }
        })
        var bit = $('#codigo').next().find('li input[name="selectItem"][value=""]').is(':checked');

        if (!bit) {
            $('#codigo').next().find('li.selected input[type="checkbox"]').each((index, target) => {
                if ($(target).val() != "TODOS") {
                    codigoArray.push("'" + $(target).val() + "'")
                }
            })
        } else {
            codigoArray = [];
            completo = true;
        }



        if (diaInicioArray.length == 0) {
            mensaje += "Es necesario seleccionar el dia. \n"
            error = true;
        }
        if (codigoArray.length == 0 && completo == false) {
            mensaje += "Es necesario seleccionar un codigo. \n"
            error = true;
        }

        if (error) {
            alert(mensaje);
            return false;

        }


        var Periodo1 = parseInt($('#pericodi1').val());
        var Recalculo1 = parseInt($('#recacodi1').val());
        var Periodo2 = parseInt($('#pericodi2').val());
        var Recalculo2 = parseInt($('#recacodi2').val());

        //if (Recalculo1 > Recalculo2) {
        //    Recalculo2 = Recalculo1;
        //}
        //if (parseInt($('#recacodi2').val()) < Recalculo1) {
        //    Recalculo1 = parseInt($('#recacodi2').val());
        //}
        return {
            TipoInfoCodi: $('#trnenvtipinf1').val(),
            "periCodi1": $('#pericodi1').val(),
            "versionCodi1": Recalculo1,
            "periCodi2": $('#pericodi2').val(),
            "versionCodi2": Recalculo2,
            "diaArray": diaInicioArray,
            "emprCodi": $('#emprcodi1 option:selected').val() == '' ? null : $('#emprcodi1 option:selected').val(),
            "cliCodi": $('#cliemprcodi1 option:selected').val() == '' ? null : $('#cliemprcodi1 option:selected').val(),
            "barrCodi": $('#barrcodi1 option:selected').val() == '' ? null : $('#barrcodi1 option:selected').val(),
            "tipoEntregaCodi": $('#flag1').val() == '' ? null : $('#flag1').val(),
            "codigoRetiroArray": codigoArray
        }
    },
    dibujarTablaAdicional: () => {
        var promise = new Promise((response, reject) => {
            var param = desviacion.parameter();
            if (param !== false) {
                $.ajax({
                    type: 'POST',
                    contentType: 'application/json; charset=utf-8',
                    url: controler + "ListarCostoMarginalDesviacion",
                    data: JSON.stringify(param),
                    success: function (result) {
                        var data = result.Data;
                        var table = $('#tblDesviacion').find('tbody');
                        table.html('')
                        data.map((item, index, array) => {
                            var tr = $('<tr></tr>');
                            tr.append(`<td>${item.Dia}</td>`);
                            tr.append(`<td>${item.Hora}</td>`);
                            tr.append(`<td class="cmg1">${item.EntregaRetiro1}</td>`);
                            tr.append(`<td class="cmg2">${item.EntregaRetiro2}</td>`);
                            tr.append(`<td class="desviacion">${item.Desviacion}</td>`);
                            tr.append(`<td class="fecha" style="display:none">${item.FechaIntervalo}</td>`);
                            table.append(tr);
                        })
                        response(data);

                    },
                    error: function () {
                        reject();
                    }
                });

            } else {

                reject();
            }



        })
        return promise;
    },
    dibujarGraficaLineal: () => {

        this.desviacion.ajaxGraficoLineal((data) => {
            arrayGrafica = [];
            data.map((item, index, array) => {
                let grafico = {
                    type: 'area',
                    name: item.PeriodoMesVersion,
                    data: []
                };
                item.ListaComparativos.map((itemParent, index, array) => {
                    grafico.data.push([
                        parseInt(itemParent.FechaIntervalo.replace("/Date(", "").replace(")/", ""), 10),
                        itemParent.valorInicial
                    ])
                })
                arrayGrafica.push(grafico);
            })
            desviacion.chartJS.init('graficoEntregaRetiro_1', arrayGrafica);
        })

    },

    dibujarGraficaDesviacion: () => {
        let barra = {
            "type": "column",
            name: "% VARIACIÓN",
            data: []
        };
        $('#tblDesviacion > tbody > tr').each((index, target) => {
            var tr = $(target);
            barra.data.push([
                parseInt(tr.find('.fecha').html().replace("/Date(", "").replace(")/", ""), 10),
                parseFloat(tr.find('.desviacion').html())
            ])
        });
        desviacion.chartJS.init('graficoDesviacion', [barra]);
    },
    ajaxGraficoLineal: (callback) => {


        $.ajax({
            type: 'POST',
            contentType: 'application/json; charset=utf-8',
            url: controler + "ListarComparativoEntregaRetiroValorDET",
            data: JSON.stringify(desviacion.parameter()),
            success: function (result) {
                var data = result.Data;
                callback(data);
            },
            error: function () {

            }
        });
    }


},

    buscar2 = function () {
        var pericodi = $("#pericodi1 option:selected").val();
        var version = $("#recacodi1 option:selected").val();
        var pericodi2 = $("#pericodi2 option:selected").val();
        var version2 = $("#recacodi2 option:selected").val();
        if (pericodi == "" || version == "") {
            alert('Por favor, seleccione el Periodo y la versión');
        } else if (pericodi2 == "" || version2 == "") {
            alert('Por favor, seleccione el Periodo y la versión');
        }
        else {
            var trnenvtipinf = $("#trnenvtipinf option:selected").val();
            var empcodi = $("#emprcodi1 option:selected").val();
            var cliemprcodi = $("#cliemprcodi1 option:selected").val();
            var barrcodi = $("#barrcodi1 option:selected").val();
            var flag = $("#flag1 option:selected").val();
            if (empcodi == '')
                empcodi = null;
            if (barrcodi == '')
                barrcodi = null;
            if (pericodi == '')
                pericodi = null;
            if (cliemprcodi == '' || cliemprcodi == 'TODOS')
                cliemprcodi = null;
            if (flag == 'T')
                flag = null;


            desviacion.dibujarTablaAdicional().then(() => {
                desviacion.dibujarGraficaLineal();
                desviacion.dibujarGraficaDesviacion();
            }).catch(() => {

            });
        }
    }


cargarCodigos = () => {
    $('#codigo').text('..Cargando')

    $.ajax({
        type: 'POST',
        url: controler + "ListarCodigos",
        data: {
            EmprCodi: $('#emprcodi1').val()
        },
        success: function (result) {
            var data = result;
            var select = $('#codigo');
            select.find('option:not(:first-child)').remove();
            data.map((item, index, array) => {
                var tr = $('<option></option>');
                tr.attr('value', item.ValoTranCodEntRet)
                tr.html(item.ValoTranCodEntRet)
                select.append(tr);
            })
            select.multipleSelect({
                width: '150px',
                filter: true,
                selectAll: true,
                single: false
            });
            console.log(data)
        },
        error: function () {

        }
    });
}


cargarEmpresas = () => {
    var Periodo1 = parseInt($('#pericodi1').val());
    var Periodo2 = parseInt($('#pericodi2').val());

    if (Periodo1 > Periodo2) {
        Periodo2 = Periodo1;
    }
    if (parseInt($('#pericodi2').val()) < Periodo1) {
        Periodo1 = parseInt($('#pericodi2').val());
    }
    $.ajax({
        type: 'POST',
        url: controler + "ListarEmpresasAsociadas",
        data: {
            PeriCodi1: Periodo1,
            PeriCodi2: Periodo2
        },
        success: function (result) {
            var data = result;
            var select = $('#emprcodi1');
            select.find('option:not(:first-child)').remove();
            data.map((item, index, array) => {
                var tr = $('<option></option>');
                tr.attr('value', item.EmprCodi)
                tr.html(item.EmprNombre)
                select.append(tr);
            })
            select.multipleSelect({
                width: '150px',
                filter: true,
                selectAll: true,
                single: false
            });
            console.log(data)
        },
        error: function () {

        }
    });
}


$(document).ready(() => {
    $('.cliente').hide();
    $('.cliente1').hide();
    $('.CMG-child[data-for-id="DSV"]').hide();
    $('.CMG').click((target) => {
        $('.CMG').removeClass('active')
        $(target.currentTarget).addClass('active')
        var dataId = $(target.currentTarget).attr('data-id');

        $('.CMG-child').hide();
        $('.CMG-child[data-for-id="' + dataId + '"]').show();

        if (dataId == 'CMP') {
        } else if (dataId == 'DSV') {
        }
    })


    $('#trnenvtipinf').change((event) => {
        fillBarraClear('barrcodi');
        fillaClientesClear('cliemprcodi');
        $('#emprcodi').trigger('change')
    })

    $('#trnenvtipinf1').change((event) => {
        fillBarraClear('barrcodi1');
        fillaClientesClear('cliemprcodi1');
        $('#emprcodi1').trigger('change')
    })


    $('#emprcodi').change((event) => {
        fillBarraClear('barrcodi');
        fillaClientesClear('cliemprcodi');

        let tipoInfoCodigo = $("#trnenvtipinf option:selected").val();
        let pericodi = $('#pericodi option:selected').val();
        var emprCodi = $("#emprcodi option:selected").val();
        var enumEntregaRetiro = $("#flag option:selected").val();
        if (enumEntregaRetiro == 'R' && tipoInfoCodigo == 1) {
            $('.cliente').show();
            fillaClientes('cliemprcodi', emprCodi)
        } else if (enumEntregaRetiro == 'E' || tipoInfoCodigo == 2) {
            $('.cliente').hide();
            fillBarra('barrcodi', tipoInfoCodigo, pericodi, parseInt(emprCodi), 0, enumEntregaRetiro);
        }
    })



    $('#emprcodi1').change((event) => {
        fillBarraClear('barrcodi1');
        fillaClientesClear('cliemprcodi1');
        let tipoInfoCodigo = $("#trnenvtipinf1 option:selected").val();
        let pericodi = $('#pericodi1 option:selected').val();
        var emprCodi = $("#emprcodi1 option:selected").val();
        var enumEntregaRetiro = $("#flag1 option:selected").val();

        if (enumEntregaRetiro == 'R' && tipoInfoCodigo == 1) {
            $('.cliente1').show();
            fillaClientes('cliemprcodi1', emprCodi)
        } else if (enumEntregaRetiro == 'E' || tipoInfoCodigo == 2) {
            $('.cliente1').hide();
            fillBarra('barrcodi1', tipoInfoCodigo, pericodi, parseInt(emprCodi), 0, enumEntregaRetiro);
        }
    })


    $('#cliemprcodi').change((event) => {
        let tipoInfoCodigo = $("#trnenvtipinf option:selected").val();
        var clicCodi = $("#cliemprcodi option:selected").val();
        var emprCodi = $("#emprcodi option:selected").val();
        var enumEntregaRetiro = $("#flag option:selected").val();
        let pericodi = $('#pericodi option:selected').val();

        fillBarra('barrcodi', tipoInfoCodigo, pericodi, parseInt(emprCodi), clicCodi, enumEntregaRetiro);
    })


    $('#cliemprcodi1').change((event) => {
        let tipoInfoCodigo = $("#trnenvtipinf1 option:selected").val();
        var clicCodi = $("#cliemprcodi1 option:selected").val();
        var emprCodi = $("#emprcodi1 option:selected").val();
        var enumEntregaRetiro = $("#flag1 option:selected").val();
        let pericodi = $('#pericodi1 option:selected').val();
        fillBarra('barrcodi1', tipoInfoCodigo, pericodi, emprCodi, clicCodi, enumEntregaRetiro);
    })


    $('#flag').change(function () {
        let tipoInfoCodigo = $("#trnenvtipinf option:selected").val();
        let isEntegaRetiro = $('#flag').val();
        let emprCodi = $("#emprcodi option:selected").val();
        let enumEntregaRetiro = $("#flag option:selected").val();
        let pericodi = $('#pericodi option:selected').val();
        let clicCodi = 0;

        if (isEntegaRetiro == 'R' && tipoInfoCodigo == 1) {
            $('.cliente').show();
            fillBarraClear('barrcodi');
            fillaClientes('cliemprcodi', emprCodi);
        } else if (isEntegaRetiro == 'E' || tipoInfoCodigo == 2) {
            $('.cliente').hide();
            fillaClientesClear('cliemprcodi')
            fillBarra('barrcodi', tipoInfoCodigo, pericodi, parseInt(emprCodi), clicCodi, enumEntregaRetiro);
        }
    })

    $('#flag1').change(function () {

        let tipoInfoCodigo = $("#trnenvtipinf1 option:selected").val();
        let isEntegaRetiro = $('#flag1').val();
        let emprCodi = $("#emprcodi1 option:selected").val();
        let enumEntregaRetiro = $("#flag1 option:selected").val();
        let pericodi = $('#pericodi1 option:selected').val();
        let clicCodi = 0;
        if (isEntegaRetiro == 'R' && tipoInfoCodigo == 1) {
            $('.cliente1').show();
            fillBarraClear('barrcodi1');
            fillaClientes('cliemprcodi1', emprCodi);
        } else if (isEntegaRetiro == 'E' || tipoInfoCodigo == 2) {
            $('.cliente1').hide();
            fillaClientesClear('cliemprcodi1')
            fillBarra('barrcodi1', tipoInfoCodigo, pericodi, parseInt(emprCodi), clicCodi, enumEntregaRetiro);
        }
    })

    function fillBarra(paramIdBarra, tipoInfoCodigo, periCodi, emprCodi, cliCodi, enumEntregaRetiro) {
        let idBarrcodi = '#' + paramIdBarra;
        cargarBarrasClientes(tipoInfoCodigo, periCodi, emprCodi, cliCodi, enumEntregaRetiro).then((data) => {
            $(idBarrcodi).find('option:not(:first-child)').remove();
            data.map((item) => {
                $(idBarrcodi).append(`<option value="${item.Value}">${item.Text}</option>`)
            })
            $(idBarrcodi).prop('disabled', false);
            $(idBarrcodi).find('option:first-child').text('--Todos--');
            $(idBarrcodi).multipleSelect({
                width: '189px',
                filter: true,
                selectAll: false,
                single: true,
                onClick: function (view) {
                    var emprcodi = $(idBarrcodi).multipleSelect('getSelects');
                    $(idBarrcodi).val(emprcodi);
                },
                onClose: function (view) {
                    var emprcodi = $(idBarrcodi).multipleSelect('getSelects');
                    $(idBarrcodi).val(emprcodi);
                }
            }
            );
        });
    }

    function fillBarraClear(paramIdBarra) {
        $('#' + paramIdBarra).find('option:not(:first-child)').remove();
        $('#' + paramIdBarra).multipleSelect({
            width: '189px',
            filter: true,
            selectAll: false,
            single: true
        });
    }

    function fillaClientes(idCliente, emprCodi) {
        let id = '#' + idCliente;
        var promise = new Promise((resolve, reject) => {
            cargarComboboxCliente(emprCodi).then((data) => {
                $(id).find('option:not(:first-child)').remove();
                data.map((item) => {
                    $(id).append(`<option value="${item.Value}">${item.Text}</option>`)
                })
                $(id).prop('disabled', false);
                $(id).find('option:first-child').text('TODOS');
                $(id).multipleSelect({
                    width: '189px',
                    filter: true,
                    selectAll: false,
                    single: true,
                    onClick: function (view) {
                        var emprcodi = $("#cliemprcodi1").multipleSelect('getSelects');
                        $(id).val(emprcodi);
                    },
                    onClose: function (view) {
                        var emprcodi = $("#cliemprcodi1").multipleSelect('getSelects');
                        $(id).val(emprcodi);
                    }
                });
                resolve();
            }).catch(() => {
                reject();
            });
        })
        return promise;
    }

    function fillaClientesClear(paramIdCliente) {

        $('#' + paramIdCliente).find('option:not(:first-child)').remove();
        $('#' + paramIdCliente).multipleSelect({
            width: '189px',
            filter: true,
            selectAll: false,
            single: true,
            onClick: function (view) {
                var emprcodi = $(idBarrcodi).multipleSelect('getSelects');
                $(idBarrcodi).val(emprcodi);
            },
            onClose: function (view) {
                var emprcodi = $(idBarrcodi).multipleSelect('getSelects');
                $(idBarrcodi).val(emprcodi);
            }
        })
    }
})