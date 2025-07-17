var dt;

$(document).ready(function () {
    $('#selBarra').multipleSelect({
        single: true,
        filter: true,
        placeholder: 'Seleccione una Barra...',
    });

    $('#selBarra').on('change', function () {
        let barra = $('#selBarra').val();
        listarBarras(barra);
    });

    listarBarras(0);
});

function listarBarras(id) {
    $.ajax({
        type: 'POST',
        url: controller + "ListarBarras",
        contentType: 'application/json; charset=utf-8',
        data: JSON.stringify({
            barra: id
        }),
        datatype: 'json',
        traditional: true,
        success: function (result) {
            dt = $('#dt').DataTable({
                data: result.ListaBarras,
                columns: [
                    { title: 'Codigo Barra', data: 'Dpobarcodi', visible: false},
                    { title: 'Codigo Barra', data: 'Dpobarcodiexcel' },
                    { title: 'Barra Nombre', data: 'Dpobarnombre' },
                    { title: 'Barra Tensión', data: 'Dpobartension' },
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