//Declarando las variables 
let controlador = siteRoot + 'Reactor/';
let hot;
let hotOptions;
let evtHot;

$(function () {

    $("#popUpEditarComentario").addClass("general-popup");

    $("#btnRegresar").click(function () {
        regresar();
    });

    $('#btnGuardar').click(function () {
        calcularAntesGuardar();
    });

    $('#Celda1').multipleSelect({
        width: '98%',
        filter: true,
        single: true
    });

    $('#Celda2').multipleSelect({
        width: '98%',
        filter: true,
        single: true
    });

    $('#Celda1').change(function () {
        let IdCelda = obtenerValorMultiselect('Celda1');
        consultarCelda(IdCelda);
    });

    $('#Celda2').change(function () {
        let IdCelda = obtenerValorMultiselect('Celda2');
        consultarCelda2(IdCelda);
    });

    setearMultiselect("hiddenCelda1", "Celda1");
    setearMultiselect("hiddenCelda2", "Celda2");

    inputSoloNumerosDecimales([
        'CapacidadMvar',
        'CapacidadA'
    ]);

    $("#btnCalcular").click(function () {
        calcular();
    });

    calcular();
});
function regresar() {
    const accion = $("#Accion").val();
    if (accion == "Modificar") {
        window.location.href = siteRoot + "Evaluacion/Reactor/Index/";
    } else if (accion == "Incluir") {
        window.location.href = controlador + "BuscarReactor";
    }
};


function editarComentario(id) {

    const comentario = $('#' + id ).val();

    $.ajax({
        type: 'POST',
        url: controlador + "EditarComentario",
        data: {
            comentario: comentario
        },
        success: function (evt) {
            $('#editarComentario').html(evt);
            $('#mensaje').css("display", "none");

            setTimeout(function () {
                $('#popUpEditarComentario').bPopup({
                    easing: 'easeOutBack',
                    speed: 450,
                    transition: 'slideDown'
                });
            }, 50);

            $("#btnRegistrarComentario").click(function () {
                const com = $('#Comentario').val();
                $('#' + id).val(com);

                $('#popUpEditarComentario').bPopup().close();
            });

        },
        error: function () {
            mostrarMensaje('Ha ocurrido un error.', 'error');
        }
    });
};
let guardar = function () {

    const acc = $("#Accion").val();
    let mensajeConfirmar = "";
    if (acc == "Modificar") {
        mensajeConfirmar = "¿Está seguro de modificar el registro del reactor?";
    } else if (acc == "Incluir") {
        mensajeConfirmar = "¿Está seguro de guardar el registro del reactor?";
    }


    limpiarMensaje();
    if (validarGuardar()) {
        let IdCelda1 = obtenerValorMultiselect('Celda1');
        let IdCelda2 = obtenerValorMultiselect('Celda2');

        if (IdCelda1 == IdCelda2) {
            alert("El valor de la Celda 1 y Celda 2 no deben ser iguales");
            return;
        }

        let Equicodi = $('#CodigoId').val();
        let IdReactor = $('#IdReactor').val();
        let IdProyecto = $('#IdProyecto').val();
        let Fecha = $('#FechaActualizacion').val();
        
        let CapacidadMvar = $('#CapacidadMvar').val();
        let CapacidadA = $('#CapacidadA').val();
        let CapacidadTransmisionA = $('#CapacidadTransmisionA').val();
        let CapacidadTransmisionAComent = $('#CapacidadTransmisionAComent').val();
        let CapacidadTransmisionMvar = $('#CapacidadTransmisionMvar').val();
        let CapacidadTransmisionMvarComent = $('#CapacidadTransmisionMvarComent').val();
        let FactorLimitanteCalc = $('#FactorLimitanteCalc').val();
        let FactorLimitanteCalcComent = $('#FactorLimitanteCalcComent').val();
        let FactorLimitanteFinal = $('#FactorLimitanteFinal').val();
        let FactorLimitanteFinalComent = $('#FactorLimitanteFinalComent').val();
        let Observaciones = $('#Observaciones').val();

        let textoConfirmación = mensajeConfirmar;

        if (confirm(textoConfirmación)) {

            let datos = {
                Equicodi: Equicodi,
                IdReactor: IdReactor,
                IdProyecto: IdProyecto,
                Fecha: Fecha,
                IdCelda1: IdCelda1,
                IdCelda2: IdCelda2,
                CapacidadMvar: CapacidadMvar,
                CapacidadA: CapacidadA,
                CapacidadTransmisionA: CapacidadTransmisionA,
                CapacidadTransmisionAComent: CapacidadTransmisionAComent,
                CapacidadTransmisionMvar: CapacidadTransmisionMvar,
                CapacidadTransmisionMvarComent: CapacidadTransmisionMvarComent,
                FactorLimitanteCalc: FactorLimitanteCalc,
                FactorLimitanteCalcComent: FactorLimitanteCalcComent,
                FactorLimitanteFinal: FactorLimitanteFinal,
                FactorLimitanteFinalComent: FactorLimitanteFinalComent,
                Observaciones: Observaciones

            };

            $.ajax({
                type: 'POST',
                url: controlador + 'GuardarReactor',
                dataType: 'json',
                data: datos,
                cache: false,
                success: function (resultado) {
                    if (resultado == "") {
                        mostrarMensaje('Se ha guardado el registro del Reactor correctamente.', 'exito');
                    } else
                        alert(resultado)

                },
                error: function () {
                    mostrarMensaje('Ha ocurrido un error.', 'error');
                }
            });
        }
    }
};

function validarGuardar() {
    let campos = [];

    if ($('#Celda1').val() == '0') campos.push('Celda1/Celda');

    if (campos.length > 0) {

        mensajeValidador(campos);
        return false;
    }

    return true;
}
let limpiarMensaje = function () {
    $('#mensaje').css("display", "none");

};

function mensajeValidador(campos) {
    let texto = "Los campos: \n";

    for (const campo of campos) {
        texto += campo + "(*) \n";
    }

    alert(texto + " son requeridos");
}
let mostrarMensaje = function (mensaje, tipo) {
    if (tipo == 'error') {
        $('#mensaje').removeClass("action-exito");
        $('#mensaje').addClass("action-error");
        $('#mensaje').text(mensaje);
        $('#mensaje').css("display", "block");
    } else {
        $('#mensaje').removeClass("action-error");
        $('#mensaje').addClass("action-exito");
        $('#mensaje').text(mensaje);
        $('#mensaje').css("display", "block");
    }

};

let consultarCelda = function (IdCelda) {
    setTimeout(function () {
        $.ajax({
            type: 'POST',
            url: controlador + 'ConsultaCelda',
            data: { IdCelda: IdCelda },
            success: function (evt) {
                $('#mensaje').css("display", "none");

                if (evt) {
                    $('#Celda1PosicionNucleoTc').val(evt.PosicionNucleoTc);
                    $('#Celda1PickUp').val(evt.PickUp);
                }
            },
            error: function () {
                mostrarMensaje('Ha ocurrido un error.', 'error');
            }
        });
    }, 100);
};

let consultarCelda2 = function (IdCelda) {
    setTimeout(function () {
        $.ajax({
            type: 'POST',
            url: controlador + 'ConsultaCelda',
            data: { IdCelda: IdCelda },
            success: function (evt) {
                $('#mensaje').css("display", "none");

                if (evt) {
                    $('#Celda2PosicionNucleoTc').val(evt.PosicionNucleoTc);
                    $('#Celda2PickUp').val(evt.PickUp);
                }
            },
            error: function () {
                mostrarMensaje('Ha ocurrido un error.', 'error');
            }
        });
    }, 100);
};

let calcular = function () {

    let Equicodi = $('#CodigoId').val();
    let IdReactor = $('#IdReactor').val();
    let IdProyecto = $('#IdProyecto').val();

    let IdCelda1 = obtenerValorMultiselect('Celda1');
    let IdCelda2 = obtenerValorMultiselect('Celda2');

    let CapacidadMvar = $('#CapacidadMvar').val();
    let CapacidadA = $('#CapacidadA').val();
    let CapacidadTransmisionA = $('#CapacidadTransmisionA').val();
    let CapacidadTransmisionMvar = $('#CapacidadTransmisionMvar').val();
    let FactorLimitanteCalc = $('#FactorLimitanteCalc').val();
    let FactorLimitanteFinal = $('#FactorLimitanteFinal').val();
    let NivelTension = $('#NivelTension').val();

    limpiarMensaje();

    let datos = {
        Equicodi: Equicodi,
        IdReactor: IdReactor,
        IdProyecto: IdProyecto,
        IdCelda1: IdCelda1,
        IdCelda2: IdCelda2,
        CapacidadMvar: CapacidadMvar,
        CapacidadA: CapacidadA,
        CapacidadTransmisionA: CapacidadTransmisionA,
        CapacidadTransmisionMvar: CapacidadTransmisionMvar,
        FactorLimitanteCalc: FactorLimitanteCalc,
        FactorLimitanteFinal: FactorLimitanteFinal,
        NivelTension: NivelTension

    };


    $.ajax({
        type: 'POST',
        url: controlador + 'CalcularReactor',
        dataType: 'json',
        data: datos,
        cache: false,
        success: function (resultado) {

            if (resultado.MensajeError == "") {
                mostrarMensaje('Se realizó el cálculo de los valores.', 'exito');

                $('#CapacidadTransmisionA').val(resultado.CapacidadTransmisionA);
                $('#CapacidadTransmisionMvar').val(resultado.CapacidadTransmisionMvar);
                $('#FactorLimitanteCalc').val(resultado.FactorLimitanteCalc);
                $('#FactorLimitanteFinal').val(resultado.FactorLimitanteFinal);

            } else {
                alert(resultado.MensajeError);
            }


        },
        error: function () {
            mostrarMensaje('Ha ocurrido un error.', 'error');
        }
    });
};

//Permite obtener el valor de los multiselect por ID
function obtenerValorMultiselect(id) {
    let array = $('#' + id).val();
    let valor;

    if (array != null && array != "") {
        valor = array[0].toString();
    } else {
        valor = 0;
    }

    return valor;
}

//Permite setear los valores en los multiselect al editar
function setearMultiselect(hidden, id) {
    const valorOculto = $("#" + hidden).val();
    if (valorOculto != null && valorOculto != "") {
        $('#' + id).multipleSelect('setSelects', [valorOculto]);
    } else {
        $('#' + id).multipleSelect('setSelects', ["0"]);
    }
}

let calcularAntesGuardar = function () {

    const acc = $("#Accion").val();
    let mensajeConfirmar = "";
    if (acc == "Modificar") {
        mensajeConfirmar = "¿Está seguro de modificar el registro del reactor?";
    } else if (acc == "Incluir") {
        mensajeConfirmar = "¿Está seguro de guardar el registro del reactor?";
    }


    limpiarMensaje();
    if (validarGuardar()) {
        let IdCelda1 = obtenerValorMultiselect('Celda1');
        let IdCelda2 = obtenerValorMultiselect('Celda2');

        if (IdCelda1 == IdCelda2) {
            alert("El valor de la Celda 1 y Celda 2 no deben ser iguales");
            return;
        }

        let Equicodi = $('#CodigoId').val();
        let IdReactor = $('#IdReactor').val();
        let IdProyecto = $('#IdProyecto').val();

        let CapacidadMvar = $('#CapacidadMvar').val();
        let CapacidadA = $('#CapacidadA').val();
        let CapacidadTransmisionA = $('#CapacidadTransmisionA').val();
        let CapacidadTransmisionMvar = $('#CapacidadTransmisionMvar').val();
        let FactorLimitanteCalc = $('#FactorLimitanteCalc').val();
        let FactorLimitanteFinal = $('#FactorLimitanteFinal').val();
        let NivelTension = $('#NivelTension').val();

        let textoConfirmación = mensajeConfirmar;

        if (confirm(textoConfirmación)) {

            let datos = {
                Equicodi: Equicodi,
                IdReactor: IdReactor,
                IdProyecto: IdProyecto,
                IdCelda1: IdCelda1,
                IdCelda2: IdCelda2,
                CapacidadMvar: CapacidadMvar,
                CapacidadA: CapacidadA,
                CapacidadTransmisionA: CapacidadTransmisionA,
                CapacidadTransmisionMvar: CapacidadTransmisionMvar,
                FactorLimitanteCalc: FactorLimitanteCalc,
                FactorLimitanteFinal: FactorLimitanteFinal,
                NivelTension: NivelTension
            };

            $.ajax({
                type: 'POST',
                url: controlador + 'CalcularReactor',
                dataType: 'json',
                data: datos,
                cache: false,
                success: function (resultado) {

                    if (resultado.MensajeError == "") {
                        mostrarMensaje('Se realizó el cálculo de los valores.', 'exito');

                        $('#CapacidadTransmisionA').val(resultado.CapacidadTransmisionA);
                        $('#CapacidadTransmisionMvar').val(resultado.CapacidadTransmisionMvar);
                        $('#FactorLimitanteCalc').val(resultado.FactorLimitanteCalc);
                        $('#FactorLimitanteFinal').val(resultado.FactorLimitanteFinal);

                        guardar2();
                    } else {
                        alert(resultado.MensajeError);
                    }


                },
                error: function () {
                    mostrarMensaje('Ha ocurrido un error.', 'error');
                }
            });

        }
    }
};

let guardar2 = function () {

    let IdCelda1 = obtenerValorMultiselect('Celda1');
    let IdCelda2 = obtenerValorMultiselect('Celda2');

    let Equicodi = $('#CodigoId').val();
    let IdReactor = $('#IdReactor').val();
    let IdProyecto = $('#IdProyecto').val();
    let Fecha = $('#FechaActualizacion').val();

    let CapacidadMvar = $('#CapacidadMvar').val();
    let CapacidadA = $('#CapacidadA').val();
    let CapacidadTransmisionA = $('#CapacidadTransmisionA').val();
    let CapacidadTransmisionAComent = $('#CapacidadTransmisionAComent').val();
    let CapacidadTransmisionMvar = $('#CapacidadTransmisionMvar').val();
    let CapacidadTransmisionMvarComent = $('#CapacidadTransmisionMvarComent').val();
    let FactorLimitanteCalc = $('#FactorLimitanteCalc').val();
    let FactorLimitanteCalcComent = $('#FactorLimitanteCalcComent').val();
    let FactorLimitanteFinal = $('#FactorLimitanteFinal').val();
    let FactorLimitanteFinalComent = $('#FactorLimitanteFinalComent').val();
    let Observaciones = $('#Observaciones').val();

    limpiarMensaje();

    let datos = {
        Equicodi: Equicodi,
        IdReactor: IdReactor,
        IdProyecto: IdProyecto,
        Fecha: Fecha,
        IdCelda1: IdCelda1,
        IdCelda2: IdCelda2,
        CapacidadMvar: CapacidadMvar,
        CapacidadA: CapacidadA,
        CapacidadTransmisionA: CapacidadTransmisionA,
        CapacidadTransmisionAComent: CapacidadTransmisionAComent,
        CapacidadTransmisionMvar: CapacidadTransmisionMvar,
        CapacidadTransmisionMvarComent: CapacidadTransmisionMvarComent,
        FactorLimitanteCalc: FactorLimitanteCalc,
        FactorLimitanteCalcComent: FactorLimitanteCalcComent,
        FactorLimitanteFinal: FactorLimitanteFinal,
        FactorLimitanteFinalComent: FactorLimitanteFinalComent,
        Observaciones: Observaciones

    };

    $.ajax({
        type: 'POST',
        url: controlador + 'GuardarReactor',
        dataType: 'json',
        data: datos,
        cache: false,
        success: function (resultado) {
            if (resultado == "") {
                mostrarMensaje('Se ha guardado el registro del Reactor correctamente.', 'exito');
            } else
                alert(resultado)

        },
        error: function () {
            mostrarMensaje('Ha ocurrido un error.', 'error');
        }
    });
    
};