var controlador = siteRoot + 'admin/empresa/';
var ancho = 900;

$(function () {
    ancho = $('#mainLayout').width() - 50;

    $('#btnIrAlListado').click(function () {
        document.location.href = controlador + 'index';
    });

    $('#txtFecha').Zebra_DatePicker({
        onSelect: function () {
            buscarElementos();
        }
    });

    $('#empresa').multipleSelect({
        width: '350px',
        filter: true,
        single: true,
        onClose: function () {
            buscarElementos();
        }
    });

    $('#btnConsultar').on('click', function () {
        buscarElementos();
    });

    $("#empresa").multipleSelect("setSelects", [0]);

    buscarElementos();
});

///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
/// Historico
///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
function buscarElementos() {
    $("#listado").html('');

    pintarBusqueda();
};

function pintarBusqueda() {
    if (getEmpresa() > 0) {

        $.ajax({
            type: 'POST',
            url: controlador + "ListaEstadoHistoricoEmpresa",
            data: {
                strfecha: $("#txtFecha").val(),
                idEmpresa: getEmpresa()
            },
            success: function (evt) {
                $('#listado').css("width", ancho + "px");
                $('#listado').html(evt);

                $('#listadoEmpresa').dataTable({
                    "sPaginationType": "full_numbers",
                    "sDom": 'ft',
                    "ordering": false,
                    "iDisplayLength": 50
                });
            },
            error: function (err) {
                alert('Ha ocurrido un error');
            }
        });
    }
};

function getEmpresa() {
    return parseInt($('#empresa').val()) || 0;
}