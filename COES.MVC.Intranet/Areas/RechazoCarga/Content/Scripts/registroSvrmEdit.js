var controlador = siteRoot + "rechazocarga/registrosvrm/";

$( /**
   * Llamadas iniciales
   * @returns {} 
   */
   $(document).ready(function () {
       //mostrarMensaje("Selecionar el periodo y la versión");

       $('#fechaRegistro').Zebra_DatePicker({
           format: 'd/m/Y'
       });

       $('#btnBuscar').click(function () {
           buscarEmpresa();
       });

       $('#btnBuscarEmpresa').click(function () {

           document.getElementById("empresaBuscar").value = '';
           $('#listadoEmpresas').html('');
           muestraBuscarEmpresa();
       });

       $('#btnGuadarEdicion').click(function () {
           guardarRegistroSvrm();
       });

       $('#btnCancelarEdicion').click(function () {
           cancelar();
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
function buscarEmpresa() {
    var empresaIngresada = document.getElementById("empresaBuscar").value;
    
    $.ajax({
        type: 'POST',
        datatype: 'json',
        url: controlador + "ListarEmpresas",
        data: {
            empresa: empresaIngresada
        },
        success: function (result) {
            $('#listadoEmpresas').css("width", "90%");
            $('#listadoEmpresas').html(result);
            $('#tablaListaEmpresas').dataTable({
                "filter": false,
                "ordering": true,
                "paging": false,
                "scrollY": 150,
                "scrollX": true,
                "bDestroy": true,
                "autoWidth": false,
                "columnDefs": [
                      { "width": "20%", "targets": 0 },
                      { "width": "80%", "targets": 1 }
                ]
            });
            $('#btnSeleccionarEmpresa').click(function () {
                seleccionarEmpresa();
            });
        },
        error: function () {
            alert("Error al cargar la Lista de Empresas.");
        }
    });
}

function muestraBuscarEmpresa() {
    $("#empresaBuscar").val('');
    $("#listadoEmpresas").html("");
    $("#popupEdicion").bPopup({
        autoClose: false
    });
}

function seleccionarEmpresa() {
    var empresa = $('input:radio[name=codEmpresa]:checked').val(); 
    if (empresa == "" || empresa == null) {
        alert('No se ha seleccionado una empresa');
        return false;
    }
    var datos = empresa.split('/');
    document.getElementById("empresaEdit").value = datos[1];
    document.getElementById("empresaSeleccionada").value = datos[0]; 

    $('#popupEdicion').bPopup().close();

    //luego de seleccionar la empresa se debe de cargar la lista de punto de medicion segun el tipo de empresa
    listarPuntoMedicion(datos[0]);
}

function guardarRegistroSvrm() {
        
    var empresaseleccionada = document.getElementById("empresaSeleccionada").value;
    if (empresaseleccionada == "" || empresaseleccionada == null) {
        mostrarAlerta('La empresa no ha sido seleccionada.');
        return;
    }

    var puntomedicion = $('#puntoMedicion').val();
    if (puntomedicion == "" || puntomedicion == null) {
        mostrarAlerta('El punto de medición no ha sido seleccionado.');
        return;
    }

    var eracmfhp = $('#eracmfHP').val();
    if (eracmfhp == "" || eracmfhp == null) {
        mostrarAlerta('No se ha ingresado ERACMF en HP.');
        return;
    }

    var eracmfhfp = $('#eracmfHFP').val();
    if (eracmfhfp == "" || eracmfhfp == null) {
        mostrarAlerta('No se ha ingresado ERACMF en HFP.');
        return;
    }

    var eracmthp = $('#eracmtHP').val();
    if (eracmthp == "" || eracmthp == null) {
        mostrarAlerta('No se ha ingresado ERACMT en HP.');
        return;
    }

    var eracmthfp = $('#eracmtHFP').val();
    if (eracmthfp == "" || eracmthfp == null) {
        mostrarAlerta('No se ha ingresado ERACMT en HFP.');
        return;
    }

    var maxdemcont = $('#maxDemCont').val();
    if (maxdemcont == "" || maxdemcont == null) {
        mostrarAlerta('No se ha ingresado máxima demanda contratada.');
        return;
    }

    var maxdemdisp = $('#maxDemDisp').val();
    if (maxdemdisp == "" || maxdemdisp == null) {
        mostrarAlerta('No se ha ingresado máxima demanda disponible.');
        return;
    }

    var maxdemcom = $('#maxDemCom').val();
    if (maxdemcom == "" || maxdemcom == null) {
        mostrarAlerta('No se ha ingresado máxima demanda comprometida.');
        return;
    }

    var documento = $('#documentoEdit').val();
    if (documento == "" || documento == null) {
        alert('No se ha ingresado nombre documento.');
        return;
    }

    var fecharegistro = $('#fechaRegistro').val();
    if (fecharegistro == "" || fecharegistro == null) {
        mostrarAlerta('No se ha ingresado la fecha de registro.');
        return;
    }
    var estado = $('#estadoEdit').val();
    if (estado == "" || estado == null) {
        mostrarAlerta('No se ha seleccionado un estado.');
        return;
    }
   
    var esNuevo = true;

    $.ajax({
        type: 'POST',
        url: controlador + 'GuardarRegistroSvrm',
        data: {
            codigoRegistroSvrm: 0,
            empresa: empresaseleccionada,
            puntoMedicion: puntomedicion,
            eracmfHP: eracmfhp,
            eracmfHFP:eracmfhfp,
            eracmtHP:eracmthp,
            eracmtHFP:eracmthfp,
            maxDemCont:maxdemcont, 
            maxDemDisp: maxdemdisp,
            maxDemComp:maxdemcom,
            documento: documento,
            fechaRegistro: fecharegistro,
            estado: estado,            
            esNuevo: true
        },
        dataType: 'json',
        success: function (result) {

            if (result.success) {
                mostrarExito('registro ingresado');
                limpiarDatosRegistroSvrm();
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
    window.location.href = controlador + "index";
}

function listarPuntoMedicion(empcodi) {

    $('option', '#puntoMedicion').remove();
    $.ajax({
        type: 'POST',
        url: controlador + "ObtenerListaPuntoMedicion",
        dataType: 'json',
        data: {
            codigoEmpresa: empcodi,
            codigoEquipo: 0
        },
        success: function (aData) {
            $('#puntoMedicion').get(0).options.length = 0;
            $('#puntoMedicion').get(0).options[0] = new Option("--SELECCIONE--", "");
            $.each(aData, function (i, item) {
                $('#puntoMedicion').get(0).options[$('#puntoMedicion').get(0).options.length] = new Option(item.Equinomb, item.Equicodi);
            });
        },
        error: function () {
            alert("Error al cargar la Lista de Empresas.");
        }
    });
}

function soloNumeros(e) {
    var key = window.Event ? e.which : e.keyCode;
    return (key <= 13 || (key >= 48 && key <= 57) || key == 46);
}


function limpiarDatosRegistroSvrm() {
    document.getElementById("empresaSeleccionada").value = '';

    $('#empresaEdit').val('');
    $('#puntoMedicion').val('');
    $('#eracmfHP').val('');
    $('#eracmfHFP').val('');
    $('#eracmtHP').val('');
    $('#eracmtHFP').val('');
    $('#maxDemCont').val('');
    $('#maxDemDisp').val('');
    $('#maxDemCom').val('');
    $('#documentoEdit').val('');
    $('#fechaRegistro').val('');
    $('#estadoEdit').val('');
    
}