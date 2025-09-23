var controlador = siteRoot + 'Yupana/GeneracionMeta/';

var ANCHO_LISTADO = 1100;
var TIPO_MODOS_GENERAL = 1;
var TIPO_MODOS_RELACIONADOS = 2;

var IMG_EDITAR = `<img src="${siteRoot}Content/Images/btn-edit.png" title="Editar Restricción"/>`;
var IMG_ELIMINAR = '<img src="' + siteRoot + 'Content/Images/btn-cancel.png" title="Eliminar Restricción" width="19" height="19" style="">';
var IMG_DUPLICAR = '<img src="' + siteRoot + 'Content/Images/btn-x2.png" title="Duplicar Restricción" width="20" height="20" style="">';
var IMG_DETALLES = '<img src="' + siteRoot + 'Content/Images/btn-properties.png" title="Relaciones" width="19" height="19" style="">';

var OBJETO_DATA_HIDRO = {
    origen: generarObjIntercambio(TIPO_MODOS_GENERAL),
    destino: generarObjIntercambio(TIPO_MODOS_RELACIONADOS),

};

var OBJETO_DATA_TERMO = {
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
        mantenerRecursoYRelaciones(0);
    });

    $('#btnRegresar').click(function () {
        history.back();
    });

    //ANCHO_LISTADO = $("#mainLayout").width() > 900 ? $("#mainLayout").width() - 5 : 900;

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
                    "scrollY": 590,
                    "scrollX": true,
                    "sDom": 'ft',
                    "ordering": false,
                    "paging": false
                    //"iDisplayLength": 50
                });

                viewEvent();

            } else {
                mostrarMensaje('mensaje', 'error', 'Error: ' + evt.Mensaje);
            }
        },
        error: function (err) {
            mostrarMensaje('mensaje', 'error', 'Se ha producido un error.');
        }
    });
}

function _dibujarTablaListado(model) {
    var lista = model.Recursos;

    var cadena = '';
    cadena += `
    <table class="pretty tabla-icono" border="0" cellspacing="0" id="TablaRestricciones" cellspacing="0"  >
        <thead>
            <tr>
                <th style="width: 15%;">Acciones</th>
                <th style="width: 10%;">Código</th>
                <th style="width: 50%;">Nombre</th>
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
                <td style="text-align:center; ${sStyle}">${item.Rescfgcodi}</td>
                <td style="text-align:left; ${sStyle}">${item.Rescfgnombre}</td>
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
                
        `;
    return html;
}

function viewEvent() {
    $('.viewEdicion').unbind();
    $('.viewEdicion').click(function (event) {
        event.preventDefault();
        var rescfgcodi = $(this).attr("id").split(",")[1];
        mantenerRecursoYRelaciones(rescfgcodi);

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

   
};

function eliminarRecurso(id) {
    if (confirm('¿Está seguro de eliminar el registro de generación meta?')) {

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


function validarRecurso(esDuplicar) {
    var html = "";
    var flag = true;

    var caja = esDuplicar ? $('#txtNombreDpc').val() : $('#txtNombre').val();

    var numero = Number(caja);
    if (numero != undefined && Number.isInteger(numero)) {
        html = "Debe ingresar un nombre correcto.";
        flag = false;
    }
    else {
        if (caja == "") {
            html = "Debe ingresar un nombre.";
            flag = false;
        }
    }



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
        var nombre = $("#txtNombreDpc").val();

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
                    /*alert("Ha ocurrido un error: " + evt.Mensaje);*/
                    mostrarMensaje('mensaje2', 'alert', "Error: " + evt.Mensaje);
                }
            },
            error: function () {
                mostrarMensaje('mensaje2', 'error', 'Se ha producido un error.');
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
            <span>Duplicar registro Generación Meta</span>
        </div>

        <div class="content-registro popup-text" style="height: 200px">

            <div class="content-hijo" id="mainLayoutEdit" style="padding-top:8px; padding-left: 0px; padding-right: 0px;">
            <div id="mensaje2" class="action-message">Por favor complete los datos</div>

               <table id="tablaDatos" cellpadding="5" style='width: 100%;'>
                   <tr id="trNombre">
                       <td class="registro-label">Nombre:</td>
                       <td class="registro-control">
                            <input type="text" style="width:350px" maxlength="120" id="txtNombreDpc" value= "" name="Rescfgnombre"/>
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

function mantenerRecursoYRelaciones(rescfgcodi) {

    rescfgcodi = rescfgcodi || 0;

    limpiarBarraMensaje('mensaje');
    $.ajax({
        type: 'POST',
        url: controlador + "CargarRelaciones",
        data: {
            rescfgcodi: rescfgcodi
        },
        success: function (evt) {
            if (evt.Resultado != "-1") {

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

                obtenerSeccionModosOp(rescfgcodi);

                $("#btnGrabarRelacion").click(function () {
                    guardarDatoYRelaciones(rescfgcodi);
                });

                $("#btnCancelarRelacion").click(function () {
                    $('#popupRelaciones').bPopup().close();
                });

            } else {
                mostrarMensaje('mensaje', 'error', "Ha ocurrido un error: " + evt.Mensaje)
            }
        },
        error: function (err) {
            mostrarMensaje('mensaje', 'error', "Error al cargar registro");
        }
    });
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

function guardarDatoYRelaciones(rescfgcodi) {
    var validacion = validarRecurso(false);

    if (validacion == "") {
        var nombre = $("#txtNombre").val();

        var objEstacion = {};
        var relacionesHIDRO = OBJETO_DATA_HIDRO.destino.DATA_INICIAL;
        var relacionesTERMO = OBJETO_DATA_TERMO.destino.DATA_INICIAL;
        var mensaje = "";

        if (mensaje == "") {
            var dataJson = {
                nombre: nombre,
                rescfgcodi: rescfgcodi,
                listaRelacionesHidro: relacionesHIDRO,
                listaRelacionesTermo: relacionesTERMO,
            };

            $.ajax({
                url: controlador + "GuardarDatoYRelaciones",
                type: 'POST',
                contentType: 'application/json; charset=UTF-8',
                dataType: 'json',
                data: JSON.stringify(dataJson),
                success: function (result) {
                    if (result.Resultado == "1") {

                        $('#popupRelaciones').bPopup().close();
                        mostrarListadoRecursos();
                        mostrarMensaje('mensaje', 'exito', 'Los datos se guardaron correctamente.');

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
    } else {
        mostrarMensaje('mensaje3', 'alert', validacion);
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
                var strModosXRecurso = JSON.stringify(result.RecursosTermo);
                var strHidroXRecurso = JSON.stringify(result.RecursosHidro);

                OBJETO_DATA_HIDRO.origen.DATA_INICIAL = JSON.parse(strHidroGral);
                OBJETO_DATA_HIDRO.destino.DATA_INICIAL = JSON.parse(strHidroXRecurso);

                OBJETO_DATA_TERMO.origen.DATA_INICIAL = JSON.parse(strModosGral);
                OBJETO_DATA_TERMO.destino.DATA_INICIAL = JSON.parse(strModosXRecurso);

                ordenarLista(OBJETO_DATA_HIDRO.origen.DATA_INICIAL);
                ordenarLista(OBJETO_DATA_HIDRO.destino.DATA_INICIAL);

                ordenarLista(OBJETO_DATA_TERMO.origen.DATA_INICIAL);
                ordenarLista(OBJETO_DATA_TERMO.destino.DATA_INICIAL);

                limpiarChecksListado(OBJETO_DATA_HIDRO);
                limpiarChecksListado(OBJETO_DATA_TERMO);

                actualizarListados(OBJETO_DATA_HIDRO, 1);
                actualizarListados(OBJETO_DATA_TERMO, 2);
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

    // Configurar evento para el checkbox de cabecera
    $("#checkModosCab" + num).on("click", function () {
        var check = $('#checkModosCab' + num).is(":checked");
        $(".checkModos" + num).prop("checked", check);
    });

    verificarCheckCabecera(num, '.checkModos', '#checkModosCab', true);


    $("#tablaEquipoGen" + num).DataTable({
        "scrollY": "140px",
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

    verificarCheckCabecera(num, '.checkModosRel', '#checkModosRelCab', true);
    $("#tablaEquipoRel" + num).DataTable({
        "scrollY": "140px",
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
        verificarCheckCabecera(num, '.checkModos', '#checkModosCab', false);
        verificarCheckCabecera(num, '.checkModosRel', '#checkModosRelCab', false);
    });

    $("#btnMoveLeft" + num).unbind();
    $("#btnMoveLeft" + num).on("click", function (e) {
        limpiarBarraMensaje('mensaje3')
        pasarDataDeEstacionAGeneral(OBJETO_DATA, num);
        verificarCheckCabecera(num, '.checkModos', '#checkModosCab', false);
        verificarCheckCabecera(num, '.checkModosRel', '#checkModosRelCab', false);
    });
}
function verificarCheckCabecera(num, claseH, claseCab, esPorUnidad) {

    if (esPorUnidad) {
        // evento para los checkboxes de equipo
        $(claseH + num).on("change", function () {
            // Verificar si algún checkbox de equipo está desmarcado
            var numMarcadpos = $(claseH + num + ':checked').length;
            var numTotales = $(claseH + num).length;

            var todosMarcados = numMarcadpos == numTotales;

            // Si no todos están marcados, desmarcar la cabecera
            if (!todosMarcados) {
                $(claseCab + num).prop("checked", false);
            } else {
                // Si todos están marcados, marcar la cabecera
                $(claseCab + num).prop("checked", true);
            }
        });
    } else {
        // Verificar si algún checkbox de equipo está desmarcado
        var numMarcadpos = $(claseH + num + ':checked').length;
        var numTotales = $(claseH + num).length;

        var todosMarcados = numMarcadpos == numTotales;

        // Si no todos están marcados, desmarcar la cabecera
        if (!todosMarcados) {
            $(claseCab + num).prop("checked", false);
        } else {
            // Si todos están marcados, marcar la cabecera
            $(claseCab + num).prop("checked", true);
        }
    }
}
function verificarCheckCabecera2(num) {
    // Nuevo: Configurar evento para los checkboxes de equipo
    $('.checkModos' + num).on("change", function () {
        // Verificar si algún checkbox de equipo está desmarcado
        var numMarcadpos = $('.checkModos' + num + ':checked').length;
        var numTotales = $('.checkModos' + num).length;

        var todosMarcados = numMarcadpos == numTotales;

        // Si no todos están marcados, desmarcar la cabecera
        if (!todosMarcados) {
            $('#checkModosCab' + num).prop("checked", false);
        } else {
            // Si todos están marcados, marcar la cabecera
            $('#checkModosCab' + num).prop("checked", true);
        }
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

    var listaDataInicial = OBJETO_DATA.origen.DATA_INICIAL;
    var listaDataSeleccionada = OBJETO_DATA.origen.DATA_SELECCIONADA;

    //el ancho se define en el css
    var htmlEquipo = `        
        <table class="tabla-formulario" id="tablaEquipoGen${num}" >
            <thead>
                <tr> `;
    htmlEquipo += `          
                    <th style='text-align: center;vertical-align: middle; width:30px;'>
                         <input type="checkbox" class="" name="check_equipo" id="checkModosCab${num}" value="-1">
                    </th>`;
    htmlEquipo += `
                    <th style='width:120px;'>Tipo Equipo</th>
                    <th style='width:220px;'>Modo operación</th>
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

            var elemento = listaDataSeleccionada.find(obj => obj.Rescfgcodi === rescfgcodi);
            var check = elemento != null ? "checked" : "";

            var displayCheck = "inline-block";


            htmlEquipo += `
                <tr style="cursor:pointer;"> `;
            htmlEquipo += `    

                    <td style='text-align: center;vertical-align: middle; width:30px;'>
                        <input type="checkbox" class="checkModos${num}" name="check_equipo" id="${rescfgcodi}" value="${rescfgcodi}" ${check} style="display: ${displayCheck}" />
                    </td> `;
            htmlEquipo += `
                    <td style='width:120px;'>${tipoEquipo}</td>
                    <td style='width:220px;'>${nombre}</td>                                  
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
        <table class="tabla-formulario" id="tablaEquipoRel${num}" style="width:380px">
            <thead>
                <tr>`;
    htmlEquipo += ` 
                    <th style='text-align: center;vertical-align: middle; width:30px;'>
                         <input type="checkbox" class="" name="check_equipoRel" id="checkModosRelCab${num}" ${checkTodos} value="-1">
                    </th>`;
    htmlEquipo += `
                    <th style='width:120px;'>Tipo Equipo</th>
                    <th style='width:220px;'>Modo operación</th>
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

            var elemento = listaDataSeleccionada.find(obj => obj.Rescfgcodi === rescfgcodi);
            var check = elemento != null ? "checked" : "";

            var displayCheck = "inline-block";

            htmlEquipo += `
                <tr style="cursor:pointer;">   ` ;
            htmlEquipo += `
                    <td style='text-align: center;vertical-align: middle; width:30px;'>
                        <input type="checkbox" class="checkModosRel${num}" name="check_equipo" id="${rescfgcodi}" value="${rescfgcodi}" ${check} style="display: ${displayCheck}" />
                        
                    </td>      ` ;
            htmlEquipo += `
                    <td style='width:120px;'>${tipoEquipo}</td>
                    <td style='width:220px;'>${nombre}</td>                       
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

    //Obtengo destino seleccionados 
    var codigosD = listarCodigosChecked(TIPO_MODOS_RELACIONADOS, num);
    var idsFiltrarD = codigosD.split(";").map(Number);
    var filtradosD = OBJETO_DATA.destino.DATA_INICIAL.filter(obj => idsFiltrarD.includes(obj.Rescfgcodi));
    OBJETO_DATA.destino.DATA_SELECCIONADA = filtradosD;

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
    //Obtengo orige n seleccionados 
    var codigosO = listarCodigosChecked(TIPO_MODOS_GENERAL, num);
    var idsFiltrarO = codigosO.split(";").map(Number);
    var filtradosO = OBJETO_DATA.origen.DATA_INICIAL.filter(obj => idsFiltrarO.includes(obj.Rescfgcodi));
    OBJETO_DATA.origen.DATA_SELECCIONADA = filtradosO;

    //Obtener elementos seleccionados
    var codigos = listarCodigosChecked(TIPO_MODOS_RELACIONADOS, num);
    var idsFiltrar = codigos.split(";").map(Number);
    var filtrados = OBJETO_DATA.destino.DATA_INICIAL.filter(obj => idsFiltrar.includes(obj.Rescfgcodi));
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