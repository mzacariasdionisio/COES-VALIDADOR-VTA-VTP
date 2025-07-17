//variables globales
var controlador = siteRoot + 'Migraciones/AnexoA/'
var idEmpresa = "";
var idUbicacion = "";
var idEquipo = "";
var fechaInicio = "";
var fechaFin = "";

$(function () {
    $('#cbEmpresa').multipleSelect({
        width: '150px',
        filter: true,
        onClose: function (view) {
            cargarListaReporteReservaFría();
        }
    });

    $('#btnBuscar').click(function () {
        cargarListaReporteReservaFría();
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
    cargarListaReporteReservaFría();
}

function cargarListaReporteReservaFría() {
    validacionesxFiltro(1);
    if (resultFiltro) {
        $.ajax({
            type: 'POST',
            url: controlador + 'CargarReporteReservaFriaSistema',
            data: {
                idEmpresa: idEmpresa,
                fechaInicio: fechaInicio,
                fechaFin: fechaFin
            },
            success: function (data) {
                if (data.Error == undefined) {
                    $('#listado').html(data.Resultado);
                    $('#idGraficoContainer').html('');
                } else {
                    alert("Ha ocurrido un error");
                    console.log(data.Descripcion);
                }
            },
            error: function (err) {
                alert("Ha ocurrido un error");
                console.log(err.responseText)
            }
        });
    }
    else {
        $("#listado").html('');
        $('#idGraficoContainer').html('');
    }
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

    var equipo = $('#cbEquipo').multipleSelect('getSelects');
    if (equipo == "[object Object]") equipo = "-1";
    $('#hfEquipo').val(equipo);
    idEquipo = $('#hfEquipo').val();

    fechaInicio = $('#txtFechaInicio').val();
    fechaFin = $('#txtFechaFin').val();

    var arrayFiltro = arrayFiltro || [];

    if (valor == 1)
        arrayFiltro.push({ id: idEmpresa, mensaje: "Seleccione la opcion Empresa" });

    validarFiltros(arrayFiltro);
}