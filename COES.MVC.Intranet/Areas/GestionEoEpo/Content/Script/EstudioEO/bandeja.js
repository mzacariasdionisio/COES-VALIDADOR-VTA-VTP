var controlador = siteRoot + 'gestioneoepo/estudioeo/';
var controladorEpo = siteRoot + 'gestioneoepo/estudioepo/';
var CantidadFilas = 20;

$(document).ready(function () {
    $('#txtFchPresentacionIni').inputmask({
        mask: "1/2/y",
        placeholder: "dd/mm/yyyy",
        alias: "datetime"
    });

    $('#txtFchPresentacionIni').Zebra_DatePicker({
        readonly_element: false,
        onSelect: function (date) {
            $('#txtFchPresentacionIni').val(date);
        }
    });


    $('#txtFchPresentacionFin').inputmask({
        mask: "1/2/y",
        placeholder: "dd/mm/yyyy",
        alias: "datetime"
    });

    $('#txtFchPresentacionFin').Zebra_DatePicker({
        readonly_element: false,
        onSelect: function (date) {
            $('#txtFchPresentacionFin').val(date);
        }
    });


    $('#txtFchConformidadIni').inputmask({
        mask: "1/2/y",
        placeholder: "dd/mm/yyyy",
        alias: "datetime"
    });

    $('#txtFchConformidadIni').Zebra_DatePicker({
        readonly_element: false,
        onSelect: function (date) {
            $('#txtFchConformidadIni').val(date);
        }
    });


    $('#txtFchConformidadFin').inputmask({
        mask: "1/2/y",
        placeholder: "dd/mm/yyyy",
        alias: "datetime"
    });

    $('#txtFchConformidadFin').Zebra_DatePicker({
        readonly_element: false,
        onSelect: function (date) {
            $('#txtFchConformidadFin').val(date);
        }
    });


    $("#btnNuevo").click(function () {

    })

    $('#cboPuntCodi').multipleSelect({
        filter: true,
        selectAll: false
    })


    $("#btnBuscar").click(function () {
        buscar();
    })

    setTimeout(function () { buscar() }, 500);

    //buscar();
});

function ValidarFechas() {
    var _rpt = true;

    var dPreseIni = document.getElementById("txtFchPresentacionIni").value;
    var dPreseFin = document.getElementById("txtFchPresentacionFin").value;

    var dConformIni = document.getElementById("txtFchConformidadIni").value;
    var dConformFin = document.getElementById("txtFchConformidadFin").value;

    if ((dPreseIni == "" && dPreseFin == "") && (dConformIni == "" && dConformFin == ""))
        return true;

    if (process(dPreseIni) > process(dPreseFin)) {
        mostrarMensajePopup("La fecha inicial es mayor a la fecha final de la presentanción.");
        _rpt = false;
    }
    else if (process(dConformIni) > process(dConformFin)) {
        mostrarMensajePopup("La fecha inicial es mayor a la fecha final de la conformidad.");
        _rpt = false;
    }
    else if ((dPreseIni == "" && dPreseFin != "") || (dPreseIni != "" && dPreseFin == "")) {
        mostrarMensajePopup("Ingrese ambas fechas de rango de presentanción.");
        _rpt = false;
    }
    else if ((dConformIni == "" && dConformFin != "") || (dConformIni != "" && dConformFin == "")) {
        mostrarMensajePopup("Ingrese ambas fechas de rango de conformidad.");
        _rpt = false;
    }
    return _rpt

}

function process(date) {
    var parts = date.split("/");
    return new Date(parts[2], parts[1] - 1, parts[0]);
}


function buscar() {

    if (!ValidarFechas())
        return;
    pintarPaginado();
    pintarBusqueda(1);
    pintarPaginadoEpo();
    pintarBusquedaEpo(1);
}

function pintarBusqueda(nroPagina) {
    $('#hfNroPagina').val(nroPagina);

    var dataFiltros = filtros(nroPagina);

    if (dataFiltros.Esteocodiproy == "") {
        document.getElementById("paginadoEpo").style.display = "none";
        document.getElementById("listadoEpo").style.display = "none";
    }
    else {
        document.getElementById("paginadoEpo").style.display = "block";
        document.getElementById("listadoEpo").style.display = "block";
    }

    $.ajax({
        type: 'POST',
        url: controlador + "Listado",
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
        error: function () {
            //mostrarError();
        }
    });
}

function pintarBusquedaEpo(nroPagina) {
    $('#hfNroPaginaEpo').val(nroPagina);

    var dataFiltrosEpo = filtrosEpo(nroPagina);

    $.ajax({
        type: 'POST',
        url: controlador + "ListadoEpo",
        data: dataFiltrosEpo,
        success: function (evt) {
            $('#listadoEpo').css("width", $('#mainLayout').width() + "px");

            $('#listadoEpo').html(evt);
            $('#tablaEpo').dataTable({
                "scrollY": 430,
                "scrollX": false,
                'searching': false,
                "ordering": false,
                "paging": false,
                "pagingType": "full_numbers",
                "iDisplayLength": 50
            });
        },
        error: function () {
            //mostrarError();
        }
    });
}

function CargarRegistros() {
    pintarPaginado();
    pintarBusqueda(1);
}

function CargarRegistrosEpo() {
    pintarPaginadoEpo();
    pintarBusquedaEpo(1);
}

function pintarPaginado() {
    var dataFiltros = filtros(1);

    $.ajax({
        type: 'POST',
        url: controlador + "paginado",
        data: dataFiltros,
        success: function (evt) {
            $('#paginado').html(evt);
            mostrarPaginado();

            $("#ddlCantidadRegistros").val(CantidadFilas);
        },
        error: function () {
            //mostrarError('mensaje');
        }
    });
}

function pintarPaginadoEpo() {
    var dataFiltrosEpo = filtrosEpo(1);

    $.ajax({
        type: 'POST',
        url: controlador + "paginadoEpo",
        data: dataFiltrosEpo,
        success: function (evt) {
            $('#paginadoEpo').html(evt);
            mostrarPaginadoEpo();

            $("#ddlCantidadRegistrosEpo").val(CantidadFilas);
        },
        error: function () {
            //mostrarError('mensaje');
        }
    });
}

function nuevoEstudioEo(id) {
    window.location = controlador + "RegistrarEstudioEo/" + id;
}

function revisionEstudioEo(id) {
    window.location = controlador + "revision/" + id;
}

function noVigenciaEstudioEo(id) {
    window.location = controlador + "EstablecerNoVigencia?Esteocodi=" + id;
}

function anular(esteocodi) {
    mostrarConfirmacion("¿Confirma que desea anular el estudio seleccionado?", AnularEstudioEO, esteocodi);
}

function AnularEstudioEO(parametros) {
    $.ajax({
        url: controlador + 'Anular',
        type: 'POST',
        data: { Esteocodi: parametros[0] },
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

function filtros(nroPagina, nroFilas) {
    var filas = 20;

    if (typeof $("#ddlCantidadRegistros").val() != "undefined") {
        filas = $("#ddlCantidadRegistros").val();
    }

    CantidadFilas = filas;

    var filtro = {

        FIniPresentacion: $('#txtFchPresentacionIni').val(),
        FFinPresentacion: $('#txtFchPresentacionFin').val(),
        FIniConformidad: $('#txtFchConformidadIni').val(),
        FFinConformidad: $('#txtFchConformidadFin').val(),

        Estacodi: $('#cboEstado').val(),
        Emprcoditp: $('#cboTitularProyecto').val(),
        Esteonomb: $('#txtNombreEstudio').val(),
        PuntCodi: $('#cboPuntCodi').val(),
        Esteocodiproy: $('#txtCodigioProyecto').val(),
        Esteoanospuestaservicio: $('#txtAnioPuestoServicio').val(),
        nroPagina: nroPagina,
        nroFilas: filas,
        EsteoTipoProyecto: $('#cboTipoProyecto').val(),
        Zoncodi: $('#cboZonaProyeto').val(),
    }
    console.log(filtro);
    return filtro;
}

function filtrosEpo(nroPagina, nroFilas) {
    var filas = 20;

    if (typeof $("#ddlCantidadRegistrosEpo").val() != "undefined") {
        filas = $("#ddlCantidadRegistrosEpo").val();
    }

    CantidadFilas = filas;

    var filtro = {

        FIniPresentacion: $('#txtFchPresentacionIni').val(),
        FFinPresentacion: $('#txtFchPresentacionFin').val(),
        FIniConformidad: $('#txtFchConformidadIni').val(),
        FFinConformidad: $('#txtFchConformidadFin').val(),

        Estacodi: $('#cboEstado').val(),
        Emprcoditp: $('#cboTitularProyecto').val(),
        Esteponomb: $('#txtNombreEstudio').val(),
        PuntCodi: $('#cboPuntCodi').val(),
        Estepoanospuestaservicio: $('#txtAnioPuestoServicio').val(),
        Estepocodiproy: $('#txtCodigioProyecto').val(),
        nroPagina: nroPagina,
        nroFilas: CantidadFilas,
        EstepoTipoProyecto: $('#cboTipoProyecto').val(),
        Zoncodi: $('#cboZonaProyeto').val(),
    }
    console.log(filtro);
    return filtro;
}

function nuevoEstudioEpo(id) {
    window.location = controladorEpo + "RegistrarEstudioEpo/" + id;
}
function revisionEstudioEpo(id) {
    window.location = controladorEpo + "revision/" + id;
}
function noVigenciaEstudioEpo(id) {
    window.location = controladorEpo + "EstablecerNoVigencia?Estepocodi=" + id;
}

/*Inicio Mejoras EO-EPO-II*/
function ConfirmarNoVigenciaEo(id) {
    mostrarConfirmacion("¿Desea confirmar No vigente?", noVigenciaEstudioEo, id);
}
/*Fin Mejoras EO-EPO-II*/