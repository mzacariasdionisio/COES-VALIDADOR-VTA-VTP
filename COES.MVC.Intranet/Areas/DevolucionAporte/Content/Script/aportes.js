var controlador = siteRoot + 'devolucionaporte/aportes/';

var aportantesTodos = [];
var datax = [];

$(document).ready(function () {
    pintarBusqueda(1);

    $("#cboAnioInversion").change(function () {
        ObtenerPresupuesto($(this).val());
    });

    $("#chkTodos").change(function () {
        if ($(this).is(":checked")) {
            aportantesTodos.forEach(function (item) {
                datax.push({
                    Aporcodi: item.Aporcodi,
                    Prescodi: $("#cboAnioInversion").val(),
                    Tabcdcodiestado: 4,
                    Aporaniosinaporte: item.Aporaniosinaporte,
                    Reprocesar: "1",
                    Eliminado: 0
                });
            });
        } else {
            datax = [];
            $('.reprocesar').each(function () {
                var aporcodi = $(this).val();
                var vaporcodi = aporcodi.split("_");
                $(this).prop("checked", false);

                $("#txtSinAportar_" + vaporcodi[0]).prop("disabled", "disabled");
            });
        }
    });
});

function calcular() {
    var tipo = document.getElementById("txtTipoCuota").value;
    var resultado = tipo * 3000;
    document.getElementById("txtCalcAuto").value = resultado.toString();
}

function seleccionar(aporcodi, estado) {
    if ($("#chkReProcesar_" + aporcodi).is(":checked")) {
        $("#mensaje").hide();
        if (estado == 5) {
            $("#txtSinAportar_" + aporcodi).removeAttr("disabled");
        }

        datax.push({
            Aporcodi: aporcodi,
            Prescodi: $("#cboAnioInversion").val(),
            Tabcdcodiestado: 4,
            Aporaniosinaporte: $("#txtSinAportar_" + aporcodi).val(),
            Reprocesar: "1",
            Eliminado: 0
        });
    } else {
        var idx = datax.findIndex(d => d.Aporcodi == aporcodi);

        if (estado == 5) {
            $("#txtSinAportar_" + aporcodi).prop("disabled", true);
        }

        if (idx > -1) {
            datax[idx].Eliminado = 1;
        }
    }
}

function anioSinAportar(aporcodi, obj) {
    var idx = datax.findIndex(d => d.Aporcodi == aporcodi);
    datax[idx].Aporaniosinaporte = $(obj).val();

}

function pintarBusqueda(nroPagina) {
    //$('#hfNroPagina').val(nroPagina);
    $.ajax({
        type: 'POST',
        url: controlador + "ListadoAportes",
        data: { prescodi: $("#cboAnioInversion").val() },
        success: function (evt) {
            $('#listado').css("width", $('#mainLayout').width() + "px");

            $('#listado').html(evt);

            $(evt).find('.fila').each(function () {
                var currentRow = $(this).closest("tr");
                var aporcodi = currentRow.find("td:eq(0)").html();

                var sinaportar = $(currentRow.find('td').eq(4)).find('#txtSinAportar_' + aporcodi).val();

                aportantesTodos.push({
                    Aporcodi: aporcodi,
                    Aporaniosinaporte: sinaportar
                });
            });

            $('#tablaAportes').dataTable({
                "scrollY": 430,
                "scrollX": false,
                'searching': true,
                "ordering": false,
                "pagingType": "full_numbers",
                "iDisplayLength": 50
            });

            $('#tablaAportes').on('page.dt', function () {
                var checked = $("#chkTodos").is(":checked");

                setTimeout(function () {
                    if (checked) {
                        seleccionarTodos();
                    }
                }, 100);
            });

            $("#chkTodos").change(function () {
                var checked = $(this).is(":checked");

                setTimeout(function () {
                    if (!checked) {
                        datax = [];
                    }

                    if (checked) {
                        seleccionarTodos();
                    }
                }, 100);
            });
        },
        error: function () {
            mostrarError();
        }
    });
}

function seleccionarTodos() {
    var rows, checked;
    rows = $('#tablaAportes').find('tbody tr');
    checked = $(this).prop('checked');

    $.each(rows, function () {
        var aporcodi = $($(this).find('td').eq(5)).find('input').val();
        var vaporcodi = aporcodi.split("_");

        var idx = datax.findIndex(d => d.Aporcodi == vaporcodi[0] && d.Eliminado == 1);

        if (idx == -1) {
            $($(this).find('td').eq(5)).find('input').prop('checked', $("#chkTodos").is(":checked"));
            seleccionar(vaporcodi[0], vaporcodi[1]);
        } else {
            $($(this).find('td').eq(5)).find('input').prop('checked', false);
            datax[idx].Eliminado = 1;
        }
    });
}

function ObtenerPresupuesto(iprescodi) {
    //$('#hfNroPagina').val(nroPagina);
    datax = [];
    aportantesTodos = [];
    $.ajax({
        type: 'POST',
        url: siteRoot + 'devolucionaporte/amortizaciones/ObtenerAmortizacion',
        data: { prescodi: iprescodi },
        success: function (data) {
            $("#txtMonto").val(data.Presmonto.toString().replace(/(\d)(?=(\d{3})+(?!\d))/g, "$1,"));
            $("#txtCuotas").val(data.Presamortizacion);
            pintarBusqueda(1);
        },
        error: function () {
            mostrarError();
        }
    });
}

function procesar() {
    if ($("#cboAnioInversion").val() == 0) {
        mostrarError("Debe seleccionar Año Inversión");
        return;
    }
    if ($("#txtTipoCuota").val() == '' || $("#txtTipoCuota").val() == 0) {
        mostrarError("Debe introducir un tipo de cambio.");
        return;
    }

    mostrarConfirmacion("", ProcesarData, "");
}

function onlyUnique(item, index, self) {
    return self.indexOf(item) === index;
}

function ProcesarData() {

    if (datax.filter(c => c.Eliminado == 0).length == 0) {
        $('#popupConfirmarOperacion').bPopup().close();
        mostrarError("Debe seleccionar un Aportante");
        return;
    } else {
        $("#mensaje").hide();
    }

    var eliminados = datax.filter(c => c.Eliminado == 1)

    eliminados.forEach(function (item) {
        var idx = datax.findIndex(d => d.Aporcodi == item.Aporcodi);
        datax.splice(idx, 1);
    })

    var aportantes = datax.filter(c => c.Eliminado == 0).map(function (a) { return a.Aporcodi; });
    aportantes = aportantes.filter(onlyUnique)

    var data = [];
    aportantes.forEach(function (item) {
        var idx = datax.findIndex(d => d.Aporcodi == item);

        data.push({
            Aporcodi: item,
            Prescodi: $("#cboAnioInversion").val(),
            Tabcdcodiestado: 4,
            Aporaniosinaporte: datax[idx].Aporaniosinaporte,
            Reprocesar: "1",
            Eliminado: 0
        });
    })

    var params = {
        listadoaportante: data,
        montoMin: $("#txtCalcAuto").val()
    };

    $.ajax({
        type: 'POST',
        contentType: 'application/json',
        url: controlador + "Procesar",
        data: JSON.stringify(params),
        success: function (data) {
            $('#popupConfirmarOperacion').bPopup().close();
            mostrarMensajePopup("Se realizo con exito la operación", "");
            pintarBusqueda(1);
            datax = [];
            $("#chkTodos").prop('checked', false);
        },
        error: function () {
            mostrarError();
        }
    });
}

function pintarCronograma(aporcodi) {
    //$('#hfNroPagina').val(nroPagina);
    $.ajax({
        type: 'POST',
        url: controlador + "ListadoCronograma",
        data: { aporcodi: aporcodi },
        success: function (evt) {
            $('#cronograma').css("width", $('#popupCronograma').width() + "px !important");

            $('#cronograma').html(evt);
            $('#tablaCronograma').dataTable({
                "scrollCollapse": true,
                "sDom": 't',
                "ordering": false,
                "iDisplayLength": 50,
                "columnDefs": [
                    {
                        "targets": [0],
                        "width": "5%",
                    },
                    {
                        "targets": [1],
                        "width": "5%",
                    },
                    {
                        "targets": [2],
                        "width": "5%"
                    },
                    {
                        "targets": [3],
                        "width": "5%"
                    },
                    {
                        "targets": [4],
                        "width": "5%"
                    },
                    {
                        "targets": [5],
                        "width": "5%"
                    },
                    {
                        "targets": [6],
                        "width": "5%"
                    },
                    {
                        "targets": [7],
                        "width": "5%"
                    }
                ],
                "fixedColumns": true
            });

            $("#cronograma").find('table').attr('style', 'width: 550px !important');
        },
        error: function () {
            mostrarError();
        }
    });
}

function VerCronograma(aporcodi) {
    $.ajax({
        type: 'POST',
        url: controlador + 'cronograma',
        data: { aporcodi: aporcodi },
        global: false,
        success: function (evt) {
            $('#contenidoCronograma').html(evt);
            setTimeout(function () {
                $('#popupCronograma').bPopup({
                    autoClose: false
                });

                pintarCronograma(aporcodi);
            }, 50);

            $('#popupCronograma').bPopup({
                follow: [false, false], //x, y
                position: [390, 50] //x, y
            });
        },
        error: function () {
            mostrarMensaje('mensaje', 'error', 'Se ha producido un error');
        }
    });
}

function descargarCronograma(aporcodi) {
    window.location = controlador + "ExportarCronograma?aporcodi=" + aporcodi;
    $("#cronograma").find('table').attr('style', 'width: 550px !important');
}


function descargarResumen() {
    window.location = controlador + "ExportarResumen?prescodi=" + $("#cboAnioInversion").val();
    $("#cronograma").find('table').attr('style', 'width: 550px !important');
}

mostrarExito = function () {
    $('#mensaje').removeClass();
    $('#mensaje').addClass('action-exito');
    $('#mensaje').html("La operación se realizó correctamente");
}

mostrarError = function (mensaje) {
    if (mensaje == null) mensaje = "";
    if (mensaje.length == 0) mensaje = "Ha ocurrido un error";

    $('#mensaje').removeClass();
    $('#mensaje').addClass('action-error');
    $('#mensaje').html(mensaje);
}

function exportarCronogramaMasivo() {

    var data = {
        Prescodi: $("#cboAnioInversion").val(),
    }

    $.ajax({
        type: 'POST',
        contentType: 'application/json',
        url: controlador + "ExportarCronogramaMasivo",
        data: JSON.stringify(data),
        success: function (data) {
            $('#popupConfirmarOperacion').bPopup().close();
            mostrarMensajePopup("Se realizo con exito la operación", "");
            descargarCronogramaMasivo();
            datax = [];
            $("#chkTodos").prop('checked', false);
        },
        error: function () {
            mostrarError();
        }
    });
}

function descargarCronogramaMasivo() {
    window.location = controlador + "DescargarCronogramaMasivo";
    $("#cronograma").find('table').attr('style', 'width: 550px !important');
}