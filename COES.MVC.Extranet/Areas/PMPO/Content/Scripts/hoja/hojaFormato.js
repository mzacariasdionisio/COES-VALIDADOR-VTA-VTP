//VARIABLES GLOBALES
var LISTA_OBJETO_HOJA = [];

var hojaPrincipal = '0';
var MAIN_VER_ULTIMO_ENVIO = true;
var INIT_RENDERIZAR = false;
var ARRAY_FILES_MENSAJE = [];
var CONTADOR_UPLOADER = 0;

var ARRAY_FILES_GRABAR_DATOS = [];

$(function () {
    //if (typeof (INIT_RENDERIZAR) != "undefined" && INIT_RENDERIZAR == false) {}
});

function inicializarFormato(numHoja) {
    var configuracionHoja = getConfigHoja(numHoja);
    limpiarBarra(numHoja);

    //filtros
    $(getIdElemento(numHoja, '#txtFecha')).Zebra_DatePicker({
        onSelect: function () {
            //setVerUltimoEnvio(VER_ULTIMO_ENVIO, numHoja);
            btnConsultar(numHoja);
        }
    });

    if (!configuracionHoja.tieneFiltroFecha) {
        $(getIdElemento(numHoja, '#txtMes')).Zebra_DatePicker({
            format: 'm Y',
            onSelect: function () {
                setVerUltimoEnvioGlobal(MAIN_VER_ULTIMO_ENVIO);
                btnConsultar(numHoja);
            }
        });
    } else {
        $(getIdElemento(numHoja, '#txtMes')).Zebra_DatePicker({
            format: 'm Y',
            onSelect: function () {
                //setVerUltimoEnvio(VER_ULTIMO_ENVIO, numHoja);
                btnConsultar(numHoja);
            },
            direction: configuracionHoja.valorFiltroFecha
        });
    }

    $(getIdElemento(numHoja, '#cbSemana')).unbind('change');
    $(getIdElemento(numHoja, '#cbSemana')).change(function () {
        //setVerUltimoEnvio(VER_ULTIMO_ENVIO, numHoja);
        btnConsultar(numHoja);
    });

    $(getIdElemento(numHoja, '#Anho')).Zebra_DatePicker({
        format: 'Y',
        onSelect: function () {
            cargarSemanaAnho(numHoja);
        }
    });

    if (!configuracionHoja.tieneFiltroEmpresaPmpo) {
        $(getIdElemento(numHoja, '#cbEmpresa')).unbind('change');
        $(getIdElemento(numHoja, '#cbEmpresa')).change(function () {
            //setVerUltimoEnvio(VER_ULTIMO_ENVIO, numHoja);
            btnConsultar(numHoja);
        });
    } else {
    }

    $(getIdElemento(numHoja, '#cbUnidadEnvio')).unbind();
    $(getIdElemento(numHoja, '#cbUnidadEnvio')).change(function () {
        setVerUltimoEnvioGlobal(MAIN_VER_ULTIMO_ENVIO);
        btnConsultar(numHoja);
    });

    if (configuracionHoja.tieneFiltroCentral) {
        $(getIdElemento(numHoja, '#cbCentral')).unbind('change');
        $(getIdElemento(numHoja, '#cbCentral')).change(function () {
            hideColumnaGrilla(numHoja);
        });
    }
    if (configuracionHoja.tieneFiltroArea) {
        $(getIdElemento(numHoja, '#cbArea')).unbind('change');
        $(getIdElemento(numHoja, '#cbArea')).change(function () {
            hideColumnaGrilla(numHoja);
        });
    }

    if (configuracionHoja.tieneFiltroSubestacion) {
        $(getIdElemento(numHoja, '#cbSubestacion')).unbind('change');
        $(getIdElemento(numHoja, '#cbSubestacion')).change(function () {
            hideColumnaGrilla(numHoja);
        });
    }

    if (configuracionHoja.tieneFiltroFamilia) {
        $(getIdElemento(numHoja, '#cbFamilia')).unbind('change');
        $(getIdElemento(numHoja, '#cbFamilia')).change(function () {
            hideColumnaGrilla(numHoja);
        });
    }

    if (configuracionHoja.tieneFiltroFormato && !configuracionHoja.tieneFiltroEmpresaPmpo) {
        $(getIdElemento(numHoja, '#cbTipoFormato')).unbind('change');
        $(getIdElemento(numHoja, '#cbTipoFormato')).change(function () {
            CANTIDAD_CLICK_TIPO_FORMATO++;
            cargarFormato(numHoja);
        });
    }

    //botones
    $(getIdElemento(numHoja, '#btnHOP')).unbind('click');
    $(getIdElemento(numHoja, '#btnHOP')).click(function () {
        btnHOP(numHoja);
    });

    $(getIdElemento(numHoja, '#btnEditarEnvio')).unbind('click');
    $(getIdElemento(numHoja, '#btnEditarEnvio')).click(function () {
        btnConsultar(numHoja);
    });

    $(getIdElemento(numHoja, '#btnConsultar')).unbind('click');
    $(getIdElemento(numHoja, '#btnConsultar')).click(function () {
        btnConsultar(numHoja);
    });

    $(getIdElemento(numHoja, '#btnDescargarFormato')).unbind('click');
    $(getIdElemento(numHoja, '#btnDescargarFormato')).click(function () {
        btnDescargarFormato(numHoja);
    });

    btnSelectExcel(numHoja);
    btnFileAttachEnvio();

    $(getIdElemento(numHoja, '#btnEnviarDatos')).unbind('click');
    $(getIdElemento(numHoja, '#btnEnviarDatos')).click(function () {
        btnEnviarDatos(numHoja);
    });
    $('#enviar-btn-send').unbind();
    $('#enviar-btn-send').click(function () {
        envioData(numHoja);
    });

    $(getIdElemento(numHoja, '#btnMostrarErrores')).unbind('click');
    $(getIdElemento(numHoja, '#btnMostrarErrores')).click(function () {
        btnMostrarErrores(numHoja);
    });

    $(getIdElemento(numHoja, '#btnVerJustificaciones')).unbind('click');
    $(getIdElemento(numHoja, '#btnVerJustificaciones')).click(function () {
        btnVerJustificaciones(numHoja);
    });

    $(getIdElemento(numHoja, '#btnVerEnvios')).unbind('click');
    $(getIdElemento(numHoja, '#btnVerEnvios')).click(function () {
        btnVerEnvios(numHoja);
    });

    $(getIdElemento(numHoja, '#btnGrafico')).unbind('click');
    $(getIdElemento(numHoja, '#btnGrafico')).click(function () {
        btnGrafico(numHoja);
    });

    $(getIdElemento(numHoja, '#btnManttos')).unbind('click');
    $(getIdElemento(numHoja, '#btnManttos')).click(function () {
        btnManttos(numHoja);
    });

    $(getIdElemento(numHoja, '#btnEventos')).unbind('click');
    $(getIdElemento(numHoja, '#btnEventos')).click(function () {
        btnEventos(numHoja);
    });

    $(getIdElemento(numHoja, '#btnLeyenda')).unbind('click');
    $(getIdElemento(numHoja, '#btnLeyenda')).click(function () {
        btnVerLeyenda(numHoja);
    });

    $(getIdElemento(numHoja, '#btnFeriados')).unbind('click');
    $(getIdElemento(numHoja, '#btnFeriados')).click(function () {
        btnVerFeriados(numHoja);
    });

    $(getIdElemento(numHoja, '#btnManualUsuario')).unbind('click');
    $(getIdElemento(numHoja, '#btnManualUsuario')).click(function () {
        btnDescargarManualUsuario(numHoja);
    });

    $(getIdElemento(numHoja, '#btnExpandirRestaurar')).unbind('click');
    $(getIdElemento(numHoja, '#btnExpandirRestaurar')).click(function () {
        btnExpandirRestaurar(numHoja);
    });

}

function cargarHoja(numHoja, formato, id) {
    $.ajax({
        type: 'POST',
        data: {
            idHoja: numHoja,
            idFormato: formato
        },
        url: controlador + "ViewHojaCargaDatos",
        success: function (evt) {
            //quitar htmls existentes
            $(getIdElementoGrafico(numHoja, '')).remove();

            //agregar nuevo html
            $('#' + id).html(evt);

            //IMPORTANTE
            //Existe código dentro de <script type="text/javascript"> del HTML de ViewHojaCargaDatos
            //Esto ejecuta el método que genera el Handsontable

            //mostrar si tiene gráfico            
            if (!getTieneGrafico(numHoja)) {
                $(getIdElemento(numHoja, '#btnGrafico')).parent().hide();
            }
        },
        error: function (err) {
            mostrarErrorPrincipal('Ocurrió un error.');
        }
    });
}

function cargarHojaView(numHojaPrincipal, numHoja, formato, id, nombreHoja) {

    var idHoja = numHoja;
    var htmlTab = `
        <div id='formHoja${idHoja}'>
            <div class='content-hijo' id='mainLayout' style='overflow:auto; background-color:#fff'>

                <div class='content-tabla' style='display:block;'>
                    <div class='bodyexcel' id='detalleFormato${idHoja}'></div>
                </div>

                <div style='clear:both; height:30px'></div>

                <input type='hidden' id='hfHoja' value='${idHoja}' />
                <input type='hidden' id='hfHojaNombre' value='${nombreHoja}' />
            </div>
        </div>
    `;

    //quitar htmls existentes
    $(getIdElementoGrafico(numHoja, '')).remove();

    //agregar nuevo html
    $('#' + id).html(htmlTab);

    setEstaCargado(true, numHoja);
    setNombreHoja(getHojaNombre(numHoja), numHoja);

    //generar handsontable si todos estan cargados
    var cargado = true;
    var listaHoja = getListaHoja(numHojaPrincipal);
    for (var nh = 0; nh < listaHoja.length; nh++) {
        cargado = cargado && getEstaCargado(listaHoja[nh].name);
    }

    if (cargado) {
        btnConsultar(numHojaPrincipal);
    }

}

function inicializarHoja(numHoja) {
    inicializarFormato(numHoja);
    btnConsultar(numHoja);
}

function inicializarHojaView(numHoja) {
    var formato = getIdFormato(numHoja);

    inicializarFormato(numHoja);

    var listaHoja = getListaHoja(numHoja);
    for (var nh = 0; nh < listaHoja.length; nh++) {
        cargarHojaView(numHoja, listaHoja[nh].name, formato, listaHoja[nh].id, listaHoja[nh].hoja.nombreHoja);
    }
}

function mostrarFormulario(numHoja, accion) {
    hideAllMensaje(numHoja);
    setListaErrores([], numHoja);
    setColsToHide([], numHoja);

    var idEnvio = $(getIdElemento(numHoja, "#hfIdEnvio")).val();
    setIdEmpresa(numHoja, getIdEmpresa(numHoja));
    setHorizonte(numHoja, getHorizonte(numHoja));
    $(getIdElemento(numHoja, '#hfEmpresa')).val($(getIdElemento(numHoja, '#cbEmpresa')).val());
    $(getIdElemento(numHoja, '#hfHorizonte')).val($(getIdElemento(numHoja, '#cbHorizonte')).val());

    var verUltimoEnvio = getVerUltimoEnvio(numHoja);

    var codigoFormato = getIdFormato(numHoja);
    var codigoEmpresa = getIdEmpresa(numHoja);
    var cadenaFecha = getFecha(numHoja);
    var configuracionHoja = getConfigHoja(numHoja);

    if (accion == OPCION_CONSULTAR || accion == OPCION_ENVIO_ANTERIOR) {
        var mensaje = "Por favor, espere mientras se carga la información de los filtros seleccionados.";

        $(getIdElemento(numHoja, '#mensaje')).removeClass();
        $(getIdElemento(numHoja, '#mensaje')).addClass("action-message");
        $(getIdElemento(numHoja, '#mensaje')).html(mensaje);
        $(getIdElemento(numHoja, '#mensaje')).css("display", "block");
    }

    $.ajax({
        type: 'POST',
        url: controlador + "MostrarGrilla",
        dataType: 'json',
        data: {
            idEmpresa: codigoEmpresa,
            fecha: cadenaFecha,
            semana: getSemana(numHoja),
            mes: getMes(numHoja),
            idEnvio: idEnvio,
            idFormato: codigoFormato,
            idHoja: getIdHoja(numHoja),
            verUltimoEnvio: verUltimoEnvio,
            tipoinfocodi: getUnidadEnvio(numHoja)
        },
        success: function (evt) {
            if (evt != -1) {
                if (evt.Resultado != '-1') {
                    //
                    actualizarViewHoja(evt, numHoja);

                    if (VER_ULTIMO_ENVIO == verUltimoEnvio) {
                        idEnvio = getVariableEvt(numHoja).IdEnvioLast;
                        setIdEnvio(numHoja, idEnvio);
                        if (idEnvio > 0) {
                            accion = OPCION_ENVIO_ANTERIOR;
                        } else {
                            verUltimoEnvio = NO_VER_ULTIMO_ENVIO;
                        }
                    }

                    //interfaz segun opcion seleccionada
                    hideAllMensaje(numHoja);
                    generaFiltroGrilla(numHoja);
                    switch (accion) {
                        case OPCION_CONSULTAR:
                            var mensaje = obtenerMensajeEnvio(numHoja);
                            mostrarMensajeInformativo(numHoja, mensaje);

                            var mensaje2 = obtenerMensajeAlertaScada(numHoja);
                            mostrarEventoAlerta(numHoja, mensaje2);

                            break;
                        case OPCION_ENVIAR_DATOS:
                            var mensaje = obtenerMensajeEnvio(numHoja, idEnvio);
                            mostrarMensajeExito(numHoja, "Los datos se enviaron correctamente. " + mensaje);

                            verUltimoEnvio = VER_ENVIO;

                            break;
                        case OPCION_ENVIO_ANTERIOR:
                            var mensaje = obtenerMensajeEnvio(numHoja, idEnvio);
                            mostrarMensajeInformativo(numHoja, mensaje);

                            verUltimoEnvio = VER_ENVIO;

                            break;
                        case OPCION_IMPORTAR_DATOS:
                            mostrarEventoCorrecto(numHoja, "<strong>Los datos se cargaron correctamente en el excel web, presione el botón enviar para grabar.</strong>");
                            break;
                    }

                    mostrarOpcionesMenu(numHoja, verUltimoEnvio);
                    mostrarElementosEnvio(numHoja);

                    if (getEsActivoTab(numHoja) && configuracionHoja.tienePanelIEOD) {
                        mostrarPanelIEOD(numHoja);
                    }

                    updateDimensionHandson(numHoja);
                } else {

                }
            }
            else {
                alert("La empresa no tiene puntos de medición para cargar.");
            }
        },
        error: function (err) {
            alert("Error al cargar Excel Web");
        }
    });
}

function mostrarElementosEnvio(numHoja) {
    $(getIdElemento(numHoja, '#filtro_grilla')).css("display", "inline-block");
    $(getIdElemento(numHoja, '#barra')).css("display", "table");
}

function limpiarBarra(numHoja) {
    $(getIdElemento(numHoja, '#barra')).css("display", "none");
    $(getIdElemento(numHoja, '#filtro_grilla')).css("display", "none");
    $(getIdElemento(numHoja, '#detalleFormato' + getIdHoja(numHoja))).html("");
}

function mostrarPanelIEOD(numHoja) {
    var codigoEmpresa = getIdEmpresa(numHoja);
    var cadenaFecha = getFecha(numHoja);
    var codigoFormato = getIdFormato(numHoja);
    var configuracionHoja = getConfigHoja(numHoja);

    //mostrar Panel IEOD
    if (configuracionHoja.tienePanelIEOD) {
        dibujarPanelIeod(codigoFormato, 0, -1, codigoEmpresa, cadenaFecha);
    }
}

//////////////////////////////////////////////////////////
//// btnConsultar
//////////////////////////////////////////////////////////
function btnConsultar(numHoja) {
    mostrarExcelWeb(numHoja);
}

function mostrarExcelWeb(numHoja) {
    limpiarBarra(numHoja);
    if ($(getIdElemento(numHoja, "#txtFecha")).val() != "" || $(getIdElemento(numHoja, "#hfSemana")).val() != "" || $(getIdElemento(numHoja, "#hfMes")).val()) {
        setIdEnvio(numHoja, 0);
        mostrarFormulario(numHoja, OPCION_CONSULTAR);
    }
    else {
        alert("Error!.Ingresar fecha correcta");
    }
}

function actualizarViewHoja(evt, numHoja) {
    //la variable evt es para todos los views, pero se crearan varios handsontable
    setVariableEvt(evt, numHoja);
    setVerUltimoEnvio(NO_VER_ULTIMO_ENVIO, numHoja);

    //establecer valores de handson
    if (!getTieneHojaView(numHoja)) {
        //validacion de formato
        getErrores(numHoja)[ERROR_BLANCO].validar = evt.ValidacionFormatoCheckblanco;
        setTieneHOP(evt.ValidaHorasOperacion, numHoja);

        if (typeof getVariableHot(numHoja) != 'undefined') {
            getVariableHot(numHoja).destroy();
        }

        crearGrillaExcel(evt, numHoja);
    } else {
        var listaHoja = getListaHoja(numHoja);
        for (var nh = 0; nh < listaHoja.length; nh++) {
            //la variable evt
            var numHojaView = listaHoja[nh].name

            if (typeof getVariableHot(numHojaView) != 'undefined') {
                getVariableHot(numHojaView).destroy();
            }

            var evtView = JSON.parse(JSON.stringify(evt));

            //copiar variables del formatomodel a cada hoja
            evtView.Handson = evt.ListaFormatoModel[nh].Handson;
            evtView.ListaCambios = evt.ListaFormatoModel[nh].ListaCambios;
            evtView.ListaHojaPto = evt.ListaFormatoModel[nh].ListaHojaPto;
            evtView.ListaFeriado = evt.ListaFormatoModel[nh].ListaFeriado;

            setVariableEvt(evtView, numHojaView);
            setVerUltimoEnvio(NO_VER_ULTIMO_ENVIO, numHoja);

            crearGrillaExcel(evtView, numHojaView);

            //validacion de formato
            getErrores(numHojaView)[ERROR_BLANCO].validar = true;
        }
    }
}

function mostrarOpcionesMenu(numHoja, verUltimoEnvio) {

    //opciones a mostrar
    if (getVariableEvt(numHoja).Handson.ReadOnly) {
        $(getIdElemento(numHoja, '#btnSelectExcel' + numHoja)).parent().css("display", "none");
        $(getIdElemento(numHoja, '#btnEnviarDatos')).parent().css("display", "none");
        $(getIdElemento(numHoja, '#btnMostrarErrores')).parent().css("display", "none");
    }
    else {
        $(getIdElemento(numHoja, '#btnSelectExcel' + numHoja)).parent().css("display", "table-cell");
        $(getIdElemento(numHoja, '#btnEnviarDatos')).parent().css("display", "table-cell");
        $(getIdElemento(numHoja, '#btnMostrarErrores')).parent().css("display", "table-cell");
    }

    if (getTieneHOP(numHoja)) {
        $(getIdElemento(numHoja, '#btnHOP')).parent().css("display", "table-cell");
    } else {
        $(getIdElemento(numHoja, '#btnHOP')).parent().css("display", "none");
    }

    if (VER_ULTIMO_ENVIO == verUltimoEnvio || VER_ENVIO == verUltimoEnvio) {
        $(getIdElemento(numHoja, '#btnEditarEnvio')).parent().css("display", "table-cell");
    } else {
        $(getIdElemento(numHoja, '#btnEditarEnvio')).parent().css("display", "none");
    }

    if (getVariableEvt(numHoja).ValidaMantenimiento) {
        $(getIdElemento(numHoja, '#btnManttos')).parent().css("display", "table-cell");
    } else {
        $(getIdElemento(numHoja, '#btnManttos')).parent().css("display", "none");
    }

    if (getVariableEvt(numHoja).ValidaEventos) {
        $(getIdElemento(numHoja, '#btnEventos')).parent().css("display", "table-cell");
    } else {
        $(getIdElemento(numHoja, '#btnEventos')).parent().css("display", "none");
    }


    horizonte(numHoja, getHorizonte(numHoja));
}

function horizonte(numHoja, periodo) {
    //setIdEnvio(numHoja, Model.IdEnvio);

    /*
    $("#hfIdEnvioMain").val(Model.IdEnvio);
    $("#hfFormatoMain").val(Model.IdFormato);
    $("#hfEmpresaMain").val(Model.IdEmpresa);
    $("#hfFechaMain").val(Model.Fecha);
    $("#hfSemanaMain").val(Model.NroSemana);
    $("#hfMesMain").val(Model.Mes);

    $("#txtFecha").val(Model.Dia);
    $("#Anho").val(Model.Anho);
    $("#txtMes").val(Model.Mes);
    */
    var periodoInput = parseInt(periodo) || 0;
    switch (periodoInput) {
        case 1: //dia
            $(getIdElemento(numHoja, '.cntFecha')).css("display", "table-cell");
            $(getIdElemento(numHoja, '.cntSemana')).css("display", "none");
            $(getIdElemento(numHoja, '.cntMes')).css("display", "none");
            break;
        case 2: //semanal
            $(getIdElemento(numHoja, '.cntFecha')).css("display", "none");
            $(getIdElemento(numHoja, '.cntSemana')).css("display", "table-cell");
            $(getIdElemento(numHoja, '.cntMes')).css("display", "none");

            //cargarSemanaAnho(numHoja, periodo);

            break;
        //mensual

        //break;
        case 3: case 5:
            $(getIdElemento(numHoja, '.cntFecha')).css("display", "none");
            $(getIdElemento(numHoja, '.cntSemana')).css("display", "none");
            $(getIdElemento(numHoja, '.cntMes')).css("display", "inline");
            $(getIdElemento(numHoja, '.cntMes.divmes')).css("display", "block");
            break;
    }
}

//////////////////////////////////////////////////////////
//// btnDescargarFormato
//////////////////////////////////////////////////////////
function btnDescargarFormato(numHoja) {
    if (validarSeleccionDatos(numHoja)) {
        descargarFormato(numHoja);
    }
    else {
        alert("Por favor, seleccione la empresa correcta.");
    }
}

function descargarFormato(numHoja) {
    var data = getData(numHoja);
    var idEnvio = $(getIdElemento(numHoja, "#hfIdEnvio")).val();
    var listaHoja = getListaDataNumHoja(numHoja);
    var listaData = getListaData(numHoja);

    $.ajax({
        type: 'POST',
        async: true,
        contentType: 'application/json',
        url: controlador + 'generarformato',
        data: JSON.stringify({
            data: data,
            idEmpresa: getIdEmpresa(numHoja),
            dia: getFecha(numHoja),
            fecha: getFecha(numHoja),
            semana: getSemana(numHoja),
            mes: getMes(numHoja),
            idFormato: getIdFormato(numHoja),
            listaHoja: listaHoja,
            listaData: listaData,
            idHoja: getIdHoja(numHoja),
            idEnvio: idEnvio,
            tipoinfocodi: getUnidadEnvio(numHoja)
        }),
        beforeSend: function () {
            mostrarEventoCorrecto(numHoja, "Descargando información ...");
        },
        success: function (model) {
            if (model.Resultado != "-1") {
                mostrarEventoCorrecto(numHoja, "<strong>Los datos se descargaron correctamente</strong>");
                window.location.href = controlador + 'descargarformato?archivo=' + encodeURIComponent(model.Resultado);
            }
            else {
                mostrarEventoError(numHoja, "Error en descargar el archivo");
            }
        },
        error: function (result) {
            mostrarEventoError(numHoja, 'Ha ocurrido un error al descargar el archivo excel. ' + result.status + ' - ' + result.statusText + '.');
        }
    });
}

//////////////////////////////////////////////////////////
//// btnSelectExcel
//////////////////////////////////////////////////////////
function btnSelectExcel(numHoja) {
    if (CONTADOR_UPLOADER == 0) {
        var url = controlador + '/Upload';
        $("#btnSelectExcel" + numHoja).unbind();

        crearPupload(numHoja, url);
        setCantidadClickImportar(1, numHoja);
    }
}

function crearPupload(numHoja, url) {
    var uploader = new plupload.Uploader({
        runtimes: 'html5,flash,silverlight,html4',
        browse_button: 'btnSelectExcel' + numHoja,
        url: url,
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
                mostrarEventoAlerta(numHoja, "Por favor, espere ...(<strong>" + file.percent + "%</strong>)");
            },
            UploadComplete: function (up, file) {
                leerFileUpExcel(numHoja);
            },
            Error: function (up, err) {
                mostrarEventoError(numHoja, err.code + "-" + err.message);
            }
        }
    });

    uploader.init();
}

function leerFileUpExcel(numHoja) {
    mostrarEventoAlerta(numHoja, "Por favor, espere mientras se procesa el archivo seleccionado.");

    $.ajax({
        type: 'POST',
        url: controlador + 'LeerFileUpExcel',
        dataType: 'json',
        async: true,
        data: {
            idEmpresa: getIdEmpresa(numHoja),
            fecha: getFecha(numHoja),
            semana: getSemana(numHoja),
            mes: getMes(numHoja),
            idFormato: getIdFormato(numHoja),
            tieneHojaView: getTieneHojaView(numHoja),
            idHoja: getIdHoja(numHoja),
            tipoinfocodi: getUnidadEnvio(numHoja)
        },
        success: function (model) {
            if (model.Resultado == "1") {
                limpiarError(numHoja);
                setIdEnvio(numHoja, -1); //-1 indica que el handsontable mostrara datos del archivo excel         

                mostrarFormulario(numHoja, OPCION_IMPORTAR_DATOS);
            } else {
                mostrarEventoError(numHoja, "Error: " + model.Mensaje);
            }
        },
        error: function (err) {
            mostrarEventoError(numHoja, "Ha ocurrido un error");
        }
    });
}

//////////////////////////////////////////////////////////
//// btnEnviarDatos
//////////////////////////////////////////////////////////
function btnEnviarDatos(numHoja) {
    if (getVariableEvt(numHoja).Handson.ReadOnly) {
        alert("No se puede enviar información, solo diponible para  visualización");
        return
    }
    else {
        enviarExcelWeb(numHoja);
    }
}

function enviarExcelWeb(numHoja) {
    if (validarEnvio(numHoja)) {

        ARRAY_FILES_GRABAR_DATOS = [];
        $("#enviar-txt-comment").val('');
        $("#enviar-div-attachments").html('');

        setTimeout(function () {
            $('#popupEnviarCOES').bPopup({
                easing: 'easeOutBack',
                speed: 450,
                transition: 'slideDown',
                modalClose: false
            });
        }, 50);

        //envioData(numHoja);
    }
}

function validarEnvio(numHoja) {
    generarListaErroresFromData(numHoja);
    var existeErrores = existeListaErrores(numHoja);

    //valida si existen errores
    if (existeErrores) {
        mostrarEventoError(numHoja, "Existen errores en las celdas, favor de corregir y vuelva a envíar");
        mostrarDetalleErrores(numHoja);
        return false;
    }

    return true;
}

function envioData(numHoja) {
    var empresa = getIdEmpresa(numHoja);
    setIdEmpresa(numHoja, empresa);

    var data = getData(numHoja);
    var listaHoja = getListaDataNumHoja(numHoja);
    var listaData = getListaData(numHoja);

    var text = $("#enviar-txt-comment").val();
    var files = JSON.stringify(ARRAY_FILES_GRABAR_DATOS);

    $('#popupEnviarCOES').bPopup().close();

    //78puntos demoran 65 segundos 
    var secXPto = (1 * 65.0 / 78.0); //0.833
    var totalPto = parseInt(getListaPtos(numHoja).length / 2) || 1;
    var porcentajeXSegundo = Math.round(100.0 / (secXPto * totalPto), 2);

    var porcentajeAvance = 0;
    var flagActivarRefresco = "S";
    var intervalEnvio = setInterval(function () {
        porcentajeAvance += porcentajeXSegundo;
        if (porcentajeAvance > 95) porcentajeAvance = 95;

        if (flagActivarRefresco == "S") {
            mostrarEventoCorrecto(numHoja, "Enviando Información al COES.. (" + Math.round(porcentajeAvance, 0) + "%)");
        }

    }, 1000);

    $.ajax({
        type: 'POST',
        dataType: 'json',
        //async: false,
        contentType: 'application/json',
        traditional: true,
        url: controlador + "GrabarExcelWeb",
        data: JSON.stringify({
            data: data,
            idEmpresa: getIdEmpresa(numHoja),
            fecha: getFecha(numHoja),
            semana: getSemana(numHoja),
            mes: getMes(numHoja),
            idFormato: getIdFormato(numHoja),
            listaHoja: listaHoja,
            listaData: listaData,
            idHoja: getIdHoja(numHoja),
            tipoinfocodi: getUnidadEnvio(numHoja),
            text: text,
            files: files
        }),
        beforeSend: function () {
            mostrarEventoCorrecto(numHoja, "Enviando Información al COES..");
        },
        success: function (evt) {
            clearInterval(intervalEnvio);
            flagActivarRefresco = "N";

            if (evt.Resultado != "-1") {
                var idEnvio = parseInt(evt.Resultado) || 0;
                if (idEnvio == 0) {
                    mostrarEventoError(numHoja, "Error al Grabar: " + "No existe actualización de datos.");
                    $('#popupEnviarCOES').bPopup().close();
                } else {
                    setIdEnvio(numHoja, idEnvio);

                    ARRAY_FILES_GRABAR_DATOS = [];
                    $("#enviar-txt-comment").val('');
                    $("#enviar-div-attachments").html('');

                    //////////////////////////////////////////////////////////
                    mostrarFormulario(numHoja, OPCION_ENVIAR_DATOS);
                    mostrarEventoCorrecto(numHoja, "Los datos se enviaron correctamente. Por favor, espere mientras se actualiza la pantalla con los datos del envío.");
                }
            }
            else {
                mostrarEventoError(numHoja, "Error al Grabar: " + evt.Mensaje);
                $('#popupEnviarCOES').bPopup().close();
            }
        },
        error: function (err) {
            clearInterval(intervalEnvio);

            mostrarEventoError();
        }
    });
}

function btnFileAttachEnvio() {
    if (CONTADOR_UPLOADER == 0) {
        crearUploaderMensajeEnvioCOES();
    }
}

function crearUploaderMensajeEnvioCOES() {

    var fupAttachmentCoes = new plupload.Uploader({
        runtimes: 'html5,flash,silverlight,html4',
        browse_button: 'enviar-btn-file-attach',
        url: controlador + 'FileAttachment',
        multi_selection: false,
        unique_names: true,
        init: {
            FilesAdded: function (sender, files) {
                if (fupAttachmentCoes.files.length == 2) {
                    fupAttachmentCoes.removeFile(fupAttachmentCoes.files[0]);
                }

                $("#enviar-div-message-mensaje").hide();

                plupload.each(files, function (file) {
                    var item = $('<div>'),
                        btnFileRemove = $('<a>');

                    btnFileRemove
                        .attr('href', 'javascript:void(0)')
                        .html('x');

                    item.append(file.name);
                    item.append('&nbsp;');
                    item.append(btnFileRemove);

                    $('#enviar-div-attachments').append(item);
                });

                $('#enviar-div-attachments').show();

                fupAttachmentCoes.start();
                sender.refresh();
            },
            UploadComplete: function (sender, files) {
                var arrFiles;
                if (files != null && files.length > 0) {
                    arrFiles = [];

                    files.forEach(function (file) {
                        arrFiles.push({
                            fileName: file.name,
                            tmpFileName: file.target_name
                        });
                    });
                }

                //quitar del uploader los archivos subidos con anterioridad
                $.each(fupAttachmentCoes.files, function (i, file) {
                    if (file && file !== undefined) {
                        fupAttachmentCoes.removeFile(file);
                    }
                });

                ARRAY_FILES_GRABAR_DATOS = ARRAY_FILES_GRABAR_DATOS.concat(arrFiles);
            },
            Error: function (sender, e) {
                var message = "\nError #" + e.code + ": " + e.message.substring(0, e.message.length - 1),
                    sfxMessage;

                switch (e.code) {
                    case -600:
                        sfxMessage = e.file.size + ' bytes';
                        break;
                    case -601:
                        sfxMessage = e.file.type;
                        break;
                    case -200:
                        sfxMessage = e.status;
                        break;
                }

                if (sfxMessage) {
                    message += ' - ' + sfxMessage;
                }

                message += '.';

                $("#enviar-div-message-mensaje").show();
                $("#enviar-div-message-mensaje").html(message);
            }
        }
    });

    fupAttachmentCoes.init();
}

//////////////////////////////////////////////////////////
//// btnMostrarErrores
//////////////////////////////////////////////////////////
function btnMostrarErrores(numHoja) {
    mostrarDetalleErrores(numHoja);
}

//////////////////////////////////////////////////////////
//// btnVerJustificaciones
//////////////////////////////////////////////////////////
function btnVerJustificaciones(numHoja) {
    popUpListaJustificaciones(numHoja);
}

//////////////////////////////////////////////////////////
//// btnVerEnvios
//////////////////////////////////////////////////////////
function btnVerEnvios(numHoja) {
    popUpListaEnvios(numHoja);
}

function mostrarEnvioExcelWeb(envio, numHoja) {
    $('#enviosanteriores').bPopup().close();
    setIdEnvio(numHoja, envio);
    mostrarFormulario(numHoja, OPCION_ENVIO_ANTERIOR);
}

//////////////////////////////////////////////////////////
//// btnManttos
//////////////////////////////////////////////////////////
function btnManttos(numHoja) {
    popupManttos(numHoja);
}

//////////////////////////////////////////////////////////
//// btnEventos
//////////////////////////////////////////////////////////
function btnEventos(numHoja) {
    popupEventos(numHoja);
}

//////////////////////////////////////////////////////////
//// btnVerLeyenda
//////////////////////////////////////////////////////////
function btnVerLeyenda(numHoja) {
    popupLeyenda(numHoja);
}

//////////////////////////////////////////////////////////
//// btnVerFeriados
//////////////////////////////////////////////////////////
function btnVerFeriados(numHoja) {
    popupFeriados(numHoja);
}

//////////////////////////////////////////////////////////
//// btnDescargarManualUsuario
//////////////////////////////////////////////////////////
function btnDescargarManualUsuario(numHoja) {
    descargarManualUsuario(numHoja);
}

//////////////////////////////////////////////////////////
//// btnGrafico
//////////////////////////////////////////////////////////
function btnGrafico(numHoja) {
    mostrarGrafico(numHoja);
}

//////////////////////////////////////////////////////////
//// btnExpandirRestaurar
//////////////////////////////////////////////////////////
function btnExpandirRestaurar(numHoja) {
    expandirRestaurar(numHoja);
}

//////////////////////////////////////////////////////////
//// btnHOP
//////////////////////////////////////////////////////////
function btnHOP(numHoja) {
    popupListaHOP(numHoja);
}

//////////////////////////////////////////////////////////
//// Varios Formatcodi en una Pantalla
//////////////////////////////////////////////////////////

//////////////////////////////////////////////////////////
//// cbEmpresa
//////////////////////////////////////////////////////////
function cargarFormatosXEmpresa(numHoja) {
    var emprcodi = $(getIdElemento(numHoja, '#cbEmpresa')).val();

    $.ajax({
        type: 'POST',
        url: controlador + 'CargarFormatoXEmpresa',
        dataType: 'json',
        async: false,
        data: {
            emprcodi: emprcodi
        },
        success: function (model) {
            $(getIdElemento(numHoja, '#cbTipoFormato')).unbind('change');
            $(getIdElemento(numHoja, '#cbTipoFormato')).empty();

            if (model.ListaFormato.length == 0) {
                $(getIdElemento(numHoja, '#cbTipoFormato')).append('<option value="0" selected="selected"> -No existe- </option>');
            }
            for (var i = 0; i < model.ListaFormato.length; i++) {
                var obj = model.ListaFormato[i];
                $(getIdElemento(numHoja, '#cbTipoFormato')).append('<option value="' + obj.Formatcodi + '">' + obj.Formatnombre + '</option>');
            }

            $(getIdElemento(numHoja, '#cbTipoFormato')).change(function () {
                setVerUltimoEnvioGlobal(MAIN_VER_ULTIMO_ENVIO);
                cargarFormato(numHoja);
            });
            cargarFormato(numHoja);
        },
        error: function (err) {
            alert("Ocurrió un error");
        }
    });
}
//////////////////////////////////////////////////////////
//// cbTipoFormato
//////////////////////////////////////////////////////////

function cargarFormato(numHojaPrincipal) {
    /*//Panel IEOD
    tipoFormato = 2;
    idFormato = idFormatoDemandaDiaria;
    idFuentedatos = 0;
    idPosFormato = 1;
    dibujarPanelIeod(tipoFormato, idPosFormato, -1);
    */

    //Configuracion Tab
    LISTA_OBJETO_HOJA = {};
    var prefijo = 'formHoja';

    $("#mensaje").hide();
    //$("#cargaDatos").html('');
    $("#tab-container-padre").hide();
    var formatcodi = $(getIdElemento(numHojaPrincipal, '#cbTipoFormato')).val();
    $.ajax({
        type: 'POST',
        url: controlador + 'CargarFormato',
        dataType: 'json',
        async: false,
        data: {
            formatcodi: formatcodi
        },
        success: function (model) {
            if (model.ListaMeHojaPadre != null && model.ListaMeHojaPadre.length > 0) {
                cargarListaTabsFromHojas(model.ListaMeHojaPadre, numHojaPrincipal, 'tab-container-padre', prefijo, true);
                for (var i = 0; i < model.ListaMeHojaPadre.length; i++) {
                    var numHoja = model.ListaMeHojaPadre[i].Hojacodi;
                    var numHojaStr = numHoja + '';

                    LISTA_OBJETO_HOJA[numHojaStr] = crearObjetoHoja();
                    setEsHojaPadre(true, numHojaStr);

                    inicializarFormatoView(numHojaPrincipal, numHojaStr, formatcodi, prefijo + numHoja, model, listarHojaHijo(numHoja, model.ListaMeHoja));
                }
            } else {
                var numHoja = 0;
                var numHojaStr = numHoja + '';

                LISTA_OBJETO_HOJA[numHojaStr] = crearObjetoHoja();
                setEsHojaPadre(true, numHojaStr);

                inicializarFormatoView(numHojaPrincipal, numHojaStr, formatcodi, 'cargaDatos', model, listarHojaHijo(numHoja, model.ListaMeHoja));
            }

            CONTADOR_UPLOADER++;
        },
        error: function (err) {
            alert("Ocurrió un error");
        }
    });
}

function listarHojaHijo(hojacodipadre, lista) {
    var l = [];

    if (lista != null) {
        for (var i = 0; i < lista.length; i++) {
            if (lista[i].Hojapadre == hojacodipadre) {
                l.push(lista[i]);
            }
        }
    }

    return l;
}

function cargarListaTabsFromHojas(listaHoja, numHoja, idContainerTab, prefijo, esPadre, tieneGrafico) {
    var idContainer = idContainerTab.replace('#', '');
    if (listaHoja != null && listaHoja.length > 0) {
        if (prefijo == 'formHoja' || esPadre) {
            $("#cargaDatos").html("<div id='" + idContainer + "' class='tab-container'>        <ul class='etabs'>        </ul>        <div class='panel-container'>        </div>    </div>");
        }
        if ('tab-container' == idContainer)
            $("#cargaDatos").html("<div id='" + idContainer + "' class='tab-container'>        <ul class='etabs'>        </ul>        <div class='panel-container'>        </div>    </div>");


        $('#' + idContainer).unbind();

        $(getIdElemento(numHoja, '#' + idContainer) + " ul.etabs").html('');
        $(getIdElemento(numHoja, '#' + idContainer) + " div.panel-container").html('');
        var strTabLi = '';
        var strTabDiv = '';

        for (var i = 0; i < listaHoja.length; i++) {
            var hoja = listaHoja[i];
            //if (i == 0) prefijo += hoja.Hojacodi;
            var idTab = prefijo + hoja.Hojacodi;
            var classActive = i == 0 ? 'active' : '';
            var styleDiv = i == 0 ? 'style="display: block;"' : 'style="display: none;"';
            //var classActive = '';
            //var styleDiv = '';

            strTabLi += `<li class="tab ${classActive}"><a href="#${idTab}" id="idView${hoja.Hojacodi}" onclick="actualizarPanelTab('${numHoja}','${idContainer}','${idTab}')"> ${hoja.Hojanombre} </a></li>                        `;
            strTabDiv += `<div id="${idTab}" class="${classActive}" ${styleDiv}></div>`;
        }

        if (tieneGrafico) {
            var numHoja1 = listaHoja[0].Hojacodi;
            var numHoja2 = listaHoja[1].Hojacodi;
            strTabLi += `<li class="tab "><a href="#graficoTab" id="idViewGrafico" onclick="actualizarGrafico('${numHoja}','${idContainer}','${numHoja1}','${numHoja2}')"> Gráficos </a></li>`;
            strTabDiv += `<div id="graficoTab" class="" style="display: none;"></div>`;
        }

        $(getIdElemento(numHoja, '#' + idContainer) + " ul.etabs").html(strTabLi);
        $(getIdElemento(numHoja, '#' + idContainer) + " div.panel-container").html(strTabDiv);

        $(getIdElemento(numHoja, '#' + idContainer)).show();
        $(getIdElemento(numHoja, '#' + idContainer)).easytabs({
            animate: false
        });

        var idViewDefecto = prefijo + listaHoja[0].Hojacodi;
        $(getIdElemento(numHoja, '#' + idContainer)).easytabs('select', '#' + idViewDefecto);
    }
}

function actualizarPanelTab(numHoja, idContainer, id) {

    $(getIdElemento(numHoja, '#' + idContainer)).easytabs('select', '#' + id);

    //Actualizar Hanson
    updateDimensionHandson(numHoja);

    //Mostrar panel
    mostrarPanelIEOD(numHoja);
}

function actualizarGrafico(numHoja, idContainer, idHoja1, idHoja2) {
    $(getIdElemento(numHoja, '#' + idContainer)).easytabs('select', '#idViewGrafico');

    $("#graficoTab").html('');

    var listaHpto = getListaPtos(idHoja1);
    var listaCombo = listarPtomedinombFromHojaptomed(listaHpto);
    var listaTpto = listarTptoFromHojaptomed(listaHpto, listaCombo[0].Ptomedinomb);

    var htmlComboPto = '';
    for (var i = 0; i < listaCombo.length; i++) {
        var regPto = listaCombo[i];
        htmlComboPto += `<option value="${regPto.Ptomedinomb}">${regPto.Ptomedinomb}</option>`;
    }

    var htmlComboTpto = '';
    for (var i = 0; i < listaTpto.length; i++) {
        var regTpto = listaTpto[i];
        htmlComboTpto += `<option value="${regTpto.Tptomedicodi}">${regTpto.Tipoptomedinomb}</option>`;
    }

    $("#cbPtoMedicion").unbind();
    $("#cbTptoMedicion").unbind();

    var htmlG = `
        <div class="search-content2" id="filtro_grilla" style="display: inline-block;">
            <div style="display:table">
                <div style="display:table-row">

                    <div class="filtro-label" style="display: table-cell; width: 110px;">
                        <label>Punto de medición:</label>
                    </div>
                    <div class="filtro-contenido div_pto" style="display:table-cell;width:220px;">
                        <select id="cbPtoMedicion" style="width:220px;">
                            ${htmlComboPto}
                        </select>
                    </div>

                    <div class="filtro-label" style="display: table-cell; width: 30px; padding-left: 10px;">
                        <label>Tipo:</label>
                    </div>
                    <div class="filtro-contenido div_tptomedicodi" style="display:table-cell;width:128px;">
                        <select id="cbTptoMedicion" style="width:128px;">
                            ${htmlComboTpto}
                        </select>
                    </div>

                </div>
            </div>
        </div>

        <div id="gfx-1" style="height: 400px; width: 1028px;"></div>

        <div id="gfx-2" style="height: 400px; width: 1028px;"></div>

    `;
    $("#graficoTab").html(htmlG);

    $("#cbPtoMedicion").change(function () {

        $("#cbTptoMedicion").empty();

        var listaHpto2 = getListaPtos(idHoja1);
        var listaTpto2 = listarTptoFromHojaptomed(listaHpto2, $("#cbPtoMedicion").val());

        var htmlComboTpto2 = '';
        for (var i = 0; i < listaTpto2.length; i++) {
            var regTpto = listaTpto2[i];
            htmlComboTpto2 += `<option value="${regTpto.Tptomedicodi}">${regTpto.Tipoptomedinomb}</option>`;
        }

        $("#cbTptoMedicion").html(htmlComboTpto2);

        dibujarHojas($("#cbPtoMedicion").val(), $("#cbTptoMedicion").val(), idHoja1, idHoja2);
    });
    $("#cbTptoMedicion").change(function () {
        dibujarHojas($("#cbPtoMedicion").val(), $("#cbTptoMedicion").val(), idHoja1, idHoja2);
    });

    dibujarHojas(listaCombo[0].Ptomedinomb, listaTpto[0].Tptomedicodi, idHoja1, idHoja2);
}

function dibujarHojas(ptomedinomb, tptomedicodi, idHoja1, idHoja2) {
    dibujarGraficoPmpo('gfx-1', ' INF-HISTORICA ', idHoja1, ptomedinomb, tptomedicodi);
    dibujarGraficoPmpo('gfx-2', ' INF-PRONOSTICADA ', idHoja2, ptomedinomb, tptomedicodi);
}

function dibujarGraficoPmpo(idGrafico, titulo, numHoja, ptomedinomb, tptomedicodi) {
    var listaHpto = getListaPtos(numHoja);
    var posPtoCol = buscarPtoyHptoFromHojaptomed(listaHpto, ptomedinomb, tptomedicodi) + 6;
    var posBloqueCol = 4;
    var posFechaCol = 1;

    var data = getVariableHot(numHoja).getData();

    var blocks = [{ id: 1, text: '1' },
    { id: 2, text: '2' },
    { id: 3, text: '3' },
    { id: 4, text: '4' },
    { id: 5, text: '5' }];

    var series = [];

    if (posPtoCol >= 6) {
        for (var block = 1, bc = blocks.length; block <= bc; block++) {
            var info = [];

            for (var m = 3, n = data.length; m < n; m++) {
                if (data[m][posBloqueCol] == block) {
                    var date = moment(data[m][posFechaCol], 'DD/MM/YYYY');
                    var value = data[m][posPtoCol];

                    info.push([date.valueOf(), ($.isNumeric(value) ? parseFloat(value) : 0)]);
                }
            }

            if (info != null && info.length > 0) {

                series.push({
                    name: ('Bloque ' + block),
                    data: info,
                    dataGrouping: {
                        enabled: false
                    },
                    tooltip: {
                        valueDecimals: 2
                    }
                });
            }
        }

        $('#' + idGrafico).highcharts('StockChart', {
            rangeSelector: { selected: 1 },
            title: { text: titulo },
            series: series
        });
    } else {
        $('#' + idGrafico).html('');
    }

}

function inicializarFormatoView(numHojaPrincipal, numHoja, formato, id, model, listaHojaModel) {


    //cargarListaEmpresa(numHoja, model.ListaEmpresas);

    setEstaCargado(true, numHoja);
    setNombreHoja(getHojaNombre(numHoja), numHoja);

    inicializarFormatoViewTab(listaHojaModel, numHoja);


    var htmlTab = `

`;

}

function cargarListaEmpresa(numHoja, lista) {
    $(getIdElemento(numHoja, '#cbEmpresa')).empty();
    for (var i = 0; i < lista.length; i++) {
        $(getIdElemento(numHoja, '#cbEmpresa')).append('<option value=' + lista[i].Emprcodi + '>' + lista[i].Emprnomb + '</option>');
    }
}

function inicializarFormatoViewTab(listaHoja, hojaPrincipal) {
    var listaHojaView = [];

    if (listaHoja != null) {
        var prefijo = "view";
        cargarListaTabsFromHojas(listaHoja, hojaPrincipal, 'tab-container', prefijo, false, true); //hojaFormato.js

        for (var i = 0; i < listaHoja.length; i++) {
            var hoja = listaHoja[i];
            var keyHoja = hoja.Hojacodi.toString();
            LISTA_OBJETO_HOJA[keyHoja] = crearObjetoHoja();

            listaHojaView.push(crearHojaView(keyHoja, 'view' + keyHoja, getHoja(keyHoja)));

            setNombreHoja(hoja.Hojanombre, keyHoja);
            getErrores(keyHoja)[ERROR_NO_NUMERO].validar = true;
            getErrores(keyHoja)[ERROR_LIM_INFERIOR].validar = true;
            getErrores(keyHoja)[ERROR_LIM_SUPERIOR].validar = true;
            getErrores(keyHoja)[ERROR_BLANCO].validar = true;

        }
    }

    //configuración de la hoja
    setListaHoja(listaHojaView, hojaPrincipal);
    setTieneFiltro(true, hojaPrincipal);
    setValidacionDataCongelada(false, hojaPrincipal);

    var config = crearConfigHoja();
    config.tieneFiltroArea = true;
    config.tieneFiltroSubestacion = true;
    config.tieneFiltroFormato = true;
    config.tienePanelIEOD = false;
    config.verUltimoEnvio = true;
    config.tieneFiltroEmpresaPmpo = true;

    setConfigHoja(config, hojaPrincipal);

    //crear vistas
    inicializarHojaView(hojaPrincipal); //hojaFormato.js
}

//////////////////////////////////////////////////////////
//// FILTROS
//////////////////////////////////////////////////////////
function cargarSemanaAnho(numHoja, periodo) {
    var anho = getAnho(numHoja);
    var semana = anho + "1";

    //if (periodo == 2) {
    $.ajax({
        type: 'POST',
        url: controlador + 'CargarSemanas',
        async: false,
        data: {
            idAnho: anho
        },
        success: function (aData) {
            $(getIdElemento(numHoja, "#divSemana")).html(aData);

            $(getIdElemento(numHoja, '#cbSemana')).unbind('change');
            $(getIdElemento(numHoja, '#cbSemana')).change(function () {
                btnConsultar(numHoja);
            });

            if (numHoja != "")
                btnConsultar(numHoja);
        },
        error: function (err) {
            alert("Ha ocurrido un error");
        }
    });
    //}
}

function hideColumnaGrilla(numHoja) {
    var listaHideSize = [];
    var central = $(getIdElemento(numHoja, "#cbCentral")).val();
    var area = $(getIdElemento(numHoja, "#cbArea")).val();
    var subestacion = $(getIdElemento(numHoja, "#cbSubestacion")).val();
    var tipoEquipo = $(getIdElemento(numHoja, "#cbFamilia")).val();

    var configuracionHoja = getConfigHoja(numHoja);

    //Solo filtrar centrales
    if (configuracionHoja.tieneFiltroCentral && !configuracionHoja.tieneFiltroArea && !configuracionHoja.tieneFiltroSubestacion && !configuracionHoja.tieneFiltroFamilia) {
        for (var i = 0; i < getListaPtos(numHoja).length; i++) {
            var equipadrePto = getListaPtos(numHoja)[i].Equipadre;
            if (equipadrePto == central || central == 0) { }
            else { listaHideSize.push(i + 1); }
        }

        updateHandsonColVisible(numHoja, listaHideSize);
    }

    //Solo filtrar por area y subestaciones
    if (!configuracionHoja.tieneFiltroCentral && configuracionHoja.tieneFiltroArea && configuracionHoja.tieneFiltroSubestacion && configuracionHoja.tieneFiltroFamilia) {
        var listaHoja = getListaHoja(numHoja);

        if (listaHoja.length == 0) {
            var numTabHoja = numHoja;

            for (var i = 0; i < getListaPtos(numTabHoja).length; i++) {
                var ptoAreaoperativa = getListaPtos(numTabHoja)[i].AreaOperativa;
                var ptoSubestacion = getListaPtos(numTabHoja)[i].Areacodi;
                var ptoTipoEquipo = getListaPtos(numTabHoja)[i].Famcodi;
                if ((ptoAreaoperativa == area || area == 0 || area == "0")
                    && (subestacion == ptoSubestacion || subestacion == 0)
                    && (tipoEquipo == ptoTipoEquipo || tipoEquipo == 0)
                ) { }
                else { listaHideSize.push(i + 1); }
            }

            updateHandsonColVisible(numTabHoja, listaHideSize);
        }
        else {
            for (var nh = 0; nh < listaHoja.length; nh++) {
                listaHideSize = [];
                var numTabHoja = listaHoja[nh].name;

                for (var i = 0; i < getListaPtos(numTabHoja).length; i++) {
                    var ptoAreaoperativa = getListaPtos(numTabHoja)[i].AreaOperativa;
                    var ptoSubestacion = getListaPtos(numTabHoja)[i].Areacodi;
                    var ptoTipoEquipo = getListaPtos(numTabHoja)[i].Famcodi;
                    if ((ptoAreaoperativa == area || area == 0 || area == "0")
                        && (subestacion == ptoSubestacion || subestacion == 0)
                        && (tipoEquipo == ptoTipoEquipo || tipoEquipo == 0)
                    ) { }
                    else { listaHideSize.push(i + 1); }
                }

                updateHandsonColVisible(numTabHoja, listaHideSize);
            }
        }
    }
}

function updateHandsonColVisible(numHoja, listaHideSize) {
    getVariableHot(numHoja).updateSettings({
        hiddenColumns: {
            columns: listaHideSize
        }
    });

    getVariableHot(numHoja).render();
}

function generaFiltroGrilla(numHoja) {
    if (getTieneFiltro(numHoja)) {
        var configuracionHoja = getConfigHoja(numHoja);

        if (configuracionHoja.tieneFiltroCentral) {
            //generar el filtro combo de centrales
            $(getIdElemento(numHoja, '#cbCentral')).empty();
            var listaCentral = getVariableEvt(numHoja).ListaEquipo;
            if (listaCentral != null && listaCentral.length >= 1) {
                if (listaCentral.length != 1) {
                    $(getIdElemento(numHoja, '#cbCentral')).append('<option value="0" selected="selected"> -TODOS- </option>');
                }
                for (var i = 0; i < listaCentral.length; i++) {
                    $(getIdElemento(numHoja, '#cbCentral')).append('<option value=' + listaCentral[i].Equicodi + '>' + listaCentral[i].Equinomb + '</option>');
                }
            }
        }

        if (configuracionHoja.tieneFiltroArea) {
            //generar el filtro combo de areas
            $(getIdElemento(numHoja, '#cbArea')).empty();
            var listaArea = getVariableEvt(numHoja).ListaAreaOperativa;
            if (listaArea != null && listaArea.length >= 1) {
                if (listaArea.length != 1) {
                    $(getIdElemento(numHoja, '#cbArea')).append('<option value="0" selected="selected"> -TODOS- </option>');
                }
                for (var i = 0; i < listaArea.length; i++) {
                    if (listaArea[i] != null) {
                        $(getIdElemento(numHoja, '#cbArea')).append('<option value="' + listaArea[i] + '">' + listaArea[i] + '</option>');
                    }
                }
            }
        }

        if (configuracionHoja.tieneFiltroSubestacion) {
            //generar el filtro combo de Subestacion
            $(getIdElemento(numHoja, '#cbSubestacion')).empty();
            var listaSubestacion = getVariableEvt(numHoja).ListaSubestacion;
            if (listaSubestacion != null && listaSubestacion.length >= 1) {
                if (listaSubestacion.length != 1) {
                    $(getIdElemento(numHoja, '#cbSubestacion')).append('<option value="0" selected="selected"> -TODOS- </option>');
                }
                for (var i = 0; i < listaSubestacion.length; i++) {
                    $(getIdElemento(numHoja, '#cbSubestacion')).append('<option value=' + listaSubestacion[i].Areacodi + '>' + listaSubestacion[i].Areanomb + '</option>');
                }
            }
        }

        if (configuracionHoja.tieneFiltroFamilia) {
            //generar el filtro combo de Tipo de Equipo
            $(getIdElemento(numHoja, '#cbFamilia')).empty();
            var listaFamilia = getVariableEvt(numHoja).ListaFamilia;
            if (listaFamilia != null && listaFamilia.length >= 1) {
                if (listaFamilia.length != 1) {
                    $(getIdElemento(numHoja, '#cbFamilia')).append('<option value="0" selected="selected"> -TODOS- </option>');
                }
                for (var i = 0; i < listaFamilia.length; i++) {
                    $(getIdElemento(numHoja, '#cbFamilia')).append('<option value=' + listaFamilia[i].Famcodi + '>' + listaFamilia[i].Famabrev + '</option>');
                }
            }
        }
    }

    if (getTieneGrafico(numHoja)) {
        generaFiltroGrafico(numHoja);
    }
}