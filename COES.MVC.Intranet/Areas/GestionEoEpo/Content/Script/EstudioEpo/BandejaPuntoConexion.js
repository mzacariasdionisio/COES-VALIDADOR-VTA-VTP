var controlador = siteRoot + 'gestioneoepo/PuntoConexion/';
var CantidadFilas = 20;


$(document).ready(function () {
    
    $("#btnNuevo").click(function () {

    })

    $("#btnBuscar").click(function () {
        buscar();
    })


    buscar();
});



function buscar() {
    pintarPaginado();
    pintarBusqueda(1);
}

function pintarBusqueda(nroPagina) {
    $('#hfNroPagina').val(nroPagina);

    var dataFiltros = filtros(nroPagina);

    $.ajax({
        type: 'POST',
        url: controlador + "ListadoPuntoConexionEpo",
        data: dataFiltros,
        success: function (evt) {
            $('#listado').css("width", $('#mainLayout').width() + "px");

            $('#listado').html(evt);
            $('#tabla').dataTable({
                "scrollY": 430,
                "scrollX": false,
                'searching': false,
                "ordering": false,
                "paging": false,
                "pagingType": "full_numbers",
                "iDisplayLength": 50
            });
        },
        error: function (ex) {
            //mostrarError();
            console.log(ex)
        }
    });
}

function pintarPaginado() {
    var dataFiltros = filtros(1);

    $.ajax({
        type: 'POST',
        url: controlador + "Paginado",
        data: dataFiltros,
        success: function (evt) {
            $('#paginado').html(evt);
            mostrarPaginado();

            $("#ddlCantidadRegistros").val(CantidadFilas);
        },
        error: function (ex) {
            //mostrarError();
            console.log(ex)
        }
    });
}

function CargarRegistros() {
    pintarPaginado();
    pintarBusqueda(1);
}

function nuevoPuntoConexion(id) {
    window.location = controlador + "RegistrarPuntoConexionEpo/" + id;
}
function anularPuntoConexion(id) {
    mostrarConfirmacion("¿Confirma que desea anular el punto de conexion seleccionado?", _anularPuntoConexion, id);
}
function revisionPuntoConexion(id) {
    window.location = controlador + "revision/" + id;
}

function noVigenciaEstudioEpo(id) {
    window.location = controlador + "EstablecerNoVigencia?Estepocodi=" + id;
}



function _anularPuntoConexion(parametros) {
    $.ajax({
        url: controlador + 'Anular',
        type: 'POST',
        data: { PUNTCODI: parametros[0] },
        success: function (d) {
            if (d.bResult) {
                $('#popupZ').bPopup().close();
                $("#popupConfirmarOperacion .b-close").click();

                mostrarMensajePopup("El registro se actualizó correctamente!.", 1);

                buscar();
            }
            else { alert(d.sMensaje); }
        }
    });
}

function filtros(nroPagina) {
    var filas = 20;

    if (typeof $("#ddlCantidadRegistros").val() != "undefined") {
        filas = $("#ddlCantidadRegistros").val();
    }

    CantidadFilas = filas;

    var filtro = {
        ZonCodi: $('#cboZonaProyeto').val(),
        PuntDescripcion: $('#txtPuntoConexion').val(),
        nroPagina: nroPagina,
        nroFilas: filas,
    }
    return filtro;
}