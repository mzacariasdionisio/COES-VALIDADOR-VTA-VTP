var controler = siteRoot + "Intervenciones/Registro/";

//variables de informes intervenciones, ayudaran a validar el contenido de las secciones del informe
const ValFechaLarga = "{FECHA_LARGA}";
const ValNroSemOperativa = "{NRO_SEM_OPE}";
const ValDiaAnioAnio = "{DIAANIO_ANIO}";
const ValPorcReservPrim = "{%RESERV_PRIM}";
const ValPrecMaxResevSec = "{PREC_MAX_RSEC}";
const ValVolumenTotal = "{VOL_TOTAL}";
const ValDiaIniSemOper = "{DIA_INISEMOPE}";
const ValSemIniALSemFin = "{SEM_DEL_AL}";
const ValMesAnioSemOperativa = "{MES_ANIOSEMOPE}";
const ValDiaMesAnio = "{DIA_MES_ANIO}";

var lstVaraiablesSecciones = [
    ValFechaLarga, ValNroSemOperativa, ValDiaAnioAnio, ValPorcReservPrim, ValPrecMaxResevSec, ValVolumenTotal, ValDiaIniSemOper, ValSemIniALSemFin, ValMesAnioSemOperativa, ValDiaMesAnio
];

const SEPARADOR_INI_VARIABLE = "{";
const SEPARADOR_FIN_VARIABLE = "}";

$(function () {
    $('#tab-container-config').easytabs({
        animate: false
    });
    //$('#tab-container-config').bind('easytabs:after', function () {
    //   // refrehDatatable();
    //});
    
    $("#GuardarItems").click(function () {
        GuardarItems();
    });

    $('#btnRegresar').click(function () {        
        window.location.href = controler + 'ActualizaReporte';
    });
    
    
    limpiarBarraMensaje("mensaje");
    //mostrarMensaje('mensaje', 'error', 'Ha ocurrido un error al guardar.');
    //mostrarMensaje('mensaje', 'exito', "Los cambios fueron guardados exitosamente...");
});


function GuardarItems() {
    var idrepor = $('#hfidrepor').val();
    var nroItems = $('#hfNrotab').val();

    var filtro = datosSecciones(nroItems, idrepor);
    var msg = validarTextoSecciones(filtro.ListaItems);
 
    if (msg == "") {
         $.ajax({
        type: 'POST',
        url: controler + "GuardarSecciones",
        contentType: "application/json",
        dataType: 'json',
        data: JSON.stringify({
            lista: filtro.ListaItems,
        }
        ),
        cache: false,
        success: function (evt) {
            if (evt.Resultado != "-1") {

                mostrarMensaje('mensaje', 'exito', "Los cambios fueron guardados exitosamente...");

            } else {
                mostrarMensaje('mensaje', 'error', evt.Mensaje);
            }
        },
        error: function (err) {
            mostrarMensaje('mensaje', 'error', 'Ha ocurrido un error al guardar.');
        }
    });
    } else {
        mostrarMensaje('mensaje', 'error', msg);
    }
}

function datosSecciones(nroItems, idrepor) {
    var filtro = {};    

    filtro.ListaItems = ObtenerListaItems(nroItems, idrepor);
    
    return filtro;
}

function ObtenerListaItems(nroItems, idrepor) {
    ///leer la tabla y guardarlos en un arreglo
    var ListaobjSeccion = [];

    for (var i = 1; i <= nroItems; i++) {

        var txtInseccodi = $('#hd_' + i).val();
        var strtxt = '#txt_' + txtInseccodi;
        var strSeccion = '#seccion_' + txtInseccodi;

        //var myContent = tinymce.get(strtxt).getContent();

        var txtInseccontenido = $(strtxt).val();
        var nameseccion = $('#seccion_' + txtInseccodi).val();

        let seccion = {
            "Inseccodi": txtInseccodi,
            "Insecnombre": nameseccion,
            "Inseccontenido": txtInseccontenido.trim(),
            "Inrepcodi": idrepor
        }
        ListaobjSeccion.push(seccion);
    }
    return ListaobjSeccion;
}

function validarTextoSecciones(ListaItems) {
    var msj = "";
    for (key in ListaItems) {
        var item = ListaItems[key];

        var contenidoSeccion = item.Inseccontenido.trim();
        if (contenidoSeccion == "") {
            msj += "<p>Error encontrado en la sección " + item.Insecnombre + " Debe ingresar contenido.</p>";
        }

        var tienenMismaCantidadAsunto = validarCantidadDeSeparadores(contenidoSeccion);

        if (!tienenMismaCantidadAsunto) {
            msj += "<p>Revisar Sección " + item.Insecnombre + ", la cantidad de caracteres '{' no coincide con la cantidad de caracteres '}'. </p>";
        }

        //valido que las palabras dentro de un {} sean variables admitidas
        msj = validarVariablesCorrectas(contenidoSeccion, item.Insecnombre , lstVaraiablesSecciones, msj);
    }
    return msj;
}

function validarCantidadDeSeparadores(campo) {

    //valido que la cantidad de { sea igual al de }
    let regexIni = new RegExp(SEPARADOR_INI_VARIABLE, 'g')
    let regexFin = new RegExp(SEPARADOR_FIN_VARIABLE, 'g')
    /*const regex = /{/g;*/
    const lstSeparadorIniCampo = campo.match(regexIni);
    const lstSeparadorFinCampo = campo.match(regexFin);

    var tienenMismaCantidad = false;
    if (lstSeparadorIniCampo != null) {
        if (lstSeparadorFinCampo != null) {
            if (lstSeparadorIniCampo.length == lstSeparadorFinCampo.length)
                tienenMismaCantidad = true;
        } else {
            tienenMismaCantidad = false;
        }
    } else {
        if (lstSeparadorFinCampo != null) {
            tienenMismaCantidad = false;
        } else {
            tienenMismaCantidad = true;
        }
    }

    return tienenMismaCantidad;
}

function validarVariablesCorrectas(valorCampo, campo, lstVariables, msj) {
    //validando textos tipo: {texto}
    const regex = /{\w+}/g;
    const lstPalabrasDentroParentesis = valorCampo.match(regex);
    if (lstPalabrasDentroParentesis != null) {
        let lstDiferentes = lstPalabrasDentroParentesis.filter(x => !lstVariables.includes(x));
        if (lstDiferentes.length > 0) {
            
            msj += "<p>Revisar Sección " + campo + ". Se detectó variables ( texto dentro de {} ) no reconocidas. </p>";
        }
    }

    //validando textos tipo: {}
    const regex2 = /{}/g;
    const lstPalabrasSoloParentesis = valorCampo.match(regex2);
    if (lstPalabrasSoloParentesis != null) {
        if (lstPalabrasSoloParentesis.length > 0) {
            if (campo == "To")
                campo = "Para";
            msj += "<p>Revisar campo " + campo + ". Se detectó texto ( {} ) no permitido. </p>";
        }
    }

    return msj;
}


function mostrarMensaje(id, tipo, mensaje) {
    $("#" + id).css("display", "block");
    $('#' + id).removeClass();
    $('#' + id).addClass('action-' + tipo);
    $('#' + id).html(mensaje);

}

function limpiarBarraMensaje(id) {
    $('#' + id).css("display", "none");
    $('#' + id).removeClass();
    $('#' + id).addClass('action-message');
    $('#' + id).html('');
}