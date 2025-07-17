var controlador = siteRoot + "intercambioOsinergmin/importaciones/";
var table;
var radio;

$(function () {

    var fechaInicial = $('#hdnFechaIni').val();
    var fechaFin = $('#hdnFechaFin').val();

    $('#fechaDia').Zebra_DatePicker({        
        direction: [fechaInicial, fechaFin],
        format: 'd/m/Y'
    });

    $('#btnCancelar').click(function () {
        cancelar();
    });

    $('.rbTipo').click(function () {
        mostrarTipo();
    });

    $('#btnImportarTabla04').click(function () {
        importarTodo("TMP_CLI_TABLA04");
    });

    //- alpha.HDT - 26/04/2017: Cambio para atender el requerimiento. 

    $('#btnExportarTabla04').click(function () {
        exportarTabla4();
    });

    $('#btnExportarTabla05').click(function () {
        exportarTabla5();
    });

    $('#btnEnviarMedidores').click(function () {
        procesarEnvioCoes();
    });
    //- HDT Fin
    

    $('#btnImportarTabla05').click(function () {
        importarTodo("TMP_CLI_TABLA05");
    });

    //- alpha.HDT - 11/07/2017: Cambio para atender el requerimiento. 
    $('#btnGenerarSuministros').click(function () {
        generarSuministros();
    });
    //- HDT Fin

    //- pr16.HDT - Inicio 01/04/2018: Cambio para atender el requerimiento.
    asegurarCoherenciaEtiquetaPerSicli();

    $('#btnAbrirCerrar').click(function () {
        abrirCerrarPeriodo();
    });
    //- HDT Fin

    pintarBusqueda();

    $('#tab-container').easytabs({
        animate: false
    });

    $('#tab-container-secundario').easytabs({
        animate: false
    });

    $('#btnGenerarReporte').click(function () {
        generarReporte();
    });

    $('#btnImportarTabla02').click(function () {
        importarTodo("TMP_CLI_TABLA02");
    });

    crearPupload();
    validarMostrarSincronizacion();
});

//- pr16.HDT - Inicio 01/04/2018: Cambio para atender el requerimiento.
function asegurarCoherenciaEtiquetaPerSicli() {
    if ($('#estadoCerrado').val() == "1") {
        $("#btnAbrirCerrar").val('Abrir Periodo');

        $('#btnImportarTabla04').hide();
        $('#btnImportarTabla05').hide();
        $('#btnGenerarSuministros').hide();

        $('#btnImportarTabla02').hide();
        //$('#btnImportarDatos').hide();
    }
    else {
        $("#btnAbrirCerrar").val('Cerrar Periodo');

        $('#btnImportarTabla04').show();
        $('#btnImportarTabla05').show();
        $('#btnGenerarSuministros').show();

        $('#btnImportarTabla02').show();
        //$('#btnImportarDatos').show();
    }
}

//- pr16.HDT - Inicio 01/04/2018: Cambio para atender el requerimiento.
function abrirCerrarPeriodo() {
    var cerrado = 0;
    var accion = '';

    if ($('#estadoCerrado').val() == "1") {
        accion = 'Abrir';
    }
    else {
        accion = 'Cerrar';
    }
    if (confirm('¿Está seguro de ' + accion + ' el Periodo?')) {
        var pSicliCodi = $('#pSicliCodi').val();
        procesarAperturaCierre(pSicliCodi, accion);
    }
}

function procesarAperturaCierre(pSicliCodi, accion) {
    $.ajax({
        type: "POST",
        url: controlador + "abrirCerrarPeriodo",
        data: {
            pSicliCodi: pSicliCodi,
            accion: accion
        },
        dataType: "json",
        success: function (result) {
            alert(result.mensaje);
            location.reload();
        },
        error: function (xhr) {
            mostrarError("Ocurrio un problema >> readyState: " + xhr.readyState + " | status: " + xhr.status + " | responseText: " + xhr.responseText);
        }
    });
}

function cancelar() {
    window.location.href = controlador + "Index";
}

descargarArchivo = function (url) {
    document.location.href = controlador + "download?url=" + url;
}

var pintarBusqueda =
    function () {
        pintarTabla04();
        pintarTabla05();
        pintarTabla02();
    };

/*
Permite cargar la información de la tabla 04.
*/
function pintarTabla04() {

    $("#listado04").html("");

    if ($("#PeriodoRemisionModel_Periodo").val() === null) return;

    $.ajax({
        type: "POST",
        url: controlador + "listaControlTabla04",
        data: {
            periodo: $("#PeriodoImportacionModel_Periodo").val()
        },
        success: function (evt) {
            $("#listado04").html(evt);

            table = $("#tabla04").DataTable({
                "scrollY": 850,
                "scrollX": false,
                "sDom": "t",
                "ordering": false,
                "bDestroy": true,
                "bPaginate": false,
                "iDisplayLength": 50,
                "autoWidth": true
            });

            var hash = window.location.hash;
            $("#etabs").easytabs("option", "active", 2);

        },
        error: function (xhr) {
            mostrarError("Ocurrio un problema >> readyState: " + xhr.readyState + " | status: " + xhr.status + " | responseText: " + xhr.responseText);
        }
    });
}

/*
Permite cargar la información de la tabla 05.
*/
function pintarTabla05() {

    $("#listado05").html("");

    if ($("#PeriodoRemisionModel_Periodo").val() === null) return;

    $.ajax({
        type: "POST",
        url: controlador + "listaControlTabla05",
        data: {
            periodo: $("#PeriodoImportacionModel_Periodo").val()
        },
        success: function (evt) {
            $("#listado05").html(evt);
            table = $("#tabla05").DataTable({
                "scrollY": 850,
                "scrollX": false,
                "sDom": "t",
                "ordering": false,
                "bDestroy": true,
                "bPaginate": false,
                "iDisplayLength": 50,
                "autoWidth": true
            });

            $("#etabs").easytabs("option", "active", 2);
        },
        error: function (xhr) {
            mostrarError("Ocurrio un problema >> readyState: " + xhr.readyState + " | status: " + xhr.status + " | responseText: " + xhr.responseText);
        }
    });
}

function limpiarTab01() {
    pintarTabla05();

    return false;
}

function limpiarTab02() {
    pintarTabla04();

    return false;
}

// Permite configurar un parámetro en una URL dada.
function configurarParametroEnUrl(url, nombreParametro, valorParametro) {
    var hash = location.hash;
    url = url.replace(hash, '');

    if (url.indexOf("?") >= 0) {

        var parametros = url.substring(url.indexOf("?") + 1).split("&");
        var parametroEncontrado = false;

        parametros.forEach(function (parametro, indice) {
            var p = parametro.split("=");
            if (p[0] == nombreParametro) {
                parametros[indice] = nombreParametro + "=" + valorParametro;
                parametroEncontrado = true;
            }
        });

        if (!parametroEncontrado) {
            parametros.push(nombreParametro + "=" + valorParametro);
        }

        url = url.substring(0, url.indexOf("?") + 1) + parametros.join("&");
    }
    else {
        url += "?" + paramName + "=" + paramValue;
    }
        
    return url;
}

function mostrarError(mensaje) {
    $('#mensaje').removeClass();
    $('#mensaje').html(mensaje);
    $('#mensaje').addClass('action-error');
}

function mostrarExito(mensaje) {
    $('#mensaje').removeClass();
    $('#mensaje').html(mensaje);
    $('#mensaje').addClass('action-exito');
}

//- HDT Inicio 26/04/2017
// Permite exportar el contenido de la tabla 04.
function exportarTabla4() {

    var cadenaEmpresas = "";

    var checkboxes = document.getElementById('tbSeleccionados04').getElementsByTagName('input');

    for (var i = 0; i < checkboxes.length; i++) {
        if (checkboxes[i].type == 'checkbox' && checkboxes[i].checked == true) {

            var valor = checkboxes[i].id;

            if (cadenaEmpresas == "") {
                cadenaEmpresas = "'" + valor + "'";
            }
            else {
                cadenaEmpresas = cadenaEmpresas + ", '" + valor + "'";
            }
        }
    }
    var fecha = $("#fechaDia").val();
    $.ajax({
        type: "POST",
        url: controlador + "exportarDatosTabla",
        data: {
            periodo: $("#PeriodoImportacionModel_Periodo").val(),
            tabla: 'Tabla04',
            empresasIn: cadenaEmpresas,
            fechaDia: fecha
        },
        dataType: "json",
        success: function (result) {
            if (result !== -1) {
                document.location.href = controlador + "descargarTabla04?file=" + result;
            }
            else {
                mostrarMensaje("mensaje", $mensajeErrorGeneral, $tipoMensajeError, $modoMensajeCuadro);
            }
        },
        error: function () {
            mostrarMensaje("mensaje", $mensajeErrorGeneral, $tipoMensajeError, $modoMensajeCuadro);
        }
    });

}

//- HDT Inicio 26/04/2017
// Permite exportar el contenido de la tabla 04.
function exportarTabla5() {

    var cadenaEmpresas = "";

    var checkboxes = document.getElementById('tbSeleccionados05').getElementsByTagName('input');

    for (var i = 0; i < checkboxes.length; i++) {
        if (checkboxes[i].type == 'checkbox' && checkboxes[i].checked == true) {

            var valor = checkboxes[i].id;

            if (cadenaEmpresas == "") {
                cadenaEmpresas = "'" + valor + "'";
            }
            else {
                cadenaEmpresas = cadenaEmpresas + ", '" + valor + "'";
            }
        }
    }

    $.ajax({
        type: "POST",
        url: controlador + "exportarDatosTabla",
        data: {
            periodo: $("#PeriodoImportacionModel_Periodo").val(),
            tabla: 'Tabla05',
            empresasIn: cadenaEmpresas,
            fechaDia:''
        },
        dataType: "json",
        success: function (result) {
            if (result !== -1) {
                document.location.href = controlador + "descargarTabla05?file=" + result;
            }
            else {
                mostrarMensaje("mensaje", $mensajeErrorGeneral, $tipoMensajeError, $modoMensajeCuadro);
            }
        },
        error: function () {
            mostrarMensaje("mensaje", $mensajeErrorGeneral, $tipoMensajeError, $modoMensajeCuadro);
        }
    });
}

function importarTodo(tab) {

    //- HDT Inicio 02/04/2017
    var cadenaEmpresas = "";
    var nombreTab = "";
    var checkboxes = null;
    if (tab == "TMP_CLI_TABLA04") {
        checkboxes = document.getElementById('tbSeleccionados04').getElementsByTagName('input');
    }
    else if (tab == "TMP_CLI_TABLA05") {
        checkboxes = document.getElementById('tbSeleccionados05').getElementsByTagName('input');
    }
    else if (tab == "TMP_CLI_TABLA02") {
        checkboxes = document.getElementById('tbSeleccionados02').getElementsByTagName('input');
    }
    else {
        alert("No se puede importar porque la pestaña elegida no fue aun implementada. Contáctese con el administrador del sistema");
        return;
    }

    for (var i = 0; i < checkboxes.length; i++) {
        if (checkboxes[i].type == 'checkbox' && checkboxes[i].checked == true) {

            var valor = checkboxes[i].id;

            if (cadenaEmpresas == "") {
                cadenaEmpresas = valor;
            }
            else {
                cadenaEmpresas = cadenaEmpresas + "," + valor;
            }
        }
    }

    if (cadenaEmpresas == null || cadenaEmpresas == "") {
        alert("No ha seleccionado Entidad alguna para procesar");
        return;
    }

    $.ajax({
        type: 'POST',
        url: controlador + 'importarRegistros',
        data: {
            periodo: $("#PeriodoImportacionModel_Periodo").val(),
            tabla: tab
            //- HDT Inicio 02/04/2016
            , empresasIn: cadenaEmpresas
            //- HDT Fin
        },
        dataType: 'json',
        success: function (result) {
            
            if (result.resultado == 1) {
                mostrarExito(result.mensaje);

                if (tab == "TMP_CLI_TABLA04") {
                    pintarTabla04();
                    validarMostrarSincronizacion();
                }
                else if (tab == "TMP_CLI_TABLA05") {
                    pintarTabla05();
                }
                 else if (tab == "TMP_CLI_TABLA02") {
                    pintarTabla02();
                }

                return false;
            } else {
                //- HDT Inicio 26/04/2016
                if (result.lstMensajes.length > 0) {
                    mostrarErrores(result.lstMensajes);
                }
                mostrarError(result.mensaje);
                //- HDT Fin
            }
            
        },
        error: function () {
            //- HDT.Alpha Inicio 19/10/2017. Permite orientar al usuario sobre qué hacer de presentarse la excepción no controlada.
            //mostrarError('Ha ocurrido un error: verificar el query');
            mostrarError('Ha ocurrido un problema en la importación, por favor vuelva a intentar y si el problema persiste contacte al Administrador');
            //- HDT Fin
        }
    });
}

function mostrarErrores(lista) {

    $('#idTerrores').html(dibujarTablaError(lista));
    setTimeout(function () {
        $('#validaciones').bPopup({
            easing: 'easeOutBack',
            speed: 450,
            transition: 'slideDown',
            modalClose: false,
            //- HDT Inicio 24/08/2017
            onClose: function () {
                //- Al cerrar la ventana se debe regrescar la ventana.
                pintarTabla05();
                pintarTabla04();
            }
            //- HDT Fin
        });
        $('#tablaError').dataTable({
            "scrollY": 330,
            "scrollX": true,
            "sDom": 't',
            "ordering": false,
            "bPaginate": false,
            "iDisplayLength": -1
        });

    }, 50);
}

//- HDT Inicio 02/04/2016
// Permite mostrar las incidencias de un registro de importación dado.
function mostrarIncidencias(rcImCodi) {
    $.ajax({
        type: "POST",
        url: controlador + "listarIncidenciasImportacion",
        data: {
            rcImCodi: rcImCodi
        },
        success: function (evt) {

            //- Se procesó con éxito
            $('#idTerrores').html(dibujarTablaError(evt));
            setTimeout(function () {
                $('#validaciones').bPopup({
                    easing: 'easeOutBack',
                    speed: 450,
                    transition: 'slideDown',
                    modalClose: false
                });
                $('#tablaError').dataTable({
                    "scrollY": 330,
                    "scrollX": true,
                    "sDom": 't',
                    "ordering": false,
                    "bPaginate": false,
                    "iDisplayLength": -1
                });

            }, 50);

        },
        error: function (xhr) {
            mostrarError("Ocurrio un problema >> readyState: " + xhr.readyState + " | status: " + xhr.status + " | responseText: " + xhr.responseText);
        }
    });
}

function dibujarTablaError(lista) {
    var cadena = "<div style='clear:both; height:5px'></div> ";
    cadena += "<table id='tablaError' border='1' class='pretty tabla-adicional' cellspacing='0'>";
    cadena += "<thead><tr><th>Detalle</tr></thead>";
    cadena += "<tbody>";

    for (key in lista) {
        cadena += "<tr><td>" + lista[key] + "</td></tr>";
    }

    cadena += "</tbody></table>";
    return cadena;
}

//- HDT Inicio 02/04/2017
function checkAll(ele) {
    var checkboxes = document.getElementsByTagName('input');
    if (ele.checked) {
        for (var i = 0; i < checkboxes.length; i++) {
            if (checkboxes[i].type == 'checkbox') {
                checkboxes[i].checked = true;
            }
        }
    } else {
        for (var i = 0; i < checkboxes.length; i++) {
            if (checkboxes[i].type == 'checkbox') {
                checkboxes[i].checked = false;
            }
        }
    }
}

//function dibujarTablaError(lista) {
//    var cadena = "<div style='clear:both; height:5px'></div> ";
//    cadena += "<table id='tablaError' border='1' class='pretty tabla-adicional' cellspacing='0'>";
//    cadena += "<thead><tr><th>Detalle</tr></thead>";
//    cadena += "<tbody>";

//    for (key in lista) {
//        cadena += "<tr><td>" + lista[key] + "</td></tr>";
//    }

//    cadena += "</tbody></table>";
//    return cadena;
//}

//- HDT Inicio 11/07/2017
//- Permite mostrar la tabla de incidencias respecto a suministros que se pueden generar.
function generarSuministros(rcImCodi) {

    var checkboxes = document.getElementById('tbSeleccionados05').getElementsByTagName('input');

    var cadenaEmpresas = '';

    for (var i = 0; i < checkboxes.length; i++) {
        if (checkboxes[i].type == 'checkbox' && checkboxes[i].checked == true) {

            var valor = checkboxes[i].value;

            if (cadenaEmpresas == "") {
                cadenaEmpresas = "'" + valor + "'";
            }
            else {
                cadenaEmpresas = cadenaEmpresas + ", " + "'" + valor + "'";
            }
        }
    }

    if (cadenaEmpresas == null || cadenaEmpresas == "") {
        alert("No ha seleccionado Entidad alguna para procesar");
        return;
    }

    $.ajax({
        type: "POST",
        url: controlador + "listarIncidenciasImportacionSuministros",
        data: {
            periodo: $("#PeriodoImportacionModel_Periodo").val(),
            empresasIn: cadenaEmpresas
        },
        success: function (evt) {

            //- Se procesó con éxito
            $('#idErroresSuministro').html(dibujarTablaIncidencias(evt));
            setTimeout(function () {
                $('#suministrosGenerar').bPopup({
                    easing: 'easeOutBack',
                    speed: 450,
                    transition: 'slideDown',
                    modalClose: false
                });
                /*
                $('#idTerroresSuministro').dataTable({
                    "scrollY": 330,
                    "scrollX": true,
                    "sDom": 't',
                    "ordering": false,
                    "bPaginate": false,
                    "iDisplayLength": -1
                });
                */

            }, 50);

        },
        error: function (xhr) {
            mostrarError("Ocurrio un problema >> readyState: " + xhr.readyState + " | status: " + xhr.status + " | responseText: " + xhr.responseText);
        }
    });
}

//- HDT Inicio 11/10/2017
//- Permite dibujar la tabla de incidencias a fin de permitir
//- la corrección de errores en caso aplique.
function dibujarTablaIncidencias(lista) {
    var cadena = "<div style='clear:both; height:5px'></div> ";
    cadena += "<table width='100%'><tr><td>";
    cadena += "<div align='right'><input type='button' id='btnCorregir' name='btnCorregir' value='Generar' onclick='procesarSuministros()' /></div>";
    cadena += "</td></tr></table>";

    cadena += "<div style='clear:both; height:5px'></div> ";

    cadena += "<table id='idTerroresSuministro' border='1' class='pretty tabla-adicional incidentes' cellspacing='0'>";
    cadena += "<thead><tr><th>Sumnistro</th><th>Cliente</th><th>Suministrador</th><th>Barra</th><th>Área</th></tr></thead>";
    cadena += "<tbody>";

    var mostrarAvisoNoRuc = false;
    var textoBarra = "";

    for (var i = 0; i < lista.ListaIioLogImportacionDTO.length; i++) {

        if (lista.ListaIioLogImportacionDTO[i].Cliente.indexOf("(*)") != -1) {
            textoBarra = "";
            mostrarAvisoNoRuc = true;
        }
        else {
            textoBarra = lista.ListaIioLogImportacionDTO[i].Barra + " " + lista.ListaIioLogImportacionDTO[i].Tension + " kV";
        }

        cadena += "<tr>" +
                  "<td>" +
                        "<input class='ulogCodiIncidente' type='hidden' value='" + lista.ListaIioLogImportacionDTO[i].UlogCodi + "' />" +
                        "<input class='empresaIncidente' type='hidden' value='" + lista.ListaIioLogImportacionDTO[i].EmprCodi + "' />" +
                        "<input class='suministroIncidente' type='hidden' value='" + lista.ListaIioLogImportacionDTO[i].Suministro + "' />" +
                        "<input class='barraIncidente' type='hidden' value='" + lista.ListaIioLogImportacionDTO[i].Barra + "' />" +
                        "<input class='tensionIncidente' type='hidden' value='" + lista.ListaIioLogImportacionDTO[i].Tension + "' />" +
                        "<input class='ulogTablaAfectadaIncidente' type='hidden' value='" + lista.ListaIioLogImportacionDTO[i].UlogTablaAfectada + "' />" +
                        "<input class='psicliCodiIncidente' type='hidden' value='" + lista.ListaIioLogImportacionDTO[i].PsicliCodi + "' />" +
                        "<input class='empresaSuministrador' type='hidden' value='" + lista.ListaIioLogImportacionDTO[i].EmprCodiSumi + "' />" +
                        lista.ListaIioLogImportacionDTO[i].Suministro +
                  "</td>" +
                  "<td>" +
                        lista.ListaIioLogImportacionDTO[i].Cliente +
                    "</td>" +
                  "<td>" +
                        lista.ListaIioLogImportacionDTO[i].Suministrador +
                  "</td>" +
                  "<td>" +
                        textoBarra +
                  "</td>" +
                  "<td style='height:22px;'>";

        if (lista.ListaIioLogImportacionDTO[i].Cliente.indexOf("(*)") == -1 && lista.ListaIioLogImportacionDTO[i].Suministrador.indexOf("(*)") == -1) {
            cadena += "<select style='width:100%;' class='areaIncidente'>";
            for (var j = 0; j < lista.ListaEqAreaDTO.length; j++) {
                if (j == 0) {
                    cadena += "<option value=''>--Seleccione--</option>";
                }
                else {
                    //- HDT.Alpha Inicio 19/10/2017. Cambio para distinguir las áreas de la lista de selección.
                    //cadena += "<option value='" + lista.ListaEqAreaDTO[j].Areacodi + "'>" + lista.ListaEqAreaDTO[j].Areanomb + "</option>";
                    cadena += "<option value='" + lista.ListaEqAreaDTO[j].Areacodi + "'>" + lista.ListaEqAreaDTO[j].Areanomb + ' - ' + lista.ListaEqAreaDTO[j].Areaabrev + "</option>";
                    //- HDT Fin
                }
            }
            cadena += "</select>";
        }

        cadena += "</td>" +
                  "</tr>";
    }
    
    cadena += "</tbody></table>";

    if (mostrarAvisoNoRuc) {
        cadena += "<br />";
        cadena += "<p>(*) No existe el registro de Cliente Libre como empresa en la base de datos del COES, ";
        cadena += "debe registrarlo para crear el Suministro de Cliente Libre.</p>";
    }

    return cadena;
}

//- HDT Inicio 11/10/2017
//- Permite procesar los incidentes para los que se procesarán los suministros.
//- Se debe precisar que para procesar los suministros se requiere que al menos
//- exista un suministro con área seleccionada.
function procesarSuministros() {

    var incidentes = [];
    
    $('#idTerroresSuministro tbody tr').each(function (index, value) {


        $(this).find('.ulogCodiIncidente').each(function () {

            var areaIncidente = $(this).closest('tr').find('.areaIncidente').val();

            if (areaIncidente != null && areaIncidente != '') {
                var ulogCodiIncidente = $(this).val();
                var empresaIncidente = $(this).closest('tr').find('.empresaIncidente').val();
                var suministroIncidente = $(this).closest('tr').find('.suministroIncidente').val();
                var barraIncidente = $(this).closest('tr').find('.barraIncidente').val();
                var tensionIncidente = $(this).closest('tr').find('.tensionIncidente').val();
                var ulogTablaAfectadaIncidente = $(this).closest('tr').find('.ulogTablaAfectadaIncidente').val();
                var psicliCodiIncidente = $(this).closest('tr').find('.psicliCodiIncidente').val();
                var empresaSuministrador = $(this).closest('tr').find('.empresaSuministrador').val();

                var incidente = {
                                    UlogCodi: parseInt(ulogCodiIncidente),
                                    EmprCodi: parseInt(empresaIncidente),                                    
                                    Suministro: suministroIncidente,
                                    Barra: barraIncidente,
                                    CodigoArea: parseInt(areaIncidente),
                                    Tension: tensionIncidente,
                                    UlogTablaAfectada: ulogTablaAfectadaIncidente,
                                    PsicliCodi: psicliCodiIncidente,
                                    EmprCodiSumi: parseInt(empresaSuministrador)    
                                };

                incidentes.push(incidente);
            }

        });

    });

    if (incidentes.length == 0) {
        alert('Por favor seleccione el Área de al menos un Suministro');
        return;
    }

    //- Lanzando el procesamiento de la generación de suministros.
    $.ajax({
        type: 'POST',
        url: controlador + 'generarSuministrosDesdeIncidentes',
        data: JSON.stringify(incidentes),
        dataType: 'json',
        contentType: 'application/json; charset=utf-8',
        success: function (result) {

            if (result.resultado == 1) {
                mostrarExito(result.mensaje);
                $('#suministrosGenerar').bPopup().close();
                pintarTabla05();

                return false;
            } else {
                mostrarError(result.mensaje);
            }
        },
        error: function () {
            mostrarError('Ha ocurrido un error, contáctese con el Administrador del Sistema');
        }
    });
}

//Importacion Informacion Base 08/07/2019
function mostrarAlerta(mensaje) {
    $('#mensaje').removeClass();
    $('#mensaje').html(mensaje);
    $('#mensaje').addClass('action-alert');
}
function crearPupload() {
    uploader = new plupload.Uploader({
        runtimes: 'html5,flash,silverlight,html4',
        browse_button: 'btnImportarDatos',
        url: siteRoot + 'intercambioOsinergmin/importaciones/Subir',
        flash_swf_url: 'Scripts/Moxie.swf',
        silverlight_xap_url: 'Scripts/Moxie.xap',
        multi_selection: false,
        filters: {
            max_file_size: '5mb',
            mime_types: [
                { title: "Archivos Excel .xlsx", extensions: "xlsx,xls" }
            ]
        },
        init: {
            FilesAdded: function (up, files) {
                if (uploader.files.length == 2) {
                    uploader.removeFile(uploader.files[0]);
                }
                uploader.start();
                up.refresh();
            },
            UploadProgress: function (up, file) {
                mostrarAlerta("Por favor espere ...(<strong>" + file.percent + "%</strong>)");
            },
            UploadComplete: function (up, file) {
                mostrarAlerta("Subida completada <strong>Por favor espere</strong>");
                leerExcelSubido();
                //limpiarError();
            },
            Error: function (up, err) {
                mostrarError(err.code + "-" + err.message);
            }
        }
    });
    uploader.init();
}

function leerExcelSubido() {
    var periodo = document.getElementById('periodo').value;

    var puedeImportar = true;
    //var cantidadRegistros = document.getElementById('hfCantidadRegistros').value;

    
    if (puedeImportar) {
        $.ajax({
            type: 'POST',
            url: controlador + 'LeerExcelSubido',
            dataType: 'json',
            async: false,
            data: {
                periodo: periodo
            },
            success: function (respuesta) {
                if (respuesta.Exito == false) {
                    mostrarError(respuesta.Mensaje);
                } else {                    
                    mostrarExito("Archivo importado");
                }
            },
            error: function () {
                mostrarError("Ha ocurrido un error");
            }
        });
    } else {
        mostrarAlerta("Operación cancelada");
    }

}

function generarReporte() {

    var periodo = document.getElementById('periodo').value;
    var tipoReporte = document.getElementById('tipoReporte').value;

    $.ajax({
        type: 'POST',
        dataType: 'json',
        url: controlador + "GenerarReporte",
        data: {
            periodo: periodo,
            tipoReporte: tipoReporte
        },
        success: function (result) {

            if (result != "-1") {
                window.location.href = controlador + 'DescargarFormato?file=' + result;
                //mostrarExito("Se ha eliminado el registro correctamente.");
                //pintarBusqueda();
            }
            else {
                alert("Error al generar el archivo.");
            }

        },
        error: function () {
            mostrarError('Opción Reporte: Ha ocurrido un error');
        }
    });
};

function generarComparativoEmpresa() {

    var periodo = document.getElementById('periodo').value;

    $.ajax({
        type: 'POST',
        dataType: 'json',
        url: controlador + "GenerarReporteComparativoEmpresa",
        data: {
            periodo: periodo
        },
        success: function (result) {

            if (result != "-1") {
                window.location.href = controlador + 'DescargarFormato?file=' + result;
                //mostrarExito("Se ha eliminado el registro correctamente.");
                //pintarBusqueda();
            }
            else {
                alert("Error al generar el archivo.");
            }

        },
        error: function () {
            mostrarError('Opción Reporte: Ha ocurrido un error');
        }
    });
};

function generarComparativoHistorico() {

    var periodo = document.getElementById('periodo').value;

    $.ajax({
        type: 'POST',
        dataType: 'json',
        url: controlador + "GenerarReporteComparativoHistorico",
        data: {
            periodo: periodo
        },
        success: function (result) {

            if (result != "-1") {
                window.location.href = controlador + 'DescargarFormato?file=' + result;
                //mostrarExito("Se ha eliminado el registro correctamente.");
                //pintarBusqueda();
            }
            else {
                alert("Error al generar el archivo.");
            }

        },
        error: function () {
            mostrarError('Opción Reporte: Ha ocurrido un error');
        }
    });
};

function pintarTabla02() {

    $("#listado02").html("");

    if ($("#PeriodoRemisionModel_Periodo").val() === null) return;

    $.ajax({
        type: "POST",
        url: controlador + "listaControlTabla02",
        data: {
            periodo: $("#PeriodoImportacionModel_Periodo").val()
        },
        success: function (evt) {
            $("#listado02").html(evt);
            table = $("#tabla02").DataTable({
                "scrollY": 850,
                "scrollX": false,
                "sDom": "t",
                "ordering": false,
                "bDestroy": true,
                "bPaginate": false,
                "iDisplayLength": 50,
                "autoWidth": true
            });

            $("#etabs").easytabs("option", "active", 2);
        },
        error: function (xhr) {
            mostrarError("Ocurrio un problema >> readyState: " + xhr.readyState + " | status: " + xhr.status + " | responseText: " + xhr.responseText);
        }
    });
}

function limpiarTab03() {
    pintarTabla02();

    return false;
}

function validarMostrarSincronizacion() {    

    var periodo = document.getElementById('periodo').value;

    $.ajax({
        type: 'POST',
        dataType: 'json',
        url: controlador + "ObtenerEmpresasPendientes",
        data: {
            periodo: periodo
        },
        success: function (result) {

            var res = result.split('@');
            if (res[0] > 0) {
                document.getElementById('tdEnviarMedidores').style.visibility = 'hidden';
                document.getElementById('tdTextoFechaSincronizacion').style.visibility = 'hidden';
                document.getElementById('tdFechaSincronizacion').style.visibility = 'hidden';               
                
            } else {
                document.getElementById('tdEnviarMedidores').style.visibility = '';
                document.getElementById('tdTextoFechaSincronizacion').style.visibility = '';
                document.getElementById('tdFechaSincronizacion').style.visibility = '';

                document.getElementById('fechaSincronizacion').value = res[1];               
            }

        },
        error: function () {
            mostrarError('Ha ocurrido un error');
        }
    });
};

function procesarEnvioCoes() {

    var res = confirm('¿Está seguro de realizar el proceso?');

    if (!res) {
        return;
    }

    var periodoRegistro = document.getElementById('periodo').value;

    //if ($("#Anio").val() === null) return;
    $.ajax({
        type: "POST",
        url: controlador + "ProcesarEnvioCoes",
        data: {
            periodo: periodoRegistro
        },
        success: function (evt) {

            if (evt == 'OK') {
                alert('Se realizó el envio de datos a Medidores satisfactoriamente.');
                //busqueda();
            } else {
                //alert('pp');
                mostrarError(evt);
            }


        },
        error: function () {
            mostrarMensaje("mensaje", $mensajeErrorGeneral, $tipoMensajeError, $modoMensajeCuadro);
        }
    });
}
