var controler = siteRoot + "Intervenciones/Registro/";

var EVENCLASECODI_ANUAL = 5;
var EVENCLASECODI_SEMANAL = 3;
var EVENCLASECODI_DIARIO = 2;
var EVENCLASECODI_EJECUTADO = 1;

var data = [];
var dataCodigo = [];
var fechaIniProg = null;
var fechaFinProg = null;

$(document).ready(function ($) {
    $("#btnOcultarMenu").click();

    // Acciones barra de herramientas
    $('#btnNuevo').click(function () {
        nuevaIntervencionCruzada();
    });

    $('#btnExportarExcel').click(function () {
        exportarExcelIntervencionesCruzadas(1);
    });

    $('#btnExportarExcelIndisp').click(function () {
        exportarExcelIntervencionesCruzadas(2);
    });

    $('#btnReporteActividades').click(function () {
        reporteActividades();
    });

    //acciones de consulta
    $('.txtFecha').inputmask({
        mask: "1/2/y",
        placeholder: "dd/mm/yyyy",
        alias: "datetime"
    });

    $('.txtFecha').Zebra_DatePicker({
        readonly_element: false
    });

    $('#cboTipoEventoFiltro, #cboCausaFiltro, #cboDisponibilidad, #estadocodi').multipleSelect({
        selectAll: true,
        filter: true,
        placeholder: "SELECCIONE"
    });

    $('#cboClaseProgramacionFiltro, #cboManiobras').multipleSelect({
        selectAll: true,
        filter: true,
        placeholder: "SELECCIONE"
    });

    $('#cboEmpresaFiltro').multipleSelect({
        // placeholder: "--Todos--",
        filter: true,
        placeholder: "SELECCIONE"
        //single: true,
    });

    $('#cboTipoEventoFiltro, #cboCausaFiltro, #cboDisponibilidad').multipleSelect('checkAll');
    $('#cboClaseProgramacionFiltro, #cboManiobras').multipleSelect('checkAll');
    $('#cboEmpresaFiltro').multipleSelect('checkAll');
    $("#estadocodi").multipleSelect('checkAll');

    $('#btnConsultar').click(function () {
        mostrarGrillaExcel()
    });

    $('#btnSelectFecha').click(function () {
        var objConf = {
            idPopup: '#popupSeleccionarFecha',
            idFechaInicio: '#Entidad_Interfechaini',
            idFechaFin: '#Entidad_Interfechafin',
            titulo: 'Seleccionar fechas'
        }
        selectDate_mostrarPopup(objConf);
    });

    $(".check_mostrarAdjuntos").on("click", function () {
        mostrarGrillaExcel();
    });

    $(".check_mostrarNotas").on("click", function () {
        mostrarGrillaExcel();
    });

    $('#btnProcedimientosManiobras').click(function () {
        if (typeof hot !== 'undefined') {
            descargarProcedimientosManiobras();
        } else {
            alert('No es posible descargar maniobras , Realize la consulta');
        }
    });

    $('#btnReporteNTIITR').click(function () {
        descargarReporteNTIITR();
    });

    $('#btnIrEquivalencia').on('click', function () {
        var url = siteRoot + 'IEOD/Configuracion/EquivalenciaEquipoScada/';
        window.open(url, '_blank').focus();
    });

    $('#btnContraer').click(function (e) {
        $('#Contenido').toggle();
        $(this).css("display", "none");
        $('#btnDescontraer').css("display", "block");
        //asignar tamaño de handson
        ocultar = 1;

        $("#listado").hide();
        var nuevoTamanioTabla = getHeightTablaListado();
        document.getElementById('grillaExcel').style.height = nuevoTamanioTabla + "px";
        $("#listado").show();
        updateDimensionHandson(hot);
    });

    $('#btnDescontraer').click(function (e) {
        $('#Contenido').slideDown();
        $(this).css("display", "none");
        $('#btnContraer').css("display", "block");
        ocultar = 0;

        $("#grillaExcel").hide();
        var nuevoTamanioTabla = getHeightTablaListado();
        document.getElementById('grillaExcel').style.height = nuevoTamanioTabla + 'px';
        $("#grillaExcel").show();
        updateDimensionHandson(hot);
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

    // Valida HorasIndispo
    _mostrarH48Indisp();
    $('#cboFamilias').on("change", function () {
        _mostrarH48Indisp();
    });
});

function mostrarGrillaExcel(loadData) {
    //si el popup de mensajes está visible entonces cerrarlo
    $('#popupMensajes').bPopup().close();

    var objData = getObjetoFiltro();
    var msj = validarConsulta(objData);

    if (msj == "") {
        $.ajax({
            type: 'POST',
            url: controler + "GrillaExcel",
            data: objData,
            dataType: 'json',
            success: function (result) {
                if (result.Resultado != "-1") {
                    if (objData.TipoProgramacion > 5 || (objData.InterFechaIni != fechaIniProg) || (objData.InterFechaFin != fechaFinProg)) {
                        $("#btnReporteActividades").hide();
                    }
                    else {
                        $("#btnReporteActividades").show();
                    }

                    if (objData.TipoProgramacion == 1 || objData.TipoProgramacion == 2 || objData.TipoProgramacion == 3) {
                        $("#tacometroF1").show();
                        $("#tacometroF2").show();
                        generarDashboard(objData.TipoProgramacion);
                    }
                    else {
                        $("#tacometroF1").hide();
                        $("#tacometroF2").hide();
                    }

                    //completar información
                    ObtenerDataGridCruzada(result.GridExcel.ListaFecha, result.GridExcel.ListaEq);
                    result.GridExcel.Data = data;
                    result.GridExcel.DataCodigo = dataCodigo;

                    if (loadData) {
                        refrescarCeldasModificadas(result.GridExcel);
                    } else {
                        $("#grillaExcel").hide();
                        $("#alerta").hide();

                        if (result.Resultado == '-1') {
                            $("#grillaExcel").hide();
                            alert(result.Mensaje);
                            return;
                        }

                        MODELO_GRID = result.GridExcel;
                        generarHoTweb();
                    }

                } else {
                    alert(result.StrMensaje);
                }
            },
            error: function (err) {
                alert('Lo sentimos no se puede mostrar la consulta . *Revise que el rango de fechas no debe de sobrepasar el año')
            }
        });
    } else {
        alert(msj);
    }
}

function ObtenerDataGridCruzada(listaAllFecha, listaAllEq) {
    // Se arma la matriz de datos
    var listaFila = [];
    var listaFilaCodigo = [];

    var row = 0;
    for (var i = 0; i < listaAllEq.length; i++) {
        var regEq = listaAllEq[i];

        var listaCelda = [];
        var listaCeldaCodigo = [];

        listaCelda.push(regEq.EmprNomb);
        listaCelda.push(regEq.AreaNomb);
        listaCelda.push(regEq.FamAbrev);
        listaCelda.push(regEq.EquiNomb);
        listaCeldaCodigo.push(regEq.Emprcodi.toString());
        listaCeldaCodigo.push(regEq.Areacodi.toString());
        listaCeldaCodigo.push(regEq.Equicodi.toString());

        var col = listaCelda.length;
        for (var j = 0; j < listaAllFecha.length; j++) {
            var regDia = listaAllFecha[j];

            var htmlDiaEq = "";

            var objDiaXEq = regEq.ListaEqDia.find((x) => x.Dia == regDia.Dia);
            if (objDiaXEq != null) {

                var listadoFormat = objDiaXEq.ListaDato.map((x) => format("{5} <a href='#' content='{8}, {9}, {10}, {11}, {12}, {13}, {14}, {15}, {16}, {17}, {18}, {19}, {20}' class='intervencion intercodi_{1} {0}' title='{2}'>{3}-{4}  {7}</a>{6}{21}{22}"
                    , x.CeldaClase, x.Intercodi, x.Title, x.CeldaHoraIni, x.CeldaHoraFin, x.CeldaHorizonte, (x.TieneArchivo ? "<span class='cruz_file'>*</span>" : ""),
                    x.VistoBueno, x.Tipoevenabrev, x.EstadoRegistro, x.Operadornomb, x.Famabrev, x.InterfechainiDesc, x.InterfechafinDesc, x.InterindispoDesc
                    , x.InterinterrupDesc, x.InterconexionprovDesc, x.IntersistemaaisladoDesc, x.UltimaModificacionUsuarioDesc, x.UltimaModificacionFechaDesc, x.Interdescrip, (x.TieneNota ? "<span class='cruz_file'>!</span>" : ""), (x.Interflagsustento != 1 ? "" : "<span class='cruz_file'>#</span>")));

                htmlDiaEq = listadoFormat.join(" <br/> ");
            }

            listaCelda.push(htmlDiaEq);
            col++;
        }

        listaCelda.push(regEq.HorasIndispXEq.toString());
        row++;

        listaFila.push(listaCelda);
        listaFilaCodigo.push(listaCeldaCodigo);
    }

    data = listaFila;
    dataCodigo = listaFilaCodigo;
}

function format(fmt, ...args) {
    if (!fmt.match(/^(?:(?:(?:[^{}]|(?:\{\{)|(?:\}\}))+)|(?:\{[0-9]+\}))+$/)) {
        throw new Error('invalid format string.');
    }
    return fmt.replace(/((?:[^{}]|(?:\{\{)|(?:\}\}))+)|(?:\{([0-9]+)\})/g, (m, str, index) => {
        if (str) {
            return str.replace(/(?:{{)|(?:}})/g, m => m[0]);
        } else {
            if (index >= args.length) {
                throw new Error('argument index is out of range in format');
            }
            return args[index];
        }
    });
}

function exportarExcelIntervencionesCruzadas(tipoReporte) {
    var objData = getObjetoFiltro();
    objData.TipoReporte = tipoReporte;

    var msj = validarConsulta(objData);

    if (msj == "") {
        $.ajax({
            type: 'POST',
            url: controler + 'ExportarIntervencionesCruzadas',
            dataType: 'json',
            data: objData,
            success: function (result) {
                if (result.Resultado != "-1") {
                    var paramList = [
                        { tipo: 'input', nombre: 'file', value: result.NombreArchivo }
                    ];
                    var form = CreateForm(controler + 'AbrirArchivo', paramList);
                    document.body.appendChild(form);
                    form.submit();
                } else {
                    alert(result.Mensaje);
                }
            },
            error: function (err) {
                alert('Lo sentimos no se puede mostrar la consulta . *Revise que el rango de fechas no debe de sobrepasar el año')
            }
        });
    } else {
        alert(msj);
    }
}

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

function causasFiltro() {
    var tipoEvenCodi = $('#cboTipoEventoFiltro').val();

    if (tipoEvenCodi != "" || tipoEvenCodi == null) {
        $.ajax({
            type: 'POST',
            url: controler + "ListarCboCausa",
            datatype: 'json',
            data: JSON.stringify({ claProCodi: $('#cboTipoEventoFiltro').val() }),
            contentType: "application/json",
            success: function (modelo) {
                $('#cboCausaFiltro').empty();
                //var option = '<option value="0">----- Seleccione  ----- </option>';
                $.each(modelo.ListaCausas, function (k, v) {
                    var option = '<option value =' + v.Subcausacodi + '>' + v.Subcausadesc + '</option>';
                    $('#cboCausaFiltro').append(option);
                })
                $('#cboCausaFiltro').multipleSelect({
                    filter: true,
                    placeholder: "SELECCIONE"
                });
                $("#cboCausaFiltro").multipleSelect("refresh");
                $('#cboCausaFiltro').multipleSelect('checkAll');
            }
        });
    } else {
        $('#cboCausaFiltro').empty();
        $('#cboCausaFiltro').multipleSelect({
            filter: true,
            placeholder: "SELECCIONE"
        });
        $("#cboCausaFiltro").multipleSelect("refresh");
        $('#cboCausaFiltro').multipleSelect('checkAll');
        //var option = '<option value="0">----- Seleccione un tipo intervencion ----- </option>';
        //$('#cboCausaFiltro').append(option);
    }

    //$('#cboCausaFiltro').multipleSelect({
    //    filter: true,
    //    placeholder: "SELECCIONE"
    //});
    //$("#cboCausaFiltro").multipleSelect("refresh");
    //$('#cboCausaFiltro').multipleSelect('checkAll');
}

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
            $("#Entidad_Interfechaini").val(model.Progrfechaini);
            $("#Entidad_Interfechafin").val(model.Progrfechafin);

            //llenar varibaler global
            fechaIniProg = model.Progrfechaini;
            fechaFinProg = model.Progrfechafin;

            if (tipoProgramacion > 5) {
                $("#btnReporteActividades").hide();
            }
            else {
                $("#btnReporteActividades").show();
            }
        },
        error: function (err) {
            alert("Lo sentimos, se ha producido un error al obtener las fechas de operacion");
        }
    });
}

//Exportar reporte actividades nuevas y canceladas
function reporteActividades() {
    var codprograma = parseInt($('#Programacion').val()) || 0;

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

//#region Metodos Generales

function _mostrarH48Indisp() {
    var id = parseInt($("#cboFamilias").val()) || 0;
    if (id == 1) {
        $("#cboHrasIndispp").parent().parent().show()
    } else {
        $("#cboHrasIndispp").parent().parent().hide()
        $("#cboHrasIndispp").val(0);
    }
}

//funcion que calcula el ancho disponible para la tabla reporte
function getHeightTablaListado() {
    return $(window).height()
        - $("header").height()
        - $("#cntTitulo").height() - 2
        - $("#Reemplazable .form-title").height()
        - 15
        - $("#Contenido").parent().height() //Filtros
        - 20 //leyenda
        - 15
        - 61 //- $(".footer").height() - 10

        - 18 //propio de la tabla
        ;
}

function validarConsulta(objFiltro) {
    var listaMsj = [];

    if (objFiltro.TipoProgramacion <= 0)
        listaMsj.push("Seleccione un Tipo de Programación");

    //tipo evento
    if (objFiltro.TipoEvenCodi == "")
        listaMsj.push("Tipo evento: Seleccione una opción.");
    //causa
    if (objFiltro.Subcausa == "")
        listaMsj.push("Causa: Seleccione una opción.");
    //Disponibilidad
    if (objFiltro.InterIndispo == "")
        listaMsj.push("Disponibilidad: Seleccione una opción.");
    //clase programación
    if (objFiltro.ClaseProgramacion == "")
        listaMsj.push("Clase programación: Seleccione una opción.");
    //Empresa
    if (objFiltro.Emprcodi == "")
        listaMsj.push("Empresa: Seleccione una opción.");
    //maniobras
    if (objFiltro.Maniobras == "")
        listaMsj.push("Maniobras: Seleccione una opción.");

    // Valida filtros de fecha cuando falta seleccionar cualquiera de ellas
    if (objFiltro.InterFechaIni == "") {
        listaMsj.push("Seleccione una fecha de inicio");
    }
    if (objFiltro.InterFechaFin == "") {
        listaMsj.push("Seleccione una fecha de fin");
    }

    // Valida consistencia del rango de fechas
    if (!validarFormatoFech(objFiltro.InterFechaIni) || !validarFormatoFech(objFiltro.InterFechaFin)) {
        listaMsj.push("La fecha ingresada no tiene el formato correcto");
    } else {
        if (objFiltro.InterFechaIni != "" && objFiltro.InterFechaFin != "") {
            if (!CompararFechas(objFiltro.InterFechaIni, objFiltro.InterFechaFin)) {
                listaMsj.push("La fecha de inicio no puede ser mayor que la fecha de fin");
            }
        }
    }

    var msj = listaMsj.join('\n');
    return msj;
}

function getObjetoFiltro() {
    var progrCodi = parseInt($('#Programacion').val()) || 0;
    var tipoProgramacion = parseInt($('#TipoProgramacion').val()) || 0;

    var interFechaIni = $('#Entidad_Interfechaini').val();
    var interFechaFin = $('#Entidad_Interfechafin').val();

    var emprCodi = $('#cboEmpresaFiltro').multipleSelect('getSelects').join(',');
    var tipoEvenCodi = $('#cboTipoEventoFiltro').multipleSelect('getSelects').join(',');
    var estadoCodi = $('#estadocodi').multipleSelect('getSelects').join(','); //nuevo filtro

    var interIndispo = $('#cboDisponibilidad').multipleSelect('getSelects').join(',');
    //var interIndispo = $('#cboDisponibilidad').val();

    var vCausa = $('#cboCausaFiltro').multipleSelect('getSelects').join(',');
    //var vCausa = $('#cboCausaFiltro').val();

    var vClaseProgramacion = $('#cboClaseProgramacionFiltro').multipleSelect('getSelects').join(',');
    //var vClaseProgramacion = $('#cboClaseProgramacionFiltro').val();

    var vFamilia = $('#cboFamilias').val();

    var vManiobras = $('#cboManiobras').val() != null && $('#cboManiobras')[0].length == $('#cboManiobras').val().length ? "0" : $('#cboManiobras').multipleSelect('getSelects').join(',');
    //var vManiobras = $('#cboManiobras').val();
    var vHorasIndispo = $('#cboHrasIndispp').val();

    var estadoFiles = getEstadoFiles();
    var estadoNota = getCheckNota();

    var obj = {
        Progrcodi: progrCodi,
        TipoProgramacion: tipoProgramacion,

        InterFechaIni: interFechaIni,
        InterFechaFin: interFechaFin,

        Emprcodi: emprCodi,
        TipoEvenCodi: tipoEvenCodi,

        InterIndispo: interIndispo,

        Subcausa: vCausa,
        ClaseProgramacion: vClaseProgramacion,

        TipoGrupoEquipo: vFamilia,
        Maniobras: vManiobras,
        HorasIndispo: vHorasIndispo,

        EstadoFiles: estadoFiles,
        EstadoNota: estadoNota,
        EstadoCodi: estadoCodi,
    };

    return obj;
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

//check Mostrar Archivos
function getEstadoFiles() {
    var estado = "0";
    if ($('#check_mostrarAdjuntos').is(':checked')) {
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

// solo para formatos dd/mm/yyy
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

function validarFormatoFech(obj) {
    var patron = /^\d{1,2}\/\d{2}\/\d{4}$/
    if (!patron.test(obj)) {
        return false;
    }
    return true;
}

//#endregion

function descargarProcedimientosManiobras() {
    // 6  -> EquiCodi
    var objData = getObjetoFiltro();
    var msj = validarConsulta(objData);

    if (msj == "") {
        $.ajax({
            type: 'POST',
            url: controler + 'ExportarProcedimientosManiobras',
            dataType: 'json',
            data: objData,
            success: function (result) {
                if (result.Resultado != "-1") {
                    if (result.Resultado != "0")
                        window.location = controler + 'AbrirDescargarProcedimientosManiobras';
                    else {
                        alert("No existen equipos.");
                    }
                } else {
                    alert(result.Mensaje);
                }
            },
            error: function (err) {
                alert("Lo sentimos, se ha producido un error");
            }
        });
    } else {
        alert(msj);
    }
}

function descargarReporteNTIITR() {
    $.ajax({
        type: 'POST',
        url: controler + 'ExportarReporteNTIITR',
        data: {
            fechaIni: toDate($('#Entidad_Interfechaini').val()).toISOString(),
            fechaFin: toDate($('#Entidad_Interfechafin').val()).toISOString()
        },
        dataType: 'json',
        success: function (result) {
            if (result.Resultado !== "-1") {
                var paramList = [
                    { tipo: 'input', nombre: 'file', value: result.NombreArchivo }
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
 * Opciones de comunicaciones
 * */
function enviarRespuestaComunicacion() {
    $('#frmRegistroMensajePopup').attr('src', '');
    $('#frmRegistroMensajePopup').hide();

    var intercodis = ($("#hfIntercodiMsj").val()).trim();

    var url = controlador + 'IntervencionesMensajeRegistro' +
        '?intercodis=' + intercodis +
        '&origen=' + 'CRUZADA';

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
 * Log de modificaciones
 * @param {any} interCodi
 */
function abrirPopupLog(interCodi) {
    $.ajax({
        type: 'POST',
        url: controler + "ListadoModificaciones",
        data: {
            interCodi: interCodi
        },
        success: function (evt) {
            $('#popupModificaciones').html(evt);

            $('#popupModificaciones').bPopup({
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

/**
 * Tacometro F1 y F2
 * */
function generarDashboard(horizonte) {

    var fechaPeriodo = $('#Entidad_Interfechaini').val();
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
                y: 25,
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
            marginTop: 6,
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
            enabled: false,
            align: 'center',
            verticalAlign: 'bottom'
        },

        series: series

    });


};