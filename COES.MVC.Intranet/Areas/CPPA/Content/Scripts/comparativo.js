const controller = siteRoot + "CPPA/Comparativo/";
const imageRoot = siteRoot + "Content/Images/";
const errorMessage = "Ha ocurrido un error.";
let listRevision;

$(document).ready(function () {
    $('#cbBaseAnio').change(function () {
        cargarAjustes($(this).val(), '#cbBaseAjuste', '#cbBaseRevision');
    });

    $('#cbBaseAjuste').change(function () {
        cargarRevisiones($('#cbBaseAnio').val(), $(this).val(), '#cbBaseRevision');
    });

    $('#cbCompararAnio').change(function () {
        cargarAjustes($(this).val(), '#cbCompararAjuste', '#cbCompararRevision');
    });

    $('#cbCompararAjuste').change(function () {
        cargarRevisiones($('#cbCompararAnio').val(), $(this).val(), '#cbCompararRevision');
    });

    $('#btnDescargar').click(function () {
        descargarComparativo($('#cbBaseRevision').val(), $('#cbCompararRevision').val());
    });

    obtenerDatosInciales();
});

function obtenerDatosInciales() {
    $.ajax({
        type: 'POST',
        url: controller + "ObtenerDatosIniciales",
        data: {},
        success: function (model) {
            if (model.sResultado == "-1") {
                alert(model.sMensaje);
            } else {
                cargarAnios(model, '#cbBaseAnio', '#cbBaseAjuste', '#cbBaseRevision');
                cargarAnios(model, '#cbCompararAnio', '#cbCompararAjuste', '#cbCompararRevision');
            }
        },
        error: function () {
            alert(errorMessage);
        }
    });
}

function cargarAnios(model, idAnio, idAjuste, idRevision) {

    if (model.ListaAnio.length > 0) {
        $(idAnio).prop('disabled', false).empty().append('<option value="">--Seleccione--</option>');
        $(idAjuste).prop('disabled', true).empty().append('<option value="">--Seleccione--</option>');
        $(idRevision).prop('disabled', true).empty().append('<option value="">--Seleccione--</option>');

        model.ListaAnio
            .forEach((anio) => { $(idAnio).append('<option value="' + anio.Entero1 + '">' + anio.String1 + '</option>'); });
    }
    else {
        $(idAnio).prop('disabled', true).empty().append('<option value="">--Seleccione--</option>');
        $(idAjuste).prop('disabled', true).empty().append('<option value="">--Seleccione--</option>');
        $(idRevision).prop('disabled', true).empty().append('<option value="">--Seleccione--</option>');
    }

    if (model.ListRevision.length > 0) {
        listRevision = model.ListRevision;
    }
}

function cargarAjustes(year, idAjuste, idRevision) {

    if (year) {
        $(idAjuste).prop('disabled', false).empty().append('<option value="">--Seleccione--</option>');
        $(idRevision).prop('disabled', true).empty().append('<option value="">--Seleccione--</option>');

        listRevision
            .filter(x => x.Cpaapanio === parseInt(year))
            .map(x => x.Cpaapajuste)
            .filter((value, index, self) => self.indexOf(value) === index) // Eliminar duplicados
            .forEach((cpaapajuste) => { $(idAjuste).append('<option value="' + cpaapajuste + '">' + cpaapajuste + '</option>'); });
    }
    else {
        $(idAjuste).prop('disabled', true).empty().append('<option value="">--Seleccione--</option>');
        $(idRevision).prop('disabled', true).empty().append('<option value="">--Seleccione--</option>');
    }
}

function cargarRevisiones(year, fit, idRevision) {

    if (year && fit) {
        $(idRevision).prop('disabled', false).empty().append('<option value="">--Seleccione--</option>');

        listRevision
            .filter(x => x.Cpaapanio === parseInt(year) && x.Cpaapajuste === fit)
            .forEach((revision) => {
                let estado = revision.Cparestado == 'A' ? '' : ' [' + revision.Cparestado + ']';
                $(idRevision).append('<option value="' + revision.Cparcodi + '">' + revision.Cparrevision + estado + '</option>');
            });
    }
    else {
        $(idRevision).prop('disabled', true).empty().append('<option value="">--Seleccione--</option>');
    }
}

function descargarComparativo(cparcodiBase, cparcodiComparar) {

    if (!cparcodiBase) {
        alert("Debe seleccionar una Revisión ‘Base’ de un Ajuste Presupuestal.");
        return;
    }

    if (!cparcodiComparar) {
        alert("Debe seleccionar una Revisión ‘Comparar’ de un Ajuste Presupuestal.");
        return;
    }

    if (cparcodiBase == cparcodiComparar) {
        alert("Debe seleccionar Revisiones de Ajustes Presupuestales diferentes.");
        return;
    }

    $.ajax({
        type: 'POST',
        url: controller + 'ExportarReporteComparativo',
        contentType: 'application/json; charset=utf-8',
        data: JSON.stringify({
            cparcodiBase: cparcodiBase,
            cparcodiComparar: cparcodiComparar,
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
