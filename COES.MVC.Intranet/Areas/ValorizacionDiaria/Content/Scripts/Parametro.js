var controlador = siteRoot + 'ValorizacionDiaria/Parametro/';


$(function () {
    $('#tab-container').easytabs();
});

$(function () {
    $('#txtFechaInicio').Zebra_DatePicker({

    });
    $('#txtFechaFinal').Zebra_DatePicker({

    });

    $('#txtFechaEneReactiva').Zebra_DatePicker({
        format: 'm Y',
        onSelect: function () {
            ListarCargosENR();
        }
    });

    $('#txtHoraEjecucion').mask("99:99:99");

    $('#txtLunes').mask("9.99");
    $('#txtMartes').mask("9.99");
    $('#txtMiercoles').mask("9.99");
    $('#txtJueves').mask("9.99");
    $('#txtViernes').mask("9.99");
    $('#txtSabado').mask("9.99");
    $('#txtDomingo').mask("9.99");

    //$('#txtValorMargenR').mask("9.99");

    //$('#txtFueraBanda').mask("9.99");
    //$('#txtCostoOtrosEquipos').mask("9.99");
    //$('#txtFRECTotal').mask("9.99");
    ListarCargosENR(); // cambio

});
$(function () {
    $('#btnGuardarHoraE').on("click", function () {
        if (ValidarHoraEjecucion() === "") {
            GuardarHoraEjecucion();
        }
        else {
            $('#PProceso').show();
            $('#PProceso').removeClass();
            $('#PProceso').html(ValidarHoraEjecucion());
            $('#PProceso').addClass('action-alert');
        }
    });


});

$(function () {
    $('#btnGuardarMargenR').on("click", function () {
        if (validarMargenR() === "") {
            GuardarMargenReserva();
        }
        else {
            $('#mensajeMargenR').show();
            $('#mensajeMargenR').removeClass();
            $('#mensajeMargenR').html(validarMargenR());
            $('#mensajeMargenR').addClass('action-alert');
        }
    });
    $('#btnGuardarMontoServicio').on("click", function () {
        console.log(validarSCeIO());
        if (validarSCeIO() === "") {
            GuardarFactorReparto();
            GuardarPorcentajePerdida();
            GuardarCostoOportunidad();
            $('#mensajeGeneral').show();
            $('#mensajeGeneral').removeClass();
            $('#mensajeGeneral').addClass('action-exito');
            $('#mensajeGeneral').html("Datos Guardados Correctamente!");
        }
        else {
            $('#mensajeGeneral').show();
            $('#mensajeGeneral').removeClass();
            $('#mensajeGeneral').html(validarSCeIO());
            $('#mensajeGeneral').addClass('action-alert');
        }

    });
    //paso1
    //GuardarMontoReactiva
    $('#btnGuardarCargosC').on("click", function () {
        if (validarAporteAd() === "") { // cambios
            GuardarAporteAd()
        }
        else {
            $('#mensajeCargoC').show();
            $('#mensajeCargoC').removeClass();
            $('#mensajeCargoC').html(validarAporteAd());
            $('#mensajeCargoC').addClass('action-alert');
        }

    });

    $('#btnAñadirEmpresaEneReactiva').on("click", function () {
        var mensaje = validarCargosENR();
        if (mensaje === "") {
            GuardarMontoReactiva();
            $('#mensajeCargoC').show();
            $('#mensajeCargoC').removeClass();
            $('#mensajeCargoC').html('Añadido correctamente');
            $('#mensajeCargoC').addClass('action-exito');
        } else {
            $('#mensajeCargoC').show();
            $('#mensajeCargoC').removeClass();
            $('#mensajeCargoC').html(mensaje);
            $('#mensajeCargoC').addClass('action-alert');
        }
    });

});

//Hora de Ejecucion
function GuardarHoraEjecucion() {
    horaejecucion = JSON.stringify({ 'horaejecucion': $('#txtHoraEjecucion').val() });
    console.log(horaejecucion);
    $.ajax({
        url: controlador + "GuardarHoraEjecucion",
        contentType: "application/json; charset=utf-8",
        type: 'POST',
        data: horaejecucion,
        dataType: 'json',
        success: function (result) {
            if (result >= 1) {
                $('#PProceso').removeClass();
                $('#PProceso').addClass('action-exito');
                $('#PProceso').text('Se Guardó Correctamente!');
            } else {
                $('#PProceso').removeClass();
                $('#PProceso').addClass('action-error');
                $('#PProceso').text('Error al Guardar Hora de Ejecución');
            }
        },
        error: function (result) {
            $('#PProceso').removeClass();
            $('#PProceso').addClass('action-alert');
            $('#PProceso').html('no se obtuvo conexión con el servidor');
        }
    });
}
//Validar Hora Ejecucion
function ValidarHoraEjecucion() {

    var horaEjecucion = $('#txtHoraEjecucion').val();
    //var fechaInicios = "";
    var resultado = horaEjecucion.split(":");
    var hora = resultado[0];
    var minuto = resultado[1];

    var HoraEje = '';
    HoraEje = "";
    var flagHoraEje = true;
    if ($('#txtHoraEjecucion').val().replace(/\s/g, '') === "") {
        HoraEje = "Ingrese un Valor";
        $('#txtHoraEjecucion').focus();
        flagHoraEje = false;
    }
    else {
        if (hora > 24) {
            HoraEje = "Ingrese Hora Correcta";
            $('#txtHoraEjecucion').focus();
            flagHoraEje = false;
        }

        if (minuto > 60) {
            HoraEje = "Ingrese Minuto Correcto";
            $('#txtHoraEjecucion').focus();
            flagHoraEje = false;
        }
    }
    if ($('#txtHoraEjecucion').val().length !== 5) {
        HoraEje = "Ingrese Una Hora Hora de Ejecución Válida";
        $('#txtHoraEjecucion').focus();
        flagHoraEje = false;
    }

    if (flagHoraEje) HoraEje = "";

    return HoraEje;
};

//Margen de Reparto
function validarMargenR() {
    var mensajeMargeR = '';
    var flagMargenR = true;    

    if (!/^\d*\.?\d*$/.test($('#txtValorMargenR').val().replace(/\s/g, ''))) {
        mensajeMargeR = mensajeMargeR + "<li>Ingrese un Valor Númerico para Margen de Reserva</li>";
        $('#txtValorMargenR').focus();
        flagMargenR = false;
    }

    if ($('#txtValorMargenR').val().replace(/\s/g, '') === "") {
        mensajeMargeR = mensajeMargeR + "<li>Ingrese un Valor para Margen de Reserva</li>";
        $('#txtValorMargenR').focus();
        flagMargenR = false;
    }

    if (flagMargenR) mensajeMargeR = "";
    return mensajeMargeR;
};
function GuardarMargenReserva() {
    ValorMReserva = JSON.stringify({ 'ValorMargenReserva': $('#txtValorMargenR').val().replace(/\s/g, '') });
    $.ajax({
        url: controlador + "GuardarMargenReserva",
        contentType: "application/json; charset=utf-8",
        type: 'POST',
        data: ValorMReserva,
        dataType: 'json',
        success: function (result) {
            if (result >= 1) {
                $('#mensajeMargenR').removeClass();
                $('#mensajeMargenR').addClass('action-exito');
                $('#mensajeMargenR').text('Se Guardó Correctamente!');
            } else {
                $('#mensajeMargenR').removeClass();
                $('#mensajeMargenR').addClass('action-error');
                $('#mensajeMargenR').text('Error al Guardar Margen de Reserva');
            }
        },
        error: function (result) {
            $('#mensajeMargenR').removeClass();
            $('#mensajeMargenR').addClass('action-alert');
            $('#mensajeMargenR').html('no se obtuvo conexión con el servidor');
        }
    });

}
//end Margen de Registro
//++++++++++++++++++++++

//Factor de Reparto
ValidarSumaFactoredeReparto=function() {
    var suma = 0;
    flagFR = true;

    var lunes = parseFloat($('#txtLunes').val());
    var martes = parseFloat($('#txtMartes').val());
    var miercoles = parseFloat($('#txtMiercoles').val());
    var jueves = parseFloat($('#txtJueves').val());
    var viernes = parseFloat($('#txtViernes').val());
    var sabado = parseFloat($('#txtSabado').val());
    var domingo = parseFloat($('#txtDomingo').val());

    suma = lunes + martes + miercoles + jueves + viernes + sabado + domingo;

    if (suma == 1 || suma == 0.9999999999999999) {
        flagFR= true;
    } else {
        $('#mensajeGeneral').text('La Suma de los valores de los Factores de Reparto de Energía semanal debe ser 1');
        flagFR = false;
    }
    return flagFR;

};
function GuardarFactorReparto() {

    var data = {
        ValorLunes: $('#txtLunes').val().replace(/\s/g, ''),
        ValorMartes: $('#txtMartes').val().replace(/\s/g, ''),
        ValorMiercoles: $('#txtMiercoles').val().replace(/\s/g, ''),
        ValorJueves: $('#txtJueves').val().replace(/\s/g, ''),
        ValorViernes: $('#txtViernes').val().replace(/\s/g, ''),
        ValorSabado: $('#txtSabado').val().replace(/\s/g, ''),
        ValorDomingo: $('#txtDomingo').val().replace(/\s/g, '')
    };

    $.ajax({
        url: controlador + "GuardarFactorReparto",
        contentType: "application/json; charset=utf-8",
        type: 'POST',
        data: JSON.stringify(data),
        dataType: 'json',
        success: function (result) {
            if (result >= 1) {
                //$('#mensajeGeneral').show();
                //$('#mensajeGeneral').removeClass();
                //$('#mensajeGeneral').addClass('action-exito');
                //$('#mensajeGeneral').text('Factor de Reparto Correcto');
            } else {
                $('#mensajeGeneral').show();
                $('#mensajeGeneral').removeClass();
                $('#mensajeGeneral').addClass('action-error');
                $('#mensajeGeneral').text('Error Factor de Reparto!');
            }
        },
        error: function (result) {
            $('#mensajeGeneral').show();
            $('#mensajeGeneral').removeClass();
            $('#mensajeGeneral').addClass('action-alert');
            $('#mensajeGeneral').html('no se obtuvo conexion con el servidor');
        }
    });

}
//!!!!!!!!!!!!!!!!!!!!!!!!
//Porcentaje de Perdidas
function GuardarPorcentajePerdida() {
    ValorPorcentajePerdida = JSON.stringify({ 'ValorPorcentajePerdida': $('#txtPorcentajePerdida').val().replace(/\s/g, '') });


    $.ajax({
        url: controlador + "GuardarPorcentajePerdida",
        contentType: "application/json; charset=utf-8",
        type: 'POST',
        data: ValorPorcentajePerdida,
        dataType: 'json',
        success: function (result) {
            if (result >= 1) {
                //$('#mensajeGeneral').show();
                //$('#mensajeGeneral').removeClass();
                //$('#mensajeGeneral').addClass('action-exito');
                //$('#mensajeGeneral').html('Porcentaje Perdida Guardado Correctamente');
            } else {
                $('#mensajeGeneral').show();
                $('#mensajeGeneral').removeClass();
                $('#mensajeGeneral').addClass('action-error');
                $('#mensajeGeneral').html('Error Porcentaje Perdida!');
            }
        },
        error: function (result) {
            $('#mensajeGeneral').show();
            $('#mensajeGeneral').removeClass();
            $('#mensajeGeneral').addClass('action-alert');
            $('#mensajeGeneral').html('no se obtuvo conexion con el servidor');
        }
    });


}
//end porcentaje de Perdidas
//--------------------------
//Costo Oportunidad
function GuardarCostoOportunidad() {
    ValorCostoOportunidad = JSON.stringify({ 'ValorCostoOportunidad': $('#txtCostoOportunidad').val().replace(/\s/g, '') });
    $.ajax({
        url: controlador + "GuardarCostoOportunidad",
        contentType: "application/json; charset=utf-8",
        type: 'POST',
        data: ValorCostoOportunidad,
        dataType: 'json',
        success: function (result) {
            if (result >= 1) {
                //$('#mensajeGeneral').show();
                //$('#mensajeGeneral').removeClass();
                //$('#mensajeGeneral').addClass('action-exito');
                //$('#mensajeGeneral').html('Costo Oportunidad Guardado Correctamente');
            } else {
                $('#mensajeGeneral').show();
                $('#mensajeGeneral').removeClass();
                $('#mensajeGeneral').addClass('action-error');
                $('#mensajeGeneral').html('Error Costo Oportunidad!');
            }
        },
        error: function (result) {
            $('#mensajeGeneral').show();
            $('#mensajeGeneral').removeClass();
            $('#mensajeGeneral').addClass('action-alert');
            $('#mensajeGeneral').html('no se obtuvo conexion con el servidor');
        }
    });
}
//#######################

/**/
validarSCeIO = function () {

    var mensajeSCeIO = '';
    mensajeSCeIO = "<ul>", flag = true;
   
    if (ValidarSumaFactoredeReparto()==false) {
        mensajeSCeIO = mensajeSCeIO + "<li>La Suma de los valores de los Factores de Reparto de Energía semanal debe ser 1</li>";
        flag = false;
    }
    if (!/^\d*\.?\d*$/.test($('#txtLunes').val().replace(/\s/g, ''))) {
        mensajeSCeIO = mensajeSCeIO + "<li>Ingrese un Valor Númerico para Lunes</li>";
        flag = false;
    }
    if (!/^\d*\.?\d*$/.test($('#txtMartes').val().replace(/\s/g, ''))) {
        mensajeSCeIO = mensajeSCeIO + "<li>Ingrese un Valor Númerico para Martes</li>";
        flag = false;
    }
    if (!/^\d*\.?\d*$/.test($('#txtMiercoles').val().replace(/\s/g, ''))) {
        mensajeSCeIO = mensajeSCeIO + "<li>Ingrese un Valor Númerico para Miercoles</li>";
        flag = false;
    }
    if (!/^\d*\.?\d*$/.test($('#txtJueves').val().replace(/\s/g, ''))) {
        mensajeSCeIO = mensajeSCeIO + "<li>Ingrese un Valor Númerico para Jueves</li>";
        flag = false;
    }
    if (!/^\d*\.?\d*$/.test($('#txtViernes').val().replace(/\s/g, ''))) {
        mensajeSCeIO = mensajeSCeIO + "<li>Ingrese un Valor Númerico para Viernes</li>";
        flag = false;
    }
    if (!/^\d*\.?\d*$/.test($('#txtSabado').val().replace(/\s/g, ''))) {
        mensajeSCeIO = mensajeSCeIO + "<li>Ingrese un Valor Númerico para Sabado</li>";
        flag = false;
    }
    if (!/^\d*\.?\d*$/.test($('#txtDomingo').val().replace(/\s/g, ''))) {
        mensajeSCeIO = mensajeSCeIO + "<li>Ingrese un Valor Númerico para Domingo</li>";
        flag = false;
    }

    if ($('#txtCostoOportunidad').val().replace(/\s/g, '') === "") {
        mensajeSCeIO = mensajeSCeIO + "<li>Ingrese un Valor para Costo de Oportunidad</li>";
        flag = false;
    }
    if ($('#txtPorcentajePerdida').val().replace(/\s/g, '') === "") {
        mensajeSCeIO = mensajeSCeIO + "<li>Ingrese un Valor para Porcentaje de Pérdidas</li>";
        flag = false;
    }
    if ($('#txtLunes').val().replace(/\s/g, '') === "" || $('#txtMartes').val().replace(/\s/g, '') === "" || $('#txtMiercoles').val().replace(/\s/g, '') === "" || $('#txtJueves').val().replace(/\s/g, '') === "" || $('#txtViernes').val().replace(/\s/g, '') === "" || $('#txtSabado').val().replace(/\s/g, '') === "" || $('#txtDomingo').val().replace(/\s/g, '') === "") {
        mensajeSCeIO = mensajeSCeIO + "<li>Faltan Ingresar Valores Para Factores de Reparto de Energía semanal</li>";
        flag = false;
    }
    if (!/^\d*\.?\d*$/.test($('#txtCostoOportunidad').val().replace(/\s/g, ''))) {
        mensajeSCeIO = mensajeSCeIO + "<li>Ingrese un Valor Númerico para Costo de Oportunidad</li>";
        flag = false;
    }
    if (!/^\d*\.?\d*$/.test($('#txtPorcentajePerdida').val().replace(/\s/g, ''))) {
        mensajeSCeIO = mensajeSCeIO + "<li>Ingrese un Valor Númerico para Porcentaje de Pérdida</li>";
        flag = false;
    }

    mensajeSCeIO = mensajeSCeIO + "</ul>";
    if (flag) mensajeSCeIO = "";
    return mensajeSCeIO;
};

/**/

/**Cargar monto de la empresa por fecha**/
$('#cbEmpresa').change(function () {
    //MostrarMontoPorFecha(); // cambio
});

//AporteAd
function GuardarAporteAd() {

    var datos = {
        CostoFueraBanda: $('#txtFueraBanda').val().replace(/\s/g, ''),
        CostoOtrosEquipos: $('#txtCostoOtrosEquipos').val().replace(/\s/g, ''),
        FRECTotal: $('#txtFRECTotal').val().replace(/\s/g, '')
    };

    $.ajax({
        url: controlador + "GuardarAporteAd",
        contentType: "application/json; charset=utf-8",
        type: 'POST',
        data: JSON.stringify(datos),
        dataType: 'json',
        success: function (result) {
            if (result >= 1) {
                $('#mensajeCargoC').show();
                $('#mensajeCargoC').removeClass();
                $('#mensajeCargoC').addClass('action-exito');
                $('#mensajeCargoC').text('Datos Guardados Correctamente');
            } else {
                $('#mensajeCargoC').show();
                $('#mensajeCargoC').removeClass();
                $('#mensajeCargoC').addClass('action-error');
                $('#mensajeCargoC').text('Error!');
            }
        },
        error: function (result) {
            $('#mensajeCargoC').show();
            $('#mensajeCargoC').removeClass();
            $('#mensajeCargoC').addClass('action-alert');
            $('#mensajeCargoC').html('no se obtuvo conexion con el servidor');
        }
    });


}

function diasEnUnMesMC(mes, año) {
    return new Date(año, mes, 0).getDate();
}

function ObtenerFecha() {
    var fecha = $('#txtFechaEneReactiva').val();
    var fechaFinals = "";
    var fechaMes = fecha.split(" ", 1);
    var fechaAnio = fecha.substr(4, 7);

    fechaMes = fechaMes[0];

    var number = 0;
    switch (fechaMes) {
        case "Ene":
            number = 1;
            break;
        case "Feb":
            number = 2;
            break;
        case "Mar":
            number = 3;
            break;
        case "Abr":
            number = 4;
            break;
        case "May":
            number = 5;
            break;
        case "Jun":
            number = 6;
            break;
        case "Jul":
            number = 7;
            break;
        case "Ago":
            number = 8;
            break;
        case "Set":
            number = 9;
            break;
        case "Oct":
            number = 10;
            break;
        case "Nov":
            number = 11;
            break;
        case "Dic":
            number = 12;
            break;
    }

    var dias = diasEnUnMesMC(number, fechaAnio);

    return fechaFinals = dias + " " + number + " " + fechaAnio;
}

//MontoporReactiva
function GuardarMontoReactiva() {

    var datos = {
        emprcodi: $('#cbEmpresa').val(),
        caermonto: $('#monto').val(),
        date: $('#txtFechaEneReactiva').val()
    };

    $.ajax({
        url: controlador + "GuardarMontoporReactiva",
        contentType: "application/json; charset=utf-8",
        type: 'POST',
        data: JSON.stringify(datos),
        dataType: 'json',
        beforeSend: function (result) {

        },
        success: function (result) {
            if (result >= 1) {
                $('#mensajeGeneral').show();
                $('#mensajeGeneral').removeClass();
                $('#mensajeGeneral').addClass('action-exito');
                $('#mensajeGeneral').text('Datos Guardados Correctamente');
                ListarCargosENR(); // cambio
            } else {
                alert('Falló el envio');
                $('#mensajeGeneral').show();
                $('#mensajeGeneral').removeClass();
                $('#mensajeGeneral').addClass('action-error');
                $('#mensajeGeneral').text('Error!');
            }
        },
        error: function (result) {
            $('#mensajeGeneral').show();
            $('#mensajeGeneral').removeClass();
            $('#mensajeGeneral').addClass('action-alert');
            $('#mensajeGeneral').html('no se obtuvo conexion con el servidor');
        }
    });
}

//MostrarMonto
MostrarMontoPorFecha = function () {

    //emprcodi: $('#cbEmpresa').val();
    emprcodi = JSON.stringify({
        'emprcodi': $('#cbEmpresa').val()
    });

    $.ajax({
        url: controlador + "MostrarMontodeEmpresaSeleccionada",
        contentType: "application/json; charset=utf-8",
        type: 'POST',
        data: emprcodi,
        dataType: 'json',
        success: function (result) {
            if (result >= 1) {
                $('#monto').val(result);
                $('#mensajeGeneral').show();
                $('#mensajeGeneral').removeClass();
                $('#mensajeGeneral').addClass('action-exito');
                $('#mensajeGeneral').text('Datos Guardados Correctamente');
            } else {
                $('#monto').val('0.0');
                $('#mensajeGeneral').show();
                $('#mensajeGeneral').removeClass();
                $('#mensajeGeneral').addClass('action-error');
                $('#mensajeGeneral').text('Error!');
            }
        }
    });
}

validarAporteAd = function () {
    var mensajeAporteAd = '';
    mensajeAporteAd = "<ul>", flag = true;

    if ($('#txtFueraBanda').val().replace(/\s/g, '') === "") {
        mensajeAporteAd = mensajeAporteAd + "<li>Ingrese un Valor para Costo Fuera de Banda</li>";
        flag = false;
    }
    if ($('#txtCostoOtrosEquipos').val().replace(/\s/g, '') === "") {
        mensajeAporteAd = mensajeAporteAd + "<li>Ingrese un Valor para Costo otros Equipos</li>";
        flag = false;
    }
    if ($('#txtFRECTotal').val().replace(/\s/g, '') === "") {
        mensajeAporteAd = mensajeAporteAd + "<li>Ingrese un Valor para FREC Total</li>";
        flag = false;
    }

    if (!/^\d*\.?\d*$/.test($('#txtFueraBanda').val().replace(/\s/g, ''))) {
        mensajeAporteAd = mensajeAporteAd + "<li>Ingrese un Valor Númerico para Costo Fuera de Banda</li>";
        flag = false;
    }
    if (!/^\d*\.?\d*$/.test($('#txtCostoOtrosEquipos').val().replace(/\s/g, ''))) {
        mensajeAporteAd = mensajeAporteAd + "<li>Ingrese un Valor Númerico para Costo otros Equipos</li>";
        flag = false;
    }
    if (!/^\d*\.?\d*$/.test($('#txtFRECTotal').val().replace(/\s/g, ''))) {
        mensajeAporteAd = mensajeAporteAd + "<li>Ingrese un Valor Númerico para FREC Total</li>";
        flag = false;
    }

    mensajeAporteAd = mensajeAporteAd + "</ul>";
    if (flag) mensajeAporteAd = "";

    return mensajeAporteAd;
};

// cambios
validarCargosENR = function () {
    var mensajeCargosENR = '', flag = true;

    if ($('#cbEmpresa').val() <= 0 || $('#cbEmpresa').val() == null) {
        mensajeCargosENR = mensajeCargosENR+ "<li>Elija un participante</li>";
        flag = false;
    }

    if ($('#monto').val() < 0 || $('#monto').val() == null) {
        mensajeCargosENR = mensajeCargosENR+ "<li>Ingrese un Valor</li>";
        flag = false;
    }
    if (flag) mensajeCargosENR = "";
    return mensajeCargosENR;
};
// cambios
var ListarCargosENR = function () {
    var datos = {
        fecha: $('#txtFechaEneReactiva').val() //ObtenerFecha()
    };

    $.ajax({
        url: controlador + "ListaEnergiaReactiva",
        contentType: "application/json; charset=utf-8",
        type: 'POST',
        data: JSON.stringify(datos),
        dataType: 'json',
        beforeSend: function (result) {

        },
        success: function (result) {
            $('#ListarCargosENR').css("width", $('#mainLayout').width() + "px");
            $('#ListarCargosENR').html(result);
        },
        error: function (result) {
            $('#ListarCargosENR').css("width", $('#mainLayout').width() + "px");
            $('#ListarCargosENR').html(result.responseText);
        }
    });
}

var eliminarEneReactiva = function (emprcodi, fecha) {
    console.log(fecha);

    var datos = {
        emprcodi: emprcodi,
        date: fecha
    };

    $.ajax({
        url: controlador + "EliminarMontoporReactiva",
        contentType: "application/json; charset=utf-8",
        type: 'POST',
        data: JSON.stringify(datos),
        dataType: 'json',
        beforeSend: function (result) {

        },
        success: function (result) {
            if (result >= 1) {
                $('#mensajeGeneral').show();
                $('#mensajeGeneral').removeClass();
                $('#mensajeGeneral').addClass('action-exito');
                $('#mensajeGeneral').text('Elimado Correctamente');
                ListarCargosENR(); // cambio
            } else {
                alert('Falló el envio');
                $('#mensajeGeneral').show();
                $('#mensajeGeneral').removeClass();
                $('#mensajeGeneral').addClass('action-error');
                $('#mensajeGeneral').text('Error!');
            }
        },
        error: function (result) {
            $('#mensajeGeneral').show();
            $('#mensajeGeneral').removeClass();
            $('#mensajeGeneral').addClass('action-alert');
            $('#mensajeGeneral').html('no se obtuvo conexion con el servidor');
        }
    });
}




//endAporteAd

//************************
