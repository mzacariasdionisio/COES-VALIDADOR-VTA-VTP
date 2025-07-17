var controlador = siteRoot + 'campanias/plantransmision/';
var controladorFichas = siteRoot + 'campanias/fichasproyecto/';

$(function () {
    $('#btnAprobar').on('click', function () {
        envioFueraPlazoA('Aprobar');
    });
    $('#btnRechazar').on('click', function () {
        envioFueraPlazoD('Rechazar');
    });
    $('#btnCancelar').on('click', function () {
        cancelar();
    });
});

function envioFueraPlazoA(estado) {
    popupEnvioFueraPlazoA(estado);
}
function envioFueraPlazoD(estado) {
    popupEnvioFueraPlazoD(estado);
}

function popupEnvioFueraPlazoA(estado) {
    estadoN = estado;
    $('#popupEnvioFueraPlazoA').bPopup({
        easing: 'easeOutBack',
        speed: 450,
        transition: 'slideDown'
    });
}
function popupEnvioFueraPlazoD(estado) {
    estadoN = estado;
    $('#popupEnvioFueraPlazoD').bPopup({
        easing: 'easeOutBack',
        speed: 450,
        transition: 'slideDown'
    });
}

var cancelar = function () {
    parent.history.back();
};

function cargarDatos() {
    return Promise.all([cargarEmpresa()])
        .then(cargarPeriodo);
}

function cargarEmpresaProyecto() {
    return new Promise((resolve, reject) => {
        $.ajax({
            type: 'POST',
            url: controlador + 'ListarEmpresas',
            contentType: "application/json; charset=utf-8",
            data: JSON.stringify(''),
            success: function (eData) {
                console.log(eData);
                cargarListaEmpresasProyecto(eData);
                resolve();
            },
            error: function (err) {
                alert("Ha ocurrido un error");
                reject(err);
            }
        });
    });
}


function ObtenerListadoProyecto(codigoPlan) {
    $("#listadoProyecto").html('');
    console.log("entro a la tramsision");
    $.ajax({
        type: 'POST',
        url: controlador + 'ListadoProyecto',
        data: {
            id: codigoPlan,
        },
        success: function (eData) {
            $('#listadoProyecto').css("width", $('.form-main').width() + "px");
            $('#listadoProyecto').html(eData);
            $('#tablaProyecto').dataTable({
                bJQueryUI: true,
                "scrollY": 440,
                "scrollX": false,
                "sDom": 'ft',
                "ordering": true,
                "order": [[0, 'desc']],
                "iDisplayLength": -1
            });
        },
        error: function (err) {
            alert("Ha ocurrido un error");
        }
    });
}

consultarProyecto = function (id) {

    $.ajax({
        type: 'POST',
        url: controlador + 'proyecto',
        data: {
            id: id,
            modo: "consultar"
        },
        
        success: function (evt) {
            $('#contenidoProyecto').html(evt);

            $("#modal1").modal({
                escapeClose: false,
                clickClose: false,
                showClose: true
            });
        },
        error: function () {
            mostrarMensaje('mensaje', 'error', 'Se ha producido un error.');
        }
    });
}


function cargarDepartamentos(){
    $.ajax({
        type: 'POST',
        url: controladorFichas + 'ListarDepartamentos',
        contentType: "application/json; charset=utf-8",
        data: JSON.stringify(''),
        success: function (eData) {
            console.log(eData);
            cargarListaDepartamento(eData);
        },
        error: function (err) {
            alert("Ha ocurrido un error");
        }
    });
}

function buscarPlantransmision(codPlanTr) {
    $.ajax({
        type: 'POST',
        url: controlador + 'BuscarPlanTransmision',
        contentType: "application/json; charset=utf-8",
        data: JSON.stringify({ id: codPlanTr }),
        success: function (eData) {
            console.log("Plan transmision");
            console.log(eData);
            if (eData != null && eData.length > 0 ) {
                var planT = eData[0];
                planTransEncontrado = planT;
                var periodoCodi = String(planT.Pericodi);
                $("#empresaSelect").val(planT.Codempresa).prop("disabled", true);
                $("#periodoSelect").val(periodoCodi).prop("disabled", true);
                console.log(planT);
            }
        },
        error: function (err) {
            alert("Ha ocurrido un error");
        }
    });
}


function cargarListaDepartamento(departamentos) {
    var selectDepartamentos = $('#departamentosSelect');
    $.each(departamentos, function (index, departamento) {
        // Crear la opción
        var option = $('<option>', {
            value: departamento.Id,
            text: departamento.Nombre
        });

        // Agregar la opción al select
        selectDepartamentos.append(option);
    });

    selectDepartamentos.change(function () {
        // Obtener el id del departamento seleccionado
        var idSeleccionado = $(this).val();
        // Llamar a la función con el id del departamento
        console.log(idSeleccionado);
        cargarProvincia(idSeleccionado);
    });

}

function cargarProvincia(id) {
    $.ajax({
        type: 'POST',
        url: controladorFichas + 'ListarProvincias',
        contentType: "application/json; charset=utf-8",
        data: JSON.stringify({ id: id }),
        success: function (eData) {
            console.log(eData);
            cargarListaProvincia(eData);
        },
        error: function (err) {
            alert("Ha ocurrido un error");
        }
    });
}

function cargarListaProvincia(provincias) {
    var selectProvincias = $('#provinciasSelect');
    selectProvincias.empty();
    var optionDefault = $('<option>', {
        value: "",
        text: "Seleccione una provincia"
    });
    selectProvincias.append(optionDefault);
    cargarListaDistritos([]);
    $.each(provincias, function (index, provincia) {
        // Crear la opción
        var option = $('<option>', {
            value: provincia.Id,
            text: provincia.Nombre
        });

        // Agregar la opción al select
        selectProvincias.append(option);
    });

    selectProvincias.change(function () {
        // Obtener el id del departamento seleccionado
        var idSeleccionado = $(this).val();
        // Llamar a la función con el id del departamento
        cargarDistrito(idSeleccionado);
    });

}

function cargarDistrito(id) {
    $.ajax({
        type: 'POST',
        url: controladorFichas + 'ListarDistritos',
        contentType: "application/json; charset=utf-8",
        data: JSON.stringify({ id: id }),
        success: function (eData) {
            console.log(eData);
            cargarListaDistritos(eData);
        },
        error: function (err) {
            alert("Ha ocurrido un error");
        }
    });
}

function cargarListaDistritos(distritos) {
    var selectDistritos = $('#distritosSelect');
    selectDistritos.empty();
    var optionDefault = $('<option>', {
        value: "",
        text: "Seleccione un distrito"
    });
    selectDistritos.append(optionDefault);
    $.each(distritos, function (index, distrito) {
        // Crear la opción
        var option = $('<option>', {
            value: distrito.Id,
            text: distrito.Nombre
        });

        // Agregar la opción al select
        selectDistritos.append(option);
    });

}


function cargarEmpresa() {
    return new Promise((resolve, reject) => {
        $.ajax({
            type: 'POST',
            url: controlador + 'ListarEmpresas',
            contentType: "application/json; charset=utf-8",
            data: JSON.stringify(''),
            success: function (eData) {
                console.log(eData);
                cargarListaEmpresas(eData);
                resolve();
            },
            error: function (err) {
                alert("Ha ocurrido un error");
                reject(err);
            }
        });
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
                console.log(eData);
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

function aprobarEnvioVigente(){
    var id = $("#txtCodPlanTransmision").val();
    //cargarLoading();
    $.ajax({
        type: 'POST',
        url: controlador + 'AprobarEnvio',
        data: {
            id: id
        },
        dataType: 'json',
        success: function (result) {
            console.log
            idPlanTransmision = 0;
            //stopLoading();
            if (result == 1) {
                mostrarMensaje('mensajeRevisar', 'exito', 'El envío ha sido aprobado correctamente.');
                disabledButton();
                setTimeout(function () {
                    document.location.href = controlador;
                }, 3000);
            } else {
                mostrarMensaje('mensajeRevisar', 'error', 'Se ha producido un error.');
            }
        },
        error: function () {
            idPlanTransmision = 0;
            //stopLoading();
            mostrarMensaje('mensajeRevisar', 'error', 'Se ha producido un error.');
        }
    });
    $(`#popupEnvioFueraPlazoA`).bPopup().close();
}

function rechazarEnvioVigente(){
    var param = {};
    var id = $("#txtCodPlanTransmision").val();
    var motivo = $("#txtMotivoRechazo").val();
    param.CodPlanTransmision = id;
    param.Comentarios = motivo;
    //cargarLoading();
    $.ajax({
        type: 'POST',
        url: controlador + 'RechazarEnvio',
        data: param,
        dataType: 'json',
        success: function (result) {
            console.log
            idPlanTransmision = 0;
            //stopLoading();
            if (result == 1) {
                
                mostrarMensaje('mensajeRevisar', 'exito', 'El envío ha sido rechazado correctamente.');
                disabledButton();
                setTimeout(function () {
                    document.location.href = controlador;
                }, 3000);
            } else {
                mostrarMensaje('mensajeRevisar', 'error', 'Se ha producido un error.');
            }
        },
        error: function () {
            idPlanTransmision = 0;
            //stopLoading();
            mostrarMensaje('mensajeRevisar', 'error', 'Se ha producido un error.');
        }
    });
    $(`#popupEnvioFueraPlazoD`).bPopup().close();
}
mostrarMensaje = function (id, tipo, mensaje) {
    $('#' + id).removeClass();
    $('#' + id).addClass('action-' + tipo);
    $('#' + id).html(mensaje);
}

disabledButton = function() {
    document.getElementById("btnAprobar").disabled = true;
    document.getElementById("btnRechazar").disabled = true;
    document.getElementById("btnAprobar").style.visibility = "hidden";
    document.getElementById("btnRechazar").style.visibility = "hidden";
}