var controler = siteRoot + "Intervenciones/Parametro/";

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
var hot = null;

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

    $('#btnGrabarInforme').click(function () {
        GuardarItems();
    });

    $('#btnRetornarPersonalizado').click(function () {
        window.location.href = siteRoot + 'intervenciones/registro/programaciones';
    });

    $('#btnRegresar').click(function () {
        window.location.href = controler + 'ActualizaReporte';
    });

    $('#btnGenerarInforme').click(function () {
        imprimirInforme();
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
                tipoReporte: idrepor,
                progcodi: $('#hfCodigoPrograma').val(),
                variables: ($('#hfCodigoPrograma').val() != "0") ? hot.getData() : null
            }),
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
        msj = validarVariablesCorrectas(contenidoSeccion, item.Insecnombre, lstVaraiablesSecciones, msj);
    }

    var progcodi = $('#hfCodigoPrograma').val();

    if (msj == "" && progcodi != "0") {
        var data = hot.getData();
        for (var i = 1; i < data.length; i++) {
            if (data[i][6] == "N") {
                var nuevoValor = data[i][4];
                console.log(nuevoValor);
                if (nuevoValor != null && nuevoValor != "" && !validarNumero(nuevoValor)) {
                    msj = "Ingrese valores correctos para las variables.";
                }
            }
        }
    }

    return msj;
}

function validarNumero(texto) {
    return /^-?[\d.]+(?:e-?\d+)?$/.test(texto);
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

function imprimirInforme() {
    var progCodi = $('#hfCodigoPrograma').val();
    var reporte = $('#hfidrepor').val();
    var idTipoProgramacion = 0;
    var tipo = 0;

    if (reporte == 1) {
        idTipoProgramacion = 2;
        tipo = 1;
    }
    else if (reporte == 2) {
        idTipoProgramacion = 2;
        tipo = 2;
    }
    else if (reporte == 3) {
        idTipoProgramacion = 3;
        tipo = 1;
    }
    else if (reporte == 4) {
        idTipoProgramacion = 3;
        tipo = 2;
    }
    else if (reporte == 5) {
        idTipoProgramacion = 4;
        tipo = 1;
    }

    $.ajax({
        type: 'POST',
        url: siteRoot + 'intervenciones/registro/GenerarWordInforme',
        data: { progrCodi: progCodi, idTipoProgramacion: idTipoProgramacion, tipo: tipo },
        dataType: 'json',
        success: function (result) {
            if (result != -1) {
                //Modificación 15/05/2019
                var paramList = [
                    { tipo: 'input', nombre: 'file', value: result }
                ];
                var form = CreateForm(siteRoot + 'intervenciones/registro/AbrirArchivo', paramList);
                document.body.appendChild(form);
                form.submit();
                return true;
            }
            else {
                alert("Ha ocurrido un error");
            }
        },
        error: function (err) {
            alert("Ha ocurrido un error");
        }
    });
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

function cargarGrillaVariables(result) {
    if (hot != null) {
        hot.destroy();
    }
    var container = document.getElementById('variables');
    var data = result;

    var tituloRenderer = function (instance, td, row, col, prop, value, cellProperties) {
        Handsontable.renderers.TextRenderer.apply(this, arguments);
        td.style.fontWeight = 'bold';
        td.style.fontSize = '11px';
        td.style.textAlign = 'center';
        td.style.color = '#fff';
        td.style.backgroundColor = '#4C97C3';
    };

    var disabledRenderer = function (instance, td, row, col, prop, value, cellProperties) {
        Handsontable.renderers.TextRenderer.apply(this, arguments);
        td.style.backgroundColor = '#F2F2F2';
        td.style.fontSize = '11px';
        td.style.verticalAlign = 'middle';
    };

    var calculoRenderer = function (instance, td, row, col, prop, value, cellProperties) {
        Handsontable.renderers.TextRenderer.apply(this, arguments);
        td.style.backgroundColor = '#FFFFF';
        td.style.fontWeight = 'bold';
        td.style.fontSize = '11px';
        td.style.textAlign = 'center';
    };

    hot = new Handsontable(container, {
        data: data,
        fixedRowsTop: 1,
        colWidths: [1, 200, 200, 200, 200, 200, 1],
        maxRows: result.length,
        cells: function (row, col, prop) {
            var cellProperties = {};

            if (row == 0) {
                cellProperties.renderer = tituloRenderer;
                cellProperties.readOnly = true;
            }

            if (row > 0 && (col <= 3 || col > 4)) {
                cellProperties.renderer = disabledRenderer;
                cellProperties.readOnly = true;
            }

            if (row > 0 && col == 4) {
                cellProperties.renderer = calculoRenderer;
                if (data[row][6] == "N") {
                    cellProperties.format = '0,0.00000';
                    cellProperties.type = 'numeric';
                }
            }

            return cellProperties;
        }
    });

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

function CreateForm(path, params, method = 'post') {
    var form = document.createElement('form');
    form.method = method;
    form.action = path;

    $.each(params, function (index, obj) {
        var hiddenInput = document.createElement(obj.tipo);
        hiddenInput.type = 'hidden';
        hiddenInput.name = obj.nombre;
        hiddenInput.value = obj.value;
        form.appendChild(hiddenInput);
    });
    return form;
}