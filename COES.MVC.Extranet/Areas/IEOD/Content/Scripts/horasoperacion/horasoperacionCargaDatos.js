var controlador = siteRoot + 'IEOD/HorasOperacion/';
$(function () {
    inicializarPantalla();
});

function inicializarPantalla() {

    idFuenteDatos = idFuenteDatosHorasOperacion;
    idFormato = 0;
    tipoFormato = 0;
    dibujarPanelIeod(tipoFormato, 5, -1);
    moment.locale('es');
    $('#tabla').contextMenu({
        selector: '.context-menu-one',
        callback: function (key, options) {

            var idHopcodi = $(this).attr('id'); // extrae el codigo de equipo y la hora seleccionada

            if ($("#hfIdEnvio").val() == 0) {
                if (key == "eliminar") {

                    eliminarHorasDeOperacion(idHopcodi);
                    actualizaListadoHorasDeOperacion(evtHot);
                }
                if (key == "modificar") {

                    modificaHoraOperacion(idHopcodi);
                }
            }
        },
        items: {
            "eliminar": { name: "ELIMINAR" },
            "modificar": { name: "MODIFICAR" },
        }
    });

    $('.context-menu-one').on('click', function (e) {
        console.log('clicked', this);
    })

    $('#txtFecha').Zebra_DatePicker({
        onSelect: function () {
            limpiarBarra();
            cargarTipoCentral();
            mostrarOcultarCentral(0);
            hideMensaje();
            hideMensajeEvento();
            $('#listado').html("");
        }
    });
    $('#cbEmpresa').change(function () {
        limpiarBarra();
        cargarTipoCentral();
        mostrarOcultarCentral(0);
        hideMensaje();
        hideMensajeEvento();
        $('#listado').html("");
        dibujarPanelIeod(tipoFormato, 5, -1);
    });

    $('#btnAgregarHoraOperacion').click(function () {
        $("#newHorasOperacion .popup-title span").html('Registro de Horas de Operación');
        APP_OPCION = OPCION_NUEVO;
        var fechaValida = validarFecha($('#txtFecha').val());

        if (fechaValida) {
            fechaValida = $('#txtFecha').val();
            var strObjForm = JSON.stringify(OBJ_DATA_HORA_OPERACION);
            var objForm = JSON.parse(strObjForm);
            objForm.FechaIni = fechaValida;
            objForm.HoraIni = "00:00:00";
            objForm.FechaFin = fechaValida;
            objForm.HoraFin = "00:00:00";
            objForm.Fechahorordarranq = fechaValida;
            objForm.Hophorordarranq = '';
            objForm.FechaHophorparada = fechaValida;
            objForm.Hophorparada = '';
            objForm.Hopnotifuniesp = FLAG_UNIDAD_ESPECIAL_AGENTE_CREACION;

            view_FormularioHO(objForm);
        } else {
            if (confirm("¿La Fecha modificada no es la misma del reporte, se perderan los datos!, desea continuar?")) {
                generaListado();
            }
        }
    });

    $('#btnContinuarHoraOperacion').click(function () {
        hideMensajeEvento();
        generaHorasDeOperacionAuto(evtHot);
    });

    $('#btnConsultar').click(function () {
        generaListado();
    });

    $('#btnVerEnvios').click(function () {
        popUpListaEnvios();
    });

    $('#btnEnviarDatos').click(function () {
        isVerif = validarHorasOperacion();
        if (isVerif) {
            verEnvioHorasOperacion(1);
        }
    });

    cargarValoresIniciales();
}

function cargarValoresIniciales() {
    cargarTipoCentral();
    mostrarOcultarCentral(0);
    limpiarBarra();
}

//////////////////////////////////////////////////////////////////////////////////////////
// Detalle
//////////////////////////////////////////////////////////////////////////////////////////

function verHOP(idHopcodi) {
    APP_OPCION = OPCION_VER;

    modificaHoraOperacion(idHopcodi);

    $("#newHorasOperacion .popup-title span").html('Detalle de Hora de Operación');

    $("#cbTipoOp").prop('disabled', 'disabled');

    $("input[name=chkUnidades]").each(function () {
        $("#" + this.id).prop("disabled", "disabled");
    });
    $("select[name=cbTipoOperacion]").each(function () {
        $("#" + this.id).prop("disabled", "disabled");
    });

    $("#txtOrdenArranqueH").prop('disabled', 'disabled');
    $("#chkCompensacionOrdArrq").prop('disabled', 'disabled');

    $("#chkArranqueBlackStart").prop('disabled', 'disabled');

    $("#chkEnsayoPotenciaEfectiva").prop('disabled', 'disabled');

    $("#txtEnParaleloH").prop('disabled', 'disabled');

    $("#chkFueraServicio").prop('disabled', 'disabled');

    $("#txtOrdenParadaH").prop('disabled', 'disabled');
    $("#chkCompensacionOrdPar").prop('disabled', 'disabled');

    $("#txtFueraParaleloF").prop('disabled', 'disabled');
    $("#txtFueraParaleloH").prop('disabled', 'disabled');

    $("#TxtDescripcion").prop('disabled', 'disabled');
    $("#txtObservacion").prop('disabled', 'disabled');

    $("#chkSistemaAislado").prop('disabled', 'disabled');

    $("#cbMotOpForzada").prop('disabled', 'disabled');

    $("#chkLimTransm").prop('disabled', 'disabled');
    $("#chkAsociarLineaTransm").prop('disabled', 'disabled');

    $('#trNuevoLineaAsociada').hide();
    $('#btnAgregarLinea').hide();
    $("#nuevaLinea").prop('disabled', 'disabled');

    $("#btnAceptar2").hide();
    $("#btnCancelar2").hide();
}

function editarHOP(idHopcodi) {
    $("#mensajePrincipal").hide();

    APP_OPCION = OPCION_EDITAR;
    
    modificaHoraOperacion(idHopcodi);

    $("#newHorasOperacion .popup-title span").html('Edición de Hora de Operación');
    $(".vistaDetalle").show();
    $(".vistaDetalle.unidades").show();
}

function eliminarHOP(idHopcodi) {
    eliminarHorasDeOperacion(idHopcodi);
    actualizaListadoHorasDeOperacion(evtHot);
}