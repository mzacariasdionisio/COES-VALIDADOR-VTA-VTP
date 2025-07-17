var controlador = siteRoot + 'campanias/plantransmision/';
var controladorRevision = siteRoot + 'campanias/revisionenvio/';
var idPlanTransmision = 0;
let isLoading = false;

$(function () {
    $('#btnConsultar').on('click', function () {
        ObtenerListado();
    });
    $('#btnExcel').on('click', function () {
        excel();
    });
    cargarDatos().then(() => {
        ObtenerListado();
    });
    $('#btnAtrasRevision').click(function () {
        window.location.href = controladorRevision;
    });    
});

function cargarDatos() {
    return Promise.all([cargarPeriodo(), cargarEmpresa()])
        .then();
}

function ObtenerListado() {

    $("#listado").html('');
    var empresa = $('#empresaSelect').multipleSelect('getSelects');
    $.ajax({
        type: 'POST',
        url: controladorRevision + 'ListadoEnvio',
        data: {
            empresa: empresa.join(','),
            estado: $('#cbEstado').val(),
            periodo: $('#periodoSelect').val() === null || $('#periodoSelect').val() === '' ? '0' : $('#periodoSelect').val(),
            estadoExcl: 'Registrado',
        },
        success: function (eData) {
            $('#listado').css("width", $('.form-main').width() + "px");
            $('#listado').html(eData);

            const totalColumnas = $('#tabla thead th').length;
            const penultimaColumna = totalColumnas - 2;

            $('#tabla').DataTable({
                bJQueryUI: true,
                scrollY: 440,
                scrollX: false,
                sDom: 'ft',
                ordering: true,
                order: [[0, 'desc']],
                iDisplayLength: -1,
                columnDefs: [
                    {
                        targets: penultimaColumna,
                        orderable: false
                    }
                ]
            });

            // Cuando se hace clic en el checkbox general
            $('#checkall').on('click', function () {
                var isChecked = $(this).is(':checked');
                $('#tabla input.chkbox_class[type="checkbox"]').prop('checked', isChecked);
            });

            // Cuando se hace clic en un checkbox individual
            $('#tabla').on('change', 'input.chkbox_class[type="checkbox"]', function () {
                var total = $('#tabla input.chkbox_class[type="checkbox"]').length;
                var checked = $('#tabla input.chkbox_class[type="checkbox"]:checked').length;
                $('#checkall').prop('checked', total === checked);
            });
        }
,
        error: function (err) {
            alert("Ha ocurrido un error");
        }
    });
}

function cierrePlanTransmision(id) {
    idPlanTransmision = id;
    $('#popupEnvioCierre').bPopup({
        easing: 'easeOutBack',
        speed: 450,
        transition: 'slideDown'
    });
}

function cierreMasivoPlanTransmision() {
    idPlanTransmision = 0;
    $('#popupEnvioCierreMasivo').bPopup({
        easing: 'easeOutBack',
        speed: 450,
        transition: 'slideDown'
    });
}

function consultarPlanTransmision(id) {
    document.location.href = controladorRevision + "envio?id=" + id;
}

function cargarEmpresa() {
    return new Promise((resolve, reject) => {
        $.ajax({
            type: 'POST',
            url: controlador + 'ListarEmpresas',
            contentType: "application/json; charset=utf-8",
            data: JSON.stringify(''),
            success: function (eData) {
                cargarListaEmpresas(eData);
                $('#empresaSelect').multipleSelect({
                    width: '178px',
                    filter: true
                });
                $('#empresaSelect').multipleSelect('checkAll');
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

mostrarMensaje = function (id, tipo, mensaje) {
    $('#' + id).attr('class', '');
    $('#' + id).addClass('action-' + tipo);
    $('#' + id).html(mensaje);
}

consultarProyecto = function (id) {

    $.ajax({
        type: 'POST',
        url: controladorRevision + 'proyecto',
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

cerrarEnvioMasivo = function() {
    var idenvios;
    if(idPlanTransmision == 0) {
        var idenvios = $('input:checkbox:checked.chkbox_class').map(function () {
            return this.value;
        }).get().join(",");
        popupClose('popupEnvioCierreMasivo');
    } else {
        idenvios = idPlanTransmision;
        popupClose('popupEnvioCierre');
    }
    $.ajax({
        type: 'POST',
        url: controladorRevision + 'CerrarEnvioMasivo',
        data: {
            idenvios: idenvios,
        },
        success: function (result) {
            if (result == 1) {
                ObtenerListado();
                mostrarMensaje('mensaje', 'exito', 'Se cerraron correctamente el/los envío(s) seleccionado(s).');
            }
            else {
                alert("Ha ocurrido un error al cerrar envíos");
            }

        },
        error: function (err) {
            alert("Ha ocurrido un error");
        }
    });
}
