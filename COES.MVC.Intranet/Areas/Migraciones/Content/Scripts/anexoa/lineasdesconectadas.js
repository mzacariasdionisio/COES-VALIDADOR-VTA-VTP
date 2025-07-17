//variables globales
var controlador = siteRoot + 'Migraciones/AnexoA/'
var idEmpresa = "";
var idArea = "";
var idSubestacion = "";
var strequipo = "";
var fechaInicio = "";
var fechaFin = "";

$(function () {

    $('#cbEmpresa').multipleSelect({
        width: '150px',
        filter: true,
        onClose: function (view) {
            cargarLista();
        }
    });
    $('#cbAreaOperativa').multipleSelect({
        width: '150px',
        filter: false,
        onClose: function (view) {
            //cargarSubEstacion();
            cargarLista();
        }

    });
    $('#btnBuscar').click(function () {
        cargarLista();
    });

    $('#txtFechaInicio').Zebra_DatePicker({
        //direction: -1
    });

    $('#txtFechaFin').Zebra_DatePicker({
        //direction: -1
    });

    cargarValoresIniciales();
});

function cargarValoresIniciales() {
    $('#cbEmpresa').multipleSelect('checkAll');
    $('#cbAreaOperativa').multipleSelect('checkAll');
    cargarLista();
    //cargarSubEstacion();
}

function cargarSubEstacion() {

    validacionesxFiltro(1);
    if (resultFiltro) {

        $.ajax({
            type: 'POST',
            url: controlador + 'CargarSubEstacion',
            data: { idArea: idArea },
            success: function (aData) {
                $('#subestacion').html(aData);

            },
            error: function () {
                alert("Ha ocurrido un error");
            }
        });
    }
    else {
    }
}

function cargarLista() {
    validacionesxFiltro(2);
    if (resultFiltro) {
        $.ajax({
            type: 'POST',
            data: { fechaInicio: fechaInicio, fechaFin: fechaFin, empresa: idEmpresa, area: idArea },
            url: controlador + 'CargarListaDesconectadasPorTension',
            success: function (aData) {
                $('#listado').html(aData.Resultado);
                $('#reporte').dataTable({
                    "bAutoWidth": false,
                    "bSort": false,
                    "scrollY": 500,
                    "scrollX": true,
                    "sDom": 't',
                    "iDisplayLength": -1,
                    "language": {
                        "emptyTable": "No Existen registros..!"
                    }
                });
            },
            error: function () {
                alert("Ha ocurrido un error");
            }
        });
    }
    else {
    }
}

//validaciones de los filtros de busqueda
function validacionesxFiltro(valor) {
    var empresa = $('#cbEmpresa').multipleSelect('getSelects');
    if (empresa == "[object Object]") empresa = "-1";
    $('#hfEmpresa').val(empresa);
    idEmpresa = $('#hfEmpresa').val();

    var areaOperativa = $('#cbAreaOperativa').multipleSelect('getSelects');
    if (areaOperativa == "[object Object]") areaOperativa = "-1";
    $('#hfArea').val(areaOperativa);
    idArea = $('#hfArea').val();

    var subestacion = $('#cbSubEstacion').multipleSelect('getSelects');
    if (subestacion == "[object Object]") subestacion = "-1";
    $('#hfSubEstacion').val(subestacion);
    idSubestacion = $('#hfSubEstacion').val();

    strequipo = $('#equipo').val();

    fechaInicio = $('#txtFechaInicio').val();
    fechaFin = $('#txtFechaFin').val();

    var arrayFiltro = arrayFiltro || [];

    if (valor == 1)
        arrayFiltro.push({ id: idEmpresa, mensaje: "Seleccione la opcion Empresa" }, { id: idArea, mensaje: "Seleccione la opcion Área Operativa" });

    else
        arrayFiltro.push({ id: idEmpresa, mensaje: "Seleccione la opcion Empresa" }, { id: idArea, mensaje: "Seleccione la opcion Área" }, { id: idSubestacion, mensaje: "Seleccione la opcion SubEstación" });

    validarFiltros(arrayFiltro);
}