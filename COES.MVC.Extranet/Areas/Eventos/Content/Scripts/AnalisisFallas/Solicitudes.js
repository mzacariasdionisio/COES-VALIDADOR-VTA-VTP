var controlador = siteRoot + 'eventos/AnalisisFallas/';

$(function () {

    $("#btnConsultar").click(function () {
        Consultar();
    });
    $("#btnExportar").click(function () {
        exportarReporte();
    });

    $("#btnNuevaSolicitud").click(function () {
        redireccionarNuevaSolicitud();
    });

    ObtenerListado();
});

function redireccionarNuevaSolicitud() {
    location.href = controlador + "NuevaSolicitud";
};

function ObtenerListado() {
    var empresa = $("#cbEmpresa").val();

    $.ajax({
        type: 'POST',
        url: controlador + "ListadoSolicitudes",
        data: {
            emprcodi: empresa
        },
        success: function (eData) {
            $('#listado').css("width", $('.form-main').width() + "px");

            $('#listado').html(eData);
            $('#tabla').dataTable({
                "scrollY": 430,
                "scrollX": false,
                "sDom": 't',
                "ordering": false,
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

function VerArchivosAdjuntados(codSoli) {
    abrirPopupArchivos(codSoli);
}

function abrirPopupArchivos(idtSoli) {
    $.ajax({
        type: 'POST',
        url: controlador + "VerArchivosAdjuntados",
        data: {
            codSoli: idtSoli
        },
        success: function (evt) {
            $('#contenidoDetalle').html(evt);
            var excep_resultado = $("#hdResultado_ED").val();
            var excep_mensaje = $("#hdMensaje_ED").val();

            if (excep_resultado !== "-1") {
                setTimeout(function () {
                    $('#popupArchivosAdjuntados').bPopup({
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

////////////////////////////////////////////////////////////////////////////////////////////////////////
/// Exportación
function exportarReporte() {
    var empresa = $("#cbEmpresa").val();

    $.ajax({
        type: 'POST',
        url: controlador + 'ExportarReporteSolicitudes',
        data: {
            emprcodi: empresa
        },
        success: function (result) {
            if (result.Resultado !== "-1") {
                var paramList = [
                    { tipo: 'input', nombre: 'file1', value: result.NombreArchivo },
                ];
                var form = CreateForm(controlador + 'DescargarReporte', paramList);
                document.body.appendChild(form);
                form.submit();
            }
            else {
                alert('Ha ocurrido un error');
            }
        },
        error: function (err) {
            alert('Ha ocurrido un error');
        }
    });
}

function CreateForm(path, params, method = 'post') {
    var form = document.createElement('form');
    form.method = method;
    form.action = path;

    $.each(params, function (index, obj) {
        var hiddenInput = document.createElement(obj.tipo);
        hiddenInput.type = 'hidden';
        hiddenInput.name = obj.nombre;
        hiddenInput.value = obj.value;
        form.appendChild(hiddenInput);
    });
    return form;
}