var controlador = siteRoot + 'mediciones/ejecutado/'

$(function () {

    $('#txtFechaDesde').Zebra_DatePicker({
        format: 'm Y'
    });

    $('#txtFechaHasta').Zebra_DatePicker({
        format: 'm Y'
    });

    $('#cbTipoGeneracion').multipleSelect({
        width: '140px',
        filter: true
    });

    $('#btnConsultar').click(function () {
        consultar();
    });

    $('#btnExportar').click(function () {
        exportar();
    });

    $('#cbTipoEmpresa').multipleSelect({
        width: '140px',
        filter: true,
        onClick: function (view) {
            cargarEmpresas();
        },
        onClose: function (view) {
            cargarEmpresas();
        }
    });

    cargarPrevio();
    cargarEmpresas();
    consultar();
});

cargarPrevio = function () {
    $('#cbTipoGeneracion').multipleSelect('checkAll');
    $('#cbTipoEmpresa').multipleSelect('checkAll');
}

cargarEmpresas = function () {
    var tipoEmpresa = $('#cbTipoEmpresa').multipleSelect('getSelects');
    $('#hfTipoEmpresa').val(tipoEmpresa);

    $.ajax({
        type: 'POST',
        url: controlador + "empresas",
        data: {
            tiposEmpresa: $('#hfTipoEmpresa').val()
        },
        success: function (evt) {
            $('#empresas').html(evt);
        },
        error: function () {
            mostrarError();
        }
    });
}

consultar = function () {
    var empresa = $('#cbEmpresas').multipleSelect('getSelects');
    var tipoGeneracion = $('#cbTipoGeneracion').multipleSelect('getSelects');
    var tipoEmpresa = $('#cbTipoEmpresa').multipleSelect('getSelects');
    if (empresa == "[object Object]") empresa = "";
    $('#hfEmpresa').val(empresa);
    $('#hfTipoGeneracion').val(tipoGeneracion);
    $('#hfTipoEmpresa').val(tipoEmpresa);

    if ($('#txtFechaDesde').val() != "" && $('#txtFechaHasta').val() != "") {
        $.ajax({
            type: 'POST',
            url: controlador + "consultaacumulado",
            dataType: 'json',
            data: {
                fechaInicio: $('#txtFechaDesde').val(),
                fechaFin: $('#txtFechaHasta').val(),
                tiposEmpresa: $('#hfTipoEmpresa').val(),
                empresas: $('#hfEmpresa').val(),
                tiposGeneracion: $('#hfTipoGeneracion').val()
            },
            success: function (result) {
                $('#listado').html(result);
                $('#tabla').tableScroll({ height: 590, width:1153 });                
            },
            error: function () {
                mostrarError();
            }
        });
    }
    else {
        alert("Por favor seleccione mes.");
    }
}

exportar = function () {
    var empresa = $('#cbEmpresas').multipleSelect('getSelects');
    var tipoGeneracion = $('#cbTipoGeneracion').multipleSelect('getSelects');
    var tipoEmpresa = $('#cbTipoEmpresa').multipleSelect('getSelects');
    if (empresa == "[object Object]") empresa = "";
    $('#hfEmpresa').val(empresa);
    $('#hfTipoGeneracion').val(tipoGeneracion);
    $('#hfTipoEmpresa').val(tipoEmpresa);

    if ($('#txtFechaDesde').val() != "" && $('#txtFechaHasta').val()) {
        $.ajax({
            type: 'POST',
            url: controlador + "exportaracumulado",
            dataType: 'json',
            data: {
                fechaInicio: $('#txtFechaDesde').val(),
                fechaFin: $('#txtFechaHasta').val(),
                tiposEmpresa: $('#hfTipoEmpresa').val(),
                empresas: $('#hfEmpresa').val(),
                tiposGeneracion: $('#hfTipoGeneracion').val()                
            },
            success: function (result) {
                if (result == "1") {
                    window.location = controlador + 'descargaracumulado'
                }
                else {
                    alert(result);
                }
            },
            error: function () {
                mostrarError();
            }
        });
    }
    else {
        alert("Por favor seleccione mes.");
    }
}

mostrarAlerta = function (mensaje) {
    alert(mensaje);
}

mostrarError = function () {
    alert("Ha ocurrido un error.");
}