
var controler = siteRoot + "transferencias/solicitudcodigo/";

$(document).ready(function () {

    var objTablaDetalle = new tablaDetalle();
    let txtesPrimerRegistro = $('#Entidad_esPrimerRegistro').val();

    let targetRaddio = $('[name="Entidad.TrnpcTipoCasoAgrupado"]:checked');

    if (targetRaddio.val() == 'AGRVTP') {
        let contenedorPotenciaTrans = $('[data-for-potencia="AGRVTA"] input[type="text"]').addClass('ignore');

        $('[data-for-potencia="' + targetRaddio.val() + '"]').show()

    } else {
        let contenedorPotenciaTrans = $('[data-for-potencia="AGRVTA"] input[type="text"]').removeClass('ignore');
        $('[data-for-potencia="' + targetRaddio.val() + '"]').show()


        if (parseInt(txtesPrimerRegistro) === 0) {

            $('td[data-for-potencia="AGRVTA"] input').prop('disabled', true);
            $('td[data-for-potencia="AGRVTA"] input').val('');

            $('.mensaje-pie').html('Para cambiar la potencia contratada ir al primer registro del agrupado.')
        }

    }


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

    $('#BARRCODISUM').multipleSelect({
        width: '200px',
        filter: true,
        selectAll: false,
        single: true,
        placeholder: '--Seleccione--'
    });

    $('.txtFecha').Zebra_DatePicker({
    });

    add_deleteEvent();
    SolicitarBajaVTP();

    $('#btnAgregar').click(function () {
        var idsum = $('select[id="BARRCODISUM"]').val();
        var nomsum = $("#BARRCODISUM option:selected").text();
        if (idsum == null) {
            alert("Selecciona la barra de suministro");
            return;
        }
        if (confirm("¿Está seguro agregar la barra de suministro?")) {


            let barraIdgenerado = [];

            $('#tabla tbody > tr').each((index, element) => {
                let trTarget = $(element);
                if (trTarget.attr('id') == 'fila_0') {
                    let numeroRegistro = 1;
                    let idRow = trTarget.attr('data-id-row');
                    barraIdgenerado.push({
                        IdGenerado: idRow,
                        NroRegistros: numeroRegistro,
                        IndexLista: index
                    })
                }
            })
            $.ajax({
                type: 'POST',
                url: controler + 'ActualizarIdGeneradoEdit',
                data: JSON.stringify(barraIdgenerado),
                contentType: 'application/json; charset=utf-8',
                dataType: 'JSON',
                success: () => {


                    let idGenerado = (new Date()).getTime();
                    $.ajax({
                        type: 'POST',
                        url: controler + "ListaDetalle",
                        data: {
                            idbarrsum: idsum, nombarrsum: nomsum, numreg: '1', tipo: 'A', idGenerado: idGenerado
                        },
                        success: function (evt) {

                            $('#listaSuministro').html(evt);
                            $('#tabla tbody > tr:last-child').attr('data-id-row', idGenerado)
                            let targetRaddio = $('[name="Entidad.TrnpcTipoCasoAgrupado"]:checked');
                            if (targetRaddio.val() == 'AGRVTP') {
                                objTablaDetalle.rowAdd({
                                    idRow: idGenerado,
                                    idsum: idsum,
                                    nomsum: nomsum,
                                    CoregeCodiPotcn: 0,

                                });
                            }


                            //---- fin -- //
                            //    }
                            //})


                        },
                        error: function () {
                            mostrarError();
                        }
                    });

                }
            })

        }

    });

    $('[name="Entidad.TrnpcTipoCasoAgrupado"]').click((event) => {
        if (parseInt(txtesPrimerRegistro) === 0) {
            event.preventDefault();
            event.stopPropagation();
            return false;
        }
        if (confirm("¿Al cambiar el tipo de potencia los datos almacenados de perderán?")) {

            let target = $(event.currentTarget);
            const isHIDDEN = $('[data-for-potencia="' + target.val() + '"]').is(':hidden');
            if (target.prop('checked') && isHIDDEN) {
               
                $('[data-for-potencia]').hide();
                $('[data-for-potencia="' + target.val() + '"]').show()
           
                if (target.val() == 'AGRVTP' ) {
                    $('[data-for-potencia="AGRVTA"] input[type="text"]').addClass('ignore');

                    objTablaDetalle._obtenerPotenciaVTPRegistradas().then((data) => {

                        data.map((item) => {
                            let codigoGenerado = item.CoregeCodi;
                            let param = item;
                            param.idsum = item.BarrCodi;
                            param.nomsum = item.BarrSuministro;
                            param.CoregeCodiPotcn = codigoGenerado;
                            param.idRow = codigoGenerado;

                            let existeEnListaBarras = 0;

                            $('#tabla tbody > tr > td.idcod').each((index, element) => {
                                if (codigoGenerado == $(element).html()) {
                                    existeEnListaBarras++;
                                }
                            })
                            if (existeEnListaBarras > 0) {
                                objTablaDetalle.rowAdd(param);
                            }

                        })
                        objTablaDetalle.agregarFilasOfBarraExistente();
                    }).catch(() => {

                    })



                } else {
                    objTablaDetalle.limpiarTabla();
                    $('[data-for-potencia="AGRVTA"] input[type="text"]').removeClass('ignore');
                }

            }
        } else {
            event.preventDefault();
            event.stopPropagation();
            return false;
        }
    })
    function add_deleteEvent() {

        $(document).on('click', "#tabla tbody > tr a.quitar", function (e) {
            e.preventDefault();


            if (confirm("¿Desea quitar la barrra de suministro?")) {
                var idsum = $('select[id="BARRCODISUM"]').val();
                var nomsum = $("#BARRCODISUM option:selected").text();
                var id = $(this).attr("id").split("_")[1];
                let trRow = $(this).parent().parent();
                let idRow = trRow.attr('data-id-row');
                let idcod = trRow.find('.idcod').html();

                $.ajax({
                    type: 'POST',
                    url: controler + "ListaDetalle",
                    data: {
                        idbarrsum: id, nombarrsum: nomsum, numreg: '1', tipo: 'D', idcod: idcod,
                        idGenerado: idRow
                    },
                    success: function (evt) {
                        $('#listaSuministro').html(evt);

                        objTablaDetalle.removeFila(idRow, idcod);

                    },
                    error: function () {
                        mostrarError();
                    }
                });
            }
        });
    };

    function SolicitarBajaVTP() {
        $(".bajar").click(function (e) {
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
                            location.reload();
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

    $("#btnGrabar").on("click", function () {

        let totalBarras = $('#tabla tbody tr').length;
        if (totalBarras == 0) {

            alert("Es necesario agregar una barra de suministro");
            return;
        }

        var index = 0;
        var det = "";
        var posicion = -1;
        $("#tabla tr").each(function () {

            if (index > 0) {
                posicion++;
                var iddet = $(this).find("td").eq(0).html();
                var idsum = $(this).find("td").eq(1).html();
                var idcod = $(this).find("td").eq(2).html();
                if (idcod == "0") {
                    if (det == "") {
                        det = iddet + '_' + idsum + '_' + idcod + '_' + posicion;
                    } else {
                        det = det + ',' + iddet + '_' + idsum + '_' + idcod + '_' + posicion;
                    }
                }
            }
            index++;

            $("#detalle").val(det)
        });

        $('#ListaPotenciasContratadasVTP').val(JSON.stringify(objTablaDetalle._enviarTablaServidor()));

        var error = 0;
        if ($('#rdbNivelCargaPotenciaSuministro').prop('checked')) {

            $('#tblAgrvtp tbody tr input').each((index, target) => {
                let elementHTML = $(target);

                if (elementHTML.val() == '') {
                    error++;
                    elementHTML.next().addClass('error-show');
                } else {
                    elementHTML.next().removeClass('error-show');
                }

            })
        }

        if (error == 0) {
            $("#submit").click();
        } else {
            alert('No se agregaron valores a la potencia contratada')
        }

    });

    $('#btnSolicitarCliente').click(() => {

        $.ajax({
            type: 'GET',
            url: controler + "SolicitudCliente",
            data: {
                empresaGeneradora: $('#Entidad_EmprNombre').val()
            },
            success: function (evt) {
                $('#popupZ2 #popup-title span').html('Solicitud de Creación de Cliente');
                $('#popupZ2 #contenidoPopup').html(evt);
                setTimeout(function () {
                    $('#popupZ2').bPopup({
                        easing: 'easeOutBack',
                        speed: 450,
                        transition: 'slideDown'
                    });
                    $('#contenidoPopup').css('overflow', 'hidden')
                }, 50);
            },
            error: function () {
                mostrarError();
            }
        });
    })


    $(document).on('click', '#btnGrabarSolicitudCliente', function (event) {
        if ($('#frmSolicitudCliente').valid()) {
            $.ajax({
                type: 'POST',
                url: controler + "solicitudCliente",
                data: $('#frmSolicitudCliente').serialize(),
                success: function (evt) {
                    if (evt) {
                        $('#btnCerrarSolicitudCliente').trigger('click')
                        alert(`Estimado, se le notificará por correo electrónico cuando el COES registre el nuevo cliente y pueda proseguir con el registro de solicitud de código.`);
                    }
                },
                error: function () {
                    mostrarError();
                }
            });
        }
    });

    $(document).on('blur', '#RucCliente', function (event) {
        $('#RazonSocial').val('')
        $.ajax({
            type: 'POST',
            url: controler + "obtenerdatos",
            data: { ruc: $('#RucCliente').val() },
            success: function (evt) {
                if (evt == -1) {
                    alert('Problemas con el servicio. Por favor digite el RUC y Razon Social');
                    $('#RazonSocial').removeClass('disable')
                    $('#RazonSocial').removeAttr('readonly')
                } else if (evt == -2) {
                    alert('Estimado Usuario. El Ruc ingresado no existe. Comuniquese con el area para mayor información.');
                } else if (evt == -3) {
                    alert('El RUC ingresado esta de baja');
                } else {
                    $('#RazonSocial').val(evt.RazonSocial)
                    $('#RazonSocial').valid();
                }

            },
            error: function () {
                alert('Problemas con el servicio. Por favor digite el RUC y Razon Social');
                $('#RazonSocial').removeClass('disable')
                $('#RazonSocial').removeAttr('readonly')
            }
        });
    });

    function tablaDetalle() {
        let input = $(`<div class="editor-field disabled">
                        <input  type="text" class="numeroDecimal" maxlength = "10" style='width:85px;margin:2px'/> 
                        <span class="error error-hide">*</span>
                       </div>`)
        let tbody = $('#tblAgrvtp tbody')

        this.limpiarTabla = () => {
            tbody.html('')
        }
        this.rowAdd = function (data) {
            let tr = $('<tr data-id-generado="' + data.CoregeCodiPotcn + '"></tr>')
            tr.attr('data-id-row', data.idRow)
            tr.attr('data-idsum', data.idsum)
            var td = $('<td></td>');
            td.html(data.nomsum)
            tr.append(td);
            //
            td = $('<td></td>');
            if (data.TrnPctHpMwFija != null) {
                let valorInput = input.clone();
                valorInput.find('input').val(data.TrnPctHpMwFija);
                td.append(valorInput)
            } else {
                td.append(input.clone())
            }
            tr.append(td);

            td = $('<td></td>');
            if (data.TrnPctHfpMwFija != null) {
                let valorInput = input.clone();
                valorInput.find('input').val(data.TrnPctHfpMwFija);
                td.append(valorInput)
            } else {
                td.append(input.clone())
            }
            tr.append(td);
            td = $('<td></td>');
            if (data.TrnPctTotalMwFija != null) {
                let valorInput = input.clone();
                valorInput.find('input').val(data.TrnPctTotalMwFija);
                td.append(valorInput)
            } else {
                td.append(input.clone())
            }
            tr.append(td);

            td = $('<td></td>');
            if (data.TrnPctHpMwFijaVariable != null) {
                let valorInput = input.clone();
                valorInput.find('input').val(data.TrnPctHpMwFijaVariable);
                td.append(valorInput)
            } else {
                td.append(input.clone())
            }
            tr.append(td);

            td = $('<td></td>');
            if (data.TrnPctHfpMwFijaVariable != null) {
                let valorInput = input.clone();
                valorInput.find('input').val(data.TrnPctHfpMwFijaVariable);
                td.append(valorInput)
            } else {
                td.append(input.clone())
            }
            tr.append(td);

            td = $('<td></td>');
            if (data.TrnPctTotalMwVariable != null) {
                let valorInput = input.clone();
                valorInput.find('input').val(data.TrnPctTotalMwVariable);
                td.append(valorInput)
            } else {
                td.append(input.clone())
            }
            tr.append(td);
            tbody.append(tr);

        }

        this.agregarFilasOfBarraExistente = function () {
            let totalFilas = $('#tblAgrvtp tbody tr').length;
            $('#tabla tbody > tr').each((index, element) => {
                let trTarget = $(element);
                let idsum = trTarget.find('.idsum').html();
                let idcod = trTarget.find('.idcod').html();
                let nomsum = trTarget.find('.idcod').next().html();

                if (idcod == 0 || totalFilas == 0) {
                    this.rowAdd({
                        idsum: idsum,
                        nomsum: nomsum,
                        CoregeCodiPotcn: 0,
                        idRow: trTarget.attr('data-id-row')
                    });
                }

            })
        }

        this.removeFila = function (idRow, idcod) {


            if (idcod == 0) {
                $('#tblAgrvtp [data-id-row="' + idRow + '"]').remove();
            } else {
                $('#tblAgrvtp [data-id-generado="' + idcod + '"]').remove();
            }

        }
        this._enviarTablaServidor = function () {
            var arrayPotencia = [];
            $('#tblAgrvtp tbody > tr').each((index, element) => {
                let idGenerado = $(element).attr('data-id-row');
                let tdCell = $(element)
                //aqui
                var idFila = $('#tabla [data-id-row="' + idGenerado + '"]').attr('id');

                let barrCodi = tdCell.attr('data-idsum');
                idGenerado = idFila == "fila_0" ? "0" : idGenerado;

                let potenciaAuxi = {
                    PeriCodi: $('#Entidad_PeridcCodi').val(),
                    BarrCodi: barrCodi,
                    CoresoCodi: $('#Entidad_SoliCodiRetiCodi').val(),
                    CoregeCodi: idGenerado,
                    TrnpcNumOrd: null,
                    TrnpcTipoCasoAgrupado: 'AGRVTP',
                    TrnPctHpMwFija: tdCell.children('td:nth-child(0n+2)').find('input').val(),
                    TrnPctHfpMwFija: tdCell.children('td:nth-child(0n+3)').find('input').val(),
                    TrnPctTotalMwFija: tdCell.children('td:nth-child(0n+4)').find('input').val(),
                    TrnPctHpMwFijaVariable: tdCell.children('td:nth-child(0n+5)').find('input').val(),
                    TrnPctHfpMwFijaVariable: tdCell.children('td:nth-child(0n+6)').find('input').val(),
                    TrnPctTotalMwVariable: tdCell.children('td:nth-child(0n+7)').find('input').val(),
                    TrnPctComeObs: null
                };
                arrayPotencia.push(potenciaAuxi);
            })

            return arrayPotencia;
        }

        this._obtenerPotenciaVTPRegistradas = () => {

            var promise = new Promise((resolve, reason) => {

                $.ajax({
                    type: 'POST',
                    url: controler + "ListarPotenciasContratadas",
                    data: {
                        coresocodi: $('#Entidad_SoliCodiRetiCodi').val(),
                        periCodi: $('#Entidad_PeridcCodi').val(),
                    },
                    success: function (evt) {
                        resolve(evt)
                    },
                    error: function () {

                        mostrarError();
                        reason();
                    }
                });

            });

            return promise;

        }
    }
    mostrarError = function () {
        alert("Ha ocurrido un error.");
    }
});
