//Declarando las variables 
let controlador = siteRoot + 'CeldaAcoplamiento/';
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

    inputSoloNumerosDecimales([
        'InterruptorCapacidadA'
    ]);
    $("#btnCalcular").click(function () {
        validarGuardar();
        guardar2(false);
        calcular();
    });

    $('#IdInterruptor').multipleSelect({
        width: '98%',
        filter: true,
        single: true
    });

    $('#IdInterruptor').change(function () {
        let idInterruptor = obtenerValorMultiselect('IdInterruptor');
        let idEquipo = $("#IdEquipo").val();

        if (idInterruptor == '0') {
            $("#InterruptorEmpresa").val("");
            $("#InterruptorTension").val("");
            $("#InterruptorCapacidadA").val("");
            $("#InterruptorCapacidadMva").val("");
            return;
        }

        seleccionarInterruptor(idEquipo, idInterruptor);
    });

    setearMultiselect("hiddenIdInterruptor", "IdInterruptor");

    calcular();

});
function regresar() {
    
    const accion = $("#Accion").val();
    if (accion == "Modificar") {
        window.location.href = siteRoot + "Evaluacion/CeldaAcoplamiento/Index/";
    } else if (accion == "Incluir") {
        window.location.href = controlador + "BuscarCeldaAcoplamiento";
    }
};


let seleccionarInterruptor = function (idEquipo, idInterruptor) {

    setTimeout(function () {
        $.ajax({
            type: 'POST',
            url: controlador + 'SeleccionarInterruptor',
            data: { idEquipo: idEquipo, idInterruptor: idInterruptor },
            success: function (evt) {

                if (evt) {
                    $("#InterruptorEmpresa").val(evt.EmprNomb);
                    $("#InterruptorTension").val(evt.Tension);
                    $("#InterruptorCapacidadA").val(evt.CapacidadA);
                    $("#InterruptorCapacidadMva").val(evt.CapacidadMva);
                } else {
                    $("#InterruptorEmpresa").val("");
                    $("#InterruptorTension").val("");
                    $("#InterruptorCapacidadA").val("");
                    $("#InterruptorCapacidadMva").val("");
                }
            },
            error: function () {
                mostrarMensaje('Ha ocurrido un error.');
            }
        });
    }, 100);

};

function editarComentario(id) {

    const comentario = $('#' + id).val();

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
        mensajeConfirmar = "¿Está seguro de modificar el registro de la celda de acoplamiento?";
    } else if (acc == "Incluir") {
        mensajeConfirmar = "¿Está seguro de guardar el registro de la celda de acoplamiento?";
    }

    limpiarMensaje();
    if (validarGuardar()) {
        let Equicodi = $('#CodigoId').val();
        let IdCelda = $('#IdCeldaAcoplamiento').val();
        let IdProyecto = $('#IdProyecto').val();
        let Fecha = $('#FechaActualizacion').val();

        let IdInterruptor = obtenerValorMultiselect('IdInterruptor');

        let CapacidadA = $('#InterruptorCapacidadA').val();
        let CapacidadAComent = $('#InterruptorCapacidadAComent').val();
        let CapacidadMvar = $('#InterruptorCapacidadMva').val();
        let CapacidadMvarComent = $('#InterruptorCapacidadMvaComent').val();
        let CapacidadTransmisionA = $('#CapacidadTransmisionA').val();
        let CapacidadTransmisionAComent = $('#CapacidadTransmisionAComent').val();
        let CapacidadTransmisionMvar = $('#CapacidadTransmisionMva').val();
        let CapacidadTransmisionMvarComent = $('#CapacidadTransmisionMvaComent').val();
        let FactorLimitanteCalc = $('#FactorLimitanteCalc').val();
        let FactorLimitanteCalcComent = $('#FactorLimitanteCalcComent').val();
        let FactorLimitanteFinal = $('#FactorLimitanteFinal').val();
        let FactorLimitanteFinalComent = $('#FactorLimitanteFinalComent').val();
        let Observaciones = $('#Observaciones').val();

        let textoConfirmación = mensajeConfirmar;

        if (confirm(textoConfirmación)) {

            let datos = {
                Equicodi: Equicodi,
                IdCelda: IdCelda,
                IdProyecto: IdProyecto,
                Fecha: Fecha,
                IdInterruptor: IdInterruptor,
                CapacidadA: CapacidadA,
                CapacidadAComent: CapacidadAComent,
                CapacidadMvar: CapacidadMvar,
                CapacidadMvarComent: CapacidadMvarComent,
                CapacidadTransmisionA: CapacidadTransmisionA,
                CapacidadTransmisionAComent: CapacidadTransmisionAComent,
                CapacidadTransmisionMvar: CapacidadTransmisionMvar,
                CapacidadTransmisionMvarComent: CapacidadTransmisionMvarComent,
                FactorLimitanteCalc: FactorLimitanteCalc,
                FactorLimitanteCalcComent: FactorLimitanteCalcComent,
                FactorLimitanteFinal: FactorLimitanteFinal,
                FactorLimitanteFinalComent: FactorLimitanteFinalComent,
                Observaciones: Observaciones,
            };

            $.ajax({
                type: 'POST',
                url: controlador + 'GuardarCeldaAcoplamiento',
                dataType: 'json',
                data: datos,
                cache: false,
                success: function (resultado) {
                    if (resultado == "") {
                        mostrarMensaje('Se ha guardado el registro de la Celda de Acoplamiento correctamente.', 'exito');
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

    if ($('#IdInterruptor').val() == '0') campos.push('Interruptor');

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

let calcular = function () {

    let Equicodi = $('#CodigoId').val();
    let IdCelda = $('#IdCeldaAcoplamiento').val();
    let IdProyecto = $('#IdProyecto').val();

    let IdInterruptor = obtenerValorMultiselect('IdInterruptor');

    let CapacidadA = $('#InterruptorCapacidadA').val();
    let CapacidadMvar = $('#InterruptorCapacidadMva').val();
    let CapacidadTransmisionA = $('#CapacidadTransmisionA').val();
    let CapacidadTransmisionMvar = $('#CapacidadTransmisionMva').val();
    let FactorLimitanteCalc = $('#FactorLimitanteCalc').val();
    let FactorLimitanteFinal = $('#FactorLimitanteFinal').val();

    limpiarMensaje();

    let datos = {
        Equicodi: Equicodi,
        IdCelda: IdCelda,
        IdProyecto: IdProyecto,
        IdInterruptor: IdInterruptor,
        CapacidadA: CapacidadA,
        CapacidadMvar: CapacidadMvar,
        CapacidadTransmisionA: CapacidadTransmisionA,
        CapacidadTransmisionMvar: CapacidadTransmisionMvar,
        FactorLimitanteCalc: FactorLimitanteCalc,
        FactorLimitanteFinal: FactorLimitanteFinal
    };

    $.ajax({
        type: 'POST',
        url: controlador + 'CalcularCeldaAcoplamiento',
        dataType: 'json',
        data: datos,
        cache: false,
        success: function (resultado) {
            if (resultado.MensajeError == "") {
                mostrarMensaje('Se realizó el cálculo de los valores.', 'exito');

                $('#InterruptorCapacidadMva').val(resultado.CapacidadMvar);
                $('#CapacidadTransmisionA').val(resultado.CapacidadTransmisionA);
                $('#CapacidadTransmisionMva').val(resultado.CapacidadTransmisionMvar);
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

let guardar2 = function (muestramensaje) {

    limpiarMensaje();

    let Equicodi = $('#CodigoId').val();
    let IdCelda = $('#IdCeldaAcoplamiento').val();
    let IdProyecto = $('#IdProyecto').val();
    let Fecha = $('#FechaActualizacion').val();

    let IdInterruptor = obtenerValorMultiselect('IdInterruptor');

    let CapacidadA = $('#InterruptorCapacidadA').val();
    let CapacidadAComent = $('#InterruptorCapacidadAComent').val();
    let CapacidadMvar = $('#InterruptorCapacidadMva').val();
    let CapacidadMvarComent = $('#InterruptorCapacidadMvaComent').val();
    let CapacidadTransmisionA = $('#CapacidadTransmisionA').val();
    let CapacidadTransmisionAComent = $('#CapacidadTransmisionAComent').val();
    let CapacidadTransmisionMvar = $('#CapacidadTransmisionMva').val();
    let CapacidadTransmisionMvarComent = $('#CapacidadTransmisionMvaComent').val();
    let FactorLimitanteCalc = $('#FactorLimitanteCalc').val();
    let FactorLimitanteCalcComent = $('#FactorLimitanteCalcComent').val();
    let FactorLimitanteFinal = $('#FactorLimitanteFinal').val();
    let FactorLimitanteFinalComent = $('#FactorLimitanteFinalComent').val();
    let Observaciones = $('#Observaciones').val();

    let datos = {
        Equicodi: Equicodi,
        IdCelda: IdCelda,
        IdProyecto: IdProyecto,
        Fecha: Fecha,
        IdInterruptor: IdInterruptor,
        CapacidadA: CapacidadA,
        CapacidadAComent: CapacidadAComent,
        CapacidadMvar: CapacidadMvar,
        CapacidadMvarComent: CapacidadMvarComent,
        CapacidadTransmisionA: CapacidadTransmisionA,
        CapacidadTransmisionAComent: CapacidadTransmisionAComent,
        CapacidadTransmisionMvar: CapacidadTransmisionMvar,
        CapacidadTransmisionMvarComent: CapacidadTransmisionMvarComent,
        FactorLimitanteCalc: FactorLimitanteCalc,
        FactorLimitanteCalcComent: FactorLimitanteCalcComent,
        FactorLimitanteFinal: FactorLimitanteFinal,
        FactorLimitanteFinalComent: FactorLimitanteFinalComent,
        Observaciones: Observaciones,
    };

    $.ajax({
        type: 'POST',
        url: controlador + 'GuardarCeldaAcoplamiento',
        dataType: 'json',
        data: datos,
        cache: false,
        success: function (resultado) {

            if (muestramensaje) {
                if (resultado == "") {
                    mostrarMensaje('Se ha guardado el registro de la Celda de Acoplamiento correctamente.', 'exito');
                } else
                    alert(resultado)
            }
            

        },
        error: function () {
            mostrarMensaje('Ha ocurrido un error.', 'error');
        }
    });

};

let calcularAntesGuardar = function () {

    const acc = $("#Accion").val();
    let mensajeConfirmar = "";
    if (acc == "Modificar") {
        mensajeConfirmar = "¿Está seguro de modificar el registro de la celda de acoplamiento?";
    } else if (acc == "Incluir") {
        mensajeConfirmar = "¿Está seguro de guardar el registro de la celda de acoplamiento?";
    }

    limpiarMensaje();
    if (validarGuardar()) {
        let Equicodi = $('#CodigoId').val();
        let IdCelda = $('#IdCeldaAcoplamiento').val();
        let IdProyecto = $('#IdProyecto').val();

        let IdInterruptor = obtenerValorMultiselect('IdInterruptor');

        let CapacidadA = $('#InterruptorCapacidadA').val();
        let CapacidadMvar = $('#InterruptorCapacidadMva').val();
        let CapacidadTransmisionA = $('#CapacidadTransmisionA').val();
        let CapacidadTransmisionMvar = $('#CapacidadTransmisionMva').val();
        let FactorLimitanteCalc = $('#FactorLimitanteCalc').val();
        let FactorLimitanteFinal = $('#FactorLimitanteFinal').val();

        let textoConfirmación = mensajeConfirmar;

        if (confirm(textoConfirmación)) {

            guardar2(false);

            let datos = {
                Equicodi: Equicodi,
                IdCelda: IdCelda,
                IdProyecto: IdProyecto,
                IdInterruptor: IdInterruptor,
                CapacidadA: CapacidadA,
                CapacidadMvar: CapacidadMvar,
                CapacidadTransmisionA: CapacidadTransmisionA,
                CapacidadTransmisionMvar: CapacidadTransmisionMvar,
                FactorLimitanteCalc: FactorLimitanteCalc,
                FactorLimitanteFinal: FactorLimitanteFinal
            };

            $.ajax({
                type: 'POST',
                url: controlador + 'CalcularCeldaAcoplamiento',
                dataType: 'json',
                data: datos,
                cache: false,
                success: function (resultado) {
                    if (resultado.MensajeError == "") {
                        mostrarMensaje('Se realizó el cálculo de los valores.', 'exito');

                        $('#InterruptorCapacidadMva').val(resultado.CapacidadMvar);

                        $('#CapacidadTransmisionA').val(resultado.CapacidadTransmisionA);
                        $('#CapacidadTransmisionMva').val(resultado.CapacidadTransmisionMvar);
                        $('#FactorLimitanteCalc').val(resultado.FactorLimitanteCalc);
                        $('#FactorLimitanteFinal').val(resultado.FactorLimitanteFinal);

                        guardar2(true);
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