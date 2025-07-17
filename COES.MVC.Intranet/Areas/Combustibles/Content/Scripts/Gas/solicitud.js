var controlador = siteRoot + 'Combustibles/GestionGas/';

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
        enviarExcelWeb();
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
        //actualizarHandsonValorCelda("");
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

    //mostrar opción
    _mostrarGrillaXCentralYOpcion();
    crearPuploadFormato3();
});

function _mostrarGrillaXCentralYOpcion() {
    var tipoOpcion = $("#hdTipoOpcion").val();
    if (tipoOpcion != "SA") {
        mostrarGrilla();
    }
    else {
        mostrarGrillaNuevaAsignacion();
    }
}

function iniciarEasyTabSustentatorio() {
    document.querySelector('#div_informe_sustentatorio').click();
}

function regresarPrincipal() {
    var estadoEnvio = $("#hdIdEstado").val();
    if (estadoEnvio == EST_SOLICITUD_ASIGNACION) estadoEnvio = EST_SOLICITUD;
    document.location.href = controlador + "Index?carpeta=" + estadoEnvio;
}

function enviarExcelWeb() {
    if (confirm("¿Desea enviar información a COES?")) {
        var idEnvio = $("#hfIdEnvio").val();
        var empresa = $("#cbEmpresa").val();
        var tipoCentral = $("#hdTipoCentral").val();
        var tipoOpcion = $("#hdTipoOpcion").val();
        var tipoAccionForm = $("#hdTipoAccion").val();
        var usuarioGenerador = $("#hdUsuarioGenerador").val();
        var esPrimeraCarga = $("#hdEsPrimeraCarga").val();
        var mesVigencia = $("#cbMes").val();

        var esValidoEnvio = true;
        if (tipoOpcion != "SA") {
            esValidoEnvio = validarEnvio();
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
                TipoAccionForm: tipoAccionForm,
                UsuarioGenerador: usuarioGenerador,
                EsPrimeraCarga: esPrimeraCarga,
                MesVigencia: mesVigencia
            };

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
                    if (evt.Resultado != "-1") {
                        if (evt.Resultado == "1") {
                            alert("Se envió la información correctamente");
                            //mostrar pantalla de consolidado de envio

                            /*if (MODELO_WEB.Estenvcodi == ESTADO_OBSERVADO || MODELO_WEB.Estenvcodi == ESTADO_LEVANTAMIENTO_OBS) {
                                $("#hdIdEstado").val(ESTADO_LEVANTAMIENTO_OBS);
                            }*/
                            document.location.href = controlador + "Index?carpeta=" + 3;
                        }
                        if (evt.Resultado == "2") {
                            alert("Información es idéntica a la enviada anteriormente, no se realizó ningún cambio");
                        }

                    } else {
                        alert("Ha ocurrido un error: " + evt.Mensaje);
                    }
                },
                error: function (err) {
                    alert("Ha ocurrido un error.");
                }
            });
        } else {
            alert("Error al Grabar, hay errores en celdas");
            mostrarDetalleErrores();
        }
    }
}

//Descargar formato
function exportarFormularioEnvio() {
    var idEnvio = $("#hfIdEnvio").val();
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
                var idEnvio = $("#hfIdEnvio").val();
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