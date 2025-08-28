const controlador = siteRoot + 'ValidadorVtpvtea/';


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

    $('#btnDescargaComparacionVTEA').on('click', function () {
        descargarReporte('ComparacionVTEA');
    });

    $('#btnDescargaComparacionVTP').on('click', function () {
        descargarReporte('ComparacionVTP');
    });

    $('#btnDescargaDiferencias').on('click', function () {
        descargarReporte('DiferenciaVTPVTEA');
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

            $('#cbVersionVTP').get(0).options.length = 0;   
            $('#cbVersionVTEA').get(0).options.length = 0;

            if (result.StrMensajeError == '') {
                            
                $.each(result.VersionesVtp.Versiones, function (i, item) {
                    $('#cbVersionVTP').get(0).options[$('#cbVersionVTP').get(0).options.length] = new Option(item.RecPotNombre, item.RecPotNombre);
                });                

               
                $.each(result.VersionesVtea.Versiones, function (i, item) {
                    $('#cbVersionVTEA').get(0).options[$('#cbVersionVTEA').get(0).options.length] = new Option(item.RecaNombre, item.RecaNombre);
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
     let version = $("#cbVersionVTP").val();
     let versionVTEA = $("#cbVersionVTEA").val();
        
     if (inicializar == 0) {

         if ((periodo == '' || periodo == null) && (version == '' || version == null) && (versionVTEA == '' || versionVTEA == null)) {
             mostrarMensaje('mensaje', 'error', 'Los datos “Mes de valorización”, “Versión VTP” y “Versión VTEA” están vacíos. No es posible procesar la evaluación.');
             return;
         }

         if (periodo == '' || periodo == null) {
             mostrarMensaje('mensaje', 'error', 'El dato “Mes de valorización” está vacío. No es posible procesar la evaluación.');
             return;
         }

         if ((version == '' || version == null) && (versionVTEA == '' || versionVTEA == null)) {
             mostrarMensaje('mensaje', 'error', 'Los datos “Versión VTP” y “Versión VTEA” están vacíos. No es posible procesar la evaluación.');
             return;
         }

         if (version == '' || version == null) {
             mostrarMensaje('mensaje', 'error', 'El dato “Versión VTP” está vacío. No es posible procesar la evaluación.');
             return;
         }

         if (versionVTEA == '' || versionVTEA == null) {
             mostrarMensaje('mensaje', 'error', 'El dato “Versión VTEA” está vacío. No es posible procesar la evaluación.');
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
                versionVTP: version,
                versionVTEA: versionVTEA,
                inicializar: inicializar
            },
            success: function (evt) {

                $('#tab-container').show();
                $('#tab-container').easytabs('select', '#informacionVTP');

                $('#detalleComparacionVTEA').html(evt.VistaComparacionVTEA);
                $('#detalleComparacionVTP').html(evt.VistaComparacionVTP);
                $('#detalleComparacionDiferencias').html(evt.VistaComparacionDiferencia);

                $('#tablaListadoComparacionVTEA').dataTable({
                    "iDisplayLength": 20,
                    "lengthMenu": [[20, 50, 100], [20, 50, 100]],
                    "pagingType": "full_numbers",
                    columnDefs: [

                    ],
                    language: {
                        info: 'Mostrando página _PAGE_ de _PAGES_',
                        infoEmpty: '',
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
                    order: [[1, 'asc']]
                });

                $('#tablaListadoComparacionVTP').dataTable({
                    "iDisplayLength": 20,
                    "lengthMenu": [[20, 50, 100], [20, 50, 100]],
                    "pagingType": "full_numbers",
                    columnDefs: [

                    ],
                    language: {
                        info: 'Mostrando página _PAGE_ de _PAGES_',
                        infoEmpty: '',
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
                    order: [[1, 'asc']]
                });

                $('#tablaListadoComparacionDiferencias').dataTable({
                    "iDisplayLength": 20,
                    "lengthMenu": [[20, 50, 100], [20, 50, 100]],
                    "pagingType": "full_numbers",
                    columnDefs: [

                    ],
                    language: {
                        info: 'Mostrando página _PAGE_ de _PAGES_',
                        infoEmpty: '',
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
                    order: [[1, 'asc']]
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
    let versionVTP = $("#cbVersionVTP").val();
    let versionVTEA = $("#cbVersionVTEA").val();

    let mensajeError = '';
    switch (seccion) {

        case 'ComparacionVTEA': {
            let tablaComparacionVTEA = $("#tablaListadoComparacionVTEA").DataTable();

            if (tablaComparacionVTEA.data().length == 0) {
                mensajeError = 'La grilla está vacía, no es posible descargar.';
            }
            break;
        }
        case 'ComparacionVTP': {
            let tablaComparacionVTP = $("#tablaListadoComparacionVTP").DataTable();

            if (tablaComparacionVTP.data().length == 0) {
                mensajeError = 'La grilla está vacía, no es posible descargar.';
            }
            break;
        }
        case 'DiferenciaVTPVTEA': {
            let tablaDiferencia = $("#tablaListadoComparacionDiferencias").DataTable();

            if (tablaDiferencia.data().length == 0) {
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
            versionVTP: versionVTP,
            versionVTEA: versionVTEA,
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

