var controlador = siteRoot + 'hidrologia/';

$(function () {
    $("#btnBuscar").click(function () {
        if (validarFiltroSeleccionado()) {
            mostrarReportePronostico();
        }
    });

    $("#btnExpotar").click(function () {
        if (validarFiltroSeleccionado()) {
            exportarReportePronostico();
        }
    });

    $("#cbTipoReporte").change(function () {
        mostrarFiltro();
    });

    $('#fecha').Zebra_DatePicker({
        onSelect: function () {
            mostrarReportePronostico();
        }
    });
    $('#anho').Zebra_DatePicker({
        format: 'Y',
        onSelect: function () {
            cargarSemanaAnho()
        }
    });

    mostrarFiltro();
});

function validarFiltroSeleccionado() {
    var tipoReporte = $("#cbTipoReporte").val();
    if (tipoReporte == "-1") {
        alert("Debe seleccionar un tipo de reporte");
        return false;
    }

    if (tipoReporte == 1) {// diario
        var fecha = $("#fecha").val();
        if (fecha == "") {
            alert("Debe seleccionar una fecha");
            return false;
        }
    }
    if (tipoReporte == 2) {//semanal
        var numsemana = $("#cbSemanas").val();
        if (numsemana == "0") {
            alert("Debe seleccionar una semana");
            return false;
        }
    }

    return true;
}

function mostrarReportePronostico() {
    var tipoReporte = $("#cbTipoReporte").val();
    var fecha = getFecha();
    var semana = getSemana();
    //fecha = '20/09/2015';//TODO quitar

    $.ajax({
        type: 'POST',
        url: controlador + 'reporte/CargarListaPronostico',
        data: {
            tipoReporte: tipoReporte,
            fecha: fecha,
            semana: semana
        },
        success: function (aData) {
            $('#listado').html(aData);
        },
        error: function () {
            alert("Ha ocurrido un error");
        }
    });
}

function exportarReportePronostico() {
    var tipoReporte = $("#cbTipoReporte").val();
    var fecha = getFecha();
    var semana = getSemana();
    //fecha = '20/09/2015';//TODO quitar

    $.ajax({
        type: 'POST',
        url: controlador + 'reporte/ExportarExcel',
        data: {
            tipoReporte: tipoReporte,
            fecha: fecha,
            semana: semana
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

function verDetalleFaltante(calculoPtoCodi, sfecha) {
    var ptoCalculadocodi = calculoPtoCodi;
    var fecha = sfecha;
    popupVerDetalleFaltante(ptoCalculadocodi, fecha);
}


function popupVerDetalleFaltante(ptoCalculadocodi, fecha) {
    var tipoReporte = $("#cbTipoReporte").val();
    $.ajax({
        type: 'POST',
        contentType: 'application/json; charset=utf-8',
        traditional: true,
        url: controlador + 'reporte/ViewDetalleFaltante',
        data: JSON.stringify({
            tipoReporte: tipoReporte,
            ptoCalculadocodi: ptoCalculadocodi,
            fecha: fecha
        }),
        success: function (result) {
            $('#idDetalleFaltante').html(result);
            setTimeout(function () {
                $('#detalleFaltante').bPopup({
                    easing: 'easeOutBack',
                    speed: 450,
                    transition: 'slideDown',
                    modalClose: false
                });
            }, 50);

            $("#btnCerrarVerDetalle").click(function () {
                $('#detalleFaltante').bPopup().close();
            });
        },
        error: function (result) {
            alert('Ha ocurrido un error al generar vista');
        }
    });
}

function mostrarFiltro() {
    $("#reporte").css("display", "none");
    tipoRep = $("#cbTipoReporte").val();

    switch (tipoRep) {
        case '1': //diario
            mostrarReportePronostico();
            $(".filaDia").show();
            $(".filaSemana").hide();
            break;
        case '2': //semanal
            cargarSemanaAnho();
            $(".filaDia").hide();
            $(".filaSemana").show();
            break;
        case '-1':
        default:
            $(".filaDia").hide();
            $(".filaSemana").hide();
            break;
    }
}


function cargarSemanaAnho() {
    var anho = $('#anho').val();
    $('#hfAnho').val(anho);

    $.ajax({
        type: 'POST',
        url: controlador + 'reporte/CargarSemanas',

        data: { idAnho: $('#hfAnho').val() },

        success: function (aData) {
            $('#divSemana').html(aData);

            mostrarReportePronostico();

            $("#cbSemanas").change(function () {
                mostrarReportePronostico();
            });
        },
        error: function () {
            alert("Ha ocurrido un error");
        }
    });
}

function getSemana() {
    var semana = "";
    var cbsemana = $("#cbSemanas").val();
    if (cbsemana == "0" || cbsemana == "" || cbsemana == undefined) {
        semana = $("#hfSemana").val();
    } else {
        semana = cbsemana;
    }

    if (semana == "0" || semana == "") {
        semana = "1";
    }

    $("#cbSemanas").val(semana);
    $('#hfSemana').val(semana);
    $('#hfAnho').val($('#anho').val());
    semana = $("#hfSemana").val();
    anho = $('#hfAnho').val();

    if (semana !== undefined && anho !== undefined) {
        semana = anho.toString() + semana;
    } else {
        semana = '';
    }

    return semana;
}

function getFecha() {
    $('#hfFecha').val($('#fecha').val());
    fecha = $("#hfFecha").val();
    fecha = fecha !== undefined ? fecha : '';
    return fecha;
}