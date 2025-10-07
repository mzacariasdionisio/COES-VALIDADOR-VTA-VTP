const controlador = siteRoot + 'ValidadorVteaSalida/';


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

    $('#btnDescarga').on('click', function () {
        descargarReporte();
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

    let periodo = $("#cbPeriodo option:selected").text();

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
                $.each(result.VersionesVtea.Versiones, function (i, item) {
                    $('#cbVersion').get(0).options[$('#cbVersion').get(0).options.length] = new Option(item.RecaNombre, item.RecaCodi);
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

     let periodo = $("#cbPeriodo option:selected").text();
     let version = $("#cbVersion option:selected").text();   

     if (inicializar == 0) {

         if ((periodo == '' || periodo == null) && (version == '' || version == null)) {
             mostrarMensaje('mensaje', 'error', 'Los datos “Mes de valorización” y “Versión VTEA” están vacíos. No es posible procesar la evaluación.');
             return;
         }

         if (periodo == '' || periodo == null) {
             mostrarMensaje('mensaje', 'error', 'El dato “Mes de valorización” está vacío. No es posible procesar la evaluación.');
             return;
         }

         if (version == '' || version == null) {
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
                version: version,
                esInicio: inicializar
            },
            success: function (evt) {

                $('#tab-container').show();
                $('#tab-container').easytabs('select', '#RetirosNegativos');
                $('#detalleRetirosNegativos').html(evt.VistaBarrasBrg);
                $('#detalleSinDeclaracion').html(evt.VistaBarrasNoBrg);
                $('#detalleDeclaracionesNuevas').html(evt.VistaBarrasSinAnalizar);
                $('#detalleFinContrato').html(evt.VistaBarrasDiferencia);

                configurarDataTable('#tablaListadoRetirosNegativos');
                configurarDataTable('#tablaListadoSinDeclaracion');
                configurarDataTable('#tablaListadoDeclaracionesNuevas');
                configurarDataTable('#tablaListadoFinContrato');
              

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

function configurarDataTable(selector) {
    $(selector).DataTable({
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
}

function descargarReporte(seccion) {
    let periodo = $("#cbPeriodo option:selected").text();
    let version = $("#cbVersion option:selected").text();   

    let mensajeError = '';
    let tablaListadoRetirosNegativos = $("#tablaListadoRetirosNegativos").DataTable();
    let tablaListadoSinDeclaracion = $("#tablaListadoSinDeclaracion").DataTable();
    let tablaListadoDeclaracionesNuevas = $("#tablaListadoDeclaracionesNuevas").DataTable();
    let tablaListadoFinContrato = $("#tablaListadoFinContrato").DataTable();

    if (tablaListadoRetirosNegativos.data().length == 0 && tablaListadoSinDeclaracion.data().length == 0
        && tablaListadoDeclaracionesNuevas.data().length == 0 && tablaListadoFinContrato.data().length == 0) {
        mensajeError = 'Las grillas están vacías, no es posible descargar.';
    }
    
    if (mensajeError != '') {
        mostrarMensaje('mensaje', 'error', mensajeError);
        return;
    }

    $.ajax({
        type: 'POST',
        url: controlador + 'GenerarReporte',
        data: {
            periodo: periodo,
            version: version
        },
        dataType: 'json',
        success: function (result) {
            if (result != "-1") {
                window.location.href = controlador + 'DescargarArchivo?file=' + result;
               
            }
            else {
                alert("Ha ocurrido un error interno no previsto en el sistema. Por favor comunique al Administrador del sistema.");
            }
        },
        error: function (err) {
            alert("Ha ocurrido un error interno no previsto en el sistema.Por favor comunique al Administrador del sistema.");
        }
    });
}
