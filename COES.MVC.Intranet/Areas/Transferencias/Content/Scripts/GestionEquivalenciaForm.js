var controler = siteRoot + "transferencias/GestionEquivalenciaVTEAVTP/";
$(document).ready(function () {

    iniciarEventos();
})




function inicializarDatatable() {
    $('#detalleVTEA').dataTable({
        data: null,
        "columns": [
            { "data": "Cliemprnombvtea" },
            { "data": "Barrnombvtea" },
            { "data": "Codigovtea" },
            {
                "data": null, render: () => {
                    return ` <a href="javascript:void(0)" class="quitar"><img src="../../../Content/Images/btn-cancel.png" title="Quitar" alt="Quitar" /></a>`;
                }
            },
            { "data": "Genemprcodivtea", visible: false },
            { "data": "Cliemprcodivtea", visible: false },
            { "data": "Barrcodivtea", visible: false },
            { "data": "Coresovtea", visible: false }
        ],
        "bAutoWidth": false,
        "sDom": 't',
        "ordering": false,
        scrollY: "90px",
        paging: false,
        "scrollX": "100%",
        "sScrollXInner": "100%",
        "drawCallback": function (settings) {

            $('#detalleVTEA tr a.quitar').unbind('click')
            $('#detalleVTEA tr a.quitar').click((event) => {
                mostrarConfirmacion("¿Estas seguro en quitar el codigo?",
                    function () {
                        let rowSelect = $(event.currentTarget).parents('tr');
                        let rowData = $('#detalleVTEA').DataTable().rows(rowSelect).data();
                        $('#detalleVTEA').DataTable().rows(rowSelect).remove().draw();
                        $('.dataTables_scrollHeadInner').css('width', '100%')
                        $('.dataTables_scrollHeadInner   table').css('width', '100%')
                        $('#popupConfirmarOperacion').bPopup().close();
                    }, "");
            })
        }
    });

    $('#detalleVTP').dataTable({
        data: null,
        "columns": [
            { "data": "Cliemprnombvtp" },
            { "data": "Barrnombvtp" },
            //{ "data": "CoregeCodVTP" }, 
            { "data": "Codigovtp" },
            {
                "data": null, render: () => {
                    return ` <a href="javascript:void(0)" class="quitar"><img src="../../../Content/Images/btn-cancel.png" title="Quitar" alt="Quitar" /></a>`;
                }
            },
            { "data": "Genemprcodivtp", visible: false },
            { "data": "Cliemprcodivtp", visible: false },
            { "data": "Barrcodivtp", visible: false },
            { "data": "Coresovtp", visible: false }
            //{ "data": "CoregeCodi", visible: false }
        ],
        "bAutoWidth": false,
        "sDom": 't',
        "ordering": false,
        scrollY: "90px",
        paging: false,
        "scrollX": "100%",
        "sScrollXInner": "100%",
        "drawCallback": function (settings) {
            $('#detalleVTP tr a.quitar').unbind('click')
            $('#detalleVTP tr a.quitar').click((event) => {
                mostrarConfirmacion("¿Estas seguro en quitar el codigo?",
                    function () {
                        let rowSelect = $(event.currentTarget).parents('tr');
                        let rowData = $('#detalleVTP').DataTable().rows(rowSelect).data();
                        $('#detalleVTP').DataTable().rows(rowSelect).remove().draw();
                        $('.dataTables_scrollHeadInner').css('width', '100%')
                        $('.dataTables_scrollHeadInner   table').css('width', '100%')
                        $('#popupConfirmarOperacion').bPopup().close();
                    }, "");
            })
        }
    });

    $('.dataTables_scrollHeadInner').css('width', '100%')
    $('.dataTables_scrollHeadInner   table').css('width', '100%')


}

function iniciarEventos() {
    inicializarDatatable();
    // VTEA
    $('#EMPRVTEA').change((event) => {

        $('#CLIEVTEA').find('option:not(:first-child)').remove();
        $('#BARTRAVTEA').find('option:not(:first-child)').remove();
        $('#CODIVTEA').find('option:not(:first-child)').remove();

        let loadCombobox = $('#CLIEVTEA');
        loadCombobox.find('option:first-child').text('Cargando....');
        loadCombobox.prop('disabled', true);

        cargarComboboxCliente($(event.currentTarget).val() == "" ? 0 : $(event.currentTarget).val()).then((data) => {
            $('#CLIEVTEA').find('option:not(:first-child)').remove();
            data.map((item) => {
                $('#CLIEVTEA').append(`<option value="${item.Value}">${item.Text}</option>`)
            })
            loadCombobox.prop('disabled', false);
            loadCombobox.find('option:first-child').text('--Seleccione--');
        }).catch((error) => {
            loadCombobox.prop('disabled', false);
            loadCombobox.find('option:first-child').text('--Seleccione--');
            alert(error)
        })
    })

    $('#CLIEVTEA').change((event) => {

        $('#BARTRAVTEA').find('option:not(:first-child)').remove();
        $('#CODIVTEA').find('option:not(:first-child)').remove();


        let loadCombobox = $('#BARTRAVTEA');
        loadCombobox.find('option:first-child').text('Cargando....');
        loadCombobox.prop('disabled', true);
        let genemprcodi = $('#EMPRVTEA').val() == "" ? 0 : $('#EMPRVTEA').val();
        let cliemprcodi = $(event.currentTarget).val() == "" ? 0 : $(event.currentTarget).val();
        cargarComboboxBarraTrans(genemprcodi, cliemprcodi).then((data) => {
            $('#BARTRAVTEA').find('option:not(:first-child)').remove();
            data.map((item) => {
                $('#BARTRAVTEA').append(`<option value="${item.Value}">${item.Text}</option>`)
            })
            loadCombobox.prop('disabled', false);
            loadCombobox.find('option:first-child').text('--Seleccione--');
        }).catch((error) => {
            loadCombobox.prop('disabled', false);
            loadCombobox.find('option:first-child').text('--Seleccione--');
            alert(error)
        })
    })

    $('#BARTRAVTEA').change((event) => {
        if ($(event.currentTarget).val() == "") {
            $('#CODIVTEA').find('option:not(:first-child)').remove();
        }

        let loadCombobox = $('#CODIVTEA');
        loadCombobox.find('option:first-child').text('Cargando....');
        loadCombobox.prop('disabled', true);
        let genemprcodi = $('#EMPRVTEA').val() == "" ? 0 : $('#EMPRVTEA').val();
        let cliemprcodi = $('#CLIEVTEA').val() == "" ? 0 : $('#CLIEVTEA').val();
        let barrcodi = $(event.currentTarget).val() == "" ? 0 : $(event.currentTarget).val();
        cargarComboboxVTEA(genemprcodi, cliemprcodi, barrcodi).then((data) => {
            $('#CODIVTEA').find('option:not(:first-child)').remove();
            data.map((item) => {
                let option = $(`<option value="${item.SoliCodiRetiCodi}">${item.SoliCodiRetiCodigo}</option>`);
                option.data('itemVTEA', item)
                $('#CODIVTEA').append(option)
            })
            loadCombobox.prop('disabled', false);
            loadCombobox.find('option:first-child').text('--Seleccione--');
        }).catch((error) => {
            loadCombobox.prop('disabled', false);
            loadCombobox.find('option:first-child').text('--Seleccione--');
            alert(error)
        })
    })
    //===========================================================
    //-- VTP
    //===========================================================
    $('#EMPRVTP').change((event) => {

        $('#CLIEVTP').find('option:not(:first-child)').remove();
        $('#BARSUM').find('option:not(:first-child)').remove();
        $('#CODIVTP').find('option:not(:first-child)').remove();

        let loadCombobox = $('#CLIEVTP');
        loadCombobox.find('option:first-child').text('Cargando....');
        loadCombobox.prop('disabled', true);
        cargarComboboxCliente($(event.currentTarget).val() == "" ? 0 : $(event.currentTarget).val()).then((data) => {
            $('#CLIEVTP').find('option:not(:first-child)').remove();
            data.map((item) => {
                $('#CLIEVTP').append(`<option value="${item.Value}">${item.Text}</option>`)
            })
            loadCombobox.prop('disabled', false);
            loadCombobox.find('option:first-child').text('--Seleccione--');
        }).catch((error) => {
            loadCombobox.prop('disabled', false);
            loadCombobox.find('option:first-child').text('--Seleccione--');
            alert(error)

        })
    })

    $('#CLIEVTP').change((event) => {

        $('#BARSUM').find('option:not(:first-child)').remove();
        $('#CODIVTP').find('option:not(:first-child)').remove();

        let loadCombobox = $('#BARSUM');
        loadCombobox.find('option:first-child').text('Cargando....');
        loadCombobox.prop('disabled', true);
        let genemprcodi = $('#EMPRVTP').val() == "" ? 0 : $('#EMPRVTP').val();
        let cliemprcodi = $(event.currentTarget).val() == "" ? 0 : $(event.currentTarget).val();
        cargarComboboxBarraSum(genemprcodi, cliemprcodi).then((data) => {
            $('#BARSUM').find('option:not(:first-child)').remove();
            data.map((item) => {
                $('#BARSUM').append(`<option value="${item.Value}">${item.Text}</option>`)
            })
            loadCombobox.prop('disabled', false);
            loadCombobox.find('option:first-child').text('--Seleccione--');
        }).catch((error) => {
            loadCombobox.prop('disabled', false);
            loadCombobox.find('option:first-child').text('--Seleccione--');
            alert(error)
        })
    })

    $('#BARSUM').change((event) => {
        $('#CODIVTP').find('option:not(:first-child)').remove();
        let loadCombobox = $('#CODIVTP');
        loadCombobox.find('option:first-child').text('Cargando....');
        loadCombobox.prop('disabled', true);
        let genemprcodi = $('#EMPRVTP').val() == "" ? 0 : $('#EMPRVTP').val();
        let cliemprcodi = $('#CLIEVTP').val() == "" ? 0 : $('#CLIEVTP').val();
        let barrcodi = $(event.currentTarget).val() == "" ? 0 : $(event.currentTarget).val();
        cargarComboboxVTP(barrcodi, genemprcodi, cliemprcodi).then((data) => {
            $('#CODIVTP').find('option:not(:first-child)').remove();
            data.map((item) => {
                let option = $(`<option value="${item.CoregeCodi}">${item.CoregeCodVTP}</option>`);
                option.data('itemVTP', item)
                $('#CODIVTP').append(option)
            })

            loadCombobox.prop('disabled', false);
            loadCombobox.find('option:first-child').text('--Seleccione--');
        }).catch((error) => {
            loadCombobox.prop('disabled', false);
            loadCombobox.find('option:first-child').text('--Seleccione--');
            alert(error)
        })
    })

    //Agregar VTEA
    $('#btnAgregarVTEA').click(() => {
        let error = false;
        var idemp = $('select[id="EMPRVTEA"]').val();
        var emp = $("#EMPRVTEA option:selected").text();
        var idcli = $('select[id="CLIEVTEA"]').val();
        var cli = $("#CLIEVTEA option:selected").text();
        var idbar = $('select[id="BARTRAVTEA"]').val();
        var bar = $("#BARTRAVTEA option:selected").text();

        let itemVTEA = $('#CODIVTEA').find('option:selected').data('itemVTEA');

        var data = {
            'Cliemprnombvtea': cli,
            'Barrnombvtea': bar,
            'Codigovtea': itemVTEA.SoliCodiRetiCodigo,
            'Genemprcodivtea': idemp,
            'Cliemprcodivtea': idcli,
            'Barrcodivtea': idbar,
            'Coresovtea': itemVTEA.SoliCodiRetiCodi
        }

        if (typeof itemVTEA == 'undefined') {
            error = true;
            mostrarMensajeError("El codigo VTEA no ha sido seleccionado.");

        } else {

            $('#detalleVTEA').DataTable().rows()[0].map((index) => {
                let rowData = $('#detalleVTEA').DataTable().row(index).data();

                if (rowData.Coresovtea == itemVTEA.SoliCodiRetiCodi) {
                    error = true;
                    mostrarMensajeError("El codigo VTEA ingresado se encuentra asignado.");
                }
            })
            if (!error) {

                existevtea(itemVTEA.SoliCodiRetiCodi).then((result) => {
                    console.log(result);
                    if (result == "False") {
                        $('#detalleVTEA').DataTable().row.add(data).draw();
                        $('.dataTables_scrollHeadInner').css('width', '100%')
                        $('.dataTables_scrollHeadInner   table').css('width', '100%')
                    } else {
                        mostrarMensajeError("Código VTEA ya pertenece a una relación de equivalencia");
                    }

                }).catch((error) => {
                    alert(error)
                })

            }

        }

    })

    $('#btnAgregarVTP').click(() => {
        let error = false;
        var idemp = $('select[id="EMPRVTP"]').val();
        var emp = $("#EMPRVTP option:selected").text();
        var idcli = $('select[id="CLIEVTP"]').val();
        var cli = $("#CLIEVTP option:selected").text();
        var idbar = $('select[id="BARSUM"]').val();
        var bar = $("#BARSUM option:selected").text();
        let itemVTP = $('#CODIVTP').find('option:selected').data('itemVTP');

        var data = {
            'Cliemprnombvtp': cli,
            'Barrnombvtp': bar,
            'Codigovtp': itemVTP.CoregeCodVTP,
            'Genemprcodivtp': idemp,
            'Cliemprcodivtp': idcli,
            'Barrcodivtp': idbar,
            'Coresovtp': itemVTP.CoregeCodi
        }

        if (typeof itemVTP == 'undefined') {
            error = true;
            mostrarMensajeError("El codigo VTP no ha sido seleccionado.");

        } else {
            $('#detalleVTP').DataTable().rows()[0].map((index) => {
                let rowData = $('#detalleVTP').DataTable().row(index).data();
                if (rowData.Coresovtp == itemVTP.CoregeCodi) {
                    error = true;
                    mostrarMensajeError("El codigo VTP ingresado se encuentra asignado.");
                }
            })
            if (!error) {
                existevtp(itemVTP.CoregeCodi).then((result) => {
                    console.log(result);
                    if (result == "False") {
                        $('#detalleVTP').DataTable().row.add(data).draw();
                        $('.dataTables_scrollHeadInner').css('width', '100%')
                        $('.dataTables_scrollHeadInner   table').css('width', '100%')
                    } else {
                        mostrarMensajeError("Código VTP ya pertenece a una relación de equivalencia");
                    }

                }).catch((error) => {
                    alert(error)
                })



            }
        }
    })

    //Grabar
    $('#btnGrabar').click(() => {
        let intVTEA = $('#detalleVTEA').DataTable().rows()[0].length;
        let intVTP = $('#detalleVTP').DataTable().rows()[0].length;
        var vari = document.getElementById('retrelvari').value;
        console.log(vari);
        let error = false;
        $('#btnGrabar').prop('disabled', true);
        if (intVTEA == 0) {
            error = true;
            $('#btnGrabar').prop('disabled', false);
            mostrarMensajeError("Es necesario ingresar codigos VTEA.");

        }
        else if (intVTP == 0) {
            error = true;
            $('#btnGrabar').prop('disabled', false);
            mostrarMensajeError("Es necesario ingresar codigos VTP.");
        }
        
        else if  (vari == 0 || vari == null) {
            $('#btnGrabar').prop('disabled', false);
            mostrarMensajeError("Es necesario ingresar la variación");
            return;
        }
        if (!error) {

            var tableVTEA = $('#detalleVTEA').DataTable();
            var data = tableVTEA.rows().data();
            var cadenaVtea = "";
            data.each(function (obj, index) {
                if (cadenaVtea == "") {
                    cadenaVtea = obj["Genemprcodivtea"] + "_" + obj["Cliemprcodivtea"] + "_" + obj["Barrcodivtea"] + "_" + obj["Coresovtea"]
                } else {
                    cadenaVtea = cadenaVtea + "," + obj["Genemprcodivtea"] + "_" + obj["Cliemprcodivtea"] + "_" + obj["Barrcodivtea"] + "_" + obj["Coresovtea"]
                }
            });

            var tableVTP = $('#detalleVTP').DataTable();
            var data2 = tableVTP.rows().data();
            var cadenaVtp = "";
            data2.each(function (obj, index) {
                if (cadenaVtp == "") {
                    cadenaVtp = obj["Genemprcodivtp"] + "_" + obj["Cliemprcodivtp"] + "_" + obj["Barrcodivtp"] + "_" + obj["Coresovtp"]
                } else {
                    cadenaVtp = cadenaVtp + "," + obj["Genemprcodivtp"] + "_" + obj["Cliemprcodivtp"] + "_" + obj["Barrcodivtp"] + "_" + obj["Coresovtp"]
                }
            });

            
            var id = $('input:hidden[name=RetrelCodi]').val()

            

            $.ajax({
                type: 'POST',
                url: controler + "RegistrarEquivalencia",
                data: {
                    cadenaVtea: cadenaVtea,
                    cadenaVtp: cadenaVtp,
                    variacion: vari,
                    id: id
                },
                dataType: 'json',
                success: function (evt) {
                    //$('#popupEq').bPopup().close();
                    $('#btnGrabar').prop('disabled', false);
                    $('#popupMensajeZ #btnAceptarMsj').hide();
                    $('#popupMensajeZ #cmensaje').html('<div class="exito mensajes">Registro Exitoso</div>');
                    setTimeout(function () {
                        $('#popupMensajeZ').bPopup({
                            easing: 'easeOutBack',
                            speed: 450,
                            transition: 'slideDown',
                        });
                    }, 50);
                    //location.reload();
                    window.location.href = controler;
                },
                error: function () {
                    $('#btnGrabar').prop('disabled', false);
                    alert("Lo sentimos, ha ocurrido un error inesperado");
                }
            });

        }
    })

}

function mostrarMensajeError(mensaje) {
    $('#popupMensajeZ #btnAceptarMsj').hide();
    $('#popupMensajeZ #cmensaje').html('<div class="error mensajes">' + mensaje + '</div>');
    setTimeout(function () {
        $('#popupMensajeZ').bPopup({
            easing: 'easeOutBack',
            speed: 450,
            transition: 'slideDown',
        });
    }, 50);

}

function cargarComboboxCliente(emprcodi) {

    var pro = new Promise((resolve, reject) => {

        $('#loading').addClass('cancel')
        $.ajax({
            type: 'POST',
            url: controler + "ListaInterCoReSoCliPorEmpresa",
            data: {
                emprcodi: emprcodi
            },
            success: function (evt) {
                resolve(evt)
            },
            error: function () {

                reject("Lo sentimos, ha ocurrido un error inesperado");
            }
        });
    })
    return pro;
}
function cargarComboboxBarraTrans(genemprcodi, clienemprcodi) {
    var pro = new Promise((resolve, reject) => {

        $('#loading').addClass('cancel')
        $.ajax({
            type: 'POST',
            url: controler + "ListaInterCoReSoByEmpr",
            data: {
                genemprcodi: genemprcodi,
                clienemprcodi: clienemprcodi
            },
            success: function (evt) {
                resolve(evt)
            },
            error: function () {

                $('#loading').removeClass('cancel')
                reject("Lo sentimos, ha ocurrido un error inesperado");
            }
        });
    })
    return pro;
}
function cargarComboboxBarraSum(genemprcodi, clienemprcodi) {
    var pro = new Promise((resolve, reject) => {

        $('#loading').addClass('cancel')
        $.ajax({
            type: 'POST',
            url: controler + "ListaInterCoReGeByEmpr",
            data: {
                genemprcodi: genemprcodi,
                cliemprcodi: clienemprcodi
            },
            success: function (evt) {
                resolve(evt)
            },
            error: function () {

                $('#loading').removeClass('cancel')
                reject("Lo sentimos, ha ocurrido un error inesperado");
            }
        });
    })
    return pro;
}
function cargarComboboxVTEA(genemprcodi, cliemprcodi, barrcodi) {
    var pro = new Promise((resolve, reject) => {

        $('#loading').addClass('cancel')
        $.ajax({
            type: 'POST',
            url: controler + "ListarCodigoVTEAByEmprBarr",
            data: {
                genemprcodi: genemprcodi,
                cliemprcodi: cliemprcodi,
                barrcodi: barrcodi
            },
            success: function (evt) {
                resolve(evt)
            },
            error: function () {

                $('#loading').removeClass('cancel')
                reject("Lo sentimos, ha ocurrido un error inesperado");
            }
        });
    })
    return pro;
}
function cargarComboboxVTP(barrcodisum, genemprcodi, cliemprcodi) {
    var pro = new Promise((resolve, reject) => {
        $('#loading').addClass('cancel')
        $.ajax({
            type: 'POST',
            url: controler + "ListarCodigosVTPByEmpBar",
            data: {
                barrcodisum: barrcodisum,
                genemprcodi: genemprcodi,
                cliemprcodi: cliemprcodi
            },
            success: function (evt) {
                resolve(evt)
            },
            error: function () {

                $('#loading').removeClass('cancel')
                reject("Lo sentimos, ha ocurrido un error inesperado");
            }
        });
    })
    return pro;
}

mostrarError = function () {
    alert("Ha ocurrido un error.");
}


function cargarDetalle() {

    var id = $('input:hidden[name=RetrelCodi]').val()

    $.ajax({
        type: 'POST',
        url: controler + "ListaDetalle",
        data: {
            id: id
        },
        success: function (evt) {
            if (evt != null) {
                console.log(evt);
                evt.forEach(function (obj, indice) {
                    console.log(obj["Genemprnombvtea"]);
                    var datavtea = {};
                    if (obj["Cliemprnombvtea"] != null) {
                        datavtea = {
                            'Cliemprnombvtea': obj["Cliemprnombvtea"],
                            'Barrnombvtea': obj["Barrnombvtea"],
                            'Codigovtea': obj["Codigovtea"],
                            'Genemprcodivtea': obj["Genemprcodivtea"],
                            'Cliemprcodivtea': obj["Cliemprcodivtea"],
                            'Barrcodivtea': obj["Barrcodivtea"],
                            'Coresovtea': obj["Coresocodvtea"]
                        }
                        $('#detalleVTEA').DataTable().row.add(datavtea).draw();
                        $('.dataTables_scrollHeadInner').css('width', '100%')
                        $('.dataTables_scrollHeadInner   table').css('width', '100%')
                    }
                    var datavtp = {};
                    if (obj["Cliemprnombvtp"] != null) {
                        datavtp = {
                            'Cliemprnombvtp': obj["Cliemprnombvtp"],
                            'Barrnombvtp': obj["Barrnombvtp"],
                            'Codigovtp': obj["Codigovtp"],
                            'Genemprcodivtp': obj["Genemprcodivtp"],
                            'Cliemprcodivtp': obj["Cliemprcodivtp"],
                            'Barrcodivtp': obj["Barrcodivtp"],
                            'Coresovtp': obj["Coresocodvtp"]
                        }
                        $('#detalleVTP').DataTable().row.add(datavtp).draw();
                        $('.dataTables_scrollHeadInner').css('width', '100%')
                        $('.dataTables_scrollHeadInner   table').css('width', '100%')
                    }
                });
            }
        },
        error: function () {
            alert("Lo sentimos, ha ocurrido un error inesperado");
        }
    });
}

function existevtea(id) {
    var pro = new Promise((resolve, reject) => {
        $('#loading').addClass('cancel')
        $.ajax({
            type: 'POST',
            url: controler + "ExisteVTEA",
            data: {
                id: id
            },
            success: function (evt) {
                resolve(evt)
            },
            error: function () {

                $('#loading').removeClass('cancel')
                reject("Lo sentimos, ha ocurrido un error inesperado");
            }
        });
    })
    return pro;
}

function existevtp(id) {
    var pro = new Promise((resolve, reject) => {
        $('#loading').addClass('cancel')
        $.ajax({
            type: 'POST',
            url: controler + "ExisteVTP",
            data: {
                id: id
            },
            success: function (evt) {
                resolve(evt)
            },
            error: function () {

                $('#loading').removeClass('cancel')
                reject("Lo sentimos, ha ocurrido un error inesperado");
            }
        });
    })
    return pro;
}