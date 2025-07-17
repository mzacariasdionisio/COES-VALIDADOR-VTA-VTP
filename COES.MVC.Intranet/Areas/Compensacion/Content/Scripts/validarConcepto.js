var controlador = siteRoot + "compensacion/general/";

$( /**
   * Llamadas iniciales
   * @returns {} 
   */
   $(document).ready(function () {
       //mostrarMensaje("Selecionar el periodo y la versión");    

       $('#btnValidar').click(function () {
           procesarValidacion();
       });

       $('#fechaIni').Zebra_DatePicker({
           format: 'd/m/Y'
       });

       $('#btnRegresar').click(function () {
           regresar();
       });
       //$("#pericodi").change(function () { ObtenerPeriodoCalculo(this.value, ''); });


       //Inicializamos la pantalla
       //ObtenerPeriodoCalculo($("#pericodi").val(), '');
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

function procesarValidacion()
{
    var crcvalcodi = $("#crcvalcodi").val();

    if (crcvalcodi == '' || crcvalcodi == '0') {
        mostrarAlerta('Debe seleccionar un concepto.');
        return;
    }

    var fecha = $("#fechaIni").val();

    if (fecha == '' ) {
        mostrarAlerta('Debe ingresar una fecha.');
        return;
    }

    $.ajax({
        type: "POST",
        url: controlador + "ListErroresConceptos",
        data: {
            crcvalcodi: crcvalcodi,
            fechadatos: fecha
        },
        success: function (evt) {
            $("#listado").html(evt);
            $("#tabla").dataTable({
                "scrollY": 314,
                "scrollX": false,
                "sDom": "t",
                "ordering": false,
                "bDestroy": true,
                "bPaginate": false,
                "iDisplayLength": 50
            });
        },
        error: function () {
            mostrarMensaje("mensaje", $mensajeErrorGeneral, $tipoMensajeError, $modoMensajeCuadro);
        }
    });
}

function regresar() {
    window.location.href = controlador + "ModosOperacion";
}

