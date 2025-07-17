var controlador = siteRoot + 'CalculoResarcimiento/Consolidado/';

const REPORTEEXCEL_COMPARATIVOSEMESTRALTRIMESTRAL = 8;

$(function () {
    $('#btnDescargarComparativo').click(function () {
        limpiarBarraMensaje("mensaje_comparativo");
        var periodoTrim = parseInt($("#cbPeriodoTrimestral").val()) || 0;
        if (periodoTrim == 0) {
            mostrarMensaje('mensaje_comparativo', 'error', 'Debe seleccionar un periodo trimestral válido.');
        } else {
            exportarReporteEnExcel(REPORTEEXCEL_COMPARATIVOSEMESTRALTRIMESTRAL);
            cerrarPopup('popupComparativo');
        }

    });

});

function mostrarListadoPeriodosEnlazados() {
    limpiarBarraMensaje("mensaje_comparativo");
    var datos = {};
    datos = getDatosReporte();

    var msg = validarCamposReporte(datos, 1);

    var periodoId = datos.Periodo;
    if (msg == "") {
        $.ajax({
            type: 'POST',
            url: controlador + 'ListarPeriodosTrimestralesAsociados',
            data: {
                periodoId: periodoId
            },
            dataType: 'json',
            success: function (evt) {
                if (evt.Resultado != "-1") {
                    var htmlPT = dibujarListaPeriodosTrimestrales(evt.ListaPeriodos);
                    $("#lstPeriodosTrimestrales").html(htmlPT);
                    abrirPopup("popupComparativo");
                } else {
                    mostrarMensaje('mensaje', 'error', 'Error: ' + evt.Mensaje);
                }
            },
            error: function (err) {
                mostrarMensaje('mensaje', 'error', 'Ha ocurrido un error.');
            }
        });
       
    } else {
        mostrarMensaje('mensaje', 'error', msg);
    }
}

function dibujarListaPeriodosTrimestrales(listaPeriodosT) {   
    var cadena = '';
    cadena += `
    <div style="width: 100px; float:left;text-align:center;">Periodo:</div>
    <div style="width: 180px; float:left;">
          <select id="cbPeriodoTrimestral" style = "width: 150px;">
               <option value="0">-SELECCIONAR-</option>
     `;
    for (key in listaPeriodosT) {
        var item = listaPeriodosT[key];    
        cadena += `
                    <option value="${item.Repercodi}">${item.Repernombre}</option>
        `;
    }
    cadena += `
          </select>
    </div>

    `;  

    return cadena;
}

////////////////////////////////////////////
//             Reportes                   //
////////////////////////////////////////////

function exportarReporteEnExcel(codigoReporte) {
    limpiarBarraMensaje("mensaje");

    var datos = {};
    datos = getDatosReporte();

    var msg = validarCamposReporte(datos, codigoReporte);

    var periodoId = datos.Periodo ;
    var periodoTrim = parseInt($("#cbPeriodoTrimestral").val()) || 0;

    if (msg == "") {
        $.ajax({
            type: 'POST',
            url: controlador + 'GenerarReporteEnExcel',
            data: {
                codigoReporte: codigoReporte,
                periodoId: periodoId,
                periodoTrimestral: periodoTrim
            },
            dataType: 'json',
            success: function (evt) {
                if (evt.Resultado != "-1") {
                    window.location = controlador + "Exportar?file_name=" + evt.Resultado;
                } else {
                    mostrarMensaje('mensaje', 'error', 'Error: ' + evt.Mensaje);
                }
            },
            error: function (err) {
                mostrarMensaje('mensaje', 'error', 'Ha ocurrido un error.');
            }
        });
    } else {
        mostrarMensaje('mensaje', 'error', msg);
    }
}

function exportarReporteEnWord(codigoReporte) {
    limpiarBarraMensaje("mensaje");

    var datos = {};
    datos = getDatosReporte();

    var msg = validarCamposReporte(datos, codigoReporte);

    var periodoId = datos.Periodo;

    if (msg == "") {
        $.ajax({
            type: 'POST',
            url: controlador + 'GenerarReporteEnWord',
            data: {
                codigoReporte: codigoReporte,
                periodoId: periodoId
            },
            dataType: 'json',
            success: function (evt) {
                if (evt.Resultado != "-1") {
                    window.location = controlador + "Exportar?file_name=" + evt.Resultado;
                } else {
                    mostrarMensaje('mensaje', 'error', 'Error: ' + evt.Mensaje);
                }
            },
            error: function (err) {
                mostrarMensaje('mensaje', 'error', 'Ha ocurrido un error.');
            }
        });
    } else {
        mostrarMensaje('mensaje', 'error', msg);
    }
}

function exportarReporteEnZip(codigoReporte) {
    limpiarBarraMensaje("mensaje");
    var datos = {};
    datos = getDatosReporte();

    var msg = validarCamposReporte(datos, codigoReporte);

    var periodoId = datos.Periodo;

    if (msg == "") {
        $.ajax({
            type: 'POST',
            url: controlador + 'GenerarReporteEnZip',
            data: {
                codigoReporte: codigoReporte,
                periodoId: periodoId
            },
            dataType: 'json',
            success: function (evt) {
                if (evt.Resultado != "-1") {
                    window.location = controlador + "ExportarZip?file_name=" + evt.Resultado + "&reporteId=" + evt.idReporteZip;
                } else {
                    mostrarMensaje('mensaje', 'error', 'Error: ' + evt.Mensaje);
                }
            },
            error: function (err) {
                mostrarMensaje('mensaje', 'error', 'Ha ocurrido un error.');
            }
        });
    } else {
        mostrarMensaje('mensaje', 'error', msg);
    }
}

////////////////////////////////////////////
//             General                    //
////////////////////////////////////////////

function getDatosReporte() {
    var obj = {};

    obj.Periodo = parseInt($("#cbPeriodo").val()) || 0;   

    return obj;
}

function validarCamposReporte(datos, codigoReporte) {
    var msj = "";

    if (codigoReporte == REPORTEEXCEL_COMPARATIVOSEMESTRALTRIMESTRAL) {
        if (datos.Periodo == 0) {
            msj += "<p>Debe seleccionar un periodo semestral válido.</p>";
        }
    } else {
        if (datos.Periodo == 0) {
            msj += "<p>Debe seleccionar un periodo válido.</p>";
        }
    }    

    return msj;
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