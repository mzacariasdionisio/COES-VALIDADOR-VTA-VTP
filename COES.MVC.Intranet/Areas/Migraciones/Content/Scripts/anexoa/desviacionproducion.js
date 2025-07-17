var controlador = siteRoot + 'Migraciones/AnexoA/'
$(function () {

    $('#cbEmpresa').multipleSelect({
        width: '200px',
        filter: true,
        onClose: function (view) {
            /*cargarUbicacion();*/
            cargarLista();
            // cargarLista();
        }
    });

    cargarValoresIniciales();
});

function cargarValoresIniciales() {
    // $('#cbEmpresa').multipleSelect('checkAll');
    $('#cbEmpresa').multipleSelect('checkAll');
    cargarLista();
}

function cargarLista() {
    var empresaarray = $('#cbEmpresa').multipleSelect('getSelects');
    if (empresaarray == "[object Object]") empresaarray = "-1";
    $('#hfEmpresa').val(empresaarray);
    var idEmpresa = $('#hfEmpresa').val();
    var fechaInicio = $('#txtFechaInicio').val();
    var fechaFin = $('#txtFechaFin').val();
    if (empresaarray != "") {
       $.ajax({
            type: 'POST',
            url: controlador + 'CargarListaDesviacionesProduccionUG',
            data: {
                fechaInicio: fechaInicio,
                fechaFin: fechaFin,
                empresa:idEmpresa
            },
            success: function (aData) {
                $('#listado').css("width", $('#mainLayout').width() - 20 + "px");
                $('#listado').html(aData.Resultado);
                $("#tabla_wrapper").css("width", "100%");
            },
            error: function () {
                alert("Ha ocurrido un error");
            }
        });
    } else {
        alert("Debe seleccionar al menos una empresa");
        $('#cbEmpresa').multipleSelect('checkAll');
    }
}
