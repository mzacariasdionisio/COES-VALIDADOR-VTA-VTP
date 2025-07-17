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

        }
    });

    $('#txtFechaFinal').Zebra_DatePicker({
        direction: true,
        onSelect: function (date) {
            $('#txtExportarHasta').val(date);


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
});


cargarPrevio = function () {
    $('#cbTipoGeneracion').multipleSelect('checkAll');
    $('#cbTipoEmpresa').multipleSelect('checkAll');
}

iniciar = function () {
    pintarPaginado();
    buscar(1);
}

buscar = function (nroPagina) {

    if ($('#txtFechaInicial').val() != "" && $('#txtFechaFinal').val() != "") {
        $.ajax({
            type: 'POST',
            url: controlador + "listaGeneracionNoCoes",
            data: {
                fechaInicial: $('#txtFechaInicial').val(),
                fechaFinal: $('#txtFechaFinal').val(),
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

    if ($('#txtFechaInicial').val() != "" && $('#txtFechaFinal').val() != "") {
        $.ajax({
            type: 'POST',
            url: controlador + "paginadoGeneracionNoCoes",
            data: {
                fechaInicial: $('#txtFechaInicial').val(), fechaFinal: $('#txtFechaFinal').val()
            },
            success: function (evt) {
                $('#paginadoGeneracionNoCoes').html(evt);
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

exportarFormato = function () {
   
    if ($('#txtFechaInicial').val() != "" && $('#txtFechaFinal').val() != "") {

        $.ajax({
            type: 'POST',
            url: controlador + "validarexportacionGeneracionNoCoes",
            dataType: 'json',
            data: {
                fechaInicial: $('#txtFechaInicial').val(), fechaFinal: $('#txtFechaFinal').val(), parametros: $('#hfParametro').val()
            },
            success: function (result) {

                
                if (result == 1) {
                    $.ajax({
                        type: 'POST',
                        url: controlador + "exportarGeneracionNoCoes",
                        dataType: 'json',
                        data: {
                            fechaInicial: $('#txtFechaInicial').val(), fechaFinal: $('#txtFechaFinal').val()
                        },
                        success: function (result2) {                            

                            if (result2 == "1") {
                                window.location = controlador + 'DescargarGeneracionNoCoes';
                            }
                            else {
                                alert(result2);
                            }                           
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