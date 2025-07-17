var controlador = siteRoot + 'costooportunidad/admin/';
var hotOperacion = null;
var hotReporte = null;
var hotEquipo = null;
var hotSeniales = null;

const CREAR = 1;
const EDITAR = 2;

var arregloEstructura = [
    { id: 1, text: 'Por unidad' },
    { id: 2, text: 'Por central' }
];

var arregloSiNo = [
    { id: 1, text: 'Si' },
    { id: 2, text: 'No' }
];

var arregloZona = [];
var arregloCanal = [];

$(function () {
    $('#tab-container').easytabs({
        animate: false
    });

    $('#cbPeriodo').on('change', function () {
        cargarVersion();
    });

    $('.txtFechaHabilitacion').Zebra_DatePicker({});

    $('#btnConsultar').on('click', function () {
        if ($('#cbVersion').val() != "0") {
            $('#mensaje').html('');
            $('#mensaje').removeClass();
            cargarDatosVersion();
            cargarListadoURS();
        }
        else {
            $('#lblDatosVersion').html('');
            $('#listadoURS').html('');
            //$('#datosURS').html('');
            mostrarMensaje('mensaje', 'alert', 'Seleccione un periodo y versión.');
        }
    });

    $('#btnGrabarConfiguracion').on('click', function () {
        grabarConfiguracion();
    });



    $('#btnImportar').on('click', function () {
        if ($('#cbVersion').val() != "0") {
            if (confirm('¿Está seguro de realizar esta operación?')) {
                importarConfiguracion();
            }
        }
        else {
            mostrarMensaje('mensaje', 'alert', 'Seleccione un periodo y versión.');
        }
    });

    cargarConfiguracionInicial();
    cargarVersion();
    cargarListadoPeriodosProgramacion();

    $('#txtAddVigencia').Zebra_DatePicker({
        format: 'd/m/Y',
    });

    $('#txtEditVigencia').Zebra_DatePicker({
        format: 'd/m/Y',
    });


    $("#btnAgregarPeriodo").click(function () {
        mostrarPopupPeriodoProg(CREAR);
    });

    $("#btnVerificarPeriodo").click(function () {
        mostrarPopupVerificar();
    });

    $('#cbVerificarAnioPeriodo').change(function () {
        cargarPeriodos();
    });

    $('#btnVerificarP').click(function () {
        verificarPeriodoProg();
    });

    $('#btnAddCancelarPeriodo').click(function () {
        $('#agregarPeriodo').bPopup().close();
    });

    $("#btnAddGrabarPeriodo").click(function () {
        grabarPeriodoProg(CREAR, null);
    });

    $('#btnEditCancelarPeriodo').click(function () {
        $('#editarPeriodo').bPopup().close();
    });

    $("#btnEditGrabarPeriodo").click(function () {
        grabarPeriodoProg(EDITAR, parseInt($("#hfIdPeriodo").val()));
    });
});

function cargarConfiguracionInicial() {
    $('#datosURS').hide();
    $('#excelOperacion').html('');
    $('#excelExtranet').html('');
    $('#excelEquipo').html('');
    //$('#excelSeniales').html('');
}

function cargarVersion() {

    $.ajax({
        type: 'POST',
        url: controlador + 'obtenerversiones',
        data: {
            idPeriodo: $('#cbPeriodo').val()
        },
        dataType: 'json',
        global: false,
        success: function (result) {
            if (result != -1) {
                $('#cbVersion').get(0).options.length = 0;
                $('#cbVersion').get(0).options[0] = new Option("--SELECCIONE--", "0");
                $.each(result, function (i, item) {
                    $('#cbVersion').get(0).options[$('#cbVersion').get(0).options.length] = new Option(item.Coverdesc, item.Covercodi);
                });
            }
            else {
                mostrarMensaje('mensaje', 'error', 'Se ha producido un error.');
            }
        },
        error: function () {
            mostrarMensaje('mensaje', 'error', 'Se ha producido un error.');
        }
    });
};

function cargarDatosVersion() {
    $.ajax({
        type: 'POST',
        url: controlador + 'ObtenerFechasVersion',
        data: {
            idVersion: $('#cbVersion').val()
        },
        dataType: 'json',
        global: false,
        success: function (result) {
            if (result.Result == 1) {
                $('#lblDatosVersion').html("Inicio versión <strong>" + result.Version.FechaInicio + "</strong> - Fin versión <strong>" + result.Version.FechaFin + "</strong>")
                $('#hfFechaInicioVersion').val(result.Version.FechaInicio);
                $('#hfFechaFinVersion').val(result.Version.FechaFin);
            }
            else {
                mostrarMensaje('mensaje', 'error', 'Se ha producido un error.');
            }
        },
        error: function () {
            mostrarMensaje('mensaje', 'error', 'Se ha producido un error.');
        }
    });
};

function cargarListadoURS() {
    $.ajax({
        type: 'POST',
        url: controlador + 'ListadoConfiguracionURS',
        success: function (evt) {
            $('#listadoURS').html(evt);

            $('#datosURS').hide();
            $('#spanCentral').text('');
            $('#txtInicioHabilitacion').val('');
            $('#txtFinHabilitacion').val('');
            $('#hfIdUrs').val('');
            $('#hfNombreCentral').val('');
            $('#hfNombreURS').val('');
            $('#excelOperacion').html('');
            $('#excelExtranet').html('');
            $('#excelEquipo').html('');
            //$('#excelSeniales').html('');
        },
        error: function () {
            mostrarMensaje('mensaje', 'error', 'Ha ocurrido un error.');
        }
    });
};

function verConfiguracion(idUrs, grupoNombre, ursNombre) {

    $.ajax({
        type: 'POST',
        url: controlador + 'ObtenerConfiguracionURS',
        data: {
            idPeriodo: $('#cbPeriodo').val(),
            idVersion: $('#cbVersion').val(),
            idUrs: idUrs
        },
        dataType: 'json',
        global: false,
        success: function (result) {
            if (result.Result == 1) {
                if (hotOperacion != null) hotOperacion.destroy();
                if (hotReporte != null) hotReporte.destroy();
                if (hotEquipo != null) hotEquipo.destroy();
                $('#datosURS').show();
                $('#spanCentral').text(grupoNombre + " - " + ursNombre);
                $('#txtInicioHabilitacion').val(result.Data.FechaInicio);
                $('#txtFinHabilitacion').val(result.Data.FechaFin);
                $('#hfIdUrs').val(idUrs);
                $('#hfNombreCentral').val(grupoNombre);
                $('#hfNombreURS').val(ursNombre);
                $('#excelOperacion').html('');
                $('#excelExtranet').html('');
                $('#excelEquipo').html('');
                //$('#excelSeniales').html('');                
                hotOperacion = cargarSeccionConfiguracion(result.Data.DatosOperacionURS, 1);
                hotReporte = cargarSeccionConfiguracion(result.Data.DatosReporteExtranet, 2);
                hotEquipo = cargarSeccionConfiguracion(result.Data.DatosEquipoRPF, 3);
                cargarZonas(result.Data.ListaZonas);
            }
            else {
                mostrarMensaje('mensaje', 'error', 'Se ha producido un error.');
            }
        },
        error: function () {
            mostrarMensaje('mensaje', 'error', 'Se ha producido un error.');
        }
    });
};

var dropdownSiNoRenderer = function (instance, td, row, col, prop, value, cellProperties) {
    var selectedId;

    for (var index = 0; index < arregloSiNo.length; index++) {
        if (parseInt(value) === arregloSiNo[index].id) {
            selectedId = arregloSiNo[index].id;
            value = arregloSiNo[index].text;
        }
    }
    $(td).addClass("estilocombo");
    Handsontable.TextCell.renderer.apply(this, arguments);
    $('#selectedId').text(selectedId);
};

var dropdownEstructuraRenderer = function (instance, td, row, col, prop, value, cellProperties) {
    var selectedId;

    for (var index = 0; index < arregloEstructura.length; index++) {
        if (parseInt(value) === arregloEstructura[index].id) {
            selectedId = arregloEstructura[index].id;
            value = arregloEstructura[index].text;
        }
    }
    $(td).addClass("estilocombo");
    Handsontable.TextCell.renderer.apply(this, arguments);
    $('#selectedId').text(selectedId);
};

var dropdownZonaRenderer = function (instance, td, row, col, prop, value, cellProperties) {
    var selectedId;

    for (var index = 0; index < arregloZona.length; index++) {
        if (parseInt(value) === arregloZona[index].id) {
            selectedId = arregloZona[index].id;
            value = arregloZona[index].text;
        }
    }
    $(td).addClass("estilocombo");
    Handsontable.TextCell.renderer.apply(this, arguments);
    $('#selectedId').text(selectedId);
};

var dropdownCanalRenderer = function (instance, td, row, col, prop, value, cellProperties) {
    var selectedId;

    for (var j in arregloCanal) {
        for (var index = 0; index < arregloCanal[j].data.length; index++) {
            if (parseInt(value) === arregloCanal[j].data[index].id) {
                selectedId = arregloCanal[j].data[index].id;
                value = arregloCanal[j].data[index].text;
            }
        }
    }
    $(td).addClass("estilocombo");
    Handsontable.TextCell.renderer.apply(this, arguments);
    $('#selectedId').text(selectedId);
};

function cargarSeccionConfiguracion(data, tipo) {

    var headers = [];
    var columns = [
        { type: 'numeric', readOnly: true },
        {},
        { type: "numeric" },
        {
            type: "date",
            dateFormat: "DD/MM/YYYY",
            correctFormat: true
        },
        {
            type: "date",
            dateFormat: "DD/MM/YYYY",
            correctFormat: true
        }
    ];
    var idContainer = "";
    var widths = [];

    if (tipo == 1) {
        headers = ["", "Operación de la URS", "Desde", "Hasta"];
        widths = [1, 400, 200, 200];
        idContainer = "excelOperacion";
    }
    else if (tipo == 2) {
        headers = ["", "Reporte Extranet", "Desde", "Hasta"];
        widths = [1, 400, 200, 200];
        idContainer = "excelExtranet";
    }
    else if (tipo == 3) {
        headers = ["", "Tiene equipo", "R_Equipo", "Desde", "Hasta"];
        widths = [1, 200, 200, 200, 200];
        idContainer = "excelEquipo";
    }

    if (tipo == 1 || tipo == 2) {
        columns = [
            { type: 'numeric', readOnly: true },
            {},
            {
                type: "date",
                dateFormat: "DD/MM/YYYY",
                correctFormat: true
            },
            {
                type: "date",
                dateFormat: "DD/MM/YYYY",
                correctFormat: true
            }
        ];

    }

    var container = document.getElementById(idContainer);

    var hotOptions = {
        data: data,
        colHeaders: headers,
        columns: columns,
        colWidths: widths,
        cells: function (row, col, prop) {

            var cellProperties = {};
            if (col == 1) {
                cellProperties.editor = 'select2';

                if (tipo == 1 || tipo == 2) {
                    cellProperties.renderer = dropdownEstructuraRenderer;
                    cellProperties.select2Options = {
                        data: arregloEstructura,
                        dropdownAutoWidth: true,
                        width: 'resolve',
                        allowClear: false,
                    };
                }
                else {
                    cellProperties.renderer = dropdownSiNoRenderer;
                    cellProperties.select2Options = {
                        data: arregloSiNo,
                        dropdownAutoWidth: true,
                        width: 'resolve',
                        allowClear: false,
                    };
                }


            }
            if (tipo == 3 && col == 2) {
                cellProperties.format = '0,0.00';
                cellProperties.type = 'numeric';
            }

            return cellProperties;
        },
        afterLoadData: function () {
            this.render();
        }
    };

    var hot = new Handsontable(container, hotOptions);

    var items = [];

    if (tipo == 1) {
        items = {
            "row_below": {
                name: function () {
                    return " <div class= 'icon-agregar' >Agregar fila</div> ";
                }
            },
            "remove_row": {
                name: function () {
                    return " <div class= 'icon-eliminar' >Eliminar fila</div> ";
                },
                disabled: function () {
                    return (hot.getData().length == 1);
                }
            },
            "properties_row": {
                name: function () {
                    return " <div class= 'icon-properties' >Configurar Señales</div> ";
                },
                disabled: function () {
                    return (hot.getDataAtCell(hot.getSelected()[0], 1) == null || hot.getDataAtCell(hot.getSelected()[0], 0) == null)
                }
            }
        }
    }
    else if (tipo == 2) {
        items = {
            "row_below": {
                name: function () {
                    return " <div class= 'icon-agregar' >Agregar fila</div> ";
                }
            },
            "remove_row": {
                name: function () {
                    return " <div class= 'icon-eliminar' >Eliminar fila</div> ";
                },
                disabled: function () {
                    return (hot.getData().length == 1);
                }
            },
            "generador_row": {
                name: function () {
                    return " <div class= 'icon-properties' >Configurar Unidades</div> ";
                },
                disabled: function () {
                    return (hot.getDataAtCell(hot.getSelected()[0], 1) == null || hot.getDataAtCell(hot.getSelected()[0], 1) == "2" || hot.getDataAtCell(hot.getSelected()[0], 0) == null)
                }
            }
        }
    }
    else {
        items = {
            "row_below": {
                name: function () {
                    return " <div class= 'icon-agregar' >Agregar fila</div> ";
                }
            },
            "remove_row": {
                name: function () {
                    return " <div class= 'icon-eliminar' >Eliminar fila</div> ";
                },
                disabled: function () {
                    return (hot.getData().length == 1);
                }
            }
        }
    }

    hot.updateSettings({
        contextMenu: {
            callback: function (key, options) {
                if (key === 'properties_row') {
                    var fila = hot.getSelected()[0];
                    var idConfiguracion = hot.getDataAtCell(fila, 0);
                    var tipoConfiguracion = hot.getDataAtCell(fila, 1);
                    var idUrs = $('#hfIdUrs').val();
                    cargarConfiguracionSenialView(idConfiguracion, tipoConfiguracion, idUrs);

                }
                if (key === 'generador_row') {
                    var fila = hot.getSelected()[0];
                    var idConfiguracion = hot.getDataAtCell(fila, 0);
                    var idUrs = $('#hfIdUrs').val();
                    cargarConfiguracionGeneradorView(idConfiguracion, idUrs);
                }
            },
            items: items
        }
    });

    return hot;
}

function cargarZonas(data) {
    arregloZona = [];

    for (var j in data) {
        arregloZona.push({ id: data[j].Zonacodi, text: data[j].Zonanomb });
    }
}

function cargarConfiguracionSenialView(idConfiguracion, tipoConfiguracion, idUrs) {
    $.ajax({
        type: 'POST',
        url: controlador + 'ConfiguracionSenial',
        global: false,
        success: function (html) {
            $('#contenidoSeniales').html(html);
            $('#popupSeniales').bPopup({
                autoClose: false,
                follow: [false, false],
                modalClose: false,
            });

            $('#btnGrabarSenial').on('click', function () {
                grabarConfiguracionSenial();
            });

            $('#btnCancelarSenial').on('click', function () {
                $('#popupSeniales').bPopup().close();
            });

            cargarConfiguracionSenial(idConfiguracion, tipoConfiguracion, idUrs);
        },
        error: function () {
            mostrarMensaje('mensaje', 'error', 'Se ha producido un error.');
        }
    });
}

function cargarConfiguracionGeneradorView(idConfiguracion, idUrs) {
    $.ajax({
        type: 'POST',
        url: controlador + 'ConfiguracionGen',
        data: {
            idCondiguracionDet: idConfiguracion,
            idUrs: idUrs
        },
        global: false,
        success: function (html) {
            $('#contenidoGenerador').html(html);
            $('#popupGenerador').bPopup({
            });

            $('#btnGrabarGenerador').on('click', function () {
                grabarConfiguracionGenerador();
            });

            $('#btnCancelarGenerador').on('click', function () {
                $('#popupGenerador').bPopup().close();
            });

            $('#cbSelectAll').click(function (e) {
                var table = $(e.target).closest('table');
                $('td input:checkbox', table).prop('checked', this.checked);
            });

        },
        error: function () {
            mostrarMensaje('mensaje', 'error', 'Se ha producido un error.');
        }
    });
}

function grabarConfiguracionGenerador() {
    $('#mensajeGenerador').removeClass();
    $('#mensajeGenerador').html('');
    var equipos = "";
    var newEquipos = "";
    countEquipos = 0;
    $('#tablaGeneracion tbody input:checked').each(function () {
        equipos = equipos + $(this).val() + ",";
        countEquipos++;
    });

    if (countEquipos > 0) {
        newEquipos = equipos.substring(0, equipos.length - 1);
    }

    if (countEquipos > 0) {

        $.ajax({
            type: 'POST',
            url: controlador + 'GrabarConfiguracionGenerador',
            data: {
                idConfiguracionDet: $('#hfIdConfiguracionDet').val(),
                data: newEquipos
            },
            dataType: 'json',
            success: function (result) {
                if (result.Result == 1) {
                    mostrarMensaje('mensajeGenerador', 'exito', 'La configuración se grabó correctamente.');
                    $('#popupGenerador').bPopup().close();
                }
                else {
                    mostrarMensaje('mensajeGenerador', 'error', 'Ha ocurrido un error.');
                }
            },
            error: function () {
                mostrarMensaje('mensajeGenerador', 'error', 'Ha ocurrido un error.');
            }
        });
    }
    else {
        mostrarMensaje('mensajeGenerador', 'alert', 'Por favor seleccione al menos una unidad');
    }
}

function cargarConfiguracionSenial(idConfiguracion, tipoConfiguracion, idUrs) {

    $.ajax({
        type: 'POST',
        url: controlador + 'ObtenerConfiguracionSenial',
        data: {
            idCondiguracionDet: idConfiguracion,
            tipo: tipoConfiguracion,
            idUrs: idUrs
        },
        dataType: 'json',
        global: true,
        success: function (result) {
            if (result.Result == 1) {
                cargarGrillaSenial(result.Data);
                $('#mensajeSenial').html('');
                $('#mensajeSenial').removeClass();
                $('#hfIdConfiguracionDetalle').val(idConfiguracion);
                setTimeout(function () {
                    $('#popupSeniales').bPopup({
                        autoClose: false,
                        follow: [false, false],
                        modalClose: false,
                    });
                }, 50);
            }
            else {
                mostrarMensaje('mensaje', 'error', 'Se ha producido un error.');
            }
        },
        error: function () {
            mostrarMensaje('mensaje', 'error', 'Se ha producido un error.');
        }
    });

};

function cargarGrillaSenial(data) {
    var headers = ["", "URS - GEN", "UBICACION", "TIPO SEÑAL", 'SEÑAL', 'VALOR INICIAL'];
    var columns = [
        { type: 'numeric', readOnly: true },
        { type: 'text', readOnly: true },
        {},
        { type: 'text', readOnly: true },
        {},
        { type: "numeric" }
    ];
    var widths = [1, 130, 170, 150, 530, 130];
    var container = document.getElementById('excelSeniales');
    var grupos = data.DataSeniales.length / 6;

    var merge = [];
    arregloCanal = [];

    for (var i = 0; i < grupos; i++) {
        merge.push({ row: 0 + i * 6, col: 1, rowspan: 6, colspan: 1 });
        merge.push({ row: 0 + i * 6, col: 2, rowspan: 6, colspan: 1 });
        arregloCanal.push({ grupo: i, data: [], renderer: null });
        cargarCanales(data.DataCanales[i].Data, i);
    }


    var hotOptions = {
        data: data.DataSeniales,
        colHeaders: headers,
        columns: columns,
        colWidths: widths,
        mergeCells: merge,
        fillHandle: false,
        cells: function (row, col, prop) {

            var cellProperties = {};

            if (col == 2) {
                cellProperties.editor = 'select2';
                cellProperties.renderer = dropdownZonaRenderer;
                cellProperties.select2Options = {
                    data: arregloZona,
                    dropdownAutoWidth: true,
                    width: 'resolve',
                    allowClear: false
                }
            }
            if (col == 4) {
                cellProperties.editor = 'select2';
                cellProperties.renderer = dropdownCanalRenderer;
                cellProperties.select2Options = {
                    data: (arregloCanal[Math.floor(row / 6)]).data,
                    dropdownAutoWidth: true,
                    width: 'resolve',
                    allowClear: false
                }
            }

            if (col == 5) {
                cellProperties.format = '0,0.00';
                cellProperties.type = 'numeric';
            }

            return cellProperties;
        },
        afterLoadData: function () {
            this.render();
        },
        afterChange: function (changes, source) {
            if (source == "edit") {

                var filaCambiada = (changes[0])[0];
                var propiedadCambiada = ((changes[0])[1]);
                var valorActual = (changes[0])[2];
                var valorNuevo = (changes[0])[3];

                if (propiedadCambiada == 2) {
                    if (valorActual != valorNuevo) {

                        $.ajax({
                            type: 'POST',
                            url: controlador + 'ListaCanalPorZona',
                            data: {
                                zonaCodi: valorNuevo
                            },
                            dataType: 'json',
                            global: false,
                            success: function (result) {
                                cargarCanales(result, Math.floor(filaCambiada / 6));

                            },
                            error: function () {
                                mostrarMensaje('mensaje', 'error', 'Se ha producido un error.');
                            }
                        });
                    }
                }
            }
        }
    };

    /*if (hotSeniales != null) {
        hotSeniales.destroy()
    }*/

    hotSeniales = new Handsontable(container, hotOptions);

};

function cargarCanales(data, indice) {
    (arregloCanal[indice]).data = [];
    for (var j in data) {
        (arregloCanal[indice]).data.push({ id: data[j].Canalcodi, text: data[j].Canalnomb });
    }
};

function grabarConfiguracion() {
    var validacion = validarRegistro();

    if (validacion == "") {
        $.ajax({
            type: 'POST',
            url: controlador + "GrabarConfiguracionURS",
            contentType: 'application/json',
            data: JSON.stringify({
                idUrs: $('#hfIdUrs').val(),
                idPeriodo: $('#cbPeriodo').val(),
                idVersion: $('#cbVersion').val(),
                fechaInicio: $('#txtInicioHabilitacion').val(),
                fechaFin: $('#txtFinHabilitacion').val(),
                dataOperacion: hotOperacion.getData(),
                dataReporte: hotReporte.getData(),
                dataEquipo: hotEquipo.getData()
            }),
            dataType: 'json',
            success: function (result) {
                if (result.Result == 1) {
                    mostrarMensaje('mensaje', 'exito', 'La configuración se grabó satisfactoriamente.');
                    verConfiguracion($('#hfIdUrs').val(), $('#hfNombreCentral').val(), $('#hfNombreURS').val());
                }
                else {
                    mostrarMensaje('mensaje', 'error', 'Se ha producido un error.');
                }
            },
            error: function () {
                mostrarMensaje('mensaje', 'error', 'Se ha producido un error.');
            }
        });
    }
    else {
        mostrarMensaje('mensaje', 'alert', validacion);
    }
};

function validarRegistro() {
    var flagValidacion = true;
    var mensaje = "<ul>";
    if ($('#txtInicioHabilitacion').val() != '' && $('#txtFinHabilitacion').val()) {
        if (getFecha($('#txtInicioHabilitacion').val()) > getFecha($('#txtFinHabilitacion').val())) {
            mensaje = mensaje + "<li>La de inicio de habilitación no puede ser mayor a la fecha final de habiliación</li>";
            flagValidacion = false;
        }
    }
    var mensajeOperacion = validarSeccionConfiguracion(hotOperacion.getData(), 0);
    var mensajeReporte = validarSeccionConfiguracion(hotReporte.getData(), 1);
    var mensajeEquipo = validarSeccionConfiguracion(hotEquipo.getData(), 2);

    var flagDatosBasicos = true;

    if (mensajeOperacion != "" || mensajeReporte != "" || mensajeEquipo != "") {
        flagValidacion = false;
        flagDatosBasicos = false;
        mensaje = mensaje + mensajeOperacion;
        mensaje = mensaje + mensajeReporte;
        mensaje = mensaje + mensajeEquipo;
    }

    if (flagDatosBasicos) {
        var mensajeFechaOperacion = validarSeccionFechas(hotOperacion.getData(), 0);
        var mensajeFechaReporte = validarSeccionFechas(hotReporte.getData(), 1);
        var mensajeFechaEquipo = validarSeccionFechas(hotEquipo.getData(), 2);
        if (mensajeFechaOperacion != "" || mensajeFechaReporte != "" || mensajeFechaEquipo != "") {
            flagValidacion = false;
            mensaje = mensaje + mensajeFechaOperacion;
            mensaje = mensaje + mensajeFechaReporte;
            mensaje = mensaje + mensajeFechaEquipo;
        }

        var mensajeValoresOperacion = validarValoresConfiguracion(hotOperacion.getData(), 0);
        var mensajeValoresReporte = validarValoresConfiguracion(hotReporte.getData(), 1);
        var mensajeValoresEquipo = validarValoresConfiguracion(hotEquipo.getData(), 2);

        if (mensajeValoresOperacion != "" || mensajeValoresReporte != "" || mensajeValoresEquipo != "") {
            flagValidacion = false;
            mensaje = mensaje + mensajeValoresOperacion;
            mensaje = mensaje + mensajeValoresReporte;
            mensaje = mensaje + mensajeValoresEquipo;
        }
    }

    mensaje = mensaje + "</ul>";
    if (flagValidacion) mensaje = "";
    return mensaje;
};

function validarSeccionConfiguracion(data, tipo) {
    var secciones = ["Operación de la URS", "Reporte Extranet", "Equipo para RPF"];
    var columnas = ["Operación de la URS", "Reporte Extranet", "Tiene equipo"];
    var mensaje = "";
    for (var i in data) {
        if (data[i][1] == "" || data[i][1] == null) {
            mensaje = mensaje + "<li>Complete los datos de la columna '" + columnas[tipo] + "' en la sección : '" + secciones[tipo] + "'</li>";
        }
        var indiceFecha = 2;
        if (tipo == 2) indiceFecha = 3;
        if (data[i][indiceFecha] == "" || data[i][indiceFecha] == null) {
            mensaje = mensaje + "<li>Ingrese fecha 'desde' en la sección: '" + secciones[tipo] + "'</li>";
        }
    }
    return mensaje;
};

function validarSeccionFechas(data, tipo) {
    var fechaInicioVersion = getFecha($('#hfFechaInicioVersion').val());
    var fechaFinVersion = getFecha($('#hfFechaFinVersion').val());
    var secciones = ["Operación de la URS", "Reporte Extranet", "Equipo para RPF"];
    var mensaje = "";
    for (var i in data) {
        var indiceFecha = 2;
        if (tipo == 2) indiceFecha = 3;

        if (data[i][indiceFecha + 1] != null && data[i][indiceFecha + 1] != "") {
            if (getFecha(data[i][indiceFecha]) > getFecha(data[i][indiceFecha + 1])) {
                mensaje = mensaje + "<li>Sección " + secciones[tipo] + ": La fecha desde no puede superar la fecha hasta.</li>";
            }
        }

        if (i == 0) {
            if (getFecha(data[i][indiceFecha]) != fechaInicioVersion) {
                mensaje = mensaje + "<li>Sección " + secciones[tipo] + ": La fecha desde del primer registro debe ser igual a la fecha de inicio de la versión.</li>";
            }
        }
        if (i == data.length - 1) {
            if (data[i][indiceFecha + 1] != null && data[i][indiceFecha + 1] != "") {

                if (getFecha(data[i][indiceFecha + 1]) != fechaFinVersion) {
                    mensaje = mensaje + "<li>Sección " + secciones[tipo] + ": La fecha fin del último registro debe ser igual a la fecha de fin de la versión.</li>";
                }
            }
        }

        if (i > 0) {
            if (data[i - 1][indiceFecha + 1] != null && data[i - 1][indiceFecha + 1] != "") {
                console.log(data[i - 1][indiceFecha + 1]);
                console.log(data[i][indiceFecha]);
                console.log(getFecha(data[i - 1][indiceFecha + 1]));
                console.log(getFecha(data[i][indiceFecha]));

                if ((getFecha(data[i - 1][indiceFecha + 1]) + 86400 * 1000) != getFecha(data[i][indiceFecha])) {
                    mensaje = mensaje + "<li>Sección " + secciones[tipo] + ": La configuración debe cubrir todo el rango de fechas.</li>";
                }
            }
        }
    }
    return mensaje;
};

function validarValoresConfiguracion(data, tipo) {
    var secciones = ["Operación de la URS", "Reporte Extranet", "Equipo para RPF"];
    var mensaje = "";
    for (var i in data) {
        if (i > 0) {
            if (data[i][1] == data[i - 1][1]) {
                var suma = i + 1;
                mensaje = mensaje + "<li>Sección " + secciones[tipo] + ": Los valores de las filas " + (i) + " y " + suma + " no debens ser iguales.</li>";
            }
        }
        var indice = 2;
        if (tipo == 2) indice = 3;
        if (!validarFecha(data[i][indice])) {
            mensaje = mensaje + "<li>Sección " + secciones[tipo] + ": Corregir el formato de fecha de la columna desde.</li>";
        }
        if (data[i][indice + 1] != null && data[i][indice + 1] != "") {
            if (!validarFecha(data[i][indice + 1])) {
                mensaje = mensaje + "<li>Sección " + secciones[tipo] + ": Corregir el formato de fecha de la columna hasta.</li>";
            }
        }

        if (tipo == 2) {
            if (data[i][2] != null && data[i][2] != "") {
                if (!validarNumero(data[i][2])) {
                    mensaje = mensaje + "<li>Sección " + secciones[tipo] + ": El valor de R_Equipo debe ser numérico</li>";
                }
            }
        }
    }
    return mensaje;
};

function grabarConfiguracionSenial() {
    var mensaje = validarConfiguracionSenial(hotSeniales.getData());

    if (mensaje == "") {
        $.ajax({
            type: 'POST',
            url: controlador + "GrabarConfiguracionSenial",
            contentType: 'application/json',
            data: JSON.stringify({
                idConfiguracionDet: $('#hfIdConfiguracionDetalle').val(),
                idUrs: $('#hfIdUrs').val(),
                data: hotSeniales.getData()
            }),
            dataType: 'json',
            success: function (result) {
                if (result.Result == 1) {
                    mostrarMensaje('mensaje', 'exito', 'La configuración de las señales se realizó con éxito');
                    $('#popupSeniales').bPopup().close();
                }
                else {
                    mostrarMensaje('mensajeSenial', 'error', 'Se ha producido un error.');
                }
            },
            error: function () {
                mostrarMensaje('mensajeSenial', 'error', 'Se ha producido un error.');
            }
        });
    }
    else {
        mostrarMensaje('mensajeSenial', 'alert', mensaje)
    }
}

function validarConfiguracionSenial(data) {

    var mensaje = "";
    var flagZona = true;
    var flagCanal = true;
    var flagValor = true;
    for (var i in data) {
        if ((data[i][2] == null || data[i][2] == "") && (i) % 6 == 0) {
            flagZona = false;
        }
        if (data[i][4] == null || data[i][4] == "") {
            flagCanal = false;
        }
        if (data[i][5] != null && data[i][5] != "") {
            if (!validarNumero(data[i][5])) {
                flagValor = false;
            }
        }
    }
    if (!flagZona) mensaje = mensaje + "<li>Debe seleccionar todas las zonas</li>";
    if (!flagCanal) mensaje = mensaje + "<li>Debe seleccionar todas los canales</li>";
    if (!flagValor) mensaje = mensaje + "<li>Debe ingresar solo valores numéricos en el campo 'Valor inicial'</li>";

    return mensaje;
}

function validarNumero(texto) {
    return /^-?[\d.]+(?:e-?\d+)?$/.test(texto);
}

function validarFecha(fecha) {
    return /^(0[1-9]|[12][0-9]|3[01])[- /.](0[1-9]|1[012])[- /.](19|20)\d\d$/.test(fecha);
}

function importarConfiguracion() {
    $.ajax({
        type: 'POST',
        url: controlador + "ImportarConfiguracionURS",
        data: {
            idPeriodo: $('#cbPeriodo').val(),
            idVersion: $('#cbVersion').val()
        },
        dataType: 'json',
        success: function (result) {
            if (result.Result == 1) {
                mostrarMensaje('mensaje', 'exito', 'La importación se realizó con éxito.');
                cargarListadoURS();
            }
            else if (result.Result == 2) {
                mostrarMensaje('mensaje', 'alert', 'No se encontraron configuraciones en el periodo y versión anterior.');
            }
            else if (result.Result == 3) {
                mostrarMensaje('mensaje', 'alert', 'No se encontró periodo y versión anterior.');
            }
            else if (result.Result == 4) {
                mostrarMensaje('mensaje', 'alert', 'No se puede realizar la importación porque el periodo y versión ya tiene datos de configuración.');
            }
            else {
                mostrarMensaje('mensaje', 'error', 'Se ha producido un error.');
            }
        },
        error: function () {
            mostrarMensaje('mensaje', 'error', 'Se ha producido un error.');
        }
    });
}

function getFecha(date) {
    var parts = date.split("/");
    var date1 = new Date(parts[1] + "/" + parts[0] + "/" + parts[2]);
    return date1.getTime();
}

function mostrarMensaje(id, tipo, mensaje) {
    $('#' + id).removeClass();
    $('#' + id).addClass('action-' + tipo);
    $('#' + id).html(mensaje);
}

//******** PERIODOS DE PROGRAMACION **********/

function cargarListadoPeriodosProgramacion() {
    $.ajax({
        type: 'POST',
        url: controlador + 'ListarPeriodosProg',
        success: function (evt) {
            if (evt.Resultado != "-1") {
                $('#listadoPeriodosProg').html(evt.Resultado);
                refrehDatatable();
                $("#hfultimaVigencia").val(evt.UltimaVigencia);
            } else {
                mostrarMensaje_('mensaje', 'error', 'Ha ocurrido un error: ' + evt.Mensaje);
            }
        },
        error: function () {
            mostrarMensaje_('mensaje', 'error', 'Ha ocurrido un error.');
        }
    });
}

function refrehDatatable() {
    $('#tabla_lstPeriodosProg').dataTable({
        "sDom": 't',
        "ordering": false,
    });
}

function mostrarPopupPeriodoProg(accion) {
    var id = "";
    var ventana = "";
    if (accion == CREAR) {
        id = "mensajeAddPeriodo";
        ventana = "agregarPeriodo";
    }
    if (accion == EDITAR) {
        id = "mensajeEditPeriodo";
        ventana = "editarPeriodo";
    }

    ocultarMensajeValidacion(id);
    limpiarCamposPeriodoProg(accion);
    abrirPopup(ventana);
}

function ocultarMensajeValidacion(id) {
    mostrarMensaje_(id, 'message', '');
    $("#" + id).css("display", "none");
}

function limpiarCamposPeriodoProg(accion) {
    if (accion == CREAR) {
        $('#txtAddValor').val(0.5);
        $('#txtAddVigencia').val($('#hftxtAddVigencia').val());
    }
}

function abrirPopup(contentPopup) {
    setTimeout(function () {
        $("#" + contentPopup).bPopup({
            easing: 'easeOutBack',
            speed: 450,
            transition: 'slideDown',
            modalClose: false
        });
    }, 50);
}

function grabarPeriodoProg(accion, idPeriodo) {
    var msg = "";
    var objCampos = {};
    var popup = "";
    var notamsg = "";
    var fecIni = "";

    if (accion == CREAR) {
        popup = "agregarPeriodo";
        notamsg = "mensajeAddPeriodo";
    }

    if (accion == EDITAR) {
        popup = "editarPeriodo";
        notamsg = "mensajeEditPeriodo";
        fecIni = $("#hfFecVigenciaIni").val();
    }

    objCampos = getCamposPeriodoProg(accion);

    msg = validarCamposPeriodoProg(accion, objCampos);
    if (msg == "") {

        limpiarBarraMensaje("mensajeAddPeriodo");
        $.ajax({
            type: 'POST',
            url: controlador + 'GuardarPeriodoProg',
            data: {
                accion: accion,
                idPeriodo: idPeriodo,
                valor: objCampos.valor,
                vigencia: objCampos.vigencia,
                fechaIni: fecIni,
            },
            success: function (result) {
                if (result.Resultado == "1") {
                    $('#' + popup).bPopup().close();

                    mostrarMensaje_('mensaje_periodos', 'exito', 'Periodo de Programación T registrado con éxito.');
                    cargarListadoPeriodosProgramacion();
                    $("#hfultimaVigencia").val(result.UltimaVigencia);
                } else {
                    mostrarMensaje_(notamsg, 'error', result.Mensaje);
                }
            },
            error: function () {
                mostrarMensaje_(notamsg, 'error', 'Ha ocurrido un error.');
            }
        });
    }
    else {
        mostrarMensaje_(notamsg, 'error', msg);
    }
}

function editarPeriodoProg(idPeriodo, valor, dia, mes, anio) {
    var fecha = dia.toString().padStart(2, '0') + "/" + mes.toString().padStart(2, '0') + "/" + anio;

    $("#txtEditValor").val(valor);
    $("#txtEditVigencia").val(fecha);
    $("#hfIdPeriodo").val(idPeriodo);
    $("#hfFecVigenciaIni").val(fecha);

    mostrarPopupPeriodoProg(EDITAR);
}

function eliminarPeriodoProg(idPeriodo) {

    if (confirm('¿Esta seguro de eliminar el periodo de programación escogido?')) {

        $.ajax({
            type: 'POST',
            url: controlador + 'EliminarPeriodoProg',
            data: {
                idPeriodo: idPeriodo
            },
            success: function (result) {
                if (result.Resultado == "1") {
                    mostrarMensaje_('mensaje_periodos', 'exito', 'Periodo de Programación T eliminado con éxito.');
                    cargarListadoPeriodosProgramacion();
                    $("#hfultimaVigencia").val(result.UltimaVigencia);
                } else {
                    mostrarMensaje_(notamsg, 'error', 'Se ha producido un error: ' + result.Mensaje);
                }
            },
            error: function () {
                mostrarMensaje_(notamsg, 'error', 'Ha ocurrido un error.');
            }
        });
    }
}

function validarCamposPeriodoProg(accion, objCampos) {
    var msj = "";
    var val;
    const datosValidos = [0.25, 0.5, 0.75, 1, 1.5, 2, 3, 4, 6, 8, 12, 24];

    if (objCampos.valor == "") {
        msj += "<p>Debe ingresar un valor correcto.</p>";
    } else {
        val = parseFloat(objCampos.valor);
        if (val % 0.25 == 0) {
            if (val >= 0.25 && val <= 24) {
                if (!datosValidos.includes(val)) {
                    msj = "<p>Debe ingresar un valor correcto (0.25, 0.5, 0.75, 1, 1.5, 2, 3, 4, 6, 8, 12 ó 24).</p>";
                }
            } else {
                msj += "<p>Debe ingresar un valor correcto (0.25, 0.5, 0.75, 1, 1.5, 2, 3, 4, 6, 8, 12 ó 24).</p>";
            }
        } else {
            msj += "<p>Debe ingresar un valor correcto (0.25, 0.5, 0.75, 1, 1.5, 2, 3, 4, 6, 8, 12 ó 24).</p>";
        }
    }

    if (accion == CREAR) {
        var ultimaVigencia = $('#hfultimaVigencia').val();
        if (ultimaVigencia != "") {

            const [dayU, monthU, yearU] = ultimaVigencia.split('/');
            const [dayV, monthV, yearV] = objCampos.vigencia.split('/');

            const ultVigencia = new Date(yearU, monthU - 1, dayU);
            const vigencia = new Date(yearV, monthV - 1, dayV);
            if (vigencia <= ultVigencia) {
                msj += "<p>Debe ingresar una vigencia correcta (posterior a " + ultimaVigencia + ").</p>";
            }
        }
    }



    return msj;
}

function getCamposPeriodoProg(accion) {
    var obj = {};

    if (accion == CREAR) {
        obj.valor = $('#txtAddValor').val();
        obj.vigencia = $('#txtAddVigencia').val();
    }
    if (accion == EDITAR) {
        obj.valor = $('#txtEditValor').val();
        obj.vigencia = $('#txtEditVigencia').val();
    }

    return obj;
}

function diaActual() { //devuelve strFecha en formato dd/mm/yyyy
    var now = new Date();
    var strDateTime = [[AddZero(now.getDate()), AddZero(now.getMonth() + 1), now.getFullYear()].join("/")].join(" ");

    return strDateTime;
}


function mostrarPopupVerificar() {
    ocultarMensajeValidacion("mensajeVerificarPeriodo");
    limpiarCamposVerificar();
    abrirPopup("verificarPeriodo");
}

function limpiarCamposVerificar() {
    $('#cbVerificarVersion').val(-1);
    $('#cbVerificarAnioPeriodo').val($('#hfAnioCombo').val());
    cargarPeriodos();
    $('#cbVerificarMesPeriodo').val($('#hfMesCombo').val());

    $('#rangoAVerificar').html("");
    $('#listadoVerificacion').html("");
}

function cargarPeriodos() {
    var annio = -1;

    annio = parseInt($("#cbVerificarAnioPeriodo").val()) || 0;
    $("#cbVerificarMesPeriodo").empty();

    $.ajax({
        type: 'POST',
        url: controlador + "PeriodoListado",
        data: {
            anio: annio,
        },
        success: function (evt) {
            if (evt.Resultado != "-1") {
                if (evt.ListaPeriodos.length > 0) {
                    $.each(evt.ListaPeriodos, function (i, item) {
                        $('#cbVerificarMesPeriodo').get(0).options[$('#cbVerificarMesPeriodo').get(0).options.length] = new Option(item.Copernomb, item.Copercodi);
                    });

                } else {
                    $('#cbVerificarMesPeriodo').get(0).options[0] = new Option("--", "0");
                }

            } else {
                mostrarMensaje_('mensajeVerificarPeriodo', 'error', "Ha ocurrido un error: " + evt.Mensaje);
            }
        },
        error: function (err) {
            mostrarMensaje_('mensajeVerificarPeriodo', 'error', 'Ha ocurrido un error.');
        }
    });
}

function verificarPeriodoProg() {
    var msg = "";
    var objCampos = {};
    var notamsg = "mensajeVerificarPeriodo";

    $('#rangoAVerificar').html("");
    $('#listadoVerificacion').html("");

    objCampos = getCamposVerificarPeriodoProg();

    msg = validarCamposVerificacionPeriodoProg(objCampos);
    if (msg == "") {

        limpiarBarraMensaje(notamsg);
        $.ajax({
            type: 'POST',
            url: controlador + 'VerificarPeriodoProg',
            data: {
                periodo: objCampos.periodo,
                version: objCampos.version
            },
            success: function (result) {
                var tipoMsg = "";
                var mensaje = "";

                if (result.Resultado != "-1") {
                    $('#rangoAVerificar').html(result.Rango);

                    if (result.HayDiferencia) {
                        tipoMsg = "alert";
                        mensaje = "Existen diferencias entre el periodo de programación y la variable delta";

                        $('#listadoVerificacion').html(result.Resultado);
                        refrehDatatable2();
                    } else {
                        tipoMsg = "exito";
                        mensaje = "No existen diferencias entre el periodo de programación y la variable delta";
                    }

                    mostrarMensaje_(notamsg, tipoMsg, mensaje);

                } else {
                    mostrarMensaje_(notamsg, 'error', 'Se ha producido un error: ' + result.Mensaje);
                }
            },
            error: function () {
                mostrarMensaje_(notamsg, 'error', 'Ha ocurrido un error.');
            }
        });
    }
    else {
        mostrarMensaje_(notamsg, 'error', msg);
    }
}

function refrehDatatable2() {

    $('#tabla_Verificados').dataTable({
        "filter": false,
        "scrollCollapse": true,
        "paging": false,
        "scrollY": 220,
    });
}

function getCamposVerificarPeriodoProg() {
    var obj = {};

    obj.periodo = $("#cbVerificarMesPeriodo").val();
    obj.version = parseInt($("#cbVerificarVersion").val());

    return obj;
}

function validarCamposVerificacionPeriodoProg(objCampos) {
    var msj = "";

    if (objCampos.periodo == "") {
        msj += "<p>Debe escoger un periodo correcto.</p>";
    }

    if (objCampos.version == -1) {
        msj += "<p>Debe escoger una versión correcta.</p>";
    }

    return msj;
}

function mostrarMensaje_(id, tipo, mensaje) {
    $("#" + id).css("display", "block");
    $('#' + id).removeClass();
    $('#' + id).addClass('action-' + tipo);
    $('#' + id).html(mensaje);

}

function limpiarBarraMensaje(id) {
    $('#' + id).css("display", "none");
    $('#' + id).removeClass();
    $('#' + id).addClass('action-message');
    $('#' + id).html('');
}

