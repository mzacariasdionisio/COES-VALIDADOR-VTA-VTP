var controlador = siteRoot + "intervenciones/configuracion/";

$(function () {

    consultar();

    $('#btnGrabar').on('click', function () {
        grabar();
    });

    $('#btnExportar').on('click', function () {
        exportar();
    });

    $('#btnConsultar').on('click', function () {
        consultar();
    });

});

function consultar() {
    $.ajax({
        type: 'POST',
        url: controlador + 'listanotificacion',
        data: {
            empresa: $('#cbEmpresa').val(),
            estado: $('#cbEstado').val()
        },
        success: function (evt) {
            $('#listado').html(evt);          
        },
        error: function () {
            mostrarMensaje('mensaje', 'error', 'Ha ocurrido un error.');
        }
    });
}

function grabar() {
    var array = [];
    $('#tablaNotificacion tbody tr').each(function () {
        var item = [];
        item.push($(this).find("#hfEmpresa").val());
        item.push($(this).find("#hfEmpresaNombre").val());
        item.push($(this).find("#hfUsuario").val());
        $(this).find('input:checkbox').each(function () {
            item.push(($(this).is(":checked") == true) ? 1 : 0);
        });
        item.push($(this).find("#hfCodigo").val());
        array.push(item);        
    });

    console.log(array);

    var validacion = validar(array);

    if (validacion == "") {
        $.ajax({
            type: 'POST',
            url: controlador + 'GrabarNotificacion',
            contentType: 'application/json',
            data: JSON.stringify({
                data: array
            }),
            dataType: 'json',
            success: function (result) {
                if (result == 1) {
                    mostrarMensaje('mensaje', 'exito', 'La configuración de notificación de mensajes se realizó correctamente.');
                    consultar();
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
    else {
        mostrarMensaje('mensaje', 'alert', validacion);
    }
}

function validar(data) {
        
    var arregloEmpresa = [];
    for (var k = 0; k < data.length; k++) {
        var itemEmpresa = { id: data[k][0], empresa: data[k][1] };
        var flagExiste = false;
        for (var t = 0; t < arregloEmpresa.length; t++) {
            if (itemEmpresa.id == arregloEmpresa[t].id) {
                flagExiste = true;
            }
        }
        if (!flagExiste) arregloEmpresa.push(itemEmpresa);
    }

    console.log(arregloEmpresa);

    var html = "<ul>";
    var flag = true;
    for (var i = 0; i < arregloEmpresa.length; i++) {
        var flagDiario = true;
        var flagSemanal = true;
        var flagMensual = true;
        var flagAnual = true;
        var flagEjecutado = true;
        for (var k = 0; k < data.length; k++) {
            if (arregloEmpresa[i].id == data[k][0]) {
                if (data[k][3] == 1) flagEjecutado = false;
                if (data[k][4] == 1) flagDiario = false;
                if (data[k][5] == 1) flagSemanal = false;
                if (data[k][6] == 1) flagMensual = false;
                if (data[k][7] == 1) flagAnual = false;                
            }
        }
        if (flagEjecutado) html = html + "<li> Para la empresa " + arregloEmpresa[i].empresa + " y el tipo de programación EJECUTADO debe seleccionar la casilla de al menos un Usuario. </li>";
        if (flagDiario) html = html + "<li> Para la empresa " + arregloEmpresa[i].empresa + " y el tipo de programación DIARIO debe seleccionar la casilla de al menos un Usuario. </li>";
        if (flagSemanal) html = html + "<li> Para la empresa " + arregloEmpresa[i].empresa + " y el tipo de programación SEMANAL debe seleccionar la casilla de al menos un Usuario. </li>";
        if (flagMensual) html = html + "<li> Para la empresa " + arregloEmpresa[i].empresa + " y el tipo de programación MENSUAL debe seleccionar la casilla de al menos un Usuario. </li>";
        if (flagAnual) html = html + "<li> Para la empresa " + arregloEmpresa[i].empresa + " y el tipo de programación ANUAL debe seleccionar la casilla de al menos un Usuario. </li>";
        

        if (flagDiario || flagSemanal || flagMensual || flagAnual || flagEjecutado) flag = false;
    }

    html = html + "</ul>";

    if (flag) html = "";

    return html;
}

function exportar() {
    $.ajax({
        type: 'POST',
        url: controlador + "ExportarNotificacion",
        data: {
            empresa: $('#cbEmpresa').val(),
            estado: $('#cbEstado').val()
        },
        dataType: 'json',
        cache: false,
        success: function (resultado) {
            if (resultado == 1) {
                location.href = controlador + "DescargarNotificacion";
            }
            else {
                mostrarMensaje('mensaje', 'error', 'Ha ocurrido un error.');
            }
        },
        error: function () {
            mostrarMensaje('mensaje', 'error', 'Ha ocurrido un error.');
        }
    });
}

function verHistorico(idEmpresa, idUsuario, empresa, usuario) {
    $.ajax({
        type: 'POST',
        url: controlador + 'historiconotificacion',
        data: {
            idEmpresa: idEmpresa,
            empresa: empresa,
            idUsuario: idUsuario,
            usuario: usuario
        },
        success: function (evt) {
            $('#contenidoHistorico').html(evt);
            setTimeout(function () {
                $('#popupHistorico').bPopup({
                    autoClose: false,
                    modalClose: false,
                    escClose: false,
                    follow: [false, false]
                });
            }, 50);
        },
        error: function () {
            mostrarMensaje('mensaje', 'error', 'Ha ocurrido un error.');
        }
    });
}

function mostrarMensaje(id, tipo, mensaje) {
    $('#' + id).removeClass();
    $('#' + id).addClass('action-' + tipo);
    $('#' + id).html(mensaje);
}
