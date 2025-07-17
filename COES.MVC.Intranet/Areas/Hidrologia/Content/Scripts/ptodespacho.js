var controlador = siteRoot + 'hidrologia/'
var table = null;

$(function () {
    $('#cbEmpresa').multipleSelect({
        width: '200px',
        filter: true
    });
    $('#cbEstado').val("S");


    $('#btnBuscar').click(function () {
        buscarDespacho();
    });

    $('#btnNuevo').click(function () {
        nuevoPtoDespacho(0);
    });
    cargarPrevio();
    buscarDespacho();
        
    $(window).resize(function () {
        $('#listado').css("width", $('#mainLayout').width() + "px");
    });
});

function cargarPrevio() {
    $('#cbEmpresa').multipleSelect('checkAll');
}

function buscarDespacho() {
    if ($('#cbEmpresa').multipleSelect('rowCountSelected') <= 100 || $('#cbEmpresa').multipleSelect('isAllSelected') == "S") {

        pintarPaginado();
        mostrarListado(1);
    }
    else {
        alert("No puede seleccionar más de 100 empresas.");
    }
}
function pintarBusqueda(nroPagina) {
    mostrarListado(nroPagina);
}

function pintarBusqueda(nroPagina) {
    mostrarListado(nroPagina);
}

function pintarPaginado() {
    var empresas = $('#cbEmpresa').multipleSelect('getSelects');
    $('#hfEmpresa').val($('#cbEmpresa').multipleSelect('isAllSelected') == "S" ? "-1" : empresas);
    $.ajax({
        type: 'POST',
        url: controlador + "ptodespacho/paginado",
        data: {
            empresas: $('#hfEmpresa').val(),
            nroPagina: 1
        },
        success: function (evt) {
            $('#paginado').html(evt);
            mostrarPaginado();
        },
        error: function () {
            alert("Ha ocurrido un error en el paginado");
        }
    });
}


function nuevoPtoDespacho(id) {
    $.ajax({
        type: 'POST',
        url: controlador + "ptodespacho/editar?id=" + id,
        success: function (evt) {
            $('#agregarPto').html(evt);
            setTimeout(function () {
                $('#popupUnidad').bPopup({
                    easing: 'easeOutBack',
                    speed: 450,
                    transition: 'slideDown'
                });
            }, 50);
        },
        error: function () {
            mostrarError();
        }
    });
}
function mostrarListado(nroPaginas) {
    var empresa = $('#cbEmpresa').multipleSelect('getSelects');
    $('#hfEmpresa').val($('#cbEmpresa').multipleSelect('isAllSelected') == "S" ? "-1" : empresa);

    $.ajax({
        type: 'POST',
        url: controlador + "ptodespacho/lista",
        data: {
            empresas: $('#hfEmpresa').val(),
            nroPagina: nroPaginas
        },
        success: function (evt) {
            
            $('#listado').css("width", $('#mainLayout').width() + "px");

            $('#listado').html(evt);
            table = $('#tabla').dataTable({
                "scrollY": 500,
                "scrollX": true,
                "sDom": 't',
                "ordering": false,
                "iDisplayLength": 40
            });
        },
        error: function () {
            alert("Ha ocurrido un error en la lista");
        }
    });
mostrarError = function () {
    $('#mensaje').removeClass("action-exito");
    $('#mensaje').addClass("action-error");
    $('#mensaje').text("Ha ocurrido un error");
    $('#mensaje').css("display", "block");
};
}

function mostrarPtoDespacho(id) {
    nuevoPtoDespacho(id);
}

function eliminarPtoDespacho(id) {
    if (confirm('¿Está seguro que desea eliminar registro?')) {

        $.ajax({
            type: 'POST',
            url: controlador + 'ptodespacho/deletePuntoDespacho',
            data: {
                ptoDespacho: id
            },
            dataType: 'json',
            success: function (result) {
                if (result) {
                    buscarDespacho();
                    mostrarMensajeDespacho('mensajeDespacho', 'exito', 'La operación se realizó correctamente.');
                }
                else {
                    mostrarMensajeDespacho('mensajeDespacho', 'alert', 'No se puede eliminar el grupo existente.');
                }
            },
            error: function () {
                mostrarMensajeDespacho('mensajeDespacho', 'error', 'Ha ocurrido un error.');
            }
        });
    }
}

mostrarMensajeDespacho = function (id, tipo, mensaje) {
    $('#' + id).removeClass();
    $('#' + id).addClass('action-' + tipo);
    $('#' + id).html(mensaje);
}