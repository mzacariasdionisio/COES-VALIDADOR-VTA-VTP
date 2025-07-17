var controlador = siteRoot + 'PMPO/QnConfiguracion/';

var AGREGAR_EH = 1;
var EDITAR_EH = 2;
var DETALLES_EH = 3;

var DE_LISTADO = 1;
var DE_POPUP = 2;

var TIPO_PTOS_GENERAL = 1;
var TIPO_PTOS_POR_ESTACION = 2;

var PREFIJO_PTOS_GRAL = 'ptos_gral_';
var PREFIJO_PTOS_ESTACION = 'ptos_est_';

var validarCambioDePestaña = true;

var OBJETO_DATA = {
    origen: generarObjIntercambio(TIPO_PTOS_GENERAL),
    destino: generarObjIntercambio(TIPO_PTOS_POR_ESTACION),
    
};

function generarObjIntercambio(tipo) {
    var obj = {
        tipoObj: tipo,
        DATA_INICIAL: [],
        DATA_SELECCIONADA: []
    };

    return obj;
}

$(function () {

    $('#tab-container').easytabs({
        animate: false,
        select: '#vistaListado'
    });
    
    $('#tab-container').bind('easytabs:before', function () {
        var esTabDetalle = $("#tab-container .tab.active").html().toLowerCase().includes('detalle');
        var existeHtmlTabDetalle = $("#vistaDetalle").html().trim() != '';
        var esEditarCrear = parseInt($("#hfAccionDetalle").val()) != DETALLES_EH;

        if (validarCambioDePestaña) {
            if (esTabDetalle && existeHtmlTabDetalle && esEditarCrear) {
                if (confirm('¿Desea cambiar de pestaña? Si selecciona "Aceptar" se perderán los cambios. Si selecciona "Cancelar" se mantendrá en la misma pestaña')) {
                    $("#vistaDetalle").html(''); //limpiar tab Detalle                    
                } else {
                    return false;
                }
            }
        }
        validarCambioDePestaña = true;
    });

    mostrarListadoEstacionesHidrologicas();

    $('#chkReordenarEstaciones').click(function () {
        if ($(this).is(':checked')) {
            var oTable = $('#tabla_Caudal').dataTable();
            $("#tabla_Caudal .ui-sortable").sortable("enable");
            oTable.rowReordering({ sURL: controlador + "UpdateOrder" });
        } else {
            
            $("#tabla_Caudal .ui-sortable").sortable("disable");
        }
    });

    $('#btnAgregarEstacion').click(function () {
        mantenerEstacionH(AGREGAR_EH, null, DE_LISTADO);
    });

    $('#btnGenerarArchivos').click(function () {
        generarArchivos();
    });    

    $('#btnVerCodigosSddp').click(function () {
        window.open(siteRoot + 'PMPO/CodigoSDDP/Index', '_blank').focus();
    });

    
});

function mostrarListadoEstacionesHidrologicas() {

    $.ajax({
        type: 'POST',
        url: controlador + "ListarEstacionesHidrologicas",
        dataType: 'json',
        data: {},
        success: function (evt) {
            if (evt.Resultado != "-1") {

                $("#cuadroEstaciones").html(evt.HtmlListadoEstacionesH);
                refrehDatatable();
                

            } else {
                mostrarMensaje_('mensaje', 'error', 'Se ha producido un error: ' + evt.Mensaje);
            }
        },
        error: function (err) {
            alert("Ha ocurrido un error");
        }
    });
}

function refrehDatatable() {
    $('#tabla_Caudal').dataTable({
        "scrollY": 430,
        "scrollX": true,
        "sDom": 'ft',        
        "iDisplayLength": -1
    });
}

function mantenerEstacionH(accion, pmehcodi, origen) {
    $('#tab-container').easytabs('select', '#vistaDetalle');

    mostrarVistaDetalles(accion, pmehcodi);

    if (origen == DE_POPUP)
        $('#historialEH').bPopup().close();
}

function mostrarVistaDetalles(accion, pmehcodi) {
    $("#vistaDetalle").html('');
    pmehcodi = pmehcodi || 0;

    $.ajax({
        type: 'POST',
        url: controlador + "CargarDetalles",
        data: {
            accion: accion,
            pmehcodi: pmehcodi
        },
        success: function (evt) {
            $('#vistaDetalle').html(evt);
            
            llenarCamposVistaDetalle(accion);
            
            obtenerSeccionPtos(accion, pmehcodi);    

            $('#txtSddp').change(function () {
                mostrarDescripcionSddp(accion, pmehcodi);
            });

            $("#btnGrabarEstacionH").click(function () {
                guardarEstacionHidrologica(AGREGAR_EH);
            });

            $("#btnActualizarEstacionH").click(function () {
                guardarEstacionHidrologica(EDITAR_EH);
            });

            $("#btnCancelarEstacionH").click(function () {
                $("#vistaDetalle").html(''); //limpiar tab Detalle

                validarCambioDePestaña = false;
                $('#tab-container').easytabs('select', '#vistaListado');

                mostrarListadoEstacionesHidrologicas();
            });
            
        },
        error: function (err) {
            alert('Ha ocurrido un error: ' + err);
        }
    });

}

function obtenerSeccionPtos(accion, pmehcodi) {
    
    $.ajax({
        url: controlador + "ListarDataPtos",
        data: {
            accion: accion,
            pmehcodi: pmehcodi
        },
        type: 'POST',
        success: function (result) {
            if (result.Resultado === "-1") {
                mostrarMensaje_('mensaje', 'error', 'Se ha producido un error al obtener data de ptos: ' + result.Mensaje);
            } else {
                var strPtosGral = JSON.stringify(result.ListaPtosMedicion);
                var strPtosXEstacion = JSON.stringify(result.ListaPtosXEstacion);

                OBJETO_DATA.origen.DATA_INICIAL = JSON.parse(strPtosGral);
                OBJETO_DATA.destino.DATA_INICIAL = JSON.parse(strPtosXEstacion);

                limpiarChecksListado();
                actualizarListados(accion);                   
            }
        },
        error: function (xhr, status) {
            mostrarMensaje_('mensaje', 'error', 'Se ha producido un error.');
        }
    });
    
}


function actualizarListados(accion) {
    generarVistaTabla(TIPO_PTOS_GENERAL, accion);
    configurarTabla(TIPO_PTOS_GENERAL);

    generarVistaTabla(TIPO_PTOS_POR_ESTACION, accion);
    configurarTabla(TIPO_PTOS_POR_ESTACION);

    $("#btnMoveRight").unbind();
    $("#btnMoveRight").on("click", function (e) {
        pasarDataDeGeneralAEstacion(accion);
    });

    $("#btnMoveLeft").unbind();
    $("#btnMoveLeft").on("click", function (e) {
        pasarDataDeEstacionAGeneral(accion);
    });
}

function generarVistaTabla(tipo, accion) {
    
    var htmlTabla = '';
    var html = '';
    
    if (tipo == TIPO_PTOS_GENERAL) {
        htmlTabla = generarHtmlPtosGeneral(accion);
        html += `${htmlTabla}`;

        $("#ori_equipos").html(html);
    }
    if (tipo == TIPO_PTOS_POR_ESTACION) {
        htmlTabla = generarHtmlPtosXEstacion(accion);
        html += `${htmlTabla}`;

        $("#des_equipos").html(html);
    }         
}


function generarHtmlPtosGeneral(accion) {
    var tipo = TIPO_PTOS_GENERAL;
    var pref = ObtenerPrefijoSegunTipo(tipo);
    var listaDataInicial = obtenerListaDeDataInicial(tipo);
    var listaDataSeleccionada = obtenerListaDeDataSeleccionada(tipo);

    var htmlEquipo = `        
        <table class="tabla-formulario" id="${pref}tablaEquipo" style='width:100%;'>
            <thead>
                <tr> `;
    if (accion != DETALLES_EH)//no muestra checkboxs
        htmlEquipo += `          
                    <th style='text-align: center;vertical-align: middle;'>
                         <input type="checkbox" name="${pref}check_equipo" id="${pref}check_equipo_-1" value="-1">
                    </th>`;
    htmlEquipo += `
                    <th>Pto de Medición</th>   
                    <th>Formato Naturales</th>                    
                </tr>
            </thead>
            <tbody>
    `;

    if (esValidoLista(listaDataInicial)) {
        for (var i = 0; i < listaDataInicial.length; i++) {
            var reg = listaDataInicial[i];

            var ptomedicodi = reg.Ptomedicodi;
            var nombrePto = reg.NombrePto;
            var check = buscarSeleccionado(listaDataSeleccionada, tipo, ptomedicodi) != null ? "checked" : "";
            
            var displayCheck = "inline-block";


            htmlEquipo += `
                <tr style="cursor:pointer;"> `;
            if (accion != DETALLES_EH)//no muestra checkboxs
                htmlEquipo += `    

                    <td style='text-align: center;vertical-align: middle;'>
                        <input type="checkbox" name="${pref}check_equipo" id="${pref}check_equipo_${ptomedicodi}" value="${ptomedicodi}" ${check} style="display: ${displayCheck}" />                        
                    </td> `;
            htmlEquipo += `           
                    <td>${nombrePto}</td>   
                    <td>${reg.Formatnombre}</td>                                     
                </tr>
            ` ;
        }
    }

    htmlEquipo += ` 
            </tbody>
        </table>
    `;

    return htmlEquipo;
}


function generarHtmlPtosXEstacion(accion) {
    var pref = PREFIJO_PTOS_ESTACION;
    var tipo = TIPO_PTOS_POR_ESTACION;
    var listaDataInicial = obtenerListaDeDataInicial(tipo);
    var listaDataSelect = obtenerListaDeDataSeleccionada(tipo);
    var checkTodos = "";

    var htmlEquipo = `        
        <table class="tabla-formulario" id="${pref}tablaEquipo" style="width:100%">
            <thead>
                <tr>`;
    if (accion != DETALLES_EH)//no muestra checkboxs
        htmlEquipo += ` 
                    <th style='text-align: center;vertical-align: middle;'>
                         <input type="checkbox" name="${pref}check_equipo" id="${pref}check_equipo_-1" ${checkTodos} value="-1">
                    </th>`;
    htmlEquipo += `
                    <th>Pto de Medición</th>
                    <th>Descripción</th>
                    <th>Factor</th>
                    <th style="display: none;"></th>
                    <th style="display: none;"></th>
                </tr>
            </thead>
            <tbody>
    `;

    if (esValidoLista(listaDataInicial)) {
        for (var i = 0; i < listaDataInicial.length; i++) {
            var reg = listaDataInicial[i];

            var ptomedicodi = reg.Ptomedicodi;
            var descripcion = reg.Descripcion != null ? reg.Descripcion : "";
            var nombrePto = reg.NombrePto;
            var factor = reg.Factor;

            var check = buscarSeleccionado(listaDataSelect, tipo, ptomedicodi) != null ? "checked" : "";
            
            var displayCheck = "inline-block";

            htmlEquipo += `
                <tr style="cursor:pointer;">   ` ;
            if (accion != DETALLES_EH) //no muestra checkboxs
                htmlEquipo += `
                    <td style='text-align: center;vertical-align: middle;'>
                        <input type="checkbox" name="${pref}check_equipo" id="${pref}check_equipo_${ptomedicodi}" value="${ptomedicodi}" ${check} style="display: ${displayCheck}" />
                        
                    </td>      ` ;
            htmlEquipo += `
                    <td>${nombrePto}</td>                       
                    <td>${descripcion}</td>
                    <td>` ;
            if (accion != DETALLES_EH) //factor es editable
                htmlEquipo += `
                        <input id="factor_${ptomedicodi}" type="number" style=" text-align: center;"  min="-1" max="1" step="0.01" value="${factor}" onchange="actualizarFactor(${ptomedicodi}, ${i})"/>`;
            else //factor no es editable
                htmlEquipo += `
                        <input disabled id="factor_${ptomedicodi}" type="number" style=" text-align: center;"  min="-1" max="1" step="0.01" value="${factor}" onchange="actualizarFactor(${ptomedicodi}, ${i})"/>`;

            htmlEquipo += `
                    </td>
                    <td style="display: none;">
                        ${ptomedicodi}
                    </td>
                    <td style="display: none;" id="factorhd_${ptomedicodi}">
                        ${factor}
                    </td>
                </tr>
            ` ;
        }
    }

    htmlEquipo += ` 
            </tbody>
        </table>
    `;

    return htmlEquipo;
}

function ObtenerPrefijoSegunTipo(tipo) {
    switch (tipo) {
        case TIPO_PTOS_GENERAL:
            return PREFIJO_PTOS_GRAL;
            break;
        case TIPO_PTOS_POR_ESTACION:
            return PREFIJO_PTOS_ESTACION;
            break;
    }

    return '';
}

function configurarTabla(tipo) {
    var pref = ObtenerPrefijoSegunTipo(tipo);
    $('input[name=' + pref + 'check_equipo]:checkbox').unbind();
    $('input[name=' + pref + 'check_equipo]:checkbox').change(function (e) {
        actualizarDataSeleccionado(tipo, $(this).val(), $(this).prop("checked"));
    });

    $("#" + pref + "tablaEquipo").DataTable({    
        "scrollY": "320px",
        "scrollCollapse": true,
        "paging": false,
        "oLanguage": {
            "sEmptyTable": "No existen registros"
        }
    });
}

function actualizarDataSeleccionado(tipo, codigoReg, check) {
    codigoReg = parseInt(codigoReg);
    if (check) {
        agregarAListaSeleccionada(tipo, codigoReg);
    } else {
        quitarDeListaSeleccionada(tipo, codigoReg);
    }

    var prefijo = ObtenerPrefijoSegunTipo(tipo);

    //Analisis check cabecera de ambos listados   
    evaluarCheckCabecera(tipo, prefijo);    
}


function agregarAListaSeleccionada(tipo, codigoReg) {
    var pref = ObtenerPrefijoSegunTipo(tipo);
    var listaDataIni = obtenerListaDeDataInicial(tipo);
    var listaDataSeleccionado = obtenerListaDeDataSeleccionada(tipo);

    var listaFiltro = [];
    if (codigoReg > -1) { // check para un registro
        listaFiltro = listaDataSeleccionado;
        for (var i = 0; i < listaDataIni.length; i++) {
            var regIni = listaDataIni[i];
            if (regIni.Ptomedicodi == codigoReg) {
                listaFiltro.push(regIni);
            }
        }
    }
    else { //check a la cabecera
        listaFiltro = listaDataIni;

        $('input[name=' + pref + 'check_equipo]:checkbox').each(function () {
            $(this).prop('checked', true).attr('checked', 'checked');
        });
    }

    setearListaADataSeleccionada(tipo, listaFiltro);

    return listaFiltro;
}

function quitarDeListaSeleccionada(tipo, codigoReg) {
    var pref = ObtenerPrefijoSegunTipo(tipo);
    var listaDataSeleccionado = obtenerListaDeDataSeleccionada(tipo);

    var listaFiltro = [];
    if (codigoReg > -1) { // quito check para un registro
        for (var j = 0; j < listaDataSeleccionado.length; j++) {
            var regCheck = listaDataSeleccionado[j];
            if (regCheck.Ptomedicodi != codigoReg) {
                listaFiltro.push(regCheck);
            }
        }
    } else { //quito check a la cabecera
        $('input[name=' + pref + 'check_equipo]:checkbox').each(function () {
            $(this).prop('checked', false).removeAttr('checked');
        });
    }

    setearListaADataSeleccionada(tipo, listaFiltro);

    return listaFiltro;
}

function todosChecksEstanMarcados(tipo) {
    var listaDataInicial = obtenerListaDeDataInicial(tipo);
    var listaDataSeleccionado = obtenerListaDeDataSeleccionada(tipo);

    if (esValidoLista(listaDataInicial)) {
        for (var i = 0; i < listaDataInicial.length; i++) {
            var ptomedicodiini = listaDataInicial[i].Ptomedicodi;
            if (obtenerRegistroDeLista(listaDataSeleccionado, ptomedicodiini) == null) {
                return false;
            }
        }

        return true;
    }

    return false;
}

function obtenerRegistroDeLista(listaFiltro, codigo) {

    if (esValidoLista(listaFiltro)) {
        for (var i = 0; i < listaFiltro.length; i++) {
            if (listaFiltro[i].Ptomedicodi == codigo) {
                return listaFiltro[i];
            }
        }
    }

    return null;
}



function pasarDataDeGeneralAEstacion(accion) {
    //Quitar los elementos del Origen
    var listaFiltro = [];

    var listaIniEq = OBJETO_DATA.origen.DATA_INICIAL;
    var listaOrigEq = OBJETO_DATA.origen.DATA_SELECCIONADA;

    if (esValidoLista(listaIniEq) && esValidoLista(listaOrigEq)) {
        for (var i = 0; i < listaIniEq.length; i++) {
            var regIni = listaIniEq[i];
            var existeEquipo = false;
            for (var j = 0; j < listaOrigEq.length && !existeEquipo; j++) {
                var regCheck = listaOrigEq[j];                
                regCheck.Factor = 1;
                if (regIni.Ptomedicodi == regCheck.Ptomedicodi) {
                    existeEquipo = true;
                }
            }

            if (!existeEquipo) {
                listaFiltro.push(regIni);
            }
        }

        OBJETO_DATA.origen.DATA_INICIAL = listaFiltro;
        OBJETO_DATA.origen.DATA_SELECCIONADA = [];
    }

    //Mover al destino
    OBJETO_DATA.destino.DATA_INICIAL = OBJETO_DATA.destino.DATA_INICIAL.concat(listaOrigEq);
    OBJETO_DATA.destino.DATA_SELECCIONADA = OBJETO_DATA.destino.DATA_SELECCIONADA.concat(listaOrigEq);

    //Visualizar cambios 
    actualizarListados(accion);

    //Analizo check cabecera de listado Ptos Estacion
    var tipo = TIPO_PTOS_POR_ESTACION;
    var prefijo = ObtenerPrefijoSegunTipo(tipo);
    evaluarCheckCabecera(tipo, prefijo);
}

function pasarDataDeEstacionAGeneral(accion) {
    //Quitar los elementos del destino
    var listaFiltro = [];

    var listaIniEq = OBJETO_DATA.destino.DATA_INICIAL;
    var listaOrigEq = OBJETO_DATA.destino.DATA_SELECCIONADA;

    if (esValidoLista(listaIniEq) && esValidoLista(listaOrigEq)) {
        for (var i = 0; i < listaIniEq.length; i++) {
            var regIni = listaIniEq[i];
            var existeEquipo = false;
            for (var j = 0; j < listaOrigEq.length && !existeEquipo; j++) {
                var regCheck = listaOrigEq[j];
                if (regIni.Ptomedicodi == regCheck.Ptomedicodi) {
                    existeEquipo = true;
                }
            }

            if (!existeEquipo) {
                listaFiltro.push(regIni);
            }
        }

        OBJETO_DATA.destino.DATA_INICIAL = listaFiltro;
        OBJETO_DATA.destino.DATA_SELECCIONADA = [];
    }

    //Mover al destino
    OBJETO_DATA.origen.DATA_INICIAL = listaOrigEq.concat(OBJETO_DATA.origen.DATA_INICIAL);
    OBJETO_DATA.origen.DATA_SELECCIONADA = OBJETO_DATA.origen.DATA_SELECCIONADA.concat(listaOrigEq);

    //Visualizar cambios
    actualizarListados(accion);

    //Analizo check cabecera de listado Ptos Generales
    var tipo = TIPO_PTOS_GENERAL;
    var prefijo = ObtenerPrefijoSegunTipo(tipo);
    evaluarCheckCabecera(tipo, prefijo);
}

function setearListaADataSeleccionada(tipo, lista) {
    var objData = getObjetoDataByTipo(tipo);
    objData.DATA_SELECCIONADA = lista;
}

function obtenerListaDeDataSeleccionada(tipo) {
    var objData = getObjetoDataByTipo(tipo);
    return objData.DATA_SELECCIONADA;
}

function obtenerListaDeDataInicial(tipo) {
    var objData = getObjetoDataByTipo(tipo);
    return objData.DATA_INICIAL;
}

function getObjetoDataByTipo(tipo) {
    switch (tipo) {
        case TIPO_PTOS_GENERAL:
            return OBJETO_DATA.origen;
            break;
        case TIPO_PTOS_POR_ESTACION:
            return OBJETO_DATA.destino;
            break;
    }
}

function evaluarCheckCabecera(tipo, pref) {
    if (todosChecksEstanMarcados(tipo)) {
        $("#" + pref + "check_equipo_-1").prop('checked', true).attr('checked', 'checked'); //marcar check cabecera
    } else {
        $("#" + pref + "check_equipo_-1").prop('checked', false).removeAttr('checked');
    }
}

function buscarSeleccionado(listaDataInicial, tipo, codigo) {
    var listaDataSeleccionado = obtenerListaDeDataSeleccionada(tipo);

    var listaFiltro = listarElementosCheck(listaDataInicial, listaDataSeleccionado);

    return obtenerRegistroDeLista(listaFiltro, codigo);
}

function listarElementosCheck(listaDataIni, listaDataCheck) {
    var listaFiltro = [];

    if (esValidoLista(listaDataIni) && esValidoLista(listaDataCheck))
        for (var i = 0; i < listaDataIni.length; i++) {
            var regIni = listaDataIni[i];
            for (var j = 0; j < listaDataCheck.length; j++) {
                var regCheck = listaDataCheck[j];
                if (regIni.Ptomedicodi == regCheck.Ptomedicodi) {
                    listaFiltro.push(regCheck);
                    j = listaDataCheck.length;
                }
            }
        }

    return listaFiltro;
}

function esValidoLista(lista) {
    return lista !== undefined && lista != null && lista.length > 0;
}

function limpiarChecksListado() {    
    OBJETO_DATA.origen.DATA_SELECCIONADA = [];
    OBJETO_DATA.destino.DATA_SELECCIONADA = [];
}


function mostrarDescripcionSddp(accion, pmehcodi) {
    var sddpcodiSeleccionado = $("#txtSddp").val() || -1;

    $.ajax({
        url: controlador + "ObtenerDescripcion",
        data: {
            accion: accion,
            sddpcodi: sddpcodiSeleccionado,
            pmehcodi: pmehcodi
        },
        type: 'POST',
        success: function (result) {
            if (result.Resultado === "-1") {
                mostrarMensaje_('mensaje', 'error', 'Se ha producido un error al obtener descripción de la estación: ' + result.Mensaje);
            } else {
                $('#txtDescripcion').val(result.Descripcion);
                $('#hfDescripcion').val(result.Descripcion);  
            }
        },
        error: function (xhr, status) {
            mostrarMensaje_('mensaje', 'error', 'Se ha producido un error.');
        }
    });    
}

function actualizarFactor(ptomedicodi, mirow) {
    var valorFactorActual = $("#factor_" + ptomedicodi).val();    
    var valorInputFactor = '<input id="factor_' + ptomedicodi + '" type="number" style=" text-align: center;"  min="-1" max="1" step="0.01" value="' + valorFactorActual + '" onchange="actualizarFactor(' + ptomedicodi + ',' + mirow + ')" />';

    $("#" + PREFIJO_PTOS_ESTACION + "tablaEquipo").DataTable().cell({ row: mirow, column: 3 }).data(valorInputFactor);
    $("#" + PREFIJO_PTOS_ESTACION + "tablaEquipo").DataTable().cell({ row: mirow, column: 5 }).data(valorFactorActual);

    //actualiza valores de factor en DESTINO
    OBJETO_DATA.destino.DATA_INICIAL[mirow].Factor = parseFloat(valorFactorActual);
    
}

function guardarEstacionHidrologica(accion) {
    var objEstacion = {};
    objEstacion = getCamposEstacionJson();

    var mensaje = validarDatosObligatiors(objEstacion);

    if (mensaje == "") {        
        var dataJson = {
            accion: accion,
            ptomedicodi: objEstacion.ptomedicodi,
            referencia: objEstacion.referencia,
            descripcion: objEstacion.descripcion,
            integrante: objEstacion.integrante,
            listaPtosPorEstacion: objEstacion.dataPtosXEstacion
        };

        $.ajax({
            url: controlador + "RegistrarEstacionHidrologica",
            type: 'POST',
            contentType: 'application/json; charset=UTF-8',
            dataType: 'json',
            data: JSON.stringify(dataJson),
            success: function (result) {
                if (result.Resultado == "1") {
                    $("#vistaDetalle").html('');//limpiar tab Detalle
                    validarCambioDePestaña = false;
                    $('#tab-container').easytabs('select', '#vistaListado');

                    mostrarMensaje_('mensaje', 'exito', 'Estación Hidrológica registrada con éxito.');
                    mostrarListadoEstacionesHidrologicas();

                } else {
                    mostrarMensaje_('mensaje', 'error', 'Se ha producido un error: ' + result.Mensaje);
                }

            },
            error: function (xhr, status) {
                mostrarMensaje_('mensaje', 'error', 'Se ha producido un error.');
            }
        });
    }
    else {
        mostrarMensaje_('mensaje', 'error', mensaje);
    }
}


function ObtenerDataDeTabla(idTabla) {    
    var arrayTablaEstacion = [];
    var arrayPtosSddp = [];

    var table = $('#' + idTabla).DataTable();    
    var datos = table.rows().data();

    datos.map((row) => {
        arrayTablaEstacion.push(row);
    });

    arrayTablaEstacion.forEach((reg, index) => {
        var pto = {
            Ptomedicodi: reg[4],
            PtoXEstacionCodi: 0,
            NombrePto: reg[1],
            Descripcion: reg[2],
            Factor:reg[5]
        }
        arrayPtosSddp.push(pto);
    });
    return arrayPtosSddp;
}

function getCamposEstacionJson() {
    var obj = {};

    var strSddp = $("#txtSddp").val();
    var arrayValue = strSddp.split('*');
    obj.dataPtosXEstacion = [];
    
    obj.integrante = "N";
    if ($("#chkIntegrante")[0].checked) obj.integrante = "S";

    $("#hfIntegrante").val(obj.integrante);

    obj.ptomedicodi = arrayValue[0] || 0;
    obj.referencia = $("#txtReferencia").val() || "";
    obj.descripcion = $("#txtDescripcion").val() || "";  
    obj.dataPtosXEstacion = ObtenerDataDeTabla(PREFIJO_PTOS_ESTACION + "tablaEquipo");

    return obj;
}

function validarDatosObligatiors(objEstacion) {
    var msj = "";
    if (objEstacion.ptomedicodi == 0) {
        msj += "<p>Debe seleccionar un SDDP correcto.</p>";
    }

    return msj;
}

function llenarCamposVistaDetalle(accion) {
    var calChk = $("#hfIntegrante").val();
    var valorCombo = $("#hfSddp").val();

    if (accion != AGREGAR_EH) {
        if (calChk == "S") document.getElementById("chkIntegrante").checked = true;
        $("#txtSddp").val(valorCombo);
    }

    if (accion == DETALLES_EH) {
        document.getElementById("chkIntegrante").disabled = true;
        document.getElementById("txtReferencia").disabled = true;
        document.getElementById("txtDescripcion").disabled = true;
        document.getElementById("txtSddp").disabled = true;
    }
    
}

function mostrarVersiones(ptomedicodi) {
    $('#listadoHEH').html('');

    $.ajax({
        type: 'POST',
        url: controlador + "VersionListado",
        data: {
            pmhecodi: ptomedicodi
        },
        success: function (evt) {
            if (evt.Resultado != "-1") {
                $('#listadoHEH').html(evt.Resultado);

                $("#listadoHEH").css("width", (820) + "px");
                
                abrirPopup("historialEH");
            } else {

                mostrarMensaje_('mensaje', 'error', 'Ha ocurrido un error al listar las versiones :' + evt.Mensaje);
            }
        },
        error: function (err) {

            mostrarMensaje_('mensaje', 'error', 'Ha ocurrido un error.');
        }
    });
}


function eliminarEstacionH(ptomedicodi) {

    var msgConfirmacion = '¿Esta seguro que desea eliminar la estación hidrológica?'; 

    if (confirm(msgConfirmacion)) {
        $.ajax({
            url: controlador + "EliminarEstacionHidrologica",
            data: {
                pmhecodi: ptomedicodi
            },
            type: 'POST',
            success: function (result) {

                if (result.Resultado === "-1") {
                    mostrarMensaje_('mensaje', 'error', 'Se ha producido un error al eliminar la estación hidrológica: ' + result.Mensaje);
                } else {
                    mostrarListadoEstacionesHidrologicas();
                    mostrarMensaje_('mensaje', 'exito', 'Eliminación de estación hidrológica realizada con éxito.');
                }
            },
            error: function (xhr, status) {
                mostrarMensaje_('mensaje', 'error', 'Se ha producido un error.');
            }
        });
    }
}

function generarArchivos() {    
    $.ajax({
        type: 'POST',
        url: controlador + "GenerarArchivoSalida",
        data: { },
        success: function (evt) {
            if (evt.Resultado != "-1") {
                window.location = controlador + "Exportar?file_name=" + evt.Resultado;
            } else {
                mostrarMensaje_('mensaje', 'error', evt.Mensaje);
            }
        },
        error: function (err) {
            alert('Ha ocurrido un error.');
        }
    });
    
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

function sleep(ms) {
    return new Promise(resolve => setTimeout(resolve, ms));
}

async function mostrarMensaje_(id, tipo, mensaje) {
    $("#" + id).css("display", "block");    
    quitarClases(id);
    $('#' + id).addClass('action-' + tipo);
    $('#' + id).html(mensaje);

    await sleep(10000);

    limpiarBarraMensaje(id);
}

function limpiarBarraMensaje(id) {
    $('#' + id).css("display", "none");    
    quitarClases(id);
    $('#' + id).addClass('action-message');
    $('#' + id).html('');
}

function quitarClases(id) {
    document.getElementById(id).classList.remove("action-message");
    document.getElementById(id).classList.remove("action-error");
    document.getElementById(id).classList.remove("action-exito");
    document.getElementById(id).classList.remove("action-alert");
    document.getElementById(id).classList.remove("action-titulo");
    
}