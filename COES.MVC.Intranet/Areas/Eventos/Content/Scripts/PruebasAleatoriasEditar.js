var controladorpa = siteRoot + 'eventos/pruebasaleatorias/';
var mensajeFlujograma = "Debe seleccionar paso previo de flujograma...";

$(function () {



    //1: inicial
    $('#btnSIC2HOP').click(function () {
        if ($('#hfAccion').val() == "1") {
            actualizarPasoInicial("SIC2HOP");
        }
        else {
            alert("No es posible grabar en modo 'Visualizar'");
        }
    });

    //2: actualizarPasoPrevio1Posterior1
    $('#btnHOP2UT30D').click(function () {
        if ($('#hfAccion').val() == "1") {
            actualizarPasoPrevio1Posterior1('HOP2UT30D', 'SIC2HOP', 'UT30D2SORT');
        }
        else {
            alert("No es posible grabar en modo 'Visualizar'");
        }

    });

    $('#btnUT30D2SORT').click(function () {
        if ($('#hfAccion').val() == "1") {
            actualizarPasoPrevio1Posterior1('UT30D2SORT', 'HOP2UT30D', 'SORT2PRUE');
        }

    });

    $('#btnUPRUE2RPRUE').click(function () {
        if ($('#hfAccion').val() == "1") {
            actualizarPasoPrevio1Posterior1('UPRUE2RPRUE', 'GPRUESI2UPRUE', 'RPRUE2OA');
        }

    });

    $('#btnRPRUE2OA').click(function () {

        if ($('#hfAccion').val() != "1") {
            return;
        }

        //formato de correo
        $.ajax({
            type: 'POST',
            url: siteRoot + 'eventos/enviarcorreos/' + "formatocorreo",
            data: {
                id: $('#hfPAFECHA').val(),
                plantilla: 'pruebaaleatoria'
            },
            success: function (result) {

                $('#contenidoEdicion').html(result);
                setTimeout(function () {
                    $('#popupEdicion').bPopup({
                        autoClose: false
                    });

                },
                    50);

                cargaDeArchivo();

                $.ajax({
                    success: function () {

                        iniciarControlTexto('Contenido');
                    }
                });


                $('#btnEnviar').click(function () {
                    var correoValido = validarFormatoCorreo();
                    if (correoValido < 0)
                        return;
                    $.ajax({
                        type: 'POST',
                        url: siteRoot + 'eventos/enviarcorreos/' + "enviarCorreo",
                        dataType: 'json',
                        data: $('#formFormatoCorreo').serialize(),
                        success: function (result) {
                            if (result >= 1) {
                                $('#popupEdicion').bPopup().close();

                            }
                            if (result == -1) {
                                mostrarError();
                            }
                        },
                        error: function () {
                            mostrarError();
                        }
                    });


                });

                actualizarPasoPrevio1Posterior1('RPRUE2OA', 'GPRUESI2UPRUE', 'OA2PRIARR');

                pintarCuadros();

            },
            error: function () {

            }


        });


    });

    $('#btnPABORT2RESPRUE').click(function () {
        if ($('#hfAccion').val() == "1") {
            actualizarPasoPrevio1Posterior1('PABORT2RESPRUE', 'FALLUNIDNO2PABORT', 'RESPRUE2FIN');
        }

    });

    //3: actualizarPasoPrevio1Posterior2(paso, pasoPrevio, pasoPosterior1, pasoPosterior2)
    $('#btnSORT2PRUE').click(function () {
        if ($('#hfAccion').val() == "1") {
            actualizarPasoPrevio1Posterior2('SORT2PRUE', 'UT30D2SORT', 'PRUENO2PA', 'PRUESI2GPRUE');
        }

    });


    $('#btnOA2PRIARR').click(function () {
        if ($('#hfAccion').val() == "1") {
            actualizarPasoPrevio1Posterior2('OA2PRIARR', 'RPRUE2OA', 'PRIARRSI2EXIT', 'PRIARRNO2REARR');
        }

    });


    //4: actualizarPasoPrevio1Posterior2(paso, pasoPrevio, pasoPosterior1, pasoPosterior2)
    $('#btnPRUENO2PA').click(function () {
        if ($('#hfAccion').val() == "1") {
            actualizarPasoPrevio1Posterior2('PRUENO2PA', 'SORT2PRUE', 'PA2FIN', 'PRUESI2GPRUE');
        }

    });

    $('#btnGPRUESI2UPRUE').click(function () {
        if ($('#hfAccion').val() == "1") {
            actualizarPasoPrevio1Posterior2('GPRUESI2UPRUE', 'PRUESI2GPRUE', 'UPRUE2RPRUE', 'GPRUENO2NPRUE');
        }

    });

    $('#btnREARRNO2NOEXIT').click(function () {
        if ($('#hfAccion').val() == "1") {
            actualizarPasoPrevio1Posterior2('REARRNO2NOEXIT', 'PRIARRNO2REARR', 'NOEXIT2RESRESLT', 'REARRSI2SEGARR');
        }

    });

    $('#btnSEGARRNO2NOEXIT').click(function () {
        if ($('#hfAccion').val() == "1") {
            actualizarPasoPrevio1Posterior2('SEGARRNO2NOEXIT', 'REARRSI2SEGARR', 'NOEXIT2RESRESLT', 'SEGARRSI2EXIT');
        }

    });

    $('#btnFALLUNIDSI2NOEXIT').click(function () {
        if ($('#hfAccion').val() == "1") {
            actualizarPasoPrevio1Posterior2('FALLUNIDSI2NOEXIT', 'EXITNO2FALLUNID', 'NOEXIT2RESRESLT', 'FALLUNIDNO2PABORT');
        }

    });

    $('#btnEXITSI2RESPRUE').click(function () {
        if ($('#hfAccion').val() == "1") {
            //actualizarPasoPrevio1Posterior2('EXITSI2RESPRUE', 'PRIARRSI2EXIT', 'EXITSI2RESPRUE', 'EXITNO2FALLUNID');
            //actualizarPasoPrevio1Posterior2('EXITSI2RESPRUE', 'PRIARRSI2EXIT', 'EXITSI2RESPRUE', 'EXITNO2FALLUNID');
            actualizarPasoPrevio2Posterior2Condicion1('EXITSI2RESPRUE', 'SEGARRSI2EXIT', 'PRIARRSI2EXIT', 'EXITSI2RESPRUE', 'EXITNO2FALLUNID', 'EXITNO2FALLUNID');            
        }
    });

    $('#btnFALLUNIDNO2PABORT').click(function () {
        if ($('#hfAccion').val() == "1") {
            actualizarPasoPrevio1Posterior2('FALLUNIDNO2PABORT', 'EXITNO2FALLUNID', 'PABORT2RESPRUE', 'FALLUNIDSI2NOEXIT');
        }

    });

    $('#btnPAVERIFDATOSING').click(function () {
        var txt;

        if (($('#hfAccion').val() == "1") && ($('#hfRESPRUE2FIN').val() == "S") && ($('#hfPAVERIFDATOSING').val() == "N")) {

            var r = confirm("¿ Confirma que verificó Horas de operación y Datos de Ejecutado ?");
            if (r == true) {
                //actualizarPasoPrevio1Posterior1('PAVERIFDATOSING', 'SIC2HOP', 'UT30D2SORT');
                actualizarPasoPrevio1Final('PAVERIFDATOSING', 'RESPRUE2FIN', 'PruebaExitosaFin');
            }
        }

    });
    

    //5: actualizarPasoPrevio1Final(paso, pasoPrevio, pasoFinal)
    $('#btnPA2FIN').click(function () {

        if ($('#hfAccion').val() != "1") {
            return;
        }



        //formato de correo
        $.ajax({
            type: 'POST',
            url: siteRoot + 'eventos/enviarcorreos/' + "formatocorreo",
            data: {
                id: $('#hfPAFECHA').val(),
                plantilla: 'pruebaaleatoria'
            },
            success: function (result) {


                $('#contenidoEdicion').html(result);
                setTimeout(function () {
                    $('#popupEdicion').bPopup({
                        autoClose: false
                    });

                },
                    50);
                cargaDeArchivo();

                $.ajax({
                    success: function () {

                        iniciarControlTexto('Contenido');
                    }
                });


                $('#btnEnviar').click(function () {
                    var correoValido = validarFormatoCorreo();
                    if (correoValido < 0)
                        return;
                    $.ajax({
                        type: 'POST',
                        url: siteRoot + 'eventos/enviarcorreos/' + "enviarCorreo",
                        dataType: 'json',
                        data: $('#formFormatoCorreo').serialize(),
                        success: function (result) {
                            if (result >= 1) {
                                $('#popupEdicion').bPopup().close();

                            }
                            if (result == -1) {
                                mostrarError();
                            }
                        },
                        error: function () {
                            mostrarError();
                        }
                    });
                });


                actualizarPasoPrevio1Final('PA2FIN', 'PRUENO2PA', 'DiaDePrueba');

                pintarCuadros();

            },
            error: function () {

            }


        });




    });


    $('#btnNPRUE2FIN').click(function () {


        if ($('#hfAccion').val() != "1") {
            return;
        }

        //formato de correo
        $.ajax({
            type: 'POST',
            url: siteRoot + 'eventos/enviarcorreos/' + "formatocorreo",
            data: {
                id: $('#hfPAFECHA').val(),
                plantilla: 'pruebaaleatoria'
            },
            success: function (result) {


                $('#contenidoEdicion').html(result);
                setTimeout(function () {
                    $('#popupEdicion').bPopup({
                        autoClose: false
                    });

                },
                    50);
                cargaDeArchivo();

                $.ajax({
                    success: function () {

                        iniciarControlTexto('Contenido');
                    }
                });

                $('#btnEnviar').click(function () {
                    var correoValido = validarFormatoCorreo();
                    if (correoValido < 0)
                        return;
                    $.ajax({
                        type: 'POST',
                        url: siteRoot + 'eventos/enviarcorreos/' + "enviarCorreo",
                        dataType: 'json',
                        data: $('#formFormatoCorreo').serialize(),
                        success: function (result) {
                            if (result >= 1) {
                                $('#popupEdicion').bPopup().close();

                            }
                            if (result == -1) {
                                mostrarError();
                            }
                        },
                        error: function () {
                            mostrarError();
                        }
                    });

                });

                actualizarPasoPrevio1Final('NPRUE2FIN', 'GPRUENO2NPRUE', 'GruposPrueba');

                pintarCuadros();


            },
            error: function () {

            }


        });

    });


    $('#btnRESRESLT2FIN').click(function () {
        if ($('#hfAccion').val() == "1") {
            actualizarPasoPrevio1Final('RESRESLT2FIN', 'NOEXIT2RESRESLT', 'PruebaExitosaFin');
        }

    });


    //6: actualizarPasoPrevio1Posterior2Condicion1(paso, pasoPrevio, pasoPosterior1, pasoPosterior2, pasoCondicion)
    $('#btnPRUESI2GPRUE').click(function () {
        if ($('#hfAccion').val() == "1") {
            actualizarPasoPrevio1Posterior2Condicion1('PRUESI2GPRUE', 'SORT2PRUE', 'GPRUENO2NPRUE', 'GPRUESI2UPRUE', 'PRUENO2PA');
        }

    });


    $('#btnGPRUENO2NPRUE').click(function () {
        if ($('#hfAccion').val() == "1") {
            actualizarPasoPrevio1Posterior2Condicion1('GPRUENO2NPRUE', 'PRUESI2GPRUE', 'NPRUE2FIN', 'UPRUE2RPRUE', 'GPRUESI2UPRUE');
        }

    });


    $('#btnPRIARRSI2EXIT').click(function () {
        if ($('#hfAccion').val() == "1") {
            actualizarPasoPrevio1Posterior2Condicion1('PRIARRSI2EXIT', 'OA2PRIARR', 'EXITNO2FALLUNID', 'EXITSI2RESPRUE', 'PRIARRNO2REARR');
        }

    });


    $('#btnPRIARRNO2REARR').click(function () {
        if ($('#hfAccion').val() == "1") {
            actualizarPasoPrevio1Posterior2Condicion1('PRIARRNO2REARR', 'OA2PRIARR', 'REARRSI2SEGARR', 'REARRNO2NOEXIT', 'PRIARRSI2EXIT');
        }
    });


    $('#btnREARRSI2SEGARR').click(function () {
        if ($('#hfAccion').val() == "1") {
            actualizarPasoPrevio1Posterior2('REARRSI2SEGARR', 'PRIARRNO2REARR', 'SEGARRSI2EXIT', 'SEGARRNO2NOEXIT');
        }
    });


    $('#btnEXITNO2FALLUNID').click(function () {
        if ($('#hfAccion').val() == "1") {
            //actualizarPasoPrevio1Posterior2('EXITNO2FALLUNID', 'PRIARRSI2EXIT', 'FALLUNIDSI2NOEXIT', 'FALLUNIDNO2PABORT');
            //actualizarPasoPrevio1Posterior2Condicion1('EXITNO2FALLUNID', 'SEGARRSI2EXIT', 'FALLUNIDSI2NOEXIT', 'FALLUNIDNO2PABORT', 'EXITSI2RESPRUE');
            actualizarPasoPrevio1Posterior2Condicion1DobleCondicion('EXITNO2FALLUNID', 'SEGARRSI2EXIT', 'PRIARRSI2EXIT', 'FALLUNIDSI2NOEXIT', 'FALLUNIDNO2PABORT', 'EXITSI2RESPRUE');
        }

    });

    //7: actualizarPasoPrevio2Posterior2Condicion1(paso, pasoPrevio1, pasoPrevio2, pasoPosterior1, pasoPosterior2, pasoCondicion)
    $('#btnSEGARRSI2EXIT').click(function () {
        if ($('#hfAccion').val() == "1") {
            actualizarPasoPrevio1Posterior2('SEGARRSI2EXIT', 'REARRSI2SEGARR', 'EXITNO2FALLUNID', 'EXITSI2RESPRUE');
        }

    });

    //8: actualizarPasoPrevio2Final(paso, pasoPrevio1, pasoPrevio2, pasoFinal)
    $('#btnRESPRUE2FIN').click(function () {
        if ($('#hfAccion').val() == "1") {
            //actualizarPasoPrevio2Final('RESPRUE2FIN', 'EXITSI2RESPRUE', 'PABORT2RESPRUE', 'PruebaExitosaFin');
            //actualizarPasoPrevio1Posterior1('RESPRUE2FIN', 'EXITSI2RESPRUE', 'PAVERIFDATOSING');
            //actualizarPasoPrevio2Posterior2Condicion1 1Posterior1('RESPRUE2FIN', 'EXITSI2RESPRUE', 'PAVERIFDATOSING');
            actualizarPasoPrevio2Posterior2Condicion1('RESPRUE2FIN', 'EXITSI2RESPRUE', 'PABORT2RESPRUE', 'PAVERIFDATOSING', 'PAVERIFDATOSING', 'PAVERIFDATOSING')
                        777
        }

    });

    //9: actualizarPasoPrevio3Posterior1(paso, pasoPrevio1, pasoPrevio2, pasoPrevio3, pasoPosterior1)
    $('#btnNOEXIT2RESRESLT').click(function () {
        if ($('#hfAccion').val() == "1") {
            actualizarPasoPrevio3Posterior1('NOEXIT2RESRESLT', 'REARRNO2NOEXIT', 'SEGARRNO2NOEXIT', 'FALLUNIDSI2NOEXIT', 'RESRESLT2FIN');
        }

    });

    $('#btnGrabarObservacion').click(function () {
        grabarObservacion();
    });


    $('#btnCancelar').click(function () {
        document.location.href = controladorpa;
    });


    $(document).ready(function () {
        $('#cbUsuario').val($('#hfPernomb').val());
        pintarCuadros();
        $('#mensaje').removeClass();
    });


});


function pintarCuadros() {

    if ($('#hfSIC2HOP').val() == "S") colorearPaso("btnSIC2HOP");
    if ($('#hfHOP2UT30D').val() == "S") colorearPaso("btnHOP2UT30D");
    if ($('#hfUT30D2SORT').val() == "S") colorearPaso("btnUT30D2SORT");
    if ($('#hfSORT2PRUE').val() == "S") colorearPaso("btnSORT2PRUE");
    if ($('#hfPRUENO2PA').val() == "S") colorearPaso("btnPRUENO2PA");
    if ($('#hfPA2FIN').val() == "S") colorearPaso("btnPA2FIN");
    if ($('#hfPRUESI2GPRUE').val() == "S") colorearPaso("btnPRUESI2GPRUE");
    if ($('#hfGPRUENO2NPRUE').val() == "S") colorearPaso("btnGPRUENO2NPRUE");
    if ($('#hfNPRUE2FIN').val() == "S") colorearPaso("btnNPRUE2FIN");
    if ($('#hfGPRUESI2UPRUE').val() == "S") colorearPaso("btnGPRUESI2UPRUE");
    if ($('#hfUPRUE2RPRUE').val() == "S") colorearPaso("btnUPRUE2RPRUE");
    if ($('#hfRPRUE2OA').val() == "S") colorearPaso("btnRPRUE2OA");
    if ($('#hfOA2PRIARR').val() == "S") colorearPaso("btnOA2PRIARR");
    if ($('#hfPRIARRSI2EXIT').val() == "S") colorearPaso("btnPRIARRSI2EXIT");
    if ($('#hfPRIARRNO2REARR').val() == "S") colorearPaso("btnPRIARRNO2REARR");
    if ($('#hfREARRNO2NOEXIT').val() == "S") colorearPaso("btnREARRNO2NOEXIT");
    if ($('#hfREARRSI2SEGARR').val() == "S") colorearPaso("btnREARRSI2SEGARR");
    if ($('#hfSEGARRNO2NOEXIT').val() == "S") colorearPaso("btnSEGARRNO2NOEXIT");
    if ($('#hfSEGARRSI2EXIT').val() == "S") colorearPaso("btnSEGARRSI2EXIT");
    if ($('#hfEXITNO2FALLUNID').val() == "S") colorearPaso("btnEXITNO2FALLUNID");
    if ($('#hfFALLUNIDSI2NOEXIT').val() == "S") colorearPaso("btnFALLUNIDSI2NOEXIT");
    if ($('#hfEXITSI2RESPRUE').val() == "S") colorearPaso("btnEXITSI2RESPRUE");
    if ($('#hfFALLUNIDNO2PABORT').val() == "S") colorearPaso("btnFALLUNIDNO2PABORT");
    if ($('#hfPABORT2RESPRUE').val() == "S") colorearPaso("btnPABORT2RESPRUE");
    if ($('#hfRESPRUE2FIN').val() == "S") colorearPaso("btnRESPRUE2FIN");
    if ($('#hfNOEXIT2RESRESLT').val() == "S") colorearPaso("btnNOEXIT2RESRESLT");
    if ($('#hfRESRESLT2FIN').val() == "S") colorearPaso("btnRESRESLT2FIN");
    if ($('#hfPAVERIFDATOSING').val() == "S") colorearPaso("btnPAVERIFDATOSING");
    

    //coloreado de condicionales
    if (($('#hfPRUENO2PA').val() == "S") || ($('#hfPRUESI2GPRUE').val() == "S")) colorearPaso("btndiaPrueba");

    if (($('#hfGPRUESI2UPRUE').val() == "S") || ($('#hfGPRUENO2NPRUE').val() == "S")) colorearPaso("btnGrupoPrueba");
    if (($('#hfPRIARRSI2EXIT').val() == "S") || ($('#hfPRIARRNO2REARR').val() == "S")) colorearPaso("btnPrimerArranque");
    if (($('#hfREARRSI2SEGARR').val() == "S") || ($('#hfREARRNO2NOEXIT').val() == "S")) colorearPaso("btnSolicitudEmpresa");
    if (($('#hfSEGARRSI2EXIT').val() == "S") || ($('#hfSEGARRNO2NOEXIT').val() == "S")) colorearPaso("btnSegundoArranque");
    if (($('#hfEXITNO2FALLUNID').val() == "S") || ($('#hfEXITSI2RESPRUE').val() == "S")) colorearPaso("btnPruebaExitosa");
    if (($('#hfFALLUNIDSI2NOEXIT').val() == "S") || ($('#hfFALLUNIDNO2PABORT').val() == "S")) colorearPaso("btnFallaUnidad");

    if ($('#hfPA2FIN').val() == "S") colorearPaso("btnDiaDePrueba");
    if ($('#hfNPRUE2FIN').val() == "S") colorearPaso("btnGruposPrueba");
    //if ($('#hfRESPRUE2FIN').val() == "S") colorearPaso("btnPruebaExitosaFin");
    if ($('#hfPAVERIFDATOSING').val() == "S") colorearPaso("btnPruebaExitosaFin");
    if ($('#hfRESRESLT2FIN').val() == "S") colorearPaso("btnUnidadIndisponible");

    if ($('#hfPAVERIFDATOSING').val() == "S") colorearPaso("btnPAVERIFDATOSING");

}


function retornar() {
    return false;
}


function colorearPaso(id) {

    var id1 = "#" + id;
    var data = $(id1).mouseout().data('maphilight') || {};

    data.alwaysOn = true;
    $(id1).data('maphilight', data).trigger('alwaysOn.maphilight');

}

function actualizarPasoInicial(paso) {
    $('#mensaje').removeClass();
    $('#mensaje').html('');
    var fecha = $('#hfPAFECHA').val();

    paso = paso.toUpperCase();


    $('#hfPernomb').val($('#cbUsuario').val());


    if ($('#hfFINAL').val() == "S") return;


    switch (paso) {
        case "SIC2HOP":
            //predecesor
            if ($('#hfPernomb').val() == "0" || $('#hfPernomb').val() == "" || $('#hfPernomb').val() == null) {
                alert("Debe seleccionar programador...");
                return;
            } else {
                if ($('#hfSIC2HOP').val() == "S") return;
                if ($('#hfHOP2UT30D').val() == "N") {

                    $.ajax({
                        type: 'POST',
                        url: controladorpa + "actualizarpasoinicial",
                        dataType: 'json',
                        data: {
                            id: $('#hfPAFECHA').val(),
                            paso: paso,
                            programador: $('#hfPernomb').val()
                        },
                        success: function (result) {
                            if (result == "") {
                                $('#hfSIC2HOP').val("S");
                                //colorear boton
                                colorearPaso("btnSIC2HOP");
                                pintarCuadros();

                            } else {

                            }
                        },
                        error: function () {

                        }


                    });

                }


            }
    }


}


function actualizarPasoPrevio1Posterior1(paso, pasoPrevio, pasoPosterior) {

    $('#mensaje').removeClass();
    $('#mensaje').html('');
    var fecha = $('#hfPAFECHA').val();

    paso = paso.toUpperCase();

    if ($('#hfFINAL').val() == "S") return;

    var idPaso = '#hf' + paso;
    var idPasoPrevio = '#hf' + pasoPrevio;
    var idPasoPosterior = '#hf' + pasoPosterior;

    if ($(idPaso).val() == "S") return;

    if ($(idPasoPrevio).val() == "S" && $(idPasoPosterior).val() == "N") {
        //actualiza campo
        $.ajax({
            type: 'POST',
            url: controladorpa + "actualizarpaso",
            dataType: 'json',
            data: {
                id: $('#hfPAFECHA').val(),
                paso: paso
            },
            success: function (result) {
                if (result == "") {
                    $(idPaso).val("S");
                    //colorear boton
                    var btnPaso = "btn" + paso;
                    colorearPaso(btnPaso);
                    pintarCuadros();

                } else {

                }
            },
            error: function () {

            }


        });
    } else {
        mostrarAlerta(mensajeFlujograma);
    }

}


function actualizarPasoPrevio1Posterior2(paso, pasoPrevio, pasoPosterior1, pasoPosterior2) {
    $('#mensaje').removeClass();
    $('#mensaje').html('');
    var fecha = $('#hfPAFECHA').val();

    paso = paso.toUpperCase();



    if ($('#hfFINAL').val() == "S") return;

    var idPaso = '#hf' + paso;
    var idPasoPrevio = '#hf' + pasoPrevio;
    var idPasoPosterior1 = '#hf' + pasoPosterior1;
    var idPasoPosterior2 = '#hf' + pasoPosterior2;

    if ($(idPaso).val() == "S") return;

    if ($(idPasoPrevio).val() == "S" && $(idPasoPosterior1).val() == "N" && $(idPasoPosterior2).val() == "N") {
        //actualiza campo
        $.ajax({
            type: 'POST',
            url: controladorpa + "actualizarpaso",
            dataType: 'json',
            data: {
                id: $('#hfPAFECHA').val(),
                paso: paso
            },
            success: function (result) {
                if (result == "") {
                    $(idPaso).val("S");
                    //colorear boton
                    var btnPaso = "btn" + paso;
                    colorearPaso(btnPaso);
                    pintarCuadros();

                }
                else {

                }
            },
            error: function () {

            }





        });
    } else {
        mostrarAlerta(mensajeFlujograma);
    }

}


function actualizarPasoPrevio1Final(paso, pasoPrevio, pasoFinal) {

    $('#mensaje').removeClass();
    $('#mensaje').html('');
    var fecha = $('#hfPAFECHA').val();

    paso = paso.toUpperCase();

    if ($('#hfFINAL').val() == "S") return;

    var idPaso = '#hf' + paso;
    var idPasoPrevio = '#hf' + pasoPrevio;


    if ($(idPaso).val() == "S") return;



    if ($(idPasoPrevio).val() == "S") {
        //actualiza campo
        $.ajax({
            type: 'POST',
            url: controladorpa + "actualizarpaso",
            dataType: 'json',
            data: {
                id: $('#hfPAFECHA').val(),
                paso: paso
            },
            success: function (result) {
                if (result == "") {
                    $(idPaso).val("S");
                    //colorear boton
                    var btnPaso = "btn" + paso;
                    colorearPaso(btnPaso);
                    var btnFinal = "btn" + pasoFinal;
                    colorearPaso(btnFinal);
                    pintarCuadros();
                    $('#hfFINAL').val("S")
                } else {

                }
            },
            error: function () {

            }


        });
    }
    else {
        mostrarAlerta(mensajeFlujograma);
    }

}



function actualizarPasoPrevio1Posterior2Condicion1(paso, pasoPrevio, pasoPosterior1, pasoPosterior2, pasoCondicion) {

    $('#mensaje').removeClass();
    $('#mensaje').html('');
    var fecha = $('#hfPAFECHA').val();

    paso = paso.toUpperCase();


    if ($('#hfFINAL').val() == "S") return;

    var idPaso = '#hf' + paso;
    var idPasoPrevio = '#hf' + pasoPrevio;
    var idPasoPosterior1 = '#hf' + pasoPosterior1;
    var idPasoPosterior2 = '#hf' + pasoPosterior2;
    var idPasoCondicion = '#hf' + pasoCondicion;


    if ($(idPaso).val() == "S") return;



    if ($(idPasoPrevio).val() == "S" && $(idPasoPosterior1).val() == "N" && $(idPasoPosterior2).val() == "N" && $(idPasoCondicion).val() == "N") {
        //actualiza campo
        $.ajax({
            type: 'POST',
            url: controladorpa + "actualizarpaso",
            dataType: 'json',
            data: {
                id: $('#hfPAFECHA').val(),
                paso: paso
            },
            success: function (result) {
                if (result == "") {
                    $(idPaso).val("S");
                    //colorear boton
                    var btnPaso = "btn" + paso;
                    colorearPaso(btnPaso);
                    pintarCuadros();

                } else {

                }
            },
            error: function () {

            }


        });
    } else {
        mostrarAlerta(mensajeFlujograma);
    }
}


function actualizarPasoPrevio1Posterior2Condicion1DobleCondicion(paso, pasoPrevio, pasoPrevio2, pasoPosterior1, pasoPosterior2, pasoCondicion) {

    $('#mensaje').removeClass();
    $('#mensaje').html('');
    var fecha = $('#hfPAFECHA').val();

    paso = paso.toUpperCase();


    if ($('#hfFINAL').val() == "S") return;

    var idPaso = '#hf' + paso;
    var idPasoPrevio = '#hf' + pasoPrevio;
    var idPasoPrevio2 = '#hf' + pasoPrevio2;
    var idPasoPosterior1 = '#hf' + pasoPosterior1;
    var idPasoPosterior2 = '#hf' + pasoPosterior2;
    var idPasoCondicion = '#hf' + pasoCondicion;


    if ($(idPaso).val() == "S") return;



    if (($(idPasoPrevio).val() == "S" || $(idPasoPrevio2).val() == "S") && $(idPasoPosterior1).val() == "N" && $(idPasoPosterior2).val() == "N" && $(idPasoCondicion).val() == "N") {
        //actualiza campo
        $.ajax({
            type: 'POST',
            url: controladorpa + "actualizarpaso",
            dataType: 'json',
            data: {
                id: $('#hfPAFECHA').val(),
                paso: paso
            },
            success: function (result) {
                if (result == "") {
                    $(idPaso).val("S");
                    //colorear boton
                    var btnPaso = "btn" + paso;
                    colorearPaso(btnPaso);
                    pintarCuadros();

                } else {

                }
            },
            error: function () {

            }


        });
    } else {
        mostrarAlerta(mensajeFlujograma);
    }
}


function actualizarPasoPrevio2Posterior2Condicion1(paso, pasoPrevio1, pasoPrevio2, pasoPosterior1, pasoPosterior2, pasoCondicion) {

    $('#mensaje').removeClass();
    $('#mensaje').html('');
    var fecha = $('#hfPAFECHA').val();

    paso = paso.toUpperCase();



    if ($('#hfFINAL').val() == "S") return;

    var idPaso = '#hf' + paso;
    var idPasoPrevio1 = '#hf' + pasoPrevio1;
    var idPasoPrevio2 = '#hf' + pasoPrevio2;
    var idPasoPosterior1 = '#hf' + pasoPosterior1;
    var idPasoPosterior2 = '#hf' + pasoPosterior2;
    var idPasoCondicion = '#hf' + pasoCondicion;


    if ($(idPaso).val() == "S") return;



    if (($(idPasoPrevio1).val() == "S" || $(idPasoPrevio2).val() == "S")
        && $(idPasoPosterior1).val() == "N" && $(idPasoPosterior2).val() == "N" && $(idPasoCondicion).val() == "N") {
        //actualiza campo
        $.ajax({
            type: 'POST',
            url: controladorpa + "actualizarpaso",
            dataType: 'json',
            data: {
                id: $('#hfPAFECHA').val(),
                paso: paso
            },
            success: function (result) {
                if (result == "") {
                    $(idPaso).val("S");
                    //colorear boton
                    var btnPaso = "btn" + paso;
                    colorearPaso(btnPaso);
                    pintarCuadros();

                } else {

                }
            },
            error: function () {

            }

        });
    }

}


function actualizarPasoPrevio2Final(paso, pasoPrevio1, pasoPrevio2, pasoFinal) {

    $('#mensaje').removeClass();
    $('#mensaje').html('');
    var fecha = $('#hfPAFECHA').val();
    paso = paso.toUpperCase();

    if ($('#hfFINAL').val() == "S") return;

    var idPaso = '#hf' + paso;
    var idPasoPrevio1 = '#hf' + pasoPrevio1;
    var idPasoPrevio2 = '#hf' + pasoPrevio2;

    if ($(idPaso).val() == "S") return;

    if ($(idPasoPrevio1).val() == "S" || $(idPasoPrevio2).val() == "S") {
        //actualiza campo
        $.ajax({
            type: 'POST',
            url: controladorpa + "actualizarpaso",
            dataType: 'json',
            data: {
                id: $('#hfPAFECHA').val(),
                paso: paso
            },
            success: function (result) {
                if (result == "") {
                    $(idPaso).val("S");
                    //colorear boton
                    var btnPaso = "btn" + paso;
                    colorearPaso(btnPaso);
                    var btnFinal = "btn" + pasoFinal;
                    $('#hfFINAL').val("S")
                    colorearPaso(btnFinal);
                    pintarCuadros();
                } else {

                }
            },
            error: function () {

            }


        });
    } else {
        mostrarAlerta(mensajeFlujograma);
    }



}


function actualizarPasoPrevio3Posterior1(paso, pasoPrevio1, pasoPrevio2, pasoPrevio3, pasoPosterior1) {
    $('#mensaje').removeClass();
    $('#mensaje').html('');
    var fecha = $('#hfPAFECHA').val();

    paso = paso.toUpperCase();


    if ($('#hfFINAL').val() == "S") return;

    var idPaso = '#hf' + paso;
    var idPasoPrevio1 = '#hf' + pasoPrevio1;
    var idPasoPrevio2 = '#hf' + pasoPrevio2;
    var idPasoPrevio3 = '#hf' + pasoPrevio3;
    var idPasoPosterior1 = '#hf' + pasoPosterior1;


    if ($(idPaso).val() == "S") return;



    if (($(idPasoPrevio1).val() == "S" || $(idPasoPrevio2).val() == "S" || $(idPasoPrevio3).val() == "S")
        && $(idPasoPosterior1).val() == "N") {
        //actualiza campo
        $.ajax({
            type: 'POST',
            url: controladorpa + "actualizarpaso",
            dataType: 'json',
            data: {
                id: $('#hfPAFECHA').val(),
                paso: paso
            },
            success: function (result) {
                if (result == "") {
                    $(idPaso).val("S");
                    //colorear boton
                    var btnPaso = "btn" + paso;
                    colorearPaso(btnPaso);
                    pintarCuadros();

                } else {

                }
            },
            error: function () {

            }


        });
    } else {
        mostrarAlerta(mensajeFlujograma);
    }

}


function grabarObservacion() {


    var fecha = $('#hfPAFECHA').val();

    $.ajax({
        type: 'POST',
        url: controladorpa + "grabarobservacion",
        dataType: 'json',
        data: {
            id: $('#hfPAFECHA').val(),
            observacion: $('#txtObservaciones').val()
        },
        success: function (result) {
            mostrarExito("Se grabó la observación...");
        },
        error: function () {

        }


    });

}

mostrarAlerta = function (mensaje) {
    $('#mensaje').removeClass();
    $('#mensaje').addClass('action-alert');
    $('#mensaje').html(mensaje);
}

mostrarExito = function (mensaje) {
    $('#mensaje').removeClass();
    $('#mensaje').addClass('action-exito');
    $('#mensaje').html(mensaje);
}


iniciarControlTexto = function (id) {


    var result = $('#' + id).val();

    tinymce.remove();

    tinymce.init({
        selector: '#' + id,
        plugins: [
        'paste textcolor colorpicker textpattern'
        ],
        toolbar1: 'insertfile undo redo | bold italic | alignleft aligncenter alignright alignjustify |  bullist numlist outdent indent| forecolor backcolor',
        menubar: false,
        language: 'es',
        statusbar: false,
        setup: function (editor) {
            editor.on('change', function () {
                editor.save();
            });
        }
    });

}

mostrarError = function () {
    alert("Ha ocurrido un error");

}

validarFormatoCorreo = function () {
    var mensaje = "";

    var validaFrom = validarCorreo($('#From').val(), 1, 1);
    if (validaFrom < 0) mensaje = mensaje + " " + "Revisar campo From.\n";

    var validaTo = validarCorreo($('#To').val(), 1, 1);
    if (validaTo < 0) mensaje = mensaje + " " + "Revisar campo To.\n";

    var validaCc = validarCorreo($('#CC').val(), 0, -1);
    if (validaCc < 0) mensaje = mensaje + " " + "Revisar campo CC.\n";

    var validaBcc = validarCorreo($('#BCC').val(), 0, -1);
    if (validaBcc < 0) mensaje = mensaje + " " + "Revisar campo BCC.\n";

    var asunto = $('#Asunto').val().trim() + "";

    if (asunto == "")
        mensaje = mensaje + " " + "Revisar asunto del correo.\n";

    var contenido = $('#Contenido').val().trim() + "";
    var spans = contenido.indexOf('">');
    var spans2 = contenido.indexOf('</');

    if (spans > 0 && spans2 > 0) {
        if ((spans2 - spans) < 10) {
            mensaje = mensaje + " " + "Revisar contenido del correo.\n";
        }
    }


    if (mensaje == "") {
        return 1;
    } else {
        alert(mensaje);
        return -1;
    }

}

validarCorreo = function (cadena, minimo, maximo) {
    var arreglo = cadena.split(';');
    var nroCorreo = 0;
    var longitud = arreglo.length;

    if (longitud == 0) {
        arreglo = cadena;
        longitud = 1;
    }

    for (var i = 0; i < longitud; i++) {

        var palabra = arreglo[i].trim();
        var validacion = validarDirecccionCorreo(palabra);

        if (validacion)
            nroCorreo++;

    }

    if (minimo > nroCorreo)
        return -1;

    if (maximo > 0 && nroCorreo > maximo)
        return -1;

    return 1;
}

function validarDirecccionCorreo(email) {
    var re = /^(([^<>()\[\]\\.,;:\s@"]+(\.[^<>()\[\]\\.,;:\s@"]+)*)|(".+"))@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}])|(([a-zA-Z\-0-9]+\.)+[a-zA-Z]{2,}))$/;
    return re.test(email);
}













