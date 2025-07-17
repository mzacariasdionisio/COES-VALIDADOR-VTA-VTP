var controlador = siteRoot + 'Combustibles/configuracion/';

$(function () {
    $('#btnInicio').click(function () {
        regresarPrincipal();
    });

    $('#btnActualizar').click(function () {
        ejecutarNotificaciones(4);
    });
    $('#btnPR31CulminacionPlazoAgente').click(function () {
        ejecutarNotificaciones(1);
    });
    $('#btnPR31RecordatorioCOES').click(function () {
        ejecutarNotificaciones(2);
    });
    $('#btnPR31RecordatorioAgente').click(function () {
        ejecutarNotificaciones(3);
    });

    ObtenerListaCentralXFenerg();
});
function regresarPrincipal() {
    document.location.href = siteRoot + "Combustibles/Gestion/Index";
}

function verDetalle(id) {
    document.location.href = siteRoot + 'Combustibles/configuracion/DetalleParametro/' + id;
}
function verParametrosGruposMop(idGrupo, idAgrup, fechaConsulta) {
    var url = siteRoot + 'Migraciones/Parametro/' + "Index?grupocodi=" + idGrupo + "&agrupcodi=" + idAgrup + "&fechaConsulta=" + fechaConsulta;
    window.open(url, '_blank').focus();
}

//
function actualizarConfiguracionParametros() {

    $.ajax({
        type: 'POST',
        url: controlador + 'ActualizarCentralXFenerg',
        success: function (evt) {
            if (evt.Resultado != "-1") {
                alert("El proceso se ejecutó correctamente");
                ObtenerListaCentralXFenerg();

            } else {
                alert("Ha ocurrido un error: " + evt.Mensaje);
            }
        },
        error: function (err) {
            alert("Ha ocurrido un error...");
        }
    });
}

function ejecutarNotificaciones(tipo) {

    $.ajax({
        type: 'POST',
        url: controlador + 'EjecutarProcesoAutomatico',
        data: {
            tipo: tipo
        },
        success: function (evt) {
            if (evt.Resultado != "-1") {
                alert("El proceso se ejecutó correctamente");
            } else {
                alert("Ha ocurrido un error: " + evt.Mensaje);
            }
        },
        error: function (err) {
            alert("Ha ocurrido un error...");
        }
    });
}

function ObtenerListaCentralXFenerg() {

    $.ajax({
        type: 'GET',
        url: controlador + 'ObtenerListaCentralXFenerg',
        success: function (evt) {
            if (evt.Resultado != "-1") {

                $("#reporte").html(evt.HtmlListado);

                $('#listado').css("width", $('#mainLayout').width() + "px");

            } else {
                alert("Ha ocurrido un error: " + evt.Mensaje);
            }
        },
        error: function (err) {
            alert("Ha ocurrido un error...");
        }
    });

}