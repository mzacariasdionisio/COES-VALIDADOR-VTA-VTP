var controlador = siteRoot + 'hidrologia/descargarseries/';

var plot = null;
var charAnual;
var charMensual;
var charComparativaVolumen;
var charComparativaNaturalEvaporada;
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
    

    $('#txtFechaDesdeTab7').blur(function () {
        if ($(this).val().trim() === '') {
            $(this).val(formattedDate);
        }
    });

    $('#txtFechaHastaTab7').blur(function () {
        if ($(this).val().trim() === '') {
            $(this).val(endDate);
        }
    });

    $('#txtFechaDesdeTab8').blur(function () {
        if ($(this).val().trim() === '') {
            $(this).val(formattedDate);
        }
    });

    $('#txtFechaHastaTab8').blur(function () {
        if ($(this).val().trim() === '') {
            $(this).val(endDate);
        }
    });
    $('#txtFechaDesdeTab9').blur(function () {
        if ($(this).val().trim() === '') {
            $(this).val(formattedDate);
        }
    });

    $('#txtFechaHastaTab9').blur(function () {
        if ($(this).val().trim() === '') {
            $(this).val(endDate);
        }
    });
    
    $('#tab-container').easytabs({
        animate: false
    });

    $('#tab-container-principal').easytabs({
        animate: false
    });



    $('#txtFechaDesdeTab7').Zebra_DatePicker({
        readonly_element: false,
        format: 'm/Y',  // Formato de mes y año
        view: 'years',  // Vista inicial
        start_date: '01/1965', // Fecha de inicio
        direction: ['01/1965', endDate], // Rango de fechas permitido
        onSelect: function (date) {
            $('#txtFechaDesdeTab7').val(date);
        }
    });


    $('#txtFechaDesdeTab7').inputmask({
        mask: "1/y",
        placeholder: "mm/yyyy",
        alias: "date",
        hourFormat: "24"
    });



    $('#txtFechaHastaTab7').Zebra_DatePicker({
        readonly_element: false,
        format: 'm/Y',  // Formato de mes y año
        view: 'years',  // Vista inicial
        start_date: '02/1965', // Fecha de inicio
        direction: ['02/1965', endDate], // Rango de fechas permitido
        onSelect: function (date) {
            $('#txtFechaHastaTab7').val(date);
        }
    });


    $('#txtFechaHastaTab7').inputmask({
        mask: "1/y",
        placeholder: "mm/yyyy",
        alias: "date",
        hourFormat: "24"
    });
    $('#txtFechaDesdeTab8').Zebra_DatePicker({
        readonly_element: false,
        format: 'm/Y',  // Formato de mes y año
        view: 'years',  // Vista inicial
        start_date: '01/1965', // Fecha de inicio
        direction: ['01/1965', endDate], // Rango de fechas permitido
        onSelect: function (date) {
            $('#txtFechaDesdeTab7').val(date);
        }
    });


    $('#txtFechaDesdeTab8').inputmask({
        mask: "1/y",
        placeholder: "mm/yyyy",
        alias: "date",
        hourFormat: "24"
    });



    $('#txtFechaHastaTab8').Zebra_DatePicker({
        readonly_element: false,
        format: 'm/Y',  // Formato de mes y año
        view: 'years',  // Vista inicial
        start_date: '02/1965', // Fecha de inicio
        direction: ['02/1965', endDate], // Rango de fechas permitido
        onSelect: function (date) {
            $('#txtFechaHastaTab8').val(date);
        }
    });


    $('#txtFechaHastaTab8').inputmask({
        mask: "1/y",
        placeholder: "mm/yyyy",
        alias: "date",
        hourFormat: "24"
    });

    $('#txtFechaDesdeTab9').Zebra_DatePicker({
        readonly_element: false,
        format: 'm/Y',  // Formato de mes y año
        view: 'years',  // Vista inicial
        start_date: '01/1965', // Fecha de inicio
        direction: ['01/1965', endDate], // Rango de fechas permitido
        onSelect: function (date) {
            $('#txtFechaDesdeTab9').val(date);
        }
    });


    $('#txtFechaDesdeTab9').inputmask({
        mask: "1/y",
        placeholder: "mm/yyyy",
        alias: "date",
        hourFormat: "24"
    });



    $('#txtFechaHastaTab9').Zebra_DatePicker({
        readonly_element: false,
        format: 'm/Y',  // Formato de mes y año
        view: 'years',  // Vista inicial
        start_date: '02/1965', // Fecha de inicio
        direction: ['02/1965', endDate], // Rango de fechas permitido
        onSelect: function (date) {
            $('#txtFechaHastaTab9').val(date);
        }
    });


    $('#txtFechaHastaTab9').inputmask({
        mask: "1/y",
        placeholder: "mm/yyyy",
        alias: "date",
        hourFormat: "24"
    });
    $('#tab7Paso7').click(function () {
        var self = this;
        ; $.ajax({
            type: 'POST',
            url: controlador + "Index",
            data: {

            },
            success: function (evt) {

                $('#listadotab7').css("width", $('#mainLayout').width() + "px");
                $('#listadotab7').html(evt);
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

    $('#tab8Paso8').click(function () {
        var self = this;
        $.ajax({
            type: 'POST',
            url: controlador + "graficoMatricesMensuales",
            data: {

            },
            success: function (evt) {

                $('#listadotab8').css("width", $('#mainLayout').width() + "px");
                $('#listadotab8').html(evt);
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

    $('#tab9Paso9').click(function () {
        var self = this;
        $.ajax({
            type: 'POST',
            url: controlador + "graficoModeloPerseo",
            data: {

            },
            success: function (evt) {

                $('#listadotab9').css("width", $('#mainLayout').width() + "px");
                $('#listadotab9').html(evt);
                $('#tabla').dataTable({
                    "scrollY": 430,
                    "scrollX": false,
                    "sDom": 't',
                    "ordering": false,
                    "iDisplayLength": -1
                });
                document.getElementById('txtExcel').checked = true;

            },
            error: function () {
                mostrarError();
            }
        });
    });

   

    obtenerCuenca();
        

});

function limpiarTab1() {

}
function formatDateToMMYYYY(date) {
    const month = ("0" + (date.getMonth() + 1)).slice(-2); // Obtener el mes y agregar un 0 inicial si es necesario
    const year = date.getFullYear(); // Obtener el año
    return `${month}/${year}`;
}
function habilitarRangeFecha(elementTipoaño) {
    if (elementTipoaño.value) {
       

        // controles tab 7
        $('#txtFechaDesdeTab7').prop('disabled', false);
        $('#txtFechaHastaTab7').prop('disabled', false);
        $('#cbCuencaTab7').prop('disabled', false);


        // controles tab 8
        $('#txtFechaDesdeTab8').prop('disabled', false);
        $('#txtFechaHastaTab8').prop('disabled', false);
        $('#btnExportarTab8').prop('disabled', false);


        // controles tab 9
        $('#txtFechaDesdeTab9').prop('disabled', false);
        $('#txtFechaHastaTab9').prop('disabled', false);
        $('#cbCuencaTab9').prop('disabled', false);


        obtenerCuenca();

    } else {
       
        // controles tab 7
        $('#txtFechaDesdeTab7').prop('disabled', true);
        $('#txtFechaHastaTab7').prop('disabled', true);
        $('#cbCuencaTab7').prop('disabled', true);


        // controles tab 8
        $('#txtFechaDesdeTab8').prop('disabled', true);
        $('#txtFechaHastaTab8').prop('disabled', true);
        $('#btnExportarTab8').prop('disabled', true);

        // controles tab 9
        $('#txtFechaDesdeTab9').prop('disabled', true);
        $('#txtFechaHastaTab9').prop('disabled', true);
        $('#btnExportarTab9').prop('disabled', true);
    }
}

function habilitarRangeFechaMatricesMensuales(elementTipoPtoMed) {
    if (elementTipoPtoMed.value) {
        // controles tab 8
        $('#txtFechaDesdeTab8').prop('disabled', false);
        $('#txtFechaHastaTab8').prop('disabled', false);
        $('#btnExportarTab8').prop('disabled', false);
    } else {
        // controles tab 8
        $('#txtFechaDesdeTab8').prop('disabled', true);
        $('#txtFechaHastaTab8').prop('disabled', true);
        $('#btnExportarTab8').prop('disabled', true);
    }
}

function habilitarTipoPuntoMedicion(tipoPtoMedicion) {
    if (tipoPtoMedicion.value) {
       
        //controles tab 8
        $('#cbTipoPuntoMedicionTab8').prop('disabled', false);
    } else {
        // controles tab 8

        $('#cbTipoPuntoMedicionTab8').prop('disabled', true);
        $('#cbTipoPuntoMedicionTab8').val('');
    }
}

function habilitarBtnDescargar(cuenca) {
    if (cuenca.value) {


        //controles tab 7
        $('#btnExportarTab7').prop('disabled', false);
        //controles tab 9
        $('#btnExportarTab9').prop('disabled', false);
        $('#txtExcel').prop('disabled', false);
        $('#txtCsv').prop('disabled', false);
        $('#txtTxt').prop('disabled', false);


        
    } else {
        //controles tab 7
        $('#btnExportarTab7').prop('disabled', true);
        //controles tab 9
        $('#btnExportarTab9').prop('disabled', true);
        $('#txtExcel').prop('disabled', true);
        $('#txtCsv').prop('disabled', true);
        $('#txtTxt').prop('disabled', true);
    }

}

function exportarExcelGraficoModeloPerseo() {
    const fechaInicioStr = $('#txtFechaDesdeTab9').val();
    const fechaFinStr = $('#txtFechaHastaTab9').val();
    const [inicioMes, inicioAnio] = fechaInicioStr.split('/').map(Number);
    const [finMes, finAnio] = fechaFinStr.split('/').map(Number);
    var strTipoArchivo = "";
    var strTipoArchivoExcel = 0;
    var strTipoArchivoCsv = 0;
    var strTipoArchivoTxt = 0;
    $('input[name="export"]:checked').each(function () {
        strTipoArchivo = strTipoArchivo + this.value + ",";
        if (this.value == "excel") {
            strTipoArchivoExcel = 1;
        } else if (this.value == "csv") {
            strTipoArchivoCsv = 1;
        } else if (this.value == "txt") {
            strTipoArchivoTxt = 1;
        }
    });

    $.ajax({
        type: 'POST',
        url: controlador + "exportarModeloPerseo",
        data: {
            cuenca: $('#cbCuencaTab9').val(),
            tptomedicodi: $('#cbTipoPuntoMedicionTab9').val(),
            tiposeriecodi: $('#cbTipoSerieTab9').val(),
            anioinicio: inicioAnio,
            aniofin: finAnio,
            rangofechainicial: fechaInicioStr,
            rangofechafinal: fechaFinStr,
            tipoArchivo: strTipoArchivo            
        },
        dataType: 'json',
        success: function (resultado) {

            if (resultado == -1) {
                mostrarError();
            } else if (resultado == 0) {
                alert('No existe información en base de datos para el filtro seleccionado.');
            } else {
                if (strTipoArchivoExcel) {
                    setTimeout(() => {
                        window.location = controlador + "ExportarReporteExcelModeloPerseo";
                    }, 100);
                }
                
                if (strTipoArchivoTxt) {
                    setTimeout(() => {
                        window.location = controlador + "ExportarReporteTxtModeloPerseo?cuenca=" + $('#cbCuencaTab9').val() + "&tptomedicodi=" + $('#cbTipoPuntoMedicionTab9').val() + "&tiposeriecodi=" + $('#cbTipoSerieTab9').val() + "&anioinicio=" + inicioAnio + "&aniofin=" + finAnio + "&rangofechainicial=" + fechaInicioStr + "&rangofechafinal=" + fechaFinStr + "&tipoArchivo=TXT";
                    }, 30000);
                }

                if (strTipoArchivoCsv) {
                    setTimeout(() => {
                        window.location = controlador + "ExportarReporteTxtModeloPerseo?cuenca=" + $('#cbCuencaTab9').val() + "&tptomedicodi=" + $('#cbTipoPuntoMedicionTab9').val() + "&tiposeriecodi=" + $('#cbTipoSerieTab9').val() + "&anioinicio=" + inicioAnio + "&aniofin=" + finAnio + "&rangofechainicial=" + fechaInicioStr + "&rangofechafinal=" + fechaFinStr + "&tipoArchivo=CSV";
                    }, 4000);
                }
                
            }

        },
        error: function () {
            mostrarError();
        }
    });
}
function exportarExcelGraficoMatricesMensuales() {
    const fechaInicioStr = $('#txtFechaDesdeTab8').val();
    const fechaFinStr = $('#txtFechaHastaTab8').val();
    const [inicioMes, inicioAnio] = fechaInicioStr.split('/').map(Number);
    const [finMes, finAnio] = fechaFinStr.split('/').map(Number);

    $.ajax({
        type: 'POST',
        url: controlador + "exportarMatricesMensuales",
        data: {
            cuenca: $('#cbCuencaTab8').val(),
            tptomedicodi: $('#cbTipoPuntoMedicionTab8').val(),
            tiposeriecodi: $('#cbTipoSerieTab8').val(),
            anioinicio: inicioAnio,
            aniofin: finAnio,
            rangofechainicial: fechaInicioStr,
            rangofechafinal: fechaFinStr
        },
        dataType: 'json',
        success: function (resultado) {

            if (resultado == -1) {
                mostrarError();
            } else if (resultado == 0) {
                alert("No existe información en base de datos para el filtro seleccionado.");
            } else {
                window.location = controlador + "ExportarReporteMatricesMensuales";
            }
        },
        error: function () {
            mostrarError();
        }
    });
}
function exportarExcelGraficoTablaVertical() {

 
    const fechaInicioStr = $('#txtFechaDesdeTab7').val();
    const fechaFinStr = $('#txtFechaHastaTab7').val();

    // Convertir las variables de entrada a objetos Date
    const [inicioMes, inicioAnio] = fechaInicioStr.split('/').map(Number);
    const [finMes, finAnio] = fechaFinStr.split('/').map(Number);

    $.ajax({
        type: 'POST',
        url: controlador + "exportarTablaVertical",
        data: {
            cuenca: $('#cbCuencaTab7').val(),
            tptomedicodi: $('#cbTipoPuntoMedicionTab7').val(), 
            tiposeriecodi: $('#cbTipoSerieTab7').val(), 
            anioinicio: inicioAnio,
            aniofin: finAnio,
            rangofechainicial: fechaInicioStr,
            rangofechafinal: fechaFinStr
        },
        dataType: 'json',
        success: function (resultado) {

            if (resultado == -1) {
                mostrarError();
            } else if (resultado == 0) {
                alert('No existe información en base de datos para el filtro seleccionado.');
            } else {
                window.location = controlador + "ExportarReporteTablaVertical";
            }

        },
        error: function () {
            mostrarError();
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
           
            var cbCuencaTab7 = document.getElementById('cbCuencaTab7');
            var cbCuencaTab8 = document.getElementById('cbCuencaTab8');
            var cbCuencaTab9 = document.getElementById('cbCuencaTab9');

            if (cbCuencaTab7) {
                $("#cbCuencaTab7").empty();
                $('#cbCuencaTab7').get(0).options.length = 0;
                $('#cbCuencaTab7').get(0).options[0] = new Option("--SELECCIONE--", "");
            }
            if (cbCuencaTab8) {
                $("#cbCuencaTab8").empty();
                $('#cbCuencaTab8').get(0).options.length = 0;
                $('#cbCuencaTab8').get(0).options[0] = new Option("--SELECCIONE--", "");
            }
            if (cbCuencaTab9) {
                $("#cbCuencaTab9").empty();
                $('#cbCuencaTab9').get(0).options.length = 0;
                $('#cbCuencaTab9').get(0).options[0] = new Option("--SELECCIONE--", "");
            }
           
            $.each(aData, function (i, item) {
               
                if (cbCuencaTab7) {
                    $('#cbCuencaTab7').get(0).options[$('#cbCuencaTab7').get(0).options.length] = new Option(item.Text, item.Value);
                }
                if (cbCuencaTab8) {
                    $('#cbCuencaTab8').get(0).options[$('#cbCuencaTab8').get(0).options.length] = new Option(item.Text, item.Value);
                }
                if (cbCuencaTab9) {
                    $('#cbCuencaTab9').get(0).options[$('#cbCuencaTab9').get(0).options.length] = new Option(item.Text, item.Value);
                }

            });
        },
        error: function () {
            alert("Ha ocurrido un error");
        }
    });
}




mostrarError = function () {

    alert('Ha ocurrido un error.');
}

