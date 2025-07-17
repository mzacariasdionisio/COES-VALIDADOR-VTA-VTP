var controlador = siteRoot + 'YupanaContinuo/CompromisoHidraulico/';

/** CONSTANTES */
var VIENE_DE_CONSULTA = 1;

var TIPO_SIN_COMPROMISO = 1;
var TIPO_CON_COMPROMISO = 2;

var PESTANIA_SIN_COMP = 0;
var PESTANIA_CON_COMP = 1
var PESTANIA_AMBAS = 2;

var FORMATO_SC = 128;
var FORMATO_CC = 127;

var PREFIJO_SC = "SC";
var PREFIJO_CC = "CC";


/** Variables */
var numEnviosSC = -1;
var numEnviosCC = -1;

var htmlCambiosSC = "";
var htmlCambiosCC = "";

var permisoParaEditarSC = false; //para validar si tiene permitido editar (pestaña SC)
var permisoParaGuardarSC = false; //para validar si tiene permitido guardar (pestaña SC)
var permisoParaEditarCC = false; //para validar si tiene permitido editar (pestaña cC)
var permisoParaGuardarCC = false; //para validar si tiene permitido guardar (pestaña cC)



$(function () {
    
    $('#txtFecha').Zebra_DatePicker({
        onSelect: function () {
            $("#mensaje_compr_alerta").hide();

            $("#hfIdEnvioSC").val(0); //obtenedremos el ultimoEnvio en el controller
            $("#hfIdEnvioCC").val(0); //obtenedremos el ultimoEnvio en el controller
            cargarListado(VIENE_DE_CONSULTA, PESTANIA_AMBAS);
        }

    });

    var sFecha = "";
    if ($("#txtFecha").val() != "") {
        sFecha = $('#txtFecha').val();
        fechaActual = new Date();
    }
    else {
        alert("Error!.Ingresar fecha correcta");
    }

    $('#tab-container').easytabs('select', '#vistaSinComp');

    cargarListado(0, PESTANIA_AMBAS);


    $("#btnGuardarData" + PREFIJO_SC).css("display", "none");
    $("#btnGuardarData" + PREFIJO_CC).css("display", "none");


    $('#btnEditarDataSC').click(function () {
        permitirGuardar(PREFIJO_SC);
    }); 
    $('#btnEditarDataCC').click(function () {
        permitirGuardar(PREFIJO_CC);
    }); 


    $('#btnGuardarDataSC').click(function () {   
        if (permisoParaGuardarSC)
            guardarDataTabla(TIPO_SIN_COMPROMISO);
        else
            alert("El envío no tiene permiso para guardar.");
    });
    $('#btnGuardarDataCC').click(function () {
        if (permisoParaGuardarCC)
            guardarDataTabla(TIPO_CON_COMPROMISO);
        else
            alert("El envío no tiene permiso para guardar.");
    });


    $('#btnVerHistorialSC').click(function () {        
        popUpListaEnvios(TIPO_SIN_COMPROMISO);        
    });        
    $('#btnVerHistorialCC').click(function () {
        popUpListaEnvios(TIPO_CON_COMPROMISO);
    });

    
});




/**
 * Muestra el listado
 */
function cargarListado(origen, pestania_) { 
    
    origen = parseInt(origen) || 0;
    var msj = "";
    var obj = {};
    
    var prefijo = "";

    if (origen == VIENE_DE_CONSULTA) {
        obj = getObjetoJsonConsulta();
        msj = validarConsulta(obj);        
    }

    if (msj == "") {

        $.ajax({
            url: controlador + "CargarTablaCompromisoHidraulico",
            data: {
                pestania: pestania_, 
                fechaConsulta: $("#txtFecha").val(),
                idEnvioSC: $("#hfIdEnvioSC").val(),
                idEnvioCC: $("#hfIdEnvioCC").val()
            },
            type: 'POST',
            success: function (result) {
                if (result.Resultado === "-1") {
                    mostrarMensaje_('mensaje', 'error', 'Se ha producido un error al cargar el listado de Compromiso Hidráulico. Error: ' + result.Mensaje);
                    //mostrarMensaje('mensaje', 'error', 'Se ha producido un error: ' + result.Mensaje);
                } else {

                    $('#hfFecha').val($('#txtFecha').val());
                    
                    //activarEdicionDeLaTabla(!result.TieneRegistroPrevio);

                    if (pestania_ == PESTANIA_SIN_COMP) {
                        $('#tab-container').easytabs('select', '#vistaSinComp');

                        mostrarPestania(result.NumEnviosSC, result.HtmlSC, PREFIJO_SC, "contenidoSinCompromiso", "tblSinCompromiso");
                        $('#versionMostradaSC').text(result.VersionFechaSC);
                        htmlCambiosSC = result.HtmlCambiosSC;
                        //numEnviosSC = result.NumEnviosSC; 
                        darEstilosSegunVersionMostrada(PREFIJO_SC, result.NumEnviosSC, result.EsUltimaVersionSC);
                        
                    } else {
                        if (pestania_ == PESTANIA_CON_COMP) {
                            $('#tab-container').easytabs('select', '#vistaConComp');

                            mostrarPestania(result.NumEnviosCC, result.HtmlCC, PREFIJO_CC, "contenidoConCompromiso", "tblConCompromiso");
                            $('#versionMostradaCC').text(result.VersionFechaCC);
                            htmlCambiosCC = result.HtmlCambiosCC;
                            //numEnviosCC = result.NumEnviosCC;
                            darEstilosSegunVersionMostrada(PREFIJO_CC, result.NumEnviosCC, result.EsUltimaVersionCC);
                            
                        } else {
                            if (pestania_ == PESTANIA_AMBAS) {
                                $('#tab-container').easytabs('select', '#vistaSinComp');

                                mostrarPestania(result.NumEnviosSC, result.HtmlSC, PREFIJO_SC, "contenidoSinCompromiso", "tblSinCompromiso");
                                $('#versionMostradaSC').text(result.VersionFechaSC);
                                htmlCambiosSC = result.HtmlCambiosSC;
                                //numEnviosSC = result.NumEnviosSC;  
                                darEstilosSegunVersionMostrada(PREFIJO_SC, result.NumEnviosSC, result.EsUltimaVersionSC);
                                                              

                                mostrarPestania(result.NumEnviosCC, result.HtmlCC, PREFIJO_CC, "contenidoConCompromiso", "tblConCompromiso");
                                $('#versionMostradaCC').text(result.VersionFechaCC);
                                htmlCambiosCC = result.HtmlCambiosCC;
                                //numEnviosCC = result.NumEnviosCC;
                                darEstilosSegunVersionMostrada(PREFIJO_CC, result.NumEnviosCC, result.EsUltimaVersionCC);
                                
                            }
                        }
                    }
                    
                    
                }
            },
            error: function (xhr, status) {
                mostrarMensaje_('mensaje', 'error', 'Se ha producido un error al cargar las tablas de compromiso hidráulico.');                
            }
        });
    } else {
        alert(msj);
    }
}


function darEstilosSegunVersionMostrada(prefijo, numEnvios, esUltimaVersion) {

    if (esUltimaVersion == true) { //hay envios y se mostrará la ultima version
        $("#btnEditarData" + prefijo).css("display", "block");
       
        if (prefijo == PREFIJO_SC) permisoParaEditarSC = true;
        if (prefijo == PREFIJO_CC) permisoParaEditarCC = true;     
        bloquearTabla(prefijo);

        $("#btnGuardarData" + prefijo).css("display", "none");
        if (prefijo == PREFIJO_SC) permisoParaGuardarSC = false;
        if (prefijo == PREFIJO_CC) permisoParaGuardarCC = false;
    }
    else {
        if (numEnvios == 0) { //el dia seleccionado no tiene envios

            $("#btnGuardarData" + prefijo).css("display", "block");
            if (prefijo == PREFIJO_SC) permisoParaGuardarSC = true;
            if (prefijo == PREFIJO_CC) permisoParaGuardarCC = true;

            $("#btnEditarData" + prefijo).css("display", "none");
            if (prefijo == PREFIJO_SC) permisoParaEditarSC = false;
            if (prefijo == PREFIJO_CC) permisoParaEditarCC = false;

        } else {//hay envios y se mostrará cualquier version, menos la ultima
            $("#btnEditarData" + prefijo).css("display", "none");
            if (prefijo == PREFIJO_SC) permisoParaEditarSC = false;
            if (prefijo == PREFIJO_CC) permisoParaEditarCC = false;
            bloquearTabla(prefijo);

            $("#btnGuardarData" + prefijo).css("display", "none");
            if (prefijo == PREFIJO_SC) permisoParaGuardarSC = false;
            if (prefijo == PREFIJO_CC) permisoParaGuardarCC = false;
        }
        
    }
    
}

function bloquearTabla(prefijo) {

    //Bloqueo del check cabecera
    $('input[type=checkbox][name^="checkd_todo' + prefijo + '_"]').each(function () {        
        document.getElementsByName(this.name)[0].disabled = true;        
    });

    //bloqueo de los check del cuerpo
    $('input[type=checkbox][name^="checkcomph' + prefijo + '_"]').each(function () {
        $("#" + this.id).attr('disabled', true);
    });
}


function permitirGuardar(prefijo) {
    var tienePermisoEditar = false;

    if (prefijo == PREFIJO_SC) tienePermisoEditar = permisoParaEditarSC;            
    if (prefijo == PREFIJO_CC) tienePermisoEditar = permisoParaEditarCC;        
    

    if (tienePermisoEditar) {
        activarTabla(prefijo);
        $("#btnGuardarData" + prefijo).css("display", "block");
        $("#btnEditarData" + prefijo).css("display", "none");

        if (prefijo == PREFIJO_SC) permisoParaGuardarSC = true;        
        if (prefijo == PREFIJO_CC) permisoParaGuardarCC = true;
        
       
    } else {
        alert("El envío no tiene permiso para edición.");
    }
    
}

function activarTabla(prefijo) {

    //Activar del check cabecera
    $('input[type=checkbox][name^="checkd_todo' + prefijo + '_"]').each(function () {
        document.getElementsByName(this.name)[0].disabled = false;
    });

    //Activar de los check del cuerpo
    $('input[type=checkbox][name^="checkcomph' + prefijo + '_"]').each(function () {
        $("#" + this.id).attr('disabled', false);
    });
}

function mostrarPestania(numEnvios, tablaHtml,  prefijo, contenido, tbl) {
    
    if (prefijo == "SC") {
        $("#" + contenido).show();
        $("#" + tbl).html(tablaHtml);
        $('#tabla_' + prefijo).dataTable({
            "scrollX": true,
            //"scrollY": "350px",
            "scrollCollapse": true,
            "sDom": 't',
            "ordering": false,
            paging: false,
        });
    }
    if (prefijo == "CC") {
        $("#" + contenido).show();
        $("#" + tbl).html(tablaHtml);
        $('#tabla_' + prefijo).dataTable({
            //"scrollX": true,
            //"scrollY": "350px",
            "scrollCollapse": true,
            "sDom": 't',
            "ordering": false,
            paging: false,
        });
    }

    //Toda la columna cambia (al escoger casilla cabecera)
    $('input[type=checkbox][name^="checkd_todo' + prefijo + '_"]').unbind();
    $('input[type=checkbox][name^="checkd_todo' + prefijo + '_"]').change(function () {        
        var nota = "";
        var idchecktodo = $(this).attr('name') + '';        
        var codigo = idchecktodo.substring(14, idchecktodo.length); //ptomedicodi_emprcodi
        var valorCheck = $(this).prop('checked');
        nota = valorCheck ? "Desmarcar Todo" : "Marcar Todo";
        $('#' + idchecktodo).text(nota);
        $("input[type=checkbox][id$=_" + codigo + "]").each(function () {
            $("#" + this.id).prop("checked", valorCheck);
        });
    });

    //Todo debajo debe cambiar (al escoger una casilla)
    $('input[type=checkbox][name^="checkcomph' + prefijo + '_"]').unbind();
    $('input[type=checkbox][name^="checkcomph' + prefijo + '_"]').change(function () {

        var idcheckSeleccionado = $(this).attr('id') + '';
        var codigos = idcheckSeleccionado.substring(13, idcheckSeleccionado.length); //#fila_ptomedicodi_emprcodi

        var arrayCodigos = codigos.split('_');
        var filaSeleccionada = arrayCodigos[0];
        var valorCheck = $(this).prop('checked');
        var empresa_central = arrayCodigos[1] + "_" + arrayCodigos[2];

        $("input[type=checkbox][id$=_" + empresa_central + "]").each(function () {
            var idCheck = this.id;
            var arrayCheck = idCheck.split('_');
            var filaCheck = arrayCheck[1];
            if (parseInt(filaSeleccionada) <= parseInt(filaCheck)) {
                $("#" + this.id).prop("checked", valorCheck);
            }

        });
    });

    //Cuando para el dia no existen registros guardados,  se selecciona segun la hora actual
    if (numEnvios == 0) {
        //Todo debajo debe estar seleccionado, respecto a la hora actual
        var filaSeleccionada = obtenerFilaInicioSegunHora();
       
        $('input[type=checkbox][name^="checkcomph' + prefijo + '_"]').each(function () {
            var idCheck = this.id;
            var arrayCheck = idCheck.split('_');
            var filaCheck = arrayCheck[1];
            if (parseInt(filaSeleccionada) <= parseInt(filaCheck)) {
                $("#" + this.id).prop("checked", true);
            }
        });
    }
}

function obtenerFilaInicioSegunHora() {
        
    var hoy = moment().format("HH");    
    var hora = parseInt(hoy);
    var fila = hora + 1;

    return fila;
}

/**
 * Parametros para consulta
 * */
function getObjetoJsonConsulta() {
    var obj = {};

    obj.consulta_fecha = $("#txtFecha").val() || 0;

    return obj;
}

/**
 * Validar que existan todos los parametros al momento de hacer la consulta
 */
function validarConsulta(obj) {
    var msj = "";

    if (obj.consulta_fecha == 0) {
        msj += "Debe ingresar una fecha correcta para realizar la consulta." + "\n";
    } else {
        
    }

    return msj;
}

function guardarDataTabla(tipoCompromiso) {

    var formato = -1;
    var pestaniaMostrar = -1;
    var prefijo = "";
    if (tipoCompromiso == TIPO_SIN_COMPROMISO) {
        formato = FORMATO_SC;
        pestaniaMostrar = PESTANIA_SIN_COMP;
        prefijo = PREFIJO_SC;
    }
    if (tipoCompromiso == TIPO_CON_COMPROMISO) {
        formato = FORMATO_CC;
        pestaniaMostrar = PESTANIA_CON_COMP;
        prefijo = PREFIJO_CC;
    }

    $('#hfFecha').val($('#txtFecha').val());
        
    if (confirm("¿Desea guardar la información?")) {
        
        var dataValido = listarNoCheckDeUsuario(tipoCompromiso);
                
        var dataJson = {
            lstNoSeleccionados: dataValido,
            idEmpresa: -1,
            formatoReal: formato,
            fechaConsulta: $('#hfFecha').val() 
        };

        $.ajax({
            url: controlador + "GuardarDataTablaCompromiso",
            data: JSON.stringify(dataJson),
            type: 'POST',
            contentType: 'application/json; charset=UTF-8',
            success: function (evt) {
                $("#mensaje_compr_alerta").hide();

                if (evt.Resultado == 1) {
                    $("#hfIdEnvio" + prefijo).val(evt.IdEnvio);

                    cargarListado(0, pestaniaMostrar);
                    mostrarMensaje_('mensaje', 'error', "Los datos se enviaron correctamente. ");
                }
                else {
                    
                    mostrarMensaje_('mensaje', 'error', "Error al Grabar");
                }
            },
            error: function () {
                mostrarMensaje_('mensaje', 'error', "Ocurrió un error. ");
            }
        });
        
    }
}

function listarNoCheckDeUsuario(tipoCompromiso) {
    var listaData = [];
    var prefijo = "";

    if (tipoCompromiso == TIPO_SIN_COMPROMISO) prefijo = PREFIJO_SC;
    if (tipoCompromiso == TIPO_CON_COMPROMISO) prefijo = PREFIJO_CC;

    $('input[type=checkbox][id^="checkcomph' + prefijo +'_"]:not(:checked)').each(function () {

        var idCheck = $(this).attr('id') + '';
        var arr = idCheck.split('_');

        var idFila = arr[1];
        var idPtoMedicion = arr[2];
        var idEmpresa = arr[3];

        var strFecha = $("#hfTdFecha" + prefijo + "_" + idFila).val();
        var strH = $("#hfTdh" + prefijo + "_" + idFila).val();

        var entity = {
            Emprcodi: idEmpresa,
            PtoMedicion: idPtoMedicion,
            FechaDesc: strFecha,
            Hx: strH,
            Flag: 0            
        };

        listaData.push(entity);
    });

    return listaData;
}


function popUpListaEnvios(tipoCompromiso) {
    var prefijo = "";
    var htmlMostrar = "";

    if (tipoCompromiso == TIPO_SIN_COMPROMISO) { prefijo = PREFIJO_SC; htmlMostrar = htmlCambiosSC; }
    if (tipoCompromiso == TIPO_CON_COMPROMISO) { prefijo = PREFIJO_CC; htmlMostrar = htmlCambiosCC; }
    
    var lista = "";
    
    $('#idEnviosAnteriores' + prefijo).html(htmlMostrar); 
    setTimeout(function () {
        $('#enviosanteriores' + prefijo).bPopup({
            easing: 'easeOutBack',
            speed: 450,
            transition: 'slideDown',
            modalClose: false
        });
        $('#tablalenvio' + prefijo).dataTable({
            "scrollY": 330,
            "scrollX": true,
            "sDom": 't',
            "ordering": false,
            "bPaginate": false,
            "iDisplayLength": -1
        });
    }, 50);
}

function mostrarEnvioExcelWeb(tipoCompromiso, envio) {
    

    if (tipoCompromiso == PESTANIA_SIN_COMP) {
        $('#enviosanterioresSC').bPopup().close();
        $("#hfIdEnvioSC").val(envio);
        cargarListado(0, PESTANIA_SIN_COMP);
    }
    if (tipoCompromiso == PESTANIA_CON_COMP) {
        $('#enviosanterioresCC').bPopup().close();
        $("#hfIdEnvioCC").val(envio);
        cargarListado(0, PESTANIA_CON_COMP); 
    }
 
}


/**
 * Muestra mensajes de notificación
 */
function mostrarMensaje_(id, tipo, mensaje) {
    //$('#' + id).removeClass();
    //$('#' + id).addClass('action-' + tipo);
    //$('#' + id).html(mensaje);

    alert(mensaje);
};