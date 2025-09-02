var controlador = siteRoot + 'Yupana/CondicionInicial/';
var ANCHO_LISTADO = 900;
var TIPO_MODOS_GENERAL = 1;
var TIPO_MODOS_RELACIONADOS = 2;
var CAL = 4;
var INY = 5;
var listaModosCOES;

var IMG_EDITAR = `<img src="${siteRoot}Content/Images/btn-edit.png" title="Editar"/>`;
var IMG_ELIMINAR = '<img src="' + siteRoot + 'Content/Images/btn-cancel.png" title="Eliminar" width="19" height="19" style="">';
var IMG_DETALLES = '<img src="' + siteRoot + 'Content/Images/btn-properties.png" title="Relacionar modos operación" width="19" height="19" style="">';

var OBJETO_DATA = {
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

    $('#tab-container').easytabs({
        animate: false
    });

    $('#tab-container').easytabs('select', '#caldero');

    $("#tab_inyeccion").on("click", function () {
        mostrarListadoRecursos(INY);
    });

    $('#btnNuevoCaldero').on('click', function () {
        mantenerRecurso(0, CAL);
    });

    $('#btnNuevoInyeccion').on('click', function () {
        mantenerRecurso(0, INY);
    });

    $('#btnRegresar').click(function () {
        document.location.href = siteRoot + 'Yupana/CondicionInicial/index';
    });

    ANCHO_LISTADO = $("#mainLayout").width() > 900 ? $("#mainLayout").width() - 5 : 900;

    mostrarListadoRecursos(CAL); // pestaña Caldero

});

function mostrarListadoRecursos(tipo) {

    limpiarBarraMensaje('mensaje');
    $.ajax({
        type: 'POST',
        url: controlador + "ListarEntidadXTipo",
        dataType: 'json',
        data: {
            tipo: tipo,
        },
        success: function (evt) {
            if (evt.Resultado != "-1") {

                var html = _dibujarTablaListado(evt, tipo);

                if (tipo == CAL) {
                    $('#listadoCaldero').html(html);
                    $(`#TablaCalderos`).dataTable({
                        bJQueryUI: true,
                        "scrollY": 430,
                        "scrollX": false,
                        "sDom": 'ft',
                        "ordering": true,
                        "destroy": "true",
                        "iDisplayLength": -1
                    });
                }
                if (tipo == INY) {
                    $('#listadoInyeccion').html(html);
                    $(`#TablaInyeccion`).dataTable({
                        bJQueryUI: true,
                        "scrollY": 430,
                        "scrollX": false,
                        "sDom": 'ft',
                        "ordering": true,
                        "destroy": "true",
                        "iDisplayLength": -1
                    });
                }

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

function _dibujarTablaListado(model, tipo) {
    idtabla = "";

    if (tipo == CAL)
        idtabla = "TablaCalderos";
    if (tipo == INY)
        idtabla = "TablaInyeccion";

    var lista = model.Recursos;

    var cadena = '';
    cadena += `
    <table class="pretty tabla-icono" border="0" cellspacing="0" id="${idtabla}" style='width:100%'>
        <thead>
            <tr>
                <th>Acciones</th>
                <th>Nombre</th>
                <th>E/S</th>
                <th>F/S</th>
                <th>Usuario modificación</th>
                <th>Fecha modificación</th>
            </tr>
        </thead>
        <tbody>
    `;

    for (key in lista) {
        var item = lista[key];

        //
        var sdisabled = "";
        var sStyle = item.EstiloEstado;

        var tdOpciones = _tdAcciones(model, item, sStyle);
        cadena += `

            <tr id="fila_${item.Rescfgcodi}">
                ${tdOpciones}
                <td style="text-align:left; ${sStyle}">${item.Rescfgnombre}</td>
                <td style="text-align:left; ${sStyle}">${item.RescfgesDesc}</td>
                <td style="text-align:left; ${sStyle}">${item.RescfgfsDesc}</td>
                <td style="text-align:left; ${sStyle}">${item.Rescfgusumodificacion}</td>
                <td style="text-align:left; ${sStyle}">${item.RescfgfecmodificacionDesc}</td>
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
        rescfgcodi = $(this).attr("id").split(",")[1];
        restipcodi = $(this).attr("id").split(",")[2];
        mantenerRecurso(rescfgcodi, restipcodi);
    });

    $('.viewEliminacion').click(function (event) {
        event.preventDefault();
        rescfgcodi = $(this).attr("id").split(",")[1];
        restipcodi = $(this).attr("id").split(",")[2];
        eliminarRecurso(rescfgcodi, restipcodi);
    });

    $('.viewRelaciones').unbind();
    $('.viewRelaciones').click(function (event) {
        event.preventDefault();
        codigoRecurso = $(this).attr("id").split(",")[1];
        mantenerRelaciones(codigoRecurso, false);
    });
};

function mantenerRecurso(rescfgcodi, tipo) {

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
                $("#popupFormulario").html(generarHtmlPoputRecurso(evt, tipo));

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
                    grabarRecurso(rescfgcodi, tipo);
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
function generarHtmlPoputRecurso(model, tipo) {

    var nombre = model.Recurso.Rescfgnombre || "";
    var valorES = model.Recurso.Rescfges != null ? model.Recurso.Rescfges : "";
    var valorFS = model.Recurso.Rescfgfs != null ? model.Recurso.Rescfgfs : "";

    var htmlTitulo = "";
    if (tipo == CAL)
        htmlTitulo += `Caldero`;
    else
        htmlTitulo += `Inyección`;

    var html = `
        <div><span class="button b-close"><span>X</span></span></div>
        <div class="popup-title">
            <span>${htmlTitulo}</span>
        </div>

        <div class="content-registro popup-text" style="height: 200px">

            <div class="content-hijo" id="mainLayoutEdit" style="padding-top:8px; padding-left: 0px; padding-right: 0px;">
            <div id="mensaje2" class="action-message">Por favor complete los datos</div>

               <table id="tablaDatos" cellpadding="5" style='width: 100%;'>
                   <tr id="trNombre">
                       <td class="registro-label">Nombre:</td>
                       <td class="registro-control">
                            <input type="text" style="width:350px" maxlength="250" id="txtNombre" value= "${nombre}" name="Rescfgnombre"/>
                       </td>
                   </tr>
                   <tr>
                     <td class="registro-label">E/S:</td>
                     <td class="registro-control">
                            <input type="text" id="txtES" name="Rescfges" maxlength="27" value="${valorES}" style="width:150px; " />
                     </td>
                  </tr>
                  <tr class="bloqueLimites">
                    <td class="registro-label">F/S:</td>
                    <td class="registro-control">
                            <input type="text" id="txtFS" name="Rescfgfs" maxlength="27" value="${valorFS}" style="width:150px; " />
                    </td>
                  </tr>
                </table>

                <div style="clear:both; width:200px; margin:auto; text-align:center; margin-top:20px">
                    <input type="button" id="btnGrabarRecurso" value="Grabar" />
                    <input type="button" id="btnCancelarRecurso" value="Cancelar" />
                </div>


           </div>
        </div>
    `;

    return html;
}

function eliminarRecurso(id, tipo) {
    if (confirm('¿Desea eliminar el recurso?')) {

        $.ajax({
            type: 'POST',
            url: controlador + 'EliminarRecurso',
            data: {
                rescfgcodi: id
            },
            dataType: 'json',
            success: function (evt) {
                if (evt.Resultado != "-1") {
                    mostrarListadoRecursos(tipo); 
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

function grabarRecurso(id, tipo) {
    var validacion = validarRecurso(tipo);

    if (validacion == "") {
        var nombre = $("#txtNombre").val();
        var rescfges = $("#txtES").val();
        var rescfgfs = $("#txtFS").val();
        var rescfgfs = $("#txtFS").val();

        $.ajax({
            type: 'POST',
            url: controlador + 'GuardarRecurso',
            data: {
                rescfgcodi: id,
                restipcodi: tipo,
                nombre: nombre,
                rescfges: rescfges,
                rescfgfs: rescfgfs
            },
            success: function (evt) {
                if (evt.Resultado != "-1") {
                    $('#popupFormulario').bPopup().close();
                    mostrarListadoRecursos(tipo);
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

function validarRecurso(tipo) {
    var html = "<ul>";
    var flag = true;

    // Expresión regular para verificar el formato del tipo de cambio con un máximo de 3 decimales
    var regex = /^\d{1,3}(\.\d{1})?$/; ///^\d+(\.\d{1,3})?$/;

    var numero = Number($('#txtNombre').val());
    if (numero != undefined && Number.isInteger(numero)) {
        html = html + "<li>Nombre: Debe ingresar un nombre correcto.</li>";
        flag = false;
    }
    else {
        if ($('#txtNombre').val() == "") {
            html = html + "<li>Nombre: Debe ingresar un nombre.</li>";
            flag = false;
        }
    }

    if ($('#txtES').val() == "" || $('#txtES').val() < 0) {
        html = html + "<li>E/S: Debe ingresar un valor mayor o igual a 0.</li>";
        flag = false;
    }
    else {
        if (!regex.test($('#txtES').val())) {
            // El valor no cumple con el formato requerido
            html = html + "<li>E/S: Debe tener un máximo de 3 números enteros y 1 decimal.</li>";
            flag = false;
        }
    }

    if ($('#txtFS').val() == "" || $('#txtFS').val() < 0) {
        html = html + "<li>F/S: Debe ingresar un valor mayor o igual a 0.</li>";
        flag = false;
    }
    else {
        if (!regex.test($('#txtFS').val())) {
            // El valor no cumple con el formato requerido
            html = html + "<li>F/S: Debe tener un máximo de 3 números enteros y 1 decimal.</li>";
            flag = false;
        }
    }

    html = html + "</ul>";

    if (flag) {
        html = "";
    }
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
    var relaciones = OBJETO_DATA.destino.DATA_INICIAL;
    var mensaje = "";

    if (mensaje == "") {
        var dataJson = {
            rescfgcodi: rescfgcodi,
            listaRelaciones: relaciones
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
                var strModosGral = JSON.stringify(result.ModosGenerales);
                var strModosXRecurso = JSON.stringify(result.Recursos);

                OBJETO_DATA.origen.DATA_INICIAL = JSON.parse(strModosGral);
                OBJETO_DATA.destino.DATA_INICIAL = JSON.parse(strModosXRecurso);

                ordenarLista(OBJETO_DATA.origen.DATA_INICIAL);
                ordenarLista(OBJETO_DATA.destino.DATA_INICIAL);

                limpiarChecksListado();
                actualizarListados();
            }
        },
        error: function (xhr, status) {
            mostrarMensaje('mensaje', 'error', 'Se ha producido un error.');
        }
    });
}

function listarCodigosChecked(tipo) {
    var selected = [];

    if (tipo == TIPO_MODOS_GENERAL) {
        $('input[type=checkbox].checkModos').each(function () {
            if ($(this).is(":checked")) {
                selected.push($(this).attr('id'));
            }
        });
    }
    else {
        $('input[type=checkbox].checkModosRel').each(function () {
            if ($(this).is(":checked")) {
                selected.push($(this).attr('id'));
            }
        });
    }

    return selected.join(";");
}

function limpiarChecksListado() {
    OBJETO_DATA.origen.DATA_SELECCIONADA = [];
    OBJETO_DATA.destino.DATA_SELECCIONADA = [];
}

function actualizarListados() {
    generarVistaTabla(TIPO_MODOS_GENERAL);
    //var pref = ObtenerPrefijoSegunTipo(tipo);
    $('.checkModos').unbind();

    $("#checkModosCab").on("click", function () {
        var check = $('#checkModosCab').is(":checked");
        $(".checkModos").prop("checked", check);
    });
    $("#tablaEquipoGen").DataTable({
        "scrollY": "320px",
        "scrollCollapse": true,
        "paging": false,
        "oLanguage": {
            "sEmptyTable": "No existen registros"
        }
    });

    generarVistaTabla(TIPO_MODOS_RELACIONADOS);
    $('.checkModosRel').unbind();

    $("#checkModosRelCab").on("click", function () {
        var check = $('#checkModosRelCab').is(":checked");
        $(".checkModosRel").prop("checked", check);
    });
    $("#tablaEquipoRel").DataTable({
        "scrollY": "320px",
        "scrollCollapse": true,
        "paging": false,
        "oLanguage": {
            "sEmptyTable": "No existen registros"
        }
    });

    $("#btnMoveRight").unbind();
    $("#btnMoveRight").on("click", function (e) {
        limpiarBarraMensaje('mensaje3')
        pasarDataDeGeneralAEstacion();
    });

    $("#btnMoveLeft").unbind();
    $("#btnMoveLeft").on("click", function (e) {
        limpiarBarraMensaje('mensaje3')
        pasarDataDeEstacionAGeneral();
    });
}

function generarVistaTabla(tipo) {

    var htmlTabla = '';
    var html = '';

    if (tipo == TIPO_MODOS_GENERAL) {
        htmlTabla = generarHtmlModosGeneral();
        html += `${htmlTabla}`;

        $("#ori_equipos").html(html);
    }
    if (tipo == TIPO_MODOS_RELACIONADOS) {
        htmlTabla = generarHtmlModosRelacionados();
        html += `${htmlTabla}`;

        $("#des_equipos").html(html);
    }
}

function generarHtmlModosGeneral() {
    var tipo = TIPO_MODOS_GENERAL;
    //var pref = ObtenerPrefijoSegunTipo(tipo);
    var listaDataInicial = OBJETO_DATA.origen.DATA_INICIAL;
    var listaDataSeleccionada = OBJETO_DATA.origen.DATA_SELECCIONADA;

    var htmlEquipo = `        
        <table class="tabla-formulario" id="tablaEquipoGen" style=''>
            <thead>
                <tr> `;
    htmlEquipo += `          
                    <th style='text-align: center;vertical-align: middle; width:30px;'>
                         <input type="checkbox" class="checkModos" name="check_equipo" id="checkModosCab" value="-1">
                    </th>`;
    htmlEquipo += `
                    <th style='width:120px;'>Tipo Equipo</th>
                    <th style='width:230px;'>Modo operación</th>                  
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
            var codigos = listarCodigosChecked(tipo);
            var idsFiltrar = codigos.split(";").map(Number);

            var elemento = listaDataSeleccionada.find(obj => obj.Rescfgcodi === rescfgcodi);
            var check = elemento != null ? "checked" : "";

            var displayCheck = "inline-block";


            htmlEquipo += `
                <tr style="cursor:pointer;"> `;
            htmlEquipo += `    

                    <td style='text-align: center;vertical-align: middle; width:30px;'>
                        <input type="checkbox" class="checkModos" name="check_equipo" id="${rescfgcodi}" value="${rescfgcodi}" ${check} style="display: ${displayCheck}" />
                    </td> `;
            htmlEquipo += `
                    <td style='width:120px;'>${tipoEquipo}</td>
                    <td style='width:230px;'>${nombre}</td>                                  
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

function generarHtmlModosRelacionados() {
    var tipo = TIPO_MODOS_RELACIONADOS;
    //var pref = ObtenerPrefijoSegunTipo(tipo);
    var listaDataInicial = OBJETO_DATA.destino.DATA_INICIAL;
    var listaDataSeleccionada = OBJETO_DATA.destino.DATA_SELECCIONADA;

    var checkTodos = "";

    var htmlEquipo = `        
        <table class="tabla-formulario" id="tablaEquipoRel" style="">
            <thead>
                <tr>`;
    htmlEquipo += ` 
                    <th style='text-align: center;vertical-align: middle; width:30px;'>
                         <input type="checkbox" class="checkModosRel" name="check_equipoRel" id="checkModosRelCab" ${checkTodos} value="-1">
                    </th>`;
    htmlEquipo += `
                    <th style='width:120px;'>Tipo Equipo</th>
                    <th style='width:230px;'>Modo operación</th>
                </tr>
            </thead>
            <tbody>
    `;

    if (listaDataInicial !== undefined && listaDataInicial != null && listaDataInicial.length > 0 ) {
        for (var i = 0; i < listaDataInicial.length; i++) {
            var reg = listaDataInicial[i];

            var rescfgcodi = reg.Rescfgcodi;
            var nombre = reg.Rescfgnombre;
            var tipoEquipo = reg.Restipnombre;

            //Obtener elementos seleccionados
            var codigos = listarCodigosChecked(tipo);
            var idsFiltrar = codigos.split(";").map(Number);

            var elemento = listaDataSeleccionada.find(obj => obj.Rescfgcodi === rescfgcodi);
            var check = elemento != null ? "checked" : "";

            var displayCheck = "inline-block";

            htmlEquipo += `
                <tr style="cursor:pointer;">   ` ;
            htmlEquipo += `
                    <td style='text-align: center;vertical-align: middle; width:30px;'>
                        <input type="checkbox" class="checkModosRel" name="check_equipo" id="${rescfgcodi}" value="${rescfgcodi}" ${check} style="display: ${displayCheck}" />
                        
                    </td>      ` ;
            htmlEquipo += `
                    <td style='width:120px;'>${tipoEquipo}</td>
                    <td style='width:230px;'>${nombre}</td>                       
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

function pasarDataDeGeneralAEstacion() {

    //Obtener elementos seleccionados
    var codigos = listarCodigosChecked(TIPO_MODOS_GENERAL);
    var idsFiltrar = codigos.split(";").map(Number);
    filtrados = OBJETO_DATA.origen.DATA_INICIAL.filter(obj => idsFiltrar.includes(obj.Rescfgcodi));
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
    actualizarListados();
}

function pasarDataDeEstacionAGeneral() {

    //Obtener elementos seleccionados
    var codigos = listarCodigosChecked(TIPO_MODOS_RELACIONADOS);
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
    actualizarListados();
}

function ordenarLista(lista) {
    lista.sort((x, y) => x.Rescfgnombre.localeCompare(y.Rescfgnombre)); // ordenamieto
}

mostrarMensaje = function (id, tipo, mensaje) {
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