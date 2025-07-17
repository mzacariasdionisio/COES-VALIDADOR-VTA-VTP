var controlador = siteRoot + 'campanias/PlanTransmision/';
var idEliminacion = 0;
var idNuevaVersion = 0;
var catMaestraEstado = 50;

$(function () {
    popupCargarBienvenida();
    $('#btnConsultar').on('click', function () {
        consultar();
    });
    $('#btnNuevoPlanTransmision').on('click', function () {
        grabarPlanTransmision();
    })

    cargarDatos().then(() => {
        ObtenerListado();
    });
});

function cargarDatos() {
    return Promise.all([cargarPeriodo()])
        .then(cargarEmpresa,cargarCatalogoEstado(catMaestraEstado, "#cbEstado"));
}


function popupClose(id) {
    $(`#${id}`).bPopup().close();
}

function popupCrearPlanTransmision() {
    $('#popupCrearPlanTransmision').bPopup({
        easing: 'easeOutBack',
        speed: 450,
        transition: 'slideDown'
    });
}

function grabarPlanTransmision() {
    nuevoPlanTransmision();
}


function ObtenerListado() {
    $("#listado").html('');
    console.log('periodo', $('#periodoSelect').val() )
    $.ajax({
        type: 'POST',
        url: controlador + 'Listado',
        data: {
            empresa: $('#empresaSelect').val(),
            estado: $('#cbEstado').val(),
            periodo: $('#periodoSelect').val() === null || $('#periodoSelect').val() === '' ? '0' : $('#periodoSelect').val(),
            vigente: $('#cbSelectEnviovigente').is(':checked') ? $('#cbSelectEnviovigente').val() : 'T',
            fueraplazo: 'T',
            estadoExcl: 'T',
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
                "order": [[0, 'desc']],
                "iDisplayLength": -1
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
            estadoExcl: 'T',
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
                "order": [[0, 'desc']],
                "iDisplayLength": -1
            });
        },
        error: function () {
            mostrarMensaje('mensaje', 'error', 'Ha ocurrido un error.');
        }
    });
}

nuevoPlanTransmision = function () {
    var codemp = $("#empresaSelect").val();
    var codperi = $("#periodoSelect").val();
    var url = controlador + 'envio';
    if (codemp) {
        url += `?emp=${codemp}`;
    }
    if (codperi) {
        url += (codemp ? `&plan=${codperi}` : `?plan=${codperi}`);
    }
    document.location.href = url;}

function nuevaVersionEnvio() {
    document.location.href = controlador + "envio?idN=" + idNuevaVersion;
}
function editarPlanTransmision(id) {
    document.location.href = controlador + "envio?id=" + id;
}
function consultarPlanTransmision(id) {
    document.location.href = controlador + "envio?id=" + id+"&consult=true";
}


function absolverPlanTransmision(id) {
    document.location.href = controlador + "absolverenvio?id=" + id;
}

function cargarEmpresa() {
    $.ajax({
        type: 'POST',
        url: controlador + 'ListarEmpresas',
        contentType: "application/json; charset=utf-8",
        data: JSON.stringify(''),
        success: function (eData) {
            console.log(eData);
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

function popupEliminarPlanTransmision(id) {
    idEliminacion = id
    $('#popupEliminarPlan').bPopup({
        easing: 'easeOutBack',
        speed: 450,
        transition: 'slideDown'
    });
}

function popupNuevaVersionEnvio(id) {
    idNuevaVersion = id
    $('#popupNuevaVersionEnvio').bPopup({
        easing: 'easeOutBack',
        speed: 450,
        transition: 'slideDown'
    });
}

function popupCargarBienvenida() {
    $('#popupBienvenida').bPopup({
        easing: 'easeOutBack',
        speed: 450,
        transition: 'slideDown'
    });
}


function cargarEliminarPlanTrans() {
 
        $.ajax({
            type: 'POST',
            url: controlador + 'EliminarPlanTrans',
            data: {
                id: idEliminacion
            },
            dataType: 'json',
            success: function (response) {
                console.log(idEliminacion);
                console.log(response);
                idEliminacion = 0;
                if (response.result == 1) {
                    mostrarMensaje('mensaje', 'exito', 'El Plan ha sido eliminado correctamente.');
                    ObtenerListado();
                } else {
                    mostrarMensaje('mensaje', 'error', 'Se ha producido un error.');
                }
            },
            error: function () {
                console.log(idEliminacion);
                idEliminacion = 0;
                mostrarMensaje('mensaje', 'error', 'Se ha producido un error.');
            }
        });

    
        $(`#popupEliminarPlan`).bPopup().close();

}

function cargarCatalogoEstado(id, selectHtml) {
    $.ajax({
        type: 'POST',
        url: controlador + 'ListarCatalogo',
        contentType: "application/json; charset=utf-8",
        data: JSON.stringify({ id: id }),
        success: function (eData) {
            cargarListaParametrosEstado(eData, selectHtml);
        },
        error: function (err) {
            alert("Ha ocurrido un error");
        }
    });
}

function cargarListaParametrosEstado(listaValores, selectHtml) {
    var selectData = $(selectHtml);
    $.each(listaValores, function (index, catalogo) {
        // Crear la opción
        var option = $('<option>', {
            value: catalogo.DesDataCat,
            text: catalogo.DesDataCat
        });

        // Agregar la opción al select
        selectData.append(option);
    });

}

