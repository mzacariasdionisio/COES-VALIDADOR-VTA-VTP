var controlador = siteRoot + 'stockcombustibles/cumplimiento/';

var hot = null;
//const ValListaCorreoEmpresa = "{Lista_Correos_Empresa}";
const ValFechaIEOD = "{Fecha_IEOD}";
const ValEmpresa = "{Empresa}";
const ValListaFormatoPendiente = "{Lista_Formato_Pendientes}";

var lstVaraiablesCorreo = [
    ValFechaIEOD, ValEmpresa, ValListaFormatoPendiente
];

var arregloSiNo = [
    { id: 'S', text: 'Si' },
    { id: 'N', text: 'No' }
];

const NOTIFICACION = 1;
const VER = 1;
const EDITAR = 2;
const ENVIO_AUTOMATICO = 0;
const ENVIO_MANUAL = 1;
const VARIABLE_ASUNTO = 0;
const VARIABLE_CC = 2;
const VARIABLE_PARA = 3;
const SEPARADOR_INI_VARIABLE = "{";// Debe coincidir con separadores de los valores en ConstantesCalidadProducto
const SEPARADOR_FIN_VARIABLE = "}";// Debe coincidir con separadores de los valores en ConstantesCalidadProducto


$(function () {
    $('#tab-container').easytabs({
        animate: false
    });
    $('#tab-container').easytabs('select', '#plantillas');

    mostrarDetalle($('#hfIdPlantillaCorreo').val(), EDITAR);

    cargarDatosConfiguracion();

    cargarDatosFormato();
});

function mostrarDetalle(plantillacodi, accion) {
    limpiarBarraMensaje("mensaje");
    
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
                mostrarMensaje('mensajePlantilla', 'error', evt.Mensaje);
            }
        },
        error: function (err) {
            mostrarMensaje('mensajePlantilla', 'error', 'Ha ocurrido un error.');
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
    var val_To = objPlantilla.Planticorreos != null ? objPlantilla.Planticorreos : "";
    var val_CC = objPlantilla.PlanticorreosCc != null ? objPlantilla.PlanticorreosCc : "";
    var val_BCC = "{Lista_Correos_Empresa}";
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


     cadena += `                     
                    <tr>
                        <td class="tbform-label">Para (*):</td>
                        <td class="registro-control" style="width:1100px;"><input type="text" name="To" id="To" value="${val_To}" maxlength="100" style="width:1090px; background:${colorFondo}" ${habilitacion}/></td>
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
                <td class="registro-control" style="width:1100px;"><input name="BCC" id="BCC" type="text" value="${val_BCC}" maxlength="120" style="width:1090px; background:##F0F0F0" disabled/></td>
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
                mostrarMensaje('mensajePlantilla', 'error', evt.Mensaje);
            }
        },
        error: function () {
            mostrarMensaje('mensajePlantilla', 'error', 'Ha ocurrido un error.');
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
                    
                    mostrarMensaje('mensajePlantilla', 'exito', "Los cambios fueron guardados exitosamente...");
                    

                } else {
                    mostrarMensaje('mensajePlantilla', 'error', evt.Mensaje);
                }
            },
            error: function (err) {
                mostrarMensaje('mensajePlantilla', 'error', 'Ha ocurrido un error.' + evt.Mensaje);
            }
        });
    } else {
        mostrarMensaje('mensajePlantilla', 'error', msg);
    }
}

function getPlantillaCorreo() {
    var obj = {};

    var idPlantilla = $("#hdIdPlantillaEnDetalle").val();
   
    obj.Planticorreos = $("#To").val(); 
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

    var idPlantilla = $("#hdIdPlantillaEnDetalle").val();
   
    var validaTo = validarCorreo($('#To').val(), 1, -1, "To");
    if (validaTo < 0) {
        msj += "<p>Error encontrado en el campo Para. Revisar que el/los correo(s) sea(n) válido(s) y separados por punto y coma (;).</p>";
    }   


    /*********************************************** validacion del campo CC ***********************************************/
    var validaCc = validarCorreo($('#CC').val(), 0, -1, "Cc");
    if (validaCc < 0) {
        msj += "<p>Error encontrado en el campo CC. Revisar que el/los correo(s) sea(n) válido(s) y separados por punto y coma (;).</p>";
    }


    /*********************************************** validacion del campo BCC ***********************************************/
    /*var validaBcc = validarCorreo($('#BCC').val(), 0, -1, "Bcc");
    if (validaBcc < 0) {
        msj += "<p>Error encontrado en el campo BCC. Revisar que el/los correo(s) sea(n) válido(s) y separados por punto y coma (;).</p>";
    }*/


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


function cargarDatosConfiguracion() {

    $('#btnGrabarConfiguracion').on('click', function () {
        grabarConfiguracionNotificacion();
    });

    $('#cbHoraEjecucion').val($('#hfHoraEjecucion').val());
    $('#cbEstadoEjecucion').val($('#hfEstadoEjecucion').val());
}

function grabarConfiguracionNotificacion() {

    var msg = validarRegistroConfiguracion();
    if (msg == "") {

        $.ajax({
            type: 'POST',
            url: controlador + 'GrabarConfiguracionCorreo',
            data: $('#frmConfiguracion').serialize(),
            dataType: 'json',
            global: false,
            success: function (result) {
                if (result > 0) {
                    mostrarMensaje('mensaheConfiguracion', 'exito', 'Los datos se grabaron correctamente.');
                }
                else {
                    mostrarMensaje('mensaheConfiguracion', 'error', 'Se ha producido un error.');
                }
            },
            error: function () {
                mostrarMensaje('mensaheConfiguracion', 'error', 'Se ha producido un error.');
            }
        });
    }
    else {
        mostrarMensaje('mensaheConfiguracion', 'alert', msg);
    }
}

function validarRegistroConfiguracion() {
    var mensaje = "<ul>";
    var flag = true;

    if ($('#txtFirmante').val() == "") {
        mensaje = mensaje + "<li>Ingrese el nombre del firmante.</li>";
        flag = false;
    }

    if ($('#txtCargo').val() == "") {
        mensaje = mensaje + "<li>Ingrese el cargo del firmante.</li>";
        flag = false;
    }

    if ($('#txtAnexo').val() == "") {
        mensaje = mensaje + "<li>Ingrese el anexo del firmante.</li>";
        flag = false;
    }

    if ($('#cbHoraEjecucion').val() == "") {
        mensaje = mensaje + "<li>Seleccione la hora de ejecución.</li>";
        flag = false;
    }

    if ($('#cbEstadoEjecucion').val() == "") {
        mensaje = mensaje + "<li>Seleccione el estado de ejecución.</li>";
        flag = false;
    }

    mensaje = mensaje + "</ul>";

    if (flag) mensaje = "";

    return mensaje;
}

function cargarDatosFormato() {
    $('#cbTipoEmpresa').on('change', function () {
        cargarEmpresaPorTipo();
    });

    $('#btnConsultaFormato').on('click', function () {
        consultarEmpresaFormato();
    });

    $('#btnGrabarFormato').on('click', function () {
        grabarEmpresaFormato();
    });
}

function cargarEmpresaPorTipo() {
    $.ajax({
        type: 'POST',
        url: controlador + 'GetEmpresaFormato',
        data: {
            idTipoEmpresa: $('#cbTipoEmpresa').val()
        },
        dataType: 'json',
        global: false,
        success: function (result) {
            if (result != -1) {
                $('#cbEmpresa').get(0).options.length = 0;
                $('#cbEmpresa').get(0).options[0] = new Option("-TODOS-", "-1");
                $.each(result, function (i, item) {
                    $('#cbEmpresa').get(0).options[$('#cbEmpresa').get(0).options.length] = new Option(item.Emprnomb, item.Emprcodi);
                });               
            }
            else {
                mostrarMensaje('mensajeFormato', 'error', 'Se ha producido un error.');
            }
        },
        error: function () {
            mostrarMensaje('mensajeFormato', 'error', 'Se ha producido un error.');
        }
    });
}

function consultarEmpresaFormato() {   
    $.ajax({
        type: 'POST',
        url: controlador + 'GetConsuntaFormato',
        data: {
            idTipoEmpresa: $('#cbTipoEmpresa').val(),
            idEmpresa: $('#cbEmpresa').val(),
            idFormato: $('#cbFormato').val()
        },
        dataType: 'json',
        success: function (result) {            
            cargarGrilla(result);               
            
        },
        error: function () {
            mostrarMensaje('mensajeFormato', 'error', 'Se ha producido un error.');
        }
    });       
}

function grabarEmpresaFormato() {
    $.ajax({
        type: 'POST',
        url: controlador + 'GrabarConfiguracionFormato',
        contentType: 'application/json',
        data: JSON.stringify({
            data: getData()
        }),
        dataType: 'json',
        success: function (result) {
            if (result == 1) {
                mostrarMensaje('mensajeFormato', 'exito', 'Los datos fueron grabados correctamente.');
            }
            else {
                mostrarMensaje('mensajeFormato', 'error', 'Se ha producido un error.');
            }
        },
        error: function () {
            mostrarMensaje('mensajeFormato', 'error', 'Se ha producido un error.');
        }
    });
}

function cargarGrilla(result) {

    if (hot != null) hot.destroy();   
    var grilla = document.getElementById('listaFormato');

    hot = new Handsontable(grilla, {
        data: result,
        fixedRowsTop: 2,
        height: 400,      
        colWidths: [1, 300, 150, 150, 150, 150, 150, 150, 150, 150, 150, 150, 150, 150, 150, 150, 150, 150, 150, 150, 150, 150],
        rowHeaders: false,
        cells: function (row, col, prop) {
            var cellProperties = {};

            if (row < 2 ) {
                cellProperties.renderer = tituloRenderer;
                cellProperties.readOnly = true;
            }

            if (col < 2 && row > 1) {
                cellProperties.renderer = subTituloRenderer;
                cellProperties.readOnly = true;
            }

            if (col > 1 && row > 1) {
                cellProperties.editor = 'select2';
                cellProperties.renderer = dropdownSiNoRenderer;
                cellProperties.select2Options = {
                    data: arregloSiNo,
                    dropdownAutoWidth: true,
                    width: 'resolve',
                    allowClear: false
                }
            }

            return cellProperties;
        },
        afterLoadData: function () {
            this.render();
        }
    });
    hot.render();   
}

var tituloRenderer = function (instance, td, row, col, prop, value, cellProperties) {
    Handsontable.renderers.TextRenderer.apply(this, arguments);
    td.style.fontWeight = 'bold';
    td.style.fontSize = '11px';
    td.style.textAlign = 'center';
    td.style.verticalAlign = 'middle';
    td.style.color = '#fff';
    td.style.backgroundColor = '#4C97C3';
};

var subTituloRenderer = function (instance, td, row, col, prop, value, cellProperties) {
    Handsontable.renderers.TextRenderer.apply(this, arguments);
    
    td.style.fontSize = '11px';
    td.style.textAlign = 'left';
    td.style.verticalAlign = 'middle';
    td.style.color = '#6C6C6C';
    td.style.backgroundColor = '#E5E5E5';
};

var dropdownSiNoRenderer = function (instance, td, row, col, prop, value, cellProperties) {
    var selectedId;

    for (var index = 0; index < arregloSiNo.length; index++) {
        if (value === arregloSiNo[index].id) {
            selectedId = arregloSiNo[index].id;
            value = arregloSiNo[index].text;
        }
    }
    $(td).addClass("estilocombo");
    Handsontable.TextCell.renderer.apply(this, arguments);
    $('#selectedId').text(selectedId);
};

function getData() {
    return hot.getData();
}