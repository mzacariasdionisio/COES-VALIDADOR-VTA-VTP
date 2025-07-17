//variables globales
var controlador = siteRoot + 'Migraciones/AnexoA/';
var idEmpresa = "";
var idUbicacion = "";
var idEquipo = "-1";
var fechaInicio = "";
var fechaFin = "";
var anchoListado = 900;

$(function () {

    $('#cbEmpresa').multipleSelect({
        width: '250px',
        filter: true,
        onClose: function (view) {
            cargarUbicacion();
        }
    });

    $('#txtFechaInicio').Zebra_DatePicker({
        //direction: -1
    });

    $('#txtFechaFin').Zebra_DatePicker({
        //direction: -1
    });

    anchoListado = $("#mainLayout").width() - 20;

    cargarValoresIniciales();
});



function cargarValoresIniciales() {
    $('#cbEmpresa').multipleSelect('checkAll');

    fechaInicio = $('#txtFechaInicio').val();
    fechaFin = $('#txtFechaFin').val();
    cargarUbicacion();
}

function pintarBusqueda() {
    cargarLista();
}

function cargarUbicacion() {
    var empresa = $('#cbEmpresa').multipleSelect('getSelects');
    if (empresa == "[object Object]") empresa = "-1";
    $('#hfEmpresa').val(empresa);
    idEmpresa = $('#hfEmpresa').val();

    $.ajax({
        type: 'POST',
        async: false,
        url: controlador + 'CargarUbicacion',
        data: { idEmpresa: idEmpresa },
        success: function (aData) {

            $('#ubicacion').html(aData);

            $('#cbUbicacion').multipleSelect({
                width: '150px',
                filter: true,
                onClose: function (view) {
                    pintarBusqueda();
                }
            });
            $('#cbUbicacion').multipleSelect('checkAll');
            pintarBusqueda();
        },
        error: function () {
            alert("Ha ocurrido un error");
        }
    });
}

function cargarLista() {

    var empresa = $('#cbEmpresa').multipleSelect('getSelects');
    if (empresa == "[object Object]") empresa = "-1";
    $('#hfEmpresa').val(empresa);
    iEmpresa = $('#hfEmpresa').val();

    var ubicacion = $('#cbUbicacion').multipleSelect('getSelects');
    if (ubicacion == "[object Object]") ubicacion = "-1";
    $('#hfUbicacion').val(ubicacion);
    iUbicacion = $('#hfUbicacion').val();

    $.ajax({
        type: 'POST',
        async: false,
        url: controlador + 'CargarListaRestriccionesSuministros',
        data: { idEmpresa: iEmpresa, idUbicacion: iUbicacion },
        success: function (aData) {

            $('#listado').html(aData.Resultado);
        },
        error: function () {
            alert("Ha ocurrido un error en cargar lista");
        }
    });
}

//validaciones de los filtros de busqueda
function validacionesxFiltro(valor) {

    var empresa = $('#cbEmpresa').multipleSelect('getSelects');
    if (empresa == "[object Object]") empresa = "-1";
    $('#hfEmpresa').val(empresa);
    idEmpresa = $('#hfEmpresa').val();

    var ubicacion = $('#cbUbicacion').multipleSelect('getSelects');
    if (ubicacion == "[object Object]") ubicacion = "-1";
    $('#hfUbicacion').val(ubicacion);
    idUbicacion = $('#hfUbicacion').val();


    var equipo = "-1"; /*$('#cbEquipo').multipleSelect('getSelects');
    if (equipo == "[object Object]") equipo = "-1";*/
    $('#hfEquipo').val(equipo);
    idEquipo = $('#hfEquipo').val();

    fechaInicio = $('#txtFechaInicio').val();
    fechaFin = $('#txtFechaFin').val();

    var arrayFiltro = arrayFiltro || [];

    if (valor == 1) {
        arrayFiltro.push({ id: idEmpresa, mensaje: "Seleccione la opcion Empresa" }, { id: idUbicacion, mensaje: "Seleccione la opcion Ubicación" });
    }
    else {
        arrayFiltro.push({ id: idEmpresa, mensaje: "Seleccione la opcion Empresa" }, { id: idUbicacion, mensaje: "Seleccione la opcion Ubicación" }/*, { id: idEquipo, mensaje: "Seleccione la opcion Equipo" }*/);
    }
    validarFiltros(arrayFiltro);
}