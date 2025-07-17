var controlador = siteRoot + 'Combustibles/ConfiguracionGas/';

var IMG_EDITAR = '<img src="' + siteRoot + 'Content/Images/btn-edit.png" alt="" width="19" height="19" style="">';
var IMG_VER = '<img src="' + siteRoot + 'Content/Images/btn-open.png" alt="" width="19" height="19" style="">';
var IMG_COPIAR = '<img src="' + siteRoot + 'Content/Images/copiar.png" alt="" width="19" height="19" style="">';
var IMG_ELIMINAR = '<img src="' + siteRoot + 'Content/Images/btn-cancel.png" alt="" width="19" height="19" style="">';
var IMG_VERSIONES = '<img src="' + siteRoot + 'Content/Images/btn-properties.png" alt="" width="19" height="19" style="">';
var IMG_AGREGAR = '<img src="' + siteRoot + 'Content/Images/btn-add.png" alt="" width="19" height="19" style="">';

var CONFIGURACION_DEFAULT = null;

var CORRELATIVO_SECCION = 0;
var CORRELATIVO_AGRUP = 0;
var LISTA_PROPIEDAD = [];

$(function () {
    $('#tab-container').easytabs({
        animate: false
    });
    $('#tab-container').easytabs('select', '#vistaListado');

    //formulario
    $("#btnNuevo").click(function () {
        $("#div_detalle").html('');
        inicializarFormulario(null, true);
    });
    $("#btnNuevaPropiedad").click(function () {
        inicializarFormularioPropiedad(null);
    });

    ancho = $("#mainLayout").width() > 1100 ? $("#mainLayout").width() - 30 : 1100;

    $("#div_detalle").css("width", (ancho) + "px");
    $("#div_detalle").css("height", "700px");

    cargarConfiguracionDefault();
});

///////////////////////////
/// Listado Plantilla
///////////////////////////

function cargarConfiguracionDefault() {
    $.ajax({
        type: 'POST',
        url: controlador + 'ListadoReportePlantilla',
        dataType: 'json',
        cache: false,
        success: function (data) {
            if (data.Resultado != "-1") {
                LISTA_PROPIEDAD = data.ListadoPropiedad;

                $("#mensaje").hide();

                var html = dibujarTablaReporte(data.ListadoPlantilla, data.TienePermisoAdmin);
                $("#div_reporte").html(html);

                var html2 = dibujarTablaReportePropiedad(LISTA_PROPIEDAD, data.TienePermisoAdmin);
                $("#div_reporte_propiedad").html(html2);
            } else {
                alert("Ha ocurrido un error: " + data.Mensaje);
            }
        },
        error: function (err) {
            alert('Ha ocurrido un error.');
        }
    });
}

function dibujarTablaReporte(lista, esAdmin) {

    var cadena = '';
    cadena += `
    <table border="0" class="pretty tabla-icono dataTable" cellspacing="0" width="100%" id="reportePlantilla">
        <thead>
            <tr>
                <th style='width: 70px'>Acción</th>
                <th style='width: 40px'>Código</th>
                <th style='width: 140px'>Nombre</th>
                <th style='width: 70px'>Vigencia desde</th>
                <th style='width: 70px'>Estado actual</th>
                <th style='width: 70px'>Usuario modificación</th>
                <th style='width: 70px'>Fecha modificación</th>
            </tr>
        </thead>
        <tbody>
    `;

    for (key in lista) {
        var item = lista[key];
        var claseFila = item.EsVigente ? 'fila_plantilla_vigente' : '';

        var htmlEliminar = `
                    <a title="Eliminar registro" href="JavaScript:eliminarModelo(${item.Cbftcodi});">  ${IMG_ELIMINAR} </a>
            `;
        if (item.Cbftcodi == 1 || item.EsVigente)
            htmlEliminar = "";

        var htmlTdAccion = "";
        if (esAdmin) {
            htmlTdAccion = `
                    <a title="Copiar registro" href="JavaScript:copiarModelo(${item.Cbftcodi});">${IMG_COPIAR} </a>
                    <a title="Ver registro" href="JavaScript:verModelo(${item.Cbftcodi});">${IMG_VER} </a>
                    <a title="Editar registro" href="JavaScript:editarModelo(${item.Cbftcodi});">${IMG_EDITAR} </a>
                    ${htmlEliminar}
            `;
        } else {
            htmlTdAccion = `
                    <a title="Ver registro" href="JavaScript:verModelo(${item.Cbftcodi});">${IMG_VER} </a>
            `;
        }

        cadena += `
            <tr>
                <td  class='${claseFila}' style="height: 24px">
                    ${htmlTdAccion}
                </td>
                <td  class='${claseFila}' style="height: 24px">${item.Cbftcodi}</td>
                <td  class='${claseFila}' style="text-align: left; height: 24px">${item.Cbftnombre}</td>
                <td  class='${claseFila}' style="height: 24px">${item.CbftfechavigenciaDesc}</td>
                <td  class='${claseFila}' style="height: 24px">${item.EstadoActual}</td>
                <td  class='${claseFila}' style="height: 24px">${item.UltimaModificacionUsuarioDesc}</td>
                <td  class='${claseFila}' style="height: 24px">${item.UltimaModificacionFechaDesc}</td>
            </tr>
        `;
    }
    cadena += "</tbody></table>";

    cadena += `
        <div class="leyenda_alerta" style="HEIGHT: 38px;margin-top: 20px;">
            <div class="content-action">
                <div class="td_inline">

                    <table>
                        <tbody><tr>
                            <!--alerta-->
                            <td style="width: 50px;height: 20px;border: 1px solid #dddddd;" class="fila_plantilla_vigente">
                                &nbsp;
                            </td>
                            <td style="vertical-align: middle;padding-left: 5px;">Plantilla vigente al día de hoy</td>
                            
                        </tr>
                    </tbody></table>
                </div>

            </div>
        </div>
    `;

    return cadena;
}

function guardarFormulario() {
    var obj = generarJsonModelo();
    var msj = validarJsonModelo(obj);

    if (msj == '') {
        $.ajax({
            type: 'POST',
            url: controlador + "GuardarPlantilla",
            data: {
                strConf: JSON.stringify(obj)
            },
            cache: false,
            success: function (model) {
                if (model.Resultado != "-1") {
                    $("#mensaje").show();
                    mostrarMensaje("mensaje", "El registro se guardó correctamente.", $tipoMensajeExito, $modoMensajeCuadro);

                    cargarConfiguracionDefault();
                    $('#tab-container').easytabs('select', '#vistaListado');
                } else {
                    alert("Ha ocurrido un error: " + model.Mensaje);
                    //$("#mensaje").show();
                    //mostrarMensaje("mensaje", model.Mensaje, $tipoMensajeError, $modoMensajeCuadro);
                }
            },
            error: function () {
            }
        });
    } else {
        $("#mensaje").show();
        mostrarMensaje("mensaje", msj, $tipoMensajeError, $modoMensajeCuadro);
    }
}

function validarJsonModelo(obj) {
    var msj = '';

    if (obj.Recurcodi <= 0)
        msj += "No ha seleccionado embalse. " + "<br/>";
    //if (obj.Modembindyupana == "S" && )

    return msj;
}

function editarModelo(cbftcodi) {
    $("#mensaje").hide();

    $('#tab-container').easytabs('select', '#vistaDetalle');
    $("#div_detalle").html('');

    $.ajax({
        type: 'POST',
        url: controlador + 'ObtenerPlantilla',
        dataType: 'json',
        data: {
            cbftcodi: cbftcodi
        },
        cache: false,
        success: function (data) {
            if (data.Resultado != "-1") {
                inicializarFormulario(data.Plantilla, true);
            } else {
                alert("Ha ocurrido un error: " + data.Mensaje);
            }
        },
        error: function (err) {
            alert('Ha ocurrido un error.');
        }
    });
}


function verModelo(cbftcodi) {
    $("#mensaje").hide();

    $('#tab-container').easytabs('select', '#vistaDetalle');
    $("#div_detalle").html('');

    $.ajax({
        type: 'POST',
        url: controlador + 'ObtenerPlantilla',
        dataType: 'json',
        data: {
            cbftcodi: cbftcodi
        },
        cache: false,
        success: function (data) {
            if (data.Resultado != "-1") {
                inicializarFormulario(data.Plantilla, false);
            } else {
                alert("Ha ocurrido un error: " + data.Mensaje);
            }
        },
        error: function (err) {
            alert('Ha ocurrido un error.');
        }
    });
}

function eliminarModelo(cbftcodi) {
    $("#mensaje").hide();

    if (confirm("¿Desea eliminar la plantilla?"))
        $.ajax({
            type: 'POST',
            url: controlador + 'EliminarPlantilla',
            dataType: 'json',
            data: {
                cbftcodi: cbftcodi
            },
            cache: false,
            success: function (data) {
                if (data.Resultado != "-1") {
                    alert("El registro ha sido eliminado");
                    cargarConfiguracionDefault();
                } else {
                    alert("Ha ocurrido un error: " + data.Mensaje);
                }
            },
            error: function (err) {
                alert('Ha ocurrido un error.');
            }
        });
}

function copiarModelo(cbftcodi) {
    $("#mensaje").hide();

    if (confirm("¿Desea copiar la plantilla?"))
        $.ajax({
            type: 'POST',
            url: controlador + 'CopiarPlantilla',
            dataType: 'json',
            data: {
                cbftcodi: cbftcodi
            },
            cache: false,
            success: function (data) {
                if (data.Resultado != "-1") {
                    alert("Se ha copiado la plantilla");
                    cargarConfiguracionDefault();
                } else {
                    alert("Ha ocurrido un error: " + data.Mensaje);
                }
            },
            error: function (err) {
                alert('Ha ocurrido un error.');
            }
        });
}

///////////////////////////
/// Formulario Propiedad
///////////////////////////

function dibujarTablaReportePropiedad(lista, esAdmin) {

    var cadena = '';
    cadena += `
    <table border="0" class="pretty tabla-icono dataTable" cellspacing="0" width="100%" id="reportePlantilla">
        <thead>
            <tr>
                <th style='width: 70px'>Acción</th>
                <th style='width: 40px'>Código</th>
                <th style='width: 140px'>Nombre</th>
                <th style='width: 140px'>Unidad</th>
                <th style='width: 140px'>Estado</th>
            </tr>
        </thead>
        <tbody>
    `;

    for (key in lista) {
        var item = lista[key];
        var claseFila = '';

        var htmlTdAccion = "";
        if (esAdmin) {
            htmlTdAccion = `
                    <a title="Ver registro" href="JavaScript:verPropiedad(${item.Ccombcodi});">${IMG_VER} </a>
                    <a title="Editar registro" href="JavaScript:editarPropiedad(${item.Ccombcodi});">${IMG_EDITAR} </a>
            `;
        } else {
            htmlTdAccion = `
                    <a title="Ver registro" href="JavaScript:verPropiedad(${item.Ccombcodi});">${IMG_VER} </a>
            `;
        }

        cadena += `
            <tr>
                <td  class='${claseFila}' style="height: 24px">
                    ${htmlTdAccion}
                </td>
                <td  class='${claseFila}' style="height: 24px">${item.Ccombcodi}</td>
                <td  class='${claseFila}' style="text-align: left;">${item.Ccombnombre}</td>
                <td  class='${claseFila}' style="text-align: left;">${item.Ccombunidad}</td>
                <td  class='${claseFila}' style="text-align: left;">${item.CcombestadoDesc}</td>
            </tr>
        `;
    }
    cadena += "</tbody></table>";

    return cadena;
}

function inicializarFormularioPropiedad(objEdit) {

    var htmlForm = dibujarHtmlFormularioPropiedad(objEdit);
    $("#popupFormPropiedad").html(htmlForm);

    $('#popupFormPropiedad').bPopup({
        modalClose: false,
        easing: 'easeOutBack',
        speed: 450,
        transition: 'slideDown',
        onClose: function () {
            $('#popup').empty();
        }
    });
    $(function () {
        $('#btnGrabarPropiedadForm').unbind();
        $("#btnGrabarPropiedadForm").click(function () {
            guardarFormularioPropiedad();
        });

        $('#btnCancelarPropiedadForm').unbind();
        $("#btnCancelarPropiedadForm").click(function () {
            $('#popupFormPropiedad').bPopup().close();
        });
    });

}

function dibujarHtmlFormularioPropiedad(objEdit) {
    var codigo = objEdit != null ? objEdit.Ccombcodi : 0;
    var nombre = objEdit != null ? objEdit.Ccombnombre : '';
    var nombreFicha = objEdit != null ? objEdit.Ccombnombreficha : '';
    var unidad = objEdit != null ? objEdit.Ccombunidad : '';

    //Div general
    var html = `
    <div class="popup-title">
        <span>Formulario Propiedad</span>
    </div>

    <div class="content-registro" style="width:auto">

        <div style="clear:both; height:10px;"></div>

        <div id="mensajeFormPropiedad" class="action-message" style="margin:0; margin-bottom:10px; ">Por favor complete los datos</div>

        <table cellpadding="8" style="width:auto">
            <tbody>
                <tr>
                    <td class="registro-label">Nombre:</td>
                    <td class="registro-control">
                        <input type="text" id="txtNombrePropiedad" value='${nombre}' maxlength="130" style="width: 300px;" autocomplete="off" />
                        <input type="hidden" id="idPropiedad" value='${codigo}' />
                    </td>
                </tr>
                <tr>
                    <td class="registro-label">Nombre de ficha:</td>
                    <td class="registro-control">
                        <input type="text" id="txtNombreFichaPropiedad" value='${nombreFicha}' maxlength="250" style="width: 300px;" autocomplete="off" />
                    </td>
                </tr>
                <tr>
                    <td class="registro-label">Unidad:</td>
                    <td class="registro-control">
                        <input type="text" id="txtNombreUnidad" value='${unidad}' maxlength="13" style="width: 110px;" autocomplete="off" />
                    </td>
                </tr>
            </tbody>
        </table>

        <table class="btnAcciones" style="width: 200px; float: right;">
             <tbody><tr>
                    <td>
                        <input type="button" id="btnGrabarPropiedadForm" value="Grabar">
                        <input type="button" id="btnCancelarPropiedadForm" value="Cancelar">
                    </td>
                </tr>
            </tbody>
        </table>

    </div>
`;

    return html;
}

function guardarFormularioPropiedad() {
    var obj = generarJsonPropiedad();
    var msj = validarJsonPropiedad(obj);

    if (msj == '') {
        $.ajax({
            type: 'POST',
            url: controlador + "GuardarPropiedad",
            data: {
                strConf: JSON.stringify(obj)
            },
            cache: false,
            success: function (model) {
                if (model.Resultado != "-1") {
                    $("#mensajeFormPropiedad").show();
                    mostrarMensaje("mensajeFormPropiedad", "El registro se guardó correctamente.", $tipoMensajeExito, $modoMensajeCuadro);

                    cargarConfiguracionDefault();
                    $('#tab-container').easytabs('select', '#vistaPropiedad');
                    $('#popupFormPropiedad').bPopup().close();
                } else {
                    $("#mensajeFormPropiedad").show();
                    mostrarMensaje("mensajeFormPropiedad", model.Mensaje, $tipoMensajeError, $modoMensajeCuadro);
                }
            },
            error: function () {
            }
        });
    } else {
        $("#mensajeFormPropiedad").show();
        mostrarMensaje("mensajeFormPropiedad", msj, $tipoMensajeError, $modoMensajeCuadro);
    }
}

function generarJsonPropiedad() {
    var codigo = parseInt($("#idPropiedad").val()) || 0;
    var nombre = $("#txtNombrePropiedad").val();
    var nombreFicha = $("#txtNombreFichaPropiedad").val();
    var unidad = $("#txtNombreUnidad").val();

    var obj = {
        Ccombcodi: codigo,
        Ccombnombre: (nombre ?? "").trim(),
        Ccombnombreficha: (nombreFicha ?? "").trim(),
        Ccombunidad: (unidad ?? "").trim(),
    };

    return obj;
}

function validarJsonPropiedad(obj) {
    var msj = '';

    if (obj.Ccombnombre == "")
        msj += "Debe ingresar nombre de propiedad. " + "<br/>";

    return msj;
}

function editarPropiedad(codigo) {
    $("#mensajeFormPropiedad").hide();

    $("#popupFormPropiedad").html('');

    var objProp = obtenerObjPropiedad(codigo, LISTA_PROPIEDAD);
    inicializarFormularioPropiedad(objProp);
}

///////////////////////////
/// Formulario web Plantilla
///////////////////////////

function generarJsonModelo() {
    var codigo = parseInt($("#idPlantilla").val()) || 0;
    var nombre = $("#txtNombrePlantilla").val();
    var fechaVig = $("#txtFormFechaVig").val();

    var obj = {
        Cbftcodi: codigo,
        CbftfechavigenciaDesc: fechaVig,
        Cbftnombre: nombre,
    };

    obj.ListaSeccion = _generarJsonListaSeccion();

    return obj;
}

function _generarJsonListaSeccion() {
    var listaSeccion = [];

    $(`table[id^="table_seccion_"]`).each(function (i, obj) {
        var idHtml = $(obj).attr('id');

        var codigoFila = parseInt(idHtml.split("_")[2]) || 0;

        var cbftitcodi = parseInt($(`#${idHtml} input[name^="codigoSeccion"]`).val()) || 0;
        var numeral = $(`#${idHtml} select[name^="cbNumeralSeccion"]`).val();
        var nombre = $(`#${idHtml} input[name^="txtNombreSeccion"]`).val();

        var propCnp0 = parseInt($(`#${idHtml} select[name="cbItemPropiedadSeccionCnp0"]`).val()) || 0;
        var propCnp1 = parseInt($(`#${idHtml} select[name="cbItemPropiedadSeccionCnp1"]`).val()) || 0;
        var propCnp2 = parseInt($(`#${idHtml} select[name="cbItemPropiedadSeccionCnp2"]`).val()) || 0;
        var propCnp3 = parseInt($(`#${idHtml} select[name="cbItemPropiedadSeccionCnp3"]`).val()) || 0;
        var propCnp4 = parseInt($(`#${idHtml} select[name="cbItemPropiedadSeccionCnp4"]`).val()) || 0;
        var propCnp5 = parseInt($(`#${idHtml} select[name="cbItemPropiedadSeccionCnp5"]`).val()) || 0;
        var propCnp6 = parseInt($(`#${idHtml} select[name="cbItemPropiedadSeccionCnp6"]`).val()) || 0;

        var listaItemXSeccion = _generarJsonListaItemXSeccion(codigoFila);

        var objSeccion = {
            Cbftitcodi: cbftitcodi,
            Ccombcodi: null,
            Cbftitesseccion: 'S',
            Cbftitnombre: nombre,
            Cbftitnumeral: numeral,
            Cbftitcnp0: propCnp0 > 0 ? propCnp0 : null,
            Cbftitcnp1: propCnp1 > 0 ? propCnp1 : null,
            Cbftitcnp2: propCnp2 > 0 ? propCnp2 : null,
            Cbftitcnp3: propCnp3 > 0 ? propCnp3 : null,
            Cbftitcnp4: propCnp4 > 0 ? propCnp4 : null,
            Cbftitcnp5: propCnp5 > 0 ? propCnp5 : null,
            Cbftitcnp6: propCnp6 > 0 ? propCnp6 : null,
            ListaItemXSeccion: listaItemXSeccion
        };

        listaSeccion.push(objSeccion);
    });

    return listaSeccion;
}

function _generarJsonListaItemXSeccion(correlativoSeccion) {
    var listaItem = [];

    $(`tr[id^="tr_table_agrup_${correlativoSeccion}_"]`).each(function (i, obj) {
        var idHtml = $(obj).attr('id');

        var codigoFila = parseInt(idHtml.split("_")[4]) || 0;

        var cbftitcodi = parseInt($(`#${idHtml} input[name^="codigoItem"]`).val()) || 0;
        var numeral = $(`#${idHtml} select[name^="cbItemNumeral"]`).val();
        var propiedad = parseInt($(`#${idHtml} select[name^="cbItemPropiedad"]`).val()) || 0;
        var nombre = '';
        var instructivo = $(`#${idHtml} textarea[name^="txtItemInstructivo"]`).val();
        var tipoDato = $(`#${idHtml} select[name^="cbItemTipo"]`).val();
        var abrev = $(`#${idHtml} input[name^="txtItemAbrev"]`).val();
        var formula = $(`#${idHtml} input[name^="txtItemFormula"]`).val();

        var confid = 'N';
        if ($(`#${idHtml} input[name^="checkItemConfidencial"]`).is(':checked')) confid = 'S';

        var tipoCelda = $(`#${idHtml} select[name^="cbItemTipoCelda"]`).val();

        var celdatipo1 = '';
        var celdatipo2 = '';
        var celdatipo3 = '';
        var celdatipo4 = '';
        var esVisibleInput = tipoCelda == 'M' && tipoDato == 'DESPLEGABLE';
        var esVisibleCombo = (tipoCelda == 'M' && (tipoDato == 'TEXTO' || tipoDato == 'NUMERO' || tipoDato == 'FORMULA')) || (propiedad == 150 || propiedad == 166);

        if (esVisibleInput) {
            celdatipo1 = $(`#${idHtml} input[name^="txtItemTipoCeldaInput1"]`).val();
            celdatipo2 = $(`#${idHtml} input[name^="txtItemTipoCeldaInput2"]`).val();
            celdatipo3 = $(`#${idHtml} input[name^="txtItemTipoCeldaInput3"]`).val();
            celdatipo4 = $(`#${idHtml} input[name^="txtItemTipoCeldaInput4"]`).val();
        }

        if (esVisibleCombo) {
            celdatipo1 = $(`#${idHtml} select[name^="cbItemTipoCeldaInput1"]`).val();
            celdatipo2 = $(`#${idHtml} select[name^="cbItemTipoCeldaInput2"]`).val();
            celdatipo3 = $(`#${idHtml} select[name^="cbItemTipoCeldaInput3"]`).val();
            celdatipo4 = $(`#${idHtml} select[name^="cbItemTipoCeldaInput4"]`).val();
        }

        var objItem = {
            Cbftitcodi: cbftitcodi,
            Ccombcodi: propiedad,
            Cbftitesseccion: 'N',
            Cbftitnombre: nombre,
            Cbftitnumeral: numeral,
            Cbftitinstructivo: instructivo,
            Cbftitabrev: abrev,
            Cbftitformula: formula,
            Cbftittipodato: tipoDato,
            Cbftitconfidencial: confid,
            Cbftittipocelda: tipoCelda,
            Cbftitceldatipo1: celdatipo1,
            Cbftitceldatipo2: celdatipo2,
            Cbftitceldatipo3: celdatipo3,
            Cbftitceldatipo4: celdatipo4,
        };

        listaItem.push(objItem);
    });

    return listaItem;
}

function _generarItem() {

    var objItem = {
        Ccombcodi: null,
        Cbftitesseccion: 'N',
        Cbftitnombre: '',
        Cbftitnumeral: '',
        Cbftitinstructivo: '',
        Cbftitabrev: '',
        Cbftitformula: '',
        Cbftittipodato: 'TEXTO',
        Cbftittipocelda: 'U',
    };

    return objItem;
}

////////////////////////////////////////////////////////////
function inicializarFormulario(objEdit, esEditable) {
    $('#tab-container').easytabs('select', '#vistaDetalle');

    var htmlForm = dibujarHtmlFormulario(objEdit, esEditable);
    $("#div_detalle").html(htmlForm);

    if (esEditable) {
        $("#div_detalle table.btnAcciones").show();
        $("#btnAddSeccion").show();
        $("#mensajeForm").show();
    }

    $("#div_detalle").show();
    $(function () {
        inicializarSelect();

        $('#txtFormFechaVig').unbind();
        $('#txtFormFechaVig').Zebra_DatePicker({
        });

        $('#btnGrabarForm').unbind();
        $("#btnGrabarForm").click(function () {
            guardarFormulario();
        });
    });

}

function inicializarSelect() {

    $('select[name="cbItemPropiedad"]').change(function () {
        var codigo = parseInt($(this).val()) || 0;

        var idHtml = $(this).attr('id');
        var correlativoSeccion = parseInt(idHtml.split("_")[1]) || 0;
        var correlativoTr = parseInt(idHtml.split("_")[2]) || 0;

        var objProp = obtenerObjPropiedad(codigo, LISTA_PROPIEDAD);
        var texto = objProp != null ? objProp.Ccombunidad : '';

        $(`#lblItemUnidad_${correlativoSeccion}_${correlativoTr}`).html(texto);
    });

    $('select[name="cbItemTipo"]').change(function () {
        var itemTipo = $(this).val();

        var idHtml = $(this).attr('id');
        var correlativoSeccion = parseInt(idHtml.split("_")[1]) || 0;
        var correlativoTr = parseInt(idHtml.split("_")[2]) || 0;

        switch (itemTipo) {
            case 'TEXTO':
            case 'FECHA':
                $(`#txtItemAbrev_${correlativoSeccion}_${correlativoTr}`).val('');
                $(`#txtItemFormula_${correlativoSeccion}_${correlativoTr}`).val('');
                $(`#txtItemAbrev_${correlativoSeccion}_${correlativoTr}`).hide();
                $(`#txtItemFormula_${correlativoSeccion}_${correlativoTr}`).hide();
                break;
            case 'NUMERO':
                $(`#txtItemFormula_${correlativoSeccion}_${correlativoTr}`).val('');
                $(`#txtItemAbrev_${correlativoSeccion}_${correlativoTr}`).show();
                $(`#txtItemFormula_${correlativoSeccion}_${correlativoTr}`).hide();
                break;
            case 'FORMULA':
            case 'DESPLEGABLE':
                $(`#txtItemAbrev_${correlativoSeccion}_${correlativoTr}`).val('');
                $(`#txtItemFormula_${correlativoSeccion}_${correlativoTr}`).val('');
                $(`#txtItemAbrev_${correlativoSeccion}_${correlativoTr}`).show();
                $(`#txtItemFormula_${correlativoSeccion}_${correlativoTr}`).show();
                break;
        }


        var itemTipoCelda = $(`#cbItemTipoCelda_${correlativoSeccion}_${correlativoTr}`).val();
        _mostrarOcultarFilaEventoCbItemTipoCelda(itemTipoCelda, correlativoSeccion, correlativoTr);
    });

    $('select[name="cbItemTipoCelda"]').change(function () {
        var itemTipoCelda = $(this).val();
        var idHtml = $(this).attr('id');
        var correlativoSeccion = parseInt(idHtml.split("_")[1]) || 0;
        var correlativoTr = parseInt(idHtml.split("_")[2]) || 0;

        _mostrarOcultarFilaEventoCbItemTipoCelda(itemTipoCelda, correlativoSeccion, correlativoTr);
    });
    ////combo para agregar filas de configuracion
    //$('#cbNumeralSeccion').multipleSelect({
    //    width: '80px',
    //    filter: true,
    //    single: true,
    //    onClose: function () {
    //    }
    //});
    //$('#cbNumeralSeccion').multipleSelect("setSelects", [0]);

    ////combo para seleccionar el numeral del item
    //$('select[name^="cbItemNumeral"]').multipleSelect({
    //    width: '80px',
    //    filter: true,
    //    single: true,
    //    onClose: function () {
    //    }
    //});
    //$('select[name^="cbItemNumeral"]').multipleSelect("setSelects", [0]);

}

function _mostrarOcultarFilaEventoCbItemTipoCelda(itemTipoCelda, correlativoSeccion, correlativoTr) {

    var itemTipo = $(`#cbItemTipo_${correlativoSeccion}_${correlativoTr}`).val();

    $(`#txtItemTipoCeldaInput1_${correlativoSeccion}_${correlativoTr}`).val('');
    $(`#txtItemTipoCeldaInput2_${correlativoSeccion}_${correlativoTr}`).val('');
    $(`#txtItemTipoCeldaInput3_${correlativoSeccion}_${correlativoTr}`).val('');
    $(`#txtItemTipoCeldaInput4_${correlativoSeccion}_${correlativoTr}`).val('');

    $(`#cbItemTipoCelda1_${correlativoSeccion}_${correlativoTr}`).val('E');
    $(`#cbItemTipoCelda2_${correlativoSeccion}_${correlativoTr}`).val('E');
    $(`#cbItemTipoCelda3_${correlativoSeccion}_${correlativoTr}`).val('E');
    $(`#cbItemTipoCelda4_${correlativoSeccion}_${correlativoTr}`).val('E');

    $(`#txtItemTipoCeldaInput1_${correlativoSeccion}_${correlativoTr}`).hide();
    $(`#txtItemTipoCeldaInput2_${correlativoSeccion}_${correlativoTr}`).hide();
    $(`#txtItemTipoCeldaInput3_${correlativoSeccion}_${correlativoTr}`).hide();
    $(`#txtItemTipoCeldaInput4_${correlativoSeccion}_${correlativoTr}`).hide();

    $(`#cbItemTipoCelda1_${correlativoSeccion}_${correlativoTr}`).hide();
    $(`#cbItemTipoCelda2_${correlativoSeccion}_${correlativoTr}`).hide();
    $(`#cbItemTipoCelda3_${correlativoSeccion}_${correlativoTr}`).hide();
    $(`#cbItemTipoCelda4_${correlativoSeccion}_${correlativoTr}`).hide();

    switch (itemTipoCelda) {
        case 'U':

            break;
        case 'M':
            if (itemTipo == 'DESPLEGABLE') {
                $(`#txtItemTipoCeldaInput1_${correlativoSeccion}_${correlativoTr}`).show();
                $(`#txtItemTipoCeldaInput2_${correlativoSeccion}_${correlativoTr}`).show();
                $(`#txtItemTipoCeldaInput3_${correlativoSeccion}_${correlativoTr}`).show();
                $(`#txtItemTipoCeldaInput4_${correlativoSeccion}_${correlativoTr}`).show();
            }

            if (itemTipo == 'TEXTO' || itemTipo == 'NUMERO') {
                $(`#cbItemTipoCelda1_${correlativoSeccion}_${correlativoTr}`).show();
                $(`#cbItemTipoCelda2_${correlativoSeccion}_${correlativoTr}`).show();
                $(`#cbItemTipoCelda3_${correlativoSeccion}_${correlativoTr}`).show();
                $(`#cbItemTipoCelda4_${correlativoSeccion}_${correlativoTr}`).show();
            }
            break;
    }
}

function dibujarHtmlFormulario(objEdit, esEditable) {
    var codigoPlantilla = objEdit != null ? objEdit.Cbftcodi : 0;
    var sFechaActual = objEdit != null ? objEdit.CbftfechavigenciaDesc : $("#hdFechaActual").val();
    var nombrePlantilla = objEdit != null ? objEdit.Cbftnombre : '';
    var listaSeccion = objEdit != null ? objEdit.ListaSeccion : [];

    var htmlDivTablaSeccion = '';
    for (var i = 0; i < listaSeccion.length; i++) {
        var objSeccion = listaSeccion[i];
        htmlDivTablaSeccion += generarHtmlTablaSeccion(objSeccion, CORRELATIVO_SECCION, objSeccion.ListaItemXSeccion, esEditable);
        CORRELATIVO_SECCION++;
    }

    //Div general
    var html = `
    <div class="content-registro" style="width:auto">
        <table class="btnAcciones" style="width: 200px; float: right; display: none">
             <tbody><tr>
                    <td>
                        <input type="button" id="btnGrabarForm" value="Grabar">
                        <input type="button" id="btnCancelarForm" value="Cancelar">
                    </td>
                </tr>
            </tbody>
        </table>

        <div style="clear:both; height:10px;"></div>

        <div id="mensajeForm" class="action-message" style="margin:0; margin-bottom:10px; display: none">Por favor complete los datos</div>

        <table cellpadding="8" style="width:auto">
            <tbody>
                <tr>
                    <td class="registro-label">Nombre:</td>
                    <td class="registro-control">
                        <input type="text" id="txtNombrePlantilla" value='${nombrePlantilla}' />
                        <input type="hidden" id="idPlantilla" value='${codigoPlantilla}' />
                    </td>
                </tr>
                <tr>
                    <td class="registro-label">Fecha de vigencia:</td>
                    <td class="registro-control" colspan="3">
                        <input type="text" id="txtFormFechaVig" style="width: 94px;" value="${sFechaActual}" />
                    </td>
                </tr>
            </tbody>
        </table>


        <!--Secciones-->
        <div class="vistaDetalle popup-title" style="display: block;">
            <span id="tituloDetalle">Secciones e Ítems</span>
        </div>
        <div style='padding-top: 5px;padding-bottom: 20px;'>

            <select id="cbNumeralSeccion" style='width: 50px;'>
                <option value="1.0">1.0</option>
                <option value="2.0">2.0</option>
                <option value="3.0">3.0</option>
                <option value="4.0">4.0</option>
                <option value="5.0">5.0</option>
                <option value="6.0">6.0</option>
                <option value="7.0">7.0</option>
                <option value="8.0">8.0</option>
                <option value="9.0">9.0</option>
                <option value="10.0">10.0</option>
                <option value="11.0">11.0</option>
                <option value="12.0">12.0</option>
                <option value="13.0">13.0</option>
                <option value="14.0">14.0</option>
                <option value="15.0">15.0</option>
                <option value="16.0">16.0</option>
                <option value="17.0">17.0</option>
                <option value="18.0">18.0</option>
                <option value="19.0">19.0</option>
                <option value="20.0">20.0</option>
            </select>

            <input type="button" id='btnAddSeccion' value="Agregar sección" style="display: none" onclick="agregarTableSeccion();">
        </div>
        <div id="div_tabla_seccion" style='min-height: 100px;'>
            ${htmlDivTablaSeccion}
        </div>

    </div>

`;

    return html;
}

function agregarTableSeccion() {
    var numeralSeccion = $("#cbNumeralSeccion").val();
    var objSeccion = {
        Cbftitnombre: '',
        Cbftitnumeral: numeralSeccion
    };
    var htmlTr = generarHtmlTablaSeccion(objSeccion, CORRELATIVO_SECCION, [], true);
    CORRELATIVO_SECCION++;

    $("#div_tabla_seccion").append(htmlTr);

    inicializarSelect();
}

function generarHtmlTablaSeccion(objSeccion, correlativoSeccion, listaItem, esEditable) {

    var htmlTablaAgrup = generarHtmlTablaAgrupItem(objSeccion.Cbftitnumeral, correlativoSeccion, listaItem, esEditable);
    var htmlTd00 = _getTdItemPropiedadSeccion(correlativoSeccion, objSeccion, 0);
    var htmlTd01 = _getTdItemPropiedadSeccion(correlativoSeccion, objSeccion, 1);
    var htmlTd02 = _getTdItemPropiedadSeccion(correlativoSeccion, objSeccion, 2);
    var htmlTd03 = _getTdItemPropiedadSeccion(correlativoSeccion, objSeccion, 3);
    var htmlTd04 = _getTdItemPropiedadSeccion(correlativoSeccion, objSeccion, 4);
    var htmlTd05 = _getTdItemPropiedadSeccion(correlativoSeccion, objSeccion, 5);
    var htmlTd06 = _getTdItemPropiedadSeccion(correlativoSeccion, objSeccion, 6);

    var htmlTdBtn = "";
    if (esEditable)
        htmlTdBtn = `

                        <a title="Eliminar registro" href="JavaScript:quitarTableSeccion(${correlativoSeccion});">  ${IMG_ELIMINAR} </a>
                        <input type="hidden" id="codigoSeccion_${correlativoSeccion}" name='codigoSeccion' value='${objSeccion.Cbftitcodi}'  />
        `;

    var htmlTr = `

    <table id='table_seccion_${correlativoSeccion}' class='table_agrup' style="margin-bottom: 20px;">
        <tbody>

            <tr id='' class='tr_cab'>
                <td style='width: 10px;' class='td01'></td>
                <td style='width: 150px; vertical-align: top;' class='td02'>
                    Numeral :
                    <select id="cbNumeralSeccion_${correlativoSeccion}" name='cbNumeralSeccion' disabled style="width: 55px;">
                        <option value="${objSeccion.Cbftitnumeral}">${objSeccion.Cbftitnumeral}</option>
                    </select>
                </td>

                <td class='td02'>
                    Nombre: <input type="text" id="txtNombreSeccion_${correlativoSeccion}" name='txtNombreSeccion' value='${objSeccion.Cbftitnombre}' style="background-color: white; width: 400px;" />

                    <br/>

                    <table style='margin-top: 15px;'>
                        <tr>
                            <td>Tipo opción sección generador</td>
                            <td>Número de columnas</td>
                            <td>Observación (COES)</td>
                            <td>Subsanación de la observación (Generador)</td>
                            <td>Respuesta Final (COES)</td>
                            <td>Estado</td>
                            <td>Último mes</td>
                        </tr>
                        <tr>
                            <td>${htmlTd00}</td>
                            <td>${htmlTd01}</td>
                            <td>${htmlTd02}</td>
                            <td>${htmlTd03}</td>
                            <td>${htmlTd04}</td>
                            <td>${htmlTd05}</td>
                            <td>${htmlTd06}</td>
                        </tr>
                    </table>

                </td>

                <td class='td02'></td>
                <td class='td02'></td>

                <td style='width: 30px; text-align: center; height: 30px; vertical-align: top;' class='td03' rowspan="2">
                    ${htmlTdBtn}
                </td>
            </tr>

            <tr class='tr_det'>
                <td class='td01'></td>
                <td colspan="5" class='td02'>
                    ${htmlTablaAgrup}
                </td>
                <td class='td03'></td>
            </tr>

        </tbody>
    </table>
`;

    return htmlTr;
}

function quitarTableSeccion(correlativoSeccion) {
    $(`#table_seccion_${correlativoSeccion}`).remove();
}

function generarHtmlTablaAgrupItem(numeralSeccion, correlativoSeccion, listaItem, esEditable) {
    //crear tabla con la primera fila default
    var htmlTrAgrupDefault = '';
    if (listaItem.length > 0) {
        for (var i = 0; i < listaItem.length; i++) {
            htmlTrAgrupDefault += generarHtmlTrFilaItem(numeralSeccion, correlativoSeccion, listaItem[i], esEditable);
        }
    }
    else {
        htmlTrAgrupDefault = generarHtmlTrFilaItem(numeralSeccion, correlativoSeccion, _generarItem(), esEditable);
    }

    var htmlTdBtn = "";
    if (esEditable)
        htmlTdBtn = `
                    <a title="Agregar registro" href="JavaScript:agregarTrAgrupacionItemToTableSeccion(${correlativoSeccion});">  ${IMG_AGREGAR} </a>
        `;

    var html = `
    <table id='table_agrup_item_${correlativoSeccion}' class='table_agrup_item' style="margin-top: 20px;margin-bottom: 20px;">
        <thead>
            <tr>
                <th class='th1'>Acción</th>

                <th class='th1'>Numeral</th>
                <th class='th1'>Nombre</th>
                <th class='th1'>Unidad/Formato</th>
                <th class='th1'>Instructivo</th>
                <th class='th1'>Tipo</th>

                <th class='th1'>Abrev</th>
                <th class='th1'>Fórmula</th>
                <th class='th1'>Confidencialidad</th>
                <th class='th1'>Tipo Celda</th>
                <th class='th1'>Tipo 1</th>

                <th class='th1'>Tipo 2</th>
                <th class='th1'>Tipo 3</th>
                <th class='th1'>Tipo 4</th>

                <th class='th2'>
                    ${htmlTdBtn}
                </th>
            </tr>
        </thead>
        <tbody class='tbody_table_agrup_item'>
            ${htmlTrAgrupDefault}
        </tbody>
    </table>
    `;

    return html;
}

function agregarTrAgrupacionItemToTableSeccion(correlativoSeccion) {
    var numeralSeccion = $(`#cbNumeralSeccion_${correlativoSeccion}`).val();
    var htmlTrAgrupDefault = generarHtmlTrFilaItem(numeralSeccion, correlativoSeccion, _generarItem(), true);

    $(`#table_agrup_item_${correlativoSeccion} tbody.tbody_table_agrup_item`).append(htmlTrAgrupDefault);

    inicializarSelect();
}

function generarHtmlTrFilaItem(numeralSeccion, correlativoSeccion, objItem, esEditable) {

    var htmlTdBtn = "";
    if (esEditable)
        htmlTdBtn = `
                    <a title="Eliminar registro" href="JavaScript:quitarTrFilaItem(${correlativoSeccion},${CORRELATIVO_AGRUP});">  ${IMG_ELIMINAR} </a>
                    <input type="hidden" id="codigoItem_${correlativoSeccion}_${CORRELATIVO_AGRUP}" name='codigoItem' value='${objItem.Cbftitcodi}'  />
        `;

    var htmlTd01 = _getTdItemNumeral(numeralSeccion, correlativoSeccion, CORRELATIVO_AGRUP, objItem);
    var htmlTd02 = _getTdItemPropiedad(correlativoSeccion, CORRELATIVO_AGRUP, objItem);
    var htmlTd03 = _getTdItemUnidad(correlativoSeccion, CORRELATIVO_AGRUP, objItem);
    var htmlTd04 = `<textarea id="txtItemInstructivo_${correlativoSeccion}_${CORRELATIVO_AGRUP}" name='txtItemInstructivo' style="background-color: white; width: 200px;" maxlength="500" autocomplete="off" >${objItem.Cbftitinstructivo}</textarea>`;
    var htmlTd05 = _getTdItemTipo(correlativoSeccion, CORRELATIVO_AGRUP, objItem);
    var htmlTd06 = _getTdItemAbrev(correlativoSeccion, CORRELATIVO_AGRUP, objItem);
    var htmlTd07 = _getTdItemFormula(correlativoSeccion, CORRELATIVO_AGRUP, objItem);
    var htmlTd08 = _getTdItemConfidencial(correlativoSeccion, CORRELATIVO_AGRUP, objItem);
    var htmlTd09 = _getTdItemTipoCelda(correlativoSeccion, CORRELATIVO_AGRUP, objItem);
    var htmlTd10 = _getTdItemTipoCeldaInput(correlativoSeccion, CORRELATIVO_AGRUP, objItem, 1);
    var htmlTd11 = _getTdItemTipoCeldaInput(correlativoSeccion, CORRELATIVO_AGRUP, objItem, 2);
    var htmlTd12 = _getTdItemTipoCeldaInput(correlativoSeccion, CORRELATIVO_AGRUP, objItem, 3);
    var htmlTd13 = _getTdItemTipoCeldaInput(correlativoSeccion, CORRELATIVO_AGRUP, objItem, 4);

    var html = `
            <tr id='tr_table_agrup_${correlativoSeccion}_${CORRELATIVO_AGRUP}' class='tr_table_agrup_item'>

                <td style='width: 30px; text-align: center; border-left: 1px solid #EAEAEA;' class='td03'>
                    ${htmlTdBtn}
                </td>

                <td class='td_agrup' style="padding: 5px;">
                    ${htmlTd01}
                </td>
                <td class='td_agrup' style="padding: 5px;">
                    ${htmlTd02}
                </td>
                <td class='td_agrup' style="padding: 5px; text-align: center; vertical-align: middle;">
                    ${htmlTd03}
                </td>
                <td class='td_agrup' style="padding: 5px;">
                    ${htmlTd04}
                </td>
                <td class='td_agrup' style="padding: 5px;">
                    ${htmlTd05}
                </td>
                <td class='td_agrup' style="padding: 5px;">
                    ${htmlTd06}
                </td>
                <td class='td_agrup' style="padding: 5px;">
                    ${htmlTd07}
                </td>
                <td class='td_agrup' style="padding: 5px;">
                    ${htmlTd08}
                </td>
                <td class='td_agrup' style="padding: 5px;">
                    ${htmlTd09}
                </td>
                <td class='td_agrup' style="padding: 5px;">
                    ${htmlTd10}
                </td>
                <td class='td_agrup' style="padding: 5px;">
                    ${htmlTd11}
                </td>
                <td class='td_agrup' style="padding: 5px;">
                    ${htmlTd12}
                </td>
                <td class='td_agrup' style="padding: 5px;">
                    ${htmlTd13}
                </td>
                <td></td>
            </tr>
    `;

    CORRELATIVO_AGRUP++;

    return html;
}

function _getTdItemPropiedadSeccion(correlativoSeccion, objItem, num) {
    var valorCeldaTipo = '';
    switch (num) {
        case 0: valorCeldaTipo = objItem.Cbftitcnp0;
            break;
        case 1: valorCeldaTipo = objItem.Cbftitcnp1;
            break;
        case 2: valorCeldaTipo = objItem.Cbftitcnp2;
            break;
        case 3: valorCeldaTipo = objItem.Cbftitcnp3;
            break;
        case 4: valorCeldaTipo = objItem.Cbftitcnp4;
            break;
        case 5: valorCeldaTipo = objItem.Cbftitcnp5;
            break;
        case 6: valorCeldaTipo = objItem.Cbftitcnp6;
            break;
    }

    var sHtml = `
        <select id="cbItemPropiedadSeccionCnp${num}_${correlativoSeccion}" name='cbItemPropiedadSeccionCnp${num}' style="width: 250px;">
            <option value="-1">--SELECCIONE--</option>
    `;
    for (var i = 0; i < LISTA_PROPIEDAD.length; i++) {
        var sSelected = '';
        if (valorCeldaTipo == LISTA_PROPIEDAD[i].Ccombcodi) sSelected = ' selected ';

        sHtml += `<option value="${LISTA_PROPIEDAD[i].Ccombcodi}" ${sSelected} >${LISTA_PROPIEDAD[i].Ccombcodi} - ${LISTA_PROPIEDAD[i].Ccombnombre}</option>`;
    }
    sHtml += `</select>`;

    return sHtml;
}

function _getTdItemNumeral(numeralSeccion, correlativoSeccion, correlativoTr, objItem) {
    var sHtml = `
        <select id="cbItemNumeral_${correlativoSeccion}_${correlativoTr}" name='cbItemNumeral' style="width: 55px;">
    `;
    for (var i = 1; i <= 30; i++) {
        var numeralItem = parseInt(numeralSeccion) + "." + (`0${i}`.slice(-2));
        var descNumeralItem = parseInt(numeralSeccion) + "." + i;

        var sSelected = '';
        if (objItem.Cbftitnumeral == numeralItem) sSelected = ' selected ';

        sHtml += `<option value="${numeralItem}" ${sSelected} >${descNumeralItem}</option>`;
    }
    sHtml += `</select>`;

    return sHtml;
}

function _getTdItemPropiedad(correlativoSeccion, correlativoTr, objItem) {
    var sHtml = `
        <select id="cbItemPropiedad_${correlativoSeccion}_${correlativoTr}" name='cbItemPropiedad' style="width: 330px;">
            <option value="-1">--SELECCIONE--</option>
    `;
    for (var i = 0; i < LISTA_PROPIEDAD.length; i++) {
        var sSelected = '';
        if (objItem.Ccombcodi == LISTA_PROPIEDAD[i].Ccombcodi) sSelected = ' selected ';

        sHtml += `<option value="${LISTA_PROPIEDAD[i].Ccombcodi}" ${sSelected} >${LISTA_PROPIEDAD[i].Ccombcodi} - ${LISTA_PROPIEDAD[i].Ccombnombre}</option>`;
    }
    sHtml += `</select>`;

    return sHtml;
}

function _getTdItemUnidad(correlativoSeccion, correlativoTr, objItem) {
    var objProp = obtenerObjPropiedad(objItem.Ccombcodi, LISTA_PROPIEDAD);
    var texto = objProp != null ? objProp.Ccombunidad : '';

    var sHtml = `
        <span id="lblItemUnidad_${correlativoSeccion}_${correlativoTr}" name='lblItemUnidad' >${texto}</span>
    `;
    return sHtml;
}

function _getTdItemTipo(correlativoSeccion, correlativoTr, objItem) {
    var listaTipo = [];
    listaTipo.push({ Texto: "Texto", Codigo: "TEXTO" });
    listaTipo.push({ Texto: "Fecha", Codigo: "FECHA" });
    listaTipo.push({ Texto: "Número", Codigo: "NUMERO" });
    listaTipo.push({ Texto: "Fórmula", Codigo: "FORMULA" });
    listaTipo.push({ Texto: "Desplegable", Codigo: "DESPLEGABLE" });

    var sHtml = `
        <select id="cbItemTipo_${correlativoSeccion}_${correlativoTr}" name='cbItemTipo' style="width: 100px;">
    `;
    for (var i = 0; i < listaTipo.length; i++) {
        var sSelected = '';
        if (objItem.Cbftittipodato == listaTipo[i].Codigo) sSelected = ' selected ';

        sHtml += `<option value="${listaTipo[i].Codigo}" ${sSelected} >${listaTipo[i].Texto}</option>`;
    }
    sHtml += `</select>`;

    return sHtml;
}

function _getTdItemAbrev(correlativoSeccion, correlativoTr, objItem) {
    var esOculto = false;

    switch (objItem.Cbftittipodato) {
        case 'TEXTO':
        case 'FECHA':
            esOculto = true;
            break;
        case 'NUMERO':
            break;
        case 'FORMULA':
        case 'DESPLEGABLE':
            break;
    }

    var styleDisplay = '';
    if (esOculto) styleDisplay = 'display: none;';

    var sHtml = `<input type="text" id="txtItemAbrev_${correlativoSeccion}_${correlativoTr}" name='txtItemAbrev' value='${objItem.Cbftitabrev}' style="background-color: white; width: 146px;${styleDisplay}" maxlength="20" />`;
    return sHtml;
}

function _getTdItemFormula(correlativoSeccion, correlativoTr, objItem) {
    var esOculto = false;

    switch (objItem.Cbftittipodato) {
        case 'TEXTO':
        case 'FECHA':
            esOculto = true;
            break;
        case 'NUMERO':
            esOculto = true;
            break;
        case 'FORMULA':
        case 'DESPLEGABLE':
            break;
    }

    var styleDisplay = '';
    if (esOculto) styleDisplay = 'display: none;';

    var sHtml = `<input type="text" id="txtItemFormula_${correlativoSeccion}_${correlativoTr}" name='txtItemFormula' value='${objItem.Cbftitformula}' style="background-color: white; width: 200px;${styleDisplay}" maxlength="200" />`;
    return sHtml;
}

function _getTdItemConfidencial(correlativoSeccion, correlativoTr, objItem) {
    var esChecked = "";
    if (objItem.Cbftitconfidencial == 'S') esChecked = "checked";

    var sHtml = `
        <input type="checkbox" id="checkItemConfidencial_${correlativoSeccion}_${correlativoTr}" name='checkItemConfidencial' ${esChecked}/>
    `;
    return sHtml;
}

function _getTdItemTipoCelda(correlativoSeccion, correlativoTr, objItem) {
    var listaTipo = [];
    listaTipo.push({ Texto: "Unicelda", Codigo: "U" });
    listaTipo.push({ Texto: "Multicelda", Codigo: "M" });

    var sHtml = `
        <select id="cbItemTipoCelda_${correlativoSeccion}_${correlativoTr}" name='cbItemTipoCelda' style="width: 100px;">
    `;
    for (var i = 0; i < listaTipo.length; i++) {
        var sSelected = '';
        if (objItem.Cbftittipocelda == listaTipo[i].Codigo) sSelected = ' selected ';

        sHtml += `<option value="${listaTipo[i].Codigo}" ${sSelected} >${listaTipo[i].Texto}</option>`;
    }
    sHtml += `</select>`;

    return sHtml;
}

function _getTdItemTipoCeldaInput(correlativoSeccion, correlativoTr, objItem, num) {
    var valorCeldaTipo = '';
    switch (num) {
        case 1: valorCeldaTipo = objItem.Cbftitceldatipo1;
            break;
        case 2: valorCeldaTipo = objItem.Cbftitceldatipo2;
            break;
        case 3: valorCeldaTipo = objItem.Cbftitceldatipo3;
            break;
        case 4: valorCeldaTipo = objItem.Cbftitceldatipo4;
            break;
    }

    var esVisibleInput = objItem.Cbftittipocelda == 'M' && objItem.Cbftittipodato == 'DESPLEGABLE';
    var esVisibleCombo = (objItem.Cbftittipocelda == 'M' && (objItem.Cbftittipodato == 'TEXTO' || objItem.Cbftittipodato == 'NUMERO' || objItem.Cbftittipodato == 'FORMULA'))
        || (objItem.Ccombcodi == 150 || objItem.Ccombcodi == 166);

    var styleDisplayInput = 'display: none';
    if (esVisibleInput) styleDisplayInput = 'display: block;';

    var styleDisplayCombo = 'display: none';
    if (esVisibleCombo) styleDisplayCombo = 'display: block;';

    var sHtml = `<input type="text" id="txtItemTipoCeldaInput${num}_${correlativoSeccion}_${correlativoTr}" name='txtItemTipoCeldaInput${num}' value='${valorCeldaTipo}' style="background-color: white; width: 200px;${styleDisplayInput}" />`;

    var listaTipo = [];
    if (objItem.Cbftittipodato == 'FORMULA') {
        listaTipo.push({ Texto: "No Editable", Codigo: "NE" });
        listaTipo.push({ Texto: "Solo lectura y No Aplica", Codigo: "SLNA" });
        listaTipo.push({ Texto: "Solo lectura", Codigo: "SL" });
    } else {
        listaTipo.push({ Texto: "Editable", Codigo: "E" });
        listaTipo.push({ Texto: "No Editable", Codigo: "NE" });
        listaTipo.push({ Texto: "Solo lectura y No Aplica", Codigo: "SLNA" });
    }
    if (objItem.Ccombcodi == 150 || objItem.Ccombcodi == 166) //3.16 y 4.15
    {
        listaTipo = [];
        listaTipo.push({ Texto: "Solo lectura y No Aplica", Codigo: "SLNA" });
        listaTipo.push({ Texto: "Solo lectura", Codigo: "SL" });
    }

    sHtml += `
        <select id="cbItemTipoCelda${num}_${correlativoSeccion}_${correlativoTr}" name='cbItemTipoCeldaInput${num}' style="width: 100px; ${styleDisplayCombo}">
    `;
    for (var i = 0; i < listaTipo.length; i++) {
        var sSelected = '';
        if (valorCeldaTipo == listaTipo[i].Codigo) sSelected = ' selected ';

        sHtml += `<option value="${listaTipo[i].Codigo}" ${sSelected} >${listaTipo[i].Texto}</option>`;
    }
    sHtml += `</select>`;

    return sHtml;
}

function quitarTrFilaItem(correlativoSeccion, correlativoAgrup) {
    $(`#tr_table_agrup_${correlativoSeccion}_${correlativoAgrup}`).remove();
}

function obtenerObjPropiedad(codigo, listaObj) {
    for (var i = 0; i < listaObj.length; i++) {
        if (listaObj[i].Ccombcodi == codigo)
            return listaObj[i];
    }

    return null;
}