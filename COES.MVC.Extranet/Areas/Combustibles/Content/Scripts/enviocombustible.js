var controlador = siteRoot + 'Combustibles/envio/';

$(function () {
    $('#tab-container').easytabs();

    $('#btnInicio').click(function () {
        regresarPrincipal();
    });

    $('#btnEnviarDatos').click(function () {
        enviarExcelWeb();
    });

    $('#btnMostrarErrores').click(function () {
        mostrarDetalleErrores();
    });

    $(document).on('paste', 'input[type="text"].onlyNumber', function (event) {
        if (!event.originalEvent.clipboardData.getData('Text').match(/^\d+\.\d{0,10}$/)) {
            event.preventDefault();
        }
    });
    mostrarGrilla();
});

function regresarPrincipal() {
    var estadoEnvio = $("#hdIdEstado").val();
    document.location.href = controlador + "Index?carpeta=" + estadoEnvio;
}

function enviarExcelWeb() {
    if (confirm("¿Desea enviar información a COES?  Una vez enviado, se procederá con la revisión por parte de COES.")) {
        if (validarEnvio()) {
            var idEnvio = $("#hfIdEnvio").val();
            var empresa = $("#IdEmpresa").val();
            var equicodi = $("#hdIdEquipo").val();
            var grupocodi = $("#hdIdGrupo").val();
            var combustible = $("#hdIdCombustible").val();
            var idFenergcodi = $("#hdIdFenerg").val();

            MODELO_WEB.ListaErrores = [];
            var dataJson = JSON.stringify(MODELO_WEB);

            $.ajax({
                type: 'POST',
                dataType: 'json',
                url: controlador + "GrabarDatosCombustible",
                data: {
                    idEnvio: idEnvio,
                    idEmpresa: empresa,
                    equicodi: equicodi,
                    grupocodi: grupocodi,
                    idFenergcodi: idFenergcodi,
                    dataJson: dataJson
                },
                beforeSend: function () {
                    //mostrarExito("Enviando Información ..");
                },
                success: function (evt) {
                    if (evt.Resultado != "-1") {
                        if (evt.Resultado == "1") {
                            alert("Se envió la información correctamente");
                            //mostrar pantalla de consolidado de envio

                            if (MODELO_WEB.Estenvcodi == ESTADO_OBSERVADO || MODELO_WEB.Estenvcodi == ESTADO_LEVANTAMIENTO_OBS) {
                                $("#hdIdEstado").val(ESTADO_LEVANTAMIENTO_OBS);
                            }
                            regresarPrincipal();
                        }
                        if (evt.Resultado == "2") {
                            alert("Información es idéntica a la enviada anteriormente, no se realizó ningún cambio");
                        }

                    } else {
                        alert("Ha ocurrido un error: " + evt.Mensaje);
                    }
                },
                error: function (err) {
                    alert("Ha ocurrido un error.");
                }
            });
        }
    }
}