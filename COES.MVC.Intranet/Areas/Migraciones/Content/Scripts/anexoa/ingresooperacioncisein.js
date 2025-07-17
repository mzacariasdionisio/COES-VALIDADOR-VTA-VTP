//variables globales
var controlador = siteRoot + 'Migraciones/AnexoA/'
var idTipoEquipo = "";
var idUbicacion = "";
var idEquipo = "";
var fechaInicio = "";
var fechaFin = "";

$(function () {


    $('#cbEmpresa').multipleSelect({
        width: '200px',
        filter: true,
        onClose: function () {
            cargarListaIngresoOperacion();
        }
    });
    $('#cbTipoEquipo').multipleSelect({
        width: '200px',
        filter: true,
        onClose: function () {
            cargarListaIngresoOperacion();
        }
    });


    $('#txtFechaInicio').Zebra_DatePicker({
        //direction: -1
    });

    $('#txtFechaFin').Zebra_DatePicker({
        //direction: -1
    });
    cargarValoresIniciales();
    cargarListaIngresoOperacion();

});

function cargarValoresIniciales() {
    $('#cbEmpresa').multipleSelect('checkAll');
    

    $('#cbTipoEquipo').multipleSelect('checkAll');
    
    //$('#cbEmpresa').multipleSelect({
    //    width: '150px',
    //    filter: true,
    //    onClose: function (view) {
    //        cargarUbicacion();
    //    }
    //});

    // cargarUbicacion();
}

function cargarListaIngresoOperacion() {

    validacionesxFiltro(2);

    if (resultFiltro) {

        $.ajax({
            type: 'POST',
            url: controlador + 'CargarListaIngresoOperacion',
            data: { empresas: idsEmpresas, sTipoEquipo: idTipoEquipo, fechaInicio: fechaInicio, fechaFin: fechaFin },
            success: function (aData) {
                $('#listado1').html(aData.Resultado);
                $('#idGraficoContainer').html('');
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

    var empresas = $('#cbEmpresa').multipleSelect('getSelects');
    if (empresas == "[object Object]") empresas = "-1";
    $('#hfEmpresa').val(empresas);
    idsEmpresas = $('#hfEmpresa').val();


    var tipoequipo = $('#cbTipoEquipo').multipleSelect('getSelects');
    if (tipoequipo == "[object Object]") tipoequipo = "-1";
    $('#hfTipoEquipo').val(tipoequipo);
    idTipoEquipo = $('#hfTipoEquipo').val();


    fechaInicio = $('#txtFechaInicio').val();
    fechaFin = $('#txtFechaFin').val();

    var arrayFiltro = arrayFiltro || [];

    if (valor == 1)
        arrayFiltro.push({ id: idTipoEquipo, mensaje: "Seleccione la opcion Tipo Equipo" }, { id: idsEmpresas, mensaje: "Seleccione empresa" });

    else
        arrayFiltro.push({ id: idTipoEquipo, mensaje: "Seleccione la opcion Tipo Equipo" }, { id: idsEmpresas, mensaje: "Seleccione empresa" });

    validarFiltros(arrayFiltro);



}