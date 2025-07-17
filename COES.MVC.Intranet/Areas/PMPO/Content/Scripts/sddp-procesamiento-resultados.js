var controlador = siteRoot + 'PMPO/ProcesamientoResultadosSDDP/';

var flagActivarRefresco = "";
var SEG_REFRESCA_LOG = 15;

var gifcargando = '<img src="' + siteRoot + 'Content/Images/loading1.gif" alt="" width="17" height="17" style="padding-left: 17px;">';
var imgOK = '<img src="' + siteRoot + 'Content/Images/ico-done.gif" alt="" width="17" height="17" style="">';
var imgERROR = '<img src="' + siteRoot + 'Content/Images/error.png" alt="" width="17" height="17" style="">';
var imgALERTA = '<img src="' + siteRoot + 'Content/Images/ico-info.gif" alt="" width="17" height="17" style="">';

$(document).ready(function () {
    $('#tab-container').easytabs({
        animate: false
    });
    $('#tab-container').easytabs('select', '#vistaReporte');

    //
    $("#filtro-anio").change(function () {
        cargarPeriodos();
    });
    $("#filtro-mes").change(function () {
        construirRuta();
        mostrarLogProceso();
    });

    $('#btnProcesarNew').click(function () {
        procesarArchivosNew();
    });

    //
    $('#btn-ver-codigo').click(function () {
        window.open(siteRoot + 'PMPO/CodigoSDDP/Index', '_blank').focus();
    });

    construirRuta();
    mostrarLogProceso();
    mostrarLoadingSddp();
});

function cargarPeriodos() {
    var anio = parseInt($("#filtro-anio").val()) || 0;

    $("#filtro-mes").empty()

    $.ajax({
        type: 'POST',
        url: controlador + "ListaMesxAnio",
        dataType: 'json',
        data: {
            anio: anio,
        },
        success: function (model) {
            if (model.Resultado != "-1") {

                for (var i = 0; i < model.ListaMes.length; i++) {
                    var obj = model.ListaMes[i];
                    $('#filtro-mes').append('<option value="' + obj.PmPeriCodi + "|" + obj.Semanadesc + '">' + obj.PmPeriNombre + '</option>');
                }

                construirRuta();
                mostrarLogProceso();
            } else {
                alert("Ha ocurrido un error: " + model.Mensaje);
            }
        },
        error: function (err) {
            alert("Ha ocurrido un error");
        }
    });
}

////////////////////////////////////////////////////////////////////////////////////
// Procesar archivos .csv
////////////////////////////////////////////////////////////////////////////////////

function construirRuta() {
    var rutabase = $('#hdRutaCarga').val();
    var semana = $('#filtro-mes').val().split("|")[1];
    var rutafinal = `${rutabase}\\${semana}`;

    $('#carpeta').val(rutafinal);
}

function mostrarLoadingSddp() {
    setInterval(function () {
        if (flagActivarRefresco == "S") {
            mostrarLogProceso();
        }
    }, SEG_REFRESCA_LOG * 1000);
}

function procesarArchivosNew() {
    var pmpericodi = $("#filtro-mes").val().split("|")[0];

    var carpeta = $('#carpeta').val();
    if (carpeta == "0" || carpeta == "" || carpeta == null) {
        $("#mensaje").show();
        mostrarMensaje("mensaje", 'No se ha definido la carpeta de donde se obtendrán los archivos.', $tipoMensajeAlerta, $modoMensajeCuadro);
        return;
    }

    var rutabase = $('#hdRutaCarga').val();
    if (carpeta.toLowerCase().indexOf(rutabase.toLowerCase()) == -1) {
        $("#mensaje").show();
        mostrarMensaje("mensaje", 'La carpeta de donde se obtendrán los archivos debe iniciar con: ' + rutabase, $tipoMensajeAlerta, $modoMensajeCuadro);
        return;
    }

    $('#logProceso').val('');
    flagActivarRefresco = 'N';
    $("#mensaje_log").hide();
    $("#mensaje").hide();

    if (confirm("¿Desea procesar el periodo seleccionado?")) {
        $.ajax({
            type: 'POST',
            url: controlador + 'ProcesarArchivos',
            dataType: 'json',
            data: {
                pmpericodi: pmpericodi,
                carpeta: carpeta
            },
            success: function (model) {
                if (model.Resultado != "-1" && model.Resultado != "-2") {
                    if (model.Resultado == "0") {
                        flagActivarRefresco = 'N';
                    } else {
                        flagActivarRefresco = 'S';
                        mostrarLogProceso();
                    }
                } else {
                    if (model.Resultado == "-2") {
                        mostrarMensaje("mensaje_log", "Existen mensajes de validación. <ul><li>" + model.ListaVal.join('</li><li>') + "</li></ul>", $tipoMensajeAlerta, $modoMensajeCuadro);
                        $("#mensaje_log").show();
                    } else {
                        mostrarMensaje("mensaje", model.Mensaje, $tipoMensajeError, $modoMensajeCuadro);
                        $("#mensaje").show();
                    }
                    flagActivarRefresco = 'N';
                }
            },
            error: function (err) {
                flagActivarRefresco = 'N';
                alert('Ha ocurrido un error');
            }
        });
    }
}

function mostrarLogProceso() {
    var pmpercodi = $("#filtro-mes").val().split("|")[0];

    $("#mensaje").hide();
    $("#div_log").hide();

    $.ajax({
        type: 'POST',
        url: controlador + 'ListaLogxMes',
        dataType: 'json',
        data: {
            pmpericodi: pmpercodi,
        },
        success: function (model) {
            if (model.Resultado != "-1") {
                if (model.Envio != null) {
                    $("#div_log").show();
                    $("#log_cod_envio").html(model.Envio.Enviocodi);
                    $("#log_usu_envio").html(model.Envio.Userlogin);
                    $("#log_fecha_envio").html(model.Envio.EnviofechaDesc);

                    //$('#tbl_envio').dataTable({
                    //    "scrollX": true,
                    //    "destroy": "true",
                    //    "sDom": 't',
                    //    "ordering": false,
                    //    "iDisplayLength": -1
                    //});

                    $('#div_log_detalle').html(dibujarTablaLog(model.ListaLog));
                    //$('#reporteLog').dataTable({
                    //    "scrollX": true,
                    //    "destroy": "true",
                    //    "sDom": 't',
                    //    "ordering": false,
                    //    "iDisplayLength": -1
                    //});
                }

                flagActivarRefresco = model.Resultado == "0" ? 'S' : 'N';

                if (model.Resultado == "0") {

                    var porcentaje = model.Envio != null ? model.Envio.Enviodesc : '';
                    if (porcentaje != '-1') {
                        $("#mensaje").show();
                        var htmlMsj = `Procesamiento de Resultados de SDDP en ejecución. Porcentaje de avance: ${porcentaje}%. ${gifcargando}`;
                        mostrarMensaje("mensaje", htmlMsj, $tipoMensajeAlerta, $modoMensajeCuadro);
                    }
                }
                else {
                }
            } else {
                flagActivarRefresco = 'N';
                mostrarMensaje("mensaje", model.Mensaje, $tipoMensajeAlerta, $modoMensajeCuadro);
                $("#mensaje").show();
            }
        },
        error: function (err) {
            flagActivarRefresco = 'N';
            mostrarMensaje("mensaje", 'Ha ocurrido un error', $tipoMensajeError, $modoMensajeCuadro);
        }
    });

}

function dibujarTablaLog(lista) {
    if (lista.length == 0)
        return "<b></b>";

    var cadena = '';
    cadena += `
    <table border="0" class="pretty tabla-icono dataTable" cellspacing="0" width="100%" id="reporteLog">
        <thead>
            <tr>
                <th style='width: 40px'>Estado</th>
                <th style='width: 70px'>Fecha</th>
                <th style='width: 70px'>Hora</th>
                <th style='width: 400px'>Descripción</th>
            </tr>
        </thead>
        <tbody>
    `;

    for (key in lista) {
        var item = lista[key];
        var strIEstado = '';
        switch (item.Pmologtipo) {
            case 3:
                strIEstado = '<a title="">' + imgOK + '</a>';
                break;
            case 4:
                strIEstado = '<a title="">' + imgERROR + '</a>';
                break;
            case 5:
                strIEstado = '<a title="">' + imgALERTA + '</a>';
                break;
            default:
                strIEstado = item.PmologtipoDesc;
                break;
        }

        cadena += `
            <tr>
                <td style="height: 24px">${strIEstado}</td>
                <td style="height: 24px">${item.FechaDesc}</td>
                <td style="height: 24px">${item.HoraDesc}</td>
                <td style="height: 24px; text-align: left;padding-left: 10px;">${item.LogDesc}</td>
            </tr>
        `;
    }
    cadena += "</tbody></table>";

    return cadena;
}

////////////////////////////////////////////////////////////////////////////////////
// Descargar reporte
////////////////////////////////////////////////////////////////////////////////////
function exportarArchivosNew(archivoExportar) {
    var pmpericodi = $("#filtro-mes").val().split("|")[0];

    if (archivoExportar == 0) {
        $("#mensaje").show();
        mostrarMensaje("mensaje", 'No se ha seleccionado un reporte.', $tipoMensajeAlerta, $modoMensajeCuadro);
        return;
    }

    $("#mensaje").hide();

    $.ajax({
        type: 'POST',
        url: controlador + 'ExportarArchivos',
        dataType: 'json',
        data: {
            pmpericodi: pmpericodi,
            codigoReporte: archivoExportar,
        },
        success: function (model) {
            if (model.Resultado != "-1") {
                if (archivoExportar == 18) {
                    document.location.href = controlador + 'DescargarResultadosSDDP?formato=' + 1 + '&file=' + model.Resultado
                } else {
                    document.location.href = controlador + 'descargar?formato=' + 1 + '&file=' + model.Resultado
                }

                $("#mensaje").show();
                mostrarMensaje("mensaje", "Exportación realizada", $tipoMensajeExito, $modoMensajeCuadro);
            }
            else {
                $("#mensaje").show();
                mostrarMensaje("mensaje", model.Mensaje, $tipoMensajeAlerta, $modoMensajeCuadro);
            }

        },
        error: function (err) {
            alert('Ha ocurrido un error');
        }
    });
}
