var controlador = siteRoot + 'Titularidad/Configuracion/';

var TIPO_MIGR_DUPLICIDAD = 1;
var TIPO_MIGR_INSTALACION_NO_CORRESPONDEN = 2;
var TIPO_MIGR_CAMBIO_RAZON_SOCIAL = 3;
var TIPO_MIGR_FUSION = 4;
var TIPO_MIGR_TRANSFERENCIA = 5;

var LISTA_TIPOOPERACION = [];
var LISTA_PARAMETRO = [];
var LISTA_PLANTILLA = [];
var PLANTILLA_SELECTED = null;

$(function () {

    $('#tab-container').easytabs({
        animate: false
    });
    $('#tab-container').bind('easytabs:after', function () {
        refrehDatatable();
    });

    $('#cbFiltroTipoOp').multipleSelect({
        width: '250px',
        filter: true,
        single: false,
        onClose: function () {
            listado();
        }
    });

    $('#cbFiltroTipoOp').multipleSelect('checkAll');

    $('input[name=check_estado_todos]:checkbox').change(function (e) {
        listado();
    });

    $('input[name=check_str]:checkbox').change(function (e) {
        listado();
    });

    //
    $('#btnNuevoQuery').on('click', function () {
        abriPopupNuevoQuery(null);
    });

    $('#btnNuevoPlantilla').on('click', function () {
        abriPopupNuevoPlantilla(null);
    });

    $('#btnNuevoParametro').on('click', function () {
        abriPopupNuevoParametro(null);
    });

    listado();
});


///////////////////////////
/// web 
///////////////////////////

function listado() {
    $('#listado').html('');

    ancho = $("#mainLayout").width() > 1100 ? $("#mainLayout").width() - 30 : 1100;

    var checkTodos = $('#check_estado_todos').is(':checked') ? '-1' : "1";
    var checkStr = $('#check_str').is(':checked') ? '1' : "-1";

    $.ajax({
        type: 'POST',
        url: controlador + "ListadoQuery",
        data: {
            tipoOp: getTipoOp(),
            flagStr: checkStr,
            flagActivo: checkTodos
        },
        success: function (evt) {
            if (evt.Resultado != "-1") {
                LISTA_TIPOOPERACION = evt.ListaTipoOperacion;
                LISTA_PARAMETRO = evt.ListaParametro;
                LISTA_PLANTILLA = evt.ListaPlantilla;

                $('#listado').html(evt.Resultado);
                $('#listado2').html(evt.Resultado2);
                $('#listado3').html(evt.Resultado3);

                $("#listado").css("width", (ancho) + "px");
                refrehDatatable();

                $("#listado2").css("width", (ancho) + "px");
                $("#listado3").css("width", (ancho) + "px");
            } else {
                alert("Ha ocurrido un error: " + evt.Mensaje);
            }
        },
        error: function (err) {
            alert('Ha ocurrido un error.');
        }
    });
}

function getTipoOp() {
    if ($('#cbFiltroTipoOp').multipleSelect('isAllSelected') == "S")
        return -1;

    var tipoOp = $('#cbFiltroTipoOp').multipleSelect('getSelects');
    if (tipoOp == "[object Object]" || tipoOp.length == 0) tipoOp = "-1";
    $('#hfFiltroTipoOp').val(tipoOp);
    var idValor = $('#hfFiltroTipoOp').val();

    return idValor;
}


function refrehDatatable() {
    $('#tabla_query').dataTable({
        "sPaginationType": "full_numbers",
        "destroy": "true",
        "ordering": true,
        "searching": true,
        "iDisplayLength": -1,
        "info": false,
        "paging": false,
        "scrollX": true,
        "scrollY": "400px"
    });
}

///////////////////////////
/// Query 
///////////////////////////
function abriPopupNuevoQuery(query) {
    if (query == null) {
        query = {
            Miqubacodi: 0,
            Miqplacodi: null,
            Miqubaquery: '',
            Miqubamensaje: 'Se procesó correctamente la información de ',
            Miqubaflag: 1,
            Miqubanomtabla: '',
            Miqubastr: 'N',
            Miqubaactivo: 1,
            Miqubaflagtbladicional: 'N',
            ListaParametroValor: [],
            ListaTipoOpValor: [],
            Plantilla: null
        };

        $("#popupConfiguracion .popup-title span").html("Registro de Querybase");
    } else {
        $("#popupConfiguracion .popup-title span").html("Edición de Querybase");
    }

    PLANTILLA_SELECTED = query.Plantilla;

    var htmlPlant = '<option value="-1">--SELECCIONE--</option>';
    for (var i = 0; i < LISTA_PLANTILLA.length; i++) {
        var regItem = LISTA_PLANTILLA[i];
        var strSelected = regItem.Miqplacodi == query.Miqplacodi ? "selected" : "";
        htmlPlant += `<option value="${regItem.Miqplacodi}" ${strSelected}> ${regItem.Miqplanomb} </option>`;
    }

    var htmlTipoOp = _getHtmlTipoOp(false, query.ListaTipoOpValor, false);
    var htmlParam2 = (query.Miqplacodi > 0) ? _getHtmlParamQuery(2, false, query.ListaParametroValor, false) : '';
    var htmlParam1 = _getHtmlParamQuery(1, false, query.ListaParametroValor, false);

    var checkAdicional = query.Miqubaflagtbladicional == "S" ? "checked" : "";
    var checkActivo = query.Miqubaactivo == 1 ? "checked" : "";
    var checkSgi = query.Miqubaflag == 1 ? "checked" : "";
    var checkStr = query.Miqubastr == "S" ? "checked" : "";

    var html = `
        <div>
            <INPUT type="hidden" id="id_query" value="${query.Miqubacodi}" />
            <table class="content-tabla" style="width: auto;" role="presentation">
                <tbody>
                    <tr>
                        <td COLSPAN=6 STYLE='vertical-align: top;'>                            
                            <span style="min-width: 147px; display: inline-block;">Tabla:</span>
                            <input type="text" id='nombre_tabla' value='${query.Miqubanomtabla}' />

                            <span style="padding-left: 30px;; display: inline-block;">Aplicativo STR:</span>
                            <input type="checkbox" id="check_str_query" ${checkStr} style="margin-top: 5px;">

                            <span style="padding-left: 30px;; display: inline-block;">Mostrar Log a SGI:</span>
                            <input type="checkbox" id="check_sgi_query" ${checkSgi} style="margin-top: 5px;">

                            <span style="padding-left: 30px;; display: inline-block;">Query activo:</span>
                            <input type="checkbox" id="check_activo_query" ${checkActivo} style="margin-top: 5px;">

                            <span style="padding-left: 30px;; display: inline-block;">¿Mostrar en Tab adicional?:</span>
                            <input type="checkbox" id="check_activo_adicional" ${checkAdicional} style="margin-top: 5px;">
                        </td>
                    </tr>
                    <tr>
                        <td COLSPAN=6 style='vertical-align: top;padding-bottom: 20px;'>                            
                            <span style="min-width: 147px; display: inline-block;">Mensaje Log de proceso:</span>
                            <input type="text" id="txt_mensaje_log_proceso" maxlength="200" style='width: 643px;' value='${query.Miqubamensaje}' />
                        </td>
                    </tr>

                    <tr>
                        <td COLSPAN=6 style='vertical-align: top;'>Plantilla de query:
                            <select id="id_plantilla" style='width: 667px;'>
                                ${htmlPlant}
                            </select>
                            <input type="button" value="Actualizar Query" id="btnUpdateQuery" style="width:110px" />
                        </td>
                    </tr>


                    <tr>
                        <td>Tipo de operación</td>                        
                        <td>Parámetros para crear Querybase</td>                     
                        <td>Parámetros reemplazados en tiempo de ejecución</td>
                    </tr>
                    <tr>
                        <td style='vertical-align: top;' id="td_html_tipoOp">${htmlTipoOp}</td>
                        <td style='vertical-align: top;width: 500px;' id="td_html_param2">${htmlParam2}</td>
                        <td style='vertical-align: top;' id="td_html_param1">${htmlParam1}</td>
                    </tr>

                    <tr>
                        <td colspan=6>
                            Query: <br>
                            <textarea id="txta_query" style="width: 1150px;height: 190px;">${query.Miqubaquery}</textarea>
                        </td>
                    </tr>

                    <tr>
                        <td style="height: 15px"></td>
                    </tr>
                    <tr>
                        <td>
                            <input type="submit" value="Guardar" id="btnGuardarQuery" style="width:70px" />
                        </td>
                    </tr>
                </tbody>
            </table>
        </div>
    `;

    $("#divPopupConf").html(html);

    $("#btnGuardarQuery").unbind();
    $('#btnGuardarQuery').on('click', function () {
        guardarQuery();
    });

    $('#id_plantilla').unbind();
    $('#id_plantilla').change(function () {
        getPlantilla($('#id_plantilla').val());
    });

    $("#btnUpdateQuery").unbind();
    $('#btnUpdateQuery').on('click', function () {
        updateQuery();
    });

    setTimeout(function () {
        $('#popupConfiguracion').bPopup({
            easing: 'easeOutBack',
            speed: 450,
            transition: 'slideDown',
            modalClose: true
        });
    }, 50);
}

function _getHtmlTipoOp(cargarDePlantilla, listaTipoOpValor, obligatorio) {
    var htmlTipoOp = "";
    for (var i = 0; i < LISTA_TIPOOPERACION.length; i++) {
        var regItem = LISTA_TIPOOPERACION[i];

        if (cargarDePlantilla) {
            var tieneChecked = tieneTipoOpXPlanActivo(regItem.Tmopercodi, listaTipoOpValor);
            if (tieneChecked)
                htmlTipoOp += `<input type="checkbox" name="filtro_tipoOp" value="${regItem.Tmopercodi}" checked disabled> ${regItem.Tmoperdescripcion} <br/>`;
        } else {
            //cargar de bd
            var tieneChecked = tieneTipoOpXQueryActivo(regItem.Tmopercodi, listaTipoOpValor);

            if (obligatorio) {
                if (tieneChecked)
                    htmlTipoOp += `<input type="checkbox" name="filtro_tipoOp" value="${regItem.Tmopercodi}" checked disabled> ${regItem.Tmoperdescripcion} <br/>`;
            }
            else {
                var strChecked = tieneChecked ? "checked" : "";
                htmlTipoOp += `<input type="checkbox" name="filtro_tipoOp" value="${regItem.Tmopercodi}" ${strChecked}> ${regItem.Tmoperdescripcion} <br/>`;
            }
        }
    }

    return htmlTipoOp;
}

function _getHtmlParamQuery(tipo, cargarDePlantilla, listaParamValor, obligatorio) {
    if (tipo == 1) {
        var htmlParam1 = "";
        for (var i = 0; i < LISTA_PARAMETRO.length; i++) {
            var regItem = LISTA_PARAMETRO[i];
            if (regItem.Migpartipo == 1) {

                if (cargarDePlantilla) {
                    var tieneChecked = tieneParamXPlanActivo(regItem.Migparcodi, listaParamValor);
                    if (tieneChecked)
                        htmlParam1 += `<input type="checkbox" name="filtro2_param" value="${regItem.Migparcodi}" checked disabled> ${regItem.Migparnomb} <br/>`;

                } else {
                    //cargar de bd
                    var regParamXQuery = getParamXQueryActivo(regItem.Migparcodi, listaParamValor);
                    var tieneChecked = regParamXQuery != null;

                    if (obligatorio) {
                        if (tieneChecked)
                            htmlParam1 += `<input type="checkbox" name="filtro2_param" value="${regItem.Migparcodi}" checked disabled> ${regItem.Migparnomb} <br/>`;
                    }
                    else {
                        var strChecked = tieneChecked ? "checked" : "";
                        htmlParam1 += `<input type="checkbox" name="filtro2_param" value="${regItem.Migparcodi}" ${strChecked}> ${regItem.Migparnomb} <br/>`;
                    }
                }
            }
        }

        return htmlParam1;
    } else {
        var htmlParam2 = "";
        for (var i = 0; i < LISTA_PARAMETRO.length; i++) {
            var regItem = LISTA_PARAMETRO[i];
            if (regItem.Migpartipo == 2) {

                if (cargarDePlantilla) {
                    var tieneChecked = tieneParamXPlanActivo(regItem.Migparcodi, listaParamValor);
                    if (tieneChecked)
                        htmlParam2 += `<input type="checkbox" name="filtro1_param" value="${regItem.Migparcodi}" checked disabled> 
                            <span style="min-width: 204px; display: inline-block;"> ${regItem.Migparnomb} </span>
                            <input type="text" id="valor1_param_${regItem.Migparcodi}" value=""/> <br/>`;

                } else {
                    //cargar de bd
                    var regParamXQuery = getParamXQueryActivo(regItem.Migparcodi, listaParamValor);
                    var tieneChecked = regParamXQuery != null;
                    var strChecked = tieneChecked ? "checked" : "";
                    var strvalor = tieneChecked ? regParamXQuery.Mgqparvalor : "";

                    if (obligatorio) {
                        if (tieneChecked)
                            htmlParam2 += `<input type="checkbox" name="filtro1_param" value="${regItem.Migparcodi}" checked disabled> 
                            <span style="min-width: 204px; display: inline-block;"> ${regItem.Migparnomb} </span>
                            <input type="text" id="valor1_param_${regItem.Migparcodi}" value="${strvalor}"/> <br/>`;
                    }
                    else {
                        htmlParam2 += `<input type="checkbox" name="filtro1_param" value="${regItem.Migparcodi}" ${strChecked}> 
                            <span style="min-width: 204px; display: inline-block;"> ${regItem.Migparnomb} </span>
                            <input type="text" id="valor1_param_${regItem.Migparcodi}" value="${strvalor}"/> <br/>`;

                    }

                }
            }
        }

        return htmlParam2;
    }
}

function editarQuery(id) {
    $.ajax({
        type: 'POST',
        url: controlador + 'ObtenerQuery',
        data: {
            id: id
        },
        dataType: 'json',
        cache: false,
        async: true,
        success: function (result) {
            if (result.Resultado == "1") {
                abriPopupNuevoQuery(result.Query);
            } else {
                alert("Ha ocurrido un error: " + result.Mensaje);
            }
        },
        error: function (err) {
            alert('Ha ocurrido un error');
        }
    });
}

function guardarQuery() {
    var obj = getObjQuery();
    var msjVal = val_objQuery(obj);

    if (msjVal == '') {

        $.ajax({
            type: 'POST',
            url: controlador + 'GuardarQuery',
            data: {
                strQuery: JSON.stringify(obj)
            },
            dataType: 'json',
            cache: false,
            async: true,
            success: function (result) {
                if (result.Resultado == "1") {
                    alert('La query ha sido guardada correctamente');
                    $('#popupConfiguracion').bPopup().close();
                    listado();
                } else {
                    alert("Ha ocurrido un error: " + result.Mensaje);
                }
            },
            error: function (err) {
                alert('Ha ocurrido un error');
            }
        });
    }
}

function getObjQuery() {
    var listaTipoOpValor = [];
    $("input[name=filtro_tipoOp]:checked").each(function () {
        var id = $(this).val();
        listaTipoOpValor.push({ Tmopercodi: id, Mqxtopactivo: 1 });
    });

    var listaParametroValor1 = [];
    $("input[name=filtro1_param]:checked").each(function () {
        var id = $(this).val();
        var valor = $("#valor1_param_" + id).val();
        listaParametroValor1.push({ Migparcodi: id, Mgqparactivo: 1, Mgqparvalor: valor });
    });

    var listaParametroValor2 = [];
    $("input[name=filtro2_param]:checked").each(function () {
        var id = $(this).val();
        listaParametroValor2.push({ Migparcodi: id, Mgqparactivo: 1 });
    });

    var idPlantilla = parseInt($("#id_plantilla").val()) || 0;

    var checkAdicional = $('#check_activo_adicional').is(':checked') ? "S" : "N";
    var checkActivo = $('#check_activo_query').is(':checked') ? 1 : 0;
    var checkSgi = $('#check_sgi_query').is(':checked') ? 1 : 0;
    var checkStr = $('#check_str_query').is(':checked') ? "S" : "N";

    var query = {
        Miqubacodi: $("#id_query").val(),
        Miqplacodi: idPlantilla > 0 ? idPlantilla : null,
        Miqubaquery: $("#txta_query").val(),
        Miqubamensaje: $("#txt_mensaje_log_proceso").val(),
        Miqubaflag: checkSgi,
        Miqubanomtabla: $("#nombre_tabla").val(),
        Miqubastr: checkStr,
        Miqubaactivo: checkActivo,
        Miqubaflagtbladicional: checkAdicional,
        ListaParametroValor: listaParametroValor1.concat(listaParametroValor2),
        ListaTipoOpValor: listaTipoOpValor
    };

    return query;
}

function val_objQuery(obj) {
    var msj = '';
    if (obj.Miqubaquery == '')
        msj += "Debe ingresar query";

    return msj;
}

//
function updateQuery() {
    if (PLANTILLA_SELECTED != null) {
        var strQuery = PLANTILLA_SELECTED.Miqpladesc;

        var nuevoObjQuery = getObjQuery();

        for (var i = 0; i < LISTA_PARAMETRO.length; i++) {
            var regItem = LISTA_PARAMETRO[i];
            if (regItem.Migpartipo == 2) {
                var regParamXQuery = getParamXQueryActivo(regItem.Migparcodi, nuevoObjQuery.ListaParametroValor);
                if (regParamXQuery != null) {
                    strQuery = strQuery.replaceAll(regItem.Migparnomb, regParamXQuery.Mgqparvalor);
                }
            }
        }

        $("#txta_query").val(strQuery);
    }
}

function getPlantilla(id) {
    PLANTILLA_SELECTED = null;

    if (id > 0) {
        $.ajax({
            type: 'POST',
            url: controlador + 'ObtenerPlantilla',
            data: {
                id: id
            },
            dataType: 'json',
            cache: false,
            async: true,
            success: function (result) {
                if (result.Resultado == "1") {
                    PLANTILLA_SELECTED = result.Plantilla;

                    $("#td_html_tipoOp").html(_getHtmlTipoOp(true, PLANTILLA_SELECTED.ListaTipoOp));
                    $("#td_html_param1").html(_getHtmlParamQuery(1, true, PLANTILLA_SELECTED.ListaParametro));
                    $("#td_html_param2").html(_getHtmlParamQuery(2, true, PLANTILLA_SELECTED.ListaParametro));

                    updateQuery();
                } else {
                    alert("Ha ocurrido un error: " + result.Mensaje);
                }
            },
            error: function (err) {
                alert('Ha ocurrido un error');
            }
        });
    } else {
        $("#td_html_tipoOp").html(_getHtmlTipoOp(false, [], false));
        $("#td_html_param1").html(_getHtmlParamQuery(1, false, [], false));
        $("#td_html_param2").html('');
    }
}

///////////////////////////
/// Plantilla 
///////////////////////////
function abriPopupNuevoPlantilla(plantilla) {
    if (plantilla == null) {
        plantilla = {
            Miqplacodi: 0,
            Miqplanomb: '',
            Miqpladesc: '',
            Miqplacomentario: '',
            ListaParametro: [],
            ListaTipoOp: []
        };

        $("#popupConfiguracion .popup-title span").html("Registro de plantilla");
    } else {
        $("#popupConfiguracion .popup-title span").html("Edición de plantilla");
    }

    var htmlTipoOp = "";
    for (var i = 0; i < LISTA_TIPOOPERACION.length; i++) {
        var regItem = LISTA_TIPOOPERACION[i];
        var strChecked = tieneTipoOpXPlanActivo(regItem.Tmopercodi, plantilla.ListaTipoOp) ? "checked" : "";
        htmlTipoOp += `<input type="checkbox" name="filtro_tipoOp" value="${regItem.Tmopercodi}" ${strChecked}> ${regItem.Tmoperdescripcion} <br/>`;
    }

    var htmlParam1 = "";
    for (var i = 0; i < LISTA_PARAMETRO.length; i++) {
        var regItem = LISTA_PARAMETRO[i];
        if (regItem.Migpartipo == 1) {
            var strChecked = tieneParamXPlanActivo(regItem.Migparcodi, plantilla.ListaParametro) ? "checked" : "";
            htmlParam1 += `<input type="checkbox" name="filtro_param" value="${regItem.Migparcodi}" ${strChecked}> ${regItem.Migparnomb} <br/>`;
        }
    }

    var htmlParam2 = "";
    for (var i = 0; i < LISTA_PARAMETRO.length; i++) {
        var regItem = LISTA_PARAMETRO[i];
        if (regItem.Migpartipo == 2) {
            var strChecked = tieneParamXPlanActivo(regItem.Migparcodi, plantilla.ListaParametro) ? "checked" : "";
            htmlParam2 += `<input type="checkbox" name="filtro_param" value="${regItem.Migparcodi}" ${strChecked}> ${regItem.Migparnomb} <br/>`;
        }
    }

    var html = `
        <div>
            <INPUT type="hidden" id="id_plant" value="${plantilla.Miqplacodi}" />
            <table class="content-tabla" style="width: auto;" role="presentation">
                <tbody>
                    <tr>
                        <td></td>
                        <td>Tipo de operación</td>                        
                        <td>Parámetros</td>
                    </tr>
                    <tr>
                        <td></td>
                        <td style='vertical-align: top;'>${htmlTipoOp}</td>
                        <td style='vertical-align: top;'>${htmlParam2}</td>
                        <td style='vertical-align: top;'>${htmlParam1}</td>
                    </tr>

                    <tr>
                        <td style='vertical-align: top;'>Nombre: </td>
                        <td colspan=4 style='vertical-align: top;'>
                            <input type='text' id='txt_nombre' value='${plantilla.Miqplanomb}' style='width: 700px;'/>                        
                        </td>
                    </tr>

                    <tr>
                        <td style='vertical-align: top;'>Plantilla de query: </td>
                    </tr>
                    <tr>
                        <td colspan=4>
                            <textarea id="txta_plant" style="width: 1150px;height: 190px;">${plantilla.Miqpladesc}</textarea>
                        </td>
                    </tr>

                    <tr>
                        <td style='vertical-align: top;'>Comentario: </td>
                    </tr>
                    <tr>
                        <td colspan=4>
                            <textarea id="txta_comentario" style="width: 1150px;height: 85px;">${plantilla.Miqplacomentario}</textarea>
                        </td>
                    </tr>

                    <tr>
                        <td style="height: 15px"></td>
                    </tr>
                    <tr>
                        <td>
                            <input type="submit" value="Guardar" id="btnGuardarPlantilla" style="width:70px" />
                        </td>
                    </tr>
                </tbody>
            </table>
        </div>
    `;

    $("#divPopupConf").html(html);

    $("#btnGuardarPlantilla").unbind();
    $('#btnGuardarPlantilla').on('click', function () {
        guardarPlantilla();
    });

    setTimeout(function () {
        $('#popupConfiguracion').bPopup({
            easing: 'easeOutBack',
            speed: 450,
            transition: 'slideDown',
            modalClose: true
        });
    }, 50);
}

function editarPlantilla(id) {
    $.ajax({
        type: 'POST',
        url: controlador + 'ObtenerPlantilla',
        data: {
            id: id
        },
        dataType: 'json',
        cache: false,
        async: true,
        success: function (result) {
            if (result.Resultado == "1") {
                abriPopupNuevoPlantilla(result.Plantilla);
            } else {
                alert("Ha ocurrido un error: " + result.Mensaje);
            }
        },
        error: function (err) {
            alert('Ha ocurrido un error');
        }
    });
}

function guardarPlantilla() {
    var obj = getObjPlantilla();
    var msjVal = val_objPlantilla(obj);

    if (msjVal == '') {

        $.ajax({
            type: 'POST',
            url: controlador + 'GuardarPlantilla',
            data: {
                strPlantilla: JSON.stringify(obj)
            },
            dataType: 'json',
            cache: false,
            async: true,
            success: function (result) {
                if (result.Resultado == "1") {
                    alert('La plantilla ha sido guardado correctamente');
                    $('#popupConfiguracion').bPopup().close();
                    listado();
                } else {
                    alert("Ha ocurrido un error: " + result.Mensaje);
                }
            },
            error: function (err) {
                alert('Ha ocurrido un error');
            }
        });
    }
}

function getObjPlantilla() {
    var listaParamXPlant = [];
    $("input[name=filtro_param]:checked").each(function () {
        var id = $(this).val();
        listaParamXPlant.push({ Migparcodi: id, Miplpractivo: 1 });
    });

    var listaTipoOpXPlant = [];
    $("input[name=filtro_tipoOp]:checked").each(function () {
        var id = $(this).val();
        listaTipoOpXPlant.push({ Tmopercodi: id, Miptopactivo: 1 });
    });

    var plantilla = {
        Miqplacodi: $("#id_plant").val(),
        Miqplanomb: $("#txt_nombre").val(),
        Miqpladesc: $("#txta_plant").val(),
        Miqplacomentario: $("#txta_comentario").val(),
        ListaParametro: listaParamXPlant,
        ListaTipoOp: listaTipoOpXPlant
    };

    return plantilla;
}

function val_objPlantilla(obj) {
    var msj = '';
    if (obj.Miqplanomb == '')
        msj += "Debe ingresar nombre";
    if (obj.Miqpladesc == '')
        msj += "Debe ingresar plantilla";
    if (obj.Miqpladesc == '')
        msj += "Debe ingresar comentario";

    return msj;
}

///////////////////////////
/// Parametro 
///////////////////////////
function abriPopupNuevoParametro(parametro) {
    if (parametro == null) {
        parametro = {
            Migparcodi: 0,
            Migparnomb: '',
            Migpartipo: 1,
            Migpardesc: ''
        };

        $("#popupConfiguracion .popup-title span").html("Registro de parámetro");
    } else {
        $("#popupConfiguracion .popup-title span").html("Edición de parámetro");
    }

    var check1 = parametro.Migpartipo == 1 ? "selected" : "";
    var check2 = parametro.Migpartipo == 2 ? "selected" : "";

    var html = `
        <div>
            <INPUT type="hidden" id="id_param" value="${parametro.Migparcodi}" />
            <table class="content-tabla" style="width: auto;" role="presentation">
                <tbody>
                    <tr>
                        <td>Tipo de parámetro</td>                        
                        <td>
                            <select id='tipo_param' style='width: 300px;'>
                                <option value=1 ${check1} >Se reemplaza en tiempo de ejecución</option>
                                <option value=2 ${check2} >Se reemplaza para crear querybase</option>
                            </select>
                        </td>
                    </tr>
                    <tr>
                        <td>Nombre: </td>
                        <td>
                            <input type="text" id="nomb_param" value="${parametro.Migparnomb}"/>
                        </td>
                    </tr>

                    <tr>
                        <td style="height: 15px"></td>
                    </tr>
                    <tr>
                        <td>
                            <input type="submit" value="Guardar" id="btnGuardarParametro" style="width:70px" />
                        </td>
                    </tr>
                </tbody>
            </table>
        </div>
    `;

    $("#divPopupConf").html(html);

    $("#btnGuardarParametro").unbind();
    $('#btnGuardarParametro').on('click', function () {
        guardarParametro();
    });

    setTimeout(function () {
        $('#popupConfiguracion').bPopup({
            easing: 'easeOutBack',
            speed: 450,
            transition: 'slideDown',
            modalClose: true
        });
    }, 50);
}

function editarParametro(id) {
    $.ajax({
        type: 'POST',
        url: controlador + 'ObtenerParametro',
        data: {
            id: id
        },
        dataType: 'json',
        cache: false,
        async: true,
        success: function (result) {
            if (result.Resultado == "1") {
                abriPopupNuevoParametro(result.Parametro);
            } else {
                alert("Ha ocurrido un error: " + result.Mensaje);
            }
        },
        error: function (err) {
            alert('Ha ocurrido un error');
        }
    });
}

function guardarParametro() {
    var obj = getObjParametro();
    var msjVal = val_objParametro(obj);

    if (msjVal == '') {

        $.ajax({
            type: 'POST',
            url: controlador + 'GuardarParametro',
            data: {
                strParametro: JSON.stringify(obj)
            },
            dataType: 'json',
            cache: false,
            async: true,
            success: function (result) {
                if (result.Resultado == "1") {
                    alert('El parámetro ha sido guardado correctamente');
                    $('#popupConfiguracion').bPopup().close();
                    listado();
                } else {
                    alert("Ha ocurrido un error: " + result.Mensaje);
                }
            },
            error: function (err) {
                alert('Ha ocurrido un error');
            }
        });
    }
}

function getObjParametro() {
    var parametro = {
        Migparcodi: $("#id_param").val(),
        Migparnomb: ($("#nomb_param").val()).trim(),
        Migpartipo: $("#tipo_param").val(),
    };

    return parametro;
}

function val_objParametro(obj) {
    var msj = '';
    if (obj.Migparnomb == '')
        msj += "Debe ingresar nombre";

    return msj;
}

///////
function tieneParamXPlanActivo(migparcodi, listaParamXPlant) {

    for (var i = 0; i < listaParamXPlant.length; i++) {
        var regItem = listaParamXPlant[i];
        if (regItem.Migparcodi == migparcodi && regItem.Miplpractivo == 1)
            return true;
    }

    return false;
}
function tieneTipoOpXPlanActivo(tmopercodi, listaTipoOpXPlant) {

    for (var i = 0; i < listaTipoOpXPlant.length; i++) {
        var regItem = listaTipoOpXPlant[i];
        if (regItem.Tmopercodi == tmopercodi && regItem.Miptopactivo == 1)
            return true;
    }

    return false;
}

//////
function tieneTipoOpXQueryActivo(tmopercodi, listaTipoOpXQuery) {
    return getTipoOpXQueryActivo(tmopercodi, listaTipoOpXQuery) != null;
}

function getParamXQueryActivo(migparcodi, listaParamXQuery) {

    for (var i = 0; i < listaParamXQuery.length; i++) {
        var regItem = listaParamXQuery[i];
        if (regItem.Migparcodi == migparcodi && regItem.Mgqparactivo == 1)
            return regItem;
    }

    return null;
}

function getTipoOpXQueryActivo(tmopercodi, listaTipoOpXQuery) {

    for (var i = 0; i < listaTipoOpXQuery.length; i++) {
        var regItem = listaTipoOpXQuery[i];
        if (regItem.Tmopercodi == tmopercodi && regItem.Mqxtopactivo == 1)
            return regItem;
    }

    return null;
}
