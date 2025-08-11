var controlador = siteRoot + 'ValidadorVTPEntrada/';
var uploader;

$(function () {

    $('#tab-container').easytabs({
        animate: false
    });   

    $('#cbPeriodo').on('change', function () {
        consultar(1);
        cargarVersiones();
    });

    $('#cbVersion').on('change', function () {
        consultar(1);  
    });  

    consultar(1);
});

function mostrarMensaje(id, tipo, mensaje) {
    $("#" + id).css("display", "block");
    $('#' + id).removeClass();
    $('#' + id).addClass('action-' + tipo);
    $('#' + id).html(mensaje);

}

function limpiarBarraMensaje(id) {
    $('#' + id).css("display", "none");
    $('#' + id).removeClass();
    $('#' + id).addClass('action-message');
    $('#' + id).html('');
}

function cargarVersiones() {

    let periodo = $("#cbPeriodo").val();

    $.ajax({
        type: 'POST',
        url: controlador + 'ObtenerVersiones',
        data: {
            periodo: periodo
        },
        dataType: 'json',
        global: false,
        success: function (result) {
            if (result.StrMensajeError != -1) {
                $('#cbVersion').get(0).options.length = 0;
                //$('#cbPeriodo').get(0).options[0] = new Option("--SELECCIONE--", "");
                $.each(result.ListVersiones, function (i, item) {
                    $('#cbVersion').get(0).options[$('#cbVersion').get(0).options.length] = new Option(item.RecPotNombre, item.RecPotNombre);
                });
                //$('#tab-container').hide();
            }
            else {
                mostrarMensaje('mensaje', 'error', 'Se ha producido un error.');
            }
        },
        error: function () {
            mostrarMensaje('mensaje', 'error', 'Se ha producido un error.');
        }
    });
}

 function consultar (inicializar) {

    let periodo = $("#cbPeriodo").val();
    let version = $("#cbVersion").val();

     if (periodo == '') {
         periodo = 0;
     }

     if (version == '') {
         version = 0;
     }

    setTimeout(function () {
        $.ajax({
            type: 'GET',
            url: controlador + 'CargarReporteConsolidadoHtml',
            data: {
                periodo: periodo,
                version: version,
                inicializar: inicializar
            },
            success: function (evt) {
                $('#tab-container').show();
                $('#detalleBarrasBrg').html(evt.VistaBarrasBrg);
                $('#detalleBarrasNoBrg').html(evt.VistaBarrasNoBrg);
                $('#detalleBarrasSinAnalizar').html(evt.VistaBarrasSinAnalizar);
                $('#detalleDiferenciaPotencias').html(evt.VistaBarrasDiferencia);

                $('#tablaListadoBarras').dataTable({
                    "iDisplayLength": 25,
                    columnDefs: [
                        
                    ],
                    language: {
                        info: 'Mostrando página _PAGE_ de _PAGES_',
                        infoEmpty: 'No hay registros disponibles',
                        infoFiltered: '(filtrado de _MAX_ registros totales)',
                        lengthMenu: 'Mostrar _MENU_ registros por página',
                        zeroRecords: 'No se encontró nada'
                    },
                    order: [[4, 'asc']]
                });

                $('#tablaListadoBarrasNoBrg').dataTable({
                    "iDisplayLength": 25,
                    columnDefs: [

                    ],
                    language: {
                        info: 'Mostrando página _PAGE_ de _PAGES_',
                        infoEmpty: 'No hay registros disponibles',
                        infoFiltered: '(filtrado de _MAX_ registros totales)',
                        lengthMenu: 'Mostrar _MENU_ registros por página',
                        zeroRecords: 'No se encontró nada'
                    },
                    order: [[4, 'asc']]
                });

                $('#tablaListadoBarrasSinAnalizar').dataTable({
                    "iDisplayLength": 25,
                    columnDefs: [

                    ],
                    language: {
                        info: 'Mostrando página _PAGE_ de _PAGES_',
                        infoEmpty: 'No hay registros disponibles',
                        infoFiltered: '(filtrado de _MAX_ registros totales)',
                        lengthMenu: 'Mostrar _MENU_ registros por página',
                        zeroRecords: 'No se encontró nada'
                    },
                    order: [[4, 'asc']]
                });

                $('#tablaListadoBarrasDiferencia').dataTable({
                    "iDisplayLength": 25,
                    columnDefs: [

                    ],
                    language: {
                        info: 'Mostrando página _PAGE_ de _PAGES_',
                        infoEmpty: 'No hay registros disponibles',
                        infoFiltered: '(filtrado de _MAX_ registros totales)',
                        lengthMenu: 'Mostrar _MENU_ registros por página',
                        zeroRecords: 'No se encontró nada'
                    },
                    order: [[4, 'asc']]
                });

                $('.dataTables_filter input').attr('maxLength', 50);
            },
            error: function () {
                alert('Hubo un error');
            }
        });
    }, 100);
}



