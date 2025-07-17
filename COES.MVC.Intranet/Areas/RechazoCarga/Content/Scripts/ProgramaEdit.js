var controlador = siteRoot + "rechazocarga/programa/";

$( /**
   * Llamadas iniciales
   * @returns {} 
   */
   $(document).ready(function () {
       //mostrarMensaje("Selecionar el periodo y la versión");            

       $('#btnGuadarEdicion').click(function () {
           guardarPrograma();
       });

       $('#btnCancelarEdicion').click(function () {
           cancelar();
       });

       $("#horizonteEdit").change(function () {
           habilitarHorizonte();
       });
       $('#fechaMensual').Zebra_DatePicker({
           direction: 30,
           format: 'Y-m',
           onSelect: function (date) {
               $('#fechaMensual').val(date);
               generarCodigoPrograma();
           }
       });
       $('#fechaDiaria').Zebra_DatePicker({
           direction: true,
           format: 'd/m/Y',
           onSelect: function (date) {
               $('#fechaDiaria').val(date);
               generarCodigoPrograma();
           }
       });
       $("#cbSemanaAnio").change(function () {
           generarCodigoPrograma();
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


function guardarPrograma() {

    var horizonte = $('#horizonteEdit').val();
    if (horizonte == "" || horizonte == "0") {
        mostrarAlerta('No se ha seleccionado el horizonte.');
        return;
    }

    var programaAbrev = $('#codigoProgramaEdit').val();
    if (programaAbrev == "" || programaAbrev == null) {
        mostrarAlerta('No se ha ingresado el código programa.');
        return;
    } 

    var horizonteReprograma = $('#hdnHorizonteReprograma').val();
    var codigoProgramaRef = $('#codigoProgramaRef').val();
    if (horizonte == horizonteReprograma) {        
        if (codigoProgramaRef == "" || codigoProgramaRef == null) {
            //mostrarAlerta('No se ha ingresado el código programa de Ref.');
            //return;
        }
    }

    var fechaMensual = $('#fechaMensual').val();
    var semana = $('#cbSemanaAnio').val();
    var fechaDiaria = $('#fechaDiaria').val();

    var esNuevo = true;

    $.ajax({
        type: 'POST',
        url: controlador + 'GuardarPrograma',
        data: {
            codPrograma: 0,
            horizonte: horizonte,
            programaAbrev: programaAbrev,            
            fechaMensual: fechaMensual,
            semana: semana,
            fechaDiaria: fechaDiaria,
            codigoProgramaRef: codigoProgramaRef,
            esNuevo: true
        },
        dataType: 'json',
        success: function (result) {

            if (result.success) {
                mostrarExito('Registro ingresado satisfactoriamente.');
                limpiarDatosPrograma();
            }
            else {
                alert(result.message);
            }

        },
        error: function () {
            mostrarMensaje('mensaje', 'Ha ocurrido un error', $tipoMensajeError, $modoMensajeCuadro);
        }
    });
}
function cancelar() {
    window.location.href = controlador + "Index";
}

function limpiarDatosPrograma() {    
    $('#horizonteEdit').val('0');
    $('#codigoProgramaEdit').val('');
    document.getElementById('trDiaria').style.display = 'none';
    document.getElementById('trMensual').style.display = 'none';
    document.getElementById('trSemanal').style.display = 'none';
    $('#codigoProgramaRef').val('');
   
}

function habilitarHorizonte() {

    var horizonte = $('#horizonteEdit').val();
    var horizonteSemanal = $('#hdnHorizonteSemanal').val();
    var horizonteMensual = $('#hdnHorizonteMensual').val();

    //var horizonteReprograma = $('#hdnHorizonteReprograma').val();

    document.getElementById('trDiaria').style.display = 'none';
    document.getElementById('trMensual').style.display = 'none';
    document.getElementById('trSemanal').style.display = 'none';
    document.getElementById('trCodigoProgramaRef').style.visibility = 'hidden';


    if (horizonte > 0) {
        switch (horizonte) {
            case horizonteSemanal: document.getElementById('trSemanal').style.display = ''; break;
            case horizonteMensual: document.getElementById('trMensual').style.display = ''; break;
            //case horizonteReprograma: document.getElementById('trCodigoProgramaRef').style.visibility = 'visible'; break;
            default: document.getElementById('trDiaria').style.display = ''; break;
        }
    }    

    $('#codigoProgramaEdit').val('');
}

function generarCodigoPrograma() {

    var horizonte = $('#horizonteEdit').val();
    var horizonteSemanal = $('#hdnHorizonteSemanal').val();
    var horizonteMensual = $('#hdnHorizonteMensual').val();

    var fechaMensual = $('#fechaMensual').val();
    var semana = $('#cbSemanaAnio').val();
    var fechaDiaria = $('#fechaDiaria').val();

    $.ajax({
        type: 'POST',
        url: controlador + 'GenerarCodigoPrograma',
        data: {
            
            horizonte: horizonte,
            fechaMensual: fechaMensual,
            semana: semana,
            fechaDiaria: fechaDiaria
        },
        dataType: 'json',
        success: function (result) {

            if (result.success) {
                $('#codigoProgramaEdit').val(result.message);
            }
            else {
                $('#codigoProgramaEdit').val('');
            }

        },
        error: function () {
            mostrarMensaje('mensaje', 'Ha ocurrido un error', $tipoMensajeError, $modoMensajeCuadro);
        }
    });

}