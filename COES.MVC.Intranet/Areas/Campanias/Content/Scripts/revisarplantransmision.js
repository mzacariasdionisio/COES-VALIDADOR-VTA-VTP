var controlador = siteRoot + 'campanias/plantransmision/';
var controladorRevision = siteRoot + 'campanias/revisionenvio/';
var idPlanTransmision = 0;
let isLoading = false;

$(function () {
    $('#btnConsultar').on('click', function () {
        consultar();
    });
    cargarDatos().then(() => {
        ObtenerListado();
    });
    $('#subtipoSelect').multipleSelect({
        width: '178px',
        filter: true
    });
    $('#subtipoSelect').multipleSelect('checkAll');
    $('#tipoProyectoSelect').multipleSelect({
        width: '178px',
        filter: true,
        onClick: function () {
            verificarTipoProyecto();
        },
        onCheckAll: function () {
            $('#subtipoSelect').multipleSelect('checkAll');
            verificarTipoProyecto();
        },
        onUncheckAll: function () {
            verificarTipoProyecto();
        }
    });
    $('#tipoProyectoSelect').multipleSelect('checkAll');

    $('#checkall').click(function () {
        $('input:checkbox').prop('checked', this.checked);
    });

    verificarTipoProyecto();
    
    $('#btnEnviarObservacion').click(function () {
        $('#popupEnviarObservacion').bPopup({
            easing: 'easeOutBack',
            speed: 450,
            transition: 'slideDown'
        });
        $('#btnEnviarPopupObs').off('click').on('click', function() {
            var idproyecto_val = $('input:checkbox:checked.chkbox_class').map(function () {
                return this.value;
            }).get().join(",");
            $.ajax({
                type: 'POST',
                url: controladorRevision + 'EnviarObservacion',
                data: {
                    idproyectos: idproyecto_val,
                },
                success: function (result) {
                    if (result == 1) {
                        ObtenerListado();
                        mostrarMensaje('mensaje', 'exito', 'Se enviaron correctamente las observaciones seleccionadas.');
                    }
                    else {
                        alert("Ha ocurrido un error al enviar observaciones");
                    }
        
                },
                error: function (err) {
                    alert("Ha ocurrido un error");
                }
            });
            popupClose('popupEnviarObservacion');
        });
    
    });    
    $('#btnCierreMasivo').click(function () {
        document.location.href = controladorRevision + "cerrarenvio";
    });         
    
});

function cargarDatos() {
    return Promise.all([cargarPeriodo(), cargarEmpresa()])
        .then();
}

function ObtenerListado() {

    $("#listado").html('');
    var empresa = $('#empresaSelect').multipleSelect('getSelects');
    var tipoproyecto = $('#tipoProyectoSelect').multipleSelect('getSelects');
    var subtipoproyecto = $('#subtipoSelect').multipleSelect('getSelects');
    var totalSubtipos = $('#subtipoSelect option').length;

    if (subtipoproyecto.length > 0 && subtipoproyecto.length < totalSubtipos) {
        tipoproyecto = ['1'];  
    }
    $.ajax({
        type: 'POST',
        url: controladorRevision + 'Listado',
        data: {
            empresa: empresa.join(','),
            estado: $('#cbEstado').val(),
            periodo: $('#periodoSelect').val() === null || $('#periodoSelect').val() === '' ? '0' : $('#periodoSelect').val(),
            tipoproyecto: tipoproyecto.join(','),
            subtipoproyecto: subtipoproyecto.join(','),
            observados: $('#cbSelectObservados').is(':checked') ? $('#cbSelectObservados  ').val() : 'T',
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
        },
        error: function (err) {
            alert("Ha ocurrido un error");
        }
    });
}

consultar = function () {
    $("#listado").html('');
    var empresa = $('#empresaSelect').multipleSelect('getSelects');
    var tipoproyecto = $('#tipoProyectoSelect').multipleSelect('getSelects');
    var subtipoproyecto = $('#subtipoSelect').multipleSelect('getSelects');
     var totalSubtipos = $('#subtipoSelect option').length;

     if (subtipoproyecto.length > 0 && subtipoproyecto.length < totalSubtipos) {
        tipoproyecto = ['1']; 
    }
    $.ajax({
        type: 'POST',
        url: controladorRevision + 'Listado',
        data: {
            empresa: empresa.join(','),
            estado: $('#cbEstado').val(),
            periodo: $('#periodoSelect').val() === null || $('#periodoSelect').val() === '' ? '0' : $('#periodoSelect').val(),
            tipoproyecto: tipoproyecto.join(','),
            subtipoproyecto: subtipoproyecto.join(','),
            observados: $('#cbSelectObservados').is(':checked') ? $('#cbSelectObservados  ').val() : 'T',
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
        error: function (err) {
            alert("Ha ocurrido un error");
        }
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
        if(empresa.Emprcodi != 0){
            var option = $('<option>', {
                value: empresa.Emprcodi,
                text: empresa.Emprnomb
            });
    
            // Agregar la opción al select
            selectEmpresa.append(option);
        }
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

function cargarTipoProyecto() {
    $.ajax({
        type: 'POST',
        url: controlador + 'ListarProyectos',
        contentType: "application/json; charset=utf-8",
        data: JSON.stringify(''),
        success: function (eData) {
            cargarListaTipoProyecto(eData);
            $('#tipoProyectoSelect').multipleSelect({
                width: '178px',
                filter: true
            });
            $('#tipoProyectoSelect').multipleSelect('checkAll');
        },
        error: function (err) {
            alert("Ha ocurrido un error");
        }
    });
}

function cargarListaTipoProyecto(tipos) {
    var selectTipo = $('#tipoProyectoSelect');
    $.each(tipos, function (index, tipo) {
        // Crear la opción
        var option = $('<option>', {
            value: tipo.TipoCodigo,
            text: tipo.TipoNombre
        });

        // Agregar la opción al select
        selectTipo.append(option);
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
            if (evt && evt.redirect) {
                window.location.href = evt.redirect;
                return;
            }
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

function verificarTipoProyecto() {
    const valoresSeleccionados = $('#tipoProyectoSelect').multipleSelect('getSelects');
    if (valoresSeleccionados.includes('1')) {
        $('#subtipoSelect').multipleSelect('enable');
    } else {
        $('#subtipoSelect').multipleSelect('disable');
    }
}