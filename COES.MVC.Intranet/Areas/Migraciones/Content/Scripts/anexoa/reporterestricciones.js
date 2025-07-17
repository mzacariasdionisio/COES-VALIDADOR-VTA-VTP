//variables globales
var controlador = siteRoot + 'Migraciones/AnexoA/'
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

function pintarBusqueda(nPagina) {
    cargarLista(nPagina);
}

function pintarPaginado() {
    var iEmpresa = "";
    if ($("#cbEmpresa option:not(:selected)").length == 0) { iEmpresa = "-1"; }
    else { iEmpresa = idEmpresa; }

    var iUbicacion = "";
    if ($("#cbUbicacion option:not(:selected)").length == 0) { iUbicacion = "-1"; }
    else { iUbicacion = idUbicacion; }

    var iEquipo = "";
    if ($("#cbEquipo option:not(:selected)").length == 0) { iEquipo = "-1"; }
    else { iEquipo = idEquipo; }

    $.ajax({
        type: 'POST',
        async: false,
        url: controlador + "Paginado",
        data: {
            idEmpresa: iEmpresa, idUbicacion: iUbicacion, idEquipo: iEquipo, fechaInicio: fechaInicio, fechaFin: fechaFin
        },
        success: function (evt) {
            $('#paginado').html(evt);
            mostrarPaginado();
        },
        error: function () {
            alert("Ha ocurrido un error en paginado");
        }
    });
}

function cargarUbicacion() {
    /*validacionesxFiltro(1);*/

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
            $('#cbTipoRecursos').html(aData);
            $('#cbUbicacion').multipleSelect({
                width: '150px',
                filter: true,
                onClose: function (view) {
                    /*cargarEquipo();*/
                    pintarBusqueda(1);
                    //pintarPaginado();
                }
            });
            $('#cbUbicacion').multipleSelect('checkAll');
            /*cargarEquipo();*/
            pintarBusqueda(1);
            //pintarPaginado();

        },
        error: function () {
            alert("Ha ocurrido un error");
        }
    });
}

function cargarEquipo() {

    validacionesxFiltro(1);

    if (resultFiltro) {
        $.ajax({
            type: 'POST',
            url: controlador + 'CargarEquipos',
            data: { idEmpresa: idEmpresa, idUbicacion: idUbicacion },
            success: function (aData) {
                $('#equipo').html(aData);
                $('#cbEquipo').multipleSelect({
                    width: '150px',
                    filter: true,
                    onClose: function (view) {
                        pintarBusqueda(1);
                        //pintarPaginado();
                    }
                });

                $('#cbEquipo').multipleSelect('checkAll');

                pintarBusqueda(1);
                //pintarPaginado();
            },
            error: function () {
                alert("Ha ocurrido un error");
            }
        });
    }
}

function cargarLista(nPagina) {

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
        url: controlador + 'CargarListaRestricciones',
        data: { idEmpresa: iEmpresa, idUbicacion: iUbicacion, fechaInicio: fechaInicio, fechaFin: fechaFin },
        success: function (aData) {
            $("#listado").css("width", anchoListado + "px");
            $('#listado').html(aData.Resultado);
            $('#idGraficoContainer').html('');
            $('#tb_restri').dataTable();
            $('#tb_restri2').dataTable();

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