var controlador = siteRoot + 'Migraciones/Reporte/';

$(function () {
    //Fechas    
    $('#txtFechaDesde').Zebra_DatePicker({
        format: "d/m/Y",
        pair: $('#txtFechaHasta'),
        direction: false,
    });

    $('#txtFechaHasta').Zebra_DatePicker({
        format: "d/m/Y",
        pair: $('#txtFechaDesde'),
        direction: [true, "01/01/2050"]
    });

    $('#btnConsultar').click(function () {
        mostrarListadoRepHidrologia();
    });

    $("#btnManualUsuario").click(function () {
        window.location = controlador + 'DescargarManualUsuario';
    });

    $('#btnExportar').click(function () {
        exportarRepHidro();
    });

    $('#btnConfigurarReporte').on('click', function () {
        abrirVentanaConfigReporte();
    });

    mostrarListadoRepHidrologia();
    
});

function abrirVentanaConfigReporte() {
    var idReporte = $('#hfIdReporte').val();
    var url = siteRoot + 'ReportesMedicion/formatoreporte/IndexDetalle?id=' + idReporte;
    window.open(url, '_blank');
}


function mostrarListadoRepHidrologia() {
    limpiarBarraMensaje("mensaje");

    var objData = getObjetoFiltro();
    var msj = validarConsulta(objData);

    if (msj == "") {

        $.ajax({
            type: 'POST',
            url: controlador + "listarRepHidrologia",
            data: {
                fechaInicial: objData.FechaInicio,
                fechaFinal: objData.FechaFin
            },
            success: function (evt) {
                if (evt.Resultado != "-1") {

                    var htmlReporte = dibujarTablaReporte(evt.ListadoHidrologia);
                    $("#listadoHidrologiaSemanal").html(htmlReporte);

                    $('#tablaReporteHidro').dataTable({
                        "scrollY": 480,
                        "scrollX": false,
                        "sDom": 'ft',
                        "ordering": false,
                        "iDisplayLength": -1
                    });

                } else {
                    mostrarMensaje('mensaje', 'error', evt.Mensaje);
                }
            },
            error: function (err) {
                mostrarMensaje('mensaje', 'error', 'Ha ocurrido un error.');
            }
        });
    } else {
        mostrarMensaje('mensaje', 'error', msj);
    }
}

function getObjetoFiltro() {

    var fechaInicio = $('#txtFechaDesde').val();
    var fechaFin = $('#txtFechaHasta').val();

    var obj = {
        FechaInicio: fechaInicio,
        FechaFin: fechaFin
    };

    return obj;
}

function validarConsulta(objFiltro) {
    var listaMsj = [];

    // Valida consistencia del rango de fechas
    var fechaInicio = objFiltro.FechaInicio;
    var fechaFin = objFiltro.FechaFin;
    if (fechaInicio != "" && fechaFin != "") {
        if (CompararFechas(fechaInicio, fechaFin) == false) {
            listaMsj.push("La fecha de inicio no puede ser mayor que la fecha de fin.");
        }
    }

    var msj = listaMsj.join('\n');
    return msj;
}

function dibujarTablaReporte(listado) {

    var cadena = '';
    cadena += `
    <table border="0" class="pretty tabla-icono" id="tablaReporteHidro" >
       <thead>
           <tr style="height: 22px;">
               <th>Fecha</th>
               <th>Elemento Hidrológico</th>
               <th>Unidad</th>
               <th>Valor</th>
               <th>Usuario</th>
               <th>Hora Registro</th>
            </tr>
        </thead>
        <tbody>
    `;

    for (var key in listado) {
        var item = listado[key];

        var valor = item.H1.toFixed(3);
        cadena += `
            <tr>
                <td>${item.MedifechaPto}</td>
                <td>${item.OrigenPtomedidesc}</td>
                <td>${item.Tipoinfodesc}</td>
                <td>${valor}</td>
                <td>${item.Lastuser}</td>
                <td>${item.LastdateDesc}</td>
           </tr >
        `;
    }

    cadena += `
        </tbody >
    </table >
    `;

    return cadena;
}

function exportarRepHidro() {
    limpiarBarraMensaje("mensaje");

    var objData = getObjetoFiltro();
    var msj = validarConsulta(objData);

    if (msj == "") {
        $.ajax({
            type: 'POST',
            url: controlador + 'GenerarArchivoRH',
            data: {
                fechaInicial: objData.FechaInicio,
                fechaFinal: objData.FechaFin
            },
            dataType: 'json',
            success: function (evt) {
                if (evt.Resultado != "-1") {
                    window.location = controlador + "ExportarReporte?file_name=" + evt.Resultado;
                } else {
                    mostrarMensaje('mensaje', 'error', 'Error: ' + evt.Mensaje);
                }
            },
            error: function (err) {
                mostrarMensaje('mensaje', 'error', 'Ha ocurrido un error.');
            }
        });
    } else {
        mostrarMensaje('mensaje', 'error', msj);
    }
}


/**********************************************/
/************ Funciones Generales  ************/
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

function cerrarPopup(id) {
    $("#" + id).bPopup().close()
}

function abrirPopup(contentPopup) {
    setTimeout(function () {
        $("#" + contentPopup).bPopup({
            easing: 'easeOutBack',
            speed: 450,
            transition: 'slideDown',
            modalClose: false
        });
    }, 50);
}


function mostrarMensaje(id, tipo, mensaje) {
    $("#" + id).css("display", "block");
    $('#' + id).removeClass();
    $('#' + id).addClass('action-' + tipo);
    $('#' + id).html(mensaje);

}

function limpiarBarraMensaje(id) {
    $('#' + id).css("display", "none");
    $('#' + id).removeClass();
    $('#' + id).addClass('action-message');
    $('#' + id).html('');
}
