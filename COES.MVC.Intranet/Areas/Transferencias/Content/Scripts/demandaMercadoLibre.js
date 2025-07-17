var controlador = siteRoot + "transferencias/demandamercadolibre/";

$( /**
   * Llamadas iniciales
   * @returns {} 
   */
   $(document).ready(function () {       

       $('#mesReferencia').Zebra_DatePicker({
           format: 'm/Y'
       });
            

       $('#btnConsultar').click(function () {
           buscar();
       });

       $('#btnExportar').click(function () {
           exportarDatos();
       });
       
   }));

function mostrarExito(mensaje) {
    $('#mensaje').removeClass();
    $('#mensaje').html(mensaje);
    $('#mensaje').addClass('action-exito');
}

function mostrarError(mensaje) {
    $('#mensaje').removeClass();
    $('#mensaje').html(mensaje);
    $('#mensaje').addClass('action-error');
}

function mostrarAlerta(mensaje) {
    $('#mensaje').removeClass();
    $('#mensaje').html(mensaje);
    $('#mensaje').addClass('action-alert');
}

function mostrarMensaje(mensaje) {
    $('#mensaje').removeClass();
    $('#mensaje').html(mensaje);
    $('#mensaje').addClass('action-message');
}

function buscar() {
    var periodo = $("#mesReferencia").val();

    if (periodo == '') {
        mostrarAlerta('Debe seleccionar un periodo');
        return;
    }
    pintarBusqueda(periodo);
    pintarBusquedaOsinergmin(periodo);
}

var pintarBusqueda =   
    function (periodo) {
        $.ajax({
            type: "POST",
            url: controlador + "ListarDemandas",
            data: {
                periodoInicial: periodo,
                tipoEmpresa: $("#tipoEmpresa").val(),
                nombreEmpresa: $("#empresa").val()
            },
            success: function (evt) {

                $('#listadoInformacionComparativa').css("width", $('#mainLayout').width() + "px");
                $('#listadoInformacionComparativa').html(evt);
                $('#tablaComparativa').dataTable({
                    "sPaginationType": "full_numbers",
                    "destroy": "true",
                    "aaSorting": [[0, "asc"]]
                });

                mostrarMensaje("Consulta generada.");
            },
            error: function () {
                mostrarError('Opción Consultar: Ha ocurrido un error');
            }
        });
    };
var pintarBusquedaOsinergmin =   
    function (periodo) {
        $.ajax({
            type: "POST",
            url: controlador + "ListarDemandasOsinergmin",
            data: {
                periodoInicial: periodo,
                tipoEmpresa: $("#tipoEmpresa").val(),
                nombreEmpresa: $("#empresa").val()
            },
            success: function (evt) {

                $('#listadoInformacionComparativaOsinergmin').css("width", $('#mainLayout').width() + "px");
                $('#listadoInformacionComparativaOsinergmin').html(evt);
                $('#tablaComparativaOsinergmin').dataTable({
                    "sPaginationType": "full_numbers",
                    "destroy": "true",
                    "aaSorting": [[0, "asc"]]
                });

                mostrarMensaje("Consulta generada.");
            },
            error: function () {
                mostrarError('Opción Consultar: Ha ocurrido un error');
            }
        });
    };

function exportarDatos() {
    var periodo = $("#mesReferencia").val();

    if (periodo == '') {
        mostrarAlerta('Debe seleccionar un periodo');
        return;
    }
    if (confirm("¿Desea exportar los datos?")) {
        $.ajax({
            type: 'POST',
            dataType: 'json',
            url: controlador + "GenerarReporte",
            data: {
                periodoInicial: periodo,
                tipoEmpresa: $("#tipoEmpresa").val(),
                nombreEmpresa: $("#empresa").val()
            },
            success: function (result) {

                if (result != "-1") {
                    window.location.href = controlador + 'DescargarFormato?file=' + result;
                    //mostrarExito("Se ha eliminado el registro correctamente.");
                    //pintarBusqueda();
                }
                else {
                    alert("Error al generar el archivo.");
                }

            },
            error: function () {
                mostrarError('Opción Reporte: Ha ocurrido un error');
            }
        });
    }
}