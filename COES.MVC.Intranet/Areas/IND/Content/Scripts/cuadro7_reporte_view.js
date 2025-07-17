var controlador = siteRoot + 'IND/Reporte7/';
var ancho = 1000;
var ALTURA_HANDSON = 600;
var LISTA_HoT = [null];

$(function () {
    $('#cntMenu').css("display", "none");

    $('#tab-container').easytabs({
        animate: false
    });

    $('#tab-container').bind('easytabs:after', function () {
        refrehDatatable();
    });

    $('#tab-container').easytabs('select', '#tabTotal');

    $('#cbFamilia').change(function () {
        cargarListaEmpresa();
    });

    $('#cbCentralIntegrante').change(function () {
        cargarListaEmpresa();
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
            cargarHansonWeb();
        }
    });

    $('#cbCentral').multipleSelect('checkAll');

    var fechaIni = $("#hfFechaIni").val();
    var fechaFin = $("#hfFechaFin").val();

    $('#desc_fecha_ini').Zebra_DatePicker({
        direction: [fechaIni, fechaFin],
        pair: $('#desc_fecha_fin'),
        onSelect: function (date) {
            cargarHansonWeb();

        }
    });

    $('#desc_fecha_fin').Zebra_DatePicker({
        direction: [fechaIni, fechaFin],
        onSelect: function (date) {
            cargarHansonWeb();
        }
    });

    $("#btnExportarExcel").click(function () {
        exportarExcel();
    });

    $("#btnRegresar").click(function () {
        var cuadro = parseInt($("#hfCuadro").val()) || 0;
        var recalculo = parseInt($("#hfRecalculo").val()) || 0;
        var periodo = parseInt($("#hfPeriodo").val()) || 0;

        window.location.href = siteRoot + "IND/reporte7/cuadro7?idCuadro=" + cuadro + "&recacodi=" + recalculo;
    });

    cargarHansonWeb();

});

///////////////////////////
/// Listar filtros 
///////////////////////////

function cargarListaEmpresa() {
    $("#div_empresa_filtro").html('');
    $("#div_central_filtro").html('');

    $.ajax({
        type: 'POST',
        url: controlador + 'ViewCargarFiltros',
        dataType: 'json',
        data: {
            irptcodi: $("#hfReporteVersion").val(),
            centralIntegrante: $('#cbCentralIntegrante').val(),
            empresa: getEmpresa(),
            famcodi: $("#cbFamilia").val()
        },
        cache: false,
        success: function (data) {
            $("#div_empresa_filtro").html('<select id="cbEmpresa" name="cbEmpresa"></select>');
            $("#div_central_filtro").html('<select id="cbCentral" name="cbCentral"></select>');

            if (data.Resultado != "-1") {
                if (data.ListaEmpresa.length > 0) {
                    $.each(data.ListaEmpresa, function (i, item) {
                        $('#cbEmpresa').get(0).options[$('#cbEmpresa').get(0).options.length] = new Option(item.Emprnomb, item.Emprcodi);
                    });
                }
                if (data.ListaCentral.length > 0) {
                    $.each(data.ListaCentral, function (i, item) {
                        $('#cbCentral').get(0).options[$('#cbCentral').get(0).options.length] = new Option(item.Central, item.Equipadre);
                    });
                }

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
                        cargarHansonWeb();
                    }
                });
                $('#cbCentral').multipleSelect('checkAll');

                cargarHansonWeb();
            } else {
                alert("Ha ocurrido un error: " + data.Mensaje);
            }
        },
        error: function (err) {
            mostrarError();
        }
    });
}

function cargarListaCentral() {
    $("#div_central_filtro").html('');

    $.ajax({
        type: 'POST',
        url: controlador + 'ViewCargarFiltros',
        dataType: 'json',
        data: {
            irptcodi: $("#hfReporteVersion").val(),
            centralIntegrante: $('#cbCentralIntegrante').val(),
            empresa: getEmpresa(),
            famcodi: $("#cbFamilia").val()
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
                    cargarHansonWeb();
                }
            });
            $('#cbCentral').multipleSelect('checkAll');

            cargarHansonWeb();
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
/// Handson 
///////////////////////////

function cargarHansonWeb() {
    var cuadro = parseInt($("#hfCuadro").val()) || 0;

    ALTURA_HANDSON = parseInt($(".panel-container").height());
    ancho = $("#mainLayout").width() > 1100 ? $("#mainLayout").width() - 30 : 800;

    $("#listado1").html('');

    var container0 = document.getElementById('listado1');

    if (typeof LISTA_HoT[0] != 'undefined' && LISTA_HoT[0] !== null) {
        LISTA_HoT[0].destroy();
    }
    if (typeof LISTA_HoT[1] != 'undefined' && LISTA_HoT[1] !== null) {
        LISTA_HoT[1].destroy();
    }
    if (typeof LISTA_HoT[2] != 'undefined' && LISTA_HoT[2] !== null) {
        LISTA_HoT[2].destroy();
    }

    $.ajax({
        type: 'POST',
        url: controlador + "ViewCargarExcelWeb7",
        data: {
            irptcodi: $("#hfReporteVersion").val(),
            centralIntegrante: $('#cbCentralIntegrante').val(),
            empresa: getEmpresa(),
            central: getCentral(),
            famcodi: $("#cbFamilia").val(),
            strFechaIni: $("#desc_fecha_ini").val(),
            strFechaFin: $("#desc_fecha_fin").val()
        },
        success: function (evt) {
            if (evt.Resultado != "-1") {

                crearGrillaExcelCuadro7(0, container0, evt.HandsonTotal, ALTURA_HANDSON);

            } else {
                alert("Ha ocurrido un error: " + evt.Mensaje);
            }
        },
        error: function (err) {
            alert('Ha ocurrido un error.');
        }
    });
}

function exportarExcel() {

    $.ajax({
        type: 'POST',
        url: controlador + "GenerarArchivoExcelCuadro",
        data: {
            irptcodi: $("#hfReporteVersion").val(),
            centralIntegrante: $('#cbCentralIntegrante').val(),
            empresa: getEmpresa(),
            central: getCentral(),
            famcodi: $("#cbFamilia").val(),
            strFechaIni: $("#desc_fecha_ini").val(),
            strFechaFin: $("#desc_fecha_fin").val()
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

function refrehDatatable() {
    $('#tabla_reporte_consolidado').dataTable({
        "sPaginationType": "full_numbers",
        "destroy": "true",
        "ordering": false,
        "searching": true,
        "iDisplayLength": 15,
        "info": false,
        "paging": false,
        "scrollX": true,
        "scrollY": $('#listado1').height() > 400 ? 400 + "px" : "100%"
    });
}

