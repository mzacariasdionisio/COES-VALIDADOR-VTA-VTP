//Declarando las variables 
let controlador = siteRoot + 'Linea/';
let hot;
let hotOptions;
let evtHot;

$(function () {

    $("#popUpIncluir").addClass("general-popup");

    $("#btnIncluir").click(function () {
        incluir(0);
    });

    $("#btnCerrar").click(function () {
        regresar();
    });

    $("#btnConsultar").click(function () {
        consultar();
    });

    inputSoloNumeros('codigoId');

    $('#idUbicacion').multipleSelect({
        width: '100%',
        filter: true,
        single: true
    });

    $('#idTitular').multipleSelect({
        width: '100%',
        filter: true,
        single: true
    });

    $('#idUbicacion').multipleSelect('setSelects', ['']);
    $('#idTitular').multipleSelect('setSelects', ['']);

    consultar();

});
function regresar2() {
    window.location.href = controlador + "Index2";
};
function regresar() {
    window.location.href = siteRoot + "Evaluacion/Linea/Index";
};

let consultar = function () {

    setTimeout(function () {

        let equiCodi = $("#codigoId").val();
        let codigo = $("#codigo").val();
        let ubicacion = $("#idUbicacion").val();
        let emprCodigo = $("#idTitular").val();
        let equiEstado = $("#estado").val();


        $.ajax({
            type: 'POST',
            url: controlador + 'BuscarLineaLista?equiCodi=' + equiCodi + "&codigo=" + codigo + "&ubicacion=" + ubicacion + "&emprCodigo=" + emprCodigo + "&equiEstado=" + equiEstado,
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
                    }, order: [[7, 'asc']]
                });

                $('.dataTables_filter input').attr('maxLength', 50);


                $('#tablaListado').on('change', 'input[type="radio"][name="seleccionGrupo"]', function () {

                    let fila = $(this).closest('tr');
                    
                    $("#IdEquipo").val(fila.find('td:eq(1)').text());
                });
            },
            error: function () {
                mostrarMensaje('mensaje', 'error', 'Ha ocurrido un error.');
            }
        });
    }, 100);
};

function incluir(e) {

    const idEquipo = $("#IdEquipo").val();
    if (idEquipo == '') {
        alert("Seleccione una línea a incluir")
        return;
    }

    $.ajax({
        type: 'POST',
        url: controlador + "Incluir?id=" + idEquipo,
        success: function (evt) {
            $('#incluir').html(evt);
            setTimeout(function () {
                $('#popUpIncluir').bPopup({
                    easing: 'easeOutBack',
                    speed: 450,
                    transition: 'slideDown'
                });
            }, 50);
            $("#btnCerrarIncluir").click(function () {
                $("#popUpIncluir").bPopup().close();
            });
            $("#btnSeleccionar").click(function () {
                const id = $("#IdEquipo").val();
                let idProyecto = $("#idProyecto").val();
                if (idProyecto == '') {
                    alert("Seleccione un proyecto")
                    return;
                }
                window.location.href = controlador + "IncluirModificar?idLinea=0&idProyecto=" + idProyecto + "&idEquipo=" + id + "&accion=Incluir";
            });

        },
        error: function () {
            mostrarError();
        }
    });
};

let mostrarError = function () {
    $('#mensaje').removeClass("action-exito");
    $('#mensaje').addClass("action-error");
    $('#mensaje').text("Ha ocurrido un error");
    $('#mensaje').css("display", "block");
};

