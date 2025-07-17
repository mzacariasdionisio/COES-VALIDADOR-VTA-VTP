var controlador = siteRoot + 'Equipamiento/FichaTecnica/';
var TREE_DATA = null;
var OPCION_VER = 1;
var OPCION_EDITAR = 2;
var OPCION_NUEVO = 3;

var TIPO_ITEM_PROP = "item";
var TIPO_ITEM_AGRUP = "agrp";

var ORIGEN_TIPO_EQUIPO = 1;
var ORIGEN_CATEGORIA_GRUPO = 2;
var ORIGEN_FICHA_TECNICA = 3;
var ORIGEN_TIPO_EQUIPO_DESC = "Tipo de Equipo";
var ORIGEN_CATEGORIA_GRUPO_DESC = "Categoría de Grupo";
var TIPO_PROPIEDAD_DESC = "Propiedad de Equipo";
var TIPO_CONCEPTO_DESC = "Concepto de Grupo";
var TIPO_FICHA_TECNICA_PROP_DESC = "Propiedad de Ficha Técnica";

var LISTA_EQ_PROPIEDAD = null;
var LISTA_PR_CONCEPTO = null;
var LISTA_EQ_PROPIEDAD_GLOBAL = [];
var LISTA_PR_CONCEPTO_GLOBAL = [];

var IMG_VER = `<img src="${siteRoot}Content/Images/btn-open.png" title="Ver Ficha Tecnica"/>`;
var IMG_EDITAR = `<img src="${siteRoot}Content/Images/btn-edit.png" title="Editar Ficha Tecnica"/>`;
var IMG_COPIAR = `<img src="${siteRoot}Content/Images/copiar.png" title="Copiar Ficha Tecnica"/>`;
var IMG_ELIMINAR = `<img src="${siteRoot}Content/Images/Trash.png" title="Eliminar Ficha Tecnica"/>`;

var FICHA_GLOBAL = null;
var HTML_BlANCO = "";
var ancho = 1200;
var listaAgrupada = [];
var listaTotal = [];

$(function () {
    $('#tab-container-config').easytabs({
        animate: false
    });
    $('#tab-container-config').bind('easytabs:after', function () {
        refrehDatatable();
    });

    $('#btnConsultar').on('click', function () {
        listarFichaTecnica();
    });

    $('#btnAgregar').on('click', function () {
        nuevaFichaTecnica();
    });

    listarFichaTecnica();
    HTML_BlANCO = $("#editarFichaTecnica").html();

    $("#btnManualUsuario").click(function () {
        window.location = controlador + 'DescargarManualUsuario';
    });
});

///////////////////////////
/// Consulta
///////////////////////////
function listarFichaTecnica() {
    $('#tab-container-config').easytabs('select', '#vistaListado');

    $.ajax({
        type: 'POST',
        url: controlador + "FichaTecnicaLista",
        dataType: 'json',
        data: {
        },
        success: function (result) {
            if (result.Resultado != "-1") {
                $('#listado').html('');
                //$('#listado').css("width", ANCHO_LISTADO + "px");
                var html = _dibujarTablaListado(result);
                $('#listado').html(html);
                refrehDatatable();
                //HTML_BlANCO = $("#editarFichaTecnica").html();
                viewEvent();

            } else {
                alert(result.Mensaje);
            }
        },
        error: function (err) {
            alert('Error: Se ha producido un error inesperado.');
        }
    });
}

function _dibujarTablaListado(model) {
    var lista = model.ListaFichaTecnica;

    var cadena = '';
    cadena += `
    <table border="0" class="pretty tabla-icono" id="TablaFichaTecnica" cellspacing="0" width="100%" >
        <thead>
            <tr>
                <th colspan="3"></th>
                <th colspan="3">Ficha Técnica</th>
                <th colspan="3" class="ficha_padre">Ficha Técnica Padre</th>
                <th colspan="2">Última modificación</th>
                <th></th>
            </tr>
            <tr>
                <th style="width: 120px">Acciones</th>
                <th style="width: 50px">Código</th>
                <th style="width: 50px">Oficial</th>
                <th style="width: 150px">Origen</th>
                <th style="width: 240px">Tipo</th>
                <th style="width: 400px">Nombre</th>
                <th style="width: 150px" class="ficha_padre">Origen</th>
                <th style="width: 240px" class="ficha_padre">Tipo</th>
                <th style="width: 330px" class="ficha_padre">Nombre</th>
                <th style="width: 120px">Usuario</th>
                <th style="width: 120px">Fecha</th>
                <th style="width: 120px">Estado</th>
            </tr>
        </thead>
        <tbody>
    `;

    for (key in lista) {
        var item = lista[key];

        var claseFila = "";
        if (item.Ftecprincipal == 1) { claseFila = "clase_principal"; }
        if (item.Fteqestado == "B" || item.Fteqestado == "X") { claseFila = "clase_eliminado"; }
        //var sStyle = item.EstiloEstado;
        var tdOpciones = _tdAcciones(item);

        cadena += `

            <tr id="fila_${item.Concepcodi}" class="${claseFila}">
                ${tdOpciones}
                <td style="text-align:center;">${item.Fteqcodi}</td>
                <td style="text-align:center;">${item.FtecprincipalDesc}</td>
                <td>${item.OrigenDesc}</td>
                <td class="ficha_tipo">${item.OrigenTipoDesc}</td>
                <td class="ficha_nombre">${item.Fteqnombre}</td>
                <td class="ficha_padre">${item.OrigenPadreDesc}</td>
                <td class="ficha_padre">${item.OrigenPadreTipoDesc}</td>
                <td class="ficha_padre">${item.Fteqnombrepadre}</td>
                <td>${item.UltimaModificacionUsuarioDesc}</td>
                <td>${item.UltimaModificacionFechaDesc}</td>
                <td>${item.FteqestadoDesc}</td>
            </tr>
        `;
    }
    cadena += "</tbody></table>";

    return cadena;
}

function _tdAcciones(item) {
    var html = '';

    html += `<td style="width: 120px">`;

    if (item.Fteqestado == "B") {
        html += `   
                <a href="#" id="view, ${item.Fteqcodi}" class="viewVer">
                    ${IMG_VER}
                </a>
                <a href="#" id="view, ${item.Fteqcodi}" class="viewEdicion">
                    ${IMG_EDITAR}
                </a>
                <a href="#" id="view, ${item.Fteqcodi}" class="viewCopiar">
                    ${IMG_COPIAR}
                </a>
        `;
    }
    else {
        html += `   
                <a href="#" id="view, ${item.Fteqcodi}" class="viewVer">
                    ${IMG_VER}
                </a>
                <a href="#" id="view, ${item.Fteqcodi}" class="viewEdicion">
                    ${IMG_EDITAR}
                </a>
                <a href="#" id="view, ${item.Fteqcodi}" class="viewCopiar">
                    ${IMG_COPIAR}
                </a>
                <a href="#" id="view, ${item.Fteqcodi}" class="viewEliminar">
                    ${IMG_ELIMINAR}
                </a>
        `;
    }

    return html;
}

function viewEvent() {
    $('.viewVer').click(function (event) {
        event.preventDefault();
        idft = $(this).attr("id").split(",")[1];
        verFichaTecnica(idft, false);
    });
    $('.viewEdicion').click(function (event) {
        event.preventDefault();
        idft = $(this).attr("id").split(",")[1];
        editarFichaTecnica(idft, true);
    });
    $('.viewCopiar').click(function (event) {
        event.preventDefault();
        idft = $(this).attr("id").split(",")[1];
        copiarFichaTecnica(idft, true);
    });
    $('.viewEliminar').click(function (event) {
        event.preventDefault();
        idft = $(this).attr("id").split(",")[1];
        eliminarFichaTecnica(idft, true);
    });
};

function refrehDatatable() {
    $('#TablaFichaTecnica').dataTable({
        "destroy": "true",
        "scrollY": 430,
        "scrollX": true,
        "sDom": 'ft',
        "ordering": false,
        "paging": false,
        "searching": true,
        "iDisplayLength": -1
    });

}

///////////////////////////
/// Edición
///////////////////////////

function copiarFichaTecnica(id) {
    if (confirm('¿Desea Copiar la Ficha Técnica?')) {
        $.ajax({
            type: 'POST',
            dataType: 'json',
            traditional: true,
            url: controlador + 'FichaTecnicaCopiar',
            data: {
                idFT: id
            },
            cache: false,
            success: function (result) {
                if (result.Resultado == '-1') {
                    alert('Ha ocurrido un error:' + result.Mensaje);
                } else {
                    alert("Se copió correctamente la Ficha Técnica");
                    listarFichaTecnica();
                }
            },
            error: function (err) {
                alert('Ha ocurrido un error.');
            }
        });
    }
}

function eliminarFichaTecnica(id) {
    if (confirm('¿Desea eliminar la Ficha Técnica?')) {
        $.ajax({
            type: 'POST',
            dataType: 'json',
            traditional: true,
            url: controlador + 'FichaTecnicaEliminar',
            data: {
                idFT: id
            },
            cache: false,
            success: function (result) {
                if (result.Resultado == '-1') {
                    alert('Ha ocurrido un error:' + result.Mensaje);
                } else {
                    alert("Se eliminó correctamente la Ficha Técnica");
                    listarFichaTecnica();
                }
            },
            error: function (err) {
                alert('Ha ocurrido un error.');
            }
        });
    }
}

function editarFichaTecnica(id) {
    formularioFichaTecnica(id, OPCION_EDITAR);
}

function verFichaTecnica(id) {
    formularioFichaTecnica(id, OPCION_VER);
}

function nuevaFichaTecnica() {
    formularioFichaTecnica(0, OPCION_NUEVO);
}

function formularioFichaTecnica(id, opcion) {
    $('#tab-container-config').easytabs('select', '#vistaDetalle');
    $('#editarFichaTecnica').html('');

    TREE_DATA = null;
    $.ajax({
        type: 'POST',
        url: controlador + "FichaTecnicaFormulario",
        dataType: 'json',
        data: {
            id: id
        },
        success: function (evt) {
            if (evt.Resultado != "-1") {
                FICHA_GLOBAL = evt;

                LISTA_EQ_PROPIEDAD_GLOBAL = evt.ListaPropiedad;
                LISTA_PR_CONCEPTO_GLOBAL = evt.ListaConcepto;
                evt.ListaPropiedad = null;
                evt.ListaConcepto = null;

                $('#editarFichaTecnica').html(HTML_BlANCO);
                $("#tab-cabecera").show();

                $('#tab-container').unbind();
                //$('#editarFichaTecnica').html(HTML_BlANCO);
                $('#mensaje').css("display", "none");

                inicializarHiddens(evt);
                inicializarFichaTecnica(opcion);
                inicializarNota(opcion);

                setTimeout(function () {
                    $('#tab-container').easytabs({
                        animate: false
                    });
                    $("#tab-container").easytabs({ updateHash: true, defaultTab: "li:eq(1)" });
                    $('#tab-container').easytabs('select', '#vistaTree');
                }, 50);
                
               
                $('#txtVigenciaExtranet').inputmask({
                    mask: "1/2/y",
                    placeholder: "dd/mm/yyyy",
                    alias: "datetime"
                });
                $('#txtVigenciaExtranet').Zebra_DatePicker({
                    readonly_element: false
                });

            } else {
                alert("Ha ocurrido un error: " + evt.Mensaje);
            }
        },
        error: function (err) {
            alert('Ha ocurrido un error.');
        }
    });
}

function inicializarHiddens(model) {
    $("#hfNombre").val(model.FichaTecnica.Fteqnombre);
    $("#hfCodigo").val(model.FichaTecnica.Fteqcodi);
    $("#hfFamcodi").val(model.FichaTecnica.Famcodi);
    $("#hfCatecodi").val(model.FichaTecnica.Catecodi);
    $("#hfJsonTree").val(model.TreeJson);
    $("#hfOrigen").val(model.FichaTecnica.Origen);
    $("#hfNumOrigenpadre").val(model.NumOrigenpadre);
    $("#hfCodigoPadre").val(model.FichaTecnica.Fteqpadre);
    $("#hfEstado").val(model.FichaTecnica.Fteqestado);
    $("#hfJsonNota").val(model.NotaJson);
    $("#hdVisibilidad").val(model.FichaTecnica.FlagVisibilidadExt);
    $("#hfFecVigencia").val(model.FichaTecnica.FechaVigenciaExt);
    
}

function inicializarFichaTecnica(opcion) {
    var origen = $("#hfOrigen").val();
    if (origen == ORIGEN_TIPO_EQUIPO) {
        $("#origen_1").prop("checked", true);
    } else {
        $("#origen_2").prop("checked", true);
    }

    //var numOrigenPadre = $("#hfNumOrigenpadre").val();
    var codigoPadre = parseInt($("#hfCodigoPadre").val()) || 0;
    $("#cbFichaPadre").val(codigoPadre);
    //if (numOrigenPadre > 0) {
    //    $("#tr_ficha_padre").show();
    //}
    $("#tr_ficha_padre").show();

    var estado = $("#hfEstado").val();
    $("#cbEstado").val(estado);

    


    switch (opcion) {
        case OPCION_VER:
            $("#tdVistaPrevia").show();
            $("#cbFamiliaEquipo").prop('disabled', 'disabled');
            $("#cbCategoriaGrupo").prop('disabled', 'disabled');
            $("#cbEstado").prop('disabled', 'disabled');
            $("#origen_1").prop('disabled', 'disabled');
            $("#origen_2").prop('disabled', 'disabled');
            $("#txtNombre").prop('disabled', 'disabled');
            $("#cbFichaPadre").prop('disabled', 'disabled');

            //$("#popupConfiguracion").css("width", "1200px");
            $("#cbFichaPadre").show();
            $(".tr_estado").show();

            $("#tab-container").show();

            //Listas desplegables
            inicializaDesplegables();
            if (FICHA_GLOBAL.FichaTecnica.Fteqpadre > 0) {
                $("#cbFichaPadre").val(FICHA_GLOBAL.FichaTecnica.Fteqpadre);
            }

            //Iniciar Vigencia y visibilidad Ext            
            if ($("#hdVisibilidad").val() != "") {
                if ($("#hdVisibilidad").val() == "S") 
                    document.getElementById("visSi").checked = true;
                else
                    document.getElementById("visNo").checked = true;                
            }
            $("#txtVigenciaExtranet").val($("#hfFecVigencia").val());
            document.getElementById("visSi").disabled = true;
            document.getElementById("visNo").disabled = true;
            document.getElementById("txtVigenciaExtranet").disabled = true;

            break;
        case OPCION_EDITAR:
            //$("#popupConfiguracion").css("width", "1200px");
            $("#tdGuardar").show();
            $("#tdVistaPrevia").show();
            $("#txtNombre").removeAttr('disabled');
            $("#cbFichaPadre").removeAttr('disabled');
            $("#cbFichaPadre").show();
            $(".tr_estado").show();
            $("#cbEstado").removeAttr('disabled');
            $("#tab-container").show();

            //Listas desplegables
            inicializaDesplegables();
            if (FICHA_GLOBAL.FichaTecnica.Fteqpadre > 0) {
                $("#cbFichaPadre").val(FICHA_GLOBAL.FichaTecnica.Fteqpadre);
            }

            //Iniciar Vigencia y visibilidad Ext
            
            if ($("#hdVisibilidad").val() != "") {
                if ($("#hdVisibilidad").val() == "S")
                    document.getElementById("visSi").checked = true;
                else
                    document.getElementById("visNo").checked = true;
            }
            $("#txtVigenciaExtranet").val($("#hfFecVigencia").val());
            document.getElementById("visSi").disabled = false;
            document.getElementById("visNo").disabled = false;
            document.getElementById("txtVigenciaExtranet").disabled = false;

            break;
        case OPCION_NUEVO:
            //$("#popupConfiguracion").css("width", "800px");
            $("#tdGuardar").show();
            $("#cbFamiliaEquipo").removeAttr('disabled');
            $("#cbCategoriaGrupo").removeAttr('disabled');
            $("#txtNombre").removeAttr('disabled');
            $("#origen_1").removeAttr('disabled');
            $("#origen_2").removeAttr('disabled');
            $("#cbFichaPadre").removeAttr('disabled');
            $("#cbFichaPadre").show();
            $("#tab-container").hide();

            $(".tr_estado").hide();
            //Listas desplegables
            inicializaDesplegables();

            //Iniciar Vigencia y visibilidad Ext
            document.getElementById("visNo").checked = true;
            document.getElementById("visSi").disabled = false;
            document.getElementById("visNo").disabled = false;
            document.getElementById("txtVigenciaExtranet").disabled = false;

            break;
    }

    //
    $("#btnVistaPrevia").unbind();
    $('#btnVistaPrevia').click(function () {
        iniciarVistaPrevia();
    });
    //
    $("#btnGuardar").unbind();
    $('#btnGuardar').click(function () {
        guardarFichaTecnica();
    });
    mostrarOrigenDatos(opcion);
    $('input[type=radio][name=origen]').unbind();
    $('input[type=radio][name=origen]').change(function () {
        mostrarOrigenDatos(opcion);
    });

    $("#cbFamiliaEquipo").val($("#hfFamcodi").val() || 0);
    $("#cbCategoriaGrupo").val($("#hfCatecodi").val() || 0);
    $("#cbEstado").val($("#hfEstado").val());
    $("#txtNombre").val($("#hfNombre").val());

    var id = $("#hfCodigo").val();

    $("#cbFamiliaEquipo").unbind();
    $("#cbFamiliaEquipo").change(function () {
        mostrarFichaPadre();
    });
    $("#cbCategoriaGrupo").unbind();
    $("#cbCategoriaGrupo").change(function () {
        mostrarFichaPadre();
    });

    switch (opcion) {
        case OPCION_VER:
            cargarMenuTreeLectura(id);
            break;
        default:
            cargarMenuTree(id);
            break;
    }
}

//Iniciar vista Previa
var CORRELATIVO_TEMPORAL_ITEM = 1000;
function iniciarVistaPrevia() {

    $('#tab-container').easytabs('select', '#vistaPrevia');
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
    $("#detalle_ficha_tecnica").css("width", ancho + "px");
    $("#detalle_ficha_tecnica").html(htmlVistPrevia);
}

function _mapearItemsTotales(listaTotal, dataTree, idPadre) {
    //convertir a ListaAllItems
    for (var i = 0; i < dataTree.length; i++) {
        var reg = dataTree[i];
        reg.data.Ftitnombre = reg.title.trim();
        reg.data.Ftitpadre = idPadre;

        CORRELATIVO_TEMPORAL_ITEM++;
        reg.data.Ftitcodi = CORRELATIVO_TEMPORAL_ITEM;

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

//Inicializa Listas Desplegables
function inicializaDesplegables() {
    $('#cbFamiliaEquipo').empty();
    var optionTipoEquipo = '<option value="-2" >-SELECCIONE-</option>';
    $.each(FICHA_GLOBAL.ListaFamilia, function (k, v) {
        optionTipoEquipo += '<option value =' + v.Famcodi + '>' + v.Famnomb + '</option>';
    })
    $('#cbFamiliaEquipo').append(optionTipoEquipo);

    $('#cbCategoriaGrupo').empty();
    var optionCategoria = '<option value="-2" >-SELECCIONE-</option>';
    $.each(FICHA_GLOBAL.ListaCategoria, function (k, v) {
        optionCategoria += '<option value =' + v.Catecodi + '>' + v.Catenomb + '</option>';
    })
    $('#cbCategoriaGrupo').append(optionCategoria);

    $('#cbEstado').empty();
    var optionEstado = '';
    $.each(FICHA_GLOBAL.ListaEstado, function (k, v) {
        optionEstado += '<option value =' + v.EstadoCodigo + '>' + v.EstadoDescripcion + '</option>';
    })
    $('#cbEstado').append(optionEstado);

    $('#cbFichaPadre').empty();
    var optionFichaPadre = '<option value="0">--[No Seleccionado]--</option>';
    $.each(FICHA_GLOBAL.ListaFichaTecnicaPadre, function (k, v) {
        optionFichaPadre += '<option value =' + v.Fteqcodi + '>' + v.OrigenTipoDesc + " - " + v.Fteqcodi + " " + v.Fteqnombre + '</option>';
    })
    $('#cbFichaPadre').append(optionFichaPadre);
}

function mostrarOrigenDatos(opcion) {
    var origen = $('input[name="origen"]:checked').val();
    if (origen == ORIGEN_TIPO_EQUIPO) {
        $("#cbCategoriaGrupo").hide();
        $("#cbFamiliaEquipo").show();
        switch (opcion) {
            case OPCION_VER:
                break;
            case OPCION_NUEVO:
                mostrarFichaPadre();
                break;
        }
    } else {
        $("#cbFamiliaEquipo").hide();
        $("#cbCategoriaGrupo").show();
        switch (opcion) {
            case OPCION_VER:
                break;
            case OPCION_NUEVO:
                mostrarFichaPadre();
                break;
        }
    }
}

function guardarFichaTecnica() {
    var entity = getObjetoJson();
    if (confirm('¿Desea guardar la Ficha Técnica?')) {
        var msj = validarTree(entity);

        if (msj == "") {
            var obj = JSON.stringify(entity.ListaItem);

            $.ajax({
                type: 'POST',
                dataType: 'json',
                traditional: true,
                url: controlador + 'FichaTecnicaGuardar',
                data: {
                    dataTree: obj,
                    idFT: entity.id,
                    famcodi: entity.Famcodi,
                    catecodi: entity.Catecodi,
                    nombre: entity.Nombre,
                    tipoOrigen: entity.Origen,
                    idFTPadre: entity.idPadre,
                    estado: entity.estado,
                    esVisibleExt: entity.VisibleExt,
                    fecVigenciaExt: entity.StrFecVigenciaExt
                },
                cache: false,
                success: function (result) {
                    if (result.Resultado == '-1') {
                        alert('Ha ocurrido un error:' + result.Mensaje);
                    } else {
                        alert("Se guardó correctamente la Ficha Técnica");
                        listarFichaTecnica();
                        if (entity.id != 0) {
                            formularioFichaTecnica(entity.id, OPCION_EDITAR);
                        } else {
                            $('#tab-container-config').easytabs('select', '#vistaListado');
                            $('#editarFichaTecnica').html('');
                        }
                    }
                },
                error: function (err) {
                    alert('Ha ocurrido un error.');
                }
            });
        } else {
            alert(msj);
        }
    }
}

function getObjetoJson() {
    var obj = {};

    obj.id = parseInt($("#hfCodigo").val()) || 0;
    obj.Nombre = $("#txtNombre").val()
    obj.Famcodi = parseInt($("#cbFamiliaEquipo").val()) || 0;
    obj.Catecodi = parseInt($("#cbCategoriaGrupo").val()) || 0;
    obj.ListaItem = ListarTreeItemsFichaTecnica();
    obj.Origen = parseInt($('input[name="origen"]:checked').val());
    obj.idPadre = parseInt($("#cbFichaPadre").val()) || 0;
    obj.idPadre = obj.idPadre > 0 ? obj.idPadre : null;
    obj.estado = $("#cbEstado").val();

    var sel = document.querySelector('input[name="rdVisibilidadExt"]:checked');
    obj.VisibleExt = sel != null ? sel.value : "";
    obj.StrFecVigenciaExt = $("#txtVigenciaExtranet").val();

    return obj;
}

function validarTree(obj) {
    var msj = "";
    if (obj.Nombre == null || obj.Nombre.trim() == "") {
        msj += "Debe ingresar nombre." + "\n";
    }

    if (obj.Origen == ORIGEN_TIPO_EQUIPO) {
        if (obj.Famcodi <= 0) {
            msj += "Debe seleccionar un Tipo de Equipo." + "\n";
        }
        obj.Catecodi = null;
    } else {
        if (obj.Catecodi <= 0) {
            msj += "Debe seleccionar un Categoria de Grupo." + "\n";
        }
        obj.Famcodi = null;
    }

    if (obj.ListaItem.length == 0) {
        msj += "La configuración debe tener al menos un item.";
    }

    //valido visibilidad y vigencia ingresada
    var fecha = obj.StrFecVigenciaExt;
    if (obj.VisibleExt == "") {
        msj += "Seleccione la visibilidad en extranet.";
    } else {
        if (obj.VisibleExt == "S") {
            if (fecha.trim() != "") {
                const myArray = fecha.split("/");
                let dia = myArray[0];
                let mes = myArray[1];
                let anio = myArray[2];

                if (isNaN(dia) || isNaN(mes) || isNaN(anio)) {
                    msj += "Debe ingresar una fecha de vigencia correcta." + "\n";
                }
            } else {
                msj += "Debe ingresar una fecha de vigencia.";
            }
        } else {
            if (fecha.trim() != "") {
                const myArray = fecha.split("/");
                let dia = myArray[0];
                let mes = myArray[1];
                let anio = myArray[2];

                if (isNaN(dia) || isNaN(mes) || isNaN(anio)) {
                    msj += "Debe ingresar una fecha de vigencia correcta." + "\n";
                }
            }
        }
    }
    

    return msj;
}

function mostrarFichaPadre() {
    var entity = getObjetoNuevoJson();
    $("#tr_ficha_padre").hide();
    $("#cbFichaPadre").empty();
    $("#cbFichaPadre").append('<option value="-1" selected="selected">[No Seleccionado]</option>');

    $.ajax({
        type: 'POST',
        dataType: 'json',
        traditional: true,
        url: controlador + 'ListaFichaTecnicaPadre',
        data: {
            famcodi: entity.Famcodi,
            catecodi: entity.Catecodi,
            tipoOrigen: entity.Origen
        },
        cache: false,
        success: function (result) {
            if (result.Resultado == '-1') {
                alert('Ha ocurrido un error:' + result.Mensaje);
            } else {
                for (var c = 0; c < result.ListaFichaTecnicaPadre.length; c++) {
                    var v = result.ListaFichaTecnicaPadre[c];
                    $("#cbFichaPadre").append('<option value =' + v.Fteqcodi + '>' + v.OrigenTipoDesc + " - " + v.Fteqcodi + " " + v.Fteqnombre + '</option>');
                }
                $("#tr_ficha_padre").show();
            }
        },
        error: function (err) {
            alert('Ha ocurrido un error.');
        }
    });
}

function getObjetoNuevoJson() {
    var obj = {};

    obj.id = parseInt($("#hfCodigo").val()) || 0;
    obj.Nombre = $("#txtNombre").val()
    obj.Famcodi = parseInt($("#cbFamiliaEquipo").val()) || 0;
    obj.Catecodi = parseInt($("#cbCategoriaGrupo").val()) || 0;
    obj.Origen = parseInt($('input[name="origen"]:checked').val());
    obj.idPadre = parseInt($("#cbFichaPadre").val()) || 0;
    obj.idPadre = obj.idPadre > 0 ? obj.idPadre : null;
    obj.estado = $("#cbEstado").val();

    return obj;
}

///////////////////////////
/// Tree
///////////////////////////

function cargarMenuTree(id) {
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

                nodoPadre = node.parent;
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

        switch (data.cmd) {
            case "moveUp":
                refNode = node.getPrevSibling();
                if (refNode) {
                    node.moveTo(refNode, "before");
                    node.setActive();
                }
                break;
            case "moveDown":
                refNode = node.getNextSibling();
                if (refNode) {
                    node.moveTo(refNode, "after");
                    node.setActive();
                }
                break;
            case "indent":
                refNode = node.getPrevSibling();
                if (refNode) {
                    node.moveTo(refNode, "child");
                    refNode.setExpanded();
                    node.setActive();
                }
                break;
            case "outdent":
                if (!node.isTopLevel()) {
                    node.moveTo(node.getParent(), "after");
                    node.setActive();
                }
                break;
            case "rename":
                if (node.isFolder()) {
                    editarNodeAgrupamiento(node);
                } else {
                    editarNodePropiedad(node);
                }
                break;
            case "remove":
                refNode = node.getNextSibling() || node.getPrevSibling() || node.getParent();
                node.remove();
                if (refNode) {
                    refNode.setActive();
                }
                break;
            case "addNodoHermano":
                node.editCreateNode("after", data.objeto);
                UpdateOrdenTreeItemsFichaTecnica();
                break;
            case "addNodoHijo":
                node.editCreateNode("child", data.objeto);
                UpdateOrdenTreeItemsFichaTecnica();
                break;
            case "addNodoSubHijo":
                node.editCreateNode("child", data.objeto);
                UpdateOrdenTreeItemsFichaTecnica();
                break;
            case "cut":
                CLIPBOARD = { mode: data.cmd, data: node };
                break;
            case "copy":
                CLIPBOARD = {
                    mode: data.cmd,
                    data: node.toDict(function (n) {
                        delete n.key;
                    })
                };
                break;
            case "clear":
                CLIPBOARD = null;
                break;
            case "paste":
                if (CLIPBOARD.mode === "cut") {
                    // refNode = node.getPrevSibling();
                    CLIPBOARD.data.moveTo(node, "child");
                    CLIPBOARD.data.setActive();
                } else if (CLIPBOARD.mode === "copy") {
                    node.addChildren(CLIPBOARD.data).setActive();
                }
                break;
            default:
                //alert("Unhandled command: " + data.cmd);
                return;
        }
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

            switch (ui.cmd) {
                case "addNodoHermano":
                    formularioAgrupamiento(OPCION_NUEVO, that, ui);
                    break;
                case "addNodoHijo":
                    formularioPropiedad(OPCION_NUEVO, that, ui);
                    break;
                case "addNodoSubHijo":
                    formularioPropiedad(OPCION_NUEVO, that, ui);
                    break;
                default:
                    setTimeout(function () {
                        $(that).trigger("nodeCommand", { cmd: ui.cmd });
                    }, 100);
                    break;
            }
        }
    });

    $(".fancytree-container").toggleClass("fancytree-connectors", true);
}

function cargarMenuTreeLectura(id) {
    var data = $("#hfJsonTree").val();
    TREE_DATA = JSON.parse(data);

    var CLIPBOARD = null;

    $("#treeFT").fancytree({
        checkbox: false,
        titlesTabbable: true,     // Add all node titles to TAB chain
        quicksearch: true,        // Jump to nodes when pressing first character
        source: TREE_DATA,

        extensions: ["table", "gridnav"],

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

            if (node.isFolder()) { //cuando es agrupamiento solo mostrar orientacion
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

            $tdList.eq(0).text(node.getIndexHier()); //orden
            //1 es propio del plugin

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
    });

    $("#treeFT").off('contextmenu');

    $(".fancytree-container").toggleClass("fancytree-connectors", true);
}

var lJsonFinal = [];

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
    }
    reg.data = data;
    reg.children = [];

    return reg;
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

///////////////////////////
/// Agrupamiento
///////////////////////////

function formularioAgrupamiento(opcion, nodo, ui) {
    $("#popupAgrupacion .popup-title span").html("Nueva Agrupación");
    $("#frmAgrpNomb").val("");
    $("#frmAgrpOrientacion").val("V");

    $("#btnAgrpGuardar").unbind();
    $('#btnAgrpGuardar').click(function () {
        nuevoAgrupamiento(nodo, ui);
    });
    $("#btnAgrpCancelar").unbind();
    $('#btnAgrpCancelar').click(function () {
        cerrarPopupAgrupacion();
    });

    cargarTableFtNota('', TIPO_ITEM_AGRUP);

    setTimeout(function () {
        $('#popupAgrupacion').bPopup({
            easing: 'easeOutBack',
            speed: 450,
            transition: 'slideDown'
        });
    }, 50);
}

function nuevoAgrupamiento(nodo, ui) {
    var objeto = {};
    var nombre = $("#frmAgrpNomb").val();
    var orientacion = $("#frmAgrpOrientacion").val();

    var objListaNota = getObjListaNotaFromTable();
    var listaNotacodi = objListaNota.listaNotacodi;
    var listaNotanum = objListaNota.listaNotanum;

    if (nombre != null && nombre.trim() != "") {
        objeto.title = nombre.trim();
        objeto.folder = true;
        objeto.data = {
            Ftitnombre: nombre,
            Ftitcodi: 0,
            Ftitorientacion: orientacion,
            ListaNotacodi: listaNotacodi,
            ListaNotanum: listaNotanum
        }
        setTimeout(function () {
            $(nodo).trigger("nodeCommand", { cmd: ui.cmd, objeto: objeto });
        }, 100);
        cerrarPopupAgrupacion();
    } else {
        alert("Debe ingresar nombre de Agrupamiento.");
    }
}

function cerrarPopupAgrupacion() {
    $('#popupAgrupacion').bPopup().close();
}

function editarNodeAgrupamiento(node) {
    $("#popupAgrupacion .popup-title span").html("Edición Agrupación");
    $("#frmAgrpNomb").val(node.data.Ftitnombre);
    $("#frmAgrpOrientacion").val(node.data.Ftitorientacion);

    cargarTableFtNota(node.data.ListaNotacodi, TIPO_ITEM_AGRUP);

    $("#btnAgrpGuardar").unbind();
    $('#btnAgrpGuardar').click(function () {
        editarAgrupamiento(node);
    });
    $("#btnAgrpCancelar").unbind();
    $('#btnAgrpCancelar').click(function () {
        cerrarPopupAgrupacion();
    });

    setTimeout(function () {
        $('#popupAgrupacion').bPopup({
            easing: 'easeOutBack',
            speed: 450,
            transition: 'slideDown'
        });
    }, 50);
}

function editarAgrupamiento(node) {
    var nombre = $("#frmAgrpNomb").val();
    var orientacion = $("#frmAgrpOrientacion").val();
    var objListaNota = getObjListaNotaFromTable();
    var listaNotacodi = objListaNota.listaNotacodi;
    var listaNotanum = objListaNota.listaNotanum;

    if (nombre != null && nombre.trim() != "") {
        node.data.Ftitnombre = nombre;
        node.title = nombre;
        node.data.Ftitorientacion = orientacion;
        node.data.ListaNotacodi = listaNotacodi;
        node.data.ListaNotanum = listaNotanum;

        $("#treeFT").fancytree("getRootNode").render(true, true); //forzar update de Tree

        cerrarPopupAgrupacion();
    } else {
        alert("Debe ingresar nombre de Agrupamiento.");
    }
}

///////////////////////////
/// Propiedad
///////////////////////////

function formularioPropiedad(opcion, nodo, ui) {
    $('#container_propiedad').unbind();
    $("#tab_vistaItemProp").show();

    $("#div_tabla_eq_propiedad").hide();
    $("#div_tabla_pr_concepto").hide();

    $("#hfCodigoPropiedad").val("");
    $("#hfDescPropiedad").val("");
    $("#hfDescFT").val("");
    $("#hfItemUnidad").val("");
    $("#hfItemTipo").val("");
    $("#descripcionPropiedad").val("");
    $("#frmPropNomb").val("");
    $("#nombFichaTec").val("");

    $("#btnPropGuardar").unbind();
    $('#btnPropGuardar').click(function () {
        nuevoPropiedad(nodo, ui);
    });
    $("#btnPropCancelar").unbind();
    $('#btnPropCancelar').click(function () {
        cerrarPopupPropiedad();
    });

    var famcodi = $("#cbFamiliaEquipo").val() || 0;
    var catecodi = $("#cbCategoriaGrupo").val() || 0;
    $("#cbPropFamiliaEquipo").val(famcodi);
    $("#cbPropCategoriaGrupo").val(catecodi);

    $("#cbPropFamiliaEquipo").removeAttr('disabled');
    $("#cbPropCategoriaGrupo").removeAttr('disabled');

    $("#cbPropFamiliaEquipo").unbind();
    $('#cbPropFamiliaEquipo').change(function () {
        var famcodi = $("#cbPropFamiliaEquipo").val() || 0;
        cargarTablePropiedad(famcodi);
    });
    $("#cbPropCategoriaGrupo").unbind();
    $('#cbPropCategoriaGrupo').change(function () {
        var catecodi = $("#cbPropCategoriaGrupo").val() || 0;
        cargarTableConcepto(catecodi);
    });

    var origen = parseInt($('input[name="origen"]:checked').val());

    $("#div_tabla_ft_propiedad").hide();
    $("#tr_fila_origen_dato").hide();
    $("#origenProp1").removeAttr('disabled');
    $("#origenProp2").removeAttr('disabled');
    $("#origenProp3").removeAttr('disabled');
    $("#origenProp1").hide();
    $("#origenProp2").hide();
    $("#origenProp3").hide();
    switch (origen) {
        case ORIGEN_TIPO_EQUIPO:
            $("#origenProp1").prop("checked", true);
            $("#origenProp1").removeAttr('disabled');
            $("#origenProp3").removeAttr('disabled');
            $("#origenProp1").show();
            $("#origenProp2").show();
            $("#origenProp3").show();

            $("#tr_fila_origen_dato").show();
            $("#cbPropCategoriaGrupo").hide();
            $("#cbPropFamiliaEquipo").show();

            if (famcodi == 5) {
                $("#cbPropCategoriaGrupo").val(4);
            }

            $(".tdNombreFT").show();
            //$(".tdNombre").hide();
            cargarTablePropiedad(famcodi);

            break;
        case ORIGEN_CATEGORIA_GRUPO:
            $("#origenProp2").prop("checked", true);
            $("#origenProp2").removeAttr('disabled');
            $("#origenProp3").removeAttr('disabled');
            $("#origenProp2").show();
            $("#origenProp3").show();
            $("#origenProp1").show();

            $("#tr_fila_origen_dato").show();
            $("#cbPropFamiliaEquipo").hide();
            $("#cbPropCategoriaGrupo").show();
            $(".tdNombreFT").show();
            //$(".tdNombre").hide();
            cargarTableConcepto(catecodi);
            break;
        case ORIGEN_FICHA_TECNICA:
            $(".tdNombreFT").hide();
            //$(".tdNombre").show();
            break;
    }
    cargarTableFtPropiedad();

    cargarTableFtNota('', TIPO_ITEM_PROP);

    $('input[type=radio][name=origen_prop]').change(function () {
        $("#tr_fila_origen_dato").hide();
        $("#div_tabla_eq_propiedad").hide();
        $("#div_tabla_pr_concepto").hide();
        $("#div_tabla_ft_propiedad").hide();

        $("#hfCodigoPropiedad").val('');
        $("#descripcionPropiedad").val('');
        $("#hfDescPropiedad").val('');
        $("#hfDescFT").val('');
        $("#hfItemUnidad").val('');
        $("#hfItemTipo").val('');
        $("#frmPropNomb").val('');
        $("#nombFichaTec").val('');

        var valorRadio = parseInt(this.value) || 0;

        switch (valorRadio) {
            case ORIGEN_TIPO_EQUIPO:
                $("#tr_fila_origen_dato").show();
                $("#div_tabla_eq_propiedad").show();
                $("#cbPropCategoriaGrupo").hide();
                $("#cbPropFamiliaEquipo").show();
                $(".tdNombreFT").show();
                //$(".tdNombre").hide();

                /*if (famcodi == 5) {
                    $("#cbPropCategoriaGrupo").show();
                    cargarTableConcepto(4);
                }
                */
                var famcodi = $("#cbFamiliaEquipo").val() || 0;
                cargarTablePropiedad(famcodi);

                break;
            case ORIGEN_CATEGORIA_GRUPO:
                $("#tr_fila_origen_dato").show();
                $("#div_tabla_pr_concepto").show();
                $("#cbPropFamiliaEquipo").hide();
                $("#cbPropCategoriaGrupo").show();
                $(".tdNombreFT").show();
                //$(".tdNombre").hide();

                var catecodi = $("#cbPropCategoriaGrupo").val() || 0;
                cargarTableConcepto(catecodi);
                break;
            case ORIGEN_FICHA_TECNICA:
                $("#div_tabla_ft_propiedad").show();
                $(".tdNombreFT").hide();
                //$(".tdNombre").show();
                break;
        }
    });
}

function editarNodePropiedad(node) {
    $('#tab-container_propiedad').unbind();
    $("#div_tabla_eq_propiedad").hide();
    $("#div_tabla_pr_concepto").hide();

    $("#hfCodigoPropiedad").val("");
    $("#hfDescPropiedad").val("");
    $("#hfDescFT").val("");
    $("#hfItemUnidad").val("");
    $("#hfItemTipo").val("");
    $("#descripcionPropiedad").val("");
    $("#frmPropNomb").val("");

    $("#btnPropGuardar").unbind();
    $('#btnPropGuardar').click(function () {
        editarPropiedad(node);
    });
    $("#btnPropCancelar").unbind();
    $('#btnPropCancelar').click(function () {
        cerrarPopupPropiedad();
    });

    var famcodi = $("#cbFamiliaEquipo").val() || 0;
    var catecodi = $("#cbCategoriaGrupo").val() || 0;
    $("#cbPropFamiliaEquipo").val(famcodi);
    $("#cbPropCategoriaGrupo").val(catecodi);

    var origen = (node != undefined && node != null) ? node.data.Origen : ORIGEN_TIPO_EQUIPO;

    $("#div_tabla_ft_propiedad").hide();
    $("#tr_fila_origen_dato").hide();
    $("#origenProp1").prop('disabled', 'disabled');
    $("#origenProp2").prop('disabled', 'disabled');
    $("#origenProp3").prop('disabled', 'disabled');
    $("#origenProp1").hide();
    $("#origenProp2").hide();
    $("#origenProp3").hide();


    $("#cbPropFamiliaEquipo").prop('disabled', 'disabled');
    $("#cbPropCategoriaGrupo").prop('disabled', 'disabled');
    switch (origen) {
        case ORIGEN_TIPO_EQUIPO:
            $("#origenProp1").prop("checked", true);
            $("#origenProp1").show();
            $("#origenProp2").show();
            $("#origenProp3").show();

            $("#tr_fila_origen_dato").show();
            $("#cbPropCategoriaGrupo").hide();
            $("#cbPropFamiliaEquipo").show();

            $(".tdNombreFT").show();
            //$(".tdNombre").hide();
            break;
        case ORIGEN_CATEGORIA_GRUPO:
            $("#origenProp2").prop("checked", true);
            $("#origenProp1").show();
            $("#origenProp2").show();
            $("#origenProp3").show();

            $("#tr_fila_origen_dato").show();
            $("#cbPropFamiliaEquipo").hide();
            $("#cbPropCategoriaGrupo").show();

            $(".tdNombreFT").show();
            //$(".tdNombre").hide();
            break;
        case ORIGEN_FICHA_TECNICA:
            $("#origenProp3").prop("checked", true);
            $("#origenProp1").show();
            $("#origenProp2").show();
            $("#origenProp3").show();

            $(".tdNombreFT").hide();
            //$(".tdNombre").show();
            break;
    }

    node.data.NombreFichaTec = _getNombreFichaTecnica(origen, node.data.Propcodi, node.data.Concepcodi);

    $("#descripcionPropiedad").val(node.data.OrigenTipo + " - " + node.data.OrigenTipoDesc);
    $("#frmPropNomb").val((node.title));
    $("#nombFichaTec").val((node.data.NombreFichaTec));

    cargarTableFtNota(node.data.ListaNotacodi, TIPO_ITEM_PROP);

    abrirPopupPropiedad(OPCION_EDITAR);
}

function nuevoPropiedad(nodo, ui) {
    var objeto = {};
    var msj = '';
    var nombre = $("#frmPropNomb").val();
    //var nombre = $("#nombFichaTec").val();
    var origen = parseInt($('input[name="origen_prop"]:checked').val());
    //if (origen == ORIGEN_FICHA_TECNICA) {
    //    nombre = $("#frmPropNomb").val();
    //}

    var Famcodi = parseInt($("#cbPropFamiliaEquipo").val()) || 0;
    var Catecodi = parseInt($("#cbPropCategoriaGrupo").val()) || 0;
    var OrigenDesc = "";
    var codigoProp = $("#hfCodigoPropiedad").val();
    var descProp = $("#hfDescPropiedad").val();
    var descFT = $("#hfDescFT").val();
    var unidad = $("#hfItemUnidad").val();
    var itemTipo = $("#hfItemTipo").val();
    var Propcodi = null;
    var Concepcodi = null;
    var Ftpropcodi = null;

    var objListaNota = getObjListaNotaFromTable();
    var listaNotacodi = objListaNota.listaNotacodi;
    var listaNotanum = objListaNota.listaNotanum;

    switch (origen) {
        case ORIGEN_TIPO_EQUIPO:
            if (codigoProp <= 0) {
                msj += "Debe seleccionar una Propiedad." + "\n";
            } else {
                Propcodi = codigoProp;
            }
            if (Famcodi < 0) {
                msj += "Debe seleccionar un Tipo de Equipo." + "\n";
            }
            OrigenDesc = TIPO_PROPIEDAD_DESC;

            break;
        case ORIGEN_CATEGORIA_GRUPO:
            if (codigoProp <= 0) {
                msj += "Debe seleccionar un Concepto." + "\n";
            } else {
                Concepcodi = codigoProp;
            }
            if (Catecodi < 0) {
                msj += "Debe seleccionar un Categoria de Grupo." + "\n";
            }
            OrigenDesc = TIPO_CONCEPTO_DESC;

            break;
        case ORIGEN_FICHA_TECNICA:
            if (codigoProp <= 0) {
                msj += "Debe seleccionar una Propiedad." + "\n";
            } else {
                Ftpropcodi = codigoProp;
            }
            OrigenDesc = TIPO_FICHA_TECNICA_PROP_DESC;

            break;
    }

    if (nombre == null || nombre.trim() == "") {
        msj += "Debe ingresar nombre de propiedad" + "\n";
    }
    if (nombre.length > 300) {
        msj += "El nombre de la propiedad no debe exceder los 300 caracteres" + "\n";
    }

    if (msj == "") {
        objeto.title = nombre;
        objeto.folder = false;
        objeto.data = {
            Ftitcodi: 0,
            Ftitorientacion: "",
            Origen: origen,
            OrigenDesc: OrigenDesc,
            OrigenTipo: codigoProp,
            OrigenTipoDesc: descProp,
            NombreFichaTec: descFT,
            Concepcodi: Concepcodi,
            Ftpropcodi: Ftpropcodi,
            Propcodi: Propcodi,
            ItemUnidad: unidad,
            ItemTipo: itemTipo,
            ListaNotacodi: listaNotacodi,
            ListaNotanum: listaNotanum,
            Ftitorden: "",
            Orden: ""
        }
        setTimeout(function () {
            $(nodo).trigger("nodeCommand", { cmd: ui.cmd, objeto: objeto });
        }, 100);
        cerrarPopupPropiedad();
    } else {
        alert(msj);
    }
}

function editarPropiedad(node) {
    var nombre = $("#frmPropNomb").val();
    var objListaNota = getObjListaNotaFromTable();
    var listaNotacodi = objListaNota.listaNotacodi;
    var listaNotanum = objListaNota.listaNotanum;

    if (nombre != null && nombre.trim() != "") {
        node.data.Ftitnombre = nombre;
        node.title = nombre;
        node.data.ListaNotacodi = listaNotacodi;
        node.data.ListaNotanum = listaNotanum;

        $("#treeFT").fancytree("getRootNode").render(true, true); //forzar update de Tree

        cerrarPopupPropiedad();
    } else {
        alert("Debe ingresar nombre de Propiedad.");
    }
}

function cerrarPopupPropiedad() {
    $('#popupPropiedad').bPopup().close();
    $('#popupPropiedad').bPopup().close();
}

function abrirPopupPropiedad(tipo) {
    setTimeout(function () {
        $('#popupPropiedad').bPopup({
            easing: 'easeOutBack',
            speed: 50,
            transition: 'slideDown'
        });

        $('#tab-container_propiedad').easytabs({
            animate: false
        });

        if (tipo == OPCION_NUEVO) {
            $("#tab_vistaItemProp").show();
            $("#tab-container_propiedad").easytabs({ updateHash: true, defaultTab: "li:eq(1)" });
            $('#tab-container_propiedad').easytabs('select', '#vistaItemProp');
        } else {
            $("#tab_vistaItemProp").hide();
            $("#tab-container_propiedad").easytabs({ updateHash: true, defaultTab: "li:eq(2)" });
            $('#tab-container_propiedad').easytabs('select', '#vistaItemNota');
        }
    }, 50);
}

function cargarTablePropiedad(famcodi) {
    LISTA_EQ_PROPIEDAD = null;

    if (famcodi != -2) {
        $.ajax({
            type: "POST",
            url: controlador + "ListaEqPropiedad",
            data: {
                famcodi: famcodi
            },
            global: false,
            success: function (result) {
                if (result.Resultado == '-1') {
                    alert('Ha ocurrido un error:' + result.Mensaje);
                } else {
                    var htmlTable = "<table id='tabla_eq_propiedad' style='width: 100%' class='pretty tabla-icono'><thead><tr><th style='width:12px'>Código</th><th style='width:24px'>Nombre</th><th style='width:26px'>Nombre Ficha Técnica</th><th style='width:18px'>Abreviatura</th><th style='width:6px'>Unidad</th><th style='width:6px'>Tipo</th></tr></thead>";

                    var tbody = '';

                    LISTA_EQ_PROPIEDAD = result.ListaPropiedad;
                    for (var i = 0; i < result.ListaPropiedad.length; i++) {
                        var item = result.ListaPropiedad[i];
                        item.NombreFamilia = convertirStringToHtml(item.NombreFamilia);
                        item.Propabrev = convertirStringToHtml(item.Propabrev);
                        item.Propnomb = convertirStringToHtml(item.Propnomb);
                        item.Propnombficha = convertirStringToHtml(item.Propnombficha);
                        item.Propunidad = convertirStringToHtml(item.Propunidad);
                        item.Propdefinicion = convertirStringToHtml(item.Propdefinicion);
                        item.Proptipo = convertirStringToHtml(item.Proptipo);

                        var fila = "<tr onclick=\"seleccionarEqPropiedad(" + item.Propcodi + ");\" style='cursor:pointer'>";
                        fila += "<td style='text-align: center; padding-top: 7px; padding-bottom: 7px;'>" + item.Propcodi + "</td>";
                        /*fila += "<td style='text-align: center'>" + item.NombreFamilia + "</td>";*/
                        fila += "<td style='text-align: left'>" + item.Propnomb + "</td>";
                        fila += "<td style='text-align: left'>" + item.Propnombficha + "</td>";
                        fila += "<td style='text-align: left'>" + item.Propabrev + "</td>";
                        /*fila += "<td style='text-align: left'>" + item.Propdefinicion + "</td>";*/
                        fila += "<td style='text-align: center'>" + item.Propunidad + "</td>";
                        fila += "<td style='text-align: center'>" + item.Proptipo + "</td>";
                        fila += "</tr>";
                        tbody += fila;
                    }
                    htmlTable += tbody + "<tbody></tbody></table>";
                    $('#div_tabla_eq_propiedad').html(htmlTable);
                    $("#div_tabla_eq_propiedad").show();

                    setTimeout(function () {
                        $('#tabla_eq_propiedad').dataTable({
                            "scrollY": 500,
                            "sPaginationType": "full_numbers",
                            "destroy": "true",
                            "bInfo": true,
                            //"bLengthChange": false,
                            //"sDom": 'fpt',
                            "ordering": true,
                            "order": [[1, "asc"]],
                            "iDisplayLength": 15
                        });
                    }, 350);

                    abrirPopupPropiedad(OPCION_NUEVO);
                }
            },
            error: function (req, status, error) {
                alert("Ha ocurrido un error.");
            }
        });
    } else {
        $('#div_tabla_eq_propiedad').html('');
        $("#div_tabla_eq_propiedad").show();
    }
}

function seleccionarEqPropiedad(Propcodi) {
    var obj = getEqPropiedadFromTemporal(Propcodi);
    var Propcodi = obj != null ? obj.Propcodi : 0;
    var Propnomb = obj != null ? obj.Propnomb : "";
    var Propunidad = obj != null ? obj.Propunidad : "";
    var Proptipo = obj != null ? obj.Proptipo : "";
    var PropnombFT = obj != null ? obj.Propnombficha : "";

    $("#hfCodigoPropiedad").val(Propcodi);
    $("#descripcionPropiedad").val(Propcodi + " - " + convertirStringToHtml(Propnomb));
    $("#hfDescPropiedad").val((Propnomb));
    $("#hfDescFT").val((PropnombFT));
    $("#hfItemUnidad").val((Propunidad));
    $("#hfItemTipo").val(Proptipo);

    $("#frmPropNomb").val((Propnomb));
    $("#nombFichaTec").val((PropnombFT));
}

function getEqPropiedadFromTemporal(propcodi) {
    var obj = null;

    if (LISTA_EQ_PROPIEDAD != null) {
        for (var i = 0; i < LISTA_EQ_PROPIEDAD.length; i++) {
            var item = LISTA_EQ_PROPIEDAD[i];
            if (item.Propcodi == propcodi) {
                return item;
            }
        }
    }

    return obj;
}

function cargarTableConcepto(catecodi) {
    LISTA_PR_CONCEPTO = null;

    if (catecodi != -2) {
        $.ajax({
            type: "POST",
            url: controlador + "ListaPrConcepto",
            data: {
                catecodi: catecodi
            },
            global: false,
            success: function (result) {
                if (result.Resultado == '-1') {
                    alert('Ha ocurrido un error:' + result.Mensaje);
                } else {
                    var htmlTable = "<table id='tabla_pr_concepto' style='width: 100%' class='pretty tabla-icono'><thead><tr><th width='12%'>Código</th><th width='20%'>Nombre</th><th width='20%'>Nombre Ficha Técnica</th><th width='18%'>Abreviatura</th><th width='6%'>Unidad</th><th width='6%'>Tipo</th></tr></thead>";

                    var tbody = '';
                    LISTA_PR_CONCEPTO = result.ListaConcepto;
                    for (var i = 0; i < result.ListaConcepto.length; i++) {
                        var item = result.ListaConcepto[i];
                        item.Catenomb = convertirStringToHtml(item.Catenomb);
                        item.Concepnombficha = convertirStringToHtml(item.Concepnombficha);
                        item.Concepabrev = convertirStringToHtml(item.Concepabrev);
                        item.Concepdesc = convertirStringToHtml(item.Concepdesc);
                        item.Concepunid = convertirStringToHtml(item.Concepunid);
                        item.Conceptipo = convertirStringToHtml(item.Conceptipo);


                        var fila = "<tr onclick=\"seleccionarPrConcepto(" + item.Concepcodi + ");\" style='cursor:pointer'>";
                        fila += "<td style='text-align: center; padding-top: 7px; padding-bottom: 7px;'>" + item.Concepcodi + "</td>";
                        /*fila += "<td style='text-align: center'>" + item.Catenomb + "</td>";*/
                        fila += "<td style='text-align: left'>" + item.Concepdesc + "</td>";
                        fila += "<td style='text-align: left'>" + item.Concepnombficha + "</td>";
                        fila += "<td style='text-align: left'>" + item.Concepabrev + "</td>";
                        fila += "<td style='text-align: center'>" + item.Concepunid + "</td>";
                        fila += "<td style='text-align: center'>" + item.Conceptipo + "</td>";
                        fila + "</tr>";
                        tbody += fila;
                    }

                    htmlTable += tbody + "<tbody></tbody></table>";
                    $('#div_tabla_pr_concepto').html(htmlTable);
                    $("#div_tabla_pr_concepto").show();

                    $('#tabla_pr_concepto').dataTable({
                        "sPaginationType": "full_numbers",
                        "destroy": "true",
                        "bInfo": true,
                        //"bLengthChange": false,
                        //"sDom": 'fpt',
                        "ordering": true,
                        "order": [[1, "asc"]],
                        "iDisplayLength": 15
                    });

                    setTimeout(function () {
                        $('#tabla_pr_concepto').dataTable({
                            "scrollY": 500,
                            "sPaginationType": "full_numbers",
                            "destroy": "true",
                            "bInfo": true,
                            //"bLengthChange": false,
                            //"sDom": 'fpt',
                            "ordering": true,
                            "order": [[1, "asc"]],
                            "iDisplayLength": 15
                        });
                    }, 350);

                    abrirPopupPropiedad(OPCION_NUEVO);
                }
            },
            error: function (req, status, error) {
                alert("Ha ocurrido un error.");
            }
        });
    } else {
        $('#div_tabla_pr_concepto').html('');
        $("#div_tabla_pr_concepto").show();
    }
}

function seleccionarPrConcepto(Concepcodi) {
    var obj = getPrConceptoFromTemporal(Concepcodi);
    var Concepcodi = obj != null ? obj.Concepcodi : 0;
    var Concepdesc = obj != null ? obj.Concepdesc : "";
    var Concepunid = obj != null ? obj.Concepunid : "";
    var Conceptipo = obj != null ? obj.Conceptipo : "";
    var concepnombFT = obj != null ? obj.Concepnombficha : "";

    $("#hfCodigoPropiedad").val(Concepcodi);
    $("#hfDescPropiedad").val(convertirStringToHtml(Concepdesc));
    $("#hfDescFT").val((concepnombFT));
    $("#descripcionPropiedad").val(Concepcodi + " - " + convertirStringToHtml(Concepdesc));
    $("#hfItemUnidad").val(convertirStringToHtml(Concepunid));
    $("#hfItemTipo").val(convertirStringToHtml(Conceptipo));

    $("#frmPropNomb").val(convertirStringToHtml(Concepdesc));
    $("#nombFichaTec").val((concepnombFT));
}

function getPrConceptoFromTemporal(Concepcodi) {
    var obj = null;

    if (LISTA_PR_CONCEPTO != null) {
        for (var i = 0; i < LISTA_PR_CONCEPTO.length; i++) {
            var item = LISTA_PR_CONCEPTO[i];
            if (item.Concepcodi == Concepcodi) {
                return item;
            }
        }
    }

    return obj;
}

function convertirStringToHtml(str) {
    if (str != undefined && str != null) {
        //return str.replace(/&/g, "&amp;").replace(/>/g, "&gt;").replace(/</g, "&lt;").replace(/"/g, "&quot;");
        return str.replace(/>/g, "&gt;").replace(/</g, "&lt;").replace("'", "&#39;");//.replace(/"/g, "&quot;");
    }

    return "";
}

function seleccionarFtPropiedad(Ftpropcodi, Ftpropnomb, Ftpropdefinicion, Ftpropunidad, Ftproptipo) {
    $("#hfCodigoPropiedad").val(Ftpropcodi);
    $("#descripcionPropiedad").val(Ftpropcodi + " - " + convertirStringToHtml(Ftpropnomb));
    $("#hfDescPropiedad").val((Ftpropnomb));
    $("#hfItemUnidad").val((Ftpropunidad));
    $("#hfItemTipo").val(Ftproptipo);

    $("#frmPropNomb").val((Ftpropnomb));
}

function cargarTableFtPropiedad() {
    $('#tabla_ft_propiedad').dataTable({
        "sPaginationType": "full_numbers",
        "destroy": "true",
        "bInfo": false,
        "bLengthChange": false,
        "sDom": 'fpt',
        "ordering": true,
        "order": [[2, "asc"]],
        "iDisplayLength": 15
    });
}

function cargarTableFtNota(listacodi, id_tipo_item) {
    $("#div_tabla_ft_item_nota").hide();
    $("#div_tabla_ft_item_nota").html('');

    $("#div_tabla_ft_agrp_nota").hide();
    $("#div_tabla_ft_agrp_nota").html('');

    var notaData = {};
    var arrayCodiCheck = [];

    var data = $("#hfJsonNota").val();
    notaData = JSON.parse(data);

    if (listacodi != '' && listacodi != null) {
        arrayCodiCheck = listacodi.split(',');
    }

    if (notaData != undefined && notaData != null && notaData.length > 0) {
        var htmlTable = '';
        if (id_tipo_item == TIPO_ITEM_AGRUP) {
            htmlTable += '<div class="tbform-label">Notas: </div>';
        }

        htmlTable += "<table id='tabla_ft_item_nota' border='0' class='pretty tabla-icono' cellspacing='0'><thead><tr><th style='width: 70px'>Activo</th><th style='width: 70px'>Número</th><th style='width: 240px'>Descripción</th></tr></thead><tbody>";

        var tbody = '';
        for (var i = 0; i < notaData.length; i++) {
            var item = notaData[i];
            item.Ftnotdesc = convertirStringToHtml(item.Ftnotdesc);

            var checkedNota = esCheckedNotaFromLista(item.Ftnotacodi, arrayCodiCheck);

            var fila = "<tr>";
            fila += "<td>";
            fila += "<input type='checkbox' id='id_ni_codigo" + item.Ftnotacodi + "' name='ck_item_nota_codigo' " + checkedNota + " />";
            fila += "<input type='hidden' id='id_ni_num" + item.Ftnotacodi + "' name='ck_item_nota_num' value='" + item.Ftnotanum + "' />";
            fila += "</td>";
            fila += "<td style='text-align: center; padding-top: 7px; padding-bottom: 7px;'>" + item.Ftnotanum + "</td>";
            fila += "<td class='desc' style='text-align: center'>" + item.Ftnotdesc + "</td>";
            fila + "</tr>";
            tbody += fila;
        }

        htmlTable += tbody + "<tbody></tbody></table>";
        $('#div_tabla_ft_' + id_tipo_item + '_nota').html(htmlTable);

        $('#tabla_ft_item_nota').dataTable({
            "sPaginationType": "full_numbers",
            "destroy": "true",
            "bInfo": false,
            "bLengthChange": false,
            "sDom": 't',
            "ordering": false,
            "order": [[2, "asc"]],
            "iDisplayLength": -1
        });

        $("#div_tabla_ft_" + id_tipo_item + "_nota").show();
    }
}

function esCheckedNotaFromLista(notacodi, arrayCodi) {
    for (var i = 0; i < arrayCodi.length; i++) {
        if (arrayCodi[i] == notacodi) { return 'checked'; }
    }

    return '';
}

function getObjListaNotaFromTable() {
    var listaNotacodi = '';
    var listaNotanum = '';

    var selectedNotacodi = [];
    var selectedNotanum = [];
    $('#tabla_ft_item_nota input[name=ck_item_nota_codigo]').each(function () {
        if ($(this).is(":checked")) {
            var valorid = $(this).attr('id');
            if (valorid != null && valorid.length > 12) {
                valorid = valorid.substring(12, valorid.length);
            }
            selectedNotacodi.push(valorid);
        }
    });

    for (var num = 0; num < selectedNotacodi.length; num++) {
        var datanum = $("#id_ni_num" + selectedNotacodi[num]).val();
        selectedNotanum.push("(" + datanum + ")");
    }

    var listaNotacodi = selectedNotacodi.toString();
    var listaNotanum = selectedNotanum.join(" ");

    var obj = {};
    obj.listaNotacodi = listaNotacodi;
    obj.listaNotanum = listaNotanum;

    return obj;
}

///////////////////////////
/// Notas
///////////////////////////
function inicializarNota(opcion) {
    configurarFormularioNotaNuevo();

    var arrayVisible = [];

    switch (opcion) {
        case OPCION_VER:
            $("#vistaNota .content-botonera").hide();
            arrayVisible = [{ "targets": [0], "visible": false, }];
            break;
        case OPCION_EDITAR:
            $("#vistaNota .content-botonera").show();
            break;
        case OPCION_NUEVO:
            $("#vistaNota .content-botonera").hide()
            break;
    }

    $("#btnNotaNuevo").unbind();
    $('#btnNotaNuevo').click(function () {
        $("#btnNotaNuevo").hide();
        configurarFormularioNotaNuevo();
    });

    $("#btnNotaGuardar").unbind();
    $('#btnNotaGuardar').click(function () {
        guardarNota();
    });

    $("#btnNotaConsultar").unbind();
    $('#btnNotaConsultar').click(function () {
        listarNotaXFichaTecnica();
    });

    listarNotaXFichaTecnica(arrayVisible);
}

function listarNotaXFichaTecnica(arrayVisible) {
    $("#btnNotaNuevo").show();
    $("#formularioNota").hide();

    var idFT = $("#hfCodigo").val();
    $.ajax({
        type: 'POST',
        url: controlador + 'FichaTecnicaNotaLista',
        data: {
            idFT: idFT
        },
        success: function (result) {
            $('#listadoNota').html(result);

            $('#tablaListadoNota').dataTable({
                "sPaginationType": "full_numbers",
                "destroy": "true",
                "bInfo": false,
                "bLengthChange": false,
                "sDom": 't',
                "ordering": false,
                "order": [[2, "asc"]],
                "iDisplayLength": -1,
                "columnDefs": arrayVisible
            });

            $("#hfJsonNota").val($("#hfJsonNotaLista").val());
        },
        error: function (err) {
            alert('Ha ocurrido un error.');
        }
    });
}

function configurarFormularioNotaNuevo() {
    $("#formularioNota").show();
    $("#hfCodigoNota").val("0");
    $("#frmNotaNum").val("");
    $("#frmNotaDesc").val("");
    $("#btnNotaGuardar").val("Agregar");
    $("#formularioNota .popup-title span").html("Nueva Nota");
}

function editarNota(id, num, desc) {
    $("#formularioNota").show();
    $("#hfCodigoNota").val(id);
    $("#frmNotaNum").val(num);
    $("#frmNotaDesc").val(desc);
    $("#btnNotaGuardar").val("Actualizar");
    $("#formularioNota .popup-title span").html("Modificación de Nota");
}

function guardarNota() {
    var idFT = $("#hfCodigo").val();
    var entity = getObjetoNotaJson();
    if (confirm('¿Desea guardar la nota?')) {
        var msj = validarNota(entity);

        if (msj == "") {
            $.ajax({
                type: 'POST',
                dataType: 'json',
                traditional: true,
                url: controlador + 'FichaTecnicaNotaGuardar',
                data: {
                    idNota: entity.idNota,
                    idFT: idFT,
                    numero: entity.numero,
                    desc: entity.desc
                },
                cache: false,
                success: function (result) {
                    if (result.Resultado == '-1') {
                        alert('Ha ocurrido un error:' + result.Mensaje);
                    } else {
                        alert("Se guardó correctamente la nota");
                        listarNotaXFichaTecnica();
                    }
                },
                error: function (err) {
                    alert('Ha ocurrido un error.');
                }
            });
        } else {
            alert(msj);
        }
    }
}

function eliminarNota(id) {
    if (confirm('¿Desea eliminar la nota?')) {
        $.ajax({
            type: 'POST',
            dataType: 'json',
            traditional: true,
            url: controlador + 'FichaTecnicaNotaEliminar',
            data: {
                idNota: id
            },
            cache: false,
            success: function (result) {
                if (result.Resultado == '-1') {
                    alert('Ha ocurrido un error:' + result.Mensaje);
                } else {
                    alert("Se eliminó correctamente la nota");
                    listarNotaXFichaTecnica();
                }
            },
            error: function (err) {
                alert('Ha ocurrido un error.');
            }
        });
    }
}

function validarNota(obj) {
    var msj = "";

    if (obj.numero <= 0) {
        msj += "Debe ingresar un número válido";
    }

    if (obj.desc == null || obj.desc == "") {
        msj += "Debe ingresar una descripción";
    } else {
        if (obj.desc.length > 500) {
            msj += "La descripción debe ser menor de 500 caracteres";
        }
    }

    return msj;
}

function getObjetoNotaJson() {
    var obj = {};
    obj.idNota = parseInt($("#hfCodigoNota").val()) || 0;
    obj.numero = parseInt($("#frmNotaNum").val()) || 0;
    obj.desc = $("#frmNotaDesc").val();

    return obj;
}

//Util
function _getNombreFichaTecnica(origen, codigoProp, codigoConc) {
    switch (origen) {
        case ORIGEN_TIPO_EQUIPO:
            var obj1 = _obtenerEqPropiedadXLista(codigoProp, LISTA_EQ_PROPIEDAD_GLOBAL);
            return obj1.Propnombficha ?? "";
            break;
        case ORIGEN_CATEGORIA_GRUPO:
            var obj2 = _obtenerPrConceptoXLista(codigoConc, LISTA_PR_CONCEPTO_GLOBAL);
            return obj2.Concepnombficha ?? "";
            break;
        case ORIGEN_FICHA_TECNICA:

            break;
    }

    return "";
}

function _obtenerEqPropiedadXLista(codigo, lista) {
    for (var i = 0; i < lista.length; i++) {
        if (lista[i].Propcodi == codigo) { return lista[i]; }
    }

    return null;
}

function _obtenerPrConceptoXLista(codigo, lista) {
    for (var i = 0; i < lista.length; i++) {
        if (lista[i].Concepcodi == codigo) { return lista[i]; }
    }

    return null;
}