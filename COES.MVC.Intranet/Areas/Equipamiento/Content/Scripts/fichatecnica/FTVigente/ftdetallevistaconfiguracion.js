var controlador = siteRoot + 'Equipamiento/FTVigente/';

var anchoPortal = 1200; //1200px es el ancho del portal. Este debe ser el mismo que se mostrará en Intranet
var OPCION_ACTUAL = 0;
var TIPO_EQ_FICHA_SELECCIONADO = -2;
var DETALLE_FT_GLOBAL = null;
var IMG_OCULTO_INTRANET_ = `<img src="${siteRoot}Content/Images/ContextMenu/menuhidden.png" title="Oculto para Intranet"/>`;
const COLOR_CELDA = "#CBFBE6";

//generar detalle
function GenerarDetalleFT(tipo, idTipo, idElemento, idFicha, ancho, opcionVista, fecha) {
    $("#detalle_ficha_tecnica").html("");

    $.ajax({
        type: 'POST',
        url: controlador + "GenerarDetalleFichaTecnicaConfig",
        dataType: 'json',
        data: {
            tipo: tipo,
            idTipo: idTipo,
            idElemento: idElemento,
            fecha: fecha,
            idFicha: idFicha
        },
        success: function (evt) {
            if (evt.Resultado != "-1") {
                DETALLE_FT_GLOBAL = evt;
                OPCION_ACTUAL = opcionVista;
                TIPO_EQ_FICHA_SELECCIONADO = idTipo;

                $("#hfFlagExisteComentario").val(evt.FlagExisteComentario);

                //Inicializar vista previa
                $("#detalle_ficha_tecnica").css("width", anchoPortal + "px");
                $("#detalle_ficha_tecnica").html(generarHtmlReporteDetalleFichaTecnica(evt));
                //$("#reporte").css("width", ancho + "px");

                //evento de tablas hijas
                cargarEventoHijos();
                //cargarEventosJs();

            } else {
                alert("Ha ocurrido un error: " + evt.Mensaje);
            }
        },
        error: function (err) {
            alert("Error al cargar visualización");
        }
    });
}

//Generar html
function generarHtmlReporteDetalleFichaTecnica(model) {
    var raiz = -1;
    var listaDataXNivel = model.ListaAllItems.filter(x => x.Nivel == 1);

    //obtener profundidad horizontal
    var colspan = profundidadHorizontalRecursivo(listaDataXNivel);
    var htmlItem = _getHtmlItem(model.ListaAllItems, model.ListaTreeItems, raiz, true, 1, colspan, colspan);

    var htmlNota = '';
    if (model.ListaNota.length > 0) {
        htmlNota = _getHtmlNota(model.ListaNota, model.ListaAllItems);
    }

    var htmlHijo = '';
    if (model.FichaTecnica != null) {
        htmlHijo = _getHtmlDetalleFichaTecnicaHijo(model.IdFicha, model.ListaHijo, model.FichaTecnica, model.ListaEquipo, model.ListaEquipo2, model.ListaGrupo, -2, "-2");
    }

    var html = `
        <table id="reporte" class="pretty tabla-icono tree_ficha_tecnica" style="overflow-y: hidden; overflow-x: auto; width: ${anchoPortal}px; table-layout: fixed;">
            <tbody>
                ${htmlItem}
                ${htmlNota}
            </tbody>
        </table>
        <div id="reportHijos" style = "width: 1200px;">
            ${htmlHijo}
        </div>
    `;

    return html;
}

function _getHtmlItem(listaAllData, listaTreeData, idPadre, iniciaNuevaFila, nivel, numMaxCol, colspanDisponible) {
    var colSustento = $("#hflagsustento").val() === "true"? 1 : 0;

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
        if (tipoAgrup == reg.Ftittipoitem) {

            if (reg.Nivel == 1 || reg.ListaHijos.length == 0) {

                //obtener profundidad vertical
                var rowspan = profundidadVerticalRecursivo(reg.ListaHijos);
                //
                if (iniciaNuevaFila) {
                    htmlItem += `<tr>`;
                }
                var colspanFila = colspanDisponible - (reg.Nivel == 1 ? 1 : 2) + 1 + colSustento; //se agrega 1 por la columna adicional de valor
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
                        ${_getHtmlItem(listaAllData, reg.ListaHijos, reg.Ftitcodi, false, reg.Nivel + 1, numMaxCol, colspanDisponibleSig)}
                `;
                }

            }
            else {
                //obtener profundidad vertical
                var rowspan = profundidadVerticalRecursivo(reg.ListaHijos);
                //
                if (iniciaNuevaFila) {
                    htmlItem += `<tr>`;
                }

                htmlItem += `
                     <td rowspan="${rowspan}" class="${claseAgrupamiento} orden">${orden}</td>
                     <td rowspan="${rowspan}" class="${claseAgrupamiento} nombre">${reg.Ftitnombre} <span class="nota_agrup">${reg.ListaNotanum}</span> </td>
                `;

                iniciaNuevaFila = true;

                if (rowspan > 0) {
                    var colspanDisponibleSig = colspanDisponible - 2;

                    htmlItem += `
                        ${_getHtmlItem(listaAllData, reg.ListaHijos, reg.Ftitcodi, false, reg.Nivel + 1, numMaxCol, colspanDisponibleSig)}
                    `;
                }
            }

        }
        else {
            //obtener profundidad vertical
            var rowspan = profundidadVerticalRecursivo(reg.ListaHijos);
            if (reg.ListaHijos.length > 0) rowspan++;
            rowspan = rowspan > 0 ? rowspan : 1;

            if (iniciaNuevaFila) {
                htmlItem += `<tr>`;
            }

            var htmlItemUnidad = _getHtmlItemUnidad(reg.ItemUnidadDesc);

            var htmlItemValor = '';
            var htmlItemSustento = '';
            if (OPCION_ACTUAL != 2) {
                htmlItemValor = _getHtmlItemValor(reg);
                htmlItemSustento = _getHtmlItemSustento(reg);
            }

            var htmlcomentario = ``;
            if (reg.Itemcomentario != null && reg.Itemcomentario != "") {
                htmlcomentario += `<img src="` + siteRoot + `Content/Images/ico-info.gif" title="Informado por el Agente: \n${reg.Itemcomentario}" width="16" height="16" style ="vertical-align: bottom;">`;
            }

            var colspanDispItem = colspanDisponible - 3;
            //if (reg.ListaHijos.length > 0) colspanDispItem = colspanDisponible - 4;

            //Coloreo si la propiedad o concepto tenga check "Mostrar en color verde claro celda FT"
            var colorCelda = "";
            var valCheck = 0;
            if (reg.Concepcodi != null || reg.Propcodi != null) {

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

            }

            var htmlColSustento = ``;
            if ($("#hflagsustento").val() === "true" ) {
                htmlColSustento = `<td class="${clasePropiedad} sustento" style="background: ${colorCelda};">${htmlItemSustento}</td>`;
            }

            htmlItem += `
                     <td rowspan="${rowspan}" style="background: ${colorCelda};" class="${clasePropiedad} orden">${orden}</td>
                     <td colspan="${colspanDispItem}" style="background: ${colorCelda};" class="${clasePropiedad} nombre">${reg.Ftitnombre} <span class="nota_item">${reg.ListaNotanum} ${htmlcomentario}</span> </td>
                     <td class="${clasePropiedad} unidad" style="background: ${colorCelda};">${htmlItemUnidad}</td>
                     <td class="${clasePropiedad} valor" style="background: ${colorCelda};">${htmlItemValor}</td>
                     ${htmlColSustento}
                `;

            if (iniciaNuevaFila) {
                htmlItem += `</tr>`;
            }

            iniciaNuevaFila = true;

            if (reg.ListaHijos.length > 0) {
                var colspanDisponibleSig = colspanDisponible - 1;

                htmlItem += `
                        ${_getHtmlItem(listaAllData, reg.ListaHijos, reg.Ftitcodi, true, reg.Nivel + 1, numMaxCol, colspanDisponibleSig)}
                    `;
            }
        }

    }

    return htmlItem;
}

function _getHtmlNota(listaNota, listaAllData) {
    var htmlNota = '';

    var listaDataXNivel = listaAllData.filter(x => x.Nivel == 1);
    //obtener profundidad horizontal
    var colspan = profundidadHorizontalRecursivo(listaDataXNivel);
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

function _getHtmlDetalleFichaTecnicaHijo(idFicha, listaHijo, fichaTecnicaPadre, listaEquipo, listaEquipo2, listaGrupo, codEmpresa, strEstado) {
    var htmlHijo = '';

    var contadorEquipo = 1;
    var contadorGrupo = 1;
    var clasever = "";
    for (var i = 0; i < listaHijo.length; i++) {
        var hijo = listaHijo[i]

        //Tipo de equipo
        if (hijo.Famcodi != null) {
            var nombreListaEquipo = hijo.Famnomb;
            var listaFamcodi = [2, 3, 22, 36, 38];

            if (fichaTecnicaPadre.Famcodi != null && listaFamcodi.includes(hijo.Famcodi)) {
                if (hijo.Famcodi == 2 || hijo.Famcodi == 3) {
                    nombreListaEquipo = "Unidades de Generación";
                }
                if (hijo.Famcodi == 22) {
                    nombreListaEquipo = "Generadores de Vapor";
                }
                if (hijo.Famcodi == 36) {
                    nombreListaEquipo = "Componente de Central Solar Fotovoltaica";
                }
                if (hijo.Famcodi == 38) {
                    nombreListaEquipo = "Componente de Central Eólica";
                }
            }

            // llamar lista equipos
            var listaEquipo_ = listaEquipo.filter(x => x.Famcodi == hijo.Famcodi);

            //Filtrar lista de equipos por combo empresa
            if (TIPO_EQ_FICHA_SELECCIONADO == 1 && codEmpresa != -2) {
                listaEquipo_ = listaEquipo_.filter(x => x.Emprcodi == codEmpresa);
            }
            //

            //Filtrar por estado
            if (strEstado != "-2") {
                listaEquipo_ = listaEquipo_.filter(x => x.Equiestado == strEstado);
            }

            if (listaEquipo_.length > 0) {

                var clasehijosEq = "tbhijos";
                //Mostrar opción Ver más
                var htmlVermasmenos = "";
                if (listaEquipo_.length > 5 && TIPO_EQ_FICHA_SELECCIONADO == 1) {
                    clasever = "mas";
                    htmlVermasmenos = `<label class="${clasever}" onclick="verFilasTabla(this, ${hijo.Famcodi});"  id="vermasmenos_${contadorEquipo}" >Ver más</label>`;
                }

                var htmlThEmpresa = "";
                if (TIPO_EQ_FICHA_SELECCIONADO == 1) {
                    htmlThEmpresa = `<th style="width:150px;">Empresa</th>`;
                }
                else {
                    clasehijosEq = "tbhijos2";
                }

                htmlHijo += `<div class="tabla_hijo titulo">${nombreListaEquipo}</div>
                         <div style="height:auto; width: 800px; margin: 0 auto;">
                        <table class="tabla_hijo pretty tabla-icono ${clasehijosEq} tboculto" id="hijoeq_${hijo.Famcodi}" style="width: 800px;">
                        ${htmlVermasmenos}
                             <thead>
                                <tr>
                                <th style="width:70px;">Código</th>
                                <th style="width:350px;">Nombre</th>
                                <th style="width:150px;">Abreviatura</th>
                                ${htmlThEmpresa}
                                <th style="width:100px;">Estado</th>
                                <th style="width:60px;">Oculto Intranet</th>
                                <th style="width:70px;"></th>
                                </tr>
                            </thead>
                                <tbody>
                        `;

                for (var j = 0; j < listaEquipo_.length; j++) {
                    var equipo = listaEquipo_[j];

                    var claseFila = "";
                    var htmlTdEmpresa = "";
                    if (TIPO_EQ_FICHA_SELECCIONADO == 1) {
                        htmlTdEmpresa = `<td class="abrev">${equipo.Emprnomb}</td>`;
                    }

                    var htmlTdOcultoEqIntranet = `<td class=""><span id="ocultoInt_${equipo.Equicodi} "></span></td>`;
                    if (equipo.EstadoDesc == "Activo") {

                        if (equipo.FtverocultoIntranet == "S") {
                            htmlTdOcultoEqIntranet = `<td class=""><span id="ocultoInt_${equipo.Equicodi}"> ${IMG_OCULTO_INTRANET_}</span></td>`;
                        }
                        claseFila = "context-menu-fila-activo";
                    }
                    else {
                        htmlTdOcultoEqIntranet = `<td class=""><span id="ocultoInt_${equipo.Equicodi} ">Oculto</span></td>`;
                    }


                    htmlHijo += `
                    <tr id="fila_${equipo.Equicodi}_${hijo.Fteqcodi}" class=${claseFila}>
                    <td>${equipo.Equicodi}</td>
                    <td class="nombre">${equipo.Equinomb}</td>
                    <td class="abrev">${equipo.Equiabrev}</td>
                    ${htmlTdEmpresa}
                    <td class="">${equipo.EstadoDesc}</td>
                    ${htmlTdOcultoEqIntranet}
                    <td>
                        <a href="javascript:verDetalle('${hijo.Fteqcodi}', '${equipo.Equicodi}');">Ver Contenido</a>
                    </td>
                    </tr>
                `;
                }

                htmlHijo += `
                    </tbody>
                    </table>
                    </div>
                `;
            }
        }
        else {
            //Modo de operación
            var nombreListaGrupo = hijo.Catenomb;
            var listaGrupo_ = [];

            if (fichaTecnicaPadre.Famcodi != null && hijo.Catecodi == 2) {

                nombreListaGrupo = "Modos de Operación";

                listaGrupo_ = listaGrupo;
            }

            //Filtrar por estado
            if (strEstado != "-2") {
                listaGrupo_ = listaGrupo_.filter(x => x.GrupoEstado == strEstado);
            }

            if (listaGrupo_.length > 0) {
                var clasehijoGr = "tbhijos2";
                //Mostrar opción Ver más
                var htmlVermasmenosGrupo = "";
                if (listaGrupo_.length > 5 && TIPO_EQ_FICHA_SELECCIONADO == 1) {
                    clasever = "mas";
                    htmlVermasmenosGrupo = `<label class="${clasever}" onclick="verFilasTabla(this, ${hijo.Catecodi});"  id="vermasmenosgr_${contadorGrupo++}" >Ver más</label>`;
                }


                htmlHijo += `<div class="tabla_hijo titulo">${nombreListaGrupo}</div>
                         <div style="height:auto; width: 800px; margin: 0 auto;">
                        <table class="tabla_hijo pretty tabla-icono ${clasehijoGr} tboculto2"  id="hijoeq_${hijo.Catecodi}" style="width: 800px;">
                        ${htmlVermasmenosGrupo}
                             <thead>
                                <tr>
                                <th style="width:70px;">Código</th>
                                <th style="width:350px;">Nombre</th>
                                <th style="width:150px;">Abreviatura</th>
                                <th style="width:100px;">Estado</th>
                                <th style="width:60px;">Oculto Intranet</th>
                                <th style="width:70px;"></th>
                                </tr>
                            </thead>
                                <tbody>
                        `;

                for (var k = 0; k < listaGrupo_.length; k++) {
                    var grupo = listaGrupo_[k];
                    var claseFilaGr = "";

                    var htmlTdOcultogrIntranet = `<td class=""><span id="ocultoInt_${grupo.Grupocodi} "></span></td>`;
                    if (grupo.GrupoEstadoDesc == "Activo") {
                        if (grupo.FtverocultoIntranet == "S") {
                            htmlTdOcultogrIntranet = `<td class=""><span id="ocultoInt_${grupo.Grupocodi}"> ${IMG_OCULTO_INTRANET_}</span></td>`;
                        }
                        claseFilaGr = "context-menu-fila-activo";
                    }
                    else {
                        htmlTdOcultogrIntranet = `<td class=""><span id="ocultoInt_${grupo.Grupocodi} ">Oculto</span></td>`;
                    }


                    htmlHijo += `
                    <tr id="fila_${grupo.Grupocodi}_${hijo.Fteqcodi}" class=${claseFilaGr}>
                    <td>${grupo.Grupocodi}</td>
                    <td class="nombre">${grupo.Gruponomb}</td>
                    <td class="abrev">${grupo.Grupoabrev}</td>
                    <td class="">${grupo.GrupoEstadoDesc}</td>
                    ${htmlTdOcultogrIntranet}
                    <td>
                        <a href="javascript:verDetalle('${hijo.Fteqcodi}', '${grupo.Grupocodi}');">Ver Contenido</a>
                    </td>
                    </tr>
                `;
                }

                htmlHijo += `
                    </tbody>
                    </table>
                    </div>
                `;
            }
        }
    }
    return htmlHijo;
}

function _getHtmlItemUnidad(unidad) {
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

function _getHtmlItemValor(reg) {
    if (reg.Valor == null || reg.Valor == "") {
        return '';
    }

    var formatedo = reg.Valor.trim();

    if (reg.EsArchivo) {
        formatedo = getHtmlCeldaValor(reg.Valor);
    }

    return formatedo;
}

function _getHtmlItemSustento(reg) {
    if (reg.ItemSustento == null || reg.ItemSustento == "") {
        return '';
    }

    var formatedo = '';
    formatedo = getHtmlCeldaSustento(reg.ItemSustento);

    return formatedo;
}

function getHtmlCeldaSustento(urlSustento) {
    var arrayLink = getListaEnlaceXTexto(urlSustento);
    var htmlDiv = '';
    for (var i = 0; i < arrayLink.length; i++) {
        var link = arrayLink[i];
        var esConfidencial = (link.toLocaleUpperCase()).includes('DescargarConfidencial?'.toLocaleUpperCase());
        var textoTitle = esConfidencial ? 'Descargar archivo de sustento (CONFIDENCIAL): ' + link : 'Descargar archivo de sustento: ' + link ;

        htmlDiv += `
                <a style="link" onclick='descargarArchivoSustento("${link}")'><img src="${siteRoot}Content/Images/file.png" alt="Sustento" title='${textoTitle}'/></a>
            `;
    }

    return htmlDiv;
}

function getListaEnlaceXTexto(texto) {
    if (texto == null || texto == undefined) texto = "";
    texto = texto.trim();

    texto = texto.replace(/(?:\r\n|\r|\n)/g, ' ');
    texto = texto.replace(/(\n)+/g, ' ');

    var arrayLink = [];
    var arraySep = texto.split(' ');
    for (var i = 0; i < arraySep.length; i++) {
        var posibleLink = arraySep[i].trim();
        if (posibleLink.length > 0 && (
            posibleLink.toLowerCase().startsWith('http') || posibleLink.toLowerCase().startsWith('www'))) {
            arrayLink.push(posibleLink);
        }
    }

    return arrayLink;
}

function descargarArchivoSustento(urlSustento) {
    if (urlSustento != "" && urlSustento != null) {
        window.open(urlSustento, '_blank').focus();
    }
}

function getHtmlCeldaValor(urlValor) {
    var arrayLink = getListaEnlaceXTexto(urlValor);
    var htmlDiv = '';
    for (var i = 0; i < arrayLink.length; i++) {
        var link = arrayLink[i];
        var textoTitle = 'Archivo: ' + link;

        htmlDiv += `
                <a style="link" onclick='descargarArchivoValor("${link}")'><img src="${siteRoot}Content/Images/file.png" alt="Archivo" title='${textoTitle}'/></a>
            `;
    }

    return htmlDiv;
}

function descargarArchivoValor(urlValor) {
    if (urlValor != "" && urlValor != null) {

        window.open(urlValor, '_blank').focus();
    }
}

function profundidadHorizontalRecursivo(listaAllData) {
    var tipoAgrup = 1;
    var listaProfundidad = [];
    if (listaAllData != null) {
        listaAllData.sort((x, y) => x.Ftitorden - y.Ftitorden); // ordenamiento

        for (var i = 0; i < listaAllData.length; i++) {
            var reg = listaAllData[i];

            if (reg.Nivel == 1) {
                var profundidadRecursivo = profundidadHorizontalRecursivo(reg.ListaHijos);
                listaProfundidad.push((1 + profundidadRecursivo));
            } else {
                if (tipoAgrup == reg.Ftittipoitem) {
                    var profundidadRecursivo = profundidadHorizontalRecursivo(reg.ListaHijos);
                    listaProfundidad.push((2 + profundidadRecursivo));
                } else {
                    if (reg.ListaHijos.length > 0) {
                        var profundidadRecursivo = profundidadHorizontalRecursivo(reg.ListaHijos);
                        listaProfundidad.push((1 + profundidadRecursivo));
                    } else {
                        listaProfundidad.push(3);
                    }
                }
            }
        }
    }

    if (listaProfundidad.length > 0) {
        var m = MyMax(listaProfundidad);
        return m;
    }
    else {
        return 0;
    }

}

function profundidadVerticalRecursivo(listaHijos) {

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
                listaProfundidad.push(filaAdicional + profundidadVerticalRecursivo(reg.ListaHijos));
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

function MyMax(myarr) {
    var al = myarr.length;
    maximum = myarr[al - 1];
    while (al--) {
        if (myarr[al] > maximum) {
            maximum = myarr[al]
        }
    }
    return maximum;
};

function cargarEventoHijos() {

    $('#cbEmpresaDetalle').change(function () {
        $("#detalle_ficha_tecnica").html();
        var codEmpr = parseInt($("#cbEmpresaDetalle").val());
        var strEstado = $("#cbEstado").val();

        if (DETALLE_FT_GLOBAL.FichaTecnica != null) {
            var htmlHijo = _getHtmlDetalleFichaTecnicaHijo(DETALLE_FT_GLOBAL.IdFicha, DETALLE_FT_GLOBAL.ListaHijo, DETALLE_FT_GLOBAL.FichaTecnica, DETALLE_FT_GLOBAL.ListaEquipo, DETALLE_FT_GLOBAL.ListaEquipo2, DETALLE_FT_GLOBAL.ListaGrupo, codEmpr, strEstado);
            $('#reportHijos').html('');
            $('#reportHijos').html(htmlHijo);

            cargarEventoHijos();
        }
    });

    $('#cbEstado').change(function () {
        $("#detalle_ficha_tecnica").html();
        var codEmpr = parseInt($("#cbEmpresaDetalle").val());
        var strEstado = $("#cbEstado").val();

        if (DETALLE_FT_GLOBAL.FichaTecnica != null) {
            var htmlHijo = _getHtmlDetalleFichaTecnicaHijo(DETALLE_FT_GLOBAL.IdFicha, DETALLE_FT_GLOBAL.ListaHijo, DETALLE_FT_GLOBAL.FichaTecnica, DETALLE_FT_GLOBAL.ListaEquipo, DETALLE_FT_GLOBAL.ListaEquipo2, DETALLE_FT_GLOBAL.ListaGrupo, codEmpr, strEstado);
            $('#reportHijos').html('');
            $('#reportHijos').html(htmlHijo);

            cargarEventoHijos();
        }
    });

    $('.tbhijos').unbind();
    $('.tbhijos').dataTable({
        "sDom": 'ft',
        "searching": true,
        "ordering": false,
        "paging": true,
        "pageLength": 5,
    });

    $('.tbhijos2').unbind();
    $('.tbhijos2').dataTable({
        "sDom": 'ft',
        "searching": true,
        "ordering": false,
        "paging": true,
        "pageLength": -1,
    });

    eventoContextMenuHijosActivo();

};

function verFilasTabla(id, codigo) {
    var idtabla = "#hijoeq_" + codigo;


    var filas = 5;
    var clase = $(id).attr("class");

    $(idtabla).DataTable().destroy();

    if (clase == "mas") {

        $(idtabla).dataTable({
            "sDom": 'ft',
            "searching": true,
            "ordering": false,
            "paging": true,
            "pageLength": -1,
            lengthMenu: [
                [5, -1],
                ['Ver menos', 'Ver más'],
            ],
        });

        // cambiar a ver menos
        id.className = "menos"
        id.textContent = "Ver menos"

    }
    else {

        $(idtabla).dataTable({
            "sDom": 'ft',
            "searching": true,
            "ordering": false,
            "paging": true,
            "pageLength": 5,
            lengthMenu: [
                [5, -1],
                ['Ver menos', 'Ver más'],
            ],
        });

        // cambiar a ver más
        id.className = "mas"
        id.textContent = "Ver más"
    }

}

function eventoContextMenuHijosActivo() {
    var listaEq = DETALLE_FT_GLOBAL.ListaEquipo;
    var objItems = {};

    objItems = {
        "ocultarInt": { name: "Ocultar [Intranet]. " },
        "visualizarInt": { name: "Visualizar [Intranet]. " }
    };

    $('.tboculto').unbind();
    $('.tboculto').contextMenu({
        selector: '.context-menu-fila-activo',
        callback: function (key, options) {

            var idElement = $(this).attr("id").split("_")[1];
            var idElementFicha = $(this).attr("id").split("_")[2];

            var equipo = listaEq.find((eq) => eq.Equicodi == idElement);

            //Actualiza los equipos ocultos
            if (key == "ocultarInt" && equipo.FtverocultoIntranet != "S") {
                modificarVisibilidadEqModos(idElement, "S", idElementFicha, 3)
            }
            if (key == "visualizarInt" && equipo.FtverocultoIntranet != "N") {
                modificarVisibilidadEqModos(idElement, "N", idElementFicha, 3)
            }
        },
        items: objItems
    });

    var listaGr = DETALLE_FT_GLOBAL.ListaGrupo;
    var objItems2 = {};

    objItems2 = {
        "ocultarInt": { name: "Ocultar [Intranet]. " },
        "visualizarInt": { name: "Visualizar [Intranet]. " }
    };

    $('.tboculto2').unbind();
    $('.tboculto2').contextMenu({
        selector: '.context-menu-fila-activo',
        callback: function (key, options) {

            var idElement = $(this).attr("id").split("_")[1];
            var idElementFicha = $(this).attr("id").split("_")[2];

            var grupo = listaGr.find((gr) => gr.Grupocodi == idElement);

            //Actualiza los equipos ocultos
            if (key == "ocultarInt" && grupo.FtverocultoIntranet != "S") {
                modificarVisibilidadEqModos(idElement, "S", idElementFicha, 3)
            }
            if (key == "visualizarInt" && grupo.FtverocultoIntranet != "N") {
                modificarVisibilidadEqModos(idElement, "N", idElementFicha, 3)
            }
        },
        items: objItems2
    });
}

function modificarVisibilidadEqModos(idElement, opcion, idElementFicha, tipoOculto) {

    //var codFT = DETALLE_FT_GLOBAL.FichaTecnica.Fteqcodi;

    $.ajax({
        type: 'POST',
        url: controlador + "UpdateVisibilidadEquiposModos",
        data: {
            idElemento: idElement,
            idFT: idElementFicha,
            opcion: opcion,
            tipoOculto: tipoOculto
        },
        success: function (evt) {
            var tipo = $("#hfTipoElemento").val();
            var idTipo = $("#hfTipoElementoId").val();
            var idElemento = $("#hfCodigo").val();
            var idFicha = $("#hfCodigoFicha").val();

            var fecha = $("#hfFecha").val();

            $("#detalle_ficha_tecnica").html('');
            GenerarDetalleFT(tipo, idTipo, idElemento, idFicha, anchoPortal, 1, fecha);
        },
        error: function (err) {
            alert('Ha ocurrido un error.');
        }
    });
}