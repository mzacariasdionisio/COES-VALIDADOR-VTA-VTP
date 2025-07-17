$.message = "";
var controler = siteRoot + "resarcimientos/rechazocarga/";
//----paginado---

function buscarReporte() {
    pintarPaginado();
    mostrarListado(1);
}

function pintarPaginado() {
    $.ajax({
        type: 'POST',
        url: controler + "paginado",
        data: {
            empresa: $('#CboEmpresasGeneradoras').val(),
            periodo: $('#CboPeriodo').val(),
            cliente: $('#CboCliente').val(),
            pentrega: $('#CboPuntoEntrega').val(),
            ntension: $('#CboTension').val()
        },
        success: function (evt) {
            $('#paginado').html(evt);
            mostrarPaginado();
        },
        error: function (req, status, error) {
            mensajeOperacion("Ha ocurrido un error");
            validaErrorOperation(req.status);
        }
    });
}

function pintarBusqueda(nroPagina) {
    mostrarListado(nroPagina);
}

function mostrarListado(nroPagina) {
    $.ajax({
        type: 'POST',
        url: controler + "DefaultCarga",
        data: {
            empresa: $('#CboEmpresasGeneradoras').val(),
            periodo: $('#CboPeriodo').val(),
            cliente: $('#CboCliente').val(),
            pentrega: $('#CboPuntoEntrega').val(),
            ntension: $('#CboTension').val(),
            nroPagina: nroPagina
        },
        success: function (evt) {
            $('#listado').css("width", $('#mainLayout').width() + "px");

            $('#listado').html(evt);
            $('#tblTramites').dataTable({
                "scrollY": 430,
                "scrollX": true,
                "sDom": 't',
                "ordering": true,
                "iDisplayLength": 50,
                "bDestroy": true
            });
        },
        error: function (req, status, error) {
            mensajeOperacion("Ha ocurrido un error");
            validaErrorOperation(req.status);
        }
    });
}
