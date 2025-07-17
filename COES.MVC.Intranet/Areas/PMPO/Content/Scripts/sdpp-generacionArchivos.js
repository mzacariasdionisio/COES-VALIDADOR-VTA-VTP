var controlador = siteRoot + 'PMPO/GeneracionArchivosDAT/';
var ANCHO_REPORTE = 1000;

$(document).ready(function () {
    $("#filtro-anio").change(function () {
        cargarPeriodos();
    });
    $("#filtro-mes").change(function () {
        consultarResultadoDat('S');
        consultarResultadoDat('M');
    });


    $('#btn-relacion-equipo').click(function () {
        location.href = controlador + "Correlaciones";
    });
    $('#btn-relacion-barra').click(function () {
        location.href = controlador + "Dbus";
    });

    //semanal
    $('#btnProcesar_semanal').click(function () {
        guardarDat('S');
    });
    $('#btnDescargar_semanal').click(function () {
        descargarDat('S');
    });

    //mensual
    $('#btnProcesar_mensual').click(function () {
        guardarDat('M');
    });
    $('#btnDescargar_mensual').click(function () {
        descargarDat('M');
    });

    //
    consultarResultadoDat('S');
    consultarResultadoDat('M');
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
                    $('#filtro-mes').append('<option value="' + obj.PmPeriCodi + '">' + obj.PmPeriNombre + '</option>');
                }

                consultarResultadoDat('S');
                consultarResultadoDat('M');
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
// Generar archivos .dat
////////////////////////////////////////////////////////////////////////////////////

function consultarResultadoDat(horizonte) {
    var pmpericodi = parseInt($("#filtro-mes").val()) || 0;
    ANCHO_REPORTE = $("#mainLayout").width() > 1100 ? $("#mainLayout").width() - 30 : 1100;

    var prefijo = (horizonte == "S") ? "semanal" : "mensual";
    $("#div_formato-" + prefijo).hide();

    var prefijo = (horizonte == "S") ? "semanal" : "mensual";

    $.ajax({
        type: 'POST',
        url: controlador + "ListaFormatoXMes",
        dataType: 'json',
        data: {
            pmpericodi: pmpericodi,
            horizonte: horizonte
        },
        success: function (evt) {
            if (evt.Resultado != "-1") {

                $("#reporte_" + prefijo).html(dibujarTablaReporteSemanal(evt.ListaPmpoformato, prefijo));

                $('#listado').css("width", ANCHO_REPORTE + "px");
                $('#table-resumen-' + prefijo).dataTable({
                    "sDom": 't',
                    "ordering": false,
                    "iDisplayLength": -1,
                    "stripeClasses": []
                });

                //
                $("#chkSeleccionar").unbind();
                $("#chkSeleccionar").prop('checked', false);

                $("#chkSeleccionar-" + prefijo).on("click", function () {
                    var check = $('#chkSeleccionar-' + prefijo).is(":checked");
                    $(".chkSeleccion-" + prefijo).prop("checked", check);
                });

                $("#div_formato-" + prefijo).show();
            } else {
                alert("Ha ocurrido un error: " + evt.Mensaje);
            }
        },
        error: function (err) {
            alert("Ha ocurrido un error");
        }
    });
}

function dibujarTablaReporteSemanal(lista, prefijo) {

    var cadena = '';
    cadena += `
    <div style='clear:both; height:5px'></div>
    <table id='table-resumen-${prefijo}' border='1' class='pretty tabla-icono' cellspacing='0' style='width: 100%'>
        <thead>
            <tr>
                <th style='width: 50px'>Orden</th>
                <th style='width: 150px'>Concepto</th>
                <th style='width: 150px'>Tabla</th>
                <th style='width: 70px'>Nombre Archivo</th>
                <th style='width: 70px'>Nro Registros</th>
                <th style='width: 50px'>Seleccionar <input type="checkbox" id="chkSeleccionar-${prefijo}"> </th>
            </tr>
        </thead>
        <tbody>
    `;

    for (key in lista) {
        var formato = lista[key];

        cadena += `
            <tr>
                <td style='text-align: center;'>${formato.PmFTabOrden}</td>
                <td style='text-align: center;'>${formato.PmFTabNombreTabla}</td>
                <td style='text-align: center;'>${formato.PmFTabDescripcionTabla}</td>
                <td style='text-align: center;'>
                    <a style="cursor:pointer" onclick="redirec('${formato.IndexWeb}','${formato.TipoFormato}');">${formato.PmFTabNombArchivo}</a>
                </td>

                <td style='text-align: center;'>${formato.PmFTabQueryCount}</td>
                <td style='text-align: center;'>
                    <input type="checkbox" value="${formato.PmFTabNombArchivo}" class="chkSeleccion-${prefijo}">
                </td>
            </tr>
        `;
    }
    cadena += "</tbody></table>";

    return cadena;
}

function redirec(web, filtroFormato) {
    var periodo = $("#filtro-mes").val();
    var urlRedireccion = controlador + web + "?Periodo=" + periodo;
    if (filtroFormato != null && filtroFormato != '')
        urlRedireccion += "&tipoFormato=" + filtroFormato;

    location.href = urlRedireccion;
}

function guardarDat(horizonte) {
    var periodo = $("#filtro-mes").val();
    var prefijo = (horizonte == "S") ? "semanal" : "mensual";

    var archivos = [];
    $(".chkSeleccion-" + prefijo + ":checked").each(function () {
        archivos.push($(this).val());
    });

    var tipoReporteMantto = $('input[name="rbMantto"]:checked').val();

    $("#mensaje-dat").hide();

    $.ajax({
        type: 'POST',
        url: controlador + "Procesar",
        dataType: 'json',
        contentType: 'application/json',
        data: JSON.stringify({
            archivos: archivos,
            periodo: periodo,
            horizonte: horizonte,
            tipoReporteMantto: tipoReporteMantto
        }),
        cache: false,
        success: function (model) {
            $("#mensaje-dat").show();

            if (model.Resultado != "-1") {
                mostrarMensaje("mensaje-dat", "Proceso finalizado.", $tipoMensajeExito, $modoMensajeCuadro);

                consultarResultadoDat(horizonte);
            } else {
                mostrarMensaje("mensaje-dat", model.Mensaje, $tipoMensajeAlerta, $modoMensajeCuadro);
            }
        },
        error: function (err) {
            alert("Ha ocurrido un error.")
        }
    });
}

function descargarDat(horizonte) {
    var periodo = $("#filtro-mes").val();
    var prefijo = (horizonte == "S") ? "semanal" : "mensual";

    var archivos = [];
    $(".chkSeleccion-" + prefijo + ":checked").each(function () {
        archivos.push($(this).val());
    });

    var listaMsj = [];
    if (archivos.length == 0) {
        listaMsj.push('No se ha seleccionado ningun archivo.');
    }

    var carpeta = $('#carpeta').val();
    var esDescarga = false;
    if (carpeta == "0" || carpeta == "" || carpeta == null) {
        esDescarga = true;
    } else {
        var rutabase = $('#hddrutaDescarga').val();
        if (carpeta.indexOf(rutabase) == -1) {
            listaMsj.push('La carpeta donde se generarán los archivos .DAT debe iniciar con: ' + rutabase);
        }
    }

    $("#mensaje-dat").hide();

    if (listaMsj.length > 0) {
        $("#mensaje-dat").show();
        mostrarMensaje("mensaje-dat", model.Mensaje, $tipoMensajeAlerta, $modoMensajeCuadro);
    } else {
        $.ajax({
            type: 'POST',
            url: controlador + "GenerarDat",
            dataType: 'json',
            contentType: 'application/json',
            data: JSON.stringify({
                archivos: archivos,
                periodo: periodo,
                carpeta: carpeta,
                horizonte: horizonte
            }),
            cache: false,
            success: function (model) {
                $("#mensaje-dat").show();

                if (model.Resultado != "-1") {
                    mostrarMensaje("mensaje-dat", "Se generaron correctamente los archivos (.DAT)", $tipoMensajeExito, $modoMensajeCuadro);

                    if (esDescarga) {
                        document.location.href = controlador + 'descargar?formato=' + 1 + '&file=' + model.nameFile
                    }

                } else {
                    mostrarMensaje("mensaje-dat", model.Mensaje, $tipoMensajeAlerta, $modoMensajeCuadro);
                }
            },
            error: function (err) {
                alert("Ha ocurrido un error.")
            }
        });
    }
}
