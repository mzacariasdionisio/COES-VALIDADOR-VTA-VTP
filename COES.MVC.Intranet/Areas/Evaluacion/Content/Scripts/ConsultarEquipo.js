//Declarando las variables 
let controlador = siteRoot + 'Transversal/';
let hot;
let hotOptions;
let evtHot;

$(function () {

    consultar();

    $("#btnRegresar").click(function () {
        retornar();
    });

});

function buscarEquipo() {
    consultar();
}

let consultar = function () {
    const codigoId = $("#hCodigoId").val();
    setTimeout(function () {
        $.ajax({
            type: 'POST',
            url: controlador + 'ListaConsultarEquipo?codigoId=' + codigoId,
            success: function (evt) {
                $('#lista').html(evt);
                $('#tablaListado').dataTable({
                    "iDisplayLength": 25,
                    language: {
                        info: 'Mostrando página _PAGE_ de _PAGES_',
                        infoEmpty: 'No hay registros disponibles',
                        infoFiltered: '(filtrado de _MAX_ registros totales)',
                        lengthMenu: 'Mostrar _MENU_ registros por página',
                        zeroRecords: 'No se encontró nada'
                    }
                });

                $('.dataTables_filter input').attr('maxLength', 50);
            },
            error: function () {
                mostrarMensaje('mensaje', 'error', 'Ha ocurrido un error.');
            }
        });
    }, 100);
};

function retornar() {
    let ret = siteRoot + $("#hParthReturn").val();

    window.location.href = ret;
};
let mostrarError = function () {
    $('#mensaje').removeClass("action-exito");
    $('#mensaje').addClass("action-error");
    $('#mensaje').text("Ha ocurrido un error");
    $('#mensaje').css("display", "block");
};
