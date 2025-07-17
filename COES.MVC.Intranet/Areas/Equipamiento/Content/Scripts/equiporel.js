var controlador = siteRoot + 'Equipamiento/Relacion/';
var ANCHO_LISTADO = 900;

$(function () {
    $('#tab-container').easytabs({
        animate: false
    });

    $("#cbFam2").change(function () {
        generaListado();
    });
    $('#btnBuscar').click(function () {
        generaListado();
    });

    $('#btnRegresar').click(function () {
        location.href = controlador + "IndexTipoRel";
    });
    $("#btnNuevo").click(function () {
        nuevoEquiporel();
    });
    $("#cbFam").change(function () {
        APP_MOSTRAR_MODAL_EQUIPO_YA_CARGADO = 0;
    });

    generaListado();
});

function generaListado() {
    $('#listado').html('');
    $("#hfFamnomb1Reporte").val('');
    $("#hfFamnomb1Reporte").val('');
    ANCHO_LISTADO = $('#mainLayout').width() - 30;

    var idTipoRel = getIdTipoRel();

    var famcodis = $("#cbFam2").val();
    famcodis = famcodis != undefined && famcodis != null && famcodis != '' ? famcodis : '0-0';
    var famcodi1 = parseInt(famcodis.split('-')[0]) || 0;
    var famcodi2 = parseInt(famcodis.split('-')[1]) || 0;

    $.ajax({
        type: 'POST',
        url: controlador + "ListarEquipoRel",
        data: {
            idTipoRel: idTipoRel,
            famcodi1: famcodi1,
            famcodi2: famcodi2
        },
        success: function (evt) {
            if (evt.Resultado != "-1") {
                $('#listado').css("width", ANCHO_LISTADO + "px");
                $('#listado').html(evt.Resultado);

                $('#tablaReporte').dataTable({
                    "scrollY": $("#listado").height() > 400 ? 400 + "px" : "100%",
                    "scrollX": false,
                    "sDom": 'ft',
                    "ordering": true,
                    "iDisplayLength": -1
                });
            }
            else
                alert("Ha ocurrido un error: " + evt.Mensaje);

        },
        error: function (err) {
            alert("Ha ocurrido un error");
        }
    });
}

function getIdTipoRel() {
    return parseInt($("#tiporelcodi").val()) || 0;
}

function getFamcodi1() {
    var famcodis = $("#cbFam").val();
    famcodis = famcodis != undefined && famcodis != null && famcodis != '' ? famcodis : '0-0';
    var famcodi1 = parseInt(famcodis.split('-')[0]) || 0;
    return famcodi1;
}

function getFamcodi2() {
    var famcodis = $("#cbFam").val();
    famcodis = famcodis != undefined && famcodis != null && famcodis != '' ? famcodis : '0-0';
    var famcodi2 = parseInt(famcodis.split('-')[1]) || 0;
    return famcodi2;
}

///////////////////////////
/// Formulario
///////////////////////////

function nuevoEquiporel() {
    APP_OPCION = OPCION_NUEVO;
    inicializarFormulario();

    $("#btnBuscarEquipoPopup1").show();
    $("#btnBuscarEquipoPopup2").show();
    $("#btnAceptar2").show();
    $("#btnCancelar2").show();
}

function guardarRelacion() {
    var entity = getObjetoJson();
    if (confirm('¿Desea guardar la Relación de Equipos?')) {
        var msj = validarRelacion(entity);

        if (msj == "") {
            var obj = JSON.stringify(entity);

            $.ajax({
                type: 'POST',
                dataType: 'json',
                traditional: true,
                url: controlador + 'GuardarRelacionEquipo',
                data: {
                    strJsonData: obj
                },
                cache: false,
                success: function (result) {
                    if (result.Resultado == '-1') {
                        alert('Ha ocurrido un error: ' + result.Mensaje);
                    } else {
                        alert("Se guardó correctamente la relación");
                        nuevoEquiporel();
                        $("#formulario").hide();

                        $('#tab-container').easytabs('select', '#vistaListado');
                        generaListado();
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

function eliminarEquipoRelacion(equicodi1, equicodi2) {
    if (confirm("¿Desea eliminar la relación de equipos?")) {
        var tiporelcodi = parseInt($("#tiporelcodi").val()) || 0;

        $.ajax({
            type: 'POST',
            dataType: 'json',
            traditional: true,
            url: controlador + 'EliminarRelacionEquipo',
            data: {
                tiporelcodi: tiporelcodi,
                equicodi1: equicodi1,
                equicodi2: equicodi2
            },
            cache: false,
            success: function (result) {
                if (result.Resultado == '-1') {
                    alert('Ha ocurrido un error:' + result.Mensaje);
                } else {
                    alert("Se eliminó correctamente la relación");

                    $('#tab-container').easytabs('select', '#vistaListado');
                    generaListado();
                }
            },
            error: function (err) {
                alert('Ha ocurrido un error.');
            }
        });
    }
}

function getObjetoJson() {
    var obj = {};

    obj.Equicodi1 = parseInt($("#cboEquipo1").val()) || 0;
    obj.Equicodi2 = parseInt($("#cboEquipo2").val()) || 0;
    obj.Famcodi1 = getFamcodi1();
    obj.Famcodi2 = getFamcodi2();
    obj.Tiporelcodi = parseInt($("#tiporelcodi").val()) || 0;

    return obj;
}

function validarRelacion(obj) {
    var msj = "";

    if (obj.Equicodi1 <= 0 || obj.Equicodi2 <= 0) {
        msj += "Debe seleccionar un equipo." + "\n";
    }

    return msj;
}

function inicializarFormulario() {
    $("#formulario").show();

    $('#btnBuscarEquipoPopup1').unbind();
    $('#btnBuscarEquipoPopup1').click(function () {
        TIPO_POPUP_EQUIPO = POPUP_EQUIPO1;
        visualizarBuscarEquipo();
    });

    $('#btnBuscarEquipoPopup2').unbind();
    $('#btnBuscarEquipoPopup2').click(function () {
        TIPO_POPUP_EQUIPO = POPUP_EQUIPO2;
        visualizarBuscarEquipo();
    });

    $("#cboEmpresa1").val('');
    $("#nomEmpresa1").val('');
    $("#cboEquipo1").val('');
    $("#nomEquipo1").val('');
    $("#cboTipoEquipo1").val('');
    $("#famAbrev1").val('');

    $("#cboEmpresa2").val('');
    $("#nomEmpresa2").val('');
    $("#cboEquipo2").val('');
    $("#nomEquipo2").val('');
    $("#cboTipoEquipo2").val('');
    $("#famAbrev2").val('');

    $("#hfHoDetalleJson").val('');
    $("#hfCodigo").val(0);

    $("#btnAceptar2").unbind();
    $("#btnAceptar2").click(function () {
        guardarRelacion();
    });

    $('#tab-container').easytabs('select', '#vistaDetalle');
}

///////////////////////////
/// Búsqueda Equipo
///////////////////////////
var APP_MOSTRAR_MODAL_EQUIPO_YA_CARGADO = 0;
var TIPO_POPUP_EQUIPO = 0;
var POPUP_EQUIPO1 = 1;
var POPUP_EQUIPO2 = 2;

var LISTA_FAMILIA = [];

var APP_OPCION = -1;
var OPCION_COPIAR = 1;
var OPCION_EDITAR = 2;
var OPCION_NUEVO = 3;
var OPCION_ELIMINAR = 4;
var OPCION_VER = 5;
var ancho = 1100;

function visualizarBuscarEquipo() {

    if (APP_MOSTRAR_MODAL_EQUIPO_YA_CARGADO == 0) {
        cargarBusquedaEquipo(APP_MOSTRAR_MODAL_EQUIPO_YA_CARGADO);
    } else {
        openBusquedaEquipo();
        buscarEquipo();
    }

    APP_MOSTRAR_MODAL_EQUIPO_YA_CARGADO++;
}

function cargarBusquedaEquipo(flag) {
    var idTipoRel = getIdTipoRel();
    var famcodi1 = getFamcodi1();
    var famcodi2 = getFamcodi2();
    LISTA_FAMILIA = [];

    $.ajax({
        type: "POST",
        url: controlador + "BusquedaEquipo",
        data: {
            filtroFamilia: famcodi1 + ',' + famcodi2,
            idTipoRel: idTipoRel,
            famcodi1: famcodi1,
            famcodi2: famcodi2
        },
        global: false,
        success: function (evt) {
            $('#busquedaEquipo').html(evt);
            $("#cbFamiliaEquipo > option").each(function () {
                var obj = { texto: this.text, valor: this.value };
                LISTA_FAMILIA.push(obj);
            });

            openBusquedaEquipo();
        },
        error: function (req, status, error) {
            alert('Ha ocurrido un error.');
        }
    });
}

function openBusquedaEquipo() {
    actualizarListaFamilia();

    $('#busquedaEquipo').bPopup({
        easing: 'easeOutBack',
        speed: 450,
        transition: 'slideDown'
    });
    mostrarAreas();
    mostrarEquipos("0");
    $('#txtFiltro').focus();
}

function actualizarListaFamilia() {
    $('#cbFamiliaEquipo').empty(); 
    //$('#cbFamiliaEquipo').append('<option value="0">--TODOS--</option>');

    var famcodiMostrar = TIPO_POPUP_EQUIPO == POPUP_EQUIPO1 ? getFamcodi1() : getFamcodi2();

    for (var i = 0; i < LISTA_FAMILIA.length; i++) {
        if (LISTA_FAMILIA[i].valor == famcodiMostrar)
            $('#cbFamiliaEquipo').append('<option value=' + LISTA_FAMILIA[i].valor + '>' + LISTA_FAMILIA[i].texto + '</option>');
    }

    $("#hfFiltroFamilia").val(famcodiMostrar);
    mostrarAreas();
    mostrarEquipos("0");
}

function seleccionarEquipo(equicodi, equinomb, Areanomb, Emprnomb, Famabrev, emprcodi, famcodi) {
    if (TIPO_POPUP_EQUIPO == POPUP_EQUIPO1) {
        $("#cboEmpresa1").val('');
        $("#nomEmpresa1").val('');
        $("#cboEquipo1").val('');
        $("#nomEquipo1").val('');
        $("#cboTipoEquipo1").val('');
        $("#famAbrev1").val('');


        $("#cboEmpresa1").val(emprcodi);
        $("#nomEmpresa1").val(Emprnomb);
        $("#cboEquipo1").val(equicodi);
        $("#nomEquipo1").val(Areanomb + " - " + equinomb);
        $("#cboTipoEquipo1").val(famcodi);
        $("#famAbrev1").val(Famabrev);

        $('#busquedaEquipo').bPopup().close();
    }

    if (TIPO_POPUP_EQUIPO == POPUP_EQUIPO2) {
        $("#cboEmpresa2").val('');
        $("#nomEmpresa2").val('');
        $("#cboEquipo2").val('');
        $("#nomEquipo2").val('');
        $("#cboTipoEquipo2").val('');
        $("#famAbrev2").val('');


        $("#cboEmpresa2").val(emprcodi);
        $("#nomEmpresa2").val(Emprnomb);
        $("#cboEquipo2").val(equicodi);
        $("#nomEquipo2").val(Areanomb + " - " + equinomb);
        $("#cboTipoEquipo2").val(famcodi);
        $("#famAbrev2").val(Famabrev);

        $('#busquedaEquipo').bPopup().close();
    }
}

function mostrarError(err) {
    console.log(err);
}