var controlador = siteRoot + "generacionrer/";

$(function () {

    $('#txtFecha').Zebra_DatePicker({
        onSelect: function () {
            validarFecha();
        }
    });

    $('#cbHorizonte').change(function () {
        horizonte();
    });

    $('#cbSemana').change(function () {
        mostrarFechas();
    });

    $('#cbAnio').change(function () {
        $('#cbSemana').val("1");
        mostrarFechas();
    });
});

horizonte = function () {    
    $.ajax({
        type: 'POST',
        url: controlador + 'validarhorizonte',
        dataType: 'json',
        data: { anio: $('#cbAnio').val(), horizonte: $('#cbHorizonte').val(), semana: $('#cbSemana').val(), fecha: $('#txtFecha').val() },
        success: function (result) {
            
            $('#contentPlazo').text(result.ValidacionPlazo);
            
            if ($('#cbHorizonte').val() == "0") {
                $('#cntFecha').css("display", "block");
                $('#cntSemana').css("display", "none");
                $('#cntAnio').css("display", "none");
                $('#fechasSemana').css("display", "none");
            }
            if ($('#cbHorizonte').val() == "1") {
                $('#cntFecha').css("display", "none");
                $('#cntSemana').css("display", "block");
                $('#cntAnio').css("display", "block");
                $('#fechasSemana').css("display", "block");
            }
        },
        error: function () {
            mostrarError();
        }
    });  
};

validarFecha = function () {
    $.ajax({
        type: 'POST',
        url: controlador + 'validarseleccionfecha',
        dataType: 'json',
        data: { fecha: $('#txtFecha').val() },
        success: function (result) {
            $('#contentPlazo').text(result.ValidacionPlazo);           
        },
        error: function () {
            mostrarError();
        }
    });
}

mostrarFechas = function ()
{
    if ($('#cbSemana').val() != "")
    {
        $.ajax({
            type: 'POST',
            url: controlador + 'obtenerfechas',
            dataType: 'json',
            data: { nroSemana: $('#cbSemana').val(), anio: $('#cbAnio').val() },
            success: function (result) {
                $('#txtFechaInicio').text(result.FechaInicio);
                $('#txtFechaFin').text(result.FechaFin);
                $('#contentPlazo').text(result.ValidacionPlazo);
            },
            error: function () {
                mostrarError();
            }
        });
    }
    else
    {
        $('#txtFechaInicio').text("");
        $('#txtFechaFin').text("");
    }
}

descargarFormato = function ()
{
    var mensaje = validacion();
    if (mensaje == "") {
        limpiarMensaje();
        $.ajax({
            type: 'POST',
            url: controlador + 'generarformato',
            dataType: 'json',
            data: {
                anio: $('#cbAnio').val(),
                idEmpresa: $('#cbEmpresa').val(), desEmpresa: $('#cbEmpresa option:selected').text(),
                horizonte: $('#cbHorizonte').val(), semana: $('#cbSemana').val(), fecha: $('#txtFecha').val()
            },
            success: function (result) {
                if (result == "1") {
                    window.location = controlador + 'descargarformato';
                }
                else  {
                    alert(result);
                }
            },
            error: function () {
                mostrarError();
            }
        });
    }
    else {
        mostrarAlerta(mensaje);
    }
}

validacion = function ()
{    
    var validacion = "<ul>";
    var flag = true;
    if ($('#cbEmpresa').val() == "")
    {
        validacion = validacion + "<li>Seleccione empresa.</li>";
        flag = false;
    }
    if ($('#cbHorizonte').val() == "0")
    {
        if ($('#txtFecha').val() == "")
        {
            validacion = validacion + "<li>Seleccione fecha.</li>";
            flag = false;
        }
    }
    if ($('#cbHorizonte').val() == "1")
    {
        if ($('#cbSemana').val() == "")
        {
            validacion = validacion + "<li>Seleccione semana operativa.</li>";
            flag = false;
        }
    }
    validacion = validacion + "</ul>";

    if (flag == true) validacion = "";

    return validacion;
}

mostrarAlerta = function (alerta)
{
    $('#mensaje').removeClass("action-message");
    $('#mensaje').removeClass("action-exito");
    $('#mensaje').addClass("action-alert");
    $('#mensaje').html(alerta);
    $('#mensaje').css("display", "block");
}

limpiarMensaje = function () {
    $('#mensaje').removeClass("action-alert");
    $('#mensaje').removeClass("action-exito");
    $('#mensaje').addClass("action-message");
    $('#mensaje').html("Complete los datos, seleccione y procese archivo.");
}

mostrarExito = function (mensaje)
{
    $('#mensaje').removeClass("action-alert");
    $('#mensaje').removeClass("action-message");
    $('#mensaje').addClass("action-exito");
    $('#mensaje').html(mensaje);
}

mostrarError = function ()
{
    alert("Ha ocurrido un error.");
}

/***CARGA DE DATOS***/

validarArchivo = function () {
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

    var mensaje = validacion();

    if (mensaje == "") {
        $.ajax({
            type: 'POST',
            url: controlador + 'validararchivo',
            dataType: 'json',
            data: {
                anio: $('#cbAnio').val(),
                idEmpresa: $('#cbEmpresa').val(), desEmpresa: $('#cbEmpresa option:selected').text(),
                horizonte: $('#cbHorizonte').val(), semana: $('#cbSemana').val(), fecha: $('#txtFecha').val()
            },
            cache: false,
            success: function (resultado) {
                if (resultado == "OK") {
                    procesarArchivo($('#cbHorizonte').val());
                }
                else if (resultado == "EXISTE") {

                    if (confirm('Existen datos registrados. ¿Desea reemplazar los datos?')) {
                        procesarArchivo($('#cbHorizonte').val());
                    }
                    else
                    {
                        limpiarTodo();
                    }
                }
                else if (resultado == "ERROR") {
                    $('#percentValidacion').addClass('etapa-error');
                    $('#percentValidacion').text("Ha ocurrido un error.");
                }
                else {
                    $('#percentValidacion').addClass('etapa-alert');
                    $('#percentValidacion').text("No ha pasado la validación.");

                    $('#validacion').html(resultado);
                    $('#consulta').html("");

                }
            },
            error: function () {
                $('#percentValidacion').addClass('etapa-error');
                $('#percentValidacion').text("Ha ocurrido un error.");
            }
        });
    }
    else {
        $('#percentValidacion').addClass('etapa-alert');
        $('#percentValidacion').html(validacion);
    }
}

procesarArchivo = function(horizonte) 
{
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
            anio: $('#cbAnio').val(),
            idEmpresa: $('#cbEmpresa').val(), horizonte: horizonte,
            semana: $('#cbSemana').val(), fecha: $('#txtFecha').val()
        },
        success: function (resultado) {
            if (resultado.Resultado == 1) {
                mostrarExito(resultado.Mensaje);
                $('#contentPlazo').html(resultado.Validacion);
                $('#btnProcesarFile').css('display', 'none');
                cargarListado();
                $('#contentEtapa').css("display", "none");
            }
            else {
                $('#percentGrabado').addClass('etapa-error');
                $('#percentGrabado').text(resultado.Mensaje);
            }
        },
        error: function () {
            $('#percentGrabado').addClass('etapa-error');
            $('#percentGrabado').text("Ha ocurrido un error.");
        }
    });
}

loadInfoFile = function(fileName, fileSize) 
{
    $('#fileInfo').removeClass("file-ok");
    $('#fileInfo').removeClass("file-alert");
    if ($('#cbEmpresa').val() != "") {

        $('#fileInfo').html(fileName + " (" + fileSize + "). <strong>Por favor procese el archivo.</strong>");
        $('#fileInfo').addClass("file-ok");
    }
    else
    {
        $('#fileInfo').html(fileName + " (" + fileSize + ")");
        $('#fileInfo').addClass("file-ok");
    }
}

loadValidacionFile= function(mensaje) 
{
    $('#fileInfo').removeClass("file-ok");
    $('#fileInfo').removeClass("file-alert");

    $('#fileInfo').html(mensaje);
    $('#fileInfo').addClass("file-alert");
}

mostrarProgreso = function(porcentaje) 
{
    $('#percentCargaArchivo').text(porcentaje + "%");
}

cargarEtapa = function ()
{
    $('#contentEtapa').css('display', 'block');
    $('#percentCargaArchivo').addClass('etapa-progress')
}

ocultarEtapa = function ()
{
    $('#contentEtapa').css('display', 'none');
}

limpiarTodo = function ()
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

    $('#percentCargaArchivo').html("");
    $('#percentValidacion').html("");
    $('#percentGrabado').html("");
    $('#validacion').html("");

    $('#contentEtapa').css("display", "none");
}

cargarListado = function ()
{
    $.ajax({
        type: 'POST',
        url: controlador + "cargardatos",
        data: {
            anio: $('#cbAnio').val(),
            idEmpresa: $('#cbEmpresa').val(), 
            horizonte: $('#cbHorizonte').val(), fecha: $('#txtFecha').val(), nroSemana: $('#cbSemana').val()
        },
        success: function (evt) {
            $('#consulta').html(evt);
        },
        error: function () {
            alert("Ha ocurrido un error");
        }
    });
    
}

verValidaciones = function ()
{
    $.ajax({
        type: 'POST',
        url: controlador + "validacion",
       
        success: function (evt) {
            $('#validacionDatos').html(evt);

            setTimeout(function () {
                $('#validaciones').bPopup({
                    easing: 'easeOutBack',
                    speed: 450,
                    transition: 'slideDown'
                });
            }, 50);
        },
        error: function () {
            mostrarError();
        }
    });
}