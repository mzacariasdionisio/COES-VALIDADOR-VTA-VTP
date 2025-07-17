var controlador = siteRoot + 'hidrologia/'
var oTable;
$(function () {

    $('#btnRegresar').click(function () {
        recargar();
    });

    ////
    $('#btnAgregarHoja').click(function () {
        agregarHojaExcelweb();
    });
    $('#btnPopupHojaExcel').click(function () {
        $("#IdLecturaNuevo").val($("#hfLecturaNuevo").val());
        $("#IdCabeceraNuevo").val($("#hfCabeceraNuevo").val());

        setTimeout(function () {
            $('#popupNuevaHoja').bPopup({
                easing: 'easeOutBack',
                speed: 450,
                transition: 'slideDown'
            });
        }, 50);
    });

    ////
    $('#btnEjecutarCopiaConfig').click(function () {
        ejecutarCopiaConfig();
    });
    $('#btnAbrirPopupCopiaConfig').click(function () {
        setTimeout(function () {
            $('#popupCopiaConfig').bPopup({
                easing: 'easeOutBack',
                speed: 450,
                transition: 'slideDown'
            });
        }, 50);
    });

    $('#btnBuscar').click(function () {
        mostrarListaPto();
    });
    $('#btnActualizar').click(function () {
        editarPto();
    });

    $('#btnPunto').click(function () {
        nuevoPto();
    });

    //
    $('#btnNuevo').click(function () {
        agregarEmpresa();
    });
    $('#btnCancelar2').click(function () {
        $('#agregaEmpresa').bPopup().close();
    });

    $('#cbFiltroEmpresa').val($('#hfFiltroEmpresa').val());

    $('#btnManual').click(function () {
        window.open('~/documentos/Manual_Usuario_Hist%C3%B3rico_m%C3%B3dulo_Stock_Combustible.pdf', '_blank');
    });

    mostrarListaPto();

    var valorperiodo = parseInt($('#hfPeriodo').val()) || 0;

    $('#txtFechaPeriodo').Zebra_DatePicker({
        format: 'd/m/Y',
        onSelect: function () {
            actualizarVistaPrevia();
        }
    });

    if (valorperiodo == 3 || valorperiodo == 5) {

        $('#txtFechaVigencia').Zebra_DatePicker({
            format: 'm Y',
            onSelect: function () {
                actualizarVistaPrevia();
            }
        });
    }
    else {
        $('#txtFechaVigencia').Zebra_DatePicker({


            format: 'd/m/Y',
            onSelect: function () {
                actualizarVistaPrevia();
            }
        });
    }

    $('#txtDiaPlazo').change(function () {
        actualizarVistaPreviaIncremento();
    });

    $('#txtHoraPlazo').change(function () {
        actualizarVistaPreviaIncremento();
    });

    $('#btnCancelar').click(function () {
        $('#popupmpto').bPopup().close();
    });

    $('#txtMesPeriodo').Zebra_DatePicker({
        format: 'm Y',
        onSelect: function () {
            actualizarVistaPrevia();
        }
    });

});
//convierte 2 cadenas de texto fecha(dd/mm/yyyy) y horas(hh:mm:ss) a tipo Date
function convertStringToDate(fecha, horas) {
    var partsFecha = fecha.split('/');
    if (horas == "") {
        return "";
    }
    var partsHoras = horas.split(':');
    //new Date(yyyy, mm-1, dd, hh, mm, ss);
    return new Date(partsFecha[2], partsFecha[1] - 1, partsFecha[0], partsHoras[0], partsHoras[1], partsHoras[2]);
}

function actualizarVistaPrevia() {
    var obj = getObjPlazo();
    var strFecha = obj.Fecha;
    if (strFecha !== undefined) {
        var dhoy = moment(convertStringToDate(strFecha, '00:00:00'));
        var dIniPlazo = moment(convertStringToDate(strFecha, '00:00:00'));
        var dFinPlazo = moment(convertStringToDate(strFecha, '00:00:00'));
        var dFueraPlazo = moment(convertStringToDate(strFecha, '00:00:00'));

        dIniPlazo = dIniPlazo.add(obj.Plazinimes, 'M').add(obj.Plazinidia, 'days').add(obj.Plazinimin, 'minutes');
        dFinPlazo = dFinPlazo.add(obj.Plazfinmes, 'M').add(obj.Plazfindia, 'days').add(obj.Plazfinmin, 'minutes');
        dFueraPlazo = dFueraPlazo.add(obj.Plazfuerames, 'M').add(obj.Plazfueradia, 'days').add(obj.Plazfueramin, 'minutes');

        $("#txtFechaPeriodo").text(dhoy.format('DD/MM/YYYY'));
        $("#txtFechaEnPlazo1").text(dIniPlazo.format('DD/MM/YYYY HH:mm'));
        $("#txtFechaEnPlazo2").text(dFinPlazo.format('DD/MM/YYYY HH:mm'));
    }
}


function actualizarVistaPreviaIncremento() {
    var obj = getObjPlazoIncremento();
    var strFecha = obj.Fecha;
    if (strFecha !== undefined) {
        var dhoy = moment(convertStringToDate(strFecha, '00:00:00'));
        var dIniPlazo = moment(convertStringToDate(strFecha, '00:00:00'));
        var dFinPlazo = moment(convertStringToDate(strFecha, '00:00:00'));
        var dFueraPlazo = moment(convertStringToDate(strFecha, '00:00:00'));

        dIniPlazo = dIniPlazo.add(obj.Plazinimes, 'M').add(obj.Plazinidia, 'days').add(obj.Plazinimin, 'minutes');
        dFinPlazo = dFinPlazo.add(obj.Plazfinmes, 'M').add(obj.Plazfindia, 'days').add(obj.Plazfinmin, 'minutes');
        dFueraPlazo = dFueraPlazo.add(obj.Plazfuerames, 'M').add(obj.Plazfueradia, 'days').add(obj.Plazfueramin, 'minutes');

        $("#txtFechaEnPlazo1").text(dIniPlazo.format('DD/MM/YYYY HH:mm'));
        $("#txtFechaEnPlazo2").text(dFinPlazo.format('DD/MM/YYYY HH:mm'));
    }
}


function getObjPlazo() {
    var obj = {};

    var strFecha = $("#txtFechaPeriodo").val();
    var valorperiodo = parseInt($('#hfPeriodo').val()) || 0;

    if (valorperiodo == 3 || valorperiodo == 5) {//mensual, Semanal x Mes
        strFecha = $("#txtMesPeriodo").val();
        strFecha = "01" + "/" + strFecha.substr(0, 2) + "/" + strFecha.substr(3, 4);
    } else {
        if (valorperiodo == 2) {
            strFecha = $("#cbSemana").val();
        } else {
        }
    }
    obj.Fecha = strFecha;

    obj.Plazinimes = parseInt($("#Mesplazo").val()) || 0;
    obj.Plazinidia = parseInt($("#DiaPlazo").val()) || 0;
    obj.Plazfinmes = parseInt($("#Mesfinplazo").val()) || 0;

    obj.Plazinimin = convertirHoraAMinutos($("#txtMinPlazo").val());
    obj.Plazfinmin = (convertirHoraAMinutosExtension($("#txtHoraPlazo").val()) + (convertirHoraAMinutos($("#txtMinFinPlazo").val())));
    obj.Plazfindia = (parseInt($("#txtDiaPlazo").val()) + parseInt($("#DiaFinPlazo").val())) || 0;

    return obj;
}

function getObjPlazoIncremento() {
    var obj = {};

    var strFecha = $("#txtFechaPeriodo").val();
    var valorperiodo = parseInt($('#hfPeriodo').val()) || 0;

    if (valorperiodo == 3 || valorperiodo == 5) {//mensual, Semanal x Mes
        strFecha = $("#txtMesPeriodo").val();
        strFecha = "01" + "/" + strFecha.substr(0, 2) + "/" + strFecha.substr(3, 4);
    } else {
        if (valorperiodo == 2) {
            strFecha = $("#cbSemana").val();
        } else {
        }
    }
    obj.Fecha = strFecha;
    obj.Plazinimes = parseInt($("#Mesplazo").val()) || 0;
    obj.Plazinidia = parseInt($("#DiaPlazo").val()) || 0;
    obj.Plazfinmes = parseInt($("#Mesfinplazo").val()) || 0;
    obj.Plazfindia = (parseInt($("#txtDiaPlazo").val()) + parseInt($("#DiaFinPlazo").val())) || 0;
    obj.Plazinimin = convertirHoraAMinutos($("#txtMinPlazo").val());
    obj.Plazfinmin = (convertirHoraAMinutosExtension($("#txtHoraPlazo").val()) + (convertirHoraAMinutos($("#txtMinFinPlazo").val())));

    return obj;
}

function convertirHoraAMinutos(minPlazo) {
    var mPlazo = 0;
    if (minPlazo !== undefined) {
        mPlazo = parseInt(minPlazo.substr(0, 2)) * 60 + parseInt(minPlazo.substr(3, 2));
        mPlazo = parseInt(mPlazo) || 0;
    }

    return mPlazo;
}

function convertirHoraAMinutosExtension(minPlazo) {
    var minPlazo = 0;
    if (minPlazo !== undefined) {
        mPlazo = minPlazo * 60;
        mPlazo = parseInt(mPlazo) || 0;
    }

    return mPlazo;
}

function mostrarListaPto() {
    var empresa = $('#cbFiltroEmpresa').val();
    var formato = $('#hfFormato').val();
    var idHoja = parseInt($('#cbHoja').val()) || -1;
    var formatoOrigen = parseInt($('#idFormatoOrigen').val()) || 0;

    $.ajax({
        type: 'POST',
        url: controlador + "formatomedicion/ListaPtoMedicion",
        data: {
            empresa: empresa,
            formato: formato,
            hoja: idHoja,
            formatoOrigen: formatoOrigen
        },
        success: function (evt) {

            $('#listpto').html(evt);
            oTable = $('#tablaPtos').dataTable({
                "bJQueryUI": true,
                "scrollY": 650,
                "scrollX": true,
                "sDom": 't',
                "ordering": formatoOrigen <= 0,
                "iDisplayLength": 400,
                "sPaginationType": "full_numbers"
            });

            if (formatoOrigen <= 0)
                oTable.rowReordering({ sURL: controlador + "/formatomedicion/UpdateOrder?formato=" + formato + "&hoja=" + idHoja + "&empresa=" + empresa });
        },
        error: function (err) {
            alert("Ha ocurrido un error");
        }
    });
}

function recargar() {
    var codigoApp = parseInt($("#hdCodigoApp").val()) || 0;
    document.location.href = controlador + "formatomedicion/Index?app=" + codigoApp;
}

function modificarPto(punto, tipoinfo, tptomedi, pos, limsup, liminf, activo, observacion, filas, dia, hora, emprecodi, formatcodi, hptoindcheck) {
    var checkInd = hptoindcheck == "S" ? 1 : 0;

    //fila adicional de check para Extranet
    $("#fila_adicional").hide();
    var strTdCheckAdicional = '';

    if (tipoinfo == 11) strTdCheckAdicional = 'Indicar si el dato es ejecutado o proyectado';
    if (tipoinfo == 14) strTdCheckAdicional = 'Indicar condición de vertimiento';
    if (tipoinfo == 1) strTdCheckAdicional = '¿Habilitar recepción de pronóstico?';
    if (strTdCheckAdicional != '') {
        $("#fila_adicional").show();
        $("#td_check_adicional").html(strTdCheckAdicional);
    }

    if (activo == 1) {
        listarHistoricoExtPlazo(formatcodi, punto, tipoinfo);
        var confIni = obtenerDatoHoraMin($('#hfMinPlazo').val());
        $('#txtMinPlazo').val(confIni[0] + ":" + confIni[1]);
        var confFin = obtenerDatoHoraMin($('#hfMinFinPlazo').val());
        $('#txtMinFinPlazo').val(confFin[0] + ":" + confFin[1]);

        var confFinExt = obtenerDatoHoraMin(hora);

        var valorperiodo = parseInt($('#hfPeriodo').val()) || 0;
        //Periodo Mensual
        if (valorperiodo == 3 || valorperiodo == 5) {//mensual, Semanal x Mes
            document.getElementById("txtFechaPeriodo").disabled = true;
            document.getElementById("cbSemana").disabled = true;
            //Periodo Semanal
        } else if (valorperiodo == 2) {
            document.getElementById("txtFechaPeriodo").disabled = true;
            document.getElementById("txtMesPeriodo").disabled = true;
        }
        //Periodo diaria
        else if (valorperiodo == 1) {
            document.getElementById("cbSemana").disabled = true;
            document.getElementById("txtMesPeriodo").disabled = true;
        }
    }
    $('#idLimSup').val(limsup);
    $('#idLimInf').val(liminf);
    $('#hfPunto').val(punto);
    $('#hfTipoinfo').val(tipoinfo);
    $('#hfTptomedi').val(tptomedi);
    $('#txtObservacionPunto').val(observacion);
    $('#hfEmprcodi').val(emprecodi);

    document.getElementById("idActivo").checked = parseInt(activo);
    if (document.getElementById("idCheckAdicional") != null)
        document.getElementById("idCheckAdicional").checked = checkInd;

    if (activo == 1) {
        actualizarVistaPrevia();
        actualizarVistaPreviaIncremento();
    }

    setTimeout(function () {
        $('#popupmpto').bPopup({
            easing: 'easeOutBack',
            speed: 450,
            transition: 'slideDown'
        });
    }, 50);
}

function editarPto(punto) {
    formato = $('#hfFormato').val();
    limsup = $('#idLimSup').val();
    liminf = $('#idLimInf').val();
    punto = $('#hfPunto').val();
    tipoinfo = $('#hfTipoinfo').val();
    tptomedi = $('#hfTptomedi').val();
    minfila = $('#txtMinFilas').val();
    diaPlazo = $('#txtDiaPlazo').val();
    horaPlazo = $('#txtHoraPlazo').val();
    fechaVigencia = $('#txtFechaVigencia').val();
    var valorperiodo = parseInt($('#hfPeriodo').val()) || 0;
    if (valorperiodo == 3 || valorperiodo == 5) {
        fechaVigencia = "01" + "/" + fechaVigencia.substr(0, 2) + "/" + fechaVigencia.substr(3, 4);
    }
    estadoConf = $('#hfEstadoConf').val();
    minPlazo = $('#txtHoraPlazo').val();

    plazptocodi = $('#hfPlzPtoCodi').val();
    plazptocodigo = $('#hfPlzPtoCodigo').val();
    emprcodi = $('#hfEmprcodi').val();

    if (plazptocodi == undefined) {
        if (plazptocodigo != '') {
            plazptocodi = plazptocodigo;
        }
        else {
            plazptocodi = 0;
        }
    }
    if (plazptocodi == '') {
        if (plazptocodigo != undefined) {
            plazptocodi = plazptocodigo;
        }
        else {
            plazptocodi = 0;
        }
    }
    if (minfila == '' || diaPlazo == '' || horaPlazo == '') {
        alert("No debe dejar campos en blanco");
    }
    else {
        var activo = document.getElementById("idActivo").checked ? 1 : 0;
        hoja = ($('#hfIndicadorHoja').val() == "S") ? $('#cbHoja').val() : -1;

        var indcheckadicional = "N";
        if (document.getElementById("idCheckAdicional") != null)
            indcheckadicional = document.getElementById("idCheckAdicional").checked ? "S" : "N";

        $.ajax({
            type: 'POST',
            url: controlador + "formatomedicion/EditarPto",
            dataType: 'json',
            data: {
                formato: formato,
                tipoinfo: tipoinfo,
                tptomedi: tptomedi,
                punto: punto,
                limsup: limsup,
                liminf: liminf,
                estado: activo,
                hoja: hoja,
                observacion: $('#txtObservacionPunto').val(),
                fila: parseInt(minfila) || 0,
                diaPlazo: parseInt(diaPlazo) || 0,
                horaPlazo: (parseInt(minPlazo) || 0) * 60,
                fechaVigencia: fechaVigencia,
                estadoConf: estadoConf,
                plazptocodi: parseInt(plazptocodi) || 0,
                emprcodi: emprcodi,
                indcheckadicional: indcheckadicional
            },
            success: function (evt) {
                if (evt.Resultado == 1) {
                    alert("El registro se guardó correctamente.");
                    $('#popupmpto').bPopup().close();
                    mostrarListaPto();
                }
                else {
                    alert(evt.Mensaje);
                }
            },
            error: function (err) {
                alert("Ocurrio un error");
            }
        });
    }
}

function eliminarPto(idFormato, tipoInfo, tptomedi, idOrden, idPto) {
    idEmpresa = $('#cbFiltroEmpresa').val();
    if (confirm("¿Desea eliminar el punto de medición del formato? \n" +
        "Considerar lo siguiente:\n\n" +
        "1. Si el punto de medición contiene data histórica, se recomienda cambiar el estado de Activo a Desactivo. \n\n" +
        "2. Si el punto de medición fue agregado por error, se recomienda eliminar.")) {

        var idHoja = ($('#hfIndicadorHoja').val() == 'S') ? $('#cbHoja').val() : -1;

        $.ajax({
            type: 'POST',
            url: controlador + "formatomedicion/EliminarPto",
            data: {
                idEmpresa: idEmpresa,
                idFormato: idFormato,
                tipoInfo: tipoInfo,
                tptomedi: tptomedi,
                idOrden: idOrden,
                idPunto: idPto,
                hoja: idHoja
            },
            success: function (evt) {
                mostrarListaPto();
            },
            error: function (err) {
                alert("Error al mostrar Ventana para agregar puntos");
            }
        });
    }
}

///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
// 
function nuevoPto() {
    formato = $('#hfFormato').val();
    $.ajax({
        type: 'POST',
        url: controlador + "formatomedicion/MostrarAgregarPto",
        data: {
            formato: formato
        },
        global: false,
        success: function (evt) {
            $('#agregarPto').html(evt);
            setTimeout(function () {
                $('#busquedaEquipo').bPopup({
                    easing: 'easeOutBack',
                    speed: 50,
                    transition: 'slideDown'
                });
            }, 50);

            inicializarInterfazAgregarPto();

        },
        error: function (err) {
            alert("Error al mostrar Ventana para agregar puntos");
        }
    });
}

function inicializarInterfazAgregarPto() {
    $('#idFamilia').unbind();
    $('#idFamilia').on('change', function () {
        var empresa = $('#cbEmpresa').val();
        listarequipo(empresa, $(this).val());
    });

    $('#cbTipoPunto').unbind();
    $('#cbTipoPunto').on('change', function () {
        $('#trTipoMedida').show();
        $('#trTipoPuntoMedicion').show();
        if ($('#cbTipoPunto').val() == 1) {
            $('#trTipoGrupo').hide();
            $('#trGrupo').hide();
            $('#trTipoEquipo').show();
            $('#trEquipo').show();
            $('#trTipoCliente').hide();
            $('#trTipoBarra').hide();
        }
        if ($('#cbTipoPunto').val() == 2) {
            $('#trTipoGrupo').show();
            $('#trGrupo').show();
            $('#trTipoEquipo').hide();
            $('#trEquipo').hide();
            $('#trTipoCliente').hide();
            $('#trTipoBarra').hide();
        }
        if ($('#cbTipoPunto').val() == 3) {
            $('#trTipoGrupo').hide();
            $('#trGrupo').hide();
            $('#trTipoEquipo').hide();
            $('#trEquipo').hide();
            $('#trTipoCliente').show();
            $('#trTipoBarra').show();
            $('#trTipoMedida').hide();
            $('#trTipoPuntoMedicion').hide();
            listarptotransferencia();
        }
    });


    $('#idorigenlectura').unbind();
    $('#idorigenlectura').on('change', function () {
        var origlectcodi = parseInt($('#idorigenlectura').val()) || 0;
        listarEmpresa(origlectcodi);
    });

    $('#cbTipoGrupo').unbind();
    $('#cbTipoGrupo').on('change', function () {
        cargarGrupos();
    });

    $('#cbGrupo').unbind();
    $('#cbGrupo').on('change', function () {
        listarpto($('#cbGrupo').val(), 2)
    });

    $('#cbTipoCliente').unbind();
    $('#cbTipoCliente').on('change', function () {
        listarptotransferencia();
    });

    $('#cbTipoBarra').unbind();
    $('#cbTipoBarra').on('change', function () {
        listarptotransferencia();
    });

    $('#btnFiltroPto').unbind();
    $('#btnFiltroPto').click(function () {
        mostrarFiltrosSegunPtomedicodi();
    });

    $('#btnAgregarPto').unbind();
    $('#btnAgregarPto').on('click', function () {
        agregarNuevoPto();
    });

    empresa = $('#cbEmpresa2').val();
    listarequipo(empresa, -2);
}

function mostrarFiltrosSegunPtomedicodi() {
    var origlectcod = $('#idorigenlectura').val() || 0;
    var ptomedicodi = $("#txtFiltroPo").val() || 0;

    $.ajax({
        type: 'POST',

        dataType: 'json',
        url: controlador + "formatomedicion/ListarDataFiltroXPtomedicodi",
        data: {
            opcion: $("#cbTipoPunto").val(),
            ptomedicodi: ptomedicodi,
            origlectcodi: origlectcod
        },
        success: function (model) {
            if (model.StrResultado != -1) {
                if (model.Ptomedicodi > 0) {
                    $("#idorigenlectura").val(model.Origlectcodi);

                    cargarComboEmpresa(model.ListaEmpresa2, model.IdEmpresa);

                    if ($('#cbTipoPunto').val() == 1) {
                        cargarComboFamilia(model.ListaFamilia, model.IdFamilia);

                        cargarComboEquipo(model.ListaEquipo, model.IdEquipo);
                    }
                    if ($('#cbTipoPunto').val() == 2) {
                        cargarComboTipoGrupo(model.ListaTipoGrupo, model.IdTipoGrupo);
                        cargarComboGrupo(model.ListaGrupos, model.IdGrupo);
                    }
                    if ($('#cbTipoPunto').val() == 3) {
                    }

                    cargarComboPto(model.ListaPtos, model.Ptomedicodi);

                    var opcion = $('#idPunto option:selected').html();
                    setearMedidaSegunPto(opcion);
                } else {
                    $("#cbEmpresa").val(model.IdEmpresa);
                    $("#idFamilia").val(model.IdFamilia);
                    $("#idequipo").val(model.IdEquipo);
                    $("#idLectura").val(model.IdLectura);
                    $("#idPunto").val(model.Ptomedicodi);
                }
            } else {
                alert(model.Resultado);
            }
        },
        error: function (err) {
            alert("Error al mostrar lista de Tipo de medida");
        }
    });
}

function agregarNuevoPto() {
    var punto = $('#idPunto').val();
    var medida = $('#idMedidas').val();
    var limsup = parseFloat($('#idLimSupNuevo').val()) || 0;
    var liminf = parseFloat($('#idLimInfNuevo').val()) || 0;
    var formato = $('#hfFormato').val();
    var empresa = $('#cbEmpresa').val();
    var hoja = parseInt($('#cbHoja').val()) || -1;
    var idTipoPtoMedicion = $('#idTipoPtoMedicion').val();



    if (punto == -2) {
        alert("Seleccionar punto de medición");
    }
    else {
        if (medida == 0) {
            alert("Seleccionar medida")
        }
        else {

            $('#busquedaEquipo').bPopup().close();
            $.ajax({
                type: 'POST',
                url: controlador + 'formatomedicion/AgregarPto',
                dataType: 'json',
                data: {
                    empresa: empresa,
                    formato: formato,
                    punto: punto,
                    medida: medida,
                    limsup: limsup,
                    liminf: liminf,
                    hoja: hoja,
                    tipoPtoMedicion: idTipoPtoMedicion
                },
                cache: false,
                success: function (res) {
                    switch (res.Resultado) {
                        case 1:
                            if (res.Resultado == 1) {
                                addItemEmpresa();
                                mostrarListaPto();
                            }
                            break;
                        case -1:
                            alert("Error en BD ptos de medicion: " + res.Descripcion);
                            break;
                        case 0:
                            alert("Ya existe la información ingresada");
                            break;
                    }

                },
                error: function (err) {
                    alert("Error al grabar punto de medición");
                }
            });
        }
    }

}

/////////////////////////////////////////////////
/// Filtros
/////////////////////////////////////////////////

function listarEmpresa(origlect) {
    $("#cbEmpresa").empty();
    $("#cbEmpresa").append('<option value="0" selected="selected"> [Seleccionar Empresa] </option>');

    $.ajax({
        type: 'POST',
        url: controlador + "formatomedicion/ListarEmpresas",
        data: {
            origlectcodi: origlect
        },

        success: function (model) {
            cargarComboEmpresa(model.ListaEmpresa2, 0);

            $('#cbEmpresa').unbind();
            $('#cbEmpresa').change(function () {
                var origlectcodi = parseInt($('#idorigenlectura').val()) || 0;
                var empresa = parseInt($('#cbEmpresa').val()) || 0;
                listarFamilia(empresa, origlectcodi);
                listarTipogrupo(empresa, origlectcodi);
            });

            var origlectcodi = parseInt($('#idorigenlectura').val()) || 0;
            var empresa = parseInt($('#cbEmpresa').val()) || 0;
            listarFamilia(empresa, origlectcodi);
            listarTipogrupo(empresa, origlectcodi);
        },
        error: function (err) {
            alert("Error al mostrar lista de empresas");
        }
    });
}

function cargarComboEmpresa(lista, id) {
    $("#cbEmpresa").empty();
    if (id <= 0 && lista.length != 1)
        $("#cbEmpresa").append('<option value="0" selected="selected"> [Seleccionar Empresa] </option>');
    for (var i = 0; i < lista.length; i++) {
        var regEmp = lista[i];
        var selEmp = id == regEmp.Emprcodi ? 'selected="selected"' : '';
        $("#cbEmpresa").append('<option value=' + regEmp.Emprcodi + ' ' + selEmp + '  >' + regEmp.Emprnomb + '</option>');
    }
}

function listarFamilia(empresa, origlect) {
    $("#idFamilia").empty();
    $("#idFamilia").append('<option value="0" selected="selected"> [Seleccionar Tipo de Equipo] </option>');

    $.ajax({
        type: 'POST',
        url: controlador + "formatomedicion/ListarFamilia",
        data: {
            empresa: empresa,
            origlectcodi: origlect
        },

        success: function (model) {
            cargarComboFamilia(model.ListaFamilia, 0);

            $('#idFamilia').unbind();
            $('#idFamilia').change(function () {
                var origlectcodi = parseInt($('#idorigenlectura').val()) || 0;
                var empresa = parseInt($('#cbEmpresa').val()) || 0;
                var familia = parseInt($("#idFamilia").val()) || 0;

                listarequipo(empresa, familia, origlectcodi);
            });

            var origlectcodi = parseInt($('#idorigenlectura').val()) || 0;
            var empresa = parseInt($('#cbEmpresa').val()) || 0;
            var familia = parseInt($("#idFamilia").val()) || 0;

            listarequipo(empresa, familia, origlectcodi);
        },
        error: function (err) {
            alert("Error al mostrar lista de Tipo de equipos");
        }
    });
}

function cargarComboFamilia(lista, id) {
    $("#idFamilia").empty();
    if (id <= 0 && lista.length != 1)
        $("#idFamilia").append('<option value="0" selected="selected"> [Seleccionar Tipo de Equipo] </option>');
    for (var i = 0; i < lista.length; i++) {
        var reg = lista[i];
        var sel = id == reg.Famcodi ? 'selected="selected"' : '';
        $("#idFamilia").append('<option value=' + reg.Famcodi + ' ' + sel + '  >' + reg.Famnomb + '</option>');
    }
}

function listarTipogrupo(empresa, origlect) {
    $("#cbTipoGrupo").empty();
    $("#cbTipoGrupo").append('<option value="-1" selected="selected">--SELECCIONE--</option>');

    $.ajax({
        type: 'POST',
        url: controlador + "formatomedicion/ListarTipoGrupo",
        data: {
            empresa: empresa,
            origlectcodi: origlect
        },

        success: function (model) {
            cargarComboTipoGrupo(model.ListaTipoGrupo, 0);

            $('#cbTipoGrupo').unbind();
            $('#cbTipoGrupo').change(function () {
                cargarGrupos();
            });

            cargarGrupos();
        },
        error: function (err) {
            alert("Error al mostrar lista de Tipo de grupos");
        }
    });
}

function cargarComboTipoGrupo(lista, id) {
    $("#cbTipoGrupo").empty();
    if (id <= 0 && lista.length != 1)
        $("#cbTipoGrupo").append('<option value="-1" selected="selected">--SELECCIONE--</option>');
    for (var i = 0; i < lista.length; i++) {
        var reg = lista[i];
        var sel = id == reg.Catecodi ? 'selected="selected"' : '';
        $("#cbTipoGrupo").append('<option value=' + reg.Catecodi + ' ' + sel + '  >' + reg.Catenomb + '</option>');
    }
}

function cargarGrupos() {
    $('#cbGrupo').get(0).options.length = 0;
    $('#cbGrupo').get(0).options[0] = new Option("--SELECCIONE--", "-1");

    var empresa = parseInt($('#cbEmpresa').val()) || 0;
    if ($('#cbTipoGrupo').val() != "") {

        $.ajax({
            type: 'POST',
            url: controlador + 'formatomedicion/obtenergrupos',
            data: {
                categoria: $('#cbTipoGrupo').val(),
                idEmpresa: empresa
            },
            dataType: 'json',
            success: function (result) {
                cargarComboGrupo(result, -1);

            },
            error: function (err) {
                alert("Error al cargar los grupos");
            }
        });
    }
}

function cargarComboGrupo(lista, id) {
    $("#cbGrupo").empty();
    if (id <= 0 && lista.length != 1)
        $("#cbGrupo").append('<option value="-1" selected="selected">--SELECCIONE--</option>');
    for (var i = 0; i < lista.length; i++) {
        var reg = lista[i];
        var sel = id == reg.Grupocodi ? 'selected="selected"' : '';
        $("#cbGrupo").append('<option value=' + reg.Grupocodi + ' ' + sel + '  >' + reg.Gruponomb + '</option>');
    }
}

function listarequipo(empresa, familia) {
    $("#idequipo").empty();
    $("#idequipo").append('<option value="0" selected="selected"> [Seleccionar Equipo] </option>');

    $.ajax({
        type: 'POST',
        url: controlador + "formatomedicion/listarequipo",
        data: {
            empresa: empresa, familia: familia
        },
        global: false,
        success: function (model) {
            cargarComboEquipo(model.ListaEquipo, 0);

            $('#idequipo').unbind();
            $('#idequipo').change(function () {
                listarpto($(this).val(), 1);
            });

            var idEquipo = parseInt($('#idequipo').val()) || 0;

            listarpto(idEquipo, 1);
        },
        error: function (err) {
            alert("Error al mostrar lista de equipos");
        }
    });
}

function cargarComboEquipo(lista, id) {
    $("#idequipo").empty();
    if (id <= 0 && lista.length != 1)
        $("#idequipo").append('<option value="0" selected="selected"> [Seleccionar Equipo] </option>');
    for (var i = 0; i < lista.length; i++) {
        var regEq = lista[i];
        var selEq = id == regEq.Equicodi ? 'selected="selected"' : '';
        $("#idequipo").append('<option value=' + regEq.Equicodi + ' ' + selEq + '  >' + regEq.Equinomb + '</option>');
    }
}

function listarpto(codigo, opcion) {
    $("#idPunto").empty();
    var origlectcodi = $('#idorigenlectura').val() || 0;

    $.ajax({
        type: 'POST',
        global: false,
        url: controlador + "formatomedicion/listarpto",
        data: {
            origlectcodi: origlectcodi,
            codigo: codigo,
            opcion: opcion
        },
        success: function (model) {
            var ptomedicodi = $("#txtFiltroPo").val() || 0;
            cargarComboPto(model.ListaPtos, ptomedicodi);

            $('#idPunto').unbind();
            $('#idPunto').on('change', function () {
                var opcion = $('#idPunto option:selected').html();
                setearMedidaSegunPto(opcion);
            });
        },
        error: function (err) {
            alert("Error al mostrar lista de ptos");
        }
    });
}

function cargarComboPto(lista, id) {
    $("#idPunto").empty();

    if (id <= 0 && lista.length != 1)
        $("#idPunto").append('<option value="0" selected="selected"> [Seleccionar Pto] </option>');

    for (var i = 0; i < lista.length; i++) {
        var reg = lista[i];
        var selPto = id == reg.Ptomedicodi ? 'selected="selected"' : '';
        var nombrePunto = reg.Ptomedicodi + "-" + reg.Ptomedielenomb + "-" + reg.Tipoinfocodi;//+ "-" + reg.Origlectnombre;
        var codigoPunto = reg.Ptomedicodi; //+ "/" + reg.Tipoinfocodi + "/" + reg.Tipoptomedicodi + "/" + reg.Tipoinfoabrev + "/" + reg.Tipoptomedinomb;
        $("#idPunto").append('<option value=' + codigoPunto + ' ' + selPto + '  >' + nombrePunto + '</option>');
    }
}

function setearMedidaSegunPto(opcion) {
    $("#idLimSupNuevo").val('');
    $("#idLimInfNuevo").val('');

    var param = opcion.split("-");

    if ($('#cbTipoPunto').val() != "3") {
        var tipoinfocodi = parseInt(param[2]);
        $('#idMedidas').val(tipoinfocodi);

        if (tipoinfocodi == 1) {
            $("#idLimSupNuevo").val(10000);
            $("#idLimInfNuevo").val(0);
        }
        if (tipoinfocodi == 2) {
            $("#idLimSupNuevo").val(1000);
            $("#idLimInfNuevo").val(-1000);
        }
    }
}

function listarptotransferencia() {

    $.ajax({
        type: 'POST',
        global: false,
        url: controlador + "formatomedicion/listarpto",
        data: {
            codigo: -1,
            opcion: 3,
            idEmpresa: $('#cbEmpresa').val(),
            cliente: $('#cbTipoCliente').val(),
            barra: $('#cbTipoBarra').val()
        },
        success: function (model) {
            cargarComboPto(model.ListaPtos, 0);

            $('#idMedidas').val("3");
            $('#idTipoPtoMedicion').val("-1");
        },
        error: function (err) {
            alert("Error al mostrar lista de ptos");
        }
    });
}

function addItemEmpresa() {
    var filtroDefault = $('#hfFiltroEmpresa').val();

    //solo agregar empresa cuando exista combo de empresa
    if (filtroDefault != "-1") {
        var empresa = $('#cbEmpresa').val();
        var textoEmp = $('#cbEmpresa option:selected').text();
        if (!buscarEmpresa(empresa)) {
            $('#cbFiltroEmpresa').append($('<option>', {
                value: empresa,
                text: textoEmp
            }));
            $('#cbFiltroEmpresa').val(empresa);
        }
    }
}

function buscarEmpresa(idEmpresa) {
    var resultado = false;
    $("#cbFiltroEmpresa option").each(function () {
        if (idEmpresa == $(this).attr('value')) {
            resultado = true;
        }
    });
    return resultado;
}

//////////////////////////////////////////////////////////////////////////////////////////////////////////
///
function listarHistoricoExtPlazo(formatcodi, tptomedi, tipoinfo) {

    $.ajax({
        type: 'POST',
        url: controlador + '/formatomedicion/' + 'ListarHistorico',
        data: {
            formatcodi: formatcodi,
            tptomedi: tptomedi,
            tipoinfo: tipoinfo
        },
        success: function (result) {
            $('#listadoHistoExtPla').html(result);

            $('#tablaListadoPlazos').dataTable(
                {
                    "scrollX": true,
                    "scrollY": "100px",
                    "scrollCollapse": true,
                    "sDom": 't',
                    "ordering": false,
                }
            );
            var valorperiodo = parseInt($('#hfPeriodo').val()) || 0;
            var dato = $('#hfFechaFinal').val();
            if ($('#hfFechaFinal').val() != '-1') {
                if (valorperiodo == 3 || valorperiodo == 5) {
                    strFecha = dato.substr(3, 2) + " " + dato.substr(6, 9);
                    $('#txtFechaVigencia').val(strFecha);
                } else {
                    $('#txtFechaVigencia').val($('#hfFechaFinal').val());
                }
                $('#txtDiaPlazo').val($('#hfdiaFinPlazo').val());
                $('#txtHoraPlazo').val($('#hfhoraFinPlazo').val());
                $('#txtMinFilas').val($('#hfminFilaPlazo').val());
            }
            else {
                if (valorperiodo == 3 || valorperiodo == 5) {
                    var valor = $('#txtMesPeriodo').val();
                    strFecha = valor;
                    $('#txtFechaVigencia').val(strFecha);
                } else {
                    $('#txtFechaVigencia').val($('#hfFechaActual').val());
                }
                $('#txtDiaPlazo').val('0');
                $('#txtHoraPlazo').val('0');
                $('#txtMinFilas').val('0');
            }

            actualizarVistaPrevia();
            actualizarVistaPreviaIncremento();

        },
        error: function (err) {
            alert('Ha ocurrido un error: ' + err);
        }
    });

    actualizarVistaPrevia();
}

function obtenerDatoHoraMin(hfMinutos) {
    var arrayHM = [];

    var txtHoras = "00" + Math.floor(hfMinutos / 60).toString();
    var horas = txtHoras.substr(txtHoras.length - 2, 2);
    var txtMinutos = "00" + (hfMinutos % 60).toString();
    var minutos = txtMinutos.substr(txtMinutos.length - 2, 2);

    arrayHM.push(horas);
    arrayHM.push(minutos);

    return arrayHM;
}


function editarPlazoExtension(plzCodi, dia, horas, fechavigencia, filas) {
    var valorperiodo = parseInt($('#hfPeriodo').val()) || 0;

    if (valorperiodo == 3 || valorperiodo == 5) {
        strFecha = fechavigencia.substr(3, 2) + " " + fechavigencia.substr(6, 9);
        $("#txtFechaVigencia").val(strFecha);
    }
    else {
        $("#txtFechaVigencia").val(fechavigencia);
    }
    $("#txtDiaPlazo").val(dia);
    $("#txtHoraPlazo").val(horas);

    $('#txtMinFilas').val(filas);
    $('#hfPlzPtoCodi').val(plzCodi);

    actualizarVistaPrevia();
    actualizarVistaPreviaIncremento();
}

function eliminarPlazoExtension(id, formatcodi, punto, tipoinfo) {

    if (confirm('¿Desea eliminar el registro?')) {
        $.ajax({
            type: 'POST',
            dataType: 'json',
            traditional: true,
            url: controlador + 'formatomedicion/PlazoExtenEliminar',
            data: {
                id: id
            },
            cache: false,
            success: function (result) {
                if (result.Resultado == '-1') {
                    alert('Ha ocurrido un error:' + result.Mensaje);
                } else {
                    alert("Se eliminó correctamente el registro");
                    listarHistoricoExtPlazo(formatcodi, punto, tipoinfo);
                }
            },
            error: function (err) {
                alert('Ha ocurrido un error.');
            }
        });
    }
}

//////////////////////////////////////////////////////////////////////////////////////////////////////////
///
function agregarHojaExcelweb() {
    var idFormato = parseInt($('#hfFormato').val()) || 0;
    var idLectura = parseInt($('#IdLecturaNuevo').val()) || 0;
    var idCabecera = parseInt($('#IdCabeceraNuevo').val()) || 0;
    var nombreHoja = $('#nombreHojaExcelNuevo').val();

    if (idFormato > 0 && idLectura > 0 && idCabecera > 0 && nombreHoja != null && nombreHoja != '') {
        $.ajax({
            type: 'POST',
            url: controlador + "formatomedicion/AgregarHojaExcelWeb",
            data: {
                formatcodi: idFormato,
                lectcodi: idLectura,
                cabcodi: idCabecera,
                nombre: nombreHoja
            },
            success: function (evt) {
                switch (evt.Resultado) {
                    case 1:
                        $('#popupNuevaHoja').bPopup().close();
                        location.reload();
                        break;
                    default:
                        alert('Ha ocurrido un error: ' + evt.Mensaje);
                        break;
                }
            },
            error: function (err) {
                alert("Ha ocurrido un error");
            }
        });
    } else {
        alert('Debe completar todos los campos.');
    }
}

//////////////////////////////////////////////////////////////////////////////////////////////////////////
///
function ejecutarCopiaConfig() {
    var idFormato = parseInt($('#hfFormato').val()) || 0;
    var tipoCopia = parseInt($('input[name=cbTipoCopia]:checked').val()) || 0;

    if (tipoCopia > 0) {
        if (confirm("¿Desea ejecutar la copia de configuración del formato?")) {

            $.ajax({
                type: 'POST',
                url: controlador + "formatomedicion/EjecutarCopiaConfiguracion",
                data: {
                    formatcodi: idFormato,
                    tipoCopia: tipoCopia
                },
                success: function (evt) {
                    switch (evt.Resultado) {
                        case 1:
                            alert('El proceso se realizó correctamente.');
                            $('#popupCopiaConfig').bPopup().close();
                            mostrarListaPto();
                            break;
                        case 2:
                            alert('Ha ocurrido un error: ' + 'El formato no tiene una Hoja excel web definida.');
                        case 3:
                            alert('Ha ocurrido un error: ' + 'Existen puntos de medición asociados a Hojas excel web que no pertenecen al formato.');
                        case 4:
                            alert('Ha ocurrido un error: ' + 'Existen puntos de medición asociados a Hojas excel web que no pertenecen al formato.');
                            break;
                        default:
                            alert('Ha ocurrido un error: ' + evt.Mensaje);
                            break;
                    }
                },
                error: function (err) {
                    alert("Ha ocurrido un error");
                }
            });
        }
    }
    else {
        alert('Debe seleccionar una opción');
    }
}
