var controlador = siteRoot + 'Subastas/ActivacionOferta/';


const TIPOINFO_SUBIR = 1;
const TIPOINFO_BAJAR = 2;
const SUBIR = "Subir";
const BAJAR = "Bajar";
const MSG_ERROR_LECTURA = 'No se puede realizar esta acción dado que la ventana esta en modo de solo lectura.';

var listaHistorialActivaciones = [];
var listaDeErroresGeneral = [];
var listaDeErroresSubir = [];
var listaDeErroresBajar = [];
var datosEnDeficitGeneral = [];
var datosEnReduccionGeneral = [];
var datosEnDeficitSubir = [];
var datosEnDeficitBajar = [];
var datosEnReduccionSubir = [];
var datosEnReduccionBajar = [];

var ventanaActivacionSoloLectura = true;

var containerTablaSubir;
var tblHndSubir;
var containerTablaBajar;
var tblHndBajar;

var anchoTablaActivacion;


const TAB_ACTIVACION = 1;
const TAB_INDISPONIBILIDAD = 2;
var tabSeleccionado = TAB_ACTIVACION;

$(function () {  

    $('#tab-container').easytabs({
        animate: false
    });
    $('#tab-container').easytabs('select', '#tabActivacion');

    $('#fechaOferta').Zebra_DatePicker({
        format: "d/m/Y",
        direction: ['01/01/1900', diaManiana()] //sos quitar
        /*direction: ['01/01/1900', $("#hdDiaSiguiente").val()]*/
    });

    $('#btnConsultar').click(function () {
        limpiarBarraMensaje("mensaje_activacion");
        limpiarBarraMensaje("mensaje_indisponibilidad");
        //prelecciono pestaña defecto
        $('#tab-container').easytabs('select', '#tabActivacion');
        //cargo handson
        cargarDatosActivacion();        
    });

    $('#btnErrores').click(function () {
        mostrarListadoErrores();
    });

    $('#btnActivaciones').click(function () {
        mostrarListadoActivaciones();
    });

    $('#btnActivar').click(function () {
        activarOfertasPorDefecto();
    });

    cargarDatosActivacion();

    anchoTablaActivacion = $("#mainLayout").width();
    $('#contenedorTablaSubir').css("width", anchoTablaActivacion - 70);
    $('#contenedorTablaBajar').css("width", anchoTablaActivacion - 70);


    $('#tab-container').bind('easytabs:before', function (id, val, t) {
        var msgInd = $("#mensaje_indisponibilidad").html();
        if (t.selector == "#tabActivacion") {
            //borro mensajes
            limpiarBarraMensaje("mensaje_activacion");
            if (msgInd != msgSinIndisponibilidad)
                limpiarBarraMensaje("mensaje_indisponibilidad");
            precargarDetallesOtraPestania();
            tabSeleccionado = TAB_ACTIVACION;
            precargarDetallesOtraPestania();
        }
        if (t.selector == "#tabIndisponibilidad") {
            //borro mensajes
            limpiarBarraMensaje("mensaje_activacion");
            if (msgInd != msgSinIndisponibilidad)
                limpiarBarraMensaje("mensaje_indisponibilidad");
            precargarDetallesOtraPestania();
            tabSeleccionado = TAB_INDISPONIBILIDAD;
            precargarDetallesOtraPestania();
        }       
    });

});

function cargarDatosActivacion() {

    var fechaOferta = $("#fechaOferta").val();
    var maniana = diaManiana();

    //Verifico la edicion de datos
    if (maniana === fechaOferta) {
        ventanaActivacionSoloLectura = false;
    } else {
        ventanaActivacionSoloLectura = true;
    }

    limpiarBarraMensaje("mensaje_activacion");

    $.ajax({
        type: 'POST',
        url: controlador + "CargarDatosActivacion",
        data: {
            fechaOferta: fechaOferta
        },       
        success: function (evt) {

            if (evt.Resultado == "1") {

                var data = evt.DataActivacionOferta;
                listarMotivos("cbMotivosSubir", evt.ListaMotivosActivacionSubir);
                listarMotivos("cbMotivosBajar", evt.ListaMotivosActivacionBajar);

                inicializarNuevaActivacion(false);
                iniciarHandsontables();
               
                listaHistorialActivaciones = evt.HistorialActivaciones;

                if (data.ExisteActivacionOferta) {
                    cargarHansonTablas(data);
                }

                //Una vez terminado de cargar ACTIVACIONES cargo IDNISPONIBILIDAD
                cargarDatosIndisponibilidad();
                
            } else {
                mostrarMensaje('mensaje_activacion', 'error', evt.Mensaje);
            }
        },
        error: function (xhr, status) {
            mostrarMensaje('mensaje_activacion', 'error', 'Se ha producido un error.');
        }
    });

}

function cargarHansonTablas(datosEnBD) {
    tblHndSubir.loadData([]); 
    tblHndBajar.loadData([]);

    //Subir
    cargarDataAHanson(datosEnBD, SUBIR);
    setearMotivos(datosEnBD, SUBIR);

    //Bajar
    cargarDataAHanson(datosEnBD, BAJAR);
    setearMotivos(datosEnBD, BAJAR);
}

function setearMotivos(datosEnBd, tipoInfo) {
    var combo;
    var lstSeleccionados;
    if (tipoInfo == SUBIR) {
        combo = $('#cbMotivosSubir');
        lstSeleccionados = datosEnBd.IdsMotivosSubir;
    }
    if (tipoInfo == BAJAR) {
        combo = $('#cbMotivosBajar');
        lstSeleccionados = datosEnBd.IdsMotivosBajar;
    }

    combo.multipleSelect('setSelects', lstSeleccionados);
}

function cargarDataAHanson(datosEnBd, tipoInfo) {
    var tablaHandson;
    var datosDeficit, datosReduccion = [];
    if (tipoInfo == SUBIR) {
        tablaHandson = tblHndSubir;

        datosDeficit = datosEnBd.DatosDeficitSubir;
        datosReduccion = datosEnBd.DatosReduccionBandaSubir;
    }
    if (tipoInfo == BAJAR) {
        tablaHandson = tblHndBajar;

        datosDeficit = datosEnBd.DatosDeficitBajar;
        datosReduccion = datosEnBd.DatosReduccionBandaBajar;
    }

    var lstData ;

    lstData = [
        { H1: '1', H2: '2', H3: '3', H4: '4', H5: '5', H6: '6', H7: '7', H8: '8', H9: '9', H10: '10', H11: '11', H12: '12', H13: '13', H14: '14', H15: '15', H16: '16', H17: '17', H18: '18', H19: '19', H20: '20', H21: '21', H22: '22', H23: '23', H24: '24', H25: '25', H26: '26', H27: '27', H28: '28', H29: '29', H30: '30', H31: '31', H32: '32', H33: '33', H34: '34', H35: '35', H36: '36', H37: '37', H38: '38', H39: '39', H40: '40', H41: '41', H42: '42', H43: '43', H44: '44', H45: '45', H46: '46', H47: '47', H48: '48' },
        { H1: '00:30', H2: '01:00', H3: '01:30', H4: '02:00', H5: '02:30', H6: '03:00', H7: '03:30', H8: '04:00', H9: '04:30', H10: '05:00', H11: '05:30', H12: '06:00', H13: '06:30', H14: '07:00', H15: '07:30', H16: '08:00', H17: '08:30', H18: '09:00', H19: '09:30', H20: '10:00', H21: '10:30', H22: '11:00', H23: '11:30', H24: '12:00', H25: '12:30', H26: '13:00', H27: '13:30', H28: '14:00', H29: '14:30', H30: '15:00', H31: '15:30', H32: '16:00', H33: '16:30', H34: '17:00', H35: '17:30', H36: '18:00', H37: '18:30', H38: '19:00', H39: '19:30', H40: '20:00', H41: '20:30', H42: '21:00', H43: '21:30', H44: '22:00', H45: '22:30', H46: '23:00', H47: '23:30', H48: '00:00' },
        { H1: 'DÉFICIT DE RSF' },
        {
            H1: datosDeficit.H1, H2: datosDeficit.H2, H3: datosDeficit.H3, H4: datosDeficit.H4, H5: datosDeficit.H5, H6: datosDeficit.H6, H7: datosDeficit.H7, H8: datosDeficit.H8,
            H9: datosDeficit.H9, H10: datosDeficit.H10, H11: datosDeficit.H11, H12: datosDeficit.H12, H13: datosDeficit.H13, H14: datosDeficit.H14, H15: datosDeficit.H15, H16: datosDeficit.H16,
            H17: datosDeficit.H17, H18: datosDeficit.H18, H19: datosDeficit.H19, H20: datosDeficit.H20, H21: datosDeficit.H21, H22: datosDeficit.H22, H23: datosDeficit.H23, H24: datosDeficit.H24,
            H25: datosDeficit.H25, H26: datosDeficit.H26, H27: datosDeficit.H27, H28: datosDeficit.H28, H29: datosDeficit.H29, H30: datosDeficit.H30, H31: datosDeficit.H31, H32: datosDeficit.H32,
            H33: datosDeficit.H33, H34: datosDeficit.H34, H35: datosDeficit.H35, H36: datosDeficit.H36, H37: datosDeficit.H37, H38: datosDeficit.H38, H39: datosDeficit.H39, H40: datosDeficit.H40,
            H41: datosDeficit.H41, H42: datosDeficit.H42, H43: datosDeficit.H43, H44: datosDeficit.H44, H45: datosDeficit.H45, H46: datosDeficit.H46, H47: datosDeficit.H47, H48: datosDeficit.H48
        },
        { H1: 'REDUCCIÓN DE BANDA A (MW)' },
        {
            H1: datosReduccion.H1, H2: datosReduccion.H2, H3: datosReduccion.H3, H4: datosReduccion.H4, H5: datosReduccion.H5, H6: datosReduccion.H6, H7: datosReduccion.H7, H8: datosReduccion.H8,
            H9: datosReduccion.H9, H10: datosReduccion.H10, H11: datosReduccion.H11, H12: datosReduccion.H12, H13: datosReduccion.H13, H14: datosReduccion.H14, H15: datosReduccion.H15, H16: datosReduccion.H16,
            H17: datosReduccion.H17, H18: datosReduccion.H18, H19: datosReduccion.H19, H20: datosReduccion.H20, H21: datosReduccion.H21, H22: datosReduccion.H22, H23: datosReduccion.H23, H24: datosReduccion.H24,
            H25: datosReduccion.H25, H26: datosReduccion.H26, H27: datosReduccion.H27, H28: datosReduccion.H28, H29: datosReduccion.H29, H30: datosReduccion.H30, H31: datosReduccion.H31, H32: datosReduccion.H32,
            H33: datosReduccion.H33, H34: datosReduccion.H34, H35: datosReduccion.H35, H36: datosReduccion.H36, H37: datosReduccion.H37, H38: datosReduccion.H38, H39: datosReduccion.H39, H40: datosReduccion.H40,
            H41: datosReduccion.H41, H42: datosReduccion.H42, H43: datosReduccion.H43, H44: datosReduccion.H44, H45: datosReduccion.H45, H46: datosReduccion.H46, H47: datosReduccion.H47, H48: datosReduccion.H48
        }
    ];
    
    tablaHandson.loadData(lstData);
}

function inicializarNuevaActivacion(consultaDesdeHistorial) {
    consultaDesdeHistorial = consultaDesdeHistorial != null ? consultaDesdeHistorial : false;
    if (!consultaDesdeHistorial) {
        listaHistorialActivaciones = [];
        $('#cbMotivosSubir').multipleSelect('uncheckAll');
        $('#cbMotivosBajar').multipleSelect('uncheckAll');
    }
    
    listaDeErroresGeneral = [];
    listaDeErroresSubir = [];
    listaDeErroresBajar = [];
    datosEnDeficitGeneral = [];
    datosEnReduccionGeneral = [];
    datosEnDeficitSubir = [];
    datosEnDeficitBajar = [];
    datosEnReduccionSubir = [];
    datosEnReduccionBajar = [];
    

    //Activo y desactivo notones
    if (ventanaActivacionSoloLectura) {
        $('#btnErrores').css("display", "none");
        $('#btnActivar').css("display", "none");
    } else {
        $('#btnErrores').css("display", "block");
        $('#btnActivar').css("display", "block");
    }
}

function iniciarHandsontables() {

    /******************************************/
    /************** SUBIR *********************/

    // Inicio datos
    Handsontable.renderers.registerRenderer('datosIngresadosSubirRenderer', datosIngresadosSubirRenderer);

    // #region Handsontable SEMANA/MES
    containerTablaSubir = document.getElementById('tblSubir');

    tblHndSubir = new Handsontable(containerTablaSubir, {
        data: [
            { H1: '1', H2: '2', H3: '3', H4: '4', H5: '5', H6: '6', H7: '7', H8: '8', H9: '9', H10: '10', H11: '11', H12: '12', H13: '13', H14: '14', H15: '15', H16: '16', H17: '17', H18: '18', H19: '19', H20: '20', H21: '21', H22: '22', H23: '23', H24: '24', H25: '25', H26: '26', H27: '27', H28: '28', H29: '29', H30: '30', H31: '31', H32: '32', H33: '33', H34: '34', H35: '35', H36: '36', H37: '37', H38: '38', H39: '39', H40: '40', H41: '41', H42: '42', H43: '43', H44: '44', H45: '45', H46: '46', H47: '47', H48: '48' },
            { H1: '00:30', H2: '01:00', H3: '01:30', H4: '02:00', H5: '02:30', H6: '03:00', H7: '03:30', H8: '04:00', H9: '04:30', H10: '05:00', H11: '05:30', H12: '06:00', H13: '06:30', H14: '07:00', H15: '07:30', H16: '08:00', H17: '08:30', H18: '09:00', H19: '09:30', H20: '10:00', H21: '10:30', H22: '11:00', H23: '11:30', H24: '12:00', H25: '12:30', H26: '13:00', H27: '13:30', H28: '14:00', H29: '14:30', H30: '15:00', H31: '15:30', H32: '16:00', H33: '16:30', H34: '17:00', H35: '17:30', H36: '18:00', H37: '18:30', H38: '19:00', H39: '19:30', H40: '20:00', H41: '20:30', H42: '21:00', H43: '21:30', H44: '22:00', H45: '22:30', H46: '23:00', H47: '23:30', H48: '00:00' },
            { H1: 'DÉFICIT DE RSF' },
            { H1: '', H2: '', H3: '', H4: '', H5: '', H6: '', H7: '', H8: '', H9: '', H10: '', H11: '', H12: '', H13: '', H14: '', H15: '', H16: '', H17: '', H18: '', H19: '', H20: '', H21: '', H22: '', H23: '', H24: '', H25: '', H26: '', H27: '', H28: '', H29: '', H30: '', H31: '', H32: '', H33: '', H34: '', H35: '', H36: '', H37: '', H38: '', H39: '', H40: '', H41: '', H42: '', H43: '', H44: '', H45: '', H46: '', H47: '', H48: '' },
            { H1: 'REDUCCIÓN DE BANDA A (MW)' },
            { H1: '', H2: '', H3: '', H4: '', H5: '', H6: '', H7: '', H8: '', H9: '', H10: '', H11: '', H12: '', H13: '', H14: '', H15: '', H16: '', H17: '', H18: '', H19: '', H20: '', H21: '', H22: '', H23: '', H24: '', H25: '', H26: '', H27: '', H28: '', H29: '', H30: '', H31: '', H32: '', H33: '', H34: '', H35: '', H36: '', H37: '', H38: '', H39: '', H40: '', H41: '', H42: '', H43: '', H44: '', H45: '', H46: '', H47: '', H48: '' },
        ],
        dataSchema: {
            H1: null, H2: null, H3: null, H4: null, H5: null, H6: null, H7: null, H8: null, H9: null, H10: null,
            H11: null, H12: null, H13: null, H14: null, H15: null, H16: null, H17: null, H18: null, H19: null, H20: null,
            H21: null, H22: null, H23: null, H24: null, H25: null, H26: null, H27: null, H28: null, H29: null, H30: null,
            H31: null, H32: null, H33: null, H34: null, H35: null, H36: null, H37: null, H38: null, H39: null, H40: null,
            H41: null, H42: null, H43: null, H44: null, H45: null, H46: null, H47: null, H48: null
        },

        
        mergeCells: [
            { row: 2, col: 0, rowspan: 1, colspan: 48 },
            { row: 4, col: 0, rowspan: 1, colspan: 48 }
        ],
        colWidths: 55,
        height: '160px',
        width: '100%',

    });    

    tblHndSubir.addHook('beforeChange', function (changes, [source]) {
        listaDeErroresSubir = [];
        datosEnDeficitSubir = [];
        datosEnReduccionSubir = [];
    });   

    tblHndSubir.addHook('afterInit', validaRangosDeficitSubir);
    tblHndSubir.addHook('afterInit', validaRangosReduccionSubir);
    tblHndSubir.addHook('afterInit', validarExistenciaMotivoSubir);
    tblHndSubir.init();    

    tblHndSubir.addHook('afterChange', function (changes, [source]) {
        listaDeErroresGeneral = [];

        validaRangosDeficitSubir();
        validaRangosReduccionSubir();
        validarExistenciaMotivoSubir();
        validarExistenciaMotivoBajar();

        //Obtengo lista Errores General
        listaDeErroresGeneral = listaDeErroresGeneral.concat(listaDeErroresBajar.concat(listaDeErroresSubir));
    });

    //valido existe de dato en deficit de RSF en subir
    function validaRangosDeficitSubir() {
        datosEnDeficitGeneral = datosEnDeficitBajar.concat(datosEnDeficitSubir);
        datosEnReduccionGeneral = datosEnReduccionBajar.concat(datosEnReduccionSubir);
        agregarValErrorSinDeficitNiReduccion();
    }

    //valido existe de dato en Reduccion en subir
    function validaRangosReduccionSubir() {
        datosEnDeficitGeneral = datosEnDeficitBajar.concat(datosEnDeficitSubir);
        datosEnReduccionGeneral = datosEnReduccionBajar.concat(datosEnReduccionSubir);
        agregarValErrorSinDeficitNiReduccion();
    }

    //function agregarValErrorSinDeficit() {
    //    if (datosEnDeficitGeneral.length == 0) {
    //        agregarError(SUBIR + " / " + BAJAR, "Déficit de RSF", "", "", "Se debe ingresar como mínimo una celda con valor numérico correcto en la información de “Déficit de RSF” para poder realizar la activación.");//
    //    }
    //}

    function agregarValErrorSinDeficitNiReduccion() {
        if (datosEnDeficitGeneral.length == 0 && datosEnReduccionGeneral.length == 0) {
            agregarError(SUBIR + " / " + BAJAR, "Déficit de RSF / Reducción de Banda", "", "", "Se debe ingresar como mínimo una celda con valor numérico correcto en la información de “Déficit de RSF” y/o “Reducción de Banda” para poder realizar la activación.");//
        }
    }
  
 
    /******************************************/
    /************** BAJAR *********************/


    // Inicio datos
    Handsontable.renderers.registerRenderer('datosIngresadosBajarRenderer', datosIngresadosBajarRenderer);

    // #region Handsontable SEMANA/MES
    containerTablaBajar = document.getElementById('tblBajar');

    tblHndBajar = new Handsontable(containerTablaBajar, {
        data: [
            { H1: '1', H2: '2', H3: '3', H4: '4', H5: '5', H6: '6', H7: '7', H8: '8', H9: '9', H10: '10', H11: '11', H12: '12', H13: '13', H14: '14', H15: '15', H16: '16', H17: '17', H18: '18', H19: '19', H20: '20', H21: '21', H22: '22', H23: '23', H24: '24', H25: '25', H26: '26', H27: '27', H28: '28', H29: '29', H30: '30', H31: '31', H32: '32', H33: '33', H34: '34', H35: '35', H36: '36', H37: '37', H38: '38', H39: '39', H40: '40', H41: '41', H42: '42', H43: '43', H44: '44', H45: '45', H46: '46', H47: '47', H48: '48' },
            { H1: '00:30', H2: '01:00', H3: '01:30', H4: '02:00', H5: '02:30', H6: '03:00', H7: '03:30', H8: '04:00', H9: '04:30', H10: '05:00', H11: '05:30', H12: '06:00', H13: '06:30', H14: '07:00', H15: '07:30', H16: '08:00', H17: '08:30', H18: '09:00', H19: '09:30', H20: '10:00', H21: '10:30', H22: '11:00', H23: '11:30', H24: '12:00', H25: '12:30', H26: '13:00', H27: '13:30', H28: '14:00', H29: '14:30', H30: '15:00', H31: '15:30', H32: '16:00', H33: '16:30', H34: '17:00', H35: '17:30', H36: '18:00', H37: '18:30', H38: '19:00', H39: '19:30', H40: '20:00', H41: '20:30', H42: '21:00', H43: '21:30', H44: '22:00', H45: '22:30', H46: '23:00', H47: '23:30', H48: '00:00' },
            { H1: 'DÉFICIT DE RSF' },
            { H1: '', H2: '', H3: '', H4: '', H5: '', H6: '', H7: '', H8: '', H9: '', H10: '', H11: '', H12: '', H13: '', H14: '', H15: '', H16: '', H17: '', H18: '', H19: '', H20: '', H21: '', H22: '', H23: '', H24: '', H25: '', H26: '', H27: '', H28: '', H29: '', H30: '', H31: '', H32: '', H33: '', H34: '', H35: '', H36: '', H37: '', H38: '', H39: '', H40: '', H41: '', H42: '', H43: '', H44: '', H45: '', H46: '', H47: '', H48: '' },
            { H1: 'REDUCCIÓN DE BANDA A (MW)' },
            { H1: '', H2: '', H3: '', H4: '', H5: '', H6: '', H7: '', H8: '', H9: '', H10: '', H11: '', H12: '', H13: '', H14: '', H15: '', H16: '', H17: '', H18: '', H19: '', H20: '', H21: '', H22: '', H23: '', H24: '', H25: '', H26: '', H27: '', H28: '', H29: '', H30: '', H31: '', H32: '', H33: '', H34: '', H35: '', H36: '', H37: '', H38: '', H39: '', H40: '', H41: '', H42: '', H43: '', H44: '', H45: '', H46: '', H47: '', H48: '' },
        ],
        dataSchema: {
            H1: null, H2: null, H3: null, H4: null, H5: null, H6: null, H7: null, H8: null, H9: null, H10: null,
            H11: null, H12: null, H13: null, H14: null, H15: null, H16: null, H17: null, H18: null, H19: null, H20: null,
            H21: null, H22: null, H23: null, H24: null, H25: null, H26: null, H27: null, H28: null, H29: null, H30: null,
            H31: null, H32: null, H33: null, H34: null, H35: null, H36: null, H37: null, H38: null, H39: null, H40: null,
            H41: null, H42: null, H43: null, H44: null, H45: null, H46: null, H47: null, H48: null
        },


        mergeCells: [
            { row: 2, col: 0, rowspan: 1, colspan: 48 },
            { row: 4, col: 0, rowspan: 1, colspan: 48 }
        ],

        colWidths: 55,
        height: '160px',
        width: '100%',
    });

    tblHndBajar.addHook('beforeChange', function (changes, [source]) {
        listaDeErroresBajar = [];
        datosEnDeficitBajar = [];
        datosEnReduccionBajar = [];
    });
    tblHndBajar.addHook('afterChange', function (changes, [source]) {
        listaDeErroresGeneral = [];

        validaRangosDeficitBajar();
        validaRangosReduccionBajar();
        validarExistenciaMotivoSubir();
        validarExistenciaMotivoBajar();

        //Obtengo lista Errores General
        listaDeErroresGeneral = listaDeErroresGeneral.concat(listaDeErroresSubir.concat(listaDeErroresBajar));
    });

    tblHndBajar.addHook('afterInit', validaRangosDeficitBajar);
    tblHndBajar.addHook('afterInit', validaRangosReduccionBajar);
    tblHndBajar.addHook('afterInit', validarExistenciaMotivoBajar);
    tblHndBajar.init();

    //valido existe de dato en deficit de RSF en bajar
    function validaRangosDeficitBajar() {
        datosEnReduccionGeneral = datosEnReduccionSubir.concat(datosEnReduccionBajar);
        datosEnDeficitGeneral = datosEnDeficitSubir.concat(datosEnDeficitBajar);
        //
        agregarValErrorSinDeficitNiReduccion();
    }

    //valido existe de dato en Reduccion en bajar
    function validaRangosReduccionBajar() {
        datosEnDeficitGeneral = datosEnDeficitSubir.concat(datosEnDeficitBajar);
        datosEnReduccionGeneral = datosEnReduccionSubir.concat(datosEnReduccionBajar);
        agregarValErrorSinDeficitNiReduccion();
    }

    precargarDetallesOtraPestania();
    
}

function validarExistenciaMotivoSubir() { //
    //solo valida motivo si se ingresa deficit de RSF
    let numDatos = datosEnDeficitSubir.length;

    if (numDatos > 0) {
        var motivos = $("#cbMotivosSubir").val();
        var numMotivos = motivos != null ? motivos.length : 0;

        if (numMotivos == 0) {
            agregarErrorMotivo(SUBIR, "Motivo", "Se debe escoger motivo(s).");
        } else {
            quitarErrorMotivo(SUBIR);
        }
    } else {
        quitarErrorMotivo(SUBIR);
    }
}

function validarExistenciaMotivoBajar() {
    //solo valida motivo si se ingresa deficit de RSF
    let numDatos = datosEnDeficitBajar.length;

    if (numDatos > 0) {
        var motivos = $("#cbMotivosBajar").val();
        var numMotivos = motivos != null ? motivos.length : 0;

        if (numMotivos == 0) {
            agregarErrorMotivo(BAJAR, "Motivo", "Se debe escoger motivo(s).");
        } else {
            quitarErrorMotivo(BAJAR);
        }
    } else {
        quitarErrorMotivo(BAJAR);
    }
}

function datosIngresadosSubirRenderer(instance, td, row, col, prop, value, cellProperties) {
    Handsontable.renderers.TextRenderer.apply(this, arguments);

    datosIngresadosRenderer(instance, td, row, col, prop, value, cellProperties, TIPOINFO_SUBIR);
}

function datosIngresadosBajarRenderer(instance, td, row, col, prop, value, cellProperties) {
    Handsontable.renderers.TextRenderer.apply(this, arguments);

    datosIngresadosRenderer(instance, td, row, col, prop, value, cellProperties, TIPOINFO_BAJAR);
}

function datosIngresadosRenderer(instance, td, row, col, prop, value, cellProperties, tipoInformacion) {
    
    var tipoInfo = "";
    if (tipoInformacion == TIPOINFO_SUBIR) tipoInfo = SUBIR;
    if (tipoInformacion == TIPOINFO_BAJAR) tipoInfo = BAJAR;

    var tipoDato = "";
    if (row == 3) tipoDato = "Déficit de RSF";
    if (row == 5) tipoDato = "Reducción de banda a (MW)";

    var numEscenario = parseInt(prop.substring(1));

    if (value != null && value != "") {

        //Valido si es NO numerico    
        if (isNaN(value)) {
            td.className = 'celda_error_roja htCenter';
            agregarError(tipoInfo, tipoDato, numEscenario, value, "Solo se permiten valores numéricos (con punto como separador decimal) mayores a cero.");
            if (row == 3) {
                if (tipoInfo == SUBIR) datosEnDeficitSubir.push(value);
                if (tipoInfo == BAJAR) datosEnDeficitBajar.push(value);
            }
            if (row == 5) {
                if (tipoInfo == SUBIR) datosEnReduccionSubir.push(value);
                if (tipoInfo == BAJAR) datosEnReduccionBajar.push(value);
            }
        } else {
            //valido si son negativos
            if (value < 0) {
                td.className = 'celda_error_roja htCenter';
                agregarError(tipoInfo, tipoDato, numEscenario, value, "Solo se permiten valores numéricos (con punto como separador decimal) mayores a cero.");
                if (row == 3) {
                    if (tipoInfo == SUBIR) datosEnDeficitSubir.push(value);
                    if (tipoInfo == BAJAR) datosEnDeficitBajar.push(value);
                }
                if (row == 5) {
                    if (tipoInfo == SUBIR) datosEnReduccionSubir.push(value);
                    if (tipoInfo == BAJAR) datosEnReduccionBajar.push(value);
                }
            } else {
                //Obtengo si el valor real ingresado tiene decimales
                var pE = Math.trunc(value);
                var pD;
                var arrN = (value + "").split(".");
                if (arrN.length > 1) {
                    pD = (value + "").replace((pE + ""), "0");
                }
                var pdecR = pD != undefined ? (pD + "").substring(2) : "";
                var numPDNumReal = pdecR.length;

                //Antes de validar lo convierto y redondeo a maximo 2 decimales
                //var valo1 = parseFloat(td.innerHTML);
                var valo2 = parseFloat(value);
                //var nuevoVal1 = valo1.toFixed(2);
                var nuevoVal2 = valo2.toFixed(2);

                //Cambio valor a 2 decimales en cada celda, solo si el numero real tiene decimales
                if (numPDNumReal > 0) {
                    td.innerHTML = valo2.toFixed(2);
                }

                //Uso el nuevo valor (max 2 decimales) para la s validaciones
                value = nuevoVal2;

                //valido si son positivos pero con mas de 3 cifras enteras o mas de 1 cifra decimal
                //var valor = new Decimal(value);
                var parteEntera = Math.trunc(value);
                //var parteDecimal = valor.sub(parteEntera);     
                var parteDecimal;
                var arrNum = (value + "").split(".");
                if (arrNum.length > 1) {
                    parteDecimal = (value + "").replace((parteEntera + ""), "0");
                }
                
                var pe = parteEntera + "";
                var pd = parteDecimal != undefined ? (parteDecimal + "").substring(2): "";
                var numPE = pe.length;
                var numPD = pd.length;                                  

                //Solo es aceptado si tiene hasta 3 cifras enteras y 2 cifra2 decimales
                if (numPE > 3 || numPD > 2) {
                    td.className = 'celda_error_roja htCenter';
                    agregarError(tipoInfo, tipoDato, numEscenario, value, "Solo está permitido el ingreso de valores con máximo 3 cifras enteras y/o como máximo 2 cifras decimales.");
                    if (row == 3) {
                        if (tipoInfo == SUBIR) datosEnDeficitSubir.push(value);
                        if (tipoInfo == BAJAR) datosEnDeficitBajar.push(value);
                    }
                    if (row == 5) {
                        if (tipoInfo == SUBIR) datosEnReduccionSubir.push(value);
                        if (tipoInfo == BAJAR) datosEnReduccionBajar.push(value);
                    }
                } else {
                    if (value > 0) {
                        td.className = 'celda_azul_claro htCenter';
                        if (row == 3) {
                            if (tipoInfo == SUBIR) datosEnDeficitSubir.push(value);
                            if (tipoInfo == BAJAR) datosEnDeficitBajar.push(value);
                        }
                        if (row == 5) {
                            if (tipoInfo == SUBIR) datosEnReduccionSubir.push(value);
                            if (tipoInfo == BAJAR) datosEnReduccionBajar.push(value);
                        }
                    }                    
                }
            }
        }
    } 
        
}

function agregarError(tipoInfo, tipoDato, numEscenario, valor, mensajeError) {
    //Agrega al array de tipo de informacion
    if (validarError(tipoInfo, tipoDato, numEscenario, valor, mensajeError)) {
        var regError = {
            TipoInfo: tipoInfo,
            TipoDato: tipoDato,
            NumEscenario: numEscenario,
            Valor: valor,
            Mensaje: mensajeError
        };

        if (tipoInfo === SUBIR)
            listaDeErroresSubir.push(regError);
        else{
            if (tipoInfo === BAJAR)
                listaDeErroresBajar.push(regError);
            else {
                listaDeErroresGeneral.push(regError); //para validar existencia de rangos en deficit
            }
                
        }
    }

   
}

function validarError(tipoInfo, tipoDato, numEscenario, valor, mensajeError) {
    var arrayData = [];

    if (tipoInfo === SUBIR) {
        arrayData = listaDeErroresSubir.slice();
    }
    else {
        if (tipoInfo === BAJAR) {
            arrayData = listaDeErroresBajar.slice();
        } else {
            arrayData = listaDeErroresGeneral.slice();
        }
    }
                   

    for (var j in arrayData) {
        if (arrayData[j]['TipoInfo'] == tipoInfo && arrayData[j]['TipoDato'] == tipoDato && arrayData[j]['NumEscenario'] == numEscenario && arrayData[j]['Valor'] == valor && arrayData[j]['Mensaje'] == mensajeError) {
            return false;
        }
    }
    return true;
}

function agregarErrorMotivo(tipoInfo, tipoDato, mensajeError) {
    //Agrega al array de tipo de informacion
    if (validarErrorMotivo(tipoInfo, tipoDato, mensajeError)) {
        var regError = {
            TipoInfo: tipoInfo,
            TipoDato: tipoDato,
            NumEscenario: "",
            Valor: "",
            Mensaje: mensajeError
        };
        listaDeErroresGeneral.push(regError);
          
    }


}
function validarErrorMotivo(tipoInfo, tipoDato, mensajeError) {
    var arrayData = [];

    arrayData = listaDeErroresGeneral.slice();

    for (var j in arrayData) {
        if (arrayData[j]['TipoInfo'] == tipoInfo && arrayData[j]['TipoDato'] == tipoDato && arrayData[j]['Mensaje'] == mensajeError) {
            return false;
        }
    }
    return true;
}

function quitarErrorMotivo(tipoInfo) {
    const index = listaDeErroresGeneral.findIndex(x => x.TipoInfo === tipoInfo && x.TipoDato === "Motivo");
    
    if (index >= 0) {
        const filaEliminada = listaDeErroresGeneral.splice(index, 1);
    }
    

}

function mostrarListadoErrores() {
    limpiarBarraMensaje("mensaje_activacion");

    if (!ventanaActivacionSoloLectura) {
        setTimeout(function () {
            $('#tablaErrores').html(dibujarTablaErrores());

            $('#contenedorErrores').bPopup({
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

        }, 200);
    } else {
        mostrarMensaje('mensaje_activacion', 'error', MSG_ERROR_LECTURA);
    }

    
}

function dibujarTablaErrores() {
    var cadena = `
        <div style='clear:both; height:5px'></div>
            <table id='tablaError' border='1' class='pretty tabla-adicional' cellspacing='0' style='width: 850px;'>
                <thead>
                    <tr>
                        <th style='width: 60px;'>Tipo Información</th>
                        <th style='width: 120px;'>Dato</th>
                        <th style='width: 80px;'>Escenario</th>
                        <th style='width: 140px;'>Valor</th>
                        <th style='width: 400px;'>Tipo Error</th>
                    </tr>
                </thead>
                <tbody>
    `;

    //ordeno el listado
    listaDeErroresGeneral.sort(function (a, b) {
        return b.TipoInfo.localeCompare(a.TipoInfo) || a.NumEscenario - b.NumEscenario || a.TipoDato.localeCompare(b.TipoDato);
    });


    for (var i = 0; i < listaDeErroresGeneral.length; i++) {
        var item = listaDeErroresGeneral[i];
        cadena += `
                    <tr>
                        <td style='width:  60px; text-align: left; white-space: break-spaces;'>${item.TipoInfo}</td>
                        <td style='width: 120px; text-align: left; white-space: break-spaces;'>${item.TipoDato}</td>
                        <td style='width:  80px; text-align: left; white-space: break-spaces;'>${item.NumEscenario}</td>
                        <td style='width: 140px; text-align: left; white-space: break-spaces;'>${item.Valor}</td>
                        <td style='width: 400px; text-align: left; white-space: break-spaces;'>${item.Mensaje}</td>
                    </tr>
        `;
    }

    cadena += `
                </tbody>
            </table>
        </div>
    `;

    return cadena;
}

function activarOfertasPorDefecto() {
    limpiarBarraMensaje("mensaje_activacion");

    if (!ventanaActivacionSoloLectura) {
        //Verifico la existencia de errores
        if (listaDeErroresGeneral.length == 0) {
            guardarActivacionDeOfertas();
        } else {
            mostrarListadoErrores();
        }
    } else {
        mostrarMensaje('mensaje_activacion', 'error', MSG_ERROR_LECTURA);
    }
}

function guardarActivacionDeOfertas() {
    limpiarBarraMensaje("mensaje_activacion");

    var dataHandsonSubir, dataHandsonBajar, dataValidoSubir, dataValidoBajar = [];

    dataHandsonSubir = tblHndSubir.getSourceData();
    dataHandsonBajar = tblHndBajar.getSourceData();

    var dataGuardar = obtenerDataGuardar();
    var dataJson = {
        fechaOferta:  $("#fechaOferta").val(),
        datosAGuardar: dataGuardar
    };
    
    $.ajax({
        url: controlador + "GuardarActivacionOferta",
        type: 'POST',
        contentType: 'application/json; charset=UTF-8',
        dataType: 'json',
        data: JSON.stringify(dataJson),
        success: function (result) {

            if (result.Resultado != "-1") {
                cargarDatosActivacion();
                var msgNA = result.Resultado != "1" ? "No se encontró Oferta por defecto registrada en los últimos 5 años para las siguientes URS: " + result.Resultado + ". Debe gestionar su registro." : "";
                if (result.Resultado != "2")
                    mostrarMensaje('mensaje_activacion', 'exito', 'Se guardo la información ingresada y se realizó el proceso de activación de forma correcta. ' + msgNA);
                if (result.Resultado == "2")
                    mostrarMensaje('mensaje_activacion', 'alert', 'Se guardo la información ingresada pero no se realizó el proceso de activación.');
            } else {
                mostrarMensaje('mensaje_activacion', 'error', result.Mensaje);
            }
        },
        error: function (xhr, status) {
            mostrarMensaje('mensaje_activacion', 'error', 'Se ha producido un error.');
        }
    });
    
}

function obtenerDataGuardar(dataHandson) {
    
    var datosDeficitSubir, datosReduccionBandaSubir, datosDeficitBajar, datosReduccionBandaBajar = [];
    var lstIdsMotivoSubir = $("#cbMotivosSubir").val();
    var lstIdsMotivoBajar = $("#cbMotivosBajar").val();

    datosDeficitSubir = ObtenerInformacionDeHandson(3, SUBIR);
    datosReduccionBandaSubir = ObtenerInformacionDeHandson(5, SUBIR);
    datosDeficitBajar = ObtenerInformacionDeHandson(3, BAJAR);
    datosReduccionBandaBajar = ObtenerInformacionDeHandson(5, BAJAR);

    var dataGuardar = {
        DatosDeficitSubir: datosDeficitSubir,
        DatosReduccionBandaSubir: datosReduccionBandaSubir,
        IdsMotivosSubir: lstIdsMotivoSubir != null ? lstIdsMotivoSubir : [],
        DatosDeficitBajar: datosDeficitBajar,
        DatosReduccionBandaBajar: datosReduccionBandaBajar,
        IdsMotivosBajar: lstIdsMotivoBajar != null ? lstIdsMotivoBajar : []
    };
  
    return dataGuardar;
}

function ObtenerInformacionDeHandson(fila, tipoInfo) {

    var dataHandson = [];

    if (tipoInfo == SUBIR)
        dataHandson = tblHndSubir.getSourceData();
    if (tipoInfo == BAJAR)
        dataHandson = tblHndBajar.getSourceData();


    var lstData = dataHandson[fila];

    lstData.H1 = formatearValor2Decimales(lstData.H1);
    lstData.H2 = formatearValor2Decimales(lstData.H2);
    lstData.H3 = formatearValor2Decimales(lstData.H3);
    lstData.H4 = formatearValor2Decimales(lstData.H4);
    lstData.H5 = formatearValor2Decimales(lstData.H5);
    lstData.H6 = formatearValor2Decimales(lstData.H6);
    lstData.H7 = formatearValor2Decimales(lstData.H7);
    lstData.H8 = formatearValor2Decimales(lstData.H8);
    lstData.H9 = formatearValor2Decimales(lstData.H9);

    lstData.H10 = formatearValor2Decimales(lstData.H10);
    lstData.H11 = formatearValor2Decimales(lstData.H11);
    lstData.H12 = formatearValor2Decimales(lstData.H12);
    lstData.H13 = formatearValor2Decimales(lstData.H13);
    lstData.H14 = formatearValor2Decimales(lstData.H14);
    lstData.H15 = formatearValor2Decimales(lstData.H15);
    lstData.H16 = formatearValor2Decimales(lstData.H16);
    lstData.H17 = formatearValor2Decimales(lstData.H17);
    lstData.H18 = formatearValor2Decimales(lstData.H18);
    lstData.H19 = formatearValor2Decimales(lstData.H19);

    lstData.H20 = formatearValor2Decimales(lstData.H20);
    lstData.H21 = formatearValor2Decimales(lstData.H21);
    lstData.H22 = formatearValor2Decimales(lstData.H22);
    lstData.H23 = formatearValor2Decimales(lstData.H23);
    lstData.H24 = formatearValor2Decimales(lstData.H24);
    lstData.H25 = formatearValor2Decimales(lstData.H25);
    lstData.H26 = formatearValor2Decimales(lstData.H26);
    lstData.H27 = formatearValor2Decimales(lstData.H27);
    lstData.H28 = formatearValor2Decimales(lstData.H28);
    lstData.H29 = formatearValor2Decimales(lstData.H29);

    lstData.H30 = formatearValor2Decimales(lstData.H30);
    lstData.H31 = formatearValor2Decimales(lstData.H31);
    lstData.H32 = formatearValor2Decimales(lstData.H32);
    lstData.H33 = formatearValor2Decimales(lstData.H33);
    lstData.H34 = formatearValor2Decimales(lstData.H34);
    lstData.H35 = formatearValor2Decimales(lstData.H35);
    lstData.H36 = formatearValor2Decimales(lstData.H36);
    lstData.H37 = formatearValor2Decimales(lstData.H37);
    lstData.H38 = formatearValor2Decimales(lstData.H38);
    lstData.H39 = formatearValor2Decimales(lstData.H39);

    lstData.H40 = formatearValor2Decimales(lstData.H40);
    lstData.H41 = formatearValor2Decimales(lstData.H41);
    lstData.H42 = formatearValor2Decimales(lstData.H42);
    lstData.H43 = formatearValor2Decimales(lstData.H43);
    lstData.H44 = formatearValor2Decimales(lstData.H44);
    lstData.H45 = formatearValor2Decimales(lstData.H45);
    lstData.H46 = formatearValor2Decimales(lstData.H46);
    lstData.H47 = formatearValor2Decimales(lstData.H47);
    lstData.H48 = formatearValor2Decimales(lstData.H48);

    return lstData;
}

function formatearValor2Decimales(valorFormatear) {
    var salida = "";

    if (valorFormatear != null) {
        if (isNaN(valorFormatear)) {
            if (valorFormatear.trim() != "") {
                salida = parseFloat(valorFormatear).toFixed(2).toString();
            } else {
                salida = "";
            }
        } else {
            salida = parseFloat(valorFormatear).toFixed(2).toString();
        }
    } else {
        salida = null;
    }

    return salida;
}

function mostrarListadoActivaciones() {
    limpiarBarraMensaje("mensaje_activacion");

    setTimeout(function () {
        $('#tablaActivaciones').html(dibujarTablaActivaciones());

        $('#contenedorActivaciones').bPopup({
            easing: 'easeOutBack',
            speed: 450,
            transition: 'slideDown',
            modalClose: false
        });
        $('#tblActivaciones').dataTable({
            "scrollY": 330,
            "scrollX": true,
            "sDom": 't',
            "ordering": false,
            "bPaginate": false,
            "iDisplayLength": -1
        });

    }, 200);

}

function dibujarTablaActivaciones() {
    var cadena = `
        <div style='clear:both; height:5px'></div>
            <table id='tblActivaciones' border='1' class='pretty tabla-adicional' cellspacing='0' style='width: 550px;'>
                <thead>
                    <tr>
                        <th style='width: 120px;'>Fecha de Oferta</th>
                        <th style='width: 120px;'>Fecha de Activación</th>
                        <th style='width: 150px;'>Usuario</th>
                    </tr>
                </thead>
                <tbody>
    `;    


    for (var i = 0; i < listaHistorialActivaciones.length; i++) {
        var item = listaHistorialActivaciones[i];
        cadena += `
                    <tr style="cursor:pointer;" onclick="abrirDatosActivacion(${item.Smapaccodi});">
                        <td title="Abrir datos de Activación" style='width: 120px; text-align: center; white-space: break-spaces;'>${item.SmapacfechaDesc}</td>
                        <td title="Abrir datos de Activación" style='width: 120px; text-align: center; white-space: break-spaces;'>${item.SmapacfeccreacionDesc}</td>
                        <td title="Abrir datos de Activación" style='width: 150px; text-align: center; white-space: break-spaces;'>${item.Smapacusucreacion}</td>
                    </tr>
        `;
    }

    cadena += `
                </tbody>
            </table>
        </div>
    `;

    return cadena;
}

function abrirDatosActivacion(smapaccodi) {
    var sdf = 0;
    cerrarPopup('contenedorActivaciones');

    
        ventanaActivacionSoloLectura = true;
    

    limpiarBarraMensaje("mensaje_activacion");

    $.ajax({
        type: 'POST',
        url: controlador + "CargarDatosHistorial",
        data: {
            smapaccodi: smapaccodi
        },
        success: function (evt) {

            if (evt.Resultado == "1") {

                var data = evt.DataActivacionOferta;
                listarMotivos("cbMotivosSubir", evt.ListaMotivosActivacionSubir);
                listarMotivos("cbMotivosBajar", evt.ListaMotivosActivacionBajar);

                inicializarNuevaActivacion(true);
                iniciarHandsontables();
               
                

                if (data.ExisteActivacionOferta) {
                    cargarHansonTablas(data);
                }

            } else {
                mostrarMensaje('mensaje_activacion', 'error', evt.Mensaje);
            }
        },
        error: function (xhr, status) {
            mostrarMensaje('mensaje_activacion', 'error', 'Se ha producido un error.');
        }
    });
}

//cbMotivosBajar
function listarMotivos(idCombo, listadoMotivos) {
    $('#' + idCombo).empty();
    //Listamos identificadores
    if (listadoMotivos.length > 0) {
        for (var i = 0; i < listadoMotivos.length; i++) {
            $('#' + idCombo).append('<option value=' + listadoMotivos[i].Smammcodi + '>' + listadoMotivos[i].Smammdescripcion + '</option>');  // listadoMotivos es List<string>, si es objeto usar asi ListaIdentificador[i].Equicodi
        }

    }

    $('#cbMotivosSubir').multipleSelect({
        filter: true,
        maxHeight: +120,
        maxHeightUnit: 'row'
    });
    $('#cbMotivosSubir').change(function () {
        validarExistenciaMotivoSubir();
    });
    $('#cbMotivosBajar').multipleSelect({
        filter: true,
        maxHeight: +120,
        maxHeightUnit: 'row'
    });
    $('#cbMotivosBajar').change(function () {
        validarExistenciaMotivoBajar();
    });

    //Bloqueo seleccion si estan en modo lectura

    if (ventanaActivacionSoloLectura) {
        $('input[type=checkbox][name^="selectAllIdMotivoSubir"]').each(function () {
            document.getElementsByName(this.name)[0].disabled = true;
        });
        $('input[type=checkbox][name^="selectItemIdMotivoSubir"]').each(function (index) {
            document.getElementsByName(this.name)[index].disabled = true;
        });

        $('input[type=checkbox][name^="selectAllIdMotivoBajar"]').each(function () {
            document.getElementsByName(this.name)[0].disabled = true;
        });
        $('input[type=checkbox][name^="selectItemIdMotivoBajar"]').each(function (index) {
            document.getElementsByName(this.name)[index].disabled = true;
        });
    }


    
}

/**********************************************/
/************ Funciones Generales  ************/


function AddZero(num) {
    return (num >= 0 && num < 10) ? "0" + num : num + "";
}

function diaManiana() { //devuelve strFecha en formato dd/mm/yyyy
    var hoy = new Date();
    let DIA_EN_MILISEGUNDOS = 24 * 60 * 60 * 1000;
    //let DIA_EN_MILISEGUNDOS = 48 * 60 * 60 * 1000;
    let manana = new Date(hoy.getTime() + DIA_EN_MILISEGUNDOS);
    var strDateTime = [[AddZero(manana.getDate()), AddZero(manana.getMonth() + 1), manana.getFullYear()].join("/")].join(" ");

    return strDateTime;
}


function cerrarPopup(id) {
    $("#" + id).bPopup().close()
}

function mostrarMensaje(id, tipo, mensaje) {
    $("#" + id).css("display", "block");
    $('#' + id).removeClass();
    $('#' + id).addClass('action-' + tipo);
    $('#' + id).html(mensaje);

}

function limpiarBarraMensaje(id) {
    $('#' + id).css("display", "none");
    $('#' + id).removeClass();
    $('#' + id).addClass('action-message');
    $('#' + id).html('');
}


function deshabilitarSelect(id) {
    document.getElementById(id).disabled = true;
    $("#" + id).css("background", "#F2F4F3");
}

function habilitarSelect(id) {
    document.getElementById(id).disabled = false;
    $("#" + id).css("background", "white");
}


function precargarDetallesOtraPestania() {
    
    if (tabSeleccionado == TAB_ACTIVACION) {

        tblHndSubir.updateSettings({
            cells(row, col, prop) {
                const cellProperties = {};

                switch (row) {
                    case 0:
                    case 1:
                        cellProperties.readOnly = true;
                        cellProperties.className = 'htCenter fondo_activacion_escenarios ';
                        break;

                    case 2:
                    case 4:
                        cellProperties.readOnly = true;
                        cellProperties.className = 'fondo_activacion_datos celda_negrita ';
                        break;

                    case 3:
                    case 5:
                        cellProperties.readOnly = ventanaActivacionSoloLectura;
                        cellProperties.renderer = "datosIngresadosSubirRenderer"; // uses lookup map
                        break;
                    default:
                }

                return cellProperties;
            },


        });

        tblHndBajar.updateSettings({
            cells(row, col, prop) {
                const cellProperties = {};

                switch (row) {
                    case 0:
                    case 1:
                        cellProperties.readOnly = true;
                        cellProperties.className = 'htCenter fondo_activacion_escenarios ';
                        break;

                    case 2:
                    case 4:
                        cellProperties.readOnly = true;
                        cellProperties.className = 'fondo_activacion_datos celda_negrita ';
                        break;

                    case 3:
                    case 5:
                        cellProperties.readOnly = ventanaActivacionSoloLectura;
                        cellProperties.renderer = "datosIngresadosBajarRenderer"; // uses lookup map
                        break;
                    default:
                }

                return cellProperties;
            },


        });

    }

    if (tabSeleccionado == TAB_INDISPONIBILIDAD) {

        if (tblHndIndisponibilidad != null) {
            tblHndIndisponibilidad.updateSettings({
                cells(row, col, prop) {
                    const cellProperties = {};

                    var colBandaUrs = 3;
                    var colIndisponibilidad = 4;
                    var colTipo = 5;
                    var colBandaDisp = 6;
                    var colMotivo = 7;

                    switch (col) {
                        case colIndisponibilidad:
                            cellProperties.renderer = "datosIngresadosCombosIndisponibilidadRenderer";
                            break;
                        case colBandaDisp:
                        case colMotivo:
                            cellProperties.renderer = "datosIngresadosIndisponibilidadRenderer"; // uses lookup map//
                            break;
                        default:
                    }
                    if (col == colIndisponibilidad) {
                        if (prop == 'IntdetindexisteDesc') {
                            if (ventanaIndispSoloLectura) {
                                cellProperties.readOnly = true;
                                cellProperties.className = 'fondo_soloLectura_indisponibilidad';
                            }
                        }
                    }

                    if (col == colTipo) {
                        if (prop == 'IntdettipoDesc') {
                            if (!ventanaIndispSoloLectura) {
                                var valorIndisponibilidad = tblHndIndisponibilidad.getDataAtCell(row, colIndisponibilidad);

                                if (valorIndisponibilidad == 'No') {
                                    cellProperties.className = 'fondo_soloLectura_indisponibilidad';
                                    cellProperties.readOnly = true;
                                } else {
                                    if (valorIndisponibilidad == 'Si') {
                                        cellProperties.className = 'fondo_editable_indisponibilidad';
                                        cellProperties.readOnly = false;
                                    } else {
                                        var sdf = 0;
                                    }
                                }
                            }
                            else {
                                cellProperties.readOnly = true;
                                cellProperties.className = 'fondo_soloLectura_indisponibilidad';
                            }
                        }
                    }

                    if (col == colBandaDisp) {
                        if (prop == 'Intdetbanda') {
                            if (!ventanaIndispSoloLectura) {
                                var valorIndisponibilidad = tblHndIndisponibilidad.getDataAtCell(row, colIndisponibilidad);
                                var valorTipo = tblHndIndisponibilidad.getDataAtCell(row, colTipo);

                                if (valorIndisponibilidad == 'No') {
                                    cellProperties.className = 'fondo_soloLectura_indisponibilidad';
                                    cellProperties.readOnly = true;

                                } else {
                                    if (valorIndisponibilidad == 'Si') {

                                        if (valorTipo == 'Parcial') {

                                            cellProperties.className = 'fondo_editable_indisponibilidad';
                                            cellProperties.readOnly = false;

                                        } else {
                                            if (valorTipo == 'Total') {

                                                cellProperties.className = 'fondo_soloLectura_indisponibilidad';
                                                cellProperties.readOnly = true;

                                            }
                                        }

                                    }
                                }
                            } else {
                                cellProperties.readOnly = true;
                                cellProperties.className = 'fondo_soloLectura_indisponibilidad';
                            }
                        }
                    }

                    if (col == colMotivo) {
                        if (prop == 'Intdetmotivo') {
                            if (!ventanaIndispSoloLectura) {
                                var valorIndisponibilidad = tblHndIndisponibilidad.getDataAtCell(row, colIndisponibilidad);

                                if (valorIndisponibilidad == 'No') {
                                    cellProperties.className = 'fondo_soloLectura_indisponibilidad';
                                    cellProperties.readOnly = true;
                                } else {
                                    if (valorIndisponibilidad == 'Si') {
                                        cellProperties.className = 'fondo_editable_indisponibilidad';
                                        cellProperties.readOnly = false;
                                    }
                                }
                            } else {
                                cellProperties.readOnly = true;
                                cellProperties.className = 'fondo_soloLectura_indisponibilidad';
                            }
                        }
                    }

                    return cellProperties;
                },
            });
        }
    }
}