var controlador = siteRoot + 'Titularidad/Transferencia/';

$(function () {
    $('#btnIrAlListado').click(function () {
        document.location.href = siteRoot + 'Titularidad/Transferencia/index';
    });

    $('#btnProcesar').click(function () {
        procesarTransferencia();
    });

    $("#stepTransf").show();
    $("#stepTransf").children("div").steps({
        headerTag: "h3",
        bodyTag: "div",
        transitionEffect: "slideLeft",
        onStepChanging: function (event, currentIndex, newIndex) {
            $('#mensaje').css("display", "none");

            var numStep = parseInt(currentIndex) || 0;
            var bRes = false;
            switch (numStep) {
                case STEP_FORMULARIO:
                    var bRes = validarExistenciaTransferencia();
                    if (OBJETO_DATA.actualizarInstalaciones) {
                        listarDatosXEmpresa();
                    }
                    mostrarInterfazCabecera();
                    mostrarInterfazEquipos();
                    break;
                case STEP_EQUIPO:
                    if (newIndex > 0)
                        mostrarInterfazResumen();
                    return true;
                case STEP_PROCESAR:
                    return true;
            }

            return bRes;
        },
        onStepChanged: function (event, currentIndex, priorIndex) {
            var numStep = parseInt(currentIndex) || 0;
            switch (numStep) {
                case STEP_FORMULARIO:
                    $("#steps-uid-0-p-0").parent().css('height', '651px');
                    $('#resumen_transf').hide();
                    break;
                case STEP_EQUIPO:
                    $("#steps-uid-0-p-1").parent().css('height', '690px');
                    $("#" + PREFIJO_EMPRESA_ORIGEN + "tablaEquipo").DataTable().draw();
                    $("#" + PREFIJO_EMPRESA_DESTINO + "tablaEquipo").DataTable().draw();
                    break;
                case STEP_PROCESAR:
                    //
                    var obj = getObjTransferencia();
                    var msjVal = val_objTransferencia(obj);

                    if (msjVal == '')
                        msjVal = val_ExisteEquiposTransf(obj);
                    if (msjVal != '') {
                        $("#steps-uid-0 div.actions.clearfix ul li:eq(2)").hide();
                        mostrarError(msjVal);
                    } else {
                        $("#steps-uid-0 div.actions.clearfix ul li:eq(2)").show();
                    }

                    //
                    $("#steps-uid-0-p-2").parent().css('height', '522px');
                    break;
            }
        },
        onFinishing: function (event, currentIndex) {
            $('#mensaje').css("display", "none");
            procesarTransferencia();
            return true;
        },
        onFinished: function (event, currentIndex) {
        },
        labels: {
            previous: "Anterior",
            next: "Siguiente",
            finish: "Procesar"
        }
    });


    $('#txtFecha').Zebra_DatePicker({
        direction: ['01/01/1900', $('#hdFecha').val()]
    });
    $('#final_txtFecha').Zebra_DatePicker({
        direction: 0
    });

    $('#cbTipoMigracion').on('change', function () {
        mostrarInterfazTipoMigracion();
    });

    $('#orig_empr_codigo').multipleSelect({
        width: '683px',
        filter: true,
        single: true,
        onClose: function () {
            mostrarDatosEmpresa(TIPO_EMPRESA_ORIGEN);
        }
    });
    $('#dest_empr_codigo').multipleSelect({
        width: '683px',
        filter: true,
        single: true,
        onClose: function () {
            mostrarDatosEmpresa(TIPO_EMPRESA_DESTINO);
        }
    });
    $("#orig_empr_codigo").multipleSelect("setSelects", [0]);
    $("#dest_empr_codigo").multipleSelect("setSelects", [0]);

    mostrarInterfazTipoMigracion();
});

////////////////////////////////////////////////////////////////////////////////////////////////////
/// Cargar datos
////////////////////////////////////////////////////////////////////////////////////////////////////

function mostrarInterfazTipoMigracion() {
    var tipoMigraOp = getObjTransferencia().tipoMigraOp;

    $("#div_tipo_operacion").show();
    $(".fecha_form").hide();
    $("#orig_field_empresa").hide();
    $("#dest_field_empresa").hide();

    $("#orig_field_empresa legend").html('Origen');
    $("#dest_field_empresa legend").html('Destino');

    switch (tipoMigraOp) {
        case TIPO_MIGR_DUPLICIDAD:
        case TIPO_MIGR_INSTALACION_NO_CORRESPONDEN:
            $("#orig_field_empresa").show();
            $("#dest_field_empresa").show();
            break;
        case TIPO_MIGR_CAMBIO_RAZON_SOCIAL:
        case TIPO_MIGR_FUSION:
        case TIPO_MIGR_TRANSFERENCIA:
            $(".fecha_form").show();
            $("#orig_field_empresa").show();
            $("#dest_field_empresa").show();
            break;
    }

    mostrarNotaTipoMigracion(tipoMigraOp);
}

function mostrarNotaTipoMigracion(tipoMigraOp) {
    $("#nota_x_tipo_form").html('');
    var htmlNota = '';

    switch (tipoMigraOp) {
        case TIPO_MIGR_DUPLICIDAD:
            htmlNota = NOTA_MIGR_DUPLICIDAD;
            break;
        case TIPO_MIGR_INSTALACION_NO_CORRESPONDEN:
            htmlNota = NOTA_MIGR_INSTALACION_NO_CORRESPONDEN;
            break;
        case TIPO_MIGR_CAMBIO_RAZON_SOCIAL:
            htmlNota = NOTA_MIGR_CAMBIO_RAZON_SOCIAL;
            break;
        case TIPO_MIGR_FUSION:
            htmlNota = NOTA_MIGR_FUSION;
            break;
        case TIPO_MIGR_TRANSFERENCIA:
            htmlNota = NOTA_MIGR_TRANSFERENCIA;
            break;
    }

    $("#nota_x_tipo_form").html(htmlNota);
}

function mostrarDatosEmpresa(tipo) {
    var pref = getPrefijoXTipo(tipo);

    limpiarDatosEmpresa(tipo);

    var idEmpresa = getEmpresaByTipo(tipo);

    if (idEmpresa > 0) {
        $.ajax({
            type: 'POST',
            url: controlador + 'ObtenerEmpresaById',
            data: { idEmpresa: idEmpresa },
            dataType: 'json',
            cache: false,
            success: function (result) {
                if (result.Resultado != "-1") {
                    $("#" + pref + "empr_ruc").val(result.Empresa.Emprruc);
                    $("#" + pref + "empr_razon").val(result.Empresa.Emprrazsocial);

                    $("#" + pref + "empr_codi").val(result.Empresa.Emprcodi);
                    $("#" + pref + "empr_abrev").val(result.Empresa.Emprabrev);
                    $("#" + pref + "empr_nombre").val(result.Empresa.Emprnomb);

                    $("#" + pref + "empr_estado").val(result.Empresa.EmprestadoDesc);
                    $("#" + pref + "empr_agente").val(result.Empresa.EmprseinDesc);
                    $("#" + pref + "empr_coes").val(result.Empresa.EmprcoesDesc);
                    $("#" + pref + "empr_tipo").val(result.Empresa.Tipoemprdesc);

                    var est_backcolor = '';
                    var est_color = '';
                    switch (result.Empresa.Emprestado) {
                        case ESTADO_EMP_ACTIVO:
                            est_backcolor = '#2184be';
                            est_color = '#fff';
                            break;
                        case ESTADO_EMP_BAJA:
                            est_backcolor = '#FFDDDD';
                            est_color = '#4876AA';
                            break;
                        case ESTADO_EMP_ELIMINADO:
                            est_backcolor = '#A4A4A4';
                            est_color = '#FFFFFF';
                            break;
                    }
                    $("#" + pref + "empr_estado").css('background-color', est_backcolor);
                    $("#" + pref + "empr_estado").css('color', est_color);

                    var agente_backcolor = result.Empresa.Emprsein == 'S' ? '#43a243' : '#fd4444';
                    var agente_color = 'white';
                    $("#" + pref + "empr_agente").css('background-color', agente_backcolor);
                    $("#" + pref + "empr_agente").css('color', agente_color);

                    var coes_backcolor = result.Empresa.Emprcoes == 'S' ? '#43a243' : '#fd4444';
                    var coes_color = 'white';
                    $("#" + pref + "empr_coes").css('background-color', coes_backcolor);
                    $("#" + pref + "empr_coes").css('color', coes_color);

                    setObjEmpresa(result.Empresa);
                } else {
                    alert('Ha ocurrido un error');
                }
            },
            error: function (err) {
                alert('Ha ocurrido un error');
            }
        });
    }
};

function limpiarDatosEmpresa(tipo) {
    var pref = getPrefijoXTipo(tipo);
    $("#" + pref + "empr_ruc").val('');
    $("#" + pref + "empr_razon").val('');

    $("#" + pref + "empr_codi").val('');
    $("#" + pref + "empr_abrev").val('');
    $("#" + pref + "empr_nombre").val('');

    $("#" + pref + "empr_estado").val('');
    $("#" + pref + "empr_agente").val('');
    $("#" + pref + "empr_coes").val('');
    $("#" + pref + "empr_tipo").val('');

    var est_backcolor = '#F2F4F3';
    var est_color = '#4876AA';
    $("#" + pref + "empr_estado").css('background-color', est_backcolor);
    $("#" + pref + "empr_estado").css('color', est_color);

    var agente_backcolor = '#F2F4F3';
    var agente_color = '#4876AA';
    $("#" + pref + "empr_agente").css('background-color', agente_backcolor);
    $("#" + pref + "empr_agente").css('color', agente_color);

    var coes_backcolor = '#F2F4F3';
    var coes_color = '#4876AA';
    $("#" + pref + "empr_coes").css('background-color', coes_backcolor);
    $("#" + pref + "empr_coes").css('color', coes_color);
}

//
function listarDatosXEmpresa() {
    //inicializar variables por tipo
    OBJETO_DATA.origen = generarObjTransf(TIPO_EMPRESA_ORIGEN);
    OBJETO_DATA.destino = generarObjTransf(TIPO_EMPRESA_DESTINO);

    var idEmpresa = getEmpresa();

    $.ajax({
        type: 'POST',
        url: controlador + 'ListarDatosXEmpresa',
        data: { idEmpresa: idEmpresa },
        dataType: 'json',
        cache: false,
        success: function (obj) {
            if (obj.Resultado == "1") {
                var str1 = JSON.stringify(obj.ListaEquipo);
                var str2 = JSON.stringify(obj.ListaArea);
                var str3 = JSON.stringify(obj.ListaFamilia);

                OBJETO_DATA.origen.DATA_INICIAL_AREA = JSON.parse(str2);
                OBJETO_DATA.origen.DATA_INICIAL_FAMILIA = JSON.parse(str3);
                OBJETO_DATA.origen.DATA_INICIAL_EQUIPO = JSON.parse(str1);

                OBJETO_DATA.destino.DATA_INICIAL_AREA = JSON.parse(str2);
                OBJETO_DATA.destino.DATA_INICIAL_FAMILIA = JSON.parse(str3);

                if (tieneTransferirEmpresa(OBJETO_DATA.tipoMigraOp)) {
                    OBJETO_DATA.destino.DATA_INICIAL_EQUIPO = JSON.parse(str1);
                    OBJETO_DATA.destino.DATA_INICIAL_EQUIPO = eq_buscarXFiltro(TIPO_EMPRESA_DESTINO, ESTADO_EQ_NO_ELIMINADO);

                    OBJETO_DATA.origen.DATA_INICIAL_EQUIPO = eq_buscarXFiltro(TIPO_EMPRESA_ORIGEN, ESTADO_EQ_ELIMINADO);
                }

                mostrarInterfazEquipos();
                actualizarTabEquipo();
            }
        },
        error: function (err) {
            alert('Ha ocurrido un error');
        }
    });
}

function mostrarInterfazCabecera() {
    $('#final_cbTipoMigracionORegistrar').val(OBJETO_DATA.tipoMigraOp);
    $('#final_txtFecha').val(OBJETO_DATA.strFechaCorte);
    $('#final_orig_empr_codigo').val(OBJETO_DATA.emprcodiOrigen);
    $('#final_dest_empr_codigo').val(OBJETO_DATA.emprcodi);

    $(".final_fecha_form").hide();
    if (tieneFechaCorte(OBJETO_DATA.tipoMigraOp))
        $(".final_fecha_form").show();

    $('#resumen_transf').show();
}

function mostrarInterfazEquipos() {
    $("#orig_div_empresa").show();
    $("#orig_div_empresa").css('float', 'left');
    $("#dest_div_empresa").show();
    $("#dest_div_empresa").css('float', 'left');
    $("#eq_btnMoveLeft").show();
    $("#eq_btnMoveRight").show();

    if (tieneTransferirEmpresa(OBJETO_DATA.tipoMigraOp)) {
        $("#orig_div_empresa").hide();
        $("#dest_div_empresa").css('float', 'right');
        $("#eq_btnMoveLeft").hide();
        $("#eq_btnMoveRight").hide();

        if (esValidoLista(OBJETO_DATA.origen.DATA_INICIAL_EQUIPO)) {
            $("#orig_div_empresa").show();
        }
    }
}

function mostrarInterfazResumen() {
    $("#div_procesar").html('');
    var obj = getObjTransferencia();
    var msjVal = val_objTransferencia(obj);

    if (msjVal == '') {
        msjVal = val_ExisteEquiposTransf(obj);
        if (msjVal == '') {
            $.ajax({
                type: 'POST',
                url: controlador + 'GenerarViewProcesarHtml',
                data: {
                    strTransf: getJsonObjTransf(obj)
                },
                dataType: 'json',
                cache: false,
                success: function (result) {
                    if (result.Resultado != "-1") {
                        $("#div_procesar").html(result.Resultado);

                        $('#tabla_familias').dataTable({
                            "sPaginationType": "full_numbers",
                            "stripeClasses": [],
                            "ordering": true,
                            "iDisplayLength": 15
                        });
                    } else {
                        mostrarError(result.Mensaje);
                    }
                },
                error: function (err) {
                    alert('Ha ocurrido un error');
                }
            });
        }
    }

    if (msjVal != '') {
        $("#steps-uid-0 div.actions.clearfix ul li:eq(2)").hide();
        mostrarError(msjVal);
    } else {
        $("#steps-uid-0 div.actions.clearfix ul li:eq(2)").show();
    }
}

function actualizarTabEquipo() {
    eq_ui_generarView(TIPO_EMPRESA_ORIGEN);
    eq_ui_configTabla(TIPO_EMPRESA_ORIGEN);

    eq_ui_generarView(TIPO_EMPRESA_DESTINO);
    eq_ui_configTabla(TIPO_EMPRESA_DESTINO);

    $("#eq_btnMoveRight").unbind();
    $("#eq_btnMoveRight").on("click", function (e) {
        eq_data_Transferir();
    });

    $("#eq_btnMoveLeft").unbind();
    $("#eq_btnMoveLeft").on("click", function (e) {
        eq_data_Regresar();
    });
}

/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
/// Tab Equipo
/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

function eq_ui_generarView(tipo) {
    var pref = getPrefijoXTipo(tipo);

    var htmlTipoEquipo = eq_ui_generarTablaTipo(tipo);
    var htmlArea = eq_ui_generarTablaArea(tipo);
    var htmlEquipo = eq_ui_generarTabla(tipo);

    var html = '';
    html += `        
        <div class="search-content" style="padding:10px; margin-bottom:5px" id="${pref}cntTipoEquipo">
            ${htmlTipoEquipo}
        </div>
        <table style="width: 100%">
            <tr>
                <td style="width:30%; vertical-align:top">
                    <div id="${pref}cntArea" class="cntArea">
                        ${htmlArea}
                    </div>
                </td>
                <td style="width:3%"></td>
                <td style="width:67%;vertical-align:top">
                    <div id="${pref}cntEquipo" style="width:100%">
                        ${htmlEquipo}
                    </div>
                </td>
            </tr>
        </table>
    `;

    $("#" + pref + "equipos").html(html);

    $("#" + pref + "tblFormEquipo").show();

    $("#" + pref + "tablaArea").dataTable({
        "sPaginationType": "full_numbers",
        "destroy": "true",
        "bInfo": false,
        "bLengthChange": false,
        "oLanguage": {
            "sEmptyTable": "No existen registros"
        }
    });
}

function eq_mostrarByTipo(tipo, areacodi, tipoEquipo) {
    setAreacodi(tipo, areacodi);
    setFamcodi(tipo, tipoEquipo);

    var pref = getPrefijoXTipo(tipo);
    $("#" + pref + "cntEquipo").html('');
    var htmlEquipo = eq_ui_generarTabla(tipo);
    $("#" + pref + "cntEquipo").html(htmlEquipo);

    eq_ui_configTabla(tipo);
}

function eq_ui_generarTablaTipo(tipo) {
    var pref = getPrefijoXTipo(tipo);
    var listaFamilia = OBJETO_DATA.origen.DATA_INICIAL_FAMILIA;

    var htmlItem = "";
    if (esValidoLista(listaFamilia)) {
        for (var i = 0; i < listaFamilia.length; i++) {
            var codigo = listaFamilia[i].Famcodi;
            var nombre = listaFamilia[i].Famnomb;
            htmlItem += `<option value="${codigo}">${nombre}</option>`;
        }
    }

    var htmlTipoEquipo = `
    <table class="content-tabla-search" style="width:auto" id="${pref}tablaTipoEquipo">
        <tr>
            <td>Tipo de Equipo:</td>
            <td>
                <select id="${pref}cbFamiliaEquipo" style="width: 235px;">
                    <option value="0">--TODOS--</option>
                    ${htmlItem}
                </select>
            </td>
        </tr>
    </table>
    `;

    return htmlTipoEquipo;
}

function eq_ui_generarTablaArea(tipo) {
    var pref = getPrefijoXTipo(tipo);

    var htmlArea = `
        <table border="0" class="pretty" id="${pref}tablaArea">
            <thead>
                <tr>
                    <th>Ubicación</th>
                </tr>
            </thead>
            <tbody>`
        ;

    var listaArea = OBJETO_DATA.origen.DATA_INICIAL_AREA;
    if (esValidoLista(listaArea)) {
        for (var i = 0; i < listaArea.length; i++) {
            var codigo = listaArea[i].Areacodi;
            var nombre = listaArea[i].Areanomb;
            htmlArea += `
                <tr onclick="eq_mostrarByTipo(${tipo},${codigo});" style="cursor:pointer">
                    <td>${nombre}</td>
                </tr>
            ` ;
        }
    }

    htmlArea += ` 
            </tbody>
        </table>
    `;

    return htmlArea;
}

function eq_ui_generarTabla(tipo) {
    var pref = getPrefijoXTipo(tipo);
    var listaDataInicial = eq_buscarXFiltro(tipo, ESTADO_EQ_TODOS);
    var listaDataSelect = eq_getListaDataSeleccionado(tipo);

    var htmlEquipo = `
        <table class="tabla-formulario" id="${pref}tablaEquipo" style='width:100%'>
            <thead>
                <tr>`;
    if (!tieneTransferirEmpresa(OBJETO_DATA.tipoMigraOp)) {
        var checkTodos = eq_CheckedFiltro(tipo) ? "checked" : "";

        htmlEquipo += `
                    <th style='text-align: center;vertical-align: middle;'>
                         <input type="checkbox" name="${pref}check_equipo" id="${pref}check_equipo_-1" ${checkTodos} value="-1">
                    </th>`;
    }
    htmlEquipo += ` <th>Área</th>
                    <th>Tipo</th>
                    <th>Equipo</th>
                </tr>
            </thead>
            <tbody>
    `;

    if (esValidoLista(listaDataInicial)) {
        for (var i = 0; i < listaDataInicial.length; i++) {
            var reg = listaDataInicial[i];
            var codigo = reg.Equicodi;
            var area = reg.Areanomb;
            var fam = reg.Famabrev;
            var abrev = reg.Equiabrev;
            var codigoArea = reg.Areacodi;
            var estiloTr = reg.Osigrupocodi;

            var check = "";
            var displayCheck = "none";
            if (reg.Equiestado != ESTADO_EQ_ELIMINADO) {
                check = eq_buscarSeleccionado(listaDataSelect, tipo, codigo) != null ? "checked" : "";
                displayCheck = "inline-block";
            }

            htmlEquipo += `
                <tr style="cursor:pointer;${estiloTr}">
                    `;

            if (!tieneTransferirEmpresa(OBJETO_DATA.tipoMigraOp)) {
                htmlEquipo += `
                    <td style='text-align: center;vertical-align: middle;'>
                        <input type="checkbox" name="${pref}check_equipo" id="${pref}check_equipo_${codigo}" value="${codigo}" ${check} style="display: ${displayCheck}" />
                        <input type="hidden" name="check_equipo_area" id="fila_equipo_area_${codigo}" value="${codigoArea}" />
                    </td>
            `;
            }
            htmlEquipo += `
                    <td>${area}</td>
                    <td>${fam}</td>
                    <td>
                        ${abrev}
                        <input type="hidden" id="fila_equipo_${codigo}" value="${codigo}" />
                    </td>
                </tr>
            ` ;
        }
    }

    htmlEquipo += ` 
            </tbody>
        </table>
    `;

    return htmlEquipo;
}

function eq_ui_configTabla(tipo) {
    var pref = getPrefijoXTipo(tipo);
    $('input[name=' + pref + 'check_equipo]:checkbox').unbind();
    $('input[name=' + pref + 'check_equipo]:checkbox').change(function (e) {
        eq_data_actualizarData(tipo, $(this).val(), $(this).prop("checked"));
    });

    $("#" + pref + "cbFamiliaEquipo").unbind();
    $("#" + pref + "cbFamiliaEquipo").change(function (e) {
        eq_mostrarByTipo(tipo, getAreacodi(tipo), $("#" + pref + "cbFamiliaEquipo").val());
    });

    $("#" + pref + "tablaEquipo").DataTable({
        "scrollY": "380px",
        "scrollCollapse": true,
        "paging": false,
        "oLanguage": {
            "sEmptyTable": "No existen registros"
        }
    });
}

///

function eq_data_actualizarData(tipo, codigoEquipo, check) {
    codigoEquipo = parseInt(codigoEquipo);
    if (check) {
        eq_data_agregarToListaData(tipo, codigoEquipo);
    } else {
        eq_data_quitarToListaData(tipo, codigoEquipo);
    }

    var pref = getPrefijoXTipo(tipo);
    if (eq_CheckedFiltro(tipo)) {
        $("#" + pref + "check_equipo_-1").prop('checked', true).attr('checked', 'checked');
    } else {
        $("#" + pref + "check_equipo_-1").prop('checked', false).removeAttr('checked');
    }
}

function eq_data_agregarToListaData(tipo, codigoEquipo) {
    var pref = getPrefijoXTipo(tipo);
    var listaDataIni = eq_buscarXFiltro(tipo, ESTADO_EQ_NO_ELIMINADO);
    var listaDataSeleccionado = eq_getListaDataSeleccionado(tipo);

    var listaFiltro = [];
    if (codigoEquipo > -1) {
        listaFiltro = listaDataSeleccionado;
        for (var i = 0; i < listaDataIni.length; i++) {
            var regIni = listaDataIni[i];
            if (regIni.Equicodi == codigoEquipo) {
                listaFiltro.push(regIni);
            }
        }
    }
    else {
        listaFiltro = listaDataIni;

        $('input[name=' + pref + 'check_equipo]:checkbox').each(function () {
            $(this).prop('checked', true).attr('checked', 'checked');
        });
    }

    eq_setListaDataSeleccionado(tipo, listaFiltro);

    return listaFiltro;
}

function eq_data_quitarToListaData(tipo, codigoEquipo) {
    var pref = getPrefijoXTipo(tipo);
    var listaDataSeleccionado = eq_getListaDataSeleccionado(tipo);

    var listaFiltro = [];
    if (codigoEquipo > -1) {
        for (var j = 0; j < listaDataSeleccionado.length; j++) {
            var regCheck = listaDataSeleccionado[j];
            if (regCheck.Equicodi != codigoEquipo) {
                listaFiltro.push(regCheck);
            }
        }
    } else {
        $('input[name=' + pref + 'check_equipo]:checkbox').each(function () {
            $(this).prop('checked', false).removeAttr('checked');
        });
    }

    eq_setListaDataSeleccionado(tipo, listaFiltro);

    return listaFiltro;
}

///

function eq_buscarSeleccionado(listaDataInicial, tipo, codigoEquipo) {
    var listaDataSeleccionado = eq_getListaDataSeleccionado(tipo);

    var listaFiltro = eq_listarElementosCheck(listaDataInicial, listaDataSeleccionado);

    return eq_getEquipoFromLista(listaFiltro, codigoEquipo);
}

function eq_CheckedFiltro(tipo) {
    var listaDataInicial = eq_buscarXFiltro(tipo, ESTADO_EQ_NO_ELIMINADO);
    var listaDataSeleccionado = eq_getListaDataSeleccionado(tipo);

    if (esValidoLista(listaDataInicial)) {
        for (var i = 0; i < listaDataInicial.length; i++) {
            var equicodiIni = listaDataInicial[i].Equicodi;
            if (eq_getEquipoFromLista(listaDataSeleccionado, equicodiIni) == null) {
                return false;
            }
        }

        return true;
    }

    return false;
}

function eq_buscarXFiltro(tipo, estado) {
    var famcodi = getFamcodi(tipo);
    var areacodi = getAreacodi(tipo);
    var listaInicial = eq_getListaDataInicial(tipo);

    areacodi = parseInt(areacodi) || 0;
    famcodi = parseInt(famcodi) || 0;

    var listaFiltro = [];
    for (var i = 0; i < listaInicial.length; i++) {
        var reg = listaInicial[i];
        if (
            ((estado == ESTADO_EQ_TODOS) || (estado == ESTADO_EQ_NO_ELIMINADO && reg.Equiestado != ESTADO_EQ_ELIMINADO) || (estado == ESTADO_EQ_ELIMINADO && reg.Equiestado == ESTADO_EQ_ELIMINADO)) &&
            (areacodi <= 0 || reg.Areacodi == areacodi) && (famcodi <= 0 || reg.Famcodi == famcodi)
        ) {
            listaFiltro.push(reg);
        }
    }

    return listaFiltro;
}

///

function eq_listarElementosCheck(listaDataIni, listaDataCheck) {
    var listaFiltro = [];

    if (esValidoLista(listaDataIni) && esValidoLista(listaDataCheck))
        for (var i = 0; i < listaDataIni.length; i++) {
            var regIni = listaDataIni[i];
            for (var j = 0; j < listaDataCheck.length; j++) {
                var regCheck = listaDataCheck[j];
                if (regIni.Equicodi == regCheck.Equicodi) {
                    listaFiltro.push(regCheck);
                    j = listaDataCheck.length;
                }
            }
        }

    return listaFiltro;
}

///

function eq_data_Transferir() {
    //Quitar los elementos del Origen
    var listaFiltro = [];

    var listaIniEq = OBJETO_DATA.origen.DATA_INICIAL_EQUIPO;
    var listaOrigEq = OBJETO_DATA.origen.DATA_SELECT_EQUIPO;

    if (esValidoLista(listaIniEq) && esValidoLista(listaOrigEq)) {
        for (var i = 0; i < listaIniEq.length; i++) {
            var regIni = listaIniEq[i];
            var existeEquipo = false;
            for (var j = 0; j < listaOrigEq.length && !existeEquipo; j++) {
                var regCheck = listaOrigEq[j];
                if (regIni.Equicodi == regCheck.Equicodi) {
                    existeEquipo = true;
                }
            }

            if (!existeEquipo) {
                listaFiltro.push(regIni);
            }
        }

        OBJETO_DATA.origen.DATA_INICIAL_EQUIPO = listaFiltro;
        OBJETO_DATA.origen.DATA_SELECT_EQUIPO = [];
    }

    //Mover al destino
    OBJETO_DATA.destino.DATA_INICIAL_EQUIPO = OBJETO_DATA.destino.DATA_INICIAL_EQUIPO.concat(listaOrigEq);
    OBJETO_DATA.destino.DATA_SELECT_EQUIPO = OBJETO_DATA.destino.DATA_SELECT_EQUIPO.concat(listaOrigEq);

    //Visualizar cambios
    actualizarTabEquipo();
}

function eq_data_Regresar() {
    //Quitar los elementos del destino
    var listaFiltro = [];

    var listaIniEq = OBJETO_DATA.destino.DATA_INICIAL_EQUIPO;
    var listaOrigEq = OBJETO_DATA.destino.DATA_SELECT_EQUIPO;

    if (esValidoLista(listaIniEq) && esValidoLista(listaOrigEq)) {
        for (var i = 0; i < listaIniEq.length; i++) {
            var regIni = listaIniEq[i];
            var existeEquipo = false;
            for (var j = 0; j < listaOrigEq.length && !existeEquipo; j++) {
                var regCheck = listaOrigEq[j];
                if (regIni.Equicodi == regCheck.Equicodi) {
                    existeEquipo = true;
                }
            }

            if (!existeEquipo) {
                listaFiltro.push(regIni);
            }
        }

        OBJETO_DATA.destino.DATA_INICIAL_EQUIPO = listaFiltro;
        OBJETO_DATA.destino.DATA_SELECT_EQUIPO = [];
    }

    //Mover al destino
    OBJETO_DATA.origen.DATA_INICIAL_EQUIPO = OBJETO_DATA.origen.DATA_INICIAL_EQUIPO.concat(listaOrigEq);
    OBJETO_DATA.origen.DATA_SELECT_EQUIPO = OBJETO_DATA.origen.DATA_SELECT_EQUIPO.concat(listaOrigEq);

    //Visualizar cambios
    actualizarTabEquipo();
}

////////////////////////////////////////////////////////////////////////////////////////////////////
/// Procesar Transferencia
////////////////////////////////////////////////////////////////////////////////////////////////////

function getObjTransferencia() {
    var tipoMigraOp = parseInt($('#cbTipoMigracion').val()) || 0;
    var emprcodiOrigen = parseInt($('#orig_empr_codigo').val()) || 0;
    var emprcodi = parseInt($('#dest_empr_codigo').val()) || 0;
    var strFechaCorte = $("#txtFecha").val();
    var descripcion = $("#txtDescripcion").val();
    var checkStr = getCheckStr();
    var checkPM = getCheckPM(); //ASSETEC 202108 TIEE

    if (tipoMigraOp === TIPO_MIGR_CAMBIO_RAZON_SOCIAL || tipoMigraOp === TIPO_MIGR_DUPLICIDAD || tipoMigraOp === TIPO_MIGR_FUSION) {
        checkStr = 1;
    }

    var listaEquicodi = [];

    for (var i = 0; i < OBJETO_DATA.destino.DATA_INICIAL_EQUIPO.length; i++) {
        var reg = OBJETO_DATA.destino.DATA_INICIAL_EQUIPO[i];
        listaEquicodi.push(reg.Equicodi);
    }
    var obj = {
        tipoMigraOp: tipoMigraOp,
        emprcodiOrigen: emprcodiOrigen,
        emprcodi: emprcodi,
        strFechaCorte: strFechaCorte,
        descripcion: descripcion,
        listaEquicodi: listaEquicodi,
        regHistoricoTransf: getCheckHist(),
        regStrTransf: checkStr,
        regPM: checkPM
    };

    return obj;
}

function val_objTransferencia(obj) {
    var msj = '';
    var saltoLinea = '<br/>';

    //
    if (obj.tipoMigraOp <= 0) {
        msj += 'Debe seleccionar un Tipo de operación' + saltoLinea;
    }

    if (obj.descripcion == null || obj.descripcion == '') {
        msj += 'Debe ingresar descripción de la transferencia' + saltoLinea;
    } else {
        if (obj.descripcion.length < 20) {
            msj += 'Debe ingresar descripción de la transferencia con un mínimo de 20 caracteres' + saltoLinea;
        }
    }

    //origen
    if (obj.emprcodiOrigen <= 0) {
        msj += 'Debe seleccionar una Empresa Origen' + saltoLinea;
    }

    //destino
    if (obj.emprcodi <= 0) {
        msj += 'Debe seleccionar una Empresa Destino' + saltoLinea;
    }

    //Validacion códigos
    if (obj.emprcodi > 0 && obj.emprcodiOrigen > 0 && obj.emprcodi == obj.emprcodiOrigen) {
        msj += 'Debe seleccionar una Empresa Destino diferente a la Empresa Origen' + saltoLinea;
    }

    //fecha
    switch (obj.tipoMigraOp) {
        case TIPO_MIGR_CAMBIO_RAZON_SOCIAL:
        case TIPO_MIGR_FUSION:
        case TIPO_MIGR_TRANSFERENCIA:
            if (obj.strFechaCorte == null || obj.strFechaCorte == '') {
                msj += 'Debe seleccionar una Fecha' + saltoLinea;
            }
            break;
        default:
            obj.strFechaCorte = '';
            break;
    }

    if (msj == '') {
        if (!OBJETO_DATA.validado) {
            OBJETO_DATA.tipoMigraOp = obj.tipoMigraOp;
            OBJETO_DATA.emprcodiOrigen = obj.emprcodiOrigen;
            OBJETO_DATA.emprcodi = obj.emprcodi;
            OBJETO_DATA.strFechaCorte = obj.strFechaCorte;
            OBJETO_DATA.validado = true;
            OBJETO_DATA.actualizarInstalaciones = true;
        } else {
            OBJETO_DATA.actualizarInstalaciones = false;
            OBJETO_DATA.strFechaCorte = obj.strFechaCorte;
            //si la diferencia puede provocar cambios, actualizar la lista de equipos, puntos, grupos
            if (OBJETO_DATA.emprcodiOrigen != obj.emprcodiOrigen || OBJETO_DATA.emprcodi != obj.emprcodi
                || OBJETO_DATA.tipoMigraOp != obj.tipoMigraOp) {
                msj += 'Existen modificaciones en [1. Datos de Transferencia] que borrará la información configurada hasta ahora' + saltoLinea;

                OBJETO_DATA.tipoMigraOp = obj.tipoMigraOp;
                OBJETO_DATA.emprcodiOrigen = obj.emprcodiOrigen;
                OBJETO_DATA.emprcodi = obj.emprcodi;
                OBJETO_DATA.strFechaCorte = obj.strFechaCorte;
                OBJETO_DATA.actualizarInstalaciones = true;
                OBJETO_DATA.origen.DATA_SELECT_EQUIPO = [];
                OBJETO_DATA.destino.DATA_INICIAL_EQUIPO = [];
                OBJETO_DATA.destino.DATA_SELECT_EQUIPO = [];
                msj = '';
            }
        }
    }

    return msj;
}

function val_ExisteEquiposTransf(obj) {
    switch (obj.tipoMigraOp) {
        case TIPO_MIGR_INSTALACION_NO_CORRESPONDEN:
        case TIPO_MIGR_TRANSFERENCIA:
            if (OBJETO_DATA.destino.DATA_INICIAL_EQUIPO.length <= 0) {
                return 'Debe seleccionar uno o más equipos';
            }
            break;
    }

    return '';
}

function validarExistenciaTransferencia() {
    var obj = getObjTransferencia();
    var msjVal = val_objTransferencia(obj);
    var bRes = false;

    if (msjVal == '') {
        $.ajax({
            type: 'POST',
            url: controlador + 'ValidarNuevaMigracion',
            data: {
                tmopercodi: obj.tipoMigraOp,
                emprcodiOrigen: obj.emprcodiOrigen,
                emprcodi: obj.emprcodi,
                strFechaCorte: obj.strFechaCorte
            },
            dataType: 'json',
            cache: false,
            async: false,
            success: function (result) {
                bRes = result.Resultado != "-1";
                if (!bRes) {
                    mostrarError(result.Mensaje);
                }
            },
            error: function (err) {
                alert('Ha ocurrido un error');
                bRes = false;
            }
        });
    } else {
        mostrarError(msjVal);
        bRes = false;
    }
    return bRes;
}

function procesarTransferencia() {
    var obj = getObjTransferencia();
    var msjVal = val_objTransferencia(obj);

    if (msjVal == '') {
        msjVal = val_ExisteEquiposTransf(obj);
        if (msjVal == '') {
            $("#steps-uid-0 div.actions.clearfix").hide();
            $.ajax({
                type: 'POST',
                url: controlador + 'ProcesarTransferencia',
                data: {
                    strTransf: getJsonObjTransf(obj)
                },
                dataType: 'json',
                cache: false,
                async: true,
                success: function (result) {
                    var bRes = result.Resultado == "1";
                    if (bRes) {
                        if (result.Mensaje == "")
                        {   //Sin errores
                            alert('La transferencia ha sido registrado correctamente');
                            mostrarDetalleTransferencia(result.Migracodi);
                        }
                        else {
                            //ASSETEC 202108 - Errores en el Proceso de Mercado
                            alert('La transferencia ha sido registrado correctamente, pero existen algunos errores en el Proceso de Mercado...!');
                            mostrarError(result.Mensaje);
                            $("#steps-uid-0 div.actions.clearfix").show();

                            mostrarDetalleTransferencia(result.Migracodi);
                        }
                    } else {
                        mostrarError(result.Mensaje);
                        $("#steps-uid-0 div.actions.clearfix").show()
                    }
                },
                error: function (err) {
                    alert('Ha ocurrido un error');
                    $("#steps-uid-0 div.actions.clearfix").show()
                    bRes = false;
                }
            });
        }
    }

    if (msjVal != '') {
        $("#steps-uid-0 div.actions.clearfix ul li:eq(2)").hide();
        mostrarError(msjVal);
    } else {
        $("#steps-uid-0 div.actions.clearfix ul li:eq(2)").show();
    }
}

function getJsonObjTransf(obj) {
    var reg = {};
    reg.Tmopercodi = obj.tipoMigraOp;
    reg.EmprcodiOrigen = obj.emprcodiOrigen;
    reg.Emprcodi = obj.emprcodi;
    reg.StrFechaCorte = obj.strFechaCorte;
    reg.Descripcion = obj.descripcion;
    reg.ListaEquicodi = obj.listaEquicodi;
    reg.ListaPtomedicodi = obj.listaPtomedicodi;
    reg.ListaGrupocodi = obj.listaGrupocodi;
    reg.RegHistoricoTransf = obj.regHistoricoTransf;
    reg.RegStrTransf = obj.regStrTransf;
    reg.RegPM = obj.regPM; //ASSETEC 202108 TIEE

    return JSON.stringify(reg);
}

function getCheckStr() {
    var estado = 0;
    if ($('#check_str').is(':checked')) {
        estado = 1;
    }
    return estado;
}

//ASSETEC 202108 TIEE
function getCheckPM() {
    var estado = 0;
    if ($('#check_pm').is(':checked')) {
        estado = 1;
    }
    return estado;
}
//-------------------

function getCheckHist() {
    var estado = false;
    if ($('#check_hist').is(':checked')) {
        estado = true;
    }
    return estado;
}

////////////////////////////////////////////////////////////////////////////////////////////////////
/// Útil
////////////////////////////////////////////////////////////////////////////////////////////////////

function getPrefijoXTipo(tipo) {
    switch (tipo) {
        case TIPO_EMPRESA_ORIGEN:
            return PREFIJO_EMPRESA_ORIGEN;
            break;
        case TIPO_EMPRESA_DESTINO:
            return PREFIJO_EMPRESA_DESTINO;
            break;
    }

    return '';
}

function getEmpresa() {
    return OBJETO_DATA.emprcodiOrigen;
}

function setObjEmpresa(tipo, objEmpr) {
    if (objEmpr !== undefined) {
        var objData = getObjetoDataByTipo(tipo);
        objData.empresa = objEmpr;
    }
}

function getObjEmpresa(tipo) {
    var objData = getObjetoDataByTipo(tipo);
    return objData.empresa;
}

function getEmpresaByTipo(tipo) {
    var pref = getPrefijoXTipo(tipo);
    return parseInt($('#' + pref + 'empr_codigo').val()) || 0;
}

function setFamcodi(tipo, famcodi) {
    if (famcodi !== undefined) {
        var objData = getObjetoDataByTipo(tipo);
        objData.SELECT_CODIGO_FAMILIA = famcodi;
    }
}

function getFamcodi(tipo) {
    var objData = getObjetoDataByTipo(tipo);
    return objData.SELECT_CODIGO_FAMILIA;
}

function setAreacodi(tipo, areacodi) {
    if (areacodi !== undefined) {
        var objData = getObjetoDataByTipo(tipo);
        objData.SELECT_CODIGO_AREA = areacodi;
    }
}

function getAreacodi(tipo) {
    var objData = getObjetoDataByTipo(tipo);
    return objData.SELECT_CODIGO_AREA;
}

function eq_getListaDataSeleccionado(tipo) {
    var objData = getObjetoDataByTipo(tipo);
    return objData.DATA_SELECT_EQUIPO;
}

function eq_getListaDataInicial(tipo) {
    var objData = getObjetoDataByTipo(tipo);
    return objData.DATA_INICIAL_EQUIPO;
}

function eq_setListaDataSeleccionado(tipo, lista) {
    var objData = getObjetoDataByTipo(tipo);
    objData.DATA_SELECT_EQUIPO = lista;
}

function eq_getEquipoFromLista(listaFiltro, codigoEquipo) {

    if (esValidoLista(listaFiltro)) {
        for (var i = 0; i < listaFiltro.length; i++) {
            if (listaFiltro[i].Equicodi == codigoEquipo) {
                return listaFiltro[i];
            }
        }
    }

    return null;
}

function getObjetoDataByTipo(tipo) {
    switch (tipo) {
        case TIPO_EMPRESA_ORIGEN:
            return OBJETO_DATA.origen;
            break;
        case TIPO_EMPRESA_DESTINO:
            return OBJETO_DATA.destino;
            break;
    }
}

function esValidoLista(lista) {
    return lista !== undefined && lista != null && lista.length > 0;
}