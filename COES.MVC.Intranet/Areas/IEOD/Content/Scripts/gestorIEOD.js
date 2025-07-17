
var controlador = siteRoot + 'IEOD/Gestor/';
var ruta = siteRoot + 'IEOD/Gestor/';


$(function () {   
   
    $('#btnNuevo').click(function () {
        redactar(0);
    });

    $('#btnEnviarMensaje').click(function () {
        enviarMail(1);
    });

    $('#FchPeriodo').Zebra_DatePicker({
    });  

    $('#cbEmpresa').change(function () {
        setEmpresas(1);

    });
    $('#cbTipoMensaje').change(function () {
        var tip = $('#cbTipoMensaje').val();

        if (parseInt(tip) == 2) {
            $('#F1').show();
            $('#F2').show();
            $('#F3').show();
        }
        else
        {
            $('#F1').hide();
            $('#F1_1').hide();
            $('#F2').hide();
            $('#F3').hide();
        }

    });

    $('#cbFormato').change(function () {
        var n = $('#cbFormato').val();
        if (parseInt(n) == 999) {
            $('#F1_1').show();
        }
        else {
            $('#cbFuenteD').val(0);
            $('#F1_1').hide();
        }

    });

});

function listarEnviados(val) {
    pintarPaginado(1, val);
    listarMensajes(val, 1);
}

function listarRecibidos(val) {
    pintarPaginado(1, val);
    listarMensajes(val, 1);
}
function volver(val) {
    var npagina = $('#hfPaginaActual').val();
      
    listarMensajes(val, 1);
}

function listarMensajes(val, npagina) {
    $.ajax({
        type: 'POST',
        url: controlador + "ListaMail",
        data: {
            id: val,
            nropagina: npagina
        },
        success: function (evt) {            
            $('#contenido').html(evt);
            pintarPaginado(1, val);
        },
        error: function (ex) {          
            alert("Ha ocurrido un error");
        }
    });
}

function listarLog(val) {

    $.ajax({
        type: 'POST',
        url: controlador + "ListaLog",
        data: {
            id: val
        },
        success: function (evt) {
            $('#footer-log').html(evt);            
        },
        error: function (ex) {
            alert("Ha ocurrido un error");
        }
    });
}

function verContenido(val) {

    $.ajax({
        type: 'POST',
        url: controlador + "MailDetalle",
        data: {
            id: val
        },

        success: function (evt) {           
            $('#contenido').html(evt);
            $('#ContentMsgDet').summernote({
                lang: 'es-ES',
                height: 200
            });
            
            var mensaj = llenarMensaje(1);
            $('.summernote').summernote('code', mensaj);

        },
        error: function (ex) {          
            alert("Ha ocurrido un error");
        }
    });
}

function redactar(val) {

    $.ajax({
        type: 'POST',
        url: controlador + "Redactar",
        data: {
            id: val
        },

        success: function (evt) {
            
            $('#contenido').html(evt);         
           $('#ContentMsg').summernote({
                lang: 'es-ES',
                height: 200
           });
           llenarMensaje(1);

        },
        error: function (ex) {           
            alert("Ha ocurrido un error");
        }
    });
}

function enviarMail(val) {
   
    var valido = 1;
    var msj = $('#ContentMsg').summernote('code');

    var selecionado = "";

    var regex = new RegExp("\"", "g")
    var newMsj = '"' + msj.replace(regex, "'") + '"'
    var TMPmensaje = msj.replace(/</g, "¬");

    var tip = $('#cbTipoMensaje').val();

    if ($('#CorreoDest').val() == "") {
        valido = 0;       
        toastr.warning('Debe Ingresar el correo de destino');
    }
    if (valido == 1) {
        if ($('#asuntoM').val() == "") {
            valido = 0;          
            toastr.warning('Debe Ingresar el asunto');
        }
    }
    if (valido == 1) {
        if ($('#cbTipoMensaje').val() == 0) {
            valido = 0;            
            toastr.warning('Debe seleccionar un tipo de mensaje');
        }
    }
    if (valido == 1) {
        if (parseInt(tip) == 2) {
            if ($('#cbFormato').val() == 0) {
                valido = 0;                
                toastr.warning('Debe seleccionar un formato');
            }
            if ($('#cbFormato').val() == 999) {
                if ($('#cbFuenteD').val() == 0) {
                    valido = 0;                    
                    toastr.warning('Debe seleccionar un fuente de datos');
                }
            }

            if (valido == 1) {
                if ($('#FchPeriodo').val() == "") {
                    valido = 0;                    
                    toastr.warning('Debe Ingresar la fecha del periodo');
                }
            }
            if (valido == 1) {
                if ($('#FchPlazo').val() == "") {
                    valido = 0;                   
                    toastr.warning('Debe Ingresar la fecha de ampliacion');
                }
            }

            if ($('#cbFormato').val() > 0 && $('#cbFormato').val() < 999) {
                selecionado = $("#cbFormato option:selected").text();
            } else {
                selecionado = $("#cbFuenteD option:selected").text();
            }            
        }
    }


    if (valido == 1) {
        
        $.ajax({
            type: 'POST',
            url: controlador + "EnviarMail",
            data: {
                Correo: $('#CorreoDest').val(),
                Asunto: $('#asuntoM').val(),
                TipoCorreo: $('#cbTipoMensaje').val(),
                EstMsg: 1,
                Mensaje: TMPmensaje,
                Periodo: $('#FchPeriodo').val(),
                FormatCodi: $('#cbFormato').val(),
                idFuente: $('#cbFuenteD').val(),
                FchAmpl: $('#FchPlazo').val(),
                seleccion: selecionado
            },

            success: function (evt) {
                toastr.success('Correo Enviado');              
                listarMensajes(1, 1);


            },
            error: function (ex) {                
                alert("Ha ocurrido un error");
            }
        });

    }
    listarLog(1);
}

function setEmpresas(val)
{
    var flag = 0;
    var codi = $('#cbEmpresa').val();
    var nomb = $("#cbEmpresa option:selected").text();    

    $.ajax({
        type: 'POST',
        url: controlador + 'SetEmpresa',
        dataType: 'json',
        data: { emprcodi: codi, nomEmpr: nomb },
        cache: false,
        global: false,
        success: function (aData) {
            flag = 1;
        },
        error: function () {
            alert("Ha ocurrido un error");
        }
    });       

}

function llenarMensaje(val)
{
    var mensaje = "";

    $.ajax({
        type: 'POST',
        url: controlador + "GetMensaje",
        data: {
            id: val
        },

        success: function (evt) {            
            $('.summernote').summernote('code', evt);
            $('#ContentMsgDet').summernote('disable');         
        },
        error: function (ex) {
            
            alert("Ha ocurrido un error al leer el mensaje");
        }
    });

    return mensaje;
}


function evalua(idestado, idMensaje ) {

    var msj = $('#ContentMsgDet').summernote('code');

    var regex = new RegExp("\"", "g")
    var newMsj = '"' + msj.replace(regex, "'") + '"'
    var TMPmensaje = msj.replace(/</g, "¬");

    $.ajax({
        type: 'POST',
        url: controlador + "AprobarRechazar",
        data: {
            MsgCodi: idMensaje,
            estado: idestado,
            Texto: TMPmensaje
        },

        success: function (evt) {
            $("#btnAP").disabled = true;
            $("#btnRZ").disabled = true;

            if (idestado == 2)
            {              
                toastr.success('La solicitud fue aceptada correctamente');
            }
            if (idestado == 3) {                
                toastr.success('La solicitud ha sido rechazada correctamente');
            }
            /*ListarMensajes(1);*/


        },
        error: function (ex) {           
            toastr.error('Ha ocurrido un error');           
        }
    });

}

function pintarPaginado(id, val) {
    $.ajax({
        type: 'POST',
        url: controlador + "paginado",
        data: {
            id: id,
            orden: val,
        },
        success: function (evt) {
            $('#paginado').html(evt);
            mostrarPaginado();
        },
        error: function () {
            alert("Ha ocurrido un error paginado");
        }
    });
}