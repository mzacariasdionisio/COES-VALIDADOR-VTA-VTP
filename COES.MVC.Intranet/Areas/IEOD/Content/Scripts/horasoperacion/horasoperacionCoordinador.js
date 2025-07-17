//scrips relacionados => "globales.js"
var msAlerta = 1000 * 60 * 5;

$(document).ready(function () {
    inicializarPantallaEms();
});

window.addEventListener('resize', function (event) {
    _setearDimensionTabla();
});

function inicializarPantallaEms() {
    moment.locale('es');

    $("#btnOcultarMenu").click();

    $('#tab-container').easytabs({
        animate: false
    });
    $('#tab-container').bind('easytabs:after', function () {
        refrehDatatable();
    });

    $('#cbEmpresa').change(function () {
        $("#mensajePrincipal").hide();
        cargarVista();
    });

    $('#txtFecha').Zebra_DatePicker({
        onSelect: function () {
            $("#mensajePrincipal").hide();
            mostrarFiltroHoraMinuto();
            cargarVista();
        }
    });

    ui_setInputmaskHoraMin("#input_horamin_check");
    ui_setInputmaskHoraMin("#input_horamin");
    $("#input_horamin_check").on('focusout', function (e) { setearValorHoraMin("#input_horamin_check"); });
    $("#input_horamin").on('focusout', function (e) { setearValorHoraMin("#input_horamin"); });

    $('input[name=checkHoraMinuto]').change(function () {
        mostrarFiltroHoraMinuto();
    });
    mostrarFiltroHoraMinuto();

    $("#btnConsultar").click(function () {
        $("#mensajePrincipal").hide();
        cargarVista();
    });

    $('#btnColorCentral').click(function () {
        redireccionarCentralColor();
    });

    $("#btnReporte").click(function () {
        redireccionarReporteHOP();
    });

    $('#btnManual').click(function () {
        openManual();
    });

    $("#btnAgregarHoraOperacionEms").click(function () {
        agregarHoraOperacionEms();

    });

    $("#btnNuevoDia").click(function () {
        registrarNuevoDia();
    });

    $("#btnContinuarDia").click(function () {
        continuarDia();
    });

    $('#btnVerEnvios').click(function () {
        popUpListaEnvios();
    });

    $('#btnVerLeyenda').click(function () {
        mostrarPopupLeyendaDesglose();
    });

    $('#btnReporteCT').click(function () {
        generarExcelReporteCT();
    });

    $('#btnAceptar2').hide();
    $('#btnCancelar2').hide();

    //determinar dimensiones (ancho, alto) para cada tab / tabla
    _setearDimensionTabla();

    cargarVista();

    //ejecutar alertas 5 minutos despues de cargar la pantalla

    setTimeout(function () {
        ejecutarCadaXMinutos();
    }, msAlerta);
}

function _setearDimensionTabla() {
    var anchoVisible = $(window).width() - 80;
    ANCHO_VISIBLE_GRAFICO = anchoVisible < ANCHO_VISIBLE_GRAFICO_DEFAULT ? ANCHO_VISIBLE_GRAFICO_DEFAULT : anchoVisible;

    ANCHO_LISTADO_EMS = ANCHO_VISIBLE_GRAFICO - 30;

    var altoVisible = $(window).height() - 310;
    ALTO_VISIBLE_LISTADO_EMS = altoVisible < ALTO_VISIBLE_LISTADO_EMS_DEFAULT ? ALTO_VISIBLE_LISTADO_EMS_DEFAULT : altoVisible;

    //gráfico. Dependiendo de la escala de la pantalla o del navegador no se calcula correctamente el alto de los borders
    var tamanioBorder = parseFloat($("#divAcciones").css("borderLeftWidth").replace('px', ''));
    if (tamanioBorder <= 0) tamanioBorder = 1;
    HEIGTH_BORDER_CELDA_TD = tamanioBorder;
}

function openManual() {
    window.location = controlador + 'MostrarManual';
}

function cargarVista(idEnvio) {

    ORDEN_GRAFICO_TIPO = 'asc';
    ORDEN_GRAFICO_CAMPO = ORDEN_GRAFICO_CAMPO_CI;

    APP_OPCION = -1;
    $('#enviosanteriores').bPopup().close();
    $("#btnGuardarHoraOperacionEms").parent().hide();
    $('#imgGuardarHoraOperacion').css('opacity', '0.5');

    $('.tab_correo').hide();

    cargarListado(idEnvio);
}

function cargarListado(idEnvio) {

    $("#botones-listado").hide();
    $("#mensajeFijo").hide();
    $(".leyenda_alerta").hide();

    $('#mensajeAlerta').html("");
    $("#mensajeAlerta").hide();

    $('#listado').html("");
    $('#listado').hide();
    $('.msjModifHOP').hide();

    $('#grafico').html("");
    $('#graficoUnidNoReg').html("");
    $('#tituloGraficoUnidNoReg').hide();
    $('#graficoUnidNoRegScada').html("");
    $('#tituloGraficoUnidNoRegScada').hide();

    $('#btnAceptar2').hide();
    $('#btnCancelar2').hide();
    $('#btnAceptar3').hide();
    $('#btnCancelar3').hide();

    $("#divValidacion_div").html('');
    $("#tblValidacionOtroApp").hide();
    $(".divEdicionMasiva").html('');

    listadoHOP(idEnvio);
}

function mostrarFiltroHoraMinuto() {
    var fechaDefecto = $("#txtFechaTmpAlCargarVista").val();
    var horaDefecto = $("#txtHoraMinTmpAlCargarVista").val();
    var fechaConsulta = $("#txtFecha").val();

    if (fechaDefecto != fechaConsulta) {
        $('input[name=checkHoraMinuto][value="2"]').prop("checked", true);
        $("#celda_horamin_check").hide();
        $("#celda_horamin_input").show();
        $("#input_horamin").val("23:59");
    } else {
        //hoy día

        $("#celda_horamin_check").show();
        $("#celda_horamin_input").hide();
        $("#input_horamin_check").val(horaDefecto);

        var valorCheck = getValorCheckHoraMinuto();
        if (VALOR_CHECK_HORAMIN_TIEMPOREAL == valorCheck) {
            $('#input_horamin_check').prop('disabled', 'disabled');
        } else {
            $('#input_horamin_check').removeAttr("disabled");
        }
    }
}

function getValorCheckHoraMinuto() {
    return parseInt($('input[name=checkHoraMinuto]:checked').val()) || 0;
}

function getValorHoraMin(id) {
    var fechaDefecto = $("#txtFechaTmpAlCargarVista").val();
    var horaDefecto = $("#txtHoraMinTmpAlCargarVista").val();
    var fechaConsulta = $("#txtFecha").val();

    var hmin = obtenerHoraValida($(id).val());

    if (fechaDefecto != fechaConsulta) {
        if (hmin == '')
            hmin = '23:59';
    } else {
        if (hmin == '')
            hmin = horaDefecto;
    }

    return hmin;
}

function setearValorHoraMin(id) {
    var hmin = getValorHoraMin(id);
    $(id).val(hmin);
}

function getValorHoraMinToConsulta() {
    var fechaDefecto = $("#txtFechaTmpAlCargarVista").val();
    var horaDefecto = $("#txtHoraMinTmpAlCargarVista").val();
    var fechaConsulta = $("#txtFecha").val();
    var hmin = '';

    if (fechaDefecto != fechaConsulta) {
        hmin = obtenerHoraValida($('#input_horamin').val());
        if (hmin == '')
            hmin = '23:59';
    } else {
        var valorCheck = getValorCheckHoraMinuto();
        if (VALOR_CHECK_HORAMIN_TIEMPOREAL == valorCheck) {
            hmin = horaDefecto;
        } else {
            hmin = obtenerHoraValida($('#input_horamin_check').val());
            if (hmin == '')
                hmin = horaDefecto;
        }
    }

    return hmin;
}

//////////////////////////////////////////////////////////////////////////////////////////
// Agregar Hora de Operación
//////////////////////////////////////////////////////////////////////////////////////////
function agregarHoraOperacionEms() {

    APP_OPCION = OPCION_NUEVO;
    ORIGEN_DETALLE = "AGREGAR";

    //cambiar tab a detalle
    $('#tab-container').easytabs('select', '#vistaDetalle');
    $("#div_vista_detalle_contenido").css('width', ANCHO_LISTADO_EMS + 'px');
    $("#divValidacion_div").html('');
    $("#tblValidacionOtroApp").hide();
    $(".divEdicionMasiva").html('');

    //generar objetos formularios
    var listaObj = [];
    listaObj.push(_objInicializarBotonAgregar());

    //generar html y activar eventos js
    agregarListaDivFormulario(listaObj);
}

function enviarHorasOperacionEms(listaObjDTO) {

    $("#btnAceptar2").hide();
    $("#btnAceptar3").hide();
    $("#btnCancelar2").hide();
    $("#btnCancelar3").hide();

    //Generar objeto json
    var empresa = 0;
    var fecha = $('#txtFecha').val();
    var tipoCentral = $('#cbTipoCentral').val();

    var matriz = JSON.stringify(formatJavaScriptSerializer(listaObjDTO));
    var confirmarInterv = parseInt($("#hfConfirmarValInterv").val()) || 0;

    var dataJson = {
        idEmpresa: empresa,
        tipoCentral: tipoCentral,
        fecha: fecha,
        flagConfirmarInterv: confirmarInterv,
        data: matriz,
    };

    $.ajax({
        url: controlador + "RegistrarEnvioHorasOperacion",
        type: 'POST',
        contentType: 'application/json; charset=UTF-8',
        dataType: 'json',
        data: JSON.stringify(dataJson),
        success: function (model) {
            _irATabPantalla();

            if (model.Resultado == 1) {
                $(".fila_val_costo_incremental").hide();
                $(".fila_val_intervenciones").hide();
                $("#tblValidacionOtroApp").hide();

                var strMensaje = APP_OPCION != OPCION_ELIMINAR ? "La Hora de Operación se guardó correctamente" : "La Hora de Operación se eliminó correctamente";

                $("#mensajePrincipal").prop('class', 'action-exito');
                $("#mensajePrincipal").text(strMensaje);
                $("#mensajePrincipal").show();
                $("#mensajePrincipal").fadeOut(2500);

                $("#btnAceptar2").hide();
                $("#btnAceptar3").hide();
                $("#btnCancelar2").hide();
                $("#btnCancelar3").hide();
                cargarVista();
            }
            else {
                //Validación de intervenciones y/o costos incrementales
                if (model.Resultado == 2) {
                    $("#btnAceptar3").show();
                    $("#btnCancelar3").show();

                    $('#btnAceptar3').unbind();
                    $('#btnCancelar3').unbind();

                    //opción edición desde "Detalle" que viene desde el botón editar múltiple o botón nuevo
                    if (APP_OPCION == OPCION_EDITAR || APP_OPCION == OPCION_NUEVO) {
                        $('#btnAceptar3').click(function () {
                            $('#hfConfirmarValInterv').val(1);
                            ho_ValidarFormulario();
                        });
                        $('#btnCancelar3').click(function () {
                            var idModoGrupo = parseInt($('#detalle1 #cbModoOpGrupo').val()) || 0;
                            editarHOP(idModoGrupo);
                        });
                    }
                    else {
                        //opción edición desde "Listado""
                        $('#btnAceptar3').click(function () {
                            $('#hfConfirmarValInterv').val(1);
                            ho_ValidarRegistrosEnGrid();
                        });

                        $('#btnCancelar3').click(function () {
                            $('#tab-container').easytabs('select', '#vistaListado');
                            $("#divValidacion_div").html('');
                            $("#tblValidacionOtroApp").hide();
                            $(".divEdicionMasiva").html('');
                        });
                    }

                    var msjAlert = '';
                    //ci
                    if (model.ListaValidacionHorasOperacionCostoIncremental != null && model.ListaValidacionHorasOperacionCostoIncremental.length > 0) {
                        msjAlert += 'Existen centrales con costos incrementales más caras por bajar. ';
                        costoIncr_mostrarAdvertencia(model.ListaHorasOperacionCostoIncremental, model.ListaValidacionHorasOperacionCostoIncremental);
                    }

                    //intervenciones
                    if (model.ListaValidacionHorasOperacionIntervencion != null && model.ListaValidacionHorasOperacionIntervencion.length > 0) {
                        msjAlert += 'Se ha encontrado equipos que están en mantenimiento y fuera de servicio. ';
                        interv_mostrarAdvertencia(model.ListaHorasOperacionIntervencion, model.ListaValidacionHorasOperacionIntervencion);
                    }

                    alert(msjAlert);
                } else {
                    $("#btnAceptar2").show();
                    $("#btnCancelar2").show();
                    alert(model.Mensaje);
                }
            }
        },
        error: function (err) {
            alert("Se perdió la conexión usuario/servidor COES, no llegó la información al servidor.");
        }
    });

}

async function validarCruceHorasOperacion(listaObjDTO) {

    var model = {};
    try {
        //TODO clonar lista

        //Generar objeto json
        var fecha = $('#txtFecha').val();
        var dataForm = JSON.stringify(formatJavaScriptSerializer(listaObjDTO));
        var dataListado = JSON.stringify(formatJavaScriptSerializer(GLOBAL_HO.ListaHorasOperacion));

        var dataJson = {
            fecha: fecha,
            dataForm: dataForm,
            dataListado: dataListado,
        };

        model = await $.ajax({
            url: controlador + "ValidarRegistrarEnvioHorasOperacion",
            type: 'POST',
            contentType: 'application/json; charset=UTF-8',
            dataType: 'json',
            data: JSON.stringify(dataJson),
        });
    } catch (error) {
        alert("Se perdió la conexión usuario/servidor COES, no llegó la información al servidor.");
        model.Resultado = -1;
        model.ListaValCruce = [];
        model.ListaValCruce.push({ Mensaje: "Error en validación." });
    }

    return model;
}

//////////////////////////////////////////////////////////////////////////////////////////
// Guardar Hora de Operación (Listado)
//////////////////////////////////////////////////////////////////////////////////////////

function existeModificacionesHOP() {
    var hayGuardar = $('#imgGuardarHoraOperacion').css('opacity');
    $(".msjModifHOP").hide();
    $("#btnGuardarHoraOperacionEms").parent().hide();

    if (hayGuardar == '1') {
        alert("Existe modificaciones a las Horas de Operación. Guarde los cambios para continuar.");
        $("#btnGuardarHoraOperacionEms").parent().show();
        $(".msjModifHOP").show();
        return true;
    } else {
        return false;
    }
}

//////////////////////////////////////////////////////////////////////////////////////////
// Detalle
//////////////////////////////////////////////////////////////////////////////////////////

function verHOP(grupocodi) {
    APP_OPCION = OPCION_VER;

    //mostrar formularios
    editarHOPMejora(grupocodi);
}

function editarHOP(grupocodi, objConvertir) {
    APP_OPCION = OPCION_EDITAR;

    //mostrar formularios
    editarHOPMejora(grupocodi, objConvertir);
}

function editarHOPMejora(grupocodi, objConvertir) {
    //cambiar tab a detalle
    $('#tab-container').easytabs('select', '#vistaDetalle');
    $("#div_vista_detalle_contenido").css('width', ANCHO_LISTADO_EMS + 'px');
    $("#divValidacion_div").html('');
    $("#tblValidacionOtroApp").hide();
    $(".divEdicionMasiva").html('');

    //las horas de operación del modo
    var elementosFiltrados = GLOBAL_HO.ListaHorasOperacion.filter((x) => x.Grupocodi == grupocodi);

    //ordenar por fecha
    elementosFiltrados = elementosFiltrados.filter(function (elemento) {
        return elemento !== undefined;
    }).sort((a, b) => a.Hophorini - b.Hophorini);

    //generar objetos formularios
    var listaObj = [];
    for (var i = 0; i < elementosFiltrados.length; i++) {
        var objDTO = elementosFiltrados[i];
        objDTO.Editar = 'S';
        objDTO.IdPos = i + 1;
        listaObj.push(_objInicializarBotonEditar(objDTO));
    }
    if (objConvertir != undefined && objConvertir != null)
        listaObj.push(objConvertir);

    //generar html y eventos js
    agregarListaDivFormulario(listaObj);
}

function eliminarHOP(hopcodi) {

    hopcodi = parseFloat(hopcodi);

    $("#mensajePrincipal").hide();

    var elementosFiltrado = [];
    if (confirm("¿Desea eliminar hora de operación?")) {
        if (GLOBAL_HO.ListaHorasOperacion.length > 0) {

            elementosFiltrado = GLOBAL_HO.ListaHorasOperacion.filter((x) => x.Hopcodi == hopcodi);
            for (var i = 0; i < elementosFiltrado.length; i++) {
                elementosFiltrado[i].OpcionCrud = -1;
            }

            APP_OPCION = OPCION_ELIMINAR;
        }
    }

    if (APP_OPCION == OPCION_ELIMINAR) {
        //Enviar cambios al servidor

        $('#hfConfirmarValInterv').val(1); //no validar con incrementales o intervenciones
        enviarHorasOperacionEms(elementosFiltrado);
    }
}

///////////////////////////////////////////////////////////////////////////////////////////////////
/// Bitacora - F/S
///////////////////////////////////////////////////////////////////////////////////////////////////

function visualizarBitacora(id, hayCambios, numero) {
    APP_MOSTRAR_MODAL_EQUIPO_YA_CARGADO = 0;
    if (hayCambios > 0) {
        setTimeout(function () {
            $('#popupBitacora').bPopup({
                easing: 'easeOutBack',
                speed: 450,
                transition: 'slideDown',
                modalClose: true
            });
        }, 50);
    } else {
        $('#busquedaEquipoBitacora').html('');
        $.ajax({
            type: 'POST',
            url: controlador + "Bitacora",
            data: {
                id: id,
            },
            success: function (dataHtml) {
                $('#idPopupBitacora').html(dataHtml);
                $("#IdNumeroDivBitacora").val(numero);
                bitacora_Inicializar();
                setTimeout(function () {
                    $('#popupBitacora').bPopup({
                        easing: 'easeOutBack',
                        speed: 450,
                        transition: 'slideDown',
                        modalClose: true
                    });
                }, 50);
            },
            error: function (err) {
                alert("Ha ocurrido un error");
            }
        });
    }
}

//////////////////////////////////////////////////////////////////////////////////////////
// Listado
//////////////////////////////////////////////////////////////////////////////////////////

function listadoHOP(idEnvio) {
    $('#txtFechaTmp2').val(getFechaEms());
    $('#tab-container').easytabs('select', '#vistaListado');
    $('#hfConfirmarValInterv').val(0); //inicializar validación de Costo incremental en tiempo real

    //pestaña "Congestión"
    congestionCargarFrame();

    $.ajax({
        type: 'POST',
        url: controlador + 'ListaReporteHopEms',
        data: {
            sfecha: getFechaEms(),
            idEmpresa: getEmpresaEms(),
            idEnvio: idEnvio,
            sFecha2: $('#txtFechaTmp2').val(),
            flagHoraTR: getValorCheckHoraMinuto(),
            horaCI: getValorHoraMinToConsulta(),
        },
        success: function (model) {
            if (model.Resultado != -1) {
                GLOBAL_HO = model; //listado de horas de operación, data maestra

                LISTA_TIPO_OPERACION = model.ListaTipoOperacion;
                LISTA_MODO_OPERACION = model.ListaModosOperacion;
                LISTA_CENTRALES = model.ListaCentrales;
                LISTA_CALIFICACION = model.ListaTipoOperacion;
                LISTA_TIPO_DESGLOSE = model.ListaTipoDesglose;
                TIENE_PERMISO_ADMIN = model.TienePermisoAdministrador;

                $('#listado').show();

                //filtro hora minuto
                $('#txtHoraMinTmpAlCargarVista').val(model.HoraMinutoActual);

                $('#hfStrEquiposInv').val(model.StrLstTotalEquiposInvolucrados);

                var valorCheck = getValorCheckHoraMinuto();
                if (VALOR_CHECK_HORAMIN_TIEMPOREAL == valorCheck) {
                    $('#input_horamin_check').val(model.HoraMinutoActual);
                }

                //Listado
                $('#listado').html(_generarListadoHoraOperacionMejoraHtml(model.ListaHorasOperacion));
                $("#resultado").css('width', ANCHO_LISTADO_EMS + 'px');
                $("#grafico").css('width', ANCHO_VISIBLE_GRAFICO + 'px');
                $("#graficoUnidNoReg").css('width', ANCHO_VISIBLE_GRAFICO + 'px');
                $("#graficoUnidNoRegScada").css('width', ANCHO_VISIBLE_GRAFICO + 'px');

                aplicarTxtGrdOnChange("");

                //Grafico
                $('#cbCentral').val(GLOBAL_HO.IdCentralSelect);
                $('#hfEquipoDefault').val(GLOBAL_HO.IdEquipo);
                $("#chkDesglose").prop("checked", true);
                $("#chkCongestion").prop("checked", true);

                $('#grafico').html(dibujarTablaHorasOperacionEms());

                $('#tituloGraficoUnidNoReg').show();
                $('#graficoUnidNoReg').html(generaViewListaUnidadesNoRegistradasEms(getFechaEms()));
                $('#tituloGraficoUnidNoRegScada').show();
                $('#graficoUnidNoRegScada').html(generaViewListaUnidadesNoRegistradasScada(getFechaEms()));

                graficoEventoContextMenu();
                graficoEmsContextMenu();
                graficoScadaContextMenu();
                grafico_displayDiv('dv-desglose');
                grafico_displayDiv('dv-congestion');

                //Pestaña 1: Tabla Listado
                $('#reporteHO').dataTable({
                    "scrollX": true,
                    "scrollCollapse": false,
                    "sDom": 't',
                    "ordering": false,
                    paging: false,
                    "scrollY": ALTO_VISIBLE_LISTADO_EMS >= ALTO_VISIBLE_LISTADO_EMS_DEFAULT ? ALTO_VISIBLE_LISTADO_EMS + "px" : "100%",
                    fixedColumns: {
                        leftColumns: 5
                    }
                });
                // Fin Tabla Listado

                //Pestaña 4: Congestión
                $("#vistaCongestion").css('width', ANCHO_LISTADO_EMS + 'px');
                $("#ifrCongestion").css('width', ANCHO_LISTADO_EMS + 'px');

                //Pestaña 5: Costos Térmicos
                $('#costoCT').html(_generarListarReporteHopCTHtml(model.ListaModosOperacionCT));
                $("#resultado_tbl_ct").css('width', (ANCHO_LISTADO_EMS) + 'px');

                refrehDatatable();

                ////Pestaña Correos
                //$('#listadoCorreo').html(data[3]);
                //if (data[3].length > 0)
                //    $('.tab_correo').show();

                //Alertas sobre los tabs
                if (model.ListaAlerta.length > 0) {
                    $("#mensajeAlerta").html(_generarListarAlertaHtml(model.ListaAlerta));
                    $("#mensajeAlerta").show();
                }

                //Intervenciones del día anterior
                var numHopConIntervAnt = model.TotalValInterFS;
                if (numHopConIntervAnt > 0) {
                    var strHtml = 'Existe inconsistencia con Intervención F/S. Revisar la(s) alerta(s) de campana morada.';

                    $("#mensajeFijo").html(strHtml);
                    $("#mensajeFijo").prop('class', 'action-alert');
                    $("#mensajeFijo").show();
                }

                //por defecto el gráfico
                $('#tab-container').easytabs('select', '#vistaGrafico');
            } else {
                alert(model.Mensaje);
            }
        },
        error: function (err) {
            alert("Se perdió la conexión usuario/servidor COES.");
        }
    });
    
}

function _generarListadoHoraOperacionMejoraHtml(listaData) {
    var html = '';

    html += `
    <div class='freeze_table' id='resultado' style=''>
    <table id='reporteHO' class='pretty tabla-icono' style='table-layout: fixed;' >
        <thead>
            <tr>
                <th style='width:60px' class=''>Alertas</th>
                <th style='width:90px'>Acciones</th>
                <th style='width:140px'>Empresa</th>
                <th style='width:120px'>Central</th>
                <th style='width:270px'>Modos de Operación</th>
                
                <th style='width:100px'>Orden de Arranque</th>
                <th style='width:100px'>EN PARALELO</th>
                <th style='width:100px'>Orden de Parada</th>
                <th style='width:100px'>FIN DE REGISTRO</th>
                <th style='width:150px'>Calificación</th>
                <th style='width:90px'>Ensayo de Pe</th>
                <th style='width:90px'>Ensayo de PMin</th>
                <th style='width:90px'>Lím Transm.</th>

                <th style='width:500px;'>Observación</th>
            </tr>
        </thead>
        <tbody>
    `;

    var classAlerta;
    var fila = 0;

    if (listaData.length > 0) {
        for (var i = 0; i < listaData.length; i++) {
            fila++;
            classAlerta = "";

            var reg = listaData[i];
            var htmlTdAlerta = _getHtmlTdAlerta(reg);
            var htmlTdAcciones = _getHtmlTdAcciones(reg, TIENE_PERMISO_ADMIN, fila);

            var htmlBloqueColEditable = _getHtmlTdEditableHo(reg, fila);

            html += `
            <tr id='hop${reg.Hopcodi}' class='${classAlerta}' style='width:60px;background-color: ${reg.ColorTermica}'>
                <td class='class_hop_ems' style="width: 60px;">
                    ${htmlTdAlerta}
                </td>
                <td class='class_hop_ems' style="width:90px;">
                    ${htmlTdAcciones}
                </td>

                <td class='class_hop_ems hor_empresa' style="width:140px;"><div title="${reg.Emprnomb}" style="width: 135px;overflow:hidden;white-space:nowrap;">${reg.Emprnomb}</div></td>
                <td class='class_hop_ems hor_central' style="width:120px;" title="${reg.Central}">${reg.Central}</td>

                ${htmlBloqueColEditable}

                <td style="width:90px;" class='class_hop_ems hor_ensayo_pe'>${reg.HopensayopeDesc}</td>
                <td style="width:90px;" class='class_hop_ems hor_ensayo_pe'>${reg.HopensayopminDesc}</td>
                <td style="width:90px;" class='class_hop_ems hor_lim_trans'>${reg.HoplimtransDesc}</td>

                <td class='class_hop_ems hor_desc' style="width:500px">${reg.Hopdesc}</td>

            </tr>
            `;
        }

    }

    html += `
        </tbody>
    </table>
        `;

    return html;
}

function _getHtmlTdAlerta(reg) {
    var html = '';

    if (reg.TieneAlertaScada == 1 || reg.TieneAlertaEms == 1) {
        if (reg.TieneAlertaEms == 1)
            html += `<div class='bellImgEms' title='Alerta Ems' onclick='verAlertaEmsXHOP(${reg.Hopcodi})'/> `;
        else {
            if (reg.TieneAlertaScada == 1) {
                html += `<div class='bellImgScada' title='Alerta Scada' onclick='verAlertaScadaXHOP(${reg.Hopcodi})'/> `;
            }
            else {
                html += `<div style='display: inline; margin-right: 10px; margin-left: 10px;'></div>`;
            }
        }
    }
    else {
        html += `<div style='display: inline; margin-right: 10px; margin-left: 10px;'></div>`;
    }

    if (reg.TieneAlertaCostoIncremental == 1) {
        html += `<div class='bellImgCostoIncremental' title='Alerta Costo Incremental' onclick='verAlertaCostoIncrementalXHOP(${reg.Hopcodi})' /> `;
    }
    else {
        html += `<div style='display: inline; margin-right: 10px; margin-left: 10px;'></div>`;
    }

    if (reg.TieneAlertaIntervencion == 1) {
        html += `<div class='bellImgIntervencion' title='Alerta Intervención' onclick='verAlertaIntervencionXHOP(${reg.Hopcodi})'/> `;
    }
    else {
        html += `<div style='display: inline; margin-right: 10px; margin-left: 10px;'></div>`;
    }

    html += ``;

    return html;
}

function _getHtmlTdAcciones(reg, tienePermisoAdmin, fila) {
    var html = '';

    html += `<a class='ver_hop' href='JavaScript:verHOP(${reg.Grupocodi})' style='margin-right: 4px;'>
                    <img src='${siteRoot}Content/Images/btn-open.png' alt='Ver registro' title='Ver registro' />
            </a>`;

    if (tienePermisoAdmin)
        html += `<a class='edt_hop' href='JavaScript:editarHOP(${reg.Grupocodi})' style='margin-right: 4px;'>
                    <img src='${siteRoot}Content/Images/btn-edit.png' alt='Editar registro' title='Editar registro' />
                 </a>`;

    if (tienePermisoAdmin)
        html += `<a class='eli_hop' href='JavaScript:eliminarHOP(${reg.Hopcodi})' style='margin-right: 4px;'>
                    <img src='${siteRoot}Content/Images/Trash.png' alt='Eliminar registro' title='Eliminar registro' />
                </a>`;

    html += `<a id="des_hop_clone_${fila}" class='des_hop' href="JavaScript:deshacerHOP(${reg.Hopcodi}, 'des_hop_clone_${fila}')" style='margin-right: 4px;display:none;'>
                    <img src='${siteRoot}Content/Images/btn-undo.png' alt='Deshacer cambios' title='Deshacer cambios' />
            </a>`;

    return html;
}

function _getHtmlTdEditableHo(reg, fila) {
    var html = '';

    var calif = reg.FlagCalificado == 1 ? "" : "no_calificado";
    var idCelda = "tdMO-" + fila;
    var idLista = "ddMO-" + fila;

    if (reg.Grupotipomodo == "E") {
        html += `
            <td style="width:270px;" class='class_hop_ems hor_grupo' title="${reg.Gruponomb}">${reg.Gruponomb}</td>
            <td style="width:100px;" class='class_hop_ems hor_arranque'>${reg.HophorordarranqDesc}</td>
            <td style="width:100px;"  class='class_hop_ems hor_ini'>${reg.HophoriniDesc}</td>
            <td style="width:100px;" class='class_hop_ems hor_parada'>${reg.HophorparadaDesc}</td>
            <td style="width:100px;" class='class_hop_ems hor_fin'>${reg.HophorfinDesc}</td>
            <td style="width:150px;" class='class_hop_ems hor_tipo_op ${calif}' style="text-align: left !important;">${reg.Subcausadesc}</td>
        `;
    } else {

        html += `
        <td style="width:270px;" class='class_hop_ems hor_grupo' title="${reg.Gruponomb}">
            <div id="${idCelda}">
                <span onclick="javascript:editarMOEnGrid('${idCelda}','${idLista}',${reg.CodiPadre},'${reg.Gruponomb}',${fila});">${reg.Gruponomb}</span>
            </div>
        </td>
        <td style="width:100px;" class='class_hop_ems hor_arranque' id="td-hor_arranque-${fila}">
            <input class="txtEnGrid" type="text" style="width:70px" id="txt-hor_arranque-${fila}" name="txt-hor_arranque-${fila}" value="${reg.HophorordarranqDesc}">
        </td>
        <td style="width:100px;" class='class_hop_ems hor_ini' id="td-hor_ini-${fila}">
            <input class="txtEnGrid" type="text" style="width:70px" id="txt-hor_ini-${fila}" name ="txt-hor_ini-${fila}" value="${reg.HophoriniDesc}">
        </td>
        <td style="width:100px;" class='class_hop_ems hor_parada' id="td-hor_parada-${fila}">
            <input class="txtEnGrid" type="text" style="width:70px" id="txt-hor_parada-${fila}" name ="txt-hor_parada-${fila}" value="${reg.HophorparadaDesc}">
        </td>
        <td style="width:100px;" class='class_hop_ems hor_fin' id="td-hor_fin-${fila}">
            <input class="txtEnGrid" type="text" style="width:70px" id="txt-hor_fin-${fila}" name="txt-hor_fin-${fila}" value="${reg.HophorfinDesc}">
        </td>
        <td style="width:150px;" class='class_hop_ems hor_tipo_op ${calif}' style="text-align: left !important;">
            <div id="td-hor_tipo_op-${fila}">
                <span onclick="javascript:editarCalifEnGrid('td-hor_tipo_op-${fila}','dd-hor_tipo_op-${fila}','${reg.Subcausacodi}');">${reg.Subcausadesc}</span>
            </div>
        </td>
        `;
    }

    return html;
}

//////////////////////////////////////////////////////////////////////////////////////////
// Congestión
//////////////////////////////////////////////////////////////////////////////////////////
function congestionCargarFrame() {
    var urlCongestion = `${siteRoot}cortoplazo/congestion/listamanualPorFecha?fecha=${getFechaEms()}&flagHoraTR=${getValorCheckHoraMinuto()}&horaTR=${getValorHoraMinToConsulta()}`;
    $("#ifrCongestion").attr("src", urlCongestion);
}

//////////////////////////////////////////////////////////////////////////////////////////
// Listado - Alertas Programado, Pruebas aleatorias
//////////////////////////////////////////////////////////////////////////////////////////

function ejecutarCadaXMinutos() {
    //cada 5 minutos
    setInterval(() => mostrarAlertas(), (msAlerta));
}

function mostrarAlertas() {

    $.ajax({
        type: 'POST',
        url: controlador + 'MostrarAlertas',
        data: {
            sfecha: getFechaEms(),
            flagHoraTR: getValorCheckHoraMinuto(),
            horaCI: getValorHoraMinToConsulta(),
            alertasOcultas: $("#hfAlertasOcultas").val()
        },
        success: function (model) {
            if (model.Resultado != -1) {
                $("#mensajeAlerta").hide();

                if (model.ListaAlerta.length > 0) {
                    $("#mensajeAlerta").html(_generarListarAlertaHtml(model.ListaAlerta));
                    $("#mensajeAlerta").show();
                }
            } else {
                //alert(model.Mensaje);
            }
        },
        error: function (err) {
            alert("Se perdió la conexión usuario/servidor COES.");
        }
    });
}

function ocultarAlerta(element, codAlerta) {

    $("#hfAlertasOcultas").val($("#hfAlertasOcultas").val() + codAlerta + ",");

    $("#" + element).remove();

    var numAlertas = 0;

    $("#mensajeAlerta .hopAlerta").each(function (index) {

        var horaTexto = $(this).attr("hora");

        var hora = moment(horaTexto, 'YYYY-MM-DD HH:mm');

        if ((numAlertas < 2) && (hora < moment())) {

            $(this).show();
            numAlertas++;
        }
    });

    if ($("#mensajeAlerta .hopAlerta").length == 0) {
        $("#mensajeAlerta").hide();
    }
}

function _generarListarAlertaHtml(listaData) {
    var html = '';

    var display = '';
    var br = "<br>";
    var indice = 0;

    for (var i = 0; i < listaData.length; i++) {
        var item = listaData[i];

        indice++;

        if (indice == listaData.Count) { br = ""; }

        html += `<div id="dvAlerta${indice}" hora="${item.FechaHoraDesc}" class="hopAlerta" style="display:${display}">
                                <a href="javascript:ocultarAlerta('dvAlerta${indice}', '${item.CodAlerta}');">[X]</a>
                                &nbsp;&nbsp;${item.MsjAlerta} ${item.HoraDesc}${br}
                             </div>`

        if (indice > 1) {
            display = "none";
        }
    }

    return html;
}

//////////////////////////////////////////////////////////////////////////////////////////
// Listado - Alertas - Ems Scada Intervenciones
//////////////////////////////////////////////////////////////////////////////////////////

function verAlertaEmsXHOP(hopcodipadre) {
    verAlertaGenericoXhop(1, "ListarAlertaEmsXHOP", 'Detalle de Alerta EMS por Hora de Operación', hopcodipadre);
}

function verAlertaScadaXHOP(hopcodipadre) {
    verAlertaGenericoXhop(2, "ListarAlertaScadaXHOP", 'Detalle de Alerta Scada por Hora de Operación', hopcodipadre);
}

function verAlertaIntervencionXHOP(hopcodipadre) {
    verAlertaGenericoXhop(3, "ListarAlertaIntervencionXHOP", 'Detalle de Alerta de Intervenciones por Hora de Operación', hopcodipadre);
}

function verAlertaCostoIncrementalXHOP(hopcodipadre) {
    mostrarPopupAlertaGenericoXModo('Alerta de Costo Incremental por Hora de Operación', 'La hora de operación es de una central con costo incremental que es más cara por bajar.');
}

function verAlertaGenericoXhop(tipoPopup, metodoController, titulo, hopcodipadre) {
    $('#formAlertaGenericoXHOP').html('');
    $("#popupAlertaGenericoXHOP.popup-title").html('');

    $.ajax({
        type: 'POST',
        url: controlador + metodoController,
        data: {
            sfecha: getFechaEms(),
            idEmpresa: getEmpresaEms(),
            hopcodipadre: hopcodipadre
        },
        success: function (model) {
            if (model.Resultado != -1) {
                var htmlPopup = "";
                if (tipoPopup == 1) htmlPopup = _getHtmlPopupValidacionAplicativoEmsXHOP(model.HoraOperacion, model.ListaValidacionHorasOperacionEms);
                if (tipoPopup == 2) htmlPopup = _getHtmlPopupValidacionAplicativoScadaXHOP(model.HoraOperacion, model.ListaValidacionHorasOperacionScada);
                if (tipoPopup == 3) htmlPopup = _getHtmlPopupValidacionAplicativoIntervencionXHOP(model.HoraOperacion, model.ListaValidacionHorasOperacionIntervencion);

                mostrarPopupAlertaGenericoXModo(titulo, htmlPopup);
            } else {
                alert("Ha ocurrido un error.");
            }
        },
        error: function (err) {
            alert("Se perdió la conexión usuario/servidor COES.");
        }
    });
}

function _getHtmlPopupValidacionAplicativoEmsXHOP(HoraOperacion, ListaValidacionHorasOperacionEms) {
    var html = "";

    var htmlListado = "";

    for (var i = 0; i < ListaValidacionHorasOperacionEms.length; i++) {
        var item = ListaValidacionHorasOperacionEms[i];
        htmlListado += `        
                    <tr>
                        <td>${item.Equinomb}</td>
                        <td>${item.FechaIniDesc}</td>
                        <td>${item.FechaFinDesc}</td>
                    </tr>
        `;
    }


    html += `

    <div class='panel-container'>
        <div class="form-search">

            <table style="width:100%;margin-right: auto" class="table-form-show">
                <tr>
                    <td class="tbform-label" style="width: 117px;">Empresa:</td>
                    <td class="tbform-control">
                        ${HoraOperacion.Emprnomb}
                    </td>
                </tr>
                <tr>
                    <td class="tbform-label">Central:</td>
                    <td class="tbform-control">
                        ${HoraOperacion.Central}
                    </td>
                </tr>
                <tr>
                    <td class="tbform-label">Modo de Operación:</td>
                    <td class="tbform-control">
                        ${HoraOperacion.Gruponomb}
                    </td>
                </tr>
                <tr>
                    <td class="tbform-label">EN PARALELO:</td>
                    <td class="tbform-control">
                        ${HoraOperacion.HophoriniDesc}
                    </td>
                </tr>
                <tr>
                    <td class="tbform-label">FIN DE REGISTRO:</td>
                    <td class="tbform-control">
                        ${HoraOperacion.HophorfinDesc}
                    </td>
                </tr>
            </table>

            <br />
            Unidades que tienen estado Inactivo:
            <table class="pretty tabla-icono tabla-ems" style="width: 400px;">
                <thead>
                    <tr>
                        <th style="">Unidad</th>
                        <th style="">Hora Inicio</th>
                        <th style="">Hora Fin</th>
                    </tr>
                </thead>
                <tbody>
                    ${htmlListado}
                </tbody>
            </table>
        </div>
    </div>

    `;

    return html
}

function _getHtmlPopupValidacionAplicativoScadaXHOP(HoraOperacion, ListaValidacionHorasOperacionEms) {
    var html = "";

    var htmlListado = "";

    for (var i = 0; i < ListaValidacionHorasOperacionEms.length; i++) {
        var item = ListaValidacionHorasOperacionEms[i];
        htmlListado += `        
                    <tr>
                        <td>${item.Equinomb}</td>
                        <td>${item.FechaIniDesc}</td>
                        <td>${item.FechaFinDesc}</td>
                    </tr>
        `;
    }


    html += `

    <div class='panel-container'>
        <div class="form-search">

            <table style="width:100%;margin-right: auto" class="table-form-show">
                <tr>
                    <td class="tbform-label" style="width: 117px;">Empresa:</td>
                    <td class="tbform-control">
                        ${HoraOperacion.Emprnomb}
                    </td>
                </tr>
                <tr>
                    <td class="tbform-label">Central:</td>
                    <td class="tbform-control">
                        ${HoraOperacion.Central}
                    </td>
                </tr>
                <tr>
                    <td class="tbform-label">Modo de Operación:</td>
                    <td class="tbform-control">
                        ${HoraOperacion.Gruponomb}
                    </td>
                </tr>
                <tr>
                    <td class="tbform-label">EN PARALELO:</td>
                    <td class="tbform-control">
                        ${HoraOperacion.HophoriniDesc}
                    </td>
                </tr>
                <tr>
                    <td class="tbform-label">FIN DE REGISTRO:</td>
                    <td class="tbform-control">
                        ${HoraOperacion.HophorfinDesc}
                    </td>
                </tr>
            </table>

            <br />
            Unidades que no presentan señal Scada:
            <table class="pretty tabla-icono tabla-ems" style="width: 400px;">
                <thead>
                    <tr>
                        <th style="">Unidad</th>
                        <th style="">Hora Inicio</th>
                        <th style="">Hora Fin</th>
                    </tr>
                </thead>
                <tbody>
                    ${htmlListado}
                </tbody>
            </table>
        </div>
    </div>

    `;

    return html
}

function _getHtmlPopupValidacionAplicativoIntervencionXHOP(HoraOperacion, ListaValidacionHorasOperacionIntervencion) {
    var html = "";

    var htmlListado = "";

    for (var i = 0; i < ListaValidacionHorasOperacionIntervencion.length; i++) {
        var item = ListaValidacionHorasOperacionIntervencion[i];
        htmlListado += `        
                    <tr>
                        <td>${item.Equinomb}</td>
                        <td style="text-align:left;">${item.Tipoevenabrev}</td>
                        <td>${item.FechaIniDesc}</td>
                        <td>${item.FechaFinDesc}</td>
                        <td>${item.Interindispo}</td>
                        <td style="text-align:left;">${item.Interdescrip}</td>
                    </tr>
        `;
    }

    html += `

    <div class='panel-container'>
        <div class="form-search">

            <table style="width:100%;margin-right: auto" class="table-form-show">
                <tr>
                    <td class="tbform-label" style="width: 117px;">Empresa:</td>
                    <td class="tbform-control">
                        ${HoraOperacion.Emprnomb}
                    </td>
                </tr>
                <tr>
                    <td class="tbform-label">Central:</td>
                    <td class="tbform-control">
                        ${HoraOperacion.Central}
                    </td>
                </tr>
                <tr>
                    <td class="tbform-label">Modo de Operación:</td>
                    <td class="tbform-control">
                        ${HoraOperacion.Gruponomb}
                    </td>
                </tr>
                <tr>
                    <td class="tbform-label">EN PARALELO:</td>
                    <td class="tbform-control">
                        ${HoraOperacion.HophoriniDesc}
                    </td>
                </tr>
                <tr>
                    <td class="tbform-label">FIN DE REGISTRO:</td>
                    <td class="tbform-control">
                        ${HoraOperacion.HophorfinDesc}
                    </td>
                </tr>
            </table>

            <br />
            Unidades que están en mantenimiento y fuera de servicio:
            <table class="pretty tabla-icono tabla-ems" style="width: 400px;">
                <thead>
                    <tr>
                        <th style="">Unidad</th>
                        <th>Tip.<br> Interv.</th>
                        <th style="">Hora Inicio</th>
                        <th style="">Hora Fin</th>
                        <th>Disp.</th>
                        <th>Descripción</th>
                    </tr>
                </thead>
                <tbody>
                    ${htmlListado}
                </tbody>
            </table>
        </div>
    </div>

    `;

    return html
}

//////////////////////////////////////////////////////////////////////////////////////////
// Gráfico
//////////////////////////////////////////////////////////////////////////////////////////

function graficoEventoContextMenu() {
    var objItems = {};
    if (TIENE_PERMISO_ADMIN) {
        objItems = {
            /*"ver": { name: "VER" },*/
            "modificar": { name: "EDITAR" },
            "eliminar": { name: "ELIMINAR" }
        };
    } else {
        objItems = {
            "ver": { name: "VER" }
        };
    }

    $('#tabla').unbind();
    $('#tabla').contextMenu({
        selector: '.context-menu-one',
        callback: function (key, options) {
            var idHopcodi = parseInt($(this).attr('id').replace("hopGraf", "")) || 0;
            if (key == "ver") {
                var grupocodi = $(this).attr('grupocodi');
                verHOP(grupocodi);
            }
            if (key == "modificar") {
                var grupocodi = $(this).attr('grupocodi');
                editarHOP(grupocodi);
            }
            if (key == "eliminar") {
                eliminarHOP(idHopcodi);
            }
        },
        items: objItems
    });

    var objItemsHP = {};

    if (TIENE_PERMISO_ADMIN) {
        objItemsHP = {
            "convertir": { name: "CONVERTIR" }
        };
    }

    $('#tabla').contextMenu({
        selector: '.context-menu-hp',
        callback: function (key, options) {
            var grupocodi = $(this).attr('grupocodi');
            var horaInicio = $(this).attr('data-inicio');
            var horaFin = $(this).attr('data-fin');

            if (key == "convertir") {
                convertirHOProgramada(parseInt(grupocodi), horaInicio, horaFin);
            }
        },
        items: objItemsHP
    });
}

function graficoScadaContextMenu() {

    var objItemsHP = {};

    if (TIENE_PERMISO_ADMIN) {
        objItemsHP = {
            "convertir": { name: "CONVERTIR" }
        };
    }

    $('#graficoUnidNoRegScada').unbind();
    $('#graficoUnidNoRegScada').contextMenu({

        selector: '.context-menu-unr',
        callback: function (key, options) {
            var emprcodi = $(this).attr('data-empresa');
            var equipadre = $(this).attr('data-central');
            var horaInicio = $(this).attr('data-inicio');
            var horaFin = $(this).attr('data-fin');

            agregarHoraOperacionXCentral(emprcodi, equipadre, horaInicio, horaFin, TIPO_VENTANA_SCADA);
        },
        items: objItemsHP
    });
}

function graficoEmsContextMenu() {

    var objItemsHP = {};

    if (TIENE_PERMISO_ADMIN) {
        objItemsHP = {
            "convertir": { name: "CONVERTIR" }
        };
    }

    $('#graficoUnidNoRegScada, #graficoUnidNoReg').unbind();
    $('#graficoUnidNoRegScada, #graficoUnidNoReg').contextMenu({

        selector: '.context-menu-unr',
        callback: function (key, options) {
            var emprcodi = $(this).attr('data-empresa');
            var equipadre = $(this).attr('data-central');
            var horaInicio = $(this).attr('data-inicio');
            var horaFin = $(this).attr('data-fin');

            agregarHoraOperacionXCentral(emprcodi, equipadre, horaInicio, horaFin, TIPO_VENTANA_EMS);
        },
        items: objItemsHP
    });
}

function convertirHOProgramada(grupocodi, horaInicio, horaFin) {
    APP_OPCION = OPCION_EDITAR;
    ORIGEN_DETALLE = "CONVERTIR";

    //hora fin siempre debe ser 23:58 por defecto
    if (horaFin == "00:00:00") {
        horaFin = "23:58:00";
    }

    //crear objeto
    var objForm = _objInicializarBotonEditarConvertir(grupocodi, horaInicio, horaFin);
    objForm.TipoVentana = TIPO_VENTANA_YUPANA;

    //edición múltiple
    editarHOPMejora(grupocodi, objForm);
}

function agregarHoraOperacionXCentral(emprcodi, equipadre, horaInicio, horaFin, fuente) {

    APP_OPCION = OPCION_NUEVO;
    ORIGEN_DETALLE = "AGREGAR";

    //hora fin siempre debe ser 23:58 por defecto
    if (horaFin == "00:00:00") {
        horaFin = "23:58:00";
    }

    //cambiar tab a detalle
    $('#tab-container').easytabs('select', '#vistaDetalle');
    $("#div_vista_detalle_contenido").css('width', ANCHO_LISTADO_EMS + 'px');
    $("#divValidacion_div").html('');
    $("#tblValidacionOtroApp").hide();
    $(".divEdicionMasiva").html('');

    //generar objetos formularios
    var objForm = _objInicializarBotonAgregar();
    objForm.HoraIni = horaInicio;
    objForm.HoraFin = horaFin;
    objForm.IdEmpresa = emprcodi;
    objForm.IdCentralSelect = equipadre;
    objForm.TipoVentana = fuente;

    var listaObj = [];
    listaObj.push(objForm);

    //generar html y activar eventos js
    agregarListaDivFormulario(listaObj);
}

//////////////////////////////////////////////////////////////////////////////////////////
// Gráfico - Alertas
//////////////////////////////////////////////////////////////////////////////////////////

function verAlertaEmsXModo(listaHopcodipadre) {
    verAlertaGenerico(1, "ListarAlertaEmsXModo", 'Detalle de Alerta EMS por Modo de Operación', listaHopcodipadre);
}

function verAlertaScadaXModo(listaHopcodipadre) {
    verAlertaGenerico(2, "ListarAlertaScadaXModo", 'Detalle de Alerta Scada por Modo de Operación', listaHopcodipadre);
}

function verAlertaIntervencionXModo(listaHopcodipadre) {
    verAlertaGenerico(3, "ListarAlertaIntervencionXModo", 'Detalle de Alerta de Intervenciones por Modo de Operación', listaHopcodipadre);
}

function verAlertaCostoIncrementalXModo(listaHopcodipadre) {
    mostrarPopupAlertaGenericoXModo('Alerta de Costo Incremental por Modo de Operación', 'El modo de operación es de una central con costo incremental que es más cara por bajar.');
}

function verAlertaGenerico(tipoPopup, metodoController, titulo, listaHopcodipadre) {
    $('#formAlertaGenericoXHOP').html('');
    $("#popupAlertaGenericoXHOP.popup-title").html('');

    $.ajax({
        type: 'POST',
        url: controlador + metodoController,
        data: {
            sfecha: getFechaEms(),
            idEmpresa: getEmpresaEms(),
            listaHopcodipadre: listaHopcodipadre
        },
        success: function (model) {
            if (model.Resultado != -1) {
                var htmlPopup = "";
                if (tipoPopup == 1) htmlPopup = _getHtmlPopupValidacionAplicativoEmsXModo(model.HoraOperacion, model.ListaHorasOperacion, model.ListaValidacionHorasOperacionEms);
                if (tipoPopup == 2) htmlPopup = _getHtmlPopupValidacionAplicativoScadaXModo(model.HoraOperacion, model.ListaHorasOperacion, model.ListaValidacionHorasOperacionScada);
                if (tipoPopup == 3) htmlPopup = _getHtmlPopupValidacionAplicativoIntervencionXModo(model.HoraOperacion, model.ListaHorasOperacion, model.ListaValidacionHorasOperacionIntervencion);

                mostrarPopupAlertaGenericoXModo(titulo, htmlPopup);
            } else {
                alert("Ha ocurrido un error.");
            }
        },
        error: function (err) {
            alert("Se perdió la conexión usuario/servidor COES.");
        }
    });
}

function _getHtmlPopupValidacionAplicativoEmsXModo(HoraOperacion, ListaHorasOperacion, ListaValidacionHorasOperacionEms) {
    var html = "";

    var htmlListado = "";

    for (var i = 0; i < ListaHorasOperacion.length; i++) {
        var item = ListaHorasOperacion[i];

        htmlListado += `
        <table class="pretty tabla-icono tabla-ems" style="width: 400px;">
                <thead>
                    <tr>
                        <th style="">EN PARALELO:</th>
                        <th style="background-color: white !important; color: #335873;" colspan="2">${item.HophoriniDesc}</th>
                    </tr>
                    <tr>
                        <th style="">FIN DE REGISTRO:</th>
                        <th style="background-color: white !important; color: #335873;" colspan="2">${item.HophorfinDesc}</th>
                    </tr>
                    <tr>
                        <th style="">Unidad</th>
                        <th style="">Hora Inicio</th>
                        <th style="">Hora Fin</th>
                    </tr>
                </thead>
                <tbody>
        `;

        var lista = ListaValidacionHorasOperacionEms.filter(function (elemento) {
            return elemento.HoraOperacion.Hopcodi === item.Hopcodi;
        });

        for (var j = 0; j < lista.length; j++) {
            var itemNoOpero = lista[j];

            htmlListado += `
                        <tr>
                            <td>${itemNoOpero.Equinomb}</td>
                            <td>${itemNoOpero.FechaIniDesc}</td>
                            <td>${itemNoOpero.FechaFinDesc}</td>
                        </tr>
            `;
        }


        htmlListado += `
                </tbody>
            </table>

            <br />

        `;

    }


    html += `

    <div class='panel-container'>
        <div class="form-search">

            <table style="width:100%;margin-right: auto" class="table-form-show">
                <tr>
                    <td class="tbform-label" style="width: 117px;">Empresa:</td>
                    <td class="tbform-control">
                        ${HoraOperacion.Emprnomb}
                    </td>
                </tr>
                <tr>
                    <td class="tbform-label">Central:</td>
                    <td class="tbform-control">
                        ${HoraOperacion.Central}
                    </td>
                </tr>
                <tr>
                    <td class="tbform-label">Modo de Operación:</td>
                    <td class="tbform-control">
                        ${HoraOperacion.Grupoabrev}
                    </td>
                </tr>
            </table>

            <br />
            Unidades que tienen estado Inactivo por cada Hora de Operación:

            ${htmlListado}
        </div>
    </div>

    `;

    return html
}

function _getHtmlPopupValidacionAplicativoScadaXModo(HoraOperacion, ListaHorasOperacion, ListaValidacionHorasOperacionScada) {
    var html = "";

    var htmlListado = "";

    for (var i = 0; i < ListaHorasOperacion.length; i++) {
        var item = ListaHorasOperacion[i];

        htmlListado += `
        <table class="pretty tabla-icono tabla-ems" style="width: 400px;">
                <thead>
                    <tr>
                        <th style="">EN PARALELO:</th>
                        <th style="background-color: white !important; color: #335873;" colspan="2">${item.HophoriniDesc}</th>
                    </tr>
                    <tr>
                        <th style="">FIN DE REGISTRO:</th>
                        <th style="background-color: white !important; color: #335873;" colspan="2">${item.HophorfinDesc}</th>
                    </tr>
                    <tr>
                        <th style="">Unidad</th>
                        <th style="">Hora Inicio</th>
                        <th style="">Hora Fin</th>
                    </tr>
                </thead>
                <tbody>
        `;

        var lista = ListaValidacionHorasOperacionScada.filter(function (elemento) {
            return elemento.HoraOperacion.Hopcodi === item.Hopcodi;
        });

        for (var j = 0; j < lista.length; j++) {
            var itemNoOpero = lista[j];

            htmlListado += `
                        <tr>
                            <td>${itemNoOpero.Equinomb}</td>
                            <td>${itemNoOpero.FechaIniDesc}</td>
                            <td>${itemNoOpero.FechaFinDesc}</td>
                        </tr>
            `;
        }

        htmlListado += `
                </tbody>
            </table>

            <br />

        `;
    }

    html += `

    <div class='panel-container'>
        <div class="form-search">

            <table style="width:100%;margin-right: auto" class="table-form-show">
                <tr>
                    <td class="tbform-label" style="width: 117px;">Empresa:</td>
                    <td class="tbform-control">
                        ${HoraOperacion.Emprnomb}
                    </td>
                </tr>
                <tr>
                    <td class="tbform-label">Central:</td>
                    <td class="tbform-control">
                        ${HoraOperacion.Central}
                    </td>
                </tr>
                <tr>
                    <td class="tbform-label">Modo de Operación:</td>
                    <td class="tbform-control">
                        ${HoraOperacion.Grupoabrev}
                    </td>
                </tr>
            </table>

            <br />
            Unidades que no presentan señal Scada por cada Hora de Operación:

            ${htmlListado}
        </div>
    </div>

    `;

    return html
}

function _getHtmlPopupValidacionAplicativoIntervencionXModo(HoraOperacion, ListaHorasOperacion, ListaValidacionHorasOperacionIntervencion) {
    var html = "";

    var htmlListado = "";

    for (var i = 0; i < ListaHorasOperacion.length; i++) {
        var item = ListaHorasOperacion[i];

        htmlListado += `
        <table class="pretty tabla-icono tabla-ems" style="width: 400px;">
                <thead>
                    <tr>
                        <th style="">EN PARALELO:</th>
                        <th style="background-color: white !important; color: #335873;" colspan="2">${item.HophoriniDesc}</th>
                    </tr>
                    <tr>
                        <th style="">FIN DE REGISTRO:</th>
                        <th style="background-color: white !important; color: #335873;" colspan="2">${item.HophorfinDesc}</th>
                    </tr>
                    <tr>
                        <th style="background-color: #9370DB;">Unidad</th>
                        <th style="background-color: #9370DB;">Intervención <br/> Hora Inicio </th>
                        <th style="background-color: #9370DB;">Intervención <br/> Hora Fin</th>
                    </tr>
                </thead>
                <tbody>
        `;

        var lista = ListaValidacionHorasOperacionIntervencion.filter(function (elemento) {
            return elemento.HoraOperacion.Hopcodi === item.Hopcodi;
        });

        for (var j = 0; j < lista.length; j++) {
            var itemNoOpero = lista[j];

            htmlListado += `
                        <tr>
                            <td>${itemNoOpero.Equinomb}</td>
                            <td>${itemNoOpero.FechaIniDesc}</td>
                            <td>${itemNoOpero.FechaFinDesc}</td>
                        </tr>
            `;
        }

        htmlListado += `
                </tbody>
            </table>

            <br />

        `;
    }

    html += `

    <div class='panel-container'>
        <div class="form-search">

            <table style="width:100%;margin-right: auto" class="table-form-show">
                <tr>
                    <td class="tbform-label" style="width: 117px;">Empresa:</td>
                    <td class="tbform-control">
                        ${HoraOperacion.Emprnomb}
                    </td>
                </tr>
                <tr>
                    <td class="tbform-label">Central:</td>
                    <td class="tbform-control">
                        ${HoraOperacion.Central}
                    </td>
                </tr>
                <tr>
                    <td class="tbform-label">Modo de Operación:</td>
                    <td class="tbform-control">
                        ${HoraOperacion.Grupoabrev}
                    </td>
                </tr>
            </table>

            <br />
            Unidades que están en mantenimiento y fuera de servicio por cada Hora de Operación:

            ${htmlListado}
        </div>
    </div>

    `;

    return html
}

function mostrarPopupAlertaGenericoXModo(titulo, dataHtml) {
    $('#formAlertaGenericoXHOP').html(dataHtml);
    $("#popupAlertaGenericoXHOP.popup-title").html(titulo);

    var excep_resultado = parseInt($("#hdResultado").val()) || 0;
    var excep_mensaje = $("#hdMensaje").val();
    var excep_detalle = $("#hdDetalle").val();

    if (excep_resultado == -1) {
        alert(excep_mensaje);
    } else {
        setTimeout(function () {
            $('#popupAlertaGenericoXHOP').bPopup({
                easing: 'easeOutBack',
                speed: 450,
                transition: 'slideDown',
                modalClose: true
            });
        }, 50);
    }
}

//////////////////////////////////////////////////////////////////////////////////////////
// Pestaña - Costos térmicos (variables e incremental)
//////////////////////////////////////////////////////////////////////////////////////////

function _generarListarReporteHopCTHtml(listaData) {
    var html = '';

    html += `
    <div class='freeze_table' id='resultado_tbl_ct' style=''>
    <table id='tbResultadoCT' class='pretty tabla-icono' style='table-layout: fixed;' >
        <thead>
            <tr>
                <th style='width:20%'>Empresa</th>
                <th style='width:20%'>Central</th>
                <th style='width:20%'>Modo de Operación</th>
                <th style='width:8%'>CV (S/. / MWh)</th>
                <th style='width:8%'>Tramo 1</th>
                <th style='width:8%'>CI 1 (S/. / MWh)</th>
                <th style='width:7%'>Tramo 2</th>
                <th style='width:7%'>CI 2 (S/. / MWh)</th>
            </tr>
        </thead>
        <tbody>
    `;

    if (listaData.length > 0) {
        for (var i = 0; i < listaData.length; i++) {
            var reg = listaData[i];
            var claseAct = reg.FlagActivo ? "flagActivo" : "";
            html += `
            <tr style='background-color: ${CENTRAL_COLOR_DEFAULT}' class="${claseAct}">
                <td class='class_hop_ems hor_empresa'>
                    <div title="${reg.Emprnomb}" style="overflow:hidden;white-space:nowrap;">${reg.Emprnomb}</div>
                </td>
                <td class='class_hop_ems hor_central'>${reg.Central}</td>
                <td class='class_hop_ems hor_grupo'>${reg.Gruponomb}</td>
                <td class='class_hop_ems' data-order="${reg.CVariable1}">${reg.CVariable1Formateado}</td>
                <td class='class_hop_ems'>${reg.Tramo1}</td>
                <td class='class_hop_ems' data-order="${reg.CIncremental1}">${reg.CIncremental1Formateado}</td>
                <td class='class_hop_ems'>${reg.Tramo2}</td>
                <td class='class_hop_ems' data-order="${reg.CIncremental2}">${reg.CIncremental2Formateado}</td>
            </tr>
            `;
        }

    } else {
        html += `
            <tr>
                <td colspan='8' style='text-align: left'>¡No existen Horas de Operación registradas!</td>
            </tr>
        `;
    }

    html += `
        </tbody>
    </table>
    </div>
        `;

    return html;
}

function generarExcelReporteCT() {



    var dataListado = obtenerListaCTExportacion();
    var dataJson = {
        fecha: getFechaEms(),
        listaModosOperacionCT: dataListado
    };

    //limpiarBarraMensaje('mensaje');
    $.ajax({
        url: controlador + "ExportarReporteCostosCT",
        type: 'POST',
        contentType: 'application/json; charset=UTF-8',
        dataType: 'json',
        data: JSON.stringify(dataJson),
        success: function (evt) {
            if (evt.Resultado != -1) {

                window.location = controlador + "AbrirArchivo?file=" + evt.NombreArchivo;
            }
            else {
                alert('Ha ocurrido un error');
            }
        },
        error: function (err) {
            alert("Error en la exportación del reporte");
        }
    });
}

function obtenerListaCTExportacion() {
    var lstData = [];
    for (var i = 0; i < GLOBAL_HO.ListaModosOperacionCT.length; i++) {
        var item = GLOBAL_HO.ListaModosOperacionCT[i];

        var fila = {};
        fila.FlagActivo = item.FlagActivo;
        fila.Emprnomb = item.Emprnomb;
        fila.Central = item.Central;
        fila.Gruponomb = item.Gruponomb;
        fila.CVariable1 = item.CVariable1;
        fila.CVariable1Formateado = item.CVariable1Formateado;
        fila.CIncremental1 = item.CIncremental1;
        fila.CIncremental1Formateado = item.CIncremental1Formateado;
        fila.CIncremental2 = item.CIncremental2;
        fila.CIncremental2Formateado = item.CIncremental2Formateado;
        fila.Tramo1 = item.Tramo1;
        fila.Tramo2 = item.Tramo2;

        lstData.push(fila);
    }
    return lstData;
}

//////////////////////////////////////////////////////////////////////////////////////////
// Iniciar Día
//////////////////////////////////////////////////////////////////////////////////////////
function registrarNuevoDia(msgPrevio) {
    if (!existeModificacionesHOP()) {
        $.ajax({
            type: 'POST',
            url: controlador + 'ListarModoOrdParadaContinuarNuevoDia',
            data: {
                sfecha: getFechaEms()
            },
            success: function (evt) {
                if (evt.Resultado != -1) {

                    if (evt.ListaHorasOperacion.length > 0)
                        mostrarPopupNuevoDia(evt.ListaHorasOperacion);
                    else
                        registrarBDNuevoDia([], msgPrevio);
                } else {
                    alert("Error:" + evt.Descripcion);
                }
            },
            error: function (err) {
                alert("Se perdió la conexión usuario/servidor COES, no llegó la información al servidor.");
            }
        });
    }
}

function mostrarPopupNuevoDia(listaHo) {
    $('#divPopupListaHoOrdParada').html(dibujarTablaListaHoOrdParada(listaHo));
    setTimeout(function () {
        $('#popupListaHoOrdParada').bPopup({
            easing: 'easeOutBack',
            speed: 450,
            transition: 'slideDown',
            modalClose: false
        });

        $('#tablaListaHoOrdParada').dataTable({
            "scrollX": true,
            "sDom": 't',
            "ordering": false,
            "bPaginate": false,
            "iDisplayLength": -1
        });
    }, 50);

    $("#btnGuardarListaHoSeleccionada").unbind();
    $("#btnGuardarListaHoSeleccionada").click(function () {
        var listaHopcodi = [];
        $("input[name=chkHopcodiOrdParada]:checked").each(function () {
            listaHopcodi.push(this.id)
        });

        $('#popupListaHoOrdParada').bPopup().close();
        registrarBDNuevoDia(listaHopcodi, "");
    });
}

function dibujarTablaListaHoOrdParada(listaHo) {
    var cadena = "";
    cadena += "<span  style='margin-top: 10px;'>Seleccione las horas de operación que continuarán el día de hoy</span>";
    cadena += "<table border='1' class='pretty tabla-horas' cellspacing='0' width='100%' id='tablaListaHoOrdParada'>";
    cadena += "<thead>";
    cadena += "<tr>";
    cadena += "<th>EMPRESA</th>";
    cadena += "<th>CENTRAL</th>";
    cadena += "<th>MODO OPERACIÓN</th>";
    cadena += "<th>EN PARALELO</th>";
    cadena += "<th>ORDEN DE PARADA</th>";
    cadena += "<th>FIN REGISTRO</th>";
    cadena += "<th></th>";
    cadena += "</tr></thead>";

    cadena += "<tbody>";

    for (var kfil = 0; kfil < listaHo.length; kfil++) {
        var horaIni = moment(listaHo[kfil].Hophorini).format('HH:mm:ss');
        var horaFin = moment(listaHo[kfil].Hophorfin).format('HH:mm:ss');
        Hophorparada = "";
        if (listaHo[kfil].Hophorparada != null && listaHo[kfil].Hophorparada != "") {
            Hophorparada = moment(listaHo[kfil].Hophorparada).format('HH:mm:ss');
        }
        cadena += "<td>" + listaHo[kfil].Emprnomb + "</td>";
        cadena += "<td>" + listaHo[kfil].Central + "</td>";
        cadena += "<td>" + listaHo[kfil].Grupoabrev + "</td>";
        cadena += "<td>" + horaIni + "</td>"; //EN PARALELO
        cadena += "<td>" + Hophorparada + "</td>"; //ORDEN PARADA
        cadena += "<td>" + horaFin + "</td>"; //FIN REGISTRO
        cadena += '<td><input name="chkHopcodiOrdParada" type="checkbox" id="' + listaHo[kfil].Hopcodi + '" cheked/></td>';
        cadena += "</tr>";
    }

    cadena += "</tbody></table>";

    return cadena;
}

function registrarBDNuevoDia(listaHopcodi, msgPrevio) {
    var fechaSig = moment(getFechaEms(), 'DD/MM/YYYY').add('days', 1);
    var textFechaSig = moment(fechaSig).format('DD/MM/YYYY');

    if (!existeModificacionesHOP()) {
        $.ajax({
            type: 'POST',
            url: controlador + 'RegistrarNuevoDia',
            data: {
                sfecha: textFechaSig,
                strHopcodiPermitido: listaHopcodi.join(',')
            },
            success: function (evt) {

                msgPrevio += "\n" + evt[1];

                if (evt.Error == undefined) {
                    if (evt[0] == "1") {
                        alert(msgPrevio);

                        $("#txtFecha").val(textFechaSig);
                        cargarVista();
                    } else {
                        alert(msgPrevio);
                    }
                } else {
                    alert("Error:" + evt.Descripcion);
                }
            },
            error: function (err) {
                alert("Se perdió la conexión usuario/servidor COES, no llegó la información al servidor.");
            }
        });
    }
}

//////////////////////////////////////////////////////////////////////////////////////////
// Continuar Día
//////////////////////////////////////////////////////////////////////////////////////////
function continuarDia() {
    finalizarDia(true);
}

//////////////////////////////////////////////////////////////////////////////////////////
// Finalizar Día
//////////////////////////////////////////////////////////////////////////////////////////
function finalizarDia(continuar) {
    if (!existeModificacionesHOP()) {
        $.ajax({
            type: 'POST',
            url: controlador + 'FinalizarDia',
            data: {
                sfecha: getFechaEms(),
                continuar: continuar
            },
            success: function (evt) {
                if (evt.Error == undefined) {

                    var msg = evt[1];

                    registrarNuevoDia(msg);

                } else {
                    alert("Error:" + evt.Descripcion);
                }
            },
            error: function (err) {
                alert("Se perdió la conexión usuario/servidor COES, no llegó la información al servidor.");
            }
        });
    }
}

//////////////////////////////////////////////////////////////////////////////////////////
// Color
//////////////////////////////////////////////////////////////////////////////////////////
function redireccionarCentralColor() {
    location.href = siteRoot + "IEOD/Parametro/IndexParametroHo/";
};

//////////////////////////////////////////////////////////////////////////////////////////
// Reporte
//////////////////////////////////////////////////////////////////////////////////////////
function redireccionarReporteHOP() {
    location.href = controlador + "Reporte";
};

//////////////////////////////////////////////////////////////////////////////////////////
// Listado de correo
//////////////////////////////////////////////////////////////////////////////////////////
function verContenido(id) {
    $.ajax({
        type: 'POST',
        url: controlador + 'VerContenidoCorreo',
        data: {
            idCorreo: id
        },
        dataType: 'json',
        success: function (model) {
            if (model.Resultado != -1) {
                $('#contenidoLogCorreo').html(model.Mensaje);
                setTimeout(function () {
                    $('#popupLogCorreo').bPopup({
                        autoClose: false
                    });
                }, 50);
            }
            else {
                alert(model.Message);
            }
        },
        error: function (err) {
            alert("Se perdió la conexión usuario/servidor COES.");
        }
    });
}

//////////////////////////////////////////////////////////////////////////////////////////
// Listado de envíos
//////////////////////////////////////////////////////////////////////////////////////////

function popUpListaEnvios() {
    $('#idEnviosAnteriores').html(dibujarTablaEnvios(GLOBAL_HO.ListaEnvios));
    setTimeout(function () {
        $('#enviosanteriores').bPopup({
            easing: 'easeOutBack',
            speed: 450,
            transition: 'slideDown',
            modalClose: false
        });
        $('#tablalenvio').dataTable({
            "scrollY": 330,
            "scrollX": true,
            "sDom": 't',
            "ordering": false,
            "bPaginate": false,
            "iDisplayLength": -1
        });
    }, 50);
}

// Muestra los envios anteriores
function dibujarTablaEnvios(lista) {

    var cadena = "<div style='clear:both; height:5px'></div> ";
    cadena += "<table id='tablalenvio' border='1' class='pretty tabla-adicional' cellspacing='0'>";
    cadena += "<thead><tr><th>Id Envío</th><th>Fecha Hora</th><th>Usuario</th></tr></thead>";
    cadena += "<tbody>";

    for (key in lista) {
        var javaScriptDate = new Date(moment(lista[key].Enviofecha));

        cadena += "<tr onclick='cargarVista(" + lista[key].Enviocodi + ");' style='cursor:pointer'><td>" + lista[key].Enviocodi + "</td>";
        cadena += "<td>" + getFormattedDate(javaScriptDate) + "</td>";
        cadena += "<td>" + lista[key].Lastuser + "</td></tr>";
    }
    cadena += "</tbody></table>";
    return cadena;

}

//////////////////////////////////////////////////////////////////////////////////////////
// Util
//////////////////////////////////////////////////////////////////////////////////////////
function getFechaEms() {
    return $('#txtFecha').val();
}

function getEmpresaEms() {
    return $('#cbEmpresa').val();
}

function refrehDatatable() {

    $('#tbResultadoCT').dataTable({
        "scrollX": false,
        "scrollCollapse": false,
        "destroy": "true",
        "sDom": 't',
        "scrollY": (ALTO_VISIBLE_LISTADO_EMS - 55) >= ALTO_VISIBLE_LISTADO_EMS_DEFAULT ? (ALTO_VISIBLE_LISTADO_EMS - 55) + "px" : "100%",
        "bPaginate": false,
        "searching": false,
        order: [[5, 'asc']],
    });
}
