var controlador = siteRoot + 'gestioneoepo/estudioepo/';

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


    $("#btnBuscar").click(function () {
        buscar();
    })

    $("#btnExportar").click(function () {
        exportar();
    })

    /*Inicio Mejoras EO - EPO*/
    $("#btnExportarEO").click(function () {
        exportarEO();
    })
    /*Fin Mejoras EO - EPO*/
    
    buscar();
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
        document.getElementById("btnExportarEO").style.display = "none";   
    }
    else {
        document.getElementById("paginadoEo").style.display = "block";
        document.getElementById("listadoEo").style.display = "block";
        document.getElementById("btnExportarEO").style.display = "block";
    }

    $.ajax({
        type: 'POST',
        url: controlador + "ListadoWeb",
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
        url: controlador + "ListadoWebEo",
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

function exportar() {
    var dataFiltros = filtrosExportar(1);

    $.ajax({
        type: 'POST',
        url: controlador + "ExportarListadoWeb",
        data: dataFiltros,
        success: function (evt) {
            descargar(evt,1);
        },
        error: function () {
            //mostrarError('mensaje');
        }
    });
}

function exportarEO() {
    var dataFiltros = filtrosExportarEO(1);

    $.ajax({
        type: 'POST',
        url: controlador + "ExportarListadoWebEO",
        data: dataFiltros,
        success: function (evt) {
            descargar(evt,2);
        },
        error: function () {
            //mostrarError('mensaje');
        }
    });
}

function descargar(nombrearchivo, tipo) {
    window.location = controlador + "Descargar?nombreArchivo=" + nombrearchivo + "|" + tipo;
}

function verdetalle(id) {
    window.location = controlador + "VerDetalle?id=" + id;
}

function verdetalleEo(id) {
    window.location = controlador + "VerDetalleEo?id=" + id;
}

function filtros(nroPagina) {
    var filas = 20;

    if (typeof $("#ddlCantidadRegistros").val() != "undefined") {
        filas = $("#ddlCantidadRegistros").val();
    }

    CantidadFilas = filas;

    var filtro = {
        //Estepofechaini: $('#txtFchInicial').val(),
        //Estepofechafin: $('#txtFchFin').val(),

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

    return filtro;
}

function filtrosEo(nroPagina) {
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
        Esteoanospuestaservicio: $('#txtAnioPuestoServicio').val(),
        Esteocodiproy: $('#txtCodigioProyecto').val(),
        Estadescripcion: $('#cboTipoProyecto').val(),
        nroPagina: nroPagina,
        nroFilas: CantidadFilas,
        EsteoTipoProyecto: $('#cboTipoProyecto').val(),
        Zoncodi: $('#cboZonaProyeto').val()
    }

    return filtro;
}

function filtrosExportar(nroPagina) {
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
        EstepoTipoProyecto: $('#cboTipoProyecto').val(),
        Zoncodi: $('#cboZonaProyeto').val(),
        nroPagina: nroPagina
    }

    return filtro;
}

function filtrosExportarEO(nroPagina) {
    var filtro = {
        FIniPresentacion: $('#txtFchPresentacionIni').val(),
        FFinPresentacion: $('#txtFchPresentacionFin').val(),
        FIniConformidad: $('#txtFchConformidadIni').val(),
        FFinConformidad: $('#txtFchConformidadFin').val(),
        Estacodi: $('#cboEstado').val(),
        Emprcoditp: $('#cboTitularProyecto').val(),
        Esteonomb: $('#txtNombreEstudio').val(),
        PuntCodi: $('#cboPuntCodi').val(),
        Esteoanospuestaservicio: $('#txtAnioPuestoServicio').val(),
        Esteocodiproy: $('#txtCodigioProyecto').val(),
        Estadescripcion: $('#cboTipoProyecto').val(),
        EsteoTipoProyecto: $('#cboTipoProyecto').val(),
        Zoncodi: $('#cboZonaProyeto').val(),
        nroPagina: nroPagina
    }

    return filtro;
}