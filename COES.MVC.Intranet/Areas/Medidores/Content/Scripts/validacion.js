var controlador = siteRoot + 'medidores/validacionregistro/'

$(function () {

    $('#txtFechaInicial').val(getInicioMesAnterior());
    $('#txtFechaFinal').val(getFinMesAnterior());

    $('#txtFechaInicial').Zebra_DatePicker({
        pair: $('#txtFechaFinal'),
        onSelect: function (date) {
            var date1 = getFecha(date);
            var date2 = getFecha($('#txtFechaFinal').val());

            if (date1 > date2) {
                $('#txtFechaFinal').val(date);
            }
        }
    });

    $('#txtFechaFinal').Zebra_DatePicker({
        direction: true
    });

    $('#cbTipoEmpresa').multipleSelect({
        width: '100%',
        filter: true,
        onClick: function (view) {
            cargarEmpresas();
        },
        onClose: function (view) {
            cargarEmpresas();
        }
    });

    $('#cbTipoGeneracion').multipleSelect({
        width: '100%',
        filter: true
    });

    $('#cbRecursoEnergetico').multipleSelect({
        width: '100%',
        filter: true
    });

    $('#btnConsultar').click(function () {
        mostrarReporte();
    });

    $('#btnExportar').click(function () {
        exportar();
    });

    $('#btnConfiguracion').click(function () {
        document.location.href = siteRoot + 'medidores/equivalencia/index';
    });

    $('#cbFiltro').change(function () {
        filtrar();
    });

    $('#btnGrafico').on('click', function () {
        document.location.href = controlador + 'grafico';
    })

    cargarPrevio();
    cargarEmpresas();
    mostrarReporte();
});



cargarPrevio = function () {
    $('#cbTipoGeneracion').multipleSelect('checkAll');
    $('#cbTipoEmpresa').multipleSelect('checkAll');
    $('#cbRecursoEnergetico').multipleSelect('checkAll');
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
        error: function (err) {
            mostrarError();
        }
    });
}

filtrar = function () {
    var id = $('#cbFiltro').val();
    $.ajax({
        type: 'POST',
        url: controlador + "filtro",
        data: {
            id: id
        },
        success: function (evt) {
            generarTablaHtml(evt);
        },
        error: function (err) {
            mostrarError();
        }
    });
}

mostrarReporte = function () {
    $('#cbFiltro').val("0");
    var empresa = $('#cbEmpresas').multipleSelect('getSelects');
    var tipoGeneracion = $('#cbTipoGeneracion').multipleSelect('getSelects');
    var fuenteEnergia = $('#cbRecursoEnergetico').multipleSelect('getSelects');
    var tipoEmpresa = $('#cbTipoEmpresa').multipleSelect('getSelects');
    if (empresa == "[object Object]") empresa = "";

    $('#hfEmpresa').val(empresa);
    $('#hfTipoGeneracion').val(tipoGeneracion);
    $('#hfFuenteEnergia').val(fuenteEnergia);
    $('#hfTipoEmpresa').val(tipoEmpresa);

    if ($('#txtFechaInicial').val() != "" && $('#txtFechaFinal').val() != "") {
        $.ajax({
            type: 'POST',
            url: controlador + "reporte",
            data: {
                fechaInicial: $('#txtFechaInicial').val(), fechaFinal: $('#txtFechaFinal').val(),
                tiposEmpresa: $('#hfTipoEmpresa').val(),
                empresas: $('#hfEmpresa').val(), tiposGeneracion: $('#hfTipoGeneracion').val(),
                fuentesEnergia: $('#hfFuenteEnergia').val(), central: $('#cbCentral').val()
            },
            success: function (evt) {
                generarTablaHtml(evt);
                $('#cbFiltro').val("0");
            },
            error: function (err) {
                mostrarError();
            }
        });
    }
    else {
        mostrarAlerta("Seleccione rango de fechas.");
    }
}


exportar = function () {

    var empresa = $('#cbEmpresas').multipleSelect('getSelects');
    var tipoGeneracion = $('#cbTipoGeneracion').multipleSelect('getSelects');
    var fuenteEnergia = $('#cbRecursoEnergetico').multipleSelect('getSelects');
    var tipoEmpresa = $('#cbTipoEmpresa').multipleSelect('getSelects');
    if (empresa == "[object Object]") empresa = "";
    $('#hfEmpresa').val(empresa);
    $('#hfTipoGeneracion').val(tipoGeneracion);
    $('#hfFuenteEnergia').val(fuenteEnergia);
    $('#hfTipoEmpresa').val(tipoEmpresa);

    if ($('#txtFechaInicial').val() != "" && $('#txtFechaFinal').val() != "") {
        $.ajax({
            type: 'POST',
            url: controlador + "exportar",
            dataType: 'json',
            data: {
                fechaInicial: $('#txtFechaInicial').val(), fechaFinal: $('#txtFechaFinal').val(),
                tiposEmpresa: $('#hfTipoEmpresa').val(),
                empresas: $('#hfEmpresa').val(), tiposGeneracion: $('#hfTipoGeneracion').val(),
                fuentesEnergia: $('#hfFuenteEnergia').val(), central: $('#cbCentral').val(),
                id: $('#cbFiltro').val()
            },
            success: function (result) {
                if (result == 1) {
                    window.location = controlador + 'descargar';
                }
                if (result == -1) {
                    mostrarError();
                }
            },
            error: function (err) {
                mostrarError()
            }
        });
    }
    else {
        mostrarAlerta("Seleccione rango de fechas.");
    }
}


mostrarAlerta = function (mensaje) {
    alert(mensaje);
}

mostrarError = function () {
    alert("Ha ocurrido un error.");
}

getFecha = function (date) {
    var parts = date.split("/");
    var date = new Date(parts[1] + "/" + parts[0] + "/" + parts[2]);
    return date.getTime();
}

function generarTablaHtml(evt) {

    $('#reporte').css("width", $('#mainLayout').width() - 30 + "px");
    $('#reporte').html(evt);
    $('#tabla').dataTable({
        "scrollY": $('#reporte').height() > 400 ? 400 + "px" : "100%",
        "scrollX": false,
        "sDom": 't',
        "ordering": false,
        "bPaginate": false,
        "iDisplayLength": -1
    });
}
function getInicioMesAnterior() {
    var fecha = new Date();
    var mes = (fecha.getMonth() <= 9 ? (fecha.getMonth() < 1 ? "12" : ("0" + fecha.getMonth())) : fecha.getMonth());
    var anio = (fecha.getMonth() < 1 ? fecha.getFullYear() - 1 : fecha.getFullYear());
    return "01/" + mes + "/" + anio;
}

function getFinMesAnterior() {
    var fecha = new Date();
    var mes = (fecha.getMonth() <= 9 ? (fecha.getMonth() < 1 ? 12 : fecha.getMonth()) : fecha.getMonth());
    var dias = new Date(fecha.getFullYear(), mes, 0).getDate();
    var anio = (fecha.getMonth() < 1 ? fecha.getFullYear() - 1 : fecha.getFullYear());
    return dias + "/" + (mes <= 9 ? ("0" + mes) : mes) + "/" + anio;
}