var controlador = siteRoot + 'InformeEjecutivoMen/ReporteEjecutivo/';

$(function () {

    $('.pickermes').Zebra_DatePicker({
        format: 'm Y',
        onSelect: function () {
            listaVersionesReporte();
        }
    });

    $('#btnGenerar').click(function () {
        saveGenerarVersion();
    });

    listaVersionesReporte();

});

function saveGenerarVersion() {
    var mes = $('#txtMesAnio').val().replace(" ", "/");
    if (confirm("Se creara version del mes " + mes + ". ¿Desea continuar?")) {
        $.ajax({
            type: 'POST',
            url: controlador + 'SaveGenerarVersion',
            data: { fecha: $('#txtMesAnio').val() },
            success: function (aData) {
                if (aData.Total > 0) {
                    alert("Version registrada correctamente..");
                    listaVersionesReporte();
                }
            },
            error: function (err) {
                alert("Ha ocurrido un error");
            }
        });
    }
}

function listaVersionesReporte() {

    disableLink(document.getElementById("btnExportar"));
    disableLink(document.getElementById("openMenuSiosein"));
    disableLink(document.getElementById("btnNuevaNota"));
    $("#btnNuevaNota").attr("disabled", true);

    fec1 = $("#txtMesAnio").val();
    $.ajax({
        type: 'POST',
        url: controlador + 'ListaVersionesReporte',
        data: { fecha: fec1 },
        success: function (e) {

            $("#cboVersiones option").remove();

            var listSelect = e.ListaSelect;
            $('#cboVersiones').append(new Option("[Seleccionar]", ""));
            $('#cboVersiones option:first').prop('disabled', true);
            $.each(listSelect, function (i, val) {
                $('#cboVersiones').append(new Option(val.text, val.id));
            });

        },
        error: function (err) {
            alert("Ha ocurrido un error");
        }
    });
}

function SelecionarVersion() {
    setearFechas(controlador);

    enableLink(document.getElementById("btnExportar"));
    enableLink(document.getElementById("openMenuSiosein"));
    enableLink(document.getElementById("btnNuevaNota"));
    $("#btnNuevaNota").attr("disabled", false);
}

function exportarExcel() {
    $.ajax({
        type: 'POST',
        url: controlador + 'GenerarReporteEjecutivoMensual',
        dataType: 'json',
        success: function (e) {
            switch (e.Total) {
                case 1: window.location = controlador + "ExportarReporteXls?nameFile=" + e.Resultado; break;// si hay elementos
                case -1: alert(e.Mensaje);
                    alert(e.Resultado2); break;// Error en C#
            }
        },
        error: function () {
            alert("Error en reporte");;
        }
    });
}

function disableLink(link) {
    link.parentElement.classList.add('isDisabled');
    link.setAttribute('aria-disabled', 'true');
}
function enableLink(link) {
    link.parentElement.classList.remove('isDisabled');
    link.removeAttribute('aria-disabled');
}