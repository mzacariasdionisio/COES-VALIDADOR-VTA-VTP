const controller = siteRoot + "CPPA/CalcularPorcentaje/";
const imageRoot = siteRoot + "Content/Images/";
const errorMessage = "Ha ocurrido un error.";
let listRevision;

$(document).ready(function () {

    $('#tab-container').easytabs({
        animate: false
    });

    $('#cbAnio').change(function () {
        cargarAjustes($(this).val());
    });

    $('#cbAjuste').change(function () {
        cargarRevisiones($('#cbAnio').val(), $(this).val());
    });

    $('#cbRevision').change(function () {
        obtenerLogProceso($('#cbRevision').val());
    });

    $('#btnProcesar').click(function () {
        procesarCalculo($('#cbAnio').val(), $('#cbRevision').val());
    });

    $('#btnBorrar').click(function () {
        eliminarCalculo($('#cbAnio').val(), $('#cbRevision').val());
    });

    $('#btnPublicar').click(function () {
        publicarReportes($('#cbAnio').val(), $('#cbRevision').val());
    });

    $('#btnDespublicar').click(function () {
        despublicarReportes($('#cbAnio').val(), $('#cbRevision').val());
    });

    limpiarLogYTablaReporte();
    obtenerDatosInciales();
});

function obtenerDatosInciales() {
    $.ajax({
        type: 'POST',
        url: controller + "ObtenerDatosIniciales",
        data: { },
        success: function (model) {
            if (model.sResultado == "-1") {
                alert(model.sMensaje);
            } else {
                cargarAnios(model);
            }
        },
        error: function () {
            alert(errorMessage);
        }
    });
}

function cargarAnios(model) {
    limpiarLogYTablaReporte();
    if (model.ListaAnio.length > 0) {
        $('#cbAnio').prop('disabled', false).empty().append('<option value="">--Seleccione--</option>');
        $('#cbAjuste').prop('disabled', true).empty().append('<option value="">--Seleccione--</option>');
        $('#cbRevision').prop('disabled', true).empty().append('<option value="">--Seleccione--</option>');

        model.ListaAnio
            .forEach((anio) => { $('#cbAnio').append('<option value="' + anio.Entero1 + '">' + anio.String1 + '</option>'); });
    }
    else {
        $('#cbAnio').prop('disabled', true).empty().append('<option value="">--Seleccione--</option>');
        $('#cbAjuste').prop('disabled', true).empty().append('<option value="">--Seleccione--</option>');
        $('#cbRevision').prop('disabled', true).empty().append('<option value="">--Seleccione--</option>');
    }

    if (model.ListRevision.length > 0) {
        listRevision = model.ListRevision;
    }
}

function cargarAjustes(year) {
    limpiarLogYTablaReporte();
    if (year) {
        $('#cbAjuste').prop('disabled', false).empty().append('<option value="">--Seleccione--</option>');
        $('#cbRevision').prop('disabled', true).empty().append('<option value="">--Seleccione--</option>');

        listRevision
            .filter(x => x.Cpaapanio === parseInt(year))
            .map(x => x.Cpaapajuste)
            .filter((value, index, self) => self.indexOf(value) === index) // Eliminar duplicados
            .forEach((cpaapajuste) => { $('#cbAjuste').append('<option value="' + cpaapajuste + '">' + cpaapajuste + '</option>'); });
    }
    else {
        $('#cbAjuste').prop('disabled', true).empty().append('<option value="">--Seleccione--</option>');
        $('#cbRevision').prop('disabled', true).empty().append('<option value="">--Seleccione--</option>');
    }
}

function cargarRevisiones(year, fit) {
    limpiarLogYTablaReporte();
    if (year && fit) {
        $('#cbRevision').prop('disabled', false).empty().append('<option value="">--Seleccione--</option>');

        listRevision
            .filter(x => x.Cpaapanio === parseInt(year) && x.Cpaapajuste === fit)
            .forEach((revision) => {
                let estado = revision.Cparestado == 'A' ? '' : ' [' + revision.Cparestado + ']';
                $('#cbRevision').append('<option value="' + revision.Cparcodi + '">' + revision.Cparrevision + estado + '</option>');
            });
    }
    else {
        $('#cbRevision').prop('disabled', true).empty().append('<option value="">--Seleccione--</option>');
    }
}

function obtenerLogProceso(cparcodi) {
    limpiarLogYTablaReporte();
    if (!cparcodi) {
        return;
    }

    $.ajax({
        type: 'POST',
        url: controller + "ObtenerLogProceso",
        data: {
            cparcodi: cparcodi,
        },
        success: function (model) {
            if (model.sResultado == "-1") {
                alert(model.sMensaje);
            } else {
                llenarLogYTablaReporte(model, cparcodi);
            }
        },
        error: function () {
            alert(errorMessage);
        }
    });
}

function procesarCalculo(year, cparcodi) {
    if (!year || !cparcodi) {
        alert("Debe seleccionar una Revisión de un Ajuste Presupuestal.");
        return;
    }

    let rpta1 = confirm("¿Está seguro de que desea PROCESAR el cálculo para la Revisión del Ajuste del Año Presupuestal seleccionada?");
    if (!rpta1) {
        return;
    }

    if ($('#logProceso').val().length > 0) {
        let rpta2 = confirm("Ya existe un cálculo previo para la Revisión del Ajuste del Año Presupuestal seleccionada ¿Está seguro de que desea PROCESAR el cálculo nuevamente para la Revisión del Ajuste del Año Presupuestal seleccionada?");
        if (!rpta2) {
            return;
        }
    }

    $.ajax({
        type: 'POST',
        url: controller + "ProcesarCalculo",
        data: {
            cparcodi: cparcodi,
        },
        success: function (model) {
            if (model.sResultado == "-1") {
                alert(model.sMensaje);
            } else {
                alert(model.sMensaje);
                llenarLogYTablaReporte(model, cparcodi);
            }
        },
        error: function () {
            alert(errorMessage);
        }
    });
}

function eliminarCalculo(year, cparcodi) {
    if (!year || !cparcodi) {
        alert("Debe seleccionar una Revisión de un Ajuste Presupuestal.");
        return;
    }

    let rpta = confirm("¿Está seguro de que desea ELIMINAR el cálculo para la Revisión del Ajuste del Año Presupuestal seleccionada?");
    if (!rpta) {
        return;
    }

    $.ajax({
        type: 'POST',
        url: controller + "EliminarCalculo",
        data: {
            cparcodi: cparcodi,
        },
        success: function (model) {
            if (model.sResultado == "-1") {
                alert(model.sMensaje);
            } else {
                alert(model.sMensaje);
                llenarLogYTablaReporte(model, cparcodi);
            }
        },
        error: function () {
            alert(errorMessage);
        }
    });
}

function exportarReporteGeneracion(cparcodi, cpacemes) {
    $.ajax({
        type: 'POST',
        url: controller + 'ExportarReporteGeneracion',
        contentType: 'application/json; charset=utf-8',
        data: JSON.stringify({
            cparcodi: cparcodi,
            cpacemes: cpacemes
        }),
        datatype: 'json',
        traditional: true,
        success: function (model) {
            if (model.sResultado === "-1") {
                alert(model.sMensaje);
            } else {
                window.location = controller + 'abrirarchivo?tipo=' + 1 + '&nombreArchivo=' + model.sResultado;
            }
        },
        error: function () {
            alert(errorMessage);
        }
    });
}

function exportarReporteDemanda(cparcodi, cpatdmes) {
    $.ajax({
        type: 'POST',
        url: controller + 'ExportarReporteDemanda',
        contentType: 'application/json; charset=utf-8',
        data: JSON.stringify({
            cparcodi: cparcodi,
            cpatdmes: cpatdmes
        }),
        datatype: 'json',
        traditional: true,
        success: function (model) {
            if (model.sResultado === "-1") {
                alert(model.sMensaje);
            } else {
                window.location = controller + 'abrirarchivo?tipo=' + 1 + '&nombreArchivo=' + model.sResultado;
            }
        },
        error: function () {
            alert(errorMessage);
        }
    });
}

function exportarReporteTransmision(cparcodi) {
    $.ajax({
        type: 'POST',
        url: controller + 'ExportarReporteTransmision',
        contentType: 'application/json; charset=utf-8',
        data: JSON.stringify({
            cparcodi: cparcodi
        }),
        datatype: 'json',
        traditional: true,
        success: function (model) {
            if (model.sResultado === "-1") {
                alert(model.sMensaje);
            } else {
                window.location = controller + 'abrirarchivo?tipo=' + 1 + '&nombreArchivo=' + model.sResultado;
            }
        },
        error: function () {
            alert(errorMessage);
        }
    });
}

function exportarReportePorcentaje(cparcodi) {
    $.ajax({
        type: 'POST',
        url: controller + 'ExportarReportePorcentaje',
        contentType: 'application/json; charset=utf-8',
        data: JSON.stringify({
            cparcodi: cparcodi
        }),
        datatype: 'json',
        traditional: true,
        success: function (model) {
            if (model.sResultado === "-1") {
                alert(model.sMensaje);
            } else {
                window.location = controller + 'abrirarchivo?tipo=' + 1 + '&nombreArchivo=' + model.sResultado;
            }
        },
        error: function () {
            alert(errorMessage);
        }
    });
}

function publicarReportes(year, cparcodi) {
    if (!year || !cparcodi) {
        alert("Debe seleccionar una Revisión de un Ajuste Presupuestal.");
        return;
    }

    let rpta = confirm("¿Está seguro de que desea publicar todos los reportes de resultados relacionados a la Revisión del Ajuste Presupuestal seleccionada en la Extranet?");
    if (!rpta) {
        return;
    }

    $.ajax({
        type: 'POST',
        url: controller + 'PublicarReportes',
        contentType: 'application/json; charset=utf-8',
        data: JSON.stringify({
            cparcodi: cparcodi
        }),
        datatype: 'json',
        traditional: true,
        success: function (model) {
            if (model.sResultado === "-1") {
                alert(model.sMensaje);
            } else {
                visualizarBotones(model);
                alert(model.sMensaje);
            }
        },
        error: function () {
            alert(errorMessage);
        }
    });
}

function despublicarReportes(year, cparcodi) {
    if (!year || !cparcodi) {
        alert("Debe seleccionar una Revisión de un Ajuste Presupuestal.");
        return;
    }

    let rpta = confirm("¿Está seguro de que desea despublicar todos los reportes de resultados relacionados a la Revisión del Ajuste Presupuestal seleccionada de la Extranet?");
    if (!rpta) {
        return;
    }

    $.ajax({
        type: 'POST',
        url: controller + 'DespublicarReportes',
        contentType: 'application/json; charset=utf-8',
        data: JSON.stringify({
            cparcodi: cparcodi
        }),
        datatype: 'json',
        traditional: true,
        success: function (model) {
            if (model.sResultado === "-1") {
                alert(model.sMensaje);
            } else {
                visualizarBotones(model);
                alert(model.sMensaje);
            }
        },
        error: function () {
            alert(errorMessage);
        }
    });
}

function visualizarBotones(model) {
    $('#btnPublicar').hide();
    $('#btnDespublicar').hide();
    if (model.EstadoPublicacion == "N") {
        $('#btnPublicar').show();
        $('#btnDespublicar').hide();
    }
    if (model.EstadoPublicacion == "S") {
        $('#btnPublicar').hide();
        $('#btnDespublicar').show();
    }
}

function llenarLogYTablaReporte(model, cparcodi) {
    $('#btnProcesar').show();
    $('#btnBorrar').show();
    $('#logProceso').val(model.sDetalle);
    if (model.EstadoPublicacion == "N") {
        $('#btnPublicar').show();
        $('#btnDespublicar').hide();
        $('#mensajeReporte').html("");
    }
    if (model.EstadoPublicacion == "S") {
        $('#btnPublicar').hide();
        $('#btnDespublicar').show();
        $('#mensajeReporte').html("");
    }
    if (model.EstadoPublicacion == "") {
        $('#btnPublicar').hide();
        $('#btnDespublicar').hide();
        $('#mensajeReporte').html("No se tienen resultados para la Revisión del Ajuste del Año presupuestal seleccionada.");
    }

    let html = generarTablaMensual(model.ListaReporte);
    html += "<br/><br/><br/>";
    html += generarTablaAnual(cparcodi, model.ListaReporte2);

    $('#reporteCPPA').html(html);
}

function generarTablaMensual(listaReporte) {
    let html = `<table width='100%' border='1' cellpadding='5' cellspacing='2'>
                <tr class='THCabecera'>
                <td><div class='editor-label' style='color: #FFFFFF; text-align: center;'><b>Reporte Mes</b></div></td>
                <td><div class='editor-label' style='color: #FFFFFF; text-align: center;'><b>Generación</b></div></td>
                <td><div class='editor-label' style='color: #FFFFFF; text-align: center;'><b>Descargar</b></div></td>
                <td><div class='editor-label' style='color: #FFFFFF; text-align: center;'><b>Demanda (UL y D)</b></div></td>
                <td><div class='editor-label' style='color: #FFFFFF; text-align: center;'><b>Descargar</b></div></td>
                </tr>`;

    if (listaReporte.length > 0) {
        listaReporte.forEach((item) => {
            html += `
                <tr>
                    <td>${item.String1}</td>
                    <td>${item.String2}</td>
                    <td style='text-align: center;'><a href='javascript:exportarReporteGeneracion(${item.Entero1}, ${item.Entero2})' title='Descargar archivo ${item.String2}'><img src='../../../Areas/TransfPotencia/Content/Images/excel.gif' /></a></td>
                    <td>${item.String3}</td>
                    <td style='text-align: center;'><a href='javascript:exportarReporteDemanda(${item.Entero1}, ${item.Entero2})' title='Descargar archivo ${item.String3}'><img src='../../../Areas/TransfPotencia/Content/Images/excel.gif' /></a></td>
                </tr>`;
        });
    } else {
        html += "<tr><td colspan='5' style='text-align: center;'>No hay registros</td></tr>";
    }

    html += "</table>";
    return html;
}

function generarTablaAnual(cparcodi, listaReporte2) {
    let html = `<table width='100%' border='1' cellpadding='5' cellspacing='2'>
                <tr class='THCabecera'>
                <td><div class='editor-label' style='color: #FFFFFF; text-align: center;'><b>Reporte Anual</b></div></td>
                <td><div class='editor-label' style='color: #FFFFFF; text-align: center;'><b>Archivo</b></div></td>
                <td><div class='editor-label' style='color: #FFFFFF; text-align: center;'><b>Descargar</b></div></td>
                </tr>`;

    if (listaReporte2.length > 0) {
        listaReporte2.forEach((item, index) => {
            html += `
                <tr>
                    <td>${item.String1}</td>
                    <td>${item.String2}</td>
                    <td style='text-align: center;'>
                        <a href='javascript:${index === 0 ? `exportarReporteTransmision(${cparcodi})` : `exportarReportePorcentaje(${cparcodi})`}' title='${index === 0 ? "Descargar archivo Reporte Transmisión" : "Descargar archivo Reporte Porcentaje Presupuestal"}'>
                            <img src='../../../Areas/TransfPotencia/Content/Images/excel.gif' />
                        </a>
                    </td>
                </tr>`;
        });
    } else {
        html += "<tr><td colspan='3' style='text-align: center;'>No hay registros</td></tr>";
    }

    html += "</table>";
    return html;
}

function limpiarLogYTablaReporte() {
    $('#btnProcesar').hide();
    $('#btnBorrar').hide();
    $('#logProceso').val("");
    $('#btnPublicar').hide();
    $('#btnDespublicar').hide();
    $('#mensajeReporte').html("");
    $('#reporteCPPA').html("");
}