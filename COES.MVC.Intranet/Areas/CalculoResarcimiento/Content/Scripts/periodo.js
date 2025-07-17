var controlador = siteRoot + 'calculoresarcimiento/periodo/';
var hot = null;

$(function () {

    $('#btnConsultar').on('click', function () {
        consultar();
    });

    $('#btnNuevo').on('click', function () {
        editar(0);
    });

    $('#txtAnioDesde').Zebra_DatePicker({
        format: 'Y'
    });

    $('#txtAnioHasta').Zebra_DatePicker({
        format: 'Y'
    });

    $('#btnAceptarCopiar').on('click', function () {
        copiarDesdeTrimestral();
    });

    $('#btnCancelarCopiar').on('click', function () {
        $('#popupCopiar').bPopup().close();
    });

    consultar();
});

function consultar() {
    $('#mensaje').html("");
    $('#mensaje').removeClass();
    if ($('#txtAnioDesde').val() != "" && $('#txtAnioHasta').val() != "") {
        if (parseInt($('#txtAnioHasta').val()) >= parseInt($('#txtAnioDesde').val())) {
            $.ajax({
                type: 'POST',
                url: controlador + 'listado',
                data: {
                    anioDesde: $('#txtAnioDesde').val(),
                    anioHasta: $('#txtAnioHasta').val(),
                    estado: $('#cbEstadoFiltro').val()
                },
                success: function (evt) {
                    $('#listado').html(evt);
                    $('#tablaPeriodo').dataTable({
                        "iDisplayLength": 20
                    });
                },
                error: function () {
                    mostrarMensaje('mensaje', 'error', 'Ha ocurrido un error.');
                }
            });
        }
        else {
            mostrarMensaje('mensaje', 'alert', 'El año hasta debe ser mayor o igual al año desde.');
        }
    }
    else {
        mostrarMensaje('mensaje', 'alert', 'Seleccione un rango de años.');
    }
}

function editar(id) {
    limpiarMensaje('mensaje');
    $.ajax({
        type: 'POST',
        url: controlador + 'editar',
        data: {
            id: id
        },
        global: false,
        success: function (evt) {
            $('#contenidoEdicion').html(evt);
            setTimeout(function () {
                $('#popupEdicion').bPopup({
                    autoClose: false,
                    modalClose: false,
                    escClose: false,
                    follow: [false, false]
                });
            }, 50);

            $('#btnGrabar').on("click", function () {
                grabarRegistro();
            });

            $('#btnCancelar').on("click", function () {
                $('#popupEdicion').bPopup().close();
            });

            $('#txtAnio').Zebra_DatePicker({
                format: 'Y',
                onSelect: function (date) {
                    cargarPadres(date)
                }
            });

            $('#txtFechaInicio').Zebra_DatePicker({               
            });

            $('#txtFechaFin').Zebra_DatePicker({               
            });

            $('#cbTipoPeriodo').val($('#hfTipoPeriodo').val());
            $('#cbPeriodoRevision').val($('#hfPeriodoRevision').val());
            $('#cbPeriodoPadre').val($('#hfPeriodoPadre').val());
            $('#cbEstado').val($('#hfEstado').val());

            if (id == 0)
                validarTipoPeriodo($('#hfTipoPeriodo').val());

            $('#cbTipoPeriodo').on("change", function () {
                validarTipoPeriodo($('#cbTipoPeriodo').val());
            });
            
            cargarEtapas(id);
        },
        error: function () {
            mostrarMensaje('mensaje', 'error', 'Se ha producido un error.');
        }
    });
}

function validarTipoPeriodo(id) {
    if (id == "T" || id == "") {
        $('#cbPeriodoRevision').attr('disabled', true);
        $('#cbPeriodoRevision').val("");
    }
    else if(id == "S") {
        $('#cbPeriodoRevision').prop('disabled', false);
        $('#cbPeriodoRevision').val("N");
    }
}

function cargarPadres(anio) {
    $.ajax({
        type: 'POST',
        url: controlador + 'obtenerpadres',
        data: {
            anio: anio
        },
        dataType: 'json',        
        success: function (result) {
            if (result != -1) {
                $('#cbPeriodoPadre').get(0).options.length = 0;
                $('#cbPeriodoPadre').get(0).options[0] = new Option("--SELECCIONE--", "");
                $.each(result, function (i, item) {
                    $('#cbPeriodoPadre').get(0).options[$('#cbPeriodoPadre').get(0).options.length] = new Option(item.Repernombre, item.Repercodi);
                });
            }
            else {
                mostrarMensaje('mensajeEdicion', 'error', 'Se ha producido un error.');
            }
        },
        error: function () {
            mostrarMensaje('mensajeEdicion', 'error', 'Se ha producido un error.');
        }
    });
}

function cargarEtapas(id) {
    $.ajax({
        type: 'POST',
        url: controlador + 'etapas',
        data: {
            id: id
        },
        global:false,
        dataType: 'json',       
        success: function (result) {
            cargarGrillaEtapa(result);
        },
        error: function () {
            mostrarMensaje('mensajeEdicion', 'error', 'Se ha producido un error.');
        }
    });
}

function grabarRegistro() {
    var validacion = validarRegistro();
    if (validacion == "") {
       
        $.ajax({
            type: 'POST',
            url: controlador + 'grabar',
            data: $('#frmRegistro').serialize(),
            dataType: 'json',          
            success: function (result) {
                if (result == 1) {                   
                    $('#popupEdicion').bPopup().close();
                    consultar();
                    mostrarMensaje('mensaje', 'exito', 'Los datos se grabaron correctamente.');
                }
                else if (result == 2)
                {
                    mostrarMensaje('mensajeEdicion', 'alert', 'No puede registrar periodos con nombre duplicado.');
                }
                else {
                    mostrarMensaje('mensajeEdicion', 'error', 'Se ha producido un error.');
                }
            },
            error: function () {
                mostrarMensaje('mensajeEdicion', 'error', 'Se ha producido un error.');
            }
        });
    }
    else {
        mostrarMensaje('mensajeEdicion', 'alert', validacion);
    }
}

function eliminar(id) {
    if (confirm('¿Está seguro de realizar esta operación?')) {

        $.ajax({
            type: 'POST',
            url: controlador + 'eliminar',
            data: {
                id: id
            },
            dataType: 'json',
            success: function (result) {
                if (result == 1) {
                    mostrarMensaje('mensaje', 'exito', 'El registro se eliminó correctamente.');
                    consultar();
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
}

function validarRegistro() {
    var html = "<ul>";
    var flag = true;

    if ($('#txtAnio').val() == "") {
        html = html + "<li>Seleccione año.</li>";
        flag = false;
    }

    if ($('#cbTipoPeriodo').val() == "") {
        html = html + "<li>Seleccione tipo de periodo.</li>";
        flag = false;
    }
    else {
        if ($('#cbTipoPeriodo').val() == "S") {
            if ($('#cbPeriodoRevision').val() == "") {
                html = html + "<li>Indicar si el periodo es de revisión.</li>";
                flag = false;
            }
        }
    }

    if ($('#txtFechaInicio').val() == "" || $('#txtFechaFin').val() == "") {
        html = html + "<li>Seleccione fecha de inicio y fin</li>";
        flag = false;
    }
    else {
        if (getFecha($('#txtFechaInicio').val()) > getFecha($('#txtFechaFin').val())) {
            html = html + "<li>La fecha de inicio de no puede ser mayor a la fecha final.</li>";
            flag = false;
        }
    }

    if ($('#txtNombre').val() == "") {
        html = html + "<li>Ingrese nombre del periodo.</li>";
        flag = false;
    }

    if ($('#cbEstado').val() == "") {
        html = html + "<li>Seleccione estado.</li>";
        flag = false;
    }

    if ($('#txtOrdenAnual').val() == "") {
        html = html + "<li>Ingrese el orden anual.</li>";
        flag = false;
    }
    else {
        if (!validarEntero($('#txtOrdenAnual').val())) {
            html = html + "<li>El orden anual debe ser un número entero.</li>";
            flag = false;
        }
    }

    if ($('#txtTipoCambio').val() == "") {
        html = html + "<li>Ingrese el tipo de cambo.</li>";
        flag = false;
    }
    else {
        if (!validarNumero($('#txtTipoCambio').val())) {
            html = html + "<li>El tipo de cambio debe ser un valor numérico.</li>";
            flag = false;
        }
    }

    if ($('#txtFactorCompensacion').val() == "") {
        html = html + "<li>Ingrese el factor de compensación.</li>";
        flag = false;
    }
    else {
        if (!validarNumero($('#txtFactorCompensacion').val())) {
            html = html + "<li>El factor de compensación debe ser un valor numérico.</li>";
            flag = false;
        }
    }

    if ($('#cbTipoPeriodo').val() == "T" || ($('#cbTipoPeriodo').val() == "S" && $('#cbPeriodoRevision').val() == "S")) {
        if ($('#cbPeriodoPadre').val() == "") {
            html = html + "<li>Debe seleccionar un periodo padre.</li>";
            flag = false;
        }
    }

    var data = hot.getData();

    if ($('#cbTipoPeriodo').val() != "" && $('#txtFechaInicio').val() != "" || $('#txtFechaFin').val() != "") {

        if ($('#cbTipoPeriodo').val() == "S") {
            for (i = 0; i < data.length; i++) {

                if (data[i][2] == "") {
                    html = html + "<li>Ingrese fecha final para la etapa " + data[i][0] + "</li>";
                    flag = false;
                }
                else {
                    if (!validarFecha(data[i][2])) {
                        html = html + "<li>La fecha de la etapa " + data[i][0] + " no tiene formato correcto.</li>";
                        flag = false;
                    }
                    else {
                        if (getFecha($('#txtFechaInicio').val()) >= getFecha(data[i][2])) {
                            html = html + "<li>La fecha de la etapa " + data[i][0]  + " debe ser mayor a la fecha de inicio del periodo.</li>";
                            flag = false;
                        }
                    }
                }
            }
        }

        if ($('#cbTipoPeriodo').val() == "T") {
            for (i = 0; i < data.length; i++) {

                if (data[i][2] == "") {
                    if (parseInt(data[i][0]) < 4) {
                        html = html + "<li>Ingrese fecha final para la etapa " + data[i][0] + "</li>";
                        flag = false;
                    }
                }
                else {
                    if (!validarFecha(data[i][2])) {
                        html = html + "<li>La fecha de la etapa " + data[i][0] + " no tiene formato correcto.</li>";
                        flag = false;
                    }
                    else {
                       
                        if (getFecha($('#txtFechaInicio').val()) >= getFecha(data[i][2])) {
                            html = html + "<li>La fecha de la etapa " + data[i][0]  + " debe ser mayor a la fecha de inicio del periodo.</li>";
                            flag = false;
                        }
                    }
                }
            }
        }
    }

    html = html + "</ul>"

    if (flag) {
        html = "";
        var periodos = "";
        for (i = 0; i < data.length; i++) {
            periodos = periodos + data[i][0] + "@" + data[i][2] + "-";
        }
        $('#hfDataEtapa').val(periodos);
    }
    return html;
}

function cargarGrillaEtapa(result) {
    if (hot != null) {
        hot.destroy();
    }
    var container = document.getElementById('divEtapas');

    var disabledRenderer = function (instance, td, row, col, prop, value, cellProperties) {
        Handsontable.renderers.TextRenderer.apply(this, arguments);
        td.style.backgroundColor = '#F2F2F2';
        td.style.fontSize = '11px';
        td.style.verticalAlign = 'middle';
    };

    var estadoRenderer = function (instance, td, row, col, prop, value, cellProperties) {
        Handsontable.renderers.TextRenderer.apply(this, arguments);
        td.style.fontSize = '11px';
        td.style.verticalAlign = 'middle';
        td.style.textAlign = 'center';
        td.style.color = '#ffffff';
        td.style.backgroundColor = '#00CC66';
        if (value == "Culminado")
            td.style.backgroundColor = '#FF3737';
    };

    hot = new Handsontable(container, {
        data: result,
        colHeaders: ['N°', 'Etapa', 'Fecha final', 'Registro', 'Estado'],
        columns: [
            { },
            { },
            {
                type: 'date',
                dateFormat: "DD/MM/YYYY",
                correctFormat: true
            },
            {},           
            {}
        ],
        
        comments: true,        
        colWidths: [40, 530, 120, 120, 120],
        maxRows: result.length,
        cells: function (row, col, prop) {
            var cellProperties = {};

            if (col != 2) {
                cellProperties.renderer = disabledRenderer;
                cellProperties.readOnly = true;
            }
            if (col == 4) {
                cellProperties.renderer = estadoRenderer;
                cellProperties.readOnly = true;
            }
            

            return cellProperties;
        },
        beforeChange: function (changes, source) {
            for (var i = changes.length - 1; i >= 0; i--) {
                var fila = changes[i][0];
                var columna = changes[i][1];

                if (changes[i][2] != changes[i][3] && columna == 2) {
                    var fecha = getFecha(changes[i][3]);
                    var fechaActual = getFecha($('#hfFechaActual').val());                    
                    var valor = "Proceso";
                    if (fechaActual > fecha) {
                        valor = "Culminado";
                    }
                    hot.setDataAtCell(fila, 4, valor);
                }
            }
        }
    });
}

function habilitarCarga(idPeriodo) {
    var url = siteRoot + 'CalculoResarcimiento/Periodo/HabilitacionCarga?id=' + idPeriodo;

    window.open(url, "_self").focus();
}

function confirmarActualizacion(idPeriodo) {
    $.ajax({
        type: 'POST',
        url: controlador + 'ObtenerPeriodosTrimestrales',
        data: {
            idPeriodo: idPeriodo
        },
        dataType: 'json',
        global: false,
        success: function (result) {
            if (result != -1) {
                $('#cbPeriodoTrimestral').get(0).options.length = 0;
                $('#cbPeriodoTrimestral').get(0).options[0] = new Option("--SELECCIONE--", "");
                $.each(result, function (i, item) {
                    $('#cbPeriodoTrimestral').get(0).options[$('#cbPeriodoTrimestral').get(0).options.length] = new Option(item.Repernombre, item.Repercodi);
                });
                $('#hfIdPeriodoSeleccion').val(idPeriodo);

                setTimeout(function () {
                    $('#popupCopiar').bPopup({
                        autoClose: false,
                        modalClose: false,
                        escClose: false,
                        follow: [false, false]
                    });
                }, 50);

                $('#mensaje').html("");
                $('#mensaje').removeClass();
                mostrarMensaje('mensajeCopiar', 'message', 'Seleccione los datos solicitados.');

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


function copiarDesdeTrimestral() {
    if ($('#cbPeriodoTrimestral').val() != "") {
        $.ajax({
            type: 'POST',
            url: controlador + 'CopiarInterrupciones',
            data: {
                idPeriodo: $('#hfIdPeriodoSeleccion').val(),
                idPeriodoTrimestral: $('#cbPeriodoTrimestral').val()
            },
            dataType: 'json',
            global: false,
            success: function (result) {
                if (result == 1) {
                    $('#popupCopiar').bPopup().close();
                    mostrarMensaje('mensaje', 'exito', 'La operación se realizó correctamente.');
                }
                else {
                    mostrarMensaje('mensajeCopiar', 'error', 'Se ha producido un error.');
                }
            },
            error: function () {
                mostrarMensaje('mensajeCopiar', 'error', 'Se ha producido un error.');
            }
        });
    }
    else {
        mostrarMensaje('mensajeCopiar', 'alert', 'Seleccione un periodo trimestral.');
    }
}