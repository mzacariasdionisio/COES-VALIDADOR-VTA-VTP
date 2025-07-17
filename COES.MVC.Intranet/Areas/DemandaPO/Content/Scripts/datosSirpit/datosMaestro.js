var dt;

$(document).ready(function () {
    $('#txtFechaInicio').Zebra_DatePicker();
    $('#txtFechaFin').Zebra_DatePicker();

    $('#selSubestacion').multipleSelect({
        single: true,
        filter: true,
        placeholder: 'Seleccione una Subestación...',
    });

    $('#selSubestacion').on('change', function () {
        listarData();
    });

    $('#selTransformador').multipleSelect({
        single: true,
        filter: true,
        placeholder: 'Seleccione un Transformador...',
    });

    $('#selTransformador').on('change', function () {
        listarData();
    });

    $('#selBarra').multipleSelect({
        single: true,
        filter: true,
        placeholder: 'Seleccione una Barra...',
    });

    $('#selBarra').on('change', function () {
        listarData();
    });

    $('#btnConsultar').click(function () {
        listarData();
    });

    listarData();
});

function listarData() {
    const fechaInicio = $('#txtFechaInicio').val();
    if (!fechaInicio) {
        alert("Debe ingresar una fecha de inicio valida");
        return false;
    }
    const fechaFin = $('#txtFechaFin').val();
    if (!fechaFin) {
        alert("Debe ingresar una fecha final valida");
        return false;
    }
    let idSubestacion = $('#selSubestacion').val();
    let idTransformador = $('#selTransformador').val();
    let idBarra = $('#selBarra').val();
    if (idSubestacion === null) idSubestacion = 0;
    if (idTransformador === null) idTransformador = 0;
    if (idBarra === null) idBarra = 0;

    $.ajax({
        type: 'POST',
        url: controller + "ListarData",
        contentType: 'application/json; charset=utf-8',
        data: JSON.stringify({
            subestacion: idSubestacion,
            transformador: idTransformador,
            barra: idBarra,
            fechainicio: fechaInicio,
            fechafin: fechaFin
        }),
        datatype: 'json',
        traditional: true,
        success: function (result) {
            dt = $('#dt').DataTable({
                data: result.ListaDatos96,
                columns: [
                    { title: 'Subestación', data: 'Dpodatsubcodi'},
                    { title: 'Transformador', data: 'Dpodattnfcodi' },
                    { title: 'Serie', data: 'Dpodattnfserie' },
                    { title: 'Barra', data: 'Dpodatbarcodi' },
                    { title: 'Tipo', data: 'Dpodattipocodi' },
                    { title: 'Fecha', data: 'sDpodatfecha' },
                    { title: 'Total día', data: 'Meditotal' },
                ],
                pageLength: 100,
                searching: true,
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