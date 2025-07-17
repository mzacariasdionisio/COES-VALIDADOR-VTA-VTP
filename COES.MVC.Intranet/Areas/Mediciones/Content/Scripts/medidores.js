var controlador = siteRoot + 'mediciones/medidoresgeneracion/';
var contador = 0;

$(function () {

    $('#txtFechaInicial').Zebra_DatePicker({
        pair: $('#txtFechaFinal'),
        onSelect: function (date) {
            $('#txtExportarDesde').val(date);
            var date1 = getFecha(date);
            var date2 = getFecha($('#txtFechaFinal').val());

            if (date1 > date2) {
                $('#txtFechaFinal').val(date);
            }

            validarParametro();
        }
    });

    $('#txtFechaFinal').Zebra_DatePicker({
        direction: true,
        onSelect: function (date) {
            $('#txtExportarHasta').val(date);

            validarParametro();
        }
    });

    $('#txtExportarDesde').Zebra_DatePicker({
        pair: $('#txtExportarHasta'),
        onSelect: function (date) {
            var date1 = getFecha(date);
            var date2 = getFecha($('#txtExportarHasta').val());

            if (date1 > date2) {
                $('#txtExportarHasta').val(date);
            }
        }
    });

    $('#txtExportarHasta').Zebra_DatePicker({
        direction: true
    });

    $('#btnBuscar').click(function () {
        iniciar();
    });

    $('#btnExportar').click(function () {
        openExportar();
    });

    $('#cbTipoGeneracion').multipleSelect({
        width: '100%',
        filter: true
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

    $('#cbParametroExportar').multipleSelect({
        width: '100%'
    });

    $('#btnProcesarFile').click(function () {
        exportarFormato();
    });
    contador = 0;
    cargarPrevio();
    cargarEmpresas();
});

validarParametro = function () {
    var empresa = $('#cbEmpresas').multipleSelect('getSelects');
    var tipoGeneracion = $('#cbTipoGeneracion').multipleSelect('getSelects');
    var tipoEmpresa = $('#cbTipoEmpresa').multipleSelect('getSelects');

    if (empresa == "[object Object]") empresa = "";

    $('#hfEmpresa').val(empresa);
    $('#hfTipoGeneracion').val(tipoGeneracion);
    $('#hfTipoEmpresa').val(tipoEmpresa);

    if ($('#txtFechaInicial').val() != "" && $('#txtFechaFinal').val() != "") {
        $.ajax({
            type: 'POST',
            url: controlador + "ValidarParametro",
            data: {
                fechaInicial: $('#txtFechaInicial').val(),
                fechaFinal: $('#txtFechaFinal').val(),
                tiposEmpresa: $('#hfTipoEmpresa').val(),
                empresas: $('#hfEmpresa').val(), tiposGeneracion: $('#hfTipoGeneracion').val(),
                central: $('#cbCentral').val(), parametro: $('#cbParametro').val()
            },
            success: function (evt) {
                $("#cbParametroExportar").empty();
                $("#cbParametro").empty();
                if (evt == "-1") {
                    mostrarError();
                } else {
                    for (var i = 0; i < evt.length; i++) {
                        $('#cbParametroExportar').append('<option value=' + evt[i].EstadoCodigo + '>' + evt[i].EstadoDescripcion + '</option>');
                        $('#cbParametro').append('<option value=' + evt[i].EstadoCodigo + '>' + evt[i].EstadoDescripcion + '</option>');
                    }
                    if (contador == 0) {
                        iniciar();
                    }
                    contador += 1;
                }
                $("#cbParametroExportar").multipleSelect();
            },
            error: function () {
                mostrarError();
            }
        });
    }
    else {
        mostrarAlerta("Seleccione rango de fechas.");
    }
}

cargarPrevio = function () {
    $('#cbTipoGeneracion').multipleSelect('checkAll');
    $('#cbTipoEmpresa').multipleSelect('checkAll');
}

iniciar = function () {
    pintarPaginado();
    buscar(1);
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
            validarParametro();
        },
        error: function () {
            mostrarError();
        }
    });

}

buscar = function (nroPagina) {
    var empresa = $('#cbEmpresas').multipleSelect('getSelects');
    var tipoGeneracion = $('#cbTipoGeneracion').multipleSelect('getSelects');
    var tipoEmpresa = $('#cbTipoEmpresa').multipleSelect('getSelects');

    if (empresa == "[object Object]") empresa = "";

    $('#hfEmpresa').val(empresa);
    $('#hfTipoGeneracion').val(tipoGeneracion);
    $('#hfTipoEmpresa').val(tipoEmpresa);

    if ($('#txtFechaInicial').val() != "" && $('#txtFechaFinal').val() != "") {
        $.ajax({
            type: 'POST',
            url: controlador + "lista",
            data: {
                fechaInicial: $('#txtFechaInicial').val(),
                fechaFinal: $('#txtFechaFinal').val(),
                tiposEmpresa: $('#hfTipoEmpresa').val(),
                empresas: $('#hfEmpresa').val(), tiposGeneracion: $('#hfTipoGeneracion').val(),
                central: $('#cbCentral').val(), parametro: $('#cbParametro').val(),
                nroPagina: nroPagina
            },
            success: function (evt) {
                $('#listado').css("width", $('#mainLayout').width() + "px");
                $('#listado').html(evt);
                $('#tabla').dataTable({
                    "scrollY": 480,
                    "scrollX": true,
                    "sDom": 't',
                    "ordering": false,
                    "bDestroy": true,
                    "bPaginate": false,
                    "iDisplayLength": 50
                });
            },
            error: function () {
                mostrarError();
            }
        });
    }
    else {
        mostrarAlerta("Seleccione rango de fechas.");
    }
}

pintarPaginado = function () {
    var empresa = $('#cbEmpresas').multipleSelect('getSelects');
    var tipoGeneracion = $('#cbTipoGeneracion').multipleSelect('getSelects');
    var tipoEmpresa = $('#cbTipoEmpresa').multipleSelect('getSelects');

    if (empresa == "[object Object]") empresa = "";

    $('#hfEmpresa').val(empresa);
    $('#hfTipoGeneracion').val(tipoGeneracion);
    $('#hfTipoEmpresa').val(tipoEmpresa);

    if ($('#txtFechaInicial').val() != "" && $('#txtFechaFinal').val() != "") {
        $.ajax({
            type: 'POST',
            url: controlador + "paginado",
            data: {
                fechaInicial: $('#txtFechaInicial').val(), fechaFinal: $('#txtFechaFinal').val(),
                tiposEmpresa: $('#hfTipoEmpresa').val(),
                empresas: $('#hfEmpresa').val(), tiposGeneracion: $('#hfTipoGeneracion').val(),
                central: $('#cbCentral').val(), parametro: $('#cbParametro').val()
            },
            success: function (evt) {
                $('#paginado').html(evt);
                mostrarPaginado();
            },
            error: function () {
                alert("Ha ocurrido un error");
            }
        });
    }
    else {
        mostrarAlerta("Seleccione rango de fechas.");
    }
}

pintarBusqueda = function (nroPagina) {
    buscar(nroPagina);
}

openExportar = function () {
    $('#divExportar').css('display', 'block');
}

closeExportar = function () {
    $('#divExportar').css('display', 'none');
}


exportarFormato = function () {

    var formato = $("input:radio[name='rbFormato']:checked").val();
    var parametros = $('#cbParametroExportar').multipleSelect('getSelects');
    if (parametros == "") {
        var options = $('#cbParametroExportar option');
        if (options.length > 0) {
            for (var i = 0; i < options.length; i++) {
                parametros.push(options[i].value);
            }
        }
    }

    var empresa = $('#cbEmpresas').multipleSelect('getSelects');
    var tipoGeneracion = $('#cbTipoGeneracion').multipleSelect('getSelects');
    var tipoEmpresa = $('#cbTipoEmpresa').multipleSelect('getSelects');

    if (empresa == "[object Object]") empresa = "";

    $('#hfEmpresa').val(empresa);
    $('#hfTipoGeneracion').val(tipoGeneracion);
    $('#hfParametro').val(parametros);
    $('#hfTipoEmpresa').val(tipoEmpresa);

    if ($('#txtExportarDesde').val() != "" && $('#txtExportarHasta').val() != "") {

        $.ajax({
            type: 'POST',
            url: controlador + "validarexportacion",
            dataType: 'json',
            data: {
                formato: formato, fechaInicial: $('#txtExportarDesde').val(), fechaFinal: $('#txtExportarHasta').val(), parametros: $('#hfParametro').val()
            },
            success: function (result) {
                if (result == 1) {

                    $.ajax({
                        type: 'POST',
                        url: controlador + "exportar",
                        dataType: 'json',
                        data: {
                            fechaInicial: $('#txtExportarDesde').val(), fechaFinal: $('#txtExportarHasta').val(),
                            tiposEmpresa: $('#hfTipoEmpresa').val(),
                            empresas: $('#hfEmpresa').val(), tiposGeneracion: $('#hfTipoGeneracion').val(),
                            central: $('#cbCentral').val(), parametros: $('#hfParametro').val(), tipo: formato
                        },
                        success: function (result) {
                            if (result == "1") {
                                window.location = controlador + 'descargar?tipo=' + formato;
                            }
                            else {
                                alert(result);
                            }
                            /*if (result == -1) {
                                mostrarError();
                            }*/
                        },
                        error: function () {
                            mostrarError()
                        }
                    });
                }
                if (result == 2) {
                    mostrarAlerta("El lapso de tiempo no puede ser mayor a 3 meses.");
                }
                if (result == 3) {
                    mostrarAlerta("Para la exportación a CSV solo debe seleccionar un parámetro.");
                }
                if (result == 4) {
                    mostrarAlerta("Seleccione un parámetro a exportar.");
                }
                if (result == -1) {
                    mostrarError();
                }
            },
            error: function () {
                mostrarError();
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