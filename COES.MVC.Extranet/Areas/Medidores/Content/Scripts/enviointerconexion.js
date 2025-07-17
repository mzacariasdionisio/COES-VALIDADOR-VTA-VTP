var controlador = siteRoot + 'Medidores/enviointerconexion/';
var listaPeriodoMedidor = [];
$(function () {

    $('#txtFecha').Zebra_DatePicker({
        onSelect: function () {
            validarFecha();
        }
    });
    $("#cbEmpresa").val($("#hfEmpresa").val());
    validarFecha();
    validacion();
});

validarFecha = function () {
    $.ajax({
        type: 'POST',
        url: controlador + 'ValidarPlazo',
        dataType: 'json',
        data: { fecha: $('#txtFecha').val() },
        success: function (result) {
            if (result.EnPlazo) {
                $('#idcarga').css("display", "block");
            }
            else {
                $('#idcarga').css("display", "none");
            }

            $('#contentPlazo').text(result.ValidacionPlazo);
            $('#cntFecha').css("display", "block");

        },
        error: function () {
            mostrarError();
        }
    });
}

loadValidacionFile = function (mensaje) {
    $('#fileInfo').removeClass("file-ok");
    $('#fileInfo').removeClass("file-alert");

    $('#fileInfo').html(mensaje);
    $('#fileInfo').addClass("file-alert");
}

loadInfoFile = function (fileName, fileSize) {
    $('#fileInfo').removeClass("file-ok");
    $('#fileInfo').removeClass("file-alert");
    if ($('#cbEmpresa').val() != "") {

        $('#fileInfo').html(fileName + " (" + fileSize + "). <strong>Por favor procese el archivo.</strong>");
        $('#fileInfo').addClass("file-ok");
    }
    else {
        $('#fileInfo').html(fileName + " (" + fileSize + ")");
        $('#fileInfo').addClass("file-ok");
    }
}

mostrarProgreso = function (porcentaje) {
    $('#percentCargaArchivo').text(porcentaje + "%");
}

mostrarAlerta = function (alerta) {
    $('#mensaje').removeClass("action-message");
    $('#mensaje').removeClass("action-exito");
    $('#mensaje').addClass("action-alert");
    $('#mensaje').html(alerta);
    $('#mensaje').css("display", "block");
}

mostrarExito = function (mensaje) {
    $('#mensaje').removeClass("action-alert");
    $('#mensaje').removeClass("action-message");
    $('#mensaje').addClass("action-exito");
    $('#mensaje').html(mensaje);
}

limpiarTodo = function () {
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
    $('#agregaPeriodo').html("");

    $('#contentEtapa').css("display", "none");
    $('#listado').css("display", "none");
}

limpiarMensaje = function () {
    $('#mensaje').removeClass("action-alert");
    $('#mensaje').removeClass("action-exito");
    $('#mensaje').addClass("action-message");
    $('#mensaje').html("Complete los datos, seleccione y procese archivo.");
}

validacion = function () {
    var validacion = "<ul>";
    var flag = true;
    if ($('#cbEmpresa').val() == "") {
        validacion = validacion + "<li>Seleccione empresa.</li>";
        flag = false;
    }
    if ($('#txtFecha').val() == "") {
        validacion = validacion + "<li>Seleccione fecha.</li>";
        flag = false;
    }

    validacion = validacion + "</ul>";

    if (flag == true) {
        validacion = "";
        $.ajax({
            type: 'POST',
            url: controlador + 'cargarvariablessesion',
            dataType: 'json',
            data: {
                idEmpresa: $('#cbEmpresa').val(), fecha: $('#txtFecha').val()
            },
            success: function (res) {
            },
            error: function () {
            $('#percentValidacion').text("Ha ocurrido un error.");
        }
    });

    }
    return validacion;
}

cargarEtapa = function () {
    $('#contentEtapa').css('display', 'block');
    $('#percentCargaArchivo').addClass('etapa-progress')
}

cargarListado = function () {
    $.ajax({
        type: 'POST',
        url: controlador + "cargardatos",
        data: {
            idEmpresa: $('#cbEmpresa').val(),fecha: $('#txtFecha').val()
        },
        success: function (evt) {
            $('#consulta').html(evt);
            $('#tabla').dataTable({
                "scrollY": 430,
                "scrollX": true,
                "sDom": 't',
                "ordering": true,
                "iDisplayLength": 96
            });

        },
        error: function () {
            alert("Ha ocurrido un error");
        }
    });

}

ocultarEtapa = function () {
    $('#contentEtapa').css('display', 'none');
}

validarArchivo = function () {
    $('#percentCargaArchivo').removeClass('etapa-progress');
    $('#percentCargaArchivo').addClass('etapa-ok');
    $('#percentCargaArchivo').text("OK...!");

    $('#percentValidacion').removeClass('etapa-progress');
    $('#percentValidacion').removeClass('etapa-ok');
    $('#percentValidacion').removeClass('etapa-error');
    $('#percentValidacion').removeClass('etapa-alert');

    $('#percentValidacion').addClass('etapa-progress');
    $('#percentValidacion').text("Procesando...");

    $('#validacion').html("");
    $('#agregaPeriodo').html("");

    var mensaje = validacion();
    var cadena = vectorPeriodo();
    if (mensaje == "") {
        $.ajax({
            type: 'POST',
            url: controlador + 'validararchivo',
            dataType: 'json',
            data: {
                idEmpresa: $('#cbEmpresa').val(), desEmpresa: $('#cbEmpresa option:selected').text(),
                fecha: $('#txtFecha').val(), periodos: cadena
            },
            cache: false,
            success: function (res) {
                var cadenaResumen = '<table border="1" class="pretty tabla-adicional" cellspacing="0" style="width:500px">';
                cadenaResumen += '<thead><tr><th>Observación</th><th>Cantidad</th></tr></thead><tbody>';
                var totalObservaciones = 0;
                $.each(res.ListaTipoError, function (posicion, tipoError) {
                    if (tipoError.TotalObservacion > 0) {
                        totalObservaciones++;
                        mensaje = tipoError.Mensaje;
                        cadenaResumen += '<tr><td>' + mensaje + '</td><td>' + tipoError.TotalObservacion + '</td></tr>';
                    }
                }
                );
                if (totalObservaciones == 0)
                    cadenaResumen = "";
                else {
                    cadenaResumen += "</tbody></table>";
                    cadenaResumen += '<a href="#" onclick="return mostrarErrores();">Ver detalle</a>';
                }

                switch (res.tipoError) {
                    case 1:
                        $('#percentValidacion').addClass('etapa-alert');
                        $('#percentValidacion').text("No se Confirmó la Recepción.");

                        $('#validacion').html("ERROR");
                        $('#consulta').html("");
                        $('#listado').html(cadenaResumen);
                        $('#listado').css("display", "block");
                       
                        break;
                    case 2:
                        if (confirm('Existen valores nulos. ¿Desea reemplazarlos por blancos?')) {
                            procesarArchivo($('#cbHorizonte').val());
                        }
                        else {
                            limpiarTodo();
                        }
                       
                        break;
                    case 3:
                        procesarArchivo($('#cbHorizonte').val());
                        break;
                    case 4:
                        if (confirm('Existen Datos en Exportaciones e Importaciones en un mismo periodo. ¿Desea continuar?')) {
                            procesarArchivo($('#cbHorizonte').val());
                        }
                        else {
                            limpiarTodo();
                        }
                        break;

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

procesarArchivo = function (horizonte) {
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
            idEmpresa: $('#cbEmpresa').val(), fecha: $('#txtFecha').val()
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

function mostrarErrores() {

    $.ajax({
        type: 'POST',
        url: controlador + "ListarDetalleErrorEnvio",
        success: function (evt) {

            $('#detalleErrores').html(evt);

            setTimeout(function () {
                $('#validaciones').bPopup({
                    easing: 'easeOutBack',
                    speed: 450,
                    transition: 'slideDown',
                    modalClose : false
                });
                $('#tablaerror').dataTable({
                    "scrollY": 430,
                    "scrollX": true,
                    "sDom": 't',
                    "ordering": false,
                    "bPaginate": false,
                    "iDisplayLength": -1
                });

            }, 50);
            $('#alerta').css("display", "none");
            $('#mensaje').css("display", "none");
        },
        error: function () {
            mostrarError();
        }
    });
}

descargarFormato = function () {
    var mensaje = validacion();
    if (mensaje == "") {
        limpiarMensaje();
        $.ajax({
            type: 'POST',
            url: controlador + 'generarformato',
            dataType: 'json',
            data: {
                idEmpresa: $('#cbEmpresa').val(), desEmpresa: $('#cbEmpresa option:selected').text(),
                fecha: $('#txtFecha').val()
            },
            success: function (result) {
                if (result == "1") {
                    window.location = controlador + 'descargarformato';
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
        mostrarAlerta(mensaje);
    }
}

function mostrarPeriodo()
{
    var perIni = 0;
    var len = listaPeriodoMedidor.length;
    if (len > 0) {
        perIni = parseInt(listaPeriodoMedidor[len - 1].Periodofin) + 1;
    }
    if (perIni < 96) {
        $.ajax({
            type: 'POST',
            url: controlador + "IndicarPeriodoMedidor",
            data: {
                periodoIni: perIni
            },
            success: function (evt) {

                $('#opcionPeriodo').html(evt);

                setTimeout(function () {
                    $('#popup2').bPopup({
                        easing: 'easeOutBack',
                        speed: 450,
                        transition: 'slideDown',
                        modalClose: false
                    });
                }, 50);
            },
            error: function () {
                alert("Error en mostrar periodo");
            }
        });
    }
    else {
        alert("Periodo ya se encuentra ingresado");
    }

}

function agregarPeriodo()
{
    var medicodi = $("#cbMedidor").val();
    
    if (medicodi != "") {
        var periodoMedidor = {
            Periodoini: $("#hfpinicial").val(),
            Periodofin: $("#cbPeriodoFin").val(),
            Medicodi: $("#cbMedidor").val(),
            Medinombre: $('#cbMedidor option:selected').text()
        };

        listaPeriodoMedidor.push(periodoMedidor);
        $('#popup2').bPopup().close();
        $('#idTperiodo').html(dibujarTablaPeriodo());
    }
    else {
        alert("Seleccionar Medidor");
    }
}

function eliminarPeriodo()
{
    
    if (listaPeriodoMedidor.length > 0) {
        if (confirm('¿Está seguro de eliminar el periodo?')) {
            listaPeriodoMedidor.pop();
            $('#idTperiodo').html(dibujarTablaPeriodo());
        }
    }
}

function dibujarTablaPeriodo()
{

    var cadena = "<table border='1' class='pretty tabla-adicional' cellspacing='0'>";
    cadena += "<thead><tr><th>Hora Inicio</th><th>Hora Fin</th><th>Medidor</th></tr></thead>";
    cadena += "<tbody>";
    var len = listaPeriodoMedidor.length;
    for (var i = 0 ; i < len ; i++)
    {
        cadena += "<tr><td>" + convetirHoraMin(listaPeriodoMedidor[i].Periodoini) + "</td>";
        cadena += "<td>" + convetirHoraMin(listaPeriodoMedidor[i].Periodofin) + "</td>";
        cadena += "<td>" + listaPeriodoMedidor[i].Medinombre + "</td></tr>";
    }
    cadena += "</tbody></table>";
    return cadena;
}

function convetirHoraMin(horamin)
{
    var hora = "0" + Math.floor((parseInt(horamin) + 1) / 4).toString();
    hora = hora.substring((hora.length > 2) ? 1 : 0, ((hora.length > 2) ? 1 : 0) + 2);
    var minuto = "0" + (Math.floor((parseInt(horamin) + 1) % 4) * 15).toString();
    minuto = minuto.substring((minuto.length > 2) ? 1 : 0, ((minuto.length > 2) ? 1 : 0) + 2);
    return hora + ":" + minuto;
}

function verificaPeriodo()
{
    var perFin = 0;
    var len = listaPeriodoMedidor.length;
    if (len > 0) {
        perFin = parseInt(listaPeriodoMedidor[len - 1].Periodofin) ;
    }
    if (perFin < 95) {
        return 0
    }
    else {
        return 1;
    }
}

function vectorPeriodo()
{
    var cadena = "";
    var len = listaPeriodoMedidor.length;
    
    for (var i = 0; i < len; i++)
    {
        for (var j =  parseInt(listaPeriodoMedidor[i].Periodoini); j <=  parseInt(listaPeriodoMedidor[i].Periodofin); j++)
        {
            
            if (i == 0 && j == 0) {
                cadena += listaPeriodoMedidor[i].Medicodi;
            }
            else {
                cadena += "," + listaPeriodoMedidor[i].Medicodi;
            }
        }
    }
    
    return cadena;
}