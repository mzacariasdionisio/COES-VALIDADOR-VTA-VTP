var controlador = siteRoot + 'hidrologia/'

var valAreaOp2 = "";
$(function () {

    var i = $('#hfTipoPtomedicion2').val();
    $('#cbEmpresaPto').val($('#hfEmpresaPto').val());
    $('#cdPtomedibarranomb2').val($('#hfPtomedibarranomb2').val());
    $('#cdPtomedielenomb2').val($('#hfPtomedielenomb2').val());
    $('#cbFamiliaPot').val($('#hfFamilia').val());
    $('#cbEquipo').val($('#hfEquipo').val());
    $('#cbOrigenLectura2').val($('#hfOrigenLectura2').val());
    $('#cdOrden').val($('#hfIdOrden').val());
    $('#cdOsicodi').val($('#hfOsicodi').val());
    $('#cbTptomedicion').val($('#hfTipoPtomedicion2').val());
    //$('#cbGrupoPto').val($('#hfGrupoPto').val());
    //Adicionales para el caso de demanda en barras
    $('#cbEmpresaBarra').val($('#hfEmpresaBarra').val());
    $('#cbAreaOperativa').val($('#hfAreaOperativa').val());
    $('#cbTipoPunto').val($('#hfTipoPunto').val());
    $('#cbTipoGrupo').val($('#hfTipoGrupo').val());
    $('#cbEstadoPto').val($('#hfEstadoPto').val());
    $('#cbCliente').val($('#hfCliente').val());
    $('#cbBarras').val($('#hfBarraVTP').val());
    $('#trGrupoPto').hide();

    $('#resultadoArea').removeClass();
    $('#resultadoArea').text("");
    if ($('#cbOrigenLectura2').val() == 38) {

        cargarTipoPto();
    }
    $('#cbTipoSerie').val($('#hfTipoSerie').val());
    $('#cbTptomedicion').val($('#hfTipoPtomedicion2').val());
    cargarBarras();
    cargarGrupos2(1);

    if ($('#hfOrigenLectura2').val() == "6") {
        $('#trBarra').show();
        $('#trTensionBarra').show();
        $('#trAreaOperativa').show();
        $('#trEquipo').css("background-color", "#FFFFCE");
        $('#trAreaOperativa').css("background-color", "#FFFFCE");
        $('#trBarra').css("background-color", "#CCF2D9");
        $('#trTensionBarra').css("background-color", "#CCF2D9");
    }
    else {
        $('#trBarra').hide();
        $('#trTensionBarra').hide();
        $('#trAreaOperativa').hide();
        $('#trEquipo').css("background-color", "#FFFFFF");
        $('#trAreaOperativa').css("background-color", "#FFFFFF");
        $('#trBarra').css("background-color", "#FFFFFF");
        $('#trTensionBarra').css("background-color", "#FFFFFF");
    }

    $('#trTipoSerie').hide();

    valAreaOp2 = $('#hfAreaOp').val();
    mostrarAreaOperativa2();

    $('#cbOrigenLectura2').change(function () {
        mostrarAreaOperativa2()
    });

    ///--------Puntos de medición para grupos-------------///

    if ($('#hfTipoPunto').val() == 1) {
        $('#trTipoEquipo').show();
        $('#trEquipo').show();
        $('#trTipoGrupo').hide();
        $('#trGrupo').hide();
        $('#trTipoPunto').show();
        $('#trTipoCliente').hide();
        $('#trTipoPuntoConexion').hide();
        $('#trTipoBarra').hide();
        $('#trOrigenLectura').show();
        $('#trAbreviatura').show();
        $('#trDescripcion').show();

    }
    if ($('#hfTipoPunto').val() == 2) {
        $('#trTipoEquipo').hide();
        $('#trEquipo').hide();
        $('#trTipoGrupo').show();
        $('#trGrupo').show();
        $('#trTipoPunto').show();
        $('#trTipoCliente').hide();
        $('#trTipoPuntoConexion').hide();
        $('#trTipoBarra').hide();
        $('#trOrigenLectura').show();
        $('#trAbreviatura').show();
        $('#trDescripcion').show();
        cargarGrupos(1);

    }
    if ($('#hfTipoPunto').val() == 3) {
        $('#trTipoEquipo').hide();
        $('#trEquipo').hide();
        $('#trTipoGrupo').hide();
        $('#trGrupo').hide();
        $('#trTipoPunto').hide();
        $('#trTipoCliente').show();
        $('#trTipoPuntoConexion').show();
        $('#trTipoBarra').show();
        $('#trOrigenLectura').hide();
        $('#trAbreviatura').hide();
        $('#trDescripcion').hide();
    }

    if ($('#cbOrigenLectura2').val() == 32) {
        $('#trTipoCliente').show();
        $('#trTipoPuntoConexion').show();
    }


    ////---------------Para Ptos de Medición PR16-----------------------------------
    if ($('#hfOrigenLectura2').val() == "19" || $('#cbOrigLectura').val() == "19") {

        $('#cbFamiliaPto').val('45');
        $('#trSuministrador').show();
        $('#trTensionSuministro').show();
        $('#trSubEstacion').show();
        $('#trSuministrador').css("background-color", "#BEF781");
        $('#trTensionSuministro').css("background-color", "#CECEF6");
        $('#trSubEstacion').css("background-color", "#CECEF6");

        if ($('#hfpmedicion2').val() == '0') {
            $('#cbOrigenLectura2').val('19');
        }
    } else {

        $('#trSuministrador').hide();
        $('#trTensionSuministro').hide();
        $('#trSubEstacion').hide();
    }

    $('#cbEquipo').change(function () {
        generanombre();
        $('#resultadoArea').removeClass();
        $('#resultadoArea').text("");
        if ($('#cbOrigenLectura2').val() == "19" || $('#cbOrigLectura').val() == '19' || $('#cbFamiliaPto').val() == "45") {
            cargarTensionSuministroPR16();
            cargarUbicacionSuministroPR16();
        }
    });

    $('#cbCliente').change(function () {
        if ($('#cbOrigenLectura2').val() == 32) {
            cargarEquipos();
        }
    });

    $('#cbOrigenLectura2').change(function () {

        if ($('#cbOrigenLectura2').val() == 32) {
            $('#trTipoCliente').show();
            $('#trTipoPuntoConexion').show();
            cargarEquipos();
        }

        if ($('#cbOrigenLectura2').val() == 1 || $('#cbOrigenLectura2').val() == 2) {
            cargarGrupos2(0);
        } else {
            $('#trGrupoPto').hide();
        }

    });

    $('#cbBarra').change(function () {
        cargarTension();
    });

    $('#cbTptomedicion').change(function () {
        generanombre();
    });

    $('#cbEmpresaPto').change(function () {
        cargarEquipos();
        cargarGrupos2(0);
        $('#cbAreaOperativa').val("");
    });

    $('#cbFamiliaPto').on('change', function () {
        onchangecbFamiliaPto();
    });

    $('#cbCategoriaSuperior').on('change', function () {
        cargarCategoriaHijo();
    });

    $('#cbCategoria').on('change', function () {
        cargarSubclasificacion();
    });

    $('#cbSubclasificacion').on('change', function () {
        cargarEquiposActual();
    });

    $('#btnGrabar').click(function () {
        grabarPtoMedicion();
    });

    $('#btnActualizarArea').click(function () {
        actualizarAreaOperativa();
    });

    $('#cbTipoGrupo').on('change', function () {
        cargarGrupos(0);
    });

    $('#cbTipoPunto').on('change', function () {
        if ($('#cbTipoPunto').val() == 1) {
            $('#trTipoEquipo').show();
            $('#trEquipo').show();
            $('#trTipoGrupo').hide();
            $('#trGrupo').hide();
            $('#trTipoPunto').show();
            $('#trTipoCliente').hide();
            $('#trTipoPuntoConexion').hide();
            $('#trTipoBarra').hide();
            $('#trOrigenLectura').show();
            $('#trAbreviatura').show();
            $('#trDescripcion').show();
        }
        if ($('#cbTipoPunto').val() == 2) {
            $('#trTipoEquipo').hide();
            $('#trEquipo').hide();
            $('#trTipoGrupo').show();
            $('#trGrupo').show();
            $('#trTipoPunto').show();
            $('#trTipoCliente').hide();
            $('#trTipoPuntoConexion').hide();
            $('#trTipoBarra').hide();
            $('#trOrigenLectura').show();
            $('#trAbreviatura').show();
            $('#trDescripcion').show();

        }
        if ($('#cbTipoPunto').val() == 3) {
            $('#trTipoEquipo').hide();
            $('#trEquipo').hide();
            $('#trTipoGrupo').hide();
            $('#trGrupo').hide();
            $('#trTipoPunto').hide();
            $('#trTipoCliente').show();
            $('#trTipoPuntoConexion').show();
            $('#trTipoBarra').show();
            $('#trOrigenLectura').hide();
            $('#trAbreviatura').hide();
            $('#trDescripcion').hide();
        }

    });

    $('#cbOrigenLectura2').change(function () {
        if ($('#cbOrigenLectura2').val() == "6") {
            $('#trBarra').show();
            $('#trTensionBarra').show();
            $('#trAreaOperativa').show();

            $('#trEquipo').css("background-color", "#FFFFCE");
            $('#trAreaOperativa').css("background-color", "#FFFFCE");

            $('#trBarra').css("background-color", "#CCF2D9");
            $('#trTensionBarra').css("background-color", "#CCF2D9");
        }
        else {
            $('#trBarra').hide();
            $('#trTensionBarra').hide();
            $('#trAreaOperativa').hide();

            $('#trEquipo').css("background-color", "#FFFFFF");
            $('#trAreaOperativa').css("background-color", "#FFFFFF");
            $('#trBarra').css("background-color", "#FFFFFF");
            $('#trTensionBarra').css("background-color", "#FFFFFF");

        }
        if ($('#cbOrigenLectura2').val() == "19") {

            $('#trSuministrador').show();
            $('#trTensionSuministro').show();
            $('#trSubEstacion').show();

            $('#trSuministrador').css("background-color", "#BEF781");
            $('#trTensionSuministro').css("background-color", "#CECEF6");
            $('#trSubEstacion').css("background-color", "#CECEF6");

            $('#cbFamiliaPto').val('45');
            $('#txtTensiónSuministro').val("");
            if ($('#hfpmedicion2').val() == '0') {
                cargarAreaSuministro();
                $('#cbSuministrador').val("-1");
                $('#cbSubestacion').val("-1");
            }

        } else {

            $('#trSuministrador').hide();
            $('#trTensionSuministro').hide();
            $('#trSubEstacion').hide();
        }
        cargarTipoPto();
    });

    $('#cbEmpresaBarra').change(function () {
        cargarBarras1();
        $('#resultadoArea').removeClass();
        $('#resultadoArea').text("");
    });

    $('#btnCancelar').click(function () {
        cancelar();
    });

    cargarPrevioPto();

    $('#txtTensiónSuministro').keyup(function () {
        var val = $(this).val();
        if (isNaN(val)) {
            val = val.replace(/[^0-9\.]/g, '');
            if (val.split('.').length > 2)
                val = val.replace(/\.+$/, "");
        }
        $(this).val(val);
    });

    //Para el caso de PR16
    $('#cbSuministrador').val($('#hfSuministrador').val());
    $('#cbSubestacion').val($('#hfSubestacionr').val());

    $('#btnActualizarSuministrador').click(function () {
        actualizaSuministrador("editar");
    });

    $('#btnActualizarSuministro').click(function () {
        actualizarDatosEquipoSuministro();
    });

});

function onchangecbFamiliaPto() {
    empresa = $('#cbEmpresa').val();
    cargarEquipos();
    $('#cbAreaOperativa').val("");
    $(".clasificacion").hide();

    if ($('#cbFamiliaPto').val() == "45") {
        $('#trSuministrador').show();
        $('#trTensionSuministro').show();
        $('#trSubEstacion').show();
        $('#trSuministrador').css("background-color", "#BEF781");
        $('#trTensionSuministro').css("background-color", "#CECEF6");
        $('#trSubEstacion').css("background-color", "#CECEF6");

        cargarAreaSuministro();
        $('#cbSuministrador').val("-1");
        $('#txtTensiónSuministro').val("");
        $('#cbSubestacion').val("-1");

    } else {
        $('#trSuministrador').hide();
        $('#trTensionSuministro').hide();
        $('#trSubEstacion').hide();
    }
    if (($('#cbFamiliaPto').val() == "19") || ($('#cbFamiliaPto').val() == "42")) {
        $('#trTipoSerie').show();
    } else {
        $('#trTipoSerie').hide();
    }

}

function cargarBarras() {

    if ($('#hfTipoPunto').val() == 1) {
        $('option', '#cbBarra').remove();

        if ($('#cbEmpresaBarra').val() != "") {
            $.ajax({
                type: 'POST',
                url: controlador + 'ptomedicion/cargarbarras',
                dataType: 'json',
                global: false,
                data: {
                    empresa: $('#cbEmpresaBarra').val()
                },
                cache: false,
                success: function (aData) {
                    $('#cbBarra').get(0).options.length = 0;
                    $('#cbBarra').get(0).options[0] = new Option("--SELECCIONE--", "");
                    $.each(aData, function (i, item) {
                        $('#cbBarra').get(0).options[$('#cbBarra').get(0).options.length] = new Option(item.Text, item.Value);
                    });

                    $('#cbBarra').val($('#hfBarra').val());
                },
                error: function () {
                    alert("Ha ocurrido un error.");
                }
            });
        }
        else {
            $('option', '#cbBarra').remove();
        }
    }
}

function cargarBarras1() {
    $('option', '#cbBarra').remove();

    if ($('#cbEmpresaBarra').val() != "") {
        $.ajax({
            type: 'POST',
            url: controlador + 'ptomedicion/cargarbarras',
            dataType: 'json',
            global: false,
            data: {
                empresa: $('#cbEmpresaBarra').val()
            },
            cache: false,
            success: function (aData) {
                $('#cbBarra').get(0).options.length = 0;
                $('#cbBarra').get(0).options[0] = new Option("--SELECCIONE--", "");
                $.each(aData, function (i, item) {
                    $('#cbBarra').get(0).options[$('#cbBarra').get(0).options.length] = new Option(item.Text, item.Value);
                });
            },
            error: function () {
                alert("Ha ocurrido un error.");
            }
        });
    }
    else {
        $('option', '#cbBarra').remove();
    }
}

function cancelar() {
    $('#popupUnidad').bPopup().close();
}

function cargarPrevioPto() {
    $('#cbFamiliaPto').val($("#hfFamiliaPto").val());
}

function actualizaSuministrador(accion) {
    var ptomedicodi = $('#hfpmedicion2').val();
    var suministrador = $('#cbSuministrador').val();

    if (suministrador > 0) {
        if (accion == "nuevo") {

            $.ajax({
                type: 'POST',
                dataType: 'json',
                url: controlador + "ptomedicion/GrabarDatosPtoSuministrador",
                global: false,
                data: {
                    suministrador: suministrador,
                    ptomedicion: ptomedicodi
                },
                success: function (resultado) {
                    switch (resultado) {

                        case -1:
                            alert("Error al registrar suministrador.");
                            break;
                        case 1:
                            alert("Se actualizó.");
                            break;
                    }

                },
                error: function () {
                    alert("Ha ocurrido un error");
                }
            });

        } else {
            if (ptomedicodi > 0) {

                $.ajax({
                    type: 'POST',
                    dataType: 'json',
                    url: controlador + "ptomedicion/GrabarDatosPtoSuministrador",
                    global: false,
                    data: {
                        suministrador: suministrador,
                        ptomedicion: ptomedicodi
                    },
                    success: function (resultado) {
                        switch (resultado) {

                            case -1:
                                alert("Error al actualizar suministrador.");
                                break;
                            case 1:
                                alert("Se actualizó.");
                                break;
                        }

                    },
                    error: function () {
                        alert("Ha ocurrido un error");
                        return;
                    }
                });
            }
            else {
                alert("Se debe crear el punto de medición.");
            }
        }
    } else {
        alert("Seleccione una empresa suministradora.");
    }
}

function actualizarDatosEquipoSuministro() {
    var equipo = $('#cbEquipo').val();
    var areaSuministro = $('#cbSubestacion').val();
    var tensionSuministro = $('#txtTensiónSuministro').val();

    if (equipo > 0) {
        if (tensionSuministro.trim() != "" && tensionSuministro > 0) {
            if (areaSuministro > 0) {

                $.ajax({
                    type: 'POST',
                    dataType: 'json',
                    url: controlador + "ptomedicion/ActualizarEquipoSuministro",
                    global: false,
                    data: {
                        equipocodi: equipo,
                        area: areaSuministro,
                        tension: tensionSuministro
                    },
                    success: function (resultado) {
                        switch (resultado) {

                            case -1:
                                alert("Error al actualizar el equipo suministro");
                                break;
                            case 1:
                                cargarEquipos();
                                alert("Se actualizó el equipo suministro");
                                $('#cbEquipo').val(equipo);
                                break;
                        }

                    },
                    error: function () {
                        alert("Ha ocurrido un error");
                    }
                });

            } else {
                alert("Seleccione subestación de suministro.");
            }
        } else {
            alert("Ingrese una tensión de suministrador válida.");
        }
    } else {
        alert("Seleccionar equipo.");
    }
}

function grabarPtoMedicion() {

    var descripcion = $('#txtPtomedibarranomb2').val();
    var abreviatura = $('#txtPtomedielenomb2').val();
    var descrip = $('#txtPtomedidesc2').val();
    var empresa = $('#cbEmpresaPto').val();
    var equipo = $('#cbEquipo').val();
    var origen = $('#cbOrigenLectura2').val();
    var ptomedicodi = $('#hfpmedicion2').val();
    var tipopto = $('#cbTptomedicion').val();
    var orden = parseInt($('#cdOrden').val()) || 1;
    var osicodigo = $('#cdOsicodi').val();
    var codRef = -1;
    var TipoSerie = $('#cbTipoSerie').val();

    var tipoFuente = $('#cbTipoPunto').val();
    var tipoGrupo = $('#cbTipoGrupo').val();
    var grupo = (tipoFuente == 1 ? $('#cbGrupoPto').val() : $('#cbGrupo').val());

    /// PR16
    var famlia = $('#cbFamiliaPto').val();
    var areaSuministro = $('#cbSubestacion').val();
    var tensionSuministro = $('#txtTensiónSuministro').val();
    var suministrador = $('#cbSuministrador').val();
    ///

    //FIT- 
    var cliente = $('#cbCliente').val();
    var barra = $('#cbBarras').val();
    var ClienteCodigo = $('#cbCliente').val();
    var PuntoConexion = $('#txtPuntoConexion').val();
    var BarraCodigo = $('#cbBarras').val();

    if (origen == "6") {
        codRef = $('#cbBarra').val();
    } else if (origen == "2") {
        codRef = grupo;
    }

    if (tipoFuente == 3) {
        origen = 34;
        tipopto = 15;
        abreviatura = descripcion;
    }
    if (origen == 38) {
        if (TipoSerie == -1) {
            alert("El tipo de serie es obligatorio para origen de Series Hidrológicas")
            return
        }
    }


    if (tipopto == "" || tipopto == null) tipopto = 15;
    if (descripcion != "") {
        if (descrip != "") {
            if (abreviatura != "") {
                if (empresa > 0) {
                    if ((equipo > 0 && tipoFuente == 1) || (grupo > 0 && tipoFuente == 2) || ((cliente > 0 && barra > 0) && tipoFuente == 3)) {
                        if (origen > 0) {
                            if (tipopto > 0 || tipopto != -1) {
                                if (orden != "") {
                                    $.ajax({
                                        type: 'POST',
                                        dataType: 'json',
                                        url: controlador + "ptomedicion/grabar",
                                        global: false,
                                        data: {
                                            empresa: empresa,
                                            equipocodi: equipo,
                                            lectura: origen,
                                            ptomedicion: ptomedicodi,
                                            orden: orden,
                                            barranomb: descripcion,
                                            elenomb: abreviatura,
                                            osicodi: osicodigo,
                                            tipopto: tipopto,
                                            descripcion: descrip,
                                            codRef: codRef,
                                            tipoFuente: tipoFuente,
                                            grupo: grupo,
                                            estado: $('#cbEstadoPto').val(),
                                            clientcodi: ClienteCodigo,
                                            PuntoConexion: PuntoConexion,
                                            barracodi: BarraCodigo,
                                            TipoSerie: TipoSerie == -1 ? null : TipoSerie,
                                            areaOp: $('#cbAreaOp').val(),
                                            familia: famlia
                                        },
                                        success: function (resultado) {
                                            switch (resultado) {

                                                case 0:
                                                    alert("Error al grabar el pto de medición");
                                                    break;
                                                case 1:
                                                    alert("Se ha registrado correctamente");
                                                    if ($('#cbOrigenLectura2').val() == "19" && $('#cbFamiliaPto').val() == "45" && ptomedicodi == 0) { // En caso el punto de medicion sea nuevo y sea de PR16
                                                        actualizaSuministrador("nuevo");
                                                    }
                                                    $('#popupUnidad').bPopup().close();
                                                    buscarPtoMedicion();
                                                    break;
                                                case 2:
                                                    if ($('#cbOrigenLectura2').val() == "19" && $('#cbFamiliaPto').val() == "45" && ptomedicodi == 0) { // En caso el punto de medicion sea nuevo y sea de PR16
                                                        actualizaSuministrador("nuevo");
                                                    }
                                                    alert("Se grabó el punto de medición. El equipo tiene más de un punto de medición asignado.");
                                                    $('#popupUnidad').bPopup().close();
                                                    buscarPtoMedicion();

                                                    break;
                                                case 3:
                                                    alert("Ya existe un punto de medición con el mismo nombre en la misma empresa");
                                                    break;
                                            }

                                        },
                                        error: function () {
                                            alert("Ha ocurrido un error");
                                        }
                                    });
                                }
                                else {
                                    alert("Ingresar orden.");
                                }
                            }
                            else {
                                alert("Seleccionar tipo punto de medición.");
                            }
                        }
                        else {
                            alert("Seleccionar origen de lectura.");
                        }
                    }
                    else {
                        if (tipoFuente == 1 || tipoFuente == 2) {
                            alert("Seleccionar equipo o grupo");
                        }
                        else if (tipoFuente == 3) {
                            alert("Seleccionar cliente y barra");
                        }
                    }
                }
                else {
                    alert("Seleccionar empresa.");
                }
            }
            else {
                alert("Ingrese la abreviatura.");
            }
        }
        else {
            alert("Ingrese la descripción.");
        }
    } else {
        alert("Ingrese el nombre.");

    }

}

function cargarEquipos() {
    //se cargan las categorias, si no hay categorias se cargan los equipos
    $(".clasificacion").hide();
    $('#cbSubclasificacion').get(0).options.length = 0;
    $('#cbSubclasificacion').get(0).options[0] = new Option("--SELECCIONE--", "-3");

    if ($('#checkDetallado').is(':checked')) {
        cargarCategoria();
    } else {
        cargarEquiposActual();
    }
}

function cargarEquiposActual() {
    $('#cbEquipo').get(0).options.length = 0;
    $('#cbEquipo').get(0).options[0] = new Option("--SELECCIONE--", "0");

    var familia = $('#cbFamiliaPto').val();
    empresa = $('#cbEmpresaPto').val();

    if ($("#cbOrigenLectura2").val() == "32" && $("#cbCliente").val() > 0) {
        empresa = $('#cbCliente').val();
    }

    subclasificacion = $("#cbSubclasificacion").val();

    $.ajax({
        type: 'POST',
        url: controlador + 'ptomedicion/cargarequipos',
        dataType: 'json',
        data: {
            idEmpresa: empresa, idFamilia: familia, idCtgdet: subclasificacion
        },
        cache: false,
        global: false,
        success: function (aData) {
            $('#cbEquipo').get(0).options.length = 0;
            $('#cbEquipo').get(0).options[0] = new Option("--SELECCIONE--", "");
            $.each(aData, function (i, item) {
                $('#cbEquipo').get(0).options[$('#cbEquipo').get(0).options.length] = new Option(item.Text, item.Value);
            });
        },
        error: function () {
            alert("Ha ocurrido un error");
        }
    });
}

function cargarGrupos2(tipo) {
    var empr = $('#cbEmpresaPto').val();
    var origLect = $('#cbOrigenLectura2').val();

    if (empr != "-1" && (origLect == 1 || origLect == 2)) {

        $('#cbGrupoPto').get(0).options.length = 0;
        $('#cbGrupoPto').get(0).options[0] = new Option("--SELECCIONE--", "-1");
        $('#trGrupoPto').show();
        $.ajax({
            type: 'POST',
            url: controlador + 'ptomedicion/ObtenerGrupos2',
            data: {
                idEmpresa: empr
            },
            dataType: 'json',
            success: function (result) {
                $('#trGrupoPto').show();
                $.each(result, function (i, item) {
                    $('#cbGrupoPto').get(0).options[$('#cbGrupoPto').get(0).options.length] = new Option(item.Gruponomb, item.Grupocodi);
                });

                if (tipo == 1) {
                    $('#cbGrupoPto').val($('#hfGrupoPto').val());
                }
            },
            error: function () {
                mostrarError();
            }
        });
    } else {
        $('#trGrupoPto').hide();
    }

}

function cargarTipoPto() {
    var origLect = $('#cbOrigenLectura2').val();
    if (origLect == "38") {
        $.ajax({
            type: 'POST',
            url: controlador + 'ptomedicion/CargarTipoPtoHidro',
            dataType: 'json',
            data: { tipoOrigenLectura: origLect },
            cache: false,
            global: false,
            success: function (aData) {
                $('#cbTptomedicion').get(0).options.length = 0;
                $('#cbTptomedicion').get(0).options[0] = new Option("--SELECCIONE--", "");
                $.each(aData, function (i, item) {
                    $('#cbTptomedicion').get(0).options[$('#cbTptomedicion').get(0).options.length] = new Option(item.Text, item.Value);
                });
                if ($('#hfTipoPtomedicion2').val()) {
                    $('#cbTptomedicion').val($('#hfTipoPtomedicion2').val());

                }
                $('#cbTipoSerie').val("1");
            },
            error: function () {
                alert("Ha ocurrido un error");
            }
        });
    } else {
        $('#cbTipoSerie').val("-1");
        $.ajax({
            type: 'POST',
            url: controlador + 'ptomedicion/CargarTipoPto',
            dataType: 'json',
            data: { tipoOrigenLectura: origLect },
            cache: false,
            global: false,
            success: function (aData) {
                $('#cbTptomedicion').get(0).options.length = 0;
                $('#cbTptomedicion').get(0).options[0] = new Option("--SELECCIONE--", "");
                $.each(aData, function (i, item) {
                    $('#cbTptomedicion').get(0).options[$('#cbTptomedicion').get(0).options.length] = new Option(item.Text, item.Value);
                });
            },
            error: function () {
                alert("Ha ocurrido un error");
            }
        });
    }

}

function generanombre() {
    var nombreUnidad = "";
    var nombreEquipo = $("[name='idEquipo'] option:selected").text();
    if ($('#cbTptomedicion').val() != null) {
        nombreUnidad = $("[name='idTipoPtomedicion2'] option:selected").text();
    }
    $('#cdPtomedibarranomb2').val(nombreEquipo + " " + nombreUnidad);

    $('#cbAreaOperativa').val("");

    if ($('#cbEquipo').val() != "") {
        $.ajax({
            type: 'POST',
            url: controlador + 'ptomedicion/obtenerareaoperativa',
            data: {
                idEquipo: $('#cbEquipo').val()
            },
            global: false,
            dataType: 'json',
            success: function (result) {
                $('#cbAreaOperativa').val(result);
            },
            error: function () {
                alert("Ha ocurrido un error.");
            }
        });
    }
}

function cargarTension() {

    $('#txtTensionBarra').val("");
    if ($('#cbBarra').val() != "") {
        $.ajax({
            type: 'POST',
            url: controlador + 'ptomedicion/cargartension',
            data: {
                idEquipo: $('#cbBarra').val()
            },
            global: false,
            dataType: 'json',
            success: function (result) {
                $('#txtTensionBarra').val(result);
            },
            error: function () {
                alert("Ha ocurrido un error.");
            }
        });
    }
}

function cargarTensionSuministroPR16() {

    $('#txtTensiónSuministro').val("");
    if ($('#cbEquipo').val() != "") {
        $.ajax({
            type: 'POST',
            url: controlador + 'ptomedicion/cargartension',
            data: {
                idEquipo: $('#cbEquipo').val()
            },
            global: false,
            dataType: 'json',
            success: function (result) {
                $('#txtTensiónSuministro').val(result);
            },
            error: function () {
                alert("Ha ocurrido un error.");
            }
        });
    }
}

function actualizarAreaOperativa() {

    if ($('#cbEquipo').val() != "" && $('#cbAreaOperativa').val() != "") {
        $.ajax({
            type: 'POST',
            url: controlador + 'ptomedicion/actualizarareaoperativa',
            data: {
                idEquipo: $('#cbEquipo').val(),
                valor: $('#cbAreaOperativa').val()
            },
            global: false,
            dataType: 'json',
            success: function (result) {
                if (result == 1) {
                    $('#resultadoArea').removeClass();
                    $('#resultadoArea').addClass('action-exito');
                    $('#resultadoArea').text("OK...!");
                }
                else {
                    $('#resultadoArea').removeClass();
                    $('#resultadoArea').addClass('action-error');
                    $('#resultadoArea').text("Error...!");
                }
            },
            error: function () {
                alert("Ha ocurrido un error.");
            }
        });
    }
};

function cargarAreaSuministro() {

    var familia = $('#cbFamiliaPto').val();
    $.ajax({
        type: 'POST',
        url: controlador + 'ptomedicion/CargarAreas',
        dataType: 'json',
        data: { iFamilia: familia },
        cache: false,
        global: false,
        success: function (aData) {
            $('#cbSubestacion').get(0).options.length = 0;
            $('#cbSubestacion').get(0).options[0] = new Option("--SELECCIONE--", "");
            $.each(aData, function (i, item) {
                $('#cbSubestacion').get(0).options[$('#cbSubestacion').get(0).options.length] = new Option(item.Text, item.Value);
            });
        },
        error: function () {
            alert("Ha ocurrido un error");
        }
    });
};

function cargarUbicacionSuministroPR16() {

    $('#cbSubestacion').val("");
    if ($('#cbEquipo').val() != "") {
        $.ajax({
            type: 'POST',
            url: controlador + 'ptomedicion/CargarAreaEquipo',
            data: {
                idEquipo: $('#cbEquipo').val()
            },
            global: false,
            dataType: 'json',
            success: function (result) {
                $('#cbSubestacion').val(result);
            },
            error: function () {
                alert("Ha ocurrido un error.");
            }
        });
    }
}

function cargarCategoria() {
    console.log("a");
    $('#cbEquipo').get(0).options.length = 0;
    $('#cbEquipo').get(0).options[0] = new Option("--SELECCIONE--", "0");

    $("#clasificacion2").hide();
    $("#clasificacion3").hide();

    var ctgPadre = $("#hfIdCategoriaPadre").val();
    $('#cbCategoriaSuperior').get(0).options.length = 0;
    $('#cbCategoriaSuperior').get(0).options[0] = new Option("--SELECCIONE--", "-3");

    $.ajax({
        type: 'POST',
        url: controlador + 'ptomedicion/ListaCategoria',
        dataType: 'json',
        data: {
            idFamilia: $('#cbFamiliaPto').val(),
            ctgPadre: ctgPadre,
            idEmpresa: $('#cbEmpresaPto').val()
        },
        cache: false,
        success: function (aData) {
            if (aData.length > 0) {
                $("#clasificacion1").show();
                $.each(aData, function (i, item) {
                    $('#cbCategoriaSuperior').get(0).options[$('#cbCategoriaSuperior').get(0).options.length] = new Option(item.Text, item.Value);
                });
            } else {

                cargarEquiposActual();
            }
        },
        error: function () {
            mostrarError();
        }
    });
};

function cargarCategoriaHijo() {
    $('#cbEquipo').get(0).options.length = 0;
    $('#cbEquipo').get(0).options[0] = new Option("--SELECCIONE--", "0");

    $("#clasificacion2").hide();
    $("#clasificacion3").hide();

    var ctgPadre = $("#cbCategoriaSuperior").val();
    $('#cbCategoria').get(0).options.length = 0;
    $('#cbCategoria').get(0).options[0] = new Option("--SELECCIONE--", "-3");

    $('#cbSubclasificacion').get(0).options.length = 0;
    $('#cbSubclasificacion').get(0).options[0] = new Option("--SELECCIONE--", "-3");

    $.ajax({
        type: 'POST',
        url: controlador + 'ptomedicion/ListaCategoria',
        dataType: 'json',
        data: {
            idFamilia: $('#cbFamiliaPto').val(),
            ctgPadre: ctgPadre,
            idEmpresa: $('#cbEmpresaPto').val()
        },
        cache: false,
        success: function (aData) {
            var string = $("#cbCategoriaSuperior option:selected").text(), substring = "- Con subcategorías";
            if (string.indexOf(substring) !== -1) {
                cargarSubclasificacion($("#cbCategoriaSuperior").val());
            } else {
                if (aData.length > 0) {
                    $("#clasificacion2").show();
                    $.each(aData, function (i, item) {
                        $('#cbCategoria').get(0).options[$('#cbCategoria').get(0).options.length] = new Option(item.Text, item.Value);
                    });
                } else {
                    cargarSubclasificacion($("#cbCategoriaSuperior").val());
                }
            }
        },
        error: function () {
            mostrarError();
        }
    });
};

function cargarSubclasificacion(ctg) {
    $("#clasificacion3").hide();

    ctg = ctg == undefined || ctg == null ? $('#cbCategoria').val() : ctg;
    $('#cbSubclasificacion').get(0).options.length = 0;
    $('#cbSubclasificacion').get(0).options[0] = new Option("--SELECCIONE--", "-3");

    $.ajax({
        type: 'POST',
        url: controlador + 'ptomedicion/ListaSubclasificacion',
        dataType: 'json',
        data: {
            idCtg: ctg,
            idEmpresa: $('#cbEmpresaPto').val()
        },
        cache: false,
        success: function (aData) {
            if (aData.length > 0) {
                $("#clasificacion3").show();

                $('#cbSubclasificacion').get(0).options.length = 0;
                $.each(aData, function (i, item) {
                    $('#cbSubclasificacion').get(0).options[$('#cbSubclasificacion').get(0).options.length] = new Option(item.Text, item.Value);
                });

                cargarEquiposActual();
            } else {
                $('#cbEquipo').get(0).options.length = 0;
                $('#cbEquipo').get(0).options[0] = new Option("--SELECCIONE--", "0");
            }
        },
        error: function () {
            mostrarError();
        }
    });
};

function cargarGrupos(tipo) {
    $('#cbGrupo').get(0).options.length = 0;
    $('#cbGrupo').get(0).options[0] = new Option("--SELECCIONE--", "-1");

    if ($('#cbTipoGrupo').val() != "") {

        $.ajax({
            type: 'POST',
            url: controlador + 'ptomedicion/obtenergrupos',
            data: {
                categoria: $('#cbTipoGrupo').val()
            },
            dataType: 'json',
            success: function (result) {
                $.each(result, function (i, item) {
                    $('#cbGrupo').get(0).options[$('#cbGrupo').get(0).options.length] = new Option(item.Gruponomb, item.Grupocodi);
                });

                if (tipo == 1) {
                    $('#cbGrupo').val($('#hfGrupo').val());
                }

            },
            error: function () {
                mostrarError();
            }
        });

    }
}

function mostrarAreaOperativa2() {
    if ($('#cbOrigenLectura2').val() == "21") {
        if (valAreaOp2 != "") {
            $('#cbAreaOp').val(valAreaOp2);
        } else {
            $('#cbAreaOp').val("-1");
        }

        $('#trAreaOp').show();
    }
    else {
        $('#trAreaOp').hide();
        $('#cbAreaOp').val("-1");
    }
}