var controlador = siteRoot + 'hidrologia/';

$(function () {
    $("#btnBuscar").click(function () {
        if (validarFiltroSeleccionado()) {           
            buscarDatos();
        }
    });

    $("#btnExpotar").click(function () {
        if (validarFiltroSeleccionado()) {
            exportarReporteAppPowel();
        }
    });

    $("#cbTipoReporte").change(function () {
        $("#listado").html("");
        $("#paginado").html("");
        mostrarFiltro();
    });

    $('#fechaInicio').Zebra_DatePicker({        
        pair: $('#fechaFin'),
        onSelect: function (date) {           
            var date1 = getFecha(date);
            var date2 = getFecha($('#fechaFin').val());            
            if (date1 > date2) {
                $('#fechaFin').val(date);
            }
        }
    });
    $('#fechaFin').Zebra_DatePicker({
        direction: true,
        onSelect: function () {
            if (validarFiltroSeleccionado()) {
                buscarDatos();
            }
        }
    });
    $('#anhoIni').Zebra_DatePicker({
        format: 'Y',
        pair: $('#anhoFin'),
        onSelect: function (date) {           
            //var date1 = getFecha(date);
            //var date2 = getFecha($('#anhoFin').val());
            var date1 = parseInt(date);
            var date2 = parseInt($('#anhoFin').val());
            if (date1 > date2) {
                $('#anhoFin').val(date);
                cargarSemanaAnho2();
            }            
            cargarSemanaAnho()           
        }
    });

    $('#anhoFin').Zebra_DatePicker({
        format: 'Y',
        direction: true,
        onSelect: function () {
            cargarSemanaAnho2()
        }       
    });

    mostrarFiltro();
});

function buscarDatos() {
    var tipoReporte = $("#cbTipoReporte").val();
    if (tipoReporte == 1) { //Diario
        pintarPaginado(1)
    }    
    mostrarListado(1);
}

function pintarPaginado(id) {   
    var fechaIni = getFechaIni();
    var fechaFin = getFechaFin();
    $.ajax({
        type: 'POST',
        url: controlador + "Reporte/PaginadoAppPowel",
        data: {
            
            fechaInicial: fechaIni, 
            fechaFinal: fechaFin, 
        },
        success: function (evt) {
            $('#paginado').html(evt);
            mostrarPaginado();
        },
        error: function () {
            alert("Ha ocurrido un error paginado");
        }
    });
}

function pintarBusqueda(nPagina) {
    mostrarListado(nPagina);
}


function mostrarListado(nroPagina) {   
    mostrarReporteAppPowel(nroPagina);   
}


function validarFiltroSeleccionado() {

    var tipoReporte = $("#cbTipoReporte").val();
    if (tipoReporte == "-1") {
        alert("Debe seleccionar un tipo de reporte");
        return false;
    }

    if (tipoReporte == 1) {// diario
        var fechaIni = $("#fechaInicio").val();
        if (fechaIni == "") {
            alert("Debe seleccionar una fecha de Inicio");
            return false;
        }
        var fechaFin = $('#fechaFin').val();
        if (fechaFin == "") {
            alert("Debe seleccionar una fecha Fin");
            return false;
        }       
        fIni = convertStringToDate(fechaIni, "00:00:00");
        fFin = convertStringToDate(fechaFin, "00:00:00");
        if (moment(fFin).isBefore(fIni)) {
            alert("La fecha inicio no puede ser mayor  a la fecha fin");
            return false;
        }

    }
    if (tipoReporte == 2) {//semanal
        var numsemana = $("#cbSemanas").val();
        if (numsemana == "0") {
            alert("Debe seleccionar una semana Inicio");
            return false;
        }
        numsemana = $("#cbSemanas2").val();
        if (numsemana == "0") {
            alert("Debe seleccionar una semana fin");
            return false;
        }
    }

    return true;
}

function mostrarReporteAppPowel(nroPagina) {
    var tipoReporte = $("#cbTipoReporte").val();
    var fechaIni = getFechaIni();
    var fechaFin = getFechaFin();
    var semanaIni = getSemanaIni();
    var semanaFin = getSemanaFin();
  
    


    $.ajax({
        type: 'POST',
        url: controlador + 'reporte/CargarListaAppPowel',
        data: {
            tipoReporte: tipoReporte,
            fechaInicial: fechaIni,
            fechaFinal: fechaFin,
            semanaIni: semanaIni,
            semanaFin: semanaFin,
            nroPagina: nroPagina
        },
        success: function (aData) {
            $('#listado').css("width", $('#mainLayout').width() - 10 + "px");
            $('#listado').html(aData);           
            $('#tabla').dataTable({
                // "aoColumns": aoColumns(),
                "bAutoWidth": false,
                "bSort": false,
                "scrollY": 500,
                "scrollX": true,
                "sDom": 't',
                "iDisplayLength": -1,
                fixedColumns: { leftColumns: 1 },
                "language": {
                    "emptyTable": "No Existen registros..!"
                }
            });
        },
        error: function () {
            alert("Ha ocurrido un error");
        }
    });
}

function exportarReporteAppPowel() {
    var tipoReporte = $("#cbTipoReporte").val();
    var fechaIni = getFechaIni();
    var fechaFin = getFechaFin();
    var semanaIni = getSemanaIni();
    var semanaFin = getSemanaFin();


    $.ajax({
        type: 'POST',
        url: controlador + 'reporte/ExportarExcelListaAppPowel',
        data: {
            tipoReporte: tipoReporte,
            fechaInicial: fechaIni,
            fechaFinal: fechaFin,
            semanaIni: semanaIni,
            semanaFin: semanaFin,
        },
        success: function (result) {
            if (result.length > 0) {
                archivo = result[0];
                nombre = result[1];
                window.location.href = controlador + 'reporte/DescargarExcelPronostico?archivo=' + archivo + "&nombre=" + nombre;
            }
            else {
                alert("Error en descargar el archivo");
            }
        },
        error: function () {
            alert('ha ocurrido un error al descargar el archivo excel.');
        }
    });
}


function mostrarFiltro() {
    $("#reporte").css("display", "none");
    tipoRep = $("#cbTipoReporte").val();

    switch (tipoRep) {
        case '1': //diario
            buscarDatos();
            $(".filaDia").show();
            $(".filaSemana").hide();
            break;
        case '2': //semanal
            cargarSemanaAnho();
            cargarSemanaAnho2();
            $(".filaDia").hide();
            $(".filaSemana").show();
            $('#paginado').html("");
            break;
        case '-1':
        default:
            $(".filaDia").hide();
            $(".filaSemana").hide();
            break;
    }
}

function cargarSemanaAnho() {
    var anho = $('#anhoIni').val();
    $('#hfAnhoIni').val(anho);

    $.ajax({
        type: 'POST',
        url: controlador + 'reporte/CargarSemanas',

        data: { idAnho: $('#hfAnhoIni').val() },

        success: function (aData) {
            $('#divSemana').html(aData);

           // mostrarReporteAppPowel();

            $("#cbSemanas").change(function () {
               // mostrarReporteAppPowel();
            });
        },
        error: function () {
            alert("Ha ocurrido un error");
        }
    });
}

function cargarSemanaAnho2() {
    var anho2 = $('#anhoFin').val();
    $('#hfAnhoFin').val(anho2);

    $.ajax({
        type: 'POST',
        url: controlador + 'reporte/CargarSemanas2',

        data: { idAnho: $('#hfAnhoFin').val() },

        success: function (aData) {
            $('#divSemana2').html(aData);

           // mostrarReporteAppPowel(1);

            $("#cbSemanas2").change(function () {
                //mostrarReporteAppPowel(1);
            });
        },
        error: function () {
            alert("Ha ocurrido un error");
        }
    });
}

function getSemanaIni() {
    var semana = "";
    var cbsemana = $("#cbSemanas").val();
    if (cbsemana == "0" || cbsemana == "" || cbsemana == undefined) {
        semana = $("#hfSemanaIni").val();
    } else {
        semana = cbsemana;
    }

    if (semana == "0" || semana == "") {
        semana = "1";
    }

    $("#cbSemanas").val(semana);
    $('#hfSemanaIni').val(semana);
    $('#hfAnhoIni').val($('#anhoIni').val());
    semana = $("#hfSemanaIni").val();
    anho = $('#hfAnhoIni').val();

    if (semana !== undefined && anho !== undefined) {
        semana = anho.toString() + semana;
    } else {
        semana = '';
    }

    return semana;
}

function getSemanaFin() {
    var semana = "";
    var cbsemana = $("#cbSemanas2").val();
    if (cbsemana == "0" || cbsemana == "" || cbsemana == undefined) {
        semana = $("#hfSemanaFin").val();
    } else {
        semana = cbsemana;
    }

    if (semana == "0" || semana == "") {
        semana = "1";
    }

    $("#cbSemanas2").val(semana);
    $('#hfSemanaFin').val(semana);
    $('#hfAnhoFin').val($('#anhoFin').val());
    semana = $("#hfSemanaFin").val();
    anho = $('#hfAnhoFin').val();

    if (semana !== undefined && anho !== undefined) {
        semana = anho.toString() + semana;
    } else {
        semana = '';
    }

    return semana;
}

function getFechaIni() {
    $('#hfFechaInicio').val($('#fechaInicio').val());
    fecha = $("#hfFechaInicio").val();
    fecha = fecha !== undefined ? fecha : '';
    return fecha;
}

function getFechaFin() {
    $('#hfFechaFin').val($('#fechaFin').val());
    fecha = $("#hfFechaFin").val();
    fecha = fecha !== undefined ? fecha : '';
    return fecha;
}

//convierte 2 cadenas de texto fecha(dd/mm/yyyy) y horas(hh:mm:ss) a tipo Date
function convertStringToDate(fecha, horas) {
    var partsFecha = fecha.split('/');
    if (horas == "") {
        return "";
    }
    var partsHoras = horas.split(':');
    //new Date(yyyy, mm-1, dd, hh, mm, ss);
    return new Date(partsFecha[2], partsFecha[1] - 1, partsFecha[0], partsHoras[0], partsHoras[1], partsHoras[2]);
}
