var controlador = siteRoot + 'Combustibles/ConfiguracionGas/';

var IMG_VER = '<img src="' + siteRoot + 'Content/Images/btn-open.png" alt="Ver Mensaje" width="19" height="19" style="">';

const VER = 1;
const NUEVO = 2;

$(function () {
    $('#tab-container').easytabs({
        animate: false
    });
    $('#tab-container').easytabs('select', '#tabLista');

    $('#FechaDesde').Zebra_DatePicker({
        format: "d/m/Y",
        pair: $('#FechaHasta'),
        direction: -1,
        onSelect: function () {
            mostrarListadoMensajes();
        }

    });

    $('#FechaHasta').Zebra_DatePicker({
        format: "d/m/Y",
        pair: $('#FechaDesde'),
        direction: 1,
        onSelect: function () {
            mostrarListadoMensajes();
        }
    });

    $('#btnNuevoMensaje').click(function () {
        mostrarDetalle(null, NUEVO);
    });

    mostrarListadoMensajes();

});


function mostrarListadoMensajes() {
    limpiarBarraMensaje("mensaje");

    var filtro = datosFiltro();
    
    $.ajax({
        type: 'POST',
        url: controlador + "listarCorreosEnviados",
        data: {
            rangoIni : filtro.rangoIni,
            rangoFin : filtro.rangoFin
        },
        success: function (evt) {
            if (evt.Resultado != "-1") {
        
                var htmlListadoCorreos = dibujarTablaPlantillas(evt.ListadoCorreosEnviados);
                $("#div_listado").html(htmlListadoCorreos);

                $('#tablaPlantillas').dataTable({
                    "scrollY": 430,
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

function datosFiltro() {
    var filtro = {};

    var rangoIni = $("#FechaDesde").val();
    var rangoFin = $('#FechaHasta').val();

    filtro.rangoIni = rangoIni;
    filtro.rangoFin = rangoFin;

    return filtro;
}

function dibujarTablaPlantillas(listaCorreos) {
    var num = 0;
    
    var cadena = '';
    cadena += `
    <table border="0" class="pretty tabla-icono" id="tablaPlantillas" >
       <thead>
           <tr style="height: 22px;">
               <th style='width:50px;'>Acciones</th>
               <th>N°</th>
               <th>Código de Mensaje</th>
               <th>De</th>
               <th>A</th>
               <th>Asunto</th>
               <th>Usuario quien envío</th>
               <th>Fecha de Envío</th>
            </tr>
        </thead>
        <tbody>
    `;

    for (key in listaCorreos) {
        var item = listaCorreos[key];
        num++;
        cadena += `
            <tr>
                <td style='width:50px;'>        
                    <a href="JavaScript:mostrarDetalle(${item.Corrcodi},${VER});">${IMG_VER}</a>
                </td>
                <td>${num}</td>
                <td>${item.Corrcodi}</td>
                <td>${item.Corrfrom}</td>
                <td>${item.Corrto}</td>
                <td>${item.CorrasuntoDesc}</td>
                <td>${item.Corrusuenvio}</td>
                <td>${item.CorrfechaenvioDesc}</td>
           </tr >           
        `;
    }

    cadena += `                   
        </tbody >
    </table >
    `;

    return cadena;
}


function mostrarDetalle(correoId, accion) {
    limpiarBarraMensaje("mensaje");

    var id = correoId || 0;
    $('#tab-container').easytabs('select', '#tabDetalle');

    $.ajax({
        type: 'POST',
        url: controlador + "ObtenerDatosDelCorreo",
        data: {
            corrcodi: id
        },
        success: function (evt) {
            if (evt.Resultado != "-1") {
                var htmlPlantilla = dibujarPlantilla(evt.Correo,  accion);
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

                cargaDeArchivo();                

                $('#btnEnviarCorreo').click(function () {
                    enviarMensaje();
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

function dibujarPlantilla(objCorreo, accion) {

    var esEditable = true;
    var habilitacion = "";

    if (accion == VER) {
        esEditable = false;
        habilitacion = "disabled";
    }
    
    //Formato 
    var val_De = objCorreo.Corrfrom != null ? objCorreo.Corrfrom : "";
    var val_To = objCorreo.Corrto != null ? objCorreo.Corrto : "";
    var val_CC = objCorreo.Corrcc != null ? objCorreo.Corrcc : "";
    var val_BCC = objCorreo.Corrbcc != null ? objCorreo.Corrbcc : "";
    var val_Asunto = objCorreo.Corrasunto != null ? objCorreo.Corrasunto : "";
    var val_Contenido = objCorreo.Corrcontenido != null ? objCorreo.Corrcontenido : "";
    var val_Archivos = objCorreo.Archivos != null ? objCorreo.Archivos : "";
    
    var cadena = '';
    
    //Agrego nombre del correo
    if (accion == VER) {
        cadena += `
            <div id="informacionPlantilla" style="height:20px; padding-top: 8px; padding-bottom: 12px;" >
                <div class="tbform-label" style="width:120px; float: left; font-size:18px;">
                    Id correo:
                </div>
                <div id="valor" style="float: left; font-size:18px;">
                    ${objCorreo.Corrcodi}
                </div>
            </div>
    `;
    }
    cadena += `
            <table style="width:100%">    
                <tr>
                    <td style="width:120px" class="registro-label"><input type="hidden" id="hfIdPlantillaCorreo" value="${objCorreo.Corrcodi}"/></td>
                </tr>
                <tr>
                    <td style="width:120px" class="tbform-label">De (*):</td>
                    <td class="registro-control" style="width:1100px;"><input type="text" name="From" id="From" value="${val_De}" maxlength="900" style="width:1090px;" ${habilitacion} /></td>
                </tr>
                <tr>
                    <td class="tbform-label">Para (*):</td>
                    <td class="registro-control" style="width:1100px;"><input type="text" name="To" id="To" value="${val_To}" maxlength="100" style="width:1090px;" ${habilitacion}/></td>
                </tr>                 
            
                <tr>
                    <td class="tbform-label">CC:</td>
                    <td class="registro-control" style="width:1100px;"><input name="CC" id="CC" type="text" value="${val_CC}" maxlength="120" style="width:1090px;"   ${habilitacion}/></td>
                    
                </tr>
                <tr>
                    <td class="tbform-label"> BCC:</td>
                    <td class="registro-control" style="width:1100px;"><input name="BCC" id="BCC" type="text" value="${val_BCC}" maxlength="120" style="width:1090px;" ${habilitacion}/></td>
                </tr>
                <tr>
                    <td class="tbform-label">Asunto (*):</td>
                    <td class="registro-control" style="width:1100px;">
                        <textarea maxlength="450" name="Asunto" id="Asuntos" value=""  cols="" rows="3" style="resize: none;width:1090px;"  ${habilitacion}>${val_Asunto}</textarea>
                    </td>
                    
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
            </table>
    `;

    if (accion == NUEVO) {
        
        cadena += `

            <div style="clear:both; height:10px"></div>
            <table class="" style="width:100%">
                <tr>
                    <td class="tbform-label" style="width:120px">Archivos Adjuntados:</td>
                    <td class="registro-control" style ="width: 90px;">
                        <input type="button" id="btnSelectFile" value="Seleccionar un archivo" />
                    </td>
                    <td class="registro-control">
                        
                            
                            <div id="container" class="file-carga" style="padding: 0px 0px 0px 10px; margin-top: -10px; width: 85%; min-height: 0px;">
                                <div id="loadingcarga" class="estado-carga">
                                    <div class="estado-image"><img src="~/Content/Images/loadingtree.gif" /></div>
                                    <div class="estado-text">Cargando...</div>
                                </div>
                                <div id="filelist">No soportado por el navegador.</div>
                            </div>
                            <input id="fileList" type="hidden" name="Archivo" value=""/>
                        
                    </td>
                </tr>
            </table>
            <input type="hidden" id="hfLinkArchivo" name="LinkArchivo" value="@Model.LinkArchivo" />
        `;           

        if (esEditable) {
            cadena += `
        <table style="width:100%">
            <tr>
                
                <td colspan="3" style="padding-top: 10px; text-align: center;">
                    <input type="button" id="btnEnviarCorreo" value="Enviar" />
                    
                </td>
            </tr>
        </table>
        `;
        }
    } else {
        if (accion == VER) {
            cadena += `

            <div style="clear:both; height:10px"></div>
            <table class="" style="width:100%">
                <tr>
                    <td class="tbform-label" style="width:120px">Archivos Adjuntados:</td>
                    <td>
                        <table>
                           
            `;

            if (val_Archivos.trim() != "") {
                var listaArchivo = val_Archivos.split('/');

                for (var i = 0; i < listaArchivo.length; i++) {
                    var item = listaArchivo[i];
                    cadena += `
                                <tr>

                                    <td style="width:30px;">
                                        <img width="15" height="15" src="../../Content/Images/file.png" />
                                    </td>
                                    <td style="text-align:left;">                                        
                                        <h4 onclick="descargarArchivo('${item}',${objCorreo.Corrcodi});" style="cursor:pointer;margin: 3px;width:fit-content;">${item}</h4>
                                    </td>
                                </tr>
                    `;
                }
            }

            cadena += `
                       
                      </table>
                   </td>
                </tr>
            </table>
            `;

        }
    }




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
            'insertfile undo redo | styleselect fontsizeselect | forecolor backcolor | bullist numlist outdent indent | link | table | image | preview',

        menubar: false,
        readonly: sololectura,
        language: 'es',
        statusbar: false,
        convert_urls: false,
        plugin_preview_width: 1000,
        setup: function (editor) {
            editor.on('change',
                function () {
                    editor.save();
                });
            //editor.addButton('mybutton', {
            //    type: 'menubutton',
            //    text: 'Agregar Variables',
            //    tooltip: "Ingrese una variable",
            //    icon: false,
            //    menu: llenarMenus(editor, listaVariables)
            //});

        },
        //automatic_uploads: true,
        ///*
        //  URL of our upload handler (for more details check: https://www.tiny.cloud/docs/configure/file-image-upload/#images_upload_url)
        //  images_upload_url: 'postAcceptor.php',
        //  here we add custom filepicker only to Image dialog
        //*/
        //file_picker_types: 'image',
        ///* and here's our custom image picker*/
        //file_picker_callback: function (cb, value, meta) {
        //    var input = document.createElement('input');
        //    input.setAttribute('type', 'file');
        //    input.setAttribute('accept', 'image/*');

        //    /*
        //      Note: In modern browsers input[type="file"] is functional without
        //      even adding it to the DOM, but that might not be the case in some older
        //      or quirky browsers like IE, so you might want to add it to the DOM
        //      just in case, and visually hide it. And do not forget do remove it
        //      once you do not need it anymore.
        //    */

        //    input.onchange = function () {
        //        var file = this.files[0];

        //        var reader = new FileReader();
        //        reader.onload = function () {
        //            /*
        //              Note: Now we need to register the blob in TinyMCEs image blob
        //              registry. In the next release this part hopefully won't be
        //              necessary, as we are looking to handle it internally.
        //            */
        //            var id = 'blobid' + (new Date()).getTime();
        //            var blobCache = tinymce.activeEditor.editorUpload.blobCache;
        //            var base64 = reader.result.split(',')[1];
        //            var blobInfo = blobCache.create(id, file, base64);
        //            blobCache.add(blobInfo);

        //            /* call the callback and populate the Title field with the file name */
        //            cb(blobInfo.blobUri(), { title: file.name });
        //        };
        //        reader.readAsDataURL(file);
        //    };

        //    input.click();
        //},
    });

}

function enviarMensaje() {
    limpiarBarraMensaje("mensaje");
    var correo = {};
    correo = getCorreo();

    var msg = validarCamposCorreoAGuardar(correo);

    if (msg == "") {
        $.ajax({
            type: 'POST',
            url: controlador + 'EnviarCorreo',
            contentType: "application/json",
            dataType: 'json',
            data: JSON.stringify({
                correo: correo,
            }
            ),
            cache: false,
            success: function (evt) {
                if (evt.Resultado != "-1") {
                    
                    $('#tab-container').easytabs('select', '#tabLista');
                    mostrarMensaje('mensaje', 'exito', "El correo fue enviado exitosamente...");
                    mostrarListadoMensajes();
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


function getCorreo() {
    var obj = {};

    obj.Corrfrom = $("#From").val();
    obj.Corrto = $("#To").val();
    obj.Corrcc = $("#CC").val();
    obj.Corrbcc = $("#BCC").val();
    obj.Corrcontenido = $("#Contenido").val();
    obj.Corrasunto = $("#Asuntos").val();
    obj.Archivos = $("#fileList").val();
    
    return obj;
}

function validarCamposCorreoAGuardar(correo) {
    var msj = "";

    /*********************************************** validacion del campo DE ***********************************************/
    var validaFrom = 0;

    if ($('#From').val().trim() == "") {
        msj += "<p>Error encontrado en el campo 'De'. Debe ingresar remitente.</p>";
    }

    validaFrom = validarCorreo($('#From').val(), 1, 1);
    if (validaFrom < 0) {
        if (validaFrom == -1)
            msj += "<p>Error encontrado en el campo 'De'. Como mínimo debe ingresar un correo válido.</p>";
        if (validaFrom == -2)
            msj += "<p>Error encontrado en el campo 'De'. Como máximo debe ingresar un correo válido.</p>";
    }


    /*********************************************** validacion del campo TO ***********************************************/
    var validaTo = 0;

    if ($('#To').val().trim() == "") {
        msj += "<p>Error encontrado en el campo 'Para'. Debe ingresar Destinatario(s).</p>";
    }

    validaTo = validarCorreo($('#To').val(), 1, -1);
    if (validaTo < 0) {
        msj += "<p>Error encontrado en el campo 'Para'. Revisar que el/los correo(s) sea(n) válido(s) y separados por punto y coma (;).</p>";
    }
    

    /*********************************************** validacion del campo CC ***********************************************/
    var validaCc = validarCorreo($('#CC').val(), 0, -1);
    if (validaCc < 0) {
        msj += "<p>Error encontrado en el campo 'CC'. Revisar que el/los correo(s) sea(n) válido(s) y separados por punto y coma (;).</p>";
    }
    


    /*********************************************** validacion del campo BCC ***********************************************/
    var validaBcc = validarCorreo($('#BCC').val(), 0, -1);
    if (validaBcc < 0) {
        msj += "<p>Error encontrado en el campo 'BCC'. Revisar que el/los correo(s) sea(n) válido(s) y separados por punto y coma (;).</p>";
    }


    /********************************************** validacion del campo Asunto ***********************************************/
    var asunto_ = correo.Corrasunto.trim();
    if (asunto_ == "") {
        msj += "<p>Error encontrado en el campo 'Asunto'. Debe ingresar Asunto.</p>";
    }


    /*********************************************** validacion del campo Contenido ***********************************************/
    var contenido_ = correo.Corrcontenido.trim();
    if (contenido_ == "") {
        msj += "<p>Error encontrado en el campo 'Contenido'. Debe ingresar Contenido.</p>";
    }    


    return msj;
}

function validarCorreo(valCadena, minimo, maximo) {
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
        return -2;

    return 1;
}

function validarDirecccionCorreo(email) {
    var re = /^(([^<>()\[\]\\.,;:\s@"]+(\.[^<>()\[\]\\.,;:\s@"]+)*)|(".+"))@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}])|(([a-zA-Z\-0-9]+\.)+[a-zA-Z]{2,}))$/;
    return re.test(email);
}


cargaDeArchivo = function () {
    limpiarBarraMensaje("mensaje");
    uploader = new plupload.Uploader({
        runtimes: 'html5,flash,silverlight,html4',
        browse_button: 'btnSelectFile',
        container: document.getElementById('container'),
        url: controlador + 'Upload',
        flash_swf_url: '~/Content/Scripts/Moxie.swf',
        silverlight_xap_url: '~/Content/Scripts/Moxie.xap',
        multi_selection: false,
        filters: {
            max_file_size: '20mb',
            mime_types: [
                { title: "Imagenes", extensions: "jpg,jpeg,gif,png" },
                { title: "Archivos comprimidos", extensions: "zip,rar" },
                { title: "Documentos", extensions: "pdf,doc,docx,xls,xlsx,ppt,pptx,csv" }
            ]
        },
        init: {
            PostInit: function () {
                $('#filelist').html('');
                $('#container').css('display', 'none');
                limpiarBarraMensaje("mensaje");
            },
            FilesAdded: function (up, files) {
                limpiarBarraMensaje("mensaje");
                $('#filelist').html('');
                $('#container').css('display', 'block');

                for (i = 0; i < uploader.files.length; i++) {
                    var file = uploader.files[i];

                    $('#filelist').html($('#filelist').html() + '<div class="file-name" id="' + file.id + '">' + file.name +
                        ' (' + plupload.formatSize(file.size) + ') <a class="remove-item" href="JavaScript:eliminarFile(' +
                        '\'' + file.id + '\');">X</a> <b></b></div>');

                    $('#fileList').val($('#fileList').val() + "/" + file.name);
                }
                up.refresh();
                uploader.start();
            },
            UploadProgress: function (up, file) {
                document.getElementById(file.id).getElementsByTagName('b')[0].innerHTML = '<span>' + file.percent + "%</span>";
                $('#' + file.id).css('background-color', '#FFFFB7');
            },
            UploadComplete: function (up, file) {
                $('#loadingcarga').css('display', 'none');                
            },
            FileUploaded: function (up, file, result) {
                if (file.size <= 0)
                    alert("Error: El archivo adjuntado no tiene ningun tamaño valido, vuelva cargar");
            },
            Error: function (up, err) {
                
                if (err.message == "File extension error.") {
                    mostrarMensaje('mensaje', 'alert', "El archivo seleccionado no tiene extensiones permitidas.");
                } else {
                    if (err.message == "File size error.") {
                        mostrarMensaje('mensaje', 'alert', "El archivo adjuntado sobrepasa los 20MB permitidos.");
                    } else {
                        mostrarMensaje('mensaje', 'alert', err.message);
                    }
                }

            }
        }
    });
    uploader.init();

    eliminarFile = function (id) {
        uploader.removeFile(id);
        $('#' + id).remove();
    }

    //return uploader;
}

function descargarArchivo(nomArchivo, corrcodi) {    
    window.location = controlador + 'DescargarArchivoEnvio?fileName=' + nomArchivo + '&corrcodi=' + corrcodi;
    
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