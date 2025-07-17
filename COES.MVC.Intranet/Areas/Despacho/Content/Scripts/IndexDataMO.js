var controlador = siteRoot + 'Despacho/'
$(function () {
    $('#btnRegresar').click(function () {
        history.back(1);
    });
    $('#cbEmpresa').val("0");
    $('#cbEstado').val("S");
    buscarEvento();
    $('#btnBuscar').click(function () {
        buscarEvento();
    });
    $(window).resize(function () {
        $('#listado').css("width", $('#mainLayout').width() + "px");
    });
});

function buscarEvento() {
    pintarPaginado();
    mostrarListado(1);
}

function pintarPaginado() {
    $.ajax({
        type: 'POST',
        url: controlador + "DatosModoOperacion/paginado",
        data: $('#frmBusqueda').serialize(),
        success: function (evt) {
            $('#paginado').html(evt);
            mostrarPaginado();
        },
        error: function () {
            alert("Ha ocurrido un error");
        }
    });
}
function pintarBusqueda(nroPagina) {
    mostrarListado(nroPagina);
}
function mostrarListado(nroPagina) {
    $('#hfNroPagina').val(nroPagina);
    $.ajax({
        type: 'POST',
        url: controlador + "DatosModoOperacion/Lista",
        data: $('#frmBusqueda').serialize(),
        success: function (evt) {
            $('#listado').css("width", $('#mainLayout').width() + "px");

            $('#listado').html(evt);
            $('#tabla').dataTable({
                "scrollY": 430,
                "scrollX": true,
                "sDom": 't',
                "ordering": false,
                "iDisplayLength": 50
            });
        },
        error: function () {
            alert("Ha ocurrido un error");
        }
    });
}
function mostrarDetalle(id,repCodi) {
    location.href = controlador + "DatosModoOperacion/DetalleModoOperacionDatos?id=" + id + "&repCodi=" + repCodi;
}