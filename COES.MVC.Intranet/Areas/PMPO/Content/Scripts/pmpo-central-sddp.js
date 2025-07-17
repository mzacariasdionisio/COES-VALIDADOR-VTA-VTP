var controlador = siteRoot + 'PMPO/CentralSDDP/';

const AGREGAR_CS = 1;
const EDITAR_CS = 2;
const DETALLES_CS = 3;

const AGREGAR = 1;
const EDITAR = 2;
const ELIMINAR = 4;

const DE_LISTADO = 1;
const DE_POPUP = 2;

const RECALCULAR = 1;
const MOSTRAR_GUARDADO = 0;

var TIENE_PERMISO_NUEVO = false;
var PTOS_SEMANAL = 1;
var PTOS_MENSUAL = 2;

var validarCambioDePestaña = true;

var tblEvaporacion1, tblEvaporacion2, tblVolArea;
var containerEvaporacion1, containerEvaporacion2, containerVolArea;

var listaCHidroEnDetalle = [];
var listaEmbalseEnDetalle = [];
var recurcodiEnDetalle;

var topologiaAMostrar;
var accionEscenario;



$(function () {    
    topologiaAMostrar = parseInt($("#ipTopologiaMostrada").val()) || 0;   
    accionEscenario = $("#accionEsc").val();  

    $('#tab-container').easytabs({
        animate: false,
        select: '#vistaListado'
    });

    $('#tab-container').bind('easytabs:before', function () {
        var esTabDetalle = $("#tab-container .tab.active").html().toLowerCase().includes('detalle');
        var existeHtmlTabDetalle = $("#vistaDetalle").html().trim() != '';
        var esEditarCrear = parseInt($("#hfAccionDetalle").val()) != DETALLES_CS;

        if (validarCambioDePestaña) {
            if (esTabDetalle && existeHtmlTabDetalle && esEditarCrear) {
                if (confirm('¿Desea cambiar de pestaña? Si selecciona "Aceptar" se perderán los cambios. Si selecciona "Cancelar" se mantendrá en la misma pestaña')) {
                    $("#vistaDetalle").html(''); //limpiar tab Detalle
                    limpiarBarraMensaje('mensaje');
                } else {
                    return false;
                }
            }
        }
        validarCambioDePestaña = true;
    });

    mostrarListadoCentralesSddp(topologiaAMostrar, accionEscenario);

    $('#chkReordenarCentrales').click(function () {
        if ($(this).is(':checked')) {
            var oTable = $('#tabla_Centrales').dataTable();
            $("#tabla_Centrales .ui-sortable").sortable("enable");
            oTable.rowReordering({ sURL: controlador + "UpdateOrder"});
        } else {

            $("#tabla_Centrales .ui-sortable").sortable("disable");
        }
    });

    $('#btnAgregarRelacion').click(function () {
        limpiarBarraMensaje('mensaje');

        mantenerCentralSddp(AGREGAR_CS, topologiaAMostrar, null, DE_LISTADO);
    });
    
    $('#btnExportar').click(function () {
        exportarCentralesSddp(topologiaAMostrar);
    });

    $('#cmbPopupAddEmbalse').change(function () {
        cargarPtosPorFormato(AGREGAR);
    }); 

    $('#cmbPopupEditEmbalse').change(function () {
        cargarPtosPorFormato(EDITAR);
    });

    $('#btnActualizarListadoCHAdd').click(function () {
        actualizarListadoCentralesHidro();
    });
    

    $('#btnIrListadoCentralHidroAdd').click(function () {
        irANuevasCentralesHidro();
    });
    $('#btnIrListadoCentralHidroEdit').click(function () {
        irANuevasCentralesHidro();
    });

    $('#btnVerCodigosSddp').click(function () {
        window.open(siteRoot + 'PMPO/CodigoSDDP/Index', '_blank').focus();
    });
});


function mostrarListadoCentralesSddp(topcodi, accionEsce) {
    var dataJson = {
        topcodi: topcodi,     
        permisoLectura: accionEsce,
        strListaCH: $("#hfLstCH").val(),
        strListaEmb: $("#hfLstEmb").val()
    };
    limpiarBarraMensaje('mensaje');
    $.ajax({
        
        url: controlador + "ListarCentralesSppd",
        type: 'POST',        
        contentType: 'application/json; charset=UTF-8',
        dataType: 'json',
        data: JSON.stringify(dataJson),
        success: function (evt) {
            if (evt.Resultado != "-1") {                
                $("#cuadroCentralesSddp").html(evt.HtmlListadoCentralesSDDP);
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
    $('#tabla_Centrales').dataTable({
        "scrollY": 400,
        "scrollX": true,
        "sDom": 'ft',
        "iDisplayLength": -1
    });
}

function mantenerCentralSddp(accion, topcodi, centralCodi, origen) {
    $('#tab-container').easytabs('select', '#vistaDetalle');
    mostrarVistaDetalles(accion,topcodi, centralCodi, origen);
}

function mostrarVistaDetalles(accion, topcodi, recurcodi, origen) {
    $("#vistaDetalle").html(''); //limpiar tab Detalle
    recurcodi = recurcodi || 0;
    
    $.ajax({       
        type: 'POST',
        url: controlador + "CargarDetalles",
        data: {
            accion: accion,
            topcodi: topcodi,
            recurcodi: recurcodi,
            strListaCH: $("#hfLstCH").val(),
            strListaEmb: $("#hfLstEmb").val()
        },
        success: function (evt) {
            $('#vistaDetalle').html(evt);
            inicioDetalles(accion, topcodi, recurcodi);
        },
        error: function (err) {
            alert('Ha ocurrido un error: ' + err);
        }
    });

}

function inicioDetalles(accion, topcodi, recurcodi) {
    limpiarBarraMensaje('mensaje');
    cargarDatosDeCentralSddp(accion, topcodi, recurcodi);
}

function cargarDatosDeCentralSddp(accion, topcodi, recurcodi) {
    recurcodi = recurcodi || 0;

    var dataJson = {
        accion: accion,
        topcodi: topcodi,
        recurcodi: recurcodi,
        strListaCH: $("#hfLstCH").val(),
        strListaEmb: $("#hfLstEmb").val()
    };
    limpiarBarraMensaje('mensaje');
    $.ajax({        
        url: controlador + 'CargarDatosCentralSddp',
        type: 'POST',
        contentType: 'application/json; charset=UTF-8',
        dataType: 'json',
        data: JSON.stringify(dataJson),
        success: function (evt) {
            if (evt.Resultado != "-1") {
                

                TIENE_PERMISO_NUEVO = evt.TienePermisoNuevo;

                recurcodiEnDetalle = recurcodi;

                setearValoresDeCentral();

                bloqueCentralesHidroelectricas(accion, recurcodi, evt);

                bloqueEmbalses(accion, recurcodi, evt);

                administrarVisibilidadCampos(accion);

                $("#btnGrabarCentralSddp").click(function () {
                    guardarCentralSddp(AGREGAR_CS, topcodi);
                });

                $("#btnActualizarCentralSddp").click(function () {
                    guardarCentralSddp(EDITAR_CS, topcodi);
                });

                $("#btnCancelarCentralSddp").click(function () {
                    $("#vistaDetalle").html(''); //limpiar tab Detalle

                    validarCambioDePestaña = false;
                    $('#tab-container').easytabs('select', '#vistaListado');

                    mostrarListadoCentralesSddp(topcodi, accionEscenario);
                });


            } else {
                mostrarMensaje_('mensaje', 'error', 'Se ha producido un error: ' + evt.Mensaje);
            }

        },
        error: function (err) {
            mostrarMensaje('mensaje', 'error', 'Se ha producido un error al cargar los datos de la central SDDP.');

        }
    });
}

function administrarVisibilidadCampos(accion) {
    
    if (accion == DETALLES_CS) {
        document.getElementById("txtNombreSddp").disabled = true;
        document.getElementById("txtDescripcionSddp").disabled = true;
        document.getElementById("txtEstacionHidroAsociada").disabled = true;
        document.getElementById("txtTurbinamiento").disabled = true;
        document.getElementById("txtVertimiento").disabled = true;

        document.getElementById("rdCHSicoesSi").disabled = true;
        document.getElementById("rdCHSicoesNo").disabled = true;
        document.getElementById("rdCHExistente").disabled = true;
        document.getElementById("rdCHFutura").disabled = true;
        document.getElementById("rdCHParalelo").disabled = true;
        document.getElementById("rdCHCascada").disabled = true;

        document.getElementById("txtCHPotencia").disabled = true;
        document.getElementById("txtCHCoefProduccion").disabled = true;
        document.getElementById("txtCHCaudalMin").disabled = true;
        document.getElementById("txtCHCaudalMax").disabled = true;
        document.getElementById("txtCHFIForzada").disabled = true;
        document.getElementById("txtCHFIHistorica").disabled = true;
        document.getElementById("txtCHCostoOM").disabled = true;
        //
        document.getElementById("rdEPasada").disabled = true;
        document.getElementById("rdEAlmacenamiento").disabled = true;
        document.getElementById("txtEFactorEmpuntamiento").disabled = true;
        document.getElementById("rdESicoesSi").disabled = true;
        document.getElementById("rdESicoesNo").disabled = true;
        document.getElementById("txtEVolMin").disabled = true;
        document.getElementById("txtEVolMax").disabled = true;
        document.getElementById("chkEAdicVolIni").disabled = true;
        document.getElementById("chkEAjusVolMin").disabled = true;
        document.getElementById("chkEAjusVolIni").disabled = true;
        document.getElementById("rdEControlableSi").disabled = true;
        document.getElementById("rdEControlableNo").disabled = true;
        document.getElementById("rdEControlableParcialmente").disabled = true;

    } else {

        document.getElementById("txtNombreSddp").disabled = false;
        document.getElementById("txtDescripcionSddp").disabled = false;
        document.getElementById("txtEstacionHidroAsociada").disabled = false;
        document.getElementById("txtTurbinamiento").disabled = false;
        document.getElementById("txtVertimiento").disabled = false;

        document.getElementById("rdCHSicoesSi").disabled = false;
        document.getElementById("rdCHSicoesNo").disabled = false;
        document.getElementById("rdCHExistente").disabled = false;
        document.getElementById("rdCHFutura").disabled = false;
        document.getElementById("rdCHParalelo").disabled = false;
        document.getElementById("rdCHCascada").disabled = false;

        
        document.getElementById("txtCHCaudalMin").disabled = false;        
        document.getElementById("txtCHFIForzada").disabled = false;
        document.getElementById("txtCHFIHistorica").disabled = false;
        

        //
        document.getElementById("rdEPasada").disabled = false;
        document.getElementById("rdEAlmacenamiento").disabled = false;
        document.getElementById("txtEFactorEmpuntamiento").disabled = false;
        document.getElementById("rdESicoesSi").disabled = false;
        document.getElementById("rdESicoesNo").disabled = false;
        document.getElementById("txtEVolMin").disabled = false;
        document.getElementById("txtEVolMax").disabled = false;
        document.getElementById("chkEAdicVolIni").disabled = false;
        document.getElementById("chkEAjusVolMin").disabled = false;
        document.getElementById("chkEAjusVolIni").disabled = false;
        document.getElementById("rdEControlableSi").disabled = false;
        document.getElementById("rdEControlableNo").disabled = false;
        document.getElementById("rdEControlableParcialmente").disabled = false;

    }

}

function setearValoresDeCentral() {
    //Datos generales
    $('#txtNombreSddp').val($('#hfNombreSddp').val());
    
    $('#txtCodigoSddp').html($('#hfCodigoSddp').val());

    //Topologia
    $('#txtEstacionHidroAsociada').val($('#hfEstacionHidroAsociada').val());
    $('#txtTurbinamiento').val($('#hfTurbinamiento').val());
    $('#txtVertimiento').val($('#hfVertimiento').val());    

    //radio SICOES en Central hidroelectrica
    var valorCHSiCoes = $('#hfrdCHSicoes').val();
    if (valorCHSiCoes == 0)
        document.getElementById('rdCHSicoesNo').setAttribute('checked', '');
    if (valorCHSiCoes == 1)
        document.getElementById('rdCHSicoesSi').setAttribute('checked', '');

    //radio tipo Central hidroelectrica (Existente:0, Futura:1)
    var valorTipoCentralH = $('#hfrdTipoCentral').val();
    if (valorTipoCentralH == 0)
        document.getElementById('rdCHExistente').setAttribute('checked', '');
    if (valorTipoCentralH == 1)
        document.getElementById('rdCHFutura').setAttribute('checked', '');

    //radio tipo Conexion Central hidroelectrica (Paralelo:0, Cascada:1)
    var valorTipoConexionCH = $('#hfrdTipoConexion').val();
    if (valorTipoConexionCH == 0)
        document.getElementById('rdCHParalelo').setAttribute('checked', '');
    if (valorTipoConexionCH == 1)
        document.getElementById('rdCHCascada').setAttribute('checked', '');

    //radio tipo Embalse (CentralPasada:0, Almacenamiento:1)
    var valorTipoEmbalse = $('#hfrdTipoEmbalse').val();
    if (valorTipoEmbalse == 0)
        document.getElementById('rdEPasada').setAttribute('checked', '');
    if (valorTipoEmbalse == 1)
        document.getElementById('rdEAlmacenamiento').setAttribute('checked', '');

    //radio SICOES en Embalse (No:0, SI:1)
    var valorESiCoes = $('#hfrdESicoes').val();
    if (valorESiCoes == 0)
        document.getElementById('rdESicoesNo').setAttribute('checked', '');
    if (valorESiCoes == 1)
        document.getElementById('rdESicoesSi').setAttribute('checked', '');

    //checbox "Adicionar el volumen mínimo" (No:0, Si:1)
    var valorCbxAdicVolIni = $('#chkEAdicVolIni').val();
    if (valorCbxAdicVolIni == 0)
        $('#chkEAdicVolIni').prop('checked', false);
    if (valorCbxAdicVolIni == 1)
        $('#chkEAdicVolIni').prop('checked', true);

    //checbox "Ajustar volumen mínimo" (No:0, Si:1)
    var valorCbxAjusVolMin = $('#chkEAjusVolMin').val();
    if (valorCbxAjusVolMin == 0)
        $('#chkEAjusVolMin').prop('checked', false);
    if (valorCbxAjusVolMin == 1)
        $('#chkEAjusVolMin').prop('checked', true);

    //checbox "Ajustar volumen inicial" (No:0, Si:1)
    var valorCbxAjusVolIni = $('#chkEAjusVolIni').val();
    if (valorCbxAjusVolIni == 0)
        $('#chkEAjusVolIni').prop('checked', false);
    if (valorCbxAjusVolIni == 1)
        $('#chkEAjusVolIni').prop('checked', true);

    //radio tipo vertimiento (Controlable:0, No controlable:1, Parcialmente controlable:2)
    var valorTipoVertimiento = $('#hfrdTipoVertimiento').val();
    if (valorTipoVertimiento == 0)
        document.getElementById('rdEControlableSi').setAttribute('checked', '');
    if (valorTipoVertimiento == 1)
        document.getElementById('rdEControlableNo').setAttribute('checked', '');    
    if (valorTipoVertimiento == 2)
        document.getElementById('rdEControlableParcialmente').setAttribute('checked', '');
}

function bloqueCentralesHidroelectricas(accion, recurcodi, evt) {

    var valorCh = $('input:radio[name=CHcentralSICOES]:checked').val();    
    ajustarDimensionBloquesSiCoesCH(valorCh);
    
    $('input[type=radio][name=CHcentralSICOES]').change(function () {
        if (this.value == '1') { //si 

        }
        else if (this.value == '0') { //no
            
        }
        mostrarValoresDefectoAlElegirSiCoes(this.value);
        ajustarDimensionBloquesSiCoesCH(this.value);
    });

    //al cambiar tipo conexion actualiza campos
    $('input[type=radio][name=tipoConexion]').change(function () {
        var valorCh = $('input:radio[name=CHcentralSICOES]:checked').val();

        //solo si SiCOES esta elegido, actualiza campos
        if (valorCh == "1") {
            if (this.value == '1') { //Cascada:1
                $("#txtCHPotencia").val($("#hfSitxtCHPotenciaCascada").val());
                $("#txtCHCoefProduccion").val($("#hfSitxtCHCoefProduccionCascada").val());
                $("#txtCHCaudalMax").val($("#hfSitxtCHCaudalMaxCascada").val());
                $("#txtCHCostoOM").val($("#hfSitxtCHCostoOMCascada").val());
            }
            else if (this.value == '0') { //Paralelo:0
                $("#txtCHPotencia").val($("#hfSitxtCHPotenciaParalelo").val());
                $("#txtCHCoefProduccion").val($("#hfSitxtCHCoefProduccionParalelo").val());
                $("#txtCHCaudalMax").val($("#hfSitxtCHCaudalMaxParalelo").val());
                $("#txtCHCostoOM").val($("#hfSitxtCHCostoOMParalelo").val());
            }
        } 
        if ($("#txtCHPotencia").val() != "")
            redonderValor(1, parseFloat($("#txtCHPotencia").val()), 'txtCHPotencia', 2);
        if ($("#txtCHCoefProduccion").val() != "")
            redonderValor(1, parseFloat($("#txtCHCoefProduccion").val()), 'txtCHCoefProduccion', 4);
        if ($("#txtCHCaudalMax").val() != "")
            redonderValor(1, parseFloat($("#txtCHCaudalMax").val()), 'txtCHCaudalMax', 2);
        if ($("#txtCHCostoOM").val() != "")
            redonderValor(1, parseFloat($("#txtCHCostoOM").val()), 'txtCHCostoOM', 4);
    });

    cargarOpcionesTablaCentralHidro(accion, recurcodi, evt);
}

function mostrarValoresDefectoAlElegirSiCoes(opcion) {
    if (opcion == '1') { //si
        //segun tipo conexion
        var tipoConexion = $('input:radio[name=tipoConexion]:checked').val(); //Paralelo:0, Cascada:1
        if (tipoConexion == "0") {
            $("#txtCHPotencia").val($("#hfSitxtCHPotenciaParalelo").val());
            $("#txtCHCoefProduccion").val($("#hfSitxtCHCoefProduccionParalelo").val());
            $("#txtCHCaudalMax").val($("#hfSitxtCHCaudalMaxParalelo").val());
            $("#txtCHCostoOM").val($("#hfSitxtCHCostoOMParalelo").val());
        } else {
            if (tipoConexion == "1") {
                $("#txtCHPotencia").val($("#hfSitxtCHPotenciaCascada").val());
                $("#txtCHCoefProduccion").val($("#hfSitxtCHCoefProduccionCascada").val());
                $("#txtCHCaudalMax").val($("#hfSitxtCHCaudalMaxCascada").val());
                $("#txtCHCostoOM").val($("#hfSitxtCHCostoOMCascada").val());
            }
        }
        
    }
    else if (opcion == '0') {//no
        $("#txtCHPotencia").val('');
        $("#txtCHCoefProduccion").val('');
        $("#txtCHCaudalMax").val('');
        $("#txtCHCostoOM").val('');
    }

    if ($("#txtCHPotencia").val() != "")
        redonderValor(1, parseFloat($("#txtCHPotencia").val()), 'txtCHPotencia', 2);
    if ($("#txtCHCoefProduccion").val() != "")
        redonderValor(1, parseFloat($("#txtCHCoefProduccion").val()), 'txtCHCoefProduccion', 4);
    if ($("#txtCHCaudalMax").val() != "")
        redonderValor(1, parseFloat($("#txtCHCaudalMax").val()), 'txtCHCaudalMax', 2);
    if ($("#txtCHCostoOM").val() != "")
        redonderValor(1, parseFloat($("#txtCHCostoOM").val()), 'txtCHCostoOM', 4);
}

function ajustarDimensionBloquesSiCoesCH(opcion) { 
    if (opcion == '1') { //si
        $("#contenedorDatosGenerales").css("height", "150px");
        $("#contenedorTopologia").css("height", "200px");
        $("#contenedorCentralH").css("height", "433px");

        $("#contenedorDGNombreSddp").css("line-height", "4");
        $("#contenedorDGCodigoSddp").css("line-height", "4");
        $("#contenedorDGDescripcionSddp").css("line-height", "4");

        $("#contenedorTEstacionHA").css("line-height", "5");
        $("#contenedorTTurbinamiento").css("line-height", "5");
        $("#contenedorTVertimiento").css("line-height", "5");

        $("#contenedorCHLista").css("display", "table-row");

        //Desactivar ingreso de datos en algunos campos de CH        
        document.getElementById("txtCHPotencia").disabled = true;
        document.getElementById("txtCHCoefProduccion").disabled = true;
        document.getElementById("txtCHCaudalMax").disabled = true;
        document.getElementById("txtCHCostoOM").disabled = true;
    }
    else if (opcion == '0') {//no
        $("#contenedorDatosGenerales").css("height", "70px");
        $("#contenedorTopologia").css("height", "70px");
        $("#contenedorCentralH").css("height", "225px");

        $("#contenedorDGNombreSddp").css("line-height", "1");
        $("#contenedorDGCodigoSddp").css("line-height", "1");
        $("#contenedorDGDescripcionSddp").css("line-height", "1");

        $("#contenedorTEstacionHA").css("line-height", "1");
        $("#contenedorTTurbinamiento").css("line-height", "1");
        $("#contenedorTVertimiento").css("line-height", "1");

        $("#contenedorCHLista").css("display", "none");

        //Activar ingreso de datos en algunos campos de CH        
        document.getElementById("txtCHPotencia").disabled = false;
        document.getElementById("txtCHCoefProduccion").disabled = false;
        document.getElementById("txtCHCaudalMax").disabled = false;
        document.getElementById("txtCHCostoOM").disabled = false;
    }
}


//TABLA CENTRALES HIDROLOGICAS
function cargarOpcionesTablaCentralHidro(accion, recurcodi, evt) {

    mostrarCentralesHidroelectricas(accion, recurcodi, evt.ListaCentralesHidro, MOSTRAR_GUARDADO);

    $('#btnVentanaAgregarCH').unbind();    
    $('#btnVentanaAgregarCH').bind("click", function () {        
        limpiarCamposAgregarCH();
        limpiarBarraMensaje('mensajeAddCentralHidro');
        abrirPopupSecundario("agregarCentralHidro");
        $("#txtPopupAddCHFactor").val(1); //1 por defecto
    });

    $('#btnAgregarCentralHidro').unbind();    
    $('#btnAgregarCentralHidro').bind("click",function () {
        limpiarBarraMensaje('mensajeAddCentralHidro');

        var obj = {};
        obj = getObjetoCentralHidro(AGREGAR);
        var mensaje = validarDatosPopupAgregarCH(obj);
        if (mensaje == "") {
            actualizarListadoCentralHidro(accion, AGREGAR, obj, recurcodi);

        } else {
            mostrarMensaje_('mensajeAddCentralHidro', 'error', mensaje);
        }
    });

    $('#btnCancelarAddCentralHidro').unbind();
    $('#btnCancelarAddCentralHidro').click(function () {
        $('#agregarCentralHidro').bPopup().close();
    });

    $('#btnActualizarCentralHidro').unbind();
    $('#btnActualizarCentralHidro').click(function () {
        limpiarBarraMensaje('mensajeEditCentralHidro');

        var obj = {};
        obj = getObjetoCentralHidro(EDITAR);
        var mensaje = validarDatosPopupAgregarCH(obj);
        if (mensaje == "") {
            actualizarListadoCentralHidro(accion, EDITAR, obj, recurcodi);

        } else {
            mostrarMensaje_('mensajeEditCentralHidro', 'error', mensaje);
        }
    });

    $('#btnCancelarEditCentralHidro').unbind();
    $('#btnCancelarEditCentralHidro').click(function () {
        $('#editarCentralHidro').bPopup().close();
    });
}

function mostrarCentralesHidroelectricas(accion, recurcodi, lstCentralesHidro, origen) {
    var dataJson = {
        accion: accion,
        recurcodi: recurcodi,

        lstCentralesHidro: lstCentralesHidro
    };
    limpiarBarraMensaje('mensaje');
    $.ajax({

        url: controlador + "CargarCentralesHidro",
        type: 'POST',
        contentType: 'application/json; charset=UTF-8',
        dataType: 'json',
        data: JSON.stringify(dataJson),
        success: function (evt) {
            if (evt.Resultado != "-1") {

                $("#tblCentralHidroelectrica").html(evt.HtmlListadoCentralesHidro);
                $('#tabla_CentralesHidro').dataTable({
                    "scrollY": 140,
                    "scrollX": true,
                    "sDom": 't',
                    "ordering": false,
                    "iDisplayLength": 50
                });

                listaCHidroEnDetalle = lstCentralesHidro;

                if (origen == RECALCULAR) { //cuando se actualiza lista de CH
                    var valorCh = $('input:radio[name=CHcentralSICOES]:checked').val();
                    mostrarValoresDefectoAlElegirSiCoes(valorCh);
                } else { //cuando trae de bd
                    if (topologiaAMostrar == 0) //Para configuracion base
                    {
                        var valorCh = $('input:radio[name=CHcentralSICOES]:checked').val();
                        mostrarValoresDefectoAlElegirSiCoes(valorCh);
                    }
                    else {//Para escenarios diferentes al Base
                        if ($("#txtCHPotencia").val() != "")
                            redonderValor(1, parseFloat($("#txtCHPotencia").val()), 'txtCHPotencia', 2);
                        if ($("#txtCHCoefProduccion").val() != "")
                            redonderValor(1, parseFloat($("#txtCHCoefProduccion").val()), 'txtCHCoefProduccion', 4);
                        if ($("#txtCHCaudalMax").val() != "")
                            redonderValor(1, parseFloat($("#txtCHCaudalMax").val()), 'txtCHCaudalMax', 2);
                        if ($("#txtCHCostoOM").val() != "")
                            redonderValor(1, parseFloat($("#txtCHCostoOM").val()), 'txtCHCostoOM', 4);
                    }

                    if ($("#txtCHCaudalMin").val() != "")
                        redonderValor(1, parseFloat($("#txtCHCaudalMin").val()), 'txtCHCaudalMin', 2);

                }
            } else {
                mostrarMensaje_('mensaje', 'error', 'Se ha producido un error: ' + evt.Mensaje);
            }
        },
        error: function (err) {
            mostrarMensaje('mensaje', 'error', 'Se ha producido un error al cargar las centrales hidroeléctricas.');
        }
    });
}


function actualizarListadoCentralHidro(accionCentralSddp, accionCentralHidro, objDato, recurcodi) {

    var dataValidoCentralesH = obtenerDataTablaCentrales();

    var dataJson = {
        accionCentralHidro: accionCentralHidro,
        topcodi: topologiaAMostrar,
        recurcodi: recurcodi,
        centralHidroCodi: objDato.centralCodi,
        centralHidroNombre: objDato.centralNombre,
        factor: objDato.factor,
        tipoConexion: $('input:radio[name=tipoConexion]:checked').val(),
        strListaCH: $("#hfLstCH").val(),
        lstCentralesHidroEnPantalla: dataValidoCentralesH
    };

    $.ajax({
        url: controlador + "ActualizarListaCentralesHidro",
        type: 'POST',
        contentType: 'application/json; charset=UTF-8',
        dataType: 'json',
        data: JSON.stringify(dataJson),
        success: function (resultado) {
            if (resultado.Resultado == "1") {
                

                if (accionCentralHidro == AGREGAR) {
                    limpiarBarraMensaje('mensajeAddCentralHidro');
                    $('#agregarCentralHidro').bPopup().close();
                } else {
                    if (accionCentralHidro == EDITAR) {
                        limpiarBarraMensaje('mensajeEditCentralHidro');
                        $('#editarCentralHidro').bPopup().close();
                    }
                        
                }
                
                mostrarCentralesHidroelectricas(accionCentralSddp, recurcodi, resultado.ListaCentralesHidro, RECALCULAR);

                setearCamposPorDefectoAlEscogerSiCoes(resultado.CentralSddp);

            }
            if (resultado.Resultado == "-1") {
                if (accionCentralHidro == AGREGAR)
                    mostrarMensaje_('mensajeAddCentralHidro', 'error', resultado.Mensaje);

                if (accionCentralHidro == EDITAR)
                    mostrarMensaje_('mensajeEditCentralHidro', 'error', resultado.Mensaje);

            }
        },
        error: function (err) {
            if (accionCentralHidro == AGREGAR)
                mostrarMensaje_('mensajeAddCentralHidro', 'error', 'Ha ocurrido un error:' + err);

            if (accionCentralHidro == EDITAR)
                mostrarMensaje_('mensajeEditCentralHidro', 'error', 'Ha ocurrido un error:' + err);


        }
    });

}

function setearCamposPorDefectoAlEscogerSiCoes(centralSddp) {
    $("#hfSitxtCHPotenciaParalelo").val(centralSddp.PotenciaDefectoParalelo);
    $("#hfSitxtCHPotenciaCascada").val(centralSddp.PotenciaDefectoCascada);

    $("#hfSitxtCHCoefProduccionParalelo").val(centralSddp.CoefProduccionDefectoParalelo);
    $("#hfSitxtCHCoefProduccionCascada").val(centralSddp.CoefProduccionDefectoCascada);

    $("#hfSitxtCHCaudalMaxParalelo").val(centralSddp.CaudalMaxTurbinableDefectoParalelo);
    $("#hfSitxtCHCaudalMaxCascada").val(centralSddp.CaudalMaxTurbinableDefectoCascada);

    $("#hfSitxtCHCostoOMParalelo").val(centralSddp.CostoOMDefectoParalelo);
    $("#hfSitxtCHCostoOMCascada").val(centralSddp.CostoOMDefectoCascada);
}

function editarCentralHidroelectrica(centralHidroCodi, recurcodi) {

    abrirPopupSecundario("editarCentralHidro");

    setearCamposEditarCH(centralHidroCodi);
}

function eliminarCentralHidroelectrica(centralHidroCodi, recurcodi) {

    var msgConfirmacion = '¿Esta seguro que desea eliminar la central hidroeléctrica?';

    if (confirm(msgConfirmacion)) {

        var obj = {};

        obj.centralCodi = centralHidroCodi;
        obj.centralNombre = "";
        obj.factor = -100;

        actualizarListadoCentralHidro(AGREGAR_CS, ELIMINAR, obj, recurcodi);  //AGREGAR_CS es temporal (para llenar campo)        
    }
}

function obtenerDataTablaCentrales() {
    var dataTabla = [];
    dataTabla = $('#tabla_CentralesHidro').DataTable().rows().data();

    var lstData = [];

    for (var i = 0; i < dataTabla.length; i++) {
        var item = dataTabla[i];

        //Quitamos columna Acciones
        var fila = {};
        fila.Codigo = item[1];
        fila.Nombre = item[2];
        fila.Factor = item[3];

        lstData.push(fila);
    }
    return lstData;
}

function limpiarCamposAgregarCH() {
    $('#cmbPopupAddCHCentral').val(0);
    $('#txtPopupAddCHFactor').val('');

}

function setearCamposEditarCH(centralHidroCodi) {

    var lstCentralesH = obtenerDataTablaCentrales();
    var factor = -2;
    var nombCentral = "";

    for (var i = 0; i < lstCentralesH.length; i++) {
        var item = lstCentralesH[i];

        var codigoCentralH = item.Codigo;

        if (codigoCentralH == centralHidroCodi) {
            factor = item.Factor;
            nombCentral = item.Nombre;
            break;
        }

    }

    var key = centralHidroCodi + "*" + nombCentral;
    $('#cmbPopupEditCHCentral').val(key);
    $('#txtPopupEditCHFactor').val(factor);

}

function getObjetoCentralHidro(accionHidro) {
    var obj = {};
    if (accionHidro == AGREGAR) {
        var elegido = $("#cmbPopupAddCHCentral").val();

        obj.centralCodi = elegido != 0 ? $("#cmbPopupAddCHCentral").val().split('*')[0] : elegido;
        obj.centralNombre = elegido != 0 ? $("#cmbPopupAddCHCentral").val().split('*')[1].trim() : "";
        var fac = $("#txtPopupAddCHFactor").val();
        if (fac == "")
            obj.factor = null;
        else 
            obj.factor = parseFloat(fac);
    } else if (accionHidro == EDITAR) {
        obj.centralCodi = $("#cmbPopupEditCHCentral").val().split('*')[0];
        obj.centralNombre = $("#cmbPopupEditCHCentral").val().split('*')[1].trim();
        var fac = $("#txtPopupEditCHFactor").val();
        if (Number(fac) == 0)
            obj.factor = 0;
        else
            obj.factor = Number(fac) || -100;
        
    }


    return obj;
}


function validarDatosPopupAgregarCH(obj) {

    var msj = "";
    if (obj.centralCodi == 0) {
        msj += "<p>Debe seleccionar una central.</p>";
    }

    if (obj.factor != null) {
        if (-1 > obj.factor || obj.factor > 1) {
            msj += "<p>Debe ingresar un factor correcto. Entre -1 y 1.</p>";
        }
    } else
        msj += "<p>Debe ingresar un factor.</p>";
    

    return msj;
}

function bloqueEmbalses(accion, recurcodi, evt) {
    //formatos a campos
    if ($("#txtEVolMin").val() != "")
        redonderValor(1, parseFloat($("#txtEVolMin").val()), 'txtEVolMin', 4);
    if ($("#txtEVolMax").val() != "")
        redonderValor(1, parseFloat($("#txtEVolMax").val()), 'txtEVolMax', 4);

    //radio Tipo Embalse
    var tipoEmbalse = $('input:radio[name=embalseTipo]:checked').val();// CentralPasada:0, Almacenamiento:1
    ajustarDimensionBloquesTipoEmb(tipoEmbalse);
    $('input[type=radio][name=embalseTipo]').change(function () {
        if (this.value == '1') {//Almacenamiento
            //alert("Pulso si");
            
        }
        else if (this.value == '0') {//CentralPasada
            //alert("Pulso no");
        }
        ajustarDimensionBloquesTipoEmb(this.value);
        
    }); 

    //Si esta elegido "Almacenamiento"
    if (tipoEmbalse == "1") {
        // radio SiCOES
        var valorCh = $('input:radio[name=EmbalseSICOES]:checked').val();// No:0, Si:1
        ajustarDimensionBloquesSiCoesEmb(valorCh);
    }
    
    $('input[type=radio][name=EmbalseSICOES]').change(function () {
        if (this.value == '1') {//si
            //alert("Pulso si");
        }
        else if (this.value == '0') {//no
            //alert("Pulso no");
        }
        ajustarDimensionBloquesSiCoesEmb(this.value);
    });

    cargarOpcionesTablaEmbalse(accion, recurcodi, evt);

    cargarTablasEvaporacion(evt);

    cargarTablaVolumenArea(evt);
}

function ajustarDimensionBloquesTipoEmb(tipoEmbalse) { 
    if (tipoEmbalse == '1') {//Almacenamiento:1
                
        $(".contenedorEAlmacenamiento").css("display", "table-row");
        $("#contenedorECentralPasada").css("display", "none");

        // radio SiCOES
        var valCh = $('input:radio[name=EmbalseSICOES]:checked').val();// No:0, Si:1
        ajustarDimensionBloquesSiCoesEmb(valCh);
        document.getElementById('miTabclick').click(); //se necesita un click para mostrar tablas
    }
    else if (tipoEmbalse == '0') {//CentralPasada:        

        $(".contenedorEAlmacenamiento").css("display", "none");
        $("#contenedorECentralPasada").css("display", "table-row");

        var valBD = $("#hftxtEFactorEmpuntamiento").val();
        $("#txtEFactorEmpuntamiento").val(valBD);

        $("#contenedorEmbalse").css("height", "70px");
                
    }
}


function ajustarDimensionBloquesSiCoesEmb(opcion) { 
    if (opcion == '1') {//si
        $("#contenedorEmbalse").css("height", "720px");
        $("#contenedorELista").css("display", "table-row");

    }
    else if (opcion == '0') {//no
        $("#contenedorEmbalse").css("height", "430px");
        $("#contenedorELista").css("display", "none");
    }
}

//TABLA EMBALSES
function cargarOpcionesTablaEmbalse(accion, recurcodi, evt) {
    mostrarEmbalses(accion, recurcodi, evt.ListaEmbalses);

    $('#btnVentanaAgregarE').unbind();
    $('#btnVentanaAgregarE').click(function () {
        limpiarCamposAgregarE();
        abrirPopupSecundario("agregarEmbalse");
        $("#txtPopupAddEFactor").val(1); //1 por defecto
    });

    $('#btnAgregarEmbalse').unbind();
    $('#btnAgregarEmbalse').click(function () {
        limpiarBarraMensaje('mensajeAddEmbalse');

        var obj = {};
        obj = getObjetoEmbalse(AGREGAR);
        var mensaje = validarDatosPopupAgregarE(obj);
        if (mensaje == "") {
            actualizarListadoEmbalse(accion, AGREGAR, obj, recurcodi);

        } else {
            mostrarMensaje_('mensajeAddEmbalse', 'error', mensaje);
        }
    });

    $('#btnCancelarAddEmbalse').unbind();
    $('#btnCancelarAddEmbalse').click(function () {
        $('#agregarEmbalse').bPopup().close();
    });

    $('#btnActualizarEmbalse').unbind();
    $('#btnActualizarEmbalse').click(function () {
        limpiarBarraMensaje('mensajeEditEmbalse');

        var obj = {};
        obj = getObjetoEmbalse(EDITAR);
        var mensaje = validarDatosPopupAgregarE(obj);
        if (mensaje == "") {
            actualizarListadoEmbalse(accion, EDITAR, obj, recurcodi);

        } else {
            mostrarMensaje_('mensajeEditEmbalse', 'error', mensaje);
        }
    });

    $('#btnCancelarEditEmbalse').unbind();
    $('#btnCancelarEditEmbalse').click(function () {
        $('#editarEmbalse').bPopup().close();
    });
}


function mostrarEmbalses(accion, recurcodi, listaEmbalses) {
    var dataJson = {
        accion: accion,
        recurcodi: recurcodi,

        lstEmbalses: listaEmbalses
    };
    limpiarBarraMensaje('mensaje');
    $.ajax({

        url: controlador + "CargarEmbalses",
        type: 'POST',
        contentType: 'application/json; charset=UTF-8',
        dataType: 'json',
        data: JSON.stringify(dataJson),
        success: function (evt) {
            if (evt.Resultado != "-1") {
                
                $("#tblEmbalse").html(evt.HtmlListadoEmbalse);
                $('#tabla_Embalses').dataTable({
                    "scrollY": 140,
                    "scrollX": true,
                    "sDom": 't',
                    "ordering": false,
                    "iDisplayLength": 50
                });

                listaEmbalseEnDetalle = listaEmbalses;
            } else {
                mostrarMensaje_('mensaje', 'error', 'Se ha producido un error: ' + evt.Mensaje);
            }

        },
        error: function (err) {
            mostrarMensaje('mensaje', 'error', 'Se ha producido un error al cargar los embalses.');

        }
    });
}


function actualizarListadoEmbalse(accionCentralSddp, accionEmbalse, objDato, recurcodi) {

    var dataValidoEmbalses = obtenerDataTablaEmbalses();
   
    var dataJson = {
        accionEmbalse: accionEmbalse,
        recurcodi: recurcodi,

        embalseCodi: objDato.embalseCodi,
        embalseNombre: objDato.embalseNombre,
        factor: objDato.factor,
        tipoVolumen: objDato.tipoVolumen,
        //fuenteSemanal: objDato.fuenteSemanal,
        ptoSemanal: objDato.puntoSemanal,
        //fuenteMensual: objDato.fuenteMensual,
        ptoMensual: objDato.puntoMensual,

        lstEmbalsesEnPantalla: dataValidoEmbalses
    };

    $.ajax({
        url: controlador + "ActualizarListaEmbalses",
        type: 'POST',
        contentType: 'application/json; charset=UTF-8',
        dataType: 'json',
        data: JSON.stringify(dataJson),
        success: function (resultado) {
            if (resultado.Resultado == "1") {                

                if (accionEmbalse == AGREGAR) {
                    limpiarBarraMensaje('mensajeAddEmbalse');
                    $('#agregarEmbalse').bPopup().close();
                }
                    

                if (accionEmbalse == EDITAR) {
                    limpiarBarraMensaje('mensajeEditEmbalse');
                    $('#editarEmbalse').bPopup().close();
                }                    

                mostrarEmbalses(accionCentralSddp, recurcodi, resultado.ListaEmbalses);
            }
            if (resultado.Resultado == "-1") {
                if (accionEmbalse == AGREGAR)
                    mostrarMensaje_('mensajeAddEmbalse', 'error', resultado.Mensaje);

                if (accionEmbalse == EDITAR)
                    mostrarMensaje_('mensajeEditEmbalse', 'error', resultado.Mensaje);

            }
        },
        error: function (err) {
            if (accionEmbalse == AGREGAR)
                mostrarMensaje_('mensajeAddEmbalse', 'error', 'Ha ocurrido un error:' + err);

            if (accionEmbalse == EDITAR)
                mostrarMensaje_('mensajeEditEmbalse', 'error', 'Ha ocurrido un error:' + err);


        }
    });

}

async function editarDatosEmbalse(embalseCodi, recurcodi) { 

   await setearCamposEditarE(embalseCodi);

    abrirPopupSecundario("editarEmbalse");

}

function eliminarEmbalse(embalseCodi, recurcodi) {  

    var msgConfirmacion = '¿Esta seguro que desea eliminar el embalse?';

    if (confirm(msgConfirmacion)) {

        var obj = {};

        obj.embalseCodi = embalseCodi;
        obj.embalseNombre = "";
        obj.factor = -100;
        obj.tipoVolumen = 0;
        //obj.fuenteSemanal = 0;
        obj.puntoSemanal = 0;
        //obj.fuenteMensual = 0;
        obj.puntoMensual = 0;

        actualizarListadoEmbalse(AGREGAR_CS, ELIMINAR, obj, recurcodi);       
    }
}

function obtenerDataTablaEmbalses() {
    var dataTabla = [];
    dataTabla = $('#tabla_Embalses').DataTable().rows().data();

    var lstData = [];

    for (var i = 0; i < dataTabla.length; i++) {
        var item = dataTabla[i];

        //Quitamos columna Acciones
        var fila = {};
        fila.Codigo = item[1];
        fila.Nombre = item[2];
        fila.Factor = item[3];
        fila.TipoVolumen = item[4];
        fila.FormatoSemanal = item[6];
        fila.PtoSemanal = item[8];
        fila.FormatoMensual = item[9];
        fila.PtoMensual = item[11];

        lstData.push(fila);
    }
    return lstData;
}

function limpiarCamposAgregarE() {
    $('#cmbPopupAddEmbalse').val(0);
    $('#txtPopupAddEFactor').val('');
    $('#cmbPopupAddAplica').val(0);
    
    $('#cmbAddPtosSemanal').val(0);
    $("#cmbAddPtosSemanal").empty();
    
    $('#cmbAddPtosMensual').val(0);
    $("#cmbAddPtosMensual").empty();
}

async function setearCamposEditarE(embalseCodi) { 

    var lstEmbalses = obtenerDataTablaEmbalses();
    var factor = -2;    
    var nombEmbalse = "";
    var tipoVolumen = -1;
    var formatoSem = -1;
    var ptoSem = -1;
    var formatoMen = -1;
    var ptoMen = -1;

    for (var i = 0; i < lstEmbalses.length; i++) {
        var item = lstEmbalses[i];

        var codigoEmbalse = item.Codigo;

        if (codigoEmbalse == embalseCodi) {
            factor = item.Factor;
            nombEmbalse = item.Nombre;
            tipoVolumen = item.TipoVolumen;
            formatoSem = item.FormatoSemanal;
            ptoSem = item.PtoSemanal;
            formatoMen = item.FormatoMensual;
            ptoMen = item.PtoMensual;

            break;
        }

    }

    var key = embalseCodi + "*" + nombEmbalse;
    $('#cmbPopupEditEmbalse').val(key);
    $('#txtPopupEditEFactor').val(factor);
    $('#cmbPopupEditAplica').val(tipoVolumen);
    
    await cargarPtosPorFormato(EDITAR);

    await sleep(1800);

    $('#cmbEditPtosSemanal').val(formatoSem + "#" + ptoSem);
    $('#cmbEditPtosMensual').val(formatoMen + "#" + ptoMen);
}

function getObjetoEmbalse(accionEmbalse) {
    var obj = {};
    var ftr = "";
    if (accionEmbalse == AGREGAR) {
        ftr = $("#txtPopupAddEFactor").val();
        var elegido = $("#cmbPopupAddEmbalse").val();
        obj.embalseCodi = elegido != 0 ? $("#cmbPopupAddEmbalse").val().split('*')[0] : elegido;
        obj.embalseNombre = elegido != 0 ? $("#cmbPopupAddEmbalse").val().split('*')[1].trim() : "";
        obj.factor = ftr != "" ? parseFloat($("#txtPopupAddEFactor").val()) : null;
        obj.tipoVolumen = $("#cmbPopupAddAplica").val();                
        obj.puntoSemanal = $("#cmbAddPtosSemanal").val();
        obj.puntoMensual = $("#cmbAddPtosMensual").val();
    } else if (accionEmbalse == EDITAR) {
        ftr = $("#txtPopupEditEFactor").val();
        obj.embalseCodi = $("#cmbPopupEditEmbalse").val().split('*')[0];
        obj.embalseNombre = $("#cmbPopupEditEmbalse").val().split('*')[1].trim();
        obj.factor = ftr != "" ? parseFloat($("#txtPopupEditEFactor").val()) : null;
        obj.tipoVolumen = $("#cmbPopupEditAplica").val();
        obj.puntoSemanal = $("#cmbEditPtosSemanal").val();
        obj.puntoMensual = $("#cmbEditPtosMensual").val();
    }


    return obj;
}


function validarDatosPopupAgregarE(obj) {

    var msj = "";
    if (obj.embalseCodi == 0) {
        msj += "<p>Debe seleccionar un embalse.</p>";
    }
    
    if (obj.factor == null) {
        msj += "<p>Debe ingresar un factor correcto (entre 0 y 1).</p>";
    } else {
        if (obj.factor < 0 || obj.factor > 1) {
            msj += "<p>Factor fuera de rango. Debe ingresar un factor entre 0 y 1.</p>";
        }
    }

    if (obj.tipoVolumen == 0) {
        msj += "<p>Debe seleccionar un tipo de volumen.</p>";
    }

    if (obj.puntoSemanal == 0) {
        msj += "<p>Debe seleccionar un punto de medicion  de fuente semanal.</p>";
    }

    if (obj.puntoMensual == 0) {
        msj += "<p>Debe seleccionar un punto de medicion  de fuente mensual.</p>";
    }
    
    return msj;
}


function cargarTablasEvaporacion(evt) {
    
    containerEvaporacion1 = document.getElementById('tblEvaporacion1');

    tblEvaporacion1 = new Handsontable(containerEvaporacion1, {
        dataSchema: {
            Codigo: null,
            Nombre: null,            
            Dato: null,            
        },
        colHeaders: ['Cod Mes','Mes', 'Dato'],
        columns: [
            { data: 'Codigo', editor: false, className: 'soloLectura htCenter', readOnly: true }, //SoloLectura es para cambiar color de fondo al validar, falta agregarlo al css           
            { data: 'Nombre', editor: false, className: 'columna-mes htCenter', readOnly: true },
            { data: 'Dato', type: 'numeric',  className: 'soloLectura htCenter', numericFormat: { pattern: '0.000' }, }
        ],

        colWidths: [40, 80, 60],
        hiddenColumns: {
            columns: [0],
            // show UI indicators to mark hidden columns
            indicators: false
        },
        columnSorting: false, // no puede ordenarse las columnas
        minSpareRows: 0,
        //showWeekNumber: false,
        rowHeaders: false, //quita numeración de cada fila
        colHeaders: false,
        autoWrapRow: true,
        startRows: 0,
        maxRows: 6,
        
    });

    tblEvaporacion1.updateSettings({
        beforeKeyDown(e) {
            soloPermiteCaracteresNumericos(e);
        },
        beforePaste: (data, coords) => {
            return soloPermitePegarSiSonNumeros(data);
        },
        cells: function (row, col, prop) {
            var cellProperties = {};

            if (!TIENE_PERMISO_NUEVO) {
                cellProperties.readOnly = 'true';                
            }
            
            return cellProperties;
        }
    });

    containerEvaporacion2 = document.getElementById('tblEvaporacion2');

    tblEvaporacion2 = new Handsontable(containerEvaporacion2, {
        dataSchema: {
            Codigo: null,
            Nombre: null,
            Dato: null,
        },
        colHeaders: ['Cod Mes', 'Mes', 'Dato'],
        columns: [
            { data: 'Codigo', editor: false, className: 'soloLectura htCenter', readOnly: true },            
            { data: 'Nombre', editor: false, className: 'columna-mes htCenter', readOnly: true },//columna-mes es para cambiar color de fondo, unido al css
            { data: 'Dato', type: 'numeric', className: 'soloLectura htCenter', numericFormat: { pattern: '0.000' }, }
        ],

        colWidths: [40, 80, 60],
        hiddenColumns: {
            columns: [0],
            indicators: false
        },
        columnSorting: false, // no puede ordenarse las columnas
        minSpareRows: 0,
        rowHeaders: false, //quita numeración de cada fila
        colHeaders: false,
        autoWrapRow: true,
        startRows: 0,
        maxRows: 6,
    });

    tblEvaporacion2.updateSettings({
        beforeKeyDown(e) {            
            soloPermiteCaracteresNumericos(e);
        },
        beforePaste: (data, coords) => {
            return soloPermitePegarSiSonNumeros(data);            
        },
        cells: function (row, col, prop) {
            var cellProperties = {};

            if (!TIENE_PERMISO_NUEVO) {
                cellProperties.readOnly = 'true';
            }

            return cellProperties;
        }
    });

    cargarHansonTablaEvaporacion(evt.ListaEvaporacion);
}

function soloPermiteCaracteresNumericos(e) {
    if (   e.keyCode === 48 || e.keyCode === 49 || e.keyCode === 50 || e.keyCode === 51 || e.keyCode === 52 || e.keyCode === 53 || e.keyCode === 54 || e.keyCode === 55 || e.keyCode === 56 || e.keyCode === 57  //keypad
        || e.keyCode === 96 || e.keyCode === 97 || e.keyCode === 98 || e.keyCode === 99 || e.keyCode === 100 || e.keyCode === 101 || e.keyCode === 102 || e.keyCode === 103 || e.keyCode === 104 || e.keyCode === 105 //numpad
        || e.keyCode === 110 || e.keyCode === 190 //punto
        || e.keyCode === 8 || e.keyCode === 13 || e.keyCode === 46 // delete, enter, supr
        || e.keyCode === 37 || e.keyCode === 38 || e.keyCode === 39 || e.keyCode === 40 //izq, der, arriba, abajo        
        || ((e.ctrlKey || e.metaKey) && e.keyCode == 67) //permite CRTL + C
        || ((e.ctrlKey || e.metaKey) && e.keyCode == 86) //permite CRTL + V
    ) {

    } else {
        e.stopImmediatePropagation();
        e.preventDefault();
    }
}

function soloPermitePegarSiSonNumeros(data) {
    var numTextos = 0;
    for (var i = 0; i < data.length; i++) {
        var val = data[i];
        if (isNaN(val)) {
            numTextos++;
        }
    }
    if (numTextos > 0) {
        return false;
    } else {
        return true;
    }
}

function cargarHansonTablaEvaporacion(listado) {
    tblEvaporacion1.loadData([]); //hansonTableClear();
    tblEvaporacion2.loadData([]); //hansonTableClear();

    var lst1 = listado.splice(0, (listado.length / 2));    
    var lst2 = listado.splice(0, listado.length);

    cargarHansonEvaporacionX(tblEvaporacion1, lst1);
    cargarHansonEvaporacionX(tblEvaporacion2, lst2);
}

function cargarHansonEvaporacionX(tblEvaporacionX, inputJson) {
    var lstEvapX = inputJson && inputJson.length > 0 ? inputJson : [];

    var lstData = [];
    tblEvaporacionX.loadData(lstData);

    for (var index in lstEvapX) {

        var item = lstEvapX[index];

        var data = {
            Codigo: item.Codigo,
            Nombre: item.Nombre,
            Dato: item.Dato,
        };

        lstData.push(data);
    }
    tblEvaporacionX.loadData(lstData);
}

function cargarTablaVolumenArea(evt) {
    
    containerVolArea = document.getElementById('tblVolumenArea');

    tblVolArea = new Handsontable(containerVolArea, {
        dataSchema: {
            Codigo: null,
            NumPar: null,
            Volumen: null,
            Area: null,
        },
        colHeaders: ['Cod ', '#', 'Volumen (hm3)', 'Área (hm3)'],
        columns: [
            { data: 'Codigo', editor: false, className: 'soloLectura htCenter', readOnly: true }, //SoloLectura es para cambiar color de fondo al validar, falta agregarlo al css           
            { data: 'NumPar', editor: false, className: 'columna-mes htCenter', readOnly: true },
            { data: 'Volumen', type: 'numeric', className: 'soloLectura htCenter', numericFormat: { pattern: '0.000' }, },
            { data: 'Area', type: 'numeric', className: 'soloLectura htCenter', numericFormat: { pattern: '0.000' }, }
        ],

        colWidths: [20, 20, 90, 75],
        hiddenColumns: {
            columns: [0],
            // show UI indicators to mark hidden columns
            indicators: false
        },
        columnSorting: false, // no puede ordenarse las columnas
        minSpareRows: 0,
        rowHeaders: false, //quita numeración de cada fila        
        autoWrapRow: true,
        startRows: 0,
        maxRows: 5,        
    });

    tblVolArea.updateSettings({
        beforeKeyDown(e) {
            soloPermiteCaracteresNumericos(e);
        },
        beforePaste: (data, coords) => {
            return soloPermitePegarSiSonNumeros(data);
        },
        cells: function (row, col, prop) {
            var cellProperties = {};

            if (!TIENE_PERMISO_NUEVO) {
                cellProperties.readOnly = 'true';
            }

            return cellProperties;
        }
    });  

    //pinta celda cada vez q modifico celda  
    tblVolArea.addHook('afterChange', function (source) {
        var dataHandson = tblVolArea.getSourceData();
        var dataHandson2 = tblVolArea.getData();
        var miData = [];
        var intervalosVol = 1, intervalosArea = 1;
        var lstVolumen = [], lstArea = [];
        var volMin, volMax, areaMin, areaMax;

        dataHandson2.forEach(function (items) {
            var vol = items[2];
            var area = items[3];
            var pto = [vol, area];

            if (vol != null && area != null) {
                miData.push(pto);

                lstVolumen.push(vol);
                lstArea.push(area);
            }
            
        });

        if (lstVolumen.length > 0 && lstArea.length > 0) { 
            volMin = Math.min(...lstVolumen);
            volMax = Math.max(...lstVolumen);

            areaMin = Math.min(...lstArea);
            areaMax = Math.max(...lstArea);

            intervalosVol = round10(((volMax - volMin) / 5), -3);
            intervalosArea = round10(((areaMax - areaMin) / 4), -3);
        }
                        
        mostrarGraficoVolumenArea(miData, intervalosVol, intervalosArea);
    });

    cargarHansonTablaVolumenArea(evt.ListaVolumenArea);
}

function cargarHansonTablaVolumenArea(listado) {
    tblVolArea.loadData([]); //hansonTableClear();

    cargarHansonVolArea(tblVolArea, listado);

}

function cargarHansonVolArea(tblVolArea, inputJson) {
    var lstVA = inputJson && inputJson.length > 0 ? inputJson : [];

    var lstData = [];
    tblVolArea.loadData(lstData);

    for (var index in lstVA) {

        var item = lstVA[index];

        var data = {
            Codigo: item.Codigo,
            NumPar: item.NumPar,
            Volumen: item.Volumen,
            Area: item.Area,
        };

        lstData.push(data);
    }
    tblVolArea.loadData(lstData);
}


function mostrarGraficoVolumenArea(datos, intervalosVol, intervalosArea) {
    var opcion;
    opcion = {
        
        chart: {
            height: 200,
            width: 400,
            zoomType: 'xy'
        },

        title: {
            text: ''
        },
        
        yAxis: {
            title: {
                text: 'Área'
            },
            tickInterval: intervalosArea
        },

        xAxis: {            
            title: {
                text: 'Volumen'
            },
            tickInterval: intervalosVol
        },

        legend: {
            enabled: false 
        },
        
        tooltip: {            
            formatter: function () {                
                var pto = obtenerPunto(`${this.point.x}`, `${this.point.y}`,datos);
                return "<b>" + pto + " </b><br>" +`Volumen: ${this.point.x};  Área: ${this.point.y}`;
            }
        },

        series: [{
                    
            data: datos

        }],
    };

    $('#grafico1').highcharts(opcion);
}

function obtenerPunto(vol, area, datos) {
    var salida = "";

    for (var i = 0; i < datos.length; i++) {
        var item = datos[i];

        if (item[0] == vol && item[1] == area) {
            salida = salida + " Punto " + (i + 1) + ",";
        }
    }

    return salida;
}

function decimalAdjust(type, value, exp) {
    // Si exp es undefined o cero...
    if (typeof exp === 'undefined' || +exp === 0) {
        return Math[type](value);
    }
    value = +value;
    exp = +exp;
    // Si el valor no es un número o exp no es un entero...
    if (isNaN(value) || !(typeof exp === 'number' && exp % 1 === 0)) {
        return NaN;
    }
    // Shift
    value = value.toString().split('e');
    value = Math[type](+(value[0] + 'e' + (value[1] ? (+value[1] - exp) : -exp)));
    // Shift back
    value = value.toString().split('e');
    return +(value[0] + 'e' + (value[1] ? (+value[1] + exp) : exp));
}

function round10(value, exp) {
    return decimalAdjust('round', value, exp);
};

function guardarCentralSddp(accion, topcodi) {
    var objCentralSddp = {};
    objCentralSddp = obtenerObjCentralSddpJson();
    limpiarBarraMensaje('mensaje');
    var mensaje = validarDatosObligatorios(objCentralSddp);

    if (mensaje == "") {
        var dataJson = {
            accion: accion,  
            topcodi: topcodi,
            centralSddp: objCentralSddp,
            recurcodiAEditar: recurcodiEnDetalle
        };

        $.ajax({
            url: controlador + "RegistrarCentralSddp",
            type: 'POST',
            contentType: 'application/json; charset=UTF-8',
            dataType: 'json',
            data: JSON.stringify(dataJson),
            success: function (result) {
                
                if (result.Resultado == "1") {
                    $("#vistaDetalle").html('');//limpiar tab Detalle
                    validarCambioDePestaña = false;
                    $('#tab-container').easytabs('select', '#vistaListado');

                    mostrarMensaje_('mensaje', 'exito', 'Central SDDP registrada con éxito.');
                    mostrarListadoCentralesSddp(topcodi, accionEscenario);

                } else {
                    mostrarMensaje_('mensaje', 'error', 'Se ha producido un error: ' + result.Mensaje);
                }

            },
            error: function (xhr, status) {
                mostrarMensaje_('mensaje', 'error', 'Se ha producido un error.');
            }
        });
    }
    else {
        mostrarMensaje_('mensaje', 'error', mensaje);
    }
}

function obtenerObjCentralSddpJson() {
    var obj = {};

    var dataHandsonEvap, dataHandsonEvap1, dataHandsonEvap2, dataHandsonVolArea, lstCoefEvaporacion, lstVolumenArea = [];
    dataHandsonEvap1 = tblEvaporacion1.getSourceData();
    dataHandsonEvap2 = tblEvaporacion2.getSourceData();
    dataHandsonEvap = dataHandsonEvap1.concat(dataHandsonEvap2);
    dataHandsonVolArea = tblVolArea.getSourceData();
    lstCoefEvaporacion = obtenerData(dataHandsonEvap);
    lstVolumenArea = obtenerData(dataHandsonVolArea);

    var valorChSiCoes = $('input:radio[name=CHcentralSICOES]:checked').val(); 
    var valorTipoCentralCH = $('input:radio[name=tipoCentral]:checked').val(); 
    var valorTipoConexionCH = $('input:radio[name=tipoConexion]:checked').val(); 
    var valorTipoEmbalse = $('input:radio[name=embalseTipo]:checked').val(); 
    var valorESiCoes = $('input:radio[name=EmbalseSICOES]:checked').val(); 
    var valorEVertimientoC = $('input:radio[name=VertimControlable]:checked').val(); 
    

    var pot = $("#txtCHPotencia").val().trim();
    var coefP = $("#txtCHCoefProduccion").val().trim();
    var cmint = $("#txtCHCaudalMin").val().trim();
    var cmaxt = $("#txtCHCaudalMax").val().trim();
    var fif = $("#txtCHFIForzada").val().trim();
    var fih = $("#txtCHFIHistorica").val().trim();
    var com = $("#txtCHCostoOM").val().trim();
    var femp = $("#txtEFactorEmpuntamiento").val().trim();
    var vmin = $("#txtEVolMin").val().trim();
    var vmax = $("#txtEVolMax").val().trim();

    
    obj.codigoSddp = parseInt($("#txtNombreSddp").val()); 
    
    obj.descripcion = $("#txtDescripcionSddp").val();
    obj.codigoEstacionHidro = parseInt($("#txtEstacionHidroAsociada").val());        
    obj.codigoTurbinamiento = $("#txtTurbinamiento").val() != "0" ? parseInt($("#txtTurbinamiento").val()) : null;         
    obj.codigoVertimiento = $("#txtVertimiento").val() != "0" ? parseInt($("#txtVertimiento").val()) : null;         
    obj.centralHidroSiCoes = parseInt(valorChSiCoes); //No:0, SI:1 
    obj.listaCentralesHidro = valorChSiCoes == "1" ? listaCHidroEnDetalle : [];    
    obj.tipoCentralHidro = parseInt(valorTipoCentralCH); //Existente:0, Futura:1
    obj.tipoConexionCentralHidro = parseInt(valorTipoConexionCH);//Paralelo:0, Cascada:1
    obj.potencia = pot != "" ? Number(pot) : null;
    obj.coefProduccion = coefP != "" ? Number(coefP) : null;
    obj.caudalMinTurbinable = cmint != "" ? Number(cmint) : null;
    obj.caudalMaxTurbinable = cmaxt != "" ? Number(cmaxt) : null;
    obj.factorIndisForzada = fif != "" ? Number(fif) : null;
    obj.factorIndisHistorica = fih != "" ? Number(fih) : null;
    obj.costoOM = com != "" ? Number(com) : null;

    obj.tipoEmbalse = parseInt(valorTipoEmbalse);//CentralPasada:0, Almacenamiento:1
    obj.factorEmpuntamiento = valorTipoEmbalse == "0" ? (femp != "" ? Number(femp) : null) : 0; 
    obj.embalseSiCoes = parseInt(valorESiCoes);//No:0, SI:1
    obj.listaEmbalses = valorESiCoes == "1" ? listaEmbalseEnDetalle : [];    
    obj.volumenMin = valorTipoEmbalse == "1" ? (vmin != "" ? Number(vmin) : null) : 0;
    obj.volumenMax = valorTipoEmbalse == "1" ? (vmax != "" ? Number(vmax) : null) : 0;
    obj.tipoVertimiento = valorTipoEmbalse == "1" ? (parseInt(valorEVertimientoC)) : null;//Controlable:0, No controlable:1, Parcialmente controlable:2
    obj.adicionarVolMin = valorTipoEmbalse == "1" ? (($('#chkEAdicVolIni').is(":checked")) ? 1 : 0) : null;//No:0, Si:1  
    obj.ajustarVolMin = valorTipoEmbalse == "1" ? (($('#chkEAjusVolMin').is(":checked")) ? 1 : 0) : null;;//No:0, Si:1 
    obj.ajustarVolIni = valorTipoEmbalse == "1" ? (($('#chkEAjusVolIni').is(":checked")) ? 1 : 0) : null;;//No:0, Si:1 
    obj.listaCoefEvaporacion = obj.tipoEmbalse == 1 ? lstCoefEvaporacion : [];
    obj.listaVolumenArea = obj.tipoEmbalse == 1 ? lstVolumenArea : [];

    return obj;
}


function obtenerData(dataHandson) {
    var lstData = [];
    for (var index in dataHandson) {
        var item = dataHandson[index];

        lstData.push(item);
    }
    return lstData;
}

function validarDatosObligatorios(objCentralSddp) {
    var msj = "";
    if (objCentralSddp.codigoSddp == 0) {
        msj += "<p>Debe seleccionar un codigo/nombre SDDP correcto.</p>";
    }
    if (objCentralSddp.codigoEstacionHidro == 0) {
        msj += "<p>Debe seleccionar una estación hidrológica correcta.</p>";
    }
    if (objCentralSddp.centralHidroSiCoes != 0 && objCentralSddp.centralHidroSiCoes != 1) {
        msj += "<p>Debe escoger si desea cargar central hidroelectrica desde SICOES o no.</p>";
    }
    if (objCentralSddp.centralHidroSiCoes == 1) { // lista centrales debe tener minimo una central
        if (objCentralSddp.listaCentralesHidro.length == 0) {
            msj += "<p>Debe ingresar mínimo una central hidroeléctrica.</p>";
        }        
    }
    if (objCentralSddp.tipoCentralHidro != 0 && objCentralSddp.tipoCentralHidro != 1) {
        msj += "<p>Debe escoger el tipo de central hidroeléctrica (Existente o Futura).</p>";
    }
    if (objCentralSddp.tipoConexionCentralHidro != 0 && objCentralSddp.tipoConexionCentralHidro != 1) {
        msj += "<p>Debe escoger el tipo de conexión (Paralelo o Cascada).</p>";
    }
    if (objCentralSddp.potencia != null) {
        if (objCentralSddp.potencia < 0) {
            msj += "<p>Potencia debe ser un valor no negativo.</p>";
        }
    } else {
        if (objCentralSddp.centralHidroSiCoes == 1) {
            msj += "<p>Debe ingresar Potencia.</p>";
        }
    }

    if (objCentralSddp.coefProduccion != null) {
        if (objCentralSddp.coefProduccion < 0) {
            msj += "<p>Coeficiente de Producción debe ser un valor no negativo.</p>";
        }
    } else {
        if (objCentralSddp.centralHidroSiCoes == 1) {
            msj += "<p>Debe ingresar Coeficiente de Producción.</p>";
        }
    }

    if (objCentralSddp.caudalMinTurbinable != null) {
        if (objCentralSddp.caudalMinTurbinable < 0) {
            msj += "<p>Caudal Mínimo T. debe ser un valor no negativo.</p>";
        }
    } else {
        if (objCentralSddp.centralHidroSiCoes == 1) {
            msj += "<p>Debe ingresar Caudal Mínimo Turbinable.</p>";
        }
    }

    if (objCentralSddp.caudalMaxTurbinable != null) {
        if (objCentralSddp.caudalMaxTurbinable < 0) {
            msj += "<p>Caudal Máximo T. debe ser un valor no negativo.</p>";
        }
    }
    if (objCentralSddp.factorIndisForzada != null) {
        if (objCentralSddp.factorIndisForzada < 0) {
            msj += "<p>Factor de Ind. Forzada debe ser un valor no negativo.</p>";
        }
    }
    if (objCentralSddp.factorIndisHistorica != null) {
        if (objCentralSddp.factorIndisHistorica < 0) {
            msj += "<p>Factor Ind. Histórica debe ser un valor no negativo.</p>";
        }
    }
    if (objCentralSddp.costoOM != null) {
        if (objCentralSddp.costoOM < 0) {
            msj += "<p>Costo O&M debe ser un valor no negativo.</p>";
        }
    } else {
        if (objCentralSddp.centralHidroSiCoes == 1) {
            msj += "<p>Debe ingresar Costo O&M.</p>";
        }
    }  

    if (objCentralSddp.tipoEmbalse != 0 && objCentralSddp.tipoEmbalse != 1) {
        msj += "<p>Debe escoger un tipo de embalse (Central de Pasada o Almacenamiento).</p>";
    }
            
    if (objCentralSddp.factorEmpuntamiento == null) {
        msj += "<p>Debe ingresar factor de empuntamiento correcto.</p>";
    }
    if (objCentralSddp.embalseSiCoes != 0 && objCentralSddp.embalseSiCoes != 1) {
        msj += "<p>Debe escoger si desea cargar embalses desde SICOES o no.</p>";
    }
    if (objCentralSddp.embalseSiCoes == 1) { // lista embalses debe tener minimo un embalse
        if (objCentralSddp.listaEmbalses.length == 0) {
            msj += "<p>Debe ingresar mínimo un embalse.</p>";
        }
    }
    
    if (objCentralSddp.volumenMin < 0 || objCentralSddp.volumenMin == null) {
        msj += "<p>Debe ingresar volumen mínimo correcto.</p>";
    }
    if (objCentralSddp.volumenMax < 0 || objCentralSddp.volumenMax == null) {
        msj += "<p>Debe ingresar volumen máximo correcto.</p>";
    }
    if (objCentralSddp.volumenMin != null && objCentralSddp.volumenMax != null ) {
        if (objCentralSddp.volumenMin > objCentralSddp.volumenMax ) {
            msj += "<p>El volumen minimo debe ser menor o igual al volumen máximo.</p>";
        }
    }
    if (objCentralSddp.tipoEmbalse == 1) {  
        if (objCentralSddp.tipoVertimiento != 0 && objCentralSddp.tipoVertimiento != 1 && objCentralSddp.tipoVertimiento != 2) {
            msj += "<p>Debe escoger un tipo de vertimiento correcto.</p>";
        }
    }

    var coefVacios = 0;
    for (var i = 0; i < objCentralSddp.listaCoefEvaporacion.length; i++) {
        var val = objCentralSddp.listaCoefEvaporacion[i].Dato;
        if (val == null || isNaN(val))
            coefVacios++;
    }
    if (coefVacios > 0) {
        msj += "<p>La tabla de Coef.de Evaporacion contiene datos incorrectos.</p>";
    }

    var vaVacios = 0;
    for (var i = 0; i < objCentralSddp.listaVolumenArea.length; i++) {
        var area = objCentralSddp.listaVolumenArea[i].Area;
        var vol = objCentralSddp.listaVolumenArea[i].Volumen;
        if (area == null || vol == null || isNaN(area) || isNaN(vol))
            vaVacios++;
    }
    if (vaVacios > 0) {
        msj += "<p>La tabla Volumen/Área contiene datos incorrectos.</p>";
    }

    return msj;
}

function eliminarCentralSddp(topcodi, recurcodi) {

    var msgConfirmacion = '¿Esta seguro que desea eliminar la central SDDP?';
    limpiarBarraMensaje('mensaje');
    if (confirm(msgConfirmacion)) {
        $.ajax({
            url: controlador + "EliminarCentralSddp",
            data: {
                topcodi: topcodi,
                recurcodi: recurcodi
            },
            type: 'POST',
            success: function (result) {

                if (result.Resultado === "-1") {
                    mostrarMensaje_('mensaje', 'error', 'Error al eliminar la central SDDP: ' + result.Mensaje);
                } else {
                    mostrarListadoCentralesSddp(topologiaAMostrar, accionEscenario);
                    mostrarMensaje_('mensaje', 'exito', 'Eliminación de la central SDDP realizada con éxito.');
                }
            },
            error: function (xhr, status) {
                mostrarMensaje_('mensaje', 'error', 'Se ha producido un error.');
            }
        });
    }
}

function exportarCentralesSddp(topcodi) {
    limpiarBarraMensaje('mensaje');
    $.ajax({
        type: 'POST',
        url: controlador + 'GenerarExcelCentralesSddp',
        data: {
            topcodi: topcodi, 
            titulo: $("#hfTituloEscenario").val(),
            strListaCH: $("#hfLstCH").val(),
            strListaEmb: $("#hfLstEmb").val()
        },
        dataType: 'json',
        success: function (result) {
            if (result.Resultado != "-1") {
                window.location = controlador + "Exportar?file_name=" + result.Resultado; // si hay elementos
            } else {
                mostrarMensaje_('mensaje', 'error', 'Se ha producido un error al exportar la relación de centrales SDDP.');
            }                
        },
        error: function (err) {
            mostrarMensaje_('mensaje', 'error', 'Se ha producido un error.');
        }
    });

}

async function cargarPtosPorFormato(accion) { 
        
    var equicodi = 0;
    limpiarBarraMensaje('mensaje');
    if (accion == AGREGAR) {
        equicodi = parseInt($("#cmbPopupAddEmbalse").val()) || -3;
        $("#cmbAddPtosSemanal").empty();
        $("#cmbAddPtosMensual").empty();
    } else {
        equicodi = parseInt($("#cmbPopupEditEmbalse").val()) || -3;       
        $("#cmbEditPtosSemanal").empty();
        $("#cmbEditPtosMensual").empty();
    }

    $.ajax({
        type: 'POST',
        url: controlador + 'CargarPtosMedicion',
        data: {
            equicodi: equicodi
        },

        success: function (evt) {
            if (evt.Resultado != "-1") {

                var listaPtosSem = evt.ListaFormatoPtosSemanal;
                var listaPtosMen = evt.ListaFormatoPtosMensual;

                var idSem = "";
                var idMen = "";

                if (accion == AGREGAR) {
                    idSem = 'cmbAddPtosSemanal';
                    idMen = 'cmbAddPtosMensual';
                } else {
                    if (accion == EDITAR) {
                        idSem = 'cmbEditPtosSemanal';
                        idMen = 'cmbEditPtosMensual';
                    }
                }                    

                //Listamos ptos Sem
                if (listaPtosSem.length > 0) { 
                    //usando for
                    $('#' + idSem).get(0).options[0] = new Option("-- Seleccione Punto de Medicion --", "0"); //obliga seleccionar 
                    for (var i = 0; i < listaPtosSem.length; i++) {
                        $('#' + idSem).append('<option value=' + listaPtosSem[i].Codigo + '>' + listaPtosSem[i].Nombre + '</option>');
                    }
                } else {
                    $('#' + idSem).get(0).options[0] = new Option("-- Seleccione Punto de Medicion --", "0");
                } 

                //Listamos ptos Mensual
                if (listaPtosMen.length > 0) {
                    //usando for
                    $('#' + idMen).get(0).options[0] = new Option("-- Seleccione Punto de Medicion --", "0"); //obliga seleccionar 
                    for (var i = 0; i < listaPtosMen.length; i++) {
                        $('#' + idMen).append('<option value=' + listaPtosMen[i].Codigo + '>' + listaPtosMen[i].Nombre + '</option>');
                    }
                } else {
                    $('#' + idMen).get(0).options[0] = new Option("-- Seleccione Punto de Medicion --", "0");
                } 
            } else {                
                mostrarMensaje_('mensaje', 'error', 'Ha ocurrido un error: ' + evt.Mensaje);
            }
        },
        error: function (err) {
            mostrarMensaje_('mensaje', 'error', 'Se ha producido un error al cargar el listado de puntos de medición para la fuente escogida.');
        }
    });
}
function actualizarListadoCentralesHidro() { 
    $("#cmbPopupAddCHCentral").empty();
    $("#cmbPopupEditCHCentral").empty();
    limpiarBarraMensaje('mensaje');
    $.ajax({
        type: 'POST',
        url: controlador + '/ActualizarListadoCentralesHidro',
        dataType: 'json',
        data: {
            mtopcodi: topologiaAMostrar
        },
        cache: false,
        success: function (evt) {
            var listado = evt.ListaTotalCentralesHidro;

            //Agregar
            $('#cmbPopupAddCHCentral').get(0).options[0] = new Option("-- Seleccione Central --", "0"); //obliga seleccionar 
            for (var i = 0; i < listado.length; i++) {
                var val = listado[i].Equicodi + "*" + listado[i].Central;                
                $('#cmbPopupAddCHCentral').append(`<option value="${val}"> ${listado[i].Central} </option>`);

                //El combo editar tambiendebe actualizarse
                $('#cmbPopupEditCHCentral').append(`<option value="${val}"> ${listado[i].Central} </option>`);
            }

        },
        error: function (err) {
            mostrarMensaje_('mensaje', 'error', 'Ha ocurrido un error: ');
        }
    });
}

function irANuevasCentralesHidro () {
    var url = siteRoot + 'Equipamiento/Equipo/' + "Index";
    window.open(url, '_blank').focus();
};

function redonderValor(tipo, valorIngresado, idCampo, numDecimales) {
    var valorRedondeado;

    var valor = valorIngresado.value;
    if (tipo == 0)
        valor = valorIngresado.value;
    if (tipo == 1)
        valor = valorIngresado;

    if (isNaN(valor) || valor == "") {
        valorRedondeado = valor;
    } else {
        var n = Number.parseFloat(valor);
        if (contarDecimales(n) >= 7) {
            valorRedondeado = n.toFixed(7);
        } else {
            valorRedondeado = n;
        }
    }
    
    $("#" + idCampo).val(valorRedondeado);
}

function contarDecimales(value) {
    var m = Math.floor(value);
    if (m === value) return 0;
    return value.toString().split(".")[1].length || 0;
}

function setearCodigoSddp(objetoElegido) {
    limpiarBarraMensaje('mensaje');
    var codigoSddpSeleccionado = parseInt(objetoElegido.value);   
    $.ajax({
        type: 'POST',
        url: controlador + 'ObtenerSddpNum',
        data: {
            codigoSddp: codigoSddpSeleccionado
        },
        success: function (evt) {
            if(evt.Resultado == "1")
                $("#txtCodigoSddp").html(evt.SddpNum);
        },
        error: function (err) {
            mostrarMensaje_('mensaje', 'error', 'Ha ocurrido un error: ');
        }
    });

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

function abrirPopupSecundario(contentPopup) {
    setTimeout(function () {
        $("#" + contentPopup).bPopup({
            easing: 'easeOutBack',
            speed: 60,
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
    quitarClases(id);
    $('#' + id).addClass('action-' + tipo);
    $('#' + id).html(mensaje);

    if (id == "mensaje")
        location.href = "#container-titulo";

    if (tipo != "error") {
        await sleep(10000);

        limpiarBarraMensaje(id);
    }
    
}

function limpiarBarraMensaje(id) {
    $('#' + id).css("display", "none");
    quitarClases(id);
    $('#' + id).addClass('action-message');
    $('#' + id).html('');
}

function quitarClases(id) {
    document.getElementById(id).classList.remove("action-message");
    document.getElementById(id).classList.remove("action-error");
    document.getElementById(id).classList.remove("action-exito");
    document.getElementById(id).classList.remove("action-alert");
    document.getElementById(id).classList.remove("action-titulo");
}