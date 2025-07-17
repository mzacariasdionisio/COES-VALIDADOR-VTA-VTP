//variables globales
var controlador = siteRoot + 'Migraciones/AnexoA/'
var idEmpresa = "";
var idCentral = "";
var idTCombustible = "";
var fechaInicio = "";
var fechaFin = "";

$(function () {

    $('#cbEmpresa').multipleSelect({
        width: '150px',
        filter: true,
        onClose: function (view) {
            cargarCentralxEmpresa();
        }
    });

    $('#cbCentrales').multipleSelect({
        width: '150px',
        filter: true,
        onClose: function (view) {
        }
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
    $('#cbCentrales').multipleSelect('checkAll');

    cargarCentralxEmpresa();
}

function cargarCentralxEmpresa() {
    validacionesxFiltro(1);

    if (resultFiltro) {
        $.ajax({
            type: 'POST',
            async: false,
            url: controlador + 'CargarCentralxEmpresaCoGeneracion',
            data: { idEmpresa: idEmpresa },
            success: function (aData) {
                $('#centrales').html(aData);
                $('#cbCentral').multipleSelect({
                    width: '150px',
                    filter: true,
                    onClose: function (view) {
                        cargarLista();
                    }
                });
                $('#cbCentral').multipleSelect('checkAll');

                cargarLista();
            },
            error: function () {
                alert("Ha ocurrido un error");
            }
        });
    }
}

function cargarLista() {

    validacionesxFiltro(2);

    if (resultFiltro) {
        $.ajax({
            type: 'POST',
            async: false,
            url: controlador + 'CargarListaCalorUtilGeneracionProceso',
            data: { idEmpresa: idEmpresa, fechaInicio: fechaInicio, fechaFin: fechaFin, idsCentrales: idCentral },
            success: function (aData) {
                $('#listado').html(aData.Resultado);
            },
            error: function () {
                alert("Ha ocurrido un error");
            }
        });
    }
}

//validaciones de los filtros de busqueda
function validacionesxFiltro(valor) {

    var empresa = $('#cbEmpresa').multipleSelect('getSelects');
    if (empresa == "[object Object]") empresa = "-1";
    $('#hfEmpresa').val(empresa);
    idEmpresa = $('#hfEmpresa').val();

    var centrales = $('#cbCentral').multipleSelect('getSelects');
    if (centrales == "[object Object]") centrales = "-1";
    $('#hfCentrales').val(centrales);
    idCentral = $('#hfCentrales').val();

    fechaInicio = $('#txtFechaInicio').val();
    fechaFin = $('#txtFechaFin').val();

    var arrayUbicacion = arrayUbicacion || [];

    if (valor == 1)
        arrayUbicacion.push({ id: idEmpresa, mensaje: "Seleccione la opcion Empresa" }, { id: idCentral, mensaje: "Seleccione la opcion Central" });
    else
        arrayUbicacion.push({ id: idEmpresa, mensaje: "Seleccione la opcion Empresa" }, { id: idCentral, mensaje: "Seleccione la opcion Central" });

    validarFiltros(arrayUbicacion);
}