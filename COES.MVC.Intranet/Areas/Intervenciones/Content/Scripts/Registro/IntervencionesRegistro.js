var controler = siteRoot + "Intervenciones/Registro/";
var controlador = siteRoot + "Intervenciones/Registro/";
var controladorParametro = siteRoot + "Intervenciones/Parametro/";
var ANCHO_LISTADO = 900;
var TIPO_REPORTE_TABLA_GEN = 1;
var TIPO_REPORTE_TABLA_TRANS = 2;
var INTERVENCION_EJECUTADA = null;
var ocultar = 0;
var LISTAGENERAL = [];
var arrayFilasEditadas = [];
var ESTADO_APROBADO = 3;

var EVENCLASECODI_ANUAL = 5;
var EVENCLASECODI_SEMANAL = 3;
var EVENCLASECODI_DIARIO = 2;
var EVENCLASECODI_EJECUTADO = 1;

var FECHAINIPROG;
var FECHAFINPROG;

var ACTIVARVALIDACIONES = "1";
var IMG_VER = `<img src="${siteRoot}Content/Images/btn-open.png" title="Ver informacion del registro"/>`;
var IMG_EDITAR = `<img src="${siteRoot}Content/Images/btn-edit.png" title="Editar informacion del registro"/>`;
var IMG_COMUNICACION_NINGUNA = `<img src="${siteRoot}Content/Images/message_ninguno.png" title="No tiene mensajes" />`;
var IMG_COMUNICACION_EXISTE_MENSAJE = `<img src="${siteRoot}Content/Images/message_existe_mensaje.png" title="Existe(n) mensaje(s)" />`;
var IMG_COMUNICACION_TODO_LEIDO = `<img src="${siteRoot}Content/Images/message_todo_leido.png" title="Todos los mensajes están leídos" />`;
var IMG_COMUNICACION_PENDIENTE_LEER = `<img src="${siteRoot}Content/Images/message_pendiente_leer.png" title="Existe(n) mensaje(s) sin leer" />`;
var IMG_MODIFICACION = `<img src="${siteRoot}Content/Images/historial.png" title="Ver informacion de Modificaciones" />`;
var IMG_TRAZABILIDAD = `<img src="${siteRoot}Content/Images/eslabon.png" title="Ver Trazabilidad" />`;
var IMG_FILE = `<img src="${siteRoot}Content/Images/adjuntos.png" title="Tiene archivos adjuntos"/>`;
var IMG_NOTA = `<img src="${siteRoot}Content/Images/prn-ico-info.png" title="Tiene nota"/>`;
var IMG_SUSTENTO = `<img src="${siteRoot}Content/Images/pdf.png" title="Descargar sustento en formato .pdf"/>`;
var IMG_HISTORIA = `<img src="${siteRoot}Content/Images/envios.png" title="Ver Historia" />`;

var LISTA_PLANTILLA_FORMULARIO_EXCLUSION = [];
var LISTA_PLANTILLA_FORMULARIO_INCLUSION = [];

$(document).ready(function ($) {
    $.fn.dataTable.moment('DD/MM/YYYY HH:mm');
    $("#btnOcultarMenu").click();

    ANCHO_LISTADO = $("#mainLayout").width() > 900 ? $("#mainLayout").width() - 5 : 900;

    //titulo
    var estadoProgDesc = $("#estadoProgDesc").val();
    var estadoProg = parseInt($("#estadoProg").val()) || 0;
    var color = '';
    if (estadoProg == ESTADO_APROBADO) color = '#3dcb41';
    var htmlTitulo = $("#tituloIndex").html();
    var backColor = `<span style="color:${color} ;font-weight: bold;">[${estadoProgDesc}]</span>`;

    $("#tituloIndex").html(htmlTitulo + backColor);

    $('.txtFecha').inputmask({
        mask: "1/2/y",
        placeholder: "dd/mm/yyyy",
        alias: "datetime"
    });

    $('.txtFecha').Zebra_DatePicker({
        readonly_element: false
    });

    FECHAINIPROG = $("#InterfechainiD").val();
    FECHAFINPROG = $("#InterfechafinD").val();
    //var fechaIniProg = $("#InterfechainiD").val();
    //var fechaFinProg = $("#InterfechafinD").val();
    //$('#InterfechainiD').Zebra_DatePicker({
    //    direction: [fechaIniProg, fechaFinProg],
    //});
    //$('#InterfechafinD').Zebra_DatePicker({
    //    direction: [fechaIniProg, fechaFinProg],
    //});

    $('#cboTipoIntervencion, #InterDispo, #estadocodi').multipleSelect({
        selectAll: true,
        filter: true,
        placeholder: "SELECCIONE",
    });

    $('#cboFamilia, #cboUbicacion').multipleSelect({
        selectAll: true,
        filter: true,
        placeholder: "SELECCIONE",
    });

    $('#Empresa').multipleSelect({
        filter: true,
        placeholder: "SELECCIONE",
    });

    $('#cboEquipo').multipleSelect({
        filter: true,
        placeholder: "SELECCIONE",
    });

    //$('#cboConjuntoEq').multipleSelect({
    //    filter: false,
    //    placeholder: "SELECCIONE",
    //});

    $('#cboTipoIntervencion, #InterDispo, #estadocodi').multipleSelect('checkAll');
    $('#cboFamilia, #cboUbicacion ').multipleSelect('checkAll');
    $('#Empresa').multipleSelect('checkAll');
    $('#cboEquipo').multipleSelect('checkAll');
    //$('#cboConjuntoEq').multipleSelect('checkAll');

    //-------------------------------------------------------

    //Eventos de las listas desplegables
    $('#cboFamilia').on("change", function () {
        listarEquipoFiltro();
    });

    $('#cboUbicacion').on("change", function () {
        listarEquipoFiltro();
    });

    $('#TipoProgramacion').on("change", function () {
        programaciones();
    });

    $('#Programacion').on("change", function () {
        obtenerFechaOperacion();
    });

    //-------------------------------------------------------

    //Botones del menu
    $('#btnConsultar').click(function () {
        mostrarLista();
    });

    $(".check_eliminado").prop('checked', true);

    $(".check_eliminado").on("click", function () {
        mostrarLista();
    });

    $(".check_mostrarAdjuntos").prop('checked', false);

    $(".check_mostrarAdjuntos").on("click", function () {
        mostrarLista();
    });

    $(".check_mostrarMensajes").prop('checked', false);

    $(".check_mostrarMensajes").on("click", function () {
        mostrarLista();
    });

    $(".check_mostrarNotas").prop('checked', false);

    $(".check_mostrarNotas").on("click", function () {
        mostrarLista();
    });

    $('#ReporteIntervenciones').click(function () {
        reporteIntervencionesXls();
    });
    $('#ReporteTablaGen').click(function () {
        reporteTablaIntervenciones(TIPO_REPORTE_TABLA_GEN);
    });
    $('#ReporteTablaTrans').click(function () {
        reporteTablaIntervenciones(TIPO_REPORTE_TABLA_TRANS);
    });

    $('#btnManttoConsulta').click(function () {
        generarManttoConsultaRegistro();
    });

    $('#IntervencionesNuevo').click(function () {
        abrirPopupNuevo();
    });

    $('#IntervencionesEliminar').click(function () {
        eliminarMasivo();
    });

    $('#IntervencionesRecuperar').click(function () {
        recuperarIntervenciones();
    });

    $('#IntervencionesCopiar').click(function () {
        copiarIntervenciones();
    });

    $('#IntervencionesImportar').click(function () {
        var progrCodi = parseInt(document.getElementById('Progrcodi').value) || 0;
        window.location.href = controler + 'IntervencionesImportacion?progrCodi=' + progrCodi;
    });

    $('#IntervencionesEnviarMensaje').click(function () {
        mensaje();
    });

    $('#CambiarEstado').click(function () {
        cambiarestado();
    });

    //popup comunicaciones
    $('#btnEnviarRptaComunicacion').click(function () {
        enviarRespuestaComunicacion();
    });
    $('#btnDescargaPdfComunicacion').click(function () {
        descargarPdfComunicacion();
    });
    $('#btnDescargaZipComunicacion').click(function () {
        descargarZipComunicacion();
    });
    $('#IntervencionesRptComunicaciones').click(function () {
        generarIntervencionesRptComunicaciones();
    });

    $('#btnExportarExcel').click(function () {
        generarExcelXls();
    });

    $('#btnManttoPlantilla').click(function () {
        descargarManttoPlantilla();
    });

    $('#btnPotenciaIndisp').click(function () {
        reporteIntervencionesPotIndispXls();
    });

    $('#btnReporteActividades').click(function () {
        reporteActividades();
    });

    $('#btnEjecutarSustentoInclExcl').click(function () {
        ejecutarRecordatorioInclusionExclusion();
    });
    $('#btnEjecutarAprobacion').click(function () {
        ejecutarAprobacionAutomatica();
    });

    $('#btnCrearVersion').click(function () {
        crearVersionF1F2();
    });
    //-------------------------------------------------------

    $("#chkSeleccionar").prop('checked', false);

    $("#chkSeleccionar").on("click", function () {
        var check = $('#chkSeleccionar').is(":checked");
        $(".ChkSeleccion").prop("checked", check);
    });

    $('#btnRegresar2').click(function () {
        var idTipoprog = $("#idTipoProgramacion").val();
        window.location.href = controler + 'Programaciones?tipoProgramacion=' + idTipoprog;
    });

    $('#btnContraer').click(
        function (e) {
            $('#Contenido').toggle();
            $(this).css("display", "none");
            $('#btnDescontraer').css("display", "block");
            //asignar tamaño de handson
            ocultar += 1;

            $("#listado").hide();
            var nuevoTamanioTabla = getHeightTablaListado();
            $(".dataTables_scrollBody").css('height', nuevoTamanioTabla + "px");
            $("#listado").show();
        }
    );

    $('#btnDescontraer').click(
        function (e) {
            $('#Contenido').slideDown();
            $(this).css("display", "none");
            $('#btnContraer').css("display", "block");
            ocultar += 1;

            $("#listado").hide();
            var nuevoTamanioTabla = getHeightTablaListado();
            $(".dataTables_scrollBody").css('height', nuevoTamanioTabla + 'px');
            $("#listado").show();
        }
    );

    $('#IntervencionesAgrupar').click(function () {
        agruparIntervenciones();
    });

    $('#btnCancelarDesagrupar').click(function () {
        $('#popupDesagrupar').bPopup().close();
    });

    $('#IntervencionesDesagrupar').click(function () {
        var tipo = listarIntercodiChecked();

        if (tipo != null && tipo != "") {
            setTimeout(function () {
                $("#popupDesagrupar").bPopup({
                    easing: 'easeOutBack',
                    speed: 450,
                    transition: 'slideDown',
                    modalClose: false
                });
            }, 50);
        }
        else {
            alert("Seleccione registro por Desagregar.");
        }
    });

    $('#btnDesagrupar').click(function () {
        var opcion = $("#tipoDesagrupar").val();
        var strHoraIni = $("#desagregarHoraIni").val() || "";
        var strHoraFin = $("#desagregarHoraFin").val() || "";

        if (opcion > 0) {
            desagruparIntervenciones(opcion, strHoraIni, strHoraFin);
        }
        else {
            alert("Debe seleccionar un tipo.");
        }
    });

    $('#tipoDesagrupar').change(function () {
        var opcion = $('#tipoDesagrupar').val();
        var anio = "";
        if (opcion == 2) { //selecciona por horas
            $("#selecionHoras").css("display", "block");
        } else {
            $("#selecionHoras").css("display", "none");
        }
    });

    $('#ConsultaAgentes').click(function () {
        listarIntervencionesAgente();
    });

    $('#DescargarArchivos').click(function () {
        descargarArchivosAdjuntos();
    });

    $('#DescargarMsjMasivos').click(function () {
        descargarMsjMasivos();
    });

    $('#btnGuardarEdicionFechas').click(function () {
        var idPrograma = $("#Progrcodi").val();
        guardarIntervencionEjecutadas(idPrograma);
    });

    $('#btnHabilitarReversion').click(function () {
        var idPrograma = $("#Progrcodi").val();
        var idTipoprog = $("#idTipoProgramacion").val();
        habilitarReversion(idPrograma, idTipoprog);
    });

    if ($("#EsRevertido").val() == "value") {
        ActualizarTiempoRestante($("#fecPlazo").val(), 'contador', 'Finalizó');
    }

    $('#btnActivarValidaciones').click(function () {
        ACTIVARVALIDACIONES = "1";
        mostrarLista();
    });

    listarEquipoFiltro();

    var tipoProghtml = parseInt($("#idTipoProgramacion").val()) || 0;
    if (tipoProghtml != EVENCLASECODI_ANUAL) {
        if (tipoProghtml == "1")
            ACTIVARVALIDACIONES = "0";
        mostrarLista();
    }
    //-------------------------------------------------------

});

//Exportar reporte actividades nuevas y canceladas
function reporteActividades() {
    var codprograma = parseInt($('#Progrcodi').val()) || 0;

    $.ajax({
        type: 'POST',
        url: controler + 'ExportarReporteActividades',
        data: { progcodi: codprograma },
        success: function (evt) {
            if (evt.Resultado != "-1") {

                window.location = controler + "AbrirArchivo?file=" + evt.NombreArchivo;
            }
            else {
                alert('Ha ocurrido un error');
            }
        },
        error: function (err) {
            alert('Ha ocurrido un error');
        }
    });
}

//Exportar Intervenciones realizadas por el agente
function ReporteIntervencionesAgente() {

    var objData = getObjetoFiltro();
    var msj = validarConsulta(objData);

    if (msj == "") {
        $.ajax({
            type: 'POST',
            url: controler + 'ExportarIntervencionesAgente',
            data: objData,
            dataType: 'json',
            success: function (evt) {
                if (evt.Resultado != "-1") {

                    window.location = controler + "AbrirArchivo?file=" + evt.NombreArchivo;
                }
                else {
                    alert('Ha ocurrido un error');
                }
            },
            error: function (err) {
                alert('Ha ocurrido un error');
            }
        });
    } else {
        alert(msj);
    }
}

//Listar Intervenciones realizadas por el agente
function listarIntervencionesAgente() {
    //$("#listadoIntervencionesAgente").html('');
    //var $containerWidth = $(window).width() - 100;
    //$("#popupIntervencionesAgente").css({ "maxWidth": $containerWidth + "px", "width": ($containerWidth - 50) + "px" });

    var objData = getObjetoFiltro();
    var msj = validarConsulta(objData);

    var altoPantalla = $(window).height();

    $.ajax({
        type: 'POST',
        url: controler + 'ListarIntervencionesAgente',
        contentType: "application/json; charset=utf-8",
        data: JSON.stringify(objData),
        success: function (result) {
            $('#listadoIntervencionesAgente').html(result);

            $('#popupIntervencionesAgente').bPopup({
                modalClose: true,
                easing: 'easeOutBack',
                speed: 450,
                transition: 'slideDown'
            });

            setTimeout(function () {
                $('#tablaListadoIntervAgente').dataTable({
                    scrollY: 450,
                    scrollX: true,
                    "sDom": 't',
                    scrollCollapse: false,
                    "destroy": "true",
                    "bAutoWidth": false,
                    paging: false,
                    "ordering": false,
                });

                //centrado de popup
                var topPopup = parseInt((altoPantalla - 600) / 2);
                if (topPopup < 50) topPopup = 150;

                $(document).ready(function ($) {
                    $('#popupIntervencionesAgente').css("top", topPopup + "px");

                    setTimeout(function () {
                        $('#popupIntervencionesAgente').css("top", topPopup + "px");
                    }, 550);
                });

            }, 250);

            $("#btnReporteAgente").unbind()
            $("#btnReporteAgente").click(function () {
                ReporteIntervencionesAgente();
            });
        },
        error: function (err) {
            alert("Ha ocurrido un error");
        }
    });
}

//Descargar Archivos Adjuntos en .zip
function descargarArchivosAdjuntos() {
    var tipo = listarIntercodiChecked();

    _descargarArchivosAdjuntosXId(tipo);
}

function _descargarArchivosAdjuntosXId(tipo) {
    if (tipo != null && tipo != "") {
        if (confirm("¿Desea descargar los archivos adjuntos de la información seleccionada?")) {
            $.ajax({
                type: 'POST',
                url: controler + 'DescargarArchivosSeleccionados',
                data: { tipo: tipo },
                success: function (evt) {
                    if (evt.Resultado != "-1") {
                        $('input[type=checkbox].ChkSeleccion').prop('checked', false);
                        $("#chkSeleccionar").prop('checked', false); // Deschekea el check seleccionar todos
                        window.location = controler + "ExportarZip?file_name=" + evt.Resultado;

                    } else {
                        alert("Ha ocurrido un error: " + evt.Mensaje);
                    }
                },
                error: function (err) {
                    alert("Error al descargar archivos");
                }
            });
        }
    } else {
        alert("Seleccione registros para descargar archivos.");
    }
}

//Descargar mensajes masivos en .Rar
function descargarMsjMasivos() {
    var tipo = listarIntercodiChecked();
    var progrcodi = $("#Progrcodi").val();

    if (tipo != null && tipo != "") {
        if (confirm("¿Desea descargar los mensajes de la información seleccionada?")) {
            $.ajax({
                type: 'POST',
                url: controler + 'DescargarMensajesMasivos',
                data: {
                    tipo: tipo,
                    progrcodi: progrcodi,
                    modulo: 1
                },
                success: function (evt) {
                    if (evt.Resultado != "-1") {
                        $('input[type=checkbox].ChkSeleccion').prop('checked', false);
                        $("#chkSeleccionar").prop('checked', false); // Deschekea el check seleccionar todos
                        //window.location = controler + 'DescargarArchivosIntervencionesZip';
                        window.location = controler + "ExportarZipMsjMasivos?file_name=" + evt.Resultado;

                    } else {
                        alert(evt.Mensaje);
                    }
                },
                error: function (err) {
                    alert("Error al descargar mensajes");
                }
            });
        }
    } else {
        alert("Seleccione registros para descargar mensajes.");
    }
}

//guardar Intervención Ejecutadas
function guardarIntervencionEjecutadas() {
    $("#alerta").hide();

    //Obtener filas con fechas editadas
    seleccionarFilasEditadas();

    if (arrayFilasEditadas.length > 0) {
        $.ajax({
            type: "POST",
            url: controler + "GuardarIntervEjecutadasEdicionFechas",
            traditional: true,
            data: {
                arrayFilasEditadas: JSON.stringify(arrayFilasEditadas)
            },
            success: function (evt) {
                if (evt.Resultado != "-1") {
                    _mostrarMensajeAlertaTemporal(true, evt.StrMensaje);

                    mostrarLista(true);
                } else {
                    alert(evt.StrMensaje);
                }
            },
            error: function (err) {
                alert("Lo sentimos, ha ocurrido un error inesperado");
            }
        });
    } else {
        alert("No existen ediciones para realizar la acción.");
    }
}

// obtiene las intervenciones editadas en fechas del listado ejecutadas 
function seleccionarFilasEditadas() {
    arrayFilasEditadas = [];

    //recorrer cada elemento de la lista
    for (key in LISTAGENERAL) {
        var item = LISTAGENERAL[key];

        //obtener fecha ini fin actuales
        var fechaIni = $('#' + item.Intercodi + '_InterfechainiDesc').val();
        var horaMinIni = $('#' + item.Intercodi + '_inihoraEjec').val();

        var fechaFin = $('#' + item.Intercodi + '_InterfechafinDesc').val();
        var horaMinFin = $('#' + item.Intercodi + '_finhoraEjec').val();

        if ((fechaIni != item.IniFecha || horaMinIni != item.IniHoraMinuto) ||
            (fechaFin != item.FinFecha || horaMinFin != item.FinHoraMinuto)) {

            var timeIni = horaMinIni.split(":");
            var horaIni = timeIni[0];
            var minIni = timeIni[1];
            var timeFin = horaMinFin.split(":");
            var horaFin = timeFin[0];
            var minFin = timeFin[1];

            //actualiza las fechas
            item.IniFecha = fechaIni;
            item.IniHora = horaIni;
            item.IniMinuto = minIni;
            item.FinFecha = fechaFin;
            item.FinHora = horaFin;
            item.FinMinuto = minFin;

            arrayFilasEditadas.push(item);
        }
    }
}

//>>>>>>>>>>>>> Manejo de Fechas para Edición de Ejecutados >>>>>>>>>>>>>>>>>>
function obtenerFechaFinProgramacionEjec24(fechaFinProgramado) {
    if (fechaFinProgramado == "") return false;

    var sDia = fechaFinProgramado.substring(0, 2);
    var sMes = fechaFinProgramado.substring(3, 5);
    var sAnio = fechaFinProgramado.substring(6, 10);
    var fechaProg = new Date(sAnio, sMes - 1, sDia);
    return new Date(fechaProg.setDate(fechaProg.getDate() + 1));
}

// Convierte objeto Date to string format DD/MM/YYYY
function convertirDateToString(date) {
    var d = date.getDate();
    var m = date.getMonth();   // JavaScript months are 0-11
    m += 1;
    var y = date.getFullYear();

    return (('0' + d).slice(-2) + "/" + ('0' + m).slice(-2) + "/" + y);
}

function formatFecha(sFecha, sHora, sMinute) {   // DD/MM/AAAA HH:mm
    if (sFecha == "") return false;

    var sDia = sFecha.substring(0, 2);
    var sMes = sFecha.substring(3, 5);
    var sAnio = sFecha.substring(6, 10);

    return new Date(sAnio, sMes - 1, sDia, sHora, sMinute);
}
//>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>

function ObtenerTiempoFaltante(fecLimite) {
    var now = new Date();
    var tiempoRestante = (new Date(fecLimite) - now + 1000) / 1000;
    var segundos = ('0' + Math.floor(tiempoRestante % 60)).slice(-2);
    var minutos = ('0' + Math.floor(tiempoRestante / 60 % 60)).slice(-2);
    var horas = ('0' + Math.floor(tiempoRestante / 3600 % 24)).slice(-2);
    var dias = Math.floor(tiempoRestante / (3600 * 24));

    return {
        tiempoRestante,
        segundos,
        minutos,
        horas,
        dias
    }
}

function ActualizarTiempoRestante(fecLimite, elem, msgFinal) {
    //Dividimos la fecha primero utilizando el espacio para obtener solo la fecha y el tiempo por separado
    var splitDate = fecLimite.split(" ");
    var date = splitDate[0].split("/");
    var time = splitDate[1].split(":");

    // Obtenemos los campos individuales para todas las partes de la fecha
    var dd = date[0];
    var mm = date[1] - 1;
    var yyyy = date[2];
    var hh = time[0];
    var min = time[1];
    var ss = time[2];

    // Creamos la fecha con Javascript
    var fecha = new Date(yyyy, mm, dd, hh, min, ss);

    const elemento = document.getElementById(elem);
    const intervalo = setInterval(() => {
        var tiempo = ObtenerTiempoFaltante(fecha);
        elemento.innerHTML = `${tiempo.dias}d:${tiempo.horas}h:${tiempo.minutos}m:${tiempo.segundos}s`;

        if (tiempo.tiempoRestante <= 1) {
            clearInterval(intervalo);
            var idPrograma = $("#Progrcodi").val();
            //window.location.href = controler + 'IntervencionesRegistro?progCodi=' + idPrograma;
            elemento.innerHTML = msgFinal;
        }

    }, 1000)
}

function habilitarReversion(idPrograma, idTipoprog) {

    if (confirm("¿Desea Habilitar la Reversión de Intervenciones?")) {
        $.ajax({
            type: 'POST',
            url: controler + 'HabilitarReversion',
            data: {
                progrCodi: idPrograma,
                idTipoProgramacion: idTipoprog
            },
            success: function (evt) {
                if (evt.Resultado != "-1") {

                    alert("Se Habilitó correctamente el Programa");

                    window.location.href = controler + 'IntervencionesRegistro?progCodi=' + idPrograma;

                } else {
                    alert("Ocurrió un error al Habilitar Reversión: " + evt.Mensaje);
                }
            },
            error: function (err) {
                alert("Ha ocurrido un error: " + evt.Mensaje);
            }
        });
    }
}

//funcion que calcula el ancho disponible para la tabla reporte
function getHeightTablaListado() {
    var altoDisponible = $(window).height()
        - 36 //titulo del iframe
        - 15 //padding-top mainLayout
        - 32 //cabecera filtro
        - (parseInt($("#alertaEmpresas").height()) > 0 && $("#alertaEmpresas").is(":visible") ? 100 : 0) //alerta abreviatura faltante
        - (parseInt($("#Contenido").height()) > 0 && $("#Contenido").is(":visible") ? 160 : 0) //body Filtros
        - (parseInt($("#btnEjecutarSustentoInclExcl").height()) > 0 ? (15 + 42) : 0) //boton simular 
        - (parseInt($("#btnEjecutarAprobacion").height()) > 0 ? (15 + 42) : 0) //boton simular 
        - 30 //leyenda 1
        - 30 //leyenda 2
        - $(".dataTables_filter").height()
        - $(".dataTables_scrollHead").height()
        - 30 //nota abajo de tabla
        - 15 //padding-boton mainLayout
        - 40 //adicional iframe
        ;

    if (ocultar > 0) altoDisponible -= 60;

    return altoDisponible > 0 ? altoDisponible : 100;
}

function ubicacion() {
    $.ajax({
        type: 'POST',
        url: controler + "ListarCboUbicacion",
        datatype: 'json',
        data: JSON.stringify({ idEmpresa: $('#Emprcodi').val() }),
        contentType: "application/json",
        success: function (modelo) {
            $("#cboUbicacion").empty();
            //$('#cboEquipo').empty();
            $.each(modelo.ListaCboUbicacion, function (k, v) {
                var option = '<option value =' + v.Areacodi + '>' + v.Areanomb + '</option>';
                $('#cboUbicacion').append(option);
            })
        },
        error: function (err) {
            alert("Error inesperado", $('#cboEmpresa').val());
        }
    });
}

function listarEquipoFiltro() {

    var areaCodi = $('#cboUbicacion').val() != null && $('#cboUbicacion')[0].length == $('#cboUbicacion').val().length ? "0" : $('#cboUbicacion').multipleSelect('getSelects').join(',');
    var famCodi = $('#cboFamilia').val() != null && $('#cboFamilia')[0].length == $('#cboFamilia').val().length ? "0" : $('#cboFamilia').multipleSelect('getSelects').join(',');
    var idTipoPrograma = parseInt(document.getElementById('idTipoProgramacion').value) || 0;
    $.ajax({
        type: 'POST',
        url: controler + "ListarEquiposXprograma",
        datatype: 'json',
        data: JSON.stringify({ idUbicacion: areaCodi, idFamilia: famCodi, evenclasecodi: idTipoPrograma }),
        contentType: "application/json",
        success: function (modelo) {
            $('#cboEquipo').empty();
            $.each(modelo.ListaCboEquipo, function (k, v) {
                var option = '<option value =' + v.Entero1 + '>' + v.String1 + '</option>';
                $('#cboEquipo').append(option);
            })
            $('#cboEquipo').multipleSelect({
                filter: true,
                placeholder: "SELECCIONE"
            });
            $("#cboEquipo").multipleSelect("refresh");
            $('#cboEquipo').multipleSelect('checkAll');
        }
    });
}

function ui_setInputmaskHoraMin(ref_element) {
    $(ref_element).inputmask({
        mask: "h:s",
        placeholder: "hh:mm",
        alias: "datetime",
        hourformat: 24
    });
}

function asignarEventosEjecutadas(intercodi) {
    var id_inihoraEjec = '#' + intercodi + "_inihoraEjec";
    var id_InterfechafinDesc = '#' + intercodi + '_InterfechafinDesc';
    var id_finhoraEjec = '#' + intercodi + "_finhoraEjec";

    ui_setInputmaskHoraMin(id_inihoraEjec);
    ui_setInputmaskHoraMin(id_finhoraEjec);

    $(id_inihoraEjec).on('focusout', function (e) {
        $(id_inihoraEjec).val(obtenerHoraValida($(id_inihoraEjec).val()));
    });
    $(id_finhoraEjec).on('focusout', function (e) {
        actualizarFechaFin(id_finhoraEjec, id_InterfechafinDesc);
        $(id_finhoraEjec).val(obtenerHoraValida($(id_finhoraEjec).val()));
    });
}

// Actualizar el Día a mostrar
function actualizarFechaFin(id_finhoraEjec, id_InterfechafinDesc) {
    var fechaSiguiente = convertirDateToString(obtenerFechaFinProgramacion24($("#InterfechainiD").val()));
    var fecha = $("#InterfechainiD").val();
    var hora = obtenerHoraValida($(id_finhoraEjec).val());
    if (hora == '00:00') {
        $(id_InterfechafinDesc).val(fechaSiguiente);
    } else {
        $(id_InterfechafinDesc).val(fecha);
    }
}

function obtenerHoraValida(hora) {
    if (hora !== undefined && hora != null) {
        hora = hora.replace('h', '0');
        hora = hora.replace('h', '0');

        hora = hora.replace('m', '0');
        hora = hora.replace('m', '0');

        //hora = hora.replace('s', '0');
        //hora = hora.replace('s', '0');
        return hora;
    }

    return '';
}

//obtener programaciones
function programaciones() {
    var tipoProgramacion = parseInt($('#TipoProgramacion').val()) || 0;

    if (tipoProgramacion > 0) {
        $.ajax({
            type: 'POST',
            url: controler + "ListarProgramaciones",
            datatype: "json",
            contentType: 'application/json',
            data: JSON.stringify({ idTipoProgramacion: tipoProgramacion }),

            success: function (evt) {
                $('#Programacion').empty();
                evt.ProgramacionRegistro;
                var option = '<option value="0">----- Seleccione ----- </option>';
                if (EVENCLASECODI_ANUAL == tipoProgramacion) option = '<option value="0">----- Todos (más reciente) ----- </option>';
                $.each(evt.ListaProgramaciones, function (k, v) {
                    if (v.Progrcodi == evt.Entidad.Progrcodi) {
                        option += '<option value =' + v.Progrcodi + ' selected>' + v.ProgrnombYPlazoCruzado + '</option>';
                    } else {
                        option += '<option value =' + v.Progrcodi + '>' + v.ProgrnombYPlazoCruzado + '</option>';
                    }
                })
                $('#Programacion').append(option);
                $('#Programacion').trigger("change");

                if ($('#TipoProgramacion').val() == $('#idTipoProgramacion').val()) {
                    $("#Programacion").val($("#Progrcodi").val())
                    obtenerFechaOperacion();
                }
            },
            error: function (err) {
                alert("Lo sentimos, se ha producido un error");
            }
        });
    } else {
        $("#Programacion").empty();
        var option = '<option value="0">----- Seleccione un tipo de programación ----- </option>';
        $('#Programacion').append(option);
    }
}

//obtener fechas de programación seleccionada
function obtenerFechaOperacion() {
    var tipoProgramacion = parseInt($('#TipoProgramacion').val()) || 0;
    var programacion = parseInt($('#Programacion').val()) || 0;

    if (tipoProgramacion == 0 || programacion == 0 || tipoProgramacion == "" || programacion == "") {
        return;
    }

    $.ajax({
        type: 'POST',
        url: controler + "ObtenerFechaProgramacion",
        datatype: "json",
        contentType: 'application/json',
        data: JSON.stringify({ progCodi: programacion }),
        success: function (model) {
            $("#InterfechainiD").val(model.Progrfechaini);
            $("#InterfechafinD").val(model.Progrfechafin);
        },
        error: function (err) {
            alert("Lo sentimos, se ha producido un error al obtener las fechas de operacion");
        }
    });
}

//deshabilitar opciones de "Transferir", "Importar", "Mantto Transf", y "agrupar/desagrupar".
function deshabilitarOpciones() {
    $("#IntervencionesCopiar").hide();
    $("#IntervencionesImportar").hide();
    $("#btnExportarExcel").hide();

    $("#btnReporteActividades").hide();

    //$("#IntervencionesAgrupar").hide();
    //$("#IntervencionesDesagrupar").hide();
}
function habilitarOpciones() {
    $("#IntervencionesCopiar").show();
    $("#IntervencionesImportar").show();
    $("#btnExportarExcel").show();

    $("#btnReporteActividades").show();
    //$("#IntervencionesAgrupar").show();
    //$("#IntervencionesDesagrupar").show();
}

function mostrarLista(flagOmitirValidarExisteEdicion) {
    //si el popup de mensajes está visible entonces cerrarlo
    $('#popupMensajes').bPopup().close();

    var objData = getObjetoFiltro();
    var msj = validarConsulta(objData);

    if (msj == "") {
        var mostrarAlertaGuardar = false;
        var realizarConsulta = true;
        if (objData.TipoProgramacion == 1 && !flagOmitirValidarExisteEdicion) {
            //Obtener filas con fechas editadas
            seleccionarFilasEditadas();

            if (arrayFilasEditadas.length > 0) mostrarAlertaGuardar = true;
        }

        if (mostrarAlertaGuardar && !confirm("Existe edición de fechas, ¿Desea realizar la consulta? De aceptar esta opción se perderán los cambios.")) realizarConsulta = false;

        if (realizarConsulta) {
            $('#listado').html('');
            ANCHO_LISTADO = $("#mainLayout").width() > 900 ? $("#mainLayout").width() - 25 : 900;

            $.ajax({
                type: 'POST',
                url: controler + "MostrarListadoRegistro",
                data: objData,
                dataType: 'json',
                success: function (evt) {
                    if (evt.Resultado != "-1") {

                        //mostrar alertaEmpresas
                        $("#alertaEmpresas").hide();
                        if (evt.ListaEmpresasValidate.length > 0) {
                            _mostrarAlertaEmpresas(evt);
                        }

                        //deshabilitar "Transferir", "Importar", "Mantto Transf", y "agrupar/desagrupar".
                        var fechaIniProg = $("#InterfechainiD").val();
                        var fechaFinProg = $("#InterfechafinD").val();

                        if (($("#Progrcodi").val() != $('#Programacion').val()) || (FECHAINIPROG != fechaIniProg) || (FECHAFINPROG != fechaFinProg)) {
                            deshabilitarOpciones();
                        }
                        else {
                            habilitarOpciones();
                        }

                        //Generar factores F1 y F2
                        if (objData.TipoProgramacion == 1 || objData.TipoProgramacion == 2 || objData.TipoProgramacion == 3)
                            generarDashboard(objData.TipoProgramacion);

                        $("#listado").hide();
                        $('#listado').css("width", ANCHO_LISTADO + "px");

                        var html = _dibujarTablaListado(evt);
                        $('#listado').html(html);

                        $("#chkSeleccionar").prop('checked', false);

                        var nuevoTamanioTabla = getHeightTablaListado();
                        //leyenda adicional
                        //if (evt.IdTipoProgramacion == 1 || evt.IdTipoProgramacion == 2) 
                        nuevoTamanioTabla -= 25;

                        //si es ejecutado
                        if (evt.IdTipoProgramacion == 1) {
                            LISTAGENERAL = evt.ListaFilaWeb;
                            var lista = evt.ListaFilaWeb;
                            for (key in lista) {
                                var item = lista[key];
                                //Asignar eventos
                                asignarEventosEjecutadas(item.Intercodi);
                            }
                        }

                        //Alerta creacion de interv por coordinador 
                        $("#leyenda_modificado_agente").hide();

                        var alertaMsj = evt.ListaIntervCount;
                        if (document.getElementById('idTipoProgramacion').value == "1" && alertaMsj > 0) {
                            $("#leyenda_modificado_agente").show();
                        }

                        $("#listado").show();

                        var targetsnew;
                        var Ordenacion;

                        var numAlertas = evt.PintarAlerta;
                        if (numAlertas > 0) {
                            targetsnew = [];
                            targetsnew.push(0);
                            targetsnew.push(1);
                            targetsnew.push(2);
                            Ordenacion = [[6, 'asc']]; //orden a partir de la columna EMPRESA
                        }
                        else {
                            targetsnew = [0, 1];
                            Ordenacion = [[5, 'asc']]; //orden a partir de la columna EMPRESA
                        }

                        $('#TablaConsultaIntervencion').dataTable({
                            "ordering": true,
                            "columnDefs": [{
                                "targets": targetsnew,
                            }],
                            order: Ordenacion,
                            "info": false,
                            "searching": true,
                            "paging": false,
                            "scrollX": true,
                            "scrollY": $('#listado').height() > 200 ? nuevoTamanioTabla + "px" : "100%"
                        });

                        viewEvent();
                    } else {
                        alert(evt.Mensaje);
                    }
                },
                error: function (err) {
                    alert("Lo sentimos, ha ocurrido un error inesperado");
                }
            });
        }
    } else {
        alert(msj);
    }
}

function _dibujarTablaListado(model) {
    var lista = model.ListaFilaWeb;

    var htmlThAlerta = '';
    if (model.PintarAlerta > 0) {
        var anchoThAlerta = model.PintarAlerta * 20;
        htmlThAlerta = `<th style="text-align:center;width: ${anchoThAlerta}px">Alertas<br> detectadas</th>`;
    }

    var cadena = '';
    cadena += `
    <table border="0" class="pretty tabla-icono" id="TablaConsultaIntervencion" style="overflow: auto; height:auto; width: 2000px !important; white-space: nowrap">
        <thead>
            <tr>
                ${htmlThAlerta}
                <th style="text-align:center;width:2%">Sel.</th>
                <th style="text-align:center;width:4%">Acción</th>
                <th style="text-align:center;width:3%">Arch.</th>
                <th style="text-align:center;width:6%">Tip.<br> Interv.</th>
                <th style="text-align:center;width:6%">Estado</th>
                <th style="text-align:center;width:6%">Empresa</th>
                <th style="text-align:center;width:6%">Operador</th>
                <th style="text-align:center;width:6%">Ubicacion</th>
                <th style="text-align:center;width:6%">Tipo</th>
                <th style="text-align:center;width:6%">Equipo</th>
                <th style="text-align:center;width:5%">Fecha<br> inicio</th>
                <th style="text-align:center;width:5%">Fecha<br> fin</th>
                <th style="text-align:center;width:5%">MW <br>Ind.</th>
                <th style="text-align:center;width:5%">Disp.</th>
                <th style="text-align:center;width:5%">Interrup.</th>
                <th style="text-align:center;width:5%">Sist. Aisl.</th>
                <th style="text-align:center;width:5%">Inst. Prov.</th>
                <th style="text-align:center;width:10%">Descripción</th>
                <th style="text-align:center;width:5%">Mensajes <br>enviados</th>
                <th style="text-align:center;width:5%">Usuario Mod.</th>
                <th style="text-align:center;width:5%">Fec. Mod.</th>
            </tr>
        </thead>
        <tbody>
    `;

    for (key in lista) {
        var item = lista[key];

        //la fila está seleccionada
        var checkedSelec = "";
        if (item.ChkAprobacion) checkedSelec = ' checked ';

        //la fila tiene mensaje
        var checkedMsj = "";
        if (item.ChkMensaje) checkedMsj = ' checked ';

        //la empresa es distinta al operador
        var sStyleEmp = "";
        if (item.EmprNomb != item.Operadornomb) sStyleEmp = ";font-weight: bold; ";

        //intervención mayor a 1 día
        var styleFraccionado = item.EstaFraccionado ? "; font-weight: bold;color: blue;" : "";

        //intervención con rago de hora
        var styleConnsecutivoXHora = item.EsConsecutivoRangoHora ? "; font-weight: bold;color: green;" : "";

        //
        var sdisabled = "";
        var sStyle = "";
        if (item.Interfuentestado >= 5 && model.IdTipoProgramacion == 1) {
            //Creado por la extranet
            sStyle = "background-color:#ffe5ff;";
        }
        if (item.Estadocodi == 3) {   // Rechazado
            sStyle = "background-color:#FF2C2C; text-decoration:line-through";
            sdisabled = "disabled";
        }
        if (item.Interdeleted == 1) {   // Eliminado
            sStyle = "background-color:#E0DADA; text-decoration:line-through";
            sdisabled = "disabled";
        }
        if (item.Interprocesado == 2) {   // Revertido
            sStyle = "background-color:#FF9746;";
            if (item.Interdeleted == 1) sStyle = "background-color:#FF9746; text-decoration:line-through";
        }

        //
        var tdAlerta = _tdAlerta(model, item);
        var tdOpciones = _tdOpcionesXInter(model, item, sdisabled, sStyle);
        var tdFile = _tdFile(model, item, sdisabled, sStyle);
        var tdFechas = _tdFechas(model, item, sdisabled, sStyle, styleFraccionado, styleConnsecutivoXHora);

        //habilitar  check. Cuando sea eliminado puede ser "Recuperado", para otras opciones no deben considerarse a los eliminados
        var claseCheck = (item.Interdeleted == 1) ? "ChkSeleccion2" : "ChkSeleccion";
        var tdSelec = `<input type="checkbox" ${checkedSelec} class="${claseCheck}" value="${item.Intercodi}" id="${item.Intercodi}" />`;

        cadena += `
            
            <tr id="fila_${item.Intercodi}">
                ${tdAlerta}
                <td style="${sStyle}">
                    ${tdSelec}
                </td>
                ${tdOpciones}
                ${tdFile}
                <td style="text-align:left; ${sStyle}">${item.Tipoevenabrev}</td>
                <td style="text-align:left; ${sStyle} ${sStyleEmp}">${item.EstadoRegistro}</td>
                <td style="text-align:left; ${sStyle} ${sStyleEmp}">${item.EmprNomb}</td>
                <td style="text-align:left; ${sStyle}">${item.Operadornomb}</td>
                <td style="text-align:left; ${sStyle}">${item.AreaNomb}</td>
                <td style="text-align:left; ${sStyle}">${item.Famabrev}</td>
                <td style="text-align:left; ${sStyle}">${item.Equiabrev}</td>
                ${tdFechas}
                <td style="text-align:right; ${sStyle}">${item.Intermwindispo}</td>
                <td style="text-align:center; ${sStyle}">${item.InterindispoDesc}</td>
                <td style="text-align:center; ${sStyle}">${item.InterinterrupDesc}</td>
                <td style="text-align:center; ${sStyle}">${item.IntersistemaaisladoDesc}</td>
                <td style="text-align:center; ${sStyle}">${item.InterconexionprovDesc}</td>
                <td style="text-align:left; ${sStyle}">${item.Interdescrip}</td>
                <td style="${sStyle}">
                    <input type="checkbox" ${checkedMsj} class="ChkSeleccion1" disabled="disabled"  />
                </td>
                <td style="text-align:left; ${sStyle}">${item.UltimaModificacionUsuarioDesc}</td>
                <td style="text-align:left; ${sStyle}">${item.UltimaModificacionFechaDesc}</td>
            </tr>
        `;
    }
    cadena += "</tbody></table>";

    return cadena;
}

function _tdAlerta(Model, item) {
    var html = '';

    if (Model.PintarAlerta > 0) {
        html += "<td>";
        if (Model.TieneAlertaNoEjecutado) {
            if (item.AlertaNoEjecutado == 1) {
                html += `      
                        <div style="display: inline; width: 1%">
                            <div class='bellImgIntervencion' title='Alerta de Intervención no Ejecutada' onclick='verAlertaIntervencionNoEjec(1)' />
                        </div>`;
            }
            else {
                html += `<div style="display: inline; margin-right: 10px; margin-left: 10px;"></div>`;
            }
        }

        if (Model.TieneAlertaEstadoPendiente) {
            if (item.TieneAlertaEstadoPendiente) {
                html += `      
                        <div style="display: inline; width: 1%">
                            <div class='bellImgHoraOperacion' title='Alerta de Estado Pendiente' onclick='verAlertaEstadoPendiente(1)' />
                        </div>`;
            }
            else {
                html += `<div style="display: inline; margin-right: 10px; margin-left: 10px;"></div>`;
            }
        }


        if (Model.TieneAlertaHoraOperacion) {
            if (item.TieneAlertaHoraOperacion) {
                html += `  
                        <div style="display: inline; width: 1%">
                            <div class='bellImgHoraOperacion' title='Alerta de Hora de Operación' onclick='verAlertaHOPXIntervencion(${item.Progrcodi}, ${item.Intercodi})' />
                        </div>`;
            }
            else {
                html += `<div style="display: inline; margin-right: 10px; margin-left: 10px;"></div>`;
            }
        }

        //para este caso existen prioridades de que alerta mostrar
        if (Model.TieneAlertaEms || Model.TieneAlertaScada || Model.TieneAlertaIDCC) {
            if (item.TieneAlertaIDCC) {
                html += `    
                        <div style="display: inline; width: 1%">
                            <div class='bellImgIDCC' title='Alerta de datos IDCC' onclick='verAlertaIDCCXIntervencion(${item.Progrcodi}, ${item.Intercodi})' />
                        </div>`;
            }
            else {
                if (item.TieneAlertaEms) {
                    html += `
                        <div style="display: inline; width: 1%">
                            <div class='bellImgEms' title='Alerta de Señales estimadas del EMS' onclick='verAlertaEmsXIntervencion(${item.Progrcodi}, ${item.Intercodi})' />
                        </div>`;
                }
                else {
                    if (item.TieneAlertaScada) {
                        html += `
                        <div style="display: inline; width: 1%">
                            <div class='bellImgScada' title='Alerta de Señales Scada' onclick='verAlertaScadaXIntervencion(${item.Progrcodi}, ${item.Intercodi})' />
                        </div>`;
                    }
                    else {
                        html += `<div style="display: inline; margin-right: 10px; margin-left: 10px;"></div>`;
                    }
                }
            }
        }

        if (Model.TieneAlertaPR21) {
            if (item.TieneAlertaPR21) {
                html += `
                        <div style="display: inline; width: 1%">
                            <div class='bellImgPR21' title='Alerta de datos PR21' onclick='verAlertaPR21XIntervencion(${item.Progrcodi}, ${item.Intercodi})' />
                        </div>`;
            }
            else {
                html += `<div style="display: inline; margin-right: 10px; margin-left: 10px;"></div>`;
            }
        }

        if (Model.TieneAlertaMedidores) {
            if (item.TieneAlertaMedidores) {
                html += `
                        <div style="display: inline; width: 1%">
                            <div class='bellImgMedidores' title='Alerta de Medidores de Generación' onclick='verAlertaMedidoresXIntervencion(${item.Progrcodi}, ${item.Intercodi})' />
                        </div>`;
            }
            else {
                html += `<div style="display: inline; margin-right: 10px; margin-left: 10px;"></div>`;
            }
        }
        html += "</td>";
    }

    return html;
}

function _tdOpcionesXInter(Model, item, sdisabled, sStyle) {
    var html = '';

    var tagA = `
            <a href="#" id="view, ${item.Intercodi}" class="viewLogTrazabilidad">
                ${IMG_TRAZABILIDAD}
            </a>`;
    if (item.Intercodsegempr == null || item.Intercodsegempr == "") tagA = "";

    var htmlNota = `
            <a href="#" id="view, ${item.Intercodi}" class="">
                ${IMG_NOTA}
            </a>`;
    if (item.Internota == null || item.Internota == "") htmlNota = "";

    var htmlImgComunicacion = '';
    switch (item.TipoComunicacion) {
        case 1: htmlImgComunicacion = IMG_COMUNICACION_NINGUNA;
            break;
        case 2: htmlImgComunicacion = IMG_COMUNICACION_TODO_LEIDO;
            break;
        case 3: htmlImgComunicacion = IMG_COMUNICACION_PENDIENTE_LEER;
            break;
        case 4: htmlImgComunicacion = IMG_COMUNICACION_EXISTE_MENSAJE;
            break;
    }

    if (sdisabled == "") {
        html += `<td style="text-align: left; ${sStyle}">`;

        if (Model.EsCerrado) {
            if (Model.EsRevertido) {
                html += `<a href="#" id="view, ${item.Intercodi}" class="viewEdicion">
                        ${IMG_EDITAR}
                    </a>
                `;
            }
            else {
                html += `<a href="#" id="view, ${item.Intercodi}" class="viewEdicion">
                        ${IMG_VER}
                    </a>
                `;
            }
        }
        else {
            html += `   
                <a href="#" id="view, ${item.Intercodi}" class="viewEdicion">
                    ${IMG_EDITAR}
                </a>
            `;
        }
        html += `   
            <a href="#" id="view, ${item.Intercodi}" class="viewComunicaciones">
                ${htmlImgComunicacion}
            </a>
            <a href="#" id="view, ${item.Intercodi}" class="viewLog">
                ${IMG_MODIFICACION}
            </a>
            ${tagA}
            <a href = "#" id = "view, ${item.Intercodi}, ${item.Equicodi}" class="viewHistoriaEq" >
                ${IMG_HISTORIA}
            </a >
            ${htmlNota}
        </td>`;
    }
    else {
        var htmlVerEliminado = '';
        if (item.Interdeleted == 1) {
            htmlVerEliminado = `
             <a href="#" id="view, ${item.Intercodi}" class="viewEdicion">
                 ${IMG_VER}
             </a>`;
        }

        html += `<td style="text-align:left; ${sStyle}">
            ${htmlVerEliminado}
            <a href="#" id="view, ${item.Intercodi}" class="viewComunicaciones">
                ${htmlImgComunicacion}
            </a>
            <a href="#" id="view, ${item.Intercodi}" class="viewLog">
                ${IMG_MODIFICACION}
            </a>
            ${tagA}
            <a href = "#" id = "view, ${item.Intercodi}, ${item.Equicodi}" class="viewHistoriaEq" >
                ${IMG_HISTORIA}
            </a >
            ${htmlNota}
        </td>`;
    }

    return html;
}

function _tdFile(Model, item, sdisabled, sStyle) {
    var html = `
        <td style="text-align:center; ${sStyle}">
    `;

    if (item.Interisfiles == 'S') {
        html += `
            <a href="#" id="view, ${item.Intercodi}" class="viewDescargaAdjunto">
                ${IMG_FILE}
                </a>
        `;
    }

    if (item.Interflagsustento == 1) {
        html += `
            <a href="#" id="view, ${item.Intercodi}" class="viewSustentoPdf">
                ${IMG_SUSTENTO}
                </a>
        `;
    }

    html += `
        </td>`;

    return html;
}

function _tdFechas(Model, item, sdisabled, sStyle, styleFraccionado, styleConnsecutivoXHora) {
    var html = '';

    if (Model.IdTipoProgramacion == 1) {

        html += `<td style="text-align:center; width:100px; ${sStyle} ${styleFraccionado}">
                    <table style="width:30px;float: left;">
                        <tr>
                            <td style="text-align:left; border: hidden; ${sStyle}">
                                <input type="text" id="${item.Intercodi}_InterfechainiDesc" class="viewFechaIni" value="${item.IniFecha}" disabled style="width: 75px; height: 15px;" />
                            </td>
                            <td style="padding-top: 0px; border: hidden;">
                                <input type="text" id="${item.Intercodi}_inihoraEjec" class="viewHoraIni" value="${item.IniHoraMinuto}" style="background-color: white; width: 50px; height: 15px;" autocomplete="off" />
                            </td>
                         </tr>
                    </table>
                  </td>

                  <td style="text-align:center; width:100px; ${sStyle} ${styleFraccionado}">
                    <table style="width:30px;float: left;">
                        <tr>
                            <td style="text-align:left; border: hidden; ${sStyle}">
                                <input type="text" id="${item.Intercodi}_InterfechafinDesc" class="viewFechaFin"  value="${item.FinFecha}" disabled style="width: 75px; height: 15px;" />
                            </td>
                            <td style="padding-top: 0px; border: hidden;">
                                <input type="text" id="${item.Intercodi}_finhoraEjec" class="viewHoraIni" value="${item.FinHoraMinuto}" style="background-color: white; width: 50px; height: 15px;" autocomplete="off" />
                            </td>
                          </tr>
                     </table>
                  </td>`;
    }
    else {
        html += `<td style="text-align:center; ${sStyle} ${styleFraccionado} ${styleConnsecutivoXHora}">${item.InterfechainiDesc}</td>
                <td style="text-align:center; ${sStyle} ${styleFraccionado} ${styleConnsecutivoXHora}">${item.InterfechafinDesc}</td>`;
    }

    return html;
}

function eliminarMasivo() {
    $("#alerta").hide();
    var tipo = listarIntercodiChecked();

    if (tipo != null && tipo != "") {
        if (confirm("¿Desea eliminar la información seleccionada?")) {
            $.ajax({
                type: 'POST',
                url: controler + 'EliminarSeleccionados',
                data: { tipo: tipo },
                success: function (evt) {
                    if (evt.Resultado != "-1") {
                        $('input[type=checkbox].ChkSeleccion').prop('checked', false);
                        $("#chkSeleccionar").prop('checked', false); // Deschekea el check seleccionar todos

                        mostrarLista();
                        _mostrarMensajeAlertaTemporal(true, evt.Mensaje);
                    } else {
                        alert(evt.Mensaje);
                    }
                },
                error: function (err) {
                    alert("Ocurrió un error al eliminar registro(s)");
                }
            });
        }
    } else {
        alert("Seleccione registro por eliminar.");
    }
}

function recuperarIntervenciones() {
    $("#alerta").hide();
    var tipo = listarIntercodiChecked2();
    var progrCodi = document.getElementById('Progrcodi').value;

    if (tipo != null && tipo != "") {
        if (confirm("¿Desea recuperar la información seleccionada?")) {
            $.ajax({
                type: 'POST',
                url: controler + 'RecuperarIntervenciones',
                data: { tipo: tipo, progrCodi: progrCodi },
                success: function (evt) {
                    if (evt.Resultado != "-1") {
                        if (evt.Resultado == "0") {
                            alert("No se permite recuperar intervenciones que provocan duplicidad a las existentes");
                        } else {
                            $('input[type=checkbox].ChkSeleccion2').prop('checked', false);
                            $("#chkSeleccionar").prop('checked', false); // Deschekea el check seleccionar todos

                            mostrarLista();
                            _mostrarMensajeAlertaTemporal(true, evt.Mensaje);
                        }
                    } else {
                        alert(evt.Mensaje);
                    }
                },
                error: function (err) {
                    alert("Ocurrió un error al recuperar registro(s)");
                }
            });
        }
    } else {
        alert("Seleccione registro por recuperar.");
    }
}

function reporteIntervencionesXls() {

    var objData = getObjetoFiltro();
    var msj = validarConsulta(objData);

    if (msj == "") {
        $.ajax({
            type: 'POST',
            url: controler + 'ExportarIntervenciones',
            data: objData,
            dataType: 'json',
            success: function (evt) {
                if (evt.Resultado != "-1") {
                    var paramList = [
                        { tipo: 'input', nombre: 'file', value: evt.NombreArchivo }
                    ];
                    var form = CreateForm(controler + 'AbrirArchivo', paramList);
                    document.body.appendChild(form);
                    form.submit();
                }
                else {
                    alert('Ha ocurrido un error');
                }
            },
            error: function (err) {
                alert('Ha ocurrido un error');
            }
        });
    } else {
        alert(msj);
    }
}

function reporteTablaIntervenciones(tipoReporte) {
    var objData = getObjetoFiltro();
    objData.TipoReporte = tipoReporte;

    var msj = validarConsulta(objData);

    if (msj == "") {
        $.ajax({
            type: 'POST',
            url: controler + 'ExportarReporteTablaIntervenciones',
            data: objData,
            dataType: 'json',
            success: function (model) {
                if (model.Resultado !== "-1") {
                    var paramList = [
                        { tipo: 'input', nombre: 'file', value: model.NombreArchivo }
                    ];
                    var form = CreateForm(controler + 'AbrirArchivo', paramList);
                    document.body.appendChild(form);
                    form.submit();
                }
                else {
                    alert('Ha ocurrido un error');
                }
            },
            error: function (err) {
                alert('Ha ocurrido un error');
            }
        });
    } else {
        alert(msj);
    }
}

function reporteIntervencionesPotIndispXls() {

    var objData = getObjetoFiltro();
    var msj = validarConsulta(objData, true);

    if (msj == "") {
        $.ajax({
            type: 'POST',
            url: controler + 'ExportarReporteTablaPotenciaIndisponible',
            data: objData,
            dataType: 'json',
            success: function (evt) {
                if (evt.Resultado != "-1") {
                    var paramList = [
                        { tipo: 'input', nombre: 'file', value: evt.NombreArchivo }
                    ];
                    var form = CreateForm(controler + 'AbrirArchivo', paramList);
                    document.body.appendChild(form);
                    form.submit();
                }
                else {
                    alert('Ha ocurrido un error');
                }
            },
            error: function (err) {
                alert('Ha ocurrido un error');
            }
        });
    } else {
        alert(msj);
    }
}

function mensaje() {
    var intercodis = listarIntercodiChecked();

    if (intercodis == "") {
        alert("Debe seleccionar una o varias intervenciones.");
    } else {
        //Modificación 15/05/2019
        var paramList = [
            { tipo: 'input', nombre: 'intercodis', value: intercodis },
            { tipo: 'input', nombre: 'origen', value: 'registro' }
        ];
        var form = CreateForm(controler + 'IntervencionesMensajeRegistro', paramList);
        document.body.appendChild(form);
        form.submit();
    }
    return false;
}

//18-08-2023 Iteracion2
function cambiarestado() {
    $('#popupFormCambiarEstado').html('');

    var intercodis = listarIntercodiChecked();
    if (intercodis == "") {
        alert("Debe seleccionar una o varias intervenciones.");
    } else {
        $.ajax({
            type: 'POST',
            url: controler + "IntervencionesCambiarEstado",
            data: {
                stringIntervenciones: intercodis,
                rep: 1
            },
            success: function (evt) {
                $('#popupFormCambiarEstado').html(evt);

                setTimeout(function () {
                    $('#popupFormCambiarEstado').bPopup({
                        modalClose: false,
                        easing: 'easeOutBack',
                        speed: 450,
                        transition: 'slideDown'
                    });
                }, 50);
            },
            error: function (err) {
                alert("Lo sentimos, ha ocurrido un error inesperado");
            }
        });
    }

}

function listarIntercodiChecked() {
    var selected = [];
    $('input[type=checkbox].ChkSeleccion').each(function () {
        if ($(this).is(":checked")) {
            selected.push($(this).attr('id'));
        }
    });

    return selected.join(";");
}

function listarIntercodiChecked2() {
    var selected = [];
    $('input[type=checkbox].ChkSeleccion2').each(function () {
        if ($(this).is(":checked")) {
            selected.push($(this).attr('id'));
        }
    });

    return selected.join(";");
}

function copiarIntervenciones() {
    var progrCodi = document.getElementById('Progrcodi').value;
    var tipoProgramacion = document.getElementById('idTipoProgramacion').value;

    if (confirm("¿Desea transferir la información de intervenciones?")) {
        $.ajax({
            type: 'POST',
            url: controler + 'CopiarIntervenciones',
            data: {
                idProgramacion: progrCodi,
                idTipoProgramacion: tipoProgramacion
            },
            dataType: 'json',
            success: function (result) {
                if (result.Resultado != "-1") {
                    mostrarLista();
                    alert("¡Se ha realizado correctamente la transferencia de registros de intervenciones!")
                } else {
                    alert(result.Mensaje);
                }
            },
            error: function (err) {
                alert("Ha ocurrido un error");
            }
        });
    }
}

function generarExcelXls() {
    var progrCodi = document.getElementById('Progrcodi').value;
    var tipoProgramacion = document.getElementById('idTipoProgramacion').value;

    $.ajax({
        type: 'POST',
        url: controler + 'ExportarIntervencionesXLS',
        data: {
            idProgramacion: progrCodi, idTipoProgramacion: tipoProgramacion
        },
        dataType: 'json',
        success: function (evt) {

            alert(evt.Mensaje);

            if (evt.Resultado != "-1") {
                var paramList = [
                    { tipo: 'input', nombre: 'file', value: evt.NombreArchivo }
                ];
                var form = CreateForm(controler + 'abrirarchivo', paramList);
                document.body.appendChild(form);
                form.submit();
            }
        },
        error: function (err) {
            alert('Ha ocurrido un error');
        }
    });
}

function generarManttoConsultaRegistro() {
    var objData = getObjetoFiltro();
    var msj = validarConsulta(objData);

    if (msj == "") {
        $.ajax({
            type: 'POST',
            url: controler + 'GenerarManttoConsultaRegistro',
            data: objData,
            dataType: 'json',
            success: function (evt) {
                if (evt.Resultado != "-1") {
                    var paramList = [
                        { tipo: 'input', nombre: 'file', value: evt.NombreArchivo }
                    ];
                    var form = CreateForm(controler + 'AbrirArchivo', paramList);
                    document.body.appendChild(form);
                    form.submit();
                }
                else {
                    alert('Ha ocurrido un error');
                }
            },
            error: function (err) {
                alert('Ha ocurrido un error');
            }
        });
    } else {
        alert(msj);
    }
}

function descargarManttoPlantilla() {

    var objData = getObjetoFiltro();
    var msj = validarConsulta(objData);

    if (msj == "") {
        $.ajax({
            type: 'POST',
            url: controler + 'DescargarManttoPlantillaActualizada',
            data: objData,
            dataType: 'json',
            success: function (evt) {
                if (evt.Resultado != "-1") {
                    var paramList = [
                        { tipo: 'input', nombre: 'file', value: evt.NombreArchivo }
                    ];
                    var form = CreateForm(controler + 'AbrirArchivo', paramList);
                    document.body.appendChild(form);
                    form.submit();
                }
                else {
                    alert('Ha ocurrido un error');
                }
            },
            error: function (err) {
                alert('Ha ocurrido un error');
            }
        });
    } else {
        alert(msj);
    }
}

function descargarFileTrazabilidad(fullPath, filename) {
    //Modificación 15/05/2019
    var paramList = [
        { tipo: 'input', nombre: 'fullPath', value: fullPath },
        { tipo: 'input', nombre: 'filename', value: filename }
    ];
    var form = CreateForm(controler + 'DescargarArchivoDesdeCualquierDirectorio', paramList);
    document.body.appendChild(form);
    form.submit();
    //:fin
    //window.location = controler + 'DescargarArchivoDesdeCualquierDirectorio?fullPath=' + fullPath + '&&filename=' + filename;
}

function descargarFileSustento(intercodi) {
    window.location = controler + 'DescargarPDFSustento?intercodi=' + intercodi;
}

function viewEvent() {
    $('.viewEdicion').click(function (event) {
        event.preventDefault();
        interCodi = $(this).attr("id").split(",")[1];
        abrirPopupEdicion(interCodi);
    });

    $('.viewComunicaciones').click(function (event) {
        event.preventDefault();
        interCodi = $(this).attr("id").split(",")[1];
        abrirPopupComunicaciones(interCodi);
    });

    $('.viewLog').click(function (event) {
        event.preventDefault();
        interCodi = $(this).attr("id").split(",")[1];
        abrirPopupLog(interCodi);
    });

    $('.viewLogTrazabilidad').click(function (event) {
        event.preventDefault();
        interCodi = $(this).attr("id").split(",")[1];
        abrirPopupLogTrazabilidad(interCodi);
    });

    $('.viewSustentoPdf').click(function (event) {
        event.preventDefault();
        interCodi = $(this).attr("id").split(",")[1];
        descargarFileSustento(interCodi);
    });

    $('.viewHistoriaEq').click(function (event) {
        event.preventDefault();
        var equipo = $(this).attr("id").split(",")[2];
        abrirPopupHistoriaEquipo(equipo);
    });

    $('.viewDescargaAdjunto').click(function (event) {
        event.preventDefault();
        interCodi = $(this).attr("id").split(",")[1];
        _descargarArchivosAdjuntosXId(interCodi);
    });
};

function abrirPopupEdicion(interCodi) {
    $('#popupFormIntervencion').html('');

    var objParam = {
        interCodi: interCodi,
        progrCodi: 0,
        tipoProgramacion: 0,
        escruzadas: false,
    };

    //IntervencionesFormulario.js
    mostrarIntervencion(objParam);
}

function abrirPopupNuevo() {
    APP_MOSTRAR_MODAL_EQUIPO_YA_CARGADO = 0;

    $('#popupFormIntervencion').html('');

    var objParam = {
        interCodi: 0,
        progrCodi: parseInt($("#Progrcodi").val()) || 0,
        tipoProgramacion: parseInt($("#idTipoProgramacion").val()) || 0,
        escruzadas: false,
    };

    //IntervencionesFormulario.js
    mostrarIntervencion(objParam);
}

function abrirPopupLog(interCodi) {
    $.ajax({
        type: 'POST',
        url: controler + "ListadoModificaciones",
        data: {
            interCodi: interCodi
        },
        success: function (evt) {
            $('#popup').html(evt);

            $('#popup').bPopup({
                modalClose: false,
                easing: 'easeOutBack',
                speed: 50,
                transition: 'slideDown'
            });

            setTimeout(function () {
                $('#TablaConsultaMensajes').dataTable({
                    "ordering": true,
                    "info": false,
                    "sDom": 'ft',
                    "searching": false,
                    "paging": false,
                    "scrollX": true,
                    scrollCollapse: false,
                    "destroy": "true",
                    "bAutoWidth": false,
                    "scrollY": 500 + "px"
                });
            }, 150);
        },
        error: function (err) {
            alert("Lo sentimos, ha ocurrido un error inesperado");
        }
    });
}

function abrirPopupLogTrazabilidad(interCodi) {
    $.ajax({
        type: 'POST',
        url: controler + "ListadoTrazabilidad",
        data: {
            interCodi: interCodi
        },
        success: function (evt) {
            $('#popup').html(evt);

            setTimeout(function () {
                $('#popup').bPopup({
                    modalClose: false,
                    easing: 'easeOutBack',
                    speed: 450,
                    transition: 'slideDown'
                });
            }, 50);
        },
        error: function (err) {
            alert("Lo sentimos, ha ocurrido un error inesperado");
        }
    });
}

function agruparIntervenciones() {
    $("#alerta").hide();
    var tipo = listarIntercodiChecked();

    if (tipo != null && tipo != "") {
        if (confirm("¿Desea Agrupar la información seleccionada?")) {
            $.ajax({
                type: 'POST',
                url: controler + 'AgruparIntervenciones',
                data: { tipo: tipo },
                success: function (evt) {
                    if (evt.Resultado != "-1") {
                        if (evt.Resultado == "0") {
                            alert("No existen registros válidos para realizar la acción o no son continuos");
                        } else {
                            $('input[type=checkbox].ChkSeleccion').prop('checked', false);
                            $("#chkSeleccionar").prop('checked', false); // Deschekea el check seleccionar todos

                            mostrarLista();
                            _mostrarMensajeAlertaTemporal(true, evt.Mensaje);
                        }
                    } else {
                        alert(evt.Mensaje);
                    }
                },
                error: function (err) {
                    alert("Ocurrió un error al Agrupar registro(s)");
                }
            });
        }
    } else {
        alert("Seleccione registro por Agrupar.");
    }
}

function desagruparIntervenciones(opcion, strHoraIni, strHoraFin) {
    $("#alerta").hide();
    var tipo = listarIntercodiChecked();

    var msj = "";
    //Validar las horas strHoraIni, strHoraFin
    if (opcion == 2) {
        msj = validarHorasDesagregadas(strHoraIni, strHoraFin);
    }

    if (msj == "") {
        $.ajax({
            type: 'POST',
            url: controler + 'DesagregarIntervenciones',
            data: {
                opcion: opcion,
                tipo: tipo,
                strHoraIni: strHoraIni,
                strHoraFin: strHoraFin
            },
            success: function (evt) {
                if (evt.Resultado != "-1") {
                    if (evt.Resultado == "0") {
                        alert("No existen registros válidos para realizar la acción");
                    } else {
                        $('input[type=checkbox].ChkSeleccion').prop('checked', false);
                        $("#chkSeleccionar").prop('checked', false); // Deschekea el check seleccionar todos
                        $('#popupDesagrupar').bPopup().close();
                        mostrarLista();
                        _mostrarMensajeAlertaTemporal(true, evt.Mensaje);
                    }
                } else {
                    alert("Ocurrió un error al desagrupar");
                }
            },
            error: function (err) {
                alert("Ocurrió un error al desagrupar");
            }
        });
    }
    else {
        alert(msj);
    }
}

//validar Horas desagregadas
function validarHorasDesagregadas(strHoraIni, strHoraFin) {
    var validacion = "";
    if ($("#desagregarHoraIni").val() == null || $("#desagregarHoraIni").val() == '') {
        validacion = "Debe ingresar las horas Inicio y final";
    }
    else {
        //Dividimos la hora y minuto
        var timeInicio = strHoraIni.split(":");
        var timeFin = strHoraFin.split(":");
        var iniHora = timeInicio[0];
        var iniMin = timeInicio[1];
        var finHora = timeFin[0];
        var finMin = timeFin[1];

        var horainicial = iniHora + "/" + iniMin + "/" + "00";
        var horafinal = finHora + "/" + finMin + "/" + "00";

        if (validarFechaRegistro(horainicial, horafinal)) {
            validacion = "Hora Incio debe ser menor a la hora Final.";
        }
    }
    return validacion;
}

function validarFechaRegistro(fecha1, fecha2) {
    var segundosfecha1 = convertirASegundos(fecha1);
    var segundosfecha2 = convertirASegundos(fecha2);

    var diferencia = segundosfecha2 - segundosfecha1;

    if (diferencia <= 0)
        return true;
    else
        return false;
}

function convertirASegundos(tiempo) {

    var x = tiempo.split('/');
    var hor = x[0];
    var min = x[1];
    var sec = x[2];
    return (Number(sec) + (Number(min) * 60) + (Number(hor) * 3600));
}

///////////////////////////////////////////////////////////////////////////////////////
// Utilitario
///////////////////////////////////////////////////////////////////////////////////////
function validarConsulta(objFiltro, validarConjEq) {
    var listaMsj = [];

    //evenclase
    if (objFiltro.tipoProgramacion <= 0)
        listaMsj.push("Seleccione Tipo de programación.");
    //Tipo intervención
    if (objFiltro.TipoEvenCodi == "")
        listaMsj.push("Tipo de intervención: Seleccione una opción.");
    //Estado
    if (objFiltro.EstadoCodi == "")
        listaMsj.push("Estado: Seleccione una opción.");
    //Ubicación
    if (objFiltro.AreaCodi == "")
        listaMsj.push("Ubicación: Seleccione una opción.");
    //Tipo equipo
    if (objFiltro.FamCodi == "")
        listaMsj.push("Tipo de equipo: Seleccione una opción.");
    //Equipo
    //

    //Empresa
    if (objFiltro.Emprcodi == "")
        listaMsj.push("Empresa: Seleccione una opción.");
    //Disponibilidad
    if (objFiltro.InterIndispo == "")
        listaMsj.push("Disponibilidad: Seleccione una opción.");

    // Valida filtros de fecha cuando falta seleccionar cualquiera de ellas
    if (objFiltro.InterFechaIni == "") {
        listaMsj.push("Seleccione una fecha de inicio.");
    }
    if (objFiltro.InterFechaFin == "") {
        listaMsj.push("Seleccione una fecha de fin.");
    }

    // Valida consistencia del rango de fechas
    var interfechaini = toDate(objFiltro.InterFechaIni).toISOString();
    var interfechafin = toDate(objFiltro.InterFechaFin).toISOString();
    if (interfechaini != "" && interfechafin != "") {
        if (CompararFechas(interfechaini, interfechafin) == false) {
            listaMsj.push("La fecha de inicio no puede ser mayor que la fecha de fin.");
        }
    }

    //Reporte excel de potencia indisponible
    if (validarConjEq) {
        if (objFiltro.TipoGrupoEquipo == "0")
            listaMsj.push("Seleccione Conjunto de equipo.");
    }

    var msj = listaMsj.join('\n');
    return msj;
}

function getObjetoFiltro() {

    var tipoProgramacion = $("#TipoProgramacion") == $("#idTipoProgramacion").val() ? parseInt($('#idTipoProgramacion').val()) : parseInt($('#TipoProgramacion').val());
    //var tipoProgramacion = parseInt($('#TipoProgramacion').val()) || 0;

    var progrCodiReal = parseInt($('#Progrcodi').val()) || 0;
    var progrCodi = 0;
    if (tipoProgramacion == EVENCLASECODI_ANUAL) { //si el programa es anual entonces solo consultar el programa seleccionado, caso contrario permitir consulta para fechas distintas al programa seleccionado
        progrCodi = progrCodiReal;
    }

    var interFechaIni = $('#InterfechainiD').val();
    var interFechaFin = $('#InterfechafinD').val();

    //var areaCodi = $('#cboUbicacion').multipleSelect('getSelects');
    //if (areaCodi == "[object Object]") areaCodi = "0";

    var emprCodi = $('#Empresa').multipleSelect('getSelects').join(',');
    var tipoEvenCodi = $('#cboTipoIntervencion').multipleSelect('getSelects').join(',');
    var estadoCodi = $('#estadocodi').multipleSelect('getSelects').join(',');
    var areaCodi = $('#cboUbicacion').multipleSelect('getSelects').join(',');
    var famCodi = $('#cboFamilia').val() != null && $('#cboFamilia')[0].length == $('#cboFamilia').val().length ? "0" : $('#cboFamilia').multipleSelect('getSelects').join(',');
    //var famCodi =  $('#cboFamilia').multipleSelect('getSelects').join(',');
    var equicodi = $('#cboEquipo').val() != null && $('#cboEquipo')[0].length == $('#cboEquipo').val().length ? "0" : $('#cboEquipo').multipleSelect('getSelects').join(',');
    //var equicodi = $('#cboEquipo').multipleSelect('getSelects').join(',');

    var interIndispo = $('#InterDispo').multipleSelect('getSelects').join(',');

    var descripcion = $('#txtNombreFiltro').val();
    var estadoEliminado = getEstado();

    var estadoFiles = getEstadoFiles();
    var estadoMensaje = getCheckMensaje();
    var estadoNota = getCheckNota();
    var aprobacion = listarIntercodiChecked();

    var tipoGrupoEquipo = $('#cboConjuntoEq').val();

    var obj = {
        ProgrcodiReal: progrCodiReal,
        Progrcodi: progrCodi,
        TipoProgramacion: tipoProgramacion,

        InterFechaIni: interFechaIni,
        InterFechaFin: interFechaFin,

        Emprcodi: emprCodi,
        TipoEvenCodi: tipoEvenCodi,
        EstadoCodi: estadoCodi,
        AreaCodi: areaCodi,
        FamCodi: famCodi,
        Equicodi: equicodi,

        InterIndispo: interIndispo,

        Descripcion: descripcion,
        EstadoEliminado: estadoEliminado,
        EstadoFiles: estadoFiles,
        EstadoMensaje: estadoMensaje,
        EstadoNota: estadoNota,
        TieneValidaciones: ACTIVARVALIDACIONES,

        CheckIntercodi: aprobacion,

        TipoGrupoEquipo: tipoGrupoEquipo,
    };

    return obj;
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

function CreateForm(path, params, method = 'post') {
    var form = document.createElement('form');
    form.method = method;
    form.action = path;

    $.each(params, function (index, obj) {
        var hiddenInput = document.createElement(obj.tipo);
        hiddenInput.type = 'hidden';
        hiddenInput.name = obj.nombre;
        hiddenInput.value = obj.value;
        form.appendChild(hiddenInput);
    });
    return form;
}

//check eliminados
function getEstado() {
    var estado = "0";
    if ($('#check_eliminado').is(':checked')) {
        estado = '-1';
    }
    return estado;
}

//check Mostrar Archivos
function getEstadoFiles() {
    var estado = "0";
    if ($('#check_mostrarAdjuntos').is(':checked')) {
        estado = '1';
    }
    return estado;
}

function _mostrarMensajeAlertaTemporal(esExito, mensaje) {
    $("#alerta").hide();
    $("#alerta").show();

    if (esExito)
        $("#alerta").html(`<div class='action-exito ' style='margin: 0; padding-top: 5px; padding-bottom: 5px;'>${mensaje}</div>`);
    else
        $("#alerta").html(`<div class='action-error ' style='margin: 0; padding-top: 5px; padding-bottom: 5px;'>${mensaje}</div>`);
    setTimeout(function () { $("#alerta").fadeOut(1000) }, 2000);
}

//check mensajes
function getCheckMensaje() {
    var estado = "0";
    if ($('#check_mostrarMensajes').is(':checked')) {
        estado = '1';
    }
    return estado;
}

//check Notas
function getCheckNota() {
    var estado = "0";
    if ($('#check_mostrarNotas').is(':checked')) {
        estado = '1';
    }
    return estado;
}

//////////////////////////////////////////////////////////////////////////////////////////
// Listado - Alertas
//////////////////////////////////////////////////////////////////////////////////////////

///Mostrar Alerta No Ejecutado
function verAlertaIntervencionNoEjec() {
    limpiarAlertas();

    $('#formAlertaIntervNoEjecutada').html('El mantenimiento programado no se ha registrado como Ejecutado.');

    setTimeout(function () {
        $('#popupAlertaIntervNoEjecutada').bPopup({
            easing: 'easeOutBack',
            speed: 450,
            transition: 'slideDown',
            modalClose: true
        });
    }, 50);
}

///Mostrar Alerta Estado Pendiente
function verAlertaEstadoPendiente() {
    limpiarAlertas();

    $('#formAlertaEstadoPendiente').html('Intervencion en Estado Pendente.');

    setTimeout(function () {
        $('#popupAlertaEstadoPendiente').bPopup({
            easing: 'easeOutBack',
            speed: 450,
            transition: 'slideDown',
            modalClose: true
        });
    }, 50);
}


//Mostrar Alerta Hora de Operación
function verAlertaHOPXIntervencion(progrcodi, intercodi) {
    limpiarAlertas();

    $.ajax({
        type: 'POST',
        url: controlador + "VerAlertaHOPXIntervencion",
        data: {
            progrcodi: progrcodi,
            intercodi: intercodi
        },
        success: function (dataHtml) {
            mostrarPopupAlertaIntervencionXHOP(dataHtml);
        },
        error: function (err) {
            alert("Ha ocurrido un error");
        }
    });
}

function mostrarPopupAlertaIntervencionXHOP(dataHtml) {
    $('#formAlertaHo').html(dataHtml);

    var excep_resultado = parseInt($("#hdResultado").val()) || 0;
    var excep_mensaje = $("#hdMensaje").val();
    var excep_detalle = $("#hdDetalle").val();

    var interindispo = $("#hdInterindispo").val();
    var htmlTitulo = 'Alerta de Intervención F/S con Horas de Operación';
    var widthHtml = '750px';
    if (interindispo === 'E' || interindispo === 'E/S') {
        htmlTitulo = 'Alerta de Intervención E/S sin Horas de Operación';
        widthHtml = '365px';
    }

    $("#popupAlertaHo .popup-title span").html(htmlTitulo);
    $("#popupAlertaHo").css('width', widthHtml);

    if (excep_resultado === -1) {
        alert(excep_mensaje);
    } else {
        setTimeout(function () {
            $('#popupAlertaHo').bPopup({
                easing: 'easeOutBack',
                speed: 450,
                transition: 'slideDown',
                modalClose: true
            });
        }, 50);
    }
}

//Mostrar Alerta Scada
function verAlertaScadaXIntervencion(progrcodi, intercodi) {
    limpiarAlertas();

    $.ajax({
        type: 'POST',
        url: controlador + "VerAlertaScadaXIntervencion",
        data: {
            progrcodi: progrcodi,
            intercodi: intercodi
        },
        success: function (dataHtml) {
            mostrarPopupAlertaIntervencionXScada(dataHtml);
        },
        error: function (err) {
            alert("Ha ocurrido un error");
        }
    });
}

function mostrarPopupAlertaIntervencionXScada(dataHtml) {
    $('#formAlertaScada').html(dataHtml);

    var excep_resultado = parseInt($("#hdResultado").val()) || 0;
    var excep_mensaje = $("#hdMensaje").val();
    var excep_detalle = $("#hdDetalle").val();

    var interindispo = $("#hdInterindispo").val();
    var htmlTitulo = 'Alerta de Intervención F/S con Señales Scada';
    var widthHtml = '750px';
    if (interindispo === 'E' || interindispo === 'E/S') {
        htmlTitulo = 'Alerta de Intervención E/S sin Señales Scada';
        widthHtml = '750px';
    }

    $("#popupAlertaScada .popup-title span").html(htmlTitulo);
    $("#popupAlertaScada").css('width', widthHtml);

    if (excep_resultado === -1) {
        alert(excep_mensaje);
    } else {
        setTimeout(function () {
            $('#popupAlertaScada').bPopup({
                easing: 'easeOutBack',
                speed: 450,
                transition: 'slideDown',
                modalClose: true
            });
        }, 50);
    }
}

//Mostrar Alerta Ems
function verAlertaEmsXIntervencion(progrcodi, intercodi) {
    limpiarAlertas();

    $.ajax({
        type: 'POST',
        url: controlador + "VerAlertaEmsXIntervencion",
        data: {
            progrcodi: progrcodi,
            intercodi: intercodi
        },
        success: function (dataHtml) {
            mostrarPopupAlertaIntervencionXEms(dataHtml);
        },
        error: function (err) {
            alert("Ha ocurrido un error");
        }
    });
}

function mostrarPopupAlertaIntervencionXEms(dataHtml) {
    $('#formAlertaEms').html(dataHtml);

    var excep_resultado = parseInt($("#hdResultado").val()) || 0;
    var excep_mensaje = $("#hdMensaje").val();
    var excep_detalle = $("#hdDetalle").val();

    var interindispo = $("#hdInterindispo").val();
    var htmlTitulo = 'Alerta de Intervención F/S con Señales estimadas del EMS';
    var widthHtml = '750px';
    if (interindispo === 'E' || interindispo === 'E/S') {
        htmlTitulo = 'Alerta de Intervención E/S sin Señales estimadas del EMS';
        widthHtml = '750px';
    }

    $("#popupAlertaEms .popup-title span").html(htmlTitulo);
    $("#popupAlertaEms").css('width', widthHtml);

    if (excep_resultado === -1) {
        alert(excep_mensaje);
    } else {
        setTimeout(function () {
            $('#popupAlertaEms').bPopup({
                easing: 'easeOutBack',
                speed: 450,
                transition: 'slideDown',
                modalClose: true
            });
        }, 50);
    }
}

//Mostrar Alerta IDCC
function verAlertaIDCCXIntervencion(progrcodi, intercodi) {
    limpiarAlertas();

    $.ajax({
        type: 'POST',
        url: controlador + "VerAlertaIDCCXIntervencion",
        data: {
            progrcodi: progrcodi,
            intercodi: intercodi
        },
        success: function (dataHtml) {
            mostrarPopupAlertaIntervencionXIDCC(dataHtml);
        },
        error: function (err) {
            alert("Ha ocurrido un error");
        }
    });
}

function mostrarPopupAlertaIntervencionXIDCC(dataHtml) {
    $('#formAlertaIDCC').html(dataHtml);

    var excep_resultado = parseInt($("#hdResultado").val()) || 0;
    var excep_mensaje = $("#hdMensaje").val();
    var excep_detalle = $("#hdDetalle").val();

    var interindispo = $("#hdInterindispo").val();
    var htmlTitulo = 'Alerta de Intervención F/S con datos IDCC';
    var widthHtml = '750px';
    if (interindispo === 'E' || interindispo === 'E/S') {
        htmlTitulo = 'Alerta de Intervención E/S sin datos IDCC';
        widthHtml = '750px';
    }

    $("#popupAlertaIDCC .popup-title span").html(htmlTitulo);
    $("#popupAlertaIDCC").css('width', widthHtml);

    if (excep_resultado === -1) {
        alert(excep_mensaje);
    } else {
        setTimeout(function () {
            $('#popupAlertaIDCC').bPopup({
                easing: 'easeOutBack',
                speed: 450,
                transition: 'slideDown',
                modalClose: true
            });
        }, 50);
    }
}

//Mostrar Alerta PR21
function verAlertaPR21XIntervencion(progrcodi, intercodi) {
    limpiarAlertas();

    $.ajax({
        type: 'POST',
        url: controlador + "VerAlertaPR21XIntervencion",
        data: {
            progrcodi: progrcodi,
            intercodi: intercodi
        },
        success: function (dataHtml) {
            mostrarPopupAlertaIntervencionXPR21(dataHtml);
        },
        error: function (err) {
            alert("Ha ocurrido un error");
        }
    });
}

function mostrarPopupAlertaIntervencionXPR21(dataHtml) {
    $('#formAlertaPR21').html(dataHtml);

    var excep_resultado = parseInt($("#hdResultado").val()) || 0;
    var excep_mensaje = $("#hdMensaje").val();
    var excep_detalle = $("#hdDetalle").val();

    var interindispo = $("#hdInterindispo").val();
    var htmlTitulo = 'Alerta de Intervención F/S con datos PR21';
    var widthHtml = '750px';
    if (interindispo === 'E' || interindispo === 'E/S') {
        htmlTitulo = 'Alerta de Intervención E/S sin datos PR21';
        widthHtml = '750px';
    }

    $("#popupAlertaPR21 .popup-title span").html(htmlTitulo);
    $("#popupAlertaPR21").css('width', widthHtml);

    if (excep_resultado === -1) {
        alert(excep_mensaje);
    } else {
        setTimeout(function () {
            $('#popupAlertaPR21').bPopup({
                easing: 'easeOutBack',
                speed: 450,
                transition: 'slideDown',
                modalClose: true
            });
        }, 50);
    }
}


//Mostrar Alerta Medidores
function verAlertaMedidoresXIntervencion(progrcodi, intercodi) {
    limpiarAlertas();

    $.ajax({
        type: 'POST',
        url: controlador + "VerAlertaMedidoresXIntervencion",
        data: {
            progrcodi: progrcodi,
            intercodi: intercodi
        },
        success: function (dataHtml) {
            mostrarPopupAlertaIntervencionXMedidores(dataHtml);
        },
        error: function (err) {
            alert("Ha ocurrido un error");
        }
    });
}

function mostrarPopupAlertaIntervencionXMedidores(dataHtml) {
    $('#formAlertaMedidores').html(dataHtml);

    var excep_resultado = parseInt($("#hdResultado").val()) || 0;
    var excep_mensaje = $("#hdMensaje").val();
    var excep_detalle = $("#hdDetalle").val();

    var interindispo = $("#hdInterindispo").val();
    var htmlTitulo = 'Alerta de Intervención F/S con datos de Medidores';
    var widthHtml = '750px';
    if (interindispo === 'E' || interindispo === 'E/S') {
        htmlTitulo = 'Alerta de Intervención E/S sin datos de Medidores';
        widthHtml = '750px';
    }

    $("#popupAlertaMedidores .popup-title span").html(htmlTitulo);
    $("#popupAlertaMedidores").css('width', widthHtml);

    if (excep_resultado === -1) {
        alert(excep_mensaje);
    } else {
        setTimeout(function () {
            $('#popupAlertaMedidores').bPopup({
                easing: 'easeOutBack',
                speed: 450,
                transition: 'slideDown',
                modalClose: true
            });
        }, 50);
    }
}

function limpiarAlertas() {
    $('#formAlertaHo').html('');
    $('#formAlertaScada').html('');
    $('#formAlertaEms').html('');
    $('#formAlerta').html('');
    $('#formAlertaPR21').html('');
    $('#formAlertaMedidores').html('');
}

function _mostrarAlertaEmpresas(model) {
    $("#alertaEmpresas").show();

    var cuerpo = _dibujarTablaEmpresasAbrev(model);
    $('#alertaEmpresas').html(cuerpo);

    //$("#alertaEmpresas").html(`<div class='action-alert ></div>`);
    //setTimeout(function () { $("#alerta").fadeOut(1000) }, 2000);

    // eventos
    //$('#btnActualizarAbrev').click(function () {
    //    var idEmpresa = $("#").val();
    //    var abreviatura = $("#").val();
    //    actualizarAbreviaturaEmpresa(idEmpresa, abreviatura);
    //});
}

function _dibujarTablaEmpresasAbrev(model) {
    var lista = model.ListaEmpresasValidate;
    var cadena = '';
    cadena += `No existe abreviatura para las siguientes empresas:
    <table id="tablaAlertaEmprsas" border="1" class="pretty tabla-adicional" cellspacing="0" style="width: auto;">
        <thead>
            <tr>
                <th>Código de empresa</th>
                <th>Nombre de empresa</th>
                <th>Abreviatura</th>
                <th></th>
            </tr>
        </thead>
        <tbody>
    `;

    for (key in lista) {
        var item = lista[key];
        //
        var sdisabled = "";
        var sStyle = "";
        cadena += `

            <tr id="fila_${item.Emprcodi}">
                <td style="text-align:center; ${sStyle}">${item.Emprcodi}</td>
                <td style="text-align:left; ${sStyle}">${item.EmprNomb}</td>
                <td style="padding-top: 0px;">
                    <input type="text" id="${item.Emprcodi}_abrev" value="" />
                </td>
                <td>
                    <input type="button" onclick="actualizarAbreviaturaEmpresa(${item.Emprcodi});" id="btnActualizarAbrev" value="Actualizar abreviatura">
                </td>
            </tr>
        `;
    }
    cadena += "</tbody></table>";

    return cadena;
}

//guardar abreviatura
function actualizarAbreviaturaEmpresa(idEmpresa) {

    var idAbrev = "#" + idEmpresa.toString() + "_abrev";
    var abreviatura = $(idAbrev).val();
    var codprograma = parseInt($('#Progrcodi').val()) || 0;

    if (confirm('¿Desea actualizar el registro?')) {

        var msj = "";
        if (abreviatura == null || abreviatura == '') {
            msj += "La abreviatura debe tener al menos un caracter";
        }
        else {
            if (abreviatura.length > 6) {
                msj += "La abreviatura no puede tener mas de 6 caracteres";
            }
        }

        if (msj == "") {
            $.ajax({
                type: 'POST',
                dataType: 'json',
                traditional: true,
                url: controler + "ActualizarAbreviaturaEmpresa",
                data: {
                    emprcodi: idEmpresa,
                    emprabrev: abreviatura,
                    progrcodi: codprograma
                },
                cache: false,
                success: function (result) {
                    if (result.Resultado != '-1') {
                        if (result.Resultado == "0") {
                            alert("La abreviatura debe ser único");
                        }
                        else {
                            alert("Se guardó correctamente el registro");
                            mostrarLista();
                        }
                    } else {
                        alert(result.Mensaje);
                    }
                },
                error: function (err) {
                    alert('Ha ocurrido un error.');
                }
            });
        } else {
            alert(msj);
        }
    }
}

//Realizar recordatorio
function ejecutarRecordatorioInclusionExclusion() {
    var idPrograma = $("#Progrcodi").val();

    $.ajax({
        url: controladorParametro + "EjecutarRecordatorioInclusionExclusion",
        type: 'POST',
        data: {
            progrcodi: idPrograma
        },
        dataType: 'json',
        success: function (model) {
            if (model.Resultado !== '-1') {
                alert("Se ha ejecutado correctamente el proceso.");
            } else {
                alert("Ha ocurrido un error: " + model.Mensaje);
            }
        },
        error: function (result) {
            alert("Ha ocurrido un error.");
        }
    });
}

//Realizar recordatorio
function ejecutarAprobacionAutomatica() {
    var idPrograma = $("#Progrcodi").val();

    $.ajax({
        url: controladorParametro + "EjecutarAprobacionAutomatica",
        type: 'POST',
        data: {
            progrcodi: idPrograma
        },
        dataType: 'json',
        success: function (model) {
            if (model.Resultado !== '-1') {

                if (model.Resultado == '1') {
                    alert("Se ha ejecutado correctamente el proceso.");
                    location.reload(); //actualizar toda la pantalla
                }

                if (model.Resultado == '2')
                    alert("No se realizó la aprobación, Se envío notificación con los errores.");

                if (model.Resultado == '0')
                    alert("No existen registros válidos para aprobar.");

            } else {
                alert("Ha ocurrido un error: " + model.Mensaje);
            }
        },
        error: function (result) {
            alert("Ha ocurrido un error.");
        }
    });
}

/**
 * Comunicaciones
 * */
function abrirPopupComunicaciones(interCodi) {
    $("#hfIntercodiMsj").val(interCodi);

    listarMensajes(true);
}

var LISTA_MENSAJE_X_INT = [];

function listarMensajes(abrirPopup) {
    var interCodi = $("#hfIntercodiMsj").val();
    var tipoRemitente = $("#ddl-sender").val();
    var estadoMensaje = $("#ddl-state").val();

    $("#div-comments").show();
    $("#div-msg-contenido").hide();
    $("#lst-comments").html('');
    $('#frmRegistroMensajePopup').attr('src', '');
    $('#frmRegistroMensajePopup').hide();

    ARRAY_FILES_MENSAJE = [];

    $.ajax({
        type: 'POST',
        url: controlador + "ListadoMensaje",
        dataType: 'json',
        data: {
            interCodi: interCodi,
            tipoRemitente: tipoRemitente,
            estadoMensaje: estadoMensaje
        },
        success: function (evt) {
            if (evt.Resultado != "-1") {
                LISTA_MENSAJE_X_INT = evt.ListaMensajes;

                $('#lst-comments').html(dibujarTablaMensajes(LISTA_MENSAJE_X_INT));

                if (abrirPopup) {
                    setTimeout(function () {
                        $('#popupMensajes').bPopup({
                            easing: 'easeOutBack',
                            speed: 450,
                            transition: 'slideDown',
                            modalClose: false
                        });
                    }, 50);
                }

            } else {
                alert(evt.StrMensaje);
            }
        },
        error: function (err) {
            alert("Ha ocurrido un error");
        }
    });
}

function dibujarTablaMensajes(lista) {
    var htmlListado = '';

    if (lista != null && lista.length > 0) {
        for (key in lista) {
            var reg = lista[key];

            var htmlCC = reg.Msgcc != null && reg.Msgcc != '' ? `CC: ${reg.Msgcc}` : "";

            var estiloLeido = '';
            if (reg.EsLeido) estiloLeido = `background-color: #EDF5FC;`;

            var estiloArchivo = '';
            if (reg.Msgflagadj == 1) {
                estiloArchivo = `background-image: url(${siteRoot}Content/Images/attachments.png); `;
            }

            var htmlLectura = '';
            if (reg.EsLeido) {
                var textoLeido = '';
                if (reg.FechaDescLectura != null && reg.FechaDescLectura != "") textoLeido = `Leído por ${reg.UsuarioLectura} en ${reg.FechaDescLectura}`;

                htmlLectura = `
                    <div style="font-style: italic;text-align: right; color: green;height: 24px; background-image: url(${siteRoot}Content/Images/comment-seen.png); 
                                background-position: right center; background-repeat: no-repeat;padding-right: 34px;">
                        ${textoLeido}
                    </div>
                `;
            } else {
                htmlLectura = `
                    <div style="font-style: italic;text-align: right; color: red;">
                        [No leído]
                    </div>
                `;
            }

            htmlListado += `
                <section onclick="verContenidoMensaje(${reg.Msgcodi});" style='cursor: pointer; ${estiloLeido}'>

                    <div style='display: inline-block;'>
                        <div style='width:895px; float: left;font-weight: bold; margin-bottom: 5px; font-size: 14px;' >
                            ${reg.Remitente} (${reg.Msgfrom})
                        </div>
                        <div style='width:120px; float: right; text-align: right' >
                            ${reg.MsgfeccreacionDesc}
                        </div>
                        <div style='width:30px; float: right; height: 24px; ${estiloArchivo}; background-position: right center; background-repeat: no-repeat;' >                            
                        </div>
                    </div>
                    <div>
                        <div>Para: ${reg.Msgto}</div>
                        <div style='word-break: break-word;'>${htmlCC}</div>
                    </div>

                    <div style='display: inline-block;padding-top: 8px;'>
                        <div style='width:1050px; float: left;' >
                            Asunto: ${reg.Msgasunto}
                        </div>
                    </div>
                    <div style="height: 24px;">
                        ${htmlLectura}
                    </div>
                </section>
        `;
        }
    }

    return htmlListado;
}

async function verContenidoMensaje(msgcodi) {

    $("#div-comments").hide();
    $("#div-msg-contenido").show();
    $("#div-detalle-msg-contenido").html('');

    var reg = buscarMensaje(msgcodi, LISTA_MENSAJE_X_INT);
    if (reg != null) {
        var interCodi = ($("#hfIntercodiMsj").val()).trim();

        //si el mensaje es del agente y no está leído
        if (reg.Remitente == "AGENTE" && reg.Msgestado == "N") {
            await marcarComoLeido(interCodi, reg.Msgcodi);
        }

        var htmlCC = reg.Msgcc != null && reg.Msgcc != '' ? `CC: ${reg.Msgcc}` : "";

        var html = `
            <section>
                <div style='display: inline-block;'>
                    <div style='width:1050px; float: left;FONT-SIZE: 18px;' >
                        ${reg.Msgasunto}
                    </div>
                    <div style='width:50px; float: right; text-align: right' >
                        <img src="${siteRoot}Content/Images/btn-regresar.png" style="width:20px; height:20px;cursor: pointer;" title="Regresar a listado de comunicaciones" onclick="volverAListadoMensaje();" />
                    </div>
                </div>

                <div class='linea_division_mensaje'></div>
                
                <div style='display: inline-block;'>
                    <div style='width:850px; float: left;font-weight: bold; margin-bottom: 5px; font-size: 14px;' >
                        ${reg.Remitente} (${reg.Msgfrom})
                    </div>
                    <div style='width:150px; float: right; text-align: right' >
                        ${reg.MsgfeccreacionDesc}
                    </div>
                </div>
                <div>
                    <div>Para: ${reg.Msgto}</div>
                    <div style='word-break: break-word;'>${htmlCC}</div>
                </div>

                <div class='linea_division_mensaje'></div>

                <div style='padding: 10px; border: 1px solid #dddddd; min-height: 150px;'>
                    ${reg.Msgcontenido}
                </div>

                <div>
                    <div id="html_listaArchivosMensaje">
                    </div>

                    <div id="listaArchivos2">
                        <iframe id="vistaprevia_contenido_mensaje" style="width: 100%; height:500px;" frameborder="0" hidden></iframe>
                    </div>
                </div>
            </section>
        `;

        $("#div-detalle-msg-contenido").html(html);

        //si tiene archivos, entonces mostrarlos con opción de vista previa
        LISTA_SECCION_ARCHIVO_X_MENSAJE = [];
        if (reg.Msgflagadj == 1) {

            var seccion = {
                Inpstidesc: 'Archivos adjuntos',
                EsEditable: false,
                ListaArchivo: reg.ListaArchivo,
                Modulo: TIPO_MODULO_MENSAJE,
                Progrcodi: reg.Progrcodi,
                Carpetafiles: reg.Msgcodi,
                Subcarpetafiles: 0,
                TipoArchivo: TIPO_ARCHIVO_MENSAJE,
                IdDiv: `html_listaArchivosMensaje`,
                IdDivVistaPrevia: 'vistaprevia_contenido_mensaje',
                IdPrefijo: arch_getIdPrefijo(0)
            };

            LISTA_SECCION_ARCHIVO_X_MENSAJE.push(seccion);

            arch_cargarHtmlArchivoEnPrograma(seccion.IdDiv, seccion);
        }
    }
}

function volverAListadoMensaje() {
    listarMensajes(false);
}

function buscarMensaje(codigo, lista) {
    if (lista.length > 0) {
        for (var i = 0; i < lista.length; i++) {
            if (lista[i].Msgcodi == codigo) {
                return lista[i];
            }
        }
    }

    return null;
}

async function marcarComoLeido(intercodi, msgcodi) {

    return $.ajax({
        type: 'POST',
        url: controlador + "MarcarMensajeLeido",
        dataType: 'json',
        data: {
            interCodi: intercodi,
            msgcodi: msgcodi
        },
        success: function (evt) {
            if (evt.Resultado != "-1") {

            } else {
                alert("Ha ocurrido un error: " + evt.StrMensaje);
            }
        },
        error: function (err) {
            alert("Ha ocurrido un error");
        }
    });
}

/**
 * Reporte comunicaciones
 * */

function generarIntervencionesRptComunicaciones() {
    var intercodis = listarIntercodiChecked();

    if (intercodis == "") {
        alert("Debe seleccionar una o varias intervenciones.");
    } else {
        $.ajax({
            type: 'POST',
            url: controler + 'DescargarReporteComunicacionSeleccionados',
            data: {
                intercodis: intercodis
            },
            dataType: 'json',
            success: function (evt) {
                if (evt.Resultado != "-1") {
                    var paramList = [
                        { tipo: 'input', nombre: 'file', value: evt.NombreArchivo }
                    ];
                    var form = CreateForm(controler + 'abrirarchivo', paramList);
                    document.body.appendChild(form);
                    form.submit();
                } else {
                    alert("Ha ocurrido un error: " + evt.Mensaje);
                }
            },
            error: function (err) {
                alert('Ha ocurrido un error');
            }
        });
    }
}

/**
 * Opciones de comunicaciones
 * */
function enviarRespuestaComunicacion() {
    $('#frmRegistroMensajePopup').attr('src', '');
    $('#frmRegistroMensajePopup').hide();

    var progrcodi = document.getElementById('Progrcodi').value;
    var tipoProgramacion = document.getElementById('idTipoProgramacion').value;
    var intercodis = ($("#hfIntercodiMsj").val()).trim();

    var url = controlador + 'IntervencionesMensajeRegistro' +
        '?intercodis=' + intercodis +
        '&progrcodi=' + progrcodi +
        '&evenclasecodi=' + tipoProgramacion +
        '&origen=' + 'REGISTRO';

    $('#frmRegistroMensajePopup').attr('src', url);
    $('#frmRegistroMensajePopup').show();
}

function descargarPdfComunicacion() {
    var interCodi = ($("#hfIntercodiMsj").val()).trim();

    if (LISTA_MENSAJE_X_INT.length > 0) {
        window.location.href = controlador + `DownloadFilePdfListadoMensaje?interCodi=${interCodi}`;
    } else {
        alert("No existe mensajes de la intervención seleccionada.");
    }
}

function descargarZipComunicacion() {
    var interCodi = ($("#hfIntercodiMsj").val()).trim();

    $.ajax({
        type: 'POST',
        url: controler + 'DescargarZipXMensaje',
        data: { interCodi: interCodi },
        success: function (evt) {
            if (evt.Resultado != "-1") {

                window.location = controler + "ExportarZip?file_name=" + evt.Resultado;

            } else {
                alert("Ha ocurrido un error: " + evt.Mensaje);
            }
        },
        error: function (err) {
            alert("Error al descargar archivos");
        }
    });
}

/**
 * história de un equipo
 * */
function abrirPopupHistoriaEquipo(equicodi) {
    $('#popupFormHistoria').html('');

    var objParam = {
        equicodi: equicodi,
        progrCodi: 0,
        tipoProgramacion: 0,
        escruzadas: false,
    };

    //IntervencionesHistoria.js
    formularioHistoria(objParam);
}

/**
 * Tacometro F1 y F2
 * */
function generarDashboard(horizonte) {

    var fechaPeriodo = $("#InterfechainiD").val();
    $.ajax({
        url: controlador + "ConstruirDashboardFiltro",
        data: {
            fecha: fechaPeriodo,
            horizonte: horizonte
        },
        type: 'POST',
        success: function (result) {

            if (result.Graficos.length > 0) {
                graficoTacometro(result.Graficos[0], 'tacometroF1');
                graficoTacometro(result.Graficos[1], 'tacometroF2');
            }
            else {
                alert("Error, no se generó versión para la fecha seleccionada");
            }
        },
        error: function (xhr, status) {
        },
        complete: function (xhr, status) {
        }
    });
};

function graficoTacometro(dataResult, content) {
    //var data = dataResult.Grafico;
    var data = dataResult;

    //console.log("Gráfico");
    //console.log(data);

    if (data.PlotBands.length < 1) {
        return;
    }
    var dataPlot = [];
    for (var i in data.PlotBands) {
        var item = data.PlotBands[i];
        if (item === null) {
            continue;
        }
        dataPlot.push({ from: item.From, to: item.To, color: item.Color, thickness: item.Thickness });
    }

    var series = [];
    for (var d in data.SerieData) {
        item = data.SerieData[d];
        var align = (d % 2 === 0) ? "right" : "left";
        series.push({
            name: item.Name,
            color: item.Color,
            data: item.Data,
            tooltip: {
                valueSuffix: ' %'
            },
            dial: {
                backgroundColor: item.Color
            },
            showInLegend: true,
            dataLabels: {
                align: "center",
                enabled: true,
                color: item.Color,
                allowOverlap: false,
                allowOverlap: true
            }
        });
    }

    Highcharts.chart(content, {

        chart: {
            type: 'gauge',
            backgroundColor: null,
            plotBorderWidth: null,
            marginTop: 4,
            marginBottom: -2,
            marginLeft: 0,
            plotShadow: false,
            borderWidth: 0,
            plotBorderWidth: 0,
            marginRight: 0
        },
        tooltip: {
            userHTML: true,
            style: {
                padding: 0,
                width: 0,
                height: 0,
            },
            formatter: function () {
                return this.point.residents;
            },
        },
        title: {
            text: ''
        },
        pane: {
            startAngle: -90,
            endAngle: 90,
            background: null
        },
        xAxis: {
            enabled: false,
            showEmpty: false,
        },
        yAxis: {
            min: data.YaxixMin,
            max: data.YaxixMax,
            lineColor: 'transparent',
            minorTickWidth: 0,
            tickLength: 0,
            tickPositions: data.YaxixTickPositions,
            labels: {
                step: 1,
                distance: 10
            },
            plotBands: dataPlot
        },
        credits: {
            enabled: false
        },
        legend: {
            align: 'center',
            verticalAlign: 'bottom'
        },

        series: series

    });


};

//Crear nueva versión Factores
function crearVersionF1F2() {
    var evenclasecodi = $("#idTipoProgramacion").val();
    var fechaPeriodo = $("#InterfechainiD").val();

    $.ajax({
        type: 'POST',
        async: true,
        url: controler + 'GuardarNuevaVersion',
        data: {
            fechaPeriodo: fechaPeriodo,
            horizonte: evenclasecodi
        },
        success: function (result) {
            if (result.Resultado == "-1") {
                alert(result.Mensaje);
            } else {
                alert("Se creó correctamente la versión");
                mostrarLista();
            }
        },
        error: function (err) {
            alert("Ha ocurrido un error");
        }
    });
}