var controlador = siteRoot + 'Yupana/Configuracion/';
var ANCHO_LISTADO = 900;
var MOD = 1;
var HIDRO = 2;
var RER = 3;
var listaRecursosCOES;

var IMG_EDITAR = `<img src="${siteRoot}Content/Images/btn-edit.png" title="Editar"/>`;
var IMG_ELIMINAR = '<img src="' + siteRoot + 'Content/Images/btn-cancel.png" title="Eliminar" width="19" height="19" style="">';

$(function () {

    $('#tab-container').easytabs({
        animate: false
    });

    $('#tab-container').easytabs('select', '#modOp');

    $("#tab_hidro").on("click", function () {
        mostrarListadoRecursos(HIDRO);
    });

    $("#tab_rer").on("click", function () {
        mostrarListadoRecursos(RER);
    });

    $('#btnNuevoModo').on('click', function () {
        mantenerRecurso(0, MOD);
    });

    $('#btnNuevoHidro').on('click', function () {
        mantenerRecurso(0, HIDRO);
    });

    $('#btnNuevoRer').on('click', function () {
        mantenerRecurso(0, RER);
    });

    ANCHO_LISTADO = $("#mainLayout").width() > 900 ? $("#mainLayout").width() - 5 : 900;

    mostrarListadoRecursos(MOD); // pestaña Mmodo

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

                if (tipo == MOD) {
                    $('#listadoModo').html(html);
                    $(`#TablaModos`).dataTable({
                        bJQueryUI: true,
                        "scrollY": 430,
                        "scrollX": false,
                        "sDom": 'ft',
                        "ordering": true,
                        "destroy": "true",
                        "iDisplayLength": -1
                    });
                }

                if (tipo == HIDRO) {
                    $('#listadoHidro').html(html);
                    $(`#TablaHidro`).dataTable({
                        bJQueryUI: true,
                        "scrollY": 430,
                        "scrollX": false,
                        "sDom": 'ft',
                        "ordering": true,
                        "destroy": "true",
                        "iDisplayLength": -1
                    });
                }
                if (tipo == RER) {
                    $('#listadoRer').html(html);
                    $(`#TablaRer`).dataTable({
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
    var idtabla = "";

    if (tipo == MOD)
        idtabla = "TablaModos";
    if (tipo == HIDRO)
        idtabla = "TablaHidro";
    if (tipo == RER)
        idtabla = "TablaRer";

    var lista = model.Recursos;


    var cadena = '';
    cadena += `
    <table class="pretty tabla-icono" border="0" cellspacing="0" id="${idtabla}" style='width:100%'>
        <thead>
            <tr>
                <th>Acciones</th>
                <th>Nombre</th>
                <th>Usuario modificación</th>
                <th>Fecha modificación</th>
            </tr>
        </thead>
        <tbody>
    `;

    for (var key in lista) {
        var item = lista[key];

        //
        var sdisabled = "";
        var sStyle = item.EstiloEstado;

        var tdOpciones = _tdAcciones(model, item, sStyle);

        cadena += `

            <tr id="fila_${item.Rescfgcodi}">
                ${tdOpciones}
                <td style="text-align:left; ${sStyle}">${item.Rescfgnombre}</td>
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
        `;

    return html;
}

function viewEvent() {
    $('.viewEdicion').click(function (event) {
        event.preventDefault();
        var rescfgcodi = $(this).attr("id").split(",")[1];
        var restipcodi = $(this).attr("id").split(",")[2];
        mantenerRecurso(rescfgcodi, restipcodi);
    });
    $('.viewEliminacion').click(function (event) {
        event.preventDefault();
        var rescfgcodi = $(this).attr("id").split(",")[1];
        var restipcodi = $(this).attr("id").split(",")[2];
        eliminarRecurso(rescfgcodi, restipcodi);
    });
};

function mantenerRecurso(rescfgcodi, tipo) {

    limpiarBarraMensaje('mensaje');
    $.ajax({
        type: 'POST',
        url: controlador + "ObtenerRecurso",
        dataType: 'json',
        data: {
            rescfgcodi: rescfgcodi,
            tipo: tipo
        },
        success: function (evt) {
            if (evt.Resultado != "-1") {

                //Inicializar Formulario
                if (tipo == MOD)
                    listaRecursosCOES = evt.ListaModosOpCOES;
                else {
                    if (tipo == HIDRO)
                        listaRecursosCOES = evt.ListaUnidadesCOES;
                    else
                        listaRecursosCOES = evt.ListaRerCOES;
                }

                $("#popupFormulario").html(generarHtmlPoputRecurso(evt, tipo));
                inicializarCombosFormulario(evt, tipo);

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

    var htmlTitulo = "";
    if (tipo == MOD)
        htmlTitulo += `Modo Operación`;
    else {
        if (tipo == HIDRO)
            htmlTitulo += `Unidad Hidrológica`;
        else
            htmlTitulo += `Unidad RER`;
    }

    var nombre = model.Recurso.Rescfgnombre || "";

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
                         <td  class="registro-control">
                              <select style="background-color:white" id="cbNombre" name="${nombre}">
                              </select>
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

function inicializarCombosFormulario(evt, tipo) {

    if (tipo == MOD) {
        $('#cbNombre').empty();
        var option = '<option value="-2" >-SELECCIONE-</option>';
        $.each(evt.ListaModosOpCOES, function (k, v) {
            option += '<option value =' + v.Grupocodi + '>' + v.Gruponomb + '</option>';
        })
        $('#cbNombre').append(option);

    }
    else {
        if (tipo == HIDRO) {
            $('#cbNombre').empty();
            var option = '<option value="-2" >-SELECCIONE-</option>';
            $.each(evt.ListaUnidadesCOES, function (k, v) {
                option += '<option value =' + v.Equicodi + '>' + v.Equinomb + '</option>';
            })
            $('#cbNombre').append(option);
        }
        else {
            $('#cbNombre').empty();
            var option = '<option value="-2" >-SELECCIONE-</option>';
            $.each(evt.ListaRerCOES, function (k, v) {
                option += '<option value =' + v.Grupocodi + '>' + v.Gruponomb + '</option>';
            })
            $('#cbNombre').append(option);
        }
    }

    if (evt.Recurso.Rescfgcodi > 0) {
        var codigo = evt.Recurso.ListaRelaciones[0].Resdetcodigo;
        $("#cbNombre").val(codigo);
    }
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
        var codigo = $('#cbNombre').val() || 0;

        if (tipo == MOD)
            nombre = listaRecursosCOES.find(obj => obj.Grupocodi === Number(codigo)).Grupoabrev;
        else {
            if (tipo == HIDRO)
                nombre = listaRecursosCOES.find(obj => obj.Equicodi === Number(codigo)).Equinomb;
            else
                nombre = listaRecursosCOES.find(obj => obj.Grupocodi === Number(codigo)).Gruponomb;
        }

        $.ajax({
            type: 'POST',
            url: controlador + 'GuardarRecurso',
            data: {
                rescfgcodi: id,
                restipcodi: tipo,
                codigo: codigo,
                nombre: nombre
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

    if ($('#cbNombre').val() == '-2') {
        html = html + "<li>Nombre: Debe ingresar un nombre.</li>";//Campo Requerido
        flag = false;
    }

    html = html + "</ul>";

    if (flag) {
        html = "";
    }
    return html;
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