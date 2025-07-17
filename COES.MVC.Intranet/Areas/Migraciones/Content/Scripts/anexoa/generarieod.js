var controlador = siteRoot + 'Migraciones/AnexoA/'
$(function () {

    $('#btnGenerar').click(function () {
        saveGenerarIEOD();
    });

    $('#txtFecha').Zebra_DatePicker({
        onSelect: function () {
            cargarGenerarIEOD();
        }
    });

    cargarValoresIniciales();
});

function cargarValoresIniciales() {
    cargarGenerarIEOD();
}

function cargarGenerarIEOD() {
    $.ajax({
        type: 'POST',
        url: controlador + 'CargarGenerarIEOD',
        data: { fecha: $('#txtFecha').val() },
        success: function (aData) {
            $('#listado').html(aData.Resultado);
        },
        error: function () {
            alert("Ha ocurrido un error");
        }
    });
}

function saveGenerarIEOD() {
    if (confirm("Se creara una nueva version con fecha " + $('#txtFecha').val() + ". ¿Desea continuar?")) {
        $.ajax({
            type: 'POST',
            url: controlador + 'SaveGenerarIEOD',
            data: { fecha: $('#txtFecha').val() },
            success: function (aData) {
                if (aData.Total > 0) {
                    alert("Version registrada correctamente..");
                    cargarGenerarIEOD();
                }
            },
            error: function () {
                alert("Ha ocurrido un error");
            }
        });
    }
}