var controlador = siteRoot + "intervenciones/consultasyreportes/";

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

    buscar();
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
    var fechaIni = $('#Entidad_Interfechaini').val();
    var fechaFin = $('#Entidad_Interfechafin').val();

    if (fechaIni == "" && fechaFin == "") {
        alert("No ha colocado las fechas para realizar la consulta");
        return;
    }

    // Valida filtros de fecha cuando falta seleccionar cualquiera de ellas
    if (fechaIni == null || fechaIni == undefined || fechaIni == "") {
        alert("Seleccione una fecha de inicio");
        return;
    }

    if (fechaFin == null || fechaFin == undefined || fechaFin == "") {
        alert("Seleccione una fecha fin");
        return;
    }

    //// Valida consistencia del rango de fechas
    //if (Date.parse(fechaIni) > Date.parse(fechaFin)) {
    //    alert("La fecha de inicio no puede ser mayor que la fecha de fin");
    //    return;
    //}   

    // Valida consistencia del rango de fechas
    if (CompararFechas(fechaIni, fechaFin) == false) {
        alert("La fecha de inicio no puede ser mayor que la fecha de fin");
        return;
    }

    mostrarLista(-1);
}

function mostrarLista(tipoMensaje) {
    var fechaIni = $('#Entidad_Interfechaini').val();
    var fechaFin = $('#Entidad_Interfechafin').val();

    $("#listado").html('');
    $.ajax({
        type: 'POST',
        url: controlador + "RptMensajesListado",
        data: { fechaIni: toDate(fechaIni).toISOString(), fechaFin: toDate(fechaFin).toISOString(), estado: tipoMensaje },
        success: function (evt) {
            $("#listado").hide();

            var anchoTabla = $('#mainLayout').width() - 190 + "px";

            var nuevoTamanioTabla = getHeightTablaListado();
            $("#listado").show();
            nuevoTamanioTabla = nuevoTamanioTabla > 250 ? nuevoTamanioTabla : 250;

            $('#listado').html(evt);
            $('#listado2').css("width", anchoTabla); //180 width de la bandeja
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
    var fechaIni = $('#Entidad_Interfechaini').val();
    var fechaFin = $('#Entidad_Interfechafin').val();

    if (fechaIni == "" && fechaFin == "") {
        alert("No ha colocado las fechas para realizar la consulta");
        return;
    }

    // Valida filtros de fecha cuando falta seleccionar cualquiera de ellas
    if (fechaIni == null || fechaIni == undefined || fechaIni == "") {
        alert("Seleccione una fecha de inicio");
        return;
    }
    if (fechaFin == null || fechaFin == undefined || fechaFin == "") {
        alert("Seleccione una fecha fin");
        return;
    }

    //// Valida consistencia del rango de fechas
    //if (Date.parse(fechaIni) > Date.parse(fechaFin)) {
    //    alert("La fecha de inicio no puede ser mayor que la fecha de fin");
    //    return;
    //}

    // Valida consistencia del rango de fechas
    if (CompararFechas(fechaIni, fechaFin) == false) {
        alert("La fecha de inicio no puede ser mayor que la fecha de fin");
        return;
    }

    $.ajax({
        type: 'POST',
        url: controlador + 'GenerarExcelMensajes',
        data: { fechaIni: toDate(fechaIni).toISOString(), fechaFin: toDate(fechaFin).toISOString() },
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

function verContenido(id) {
    $.ajax({
        type: 'POST',
        url: controlador + 'VerContenidoMensaje',
        data: {
            idMensaje: id
        },
        dataType: 'json',
        success: function (model) {
            if (model.StrMensaje !== "-1") {
                $('#contenidoLogMensaje').html(getHtmlSaltoLinea(model.StrMensaje));
                setTimeout(function () {
                    $('#popupLogMensaje').bPopup({
                        autoClose: false
                    });
                }, 50);
            }
            else {
                alert(model.StrMensaje);
            }
        },
        error: function (err) {
            mostrarError('Ha ocurrido un error');
        }
    });
}

////////////////////////////////////////////////////////////////////////////////
/// Util
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

//Obtener formato html
function getHtmlSaltoLinea(str) {
    if (str != null) {
        //return str.replace(/(?:\r\n|\r|\n)/g, '<br>');
    }
    return str;
}
