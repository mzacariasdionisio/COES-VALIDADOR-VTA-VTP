var controlador = siteRoot + 'planificacion/NuevosProyectos/';

var CantidadFilas = 20;
$(document).ready(function () {

    //Inicio Mejoras EO - EPO
    //$('#txtFchInicial').inputmask({
    //    mask: "1/2/y",
    //    placeholder: "dd/mm/yyyy",
    //    alias: "datetime"
    //});

    //$('#txtFchInicial').Zebra_DatePicker({
    //    readonly_element: false,
    //    onSelect: function (date) {
    //        $('#txtFchInicial').val(date);
    //    }
    //});

    //$('#txtFchFin').inputmask({
    //    mask: "1/2/y",
    //    placeholder: "dd/mm/yyyy",
    //    alias: "datetime"
    //});

    //$('#txtFchFin').Zebra_DatePicker({
    //    readonly_element: false,
    //    onSelect: function (date) {
    //        $('#txtFchFin').val(date);
    //    }
    //});

    //$('#txtFchPresentacionIni').inputmask({
    //    mask: "1/2/y",
    //    placeholder: "dd/mm/yyyy",
    //    alias: "datetime"
    //});

    //$('#txtFchPresentacionIni').Zebra_DatePicker({
    //    readonly_element: false,
    //    onSelect: function (date) {
    //        $('#txtFchPresentacionIni').val(date);
    //    }
    //});


    //$('#txtFchPresentacionFin').inputmask({
    //    mask: "1/2/y",
    //    placeholder: "dd/mm/yyyy",
    //    alias: "datetime"
    //});

    //$('#txtFchPresentacionFin').Zebra_DatePicker({
    //    readonly_element: false,
    //    onSelect: function (date) {
    //        $('#txtFchPresentacionFin').val(date);
    //    }
    //});

    $('#cboPuntCodi').multipleSelect({
        filter: true,
        selectAll: false
    });

    //$('#txtFchConformidadIni').inputmask({
    //    mask: "1/2/y",
    //    placeholder: "dd/mm/yyyy",
    //    alias: "datetime"
    //});

    //$('#txtFchConformidadIni').Zebra_DatePicker({
    //    readonly_element: false,
    //    onSelect: function (date) {
    //        $('#txtFchConformidadIni').val(date);
    //    }
    //});


    //$('#txtFchConformidadFin').inputmask({
    //    mask: "1/2/y",
    //    placeholder: "dd/mm/yyyy",
    //    alias: "datetime"
    //});

    //$('#txtFchConformidadFin').Zebra_DatePicker({
    //    readonly_element: false,
    //    onSelect: function (date) {
    //        $('#txtFchConformidadFin').val(date);
    //    }
    //});
    //Fin Mejoras EO - EPO

    $("#btnBuscar").click(function () {
        buscar();
    })

    $("#btnExportar").click(function () {
        exportar();
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
        url: controlador + "ListadoWebEo",
        data: dataFiltros,
        success: function (evt) {
            $('#listado').css("width", $('#mainLayout').width() + "px");

            $('#listado').html(evt);
            $('#tabla').dataTable({
                "scrollY": 430,
                "scrollX": true,
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
        url: controlador + "paginadoEo",
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

function CargarRegistros() {
    pintarPaginado();
    pintarBusqueda(1);
}

function exportar() {
    var dataFiltros = filtrosExportar(1);

    $.ajax({
        type: 'POST',
        url: controlador + "ExportarListadoWebEo",
        data: dataFiltros,
        success: function (evt) {
            descargar(evt);
        },
        error: function () {
            //mostrarError('mensaje');
        }
    });
}

function descargar(nombrearchivo) {
    window.location = controlador + "DescargarEo?nombreArchivo=" + nombrearchivo;
}

function verdetalle(id) {
    window.location = controlador + "VerDetalleEo?id=" + id;
}

function filtros(nroPagina) {
    var filas = 20;

    if (typeof $("#ddlCantidadRegistros").val() != "undefined") {
        filas = $("#ddlCantidadRegistros").val();
    }

    CantidadFilas = filas;

    var filtro = {
        //Inicio Mejoras EO - EPO

        //Esteofechaini: $('#txtFchInicial').val(),
        //Esteofechafin: $('#txtFchFin').val(),
        FIniPresentacion: GetDateNormalFormat($('#txtFchPresentacionIni').val()),
        FFinPresentacion: GetDateNormalFormat($('#txtFchPresentacionFin').val()),
        FIniConformidad: GetDateNormalFormat($('#txtFchConformidadIni').val()),
        FFinConformidad: GetDateNormalFormat($('#txtFchConformidadFin').val()),
        //FIniPresentacion: $('#txtFchPresentacionIni').val(),
        //FFinPresentacion: $('#txtFchPresentacionFin').val(),
        //FIniConformidad: $('#txtFchConformidadIni').val(),
        //FFinConformidad: $('#txtFchConformidadFin').val(),
        Estacodi: $('#cboEstado').val(),
        Emprcoditp: $('#cboTitularProyecto').val(),
        Esteonomb: $('#txtNombreEstudio').val(),
        //Esteopuntoconexion: $('#txtPuntoConexion').val(),
        PuntCodi: $('#cboPuntCodi').val(),
        Esteoanospuestaservicio: $('#txtAnioPuestoServicio').val(),
        Esteocodiproy: $('#txtCodigioProyecto').val(),
        nroPagina: nroPagina,
        nroFilas: CantidadFilas,
        EsteoTipoProyecto: $('#cboTipoProyecto').val(),
        Zoncodi: $('#cboZonaProyeto').val()

        //Fin Mejoras EO - EPO
    }

    return filtro;
}
//Inicio Mejoras EO-EPO-II
function filtrosExportar(nroPagina) {
    var filtro = {
        //Esteofechaini: "",
        //Esteofechafin: "",
        //Estacodi: "",
        //Emprcoditp: "",
        //Esteonomb: "",
        //Esteopuntoconexion: "",
        //Esteoanospuestaservicio: "",
        //Esteocodiproy: "",
        //nroPagina: nroPagina,
        //EsteoTipoProyecto: 0
        FIniPresentacion: GetDateNormalFormat($('#txtFchPresentacionIni').val()),
        FFinPresentacion: GetDateNormalFormat($('#txtFchPresentacionFin').val()),
        FIniConformidad: GetDateNormalFormat($('#txtFchConformidadIni').val()),
        FFinConformidad: GetDateNormalFormat($('#txtFchConformidadFin').val()),
        //FIniPresentacion: $('#txtFchPresentacionIni').val(),
        //FFinPresentacion: $('#txtFchPresentacionFin').val(),
        //FIniConformidad: $('#txtFchConformidadIni').val(),
        //FFinConformidad: $('#txtFchConformidadFin').val(),
        Estacodi: $('#cboEstado').val(),
        Emprcoditp: $('#cboTitularProyecto').val(),
        Esteonomb: $('#txtNombreEstudio').val(),
        PuntCodi: $('#cboPuntCodi').val(),
        Esteoanospuestaservicio: $('#txtAnioPuestoServicio').val(),
        Esteocodiproy: $('#txtCodigioProyecto').val(),
        nroPagina: nroPagina,
        nroFilas: CantidadFilas,
        EsteoTipoProyecto: $('#cboTipoProyecto').val(),
        Zoncodi: $('#cboZonaProyeto').val()
    }

    return filtro;
}
//Fin Mejoras EO-EPO-II

// Convierte formato dd/mm/yyyy a yyyy-mm-dd
GetISODate = function (fechaFormatoNormal) {
    if (!fechaFormatoNormal) {
        return null;
    }
    let fechaISO = fechaFormatoNormal.substring(6, 10) + "-" + fechaFormatoNormal.substring(3, 5) + "-" + fechaFormatoNormal.substring(0, 2);
    return fechaISO;
}

// Convierte formato yyyy-mm-dd a dd/mm/yyyy
GetDateNormalFormat = function (fechaFormatoISO) {
    if (!fechaFormatoISO) {
        return null;
    }
    let fechaNormal = fechaFormatoISO.substring(8, 10) + "/" + fechaFormatoISO.substring(5, 7) + "/" + fechaFormatoISO.substring(0, 4);
    return fechaNormal;
}