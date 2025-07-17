const controller = siteRoot + "CPPA/CalcularTotales/";
const imageRoot = siteRoot + "Content/Images/";

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
        procesarCalculo($('#cbRevision').val());
    });

    $('#btnBorrar').click(function () {
        eliminarCalculo($('#cbRevision').val());
    });

});

function cargarAjustes(year) {
    limpiarLogYTablaReporte();
    if (year) {
        $('#cbAjuste').prop('disabled', false).empty().append('<option value="">--Seleccione--</option>');
        $('#cbRevision').prop('disabled', true).empty().append('<option value="">--Seleccione--</option>');

        listRevision
            .filter(x => x.Cpaapanio === parseInt(year))
            .map(x => x.Cpaapajuste)
            .filter((value, index, self) => self.indexOf(value) === index) // Eliminar duplicados
            .forEach((cpaapajuste) => { $('#cbAjuste').append('<option value="' + cpaapajuste + '">' + cpaapajuste + '</option>');});
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
                llenarLogYTablaReporte(model);
            }
        },
        error: function () {
            alert("Lo sentimos, ha ocurrido un error inesperado.");
        }
    });
}

function procesarCalculo(cparcodi) {
    if (!cparcodi) {
        alert("Debe seleccionar una Revisión de un Ajuste Presupuestal.");
        return;
    }

    let rpta = confirm("¿Está seguro que desea PROCESAR el cálculo para la Revisión del Ajuste del Año Presupuestal seleccionada?");
    if (!rpta) {
        return;
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
                llenarLogYTablaReporte(model);
            }
        },
        error: function () {
            alert("Lo sentimos, ha ocurrido un error inesperado.");
        }
    });
}

function eliminarCalculo(cparcodi) {
    if (!cparcodi) {
        alert("Debe seleccionar una Revisión de un Ajuste Presupuestal.");
        return;
    }

    let rpta = confirm("¿Está seguro que desea ELIMINAR el cálculo para la Revisión del Ajuste del Año Presupuestal seleccionada?");
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
                llenarLogYTablaReporte(model);
            }
        },
        error: function () {
            alert("Lo sentimos, ha ocurrido un error inesperado.");
        }
    });
}

function exportarReporteExcel(cparcodi, cpacemes)
{
    $.ajax({
        type: 'POST',
        url: controller + 'ExportarReporte',
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
            alert("Ha ocurrido un problema...");
        }
    });
}

function llenarLogYTablaReporte(model)
{
    $('#logProceso').val(model.sDetalle);
    let html = "<table border='1' cellpadding='5' cellspacing='2'><tr class='THCabecera'><td><div class='editor-label' style='color:#FFFFFF;'><b>Reporte Mes</b></div></td><td><div class='editor-label' style='color:#FFFFFF;'><b>Archivo Montos</b></div></td><td><div class='editor-label' style='color:#FFFFFF;'><b>Descarga</b></div></td></tr>";
    if (model.ListaReporte.length > 0) {
        let total = model.ListaReporte.length;
        let data = model.ListaReporte;
        for (let i = 0; i < total; i++) {
            html += "<tr>";
            html += "<td>" + data[i].String1 + "</td>";
            html += "<td>" + data[i].String2 + "</td>";
            html += "<td><a href='javascript: exportarReporteExcel(" + data[i].Entero1 + "," + + data[i].Entero2 + ")' title='Descargar archivo " + data[i].String2 + "'><img src='../../../Areas/TransfPotencia/Content/Images/excel.gif' /></a></td>";
            html += "</tr>";
        }
    }
    else {
        html += "<tr><td colspan='3' style='text-align: center;'>No hay registros</td></tr>"
    }
    html += "</table>";
    $('#reporteCPPA').html(html);
}

function limpiarLogYTablaReporte() {
    $('#logProceso').val("");
    let html = "<table border='1' cellpadding='5' cellspacing='2'><tr class='THCabecera'><td><div class='editor-label' style='color:#FFFFFF;'><b>Reporte Mes</b></div></td><td><div class='editor-label' style='color:#FFFFFF;'><b>Archivo Montos</b></div></td><td><div class='editor-label' style='color:#FFFFFF;'><b>Descarga</b></div></td></tr>";
    html += "</table>";
    $('#reporteCPPA').html(html);
}