const controlador = siteRoot + 'ValidadorVteaAnalisis/';
let graficoBarrasRol = null;

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

    $('#btnDescargaRolEmpresa').on('click', function () {
        descargarReporte('RolEmpresa');
    });
    $('#btnDescargaEnergia').on('click', function () {
        descargarReporte('Energia');
    });

    //$('#popUpGrafico').bPopup().close();

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
                $('#tab-container').easytabs('select', '#RolEmpresa');
                $('#detalleRolEmpresa').html(evt.VistaBarrasBrg);
                $('#detalleEnergia').html(evt.VistaBarrasNoBrg);               

                $('#tablaListadoRolEmpresa').dataTable({
                    "iDisplayLength": 10,
                    "lengthMenu": [[10, 20, 50, 100], [10, 20, 50, 100]],
                    "pagingType": "full_numbers",
                    columnDefs: [
                        { width: "10%", targets: 1 }
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
                    order: [[1, 'desc'], [0, 'asc']]
                });

                $('#tablaListadoEnergia').dataTable({
                    "iDisplayLength": 10,
                    "lengthMenu": [[10, 20, 50, 100], [10, 20, 50, 100]],
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
                    order: [[5, 'desc'], [1, 'asc'], [2, 'asc']]
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
    let periodo = $("#cbPeriodo option:selected").text();
    let version = $("#cbVersion option:selected").text();

    let mensajeError = '';
   
    switch (seccion) {
       
        case 'RolEmpresa': {
            let tablaBarraSinAnalizar = $("#tablaListadoRolEmpresa").DataTable();

            if (tablaBarraSinAnalizar.data().length == 0) {
                mensajeError = 'La grilla está vacía, no es posible descargar.';
            }
            break;
        }
        case 'Energia': {
            let tablaBarraDiferencia = $("#tablaListadoEnergia").DataTable();

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
        url: controlador + 'GenerarReporte',
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
                alert("Ha ocurrido un error interno no previsto en el sistema. Por favor comunique al Administrador del sistema.");
            }
        },
        error: function (err) {
            alert("Ha ocurrido un error interno no previsto en el sistema.Por favor comunique al Administrador del sistema.");
        }
    });
}

function verGraficoEmpresa(empresa) {
    let periodo = $("#cbPeriodo option:selected").text();
    $.ajax({
        type: 'POST',
        url: controlador + 'ObtenerRolHistorico',
        data: {
            empresa: empresa,
            periodo: periodo
        },
        dataType: 'json',
        global: false,
        success: function (result) {
           
            if (result.StrMensajeError == '') {
                setTimeout(function () {
                    $('#popupGrafico').bPopup({
                        autoClose: false
                    });
                }, 200);
                generarGraficoBarras(result, empresa);
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

function generarGraficoBarras(model, empresa) {
    // Obtener los datos de la tabla 
    let tableData = model.DatosHisRol.VTEARolHist;

    if (tableData.length == 0) {
        return;
    }

    $('#span_pop_title').html(empresa);

    if (graficoBarrasRol) {
        graficoBarrasRol.destroy();
    }
    
    // Asignamos un color específico a cada valor de Rol
    const coloresRol = {
        0: '#FFF9CC', // amarillo
        1: '#CC2E00', // rojo
        2: '#33C46A'  // verde
    };
        
    const seriesData = tableData.map(item => {
        const timestamp = new Date(item.TIME).getTime(); // timestamp en ms
        return {
            x: timestamp,
            y: parseInt(item.ROL),
            color: coloresRol[item.ROL]
        };
    });

    // Obtener el timestamp más reciente
    let maxTimestamp = Number.NEGATIVE_INFINITY;

    for (const item of tableData) {
        const time = new Date(item.TIME).getTime();
        if (time > maxTimestamp) {
            maxTimestamp = time;
        }
    }

    // Calcular 10 meses atrás
    const maxDate = new Date(maxTimestamp);
    const minDate = new Date(maxDate);
    minDate.setMonth(minDate.getMonth() - 9);
    const minTimestamp = minDate.getTime();

    Highcharts.setOptions({
        lang: {
            rangeSelectorFrom: 'Desde',
            rangeSelectorTo: 'Hasta'
        }
    });

    graficoBarrasRol = Highcharts.stockChart("AreaGrafico", {
        chart: {
            type: 'column'
        },
        rangeSelector: {          
            
            buttons: [
                { type: 'month', count: 9, text: '10m' },
                { type: 'ytd', text: 'Año Actual' },
                { type: 'all', text: 'Todos' }
            ],
            selected: 0,
            inputEnabled: true,
            inputDateFormat: '%Y-%m',
            inputEditDateFormat: '%Y-%m',
            buttonTheme: {
                width: 80                    
            }
        },
        title: { text: '' },
        xAxis: {
            type: 'datetime',
            min: minTimestamp,
            max: maxTimestamp,
            title: { text: "Periodo" },
            labels: {
                format: '{value:%Y-%m}'
            }
          
        },
        yAxis: {
            min: 0,
            max: 2,
            allowDecimals: false,
            title: { text: 'Rol' },
            tickInterval: 1,
            opposite: false 
        },
        tooltip: {
            enabled: false
        },
        series: [
            {
                name: 'Rol',
                data: seriesData,
                showInLegend: false
            }
        ]
    });
}

function verGraficoEmpresaEnergia(codigo, empresa, cliente, barra) {
    let periodo = $("#cbPeriodo").val();
    let version = $("#cbVersion").val();
    let periodoTexto = $("#cbPeriodo option:selected").text();
    let versionTexto = $("#cbVersion option:selected").text();

    window.location.href = controlador + "DetalleEmpresaEnergia?codigo=" + codigo + "&empresa=" + empresa + "&cliente=" + cliente + "&barra=" + barra
        + "&pericodi=" + periodo + "&recacodi=" + version + "&periodoTexto=" + periodoTexto + "&versionTexto=" + versionTexto;
}

