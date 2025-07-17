var controlador = siteRoot + 'Hidrologia/ModeloEmbalse/';

var IMG_EDITAR = '<img src="' + siteRoot + 'Content/Images/btn-edit.png" alt="" width="19" height="19" style="">';
var IMG_ELIMINAR = '<img src="' + siteRoot + 'Content/Images/btn-cancel.png" alt="" width="19" height="19" style="">';
var IMG_VERSIONES = '<img src="' + siteRoot + 'Content/Images/btn-properties.png" alt="" width="19" height="19" style="">';
var IMG_AGREGAR = '<img src="' + siteRoot + 'Content/Images/btn-add.png" alt="" width="19" height="19" style="">';

var CONFIGURACION_DEFAULT = null;

var EQUICODI_FICTICIO = 1000000;
var CORRELATIVO_AGRUP = 0;

var COMPONENTE_TIPO_CAU = '1';
var COMPONENTE_TIPO_CAA = '2';
var COMPONENTE_TIPO_CT = '3';

$(function () {
    $('#tab-container').easytabs({
        animate: false
    });
    $('#tab-container').easytabs('select', '#vistaListado');
    //$('#tab-container').easytabs('select', '#vistaDetalle');

    //formulario
    $("#btnNuevo").click(function () {
        $("#div_detalle").html('');
        inicializarFormulario(null);
    });

    cargarConfiguracionDefault();
});

///////////////////////////
/// Listado
///////////////////////////

function cargarConfiguracionDefault() {

    $('#tab-container').easytabs('select', '#vistaListado');

    $.ajax({
        type: 'POST',
        url: controlador + 'ListadoEmbalse',
        dataType: 'json',
        cache: false,
        success: function (data) {
            if (data.Resultado != "-1") {
                $("#mensaje").hide();
                CONFIGURACION_DEFAULT = data.ConfiguracionDefault;

                var html = dibujarTablaReporteEmbalse(data.ListaEmbalse);
                $("#div_reporte").html(html);
            } else {
                alert("Ha ocurrido un error: " + data.Mensaje);
            }
        },
        error: function (err) {
            alert('Ha ocurrido un error.');
        }
    });
}

function dibujarTablaReporteEmbalse(lista) {

    var cadena = '';
    cadena += `
    <table border="0" class="pretty tabla-icono dataTable" cellspacing="0" width="100%" id="reporteEmbalse">
        <thead>
            <tr>
                <th style='width: 70px'>Acción</th>
                <th style='width: 110px'>Embalse</th>
                <th style='width: 40px'>Volumen PDO/RDO YUPANA</th>
                <th style='width: 110px'>Punto de medición del volumen ejecutado</th>
                <th style='width: 110px'>Central Turbinante</th>
                <th style='width: 70px'>Fecha de vigencia</th>
                <th style='width: 70px'>Usuario modificación</th>
                <th style='width: 70px'>Fecha modificación</th>
            </tr>
        </thead>
        <tbody>
    `;

    for (key in lista) {
        var item = lista[key];

        var sPto = '';
        if (item.Ptomedicodi > 0) sPto = item.Ptomedicodi + ' - ' + item.Ptomedielenomb;

        cadena += `
            <tr>
                <td style="height: 24px">
                    <a title="Editar registro" href="JavaScript:editarModeloEmbalse(${item.Modembcodi});">${IMG_EDITAR} </a>
                    <a title="Eliminar registro" href="JavaScript:eliminarModeloEmbalse(${item.Modembcodi});">  ${IMG_ELIMINAR} </a>
                    <a title="Histórico de versiones" href="JavaScript:mostrarVersiones(${item.Recurcodi});"> ${IMG_VERSIONES} </a>
                </td>
                <td style="text-align: left; height: 24px">${item.Recurnombre}</td>
                <td style="height: 24px">${item.ModembindyupanaDesc}</td>
                <td style="text-align: left; height: 24px">${sPto}</td>
                <td style="height: 24px">${item.CentralTurbinate}</td>
                <td style="height: 24px">${item.ModembfecvigenciaDesc}</td>
                <td style="height: 24px">${item.UltimaModificacionUsuarioDesc}</td>
                <td style="height: 24px">${item.UltimaModificacionFechaDesc}</td>
            </tr>
        `;
    }
    cadena += "</tbody></table>";

    return cadena;
}

function guardarFormulario() {
    var objEmbalse = generarJsonModeloEmbalse();
    var msj = validarJsonModeloEmbalse(objEmbalse);

    if (msj == '') {
        $.ajax({
            type: 'POST',
            url: controlador + "GuardarEmbalse",
            contentType: "application/json; charset=utf-8",
            data: JSON.stringify(objEmbalse),
            cache: false,
            success: function (model) {
                if (model.Resultado != "-1") {
                    $("#mensaje").show();
                    mostrarMensaje("mensaje", "El registro se guardó correctamente.", $tipoMensajeExito, $modoMensajeCuadro);
                    cargarConfiguracionDefault();
                } else {
                    $("#mensaje").show();
                    mostrarMensaje("mensaje", model.Mensaje, $tipoMensajeError, $modoMensajeCuadro);
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

function validarJsonModeloEmbalse(obj) {
    var msj = '';

    if (obj.Recurcodi <= 0)
        msj += "No ha seleccionado embalse. " + "<br/>";
    //if (obj.Modembindyupana == "S" && )

    return msj;
}

function editarModeloEmbalse(modembcodi) {
    $("#mensaje").hide();

    $('#tab-container').easytabs('select', '#vistaDetalle');
    $("#div_detalle").html('');

    $.ajax({
        type: 'POST',
        url: controlador + 'ObtenerEmbalse',
        dataType: 'json',
        data: {
            modembcodi: modembcodi
        },
        cache: false,
        success: function (data) {
            if (data.Resultado != "-1") {
                inicializarFormulario(data.ModeloEmbalse);
            } else {
                alert("Ha ocurrido un error: " + data.Mensaje);
            }
        },
        error: function (err) {
            alert('Ha ocurrido un error.');
        }
    });
}

function eliminarModeloEmbalse(modembcodi) {
    $("#mensaje").hide();

    if(confirm("¿Desea eliminar la configuración del embalse?"))
    $.ajax({
        type: 'POST',
        url: controlador + 'EliminarEmbalse',
        dataType: 'json',
        data: {
            modembcodi: modembcodi
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

//////////////////////////////////////////////////////////////////////////
function mostrarVersiones(recurcodi) {

    $('#listadoHistorial').html('');

    $.ajax({
        type: 'POST',
        url: controlador + "ListarHistorialXEmbalse",
        data: {
            recurcodi: recurcodi,
        },
        success: function (evt) {
            if (evt.Resultado != "-1") {

                var html = dibujarTablaHistorial(evt.ListaEmbalse);
                $('#listadoHistorial').html(html);

                abrirPopup("div_historial");

                setTimeout(function () {
                    $('#tabla_historial').dataTable({
                        "scrollY": 330,
                        "scrollX": true,
                        "sDom": 't',
                        "ordering": false,
                        "bPaginate": false,
                        "iDisplayLength": -1
                    });
                }, 300);
            } else {

                alert('Ha ocurrido un error al listar las versiones :' + evt.Mensaje);
            }
        },
        error: function (err) {
            mostrarMensaje_('mensaje', 'error', 'Ha ocurrido un error.');
        }
    });
}

function dibujarTablaHistorial(lista) {

    var cadena = '';
    cadena += `
    <table border="0" class="pretty tabla-icono dataTable" cellspacing="0" width="100%" id="tabla_historial">
        <thead>
            <tr>
                <th style='width: 110px'>Embalse</th>
                <th style='width: 70px'>Fecha de vigencia</th>
                <th style='width: 70px'>Usuario modificación</th>
                <th style='width: 70px'>Fecha modificación</th>
            </tr>
        </thead>
        <tbody>
    `;

    for (key in lista) {
        var item = lista[key];

        cadena += `
            <tr onclick="mostrarExcelWebFromHistorial(${item.Modembcodi});" style="cursor:pointer" >
                <td style="text-align: left; height: 24px">${item.Recurnombre}</td>
                <td style="height: 24px">${item.ModembfecvigenciaDesc}</td>
                <td style="height: 24px">${item.UltimaModificacionUsuarioDesc}</td>
                <td style="height: 24px">${item.UltimaModificacionFechaDesc}</td>
            </tr>
        `;
    }
    cadena += "</tbody></table>";

    return cadena;
}

function mostrarExcelWebFromHistorial(codigo) {
    $('#div_historial').bPopup().close();
    editarModeloEmbalse(codigo);
}

///////////////////////////
/// Formulario web
///////////////////////////

function generarJsonModeloEmbalse() {
    var recurcodi = parseInt($("#cbFormEmbalse").val()) || 0;
    var ptomedicodi = parseInt($("#cbFormPtoVolEjec").val()) || 0;
    var modembindyupana = $("#chkFormVolumen").is(':checked') ? 'S' : 'N';
    var modembfecvigenciaDesc = $("#txtFormFechaVig").val();

    var obj = {
        Modembcodi: 0,
        Recurcodi: recurcodi,
        Ptomedicodi: ptomedicodi,
        Modembindyupana: modembindyupana,
        ModembfecvigenciaDesc: modembfecvigenciaDesc,
        ListaComponente: []
    };

    var arrayCEnt = generarJsonCaudalEntrada('CAU', 1);
    obj.ListaComponente = obj.ListaComponente.concat(arrayCEnt);

    var arrayCAA = generarJsonCentrales('CAA', 2);
    obj.ListaComponente = obj.ListaComponente.concat(arrayCAA);

    var arrayCT = generarJsonCentrales('CT', 3);
    obj.ListaComponente = obj.ListaComponente.concat(arrayCT);

    return obj;
}

function generarJsonCaudalEntrada(sufijo, modcomtipo) {
    var listaComp = [];

    var obj = {
        Equicodi: null,
        Modcomtipo: modcomtipo,
        Modcomtviaje: null,
        ListaAgrupacion: []
    };

    obj.ListaAgrupacion = generarJsonAgrupacionCaudal(sufijo, EQUICODI_FICTICIO);

    listaComp.push(obj);

    return listaComp;
}

function generarJsonCentrales(sufijo, modcomtipo) {
    var listaComp = [];

    $(`table[id^="table_central_${sufijo}_"]`).each(function (i, obj) {
        var idHtml = $(obj).attr('id');

        var equicodi = idHtml.split("_")[3];
        var modcomtviaje = parseFloat($(`#txtFormTviaje_${sufijo}_${equicodi}`).val()) || 0;

        var obj = {
            Equicodi: equicodi,
            Modcomtipo: modcomtipo,
            Modcomtviaje: modcomtviaje,
            ListaAgrupacion: []
        };

        obj.ListaAgrupacion = generarJsonAgrupacionCentral(sufijo, equicodi);

        listaComp.push(obj);
    });

    return listaComp;
}

function generarJsonAgrupacionCaudal(sufijo, equicodi) {
    var listaAgrup = [];
    var orden = 1;

    $(`tr[id^="tr_table_agrup_${sufijo}_${equicodi}_"]`).each(function (i, obj) {
        var idHtml = $(obj).attr('id');

        var filaAgrup = idHtml.split("_")[5];

        var obj = {
            Modagrorden: orden,
            ListaConfiguracion: []
        };

        var listaCau = generarJsonConfiguracion('CAUENT', sufijo, equicodi, filaAgrup);
        obj.ListaConfiguracion = obj.ListaConfiguracion.concat(listaCau);

        var listaYUPANA = generarJsonConfiguracion('CAUYUPANA', sufijo, equicodi, filaAgrup);
        obj.ListaConfiguracion = obj.ListaConfiguracion.concat(listaYUPANA);

        listaAgrup.push(obj);
        orden++;
    });

    return listaAgrup;
}

function generarJsonAgrupacionCentral(sufijo, equicodi) {
    var listaAgrup = [];
    var orden = 1;

    $(`tr[id^="tr_table_agrup_${sufijo}_${equicodi}_"]`).each(function (i, obj) {
        var idHtml = $(obj).attr('id');

        var filaAgrup = idHtml.split("_")[5];

        var obj = {
            Modagrorden: orden,
            ListaConfiguracion: []
        };

        var listaIdcc = generarJsonConfiguracion('IDCC', sufijo, equicodi, filaAgrup);
        obj.ListaConfiguracion = obj.ListaConfiguracion.concat(listaIdcc);

        var listaTNA = generarJsonConfiguracion('TNA', sufijo, equicodi, filaAgrup);
        obj.ListaConfiguracion = obj.ListaConfiguracion.concat(listaTNA);

        var listaYUPANA = generarJsonConfiguracion('POTYUPANA', sufijo, equicodi, filaAgrup);
        obj.ListaConfiguracion = obj.ListaConfiguracion.concat(listaYUPANA);

        listaAgrup.push(obj);
        orden++;
    });

    return listaAgrup;
}

function generarJsonConfiguracion(fuente, sufijo, equicodi, filaAgrup) {
    var listaConf = [];

    $(`tr[id^="tr_agrup_conf_${fuente}_${sufijo}_${equicodi}_${filaAgrup}_"]`).each(function (i, obj) {
        var idHtml = $(obj).attr('id');

        var codigoFila = parseInt(idHtml.split("_")[7]) || 0;

        var modcontipo = '';
        var equicodiTna = null;
        var recurcodi = null;
        var ptomedicodi = null;
        var modconsigno = $(`#${idHtml} select[name^="operador"]`).val();

        switch (fuente) {
            case 'CAUENT':
                modcontipo = '1';
                ptomedicodi = codigoFila;
                break;
            case 'CAUYUPANA':
                modcontipo = '2';
                recurcodi = codigoFila;
                break;
            case 'IDCC':
                modcontipo = '3';
                ptomedicodi = codigoFila;
                break;
            case 'TNA':
                modcontipo = '4';
                equicodiTna = codigoFila;
                break;
            case 'POTYUPANA':
                modcontipo = '5';
                recurcodi = codigoFila;
                break;
        }

        var objConf = {
            Modcontipo: modcontipo,
            Modconsigno: modconsigno,
            Recurcodi: recurcodi,
            Ptomedicodi: ptomedicodi,
            Equicodi: equicodiTna
        };

        listaConf.push(objConf);
    });

    return listaConf;
}

////////////////////////////////////////////////////////////
function inicializarFormulario(objEdit) {
    $('#tab-container').easytabs('select', '#vistaDetalle');

    var htmlForm = dibujarHtmlFormulario(objEdit);
    $("#div_detalle").html(htmlForm);

    $("#div_detalle").show();
    $(function () {
        $("#cbFormEmbalse").unbind();
        $("#txtFormFechaVig").unbind();
        $("#chkFormVolumen").unbind();
        $("#cbFormPtoVolEjec").unbind();
        $("#cbFormEmbalse").unbind();

        $("#btnGrabarForm").unbind();
        $("#btnCancelarForm").unbind();

        $('#txtFormFechaVig').Zebra_DatePicker({
        });

        $('#cbFormPtoVolEjec').multipleSelect({
            width: '220px',
            filter: true,
            single: true,
            onClose: function () {
            }
        });
        $("#cbFormPtoVolEjec").multipleSelect("setSelects", [$("#cbFormPtoVolEjec").val() || 0]);
        $('#cbFormEmbalse').multipleSelect({
            width: '220px',
            filter: true,
            single: true,
            onClose: function () {
            }
        });
        $("#cbFormEmbalse").multipleSelect("setSelects", [$("#cbFormEmbalse").val() || 0]);

        inicializarSelect();

        $("#btnGrabarForm").click(function () {
            guardarFormulario();
        });
    });

}

function inicializarSelect() {

    //combo para agregar filas de configuracion
    $('select[name^="cbFormFuente"]').multipleSelect({
        width: '220px',
        filter: true,
        single: true,
        onClose: function () {
        }
    });
    $('select[name^="cbFormFuente"]').multipleSelect("setSelects", [0]);

    //combo para agregar central
    $('select[id^="cbFormCentral"]').multipleSelect({
        width: '220px',
        filter: true,
        single: true,
        onClose: function () {
        }
    });
    $('select[id^="cbFormCentral"]').multipleSelect("setSelects", [0]);
}

function dibujarHtmlFormulario(objEdit) {
    //fecha actual
    var sFechaActual = $("#hdFechaActual").val();
    if (objEdit != null) sFechaActual = objEdit.ModembfecvigenciaDesc;

    //embalse
    var idRecurcodi = objEdit != null ? objEdit.Recurcodi : 0;

    var htmlcbFormEmbalse = `<option value='0'>--SELECCIONE--</option>`;
    var listaEmbalse = CONFIGURACION_DEFAULT.ListaRecursoEmbalse;
    for (var i = 0; i < listaEmbalse.length; i++) {
        var v = listaEmbalse[i];
        var esSelected = v.Recurcodi == idRecurcodi ? ' selected ' : '';
        htmlcbFormEmbalse += `<option value='${v.Recurcodi}' ${esSelected}>${v.Recurcodi} - ${v.Recurnombre}</option>`;
    }

    //checked
    var esChecked = '';
    if (objEdit != null && objEdit.Modembindyupana == "S") esChecked = ' checked ';

    //punto de medicion volumen
    var idPtomedicodi = objEdit != null ? objEdit.Ptomedicodi : 0;

    var htmlcbFormPtoVolEjec = `<option value='0'>--SELECCIONE--</option>`;
    var listaPtoVolEjec = CONFIGURACION_DEFAULT.ListaPtoVolumen;
    for (var i = 0; i < listaPtoVolEjec.length; i++) {
        var v = listaPtoVolEjec[i];
        var esSelected = v.Ptomedicodi == idPtomedicodi ? ' selected ' : '';
        htmlcbFormPtoVolEjec += `<option value='${v.Ptomedicodi}' ${esSelected}>${v.Ptomedicodi} - ${v.Ptomedielenomb}</option>`;
    }

    //lista central
    var htmlcbCentral = `<option value='0'> --SELECCIONE--</option>`;
    var listaEqCentral = CONFIGURACION_DEFAULT.ListaEqCentral;
    for (var i = 0; i < listaEqCentral.length; i++) {
        var v = listaEqCentral[i];
        htmlcbCentral += `<option value='${v.Equicodi}'> ${v.Equinomb}</option>`;
    }

    //html tabla Caudal
    var htmlTablaCaudal = '';

    var htmlDivTablaCAA = '';
    var htmlDivTablaCT = '';
    if (objEdit != null) {
        for (var i = 0; i < objEdit.ListaComponente.length; i++) {
            var objComp = objEdit.ListaComponente[i];

            if (objComp.Modcomtipo == COMPONENTE_TIPO_CAU) {
                htmlTablaCaudal = generarHtmlTablaAgrupCaudal('CAU', EQUICODI_FICTICIO, objComp.ListaAgrupacion);
            }
            if (objComp.Modcomtipo == COMPONENTE_TIPO_CAA) {
                var htmlTr = generarHtmlTablaCentral('CAA', objComp.Equicodi, objComp.Modcomtviaje, objComp.ListaAgrupacion);
                htmlDivTablaCAA += htmlTr;
            }
            if (objComp.Modcomtipo == COMPONENTE_TIPO_CT) {
                var htmlTr = generarHtmlTablaCentral('CT', objComp.Equicodi, objComp.Modcomtviaje, objComp.ListaAgrupacion);
                htmlDivTablaCT += htmlTr;
            }
        }
    }
    if (htmlTablaCaudal == '')
        htmlTablaCaudal = generarHtmlTablaAgrupCaudal('CAU', EQUICODI_FICTICIO, []);

    //Div general
    var html = `
    <div class="content-registro" style="width:auto">
        <table class="btnAcciones" style="width: 200px; float: right;">
             <tbody><tr>
                    <td>
                        <input type="button" id="btnGrabarForm" value="Grabar">
                    </td>
                    <td>
                        <input type="button" id="btnCancelarForm" value="Cancelar">
                    </td>
                </tr>
            </tbody>
        </table>

        <div style="clear:both; height:10px;"></div>

        <div id="mensajeForm" class="action-message" style="margin:0; margin-bottom:10px; ">Por favor complete los datos</div>

        <table cellpadding="8" style="width:auto">
            <tbody>
                <tr>
                    <td class="registro-label" style="width:270px;">Embalse:</td>
                    <td class="registro-control">
                        <select id='cbFormEmbalse'>
                            ${htmlcbFormEmbalse}
                        </select>
                    </td>
                </tr>
                <tr>
                    <td class="registro-label">Fecha de vigencia:</td>
                    <td class="registro-control" colspan="3">
                        <input type="text" id="txtFormFechaVig" style="width: 94px;" value="${sFechaActual}" />
                    </td>
                </tr>
                <tr>
                    <td class="registro-label">Volumen PDO/RDO YUPANA:</td>
                    <td class="registro-control">
                        <input type="checkbox" id="chkFormVolumen" ${esChecked} />
                    </td>
                </tr>
                <tr>
                    <td class="registro-label">Punto de medición de volumen ejecutado:</td>
                    <td class="registro-control">
                        <select id='cbFormPtoVolEjec'>
                            ${htmlcbFormPtoVolEjec}
                        </select>
                    </td>
                </tr>

            </tbody>
        </table>

        <!--Caudales de entrada-->
        <div class="vistaDetalle popup-title" style="display: block;">
            <span id="tituloDetalle">Caudales de entrada</span>
        </div>
        <div id="div_tabla_CAU" style='min-height: 100px;width: 1000px; margin-bottom: 20px;'>
            ${htmlTablaCaudal}
        </div>

        <!--Centrales aguas arriba-->
        <div class="vistaDetalle popup-title" style="display: block;">
            <span id="tituloDetalle">Centrales aguas arriba</span>
        </div>
        <div style='padding-top: 5px;padding-bottom: 20px;'>
            <select id='cbFormCentralCAA'>
                ${htmlcbCentral}
            </select>
            <input type="button" value="Agregar central" style="" onclick="agregarTableCentral('CAA');">
        </div>
        <div id="div_tabla_CAA" style='min-height: 100px;'>
            ${htmlDivTablaCAA}
        </div>

        <!--Centrales turbinantes-->
        <div class="vistaDetalle popup-title" style="display: block;">
            <span id="tituloDetalle">Centrales turbinantes</span>
        </div>
        <div style='padding-top: 5px;padding-bottom: 20px;'>
            <select id='cbFormCentralCT'>
                ${htmlcbCentral}
            </select>
            <input type="button" value="Agregar central" style="" onclick="agregarTableCentral('CT');">
        </div>
        <div id="div_tabla_CT" style='min-height: 100px;'>
            ${htmlDivTablaCT}
        </div>

    </div>

`;

    return html;
}

function agregarTableCentral(sufijo) {
    //TODO validar existencia de table 

    var equicodi = parseInt($("#cbFormCentral" + sufijo).val()) || 0;
    var tiempoViaje = '';

    var htmlTr = generarHtmlTablaCentral(sufijo, equicodi, tiempoViaje, []);

    $("#div_tabla_" + sufijo).append(htmlTr);

    inicializarSelect();
}

function generarHtmlTablaCentral(sufijo, equicodi, tiempoViaje, listaAgrup) {
    var objEq = obtenerObjEquipo(equicodi, CONFIGURACION_DEFAULT.ListaEqCentral);

    var equinomb = objEq.Equinomb ?? '';
    var rendimiento = objEq.Rendimiento ?? '';
    tiempoViaje = tiempoViaje ?? '';

    var htmlTablaAgrup = generarHtmlTablaAgrupCentral(sufijo, equicodi, listaAgrup);

    var styleMostrarTiempoviaje = 'CT' == sufijo ? 'DISPLAY: none;': '';

    var htmlTr = `

    <table id='table_central_${sufijo}_${equicodi}' class='table_agrup' style="width: 1000px; margin-bottom: 20px;">
        <tbody>

            <tr id='' class='tr_cab'>
                <td style='width: 10px;' class='td01'></td>
                <td style='font-weight: bold;' class='td02'>${equinomb}</td>

                <td class='td02'></td>
                <td class='td02'></td>

                <td class='td02' style="width: 200px;">
                    Rendimiento: ${rendimiento} (MW⁄m3/s)
                </td>
                <td class='td02' style="width: 200px; ${styleMostrarTiempoviaje}">
                    Tiempo de viaje: <input type="text" id="txtFormTviaje_${sufijo}_${equicodi}" value='${tiempoViaje}' style="background-color: white; width: 40px;" /> (horas)
                </td>
        
                <td style='width: 30px; text-align: center; height: 30px;' class='td03'>
                    <a title="Eliminar registro" href="JavaScript:quitarTableCentral('${sufijo}',${equicodi});">  ${IMG_ELIMINAR} </a>
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

function quitarTableCentral(sufijo, equicodi) {
    $(`#table_central_${sufijo}_${equicodi}`).remove();
}

function generarHtmlTablaAgrupCentral(sufijo, equicodi, listaAgrup) {
    //crear tabla con la primera fila default
    var htmlTrAgrupDefault = '';
    if (listaAgrup.length > 0) {
        for (var i = 0; i < listaAgrup.length; i++) {
            htmlTrAgrupDefault += generarHtmlTrAgrupacionCentral(sufijo, equicodi, listaAgrup[i].ListaConfiguracion);
        }
    }
    else {
        htmlTrAgrupDefault = generarHtmlTrAgrupacionCentral(sufijo, equicodi, []);
    }

    var html = `
    <table id='table_central_agrup_${sufijo}_${equicodi}' class='table_central_agrup' style="margin-top: 20px;margin-bottom: 20px;">
        <thead>
            <tr>
                <th class='th1'>Ptos medición IDCC</th>
                <th class='th1'>Ptos medición TNA</th>
                <th class='th1'>Códigos YUPANA</th>
                <th class='th2'>
                    <a title="Agregar registro" href="JavaScript:agregarTrAgrupacionToTableCentral('${sufijo}',${equicodi});">  ${IMG_AGREGAR} </a>
                </th>
            </tr>
        </thead>
        <tbody class='tbody_table_central_agrup'>
            ${htmlTrAgrupDefault}
        </tbody>
    </table>
    `;

    return html;
}

function generarHtmlTablaAgrupCaudal(sufijo, equicodi, listaAgrup) {
    //crear tabla con la primera fila default
    var htmlTrAgrupDefault = '';
    if (listaAgrup.length > 0) {
        for (var i = 0; i < listaAgrup.length; i++) {
            htmlTrAgrupDefault += generarHtmlTrAgrupacionCaudal(sufijo, equicodi, listaAgrup[i].ListaConfiguracion);
        }
    }
    else {
        htmlTrAgrupDefault = generarHtmlTrAgrupacionCaudal(sufijo, equicodi, []);
    }

    var html = `
    <table id='table_caudal_agrup_${sufijo}_${equicodi}' class='table_central_agrup' style="margin-top: 20px;margin-bottom: 20px;">
        <thead>
            <tr>
                <th class='th1'>Ptos medición caudal entrada</th>
                <th class='th1'>Códigos YUPANA</th>
                <th class='th2'>
                    <a title="Agregar registro" href="JavaScript:agregarTrAgrupacionToTableCaudal('${sufijo}',${equicodi});">  ${IMG_AGREGAR} </a>
                </th>
            </tr>
        </thead>
        <tbody class='tbody_table_caudal_agrup'>
            ${htmlTrAgrupDefault}
        </tbody>
    </table>
    `;

    return html;
}

function agregarTrAgrupacionToTableCentral(sufijo, equicodi) {
    var htmlTrAgrupDefault = generarHtmlTrAgrupacionCentral(sufijo, equicodi, []);

    $(`#table_central_agrup_${sufijo}_${equicodi} tbody.tbody_table_central_agrup`).append(htmlTrAgrupDefault);

    inicializarSelect();
}

function agregarTrAgrupacionToTableCaudal(sufijo, equicodi) {
    var htmlTrAgrupDefault = generarHtmlTrAgrupacionCaudal(sufijo, equicodi, []);

    $(`#table_caudal_agrup_${sufijo}_${equicodi} tbody.tbody_table_caudal_agrup`).append(htmlTrAgrupDefault);

    inicializarSelect();
}

function generarHtmlTrAgrupacionCentral(sufijo, equicodi, listaConfig) {
    var htmlIdcc = generarHtmlTablaAgrupXFuente('IDCC', sufijo, equicodi, CORRELATIVO_AGRUP, listaConfig);
    var htmlTNA = generarHtmlTablaAgrupXFuente('TNA', sufijo, equicodi, CORRELATIVO_AGRUP, listaConfig);
    var htmlYupana = generarHtmlTablaAgrupXFuente('POTYUPANA', sufijo, equicodi, CORRELATIVO_AGRUP, listaConfig);

    var html = `
            <tr id='tr_table_agrup_${sufijo}_${equicodi}_${CORRELATIVO_AGRUP}' class='tr_table_central_agrup'>
                <td class='td_agrup' style="padding: 20px;">
                    ${htmlIdcc}
                </td>
                <td class='td_agrup' style="padding: 20px;">
                    ${htmlTNA}
                </td>
                <td class='td_agrup' style="padding: 20px;">
                    ${htmlYupana}
                </td>
                <td></td>
            </tr>
    `;

    CORRELATIVO_AGRUP++;

    return html;
}

function generarHtmlTrAgrupacionCaudal(sufijo, equicodi, listaConfig) {
    var htmlIdcc = generarHtmlTablaAgrupXFuente('CAUENT', sufijo, equicodi, CORRELATIVO_AGRUP, listaConfig);
    var htmlYupana = generarHtmlTablaAgrupXFuente('CAUYUPANA', sufijo, equicodi, CORRELATIVO_AGRUP, listaConfig);

    var html = `
            <tr id='tr_table_agrup_${sufijo}_${equicodi}_${CORRELATIVO_AGRUP}' class='tr_table_central_agrup'>
                <td class='td_agrup' style="padding: 20px;">
                    ${htmlIdcc}
                </td>
                <td class='td_agrup' style="padding: 20px;">
                    ${htmlYupana}
                </td>
                <td></td>
            </tr>
    `;

    CORRELATIVO_AGRUP++;

    return html;
}

function generarHtmlTablaAgrupXFuente(fuente, sufijo, equicodi, correlativoAgrup, listaConfig) {

    //punto de medicion volumen
    var htmlcbFormFuente = '';
    switch (fuente) {
        case 'CAUENT':
            var listaPtoCau = CONFIGURACION_DEFAULT.ListaPtoCaudal;
            for (var i = 0; i < listaPtoCau.length; i++) {
                var v = listaPtoCau[i];
                htmlcbFormFuente += `<option value='${v.Ptomedicodi}'>${v.Ptomedicodi} - ${v.Ptomedielenomb}</option>`;
            }
            break;
        case 'IDCC':
            var listaPtoIDCC = CONFIGURACION_DEFAULT.ListaPtoIDCC;
            for (var i = 0; i < listaPtoIDCC.length; i++) {
                var v = listaPtoIDCC[i];
                htmlcbFormFuente += `<option value='${v.Ptomedicodi}'>${v.Ptomedicodi} - ${v.Ptomedielenomb}</option>`;
            }
            break;
        case 'TNA':
            var listaPtoTNA = CONFIGURACION_DEFAULT.ListaEqTNA;
            for (var i = 0; i < listaPtoTNA.length; i++) {
                var v = listaPtoTNA[i];
                htmlcbFormFuente += `<option value='${v.Equicodi}'>${v.Equicodi} - ${v.Areanomb} ${v.Equiabrev}</option>`;
            }
            break;
        case 'CAUYUPANA':
            var listaPtoY = CONFIGURACION_DEFAULT.ListaRecursoEmbalse;
            for (var i = 0; i < listaPtoY.length; i++) {
                var v = listaPtoY[i];
                htmlcbFormFuente += `<option value='${v.Recurcodi}'>${v.Recurcodi} - ${v.Recurnombre}</option>`;
            }
            break;
        case 'POTYUPANA':
            var listaPtoY2 = CONFIGURACION_DEFAULT.ListaRecursoPlantaH;
            for (var i = 0; i < listaPtoY2.length; i++) {
                var v = listaPtoY2[i];
                htmlcbFormFuente += `<option value='${v.Recurcodi}'>${v.Recurcodi} - ${v.Recurnombre}</option>`;
            }
            break;
    }

    //html trs
    var htmlTr = '';
    var modcontipo = obtenerTipoXAgrup(fuente);
    var listaConfXAgrup = obtenerListaObjConf(modcontipo, listaConfig);
    for (var i = 0; i < listaConfXAgrup.length; i++) {
        var objConfig = listaConfXAgrup[i];
        var codigoFila = 0;

        switch (objConfig.Modcontipo) {
            case '1': //'CAUENT'
            case '3': //'IDCC'
                codigoFila = objConfig.Ptomedicodi;
                break;
            case '2': //'CAUYUPANA'
            case '5': //'POTYUPANA'
                codigoFila = objConfig.Recurcodi;
                break;
            case '4': //'TNA'
                codigoFila = objConfig.Equicodi;
                break;
        }
        if (codigoFila > 0)
            htmlTr += generarHtmlTrAgrupConf(fuente, sufijo, equicodi, correlativoAgrup, codigoFila, objConfig.Modconsigno);
    }

    //
    var html = `
    <table style="">
        <tbody id='tbody_fuente_${fuente}_${sufijo}_${equicodi}_${correlativoAgrup}'>
            <tr>
                <td colspan="3" style="padding: 10px;text-align: center;">
                    <select name='cbFormFuente${fuente}'>
                        <option value='0'>--SELECCIONE--</option>
                        ${htmlcbFormFuente}
                    </select>
                    <a title="Agregar registro" href="JavaScript:addFuenteToAgrupacionTrCentral('${fuente}','${sufijo}',${equicodi}, ${correlativoAgrup});">  ${IMG_AGREGAR} </a>
                </td>
            </tr>
            ${htmlTr}
        </tbody>
    </table>
    `;

    return html;
}

function addFuenteToAgrupacionTrCentral(fuente, sufijo, equicodi, correlativoAgrup) {
    var idTbody = `tbody_fuente_${fuente}_${sufijo}_${equicodi}_${correlativoAgrup}`;
    var idSelect = `#${idTbody} select[name='cbFormFuente${fuente}']`;
    var codigoFila = parseInt($(idSelect).val()) || 0;

    var htmlTr = generarHtmlTrAgrupConf(fuente, sufijo, equicodi, correlativoAgrup, codigoFila, '+');

    $("#" + idTbody).append(htmlTr);

    inicializarSelect();
}

function generarHtmlTrAgrupConf(fuente, sufijo, equicodi, correlativoAgrup, codigoFila, signo) {

    var desc = '';
    switch (fuente) {
        case 'CAUENT':
            var regPto = obtenerObjPto(codigoFila, CONFIGURACION_DEFAULT.ListaPtoCaudal);
            if (regPto != null) desc = codigoFila + ' - ' + regPto.Ptomedielenomb;
            break;
        case 'IDCC':
            var regPto = obtenerObjPto(codigoFila, CONFIGURACION_DEFAULT.ListaPtoIDCC);
            if (regPto != null) desc = codigoFila + ' - ' + regPto.Ptomedielenomb;
            break;
        case 'TNA':
            var regEq = obtenerObjEquipo(codigoFila, CONFIGURACION_DEFAULT.ListaEqTNA);
            if (regEq != null) desc = codigoFila + ' - ' + regEq.Areanomb + " " + regEq.Equiabrev;
            break;
        case 'CAUYUPANA':
            var regYup = obtenerObjYup(codigoFila, CONFIGURACION_DEFAULT.ListaRecursoEmbalse);
            if (regYup != null) desc = codigoFila + ' - ' + regYup.Recurnombre;
            break;
        case 'POTYUPANA':
            var regYup = obtenerObjYup(codigoFila, CONFIGURACION_DEFAULT.ListaRecursoPlantaH);
            if (regYup != null) desc = codigoFila + ' - ' + regYup.Recurnombre;
            break;
    }

    var esSelectedMas = signo == '+' ? ' selected ' : '';
    var esSelectedMenos = signo == '-' ? ' selected ' : '';

    var htmlTr = `
    <tr id='tr_agrup_conf_${fuente}_${sufijo}_${equicodi}_${correlativoAgrup}_${codigoFila}'>
        <td style='width: 10px;' class='tr_conf_td01'>
            <a title="Eliminar registro" href="JavaScript:quitFuenteToAgrupacionTrCentral('${fuente}', '${sufijo}',${equicodi},${correlativoAgrup}, ${codigoFila});">  ${IMG_ELIMINAR} </a>
        </td>
        <td style='' class='tr_conf_td02'>${desc}</td>

        <td style='width: 40px;' class='tr_conf_td03'>
            <select name='operador' style='width: 40px;'>
                <option value='+' ${esSelectedMas}>+</option>
                <option value='-' ${esSelectedMenos}>-</option>
            </select>
        </td>
    </tr>
    `;

    return htmlTr;
}

function quitFuenteToAgrupacionTrCentral(fuente, sufijo, equicodi, correlativoAgrup, codigoFila) {
    $(`#tr_agrup_conf_${fuente}_${sufijo}_${equicodi}_${correlativoAgrup}_${codigoFila}`).remove();
}

//
function obtenerObjEquipo(equicodi, listaObjEq) {
    for (var i = 0; i < listaObjEq.length; i++) {
        if (listaObjEq[i].Equicodi == equicodi)
            return listaObjEq[i];
    }

    return null;
}
function obtenerObjPto(ptomedicodi, listaObjPto) {

    for (var i = 0; i < listaObjPto.length; i++) {
        if (listaObjPto[i].Ptomedicodi == ptomedicodi)
            return listaObjPto[i];
    }

    return null;
}
function obtenerObjYup(recurcodi, listaObjYup) {

    for (var i = 0; i < listaObjYup.length; i++) {
        if (listaObjYup[i].Recurcodi == recurcodi)
            return listaObjYup[i];
    }

    return null;
}

function obtenerListaObjConf(tipo, listaConf) {
    var lista = [];
    for (var i = 0; i < listaConf.length; i++) {
        if (listaConf[i].Modcontipo == tipo)
            lista.push(listaConf[i]);
    }

    return lista;
}
function obtenerTipoXAgrup(tipo) {
    var modcontipo = '0';
    switch (tipo) {
        case 'CAUENT':
            modcontipo = '1';
            break;
        case 'IDCC':
            modcontipo = '3';
            break;
        case 'CAUYUPANA':
            modcontipo = '2';
            break;
        case 'POTYUPANA':
            modcontipo = '5';
            break;

        case 'TNA':
            modcontipo = '4';
            break;
    }

    return modcontipo;
}

function abrirPopup(contentPopup) {
    setTimeout(function () {
        $("#" + contentPopup).bPopup({
            easing: 'easeOutBack',
            speed: 450,
            transition: 'slideDown',
            modalClose: false
        });
    }, 50);
}
