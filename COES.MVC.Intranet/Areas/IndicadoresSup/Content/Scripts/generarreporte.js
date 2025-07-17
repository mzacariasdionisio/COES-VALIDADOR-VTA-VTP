var controlador = siteRoot + 'IndicadoresSup/numeral/';
$(function () {

    $('#txtFecha').Zebra_DatePicker({
        format: 'm Y',
        direction: -1,
        onSelect: function () {
            consultar();
        }
    });

    $('#btnConsultar').on('click', function () {
        consultar();
    });

    $('#btnGenerar').on('click', function () {
        generarReporte();
    });

    $('#btnGenerarDB').on('click', function () {
        var mes = $('#txtFecha').val().replace(" ", "/");
        if (window.confirm("Se generará Datos Base para " + mes + ".\n El proceso puede tardar algunos minutos. ¿Desea continuar?")) {
            generarDB();
        }
    });

    $('#btnGenerarDatosBase').click(function () {
        popUpGenerarDatosBase();
    });

    consultar();
});

function consultar() {
    $("#btnGenerar").show();

    $.ajax({
        type: 'POST',
        url: controlador + 'listagenerarreporte',
        data: { fecha: $('#txtFecha').val() },
        success: function (evt) { $('#listado').html(evt); },
        error: function (err) { alert('Se ha producido un error:' + err.ResponseText); }
    });
}

function generarReporte() {
    $.ajax({
        type: 'POST',
        url: controlador + 'generarreporte',
        data: { fecha: $('#txtFecha').val(), cc: $("#txtNroEstado").val() },
        success: function (evt) {
            if (evt.Resultado != null) {
                if (evt.Resultado === "1") {
                    consultar();

                } else { alert(evt.Resultado); }
            } else { alert("Ha ocurrido un error. " + evt.Mensaje); }
        },
        error: function (err) {
            alert('Se ha producido un error:' + err.ResponseText);
        }
    });
}

function validaVersion(x) {
    $.ajax({
        type: 'POST',
        url: controlador + 'ValidaVersionReporte',
        data: { id: x },
        success: function (evt) {
            if (evt.NroEstadoNum === 1) {
                consultar();
            } else { alert("Ha ocurrido un error. " + evt.Mensaje); }
        },
        error: function (err) {
            alert('Se ha producido un error:' + err.ResponseText);
        }
    });
}

function exportaVersionReporte(verrcodi) {
    $.ajax({
        type: 'POST',
        url: controlador + 'ExportaVersionReporte',
        data: { verrcodi: verrcodi, fecha: $('#txtFecha').val() },
        success: function (evt) {
            switch (evt.NroEstadoNum) {
                case 1: window.location = controlador + "Exportar?fi=" + evt.Resultado; break;// si hay elementos
                case -1: alert("Ha ocurrido un error. " + evt.Mensaje); break;// Error en C#
            }
        },
        error: function (err) { alert('Se ha producido un error:' + err.ResponseText); }
    });
}

function openPopupCrear() {
    setTimeout(function () {
        $('#popupDetalle').bPopup({
            easing: 'easeOutBack',
            speed: 450,
            transition: 'slideDown',
            modalClose: false
        });
    }, 50);
}

function consultarDetalleReporte(verrcodi) {

    $.ajax({
        type: "POST",
        url: controlador + "ConsultarDetalleReporte",
        data: { verrcodi: verrcodi }
    }).done(function (result) {

        if (result.NroEstadoNum === 1) {
            $("#listadoDetalle").html(result.Resultado);
            openPopupCrear();
        } else {
            alert("Ha ocurrido un error. " + result.Mensaje);
        }

    }).fail(function (jqXHR, textStatus, errorThrown) {
        alert("Ha ocurrido un error");
    });
}

function generarDB() {
    $.ajax({
        type: 'POST',
        url: controlador + "GenerarDatosBase",
        data: { fecha: $('#txtFecha').val() },
        success: function (evt) {
            if (evt.Resultado === "1") {
                window.location = controlador + "RareaCarpetaDatosBase?fechita=" + $('#txtFecha').val();
                //$('#popupGenerarDB').bPopup().close();
            } else {
                alert("Ha ocurrido un error al generar Datos Base. " + evt.Mensaje);
            }
        },
        error: function (err) {
            alert('Se ha producido un error:' + err.ResponseText);
        }
    });
}