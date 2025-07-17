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

    $('#btnBuscar').click(function () {
        buscar();
    });

    $('#btnGenerarExcel').click(function () {
        generarExcel();
    }); 
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

function buscar() {
    var fechaInicio = $('#Entidad_Interfechaini').val();
    var fechaFin = $('#Entidad_Interfechafin').val();

    //// Valida filtros de fecha cuando falta seleccionar cualquiera de ellas
    //if (fechaInicio != "" && fechaFin == "") {
    //    alert("Seleccione una fecha de inicio");
    //    return;
    //} else if (fechaInicio == "" && fechaFin != "") {
    //    alert("Seleccione una fecha de fin");
    //    return;
    //}

    // Valida filtros de fecha cuando falta seleccionar cualquiera de ellas
    if (fechaInicio == null || fechaInicio == undefined || fechaInicio == "") {
        alert("Seleccione una fecha de inicio");
        return;
    }

    if (fechaFin == null || fechaFin == undefined || fechaFin == "") {
        alert("Seleccione una fecha fin");
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

    pintarBusqueda(1);
}

function pintarBusqueda(nroPagina) {
    mostrarLista(nroPagina);
}

function mostrarLista(nroPagina) {
    var paginaActual = $('#hfNroPagina').val(nroPagina); //Obtengo la pagina actual
    pagActualGlobal = paginaActual.val();

    var fechaInicio = $('#Entidad_Interfechaini').val();
    var fechaFin = $('#Entidad_Interfechafin').val();

    var actividad = $('#sActividad').val();

    if (fechaInicio == "" && fechaFin == "") {
        fechaInicio = null;
        fechaFin = null;
    } else if (fechaInicio != "" && fechaFin != "") {
        fechaInicio = toDate(fechaInicio).toISOString();
        fechaFin = toDate(fechaFin).toISOString();
    }

    $("#listado").html('');
    $.ajax({
        type: 'POST',
        url: controlador + "RptHistorialListado",
        data: { fechaInicio: fechaInicio, fechaFin: fechaFin, nroPagina: nroPagina, actividad: actividad },
        success: function (evt) {
            $("#listado").hide();
            $('#listado').css("width", $('#mainLayout').width() + "px");

            var nuevoTamanioTabla = getHeightTablaListado();
            $("#listado").show();
            nuevoTamanioTabla = nuevoTamanioTabla > 250 ? nuevoTamanioTabla : 250;

            $('#listado').html(evt);
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
            alert("Lo sentimos, ha ocurrido un error inesperado");
        }
    });
}

function generarExcel() {
    var fechaInicio = $('#Entidad_Interfechaini').val();
    var fechaFin = $('#Entidad_Interfechafin').val();

    var actividad = $('#sActividad').val();

    // Valida filtros de fecha cuando falta seleccionar cualquiera de ellas
    if (fechaInicio != "" && fechaFin == "") {
        alert("Seleccione una fecha de inicio");
        return;
    } else if (fechaInicio == "" && fechaFin != "") {
        alert("Seleccione una fecha de fin");
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

    if (fechaInicio == "" && fechaFin == "") {
        fechaInicio = null;
        fechaFin = null;
    } else if (fechaInicio != "" && fechaFin != "") {
        fechaInicio = toDate(fechaInicio).toISOString();
        fechaFin = toDate(fechaFin).toISOString();
    }

    $.ajax({
        type: 'POST',
        url: controlador + 'GenerarExcelHistorial',
        data: { fechaInicio: fechaInicio, fechaFin: fechaFin, actividad: actividad },
        dataType: 'json',
        success: function (result) {
            if (result == -2) {
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


//function resizePage() {
//    const container = $("#TableContainer");
//    const height = container.height() - container.find(".dataTables_scrollHead").height();
//    updateDataTable(height + "px");
//};

//var resizeTimer;

//$(window).resize(function () {
//    clearTimeout(resizeTimer);
//    resizeTimer = setTimeout(resizePage, 100);
//});

//function updateDataTable(scrollHeight) {
//    $('#tabla').DataTable(
//        {
//            destroy: true,
//            paging: false,
//            "bFilter": false,
//            "bInfo": false,
//            scrollY: scrollHeight,
//            columnDefs: [{ width: "16%", targets: 0 }]
//        }
//    );
//}

//$(document).ready(function () {
//    updateDataTable('1px'); // give it any height, it will be changed by the timer event, but it needs some size for the page to work
//    resizeTimer = setTimeout(resizePage, 100);
//});

