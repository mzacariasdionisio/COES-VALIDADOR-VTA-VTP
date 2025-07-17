// Constantes generales para el flujo de datos del modulo
const controller = siteRoot + "PrimasRER/ProcesarCalculo/";
const imageRoot = siteRoot + "Content/Images/";

$(document).ready(function () {

    $('#tab-container').easytabs({
        animate: false
    });

    $('#btnProcesar').click(function () {
        ejecutarProcesarCalculo();
    });

    $('#btnBorrar').click(function () {
        ejecutarBorrarCalculo();
    });

    $('#btnExportarIngresoPotencia').click(function () {
        ejecutarExportar('IngresoPotencia');
    });

    $('#btnExportarIngresoEnergia').click(function () {
        ejecutarExportar('IngresoEnergia');
    });

    $('#btnExportarEnergiaNeta').click(function () {
        ejecutarExportar('EnergiaNeta');
    });

    $('#btnExportarCostoMarginal').click(function () {
        ejecutarExportar('CostoMarginal');
    });

    $('#btnExportarFactorAjuste').click(function () {
        ejecutarExportar('FactorAjuste');
    });

    $('#btnExportarIngresoPrimaRer').click(function () {
        ejecutarExportar('IngresoPrimaRer');
    });

    $('#btnExportarSaldosVteaResumen').click(function () {
        ejecutarExportar('SaldosVteaResumen');
    });
    $('#btnExportarSaldosVtea1Trimestre').click(function () {
        ejecutarExportar('SaldosVtea1Trimestre');
    });
    $('#btnExportarSaldosVtea2Trimestre').click(function () {
        ejecutarExportar('SaldosVtea2Trimestre');
    });
    $('#btnExportarSaldosVtea3Trimestre').click(function () {
        ejecutarExportar('SaldosVtea3Trimestre');
    });
    $('#btnExportarSaldosVtea4Trimestre').click(function () {
        ejecutarExportar('SaldosVtea4Trimestre');
    });

    $('#btnExportarSaldosVtp').click(function () {
        ejecutarExportar('SaldosVtp');
    });

    $('#btnTarifaAdjudicada').click(function () {
        ejecutarExportar('TarifaAdjudicada');
    });

    $('#btnExportarSaldoMensualCompensar').click(function () {
        ejecutarExportar('SaldoMensualCompensar');
    });
});

function ejecutarProcesarCalculo()
{
    let data = validateAnioTarifarioAndVersion('#cbAnioTarifario1', '#cbVersion1');
    if (data.Error) {
        alert(data.ErrorMessage);
        return false;
    }

    if (!confirm("¿Está seguro que desea PROCESAR el cálculo para la versión del Año Tarifario seleccionada?")) {
        return;
    }

    procesarCalculo(data);
}

function procesarCalculo(data) {
    $.ajax({
        type: 'POST',
        url: controller + 'ProcesarCalculo',
        contentType: 'application/json; charset=utf-8',
        data: JSON.stringify({
            anio: data.AnioTarifario,
            version: data.NumeroVersion
        }),
        datatype: 'json',
        traditional: true,
        success: function (result) {
            if (result.Resultado === "-1") {
                alert("Ha ocurrio un error: " + result.Mensaje);
            } else {
                alert(result.Mensaje);
                $('#cbAnioTarifario2').val(data.AnioTarifario);
                $('#cbVersion2').val(data.NumeroVersion);
            }
        },
        error: function () {
            alert("Ha ocurrido un problema.");
        }
    });
}

function ejecutarBorrarCalculo() {
    let data = validateAnioTarifarioAndVersion('#cbAnioTarifario1', '#cbVersion1');
    if (data.Error) {
        alert(data.ErrorMessage);
        return false;
    }

    if (!confirm("¿Está seguro que desea ELIMINAR el cálculo para la versión del Año Tarifario seleccionada?")) {
        return;
    }

    borrarCalculo(data);
}

function borrarCalculo(data) {
    $.ajax({
        type: 'POST',
        url: controller + 'BorrarCalculo',
        contentType: 'application/json; charset=utf-8',
        data: JSON.stringify({
            anio: data.AnioTarifario, 
            version: data.NumeroVersion
        }),
        datatype: 'json',
        traditional: true,
        success: function (result) {
            if (result.Resultado === "-1") {
                alert("Ha ocurrio un error: " + result.Mensaje);
            } else {
                alert(result.Mensaje);
            }
        },
        error: function () {
            alert("Ha ocurrido un problema.");
        }
    });
}

function ejecutarExportar(tipo) {
    let data = validateAnioTarifarioAndVersion('#cbAnioTarifario2', '#cbVersion2');
    if (data.Error) {
        alert(data.ErrorMessage);
        return false;
    }

    switch (tipo) {
        case 'IngresoPotencia':
            exportarReporte(data, 1);
            break;
        case 'IngresoEnergia':
            exportarReporte(data, 2);
            break;
        case 'EnergiaNeta':
            exportarReporte(data, 3);
            break;
        case 'CostoMarginal':
            exportarReporte(data, 4);
            break;
        case 'FactorAjuste':
            exportarReporte(data, 5);
            break;
        case 'IngresoPrimaRer':
            exportarReporte(data, 6);
            break;
        case 'SaldosVteaResumen':
            exportarReporte(data, 7);
            break;
        case 'SaldosVtea1Trimestre':
            exportarReporte(data, 8);
            break;
        case 'SaldosVtea2Trimestre':
            exportarReporte(data, 9);
            break;
        case 'SaldosVtea3Trimestre':
            exportarReporte(data, 10);
            break;
        case 'SaldosVtea4Trimestre':
            exportarReporte(data, 11);
            break;
        case 'SaldosVtp':
            exportarReporte(data, 12);
            break;
        case 'TarifaAdjudicada':
            exportarReporte(data, 13);
            break;
        case 'SaldoMensualCompensar':
            exportarReporte(data, 14);
            break;
    }
}

function exportarReporte(data, tipoReporte) {
    $.ajax({
        type: 'POST',
        url: controller + 'ExportarReporte',
        contentType: 'application/json; charset=utf-8',
        data: JSON.stringify({
            anio: data.AnioTarifario,
            version: data.NumeroVersion,
            tipoReporte: tipoReporte
        }),
        datatype: 'json',
        traditional: true,
        success: function (result) {
            if (result.Resultado === "-1") {
                alert(result.Mensaje);
            } else {
                window.location = controller + 'abrirarchivo?tipo=' + 1 + '&nombreArchivo=' + result.Resultado;
            }
        },
        error: function () {
            alert("Ha ocurrido un problema...");
        }
    });
}
