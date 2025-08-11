var controlador = siteRoot + 'ValidadorVTPVTEA/';
var uploader;

$(function () {

    $('#tab-container').easytabs({
        animate: false
    });   

    $('#cbPeriodo').on('change', function () {
        //$('#detalleBarras').html("");
        //$('#detalleBarras1').html("");
        //$('#tab-container').hide();
    });

    $('#cbVersionVTP').on('change', function () {
        //$('#detalleBarras').html("");
        //$('#detalleBarras1').html("");
        //$('#tab-container').hide();        
    });  

    consultar(1);
});

function cargarPeriodos(anio) {
    $.ajax({
        type: 'POST',
        url: controlador + 'ObtenerPeriodos',
        data: {
            anio: anio
        },
        dataType: 'json',
        global: false,
        success: function (result) {
            if (result != -1) {
                $('#cbPeriodo').get(0).options.length = 0;
                $('#cbPeriodo').get(0).options[0] = new Option("--SELECCIONE--", "");
                $.each(result, function (i, item) {
                    $('#cbPeriodo').get(0).options[$('#cbPeriodo').get(0).options.length] = new Option(item.Repernombre, item.Repercodi);
                });
                $('#tab-container').hide();
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
    let version = $("#cbVersionVTP").val();

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
                $('#detalleComparacionVTEA').html(evt.VistaComparacionVTEA);
                $('#detalleComparacionVTP').html(evt.VistaComparacionVTP);
                $('#detalleComparacionDiferencias').html(evt.VistaComparacionDiferencia);               

                $('#tablaListadoComparacionVTEA').dataTable({
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

                $('#tablaListadoComparacionVTP').dataTable({
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

                $('#tablaListadoComparacionDiferencias').dataTable({
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



