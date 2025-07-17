$(document).ready(function () {
    $('#filtroFecha').Zebra_DatePicker();

    $('#filtroTipo').multipleSelect({
        filter: true,
        single: true,
    });

    $('#btnImportar').on('click', function () {
        const date = $('#filtroFecha').val();
        if (!date) {
            SetMessage('#message',
                'Debe seleccionar una fecha para la importación',
                'warning', true);
            return false;
        }
        const dir = $('#txtDireccion').val();
        if (!dir) {
            SetMessage('#message',
                'Debe ingresar una dirección valida',
                'warning', true);
            return false;
        }
        importarArchivos(date, dir);
    });

    function importarArchivos(date, dir) {
        $.ajax({
            type: 'POST',
            url: controller + 'ImportarArchivos',
            contentType: 'application/json; charset=utf-8',
            data: JSON.stringify({
                fechaImportacion: date,
                direccion: dir
            }),
            datatype: 'json',
            traditional: true,
            success: function (result) {
                SetMessage('#message', result.msg, result.type);
                console.log(result, '_result');
            },
            error: function () {
                alert('Ha ocurrido un error...');
            }
        });
    }
});