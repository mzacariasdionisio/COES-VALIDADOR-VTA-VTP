var controlador = siteRoot + 'PMPO/QnMensual/';

var AGREGAR_SH = 1;
var EDITAR_SH = 2;
var DETALLES_SH = 3;

var ORIGEN_BASE = 1;
var ORIGEN_HISTORICO = 2;
var ORIGEN_PRONOSTICADO = 3;
var ORIGEN_AUTOCOMPLETADO = 4;
var ORIGEN_EDITADA_USUARIO = 5;
var cabecerasHandson = [];

var DE_LISTADO = 1;
var DE_POPUP = 2;

var contenedorHt, tblSeriesHidrologica, listaErrores = [], listaErroresSinVacios = [], tblErroresdatos, tblErroresdatosSinVacios;

var anioEnDetalle = -1;
var mesEnDetalle = -1;
var tipoEnDetalle = -1;
var codigoBaseEnDetalle = -1;

var validarCambioDePestaña = true;

$(function () {

    $('#tab-container').easytabs({
        animate: false,
        select: '#vistaListado'
    });
    
    $('#tab-container').bind('easytabs:before', function () {
        var esTabDetalle = $("#tab-container .tab.active").html().toLowerCase().includes('detalle');
        var existeHtmlTabDetalle = $("#vistaDetalle").html().trim() != '';
        var esEditarCrear = parseInt($("#hfAccionDetalle").val()) != DETALLES_SH;
        if (validarCambioDePestaña) {            
            if (esTabDetalle && existeHtmlTabDetalle && esEditarCrear) {
                if (confirm('¿Desea cambiar de pestaña? Si selecciona "Aceptar" se perderán los cambios. Si selecciona "Cancelar" se mantendrá en la misma pestaña')) {
                    $("#vistaDetalle").html(''); //limpiar tab Detalle                    
                } else {
                    return false;
                }
            }
        }
        validarCambioDePestaña = true;
    });

    mostrarListadoSeriesHidrologicas();

    $('#btnEscogerSerieHidrologica').click(function () {
        limpiarCamposCrearSerie();
        abrirPopup("crearSerie");
    });

    $('#btnCancelarSerieH').click(function () {
        $('#crearSerie').bPopup().close();
    });

    //notificación
    $('#txtMesAnioNotificacion').Zebra_DatePicker({
        format: 'm Y',
    });
    $('#btnPopupNotificar').click(function () {
        $('#popupNotificar').bPopup({
            easing: 'easeOutBack',
            speed: 450,
            transition: 'slideDown',
            modalClose: false
        });
    });
    $('#btnNotificarPendientes').click(function () {
        notificarPendientes();
    });

    $('#btnCrearSerieH').click(function () {
        limpiarBarraMensaje('mensajeCrearSerie');
        
        var obj = {};
        obj = getObjetoCrearSBJson();
        var mensaje = validarDatosPopupCrearSH(obj);
        if (mensaje == "") {
            validaNuevaSerie(obj);
            
        } else {
            mostrarMensaje_('mensajeCrearSerie', 'error', mensaje);
        }
    });

    $('#tipoSerie').change(function () {
        var tipoSeleccionado = $('#tipoSerie').val();
        actualizarAnioCrearSerie(tipoSeleccionado);
    });

    tblErroresdatos = $('#tblListaerrores').DataTable({
        "data": [],
        "columns": [
            { data: "className", visible: false },
            { data: "address", className: "texto_centrado" },
            { data: "valor", className: "texto_centrado" },
            { data: "message" }
        ],
        "filter": false,
        "scrollCollapse": true,
        "paging": false,
        "scrollY": 430,
        "createdRow": function (row, data, dataIndex) {
            $(row).find('td').eq(1).addClass(data.className);
        },

    });

    tblErroresdatosSinVacios = $('#tblListaerroresSinVacios').DataTable({
        "data": [],
        "columns": [
            { data: "className", visible: false },
            { data: "address", className: "texto_centrado" },
            { data: "valor", className: "texto_centrado" },
            { data: "message" }
        ],
        "filter": false,
        "scrollCollapse": true,
        "paging": false,
        "scrollY": 430,
        "createdRow": function (row, data, dataIndex) {
            $(row).find('td').eq(1).addClass(data.className);
        },
    });
});


function mostrarListadoSeriesHidrologicas() {

    $.ajax({
        type: 'POST',
        url: controlador + "ListarSeriesHidrologica",
        dataType: 'json',
        data: {},
        success: function (evt) {
            if (evt.Resultado != "-1") {

                $("#cuadroSeries").html(evt.HtmlListadoSeriesHidrologica);
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
    $('#tabla_SerieHidrologica').dataTable({
        "scrollY": 430,
        "scrollX": true,
        "sDom": 't',
        "ordering": false,
        "iDisplayLength": 50
    });
}

function limpiarCamposCrearSerie() {
    $('#tipoSerie').val(0);
    $("#anioCrear").css("display", "none");
}

function mostrarVersiones(anio, mes, tipo) {
    $('#listadoHSH').html('');

    $.ajax({
        type: 'POST',
        url: controlador + "VersionListado",
        data: {
            anio: anio,
            mes: mes,
            tipo: tipo
        },
        success: function (evt) {
            if (evt.Resultado != "-1") {
                $('#listadoHSH').html(evt.Resultado);

                $("#listadoHSH").css("width", (820) + "px");                

                abrirPopup("historialSH");
            } else {

                mostrarMensaje_('mensaje', 'error', 'Ha ocurrido un error al listar las versiones :' + evt.Mensaje);
            }
        },
        error: function (err) {

            mostrarMensaje_('mensaje', 'error', 'Ha ocurrido un error.');
        }
    });
}

function validaNuevaSerie(objDato) {
    
    $.ajax({
        type: 'POST',
        url: controlador + "ValidacionNuevaSerie",
        data: {
            tipo: objDato.tipo,
            anio: parseInt(objDato.anio),
            mes: parseInt(objDato.mes),
        },
        success: function (resultado) {
            if (resultado.Resultado == "1") {                
                $("#anioCrear").css("display", "none");
                $('#crearSerie').bPopup().close();
                mantenerSerieHidrologica(AGREGAR_SH, parseInt(objDato.anio), parseInt(objDato.mes), objDato.tipo, null, DE_LISTADO);
            }
            if (resultado.Resultado == "-1") {
                mostrarMensaje_('mensajeCrearSerie', 'error', resultado.Mensaje);
            }
        },
        error: function (err) {
            mostrarMensaje_('mensaje', 'error', 'Ha ocurrido un error:' + err);
        }
    });
    
}

function mantenerSerieHidrologica(accion, anio, mes, tipo, qnbenvcodi, origen) {
    $('#tab-container').easytabs('select', '#vistaDetalle');

    anioEnDetalle = anio;
    mesEnDetalle = mes;
    mostrarVistaDetalles(accion, tipo, qnbenvcodi);

    if (origen == DE_POPUP)
        $('#historialSH').bPopup().close();
}

function mostrarVistaDetalles(accion, tipo, qnbenvcodi) {
    $("#vistaDetalle").html('');
    qnbenvcodi = qnbenvcodi || 0;

    $.ajax({
        type: 'POST',
        url: controlador + "CargarDetalles",
        data: {
            accion: accion,
            qnbenvcodi: qnbenvcodi
        },
        success: function (evt) {

            $('#vistaDetalle').html(evt);
            tipoEnDetalle = tipo;
            inicioDetalles(accion, tipo, qnbenvcodi);

        },
        error: function (err) {
            mostrarMensaje_('mensaje', 'error', 'Ha ocurrido un error: ' + err);
        }
    });
}

function inicioDetalles(accion, tipo, qnbenvcodi) {
    limpiarBarraMensaje('mensaje');
    cargarDatosGeneralesSH(accion, tipo, qnbenvcodi);
}


function cargarDatosGeneralesSH(accion, tipo, qnbenvcodi) {

    $.ajax({
        type: 'POST', 
        url: controlador + 'CargarDatosGeneralesSerieHidrologica',
        data: {
            accion: accion,
            qnbenvcodi: qnbenvcodi
        },

        success: function (evt) {
            if (evt.Resultado != "-1") {

                var anioSerie;
                var strRangoIni = "";
                var strRangoFin = "";

                if (accion == AGREGAR_SH) {
                    var strAnio = ($("#txtMesAnio").val()).split(' ')[1];
                    anioSerie = parseInt(strAnio);

                    anioEnDetalle = anioSerie;
                    mesEnDetalle = parseInt(($("#txtMesAnio").val()).split(' ')[0]);

                    strRangoFin = (anioSerie + 1).toString();
                    strRangoIni = (anioSerie - 4).toString();
                }
                if (accion == EDITAR_SH || accion == DETALLES_SH) {

                    anioSerie = parseInt(evt.Anio);
                    anioEnDetalle = anioSerie;
                    mesEnDetalle = parseInt(evt.Mes);
                    strRangoIni = evt.RangoIni;
                    strRangoFin = evt.RangoFin;
                }

                $('#txtRangoDesde').val(strRangoIni);
                $('#txtRangoHasta').val(strRangoFin);

                $('#txtRangoDesde').Zebra_DatePicker({
                    format: 'Y',
                    pair: $('#txtRangoHasta'),
                    direction: -1
                });

                $('#txtRangoHasta').Zebra_DatePicker({
                    format: 'Y',
                    pair: $('#txtRangoDesde'),
                    direction: [true, strRangoFin]
                });

                //botón consultar
                $('#btnConsultar').click(function () {

                    limpiarBarraMensaje('mensaje');
                    var obj = {};
                    obj = getRangoFechas();
                    var mensaje = validarRangoSH(obj);
                    if (mensaje == "") {
                        obtenerDatosParaHandsonYMostrar(anioEnDetalle, mesEnDetalle, obj.anioIni, obj.anioFin, tipo, accion, qnbenvcodi);
                    } else {
                        mostrarMensaje_('mensaje', 'error', mensaje);
                    }
                });

                //Guardar
                $("#btnGuardar").click(function () {
                    guardarSerieHidrologica(strRangoIni, strRangoFin, tipo);
                });

                //Exportar Informacion 
                $('#btnExportar').click(function () {
                    exportarInformacion(tipo, qnbenvcodi, anioEnDetalle, mesEnDetalle, accion);
                });

                //Mostrar Errores
                $("#btnMostrarErrores").click(function () {
                    mostrarErrores();
                });

                //Generar Dat
                $('#btnGenerarDat').click(function () {
                    generarArchivoDat(qnbenvcodi, accion);
                });

                //Boton Cargar Histórico
                $("#btnCargarHistorico").click(function () {
                    actualizarBloqueDelHandsonSegunOrigen(anioEnDetalle, mesEnDetalle, strRangoIni, strRangoFin, tipo, accion, qnbenvcodi, ORIGEN_HISTORICO);
                });

                //Boton Cargar Pronóstico
                $("#btnCargarPronostico").click(function () {
                    actualizarBloqueDelHandsonSegunOrigen(anioEnDetalle, mesEnDetalle, strRangoIni, strRangoFin, tipo, accion, qnbenvcodi, ORIGEN_PRONOSTICADO);
                });

                //Boton Autocompletar
                $("#btnAutocompletar").click(function () {
                    actualizarBloqueDelHandsonSegunOrigen(anioEnDetalle, mesEnDetalle, strRangoIni, strRangoFin, tipo, accion, qnbenvcodi, ORIGEN_AUTOCOMPLETADO);
                });
                
                obtenerDatosParaHandsonYMostrar(anioEnDetalle, mesEnDetalle, strRangoIni, strRangoFin, tipo, accion, qnbenvcodi);

            } else {
                mostrarMensaje_('mensaje', 'error', 'Se ha producido un error: ' + evt.Mensaje);
            }

        },
        error: function (err) {
            mostrarMensaje('mensaje', 'error', 'Se ha producido un error al cargar el listado de identificadores para la fecha escogida.');

        }
    });
}

function guardarSerieHidrologica(strRangoIni, strRangoFin, tipo) {

    if (listaErrores.length > 0) {
        alert('Existen errores en las celdas, favor de corregir y vuelva a presionar "Guardar". A continuación mostramos la lista de errores...');
        $("#btnMostrarErrores").trigger("click");

        return;
    }

    var anioI = parseInt(strRangoIni) || 0;
    var anioF = parseInt(strRangoFin) || 0;

    var dataArray = obtenerLstDatosDelHandson();

    $.ajax({
        type: 'POST',
        url: controlador + 'GrabarSerieHidrologica',
        data: {
            tipo: tipo,
            anioSerie: anioEnDetalle,
            mesSerie: mesEnDetalle,
            anioIni: anioI,
            anioFin: anioF,
            codigoBase: codigoBaseEnDetalle,

            stringJson: JSON.stringify(dataArray)
        },
        datatype: 'json',
        success: function (evt) {
            if (evt.Resultado != "-1") {
                limpiarPestañaDetalles();
                validarCambioDePestaña = false;
                $('#tab-container').easytabs('select', '#vistaListado');

                //actualizo anios en popup (al crear nuevo) 
                $("#hfNuevoMesAnioSemanal").val(evt.MesAnioSemanal);
                $("#hfNuevoMesAnioMensual").val(evt.MesAnioMensual);

                mostrarListadoSeriesHidrologicas();                
                mostrarMensaje_('mensaje', 'exito', 'Serie Base guardada correctamente.');
            } else {
                mostrarMensaje_('mensaje', 'error', "Ha ocurrido un error: " + evt.Mensaje);
            }
        },
        error: function (err) {
            mostrarMensaje_('mensaje', 'error', "Ha ocurrido un error: ");
        }
    });

}

function limpiarPestañaDetalles() {    

    $("#vistaDetalle").html(''); //limpiar tab Detalle
    anioEnDetalle = -1;
    mesEnDetalle = -1;
    tipoEnDetalle = -1;
    codigoBaseEnDetalle = -1;
}

function actualizarBloqueDelHandsonSegunOrigen(anio, mes, rangoIni, rangoFin, tipo, accion, qnbenvcodi, numBotonOrigen) {
    var nombBoton = "";

    switch (numBotonOrigen) {
        case ORIGEN_HISTORICO: nombBoton = "Cargar Histórico"; break;
        case ORIGEN_PRONOSTICADO: nombBoton = "Cargar Pronóstico"; break;
        case ORIGEN_AUTOCOMPLETADO: nombBoton = "Autocompletar"; break;
    }

    if (listaErroresSinVacios.length > 0) {
        alert('Existen errores en las celdas, favor de corregir y vuelva a presionar "' + nombBoton + '". A continuación mostramos la lista de errores...');
        mostrarErroresSinVacios();

        return;
    }

    var accion_ = accion || 0;
    var anioIni_ = parseInt(rangoIni);
    var anioFin_ = parseInt(rangoFin);

    var dataArray = obtenerLstDatosDelHandson();

    $.ajax({
        type: 'POST',
        url: controlador + 'ActualizarBloqueHandsonSegunOrigen',
        data: {
            anioSerie: anio,
            mesSerie: mes,
            anioIni: anioIni_,
            anioFin: anioFin_,
            tipo: tipo,
            accion: accion_,
            origen: numBotonOrigen,
            codigoBase: codigoBaseEnDetalle,
            stringJson: JSON.stringify(dataArray),
            qnbenvcodi: qnbenvcodi,

        },
        datatype: 'json',
        success: function (evt) {
            if (evt.Resultado != "-1") {
                mostrarHandson(evt.DataHandsonSeriesHidrologica, evt.NumFilas);

                //valida datos (genera lista Error) despues de mostrar tabla
                tblSeriesHidrologica.validateCells();
            } else {
                mostrarMensaje_('mensaje', 'error', "Ha ocurrido un error: " + evt.Mensaje);
            }
        },
        error: function (err) {
            mostrarMensaje_('mensaje', 'error', "Ha ocurrido un error: ");
        }
    });
}

function eliminarSerieHidrologica(qnbenvcodi, anio, mes, tipo) {

    var msgConfirmacion = '¿Esta seguro que desea eliminar la Serie Hidrologica?';

    if (confirm(msgConfirmacion)) {
        $.ajax({
            url: controlador + "EliminarSerieHidrologica",
            data: {
                qnbenvcodi: qnbenvcodi,
                anio: anio,
                mes: mes,
                tipo: tipo
            },
            type: 'POST',
            success: function (result) {

                if (result.Resultado === "-1") {
                    mostrarMensaje_('mensaje', 'error', 'Se ha producido un error al eliminar la Serie Hidrológica: ' + result.Mensaje);
                } else { 
                    //actualizo anios en popup (despues de eliminar SH)
                    $("#hfNuevoMesAnioSemanal").val(result.MesAnioSemanal);
                    $("#hfNuevoMesAnioMensual").val(result.MesAnioMensual);

                    mostrarListadoSeriesHidrologicas();
                    mostrarMensaje_('mensaje', 'exito', 'Eliminación de la Serie Hidrológica realizada con éxito.');
                }
            },
            error: function (xhr, status) {
                mostrarMensaje_('mensaje', 'error', 'Se ha producido un error.');
            }
        });
    }
}


function escogerVigente(qnbenvcodi, tipo, anioSerie, mesSerie) {
    if (confirm('¿Desea marcar como Vigente la Serie Hidrológica?')) {
        $.ajax({
            url: controlador + "AprobarVigencia",
            data: {
                qnbenvcodi: qnbenvcodi,
                tipo: tipo,
                anioSerie: anioSerie,
                mesSerie: mesSerie
            },
            type: 'POST',
            success: function (result) {
                if (result.Resultado === "-1") {
                    mostrarMensaje_('mensaje', 'error', 'Se ha producido un error al marcar vigencia: ' + result.Mensaje);
                } else {
                    mostrarListadoSeriesHidrologicas();
                    mostrarVersiones(anioSerie, mesSerie, tipo); 
                }
            },
            error: function (xhr, status) {
                mostrarMensaje_('mensaje', 'error', 'Se ha producido un error.');
            }
        });
    }
}


async function mostrarErrores() {
    await tblSeriesHidrologica.validateCells(); //para que valide al presionar Errores
    await sleep(1000);  //tiempo espera a validacion

    var listaErrorUnic = [];
    abrirPopup("erroresDatos");

    $.each(listaErrores, function (key, value) {
        var result = listaErrorUnic.filter(x => x.address === value.address);
        if (result.length === 0) {
            listaErrorUnic.push(value);
        }
    });

    tblErroresdatos.clear().draw();
    tblErroresdatos.rows.add(listaErrorUnic).draw();
}

async function mostrarErroresSinVacios() {
    await tblSeriesHidrologica.validateCells(); //para que valide al presionar Errores
    await sleep(1000);  //tiempo espera a validacion

    var listaErrorUnic = [];
    abrirPopup("erroresDatosSinVacio");

    $.each(listaErroresSinVacios, function (key, value) {
        var result = listaErrorUnic.filter(x => x.address === value.address);
        if (result.length === 0) {
            listaErrorUnic.push(value);
        }
    });

    tblErroresdatosSinVacios.clear().draw();
    tblErroresdatosSinVacios.rows.add(listaErrorUnic).draw();
}

function exportarInformacion(tipo, qnbenvcodi, anio, mes, accion) {

    $.ajax({
        type: 'POST',
        url: controlador + "DescargarSerieHidroCompleta",
        data: {
            codEnvio: qnbenvcodi,
            anio: anio,
            mes: mes,
            tipo: tipo,
            codigoBase: codigoBaseEnDetalle,
            accion: accion
        },
        success: function (evt) {
            if (evt.Resultado != "-1") {
                if (evt.Resultado == "0")
                    mostrarMensaje_('mensaje', 'message', evt.Mensaje);
                else
                    window.location = controlador + "Exportar?file_name=" + evt.Resultado;
            } else {
                mostrarMensaje_('mensaje', 'error', "Ha ocurrido un error: " + evt.Mensaje);
            }
        },
        error: function (err) {
            mostrarMensaje_('mensaje', 'error', 'Ha ocurrido un error.');
        }
    });
}

function obtenerDatosParaHandsonYMostrar(anio, mes, rangoIni, rangoFin, tipo, accion, qnbenvcodi) {
    var accion_ = accion || 0;
    var anioIni = parseInt(rangoIni);
    var anioFin = parseInt(rangoFin);
    $.ajax({
        type: 'POST',
        url: controlador + "ObtenerDatosParaHandson",
        data: {
            anioSerie: anio,
            mesSerie: mes,
            anioIni: anioIni,
            anioFin: anioFin,
            tipo: tipo,
            accion: accion_,
            qnbenvcodi: qnbenvcodi,
            
        },
        success: function (evt) {
            if (evt.Resultado != "-1") {

                //actualizo el listado de series hidrologicas
                if (accion == AGREGAR_SH) {
                    mostrarListadoSeriesHidrologicas();
                }

                cabecerasHandson = evt.LstCabeceras;

                //muestro handson
                mostrarHandson(evt.DataHandsonSeriesHidrologica, evt.NumFilas);
                $("#nota").html(evt.NotaVersion);
                codigoBaseEnDetalle = evt.CodigoInfoBase;

                //actualizo anios en popup (en caso de crear SH se autoguarda)
                $("#hfNuevoMesAnioSemanal").val(evt.MesAnioSemanal);
                $("#hfNuevoMesAnioMensual").val(evt.MesAnioMensual);

                //Muestro Nota (si es necesario)
                if (evt.NotaNuevoOficial != "") {
                    $("#mensajeNuevoOficial").css("display", "block");
                    
                } else {
                    $("#mensajeNuevoOficial").css("display", "none");
                }
                $("#mensajeNuevoOficial").html(evt.NotaNuevoOficial);

                //valida datos (genera lista Error) despues de mostrar tabla
                tblSeriesHidrologica.validateCells(); 

            } else {   
                //validarCambioDePestaña = false;
                //$('#tab-container').easytabs('select', '#vistaListado');
                //mostrarListadoSeriesHidrologicas();
                //limpiarPestañaDetalles();
                
                mostrarMensaje_('mensaje', 'error', "Ha ocurrido un error: " + evt.Mensaje);
            }
        },
        error: function (err) {
            mostrarMensaje_('mensaje', 'error', 'Ha ocurrido un error.');
        }
    });
}

function mostrarHandson(hot, numFilas) {
    listaErrores = [];
    listaErroresSinVacios = [];
    contenedorHt = document.getElementById('htTablaWeb');

    var nestedHeader = obtenerCabeceraAgrupada(hot.NestedHeader.ListCellNestedHeaders);

    tblSeriesHidrologica = new Handsontable(contenedorHt, {
        data: JSON.parse(hot.ListaExcelData2),
        colWidths: 100,
        nestedHeaders: nestedHeader,
        columns: hot.Columnas,
        width: '100%',
        height: 400,
        rowHeights: 23,
        rowHeaders: false,  //quita numeración en filas
        fixedColumnsLeft: 1,
        cell: JSON.parse(hot.Esquema),

        //maxFilas
        minSpareRows: 0,
        minSpareCols: 0,
        maxRows: numFilas,
    });

    tblSeriesHidrologica.updateSettings({
        beforeKeyDown(e) {
            //si se presiona ciertos caracteres
            if (e.keyCode === 106 || e.keyCode === 111 || e.keyCode === 190) { //char: '*', '/', ':'
                //no los muestra
                e.stopImmediatePropagation();
                e.preventDefault();
            }

        }
    });


    tblSeriesHidrologica.addHook('afterRender', function () {
        //Valida (genera lista Error) cada vez que modifico una celda
        tblSeriesHidrologica.validateCells();
    });


    tblSeriesHidrologica.addHook('beforePaste', function () {
        //Valida (genera lista Error) cada vez que modifico una celda
        tblSeriesHidrologica.validateCells();
    });


    tblSeriesHidrologica.addHook('beforeValidate', function (value, row, prop, source) {
        listaErrores = [];
        listaErroresSinVacios = [];
    });

    //Usado solo para obtener el listado Completo de Errores, para pintar las celdas se usa Renderer (colorCeldaValorRenderer)
    tblSeriesHidrologica.addHook('afterValidate', function (isValid, value, row, prop, source) {        
        var fila = tblSeriesHidrologica.getDataAtCell(row, 0);
        var numSddp = prop.split('.')[0];

        if (parseFloat(value) < 0) {
            var error = [];

            error = {

                address: `<b>${fila}</b>&nbsp;&nbsp;&nbsp; / &nbsp;&nbsp;&nbsp;<b>${numSddp}</b>`,
                valor: value,
                message: "El dato es negativo",
                className: "errorLimitInferior"
            };

            listaErrores.push(error);
            listaErroresSinVacios.push(error);
        }

        if (!value || value === '') {
            if (parseFloat(value) != 0) {

                var error1 = [];

                error1 = {
                    address: `<b>${fila}</b>&nbsp;&nbsp;&nbsp; / &nbsp;&nbsp;&nbsp;<b>${numSddp}</b>`,
                    valor: value,
                    message: "Campo vacío",
                    className: "errorVacio"
                };

                listaErrores.push(error1);
            }
        } else {
            if (!$.isNumeric(value)) {
                var error2 = [];

                error2 = {
                    address: `<b>${fila}</b>&nbsp;&nbsp;&nbsp; / &nbsp;&nbsp;&nbsp;<b>${numSddp}</b>`,
                    valor: value,
                    message: "El dato no es numérico",
                    className: "errorNoNumerico"
                };

                listaErrores.push(error2);
                listaErroresSinVacios.push(error2);
            }
        }

    });


    tblSeriesHidrologica.addHook('afterRenderer', function (TD, row, column, prop, value, cellProperties) {
        //pinta celda cada vez q modifico celda
        cellProperties.renderer = "miRenderCeldas"; //colorear celdas
    });


    tblSeriesHidrologica.addHook('beforeRenderer', function (TD, row, column, prop, value, cellProperties) {
        //pinta celda al mostrar tabla
        cellProperties.renderer = "miRenderCeldas"; //colorear celdas
    });


    //pinta celda cada vez q modifico celda  
    tblSeriesHidrologica.addHook('afterChange', function (source) {
        var dataHandson = tblSeriesHidrologica.getSourceData();

        for (var i = 0; i < source.length; i++) {        
            var filaCambiada = (source[i])[0];
            var propiedadCambiada = ((source[i])[1]).split('.')[0];
            var valorAntiguo = (source[i])[2];
            var valorNuevo = (source[i])[3];
            var miprop = (source[i])[1];
            var columna = tblSeriesHidrologica.propToCol(miprop);
            var data = dataHandson[filaCambiada];
            var datosCelda = data[propiedadCambiada];

            //si el valor es editado cambia su origen
            if (valorAntiguo != valorNuevo) {

                var nombClase = "sinFormato";
                if (datosCelda.ValDefecto != datosCelda.Valor) {
                    datosCelda.Origen = ORIGEN_EDITADA_USUARIO;
                    nombClase = 'celdaInfoPorUsuario';                    
                } else {
                    datosCelda.Origen = datosCelda.OrigenDefecto;
                    switch (datosCelda.OrigenDefecto) {

                        case ORIGEN_BASE: nombClase = 'celdaInfoBase'; break;
                        case ORIGEN_HISTORICO: nombClase = 'celdaInfoHistorico'; break;
                        case ORIGEN_PRONOSTICADO: nombClase = 'celdaInfoPronosticado'; break;
                        case ORIGEN_AUTOCOMPLETADO: nombClase = 'celdaInfoAutoCompletado'; break;
                    }
                }
                this.setCellMeta(filaCambiada, columna, 'className', nombClase);                
            }
        }
        this.render();
    });
}


(function (Handsontable) {
    // Register an alias    
    Handsontable.renderers.registerRenderer('miRenderCeldas', colorCeldaValorRenderer);

})(Handsontable);

//Cambia de color las celdas segun se vayan mostrando (respecto al scroll) en la tabla web 
function colorCeldaValorRenderer(instance, td, row, col, prop, value, cellProperties) {    
    Handsontable.renderers.NumericRenderer.apply(this, arguments);

    if (col != 0) { //no valida 1ra columna (ya que son no numericos)
        if (tblSeriesHidrologica != undefined) {

            if (parseFloat(value) < 0) {
                td.className = 'errorLimitInferior';
            }

            if (!value || value === '') {
                if (parseFloat(value) != 0)
                    td.className = 'errorVacio';
            } else {
                if (!$.isNumeric(value)) {
                    td.className = 'errorNoNumerico';
                }
            }
        }
    }

}


//Devuelve los datos de las filas del handson (Todo menos "Informacion Base", es decir, Historico, Pronosticado y autocompletado)
function obtenerLstDatosDelHandson() {

    var data = tblSeriesHidrologica.getSourceData();
    var lstdata = [];
    var n = 0;

    data.forEach(function (items) {

        var strFila = "";

        var arrayDatos = Object.values(data[n]);
        var arrayCampos = Object.keys(data[n]);
        var numColumnas = arrayCampos.length - 1;

        if (arrayDatos[numColumnas] != "B") { //TODO menos Info Base

            var separador = "";
            for (let index = 0; index < numColumnas; index++) {
                if (index == 0) separador = ""; else separador = "/";
                if (index == 0)
                    strFila = strFila + separador + arrayCampos[index] + ":" + arrayDatos[index];
                else
                    strFila = strFila + separador + arrayCampos[index] + ":" + arrayDatos[index].Valor + "*" + arrayDatos[index].Origen;                
            }

            lstdata.push(strFila);
        }
        n++;
    });
    return lstdata;
}


function obtenerCabeceraAgrupada(lstNestedHeaders) {

    var nestedHeaders = [];

    lstNestedHeaders.forEach(function (currentValue, index, array) {
        var nestedHeader = [];
        currentValue.forEach(function (item) {
            if (item.Colspan == 0) {
                nestedHeader.push(item.Label);
            } else {
                nestedHeader.push({ label: item.Label, colspan: item.Colspan });
            }
        });

        nestedHeaders.push(nestedHeader);
    });

    return nestedHeaders;
}

function actualizarAnioCrearSerie(opcion) {
    var mesAnio = "";
    if (opcion == 0) { //seleccione tipo
        $("#anioCrear").css("display", "none");
    } else {
        $("#anioCrear").css("display", "block");

        if (opcion == 3) { //tipo : semanal
            mesAnio = $("#hfNuevoMesAnioSemanal").val();

            $("#txtMesAnio").val(($("#hfNuevoMesAnioSemanal").val()).replace('*', ''));
        }
        if (opcion == 4) { //tipo : mensual
            mesAnio = $("#hfNuevoMesAnioMensual").val();

            $("#txtMesAnio").val(($("#hfNuevoMesAnioMensual").val()).replace('*', ''));
        }
    }

    if (mesAnio.includes("*")) {// cuando no existe registros anteriores
        $('#txtMesAnio').Zebra_DatePicker({
            format: 'm Y',
        });
    } else {// cuando  ya existe registrados
        $('#txtMesAnio').Zebra_DatePicker({
            format: 'm Y',
            direction: [mesAnio.replace('*', ''), "12 2100"],
        });
    }

}

function generarArchivoDat(qnbenvcodi_, accion ) { 
    qncodi = qnbenvcodi_ || 0;
    $.ajax({
        type: 'POST',
        url: controlador + "GenerarArchivoDat",
        data: {
            qnbenvcodi: qncodi,
            tipo: tipoEnDetalle,
            anio: anioEnDetalle, 
            mes: mesEnDetalle,
            codigoBase: codigoBaseEnDetalle, 
            accion: accion
        },
        success: function (evt) {
            if (evt.Resultado != "-1") {
                window.location = controlador + "Exportar?file_name=" + evt.Resultado;
            } else {
                mostrarMensaje_('mensaje', 'error', evt.Mensaje);
            }
        },
        error: function (err) {

            mostrarMensaje_('mensaje', 'error', 'Ha ocurrido un error.');
        }
    });

}

function getObjetoCrearSBJson() {
    var obj = {};
    var txtanio = $("#txtMesAnio").val();
    var arrayNumeros = txtanio.split(' ');
    var anio = "0";
    var mes = "0";

    if (arrayNumeros.length > 0) {
        anio = arrayNumeros[1];
        mes = arrayNumeros[0];
    }

    obj.tipo = parseInt($("#tipoSerie").val()) || 0;
    obj.aniomes = txtanio || "0";
    obj.anio = anio;
    obj.mes = mes;

    return obj;
}


function validarDatosPopupCrearSH(obj) {

    var msj = "";
    if (obj.tipo == 0) {
        msj += "<p>Debe seleccionar un tipo.</p>";
    }

    if (obj.aniomes == "0") {
        msj += "<p>Debe ingresar año.</p>";
    }

    return msj;
}

function getRangoFechas() {
    var obj = {};

    obj.anioIni = $("#txtRangoDesde").val();
    obj.anioFin = $("#txtRangoHasta").val();

    return obj;
}


function validarRangoSH(obj) {

    var msj = "";
    if (obj.anioIni == "") {
        msj += "<p>Debe Ingresar rango Inicial.</p>";
    }

    if (obj.anioFin == "") {
        msj += "<p>Debe Ingresar rango Final.</p>";
    }

    return msj;
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

function sleep(ms) {
    return new Promise(resolve => setTimeout(resolve, ms));
}

async function mostrarMensaje_(id, tipo, mensaje) {
    $("#" + id).css("display", "block");
    $('#' + id).removeClass();
    $('#' + id).addClass('action-' + tipo);
    $('#' + id).html(mensaje);    

    await sleep(6000);

    limpiarBarraMensaje(id);
}

function limpiarBarraMensaje(id) {
    $('#' + id).css("display", "none");
    $('#' + id).removeClass();
    $('#' + id).addClass('action-message');
    $('#' + id).html('');   
}

// Notificación
function notificarPendientes() {
    var mesElaboracion = $("#txtMesAnioNotificacion").val();
    var msjAlert = "¿Desea enviar notificación de envíos pendientes a los agentes?";

    if (confirm(msjAlert)) {
        $.ajax({
            type: 'POST',
            url: controlador + "NotificarPendiente",
            dataType: 'json',
            data: {
                mesElaboracion: mesElaboracion
            },
            success: function (evt) {
                if (evt.Resultado != "-1") {
                    if (evt.Resultado != "0") {
                        mostrarMensaje_('mensajeNotificar', 'exito', 'Se enviarón las notificaciones a los agentes.');

                        if (evt.NameFileLog != null && evt.NameFileLog != '') {
                            window.location.href = controlador + 'DescargarLogNotificacion?archivo=' + evt.NameFileLog;
                        }
                    } else {
                        mostrarMensaje_('mensajeNotificar', 'exito', 'No existen envíos pendientes. No se enviará notificaciones.');
                    }
                } else {
                    mostrarMensaje_('mensajeNotificar', 'error', evt.Mensaje);
                }
            },
            error: function (err) {
                mostrarMensaje_('mensajeNotificar', 'error', 'Ha ocurrido un error.');
            }
        });
    }
}
