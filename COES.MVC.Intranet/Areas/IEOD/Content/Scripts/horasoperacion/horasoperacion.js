// scrips relacionados => "globales.js"
var controlador = siteRoot + 'IEOD/HorasOperacion/';

//////////////////////////////////////////////////////////////////////////////////////////////////
/// Funciones para cargar interfaz de Horas de Operación
//////////////////////////////////////////////////////////////////////////////////////////////////

function agregarListaDivFormulario(listaObj, numeroDivOrigen) {
    numeroDivOrigen = parseInt(numeroDivOrigen) || 0;

    OPCION_DIV = NUEVO_DIV_AGREGAR;

    // Busca todos los divs con id que comienza con "detalle" dentro del contenedor padre
    var divsConIdDetalle = $('.divEdicionMasiva').find('div[id^="detalle"]');

    // Inicializa una variable para rastrear el número más alto
    var numeroMasAlto = 0;

    // Recorre todos los divs encontrados y encuentra el número más alto
    divsConIdDetalle.each(function () {
        var id = $(this).attr('id');
        var numero = parseInt(id.replace('detalle', ''), 10);
        if (!isNaN(numero) && numero > numeroMasAlto) {
            numeroMasAlto = numero;
        }
    });

    // Calcula el nuevo número
    var numero = numeroMasAlto + 1;

    //agregar formularios adicionales
    for (var i = 0; i < listaObj.length; i++) {
        var obj = listaObj[i];
        obj.Identificador = numero;

        // Crea un nuevo div con el nuevo id (div sin html)
        var nuevoDiv = $(`<div class="vistaDetalle table-list tabla-container" id="detalle${numero}" style="width: auto; display: inline-block;"></div>`);

        // Agrega el nuevo div al final del contenedor
        if (numeroMasAlto == 0) {
            $('.divEdicionMasiva').html(nuevoDiv);
        } else {
            if (numeroDivOrigen > 0)
                $('#detalle' + numeroDivOrigen).after(nuevoDiv);
            else
                $('#detalle' + (numero - 1)).after(nuevoDiv);
        }

        //agregar html al div
        view_FormularioHO(obj, numero);

        //pintamos de color a los nuevos
        if (APP_OPCION == OPCION_NUEVO || obj.Hopcodi <= 0) {
            $('#detalle' + numero + ' #tabla-det-cabecera').css({
                'background-color': '#3d8ab8',
                'border-inline': 'solid 1px #fff'
            });
        }

        var titulo = "";
        var fontcolor = "";
        switch (obj.TipoVentana) {
            case TIPO_VENTANA_YUPANA:
                $('#detalle' + numero + ' #tabla-det-cabecera').css({
                    'background-color': '#ffff00', //amarillo
                    'border-inline': 'solid 1px #fff'
                });
                titulo = "Horas Programadas";
                break;
            case TIPO_VENTANA_EMS:
                $('#detalle' + numero + ' #tabla-det-cabecera').css({
                    'background-color': '#ed2027', //amarillo
                    'border-inline': 'solid 1px #fff'
                });
                titulo = "Horas EMS";
                fontcolor = "#FFFFFF";
                break;
            case TIPO_VENTANA_SCADA:
                $('#detalle' + numero + ' #tabla-det-cabecera').css({
                    'background-color': '#fdc206', //amarillo
                    'border-inline': 'solid 1px #fff'
                });
                titulo = "Horas Scada";
                fontcolor = "#000000";
                break;
        }

        $('#detalle' + numero + ' #tabla-det-cabecera #tabla-det-titulo').text(titulo);
        if (fontcolor != "")
            $('#detalle' + numero + ' #tabla-det-cabecera').css("color", fontcolor);

        numero++;
        numeroMasAlto++;
    }

    //activar evento global sobre todos los formularios
    ui_visibleYaccionBotonForm();

    if (APP_OPCION == OPCION_VER) {
        //hacer no editable
        var divs = document.getElementsByClassName('divEdicionMasiva');
        for (var j = 0; j < divs.length; j++) {
            var elementos = divs[j].querySelectorAll('input, textarea, button, select, a');
            elementos.forEach(function (elemento) {
                elemento.disabled = true;
                elemento.style.pointerEvents = 'none';
                elemento.style.opacity = 0.5; // Puedes ajustar el valor de opacidad como desees
            });
        }
    }

    if (APP_OPCION == OPCION_EDITAR) {

        const divsDetalle = document.querySelectorAll("[id^='detalle']");

        // Almacena los innerHTMLs iniciales de los divs en el objeto
        divsDetalle.forEach(div => {
            var visibleDiv = $(div).css('display');

            //solo considerar formularios visible
            if (visibleDiv != 'none') {
                const numero = div.id.replace('detalle', ''); // obtiene el número del ID
                var valor = ho_getObjFormularioFromHtml(numero);
                divsInitialState["detalle" + numero] = valor;
            }
        });
    }
}

// Función que genera la vista para el ingreso de datos para crear o modificar un intervalo de horas de operación
function view_FormularioHO(obj, numero) {
    var htmlResult = view_generarHtmlJson(obj, numero);

    // Inserta el HTML en detalle1
    $("#detalle" + numero).html(htmlResult);

    // Llama a postRenderhtml después de insertar el HTML
    view_HtmlInterfaz(obj, numero);

    if (numero > 1) {
        $('#detalle' + numero + ' .tbform-label').hide();
    }

    $('#detalle' + numero + ' #btnAgregarHoraOperacionEms2').click(function () {

        var objNuevo = _objInicializarBotonAgregar();

        //copiar datos del formulario seleccionado
        objNuevo.IdEmpresa = parseInt($('#detalle' + numero + ' #cbEmpresa2').val()) || 0;
        objNuevo.IdCentralSelect = parseInt($('#detalle' + numero + ' #cbCentral2').val()) || 0;
        objNuevo.IdGrupoModo = parseInt($('#detalle' + numero + ' #cbModoOpGrupo').val()) || 0;
        objNuevo.HoraIni = $('#detalle' + numero + ' #txtFueraParaleloH').val();

        var objModo = getModoFromListaModo(GLOBAL_HO.ListaModosOperacion, parseInt(objNuevo.IdGrupoModo));
        if (objModo != null) objNuevo.FlagModoEspecial = objModo.FlagModoEspecial;

        var listaObj = [];
        listaObj.push(objNuevo);

        //generar nuevo div
        agregarListaDivFormulario(listaObj, numero);
    });

    $('#detalle' + numero + ' #btnEliminarHoraOperacionEms2').click(function () {
        // Obtén todos los elementos div
        const divs = document.querySelectorAll("div");

        // Inicializa una variable contador
        var contador = 0;

        // Itera sobre los elementos div y verifica las condiciones
        divs.forEach((div) => {
            if (
                (div.id.startsWith("detalle") && div.style.display !== "none") ||
                (div.id.startsWith("detalle") && !div.style.display)
            ) {
                contador++;
            }
        });

        if (contador == 1) {
            alert("No se pueden eliminar todas las horas");
            return;
        }

        if (confirm("¿Desea eliminar hora de operación?")) {

            var label_activo = true;

            if ($('#detalle' + numero + ' .tbform-label:first').is(":hidden")) {
                // El primer .tbform-label está oculto
                label_activo = false;
            }

            if ($('#detalle' + numero + ' #hfHopCodi').val() == 0) {
                $('#detalle' + numero).hide();
                if (label_activo) {

                    var siguienteDivNoOculto = $('.divEdicionMasiva > div:visible:first');

                    if (siguienteDivNoOculto.length > 0) {
                        // Obtener el ID del div
                        var idDiv = siguienteDivNoOculto.attr('id');

                        // Extraer el número del ID
                        var match = idDiv.match(/\d+/);

                        if (match) {
                            var numeroDelNombre = parseInt(match[0]);

                            // Ejecuta una acción en el div encontrado
                            // Puedes personalizar la acción según tus necesidades
                            $('#detalle' + numeroDelNombre + ' .tbform-label').show();
                        } else {
                            console.log("No se encontró un número en el ID del div.");
                        }
                    } else {
                        console.log("No se encontró un div no oculto dentro de .divEdicionMasiva.");
                    }
                }

                $('#detalle' + numero).remove();
            } else {

                var hopcodiEliminar = $('#detalle' + numero + ' #hfHopCodi').val();

                $('#detalle' + numero + ' #hfIdPos').val(-2);
                $('#detalle' + numero).hide();

                if (label_activo) {

                    var siguienteDivNoOculto = $('.divEdicionMasiva > div:visible:first');

                    if (siguienteDivNoOculto.length > 0) {
                        // Obtener el ID del div
                        var idDiv = siguienteDivNoOculto.attr('id');

                        // Extraer el número del ID
                        var match = idDiv.match(/\d+/);

                        if (match) {
                            var numeroDelNombre = parseInt(match[0]);

                            // Ejecuta una acción en el div encontrado
                            // Puedes personalizar la acción según tus necesidades
                            $('#detalle' + numeroDelNombre + ' .tbform-label').show();
                        } else {
                            console.log("No se encontró un número en el ID del div.");
                        }
                    } else {
                        console.log("No se encontró un div no oculto dentro de .divEdicionMasiva.");
                    }
                }
            }
        }
    });

    desg_formulario_Desglose(0, numero);

    const td = document.querySelector('td');
    const observer = new ResizeObserver(ajustarAlturaTablas);

    observer.observe(td);
}

function ui_visibleYaccionBotonForm() {
    $('#btnAceptar2').unbind();
    $('#btnCancelar2').unbind();

    $("#btnAceptar2").hide();
    $("#btnCancelar2").hide();
    $("#btnAceptar3").hide();
    $("#btnCancelar3").hide();

    $('.fila_val_costo_incremental').hide();
    $('.fila_val_intervenciones').hide();
    $("#tblValidacionOtroApp").hide();

    if (APP_OPCION == OPCION_NUEVO) {
        $("#btnAceptar2").show();
        $("#btnCancelar2").show();

        //botones globales
        $('#btnAceptar2').click(function () {
            $('#hfConfirmarValInterv').val(0);
            ho_ValidarFormulario();
        });
        $('#btnCancelar2').click(function () {
            $('#tab-container').easytabs('select', '#vistaListado');
            $(".divEdicionMasiva").html('');
        });

    }

    if (APP_OPCION == OPCION_EDITAR) {
        $('#btnAceptar2').show();
        $('#btnCancelar2').show();

        $('#btnAceptar2').click(function () {
            $('#hfConfirmarValInterv').val(0);
            ho_ValidarFormulario();
        });
        $('#btnCancelar2').click(function () {
            var idModoGrupo = parseInt($('#detalle1 #cbModoOpGrupo').val()) || 0;
            editarHOP(idModoGrupo);
        });
    }
}

function view_HtmlInterfaz(obj, numero) {

    view_ConfInicialRegistro(numero);

    desg_actualizarTablaDesglose(numero, obj.ListaDesglose);
}

function view_ConfInicialRegistro(numero) {
    var checkboxIDs = [
        "chkFueraServicio",
        "chkCompensacionOrdArrq",
        "chkCompensacionOrdPar",
        "chkArranqueBlackStart",
        "chkEnsayoPotenciaEfectiva",
        "chkEnsayoPotenciaMinima",
        "chkSistemaAislado",
        "chkLimTransm"
    ];

    var checkboxValues = [
        "OPFueraServ",
        "OpCompOrdArranq",
        "OpCompOrdParad",
        "OpArranqBlackStart",
        "OpOpEnsayope",
        "OpOpEnsayopmin",
        "OpSistAislado",
        "OpOpLimTransm"
    ];
    var detalle = $('#detalle' + numero);
    // Suponiendo que tienes una variable "numero" definida
    for (var i = 0; i < checkboxIDs.length; i++) {
        var checkboxID = checkboxIDs[i];
        var checkboxValue = checkboxValues[i];
        var checkboxElement = detalle.find('#' + checkboxID);
        if (checkboxElement.length) {
            var newValue = parseInt($('#detalle' + numero + ' #hf' + checkboxValue).val());
            checkboxElement.prop('checked', newValue);
        }
    }

    ui_setInputmaskHora('#detalle' + numero + ' #txtOrdenArranqueH');
    ui_setInputmaskHora('#detalle' + numero + ' #txtOrdenParadaH');
    ui_setInputmaskHora('#detalle' + numero + ' #txtFueraParaleloH');
    ui_setInputmaskHora('#detalle' + numero + ' #txtEnParaleloH');

    ui_setInputmaskHora('#detalle' + numero + ' #desg_form_txtIniH');
    ui_setInputmaskHora('#detalle' + numero + ' #desg_form_txtFinH');

    $('#detalle' + numero + ' ' + OBJ_REFERENCIA.id_ref_txtOrdenArranqueH).on('focusout', function (e) { actualizarFechasUIHoraOperacion(TIPO_HO_MODO, numero); $(this).val(obtenerHoraValida($(this).val())); });
    $('#detalle' + numero + ' ' + OBJ_REFERENCIA.id_ref_txtEnParaleloH).on('focusout', function (e) { actualizarFechasUIHoraOperacion(TIPO_HO_MODO, numero); $(this).val(obtenerHoraValida($(this).val())); });
    $('#detalle' + numero + ' ' + OBJ_REFERENCIA.id_ref_txtOrdenParadaH).on('focusout', function (e) { actualizarFechasUIHoraOperacion(TIPO_HO_MODO, numero); $(this).val(obtenerHoraValida($(this).val())); });
    $('#detalle' + numero + ' ' + OBJ_REFERENCIA.id_ref_txtFueraParaleloH).on('focusout', function (e) {
        actualizartxtFueraParaleloF('#detalle' + numero + ' #txtFueraParaleloH', '#detalle' + numero + ' #txtFueraParaleloF', numero); actualizarFechasUIHoraOperacion(TIPO_HO_MODO, numero);
        $(this).val(obtenerHoraValida($(this).val()));
    });

    $('#detalle' + numero + ' #desg_form_txtIniH').on('focusout', function (e) { $(this).val(obtenerHoraValida($(this).val())); });
    $('#detalle' + numero + ' #desg_form_txtFinH').on('focusout', function (e) { $(this).val(obtenerHoraValida($(this).val())); });

    activarDesactivarCampos($('#detalle' + numero + ' #chkFueraServicio').is(':checked'), numero);

    $('#detalle' + numero + ' #cbCentral2').val($('#detalle' + numero + ' #hfCentral').val());
    $('#detalle' + numero + ' #cbTipoOp').val($('#detalle' + numero + ' #hfTipoOerac').val());
    $('#detalle' + numero + ' #cbMotOpForzada').val($('#detalle' + numero + ' #hfMotOpForzada').val());

    $('#detalle' + numero + ' #cbTipoOp').click(function () {
        seteaCombosHijosunidades($(this).val());
    });
    $('#detalle' + numero + ' #cbTipoOp').change(function () {
        //por pruebas
        $('#detalle' + numero + ' #chkEnsayoPotenciaEfectiva').removeAttr("disabled");
        $('#detalle' + numero + ' #chkEnsayoPotenciaMinima').removeAttr("disabled");
        $('#detalle' + numero + ' #chkEnsayoPotenciaEfectiva, #detalle' + numero + ' #chkEnsayoPotenciaMinima').prop('checked', false);
        mostrarCheckEnsayoPotenciaEfectiva(numero);

        if ($(this).val() == "114") /*Pruebas Aleatorias*/ {
            $('#detalle' + numero + ' #TxtDescripcion').val(PLANTILLA_DESC_PRUEBAS_ALEATORIAS);
            $('#detalle' + numero + " #leyendaAlertaProgramada").show();
        }
        else {
            $('#detalle' + numero + " #leyendaAlertaProgramada").hide();
        }


    });

    $('#detalle' + numero + ' #cbEmpresa2').change(function (event) {
        SELECCION_POR_MODO = (event.originalEvent === undefined);

        view_EnabledOrDisabledUIHoraOperacion(numero);
        cargarCentral(numero);

        cargarListaModo_Grupo(numero);
    });

    $('#detalle' + numero + ' #cbCentral2').change(function () {
        cargarListaModo_Grupo(numero);
        view_EnabledOrDisabledUIHoraOperacion(numero);
    });

    cargarListaModo_Grupo(numero);

    $('#detalle' + numero + ' .tr-linea-transmision').hide();

    $('#detalle' + numero + ' #btnAbrirBitacora').click(function () {
        visualizarBitacora($('#detalle' + numero + ' #hfBitacoraIdEvento').val(), $('#detalle' + numero + ' #hfBitacoraHayCambios').val(), numero);
    });

    $('#detalle' + numero + ' #chkArranqueBlackStart').change(function () {
        incluirDescripcionBlackStart(numero);
    });

    $('#detalle' + numero + ' #btnAgregarDesglose').click(function () {
        desg_formulario_Desglose(0, numero);
    });

    $('#detalle' + numero + ' #chkEnsayoPotenciaEfectiva').click(function () {
        if ($('#detalle' + numero + ' #chkEnsayoPotenciaEfectiva').prop('checked'))
            $('#detalle' + numero + ' #chkEnsayoPotenciaMinima').prop('disabled', 'disabled');
        else $('#detalle' + numero + ' #chkEnsayoPotenciaMinima').removeAttr("disabled");
    });

    $('#detalle' + numero + ' #chkEnsayoPotenciaMinima').click(function () {
        if ($('#detalle' + numero + ' #chkEnsayoPotenciaEfectiva').prop('checked'))
            $('#detalle' + numero + ' #chkEnsayoPotenciaEfectiva').prop('disabled', 'disabled');
        else $('#detalle' + numero + ' #chkEnsayoPotenciaEfectiva').removeAttr("disabled");
    });

};

function _objInicializarBotonAgregar() {
    var fechaValida = getFechaEms();

    var objetoNuevo = JSON.parse(JSON.stringify(OBJ_DATA_HORA_OPERACION));

    objetoNuevo.FechaIni = fechaValida;
    objetoNuevo.HoraIni = "00:00:00";
    objetoNuevo.FechaFin = fechaValida;
    objetoNuevo.HoraFin = "23:58:00";
    objetoNuevo.Fechahorordarranq = fechaValida;
    objetoNuevo.Hophorordarranq = '';
    objetoNuevo.FechaHophorparada = fechaValida;
    objetoNuevo.Hophorparada = '';

    objetoNuevo.ListaEmpresas = GLOBAL_HO.ListaEmpresas;
    objetoNuevo.ListaCentrales = GLOBAL_HO.ListaCentrales;
    objetoNuevo.ListaTipoOperacion = GLOBAL_HO.ListaTipoOperacion;
    objetoNuevo.ListaFechaArranque = GLOBAL_HO.ListaFechaArranque;
    objetoNuevo.ListaMotOpForzada = GLOBAL_HO.ListaMotOpForzada;
    objetoNuevo.FechaSiguiente = GLOBAL_HO.FechaSiguiente;

    objetoNuevo.Fecha = GLOBAL_HO.Fecha;
    objetoNuevo.FechaAnterior = GLOBAL_HO.FechaAnterior;
    objetoNuevo.FechaSiguiente = GLOBAL_HO.FechaSiguiente;

    objetoNuevo.EtiquetaFiltro = "Modo";

    return objetoNuevo;
}

function _objInicializarBotonEditar(objHora) {
    //clonar objeto
    var objForm = JSON.parse(JSON.stringify(OBJ_DATA_HORA_OPERACION));

    objForm.IdPos = objHora.IdPos;
    objForm.Editar = objHora.Editar;

    objForm.EtiquetaFiltro = "Modo";
    //
    objForm.FechaIni = moment(objHora.Hophorini).format('DD/MM/YYYY');
    objForm.FechaFin = moment(objHora.Hophorfin).format('DD/MM/YYYY');

    objForm.HoraIni = moment(objHora.Hophorini).format('HH:mm:ss');
    objForm.HoraFin = moment(objHora.Hophorfin).format('HH:mm:ss');
    objForm.HopPruebaExitosa = objHora.HopPruebaExitosa;

    objForm.Fechahorordarranq = objForm.FechaIni;
    if (objHora.Hophorordarranq != null && objHora.Hophorordarranq != "") {
        objForm.Fechahorordarranq = moment(objHora.Hophorordarranq).format('DD/MM/YYYY');
        objForm.Hophorordarranq = moment(objHora.Hophorordarranq).format('HH:mm:ss');
    }

    objForm.FechaHophorparada = objForm.FechaIni;
    if (objHora.Hophorparada != null && objHora.Hophorparada != "") {
        objForm.FechaHophorparada = moment(objHora.Hophorparada).format('DD/MM/YYYY');
        objForm.Hophorparada = moment(objHora.Hophorparada).format('HH:mm:ss');
    }

    //
    objForm.IdEmpresa = objHora.Emprcodi;
    objForm.IdCentralSelect = objHora.Equipadre;
    objForm.IdGrupoModo = objHora.Grupocodi;
    objForm.IdTipoOperSelect = objHora.Subcausacodi;

    objForm.Hopdesc = objHora.Hopdesc; //OBS del administrador
    objForm.Hopobs = objHora.Hopobs; //OBS del agente

    if (objHora.Hopfalla == "F") {
        objForm.OpFueraServ = 1;
    }

    if (objHora.Hopcompordarrq == "S") {
        objForm.OpCompOrdArranq = 1;
    }

    if (objHora.Hopcompordpard == "S") {
        objForm.OpCompOrdParad = 1;
    }

    if (objHora.Hopsaislado == 1) {
        objForm.OpSistAislado = 1;
    }

    if (objHora.Hoplimtrans == 'S') {
        objForm.OpLimTransm = 1;
    }

    if (objHora.Hoparrqblackstart == 'S') {
        objForm.OpArranqBlackStart = 1;
    }

    if (objHora.Hopensayope == 'S') {
        objForm.OpEnsayope = 1;
    }

    if (objHora.Hopensayopmin == 'S') {
        objForm.OpEnsayopmin = 1;
    }

    objForm.UsuarioModificacion = objHora.Lastuser;
    objForm.FechaModificacion = objHora.LastdateDesc;
    objForm.IdMotOpForzadaSelect = objHora.Hopcausacodi;

    APP_OPCION = APP_OPCION != OPCION_EDITAR ? APP_OPCION : OPCION_EDITAR;
    //bitacora
    objForm.BitacoraHophoriniFecha = objHora.BitacoraHophoriniFecha == null ? "" : objHora.BitacoraHophoriniFecha;
    objForm.BitacoraHophoriniHora = objHora.BitacoraHophoriniHora == null ? "" : objHora.BitacoraHophoriniHora;
    objForm.BitacoraHophorfinFecha = objHora.BitacoraHophorfinFecha == null ? "" : objHora.BitacoraHophorfinFecha;
    objForm.BitacoraHophorfinHora = objHora.BitacoraHophorfinHora == null ? "" : objHora.BitacoraHophorfinHora;
    objForm.BitacoraDescripcion = objHora.BitacoraDescripcion == null ? "" : objHora.BitacoraDescripcion;
    objForm.BitacoraComentario = objHora.BitacoraComentario == null ? "" : objHora.BitacoraComentario;
    objForm.BitacoraDetalle = objHora.BitacoraDetalle == null ? "" : objHora.BitacoraDetalle;
    objForm.BitacoraIdSubCausaEvento = objHora.BitacoraIdSubCausaEvento == null ? "" : objHora.BitacoraIdSubCausaEvento;
    objForm.BitacoraIdEvento = objHora.Evencodi == null ? "" : objHora.Evencodi;
    objForm.BitacoraIdTipoEvento = objHora.BitacoraIdTipoEvento == null ? "" : objHora.BitacoraIdTipoEvento;
    objForm.BitacoraIdEquipo = objHora.BitacoraIdEquipo == null ? "" : objHora.BitacoraIdEquipo;
    objForm.BitacoraIdEmpresa = objHora.BitacoraIdEmpresa == null ? "" : objHora.BitacoraIdEmpresa;
    objForm.BitacoraIdTipoOperacion = objHora.BitacoraIdTipoOperacion == null ? "" : objHora.BitacoraIdTipoOperacion;

    //Desglose
    var listaDesglose = objHora.ListaDesglose;
    var listaFinal = [];
    if (listaDesglose !== undefined && listaDesglose != null) {
        listaDesglose.forEach(function (objDesg) {
            objDesg = formatObjDesglose(objDesg);
            listaFinal.push(objDesg);
        });
    }
    objForm.ListaDesglose = listaFinal;

    // Supongamos que objForm es un objeto en JavaScript
    // y fecha es una variable definida anteriormente

    objForm.Fecha = GLOBAL_HO.Fecha;
    objForm.FechaAnterior = GLOBAL_HO.FechaAnterior;
    objForm.FechaSiguiente = GLOBAL_HO.FechaSiguiente;
    objForm.ListaEmpresas = GLOBAL_HO.ListaEmpresas == null ? [] : GLOBAL_HO.ListaEmpresas.filter(function (elemento) {
        return elemento.Emprcodi === objHora.Emprcodi;
    });

    objForm.ListaCentrales = GLOBAL_HO.ListaCentrales == null ? [] : GLOBAL_HO.ListaCentrales.filter(function (elemento) {
        return elemento.Equicodi === objHora.Equipadre;
    });

    objForm.ListaTipoOperacion = GLOBAL_HO.ListaTipoOperacion == null ? [] : GLOBAL_HO.ListaTipoOperacion;

    objForm.ListaFechaArranque = GLOBAL_HO.ListaFechaArranque == null ? [] : GLOBAL_HO.ListaFechaArranque;
    objForm.ListaMotOpForzada = GLOBAL_HO.ListaMotOpForzada == null ? [] : GLOBAL_HO.ListaMotOpForzada;
    objForm.ValorAlertaEmsSI = GLOBAL_HO.ValorAlertaEmsSI;
    objForm.EtiquetaFiltro = GLOBAL_HO.EtiquetaFiltro == null ? "" : GLOBAL_HO.EtiquetaFiltro;
    objForm.FlagModoEspecial = objHora.FlagModoEspecial == null ? "" : objHora.FlagModoEspecial;

    objForm.MatrizunidadesExtra = objHora.MatrizunidadesExtra;
    if (objForm.MatrizunidadesExtra === undefined || objForm.MatrizunidadesExtra == null)
        objForm.MatrizunidadesExtra = [];

    objForm.Hopcodi = objHora.Hopcodi;

    return objForm;
}

function _objInicializarBotonEditarConvertir(grupocodi, horaInicio, horaFin) {
    var fechaValida = getFechaEms();
    var objForm = JSON.parse(JSON.stringify(OBJ_DATA_HORA_OPERACION));

    var modo = getModoFromListaModo(GLOBAL_HO.ListaModosOperacion, grupocodi);

    objForm.FechaIni = fechaValida;
    objForm.HoraIni = horaInicio;
    objForm.FechaFin = fechaValida;
    objForm.HoraFin = horaFin;
    objForm.Fechahorordarranq = fechaValida;
    objForm.Hophorordarranq = '';
    objForm.FechaHophorparada = fechaValida;
    objForm.Hophorparada = '';
    objForm.IdEmpresa = modo.Emprcodi;
    objForm.IdCentralSelect = modo.Equipadre;
    objForm.IdGrupoModo = modo.Grupocodi;
    objForm.ListaEmpresas = GLOBAL_HO.ListaEmpresas;
    objForm.ListaTipoOperacion = GLOBAL_HO.ListaTipoOperacion;
    objForm.ListaFechaArranque = GLOBAL_HO.ListaFechaArranque;
    objForm.ListaMotOpForzada = GLOBAL_HO.ListaMotOpForzada;

    objForm.Fecha = GLOBAL_HO.Fecha;
    objForm.FechaAnterior = GLOBAL_HO.FechaAnterior;
    objForm.FechaSiguiente = GLOBAL_HO.FechaSiguiente;

    objForm.ListaCentrales = GLOBAL_HO.ListaCentrales == null ? [] : GLOBAL_HO.ListaCentrales.filter(function (elemento) {
        return elemento.Equicodi === modo.Equipadre;
    });

    objForm.EtiquetaFiltro = "Modo";

    return objForm;
}

function view_generarHtmlJson(modeloJSON, numero) {
    var cbDisabled = APP_OPCION == OPCION_NUEVO ? "" : "disabled";

    var html_botonNuevo = '<a href="#" id="btnEliminarHoraOperacionEms2" style="float: right">' +
        '<div class="content-item-action">' +
        '<img src="' + siteRoot + 'Content/Images/btn-cancel.png" title="Eliminar Hora de Operación">' +
        '</div>' +
        '</a>';

    html_botonNuevo += '<a href="#" id="btnAgregarHoraOperacionEms2" style="float: right">' +
        '<div class="content-item-action">' +
        '<img src="' + siteRoot + 'Content/Images/btn-add.png" title="Agregar nueva Hora de Operación">' +
        '</div>' +
        '</a>';

    var html_total = '<div id="tabla-det-cabecera" style="height:30px;float:left;width:100%"><div style="padding-left:10px;padding-top:2px;float:left;"><h3 id="tabla-det-titulo" style="margin-top: 5px;"><h3></div>' + html_botonNuevo + '</div><div class="content-registro">';

    var anchoMin = numero == 1 ? "min-width: 580px;" : "";
    html_total += `<table class="tablitaCSS" style="max-width: 700px;${anchoMin};width:auto;">`;

    html_total += '<tr>';
    html_total += '<td class="tbform-label" style="width: 220px">Empresa</td>';
    html_total += '<td class="tbform-control">';

    html_total += '<select id="cbEmpresa2" name="cbEmpresa2" style="width:220px;" ' + cbDisabled + '>';
    html_total += '<option value="' + EMPRESA_DEFAULT_SELECT + '">--SELECCIONE--</option>';

    if (modeloJSON.ListaEmpresas.length > 0) {
        for (var i = 0; i < modeloJSON.ListaEmpresas.length; i++) {
            var selectedEmp = modeloJSON.ListaEmpresas[i].Emprcodi == modeloJSON.IdEmpresa ? " selected " : "";
            html_total += `<option value="${modeloJSON.ListaEmpresas[i].Emprcodi}" ${selectedEmp} >${modeloJSON.ListaEmpresas[i].Emprnomb}</option>`;
        }
    }

    html_total += '</select>';
    html_total += '</td>';
    html_total += '</tr>';

    html_total += '<tr>';
    html_total += '<td class="tbform-label">Central</td>';
    html_total += '<td class="tbform-control">';
    html_total += '<select id="cbCentral2" name="cbCentral2" style="width:220px;" ' + cbDisabled + '>';
    html_total += '<option value="' + ID_CENTRAL_DEFAULT_SELECT + '">--SELECCIONE--</option>';

    if (modeloJSON.ListaCentrales.length > 0) {
        for (var j = 0; j < modeloJSON.ListaCentrales.length; j++) {
            var selectedCent = modeloJSON.ListaCentrales[j].Equicodi == modeloJSON.IdCentralSelect ? " selected " : "";
            html_total += `<option value="${modeloJSON.ListaCentrales[j].Equicodi}" ${selectedCent} >${modeloJSON.ListaCentrales[j].Equinomb}</option>`;
        }
    }

    html_total += '</select>';
    html_total += '<input type="hidden" id="hfCentral" name="hfCentral" value="' + modeloJSON.IdCentralSelect + '" />';
    html_total += '<input type="hidden" id="hfIdModGrup" name="hfIdModGrup" value="' + modeloJSON.IdGrupoModo + '" />';
    html_total += '<input type="hidden" id="hfFlagCentralRsvFriaToRegistrarUnidad" name="hfFlagCentralRsvFriaToRegistrarUnidad" value="' + "" + '" />';
    html_total += '</td>';
    html_total += '</tr>';

    if (modeloJSON.EtiquetaFiltro == "") {
        modeloJSON.EtiquetaFiltro = "Modo";
    }

    html_total += '<tr class="tr-grupo-modo">';
    html_total += '<td class="tbform-label">' + modeloJSON.EtiquetaFiltro + '</td>';
    html_total += '<td class="tbform-control">';
    html_total += '<div id="listaModoGrupo">';
    html_total += '</div>';
    html_total += '</td>';
    html_total += '</tr>';

    html_total += '<tr class="tr-tipo-operacion">';
    html_total += '<td class="tbform-label">Calificación</td>';
    html_total += '<td class="tbform-control">';
    html_total += '<table style="margin:0px;">';
    html_total += '<tr>';
    html_total += '<td rowspan="2">';
    html_total += '<select id="cbTipoOp" name="cbTipoOp" style="width:220px;">';

    if (modeloJSON.ListaTipoOperacion.length > 0) {
        for (var k = 0; k < modeloJSON.ListaTipoOperacion.length; k++) {
            html_total += '<option value="' + modeloJSON.ListaTipoOperacion[k].Subcausacodi + '">' + modeloJSON.ListaTipoOperacion[k].Subcausadesc + '</option>';
        }
    }

    html_total += '</select>';
    html_total += '</td>';
    html_total += '<td><input type="checkbox" id="chkEnsayoPotenciaEfectiva" name="chkEnsayoPotenciaEfectiva" style="display: none" /><span id="txtEnsayoPotenciaEfectiva" style="display: inline-block;">Ensayo de Potencia Efectiva</span></td>';
    html_total += '</tr>';
    html_total += '<tr>';
    html_total += '<td><input type="checkbox" id="chkEnsayoPotenciaMinima" name="chkEnsayoPotenciaMinima" style="display: none" /><span id="txtEnsayoPotenciaMinima" style="display: inline-block;">Ensayo de Potencia Mínima</span></td>';
    html_total += '</tr>';
    html_total += '</table>';
    html_total += '<input type="hidden" id="hfTipoOerac" name="hfTipoOerac" value="' + modeloJSON.IdTipoOperSelect + '" />';
    html_total += '<div id="hdMensajeEnsayo" style="color:red;font-size:10px">(*) Solo se debe seleccionar un ensayo realizado </div>';
    html_total += '</td>';
    html_total += '</tr>';

    html_total += '<tr class="unidades_no_esp titulo" style="display: none"><td colspan="2"></td></tr>';
    html_total += '<tr class="unidades_no_esp" style="display: none">';
    html_total += '<td colspan="2" class="search-content">';
    html_total += '<div id="unidades_no_esp"></div>';
    html_total += '</td>';
    html_total += '</tr>';

    html_total += '<tr>';
    html_total += '<td class="tbform-label">Orden de Arranque</td>';
    html_total += '<td class="tbform-control">';
    html_total += '<select id="txtOrdenArranqueF" style="width:122px;" disabled>';
    for (var l = 0; l < modeloJSON.ListaFechaArranque.length; l++) {
        var selected = (modeloJSON.ListaFechaArranque[l] == modeloJSON.Fechahorordarranq) ? 'selected' : '';
        html_total += '<option value="' + modeloJSON.ListaFechaArranque[l] + '" ' + selected + '>' + modeloJSON.ListaFechaArranque[l] + '</option>';
    }

    html_total += '</select>';
    html_total += '<input type="text" id="txtOrdenArranqueH" style="width:100px;" value="' + modeloJSON.Hophorordarranq + '" autocomplete="off" />';
    html_total += '<input type="checkbox" id="chkCompensacionOrdArrq" name="chkFueraServicio" />Compensar';
    html_total += '</td>';
    html_total += '</tr>';

    html_total += '<tr>';
    html_total += '<td class="tbform-label">En Paralelo</td>';
    html_total += '<td class="tbform-control">';
    html_total += '<input type="text" id="txtEnParaleloF" style="width:120px;" value="' + modeloJSON.FechaIni + '" disabled />';
    html_total += '<input type="text" id="txtEnParaleloH" name="HoraIni" style="width:100px;" value="' + modeloJSON.HoraIni + '" autocomplete="off" />';
    html_total += '<input type="checkbox" id="chkArranqueBlackStart" name="chkArranqueBlackStart" style="display: none" /><span id="txtArranqueBlackStart">Arranque en Black Start</span>';
    html_total += '</td>';
    html_total += '</tr>';

    html_total += '<tr>';
    html_total += '<td class="tbform-label">Fuera de Servicio (F/S) por Falla</td>';
    html_total += '<td class="tbform-control">';
    html_total += '<input type="checkbox" id="chkFueraServicio" name="chkFueraServicio" onchange="activarDesactivarCampos(this.checked,' + modeloJSON.Identificador + ');" />';
    html_total += '<input type="button" id="btnAbrirBitacora" value="Agregar bitácora" style="display: none" />';
    html_total += '</td>';
    html_total += '</tr>';

    html_total += '<tr>';
    html_total += '<td class="tbform-label">Orden de Parada</td>';
    html_total += '<td class="tbform-control">';
    html_total += '<input type="text" id="txtOrdenParadaF" style="width:120px;" value="' + modeloJSON.FechaHophorparada + '" disabled />';
    html_total += '<input type="text" id="txtOrdenParadaH" name="TxtOrdenParadaH" style="width:100px;" value="' + modeloJSON.Hophorparada + '" autocomplete="off" />';
    html_total += '<input type="checkbox" id="chkCompensacionOrdPar" name="chkFueraServicio" />Compensar';
    html_total += '</td>';
    html_total += '</tr>';

    html_total += '<tr>';
    html_total += '<td class="tbform-label">Fuera de Paralelo</td>';
    html_total += '<td class="tbform-control">';
    html_total += '<input type="text" id="txtFueraParaleloF" style="width:120px;" value="' + modeloJSON.FechaFin + '" disabled />';
    html_total += '<input type="text" id="txtFueraParaleloH" name="TxtFueraParaleloH" style="width:100px;" value="' + modeloJSON.HoraFin + '" autocomplete="off" />';
    html_total += '</td>';
    html_total += '</tr>';

    html_total += '<tr class="unidades_modo titulo" style="display: none"><td colspan="2"></td></tr>';
    html_total += '<tr class="unidades_modo" style="display: none">';
    html_total += '<td colspan="2" class="search-content">';
    html_total += '<div id="unidades_especiales"></div>';
    html_total += '</td>';
    html_total += '</tr>';

    html_total += '<tr>';
    html_total += '<td class="tbform-label">Descripción</td>';
    html_total += '<td colspan="2" class="tbform-control">';
    html_total += '<div id="leyendaBlackStartCReservFria" style="display: none;font-style: italic;color: red;">* Indicar si el arranque fue con Black Start.</div>';
    html_total += '<div id="leyendaAlertaProgramada" style="display:none;font-style: italic;color: red;">* Formato para programar alerta:<br>A las HH:mm h, alcanzó su máxima generación.</div>';
    html_total += '<div id="leyendaGeneradoresCReservFria" style="display: none;font-style: italic;color: red;">* Indicar las unidades, generadores o grupos que operaron.</div>';
    html_total += '<textarea cols=50 rows=3 id="TxtDescripcion" name="TxtDescripcion" maxlength="600">' + (modeloJSON.Hopdesc !== null ? modeloJSON.Hopdesc : '') + '</textarea>';
    html_total += '</td>';
    html_total += '</tr>';

    html_total += '<tr>';
    html_total += '<td class="tbform-label">Observación del agente</td>';
    html_total += '<td colspan="2" class="tbform-control">' + (modeloJSON.Hopobs !== null ? modeloJSON.Hopobs : '') + '</td>';
    html_total += `<input type="hidden" id="txtObservacion" value="${modeloJSON.Hopobs !== null ? modeloJSON.Hopobs : ''}" />`;
    html_total += '</tr>';

    html_total += '<tr>';
    html_total += '<td class="tbform-label">Sistema Aislado</td>';
    html_total += '<td class="tbform-control">';
    html_total += '<input type="checkbox" id="chkSistemaAislado" name="chkSistemaAislado" />';
    html_total += '</td>';
    html_total += '</tr>';

    html_total += '<tr>';
    html_total += '<td class="tbform-label">Motivo Operación Forzada</td>';
    html_total += '<td class="tbform-control">';
    html_total += '<select id="cbMotOpForzada" name="cbMotOpForzada" style="width:220px;">';
    for (var m = 0; m < modeloJSON.ListaMotOpForzada.length; m++) {
        html_total += '<option value="' + modeloJSON.ListaMotOpForzada[m].Motcodi + '">' + modeloJSON.ListaMotOpForzada[m].Motdesc + '</option>';
    }
    html_total += '</select>';
    html_total += '</td>';
    html_total += '</tr>';

    html_total += '<tr style="">';
    html_total += '<td class="tbform-label">Límite de Transmisión</td>';
    html_total += '<td class="tbform-control">';
    html_total += '<input type="checkbox" id="chkLimTransm" name="chkLimTransm" />';
    html_total += '</td>';
    html_total += '</tr>';

    html_total += '<tr class="hop_auditoria">';
    html_total += '<td class="tbform-label">Usuario modificación </td>';
    html_total += '<td class="tbform-control">';
    html_total += modeloJSON.UsuarioModificacion;
    html_total += '</td>';
    html_total += '</tr>';

    html_total += '<tr class="hop_auditoria">';
    html_total += '<td class="tbform-label">Fecha modificación</td>';
    html_total += '<td class="tbform-control">';
    html_total += modeloJSON.FechaModificacion;
    html_total += '</td>';
    html_total += '</tr>';

    var checkPruebaExitosa = '';
    if (modeloJSON.HopPruebaExitosa == 1) checkPruebaExitosa = " checked ";

    html_total += '<tr>';
    html_total += '<td class="tbform-label">Prueba aleatoria PR25 Exitosa</td>';
    html_total += '<td class="tbform-control">';
    html_total += `<input type="checkbox" id="chkPruebaExitosa" name="chkPruebaExitosa" ${checkPruebaExitosa} />`;
    html_total += '</td>';
    html_total += '</tr>';

    html_total += '<!-- Desglose -->';
    html_total += '<tr>';
    html_total += '<td class="tbform-label">Desglose</td>';
    html_total += '<td class="tbform-control" style="padding-right: 10px;vertical-align: top !important">';

    var nuevoDesglose = `
        <form id="form_desglose_${numero}">
        <div id="desc_ho_rangos_permitido_desglose_${numero}" style="display:none;"></div>
        <table id="tablaAgregarDesglose" class="pretty-nuevo tabla-icono tabla_ho_desg" style='width; auto; max-width: 460px;'>
        <thead>
            <tr>
                <th>Nuevo registro</td>
                <th>Hora Inicio</td>
                <th>Hora Fin</td>
                <th>Valor</td>
                <th></td>
            </tr>
        </thead>
        <tbody>
            <tr>
                <td align="center">
                    <select id="desg_form_cbTipo" class="dd_tipo_desglose">`;

    $.each(GLOBAL_HO.ListaTipoDesglose, function (index, value) {
        nuevoDesglose += '<option value="' + GLOBAL_HO.ListaTipoDesglose[index].TipoDesglose + '">' + GLOBAL_HO.ListaTipoDesglose[index].Subcausadesc + '</option>';
    });

    //valor defecto nuevo desglose
    var txtIniDesg = modeloJSON.HoraIni;
    var txtFinDesg = modeloJSON.HoraFin;
    if (modeloJSON.ListaDesglose !== undefined && modeloJSON.ListaDesglose != null && modeloJSON.ListaDesglose.length > 0) {
        txtIniDesg = modeloJSON.ListaDesglose[modeloJSON.ListaDesglose.length - 1].HoraFin;
        txtFinDesg = modeloJSON.HoraFin;
    }

    nuevoDesglose += `</select>
                </td>
                <td align="center">
                    <input type="text" id="desg_form_txtIniH" style="width:62px;" value="${txtIniDesg}" autocomplete="off" />
                </td>
                <td align="center">
                    <input type="text" id="desg_form_txtFinH" style="width:62px;" value="${txtFinDesg}" autocomplete="off" />
                </td>
                <td align="center">
                    <input type="text" id="desg_form_valor" style="width:46px; text-align: center;" autocomplete="off" />
                </td>
                <td align="center">
                        <input type="hidden" id="desg_form_accion" value="3" />
                        <input type="hidden" id="desg_form_fila" value="0" />
                        <a class="btn_add_hop" href="javascript:desg_guardarDesglose(${numero});">
                        <img src="${siteRoot}Content/Images/btn-add.png">
                        </a>
                </td>
            </tr>
            </tbody>
        </table>
        </form>`;

    html_total += nuevoDesglose;

    html_total += '<div id="tablaDesglose"></div>';

    html_total += '</td>';
    html_total += '</tr>';

    html_total += '</table>';

    html_total += '</div>';

    html_total += '<input type="hidden" id="hfHopCodi" name="hfHopCodi" value="' + modeloJSON.Hopcodi + '" />';
    html_total += '<input type="hidden" id="hfIdPos" name="hfIdPos" value="' + modeloJSON.IdPos + '" />';
    html_total += '<input type="hidden" id="hfFlagModoEspecial" name="hfFlagModoEspecial" value="' + modeloJSON.FlagModoEspecial + '" />';
    html_total += '<input type="hidden" id="hfCentral" name="hfCentral" value="' + modeloJSON.IdCentralSelect + '" />';
    html_total += '<input type="hidden" id="hfIdModoGrupo" name="hfIdModoGrupo" value="' + modeloJSON.IdGrupoModo + '" />';
    html_total += '<input type="hidden" id="hfOPFueraServ" name="hfOPFueraServ" value="' + modeloJSON.OpFueraServ + '" />';
    html_total += '<input type="hidden" id="hfOpCompOrdArranq" name="hfOpCompOrdArranq" value="' + modeloJSON.OpCompOrdArranq + '" />';
    html_total += '<input type="hidden" id="hfOpCompOrdParad" name="hfOpCompOrdParad" value="' + modeloJSON.OpCompOrdParad + '" />';
    html_total += '<input type="hidden" id="hfOpArranqBlackStart" name="hfOpArranqBlackStart" value="' + modeloJSON.OpArranqBlackStart + '" />';
    html_total += '<input type="hidden" id="hfOpOpEnsayope" name="hfOpOpEnsayope" value="' + modeloJSON.OpEnsayope + '" />';
    html_total += '<input type="hidden" id="hfOpOpEnsayopmin" name="hfOpOpEnsayopmin" value="' + modeloJSON.OpEnsayopmin + '" />';
    html_total += '<input type="hidden" id="hfOpSistAislado" name="hfOpSistAislado" value="' + modeloJSON.OpSistAislado + '" />';
    html_total += '<input type="hidden" id="hfOpOpLimTransm" name="hfOpOpLimTransm" value="' + modeloJSON.OpLimTransm + '" />';
    html_total += '<input type="hidden" id="hfFechaAnterior" name="hfFechaAnterior" value="' + modeloJSON.FechaAnterior + '" />';
    html_total += '<input type="hidden" id="hfFecha" name="hfFecha" value="' + modeloJSON.Fecha + '" />';
    html_total += '<input type="hidden" id="hfFechaFin" name="hfFecha" value="' + modeloJSON.FechaFin + '" />';
    html_total += '<input type="hidden" id="hfFechaSiguiente" name="hfFechaSiguiente" value="' + modeloJSON.FechaSiguiente + '" />';
    html_total += '<input type="hidden" id="hfMotOpForzada" name="hfMotOpForzada" value="' + modeloJSON.IdMotOpForzadaSelect + '" />';
    html_total += '<input type="hidden" id="hfFlagCentralRsvFriaToRegistrarUnidad" name="hfFlagCentralRsvFriaToRegistrarUnidad" value="' + modeloJSON.FlagCentralRsvFriaToRegistrarUnidad + '" />';
    html_total += '<input type="hidden" id="hfBitacoraHayCambios" name="hfBitacoraHayCambios" value="0" />';
    html_total += '<input type="hidden" id="hfBitacoraHophoriniFecha" name="hfBitacoraHophoriniFecha" value="' + modeloJSON.BitacoraHophoriniFecha + '" />';
    html_total += '<input type="hidden" id="hfBitacoraHophoriniHora" name="hfBitacoraHophoriniHora" value="' + modeloJSON.BitacoraHophoriniHora + '" />';
    html_total += '<input type="hidden" id="hfBitacoraHophorfinFecha" name="hfBitacoraHophorfinFecha" value="' + modeloJSON.BitacoraHophorfinFecha + '" />';
    html_total += '<input type="hidden" id="hfBitacoraHophorfinHora" name="hfBitacoraHophorfinHora" value="' + modeloJSON.BitacoraHophorfinHora + '" />';
    html_total += '<input type="hidden" id="hfBitacoraDescripcion" name="hfBitacoraDescripcion" value="' + modeloJSON.BitacoraDescripcion + '" />';
    html_total += '<input type="hidden" id="hfBitacoraComentario" name="hfBitacoraComentario" value="' + modeloJSON.BitacoraComentario + '" />';
    html_total += '<input type="hidden" id="hfBitacoraDetalle" name="hfBitacoraDetalle" value="' + modeloJSON.BitacoraDetalle + '" />';
    html_total += '<input type="hidden" id="hfBitacoraIdSubCausaEvento" name="hfBitacoraIdSubCausaEvento" value="' + modeloJSON.BitacoraIdSubCausaEvento + '" />';
    html_total += '<input type="hidden" id="hfBitacoraIdEvento" name="hfBitacoraIdEvento" value="' + modeloJSON.BitacoraIdEvento + '" />';
    html_total += '<input type="hidden" id="hfBitacoraIdTipoEvento" name="hfBitacoraIdTipoEvento" value="' + modeloJSON.BitacoraIdTipoEvento + '" />';
    html_total += '<input type="hidden" id="hfBitacoraIdEquipo" name="hfBitacoraIdEquipo" value="' + modeloJSON.BitacoraIdEquipo + '" />';
    html_total += '<input type="hidden" id="hfBitacoraIdEmpresa" name="hfBitacoraIdEmpresa" value="' + modeloJSON.BitacoraIdEmpresa + '" />';
    html_total += '<input type="hidden" id="hfHopPruebaExitosa" name="hfHopPruebaExitosa" value="' + modeloJSON.HopPruebaExitosa + '" />';
    html_total += '<input type="hidden" id="hfBitacoraIdTipoOperacion" name="hfBitacoraIdTipoOperacion" value="' + modeloJSON.BitacoraIdTipoOperacion + '" />';

    return html_total;
}

///////////////////////////////////////////////////////////////////////////////////////////////////
/// Eventos .js de edición en Formulario
///////////////////////////////////////////////////////////////////////////////////////////////////

function view_EnabledOrDisabledUIHoraOperacion(numero) {

    var idModoGrupo = parseInt($('#detalle' + numero + ' #cbModoOpGrupo').val()) || 0;

    if (APP_OPCION != OPCION_VER) {
        if (idModoGrupo > 0) {

            if (glb_FlagModoEspecial(GLOBAL_HO, idModoGrupo) == FLAG_HO_ESPECIAL) {
                $('#detalle' + numero + ' #txtOrdenArranqueF').prop('disabled', 'disabled');
                $('#detalle' + numero + ' ' + OBJ_REFERENCIA.id_ref_txtOrdenArranqueH).prop('disabled', 'disabled');
                $('#detalle' + numero + ' ' + OBJ_REFERENCIA.id_ref_txtEnParaleloH).prop('disabled', 'disabled');
                $('#detalle' + numero + ' ' + OBJ_REFERENCIA.id_ref_txtOrdenParadaH).prop('disabled', 'disabled');
                $('#detalle' + numero + ' ' + OBJ_REFERENCIA.id_ref_txtFueraParaleloH).prop('disabled', 'disabled');
            } else {
                $('#detalle' + numero + ' #txtOrdenArranqueF').removeAttr("disabled");
                $('#detalle' + numero + ' ' + OBJ_REFERENCIA.id_ref_txtOrdenArranqueH).removeAttr("disabled");
                $('#detalle' + numero + ' ' + OBJ_REFERENCIA.id_ref_txtEnParaleloH).removeAttr("disabled");
                $('#detalle' + numero + ' ' + OBJ_REFERENCIA.id_ref_txtOrdenParadaH).removeAttr("disabled");
                $('#detalle' + numero + ' ' + OBJ_REFERENCIA.id_ref_txtFueraParaleloH).removeAttr("disabled");
            }
        } else {
            $('#detalle' + numero + ' #txtOrdenArranqueF').prop('disabled', 'disabled');
        }
        $('#detalle' + numero + ' #chkArranqueBlackStart').removeAttr('disabled');

        if ($('#detalle' + numero + ' #chkEnsayoPotenciaEfectiva').prop('checked')) $('#detalle' + numero + ' #chkEnsayoPotenciaMinima').prop('disabled', 'disabled');
        else $('#detalle' + numero + ' #chkEnsayoPotenciaMinima').removeAttr('disabled');

        if ($('#detalle' + numero + ' #chkEnsayoPotenciaMinima').prop('checked')) $('#detalle' + numero + ' #chkEnsayoPotenciaEfectiva').prop('disabled', 'disabled');
        else $('#detalle' + numero + ' #chkEnsayoPotenciaEfectiva').removeAttr('disabled');

    } else {
        $('#detalle' + numero + OBJ_REFERENCIA.id_ref_txtOrdenArranqueH).prop('disabled', 'disabled');
        $('#detalle' + numero + OBJ_REFERENCIA.id_ref_txtEnParaleloH).prop('disabled', 'disabled');
        $('#detalle' + numero + OBJ_REFERENCIA.id_ref_txtOrdenParadaH).prop('disabled', 'disabled');
        $('#detalle' + numero + OBJ_REFERENCIA.id_ref_txtFueraParaleloH).prop('disabled', 'disabled');
        $('#detalle' + numero + ' #chkArranqueBlackStart').prop('disabled', 'disabled');

        $('#detalle' + numero + ' #chkEnsayoPotenciaEfectiva').prop('disabled', 'disabled');
        $('#detalle' + numero + ' #chkEnsayoPotenciaMinima').prop('disabled', 'disabled');

        $('#detalle' + numero + ' #cbModoOpGrupo').prop('disabled', 'disabled');
    }

    //Ocultar UI para Centrales de Reserva Fría
    $('#detalle' + numero + ' #leyendaBlackStartCReservFria').hide();
    $('#detalle' + numero + ' #leyendaGeneradoresCReservFria').hide();
    $('#detalle' + numero + ' #chkArranqueBlackStart').hide();
    $('#detalle' + numero + ' #txtArranqueBlackStart').hide();

    var flagRFGeneradores = parseInt($('#detalle' + numero + ' #hfFlagCentralRsvFriaToRegistrarUnidad').val());

    if (idModoGrupo > 0) {
        var regModoOp = getModoFromListaModo(GLOBAL_HO.ListaModosOperacion, idModoGrupo);
        if (regModoOp != null && regModoOp.Gruporeservafria == MODO_GRUPORESERVAFRIA) {
            $('#detalle' + numero + ' #chkArranqueBlackStart').show();
            $('#detalle' + numero + ' #txtArranqueBlackStart').show();
            if (APP_OPCION != OPCION_VER) {
                $('#detalle' + numero + ' #leyendaBlackStartCReservFria').show();
                if (flagRFGeneradores == FLAG_GRUPORESERVAFRIA_TO_REGISTRAR_UNIDADES) {
                    $('#detalle' + numero + ' #leyendaGeneradoresCReservFria').show();
                }
            } else {
            }
        }
    }

    //Ocultar UI para check Ensayo potencia efectiva
    mostrarCheckEnsayoPotenciaEfectiva(numero);
}

// actualiza el valor del tipo de operacion seleccionado del combobox padre
function seteaCombosHijosunidades(tipoOperacion) {
    for (var i = 0; i < GLOBAL_HO.ListaUnidades.length; i++) {
        $("#cb" + GLOBAL_HO.ListaUnidades[i].Equicodi).val(tipoOperacion);
    }
}

//Actualizar los inputs de fecha
function actualizarFechasUIHoraOperacion(tipoHo, numero) {
    tipoHo = parseInt(tipoHo) || 0;

    switch (tipoHo) {
        case TIPO_HO_UNIDAD:
            var MatrizunidadesExtraHop = unidEsp_listarHop(numero);
            var listaUnidades = MatrizunidadesExtraHop;

            var listaHo = [];
            if (listaUnidades.length > 0) {
                for (var i = 0; i < listaUnidades.length; i++) {
                    var listaHoByUnid = listaUnidades[i].ListaHo;
                    listaHo = listaHo.concat(listaHoByUnid);
                    for (var j = 0; j < listaHoByUnid.length; j++) {
                        var objHo = listaHoByUnid[j];

                        var idFila = objHo.RefIdFila;
                        var id_txtOrdenArranqueH = idFila + " " + 'input[name=txtOrdenArranqueH]';
                        var id_txtEnParaleloH = idFila + " " + 'input[name=txtEnParaleloH]';
                        var id_txtOrdenParadaH = idFila + " " + 'input[name=txtOrdenParadaH]';
                        var id_txtFueraParaleloH = idFila + " " + 'input[name=txtFueraParaleloH]';

                        var id_txtOrdenArranqueF = idFila + " " + 'select[name=txtOrdenArranqueF]';
                        var id_txtEnParaleloF = idFila + " " + 'input[name=txtEnParaleloF]';
                        var id_txtOrdenParadaF = idFila + " " + 'input[name=txtOrdenParadaF]';
                        var id_txtFueraParaleloF = idFila + " " + 'input[name=txtFueraParaleloF]';

                        var objRef = {
                            id_ref_txtOrdenArranqueH: id_txtOrdenArranqueH,
                            id_ref_txtEnParaleloH: id_txtEnParaleloH,
                            id_ref_txtOrdenParadaH: id_txtOrdenParadaH,
                            id_ref_txtFueraParaleloH: id_txtFueraParaleloH,
                            id_ref_txtOrdenParadaF: id_txtOrdenParadaF,
                            id_ref_txtFueraParaleloF: id_txtFueraParaleloF,
                            id_ref_txtOrdenArranqueF: id_txtOrdenArranqueF,
                            id_ref_txtEnParaleloF: id_txtEnParaleloF,
                        };

                        $('#detalle' + numero + ' ' + objRef.id_ref_txtOrdenArranqueH).val(obtenerHoraValida($('#detalle' + numero + ' ' + objRef.id_ref_txtOrdenArranqueH).val()));
                        $('#detalle' + numero + ' ' + objRef.id_ref_txtEnParaleloH).val(obtenerHoraValida($('#detalle' + numero + ' ' + objRef.id_ref_txtEnParaleloH).val()));
                        $('#detalle' + numero + ' ' + objRef.id_ref_txtOrdenParadaH).val(obtenerHoraValida($('#detalle' + numero + ' ' + objRef.id_ref_txtOrdenParadaH).val()));
                        $('#detalle' + numero + ' ' + objRef.id_ref_txtFueraParaleloH).val(obtenerHoraValida($('#detalle' + numero + ' ' + objRef.id_ref_txtFueraParaleloH).val()));
                    }
                }
            }

            unidEsp_actualizarUIFechasModo(MatrizunidadesExtraHop, OBJ_REFERENCIA, numero);
            break;

        case TIPO_HO_MODO:
            $(OBJ_REFERENCIA.id_ref_txtOrdenArranqueH).val(obtenerHoraValida($(OBJ_REFERENCIA.id_ref_txtOrdenArranqueH).val()));
            $(OBJ_REFERENCIA.id_ref_txtEnParaleloH).val(obtenerHoraValida($(OBJ_REFERENCIA.id_ref_txtEnParaleloH).val()));
            $(OBJ_REFERENCIA.id_ref_txtOrdenParadaH).val(obtenerHoraValida($(OBJ_REFERENCIA.id_ref_txtOrdenParadaH).val()));
            $(OBJ_REFERENCIA.id_ref_txtFueraParaleloH).val(obtenerHoraValida($(OBJ_REFERENCIA.id_ref_txtFueraParaleloH).val()));
            break;
    }

    //luego de actualizar el modo, actualizar el desglose
    try {
        var listaDesglose = desg_generarListaDesgFromHtml(numero);
        desg_actualizarTablaDesglose(numero, listaDesglose);
    } catch (error) {
    }
}

// Actualizar el Día a mostrar
function actualizartxtFueraParaleloF(ref_txtFueraParaleloH, ref_txtFueraParaleloF, numero) {

    var fechaSiguiente = $('#detalle' + numero + ' #hfFechaSiguiente').val();
    var fecha = $('#detalle' + numero + ' #hfFecha').val();
    var hora = obtenerHoraValida($(ref_txtFueraParaleloH).val());
    if (hora == '00:00:00') {
        $(ref_txtFueraParaleloF).val(fechaSiguiente);
    } else {
        $(ref_txtFueraParaleloF).val(fecha);
    }
}

function activarDesactivarCampos(value, numero) {

    $('#detalle' + numero + ' #btnAbrirBitacora').hide();

    var habilitadoArranqueParada = false;
    if (value) {

        habilitadoArranqueParada = true;

        $('#detalle' + numero + ' #txtOrdenParadaH').val("");
        $('#detalle' + numero + ' #txtOrdenParadaF').val("");

        //verificar si se creo bitacora
        var idevento = $('#detalle' + numero + ' #hfBitacoraIdEvento').val();

        if (APP_OPCION != OPCION_VER) {
            if (idevento > 0) {
                $('#detalle' + numero + ' #btnAbrirBitacora').prop('value', 'Actualizar bitácora');
            } else {
                $('#detalle' + numero + ' #btnAbrirBitacora').prop('value', 'Agregar bitácora');
            }
            $('#detalle' + numero + ' #btnAbrirBitacora').show();
        } else {
            if (idevento > 0) {
                $('#detalle' + numero + ' #btnAbrirBitacora').prop('value', 'Visualizar bitácora');
                $('#detalle' + numero + ' #btnAbrirBitacora').show();
            }
        }
    } else {
        $('#detalle' + numero + ' #txtOrdenParadaF').val($('#detalle' + numero + ' #hfFecha').val());
    }

    $('#detalle' + numero + ' #txtOrdenArranqueH').prop("disabled", habilitadoArranqueParada);
    $('#detalle' + numero + ' #txtOrdenParadaH').prop("disabled", habilitadoArranqueParada);

    unidEsp_HabilitacionCheckFS(value, numero);
}

function ui_setInputmaskHora(ref_element) {
    $(ref_element).inputmask({
        mask: "h:s:s",
        placeholder: "hh:mm:ss",
        alias: "datetime",
        hourformat: 24
    });
}

function ui_setInputmaskHoraMin(ref_element) {
    $(ref_element).inputmask({
        mask: "h:s",
        placeholder: "hh:mm",
        alias: "datetime",
        hourformat: 24
    });
}

function view_accionAceptarGuardarForm() {

    const divsDetalle = document.querySelectorAll("[id^='detalle']");
    var divsInitialStateFinal = {};
    // Almacena los innerHTMLs iniciales de los divs en el objeto
    divsDetalle.forEach(div => {
        var visibleDiv = $(div).css('display');

        //solo considerar formularios visible
        if (visibleDiv != 'none') {
            const numero = div.id.replace('detalle', ''); // obtiene el número del ID
            var valor = ho_getObjFormularioFromHtml(numero);
            divsInitialStateFinal["detalle" + numero] = valor;
        }
    });
    const claves = Object.keys(divsInitialStateFinal);
    LISTA_HORAOP_FORM_EDITABLE = claves;
}

///////////////////////////////////////////////////////////////////////////////////////////////////
/// Eventos .js de edición en Listado
///////////////////////////////////////////////////////////////////////////////////////////////////

var _cambiosPendientes = false;

function editarMOEnGridClone(idCelda, idLista, idCentral, valorActual, fila) {

    valorActual = $.trim(valorActual);

    var seleccionado = "";
    var items = "";

    var td = $(`.DTFC_Cloned #${idCelda}`);

    originalHTML = td.find("span").prop("outerHTML");

    var listaModosOperacion = LISTA_MODO_OPERACION;

    for (var i = 0; i < listaModosOperacion.length; i++) {

        if (listaModosOperacion[i].Equipadre == idCentral) {

            seleccionado = "";

            if (listaModosOperacion[i].Gruponomb == valorActual) {
                seleccionado = " selected";
            }

            items += `<option value="${listaModosOperacion[i].Grupocodi}" ${seleccionado}>${listaModosOperacion[i].Gruponomb}</option>`;
        }
    }

    td.html(`<select id="${idLista}" name="${idLista}" class="cboEnGrid cboModoOperacion cboMOClone${fila}">${items}</select>`);

    td.attr("valor-original", originalHTML);

    aplicarOnChangeClone(idLista, fila);
}

function editarMOEnGrid(idCelda, idLista, idCentral, valorActual, fila) {

    valorActual = $.trim(valorActual);

    var seleccionado = "";
    var items = "";

    var td = $(`#${idCelda}`);
    originalHTML = td.find("span").prop("outerHTML");

    var listaModosOperacion = LISTA_MODO_OPERACION;

    for (var i = 0; i < listaModosOperacion.length; i++) {

        if (listaModosOperacion[i].Equipadre == idCentral) {

            seleccionado = "";

            if (listaModosOperacion[i].Gruponomb == valorActual) {
                seleccionado = " selected";
            }

            items += `<option value="${listaModosOperacion[i].Grupocodi}" ${seleccionado}>${listaModosOperacion[i].Gruponomb}</option>`;
        }
    }

    td.html(`<select id="${idLista}" name="${idLista}" class="cboEnGrid cboModoOperacion cboMOClone${fila}">${items}</select>`);

    td.attr("valor-original", originalHTML);

    aplicarOnChange(idLista, fila);

    editarMOEnGridClone(idCelda, idLista, idCentral, valorActual, fila);
}

function editarCalifEnGrid(idCelda, idTxt, valorActual) {

    var items = "";
    var seleccionado = "";

    var td = $(`#${idCelda}`);

    originalHTML = td.find("span").prop("outerHTML");

    for (var i = 0; i < LISTA_TIPO_OPERACION.length; i++) {

        seleccionado = "";

        if (LISTA_TIPO_OPERACION[i].Subcausacodi == valorActual) {
            seleccionado = " selected";
        }

        items += `<option value="${LISTA_TIPO_OPERACION[i].Subcausacodi}" ${seleccionado}>${LISTA_TIPO_OPERACION[i].Subcausadesc}</option>`;
    }

    td.html(`<select id="${idTxt}" style=\"width:130px\" name="${idTxt}" class="cboEnGrid">${items}</select>`);

    td.attr("valor-original", originalHTML);

    aplicarOnChange(idTxt, 0);
}

function aplicarOnChangeClone(idControl, fila) {

    $(".DTFC_Cloned #" + idControl).on("change", function () {
        _cambiosPendientes = true;

        $("#botones-listado").css("display", "block");
        $(this).addClass("txtEnGridEditado");

        var a = $(this).parent().parent().parent().find("td a.des_hop");
        a.css("display", "");

        // Clone
        var aCloned = $(".DTFC_Cloned #" + a.attr("id"));
        aCloned.css("display", "");

        // Original x si estamos editando desde la tabla clonada
        var aOriginal = $("#reporteHO #" + a.attr("id"));
        aOriginal.css("display", "");

        if (!($(this).hasClass("cboModoOperacion"))) {
            // Clone desde TipoOperación
        }
        else {
            $("#reporteHO .cboMOClone" + fila + " option[value='" + $(this).val() + "']").prop('selected', true);
        }
    });
}

function aplicarOnChange(idControl, fila) {
    $("#" + idControl).on("change", function () {
        _cambiosPendientes = true;

        $("#botones-listado").css("display", "block");

        $(this).addClass("txtEnGridEditado");

        // Original
        var a = $(this).parent().parent().parent().find("td a.des_hop");
        a.css("display", "");

        // Clone
        var aCloned = $(".DTFC_Cloned #" + a.attr("id"));
        aCloned.css("display", "");

        // Original x si estamos editando desde la tabla clonada
        var aOriginal = $("#reporteHO #" + a.attr("id"));
        aOriginal.css("display", "");

        if (!($(this).hasClass("cboModoOperacion"))) {
            // Clone desde TipoOperación
        }
        else {

            $("#reporteHO .cboMOClone" + fila + " option[value='" + $(this).val() + "']").prop('selected', true);
        }
    });
}

function aplicarTxtGrdOnChange(idControl) {

    if (idControl == "") {
        idControl = ".txtEnGrid";
    }

    ui_setInputmaskHora(idControl);
    $(idControl).on('change', function (e) {

        _cambiosPendientes = true;

        $("#botones-listado").css("display", "block");

        // Original
        var a = $(this).parent().parent().find("td a.des_hop");
        a.css("display", "");

        // Clone
        var aCloned = $(".DTFC_Cloned #" + a.attr("id"));
        aCloned.css("display", "");

        // Original
        var a = $(this).parent().parent().find("td a.des_hop");
        a.css("display", "");

        // Clone
        var aCloned = $(".DTFC_Cloned #" + a.attr("id"));
        aCloned.css("display", "");

        var valorOriginal = $(this).parent().attr("valor-original");

        if ((valorOriginal == "") || (valorOriginal == undefined)) {

            $(this).addClass("txtEnGridEditado");

            originalHTML = $(this).prop("outerHTML");

            $(this).parent().attr("valor-original", originalHTML);
        }

        $(this).val(obtenerHoraValida($(this).val()));
    });

    $(idControl).parent().attr("valor-original", "");

    $(".hor_grupo span").click();
    $(".hor_tipo_op span").click();

    $("#btnActualizarMultiple").unbind();
    $("#btnActualizarMultiple").click(function () {
        $('#hfConfirmarValInterv').val(0); //inicializar validación de Costo incremental en tiempo real
        ho_ValidarRegistrosEnGrid();
    });
}

function deshacerHOP(fila, aClase) {
    //inputs
    deshacerHOPInput(fila, aClase);

    //calificacion
    deshacerHOPDiv(fila, aClase);

    //ocultar boton de la fila
    $(".DTFC_Cloned #" + aClase).css("display", "none");

    //ocultar boton Grabar
    var ocultarGrabar = true;
    $("#reporteHO .des_hop").each(function () {
        if ($(this).css('display') != 'none') {
            ocultarGrabar = false;
        }
    });

    if (ocultarGrabar) {
        $("#botones-listado").css("display", "none");
    }
}

function deshacerHOPInput(fila, aClase) {
    $("#reporteHO #hop" + fila).find('td').each(function (column, td) {

        var valorOriginal = $(td).attr("valor-original");

        if ((valorOriginal != "") && (valorOriginal != undefined)) {

            $(td).html(valorOriginal);

            $(td).find("select,input").css('color', '');
            $(td).find("select,input").removeClass("txtEnGridEditado");
            aplicarTxtGrdOnChange("#reporteHO #" + $(td).find("input").attr("id"));
        }
    });

    $("#reporteHO #hop" + fila).find("td a.des_hop").css("display", "none");
}

function deshacerHOPDiv(fila, aClase) {
    $("#reporteHO #hop" + fila).find('div').each(function (column, td) {

        var valorOriginal = $(td).attr("valor-original");

        if ((valorOriginal != "") && (valorOriginal != undefined)) {

            $(td).html(valorOriginal);

            $(td).find("select,input").css('color', '');
            $(td).find("select,input").removeClass("txtEnGridEditado");

            var fila = aClase.replace("des_hop_clone_", "");

            var id = $(".cboMOClone" + fila).attr("id");

            aplicarOnChangeClone(id, fila)

            $(td).find("span").click();
        }
    });
}

///////////////////////////////////////////////////////////////////////////////////////////////////
/// Llenar combos de formularios
///////////////////////////////////////////////////////////////////////////////////////////////////

function cargarCentral(numero) {
    $('#detalle' + numero + ' .unidades_modo').hide();
    $('#detalle' + numero + ' #unidades_especiales').html('');

    $('#detalle' + numero + ' #cbCentral2').empty();
    $('#detalle' + numero + ' #cbCentral2').append('<option value="' + $('#detalle' + numero + ' #hfCentral').val() + '">--SELECCIONE--</option>');

    //Resetear Lista de Modos de Operación
    $('#detalle' + numero + ' #cbModoOpGrupo').empty();
    $('#detalle' + numero + ' #cbModoOpGrupo').append('<option value="' + $('#detalle' + numero + ' #hfIdModGrup').val() + '">--SELECCIONE--</option>');

    var idEmpresa = parseInt($('#detalle' + numero + ' #cbEmpresa2').val());

    $('#detalle' + numero + ' #cbCentral2').empty();
    $('#detalle' + numero + ' #cbCentral2').append('<option value="' + $('#detalle' + numero + ' #hfCentral').val() + '">--SELECCIONE--</option>');

    var elementosFiltradosCentrales = GLOBAL_HO.ListaCentrales.filter((x) => x.Emprcodi == idEmpresa);

    for (var i = 0; i < elementosFiltradosCentrales.length; i++) {
        $('#detalle' + numero + ' #cbCentral2').append('<option value=' + elementosFiltradosCentrales[i].Equicodi + '>' + elementosFiltradosCentrales[i].Equinomb + '</option>');
    }
}

function cargarListaModo_Grupo(numero) {
    var cbDisabled = APP_OPCION == OPCION_NUEVO ? "" : "disabled";

    var idCentral = $('#detalle' + numero + ' #cbCentral2').val();
    if (idCentral == null) {
        idCentral = ID_CENTRAL_DEFAULT_SELECT;
    }

    const resultados = GLOBAL_HO.ListaModosOperacion.filter(modo => (modo.Equipadre == idCentral || idCentral < 0));

    var html_ComboMO = '';
    html_ComboMO += `<select id="cbModoOpGrupo" name="cbModoOp" style="width: 325px;" ${cbDisabled} >`;
    html_ComboMO += '<option value="' + ID_MODO_DEFAULT_SELECT + '">--SELECCIONE--</option>';

    // Crear una nueva lista a partir de los objetos filtrados
    if (resultados != null) {

        for (var i = 0; i < resultados.length; i++) {
            html_ComboMO += '<option value="' + resultados[i].Grupocodi + '">' + resultados[i].Gruponomb + '</option>';
        }
    } else {

        if (Model.ListaGrupo != null) {
            for (var j = 0; j < Model.ListaGrupo.length; j++) {
                html_ComboMO += '<option value="' + Modelo.ListaGrupo[j].Equicodi + '">' + Modelo.ListaGrupo[j].Equinomb + '</option>';
            }
        }
    }
    html_ComboMO += '</select>';

    $('#detalle' + numero + ' #listaModoGrupo').html(html_ComboMO);
    if (APP_OPCION == OPCION_NUEVO) {
        $('#detalle' + numero + ' #cbModoOpGrupo').multipleSelect({
            //width: '683px',
            filter: true,
            single: true,
            onClose: function () {
                var idGrupo = obtenerValorModoDeOperacion(numero);
                var objModo = getModoFromListaModo(GLOBAL_HO.ListaModosOperacion, parseInt(idGrupo));
                if (objModo != null) {
                    $('#detalle' + numero + ' #hfFlagModoEspecial').val(objModo.FlagModoEspecial);
                }

                $('#detalle' + numero + ' #hfIdModGrup').val(idGrupo);

                seleccionarPorModoNuevoForm(numero, 1);
            }
        });
    }

    $('#detalle' + numero + ' .unidades_modo').hide();

    if (APP_OPCION == OPCION_NUEVO) {
        var arrayModo = [];
        arrayModo.push($('#detalle' + numero + ' #hfIdModGrup').val());
        $('#detalle' + numero + ' #cbModoOpGrupo').multipleSelect('setSelects', arrayModo);
    } else {
        $('#detalle' + numero + ' #cbModoOpGrupo').val($('#detalle' + numero + ' #hfIdModGrup').val());
    }

    $('#detalle' + numero + ' #cbModoOpGrupo').change(function () {
        $('#detalle' + numero + ' .unidades_modo').hide();
        view_EnabledOrDisabledUIHoraOperacion(numero);
        //si esta en la vista de coordinador, entonces cargar el modelo
        if (APP_OPCION == OPCION_NUEVO) {
            cargarListadoUnidadesEspeciales($('#detalle' + numero + ' #cbModoOpGrupo').val(), numero);
        }
    });

    //si esta en la vista de coordinador, entonces cargar el modelo

    cargarListadoUnidadesEspeciales($('#detalle' + numero + ' #cbModoOpGrupo').val(), numero);
    view_EnabledOrDisabledUIHoraOperacion(numero);
}

function cargarListadoUnidadesEspeciales(idModo, numero) {
    idModo = parseInt(idModo) || 0;
    $('#detalle' + numero + ' .unidades_modo').hide();
    $('#detalle' + numero + ' .unidades_no_esp').hide();
    $('#detalle' + numero + ' #unidades_especiales').html('');
    $('#detalle' + numero + ' #unidades_no_esp').html('');

    var html_ListaUnidadesEspeciales = cargaHtml_ListaUnidadesEspeciales(numero);

    $('#detalle' + numero + ' #unidades_especiales').html(html_ListaUnidadesEspeciales);

    view_EnabledOrDisabledUIHoraOperacion(numero);

    var tipoModoUnidadEspecial = $('#detalle' + numero + ' #tipoModoUnidadEspecial').val();
    var tipoOpDefecto = $('#detalle' + numero + ' #tipoSubcausaDefecto').val();

    $('#detalle' + numero + ' select[name=cbTipoOperacion]').each(function () {
        $('#detalle' + numero + ' #' + this.id).val(tipoOpDefecto);
    });

    $('#detalle' + numero + ' #hfIdTipoModOp').val(tipoModoUnidadEspecial);


    if (tipoModoUnidadEspecial == "0") {
        $('#detalle' + numero + ' .unidades_modo').show();
        $('#detalle' + numero + ' .unidades_modo.titulo td').text("Seleccione Unidades para el Modo de Operación");
        unidEsp_mostrarTablaFormularioUnidadesEspeciales(numero);
    }

}

function cargaHtml_ListaUnidadesEspeciales(numero) {

    var html_UnidadesModo = '';

    // Inicio de la tabla
    html_UnidadesModo += '<table class="tabla-search" id="unidadesModoOperacionExtra">';
    var lista = listarUnidadesXModo($('#detalle' + numero + ' #cbModoOpGrupo').val())

    if (lista != null && lista.length > 0) {
        for (var i = 0; i < lista.length; i++) {
            var item = lista[i];
            var idCb = "cb" + item.Equicodi;
            var check = "";
            var classEstado = item.Equiestado !== "B" ? "" : "unidad_baja";
            if (item.Equicodi == item.EquiCodiSelect) {
                check = "checked";
            }

            html_UnidadesModo += '<tr>';

            html_UnidadesModo += '<td>';
            html_UnidadesModo += '<table class="form_unid_esp">';
            html_UnidadesModo += '<tr>';
            html_UnidadesModo += '<td class="unidad_esp ' + classEstado + '">';
            html_UnidadesModo += '<input name="chkUnidades" type="checkbox" id="' + item.Equicodi + '" ' + check + ' disabled />' + item.Equinomb;

            if (item.Equiestado !== "B") {
                html_UnidadesModo += '<a class="btn_add_hop" href="JavaScript:unidEsp_agregarHoEspecial(' + item.Equicodi + ',' + numero + ')">';
                html_UnidadesModo += '<img src="' + siteRoot + 'Content/Images/btn-add.png">';
                html_UnidadesModo += '</a>';
            }

            html_UnidadesModo += '</td>';
            html_UnidadesModo += '</tr>';
            html_UnidadesModo += '<tr>';

            if (item.Equiestado !== "B") {
                html_UnidadesModo += '<td id="unidadEspecial_' + item.Equicodi + '"></td>';
            } else {
                html_UnidadesModo += '<td class="txt_estado">Equipo de Baja. Está deshabilitado el registro de horas de operación para esta unidad.</td>';
            }

            html_UnidadesModo += '</tr>';
            html_UnidadesModo += '</table>';
            html_UnidadesModo += '</td>';

            html_UnidadesModo += '</tr>';
        }
    }

    // Cierre de la tabla
    html_UnidadesModo += '</table>';

    var TipoModOp = -1;

    var valor_Modo = GLOBAL_HO.ListaModosOperacion.filter(modo => modo.Grupocodi == $('#cbModoOpGrupo').val());

    if (valor_Modo.length > 0) {
        if (valor_Modo[0].FlagModoEspecial == 'S') {
            TipoModOp = 0;
        }

        $('#hfFlagModoEspecial').val(valor_Modo[0].FlagModoEspecial);
    }

    // Elementos <input> ocultos
    html_UnidadesModo += '<input type="hidden" id="tipoModoUnidadEspecial" value="' + TipoModOp + '" />';
    html_UnidadesModo += '<input type="hidden" id="tipoSubcausaDefecto" value="' + SUBCAUSACODI_DEFAULT + '" />';
    return html_UnidadesModo;
}

function seleccionarPorModoNuevoForm(numero) {

    if (APP_OPCION == OPCION_NUEVO) {
        var comboEmpresa = $('#detalle' + numero + ' #cbEmpresa2');
        var comboCentral = $('#detalle' + numero + ' #cbCentral2');

        var valorSeleccionado = obtenerValorModoDeOperacion(numero);

        if ((valorSeleccionado != undefined) && (valorSeleccionado != "") && (valorSeleccionado != null) && (valorSeleccionado > 0)) {

            var itemSeleccionado = GLOBAL_HO.ListaModosOperacion.filter(modo => modo.Grupocodi == parseInt(valorSeleccionado));


            if (parseInt(itemSeleccionado[0].Emprcodi) != parseInt(comboEmpresa.val())) {
                comboEmpresa.val(itemSeleccionado[0].Emprcodi);

                comboEmpresa.trigger('change');

                var arrayModo = [];
                arrayModo.push(valorSeleccionado);

                $('#detalle' + numero + ' #cbModoOpGrupo').multipleSelect('setSelects', arrayModo);
            }

            comboCentral.val(itemSeleccionado[0].Equipadre);

        }
    }
}

function obtenerValorModoDeOperacion(numero) {

    var valorSeleccionado;

    var resultado = $('#detalle' + numero + ' #cbModoOpGrupo').multipleSelect('getSelects');

    if (resultado == "[object Object]")
        valorSeleccionado = "-1";
    else
        valorSeleccionado = resultado[0];

    return valorSeleccionado;
}

///////////////////////////////////////////////////////////////////////////////////////////////////
/// Operaciones de Registro, Edicion de Hora de Operación Principal
///////////////////////////////////////////////////////////////////////////////////////////////////

//Validar formulario de la hora de operación
async function ho_ValidarFormulario() {
    $("#divValidacion_div").html('');

    var esValidoHtml = ho_listarHoDTOPestaniaDetalle();

    await ho_ValidarListaHoDTO(esValidoHtml);
}

async function ho_ValidarRegistrosEnGrid() {
    $("#divValidacion_div").html('');

    var esValidoHtml = ho_listarHoDTOPestanioListado();

    await ho_ValidarListaHoDTO(esValidoHtml);
}

var listaHoTmp = []; //lista de horas de operación padre, y cada padre tiene una lista de hijos

function ho_listarHoDTOPestaniaDetalle() {
    //1.
    view_accionAceptarGuardarForm();

    //1. Actualizar cajas de texto de fechas y horas del modo de operación como de sus unidades especiales
    // Pasar el html a la variable LISTA_HORAOP_FORM_EDITABLE

    var msj = '';
    for (var contFormulario = 0; contFormulario < LISTA_HORAOP_FORM_EDITABLE.length; contFormulario++) {
        var numero = LISTA_HORAOP_FORM_EDITABLE[contFormulario].replace('detalle', ''); // obtiene el número del ID;

        var objHoForm = ho_getObjFormularioFromHtml(numero);

        // Solo filtramos los eliminados (-2)
        if (parseInt(objHoForm.IdPos) > -2) {

            //hora
            actualizartxtFueraParaleloF('#detalle' + numero + ' #txtFueraParaleloH', '#detalle' + numero + ' #txtFueraParaleloF', numero);

            var empresa = $('#detalle' + numero + ' #cbEmpresa2').val();
            var central = $('#detalle' + numero + ' #cbCentral2').val();
            var modo = $('#detalle' + numero + ' #cbModoOpGrupo').val();

            if (APP_OPCION == OPCION_NUEVO) {
                modo = $('#detalle' + numero + ' #hfIdModGrup').val(); // modo de operacion ó grupo
            }

            var calificacion = parseInt($('#detalle' + numero + ' #cbTipoOp').val()) || 0;

            if (empresa == -3) {
                msj += "Debe seleccionar una empresa" + "\n";
            }
            if (central == -3) {
                msj += "Debe seleccionar una central" + "\n";
            }
            if (modo == -3) {
                msj += "Debe seleccionar un modo de operación" + "\n";
            }
            if (calificacion == -1) {
                msj += "Debe seleccionar una calificación" + "\n";
            }

            var flagModoEsp = 0;
            if ($('#detalle' + numero + ' #hfFlagModoEspecial').val() == "S") {
                flagModoEsp = 1;
            }

            //Validación de formulario a Nivel de Modo de Operación
            var msjValidacionHo = ho_validarHoraOperacion(TIPO_HO_MODO, OBJ_REFERENCIA, flagModoEsp, $('#detalle' + numero + ' #hfTipoCentral').val(), $('#detalle' + numero + ' #hfIdPos').val(), numero);
            if (msjValidacionHo != "") {
                alert(msjValidacionHo);
                return false;
            }

            //Validación de formulario a Nivel de Unidades de un Modo de Operación Especial
            if (FLAG_HO_ESPECIAL == flagModoEsp) {
                var numUnid = 0;
                //Verificar que se haya seleccionado al menos una unidad asociada al modo de operación
                $("#detalle" + numero + " input[name='chkUnidades']:checked").each(function () {
                    numUnid += 1;
                });
                if (numUnid == 0) {
                    alert(MSJ_VAL_MODO_SIN_UNIDAD)
                    return false;
                }

                var MatrizunidadesExtraHop = unidEsp_listarHop(numero);
                var msjValidEsp = unidEsp_validarTablaUnidEspeciales(MatrizunidadesExtraHop, OBJ_REFERENCIA, numero);
                if (msjValidEsp != '') {
                    alert(msjValidEsp);
                    return false;
                }

                //Validar nuevamente el formulario debido a actualización por parte de las unidades de las Horas EN PARALELO, FUERA PARALELO del modo
                msjValidacionHo = ho_validarHoraOperacion(TIPO_HO_MODO, OBJ_REFERENCIA, flagModoEsp, $('#detalle' + numero + ' #hfTipoCentral').val(), $('#detalle' + numero + ' #hfIdPos').val(), numero);
                if (msjValidacionHo != "") {
                    alert(msjValidacionHo);
                    return false;
                }
            }
        }
    }

    if (msj.length > 0) {
        alert(msj);
        return false;
    }

    //2. Generar lista de objetos de horas de operación con su detalle
    // Pasar el LISTA_HORAOP_FORM_EDITABLE a LISTA_HORAOP_DTO_EDITABLE, que este con los campos para guardar en BD

    listaHoTmp = []; //lista de horas de operación padre, y cada padre tiene una lista de hijos

    for (var contFormulario = 0; contFormulario < LISTA_HORAOP_FORM_EDITABLE.length; contFormulario++) {
        var numero = LISTA_HORAOP_FORM_EDITABLE[contFormulario].replace('detalle', ''); // obtiene el número del ID;

        // Validación de Desglose
        var objHo = ho_getObjFormularioFromHtml(numero);
        if (objHo.esPrincipal) {
            objHo.ListaDesglose = desg_ajustarLista(objHo.ListaDesglose, -1, GLOBAL_HO.ListaHorasOperacion, numero);
        }

        var msjValDesg = desg_validarData(objHo.ListaDesglose, -1, GLOBAL_HO.ListaHorasOperacion, numero);
        if (msjValDesg != '') {
            alert(msjValDesg);
            return false;
        }

        //si el modo de operación es especial
        //la variable GLOBAL_HO.ListaHorasOperacion[].ListaHoUnidad debe incluir los eliminados pero con estado baja

        var listaH = GLOBAL_HO.ListaHorasOperacion.filter(function (elemento) {
            return elemento.Hopcodi == objHo.Hopcodi;
        });

        tipoHO = glb_FlagModoEspecial(GLOBAL_HO, objHo.IdGrupoModo);

        if (objHo.IdPos >= 0) {

            Object.keys(divsInitialState).forEach(key => {
                const array1 = divsInitialState[key];
                var valorHopcodi = array1.Hopcodi;
                var valor2Hopcodi = objHo.Hopcodi;
                if (valorHopcodi == valor2Hopcodi) {
                    objHo.OpcionCrud = 2;
                }
            });

            var objUpd = ho_updateObjHo(tipoHO, objHo, listaH);
            if (objUpd != null) listaHoTmp.push(objUpd);
        } else if (objHo.IdPos == -1) {
            var objNuevo = ho_createObjHo(tipoHO, objHo, listaH);
            listaHoTmp.push(objNuevo);
        }

    }

    //agregar los eliminados
    if (APP_OPCION == OPCION_EDITAR) {
        if (listaHoTmp.length > 0) {
            var elementosFiltrados = GLOBAL_HO.ListaHorasOperacion.filter((x) => x.Grupocodi == listaHoTmp[0].Grupocodi);
            for (var i = 0; i < elementosFiltrados.length; i++) {
                var hopcodiEditable = elementosFiltrados[i].Hopcodi;

                var eleModif = listaHoTmp.filter((x) => x.Hopcodi == hopcodiEditable);
                if (eleModif != null && eleModif.length > 0) {
                } else {
                    //si no se editó entonces se elimina
                    var objDel = ho_deleteObjHo(elementosFiltrados[i]);
                    if (objDel != null) listaHoTmp.push(objDel);
                }
            }
        }
    }

    return true;
}

function ho_listarHoDTOPestanioListado() {
    listaHoTmp = [];

    var fila = 0;
    var pos = 0;
    var idHopcodi = 0;

    $("#reporteHO .des_hop").each(function () {

        if ($(this).css('display') != 'none') {

            var tr = $(this).parent().parent();
            idHopcodi = tr.attr("id").replace("hop", "");

            if (GLOBAL_HO.ListaHorasOperacion.length > 0) {
                for (var i = 0; i < GLOBAL_HO.ListaHorasOperacion.length; i++) {
                    if (GLOBAL_HO.ListaHorasOperacion[i].Hopcodi == idHopcodi) {
                        pos = i; // encontrado
                    }
                }
            }

            //var fechaValida = getFecha($("#txtFecha").val());
            var fechaValida = $("#txtFecha").val();

            var objRef = {
                hfIdPos: fila,
                cbModoOpGrupo: tr.find(".hor_grupo").find("select").val(),
                id_ref_txtOrdenArranqueF: $("#txtFecha").val(),
                id_ref_txtFueraParaleloF: $("#txtFecha").val(),
                id_ref_txtOrdenParadaF: $("#txtFecha").val(),
                id_ref_txtEnParaleloF: $("#txtFecha").val(),
                id_ref_txtOrdenArranqueH: tr.find(".hor_arranque").find(".txtEnGrid").val(),
                id_ref_txtEnParaleloH: tr.find(".hor_ini").find(".txtEnGrid").val(),
                id_ref_txtOrdenParadaH: tr.find(".hor_parada").find(".txtEnGrid").val(),
                id_ref_txtFueraParaleloH: tr.find(".hor_fin").find(".txtEnGrid").val(),
                id_tipo_op: tr.find(".hor_tipo_op").find("select").val()
            };

            var msjValidacionHo = ho_validarHoraOperacionEnGrid(TIPO_HO_MODO, objRef, 0);

            if (msjValidacionHo != "") {
                grabar = false;
                alert(msjValidacionHo);
                return false;
            }
            else {
                var fechaFin = convertStringToDate(fechaValida, objRef.id_ref_txtFueraParaleloH);

                if (objRef.id_ref_txtFueraParaleloH == "00:00:00") {
                    fechaFin.setDate(fechaFin.getDate() + 1);
                }

                //clonar objeto DTO
                var op = JSON.parse(JSON.stringify(GLOBAL_HO.ListaHorasOperacion[pos]));

                op.Grupocodi = objRef.cbModoOpGrupo; //puede que se haya cambiado el modo

                var objModo = getModoFromListaModo(GLOBAL_HO.ListaModosOperacion, op.Grupocodi);
                var listaEqxModo = listarUnidadesXModo(op.Grupocodi);
                op.Gruponomb = objModo.Gruponomb;

                op.Hophorini = convertStringToDate(fechaValida, objRef.id_ref_txtEnParaleloH);
                op.Hophorfin = fechaFin;
                op.Hophorordarranq = convertStringToDate(fechaValida, objRef.id_ref_txtOrdenArranqueH);
                op.Hophorparada = convertStringToDate(fechaValida, objRef.id_ref_txtOrdenParadaH);
                op.Subcausacodi = objRef.id_tipo_op;
                op.OpcionCrud = 2;

                //verificar que cuando se cambia un modo, la lista de equipos sea del modo seleccionado
                var listaHoUnidadXhora = op.ListaHoUnidad;
                if (listaHoUnidadXhora.length > 0) {

                    if (listaEqxModo != null) {
                        //si el equipo no existe agregarlo al listado de unidades
                        for (var i = 0; i < listaEqxModo.length; i++) {
                            var equicodi = listaEqxModo[i].Equicodi;
                            var objEqExistente = getEquipoFromListaUnidades(listaHoUnidadXhora, equicodi);

                            if (objEqExistente == null) {
                                var objClone = JSON.parse(JSON.stringify(listaHoUnidadXhora[0]));
                                objClone.Equicodi = equicodi;
                                objClone.Equiabrev = listaEqxModo[i].Equiabrev;
                                objClone.Hopunicodi = 0;

                                listaHoUnidadXhora.push(objClone);
                            }
                        }

                        //si el equipo ya no pertenece entonces eliminarlo logicamente
                        for (var i = 0; i < listaHoUnidadXhora.length; i++) {
                            var equicodi = listaHoUnidadXhora[i].Equicodi;

                            var objEqModo = objModo.ListaEquicodi.find(x => x == equicodi);
                            if (objEqModo == undefined || objEqModo == null) {
                                listaHoUnidadXhora[i].Hopuniactivo = 0;
                                listaHoUnidadXhora[i].OpcionCrud = -1;
                            }
                        }
                    }
                }

                op.ListaHoUnidad = listaHoUnidadXhora;

                //actualizar hora de inicio y hora de fin de las unidades
                for (var i = 0; i < op.ListaHoUnidad.length; i++) {
                    op.ListaHoUnidad[i].Hophorini = op.Hophorini;
                    op.ListaHoUnidad[i].Hophorfin = op.Hophorfin;
                    op.ListaHoUnidad[i].Hophorordarranq = op.Hophorordarranq;
                    op.ListaHoUnidad[i].Hophorparada = op.Hophorparada;

                    op.ListaHoUnidad[i].Hopunihorini = op.Hophorini;
                    op.ListaHoUnidad[i].Hopunihorfin = op.Hophorfin;
                    op.ListaHoUnidad[i].Hopunihorordarranq = op.Hophorordarranq;
                    op.ListaHoUnidad[i].Hopunihorparada = op.Hophorparada;
                }

                listaHoTmp.push(op);
            }
        }
    });

    //ajustemos los desgloces
    listaHoTmp = desg_ajustarListaDesglose(listaHoTmp);

    return true;
}

async function ho_ValidarListaHoDTO(esValidoHtml) {

    //3. Validar objetos (que no haya cruces)

    if (esValidoHtml) { //aqui debemos agregar el faltante, esto es en detallles

        var esValido = await ho_ValidarMultipleHoraOperacion(listaHoTmp); // 0: si es modo de operacion tipo extra 1: normal

        //validar modos de operación que tienen reserva fria
        var validarRsvFria = val_OrdenArranqueCentralReservaFria(listaHoTmp);
        if (validarRsvFria != "" && validarRsvFria != null) {
            alert(validarRsvFria);
            esValido = false;
        }

        //validar calificacion Prueba aleatoria
        var validarPruebaAleatoria = val_OrdenArranquePruebaAleatoria(listaHoTmp);
        if (validarPruebaAleatoria != "" && validarPruebaAleatoria != null) {
            alert(validarPruebaAleatoria);
            esValido = false;
        }

        //4. Enviar a base de datos
        if (esValido) {
            //Enviar cambios al servidor
            enviarHorasOperacionEms(listaHoTmp);

        } else {
            var label = $("<label>").html("<strong>(*) Alerta del formulario</strong>");
            $("#divValidacion_div").append(label);
            _irATabPantalla();
        }
    } else {
        var label = $("<label>").html("<strong>(*) Alerta del formulario</strong>");
        $("#divValidacion_div").append(label);
        _irATabPantalla();
    }
}

function _irATabPantalla() {
    setTimeout(function () {
        $('html, body').scrollTop($("header").offset().top);
    }, 50);
}

//Registrar o Actualizar Horas de Operación del Formulario
async function ho_ValidarMultipleHoraOperacion(listaObjDTO) { // pos: indice de lista de horas de operacion si es modificacion, -1 si es nuevo

    // Validación de Cruces
    var modelVal = await validarCruceHorasOperacion(listaObjDTO);
    if (modelVal.Resultado == -1 || (modelVal.ListaValCruce != null && modelVal.ListaValCruce.length > 0)) {

        if (modelVal.Resultado > 0) {
            var html = "";
            html += '<ul>';
            for (var i = 0; i < modelVal.ListaValCruce.length; i++) {
                html += `<li>${modelVal.ListaValCruce[i].Mensaje}</li>`;
            }
            html += '</ul>';
            $("#divValidacion_div").html(html);
        }

        return false;
    }

    //Validar desglose y validar día anterior
    var isVerif = validarDesgloseXHorasOperacion(listaObjDTO) && validarHorasOperacionArranqueYParada(listaObjDTO);
    if (!isVerif)
        return false;
    else {
        return true;
    }

}

//Listar las horas de operación generadas por el formulario
function ho_getObjFormularioFromHtml(numero) {
    var objMain = {};
    objMain.esPrincipal = true;

    objMain.Hopcodi = $('#detalle' + numero + ' #hfHopCodi').val(); //IdPos
    objMain.IdPos = $('#detalle' + numero + ' #hfIdPos').val(); //IdPos
    objMain.FlagModoEspecial = $('#detalle' + numero + ' #hfFlagModoEspecial').val() == "S" ? "S" : "N"; //IdPos
    objMain.emprcodi = $('#detalle' + numero + ' #cbEmpresa2').val(); //emprcodi
    objMain.central = $('#detalle' + numero + ' #cbCentral2').val();
    objMain.IdGrupoModo = $('#detalle' + numero + ' #cbModoOpGrupo').val(); // modo de operacion ó grupo

    objMain.fecha = $('#txtFecha').val();
    objMain.fechaFin = $('#detalle' + numero + ' #txtFueraParaleloF').val(); // fecha de fuera paralelo si es fin del dia 24:00:00 hs
    if (APP_OPCION == OPCION_NUEVO) {
        objMain.IdGrupoModo = $('#detalle' + numero + ' #hfIdModGrup').val(); // modo de operacion ó grupo
    }

    objMain.ordenArranqueF = $('#detalle' + numero + ' #txtOrdenArranqueF').val();
    objMain.ordenArranque = $('#detalle' + numero + ' #txtOrdenArranqueH').val();
    objMain.enParaleloF = $('#detalle' + numero + ' #txtEnParaleloF').val(); // HoraIni (HH:mm:ss)
    objMain.enParalelo = $('#detalle' + numero + ' #txtEnParaleloH').val(); // HoraIni (HH:mm:ss)
    objMain.ordenParadaF = $('#detalle' + numero + ' #txtOrdenParadaF').val();
    objMain.ordenParada = $('#detalle' + numero + ' #txtOrdenParadaH').val();
    objMain.fueraParaleloF = $('#detalle' + numero + ' #txtFueraParaleloF').val(); //HoraFin (HH:mm:ss)
    objMain.fueraParalelo = $('#detalle' + numero + ' #txtFueraParaleloH').val(); //HoraFin (HH:mm:ss)

    objMain.observacion = $('#detalle' + numero + ' #txtObservacion').val();
    objMain.tipoOperacion = $('#detalle' + numero + ' #cbTipoOp').val();
    objMain.motivoOperacionForzada = parseInt($('#detalle' + numero + ' #cbMotOpForzada').val()) || -1;
    objMain.descripcion = $('#detalle' + numero + ' #TxtDescripcion').val();

    // valores por defecto de los checkbox en BD
    objMain.fueraServicio = null;
    objMain.compOrdArranq = "N";
    objMain.compOrdParad = "N";
    objMain.sistAislado = 0;
    objMain.limTrans = 'N';
    objMain.arranqBlackStart = 'N';
    objMain.ensayoPotenciaEfectiva = 'N';
    objMain.ensayoPotenciaMinima = 'N';
    objMain.HopPruebaExitosa = 0;

    objMain.MatrizunidadesExtra = [];
    $('#detalle' + numero + ' input[name=chkUnidades]:checked').each(function () {

        var objEquipo = getEquipoFromListaUnidades(GLOBAL_HO.ListaUnidades, this.id);
        var equiabrevDesc = objEquipo != null ? objEquipo.Equiabrev : '';

        var entity = {
            Emprcodi: $('#detalle' + numero + ' #cbEmpresa2').val(),
            EquiCodi: this.id, // unidad para el modo de operacion
            Equiabrev: equiabrevDesc,
        };
        objMain.MatrizunidadesExtra.push(entity);
    });

    objMain.MatrizunidadesExtraHop = unidEsp_listarHop(numero); //Unidades especiales

    objMain.valCkeckfueraServ = $("#detalle" + numero + " #chkFueraServicio").prop("checked");
    if (objMain.valCkeckfueraServ == 1) {
        objMain.fueraServicio = 'F';
    }

    objMain.valChckcompOrdArranq = $("#detalle" + numero + " #chkCompensacionOrdArrq").prop("checked");
    if (objMain.valChckcompOrdArranq == 1) {
        objMain.compOrdArranq = 'S';
    }

    objMain.valChckcompOrdParad = $("#detalle" + numero + " #chkCompensacionOrdPar").prop("checked");
    if (objMain.valChckcompOrdParad == 1) {
        objMain.compOrdParad = 'S';
    }

    objMain.valChcksistAislado = $("#detalle" + numero + " #chkSistemaAislado").prop("checked");
    if (objMain.valChcksistAislado == 1) {
        objMain.sistAislado = 1;
    }

    objMain.valChkPruebaExitosa = $("#detalle" + numero + " #chkPruebaExitosa").prop("checked");
    if (objMain.valChkPruebaExitosa) {
        objMain.HopPruebaExitosa = 1;
    }

    objMain.valChkLimTransm = $("#detalle" + numero + " #chkLimTransm").prop("checked");
    if (objMain.valChkLimTransm == 1) {
        objMain.limTrans = 'S';
    }

    objMain.valChkArranqBlackStart = $("#detalle" + numero + " #chkArranqueBlackStart").prop("checked");
    if (objMain.valChkArranqBlackStart == 1) {
        objMain.arranqBlackStart = 'S';
    }

    objMain.valChkEnsayoPotenciaEfectiva = $("#detalle" + numero + " #chkEnsayoPotenciaEfectiva").prop("checked");
    if (objMain.valChkEnsayoPotenciaEfectiva == 1) {
        objMain.ensayoPotenciaEfectiva = 'S';
    }

    objMain.valChkEnsayoPotenciaMinima = $("#detalle" + numero + " #chkEnsayoPotenciaMinima").prop("checked");
    if (objMain.valChkEnsayoPotenciaMinima == 1) {
        objMain.ensayoPotenciaMinima = 'S';
    }

    // Bitacora
    objMain.BitacoraHophoriniFecha = $("#detalle" + numero + " #hfBitacoraHophoriniFecha").val();
    objMain.BitacoraHophoriniHora = $("#detalle" + numero + " #hfBitacoraHophoriniHora").val();
    objMain.BitacoraHophorfinFecha = $("#detalle" + numero + " #hfBitacoraHophorfinFecha").val();
    objMain.BitacoraHophorfinHora = $("#detalle" + numero + " #hfBitacoraHophorfinHora").val();
    objMain.BitacoraDescripcion = $("#detalle" + numero + " #hfBitacoraDescripcion").val();
    objMain.BitacoraComentario = $("#detalle" + numero + " #hfBitacoraComentario").val();
    objMain.BitacoraDetalle = $("#detalle" + numero + " #hfBitacoraDetalle").val();
    objMain.BitacoraIdSubCausaEvento = $("#detalle" + numero + " #hfBitacoraIdSubCausaEvento").val();
    objMain.BitacoraIdEvento = $("#detalle" + numero + " #hfBitacoraIdEvento").val();
    objMain.BitacoraIdTipoEvento = $("#detalle" + numero + " #hfBitacoraIdTipoEvento").val();
    objMain.BitacoraIdEquipo = $("#detalle" + numero + " #hfBitacoraIdEquipo").val();
    objMain.BitacoraIdEmpresa = $("#detalle" + numero + " #hfBitacoraIdEmpresa").val();
    objMain.BitacoraIdTipoOperacion = $("#detalle" + numero + " #hfBitacoraIdTipoOperacion").val();

    // Detalle
    objMain.ListaDesglose = desg_generarListaDesgFromHtml(numero);

    return objMain;
}

//Agrega el obj Hora de Operación a lista 
function ho_createObjHo(tipoHO, objForm) {

    var objModo = getModoFromListaModo(GLOBAL_HO.ListaModosOperacion, objForm.IdGrupoModo);
    var emprnomb = objModo != null ? objModo.Emprnomb : '';
    var centralDesc = objModo != null ? objModo.Central : '';
    var grupoabrevDesc = objModo != null ? objModo.Grupoabrev : '';
    var gruponombDesc = objModo != null ? objModo.Gruponomb : '';
    var subcausadesc = getTipoOperacion(objForm.tipoOperacion);

    //creamos la hora de operacion para el modo de operacion
    var entity3 =
    {
        Hopcodi: -1,
        Hophorini: moment(convertStringToDate(objForm.enParaleloF, objForm.enParalelo)),
        Hophorfin: moment(convertStringToDate(objForm.fueraParaleloF, objForm.fueraParalelo)),
        Hophorordarranq: convertStringToDate(objForm.ordenArranqueF, objForm.ordenArranque),
        Hophorparada: convertStringToDate(objForm.ordenParadaF, objForm.ordenParada),
        Emprcodi: objForm.emprcodi,
        Emprnomb: emprnomb,
        Equipadre: objForm.central,
        Equicodi: objForm.central,
        Grupocodi: objForm.IdGrupoModo,
        FlagModoEspecial: objForm.FlagModoEspecial,
        FlagTipoHo: TIPO_HO_MODO_BD,
        Central: centralDesc,
        Grupoabrev: grupoabrevDesc,
        Gruponomb: gruponombDesc,
        Subcausacodi: objForm.tipoOperacion,
        Subcausadesc: subcausadesc,
        Hopdesc: objForm.descripcion,
        OpcionCrud: 1, // -1: eliminar, 0:lectura, 1:crear, 2:actualizar 
        UnidadesExtra: objForm.MatrizunidadesExtra,
        Hopfalla: objForm.fueraServicio,
        Hopobs: objForm.observacion,
        Hopcompordarrq: objForm.compOrdArranq,
        Hopcompordpard: objForm.compOrdParad,
        Hopsaislado: objForm.sistAislado,
        Hoplimtrans: objForm.limTrans,
        Hopcausacodi: objForm.motivoOperacionForzada,
        Hoparrqblackstart: objForm.arranqBlackStart,
        Hopensayope: objForm.ensayoPotenciaEfectiva,
        Hopensayopmin: objForm.ensayoPotenciaMinima,
        HopPruebaExitosa: objForm.HopPruebaExitosa,

        ListaDesglose: objForm.ListaDesglose,

        BitacoraHophoriniFecha: objForm.BitacoraHophoriniFecha,
        BitacoraHophoriniHora: objForm.BitacoraHophoriniHora,
        BitacoraHophorfinFecha: objForm.BitacoraHophorfinFecha,
        BitacoraHophorfinHora: objForm.BitacoraHophorfinHora,
        BitacoraDescripcion: objForm.BitacoraDescripcion,
        BitacoraComentario: objForm.BitacoraComentario,
        BitacoraDetalle: objForm.BitacoraDetalle,
        BitacoraIdSubCausaEvento: objForm.BitacoraIdSubCausaEvento,
        BitacoraIdEvento: objForm.BitacoraIdEvento,
        BitacoraIdTipoEvento: objForm.BitacoraIdTipoEvento,
        BitacoraIdEquipo: objForm.BitacoraIdEquipo,
        BitacoraIdEmpresa: objForm.BitacoraIdEmpresa,
        BitacoraIdTipoOperacion: objForm.BitacoraIdTipoOperacion
    };

    //registramos unidades asociadas
    var listaHoUnidadNueva = [];
    switch (tipoHO) {
        case FLAG_HO_ESPECIAL: //Tipo de modo de operacion extra
            listaHoUnidadNueva = ho_CreateUpdateHoraOperacionUnidEsp(entity3, objForm.IdGrupoModo, objForm.MatrizunidadesExtra, objForm.MatrizunidadesExtraHop);
            break;
        case FLAG_HO_NO_ESPECIAL: //Tipo de modo de operacion normal

            var listaEqXModo = listarUnidadesXModo(objForm.IdGrupoModo);

            for (var j = 0; j < listaEqXModo.length; j++) {
                var entity4 =
                {
                    Hopcodi: 0,
                    Hophorini: moment(convertStringToDate(objForm.enParaleloF, objForm.enParalelo)),
                    Hophorfin: moment(convertStringToDate(objForm.fueraParaleloF, objForm.fueraParalelo)),
                    Hophorordarranq: convertStringToDate(objForm.ordenArranqueF, objForm.ordenArranque),
                    Hophorparada: convertStringToDate(objForm.ordenParadaF, objForm.ordenParada),
                    Emprcodi: objForm.emprcodi,
                    Equipadre: objForm.central,
                    Equicodi: listaEqXModo[j].Equicodi,
                    Equiabrev: listaEqXModo[j].Equiabrev,
                    FlagTipoHo: TIPO_HO_UNIDAD_BD,
                    OpcionCrud: 1, // -1: eliminar, 0:lectura, 1:crear, 2:actualizar 
                    Subcausacodi: objForm.tipoOperacion,
                    Hopcodipadre: 0,

                    Hopuniactivo: objForm.ListaHoUnidad ? objForm.ListaHoUnidad[0].Hopuniactivo : 1,
                    Hopunicodi: 0,
                    Hopunifeccreacion: objForm.ListaHoUnidad ? objForm.ListaHoUnidad[0].Hopunifeccreacion : null,
                    Hopunifecmodificacion: objForm.ListaHoUnidad ? objForm.ListaHoUnidad[0].Hopunifecmodificacion : null,
                    Hopunihorarranq: objForm.ListaHoUnidad ? objForm.ListaHoUnidad[0].Hopunihorarranq : convertStringToDate(objForm.enParaleloF, objForm.enParalelo),
                    Hopunihorfin: objForm.ListaHoUnidad ? objForm.ListaHoUnidad[0].Hopunihorfin : moment(convertStringToDate(objForm.fueraParaleloF, objForm.fueraParalelo)),
                    Hopunihorini: objForm.ListaHoUnidad ? objForm.ListaHoUnidad[0].Hopunihorini : moment(convertStringToDate(objForm.enParaleloF, objForm.enParalelo)),
                    Hopunihorordarranq: objForm.ListaHoUnidad ? objForm.ListaHoUnidad[0].Hopunihorordarranq : convertStringToDate(objForm.ordenArranqueF, objForm.ordenArranque),
                    Hopunihorparada: objForm.ListaHoUnidad ? objForm.ListaHoUnidad[0].Hopunihorparada : convertStringToDate(objForm.ordenParadaF, objForm.ordenParada),
                    Hopuniusucreacion: objForm.ListaHoUnidad ? objForm.ListaHoUnidad[0].Hopuniusucreacion : null,
                    Hopuniusumodificacion: objForm.ListaHoUnidad ? objForm.ListaHoUnidad[0].Hopuniusumodificacion : null,

                    Hoparrqblackstart: objForm.arranqBlackStart,
                    Hopensayope: objForm.ensayoPotenciaEfectiva,
                    Hopensayopmin: objForm.ensayoPotenciaMinima,
                    Hopfalla: objForm.fueraServicio,
                    Hopcompordarrq: objForm.compOrdArranq,
                    Hopcompordpard: objForm.compOrdParad,
                    Hopsaislado: objForm.sistAislado,
                    Hoplimtrans: objForm.limTrans,
                    Hopcausacodi: objForm.motivoOperacionForzada,
                    Hopobs: objForm.observacion

                };
                //registramos la unidad relacionada al modo de operación
                listaHoUnidadNueva.push(entity4);
            }

            break;

    }

    //lista detalle
    entity3.ListaHoUnidad = listaHoUnidadNueva;

    return entity3;
}

//Actualiza el obj Hora de Operación de la lista
function ho_updateObjHo(tipoHO, objForm, listaHo) {

    //clonar objeto
    if (listaHo.length > 0) {
        var objetoUpdate = JSON.parse(JSON.stringify(listaHo[0]));

        // modificamos horas de operacion del modo de operacion (EVE_HORAOPERACION_DTO)
        objetoUpdate.Hophorini = moment(convertStringToDate(objForm.enParaleloF, objForm.enParalelo));
        objetoUpdate.Hophorfin = moment(convertStringToDate(objForm.fueraParaleloF, objForm.fueraParalelo));
        objetoUpdate.Hophorordarranq = convertStringToDate(objForm.ordenArranqueF, objForm.ordenArranque);
        objetoUpdate.Hophorparada = convertStringToDate(objForm.ordenParadaF, objForm.ordenParada);
        objetoUpdate.Subcausacodi = objForm.tipoOperacion;
        objetoUpdate.Hopdesc = objForm.descripcion;
        objetoUpdate.MatrizunidadesExtra = objForm.MatrizunidadesExtra;
        objetoUpdate.OpcionCrud = objForm.OpcionCrud; // -1: eliminar, 0:lectura, 1:crear, 2:actualizar
        objetoUpdate.Emprcodi = objForm.emprcodi;
        objetoUpdate.Hopfalla = objForm.fueraServicio;
        objetoUpdate.Hopobs = objForm.observacion;
        objetoUpdate.Hopcompordarrq = objForm.compOrdArranq;
        objetoUpdate.Hopcompordpard = objForm.compOrdParad;
        objetoUpdate.Hopsaislado = objForm.sistAislado;
        objetoUpdate.Hoplimtrans = objForm.limTrans;
        objetoUpdate.Hopcausacodi = objForm.motivoOperacionForzada;
        objetoUpdate.Hoparrqblackstart = objForm.arranqBlackStart;
        objetoUpdate.Hopensayope = objForm.ensayoPotenciaEfectiva;
        objetoUpdate.Hopensayopmin = objForm.ensayoPotenciaMinima;
        objetoUpdate.HopPruebaExitosa = objForm.HopPruebaExitosa;
        objetoUpdate.ListaDesglose = objForm.ListaDesglose;

        objetoUpdate.BitacoraHophoriniFecha = objForm.BitacoraHophoriniFecha;
        objetoUpdate.BitacoraHophoriniHora = objForm.BitacoraHophoriniHora;
        objetoUpdate.BitacoraHophorfinFecha = objForm.BitacoraHophorfinFecha;
        objetoUpdate.BitacoraHophorfinHora = objForm.BitacoraHophorfinHora;
        objetoUpdate.BitacoraDescripcion = objForm.BitacoraDescripcion;
        objetoUpdate.BitacoraComentario = objForm.BitacoraComentario;
        objetoUpdate.BitacoraDetalle = objForm.BitacoraDetalle;
        objetoUpdate.BitacoraIdSubCausaEvento = objForm.BitacoraIdSubCausaEvento;
        objetoUpdate.BitacoraIdEvento = objForm.BitacoraIdEvento;
        objetoUpdate.BitacoraIdTipoEvento = objForm.BitacoraIdTipoEvento;
        objetoUpdate.BitacoraIdEquipo = objForm.BitacoraIdEquipo;
        objetoUpdate.BitacoraIdEmpresa = objForm.BitacoraIdEmpresa;
        objetoUpdate.BitacoraIdTipoOperacion = objForm.BitacoraIdTipoOperacion;

        /// modificamos horas de operacion de las unidades relacionadas
        switch (tipoHO) {
            //EVE_HO_UNIDAD_DTO
            case FLAG_HO_ESPECIAL: //Tipo de Modo de Operacion con Unidades Especiales

                objetoUpdate.ListaHoUnidad = ho_CreateUpdateHoraOperacionUnidEsp(objetoUpdate, objForm.IdGrupoModo, objForm.MatrizunidadesExtra, objForm.MatrizunidadesExtraHop);
                break;
            case FLAG_HO_NO_ESPECIAL:

                //verificar que cuando se cambia un modo, la lista de equipos sea del modo seleccionado
                var op = objetoUpdate;
                var listaHoUnidadXhora = objetoUpdate.ListaHoUnidad;

                //actualizar hora de inicio y hora de fin de las unidades
                for (var i = 0; i < listaHoUnidadXhora.length; i++) {
                    listaHoUnidadXhora[i].Hophorini = op.Hophorini;
                    listaHoUnidadXhora[i].Hophorfin = op.Hophorfin;
                    listaHoUnidadXhora[i].Hophorordarranq = op.Hophorordarranq;
                    listaHoUnidadXhora[i].Hophorparada = op.Hophorparada;

                    listaHoUnidadXhora[i].Hopunihorini = op.Hophorini;
                    listaHoUnidadXhora[i].Hopunihorfin = op.Hophorfin;
                    listaHoUnidadXhora[i].Hopunihorordarranq = op.Hophorordarranq;
                    listaHoUnidadXhora[i].Hopunihorparada = op.Hophorparada;
                }

                objetoUpdate.ListaHoUnidad = listaHoUnidadXhora;

                break;
        }

        return objetoUpdate;
    }

    return null;
}

//Actualiza el obj Hora de Operación de la lista
function ho_deleteObjHo(objHo) {

    //clonar objeto
    var objetoDelete = JSON.parse(JSON.stringify(objHo));
    objetoDelete.OpcionCrud = -1; // -1: eliminar, 0:lectura, 1:crear, 2:actualizar

    if (objetoDelete.ListaHoUnidad != null && objetoDelete.ListaHoUnidad.length > 0) {
        for (var i = 0; i < objetoDelete.ListaHoUnidad.length; i++) {
            objetoDelete.ListaHoUnidad[i].OpcionCrud = -1; // -1: eliminar, 0:lectura, 1:crear, 2:actualizar
        }
    }

    return objetoDelete;
}

//Registrar o Actualizar Horas de Operación de las unidades de un Modo de Operación
function ho_CreateUpdateHoraOperacionUnidEsp(objHoModo, idGrupoModo, MatrizunidadesExtra, MatrizunidadesExtraHop) {
    //clonar objeto original
    var listaHo = [];
    listaHo.push(objHoModo);
    var listaHo2 = JSON.parse(JSON.stringify(listaHo));

    //variable para tener la lista de unidades de la tabla web
    var listaHoUnidadAguardar = [];

    //iterar por cada equipo del modo
    var listaEqXModo = listarUnidadesXModo(idGrupoModo);

    if (listaEqXModo.length > 0) {
        for (var j = 0; j < listaEqXModo.length; j++) {
            var flag01 = getFindEquipoxCodUnidadesExtra(listaEqXModo[j].Equicodi, MatrizunidadesExtra);

            if (flag01 > -1) {
                // existe unidad, Registrar o Actualizar las horas de operacion de la tabla

                for (var ttt = 0; ttt < MatrizunidadesExtraHop.length; ttt++) {
                    var objUnidEsp = MatrizunidadesExtraHop[ttt];

                    //verificar por cada equipo que está en la pantalla
                    if (objUnidEsp.Equicodi == listaEqXModo[j].Equicodi) {
                        var listaHoUnidadesTemporal = [];

                        var listaHoTmp = listarHorasOperacionByHopcodipadre(listaHo2, objHoModo.Hopcodi, objUnidEsp.Equicodi);
                        var listaHoTablaHtml = objUnidEsp.ListaHo;

                        for (var hhh = 0; hhh < listaHoTablaHtml.length; hhh++) {
                            var hopEspTmp = listaHoTablaHtml[hhh];
                            var pos02 = getPosHounidad(hopEspTmp.Hopunicodi, listaHoTmp);

                            if (pos02 > -1) { // existe unidad en hora de operación
                                // modificamos horas de operacion de la unidad relacionada
                                var listar = {
                                    Hophorini: hopEspTmp.Hophorini,
                                    Hophorfin: hopEspTmp.Hophorfin,
                                    Hophorordarranq: hopEspTmp.Hophorordarranq,
                                    Hophorparada: hopEspTmp.Hophorparada,
                                    FlagTipoHo: TIPO_HO_UNIDAD_BD,
                                    Hoparrqblackstart: hopEspTmp.arranqBlackStart,
                                    Hopensayope: hopEspTmp.ensayoPotenciaEfectiva,
                                    Hopensayopmin: hopEspTmp.ensayoPotenciaMinima,
                                    OpcionCrud: 2, // -1: eliminar, 0:lectura, 1:crear, 2:actualizar  
                                    Hopunicodi: hopEspTmp.Hopunicodi,
                                    Hopcodi: objHoModo.Hopcodi,
                                    Hopuniactivo: hopEspTmp.Hopuniactivo,
                                    Equicodi: hopEspTmp.Equicodi,
                                    Equiabrev: listaEqXModo[j].Equiabrev,
                                    //Hopunifeccreacion: hopEspTmp.Hopunifeccreacion,
                                    //Hopunifecmodificacion: hopEspTmp.Hopunifecmodificacion,
                                    Hopunihorfin: hopEspTmp.Hophorfin,
                                    Hopunihorini: hopEspTmp.Hophorini,
                                    Hopunihorordarranq: hopEspTmp.Hophorordarranq,
                                    Hopunihorparada: hopEspTmp.Hophorparada

                                }
                                listaHoUnidadesTemporal.push(listar);
                            }
                            else { // no existe unidad en horas de operacion 
                                ///****crear unidad extra nueva
                                newHopcodi = (-1) * (listaHo.length + 1);
                                var entityUnid =
                                {
                                    Hopuniactivo: 1,
                                    Hopunicodi: 0,
                                    Hopcodi: objHoModo.Hopcodi,
                                    Hophorini: hopEspTmp.Hophorini,
                                    Hophorfin: hopEspTmp.Hophorfin,
                                    Hophorordarranq: hopEspTmp.Hophorordarranq,
                                    Hophorparada: hopEspTmp.Hophorparada,
                                    Equicodi: objUnidEsp.Equicodi,
                                    Equiabrev: listaEqXModo[j].Equiabrev,
                                    Emprcodi: objUnidEsp.Emprcodi,
                                    FlagTipoHo: TIPO_HO_UNIDAD_BD,
                                    OpcionCrud: 1, // -1: eliminar, 0:lectura, 1:crear, 2:actualizar 
                                    Hopcodipadre: objHoModo.Hopcodi,
                                    Hoparrqblackstart: hopEspTmp.arranqBlackStart,
                                    Hopensayope: hopEspTmp.ensayoPotenciaEfectiva,
                                    Hopensayopmin: hopEspTmp.ensayoPotenciaMinima,

                                    Hopunihorfin: hopEspTmp.Hophorfin,
                                    Hopunihorini: hopEspTmp.Hophorini,
                                    Hopunihorordarranq: hopEspTmp.Hophorordarranq,
                                    Hopunihorparada: hopEspTmp.Hophorparada
                                };
                                //registramos la unidad relacionada al modo de operación
                                listaHoUnidadesTemporal.push(entityUnid);
                            }
                        }

                        //Eliminado Lógico o Físico
                        var listaHopcodiEliminable = unidEsp_listarHopcodiAEliminar(listaHoTmp, listaHoTablaHtml);

                        if (listaHopcodiEliminable.length > 0) {
                            for (var h2 = 0; h2 < listaHoTmp.length; h2++) {
                                var deleteFisico = false;
                                for (var ti = 0; ti < listaHopcodiEliminable.length; ti++) {
                                    if (listaHopcodiEliminable[ti] == listaHoTmp[h2].Hopunicodi) {
                                        deleteFisico = true; //eliminado fisico de la matriz auxiliar  
                                        if (listaHoTmp[h2].Hopunicodi != 0) {// si unidad viene de Bd -> eliminado lógico
                                            listaHoTmp[h2].OpcionCrud = -1; // eliminado lógico
                                            listaHoTmp[h2].Hopuniactivo = 0;
                                            listaHoUnidadesTemporal.push(listaHoTmp[h2]);
                                        }
                                    }
                                }
                            }


                        }

                        //agregar lista final
                        listaHoUnidadAguardar = listaHoUnidadAguardar.concat(listaHoUnidadesTemporal);
                    }
                }

            }
            else {
                // si no existe unidad en lista de codigos de unidades seleccionadas

                var listaHoTmp = listarHorasOperacionByHopcodipadre(listaHo2, objHoModo.Hopcodi, listaEqXModo[j].Equicodi);

                if (listaHoTmp.length > 0) {
                    for (var h2 = 0; h2 < listaHo2.length; h2++) {
                        var deleteFisico = false;
                        for (var hhh = 0; hhh < listaHoTmp.length; hhh++) {
                            if (listaHoTmp[hhh].Hopcodi == listaHo2[h2].Hopcodi) {
                                deleteFisico = true; //eliminado fisico de la matriz auxiliar  
                                if (listaHoTmp[hhh].Hopunicodi != 0) {// si unidad viene de Bd -> eliminado lógico
                                    listaHoTmp[hhh].OpcionCrud = -1; // eliminado lógico
                                    listaHoTmp[hhh].Hopuniactivo = 0;
                                    deleteFisico = false;
                                }
                            }
                        }
                    }
                }

                listaHoUnidadAguardar = listaHoUnidadAguardar.concat(listaHoTmp);
            }
        }
    }

    return listaHoUnidadAguardar;
}

///////////////////////////////////////////////////////////////////////////////////////////////////
/// Formulario de Unidades Especiales
///////////////////////////////////////////////////////////////////////////////////////////////////

function unidEsp_mostrarTablaFormularioUnidadesEspeciales(numero) {

    var jsEliminar = "";

    var hoppadre = null;
    if (numero > -1) { //Edición de Hora de Operación

        var hopcodi = parseInt($('#detalle' + numero + ' #hfHopCodi').val()) || 0;
        var elementosFiltrados = GLOBAL_HO.ListaHorasOperacion.filter((x) => x.Hopcodi == hopcodi);
        if (elementosFiltrados.length > 0)
            hoppadre = elementosFiltrados[0];
    }

    if (hoppadre == null)
        hoppadre = unidEsp_GetHopDefaultRegistro(numero);

    if (hoppadre.FlagModoEspecial == FLAG_MODO_OP_ESPECIAL) {

        var listaUnidad = listarUnidadesXModo(hoppadre.Grupocodi);

        for (var i = 0; i < listaUnidad.length; i++) {
            var objUnidad = listaUnidad[i];
            var arrayTmp = [];
            arrayTmp.push(hoppadre);

            var listaHoXUnidad = hoppadre.Hopcodi > 0 ? listarHorasOperacionByHopcodipadre(GLOBAL_HO.ListaHorasOperacion, hoppadre.Hopcodi, objUnidad.Equicodi) : arrayTmp;

            //Generar Tabla html por cada unidad
            var htmlXUnidad = '';
            htmlXUnidad += `
            <table class='tabla_ho_esp pretty tabla-icono' id='tabla_ho_esp_${objUnidad.Equicodi}' style='width: 776px'>
                <thead>
                    <tr>
                        <th>Orden de Arranque</th>
                        <th>En Paralelo</th>
                        <th>Orden de Parada</th>
                        <th>Fuera de Paralelo</th>
            `;
            if (APP_OPCION != OPCION_VER) {
                htmlXUnidad += "<th colspan='2' style='display:none;'>Desagregar</th>";
                htmlXUnidad += "<th class='col_btn_add'></th>";
            }
            htmlXUnidad += `
                    </tr>
                </thead>
                <tbody> `;

            for (var m = 0; m < listaHoXUnidad.length; m++) {
                htmlXUnidad += unidEsp_visualizarHoraOperacionUnidadEspecial(listaHoXUnidad[m], listaHoXUnidad[m].Hopcodi, objUnidad.Equicodi, m, OPCION_EDITAR, numero);

                jsEliminar += `unidEsp_quitarHoEspecial(${objUnidad.Equicodi}, ${m}, ${numero});`;
            }
            htmlXUnidad +=
                `</tbody >
            </table >`;

            $('#detalle' + numero + ' #unidadEspecial_' + objUnidad.Equicodi).html(htmlXUnidad);

            $('#detalle' + numero + ' #' + objUnidad.Equicodi).prop("checked", false);
            if (listaHoXUnidad.length > 0) {
                $('#detalle' + numero + ' #' + objUnidad.Equicodi).prop("checked", true);
            }

            //Asignar eventos 
            for (var m = 0; m < listaHoXUnidad.length; m++) {
                unidEsp_asignarEventoPos(objUnidad.Equicodi, m, numero);
            }

            //Mostrar boton agregar 
            if (APP_OPCION != OPCION_VER) {
                $('#detalle' + numero + ' #' + objUnidad.Equicodi).parent().find("a").show();
            }

        }
        //Mostrar deshabilitado las fechas de orden parada cuando esta activado el check
        unidEsp_HabilitacionCheckFS($('#detalle' + numero + ' #chkFueraServicio').prop("checked"), numero);
    }

    if (APP_OPCION != OPCION_VER) {
        $(".btn_add_hop").show();
    } else {
        $(".btn_add_hop").hide();
    }

    /*
    if ((OPCION_DIV == NUEVO_DIV_AGREGAR) && (jsEliminar != "")) {
        // Quitamos las unidades 
        eval(jsEliminar);
    }
    */
}

function unidEsp_visualizarHoraOperacionUnidadEspecial(hoUnidad, hopcodi, equicodi, pos_row, tipoOpcion, numero) {
    var Fecha = obtenerFechaByCampoHO(hoUnidad.Hophorini);
    var Hophorordarranq = OPCION_NUEVO == tipoOpcion ? '' : obtenerHoraByCampoHO(hoUnidad.Hophorordarranq);
    var Fechahorordarranq = OPCION_NUEVO == tipoOpcion ? '' : obtenerFechaByCampoHO(hoUnidad.Hophorordarranq);
    Fechahorordarranq = Fechahorordarranq != '' ? Fechahorordarranq : Fecha;
    var HoraIni = OPCION_NUEVO == tipoOpcion ? '' : obtenerHoraByCampoHO(hoUnidad.Hophorini);
    var HoraFin = OPCION_NUEVO == tipoOpcion ? '' : obtenerHoraByCampoHO(hoUnidad.Hophorfin);
    var FechaFin = obtenerFechaByCampoHO(hoUnidad.Hophorfin);
    var Hophorparada = OPCION_NUEVO == tipoOpcion ? '' : obtenerHoraByCampoHO(hoUnidad.Hophorparada);

    var htmlDiaOrdArranq = unidEsp_listarOrdArranq(Fechahorordarranq, numero);

    var idFila = 'tr_ho_esp_' + pos_row;
    var enabledInput = APP_OPCION == OPCION_VER ? 'disabled' : '';

    var disenioTabla = `
        <tr id='${idFila}'>
            <td>
                <input type="hidden" name="hopcodi" value="${hopcodi}" disabled />
                <input type="hidden" name="Hopunicodi" value="${hoUnidad.Hopunicodi}" disabled />
                <input type="hidden" name="Hopuniactivo" value="${hoUnidad.Hopuniactivo}" disabled />
                <select name="txtOrdenArranqueF" style="width:90px;" ${enabledInput}>  
                    ${htmlDiaOrdArranq}
                </select>
                <input type="Text" name="txtOrdenArranqueH" style="width:64px;" value="${Hophorordarranq}" autocomplete="off" ${enabledInput}/>
            </td>
            <td>
                <input type="text" name="txtEnParaleloF" style="width:77px;" value="${Fecha}" disabled />
                <input type="Text" name="txtEnParaleloH" style="width:64px;" value="${HoraIni}" autocomplete="off" ${enabledInput}/>
            </td>
            <td>
                <input type="text" name="txtOrdenParadaF" style="width:77px;" value="${Fecha}" disabled />
                <input type="Text" name="txtOrdenParadaH" style="width:64px;" value="${Hophorparada}" autocomplete="off" ${enabledInput}/>
            </td>
            <td>
                <input type="text" name="txtFueraParaleloF" style="width:77px;" value="${FechaFin}" disabled />
                <input type="Text" name="txtFueraParaleloH" style="width:64px;" value="${HoraFin}" autocomplete="off" ${enabledInput}/>
            </td>
    `;

    if (APP_OPCION != OPCION_VER) {
        disenioTabla += `
            <td style='text-align: center'>
                <a href='JavaScript:unidEsp_quitarHoEspecial(${equicodi},${pos_row},${numero})'>
                    <img src='${siteRoot}Content/Images/btn-cancel.png' />
                </a>
            </td>
        `;
    }
    disenioTabla += '</tr>'

    return disenioTabla;
}

function unidEsp_listarOrdArranq(Fechahorordarranq, numero) {
    var fechaHoy = $('#detalle' + numero + ' #hfFecha').val();
    var fechaAnterior = $('#detalle' + numero + ' #hfFechaAnterior').val();
    var listaFechaArranque = [];
    listaFechaArranque.push(fechaHoy);
    listaFechaArranque.push(fechaAnterior);

    var disenioTabla = '';
    listaFechaArranque.forEach(function (item) {
        var selected = "";
        if (item == Fechahorordarranq) {
            selected = "selected";
        }
        disenioTabla += `
                <option value="${item}" ${selected}> ${item}</option >
        `;
    });

    return disenioTabla;
}

function unidEsp_quitarHoEspecial(equicodi, pos, numero) {
    $('#detalle' + numero + ' #tabla_ho_esp_' + equicodi + ' #tr_ho_esp_' + pos).remove();
    var m = parseInt($('#detalle' + numero + ' #tabla_ho_esp_' + equicodi + ' tbody tr').length) || 0;

    $('#detalle' + numero + ' #' + equicodi).prop("checked", false);
    if (m > 0) {
        $('#detalle' + numero + ' #' + equicodi).prop("checked", true);
    }

    var numberOfChecked = $('#detalle' + numero + ' #tabla_ho_esp_' + equicodi + ' input:checkbox:checked').length;
    if (numberOfChecked > 0)
        $('#detalle' + numero + ' #tabla_ho_esp_' + equicodi).css('width', '983px');
    else
        $('#detalle' + numero + ' #tabla_ho_esp_' + equicodi).css('width', '776px');

    //actualizar horas
    actualizarFechasUIHoraOperacion(TIPO_HO_UNIDAD, numero);
}

function unidEsp_agregarHoEspecial(equicodi, numero) {
    var hoppadre = {};
    hoppadre = unidEsp_GetHopDefaultRegistro(numero);

    var idTable = '#detalle' + numero + ' #tabla_ho_esp_' + equicodi;
    var m = parseInt($(idTable + ' tbody tr').length) || 0;
    var htmlXUnidad = unidEsp_visualizarHoraOperacionUnidadEspecial(hoppadre, 0, equicodi, m + 1, m == 0 ? OPCION_EDITAR : OPCION_NUEVO, numero);

    $(idTable).find('tbody').append(htmlXUnidad);

    $('#detalle' + numero + ' #' + equicodi).prop("checked", true);
    unidEsp_asignarEventoPos(equicodi, m + 1, numero);

    //Mostrar deshabilitado las fechas de orden parada cuando esta activado el check
    unidEsp_HabilitacionCheckFS($('#detalle' + numero + ' #chkFueraServicio').prop('checked'), numero);
}

function unidEsp_GetHopDefaultRegistro(numero) {

    var fecha = $('#txtFecha').val();
    var fechaFin = $('#detalle' + numero + ' #txtFueraParaleloF').val(); // fecha de fuera paralello si es fin del dia 24:00:00 hs
    var equipadre = parseInt($('#detalle' + numero + ' #cbCentral2').val());
    var modoGrupo = $('#detalle' + numero + ' #cbModoOpGrupo').val(); // modo de operacion ó grupo

    var ordenArranque = $('#detalle' + numero + ' #txtOrdenArranqueH').val();
    var enParalelo = $('#detalle' + numero + ' #txtEnParaleloH').val(); // HoraIni (HH:mm:ss)
    var ordenParada = $('#detalle' + numero + ' #txtOrdenParadaH').val();
    var fueraParalelo = $('#detalle' + numero + ' #txtFueraParaleloH').val(); //HoraFin (HH:mm:ss)
    var MatrizunidadesExtra = [];
    var objModo = getModoFromListaModo(GLOBAL_HO.ListaModosOperacion, parseInt(modoGrupo));
    var flagModo = objModo != null ? objModo.FlagModoEspecial : null;

    var entity =
    {
        Hopcodi: 0,
        Hopunicodi: 0,
        Hopuniactivo: 1, //nuevo
        Hophorini: moment(convertStringToDate(fecha, enParalelo)),
        Hophorfin: moment(convertStringToDate(fechaFin, fueraParalelo)),
        Hophorordarranq: convertStringToDate(fecha, ordenArranque),
        Hophorparada: convertStringToDate(fecha, ordenParada),
        Grupocodi: modoGrupo,
        Equipadre: equipadre,
        OpcionCrud: 1, // -1: eliminar, 0:lectura, 1:crear, 2:actualizar 
        MatrizunidadesExtra: MatrizunidadesExtra,
        FlagModoEspecial: flagModo
    };

    return entity;
}

function unidEsp_asignarEventoPos(equicodi, pos_row, numero) {
    var idFila = '#detalle' + numero + ' #unidadEspecial_' + equicodi + " " + '#tr_ho_esp_' + pos_row;

    var id_txtOrdenArranqueF = idFila + " " + 'select[name=txtOrdenArranqueF]';
    var id_txtFueraParaleloF = idFila + " " + 'input[name=txtFueraParaleloF]';
    var id_txtOrdenArranqueH = idFila + " " + 'input[name=txtOrdenArranqueH]';
    var id_txtEnParaleloH = idFila + " " + 'input[name=txtEnParaleloH]';
    var id_txtOrdenParadaH = idFila + " " + 'input[name=txtOrdenParadaH]';
    var id_txtFueraParaleloH = idFila + " " + 'input[name=txtFueraParaleloH]';

    ui_setInputmaskHora(id_txtOrdenArranqueH);
    ui_setInputmaskHora(id_txtEnParaleloH);
    ui_setInputmaskHora(id_txtOrdenParadaH);
    ui_setInputmaskHora(id_txtFueraParaleloH);
    $(id_txtOrdenArranqueF).on('change', function (e) { actualizarFechasUIHoraOperacion(TIPO_HO_UNIDAD, numero) });
    $(id_txtOrdenArranqueH).on('focusout', function (e) { actualizarFechasUIHoraOperacion(TIPO_HO_UNIDAD, numero) });
    $(id_txtEnParaleloH).on('focusout', function (e) { actualizarFechasUIHoraOperacion(TIPO_HO_UNIDAD, numero) });
    $(id_txtOrdenParadaH).on('focusout', function (e) { actualizarFechasUIHoraOperacion(TIPO_HO_UNIDAD, numero) });

    $(id_txtFueraParaleloH).on('focusout', function (e) { actualizartxtFueraParaleloF(id_txtFueraParaleloH, id_txtFueraParaleloF, numero); actualizarFechasUIHoraOperacion(TIPO_HO_UNIDAD, numero) });
}

function unidEsp_listarHop(numero) {
    var MatrizunidadesExtra = [];
    $('#detalle' + numero + ' input[name=chkUnidades]').each(function () {
        var entity = {
            Emprcodi: $('#detalle' + numero + ' #cbEmpresa2').val(),
            Equicodi: this.id, // unidad del modo de operacion
            ListaHo: []
        }
        $('#detalle' + numero + ' #tabla_ho_esp_' + entity.Equicodi + " tbody").find('tr').each(function () {
            var idFila = '#detalle' + numero + ' #unidadEspecial_' + entity.Equicodi + " #" + $(this).get()[0].id;

            var id_txtOrdenArranqueH = idFila + " " + 'input[name=txtOrdenArranqueH]';
            var id_txtEnParaleloH = idFila + " " + 'input[name=txtEnParaleloH]';
            var id_txtOrdenParadaH = idFila + " " + 'input[name=txtOrdenParadaH]';
            var id_txtFueraParaleloH = idFila + " " + 'input[name=txtFueraParaleloH]';

            var hopcodi = ($(this).find("input[name='hopcodi']").first()).val();
            var Hopunicodi = ($(this).find("input[name='Hopunicodi']").first()).val();
            var Hopuniactivo = ($(this).find("input[name='Hopuniactivo']").first()).val();

            $(id_txtOrdenArranqueH).val(obtenerHoraValida($(id_txtOrdenArranqueH).val()));
            $(id_txtEnParaleloH).val(obtenerHoraValida($(id_txtEnParaleloH).val()));
            $(id_txtOrdenParadaH).val(obtenerHoraValida($(id_txtOrdenParadaH).val()));
            $(id_txtFueraParaleloH).val(obtenerHoraValida($(id_txtFueraParaleloH).val()));

            var txtOrdenArranqueF = ($(this).find("select[name='txtOrdenArranqueF']").first()).val();
            var txtOrdenArranqueH = ($(this).find("input[name='txtOrdenArranqueH']").first()).val();

            var txtEnParaleloF = ($(this).find("input[name='txtEnParaleloF']").first()).val();
            var txtEnParaleloH = ($(this).find("input[name='txtEnParaleloH']").first()).val();

            var txtOrdenParadaF = ($(this).find("input[name='txtOrdenParadaF']").first()).val();
            var txtOrdenParadaH = ($(this).find("input[name='txtOrdenParadaH']").first()).val();

            var txtFueraParaleloF = ($(this).find("input[name='txtFueraParaleloF']").first()).val();
            var txtFueraParaleloH = ($(this).find("input[name='txtFueraParaleloH']").first()).val();

            var fOrdenArranque = convertStringToDate(txtOrdenArranqueF, txtOrdenArranqueH);
            var fEnParalelo = convertStringToDate(txtEnParaleloF, txtEnParaleloH);
            var fOrdenParada = convertStringToDate(txtOrdenParadaF, txtOrdenParadaH);
            var fFueraParalelo = convertStringToDate(txtFueraParaleloF, txtFueraParaleloH);

            var hopEsp = {};
            hopEsp.Hopunicodi = Hopunicodi;
            hopEsp.Hopuniactivo = Hopuniactivo;
            hopEsp.Hopcodi = hopcodi;
            hopEsp.Emprcodi = entity.Emprcodi;
            hopEsp.Equicodi = entity.Equicodi;
            hopEsp.Hophorini = fEnParalelo;
            hopEsp.Hophorfin = fFueraParalelo;
            hopEsp.Hophorordarranq = fOrdenArranque;
            hopEsp.Hophorparada = fOrdenParada;
            hopEsp.RefIdFila = '#unidadEspecial_' + entity.Equicodi + " #" + $(this).get()[0].id;

            hopEsp.ordenArranqueF = txtOrdenArranqueF;
            hopEsp.ordenArranque = fOrdenArranque != '' ? txtOrdenArranqueH : '';

            hopEsp.enParaleloF = txtEnParaleloF; // HoraIni (HH:mm:ss) 
            hopEsp.enParalelo = txtEnParaleloH; // HoraIni (HH:mm:ss)  

            hopEsp.ordenParadaF = txtOrdenParadaF;
            hopEsp.ordenParada = fOrdenParada != '' ? txtOrdenParadaH : fOrdenParada;

            hopEsp.fueraParaleloF = txtFueraParaleloF; //HoraFin (HH:mm:ss)
            hopEsp.fueraParalelo = txtFueraParaleloH; //HoraFin (HH:mm:ss)

            entity.ListaHo.push(hopEsp);
        });

        MatrizunidadesExtra.push(entity)
    });

    return MatrizunidadesExtra;
}

function unidEsp_listarHopcodiAEliminar(listaHOMem, listaHOTbl) {
    var listaCodi = [];
    for (var i = 0; i < listaHOMem.length; i++) {
        var bEliminar = true;

        for (var j = 0; j < listaHOTbl.length; j++) {
            var HopunicodiTmp = parseInt(listaHOTbl[j].Hopunicodi) || 0;
            if (listaHOMem[i].Hopunicodi == HopunicodiTmp && HopunicodiTmp != 0) {
                bEliminar = false;
            }
        }

        if (bEliminar) {
            listaCodi.push(listaHOMem[i].Hopunicodi);
        }
    }

    return listaCodi;
}

function unidEsp_validarTablaUnidEspeciales(listaUnidades, objRefPadre, numero) {

    /////////////////////////////////////////////////////////////////////////////////////////////
    //Validar cada fila de Cada Unidad
    /////////////////////////////////////////////////////////////////////////////////////////////
    var msj = unidEsp_validarFilaTabla(listaUnidades, numero);
    if (msj != '') {
        return msj;
    }

    /////////////////////////////////////////////////////////////////////////////////////////////
    //Actualizar campos de Fecha del Formulario de la Hora de Operación modo
    /////////////////////////////////////////////////////////////////////////////////////////////
    unidEsp_actualizarUIFechasModo(listaUnidades, objRefPadre, numero);

    return '';
}

function unidEsp_validarFilaTabla(listaUnidades, numero) {

    var listaHo = [];
    if (listaUnidades.length > 0) {
        for (var i = 0; i < listaUnidades.length; i++) {
            var listaHoByUnid = listaUnidades[i].ListaHo;
            listaHo = listaHo.concat(listaHoByUnid);
            for (var j = 0; j < listaHoByUnid.length; j++) {
                var objHo = listaHoByUnid[j];

                var idFila = objHo.RefIdFila;
                var id_txtOrdenArranqueH = idFila + " " + 'input[name=txtOrdenArranqueH]';
                var id_txtEnParaleloH = idFila + " " + 'input[name=txtEnParaleloH]';
                var id_txtOrdenParadaH = idFila + " " + 'input[name=txtOrdenParadaH]';
                var id_txtFueraParaleloH = idFila + " " + 'input[name=txtFueraParaleloH]';

                var id_txtOrdenArranqueF = idFila + " " + 'select[name=txtOrdenArranqueF]';
                var id_txtEnParaleloF = idFila + " " + 'input[name=txtEnParaleloF]';
                var id_txtOrdenParadaF = idFila + " " + 'input[name=txtOrdenParadaF]';
                var id_txtFueraParaleloF = idFila + " " + 'input[name=txtFueraParaleloF]';

                var objReferencia = {
                    id_ref_txtOrdenArranqueH: id_txtOrdenArranqueH,
                    id_ref_txtEnParaleloH: id_txtEnParaleloH,
                    id_ref_txtOrdenParadaH: id_txtOrdenParadaH,
                    id_ref_txtFueraParaleloH: id_txtFueraParaleloH,
                    id_ref_txtOrdenParadaF: id_txtOrdenParadaF,
                    id_ref_txtFueraParaleloF: id_txtFueraParaleloF,
                    id_ref_txtOrdenArranqueF: id_txtOrdenArranqueF,
                    id_ref_txtEnParaleloF: id_txtEnParaleloF,
                };

                var msjValidacionHoUnidad = ho_validarHoraOperacion(TIPO_HO_UNIDAD, objReferencia, null, null, null, numero);
                if (msjValidacionHoUnidad != '') {
                    return msjValidacionHoUnidad;
                }
            }
        }

        if (listaHo.length == 0) {
            return MSJ_VAL_MODO_SIN_UNIDAD;
        }
    } else {
        return MSJ_VAL_MODO_SIN_UNIDAD;
    }

    return '';
}

function unidEsp_actualizarUIFechasModo(listaUnidades, objRefPadre, numero) {
    var listaHo = [];
    if (listaUnidades.length > 0) {
        for (var i = 0; i < listaUnidades.length; i++) {
            var listaHoByUnid = listaUnidades[i].ListaHo;
            listaHo = listaHo.concat(listaHoByUnid);
        }
    }

    ordenarListaHorasOperacion(listaHo);

    //
    var arrDateIni = [];
    for (var i = 0; i < listaHo.length; i++) {
        var dt = moment(listaHo[i].Hophorini).toDate();
        if (val_esValidoDate(dt))
            arrDateIni.push(dt);
    }
    arrDateIni = arrDateIni.sort(function (a, b) { return a.getTime() - b.getTime() });
    arrDateIni.filter((date, i, self) =>
        self.findIndex(d => d.getTime() === date.getTime()) === i
    );

    //
    var arrDateFin = [];
    for (var i = 0; i < listaHo.length; i++) {
        var dt = moment(listaHo[i].Hophorfin).toDate();
        if (val_esValidoDate(dt))
            arrDateFin.push(dt);
    }
    arrDateFin = arrDateFin.sort(function (a, b) { return a.getTime() - b.getTime() });
    arrDateFin.filter((date, i, self) =>
        self.findIndex(d => d.getTime() === date.getTime()) === i
    );

    //
    var arrDateArranq = [];
    for (var i = 0; i < listaHo.length; i++) {
        var dt = moment(listaHo[i].Hophorordarranq).toDate();
        if (val_esValidoDate(dt))
            arrDateArranq.push(dt);
    }
    arrDateArranq = arrDateArranq.sort(function (a, b) { return a.getTime() - b.getTime() });
    arrDateArranq.filter((date, i, self) =>
        self.findIndex(d => d.getTime() === date.getTime()) === i
    );

    //
    var arrDateParada = [];
    for (var i = 0; i < listaHo.length; i++) {
        var dt = moment(listaHo[i].Hophorparada).toDate();
        if (val_esValidoDate(dt))
            arrDateParada.push(dt);
    }
    arrDateParada = arrDateParada.sort(function (a, b) { return a.getTime() - b.getTime() });
    arrDateParada.filter((date, i, self) =>
        self.findIndex(d => d.getTime() === date.getTime()) === i
    );

    var strDateIniH = arrDateIni.length > 0 ? moment(arrDateIni[0]).format('HH:mm:ss') : '';
    var strDateFinH = arrDateFin.length > 0 ? moment(arrDateFin[arrDateFin.length - 1]).format('HH:mm:ss') : '';
    var strDateArranqH = arrDateArranq.length > 0 ? moment(arrDateArranq[0]).format('HH:mm:ss') : '';
    var strDateParadaH = arrDateParada.length > 0 ? moment(arrDateParada[arrDateParada.length - 1]).format('HH:mm:ss') : '';

    var fechaHoy = $('#detalle' + numero + ' #hfFecha').val();
    var strDateIniF = strDateIniH != '' ? moment(arrDateIni[0]).format('DD/MM/YYYY') : fechaHoy;
    var strDateFinF = strDateFinH != '' ? moment(arrDateFin[arrDateFin.length - 1]).format('DD/MM/YYYY') : fechaHoy;
    var strDateArranqF = strDateArranqH != '' ? moment(arrDateArranq[0]).format('DD/MM/YYYY') : fechaHoy;
    var strDateParadaF = strDateParadaH != '' ? moment(arrDateParada[arrDateParada.length - 1]).format('DD/MM/YYYY') : fechaHoy;

    $('#detalle' + numero + ' ' + objRefPadre.id_ref_txtOrdenArranqueH).val(obtenerHoraValida(strDateArranqH));
    $('#detalle' + numero + ' ' + objRefPadre.id_ref_txtEnParaleloH).val(obtenerHoraValida(strDateIniH));
    $('#detalle' + numero + ' ' + objRefPadre.id_ref_txtOrdenParadaH).val(obtenerHoraValida(strDateParadaH));
    $('#detalle' + numero + ' ' + objRefPadre.id_ref_txtFueraParaleloH).val(obtenerHoraValida(strDateFinH));
    $('#detalle' + numero + ' ' + objRefPadre.id_ref_txtOrdenArranqueF).val(obtenerHoraValida(strDateArranqF));
    $('#detalle' + numero + ' ' + objRefPadre.id_ref_txtEnParaleloF).val(obtenerHoraValida(strDateIniF));
    $('#detalle' + numero + ' ' + objRefPadre.id_ref_txtOrdenParadaF).val(obtenerHoraValida(strDateParadaF));
    $('#detalle' + numero + ' ' + objRefPadre.id_ref_txtFueraParaleloF).val(obtenerHoraValida(strDateFinF));
}

function unidEsp_HabilitacionCheckFS(value, numero) {

    var listaUnidades = unidEsp_listarHop(numero);

    if (listaUnidades != null && listaUnidades.length > 0) {
        for (var i = 0; i < listaUnidades.length; i++) {
            var listaHo = listaUnidades[i].ListaHo;
            if (listaHo.length > 0) {
                for (var j = 0; j < listaHo.length; j++) {
                    if (value) {
                        $(listaHo[j].RefIdFila + ' select[name=txtOrdenArranqueF]').prop('disabled', 'disabled');
                        $(listaHo[j].RefIdFila + ' input[name=txtOrdenArranqueH]').prop('disabled', 'disabled');
                        $(listaHo[j].RefIdFila + ' input[name=txtOrdenParadaH]').prop('disabled', 'disabled');
                        $(listaHo[j].RefIdFila + ' input[name=txtOrdenParadaF]').val('');
                        $(listaHo[j].RefIdFila + ' input[name=txtOrdenParadaH]').val('');
                    } else {
                        $(listaHo[j].RefIdFila + ' select[name=txtOrdenArranqueF]').removeAttr("disabled");
                        $(listaHo[j].RefIdFila + ' input[name=txtOrdenParadaF]').val($("#txtFecha").val());
                        $(listaHo[j].RefIdFila + ' input[name=txtOrdenArranqueH]').removeAttr("disabled");
                        $(listaHo[j].RefIdFila + ' input[name=txtOrdenParadaH]').removeAttr("disabled");
                    }
                }
            }
        }
        if (value) {
            $('#detalle' + numero + ' #txtOrdenParadaH').val("");
            $('#detalle' + numero + ' #txtOrdenParadaF').val("");
        }

        $('#detalle' + numero + ' #txtOrdenArranqueH').prop('disabled', true);
        $('#detalle' + numero + ' #txtOrdenParadaH').prop('disabled', true);

    }
}

///////////////////////////////////////////////////////////////////////////////////////////////////
/// Desglose
///////////////////////////////////////////////////////////////////////////////////////////////////

function desg_actualizarTablaDesglose(numero, listaDesg) {

    if (APP_OPCION != OPCION_VER) {
        $('#detalle' + numero + ' #btnAgregarDesglose').show();
    }

    $('#detalle' + numero + ' .desglose').hide();
    if (listaDesg.length > 0) {
        var html = desg_generarTablaDesglose(listaDesg, APP_OPCION, numero);

        $('#detalle' + numero + ' #tablaDesglose').html(html);
        $('#detalle' + numero + ' .desglose').show();
    } else {
        $('#detalle' + numero + ' #tablaDesglose').html('Sin registros');
    }

    $('#detalle' + numero + ' #desg_form_cbTipo').removeAttr("disabled");

    //valor defecto nuevo desglose
    desg_valorDefectoNuevoDesglose(numero, listaDesg);
}

function desg_valorDefectoNuevoDesglose(numero, listaDesg) {

    //luego de agregar un desglose setear valores por defecto
    if (listaDesg.length > 0) {
        var txtIni = listaDesg[listaDesg.length - 1].HoraFin;
        var txtFin = $('#detalle' + numero + ' #txtFueraParaleloH').val();

        $('#detalle' + numero + ' #desg_form_txtIniH').val(txtIni);
        $('#detalle' + numero + ' #desg_form_txtFinH').val(txtFin);

    } else {
        var txtIni = $('#detalle' + numero + ' #txtEnParaleloH').val();
        var txtFin = $('#detalle' + numero + ' #txtFueraParaleloH').val();

        $('#detalle' + numero + ' #desg_form_txtIniH').val(txtIni);
        $('#detalle' + numero + ' #desg_form_txtFinH').val(txtFin);
    }
    $('#detalle' + numero + ' #desg_form_valor').val("0");
}

function desg_formulario_Desglose(pos_row, numero) {

    var tipoOpcion = (parseInt(pos_row) || 0) > 0 ? OPCION_EDITAR : OPCION_NUEVO;
    var fecha = $('#detalle' + numero + ' #hfFecha').val();

    var tipoDesg = DESG_CODI_PLENA_CARGA;
    //datos a nivel de la hora
    var fechaIni = $('#detalle' + numero + ' #txtEnParaleloF').val();
    var fechaFin = $('#detalle' + numero + ' #txtFueraParaleloF').val();
    var horaIni = $('#detalle' + numero + ' #txtEnParaleloH').val();
    var horaFin = $('#detalle' + numero + ' #txtFueraParaleloH').val();
    var valor = 0;

    var form = "#form_desglose_" + numero + " ";

    var htmlRangoValido = 'El rango debe estar entre <b style="color:blue">' + fechaIni + " " + horaIni + '</b> y <b style="color: red">' + fechaFin + " " + horaFin + "</b>";
    htmlRangoValido += '<input type="hidden" id="idNumeroDesgloseDiv" name="idNumeroDesgloseDiv" value="' + numero + '" />';
    $("#desc_ho_rangos_permitido_desglose").html(htmlRangoValido);
    $("#desc_ho_rangos_permitido_desglose_" + numero).html(htmlRangoValido);

    $(form + "#desg_form_cbTipo").removeAttr("disabled");

    //edición de un registro
    if (OPCION_EDITAR == tipoOpcion) {
        var idFila = '#tr_desgl_' + numero + '_' + pos_row;
        var id_tipoDesg = idFila + " " + 'input[name=fila_tipoDesg]';
        var id_fila_desg_txtIni_F = idFila + " " + 'input[name=fila_desg_txtIni_F]';
        var id_fila_desg_txtIni_H = idFila + " " + 'input[name=fila_desg_txtIni_H]';
        var id_fila_desg_txtFin_F = idFila + " " + 'input[name=fila_desg_txtFin_F]';
        var id_fila_desg_txtFin_H = idFila + " " + 'input[name=fila_desg_txtFin_H]';
        var id_fila_desg_valor = idFila + " " + 'input[name=fila_desg_valor]';

        tipoDesg = parseInt($(id_tipoDesg).val()) || 0;
        fechaIni = $(id_fila_desg_txtIni_F).val();
        fechaFin = $(id_fila_desg_txtFin_F).val();
        horaIni = $(id_fila_desg_txtIni_H).val();
        horaFin = $(id_fila_desg_txtFin_H).val();
        valor = $(id_fila_desg_valor).val();

        //inicializar datos de la fila seleccionada
        $(form + '#desg_form_accion').val(tipoOpcion);
        $(form + '#desg_form_cbTipo').val(tipoDesg);
        $(form + '#desg_form_fila').val(parseInt(pos_row) || 0);
        $(form + '#desg_form_txtIniH').val(horaIni);
        $(form + '#desg_form_txtFinH').val(horaFin);
        $(form + '#desg_form_valor').val(valor);
    } else {

    }

    var id_txtIniH = form + "#desg_form_txtIniH";
    var id_txtFinH = form + "#desg_form_txtFinH";

    $(id_txtIniH).unbind();
    $(id_txtFinH).unbind();

    ui_setInputmaskHora(form + "#desg_form_txtIniH");
    ui_setInputmaskHora(form + "#desg_form_txtFinH");

    $(id_txtIniH).on('focusout', function (e) { desg_asignarEvento(numero); });
    $(id_txtFinH).on('focusout', function (e) { desg_asignarEvento(numero); });

    $(id_txtIniH).focusout(function () {
        $(this).val(obtenerHoraValida($(this).val()));
    });

    $(id_txtFinH).focusout(function () {
        $(this).val(obtenerHoraValida($(this).val()));
    });
}

function desg_asignarEvento(numero) {
    var form = "#detalle" + numero + " ";

    var id_txtIniH = form + "#desg_form_txtIniH";
    var id_txtFinH = form + "#desg_form_txtFinH";

    $(id_txtIniH).val(obtenerHoraValida($(id_txtIniH).val()));
    $(id_txtFinH).val(obtenerHoraValida($(id_txtFinH).val()));
}

function desg_obtener_ObjForm(numero) {

    var form = "#form_desglose_" + numero + ' ';

    var pos_row = parseInt($(form + '#desg_form_fila').val()) || 0;

    var tipoDesg = parseInt($(form + '#desg_form_cbTipo').val()) || 0;

    var horaIni = $(form + '#desg_form_txtIniH').val();
    var horaFin = $(form + '#desg_form_txtFinH').val();

    var fechaIni = $('#detalle' + numero + ' #txtEnParaleloF').val();
    var fechaFin = fechaIni;
    if (horaFin == "00:00:00") {
        fechaFin = obtenerDiaSiguiente(fechaIni);
    }

    var valor = parseFloat($(form + '#desg_form_valor').val()) || 0;
    $(form + '#desg_form_valor').val(valor);

    var Ichorini = convertStringToDate(fechaIni, horaIni);
    var Ichorfin = convertStringToDate(fechaFin, horaFin);
    var objDesg =
    {
        TipoDesglose: tipoDesg,
        Ichorini: Ichorini,
        Ichorfin: Ichorfin,
        FechaIni: fechaIni,
        HoraIni: horaIni,
        FechaFin: fechaFin,
        HoraFin: horaFin,
        Icvalor1: valor,
        Fila: pos_row
    };

    return objDesg;
}

function desg_generarTablaDesglose(listaDesg, tipoOpcion, numero) {
    var fecha = $('#hfFecha').val();

    //Generar Tabla html
    var html = 'Sin registros';
    if (listaDesg.length > 0) {
        html = `
        <table class='pretty tabla-icono tabla_ho_desg' id='tabla_ho_desg' style='margin-top: 5px;    margin-bottom: 5px;width; auto; max-width: 460px;'>
            <thead>
                <tr>
                    <th>Registrados</th>
                    <th>Hora Inicio</th>
                    <th>Hora Fin</th>
                    <th>Valor</th>`
            ;
        if (OPCION_VER != tipoOpcion) {
            html += "<th></th>";
        }

        html += `
                </tr>
            </thead>
            <tbody>
        `;

        listaDesg = desg_ordenarLista(listaDesg);
        var pos_row = 1;
        listaDesg.forEach(function (objDesg) {
            var idFila = 'tr_desgl_' + numero + '_' + pos_row;
            var descTipo = desgl_NombreTipoDesglose(objDesg.TipoDesglose);
            var codigoTipo = objDesg.TipoDesglose;

            var F_horini = objDesg.FechaIni;
            F_horini = F_horini != '' ? F_horini : fecha;
            var F_horfin = objDesg.FechaFin;
            F_horfin = F_horfin != '' ? F_horfin : fecha;

            var H_horini = objDesg.HoraIni;
            var H_horfin = objDesg.HoraFin;

            var valorDesg = objDesg.Icvalor1;
            html += `
                <tr id="${idFila}">
                    <td>${descTipo}</td>
                    <td>
                        <input type="hidden" class='fila_desg' name="fila_desg_pos" style="width:77px;" value="${pos_row}"  />
                        <input type="hidden" class='fila_desg' name="fila_tipoDesg" style="width:77px;" value="${codigoTipo}"  />
                        <input type="text" class='fila_desg' name="fila_desg_txtIni_F" style="width:77px;display:none;" value="${F_horini}" />
                        <input type="Text" class='fila_desg' name="fila_desg_txtIni_H" style="width:62px;" value="${H_horini}" disabled />
                    </td>                                          
                    <td>                                           
                        <input type="text" class='fila_desg' name="fila_desg_txtFin_F" style="width:77px;display:none;" value="${F_horfin}" disabled />
                        <input type="Text" class='fila_desg' name="fila_desg_txtFin_H" style="width:62px;" value="${H_horfin}" disabled />
                    </td>
                    <td>
                        <input type="text" class='fila_desg' name="fila_desg_valor" style="width:46px; text-align: center;" value="${valorDesg}" disabled />
                    </td>
                `;
            if (OPCION_VER != tipoOpcion) {
                html += `
                    <td style='text-align: center'>
                        <a href='JavaScript:desg_editarFila(${pos_row},${numero})'>
                            <img src='${siteRoot}Content/Images/btn-edit.png' title='Editar fila' />
                        </a>
                        <a href='JavaScript:desg_quitarFila(${pos_row},${numero})'>
                            <img src='${siteRoot}Content/Images/btn-cancel.png' title='Eliminar fila' />
                        </a>
                    </td>
                    `;
            }
            html += `
                </tr>
            `;

            pos_row++;
        });

        html += `
            </tbody>
        </table>
        `;
    }

    return html;
}

function desg_editarFila(pos_row, numero) {
    desg_formulario_Desglose(pos_row, numero);

    $('#detalle' + numero + ' #tabla_ho_desg #tr_desgl_' + numero + '_' + pos_row).remove();
}

function desg_quitarFila(pos_row, numero) {

    $('#detalle' + numero + ' #tabla_ho_desg #tr_desgl_' + numero + '_' + pos_row).remove();

    //luego de actualizar el modo, actualizar el desglose
    try {
        var listaDesglose = desg_generarListaDesgFromHtml(numero);
        desg_actualizarTablaDesglose(numero, listaDesglose);
    } catch (error) {
    }
}

function desg_guardarDesglose(numero) {

    var msj = '';
    var objDesg = desg_obtener_ObjForm(numero);

    //El objeto solo debe tener filas válidas
    var existeFechaValida = val_esValidoDate(moment(objDesg.Ichorini).toDate()) && val_esValidoDate(moment(objDesg.Ichorfin).toDate());

    if (existeFechaValida) {
        objDesg.Ichorini = moment(objDesg.Ichorini);
        objDesg.Ichorfin = moment(objDesg.Ichorfin);

        if (objDesg.Ichorini.isSameOrAfter(objDesg.Ichorfin)) {
            msj += desgl_NombreTipoDesglose(objDesg.TipoDesglose) + DESG_VAL_RANGO_NO_VALIDO + "\n";
        }
    } else {
        msj += desgl_NombreTipoDesglose(objDesg.TipoDesglose) + ": " + DESG_VAL_INCOMPLETO + "\n";
    }

    if (msj.length > 0) {
        alert(msj)
        return false;
    }

    var listaDesg = desg_generarListaDesgFromHtml(numero);
    listaDesg.push(objDesg);

    //Validación
    var msjVal = desg_validarData(listaDesg, -1, GLOBAL_HO.ListaHorasOperacion, numero);
    if (msjVal == "") {
        $('#popupAgregarDesglose').bPopup().close();
        desg_actualizarTablaDesglose(numero, listaDesg);
    }
    else {
        alert(msjVal);
    }
}

function desg_ajustarLista(listaHoraDesg, pos, listaHo, numero) {
    var hopmodo = {};
    if (pos > -1) { //Edición de Hora de Operación
        hopmodo = listaHo[pos];
    } else { //Registro de Hora de Operación        
        hopmodo = unidEsp_GetHopDefaultRegistro(numero);
    }
    var horaIni = moment(hopmodo.Hophorini);
    var horaFin = moment(hopmodo.Hophorfin);

    var listaHoraDesgAjust = [];
    for (var i = 0; i < listaHoraDesg.length; i++) {
        var dt1 = moment(listaHoraDesg[i].Ichorini).toDate();
        var dt2 = moment(listaHoraDesg[i].Ichorfin).toDate();

        if (val_esValidoDate(dt1) && val_esValidoDate(dt2)) {

            //Desglose incluidos en la hora de operacion
            if ((moment(listaHoraDesg[i].Ichorfin).isAfter(horaIni) && moment(listaHoraDesg[i].Ichorini).isBefore(horaFin))
                || (moment(listaHoraDesg[i].Ichorini).isSameOrAfter(horaIni) && moment(listaHoraDesg[i].Ichorfin).isSameOrBefore(horaFin))) {

                if (moment(listaHoraDesg[i].Ichorini).isBefore(horaIni)) {
                    listaHoraDesg[i].Ichorini = horaIni;
                    listaHoraDesg[i].FechaIni = obtenerFechaByCampoHO(horaIni);
                    listaHoraDesg[i].HoraIni = obtenerHoraByCampoHO(horaIni);
                }

                if (moment(listaHoraDesg[i].Ichorfin).isAfter(horaFin)) {
                    listaHoraDesg[i].Ichorfin = horaFin;
                    listaHoraDesg[i].FechaFin = obtenerFechaByCampoHO(horaFin);
                    listaHoraDesg[i].HoraFin = obtenerHoraByCampoHO(horaFin);
                }

                listaHoraDesgAjust.push(listaHoraDesg[i]);
            }
        }
    }

    listaHoraDesg = listaHoraDesgAjust;
    desg_actualizarTablaDesglose(numero, listaHoraDesg);

    return listaHoraDesg;
}

function desg_validarData(listaHoraDesg, pos, listaHo, numero) {
    pos = parseInt(pos);
    var msj = '';
    if (listaHoraDesg == null || $.isEmptyObject(listaHoraDesg) || listaHoraDesg.length == 0) {
        return msj;
    }

    if (listaHoraDesg.length > 0) {

        //Validación de cruce
        msj = desg_validarCruceDesglose(listaHoraDesg, listaHo);

        if (msj.length > 0)
            return msj;

        var hopmodo = {};
        if (pos > -1) { //Edición de Hora de Operación
            hopmodo = listaHo[pos];
        } else { //Registro de Hora de Operación        
            hopmodo = unidEsp_GetHopDefaultRegistro(numero);
        }
        var horaIni = moment(hopmodo.Hophorini);
        var horaFin = moment(hopmodo.Hophorfin);

        if (listaHoraDesg.length > 0) {
            //Validación si el desglose esta dentro de la hora de operación
            var arrDateIni = [];
            for (var i = 0; i < listaHoraDesg.length; i++) {
                var dt = moment(listaHoraDesg[i].Ichorini).toDate();
                if (val_esValidoDate(dt))
                    arrDateIni.push(dt);
            }
            arrDateIni = arrDateIni.sort(function (a, b) { return a.getTime() - b.getTime() });

            var arrDateFin = [];
            for (var i = 0; i < listaHoraDesg.length; i++) {
                var dt = moment(listaHoraDesg[i].Ichorfin).toDate();
                if (val_esValidoDate(dt))
                    arrDateFin.push(dt);
            }
            arrDateFin = arrDateFin.sort(function (a, b) { return a.getTime() - b.getTime() });

            //Validación desglose debe estar incluido en la hora de operación
            var dateFrom = moment(arrDateIni[0]);
            var dateTo = moment(arrDateFin[arrDateFin.length - 1]);

            if (moment(dateFrom).isBefore(horaIni))
                return DESG_VAL_INCLUIDO;
            if (moment(dateTo).isAfter(horaFin))
                return DESG_VAL_INCLUIDO;
        }
    }

    return msj;
}

function desg_generarListaDesgFromHtml(numero) {
    var listaDesg = [];

    $('#detalle' + numero + ' #tabla_ho_desg tbody').find('tr').each(function () {
        var idFila = " #" + $(this).get()[0].id;

        var id_pos_row = idFila + " " + 'input[name=fila_desg_pos]';
        var id_tipoDesg = idFila + " " + 'input[name=fila_tipoDesg]';
        var id_fila_desg_txtIni_F = idFila + " " + 'input[name=fila_desg_txtIni_F]';
        var id_fila_desg_txtIni_H = idFila + " " + 'input[name=fila_desg_txtIni_H]';
        var id_fila_desg_txtFin_F = idFila + " " + 'input[name=fila_desg_txtFin_F]';
        var id_fila_desg_txtFin_H = idFila + " " + 'input[name=fila_desg_txtFin_H]';
        var id_fila_desg_valor = idFila + " " + 'input[name=fila_desg_valor]';

        var pos_row = parseInt($(id_pos_row).val()) || 0;
        var tipoDesg = parseInt($(id_tipoDesg).val()) || 0;
        var fechaIni = $(id_fila_desg_txtIni_F).val();
        var fechaFin = $(id_fila_desg_txtFin_F).val();
        var horaIni = $(id_fila_desg_txtIni_H).val();
        var horaFin = $(id_fila_desg_txtFin_H).val();
        var valor = $(id_fila_desg_valor).val();

        var Ichorini = convertStringToDate(fechaIni, horaIni);
        var Ichorfin = convertStringToDate(fechaFin, horaFin);

        var objDesg =
        {
            TipoDesglose: tipoDesg,
            Ichorini: moment(Ichorini),
            Ichorfin: moment(Ichorfin),
            FechaIni: fechaIni,
            HoraIni: horaIni,
            FechaFin: fechaFin,
            HoraFin: horaFin,
            Icvalor1: valor,
            Fila: pos_row
        };

        listaDesg.push(objDesg);
    });

    return listaDesg;
}

function desg_obtenerListaDesgloseValida(listaDesg) {
    if (listaDesg == null)
        return [];

    var listaFinal = [];
    listaDesg.forEach(function (objDesg) {
        var existeDesg = val_esValidoDate(objDesg.Ichorini) && val_esValidoDate(objDesg.Ichorfin);
        if (existeDesg) {
            objDesg.Ichorini = moment(objDesg.Ichorini);
            objDesg.Ichorfin = moment(objDesg.Ichorfin);
            listaFinal.push(objDesg);
        }
    });

    return listaFinal;
}

// Ordenar lista de Desglose
function desg_ordenarLista(listaDesg) {

    for (var i = 0; i < listaDesg.length - 1; i++) {
        for (var j = 0; j < listaDesg.length - 1; j++) {
            var timeIni = moment(listaDesg[j].Ichorini).toDate().getTime();
            var timeSig = moment(listaDesg[j + 1].Ichorini).toDate().getTime();

            if (timeIni > timeSig) {
                var tmp = listaDesg[j + 1];
                listaDesg[j + 1] = listaDesg[j];
                listaDesg[j] = tmp;
            }
        }
    }

    return listaDesg;
}

function desg_obtenerUltimoRegParaCopiarHO(listaHoraDesg, hopmodo) {
    if (listaHoraDesg != null && listaHoraDesg.length > 0 && hopmodo != null) {
        var ultimoRegDes = listaHoraDesg[listaHoraDesg.length - 1];

        var horaFinDesg = moment(ultimoRegDes.Ichorfin);
        var horaFinHO = moment(hopmodo.Hophorfin);

        if (moment(horaFinDesg).isSame(horaFinHO))
            return ultimoRegDes;
    }

    return null;
}

function desg_ajustarListaDesglose(listaHoTmp) {

    for (var j = 0; j < listaHoTmp.length; j++) {
        var ListaDesglose = [];
        ListaDesglose = listaHoTmp[j].ListaDesglose;
        var horaIni = moment(listaHoTmp[j].Hophorini);
        var horaFin = moment(listaHoTmp[j].Hophorfin);

        var listaHoraDesgAjust = [];

        for (var i = 0; i < ListaDesglose.length; i++) {
            var dt1 = moment(ListaDesglose[i].Ichorini).toDate();
            var dt2 = moment(ListaDesglose[i].Ichorfin).toDate();

            if (val_esValidoDate(dt1) && val_esValidoDate(dt2)) {

                //Desglose incluidos en la hora de operacion
                if ((moment(ListaDesglose[i].Ichorfin).isAfter(horaIni) && moment(ListaDesglose[i].Ichorini).isBefore(horaFin))
                    || (moment(ListaDesglose[i].Ichorini).isSameOrAfter(horaIni) && moment(ListaDesglose[i].Ichorfin).isSameOrBefore(horaFin))) {

                    if (moment(ListaDesglose[i].Ichorini).isBefore(horaIni)) {
                        ListaDesglose[i].Ichorini = horaIni;
                        ListaDesglose[i].FechaIni = obtenerFechaByCampoHO(horaIni);
                        ListaDesglose[i].HoraIni = obtenerHoraByCampoHO(horaIni);
                    }

                    if (moment(ListaDesglose[i].Ichorfin).isAfter(horaFin)) {
                        ListaDesglose[i].Ichorfin = horaFin;
                        ListaDesglose[i].FechaFin = obtenerFechaByCampoHO(horaFin);
                        ListaDesglose[i].HoraFin = obtenerHoraByCampoHO(horaFin);
                    }

                    listaHoraDesgAjust.push(ListaDesglose[i]);
                }
            }
        }

        listaHoTmp[j].ListaDesglose = listaHoraDesgAjust;
    }

    return listaHoTmp;
}

function mostrarPopupLeyendaDesglose() {
    setTimeout(function () {
        $('#popupLeyendaDesglose').bPopup({
            easing: 'easeOutBack',
            speed: 450,
            transition: 'slideDown',
            modalClose: true
        });
    }, 50);
}

function incluirDescripcionBlackStart(numero) {
    var descripcion = $('#detalle' + numero + " #TxtDescripcion").val();
    descripcion = descripcion != null ? descripcion : '';
    var descVal = descripcion.toUpperCase();

    if ($('#detalle' + numero + ' #chkArranqueBlackStart').is(":checked")) {
        if (!(descVal.includes("BLACK") && descVal.includes("START"))) {
            descripcion = "Arranque en Black Start, " + descripcion;
            $('#detalle' + numero + " #TxtDescripcion").val(descripcion);
        }

    } else {
        var strInDesc = "Arranque en Black Start,".toUpperCase();
        var posTxt = descVal.indexOf(strInDesc);
        if (posTxt > -1) {
            var descNew = descripcion.substring(0, posTxt) + '' + descripcion.substring(posTxt + strInDesc.length, descripcion.length - posTxt + strInDesc.length);
            $('#detalle' + numero + " #TxtDescripcion").val(descNew.trim());
        }
    }
}

function mostrarCheckEnsayoPotenciaEfectiva(numero) {
    var calif = parseInt($('#detalle' + numero + ' #cbTipoOp').val()) || 0;

    $('#detalle' + numero + ' #chkEnsayoPotenciaEfectiva').hide();
    $('#detalle' + numero + ' #txtEnsayoPotenciaEfectiva').hide();
    $('#detalle' + numero + ' #chkEnsayoPotenciaMinima').hide();
    $('#detalle' + numero + ' #txtEnsayoPotenciaMinima').hide();
    $('#detalle' + numero + ' #hdMensajeEnsayo').hide();

    if (SUBCAUSACODI_POR_PRUEBAS == calif) {
        $('#detalle' + numero + ' #chkEnsayoPotenciaEfectiva').show();
        $('#detalle' + numero + ' #txtEnsayoPotenciaEfectiva').show();
        $('#detalle' + numero + ' #chkEnsayoPotenciaMinima').show();
        $('#detalle' + numero + ' #txtEnsayoPotenciaMinima').show();
        $('#detalle' + numero + ' #hdMensajeEnsayo').show();
    }
}

///////////////////////////////////////////////////////////////////////////////////////////////////
/// Costos incrementales
///////////////////////////////////////////////////////////////////////////////////////////////////
function costoIncr_mostrarAdvertencia(listaHo, listaValCI) {
    if (listaHo.length > 0) {
        var html = '';
        $("#div_msj_val_costo_incremental").prop('class', 'action-message-hop');

        html += 'Existen centrales con costos incrementales más caras por bajar: <br/>';
        html += `
                <table class="pretty tabla-icono tabla-ems" style="text-indent: 0px;width: 100%; margin-top: 7px;">
                    <thead>
                        <tr>
                            <th style="">Empresa</th>
                            <th>Central</th>
                            <th>Modo de Operación</th>
                            <th style="">Hora Inicio</th>
                            <th style="">Hora Fin</th>
                            <th>Calificación</th>
                            <th>Descripción</th>
                        </tr>
                    </thead>
                    <tbody>
                `;

        //Intervenciones
        for (var j = 0; j < listaValCI.length; j++) {
            var regValCI = listaValCI[j];
            var empresa = regValCI.Emprnomb;
            var central = regValCI.Central;
            var modo = regValCI.ModoOp;
            var horaIni = regValCI.FechaIniDesc;
            var horaFin = regValCI.FechaFinDesc;
            var calif = regValCI.Subcausadesc;
            var descrip = regValCI.Descripcion;

            html += `
                    <tr>
                        <td>${empresa}</td>
                        <td>${central}</td>
                        <td>${modo}</td>
                        <td>${horaIni}</td>
                        <td>${horaFin}</td>
                        <td>${calif}</td>
                        <td style="text-align:left;">${descrip}</td>
                    </tr>
            `;
        }
        html += `
                    </tbody>
                </table>
                `
            ;


        $("#div_msj_val_costo_incremental").html(html);
        $("#div_msj_val_costo_incremental").show();
        $(".fila_val_costo_incremental").show();
        $("#tblValidacionOtroApp").show();
    }
}

///////////////////////////////////////////////////////////////////////////////////////////////////
/// Intervenciones
///////////////////////////////////////////////////////////////////////////////////////////////////
function interv_mostrarAdvertencia(listaHo, listaInterv) {
    if (listaHo.length > 0) {
        var html = '';
        $("#div_msj_val_intervenciones").prop('class', 'action-alert');

        //Horas de operacion
        for (var i = 0; i < listaHo.length; i++) {
            //html += i > 0 ? '<br/>' : '';
            var regHo = listaHo[i];
            var horaIni = moment(regHo.Hophorini).format('DD/MM/YYYY HH:mm:ss');
            var horaFin = moment(regHo.Hophorfin).format('DD/MM/YYYY HH:mm:ss');
            var central = regHo.Central;
            var modo = regHo.Grupoabrev;

            html += `La hora de operación <b>${central}</b>-<b>${modo}</b> ${horaIni} hasta ${horaFin}.<br/>`;
        }

        html += 'Presenta unidades que están en mantenimiento y fuera de servicio: <br/>';
        html += `
                <table class="pretty tabla-icono tabla-ems" style="text-indent: 0px;width: 100%; margin-top: 7px;">
                    <thead>
                        <tr>
                            <th style="">Unidad</th>
                            <th>Tip. Interv.</th>
                            <th style="">Hora Inicio</th>
                            <th style="">Hora Fin</th>
                            <th>Disp.</th>
                            <th>Descripción</th>
                        </tr>
                    </thead>
                    <tbody>
                `;

        //Intervenciones
        for (var j = 0; j < listaInterv.length; j++) {
            var regInterv = listaInterv[j];
            var unidad = regInterv.Equinomb;
            var tipoInter = regInterv.Tipoevenabrev;
            var horaIni = regInterv.FechaIniDesc;
            var horaFin = regInterv.FechaFinDesc;
            var disp = regInterv.Interindispo;
            var descrip = regInterv.Interdescrip;

            html += `
                    <tr>
                        <td>${unidad}</td>
                        <td style="text-align:left;">${tipoInter}</td>
                        <td>${horaIni}</td>
                        <td>${horaFin}</td>
                        <td>${disp}</td>
                        <td style="text-align:left;">${descrip}</td>
                    </tr>
            `;
        }
        html += `
                    </tbody>
                </table>
                `
            ;


        $("#div_msj_val_intervenciones").html(html);
        $("#div_msj_val_intervenciones").show();
        $(".fila_val_intervenciones").show();
    }
}

//*****************************************************************
//***************** UTIL ******************************************
//*****************************************************************


function ajustarAlturaTablas() {

    // Obtén todas las tablas con la clase .tablitaCSS
    const tablas = document.querySelectorAll('table .tablitaCSS');

    // Encuentra la tabla más alta por altura
    var tablaMasAlta = null;
    var maxAlturaTabla = 0;

    tablas.forEach(tabla => {
        const alturaTabla = tabla.offsetHeight;
        if (alturaTabla > maxAlturaTabla) {
            maxAlturaTabla = alturaTabla;
            tablaMasAlta = tabla;
        }
    });

    if (tablaMasAlta) {
        // Obtén las alturas de los <td> de la primera columna de la tabla principal sin considerar los de la tabla interna
        const alturasPrimeraColumna = [];
        const primeraColumnaTDs = tablaMasAlta.querySelectorAll('td:first-child');

        primeraColumnaTDs.forEach(td => {
            // Verifica si los dos contenedores anteriores al <td> son una tabla con la clase .tablitaCSS
            const contenedoresAnteriores = td.parentElement.parentElement.parentElement;
            if (
                contenedoresAnteriores &&
                contenedoresAnteriores.tagName === 'TABLE' &&
                contenedoresAnteriores.classList.contains('tablitaCSS')
            ) {
                alturasPrimeraColumna.push(td.parentElement.offsetHeight);
            }
        });
        // Aplica las alturas de la primera columna de los <td> a las demás tablas en la misma posición
        tablas.forEach((tabla, tablaIndex) => {
            if (tabla !== tablaMasAlta) {
                const primeraColumnaTabla = tabla.querySelectorAll('td:first-child');
                var contador = 0;
                primeraColumnaTabla.forEach((td, tdIndex) => {
                    const contenedoresAnteriores = td.parentElement.parentElement.parentElement;
                    if (
                        contenedoresAnteriores &&
                        contenedoresAnteriores.tagName === 'TABLE' &&
                        contenedoresAnteriores.classList.contains('tablitaCSS')
                    ) {
                        // Aplica la altura almacenada en alturasPrimeraColumna según el índice del <td>
                        const alturaAaplicar = alturasPrimeraColumna[contador];
                        td.parentElement.style.height = alturaAaplicar + 'px';
                        contador++;
                    }
                });
            }
        });
    }

}
