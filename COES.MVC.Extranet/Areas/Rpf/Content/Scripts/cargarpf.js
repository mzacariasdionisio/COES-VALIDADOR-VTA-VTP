var controlador = siteRoot + "Rpf/Rpf/";

$(function () {
    $('#txtFecha').Zebra_DatePicker({
        direction: false
    });

    $('#btnNuevoCarga').click(function () {
        window.location.href = controlador + 'Carga';
    });

    $('#btnCancelar').click(function () {
        window.location.href = controlador + 'Carga';
    });

    $('#btnDescargarFormatoRpf').click(function () {
        donwloadFormato();
    });

    $('#btnProcesarFile').click(function () {
        $("#listado").hide();
        cargarEtapa();
        validarArchivo();
    });

    $('#cbEmpresa').change(function (e) {
        $("#fileInfo").hide();
        $("#btnPreprocesar").hide();
        $("#btnProcesarFile").hide();
        $("#btnCancelar").hide();
        $("#listado").hide();
        $("#btnSelectFile").show();
        $('#contentEtapa').css("display", "none");
    });

    load_ini();
});

function viewGrafico() {
    $("#contentGrafico").html('');
    $.ajax({
        type: 'POST',
        url: controlador + 'Comparativo',
        data: {
            idEmpresa: $('#cbEmpresa').val(),
            fecha: $('#txtFecha').val()
        },
        success: function (result) {
            if (result.length > 0) {
                $("#listado").show();
                pintarGrafico(result);
                $("#btnPreprocesar").hide();
                $("#btnProcesarFile").show();
                $("#btnCancelar").show();
                $("#btnSelectFile").hide();
            } else {
                alert('No existe puntos de equivalencia para esta empresa');
            }
        },
        error: function (result) {
            alert('ha ocurrido un error al descargar el archivo excel. ' + result.status + ' - ' + result.statusText + '.');
        }
    });
}

function openConsulta() {
    window.location.href = controlador + 'Consulta';
}

function donwloadFormato() {
    $.ajax({
        type: 'POST',
        url: controlador + 'generarformatorpf',
        data: {
            idEmpresa: $('#cbEmpresa').val(),
            fecha: $('#txtFecha').val()
        },
        success: function (result) {
            if (result.length > 0 && result != "-1") {
                mostrarExito("<strong>Los datos se descargaron correctamente</strong>");
                window.location.href = controlador + 'DescargarFormatoCsv?archivo=' + result;
            }
            else {
                alert("Error en descargar el archivo");
            }
        },
        error: function (result) {
            alert('ha ocurrido un error al descargar el archivo excel. ' + result.status + ' - ' + result.statusText + '.');
        }
    });
}

function load_ini() {
    var uploader = new plupload.Uploader({
        runtimes: 'html5,flash,silverlight,html4',
        browse_button: 'btnSelectFile',
        container: document.getElementById('container'),
        url: controlador + 'UploadCsv',
        flash_swf_url: 'Scripts/Moxie.swf',
        silverlight_xap_url: 'Scripts/Moxie.xap',
        multi_selection: false,
        filters: {
            max_file_size: '30mb',
            mime_types: [
                { title: "Archivos Excel .xlsx", extensions: "xlsx,xls" }
            ]
        },
        init: {
            PostInit: function () {
                document.getElementById('btnPreprocesar').onclick = function () {
                    if (uploader.files.length > 0) {
                        uploader.start();
                    }
                    else
                        loadValidacionFile("Seleccione archivo.");
                    return false;
                };
            },
            FilesAdded: function (up, files) {
                if (uploader.files.length == 2) {
                    uploader.removeFile(uploader.files[0]);
                }
                plupload.each(files, function (file) {
                    loadInfoFile(file.name, plupload.formatSize(file.size));
                });
                limpiarTodo();
                up.refresh();
            },
            UploadProgress: function (up, file) {
                mostrarProgreso(file.percent);
            },
            UploadComplete: function (up, file) {
                viewGrafico();
            },
            Error: function (up, err) {
                loadValidacionFile(err.code + "-" + err.message);
            }
        }
    });

    uploader.init();
}

function validarArchivo()
{
    $('#percentCargaArchivo').removeClass('etapa-progress');
    $('#percentCargaArchivo').addClass('etapa-ok');
    $('#percentCargaArchivo').text("OK...!");

    $('#percentValidacion').removeClass('etapa-progress');
    $('#percentValidacion').removeClass('etapa-ok');
    $('#percentValidacion').removeClass('etapa-error');
    $('#percentValidacion').removeClass('etapa-alert');

    $('#percentValidacion').addClass('etapa-progress');
    $('#percentValidacion').text("Validando...");

    $('#validacion').html("");

    $('#resultado').css('display', 'none');    

    var idEmpresa = $('#cbEmpresa').val();
    var fecha = $('#txtFecha').val();

    if (idEmpresa != '' && idEmpresa != '-1' && idEmpresa != '0') {

        if (fecha != '') {
            $.ajax({
                type: 'POST',
                url: controlador + 'validararchivo',
                dataType: 'json',
                data: {
                    idEmpresa: idEmpresa, fecha: fecha
                },
                cache: false,
                success: function (resultado) {
                    if (resultado == "OK") {
                        procesarArchivo(idEmpresa);
                    }
                    else if (resultado == "ERROR") {                       
                        $('#percentValidacion').addClass('etapa-error');
                        $('#percentValidacion').text("Ha ocurrido un error.");
                    }
                    else
                    {                       
                        $('#percentValidacion').addClass('etapa-alert');
                        $('#percentValidacion').text("No ha pasado la validación.");

                        $('#validacion').html(resultado);
                    }
                },
                error: function () {                   
                    $('#percentValidacion').addClass('etapa-error');
                    $('#percentValidacion').text("Ha ocurrido un error.");
                }
            });
        }
        else
        {                       
            $('#percentValidacion').addClass('etapa-alert');
            $('#percentValidacion').text("Seleccione fecha de proceso.");
        }
    }
    else
    {             
        $('#percentValidacion').addClass('etapa-alert');
        $('#percentValidacion').text("Seleccione empresa.");
    }
}

function procesarArchivo(idEmpresa) {

    $('#percentValidacion').removeClass('etapa-progress');
    $('#percentValidacion').addClass('etapa-ok');
    $('#percentValidacion').text("OK...!");
    
    $('#percentGrabado').removeClass('etapa-progress');
    $('#percentGrabado').removeClass('etapa-error');
    $('#percentGrabado').removeClass('etapa-alert');
    $('#percentGrabado').removeClass('etapa-ok');
    
    $('#percentGrabado').addClass('etapa-progress');
    $('#percentGrabado').text("Grabando...");
    $('#resultadoList').html("");
    $('#resultado').css('display', 'none');

    $.ajax({
        type: 'POST',
        url: controlador + 'procesararchivo',
        dataType: 'json',
        cache: false,
        data: {
            idEmpresa: idEmpresa
        },
        success: function (resultado) {
            if (resultado.Resultado == 1) {
                $('#percentGrabado').addClass('etapa-ok');
                $('#percentGrabado').text("Proceso completado...");
                $('#resultadoList').html(resultado.Mensaje);
                $('#resultado').css('display', 'block');
                $('#btnProcesarFile').css('display', 'none');
                $('#btnCancelar').css('display', 'none');
                $('#btnNuevoCarga').css('display', 'block');
            }
            else
            {
                $('#percentGrabado').addClass('etapa-error');
                $('#percentGrabado').text("Ha ocurrido un error.");
            }
        },
        error: function (err) {
            $('#percentGrabado').addClass('etapa-error');
            $('#percentGrabado').text("Ha ocurrido un error.");
        }
    });
}

function loadInfoFile(fileName, fileSize)
{
    $('#fileInfo').show();
    $('#fileInfo').removeClass("file-ok");
    $('#fileInfo').removeClass("file-alert");

    $("#btnPreprocesar").show();
    $('#fileInfo').html(fileName + " (" + fileSize + ")");
    $('#fileInfo').addClass("file-ok");
}

function loadValidacionFile(mensaje)
{
    $('#fileInfo').removeClass("file-ok");
    $('#fileInfo').removeClass("file-alert");

    $('#fileInfo').html(mensaje);
    $('#fileInfo').addClass("file-alert");
}

function mostrarProgreso(porcentaje)
{   
    $('#percentCargaArchivo').text(porcentaje + "%");
}

function cargarEtapa()
{
    $('#contentEtapa').css('display', 'block');
    $('#percentCargaArchivo').addClass('etapa-progress')
}

function ocultarEtapa()
{
    $('#contentEtapa').css('display', 'none');
}

function limpiarTodo()
{
    $('#percentCargaArchivo').removeClass('etapa-progress');
    $('#percentCargaArchivo').removeClass('etapa-error');
    $('#percentCargaArchivo').removeClass('etapa-alert');
    $('#percentCargaArchivo').removeClass('etapa-ok');

    $('#percentValidacion').removeClass('etapa-progress');
    $('#percentValidacion').removeClass('etapa-error');
    $('#percentValidacion').removeClass('etapa-alert');
    $('#percentValidacion').removeClass('etapa-ok');

    $('#percentGrabado').removeClass('etapa-progress');
    $('#percentGrabado').removeClass('etapa-error');
    $('#percentGrabado').removeClass('etapa-alert');
    $('#percentGrabado').removeClass('etapa-ok');
    
    $('#resultadoList').html("");

    $('#percentCargaArchivo').html("");
    $('#percentValidacion').html("");
    $('#percentGrabado').html("");
    $('#validacion').html("");

    $('#contentEtapa').css("display", "none");
}

function mostrarExito(mensaje) {
    $('#mensaje').removeClass();
    $('#mensaje').html(mensaje);
    $('#mensaje').addClass('action-exito');
}

pintarGrafico = function (result) {

    var categorias = [];
    var series = [];
    var dataRPF = [];
    var dataDespacho = [];

    for (var i in result) {
        categorias.push(result[i].Hora);
        dataRPF.push(result[i].ValorRPF);
        dataDespacho.push(result[i].ValorDespacho);
    }

    series.push({ name: 'RPF', data: dataRPF, color: '#7CB5EC' });
    series.push({ name: 'Despacho Ejecutado', data: dataDespacho, color: '#90ED7D' });

    $('#contentGrafico').highcharts({
        title: {
            text: 'Comparativo RPF VS Despacho Ejecutado',
            x: -20
        },
        xAxis: {
            categories: categorias,
            labels: {
                rotation: -90
            }
        },
        yAxis: {
            title: {
                text: 'Potencia (MW)'
            },
            plotLines: [{
                value: 0,
                width: 1,
                color: '#808080'
            }]
        },
        tooltip: {
            valueSuffix: 'MW'
        },
        legend: {



            borderWidth: 0
        },
        series: series
    });
}
