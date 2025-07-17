var controlador = siteRoot + 'Medidores/Consolidado/'

$(function () {
    
    $('#FechaDesde').Zebra_DatePicker({
        format: 'm Y'
    });

    $('#FechaHasta').Zebra_DatePicker({
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


consultar = function ()
{
    var empresa = $('#cbEmpresas').multipleSelect('getSelects');
    var tipoGeneracion = $('#cbTipoGeneracion').multipleSelect('getSelects');
    var tipoEmpresa = $('#cbTipoEmpresa').multipleSelect('getSelects');
    if (empresa == "[object Object]") empresa = "";
    $('#hfEmpresa').val(empresa);
    $('#hfTipoGeneracion').val(tipoGeneracion);
    $('#hfTipoEmpresa').val(tipoEmpresa);

    if ($('#FechaDesde').val() != "" && $('#FechaHasta').val() != "") {
        $.ajax({
            type: 'POST',
            url: controlador + "consulta",
            dataType: 'json',
            data: {
                fechaInicio: $('#FechaDesde').val(),
                fechaFin: $('#FechaHasta').val(),
                tiposEmpresa: $('#hfTipoEmpresa').val(),
                empresas: $('#hfEmpresa').val(), tiposGeneracion: $('#hfTipoGeneracion').val(),
                central: $('#cbCentral').val()
            },
            success: function (result) {
                $('#listado').html(result);
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

    if ($('#FechaDesde').val() != "" && $('#FechaHasta').val()) {
        $.ajax({
            type: 'POST',
            url: controlador + "exportar",
            dataType: 'json',
            data: {
                fechaInicio: $('#FechaDesde').val(),
                fechaFin : $('#FechaHasta').val(),
                tiposEmpresa: $('#hfTipoEmpresa').val(),
                empresas: $('#hfEmpresa').val(), tiposGeneracion: $('#hfTipoGeneracion').val(),
                central: $('#cbCentral').val()
            },
            success: function (result) {
                if (result == "1") {
                    window.location = controlador + 'descargar'
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