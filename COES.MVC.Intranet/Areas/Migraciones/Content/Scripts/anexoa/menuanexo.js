var controlador = siteRoot + 'Migraciones/AnexoA/', fec;
$(function () {

    cargarMenuAnexo();
    listaVersionesIEOD();
    $('#txtFecha').Zebra_DatePicker({/*direction: -1*/
        onSelect: function () {
            listaVersionesIEOD();
        }
    });
    $('#btnGenerar').click(function () {
        saveGenerarIEOD();
    });

    setviewpopup();
});

function saveGenerarIEOD() {
    if (confirm("Se creara una nueva version con fecha " + $('#txtFecha').val() + ". ¿Desea continuar?")) {
        $.ajax({
            type: 'POST',
            url: controlador + 'SaveGenerarIEOD',
            data: { fecha: $('#txtFecha').val() },
            success: function (aData) {
                if (aData.Total > 0) {
                    alert("Version registrada correctamente..");
                    listaVersionesIEOD();
                }
            },
            error: function () {
                alert("Ha ocurrido un error");
            }
        });
    }
}

$('#btnVerA').click(function () {
    fec = $("#txtFecha").val();
    $.ajax({
        type: 'POST',
        url: controlador + 'SetearFechaFilterA',
        data: { fec1: fec, fec2: fec, versi: $("#cboVersiones").val() },
        dataType: 'json',
        success: function (e) {
            if (e.Total == 1) {
                $('#winAnexoA').bPopup().close();
                $('#winMenu').show();
            }
        },
        error: function () {
            alert("Ha ocurrido un error");
        }
    });
});

$('#btnExportarA').click(function () {
    fec1 = $("#txtFecha").val();
    $.ajax({
        type: 'POST',
        url: controlador + 'GenerarReporteAnexoAxls',
        data: {
            fecha: fec1,
            url: "/"
        },
        dataType: 'json',
        success: function (result) {
            switch (result.Total) {
                case 1: window.location = controlador + "ExportarReporteXls?nameFile=" + result.Resultado; break;// si hay elementos
                case 2: alert("No existen registros !"); break;// sino hay elementos
                case -1: alert("Error en reporte result"); break;// Error en C#
            }
        },
        error: function () {
            alert("Error en reporte");;
        }
    });
});

function listaVersionesIEOD() {
    fec1 = $("#txtFecha").val();
    $.ajax({
        type: 'POST',
        url: controlador + 'ListaVersionesIEOD',
        data: { fecha: fec1 },
        success: function (e) {
            $('#cbVersion').html(e);
        },
        error: function () {
            alert("Ha ocurrido un error");
        }
    });
}

function cargarMenuAnexo() {
    $.ajax({
        type: 'POST',
        url: controlador + 'CargarMenu',
        data: { id: $("#txtTipreporte").val() },
        dataType: 'json',
        success: function (e) {
            $('#MenuID').html(e.Menu);
            $('#myTable').DataTable({
                "paging": false,
                "lengthChange": false,
                "pagingType": false,
                "ordering": false,
                "info": false
            });
        },
        error: function () {
            alert("Ha ocurrido un error");
        }
    });
}

function setviewpopup() {
    setTimeout(function () {
        $('#winAnexoA').bPopup({
            easing: 'easeOutBack',
            speed: 450,
            transition: 'slideDown',
            modalClose: false
        });
    }, 50);
};