var controlador = siteRoot + 'StockCombustibles/';
var listFormatCodi = [];
var listFormatPeriodo = [];
$(function () {
    $('#cbEmpresa').multipleSelect({
        width: '222px',
        filter: true
    });
    $('#cbEmpresa').multipleSelect('checkAll');

    
    $('#FechaDesde').Zebra_DatePicker({

    });

    $('#FechaHasta').Zebra_DatePicker({

    });

    $('#btnBuscar').click(function () {
        if ($('#cbFormato').val() > 0) {
            mostrarListado();
        }
        else {
            alert("Error: Seleccionar Formato");
        }
    });
    //mostrarListado();
    $('#btnExportar').click(function () {
        exportarExcel();
    });

    $('#btnConfiguracion').on('click', function () {
        document.location.href = controlador + "Cumplimiento/Configuracion";

    });

    $('#btnEnviarCorreo').on('click', function () {
        enviarCorreo();
    });

});

function cargarListaEmpresa() {
    $('#cbEmpresa').empty();
    var idFormato = $('#cbFormato').val();

    $.ajax({
        type: 'POST',
        url: controlador + 'Cumplimiento/CargarListaEmpresa',
        data: {
            idFormato: idFormato
        },
        dataType: 'json',
        success: function (data) {
            var result = data.ListaEmpresas;
            for (var i in result) {
                $('#cbEmpresa').append('<option value=' + result[i].Emprcodi + '>' + result[i].Emprnomb + '</option>');
            }

            $('#cbEmpresa').multipleSelect({
                width: '222px',
                filter: true,
            });
            $('#cbEmpresa').multipleSelect('checkAll');
        },
        error: function () {
            alert('Ha ocurrido un error.');
        }
    });
}

function mostrarListado() {
    var empresa = $('#cbEmpresa').multipleSelect('getSelects');
    if (empresa == "[object Object]") empresa = "-1";
    if (empresa == "") empresa = "-1";
    var formato = $('#cbFormato').val();
    $('#hfEmpresa').val(empresa);

    var fecha1 = getFecha($('#FechaDesde').val());
    var fecha2 = getFecha($('#FechaHasta').val());
    var diferencia = numeroDias(fecha1, fecha2);

    if ($('#cbFormato').val() != "-1") {

        if (diferencia <= 180) {

            if (diferencia >= 0) {

                $.ajax({
                    type: 'POST',
                    url: controlador + "Cumplimiento/lista",
                    data: {
                        sEmpresas: $('#hfEmpresa').val(),
                        idFormato: formato,
                        fIni: $('#FechaDesde').val(),
                        fFin: $('#FechaHasta').val(),
                        envio: $('#cbEnvioInformacion').val(),
                        estado: $('#cbEstadoEmpresa').val()
                    },
                    success: function (evt) {
                        $('#listado').css("width", $('#mainLayout').width() + "px");
                        $('#listado').html(evt);
                        $('#tabla').dataTable({
                            "bSort": false,
                            "scrollX": true,
                            "sDom": 't',
                            "iDisplayLength": 50,
                            "fixedColumns": true,
                            "language": {
                                "emptyTable": "¡No existen registros!"
                            }
                        });
                        //$("#tabla_wrapper").css("width", "100%");
                        //$("#tabla_wrapper").css("width", $('#listado').width());
                        //$(".dataTables_scrollHeadInner table").css("width", $('#listado').width());
                        var numeroFila = $(".dataTables_scrollHeadInner table thead tr th").length;
                        if (numeroFila < 5) {
                            var tamanio = 300 + 200 * numeroFila;
                            $("div.dataTables_wrapper").css("width", tamanio + "px");
                        }
                    },
                    error: function () {
                        alert("Ha ocurrido un error");
                    }
                });
            }
            else {
                alert("La fecha final no puede ser menor a la inicial.");
            }
        }
        else {
            alert("Solo se permiten realizar consulta de hasta 6 meses.");
        }
    }
    else {
        alert("Debe seleccionar un formato.");
    }
}

function exportarExcel() {
    var empresa = $('#cbEmpresa').multipleSelect('getSelects');
    if (empresa == "[object Object]") empresa = "-1";
    if (empresa == "") empresa = "-1";
    var formato = $('#cbFormato').val();
    $('#hfEmpresa').val(empresa);

    var fecha1 = getFecha($('#FechaDesde').val());
    var fecha2 = getFecha($('#FechaHasta').val());
    var diferencia = numeroDias(fecha1, fecha2);

    if (diferencia <= 180) {
        if (diferencia >= 0) {

            $.ajax({
                type: 'POST',
                url: controlador + 'Cumplimiento/GenerarReporteCumplimiento',
                data: {
                    sEmpresas: $('#hfEmpresa').val(),
                    idFormato: formato,
                    fIni: $('#FechaDesde').val(),
                    fFin: $('#FechaHasta').val(),
                    envio: $('#cbEnvioInformacion').val(),
                    estado: $('#cbEstadoEmpresa').val()
                },
                dataType: 'json',
                success: function (result) {
                    if (result == 1)
                        window.location = controlador + "Cumplimiento/ExportarReporte";
                    if (result == -1)
                        alert("Error en exportar reporte...");
                },
                error: function () {
                    alert("Error en reporte...");
                }
            });
        }
        else {
            alert("La fecha final no puede ser menor a la inicial.");
        }
    }
    else {
        alert("Solo se permiten realizar consulta de hasta 6 meses.");
    }

}

numeroDias = function (inicio, final) {
    return Math.round((final - inicio) / (1000 * 60 * 60 * 24));
}

getFecha = function (date) {
    var parts = date.split("/");
    var date = new Date(parts[1] + "/" + parts[0] + "/" + parts[2]);
    return date.getTime();
}

function enviarCorreo() {
    $.ajax({
        type: 'POST',
        url: controlador + 'Cumplimiento/EnviarCorreoNotificacion',
        data: {
            fecha: $('#FechaDesde').val()
        },
        dataType: 'json',
        success: function (result) {
            if (result == 1) {
                alert('Correos enviados correctamente.');
            }
            else {
                alert('Ha ocurrido un error.');
            }
        },
        error: function () {
            alert('Ha ocurrido un error.');
        }
    });
}