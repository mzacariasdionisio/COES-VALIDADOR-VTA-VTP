var controlador = siteRoot + 'PMPO/ParametrosFechas/';

var AGREGAR_ANIO = 1;
var EDITAR_ANIO = 2;
var DETALLES_ANIO = 3;

var DE_LISTADO = 1;
var DE_POPUP = 2;

var tblInicioSemMes;
var tblFeriados;
var containerSemanaMes;
var containerFeriados;

var TIENE_PERMISO_NUEVO = false;
var TIENE_PERMISO_EDITAR_ADMIN = false;

var filaMesSeleccionado = -1;

$(function () {

    $('#tab-container').easytabs({
        animate: false,
        select: '#vistaListado'
    });
    $('#tab-container').bind('easytabs:after', function () {
        refrehDatatable();
    });
    $('#tab-container').bind('easytabs:before', function () {
        var esTabDetalle = $("#tab-container .tab.active").html().toLowerCase().includes('detalle');
        var existeHtmlTabDetalle = $("#vistaDetalle").html().trim() != '';
        var esEditarCrear = parseInt($("#hfAccionDetalle").val()) != DETALLES_ANIO;

        if (esTabDetalle && existeHtmlTabDetalle && esEditarCrear) {
            if (confirm('¿Desea cambiar de pestaña? Si selecciona "Aceptar" se perderán los cambios. Si selecciona "Cancelar" se mantendrá en la misma pestaña')) {
                $("#vistaDetalle").html(''); //limpiar tab Detalle
            } else {
                return false;
            }
        }
    });

    mostrarListadoAniosOperativos();

    $('#btnAgregarAnio').click(function () {
        mantenerAnioOperativo(AGREGAR_ANIO, null, DE_LISTADO);//accion, aniocodi, origen
    });

    $('#btnGenerarArchivos').click(function () {
        limpiarCamposArchivosSalida();
        abrirPopup("escogerArchivos");
    });

    $('#btnGenerarExportacion').click(function () {
        generarArchivos();
    });

    $('#btnCancelarArchivo').click(function () {
        $('#escogerArchivos').bPopup().close();
    });

    $('#txtArchivosAnioIni').Zebra_DatePicker({
        format: 'Y',
        pair: $('#txtArchivosAnioFin')
    });

    $('#txtArchivosAnioFin').Zebra_DatePicker({
        format: 'Y',
        direction: 1
    });

    $('#txtAddFeriadoFec').Zebra_DatePicker({
        format: 'd/m/Y',
        //start_date: false,    
    });

    $('#btnCancelarFeriado').click(function () {
        $('#agregarFeriadoAO').bPopup().close();
    });

    $('#btnGrabarFeriado').click(function () {
        AumentarFeriado();
    });

    $("#btnCCancelarAnio").click(function () {
        $('#descripcionCambios').bPopup().close();
    });

    $("#btnCGrabarAnio").click(function () {
        guardarAnioOperativo(EDITAR_ANIO);
    });
});

function mostrarListadoAniosOperativos() {

    $.ajax({
        type: 'POST',
        url: controlador + "ListarAniosOperativos",
        dataType: 'json',
        data: {},
        success: function (evt) {
            if (evt.Resultado != "-1") {

                $("#cuadroAnios").html(evt.HtmlListadoAniosOp);
                refrehDatatable();
            } else {
                mostrarMensaje_('mensaje', 'error', 'Se ha producido un error: ' + evt.Mensaje);
            }
        },
        error: function (err) {
            alert("Ha ocurrido un error");
        }
    });
}
function refrehDatatable() {
    $('#tabla_anios').dataTable({
        "scrollY": 430,
        "scrollX": true,
        "sDom": 't',
        "ordering": false,
        "destroy": "true",
        "iDisplayLength": 50
    });
}

function mostrarVersiones(_anio) {
    $('#listadoHAO').html('');

    $.ajax({
        type: 'POST',
        url: controlador + "VersionListado",
        data: {
            anio: _anio
        },
        success: function (evt) {
            if (evt.Resultado != "-1") {
                $('#listadoHAO').html(evt.Resultado);

                $("#listadoHAO").css("width", (820) + "px");

                $('.tabla_version_x_anio').dataTable({
                    "sPaginationType": "full_numbers",
                    "destroy": "true",
                    "ordering": false,
                    "searching": true,
                    "iDisplayLength": -1,
                    "info": false,
                    "paging": false,
                    "scrollX": false,
                    "scrollY": "250px",
                });

                abrirPopup("historialAO");
            } else {

                mostrarMensaje_('mensaje', 'error', 'Ha ocurrido un error al listar las versiones :' + evt.Mensaje);
            }
        },
        error: function (err) {

            mostrarMensaje_('mensaje', 'error', 'Ha ocurrido un error.');
        }
    });
}

function AumentarFeriado() {
    var objFeriado = {};

    objFeriado = getCamposFeriadoJson();

    var mensaje = validarDatosObligatiosFeriado(objFeriado);
    if (mensaje == "") {

        var dataHandsonFeriados, dataValidoFer = [];
        dataHandsonFeriados = tblFeriados.getSourceData();
        dataValidoFer = obtenerData(dataHandsonFeriados, true);
        var anio = parseInt($('#hfAnio').val()) || 0;

        var dataJson = {
            fecha: objFeriado.fecha,
            descripcion: objFeriado.descripcion,
            anio: anio,

            listaFeriadosEnPantalla: dataValidoFer
        };

        $.ajax({
            url: controlador + "AgregarFeriado",
            type: 'POST',
            contentType: 'application/json; charset=UTF-8',
            dataType: 'json',
            data: JSON.stringify(dataJson),
            success: function (result) {

                if (result.Resultado === "-1") {
                    mostrarMensaje_('mensajeFeriado', 'error', result.Mensaje);
                } else {
                    cargarHansonTableFeriados(result.ListaFeriados);
                    $('#agregarFeriadoAO').bPopup().close();
                }
            },
            error: function (xhr, status) {
                mostrarMensaje_('mensaje', 'error', 'Se ha producido un error.');
            }
        });
    } else {
        mostrarMensaje_('mensajeFeriado', 'error', mensaje);
    }
}
function limpiarCamposFeriado() {
    $('#txtAddFeriadoDesc').val("");
}

function getCamposFeriadoJson() {
    var obj = {};

    obj.fecha = $('#txtAddFeriadoFec').val();
    obj.descripcion = $('#txtAddFeriadoDesc').val();

    return obj;
}

function validarDatosObligatiosFeriado(objFeriado) {
    var msj = "";
    if (objFeriado.fecha == "") {
        msj += "<p>Debe seleccionar una fecha correcta.</p>";
    }

    if (objFeriado.descripcion == "") {
        msj += "<p>Debe ingresar una Descripcion.</p>";
    }

    return msj;
}
function mantenerAnioOperativo(accion, aniocodi, origen) {
    $('#tab-container').easytabs('select', '#vistaDetalle');

    mostrarVistaDetalles(accion, aniocodi, origen);

    if (origen == DE_POPUP)
        $('#historialAO').bPopup().close();
}

function mostrarVistaDetalles(accion, aniocodi, origen) {
    $("#vistaDetalle").html(''); //limpiar tab Detalle
    aniocodi = aniocodi || 0;

    $.ajax({
        type: 'POST',
        url: controlador + "CargarDetalles",
        data: {
            accion: accion,
            aniocodi: aniocodi
        },
        success: function (evt) {

            $('#vistaDetalle').html(evt);

            inicioDetalles(accion, aniocodi);


        },
        error: function (err) {
            alert('Ha ocurrido un error: ' + err);
        }
    });

}

function cargarFecIniAnio(aniio, accion, aniocodi) {
    var anio = parseInt(aniio) || 0;

    $.ajax({
        type: 'POST',
        url: controlador + 'CargarFechaInicialAnio',
        data: {
            anio: anio,
            accion: accion,
            aniocodi: aniocodi
        },

        success: function (evt) {
            if (evt.Resultado != "-1") {
                var fechaInicio = evt.FechaIniAnio;

                if (accion == AGREGAR_ANIO)
                    $('#versionMostrada').html("");
                else
                    $('#versionMostrada').html("<b>Version Mostrada: </b>" + evt.NumVersion);

                TIENE_PERMISO_NUEVO = evt.TienePermisoNuevo;
                TIENE_PERMISO_EDITAR_ADMIN = evt.TienePermisoEditar;

                esAnioProcesado = evt.EsProcesado;
                validaDesactivacionFechaInicio(fechaInicio, "txtFechaIniAnio", TIENE_PERMISO_NUEVO, esAnioProcesado);

                $('#txtDiaNombre').html(evt.DiaNombre);

                $('#txtFechaIniAnio').val(fechaInicio);
                $('#hfFechaIniAnio').val(fechaInicio);

                $('#txtFechaFinAnio').html(evt.FechaFinAnio);
                $('#hfFechaFinAnio').val(evt.FechaFinAnio);

                cargarHansonTableSemanaMes(evt.ListaSemanaMes);
                cargarHansonTableFeriados(evt.ListaFeriados);
            } else {
                mostrarMensaje_('mensaje', 'error', 'Se ha producido un error: ' + evt.Mensaje);
            }

        },
        error: function (err) {
            mostrarMensaje('mensaje', 'error', 'Se ha producido un error al cargar el listado de identificadores para la fecha escogida.');

        }
    });
}

function actualizarListaSemanaMes(anioSeleccionado, diaIniAnioSeleccionado, diaIniAnioAntiguo) {

    var dataHandsonSM, dataValidoSM = [];
    dataHandsonSM = tblInicioSemMes.getSourceData();
    dataValidoSM = obtenerData(dataHandsonSM, true);
    var anio = parseInt(anioSeleccionado) || 0;
    var dataJson = {
        anio: anio,
        fechaIniAnio: diaIniAnioSeleccionado,

        listaSMEnPantalla: dataValidoSM
    };

    $.ajax({
        url: controlador + "ActualizarLstSemanaMes",
        type: 'POST',
        contentType: 'application/json; charset=UTF-8',
        dataType: 'json',
        data: JSON.stringify(dataJson),
        success: function (result) {

            if (result.Resultado === "-1") {
                $('#txtFechaIniAnio').val(diaIniAnioAntiguo); //si al escoger fechaIni erroneo, debe mostrar el antiguo
                $('#hfFechaIniAnio').val(diaIniAnioAntiguo);

                mostrarMensaje_('mensaje', 'error', 'Se ha producido un error: ' + result.Mensaje);
            } else {
                limpiarBarraMensaje('mensaje');
                $('#txtFechaFinAnio').html(result.FechaFinRango);
                $('#hfFechaFinAnio').val(result.FechaFinRango);
                cargarHansonTableSemanaMes(result.ListaSemanaMes);
            }
        },
        error: function (xhr, status) {
            mostrarMensaje_('mensaje', 'error', 'Se ha producido un error al actualizar el listado de inicios de semana mes.');
        }
    });

}

function actualizarListaFeriados(anioSeleccionado, diaIniAnioSeleccionado, diaIniAnioAntiguo, aniocodi) {

    var dataHandsonFeriados, dataValidoFeriados = [];
    dataHandsonFeriados = tblFeriados.getSourceData();
    dataValidoFeriados = obtenerData(dataHandsonFeriados, true);
    var anio = parseInt(anioSeleccionado) || 0;
    var dataJson = {
        anio: anio,
        fechaIniAnio: diaIniAnioSeleccionado,
        aniocodi: aniocodi,
        listaFeriadoPantalla: dataValidoFeriados
    };

    $.ajax({
        url: controlador + "ActualizarLstFeriados",
        type: 'POST',
        contentType: 'application/json; charset=UTF-8',
        dataType: 'json',
        data: JSON.stringify(dataJson),
        success: function (result) {

            if (result.Resultado === "-1") {
                $('#txtFechaIniAnio').val(diaIniAnioAntiguo); //si al escoger fechaIni erroneo, debe mostrar el antiguo
                $('#hfFechaIniAnio').val(diaIniAnioAntiguo);

                mostrarMensaje_('mensaje', 'error', 'Se ha producido un error: ' + result.Mensaje);
            } else {
                cargarHansonTableFeriados(result.ListaFeriados);
            }
        },
        error: function (xhr, status) {
            mostrarMensaje_('mensaje', 'error', 'Se ha producido un error al actualizar el listado de feriados.');
        }
    });

}

function quitarFeriado(fila) {

    if (confirm("¿Esta seguro de eliminar el feriado? ")) {
        tblFeriados.alter("remove_row", fila);
    }
}

function mostrarPopupFeriado() {
    var today = diaActual();
    $('#txtAddFeriadoFec').val(today);

    ocultarMensajeValidacion("mensajeFeriado");
    limpiarCamposFeriado();
    abrirPopup("agregarFeriadoAO");
}

function ocultarMensajeValidacion(id) {
    mostrarMensaje_(id, 'message', '');
    $("#" + id).css("display", "none");
}

function getObjetoArchivoJson() {
    var obj = {};

    obj.tipo = parseInt($("#tipoArchivo").val()) || 0;
    obj.anioIni = parseInt($("#txtArchivosAnioIni").val()) || 0;
    obj.anioFin = parseInt($("#txtArchivosAnioFin").val()) || 0;

    return obj;
}

function limpiarCamposArchivosSalida() {
    $('#tipoArchivo').val(0);

    var hoy = new Date();
    var anioActual = hoy.getFullYear()
    var rangoIni = anioActual;
    var rangoFin = anioActual + 3;

    $("#txtArchivosAnioIni").val(rangoIni.toString());
    $("#txtArchivosAnioFin").val(rangoFin.toString());
}

function generarArchivos() {

    var obj = {};
    obj = getObjetoArchivoJson();

    var mensaje = validarDatosArchivos(obj);

    if (mensaje == "") {

        $.ajax({
            type: 'POST',
            url: controlador + "GenerarArchivoSalida",
            data: {
                tipo: obj.tipo,
                anioIni: obj.anioIni,
                anioFin: obj.anioFin
            },
            success: function (evt) {
                if (evt.Resultado != "-1") {
                    $('#escogerArchivos').bPopup().close();
                    window.location = controlador + "Exportar?file_name=" + evt.Resultado;
                } else {
                    mostrarMensaje_('mensajeArchivo', 'error', evt.Mensaje);
                }
            },
            error: function (err) {
                alert('Ha ocurrido un error.');
            }
        });
    } else {
        mostrarMensaje_('mensajeArchivo', 'error', mensaje);
    }
}

function validarDatosArchivos(obj) {

    var msj = "";
    if (obj.tipo == 0) {
        msj += "<p>Debe seleccionar un tipo de archivo a generar.</p>";
    }

    if (obj.anioIni == 0) {
        msj += "<p>Debe ingresar año inicial correcto.</p>";
    }

    if (obj.anioFin == 0) {
        msj += "<p>Debe ingresar año final correcto.</p>";
    }

    if (obj.anioIni != 0 || obj.anioFin != 0) {
        if (obj.anioIni > obj.anioFin) {
            msj += "<p>El año final debe ser mayor o igual al año inicial.</p>";
        }
    }


    return msj;
}

function inicioDetalles(accion, aniocodi) {

    limpiarBarraMensaje('mensaje');

    var anioDefault = $('#txtAnio').val();
    var esteAnio = (new Date().getFullYear() + 10).toString();

    cargarFecIniAnio(anioDefault, accion, aniocodi);

    if (accion == EDITAR_ANIO) {//oculto nota
        $("#nota1").css("display", "block");
        $("#fecI").html("Fecha Inicio(*):");
    }
    else {
        $("#nota1").css("display", "none");
        $("#fecI").html("Fecha Inicio:");
    }


    if (accion == EDITAR_ANIO || accion == DETALLES_ANIO)  //Deshabilito Calendario Anio, cuando se edita o detalles, no debe modificarse el año
        cambiarEstadoInput("txtAnio", true);

    $('#txtAnio').Zebra_DatePicker({
        format: 'Y',
        direction: ['2000', esteAnio], //muestra solo hasta el proximo año
        onSelect: function () {
            var anioSeleccionado = $('#txtAnio').val();

            //Seteo de nuevos valores
            $('#hfAnio').val(anioSeleccionado);

            cargarFecIniAnio(anioSeleccionado, AGREGAR_ANIO, null); //cada vez q cambia año muestra DEFAULT 

        }

    });

    var diaIniAnioAntiguo = "";
    $('#txtFechaIniAnio').Zebra_DatePicker({
        format: 'd/m/Y',
        //start_date: false,   PARA QUE SE USA?
        disabled_dates: ['* * * 0,1,2,3,4,5'],
        onOpen: function () {
            diaIniAnioAntiguo = $('#txtFechaIniAnio').val();
        },
        onSelect: function () {
            var diaIniAnioSeleccionado = $('#txtFechaIniAnio').val();
            $('#hfFechaIniAnio').val(diaIniAnioSeleccionado);

            var anioSeleccionado = $('#txtAnio').val();

            actualizarListaSemanaMes(anioSeleccionado, diaIniAnioSeleccionado, diaIniAnioAntiguo);
            //Feriados
            actualizarListaFeriados(anioSeleccionado, diaIniAnioSeleccionado, diaIniAnioAntiguo, aniocodi);
        }
    });

    //Restablecer fecha por defecto
    $("#btnRestablecerDefault").click(function () {
        var anioSeleccionado = $('#txtAnio').val();

        //Seteo de nuevos valores
        $('#hfAnio').val(anioSeleccionado);

        cargarFecIniAnio(anioSeleccionado, AGREGAR_ANIO, null); //cada vez q cambia año muestra DEFAULT 
    });

    $("#btnGrabarAnioOperativo").click(function () {
        guardarAnioOperativo(AGREGAR_ANIO);
    });

    $("#btnAgregarFeriado").click(function () {
        mostrarPopupFeriado();
    });

    $("#btnActualizarAnioOperativo").click(function () {
        limpiarCamposAgregarDescripcion();
        abrirPopup("descripcionCambios");
    });

    $("#btnCancelarAnioOperativo").click(function () {
        $("#vistaDetalle").html(''); //limpiar tab Detalle

        $('#tab-container').easytabs('select', '#vistaListado');
        mostrarListadoAniosOperativos();
    });

    var dtpConfig = {
        firstDay: 1,
        showWeekNumber: true,
        i18n: {
            previousMonth: 'Mes anterior',
            nextMonth: 'Mes siguiente',
            months: ['Enero', 'Febrero', 'Marzo', 'Abril', 'Mayo', 'Junio', 'Julio', 'Agosto', 'Septiembre', 'Octubre', 'Noviembre', 'Diciembre'],
            weekdays: ['Domingo', 'Lunes', 'Martes', 'Miercoles', 'Jueves', 'Viernes', 'Sabado'],
            weekdaysShort: ['Dom', 'Lun', 'Mar', 'Mier', 'Jue', 'Vie', 'Sab']
        },
        disableDayFn: function (date) {
            // Desactivo todo menos sabados y dias anteriores a HOy
            //return date.getDay() === 0 || date.getDay() === 1 || date.getDay() === 2 || date.getDay() === 3 || date.getDay() === 4 || date.getDay() === 5 || moment().isAfter(moment(date), 'day');
        }
    };

    // #region Handsontable SEMANA/MES
    containerSemanaMes = document.getElementById('tblSemanaMes');

    tblInicioSemMes = new Handsontable(containerSemanaMes, {
        dataSchema: {

            NombreMes: null,
            FechaIniDesc: null,
            NroSemana: null,
            FechaIni: null,
            ProcesadoDesc: null,
        },
        colHeaders: ['Mes', 'Fecha', 'Semana', '¿Procesado?'],
        columns: [
            { data: 'NombreMes', editor: false, className: 'soloLectura htCenter', readOnly: true }, //SoloLectura es para cambiar color de fondo al validar, falta agregarlo al css
            { data: 'FechaIniDesc', type: 'date', dateFormat: 'DD/MM/YYYY', correctFormat: true, datePickerConfig: dtpConfig, className: 'htCenter' },
            { data: 'NroSemana', editor: false, className: 'soloLectura htCenter', readOnly: true },
            { data: 'ProcesadoDesc', editor: false, className: 'soloLectura htCenter', readOnly: true }

        ],

        colWidths: [140, 150, 80, 80],
        columnSorting: false, // no puede ordenarse las columnas
        minSpareRows: 0,
        showWeekNumber: false,
        rowHeaders: false, //quita numeración
        autoWrapRow: true,
        startRows: 0,
        width: 500, //ancho de la tabla
        height: 350, //alto de la tabla
        manualColumnResize: true,

    });

    //Validaciones en columna Fecha 
    tblInicioSemMes.updateSettings({
        afterSelection: function (row, c, r2, c2) {
            if (c == 1) {
                filaMesSeleccionado = row;
            }
        },
        beforeKeyDown: function (event) {  //evita editar directamente la celda de fechas (menos en caso dobleclick)
            event.stopImmediatePropagation();
        },

        cells: function (row, col, prop) {

            var cellProperties = {};

            if (!TIENE_PERMISO_NUEVO) { //Si no tiene permiso, cambio a readOnly la columna Fecha (las otras columnas ya son readonly, no son necesarios)                
                cellProperties.readOnly = 'true';
                cellProperties.renderer = fileNoEditable; // uses function directly                
            }
            else {

                //Para deactivar inicios Semana/Mes (fechas) ya pasadas                               
                var strFechaCelda = tblInicioSemMes.getDataAtCell(row, 1);
                var esProcesado = tblInicioSemMes.getDataAtCell(row, 3);

                if (strFechaCelda != null && strFechaCelda != "") {
                    if (esProcesado != "" && esProcesado != null) {
                        if (esProcesado == "Si" && !TIENE_PERMISO_EDITAR_ADMIN) { //no se puede editar si esta procesado y el usuario no es admin
                            cellProperties.readOnly = 'true';
                            cellProperties.renderer = fileNoEditable; // uses function directly
                        }
                    }
                }

                if (col == 1) {

                    /*VALIDACION RANGOS DATEPICKER */
                    var fechaEne = tblInicioSemMes.getDataAtCell(0, 1);
                    var fechaFeb = tblInicioSemMes.getDataAtCell(1, 1);
                    var fechaMar = tblInicioSemMes.getDataAtCell(2, 1);
                    var fechaAbr = tblInicioSemMes.getDataAtCell(3, 1);
                    var fechaMay = tblInicioSemMes.getDataAtCell(4, 1);
                    var fechaJun = tblInicioSemMes.getDataAtCell(5, 1);
                    var fechaJul = tblInicioSemMes.getDataAtCell(6, 1);
                    var fechaAgo = tblInicioSemMes.getDataAtCell(7, 1);
                    var fechaSet = tblInicioSemMes.getDataAtCell(8, 1);
                    var fechaOct = tblInicioSemMes.getDataAtCell(9, 1);
                    var fechaNov = tblInicioSemMes.getDataAtCell(10, 1);
                    var fechaDic = tblInicioSemMes.getDataAtCell(11, 1);
                    var fechaDic_ = $("#hfFechaFinAnio").val();

                    if (row == 0) { //Enero
                        setearRangoFechas(row, fechaEne, fechaFeb, this);
                    }

                    if (row == 1) { //Febrero
                        setearRangoFechas(row, fechaEne, fechaMar, this);
                    }

                    if (row == 2) { //Marzo
                        setearRangoFechas(row, fechaFeb, fechaAbr, this);
                    }
                    if (row == 3) { //Abril
                        setearRangoFechas(row, fechaMar, fechaMay, this);
                    }

                    if (row == 4) { //Mayo
                        setearRangoFechas(row, fechaAbr, fechaJun, this);
                    }

                    if (row == 5) { //Junio
                        setearRangoFechas(row, fechaMay, fechaJul, this);
                    }

                    if (row == 6) { //Julio
                        setearRangoFechas(row, fechaJun, fechaAgo, this);
                    }

                    if (row == 7) { //Agosto
                        setearRangoFechas(row, fechaJul, fechaSet, this);
                    }

                    if (row == 8) { //Setiembre
                        setearRangoFechas(row, fechaAgo, fechaOct, this);
                    }

                    if (row == 9) { //Octubre
                        setearRangoFechas(row, fechaSet, fechaNov, this);
                    }

                    if (row == 10) { //Noviembre
                        setearRangoFechas(row, fechaOct, fechaDic, this);
                    }

                    if (row == 11) { //Diciembre
                        setearRangoFechas(row, fechaNov, fechaDic_, this);
                    }

                }
            }


            return cellProperties;
        }
    });

    tblInicioSemMes.addHook('afterChange', function (TD, row, column, prop, value, cellProperties) {

        //cada cambio de fecha en la columna Fecha debe actualizar su Semana
        if (TD != null) {
            var datos = TD[0];
            var columna = datos[1];
            var fechaAntigua = datos[2];
            var fechaNueva = datos[3];
            if (columna === "FechaIniDesc" && fechaAntigua != fechaNueva) {

                var diaIniAnioSeleccionado = $('#txtFechaIniAnio').val();
                var anioSeleccionado = $('#txtAnio').val();
                actualizarListaSemanaMes(anioSeleccionado, diaIniAnioSeleccionado, diaIniAnioSeleccionado);


            }
        }
    });


    // #region Handsontable FERIADOS
    containerFeriados = document.getElementById('tblFeriadosAnio');

    tblFeriados = new Handsontable(containerFeriados, {
        dataSchema: {
            Pmfrdocodi: null,
            Pmanopcodi: null,
            Pmfrdofecha: null,
            Pmfrdodescripcion: null,
            Pmfrdoestado: null,
            Pmfrdousucreacion: null,
            Pmfrdofeccreacion: null,
            Pmfrdousumodificacion: null,
            Pmfrdofecmodificacion: null,
            PmfrdofechaDesc: null,

        },
        colHeaders: ['Acción', 'Fecha', 'Descripción'],
        columns: [
            { className: 'soloLectura htCenter' },
            { data: 'PmfrdofechaDesc', editor: false, className: 'soloLectura htCenter', readOnly: true },
            { data: 'Pmfrdodescripcion', editor: false, className: 'soloLectura htLeft', readOnly: true },

        ],
        cells: function (row, col) { //para usar botones (Eliminar feriado) en la primera columna
            var cellProperties = {};
            if (col === 0 && TIENE_PERMISO_NUEVO) { //solo muestra boton eliminar si tiene permisos
                cellProperties.readOnly = true;
                cellProperties.renderer = function (instance, td, row, col, prop, value, cellProperties) {
                    Handsontable.renderers.TextRenderer.apply(this, arguments);

                    td.innerHTML = "<a class='' href='JavaScript:quitarFeriado(" + row + ")' > <img style='margin-top: 3px; margin-bottom: -4px; height: 17px; cursor:pointer;' src='" + siteRoot + "Content/Images/btn-cancel.png' title='Eliminar Feriado' alt='Eliminar Feriado' /> </a>";
                };
            }

            if (!TIENE_PERMISO_NUEVO) {
                cellProperties.readOnly = 'true';
                cellProperties.renderer = fileNoEditable; // uses function directly
            }

            return cellProperties;
        },

        colWidths: [80, 120, 250],
        columnSorting: false, // no puede ordenarse las columnas
        minSpareRows: 0,
        rowHeaders: false, //quita numeración (de la primera columna)
        autoWrapRow: true,
        startRows: 0,
        width: 500, //ancho de la tabla
        height: 350, //alto de la tabla
        manualColumnResize: true,
    });

}

function fileNoEditable(instance, td, row, col, prop, value, cellProperties) {
    Handsontable.renderers.TextRenderer.apply(this, arguments);
    //td.style.fontWeight = 'bold';
    td.style.color = 'dimgray';
    td.style.background = 'bisque';
}

function setearRangoFechas(row, fechaInf, fechaSup, objeto) {
    if (filaMesSeleccionado == row) {
        if (fechaInf != null && fechaSup != null) {
            var fecMin = cambiarFormatoAFecha(fechaInf);
            var fecMax = cambiarFormatoAFecha(fechaSup);

            objeto.datePickerConfig.disableDayFn = function (date) {
                if (row == 0)
                    return verificarDeshabilitacionFechasEnero(date, fecMin, fecMax);
                else
                    return verificarDeshabilitacionFechas(date, fecMin, fecMax);
            }
        }
    }
}

function verificarDeshabilitacionFechas(date, fecMin, fecMax) {
    var fechaDate = moment(date).format('YYYY/MM/DD');
    var resultado = date.getDay() === 0 || date.getDay() === 1 || date.getDay() === 2 || date.getDay() === 3 || date.getDay() === 4 || date.getDay() === 5 ||
        moment(fecMin).isSameOrAfter(fechaDate) || moment(fechaDate).isSameOrAfter(fecMax);

    return resultado;
}

function verificarDeshabilitacionFechasEnero(date, fecMin, fecMax) {
    var fechaDate = moment(date).format('YYYY/MM/DD');
    var resultado = date.getDay() === 0 || date.getDay() === 1 || date.getDay() === 2 || date.getDay() === 3 || date.getDay() === 4 || date.getDay() === 5 ||
        moment(fecMin).isAfter(fechaDate) || moment(fechaDate).isSameOrAfter(fecMax);

    return resultado;
}


function cambiosRenderer(instance, td, row, col, prop, value, cellProperties) {
    Handsontable.renderers.TextRenderer.apply(this, arguments);
}

function cargarHansonTableSemanaMes(listado) {
    tblInicioSemMes.loadData([]); //hansonTableClear();

    cargarHansonSemanaMes(tblInicioSemMes, listado);
}

function cargarHansonSemanaMes(tblSemMes, inputJson) {
    var lstSemanaMes = inputJson && inputJson.length > 0 ? inputJson : [];

    var lstData = [];
    tblSemMes.loadData(lstData);

    for (var index in lstSemanaMes) {

        var item = lstSemanaMes[index];

        var data = {
            NombreMes: item.NombreMes,
            FechaIniDesc: item.FechaIniDesc,
            NroSemana: item.NroSemana,
            ProcesadoDesc: item.ProcesadoDesc

        };

        lstData.push(data);
    }
    tblSemMes.loadData(lstData);
}

function cargarHansonTableFeriados(listado) {
    tblFeriados.loadData([]); //hansonTableClear();

    cargarHansonFeriados(tblFeriados, listado);
}

function cargarHansonFeriados(tblFeriados, inputJson) {
    var lstFeriados = inputJson && inputJson.length > 0 ? inputJson : [];

    var lstData = [];
    tblFeriados.loadData(lstData);

    for (var index in lstFeriados) {

        var item = lstFeriados[index];

        var data = {
            PmfrdofechaDesc: item.PmfrdofechaDesc,
            Pmfrdodescripcion: item.Pmfrdodescripcion

        };

        lstData.push(data);
    }
    tblFeriados.loadData(lstData);
}


function guardarAnioOperativo(accion) {
    var dataHandsonSemMes, dataHandsonFeriados, dataValidoSeM, dataValidoFer = [];

    dataHandsonSemMes = tblInicioSemMes.getSourceData();
    dataHandsonFeriados = tblFeriados.getSourceData();

    dataValidoSeM = obtenerData(dataHandsonSemMes, true);
    dataValidoFer = obtenerData(dataHandsonFeriados, true);

    var anio = parseInt($("#txtAnio").val()) || 0;
    var descripcion = $("#txtDescCambios").val() || "";

    var mensaje = "";
    var objDescripcionCambios = {};
    if (accion == EDITAR_ANIO) {
        objDescripcionCambios = getCamposAgregarDescripcion();

        mensaje = validarDatosObligatiosAgregarDescripcion(objDescripcionCambios);
    }

    if (mensaje == "") {

        var dataJson = {
            accion: accion,
            anio: anio,
            fechaIniocioAnio: $('#txtFechaIniAnio').val(),
            descripcion: descripcion,
            listaInicioSemanaMes: dataValidoSeM,
            listaFeriados: dataValidoFer
        };

        $.ajax({
            url: controlador + "RegistrarAnioOperativo",
            type: 'POST',
            contentType: 'application/json; charset=UTF-8',
            dataType: 'json',
            data: JSON.stringify(dataJson),
            success: function (result) {

                if (result.Resultado == "1") {
                    $("#vistaDetalle").html('');//limpiar tab Detalle

                    $('#descripcionCambios').bPopup().close();
                    $('#tab-container').easytabs('select', '#vistaListado');
                    mostrarMensaje_('mensaje', 'exito', 'Año operativo registrado con éxito.');
                    mostrarListadoAniosOperativos();

                } else {
                    $('#descripcionCambios').bPopup().close();
                    mostrarMensaje_('mensaje', 'error', 'Se ha producido un error: ' + result.Mensaje);
                }

            },
            error: function (xhr, status) {
                mostrarMensaje_('mensaje', 'error', 'Se ha producido un error.');
            }
        });
    } else {
        mostrarMensaje_('mensajeDescripcion', 'error', mensaje);
    }
}

function limpiarCamposAgregarDescripcion() {
    $("#txtDescCambios").val("");
}

function getCamposAgregarDescripcion() {
    var obj = {};
    obj.descripcion = $('#txtDescCambios').val();

    return obj;
}

function validarDatosObligatiosAgregarDescripcion(objDesc) {
    var msj = "";

    if (objDesc.descripcion == "") {
        msj += "<p>Debe ingresar una Descripcion de la edición realizada.</p>";
    }

    return msj;
}

function escogerVigente(aniocodi) {
    if (confirm('¿Desea marcar como Vigente al año escogido?')) {
        $.ajax({
            url: controlador + "AprobarVigencia",
            data: {
                aniocodi: aniocodi
            },
            type: 'POST',
            success: function (result) {
                if (result.Resultado === "-1") {
                    mostrarMensaje_('mensaje', 'error', 'Se ha producido un error al marcar vigencia: ' + result.Mensaje);
                } else {
                    mostrarVersiones(parseInt(result.Anio));

                    //mostrarMensaje_('mensaje', 'exito', 'Cambios realizados con éxito.');
                }
            },
            error: function (xhr, status) {
                mostrarMensaje_('mensaje', 'error', 'Se ha producido un error.');
            }
        });
    }
}


function obtenerData(dataHandson, soloFilaValida) {
    var lstData = [];
    for (var index in dataHandson) {
        var item = dataHandson[index];

        lstData.push(item);
    }
    return lstData;
}

function validaDesactivacionFechaInicio(strFechaInicio, id, tienePermisos, esAnioProcesado) {
    var deshabilita = false;

    if (tienePermisos) { //si tiene permisos
        deshabilita = esAnioProcesado;
    } else { // si no tiene permisos
        deshabilita = true;
    }

    cambiarEstadoInput(id, deshabilita);
}


function cambiarEstadoInput(id, estado) {
    document.getElementById(id).disabled = estado;
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



function AddZero(num) {
    return (num >= 0 && num < 10) ? "0" + num : num + "";
}

function diaActual() { //devuelve strFecha en formato dd/mm/yyyy
    var now = new Date();
    var strDateTime = [[AddZero(now.getDate()), AddZero(now.getMonth() + 1), now.getFullYear()].join("/")].join(" ");

    return strDateTime;
}

function cambiarFechaANumero(strFecha) {
    var arrayNumeros = strFecha.split('/');
    var dia = arrayNumeros[0]; // Si desea entero debe usar parseInt(arrayCodigos[0];)
    var mes = arrayNumeros[1];
    var anio = arrayNumeros[2];
    var strNum = anio + mes + dia;

    return parseInt(strNum); //devuelve un entero tipo: 20210108
}

function cambiarFormatoAFecha(strFecha) {
    var arrayNumeros = strFecha.split('/');
    var dia = arrayNumeros[0]; // Si desea entero debe usar parseInt(arrayCodigos[0];)
    var mes = arrayNumeros[1];
    var anio = arrayNumeros[2];
    var strFechaNuevo = anio + "/" + mes + "/" + dia;

    return strFechaNuevo; //devuelve  tipo: 2021/01/31
}

function sleep(ms) {
    return new Promise(resolve => setTimeout(resolve, ms));
}

async function mostrarMensaje_(id, tipo, mensaje) {
    $("#" + id).css("display", "block");
    $('#' + id).removeClass();
    $('#' + id).addClass('action-' + tipo);
    $('#' + id).html(mensaje);

    await sleep(10000);

    limpiarBarraMensaje(id);
}

function limpiarBarraMensaje(id) {
    $('#' + id).css("display", "none");
    $('#' + id).removeClass();
    $('#' + id).addClass('action-message');
    $('#' + id).html('');
}