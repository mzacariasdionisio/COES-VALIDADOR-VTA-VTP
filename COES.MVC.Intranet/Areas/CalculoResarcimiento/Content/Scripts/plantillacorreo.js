var controlador = siteRoot + 'calculoresarcimiento/PlantillaCorreo/';


//variables de correos, ayudaran a validarlos campos de Asuntos y Contenido (igual a lo de ConstantesCalidadProducto)
//variables Solicitud de envío de interrupciones 
const ValPeriodo = "{Periodo}";
const ValMesesPeriodo = "{MesesPeriodo}";
const ValAnioPeriodo = "{AnioPeriodo}";
const ValNumPeriodo = "{NumPeriodo}";
const ValMesFinalPeriodo = "{MesFinalPeriodo}";
const ValFecFinE1Periodo = "{FechaEtapa01}";

//variables Solicitud de envío de observaciones a las interrupciones
const ValResponsable = "{NombreResponsable}";
const ValNombrePeriodo = "{NombrePeriodo}";
const ValFecFinE2Periodo = "{FechaEtapa02}";

//variables Solicitud de envío de respuestas a las observaciones        
const ValFecFinE3Periodo = "{FechaEtapa03}";

//variables Solicitud de envío de decisiones de controversias      
const ValFecFinE6Periodo = "{FechaEtapa06}";

//variables Solicitud de envío de compensaciones por mala calidad de producto
const ValMesAnioEvento = "{MesAnioEvento}";
const ValPuntoEntregaEvento = "{PuntoEntregaEvento}";


var lstVaraiablesCorreo = [
    ValPeriodo, ValMesesPeriodo, ValAnioPeriodo, ValNumPeriodo, ValMesFinalPeriodo, ValFecFinE1Periodo, ValResponsable, ValNombrePeriodo, ValFecFinE2Periodo,
    ValFecFinE3Periodo, ValFecFinE6Periodo, ValMesAnioEvento, ValPuntoEntregaEvento
];

const NOTIFICACION = 1;

const VER = 1;
const EDITAR = 2;

const ENVIO_AUTOMATICO = 0;
const ENVIO_MANUAL = 1;

const VARIABLE_ASUNTO = 0;
const VARIABLE_CC = 2;
const VARIABLE_PARA = 3;

const SOLICITUD_ENVIO_INTERRUPCIONES_TRIMESTRAL = "174";
const SOLICITUD_ENVIO_INTERRUPCIONES_SEMESTRAL = "175";

const SEPARADOR_INI_VARIABLE = "{";// Debe coincidir con separadores de los valores en ConstantesCalidadProducto
const SEPARADOR_FIN_VARIABLE = "}";// Debe coincidir con separadores de los valores en ConstantesCalidadProducto

var IMG_EDITAR = '<img src="' + siteRoot + 'Content/Images/btn-edit.png" alt="Editar Envío" width="19" height="19" style="">';
var IMG_VER = '<img src="' + siteRoot + 'Content/Images/btn-open.png" alt="Ver Envío" width="19" height="19" style="">';


$(function () {
    $('#tab-container').easytabs({
        animate: false
    });
    $('#tab-container').easytabs('select', '#tabLista');

    mostrarListadoPlantilla();
});

function mostrarListadoPlantilla() {
    limpiarBarraMensaje("mensaje");

    $.ajax({
        type: 'POST',
        url: controlador + "listarPlantillasCorreos",
        data: { },
        success: function (evt) {
            if (evt.Resultado != "-1") {
                
                var htmlPlantillas = dibujarTablaPlantillas(evt.ListadoPlantillasCorreo, evt.TienePermisoAdmin);
                $("#div_listado").html(htmlPlantillas);

                $('#tablaPlantillas').dataTable({                    
                    "scrollY": 480,
                    "scrollX": false,
                    "sDom": 't',
                    "ordering": false,
                    "iDisplayLength": -1
                });

            } else {
                mostrarMensaje('mensaje', 'error', evt.Mensaje);
            }
        },
        error: function (err) {
            mostrarMensaje('mensaje', 'error', 'Ha ocurrido un error.');
        }
    });
    
}

function dibujarTablaPlantillas(listaPlantillas, esAdmin) {
    var num = 0;
    

    var cadena = '';
    cadena += `
    <table border="0" class="pretty tabla-icono" id="tablaPlantillas" >
       <thead>
           <tr style="height: 22px;">
               <th style='width:90px;'>Acciones</th>
               <th>N°</th>
               <th>Código de Plantilla</th>
               <th>Notificación</th>
               <th>Usuario Modificación</th>
               <th>Fecha de Modificación</th>        
            </tr>
        </thead>
        <tbody>
    `;

    for (key in listaPlantillas) {
        var item = listaPlantillas[key];
        num++;

        var htmlTdAccion = "";
        if (esAdmin) {
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
                <td>${num}</td>
                <td>${item.Plantcodi}</td>
                <td style="text-align:left;">${item.Plantnomb}</td>
                <td>${item.Plantusumodificacion}</td>
                <td>${item.PlantfecmodificacionDesc}</td>
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
    limpiarBarraMensaje("mensaje");
    $('#tab-container').easytabs('select', '#tabDetalle');

    $("#hdIdPlantillaEnDetalle").val(plantillacodi);
    $.ajax({
        type: 'POST',
        url: controlador + "ObtenerDetalleCorreo",
        data: {
            plantillacodi: plantillacodi
            
        },
        success: function (evt) {
            if (evt.Resultado != "-1") {
                

                var htmlPlantilla = dibujarPlantilla(evt.PlantillaCorreo, plantillacodi, accion, ENVIO_AUTOMATICO);
                $("#div_detalle").html(htmlPlantilla);
                
                $.ajax({
                    success: function () {
                        //habilito/deshabilito edicion de contenido
                        var readonly = 0;
                        if (accion == VER) readonly = 1;

                        //muestro editor de contenido
                        iniciarControlTexto('Contenido', evt.ListaVariables, readonly);
                    }
                });               

                $('#btnGuardarPC').click(function () {
                    guardarPlantilla();                    
                });

            } else {
                mostrarMensaje('mensaje', 'error', evt.Mensaje);
            }
        },
        error: function (err) {
            mostrarMensaje('mensaje', 'error', 'Ha ocurrido un error.');
        }
    });
}

function dibujarPlantilla(objPlantilla, plantillacodi, accion, tipoEnvio) {

    var esEditable = true;
    var habilitacion = "";
    var colorFondo = "";

    if (accion == VER) {
        esEditable = false;
        habilitacion = "disabled";
    } else {
        colorFondo = "white";
    }

    
    //Formato 
    
    var val_CC = objPlantilla.PlanticorreosCc != null ? objPlantilla.PlanticorreosCc : "";
    var val_BCC = objPlantilla.PlanticorreosBcc != null ? objPlantilla.PlanticorreosBcc : "";
    var val_Asunto = objPlantilla.Plantasunto != null ? objPlantilla.Plantasunto : "";
    var val_Contenido = objPlantilla.Plantcontenido != null ? objPlantilla.Plantcontenido : "";

   
    //campo De
    var val_De = "";
    if (tipoEnvio == ENVIO_AUTOMATICO)
        val_De = "webapp@coes.org.pe";
  
    var cadena = '';

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
                    
    `;
    

    //Otros (CC, BCC, ASUNTO y CONTENIDO)
    cadena += `                
            
            <tr>
                <td class="tbform-label">CC:</td>
                <td class="registro-control" style="width:1100px;"><input name="CC" id="CC" type="text" value="${val_CC}" maxlength="120" style="width:1090px; background:${colorFondo}"  onfocus="activarBoton('botonAddPropCC');"  onblur="desactivarBoton('botonAddPropCC','CC');" ${habilitacion}/></td>
            </tr>
            <tr>
                <td class="tbform-label"> BCC:</td>
                <td class="registro-control" style="width:1100px;"><input name="BCC" id="BCC" type="text" value="${val_BCC}" maxlength="120" style="width:1090px; background:${colorFondo}" ${habilitacion}/></td>
            </tr>
            <tr>
                <td class="tbform-label">Asunto (*):</td>
                <td class="registro-control" style="width:1100px;">
                    <textarea maxlength="450" name="Asunto" id="Asuntos" value=""  cols="" rows="3" style="resize: none;width:1090px; background:${colorFondo}" onfocus="activarBoton('botonAddPropAsunto');"  onblur="desactivarBoton('botonAddPropAsunto','Asuntos');" ${habilitacion}>${val_Asunto}</textarea>
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

function iniciarControlTexto (id, listaVariables, sololectura) {

    tinymce.remove();

    tinymce.init({
        selector: '#' + id,
        plugins: [
            //'paste textcolor colorpicker textpattern link table image imagetools preview'
            'wordcount advlist anchor autolink codesample colorpicker contextmenu fullscreen image imagetools lists link media noneditable preview  searchreplace table template textcolor visualblocks wordcount'
        ],
        toolbar1:
            //'insertfile undo redo | fontsizeselect bold italic underline strikethrough | forecolor backcolor | alignleft aligncenter alignright alignjustify | bullist numlist outdent indent | link | table | image | mybutton | preview',
            'insertfile undo redo | styleselect fontsizeselect | forecolor backcolor | bullist numlist outdent indent | link | table | image | mybutton | preview',
        
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
            regObj = {};
            var regVariable1 = lista[1];
            regObj.text = regVariable1.Nombre;
            regObj.onclick = function () { e.insertContent(regVariable1.Valor); };
            listaObj.push(regObj);

            if (numVariables >= 3) {
                regObj = {};
                var regVariable2 = lista[2];
                regObj.text = regVariable2.Nombre;
                regObj.onclick = function () { e.insertContent(regVariable2.Valor); };
                listaObj.push(regObj);

                if (numVariables >= 4) {
                    regObj = {};
                    var regVariable3 = lista[3];
                    regObj.text = regVariable3.Nombre;
                    regObj.onclick = function () { e.insertContent(regVariable3.Valor); };
                    listaObj.push(regObj);

                    if (numVariables >= 5) {
                        regObj = {};
                        var regVariable4 = lista[4];
                        regObj.text = regVariable4.Nombre;
                        regObj.onclick = function () { e.insertContent(regVariable4.Valor); };
                        listaObj.push(regObj);

                        if (numVariables >= 6) {
                            regObj = {};
                            var regVariable5 = lista[5];
                            regObj.text = regVariable5.Nombre;
                            regObj.onclick = function () { e.insertContent(regVariable5.Valor); };
                            listaObj.push(regObj);

                            if (numVariables >= 7) {
                                regObj = {};
                                var regVariable6 = lista[6];
                                regObj.text = regVariable6.Nombre;
                                regObj.onclick = function () { e.insertContent(regVariable6.Valor); };
                                listaObj.push(regObj);

                                if (numVariables >= 8) {
                                    regObj = {};
                                    var regVariable7 = lista[7];
                                    regObj.text = regVariable7.Nombre;
                                    regObj.onclick = function () { e.insertContent(regVariable7.Valor); };
                                    listaObj.push(regObj);

                                    if (numVariables >= 9) {
                                        regObj = {};
                                        var regVariable8 = lista[8];
                                        regObj.text = regVariable8.Nombre;
                                        regObj.onclick = function () { e.insertContent(regVariable8.Valor); };
                                        listaObj.push(regObj);

                                        if (numVariables >= 10) {
                                            regObj = {};
                                            var regVariable9 = lista[9];
                                            regObj.text = regVariable9.Nombre;
                                            regObj.onclick = function () { e.insertContent(regVariable9.Valor); };
                                            listaObj.push(regObj);

                                            if (numVariables >= 11) {
                                                regObj = {};
                                                var regVariable10 = lista[10];
                                                regObj.text = regVariable10.Nombre;
                                                regObj.onclick = function () { e.insertContent(regVariable10.Valor); };
                                                listaObj.push(regObj);

                                                if (numVariables >= 12) {
                                                    regObj = {};
                                                    var regVariable11 = lista[11];
                                                    regObj.text = regVariable11.Nombre;
                                                    regObj.onclick = function () { e.insertContent(regVariable11.Valor); };
                                                    listaObj.push(regObj);

                                                    if (numVariables >= 13) {
                                                        regObj = {};
                                                        var regVariable12 = lista[12];
                                                        regObj.text = regVariable12.Nombre;
                                                        regObj.onclick = function () { e.insertContent(regVariable12.Valor); };
                                                        listaObj.push(regObj);

                                                        if (numVariables >= 14) {
                                                            regObj = {};
                                                            var regVariable13 = lista[13];
                                                            regObj.text = regVariable13.Nombre;
                                                            regObj.onclick = function () { e.insertContent(regVariable13.Valor); };
                                                            listaObj.push(regObj);
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
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
    
    $.ajax({
        type: 'POST',
        url: controlador + 'listarVariables',
        data: {
            idPlantilla: idPlantilla,
            campo: tipoCampo
        },
        success: function (evt) {
            if (evt.Resultado != "-1") {
                var htmlVariables = dibujarListadoVariables(evt.ListaVariables);
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
            else {
                mostrarMensaje('mensaje', 'error', evt.Mensaje);
            }
        },
        error: function () {
            mostrarMensaje('mensaje', 'error', 'Ha ocurrido un error.');
        }
    });
}

function dibujarListadoVariables(listaVariables) {

    var cadena = '';
    cadena += `
    <table border="0"  id="tablaCargos">       
        <tr>
            <td>Variable :</td>
            <td>
                <select id="cbVariable" >
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
    limpiarBarraMensaje("mensaje");
    var correo = {};
    correo = getPlantillaCorreo();

    var msg = validarCamposCorreoAGuardar(correo);

    if (msg == "") {
        $.ajax({
            type: 'POST',
            url: controlador + 'GuardarPlantillaCorreo',
            contentType: "application/json",
            dataType: 'json',
            data: JSON.stringify({
                plantillaCorreo: correo,
            }
            ),
            cache: false,
            success: function (evt) {
                if (evt.Resultado != "-1") {
                    //cerrarPopup("popupEditarCentral");

                    //muestra el listado respectivo                    
                    $('#tab-container').easytabs('select', '#tabLista');
                    mostrarMensaje('mensaje', 'exito', "Los cambios fueron guardados exitosamente...");
                    mostrarListadoPlantilla();
                    $("#div_detalle").html("");

                } else {
                    mostrarMensaje('mensaje', 'error', evt.Mensaje);
                }
            },
            error: function (err) {
                mostrarMensaje('mensaje', 'error', 'Ha ocurrido un error.' + evt.Mensaje);
            }
        });
    } else {
        mostrarMensaje('mensaje', 'error', msg);
    }
}

function getPlantillaCorreo() {
    var obj = {};        

    //otros
    obj.Plantcodi = $("#hfIdPlantillaCorreo").val();
    obj.Plantcontenido = $("#Contenido").val();
    obj.Plantasunto = $("#Asuntos").val();        
    obj.PlanticorreosCc = $("#CC").val();
    obj.PlanticorreosBcc = $("#BCC").val();
    //obj.PlanticorreoFrom = $("#From").val();        

    return obj;
}

function validarCamposCorreoAGuardar(correo) {
    var msj = "";  
    

    /*********************************************** validacion del campo CC ***********************************************/
    var validaCc = validarCorreo($('#CC').val(), 0, -1, "Cc");
    if (validaCc < 0) {
        msj += "<p>Error encontrado en el campo CC. Revisar que el/los correo(s) sea(n) válido(s) y separados por punto y coma (;).</p>";
    }
  

    /*********************************************** validacion del campo BCC ***********************************************/
    var validaBcc = validarCorreo($('#BCC').val(), 0, -1, "Bcc");
    if (validaBcc < 0) {
        msj += "<p>Error encontrado en el campo BCC. Revisar que el/los correo(s) sea(n) válido(s) y separados por punto y coma (;).</p>";
    }


    /********************************************** validacion del campo Asunto ***********************************************/
    var asunto_ = correo.Plantasunto.trim();
    if (asunto_ == "") {
        msj += "<p>Error encontrado en el campo Asunto. Debe ingresar Asunto.</p>";
    }
    
    var tienenMismaCantidadAsunto = validarCantidadDeSeparadores(asunto_);
       
    if (!tienenMismaCantidadAsunto) {
        msj += "<p>Revisar campo Asunto, la cantidad de caracteres '{' no coincide con la cantidad de caracteres '}'. </p>";
    }

    //valido que las palabras dentro de un { } sean variables admitidas
    msj = validarVariablesCorrectas(asunto_, "Asunto", lstVaraiablesCorreo, msj);  


    /*********************************************** validacion del campo Contenido ***********************************************/

    var contenido_ = correo.Plantcontenido.trim();
    if (contenido_ == "") {
        msj += "<p>Ingrese Contenido.</p>";
    }  
    var tienenMismaCantidadContenido = validarCantidadDeSeparadores(contenido_);

    
    if (!tienenMismaCantidadContenido) {
        msj += "<p>Revisar campo Contenido, la cantidad de caracteres '{' no coincide con la cantidad de caracteres '}'. </p>";
    }

    //valido que las palabras dentro de un {} sean variables admitidas   
    msj = validarVariablesCorrectas(contenido_, "Contenido", lstVaraiablesCorreo, msj);
    
        
    return msj;
}

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


function validarVariablesCorrectas(valorCampo, campo, lstVariables, msj) {
    //validando textos tipo: {texto}   
    const regex = /{\w+}/g;
    const lstPalabrasDentroParentesis = valorCampo.match(regex);
    if (lstPalabrasDentroParentesis != null) {
        let lstDiferentes = lstPalabrasDentroParentesis.filter(x => !lstVariables.includes(x));
        if (lstDiferentes.length > 0) {
            if (campo == "To")
                campo = "Para";
            msj += "<p>Revisar campo " + campo + ". Se detectó variables ( texto dentro de { } ) no reconocidas. </p>";
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

function validarCorreo(valCadena, minimo, maximo, campo) {
    var cadena = valCadena;
    
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