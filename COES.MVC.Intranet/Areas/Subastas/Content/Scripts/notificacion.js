var controlador = siteRoot + 'Subastas/Notificacion/';

const NOTIFICACION_AGENTES_SIN_OFERTASXDEFECTO = 310;
const NOTIFICACION_AGENTES_OUTOGENERADA = 311;

//const VAR_FIRMA_COES = "{FIRMA_COES}";
const VAR_ANIO_SIGUIENTE = "{ANIO_SIGUIENTE}";
const VAR_NOMBRE_EMPRESA = "{NOMBRE_EMPRESA}";
const VAR_CORREOS_AGENTE = "{CORREOS_AGENTE}";
const VAR_TABLA_OFERTAS_POR_DEFECTO_AUTOGENERADO = "{TABLA_OFERTAS_POR_DEFECTO_AUTOGENERADO}";

var lstVariablesCorreo = [
    VAR_ANIO_SIGUIENTE, VAR_NOMBRE_EMPRESA, VAR_CORREOS_AGENTE
];

var lstVariablesCorreo2 = [
    VAR_ANIO_SIGUIENTE, VAR_NOMBRE_EMPRESA, VAR_CORREOS_AGENTE, VAR_TABLA_OFERTAS_POR_DEFECTO_AUTOGENERADO
];

function listarVariableCorreoTotales() {
    var lista = [];

    lista.push({ Valor: VAR_NOMBRE_EMPRESA, Nombre: 'NOMBRE_EMPRESA' });
    lista.push({ Valor: VAR_ANIO_SIGUIENTE, Nombre: 'ANIO_SIGUIENTE' });
    lista.push({ Valor: VAR_CORREOS_AGENTE, Nombre: 'CORREOS_AGENTE' });

    return lista;
}

function listarVariableCorreoTotales2() {
    var lista = [];

    lista.push({ Valor: VAR_NOMBRE_EMPRESA, Nombre: 'NOMBRE_EMPRESA' });
    lista.push({ Valor: VAR_ANIO_SIGUIENTE, Nombre: 'ANIO_SIGUIENTE' });
    lista.push({ Valor: VAR_CORREOS_AGENTE, Nombre: 'CORREOS_AGENTE' });
    lista.push({ Valor: VAR_TABLA_OFERTAS_POR_DEFECTO_AUTOGENERADO, Nombre: 'TABLA_OFERTAS_POR_DEFECTO_AUTOGENERADO' });

    return lista;
}

const NOTIFICACION = 1;
const RECORDATORIO = 2;

const TPCORRCODI_RECORDATORIO = 5;

const VER = 1;
const EDITAR = 2;

const ENVIO_AUTOMATICO = 0;
const ENVIO_MANUAL = 1;

const VARIABLE_ASUNTO = 0;
const VARIABLE_CC = 2;
const VARIABLE_PARA = 3;

const SEPARADOR_INI_VARIABLE = "{";// Debe coincidir con separadores de los valores en ConstantesCombustibles
const SEPARADOR_FIN_VARIABLE = "}";// Debe coincidir con separadores de los valores en ConstantesCombustibles

var IMG_EDITAR = '<img src="' + siteRoot + 'Content/Images/btn-edit.png" alt="Editar Plantilla" title="Editar Plantilla" width="19" height="19" style="">';
var IMG_VER = '<img src="' + siteRoot + 'Content/Images/btn-open.png" alt="Ver Plantilla" title="Ver Plantilla" width="19" height="19" style="">';
var tipoPlantillaEnListado = -1; //Notificacion o Recordatorio
var plantillaEnDetalle = -1; //iD
var respondeATodos = "";
var listarVariableCorreoOrdenada = [];
var listarVariableCorreoOrdenada2 = [];

$(function () {
    //ordeno lista variables por nombre    
    listarVariableCorreoOrdenada = listarVariableCorreoTotales().sort((a, b) => {
        let fa = a.Nombre.toLowerCase(),
            fb = b.Nombre.toLowerCase();

        if (fa < fb) {
            return -1;
        }
        if (fa > fb) {
            return 1;
        }
        return 0;
    });

    listarVariableCorreoOrdenada2 = listarVariableCorreoTotales2().sort((a, b) => {
        let fa = a.Nombre.toLowerCase(),
            fb = b.Nombre.toLowerCase();

        if (fa < fb) {
            return -1;
        }
        if (fa > fb) {
            return 1;
        }
        return 0;
    });

    $('#tab-container').easytabs({
        animate: false
    });
    $('#tab-container').easytabs('select', '#tabLista');

    mostrarListadoPlantilla();
});

function mostrarListadoPlantilla() {
    //tipoPlantillaEnListado = parseInt($("#cbTipoPlantilla").val()) || 0;

    $.ajax({
        type: 'POST',
        url: controlador + "ListarPlantillaCorreo",
        dataType: 'json',
        success: function (evt) {
            if (evt.Resultado != "-1") {

                var htmlPlantillas = dibujarTablaPlantillas(evt.ListadoPlantillasCorreo, evt.AccionGrabar);
                $("#div_listado").html(htmlPlantillas);

                $('#tablaPlantillas').dataTable({
                    "scrollY": 480,
                    "scrollX": false,
                    "sDom": 't',
                    "ordering": false,
                    "iDisplayLength": -1
                });

            } else {
                mostrarMensaje('mensaje_Listado', 'error', evt.Mensaje);
            }
        },
        error: function (err) {
            mostrarMensaje('mensaje_Listado', 'error', 'Ha ocurrido un error.');
        }
    });

}

function dibujarTablaPlantillas(listaPlantillas, esAdmin) {

    var cadena = '';
    cadena += `
    <table border="0" class="pretty tabla-icono" id="tablaPlantillas" >
       <thead>
           <tr style="height: 22px;">
               <th style='width:90px;'>Acciones</th>
               <th>Código de Notificación</th>
               <th>Nombre de Notificación</th>
               <th>Usuario Modificación</th>
               <th>Fecha de Modificación</th>        
            </tr>
        </thead>
        <tbody>
    `;

    for (var key in listaPlantillas) {
        var item = listaPlantillas[key];

        var htmlTdAccion = "";
        if (esAdmin && item.Plantcodi > 18) {
            htmlTdAccion = `
                    <a href="JavaScript:mostrarDetalle(${item.Plantcodi}, ${VER});">${IMG_VER}</a>
                    <a href="JavaScript:mostrarDetalle(${item.Plantcodi}, ${EDITAR});">${IMG_EDITAR}</a>
            `;
        } else {
            htmlTdAccion = `
                    <a href="JavaScript:mostrarDetalle(${item.Plantcodi}, ${VER});">${IMG_VER}</a>
            `;
        }

        cadena += `
            <tr>
                <td style='width:90px;'>   
                    ${htmlTdAccion}
                </td>
                <td>${item.Plantcodi}</td>
                <td style="text-align:left;">${item.Plantnomb}</td>
                <td>${item.UltimaModificacionUsuarioDesc}</td>
                <td>${item.UltimaModificacionFechaDesc}</td>
           </tr >           
        `;
    }


    cadena += `                   
        </tbody >
    </table >
    `;

    return cadena;
}

function mostrarDetalle(plantillacodi, accion) {
    $("#div_detalle").html('');
    limpiarBarraMensaje("mensaje_Detalle");
    limpiarBarraMensaje("mensaje_Listado");
    $('#tab-container').easytabs('select', '#tabDetalle');
    plantillaEnDetalle = plantillacodi;

    $.ajax({
        type: 'POST',
        url: controlador + "ObtenerDetalleCorreo",
        data: {
            plantillacodi: plantillacodi
        },
        success: function (evt) {
            if (evt.Resultado != "-1") {
                //respondeATodos = evt.PlantillaCorreo.RespondeAAgente;

                var htmlPlantilla = dibujarPlantilla(evt.PlantillaCorreo, plantillacodi, accion, ENVIO_AUTOMATICO, evt.TipoCorreo, evt.TieneEjecucionManual);
                $("#div_detalle").html(htmlPlantilla);

                //habilito/deshabilito edicion de contenido
                var readonly = 0;
                if (accion == VER) readonly = 1;

                //muestro editor de contenido
                if (plantillacodi == NOTIFICACION_AGENTES_SIN_OFERTASXDEFECTO)
                    evt.ListaVariables = listarVariableCorreoOrdenada;

                if (plantillacodi == NOTIFICACION_AGENTES_OUTOGENERADA)
                    evt.ListaVariables = listarVariableCorreoOrdenada2;

                iniciarControlTexto('Contenido', evt.ListaVariables, readonly);

                $('#HoraR').Zebra_DatePicker({
                    format: "H:i",
                });

                $('#btnGuardarPC').click(function () {
                    guardarPlantilla();
                });

            } else {
                mostrarMensaje('mensaje_Listado', 'error', evt.Mensaje);
            }
        },
        error: function (err) {
            mostrarMensaje('mensaje_Listado', 'error', 'Ha ocurrido un error.');
        }
    });
}

function dibujarPlantilla(objPlantilla, plantillacodi, accion, tipoEnvio, tipoCorreo, flagEjecManual) {

    var esEditable = true;
    var habilitacion = "";
    var backColor = "#ffffff";

    if (accion == VER) {
        esEditable = false;
        habilitacion = "disabled";
        backColor = "";
    }

    //Formato 
    var val_To = objPlantilla.Planticorreos != null ? objPlantilla.Planticorreos : "";
    var val_CC = objPlantilla.PlanticorreosCc != null ? objPlantilla.PlanticorreosCc : "";
    var val_BCC = objPlantilla.PlanticorreosBcc != null ? objPlantilla.PlanticorreosBcc : "";
    var val_Asunto = objPlantilla.Plantasunto != null ? objPlantilla.Plantasunto : "";
    var val_Contenido = objPlantilla.Plantcontenido != null ? objPlantilla.Plantcontenido : "";

    //campo De
    var val_De = objPlantilla.PlanticorreoFrom != null ? objPlantilla.PlanticorreoFrom : "";
    var cadena = '';


    //Agrego boton para ejecutar recordatorios
    //Tambien agrego boton para ejecutar recordatorios de alguns notificaciones
    if (flagEjecManual == "S" && plantillacodi == NOTIFICACION_AGENTES_SIN_OFERTASXDEFECTO) {
        cadena += `
        <fieldset style="margin-bottom: 10px; padding-bottom: revert;">
            <legend>Notificación Manual</legend>
            Notificación automática. <b>Presionar botón ejecutar para enviar recordatorio de forma manual. </b>
            <input style="float: right;" type="button" id="btnEjecutar" value="Ejecutar"  onclick="ejecutarRecordatorio( ${plantillacodi});"/>
            
        </fieldset>
        `;
    }

    if (flagEjecManual == "S" && plantillacodi == NOTIFICACION_AGENTES_OUTOGENERADA) {
        cadena += `
        <fieldset style="margin-bottom: 10px; padding-bottom: revert;">
            <legend>Notificación Manual</legend>
            Notificación automática. <b>Presionar botón ejecutar para enviar recordatorio de forma manual. </b>
            <input style="float: right;" type="button" id="btnEjecutar" value="Ejecutar"  onclick="ejecutarRecordatorio( ${plantillacodi});"/>
            <input style="float: right;" type="button" id="btnResetear" value="Resetear"  onclick="resetearRecordatorio( ${plantillacodi});"/>
        </fieldset>
        `;
    }


    //Agrego nombre del correo
    cadena += `
    <div id="informacionPlantilla" style="height:20px; padding-top: 8px; padding-bottom: 12px;" >
        <div class="tbform-label" style="width:120px; float: left; font-size:18px;">
            Plantilla:
        </div>
        <div id="valor" style="float: left; font-size:18px;">
            ${objPlantilla.Plantcodi} - ${objPlantilla.Plantnomb}
        </div>
    </div>
    <table style="width:100%">
    `;

    cadena += `
                    <tr>
                        <td style="width:120px" class="registro-label"><input type="hidden" id="hfIdPlantillaCorreo" value="${objPlantilla.Plantcodi}"/></td>
                    </tr>
                    <tr>
                        <td style="width:120px" class="tbform-label">De:</td>
    `;

    //Habilito y deshabilito el campo De y Para
    cadena += `
                        <td class="registro-control" style="width:1100px;"><input type="text" name="From" id="From" value="${val_De}" maxlength="900" style="width:1090px;" disabled/></td>
                    </tr>
        `;
    cadena += `
                    <tr>
                        <td class="tbform-label">Para:</td>
                        <td class="registro-control" style="width:1100px;"><input type="text" name="To" id="To" value="${val_To}" maxlength="100" style="width:1090px;" disabled/></td>
                    </tr>
        `;

    //Otros (CC, BCC, ASUNTO y CONTENIDO)
    cadena += `                
            
            <tr>
                <td class="tbform-label">CC:</td>
                <td class="registro-control" style="width:1100px;"><input name="CC" id="CC" type="text" value="${val_CC}" maxlength="120" style="width:1090px; background-color: ${backColor}" ${habilitacion}/></td>
            </tr>
            <tr>
                <td class="tbform-label"> BCC:</td>
                <td class="registro-control" style="width:1100px;"><input name="BCC" id="BCC" type="text" value="${val_BCC}" maxlength="120" style="width:1090px; background-color: ${backColor}" ${habilitacion}/></td>
            </tr>
            <tr>
                <td class="tbform-label">Asunto (*):</td>
                <td class="registro-control" style="width:1100px;">
                    <textarea maxlength="450" name="Asunto" id="Asuntos" value=""  cols="" rows="3" style="resize: none;width:1090px; background-color: ${backColor}" onfocus="activarBoton('botonAddPropAsunto');"  onblur="desactivarBoton('botonAddPropAsunto','Asuntos');" ${habilitacion}>${val_Asunto}</textarea>
                </td>
                <td class="registro-label"><input type="button" id="botonAddPropAsunto" value="Agregar variable" onclick="abrirListadoVariables( ${plantillacodi}, '${VARIABLE_ASUNTO}');" style="visibility:hidden"  ></td>
            </tr>
            <tr>
                <td class="tbform-label"> Contenido (*):</td>
                <td class="registro-control" style="width:1100px;">
                    <textarea name="Contenido" id="Contenido" maxlength="2000" cols="180" rows="22"  ${habilitacion}>
                        ${val_Contenido}
                    </textarea>
                    (*): Campos obligatorios.
                </td>
                <td class="registro-label">
                    
                </td>
            </tr>
    `;

    if (esEditable) {
        cadena += `

            <tr>
                
                <td colspan="3" style="padding-top: 2px; text-align: center;">
                    <input type="button" id="btnGuardarPC" value="Guardar" />
                    
                </td>
            </tr>

        `;
    }

    cadena += `
    </table>    
    `;

    return cadena;
}

//Resetear data
function resetearRecordatorio(plantcodi) {
    limpiarBarraMensaje("mensaje_Detalle");
    limpiarBarraMensaje("mensaje_Listado");
    $.ajax({
        type: 'POST',
        url: controlador + 'ResetearRecordatorio',
        data: {
            plantcodi: plantcodi
        },
        success: function (evt) {
            if (evt.Resultado != "-1") {
                mostrarMensaje('mensaje_Detalle', evt.Resultado == "1" ? "exito" : "alert", evt.Mensaje);

            } else {
                mostrarMensaje('mensaje_Detalle', 'error', evt.Mensaje);
            }
        },
        error: function (err) {
            mostrarMensaje('mensaje_Detalle', 'error', 'Ha ocurrido un error...');

        }
    });
}

//Ejecutar recordatorio
function ejecutarRecordatorio(plantcodi) {
    limpiarBarraMensaje("mensaje_Detalle");
    limpiarBarraMensaje("mensaje_Listado");
    $.ajax({
        type: 'POST',
        url: controlador + 'EjecutarRecordatorio',
        data: {
            plantcodi: plantcodi
        },
        success: function (evt) {
            if (evt.Resultado != "-1") {

                mostrarMensaje('mensaje_Detalle', evt.Resultado == "1" ? "exito" : "alert", evt.Mensaje);

            } else {
                mostrarMensaje('mensaje_Detalle', 'error', evt.Mensaje);
            }
        },
        error: function (err) {
            mostrarMensaje('mensaje_Detalle', 'error', 'Ha ocurrido un error...');

        }
    });
}

function iniciarControlTexto(id, listaVariables, sololectura) {

    tinymce.remove();

    tinymce.init({
        selector: '#' + id,
        plugins: [
            //'paste textcolor colorpicker textpattern link table image imagetools preview'
            'wordcount advlist anchor autolink codesample colorpicker contextmenu fullscreen image imagetools lists link media noneditable preview  searchreplace table template textcolor visualblocks wordcount'
        ],
        toolbar1:
            //'insertfile undo redo | fontsizeselect bold italic underline strikethrough | forecolor backcolor | alignleft aligncenter alignright alignjustify | bullist numlist outdent indent | link | table | image | mybutton | preview',
            'insertfile undo redo | styleselect fontsizeselect | forecolor backcolor | bullist numlist outdent indent | link | table | mybutton | preview',

        menubar: false,
        readonly: sololectura ? 1 : 0,
        language: 'es',
        statusbar: false,
        convert_urls: false,
        plugin_preview_width: 1000,
        setup: function (editor) {
            editor.on('change',
                function () {
                    editor.save();
                });
            editor.addButton('mybutton', {
                type: 'menubutton',
                text: 'Agregar Variables',
                tooltip: "Ingrese una variable",
                icon: false,
                menu: llenarMenus(editor, listaVariables)
            });

        }
    });

}


function llenarMenus(e, lista) {
    var listaObj = [];
    var numVariables = lista.length;

    if (numVariables >= 1) {
        var regObj = {};
        var regVariable = lista[0];
        regObj.text = regVariable.Nombre;
        regObj.onclick = function () { e.insertContent(regVariable.Valor); };
        listaObj.push(regObj);

        if (numVariables >= 2) {
            var regObj1 = {};
            var regVariable1 = lista[1];
            regObj1.text = regVariable1.Nombre;
            regObj1.onclick = function () { e.insertContent(regVariable1.Valor); };
            listaObj.push(regObj1);

            if (numVariables >= 3) {
                var regObj2 = {};
                var regVariable2 = lista[2];
                regObj2.text = regVariable2.Nombre;
                regObj2.onclick = function () { e.insertContent(regVariable2.Valor); };
                listaObj.push(regObj2);

                if (numVariables >= 4) {
                    var regObj3 = {};
                    var regVariable3 = lista[3];
                    regObj3.text = regVariable3.Nombre;
                    regObj3.onclick = function () { e.insertContent(regVariable3.Valor); };
                    listaObj.push(regObj3);
                }
            }
        }
    }

    return listaObj;
}

function sleep(ms) {
    return new Promise(resolve => setTimeout(resolve, ms));
}

function activarBoton(idBoton) {
    $("#" + idBoton).css("visibility", "visible");
}

async function desactivarBoton(idBoton, idCaja) {
    await sleep(800);

    var caja = document.getElementById(idCaja);

    if (document.activeElement != caja) {
        $("#" + idBoton).css("visibility", "hidden");
    }
}

function abrirListadoVariables(idPlantilla, tipoCampo) {

    var lista = [];

    if (idPlantilla == NOTIFICACION_AGENTES_SIN_OFERTASXDEFECTO)
        lista = listarVariableCorreoOrdenada;

    if (idPlantilla == NOTIFICACION_AGENTES_OUTOGENERADA)
        lista = listarVariableCorreoOrdenada2;


    var htmlVariables = dibujarListadoVariables(lista);
    $("#seccionListadoVariables").html(htmlVariables);

    abrirPopup("popupAgregarVariable");

    $('#btnAceptarVar').click(function () {
        if (tipoCampo == VARIABLE_ASUNTO)
            agregarTextoA($("#cbVariable").val(), "Asuntos");

        cerrarPopup('popupAgregarVariable');
    });

    $('#btnCancelarVar').click(function () {
        cerrarPopup('popupAgregarVariable');
    });


}

function dibujarListadoVariables(listaVariables) {

    var cadena = '';
    cadena += `
    <table border="0"  id="tablaCargos">       
        <tr>
            <td>Variable :</td>
            <td>
                <select id="cbVariable" style="WIDTH: 450px;">
    `;

    for (key in listaVariables) {
        var item = listaVariables[key];
        cadena += `            
                    <option value="${item.Valor}">${item.Nombre}</option>
        `;
    }

    cadena += `            
                </select>
            </td>
         </tr >
         <tr style="text-align: center;">
                <td colspan="2" style="padding-top: 35px;">
                    <input type="button" id="btnAceptarVar" value="Agregar" />
                    <input type="button" id="btnCancelarVar" value="Cancelar" />
                </td>
            </tr>
    </table >
    `;

    return cadena;
}

function agregarTextoA(textoAIngresar, idCaja) {
    var caja = document.getElementById(idCaja);
    var inicio = caja.selectionStart;
    var fin = caja.selectionEnd;
    var textoAgregado = textoAIngresar;

    var texto = caja.value;
    var textIni = texto.substring(0, inicio);
    var textFin = texto.substring(inicio);
    var nuevoTexto = textIni + textoAgregado + textFin;
    $('#' + idCaja).val(nuevoTexto);

    caja.focus();
}

function guardarPlantilla() {
    limpiarBarraMensaje("mensaje_Detalle");
    limpiarBarraMensaje("mensaje_Listado");
    var correo = getPlantillaCorreo();

    var msg = validarCamposCorreoAGuardar(correo);

    if (msg == "") {
        $.ajax({
            type: 'POST',
            url: controlador + 'GuardarPlantillaCorreo',
            contentType: "application/json",
            dataType: 'json',
            data: JSON.stringify({
                plantillaCorreo: correo,
            }),
            cache: false,
            success: function (evt) {
                if (evt.Resultado != "-1") {

                    //muestra el listado respectivo
                    $('#tab-container').easytabs('select', '#tabLista');
                    mostrarMensaje('mensaje_Detalle', 'exito', "Los cambios fueron guardados exitosamente...");
                    mostrarListadoPlantilla();
                    $("#div_detalle").html("");

                } else {
                    mostrarMensaje('mensaje_Detalle', 'error', evt.Mensaje);
                }
            },
            error: function (err) {
                mostrarMensaje('mensaje_Detalle', 'error', 'Ha ocurrido un error.' + evt.Mensaje);
            }
        });
    } else {
        mostrarMensaje('mensaje_Detalle', 'error', msg);
    }
}

function getPlantillaCorreo() {
    var obj = {};

    //Parametros Dia repecion (recordatorios)
    obj.ParametroDiaHora = "";
    if (tipoPlantillaEnListado == TPCORRCODI_RECORDATORIO) {
        obj.ParametroDiaHora = $("#parametroDR").val();
    }

    //Hora y Estado
    obj.Hora = $("#HoraR").val();
    obj.EstadoRecordatorio = $("#cbEstadoR").val();

    //otros
    obj.Planticorreos = $("#To").val();
    obj.Plantcodi = $("#hfIdPlantillaCorreo").val();
    obj.Plantcontenido = $("#Contenido").val();
    obj.Plantasunto = $("#Asuntos").val();
    obj.PlanticorreosCc = $("#CC").val();
    obj.PlanticorreosBcc = $("#BCC").val();
    obj.PlanticorreoFrom = $("#From").val();

    return obj;
}

function validarCamposCorreoAGuardar(correo) {
    var msj = "";

    /*********************************************** validacion del campo CC ***********************************************/
    var validaCc = validarCorreo($('#CC').val(), 0, -1, "Cc");
    if (validaCc < 0) {
        msj += "<p>Error encontrado en el campo CC. Revisar que el/los correo(s) sea(n) válido(s) y separados por punto y coma (;).</p>";
    }
    var msg2 = validarTamanioCaracteres($('#CC').val(), "CC", 300);
    if (msg2 != "") {
        msj += msg2;
    }

    /*********************************************** validacion del campo BCC ***********************************************/
    var validaBcc = validarCorreo($('#BCC').val(), 0, -1, "Bcc");
    if (validaBcc < 0) {
        msj += "<p>Error encontrado en el campo BCC. Revisar que el/los correo(s) sea(n) válido(s) y separados por punto y coma (;).</p>";
    }
    var msg3 = validarTamanioCaracteres($('#BCC').val(), "BCC", 300);
    if (msg3 != "") {
        msj += msg3;
    }

    /********************************************** validacion del campo Asunto ***********************************************/
    var asunto_ = correo.Plantasunto.trim();
    if (asunto_ == "") {
        msj += "<p>Debe ingresar texto en 'Asunto'.</p>";
    }

    var tienenMismaCantidadAsunto = validarCantidadDeSeparadores(asunto_);

    if (!tienenMismaCantidadAsunto) {
        msj += "<p>Revisar campo Asunto, la cantidad de caracteres '{' no coincide con la cantidad de caracteres '}'. </p>";
    }

    //valido que las palabras dentro de un {} sean variables admitidas
    if (correo.Plantcodi == NOTIFICACION_AGENTES_SIN_OFERTASXDEFECTO)
        msj = validarVariablesCorrectas(asunto_, "Asunto", lstVariablesCorreo, msj);

    if (correo.Plantcodi == NOTIFICACION_AGENTES_OUTOGENERADA)
        msj = validarVariablesCorrectas(asunto_, "Asunto", lstVariablesCorreo2, msj);

    var msg4 = validarTamanioCaracteres(asunto_, "Asunto", 300);
    if (msg4 != "") {
        msj += msg4;
    }


    /*********************************************** validacion del campo Contenido ***********************************************/

    var contenido_ = correo.Plantcontenido.trim();
    if (!validarContenidoHtml(contenido_)) {
        msj += "<p>Debe ingresar texto en 'Contenido'.</p>";
        contenido_ = '';
    }
    var tienenMismaCantidadContenido = validarCantidadDeSeparadores(contenido_);


    if (!tienenMismaCantidadContenido) {
        msj += "<p>Revisar campo Contenido, la cantidad de caracteres '{' no coincide con la cantidad de caracteres '}'. </p>";
    }

    //valido que las palabras dentro de un {} sean variables admitidas
    if (correo.Plantcodi == NOTIFICACION_AGENTES_SIN_OFERTASXDEFECTO)
        msj = validarVariablesCorrectas(contenido_, "Contenido", lstVariablesCorreo, msj);

    if (correo.Plantcodi == NOTIFICACION_AGENTES_OUTOGENERADA)
        msj = validarVariablesCorrectas(contenido_, "Contenido", lstVariablesCorreo2, msj);


    var msg5 = validarTamanioCaracteres(asunto_, "Asunto", 2000);
    if (msg5 != "") {
        msj += msg5;
    }

    return msj;
}

//Validación de campos
function borrarVacios(textoConVacios) {
    let textoSinVacios = textoConVacios.replace(/ /g, "");
    return textoSinVacios;
}

function validarCantidadDeSeparadores(campo) {

    //valido que la cantidad de { sea igual al de }
    let regexIni = new RegExp(SEPARADOR_INI_VARIABLE, 'g')
    let regexFin = new RegExp(SEPARADOR_FIN_VARIABLE, 'g')
    /*const regex = /{/g;*/
    const lstSeparadorIniCampo = campo.match(regexIni);
    const lstSeparadorFinCampo = campo.match(regexFin);

    var tienenMismaCantidad = false;
    if (lstSeparadorIniCampo != null) {
        if (lstSeparadorFinCampo != null) {
            if (lstSeparadorIniCampo.length == lstSeparadorFinCampo.length)
                tienenMismaCantidad = true;
        } else {
            tienenMismaCantidad = false;
        }
    } else {
        if (lstSeparadorFinCampo != null) {
            tienenMismaCantidad = false;
        } else {
            tienenMismaCantidad = true;
        }
    }

    return tienenMismaCantidad;
}

function validarNumeroPuntoYComas(valorCampo, campo, msj) {
    var cadena = valorCampo;

    if (cadena != "") {

        var arreglo = cadena.split(';');
        var nroCorreo = 0;
        var longitud = arreglo.length;

        if (longitud == 0) {
            arreglo = cadena;
            longitud = 1;
        }

        for (var i = 0; i < longitud; i++) {

            var email = arreglo[i].trim();
            var validacion = validarDirecccionCorreo(email);

            if (validacion) {
                nroCorreo++;
            }

        }

        //si hay correos valido la cantidad de  ;
        if (nroCorreo > 0) {
            //num de punto y comas
            const regex = /;/g;
            const lstTotalPuntoYComas = valorCampo.match(regex);

            //num de llaves
            const regex2 = /{/g;
            const lstTotalInicioConectores = valorCampo.match(regex2);

            //solo valido si hay variables
            if (lstTotalInicioConectores != null) {
                if (lstTotalPuntoYComas != null) {
                    if (lstTotalInicioConectores.length != lstTotalPuntoYComas.length) {
                        if (campo == "To")
                            campo = "Para";
                        msj += "<p>Revisar campo " + campo + ". Se detectó que el número de punto y comas (;) es incorrecto. </p>";
                    }
                } else {
                    if (campo == "To")
                        campo = "Para";
                    msj += "<p>Revisar campo " + campo + ". Se detectó que el número de punto y comas (;) es incorrectoo. </p>";
                }
            }
        }

    }
    return msj;
}

function validarSeparacionConectores(valorCampo, campo, msj) {
    //validando que no haya {var1}{var2} (sin ; entre ellos)

    //quito los vacios
    var valorSinVacios = borrarVacios(valorCampo);
    const regex = /}\w*{/g;
    const lstTotalLlavesSinPuntoYComa = valorSinVacios.match(regex);
    if (lstTotalLlavesSinPuntoYComa != null) {
        if (lstTotalLlavesSinPuntoYComa.length > 0) {
            if (campo == "To")
                campo = "Para";
            msj += "<p>Revisar campo " + campo + ". Se detectó la omisión de (;) entre dos variables continuas. </p>";
        }
    }

    return msj;
}

function validarVariablesCorrectas(valorCampo, campo, lstVariables, msj) {
    //validando textos tipo: {texto}
    //const regex = /{\w+}/g; //no sirve cuando hay espacios entre {} dado que w es word
    const regex = /{([^}]+)}/g;//atrapa los q inician con {, sigue mientras no haya }, y termina con un }. /g para que busque todas las coincidencias
    const lstPalabrasDentroParentesis = valorCampo.match(regex);
    if (lstPalabrasDentroParentesis != null) {
        let lstDiferentes = lstPalabrasDentroParentesis.filter(x => !lstVariables.includes(x));
        if (lstDiferentes.length > 0) {
            if (campo == "To")
                campo = "Para";
            msj += "<p>Revisar campo " + campo + ". Se detectó variables ( texto dentro de {} ) no reconocidas. </p>";
        }
    }

    //validando textos tipo: {}
    const regex2 = /{}/g;
    const lstPalabrasSoloParentesis = valorCampo.match(regex2);
    if (lstPalabrasSoloParentesis != null) {
        if (lstPalabrasSoloParentesis.length > 0) {
            if (campo == "To")
                campo = "Para";
            msj += "<p>Revisar campo " + campo + ". Se detectó texto ( {} ) no permitido. </p>";
        }
    }

    return msj;
}

function validarTamanioCaracteres(valCadena, nombCampo, tamanio) {
    var salida = "";
    var tam = valCadena.length;

    if (tam > tamanio) {
        salida = "<p>La cantidad de caracteres permitidos en el campo '" + nombCampo + "' fue superado. Solo se permite " + tamanio + " caracteres.</p>";
    }
    return salida;
}

function validarCorreo(valCadena, minimo, maximo, campo) {
    var cadena = valCadena;

    if (cadena != null) {
        for (var i = 0; i < lstVariablesCorreo.length; i++) {
            cadena = cadena.replace(lstVariablesCorreo[i], "");
        }
    }

    var arreglo = cadena.split(';');
    var nroCorreo = 0;
    var longitud = arreglo.length;

    if (longitud == 0) {
        arreglo = cadena;
        longitud = 1;
    }

    for (var i = 0; i < longitud; i++) {

        var email = arreglo[i].trim();
        var validacion = validarDirecccionCorreo(email);

        if (validacion) {
            nroCorreo++;
        } else {
            if (email != "")
                return -1;
        }

    }

    if (minimo > nroCorreo)
        return -1;

    if (maximo > 0 && nroCorreo > maximo)
        return -1;

    return 1;
}

function validarDirecccionCorreo(email) {
    var re = /^(([^<>()\[\]\\.,;:\s@"]+(\.[^<>()\[\]\\.,;:\s@"]+)*)|(".+"))@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}])|(([a-zA-Z\-0-9]+\.)+[a-zA-Z]{2,}))$/;
    return re.test(email);
}

function validarNumeroEntero(item, evt) {
    evt = (evt) ? evt : window.event;
    var charCode = (evt.which) ? evt.which : evt.keyCode;

    if (charCode == 46) {
        return false;
    }

    if (charCode > 31 && (charCode < 48 || charCode > 57)) {
        return false;
    }

    return true;
}

function validarContenidoHtml(texto) {
    $("#html_prueba").html(texto);

    var textoSinHtml = ($("#html_prueba").text()).trim();

    return textoSinHtml != "";
}

//Utiles
function cerrarPopup(id) {
    $("#" + id).bPopup().close()
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