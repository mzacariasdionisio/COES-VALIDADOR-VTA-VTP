var controlador = siteRoot + 'ValidadorVTPSalida/';
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

    $('#btnProcesar').on('click', function () {
        consultar(0);
    });

    $('#btnDescargaValorizacion').on('click', function () {
        descargarReporte('Valorizacion');
    });

    $('#btnDescargaCompensacion').on('click', function () {
        descargarReporte('Compensacion');
    });
      

    consultar(1);
});

function mostrarMensaje(id, tipo, mensaje) {
    $("#" + id).css("display", "block");
    $('#' + id).removeClass();
    $('#' + id).addClass('action-' + tipo);
    $('#' + id).html(mensaje);

}

function limpiarMensaje(id) {
    $('#' + id).css("display", "none");
    $('#' + id).removeClass();
    $('#' + id).addClass('action-message');
    $('#' + id).html('');
}


function cargarVersiones() {

    let periodo = $("#cbPeriodo").val();

    limpiarMensaje('mensaje');

    $.ajax({
        type: 'POST',
        url: controlador + 'ObtenerVersiones',
        data: {
            periodo: periodo
        },
        dataType: 'json',
        global: false,
        success: function (result) {

            $('#cbVersion').get(0).options.length = 0;         

            if (result.StrMensajeError == '') {
                     
                $.each(result.VersionesVtp.Versiones, function (i, item) {
                    $('#cbVersion').get(0).options[$('#cbVersion').get(0).options.length] = new Option(item.RecPotNombre, item.RecPotNombre);
                });                
            }
            else {
                alert(result.StrMensajeError);
            }
        },
        error: function () {
            alert("Ha ocurrido un error interno no previsto en el sistema. Por favor comunique al Administrador del sistema.");
        }
    });
}

 function consultar (inicializar) {

     let periodo = $("#cbPeriodo").val();
     let version = $("#cbVersion").val();     

     if (inicializar == 0) {
         if (periodo == '' || periodo == null) {
             mostrarMensaje('mensaje', 'error', 'El dato “Mes de valorización” está vacío. No es posible procesar la evaluación.');
             return;
         }

         if (version == '' || version == null) {
             mostrarMensaje('mensaje', 'error', 'El dato “Versión” está vacío. No es posible procesar la evaluación.');
             return;
         }
     }

     limpiarMensaje('mensaje');

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
                $('#detalleValorizacion').html(evt.VistaValorizacion);
                $('#detalleCompensacion').html(evt.VistaCompensacion);

                $('#tablaListadoValorizacion').dataTable({
                    "iDisplayLength": 20,
                    "lengthMenu": [[20, 50, 100], [20, 50, 100]],
                    "pagingType": "full_numbers",
                    columnDefs: [

                    ],
                    language: {
                        info: 'Mostrando página _PAGE_ de _PAGES_',
                        infoEmpty: 'No hay registros disponibles',
                        infoFiltered: '(filtrado de _MAX_ registros totales)',
                        lengthMenu: 'Mostrar _MENU_ registros por página',
                        zeroRecords: 'No se encontró nada',
                        "paginate": {
                            "first": '<<',
                            "last": '>>',
                            "next": '>',
                            "previous": '<'
                        }
                    },
                    order: [[0, 'asc']]
                });

                $('#tablaListadoCompensacion').dataTable({
                    "iDisplayLength": 20,
                    "lengthMenu": [[20, 50, 100], [20, 50, 100]],
                    "pagingType": "full_numbers",
                    columnDefs: [

                    ],
                    language: {
                        info: 'Mostrando página _PAGE_ de _PAGES_',
                        infoEmpty: 'No hay registros disponibles',
                        infoFiltered: '(filtrado de _MAX_ registros totales)',
                        lengthMenu: 'Mostrar _MENU_ registros por página',
                        zeroRecords: 'No se encontró nada',
                        "paginate": {
                            "first": '<<',
                            "last": '>>',
                            "next": '>',
                            "previous": '<'
                        }
                    },
                    order: [[0, 'asc']]
                });

                $('#mensajeProcesar').html(evt.StrMensaje);

                $('.dataTables_filter input').attr('maxLength', 50);

                if (evt.StrMensajeError != '') {                    
               
                    alert(evt.StrMensajeError);
                }
              
            },
            error: function () {
                alert('Ha ocurrido un error interno no previsto en el sistema. Por favor comunique al Administrador del sistema.');
            }
        });
    }, 100);
}

function descargarReporte(seccion) {
    let periodo = $("#cbPeriodo").val();
    let version = $("#cbVersion").val();

    var mensajeError = '';
    switch (seccion) {
       
        case 'Valorizacion': {
            let tablaListadoValorizacion = $("#tablaListadoValorizacion").DataTable();

            if (tablaListadoValorizacion.data().length == 0) {
                mensajeError = 'La grilla está vacía, no es posible descargar.';
            }
            break;
        }
        case 'Compensacion': {
            let tablaListadoCompensacion = $("#tablaListadoCompensacion").DataTable();

            if (tablaListadoCompensacion.data().length == 0) {
                mensajeError = 'La grilla está vacía, no es posible descargar.';
            }
            break;
        }
    }

    if (mensajeError != '') {
        mostrarMensaje('mensaje', 'error', mensajeError);
        return;
    }


    $.ajax({
        type: 'POST',
        url: controlador + 'GenerarReporteSeccion',
        data: {
            periodo: periodo,
            version: version,
            seccion: seccion
        },
        dataType: 'json',
        success: function (result) {
            if (result != "-1") {
                window.location.href = controlador + 'DescargarArchivo?file=' + result;

            }
            else {
                alert('Ha ocurrido un error interno no previsto en el sistema. Por favor comunique al Administrador del sistema.');
            }
        },
        error: function (err) {
            alert('Ha ocurrido un error interno no previsto en el sistema. Por favor comunique al Administrador del sistema.');
        }
    });
}



