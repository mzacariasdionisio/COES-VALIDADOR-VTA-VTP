$.message = "";
var controler = siteRoot + "resarcimientos/reporteentrega/";
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
            empresa: $('#RCboEmpresasGeneradoras').val(),
            periodo: $('#CboPeriodo').val(),
            cliente: $('#CboCliente').val(),
            pentrega: $('#CboPuntoEntrega').val(),
            ntension: $('#CboTension').val()
        },
        success: function (evt) {
            console.log("PE - paginado" + evt)
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
        url: controler + "DefaultEntrega",
        data: {
            empresa: $('#RCboEmpresasGeneradoras').val(),
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
                "iDisplayLength": 50
            });
        },
        error: function (req, status, error) {
            mensajeOperacion("Ha ocurrido un error");
            validaErrorOperation(req.status);
        }
    });
}
//---------------

function AjaxOptionselect(cadEmpresa, cadPeriodo, cadCliente, cadPentrega, cadNtension) {
    $.ajax({
        type: "POST",
        url: controler + "Optionselect",
        //dataType: 'json',
        data: {
            empresa: cadEmpresa,
            periodo: cadPeriodo,
            cliente: cadCliente,
            pentrega: cadPentrega,
            ntension: cadNtension
        },
        cache: false,
        success: function (resultado) {
            $('#content_selection').html(resultado);
        },
        error: function (req, status, error) {
            mensajeOperacion(error);
        }
    });
}

