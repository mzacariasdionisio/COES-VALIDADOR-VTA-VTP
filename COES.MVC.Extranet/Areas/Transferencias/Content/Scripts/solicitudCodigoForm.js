var controler = siteRoot + "transferencias/solicitudcodigo/";

$(document).ready(function () {
    var objTablaDetalle = new tablaDetalle();
    add_deleteEvent();
    //====================================================
    //-- Load
    //======================================================

    $('[data-for-potencia="1"] input[type="text"]').addClass('ignore');

    //
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

    $('select[id="BARRCODI2"]').change(function () {
        var val = $(this).val();

        $.ajax({
            type: 'POST',
            url: controler + "ListaSuministro",
            data: { bartran: val, barsum: '0', tipo: 'L' },
            success: function (evt) {
                $('#listaSuministro').html(evt);

                $('[data-for-potencia]').hide();
                objTablaDetalle.limpiarTabla();
                $('[name="Entidad.TrnpcTipoCasoAgrupado"]').prop('checked', false)
            },
            error: function (e) {
                console.log(e);
                mostrarError();
            }
        });
    });

    $('#btnAgregar').click(function () {
        var idsum = $('select[id="BARRCODISUM"]').val();
        var idtra = $('select[id="BARRCODI2"]').val();
        var nomsum = $('select[id="BARRCODISUM"] option:selected').text();
        if (idtra == null) {
            alert("Seleccione la barra de transferencia");
            return;
        }
        if (idsum == null) {
            alert("Seleccione la barra de suministro");
            return;
        }
        if (confirm("¿Está seguro de agregar la barra de suministro?")) {
            let idGenerado = (new Date()).getTime();


            let barraIdgenerado = [];
            $('#tabla tbody > tr').each((index, element) => {
                let trTarget = $(element);
                let numeroRegistro = trTarget.find('.nro > input').val();
                let idRow = trTarget.attr('data-id-row');
                let idsum = trTarget.find('.id').html();

                barraIdgenerado.push({
                    BareCodi: idsum,
                    IdGenerado: idRow,
                    NroRegistros: numeroRegistro
                })
            })

            $.ajax({
                type: 'POST',
                url: controler + 'ActualizarIdGenerado',
                data: JSON.stringify(barraIdgenerado),
                contentType: 'application/json; charset=utf-8',
                dataType: 'JSON',
                success: () => {

                    $.ajax({
                        type: 'POST',
                        url: controler + "ListaSuministro",
                        data: { bartran: idtra, barsum: idsum, nombarrsum: nomsum, tipo: 'A', idGenerado: idGenerado },
                        success: function (evt) {
                            $('#listaSuministro').html(evt);

                            $('#tabla tbody > tr:last-child').attr('data-id-row', idGenerado)

                            let targetRaddio = $('[name="Entidad.TrnpcTipoCasoAgrupado"]:checked');

                            if (targetRaddio.val() == 'AGRVTP') {
                                let rowSuministroAgrvtp = $('#tblAgrvtp tbody tr[data-idsum="' + idsum + '"]');
                                let total = rowSuministroAgrvtp.length;
                                if (total == 0) {

                                    objTablaDetalle.rowAdd({
                                        idRow: idGenerado,
                                        idsum: idsum,
                                        nomsum: nomsum,
                                        CoregeCodiPotcn: 0,
                                        index: 1

                                    });


                                }

                            }
                        },
                        error: function () {
                            mostrarError();
                        }
                    });

                }
            })


        }

    });

    //event persistente se crea el evento asi el elemento exista o nos
    $(document).on('change', ".numeric", function (event) {

        var valor = $(event.currentTarget).val();
        console.log(valor)
        let idsum = $(event.currentTarget).parent().parent().find('td.id').html();
        let nomsum = $(event.currentTarget).parent().parent().find('td.nombre').html();
        let index = 0;
        let error = false;

        let total = $('#tblAgrvtp tbody tr[data-idsum="' + idsum + '"]').length;

        if (total < parseInt(valor)) {

            for (var i = 1; i <= valor; i++) {
                let existe = false;
                let rowSuministroAgrvtp = $('#tblAgrvtp tbody tr[data-idsum="' + idsum + '"]');
                rowSuministroAgrvtp.each((iSum, targetSum) => {
                    if (iSum + 1 == i) {
                        existe = true;
                    }
                })
                if (!existe) {
                    let idGenerado = (new Date()).getTime();
                    objTablaDetalle.rowAddAfter({
                        idRow: idGenerado,
                        idsum: idsum,
                        nomsum: nomsum,
                        CoregeCodiPotcn: 0,
                        index: i
                    });
                }

            }

        } else {
            error = true;
        }

        if (error) {
            let iniDelete = parseInt(valor) + 1;
            for (var i = iniDelete; i <= total; i++) {
                $('#tblAgrvtp tbody tr[data-idsum="' + idsum + '"][data-index="' + i + '"]').remove();
            }

        }

    })


    $('#btnBorrar').click(function () {
        var idsum = $('select[id="BARRCODISUM"]').val();
        var idtra = $('select[id="BARRCODI2"]').val();
        var nomsum = $('select[id="BARRCODISUM"] option:selected').text();

        if (confirm("¿Está seguro quitar todas las barras de suministro?")) {

            let barraIdgenerado = [];
            $('#tabla tbody > tr').each((index, element) => {
                let trTarget = $(element);
                let numeroRegistro = trTarget.find('.nro > input').val();
                let idRow = trTarget.attr('data-id-row');
                let idsum = trTarget.find('.id').html();
                barraIdgenerado.push({
                    BareCodi: idsum,
                    IdGenerado: idRow,
                    NroRegistros: numeroRegistro
                })
            })
            $.ajax({
                type: 'POST',
                url: controler + 'ActualizarIdGenerado',
                data: JSON.stringify(barraIdgenerado),
                contentType: 'application/json; charset=utf-8',
                dataType: 'JSON',
                success: () => {
                    $.ajax({
                        type: 'POST',
                        url: controler + "ListaSuministro",
                        data: { bartran: idtra, barsum: idsum, nombarrsum: nomsum, tipo: 'B' },
                        success: function (evt) {
                            $('#listaSuministro').html(evt);
                            objTablaDetalle.limpiarTabla();
                        },
                        error: function () {
                            mostrarError();
                        }
                    });
                }
            })

        }

    });

    function add_deleteEvent() {

        $(document).on('click', "#tabla tbody > tr a.quitar", function (e) {
            e.preventDefault();
            if (confirm("¿Desea quitar la barrra de suministro?")) {
                var idtra = $('select[id="BARRCODI2"]').val();
                var id = $(this).attr("id").split("_")[1];
                let trRow = $(this).parent().parent();
                let idRow = trRow.attr('data-id-row');

                let barraIdgenerado = [];
                $('#tabla tbody > tr').each((index, element) => {
                    let trTarget = $(element);
                    let numeroRegistro = trTarget.find('.nro > input').val();
                    let idRow = trTarget.attr('data-id-row');
                    let idsum = trTarget.find('.id').html();
                    barraIdgenerado.push({
                        BareCodi: idsum,
                        IdGenerado: idRow,
                        NroRegistros: numeroRegistro
                    })
                })
                $.ajax({
                    type: 'POST',
                    url: controler + 'ActualizarIdGenerado',
                    data: JSON.stringify(barraIdgenerado),
                    contentType: 'application/json; charset=utf-8',
                    dataType: 'JSON',
                    success: () => {

                        $.ajax({
                            type: 'POST',
                            url: controler + "ListaSuministro",
                            data: {
                                bartran: idtra,
                                barsum: id,
                                tipo: 'D',
                                idGenerado: idRow
                            },
                            success: function (evt) {
                                $('#listaSuministro').html(evt);
                                objTablaDetalle.removeFila(idRow, 0);
                                $('#tblAgrvtp tbody > tr[data-idsum="' + id + '"]').remove();
                            },
                            error: function () {
                                mostrarError();
                            }
                        });
                    }
                })

            }
        });
    };

    function validarFechas() {
        var fechaInicio = $("#Solicodiretifechainicio").val();
        var fechaInicioValida = $("#SolicodiretifechainicioValida").val();
        var fechaFin = $("#Solicodiretifechafin").val();
        if (FormatFechaValida(fechaInicio) < FormatFechaValida(fechaInicioValida)) {
            alert('Verificar que la fecha de inicio corresponda al periodo actual de declaración');
            return false;
        }
        if (FormatFechaValida(fechaFin) <= FormatFechaValida(fechaInicio)) {
            alert('La fecha Fin debe ser mayor que la fecha de Inicio de Operación.');
            return false;
        }
        return true;
    }

    function FormatFechaValida(sFecha) {   //DD/MM/AAAA
        var sDia = sFecha.substring(0, 2);
        var sMes = sFecha.substring(3, 5);
        var sAnio = sFecha.substring(6, 10);
        return new Date(sAnio, sMes - 1, sDia);
    }


    $("#btnGrabar").on("click", function () {

        if (!validarFechas()) {
            return;
        }

        let totalBarras = $('#tabla tbody tr').length;
        if (totalBarras == 0) {
            alert("Es necesario agregar una barra de suministro");
            return;
        }

       

        //let checked = $('[name="Entidad.TrnpcTipoCasoAgrupado"]:checked').length;
        var index = 0;
        var det = "";
        //if (checked == 0) {
        //    alert('No se ha seleccionado un tipo de potencia contratada')
        //} else {
            $("#tabla tr").each(function () {

                if (index > 0) {
                    var id = $(this).find("td").eq(0).html();
                    console.log(id);
                    var nro = $(this).find("td").find("#item_NroRegistros").val();
                    if (det == "") {
                        det = id + '_' + nro;
                    } else {
                        det = det + ',' + id + '_' + nro;
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
                    console.log(elementHTML.val())
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

        //}

    });

    $('[name="Entidad.TrnpcTipoCasoAgrupado"]').click((event) => {
        let target = $(event.currentTarget);
        const isHIDDEN = $('[data-for-potencia="' + target.val() + '"]').is(':hidden');
        if (target.prop('checked') && isHIDDEN) {


            if (target.val() == 'AGRVTP') {
                let totalBarras = $('#tabla tbody tr').length;
                if (totalBarras == 0) {

                    alert('Es necesario tener barras de suministros asignadas')
                    event.preventDefault();
                    event.stopPropagation();
                    return false;
                } else {
                    $('[data-for-potencia]').hide();
                    $('[data-for-potencia="' + target.val() + '"]').show()
                    let contenedorPotenciaTrans = $('[data-for-potencia="AGRVTA"] input[type="text"]').addClass('ignore');
                    objTablaDetalle.agregarFilasOfBarraExistente();

                }

            } else {
                $('[data-for-potencia]').hide();
                $('[data-for-potencia="' + target.val() + '"]').show()
                objTablaDetalle.limpiarTabla();
                let contenedorPotenciaTrans = $('[data-for-potencia="AGRVTA"] input[type="text"]').removeClass('ignore');
            }

        }

    })


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
            tr.attr('data-index', data.index);
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

        this.rowAddAfter = function (data) {
            let tr = $('<tr data-id-generado="' + data.CoregeCodiPotcn + '"></tr>')
            tr.attr('data-id-row', data.idRow)
            tr.attr('data-idsum', data.idsum)
            tr.attr('data-index', data.index);

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


            var trRoot = tbody.find('tr[data-idsum="' + data.idsum + '"]');

            trRoot.each((index, target) => {
                if (index == trRoot.length - 1) {
                    tr.insertAfter($(target))
                }
            })



        }
        //.

        this.agregarFilasOfBarraExistente = function () {
            let totalFilas = $('#tblAgrvtp tbody tr').length;
            var barraIdgenerado = [];

            $('#tabla tbody > tr').each((index, element) => {
                let idGenerado = (new Date()).getTime() + index;
                let trTarget = $(element);
                let idRow = idGenerado;
                let idsum = trTarget.find('.id').html();
                let idcod = 0;
                let nomsum = trTarget.find('.nombre').html();
                let numeroRegistro = trTarget.find('.nro > input').val();
                trTarget.attr('data-id-row', idRow)
                if (idcod == 0 || totalFilas == 0) {
                    barraIdgenerado.push({
                        BarrNombSum: nomsum,
                        BareCodi: idsum,
                        IdGenerado: idRow,
                        NroRegistros: numeroRegistro
                    })
                    this.rowAdd({
                        idRow: idRow,
                        idsum: idsum,
                        nomsum: nomsum,
                        CoregeCodiPotcn: idcod
                    });
                }
            })

            barraIdgenerado.map((item, index) => {
                var inicio = 1;
                let idGenerado = (new Date()).getTime() + index;
                for (var i = inicio; i <= item.NroRegistros - 1; i++) {
                    barraIdgenerado.push({
                        BareCodi: item.BareCodi,
                        IdGenerado: idGenerado,
                        NroRegistros: -1
                    })
                    this.rowAddAfter({
                        idRow: idGenerado,
                        idsum: item.BareCodi,
                        nomsum: item.BarrNombSum,
                        CoregeCodiPotcn: 0
                    });
                }
            })


            $.ajax({
                type: 'POST',
                url: controler + 'ActualizarIdGenerado',
                data: JSON.stringify(barraIdgenerado),
                contentType: 'application/json; charset=utf-8',
                dataType: 'JSON',
                success: () => {


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
                let idGenerado = 0;
                let tdCell = $(element)
                let barrCodi = tdCell.attr('data-idsum');

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
