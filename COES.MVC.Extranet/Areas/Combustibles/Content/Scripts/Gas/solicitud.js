var controlador = siteRoot + 'Combustibles/EnvioGas/';

const HAY_AUTOGUARDADO_Y_USO_INFO = "Usó una información autoguardada";
const HAY_AUTOGUARDADO_Y_NO_USO_INFO = "No usó una información autoguardada (reinició información)";

const VALOR_COOKIE = 0;
var debeAutoguardar = false;
var AUTOGUARDADO_HABILITADO = false;

$(function () {
    $('#tab-container').easytabs({
        animate: false
    });

    $('#cbEmpresa').change(function () {
        _mostrarGrillaXCentralYOpcion();
    });

    $('#btnInicio').click(function () {
        regresarPrincipal();
    });

    //
    $('#btnEnviarDatos').click(function () {
        enviarExcelWeb(GUARDADO_OFICIAL);
    });

    $('#btnAutoguardar').click(function () {
        enviarExcelWeb(GUARDADO_TEMPORAL);
    });
    $('#btnHistorialAutoguardado').click(function () {
        mostrarHistorialAutoguardados();
    });
    $('#btnMostrarErrores').click(function () {
        validarEnvio();
        mostrarDetalleErrores();
    });
    $('#btnLeyenda').click(function () {
        popupLeyenda();
    });
    $('#btnExpandirRestaurar').click(function () {
        expandirRestaurar();
    });

    $('#btnDescargar').click(function () {
        exportarFormularioEnvio();
    });
    $('#btnVerEnvio').click(function () {
        mostrarEnviosAnteriores();
    });

    //acciones sobre celdas
    $('#btnNoAplica').click(function () {
        actualizarHandsonValorCelda(TIPO_OPERACION_NO_APLICA, "No Aplica", "");
    });
    $('#btnLimpiar').click(function () {
        if (confirm("¿Estás seguro de limpiar la información del Formato 3 e informe sustentatorio para esta central?")) {
            var tipoOpcion = $("#hdTipoOpcion").val();

            if (tipoOpcion != "SA") {
                mostrarGrilla();
            }
        }
    });
    $('#btnMasDecimales').click(function () {
        actualizarHandsonValorCelda(TIPO_OPERACION_DECIMAL, "", 1);
    });
    $('#btnMenosDecimales').click(function () {
        actualizarHandsonValorCelda(TIPO_OPERACION_DECIMAL, "", -1);
    });
    $(document).on('paste', 'input[type="text"].onlyNumber', function (event) {
        if (!event.originalEvent.clipboardData.getData('Text').match(/^\d+\.\d{0,10}$/)) {
            event.preventDefault();
        }
    });

    AUTOGUARDADO_HABILITADO = parseInt($('#hdHabilitarAutoguardado').val()) == 1;

    var numMIn = parseInt($('#hdIMinutosAutoguardado').val());
    setInterval(function () {
        if (debeAutoguardar && AUTOGUARDADO_HABILITADO)
            enviarExcelWeb(GUARDADO_TEMPORAL);

    }, numMIn * 60 * 1000);

    //mostrar opción
    _mostrarGrillaXCentralYOpcion();
    crearPuploadFormato3();
});

function _mostrarGrillaXCentralYOpcion() {
    buscarEnviosAutoguardados();

}

function regresarPrincipal() {
    var estadoEnvio = parseInt($("#hdIdEstado").val()) || 0;
    if (estadoEnvio == ESTADO_SOLICITUD_ASIGNACION) estadoEnvio = ESTADO_SOLICITADO;
    if (estadoEnvio == ESTADO_OBSERVADO) estadoEnvio = ESTADO_SUBSANADO;   //Cuando subsana un observado, debe mostrar subsanados
    document.location.href = controlador + "Index?carpeta=" + estadoEnvio;
}

function enviarExcelWeb(tipoGuardado) {
    //const GUARDADO_TEMPORAL = 1;
    //const GUARDADO_OFICIAL = 2;
    var preguntarConfirmacion = true;
    if (tipoGuardado == GUARDADO_OFICIAL)
        preguntarConfirmacion = confirm("¿Desea enviar información a COES?  Una vez enviado, se procederá con la revisión por parte de COES.");

    if (preguntarConfirmacion) {
        var idEnvio = parseInt($("#hfIdEnvio").val());
        var idEnvioTemporal = $("#hfIdEnvioTemporal").val();
        var empresa = $("#cbEmpresa").val();
        var tipoCentral = $("#hdTipoCentral").val();
        var tipoOpcion = $("#hdTipoOpcion").val();
        var estadoEnvio = parseInt($("#hdIdEstado").val()) || 0;
        var descripcionVersion = $("#hdHistorialParteDesc").val();
        //var condTemporalAnterior = parseInt($("#hdCondicionEnvioPrevioTemporal").val()) || 1; // 0: con error, 1: correcto
        var nombX = "Envio-" + $("#hfIdEnvio").val() + "-" + $("#cbEmpresa").val();  //Envio-CodEnvio-CodEmpresa
        var condTemporalAnterior = buscarSiEnvioPresentoErroresDeConexion(nombX);

        var esValidoEnvio = true;
        if (tipoOpcion != "SA") {
            if (tipoGuardado == GUARDADO_OFICIAL) {
                esValidoEnvio = validarEnvio();
            }
        }

        var mensajeObs = '';
        if (tipoGuardado == GUARDADO_OFICIAL) {
            if (estadoEnvio == ESTADO_OBSERVADO) mensajeObs = validarSeccionesSeguimientoObs();
            if (mensajeObs != "") esValidoEnvio = false;
        }

        if (esValidoEnvio) {

            //actualizar 
            if (tipoOpcion != "SA") {
                actualizarModeloMemoria();
                actualizarModeloSustentoMemoria();
            }

            var formulario = {
                IdEnvio: idEnvio,
                IdEmpresa: empresa,
                ListaFormularioCentral: MODELO_LISTA_CENTRAL,
                FormularioSustento: MODELO_DOCUMENTO,
                TipoCentral: tipoCentral,
                TipoOpcion: tipoOpcion,
                TipoGuardado: tipoGuardado,
                DescVersion: descripcionVersion,
                CondicionEnvioPrevioTemporal: condTemporalAnterior
            };

            debeAutoguardar = false;

            var dataJson = {
                data: JSON.stringify(formulario)
            };

            $.ajax({
                type: 'POST',
                dataType: 'json',
                url: controlador + "GrabarDatosCombustible",
                contentType: 'application/json; charset=UTF-8',
                data: JSON.stringify(dataJson),
                beforeSend: function () {
                    //mostrarExito("Enviando Información ..");
                },
                success: function (evt) {
                    var fechahora = obtenerFechaHoraActual();
                    if (evt.Resultado != "-1") {
                        if (evt.Resultado == "1") {

                            //si es oficial refresca pantalla
                            if (tipoGuardado == GUARDADO_OFICIAL) {
                                alert("Se envió la información correctamente");
                                //mostrar pantalla de consolidado de envio

                                /*if (MODELO_WEB.Estenvcodi == ESTADO_OBSERVADO || MODELO_WEB.Estenvcodi == ESTADO_LEVANTAMIENTO_OBS) {
                                    $("#hdIdEstado").val(ESTADO_LEVANTAMIENTO_OBS);
                                }*/
                                regresarPrincipal();
                            } else {
                                if (tipoGuardado == GUARDADO_TEMPORAL) { //si es autoguardado solo muestro mensaje
                                    mostrarMensaje('mensaje_GuardadoTemporal', 'exito', fechahora + " - Autoguardado correctamente.");

                                    //reseteo mensaje de descripcion previa, dado que ya fue usado
                                    $("#hdHistorialParteDesc").val("");
                                }

                            }
                        }
                        if (evt.Resultado == "2") {
                            alert("Información es idéntica a la enviada anteriormente, no se realizó ningún cambio");
                        }

                    } else {
                        //si es oficial muestro alert
                        if (tipoGuardado == GUARDADO_OFICIAL) {
                            alert("Ha ocurrido un error: " + evt.Mensaje);
                        } else {
                            if (tipoGuardado == GUARDADO_TEMPORAL) { //si es autoguardado solo muestro mensaje
                                mostrarMensaje('mensaje_GuardadoTemporal', 'error', fechahora + " - Autoguardado incorrecto.");

                                //reseteo mensaje de descripcion previa, dado que ya fue usado al tratar de guardarse
                                $("#hdHistorialParteDesc").val("");
                            }

                        }
                        alert("Ha ocurrido un error: " + evt.Mensaje);
                    }

                    //elimino aa cookie con el id del envio padre dado que ya fue guardado el temporal con error
                    var nombVariable = "Envio" + parseInt($("#hfIdEnvio").val());  //EnvioX
                    eliminarCookie(nombVariable);

                    //$("#hdCondicionEnvioPrevioTemporal").val(1); //tuvo conexion con servidor

                    debeAutoguardar = true;
                },
                error: function (err) {
                    var fechahora2 = obtenerFechaHoraActual();
                    alert("Ha ocurrido un error.");

                    //si son autoguardados y no existe comunicacion con el servidor, guardo una version con estado incorrecto
                    if (tipoGuardado == GUARDADO_TEMPORAL) {

                        //creo una cookie con el id del envio padre y le seteo 0 (que indica errordel autoguardado temporal)
                        var nombVariable = "Envio-" + parseInt($("#hfIdEnvio").val()) + "-" + $("#cbEmpresa").val();  //Envio-CodEnvio-CodEmpresa
                        guardarCookkie(nombVariable, 2);
                        //$("#hdCondicionEnvioPrevioTemporal").val(0); //tuvo errores en comunicación con servidor
                        mostrarMensaje('mensaje_GuardadoTemporal', 'error', fechahora2 + " - Autoguardado incorrecto.");
                    }

                    debeAutoguardar = true;
                }
            });
        } else {
            if (mensajeObs != "") {
                alert(mensajeObs);
            } else {
                alert("Error al Grabar, hay errores en celdas");
                mostrarDetalleErrores();
            }
        }
    }
}

//Descargar formato
function exportarFormularioEnvio() {
    var idEnvio = parseInt($("#hfIdEnvio").val());
    var tipoOpcion = $("#hdTipoOpcion").val();

    //actualizar 
    if (tipoOpcion != "SA") {
        actualizarModeloMemoria();
        actualizarModeloSustentoMemoria();
    }

    var formulario = {
        IdEnvio: idEnvio,
        ListaFormularioCentral: MODELO_LISTA_CENTRAL,
    };

    if (_esValidoFormularioExportarImportar()) {

        var dataJson = {
            data: JSON.stringify(formulario)
        };

        $.ajax({
            type: 'POST',
            url: controlador + "ExportarFormularioEnvio",
            contentType: 'application/json; charset=UTF-8',
            data: JSON.stringify(dataJson),
            success: function (evt) {
                if (evt.Resultado != "-1") {
                    window.location = controlador + "ExportarReporteFormulario";
                } else {
                    alert("Ha ocurrido un error: " + evt.Mensaje);
                }

            },
            error: function (err) {
                alert("Ha ocurrido un error");
            }
        });
    } else {
        alert("No es posible descargar. Cada sección debe tener una opción seleccionada para permitir la descarga.");
    }
}

function _esValidoFormularioExportarImportar() {
    var esValidoForm = true;

    var listaCentrales = MODELO_LISTA_CENTRAL; //variable global de solicitudExcelWeb.js
    for (var i = 0; i < listaCentrales.length; i++) {
        var objTab = listaCentrales[i];

        var listaArrayItem = objTab.ArrayItem;
        for (var j = 0; j < listaArrayItem.length; j++) {
            var objItem = listaArrayItem[j];

            //seccion 2.0,3.0,4.0
            if (objItem.EsSeccion && objItem.ListaOpcionDesplegable != null && objItem.ListaOpcionDesplegable.length > 0) {
                if (objItem.TipoOpcionSeccion == null || objItem.TipoOpcionSeccion == "")
                    esValidoForm = false;
            }
        }
    }

    return esValidoForm;
}

//Importar formato
function crearPuploadFormato3() {
    var uploader = new plupload.Uploader({
        runtimes: 'html5,flash,silverlight,html4',
        browse_button: 'btnImportar',
        url: controlador + "UploadFormato3",
        flash_swf_url: 'Scripts/Moxie.swf',
        silverlight_xap_url: 'Scripts/Moxie.xap',
        multi_selection: false,
        filters: {
            max_file_size: '5mb',
            mime_types: [
                { title: "Archivos Excel .xlsx", extensions: "xlsx,xls" }
            ]
        },
        init: {
            FilesAdded: function (up, files) {
                if (uploader.files.length == 2) {
                    uploader.removeFile(uploader.files[0]);
                }
                uploader.start();
                up.refresh();
            },
            UploadProgress: function (up, file) {
                //mostrarEventoAlerta(numHoja, "Por favor espere ...(<strong>" + file.percent + "%</strong>)");
            },
            UploadComplete: function (up, file) {
                leerFileUpExcel();
            },
            Error: function (up, err) {
                //mostrarEventoError(numHoja, err.code + "-" + err.message);
            }
        }
    });

    uploader.init();
}

function leerFileUpExcel() {
    actualizarModeloMemoria();
    actualizarModeloSustentoMemoria();

    var formulario = {
        ListaFormularioCentral: MODELO_LISTA_CENTRAL_JSON,
    };

    var dataJson = {
        data: JSON.stringify(formulario)
    };

    $.ajax({
        type: 'POST',
        dataType: 'json',
        url: controlador + 'LeerFileUpExcelFormato3',
        contentType: 'application/json; charset=UTF-8',
        data: JSON.stringify(dataJson),
        success: function (evt) {
            if (evt.Resultado == "1") {
                MODELO_LISTA_CENTRAL = evt.ListaFormularioCentral;
                MODELO_LISTA_CENTRAL_JSON = _clonarModeloListaCentral(MODELO_LISTA_CENTRAL);

                //
                var idEnvio = parseInt($("#hfIdEnvio").val());
                var tipoAccionForm = $("#hdTipoAccion").val();
                formulario = {
                    IdEnvio: idEnvio,
                    ListaFormularioCentral: MODELO_LISTA_CENTRAL_JSON,
                    TipoAccionForm: tipoAccionForm
                };
                _actualizarGrillaExcelWeb(formulario);
            }
            else {
                if (evt.Resultado == "0") {
                    alert(evt.Mensaje);
                } else {
                    alert("Ha ocurrido un error en la importación del archivo excel.");
                }
            }
        },
        error: function (err) {
            alert("Ha ocurrido un error");
        }
    });
}

//VALIDAR SECCION

const SIN_ESTADO_SELEC = 0;
const CONFORME = 1;
const OBSERVADO = 2;
const OBSERVADO_INCOMPLETO = 12;
const SUBSANADO = 3;
const SUBSANADO_INCOMPLETO = 13;
const NOSUBSANADO = 4;

function validarSeccionesSeguimientoObs() {
    var msj = "";

    //obtener datos
    var lstDatosCentrales = obtenerDatosCentrales();
    var listaArchivo = obtenerDataHandsonArchivo();

    //validar
    msj += validarTabFormato3(lstDatosCentrales) + "\n";
    msj += validarTabSustento(listaArchivo) + "\n";

    return msj.trim();
}

function obtenerDatosCentrales() {

    var listaCentrales = MODELO_LISTA_CENTRAL; //variable global de solicitudExcelWeb.js

    var listaDataCentrales = [];
    for (var i = 0; i < listaCentrales.length; i++) {

        var central = listaCentrales[i];

        var objCentralX = {
            Equicodi: central.Equicodi, //codigo
            NombCentral: central.Central, //nombre central
            Tipo: $("#hdTipoCentral").val(), //'N':Nueva, 'E':Existente
            DataFormularioF3: obtenerDataHandson(i, central.Equicodi, central.ArrayItemObs),
        };

        listaDataCentrales.push(objCentralX);
    }

    return listaDataCentrales;
}

function obtenerDataHandson(posTab, equicodi, listaItemObs) {
    var listaDataSeccion = [];

    //
    var objTabCentral = MODELO_LISTA_CENTRAL[posTab];
    var dataHandson = LISTA_OBJETO_HOJA[equicodi].hot.getData();

    for (var i = 0; i < listaItemObs.length; i++) {
        var objItemObs = listaItemObs[i];
        if (objItemObs.EsColEstado) {
            var codigoEstado = 0;

            var cadenaEstado = (dataHandson[objItemObs.PosRow][objItemObs.PosCol] ?? "").toUpperCase();

            var objItemObsColSub = _getItemObsByPos(objTabCentral.ArrayItemObs, objItemObs.PosRow, objItemObs.PosCol - 2);
            var observacionHtml = (objItemObsColSub.Obs.Cbobshtml ?? "").trim();

            switch (cadenaEstado) {
                case "CONFORME":
                    codigoEstado = 1;
                    break;
                case "OBSERVADO":
                    codigoEstado = 2;
                    break;
                case "SUBSANADO":
                    codigoEstado = 3;
                    //if (observacionHtml == "") codigoEstado = 13;
                    break;
                case "NO SUBSANADO":
                    codigoEstado = 4;
                    break;
            }

            var objSeccion = {
                NumSeccion: objItemObs.NumeralSeccion, //devuelve num de la seccion
                ValorObs: observacionHtml, //devuelve valor (no es necesario)
                Estado: codigoEstado, //devuelve estado  (entero)  // 1:Conforme, 2:Observado, 3:Subsanado y 4:No subsanado
            };

            listaDataSeccion.push(objSeccion);
        }
    }

    return listaDataSeccion;
}

function obtenerDataHandsonArchivo() {
    var listaDataSeccion = [];

    var tieneObs = MODELO_DOCUMENTO.IncluirObservacion;
    var filaIni = tieneObs ? 2 : 1;

    //actualizar lista de archivo
    var colBtnEliminar = 0;
    var colNombreArchivo = colBtnEliminar + 1;
    var colConf = colNombreArchivo + 1;
    var colObsCOES = colConf + 1;
    var colSubsGen = colObsCOES + 1;
    var colRptaCOES = colSubsGen + 1;
    var colEstado = colRptaCOES + 1;

    if (tieneObs) {
        var posColObsHtml = 1;
        var objArchivo = MODELO_DOCUMENTO.SeccionCombustible.ListaObs[posColObsHtml];
        var estado = HOT_SUSTENTO.getData()[0 + filaIni][colEstado];
        objArchivo.Cbevdavalor = estado;

        var codigoEstado = SIN_ESTADO_SELEC;

        var cadenaEstado = (estado ?? "").toUpperCase();
        var observacionHtml = (objArchivo.Cbobshtml ?? "").trim();
        switch (cadenaEstado) {
            case "CONFORME":
                codigoEstado = 1;
                break;
            case "OBSERVADO":
                codigoEstado = 2;
                //if (observacionHtml == "") codigoEstado = 12;
                break;
            case "SUBSANADO":
                codigoEstado = 3;
                //if (observacionHtml == "") codigoEstado = 13;
                break;
            case "NO SUBSANADO":
                codigoEstado = 4;
                break;
        }

        var objSeccion = {
            Archivo: objArchivo.Cbarchnombreenvio, //devuelve num de la seccion
            ValorObs: observacionHtml, //devuelve valor (no es necesario)
            Estado: codigoEstado, //devuelve estado  (entero)  // 1:Conforme, 2:Observado, 3:Subsanado y 4:No subsanado
        };

        listaDataSeccion.push(objSeccion);
    }

    return listaDataSeccion;
}

//
function validarTabFormato3(lstDatosCentrales) {
    var listaMsj = [];

    for (var i = 0; i < lstDatosCentrales.length; i++) {

        var numSinEstado = 0;
        var numConformes = 0;
        var numObservados = 0;
        var numSubsanados = 0;
        var numSubsanadosIncompleto = 0;
        var numNoSubsanados = 0;

        var centralX = lstDatosCentrales[i];
        var lstSeccionesF3 = centralX.DataFormularioF3;

        for (var y = 0; y < lstSeccionesF3.length; y++) {
            var seccionF3 = lstSeccionesF3[y];
            var estadoSeccion = seccionF3.Estado; // 1:Conforme, 2:Observado, 3:Subsanado y 4:No subsanado

            if (estadoSeccion == 0)
                numSinEstado++;
            if (estadoSeccion == CONFORME)
                numConformes++;
            if (estadoSeccion == OBSERVADO)
                numObservados++;
            if (estadoSeccion == SUBSANADO)
                numSubsanados++;
            if (estadoSeccion == NOSUBSANADO)
                numNoSubsanados++;
            if (estadoSeccion == SUBSANADO_INCOMPLETO)
                numSubsanadosIncompleto++;
        }

        centralX.NumSinEstado = numSinEstado;
        centralX.NumConformes = numConformes;
        centralX.NumObservados = numObservados;
        centralX.NumSubsanados = numSubsanados;
        centralX.NumNoSubsanados = numNoSubsanados;
        centralX.NumNoSubsanadosIncompleto = numSubsanadosIncompleto;

        if (centralX.NumSinEstado > 0) {
            listaMsj.push("Central " + centralX.NombCentral + ": Existen secciones sin seleccionar estado.");
        }
        if (centralX.NumObservados > 0) {
            listaMsj.push("Central " + centralX.NombCentral + ": Existen secciones con estado OBSERVADO.");
        }
        if (centralX.NumNoSubsanadosIncompleto > 0) {
            if (centralX.NumSinEstado > 0) listaMsj.push("Central " + centralX.NombCentral + ": Existen secciones sin seleccionar estado.");
            listaMsj.push("Central " + centralX.NombCentral + ": Existen secciones con estado SUBSANADO sin detalle de la subsanación de la observación.");
        }

    }

    return listaMsj.join('\n');
}

function validarTabSustento(lstSeccionesArchivos) {
    var listaMsj = [];

    var numSinEstado = 0;
    var numConformes = 0;
    var numObservados = 0;
    var numSubsanados = 0;
    var numSubsanadosIncompleto = 0;
    var numNoSubsanados = 0;

    for (var z = 0; z < lstSeccionesArchivos.length; z++) {
        var seccionA = lstSeccionesArchivos[z];
        var estadoSeccion = seccionA.Estado; // 1:Conforme, 2:Observado, 3:Subsanado y 4:No subsanado

        if (estadoSeccion == 0)
            numSinEstado++;
        if (estadoSeccion == CONFORME)
            numConformes++;
        if (estadoSeccion == OBSERVADO)
            numObservados++;
        if (estadoSeccion == SUBSANADO)
            numSubsanados++;
        if (estadoSeccion == NOSUBSANADO)
            numNoSubsanados++;
        if (estadoSeccion == SUBSANADO_INCOMPLETO)
            numSubsanadosIncompleto++;
    }

    if (numSinEstado > 0) listaMsj.push("Existen archivos sin seleccionar estado.");
    if (numObservados > 0) listaMsj.push("Existen archivos con estado OBSERVADO.");
    if (numSubsanadosIncompleto > 0) listaMsj.push("Existen archivos con estado SUBSANADO sin detalle de la subsanación de la observación.");

    return listaMsj.join('\n');
}

//AUTOGUARDADO
function buscarEnviosAutoguardados() {
    var idEnvio = parseInt($("#hfIdEnvio").val());
    var tipoCentral = $("#hdTipoCentral").val();
    var mesVigencia = $("#cbMes").val();
    var idEmpresa = $("#cbEmpresa").val();
    var estenvcodi = $("#hdIdEstado").val() || 0;

    const HAY_AUTOGUARDADOS = 1;
    const NO_HAY_AUTOGUARDADOS = 0;

    $.ajax({
        type: 'POST',
        url: controlador + 'BuscarEnviosAutoguardados',
        dataType: 'json',
        data: {
            idEnvio: idEnvio,
            tipoCentral: tipoCentral,
            mesVigencia: mesVigencia,
            idEmpresa: idEmpresa,
            estenvcodi: estenvcodi
        },
        cache: false,
        success: function (evt) {
            if (evt.Resultado != "-1") {
                var debeBuscarAutoguardados = evt.BuscaAutoguardados;
                var existeAutoguardados = evt.HayAutoguardados;

                if (debeBuscarAutoguardados == 1) {
                    if (existeAutoguardados == HAY_AUTOGUARDADOS) {
                        if (confirm("Hay información previamente llenada para el envío a realizar ¿Desea usarlo?")) {

                            //Guardo parte de la descipción
                            $("#hdHistorialParteDesc").val(HAY_AUTOGUARDADO_Y_USO_INFO);

                            //Seteo el idEnvio del ultimo autoguardado
                            $("#hfIdEnvio").val(parseInt(evt.IdEnvio));
                            $("#hdIdEnvioData").val(evt.IdEnvio); //solo para debuggeo
                        }
                        else {
                            //Guardo parte de la descipción
                            $("#hdHistorialParteDesc").val(HAY_AUTOGUARDADO_Y_NO_USO_INFO);
                        }
                    }
                }


                //Muestra la grilla
                var tipoOpcion = $("#hdTipoOpcion").val();
                if (tipoOpcion != "SA") {
                    mostrarGrilla();
                }
                else {
                    mostrarGrillaNuevaAsignacion();
                }
            } else {
                alert('Ha ocurrido un error: ' + evt.Mensaje);
            }
        },
        error: function (err) {
            alert('Ha ocurrido un error.')
        }
    });
}

function mostrarHistorialAutoguardados() {
    var idEnvio = parseInt($("#hfIdEnvio").val());
    var tipoCentral = $("#hdTipoCentral").val();
    var mesVigencia = $("#cbMes").val();
    var idEmpresa = $("#cbEmpresa").val();
    var estenvcodi = $("#hdIdEstado").val() || 0;


    var cadena = '';

    $.ajax({
        type: 'POST',
        url: controlador + 'BuscarVersionesAutoguardados',
        dataType: 'json',
        data: {
            idEnvio: idEnvio,
            tipoCentral: tipoCentral,
            mesVigencia: mesVigencia,
            idEmpresa: idEmpresa,
            estenvcodi: estenvcodi
        },
        cache: false,
        success: function (evt) {
            if (evt.Resultado != "-1") {

                cadena += `
                        <div style='clear:both; height:5px'></div>
                            <table id='tablaHistAutoguardados' border='1' class='pretty tabla-adicional' cellspacing='0'>
                               <thead>
                                    <tr>
                                        
                                        <th>Versión</th>
                                        <th>Usuario</th>
                                        <th>Fecha de Autoguardado</th>
                                        <th>Operación</th>
                                        <th>Descripción</th>
                                    </tr>
                                </thead>
                                <tbody>
                `;
                for (var i = 0; i < evt.ListaAutoguardados.length; i++) {
                    var reg = evt.ListaAutoguardados[i];

                    cadena += `   <tr> 
                                        
                                        <td>${reg.Cbvernumversion}</td>
                                        <td>${reg.Cbverusucreacion}</td>
                                        <td>${reg.CbverfeccreacionDesc}</td>
                                        <td>${reg.CbveroperacionDesc}</td>
                                        <td>${reg.Cbverdescripcion}</td>
                                    </tr>
                    `;
                }
                cadena += `     </tbody>
                            </table>
                        </div>
                `;


                $('#idHistoryAutoguardados').html(cadena);
                $('#tablaHistAutoguardados').dataTable({
                    //"scrollY": 430,
                    //"scrollX": false,
                    "sDom": 't',
                    "ordering": false,
                    "iDisplayLength": -1
                });

                abrirPopup("historialAutoguardados");

            } else {
                alert('Ha ocurrido un error: ' + evt.Mensaje);
            }
        },
        error: function (err) {
            alert('Ha ocurrido un error.')
        }
    });



}

function obtenerFechaHoraActual() {
    var fechahora;

    let date = new Date();
    let fecha = String(date.getDate()).padStart(2, '0') + '/' + String(date.getMonth() + 1).padStart(2, '0') + '/' + date.getFullYear();
    let hora = String(date.getHours()).padStart(2, '0') + ":" + String(date.getMinutes()).padStart(2, '0') + ":" + String(date.getSeconds()).padStart(2, '0');

    fechahora = fecha + " " + hora;

    return fechahora;
}

function guardarCookkie(nombre, diasAExpirar) {
    var valor = VALOR_COOKIE;
    const d = new Date();
    d.setTime(d.getTime() + (diasAExpirar * 24 * 60 * 60 * 1000));
    let expires = "expires=" + d.toUTCString();
    document.cookie = nombre + "=" + valor + ";" + expires + ";path=/";
}

function obtenerCookie(nombre) {
    let nombreBuscado = nombre + "=";
    let ca = document.cookie.split(';');
    for (let i = 0; i < ca.length; i++) {
        let c = ca[i];
        while (c.charAt(0) == ' ') {
            c = c.substring(1);
        }
        if (c.indexOf(nombreBuscado) == 0) {
            return c.substring(nombreBuscado.length, c.length);
        }
    }
    return "";
}

function eliminarCookie(nombre) {
    document.cookie = nombre + "=; expires=Thu, 01 Jan 1970 00:00:00 UTC; path=/;";
}

//devuelve 0/1 si el autoguardado anterior presento fallas de conexion: 
function buscarSiEnvioPresentoErroresDeConexion(nombre) {  //0: con error, 1: sin error
    let val = obtenerCookie(nombre) + "";
    var strValCookie = VALOR_COOKIE + "";
    //si existe una cookie para el envio, la elimino y devuelvo 1 (valor que servira para guardar una version con error.)
    if (val == strValCookie) {
        eliminarCookie(nombre);
        return 0;
    }

    return 1;
}