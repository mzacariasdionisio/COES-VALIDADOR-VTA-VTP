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

    $('#btnProcesar').on('click', function () {
        consultar(0);       
    });

    $('#cbEmpresa').on('change', function () {
        
        filtraEmpresa();
    });

    $('#btnAnterior').on('click', function () {
        filtrarEmpresaClick(-1);
    });

    $('#btnSiguiente').on('click', function () {
        filtrarEmpresaClick(1);
    });

    $('#btnDescargaBarras').on('click', function () {
        descargarReporte('Barras');
    });

    $('#btnDescargaBarrasSinAnalizar').on('click', function () {
        descargarReporte('BarrasSinAnalizar');
    });

    $('#btnDescargaDiferenciaPotencia').on('click', function () {
        descargarReporte('BarrasDiferencia');
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

         if ((periodo == '' || periodo == null) && (version == '' || version == null)) {
             mostrarMensaje('mensaje', 'error', 'Los datos “Mes de valorización” y “Versión” están vacíos. No es posible procesar la evaluación.');
             return;
         }

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
                esInicio: inicializar
            },
            success: function (evt) {

                $('#tab-container').show();
                $('#tab-container').easytabs('select', '#barrasBrg');
                $('#detalleBarrasBrg').html(evt.VistaBarrasBrg);
                $('#detalleBarrasNoBrg').html(evt.VistaBarrasNoBrg);
                $('#detalleBarrasSinAnalizar').html(evt.VistaBarrasSinAnalizar);
                $('#detalleDiferenciaPotencias').html(evt.VistaBarrasDiferencia);

                $('#tablaListadoBarras').dataTable({
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

                $('#tablaListadoBarrasNoBrg').dataTable({
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

                $('#tablaListadoBarrasSinAnalizar').dataTable({
                    "iDisplayLength": 5,
                    "lengthMenu": [[5, 20, 50, 100], [5, 20, 50, 100]],
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

                $('#tablaListadoBarrasDiferencia').dataTable({
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

                $('#cbEmpresa').get(0).options.length = 0;

                if (evt.EmpresasBarra.length > 0) {
                    $('#cbEmpresa').get(0).options[0] = new Option("--TODOS--", "");
                    $.each(evt.EmpresasBarra, function (i, item) {
                        $('#cbEmpresa').get(0).options[$('#cbEmpresa').get(0).options.length] = new Option(item, item);
                    });
                }

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

function filtraEmpresa() {
    let empresa = $("#cbEmpresa").val(); 

    let tablaBarra = $("#tablaListadoBarras").DataTable();
    let tablaBarraNoBrg = $("#tablaListadoBarrasNoBrg").DataTable();

    tablaBarra.column(1).search(empresa).draw();
    tablaBarraNoBrg.column(1).search(empresa).draw();
}

function filtrarEmpresaClick(elemento) {
    const totalEmpresas = $('#cbEmpresa').get(0).options.length;

    if (totalEmpresas <= 1) return;

    let indiceActual = $('#cbEmpresa').prop('selectedIndex');    

    let nuevoIndice = (indiceActual + elemento + totalEmpresas) % totalEmpresas;

    $('#cbEmpresa').prop('selectedIndex', nuevoIndice);
    $('#cbEmpresa').trigger('change'); 
}

function descargarReporte(seccion) {
    let periodo = $("#cbPeriodo").val();
    let version = $("#cbVersion").val();
    let empresa = $("#cbEmpresa").val();

    var mensajeError = '';
    switch (seccion) {
        case 'Barras': {
            let tablaBarra = $("#tablaListadoBarras").DataTable();
            let tablaBarraNoBrg = $("#tablaListadoBarrasNoBrg").DataTable();

            if (tablaBarra.data().length == 0 && tablaBarraNoBrg.data().length == 0) {
                mensajeError = 'Ambas grillas de la subsección “1.1 Registros con error en el PPM y Peaje” están vacías, no es posible descargar.';
            }
            break;
        }
        case 'BarrasSinAnalizar': {
            let tablaBarraSinAnalizar = $("#tablaListadoBarrasSinAnalizar").DataTable();

            if (tablaBarraSinAnalizar.data().length == 0) {
                mensajeError = 'La grilla está vacía, no es posible descargar.';
            }
            break;
        }
        case 'BarrasDiferencia': {
            let tablaBarraDiferencia = $("#tablaListadoBarrasDiferencia").DataTable();

            if (tablaBarraDiferencia.data().length == 0) {
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
            empresa: empresa,
            seccion: seccion
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
