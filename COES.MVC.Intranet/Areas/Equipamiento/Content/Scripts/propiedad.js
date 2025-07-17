var controlador = siteRoot + 'Equipamiento/Propiedad/';
var ANCHO_LISTADO = 900;
var PROPIEDAD_WEB = null;
var PROPIEDAD_GLOBAL = null;
var OPCION_VER = 1;
var OPCION_EDITAR = 2;
var OPCION_NUEVO = 3;
var OPCION_ACTUAL = 0;

var IMG_VER = `<img src="${siteRoot}Content/Images/btn-open.png" title="Ver Propiedad"/>`;
var IMG_EDITAR = `<img src="${siteRoot}Content/Images/btn-edit.png" title="Editar Propiedad"/>`;

$(function () {

    $('#cbFamilia').val("-2");
    $('#cbFichaTecnica').val("T");
    $("#txtNombrePropiedad").blur(function () {
        $("#txtNombrePropiedad").val($("#txtNombrePropiedad").val().trim()); //quitar espacios
    });
    $("#txtNombrePropiedad").bind("paste", function (e) {
        setTimeout(function () {
            $("#txtNombrePropiedad").val($("#txtNombrePropiedad").val().trim()); //quitar espacios
        }, 100);
    });
    $("#cbEstadoConsulta").val("-1");
    $('#btnBuscar').click(function () {
        mostrarListadoPropiedades();
    });
    $('#btnNuevo').click(function () {
        mantenerPropiedades(-1, true);
    });

    $('#btnDescargar').click(function () {
        reportePropiedades();
    });

    $('#btnImportar').click(function () {
        window.location.href = controlador + 'PropiedadesImportacion';
    });

    ANCHO_LISTADO = $("#mainLayout").width() > 900 ? $("#mainLayout").width() - 5 : 900;
   
    mostrarListadoPropiedades();

    $('#btnActualizarFormula').click(function () {
        ActualizarFormulaEdit();
    });
  
});

function mostrarListadoPropiedades() {
    $("#txtNombrePropiedad").val($("#txtNombrePropiedad").val().trim()); //quitar espacios

    var famcodi = parseInt($("#cbFamilia").val());
    var fichaTec = $("#cbFichaTecnica").val() || 0;
    var nombre = $("#txtNombrePropiedad").val() || "";
    var estado = $("#cbEstadoConsulta").val() || -1;

    //limpiarBarraMensaje('mensaje');
    $.ajax({
        type: 'POST',
        url: controlador + "ListadoPropiedades",
        dataType: 'json',
        data: {
            famcodi: famcodi,
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

                $('#TablaConsultaPropiedades').dataTable({
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
    var lista = model.ListaPropiedad;

    var cadena = '';
    cadena += `
    <table border="0" class="pretty tabla-icono" id="TablaConsultaPropiedades" cellspacing="0" width="100%" >
        <thead>
            <tr>
                <th>Acciones</th>
                <th>Código</th>
                <th>Nombre</th>                
                <th>Nombre ficha técnica</th>
                <th>Abreviatura</th>
                <th>Definición</th>
                <th>Unidad</th>
                <th>Tipo de Equipo</th>
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

            <tr id="fila_${item.Propcodi}">
                ${tdOpciones}
                <td style="text-align:center; ${sStyle}">${item.Propcodi}</td>
                <td style="text-align:left; ${sStyle}">${item.Propnomb}</td>                
                <td style="text-align:left; ${sStyle}">${item.Propnombficha}</td>
                <td style="text-align:center; ${sStyle}">${item.Propabrev}</td>
                <td style="text-align:left; ${sStyle}">${item.Propdefinicion}</td>
                <td style="text-align:center; ${sStyle}">${item.Propunidad}</td>
                <td style="text-align:left; ${sStyle}">${item.NombreFamilia}</td>
            </tr>
        `;
    }
    cadena += "</tbody></table>";

    return cadena;
}

function _tdAcciones(Model, item, sStyle) {
    var html = '';

    html += `<td style="text-align: center; ${sStyle}">`;

    if ($("#TienePermiso").val()) {

        html += `   
                <a href="#" id="view, ${item.Propcodi}" class="viewVer">
                    ${IMG_VER}
                </a>
                <a href="#" id="view, ${item.Propcodi}" class="viewEdicion">
                    ${IMG_EDITAR}
                </a>
        `;
    }
    else {
        html += `<a href="#" id="view, ${item.Propcodi}" class="viewVer">
                     ${IMG_VER}
                 </a>
        `;
    }

    return html;
}

function viewEvent() {
    $('.viewEdicion').click(function (event) {
        event.preventDefault();
        propCodi = $(this).attr("id").split(",")[1];
        mantenerPropiedades(propCodi, true);
    });
    $('.viewVer').click(function (event) {
        event.preventDefault();
        propCodi = $(this).attr("id").split(",")[1];
        mantenerPropiedades(propCodi, false);
    });
};

function reportePropiedades() {

    var famcodi = parseInt($("#cbFamilia").val());
    var fichaTec = $("#cbFichaTecnica").val() || 0;
    var nombre = $("#txtNombrePropiedad").val() || "";
    var estado = $("#cbEstadoConsulta").val() || -1;

    $.ajax({
        type: 'POST',
        url: controlador + 'ExportarPropiedades',
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


function mantenerPropiedades(propcodi, editable) {
    abrirPopupFormulario(propcodi, editable);
}

function abrirPopupFormulario(propcodi, editable) {
    $('#popupFormPropiedad').html('');

    $.ajax({
        type: 'POST',
        url: controlador + "ObtenerPropiedad",
        dataType: 'json',
        data: {
            propcodi: propcodi,
            editable: editable
        },
        success: function (evt) {
            if (evt.Resultado != "-1") {
                PROPIEDAD_WEB = evt.Entidad;
                PROPIEDAD_GLOBAL = evt;

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
                $("#popupFormPropiedad").html(generarHtmlPoputPropiedadFlotante(evt));
                inicializarVistaFormulario();
                cargarEventosFormulario();

                $('#popupFormPropiedad').bPopup({
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
            alert("Error al cargar Propiedad");
        }
    });
}

function cargarEventosFormulario() {
    $("#btnGuardar").unbind();
    $("#btnGuardar").click(function () {
        guardarPropiedad();
    });

    $('#cbTipoDato').on("change", function () {
        mostrarLongitudTipoDato();        
    });

    $("#Proptipolong1").unbind();
    $("#Proptipolong2").unbind();
    $("#Propliminf").unbind();
    $("#Proplimsup").unbind();
    
    $("#Proptipolong1, #Proptipolong2").bind('keypress', function (e) {
        var key = window.Event ? e.which : e.keyCode
        return (key <= 13 || (key >= 48 && key <= 57) || key == 46);
    });

    $("#Propliminf, #Proplimsup").bind('keypress', function (e) {
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

    $("#tdEditarFormula").hide();
    ocultarBloqueFormula(); 
    if (tipodato == "DECIMAL") {
        $("#tdLongitudes").show();
        document.getElementById('nombreLongitud').innerHTML = '';
        document.getElementById('nombreLongitud').innerHTML = 'Parte entera';
        //$("#nombreLongitud").val('Parte entera');
        document.getElementById("Proptipolong1").placeholder = "##";
        $("input#Proptipolong1").get(0).setAttribute("maxlength", 2);
        $("#divParteDecimal").show(); 
        mostrarBloqueLimites(); 
    } else {
        if (tipodato == "ENTERO") {
            $("#tdLongitudes").show();
            document.getElementById('nombreLongitud').innerHTML = '';
            document.getElementById('nombreLongitud').innerHTML = 'Parte entera';
            //$("#nombreLongitud").val('Parte entera');
            document.getElementById("Proptipolong1").placeholder = "##";
            $("input#Proptipolong1").get(0).setAttribute("maxlength", 2);
            $("#Proptipolong1").show();
            $("#divParteDecimal").hide();
            mostrarBloqueLimites();
        }
        else {
            document.getElementById("Proptipolong1").placeholder = "####";
            $("input#Proptipolong1").get(0).setAttribute("maxlength", 4);

            if (tipodato == "CARACTER") {
                $("#tdLongitudes").show();
                document.getElementById('nombreLongitud').innerHTML = '';
                document.getElementById('nombreLongitud').innerHTML = 'Máx. caracteres';
                //$("#nombreLongitud").val('Máx. caracteres');
                $("#Proptipolong1").show();
                $("#divParteDecimal").hide();
                ocultarBloqueLimites();
            } else {
                $("#tdLongitudes").hide();

                if (tipodato == "FORMULA" || tipodato =="FORMULA_FAMILIA")
                    mostrarBloqueLimites();
                else
                    ocultarBloqueLimites();
            }

            if (tipodato == "FORMULA_FAMILIA") {
                $("#tdEditarFormula").show();
                mostrarBloqueFormula(); 
            }
        }
    }
}

//Generar html
function generarHtmlPoputPropiedadFlotante(model) {

    var htmlTitulo = "";
    if (model.AccionNuevo) {
        htmlTitulo +=
            `Nueva propiedad`
    }
    else if (model.AccionEditar) {
        htmlTitulo += `
        Editar Propiedad
        `;
    }
    else {
        htmlTitulo += `
        Detalle de propiedad
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
<input type="hidden" id="CodPropiedad" value="${model.Propcodi}" />
<input type="hidden" id="AccionNuevo" value="${model.AccionNuevo}" />
<input type="hidden" id="AccionEditar" value="${model.AccionEditar}" />
<input id="hdnFamilia" type="hidden" value="${model.Famcodi}" />
<input id="hdnTipo" type="hidden" value="${model.Proptipo}" />
<input id="hdnFicha" type="hidden" value="${model.Propfichaoficial}" />

<table id="tablaDatos" cellpadding="5" style='width: 100%;'>
         <tr id="trCodigo">
             <td class="tbform-label">Código:</td>
             <td class="tbform-control">
                 <input type="text" id="Propcodi" name="Propcodi" value="" style="width:100px" />
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
                 <input type="text" id="Propnomb" name="Propnomb" maxlength="250" value="" style="width:240px" />
             </td>
         </tr>
         <tr>
             <td class="tbform-label">Nombre Ficha técnica <span class= "ft_obligatorio">(*)</span>:</td>
             <td class="tbform-control">
                 <input type="text" id="Propfichanomb" name="Propnombficha" maxlength="250" value="" style="width:240px" />
             </td>
         </tr>

         <tr>
             <td class="tbform-label">Abreviatura <span class= "obligatorio">(*)</span>:</td>
             <td class="tbform-control">
                 <input type="text" id="Propabrev" name="Propabrev" maxlength="20" value="" style="width:240px" />
             </td>
         </tr>
         <tr>
             <td class="tbform-label">Definición:</td>
             <td class="tbform-control">
                 <input type="text" id="Propdefinicion" name="Propdefinicion" maxlength="50" value="" style="width:240px" />
             </td>
         </tr>
         <tr>
             <td class="tbform-label">Tipo de equipo <span class= "obligatorio">(*)</span>:</td>
             <td  class="tbform-control">
                 <select style="background-color:white" id="cbFamiliaEdit" name="Entidad.Famcodi">
                 </select>
             </td>
         </tr>
            <tr>
                <td class="tbform-label">Unidad <span class= "obligatorio">(*)</span>:</td>
                <td class="tbform-control">
                    <input type="text" id="Propunidad" name="Propunidad" maxlength="10" value="" style="width:240px" />
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
                              <input type="text" id="Proptipolong1" name="Proptipolong1" placeholder="##" maxlength="2" value="" style="width:60px" />
                          </div>
                      </div>
                      <div id="divParteDecimal" style="float:left">
                          <div class="tbform-label">Parte Decimal </div>
                          <div>
                              <input type="text" id="Proptipolong2" name="Proptipolong2" placeholder="##" maxlength="2" value="" style="width:60px" />
                          </div>
                      </div>
                </td>
            </tr>

            <tr class="bloqueLimites">
                 <td class="tbform-label">Límite Inferior:</td>
                 <td class="tbform-control">
                     <input type="text" id="Propliminf" name="Propliminf" maxlength="27" value="" style="width:240px; " />
                 </td>
            </tr>

            <tr class="bloqueLimites">
                 <td class="tbform-label">Límite Superior:</td>
                 <td class="tbform-control">
                     <input type="text" id="Proplimsup" name="Proplimsup" maxlength="27" value="" style="width:240px; " />
                 </td>
            </tr>
              <tr class="bloqueFormula" id="idFormulaCE">
                 <td class="tbform-label">Fórmula:</td>
                 <td class="tbform-control">
                     <textarea id="Propformula" name="Propformula" style="width:240px; height: 150px; resize: none;" />
                 </td>
                  <td id="tdEditarFormula">
                   <div id="divEditarFormula" style="float:left">                         
                          <div>
                              <input type="button" value="Editar Fórmula" id="btnEditarFormula name="btnEditarFormula" onclick="muestraPopupEditarFormula()" class="form-action" style="margin-top: 15px" />
                          </div>
                      </div>
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

            <tr id="trFechCreacion">
                <td class="tbform-label">Fecha creación:</td>
                <td class="tbform-control">
                    <input type="text" id="Propfeccreacion" name="Propfeccreacion"  value="" style="width:240px" />
                </td>
            </tr>
            <tr id="trUsuCreacion">
                <td class="tbform-label">Usuario creación:</td>
                <td class="tbform-control">
                    <input type="text" id="Propusucreacion" name="Propusucreacion"  value="" style="width:240px" />
                </td>
            </tr>
            <tr id="trUsuModif">
                <td class="tbform-label">Usuario modificación:</td>
                <td class="tbform-control">
                    <input type="text" id="Propusumodificacion" name="Propusumodificacion" value="" style="width:240px" />
                </td>
            </tr>
            <tr id="trFechModif">
                <td class="tbform-label">Fecha modificación:</td>
                <td class="tbform-control">
                    <input type="text" id="Propfecmodificacion" name="Propfecmodificacion" value="" style="width:240px" />
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
            $("#Propcodi").prop('disabled', 'disabled');
            $("#Propnomb").prop('disabled', 'disabled');
            $("#Propfichanomb").prop('disabled', 'disabled');
            $("#Propabrev").prop('disabled', 'disabled');
            $("#Propdefinicion").prop('disabled', 'disabled');
            $("#Propunidad").prop('disabled', 'disabled');
            $("#Proptipolong1").prop('disabled', 'disabled');
            $("#Proptipolong2").prop('disabled', 'disabled');
            $("#Propliminf").prop('disabled', 'disabled');
            $("#Proplimsup").prop('disabled', 'disabled');

            $("#cbFamiliaEdit").prop('disabled', 'disabled');
            document.getElementById('cbFamiliaEdit').style.backgroundColor = '';
            $("#cbTipoDato").prop('disabled', 'disabled');
            document.getElementById('cbTipoDato').style.backgroundColor = '';
            $("#cbFicha").prop('disabled', 'disabled');
            document.getElementById('cbFicha').style.backgroundColor = '';

            $("#btnGuardar").hide();

            $("#ChkActivo").prop('disabled', 'disabled');
            $("#ChkMostrarColor").prop('disabled', 'disabled');
            $("#ChkOcultarComent").prop('disabled', 'disabled');
            $("#Propfeccreacion").prop('disabled', 'disabled');
            $("#Propusucreacion").prop('disabled', 'disabled');
            $("#Propusumodificacion").prop('disabled', 'disabled');
            $("#Propfecmodificacion").prop('disabled', 'disabled');

            //Listas desplegables
            inicializaDesplegables();
            $(".obligatorio").hide();
            $(".ft_obligatorio").hide();
            $("#spanNota").hide();

            //LLENAR CAMPOS
            $("#cbFamiliaEdit").val(PROPIEDAD_WEB.Famcodi);
            $("#cbTipoDato").val(PROPIEDAD_WEB.Proptipo);
            $("#cbFicha").val(PROPIEDAD_WEB.Propfichaoficial);

            $("#Propcodi").val(PROPIEDAD_WEB.Propcodi);
            $("#Propnomb").val(PROPIEDAD_WEB.Propnomb);
            $("#Propfichanomb").val(PROPIEDAD_WEB.Propnombficha);
            $("#Propabrev").val(PROPIEDAD_WEB.Propabrev);
            $("#Propdefinicion").val(PROPIEDAD_WEB.Propdefinicion);
            $("#Propunidad").val(PROPIEDAD_WEB.Propunidad);

            mostrarLongitudTipoDato();
            $("#Proptipolong1").val(PROPIEDAD_WEB.Proptipolong1);
            $("#Proptipolong2").val(PROPIEDAD_WEB.Proptipolong2);
            $("#Propliminf").val(PROPIEDAD_WEB.StrPropliminf);
            $("#Proplimsup").val(PROPIEDAD_WEB.StrProplimsup);
            $("#Propformula").val(PROPIEDAD_WEB.Propformula);

            //check
            if (PROPIEDAD_WEB.Propactivo == 1) {
                $("#ChkActivo").prop('checked', true);
            }

            if (PROPIEDAD_WEB.Propocultocomentario == "S") {
                $("#ChkOcultarComent").prop('checked', true);
            }

            if (PROPIEDAD_WEB.Propflagcolor == 1) {
                $("#ChkMostrarColor").prop('checked', true);
            }

            $("#Propfeccreacion").val(PROPIEDAD_WEB.PropfeccreacionDesc);
            $("#Propusucreacion").val(PROPIEDAD_WEB.Propusucreacion);
            $("#Propusumodificacion").val(PROPIEDAD_WEB.Propusumodificacion);
            $("#Propfecmodificacion").val(PROPIEDAD_WEB.PropfecmodificacionDesc);

            $("#tdEditarFormula").hide();
            $("#Propformula").prop('disabled', 'disabled');

            break;
        case OPCION_EDITAR:
            $("#mainLayoutEdit").show();
            $("#btnGuardar").show();
            $("#btnGuardar").val('Actualizar');
            $("#Propcodi").prop('disabled', 'disabled');

            $("#trFechCreacion").hide();
            $("#trUsuCreacion").hide();
            $("#trUsuModif").hide();
            $("#trFechModif").hide();

            document.getElementById('Propnomb').style.backgroundColor = 'white';
            document.getElementById('Propfichanomb').style.backgroundColor = 'white';
            document.getElementById('Propabrev').style.backgroundColor = 'white';
            document.getElementById('Propdefinicion').style.backgroundColor = 'white';
            document.getElementById('Propunidad').style.backgroundColor = 'white';
            document.getElementById('Proptipolong1').style.backgroundColor = 'white';
            document.getElementById('Proptipolong2').style.backgroundColor = 'white';
            document.getElementById('Propliminf').style.backgroundColor = 'white';
            document.getElementById('Proplimsup').style.backgroundColor = 'white';
            document.getElementById('Propformula').style.backgroundColor = 'white';

            $("#cbFamiliaEdit").removeAttr('disabled');
            document.getElementById('cbFamiliaEdit').style.backgroundColor = 'white';
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
            $("#cbFamiliaEdit").val(PROPIEDAD_WEB.Famcodi);
            $("#cbTipoDato").val(PROPIEDAD_WEB.Proptipo);
            $("#cbFicha").val(PROPIEDAD_WEB.Propfichaoficial);
            //$("#cbPadre").val(PROPIEDAD_WEB.Propcodipadre);
            if ($("#cbFicha").val() == "S") {
                $(".ft_obligatorio").show();
            }
            else {
                $(".ft_obligatorio").hide();
            }

            $("#Propcodi").val(PROPIEDAD_WEB.Propcodi);
            $("#Propnomb").val(PROPIEDAD_WEB.Propnomb);
            $("#Propfichanomb").val(PROPIEDAD_WEB.Propnombficha);
            $("#Propabrev").val(PROPIEDAD_WEB.Propabrev);
            $("#Propdefinicion").val(PROPIEDAD_WEB.Propdefinicion);
            $("#Propunidad").val(PROPIEDAD_WEB.Propunidad);

            mostrarLongitudTipoDato();
            $("#Proptipolong1").val(PROPIEDAD_WEB.Proptipolong1);
            $("#Proptipolong2").val(PROPIEDAD_WEB.Proptipolong2);
            $("#Propliminf").val(PROPIEDAD_WEB.StrPropliminf);
            $("#Proplimsup").val(PROPIEDAD_WEB.StrProplimsup);
            $("#Propformula").val(PROPIEDAD_WEB.Propformula);

            //check
            if (PROPIEDAD_WEB.Propactivo == 1) {
                $("#ChkActivo").prop('checked', true);
            }
            if (PROPIEDAD_WEB.Propocultocomentario == "S") {
                $("#ChkOcultarComent").prop('checked', true);
            }

            if (PROPIEDAD_WEB.Propflagcolor == 1) {
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
            ocultarBloqueFormula();

            document.getElementById('Propnomb').style.backgroundColor = 'white';
            document.getElementById('Propfichanomb').style.backgroundColor = 'white';
            document.getElementById('Propabrev').style.backgroundColor = 'white';
            document.getElementById('Propdefinicion').style.backgroundColor = 'white';
            document.getElementById('Propunidad').style.backgroundColor = 'white';
            document.getElementById('Proptipolong1').style.backgroundColor = 'white';
            document.getElementById('Proptipolong2').style.backgroundColor = 'white';
            document.getElementById('Propliminf').style.backgroundColor = 'white';
            document.getElementById('Proplimsup').style.backgroundColor = 'white';
            document.getElementById('Propformula').style.backgroundColor = 'white';

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
            $("#ChkOcultarComent").prop('checked', false);

            break;
    }
}

//Inicializa Listas Desplegables
function inicializaDesplegables() {

    $('#cbFamiliaEdit').empty();
    var optionTipoEquipo = '<option value="-2" >-SELECCIONE-</option>';
    optionTipoEquipo += '<option value="0" >-TODOS-</option>';
    $.each(PROPIEDAD_GLOBAL.ListaFamilia, function (k, v) {
        optionTipoEquipo += '<option value =' + v.Famcodi + '>' + v.Famnomb + '</option>';
    })
    $('#cbFamiliaEdit').append(optionTipoEquipo);

    $('#cbTipoDato').empty();
    //var optionTipoDato = '<option value="-1" >----- Seleccione una Tipo de dato ----- </option>';
    var optionTipoDato = '';
    $.each(PROPIEDAD_GLOBAL.ListaTipoDato, function (k, v) {
        optionTipoDato += '<option value =' + v.Valor + '>' + v.Descripcion + '</option>';
    })
    $('#cbTipoDato').append(optionTipoDato);

    $('#cbFicha').empty();
    //var optionFicha = '<option value="" >----- Seleccione una ficha ----- </option>';
    var optionFichaTec = '';
    $.each(PROPIEDAD_GLOBAL.ListaFichatecnica, function (k, v) {
        optionFichaTec += '<option value =' + v.Valor + '>' + v.Descripcion + '</option>';
    })
    $('#cbFicha').append(optionFichaTec);

}

//guardar Propiedad
function guardarPropiedad() {

    actualizarPropiedadJson();
    //if (confirm('¿Desea guardar ?')) {
    //}
    var msj = validarPropiedad(PROPIEDAD_WEB);
    if (msj == "") {
        limpiarMensaje();
        //$("#btnGuardar").hide();

        var dataJson = JSON.stringify(PROPIEDAD_WEB).replace(/\/Date/g, "\\\/Date").replace(/\)\//g, "\)\\\/");
        $.ajax({
            type: 'POST',
            dataType: 'json',
            traditional: true,
            url: controlador + "PropiedadGuardar",
            data: {
                dataJson: dataJson,
                opcion: OPCION_ACTUAL
            },
            success: function (result) {
                if (result.Resultado != '-1' && result.IdPropiedad > 0) {
                    $('#popupFormPropiedad').bPopup().close();
                    alert(result.StrMensaje);
                    mostrarListadoPropiedades();
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

//Json para nueva Propiedad
function actualizarPropiedadJson() {

    //$("#descrip").val(validarTextoSinSaltoLinea($("#descrip").val()));
    if (OPCION_ACTUAL == OPCION_NUEVO) {

        PROPIEDAD_WEB.Famcodi = $("#cbFamiliaEdit").val();
        PROPIEDAD_WEB.Proptipo = $("#cbTipoDato").val();
        PROPIEDAD_WEB.Propfichaoficial = $("#cbFicha").val();
        //PROPIEDAD_WEB.Propcodipadre = $("#cbPadre").val();

        PROPIEDAD_WEB.Propcodi = $("#Propcodi").val() || -1;
        PROPIEDAD_WEB.Propnomb = $("#Propnomb").val();
        PROPIEDAD_WEB.Propnombficha = $("#Propfichanomb").val();
        PROPIEDAD_WEB.Propabrev = $("#Propabrev").val();
        PROPIEDAD_WEB.Propdefinicion = $("#Propdefinicion").val();
        PROPIEDAD_WEB.Propunidad = $("#Propunidad").val();

        var tipodato = PROPIEDAD_WEB.Proptipo;
        if (tipodato == "DECIMAL" || tipodato == "ENTERO" || tipodato == "FORMULA" || tipodato == "FORMULA_FAMILIA") {
            PROPIEDAD_WEB.Propliminf = $("#Propliminf").val();
            PROPIEDAD_WEB.Proplimsup = $("#Proplimsup").val();
            PROPIEDAD_WEB.Propformula = $("#Propformula").val();
        } else {
            PROPIEDAD_WEB.Propliminf = null;
            PROPIEDAD_WEB.Proplimsup = null;
            PROPIEDAD_WEB.Propformula = null;
        }
        

        //PROPIEDAD_WEB.Proptipolong1 = $("#Proptipolong1").val();
        //PROPIEDAD_WEB.Proptipolong2 = $("#Proptipolong2").val();

        //check
        var chkActivo = $("#ChkActivo").is(':checked');
        if (chkActivo) {
            PROPIEDAD_WEB.Propactivo = 1;
        } else {
            PROPIEDAD_WEB.Propactivo = 0;
        }

        var chkOcultarComent = $("#ChkOcultarComent").is(':checked');
        if (chkOcultarComent) {
            PROPIEDAD_WEB.Propocultocomentario = "S";
        } else {
            PROPIEDAD_WEB.Propocultocomentario = "N";
        }

        var chkMostrarColorCelda = $("#ChkMostrarColor").is(':checked');
        if (chkMostrarColorCelda) {
            PROPIEDAD_WEB.Propflagcolor = 1;
        } else {
            PROPIEDAD_WEB.Propflagcolor = 0;
        }
    }
    if (OPCION_ACTUAL == OPCION_EDITAR) {
        PROPIEDAD_WEB.Famcodi = $("#cbFamiliaEdit").val();
        PROPIEDAD_WEB.Proptipo = $("#cbTipoDato").val();
        PROPIEDAD_WEB.Propfichaoficial = $("#cbFicha").val();
        //PROPIEDAD_WEB.Propcodipadre = $("#cbPadre").val();

        PROPIEDAD_WEB.Propcodi = $("#Propcodi").val();
        PROPIEDAD_WEB.Propnomb = $("#Propnomb").val();
        PROPIEDAD_WEB.Propnombficha = $("#Propfichanomb").val();
        PROPIEDAD_WEB.Propabrev = $("#Propabrev").val();
        PROPIEDAD_WEB.Propdefinicion = $("#Propdefinicion").val();
        PROPIEDAD_WEB.Propunidad = $("#Propunidad").val();
        
        var tipodato = PROPIEDAD_WEB.Proptipo;
        if (tipodato == "DECIMAL" || tipodato == "ENTERO" || tipodato == "FORMULA" || tipodato == "FORMULA_FAMILIA") {
            PROPIEDAD_WEB.Propliminf = $("#Propliminf").val();
            PROPIEDAD_WEB.Proplimsup = $("#Proplimsup").val();
            PROPIEDAD_WEB.Propformula = $("#Propformula").val();
        } else {
            PROPIEDAD_WEB.Propliminf = null;
            PROPIEDAD_WEB.Proplimsup = null;
            PROPIEDAD_WEB.Propformula = null;
        }

        //PROPIEDAD_WEB.Proptipolong1 = $("#Proptipolong1").val();
        //PROPIEDAD_WEB.Proptipolong2 = $("#Proptipolong2").val();

        //check
        var chkActivo = $("#ChkActivo").is(':checked');
        if (chkActivo) {
            PROPIEDAD_WEB.Propactivo = 1;
        } else {
            PROPIEDAD_WEB.Propactivo = 0;
        }

        var chkOcultarComent = $("#ChkOcultarComent").is(':checked');
        if (chkOcultarComent) {
            PROPIEDAD_WEB.Propocultocomentario = "S";
        } else {
            PROPIEDAD_WEB.Propocultocomentario = "N";
        }

        var chkMostrarColorCelda = $("#ChkMostrarColor").is(':checked');
        if (chkMostrarColorCelda) {
            PROPIEDAD_WEB.Propflagcolor = 1;
        } else {
            PROPIEDAD_WEB.Propflagcolor = 0;
        }
    }

    //LIMPIAR TIPO DE DATO
    var tipodato = $("#cbTipoDato").val();
    if (tipodato == "DECIMAL") {
        PROPIEDAD_WEB.Proptipolong1 = $("#Proptipolong1").val();
        PROPIEDAD_WEB.Proptipolong2 = $("#Proptipolong2").val();
    } else {
        if (tipodato == "ENTERO" || tipodato == "CARACTER") {
            PROPIEDAD_WEB.Proptipolong1 = $("#Proptipolong1").val();
            $("#Proptipolong2").val("");
            PROPIEDAD_WEB.Proptipolong2 = $("#Proptipolong2").val();
        } else {
            $("#Proptipolong1").val("");
            $("#Proptipolong2").val("");
            PROPIEDAD_WEB.Proptipolong1 = $("#Proptipolong1").val();
            PROPIEDAD_WEB.Proptipolong2 = $("#Proptipolong2").val();
        }
    }
}

//validar Propiedad
function validarPropiedad(obj) {
    var validacion = "<ul>";
    var flag = true;

    if (obj.Propnomb == null || obj.Propnomb == '') {
        validacion = validacion + "<li>Nombre: campo requerido.</li>";//Campo Requerido
        flag = false;
    }
    if (obj.Propfichaoficial == null || obj.Propfichaoficial == '') {
        validacion = validacion + "<li>Ficha técnica: campo requerido.</li>";//Campo Requerido
        flag = false;
    }
    else {
        if (obj.Propfichaoficial == "S") {
            if (obj.Propnombficha == null || obj.Propnombficha == '') {
                validacion = validacion + "<li>Nombre Ficha técnica: campo requerido.</li>";//Campo Requerido
                flag = false;
            }
        }
    }

    if (obj.Propabrev == null || obj.Propabrev == '') {
        validacion = validacion + "<li>Abreviatura: campo requerido.</li>";//Campo Requerido
        flag = false;
    }
    if (obj.Propunidad == null || obj.Propunidad == '') {
        validacion = validacion + "<li>Unidad: campo requerido.</li>";//Campo Requerido
        flag = false;
    }
    if (obj.Famcodi == null || obj.Famcodi == '' || obj.Famcodi == '-2') {
        validacion = validacion + "<li>Tipo equipo: campo requerido.</li>";//Campo Requerido
        flag = false;
    }
    if (obj.Proptipo == null || obj.Proptipo == '') {
        validacion = validacion + "<li>Tipo Dato: campo requerido.</li>";//Campo Requerido
        flag = false;
    }

    //validamos longitudes 1 y 2
    var datoLong1 = obj.Proptipolong1;
    var datoLong2 = obj.Proptipolong2;
    var tipodato = obj.Proptipo;
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
        if (obj.Propliminf != null && obj.Propliminf != "") {
            if (tipodato == "DECIMAL") {
                var parteEntera = obj.Proptipolong1;
                var parteDecimal = obj.Proptipolong2;
                var valorDato = obj.Propliminf;

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
                    var parteEntera = obj.Proptipolong1;
                    var valorDato = obj.Propliminf;

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
                        var valorDato = obj.Propliminf;                        

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
        if (obj.Proplimsup != null && obj.Proplimsup != "") {
            if (tipodato == "DECIMAL") {
                var parteEntera = obj.Proptipolong1;
                var parteDecimal = obj.Proptipolong2;
                var valorDato = obj.Proplimsup;

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
                    var parteEntera = obj.Proptipolong1;
                    var valorDato = obj.Proplimsup;

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
                        var valorDato = obj.Proplimsup;

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
        var numInf = obj.Propliminf;
        var numSup = obj.Proplimsup;
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

    if (tipodato == "FORMULA_FAMILIA") {
        var valorDato = obj.Propformula;

        if (valorDato == null || valorDato == "") {
            validacion = validacion + "<li>Fórmula: campo requerido.</li>";
            flag = false;
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

function mostrarBloqueFormula() {
    $('.bloqueFormula').css("display", "table-row");
}

function ocultarBloqueFormula() {
    $('.bloqueFormula').css("display", "none");
}

function muestraPopupEditarFormula() {
    //debugger;
    $("#idTPropiedades").html("");
    $("#idTFunciones").html("");

    var formula = $("#Propformula").val();
    $("#PropformulaEdit").val(formula);
    document.getElementById('PropformulaEdit').style.backgroundColor = 'white';

    $("#popupEditarFormula").bPopup({
        autoClose: false
    });

    InicializarPopupFormula();
};

function InicializarPopupFormula() {
    var famcodi = $("#cbFamiliaEdit").val();

    $.ajax({
        type: 'POST',
        url: controlador + 'ObtenerPropiedadFunciones',
        data: {
            famcodi: famcodi
        },
        dataType: 'json',
        async: false,
        success: function (result) {

            if (result.Resultado != '-1') {

                setTimeout(function () {
                    $('#idTPropiedades').html(dibujarTablaPropiedades(result.ListaPropiedad));

                    $('#tablaPropiedades').dataTable({
                        "scrollY": 200,
                        "scrollX": false,
                        "sDom": 't',
                        "ordering": false,
                        "bPaginate": false,
                        "iDisplayLength": -1,
                        "autoWidth": false,
                        "columnDefs": [
                            {
                                "targets": "_all",       
                                "createdCell": function (td, cellData, rowData, row, col) {
                                    $(td).css('text-align', 'left');  
                                }                        
                            },
                            { "width": "100px", "targets": 0 },
                            { "width": "280px", "targets": 1 },
                            { "width": "150px", "targets": 2 },
                            { "width": "30px", "targets": 3 }
                        ]
                    });

                }, 200);

                setTimeout(function () {
                    $('#idTFunciones').html(dibujarTablaFunciones(result.ListaFunciones));

                    $('#tablaFunciones').dataTable({
                        "scrollY": 200,
                        "scrollX": false,
                        "sDom": 't',
                        "ordering": false,
                        "bPaginate": false,
                        "iDisplayLength": -1,
                        "autoWidth": false,
                        "columnDefs": [
                            {
                                "targets": "_all",       
                                "createdCell": function (td, cellData, rowData, row, col) {
                                    $(td).css('text-align', 'left');  
                                }
                            },
                            { "width": "50px", "targets": 0 },
                            { "width": "250px", "targets": 1 },
                            { "width": "170px", "targets": 2 },
                            { "width": "30px", "targets": 3 }
                        ]
                    });

                }, 200);

            } else {

                $('#popupEditarFormula').bPopup().close();
                alert(result.StrMensaje);
            }
        },
        error: function () {
            alert("Ha ocurrido un error");
        }
    });
};

function dibujarTablaPropiedades(listPropiedades) {
    var total = listPropiedades.length;
    var cadena = "";
    cadena += "<table id='tablaPropiedades' border='1' class='pretty tabla-adicional' cellspacing='0'>";
    cadena += "<thead><tr><th>Abreviatura</th><th>Propiedad</th><th>Tipo de dato</th><th></th></tr></thead>";
    cadena += "<tbody>";
    for (var i = 0; i < total; i++) {
        cadena += "<tr><td>" + listPropiedades[i].Propabrev + "</td>";
        cadena += "<td>" + listPropiedades[i].Propnomb + "</td>";
        cadena += "<td>" + listPropiedades[i].Proptipo + "</td>";
        cadena += "<td>" + `<a href="JavaScript:agregarPropiedad('` + listPropiedades[i].Propabrev + `');" title="Agregar">
            <img src="${siteRoot}Content/Images/btn-derivar.png" alt="">  </a>` + "</td></tr>";
    }
    cadena += "</tbody></table>";
    return cadena;
};

function dibujarTablaFunciones(listFunciones) {
    var total = listFunciones.length;
    var cadena = "";
    cadena += "<table id='tablaFunciones' border='1' class='pretty tabla-adicional' cellspacing='0'>";
    cadena += "<thead><tr><th>Nombre</th><th>Descripción</th><th>Ejemplo</th><th></th></tr></thead>";
    cadena += "<tbody>";
    for (var i = 0; i < total; i++) {
        cadena += "<tr><td>" + listFunciones[i].Eqcatdabrev + "</td>";
        cadena += "<td style='word-wrap: break-word; white-space: normal;'>" + listFunciones[i].Eqcatddescripcion + "</td>";
        cadena += "<td style='word-wrap: break-word; white-space: normal;'>" + listFunciones[i].Eqcatdnota + "</td>";
        cadena += "<td>" + `<a href="JavaScript:agregarFuncion('` + listFunciones[i].Eqcatdabrev + `');" title="Agregar">
            <img src="${siteRoot}Content/Images/btn-derivar.png" alt="">  </a>` + "</td></tr>";
    }
    cadena += "</tbody></table>";
    return cadena;
};

function agregarPropiedad(propiedad) {
    var res = $("#PropformulaEdit").val() + propiedad;
    $("#PropformulaEdit").val(res);
};

function agregarFuncion(funcion) {
    var res = funcion + '(' + $("#PropformulaEdit").val() + ')';
    $("#PropformulaEdit").val(res);
};

function ActualizarFormulaEdit() {
    var formula = $("#PropformulaEdit").val();
    $("#Propformula").val(formula);

    $('#popupEditarFormula').bPopup().close();
};