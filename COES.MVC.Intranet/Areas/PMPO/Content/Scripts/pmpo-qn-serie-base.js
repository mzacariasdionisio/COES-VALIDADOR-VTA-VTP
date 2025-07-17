var controlador = siteRoot + 'PMPO/QnSerieBase/';

var AGREGAR_SB = 1;
var EDITAR_SB = 2;
var DETALLES_SB = 3;

var DE_LISTADO = 1;
var DE_POPUP = 2;

var contenedorHt, tblSeriesBase, listaErrores = [], tblErroresdatos;
var cabecerasHandson = [];

var anioEnDetalle = -1;
var numEstacionesEnDetalle = -1;
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
        var esEditarCrear = parseInt($("#hfAccionDetalle").val()) != DETALLES_SB;

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

    mostrarListadoSeriesBase();

    $('#btnEscogerSerieBase').click(function () {
        limpiarCamposCrearSerie();
        abrirPopup("crearSerie");
    });

    $('#btnCancelarSerie').click(function () {
        $('#crearSerie').bPopup().close();
    });

    $('#btnCrearSerie').click(function () { 
        limpiarBarraMensaje('mensajeCrearSerie');
                
        var obj = {};
        obj = getObjetoCrearSBJson();
        var mensaje = validarDatosPopupCrearSB(obj);
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
            { data: "address", className:"texto_centrado" },
            { data: "valor", className: "texto_centrado"  },
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


function mostrarListadoSeriesBase() {

    $.ajax({
        type: 'POST',
        url: controlador + "ListarSeriesBase",
        dataType: 'json',
        data: {},
        success: function (evt) {
            if (evt.Resultado != "-1") {

                $("#cuadroSeries").html(evt.HtmlListadoSeriesBase);
                refrehDatatable();
                

            } else {
                mostrarMensaje_('mensaje', 'error', 'Se ha producido un error: ' + evt.Mensaje);
            }
        },
        error: function (err) {
            mostrarMensaje_('mensaje', 'error', 'Se ha producido un error.');
        }
    });
}


function refrehDatatable() {
    $('#tabla_SerieBase').dataTable({
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

function mostrarVersiones(anio, tipo) {  
    $('#listadoHSB').html('');

    $.ajax({
        type: 'POST',
        url: controlador + "VersionListado",
        data: {
            anio: anio,
            tipo: tipo
        },
        success: function (evt) {
            if (evt.Resultado != "-1") {
                $('#listadoHSB').html(evt.Resultado);

                $("#listadoHSB").css("width", (820) + "px");

                $('.tabla_version_x_anio_tipo').dataTable({
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

                abrirPopup("historialSB");
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
            anio: parseInt(objDato.anio)
        },
        success: function (resultado) {
            if (resultado.Resultado == "1") {
                $("#anioCrear").css("display", "none");
                $('#crearSerie').bPopup().close();
                mantenerSerieBase(AGREGAR_SB, parseInt(objDato.anio), objDato.tipo, null, DE_LISTADO);
            }  
            if (resultado.Resultado == "-1") {
                mostrarMensaje_('mensajeCrearSerie', 'error', resultado.Mensaje);
            }
            
        },
        error: function (err) {            
            mostrarMensaje_('mensaje', 'error', 'Ha ocurrido un error: ' + err);
        }
    });    
}

function mantenerSerieBase(accion, anio , tipo, qnbenvcodi, origen) {
    $('#tab-container').easytabs('select', '#vistaDetalle');

    anioEnDetalle = anio;
    mostrarVistaDetalles(accion, tipo, qnbenvcodi);

    if (origen == DE_POPUP)
        $('#historialSB').bPopup().close();
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

            if (accion == AGREGAR_SB)
                $("#btnExportar").hide();
            else
                $("#btnExportar").show();

            inicioDetalles(accion, tipo, qnbenvcodi);

        },
        error: function (err) {
            mostrarMensaje_('mensaje', 'error', 'Ha ocurrido un error: ' + err);
        }
    });
}

function inicioDetalles(accion, tipo, qnbenvcodi) {
    limpiarBarraMensaje('mensaje');    
    cargarDatosGeneralesSB(accion, tipo, qnbenvcodi);    
}

function cargarDatosGeneralesSB(accion, tipo, qnbenvcodi) {
    
    $.ajax({
        type: 'POST',
        url: controlador + 'CargarDatosGeneralesSerieBase',
        data: {            
            accion: accion,
            qnbenvcodi: qnbenvcodi
        },

        success: function (evt) {
            if (evt.Resultado != "-1") {

                var anioSerie;
                var strRangoIni = "";
                var strRangoFin = "";
               
                if (accion == AGREGAR_SB) {
                    anioSerie = parseInt($("#txtAnio").val());
                    anioEnDetalle = anioSerie;

                    strRangoFin = anioSerie.toString();
                    strRangoIni = (anioSerie - 2).toString();
                }
                if (accion == EDITAR_SB || accion == DETALLES_SB ) {
                    
                    anioSerie = parseInt(evt.Anio);
                    anioEnDetalle = anioSerie;
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

                //boton Consultar
                $('#btnConsultar').click(function () {
                    
                    limpiarBarraMensaje('mensaje');
                    var obj = {};
                    obj = getRangoFechas();
                    var mensaje = validarRangoSB(obj);
                    if (mensaje == "") {
                        obtenerDatosParaHandsonYMostrar(anioEnDetalle, obj.anioIni, obj.anioFin, tipo, accion, qnbenvcodi);
                    } else {
                        mostrarMensaje_('mensaje', 'error', mensaje);
                    }
                });

                //Descargar Formato
                $('#btnDescargarF').click(function () {                      
                    var data = {
                        tipo: tipo,
                        codEnvio: qnbenvcodi,
                        anio: anioEnDetalle
                    };
                    $.ajax({
                        url: controlador + "DescargarFormatoPlantilla",
                        type: 'POST',
                        contentType: 'application/json; charset=UTF-8',
                        dataType: 'json',
                        data: JSON.stringify(data),
                        success: function (result) {
                            switch (result.Resultado2) {
                                case "1": window.location = controlador + "Exportar?file_name=" + result.Resultado; break;// si hay elementos                                
                                case "-1": mostrarMensaje_('mensaje', 'error',"Error en reporte result"); break;// Error en C#
                            }
                        },
                        error: function (xhr, status) {
                            mostrarMensaje_('mensaje', 'error', 'Se ha producido un error.');
                        }
                    });
                });

                //Importar Formato
                numEstacionesEnDetalle = evt.NumEstaciones;
                importarFormato(tipo, numEstacionesEnDetalle);

                //Guardar
                $("#btnGuardar").click(function () {
                    guardarSerieBase(strRangoIni, strRangoFin, tipo);
                });

                //Exportar Informacion 
                $('#btnExportar').click(function () {
                    exportarInformacion(tipo, qnbenvcodi, anioEnDetalle, accion);
                });

                //Mostrar Errores
                $("#btnMostrarErrores").click(function () {                    
                    mostrarErrores();                                        
                });

                //Ver Envios                
                $("#btnVerHistorial").click(function () {
                    mostrarVersiones(anioEnDetalle, tipo);
                });
                
                obtenerDatosParaHandsonYMostrar(anioEnDetalle, strRangoIni, strRangoFin, tipo, accion, qnbenvcodi);
                
            } else {
                mostrarMensaje_('mensaje', 'error', 'Se ha producido un error: ' + evt.Mensaje);
            }

        },
        error: function (err) {
            mostrarMensaje('mensaje', 'error', 'Se ha producido un error al cargar el listado de identificadores para la fecha escogida.');

        }
    });
}

function guardarSerieBase(strRangoIni, strRangoFin, tipo) {

    if (listaErrores.length > 0) {

        alert('Existen errores en las celdas, favor de corregir y vuelva a presionar "Guardar". A continuación mostramos la lista de errores...');
        $("#btnMostrarErrores").trigger("click");

        return;
    }

    var anioI = parseInt(strRangoIni) || 0;
    var anioF = parseInt(strRangoFin) || 0;

    var dataArray = obtenerLstDatosDelHandson2();
    
    $.ajax({
        type: 'POST',
        url: controlador + 'GrabarSeriesBase',
        data: {
            tipo: tipo,
            anioSerie: anioEnDetalle,
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

                mostrarListadoSeriesBase();                
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

//Devuelve los datos de toda las filas del handson (rangoIni-rangoFin) (DESUSO)
function obtenerLstDatosDelHandson() {

    var data = tblSeriesBase.getSourceData();
    
    var lstdata = [];
    var n = 0;
    
    data.forEach(function (items) {

        var strFila = "";

        var arrayValores = Object.values(data[n]);
        var arrayCampos = Object.keys(data[n]);
        var numColumnas = arrayCampos.length;

        var separador = "";
        for (let index = 0; index < numColumnas; index++) {
            if (index == 0) separador = ""; else separador = "/";
            if (index == 0)
                strFila = strFila + separador + arrayCampos[index] + ":" + arrayValores[index];                
            else
                strFila = strFila + separador + arrayCampos[index] + ":" + arrayValores[index].Valor;
        }
        
        lstdata.push(strFila);

        n++;
    });
    return lstdata;
}

//Devuelve los datos de toda las filas del handson (rangoIni-rangoFin)
function obtenerLstDatosDelHandson2() {
    
    var data2 = tblSeriesBase.getData();
    var lstdata = [];
    var n = 0;

    data2.forEach(function (items) {

        var strFila = "";

        var arrayCab = cabecerasHandson;
        var arrayValores = Object.values(data2[n]);        
        var numColumnas = arrayCab.length;

        var separador = "";
        for (let index = 0; index < numColumnas; index++) {
            if (index == 0) separador = ""; else separador = "/";
            if (index == 0)
                strFila = strFila + separador + arrayCab[index] + ":" + arrayValores[index];
            else
                strFila = strFila + separador + arrayCab[index] + ":" + arrayValores[index];
        }

        lstdata.push(strFila);

        n++;
    });
    return lstdata;
}

function eliminarSerieBase(qnbenvcodi, anio, tipo) {

    var msgConfirmacion = '¿Esta seguro que desea eliminar la Serie Base?'; 

    if (confirm(msgConfirmacion)) {
        $.ajax({
            url: controlador + "EliminarSerieBase",
            data: {
                qnbenvcodi: qnbenvcodi,
                anio: anio, 
                tipo: tipo
            },
            type: 'POST',
            success: function (result) {

                if (result.Resultado === "-1") {
                    mostrarMensaje_('mensaje', 'error', 'Se ha producido un error al eliminar la Serie Base: ' + result.Mensaje);
                } else {
                    //actualizar las fechas popup crear
                    $("#hfNuevoAnioSemanal").val(result.AnioSemanal);
                    $("#hfNuevoAnioMensual").val(result.AnioMensual);

                    mostrarListadoSeriesBase();
                    mostrarMensaje_('mensaje', 'exito', 'Eliminación de la Serie Base realizada con éxito.');
                }
            },
            error: function (xhr, status) {
                mostrarMensaje_('mensaje', 'error', 'Se ha producido un error.');
            }
        });
    }
}


function escogerVigente(qnbenvcodi, tipo, anioSerie) {
    if (confirm('¿Desea marcar como Vigente la Serie Base?')) {
        $.ajax({
            url: controlador + "AprobarVigencia",
            data: {
                qnbenvcodi: qnbenvcodi,
                tipo: tipo,
                anioSerie: anioSerie
            },
            type: 'POST',
            success: function (result) {
                if (result.Resultado === "-1") {
                    mostrarMensaje_('mensaje', 'error', 'Se ha producido un error al marcar vigencia: ' + result.Mensaje);
                } else {
                    mostrarListadoSeriesBase();
                    mostrarVersiones(anioSerie, tipo);
                }
            },
            error: function (xhr, status) {
                mostrarMensaje_('mensaje', 'error', 'Se ha producido un error.');
            }
        });
    }
}


function marcarOficial(qnbenvcodi, tipo, anioSerie) {
    if (confirm('¿Desea asignar como oficial la Serie Base seleccionada?” ')) {
        $.ajax({
            url: controlador + "MarcarOficial",
            data: {
                qnbenvcodi: qnbenvcodi,
                tipo: tipo,
                anioSerie: anioSerie
            },
            type: 'POST',
            success: function (result) {
                if (result.Resultado === "-1") {
                    mostrarMensaje_('mensaje', 'error', 'Se ha producido un error al marcar oficial: ' + result.Mensaje);
                } else {
                    //actualizar las fechas popup crear
                    $("#hfNuevoAnioSemanal").val(result.AnioSemanal);
                    $("#hfNuevoAnioMensual").val(result.AnioMensual);

                    mostrarListadoSeriesBase();
                    mostrarMensaje_('mensaje', 'exito', 'La Serie Base seleccionada se marcó como Oficial.');
                }
            },
            error: function (xhr, status) {
                mostrarMensaje_('mensaje', 'error', 'Se ha producido un error.');
            }
        });
    }
}

function quitarOficial(qnbenvcodi, tipo, anioSerie) {
    if (confirm('¿Desea quitar el identificador Oficial a la Serie Base seleccionada?” ')) {
        $.ajax({
            url: controlador + "QuitarOficial",
            data: {
                qnbenvcodi: qnbenvcodi,
                tipo: tipo,
                anioSerie: anioSerie
            },
            type: 'POST',
            success: function (result) {
                if (result.Resultado === "-1") {
                    mostrarMensaje_('mensaje', 'error', 'Se ha producido un error al quitar oficial: ' + result.Mensaje);
                } else {
                    //actualizar las fechas popup crear
                    $("#hfNuevoAnioSemanal").val(result.AnioSemanal);
                    $("#hfNuevoAnioMensual").val(result.AnioMensual);

                    mostrarListadoSeriesBase();
                    mostrarMensaje_('mensaje', 'exito', 'La Serie Base seleccionada fue desoficializada.');
                }
            },
            error: function (xhr, status) {
                mostrarMensaje_('mensaje', 'error', 'Se ha producido un error.');
            }
        });
    }
}

async function mostrarErrores() {
    
    var listaErrorUnic = [];
    await tblSeriesBase.validateCells(); //para que valide al presionar Errores
    await sleep(1000);  //tiempo espera a validacion
    
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

function importarFormato(tipo, numEstaciones) {

    var uploader = new plupload.Uploader({
        runtimes: 'html5,flash,silverlight,html4',
        browse_button: "btnImportarF",
        url: controlador + "Upload",
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
            },
            UploadComplete: function (up, file) {
                leerFileUpExcel(tipo, numEstaciones);
            },
            Error: function (up, err) {
                mostrarMensaje_('mensaje', 'error', 'Se ha producido un error:' + err.message);
            }
        }
    });

    uploader.init();
}

function leerFileUpExcel(tipo, numEstaciones) {
    
    $.ajax({
        type: 'POST',
        url: controlador + 'LeerArchivoExcelYGuardar',
        dataType: 'json',
        async: true,
        data: {
            anioBase: anioEnDetalle,
            tipo: tipo,
            numEstaciones: numEstaciones
        },
        success: function (resultado) {
            if (resultado.Resultado == "1") {                
                var anioIni = $('#txtRangoDesde').val();
                var anioFin = $('#txtRangoHasta').val();

                //actualiza el Listado pero sigue mostrando Detalles
                mostrarListadoSeriesBase();
                obtenerDatosParaHandsonYMostrar(anioEnDetalle, anioIni, anioFin, tipo, null, resultado.CodigoEnvio);
                mostrarMensaje_('mensaje', 'exito', 'Serie Base guardada correctamente.');
            } else {                
                mostrarMensaje_('mensaje', 'error', resultado.Mensaje);
            }
        },
        error: function (err) {            
            mostrarMensaje_('mensaje', 'error', 'Ha ocurrido un error en leer archivo importado.');
        }
    });
}

function exportarInformacion(tipo, qnbenvcodi, anio, accion) {

    $.ajax({
        type: 'POST',
        url: controlador + "DescargarSerieBaseCompleta",
        data: {
            codEnvio: qnbenvcodi,
            anio: anio,
            tipo: tipo,
            accion: accion
        },
        success: function (evt) {
            if (evt.Resultado != "-1") {
                if (evt.Resultado == "0")                    
                    mostrarMensaje_('mensaje', 'message', evt.Mensaje);
                else
                    window.location = controlador + "Exportar?file_name=" + evt.Resultado;
            } else {
                mostrarMensaje_('mensaje', 'error',"Ha ocurrido un error: " + evt.Mensaje);
            }
        },
        error: function (err) {
            mostrarMensaje_('mensaje', 'error','Ha ocurrido un error.');
        }
    });
}

function obtenerDatosParaHandsonYMostrar(anio, rangoIni, rangoFin, tipo, accion, qnbenvcodi) {
    var accion_ = accion || 0;
    var anioIni = parseInt(rangoIni);
    var anioFin = parseInt(rangoFin);
    $.ajax({
        type: 'POST',
        url: controlador + "ObtenerDatosParaHandson",
        data: {
            anioSerie: anio,
            anioIni: anioIni,
            anioFin: anioFin,
            tipo: tipo,
            qnbenvcodi: qnbenvcodi,
            accion: accion_
        },
        success: function (evt) {
            if (evt.Resultado != "-1") {
                cabecerasHandson = evt.LstCabeceras;
                mostrarHandson(evt.DataHandsonSeriesBase, evt.NumFilas);
                $("#nota").html(evt.NotaVersion);
                codigoBaseEnDetalle = evt.CodigoInfoBase;

                //valida datos (genera lista Error) despues de mostrar tabla
                tblSeriesBase.validateCells();                

            } else {
                //validarCambioDePestaña = false;
                //$('#tab-container').easytabs('select', '#vistaListado');

                //mostrarListadoSeriesBase();
                //limpiarPestañaDetalles();
                mostrarMensaje_('mensaje', 'error',"Ha ocurrido un error: " + evt.Mensaje);
            }
        },
        error: function (err) {
            mostrarMensaje_('mensaje', 'error','Ha ocurrido un error.');
        }
    });
}

function limpiarPestañaDetalles() {
    $("#vistaDetalle").html('');//limpiar tab Detalle
    anioEnDetalle = -1;
    numEstacionesEnDetalle = -1;
    codigoBaseEnDetalle = -1;
}

function mostrarHandson(hot, numFilas) {
    listaErrores = [];
    contenedorHt = document.getElementById('htTablaWeb');

    var nestedHeader = obtenerCabeceraAgupada(hot.NestedHeader.ListCellNestedHeaders);

    tblSeriesBase = new Handsontable(contenedorHt, {
        data: JSON.parse(hot.ListaExcelData2),        
        colWidths: 100,        
        nestedHeaders: nestedHeader,
        columns: hot.Columnas,
        width: '100%',
        height: 400,
        rowHeights: 23,
        rowHeaders: false,  //quita numeración en filas
        fixedColumnsLeft: 1,    

        //maxFilas
        minSpareRows: 0,
        minSpareCols: 0,
        maxRows: numFilas,
    });  

    tblSeriesBase.addHook('afterRender', function () {   
        //Valida (genera lista Error) cada vez que modifico una celda
        tblSeriesBase.validateCells();
    });


    tblSeriesBase.addHook('beforeValidate', function (value, row, prop, source) {
        listaErrores = [];
    });


    //Usado solo para obtener el listado Completo de Errores, para pintar las celdas se usa Renderer (colorCeldaValorRenderer)
    tblSeriesBase.addHook('afterValidate', function (isValid, value, row, prop, source) {        
        var fila = tblSeriesBase.getDataAtCell(row, 0);        
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
            }
        }
        
    });

    
    tblSeriesBase.addHook('afterRenderer', function (TD, row, column, prop, value, cellProperties) {
        //pinta celda cada vez q modifico celda
        cellProperties.renderer = "miRenderCeldas"; //colorear celdas
    });


    tblSeriesBase.addHook('beforeRenderer', function (TD, row, column, prop, value, cellProperties) {
        //pinta celda al mostrar tabla
        cellProperties.renderer = "miRenderCeldas"; //colorear celdas
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
        if (tblSeriesBase != undefined) {
                        
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


function obtenerCabeceraAgupada(lstNestedHeaders) {

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
    var anio = "";
    if (opcion == 0) { //seleccione tipo
        $("#anioCrear").css("display", "none");
    } else {
        $("#anioCrear").css("display", "block");
        var currentTime = new Date();
        var anioActual = currentTime.getFullYear().toString();

        if (opcion == 1) { //tipo : semanal
            anio = $("#hfNuevoAnioSemanal").val();

            if (anio == "1") // cuando no existe oficial, muestra este año
                $("#txtAnio").val(anioActual);
            else 
                $("#txtAnio").val($("#hfNuevoAnioSemanal").val());
        }
        if (opcion == 2) { //tipo : mensual
            anio = $("#hfNuevoAnioMensual").val();

            if (anio == "1")// cuando no existe oficial, muestra este año
                $("#txtAnio").val(anioActual);
            else
                $("#txtAnio").val($("#hfNuevoAnioMensual").val());                        
        } 
        
    }

    if (anio == "1") {// cuando no existe oficial muestra todos los años
        $('#txtAnio').Zebra_DatePicker({
            format: 'Y',            
        });
    } else {// cuando  existe oficial muestra dos años siguientes
        $('#txtAnio').Zebra_DatePicker({
            format: 'Y',
            direction: [(parseInt(anio) ).toString(), (parseInt(anio) + 1).toString()],
        });
    }
    
}

function getObjetoCrearSBJson() {
    var obj = {};

    obj.tipo = parseInt($("#tipoSerie").val()) || 0;
    obj.anio = parseInt($("#txtAnio").val()) || 0;

    return obj;
}


function validarDatosPopupCrearSB(obj) {

    var msj = "";
    if (obj.tipo == 0) {
        msj += "<p>Debe seleccionar un tipo.</p>";
    }

    if (obj.anio == 0) {
        msj += "<p>Debe ingresar año.</p>";
    }    

    return msj;
}

function getRangoFechas() {
    var obj = {};

    obj.anioIni = $("#txtRangoDesde").val() ;
    obj.anioFin = $("#txtRangoHasta").val() ;

    return obj;
}


function validarRangoSB(obj) {

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

    await sleep(8000);

    limpiarBarraMensaje(id);
}

function limpiarBarraMensaje(id) {
    $('#' + id).css("display", "none");
    $('#' + id).removeClass();
    $('#' + id).addClass('action-message');
    $('#' + id).html('');
    
}