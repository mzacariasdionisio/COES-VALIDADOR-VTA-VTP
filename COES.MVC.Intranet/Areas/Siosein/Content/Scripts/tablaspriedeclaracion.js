var controlador = siteRoot + 'Siosein/TablasPrieDeclaracionMen/';
var ALTURA_HANDSON = 900;

$(function () {        
    $('.txtFecha').Zebra_DatePicker({
        format: 'm Y'
    });

    $('#btnConsultar').click(function () {
        $("#hfIdEnvio").val("0");
        mostrarExcelWeb();
    });

    $('#btnMostrarErrores').click(function () {
        mostrarDetalleErrores();
    });

    $('#btnEnviarDatos').click(function () {
        if (typeof hot != 'undefined') {
            enviarExcelWeb();
        }
    });

    $('#btnVerEnvios').click(function () {
        popUpListaEnvios();
    });

    $("#hfIdEnvio").val("0");
    mostrarExcelWeb();
});

function mostrarEnvioTablaPrie01(envio) {
    $('#enviosanteriores').bPopup().close();
    $("#hfIdEnvio").val(envio);

    mostrarFormulario(envioAnterior);
}

function enviarExcelWeb() {
    if (hot.getData().length > 2) {
        if (confirm("¿Desea enviar información a COES?")) {
            //if (validarEnvio()) {
            var fecha = $('#txtFecha').val();
            $.ajax({
                type: 'POST',
                dataType: 'json',
                //async: false,
                contentType: 'application/json',
                traditional: true,
                url: controlador + "GrabarDatosTablaPrie01dec",
                data: JSON.stringify({
                    data: hot.getData(),
                    fecha: fecha
                }),
                beforeSend: function () {
                    mostrarExito("Enviando Información ..");
                },
                success: function (evt) {
                    if (evt.Resultado == 1) {
                        $("#hfIdEnvio").val(evt.Resultado);
                        mostrarFormulario();
                        //hideMensaje();
                        mostrarExito("Los datos se enviaron correctamente");
                    }
                    else {
                        mostrarError("Error al Grabar");
                    }
                },
                error: function () {
                    mostrarError();
                }
            });
            //}
        }
    } else { alert("Excel Web vacio"); }
}

function mostrarDetalleErrores() {
    $('#idTerrores').html(dibujarTablaError());
    setTimeout(function () {
        $('#validaciones').bPopup({
            easing: 'easeOutBack',
            speed: 450,
            transition: 'slideDown',
            modalClose: false
        });
        $('#tablaError').dataTable({
            "scrollY": 330,
            "scrollX": true,
            "sDom": 't',
            "ordering": false,
            "bPaginate": false,
            "iDisplayLength": -1
        });

    }, 50);
}

//Muestra la barra de herramemntas para administrar los datos de declaración mensual ingresados
function mostrarExcelWeb() {
    if ($("#txtFecha").val() != "") {
        $('#mensajeEvento').css("display", "none");
        showMensaje();
        $('#barraDeclaracion').css("display", "block");
        mostrarFormulario(consulta);
    }
    else {
        alert("Error!.Ingresar fecha correcta");
    }
}

function mostrarFormulario(accion) {
    listErrores = [];    
    var fecha = $("#txtFecha").val();
    var idEnvio = $("#hfIdEnvio").val();
    if (typeof hot != 'undefined') {
        hot.destroy();
    }

    ALTURA_HANDSON = parseInt($(".content-hijo").height());

    $.ajax({
        type: 'POST',
        url: controlador + "MostrarGridExcelWeb",
        dataType: 'json',
        //async: false,
        data: {            
            idEnvio: idEnvio,
            fecha: fecha,
            tabla: 1,
            tipo:1
        },
        success: function (evt) {
            if (evt != -1) {
                evtHot = evt;
                excelWebPotenciaFirme(evt, ALTURA_HANDSON);
                
                switch (accion) {
                    case envioDatos:
                        //var mensaje = mostrarMensajeEnvio(idEnvio);
                        mostrarExito("Los datos se enviaron correctamente.");
                        hideMensaje();
                        break;
                    case envioAnterior:
                        var mensaje = mostrarMensajeEnvio(idEnvio);
                        mostrarExito(mensaje);
                        hideMensaje();
                        break;
                    case consulta:
                        mostrarExito("Informacion Exitosa!!");
                        //var mensaje = mostrarMensajeEnvio();
                        //mostrarMensaje("Por favor complete los datos. <strong>Plazo del Envio: </strong>" + mensaje);
                        break;
                    case importarDatos:
                        mostrarExito("<strong>Los datos se cargaron correctamente, por favor presione el botón enviar para grabar.</strong>");
                        break;
                }
               // mostrardetalleExcel(evt.Empresa, evt.Fecha, evt.FechaNext);
            }
            else {
                alert("La empresa no tiene puntos de medición para cargar.");
                return;
            }
        },
        error: function () {
            alert("Error al cargar Excel Web");
        }
    });
}

function mostrarAlerta(mensaje) {
    $('#mensaje').removeClass();
    $('#mensaje').html(mensaje);
    $('#mensaje').addClass('action-alert');
}

function mostrarExito(mensaje) {
    $('#mensaje').removeClass();
    $('#mensaje').html(mensaje);
    $('#mensaje').addClass('action-exito');
}

function mostrarError(mensaje) {
    $('#mensaje').removeClass();
    $('#mensaje').html(mensaje);
    $('#mensaje').addClass('action-error');
}

function mostrarMensaje(mensaje) {
    $('#mensaje').removeClass();
    $('#mensaje').html(mensaje);
    $('#mensaje').addClass('action-message');
}

function mostrarMensajeEnvio() {

    var envio = $("#hfIdEnvio").val();
    if (envio > 0) {
        var plazo = (evtHot.EnPlazo) ? "<strong style='color:green'>en plazo</strong>" : "<strong style='color:red'>fuera de plazo</strong>";
        var mensaje = "<strong>Código de envío</strong> : " + evtHot.IdEnvio + "   , <strong>Fecha de envío: </strong>" + evtHot.FechaEnvio + "   , <strong>Enviado en </strong>" + plazo;
        return mensaje;
    }
    else {
        if (!evtHot.EnPlazo) {
            return "<strong style='color:red'>Fuera de plazo</strong>";
        }
        else return "<strong style='color:green'>En plazo</strong>";
    }
}

function limpiarError() {
    totLimInf = 0;
    totLimSup = 0;
    totNoNumero = 0;
    listErrores = [];
}