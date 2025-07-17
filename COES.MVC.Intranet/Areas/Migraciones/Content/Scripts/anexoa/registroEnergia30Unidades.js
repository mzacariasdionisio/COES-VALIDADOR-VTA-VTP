var controlador = siteRoot + 'Migraciones/AnexoA/'
var idEmpresa = "";
$(function () {
    $('#cbEmpresa').multipleSelect({
        width: '150px',
        filter: true,
        onClose: function (view) {
            cargarListaRegistroEnergia30Unidades();
        }
    });
    
    $('#cbEmpresa').multipleSelect('checkAll');

    cargarListaRegistroEnergia30Unidades();
});

function cargarListaRegistroEnergia30Unidades() {
    validacionesxFiltro(1);
    if (resultFiltro) {
        var fechaInicio = $('#txtFechaInicio').val();
        var fechaFin = $('#txtFechaFin').val();

        $.ajax({
            type: 'POST',
            url: controlador + 'CargarListaRegistroEnergia30Unidades',
            data: { fechaInicio: fechaInicio, fechaFin: fechaFin, idEmpresa: idEmpresa },
            success: function (aData) {
                $('#listado').css("width", $('#mainLayout').width() - 20 + "px");
                $('#listado').html(aData.Resultado);
                $("#tabla_wrapper").css("width", "100%");
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
    
    var arrayUbicacion = arrayUbicacion || [];

    if (valor == 1)
        arrayUbicacion.push({ id: idEmpresa, mensaje: "Seleccione la opcion Empresa" }); //, { id: idCentral, mensaje: "Seleccione la opcion Central" });
    else
        //if ((valor == 2)) {
        //    arrayUbicacion.push({ id: idEmpresa, mensaje: "Seleccione la opcion Empresa" }, { id: idCentral, mensaje: "Seleccione la opcion Central" });
        //}
        //else {
        arrayUbicacion.push({ id: idEmpresa, mensaje: "Seleccione la opcion Empresa" }, { id: idCentral, mensaje: "Seleccione la opcion Central" }, { id: idTCombustible, mensaje: "Seleccione la opcion Tipo Combustible" });
    //}

    validarFiltros(arrayUbicacion);
}