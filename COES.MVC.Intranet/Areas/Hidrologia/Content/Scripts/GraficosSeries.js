var controlador = siteRoot + 'hidrologia/graficosseries/';

var plot = null;
var charAnual;
var charMensual;
var charComparativaVolumen;
var charComparativaNaturalEvaporada;
var charComparativaLineaTendencia;
var chartEstadisticasAnuales;
$(function () {
    var currentDate = new Date();
    var currentMonth = currentDate.getMonth();
    var currentYear = currentDate.getFullYear();
    // Ajustar a un mes anterior
    if (currentMonth === 0) {
        currentMonth = 11;
        currentYear -= 1;
    } else {
        currentMonth -= 1;
    }

    var endDate = formatDateToMMYYYY(new Date(currentYear, currentMonth, 1));
    const defaultDate = new Date(1965, 0, 1); // Enero es 0 en JavaScript
    const formattedDate = formatDateToMMYYYY(defaultDate);
    $('#txtFechaDesde').blur(function () {
        if ($(this).val().trim() === '') {
            $(this).val(formattedDate);
        }
    });

    $('#txtFechaHasta').blur(function () {
        if ($(this).val().trim() === '') {
            $(this).val(endDate);
        }
    });

    $('#txtFechaDesdeTab5').blur(function () {
        if ($(this).val().trim() === '') {
            $(this).val(formattedDate);
        }
    });

    $('#txtFechaHastaTab5').blur(function () {
        if ($(this).val().trim() === '') {
            $(this).val(endDate);
        }
    });

    $('#cbTipoSerie').val($('#hfTipoSerie').val());

    $('#tab-container').easytabs({
        animate: false
    });

    $('#tab-container-principal').easytabs({
        animate: false
    });




    $('#txtFechaDesde').Zebra_DatePicker({
        readonly_element: false,
        format: 'm/Y',  // Formato de mes y año
        view: 'years',  // Vista inicial
        start_date: '01/1965', // Fecha de inicio
        direction: ['01/1965', endDate], // Rango de fechas permitido
        onSelect: function (date) {
            $('#txtFechaDesde').val(date);
        }
    });


    $('#txtFechaDesde').inputmask({
        mask: "1/y",
        placeholder: "mm/yyyy",
        alias: "date",
        hourFormat: "24"
    });

    $('#txtFechaDesdeTab5').Zebra_DatePicker({
        readonly_element: false,
        format: 'm/Y',  // Formato de mes y año
        view: 'years',  // Vista inicial
        start_date: '01/1965', // Fecha de inicio
        direction: ['01/1965', endDate], // Rango de fechas permitido
        onSelect: function (date) {
            $('#txtFechaDesdeTab5').val(date);
        }
    });


    $('#txtFechaDesdeTab5').inputmask({
        mask: "1/y",
        placeholder: "mm/yyyy",
        alias: "date",
        hourFormat: "24"
    });



    $('#txtFechaHastaTab5').Zebra_DatePicker({
        readonly_element: false,
        format: 'm/Y',  // Formato de mes y año
        view: 'years',  // Vista inicial
        start_date: '02/1965', // Fecha de inicio
        direction: ['02/1965', endDate], // Rango de fechas permitido
        onSelect: function (date) {
            $('#txtFechaHastaTab5').val(date);
        }
    });


    $('#txtFechaHastaTab5').inputmask({
        mask: "1/y",
        placeholder: "mm/yyyy",
        alias: "date",
        hourFormat: "24"
    });

    
    $('#txtFechaHasta').Zebra_DatePicker({
        readonly_element: false,
        format: 'm/Y',  // Formato de mes y año
        view: 'years',  // Vista inicial
        start_date: '02/1965', // Fecha de inicio
        direction: ['02/1965', endDate], // Rango de fechas permitido
        onSelect: function (date) {
            $('#txtFechaHasta').val(date);
        }
    });


    $('#txtFechaHasta').inputmask({
        mask: "1/y",
        placeholder: "mm/yyyy",
        alias: "date",
        hourFormat: "24"
    });

    $('#tab1Paso1').click(function () {
        limpiarTab1();
        $('#paso').val('1');
    })
    $('#tab2Paso2').click(function () {
        limpiarTab1();
        $('#paso').val('2');
;        $.ajax({
            type: 'POST',
            url: controlador + "graficoMensual",
            data: {
                
            },
            success: function (evt) {

                $('#listado').css("width", $('#mainLayout').width() + "px");
                $('#listado').html(evt);
                $('#tabla').dataTable({
                    "scrollY": 430,
                    "scrollX": false,
                    "sDom": 't',
                    "ordering": false,
                    "iDisplayLength": -1
                });

            },
            error: function () {
                mostrarError();
            }
        });
    });

    $('#tab3Paso3').click(function () {
        var self = this;
        limpiarTab1();
        $('#paso').val('3');
        ; $.ajax({
            type: 'POST',
            url: controlador + "graficoComparativaVolumen",
            data: {

            },
            success: function (evt) {

                $('#listadotab3').css("width", $('#mainLayout').width() + "px");
                $('#listadotab3').html(evt);
                $('#tabla').dataTable({
                    "scrollY": 430,
                    "scrollX": false,
                    "sDom": 't',
                    "ordering": false,
                    "iDisplayLength": -1
                });


            },
            error: function () {
                mostrarError();
            }
        });
    });

    $('#tab4Paso4').click(function () {
        var self = this;
        limpiarTab1();
        $('#paso').val('4');
        ; $.ajax({
            type: 'POST',
            url: controlador + "graficoComparativaNaturalEvaporada",
            data: {

            },
            success: function (evt) {

                $('#listadotab4').css("width", $('#mainLayout').width() + "px");
                $('#listadotab4').html(evt);
                $('#tabla').dataTable({
                    "scrollY": 430,
                    "scrollX": false,
                    "sDom": 't',
                    "ordering": false,
                    "iDisplayLength": -1
                });

            },
            error: function () {
                mostrarError();
            }
        });
    });

    $('#tab5Paso5').click(function () {
        var self = this;
        limpiarTab1();
        $('#paso').val('5');
        ; $.ajax({
            type: 'POST',
            url: controlador + "graficoComparativaLineaTendencia",
            data: {

            },
            success: function (evt) {

                $('#listadotab5').css("width", $('#mainLayout').width() + "px");
                $('#listadotab5').html(evt);
                $('#tabla').dataTable({
                    "scrollY": 430,
                    "scrollX": false,
                    "sDom": 't',
                    "ordering": false,
                    "iDisplayLength": -1
                });

            },
            error: function () {
                mostrarError();
            }
        });
    });

    $('#tab6Paso6').click(function () {
        var self = this;
        $('#paso').val('6');


        limpiarTab1();
        ; $.ajax({
            type: 'POST',
            url: controlador + "graficoBarrasEstadisticasAnuales",
            data: {

            },
            success: function (evt) {

                $('#listadotab6').css("width", $('#mainLayout').width() + "px");
                $('#listadotab6').html(evt);
                $('#tabla').dataTable({
                    "scrollY": 430,
                    "scrollX": false,
                    "sDom": 't',
                    "ordering": false,
                    "iDisplayLength": -1
                });

                $('#txtFechaDesdeTab6').Zebra_DatePicker({
                    readonly_element: false,
                    format: 'm/Y',  // Formato de mes y año
                    view: 'years',  // Vista inicial
                    start_date: '01/1965', // Fecha de inicio
                    direction: ['01/1965', endDate], // Rango de fechas permitido
                    onSelect: function (date) {
                        $('#txtFechaDesdeTab6').val(date);
                    }
                });


                $('#txtFechaDesdeTab6').inputmask({
                    mask: "1/y",
                    placeholder: "mm/yyyy",
                    alias: "date",
                    hourFormat: "24"
                });



                $('#txtFechaHastaTab6').Zebra_DatePicker({
                    readonly_element: false,
                    format: 'm/Y',  // Formato de mes y año
                    view: 'years',  // Vista inicial
                    start_date: '01/1965', // Fecha de inicio
                    direction: ['01/1965', endDate], // Rango de fechas permitido
                    onSelect: function (date) {
                        $('#txtFechaHastaTab6').val(date);
                    }
                });


                $('#txtFechaHastaTab6').inputmask({
                    mask: "1/y",
                    placeholder: "mm/yyyy",
                    alias: "date",
                    hourFormat: "24"
                });

                $('#txtFechaDesdeTab6').val(formattedDate);
                $('#txtFechaHastaTab6').val(endDate);                

            },
            error: function () {
                mostrarError();
            }
        });
    });

    $('#btnExportar').click(function () {
        exportarGraficoAnual();

    });
    $('#btnExportarTab2').click(function () {
        exportarGraficoAnual();

    });

    $('#btnExportarTab6').click(function () {
        exportarGraficoEstadisticasAnuales();
    });

    


    $('#btnDescargar').click(function () {
        charAnual.exportChart({
            type: 'image/jpeg',
            filename: 'GraficoAnual'
        });

    });

    $('#btnDescargarTab2').click(function () {
        charMensual.exportChart({
            type: 'image/jpeg',
            filename: 'GraficoMensual'
        });

    });

    obtenerCuenca();

});

function descargarImagenTab2() {
    charMensual.exportChart({
        type: 'image/jpeg',
        filename: 'GraficoMensual'
    });
}
function descargarImagenTab3() {
    charComparativaVolumen.exportChart({
        type: 'image/jpeg',
        filename: 'GraficoComparativaVolumen'
    });
}

function descargarImagenTab4() {
    charComparativaNaturalEvaporada.exportChart({
        type: 'image/jpeg',
        filename: 'GraficoComparativaNaturalEvaporada'
    });
}

function descargarImagenTab5() {
    charComparativaLineaTendencia.exportChart({
        type: 'image/jpeg',
        filename: 'GraficoComparativaLineaTendencia'
    });
}

function descargarImagenEstadisticasAnuales() {
    chartEstadisticasAnuales.exportChart({
        type: 'image/jpeg',
        filename: 'EstadisticasAnuales'
    });
}

function limpiarTab1() {
    $('#cbPuntoMedicion').prop('disabled', true);
    $('#cbPuntoMedicion').val('');
    $('#cbCuenca').val('');
    $('#btnConsultar').prop('disabled', true);
    $('#btnDescargar').prop('disabled', true);
}
function formatDateToMMYYYY(date) {
    const month = ("0" + (date.getMonth() + 1)).slice(-2); // Obtener el mes y agregar un 0 inicial si es necesario
    const year = date.getFullYear(); // Obtener el año
    return `${month}/${year}`;
}
function habilitarRangeFecha(elementTipoaño) {
    if (elementTipoaño.value) {
        // controles tab 1
        $('#txtFechaDesde').prop('disabled', false);
        $('#txtFechaHasta').prop('disabled', false); 
        $('#cbCuenca').prop('disabled', false);

        // controles tab 2
        $('#cbAnioTab2').prop('disabled', false);


        // controles tab 3
        if (document.getElementById('cbAnioTab3') != null) {
            $('#cbAnioTab3').multipleSelect('enable');
            $('#cbAnioTab3').prop('disabled', false);
        }

        // controles tab 4
        if (document.getElementById('cbAnioTab4') != null) {
            $('#cbAnioTab4').multipleSelect('enable');
            $('#cbAnioTab4').prop('disabled', false);
        }

        // controles tab 5
        $('#txtFechaDesdeTab5').prop('disabled', false);
        $('#txtFechaHastaTab5').prop('disabled', false);
        $('#cbCuencaTab5').prop('disabled', false);


        obtenerCuenca();

    } else {
        // controles tab 1
        $('#txtFechaDesde').prop('disabled', true);
        $('#txtFechaHasta').prop('disabled', true);
        $('#cbCuenca').prop('disabled', true);
        
        // controles tab 2
        $('#cbAnioTab2').prop('disabled', true);

        // controles tab 3
        $('#cbAnioTab3').prop('disabled', true);

        // controles tab 4
        $('#cbAnioTab4').multipleSelect('disable');

        // controles tab 5
        $('#txtFechaDesdeTab5').prop('disabled', true);
        $('#txtFechaHastaTab5').prop('disabled', true);
        $('#cbCuencaTab5').prop('disabled', true);
    }
}

function habilitarTipoPuntoMedicion(tipoPtoMedicion) {
    if (tipoPtoMedicion.value) {
        // controles tab 1

        $('#cbTipoPuntoMedicion').prop('disabled', false);

        // controles tab 2
        $('#cbTipoPuntoMedicionTab2').prop('disabled', false);

        //controles tab 3
        $('#cbTipoPuntoMedicionTab3').prop('disabled', false);

        $('#cbTipoPuntoMedicionTab4').prop('disabled', false);

        obtenerCuenca();


    } else {
        // controles tab 1

        $('#cbTipoPuntoMedicion').prop('disabled', true);
        $('#cbTipoPuntoMedicion').val('');

        // controles tab 2
        $('#cbTipoPuntoMedicionTab2').prop('disabled', true);
o
    }
}

    function habilitarCuenca(element){
        if (element.value) {
            $('#cbCuencaTab2').prop('disabled', false);

            //tab3
            $('#cbCuencaTab3').prop('disabled', false);

            //tab4
            $('#cbCuencaTab4').prop('disabled', false);


        } else {
            $('#cbCuencaTab2').prop('disabled', true);
            $('#cbCuencaTab2').val('');

            //tab3
            $('#cbCuencaTab3').prop('disabled', true);
            $('#cbCuencaTab3').val('');

            //tab4
            $('#cbCuencaTab4').prop('disabled', true);
            $('#cbCuencaTab4').val('');
        }
    }
function habilitarPtoMedicion(cuenca) {
    if (cuenca.value) {
        // controles tab 1
        $('#cbPuntoMedicion').prop('disabled', false);
        $('#btnConsultar').prop('disabled', true);
        $('#btnExportar').prop('disabled', true);
        $('#btnDescargar').prop('disabled', true);


        // controles tab 2
        $('#cbPuntoMedicionTab2').prop('disabled', false);
        $('#btnConsultarTab2').prop('disabled', true);
        $('#btnExportarTab2').prop('disabled', true);
        $('#btnDescargarTab2').prop('disabled', true);

        // controles tab 3
        $('#cbPuntoMedicionTab3').prop('disabled', false);
        $('#btnConsultarTab3').prop('disabled', true);
        $('#btnExportarTab3').prop('disabled', true);
        $('#btnDescargarTab3').prop('disabled', true);

        // controles tab 4
        $('#cbPuntoMedicionTab4').prop('disabled', false);
        $('#btnConsultarTab4').prop('disabled', true);
        $('#btnExportarTab4').prop('disabled', true);
        $('#btnDescargarTab4').prop('disabled', true);

        // controles tab 5
        if (document.getElementById('cbPuntoMedicionTab5') != null) { 
            $('#cbPuntoMedicionTab5').multipleSelect('enable');
            $('#cbPuntoMedicionTab5').prop('disabled', false);
        }
        $('#btnConsultarTab5').prop('disabled', true);
        $('#btnExportarTab5').prop('disabled', true);
        $('#btnDescargarTab5').prop('disabled', true);

    
        // controles tab 6
        $('#cbPuntoMedicionTab6').prop('disabled', false);
        $('#btnConsultarTab6').prop('disabled', true);
        $('#btnExportarTab6').prop('disabled', true);
        $('#btnDescargarTab6').prop('disabled', true);

        $('#cbEstadistico').val('');

        var cbPuntoMedicionTab4 = document.getElementById('cbPuntoMedicionTab4');
        if ($('#paso').val()=='4') {
            obtenerPtoMedicionNaturalEvaporado(cuenca.value)
        } else {
            obtenerPtoMedicion(cuenca.value)
        }
    } else {
        // controles tab 1
        $('#cbPuntoMedicion').prop('disabled', true);
        $('#cbPuntoMedicion').val('');

        // controles tab 2
        $('#cbPuntoMedicionTab2').prop('disabled', true);
        $('#cbPuntoMedicionTab2').val('');

        // controles tab 3
        $('#cbPuntoMedicionTab3').prop('disabled', true);
        $('#cbPuntoMedicionTab3').val('');

        // controles tab 4
        $('#cbPuntoMedicionTab4').prop('disabled', true);
        $('#cbPuntoMedicionTab4').val('');

        // controles tab 5
        $('#cbPuntoMedicionTab5').multipleSelect('disable');


        // controles tab 6
        $('#cbPuntoMedicionTab6').prop('disabled', true);
        $('#cbPuntoMedicionTab6').val('');
    }

}

function habilitarPtoMedicionPorTipo(TipoPToMedicion) {
    if (TipoPToMedicion.value) {
        document.getElementById('cbCuencaTab6').removeAttribute("disabled");
        document.getElementById('cbCuencaTab6').value = '';
        habilitarPtoMedicion(document.getElementById('cbCuencaTab6'));
    }
}

function habilitarBtnConsultar(PtoMedicion) {
    if (PtoMedicion.value) {
        // controles tab 1
        $('#btnConsultar').prop('disabled', false);


        // controles tab 2
        $('#btnConsultarTab2').prop('disabled', false);

        // controles tab 3
        $('#btnConsultarTab3').prop('disabled', false);


        // controles tab 4
        $('#btnConsultarTab4').prop('disabled', false);

        // controles tab 5
        $('#btnConsultarTab5').prop('disabled', false);

        // controles tab 6
        $('#btnConsultarTab6').prop('disabled', false);
        

    } else {
        // controles tab 1
        $('#btnConsultar').prop('disabled', true);
        $('#btnExportar').prop('disabled', true);


        // controles tab 2
        $('#btnConsultarTab2').prop('disabled', true);
        $('#btnExportarTab2').prop('disabled', true);

        // controles tab 3
        $('#btnConsultarTab3').prop('disabled', true);
        $('#btnExportarTab3').prop('disabled', true);

        // controles tab 4
        $('#btnConsultarTab4').prop('disabled', true);
        $('#btnExportarTab4').prop('disabled', true);

        // controles tab 5
        $('#btnConsultarTab5').prop('disabled', true);
        $('#btnExportarTab5').prop('disabled', true);
        // controles tab 6
        $('#btnConsultarTab6').prop('disabled', true);
        $('#btnExportarTab6').prop('disabled', true);

    }

}

function exportarExcelGraficoMensual() {

    var cbPuntoMedicion = document.getElementById("cbPuntoMedicionTab2");
    var selectedIndexcbPuntoMedicion = cbPuntoMedicion.selectedIndex;
    var selectedTextcbPuntoMedicion = cbPuntoMedicion.options[selectedIndexcbPuntoMedicion].text;

    var cbCuenca = document.getElementById("cbCuencaTab2");
    var selectedIndexcbCuenca = cbCuenca.selectedIndex;
    var selectedTextcbCuenca = cbCuenca.options[selectedIndexcbCuenca].text;

    const cuencaNombre = selectedTextcbCuenca
    var valorEmbalse = ''
    const ptomedicionNombre = selectedTextcbPuntoMedicion
    const currentYear = new Date().getFullYear();

    if ($('#cbTipoPuntoMedicionTab2').val() == 89) {
        valorEmbalse = 'Río'
    } else {
        valorEmbalse = 'Embalse'

    }
    $.ajax({
        type: 'POST',
        url: controlador + "exportarGraficoMensual",
        data: {
            tiposeriecodi: $('#cbTipoSerieTab2').val(),
            tptomedicodi: $('#cbTipoPuntoMedicionTab2').val(),
            ptomedicodi: $('#cbPuntoMedicionTab2').val(),
            aniofin: currentYear,
            cuencaNombre: cuencaNombre,
            valorEmbalse: valorEmbalse,
            ptomedicionNombre: ptomedicionNombre,
        },
        dataType: 'json',
        success: function (resultado) {

            if (resultado != 1) {
                mostrarError();
            }
            else {
                window.location = controlador + "ExportarReporteMensual";
            }

        },
        error: function () {
            mostrarError();
        }
    });
}
function habilitarBtnDescargar() {
    // controles tab 1
    $('#btnDescargar').prop('disabled', false);
    $('#btnExportar').prop('disabled', false);

    this.ObtenerGraficoAnual()
    
    // controles tab 2
    $('#btnDescargarTab2').prop('disabled', false);


}
function habilitarBtnDescargarMensual() {
    this.ObtenerGraficoMensual()
    // controles tab 2
    $('#btnDescargarTab2').prop('disabled', false);
    $('#btnExportarTab2').prop('disabled', false);

}
function habilitarBtnDescargarComparativaVolumen() {
    this.ObtenerGraficoComparativaVolumen()
    // controles tab 3
    $('#btnDescargarTab3').prop('disabled', false);
    $('#btnExportarTab3').prop('disabled', false);

}

function habilitarBtnDescargarComparativaNaturalEvaporada() {
    this.ObtenerGraficoComparativaNaturalEvaporada()
    // controles tab 4
    $('#btnDescargarTab4').prop('disabled', false);
    $('#btnExportarTab4').prop('disabled', false);
}
function habilitarBtnDescargarComparativaLineaTendencia() {
    this.ObtenerGraficoComparativaLineaTendencia()
    // controles tab 5
    $('#btnDescargarTab5').prop('disabled', false);
    $('#btnExportarTab5').prop('disabled', false);
}

function habilitarBtnDescargarEstadisticasAnuales() {
    this.ObtenerGraficoEstadisticasAnuales()
    // controles tab 6
    $('#btnDescargarTab6').prop('disabled', false);
    $('#btnExportarTab6').prop('disabled', false);
}

creacionGraficoMensual = function (aData, resultados) {
    const ValoresMaximo = [];
    const ValoresMinimo = [];
    const ValoresPromedio = [];

    for (const mes in resultados) {
        if (resultados.hasOwnProperty(mes)) {
            ValoresMaximo.push(resultados[mes].maximo);
            ValoresMinimo.push(resultados[mes].minimo);
            ValoresPromedio.push(resultados[mes].promedio);
        }
    }

    var select = document.getElementById("cbTipoAnio2");
    var valorSeleccionado = select.options[select.selectedIndex].value;
    var selectPtoMed = document.getElementById("cbPuntoMedicionTab2");
    var valorPtoMedSeleccionado = selectPtoMed.options[selectPtoMed.selectedIndex].text;

    var selectTipoPtoMed = document.getElementById("cbTipoPuntoMedicionTab2").value;
    var hiddenTipoPtoMed = document.getElementById("hdfTipoPtoMedDescTab2").value;
    var arrayTipoPtoMed = hiddenTipoPtoMed.split("|");
    var descTipoPtoMed = "";
    for (i = 0; i <= arrayTipoPtoMed.length; i++) {
        if (arrayTipoPtoMed[i]) {
            strTipoPtoMed = arrayTipoPtoMed[i];
            var arrayDescTipoPtoMed = strTipoPtoMed.split(":");
            if (arrayDescTipoPtoMed[0] == selectTipoPtoMed) {
                descTipoPtoMed = arrayDescTipoPtoMed[1];
            }
        }
    }

    var seriesData = [];
    var categories = [];
    if (valorSeleccionado == 2) {
        seriesData = aData.ListaGraficoMensual.map(item => {
            return {
                name: item.Anio.toString(),
                type: 'column',
                yAxis: 1,
                data: [
                    item.M1, item.M2, item.M3, item.M4, item.M5, item.M6,
                    item.M7, item.M8, item.M9, item.M10, item.M11, item.M12
                ],
                tooltip: {
                    valueSuffix: ''
                }
            };
        });

        categories = [
            'ENE', 'FEB', 'MAR', 'ABR', 'MAY', 'JUN',
            'JUL', 'AGO', 'SEP', 'OCT', 'NOV', 'DIC'
        ];
    } else if (valorSeleccionado == 1) {
        var anioVig = aData.ListaGraficoMensual[0].Anio;
        var anioVigSgte = parseInt(anioVig) + 1;
        seriesData = aData.ListaGraficoMensual.map(item => {
            var anioActual = item.Anio;
            var anioSgte = parseInt(item.Anio) + 1;
            var itemSgte = aData.ListaGraficoMensual.find(ele => ele.Anio == anioSgte);
            var dataM1 = "";
            var dataM2 = "";
            var dataM3 = "";
            var dataM4 = "";
            var dataM5 = "";
            var dataM6 = "";
            var dataM7 = "";
            var dataM8 = "";
            if (itemSgte) {
                dataM1 = itemSgte.M1;
                dataM2 = itemSgte.M2;
                dataM3 = itemSgte.M3;
                dataM4 = itemSgte.M4;
                dataM5 = itemSgte.M5;
                dataM6 = itemSgte.M6;
                dataM7 = itemSgte.M7;
                dataM8 = itemSgte.M8;
            }

            return {
                name: descTipoPtoMed,
                type: 'column',
                yAxis: 1,
                data: [
                    item.M9, item.M10, item.M11, item.M12, dataM1, dataM2,
                    dataM3, dataM4, dataM5, dataM6, dataM7, dataM8
                ],
                tooltip: {
                    valueSuffix: ''
                }
            };
        });
        categories = [
            'SEP-'+ anioVig.toString(), 'OCT-' + anioVig.toString(), 'NOV-' + anioVig.toString(), 'DIC-' + anioVig.toString(), 'ENE-' + anioVigSgte.toString(), 'FEB-' + anioVigSgte.toString(),
            'MAR-' + anioVigSgte.toString(), 'ABR-' + anioVigSgte.toString(), 'MAY-' + anioVigSgte.toString(), 'JUN-' + anioVigSgte.toString(), 'JUL-' + anioVigSgte.toString(), 'AGO-' + anioVigSgte.toString()
        ];
    }

    
    charMensual = Highcharts.chart('graficoMensual', {
    chart: {
        zooming: {
            type: 'xy'
        }
    },
    title: {
        text: valorPtoMedSeleccionado,
         align: 'center'
    },
  
    xAxis: [{
        categories: categories,
        crosshair: true
    }],
        yAxis: [{ 
            title: {
                text: '',
                style: {
                    color: Highcharts.getOptions().colors[1]
                }
            },
            opposite: true

        }, { // Secondary yAxis
            title: {
                text: descTipoPtoMed,
                style: {
                    color: Highcharts.getOptions().colors[0]
                }
            },
           
            opposite: false
        }],
            tooltip: {
        shared: true
    },
    legend: {
        align: 'left',
            x: 80,
                verticalAlign: 'top',
                    y: 60,
                        floating: true,
                            backgroundColor:
        Highcharts.defaultOptions.legend.backgroundColor || // theme
            'rgba(255,255,255,0.25)'
    },
        series: [
            seriesData[0],
        {
        name: 'Máximo',
        type: 'spline',
        yAxis: 1,
        data: ValoresMaximo,
        tooltip: {
            valueSuffix: ''
        }
            },
            {
        name: 'Mínimo',
        type: 'spline',
        yAxis: 1,
        data: ValoresMinimo,
        tooltip: {
            valueSuffix: ''
        }
            },
            {
        name: 'Promedio',
        type: 'spline',
        yAxis: 1,
        data: ValoresPromedio,
        tooltip: {
            valueSuffix: ''
        }
            }

        ]
    });
}
   
creacionGraficoAnual = function (aData) {
    var select = document.getElementById("cbAnio");
    var selectPtoMed = document.getElementById("cbPuntoMedicion");
    var valorSeleccionado = select.options[select.selectedIndex].value;
    var valorPtoMedSeleccionado = selectPtoMed.options[selectPtoMed.selectedIndex].text;
    var selectTipoPtoMed = document.getElementById("cbTipoPuntoMedicion").value;
    var hiddenTipoPtoMed = document.getElementById("hdfTipoPtoMedDesc").value;
    var arrayTipoPtoMed = hiddenTipoPtoMed.split("|");
    var descTipoPtoMed = "";
    for (i = 0; i <= arrayTipoPtoMed.length; i++) {
        if (arrayTipoPtoMed[i]) {
            strTipoPtoMed = arrayTipoPtoMed[i];
            var arrayDescTipoPtoMed = strTipoPtoMed.split(":");
            if (arrayDescTipoPtoMed[0] == selectTipoPtoMed) {
                descTipoPtoMed = arrayDescTipoPtoMed[1];
            }
        } 
    }
    var seriesData = [];
    var categories = [];
    if (valorSeleccionado == 2) {
        seriesData = aData.map(item => {
            return {
                name: item.Anio.toString(),
                data: [
                    item.M1, item.M2, item.M3, item.M4, item.M5, item.M6,
                    item.M7, item.M8, item.M9, item.M10, item.M11, item.M12
                ]
            };
        });

        categories = [
            'ENE', 'FEB', 'MAR', 'ABR', 'MAY', 'JUN',
            'JUL', 'AGO', 'SEP', 'OCT', 'NOV', 'DIC'
        ];
    } else if (valorSeleccionado == 1) {
        seriesData = aData.map(item => {
            var anioActual = item.Anio;
            var anioSgte = parseInt(item.Anio) + 1;
            var itemSgte = aData.find(ele => ele.Anio == anioSgte);
            var dataM1 = "";
            var dataM2 = "";
            var dataM3 = "";
            var dataM4 = "";
            var dataM5 = "";
            var dataM6 = "";
            var dataM7 = "";
            var dataM8 = "";
            if (itemSgte) {
                dataM1 = itemSgte.M1;
                dataM2 = itemSgte.M2;
                dataM3 = itemSgte.M3;
                dataM4 = itemSgte.M4;
                dataM5 = itemSgte.M5;
                dataM6 = itemSgte.M6;
                dataM7 = itemSgte.M7;
                dataM8 = itemSgte.M8;
            }

            return {
                name: item.Anio.toString(),
                data: [
                    item.M9, item.M10, item.M11, item.M12, dataM1, dataM2,
                    dataM3, dataM4, dataM5, dataM6, dataM7, dataM8
                ]
            };
        });
        categories = [
            'SEP', 'OCT', 'NOV', 'DIC', 'ENE', 'FEB',
            'MAR', 'ABR', 'MAY', 'JUN', 'JUL', 'AGO'
        ];
    }

   

    


    charAnual = Highcharts.chart('graficoAnual', {

        title: {
            text: valorPtoMedSeleccionado,
            align: 'center'
        },
        yAxis: {
            title: {
                text: descTipoPtoMed
            }
        },

        xAxis: {
            accessibility: {
                rangeDescription: 'Rango: ENE to DIC'
            },
            categories: categories ,
        },

        legend: {
            layout: 'vertical',
            align: 'right',
            verticalAlign: 'middle'
        },

        plotOptions: {
            series: {
                label: {
                    connectorAllowed: false
                },
               // pointStart: 2010
            }
        },

        series: seriesData,

        responsive: {
            rules: [{
                condition: {
                    maxWidth: 500
                },
                chartOptions: {
                    legend: {
                        layout: 'horizontal',
                        align: 'center',
                        verticalAlign: 'bottom'
                    }
                }
            }]
        }, exporting: {
            enabled: false // Deshabilitar el icono de opciones del chart
        }

    });

}
obtenerPtoMedicion = function (cuenca){
    var cbTipoPuntoMedicionTab3 = document.getElementById('cbTipoPuntoMedicionTab3');
    var cbTipoPuntoMedicionTab2 = document.getElementById('cbTipoPuntoMedicionTab2');
    var cbTipoPuntoMedicion = document.getElementById('cbTipoPuntoMedicion');  
    var cbTipoPuntoMedicionTab5 = document.getElementById('cbTipoPuntoMedicionTab5');
    var cbTipoPuntoMedicionTab6 = document.getElementById('cbTipoPuntoMedicionTab6');
    var hdfPaso = document.getElementById('paso').value;
    
    var valTPtoMediCodi = 0;
    if (hdfPaso=='3') {
        valTPtoMediCodi = $('#cbTipoPuntoMedicionTab3').val();
    } else if (hdfPaso=='2') {
        valTPtoMediCodi = $('#cbTipoPuntoMedicionTab2').val();
    } else if (hdfPaso=='5') {
        valTPtoMediCodi = $('#cbTipoPuntoMedicionTab5').val();
    } else if (hdfPaso=='6') {
        valTPtoMediCodi = $('#cbTipoPuntoMedicionTab6').val();
    } else if (hdfPaso=='1') {
        valTPtoMediCodi = $('#cbTipoPuntoMedicion').val();
    }
    $.ajax({
        type: 'POST',
        url: controlador + 'CargarPtoMedicion',
        dataType: 'json',
        data: {
            cuenca: cuenca,
            tptomedicodi: valTPtoMediCodi
        },
        cache: false,
        global: false,
        success: function (aData) {
            var cbPuntoMedicionTab2 = document.getElementById('cbPuntoMedicionTab2');
            var cbPuntoMedicionTab3 = document.getElementById('cbPuntoMedicionTab3');
            var cbPuntoMedicionTab4 = document.getElementById('cbPuntoMedicionTab4');
            var cbPuntoMedicionTab5 = document.getElementById('cbPuntoMedicionTab5');

            var cbPuntoMedicionTab6 = document.getElementById('cbPuntoMedicionTab6');



            $('#cbPuntoMedicion').get(0).options.length = 0;
            $('#cbPuntoMedicion').get(0).options[0] = new Option("--SELECCIONE--", "");
            if (cbPuntoMedicionTab2) {
                $('#cbPuntoMedicionTab2').get(0).options.length = 0;
                $('#cbPuntoMedicionTab2').get(0).options[0] = new Option("--SELECCIONE--", "");
            }

            if (cbPuntoMedicionTab3) {
                $('#cbPuntoMedicionTab3').get(0).options.length = 0;
                $('#cbPuntoMedicionTab3').get(0).options[0] = new Option("--SELECCIONE--", "");
            }
            if (cbPuntoMedicionTab4) {
                $('#cbPuntoMedicionTab4').get(0).options.length = 0;
                $('#cbPuntoMedicionTab4').get(0).options[0] = new Option("--SELECCIONE--", "");
            }

            if (cbPuntoMedicionTab5) {
                $("#cbPuntoMedicionTab5").empty();
            }

            if (cbPuntoMedicionTab6) {
                $('#cbPuntoMedicionTab6').get(0).options.length = 0;
                $('#cbPuntoMedicionTab6').get(0).options[0] = new Option("--SELECCIONE--", "");
            }

            $.each(aData, function (i, item) {
                if (cbPuntoMedicion) {
                    $('#cbPuntoMedicion').get(0).options[$('#cbPuntoMedicion').get(0).options.length] = new Option(item.Text, item.Value);

                }
                if (cbPuntoMedicionTab2) {
                    $('#cbPuntoMedicionTab2').get(0).options[$('#cbPuntoMedicionTab2').get(0).options.length] = new Option(item.Text, item.Value);

                }
                if (cbPuntoMedicionTab3) {
                    $('#cbPuntoMedicionTab3').get(0).options[$('#cbPuntoMedicionTab3').get(0).options.length] = new Option(item.Text, item.Value);

                }
                if (cbPuntoMedicionTab4) {
                    $('#cbPuntoMedicionTab4').get(0).options[$('#cbPuntoMedicionTab4').get(0).options.length] = new Option(item.Text, item.Value);

                }
                if (cbPuntoMedicionTab5) {
                    var option = '<option value =' + item.Value + '>' + item.Text + '</option>';
                    $('#cbPuntoMedicionTab5').append(option);
                }
                if (cbPuntoMedicionTab6) {
                    $('#cbPuntoMedicionTab6').get(0).options[$('#cbPuntoMedicionTab6').get(0).options.length] = new Option(item.Text, item.Value);

                }
            });

            if (cbPuntoMedicionTab5) {
                $('#cbPuntoMedicionTab5').multipleSelect({
                    filter: true,
                    placeholder: "SELECCIONE"
                });
                $("#cbPuntoMedicionTab5").multipleSelect("refresh");
            }
         
        },
        error: function () {
            alert("Ha ocurrido un error");
        }
    });
}
obtenerPtoMedicionNaturalEvaporado = function (cuenca) {
$.ajax({
        type: 'POST',
        url: controlador + 'CargarPtoMedicionNaturalEvaporado',
        dataType: 'json',
        data: {
            cuenca: cuenca,
            tipopuntomedicion: $("#cbTipoPuntoMedicionTab4").val()
        },
        cache: false,
        global: false,
        success: function (aData) {

            var cbPuntoMedicionTab4 = document.getElementById('cbPuntoMedicionTab4');

            if (cbPuntoMedicionTab4) {
                $('#cbPuntoMedicionTab4').get(0).options.length = 0;
                $('#cbPuntoMedicionTab4').get(0).options[0] = new Option("--SELECCIONE--", "");
            }
            $.each(aData, function (i, item) {
                if (cbPuntoMedicionTab4) {
                    $('#cbPuntoMedicionTab4').get(0).options[$('#cbPuntoMedicionTab4').get(0).options.length] = new Option(item.Text, item.Value);
                }
            });
        },
        error: function () {
            alert("Ha ocurrido un error");
        }
    });
}
ObtenerGraficoAnual = function () {
    var self = this;
    // Variables de entrada
    const fechaInicioStr = $('#txtFechaDesde').val();
    const fechaFinStr = $('#txtFechaHasta').val();

    // Convertir las variables de entrada a objetos Date
    const [inicioMes, inicioAnio] = fechaInicioStr.split('/').map(Number);
    const [finMes, finAnio] = fechaFinStr.split('/').map(Number);
    $.ajax({
        type: 'POST',
        url: controlador + 'ObtenerGraficoAnual',
        dataType: 'json',
        data: {
            tiposeriecodi: $('#cbTipoSerie').val(),
            tptomedicodi: $('#cbTipoPuntoMedicion').val(),
            ptomedicodi: $('#cbPuntoMedicion').val(),
            anioinicio: inicioAnio,
            aniofin: finAnio
        },
        cache: false,
        global: false,
        success: function (aData) {
            if (aData.length == 0) {
                document.getElementById("graficoAnual").style.display = "none";
                $('#btnExportar').prop('disabled', true);
                $('#btnDescargar').prop('disabled', true);

                alert("No existe información en base de datos para el filtro seleccionado")

            } else {
                document.getElementById("graficoAnual").style.display = "block";

                // Variables de entrada
                const fechaInicioStr = $('#txtFechaDesde').val();
                const fechaFinStr = $('#txtFechaHasta').val();

                // Convertir las variables de entrada a objetos Date
                const [inicioMes, inicioAnio] = fechaInicioStr.split('/').map(Number);
                const [finMes, finAnio] = fechaFinStr.split('/').map(Number);

                const fechainicio = new Date(inicioAnio, inicioMes - 1); // Meses en JavaScript van de 0 a 11
                const fechafin = new Date(finAnio, finMes - 1);

                // Recorrer los datos y blanquear los valores fuera del rango
                aData.forEach(item => {
                    const anio = item.Anio;

                    for (let month = 1; month <= 12; month++) {
                        const monthKey = `M${month}`;
                        const currentDate = new Date(anio, month - 1);

                        // Condición para blanquear las variables M si la fecha no está dentro del rango
                        if (currentDate < fechainicio || currentDate > fechafin) {
                            item[monthKey] = null;
                        }
                    }
                });

                self.creacionGraficoAnual(aData)

            }
        },
        error: function () {
            alert("Ha ocurrido un error");
        }
    });
}
ObtenerGraficoMensual = function () {
    var self = this;
    const currentYear = new Date().getFullYear();
    const inicioAnio = $('#cbAnioTab2').val()  
    $.ajax({
        type: 'POST',
        url: controlador + 'ObtenerGraficoMensual',
        dataType: 'json',
        data: {
            tiposeriecodi: $('#cbTipoSerieTab2').val(),
            tptomedicodi: $('#cbTipoPuntoMedicionTab2').val(),
            ptomedicodi: $('#cbPuntoMedicionTab2').val(),
            anioinicio: inicioAnio,
            aniofin: currentYear
        },
        cache: false,
        global: false,
        success: function (aData) {
            if (aData.ListaGraficoMensual.length == 0) {
                document.getElementById("graficoMensual").style.display = "none";
                $('#btnExportarTab2').prop('disabled', true);
                $('#btnDescargarTab2').prop('disabled', true);

                alert("No existe información en base de datos para el filtro seleccionado")

            } else {
                document.getElementById("graficoMensual").style.display = "block";

                aData.ListaGraficoMensual.forEach(item => {
                    // Extraer los valores de los meses en un array
                    let valores = [
                        item.M1, item.M2, item.M3, item.M4, item.M5, item.M6,
                        item.M7, item.M8, item.M9, item.M10, item.M11, item.M12
                    ];

                    // Calcular el valor máximo
                    let max = Math.max(...valores);

                    // Calcular el valor mínimo
                    let min = Math.min(...valores);

                    // Calcular el valor promedio
                    let avg = valores.reduce((sum, value) => sum + value, 0) / valores.length;
                    
                    // Agregar los valores calculados al objeto
                    item.valorMaximo = max;
                    item.valorMinimo = min;
                    item.valorPromedio = parseFloat(avg.toFixed(3));
                });
                var select = document.getElementById("cbTipoAnio2");
                var valorSeleccionado = select.options[select.selectedIndex].value;

                var meses = [];
                if (valorSeleccionado == 2) {
                    meses = ["M1", "M2", "M3", "M4", "M5", "M6", "M7", "M8", "M9", "M10", "M11", "M12"];

                } else if (valorSeleccionado == 1) {
                    meses = ["M9", "M10", "M11", "M12", "M1", "M2", "M3", "M4", "M5", "M6", "M7", "M8"];

                }

                const resultados = {};

                meses.forEach(mes => {
                    const valores = aData.ListaGraficoTotal.map(item => item[mes]);
                    const max = Math.max(...valores);
                    const min = Math.min(...valores);
                    const avg = valores.reduce((sum, val) => sum + val, 0) / valores.length;
                    
                    resultados[mes] = {
                        maximo: max,
                        minimo: min,
                        promedio: parseFloat(avg.toFixed(3))
                    };
                });

                self.creacionGraficoMensual(aData, resultados)
            }
           

        },
        error: function () {
            alert("Ha ocurrido un error");
        }
    });
}

ObtenerGraficoComparativaVolumen = function () {
    var self = this;
    const currentYear = new Date().getFullYear();
    var inicioAnio = $('#cbAnioTab3').val(); 
    var cbcbAnioTab3Values = $('#cbAnioTab3').multipleSelect('getSelects');
    $('#hfAnioTab3').val(cbcbAnioTab3Values);

    $.ajax({
        type: 'POST',
        url: controlador + 'ObtenerGraficoComparativaVolumen',
        dataType: 'json',
        data: {
            tiposeriecodi: $('#cbTipoSerieTab3').val(),
            tptomedicodi: $('#cbTipoPuntoMedicionTab3').val(),
            ptomedicodi: $('#cbPuntoMedicionTab3').val(),
            anios: $('#hfAnioTab3').val(),
            aniofin: currentYear
        },
        cache: false,
        global: false,
        success: function (aData) {
            if (aData.ListaGraficoMensual.length == 0) {
                document.getElementById("graficoComparativaVolumen").style.display = "none";
                $('#btnExportarTab3').prop('disabled', true);
                $('#btnDescargarTab3').prop('disabled', true);

                alert("No existe información en base de datos para el filtro seleccionado")

            } else {

                codPuntoMedicion = $("#cbPuntoMedicionTab3").val();
                $.ajax({
                    type: 'POST',
                    url: controlador + "ListarInformacionPuntoMedicionPorEmpresa",
                    dataType: 'json',
                    async: false,
                    data: {
                        CodPuntoMedicion: codPuntoMedicion
                    },
                    success: function (datos) {
                        //Mostrar Informacion Grafico
                        var CapUtil = datos.Capacidad;
                        document.getElementById("graficoComparativaVolumen").style.display = "block";

                        aData.ListaGraficoMensual.forEach(item => {
                            // Extraer los valores de los meses en un array
                            let valores = [
                                item.M1, item.M2, item.M3, item.M4, item.M5, item.M6,
                                item.M7, item.M8, item.M9, item.M10, item.M11, item.M12
                            ];

                            // Calcular el valor máximo
                            let max = Math.max(...valores);

                            // Calcular el valor mínimo
                            let min = Math.min(...valores);

                            // Calcular el valor promedio
                            let avg = valores.reduce((sum, value) => sum + value, 0) / valores.length;

                            // Agregar los valores calculados al objeto
                            item.valorMaximo = max;
                            item.valorMinimo = min;
                            item.valorPromedio = parseFloat(avg.toFixed(3));
                        });
                        var select = document.getElementById("cbTipoAnio3");
                        var valorSeleccionado = select.options[select.selectedIndex].value;

                        var meses = [];
                        if (valorSeleccionado == 2) {
                            meses = ["M1", "M2", "M3", "M4", "M5", "M6", "M7", "M8", "M9", "M10", "M11", "M12"];

                        } else if (valorSeleccionado == 1) {
                            meses = ["M9", "M10", "M11", "M12", "M1", "M2", "M3", "M4", "M5", "M6", "M7", "M8"];

                        }

                        const resultados = {};

                        meses.forEach(mes => {
                            const valores = aData.ListaGraficoTotal.map(item => item[mes]);
                            const max = Math.max(...valores);
                            const min = Math.min(...valores);
                            const avg = valores.reduce((sum, val) => sum + val, 0) / valores.length;

                            resultados[mes] = {
                                maximo: max,
                                minimo: min,
                                promedio: parseFloat(avg.toFixed(3))
                            };
                        });

                        self.creacionGraficoComparativaVolumen(aData, resultados, CapUtil);
                        //Fin Mostrar Informacion Grafico
                    },
                    error: function () {
                        alert("Error al cargar Informacion de Punto de Medición.");
                    }
                });

                

            }


        },
        error: function () {
            alert("Ha ocurrido un error");
        }
    });
}

ObtenerGraficoComparativaNaturalEvaporada = function () {
    var self = this;
    const currentYear = new Date().getFullYear();
    var anio = $('#cbAnioTab4').multipleSelect('getSelects');
    $('#hfAnioComparacion').val(anio);
    $.ajax({
        type: 'POST',
        url: controlador + 'ObtenerGraficoComparativaNaturalEvaporada',
    // dataType: 'json',
        data: {
            tiposeriecodi: $('#cbTipoSerieTab4').val(),
            ptomedicodi: $('#cbPuntoMedicionTab4').val(),
            anioinicio: $('#hfAnioComparacion').val(),
            aniofin: currentYear
        },
        cache: false,
        global: false,
        success: function (aData) {
            if (aData.ListaGraficoMensual.length == 0) {
                document.getElementById("graficoComparativaNaturalEvaporada").style.display = "none";
                $('#btnExportarTab4').prop('disabled', true);
                $('#btnDescargarTab4').prop('disabled', true);
                alert("No existe información en base de datos para el filtro seleccionado")
            } else {
                document.getElementById("graficoComparativaNaturalEvaporada").style.display = "block";
                aData.ListaGraficoMensual.forEach(item => {
                    // Extraer los valores de los meses en un array
                    let valores = [
                        item.M1, item.M2, item.M3, item.M4, item.M5, item.M6,
                        item.M7, item.M8, item.M9, item.M10, item.M11, item.M12
                    ];

                    // Calcular el valor máximo
                    let max = Math.max(...valores);

                    // Calcular el valor mínimo
                    let min = Math.min(...valores);

                    // Calcular el valor promedio
                    let avg = valores.reduce((sum, value) => sum + value, 0) / valores.length;
                    
                    // Agregar los valores calculados al objeto
                    item.valorMaximo = max;
                    item.valorMinimo = min;
                    item.valorPromedio = parseFloat(avg.toFixed(3));
                });
                var select = document.getElementById("cbTipoAnio4");
                var valorSeleccionado = select.options[select.selectedIndex].value;

                var meses = [];
                if (valorSeleccionado == 2) {
                    meses = ["M1", "M2", "M3", "M4", "M5", "M6", "M7", "M8", "M9", "M10", "M11", "M12"];

                } else if (valorSeleccionado == 1) {
                    meses = ["M9", "M10", "M11", "M12", "M1", "M2", "M3", "M4", "M5", "M6", "M7", "M8"];

                }

                const resultados = {};

                meses.forEach(mes => {
                    const valores = aData.ListaGraficoTotal.map(item => item[mes]);
                    const max = Math.max(...valores);
                    const min = Math.min(...valores);
                    const avg = valores.reduce((sum, val) => sum + val, 0) / valores.length;
                    resultados[mes] = {
                        maximo: max,
                        minimo: min,
                        promedio: parseFloat(avg.toFixed(3))
                    };
                });

                self.creacionGraficoComparativaNaturalEvaporada(aData, resultados)
            }


        },
        error: function () {
            alert("Ha ocurrido un error");
        }
    });
}

ObtenerGraficoComparativaLineaTendencia = function () {
    var self = this;
    const fechaInicioStr = $('#txtFechaDesdeTab5').val();
    const fechaFinStr = $('#txtFechaHastaTab5').val();

    // Convertir las variables de entrada a objetos Date
    const [inicioMes, inicioAnio] = fechaInicioStr.split('/').map(Number);
    const [finMes, finAnio] = fechaFinStr.split('/').map(Number);
    const currentYear = new Date().getFullYear();
    var cbPuntoMedicionTab5 = $('#cbPuntoMedicionTab5').multipleSelect('getSelects');
    $('#hfPuntoMedicionTab5').val(cbPuntoMedicionTab5);
    $.ajax({
        type: 'POST',
        url: controlador + 'ObtenerGraficoLineaTendencia',
        // dataType: 'json',
        data: {
            tiposeriecodi: $('#cbTipoSerieTab5').val(),
            ptomedicodi: $('#hfPuntoMedicionTab5').val(),
            tptomedicodi: $('#cbTipoPuntoMedicionTab5').val(),
            anioinicio: inicioAnio,
            aniofin: finAnio
        },
        cache: false,
        global: false,
        success: function (aData) {
            if (aData.ListaGraficoMensual.length == 0) {
                document.getElementById("graficoComparativaLineaTendencia").style.display = "none";
                $('#btnExportarTab5').prop('disabled', true);
                $('#btnDescargarTab5').prop('disabled', true);
                alert("No existe información en base de datos para el filtro seleccionado")
            } else {
                document.getElementById("graficoComparativaLineaTendencia").style.display = "block";
                aData.ListaGraficoMensual.forEach(item => {
                    // Extraer los valores de los meses en un array
                    let valores = [
                        item.M1, item.M2, item.M3, item.M4, item.M5, item.M6,
                        item.M7, item.M8, item.M9, item.M10, item.M11, item.M12
                    ];

                    // Calcular el valor máximo
                    let max = Math.max(...valores);

                    // Calcular el valor mínimo
                    let min = Math.min(...valores);

                    // Calcular el valor promedio
                    let avg = valores.reduce((sum, value) => sum + value, 0) / valores.length;
                    
                    // Agregar los valores calculados al objeto
                    item.valorMaximo = max;
                    item.valorMinimo = min;
                    item.valorPromedio = parseFloat(avg.toFixed(3));
                });
               

                var meses = ["M1", "M2", "M3", "M4", "M5", "M6", "M7", "M8", "M9", "M10", "M11", "M12"];
            

                const resultados = {};

                meses.forEach(mes => {
                    const valores = aData.ListaGraficoMensual.map(item => item[mes]);
                    const max = Math.max(...valores);
                    const min = Math.min(...valores);
                    const avg = valores.reduce((sum, val) => sum + val, 0) / valores.length;
                    
                    resultados[mes] = {
                        maximo: max,
                        minimo: min,
                        promedio: parseFloat(avg.toFixed(3))
                    };
                });

                self.creacionGraficoComparativaLineaTendencia(aData, resultados)
            }


        },
        error: function () {
            alert("Ha ocurrido un error");
        }
    });
}

ObtenerGraficoEstadisticasAnuales = function () {
    var self = this;
    const currentYear = new Date().getFullYear();
    const inicioAnio = $('#cbAnioTab6').val();
    const arrayFechaInicio = $('#txtFechaDesdeTab6').val().split("/");
    const arrayFechaFin = $('#txtFechaHastaTab6').val().split("/");
    const anioInicio = arrayFechaInicio[1];
    const mesInicio = arrayFechaInicio[0];
    const anioFin = arrayFechaFin[1];
    const mesFin = arrayFechaFin[0];
    $.ajax({
        type: 'POST',
        url: controlador + 'ObtenerGraficoEstadisticasAnuales',
        dataType: 'json',
        data: {
            tiposeriecodi: $('#cbTipoSerieTab6').val(),
            tptomedicodi: $('#cbTipoPuntoMedicionTab6').val(),
            ptomedicodi: $('#cbPuntoMedicionTab6').val(),
            anioinicio: anioInicio,
            mesinicio: mesInicio,
            aniofin: anioFin,
            mesfin: mesFin
        },
        cache: false,
        global: false,
        success: function (aData) {
            const resultados = {};
           
            if (aData.ListaGraficoMensual.length == 0) {
                document.getElementById("graficoEstadisticasAnuales").style.display = "none";
                $('#btnExportarTab6').prop('disabled', true);
                $('#btnDescargarTab6').prop('disabled', true);
                alert("No existe información en base de datos para el filtro seleccionado.")
            } else {
                document.getElementById("graficoEstadisticasAnuales").style.display = "block";
                self.creacionGraficoEstadisticasAnuales(aData, resultados);
            }

        },
        error: function () {
            alert("Ha ocurrido un error");
        }
    });
}

obtenerCuenca = function () {
    $.ajax({
        type: 'POST',
        url: controlador + 'CargarEquiposCuencaHidro',
        dataType: 'json',
        data: {
        },
        cache: false,
        global: false,
        success: function (aData) {
            var cbCuenca = document.getElementById('cbCuenca');

            var cbCuencaTab2 = document.getElementById('cbCuencaTab2');
            var cbCuencaTab3 = document.getElementById('cbCuencaTab3');
            var cbCuencaTab4 = document.getElementById('cbCuencaTab4');
            var cbCuencaTab5 = document.getElementById('cbCuencaTab5');

            var cbPuntoMedicion = document.getElementById('cbPuntoMedicion');
            var cbPuntoMedicionTab2 = document.getElementById('cbPuntoMedicionTab2');
            var cbPuntoMedicionTab3 = document.getElementById('cbPuntoMedicionTab3');
            var cbPuntoMedicionTab4 = document.getElementById('cbPuntoMedicionTab4');
            var cbPuntoMedicionTab5 = document.getElementById('cbPuntoMedicionTab5');

            if (cbCuenca) {
                $('#cbCuenca').get(0).options.length = 0;
                $('#cbCuenca').get(0).options[0] = new Option("--SELECCIONE--", "");
            }          

            if (cbCuencaTab2) {
                $('#cbCuencaTab2').get(0).options.length = 0;
                $('#cbCuencaTab2').get(0).options[0] = new Option("--SELECCIONE--", "");
            }

            if (cbCuencaTab3) {
                $('#cbCuencaTab3').get(0).options.length = 0;
                $('#cbCuencaTab3').get(0).options[0] = new Option("--SELECCIONE--", "");
            }

            if (cbCuencaTab4) {
                $('#cbCuencaTab4').get(0).options.length = 0;
                $('#cbCuencaTab4').get(0).options[0] = new Option("--SELECCIONE--", "");
            }

            if (cbCuencaTab5) {
                $('#cbCuencaTab5').get(0).options.length = 0;
                $('#cbCuencaTab5').get(0).options[0] = new Option("--SELECCIONE--", "");
            }

            if (cbPuntoMedicion) {
                $('#cbPuntoMedicion').get(0).options.length = 0;
                $('#cbPuntoMedicion').get(0).options[0] = new Option("--SELECCIONE--", "");
            } 

            if (cbPuntoMedicionTab2) {
                $('#cbPuntoMedicionTab2').get(0).options.length = 0;
                $('#cbPuntoMedicionTab2').get(0).options[0] = new Option("--SELECCIONE--", "");
            }

            if (cbPuntoMedicionTab3) {
                $('#cbPuntoMedicionTab3').get(0).options.length = 0;
                $('#cbPuntoMedicionTab3').get(0).options[0] = new Option("--SELECCIONE--", "");
            }

            if (cbPuntoMedicionTab4) {
                $('#cbPuntoMedicionTab4').get(0).options.length = 0;
                $('#cbPuntoMedicionTab4').get(0).options[0] = new Option("--SELECCIONE--", "");
            }

            if (cbPuntoMedicionTab5) {
                $('#cbPuntoMedicionTab5').get(0).options.length = 0;
                $('#cbPuntoMedicionTab5').get(0).options[0] = new Option("--SELECCIONE--", "");
            }

            
            $.each(aData, function (i, item) {
                if (cbCuenca) {
                    $('#cbCuenca').get(0).options[$('#cbCuenca').get(0).options.length] = new Option(item.Text, item.Value);

                }
                if (cbCuencaTab2) {
                    $('#cbCuencaTab2').get(0).options[$('#cbCuencaTab2').get(0).options.length] = new Option(item.Text, item.Value);

                }

                if (cbCuencaTab3) {
                    $('#cbCuencaTab3').get(0).options[$('#cbCuencaTab3').get(0).options.length] = new Option(item.Text, item.Value);

                }
                if (cbCuencaTab4) {
                    $('#cbCuencaTab4').get(0).options[$('#cbCuencaTab4').get(0).options.length] = new Option(item.Text, item.Value);

                }
                if (cbCuencaTab5) {
                    $('#cbCuencaTab5').get(0).options[$('#cbCuencaTab5').get(0).options.length] = new Option(item.Text, item.Value);
                }

            });
        },
        error: function () {
            alert("Ha ocurrido un error");
        }
    });
}

exportarGraficoAnual = function () {

    var cbPuntoMedicion = document.getElementById("cbPuntoMedicion");
    var selectedIndexcbPuntoMedicion = cbPuntoMedicion.selectedIndex;
    var selectedTextcbPuntoMedicion = cbPuntoMedicion.options[selectedIndexcbPuntoMedicion].text;

    var cbCuenca = document.getElementById("cbCuenca");
    var selectedIndexcbCuenca = cbCuenca.selectedIndex;
    var selectedTextcbCuenca = cbCuenca.options[selectedIndexcbCuenca].text;

    const cuencaNombre = selectedTextcbCuenca
    var valorEmbalse = ''
    const ptomedicionNombre = selectedTextcbPuntoMedicion
    const currentYear = new Date().getFullYear();

    if ($('#cbTipoPuntoMedicion').val() == 89) {
        valorEmbalse = 'Río'
    } else {
        valorEmbalse = 'Embalse'

    }

    $.ajax({
        type: 'POST',
        url: controlador + "exportarGraficoAnual",
        data: {
            tiposeriecodi: $('#cbTipoSerie').val(),
            tptomedicodi: $('#cbTipoPuntoMedicion').val(),
            ptomedicodi: $('#cbPuntoMedicion').val(),
            aniofin: currentYear,
            cuencaNombre: cuencaNombre,
            valorEmbalse: valorEmbalse,
            ptomedicionNombre: ptomedicionNombre,
        },
        dataType: 'json',
        success: function (resultado) {

            if (resultado != 1) {
                mostrarError();
            }
            else {
                window.location = controlador + "ExportarReporte";
            }

        },
        error: function () {
            mostrarError();
        }
    });
}

exportarGraficoEstadisticasAnuales = function () {

    var cbPuntoMedicion = document.getElementById("cbPuntoMedicionTab6");
    var selectedIndexcbPuntoMedicion = cbPuntoMedicion.selectedIndex;
    var selectedTextcbPuntoMedicion = cbPuntoMedicion.options[selectedIndexcbPuntoMedicion].text;

    var cbCuenca = document.getElementById("cbCuenca");
    var selectedIndexcbCuenca = cbCuenca.selectedIndex;
    var selectedTextcbCuenca = cbCuenca.options[selectedIndexcbCuenca].text;

    const cuencaNombre = selectedTextcbCuenca
    var valorEmbalse = ''
    const ptomedicionNombre = selectedTextcbPuntoMedicion
    const currentYear = new Date().getFullYear();

    if ($('#cbTipoPuntoMedicionTab6').val() == 8) {//Caudal Natural Estimado
        valorEmbalse = 'Río'
    } else {
        valorEmbalse = 'Embalse'

    }

    $.ajax({
        type: 'POST',
        url: controlador + "ExportarGraficoAnualDesvStandar",
        data: {
            tiposeriecodi: $('#cbTipoSerieTab6').val(),
            tptomedicodi: $('#cbTipoPuntoMedicionTab6').val(),
            ptomedicodi: $('#cbPuntoMedicionTab6').val(),
            aniofin: currentYear,
            cuencaNombre: cuencaNombre,
            valorEmbalse: valorEmbalse,
            ptomedicionNombre: ptomedicionNombre,
        },
        dataType: 'json',
        success: function (resultado) {

            if (resultado != 1) {
                mostrarError();
            }
            else {
                window.location = controlador + "ExportarReporte";
            }

        },
        error: function () {
            mostrarError();
        }
    });
}


exportarExcelGraficoComparativaVolumen = function () {
    var cbPuntoMedicion = document.getElementById("cbPuntoMedicionTab3");
    var selectedIndexcbPuntoMedicion = cbPuntoMedicion.selectedIndex;
    var selectedTextcbPuntoMedicion = cbPuntoMedicion.options[selectedIndexcbPuntoMedicion].text;

    var cbCuenca = document.getElementById("cbCuencaTab3");
    var selectedIndexcbCuenca = cbCuenca.selectedIndex;
    var selectedTextcbCuenca = cbCuenca.options[selectedIndexcbCuenca].text;

    const cuencaNombre = selectedTextcbCuenca
    var valorEmbalse = ''
    const ptomedicionNombre = selectedTextcbPuntoMedicion
    const currentYear = new Date().getFullYear();

    if ($('#cbTipoPuntoMedicionTab3').val() == 89) {
        valorEmbalse = 'Río'
    } else {
        valorEmbalse = 'Embalse'

    }
    $.ajax({
        type: 'POST',
        url: controlador + "exportarGraficoComparativaVolumen",
        data: {
            tiposeriecodi: $('#cbTipoSerieTab3').val(),
            tptomedicodi: $('#cbTipoPuntoMedicionTab3').val(),
            ptomedicodi: $('#cbPuntoMedicionTab3').val(),
            aniofin: currentYear,
            cuencaNombre: cuencaNombre,
            valorEmbalse: valorEmbalse,
            ptomedicionNombre: ptomedicionNombre,
        },
        dataType: 'json',
        success: function (resultado) {

            if (resultado != 1) {
                mostrarError();
            }
            else {
                window.location = controlador + "ExportarReporteComparativaVolumen";
            }

        },
        error: function () {
            mostrarError();
        }
    });

}

exportarExcelGraficoComparativaNaturalEvaporada = function () {
    var cbPuntoMedicion = document.getElementById("cbPuntoMedicionTab4");
    var selectedIndexcbPuntoMedicion = cbPuntoMedicion.selectedIndex;
    var selectedTextcbPuntoMedicion = cbPuntoMedicion.options[selectedIndexcbPuntoMedicion].text;

    var cbCuenca = document.getElementById("cbCuencaTab4");
    var selectedIndexcbCuenca = cbCuenca.selectedIndex;
    var selectedTextcbCuenca = cbCuenca.options[selectedIndexcbCuenca].text;

    const cuencaNombre = selectedTextcbCuenca
    var valorEmbalse = ''
    const ptomedicionNombre = selectedTextcbPuntoMedicion
    const currentYear = new Date().getFullYear();

    var anio = $('#cbAnioTab4').multipleSelect('getSelects');
    $('#hfAnioComparacion').val(anio);

    $.ajax({
        type: 'POST',
        url: controlador + "exportarGraficoComparativaNaturalEvaporada",
        data: {
            tiposeriecodi: $('#cbTipoSerieTab4').val(),
            ptomedicodi: $('#cbPuntoMedicionTab4').val(),
            aniofin: currentYear,
            cuencaNombre: cuencaNombre,
            valorEmbalse: valorEmbalse,
            ptomedicionNombre: ptomedicionNombre,
            anios: $('#hfAnioComparacion').val()
        },
        dataType: 'json',
        success: function (resultado) {

            if (resultado != 1) {
                mostrarError();
            }
            else {
                window.location = controlador + "ExportarReporteComparativaNaturalEvaporada";
            }

        },
        error: function () {
            mostrarError();
        }
    });

}

exportarExcelGraficoComparativaLineaTendencia = function () {
    var cbPuntoMedicionTab5 = $('#cbPuntoMedicionTab5').multipleSelect('getSelects');
    $('#hfPuntoMedicionTab5').val(cbPuntoMedicionTab5);
    

    var cbCuenca = document.getElementById("cbCuencaTab5");
    var selectedIndexcbCuenca = cbCuenca.selectedIndex;
    var selectedTextcbCuenca = cbCuenca.options[selectedIndexcbCuenca].text;

    const cuencaNombre = selectedTextcbCuenca
    var valorEmbalse = 'Rio'
    const currentYear = new Date().getFullYear();

    if ($('#cbTipoPuntoMedicionTab5').val() == 89) {
        valorEmbalse = 'Río'
    } else {
        valorEmbalse = 'Embalse'
    }

    $.ajax({
        type: 'POST',
        url: controlador + "exportarGraficoComparativaLineaTendencia",
        data: {
            tiposeriecodi: $('#cbTipoSerieTab5').val(),
            ptomedicodi: $('#hfPuntoMedicionTab5').val(),
            aniofin: currentYear,
            cuencaNombre: cuencaNombre,
            valorEmbalse: valorEmbalse,
            tptomedicodi: $('#cbTipoPuntoMedicionTab5').val()
        },
        dataType: 'json',
        success: function (resultado) {

            if (resultado != 1) {
                mostrarError();
            }
            else {
                window.location = controlador + "ExportarReporteComparativaLineaTendencia";
            }

        },
        error: function () {
            mostrarError();
        }
    });

}

creacionGraficoComparativaVolumen = function (aData, resultados, CapUtil) {
    const ValoresMaximo = []; 
    const ValoresMinimo = [];
    const ValoresPromedio = [];
    var colorsArray = ['#B5CA92', 'red', 'black', '#AF7F24', '#AA4643', '#89A54E', '#80699B', '#3D96AE', '#DB843D', '#92A8CD', '#A47D7C', '#B5CA92', '#000000', '#AF7F24', '#4572A7', '#AA4643', '#89A54E', '#80699B', '#3D96AE', '#DB843D', '#92A8CD', '#A47D7C', '#B5CA92', '#000000', '#AF7F24', '#4572A7', '#AA4643', '#89A54E', '#80699B', '#3D96AE', '#DB843D', '#92A8CD', '#A47D7C', '#B5CA92'];
    for (const mes in resultados) {
        if (resultados.hasOwnProperty(mes)) {
            ValoresMaximo.push(resultados[mes].maximo);
            ValoresMinimo.push(resultados[mes].minimo);
            ValoresPromedio.push(resultados[mes].promedio);
        }
    }
    var select = document.getElementById("cbTipoAnio3");
    var valorSeleccionado = select.options[select.selectedIndex].value;

    var selectTipoPtoMed = document.getElementById("cbTipoPuntoMedicionTab3").value;
    var hiddenTipoPtoMed = document.getElementById("hdfTipoPtoMedDescTab3").value;
    var arrayTipoPtoMed = hiddenTipoPtoMed.split("|");
    var descTipoPtoMed = "";
    for (i = 0; i <= arrayTipoPtoMed.length; i++) {
        if (arrayTipoPtoMed[i]) {
            strTipoPtoMed = arrayTipoPtoMed[i];
            var arrayDescTipoPtoMed = strTipoPtoMed.split(":");
            if (arrayDescTipoPtoMed[0] == selectTipoPtoMed) {
                descTipoPtoMed = arrayDescTipoPtoMed[1];
            }
        }
    }

    var seriesData = {};
    var categories = [];
    var contColor = 0;
    var CapacidadUtil = 0;
    if (CapUtil) {
        CapacidadUtil = parseFloat(CapUtil);
    }

    

    if (valorSeleccionado == 2) {
        seriesData = aData.ListaGraficoMensual.map(item => {
            contColor++;
            return {              
                color: colorsArray[contColor],
                name: item.Anio.toString(),
                type: 'spline',
                //yAxis: 1,
                data: [
                    item.M1, item.M2, item.M3, item.M4, item.M5, item.M6,
                    item.M7, item.M8, item.M9, item.M10, item.M11, item.M12
                ],
                tooltip: {
                    valueSuffix: ''
                },
                zIndex: 99
            };
            
        });
        categories = [
            'ENE', 'FEB', 'MAR', 'ABR', 'MAY', 'JUN',
            'JUL', 'AGO', 'SEP', 'OCT', 'NOV', 'DIC'
        ];
    } else if (valorSeleccionado == 1) {
        seriesData = aData.ListaGraficoMensual.map(item => {
            contColor++;
            return {
                color: colorsArray[contColor],
                name: item.Anio.toString(),
                type: 'spline',
               // yAxis: 1,
                data: [
                    item.M9, item.M10, item.M11, item.M12, item.M1, item.M2,
                    item.M3, item.M4, item.M5, item.M6, item.M7, item.M8
                ],
                tooltip: {
                    valueSuffix: ''
                },
                zIndex: 99
            };
            
        });
        categories = [
            'SEP', 'OCT', 'NOV', 'DIC', 'ENE', 'FEB',
            'MAR', 'ABR', 'MAY', 'JUN', 'JUL', 'AGO'
        ];
    }

    seriesData.push({
        name: 'Máximo',
        type: 'areaspline',
        data: ValoresMaximo,
        zIndex: 1,
        backgroundColor: 'red'
    });

    seriesData.push({
        name: 'Mínimo',
        type: 'areaspline',
        color: '#ddd',
        data: ValoresMinimo,
        zIndex:2
    });

    seriesData.push({
        color: 'yellow',
        name: 'Promedio',
        type: 'spline',
        data: ValoresPromedio,
        zIndex: 3,
        tooltip: {
            valueSuffix: ''
        }
    });

    var cbPuntoMedicion = document.getElementById("cbPuntoMedicionTab3");
    var selectedIndexcbPuntoMedicion = cbPuntoMedicion.selectedIndex;
    var selectedTextcbPuntoMedicion = cbPuntoMedicion.options[selectedIndexcbPuntoMedicion].text;
    const ptomedicionNombre = selectedTextcbPuntoMedicion;

    charComparativaVolumen = Highcharts.chart('graficoComparativaVolumen', {
        chart: {
            type: 'areaspline'
        },
        title: {
            text: ptomedicionNombre,
            align: 'center'
        },
        xAxis: [{
            categories: categories,
            crosshair: true
        }],
        yAxis: {
            title: {
                useHTML: true,
                text: descTipoPtoMed
            },
            plotLines: [{
                color: 'green',
                width: 2,
                value: CapacidadUtil,
                zIndex: 999,
                animation: {
                    duration: 1000,
                    defer: 4000
                },
                label: {
                    color:'green',
                    text: 'Capacidad Util :' + CapacidadUtil,
                    align: 'left',
                    x: 0
                }
            }]
        },
        tooltip: {
            shared: true,
            headerFormat: '<span style="font-size:12px"><b>{point.key}</b></span>' +
                '<br>'
        },
        plotOptions: {        
            area: {
                stacking: 'normal',
                lineColor: '#666666',
                lineWidth: 1,
                marker: {
                    lineWidth: 1,
                    lineColor: '#666666'
                },
                backgroundColor: 'red',
            }
        },
        series: seriesData
    });

}


creacionGraficoComparativaNaturalEvaporada = function (aData, resultados) {
    const ValoresMaximo = [];
    const ValoresMinimo = [];
    const ValoresPromedio = [];
    var colorsArray = ['#B5CA92', 'red', 'black', '#AF7F24', '#AA4643', '#89A54E', '#80699B', '#3D96AE', '#DB843D', '#92A8CD', '#A47D7C', '#B5CA92', '#000000', '#AF7F24', '#4572A7', '#AA4643', '#89A54E', '#80699B', '#3D96AE', '#DB843D', '#92A8CD', '#A47D7C', '#B5CA92', '#000000', '#AF7F24', '#4572A7', '#AA4643', '#89A54E', '#80699B', '#3D96AE', '#DB843D', '#92A8CD', '#A47D7C', '#B5CA92'];
    
    for (const mes in resultados) {
        if (resultados.hasOwnProperty(mes)) {
            ValoresMaximo.push(resultados[mes].maximo);
            ValoresMinimo.push(resultados[mes].minimo);
            ValoresPromedio.push(resultados[mes].promedio);
        }
    }
    var select = document.getElementById("cbTipoAnio4");
    var valorSeleccionado = select.options[select.selectedIndex].value;
    var seriesData = {};
    var categories = [];
    var contColor = 0;
    if (valorSeleccionado == 2) {
        seriesData = aData.ListaGraficoMensual.map(item => {
            contColor++;
            return {
                color: colorsArray[contColor],
                name: item.Anio.toString(),
                type: 'spline',
                //yAxis: 1,
                data: [
                    item.M1, item.M2, item.M3, item.M4, item.M5, item.M6,
                    item.M7, item.M8, item.M9, item.M10, item.M11, item.M12
                ],
                tooltip: {
                    valueSuffix: ''
                },
                zIndex: 99
            };
        });

        categories = [
            'ENE', 'FEB', 'MAR', 'ABR', 'MAY', 'JUN',
            'JUL', 'AGO', 'SEP', 'OCT', 'NOV', 'DIC'
        ];
    } else if (valorSeleccionado == 1) {
        seriesData = aData.ListaGraficoMensual.map(item => {
            contColor++;
            return {
                color: colorsArray[contColor],
                name: item.Anio.toString(),
                type: 'spline',
                // yAxis: 1,
                data: [
                    item.M9, item.M10, item.M11, item.M12, item.M1, item.M2,
                    item.M3, item.M4, item.M5, item.M6, item.M7, item.M8
                ],
                tooltip: {
                    valueSuffix: ''
                }
            };
        });
        categories = [
            'SEP', 'OCT', 'NOV', 'DIC', 'ENE', 'FEB',
            'MAR', 'ABR', 'MAY', 'JUN', 'JUL', 'AGO'
        ];
    }

    seriesData.push({
        name: 'Máximo',
        type: 'areaspline',
        data: ValoresMaximo
    });
    seriesData.push({
        name: 'Mínimo',
        color: '#ddd',
        type: 'areaspline',
        data: ValoresMinimo
    });
    seriesData.push({
        color: 'yellow',
        name: 'Promedio',
        type: 'spline',
        data: ValoresPromedio,
        tooltip: {
            valueSuffix: ''
        }
    });

    var cbPuntoMedicion = document.getElementById("cbPuntoMedicionTab4");
    var selectedIndexcbPuntoMedicion = cbPuntoMedicion.selectedIndex;
    var selectedTextcbPuntoMedicion = cbPuntoMedicion.options[selectedIndexcbPuntoMedicion].text;
    const ptomedicionNombre = selectedTextcbPuntoMedicion;

    charComparativaNaturalEvaporada = Highcharts.chart('graficoComparativaNaturalEvaporada', {
        chart: {
            type: 'area'
        },
        title: {
            text: ptomedicionNombre,
            align: 'center'
        },
        xAxis: [{
            categories: categories,
            crosshair: true
        }],
        yAxis: {
            title: {
                useHTML: true,
                text: 'm3/s'
            },
       
        },
        tooltip: {
            shared: true,
            headerFormat: '<span style="font-size:12px"><b>{point.key}</b></span>' +
                '<br>'
        },
        plotOptions: {

            area: {
                stacking: 'normal',
                lineColor: '#666666',
                lineWidth: 1,
                marker: {
                    lineWidth: 1,
                    lineColor: '#666666'
                }
            }
        },
        series: seriesData
    });

}


creacionGraficoComparativaLineaTendencia = function (aData, resultados) {

    var months = ['ENE', 'FEB', 'MAR', 'ABR', 'MAY', 'JUN', 'JUL', 'AGO', 'SEP', 'OCT', 'NOV', 'DIC'];
    var seriesData = {};
    var extendedCategories = [];
    var yearLabels = [];
    const dataset = [];
    const datasetSeriesTrend = {};
    var intInicio = 0;
    const datasetPtoMed = [];
    var colorSeries = [];
    var colorsArray = ['#000000', '#AF7F24', '#4572A7', '#AA4643', '#89A54E', '#80699B', '#3D96AE', '#DB843D', '#92A8CD', '#A47D7C', '#B5CA92', '#000000', '#AF7F24', '#4572A7', '#AA4643', '#89A54E', '#80699B', '#3D96AE', '#DB843D', '#92A8CD', '#A47D7C', '#B5CA92', '#000000', '#AF7F24', '#4572A7', '#AA4643', '#89A54E', '#80699B', '#3D96AE', '#DB843D', '#92A8CD', '#A47D7C', '#B5CA92'];
    var contColor = 0;

    var selectTipoPtoMed = document.getElementById("cbTipoPuntoMedicionTab5").value;
    var hiddenTipoPtoMed = document.getElementById("hdfTipoPtoMedDescTab5").value;
    var arrayTipoPtoMed = hiddenTipoPtoMed.split("|");
    var descTipoPtoMed = "";
    for (i = 0; i <= arrayTipoPtoMed.length; i++) {
        if (arrayTipoPtoMed[i]) {
            strTipoPtoMed = arrayTipoPtoMed[i];
            var arrayDescTipoPtoMed = strTipoPtoMed.split(":");
            if (arrayDescTipoPtoMed[0] == selectTipoPtoMed) {
                descTipoPtoMed = arrayDescTipoPtoMed[1];
            }
        }
    }

    // Recolectar datos de los meses y años
    aData.ListaGraficoMensual.forEach(function (record) {
        var key = record.Ptomedielenomb;
        if (!seriesData[key]) {
            seriesData[key] = {
                name: '<font color="' + colorsArray[contColor] + '">______</font> ' + key,
                data: [],
                legend: {
                    enabled: true
                },
            };
            intInicio = 0;
            datasetSeriesTrend['trendline' + key] = [];
            datasetPtoMed.push(key);
            colorSeries.push(colorsArray[contColor]);
            contColor++;
        }

        for (var i = 1; i <= 12; i++) {
            seriesData[key].data.push(record['M' + i]);
            datasetSeriesTrend['trendline' + key].push([intInicio, record['M' + i]]);
            intInicio++;
            if (!extendedCategories.includes(months[i - 1])) {
                extendedCategories.push(months[i - 1]);
            }
        }

        if (!yearLabels.includes(record.Anio)) {
            yearLabels.push(record.Anio);
        }
    });

    for (var i = 0; i < datasetPtoMed.length; i++) {
        colorSeries.push(colorsArray[i]);
        seriesData['trendline' + datasetPtoMed[i]] = {
            type: 'line',
            name: '<font color="' + colorsArray[i] +'">.......</font>Linea de Tendencia ' + datasetPtoMed[i],
            data: getTrendLine(datasetSeriesTrend['trendline' + datasetPtoMed[i]]),
            marker: {
                enabled: false
            },
            legend: {
                enabled: false
            },
            states: {
                hover: {
                    lineWidth: 0
                }
            },
            enableMouseTracking: false,
            zones: [{
                dashStyle: 'dot'
            }],
        };

    }


    // Asegúrate de que extendedCategories tenga los meses repetidos correctamente
    var totalYears = yearLabels.length;
    extendedCategories = [];
    for (var j = 0; j < totalYears; j++) {
        extendedCategories = extendedCategories.concat(months);
    }



    // Convertir objeto seriesData a un array de series para Highcharts
    var seriesArray = Object.values(seriesData);

    var cbPuntoMedicion = document.getElementById("cbPuntoMedicionTab5");
    var selectedIndexcbPuntoMedicion = cbPuntoMedicion.selectedIndex;
    var selectedTextcbPuntoMedicion = cbPuntoMedicion.options[selectedIndexcbPuntoMedicion].text;
    const ptomedicionNombre = selectedTextcbPuntoMedicion;

   charComparativaLineaTendencia = Highcharts.chart('graficoComparativaLineaTendencia', {
        chart: {
            type: 'spline',
            scrollablePlotArea: {
                minWidth: 600,
                scrollPositionX: 1
            },
            events: {
                load: function () {
                    var chart = this,
                        xAxis = chart.xAxis[0];
                    yearLabels.forEach(function (year, i) {
                        var start = xAxis.toPixels(i * 12, true);
                        var end = xAxis.toPixels((i + 1) * 12, true);
                        var xPos = (start + end) / 2; // Calcula la posición central del rango de 12 meses
                        var xNumAnio = yearLabels.length / 2;
                        chart.renderer.text(
                            year,
                            xPos + 20 + xNumAnio,
                            chart.chartHeight - 45 // Ajusta la posición vertical si es necesario
                        )
                            .attr({
                                align: 'center',
                                rotation: -45 // Rotación de las etiquetas de los años
                            })
                            .css({
                                fontSize: '8px'
                            })
                            .add();
                    });
                }
            }
        },
        title: {
            text: ptomedicionNombre,
            align: 'center'
        },
        xAxis: {
            categories: extendedCategories,
            labels: {
                useHTML: true,
                rotation: -45, // Rotación de etiquetas de meses en diagonal
                style: {
                    fontSize: '10px',
                    fontFamily: 'Verdana, sans-serif'
                }
            }
        },
        yAxis: {
            title: {
                text: descTipoPtoMed
            },
            minorGridLineWidth: 0.5,
            gridLineWidth: 1,
            alternateGridColor: null,
            minorTickInterval: 'auto'
        },
        tooltip: {
            valueSuffix: ' ' + descTipoPtoMed
        },
        plotOptions: {
            spline: {
                lineWidth: 2,
                states: {
                    hover: {
                        lineWidth: 3
                    }
                },
                marker: {
                    enabled: false
                }
            }
       },
       legend: {
           enabled: true,
           symbolHeight: 0,
           symbolWidth: 0,
           symbolRadius: 0,
           squareSymbol: false,
           useHTML: true,
       },
        colors: colorSeries,
        series: seriesArray,
        navigation: {
            menuItemStyle: {
                fontSize: '10px'
            }
        }
    });



}


function getTrendLine(data) {
    const n = data.length;

    let sumX = 0,
        sumY = 0,
        sumXY = 0,
        sumX2 = 0;

    // Calculate the sums needed for linear regression
    for (let i = 0; i < n; i++) {
        const [x, y] = data[i];
        sumX += x;
        sumY += y;
        sumXY += x * y;
        sumX2 += x ** 2;
    }

    // Calculate the slope of the trend line
    const slope = (n * sumXY - sumX * sumY) / (n * sumX2 - sumX ** 2);

    // Calculate the intercept of the trend line
    const intercept = (sumY - slope * sumX) / n;

    const trendline = []; // Array to store the trend line data points

    // Find the minimum and maximum x-values from the scatter plot data
    const minX = Math.min(...data.map(([x]) => x));
    const maxX = Math.max(...data.map(([x]) => x));

    // Calculate the corresponding y-values for the trend line using the slope
    // and intercept
    trendline.push([minX, minX * slope + intercept]);
    trendline.push([maxX, maxX * slope + intercept]);

    return trendline;
}
creacionGraficoEstadisticasAnuales = function (aData, resultados) {
    const catAnios = [];
    const catValores = [];
    const dataset = [];
    var intInicio = 0;
    var valorEstadistico = "";
    aData.ListaGraficoMensual.map(item => {
        catAnios.push(item.Anio.toString());
        if ($('#cbEstadistico').val() == "1") {
            valorEstadistico = item.maximo;
        } else if ($('#cbEstadistico').val() == "2") {
            valorEstadistico = item.minimo;
        } else if ($('#cbEstadistico').val() == "3") {
            valorEstadistico = item.promedio;
        } else if ($('#cbEstadistico').val() == "4") {
            valorEstadistico = item.desv;
        }
        catValores.push(valorEstadistico);
        dataset.push([intInicio, valorEstadistico]);
        intInicio++;
    });
    
    var select = document.getElementById("cbEstadistico");
    var valorSeleccionado = select.options[select.selectedIndex].value;
    var titulo = "";
    if (valorSeleccionado == "1") {
        titulo = "Máximo";
    } else if (valorSeleccionado == "2") {
        titulo = "Mínimo";
    } else if (valorSeleccionado == "3") {
        titulo = "Promedio";
    } else if (valorSeleccionado == "4") {
        titulo = "Desviación Estándar";
    }
    var cbTipoPuntoMedicionTab6 = document.getElementById('cbTipoPuntoMedicionTab6').value;

    var cbPuntoMedicion = document.getElementById("cbPuntoMedicionTab6");
    var selectedIndexcbPuntoMedicion = cbPuntoMedicion.selectedIndex;
    var selectedTextcbPuntoMedicion = cbPuntoMedicion.options[selectedIndexcbPuntoMedicion].text;
    const ptomedicionNombre = selectedTextcbPuntoMedicion;

    var selectTipoPtoMed = document.getElementById("cbTipoPuntoMedicionTab6").value;
    var hiddenTipoPtoMed = document.getElementById("hdfTipoPtoMedDescTab6").value;
    var arrayTipoPtoMed = hiddenTipoPtoMed.split("|");
    var descTipoPtoMed = "";
    for (i = 0; i <= arrayTipoPtoMed.length; i++) {
        if (arrayTipoPtoMed[i]) {
            strTipoPtoMed = arrayTipoPtoMed[i];
            var arrayDescTipoPtoMed = strTipoPtoMed.split(":");
            if (arrayDescTipoPtoMed[0] == selectTipoPtoMed) {
                descTipoPtoMed = arrayDescTipoPtoMed[1];
            }
        }
    }

    chartEstadisticasAnuales = Highcharts.chart('graficoEstadisticasAnuales', {
        chart: {
            type: 'column'
        },
        title: {
            text: titulo + ' Anual ' + ptomedicionNombre,
            align: 'center'
        },
        subtitle: {
            text:
                '',
            align: 'left'
        },
        xAxis: {
            categories: catAnios,
            crosshair: true,
            accessibility: {
                description: 'Años'
            }
        },
        yAxis: {
            min: 0,
            title: {
                text: descTipoPtoMed
            }
        },
        tooltip: {
            valueSuffix: descTipoPtoMed
        },
        plotOptions: {
            column: {
                pointPadding: 0.2,
                borderWidth: 0
            }
        },
        series: [
            {
                name: titulo,
                data: catValores
            },
            {
                type: 'line',
                name: 'Linea de Tendencia',
                data: getTrendLine(dataset),
                marker: {
                    enabled: false
                },
                states: {
                    hover: {
                        lineWidth: 0
                    }
                },
                enableMouseTracking: false
            }
        ]
    });


}



mostrarError = function () {

    alert('Ha ocurrido un error.');
}

