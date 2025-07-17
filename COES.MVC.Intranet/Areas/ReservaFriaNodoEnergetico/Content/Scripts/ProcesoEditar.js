var controlador = siteRoot + 'reservafrianodoenergetico/proceso/';


$(function () {

    $('#txtNrprcFechaInicio').inputmask({
        mask: "1/2/y h:s:s",
        placeholder: "dd/mm/yyyy hh:mm:ss",
        alias: "datetime",
        hourFormat: "24"
    });

    $('#txtNrprcFechaInicio').Zebra_DatePicker({
        readonly_element: false,
        onSelect: function (date) {
            $('#txtNrprcFechaInicio').val(date);
        }
    });

    $('#txtNrprcFechaFin').inputmask({
        mask: "1/2/y h:s:s",
        placeholder: "dd/mm/yyyy hh:mm:ss",
        alias: "datetime",
        hourFormat: "24"
    });


    $('#txtNrprcFechaFin').Zebra_DatePicker({
        readonly_element: false,
        onSelect: function (date) {
            $('#txtNrprcFechaFin').val(date);
        }
    });

    $('#txtNrprcFecCreacion').inputmask({
        mask: "1/2/y h:s:s",
        placeholder: "dd/mm/yyyy hh:mm:ss",
        alias: "datetime"        
    });

    $('#txtNrprcFecCreacion').Zebra_DatePicker({
        readonly_element: false,
        onSelect: function (date) {
            $('#txtNrprcFecCreacion').val(date);
        }
    });

    $('#txtNrprcFecModificacion').inputmask({
        mask: "1/2/y h:s:s",
        placeholder: "dd/mm/yyyy",
        alias: "datetime"        
    });

    $('#txtNrprcFecModificacion').Zebra_DatePicker({
        readonly_element: false,
        onSelect: function (date) {
            $('#txtNrprcFecModificacion').val(date);
        }
    });

    $('#btnCancelar').click(function () {
        document.location.href = controlador;
    });

    $('#btnCancelar2').click(function () {
        document.location.href = controlador;
    });

    $(document).ready(function () {        
        
        $('#rbNrprcExceptuaCoesN').prop('checked', true);
        if ($('#hfNrprcExceptuaCoes').val() == 'S') { $('#rbNrprcExceptuaCoesS').prop('checked', true); }
        if ($('#hfNrprcExceptuaCoes').val() == 'N') { $('#rbNrprcExceptuaCoesN').prop('checked', true); }

        $('#rbNrprcExceptuaOsinergminN').prop('checked', true);
        if ($('#hfNrprcExceptuaOsinergmin').val() == 'S') { $('#rbNrprcExceptuaOsinergminS').prop('checked', true); }
        if ($('#hfNrprcExceptuaOsinergmin').val() == 'N') { $('#rbNrprcExceptuaOsinergminN').prop('checked', true); }

        $('#rbNrprcTipoIngresoM').prop('checked', true);
        if ($('#hfNrprcTipoIngreso').val() == 'A') { $('#rbNrprcTipoIngresoA').prop('checked', true); }
        if ($('#hfNrprcTipoIngreso').val() == 'M') { $('#rbNrprcTipoIngresoM').prop('checked', true); }

        $('#rbNrprcHoraFallaN').prop('checked', true);
        if ($('#hfNrprcHoraFalla').val() == 'S') { $('#rbNrprcHoraFallaS').prop('checked', true); }
        if ($('#hfNrprcHoraFalla').val() == 'N') { $('#rbNrprcHoraFallaN').prop('checked', true); }

        $('#rbNrprcFiltradoN').prop('checked', true);
        if ($('#hfNrprcFiltrado').val() == 'S') { $('#rbNrprcFiltradoS').prop('checked', true); }
        if ($('#hfNrprcFiltrado').val() == 'N') { $('#rbNrprcFiltradoN').prop('checked', true); }

        $('#cbNrperCodi').val($('#hfNrperCodi').val());
        $('#cbGrupoCodi').val($('#hfGrupoCodi').val());
        $('#cbNrcptCodi').val($('#hfNrcptCodi').val());
        

        if ($('#hfAccion').val() == 0) {
            $('#btnGrabar').hide();
        }

        if ($('#hfNrprcCodi').val() != 0) {
            $('#cbNrperCodi').prop("disabled", true);
            $('#cbGrupoCodi').prop("disabled", true);
            $('#cbNrcptCodi').prop("disabled", true);
        }
    });

    $('#btnGrabar').click(function () {
        grabar();
    });
});

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

validarRegistro = function () {

    var mensaje = "<ul>";
    var flag = true;
    
    $('#hfNrperCodi').val($('#cbNrperCodi').val());
    $('#hfGrupoCodi').val($('#cbGrupoCodi').val());
    $('#hfNrcptCodi').val($('#cbNrcptCodi').val());

    if ($('#rbNrprcExceptuaCoesS').is(':checked')) {
        $('#hfNrprcExceptuaCoes').val('S');
    }

    if ($('#rbNrprcExceptuaCoesN').is(':checked')) {
        $('#hfNrprcExceptuaCoes').val('N');
    }

    if ($('#rbNrprcExceptuaOsinergminS').is(':checked')) {
        $('#hfNrprcExceptuaOsinergmin').val('S');
    }

    if ($('#rbNrprcExceptuaOsinergminN').is(':checked')) {
        $('#hfNrprcExceptuaOsinergmin').val('N');
    }

    if ($('#rbNrprcTipoIngresoA').is(':checked')) {
        $('#hfNrprcTipoIngreso').val('A');
    }

    if ($('#rbNrprcTipoIngresoM').is(':checked')) {
        $('#hfNrprcTipoIngreso').val('M');
    }

    if ($('#rbNrprcHoraFallaS').is(':checked')) {
        $('#hfNrprcHoraFalla').val('S');
    }

    if ($('#rbNrprcHoraFallaN').is(':checked')) {
        $('#hfNrprcHoraFalla').val('N');
    }

    if ($('#rbNrprcFiltradoS').is(':checked')) {
        $('#hfNrprcFiltrado').val('S');
    }

    if ($('#rbNrprcFiltradoN').is(':checked')) {
        $('#hfNrprcFiltrado').val('N');
    }

    //completar fecha
    completarFecha("txtNrprcFechaInicio");
    completarFecha("txtNrprcFechaFin");


    if ($('#cbNrperCodi').val() == "-1" || $('#cbNrperCodi').val() == null || $('#cbNrperCodi').val() == undefined) {
        mensaje += "<li>Debe seleccionar periodo</li>";//\n";
        flag = false;
    }

    if ($('#cbGrupoCodi').val() == "-1" || $('#cbGrupoCodi').val() == null || $('#cbGrupoCodi').val() == undefined) {
        mensaje += "<li>Debe seleccionar Grupo</li>";//\n";
        flag = false;
    }

    if ($('#cbNrcptCodi').val() == "-1" || $('#cbNrcptCodi').val() == null || $('#cbNrcptCodi').val() == undefined) {
        mensaje += "<li>Debe seleccionar concepto</li>";//\n";
        flag = false;
    }

    if (isNaN($('#txtNrprcHoraUnidad').val()) || (($('#txtNrprcHoraUnidad').val() + "*") == "*")) {
        mensaje += "<li>Debe ingresar Hora Unidad</li>";//\n";
        flag = false;
    }

    if (isNaN($('#txtNrprcHoraCentral').val()) || (($('#txtNrprcHoraCentral').val() + "*") == "*")) {
        mensaje += "<li>Debe ingresar Hora Central</li>";//\n";
        flag = false;
    }

    if (isNaN($('#txtNrprcPotenciaLimite').val()) || (($('#txtNrprcPotenciaLimite').val() + "*") == "*")) {
        mensaje += "<li>Debe ingresar Potencia Límite</li>";//\n";
        flag = false;
    }

    if (isNaN($('#txtNrprcPotenciaRestringida').val()) || (($('#txtNrprcPotenciaRestringida').val() + "*") == "*")) {
        mensaje += "<li>Debe ingresar Potencia Restringida</li>";//\n";
        flag = false;
    }

    if (isNaN($('#txtNrprcPotenciaAdjudicada').val()) || (($('#txtNrprcPotenciaAdjudicada').val() + "*") == "*")) {
        mensaje += "<li>Debe ingresar Potencia Adjudicada</li>";//\n";
        flag = false;
    }

    if (isNaN($('#txtNrprcPotenciaEfectiva').val()) || (($('#txtNrprcPotenciaEfectiva').val() + "*") == "*")) {
        mensaje += "<li>Debe ingresar Potencia Efectiva</li>";//\n";
        flag = false;
    }

    if (isNaN($('#txtNrprcPotenciaPromMedidor').val()) || (($('#txtNrprcPotenciaPromMedidor').val() + "*") == "*")) {
        mensaje += "<li>Debe ingresar Potencia Promedio de Medidor</li>";//\n";
        flag = false;
    }

    if (isNaN($('#txtNrprcPrctjRestringEfect').val()) || (($('#txtNrprcPrctjRestringEfect').val() + "*") == "*")) {
        mensaje += "<li>Debe ingresar Porcentaje Potencia Restringida Efectiva</li>";//\n";
        flag = false;
    }

    if (isNaN($('#txtNrprcVolumenCombustible').val()) || (($('#txtNrprcVolumenCombustible').val() + "*") == "*")) {
        mensaje += "<li>Debe ingresar Volumen de Combustible</li>";//\n";
        flag = false;
    }

    if (isNaN($('#txtNrprcRendimientoUnidad').val()) || (($('#txtNrprcRendimientoUnidad').val() + "*") == "*")) {
        mensaje += "<li>Debe ingresar Rendimiento de Unidad</li>";//\n";
        flag = false;
    }

    if (isNaN($('#txtNrprcEde').val()) || (($('#txtNrprcEde').val() + "*") == "*")) {
        mensaje += "<li>Debe ingresar EDE</li>";//\n";
        flag = false;
    }

    if (isNaN($('#txtNrprcPadre').val()) || (($('#txtNrprcPadre').val() + "*") == "*")) {
        mensaje += "<li>Debe ingresar Padre (-1: valor por defecto)</li>";//\n";
        flag = false;
    }

    if (isNaN($('#txtNrprcSobrecosto').val()) || (($('#txtNrprcSobrecosto').val() + "*") == "*")) {
        mensaje += "<li>Debe ingresar Sobrecosto</li>";//\n";
        flag = false;
    }

    if (isNaN($('#txtNrprcRpf').val()) || (($('#txtNrprcRpf').val() + "*") == "*")) {
        mensaje += "<li>Debe ingresar porcentaje RPF</li>";//\n";
        flag = false;
    }


    if (isNaN($('#txtNrprcTolerancia').val()) || (($('#txtNrprcTolerancia').val() + "*") == "*")) {
        mensaje += "<li>Debe ingresar Tolerancia</li>";//\n";
        flag = false;
    }


    if ($('#rbNrprcFiltradoN').is(':checked')) {
        $('#hfNrprcFiltrado').val('N');
    }
    
    if (flag) mensaje = "";
    return mensaje;
}

grabar = function () {
    var mensaje = validarRegistro();
    
    if (mensaje == "") {

        $.ajax({
            type: 'POST',
            url: controlador + "grabar",
            dataType: 'json',
            data: $('#form').serialize(),
            success: function (result) {
                if (result != "-1") {
                   
                    mostrarExito();
					$('#hfNrprcCodi').val(result);
					document.location.href = controlador;

                }
                else {
                    mostrarError();
                }
            },
            error: function () {
                mostrarError();
            }
        });
    }
    else {
        mostrarAlerta(mensaje);
    }
}

mostrarError = function () {
    alert("Ha ocurrido un error");
}

completarFecha = function (id) {

    var valor = $('#' + id).val() + " ";

    if (valor.trim() == "")
        return;

    var date = valor.split(' ')[0].split('/');
    var time = valor.split(' ')[1].split(':');

    var date0 = (validarNumero(date[0])) ? date[0] : "00";
    var date1 = (validarNumero(date[1])) ? date[1] : "00";
    var date2 = (validarNumero(date[2])) ? date[2] : "00";

    var time0 = (validarNumero(time[0])) ? time[0] : "00";
    var time1 = (validarNumero(time[1])) ? time[1] : "00";
    var time2 = (validarNumero(time[2])) ? time[2] : "00";

    $('#' + id).val(date0 + "/" + date1 + "/" + date2 + " " + time0 + ":" + time1 + ":" + time2);

}

validarNumero = function (valor) {
    return !isNaN(parseFloat(valor)) && isFinite(valor);
}