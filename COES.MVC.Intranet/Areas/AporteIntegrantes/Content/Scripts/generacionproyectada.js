var sControlador = siteRoot + "aporteintegrantes/generacionproyectada/";
var error = [];

$(document).ready(function () {

    $('#btnDescargarExcelGP').click(function () {
        mostrarAlerta("Espere un momento, se esta procesando su solicitud");
        descargarArchivoGP(1);
    });

    $('#btnSelecionarExcelDur').click(function () {
        mostrarAlerta("Espere un momento, se esta procesando su solicitud");
        ProcesarArchivoDuracion();
    });

    $('#btnSelecionarExcelRen').click(function () {
        mostrarAlerta("Espere un momento, se esta procesando su solicitud");
        ProcesarArchivoRenovables();
    });

    $('#btnSelecionarExcelTer').click(function () {
        mostrarAlerta("Espere un momento, se esta procesando su solicitud");
        ProcesarArchivoTermicas();
    });

    $('#btnSelecionarExcelHid').click(function () {
        mostrarAlerta("Espere un momento, se esta procesando su solicitud");
        ProcesarArchivoHidraulicas();
    });

    $('#btnSelecionarExcelCmg').click(function () {
        mostrarAlerta("Espere un momento, se esta procesando su solicitud");
        ProcesarArchivoCostoMarginal();
    });

    $('#btnSelecionarExcelPar').click(function () {
        mostrarAlerta("Espere un momento, se esta procesando su solicitud");
        ProcesarArchivoParametros();
    });

    $('#btnProcesarCMG').click(function () {
        mostrarAlerta("Espere un momento, se esta procesando su solicitud");
        ProcesarCMG();
    });

    $('#btnProcesarEnergia').click(function () {
        mostrarAlerta("Espere un momento, se esta procesando su solicitud");
        ProcesarEnergia();
    });

    $('#btnImpResCMG').click(function () {
        mostrarAlerta("Espere un momento, se esta procesando su solicitud");
        ProcesarResCMG();
    });

    $('#btnImpResENG').click(function () {
        mostrarAlerta("Espere un momento, se esta procesando su solicitud");
        ProcesarResEnergia();
    });

    

});


ProcesarArchivoDuracion = function () {
    var caiajcodi = document.getElementById('caiajcodi').value;
    $.ajax({
        type: 'POST',
        url: sControlador + 'ProcesarArchivoDuracion',
        data: { caiajcodi: caiajcodi },
        dataType: 'json',
        cache: false,
        success: function (result) {
            if (result.sError == "")
                mostrarExito(result.sMensaje);
            else
                mostrarError("Lo sentimos no se puedo grabar la información ingresada " + result.sError);
        },
        error: function () {
            mostrarError('Lo sentimos no se puedo grabar la información ingresada');
        }
    });
}

ProcesarArchivoRenovables = function () {
    var caiajcodi = document.getElementById('caiajcodi').value;
    $.ajax({
        type: 'POST',
        url: sControlador + 'ProcesarArchivoRenovables',
        data: { caiajcodi: caiajcodi },
        dataType: 'json',
        cache: false,
        success: function (result) {
            if (result.sError == "")
                mostrarExito(result.sMensaje);
            else
                mostrarError("Lo sentimos no se puedo grabar la información ingresada " + result.sError);
        },
        error: function () {
            mostrarError('Lo sentimos no se puedo grabar la información ingresada');
        }
    });
}

ProcesarArchivoTermicas = function () {
    var caiajcodi = document.getElementById('caiajcodi').value;
    $.ajax({
        type: 'POST',
        url: sControlador + 'ProcesarArchivoTermicas',
        data: { caiajcodi: caiajcodi },
        dataType: 'json',
        cache: false,
        success: function (result) {
            if (result.sError == "")
                mostrarExito(result.sMensaje);
            else
                mostrarError("Lo sentimos no se puedo grabar la información ingresada " + result.sError);
        },
        error: function () {
            mostrarError('Lo sentimos no se puedo grabar la información ingresada');
        }
    });
}

ProcesarArchivoHidraulicas = function () {
    var caiajcodi = document.getElementById('caiajcodi').value;
    $.ajax({
        type: 'POST',
        url: sControlador + 'ProcesarArchivoHidraulicas',
        data: { caiajcodi: caiajcodi },
        dataType: 'json',
        cache: false,
        success: function (result) {
            if (result.sError == "")
                mostrarExito(result.sMensaje);
            else
                mostrarError("Lo sentimos no se puedo grabar la información ingresada " + result.sError);
        },
        error: function () {
            mostrarError('Lo sentimos no se puedo grabar la información ingresada');
        }
    });
}

ProcesarArchivoCostoMarginal = function () {
    var caiajcodi = document.getElementById('caiajcodi').value;
    $.ajax({
        type: 'POST',
        url: sControlador + 'ProcesarArchivoCostoMarginal',
        data: { caiajcodi: caiajcodi },
        dataType: 'json',
        cache: false,
        success: function (result) {
            if (result.sError == "")
                mostrarExito(result.sMensaje);
            else
                mostrarError("Lo sentimos no se puedo grabar la información ingresada " + result.sError);
        },
        error: function () {
            mostrarError('Lo sentimos no se puedo grabar la información ingresada');
        }
    });
}

ProcesarArchivoParametros = function () {
    var caiajcodi = document.getElementById('caiajcodi').value;
    $.ajax({
        type: 'POST',
        url: sControlador + 'ProcesarArchivoParametros',
        data: { caiajcodi: caiajcodi },
        dataType: 'json',
        cache: false,
        success: function (result) {
            if (result.sError == "")
                mostrarExito(result.sMensaje);
            else
                mostrarError("Lo sentimos no se puedo grabar la información ingresada " + result.sError);
        },
        error: function () {
            mostrarError('Lo sentimos no se puedo grabar la información ingresada');
        }
    });
}

ProcesarCMG = function () {
    var caiajcodi = document.getElementById('caiajcodi').value;
    $.ajax({
        type: 'POST',
        url: sControlador + 'ProcesarCalculoCMG',
        data: { caiajcodi: caiajcodi },
        dataType: 'json',
        cache: false,
        success: function (result) {
            if (result.sError == "")
                mostrarExito(result.sMensaje);
            else
                mostrarError("Lo sentimos no se pudo procesar CMG " + result.sError);
        },
        error: function () {
            mostrarError('Lo sentimos no se pudo grabar el proceso CMG');
        }
    });
}

ProcesarEnergia = function () {
    var caiajcodi = document.getElementById('caiajcodi').value;
    $.ajax({
        type: 'POST',
        url: sControlador + 'ProcesarCalculoEnergia',
        data: { caiajcodi: caiajcodi },
        dataType: 'json',
        cache: false,
        success: function (result) {
            if (result.sError == "")
                mostrarExito(result.sMensaje);
            else
                mostrarError("Lo sentimos no se pudo procesar ENERGIA " + result.sError);
        },
        error: function () {
            mostrarError('Lo sentimos no se pudo grabar el proceso ENERGIA');
        }
    });
}

ProcesarResCMG = function () {
    var caiajcodi = document.getElementById('caiajcodi').value;
    $.ajax({
        type: 'POST',
        url: sControlador + 'ProcesarResultadosCMG',
        data: { caiajcodi: caiajcodi },
        dataType: 'json',
        cache: false,
        success: function (result) {
            if (result.sError == "")
                mostrarExito(result.sMensaje);
            else
                mostrarError("Lo sentimos no se pudo procesar CMG " + result.sError);
        },
        error: function () {
            mostrarError('Lo sentimos no se pudo grabar el proceso Importar Resultados CMG');
        }
    });
}

ProcesarResEnergia = function () {
    var caiajcodi = document.getElementById('caiajcodi').value;
    $.ajax({
        type: 'POST',
        url: sControlador + 'ProcesarResultadosEnergia',
        data: { caiajcodi: caiajcodi },
        dataType: 'json',
        cache: false,
        success: function (result) {
            if (result.sError == "")
                mostrarExito(result.sMensaje);
            else
                mostrarError("Lo sentimos no se pudo procesar resultados de Energia " + result.sError);
        },
        error: function () {
            mostrarError('Lo sentimos no se pudo grabar el proceso Importar Resultados Energia');
        }
    });
}


refrescar = function () {
    var cmbPresupuesto = document.getElementById('caiprscodi');
    window.location.href = sControlador + "index?caiprscodi=" + cmbPresupuesto.value;
}


