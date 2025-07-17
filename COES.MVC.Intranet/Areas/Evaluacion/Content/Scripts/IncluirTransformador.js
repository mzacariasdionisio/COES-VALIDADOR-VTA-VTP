//Declarando las variables 
let controlador = siteRoot + 'Transformador/';
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

    mostrarDevanado();

    $('#D1IdCelda').multipleSelect({
        width: '98%',
        filter: true,
        single: true
    });

    $('#D2IdCelda').multipleSelect({
        width: '98%',
        filter: true,
        single: true
    });

    $('#D3IdCelda').multipleSelect({
        width: '98%',
        filter: true,
        single: true
    });

    $('#D4IdCelda').multipleSelect({
        width: '98%',
        filter: true,
        single: true
    });

    $('#D1IdCelda').change(function () {
        let IdCelda = obtenerValorMultiselect('D1IdCelda');
        consultarCelda(IdCelda);
    });

    $('#D2IdCelda').change(function () {
        let IdCelda = obtenerValorMultiselect('D2IdCelda');
        consultarCelda2(IdCelda);
    });
    $('#D3IdCelda').change(function () {
        let IdCelda = obtenerValorMultiselect('D3IdCelda');
        consultarCelda3(IdCelda);
    });
    $('#D4IdCelda').change(function () {
        let IdCelda = obtenerValorMultiselect('D4IdCelda');
        consultarCelda4(IdCelda);
    });

    setearMultiselect("hiddenD1IdCelda", "D1IdCelda");
    setearMultiselect("hiddenD2IdCelda", "D2IdCelda");
    setearMultiselect("hiddenD3IdCelda", "D3IdCelda");
    setearMultiselect("hiddenD4IdCelda", "D4IdCelda");

    inputSoloNumerosDecimales([
        'D1CapacidadOnanMva',
        'D1CapacidadOnafMva',
        'D2CapacidadOnanMva',
        'D2CapacidadOnafMva',
        'D3CapacidadOnanMva',
        'D3CapacidadOnafMva',
        'D4CapacidadOnanMva',
        'D4CapacidadOnafMva'
    ]);

    $("#btnCalcular").click(function () {
        calcular();
    });

    calcular();
});
function regresar() {
    const accion = $("#Accion").val();
    if (accion == "Modificar") {
        window.location.href = siteRoot + "Evaluacion/Transformador/Index/";
    } else if (accion == "Incluir") {
        window.location.href = controlador + "BuscarTransformador";
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
            mostrarMensaje('Ha ocurrido un error.', 'error');
        }
    });
};

let guardar = function () {

    const acc = $("#Accion").val();
    let mensajeConfirmar = "";
    if (acc == "Modificar") {
        mensajeConfirmar = "¿Está seguro de modificar el registro del transformador?";
    } else if (acc == "Incluir") {
        mensajeConfirmar = "¿Está seguro de guardar el registro del transformador al proyecto?";
    }

    limpiarMensaje();
    if (validarGuardar()) {
        let Equicodi = $('#CodigoId').val();
        let IdTransformador = $('#IdTransformador').val();
        let IdProyecto = $('#IdProyecto').val();
        let Fecha = $('#FechaActualizacion').val();

        let D1IdCelda = obtenerValorMultiselect('D1IdCelda');
        let D1CapacidadOnanMva = $('#D1CapacidadOnanMva').val();
        let D1CapacidadOnanMvaComent = $('#D1CapacidadOnanMvaComent').val();
        let D1CapacidadOnafMva = $('#D1CapacidadOnafMva').val();
        let D1CapacidadOnafMvaComent = $('#D1CapacidadOnafMvaComent').val();
        let D1CapacidadMva = $('#D1CapacidadMva').val();
        let D1CapacidadMvaComent = $('#D1CapacidadMvaComent').val();
        let D1CapacidadA = $('#D1CapacidadA').val();
        let D1CapacidadAComent = $('#D1CapacidadAComent').val();
        let D1PosicionTcA = $('#D1PosicionTcA').val();
        let D1PosicionPickUpA = $('#D1PosicionPickUpA').val();
        let D1CapacidadTransmisionA = $('#D1CapacidadTransmisionA').val();
        let D1CapacidadTransmisionAComent = $('#D1CapacidadTransmisionAComent').val();
        let D1CapacidadTransmisionMva = $('#D1CapacidadTransmisionMva').val();
        let D1CapacidadTransmisionMvaComent = $('#D1CapacidadTransmisionMvaComent').val();
        let D1FactorLimitanteCalc = $('#D1FactorLimitanteCalc').val();
        let D1FactorLimitanteCalcComent = $('#D1FactorLimitanteCalcComent').val();
        let D1FactorLimitanteFinal = $('#D1FactorLimitanteFinal').val();
        let D1FactorLimitanteFinalComent = $('#D1FactorLimitanteFinalComent').val();
        let D2IdCelda = obtenerValorMultiselect('D2IdCelda');
        let D2CapacidadOnanMva = $('#D2CapacidadOnanMva').val();
        let D2CapacidadOnanMvaComent = $('#D2CapacidadOnanMvaComent').val();
        let D2CapacidadOnafMva = $('#D2CapacidadOnafMva').val();
        let D2CapacidadOnafMvaComent = $('#D2CapacidadOnafMvaComent').val();
        let D2CapacidadMva = $('#D2CapacidadMva').val();
        let D2CapacidadMvaComent = $('#D2CapacidadMvaComent').val();
        let D2CapacidadA = $('#D2CapacidadA').val();
        let D2CapacidadAComent = $('#D2CapacidadAComent').val();
        let D2PosicionTcA = $('#D2PosicionTcA').val();
        let D2PosicionPickUpA = $('#D2PosicionPickUpA').val();
        let D2CapacidadTransmisionA = $('#D2CapacidadTransmisionA').val();
        let D2CapacidadTransmisionAComent = $('#D2CapacidadTransmisionAComent').val();
        let D2CapacidadTransmisionMva = $('#D2CapacidadTransmisionMva').val();
        let D2CapacidadTransmisionMvaComent = $('#D2CapacidadTransmisionMvaComent').val();
        let D2FactorLimitanteCalc = $('#D2FactorLimitanteCalc').val();
        let D2FactorLimitanteCalcComent = $('#D2FactorLimitanteCalcComent').val();
        let D2FactorLimitanteFinal = $('#D2FactorLimitanteFinal').val();
        let D2FactorLimitanteFinalComent = $('#D2FactorLimitanteFinalComent').val();
        let D3IdCelda = obtenerValorMultiselect('D3IdCelda');
        let D3CapacidadOnanMva = $('#D3CapacidadOnanMva').val();
        let D3CapacidadOnanMvaComent = $('#D3CapacidadOnanMvaComent').val();
        let D3CapacidadOnafMva = $('#D3CapacidadOnafMva').val();
        let D3CapacidadOnafMvaComent = $('#D3CapacidadOnafMvaComent').val();
        let D3CapacidadMva = $('#D3CapacidadMva').val();
        let D3CapacidadMvaComent = $('#D3CapacidadMvaComent').val();
        let D3CapacidadA = $('#D3CapacidadA').val();
        let D3CapacidadAComent = $('#D3CapacidadAComent').val();
        let D3PosicionTcA = $('#D3PosicionTcA').val();
        let D3PosicionPickUpA = $('#D3PosicionPickUpA').val();
        let D3CapacidadTransmisionA = $('#D3CapacidadTransmisionA').val();
        let D3CapacidadTransmisionAComent = $('#D3CapacidadTransmisionAComent').val();
        let D3CapacidadTransmisionMva = $('#D3CapacidadTransmisionMva').val();
        let D3CapacidadTransmisionMvaComent = $('#D3CapacidadTransmisionMvaComent').val();
        let D3FactorLimitanteCalc = $('#D3FactorLimitanteCalc').val();
        let D3FactorLimitanteCalcComent = $('#D3FactorLimitanteCalcComent').val();
        let D3FactorLimitanteFinal = $('#D3FactorLimitanteFinal').val();
        let D3FactorLimitanteFinalComent = $('#D3FactorLimitanteFinalComent').val();
        let D4IdCelda = obtenerValorMultiselect('D4IdCelda');
        let D4CapacidadOnanMva = $('#D4CapacidadOnanMva').val();
        let D4CapacidadOnanMvaComent = $('#D4CapacidadOnanMvaComent').val();
        let D4CapacidadOnafMva = $('#D4CapacidadOnafMva').val();
        let D4CapacidadOnafMvaComent = $('#D4CapacidadOnafMvaComent').val();
        let D4CapacidadMva = $('#D4CapacidadMva').val();
        let D4CapacidadMvaComent = $('#D4CapacidadMvaComent').val();
        let D4CapacidadA = $('#D4CapacidadA').val();
        let D4CapacidadAComent = $('#D4CapacidadAComent').val();
        let D4PosicionTcA = $('#D4PosicionTcA').val();
        let D4PosicionPickUpA = $('#D4PosicionPickUpA').val();
        let D4CapacidadTransmisionA = $('#D4CapacidadTransmisionA').val();
        let D4CapacidadTransmisionAComent = $('#D4CapacidadTransmisionAComent').val();
        let D4CapacidadTransmisionMva = $('#D4CapacidadTransmisionMva').val();
        let D4CapacidadTransmisionMvaComent = $('#D4CapacidadTransmisionMvaComent').val();
        let D4FactorLimitanteCalc = $('#D4FactorLimitanteCalc').val();
        let D4FactorLimitanteCalcComent = $('#D4FactorLimitanteCalcComent').val();
        let D4FactorLimitanteFinal = $('#D4FactorLimitanteFinal').val();
        let D4FactorLimitanteFinalComent = $('#D4FactorLimitanteFinalComent').val();
        let Observaciones = $('#Observaciones').val();

        let textoConfirmación = mensajeConfirmar;

        if (confirm(textoConfirmación)) {

            let datos = {
                Equicodi: Equicodi,
                IdTransformador: IdTransformador,
                IdProyecto: IdProyecto,
                Fecha: Fecha,
                D1IdCelda: D1IdCelda,
                D1CapacidadOnanMva: D1CapacidadOnanMva,
                D1CapacidadOnanMvaComent: D1CapacidadOnanMvaComent,
                D1CapacidadOnafMva: D1CapacidadOnafMva,
                D1CapacidadOnafMvaComent: D1CapacidadOnafMvaComent,
                D1CapacidadMva: D1CapacidadMva,
                D1CapacidadMvaComent: D1CapacidadMvaComent,
                D1CapacidadA: D1CapacidadA,
                D1CapacidadAComent: D1CapacidadAComent,
                D1PosicionTcA: D1PosicionTcA,
                D1PosicionPickUpA: D1PosicionPickUpA,
                D1CapacidadTransmisionA: D1CapacidadTransmisionA,
                D1CapacidadTransmisionAComent: D1CapacidadTransmisionAComent,
                D1CapacidadTransmisionMva: D1CapacidadTransmisionMva,
                D1CapacidadTransmisionMvaComent: D1CapacidadTransmisionMvaComent,
                D1FactorLimitanteCalc: D1FactorLimitanteCalc,
                D1FactorLimitanteCalcComent: D1FactorLimitanteCalcComent,
                D1FactorLimitanteFinal: D1FactorLimitanteFinal,
                D1FactorLimitanteFinalComent: D1FactorLimitanteFinalComent,
                D2IdCelda: D2IdCelda,
                D2CapacidadOnanMva: D2CapacidadOnanMva,
                D2CapacidadOnanMvaComent: D2CapacidadOnanMvaComent,
                D2CapacidadOnafMva: D2CapacidadOnafMva,
                D2CapacidadOnafMvaComent: D2CapacidadOnafMvaComent,
                D2CapacidadMva: D2CapacidadMva,
                D2CapacidadMvaComent: D2CapacidadMvaComent,
                D2CapacidadA: D2CapacidadA,
                D2CapacidadAComent: D2CapacidadAComent,
                D2PosicionTcA: D2PosicionTcA,
                D2PosicionPickUpA: D2PosicionPickUpA,
                D2CapacidadTransmisionA: D2CapacidadTransmisionA,
                D2CapacidadTransmisionAComent: D2CapacidadTransmisionAComent,
                D2CapacidadTransmisionMva: D2CapacidadTransmisionMva,
                D2CapacidadTransmisionMvaComent: D2CapacidadTransmisionMvaComent,
                D2FactorLimitanteCalc: D2FactorLimitanteCalc,
                D2FactorLimitanteCalcComent: D2FactorLimitanteCalcComent,
                D2FactorLimitanteFinal: D2FactorLimitanteFinal,
                D2FactorLimitanteFinalComent: D2FactorLimitanteFinalComent,
                D3IdCelda: D3IdCelda,
                D3CapacidadOnanMva: D3CapacidadOnanMva,
                D3CapacidadOnanMvaComent: D3CapacidadOnanMvaComent,
                D3CapacidadOnafMva: D3CapacidadOnafMva,
                D3CapacidadOnafMvaComent: D3CapacidadOnafMvaComent,
                D3CapacidadMva: D3CapacidadMva,
                D3CapacidadMvaComent: D3CapacidadMvaComent,
                D3CapacidadA: D3CapacidadA,
                D3CapacidadAComent: D3CapacidadAComent,
                D3PosicionTcA: D3PosicionTcA,
                D3PosicionPickUpA: D3PosicionPickUpA,
                D3CapacidadTransmisionA: D3CapacidadTransmisionA,
                D3CapacidadTransmisionAComent: D3CapacidadTransmisionAComent,
                D3CapacidadTransmisionMva: D3CapacidadTransmisionMva,
                D3CapacidadTransmisionMvaComent: D3CapacidadTransmisionMvaComent,
                D3FactorLimitanteCalc: D3FactorLimitanteCalc,
                D3FactorLimitanteCalcComent: D3FactorLimitanteCalcComent,
                D3FactorLimitanteFinal: D3FactorLimitanteFinal,
                D3FactorLimitanteFinalComent: D3FactorLimitanteFinalComent,
                D4IdCelda: D4IdCelda,
                D4CapacidadOnanMva: D4CapacidadOnanMva,
                D4CapacidadOnanMvaComent: D4CapacidadOnanMvaComent,
                D4CapacidadOnafMva: D4CapacidadOnafMva,
                D4CapacidadOnafMvaComent: D4CapacidadOnafMvaComent,
                D4CapacidadMva: D4CapacidadMva,
                D4CapacidadMvaComent: D4CapacidadMvaComent,
                D4CapacidadA: D4CapacidadA,
                D4CapacidadAComent: D4CapacidadAComent,
                D4PosicionTcA: D4PosicionTcA,
                D4PosicionPickUpA: D4PosicionPickUpA,
                D4CapacidadTransmisionA: D4CapacidadTransmisionA,
                D4CapacidadTransmisionAComent: D4CapacidadTransmisionAComent,
                D4CapacidadTransmisionMva: D4CapacidadTransmisionMva,
                D4CapacidadTransmisionMvaComent: D4CapacidadTransmisionMvaComent,
                D4FactorLimitanteCalc: D4FactorLimitanteCalc,
                D4FactorLimitanteCalcComent: D4FactorLimitanteCalcComent,
                D4FactorLimitanteFinal: D4FactorLimitanteFinal,
                D4FactorLimitanteFinalComent: D4FactorLimitanteFinalComent,
                Observaciones: Observaciones

            };

            $.ajax({
                type: 'POST',
                url: controlador + 'GuardarTransformador',
                dataType: 'json',
                data: datos,
                cache: false,
                success: function (resultado) {
                    if (resultado == "") {
                        mostrarMensaje('Se ha guardado el registro del Transformador correctamente.', 'exito');
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

    const famcodi = $("#FamCodigo").val();     

    let repetido = false;
    let valores = [];
    switch (famcodi) {
        case "9": {
            const ids = ['D1IdCelda', 'D2IdCelda'];
            ids.forEach(function (id) {
                let val = obtenerValorMultiselect(id);

                if (val == '0' || val =='') {                   
                    return;
                }

                if (val && valores.includes(val)) {
                    repetido = true;
                }
                valores.push(val);
            });

            break;
        }
        case "10": {
            const ids = ['D1IdCelda', 'D2IdCelda', 'D3IdCelda'];
            ids.forEach(function (id) {
                let val = obtenerValorMultiselect(id);

                if (val == '0' || val == '') {
                    return;
                }

                if (val && valores.includes(val)) {
                    repetido = true;
                }
                valores.push(val);
            });

            break;
        }
        case "47": {
            const ids = ['D1IdCelda', 'D2IdCelda', 'D3IdCelda', 'D4IdCelda'];
            ids.forEach(function (id) {
                let val = obtenerValorMultiselect(id);

                if (val == '0' || val == '') {
                    return;
                }

                if (val && valores.includes(val)) {
                    repetido = true;
                }
                valores.push(val);
            });

            break;
        }
    }

    if (repetido) {
        alert("Los valores seleccionados en las celdas deben ser diferentes.");
        return false; 
    }

    return true;
}

// Función auxiliar para mostrar errores y manejar la validación
function mostrarErroresVal(campos) {
    if (campos.length > 0) {
        mensajeValidador(campos);
        return false;
    }
    return true;
}

// Función auxiliar para validar una celda
function validarCelda(id, numero, campos) {
    if ($(`#${id}`).val() === '0') {
        campos.push(`Devanado ${numero}/Celda`);
    }
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

function mostrarDevanado() {
    const famcodi = $("#FamCodigo").val();
    if (famcodi == "9") {
        $("#table_devanado2").show();
        $("#table_devanado3").hide();
        $("#table_devanado4").hide();
    } else if (famcodi == "10") {
        $("#table_devanado2").show();
        $("#table_devanado3").show();
        $("#table_devanado4").hide();
    } else if (famcodi == "47") {
        $("#table_devanado2").show();
        $("#table_devanado3").show();
        $("#table_devanado4").show();
    } else {
        $("#table_devanado1").show();
        $("#table_devanado2").hide();
        $("#table_devanado3").hide();
        $("#table_devanado4").hide();
    }
}

let consultarCelda = function (IdCelda) {
    setTimeout(function () {
        $.ajax({
            type: 'POST',
            url: controlador + 'ConsultaCelda',
            data: { IdCelda: IdCelda },
            success: function (evt) {
                $('#mensaje').css("display", "none");

                if (evt) {
                    $('#D1PosicionTcA').val(evt.PosicionNucleoTc);
                    $('#D1PickUp').val(evt.PickUp);
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
                    $('#D2PosicionTcA').val(evt.PosicionNucleoTc);
                    $('#D2PickUp').val(evt.PickUp);
                }
            },
            error: function () {
                mostrarMensaje('Ha ocurrido un error.', 'error');
            }
        });
    }, 100);
};

let consultarCelda3 = function (IdCelda) {
    setTimeout(function () {
        $.ajax({
            type: 'POST',
            url: controlador + 'ConsultaCelda',
            data: { IdCelda: IdCelda },
            success: function (evt) {
                $('#mensaje').css("display", "none");

                if (evt) {
                    $('#D3PosicionTcA').val(evt.PosicionNucleoTc);
                    $('#D3PickUp').val(evt.PickUp);
                }
            },
            error: function () {
                mostrarMensaje('Ha ocurrido un error.', 'error');
            }
        });
    }, 100);
};

let consultarCelda4 = function (IdCelda) {
    setTimeout(function () {
        $.ajax({
            type: 'POST',
            url: controlador + 'ConsultaCelda',
            data: { IdCelda: IdCelda },
            success: function (evt) {
                $('#mensaje').css("display", "none");

                if (evt) {
                    $('#D4PosicionTcA').val(evt.PosicionNucleoTc);
                    $('#D4PickUp').val(evt.PickUp);
                }
            },
            error: function () {
                mostrarMensaje('Ha ocurrido un error.', 'error');
            }
        });
    }, 100);
};

let calcular = function () {
     

    limpiarMensaje();

    let Equicodi = $('#CodigoId').val();
    let IdTransformador = $('#IdTransformador').val();
    let IdProyecto = $('#IdProyecto').val();

    let D1IdCelda = obtenerValorMultiselect('D1IdCelda');
    let D1CapacidadOnanMva = $('#D1CapacidadOnanMva').val();
    let D1CapacidadOnafMva = $('#D1CapacidadOnafMva').val();
    let D1CapacidadMva = $('#D1CapacidadMva').val();
    let D1CapacidadA = $('#D1CapacidadA').val();
    let D1PosicionTcA = $('#D1PosicionTcA').val();
    let D1PosicionPickUpA = $('#D1PosicionPickUpA').val();
    let D1CapacidadTransmisionA = $('#D1CapacidadTransmisionA').val();
    let D1CapacidadTransmisionMva = $('#D1CapacidadTransmisionMva').val();
    let D1FactorLimitanteCalc = $('#D1FactorLimitanteCalc').val();
    let D1FactorLimitanteFinal = $('#D1FactorLimitanteFinal').val();

    let D2IdCelda = obtenerValorMultiselect('D2IdCelda');
    let D2CapacidadOnanMva = $('#D2CapacidadOnanMva').val();
    let D2CapacidadOnafMva = $('#D2CapacidadOnafMva').val();
    let D2CapacidadMva = $('#D2CapacidadMva').val();
    let D2CapacidadA = $('#D2CapacidadA').val();
    let D2PosicionTcA = $('#D2PosicionTcA').val();
    let D2PosicionPickUpA = $('#D2PosicionPickUpA').val();
    let D2CapacidadTransmisionA = $('#D2CapacidadTransmisionA').val();
    let D2CapacidadTransmisionMva = $('#D2CapacidadTransmisionMva').val();
    let D2FactorLimitanteCalc = $('#D2FactorLimitanteCalc').val();
    let D2FactorLimitanteFinal = $('#D2FactorLimitanteFinal').val();

    let D3IdCelda = obtenerValorMultiselect('D3IdCelda');
    let D3CapacidadOnanMva = $('#D3CapacidadOnanMva').val();
    let D3CapacidadOnafMva = $('#D3CapacidadOnafMva').val();
    let D3CapacidadMva = $('#D3CapacidadMva').val();
    let D3CapacidadA = $('#D3CapacidadA').val();
    let D3PosicionTcA = $('#D3PosicionTcA').val();
    let D3PosicionPickUpA = $('#D3PosicionPickUpA').val();
    let D3CapacidadTransmisionA = $('#D3CapacidadTransmisionA').val();
    let D3CapacidadTransmisionMva = $('#D3CapacidadTransmisionMva').val();
    let D3FactorLimitanteCalc = $('#D3FactorLimitanteCalc').val();
    let D3FactorLimitanteFinal = $('#D3FactorLimitanteFinal').val();

    let D4IdCelda = obtenerValorMultiselect('D4IdCelda');
    let D4CapacidadOnanMva = $('#D4CapacidadOnanMva').val();
    let D4CapacidadOnafMva = $('#D4CapacidadOnafMva').val();
    let D4CapacidadMva = $('#D4CapacidadMva').val();
    let D4CapacidadA = $('#D4CapacidadA').val();
    let D4PosicionTcA = $('#D4PosicionTcA').val();
    let D4PosicionPickUpA = $('#D4PosicionPickUpA').val();
    let D4CapacidadTransmisionA = $('#D4CapacidadTransmisionA').val();
    let D4CapacidadTransmisionMva = $('#D4CapacidadTransmisionMva').val();
    let D4FactorLimitanteCalc = $('#D4FactorLimitanteCalc').val();
    let D4FactorLimitanteFinal = $('#D4FactorLimitanteFinal').val();

    let FamCodigo = $('#FamCodigo').val();

    let D1Tension = $('#D1Tension').val();
    let D2Tension = $('#D2Tension').val();
    let D3Tension = $('#D3Tension').val();
    let D4Tension = $('#D4Tension').val();


    let datos = {
        Equicodi: Equicodi,
        IdTransformador: IdTransformador,
        IdProyecto: IdProyecto,

        D1IdCelda: D1IdCelda,
        D1CapacidadOnanMva: D1CapacidadOnanMva,
        D1CapacidadOnafMva: D1CapacidadOnafMva,
        D1CapacidadMva: D1CapacidadMva,
        D1CapacidadA: D1CapacidadA,
        D1PosicionTcA: D1PosicionTcA,
        D1PosicionPickUpA: D1PosicionPickUpA,
        D1CapacidadTransmisionA: D1CapacidadTransmisionA,
        D1CapacidadTransmisionMva: D1CapacidadTransmisionMva,
        D1FactorLimitanteCalc: D1FactorLimitanteCalc,
        D1FactorLimitanteFinal: D1FactorLimitanteFinal,

        D2IdCelda: D2IdCelda,
        D2CapacidadOnanMva: D2CapacidadOnanMva,
        D2CapacidadOnafMva: D2CapacidadOnafMva,
        D2CapacidadMva: D2CapacidadMva,
        D2CapacidadA: D2CapacidadA,
        D2PosicionTcA: D2PosicionTcA,
        D2PosicionPickUpA: D2PosicionPickUpA,
        D2CapacidadTransmisionA: D2CapacidadTransmisionA,
        D2CapacidadTransmisionMva: D2CapacidadTransmisionMva,
        D2FactorLimitanteCalc: D2FactorLimitanteCalc,
        D2FactorLimitanteFinal: D2FactorLimitanteFinal,

        D3IdCelda: D3IdCelda,
        D3CapacidadOnanMva: D3CapacidadOnanMva,
        D3CapacidadOnafMva: D3CapacidadOnafMva,
        D3CapacidadMva: D3CapacidadMva,
        D3CapacidadA: D3CapacidadA,
        D3PosicionTcA: D3PosicionTcA,
        D3PosicionPickUpA: D3PosicionPickUpA,
        D3CapacidadTransmisionA: D3CapacidadTransmisionA,
        D3CapacidadTransmisionMva: D3CapacidadTransmisionMva,
        D3FactorLimitanteCalc: D3FactorLimitanteCalc,
        D3FactorLimitanteFinal: D3FactorLimitanteFinal,

        D4IdCelda: D4IdCelda,
        D4CapacidadOnanMva: D4CapacidadOnanMva,
        D4CapacidadOnafMva: D4CapacidadOnafMva,
        D4CapacidadMva: D4CapacidadMva,
        D4CapacidadA: D4CapacidadA,
        D4PosicionTcA: D4PosicionTcA,
        D4PosicionPickUpA: D4PosicionPickUpA,
        D4CapacidadTransmisionA: D4CapacidadTransmisionA,
        D4CapacidadTransmisionMva: D4CapacidadTransmisionMva,
        D4FactorLimitanteCalc: D4FactorLimitanteCalc,
        D4FactorLimitanteFinal: D4FactorLimitanteFinal,

        FamCodigo: FamCodigo,
        D1Tension: D1Tension,
        D2Tension: D2Tension,
        D3Tension: D3Tension,
        D4Tension: D4Tension

    };

    $.ajax({
        type: 'POST',
        url: controlador + 'CalcularTransformador',
        dataType: 'json',
        data: datos,
        cache: false,
        success: function (resultado) {
            if (resultado.MensajeError == "") {
                mostrarMensaje('Se realizó el cálculo de los valores.', 'exito');

                $('#D1CapacidadA').val(resultado.D1CapacidadA);
                $('#D1CapacidadMva').val(resultado.D1CapacidadMva);
                $('#D1CapacidadTransmisionA').val(resultado.D1CapacidadTransmisionA);
                $('#D1CapacidadTransmisionMva').val(resultado.D1CapacidadTransmisionMva);
                $('#D1FactorLimitanteCalc').val(resultado.D1FactorLimitanteCalc);
                $('#D1FactorLimitanteFinal').val(resultado.D1FactorLimitanteFinal);

                $('#D2CapacidadA').val(resultado.D2CapacidadA);
                $('#D2CapacidadMva').val(resultado.D2CapacidadMva);
                $('#D2CapacidadTransmisionA').val(resultado.D2CapacidadTransmisionA);
                $('#D2CapacidadTransmisionMva').val(resultado.D2CapacidadTransmisionMva);
                $('#D2FactorLimitanteCalc').val(resultado.D2FactorLimitanteCalc);
                $('#D2FactorLimitanteFinal').val(resultado.D2FactorLimitanteFinal);

                $('#D3CapacidadA').val(resultado.D3CapacidadA);
                $('#D3CapacidadMva').val(resultado.D3CapacidadMva);
                $('#D3CapacidadTransmisionA').val(resultado.D3CapacidadTransmisionA);
                $('#D3CapacidadTransmisionMva').val(resultado.D3CapacidadTransmisionMva);
                $('#D3FactorLimitanteCalc').val(resultado.D3FactorLimitanteCalc);
                $('#D3FactorLimitanteFinal').val(resultado.D3FactorLimitanteFinal);

                $('#D4CapacidadA').val(resultado.D4CapacidadA);
                $('#D4CapacidadMva').val(resultado.D4CapacidadMva);
                $('#D4CapacidadTransmisionA').val(resultado.D4CapacidadTransmisionA);
                $('#D4CapacidadTransmisionMva').val(resultado.D4CapacidadTransmisionMva);
                $('#D4FactorLimitanteCalc').val(resultado.D4FactorLimitanteCalc);
                $('#D4FactorLimitanteFinal').val(resultado.D4FactorLimitanteFinal);

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
        mensajeConfirmar = "¿Está seguro de modificar el registro del transformador?";
    } else if (acc == "Incluir") {
        mensajeConfirmar = "¿Está seguro de guardar el registro del transformador al proyecto?";
    }

    limpiarMensaje();
    if (validarGuardar()) {
        let Equicodi = $('#CodigoId').val();
        let IdTransformador = $('#IdTransformador').val();
        let IdProyecto = $('#IdProyecto').val();

        let D1IdCelda = obtenerValorMultiselect('D1IdCelda');
        let D1CapacidadOnanMva = $('#D1CapacidadOnanMva').val();
        let D1CapacidadOnafMva = $('#D1CapacidadOnafMva').val();
        let D1CapacidadMva = $('#D1CapacidadMva').val();
        let D1CapacidadA = $('#D1CapacidadA').val();
        let D1PosicionTcA = $('#D1PosicionTcA').val();
        let D1PosicionPickUpA = $('#D1PosicionPickUpA').val();
        let D1CapacidadTransmisionA = $('#D1CapacidadTransmisionA').val();
        let D1CapacidadTransmisionMva = $('#D1CapacidadTransmisionMva').val();
        let D1FactorLimitanteCalc = $('#D1FactorLimitanteCalc').val();
        let D1FactorLimitanteFinal = $('#D1FactorLimitanteFinal').val();

        let D2IdCelda = obtenerValorMultiselect('D2IdCelda');
        let D2CapacidadOnanMva = $('#D2CapacidadOnanMva').val();
        let D2CapacidadOnafMva = $('#D2CapacidadOnafMva').val();
        let D2CapacidadMva = $('#D2CapacidadMva').val();
        let D2CapacidadA = $('#D2CapacidadA').val();
        let D2PosicionTcA = $('#D2PosicionTcA').val();
        let D2PosicionPickUpA = $('#D2PosicionPickUpA').val();
        let D2CapacidadTransmisionA = $('#D2CapacidadTransmisionA').val();
        let D2CapacidadTransmisionMva = $('#D2CapacidadTransmisionMva').val();
        let D2FactorLimitanteCalc = $('#D2FactorLimitanteCalc').val();
        let D2FactorLimitanteFinal = $('#D2FactorLimitanteFinal').val();

        let D3IdCelda = obtenerValorMultiselect('D3IdCelda');
        let D3CapacidadOnanMva = $('#D3CapacidadOnanMva').val();
        let D3CapacidadOnafMva = $('#D3CapacidadOnafMva').val();
        let D3CapacidadMva = $('#D3CapacidadMva').val();
        let D3CapacidadA = $('#D3CapacidadA').val();
        let D3PosicionTcA = $('#D3PosicionTcA').val();
        let D3PosicionPickUpA = $('#D3PosicionPickUpA').val();
        let D3CapacidadTransmisionA = $('#D3CapacidadTransmisionA').val();
        let D3CapacidadTransmisionMva = $('#D3CapacidadTransmisionMva').val();
        let D3FactorLimitanteCalc = $('#D3FactorLimitanteCalc').val();
        let D3FactorLimitanteFinal = $('#D3FactorLimitanteFinal').val();

        let D4IdCelda = obtenerValorMultiselect('D4IdCelda');
        let D4CapacidadOnanMva = $('#D4CapacidadOnanMva').val();
        let D4CapacidadOnafMva = $('#D4CapacidadOnafMva').val();
        let D4CapacidadMva = $('#D4CapacidadMva').val();
        let D4CapacidadA = $('#D4CapacidadA').val();
        let D4PosicionTcA = $('#D4PosicionTcA').val();
        let D4PosicionPickUpA = $('#D4PosicionPickUpA').val();
        let D4CapacidadTransmisionA = $('#D4CapacidadTransmisionA').val();
        let D4CapacidadTransmisionMva = $('#D4CapacidadTransmisionMva').val();
        let D4FactorLimitanteCalc = $('#D4FactorLimitanteCalc').val();
        let D4FactorLimitanteFinal = $('#D4FactorLimitanteFinal').val();

        let FamCodigo = $('#FamCodigo').val();

        let D1Tension = $('#D1Tension').val();
        let D2Tension = $('#D2Tension').val();
        let D3Tension = $('#D3Tension').val();
        let D4Tension = $('#D4Tension').val();

        let textoConfirmación = mensajeConfirmar;

        if (confirm(textoConfirmación)) {

            let datos = {
                Equicodi: Equicodi,
                IdTransformador: IdTransformador,
                IdProyecto: IdProyecto,

                D1IdCelda: D1IdCelda,
                D1CapacidadOnanMva: D1CapacidadOnanMva,
                D1CapacidadOnafMva: D1CapacidadOnafMva,
                D1CapacidadMva: D1CapacidadMva,
                D1CapacidadA: D1CapacidadA,
                D1PosicionTcA: D1PosicionTcA,
                D1PosicionPickUpA: D1PosicionPickUpA,
                D1CapacidadTransmisionA: D1CapacidadTransmisionA,
                D1CapacidadTransmisionMva: D1CapacidadTransmisionMva,
                D1FactorLimitanteCalc: D1FactorLimitanteCalc,
                D1FactorLimitanteFinal: D1FactorLimitanteFinal,

                D2IdCelda: D2IdCelda,
                D2CapacidadOnanMva: D2CapacidadOnanMva,
                D2CapacidadOnafMva: D2CapacidadOnafMva,
                D2CapacidadMva: D2CapacidadMva,
                D2CapacidadA: D2CapacidadA,
                D2PosicionTcA: D2PosicionTcA,
                D2PosicionPickUpA: D2PosicionPickUpA,
                D2CapacidadTransmisionA: D2CapacidadTransmisionA,
                D2CapacidadTransmisionMva: D2CapacidadTransmisionMva,
                D2FactorLimitanteCalc: D2FactorLimitanteCalc,
                D2FactorLimitanteFinal: D2FactorLimitanteFinal,

                D3IdCelda: D3IdCelda,
                D3CapacidadOnanMva: D3CapacidadOnanMva,
                D3CapacidadOnafMva: D3CapacidadOnafMva,
                D3CapacidadMva: D3CapacidadMva,
                D3CapacidadA: D3CapacidadA,
                D3PosicionTcA: D3PosicionTcA,
                D3PosicionPickUpA: D3PosicionPickUpA,
                D3CapacidadTransmisionA: D3CapacidadTransmisionA,
                D3CapacidadTransmisionMva: D3CapacidadTransmisionMva,
                D3FactorLimitanteCalc: D3FactorLimitanteCalc,
                D3FactorLimitanteFinal: D3FactorLimitanteFinal,

                D4IdCelda: D4IdCelda,
                D4CapacidadOnanMva: D4CapacidadOnanMva,
                D4CapacidadOnafMva: D4CapacidadOnafMva,
                D4CapacidadMva: D4CapacidadMva,
                D4CapacidadA: D4CapacidadA,
                D4PosicionTcA: D4PosicionTcA,
                D4PosicionPickUpA: D4PosicionPickUpA,
                D4CapacidadTransmisionA: D4CapacidadTransmisionA,
                D4CapacidadTransmisionMva: D4CapacidadTransmisionMva,
                D4FactorLimitanteCalc: D4FactorLimitanteCalc,
                D4FactorLimitanteFinal: D4FactorLimitanteFinal,

                FamCodigo: FamCodigo,
                D1Tension: D1Tension,
                D2Tension: D2Tension,
                D3Tension: D3Tension,
                D4Tension: D4Tension

            };


            $.ajax({
                type: 'POST',
                url: controlador + 'CalcularTransformador',
                dataType: 'json',
                data: datos,
                cache: false,
                success: function (resultado) {
                    if (resultado.MensajeError == "") {
                        mostrarMensaje('Se realizó el cálculo de los valores.', 'exito');

                        $('#D1CapacidadA').val(resultado.D1CapacidadA);
                        $('#D1CapacidadMva').val(resultado.D1CapacidadMva);
                        $('#D1CapacidadTransmisionA').val(resultado.D1CapacidadTransmisionA);
                        $('#D1CapacidadTransmisionMva').val(resultado.D1CapacidadTransmisionMva);
                        $('#D1FactorLimitanteCalc').val(resultado.D1FactorLimitanteCalc);
                        $('#D1FactorLimitanteFinal').val(resultado.D1FactorLimitanteFinal);

                        $('#D2CapacidadA').val(resultado.D2CapacidadA);
                        $('#D2CapacidadMva').val(resultado.D2CapacidadMva);
                        $('#D2CapacidadTransmisionA').val(resultado.D2CapacidadTransmisionA);
                        $('#D2CapacidadTransmisionMva').val(resultado.D2CapacidadTransmisionMva);
                        $('#D2FactorLimitanteCalc').val(resultado.D2FactorLimitanteCalc);
                        $('#D2FactorLimitanteFinal').val(resultado.D2FactorLimitanteFinal);

                        $('#D3CapacidadA').val(resultado.D3CapacidadA);
                        $('#D3CapacidadMva').val(resultado.D3CapacidadMva);
                        $('#D3CapacidadTransmisionA').val(resultado.D3CapacidadTransmisionA);
                        $('#D3CapacidadTransmisionMva').val(resultado.D3CapacidadTransmisionMva);
                        $('#D3FactorLimitanteCalc').val(resultado.D3FactorLimitanteCalc);
                        $('#D3FactorLimitanteFinal').val(resultado.D3FactorLimitanteFinal);

                        $('#D4CapacidadA').val(resultado.D4CapacidadA);
                        $('#D4CapacidadMva').val(resultado.D4CapacidadMva);
                        $('#D4CapacidadTransmisionA').val(resultado.D4CapacidadTransmisionA);
                        $('#D4CapacidadTransmisionMva').val(resultado.D4CapacidadTransmisionMva);
                        $('#D4FactorLimitanteCalc').val(resultado.D4FactorLimitanteCalc);
                        $('#D4FactorLimitanteFinal').val(resultado.D4FactorLimitanteFinal);

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


    limpiarMensaje();

    let Equicodi = $('#CodigoId').val();
    let IdTransformador = $('#IdTransformador').val();
    let IdProyecto = $('#IdProyecto').val();
    let Fecha = $('#FechaActualizacion').val();

    let D1IdCelda = obtenerValorMultiselect('D1IdCelda');
    let D1CapacidadOnanMva = $('#D1CapacidadOnanMva').val();
    let D1CapacidadOnanMvaComent = $('#D1CapacidadOnanMvaComent').val();
    let D1CapacidadOnafMva = $('#D1CapacidadOnafMva').val();
    let D1CapacidadOnafMvaComent = $('#D1CapacidadOnafMvaComent').val();
    let D1CapacidadMva = $('#D1CapacidadMva').val();
    let D1CapacidadMvaComent = $('#D1CapacidadMvaComent').val();
    let D1CapacidadA = $('#D1CapacidadA').val();
    let D1CapacidadAComent = $('#D1CapacidadAComent').val();
    let D1PosicionTcA = $('#D1PosicionTcA').val();
    let D1PosicionPickUpA = $('#D1PosicionPickUpA').val();
    let D1CapacidadTransmisionA = $('#D1CapacidadTransmisionA').val();
    let D1CapacidadTransmisionAComent = $('#D1CapacidadTransmisionAComent').val();
    let D1CapacidadTransmisionMva = $('#D1CapacidadTransmisionMva').val();
    let D1CapacidadTransmisionMvaComent = $('#D1CapacidadTransmisionMvaComent').val();
    let D1FactorLimitanteCalc = $('#D1FactorLimitanteCalc').val();
    let D1FactorLimitanteCalcComent = $('#D1FactorLimitanteCalcComent').val();
    let D1FactorLimitanteFinal = $('#D1FactorLimitanteFinal').val();
    let D1FactorLimitanteFinalComent = $('#D1FactorLimitanteFinalComent').val();

    let D2IdCelda = obtenerValorMultiselect('D2IdCelda');
    let D2CapacidadOnanMva = $('#D2CapacidadOnanMva').val();
    let D2CapacidadOnanMvaComent = $('#D2CapacidadOnanMvaComent').val();
    let D2CapacidadOnafMva = $('#D2CapacidadOnafMva').val();
    let D2CapacidadOnafMvaComent = $('#D2CapacidadOnafMvaComent').val();
    let D2CapacidadMva = $('#D2CapacidadMva').val();
    let D2CapacidadMvaComent = $('#D2CapacidadMvaComent').val();
    let D2CapacidadA = $('#D2CapacidadA').val();
    let D2CapacidadAComent = $('#D2CapacidadAComent').val();
    let D2PosicionTcA = $('#D2PosicionTcA').val();
    let D2PosicionPickUpA = $('#D2PosicionPickUpA').val();
    let D2CapacidadTransmisionA = $('#D2CapacidadTransmisionA').val();
    let D2CapacidadTransmisionAComent = $('#D2CapacidadTransmisionAComent').val();
    let D2CapacidadTransmisionMva = $('#D2CapacidadTransmisionMva').val();
    let D2CapacidadTransmisionMvaComent = $('#D2CapacidadTransmisionMvaComent').val();
    let D2FactorLimitanteCalc = $('#D2FactorLimitanteCalc').val();
    let D2FactorLimitanteCalcComent = $('#D2FactorLimitanteCalcComent').val();
    let D2FactorLimitanteFinal = $('#D2FactorLimitanteFinal').val();
    let D2FactorLimitanteFinalComent = $('#D2FactorLimitanteFinalComent').val();

    let D3IdCelda = obtenerValorMultiselect('D3IdCelda');
    let D3CapacidadOnanMva = $('#D3CapacidadOnanMva').val();
    let D3CapacidadOnanMvaComent = $('#D3CapacidadOnanMvaComent').val();
    let D3CapacidadOnafMva = $('#D3CapacidadOnafMva').val();
    let D3CapacidadOnafMvaComent = $('#D3CapacidadOnafMvaComent').val();
    let D3CapacidadMva = $('#D3CapacidadMva').val();
    let D3CapacidadMvaComent = $('#D3CapacidadMvaComent').val();
    let D3CapacidadA = $('#D3CapacidadA').val();
    let D3CapacidadAComent = $('#D3CapacidadAComent').val();
    let D3PosicionTcA = $('#D3PosicionTcA').val();
    let D3PosicionPickUpA = $('#D3PosicionPickUpA').val();
    let D3CapacidadTransmisionA = $('#D3CapacidadTransmisionA').val();
    let D3CapacidadTransmisionAComent = $('#D3CapacidadTransmisionAComent').val();
    let D3CapacidadTransmisionMva = $('#D3CapacidadTransmisionMva').val();
    let D3CapacidadTransmisionMvaComent = $('#D3CapacidadTransmisionMvaComent').val();
    let D3FactorLimitanteCalc = $('#D3FactorLimitanteCalc').val();
    let D3FactorLimitanteCalcComent = $('#D3FactorLimitanteCalcComent').val();
    let D3FactorLimitanteFinal = $('#D3FactorLimitanteFinal').val();
    let D3FactorLimitanteFinalComent = $('#D3FactorLimitanteFinalComent').val();

    let D4IdCelda = obtenerValorMultiselect('D4IdCelda');
    let D4CapacidadOnanMva = $('#D4CapacidadOnanMva').val();
    let D4CapacidadOnanMvaComent = $('#D4CapacidadOnanMvaComent').val();
    let D4CapacidadOnafMva = $('#D4CapacidadOnafMva').val();
    let D4CapacidadOnafMvaComent = $('#D4CapacidadOnafMvaComent').val();
    let D4CapacidadMva = $('#D4CapacidadMva').val();
    let D4CapacidadMvaComent = $('#D4CapacidadMvaComent').val();
    let D4CapacidadA = $('#D4CapacidadA').val();
    let D4CapacidadAComent = $('#D4CapacidadAComent').val();
    let D4PosicionTcA = $('#D4PosicionTcA').val();
    let D4PosicionPickUpA = $('#D4PosicionPickUpA').val();
    let D4CapacidadTransmisionA = $('#D4CapacidadTransmisionA').val();
    let D4CapacidadTransmisionAComent = $('#D4CapacidadTransmisionAComent').val();
    let D4CapacidadTransmisionMva = $('#D4CapacidadTransmisionMva').val();
    let D4CapacidadTransmisionMvaComent = $('#D4CapacidadTransmisionMvaComent').val();
    let D4FactorLimitanteCalc = $('#D4FactorLimitanteCalc').val();
    let D4FactorLimitanteCalcComent = $('#D4FactorLimitanteCalcComent').val();
    let D4FactorLimitanteFinal = $('#D4FactorLimitanteFinal').val();
    let D4FactorLimitanteFinalComent = $('#D4FactorLimitanteFinalComent').val();
    let Observaciones = $('#Observaciones').val();


    let datos = {
        Equicodi: Equicodi,
        IdTransformador: IdTransformador,
        IdProyecto: IdProyecto,
        Fecha: Fecha,
        D1IdCelda: D1IdCelda,
        D1CapacidadOnanMva: D1CapacidadOnanMva,
        D1CapacidadOnanMvaComent: D1CapacidadOnanMvaComent,
        D1CapacidadOnafMva: D1CapacidadOnafMva,
        D1CapacidadOnafMvaComent: D1CapacidadOnafMvaComent,
        D1CapacidadMva: D1CapacidadMva,
        D1CapacidadMvaComent: D1CapacidadMvaComent,
        D1CapacidadA: D1CapacidadA,
        D1CapacidadAComent: D1CapacidadAComent,
        D1PosicionTcA: D1PosicionTcA,
        D1PosicionPickUpA: D1PosicionPickUpA,
        D1CapacidadTransmisionA: D1CapacidadTransmisionA,
        D1CapacidadTransmisionAComent: D1CapacidadTransmisionAComent,
        D1CapacidadTransmisionMva: D1CapacidadTransmisionMva,
        D1CapacidadTransmisionMvaComent: D1CapacidadTransmisionMvaComent,
        D1FactorLimitanteCalc: D1FactorLimitanteCalc,
        D1FactorLimitanteCalcComent: D1FactorLimitanteCalcComent,
        D1FactorLimitanteFinal: D1FactorLimitanteFinal,
        D1FactorLimitanteFinalComent: D1FactorLimitanteFinalComent,
        D2IdCelda: D2IdCelda,
        D2CapacidadOnanMva: D2CapacidadOnanMva,
        D2CapacidadOnanMvaComent: D2CapacidadOnanMvaComent,
        D2CapacidadOnafMva: D2CapacidadOnafMva,
        D2CapacidadOnafMvaComent: D2CapacidadOnafMvaComent,
        D2CapacidadMva: D2CapacidadMva,
        D2CapacidadMvaComent: D2CapacidadMvaComent,
        D2CapacidadA: D2CapacidadA,
        D2CapacidadAComent: D2CapacidadAComent,
        D2PosicionTcA: D2PosicionTcA,
        D2PosicionPickUpA: D2PosicionPickUpA,
        D2CapacidadTransmisionA: D2CapacidadTransmisionA,
        D2CapacidadTransmisionAComent: D2CapacidadTransmisionAComent,
        D2CapacidadTransmisionMva: D2CapacidadTransmisionMva,
        D2CapacidadTransmisionMvaComent: D2CapacidadTransmisionMvaComent,
        D2FactorLimitanteCalc: D2FactorLimitanteCalc,
        D2FactorLimitanteCalcComent: D2FactorLimitanteCalcComent,
        D2FactorLimitanteFinal: D2FactorLimitanteFinal,
        D2FactorLimitanteFinalComent: D2FactorLimitanteFinalComent,
        D3IdCelda: D3IdCelda,
        D3CapacidadOnanMva: D3CapacidadOnanMva,
        D3CapacidadOnanMvaComent: D3CapacidadOnanMvaComent,
        D3CapacidadOnafMva: D3CapacidadOnafMva,
        D3CapacidadOnafMvaComent: D3CapacidadOnafMvaComent,
        D3CapacidadMva: D3CapacidadMva,
        D3CapacidadMvaComent: D3CapacidadMvaComent,
        D3CapacidadA: D3CapacidadA,
        D3CapacidadAComent: D3CapacidadAComent,
        D3PosicionTcA: D3PosicionTcA,
        D3PosicionPickUpA: D3PosicionPickUpA,
        D3CapacidadTransmisionA: D3CapacidadTransmisionA,
        D3CapacidadTransmisionAComent: D3CapacidadTransmisionAComent,
        D3CapacidadTransmisionMva: D3CapacidadTransmisionMva,
        D3CapacidadTransmisionMvaComent: D3CapacidadTransmisionMvaComent,
        D3FactorLimitanteCalc: D3FactorLimitanteCalc,
        D3FactorLimitanteCalcComent: D3FactorLimitanteCalcComent,
        D3FactorLimitanteFinal: D3FactorLimitanteFinal,
        D3FactorLimitanteFinalComent: D3FactorLimitanteFinalComent,
        D4IdCelda: D4IdCelda,
        D4CapacidadOnanMva: D4CapacidadOnanMva,
        D4CapacidadOnanMvaComent: D4CapacidadOnanMvaComent,
        D4CapacidadOnafMva: D4CapacidadOnafMva,
        D4CapacidadOnafMvaComent: D4CapacidadOnafMvaComent,
        D4CapacidadMva: D4CapacidadMva,
        D4CapacidadMvaComent: D4CapacidadMvaComent,
        D4CapacidadA: D4CapacidadA,
        D4CapacidadAComent: D4CapacidadAComent,
        D4PosicionTcA: D4PosicionTcA,
        D4PosicionPickUpA: D4PosicionPickUpA,
        D4CapacidadTransmisionA: D4CapacidadTransmisionA,
        D4CapacidadTransmisionAComent: D4CapacidadTransmisionAComent,
        D4CapacidadTransmisionMva: D4CapacidadTransmisionMva,
        D4CapacidadTransmisionMvaComent: D4CapacidadTransmisionMvaComent,
        D4FactorLimitanteCalc: D4FactorLimitanteCalc,
        D4FactorLimitanteCalcComent: D4FactorLimitanteCalcComent,
        D4FactorLimitanteFinal: D4FactorLimitanteFinal,
        D4FactorLimitanteFinalComent: D4FactorLimitanteFinalComent,
        Observaciones: Observaciones

    };


    $.ajax({
        type: 'POST',
        url: controlador + 'GuardarTransformador',
        dataType: 'json',
        data: datos,
        cache: false,
        success: function (resultado) {
            if (resultado == "") {
                mostrarMensaje('Se ha guardado el registro del Transformador correctamente.', 'exito');
            } else
                alert(resultado)

        },
        error: function () {
            mostrarMensaje('Ha ocurrido un error.', 'error');
        }
    });

};