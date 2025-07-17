var controlador = siteRoot + "intercambioOsinergmin/remisiones/";
var table;
var radio;

$(function () {

    $('.rbTipo').click(function () {
        mostrarTipo();
    });

    $('#btnRemitir').click(function () {
        remitirTodo();
    });

    $('#tab-container').easytabs({
        animate: false
    });

    $('#tab-container-secundario').easytabs({
        animate: false
    });

    $('#btnCancelar').click(function () {
        cancelar();
    });

    pintarBusqueda();
    procesarArchivos();
});

function cancelar() {
    window.location.href = controlador + "Index";
}

mostrarTipo = function () {

    var tipo = $('input[name=rbTipo]:checked').val();

    if (tipo == "X") {
        radio = "x";
    }

    if (tipo == "C") {
        radio = "c";
    }
    if (tipo == "Z") {
        radio = "z";
    }

}

descargarArchivo = function (url) {
    document.location.href = controlador + "download?url=" + url;
}

procesarArchivos = function () {
    $.ajax({
        type: 'POST',
        url: controlador + 'archivos',
        data: {
            periodo: $("#PeriodoRemisionModel_Periodo").val()
        },
        success: function (evt) {
            $('#listaArchivos').html(evt);
            $('#tbDocumentLibrary').dataTable({
                "ordering": true,
                "paging" : false               
            });

        },
        error: function () {

        }
    });
}

function checkAll(ele) {
    var checkboxes = document.getElementsByTagName('input');
    if (ele.checked) {
        for (var i = 0; i < checkboxes.length; i++) {
            if (checkboxes[i].type == 'checkbox') {
                checkboxes[i].checked = true;
            }
        }
    } else {
        for (var i = 0; i < checkboxes.length; i++) {
            if (checkboxes[i].type == 'checkbox') {
                checkboxes[i].checked = false;
            }
        }
    }
}

var pintarBusqueda =
    function () {
        if ($("#PeriodoRemisionModel_Periodo").val() === null) return;
        $.ajax({
            type: "POST",
            url: controlador + "listarEntidades",
            data: {
                periodo: $("#PeriodoRemisionModel_Periodo").val()
            },
            success: function (evt) {
                $("#listado").html(evt);
                table = $("#tabla").DataTable({
                    "scrollY": 850,
                    "scrollX": false,
                    "sDom": "t",
                    "ordering": false,
                    "bDestroy": true,
                    "bPaginate": false,
                    "iDisplayLength": 50
                });
                if ($("#remitir").length) {
                    $("#remitir").click(remitir);
                }
            },
            error: function (xhr) {
                mostrarError("Ocurrio un problema >> readyState: " + xhr.readyState + " | status: " + xhr.status + " | responseText: " + xhr.responseText);
            }
        });
    };

function mostrarError(mensaje) {
    $('#mensaje').removeClass();
    $('#mensaje').html(mensaje);
    $('#mensaje').addClass('action-error');
}

function mostrarExito(mensaje) {
    $('#mensaje').removeClass();
    $('#mensaje').html(mensaje);
    $('#mensaje').addClass('action-exito');
}

function remitirRegistro(id) {
    $.ajax({
        type: 'POST',
        url: controlador + 'remitirRegistroIndividual',
        data: {
            periodo: $("#PeriodoRemisionModel_Periodo").val(),
            tabla: id
        },
        dataType: 'json',
        success: function (result) {
            if (result.resultado == 0) {
                mostrarExito(result.mensaje);
            } else {
                mostrarError(result.mensaje);
            }
            pintarBusqueda();
            procesarArchivos();
        },
        error: function () {
            mostrarError('Ha ocurrido un error: Envío a Osinergmin - 3');
        }
    });
}

function remitirTodo() {
    var cadena = "";
    var checkboxes = document.getElementById('tbSeleccionados').getElementsByTagName('input');
    for (var i = 0; i < checkboxes.length; i++) {
        if (checkboxes[i].type == 'checkbox' && checkboxes[i].checked == true) {
            var valor = checkboxes[i].id;
            if (cadena == "") {
                cadena = valor;
            }
            else {
                cadena = cadena + "," + valor;
            }
        }
    }

    if (cadena != "") {
        $.ajax({
            type: 'POST',
            url: controlador + 'remitirTodo',
            data: {
                periodo: $("#PeriodoRemisionModel_Periodo").val(),
                cadena: cadena
            },
            dataType: 'json',
            success: function (result) {
                if (result.resultado == 0) {
                    mostrarExito(result.mensaje);
                } else {
                    mostrarError(result.mensaje);
                }
                pintarBusqueda();
                procesarArchivos();
            },
            error: function () {
                mostrarError('Ha ocurrido un error: Envio Osinergmin 4');
            }
        });
    } else {
        mostrarError('Por favor seleccione al menos un registro');
    }


}

function descargarLog(tab, id) {
    $.ajax({
        type: 'POST',
        url: controlador + 'Exportar',
        data: {
            periodo: $("#PeriodoRemisionModel_Periodo").val(),
            tabla: tab,
            idEnvio: id
        },
        dataType: 'json',
        success: function (result) {
            if (result != -1) {
                document.location.href = controlador + 'descargar?formato=1&file=' + result
            }
            else {
                mostrarMensaje('mensaje', 'Ha ocurrido un error', $tipoMensajeError, $modoMensajeCuadro);
            }
        },
        error: function () {
            mostrarMensaje('mensaje', 'Ha ocurrido un error', $tipoMensajeError, $modoMensajeCuadro);
        }
    });
}

function obtenerFile(tab) {

    $.ajax({
        type: 'POST',
        url: controlador + 'ExportarArchivo',
        data: {
            periodo: $("#PeriodoRemisionModel_Periodo").val(),
            tabla: tab,
            tipo: radio
        },
        dataType: 'json',
        success: function (result) {
            if (result != -1) {
                document.location.href = controlador + "downloadIntercambio?url=" + result;
            }
            else {
                mostrarMensaje('mensaje', 'Ha ocurrido un error', $tipoMensajeError, $modoMensajeCuadro);
            }
        },
        error: function () {
            mostrarMensaje('mensaje', 'Ha ocurrido un error', $tipoMensajeError, $modoMensajeCuadro);
        }
    });

}