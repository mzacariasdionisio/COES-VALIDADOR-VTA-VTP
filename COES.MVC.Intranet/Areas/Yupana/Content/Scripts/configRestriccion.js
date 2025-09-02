var controlador = siteRoot + 'Yupana/Restriccion/';
var ANCHO_LISTADO = 900;
var TIPO_MODOS_GENERAL = 1;
var TIPO_MODOS_RELACIONADOS = 2;
var MOD = 1;
var CAL = 2;
var INY = 3;
var listaModosCOES;

var IMG_EDITAR = `<img src="${siteRoot}Content/Images/btn-edit.png" title="Editar Restricción"/>`;
var IMG_ELIMINAR = '<img src="' + siteRoot + 'Content/Images/btn-cancel.png" title="Eliminar Restricción" width="19" height="19" style="">';
var IMG_DUPLICAR = '<img src="' + siteRoot + 'Content/Images/btn-x2.png" title="Duplicar Restricción" width="20" height="20" style="">';
var IMG_DETALLES = '<img src="' + siteRoot + 'Content/Images/btn-properties.png" title="Relacionar unidades" width="19" height="19" style="">';

var OBJETO_DATA_HIDRO = {
    origen: generarObjIntercambio(TIPO_MODOS_GENERAL),
    destino: generarObjIntercambio(TIPO_MODOS_RELACIONADOS),

};

var OBJETO_DATA_TERMO = {
    origen: generarObjIntercambio(TIPO_MODOS_GENERAL),
    destino: generarObjIntercambio(TIPO_MODOS_RELACIONADOS),

};

var OBJETO_DATA_RER = {
    origen: generarObjIntercambio(TIPO_MODOS_GENERAL),
    destino: generarObjIntercambio(TIPO_MODOS_RELACIONADOS),

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


    $('#btnNuevo').on('click', function () {
        mantenerRecurso(0);
    });

    $('#btnRegresar').click(function () {
        document.location.href = siteRoot + 'Yupana/Restriccion/index';
    });

    $('#btnImportar').click(function () {
        window.location.href = controlador + 'RestriccionImportacion';
    });

    $('#btnDescargar').click(function () {
        history.back();
    });

    ANCHO_LISTADO = $("#mainLayout").width() > 900 ? $("#mainLayout").width() - 5 : 900;

    mostrarListadoRecursos();

});

function mostrarListadoRecursos() {

    limpiarBarraMensaje('mensaje');
    $.ajax({
        type: 'POST',
        url: controlador + "ListarEntidadXTipo",
        dataType: 'json',
        data: {
        },
        success: function (evt) {
            if (evt.Resultado != "-1") {
                $('#listado').html('');
                $('#listado').css("width", ANCHO_LISTADO + "px");
                var html = _dibujarTablaListado(evt);
                $('#listado').html(html);

                $('#TablaRestricciones').dataTable({
                    "scrollY": 430,
                    "scrollX": true,
                    "sDom": 'ft',
                    "ordering": false,
                    "paging": false
                    //"iDisplayLength": 50
                });

                viewEvent();

            } else {
                alert(evt.Mensaje);
            }
        },
        error: function (err) {
            alert('Error: Se ha producido un error inesperado.');
        }
    });
}

function _dibujarTablaListado(model) {
    var lista = model.Recursos;

    var cadena = '';
    cadena += `
    <table class="pretty tabla-icono" border="0" cellspacing="0" id="TablaRestricciones" cellspacing="0" width:"100%" >
        <thead>
            <tr>
                <th style="width: 15%;">Acciones</th>
                <th style="width: 10%;">Código</th>
                <th style="width: 50%;">Nombre</th>
                <th style="width: 25%;">Tipo de restricción</th>
            </tr>
        </thead>
        <tbody>
    `;

    for (var key in lista) {
        var item = lista[key];

        var sStyle = "";
        var tdOpciones = _tdAcciones(model, item);

        cadena += `

            <tr id="fila_${item.Rescfgcodi}">
                ${tdOpciones}
                <td style="text-align:left; ${sStyle}">${item.Rescfgcodi}</td>
                <td style="text-align:left; ${sStyle}">${item.Rescfgnombre}</td>
                <td style="text-align:center; ${sStyle}">${item.Rescfgtipoecuacion}</td>
            </tr>
        `;
    }
    cadena += "</tbody></table>";

    return cadena;
}

function _tdAcciones(Model, item, sStyle) {
    var html = '';

    html += `<td style="text-align: center; ${sStyle}">`;

    html += `   
                <a href="#" id="view, ${item.Rescfgcodi},${item.Restipcodi}" class="viewEdicion">
                    ${IMG_EDITAR}
                </a>
                <a href="#" id="view,${item.Rescfgcodi},${item.Restipcodi}" class="viewEliminacion">
                    ${IMG_ELIMINAR}
                </a>
                <a href="#" id="view, ${item.Rescfgcodi}" class="viewDuplicar">
                    ${IMG_DUPLICAR}
                </a>
                <a href="#" id="view, ${item.Rescfgcodi}" class="viewRelaciones">
                    ${IMG_DETALLES}
                </a>
        `;

    return html;
}

function viewEvent() {

    $('.viewEdicion').unbind();
    $('.viewEdicion').click(function (event) {
        event.preventDefault();
        var rescfgcodi = $(this).attr("id").split(",")[1];
        var restipcodi = $(this).attr("id").split(",")[2];
        mantenerRecurso(rescfgcodi);
    });
    $('.viewEliminacion').click(function (event) {
        event.preventDefault();
        var rescfgcodi = $(this).attr("id").split(",")[1];
        var restipcodi = $(this).attr("id").split(",")[2];
        eliminarRecurso(rescfgcodi);
    });

    $('.viewDuplicar').unbind();

    $('.viewDuplicar').click(function (event) {
        event.preventDefault();
        var rescfgcodi = $(this).attr("id").split(",")[1];
        var restipcodi = $(this).attr("id").split(",")[2];
        duplicarRecurso(rescfgcodi);
    });

    $('.viewRelaciones').unbind();
    $('.viewRelaciones').click(function (event) {
        event.preventDefault();
        var rescfgcodi = $(this).attr("id").split(",")[1];
        mantenerRelaciones(rescfgcodi);
    });
};

function mantenerRecurso(rescfgcodi) {

    limpiarBarraMensaje('mensaje');
    $.ajax({
        type: 'POST',
        url: controlador + "ObtenerRecurso",
        dataType: 'json',
        data: {
            rescfgcodi: rescfgcodi
        },
        success: function (evt) {
            if (evt.Resultado != "-1") {

                //Inicializar Formulario
                $("#popupFormulario").html(generarHtmlPoputRecurso(evt));
                inicializarCombosFormulario(evt);
                $('#popupFormulario').bPopup({
                    modalClose: false,
                    easing: 'easeOutBack',
                    speed: 450,
                    transition: 'slideDown',
                    onClose: function () {
                        $('#popup').empty();
                    }
                });

                $('#btnGrabarRecurso').on("click", function () {
                    grabarRecurso(rescfgcodi);
                });

                $('#btnCancelarRecurso').on("click", function () {
                    $('#popupFormulario').bPopup().close();
                });

            } else {
                alert("Ha ocurrido un error: " + evt.Mensaje);
            }
        },
        error: function (err) {
            alert("Error al cargar Concepto");
        }
    });
}

//Generar html
function generarHtmlPoputRecurso(model) {

    var nombre = model.Recurso.Rescfgnombre || "";

    var html = `
        <div><span class="button b-close"><span>X</span></span></div>
        <div class="popup-title">
            <span>Restricción de generación</span>
        </div>

        <div class="content-registro popup-text" style="height: 200px">

            <div class="content-hijo" id="mainLayoutEdit" style="padding-top:8px; padding-left: 0px; padding-right: 0px;">
            <div id="mensaje2" class="action-message">Por favor complete los datos</div>

               <table id="tablaDatos" cellpadding="5" style='width: 100%;'>
                   <tr id="trNombre">
                       <td class="registro-label">Nombre:</td>
                       <td class="registro-control">
                            <input type="text" style="width:350px" maxlength="120" id="txtNombre" value= "${nombre}" name="Rescfgnombre"/>
                       </td>
                   </tr>

                   <tr class="">
                        <td class="registro-label" style="text-indent: initial;">Tipo de restricción:</td>
                        <td class="registro-control">
                            <select id="cbEcuacion" name="ecuacion">
                                <option value="-2" >-SELECCIONE-</option>
                                <option value="<="><b><=</b></option>
                                <option value="="><b>=</b></option>
                                <option value=">="><b>>=</b></option>
                            </select>
                        </td>
                   </tr>
                </table>

                <div style="clear:both; width:200px; margin:auto; text-align:center; margin-top:20px">
                    <input type="button" id="btnGrabarRecurso" value="Guardar" />
                    <input type="button" id="btnCancelarRecurso" value="Cancelar" />
                </div>
           </div>
        </div>
    `;

    return html;
}

function inicializarCombosFormulario(evt) {

    if (evt.Recurso.Rescfgcodi > 0) {
        var ecuacion = evt.Recurso.Rescfgtipoecuacion;
        $("#cbEcuacion").val(ecuacion);
    }
}

function eliminarRecurso(id) {
    if (confirm('¿Desea eliminar la restricción?')) {

        $.ajax({
            type: 'POST',
            url: controlador + 'EliminarRecurso',
            data: {
                rescfgcodi: id
            },
            dataType: 'json',
            success: function (evt) {
                if (evt.Resultado != "-1") {
                    mostrarListadoRecursos();
                    mostrarMensaje('mensaje', 'exito', 'El registro se eliminó correctamente.');
                }
                else {
                    mostrarMensaje('mensaje', 'error', 'Se ha producido un error.');
                }
            },
            error: function () {
                mostrarMensaje('mensaje', 'error', 'Se ha producido un error.');
            }
        });
    }
}

function grabarRecurso(id) {
    var validacion = validarRecurso(false);

    if (validacion == "") {
        var nombre = $("#txtNombre").val();
        var ecuacion = $('#cbEcuacion').val() || 0;

        $.ajax({
            type: 'POST',
            url: controlador + 'GuardarRecurso',
            data: {
                rescfgcodi: id,
                nombre: nombre,
                ecuacion: ecuacion
            },
            success: function (evt) {
                if (evt.Resultado != "-1") {
                    $('#popupFormulario').bPopup().close();
                    mostrarListadoRecursos();
                    mostrarMensaje('mensaje', 'exito', 'Los datos se guardaron correctamente.');
                }
                else {
                    alert("Ha ocurrido un error: " + evt.Mensaje);
                }
            },
            error: function () {
                mostrarMensaje('mensaje', 'error', 'Se ha producido un error.');
            }
        });
    }
    else {
        mostrarMensaje('mensaje2', 'alert', validacion);
    }
}

function validarRecurso(esDuplicar) {
    var html = "<ul>";
    var flag = true;

    if ($('#txtNombre').val() == "") {
        html = html + "<li>Nombre: Debe ingresar un nombre de restricción.</li>";
        flag = false;
    }
    else {
        var numero = Number($('#txtNombre').val());
        if (numero != undefined && Number.isInteger(numero)) {
            html = html + "<li>Nombre: Debe ingresar un nombre correcto.</li>";
            flag = false;
        }
        else {
            if ($('#txtNombre').val().Length > 120) {
                html = html + "<li>Nombre: Supera el límite de 120 caracteres.</li>";
                flag = false;
            }
        }
    }

    if (!esDuplicar) {
        if ($('#cbEcuacion').val() == '-2' || $('#cbEcuacion').val() == '') {
            html = html + "<li>Tipo de restricción: Debe seleccionar un tipo de restricción.</li>";//Campo Requerido
            flag = false;
        }
    }

    html = html + "</ul>";

    if (flag) {
        html = "";
    }
    return html;
}

//DUPLICAR
function duplicarRecurso(rescfgcodi) {

    limpiarBarraMensaje('mensaje');

    //Inicializar Formulario
    $("#popupFormulario").html(generarHtmlDuplicarRecurso());
    $('#popupFormulario').bPopup({
        modalClose: false,
        easing: 'easeOutBack',
        speed: 450,
        transition: 'slideDown',
        onClose: function () {
            $('#popup').empty();
        }
    });

    $('#btnDuplicarRecurso').on("click", function () {
        grabarDuplicarRecurso(rescfgcodi);
    });

    $('#btnCancelarRecurso').on("click", function () {
        $('#popupFormulario').bPopup().close();
    });
}

function grabarDuplicarRecurso(id) {

    var validacion = validarRecurso(true);

    if (validacion == "") {
        var nombre = $("#txtNombre").val();

        $.ajax({
            type: 'POST',
            url: controlador + 'DuplicarRecurso',
            data: {
                rescfgcodi: id,
                nombre: nombre,
            },
            success: function (evt) {
                if (evt.Resultado != "-1") {
                    $('#popupFormulario').bPopup().close();
                    mostrarListadoRecursos();
                    mostrarMensaje('mensaje', 'exito', 'Registro duplicado correctamente');
                }
                else {
                    alert("Ha ocurrido un error: " + evt.Mensaje);
                }
            },
            error: function () {
                mostrarMensaje('mensaje', 'error', 'Se ha producido un error.');
            }
        });
    }
    else {
        mostrarMensaje('mensaje2', 'alert', validacion);
    }
}

//Generar html
function generarHtmlDuplicarRecurso() {

    var html = `
        <div><span class="button b-close"><span>X</span></span></div>
        <div class="popup-title">
            <span>Duplicar restricción de generación</span>
        </div>

        <div class="content-registro popup-text" style="height: 200px">

            <div class="content-hijo" id="mainLayoutEdit" style="padding-top:8px; padding-left: 0px; padding-right: 0px;">
            <div id="mensaje2" class="action-message">Por favor complete los datos</div>

               <table id="tablaDatos" cellpadding="5" style='width: 100%;'>
                   <tr id="trNombre">
                       <td class="registro-label">Nombre:</td>
                       <td class="registro-control">
                            <input type="text" style="width:350px" maxlength="120" id="txtNombre" value= "" name="Rescfgnombre"/>
                       </td>
                   </tr>
                </table>

                <div style="clear:both; width:200px; margin:auto; text-align:center; margin-top:20px">
                    <input type="button" id="btnDuplicarRecurso" value="Duplicar" />
                    <input type="button" id="btnCancelarRecurso" value="Cancelar" />
                </div>
           </div>
        </div>
    `;

    return html;
}


//RELACIONES
function mantenerRelaciones(rescfgcodi) {
    rescfgcodi = rescfgcodi || 0;

    $.ajax({
        type: 'POST',
        url: controlador + "CargarRelaciones",
        data: {
            rescfgcodi: rescfgcodi
        },
        success: function (evt) {

            $('#contenidoRelaciones').html(evt);

            setTimeout(function () {
                $('#popupRelaciones').bPopup({
                    modalClose: false,
                    easing: 'easeOutBack',
                    speed: 450,
                    transition: 'slideDown',
                    onClose: function () {
                        $('#popup').empty();
                    }
                });
            }, 450);

            //llenarCamposVistaDetalle(accion);
            obtenerSeccionModosOp(rescfgcodi);

            $("#btnGrabarRelacion").click(function () {
                guardarRelaciones(rescfgcodi);
            });

            $("#btnCancelarRelacion").click(function () {
                $('#popupRelaciones').bPopup().close();
            });

        },
        error: function (err) {
            alert('Ha ocurrido un error: ' + err);
        }
    });
}

function guardarRelaciones(rescfgcodi) {
    var objEstacion = {};
    var relacionesHIDRO = OBJETO_DATA_HIDRO.destino.DATA_INICIAL;
    var relacionesTERMO = OBJETO_DATA_TERMO.destino.DATA_INICIAL;
    var relacionesRER = OBJETO_DATA_RER.destino.DATA_INICIAL;
    var mensaje = "";

    if (mensaje == "") {
        var dataJson = {
            rescfgcodi: rescfgcodi,
            listaRelacionesHidro: relacionesHIDRO,
            listaRelacionesTermo: relacionesTERMO,
            listaRelacionesRer: relacionesRER
        };

        $.ajax({
            url: controlador + "GuardarRelaciones",
            type: 'POST',
            contentType: 'application/json; charset=UTF-8',
            dataType: 'json',
            data: JSON.stringify(dataJson),
            success: function (result) {
                if (result.Resultado == "1") {

                    $('#popupRelaciones').bPopup().close();
                    mostrarMensaje('mensaje', 'exito', 'La relación fue registrada con éxito.');
                } else {
                    mostrarMensaje('mensaje3', 'alert', result.Mensaje);
                }
            },
            error: function (xhr, status) {
                mostrarMensaje('mensaje3', 'error', 'Se ha producido un error.');
            }
        });
    }
    else {
        mostrarMensaje('mensaje3', 'error', mensaje);
    }
}

function obtenerSeccionModosOp(rescfgcodi) {

    $.ajax({
        url: controlador + "ListarModosRelacionados",
        data: {
            rescfgcodi: rescfgcodi
        },
        type: 'POST',
        success: function (result) {
            if (result.Resultado === "-1") {
                mostrarMensaje('mensaje', 'error', 'Se ha producido un error al obtener data de modos de operación: ' + result.Mensaje);
            } else {
                var strModosGral = JSON.stringify(result.TermoGenerales);
                var strHidroGral = JSON.stringify(result.HidroGenerales);
                var strRerGral = JSON.stringify(result.RerGenerales);
                var strModosXRecurso = JSON.stringify(result.RecursosTermo);
                var strHidroXRecurso = JSON.stringify(result.RecursosHidro);
                var strRerXRecurso = JSON.stringify(result.RecursosRer);

                OBJETO_DATA_HIDRO.origen.DATA_INICIAL = JSON.parse(strHidroGral);
                OBJETO_DATA_HIDRO.destino.DATA_INICIAL = JSON.parse(strHidroXRecurso);

                OBJETO_DATA_TERMO.origen.DATA_INICIAL = JSON.parse(strModosGral);
                OBJETO_DATA_TERMO.destino.DATA_INICIAL = JSON.parse(strModosXRecurso);

                OBJETO_DATA_RER.origen.DATA_INICIAL = JSON.parse(strRerGral);
                OBJETO_DATA_RER.destino.DATA_INICIAL = JSON.parse(strRerXRecurso);

                ordenarLista(OBJETO_DATA_HIDRO.origen.DATA_INICIAL);
                ordenarLista(OBJETO_DATA_HIDRO.destino.DATA_INICIAL);

                ordenarLista(OBJETO_DATA_TERMO.origen.DATA_INICIAL);
                ordenarLista(OBJETO_DATA_TERMO.destino.DATA_INICIAL);

                ordenarLista(OBJETO_DATA_RER.origen.DATA_INICIAL);
                ordenarLista(OBJETO_DATA_RER.destino.DATA_INICIAL);

                limpiarChecksListado(OBJETO_DATA_HIDRO);
                limpiarChecksListado(OBJETO_DATA_TERMO);
                limpiarChecksListado(OBJETO_DATA_RER);

                actualizarListados(OBJETO_DATA_HIDRO, 1);
                actualizarListados(OBJETO_DATA_TERMO, 2);
                actualizarListados(OBJETO_DATA_RER, 3);
            }
        },
        error: function (xhr, status) {
            mostrarMensaje('mensaje', 'error', 'Se ha producido un error.');
        }
    });
}

function listarCodigosChecked(tipo, num) {
    var selected = [];

    if (tipo == TIPO_MODOS_GENERAL) {
        $('input[type=checkbox].checkModos' + num).each(function () {
            if ($(this).is(":checked")) {
                selected.push($(this).attr('id'));
            }
        });
    }
    else {
        $('input[type=checkbox].checkModosRel' + num).each(function () {
            if ($(this).is(":checked")) {
                selected.push($(this).attr('id'));
            }
        });
    }

    return selected.join(";");
}

function limpiarChecksListado(OBJETO_DATA) {
    OBJETO_DATA.origen.DATA_SELECCIONADA = [];
    OBJETO_DATA.destino.DATA_SELECCIONADA = [];
}

function actualizarListados(OBJETO_DATA, num) {
    generarVistaTabla(TIPO_MODOS_GENERAL, OBJETO_DATA, num);
    //var pref = ObtenerPrefijoSegunTipo(tipo);
    $('.checkModos' + num).unbind();

    $("#checkModosCab" + num).on("click", function () {
        var check = $('#checkModosCab' + num).is(":checked");
        $(".checkModos" + num).prop("checked", check);
    });
    $("#tablaEquipoGen" + num).DataTable({
        "scrollY": "120px",
        "scrollCollapse": true,
        "paging": false,
        "oLanguage": {
            "sEmptyTable": "No existen registros"
        }
    });

    generarVistaTabla(TIPO_MODOS_RELACIONADOS, OBJETO_DATA, num);
    $('.checkModosRel' + num).unbind();

    $("#checkModosRelCab" + num).on("click", function () {
        var check = $('#checkModosRelCab' + num).is(":checked");
        $(".checkModosRel" + num).prop("checked", check);
    });
    $("#tablaEquipoRel" + num).DataTable({
        "scrollY": "120px",
        "scrollCollapse": true,
        "paging": false,
        "oLanguage": {
            "sEmptyTable": "No existen registros"
        }
    });

    $("#btnMoveRight" + num).unbind();
    $("#btnMoveRight" + num).on("click", function (e) {
        limpiarBarraMensaje('mensaje3')
        pasarDataDeGeneralAEstacion(OBJETO_DATA, num);
    });

    $("#btnMoveLeft" + num).unbind();
    $("#btnMoveLeft" + num).on("click", function (e) {
        limpiarBarraMensaje('mensaje3')
        pasarDataDeEstacionAGeneral(OBJETO_DATA, num);
    });
}

function generarVistaTabla(tipo, OBJETO_DATA, num) {

    var htmlTabla = '';
    var html = '';
    var idOrigen = "#ori_equipos" + num;
    var idDestino = "#des_equipos" + num;

    if (tipo == TIPO_MODOS_GENERAL) {
        htmlTabla = generarHtmlModosGeneral(OBJETO_DATA, num);
        html += `${htmlTabla}`;

        $(idOrigen).html(html);
    }
    if (tipo == TIPO_MODOS_RELACIONADOS) {
        htmlTabla = generarHtmlModosRelacionados(OBJETO_DATA, num);
        html += `${htmlTabla}`;

        $(idDestino).html(html);
    }
}

function generarHtmlModosGeneral(OBJETO_DATA, num) {
    var tipo = TIPO_MODOS_GENERAL;
    //var pref = ObtenerPrefijoSegunTipo(tipo);
    var listaDataInicial = OBJETO_DATA.origen.DATA_INICIAL;
    var listaDataSeleccionada = OBJETO_DATA.origen.DATA_SELECCIONADA;

    var htmlEquipo = `        
        <table class="tabla-formulario" id="tablaEquipoGen${num}" style=''>
            <thead>
                <tr> `;
    htmlEquipo += `          
                    <th style='text-align: center;vertical-align: middle;'>
                         <input type="checkbox" class="checkModos${num}" name="check_equipo" id="checkModosCab${num}" value="-1">
                    </th>`;
    htmlEquipo += `
                    <th>Tipo Equipo</th>
                    <th>Modo operación</th>                  
                </tr>
            </thead>
            <tbody>
    `;

    if (listaDataInicial !== undefined && listaDataInicial != null && listaDataInicial.length > 0) {
        for (var i = 0; i < listaDataInicial.length; i++) {
            var reg = listaDataInicial[i];

            var rescfgcodi = reg.Rescfgcodi;
            var nombre = reg.Rescfgnombre;
            var tipoEquipo = reg.Restipnombre;

            //Obtener elementos seleccionados
            //var codigos = listarCodigosChecked(tipo);
            //var idsFiltrar = codigos.split(";").map(Number);

            var elemento = listaDataSeleccionada.find(obj => obj.Rescfgcodi === rescfgcodi);
            var check = elemento != null ? "checked" : "";

            var displayCheck = "inline-block";


            htmlEquipo += `
                <tr style="cursor:pointer;"> `;
            htmlEquipo += `    

                    <td style='text-align: center;vertical-align: middle;'>
                        <input type="checkbox" class="checkModos${num}" name="check_equipo" id="${rescfgcodi}" value="${rescfgcodi}" ${check} style="display: ${displayCheck}" />
                    </td> `;
            htmlEquipo += `
                    <td>${tipoEquipo}</td>  
                    <td>${nombre}</td>                                  
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

function generarHtmlModosRelacionados(OBJETO_DATA, num) {
    var tipo = TIPO_MODOS_RELACIONADOS;
    //var pref = ObtenerPrefijoSegunTipo(tipo);
    var listaDataInicial = OBJETO_DATA.destino.DATA_INICIAL;
    var listaDataSeleccionada = OBJETO_DATA.destino.DATA_SELECCIONADA;

    var checkTodos = "";

    var htmlEquipo = `        
        <table class="tabla-formulario" id="tablaEquipoRel${num}" style="">
            <thead>
                <tr>`;
    htmlEquipo += ` 
                    <th style='text-align: center;vertical-align: middle;'>
                         <input type="checkbox" class="checkModosRel${num}" name="check_equipoRel" id="checkModosRelCab${num}" ${checkTodos} value="-1">
                    </th>`;
    htmlEquipo += `
                    <th>Tipo Equipo</th>
                    <th>Modo operación</th>
                </tr>
            </thead>
            <tbody>
    `;

    if (listaDataInicial !== undefined && listaDataInicial != null && listaDataInicial.length > 0) {
        for (var i = 0; i < listaDataInicial.length; i++) {
            var reg = listaDataInicial[i];

            var rescfgcodi = reg.Rescfgcodi;
            var nombre = reg.Rescfgnombre;
            var tipoEquipo = reg.Restipnombre;

            //Obtener elementos seleccionados
            //var codigos = listarCodigosChecked(tipo);
            //var idsFiltrar = codigos.split(";").map(Number);

            var elemento = listaDataSeleccionada.find(obj => obj.Rescfgcodi === rescfgcodi);
            var check = elemento != null ? "checked" : "";

            var displayCheck = "inline-block";

            htmlEquipo += `
                <tr style="cursor:pointer;">   ` ;
            htmlEquipo += `
                    <td style='text-align: center;vertical-align: middle;'>
                        <input type="checkbox" class="checkModosRel${num}" name="check_equipo" id="${rescfgcodi}" value="${rescfgcodi}" ${check} style="display: ${displayCheck}" />
                        
                    </td>      ` ;
            htmlEquipo += `
                    <td>${tipoEquipo}</td>  
                    <td>${nombre}</td>                       
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

function pasarDataDeGeneralAEstacion(OBJETO_DATA, num) {

    //Obtener elementos seleccionados
    var codigos = listarCodigosChecked(TIPO_MODOS_GENERAL, num);
    var idsFiltrar = codigos.split(";").map(Number);
    var filtrados = OBJETO_DATA.origen.DATA_INICIAL.filter(obj => idsFiltrar.includes(obj.Rescfgcodi));
    OBJETO_DATA.origen.DATA_SELECCIONADA = filtrados;

    //Quitar los elementos del Origen
    var quitarLista = OBJETO_DATA.origen.DATA_INICIAL.filter(obj => !idsFiltrar.includes(obj.Rescfgcodi));
    OBJETO_DATA.origen.DATA_INICIAL = quitarLista;

    //Mover al destino
    OBJETO_DATA.destino.DATA_INICIAL = OBJETO_DATA.destino.DATA_INICIAL.concat(filtrados);
    OBJETO_DATA.destino.DATA_SELECCIONADA = OBJETO_DATA.destino.DATA_SELECCIONADA.concat(filtrados);

    ordenarLista(OBJETO_DATA.destino.DATA_INICIAL);
    ordenarLista(OBJETO_DATA.destino.DATA_SELECCIONADA);

    //limpiar origen selección
    OBJETO_DATA.origen.DATA_SELECCIONADA = [];

    //Visualizar cambios 
    actualizarListados(OBJETO_DATA, num);
}

function pasarDataDeEstacionAGeneral(OBJETO_DATA, num) {

    //Obtener elementos seleccionados
    var codigos = listarCodigosChecked(TIPO_MODOS_RELACIONADOS, num);
    var idsFiltrar = codigos.split(";").map(Number);
    filtrados = OBJETO_DATA.destino.DATA_INICIAL.filter(obj => idsFiltrar.includes(obj.Rescfgcodi));
    OBJETO_DATA.destino.DATA_SELECCIONADA = filtrados;

    //Quitar los elementos del destino
    var quitarLista = OBJETO_DATA.destino.DATA_INICIAL.filter(obj => !idsFiltrar.includes(obj.Rescfgcodi));
    OBJETO_DATA.destino.DATA_INICIAL = quitarLista;

    //Mover al origen
    OBJETO_DATA.origen.DATA_INICIAL = OBJETO_DATA.origen.DATA_INICIAL.concat(filtrados);
    OBJETO_DATA.origen.DATA_SELECCIONADA = OBJETO_DATA.origen.DATA_SELECCIONADA.concat(filtrados);

    ordenarLista(OBJETO_DATA.origen.DATA_INICIAL);
    ordenarLista(OBJETO_DATA.origen.DATA_SELECCIONADA);

    //limpiar origen selección
    OBJETO_DATA.destino.DATA_SELECCIONADA = [];

    //Visualizar cambios
    actualizarListados(OBJETO_DATA, num);
}

function ordenarLista(lista) {
    lista.sort((x, y) => x.Rescfgnombre.localeCompare(y.Rescfgnombre)); // ordenamieto
}

//EXPORTACIÓN
function reporteRestriccion() {

    //var famcodi = parseInt($("#cbCategoria").val());
    //var fichaTec = $("#cbFichaTecnica").val() || 0;
    //var nombre = $("#txtNombreConcepto").val() || "";
    //var estado = $("#cbEstadoConsulta").val() || -1;

    $.ajax({
        type: 'POST',
        url: controlador + 'ExportarRestriccion',
        dataType: 'json',
        data: {
        },
        success: function (evt) {
            if (evt.Resultado != "-1") {
                window.location = controlador + "AbrirArchivo?file=" + evt.NombreArchivo;
            }
            else {
                alert(evt.StrMensaje);
            }
        },
        error: function (err) {
            alert('Ha ocurrido un error de exportación');
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