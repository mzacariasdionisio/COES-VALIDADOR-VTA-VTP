
var controler = siteRoot + "transferencias/ReportCostoMarginales/";
(function () {

    var $chart = null;
    var arrayGrafica = [];
    var $busquedaBarras = [];
    var $zebraInicio = null;
    var $zebraFin = null;
    var ctrl = {
        tblBarraCostoMarginal: '#tblBarra_costoMarginal',
        tblBarraPromedioMarginal: '#tblBarra_promedioMarginal',
        txtInicio1: '#paso1 #txtInicio',
        txtFin1: '#paso1 #txtFin',
        paso1: '#paso1',
        cboPeriodo1: '#paso1 #cbPeriodo',
        cboVersion1: '#paso1 #cboVersion',
        btnConsultar1: '#paso1 #btnConsultar1',
        tabContainer: '#tab-container',
        paso2: '#paso2',
        tblAdicional2: '#paso2 #tblAdicional',
        cboTipoCostoMarginal2: '#paso2 #cboTipoCostoMarginal',
        btnConsultar2: '#paso2 #btnConsultar2',
        cbPeriodoInicio2: '#paso2 .cboPeriodoInicio > select',
        cboVersionInicio2: '#paso2 #cboVersion1',
        cboDiaInicioInicio2: '#paso2 #cboDiaInicio_paso2',
        cbPeriodoFin2: '#paso2 .cboPeriodoFin > select',
        cboVersionFin2: '#paso2 #cboVersion2',
        cboDiaFin2: '#paso2 #cboFin_paso2',
        cboBarra2: '#paso2 #cboBarra',

        paso3: '#paso3',
        btnConsultar3: '#paso3 #btnConsultar3',
        cbPeriodoInicio3: '#paso3 .cboPeriodoInicio > select',
        cbPeriodoFin3: '#paso3 .cboPeriodoFin > select',
        txtFechaInicio3: '#paso3 #txtFechaInicio',
        txtFechaFin3: '#paso3 #txtFechaFin',
        cboTipoReporte3: '#paso3 #cboTipoReporte',
        contenedor3: '#contenedor3'
    }
    var ajax = {

        ObtenerPeriodoPorId: (periCodi) => {
            var promise = new Promise((resolve, reject) => {
                $.ajax({
                    type: 'POST',
                    url: controler + "ObtenerPeriodoPorId",
                    data: { periCodi: periCodi },
                    success: function (data) {
                        resolve(data);
                    },
                    error: function () {
                        reject("Lo sentimos, ha ocurrido un error inesperado");
                    }
                });
            })
            return promise;
        },
        BarrasMarginales: (parametro) => {
            var promise = new Promise((resolve, reject) => {
                $.ajax({
                    type: 'POST',
                    url: controler + "BarrasMarginales",
                    contentType: 'application/json; charset=utf-8',
                    data: JSON.stringify(parametro),
                    success: function (evt) {
                        if (evt.EsCorrecto < 0) {
                            reject(evt.Mensaje)
                        } else {
                            resolve(evt.Data)
                        }
                    },
                    error: function () {
                        reject("Lo sentimos, ha ocurrido un error inesperado");
                    }
                });

            })
            return promise;
        }, BarrasMarginalesDiarioMensual: (parametro) => {
            var promise = new Promise((resolve, reject) => {
                $.ajax({
                    type: 'POST',
                    url: controler + "BarrasMarginalesDiarioMensual",
                    contentType: 'application/json; charset=utf-8',
                    data: JSON.stringify(parametro),
                    success: function (evt) {
                        if (evt.EsCorrecto < 0) {
                            reject(evt.Mensaje)
                        } else {
                            resolve(evt.Data)
                        }
                    },
                    error: function () {
                        reject("Lo sentimos, ha ocurrido un error inesperado");
                    }
                });

            })
            return promise;
        },
        ConsultaCostosMarginales: (tipoCostoMarginal, parametro) => {

            parametro.TipoCostoMarginal = tipoCostoMarginal;
            var promise = new Promise((resolve, reject) => {
                $.ajax({
                    type: 'POST',
                    contentType: 'application/json; charset=utf-8',
                    url: controler + "ConsultaCostosMarginales",
                    data: JSON.stringify(parametro),
                    success: function (evt) {
                        if (evt.EsCorrecto < 0) {
                            reject(evt.Mensaje)
                        } else {
                            resolve(evt.Data)
                        }
                    },
                    error: function () {
                        reject("Lo sentimos, ha ocurrido un error inesperado");
                    }
                });

            })
            return promise;
        },
        ListarCostoMarginalDesviacion: (tipoCostoMarginal) => {

            var error = false;
            var mensaje = "";

            var diaInicioArray = [];
            $(ctrl.cboDiaInicioInicio2).next().find('li.selected input[type="radio"]').each((index, target) => {
                if ($(target).val() != "--TODOS--") {
                    diaInicioArray.push($(target).val())
                }
            })
            var dia2Array = [];
            //$(ctrl.cboDiaFin2).next().find('li.selected input[type="radio"]').each((index, target) => {
            //    if ($(target).val() != "--TODOS--") {
            //        dia2Array.push($(target).val())
            //    }
            //})


            var BarrasArray = [];

            if ($(ctrl.cboBarra2).val() != '') {
                BarrasArray.push($(ctrl.cboBarra2).val())
            }
            //$(ctrl.cboBarra2).next().find('li.selected input[type="checkbox"]').each((index, target) => {
            //    if ($(target).val() != "--TODOS--") {
            //        BarrasArray.push($(target).val())
            //    }
            //})

            if ($(ctrl.cbPeriodoInicio2).val() == '') {

                mensaje += "Seleccione un Periodo 1. \n";
                error = true;
            }
            if ($(ctrl.cbPeriodoFin2).val() == '') {
                mensaje += "Seleccione un Periodo 2. \n";
                error = true;
            }
            if ($(ctrl.cboVersionInicio2).val() == '') {
                mensaje += "Seleccione un version 1. \n";
                error = true;
            }
            if ($(ctrl.cboVersionFin2).val() == '') {
                mensaje += "Seleccione un version 2. \n";
                error = true;
            }

            if (diaInicioArray.length == 0) {
                mensaje += "Seleccione un dia 1. \n";
                error = true;
            }

            //if (dia2Array.length == 0) {
            //    mensaje += "Seleccione un dia 2. \n";
            //    error = true;
            //}


            if (BarrasArray.length == 0) {
                mensaje += "Seleccione una barra. \n";
                error = true;
            }


            if (error) {
                alert(mensaje);
                return false;
            }

            var promise = new Promise((resolve, reject) => {
                $.ajax({
                    type: 'POST',
                    contentType: 'application/json; charset=utf-8',
                    url: controler + "ListarCostoMarginalDesviacion",
                    data: JSON.stringify({
                        PeriCodi1: $(ctrl.cbPeriodoInicio2).val(),
                        Version1Array: $(ctrl.cboVersionInicio2).val(),
                        Dia1Array: diaInicioArray,
                        PeriCodi2: $(ctrl.cbPeriodoFin2).val(),
                        Version2Array: $(ctrl.cboVersionFin2).val(),
                        // Dia2Array: dia2Array,
                        Dia2Array: diaInicioArray,
                        BarrasArray: BarrasArray,
                        TipoCostoMarginal: tipoCostoMarginal
                    }),
                    success: function (evt) {
                        if (evt.EsCorrecto < 0) {
                            reject(evt.Mensaje)
                        } else {
                            resolve(evt.Data)
                        }
                    },
                    error: function () {
                        reject("Lo sentimos, ha ocurrido un error inesperado");
                    }
                });

            })
            return promise;
        },
        ListarPromedioMarginal: (tipoPromedio, tipoCostoMarginal, parametro) => {
            parametro.tipoPromedio = tipoPromedio;
            parametro.TipoCostoMarginal = tipoCostoMarginal;
            var promise = new Promise((resolve, reject) => {
                $.ajax({
                    type: 'POST',
                    contentType: 'application/json; charset=utf-8',
                    url: controler + "ListarPromedioMarginal",
                    data: JSON.stringify(parametro),
                    success: function (evt) {
                        if (evt.EsCorrecto < 0) {
                            reject(evt.Mensaje)
                        } else {
                            resolve(evt.Data)
                        }
                    },
                    error: function () {
                        reject("Lo sentimos, ha ocurrido un error inesperado");
                    }
                });

            })
            return promise;
        },
        ListarVersiones: (periCodi) => {
            var promise = new Promise((resolve, reject) => {
                $.ajax({
                    type: 'POST',
                    url: controler + "ListarVersiones",
                    data: { periCodi: periCodi },
                    success: function (data) {
                        resolve(data);
                    },
                    error: function () {
                        reject("Lo sentimos, ha ocurrido un error inesperado");
                    }
                });

            })
            return promise;
        },

    }

    var metodo = {
        cargarDatosIniciales: function () {

            $('.multiselect').multipleSelect({

                selectAll: true,
            });
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
            init: (series, titulo = 'Costos Marginales') => {

                if (document.querySelectorAll('.grafica-detalle').length > 0) {
                    document.querySelectorAll('.grafica-detalle').forEach((index, key, target) => {
                        index.remove();
                    })
                }
                var id = $('#tab-container li.active a').attr('data-id')
                var chartype = {};
                chartype = {
                    zoomType: 'x'
                }

                $chart = Highcharts.chart('contenedor' + id, {
                    chart: chartype,
                    title: {
                        text: titulo
                    },
                    //subtitle: {
                    //    text: document.ontouchstart === undefined ?
                    //        'Click and drag in the plot area to zoom in' : 'Pinch the chart to zoom in'
                    //},
                    xAxis: functionMonthDay(),
                    tooltip: functionTootTip(),
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


        fechaMaximoMinimo: (periCodi, targetInicio, targetFin) => {
            var promise = new Promise((resolve, reject) => {
                ajax.ObtenerPeriodoPorId(periCodi).then((data) => {

                    var AnioCodi = data.AnioCodi;
                    var MesCodi = '0' + (data.MesCodi);
                    MesCodi = MesCodi.substr(MesCodi.length - 2);


                    var firstDay = ('0' + new Date(AnioCodi, MesCodi, 1).getDate());
                    firstDay = firstDay.substr(firstDay.length - 2);
                    var lastDay = ('0' + new Date(AnioCodi, MesCodi, 0).getDate());
                    lastDay = lastDay.substr(lastDay.length - 2);

                    $zebraFin = lastDay;
                    console.log(firstDay + '/' + MesCodi + '/' + AnioCodi + '||||' + lastDay + '/' + MesCodi + '/' + AnioCodi)

                    var auxiliarInicio = targetInicio.data('Zebra_DatePicker');
                    auxiliarInicio.clear_date();
                    auxiliarInicio.update({
                        direction: [firstDay + '/' + MesCodi + '/' + AnioCodi, lastDay + '/' + MesCodi + '/' + AnioCodi]
                    })

                    var auxiliarFin = targetFin.data('Zebra_DatePicker');
                    auxiliarFin.clear_date();
                    auxiliarFin.update({
                        direction: [firstDay + '/' + MesCodi + '/' + AnioCodi, lastDay + '/' + MesCodi + '/' + AnioCodi]
                    })

                    resolve();
                }).then(() => {
                    reject();
                })
            })
            return promise;
        },
        costosMarginales: {
            dibujarCboVersiones: () => {
                var promise = new Promise((resolve, reject) => {
                    ajax.ListarVersiones($(ctrl.cboPeriodo1).val()).then((data) => {
                        var option = $(ctrl.cboVersion1).find('option:not(:first-child)');
                        option.remove();
                        data.map((item, index, array) => {
                            var tr = $('<option></option>')
                            tr.attr('value', item.RecaCodi);
                            tr.html(item.RecaNombre);
                            $(ctrl.cboVersion1).append(tr);
                            resolve();
                        })
                    }).catch(() => {
                        reject();
                    })

                })
                return promise;
            },
            dibujarTabla: function (data) {
                var tblBarra_costoMarginal = $(ctrl.tblBarraCostoMarginal).find('tbody');
                tblBarra_costoMarginal.html('');
                data.map((value, index, array) => {
                    let tr = `<tr data-row-id="${value.BarrCodi}"><td>${value.BarrNombre}</td><td><input type="checkbox" class='checkedBarra' /></td></tr >`
                    tblBarra_costoMarginal.append(tr)
                })
            },
            dibujarGrafica: (tipoCostoMarginal, parametro) => {
                ajax.ConsultaCostosMarginales(tipoCostoMarginal, parametro).then((data) => {
                    arrayGrafica = [];
                    data.map((itemParent, index, array) => {
                        let barra = {
                            type: 'area',
                            name: itemParent.BarrNombre,
                            data: []
                        };

                        itemParent.CostosMarginales.map((value, index, array) => {
                            var valor = 0;
                            if (tipoCostoMarginal == "CMGTO") {
                                valor = value.CMGRTotal;
                            } else if (tipoCostoMarginal == "CMGCN") {
                                valor = value.CMGRCongestion;
                            }
                            else if (tipoCostoMarginal == "CMGEN") {
                                valor = value.CMGREnergia;
                            }
                            barra.data.push([
                                parseInt(value.FechaIntervalo.replace("/Date(", "").replace(")/", ""), 10),
                                valor
                            ])
                        })
                        arrayGrafica.push(barra);
                    })
                    metodo.chartJS.init(arrayGrafica);

                }).catch((reject) => {
                    alert(reject)
                })

            }
        },
        desviacion: {
            dibujarTablaAdicional: () => {
                var promise = new Promise((response, reject) => {
                    ajax.ListarCostoMarginalDesviacion($(ctrl.cboTipoCostoMarginal2).val()).then((data) => {
                        var table = $(ctrl.tblAdicional2).find('tbody');
                        table.html('')
                        data.map((item, index, array) => {
                            var tr = $('<tr></tr>');
                            tr.append(`<td>${item.Hora}</td>`);
                            tr.append(`<td class="cmg1">${item.CostoMarginal1}</td>`);
                            tr.append(`<td class="cmg2">${item.CostoMarginal2}</td>`);
                            tr.append(`<td class="desviacion">${item.Desviacion}</td>`);
                            tr.append(`<td class="fecha" style="display:none">${item.FechaIntervalo}</td>`);
                            table.append(tr);
                        })
                        response(data);
                    }).catch(() => {
                        reject();
                    })

                })
                return promise;
            },
            dibujarGraficaLineal: (data, titulo) => {
                arrayGrafica = [];

                let barra = {
                    type: 'area',
                    name: "Costo Marginal 1",
                    data: []
                };
                data.map((itemParent, index, array) => {

                    barra.data.push([
                        parseInt(itemParent.FechaIntervalo.replace("/Date(", "").replace(")/", ""), 10),
                        itemParent.CostoMarginal1
                    ])
                })

                arrayGrafica.push(barra);
                let barra2 = {
                    type: 'area',
                    name: "Costo Marginal 2",
                    data: []
                };
                data.map((itemParent, index, array) => {
                    barra2.data.push([
                        parseInt(itemParent.FechaIntervalo.replace("/Date(", "").replace(")/", ""), 10),
                        itemParent.CostoMarginal2
                    ])
                })

                arrayGrafica.push(barra2);

                metodo.chartJS.init(arrayGrafica, titulo);
            }
            ,
            dibujarCboVersiones: (targetPeriodo, targetVersion) => {
                var promise = new Promise((resolve, reject) => {
                    ajax.ListarVersiones(targetPeriodo.val()).then((data) => {
                        var option = targetVersion.find('option:not(:first-child)');
                        option.remove();
                        data.map((item, index, array) => {
                            var tr = $('<option></option>')
                            tr.attr('value', item.RecaCodi);
                            tr.html(item.RecaNombre);
                            targetVersion.append(tr);
                            resolve();
                        })
                    }).catch(() => {
                        reject();
                    })

                })
                return promise;
            },
            dibujarBarras: (data) => {

                var element = $(ctrl.cboBarra2);
                element.find('option:not(:first-child)').remove();
                data.map((value, index, array) => {
                    let tr = `<option value="${value.BarrCodi}">${value.BarrNombre}</option>`
                    element.append(tr)
                })

            }
        },
        promedioMarginales: {
            dibujarTabla: function (data) {
                var tblBarra_costoMarginal = $(ctrl.tblBarraPromedioMarginal).find('tbody');
                tblBarra_costoMarginal.html('');
                data.map((value, index, array) => {
                    let tr = `<tr data-row-id="${value.BarrCodi}"><td>${value.BarrNombre}</td><td><input type="checkbox" class='checkedBarra' /></td></tr >`
                    tblBarra_costoMarginal.append(tr)
                })
            },
            dibujarGrafica: (tipoPromedio, tipoCostoMarginal, parametro) => {
                ajax.ListarPromedioMarginal(tipoPromedio, tipoCostoMarginal, parametro).then((data) => {
                    arrayGrafica = [];

                    data.map((itemParent, index, array) => {
                        let barra = {
                            type: 'area',
                            name: itemParent.BarrNombre,
                            data: []
                        };
                        itemParent.CostosMarginales.map((value, index, array) => {
                            console.log(value.CMGRPromedio)
                            var valor = 0;
                            // if (tipoCostoMarginal == "CMGTO") {
                            valor = value.CMGRPromedio;
                            //}
                            barra.data.push([
                                parseInt(value.FechaIntervalo.replace("/Date(", "").replace(")/", ""), 10),
                                valor
                            ])
                        })
                        arrayGrafica.push(barra);
                    })
                    metodo.chartJS.init(arrayGrafica);

                }).catch((reject) => {
                    alert(reject)
                })

            },
            diarioInit: {
                init: () => {

                    $('.MN').hide();
                    $('.DI').show();
                    metodo.chartJS.init(null);
                    $(ctrl.tblBarraPromedioMarginal).find('tbody').html('');

                },



            },
            mensualInit: {
                init: () => {
                    $('.DI').hide();
                    $('.MN').show();
                    metodo.chartJS.init(null);
                    $(ctrl.tblBarraPromedioMarginal).find('tbody').html('');

                }
            }
        },
        tabPaneles: {
            tabSeleccionado: (id) => {
                $('div[data-for]').hide();
                $('li > a[data-id]').parent().removeClass('active')
                $('li > a[data-id="' + id + '"]').parent().addClass('active')

                //Limpiar paso 1

                //Limpiar paso 2

                //Limpiar paso 3


                $zebraInicio = null;
                $zebraFin = null;
                let tabPane = $('div[data-for="' + id + '"]');
                tabPane.show();
                switch (id) {
                    case "1":
                        //Por defecto
                        metodo.chartJS.init(null);
                        $(ctrl.txtInicio1).val('')
                        $(ctrl.txtFin1).val('')
                        $(ctrl.paso1 + ' .CMG').removeClass('active')
                        $(ctrl.paso1 + ' .CMG:first-child').addClass('active')
                        $(ctrl.tblBarraCostoMarginal).find('tbody').html('');
                        break
                    case "2":
                        break
                    case "3":
                        //Por defecto

                        $(ctrl.cboTipoReporte3).val('MN')
                        $(ctrl.paso3 + ' .CMG').removeClass('active')
                        $(ctrl.paso3 + ' .CMG:first-child').addClass('active')
                        $(ctrl.tblBarraPromedioMarginal).find('tbody').html('');
                        metodo.chartJS.init(null);
                        $(ctrl.cbPeriodoInicio3).val('');
                        $(ctrl.cbPeriodoFin3).val('');
                        $(ctrl.txtFechaInicio3).val('');
                        $(ctrl.txtFechaFin3).val('');
                        metodo.promedioMarginales.mensualInit.init();
                        break
                    default:
                }
            }
        }
    }

    var functionMonthDay = function () {
        let id = $('#tab-container li.active a').attr('data-id');
        if (id != 3) {
            return {
                type: 'datetime',
                // verificar acá
                tickInterval: id != 3 ? 3600 * 1000 : 0,
                labels: {
                    formatter: function () {
                        return id != 3 ? Highcharts.dateFormat("%H:%M", this.value) : Highcharts.dateFormat("%M");
                    }
                }
            }
        }
        else {
            return {
                type: 'datetime',
                // verificar acá
                tickInterval: id != 3 ? 3600 * 1000 : 0
            }
        }
    }

    var functionTootTip = function () {
        let id = $('#tab-container li.active a').attr('data-id');
        if (id == 2) {
            return {
                formatter: function () {
                    return Highcharts.dateFormat('%H:%M', new Date(this.x)) + '<br>' +
                        '<b>' + this.series.name + ' : ' + this.y + '</b>';
                }
            }
        } else {
            return {};
        }
    }

    //==================================================================================
    //--Gestiona los listeners
    //==================================================================================
    var handles = function () {
        this.init = () => {
            $('.txtFecha').Zebra_DatePicker({
            });

            metodo.cargarDatosIniciales();

            this.change_tabContainer();
            //Paso1
            this.click_btnConsultar1();
            this.change_cboPeriodo_paso1();
            this.change_checkedBarra_tblBarraCostoMarginal1();
            this.change_checkedAll_tblBarraCostoMarginal1();
            this.keyup_busqueda_tblBarra_costoMarginal1();
            this.click_btntipoCMG_paso1();
            //Paso2
            this.change_cboPeriodoInicioFin_paso2();
            this.click_btnConsultar2();
            this.click_btntipo_TIPDSV_paso2();
            //Paso3
            this.click_btnConsultar3();
            this.change_checkedBarra_tblBarraCostoMarginal3();
            this.change_checkedAll_tblBarraCostoMarginal3();
            this.keyup_busqueda_tblBarra_costoMarginal3();
            this.click_btntipoCMG_paso3();
            this.change_cboTipoReporte_p3();
            //
            metodo.chartJS.init(null);

        };

        //COMUNES

        this.change_tabContainer = () => {
            $(ctrl.tabContainer + ' li > a').click((target) => {
                $(ctrl.tabContainer + ' li').removeClass('active')

                var id = $(target.currentTarget).attr('data-id');
                metodo.tabPaneles.tabSeleccionado(id)
            })
        }
        //----------------------------------------------------------------------------------
        //Paso1
        //----------------------------------------------------------------------------------
        this.click_btntipoCMG_paso1 = () => {
            $(ctrl.paso1 + ' .CMG').click((target) => {
                $(ctrl.paso1 + ' .CMG').removeClass('active')
                $(target.currentTarget).addClass('active')

                var elementChecked = [];

                $(ctrl.tblBarraCostoMarginal + ' .checkedBarra:checked').each((index, targetChecked) => {
                    elementChecked.push($(targetChecked).parents('tr').attr('data-row-id'))
                })
                if (elementChecked.length > 0) {

                    var dia = $(ctrl.txtInicio1).val().split('/')[0];
                    var fin = $(ctrl.txtFin1).val().split('/')[0];

                    metodo.costosMarginales.dibujarGrafica(
                        $(target.currentTarget).attr('data-id'),
                        {
                            PeriCodi: $(ctrl.cboPeriodo1).val(),
                            CosMarVersion: $(ctrl.cboVersion1).val(),
                            DiaInicio: dia == "" ? 1 : dia,
                            DiaFin: fin == "" ? $zebraFin : fin,
                            FechaInicio: $(ctrl.txtInicio1).val(),
                            FechaFin: $(ctrl.txtFin1).val(),
                            ListaBarras: elementChecked
                        });
                }


            })
        }
        this.click_btnConsultar1 = () => {
            $(ctrl.btnConsultar1).click((target) => {
                metodo.chartJS.init(null);

                var mensaje = "";
                var error = false;
                if ($(ctrl.cboPeriodo1).val() == '') {
                    mensaje += 'Seleccione un periodo \n';
                    error = true;
                }
                if ($(ctrl.cboVersion1).val() == '') {
                    mensaje += 'Seleccione una version \n';
                    error = true;
                }


                if (!error) {

                    var dia = $(ctrl.txtInicio1).val().split('/')[0];
                    var fin = $(ctrl.txtFin1).val().split('/')[0];
                    ajax.BarrasMarginales({
                        PeriCodi: $(ctrl.cboPeriodo1).val(),
                        CosMarVersion: $(ctrl.cboVersion1).val(),
                        DiaInicio: dia == "" ? 1 : dia,
                        DiaFin: fin == "" ? $zebraFin : fin
                    }).then((data) => {
                        $busquedaBarras = data;
                        metodo.costosMarginales.dibujarTabla(data)
                        metodo.chartJS.init(null)
                    }).catch((reject) => {
                        alert(reject)
                    })
                } else {
                    alert(mensaje)
                }

            })
        }
        this.change_checkedBarra_tblBarraCostoMarginal1 = () => {
            $(document).on('click', ctrl.tblBarraCostoMarginal + ' .checkedBarra', (target) => {
                var elementChecked = [];
                $(ctrl.tblBarraCostoMarginal + ' .checkedBarra:checked').each((index, targetChecked) => {
                    elementChecked.push($(targetChecked).parents('tr').attr('data-row-id'))
                })
                if (elementChecked.length == 0) {
                    alert('Es necesario seleccionar al menos una barra.')
                    target.preventDefault();
                    target.stopPropagation();
                    return false;

                } else {

                    var dia = $(ctrl.txtInicio1).val().split('/')[0];
                    var fin = $(ctrl.txtFin1).val().split('/')[0];
                    metodo.costosMarginales.dibujarGrafica(
                        $(ctrl.paso1 + ' .CMG.active').attr('data-id')
                        , {
                            PeriCodi: $(ctrl.cboPeriodo1).val(),
                            CosMarVersion: $(ctrl.cboVersion1).val(),
                            DiaInicio: dia == "" ? 1 : dia,
                            DiaFin: fin == "" ? $zebraFin : fin,
                            FechaInicio: $(ctrl.txtInicio1).val(),
                            FechaFin: $(ctrl.txtFin1).val(),
                            ListaBarras: elementChecked
                        });
                }
            })
        }
        this.change_cboPeriodo_paso1 = () => {

            $(ctrl.cboPeriodo1).change((target) => {
                if ($(target.currentTarget).val() == '') {

                    //var plugin = $('.txtFecha').Zebra_DatePicker({
                    //});
                    $('#paso1 .txtFecha').prop('disabled', true)
                    //plugin.destroy();
                } else {
                    metodo.chartJS.init(null);
                    $(ctrl.txtInicio1).val('')
                    $(ctrl.txtFin1).val('')
                    $(ctrl.paso1 + ' .CMG').removeClass('active')
                    $(ctrl.paso1 + ' .CMG:first-child').addClass('active')
                    $(ctrl.tblBarraCostoMarginal).find('tbody').html('');
                    $('#paso1 .txtFecha').prop('disabled', false)
                    metodo.fechaMaximoMinimo($(target.currentTarget).val(), $(ctrl.txtInicio1), $(ctrl.txtFin1)).then(() => {
                        metodo.costosMarginales.dibujarCboVersiones();
                    }).catch(() => {
                    })


                }
            })
        }
        this.keyup_busqueda_tblBarra_costoMarginal1 = () => {
            $(document).on('keyup', ctrl.tblBarraCostoMarginal + ' .busque', (target) => {
                metodo.chartJS.init(null);
                $(ctrl.tblBarraCostoMarginal + ' .checkedAll').prop('checked', false)
                var valor = $(target.currentTarget).val().toLowerCase();
                var tabla = $busquedaBarras.filter(x => x.BarrNombre.toLowerCase().indexOf(valor) >= 0);
                metodo.costosMarginales.dibujarTabla(tabla)
            })
        }

        this.change_checkedAll_tblBarraCostoMarginal1 = () => {
            $(ctrl.tblBarraCostoMarginal + ' .checkedAll').click((target) => {

                $(ctrl.tblBarraCostoMarginal + ' tr input').prop('checked', $(target.currentTarget).prop('checked'));
                if ($(target.currentTarget).prop('checked')) {
                    var elementChecked = [];
                    $(ctrl.tblBarraCostoMarginal + ' .checkedBarra:checked').each((index, targetChecked) => {
                        elementChecked.push($(targetChecked).parents('tr').attr('data-row-id'))
                    })

                    var dia = $(ctrl.txtInicio1).val().split('/')[0];
                    var fin = $(ctrl.txtFin1).val().split('/')[0];
                    metodo.costosMarginales.dibujarGrafica($(ctrl.paso1 + ' .CMG.active').attr('data-id'),
                        {

                            PeriCodi: $(ctrl.cboPeriodo1).val(),
                            CosMarVersion: $(ctrl.cboVersion1).val(),
                            DiaInicio: dia == "" ? 1 : dia,
                            DiaFin: fin == "" ? $zebraFin : fin,
                            FechaInicio: $(ctrl.txtInicio1).val(),
                            FechaFin: $(ctrl.txtFin1).val(),
                            ListaBarras: elementChecked
                        });
                } else {
                    metodo.chartJS.init(null);
                }

            })
        }
        //----------------------------------------------------------------------------------
        //Paso2
        //----------------------------------------------------------------------------------
        this.change_cboPeriodoInicioFin_paso2 = () => {
            $(ctrl.cbPeriodoInicio2).change((target) => {
                $(ctrl.tblAdicional2).find('tbody').html('');
                metodo.chartJS.init(null);
                let valor = $(ctrl.cbPeriodoInicio2).val();
                if (valor != '') {
                    Promise.all([metodo.desviacion.dibujarCboVersiones($(ctrl.cbPeriodoInicio2), $(ctrl.cboVersionInicio2))
                        , ajax.BarrasMarginalesDiarioMensual({
                            tipoPromedio: "M",
                            PeriCodi: $(ctrl.cbPeriodoInicio2).val(),
                            PeriCodiFin: $(ctrl.cbPeriodoFin2).val(),
                        })
                    ]).then((arrayData) => {
                        metodo.desviacion.dibujarBarras(arrayData[1])
                        //$(ctrl.cboBarra2).multipleSelect({

                        //    selectAll: true,
                        //});
                    }).catch((reject) => {
                        alert(reject)
                    })
                }

            })

            $(ctrl.cbPeriodoFin2).change((target) => {

                $(ctrl.tblAdicional2).find('tbody').html('');
                metodo.chartJS.init(null);

                let valor = $(ctrl.cbPeriodoFin2).val();
                if (valor != '') {
                    Promise.all([metodo.desviacion.dibujarCboVersiones($(ctrl.cbPeriodoFin2), $(ctrl.cboVersionFin2))
                        , ajax.BarrasMarginalesDiarioMensual({
                            tipoPromedio: "M",
                            PeriCodi: $(ctrl.cbPeriodoInicio2).val(),
                            PeriCodiFin: $(ctrl.cbPeriodoFin2).val(),
                        })
                    ]).then((arrayData) => {
                        console.log(arrayData)
                        metodo.desviacion.dibujarBarras(arrayData[1])
                        //$(ctrl.cboBarra2).multipleSelect({

                        //    selectAll: true,

                        //});
                    }).catch((reject) => {
                        alert(reject)
                    })
                }

            })



        }

        this.click_btnConsultar2 = () => {
            $(ctrl.btnConsultar2).click((target) => {
                metodo.desviacion.dibujarTablaAdicional().then((data) => {


                    var titulo = "Costos Marginales ";
                    //$(ctrl.cboBarra2).next().find('li.selected').each((index, target) => {
                    //    if ($(target).val() != "--TODOS--") {
                    //        titulo += $(target).text() + ","
                    //    }
                    //})
                    titulo += $(ctrl.cboBarra2).find('option:selected').html();
                    metodo.desviacion.dibujarGraficaLineal(data, titulo.substr(0, titulo.length - 1));
                    $("#paso2 .TIPDSV")[0].click();
                })
            })
        }

        this.click_btntipo_TIPDSV_paso2 = () => {
            $(ctrl.paso2 + ' .TIPDSV').click((target) => {
                $(ctrl.paso2 + ' .TIPDSV').removeClass('active')
                $(target.currentTarget).addClass('active')

                var tipo = $(target.currentTarget).attr('data-id');

                if (tipo == "CM") {
                    var param = [];

                    $(ctrl.tblAdicional2 + ' > tbody > tr').each((index, target) => {
                        var tr = $(target);
                        var fecha = tr.find('.fecha').html();
                        var cmg1 = parseFloat(tr.find('.cmg1').html());
                        var cmg2 = parseFloat(tr.find('.cmg2').html());
                        param.push({
                            FechaIntervalo: fecha,
                            CostoMarginal1: cmg1,
                            CostoMarginal2: cmg2
                        })
                    });

                    var titulo = "Costos Marginales ";
                    //let esTodosBarra = $(ctrl.cboBarra2).next().find('input[name="selectAll"]:checked').length;
                    //if (esTodosBarra == 0) {
                    //    $(ctrl.cboBarra2).next().find('li.selected').each((index, target) => {
                    //        if ($(target).val() != "--TODOS--") {
                    //            titulo += $(target).text() + ","
                    //        }
                    //    })
                    //}

                    titulo += $(ctrl.cboBarra2).find('option:selected').html();
                    metodo.desviacion.dibujarGraficaLineal(param, titulo.substr(0, titulo.length - 1));

                } else if (tipo == "DS") {
                    var titulo = "Costos Marginales ";
                    //let esTodosBarra = $(ctrl.cboBarra2).next().find('input[name="selectAll"]:checked').length;
                    //if (esTodosBarra == 0) {
                    //    $(ctrl.cboBarra2).next().find('li.selected').each((index, target) => {
                    //        if ($(target).val() != "--TODOS--") {
                    //            titulo += $(target).text() + ","
                    //        }
                    //    })
                    //}
                    titulo += $(ctrl.cboBarra2).find('option:selected').html();

                    let barra = {
                        "type": "column",
                        name: "% VARIACIÓN",
                        data: []
                    };
                    $(ctrl.tblAdicional2 + ' > tbody > tr').each((index, target) => {
                        var tr = $(target);
                        barra.data.push([
                            parseInt(tr.find('.fecha').html().replace("/Date(", "").replace(")/", ""), 10),
                            parseFloat(tr.find('.desviacion').html())
                        ])
                    });
                    metodo.chartJS.init([barra], titulo.substr(0, titulo.length - 1));
                }


            })
        }
        //----------------------------------------------------------------------------------
        //Paso3
        //----------------------------------------------------------------------------------
        this.click_btntipoCMG_paso3 = () => {
            $(ctrl.paso3 + ' .CMG').click((target) => {
                $(ctrl.paso3 + ' .CMG').removeClass('active')
                $(target.currentTarget).addClass('active')

                var elementChecked = [];

                $(ctrl.tblBarraPromedioMarginal + ' .checkedBarra:checked').each((index, targetChecked) => {
                    elementChecked.push($(targetChecked).parents('tr').attr('data-row-id'))
                })
                if (elementChecked.length > 0) {
                    var parametro = {};
                    var tipoPromedio = "";
                    if ($(ctrl.cboTipoReporte3).val() == 'MN') {
                        tipoPromedio = "M";

                        parametro = {
                            PeriCodi: $(ctrl.cbPeriodoInicio3).val(),
                            PeriCodiFin: $(ctrl.cbPeriodoFin3).val(),
                            ListaBarras: elementChecked
                        }
                    } else if ($(ctrl.cboTipoReporte3).val() == 'DI') {
                        tipoPromedio = "D";

                        parametro = {
                            FechaInicio: $(ctrl.txtFechaInicio3).val(),
                            FechaFin: $(ctrl.txtFechaFin3).val(),
                            ListaBarras: elementChecked
                        }
                    }

                    metodo.promedioMarginales.dibujarGrafica(
                        tipoPromedio,
                        $(target.currentTarget).attr('data-id'),
                        parametro);
                }
            })
        }
        this.click_btnConsultar3 = () => {
            $(ctrl.btnConsultar3).click((target) => {
                $(ctrl.tblBarraPromedioMarginal + ' ' + '.busque').val('')
                var parametro = {};
                if ($(ctrl.cboTipoReporte3).val() == 'MN') {
                    parametro = {
                        tipoPromedio: "M",
                        PeriCodi: $(ctrl.cbPeriodoInicio3).val(),
                        PeriCodiFin: $(ctrl.cbPeriodoFin3).val(),
                    }
                } else if ($(ctrl.cboTipoReporte3).val() == 'DI') {
                    parametro = {
                        tipoPromedio: "D",
                        FechaInicio: $(ctrl.txtFechaInicio3).val(),
                        FechaFin: $(ctrl.txtFechaFin3).val(),
                    }
                }


                ajax.BarrasMarginalesDiarioMensual(parametro).then((data) => {
                    $busquedaBarras = data;
                    metodo.promedioMarginales.dibujarTabla(data)
                    metodo.chartJS.init(null)
                }).catch((reject) => {
                    alert(reject)
                })
            })
        }
        this.change_checkedAll_tblBarraCostoMarginal3 = () => {
            $(ctrl.tblBarraPromedioMarginal + ' .checkedAll').click((target) => {

                $(ctrl.tblBarraPromedioMarginal + ' tr input').prop('checked', $(target.currentTarget).prop('checked'));
                if ($(target.currentTarget).prop('checked')) {
                    var elementChecked = [];
                    $(ctrl.tblBarraPromedioMarginal + ' .checkedBarra:checked').each((index, targetChecked) => {
                        elementChecked.push($(targetChecked).parents('tr').attr('data-row-id'))
                    })

                    var dia = $(ctrl.txtFechaInicio3).val().split('/')[0];
                    var fin = $(ctrl.txtFechaFin3).val().split('/')[0];

                    if ($(ctrl.cboTipoReporte3).val() == 'MN') {
                        metodo.promedioMarginales.dibujarGrafica(
                            "M",
                            $(ctrl.paso3 + ' .CMG.active').attr('data-id')
                            , {
                                PeriCodi: $(ctrl.cbPeriodoInicio3).val(),
                                PeriCodiFin: $(ctrl.cbPeriodoFin3).val(),
                                ListaBarras: elementChecked
                            });
                    } else if ($(ctrl.cboTipoReporte3).val() == 'DI') {
                        metodo.promedioMarginales.dibujarGrafica(
                            "D",
                            $(ctrl.paso3 + ' .CMG.active').attr('data-id')
                            , {
                                FechaInicio: $(ctrl.txtFechaInicio3).val(),
                                FechaFin: $(ctrl.txtFechaFin3).val(),
                                ListaBarras: elementChecked
                            });
                    }

                } else {
                    metodo.chartJS.init(null);
                }

            })
        }
        this.keyup_busqueda_tblBarra_costoMarginal3 = () => {
            $(document).on('keyup', ctrl.tblBarraPromedioMarginal + ' ' + '.busque', (target) => {
                metodo.chartJS.init(null);
                $(ctrl.tblBarraPromedioMarginal + ' .checkedAll').prop('checked', false)
                var valor = $(target.currentTarget).val().toLowerCase();

                var tabla = $busquedaBarras.filter(x => x.BarrNombre.toLowerCase().indexOf(valor) >= 0);

                metodo.promedioMarginales.dibujarTabla(tabla)

            })
        }
        this.change_checkedBarra_tblBarraCostoMarginal3 = () => {
            $(document).on('click', ctrl.tblBarraPromedioMarginal + ' .checkedBarra', (target) => {
                var elementChecked = [];
                $(ctrl.tblBarraPromedioMarginal + ' .checkedBarra:checked').each((index, targetChecked) => {
                    elementChecked.push($(targetChecked).parents('tr').attr('data-row-id'))
                })
                if (elementChecked.length == 0) {
                    alert('Es necesario seleccionar al menos una barra.')
                    target.preventDefault();
                    target.stopPropagation();
                    return false;

                } else {
                    if ($(ctrl.cboTipoReporte3).val() == 'MN') {


                        $(ctrl.contenedor3).attr('data-chart-view', 'month')
                        metodo.promedioMarginales.dibujarGrafica(
                            "M",
                            $(ctrl.paso3 + ' .CMG.active').attr('data-id')
                            , {
                                PeriCodi: $(ctrl.cbPeriodoInicio3).val(),
                                PeriCodiFin: $(ctrl.cbPeriodoFin3).val(),
                                ListaBarras: elementChecked
                            });
                    } else if ($(ctrl.cboTipoReporte3).val() == 'DI') {
                        $(ctrl.contenedor3).removeAttr('data-chart-view')
                        metodo.promedioMarginales.dibujarGrafica(
                            "D",
                            $(ctrl.paso3 + ' .CMG.active').attr('data-id')
                            , {
                                FechaInicio: $(ctrl.txtFechaInicio3).val(),
                                FechaFin: $(ctrl.txtFechaFin3).val(),
                                ListaBarras: elementChecked
                            });
                    }

                }
            })
        }
        this.change_cboTipoReporte_p3 = () => {
            $(ctrl.cboTipoReporte3).change((target) => {

                if ($(target.currentTarget).val() == 'MN') {

                    metodo.promedioMarginales.mensualInit.init();
                } else if ($(target.currentTarget).val() == 'DI') {

                    metodo.promedioMarginales.diarioInit.init();
                }
            })
        }

    }



    $(document).ready(() => {
        var obj = new handles();
        obj.init();

        $('#cboDiaInicio_paso2').multipleSelect({
            width: '220px',
            filter: true,
            selectAll: false,
            single: true,
            onClick: function (view) {
                //var emprcodi = $("#emprcodi").multipleSelect('getSelects');
                //$('#hfComboEmpresa').val(emprcodi);
                //if ($('#pegrtipoinformacion option:selected').val() == 2) {
                //    obtenerCargosEmpresasGeneradoras();
                //}
            },
            onClose: function (view) {
                //var emprcodi = $("#emprcodi").multipleSelect('getSelects');
                //$('#hfComboEmpresa').val(emprcodi);
            }
        });
        $('#cboFin_paso2').multipleSelect({
            width: '220px',
            filter: true,
            selectAll: false,
            single: true,
            onClick: function (view) {
                //var emprcodi = $("#emprcodi").multipleSelect('getSelects');
                //$('#hfComboEmpresa').val(emprcodi);
                //if ($('#pegrtipoinformacion option:selected').val() == 2) {
                //    obtenerCargosEmpresasGeneradoras();
                //}
            },
            onClose: function (view) {
                //var emprcodi = $("#emprcodi").multipleSelect('getSelects');
                //$('#hfComboEmpresa').val(emprcodi);
            }
        });


    })
})();
