var controlador = siteRoot + 'Siosein/CostoOperacion/'
$(function () {

    $('#cbEmpresa').multipleSelect({
        width: '150px',
        filter: true,
        onClose: function (view) {
            cargarUbicacion();
        }
    });

    $('#btnBuscar').click(function () {
        cargarListaEventos();
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
    cargarUbicacion();
}