var controlador = siteRoot + 'ValidadorVTPVTEA/';
var uploader;

$(function () {

    $('#tab-container').easytabs({
        animate: false
    });   

    $('#cbPeriodo').on('change', function () {
        consultar(1);
        cargarVersiones();
    });

    $('#cbVersionVTP').on('change', function () {
        consultar(1);   
    });  

    $('#cbVersionVTEA').on('change', function () {
        consultar(1);
    });  

    $('#btnProcesar').on('click', function () {
        consultar(0);
    });

    consultar(1);
});

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
                $('#cbVersionVTP').get(0).options.length = 0;               
                $.each(result.ListVersiones, function (i, item) {
                    $('#cbVersionVTP').get(0).options[$('#cbVersionVTP').get(0).options.length] = new Option(item.RecPotNombre, item.RecPotNombre);
                });                
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
     let versionVTEA = $("#cbVersionVTEA").val();
        

    setTimeout(function () {
        $.ajax({
            type: 'GET',
            url: controlador + 'CargarReporteConsolidadoHtml',
            data: {
                periodo: periodo,
                version: version,
                versionVTEA: versionVTEA,
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
                    order: [[1, 'asc']]
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
                    order: [[1, 'asc']]
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
                    order: [[1, 'asc']]
                });
              

                $('.dataTables_filter input').attr('maxLength', 50);
            },
            error: function () {
                alert('Hubo un error');
            }
        });
    }, 100);
}



