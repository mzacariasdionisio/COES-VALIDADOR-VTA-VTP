var anchoPortal = 1035; //1200px es el ancho del portal. Este debe ser el mismo que se mostrará en Intranet
var OPCION_ACTUAL = 0;
var TIPO_SUBESTACION = -2;
var DETALLE_FT_GLOBAL = null;
const COLOR_CELDA = "#CBFBE6";
var IMG_DATO_ARCHIVO = '<img src="' + siteRoot + 'Content/Images/file.png" title="Archivo" width="19" height="19" style="">';
var IMG_NOTA_LLENA = '<img src="' + siteRoot + 'Content/Images/btn-notaLlena2.png" title="Detalle del Parámetro" width="19" height="19" style="">';
var IMG_NOTA_VACIA = '<img src="' + siteRoot + 'Content/Images/btn-notaVacia.png" title="Detalle del Parámetro" width="19" height="19" style="">';
var IMG_CONFIDENCIALIDAD = '<img src="' + siteRoot + 'Content/Images/DownFileConfidencial.png" title="Valor Confidencial" width="19" height="19" style="">';

//Generar html
function _extranet_generarHtmlReporteDetalleFichaTecnica(model) {
    var raiz = -1;
    var listaDataXNivel = model.ListaAllItems.filter(x => x.Nivel == 1);

    //obtener profundidad horizontal
    var colspan = _extranet_profundidadHorizontalRecursivo(listaDataXNivel);

    var htmlCabecera = ` 
            <tr>
                <td colspan='${colspan}'></td>
                <td style='width: 90px;' class='cabecera'>Valor</td>
        `;
    if (mostrarColumnaValorConfidencial) {
        htmlCabecera += `
                <td style='width: 21px;' class='cabecera'>${IMG_CONFIDENCIALIDAD}</td>
        `;
    }

    if (mostrarColumnaSustento) {
        htmlCabecera += `
                <td style='width: 74px;' class='cabecera'>Adjuntar Sustento</td>
        `;
    }
    if (mostrarColumnaInstructivoLlenado) {
        htmlCabecera += `
                <td style='' class='cabecera'>Instructivo de llenado</td>
        `;
    }
    htmlCabecera += `
            </tr>
        `;

    var htmlItem = _extranet_getHtmlItem(model.ListaAllItems, model.ListaTreeItems, model.ListaItemConfig, raiz, true, 1, colspan, colspan);

    var htmlNota = '';
    if (model.ListaNota.length > 0) {
        htmlNota = _extranet_getHtmlNota(model.ListaNota, model.ListaAllItems);
    }

    var html = `
        <table id="reporte" class="pretty tabla-icono tree_ficha_tecnica" style="overflow-y: hidden; overflow-x: auto; width: ${anchoPortal}px; table-layout: fixed;">
            <tbody>
                ${htmlCabecera}
                ${htmlItem}
                ${htmlNota}
            </tbody>
        </table>
    `;

    return html;
}

function _extranet_profundidadHorizontalRecursivo(listaAllData) {
    var tipoAgrup = 1;

    var listaProfundidad = [];
    if (listaAllData != null) {
        listaAllData.sort((x, y) => x.Ftitorden - y.Ftitorden); // ordenamiento

        for (var i = 0; i < listaAllData.length; i++) {
            var reg = listaAllData[i];

            if (reg.Nivel == 1) { //raiz
                var profundidadRecursivo = _extranet_profundidadHorizontalRecursivo(reg.ListaHijos);
                listaProfundidad.push((1 + COL_ADICIONAL + profundidadRecursivo));
            } else {
                if (tipoAgrup == reg.Ftittipoitem) {
                    var profundidadRecursivo = _extranet_profundidadHorizontalRecursivo(reg.ListaHijos);
                    listaProfundidad.push((2 + profundidadRecursivo));
                } else {
                    if (reg.ListaHijos.length > 0) {
                        var profundidadRecursivo = _extranet_profundidadHorizontalRecursivo(reg.ListaHijos);
                        listaProfundidad.push((1 + profundidadRecursivo));
                    } else {
                        listaProfundidad.push(3);
                    }
                }
            }
        }
    }

    if (listaProfundidad.length > 0) {
        var m = _extranet_MyMax(listaProfundidad);
        return m;
    }
    else {
        return 0;
    }

}

function _extranet_getHtmlItem(listaAllData, listaTreeData, listaItemConfig, idPadre, iniciaNuevaFila, nivel, numMaxCol, colspanDisponible) {

    var tipoRaiz = -1;
    var tipoAgrup = 1;

    listaTreeData.sort((x, y) => x.Ftitorden - y.Ftitorden); // ordenamieto
    var hijos = listaTreeData

    var claseAgrupamiento = "";
    var clasePropiedad = "propiedad";

    if (tipoRaiz == idPadre) {
        claseAgrupamiento = "agrup_raiz";
    }
    else {
        claseAgrupamiento = "agrup_hijo";
    }

    var htmlItem = '';

    for (var j = 0; j < hijos.length; j++) {

        var reg = hijos[j];
        var orden = reg.Orden;

        var configItem = listaItemConfig.find(x => x.Ftitcodi === reg.Ftitcodi);

        if (tipoAgrup == reg.Ftittipoitem) {

            if (reg.Nivel == 1 || reg.ListaHijos.length == 0) {

                //obtener profundidad vertical
                var rowspan = _extranet_profundidadVerticalRecursivo(reg.ListaHijos);
                //
                if (iniciaNuevaFila) {
                    htmlItem += `<tr>`;
                }
                var colspanFila = colspanDisponible - (reg.Nivel == 1 ? 1 : 2) + 1; //se agrega 1 por la columna adicional de valor 
                colspanFila += COL_ADICIONAL; //se agrega columnas adicionales de sustento y del instructivo
                htmlItem += `
                     <td class="${claseAgrupamiento} orden">${orden}</td>
                     <td colspan="${colspanFila}" class="${claseAgrupamiento} nombre">${reg.Ftitnombre} <span class="nota_agrup">${reg.ListaNotanum}</span> </td>
                     </tr>
                `;

                iniciaNuevaFila = true;

                if (rowspan > 0) {
                    var colspanDisponibleSig = reg.Nivel == 1 ? numMaxCol : colspanDisponible - 1;

                    htmlItem += `
                    <tr>
                        <td rowspan="${rowspan}"></td>
                        ${_extranet_getHtmlItem(listaAllData, reg.ListaHijos, listaItemConfig, reg.Ftitcodi, false, reg.Nivel + 1, numMaxCol, colspanDisponibleSig)}
                `;
                }

            }
            else {
                //obtener profundidad vertical
                var rowspan = _extranet_profundidadVerticalRecursivo(reg.ListaHijos);
                //
                if (iniciaNuevaFila) {
                    htmlItem += `<tr>`;
                }

                htmlItem += `
                     <td rowspan="${rowspan}" class="${claseAgrupamiento} orden">${orden}</td>
                     <td rowspan="${rowspan}" class="${claseAgrupamiento} nombre">${reg.Ftitnombre} <span class="nota_agrup">${reg.ListaNotanum}</span></td>
                `;

                iniciaNuevaFila = true;

                if (rowspan > 0) {
                    var colspanDisponibleSig = colspanDisponible - 2;

                    htmlItem += `
                        ${_extranet_getHtmlItem(listaAllData, reg.ListaHijos, listaItemConfig, reg.Ftitcodi, false, reg.Nivel + 1, numMaxCol, colspanDisponibleSig)}
                    `;
                }
            }

        }
        else {
            //obtener profundidad vertical
            var rowspan = _extranet_profundidadVerticalRecursivo(reg.ListaHijos);
            if (reg.ListaHijos.length > 0) rowspan++;
            rowspan = rowspan > 0 ? rowspan : 1;

            if (iniciaNuevaFila) {
                htmlItem += `<tr>`;
            }

            var htmlItemUnidad = _extranet_getHtmlItemUnidad(reg.ItemUnidadDesc);

            var htmlItemValor = '';
            if (OPCION_ACTUAL != 2) {
                htmlItemValor = _extranet_getHtmlItemValor(reg, configItem);
            }

            //var htmlcomentario = ``;
            //if (reg.Itemcomentario != null && reg.Itemcomentario != "") {
            //    htmlcomentario += `<img src="` + siteRoot + `Content/Images/ico-info.gif" title="Informado por el Agente: \n${reg.Itemcomentario}" width="16" height="16" style ="vertical-align: bottom;">`;
            //}

            var colspanDispItem = colspanDisponible - 3;
            //if (reg.ListaHijos.length > 0) colspanDispItem = colspanDisponible - 4;

            //Coloreo si la propiedad o concepto tenga check "Mostrar en color verde claro celda FT"
            var colorCelda = "";
            var valCheck = 0;

            if (reg.Concepcodi != null) {
                valCheck = reg.Concepflagcolor; //1 si esta con check
            }

            if (reg.Propcodi != null) {
                valCheck = reg.Propflagcolor; //1 si esta con check
            }

            //Si tiene marcado el check,pinto de color verde claro
            if (valCheck == 1) {
                colorCelda = COLOR_CELDA;
            }

            var boolDeshabilitar = !OPCION_GLOBAL_EDITAR || reg.EsReplica;
            var htmlanotacion = _extranet_crearHtmlAnotacionComentario(reg, boolDeshabilitar);
            var htmlInputAnotacion = _extranet_crearHtmlInputAnotacion(reg);
            var htmlBtnVerInstructivo = _extranet_crearHtmlVerInstructivo(reg, configItem);
            var htmlDisabled = boolDeshabilitar ? " disabled " : "";

            var mostrarOpcionValor = _evaluarMostrarInputValor(reg);
            htmlItem += `
                     <td rowspan="${rowspan}" style="background: ${colorCelda};" class="${clasePropiedad} orden">${orden}</td>
            `;

            //1. Columna "Valor""
            if (mostrarOpcionValor) {
                //para agregar o no boton de COMENTARIO

                var mostrarComentario = configItem.Fitcfgflagcomentario;

                if (mostrarComentario == "S") {
                    htmlItem += `
                     <td colspan="${colspanDispItem}" style="background: ${colorCelda};" class="${clasePropiedad} nombre">${reg.Ftitnombre} <span class="nota_item">${reg.ListaNotanum} </span> ${htmlanotacion}</td>
                    `;
                } else {
                    htmlItem += `
                     <td colspan="${colspanDispItem}" style="background: ${colorCelda};" class="${clasePropiedad} nombre">${reg.Ftitnombre} <span class="nota_item">${reg.ListaNotanum} </span></td>                     
                    `;
                }
            } else {
                htmlItem += `
                     <td colspan="${colspanDispItem}" style="background: ${colorCelda};" class="${clasePropiedad} nombre">${reg.Ftitnombre} <span class="nota_item">${reg.ListaNotanum} </span></td>                     
                `;
            }

            htmlItem += `
                    
                     <td class="${clasePropiedad} unidad" style="background: ${colorCelda};">${htmlItemUnidad}</td>
            `;
            if (mostrarOpcionValor) {
                //para agregar input oculto para almacenar COMENTARIO
                var mostrarComentario = configItem.Fitcfgflagcomentario;

                if (mostrarComentario == "S") {
                    htmlItem += `
                     <td class="${clasePropiedad} valor" style="background: ${colorCelda};">${htmlItemValor} ${htmlInputAnotacion}</td>
                    `;
                } else {
                    htmlItem += `
                    <td class="${clasePropiedad} valor" style="background: ${colorCelda};">${htmlItemValor}</td>
                    `;
                }
            } else {
                htmlItem += `
                    <td class="${clasePropiedad} valor" style="background: ${colorCelda};">${htmlItemValor}</td>
                `;
            }

            //2. Columna "Confidencial"
            if (mostrarColumnaValorConfidencial) {
                if (mostrarOpcionValor) {
                    var mostrarCheckValConfidencial = configItem.Fitcfgflagvalorconf;

                    if (mostrarCheckValConfidencial == "S") {
                        var checkedConf = reg.ItemValConfidencial == "S" ? " checked " : "";

                        //Verifico si tiene dependientes
                        var htmlFuncionalidadCopiar = "";
                        if (reg.FtitcodiDependiente != null) {
                            htmlFuncionalidadCopiar = `onchange='copiarChkADependiente(${reg.Ftitcodi},${reg.FtitcodiDependiente})'`;
                        }

                        htmlItem += `
                              <td class="${clasePropiedad} valor" style="background: ${colorCelda};">
                                    <input type="checkbox" id="chk_ValConfidencial_${reg.Ftitcodi}" name="" value="" ${checkedConf} ${htmlDisabled} ${htmlFuncionalidadCopiar}>
                              </td>                              
                            `;
                    } else {
                        htmlItem += `
                              <td class="${clasePropiedad} valor" style="background: ${colorCelda};"></td>                              
                            `;
                    }
                } else {
                    htmlItem += `
                              <td class="${clasePropiedad} valor" style="background: ${colorCelda};"></td>                              
                            `;
                }
            }


            //3. Columna "Adjuntar Sustento"
            if (mostrarColumnaSustento) {
                if (mostrarOpcionValor) {
                    var htmlArchivoXDato = configItem.Fitcfgflagsustento == "S" ? cargarHtmlDocumentoXEnvio(reg, TIPO_ARCHIVO_SUSTENTO_DATO, !boolDeshabilitar) : '';

                    htmlItem += `                              
                              <td class="${clasePropiedad} valor" style="background: ${colorCelda};">
                                <div id=''>${htmlArchivoXDato}</div>
                              </td>
                            `;
                } else {
                    htmlItem += `
                              <td class="${clasePropiedad} valor" style="background: ${colorCelda};"></td>                              
                            `;
                }
            }

            //4. Columna "Instructivo de llenado"
            if (mostrarColumnaInstructivoLlenado) {
                if (mostrarOpcionValor) {
                    htmlItem += `
                              <td class="${clasePropiedad} valor" style="background: ${colorCelda};">${htmlBtnVerInstructivo}</td>
                            `;
                } else {
                    htmlItem += `
                              <td class="${clasePropiedad} valor" style="background: ${colorCelda};"></td>                              
                            `;
                }
            }


            if (iniciaNuevaFila) {
                htmlItem += `</tr>`;
            }

            iniciaNuevaFila = true;

            if (reg.ListaHijos.length > 0) {
                var colspanDisponibleSig = colspanDisponible - 1;

                htmlItem += `
                        ${_extranet_getHtmlItem(listaAllData, reg.ListaHijos, listaItemConfig, reg.Ftitcodi, true, reg.Nivel + 1, numMaxCol, colspanDisponibleSig)}
                    `;
            }
        }

    }

    return htmlItem;
}

function copiarChkADependiente(FtitcodiManda, FtitcodiDepende) {

    var idD = 'chk_ValConfidencial_' + FtitcodiDepende;
    if ($('#chk_ValConfidencial_' + FtitcodiManda).is(":checked")) {
        document.getElementById(idD).checked = true;
    }
    else {
        document.getElementById(idD).checked = false;
    }

}

function _extranet_profundidadVerticalRecursivo(listaHijos) {

    if (!listaHijos.length > 0) {
        listaHijos = [];
    }
    var tipoAgrup = 1;
    var listaProfundidad = [];
    listaHijos.sort((x, y) => x.Ftitorden - y.Ftitorden); // ordenamiento

    for (var i = 0; i < listaHijos.length; i++) {
        var reg = listaHijos[i];
        if (tipoAgrup == reg.Ftittipoitem || reg.ListaHijos.length > 0) {

            if (reg.ListaHijos.length > 0) {
                var filaAdicional = tipoAgrup != reg.Ftittipoitem ? 1 : 0;
                listaProfundidad.push(filaAdicional + _extranet_profundidadVerticalRecursivo(reg.ListaHijos));
            } else {
                listaProfundidad.push(1);
            }
        }
        else {
            listaProfundidad.push(1);
        }
    }

    if (listaProfundidad.length > 0) {
        var suma = 0;
        listaProfundidad.forEach(function (numero) {
            suma += numero;
        });
        return suma;
    }
    else {
        return 0;
    }
}

function _extranet_getHtmlItemUnidad(unidad) {
    var htmlItemUnidad = '';

    if (unidad == null || unidad == "") {
        return htmlItemUnidad;
    }

    unidad = unidad.trim();
    for (var i = 0; i < unidad.length; i++) {
        var text = unidad[i]
        if (!isNaN(text)) {
            htmlItemUnidad += `<span class="sup">${text}</span>`;
        }
        else {
            htmlItemUnidad += `${text}`;
        }
    }

    return htmlItemUnidad;
}

function _extranet_getHtmlItemValor(reg, configItem) {
    var campoParamBloqueadoEdicion = configItem != null ? (configItem.Fitcfgflagbloqedicion != null ? configItem.Fitcfgflagbloqedicion : "") : "";
    var estaBloqueado = campoParamBloqueadoEdicion == "S";

    var formatedo = (reg.Valor == null || reg.Valor == "") ? "" : reg.Valor.trim();
    var htmlDisabled = OPCION_GLOBAL_EDITAR ? (estaBloqueado ? " disabled " : "") : " disabled ";

    if (reg.Ftpropcodi != null) {
        return `
                    ${formatedo} <input id='campo_input_${reg.Ftitcodi}' type="hidden" value="${formatedo}"/>
                `;

    } else {
        if (reg.Concepcodi != null || reg.Propcodi != null) {
            if (reg.EsArchivo) {
                var htmlArchivoXDato = !estaBloqueado ? cargarHtmlDocumentoXEnvio(reg, TIPO_ARCHIVO_VALOR_DATO, OPCION_GLOBAL_EDITAR) : '';

                return `
                        <div id=''>${htmlArchivoXDato}</div>
                    `;
            } else {
                var htmlFuncionalidadCopiar = "";
                if (reg.FtitcodiDependiente != null) {
                    htmlFuncionalidadCopiar = `onchange='copiarADependiente(${reg.Ftitcodi},${reg.FtitcodiDependiente})'`;
                }
                return `
                    <input id='campo_input_${reg.Ftitcodi}' type="text" style='width:80px' value="${formatedo}" ${htmlDisabled} ${htmlFuncionalidadCopiar}/>
                `;
            }
        }
    }

    return formatedo;
}

function copiarADependiente(FtitcodiManda, FtitcodiDepende) {
    var textoCopiar = $("#campo_input_" + FtitcodiManda).val();
    $("#campo_input_" + FtitcodiDepende).val(textoCopiar);
}

function _evaluarMostrarInputValor(reg) {
    var salida = false;
    if (reg.Ftpropcodi != null) {
        salida = false;

    } else {
        if (reg.Concepcodi != null || reg.Propcodi != null) {
            salida = true;
        }
    }

    return salida;
}

function _extranet_crearHtmlAnotacionComentario(reg, boolDeshabilitar) {
    var htmlNota = '';

    var anotacionBD = "";
    var anotacionBD = reg.Itemcomentario != null ? reg.Itemcomentario.trim() : "";
    var flagDisabled = boolDeshabilitar ? 'S' : 'N';

    if (anotacionBD == "") {
        htmlNota += `
            <div style="float: right;">
                <a href="JavaScript:_extranet_editarAnotacion(${reg.Ftitcodi},${reg.FtitcodiDependiente}, '${flagDisabled}');"> <div id="imagen_anotacion_${reg.Ftitcodi}">${IMG_NOTA_VACIA}</div></a>
            </div>
        `;
    } else {
        htmlNota += `
            <div style="float: right;">
                <a href="JavaScript:_extranet_editarAnotacion(${reg.Ftitcodi},${reg.FtitcodiDependiente}, '${flagDisabled}');"> <div id="imagen_anotacion_${reg.Ftitcodi}">${IMG_NOTA_LLENA}</div></a>
            </div>
        `;
    }



    return htmlNota;
}

function _extranet_crearHtmlInputAnotacion(reg) {
    var htmlNota = '';

    htmlNota += `
            <input id="campo_input_anotacion_${reg.Ftitcodi}" type="text" style="width:60px; display:none;" value="${reg.Itemcomentario}">
    `;

    return htmlNota;
}

function _extranet_crearHtmlVerInstructivo(reg, configItem) {
    var htmlBtn = '';

    var campoInstructivoLlenado = configItem != null ? (configItem.Fitcfgflaginstructivo != null ? configItem.Fitcfgflaginstructivo : "") : "";
    var valInstructivo = configItem != null ? (configItem.Fitcfginstructivo != null ? configItem.Fitcfginstructivo : "") : "";

    if (campoInstructivoLlenado == "S" && valInstructivo != "") {

        valInstructivo = valInstructivo.replace(/"/g, "&quot;");

        htmlBtn += `
            
            <input type="button" id="btn_ver_${reg.Ftitcodi}" value="Ver" title="${valInstructivo}" onclick="mostrarInstructivo('${valInstructivo}');"/>
        `;
    }


    return htmlBtn;
}

function mostrarInstructivo(msg) {
    $("#mensaje_instructivo").html(msg);
    _extranet_abrirPopup("popupInstructivo");
}

function _extranet_editarAnotacion(ftitcodi, ftitcodiDependiente, flagDisabled) {
    //campo_input_anotacion_1006
    var anotacionFila = $("#campo_input_anotacion_" + ftitcodi).val();

    $("#campo_anotacion_editar").val(anotacionFila);

    //deshabilitar opciones
    if (flagDisabled == 'S') {
        $("#campo_anotacion_editar").prop('disabled', true);
        $("#btnGuardarAnotacion").hide();
        $("#btnCancelarAnotacion").hide();
    }

    _extranet_abrirPopup("popupAnotacion");
    $("#hfRegFtitcodi").val(ftitcodi);
    $("#hfRegFtitcodiDependiente").val(ftitcodiDependiente);
}

function _extranet_guardarTemporalmenteAnotacion() {
    var miftitcodi = $("#hfRegFtitcodi").val();
    var miftitcodiDependiente = $("#hfRegFtitcodiDependiente").val();

    var anotacionIngresada = $("#campo_anotacion_editar").val();

    $("#campo_input_anotacion_" + miftitcodi).val(anotacionIngresada.trim());
    //copio el comentario al dependiente
    if (miftitcodiDependiente != null) {
        $("#campo_input_anotacion_" + miftitcodiDependiente).val(anotacionIngresada.trim());
    }

    if (anotacionIngresada.trim() != "") {
        $("#imagen_anotacion_" + miftitcodi).html(IMG_NOTA_LLENA);

        if (miftitcodiDependiente != null) {
            $("#imagen_anotacion_" + miftitcodiDependiente).html(IMG_NOTA_LLENA);
        }
    } else {
        $("#imagen_anotacion_" + miftitcodi).html(IMG_NOTA_VACIA);
        //copio el comentario al dependiente
        if (miftitcodiDependiente != null) {
            $("#imagen_anotacion_" + miftitcodiDependiente).html(IMG_NOTA_VACIA);
        }
    }

    _extranet_cerrarPopup("popupAnotacion");
}

function _extranet_getHtmlNota(listaNota, listaAllData) {
    var htmlNota = '';

    var listaDataXNivel = listaAllData.filter(x => x.Nivel == 1);
    //obtener profundidad horizontal
    var colspan = _extranet_profundidadHorizontalRecursivo(listaDataXNivel);
    //


    htmlNota += `
                    <tr class="fila_nota">
                        <td></td>
                        <td colspan="${colspan - 1}">

<table class="tabla_nota" style="width: 100%">
    <tbody>
          <tr>
            <td style="width: 50px;" class="titulo" rowspan="${listaNota.length}">Nota:</td>

`;


    for (var i = 0; i < listaNota.length; i++) {
        if (i != 0) {

            htmlNota += `<tr>`;
        }

        htmlNota += `
                    <td style="width: 50px;" class="num">${listaNota[i].Ftnotanum})</td>
                    <td style="width: 400px;" class="desc">${listaNota[i].Ftnotdesc}</td>
                    </tr>
                `;
    }

    htmlNota += `</tbody>
                </table>
                </td>
                </tr>
                `;

    return htmlNota;
}

function _extranet_MyMax(myarr) {
    var al = myarr.length;
    maximum = myarr[al - 1];
    while (al--) {
        if (myarr[al] > maximum) {
            maximum = myarr[al]
        }
    }
    return maximum;
}

////////////////////////////////////////////////
// Util
////////////////////////////////////////////////
function _extranet_cerrarPopup(id) {
    $("#" + id).bPopup().close()
}

function _extranet_abrirPopup(contentPopup) {
    setTimeout(function () {
        $("#" + contentPopup).bPopup({
            easing: 'easeOutBack',
            speed: 450,
            transition: 'slideDown',
            modalClose: false
        });
    }, 50);
}