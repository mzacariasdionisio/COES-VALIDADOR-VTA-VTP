var controlador = siteRoot + "intervenciones/consultasyreportes/";
var pagActualGlobal;

$(document).ready(function () {

    $('.txtFecha').inputmask({
        mask: "1/2/y",
        placeholder: "dd/mm/yyyy",
        alias: "datetime"
    });

    $('.txtFecha').Zebra_DatePicker({
        readonly_element: false
    });

    $('#emprCodi').multipleSelect({
        placeholder: "-- Todos --",
        selectAll: false,
        allSelected: onoffline
    });

    $('#btnBuscar').click(function () {
        cargarListaHtml();
    });

    $('#btnImprimir').click(function () {
        generarExcelQuebradoOSINERGMIN();
    });

    $('#btnGenerarExcel').click(function () {
        generarExcelOSINERGMIN();
    });


    $('#tipoProCodi').change(function () {
        if (parseInt($('#tipoProCodi').val()) == 1000) {
            document.getElementById("fecIni").innerHTML = "Fecha:";
            $(".fechFin").hide();
        } else {
            document.getElementById("fecIni").innerHTML = "Fecha Inicio:";
            $(".fechFin").show();
        }
    });

    if (parseInt($('#tipoProCodi').val()) == 1000) {
        document.getElementById("fecIni").innerHTML = "Fecha:";
        $(".fechFin").hide();
    } else {
        document.getElementById("fecIni").innerHTML = "Fecha Inicio:";
        $(".fechFin").show();
    }
});

//funcion que calcula el ancho disponible para la tabla reporte
function getHeightTablaListado() {
    return $(window).height()
        - $("header").height()
        - $("#cntTitulo").height() - 2
        - $("#Reemplazable .form-title").height()
        - 15
        - $("#Contenido").parent().height() //Filtros
        - 14 //<br>
        - $(".dataTables_filter").height()
        - $(".dataTables_scrollHead").height()
        - 61 //- $(".footer").height() - 10
        - 80
        ;
}

function cargarListaHtml() {
    var tipoProCodi = document.getElementById('tipoProCodi').value;
    var emprCodi = JSON.stringify($('#emprCodi').val());
    var fechaInicio = $('#Entidad_Interfechaini').val();
    var fechaFin = $('#Entidad_Interfechafin').val();

    if (parseInt($('#tipoProCodi').val()) == 1000) {
        var fechaFin = $('#Entidad_Interfechaini').val();
    }

    if (tipoProCodi == null) {
        alert("Seleccione un Tipo de Programacion")
        return;
    }

    if (fechaInicio == null || fechaInicio == undefined || fechaInicio == "") {
        alert("Seleccione una fecha de inicio")
        return;
    }
    if (fechaFin == null || fechaFin == undefined || fechaFin == "") {
        alert("Seleccione una fecha fin")
        return;
    }

    //// Valida consistencia del rango de fechas
    //if (Date.parse(fechaInicio) > Date.parse(fechaFin)) {
    //    alert("La fecha de inicio no puede ser mayor que la fecha de fin");
    //    return;
    //}

    // Valida consistencia del rango de fechas
    if (CompararFechas(fechaInicio, fechaFin) == false) {
        alert("La fecha de inicio no puede ser mayor que la fecha de fin");
        return;
    }

    $("#listado").html('');
    $.ajax({        
        url: controlador + "RptIntervencionesListadoHtml",
        type: 'POST',
        data: { tipoProCodi: tipoProCodi, emprCodi: emprCodi, fechaInicio: toDate(fechaInicio).toISOString(), fechaFin: toDate(fechaFin).toISOString() },
        success: function (data) {
            $("#listado").hide();
            $('#listado').css("width", $('#mainLayout').width() + "px");
            if (data !== null) {
                data = data.replace("<tbody><tr><td colspan='9' style='text-align: left'></td><td class='alerta_ems'></td></tr></tbody>", '<tbody></tbody>');
            }
            $('#listado').html(data);
            var nuevoTamanioTabla = getHeightTablaListado();
            $("#listado").show();
            nuevoTamanioTabla = nuevoTamanioTabla > 250 ? nuevoTamanioTabla : 250;

            $('#tabla').dataTable({
                "ordering": true,
                "info": false,
                "searching": true,
                "paging": false,
                "iDisplayLength": 25,
                "scrollX": true,
                "scrollY": $('#listado').height() > 250 ? nuevoTamanioTabla + "px" : "100%"
            });
        },
        error: function (err) {
            alert("Lo sentimos, ha ocurrido el siguiente error");
        }
    });
}

function generarExcelQuebradoOSINERGMIN() {
    var tipoProCodi = document.getElementById('tipoProCodi').value;    
    var emprCodi = JSON.stringify($('#emprCodi').val());
    var fechaInicio = $('#Entidad_Interfechaini').val();
    var fechaFin = $('#Entidad_Interfechafin').val();

    if (parseInt($('#tipoProCodi').val()) == 1000) {
        var fechaFin = $('#Entidad_Interfechaini').val();
    }

    if (tipoProCodi == null) {
        alert("Seleccione un Tipo de Programacion")
        return;
    }

    if (fechaInicio == null || fechaInicio == undefined || fechaInicio == "") {
        alert("Seleccione una fecha de inicio")
        return;
    }
    if (fechaFin == null || fechaFin == undefined || fechaFin == "") {
        alert("Seleccione una fecha fin")
        return;
    }

    //// Valida consistencia del rango de fechas
    //if (Date.parse(fechaInicio) > Date.parse(fechaFin)) {
    //    alert("La fecha de inicio no puede ser mayor que la fecha de fin");
    //    return;
    //}

    // Valida consistencia del rango de fechas
    if (CompararFechas(fechaInicio, fechaFin) == false) {
        alert("La fecha de inicio no puede ser mayor que la fecha de fin");
        return;
    }
    
    $.ajax({
        type: 'POST',
        url: controlador + 'GenerarExcelQuebradoIntervenciones',
        data: { tipoProCodi: tipoProCodi, emprCodi: emprCodi, fechaInicio: toDate(fechaInicio).toISOString(), fechaFin: toDate(fechaFin).toISOString() },
        dataType: 'json',
        success: function (result) {
            if (result == -1) {
                alert("No se encuentra datos a exportar!")
            }
            else if (result != -1) {
                document.location.href = controlador + 'Descargar?file=' + result;               
            }
            else {
                alert("Ha ocurrido un error");
            }
        },
        error: function (err) {
            alert("Ha ocurrido un error");
        }
    });
}

function generarExcelOSINERGMIN() {
    var tipoProCodi = document.getElementById('tipoProCodi').value;    
    var emprCodi = JSON.stringify($('#emprCodi').val());

    var flgformatosinergmin = false;
    if ($('#flgformatosinergmin').is(':checked')) {
        flgformatosinergmin = true;
    }

    var fechaInicio = $('#Entidad_Interfechaini').val();
    var fechaFin = $('#Entidad_Interfechafin').val();

    if (parseInt($('#tipoProCodi').val()) == 1000) {
        var fechaFin = $('#Entidad_Interfechaini').val();
    }

    if (tipoProCodi == null) {
        alert("Seleccione un Tipo de Programacion")
        return;
    }

    if (fechaInicio == null || fechaInicio == undefined || fechaInicio == "") {
        alert("Seleccione una fecha de inicio")
        return;
    }
    if (fechaFin == null || fechaFin == undefined || fechaFin == "") {
        alert("Seleccione una fecha fin")
        return;
    }

    //// Valida consistencia del rango de fechas
    //if (Date.parse(fechaInicio) > Date.parse(fechaFin)) {
    //    alert("La fecha de inicio no puede ser mayor que la fecha de fin");
    //    return;
    //}

    // Valida consistencia del rango de fechas
    if (CompararFechas(fechaInicio, fechaFin) == false) {
        alert("La fecha de inicio no puede ser mayor que la fecha de fin");
        return;
    }

    $.ajax({
        type: 'POST',
        url: controlador + 'GenerarExcelIntervenciones',
        data: { tipoProCodi: tipoProCodi, emprCodi: emprCodi, flgFormatoOsinergmin: flgformatosinergmin, fechaInicio: toDate(fechaInicio).toISOString(), fechaFin: toDate(fechaFin).toISOString() },
        dataType: 'json',
        success: function (result) {
            if (result == -1) {
                alert("No se encuentra datos a exportar!")
            }
            else if (result != -1) {
                document.location.href = controlador + 'Descargar?file=' + result;                
            }
            else {
                alert("Ha ocurrido un error");
            }
        },
        error: function (err) {
            alert("Ha ocurrido un error");
        }
    });
}

function toDate(dateStr) {
    var parts = dateStr.split("/");
    return new Date(parts[2], parts[1] - 1, parts[0]);
}

function CompararFechas(fecha1, fecha2) {

    //Split de las fechas recibidas para separarlas
    var x = fecha1.split('/');
    var z = fecha2.split('/');

    //Cambiamos el orden al formato americano, de esto dd/mm/yyyy a esto mm/dd/yyyy
    fecha1 = x[1] + '/' + x[0] + '/' + x[2];
    fecha2 = z[1] + '/' + z[0] + '/' + z[2];

    //Comparamos las fechas
    if (Date.parse(fecha1) > Date.parse(fecha2)) {
        return false;
    } else {
        return true;
    }
}