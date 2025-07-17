var controlador = siteRoot + "mediciones/generacionrer/";
var FORMATO_DIARIO;
var ancho;

$(function () {
    ancho = $("#mainLayout").width() > 1200 ? $("#mainLayout").width() - 40 : 1200;
    //
    $('#cbHorizonte').change(function () {
        horizonte();
    });
    $('#cbHorizonte').change(function () {
        horizonte();
        consultarDatos();
    });
    FORMATO_DIARIO = $('#cbHorizonte').val();
    horizonte();

    $('#txtFecha').Zebra_DatePicker({
        onSelect: function () {
            consultarDatos();
        }
    });
    $('#cbAnio').Zebra_DatePicker({
        format: 'Y',
        onSelect: function () {
            cargarSemanaAnho($('#cbAnio').val());
        }
    });

    $('#cbSemana').change(function () {
        mostrarFechas();
        consultarDatos();
    });
    mostrarFechas();

    //
    $('#cbEmpresa').multipleSelect({
        width: '178px',
        filter: true
    });
    $('#cbEmpresa').multipleSelect('checkAll');
    $('#cbCentral').multipleSelect({
        width: '178px',
        filter: true
    });
    $('#cbCentral').multipleSelect('checkAll');

    $('#cbCogeneracion').multipleSelect({
        width: '70px',
        filter: true
    });
    $('#cbCogeneracion').multipleSelect('checkAll');

    $('#btnConsultar').click(function () {
        consultarDatos();
    });

    //
    $('#btnExportar').click(function () {
        openExportar();
    });
    $('#btnPorUnidad').click(function () {
        exportar(1);
    });
    $('#btnPorCentral').click(function () {
        exportar(3);
    });
    $('#btnPorGrupo').click(function () {
        exportar(2);
    });

    $('#hfAncho').val($('#mainLayout').width());

    consultarDatos();
});


openExportar = function () {
    $('#divExportar').css('display', 'block');
}

closeExportar = function () {
    $('#divExportar').css('display', 'none');
}


horizonte = function () {

    if ($('#cbHorizonte').val() == FORMATO_DIARIO) {
        $('.cntFecha').css("display", "table-cell");
        $('.cntSemana').css("display", "none");
        $('#fechasSemana').css("display", "none");
    } else {
        $('.cntFecha').css("display", "none");
        $('.cntSemana').css("display", "table-cell");
        $('#fechasSemana').css("display", "table-cell");
    }
};

mostrarFechas = function () {
    if ($('#cbSemana').val() != "" && $('#cbAnio').val() != "") {
        $.ajax({
            type: 'POST',
            url: controlador + 'obtenerfechasanio',
            dataType: 'json',
            data: { nroSemana: $('#cbSemana').val(), anio: $('#cbAnio').val() },
            success: function (result) {
                $('#txtFechaInicio').text(result.FechaInicio);
                $('#txtFechaFin').text(result.FechaFin);
            },
            error: function (err) {
            }
        });
    }
    else {
        $('#txtFechaInicio').text("");
        $('#txtFechaFin').text("");
    }
}

function cargarSemanaAnho(anho) {
    $.ajax({
        type: 'POST',
        url: controlador + 'CargarSemanas',
        async: false,
        data: {
            idAnho: anho
        },
        success: function (aData) {
            $("#divSemana").html(aData);

            $('#cbSemana').unbind('change');
            $('#cbSemana').change(function () {
                mostrarFechas();
            });
            mostrarFechas();
        },
        error: function (err) {
            alert("Ha ocurrido un error");
        }
    });
}

consultarDatos = function () {

    var empresa = $('#cbEmpresa').multipleSelect('getSelects');
    var central = $('#cbCentral').multipleSelect('getSelects');
    var cogeneracion = $('#cbCogeneracion').multipleSelect('getSelects');
    $('#hfEmpresa').val(empresa);
    $('#hfCentral').val(central);
    $('#hfCogeneracion').val(cogeneracion);
    var tipoReporte = $('input[name=cbTipoReporte]:checked').val();

    $.ajax({
        type: 'POST',
        url: controlador + "lista",
        data: {
            anio: $('#cbAnio').val(),
            fecha: $('#txtFecha').val(), nroSemana: $('#cbSemana').val(),
            horizonte: $('#cbHorizonte').val(),
            idEmpresa: $('#hfEmpresa').val(),
            tipoCentral: $('#hfCentral').val(),
            tipoCogeneracion: $('#hfCogeneracion').val(),
            tipoReporte: tipoReporte
        },
        success: function (model) {
            $('#listado').html('');

            if (model.Resultado != "-1") {
                $('#listado').html(model.Resultado);

                var anchoReporte = $('#reporte').width();
                $("#resultado").css("width", (anchoReporte > ancho ? ancho : anchoReporte) + "px");
                $('#reporte').dataTable({
                    "scrollX": true,
                    "scrollY": "350px",
                    "scrollCollapse": false,
                    "sDom": 't',
                    "ordering": false,
                    paging: false,
                    "bAutoWidth": false,
                    "destroy": "true",
                    "iDisplayLength": -1,
                    fixedColumns: {
                        leftColumns: 1
                    }
                });
            } else {
                alert("Ha ocurrido un error: " + model.Mensaje);
            }
        },
        error: function (err) {
            alert("Ha ocurrido un error");
        }
    });
}

exportar = function (tipoPresentacion) {

    var empresa = $('#cbEmpresa').multipleSelect('getSelects');
    var central = $('#cbCentral').multipleSelect('getSelects');
    var cogeneracion = $('#cbCogeneracion').multipleSelect('getSelects');
    $('#hfEmpresa').val(empresa);
    $('#hfCentral').val(central);
    $('#hfCogeneracion').val(cogeneracion);
    var tipoReporte = $('input[name=cbTipoReporte]:checked').val();

    $.ajax({
        type: 'POST',
        url: controlador + "exportar",
        data: {
            anio: $('#cbAnio').val(),
            fecha: $('#txtFecha').val(), nroSemana: $('#cbSemana').val(),
            horizonte: $('#cbHorizonte').val(),
            idEmpresa: $('#hfEmpresa').val(),
            tipoCentral: $('#hfCentral').val(),
            tipoCogeneracion: $('#hfCogeneracion').val(),
            tipoReporte: tipoReporte,
            tipoPresentacion: tipoPresentacion
        },
        success: function (model) {
            if (model.Resultado != "-1") {
                window.location = controlador + 'descargar';
            } else {
                alert("Ha ocurrido un error: " + model.Mensaje);
            }
        },
        error: function (err) {
            alert("Ha ocurrido un error");
        }
    });
}