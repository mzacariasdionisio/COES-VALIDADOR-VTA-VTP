var controlador = siteRoot + 'Equipamiento/FTCorreoManual/';

var IMG_VER = '<img src="' + siteRoot + 'Content/Images/btn-open.png" alt="Ver Mensaje" title="Ver Mensaje" width="19" height="19" style="">';
var IMG_FILE = '<img src="' + siteRoot + 'Content/Images/file.png" alt="Archivo" title="Archivo" width="19" height="19" style="">';

const VER = 1;
const NUEVO = 2;

const ORIGEN_NUEVO = 1;
const ORIGEN_DETALLES = 2;

var miEditor;

$(function () {
    $.fn.dataTable.moment('DD/MM/YYYY HH:mm:ss');

    $('#cbEmpresa').multipleSelect({
        width: '200px',
        filter: true,
    });
    $('#cbEmpresa').multipleSelect('checkAll');

    $('#cbEmpresaCorreo').multipleSelect({
        width: '300px',
        filter: true,
    });
    $('#cbEmpresaCorreo').multipleSelect('checkAll');

    $('#tab-container').easytabs({
        animate: false
    });
    $('#tab-container').easytabs('select', '#tabLista');

    $('#FechaDesde').Zebra_DatePicker({
        format: "d/m/Y",
        pair: $('#FechaHasta'),
        direction: false,
    });

    $('#FechaHasta').Zebra_DatePicker({
        format: "d/m/Y",
        pair: $('#FechaDesde'),
        direction: true,
    });

    $('#btnNuevoMensaje').click(function () {
        $('#cbEmpresaCorreo').multipleSelect('checkAll');
        abrirPopup("popupEmpresa");
    });

    $('#btnSeleccionarEmpresa').click(function () {
        enviarCorreoNuevo();        
    });
    

    $('#btnConsultar').click(function () {
        mostrarListadoMensajes();
    });

    $('#cbProyecto').change(function () {
        
        mostrarEquiposPorProyecto();
    });
    

    mostrarListadoMensajes();

});


function mostrarListadoMensajes() {
    limpiarBarraMensaje("mensaje");

    var filtro = datosFiltro();
    var msg = validarDatosFiltroEnvios(filtro);
    var idEmpr = filtro.todos ? "-1" : $('#hfEmpresa').val()

    if (msg == "") {
        $.ajax({
            type: 'POST',
            url: controlador + "listarCorreosEnviados",
            data: {
                idsEmpresa: idEmpr,
                rangoIni: filtro.rangoIni,
                rangoFin: filtro.rangoFin
            },
            success: function (evt) {
                if (evt.Resultado != "-1") {

                    var htmlListadoCorreos = dibujarTablaPlantillas(evt.ListadoCorreosEnviados);
                    $("#div_listado").html(htmlListadoCorreos);

                    $('#tablaPlantillas').dataTable({
                        "scrollY": 430,
                        "scrollX": false,
                        "sDom": 'ft',
                        "ordering": true,
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
    } else {
        mostrarMensaje('mensaje', 'error', msg);
    }

}



function datosFiltro() {
    var filtro = {};

    var empresa = $('#cbEmpresa').multipleSelect('getSelects');    
    if (empresa == "[object Object]") empresa = "-1";

    //verifico si esta seleccionado TODOS
    var Todos = false;
    var lstSel = [];
    $("#CajaPrincipal input:checkbox[name=selectAllIdEmpresa]:checked").each(function () {
        lstSel.push($(this));
    });
    if (lstSel.length > 0)
        Todos = true;

    $('#hfEmpresa').val(empresa);

    var rangoIni = $("#FechaDesde").val();
    var rangoFin = $('#FechaHasta').val();

    filtro.empresas = empresa;
    filtro.rangoIni = rangoIni;
    filtro.rangoFin = rangoFin;
    filtro.todos = Todos;

    return filtro;
}

function validarDatosFiltroEnvios(datos) {
    var msj = "";

    if (datos.empresas.length == 0) {
        msj += "<p>Debe escoger una empresa correcta.</p>";
    }   

    if (datos.rangoIni == "") {
        if (datos.rangoFin == "") {
            msj += "<p>Debe escoger un rango inicial y final correctos.</p>";
        } else {
            msj += "<p>Debe escoger un rango inicial correcto.</p>";
        }
    } else {
        if (datos.rangoFin == "") {
            msj += "<p>Debe escoger un rango final correcto.</p>";
        } else {
            if (convertirFecha(datos.rangoIni) > convertirFecha(datos.rangoFin)) {
                msj += "<p>Debe escoger un rango correcto, la fecha final debe ser posterior o igual a la fecha inicial.</p>";
            }

        }
    }

    return msj;
}

function enviarCorreoNuevo() {
    limpiarBarraMensaje("mensaje_popupEmpresa");
    var msg = validarDatosNuevo();

    if (msg == "") {
        mostrarDetalle(null, NUEVO, ORIGEN_NUEVO);
        /*cerrarPopup("popupEmpresa");*/
    } else {
        mostrarMensaje('mensaje_popupEmpresa', 'error', msg);
    }

}

function validarDatosNuevo() {

    var empresa = $('#cbEmpresaCorreo').multipleSelect('getSelects');
    if (empresa == "[object Object]") empresa = "-1";

    var msj = "";

    if (empresa.length == 0) {
        msj += "<p>Debe escoger una empresa correcta.</p>";
    }

    return msj;
}

function convertirFecha(fecha) {
    var arrayFecha = fecha.split('/');
    var dia = arrayFecha[0];
    var mes = arrayFecha[1];
    var anio = arrayFecha[2];

    var salida = anio + mes + dia;
    return salida;
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
               <th>Asunto</th>
               <th>Destinatario</th>
               <th>Remitente</th>
               <th>Empresa</th>
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
                    <a href="JavaScript:mostrarDetalle(${item.Corrcodi},${VER}, ${ORIGEN_DETALLES});">${IMG_VER}</a>
                </td>
                <td>${num}</td>
                <td>${item.Corrcodi}</td>
                <td>${item.CorrasuntoDesc}</td>
                <td>${item.Corrto}</td>
                <td>${item.Corrfrom}</td>
                <td>${item.Emprnomb}</td>
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


function mostrarDetalle(correoId, accion, origen) {
    limpiarBarraMensaje("mensaje");

    var id = correoId || 0;
    $('#tab-container').easytabs('select', '#tabDetalle');

    var empresa;
    var emp_;
    if (accion == NUEVO) {
        empresa = $('#cbEmpresaCorreo').multipleSelect('getSelects');
        $('#hfEmpresaCorreo').val(empresa);
        emp = $('#hfEmpresaCorreo').val();
    } else {
        empresa = $('#cbEmpresa').multipleSelect('getSelects');
        $('#hfEmpresa').val(empresa);
        emp = $('#hfEmpresa').val();
    }
    $("#div_detalle").html("");

    $.ajax({
        type: 'POST',
        url: controlador + "ObtenerDatosDelCorreo",
        data: {
            corrcodi: id,
            idsEmpresas: emp,
            accion: accion
        },
        success: function (evt) {
            if (evt.Resultado != "-1") {
                var htmlPlantilla = dibujarPlantilla(evt.Correo, accion);
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

                $('#btnEnviarCorreo').unbind();
                $('#btnEnviarCorreo').click(function () {
                    enviarMensaje();
                });

                //Listamos proyectos
                var listaProyectoPorEmpresas = evt.ListadoProyectos;
                if (listaProyectoPorEmpresas.length > 0) {
                    //usando for
                    $('#cbProyecto').get(0).options[0] = new Option("--  Seleccione Proyecto  --", "0"); //obliga seleccionar para buscar tag
                    for (var i = 0; i < listaProyectoPorEmpresas.length; i++) {
                        $('#cbProyecto').append('<option value=' + listaProyectoPorEmpresas[i].Ftprycodi + '>' + listaProyectoPorEmpresas[i].Ftprynombre + '</option>');  // listaIdentificadores es List<string>, si es objeto usar asi ListaIdentificador[i].Equicodi
                    }

                } else {
                    $('#cbProyecto').get(0).options[0] = new Option("--  Seleccione Proyecto  --", "0");
                }

                if (origen == ORIGEN_NUEVO) {
                    cerrarPopup("popupEmpresa");
                }

            } else {
                if (origen == ORIGEN_NUEVO) {
                    mostrarMensaje('mensaje_popupEmpresa', 'alert', evt.Mensaje);
                } else {
                    if (origen == ORIGEN_DETALLES) {
                        mostrarMensaje('mensaje', 'alert', evt.Mensaje);
                    }
                }
                
                
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
    var colorFondo = "background: white;";

    if (accion == VER) {
        esEditable = false;
        habilitacion = "disabled";
        colorFondo = "background: #E8F0FE;";
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
                    <td class="registro-control" style="width:1100px;"><input type="text" name="From" id="From" value="webapp@coes.org.pe" maxlength="900" style="width:1090px;" disabled /></td>
                </tr>
                <tr>
                    <td class="tbform-label">Para (*):</td>
                    <td class="registro-control" style="width:1100px;"><input type="text" name="To" id="To" value="${val_To}"  style="width:1090px; ${colorFondo}" ${habilitacion}/></td>
                </tr>                 
            
                <tr>
                    <td class="tbform-label">CC (*):</td>
                    <td class="registro-control" style="width:1100px;"><input name="CC" id="CC" type="text" value="${val_CC}"  style="width:1090px; ${colorFondo}"   ${habilitacion}/></td>
                    
                </tr>
                <tr>
                    <td class="tbform-label"> BCC:</td>
                    <td class="registro-control" style="width:1100px;"><input name="BCC" id="BCC" type="text" value="${val_BCC}"  style="width:1090px; ${colorFondo}" ${habilitacion}/></td>
                </tr>
                <tr>
                    <td class="tbform-label">Asunto (*):</td>
                    <td class="registro-control" style="width:1100px;">
                        <textarea  name="Asunto" id="Asuntos" value=""  cols="" rows="3" style="resize: none;width:1090px; ${colorFondo}"  ${habilitacion}>${val_Asunto}</textarea>
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
    `;

    if (accion == NUEVO) {
        cadena += `
                <tr id="notaArchivos">
                    <td class="registro-label">

                    </td>
                    <td class="" colspan="2">
                        <spam><b>Nota: </b><i>Las imágenes y otros archivos a enviar deben ser adjuntados mediante el botón 'Seleccionar un archivo'.</i></spam>
                    </td>

                </tr>
            </table>
        `;
    }

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
                                         ${IMG_FILE} 
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
            'insertfile undo redo | styleselect fontsizeselect | forecolor backcolor | bullist numlist outdent indent | link | table | mybutton | preview',

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
            miEditor = editor;
            editor.addButton('mybutton', {
                type: 'button',
                text: 'Agregar Equipos',
                tooltip: "Ingrese equipos asociados",
                icon: false,
                onclick: agregarEquiposAsociados ,
                //menu: llenarMenus(editor, listaVariables)
            });
            editor.on("paste", function (e) {
                var pastedData = e.clipboardData.getData('text/plain');
                if (pastedData == "") {
                    e.preventDefault();
                    setTimeout(function () {
                        $("#inputText").html('');
                    }, 100)
                }
            });
        },
        
    });

}

function agregarEquiposAsociados() {

    $("#cbProyecto").val("0");
    $("#listadoEquiposA").html("");
    
    abrirPopup("popupEquipos");
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

                    $('#filelist').html($('#filelist').html() + '<div style="width: 940px; word-break: break-all;" class="file-name" id="' + file.id + '">' + file.name +
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


function enviarMensaje() {
    limpiarBarraMensaje("mensaje");
    var correo = {};
    correo = getCorreo();

    var msg = validarCamposCorreoAGuardar(correo);

    if (msg == "") {        
        var empresa = $('#cbEmpresaCorreo').multipleSelect('getSelects'); 
        $('#hfEmpresaCorreo').val(empresa);

        $.ajax({
            type: 'POST',
            url: controlador + 'EnviarCorreo',
            contentType: "application/json",
            dataType: 'json',
            data: JSON.stringify({
                correo: correo,
                idsEmpresas: $('#hfEmpresaCorreo').val(),
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

    if ($('#From').val().trim() != "webapp@coes.org.pe") {
        msj += "<p>Error encontrado en el campo 'De'. El remitente debe ser 'webapp@coes.org.pe'.</p>";
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

    var msg = validarTamanioCaracteres($('#To').val(), "Para", 1000);
    if (msg != "") {
        msj += msg;
    }

    /*********************************************** validacion del campo CC ***********************************************/
    var validaCc = validarCorreo($('#CC').val(), 1, -1);
    if (validaCc < 0) {
        msj += "<p>Error encontrado en el campo 'CC'. Revisar que el/los correo(s) sea(n) válido(s) y separados por punto y coma (;).</p>";
    }
    var msg2 = validarTamanioCaracteres($('#CC').val(), "CC", 1000);
    if (msg2 != "") {
        msj += msg2;
    }


    /*********************************************** validacion del campo BCC ***********************************************/
    var validaBcc = validarCorreo($('#BCC').val(), 0, -1);
    if (validaBcc < 0) {
        msj += "<p>Error encontrado en el campo 'BCC'. Revisar que el/los correo(s) sea(n) válido(s) y separados por punto y coma (;).</p>";
    }
    var msg3 = validarTamanioCaracteres($('#BCC').val(), "BCC", 1000);
    if (msg3 != "") {
        msj += msg3;
    }

    /********************************************** validacion del campo Asunto ***********************************************/
    var asunto_ = correo.Corrasunto.trim();
    if (asunto_ == "") {
        msj += "<p>Error encontrado en el campo 'Asunto'. Debe ingresar Asunto.</p>";
    }
    var msg4 = validarTamanioCaracteres(asunto_, "Asunto", 350);
    if (msg4 != "") {
        msj += msg4;
    }

    /*********************************************** validacion del campo Contenido ***********************************************/
    var contenido_ = correo.Corrcontenido.trim();
    if (contenido_ == "") {
        msj += "<p>Error encontrado en el campo 'Contenido'. Debe ingresar Contenido.</p>";
    }
    //var msg5 = validarMensajeCelda(contenido_);
    //if (msg5 != "") {
    //    msj += msg5;
    //}

    return msj;
}

//function validarMensajeCelda(mensajeHtml) {
//    var salida = "";

//    var textoSinEtiquetas = removerTagsHtml(mensajeHtml);
//    var tamTexto = textoSinEtiquetas.length;

//    if (tamTexto > 2000) {
//        salida = "<p>El mensaje ingresado en 'Contenido' sobrepasa la cantidad de caracteres permitidos (2000 caracteres).</p>";
//    }

//    return salida;
//}

function removerTagsHtml(str) {
    if ((str === null) || (str === ''))
        return "";
    else
        str = str.toString();

    // Expresión regular para identificar etiquetas HTML en la cadena de entrada. Reemplazar la etiqueta HTML identificada con una cadena nula.
    var sinEtiquetas = str.replace(/(<([^>]+)>)/ig, '');

    return sinEtiquetas.replace('&nbsp;', '').trim();
}

function validarTamanioCaracteres(valCadena, nombCampo, tamanio) {
    var salida = "";
    var tam = valCadena.length;

    if (tam > tamanio) {
        salida = "<p>La cantidad de caracteres permitidos en el campo '" + nombCampo + "' fue superado. Solo se permite " + tamanio + " caracteres.</p>";
    }
    return salida;
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

function mostrarEquiposPorProyecto() {
    $("#listadoEquiposA").html("");
    limpiarBarraMensaje("mensaje_popupEquipos");

    var filtro = datosProyecto();
    var msg = validarDatosProyecto(filtro);

    if (msg == "") {
        var empresa = $('#cbEmpresaCorreo').multipleSelect('getSelects');
        $('#hfEmpresaCorreo').val(empresa);

        $.ajax({
            type: 'POST',
            url: controlador + "obtenerEquipoPorProyecto",
            data: {
                idProyecto: filtro.proyectoSeleccionado,
                idsEmpresas: $('#hfEmpresaCorreo').val()
            },
            success: function (evt) {
                if (evt.Resultado != "-1") {
                    mostrarListadoEquiposRelacionados(evt.ListadoRelacionEGP);

                    
                    //Toda la columna cambia (al escoger casilla cabecera)
                    $('input[type=checkbox][name^="checkTodo_E"]').unbind();
                    $('input[type=checkbox][name^="checkTodo_E"]').change(function () {
                        var valorCheck = $(this).prop('checked');
                        $("input[type=checkbox][id^=checkE_]").each(function () {
                            $("#" + this.id).prop("checked", valorCheck);
                        });
                    });

                    //Verifico si el check cabecera debe pintarse o no al editar cualquier check hijo
                    $('input[type=checkbox][name^="checkE_"]').change(function () {
                        verificarCheckGrupalE();
                    });

                    $('#btnAgregarEquipos').unbind();
                    $('#btnAgregarEquipos').click(function () {
                        AgregarEquiposACorreo();
                    });
                    
                } else {
                    mostrarMensaje('mensaje_popupEquipos', 'error', evt.Mensaje);
                }
            },
            error: function (err) {
                mostrarMensaje('mensaje_popupEquipos', 'error', 'Ha ocurrido un error al agregar proyecto.');
            }
        });
    } else {
        mostrarMensaje('mensaje_popupEquipos', 'alert', msg);
    }
}


function datosProyecto() {
    var filtro = {};

    filtro.proyectoSeleccionado = parseInt($("#cbProyecto").val()) || 0;

    return filtro;
}

function validarDatosProyecto(datos) {

    var msj = "";

    if (datos.proyectoSeleccionado == 0) {
        msj += "<p>Debe seleccionar un proyecto.</p>";
    }

    return msj;
}

function verificarCheckGrupalE() {
    //Empresas Interrupcion Suministro con check
    var val1 = 0;
    $("input[type=checkbox][id^=checkE_]").each(function () {
        var IsCheckedIS = $("#" + this.id)[0].checked;
        if (IsCheckedIS) {

        } else {
            val1 = val1 + 1;
        }
    });

    var v = true;
    if (val1 > 0)
        v = false;

    if (!v)
        $("#checkTodo_E").removeAttr('checked');
    else
        $("#checkTodo_E").prop("checked", v);
}

function mostrarListadoEquiposRelacionados(listaEquipos) {
    $("#listadoEquiposA").html("");

    var htmlTabla = dibujarTablaListadoEquiposAsociados(listaEquipos);
    $("#listadoEquiposA").html(htmlTabla);

    var tamAlturaTablaPyE = 200;

    //primero generar datatable (esperar algunos milisegundos para que el div.html() se incruste totalmente en el body)
    setTimeout(function () {
        $('#tablaEquiposA').dataTable({
            "scrollY": tamAlturaTablaPyE,
            "scrollX": false,
            "sDom": 'ft',
            "ordering": false,
            "iDisplayLength": -1
        });
    }, 150);
}


function dibujarTablaListadoEquiposAsociados(listaEquipos) {

    

    var cadena = '';
    cadena += `
    <table border="0" class="pretty tabla-icono" id="tablaEquiposA" style="width: 500px;" >
       <thead>
           <tr style="height: 22px;">                             
               <th style='width:60px; white-space: inherit; '>
                    <input type="checkbox" class="chbx" name="checkTodo_E" id="checkTodo_E" >
                </th>

               <th style='width:60px;'>Código</th>
               <th style='width:380px;'>Equipo</th>   
            </tr>
        </thead>
        <tbody>
    `;

    for (key in listaEquipos) {
        var item = listaEquipos[key];

        var miEquicodi = item.Equicodi != null ? "S" : "N";
        var miGrupocodi = item.Grupocodi != null ? "S" : "N";

        cadena += `
            <tr>
                <td style='white-space: inherit; '>
                    <input type="checkbox" class="chbx" name="checkE_${item.Codigo}_${miEquicodi}_${miGrupocodi}" id="checkE_${item.Codigo}_${miEquicodi}_${miGrupocodi}" value="${item.Codigo}_${miEquicodi}_${miGrupocodi}" >
                </td>
                <td style="">${item.Codigo}</td>
                <td style="">${item.EquipoNomb}</td>                       
           </tr >           
        `;
    }

    cadena += `                   
        </tbody >
    </table >
    `;

    return cadena;
}


function AgregarEquiposACorreo() {
    limpiarBarraMensaje("mensaje_popupEquipos");
    limpiarBarraMensaje("mensaje");

    var filtro = datosElementoAgregar();
    var msg = validarDatosElementoAgregar(filtro);

    if (msg == "") {
        var strIdsSeleccionados = filtro.seleccionados.join(",");
        var proySeleccionado = parseInt($("#cbProyecto").val()) || 0;

        var empresa = $('#cbEmpresaCorreo').multipleSelect('getSelects');
        $('#hfEmpresaCorreo').val(empresa);

        $.ajax({
            type: 'POST',
            url: controlador + "obtenerDatosEquiposSeleccionados",
            data: {
                strIdsSeleccionados: strIdsSeleccionados,
                idProyecto: proySeleccionado,
                idsEmpresas: $('#hfEmpresaCorreo').val()
            },
            success: function (evt) {
                if (evt.Resultado != "-1") {

                    var htmlAgregar = dibujarTablaListadoEquiposSeleccionados(evt.ListadoRelacionEGP);

                    miEditor.insertContent(htmlAgregar);
                    cerrarPopup("popupEquipos");
                } else {
                    mostrarMensaje('mensaje_popupEquipos', 'error', evt.Mensaje);
                }
            },
            error: function (err) {
                mostrarMensaje('mensaje_popupEquipos', 'error', 'Ha ocurrido un error al agregar equipo/grupo.');
            }
        });
    } else {
        mostrarMensaje('mensaje_popupEquipos', 'alert', msg);
    }
}

function datosElementoAgregar() {
    var filtro = {};

    let valoresCheck = [];

    $("input[type=checkbox][name^='checkE_']:checked").each(function () {
        valoresCheck.push(this.value);
    });
    filtro.seleccionados = valoresCheck;

    return filtro;
}

function validarDatosElementoAgregar(datos) {

    var msj = "";

    if (datos.seleccionados.length == 0) {
        msj += "<p>Debe ingresar al menos un Equipo.</p>";
    }

    return msj;
}


function dibujarTablaListadoEquiposSeleccionados(listaEquipos) {

  
    var cadena = '';
    cadena += `
    <table style="height: 39px; width: 1003px; margin-left: auto; margin-right: auto;" border="1">

        <tbody>
            <tr style="height: 13px;">
                <td style="width: 173.5px; height: 13px; background-color: #07548f; text-align: center;"><strong><span style="color: #ffffff;">Empresa Titular</span></strong></td>
                <td style="width: 173.5px; height: 13px; background-color: #07548f; text-align: center;"><strong><span style="color: #ffffff;">Empresa&nbsp;Copropietaria</span></strong></td>
                <td style="width: 173.5px; height: 13px; background-color: #07548f; text-align: center;"><strong><span style="color: #ffffff;">Nombre Proyecto</span></strong></td>
                <td style="width: 173.5px; height: 13px; background-color: #07548f; text-align: center;"><strong><span style="color: #ffffff;">Tipo de equipos o categor&iacute;a</span></strong></td>
                <td style="width: 173.5px; height: 13px; background-color: #07548f; text-align: center;"><strong><span style="color: #ffffff;">Ubicaci&oacute;n</span></strong></td>
                <td style="width: 173.5px; height: 13px; background-color: #07548f; text-align: center;"><strong><span style="color: #ffffff;">Nombre de Equipo</span></strong></td>
            </tr>
                
    `;
    

    for (key in listaEquipos) {
        var item = listaEquipos[key];        

        cadena += `
           <tr style="height: 13px;">
                <td style="width: 173.5px; height: 13px; text-align: center;">${item.EmpresaNomb}</td>
                <td style="width: 146.5px; height: 13px; text-align: center;">${item.EmpresaCoNomb}</td>
                <td style="width: 160px; height: 13px; text-align: center;">${item.Ftprynomb}</td>
                <td style="width: 160px; height: 13px; text-align: center;">${item.Tipo}</td>
                <td style="width: 160px; height: 13px; text-align: center;">${item.Ubicacion}</td>
                <td style="width: 160px; height: 13px; text-align: center;">${item.EquipoNomb}</td>

           </tr >           
        `;

    }
    cadena += "</tbody></table>";


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