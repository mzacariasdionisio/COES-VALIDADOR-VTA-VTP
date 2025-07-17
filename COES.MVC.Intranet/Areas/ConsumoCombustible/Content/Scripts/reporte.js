var controlador = siteRoot + 'ConsumoCombustible/Version/';
$(function () {

    $('#desc_fecha_ini').Zebra_DatePicker({
    });

    $('#cbEmpresa').multipleSelect({
        width: '250px',
        filter: true,
        single: false,
        onClose: function () {
            cargarListaCentral();
        }
    });

    $('#cbEmpresa').multipleSelect('checkAll');

    $('#cbCentral').multipleSelect({
        width: '220px',
        filter: true,
        single: false,
        onClose: function () {
            listado();
        }
    });

    $('#cbCentral').multipleSelect('checkAll');


    //
    $("#btnExportarExcel").click(function () {
        exportarExcel($("#hfVersion").val());
    });

    $("#btnRegresar").click(function () {
        var fecha = $("#hfFechaPeriodo").val();
        fecha = fecha.replace("/", "-");
        fecha = fecha.replace("/", "-");

        window.location.href = siteRoot + "ConsumoCombustible/Version/Index?fechaConsulta=" + fecha;
    });

    //
    listado();
});

///////////////////////////
/// Listar filtros 
///////////////////////////

function cargarListaCentral() {
    $("#div_central_filtro").html('');

    $.ajax({
        type: 'POST',
        url: controlador + 'ViewCargarFiltros',
        dataType: 'json',
        data: {
            vercodi: $("#hfVersion").val(),
            empresa: getEmpresa(),
        },
        cache: false,
        success: function (data) {
            $("#div_central_filtro").html('<select id="cbCentral" name="cbCentral"></select>');

            if (data.ListaCentral.length > 0) {
                $.each(data.ListaCentral, function (i, item) {
                    $('#cbCentral').get(0).options[$('#cbCentral').get(0).options.length] = new Option(item.Central, item.Equipadre);
                });
            }

            $('#cbCentral').multipleSelect({
                width: '220px',
                filter: true,
                single: false,
                onClose: function () {
                    listado();
                }
            });
            $('#cbCentral').multipleSelect('checkAll');

            listado();
        },
        error: function (err) {
            alert('Ha ocurrido un error.');
        }
    });
}

function getEmpresa() {
    var empresa = $('#cbEmpresa').multipleSelect('getSelects');
    if (empresa == "[object Object]" || empresa.length == 0) empresa = "-1";
    $('#hfEmpresa').val(empresa);
    var idEmpresa = $('#hfEmpresa').val();

    return idEmpresa;
}

function getCentral() {
    var central = $('#cbCentral').multipleSelect('getSelects');
    if (central == "[object Object]" || central.length == 0) central = "-1";
    $('#hfCentral').val(central);
    var idEmpresa = $('#hfCentral').val();

    return idEmpresa;
}

///////////////////////////
/// web 
///////////////////////////

function listado() {
    $('#listado').html('');

    ancho = $("#mainLayout").width() > 1100 ? $("#mainLayout").width() - 30 : 1100;

    $.ajax({
        type: 'POST',
        url: controlador + "ViewReporteWeb",
        data: {
            vercodi: $("#hfVersion").val(),
            empresa: getEmpresa(),
            central: getCentral()
        },
        success: function (evt) {
            if (evt.Resultado != "-1") {
                $('#listado').html(evt.Resultado);

                $("#listado").css("width", (ancho) + "px");
                $('#tabla_reporte').dataTable({
                    "sPaginationType": "full_numbers",
                    "destroy": "true",
                    "ordering": false,
                    "searching": true,
                    "iDisplayLength": -1,
                    "info": false,
                    "paging": false,
                    "scrollX": true,
                    "scrollY": "400px"
                });
            } else {
                alert("Ha ocurrido un error: " + evt.Mensaje);
            }
        },
        error: function (err) {
            alert('Ha ocurrido un error.');
        }
    });
}

function exportarExcel(id) {

    $.ajax({
        type: 'POST',
        url: controlador + "GenerarArchivoExcelReporte",
        data: {
            vercodi: id,
            empresa: getEmpresa(),
            central: getCentral(),
        },
        success: function (evt) {
            if (evt.Resultado != "-1") {
                window.location = controlador + "Exportar?file_name=" + evt.Resultado;
            } else {
                alert("Ha ocurrido un error: " + evt.Mensaje);
            }
        },
        error: function (err) {
            alert('Ha ocurrido un error.');
        }
    });

}
