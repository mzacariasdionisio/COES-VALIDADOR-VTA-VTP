var controlador = siteRoot + 'interconexiones/informe/';

$(function () {

    $('#txtAnio').Zebra_DatePicker({
        format: 'Y',
        onSelect: function (date) {
            cargarSemanas(date);
            limpiarTabla();
        }
    });

    $('#btnConsultar').on('click', function () {
        consultar();
    });

    $('#btnNuevo').on('click', function () {
        nuevaVersion();
    });

    $('#btnAntecedentes').on('click', function () {
        document.location.href = controlador + "antecedentes";
    });

    $('#btnConfirmar').on('click', function () {
        confirmar();
    });

    $('#btnCancelar').on('click', function () {
        $('#popupVersion').bPopup().close();
    });

});

function consultar() {
    limpiarMensaje('mensaje');
    if ($('#txtAnio').val() != "" && $('#cbSemana').val() != "") {

        $.ajax({
            type: 'POST',
            url: controlador + 'lista',
            data: {
                anio: $('#txtAnio').val(),
                semana: $('#cbSemana').val()
            },
            success: function (evt) {
                $('#listado').html(evt);
                $('#tabla').dataTable({
                    "iDisplayLength": 25
                });
            },
            error: function () {
                mostrarMensaje('mensaje', 'error', 'Se ha producido un error.');
            }
        });
    }
    else {
        limpiarTabla();
        if ($('#txtAnio').val() == "" && $('#cbSemana').val() != "")
            mostrarMensaje('mensaje', 'alert', 'Seleccione año.');
        if ($('#txtAnio').val() != "" && $('#cbSemana').val() == "")
            mostrarMensaje('mensaje', 'alert', 'Seleccione semana.');
        if ($('#txtAnio').val() == "" && $('#cbSemana').val() == "")
            mostrarMensaje('mensaje', 'alert', 'Seleccione año y semana.');
    }
}

function limpiarTabla() {
    $('#listado').html("");
}

function cargarSemanas(anio) {
    $.ajax({
        type: 'POST',
        url: controlador + 'ObtenerSemanas',
        data: {
            anio: anio
        },
        dataType: 'json',
        global: false,
        success: function (result) {
            if (result != -1) {
                $('#cbSemana').get(0).options.length = 0;
                $('#cbSemana').get(0).options[0] = new Option("--SELECCIONE--", "");
                $.each(result, function (i, item) {
                    $('#cbSemana').get(0).options[$('#cbSemana').get(0).options.length] = new Option(item.NombreSemana, item.NroSemana);
                });
            }
            else {
                mostrarMensaje('mensaje', 'error', 'Se ha producido un error.');
            }
        },
        error: function () {
            mostrarMensaje('mensaje', 'error', 'Se ha producido un error.');
        }
    });
}

function nuevaVersion() {
    limpiarMensaje('mensaje');
    if ($('#txtAnio').val() != "" && $('#cbSemana').val() != "") {
        $('#popupVersion').bPopup({});
    }
    else {
        limpiarTabla();
        if ($('#txtAnio').val() == "" && $('#cbSemana').val() != "")
            mostrarMensaje('mensaje', 'alert', 'Seleccione año.');
        if ($('#txtAnio').val() != "" && $('#cbSemana').val() == "")
            mostrarMensaje('mensaje', 'alert', 'Seleccione semana.');
        if ($('#txtAnio').val() == "" && $('#cbSemana').val() == "")
            mostrarMensaje('mensaje', 'alert', 'Seleccione año y semana.');
    }
}

function confirmar() {
    $.ajax({
        type: 'POST',
        url: controlador + 'generarversion',
        data: {
            anio: $('#txtAnio').val(),
            semana: $('#cbSemana').val()
        },
        dataType: 'json',
        success: function (result) {
            if (result != -1) {
                consultar();
                $('#popupVersion').bPopup().close();
            }
            else {
                mostrarMensaje('mensaje', 'error', 'Se ha producido un error.');
            }
        },
        error: function () {
            mostrarMensaje('mensaje', 'error', 'Se ha producido un error.');
        }
    })
}

function descargar(id) {
    location.href = controlador + 'DescargarArchivo?id=' + id;
}

function visualizar(id) {
    $("#vistaprevia").hide();

    $.ajax({
        type: 'POST',
        url: controlador + 'VisualiarArchivo',
        data: {
            id: id
        },
        success: function (model) {
            if (model.Resultado != "") {
                var rutaCompleta = window.location.href;
                var rutaInicial = rutaCompleta.split("interconexiones"); //Obtener url de intranet
                var urlPrincipal = rutaInicial[0];
                var urlFrame = urlPrincipal + model.Resultado;
                console.log(urlFrame);
                var urlFinal = "https://view.officeapps.live.com/op/embed.aspx?src=" + urlFrame;

                $('#idPopupVistaPrevia').bPopup({
                    easing: 'easeOutBack',
                    speed: 450,
                    transition: 'slideDown',
                    modalClose: false
                });

                $("#vistaprevia").show();
                $('#vistaprevia').html('');
                $('#vistaprevia').attr("src", urlFinal);

            } else {
                alert("la vista previa solo permite archivos word, excel y pdf.");
            }
        },
        error: function (err) {
            document.getElementById('filelist').innerHTML = `<div class="action-alert">Ha ocurrido un error.</div>`;
        }
    });

    return true;
}

function mostrarMensaje(id, tipo, mensaje) {
    $('#' + id).removeClass();
    $('#' + id).addClass('action-' + tipo);
    $('#' + id).html(mensaje);
}

function limpiarMensaje(id) {
    $('#' + id).removeClass();
    $('#' + id).html('');
}