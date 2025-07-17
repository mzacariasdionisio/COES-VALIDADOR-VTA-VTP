var dt;

$(document).ready(function () {
    $('#selSubestacion').multipleSelect({
        single: true,
        filter: true,
        placeholder: 'Seleccione una Subestación...',
    });

    $('#selSubestacion').on('change', function () {
        let subestacion = $('#selSubestacion').val();
        listarSubestaciones(subestacion);
    });

    listarSubestaciones(0);
});

function listarSubestaciones(id) {
    $.ajax({
        type: 'POST',
        url: controller + "ListarSubestaciones",
        contentType: 'application/json; charset=utf-8',
        data: JSON.stringify({
            subestacion: id
        }),
        datatype: 'json',
        traditional: true,
        success: function (result) {
            dt = $('#dt').DataTable({
                data: result.ListaSubestaciones,
                columns: [
                    { title: 'Codigo Subestacion', data: 'Dposubcodi', visible: false },
                    { title: 'Codigo Subestacion', data: 'Dposubcodiexcel' },
                    { title: 'Subestacion Nombre', data: 'Dposubnombre' },
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