var controlador = siteRoot + 'gestioneoepo/estudioepo/';
var controladorEo = siteRoot + 'gestioneoepo/estudioeo/';
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



    $('#cboPuntCodi').multipleSelect({
        filter: true,
        selectAll: false
    })


    $("#btnNuevo").click(function () {

    })

    $("#btnBuscar").click(function () {
        buscar();
    })

    setTimeout(function () { buscar() }, 500);
    //buscar();

    /*Inicio Mejoras EO - EPO*/
    $("#btnCorreo").click(function () {
        Correo();
    })
    /*Fin Mejoras EO - EPO*/
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
    pintarPaginadoEo();
    pintarBusquedaEo(1);
}

function pintarBusqueda(nroPagina) {
    $('#hfNroPagina').val(nroPagina);

    var dataFiltros = filtros(nroPagina);

    if (dataFiltros.Estepocodiproy == "") {
        document.getElementById("paginadoEo").style.display = "none";
        document.getElementById("listadoEo").style.display = "none";
    }
    else {
        document.getElementById("paginadoEo").style.display = "block";
        document.getElementById("listadoEo").style.display = "block";
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

function pintarBusquedaEo(nroPagina) {
    $('#hfNroPaginaEo').val(nroPagina);

    var dataFiltros = filtrosEo(nroPagina);

    $.ajax({
        type: 'POST',
        url: controlador + "ListadoEo",
        data: dataFiltros,
        success: function (evt) {
            $('#listadoEo').css("width", $('#mainLayout').width() + "px");

            $('#listadoEo').html(evt);
            $('#tablaEo').dataTable({
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

function pintarPaginadoEo() {
    var dataFiltros = filtrosEo(1);

    $.ajax({
        type: 'POST',
        url: controlador + "paginadoEo",
        data: dataFiltros,
        success: function (evt) {
            $('#paginadoEo').html(evt);
            mostrarPaginadoEo();

            $("#ddlCantidadRegistrosEo").val(CantidadFilas);
        },
        error: function () {
            //mostrarError('mensaje');
        }
    });
}

function CargarRegistros() {
    pintarPaginado();
    pintarBusqueda(1);
}

function CargarRegistrosEo() {
    pintarPaginadoEo();
    pintarBusquedaEo(1);
}

function nuevoEstudioEpo(id) {
    window.location = controlador + "RegistrarEstudioEpo/" + id;
}

function revisionEstudioEpo(id) {
    window.location = controlador + "revision/" + id;
}

function noVigenciaEstudioEpo(id) {
    window.location = controlador + "EstablecerNoVigencia?Estepocodi=" + id;
}

function anular(estepocodi) {
    mostrarConfirmacion("¿Confirma que desea anular el estudio seleccionado?", AnularEstudioEO, estepocodi);
}

function AnularEstudioEO(parametros) {
    $.ajax({
        url: controlador + 'Anular',
        type: 'POST',
        data: { Estepocodi: parametros[0] },
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
        //Estepofechaini: $('#txtFchPresentacion').val(),
        //Estepofechafin: $('#txtFchConformidad').val(),

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
        nroFilas: filas,
        EstepoTipoProyecto: $('#cboTipoProyecto').val(),
        Zoncodi: $('#cboZonaProyeto').val(),
    }

    return filtro;
}

function filtrosEo(nroPagina, nroFilas) {
    var filas = 20;

    if (typeof $("#ddlCantidadRegistrosEo").val() != "undefined") {
        filas = $("#ddlCantidadRegistrosEo").val();
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

/*Inicio Mejoras EO - EPO*/
function Correo() {
    $.ajax({
        url: controlador + 'EnviarCorreo',
        type: 'POST',
        //data: { Estepocodi: parametros[0] },
        success: function (d) {
            if (d.bResult) {
                //$('#popupZ').bPopup().close();
                //$("#popupConfirmarOperacion .b-close").click();

                mostrarMensajePopup("El registro se actualizó correctamente!.", 1);
            }
            else { alert(d.sMensaje); }
        }
    });
}
function nuevoEstudioEo(id) {
    window.location = controladorEo + "RegistrarEstudioEo/" + id;
}
function revisionEstudioEo(id) {
    window.location = controladorEo + "revision/" + id;
}

function noVigenciaEstudioEo(id) {
    window.location = controladorEo + "EstablecerNoVigencia?Esteocodi=" + id;
}
/*Fin Mejoras EO - EPO*/

/*Inicio Mejoras EO-EPO-II*/
function ConfirmarNoVigenciaEpo(id) {
    mostrarConfirmacion("¿Desea confirmar No vigente?", noVigenciaEstudioEpo, id);
}
/*Fin Mejoras EO-EPO-II*/