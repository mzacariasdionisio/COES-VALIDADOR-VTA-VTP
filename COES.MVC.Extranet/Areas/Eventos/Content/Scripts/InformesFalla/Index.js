var controlador = siteRoot + 'eventos/InformesFalla/';

$(function () {
    $('#dtInicio').Zebra_DatePicker({
    });

    $('#dtFin').Zebra_DatePicker({
    });

    $("#btnConsultar").click(function () {
        Consultar();
    });

    ObtenerListado();
});

function ObtenerListado() {
    var dtInicio = $("#dtInicio").val();
    var dtFin = $("#dtFin").val();
    var miDataM = {
        DI: dtInicio,
        DF: dtFin
    };

    $("#listado").html('');

    $.ajax({
        type: 'POST',
        url: controlador + 'Listado',
        contentType: "application/json; charset=utf-8",
        data: JSON.stringify(miDataM),
        success: function (eData) {
            $('#listado').css("width", $('.form-main').width() + "px");
            $('#listado').html(eData);
            $('#tabla').dataTable({
                bJQueryUI: true,
                "scrollY": 440,
                "scrollX": false,
                "sDom": 'ft',
                "ordering": true,
                "order": [[0, 'desc']],
                "iDisplayLength": -1                
            });
        },
        error: function (err) {
            alert("Ha ocurrido un error");
        }
    });
}

function Consultar() {
    ObtenerListado();
}

function VerDetalleEvento(Evencodi) {
    abrirPopupDetalleEvento(Evencodi);
}

function abrirPopupDetalleEvento(Evencodi) {
    $.ajax({
        type: 'POST',
        url: controlador + "VerDetalleEvento",
        data: {
            Evencodi: Evencodi
        },
        success: function (evt) {
            $('#contenidoDetalle').html(evt);
            var excep_resultado = $("#hdResultado_ED").val();
            var excep_mensaje = $("#hdMensaje_ED").val();

            if (excep_resultado !== "-1") {
                setTimeout(function () {
                    $('#popupDetalleEvento').bPopup({
                        autoClose: false,
                        easing: 'easeOutBack',
                        speed: 450,
                        transition: 'slideDown'
                    });
                }, 500);
            } else {
                $('#contenidoDetalle').html('');
                alert(excep_mensaje);
            }
        },
        error: function (err) {
            alert("Lo sentimos, ha ocurrido un error inesperado");
        }
    });
}

function VerCargaArchivo(Evencodi, Tip_arch, Plazo, Emprcodi) {
    if (Emprcodi > 0)
        abrirPopupCargaArchivo(Evencodi, Tip_arch, Plazo, Emprcodi);
    else
        mostrarAlerta('El usuario debe tener una empresa asignada.');
}

function abrirPopupCargaArchivo(Evencodi, Tip_arch, Plazo, Emprcodi) {
    if (Tip_arch == 1)
        $("#popupCargaArchivo .popup-title span").text("Carga de IPI - Informe");
    else if (Tip_arch == 2)
        $("#popupCargaArchivo .popup-title span").text("Carga de IF - Informe");

    $.ajax({
        type: 'POST',
        url: controlador + "ListarInformesEventos",
        data: {
            Evencodi: Evencodi,
            Tip_arch: Tip_arch,
            Plazo: Plazo,
            Emprcodi: Emprcodi
        },
        success: function (evt) {
            $('#listaArchivosEventos').html(evt);
            var excep_resultado = $("#hdResultado_ED").val();
            var excep_mensaje = $("#hdMensaje_ED").val();

            if (excep_resultado !== "-1") {
                setTimeout(function () {
                    $('#popupCargaArchivo').bPopup({
                        autoClose: false,
                        easing: 'easeOutBack',
                        speed: 450,
                        transition: 'slideDown'
                    });
                }, 500);
            } else {
                $('#contenidoDetalle').html('');
                alert(excep_mensaje);
            }
        },
        error: function (err) {
            alert("Lo sentimos, ha ocurrido un error inesperado");
        }
    });
}
function cerrarPopUp() {
    $('#popupCargaArchivo').bPopup().close();
}