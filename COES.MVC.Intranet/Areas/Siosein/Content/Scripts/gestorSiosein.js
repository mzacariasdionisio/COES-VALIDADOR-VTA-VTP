// scrips relacionados => "globales.js"
var controlador = siteRoot + 'Siosein/Gestor/';
var ruta = siteRoot + 'Siosein/Gestor/';

const MENSAJE_ESTADO_ACTIVO = 1;
const PARAM_DEFECTO = -1;

const MENSAJES = Object.freeze({
    RECIBIDOS: 1,
    ENVIADOS: 2,
    ELIMINADOS: 3
});

$(function () {

   $('#btnNuevo').click(function () {
        Redactar(0,1);
    });

    $('#btnEnviarMensaje').click(function () {
        EnviarMail(1);
    });

    $('#FchPeriodo').Zebra_DatePicker();

    $('#idAgregarCarpeta').click(function () {
        grabarNuevaCarpeta();/**/
    });

    $('#idCancelarCarpeta').click(function () {
        cancelar();
    });

    $('#cbTablaMensaje').change(function () {
        var dato = $('#cbTablaMensaje').val();
        getEmailResponsable(dato);
    });

    $("#idCancelar").click(function () {
        $('#AgregarCarpeta').bPopup().close();
    });

    ListarRecibidos();
});

function ListarEnviados() {
    ListarMensajes(MENSAJES.ENVIADOS, PARAM_DEFECTO, PARAM_DEFECTO, PARAM_DEFECTO);
    $('#hfcarpetaActual').val(2);
}

function ListarRecibidos() {
    ListarMensajes(MENSAJES.RECIBIDOS, PARAM_DEFECTO, PARAM_DEFECTO, PARAM_DEFECTO);
    $('#hfcarpetaActual').val(1);
}

function ListarEliminados() {
    ListarMensajes(MENSAJES.ELIMINADOS, PARAM_DEFECTO, PARAM_DEFECTO, PARAM_DEFECTO);
    $('#hfcarpetaActual').val(3);
}

function ListarxCategoria(categoria) {
    ListarMensajes(MENSAJES.RECIBIDOS, categoria, PARAM_DEFECTO, PARAM_DEFECTO);
    $('#hfcategoriaActual').val(categoria);
}

function ListarxCarpeta(carpeta) {
    ListarMensajes(MENSAJES.RECIBIDOS, PARAM_DEFECTO, carpeta, PARAM_DEFECTO);
    $('#hfcarpetaActual').val(carpeta);
}

function Volver(val) {
    var npagina = $('#hfPaginaActual').val();
    /*pintarPaginado(1, val);*/
    var fecha2 = '1/' + $('#FchPeriodoIni').val();

    ListarMensajes(val, 1, 0, 1, 0, fecha2);
    if (parseInt(val) == 2) {
        ListarMensajes(val, 1, 0, 2, 0, fecha2);
    }
}

function ListarMensajes(val, tipomensaje, carpeta, estado) {
    var periodo = $('#FchPeriodoIni').val();  
    $.ajax({
        type: 'POST',
        url: controlador + "listmail",
        data: {
            id: val,
            tipomensaje: tipomensaje,
            carpeta: carpeta,
            estmsgcodi: estado,
            periodo: periodo
        },
        success: function (evt) {
            $('#contenido').html(evt);
        },
        error: function (ex) {
            alert("Ha ocurrido un error");
        }
    });
}

function ListarLog(val) {

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
        url: controlador + "MailDetail",
        data: {
            id: val
        },

        success: function (evt) {
            /*$('#listadoMensajes').hide();
            $('#contenido').css("width", $('#mainLayout').width() + "px");*/
            $('#contenido').html(evt);
            $('#ContentMsgDet').summernote({
                /**/ lang: 'es-ES',
                height: 200
            });

            var mensaj = llenar_mensaje(1);

            $('.summernote').summernote('code', mensaj);

        },
        error: function (ex) {
            /* alert(ex);*/
            alert("Ha ocurrido un error");
        }
    });
}

function Redactar(val, tipo) {
    var fecha2 = $('#FchPeriodoIni').val();

    $.ajax({
        type: 'POST',
        url: controlador + "Redactar",
        data: {
            id: val,
            periodo: fecha2,
            tipo: tipo 
        },

        success: function (evt) {
            /*$('#listadoMensajes').hide();
            $('#contenido').css("width", $('#mainLayout').width() + "px");*/
            $('#contenido').html(evt);
            /*  $('.summernote').summernote({*/
            $('#ContentMsg').summernote({
                /**/ lang: 'es-ES',
                height: 200
            });
            llenar_mensaje(1);

        },
        error: function (ex) {
            /* alert(ex);*/
            alert("Ha ocurrido un error");
        }
    });
}

function EnviarMail(val) {

    var msj = $('#ContentMsg').summernote('code');

    var regex = new RegExp("\"", "g");
    var TMPmensaje = msj.replace(/</g, "¬");

    var valido = validacionEnviarMail();

    if (valido === 1) {

        var data = {
            Msgto: $('#CorreoDest').val(),
            Msgasunto: $('#asuntoM').val(),
            Msgestado: MENSAJE_ESTADO_ACTIVO,
            Msgcontenido: TMPmensaje,
            Formatcodi: $('#cbTablaMensaje').val(),
            Tmsgcodi: $('#cbcategoriaMensaje').val(),
        };

        var dataEnvio = {
            simensaje: data,
            periodo: $('#FchPeriodo').val()
        };

        $.ajax({
            type: 'POST',
            url: controlador + "EnviarMail",
            data: JSON.stringify(dataEnvio),
            contentType: "application/json",
            dataType: "json",
            success: function (evt) {
                toastr.success('Correo Enviado');
                var fecha2 = '1/' + $('#FchPeriodoIni').val();
                ListarMensajes(1, 1, 0, 1, 1, fecha2);
            },
            error: function (ex) {
                /* alert(ex);*/
                alert("Ha ocurrido un error");
            }
        });

    }
    ListarLog(1);
}

function validacionEnviarMail() {
    var valido = 1;
    var tip = $('#cbTipoMensaje').val();
    if ($('#CorreoDest').val() === "") {
        valido = 0;
        toastr.warning('Debe Ingresar el correo de destino');
    }
    if (valido === 1) {
        if ($('#asuntoM').val() === "") {
            valido = 0;
            toastr.warning('Debe Ingresar el asunto');
        }
    }
    if (valido === 1) {
        if ($('#cbcategoriaMensaje').val() === 0) {
            valido = 0;
            toastr.warning('Debe seleccionar un tipo de mensaje');
        }
    }
    if (valido === 1) {
        if (parseInt(tip) === 2) {
            if ($('#cbTablaMensaje').val() === 0) {
                valido = 0;
                toastr.warning('Debe seleccionar una Tabla');
            }
            if (valido === 1) {
                if ($('#FchPeriodo').val() === "") {
                    valido = 0;
                    toastr.warning('Debe Ingresar la fecha del periodo');
                }
            }
        }
    }
    return valido;
}

function llenar_mensaje(val) {
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

function AgregarCarpeta(val) {

    setTimeout(function () {
        $('#AgregarCarpeta').bPopup({
            easing: 'easeOutBack',
            speed: 450,
            transition: 'slideDown',
            modalClose: false
        });
    }, 50);
}

function cancelar() {
    $('#AgregarCarpeta').bPopup().close();
}

function grabarNuevaCarpeta() {

    var nomCarpeta = $('#txtNombreCarperta').val();
    
    if (nomCarpeta == "") {
        toastr.warning('Debe ingresar un nombre para la carpeta');
        return;
    }
    
    $.ajax({
        type: 'POST',
        url: controlador + 'GuardarCarpeta',
        dataType: 'json',
        data: {
            nomCarpeta: nomCarpeta
        },
        cache: false,
        success: function (model) {
            if (model.ResultadoInt > 0) {
                $('#AgregarCarpeta').bPopup().close();
                toastr.success(model.Mensaje);
                ListarCarpetas(1);
            }
            else {
                alert("Error al grabar equipo");
            }
        },
        error: function () {
            alert("Error al grabar equipo");
        }

    });

}

function ListarCarpetas(val) {
    var fecha2 = '1/' + $('#FchPeriodoIni').val();
    $.ajax({
        type: 'POST',
        url: controlador + "ListaCarpetas",
        data: {
            id: val,
            periodo: fecha2
        },
        success: function (evt) {
            $('#ListaCarpertaUser').html(evt);

        },
        error: function (ex) {
            alert("Ha ocurrido un error");
        }
    });
}

function MoverACarpeta(val) {

    // para cada checkbox "chequeado"
    var msgcodi = $("input[name='chbxMensajes']:checked").map(function () { return this.value; }).get().join(",");


    if (msgcodi === "") {
        alert("Debe seleccionar almenos un mensaje para mover");
    } else
    {
        if (confirm('¿Estas seguro de mover los mensajes a la carpeta seleccionada?')){
           
        $.ajax({
            type: 'POST',
            url: controlador + "MoveraCarpeta",
            data: {                
                carpeta: val,                
                seleccion: msgcodi
            },

            success: function (evt) {
                toastr.success('Se movieron los mensajes seleccionados correctamente');
              
                var fecha2 = '1/' + $('#FchPeriodoIni').val();
                ListarCarpetas(1);
                ListarMensajes(1, 1, 1, 0, 0, fecha2);


            },
            error: function (ex) {
                /* alert(ex);*/
                alert("Ha ocurrido un error");
            }
        });
}

    }

   
}

function getEmailResponsable(val)
{
    var mensaje = "";

    $.ajax({
        type: 'POST',
        url: controlador + "GetEmailResponsable",
        data: {
            id: val
        },

        success: function (evt) {
            $("#CorreoDest").val(evt);
            $("#asuntoM").val("TABLA" + val + "-");
        },
        error: function (ex) {

            alert("Ha ocurrido un error al leer el mensaje");
        }
    });

    return mensaje;
}


//#region CAMBIOS SIOSEIN

/** Permite eliminar los mensajes seleccionados */
function eliminarMensajeSeleccionados() {

    var codMensajes = obtenerMensajesSeleccionado();
    if (codMensajes === "") {
        alert("Debe seleccionar almenos un mensaje");
        return;
    }

    $.ajax({
        type: 'POST',
        url: `${controlador}EliminarMensaje`,
        data: {
            msgcodi: codMensajes
        },
        success: function (evt) {
            $('#contenido').html(evt);
        },
        error: function (ex) {
            alert("Ha ocurrido un error");
        }
    });
}

/** Permite obtener codigo de los mensajes selecionados
 *@return {string} retorna codigo mensaje unido por comas
 */
function obtenerMensajesSeleccionado() {
    var msgcodi = $("input[name='chbxMensajes']:checked").map(function () { return this.value; }).get().join(",");
    return msgcodi;
}


//#endregion