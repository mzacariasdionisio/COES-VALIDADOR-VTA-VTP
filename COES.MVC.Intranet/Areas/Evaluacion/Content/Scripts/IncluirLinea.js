//Declarando las variables 
let controlador = siteRoot + 'Linea/';

$(function () {

    $("#popUpEditarComentario").addClass("general-popup");

    $("#btnIncluir").click(function () {
        incluir(0);
    });

    $("#btnRegresar").click(function () {
        regresar();
    });

    $('#btnGuardar').click(function () {
        calcularAntesGuardar();
    });

    inputSoloNumerosDecimales([
        'BancoCapacidadA',
        'BancoCapacidadMVAr',
        'CapacTransCond1Porcen',
        'CapacTransCond2Porcen',
        'CapacTransCond2Min',
        'CapacTransCond1Min',
        'LimiteSegCoes',
        'FactorLimitanteCalc',
        'FactorLimitanteFinal',
        'CapacidadABancoCondensador',
        'CapacidadMvarBancoCondensador'
    ]);

    inputSoloNumeros('CapacidadA');

    $("#btnCalcular").click(function () {
        validarGuardar();
        guardar2(false);
        calcular();
    });

    $('#IdCelda').multipleSelect({
        width: '98%',
        filter: true,
        single: true
    });

    $('#IdCelda2').multipleSelect({
        width: '98%',
        filter: true,
        single: true
    });

    $('#IdBancoCondensador').multipleSelect({
        width: '98%',
        filter: true,
        single: true
    });

    $('#IdCelda').change(function () {
        let IdCelda = obtenerValorMultiselect('IdCelda');
        let IdCelda2 = obtenerValorMultiselect('IdCelda2');
        
        consultarCelda(IdCelda);
        consultarAreaxCelda(IdCelda, IdCelda2);
    });

    $('#IdCelda2').change(function () {
        let IdCelda = obtenerValorMultiselect('IdCelda');
        let IdCelda2 = obtenerValorMultiselect('IdCelda2');

        consultarCelda2(IdCelda2);
        consultarAreaxCelda(IdCelda, IdCelda2);
    });

    $('#IdBancoCondensador').change(function () {
        let IdBanco = obtenerValorMultiselect('IdBancoCondensador');

        consultarBanco(IdBanco);

        HabilitarCamposBancoCondensador(IdBanco);
    });

    setearMultiselect("hiddenIdCelda", "IdCelda");
    setearMultiselect("hiddenIdCelda2", "IdCelda2");
    setearMultiselect("hiddenIdBancoCondensador", "IdBancoCondensador");

    let IdCelda = obtenerValorMultiselect('IdCelda');
    let IdCelda2 = obtenerValorMultiselect('IdCelda2');
    let IdBanco = obtenerValorMultiselect('IdBancoCondensador');

    consultarAreaxCelda(IdCelda, IdCelda2);

    HabilitarCamposBancoCondensador(IdBanco);

    calcular();
});
function regresar() {
    const accion = $("#Accion").val();
    if (accion == "Modificar") {
        window.location.href = siteRoot + "Evaluacion/Linea/Index/";
    } else if (accion == "Incluir") {
        window.location.href = controlador + "BuscarLinea";
    }
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
            mostrarError();
        }
    });
};





let mostrarError = function () {
    $('#mensaje').removeClass("action-exito");
    $('#mensaje').addClass("action-error");
    $('#mensaje').text("Ha ocurrido un error");
    $('#mensaje').css("display", "block");
};

function validarGuardar() {
    let campos = [];

    if ($('#IdCelda').val() == '0') campos.push('Celda1/Celda');
    if ($('#IdCelda2').val() == '0') campos.push('Celta2/Celda');

    if (campos.length > 0) {

        mensajeValidador(campos);
        return false;
    }

    return true;
}

function mensajeValidador(campos) {
    let texto = "Los campos: \n";

    for (const campo of campos) {
        texto += campo + "(*) \n";
    }

    alert(texto + " son requeridos");
}

let guardar = function () {

    const acc = $("#Accion").val();
    let mensajeConfirmar = "";
    if (acc == "Modificar") {
        mensajeConfirmar = "¿Está seguro de modificar el registro de la línea?";
    } else if (acc == "Incluir") {
        mensajeConfirmar = "¿Está seguro de guardar el registro de la línea?";
    }

    limpiarMensaje();
    if (validarGuardar()) {

        let IdCelda = obtenerValorMultiselect('IdCelda');
        let IdCelda2 = obtenerValorMultiselect('IdCelda2');

        if (IdCelda == IdCelda2) {
            alert("El valor de la Celda 1 y Celda 2 no deben ser iguales");
            return;
        }

        let IdLinea = $('#IdLinea').val();
        let IdProyecto = $('#IdProyecto').val();
        let FechaMotivo = $('#FechaMotivo').val();
        let Equicodi = $('#Equicodi').val();
        let IdArea = $('#IdArea').val();
        let CapacidadA = $('#CapacidadA').val();
        let CapacidadAComent = $('#CapacidadAComent').val();
        let CapacidadMva = $('#CapacidadMva').val();
        let CapacidadMvaComent = $('#CapacidadMvaComent').val();

        let IdBancoCondensador = obtenerValorMultiselect('IdBancoCondensador');

        let CapacidadABancoCondensador = $('#CapacidadABancoCondensador').val();
        let CapacidadABancoCondensadorComent = $('#CapacidadABancoCondensadorComent').val();
        let CapacidadMvarBancoCondensador = $('#CapacidadMvarBancoCondensador').val();
        let CapacidadMvarBancoCondensadorComent = $('#CapacidadMvarBancoCondensadorComent').val();
        let CapacTransCond1Porcen = $('#CapacTransCond1Porcen').val();
        let CapacTransCond1PorcenComent = $('#CapacTransCond1PorcenComent').val();

        let CapacTransCond1Min = $('#CapacTransCond1Min').val();
        let CapacTransCond1MinComent = $('#CapacTransCond1MinComent').val();
        let CapacTransCond1A = $('#CapacTransCond1A').val();
        let CapacTransCond1AComent = $('#CapacTransCond1AComent').val();
        let CapacTransCond2Porcen = $('#CapacTransCond2Porcen').val();
        let CapacTransCond2PorcenComent = $('#CapacTransCond2PorcenComent').val();
        let CapacTransCond2Min = $('#CapacTransCond2Min').val();
        let CapacTransCond2MinComent = $('#CapacTransCond2MinComent').val();
        let CapacTransCond2A = $('#CapacTransCond2A').val();
        let CapacTransCond2AComent = $('#CapacTransCond2AComent').val();
        let CapacidadTransmisionA = $('#CapacidadTransmisionA').val();
        let CapacidadTransmisionAComent = $('#CapacidadTransmisionAComent').val();
        let CapacidadTransmisionMva = $('#CapacidadTransmisionMva').val();
        let CapacidadTransmisionMvaComent = $('#CapacidadTransmisionMvaComent').val();
        let LimiteSegCoes = $('#LimiteSegCoes').val();
        let LimiteSegCoesComent = $('#LimiteSegCoesComent').val();
        let FactorLimitanteCalc = $('#FactorLimitanteCalc').val();
        let FactorLimitanteCalcComent = $('#FactorLimitanteCalcComent').val();
        let FactorLimitanteFinal = $('#FactorLimitanteFinal').val();
        let FactorLimitanteFinalComent = $('#FactorLimitanteFinalComent').val();
        let Observaciones = $('#Observaciones').val();

        let textoConfirmación = mensajeConfirmar;
   
        if (confirm(textoConfirmación)) {
            let datos = {
                IdEquipo: 0,
                IdLinea: IdLinea,
                IdProyecto: IdProyecto,
                FechaMotivo: FechaMotivo,
                Equicodi: Equicodi,
                IdArea: IdArea,
                CapacidadA: CapacidadA,
                CapacidadMva: CapacidadMva,
                IdCelda: IdCelda,
                IdCelda2: IdCelda2,
                IdBancoCondensador: IdBancoCondensador,
                CapacidadABancoCondensador: CapacidadABancoCondensador,
                CapacidadMvarBancoCondensador: CapacidadMvarBancoCondensador,
                CapacTransCond1Porcen: CapacTransCond1Porcen,
                CapacTransCond1Min: CapacTransCond1Min,
                CapacTransCond1A: CapacTransCond1A,
                CapacTransCond2Porcen: CapacTransCond2Porcen,
                CapacTransCond2Min: CapacTransCond2Min,
                CapacTransCond2A: CapacTransCond2A,
                CapacidadTransmisionA: CapacidadTransmisionA,
                CapacidadTransmisionMva: CapacidadTransmisionMva,
                LimiteSegCoes: LimiteSegCoes,
                FactorLimitanteCalc: FactorLimitanteCalc,
                FactorLimitanteFinal: FactorLimitanteFinal,
                Observaciones: Observaciones,

                CapacidadAComent: CapacidadAComent,
                CapacidadMvaComent: CapacidadMvaComent,
                CapacidadABancoCondensadorComent: CapacidadABancoCondensadorComent,
                CapacidadMvarBancoCondensadorComent: CapacidadMvarBancoCondensadorComent,
                CapacTransCond1PorcenComent: CapacTransCond1PorcenComent,
                CapacTransCond1MinComent: CapacTransCond1MinComent,
                CapacTransCond1AComent: CapacTransCond1AComent,
                CapacTransCond2PorcenComent: CapacTransCond2PorcenComent,
                CapacTransCond2MinComent: CapacTransCond2MinComent,
                CapacTransCond2AComent: CapacTransCond2AComent,
                CapacidadTransmisionAComent: CapacidadTransmisionAComent,
                CapacidadTransmisionMvaComent: CapacidadTransmisionMvaComent,
                

                LimiteSegCoesComent: LimiteSegCoesComent,
                FactorLimitanteCalcComent: FactorLimitanteCalcComent,
                FactorLimitanteFinalComent: FactorLimitanteFinalComent

            };

            $.ajax({
                type: 'POST',
                url: controlador + 'GuardarLinea',
                dataType: 'json',
                data: datos,
                cache: false,
                success: function (resultado) {
                    if (resultado == "") {
                        mostrarMensaje('Se ha guardado el registro de la Línea correctamente.', 'exito');
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

let consultarCelda = function (IdCelda) {
    setTimeout(function () {
        $.ajax({
            type: 'POST',
            url: controlador + 'ConsultaCelda',
            data: { IdCelda: IdCelda },
            success: function (evt) {
                $('#mensaje').css("display", "none");

                if (evt) {
                    $('#Celda1Posicion').val(evt.PosicionNucleoTc);
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
                    $('#Celda2Posicion').val(evt.PosicionNucleoTc);
                    $('#Celda2PickUp').val(evt.PickUp);
                }
            },
            error: function () {
                mostrarMensaje('Ha ocurrido un error.', 'error');
            }
        });
    }, 100);
};


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

let limpiarMensaje = function () {
    $('#mensaje').css("display", "none");

};

let consultarBanco = function (IdBanco) {
    setTimeout(function () {
        $.ajax({
            type: 'POST',
            url: controlador + 'ConsultaBanco',
            data: { IdBanco: IdBanco },
            success: function (evt) {

                if (evt) {
                    $('#CapacidadABancoCondensador').val(evt.CapacidadA);
                    $('#CapacidadMvarBancoCondensador').val(evt.CapacidadMvar);
                }
            },
            error: function () {
                mostrarMensaje('Ha ocurrido un error.');
            }
        });
    }, 100);
};

let calcular = function () {

    limpiarMensaje();
    let IdLinea = $('#IdLinea').val();
    let IdProyecto = $('#IdProyecto').val();

    let Equicodi = $('#Equicodi').val();
    let IdArea = $('#IdArea').val();
    let CapacidadA = $('#CapacidadA').val();
    let CapacidadMva = $('#CapacidadMva').val();

    let IdCelda = obtenerValorMultiselect('IdCelda');
    let IdCelda2 = obtenerValorMultiselect('IdCelda2');
    let IdBancoCondensador = obtenerValorMultiselect('IdBancoCondensador');

    let CapacTransCond1Porcen = $('#CapacTransCond1Porcen').val();
    let CapacTransCond1Min = $('#CapacTransCond1Min').val();
    let CapacTransCond1A = $('#CapacTransCond1A').val();
    let CapacTransCond2Porcen = $('#CapacTransCond2Porcen').val();
    let CapacTransCond2Min = $('#CapacTransCond2Min').val();
    let CapacTransCond2A = $('#CapacTransCond2A').val();
    let CapacidadTransmisionA = $('#CapacidadTransmisionA').val();
    let CapacidadTransmisionMva = $('#CapacidadTransmisionMva').val();
    let LimiteSegCoes = $('#LimiteSegCoes').val();
    let FactorLimitanteCalc = $('#FactorLimitanteCalc').val();
    let FactorLimitanteFinal = $('#FactorLimitanteFinal').val();       
    let Tension = $('#TensionKv').val();       

    let datos = {
        IdLinea: IdLinea,
        IdProyecto: IdProyecto,
        Equicodi: Equicodi,
        IdArea: IdArea,
        CapacidadA: CapacidadA,
        CapacidadMva: CapacidadMva,
        IdCelda: IdCelda,
        IdCelda2: IdCelda2,
        IdBancoCondensador: IdBancoCondensador,      
        CapacTransCond1Porcen: CapacTransCond1Porcen,
        CapacTransCond1Min: CapacTransCond1Min,
        CapacTransCond1A: CapacTransCond1A,
        CapacTransCond2Porcen: CapacTransCond2Porcen,
        CapacTransCond2Min: CapacTransCond2Min,
        CapacTransCond2A: CapacTransCond2A,
        CapacidadTransmisionA: CapacidadTransmisionA,
        CapacidadTransmisionMva: CapacidadTransmisionMva,
        LimiteSegCoes: LimiteSegCoes,
        FactorLimitanteCalc: FactorLimitanteCalc,
        FactorLimitanteFinal: FactorLimitanteFinal,
        Tension: Tension
    };

    $.ajax({
        type: 'POST',
        url: controlador + 'CalcularLinea',
        dataType: 'json',
        data: datos,
        cache: false,
        success: function (resultado) {
            if (resultado.MensajeError == "") {
                mostrarMensaje('Se realizó el cálculo de los valores.', 'exito');

                $('#CapacidadMva').val(resultado.CapacidadMva);

                $('#CapacTransCond1A').val(resultado.CapacTransCond1A);
                $('#CapacTransCond2A').val(resultado.CapacTransCond2A);
                $('#CapacidadTransmisionA').val(resultado.CapacidadTransmisionA);
                $('#CapacidadTransmisionMva').val(resultado.CapacidadTransmisionMva);
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

let consultarAreaxCelda = function (celda1, celda2) {
    setTimeout(function () {
        $.ajax({
            type: 'POST',
            url: controlador + 'ConsultaAreaxCelda',
            data: {
                celda1: celda1,
                celda2: celda2
            },
            success: function (evt) {
                $('#mensaje').css("display", "none");
                const idArea = $("#HiddenIdArea").val();

                let seleccion = $('#IdArea');
                seleccion.empty();
                seleccion.append('<option value="0">SELECCIONAR</option>');
                $.each(evt, function (index, item) {
                    if (idArea == item.AreaCodi) {
                        seleccion.append('<option value="' + item.AreaCodi + '" selected>' + item.AreaNomb + '</option>');
                    } else {
                        seleccion.append('<option value="' + item.AreaCodi + '">' + item.AreaNomb + '</option>');
                    }
                    
                });
            },
            error: function () {
                mostrarMensaje('Ha ocurrido un error.', 'error');
            }
        });
    }, 100);
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
        mensajeConfirmar = "¿Está seguro de modificar el registro de la línea?";
    } else if (acc == "Incluir") {
        mensajeConfirmar = "¿Está seguro de guardar el registro de la línea?";
    }

    limpiarMensaje();
    if (validarGuardar()) {

        let IdCelda = obtenerValorMultiselect('IdCelda');
        let IdCelda2 = obtenerValorMultiselect('IdCelda2');

        if (IdCelda == IdCelda2) {
            alert("El valor de la Celda 1 y Celda 2 no deben ser iguales");
            return;
        }

        let IdLinea = $('#IdLinea').val();
        let IdProyecto = $('#IdProyecto').val();

        let Equicodi = $('#Equicodi').val();
        let IdArea = $('#IdArea').val();
        let CapacidadA = $('#CapacidadA').val();
        let CapacidadMva = $('#CapacidadMva').val();

        let IdBancoCondensador = obtenerValorMultiselect('IdBancoCondensador');

        let CapacTransCond1Porcen = $('#CapacTransCond1Porcen').val();
        let CapacTransCond1Min = $('#CapacTransCond1Min').val();
        let CapacTransCond1A = $('#CapacTransCond1A').val();
        let CapacTransCond2Porcen = $('#CapacTransCond2Porcen').val();
        let CapacTransCond2Min = $('#CapacTransCond2Min').val();
        let CapacTransCond2A = $('#CapacTransCond2A').val();
        let CapacidadTransmisionA = $('#CapacidadTransmisionA').val();
        let CapacidadTransmisionMva = $('#CapacidadTransmisionMva').val();
        let LimiteSegCoes = $('#LimiteSegCoes').val();
        let FactorLimitanteCalc = $('#FactorLimitanteCalc').val();
        let FactorLimitanteFinal = $('#FactorLimitanteFinal').val();
        let Tension = $('#TensionKv').val();

        let textoConfirmación = mensajeConfirmar;

        if (confirm(textoConfirmación)) {

            guardar2(false);

        let datos = {
            IdLinea: IdLinea,
            IdProyecto: IdProyecto,
            Equicodi: Equicodi,
            IdArea: IdArea,
            CapacidadA: CapacidadA,
            CapacidadMva: CapacidadMva,
            IdCelda: IdCelda,
            IdCelda2: IdCelda2,
            IdBancoCondensador: IdBancoCondensador,
            CapacTransCond1Porcen: CapacTransCond1Porcen,
            CapacTransCond1Min: CapacTransCond1Min,
            CapacTransCond1A: CapacTransCond1A,
            CapacTransCond2Porcen: CapacTransCond2Porcen,
            CapacTransCond2Min: CapacTransCond2Min,
            CapacTransCond2A: CapacTransCond2A,
            CapacidadTransmisionA: CapacidadTransmisionA,
            CapacidadTransmisionMva: CapacidadTransmisionMva,
            LimiteSegCoes: LimiteSegCoes,
            FactorLimitanteCalc: FactorLimitanteCalc,
            FactorLimitanteFinal: FactorLimitanteFinal,
            Tension: Tension
        };

            $.ajax({
                type: 'POST',
                url: controlador + 'CalcularLinea',
                dataType: 'json',
                data: datos,
                cache: false,
                success: function (resultado) {
                    if (resultado.MensajeError == "") {
                        mostrarMensaje('Se realizó el cálculo de los valores.', 'exito');

                        $('#CapacidadMva').val(resultado.CapacidadMva);

                        $('#CapacTransCond1A').val(resultado.CapacTransCond1A);
                        $('#CapacTransCond2A').val(resultado.CapacTransCond2A);
                        $('#CapacidadTransmisionA').val(resultado.CapacidadTransmisionA);
                        $('#CapacidadTransmisionMva').val(resultado.CapacidadTransmisionMva);
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

let guardar2 = function (muestramensaje) {

    limpiarMensaje();
    let IdCelda = obtenerValorMultiselect('IdCelda');
    let IdCelda2 = obtenerValorMultiselect('IdCelda2');

    let IdLinea = $('#IdLinea').val();
    let IdProyecto = $('#IdProyecto').val();
    let FechaMotivo = $('#FechaMotivo').val();
    let Equicodi = $('#Equicodi').val();
    let IdArea = $('#IdArea').val();
    let CapacidadA = $('#CapacidadA').val();
    let CapacidadAComent = $('#CapacidadAComent').val();
    let CapacidadMva = $('#CapacidadMva').val();
    let CapacidadMvaComent = $('#CapacidadMvaComent').val();

    let IdBancoCondensador = obtenerValorMultiselect('IdBancoCondensador');

    let CapacidadABancoCondensador = $('#CapacidadABancoCondensador').val();
    let CapacidadABancoCondensadorComent = $('#CapacidadABancoCondensadorComent').val();
    let CapacidadMvarBancoCondensador = $('#CapacidadMvarBancoCondensador').val();
    let CapacidadMvarBancoCondensadorComent = $('#CapacidadMvarBancoCondensadorComent').val();
    let CapacTransCond1Porcen = $('#CapacTransCond1Porcen').val();
    let CapacTransCond1PorcenComent = $('#CapacTransCond1PorcenComent').val();

    let CapacTransCond1Min = $('#CapacTransCond1Min').val();
    let CapacTransCond1MinComent = $('#CapacTransCond1MinComent').val();
    let CapacTransCond1A = $('#CapacTransCond1A').val();
    let CapacTransCond1AComent = $('#CapacTransCond1AComent').val();
    let CapacTransCond2Porcen = $('#CapacTransCond2Porcen').val();
    let CapacTransCond2PorcenComent = $('#CapacTransCond2PorcenComent').val();
    let CapacTransCond2Min = $('#CapacTransCond2Min').val();
    let CapacTransCond2MinComent = $('#CapacTransCond2MinComent').val();
    let CapacTransCond2A = $('#CapacTransCond2A').val();
    let CapacTransCond2AComent = $('#CapacTransCond2AComent').val();
    let CapacidadTransmisionA = $('#CapacidadTransmisionA').val();
    let CapacidadTransmisionAComent = $('#CapacidadTransmisionAComent').val();
    let CapacidadTransmisionMva = $('#CapacidadTransmisionMva').val();
    let CapacidadTransmisionMvaComent = $('#CapacidadTransmisionMvaComent').val();
    let LimiteSegCoes = $('#LimiteSegCoes').val();
    let LimiteSegCoesComent = $('#LimiteSegCoesComent').val();
    let FactorLimitanteCalc = $('#FactorLimitanteCalc').val();
    let FactorLimitanteCalcComent = $('#FactorLimitanteCalcComent').val();
    let FactorLimitanteFinal = $('#FactorLimitanteFinal').val();
    let FactorLimitanteFinalComent = $('#FactorLimitanteFinalComent').val();
    let Observaciones = $('#Observaciones').val();

        let datos = {
            IdEquipo: 0,
            IdLinea: IdLinea,
            IdProyecto: IdProyecto,
            FechaMotivo: FechaMotivo,
            Equicodi: Equicodi,
            IdArea: IdArea,
            CapacidadA: CapacidadA,
            CapacidadMva: CapacidadMva,
            IdCelda: IdCelda,
            IdCelda2: IdCelda2,
            IdBancoCondensador: IdBancoCondensador,
            CapacidadABancoCondensador: CapacidadABancoCondensador,
            CapacidadMvarBancoCondensador: CapacidadMvarBancoCondensador,
            CapacTransCond1Porcen: CapacTransCond1Porcen,
            CapacTransCond1Min: CapacTransCond1Min,
            CapacTransCond1A: CapacTransCond1A,
            CapacTransCond2Porcen: CapacTransCond2Porcen,
            CapacTransCond2Min: CapacTransCond2Min,
            CapacTransCond2A: CapacTransCond2A,
            CapacidadTransmisionA: CapacidadTransmisionA,
            CapacidadTransmisionMva: CapacidadTransmisionMva,
            LimiteSegCoes: LimiteSegCoes,
            FactorLimitanteCalc: FactorLimitanteCalc,
            FactorLimitanteFinal: FactorLimitanteFinal,
            Observaciones: Observaciones,

            CapacidadAComent: CapacidadAComent,
            CapacidadMvaComent: CapacidadMvaComent,
            CapacidadABancoCondensadorComent: CapacidadABancoCondensadorComent,
            CapacidadMvarBancoCondensadorComent: CapacidadMvarBancoCondensadorComent,
            CapacTransCond1PorcenComent: CapacTransCond1PorcenComent,
            CapacTransCond1MinComent: CapacTransCond1MinComent,
            CapacTransCond1AComent: CapacTransCond1AComent,
            CapacTransCond2PorcenComent: CapacTransCond2PorcenComent,
            CapacTransCond2MinComent: CapacTransCond2MinComent,
            CapacTransCond2AComent: CapacTransCond2AComent,
            CapacidadTransmisionAComent: CapacidadTransmisionAComent,
            CapacidadTransmisionMvaComent: CapacidadTransmisionMvaComent,


            LimiteSegCoesComent: LimiteSegCoesComent,
            FactorLimitanteCalcComent: FactorLimitanteCalcComent,
            FactorLimitanteFinalComent: FactorLimitanteFinalComent

        };

    $.ajax({
        type: 'POST',
        url: controlador + 'GuardarLinea',
        dataType: 'json',
        data: datos,
        cache: false,
        success: function (resultado) {

            if (muestramensaje) {
                if (resultado == "") {
                    mostrarMensaje('Se ha guardado el registro de la Línea correctamente.', 'exito');
                } else
                    alert(resultado);
            }
           

        },
        error: function () {
            mostrarMensaje('Ha ocurrido un error.', 'error');
        }
    });

};

function HabilitarCamposBancoCondensador(IdBanco) {
    if (IdBanco > 0) {
        $('#CapacidadABancoCondensador').prop('disabled', false);
        $('#CapacidadMvarBancoCondensador').prop('disabled', false);
    } else {
        $('#CapacidadABancoCondensador').prop('disabled', true);
        $('#CapacidadMvarBancoCondensador').prop('disabled', true);
    }
}