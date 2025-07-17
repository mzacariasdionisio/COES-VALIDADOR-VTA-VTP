var controler = siteRoot + "transferencias/solicitudcodigo/";

let arrayAgrupacion = [];

const tipoCaso = {

    AgrupacionVTA: "AGRVTA",
    AgrupacionVTP: "AGRVTP"

}
$(document).ready(function () {
    validarPeriodo();
    buscar();
    agruparEnviarServidor();
    desagruparEnviarServidor();
    exportarDatos();
    importarDatos();
    btnVerErrores();
    enviarDatos();
    verEnvios();
    $('#CLICODI2').multipleSelect({
        width: '200px',
        filter: true,
        selectAll: false,
        single: true,
        placeholder: '--Seleccione--'
    });

    $('#BARRCODI2').multipleSelect({
        width: '200px',
        filter: true,
        selectAll: false,
        single: true,
        placeholder: '--Seleccione--'
    });

    $('.txtFecha').Zebra_DatePicker({
    });

    $('#btnBuscar').click(function () {
        buscar();
    });

    $('#btnGenerarExcel').click(function () {
        //generarExcel();
    });

    $('#btnNuevo').click(function () {
        location.href = controler + 'New?peridcCodi=' + $("#PERIODO2 option:selected").val() + '&peridcNombre=' + $("#PERIODO2 option:selected").html()
    })
    $('select[id="BARRCODI2"]').change(function () {
        var val = $(this).val();
        //$('select[name="ProdId"]').each(function (index, value) {
        //    $(value).val(val);
        //});
        $.ajax({
            type: 'POST',
            url: controler + "ListaSuministro",
            data: { bartran: val },
            success: function (evt) {
                $('#listaSuministro').html(evt);
            },
            error: function () {
                //mostrarError();
            }
        });
    });


    $('#PERIODO2').change(() => {
        validarPeriodo();
    })
});

validarPeriodo = function () {

    $.ajax({
        async: false,
        type: 'POST',
        url: controler + "ObtenerPeriodo",
        data: { idPeriodo: $('#PERIODO2').val() },
        success: function (evt) {
            if (evt.EsCorrecto >= 0) {
                let estado = evt.Data.PeridcEstado;
                $('#btnAgrupar').parent().hide();
                $('#btnDesagrupar').parent().hide();
                $('#btnSubir').parent().hide();
                $('#btnEnviarDatos').parent().hide();
                $('#btnVerErrores').parent().hide();
                if (estado == 'ABI') {
                    $('#btnAgrupar').parent().show();
                    $('#btnDesagrupar').parent().show();
                    $('#btnSubir').parent().show();
                    $('#btnEnviarDatos').parent().show();
                    $('#btnVerErrores').parent().show();
                }

            }
        },
        error: function () {
            //mostrarError();
        }
    });

}
seleccionarEmpresa = function () {
    $.ajax({
        type: 'POST',
        url: controler + "EscogerEmpresa",
        success: function (evt) {
            $('#popup').html(evt);
            setTimeout(function () {
                $('#popup').bPopup({
                    easing: 'easeOutBack',
                    speed: 450,
                    transition: 'slideDown'
                });
            }, 50);
        },
        error: function () {
            alert("Lo sentimos, ha ocurrido un error inesperado");
        }
    });
}

function buscar() {
    pintarPaginado();
    mostrarListado(1);
}

function pintarPaginado() {
    //$.ajax({
    //    type: 'POST',
    //    url: controler + "paginado",
    //    data: {
    //        nombre: $('#txtNombre').val(),
    //        tipoUsu: $("#TIPOUSUACODI option:selected").text(),
    //        tipoCont: $("#TIPOCONTCODI option:selected").text(),
    //        barTran: $("#BARRTRANCODI option:selected").text(),
    //        cliNomb: $("#CLICODI option:selected").text(),
    //        pericodi: $("#PERIODO2 option:selected").val(),
    //        fechaInicio: $('#txtfechaIni').val(),
    //        fechaFin: $('#txtfechaFin').val()
    //    },
    //    success: function (evt) {
    //        $('#paginado').html(evt);
    //        mostrarPaginado();
    //    },
    //    error: function () {
    //        alert("Lo sentimos, ha ocurrido un error inesperado");
    //    }
    //});
}

function mostrarListado(nroPagina) {
    debugger;
    var gg = $('#CLICODI').val();

    $('#hfNroPagina').val(nroPagina);
    $.ajax({
        type: 'POST',
        url: controler + "lista",
        data: {
            clicodi: $('#CLICODI').val(),
            nombre: $('#txtNombre').val(),
            tipoUsu: $("#TIPOUSUACODI option:selected").text(),
            tipoCont: $("#TIPOCONTCODI option:selected").text(),
            barrTran: $("#BARRTRANCODI option:selected").text(),
            cliNomb: $("#CLICODI option:selected").text(),
            pericodi: $("#PERIODO2 option:selected").val(),

            pericodiNombre: $("#PERIODO2 option:selected").html(),
            fechaInicio: $('#txtfechaIni').val(), fechaFin: $('#txtfechaFin').val(),
            // nroPagina: $('#hfNroPagina').val()
            nroPagina: 1
        },
        success: function (evt) {
            $('#listado').html(evt);
            SolicitarBajaVTEA();
            SolicitarBajaVTP();
            //ViewEvent();
            //oTable = $('#tabla').dataTable({
            //    "sPaginationType": "full_numbers",
            //    "destroy": "true",
            //    "aaSorting": [[1, "asc"]]
            //});
            agruparPotenciasCheckbox();

            $('#listado table > tbody > tr').hover(function (event) {

                $('#listado table > tbody > tr').removeClass('table-row-hover')
                $('#listado table > tbody > tr[data-row-for-id] > td[data-row-index="0"]').removeClass('table-row-hover')
                let target = $(event.currentTarget);

                if (target.hasClass('child')) {
                    let forId = target.attr('data-row-for-id');
                    let index = target.attr('data-row-index');
                    if (index != 0) {
                        target.addClass('table-row-hover')
                        $('#listado table > tbody > tr[data-row-for-id="' + forId + '"][data-row-index="0"] td[data-row-index="0"]').addClass('table-row-hover')
                    }
                    $('#listado table > tbody > tr[data-row-id="' + forId + '"]').addClass('table-row-hover');
                    //-- 
                    $('#listado table > tbody > tr[data-row-for-id="' + forId + '"]').addClass('table-row-hover')
                } else {
                    let forId = target.attr('data-row-id');

                    target.addClass('table-row-hover')
                    $('#listado table > tbody > tr[data-row-for-id="' + forId + '"]').addClass('table-row-hover')
                    $('#listado table > tbody > tr[data-row-id="' + forId + '"]').addClass('table-row-hover')
                }

            })

        },
        error: function () {
            //mostrarError();
        }
    });
}

function pintarBusqueda(nroPagina) {
    mostrarListado(nroPagina);
}

//funcion de agrupar

function agruparPotenciasCheckbox() {

    $('#listado table > tbody > tr > td input[type="checkbox"]').change((target) => {
        let currentTarget = $(target.currentTarget);
        let coresoCodi = currentTarget.parents('tr').attr('data-row-id');
        //
        let coresoCodiChild = currentTarget.parents('tr').attr('data-row-for-id');
        let checked = currentTarget.prop('checked')
        if (currentTarget.hasClass(tipoCaso.AgrupacionVTA)) {

            $('#listado table > tbody > tr[data-row-for-id="' + coresoCodi + '"] > td input[type="checkbox"]').each((index, targetVtp) => {
                let currentTargetVTP = $(targetVtp);
                currentTargetVTP.prop('checked', checked)
            })
            currentTarget.parent().next().next().next().children('input[type="checkbox"]').prop('checked', checked)
        } else if (currentTarget.hasClass(tipoCaso.AgrupacionVTP)) {

            let totalChild = 0;
            let totalFirstChild = 0;
            let totalChildChecked = 0;
            let totalFirstChildChecked = 0;
            if (typeof coresoCodi == 'undefined') {

                let currentCoresoCodi = $('#listado table > tbody > tr[data-row-id="' + coresoCodiChild + '"] > td input[type="checkbox"][class="' + tipoCaso.AgrupacionVTA + '"]');

                totalChild = $('#listado table > tbody > tr[data-row-for-id="' + coresoCodiChild + '"] > td input[type="checkbox"]').length
                totalFirstChild = $('#listado table > tbody > tr[data-row-id="' + coresoCodiChild + '"] > td input[type="checkbox"][class="' + tipoCaso.AgrupacionVTP + '"]').length;
                totalChildChecked = $('#listado table > tbody > tr[data-row-for-id="' + coresoCodiChild + '"] > td input[type="checkbox"]:checked').length
                totalFirstChildChecked = $('#listado table > tbody > tr[data-row-id="' + coresoCodiChild + '"] > td input[type="checkbox"][class="' + tipoCaso.AgrupacionVTP + '"]:checked').length;

                if ((totalChild + totalFirstChild) == (totalChildChecked + totalFirstChildChecked)) {
                    currentCoresoCodi.prop('checked', true)
                } else {
                    currentCoresoCodi.prop('checked', false)
                }

            } else {

                let currentCoresoCodi = $('#listado table > tbody > tr[data-row-id="' + coresoCodi + '"] > td input[type="checkbox"][class="' + tipoCaso.AgrupacionVTA + '"]');
                totalChild = $('#listado table > tbody > tr[data-row-for-id="' + coresoCodi + '"] > td input[type="checkbox"]').length
                totalFirstChild = 1;
                totalChildChecked = $('#listado table > tbody > tr[data-row-for-id="' + coresoCodi + '"] > td input[type="checkbox"]:checked').length
                totalFirstChildChecked = $('#listado table > tbody > tr[data-row-id="' + coresoCodi + '"] > td input[type="checkbox"][class="' + tipoCaso.AgrupacionVTP + '"]:checked').length;

                if ((totalChild + totalFirstChild) == (totalChildChecked + totalFirstChildChecked)) {
                    currentCoresoCodi.prop('checked', true)
                } else {
                    currentCoresoCodi.prop('checked', false)
                }

            }
        }


    })

}

function agruparEnviarServidor() {
    $('#btnAgrupar').click(function () {
        var arrayPotencias = [];
        //aQUI
        var mensaje = "";
        var error = false;
        var auxiliar = ''
        var estadoFila = '';

        if ($('table tbody tr  input.AGRVTA[type="checkbox"]:checked').length <= 1
            && $('table tbody tr input.AGRVTP[type="checkbox"]:checked').length <= 1) {
            error = true;
            mensaje += `Para realizar una agrupacion  a nivel de VTA/VTP, es necesario seleccionar mas de uno.`;
        }


        if (!error) {
            $('table tbody tr input.AGRVTA[type="checkbox"]:checked').each((index, target) => {

                let errorIndex = 0;
                var tr = $(target).parent().parent().find('td:nth-child(0n+2)');
                var trEstado = $(target).parent().parent().find('td:nth-child(0n+7)');

         
                if (index > 0) {
                    if (tr.html().trim() != auxiliar.trim()) {
                        mensaje += `El cliente seleccionado ${tr.html()} es diferente al cliente inicial marcado ${auxiliar}.<br/><br/>`;
                        error = true;
                    } else if (trEstado.html().trim() != estadoFila) {
                        mensaje += `El cliente seleccionado ${tr.html()} es diferente el estado con la empresa marcada ${auxiliar}.<br/><br/>`;
                        error = true;

                    }
                }
                else {
                    auxiliar = tr.html();
                    estadoFila = trEstado.html().trim();
                }

                errorIndex = 0;
                let trParent = $(target).parent().parent();
                trParent.find('td').each((indexTD, targetTD) => {
                    if (indexTD >= 14 && indexTD <= 18) {
                        let tdVTA = $(targetTD);
                        let valor = tdVTA.html().trim();
                        if (valor == '') {
                            if (errorIndex == 0) {
                                mensaje += `El cliente <mark>${tdVTA.parent().find('td:nth-child(0n+2)').html()}</mark> con la barra <mark>${tdVTA.parent().find('td:nth-child(0n+9)').html()}</mark> tiene valores sin ingresar potencias.<br/><br/>`;
                                errorIndex++;
                            }
                            tdVTA.css('background-color', '#ecaa30')
                            tdVTA.addClass('fail')
                            error = true;
                        }
                    }
                })
            })

        }


        if (!error) {
            $('table tbody tr input.AGRVTP[type="checkbox"]:checked').each((index, target) => {
                errorIndex = 0;

                let trParent = $(target).parent().parent();
                trParent.find('td').each((indexTD, targetTD) => {
                    if (indexTD >= 4 && indexTD <= 9) {

                        let tdVTA = $(targetTD);
                        let valor = tdVTA.html().trim();
                        if (valor == '') {
                            if (errorIndex == 0) {
                                mensaje += `El cliente <mark>${tdVTA.parent().find('td:nth-child(0n+2)').html()}</mark> con la barra <mark>${tdVTA.parent().find('td:nth-child(0n+9)').html()}</mark> tiene valores sin ingresar potencias.<br/><br/>`;
                                errorIndex++;
                            }
                            tdVTA.css('background-color', '#ecaa30')
                            tdVTA.addClass('fail')
                            error = true;
                        }
                    }
                })
            });
        }


        if (!error) {
            $('#listado table > tbody > tr[data-row-id]').each((index, target) => {
                let targetInputVTA = $(target).find('td input.' + tipoCaso.AgrupacionVTA);
                let coresoCodigo = $(target).attr('data-row-id');
                let tipoPotencia = $(target).attr('data-tipo');
                if (!targetInputVTA.prop('checked')) {
                    mpVerificarVTAChecked(targetInputVTA, coresoCodigo, arrayPotencias)

                } else {
                    if (tipoPotencia == 2) {
                        mpVerificarVTAChecked(targetInputVTA, coresoCodigo, arrayPotencias);
                    } else if (tipoPotencia == 1) {
                        let potenciaAuxi = {
                            CoresoCodi: coresoCodigo,
                            CoregeCodi: null,
                            TrnpcNumOrd: arrayPotencias.filter(itemPot => itemPot.CoregeCodi == null).length + 1,
                            TrnpcTipoCasoAgrupado: tipoCaso.AgrupacionVTA,
                            PeriCodi: $("#PERIODO2 option:selected").val()
                        };
                        arrayPotencias.push(potenciaAuxi)

                        $(target).nextAll('tr[data-row-id="' + coresoCodigo + '"]').each((indexChild, targetChild) => {
                            let targetInputVTPFirst = $(targetChild);
                            //let coresoCodigoChild = $(targetChild).attr('id').split('_')[1];
                            let coresoCodigoChild = $(targetChild).attr('data-row-id');
                            let potenciaAuxi = {
                                CoresoCodi: coresoCodigoChild,
                                CoregeCodi: null,
                                TrnpcNumOrd: arrayPotencias.filter(itemPot => itemPot.CoregeCodi == null).length + 1,
                                TrnpcTipoCasoAgrupado: tipoCaso.AgrupacionVTA,
                                PeriCodi: $("#PERIODO2 option:selected").val()
                            };
                            arrayPotencias.push(potenciaAuxi)


                        });

                    }

                }
            })

            $.ajax({
                type: 'POST',
                url: controler + "SaveAgruparGrilla",
                data: JSON.stringify(arrayPotencias),
                contentType: 'application/json; charset=utf-8',
                success: function (evt) {

                    $('.mensajeLogin').hide();
                    if (evt.EsCorrecto < 0) {
                        if (evt.EsCorrecto == -10) {
                            $('.mensajeLogin').show();
                        } else {
                            alert(evt.Mensaje);
                        }
                    } else {
                        mostrarListado(parseInt($('div.paginado-activo a > span').html()));
                    }
                },
                error: function () {
                    alert("Lo sentimos, ha ocurrido un error inesperado");
                }
            });

        } else {
            $('#popupZ2 #contenidoPopup').html(mensaje);
            setTimeout(function () {
                $('#popupZ2').bPopup({
                    easing: 'easeOutBack',
                    speed: 450,
                    transition: 'slideDown'
                });
            }, 50);
        }

    })

}

function desagruparEnviarServidor() {
    $('#btnDesagrupar').click(() => {
        var arrayPotencias = [];

        var mensaje = "";
        var error = false;
        var auxiliar = ''

        if ($('table tbody tr[data-tipo="1"] input.AGRVTA[type="checkbox"]:checked').length <= 0
            && $('table tbody tr input.AGRVTP[type="checkbox"]:checked').length <= 0) {
            error = true;
            mensaje += `Para realizar una agrupacion  a nivel de VTA/VTP, es necesario seleccionar mas de uno.`;
        }

        if (!error) {
            $('#listado table > tbody > tr[data-row-id]').each((index, target) => {
                let targetInputVTA = $(target).find('td input.' + tipoCaso.AgrupacionVTA);
                let coresoCodigo = $(target).attr('data-row-id');
                let tipoPotencia = $(target).attr('data-tipo');
                if (!targetInputVTA.prop('checked')) {
                    let targetInputVTPFirst = targetInputVTA.parent().next().next().next().children('input[type="checkbox"]')
                    if (targetInputVTPFirst.prop('checked')) {
                        let potenciaAuxi = {
                            CoresoCodi: coresoCodigo,
                            CoregeCodi: targetInputVTPFirst.parents('tr').attr('data-id-generado'),
                            TrnpcNumOrd: null,
                            TrnpcTipoCasoAgrupado: tipoCaso.AgrupacionVTP,
                            PeriCodi: $("#PERIODO2 option:selected").val()
                        };
                        arrayPotencias.push(potenciaAuxi)
                    }

                    $('#listado table > tbody > tr[data-row-for-id="' + coresoCodigo + '"] td input.' + tipoCaso.AgrupacionVTP + ':checked').each((indexChild, targetChild) => {
                        let targetInputVTPFirst = $(targetChild);
                        let potenciaAuxi = {
                            CoresoCodi: coresoCodigo,
                            CoregeCodi: targetInputVTPFirst.parents('tr').attr('data-id-generado'),
                            TrnpcNumOrd: null,
                            TrnpcTipoCasoAgrupado: tipoCaso.AgrupacionVTP,
                            PeriCodi: $("#PERIODO2 option:selected").val()
                        };
                        arrayPotencias.push(potenciaAuxi)
                    })

                } else {
                    if (tipoPotencia == 2) {
                        let potenciaAuxi = {
                            CoresoCodi: coresoCodigo,
                            CoregeCodi: targetInputVTA.parents('tr').attr('data-id-generado'),
                            TrnpcNumOrd: null,
                            TrnpcTipoCasoAgrupado: tipoCaso.AgrupacionVTP,
                            PeriCodi: $("#PERIODO2 option:selected").val()
                        };
                        arrayPotencias.push(potenciaAuxi)
                    } else if (tipoPotencia == 1) {
                        let potenciaAuxi = {
                            CoresoCodi: coresoCodigo,
                            CoregeCodi: null,
                            TrnpcNumOrd: null,
                            TrnpcTipoCasoAgrupado: tipoCaso.AgrupacionVTA,
                            PeriCodi: $("#PERIODO2 option:selected").val()
                        };
                        arrayPotencias.push(potenciaAuxi)


                    }
                }
            })

            $.ajax({
                type: 'POST',
                url: controler + "DesagruparPotencias",
                data: JSON.stringify(arrayPotencias),
                contentType: 'application/json; charset=utf-8',
                success: function (evt) {

                    $('.mensajeLogin').hide();
                    if (evt.EsCorrecto < 0) {
                        if (evt.EsCorrecto == -10) {
                            $('.mensajeLogin').show();
                        } else {
                            alert(evt.Mensaje);
                        }
                    } else {
                        mostrarListado(parseInt($('div.paginado-activo a > span').html()));
                    }
                },
                error: function () {
                    alert("Lo sentimos, ha ocurrido un error inesperado");
                }
            });
        } else {
            $('#popupZ2 #contenidoPopup').html(mensaje);
            setTimeout(function () {
                $('#popupZ2').bPopup({
                    easing: 'easeOutBack',
                    speed: 450,
                    transition: 'slideDown'
                });
            }, 50);
        }



    })
}

function exportarDatos() {
    $('#btnExportar').click(() => {

        $.ajax({
            type: 'POST',
            url: controler + "ExportarInformacion",
            data: {
                nombre: $('#txtNombre').val(), tipoUsu: $("#TIPOUSUACODI option:selected").text(), tipoCont: $("#TIPOCONTCODI option:selected").text(), barrTran: $("#BARRTRANCODI option:selected").text(),
                cliNomb: $("#CLICODI option:selected").text(),
                pericodi: $("#PERIODO2 option:selected").val(),
                fechaInicio: $('#txtfechaIni').val(),
                fechaFin: $('#txtfechaFin').val(),
                nroPagina: 1
            },
            success: function (evt) {

                $('.mensajeLogin').hide();
                if (evt.EsCorrecto < 0) {
                    if (evt.EsCorrecto == -10) {
                        $('.mensajeLogin').show();
                    } else {
                        alert(evt.Mensaje);
                    }
                } else {
                    descargarArchivo(evt.Data.archivoBase64, evt.Data.nombreArchivo, 'omitir')
                }
            },
            error: function () {
                alert("Lo sentimos, ha ocurrido un error inesperado");
            }
        });
    })
}

function btnVerErrores() {
    $('#btnVerErrores').click(() => {

        if ($('#listado #mensajeError').html() != '') {
            $('#popupZ2 #contenidoPopup').html(decodeHtml($('#listado #mensajeError').html()));
            setTimeout(function () {
                $('#popupZ2').bPopup({
                    easing: 'easeOutBack',
                    speed: 450,
                    transition: 'slideDown'
                });
            }, 50);
        }
    })

}
function importarDatos() {
    $('#btnSubir').click(() => {
        $('#fileImagen').trigger('click');
    })

    $('#fileImagen').change((event) => {

        var file = event.target.files[0];

        const reader = new FileReader();
        reader.readAsDataURL(file);
        reader.onloadend = () => {
            event.target.value = "";
            let archivoBase64 = reader.result.toString().split(",")[1];
            let nombreArchivo = file.name;
            $.ajax({
                type: 'POST',
                url: controler + "lista",
                data: {
                    clicodi: $('#CLICODI').val(),
                    nombre: $('#txtNombre').val(), tipoUsu: $("#TIPOUSUACODI option:selected").text(), tipoCont: $("#TIPOCONTCODI option:selected").text(), barrTran: $("#BARRTRANCODI option:selected").text(),
                    cliNomb: $("#CLICODI option:selected").text(),
                    pericodi: $("#PERIODO2 option:selected").val(),
                    fechaInicio: $('#txtfechaIni').val(),
                    fechaFin: $('#txtfechaFin').val(),
                    nroPagina: 1,
                    nombreArchivo: nombreArchivo,
                    base64: archivoBase64
                },
                success: function (evt, status, xhr) {

                    $('.mensajeLogin').hide();
                    var ct = xhr.getResponseHeader("content-type") || "";
                    if (ct.indexOf('html') > -1) {

                        $('#listado').html(evt);
                        SolicitarBajaVTEA();
                        SolicitarBajaVTP();
                        $('#mensaje').show();
                        $('#mensaje').html('Se ha cargado correctamente la información de potencias contratadas, por favor enviar datos para confirmar.');

                        //ViewEvent();
                        //oTable = $('#tabla').dataTable({
                        //    "sPaginationType": "full_numbers",
                        //    "destroy": "true",
                        //    "aaSorting": [[1, "asc"]]
                        //});
                        agruparPotenciasCheckbox();

                        $('#listado table > tbody > tr').hover(function (event) {

                            $('#listado table > tbody > tr').removeClass('table-row-hover')
                            $('#listado table > tbody > tr[data-row-for-id] > td[data-row-index="0"]').removeClass('table-row-hover')
                            let target = $(event.currentTarget);

                            if (target.hasClass('child')) {
                                let forId = target.attr('data-row-for-id');
                                let index = target.attr('data-row-index');
                                if (index != 0) {
                                    target.addClass('table-row-hover')
                                    $('#listado table > tbody > tr[data-row-for-id="' + forId + '"][data-row-index="0"] td[data-row-index="0"]').addClass('table-row-hover')
                                }
                                $('#listado table > tbody > tr[data-row-id="' + forId + '"]').addClass('table-row-hover');
                                //-- 
                                $('#listado table > tbody > tr[data-row-for-id="' + forId + '"]').addClass('table-row-hover')
                            } else {
                                let forId = target.attr('data-row-id');

                                target.addClass('table-row-hover')
                                $('#listado table > tbody > tr[data-row-for-id="' + forId + '"]').addClass('table-row-hover')
                                $('#listado table > tbody > tr[data-row-id="' + forId + '"]').addClass('table-row-hover')
                            }

                        })

                        if ($('#listado #mensajeError').html() != '') {
                            $('#popupZ2 #contenidoPopup').html(decodeHtml($('#listado #mensajeError').html()));
                            setTimeout(function () {
                                $('#popupZ2').bPopup({
                                    easing: 'easeOutBack',
                                    speed: 450,
                                    transition: 'slideDown'
                                });
                            }, 50);
                        }


                    }
                    if (ct.indexOf('json') > -1) {

                        if (evt.EsCorrecto == -10) {
                            $('.mensajeLogin').show();
                        } else {

                            $('#popupZ2 #popup-title span').html('Errores de Datos');
                            $('#popupZ2 #contenidoPopup').html(evt.Mensaje);
                            setTimeout(function () {
                                $('#popupZ2').bPopup({
                                    easing: 'easeOutBack',
                                    speed: 450,
                                    transition: 'slideDown'
                                });
                            }, 50);
                        }
                       


                    }


                },
                error: function () {
                    alert("Lo sentimos, ha ocurrido un error inesperado");
                }
            });
        };


    })
}


function enviarDatos() {

    $('#btnEnviarDatos').click(() => {
        var arrayPotencias = [];

        var mensaje = "";
        var error = false;
        var auxiliar = ''

        //aqui
        $('#listado table > tbody > tr[data-excel="1"]').each((index, targetTR) => {
            let errorIndex = 0;

            let estadoVTA = '';

            if ($(targetTR).find('td').length > 11) {
                var estadoVTP = $(targetTR).find('td:nth-child(0n+7)').html();
                let valor = estadoVTP;


                if (valor != 'Baja'
                    && valor != 'Rechazado') {
                    $(targetTR).find('td').each((index, target) => {
                        var index = $(target).index();
                        if (index >= 14 && index <= 18) {
                            let valor = $(target).html().trim();
                            if (valor == '') {
                                if (errorIndex == 0) {
                                    mensaje += `El cliente <mark>${$(target).parent().find('td:nth-child(0n+2)').html()}</mark> con la barra <mark>${$(target).parent().find('td:nth-child(0n+9)').html()}</mark> tiene valores sin ingresar potencias.<br/><br/>`;
                                }
                                $(target).css('background-color', '#ecaa30')
                                $(target).addClass('fail')
                                error = true;
                                errorIndex++;
                            }
                        }
                    })
                }

            } else {


                $(targetTR).find('td').each((indexTD, targetTD) => {


                    if (indexTD >= 4 && indexTD <= 9) {

                        console.log($(targetTD).parent().html())
                        var idrow = $(targetTD).parent().attr('data-row-for-id');

                        var estadoVTA = $('#listado table > tbody > tr[data-row-id="' + idrow + '"] td:nth-child(0n+7)').html();

                        if (estadoVTA != 'Baja'
                            && estadoVTA != 'Rechazado') {

                            let tdVTA = $(targetTD);
                            let valor = tdVTA.html().trim();
                            if (valor == '') {
                                if (errorIndex == 0) {
                                    mensaje += `El cliente <mark>${tdVTA.parent().find('td:nth-child(0n+3)').html()}</mark> con la barra <mark>${tdVTA.parent().find('td:nth-child(0n+9)').html()}</mark> tiene valores sin ingresar potencias.<br/><br/>`;
                                    errorIndex++;
                                }
                                tdVTA.css('background-color', '#ecaa30')
                                tdVTA.addClass('fail')
                                error = true;
                            }
                        }

                    }

                })
            }


        })

        if (!error) {
            $('#listado table > tbody > tr[data-excel="1"]').each((index, target) => {
                let tdCell = $(target);
                let targetInputVTA = $(target).find('td input.' + tipoCaso.AgrupacionVTA);
                let coresoCodigo = $(target).attr('data-row-id');
                let tipoPotencia = $(target).attr('data-tipo');
                if (typeof (coresoCodigo) != 'undefined') {
                    if (tipoPotencia == 2) {
                        let potenciaAuxi = {
                            PeriCodi: $("#PERIODO2 option:selected").val(),
                            CoresoCodi: coresoCodigo,
                            CoregeCodi: targetInputVTA.parents('tr').attr('data-id-generado'),
                            TrnpcNumOrd: null,
                            TrnpcTipoCasoAgrupado: tipoCaso.AgrupacionVTP,
                            TrnPctTotalMwFija: tdCell.children('td:nth-child(0n+14)').html(),
                            TrnPctHpMwFija: tdCell.children('td:nth-child(0n+15)').html(),
                            TrnPctHfpMwFija: tdCell.children('td:nth-child(0n+16)').html(),
                            TrnPctTotalMwVariable: tdCell.children('td:nth-child(0n+17)').html(),
                            TrnPctHpMwFijaVariable: tdCell.children('td:nth-child(0n+18)').html(),
                            TrnPctHfpMwFijaVariable: tdCell.children('td:nth-child(0n+19)').html(),
                            TrnPctComeObs: tdCell.children('td:nth-child(0n+20)').html()
                        };
                        arrayPotencias.push(potenciaAuxi)

                        $('#listado table > tbody > tr[data-excel="1"][data-row-for-id="' + coresoCodigo + '"]').each((indexChild, targetChild) => {
                            let tdCell = $(targetChild);
                            let targetInputVTPFirst = $(targetChild);
                            let potenciaAuxi = {
                                PeriCodi: $("#PERIODO2 option:selected").val(),
                                CoresoCodi: coresoCodigo,
                                CoregeCodi: targetInputVTPFirst.attr('data-id-generado'),
                                TrnpcNumOrd: null,
                                TrnpcTipoCasoAgrupado: tipoCaso.AgrupacionVTP,
                                TrnPctTotalMwFija: tdCell.children('td:nth-child(0n+4)').html(),
                                TrnPctHpMwFija: tdCell.children('td:nth-child(0n+5)').html(),
                                TrnPctHfpMwFija: tdCell.children('td:nth-child(0n+6)').html(),
                                TrnPctTotalMwVariable: tdCell.children('td:nth-child(0n+7)').html(),
                                TrnPctHpMwFijaVariable: tdCell.children('td:nth-child(0n+8)').html(),
                                TrnPctHfpMwFijaVariable: tdCell.children('td:nth-child(0n+9)').html(),
                                TrnPctComeObs: tdCell.children('td:nth-child(0n+10)').html()
                            };
                            arrayPotencias.push(potenciaAuxi)
                        })

                    } else if (tipoPotencia == 1) {
                        let potenciaAuxi = {
                            PeriCodi: $("#PERIODO2 option:selected").val(),
                            CoresoCodi: coresoCodigo,
                            CoregeCodi: null,
                            TrnpcNumOrd: null,
                            TrnpcTipoCasoAgrupado: tipoCaso.AgrupacionVTA,
                            TrnPctTotalMwFija: tdCell.children('td:nth-child(0n+14)').html(),
                            TrnPctHpMwFija: tdCell.children('td:nth-child(0n+15)').html(),
                            TrnPctHfpMwFija: tdCell.children('td:nth-child(0n+16)').html(),
                            TrnPctTotalMwVariable: tdCell.children('td:nth-child(0n+17)').html(),
                            TrnPctHpMwFijaVariable: tdCell.children('td:nth-child(0n+18)').html(),
                            TrnPctHfpMwFijaVariable: tdCell.children('td:nth-child(0n+19)').html(),
                            TrnPctComeObs: tdCell.children('td:nth-child(0n+20)').html()
                        };
                        arrayPotencias.push(potenciaAuxi)

                    }
                }
            })

            if (arrayPotencias.length > 0) {
                $.ajax({
                    type: 'POST',
                    url: controler + "EnviarDatos",
                    data: JSON.stringify(arrayPotencias),
                    contentType: 'application/json; charset=utf-8',
                    success: function (evt) {

                        $('.mensajeLogin').hide();

                        if (evt.EsCorrecto < 0) {
                            if (evt.EsCorrecto == -10) {
                                $('.mensajeLogin').show();
                            } else {
                                alert(evt.Mensaje);
                            }
                            
                        } else {
                            $('#mensaje').show();
                            $('#mensaje').html('Los datos de códigos VTEA/VTP y potencias contratadas se enviaron correctamente.');
                            setTimeout(() => {

                                $('#mensaje').hide()
                            }, 4000)
                            mostrarListado(parseInt($('div.paginado-activo a > span').html()));
                        }
                    },
                    error: function () {
                        alert("Lo sentimos, ha ocurrido un error inesperado");
                    }
                });

            }

            console.log(arrayPotencias)
        } else {
            $('#popupZ2 #contenidoPopup').html(mensaje);
            setTimeout(function () {
                $('#popupZ2').bPopup({
                    easing: 'easeOutBack',
                    speed: 450,
                    transition: 'slideDown'
                });
            }, 50);
        }


    })
}

function verEnvios() {
    $('#btnVerEnvios').click(() => {
        $.ajax({
            type: 'POST',
            url: controler + "ListarEnvios",
            data: {
                periCodi: $("#PERIODO2 option:selected").val(),
            },
            success: function (evt, status, xhr) {
                $('#popupZ2 #popup-title span').html('Envíos realizados');
                $('#popupZ2 #contenidoPopup').html(evt);
                setTimeout(function () {
                    $('#popupZ2').bPopup({
                        easing: 'easeOutBack',
                        speed: 450,
                        transition: 'slideDown'
                    });
                }, 50);
            }
        })

    })

}
function decodeHtml(html) {
    var txt = document.createElement("textarea");
    txt.innerHTML = html;
    return txt.value;
}
function descargarArchivo(base64, filename, extension) {
    var binaryString = window.atob(base64); // Comment this if not using base64
    var bytes = new Uint8Array(binaryString.length);
    let buffer = bytes.map(function (byte, i) {
        return binaryString.charCodeAt(i);
    });

    if (extension === void 0) {
        extension = "pdf";
    }

    var blob = new Blob([buffer]);
    var fileNameRESULT = (extension == null) ? filename : filename + "." + extension;
    if (extension == 'omitir') {
        fileNameRESULT = filename;
    }
    if (navigator.msSaveBlob) {
        // IE 10+
        navigator.msSaveBlob(blob, fileNameRESULT);
    } else {
        var link = document.createElement("a");
        // Browsers that support HTML5 download attribute
        if (link.download !== undefined) {
            var url = URL.createObjectURL(blob);
            link.setAttribute("href", url);
            link.setAttribute("download", fileNameRESULT);
            link.style.visibility = "hidden";
            document.body.appendChild(link);
            link.click();
            document.body.removeChild(link);
        }
    }
}
function mpVerificarVTAChecked(targetInputVTA, coresoCodigo, arrayPotencias) {
    let targetInputVTPFirst = targetInputVTA.parent().next().next().next().children('input[type="checkbox"]')
    if (targetInputVTPFirst.prop('checked')) {
        let potenciaAuxi = {
            CoresoCodi: coresoCodigo,
            CoregeCodi: targetInputVTPFirst.parents('tr').attr('data-id-generado'),
            TrnpcNumOrd: arrayPotencias.filter(itemPot => itemPot.CoresoCodi == coresoCodigo).length + 1,
            TrnpcTipoCasoAgrupado: tipoCaso.AgrupacionVTP,
            PeriCodi: $("#PERIODO2 option:selected").val()
        };
        arrayPotencias.push(potenciaAuxi)
    }

    $('#listado table > tbody > tr[data-row-for-id="' + coresoCodigo + '"] td input.' + tipoCaso.AgrupacionVTP + ':checked').each((indexChild, targetChild) => {
        let targetInputVTPFirst = $(targetChild);
        let rowsPan = $(targetChild).parent().attr('rowspan') - 1;
        let potenciaAuxi = {
            CoresoCodi: coresoCodigo,
            CoregeCodi: targetInputVTPFirst.parents('tr').attr('data-id-generado'),
            TrnpcNumOrd: arrayPotencias.filter(itemPot => itemPot.CoresoCodi == coresoCodigo).length + 1,
            TrnpcTipoCasoAgrupado: tipoCaso.AgrupacionVTP,
            PeriCodi: $("#PERIODO2 option:selected").val()
        };
        arrayPotencias.push(potenciaAuxi)

        // Encontrar si existe proximos asociados
        if (parseInt(rowsPan) > 0) {

            let nexRow = targetInputVTPFirst.parents('tr');

            for (var i = 1; i <= parseInt(rowsPan); i++) {
                nexRow = nexRow.next();
                let potenciaAuxi = {
                    CoresoCodi: coresoCodigo,
                    CoregeCodi: nexRow.attr('data-id-generado'),
                    TrnpcNumOrd: arrayPotencias.filter(itemPot => itemPot.CoresoCodi == coresoCodigo).length + 1,
                    TrnpcTipoCasoAgrupado: tipoCaso.AgrupacionVTP,
                    PeriCodi: $("#PERIODO2 option:selected").val()
                };
                arrayPotencias.push(potenciaAuxi)
            }

        }
    })

}
//Funciones de eliminado de registro
function SolicitarBajaVTEA() {
    //$(".delete").click(function (e) {
    //    e.preventDefault();
    //    if (confirm("Desea solicitar dar de baja este código VTEA ?")) {
    //        id = $(this).attr("id").split("_")[1];
    //        //alert(id);
    //        $.ajax({
    //            type: "post",
    //            dataType: "text",
    //            url: controler + "Delete/" + id,
    //            data: AddAntiForgeryToken({ id: id }),
    //            success: function (resultado) {
    //                if (resultado == "true") {
    //                    alert("Operación realizada correctamente.");
    //                    buscar();
    //                }
    //                else
    //                    alert("No se ha logrado dar de baja el registro");
    //            }
    //        });
    //    }
    //});
};

function SolicitarBajaVTP() {
    $(".deletevtp").click(function (e) {
        e.preventDefault();
        if (confirm("Desea solicitar dar de baja este código VTP?")) {
            id = $(this).attr("id").split("_")[1];
            //alert(id);
            $.ajax({
                type: "post",
                dataType: "text",
                url: controler + "Deletevtp/" + id,
                data: AddAntiForgeryToken({ id: id }),
                success: function (resultado) {
                    if (resultado == "true") {
                        alert("Operación realizada correctamente.");
                        buscar();
                    }
                    else
                        alert("No se ha logrado eliminar el registro");
                }
            });
        }
    });
};

AddAntiForgeryToken = function (data) {
    data.__RequestVerificationToken = $('input[name=__RequestVerificationToken]').val();
    return data;
};

function ViewEvent() {
    $('.view').click(function (e) {
        e.preventDefault();
        id = $(this).attr("id").split("_")[1];
        //abrirPopup(id);

        $.ajax({
            type: 'POST',
            url: controler + "View/" + id,
            success: function (evt) {
                //$('#listaSuministro').html(evt);
                //add_deleteEvent();
            },
            error: function () {
                mostrarError();
            }
        });
    });
};