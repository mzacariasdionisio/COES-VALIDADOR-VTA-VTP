$.message = "";
$(document).ready(function () {
    $('#btnBuscar').click(function () {
        $('.content-hijo > .search-content').toggle();
    });
    $('#btnBuscarClick').click(function () {
        buscarReporte();
    });
    $('#CboEmpresasGeneradoras').change(function () {
        $('#CboPeriodo').change();
    });

    $('#CboCliente').change(function () {
        $('#CboPuntoEntrega').prop('selectedIndex', -1);
        $('#CboTension').prop('selectedIndex', -1);
    });
    $('#CboPuntoEntrega').change(function () {
        $('#CboTension').prop('selectedIndex', -1);
    });
    
    $('#btnBuscar').trigger('click');
    $('#btnCancelar').click(function () {
    });
});

