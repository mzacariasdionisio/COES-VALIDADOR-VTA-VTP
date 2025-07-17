var controlador = siteRoot + 'IEOD/HorasOperacion/';
var controler = siteRoot + 'eventos/registro/';

function bitacora_Inicializar() {
    $("#btnBuscarEquipoBitacora").unbind();
    $("#btnGuardarBitacora").unbind();
    $('#cbTipoEventoBitacora').unbind();

    $('#txtHoraInicialBitacora').inputmask({
        mask: "1/2/y h:s:s",
        placeholder: "dd/mm/yyyy hh:mm:ss",
        alias: "datetime",
        hourFormat: "24"
    });

    $('#txtHoraInicialBitacora').Zebra_DatePicker({
        readonly_element: false,
        onSelect: function (date) {
            $('#txtHoraInicial').val(date + " 00:00:00");
        }
    });

    $('#txtHoraFinalBitacora').inputmask({
        mask: "1/2/y h:s:s",
        placeholder: "dd/mm/yyyy hh:mm:ss",
        alias: "datetime",
        hourFormat: "24"
    });

    $('#txtHoraFinalBitacora').Zebra_DatePicker({
        readonly_element: false,
        onSelect: function (date) {
            $('#txtHoraFinalBitacora').val(date + " 00:00:00");
        }
    });

    $('#cbTipoEventoBitacora').change(function () {
        cargarSubCausaEvento();
    });

    $('#btnBuscarEquipoBitacora').click(function () {
        var hayCambioBitacora = $("#hfBitacoraHayCambios").val();
        if (APP_MOSTRAR_MODAL_EQUIPO_YA_CARGADO == 0 && hayCambioBitacora == 0) {
            cargarBusquedaEquipo(APP_MOSTRAR_MODAL_EQUIPO_YA_CARGADO);
        } else {
            openBusquedaEquipo();
        }

        APP_MOSTRAR_MODAL_EQUIPO_YA_CARGADO++;
    });

    $('#btnGuardarBitacora').click(function () {
        guardarObjBitacora();
    });

    cargarPrevio();

    ajustarVista();
}


function cargarPrevio() {
    $('#cbTipoEventoBitacora').val($('#hfTipoEventoBitacora').val());
    $('#cbSubCausaEventoBitacora').val($('#hfSubCausaEventoBitacora').val());
    $('#cbEmpresaEventoBitacora').val($('#hfIdEmpresaEventoBitacora').val());
    $('#cbTipoOperacionBitacora').val($('#hfTipoOperacionBitacora').val());
}

cargarSubCausaEvento = function () {

    if ($('#cbTipoEventoBitacora').val() != "") {
        $.ajax({
            type: 'POST',
            url: controler + 'cargarsubcausaevento',
            dataType: 'json',
            data: { idTipoEvento: $('#cbTipoEventoBitacora').val() },
            cache: false,
            success: function (aData) {
                $('#cbSubCausaEventoBitacora').get(0).options.length = 0;
                $('#cbSubCausaEventoBitacora').get(0).options[0] = new Option("-SELECCIONE-", "");
                $.each(aData, function (i, item) {
                    $('#cbSubCausaEventoBitacora').get(0).options[$('#cbSubCausaEventoBitacora').get(0).options.length] =
                        new Option(item.Subcausadesc, item.Subcausacodi);
                });
            },
            error: function () {
                mostrarError();
            }
        });
    }
    else {
        $('#cbSubCausaEventoBitacora').get(0).options.length = 0;
    }
}

function cargarBusquedaEquipo(flag) {
    $.ajax({
        type: "POST",
        url: controlador + "BusquedaEquipo",
        data: {
            filtroFamilia: -1
        },
        global: false,
        success: function (evt) {
            $('#busquedaEquipoBitacora').html(evt);
            if ($('#hfCodigoEvento').val() == "0" || flag == 0) {
                openBusquedaEquipo();
            }
        },
        error: function (req, status, error) {
            mostrarError();
        }
    });
}

openBusquedaEquipo = function () {
    $('#busquedaEquipoBitacora').bPopup({
        easing: 'easeOutBack',
        speed: 450,
        transition: 'slideDown'
    });
    $('#txtFiltro').focus();
}

function seleccionarEquipo(equicodi, equinomb, Areanomb, Emprnomb, Famabrev, emprcodi) {

    seleccionarEquipoBitacora(equicodi, Areanomb, equinomb, Emprnomb, Famabrev, emprcodi);
}

seleccionarEquipoBitacora = function (idEquipo, substacion, equipo, empresa, famabrev, idEmpresa) {

    $('#cbEmpresaEventoBitacora').val(idEmpresa);
    $('#spanEquipo').text(substacion + ' ' + equipo);
    $('#busquedaEquipoBitacora').bPopup().close();
    $('#hfIdEquipoBitacora').val(idEquipo);

    $('#busquedaEquipoBitacora').bPopup().close();
}

guardarObjBitacora = function () {
    var mensaje = validarRegistro();
    if (mensaje == "") {
        $('#mensaje').html('');
        $('#mensaje').removeClass();
        
        $("#hfBitacoraHayCambios").val(1);

        var number = ($('#IdNumeroDivBitacora').val());

        $("#detalle" + number + " #hfBitacoraHayCambios").val(1);
        $("#detalle" + number + " #hfBitacoraHophoriniFecha").val($("#txtHoraInicialBitacora").val().substring(0, 10));
        $("#detalle" + number + " #hfBitacoraHophoriniHora").val($("#txtHoraInicialBitacora").val().substring(11, 19));
        $("#detalle" + number + " #hfBitacoraHophorfinFecha").val($("#txtHoraFinalBitacora").val().substring(0, 10));
        $("#detalle" + number + " #hfBitacoraHophorfinHora").val($("#txtHoraFinalBitacora").val().substring(11, 19));
        $("#detalle" + number + " #hfBitacoraDescripcion").val($("#txtDescripcionBitacora").val());
        $("#detalle" + number + " #hfBitacoraComentario").val($("#txtComentarioInternoBitacora").val());
        $("#detalle" + number + " #hfBitacoraDetalle").val($("#txtDetalleAdicionalBitacora").val());
        $("#detalle" + number + " #hfBitacoraIdSubCausaEvento").val($("#cbSubCausaEventoBitacora").val());
        $("#detalle" + number + " #hfBitacoraIdEvento").val($("#hfCodigoEvento").val());
        $("#detalle" + number + " #hfBitacoraIdTipoEvento").val($("#cbTipoEventoBitacora").val());
        $("#detalle" + number + " #hfBitacoraIdEquipo").val($("#hfIdEquipoBitacora").val());
        $("#detalle" + number + " #hfBitacoraIdEmpresa").val($("#cbEmpresaEventoBitacora").val());
        $("#detalle" + number + " #hfBitacoraIdTipoOperacion").val($("#cbTipoOperacionBitacora").val());


        $("#popupBitacora").bPopup().close();
    }
    else {
        mostrarAlerta(mensaje);
    }
}

validarRegistro = function () {

    var mensaje = "<ul>";
    var flag = true;
    if ($('#cbTipoEventoBitacora').val() == '') {
        mensaje = mensaje + "<li>Seleccione el tipo de evento.</li>";
        flag = false;
    }

    if ($('#txtHoraInicialBitacora').val() == '') {
        mensaje = mensaje + "<li>Ingrese la hora inicial.</li>";
        flag = false;
    }
    else {
        if (!$('#txtHoraInicialBitacora').inputmask("isComplete")) {
            mensaje = mensaje + "<li>Ingrese hora inicial.</li>";
            flag = false;
        }
    }

    if ($('#txtHoraFinalBitacora').val() == '') {
        mensaje = mensaje + "<li>Ingrese la hora final.</li>";
        flag = false;
    }
    else {
        if (!$('#txtHoraFinalBitacora').inputmask("isComplete")) {
            mensaje = mensaje + "<li>Ingrese hora final.</li>";
            flag = false;
        }
    }

    if ($('#txtDescripcionBitacora').val() == '') {
        mensaje = mensaje + "<li>Ingrese la descripción del evento.</li>";
        flag = false;
    }

    if ($('#cbSubCausaEventoBitacora').val() == '') {
        mensaje = mensaje + "<li>Seleccione la causa del evento.";
        flag = false;
    }

    mensaje = mensaje + "</ul>";

    if (flag) mensaje = "";

    return mensaje;
}

mostrarError = function () {
    alert("Ha ocurrido un error");
}

mostrarAlerta = function (mensaje) {
    $('#mensaje').removeClass();
    $('#mensaje').addClass('action-alert');
    $('#mensaje').html(mensaje);
}

mostrarExito = function () {
    $('#mensaje').removeClass();
    $('#mensaje').addClass('action-exito');
    $('#mensaje').html('La operación se realizó con éxito.');
}

function ajustarVista() {
    if (APP_OPCION == OPCION_VER) {
        $("#mensaje").hide();
        $("#cbTipoEventoBitacora").prop('disabled', 'disabled');
        $("#cbTipoOperacionBitacora").prop('disabled', 'disabled');
        $("#btnBuscarEquipoBitacora").hide();
        $("#cbEmpresaEventoBitacora").prop('disabled', 'disabled');
        $("#txtHoraInicialBitacora").prop('disabled', 'disabled');
        $("#txtHoraFinalBitacora").prop('disabled', 'disabled');
        $("#cbSubCausaEventoBitacora").prop('disabled', 'disabled');
        $("#txtDescripcionBitacora").prop('disabled', 'disabled');
        $("#txtDetalleAdicionalBitacora").prop('disabled', 'disabled');
        $("#txtComentarioInternoBitacora").prop('disabled', 'disabled');
        $("#btnGuardarBitacora").hide();
    }
}