var controlador = siteRoot + 'Migraciones/AnexoA/'
$(function () {

    $('#cbEmpresa').change(function () {
        cargarSubEstacion();
    });


    
    cargarValoresIniciales();
    
    //setTimeout(function () { pintarBusqueda(1); }, 3000);
});

buscar = function () {   
    pintarBusqueda(1);

}

function cargarValoresIniciales() {
    cargarSubEstacion();
}

pintarPaginado = function () {
    var subestacion = $('#cbSubEstacion').multipleSelect('getSelects');
    if (subestacion == "[object Object]") subestacion = "-1";
    $('#hfSubEstacion').val(subestacion);
    var idSubEstacion = $('#hfSubEstacion').val();

    $.ajax({
        type: 'POST',
        url: controlador + "paginadoFlujoPot",
        data: {
            fechaInicio: $('#txtFechaInicio').val(),
            fechaFin: $('#txtFechaFin').val(),
            idEmpresa: $('#cbEmpresa').val(),
            idSubEstacion: idSubEstacion
        },
        success: function (evt) {
            $('#paginado').html(evt);
            //mostrarPaginado();
        },
        error: function () {
            alert('Error Paginado!');
        }
    });
}

pintarBusqueda = function (nroPagina) {
    $('#hfNroPagina').val(nroPagina);
    cargarLista(nroPagina);
}

function cargarSubEstacion() {
    var empresa = $('#cbEmpresa').val();
    if (empresa == "") empresa = "-1";
    $('#hfEmpresa').val(empresa);
    var idEmpresa = $('#hfEmpresa').val();
    $.ajax({
        type: 'POST',
        url: controlador + 'CargarSubEstacionF',
        data: { idEmpresa: idEmpresa },
        success: function (aData) {
            $('#subestacion').html(aData.Resultado);
            $('#cbSubEstacion').multipleSelect({
                width: '250px',
                filter: true
                , onClose: function () {
                    buscar();
                }
            });

            $('#cbSubEstacion').multipleSelect('checkAll');
            buscar();
        },
        error: function () {
            alert("Ha ocurrido un error carga Estacion");
        }
    });
}

function cargarLista(nroPagina) {
    var empresa = $('#cbEmpresa').val();
    if (empresa == "") empresa = "-1";
    $('#hfEmpresa').val(empresa);
    var idEmpresa = $('#hfEmpresa').val();
    var subestacion = $('#cbSubEstacion').multipleSelect('getSelects');
    if (subestacion == "[object Object]") subestacion = "-1";
    $('#hfSubEstacion').val(subestacion);
    var idSubEstacion = $('#hfSubEstacion').val();

    var fechaInicio = $('#txtFechaInicio').val();
    var fechaFin = $('#txtFechaFin').val();

    $.ajax({
        type: 'POST',
        url: controlador + 'CargarListaRegistroFlujosEI',
        data: { fechaInicio: fechaInicio, fechaFin: fechaFin, idEmpresa: idEmpresa, idSubEstacion: idSubEstacion },
        success: function (data) {
            $('#listado').css("width", $('#mainLayout').width() + "px");
            $('#listado').html(data.Resultado);
            $('#listado').css("overflow-x", "auto");

        },
        error: function () {
            alert("Ha ocurrido un error");
        }
    });



}