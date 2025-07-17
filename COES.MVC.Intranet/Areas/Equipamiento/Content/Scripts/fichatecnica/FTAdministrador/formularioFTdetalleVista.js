var anchoPortal = 1405; //1200px es el ancho del portal. Este debe ser el mismo que se mostrará en Intranet
var OPCION_ACTUAL = 0;
var TIPO_SUBESTACION = -2;
var DETALLE_FT_GLOBAL = null;
const COLOR_CELDA = "#CBFBE6";
var IMG_NOTA_LLENA = '<img src="' + siteRoot + 'Content/Images/btn-notaLlena.png" alt="Editar Nota" width="19" height="19" style="">';
var IMG_NOTA_VACIA = '<img src="' + siteRoot + 'Content/Images/btn-notaVacia.png" alt="Editar Nota" width="19" height="19" style="">';
var IMG_CONFIDENCIALIDAD = '<img src="' + siteRoot + 'Content/Images/DownFileConfidencial.png" alt="Valor Confidencial" title="Valor Confidencial" width="19" height="19" style="">';


//Generar html
function _extranet_generarHtmlReporteDetalleFichaTecnica(model) {
    var raiz = -1;
    var listaDataXNivel = model.ListaAllItems.filter(x => x.Nivel == 1);

    //obtener profundidad horizontal
    var colspan = _extranet_profundidadHorizontalRecursivo(listaDataXNivel);
    var colspanIni = colspan + 1;

    //obtengo el colspan inicial
    if (mostrarColumnaValorConfidencial)
        colspanIni++;
    if (mostrarColumnaInstructivoLlenado)
        colspanIni++;
    if (mostrarColumnaSustento)
        colspanIni++;

    var anchoMaximoIzquierda = parseInt($(window).width() / 2);
    if ($(window).width() < 1500) {
        anchoMaximoIzquierda = colspan > 10 ? 730 : 700;
    } else {
        if (colspan > 10) { anchoMaximoIzquierda = 850; }
        else {
            if ($(window).width() >= 1900) anchoMaximoIzquierda = 760;
            else
                anchoMaximoIzquierda = 700;
        }
    }
    var htmlCabecera = `
            <tr id="1raFila">
                <td style ="width: ${anchoMaximoIzquierda}px;" colspan='${colspanIni}'></td>
            </tr> 
            <tr id="2daFila" style = "position: sticky; top: -1px; z-index: 10;">
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
                <td style='width: 77px;' class='cabecera'>Adjuntar <br/>Sustento</td>
        `;
    }
    if (mostrarColumnaInstructivoLlenado) {
        htmlCabecera += `
                <td style='width: 56px;' class='cabecera'>Instructivo <br/>de llenado</td>
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
        <table id="reporte" class="pretty tabla-icono tree_ficha_tecnica miTablaR" style="overflow-y: hidden; overflow-x: auto; width: max-content;">
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
                    var miPropcodi = reg.Propcodi != null ? reg.Propcodi : "N";
                    var miConcepcodi = reg.Concepcodi != null ? reg.Concepcodi : "N";
                    var miNombre = reg.Ftitnombre != null ? reg.Ftitnombre : "";

                    htmlItem += `<tr id="${orden}_${reg.Ftitcodi}_${miPropcodi}_${miConcepcodi}_${reg.EsTipoArchivo}" data-nombfila="${orden} ${miNombre}">`;
                    //htmlItem += `<tr>`;
                }
                var colspanFila = colspanDisponible - (reg.Nivel == 1 ? 1 : 2) + 1; //se agrega 1 por la columna adicional de valor 
                colspanFila += COL_ADICIONAL; //se agrega columnas adicionales de sustento y del instructivo
                htmlItem += `
                     <td class="${claseAgrupamiento} orden celdaIzq">${orden}</td>
                     <td colspan="${colspanFila}" class="${claseAgrupamiento} nombre celdaIzq">${reg.Ftitnombre} <span class="nota_agrup">${reg.ListaNotanum}</span> </td>
                     </tr>
                `;

                iniciaNuevaFila = true;

                if (rowspan > 0) {
                    var colspanDisponibleSig = reg.Nivel == 1 ? numMaxCol : colspanDisponible - 1;

                    var UltimoOrdenHijos = ObtenerOrdenUltimoHijo(reg);
                    var UltimoFitcodi = ObtenerUltimoFitcodi(reg);
                    var UltimoPropcodi = ObtenerUltimoPropcodi(reg);
                    var UltimoConcepcodi = ObtenerUltimoConcepcodi(reg);
                    var UltimoEsArchivo = ObtenerUltimoEsArchivo(reg);
                    var UltimoNombre = ObtenerUltimoNombre(reg);

                    htmlItem += `
                    <tr id="${UltimoOrdenHijos}_${UltimoFitcodi}_${UltimoPropcodi}_${UltimoConcepcodi}_${UltimoEsArchivo}" data-nombfila="${UltimoOrdenHijos} ${UltimoNombre}">

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

                    //DE PRUEBAS
                    var UltimoOrdenHijos1 = ObtenerOrdenUltimoHijo(reg);
                    var UltimoFitcodi1 = ObtenerUltimoFitcodi(reg);
                    var UltimoPropcodi1 = ObtenerUltimoPropcodi(reg);
                    var UltimoConcepcodi1 = ObtenerUltimoConcepcodi(reg);
                    var UltimoEsArchivo1 = ObtenerUltimoEsArchivo(reg);
                    var UltimoNombre = ObtenerUltimoNombre(reg);

                    if (rowspan > 0) {

                        htmlItem += `<tr id="${UltimoOrdenHijos1}_${UltimoFitcodi1}_${UltimoPropcodi1}_${UltimoConcepcodi1}_${UltimoEsArchivo1}" data-nombfila="${UltimoOrdenHijos1} ${UltimoNombre}">`;
                        //htmlItem += `<tr id="OElse2do2_${UltimoOrdenHijos1}_${UltimoFitcodi1}_${UltimoPropcodi1}_${UltimoConcepcodi1}_${UltimoEsArchivo1}">`;

                    }
                    else {
                        var miPropcodi3 = reg.Propcodi != null ? reg.Propcodi : "N";
                        var miConcepcodi3 = reg.Concepcodi != null ? reg.Concepcodi : "N";
                        var miNombre3 = reg.Ftitnombre != null ? reg.Ftitnombre : "";

                        //htmlItem += `<tr id="OElse2do1_${orden}_${reg.Ftitcodi}_${miPropcodi3}_${miConcepcodi3}_${reg.EsTipoArchivo}">`;
                        htmlItem += `<tr id="${orden}_${reg.Ftitcodi}_${miPropcodi3}_${miConcepcodi3}_${reg.EsTipoArchivo}" data-nombfila="${orden} ${miNombre3}">`;

                    }
                }

                htmlItem += `
                     <td rowspan="${rowspan}" class="${claseAgrupamiento} orden celdaIzq">${orden}</td>
                     <td rowspan="${rowspan}" class="${claseAgrupamiento} nombre celdaIzq">${reg.Ftitnombre} <span class="nota_agrup">${reg.ListaNotanum}</span></td>
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

                var miPropcodi4 = reg.Propcodi != null ? reg.Propcodi : "N";
                var miConcepcodi4 = reg.Concepcodi != null ? reg.Concepcodi : "N";
                var miNombre4 = reg.Ftitnombre != null ? reg.Ftitnombre : "";

                htmlItem += `<tr id="${orden}_${reg.Ftitcodi}_${miPropcodi4}_${miConcepcodi4}_${reg.EsTipoArchivo}" data-nombfila="${orden} ${miNombre4}">`;
            }

            var htmlItemUnidad = _extranet_getHtmlItemUnidad(reg.ItemUnidadDesc);

            var htmlItemValor = '';
            if (OPCION_ACTUAL != 2) {
                htmlItemValor = _extranet_getHtmlItemValor(reg);
            }

            var colspanDispItem = colspanDisponible - 3;

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
            /*var htmlDisabled = OPCION_GLOBAL_EDITAR ? "" : " disabled ";*/
            var htmlDisabled = " disabled ";

            var mostrarOpcionValor = _evaluarMostrarInputValor(reg);
            htmlItem += `
                     <td rowspan="${rowspan}" style="background: ${colorCelda};" class="${clasePropiedad} orden celdaIzq">${orden}</td>
            `;

            //1. Columna "Valor""
            if (mostrarOpcionValor) {
                //para agregar o no boton de COMENTARIO

                var mostrarComentario = configItem != undefined ? configItem.Fitcfgflagcomentario : "N";

                if (mostrarComentario == "S") {
                    htmlItem += `
                     <td colspan="${colspanDispItem}" style="background: ${colorCelda};" class="${clasePropiedad} nombre celdaIzq">${reg.Ftitnombre} <span class="nota_item">${reg.ListaNotanum} </span> ${htmlanotacion}</td>
                    `;
                } else {
                    htmlItem += `
                     <td colspan="${colspanDispItem}" style="background: ${colorCelda};" class="${clasePropiedad} nombre celdaIzq">${reg.Ftitnombre} <span class="nota_item">${reg.ListaNotanum} </span></td>
                    `;
                }
            } else {
                htmlItem += `
                     <td colspan="${colspanDispItem}" style="background: ${colorCelda};" class="${clasePropiedad} nombre celdaIzq">${reg.Ftitnombre} <span class="nota_item">${reg.ListaNotanum} </span></td>
                `;
            }

            htmlItem += `
                    
                     <td class="${clasePropiedad} unidad celdaIzq" style="background: ${colorCelda};">${htmlItemUnidad}</td>
            `;
            if (mostrarOpcionValor) {
                //para agregar input oculto para almacenar COMENTARIO
                var mostrarComentario = configItem != undefined ? configItem.Fitcfgflagcomentario : "N";

                if (mostrarComentario == "S") {
                    htmlItem += `
                     <td class="${clasePropiedad} valor celdaIzq" style="background: ${colorCelda};">${htmlItemValor} ${htmlInputAnotacion}</td>
                    `;
                } else {
                    htmlItem += `
                    <td class="${clasePropiedad} valor celdaIzq" style="background: ${colorCelda};">${htmlItemValor}</td>
                    `;
                }
            } else {
                htmlItem += `
                    <td class="${clasePropiedad} valor celdaIzq" style="background: ${colorCelda};">${htmlItemValor}</td>
                `;
            }

            //2. Columna "Confidencial"
            if (mostrarColumnaValorConfidencial) {
                if (mostrarOpcionValor) {
                    var mostrarCheckValConfidencial = configItem != undefined ? configItem.Fitcfgflagvalorconf : "N";

                    if (mostrarCheckValConfidencial == "S" && !reg.EsArchivo) { //solo mostrar check cuando no sea archivo
                        var checkedConf = reg.ItemValConfidencial == "S" ? " checked " : "";

                        //Verifico si tiene dependientes
                        var htmlFuncionalidadCopiar = "";
                        if (reg.FtitcodiDependiente != null) {
                            htmlFuncionalidadCopiar = `onchange='copiarChkADependiente(${reg.Ftitcodi},${reg.FtitcodiDependiente})'`;
                        }

                        htmlItem += `
                              <td class="${clasePropiedad} valor celdaIzq" style="background: ${colorCelda};">
                                    <input type="checkbox" id="chk_ValConfidencial_${reg.Ftitcodi}" name="" value="" ${checkedConf} ${htmlDisabled} ${htmlFuncionalidadCopiar}>
                              </td>                              
                            `;
                    } else {
                        htmlItem += `
                              <td class="${clasePropiedad} valor celdaIzq" style="background: ${colorCelda};"></td>
                            `;
                    }
                } else {
                    htmlItem += `
                              <td class="${clasePropiedad} valor celdaIzq" style="background: ${colorCelda};"></td>
                            `;
                }
            }

            //3. Columna "Adjuntar Sustento"
            if (mostrarColumnaSustento) {
                if (mostrarOpcionValor) {
                    var htmlArchivoXDato = configItem != undefined ? (configItem.Fitcfgflagsustento == "S" ? cargarHtmlDocumentoXEnvio(reg, TIPO_ARCHIVO_SUSTENTO_DATO, false) : '') : '';

                    htmlItem += `                              
                              <td class="${clasePropiedad} valor " style="background: ${colorCelda};">
                                <div id='' style="width: 75px;" >${htmlArchivoXDato}</div>
                              </td>
                            `;

                } else {
                    htmlItem += `
                              <td class="${clasePropiedad} valor " style="background: ${colorCelda};"></td>
                            `;
                }
            }

            //4. Columna "Instructivo de llenado"
            if (mostrarColumnaInstructivoLlenado) {
                if (mostrarOpcionValor) {
                    htmlItem += `
                              <td class="${clasePropiedad} valor " style="background: ${colorCelda};">${htmlBtnVerInstructivo}</td>
                            `;

                } else {
                    htmlItem += `
                              <td class="${clasePropiedad} valor " style="background: ${colorCelda};"></td>
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

function _extranet_getHtmlItemValor(reg) {
    var formatedo = replaceSpecialChars((reg.Valor == null || reg.Valor == "") ? "" : reg.Valor.trim());
    var htmlDisabled = " disabled ";

    if (reg.Ftpropcodi != null) {
        return `
                    ${formatedo} <input id='campo_input_${reg.Ftitcodi}' type="hidden" value="${formatedo}"/>
                `;

    } else {
        if (reg.Concepcodi != null || reg.Propcodi != null) {
            if (reg.EsArchivo) {
                var habilitarListaArchivo = false;
                var htmlArchivoXDato = cargarHtmlDocumentoXEnvio(reg, TIPO_ARCHIVO_VALOR_DATO, habilitarListaArchivo);

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
    var textoCopiar = replaceSpecialChars($("#campo_input_" + FtitcodiManda).val());
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
                <a title="Editar Anotación" href="JavaScript:_extranet_editarAnotacion(${reg.Ftitcodi},${reg.FtitcodiDependiente}, '${flagDisabled}');"> <div id="imagen_anotacion_${reg.Ftitcodi}">${IMG_NOTA_VACIA}</div></a>
            </div>
        `;
    } else {
        htmlNota += `
            <div style="float: right;">
                <a title="Editar Anotación" href="JavaScript:_extranet_editarAnotacion(${reg.Ftitcodi},${reg.FtitcodiDependiente}, '${flagDisabled}');"><div id="imagen_anotacion_${reg.Ftitcodi}">${IMG_NOTA_LLENA}</div></a>
            </div>
        `;
    }



    return htmlNota;
}

function _extranet_crearHtmlInputAnotacion(reg) {
    var htmlNota = '';

    htmlNota += `
            <input id="campo_input_anotacion_${reg.Ftitcodi}" type="text" style="width:60px; display:none;" value="${replaceSpecialChars(reg.Itemcomentario)}">
            
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

function _extranet_editarAnotacion(ftitcodi) {
    //campo_input_anotacion_1006
    var anotacionFila = replaceSpecialChars($("#campo_input_anotacion_" + ftitcodi).val());

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

    var anotacionIngresada = replaceSpecialChars($("#campo_anotacion_editar").val());

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
    var maximum = myarr[al - 1];
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


function ObtenerOrdenUltimoHijo(reg) {
    var salida = reg.Orden;

    if (reg.ListaHijos.length > 0) {
        if (reg.Ftittipoitem != 0)
            salida = ObtenerOrdenUltimoHijo(reg.ListaHijos[0]);
    }

    return salida;
}

function ObtenerUltimoFitcodi(reg) {
    var salida = reg.Ftitcodi;

    if (reg.ListaHijos.length > 0) {
        if (reg.Ftittipoitem != 0)
            salida = ObtenerUltimoFitcodi(reg.ListaHijos[0]);
    }

    return salida;
}

function ObtenerUltimoPropcodi(reg) {
    var salida = reg.Propcodi;

    if (reg.ListaHijos.length > 0) {
        if (reg.Ftittipoitem != 0)
            salida = ObtenerUltimoPropcodi(reg.ListaHijos[0]);
    }

    if (salida == null)
        salida = "N";

    return salida;
}

function ObtenerUltimoConcepcodi(reg) {
    var salida = reg.Concepcodi;

    if (reg.ListaHijos.length > 0) {
        if (reg.Ftittipoitem != 0)
            salida = ObtenerUltimoConcepcodi(reg.ListaHijos[0]);
    }

    if (salida == null)
        salida = "N";

    return salida;
}

function ObtenerUltimoEsArchivo(reg) {
    var salida = reg.EsTipoArchivo;

    if (reg.ListaHijos.length > 0) {
        if (reg.Ftittipoitem != 0)
            salida = ObtenerUltimoEsArchivo(reg.ListaHijos[0]);
    }

    return salida;
}

function ObtenerUltimoNombre(reg) {
    var salida = reg.Ftitnombre;

    if (reg.ListaHijos.length > 0) {
        if (reg.Ftittipoitem != 0)
            salida = ObtenerUltimoNombre(reg.ListaHijos[0]);
    }

    if (salida == null)
        salida = "";

    return salida;
}

const specialCharsMap = new Map([
    ["<", "<"],
    [">", ">"],
    ["&", "&"],
]);

function replaceSpecialChars(string) {
    if (string != undefined && string != null && string != "") {
        for (const [key, value] of specialCharsMap) {
            string = string.replace(new RegExp(key, "g"), value);
        }
    }
    return string;
}
