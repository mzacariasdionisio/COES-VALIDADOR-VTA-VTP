var controler = siteRoot + "transferencias/GestionCodigosVTEAVTP/";
const tipoCaso = {

    AgrupacionVTA: "AGRVTA",
    AgrupacionVTP: "AGRVTP"

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

$(document).ready(function () {

    /* pintarPaginado();*/
    agruparEnviarServidor();
    desagruparEnviarServidor();
    exportarDatos();
    importarDatos();
    btnVerErrores();
    enviarDatos();
    verEnvios();
    validarPeriodo(() => {

        /* buscar();*/
    });
    $('#PERIODO2').change(() => {
        validarPeriodo();
    })

    $('.txtFecha').Zebra_DatePicker({
    });

    $('#btnBuscar').click(() => {
        buscar();
    })
    $('#popupZ2 #contenidoPopup').css({
        'overflow-y': 'scroll',
        'height': '90%'
    })
    $(document).on("click", '#listado table#tabla td > a.delete', function (event) {
        var id = $(event.currentTarget).attr('id').split('_')[1];
        var opcion = confirm("¿Estas seguro en dar de baja la solicitud con sus VTP asociados?");
        if (opcion == true) {
            $.ajax({
                type: 'POST',
                url: controler + "Baja",
                data: {
                    iCoReSoCodi: id
                },
                success: function (evt) {
                    if (evt.EsCorrecto == 1) {
                        $('#popupMensajeZ #btnAceptarMsj').hide();
                        $('#popupMensajeZ #cmensaje').html('<div class="exito mensajes">' + evt.Mensaje + '</div>');
                        setTimeout(function () {
                            $('#popupMensajeZ').bPopup({
                                easing: 'easeOutBack',
                                speed: 450,
                                transition: 'slideDown',
                            });
                        }, 50);
                        mostrarListado($('#paginaActual').val());
                        buscar()
                    } else {
                        alert(evt.Mensaje)
                    }

                },
                error: function () {
                    alert("Lo sentimos, ha ocurrido un error inesperado");
                }
            });

        }
    });


    $(document).on("click", '#listado table#tabla td > a.deleteVtp', function (event) {
        var idCoreso = $(event.currentTarget).attr('id').split('_')[1];
        var idCorege = $(event.currentTarget).attr('id').split('_')[2];
        var opcion = confirm("¿Estas seguro en dar de baja el codigo VTP?");
        if (opcion == true) {
            $.ajax({
                type: 'POST',
                url: controler + "BajaVTP",
                data: {
                    iCoReSoCodi: idCoreso,
                    iCoregeCodi: idCorege
                },
                success: function (evt) {
                    if (evt.EsCorrecto == 1) {
                        $('#popupMensajeZ #btnAceptarMsj').hide();
                        $('#popupMensajeZ #cmensaje').html('<div class="exito mensajes">' + evt.Mensaje + '</div>');
                        setTimeout(function () {
                            $('#popupMensajeZ').bPopup({
                                easing: 'easeOutBack',
                                speed: 450,
                                transition: 'slideDown',
                            });
                        }, 50);
                        //mostrarListado($('#paginaActual').val());
                        buscar()
                    } else {
                        alert(evt.Mensaje)
                    }

                },
                error: function () {
                    alert("Lo sentimos, ha ocurrido un error inesperado");
                }
            });

        }
    });


    $('#tab-container').easytabs({
        animate: false
    });

    /*
     * Histórico
     */
    //buscarHis();

    $('#btnBuscarHis').click(function () {
        buscarHis();
    });

    $('#btnGenerarExcelHis').click(function () {
        generarExcelHis();
    });

    buscarDefault();
})



function buscarDefault() {
    if (_genemprcodi !== undefined && _genemprcodi != null && _genemprcodi.length > 0) {
        buscar();
    }
}

function buscar() {
    mostrarListado($('#paginaActual').val());    
}

function pintarPaginado(callback) {
    $.ajax({
        type: 'POST',
        url: controler + "paginado",
        data: {
            periCodi: $('#PERIODO2').val(),
            paginadoActual: $('#paginaActual').val(),
            genemprcodi: $('#EMPRCODI2').val(),
            clicodi: $('#CLICODI2').val(),
            tipocont: $('#TIPOCONTCODI2').val(),
            tipousu: $('#TIPOUSUACODI2').val(),
            barrcodi: $('#BARRCODI2').val(),
            barrcodisum: $('#BARRCODI3').val(),
            coresoestado: $('#ESTCODSOL').val(),
            coregecodvteavtp: $('#txtCodigoVTEAVTP').val(),
            fechaIni: $('#txtfechaIni').val(),
            fechaFin: $('#txtfechaFin').val(),
            NroPagina: $('#hfNroPagina').val()
        }, success: function (evt) {
            $('#paginado').html(evt);
            //mostrarPaginado();
            if (callback != null)
                callback();
        },
        error: function () {
            alert("Lo sentimos, ha ocurrido un error inesperado");
        }
    });
}

function mostrarListado(nroPagina) {
    console.log($('#PERIODO2').val())
    $('#hfNroPagina').val(nroPagina);
    $.ajax({
        type: 'POST',
        url: controler + "Lista",
        data: {
            genemprcodi: $('#EMPRCODI2').val(),
            clicodi: $('#CLICODI2').val(),
            tipocont: $('#TIPOCONTCODI2').val(),
            tipousu: $('#TIPOUSUACODI2').val(),
            barrcodi: $('#BARRCODI2').val(),
            barrcodisum: $('#BARRCODI3').val(),
            coresoestado: $('#ESTCODSOL').val(),
            coregecodvteavtp: $('#txtCodigoVTEAVTP').val(),
            periCodi: $('#PERIODO2').val(),
            //fechaIni: $('#txtfechaIni').val(),
            //fechaFin: $('#txtfechaFin').val(),
            NroPagina: 1,
            coresoestapr: $('#ESTCODSOLAPR').val(),

        },
        success: function (evt) {
            $('#listado').html(evt);

            agruparPotenciasCheckbox();
            mpHover();
            if ($('#ESTCODSOLAPR').val() == 'PEN') {
                $('#divAcciones').css('display', 'none');
            } else {
                $('#divAcciones').css('display', 'block');
            }
            

        },
        error: function () {
            alert("Lo sentimos, ha ocurrido un error inesperado");
        }
    });
}

function pintarBusqueda(nroPagina) {
    mostrarListado(nroPagina);
}

/*
 * Inicio: Histórico
 */
function buscarHis() {
    pintarPaginadoHis();
    mostrarListadoHis(1);
}

function pintarBusquedaHis(nroPagina){
    mostrarListadoHis(nroPagina);
}

function pintarPaginadoHis() {
    $.ajax({
        type: 'POST',
        url: controler + "paginadoHis",
        data: { nombreEmp: $("#EMPRCODI option:selected").text(), tipousu: $("#TIPOUSUACODI option:selected").text(), tipocont: $("#TIPOCONTCODI option:selected").text(), barr: $("#BARRCODI option:selected").text(), clinomb: $("#CLICODI option:selected").text(), fechaInicio: $('#txtfechaIni').val(), fechaFin: $('#txtfechaFin').val(), Solicodiretiobservacion: $('[name="SOLICODIRETIOBSERVACION"]:radio:checked').val(), radiobtn: $('[name="ESTADOLIST"]:radio:checked').val(), codretiro: $('#txtCodigoRetiro').val() },
        success: function (evt) {
            $('#paginadoHis').html(evt);
            mostrarPaginadoHis();
        },
        error: function () {
            alert("Lo sentimos, ha ocurrido un error inesperado");
        }
    });
}

function mostrarListadoHis(nroPagina) {
    
    $('#hfNroPaginaHis').val(nroPagina);
    $.ajax({
        type: 'POST',
        url: controler + "listaHis",
        data: { nombreEmp: $("#EMPRCODI option:selected").text(), tipousu: $("#TIPOUSUACODI option:selected").text(), tipocont: $("#TIPOCONTCODI option:selected").text(), barr: $("#BARRCODI option:selected").text(), clinomb: $("#CLICODI option:selected").text(), fechaInicio: $('#txtfechaIni').val(), fechaFin: $('#txtfechaFin').val(), Solicodiretiobservacion: $('[name="SOLICODIRETIOBSERVACION"]:radio:checked').val(), radiobtn: $('[name="ESTADOLIST"]:radio:checked').val(), codretiro: $('#txtCodigoRetiro').val(), NroPagina: $('#hfNroPaginaHis').val() },
        success: function (evt) {

            $('#listadoHis').css("width", $('#mainLayout').width() + "px");
            $('#listadoHis').html(evt);
            viewEvent();
            $('#tablaHis').dataTable({
                "scrollY": 430,
                "scrollX": true,
                "sDom": 't',
                "ordering": false,
                "iDisplayLength": 50
            });

        },
        error: function () {
            alert("Lo sentimos, ha ocurrido un error inesperado");
        }
    });
}

function viewEvent() {

    $('.view').click(function () {
        id = $(this).attr("id").split("_")[1];
        abrirPopup(id);
    });
};

abrirPopup = function (id) {

    $.ajax({
        type: 'POST',
        url: controler + "viewHis/" + id,
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

mostrarError = function () {
    alert("Ha ocurrido un error.");
}

generarExcelHis = function () {
    $.ajax({
        type: 'POST',
        url: controler + 'generarexcelHis',
        dataType: 'json',
        success: function (result) {
            if (result == 1) {
                window.location = controler + 'abrirexcelHis';
            }
            if (result == -1) {
                mostrarError();
            }
        },
        error: function () {
            mostrarError();
        }
    });
}
/*
 * Fin: Histórico
 */


//============ Potencias


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
                var tr = $(target).parent().parent().find('td:nth-child(0n+3)');
                var trEstado = $(target).parent().parent().find('td:nth-child(0n+9)');

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
                    if (indexTD >= 16 && indexTD <= 21) {
                        let tdVTA = $(targetTD);
                        let valor = tdVTA.html().trim();
                        if (valor == '') {
                            if (errorIndex == 0) {
                                mensaje += `El cliente <mark>${tdVTA.parent().find('td:nth-child(0n+3)').html()}</mark> con la barra <mark>${tdVTA.parent().find('td:nth-child(0n+11)').html()}</mark> tiene valores sin ingresar potencias.<br/><br/>`;
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
                    if (indexTD >= 5 && indexTD <= 10) {

                        let tdVTA = $(targetTD);
                        let valor = tdVTA.html().trim();
                        if (valor == '') {
                            if (errorIndex == 0) {
                                mensaje += `El cliente <mark>${tdVTA.parent().find('td:nth-child(0n+3)').html()}</mark> con la barra <mark>${tdVTA.parent().find('td:nth-child(0n+11)').html()}</mark> tiene valores sin ingresar potencias.<br/><br/>`;
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
                            //let coresoCodigoChild = $(targetChild).attr('id');
                            let coresoCodigoChild = coresoCodigo;
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
                    if (evt.EsCorrecto < 0) {
                        alert(evt.Mensaje);
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


validarPeriodo = function (callback) {

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
            if (callback != null) {
                callback();
            }
        },
        error: function () {
            //mostrarError();
        }
    });

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
                    if (evt.EsCorrecto < 0) {
                        alert(evt.Mensaje);
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
                genemprcodi: $('#EMPRCODI2').val(),
                clicodi: $('#CLICODI2').val(),
                tipocont: $('#TIPOCONTCODI2').val(),
                tipousu: $('#TIPOUSUACODI2').val(),
                barrcodi: $('#BARRCODI2').val(),
                barrcodisum: $('#BARRCODI3').val(),
                coresoestado: $('#ESTCODSOL').val(),
                coregecodvteavtp: $('#txtCodigoVTEAVTP').val(),
                periCodi: $('#PERIODO2').val()
                //fechaIni: $('#txtfechaIni').val(),
                //fechaFin: $('#txtfechaFin').val(),
                //NroPagina: $('#hfNroPagina').val()

            },
            success: function (evt) {
                if (evt.EsCorrecto < 0) {
                    alert(evt.Mensaje);
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
                    genemprcodi: $('#EMPRCODI2').val(),
                    clicodi: $('#CLICODI2').val(),
                    tipocont: $('#TIPOCONTCODI2').val(),
                    tipousu: $('#TIPOUSUACODI2').val(),
                    barrcodi: $('#BARRCODI2').val(),
                    barrcodisum: $('#BARRCODI3').val(),
                    coresoestado: $('#ESTCODSOL').val(),
                    coregecodvteavtp: $('#txtCodigoVTEAVTP').val(),
                    periCodi: $('#PERIODO2').val(),
                    NroPagina: 1,
                    //fechaIni: $('#txtfechaIni').val(),
                    //fechaFin: $('#txtfechaFin').val(),
                    //NroPagina: $('#hfNroPagina').val()
                    base64: archivoBase64,
                    NroPagina: 1
                },

                success: function (evt, status, xhr) {
                    var ct = xhr.getResponseHeader("content-type") || "";
                    if (ct.indexOf('html') > -1) {

                        $('#listado').html(evt);
                        agruparPotenciasCheckbox();
                        mpHover();

                        $('#mensaje').show();
                        $('#mensaje').html('Se ha cargado correctamente la información de potencias contratadas, por favor enviar datos para confirmar.');

                    }
                    if (ct.indexOf('json') > -1) {
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

                },
                error: function () {
                    alert("Lo sentimos, ha ocurrido un error inesperado");
                }
            });
        };


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



function enviarDatos() {

    $('#btnEnviarDatos').click(() => {
        var arrayPotencias = [];
        var mensaje = "";
        var error = false;
        var auxiliar = ''

        $('#listado table > tbody > tr[data-excel="1"]').each((index, targetTR) => {
            let errorIndex = 0;

            let valor = '';
            let estadoVTA = '';


            if ($(targetTR).find('td').length > 12) {
                estadoVTP = $(targetTR).find('td:nth-child(0n+16) span');

                //console.log($(targetTR).html())
                //console.log('::::::::::::::::::')
                //console.log(estadoVTP.html())
                valor = estadoVTP.html()

                if (valor != 'Baja'
                    && valor != 'Rechazado') {
                    $(targetTR).find('td').each((index, target) => {
                        var index = $(target).index();
                        if (index >= 16 && index <= 21) {
                            let valor = $(target).html().trim();
                            if (valor == '') {
                                if (errorIndex == 0) {
                                    mensaje += `El cliente <mark>${$(target).parent().find('td:nth-child(0n+3)').html()}</mark> con la barra <mark>${$(target).parent().find('td:nth-child(0n+11)').html()}</mark> tiene valores sin ingresar potencias.<br/><br/>`;
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
                    var indexTDAux = 4;
                    var indexFinal = 9;
                    if ($(targetTD).parent().find('td').length == 4) {


                        valor = $(targetTD).parent().find('td:nth-child(0n+3) span').html();
                    } else {
                        indexTDAux++;
                        indexFinal++;
                        valor = $(targetTD).parent().find('td:nth-child(0n+4) span').html();
                    }


                    if (valor.toUpperCase() != 'Baja'
                        && valor.toUpperCase() != 'Rechazado') {
                        if (indexTD >= indexTDAux && indexTD <= indexFinal) {

                            let tdVTA = $(targetTD);
                            let valor = tdVTA.html().trim();
                            if (valor == '') {
                                if (errorIndex == 0) {
                                    mensaje += `El cliente <mark>${tdVTA.parent().find('td:nth-child(0n+3)').html()}</mark> con la barra <mark>${tdVTA.parent().find('td:nth-child(0n+11)').html()}</mark> tiene valores sin ingresar potencias.<br/><br/>`;
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
                            TrnPctTotalMwFija: tdCell.children('td:nth-child(0n+17)').html(),
                            TrnPctHpMwFija: tdCell.children('td:nth-child(0n+18)').html(),
                            TrnPctHfpMwFija: tdCell.children('td:nth-child(0n+19)').html(),
                            TrnPctTotalMwVariable: tdCell.children('td:nth-child(0n+20)').html(),
                            TrnPctHpMwFijaVariable: tdCell.children('td:nth-child(0n+21)').html(),
                            TrnPctHfpMwFijaVariable: tdCell.children('td:nth-child(0n+22)').html(),
                            TrnPctComeObs: tdCell.children('td:nth-child(0n+23)').html()
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
                                TrnPctTotalMwFija: tdCell.children('td:nth-child(0n+6)').html(),
                                TrnPctHpMwFija: tdCell.children('td:nth-child(0n+7)').html(),
                                TrnPctHfpMwFija: tdCell.children('td:nth-child(0n+8)').html(),
                                TrnPctTotalMwVariable: tdCell.children('td:nth-child(0n+9)').html(),
                                TrnPctHpMwFijaVariable: tdCell.children('td:nth-child(0n+10)').html(),
                                TrnPctHfpMwFijaVariable: tdCell.children('td:nth-child(0n+11)').html(),
                                TrnPctComeObs: tdCell.children('td:nth-child(0n+12)').html()
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
                            TrnPctTotalMwFija: tdCell.children('td:nth-child(0n+17)').html(),
                            TrnPctHpMwFija: tdCell.children('td:nth-child(0n+18)').html(),
                            TrnPctHfpMwFija: tdCell.children('td:nth-child(0n+19)').html(),
                            TrnPctTotalMwVariable: tdCell.children('td:nth-child(0n+20)').html(),
                            TrnPctHpMwFijaVariable: tdCell.children('td:nth-child(0n+21)').html(),
                            TrnPctHfpMwFijaVariable: tdCell.children('td:nth-child(0n+22)').html(),
                            TrnPctComeObs: tdCell.children('td:nth-child(0n+23)').html()
                        };
                        arrayPotencias.push(potenciaAuxi)


                    }
                }
            })

            console.log(arrayPotencias)
            if (arrayPotencias.length > 0) {
                $.ajax({
                    type: 'POST',
                    url: controler + "EnviarDatos",
                    data: JSON.stringify(arrayPotencias),
                    contentType: 'application/json; charset=utf-8',
                    success: function (evt) {
                        if (evt.EsCorrecto < 0) {
                            alert(evt.Mensaje);
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

            //console.log(arrayPotencias)
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

function mpHover() {
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
mostrarError = function () {
    alert("Ha ocurrido un error.");
}