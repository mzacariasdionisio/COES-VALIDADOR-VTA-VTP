var TIPO_INCLUSION = 'I';
var TIPO_EXCLUSION = 'E';

//////////////////////////////////////////////////////////
/// Transferir antes de exclusion
async function st_VerificarObligatoriedadTransferir() {
    var idTipoprog = parseInt($("#idTipoProgramacion").val()) || 0;
    var progrcodi = document.getElementById('Progrcodi').value;

    if (EVENCLASECODI_SEMANAL == idTipoprog || EVENCLASECODI_DIARIO == idTipoprog) {

        return $.ajax({
            type: 'POST',
            url: controler + 'ObtenerNumeroIntervencionHorizonteSuperior',
            data: {
                idProgramacion: progrcodi
            },
            dataType: 'json',
            success: function (model) {
                ES_OBIGLATORIO_TRANSFERIR = (model.Total > 0);
            },
            error: function () {
                alert('Ha ocurrido un error');
            }
        });
    }
}

//////////////////////////////////////////////////////////
/// Formulario de 1 intervención
async function st_VerificarIntervencionPuedeTenerSustento(idTipoprog) {

    if (EVENCLASECODI_SEMANAL == idTipoprog || EVENCLASECODI_DIARIO == idTipoprog) {
        limpiarMensaje();
        st_mostrarBtnEnFormularioPrincipal();

        var dataJson = JSON.stringify(INTERVENCION_WEB).replace(/\/Date/g, "\\\/Date").replace(/\)\//g, "\)\\\/");

        return await $.ajax({
            type: 'POST',
            url: controlador + "VerificarIntervencionPuedeTenerJustificacion",
            dataType: 'json',
            data: {
                dataJson: dataJson,
            },
            success: function (model) {
                if (model.Resultado != "-1") {
                    var regIncl = model.IntervencionIncl;
                    var regExcl = model.IntervencionExcl;
                    st_SetearPlantillaFormularioIntervencion(regIncl, TIPO_INCLUSION);
                    st_SetearPlantillaFormularioIntervencion(regExcl, TIPO_EXCLUSION);
                } else {
                    PLANTILLA_FORMULARIO_INCLUSION = null;
                    PLANTILLA_FORMULARIO_EXCLUSION = null;
                }

                st_mostrarBtnEnFormularioPrincipal();
            },
            error: function (err) {
            }
        });
    }
}

function st_SetearPlantillaFormularioIntervencion(regPlt, tipo) {
    if (tipo == TIPO_INCLUSION) {
        //se cambió datos y ya no es necesario plantilla
        if (regPlt == null) {
            PLANTILLA_FORMULARIO_INCLUSION = null;
        }
        else {
            if (PLANTILLA_FORMULARIO_INCLUSION != null && PLANTILLA_FORMULARIO_INCLUSION.Sustento != null) { //si ya estuvo precargado
                if (regPlt.Sustento.Inpsttipo != PLANTILLA_FORMULARIO_INCLUSION.Sustento.Inpsttipo) {
                    PLANTILLA_FORMULARIO_INCLUSION = regPlt;
                }
            }
            else {
                //primer registro de formulario
                PLANTILLA_FORMULARIO_INCLUSION = regPlt;
            }

            //actualizar los datos del formulario web
            if (PLANTILLA_FORMULARIO_INCLUSION != null) {
                var sustentoTmp = PLANTILLA_FORMULARIO_INCLUSION.Sustento;
                PLANTILLA_FORMULARIO_INCLUSION = JSON.parse(JSON.stringify(INTERVENCION_WEB));
                PLANTILLA_FORMULARIO_INCLUSION.Sustento = sustentoTmp;
            }
        }
    } else {
        //se cambió datos y ya no es necesario plantilla
        if (regPlt == null) {
            PLANTILLA_FORMULARIO_EXCLUSION = null;
        }
        else {
            if (PLANTILLA_FORMULARIO_EXCLUSION != null && PLANTILLA_FORMULARIO_EXCLUSION.Sustento != null) { //si ya estuvo precargado
                if (regPlt.Sustento.Inpsttipo != PLANTILLA_FORMULARIO_EXCLUSION.Sustento.Inpsttipo) {
                    PLANTILLA_FORMULARIO_EXCLUSION = regPlt;
                }
            }
            else {
                //primer registro de formulario
                PLANTILLA_FORMULARIO_EXCLUSION = regPlt;
            }
        }
    }
}

function st_FlagTieneFormularioCompleto() {
    if (PLANTILLA_FORMULARIO_INCLUSION == null && PLANTILLA_FORMULARIO_EXCLUSION == null) {
        return true;
    } else {
        var estaCompleto = true;
        if (PLANTILLA_FORMULARIO_INCLUSION != null)
            estaCompleto = estaCompleto && PLANTILLA_FORMULARIO_INCLUSION.Sustento.TienePlantillaCompleta;

        if (PLANTILLA_FORMULARIO_EXCLUSION != null)
            estaCompleto = estaCompleto && PLANTILLA_FORMULARIO_EXCLUSION.Sustento.TienePlantillaCompleta;

        return estaCompleto;
    }
}

function st_mostrarBtnEnFormularioPrincipal() {

    $("#trSustento").hide();
    $("#btnSustentoExclPopup").hide();
    $("#btnSustentoInclPopup").hide();

    if (PLANTILLA_FORMULARIO_INCLUSION != null) {
        $("#trSustento").show();
        $("#btnSustentoInclPopup").show();
    }

    if (PLANTILLA_FORMULARIO_EXCLUSION != null) {
        $("#trSustento").show();
        $("#btnSustentoExclPopup").show();
    }

    //TODO cambiar mensaje segun OPCION_ACTUAL
    var listaMsj = [];
    if (PLANTILLA_FORMULARIO_INCLUSION != null && PLANTILLA_FORMULARIO_INCLUSION.Sustento != null && !PLANTILLA_FORMULARIO_INCLUSION.Sustento.TienePlantillaCompleta) listaMsj.push("<li>Debe ingresar Sustento de Inclusión.</li>");
    if (PLANTILLA_FORMULARIO_EXCLUSION != null && PLANTILLA_FORMULARIO_EXCLUSION.Sustento != null && !PLANTILLA_FORMULARIO_EXCLUSION.Sustento.TienePlantillaCompleta) listaMsj.push("<li>Debe ingresar Sustento de Exclusión.</li>");

    if (listaMsj.length > 0) {
        var msjAlerta = "<ul>" + listaMsj.join('') + "</ul>";
        mostrarAlerta(msjAlerta);
    }
}

function st_CargarFormularioSustento(tipo, intercodiEliminar, nroItem) {
    var objPlt = st_ObtenerPlantilla(tipo, intercodiEliminar, nroItem);

    //generar html
    var txtHtml = st_GenerarHtmlFormulario(tipo, objPlt, intercodiEliminar, nroItem);
    $('#popupFormSustento').html(txtHtml);

    //setear evento carga archivo
    LISTA_SECCION_ARCHIVO_X_REQUISITO = [];
    var listaReq = objPlt.Sustento.ListaItem;
    for (var i = 0; i < listaReq.length; i++) {
        var req = listaReq[i];
        if (req.PuedeCargarArchivoSoloFoto || req.PuedeCargarArchivo) {

            var seccion = {
                Inpstidesc: '',
                EsEditable: false,
                ListaArchivo: req.ListaArchivo ?? [],
                Modulo: TIPO_MODULO_INTERVENCION,
                Progrcodi: objPlt.Progrcodi,
                Carpetafiles: objPlt.Intercarpetafiles,
                Subcarpetafiles: req.Inpsticodi,
                TipoArchivo: TIPO_ARCHIVO_SUSTENTO,
                PuedeCargarArchivoSoloFoto: req.PuedeCargarArchivoSoloFoto,
                IdDiv: `html_archivos_item_${req.Inpsticodi}`,
                IdDivVistaPrevia: `vistaprevia_sustento`,
                IdPrefijo: arch_getIdPrefijo(req.Inpsticodi)
            };
            LISTA_SECCION_ARCHIVO_X_REQUISITO.push(seccion);

            arch_cargarHtmlArchivoEnPrograma(seccion.IdDiv, seccion);
        }
    }

    //importar excel sustento
    st_AdjuntarFormatoSustento();

    //mostrar popup
    $('#popupFormSustento').bPopup({
        easing: 'easeOutBack',
        speed: 450,
        transition: 'slideDown'
    });
}

function st_GenerarHtmlFormulario(tipo, objPlt, intercodiEliminar, nroItem) {
    var htmlTitulo = TIPO_INCLUSION == tipo ? 'Informe de Inclusión' : 'Informe de Exclusión';
    var esEditable = false; //en Intranet no es editable el sustento registrado por el agente

    var htmlGuardar = `
        <input type="button" value="Descargar plantilla" id="btnSustentoDescargar" style='' onclick="st_DescargarExcelSustento()">
    `;
    if (esEditable)
        htmlGuardar += `
        <input type="button" value="Adjuntar formato justificación" id="btnSustentoSubir" style=''>

        <input type="button" value="Guardar" id="btnGuardarPopupSustento" style='float: right;' onclick="st_GuardarContenidoPlantilla()">
        `;

    var txtHtml = `
    <div class="popup-title">
        <span>${htmlTitulo}</span>

        <input type="button" id="bSalirSustento" value="Salir" class='b-close' style='float: right; margin-right: 35px;' />
        ${htmlGuardar}

        <input type="hidden" value="${tipo}" id="hdTipoPopupSustento">     
        <input type="hidden" value="${intercodiEliminar}" id="hdIntercodiEliminarSustento">    
        <input type="hidden" value="${nroItem}" id="hdNroItem">                     
    </div>
    
    <div class="content-registro" style="height: 585px; overflow-y: auto">
    
        <h4>${objPlt.Nomprogramacion}</h4>

        <div id="mensaje3" class="action-alert" style="margin: 10px 0px 10px; display: none;"></div>

        <div>
        
            <table cellpadding="2" style="width:auto; margin-top: 10px; margin-bottom: 20px;">
                <tbody>
                    <tr>
		                <td class="registro-label" style="width:150px;">
                            Empresa:
                        </td>
		                <td class="registro-control"> 
                            ${objPlt.EmprNomb}
                        </td>
                    </tr>

                    <tr>
		                <td class="registro-label" style="">
                            Equipo:
                        </td>
		                <td class="registro-control">
                            ${objPlt.AreaNomb} ${objPlt.Equiabrev}
                        </td>
                    </tr>

                    <tr>
		                <td class="registro-label" style="">
                            Descripción:
                        </td>
		                <td class="registro-control">
                            ${objPlt.Interdescrip}
                        </td>
                    </tr>

                    <tr>
		                <td class="registro-label" style="">
                            Fecha de inicio:
                        </td>
		                <td class="registro-control">
                            ${objPlt.InterfechainiDesc}
                        </td>
                    </tr>

                    <tr>
		                <td class="registro-label" style="">
                            Fecha de fin:
                        </td>
		                <td class="registro-control">
                            ${objPlt.InterfechafinDesc}
                        </td>
                    </tr>

                    <tr>
		                <td class="registro-label" style="">
                            Tipo de Intervención:
                        </td>
		                <td class="registro-control">
                            ${objPlt.TipoEvenDesc}
                        </td>
                    </tr>
                </tbody>
            </table>

        </div>

        <table cellpadding="2" style="width:auto; margin-top: 10px;">
            <tbody>
    `;

    var listaReq = objPlt.Sustento.ListaItem;
    for (var i = 0; i < listaReq.length; i++) {
        var req = listaReq[i];

        var htmlRpta = '';
        if (!esEditable) {
            if (!req.PuedeCargarArchivoSoloFoto && !req.PuedeCargarArchivo) {
                htmlRpta += `
                        <div style="background-color: white; width:450px;" > 
                            ${req.Instdrpta}
                        </div>
                `;
            } else {
                htmlRpta += `
                    <div id="html_archivos_item_${req.Inpsticodi}" style="margin-top: 10px;">

                    </div>
                `;
            }
        } else {

            if (!req.PuedeCargarArchivoSoloFoto && !req.PuedeCargarArchivo) {
                htmlRpta += `
                    <textarea id="req_item_${req.Inpsticodi}" style="background-color: white; width:450px; height:44px;margin-right: 10px;resize:none">${req.Instdrpta}</textarea>
                `;
            } else {
                htmlRpta += `
                    <div id="html_archivos_item_${req.Inpsticodi}" style="margin-top: 10px;">

                    </div>
                `;
            }
        }

        txtHtml += `
            <tr>
		        <td class="registro-label" style="width:450px;">
                    ${req.Inpstidesc}
                </td>
		        <td class="registro-control"> 
                    ${htmlRpta}
                </td>
            </tr>
        `;
    }

    txtHtml += `
            </tbody>
        </table>

        <div>
            <iframe id="vistaprevia_sustento" style="width: 100%; height:500px;" frameborder="0" hidden></iframe>
        </div>

    </div>

    `;

    return txtHtml;
}

function st_GuardarContenidoPlantilla() {
    st_limpiarMensaje();
    var tipo = $("#hdTipoPopupSustento").val();
    var intercodiEliminar = parseInt($("#hdIntercodiEliminarSustento").val()) || 0;
    var nroItem = parseInt($("#hdNroItem").val()) || 0;

    var objPlt = st_ObtenerPlantilla(tipo, intercodiEliminar, nroItem);
    var listaMsj = st_ValidarContenidoPlantilla(objPlt);
    if (listaMsj.length == 0) {
        //recorrer cada item del formulario
        var listaReq = objPlt.Sustento.ListaItem;
        for (var i = 0; i < listaReq.length; i++) {
            var req = listaReq[i];

            //texto
            if (!req.PuedeCargarArchivoSoloFoto && !req.PuedeCargarArchivo) {
                var valorTxt = $("#req_item_" + req.Inpsticodi).val().trim();
                req.Instdrpta = valorTxt;
            } else {
                req.ListaArchivo = arch_obtenerSeccionXSubcarpeta(TIPO_MODULO_INTERVENCION, req.Inpsticodi).ListaArchivo;
            }
        }
        objPlt.Sustento.TienePlantillaCompleta = true;

        //quitar mensaje de formulario de intervencion
        if (intercodiEliminar <= 0 && nroItem <= 0) limpiarMensaje();
        //cerrar popup
        $('#popupFormSustento').bPopup().close();
    } else {
        var msjAlerta = "<ul>" + listaMsj.join('') + "</ul>";
        st_mostrarAlerta(msjAlerta);
    }
}

function st_ObtenerPlantilla(tipo, intercodiEliminar, nroItem) {
    var objPlt = null;

    if (nroItem > 0) {
        //ventana flotante de intervenciones a eliminar
        var lista = TIPO_INCLUSION == tipo ? LISTA_PLANTILLA_FORMULARIO_INCLUSION : LISTA_PLANTILLA_FORMULARIO_EXCLUSION;
        objPlt = lista.find(x => x.NroItem == nroItem);
    } else {
        if (intercodiEliminar == 0) {
            //ventana flotante cuando se crea un nuevo o edita
            objPlt = TIPO_INCLUSION == tipo ? PLANTILLA_FORMULARIO_INCLUSION : PLANTILLA_FORMULARIO_EXCLUSION;
        } else {
            //ventana flotante de intervenciones a eliminar
            objPlt = LISTA_PLANTILLA_FORMULARIO_EXCLUSION.find(x => x.Intercodi == intercodiEliminar);
        }
    }

    return objPlt;
}

function st_ValidarContenidoPlantilla(objPlt) {
    var listaMsj = [];

    var listaReq = objPlt.Sustento.ListaItem;
    for (var i = 0; i < listaReq.length; i++) {
        var req = listaReq[i];

        //texto
        if (!req.PuedeCargarArchivoSoloFoto && !req.PuedeCargarArchivo) {
            var valorTxt = $("#req_item_" + req.Inpsticodi).val().trim() ?? "";
            if (valorTxt.length == 0) listaMsj.push("<li>Ítem '" + req.Inpstidesc + "': Debe ingresar texto.</li>");
            if (valorTxt.length > 1000) listaMsj.push("<li>Ítem '" + req.Inpstidesc + "'. El texto no debe exceder los 1000 caracteres.</li>");
        } else {
            //archivos
            var listaArchivoXReq = arch_obtenerSeccionXSubcarpeta(TIPO_MODULO_INTERVENCION, req.Inpsticodi).ListaArchivo;
            if (listaArchivoXReq == null || listaArchivoXReq.length == 0) listaMsj.push("<li>Debe cargar archivos en el item '" + req.Inpstidesc + "'.</li>");
        }
    }

    return listaMsj;
}

function st_ClonarPlantillaFormulario(regIn, intercodiEliminar, nroItem) {
    //clonar objeto
    var regClone = JSON.parse(JSON.stringify(regIn));
    regClone.Intercodi = intercodiEliminar;
    regClone.NroItem = nroItem;

    //setear los datos que estan en el formulario
    if (false) {

        var listaReq = regClone.Sustento.ListaItem;
        for (var i = 0; i < listaReq.length; i++) {
            var req = listaReq[i];

            //texto
            if (!req.PuedeCargarArchivoSoloFoto && !req.PuedeCargarArchivo) {
                var valorTxt = $("#req_item_" + req.Inpsticodi).val().trim();
                req.Instdrpta = valorTxt;
            } else {
                req.ListaArchivo = arch_obtenerSeccionXSubcarpeta(TIPO_MODULO_INTERVENCION, req.Inpsticodi).ListaArchivo;
            }
        }
    }

    return regClone;
}

function st_DescargarExcelSustento() {
    var tipo = $("#hdTipoPopupSustento").val();
    var intercodiEliminar = parseInt($("#hdIntercodiEliminarSustento").val()) || 0;
    var nroItem = parseInt($("#hdNroItem").val()) || 0;

    var regIn = st_ObtenerPlantilla(tipo, intercodiEliminar, nroItem);
    var regClone = st_ClonarPlantillaFormulario(regIn, intercodiEliminar, nroItem);

    var dataJson = JSON.stringify(regClone).replace(/\/Date/g, "\\\/Date").replace(/\)\//g, "\)\\\/");

    $.ajax({
        type: 'POST',
        url: controler + 'DescargarExcelSustento',
        data: {
            dataJson: dataJson
        },
        success: function (evt) {
            if (evt.Resultado != "-1") {
                window.location = controler + "AbrirArchivo?file=" + evt.NombreArchivo;
            }
            else {
                alert('Ha ocurrido un error');
            }
        },
        error: function (err) {
            alert('Ha ocurrido un error');
        }
    });
}

function st_AdjuntarFormatoSustento() {

    var uploaderN = new plupload.Uploader({
        runtimes: 'html5,flash,silverlight,html4',
        browse_button: 'btnSustentoSubir',
        container: document.getElementById('container'),
        url: controler + 'UploadExcelSustento',
        flash_swf_url: '~/Content/Scripts/Moxie.swf',
        silverlight_xap_url: '~/Content/Scripts/Moxie.xap',
        multi_selection: false,
        filters: {
            max_file_size: '50mb',
            mime_types: [
                { title: "Archivos Excel .xlsx", extensions: "xlsx" },
            ]
        },
        init: {
            PostInit: function () {
            },

            FilesAdded: function (up, files) {
                uploaderN.start();
            },
            UploadProgress: function (up, file) {
            },
            UploadComplete: function (up, file) {
            },
            FileUploaded: function (up, file, result) {
                st_CargarContenidoArchivoAFormulario(JSON.parse(result.response).nuevonombre);
            },
            Error: function (up, err) {
                $('#container').css('display', 'true');
                if (err.code == -601) {
                    document.getElementById('filelist').innerHTML = '<div class="action-alert">' + 'La extensión del archivo no es válido' + '</div>';
                }
            }
        }
    });

    uploaderN.init();
}

function st_CargarContenidoArchivoAFormulario(fileName) {
    st_limpiarMensaje();
    var tipo = $("#hdTipoPopupSustento").val();
    var intercodiEliminar = parseInt($("#hdIntercodiEliminarSustento").val()) || 0;
    var nroItem = parseInt($("#hdNroItem").val()) || 0;

    var regIn = st_ObtenerPlantilla(tipo, intercodiEliminar, nroItem);
    var regClone = st_ClonarPlantillaFormulario(regIn, intercodiEliminar, nroItem);

    var dataJson = JSON.stringify(regClone).replace(/\/Date/g, "\\\/Date").replace(/\)\//g, "\)\\\/");

    $.ajax({
        type: 'POST',
        url: controler + 'ListarDatosExcelSustento',
        data: {
            progrcodi: regIn.Progrcodi,
            intercarpetafiles: regIn.Intercarpetafiles,
            dataJson: dataJson,
            nombreArchivoUpload: fileName
        },
        success: function (evt) {
            if (evt.Resultado != "-1") {
                //hay mensajes de error
                if (evt.ListaMensaje != null && evt.ListaMensaje.length > 0) {
                    var listaMsj = evt.ListaMensaje;
                    for (var i = 0; i < listaMsj.length; i++) {
                        listaMsj[i] = "<li>" + listaMsj[i] + "</li>";
                    }
                    var msjAlerta = "<ul>" + listaMsj.join('') + "</ul>";
                    st_mostrarAlerta(msjAlerta);
                } else {
                    //actualizar formulario

                    var listaReq = evt.IntervencionImportada.Sustento.ListaItem;

                    for (var i = 0; i < listaReq.length; i++) {
                        var req = listaReq[i];
                        if (req.PuedeCargarArchivoSoloFoto || req.PuedeCargarArchivo) {
                            var seccion = arch_obtenerSeccionXSubcarpeta(TIPO_MODULO_INTERVENCION, req.Inpsticodi);
                            seccion.ListaArchivo = req.ListaArchivo ?? [];
                            $("#listaArchivos" + seccion.IdPrefijo).html(arch_generarTablaListaBody(seccion));
                        } else {
                            $("#req_item_" + req.Inpsticodi).val(req.Instdrpta);
                        }
                    }
                }
            }
            else {
                alert('Ha ocurrido un error');
            }
        },
        error: function (err) {
            alert('Ha ocurrido un error');
        }
    });
}

//////////////////////////////////////////////////////////
/// Eliminación de intervenciones
async function st_VerificarListaIntervencionPuedeTenerSustento() {
    st_mostrarTablaFormularioListado();

    if (LISTA_PLANTILLA_FORMULARIO_EXCLUSION == null || LISTA_PLANTILLA_FORMULARIO_EXCLUSION.length == 0) {
        var idTipoprog = parseInt($("#idTipoProgramacion").val()) || 0;
        var progrcodi = parseInt($("#Progrcodi").val()) || 0;
        var intercodis = listarIntercodiChecked();

        if (EVENCLASECODI_SEMANAL == idTipoprog || EVENCLASECODI_DIARIO == idTipoprog) {

            return await $.ajax({
                type: 'POST',
                url: controlador + "VerificarListaIntervencionPuedeTenerJustificacion",
                dataType: 'json',
                data: {
                    intercodis: intercodis,
                    evenclasecodi: idTipoprog,
                    progrcodi: progrcodi
                },
                success: function (model) {
                    if (model.Resultado != "-1") {
                        LISTA_PLANTILLA_FORMULARIO_EXCLUSION = model.ListaExclusion;
                    } else {
                        LISTA_PLANTILLA_FORMULARIO_EXCLUSION = [];
                    }

                    st_mostrarTablaFormularioListado();
                },
                error: function (err) {
                }
            });
        }
    }
}

function st_FlagTieneListaFormularioCompleto() {
    if (LISTA_PLANTILLA_FORMULARIO_EXCLUSION == null || LISTA_PLANTILLA_FORMULARIO_EXCLUSION.length == 0) {
        return true;
    } else {
        var estaCompleto = true;

        for (var i = 0; i < LISTA_PLANTILLA_FORMULARIO_EXCLUSION.length; i++) {
            var regSust = LISTA_PLANTILLA_FORMULARIO_EXCLUSION[i].Sustento;

            estaCompleto = estaCompleto && regSust.TienePlantillaCompleta;
        }

        return estaCompleto;
    }
}

function st_mostrarTablaFormularioListado() {

    $("#div_lista_int_sustento").hide();
    $("#div_lista_int_sustento").html('');

    if (LISTA_PLANTILLA_FORMULARIO_EXCLUSION != null && LISTA_PLANTILLA_FORMULARIO_EXCLUSION.length > 0) {
        $("#div_lista_int_sustento").append(`
            <span>Para realizar la acción se debe cargar los formatos de Exclusión de las siguientes intervenciones: </span>
        `);


        st_AgregarHtmlDivLista(TIPO_EXCLUSION, LISTA_PLANTILLA_FORMULARIO_EXCLUSION);

        var htmlDiv = `
            <input type="button" value="Confirmar Envío a COES" onclick="eliminarMasivo();" style='cursor: pointer;margin-top: 20px;' >      
        `;

        $("#div_lista_int_sustento").append(htmlDiv);
        $("#div_lista_int_sustento").show();
    }
}

function st_AgregarHtmlDivLista(tipo, lista) {
    var numFila = lista.length;
    var htmlTitulo = TIPO_INCLUSION == tipo ? 'Intervenciones - Sustento de inclusión' : 'Intervenciones - Sustento de Exclusión';
    var htmlTabla = st_DibujarTablaListadoHtml(tipo, lista);

    var txtHtml = `
        <div class="popup-title">
            <span>${htmlTitulo}</span>             
        </div>

        <div style='margin-top: 5px;margin-bottom: 20px;'>
            ${htmlTabla}  
        </div>
    `;

    $("#div_lista_int_sustento").append(txtHtml);

    setTimeout(function () {
        $('#tbl_lista_sustento_' + tipo).dataTable({
            "ordering": false,
            "info": false,
            "searching": false,
            "paging": false,
            "scrollX": true,
            "scrollY": numFila > 2 ? "92px" : "100%"
        });
    }, 150);
}

function st_DibujarTablaListadoHtml(tipo, lista) {
    var htmlBtn = TIPO_INCLUSION == tipo ? 'Sustento de inclusión' : 'Sustento de Exclusión';

    var cadena = '';
    cadena += `
    <table border="0" class="pretty tabla-icono" id="tbl_lista_sustento_${tipo}" style="overflow: auto; height:auto; width: 2000px !important; white-space: nowrap">
        <thead>
            <tr>
                <th style="text-align:center;width:4%">Acción</th>
                <th style="text-align:center;width:6%">Tip.<br> Interv.</th>
                <th style="text-align:center;width:6%">Empresa</th>
                <th style="text-align:center;width:6%">Ubicacion</th>
                <th style="text-align:center;width:6%">Tipo</th>
                <th style="text-align:center;width:6%">Equipo</th>
                <th style="text-align:center;width:5%">Fecha<br> inicio</th>
                <th style="text-align:center;width:5%">Fecha<br> fin</th>
                <th style="text-align:center;width:5%">MW <br>Ind.</th>
                <th style="text-align:center;width:5%">Disp.</th>
                <th style="text-align:center;width:5%">Interrup.</th>
                <th style="text-align:center;width:5%">Sist. Aisl.</th>
                <th style="text-align:center;width:5%">Inst. Prov.</th>
                <th style="text-align:center;width:20%">Descripción</th>
            </tr>
        </thead>
        <tbody>
    `;

    for (key in lista) {
        var item = lista[key];
        var sStyle = '';

        cadena += `
            
            <tr id="fila_${item.Intercodi}">
                <td style="">
                    <input type="button" value="${htmlBtn}" onclick="st_CargarFormularioSustento('${tipo}', ${item.Intercodi}, ${item.NroItem});" style='cursor: pointer;' >
                </td>
                <td style="text-align:left; ${sStyle}">${item.Tipoevenabrev}</td>
                <td style="text-align:left; ${sStyle}">${item.EmprNomb}</td>
                <td style="text-align:left; ${sStyle}">${item.AreaNomb}</td>
                <td style="text-align:left; ${sStyle}">${item.Famabrev}</td>
                <td style="text-align:left; ${sStyle}">${item.Equiabrev}</td>
                <td style="text-align:center; ${sStyle} ">${item.InterfechainiDesc}</td>
                <td style="text-align:center; ${sStyle} ">${item.InterfechafinDesc}</td>
                <td style="text-align:right; ${sStyle}">${item.Intermwindispo}</td>
                <td style="text-align:center; ${sStyle}">${item.InterindispoDesc}</td>
                <td style="text-align:center; ${sStyle}">${item.InterinterrupDesc}</td>
                <td style="text-align:center; ${sStyle}">${item.IntersistemaaisladoDesc}</td>
                <td style="text-align:center; ${sStyle}">${item.InterconexionprovDesc}</td>
                <td style="text-align:left; ${sStyle}">${item.Interdescrip}</td>
            </tr>
        `;
    }
    cadena += "</tbody></table>";

    return cadena;
}

function st_ObtenerJsonLista(tipo) {
    var listaPlt = TIPO_INCLUSION == tipo ? LISTA_PLANTILLA_FORMULARIO_INCLUSION : LISTA_PLANTILLA_FORMULARIO_EXCLUSION;

    var lista = [];
    if (listaPlt != null) {
        for (var i = 0; i < listaPlt.length; i++) {
            var regWeb = listaPlt[i];
            var regIn = {
                Intercodi: regWeb.Intercodi,
                NroItem: regWeb.NroItem,
                Sustento: regWeb.Sustento
            };

            lista.push(regIn);
        }
    }

    return lista;
}

//////////////////////////////////////////////////////////
/// Importación de intervenciones

function st_mostrarTablaFormularioImportacion(nameFile, sAccion) {

    $("#div_lista_int_sustento").hide();
    $("#div_lista_int_sustento").html('');

    var hayExcl = LISTA_PLANTILLA_FORMULARIO_EXCLUSION != null && LISTA_PLANTILLA_FORMULARIO_EXCLUSION.length > 0;
    var hayIncl = LISTA_PLANTILLA_FORMULARIO_INCLUSION != null && LISTA_PLANTILLA_FORMULARIO_INCLUSION.length > 0;
    if (hayExcl) {
        $("#div_lista_int_sustento").append(`
            <span>Para realizar la acción se debe cargar los formatos de Exclusión de las siguientes intervenciones: </span>
        `);
        st_AgregarHtmlDivLista(TIPO_EXCLUSION, LISTA_PLANTILLA_FORMULARIO_EXCLUSION);
    }
    if (hayIncl) {
        $("#div_lista_int_sustento").append(`
            <span>Para realizar la acción se debe cargar los formatos de Inclusión de las siguientes intervenciones: </span>
        `);
        st_AgregarHtmlDivLista(TIPO_INCLUSION, LISTA_PLANTILLA_FORMULARIO_INCLUSION);
    }

    if (hayExcl || hayIncl) {
        var htmlDiv = `
            <input type="button" value="Confirmar Envío a COES" onclick="validarReporte('${nameFile}','${sAccion}');" style='cursor: pointer;margin-top: 20px;' >      
        `;

        $("#div_lista_int_sustento").append(htmlDiv);
        $("#div_lista_int_sustento").show();
    }
}

function st_FlagTieneListaImportacionCompleto() {
    var hayExcl = LISTA_PLANTILLA_FORMULARIO_EXCLUSION != null && LISTA_PLANTILLA_FORMULARIO_EXCLUSION.length > 0;
    var hayIncl = LISTA_PLANTILLA_FORMULARIO_INCLUSION != null && LISTA_PLANTILLA_FORMULARIO_INCLUSION.length > 0;

    if (!hayExcl && !hayIncl) {
        return true;
    } else {
        var estaCompleto = true;

        if (hayExcl) {
            for (var i = 0; i < LISTA_PLANTILLA_FORMULARIO_EXCLUSION.length; i++) {
                var regSust = LISTA_PLANTILLA_FORMULARIO_EXCLUSION[i].Sustento;

                estaCompleto = estaCompleto && regSust.TienePlantillaCompleta;
            }
        }

        if (hayIncl) {
            for (var i = 0; i < LISTA_PLANTILLA_FORMULARIO_INCLUSION.length; i++) {
                var regSust = LISTA_PLANTILLA_FORMULARIO_INCLUSION[i].Sustento;

                estaCompleto = estaCompleto && regSust.TienePlantillaCompleta;
            }
        }

        return estaCompleto;
    }
}

//util
st_mostrarAlerta = function (alerta) {
    $('#mensaje3').html(alerta);
    $('#mensaje3').css("display", "block");
}

st_limpiarMensaje = function () {
    $('#mensaje3').html("");
    $('#mensaje3').css("display", "none");
}