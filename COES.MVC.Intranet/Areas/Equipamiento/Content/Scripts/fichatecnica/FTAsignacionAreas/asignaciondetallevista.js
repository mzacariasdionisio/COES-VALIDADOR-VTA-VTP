var TREE_DATA = null;
var FICHA_GLOBAL = null;
var OPCION_ACTUAL = 0;
var anchoPortal = 1200; //1200px es el ancho del portal. Este debe ser el mismo que se mostrará en Intranet
var ancho = 1200;

//Iniciar vista Previa
var CORRELATIVO_TEMPORAL_ITEM = 1000;



function iniciarVistaPrevia() {

    //$('#tab-container2').easytabs('select', '#tabFormato');

    $("#detalle_ficha_tecnica").html('');

    var objModel = {};
    var listaItemJson = ListarTreeItemsFichaTecnica();
    var listaAgrupada = [];
    var listaTotal = [];

    //convertir a lista de items FictecItem
    _mapearItemsTotales(listaTotal, listaItemJson, -1);
    _listarTmpTreeItemsFichaTecnica(listaTotal, listaAgrupada, -1, "", 1);

    objModel.ListaAllItems = listaTotal;
    objModel.ListaTreeItems = listaAgrupada[0];
    objModel.ListaNota = FICHA_GLOBAL.ListaNota;
    objModel.IdFichaMaestra = 0;
    objModel.ListaHijo = [];
    objModel.FichaTecnica = FICHA_GLOBAL.FichaTecnica;
    objModel.ListaEquipo = [];
    objModel.ListaEquipo2 = [];
    objModel.ListaGrupo = [];

    var htmlVistPrevia = generarHtmlReporteDetalleFichaTecnica(objModel);
    /*$("#detalle_ficha_tecnica").css("width", ancho + "px");*/
    $("#detalle_ficha_tecnica").css("width", "auto");
    $("#detalle_ficha_tecnica").html(htmlVistPrevia);

    agregarColumnasFila();
}

function ListarTreeItemsFichaTecnica() {
    var padre = {};
    lJsonFinal = [];
    var lista = $("#treeFT").fancytree("getTree").rootNode.children;
    ListarTreeItemsFichaTecnicaRecursivo(lista, padre);
    
    return lJsonFinal;
}


function ListarTreeItemsFichaTecnicaRecursivo(hijos, padreJson) {
    if (hijos != null) {
        var hijosTmp = [];
        for (var i = 0; i < hijos.length; i++) {
            var item = GetItemJsonFichaTecnica(hijos[i]);

            //if (padreJson.folder == false) {
            //    item.data.Ftittipoitem = 2;
            //}

            hijosTmp.push(item);
        }

        if (padreJson.title == undefined) {
            lJsonFinal = hijosTmp;
        } else {
            padreJson.children = hijosTmp;
        }

        for (var j = 0; j < hijosTmp.length; j++) {
            if (hijos[j].children != null && hijos[j].children.length > 0) {
                this.ListarTreeItemsFichaTecnicaRecursivo(hijos[j].children, hijosTmp[j]);
            }
        }
    }
}

function GetItemJsonFichaTecnica(obj) {
    var reg = {};
    reg.title = obj.title;
    reg.folder = obj.folder;
    reg.expanded = obj.expanded;
    var data = {};
    if (obj.data == null || obj.data.Ftitcodi == undefined) {
        data = {
            Ftitcodi: 0
        };
    }
    else {
        data.Ftitcodi = obj.data.Ftitcodi;
        data.Propcodi = obj.data.Propcodi;
        data.Ftitorden = obj.data.Ftitorden;
        data.Concepcodi = obj.data.Concepcodi;
        data.Ftpropcodi = obj.data.Ftpropcodi;
        data.Fteqcodi = obj.data.Fteqcodi;
        data.Ftitnombre = obj.data.Ftitnombre;
        data.Ftitdet = obj.data.Ftitdet;
        data.Ftitpadre = obj.data.Ftitpadre;
        data.Ftitorientacion = obj.data.Ftitorientacion;
        data.Ftittipoitem = obj.data.Ftittipoitem;

        data.ListaHijos = [];
        data.Origen = obj.data.Origen;
        data.OrigenTipo = obj.data.OrigenTipo;

        data.ListaNotacodi = obj.data.ListaNotacodi;
        data.ListaNotanum = obj.data.ListaNotanum;
        if (obj.data.ItemUnidad != null && obj.data.ItemUnidad != '')
            data.ItemUnidadDesc = `[${obj.data.ItemUnidad}]`; //unidad
        data.EsTipoArchivo = obj.data.EsArchivo != null ? (obj.data.EsArchivo ? "S" : "N") : "X";
    }
    reg.data = data;
    reg.children = [];

    return reg;
}

function ListarTreeItemsFichaTecnicaRecursivo(hijos, padreJson) {
    if (hijos != null) {
        var hijosTmp = [];
        for (var i = 0; i < hijos.length; i++) {
            var item = GetItemJsonFichaTecnica(hijos[i]);

            //if (padreJson.folder == false) {
            //    item.data.Ftittipoitem = 2;
            //}

            hijosTmp.push(item);
        }

        if (padreJson.title == undefined) {
            lJsonFinal = hijosTmp;
        } else {
            padreJson.children = hijosTmp;
        }

        for (var j = 0; j < hijosTmp.length; j++) {
            if (hijos[j].children != null && hijos[j].children.length > 0) {
                this.ListarTreeItemsFichaTecnicaRecursivo(hijos[j].children, hijosTmp[j]);
            }
        }
    }
}


function cargarMenuTree() {
    var data = $("#hfJsonTree").val();
    TREE_DATA = JSON.parse(data);

    var CLIPBOARD = null;
    var TMP_itemNodoOrigenEsAgrup = false;
    var TMP_itemNodoOrigenEsProp = false;
    var TMP_itemNodoOrigenEsSubprop = false;

    $("#treeFT").fancytree({
        checkbox: false,
        titlesTabbable: true,     // Add all node titles to TAB chain
        quicksearch: true,        // Jump to nodes when pressing first character
        source: TREE_DATA,

        extensions: ["edit", "dnd", "table", "gridnav"],

        dnd: {
            preventVoidMoves: true,
            preventRecursiveMoves: true,
            autoExpandMS: 400,
            dragStart: function (node, data) {
                //TMP_itemNodoOrigenEsAgrup = false;
                //TMP_itemNodoOrigenEsProp = false;
                //TMP_itemNodoOrigenEsSubprop = false;

                //nodo Origen
                var nodoOrigen = node;
                TMP_itemNodoOrigenEsAgrup = nodoOrigen.isFolder();
                TMP_itemNodoOrigenEsProp = !nodoOrigen.isFolder();
                TMP_itemNodoOrigenEsSubprop = false;

                var nodoPadre = nodoOrigen.parent;
                if (nodoPadre != undefined && nodoPadre != null) {
                    //si el padre es propiedad entonces el elemento seleccionado es subpropiedad
                    if (!nodoPadre.isFolder()) {
                        TMP_itemNodoOrigenEsSubprop = true;
                        TMP_itemNodoOrigenEsProp = false;
                    }
                }
                if (TMP_itemNodoOrigenEsProp) {
                    //propiedad origen valida si es que no tiene hijos
                    TMP_itemNodoOrigenEsProp = nodoOrigen.children == undefined || nodoOrigen.children == null || nodoOrigen.children.length == 0; //siempre debe tener uno (bug del fancytreenode)
                }

                return true;
            },
            dragEnter: function (node, data) {

                return true;
            },
            dragDrop: function (node, data) {
                //nodo destino
                var itemNodoDestinoEsAgrup = node.isFolder();
                var itemNodoDestinoEsProp = !node.isFolder();
                var itemNodoDestinoEsSubprop = false;

                var nodoPadre = node.parent;
                if (nodoPadre != undefined && nodoPadre != null) {
                    //si el padre es propiedad entonces el elemento seleccionado es subpropiedad
                    if (!nodoPadre.isFolder()) {
                        itemNodoDestinoEsSubprop = true;
                        itemNodoDestinoEsProp = false;
                    }
                }

                //Validar movimiento
                if (itemNodoDestinoEsAgrup) { //si el destino es folder entonces permitir todo
                    data.otherNode.moveTo(node, data.hitMode);
                }

                if (itemNodoDestinoEsProp) { //si el destino es propiedad entonces solo permitir mover subpropiedad o propiedad sin hijos
                    if (TMP_itemNodoOrigenEsSubprop || TMP_itemNodoOrigenEsProp) {
                        data.otherNode.moveTo(node, data.hitMode);
                    }
                }

                if (data.hitMode == "before" || data.hitMode == "after") {
                    //si el destino es arriba o abajo de la subpropiedad entonces mover
                    if (TMP_itemNodoOrigenEsSubprop && itemNodoDestinoEsSubprop) {
                        data.otherNode.moveTo(node, data.hitMode);
                    }

                    //si el destino es arriba o abajo de la propiedad entonces mover
                    if (TMP_itemNodoOrigenEsAgrup && itemNodoDestinoEsProp) {
                        data.otherNode.moveTo(node, data.hitMode);
                    }
                }

                UpdateOrdenTreeItemsFichaTecnica();
            }
        },
        edit: {
            triggerStart: ["f2", "shift+click", "mac+enter"],
            save: function (event, data) {
                return true;
            },
            close: function (event, data) {
                if (data.save && data.isNew) {
                    // Quick-enter: add new nodes until we hit [enter] on an empty title
                    //   $("#treeFT").trigger("nodeCommand", { cmd: "addSibling" });
                }
            }
        },
        table: {
            indentation: 20,
            nodeColumnIdx: 1,
        },
        gridnav: {
            autofocusInput: false,
            handleCursorKeys: true
        },
        createNode: function (event, data) {
            var node = data.node,
                $tdList = $(node.tr).find(">td");

            if (node.isFolder()) { //cuando es agrupamiento solo mostrar orientacion y nota
                $tdList.eq(3).prop("colspan", 5);
                $tdList.eq(4).remove();
                $tdList.eq(5).remove();
                $tdList.eq(6).remove();
                $tdList.eq(7).remove();
                //$tdList.eq(9).prop("colspan", 2);
            }
        },
        renderColumns: function (event, data) {
            var node = data.node,
                $tdList = $(node.tr).find(">td");

            var ordenNodo = node.data != null && node.data.Orden != undefined ? node.data.Orden : "";
            $tdList.eq(0).text(ordenNodo);  //$tdList.eq(0).text(node.getIndexHier()); //orden
            //.eq(1) es propio del plugin

            if (node.isFolder()) {
                var descOri = node.data != null && node.data.Ftitorientacion != undefined ? node.data.Ftitorientacion : "";
                $tdList.eq(2).html(descOri);
                var nota = node.data != null && node.data.ListaNotanum != undefined ? node.data.ListaNotanum : "";;
                $tdList.eq(8).html(nota);
                var lastUser = node.data != null && node.data.UltimaModificacionUsuarioDesc != undefined ? node.data.UltimaModificacionUsuarioDesc : "";;
                $tdList.eq(9).html(lastUser);
                var lastDate = node.data != null && node.data.UltimaModificacionFechaDesc != undefined ? node.data.UltimaModificacionFechaDesc : "";;
                $tdList.eq(10).html(lastDate);
            } else {
                var origen = node.data != null && node.data.OrigenDesc != undefined ? node.data.OrigenDesc : "";;
                $tdList.eq(3).html(origen);
                var codigo = node.data != null && node.data.OrigenTipo != undefined ? node.data.OrigenTipo : "";;
                $tdList.eq(4).html(codigo);
                var propiedad = node.data != null && node.data.OrigenTipoDesc != undefined ? node.data.OrigenTipoDesc : "";;
                $tdList.eq(5).html(propiedad);
                var unidad = node.data != null && node.data.ItemUnidad != undefined ? convertirStringToHtml(node.data.ItemUnidad) : "";;
                $tdList.eq(6).html(unidad);
                var tipo = node.data != null && node.data.ItemTipo != undefined ? convertirStringToHtml(node.data.ItemTipo) : "";;
                $tdList.eq(7).html(tipo);
                var nota = node.data != null && node.data.ListaNotanum != undefined ? node.data.ListaNotanum : "";;
                $tdList.eq(8).html(nota);
                var lastUser = node.data != null && node.data.UltimaModificacionUsuarioDesc != undefined ? node.data.UltimaModificacionUsuarioDesc : "";;
                $tdList.eq(9).html(lastUser);
                var lastDate = node.data != null && node.data.UltimaModificacionFechaDesc != undefined ? node.data.UltimaModificacionFechaDesc : "";;
                $tdList.eq(10).html(lastDate);
            }
        }
    }).on("nodeCommand", function (event, data) {
        // Custom event handler that is triggered by keydown-handler and
        // context menu:
        var refNode, moveMode,
            tree = $(this).fancytree("getTree"),
            node = tree.getActiveNode();

    });

    var itemEnabledAgrup = false;
    var itemEnabledProp = false;
    var itemEnabledSubprop = false;
    $("#treeFT").contextmenu({
        delegate: "span.fancytree-node",
        menu: [
            { title: "Editar", cmd: "rename", uiIcon: "ui-icon-pencil" },
            { title: "Eliminar", cmd: "remove", uiIcon: "ui-icon-trash" },
            { title: "----" },
            { title: "Nuevo Agrupamiento", cmd: "addNodoHermano", uiIcon: "ui-icon-plus", disabled: function (event, ui) { return !itemEnabledAgrup; } },
            { title: "Nueva Propiedad", cmd: "addNodoHijo", uiIcon: "ui-icon-arrowreturn-1-e", disabled: function (event, ui) { return !itemEnabledProp; } },
            { title: "Nueva Subpropiedad", cmd: "addNodoSubHijo", uiIcon: "ui-icon-arrowreturn-1-e", disabled: function (event, ui) { return !itemEnabledSubprop; } }
        ],
        beforeOpen: function (event, ui) {
            var node = $.ui.fancytree.getNode(ui.target);
            node.setActive();

            //inicializar
            itemEnabledAgrup = false;
            itemEnabledProp = false;
            itemEnabledSubprop = false;

            itemEnabledAgrup = node.isFolder();
            itemEnabledProp = node.isFolder();
            //itemEnabledSubprop = !node.isFolder();

            //Existe elemento padre
            var nodoPadre = node.parent;
            if (nodoPadre != undefined && nodoPadre != null) {
                //si el padre es folder y el elemento seleccionado es propiedad entonces el hijo es subpropiedad
                if (nodoPadre.isFolder() && !node.isFolder()) {
                    itemEnabledSubprop = true;
                }

                //si el padre es folder entonces sus hijos pueden ser folder
                if (nodoPadre.isFolder()) {
                    itemEnabledAgrup = true;
                }
            }

        },
        select: function (event, ui) {
            var that = this;

        }
    });

    $(".fancytree-container").toggleClass("fancytree-connectors", true);
}

function UpdateOrdenTreeItemsFichaTecnica() {
    var orden = "";
    var lista = $("#treeFT").fancytree("getTree").rootNode.children;
    UpdateOrdenTreeItemsFichaTecnicaRecursivo(lista, orden);

    $("#treeFT").fancytree("getRootNode").render(true, true); //forzar update de Tree
}

function UpdateOrdenTreeItemsFichaTecnicaRecursivo(hijos, ordenPadre) {
    if (hijos != null) {
        var ordenNum = 1;
        for (var i = 0; i < hijos.length; i++) {
            var item = hijos[i].data;
            item.Ftitorden = ordenNum + "";
            ordenNum++;
        }

        for (var j = 0; j < hijos.length; j++) {
            hijos[j].data.Orden = ordenPadre == "" ? hijos[j].data.Ftitorden : ordenPadre + "." + hijos[j].data.Ftitorden;
            if (hijos[j].children != null && hijos[j].children.length > 0) {
                this.UpdateOrdenTreeItemsFichaTecnicaRecursivo(hijos[j].children, hijos[j].data.Orden);
            }
        }
    }
}

function convertirStringToHtml(str) {
    if (str != undefined && str != null) {
        //return str.replace(/&/g, "&amp;").replace(/>/g, "&gt;").replace(/</g, "&lt;").replace(/"/g, "&quot;");
        return str.replace(/>/g, "&gt;").replace(/</g, "&lt;").replace("'", "&#39;");//.replace(/"/g, "&quot;");
    }

    return "";
}


function _mapearItemsTotales(listaTotal, dataTree, idPadre) {
    //convertir a ListaAllItems
    for (var i = 0; i < dataTree.length; i++) {
        var reg = dataTree[i];
        reg.data.Ftitnombre = reg.title.trim();
        reg.data.Ftitpadre = idPadre;

        CORRELATIVO_TEMPORAL_ITEM++;
        //reg.data.Ftitcodi = CORRELATIVO_TEMPORAL_ITEM;

        if (reg.folder) {
            reg.data.Ftittipoitem = 1;
        } else {
            reg.data.Ftittipoitem = 0;
        }

        listaTotal.push(reg.data);
    }

    for (var j = 0; j < dataTree.length; j++) {
        var item = dataTree[j];
        if (item.children.length > 0) {
            _mapearItemsTotales(listaTotal, item.children, item.data.Ftitcodi);
        }
    }
}

function _listarTmpTreeItemsFichaTecnica(listaAllData, listaFinal, idPadre, ordenPadre, nivel) {
    var hijos = listaAllData.filter(x => x.Ftitpadre == idPadre).sort((m, n) => m.Ftitorden - n.Ftitorden);
    if (hijos.length > 0) {
        if (-1 == idPadre) {
            listaFinal.push(hijos);
            //listaAgrupada.push(hijos);
        }
        else {
            var padre = listaAllData.find(x => x.Ftitcodi == idPadre);
            if (padre != null) {
                padre.ListaHijos = hijos;
            }
        }

        for (var j = 0; j < hijos.length; j++) {
            var reg = hijos[j];
            reg.Nivel = nivel;

            if (ordenPadre == "") {
                reg.Orden = reg.Ftitorden.toString();
            }
            else {
                reg.Orden = ordenPadre + "." + reg.Ftitorden.toString();
            }
            //reg.Orden = ordenPadre == "" ? reg.Ftitorden.ToString() : ordenPadre + "." + reg.Ftitorden.ToString();

            this._listarTmpTreeItemsFichaTecnica(listaAllData, listaFinal, reg.Ftitcodi, reg.Orden, nivel + 1);
        }
    }
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
        htmlHijo = _getHtmlDetalleFichaTecnicaHijo(model.IdFicha, model.ListaHijo, model.FichaTecnica, model.ListaEquipo, model.ListaEquipo2, model.ListaGrupo, -2);
    }

    var htmlCab = _getHtmlCabecera(model, colspan);

    var html = `
        <table id="reporte" class="pretty tabla-icono tree_ficha_tecnica" style="overflow-y: hidden; overflow-x: auto; width: auto; table-layout: fixed;">
            <tbody>
                ${htmlCab}
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
                    var miPropcodi = reg.Propcodi != null ? reg.Propcodi : "N";
                    var miConcepcodi = reg.Concepcodi != null ? reg.Concepcodi : "N";
                    var miNombre = reg.Ftitnombre != null ? reg.Ftitnombre : "";
                                        
                    htmlItem += `<tr id="${orden}_${reg.Ftitcodi}_${miPropcodi}_${miConcepcodi}_${reg.EsTipoArchivo}" data-nombfila="${orden} ${miNombre}">`;
                    //htmlItem += `<tr id="OIf1ro_${orden}_${reg.Ftitcodi}_${miPropcodi}_${miConcepcodi}_${reg.EsTipoArchivo}">`;
                }
                var colspanFila = colspanDisponible - (reg.Nivel == 1 ? 1 : 2) + 1; //se agrega 1 por la columna adicional de valor
                htmlItem += `
                     <td class="${claseAgrupamiento} orden">${orden}</td>
                     <td colspan="${colspanFila}" class="${claseAgrupamiento} nombre">${reg.Ftitnombre} <span class="nota_agrup">${reg.ListaNotanum}</span> </td>
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
                        ${_getHtmlItem(listaAllData, reg.ListaHijos, reg.Ftitcodi, false, reg.Nivel + 1, numMaxCol, colspanDisponibleSig)}
                    `;
                    //htmlItem += `
                    //<tr id="oIf2do_${UltimoOrdenHijos}_${UltimoFitcodi}_${UltimoPropcodi}_${UltimoConcepcodi}_${UltimoEsArchivo}">

                    //    <td rowspan="${rowspan}"></td>
                    //    ${_getHtmlItem(listaAllData, reg.ListaHijos, reg.Ftitcodi, false, reg.Nivel + 1, numMaxCol, colspanDisponibleSig)}
                    //`;
                }

            }
            else {
                //obtener profundidad vertical
                var rowspan = profundidadVerticalRecursivo(reg.ListaHijos);
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
                
                var miPropcodi4 = reg.Propcodi != null ? reg.Propcodi : "N";
                var miConcepcodi4 = reg.Concepcodi != null ? reg.Concepcodi : "N";
                var miNombre4 = reg.Ftitnombre != null ? reg.Ftitnombre : "";
                
                htmlItem += `<tr id="${orden}_${reg.Ftitcodi}_${miPropcodi4}_${miConcepcodi4}_${reg.EsTipoArchivo}" data-nombfila="${orden} ${miNombre4}">`;
                //htmlItem += `<tr id="OElse_${orden}_${reg.Ftitcodi}_${miPropcodi4}_${miConcepcodi4}_${reg.EsTipoArchivo}">`;
            }

            var htmlItemUnidad = _getHtmlItemUnidad(reg.ItemUnidadDesc);

            var htmlItemValor = '';
            if (OPCION_ACTUAL != 2) {
                htmlItemValor = _getHtmlItemValor(reg);
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

            htmlItem += `
                     <td rowspan="${rowspan}" style="background: ${colorCelda};" class="${clasePropiedad} orden">${orden}</td>
                     <td colspan="${colspanDispItem}" style="background: ${colorCelda};" class="${clasePropiedad} nombre">${reg.Ftitnombre} <span class="nota_item">${reg.ListaNotanum} ${htmlcomentario}</span> </td>
                     <td class="${clasePropiedad} unidad" style="background: ${colorCelda};">${htmlItemUnidad}</td>
                     <td class="${clasePropiedad} valor" style="background: ${colorCelda};">${htmlItemValor}</td>
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


function _getHtmlDetalleFichaTecnicaHijo(idFicha, listaHijo, fichaTecnicaPadre, listaEquipo, listaEquipo2, listaGrupo, codEmpresa) {
    var htmlHijo = '';

    var contadorEquipo = 1;
    var contadorGrupo = 1;
    var clasever = "";
    for (var i = 0; i < listaHijo.length; i++) {
        var hijo = listaHijo[i]

        if (hijo.Famcodi != null) {
            var nombreListaEquipo = hijo.Famnomb;
            var listaFamcodi = [4, 5, 37, 39];

            if (fichaTecnicaPadre.Famcodi != null && listaFamcodi.includes(fichaTecnicaPadre.Famcodi)) {
                nombreListaEquipo = "Unidades de Generación";
                if (TIPO_SUBESTACION == 37) {
                    nombreListaEquipo = "Componente de Central Solar Fotovoltaica";
                }
                if (TIPO_SUBESTACION == 39) {
                    nombreListaEquipo = "Componente de Central Eólica";
                }
            }

            // llamar lista equipos
            var listaEquipo_ = listaEquipo.filter(x => x.Famcodi == hijo.Famcodi);

            //Filtrar lista de equipos por combo empresa
            if (TIPO_SUBESTACION == 1 && codEmpresa != -2) {
                listaEquipo_ = listaEquipo_.filter(x => x.Emprcodi == codEmpresa);
            }
            //

            if (listaEquipo_.length > 0) {

                var clasehijosEq = "tbhijos";
                //Mostrar opción Ver más
                var htmlVermasmenos = "";
                if (listaEquipo_.length > 5 && TIPO_SUBESTACION == 1) {
                    clasever = "mas";
                    htmlVermasmenos = `<label class="${clasever}" onclick="verFilasTabla(this, ${hijo.Famcodi});"  id="vermasmenos_${contadorEquipo}" >Ver más</label>`;
                }

                var htmlThEmpresa = "";
                if (TIPO_SUBESTACION == 1) {
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
                                <th style="width:60px;">Oculto</th>
                                <th style="width:70px;"></th>
                                </tr>
                            </thead>
                                <tbody>
                        `;

                for (var j = 0; j < listaEquipo_.length; j++) {
                    var equipo = listaEquipo_[j];

                    var htmlTdEmpresa = "";
                    if (TIPO_SUBESTACION == 1) {
                        htmlTdEmpresa = `<td class="abrev">${equipo.Emprnomb}</td>`;
                    }

                    var htmlTdOculto = `<td class=""><span id="oculto_${equipo.Equicodi} "></span></td>`;
                    if (equipo.Ftveroculto == "S") {
                        htmlTdOculto = `<td class=""><span id="oculto_${equipo.Equicodi}"> ${IMG_OCULTO_}</span></td>`;
                    }

                    htmlHijo += `
                    <tr id="fila_${equipo.Equicodi}_${hijo.Fteqcodi}" class="context-menu-fila">
                    <td>${equipo.Equicodi}</td>
                    <td class="nombre">${equipo.Equinomb}</td>
                    <td class="abrev">${equipo.Equiabrev}</td>
                    ${htmlTdEmpresa}
                    ${htmlTdOculto}
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
            var nombreListaGrupo = hijo.Catenomb;
            var abreviatura = "Abreviatura";
            var listaGrupo_ = [];

            if (fichaTecnicaPadre.Famcodi != null && hijo.Catecodi == 2) {

                nombreListaGrupo = "Modos de Operación";
                abreviatura = "Equipo/Comb.";

                listaGrupo_ = listaGrupo;
            }

            if (listaGrupo_.length > 0) {
                var clasehijoGr = "tbhijos2";
                //Mostrar opción Ver más
                var htmlVermasmenosGrupo = "";
                if (listaGrupo_.length > 5 && TIPO_SUBESTACION == 1) {
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
                                <th style="width:60px;">Oculto</th>
                                <th style="width:70px;"></th>
                                </tr>
                            </thead>
                                <tbody>
                        `;

                for (var k = 0; k < listaGrupo_.length; k++) {
                    var grupo = listaGrupo_[k];


                    var htmlTdOcultogr = `<td class=""><span id="oculto_${equipo.Grupocodi} "></span></td>`;
                    if (grupo.Ftveroculto == "S") {
                        htmlTdOcultogr = `<td class=""><span id="oculto_${equipo.Grupocodi}"> ${IMG_OCULTO_}</span></td>`;
                    }

                    htmlHijo += `
                    <tr id="fila_${grupo.Grupocodi}_${hijo.Fteqcodi}" class="context-menu-fila">
                    <td>${grupo.Grupocodi}</td>
                    <td class="nombre">${grupo.Gruponomb}</td>
                    <td class="abrev">${grupo.Grupoabrev}</td>
                    ${htmlTdOcultogr}
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
        return `
                <a style="link" href="${formatedo}"><img src="${siteRoot}Content/Images/file.png" alt="Archivo" title="Archivo" /></a>
               `;
    }

    return formatedo;
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
    var maximum = myarr[al - 1];
    while (al--) {
        if (myarr[al] > maximum) {
            maximum = myarr[al]
        }
    }
    return maximum;
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



function _getHtmlCabecera(model, colspan) {
    colspan = colspan + 1;
    var tipo = $("#hdTipoFichaSeleccionado").val();
    var cadena = '';
    cadena += `                    
               <tr id="1raF" >
                   <td colspan="${colspan}" class="agrup_raiz nombre">Ficha Técnica ${tipo} </td>
               </tr>
               <tr id="2daF">
                   <td colspan="${colspan}" class="agrup_raiz nombre"> </td>
               </tr>                                           
    `;

    return cadena;
}



