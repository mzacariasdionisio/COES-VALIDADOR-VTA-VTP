var controlador = siteRoot + 'Despacho/Concepto/';
var ANCHO_LISTADO = 900;
var CONCEPTO_WEB = null;
var CONCEPTO_GLOBAL = null;
var OPCION_VER = 1;
var OPCION_EDITAR = 2;
var OPCION_NUEVO = 3;
var OPCION_ACTUAL = 0;

var IMG_VER = `<img src="${siteRoot}Content/Images/btn-open.png" title="Ver Concepto"/>`;
var IMG_EDITAR = `<img src="${siteRoot}Content/Images/btn-edit.png" title="Editar Concepto"/>`;

$(function () {

    $('#cbCategoria').val("-2");
    $('#cbFichaTecnica').val("T");
    $("#txtNombreConcepto").blur(function () {
        $("#txtNombreConcepto").val($("#txtNombreConcepto").val().trim()); //quitar espacios
    });
    $("#txtNombreConcepto").bind("paste", function (e) {
        setTimeout(function () {
            $("#txtNombreConcepto").val($("#txtNombreConcepto").val().trim()); //quitar espacios
        }, 100);
    });
    $("#cbEstadoConsulta").val("-1");
    $('#btnBuscar').click(function () {
        mostrarListadoConceptos();
    });
    $('#btnNuevo').click(function () {
        mantenerConceptos(0, true);
    });

    $('#btnDescargar').click(function () {
        reporteConceptos();
    });

    $('#btnImportar').click(function () {
        window.location.href = controlador + 'ConceptosImportacion';
    });

    ANCHO_LISTADO = $("#mainLayout").width() > 900 ? $("#mainLayout").width() - 5 : 900;

    mostrarListadoConceptos();
});

function mostrarListadoConceptos() {
    $("#txtNombreConcepto").val($("#txtNombreConcepto").val().trim()); //quitar espacios

    var catecodi = parseInt($("#cbCategoria").val());
    var fichaTec = $("#cbFichaTecnica").val() || 0;
    var nombre = $("#txtNombreConcepto").val() || "";
    var estado = $("#cbEstadoConsulta").val() || -1;

    //limpiarBarraMensaje('mensaje');
    $.ajax({
        type: 'POST',
        url: controlador + "ListadoConceptos",
        dataType: 'json',
        data: {
            catecodi: catecodi,
            fichaTecnica: fichaTec,
            nombre: nombre,
            estado: estado
        },
        success: function (evt) {
            if (evt.Resultado != "-1") {
                $('#listado').html('');
                $('#listado').css("width", ANCHO_LISTADO + "px");
                var html = _dibujarTablaListado(evt);
                $('#listado').html(html);

                $('#TablaConsultaConceptos').dataTable({
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
    var lista = model.ListaConcepto;

    var cadena = '';
    cadena += `
    <table border="0" class="pretty tabla-icono" id="TablaConsultaConceptos" cellspacing="0" width="100%" >
        <thead>
            <tr>
                <th>Acciones</th>
                <th>Código</th>
                <th>Nombre</th>
                <th>Nombre ficha técnica</th>
                <th>Abreviatura</th>
                <th>Unidad</th>
                <th>Categoría de Grupo</th>
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

            <tr id="fila_${item.Concepcodi}">
                ${tdOpciones}
                <td style="text-align:left; ${sStyle}">${item.Concepcodi}</td>
                <td style="text-align:left; ${sStyle}">${item.Concepdesc}</td>
                <td style="text-align:left; ${sStyle}">${item.Concepnombficha}</td>
                <td style="text-align:left; ${sStyle}">${item.Concepabrev}</td>
                <td style="text-align:left; ${sStyle}">${item.Concepunid}</td>
                <td style="text-align:left; ${sStyle}">${item.Catenomb}</td>
            </tr>
        `;
    }
    cadena += "</tbody></table>";

    return cadena;
}

function _tdAcciones(Model, item, sStyle) {
    var html = '';

    html += `<td style="text-align: left; ${sStyle}">`;

    if ($("#TienePermiso").val()) {

        html += `   
                <a href="#" id="view, ${item.Concepcodi}" class="viewVer">
                    ${IMG_VER}
                </a>
                <a href="#" id="view, ${item.Concepcodi}" class="viewEdicion">
                    ${IMG_EDITAR}
                </a>
        `;
    }
    else {
        html += `<a href="#" id="view, ${item.Concepcodi}" class="viewVer">
                     ${IMG_VER}
                 </a>
        `;
    }

    return html;
}

function viewEvent() {
    $('.viewEdicion').click(function (event) {
        event.preventDefault();
        concepCodi = $(this).attr("id").split(",")[1];
        mantenerConceptos(concepCodi, true);
    });
    $('.viewVer').click(function (event) {
        event.preventDefault();
        concepCodi = $(this).attr("id").split(",")[1];
        mantenerConceptos(concepCodi, false);
    });
};

function reporteConceptos() {

    var famcodi = parseInt($("#cbCategoria").val());
    var fichaTec = $("#cbFichaTecnica").val() || 0;
    var nombre = $("#txtNombreConcepto").val() || "";
    var estado = $("#cbEstadoConsulta").val() || -1;

    $.ajax({
        type: 'POST',
        url: controlador + 'ExportarConceptos',
        dataType: 'json',
        data: {
            famcodi: famcodi,
            fichaTecnica: fichaTec,
            nombre: nombre,
            estado: estado
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


function mantenerConceptos(concepcodi, editable) {
    abrirPopupFormulario(concepcodi, editable);
}

function abrirPopupFormulario(concepcodi, editable) {
    $('#popupFormConcepto').html('');

    $.ajax({
        type: 'POST',
        url: controlador + "ObtenerConcepto",
        dataType: 'json',
        data: {
            concepcodi: concepcodi,
            editable: editable
        },
        success: function (evt) {
            if (evt.Resultado != "-1") {
                CONCEPTO_WEB = evt.Entidad;
                CONCEPTO_GLOBAL = evt;

                //....................................................
                if (evt.AccionNuevo) {
                    OPCION_ACTUAL = OPCION_NUEVO;
                } else {
                    if (evt.AccionEditar) {
                        OPCION_ACTUAL = OPCION_EDITAR;
                    }
                    else {
                        OPCION_ACTUAL = OPCION_VER;
                    }
                }

                //Inicializar Formulario
                $("#popupFormConcepto").html(generarHtmlPoputConceptoFlotante(evt));
                inicializarVistaFormulario();
                cargarEventosFormulario();

                $('#popupFormConcepto').bPopup({
                    modalClose: false,
                    easing: 'easeOutBack',
                    speed: 450,
                    transition: 'slideDown',
                    onClose: function () {
                        $('#popup').empty();
                    }
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

function cargarEventosFormulario() {
    $("#btnGuardar").unbind();
    $("#btnGuardar").click(function () {
        guardarConcepto();
    });

    $('#cbTipoDato').on("change", function () {
        mostrarLongitudTipoDato();
    });

    $("#Conceptipolong1").unbind();
    $("#Conceptipolong2").unbind();
    $("#Concepliminf").unbind();
    $("#Conceplimsup").unbind();
    $("#Conceptipolong1, #Conceptipolong2").bind('keypress', function (e) {
        var key = window.Event ? e.which : e.keyCode
        return (key <= 13 || (key >= 48 && key <= 57) || key == 46);
    });

    $("#Concepliminf, #Conceplimsup").bind('keypress', function (e) {
        var key = window.Event ? e.which : e.keyCode
        return (key <= 13 || (key >= 48 && key <= 57) || key == 46 || key == 45);
    });

    $('#cbFicha').on("change", function () {
        if ($("#cbFicha").val() == "S") {
            $(".ft_obligatorio").show();
        }
        else {
            $(".ft_obligatorio").hide();
        }
    });
}

//mostrar longitud 1 y 2
function mostrarLongitudTipoDato() {
    var tipodato = $("#cbTipoDato").val() || "";
    if (tipodato == "DECIMAL") {
        $("#tdLongitudes").show()
        document.getElementById('nombreLongitud').innerHTML = '';
        document.getElementById('nombreLongitud').innerHTML = 'Parte entera';
        //$("#nombreLongitud").val('Parte entera');
        document.getElementById("Conceptipolong1").placeholder = "##";
        $("input#Conceptipolong1").get(0).setAttribute("maxlength", 2);
        $("#divParteDecimal").show();
        mostrarBloqueLimites();
    } else {
        if (tipodato == "ENTERO") {
            $("#tdLongitudes").show()
            document.getElementById('nombreLongitud').innerHTML = '';
            document.getElementById('nombreLongitud').innerHTML = 'Parte entera';
            //$("#nombreLongitud").val('Parte entera');
            document.getElementById("Conceptipolong1").placeholder = "##";
            $("input#Conceptipolong1").get(0).setAttribute("maxlength", 2);
            $("#Conceptipolong1").show();
            $("#divParteDecimal").hide();
            mostrarBloqueLimites();
        }
        else {
            document.getElementById("Conceptipolong1").placeholder = "####";
            $("input#Conceptipolong1").get(0).setAttribute("maxlength", 4);

            if (tipodato == "CARACTER") {
                $("#tdLongitudes").show()
                document.getElementById('nombreLongitud').innerHTML = '';
                document.getElementById('nombreLongitud').innerHTML = 'Máx. caracteres';
                //$("#nombreLongitud").val('Máx. caracteres');
                $("#Conceptipolong1").show();
                $("#divParteDecimal").hide();
                ocultarBloqueLimites();
            } else {
                $("#tdLongitudes").hide();

                if (tipodato == "FORMULA")
                    mostrarBloqueLimites();
                else
                    ocultarBloqueLimites();
            }
        }
    }
}

//Generar html
function generarHtmlPoputConceptoFlotante(model) {

    var htmlTitulo = "";
    if (model.AccionNuevo) {
        htmlTitulo +=
            `Nuevo concepto`
    }
    else if (model.AccionEditar) {
        htmlTitulo += `
        Editar concepto
        `;
    }
    else {
        htmlTitulo += `
        Detalle de concepto
        `;
    }

    var htmlFormulario = _getHtmlFormulario(model);

    var html = `
        <div class="popup-title">
            <span>${htmlTitulo}</span>

            <div class="content-botonera">
                <input type="button" id="bSalir" value="Salir" class='b-close' style='float: right; margin-left: 35px;'/>
            </div>
        </div>

        <div class="ast" style="height: 610px; overflow-y: auto">

            <div class="content-hijo" id="mainLayoutEdit" style="padding-top:8px; padding-left: 0px; padding-right: 0px;" hidden>

                <div class="errorTxt"></div>
                <div id="mensaje2" class="action-alert" style="margin:0; margin-bottom:10px" hidden>Por Favor complete los datos solicitados</div>

                ${htmlFormulario}
            </div>
        </div>
    `;

    return html;
}

function _getHtmlFormulario(model) {

    var htmlFormulario = `
<input type="hidden" id="CodConcepto" value="${model.Concepcodi}" />
<input type="hidden" id="AccionNuevo" value="${model.AccionNuevo}" />
<input type="hidden" id="AccionEditar" value="${model.AccionEditar}" />

<table id="tablaDatos" cellpadding="5" style='width: 100%;'>
         <tr id="trCodigo">
             <td class="tbform-label">Código:</td>
             <td class="tbform-control">
                 <input type="text" id="Concepcodi" name="Concepcodi" value="" style="width:100px" />
             </td>
         </tr>

         <tr>
                <td class="tbform-label">Ficha técnica <span class= "obligatorio">(*)</span>:</td>
                <td class="tbform-control">
                 <select style="background-color:white" id="cbFicha" name="Entidad.Valor">
                 </select>
                </td>
         </tr>

         <tr>
             <td class="tbform-label">Nombre <span class= "obligatorio">(*)</span>:</td>
             <td class="tbform-control">
                 <input type="text" id="Concepdesc" name="Concepdesc" maxlength="250" value="" style="width:240px" />
             </td>
         </tr>
         <tr>
             <td class="tbform-label">Nombre Ficha técnica <span class= "ft_obligatorio">(*)</span>:</td>
             <td class="tbform-control">
                 <input type="text" id="Concepnombficha" name="Concepnombficha" maxlength="250" value="" style="width:240px" />
             </td>
         </tr>

         <tr>
             <td class="tbform-label">Abreviatura <span class= "obligatorio">(*)</span>:</td>
             <td class="tbform-control">
                 <input type="text" id="Concepabrev" name="Concepabrev" maxlength="20" value="" style="width:240px" />
             </td>
         </tr>
         <tr>
             <td class="tbform-label">Definición:</td>
             <td class="tbform-control">
                 <input type="text" id="Concepdefinicion" name="Concepdefinicion" maxlength="50" value="" style="width:240px" />
             </td>
         </tr>
         <tr>
             <td class="tbform-label">Categoria de grupo <span class= "obligatorio">(*)</span>:</td>
             <td  class="tbform-control">
                 <select style="background-color:white" id="cbCategoriaEdit" name="Entidad.Catecodi">
                 </select>
             </td>
         </tr>
            <tr>
                <td class="tbform-label">Unidad <span class= "obligatorio">(*)</span>:</td>
                <td class="tbform-control">
                    <input type="text" id="Concepunid" name="Concepunid" maxlength="10" value="" style="width:240px" />
                </td>
            </tr>
            <tr>
                <td class="tbform-label">Tipo de dato <span class= "obligatorio">(*)</span>:</td>
                <td class="tbform-control">
                 <select style="background-color:white" id="cbTipoDato" name="Entidad.Valor">
                 </select>
                </td>

                <td id="tdLongitudes" colspan="2">
                      <div style="float:left; margin-right:20px">
                          <div class="tbform-label"><span id="nombreLongitud">Parte entera</span> </div>
                          <div>
                              <input type="text" id="Conceptipolong1" name="Conceptipolong1" placeholder="##" maxlength="2" value="" style="width:60px" />
                          </div>
                      </div>
                      <div id="divParteDecimal" style="float:left">
                          <div class="tbform-label">Parte Decimal </div>
                          <div>
                              <input type="text" id="Conceptipolong2" name="Conceptipolong2" placeholder="##" maxlength="2" value="" style="width:60px" />
                          </div>
                      </div>
                </td>
            </tr>
            <tr class="bloqueLimites">
                 <td class="tbform-label">Límite Inferior:</td>
                 <td class="tbform-control">
                     <input type="text" id="Concepliminf" name="Concepliminf" maxlength="27" value="" style="width:240px; " />
                 </td>
            </tr>

            <tr class="bloqueLimites">
                 <td class="tbform-label">Límite Superior:</td>
                 <td class="tbform-control">
                     <input type="text" id="Conceplimsup" name="Conceplimsup" maxlength="27" value="" style="width:240px; " />
                 </td>
            </tr>
            <tr>
                <td class="tbform-label">Ocultar comentario Ficha Técnica Portal Web:</td>
                <td class="tbform-control">
                    <div>
                        <input type="checkbox" value="" id="ChkOcultarComent">
                    </div>
                </td>
            </tr>
            <tr>
                <td class="tbform-label">Mostrar en color verde claro celda FT:</td>
                <td class="tbform-control">
                    <div>
                        <input type="checkbox" value="" id="ChkMostrarColor">
                    </div>
                </td>
            </tr>
            <tr>
                <td class="tbform-label">Activo:</td>
                <td class="tbform-control">
                    <div>
                        <input type="checkbox" value="" id="ChkActivo">
                    </div>
                </td>
            </tr>
            <tr>
                <td class="tbform-label">Detalle por unidades de generación:</td>
                <td class="tbform-control">
                    <div>
                        <input type="checkbox" value="" id="ChkDetalleProp">
                    </div>
                </td>
            </tr>

            <tr id="trFechCreacion">
                <td class="tbform-label">Fecha creación:</td>
                <td class="tbform-control">
                    <input type="text" id="Concepfeccreacion" name="Concepfeccreacion"  value="" style="width:240px" />
                </td>
            </tr>
            <tr id="trUsuCreacion">
                <td class="tbform-label">Usuario creación:</td>
                <td class="tbform-control">
                    <input type="text" id="Concepusucreacion" name="Concepusucreacion"  value="" style="width:240px" />
                </td>
            </tr>
            <tr id="trUsuModif">
                <td class="tbform-label">Usuario modificación:</td>
                <td class="tbform-control">
                    <input type="text" id="Concepusumodificacion" name="Concepusumodificacion" value="" style="width:240px" />
                </td>
            </tr>
            <tr id="trFechModif">
                <td class="tbform-label">Fecha modificación:</td>
                <td class="tbform-control">
                    <input type="text" id="Concepfecmodificacion" name="Concepfecmodificacion" value="" style="width:240px" />
                </td>
            </tr>

            <tr>
                <td class="tbform-label"><span id="spanNota">(*) Campos obligatorios</span></td>
            </tr>

        <tr></tr>

</table>

<div style="clear:both; width:100px; text-align:center; margin:auto; margin-top:20px">
    <input type="button" value="" id="btnGuardar" />
</div>
    `;

    return htmlFormulario;
}

//inicializa vista formulario
function inicializarVistaFormulario() {
    switch (OPCION_ACTUAL) {
        case OPCION_VER:
            $("#mainLayoutEdit").show();
            $("#Concepcodi").prop('disabled', 'disabled');
            $("#Concepdesc").prop('disabled', 'disabled');
            $("#Concepnombficha").prop('disabled', 'disabled');
            $("#Concepabrev").prop('disabled', 'disabled');
            $("#Concepdefinicion").prop('disabled', 'disabled');
            $("#Concepunid").prop('disabled', 'disabled');
            $("#Conceptipolong1").prop('disabled', 'disabled');
            $("#Conceptipolong2").prop('disabled', 'disabled');
            $("#Concepliminf").prop('disabled', 'disabled');
            $("#Conceplimsup").prop('disabled', 'disabled');

            $("#cbCategoriaEdit").prop('disabled', 'disabled');
            document.getElementById('cbCategoriaEdit').style.backgroundColor = '';
            $("#cbTipoDato").prop('disabled', 'disabled');
            document.getElementById('cbTipoDato').style.backgroundColor = '';
            $("#cbFicha").prop('disabled', 'disabled');
            document.getElementById('cbFicha').style.backgroundColor = '';
            $("#btnGuardar").hide();

            $("#ChkActivo").prop('disabled', 'disabled');
            $("#ChkOcultarComent").prop('disabled', 'disabled');
            $("#ChkMostrarColor").prop('disabled', 'disabled');
            $("#ChkDetalleProp").prop('disabled', 'disabled');
            $("#ChkActivo").prop('disabled', 'disabled');
            $("#Concepfeccreacion").prop('disabled', 'disabled');
            $("#Concepusucreacion").prop('disabled', 'disabled');
            $("#Concepusumodificacion").prop('disabled', 'disabled');
            $("#Concepfecmodificacion").prop('disabled', 'disabled');

            //Listas desplegables
            inicializaDesplegables();
            $(".obligatorio").hide();
            $(".ft_obligatorio").hide();
            $("#spanNota").hide();

            //LLENAR CAMPOS
            $("#cbCategoriaEdit").val(CONCEPTO_WEB.Catecodi);
            $("#cbTipoDato").val(CONCEPTO_WEB.Conceptipo);
            $("#cbFicha").val(CONCEPTO_WEB.Concepfichatec);

            $("#Concepcodi").val(CONCEPTO_WEB.Concepcodi);
            $("#Concepdesc").val(CONCEPTO_WEB.Concepdesc);
            $("#Concepnombficha").val(CONCEPTO_WEB.Concepnombficha);
            $("#Concepabrev").val(CONCEPTO_WEB.Concepabrev);
            $("#Concepdefinicion").val(CONCEPTO_WEB.Concepdefinicion);
            $("#Concepunid").val(CONCEPTO_WEB.Concepunid);

            mostrarLongitudTipoDato();
            $("#Conceptipolong1").val(CONCEPTO_WEB.Conceptipolong1);
            $("#Conceptipolong2").val(CONCEPTO_WEB.Conceptipolong2);
            $("#Concepliminf").val(CONCEPTO_WEB.StrConcepliminf);
            $("#Conceplimsup").val(CONCEPTO_WEB.StrConceplimsup);

            //check
            if (CONCEPTO_WEB.Concepactivo == "1") {
                $("#ChkActivo").prop('checked', true);
            }

            if (CONCEPTO_WEB.Conceppropeq == 1) {
                $("#ChkDetalleProp").prop('checked', true);
            }

            if (CONCEPTO_WEB.Concepocultocomentario == "S") {
                $("#ChkOcultarComent").prop('checked', true);
            }

            if (CONCEPTO_WEB.Concepflagcolor == 1) {
                $("#ChkMostrarColor").prop('checked', true);
            }

            $("#Concepfeccreacion").val(CONCEPTO_WEB.ConcepfeccreacionDesc);
            $("#Concepusucreacion").val(CONCEPTO_WEB.Concepusucreacion);
            $("#Concepusumodificacion").val(CONCEPTO_WEB.Concepusumodificacion);
            $("#Concepfecmodificacion").val(CONCEPTO_WEB.ConcepfecmodificacionDesc);

            break;
        case OPCION_EDITAR:
            $("#mainLayoutEdit").show();
            $("#btnGuardar").show();
            $("#btnGuardar").val('Actualizar');
            $("#Concepcodi").prop('disabled', 'disabled');

            $("#trFechCreacion").hide();
            $("#trUsuCreacion").hide();
            $("#trUsuModif").hide();
            $("#trFechModif").hide();

            document.getElementById('Concepdesc').style.backgroundColor = 'white';
            document.getElementById('Concepnombficha').style.backgroundColor = 'white';
            document.getElementById('Concepabrev').style.backgroundColor = 'white';
            document.getElementById('Concepdefinicion').style.backgroundColor = 'white';
            document.getElementById('Concepunid').style.backgroundColor = 'white';
            document.getElementById('Conceptipolong1').style.backgroundColor = 'white';
            document.getElementById('Conceptipolong2').style.backgroundColor = 'white';
            document.getElementById('Concepliminf').style.backgroundColor = 'white';
            document.getElementById('Conceplimsup').style.backgroundColor = 'white';

            $("#cbCategoriaEdit").removeAttr('disabled');
            document.getElementById('cbCategoriaEdit').style.backgroundColor = 'white';
            $("#cbTipoDato").removeAttr('disabled');
            document.getElementById('cbTipoDato').style.backgroundColor = 'white';
            $("#cbFicha").removeAttr('disabled');
            document.getElementById('cbFicha').style.backgroundColor = 'white';

            //Listas desplegables
            inicializaDesplegables();
            $(".obligatorio").show();
            $(".ft_obligatorio").show();
            $("#spanNota").show();

            //LLENAR CAMPOS
            $("#cbCategoriaEdit").val(CONCEPTO_WEB.Catecodi);
            $("#cbTipoDato").val(CONCEPTO_WEB.Conceptipo);
            $("#cbFicha").val(CONCEPTO_WEB.Concepfichatec);
            //$("#cbPadre").val(CONCEPTO_WEB.Propcodipadre);
            if ($("#cbFicha").val() == "S") {
                $(".ft_obligatorio").show();
            }
            else {
                $(".ft_obligatorio").hide();
            }

            $("#Concepcodi").val(CONCEPTO_WEB.Concepcodi);
            $("#Concepdesc").val(CONCEPTO_WEB.Concepdesc);
            $("#Concepnombficha").val(CONCEPTO_WEB.Concepnombficha);
            $("#Concepabrev").val(CONCEPTO_WEB.Concepabrev);
            $("#Concepdefinicion").val(CONCEPTO_WEB.Concepdefinicion);
            $("#Concepunid").val(CONCEPTO_WEB.Concepunid);

            mostrarLongitudTipoDato();
            $("#Conceptipolong1").val(CONCEPTO_WEB.Conceptipolong1);
            $("#Conceptipolong2").val(CONCEPTO_WEB.Conceptipolong2);
            $("#Concepliminf").val(CONCEPTO_WEB.StrConcepliminf);
            $("#Conceplimsup").val(CONCEPTO_WEB.StrConceplimsup);

            //check
            if (CONCEPTO_WEB.Concepactivo == "1") {
                $("#ChkActivo").prop('checked', true);
            }

            if (CONCEPTO_WEB.Conceppropeq == 1) {
                $("#ChkDetalleProp").prop('checked', true);
            }

            if (CONCEPTO_WEB.Concepocultocomentario == "S") {
                $("#ChkOcultarComent").prop('checked', true);
            }

            if (CONCEPTO_WEB.Concepflagcolor == 1) {
                $("#ChkMostrarColor").prop('checked', true);
            }

            break;
        case OPCION_NUEVO:
            $("#mainLayoutEdit").show();
            $("#btnGuardar").show();
            $("#btnGuardar").val('Registrar');

            $("#trCodigo").hide();
            $("#trFechCreacion").hide();
            $("#trUsuCreacion").hide();
            $("#trUsuModif").hide();
            $("#trFechModif").hide();

            document.getElementById('Concepdesc').style.backgroundColor = 'white';
            document.getElementById('Concepnombficha').style.backgroundColor = 'white';
            document.getElementById('Concepabrev').style.backgroundColor = 'white';
            document.getElementById('Concepdefinicion').style.backgroundColor = 'white';
            document.getElementById('Concepunid').style.backgroundColor = 'white';
            document.getElementById('Conceptipolong1').style.backgroundColor = 'white';
            document.getElementById('Conceptipolong2').style.backgroundColor = 'white';
            document.getElementById('Concepliminf').style.backgroundColor = 'white';
            document.getElementById('Conceplimsup').style.backgroundColor = 'white';

            //Listas desplegables
            inicializaDesplegables();
            $(".obligatorio").show();
            $(".ft_obligatorio").show();
            $("#spanNota").show();

            if ($("#cbFicha").val() == "S") {
                $(".ft_obligatorio").show();
            }
            else {
                $(".ft_obligatorio").hide();
            }

            //.................LLENAR CAMPOS....................
            $("#ChkActivo").prop('checked', true);

            $("#ChkDetalleProp").prop('checked', false);
            $("#ChkOcultarComent").prop('checked', false);

            break;
    }
}

//Inicializa Listas Desplegables
function inicializaDesplegables() {

    $('#cbCategoriaEdit').empty();
    var optionTipoEquipo = '<option value="-2" >-SELECCIONE-</option>';
    //var optionTipoEquipo = '';
    $.each(CONCEPTO_GLOBAL.ListaCategoria, function (k, v) {
        optionTipoEquipo += '<option value =' + v.Catecodi + '>' + v.Catenomb + '</option>';
    })
    $('#cbCategoriaEdit').append(optionTipoEquipo);

    $('#cbTipoDato').empty();
    //var optionTipoDato = '<option value="-1" >----- Seleccione una Tipo de dato ----- </option>';
    var optionTipoDato = '';
    $.each(CONCEPTO_GLOBAL.ListaTipoDato, function (k, v) {
        optionTipoDato += '<option value =' + v.Valor + '>' + v.Descripcion + '</option>';
    })
    $('#cbTipoDato').append(optionTipoDato);

    $('#cbFicha').empty();
    //var optionFicha = '<option value="" >----- Seleccione una ficha ----- </option>';
    var optionFichaTec = '';
    $.each(CONCEPTO_GLOBAL.ListaFichatecnica, function (k, v) {
        optionFichaTec += '<option value =' + v.Valor + '>' + v.Descripcion + '</option>';
    })
    $('#cbFicha').append(optionFichaTec);

}

//guardar Concepto
function guardarConcepto() {

    actualizarConceptoJson();
    //if (confirm('¿Desea guardar ?')) {
    //}
    var msj = validarConcepto(CONCEPTO_WEB);
    if (msj == "") {
        limpiarMensaje();
        //$("#btnGuardar").hide();

        var dataJson = JSON.stringify(CONCEPTO_WEB).replace(/\/Date/g, "\\\/Date").replace(/\)\//g, "\)\\\/");
        $.ajax({
            type: 'POST',
            dataType: 'json',
            traditional: true,
            url: controlador + "ConceptoGuardar",
            data: {
                dataJson: dataJson,
                opcion: OPCION_ACTUAL
            },
            success: function (result) {
                if (result.Resultado != '-1' && result.IdConcepto > 0) {
                    $('#popupFormConcepto').bPopup().close();
                    alert(result.StrMensaje);
                    mostrarListadoConceptos();
                }
                else {
                    alert(result.StrMensaje);
                    //$("#mensaje").addClass("action-error");
                    //$("#mensaje").html("Hubo un error al guardar los datos");
                }
                setTimeout(function () { $("#mensaje").fadeOut(800).fadeIn(800).fadeOut(500).fadeIn(500).fadeOut(300); }, 3000);
            },
            error: function (err) {
                alert('Ha ocurrido un error.');
            }
        });
    } else {
        mostrarAlerta(msj);
    }
}

//Json para nuevo Concepto
function actualizarConceptoJson() {

    //$("#descrip").val(validarTextoSinSaltoLinea($("#descrip").val()));
    if (OPCION_ACTUAL == OPCION_NUEVO) {

        CONCEPTO_WEB.Catecodi = $("#cbCategoriaEdit").val();
        CONCEPTO_WEB.Conceptipo = $("#cbTipoDato").val();
        CONCEPTO_WEB.Concepfichatec = $("#cbFicha").val();
        //CONCEPTO_WEB.Propcodipadre = $("#cbPadre").val();

        CONCEPTO_WEB.Concepcodi = $("#Concepcodi").val() || 0;
        CONCEPTO_WEB.Concepdesc = $("#Concepdesc").val();
        CONCEPTO_WEB.Concepnombficha = $("#Concepnombficha").val();
        CONCEPTO_WEB.Concepabrev = $("#Concepabrev").val();
        CONCEPTO_WEB.Concepdefinicion = $("#Concepdefinicion").val();
        CONCEPTO_WEB.Concepunid = $("#Concepunid").val();

        var tipodato = CONCEPTO_WEB.Conceptipo;
        if (tipodato == "DECIMAL" || tipodato == "ENTERO" || tipodato == "FORMULA") {
            CONCEPTO_WEB.Concepliminf = $("#Concepliminf").val();
            CONCEPTO_WEB.Conceplimsup = $("#Conceplimsup").val();
        } else {
            CONCEPTO_WEB.Concepliminf = null;
            CONCEPTO_WEB.Conceplimsup = null;
        }

        //CONCEPTO_WEB.Conceptipolong1 = $("#Conceptipolong1").val();
        //CONCEPTO_WEB.Conceptipolong2 = $("#Conceptipolong2").val();

        //check
        var chkActivo = $("#ChkActivo").is(':checked');
        if (chkActivo) {
            CONCEPTO_WEB.Concepactivo = "1";
        } else {
            CONCEPTO_WEB.Concepactivo = "0";
        }
        var chkDetalleProp = $("#ChkDetalleProp").is(':checked');
        if (chkDetalleProp) {
            CONCEPTO_WEB.Conceppropeq = 1;
        } else {
            CONCEPTO_WEB.Conceppropeq = 0;
        }

        var chkOcultarComent = $("#ChkOcultarComent").is(':checked');
        if (chkOcultarComent) {
            CONCEPTO_WEB.Concepocultocomentario = "S";
        } else {
            CONCEPTO_WEB.Concepocultocomentario = "N";
        }

        var chkMostrarColorCelda = $("#ChkMostrarColor").is(':checked');
        if (chkMostrarColorCelda) {
            CONCEPTO_WEB.Concepflagcolor = 1;
        } else {
            CONCEPTO_WEB.Concepflagcolor = 0;
        }
    }
    if (OPCION_ACTUAL == OPCION_EDITAR) {
        CONCEPTO_WEB.Catecodi = $("#cbCategoriaEdit").val();
        CONCEPTO_WEB.Conceptipo = $("#cbTipoDato").val();
        CONCEPTO_WEB.Concepfichatec = $("#cbFicha").val();
        //CONCEPTO_WEB.Propcodipadre = $("#cbPadre").val();

        CONCEPTO_WEB.Concepcodi = $("#Concepcodi").val();
        CONCEPTO_WEB.Concepdesc = $("#Concepdesc").val();
        CONCEPTO_WEB.Concepnombficha = $("#Concepnombficha").val();
        CONCEPTO_WEB.Concepabrev = $("#Concepabrev").val();
        CONCEPTO_WEB.Concepdefinicion = $("#Concepdefinicion").val();
        CONCEPTO_WEB.Concepunid = $("#Concepunid").val();

        var tipodato = CONCEPTO_WEB.Conceptipo;
        if (tipodato == "DECIMAL" || tipodato == "ENTERO" || tipodato == "FORMULA") {
            CONCEPTO_WEB.Concepliminf = $("#Concepliminf").val();
            CONCEPTO_WEB.Conceplimsup = $("#Conceplimsup").val();
        } else {
            CONCEPTO_WEB.Concepliminf = null;
            CONCEPTO_WEB.Conceplimsup = null;
        }

        //CONCEPTO_WEB.Conceptipolong1 = $("#Conceptipolong1").val();
        //CONCEPTO_WEB.Conceptipolong2 = $("#Conceptipolong2").val();

        //check
        var chkActivo = $("#ChkActivo").is(':checked');
        if (chkActivo) {
            CONCEPTO_WEB.Concepactivo = "1";
        } else {
            CONCEPTO_WEB.Concepactivo = "0";
        }
        var chkDetalleProp = $("#ChkDetalleProp").is(':checked');
        if (chkDetalleProp) {
            CONCEPTO_WEB.Conceppropeq = 1;
        } else {
            CONCEPTO_WEB.Conceppropeq = 0;
        }

        var chkOcultarComent = $("#ChkOcultarComent").is(':checked');
        if (chkOcultarComent) {
            CONCEPTO_WEB.Concepocultocomentario = "S";
        } else {
            CONCEPTO_WEB.Concepocultocomentario = "N";
        }

        var chkMostrarColorCelda = $("#ChkMostrarColor").is(':checked');
        if (chkMostrarColorCelda) {
            CONCEPTO_WEB.Concepflagcolor = 1;
        } else {
            CONCEPTO_WEB.Concepflagcolor = 0;
        }
    }

    //LIMPIAR TIPO DE DATO
    var tipodato = $("#cbTipoDato").val();
    if (tipodato == "DECIMAL") {
        CONCEPTO_WEB.Conceptipolong1 = $("#Conceptipolong1").val();
        CONCEPTO_WEB.Conceptipolong2 = $("#Conceptipolong2").val();
    } else {
        if (tipodato == "ENTERO" || tipodato == "CARACTER") {
            CONCEPTO_WEB.Conceptipolong1 = $("#Conceptipolong1").val();
            $("#Conceptipolong2").val("");
            CONCEPTO_WEB.Conceptipolong2 = $("#Conceptipolong2").val();
        } else {
            $("#Conceptipolong1").val("");
            $("#Conceptipolong2").val("");
            CONCEPTO_WEB.Conceptipolong1 = $("#Conceptipolong1").val();
            CONCEPTO_WEB.Conceptipolong2 = $("#Conceptipolong2").val();
        }
    }
}

//validar Concepto
function validarConcepto(obj) {
    var validacion = "<ul>";
    var flag = true;

    if (obj.Concepdesc == null || obj.Concepdesc == '') {
        validacion = validacion + "<li>Nombre: campo requerido.</li>";//Campo Requerido
        flag = false;
    }


    if (obj.Concepfichatec == null || obj.Concepfichatec == '') {
        validacion = validacion + "<li>Ficha técnica: campo requerido.</li>";//Campo Requerido
        flag = false;
    }
    else {
        if (obj.Concepfichatec == "S") {
            if (obj.Concepnombficha == null || obj.Concepnombficha == '') {
                validacion = validacion + "<li>Nombre Ficha técnica: campo requerido.</li>";//Campo Requerido
                flag = false;
            }
        }
    }

    if (obj.Concepabrev == null || obj.Concepabrev == '') {
        validacion = validacion + "<li>Abreviatura: campo requerido.</li>";//Campo Requerido
        flag = false;
    }
    if (obj.Concepunid == null || obj.Concepunid == '') {
        validacion = validacion + "<li>Unidad: campo requerido.</li>";//Campo Requerido
        flag = false;
    }
    if (obj.Catecodi == null || obj.Catecodi == '' || obj.Catecodi == '-2') {
        validacion = validacion + "<li>Categoría de grupo: campo requerido.</li>";//Campo Requerido
        flag = false;
    }
    if (obj.Conceptipo == null || obj.Conceptipo == '') {
        validacion = validacion + "<li>Tipo Dato: campo requerido.</li>";//Campo Requerido
        flag = false;
    }


    //validamos longitudes 1 y 2
    var datoLong1 = obj.Conceptipolong1;
    var datoLong2 = obj.Conceptipolong2;
    var tipodato = obj.Conceptipo;
    if (tipodato == "DECIMAL") {

        //Longitud 1
        if (datoLong1 == null || datoLong1 == '') {

        }
        else {
            if (isNaN(datoLong1)) {
                validacion = validacion + "<li>Parte entera: debe ser un campo numérico entre [1-15].</li>";//Campo Requerido
                flag = false;
            } else {
                var datoLong1Numerico = parseInt(datoLong1);
                if (datoLong1Numerico < 1 || datoLong1Numerico > 15) {
                    validacion = validacion + "<li>Parte entera: el rango permitido es entre [1-15].</li>";//Campo Requerido
                    flag = false;
                }
            }
        }

        //Longitud 2
        if (datoLong2 == null || datoLong2 == '') {

        }
        else {
            if (isNaN(datoLong2)) {
                validacion = validacion + "<li>Parte decimal: debe ser un campo numérico entre [0-10].</li>";//Campo Requerido
                flag = false;
            } else {
                var datoLong2Numerico = parseInt(datoLong2);
                if (datoLong2Numerico < 0 || datoLong2Numerico > 10) {
                    validacion = validacion + "<li>Parte decimal: el rango permitido es entre [0-10].</li>";//Campo Requerido
                    flag = false;
                }
            }
        }
    } else {
        if (tipodato == "ENTERO") {
            //Longitud 1
            if (datoLong1 == null || datoLong1 == '') {

            }
            else {
                if (isNaN(datoLong1)) {
                    validacion = validacion + "<li>Parte entera: debe ser un campo numérico entre [1-15].</li>";//Campo Requerido
                    flag = false;
                } else {
                    var datoLong1Numerico = parseInt(datoLong1);
                    if (datoLong1Numerico < 1 || datoLong1Numerico > 15) {
                        validacion = validacion + "<li>Parte entera: el rango permitido es entre [1-15].</li>";//Campo Requerido
                        flag = false;
                    }
                }
            }
        }
    }

    //Validamos limites  para decimal, entero y formula
    if (tipodato == "DECIMAL" || tipodato == "ENTERO" || tipodato == "FORMULA") {

        //Validacion para Limite Inferior
        if (obj.Concepliminf != null && obj.Concepliminf != "") {
            if (tipodato == "DECIMAL") {
                var parteEntera = obj.Conceptipolong1;
                var parteDecimal = obj.Conceptipolong2;
                var valorDato = obj.Concepliminf;

                //Dado que parteEntera y parteDecimal pueden ser nulos, en esos casos toman el maximo valor posible
                if (parteEntera == "")
                    parteEntera = 15;
                if (parteDecimal == "")
                    parteDecimal = 10;

                if ((parteEntera != '') && (parteDecimal != '')) {
                    if (!isNaN(parteEntera) && !isNaN(parteDecimal)) {
                        if (valorDato == null || valorDato == '') {

                        } else {
                            if (isNaN(valorDato)) {
                                validacion = validacion + "<li>Límite inferior: debe ser un campo numérico.</li>";//Campo Requerido
                                flag = false;
                            } else {
                                var strValDatoPositivo = valorDato.replace('-', '');
                                var strValDatoPositivoSinPunto = strValDatoPositivo.replace('.', '');
                                var valorDatoNumericoP = parseFloat(strValDatoPositivo);
                                var numCifrasTotales = strValDatoPositivoSinPunto.length;
                                var valParteEntera = Math.trunc(valorDatoNumericoP);
                                var strValParteEntera = valParteEntera + "";

                                var numCifrasEntero = strValParteEntera.length;
                                var numCifrasDecimal = numCifrasTotales - numCifrasEntero;

                                if (numCifrasEntero > parteEntera) {
                                    validacion = validacion + "<li>Límite inferior: la máxima cantidad de cifras enteras es " + parteEntera + ".</li>";
                                    flag = false;
                                }

                                if (numCifrasDecimal > parteDecimal) {
                                    validacion = validacion + "<li>Límite inferior: la máxima cantidad de cifras decimales es " + parteDecimal + ".</li>";
                                    flag = false;
                                }

                            }
                        }
                    }
                }

            } else {
                if (tipodato == "ENTERO") {
                    var parteEntera = obj.Conceptipolong1;
                    var valorDato = obj.Concepliminf;

                    //Dado que parteEntera y parteDecimal pueden ser nulos, en esos casos toman el maximo valor posible
                    if (parteEntera == "")
                        parteEntera = 15;

                    if ((parteEntera != '')) {
                        if (!isNaN(parteEntera)) {
                            if (valorDato == null || valorDato == '') {

                            } else {
                                if (isNaN(valorDato)) {
                                    validacion = validacion + "<li>Límite inferior: debe ser un campo numérico.</li>";
                                    flag = false;
                                } else {
                                    var strValDatoPositivo = valorDato.replace('-', '');
                                    var strValDatoPositivoSinPunto = strValDatoPositivo.replace('.', '');
                                    var valorDatoNumericoP = parseFloat(strValDatoPositivo);
                                    var numCifrasTotales = strValDatoPositivoSinPunto.length;
                                    var valParteEntera = Math.trunc(valorDatoNumericoP);
                                    var strValParteEntera = valParteEntera + "";

                                    var numCifrasEntero = strValParteEntera.length;

                                    if (numCifrasTotales != numCifrasEntero) {
                                        validacion = validacion + "<li>Límite inferior: debe ingresar una cifra entera correcta.</li>";
                                        flag = false;
                                    }

                                    if (numCifrasEntero > parteEntera) {
                                        validacion = validacion + "<li>Límite inferior: la máxima cantidad de cifras enteras es " + parteEntera + ".</li>";
                                        flag = false;
                                    }

                                }
                            }
                        }
                    }

                } else {
                    if (tipodato == "FORMULA") {
                        //Para este caso toman el maximo valor posible
                        var parteEntera = 15;
                        var parteDecimal = 10;
                        var valorDato = obj.Concepliminf;

                        if ((parteEntera != '') && (parteDecimal != '')) {
                            if (!isNaN(parteEntera) && !isNaN(parteDecimal)) {
                                if (valorDato == null || valorDato == '') {

                                } else {
                                    if (isNaN(valorDato)) {
                                        validacion = validacion + "<li>Límite inferior: debe ser un campo numérico.</li>";//Campo Requerido
                                        flag = false;
                                    } else {
                                        var strValDatoPositivo = valorDato.replace('-', '');
                                        var strValDatoPositivoSinPunto = strValDatoPositivo.replace('.', '');
                                        var valorDatoNumericoP = parseFloat(strValDatoPositivo);
                                        var numCifrasTotales = strValDatoPositivoSinPunto.length;
                                        var valParteEntera = Math.trunc(valorDatoNumericoP);
                                        var strValParteEntera = valParteEntera + "";

                                        var numCifrasEntero = strValParteEntera.length;
                                        var numCifrasDecimal = numCifrasTotales - numCifrasEntero;

                                        if (numCifrasEntero > parteEntera) {
                                            validacion = validacion + "<li>Límite inferior: la máxima cantidad de cifras enteras es " + parteEntera + ".</li>";
                                            flag = false;
                                        }

                                        if (numCifrasDecimal > parteDecimal) {
                                            validacion = validacion + "<li>Límite inferior: la máxima cantidad de cifras decimales es " + parteDecimal + ".</li>";
                                            flag = false;
                                        }

                                    }
                                }
                            }
                        }

                    }
                }
            }

        }

        //Validacion para Limite superior
        if (obj.Conceplimsup != null && obj.Conceplimsup != "") {
            if (tipodato == "DECIMAL") {
                var parteEntera = obj.Conceptipolong1;
                var parteDecimal = obj.Conceptipolong2;
                var valorDato = obj.Conceplimsup;

                //Dado que parteEntera y parteDecimal pueden ser nulos, en esos casos toman el maximo valor posible
                if (parteEntera == "")
                    parteEntera = 15;
                if (parteDecimal == "")
                    parteDecimal = 10;

                if ((parteEntera != '') && (parteDecimal != '')) {
                    if (!isNaN(parteEntera) && !isNaN(parteDecimal)) {
                        if (valorDato == null || valorDato == '') {

                        } else {
                            if (isNaN(valorDato)) {
                                validacion = validacion + "<li>Límite superior: debe ser un campo numérico.</li>";//Campo Requerido
                                flag = false;
                            } else {
                                var strValDatoPositivo = valorDato.replace('-', '');
                                var strValDatoPositivoSinPunto = strValDatoPositivo.replace('.', '');
                                var valorDatoNumericoP = parseFloat(strValDatoPositivo);
                                var numCifrasTotales = strValDatoPositivoSinPunto.length;
                                var valParteEntera = Math.trunc(valorDatoNumericoP);
                                var strValParteEntera = valParteEntera + "";

                                var numCifrasEntero = strValParteEntera.length;
                                var numCifrasDecimal = numCifrasTotales - numCifrasEntero;

                                if (numCifrasEntero > parteEntera) {
                                    validacion = validacion + "<li>Límite superior: la máxima cantidad de cifras enteras es " + parteEntera + ".</li>";
                                    flag = false;
                                }

                                if (numCifrasDecimal > parteDecimal) {
                                    validacion = validacion + "<li>Límite superior: la máxima cantidad de cifras decimales es " + parteDecimal + ".</li>";
                                    flag = false;
                                }

                            }
                        }
                    }
                }

            } else {
                if (tipodato == "ENTERO") {
                    var parteEntera = obj.Conceptipolong1;
                    var valorDato = obj.Conceplimsup;

                    //Dado que parteEntera y parteDecimal pueden ser nulos, en esos casos toman el maximo valor posible
                    if (parteEntera == "")
                        parteEntera = 15;

                    if ((parteEntera != '')) {
                        if (!isNaN(parteEntera)) {
                            if (valorDato == null || valorDato == '') {

                            } else {
                                if (isNaN(valorDato)) {
                                    validacion = validacion + "<li>Límite superior: debe ser un campo numérico.</li>";
                                    flag = false;
                                } else {
                                    var strValDatoPositivo = valorDato.replace('-', '');
                                    var strValDatoPositivoSinPunto = strValDatoPositivo.replace('.', '');
                                    var valorDatoNumericoP = parseFloat(strValDatoPositivo);
                                    var numCifrasTotales = strValDatoPositivoSinPunto.length;
                                    var valParteEntera = Math.trunc(valorDatoNumericoP);
                                    var strValParteEntera = valParteEntera + "";

                                    var numCifrasEntero = strValParteEntera.length;

                                    if (numCifrasTotales != numCifrasEntero) {
                                        validacion = validacion + "<li>Límite superior: debe ingresar una cifra entera correcta.</li>";
                                        flag = false;
                                    }

                                    if (numCifrasEntero > parteEntera) {
                                        validacion = validacion + "<li>Límite superior: la máxima cantidad de cifras enteras es " + parteEntera + ".</li>";
                                        flag = false;
                                    }

                                }
                            }
                        }
                    }

                } else {
                    if (tipodato == "FORMULA") {
                        //Para este caso toman el maximo valor posible
                        var parteEntera = 15;
                        var parteDecimal = 10;
                        var valorDato = obj.Conceplimsup;

                        if ((parteEntera != '') && (parteDecimal != '')) {
                            if (!isNaN(parteEntera) && !isNaN(parteDecimal)) {
                                if (valorDato == null || valorDato == '') {

                                } else {
                                    if (isNaN(valorDato)) {
                                        validacion = validacion + "<li>Límite superior: debe ser un campo numérico.</li>";//Campo Requerido
                                        flag = false;
                                    } else {
                                        var strValDatoPositivo = valorDato.replace('-', '');
                                        var strValDatoPositivoSinPunto = strValDatoPositivo.replace('.', '');
                                        var valorDatoNumericoP = parseFloat(strValDatoPositivo);
                                        var numCifrasTotales = strValDatoPositivoSinPunto.length;
                                        var valParteEntera = Math.trunc(valorDatoNumericoP);
                                        var strValParteEntera = valParteEntera + "";

                                        var numCifrasEntero = strValParteEntera.length;
                                        var numCifrasDecimal = numCifrasTotales - numCifrasEntero;

                                        if (numCifrasEntero > parteEntera) {
                                            validacion = validacion + "<li>Límite superior: la máxima cantidad de cifras enteras es " + parteEntera + ".</li>";
                                            flag = false;
                                        }

                                        if (numCifrasDecimal > parteDecimal) {
                                            validacion = validacion + "<li>Límite superior: la máxima cantidad de cifras decimales es " + parteDecimal + ".</li>";
                                            flag = false;
                                        }

                                    }
                                }
                            }
                        }

                    }
                }
            }

        }

        //Validacion para Limite superior e inferior
        var numInf = obj.Concepliminf;
        var numSup = obj.Conceplimsup;
        if (numSup != null && numSup != "") {
            if (numInf != null && numInf != "") {
                if (!isNaN(numInf) && !isNaN(numSup)) {
                    var valorI = parseFloat(numInf);
                    var valorS = parseFloat(numSup);
                    if (valorI > valorS) {
                        validacion = validacion + "<li>Límite superior debe ser mayor o igual al límite inferior.</li>";
                        flag = false;
                    }
                }
            }
        }
    }



    validacion = validacion + "</ul>";

    if (flag == true) validacion = "";

    return validacion;
}

mostrarAlerta = function (alerta) {
    $('#mensaje2').html(alerta);
    $('#mensaje2').css("display", "block");
}

limpiarMensaje = function () {
    $('#mensaje2').html("");
    $('#mensaje2').css("display", "none");
}

function mostrarBloqueLimites() {
    $('.bloqueLimites').css("display", "table-row");
}

function ocultarBloqueLimites() {
    $('.bloqueLimites').css("display", "none");
}