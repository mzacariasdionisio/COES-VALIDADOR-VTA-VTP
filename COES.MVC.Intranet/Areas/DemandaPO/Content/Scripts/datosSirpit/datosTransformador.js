var dt;

$(document).ready(function () {
    $('#selTransformador').multipleSelect({
        single: true,
        filter: true,
        placeholder: 'Seleccione un Transformador...',
    });

    $('#selTransformador').on('change', function () {
        let tranformador = $('#selTransformador').val();
        listarTransformadores(tranformador);
    });

    listarTransformadores(0);
});

function listarTransformadores(id) {
    $.ajax({
        type: 'POST',
        url: controller + "ListarTransformadores",
        contentType: 'application/json; charset=utf-8',
        data: JSON.stringify({
            transformador: id
        }),
        datatype: 'json',
        traditional: true,
        success: function (result) {
            dt = $('#dt').DataTable({
                data: result.ListaTransformadores,
                columns: [
                    { title: 'Codigo Transformador', data: 'Dpotnfcodi', visible: false},
                    { title: 'Codigo Transformador', data: 'Dpotnfcodiexcel' },
                    { title: 'Subestacion Nombre', data: 'Dposubnombre' },
                    { title: 'Empresa Nombre', data: 'Emprnomb' },
                ],
                searching: false,
                bLengthChange: false,
                bSort: false,
                destroy: true,
                info: false,
            });
        },
        error: function () {
            mostrarMensaje('mensaje', 'error', "Lo sentimos, ha ocurrido un error inesperado");
        }
    });
}