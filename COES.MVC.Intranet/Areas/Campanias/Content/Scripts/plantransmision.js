var controlador = siteRoot + 'campanias/plantransmision/';
var idPlanTransmision = 0;
let isLoading = false;

$(function () {
    $('#btnConsultar').on('click', function () {
        consultar();
    });
    $('#btnExcel').on('click', function () {
        excel();
    });
    cargarDatos().then(() => {
        ObtenerListado();
    });
});

function cargarDatos() {
    return Promise.all([cargarPeriodo()])
        .then(cargarEmpresa);
}

function ObtenerListado() {

    $("#listado").html('');
    
    $.ajax({
        type: 'POST',
        url: controlador + 'Listado',
        data: {
            empresa: $('#empresaSelect').val(),
            estado: $('#cbEstado').val(),
            periodo: $('#periodoSelect').val() === null || $('#periodoSelect').val() === '' ? '0' : $('#periodoSelect').val(),
            vigente: $('#cbSelectEnviovigente').is(':checked') ? $('#cbSelectEnviovigente').val() : 'T',
            fueraplazo: $('#cbSelectFueraPlazo').is(':checked') ? $('#cbSelectFueraPlazo').val() : 'T',
            estadoExcl: 'Registrado',
        },
        success: function (eData) {
            $('#listado').css("width", $('.form-main').width() + "px");
            $('#listado').html(eData);
            $('#tabla').DataTable({
                bJQueryUI: true,
                "scrollY": 440,
                "scrollX": false,
                "sDom": 'ft',
                "ordering": false,
                "iDisplayLength": -1
            });
            $('#tablaEnvio').dataTable({
                "scrollY": 457,
                "scrollX": false,
                "sDom": 't',
                "ordering": false,
                "iDisplayLength": 50
            });
        },
        error: function (err) {
            alert("Ha ocurrido un error");
        }
    });
}

consultar = function () {
    $.ajax({
        type: 'POST',
        url: controlador + 'Listado',
        data: {
            empresa: $('#empresaSelect').val(),
            estado: $('#cbEstado').val(),
            periodo: $('#periodoSelect').val() === null || $('#periodoSelect').val() === '' ? '0' : $('#periodoSelect').val(),
            vigente: $('#cbSelectEnviovigente').is(':checked') ? $('#cbSelectEnviovigente').val() : 'T',
            fueraplazo: $('#cbSelectFueraPlazo').is(':checked') ? $('#cbSelectFueraPlazo').val() : 'T',
            estadoExcl: 'Registrado',
        },
        success: function (eData) {
            $('#listado').css("width", $('.form-main').width() + "px");
            $('#listado').html(eData);
            $('#tabla').dataTable({
                bJQueryUI: true,
                "scrollY": 440,
                "scrollX": false,
                "sDom": 'ft',
                "ordering": true,
                "iDisplayLength": -1
            });
        },
        error: function () {
            mostrarMensaje('mensaje', 'error', 'Ha ocurrido un error.');
        }
    });
}

function activarPlanTransmision(id, version) {
    idPlanTransmision = id;
    $('#versionEnvio').html(version);
    $('#popupEnvioActivar').bPopup({
        easing: 'easeOutBack',
        speed: 450,
        transition: 'slideDown'
    });
}

function activarEnvioVigente(){
    //cargarLoading();
    $.ajax({
        type: 'POST',
        url: controlador + 'ActivarEnvio',
        data: {
            id: idPlanTransmision
        },
        dataType: 'json',
        success: function (result) {
            idPlanTransmision = 0;
            //stopLoading();
            if (result == 1) {
                ObtenerListado();
                mostrarMensaje('mensaje', 'exito', 'El envío ha sido activado correctamente.');
            } else {
                mostrarMensaje('mensaje', 'error', 'Se ha producido un error.');
            }
        },
        error: function () {
            idPlanTransmision = 0;
            //stopLoading();
            mostrarMensaje('mensaje', 'error', 'Se ha producido un error.');
        }
    });
    $(`#popupEnvioActivar`).bPopup().close();
}

function consultarPlanTransmision(id) {
    document.location.href = controlador + "envio?id=" + id;
}

function revisarPlanTransmision(id) {
    document.location.href = controlador + "revisarenvio?id=" + id;
}

excel = function () {
    $.ajax({
        type: 'POST',
        url: controlador + "ExportarExcel",
        dataType: 'json',
        cache: false,
        data: {
            empresa: $('#empresaSelect').val(),
            estado: $('#cbEstado').val(),
            periodo: $('#periodoSelect').val() === null || $('#periodoSelect').val() === '' ? '0' : $('#periodoSelect').val(),
            vigente: $('#cbSelectEnviovigente').is(':checked') ? $('#cbSelectEnviovigente').val() : 'T',
            fueraplazo: $('#cbSelectFueraPlazo').is(':checked') ? $('#cbSelectFueraPlazo').val() : 'T',
            estadoExcl: 'Registrado',
        },
        success: function (resultado) {
            if (resultado == 1) {
                location.href = controlador + "DescargarExcel";
            }
            else {
                alert("NO");
            }
        },
        error: function () {
            mostrarError();
            ALERT("SI");
        }
    });
}


function cargarEmpresa() {
    $.ajax({
        type: 'POST',
        url: controlador + 'ListarEmpresas',
        contentType: "application/json; charset=utf-8",
        data: JSON.stringify(''),
        success: function (eData) {
            cargarListaEmpresas(eData);
        },
        error: function (err) {
            alert("Ha ocurrido un error");
        }
    });
}

function cargarListaEmpresas(empresas) {
    var selectEmpresa = $('#empresaSelect');
    $.each(empresas, function (index, empresa) {
        // Crear la opción
        var option = $('<option>', {
            value: empresa.Emprcodi,
            text: empresa.Emprnomb
        });

        // Agregar la opción al select
        selectEmpresa.append(option);
    });
}

function cargarPeriodo() {
    return new Promise((resolve, reject) => {
    $.ajax({
        type: 'POST',
        url: controlador + 'ListarPeriodos',
        contentType: "application/json; charset=utf-8",
        data: JSON.stringify(''),
        success: function (eData) {
            cargarListaPeriodo(eData);
            resolve(); 
        },
        error: function (err) {
            alert("Ha ocurrido un error");
            reject(err); 
        }
        });
    });
}

function cargarListaPeriodo(periodos) {
    var selectPeriodo = $('#periodoSelect');
    $.each(periodos, function (index, periodo) {
        // Crear la opción
        var option = $('<option>', {
            value: periodo.PeriCodigo,
            text: periodo.PeriNombre
        });

        // Agregar la opción al select
        selectPeriodo.append(option);
    });
}

function popupClose(id) {
    $(`#${id}`).bPopup().close();
}

function cargarLoading() {
    if (!isLoading) {
        isLoading = true;
        $('#loadingProyecto').bPopup({
            fadeSpeed: 'fast',
            opacity: 0.4,
            followSpeed: 500, // can be a string ('slow'/'fast') or int
            modalColor: '#000000',
            onClose: function () {
                isLoading = false;
            }
        });
    }
}

function stopLoading() {
    if (isLoading) {
        $('#loadingProyecto').bPopup().close();
        isLoading = false;
    }
}

mostrarMensaje = function (id, tipo, mensaje) {
    $('#' + id).attr('class', '');
    $('#' + id).addClass('action-' + tipo);
    $('#' + id).html(mensaje);
}