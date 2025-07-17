var controlador = siteRoot + "rechazocarga/registrosvrm/";

$( /**
   * Llamadas iniciales
   * @returns {} 
   */
   $(document).ready(function () {
       //mostrarMensaje("Selecionar el periodo y la versión");

       $('#fechaIni').Zebra_DatePicker({
           format: 'd/m/Y'
       });

       $('#fechaFin').Zebra_DatePicker({
           format: 'd/m/Y'
       });

       $('#fechaRegistro').Zebra_DatePicker({
           format: 'd/m/Y'
       });

       $('#btnConsultar').click(function () {
           pintarPaginado();
           pintarBusqueda(1);
       });

       $('#btnGuadarEdicion').click(function () {
           guardarEdicion();
       });

       $('#btnCancelarEdicion').click(function () {
           cancelarEdicion();
       });

       $('#btnAgregar').click(function () {
           nuevoRegistro();
       });

       $('#btnBuscarEmpresa').click(function () {
           muestraBuscarEmpresa();
       });

       $('#btnBuscar').click(function () {
           buscarEmpresa();
       });

       $('#btnSeleccionarEmpresa').click(function () {
           seleccionarEmpresa();
       });

       $('#btnExportar').click(function () {
           exportarExcel();
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

/**
    * Pinta el listado de esquema unifilares
    * @returns {} 
    */
var pintarBusqueda =
    function (id) {

        var registros = $('#cbRegistros').val();
        //alert(registros);
        if (registros == undefined || registros == null) {
            registros = 50;
        }

        $.ajax({
            type: "POST",
            url: controlador + "ListarRegistroSvrm",
            data: {
                empresa: $("#empresa").val(),
                codigoSuministro: $("#codigoSuministro").val(),
                fecIni: $("#fechaIni").val(),
                fecFin: $("#fechaFin").val(),
                maxDemComprometidaIni: $("#maxDemComprometidaIni").val(),
                maxDemComprometidaFin: $("#maxDemComprometidaFin").val(),
                nroPagina: id,
                nroRegistros: registros
            },
            success: function (evt) {

                $('#listado').css("width", $('#mainLayout').width() + "px");
                $('#listado').html(evt);
                $('#tabla').dataTable({
                    "ordering": false,
                    "paging": false,
                    "scrollY": 340,
                    "scrollX": true,
                    "sDom": 't',
                    "bPaginate": false,
                    "fixedColumns": true
                });

                if ($("#cbRegistros").length) {
                    $('#cbRegistros').val(registros);
                }
                mostrarMensaje("Consulta generada.");
            },
            error: function () {
                mostrarError('Opcion Consultar: Ha ocurrido un error');
            }
        });
    };
function nuevoRegistro() {
   
    window.location.href = controlador + "EditRegistroSvrm";
}

function guardarEdicion() {

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
       
    var codigoRegistroSvrm = parseInt(document.getElementById('codigoRegistroSvrm').value);
    //codigoRegistroSvrm = parseInt(codigoRegistroSvrm);

    $.ajax({
        type: 'POST',
        url: controlador + 'GuardarRegistroSvrm',
        data: {
            codigoRegistroSvrm: codigoRegistroSvrm,
            empresa: empresaseleccionada,
            puntoMedicion: puntomedicion,
            eracmfHP: eracmfhp,
            eracmfHFP: eracmfhfp,
            eracmtHP: eracmthp,
            eracmtHFP: eracmthfp,
            maxDemCont: maxdemcont,
            maxDemDisp: maxdemdisp,
            maxDemComp: maxdemcom,
            documento: documento,
            fechaRegistro: fecharegistro,
            estado: estado,
            esNuevo: false
        },
        dataType: 'json',
        success: function (result) {

            if (result.success) {
                $('#popupEdicion').bPopup().close();
                alert('Registro actualizado correctamente.');
               
                pintarBusqueda(1);
                //limpiarDatosRegistroSvrm();
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

function cancelarEdicion() {

    $('#popupEdicion').bPopup().close();

}
function modificarRegistroSvrm(rcsvrmcodi) {
    $('#esNuevo').val(0);

    $.ajax({
        type: 'POST',
        url: controlador + 'ObtenerRegistroSvrm',
        data: {
            rcsvrmcodi: rcsvrmcodi
        },
        dataType: 'json',
        success: function (data) {
            var jsonData = JSON.parse(data);
            var fechahora;
            $('#empresaEdit').val(jsonData.Emprrazsocial);
            $('#empresaSeleccionada').val(jsonData.Emprcodi);
            //$('#puntoMedicion').val(jsonData.Equinomb);      
            $('#eracmfHP').val(jsonData.Rcsvrmhperacmf);
            $('#eracmfHFP').val(jsonData.Rcsvrmhfperacmf);
            $('#eracmtHP').val(jsonData.Rcsvrmhperacmt);
            $('#eracmtHFP').val(jsonData.Rcsvrmhfperacmt);
            $('#maxDemCont').val(jsonData.Rcsvrmmaxdemcont);
            $('#maxDemDisp').val(jsonData.Rcsvrmmaxdemdisp);
            $('#maxDemCom').val(jsonData.Rcsvrmmaxdemcomp);
            $('#documentoEdit').val(jsonData.Rcsvrmdocumento);
            fechahora = obtenerFechaHora('date', jsonData.Rcsvrmfechavigencia);
            $('#fechaRegistro').val(fechahora);            
            $('#codigoRegistroSvrm').val(jsonData.Rcsvrmcodi);
            $('#estadoEdit').val(jsonData.Rcsvrmestado);

            listarPuntoMedicion(jsonData.Emprcodi, jsonData.Equicodi);
            $("#popupEdicion").bPopup({
                autoClose: false
            });
        },
        error: function () {
            mostrarMensaje('mensaje', 'Ha ocurrido un error', $tipoMensajeError, $modoMensajeCuadro);
        }
    });

}

function eliminarRegistroSvrm(rcsvrmcodi) {
    if (confirm("¿Desea eliminar el registro seleccionado?")) {
        $.ajax({
            type: 'POST',
            dataType: 'json',
            url: controlador + "EliminarRegistroSvrm",
            data: {
                rcsvrmcodi: rcsvrmcodi
            },
            success: function (result) {

                if (result.success) {
                    mostrarExito("Se ha eliminado el registro");
                    pintarBusqueda(1);
                }
                else {
                    alert(result.message);
                }

            },
            error: function () {
                mostrarError();
            }
        });
    }
}

function muestraBuscarEmpresa() {
    document.getElementById("empresaBuscar").value = '';
    $('#listadoEmpresas').html('');

    $("#popupBuscarEmpresa").bPopup({
        autoClose: false
    });
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
            //$('#listadoEmpresas').css("width", "400px");
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
function seleccionarEmpresa() {
    var empresa = $('input:radio[name=codEmpresa]:checked').val();
    if (empresa == "" || empresa == null) {
        alert('No se ha seleccionado una empresa');
        return false;
    }
    var datos = empresa.split('/');
    document.getElementById("empresaEdit").value = datos[1];
    document.getElementById("empresaSeleccionada").value = datos[0];

    //al seleccionar la empresa, se debe de cargar el punto de medicion, solo si es usuario libre

    $('#popupBuscarEmpresa').bPopup().close();

    //luego de seleccionar la empresa se debe de cargar la lista de punto de medicion segun el tipo de empresa
    listarPuntoMedicion(datos[0], 0);
}
function listarPuntoMedicion(empcodi, equicodi) {

    $('option', '#puntoMedicion').remove();
    $.ajax({
        type: 'POST',
        url: controlador + "ObtenerListaPuntoMedicion",
        dataType: 'json',
        data: {
            codigoEmpresa: empcodi,
            codigoEquipo: equicodi
        },
        success: function (aData) {
            $('#puntoMedicion').get(0).options.length = 0;
            $('#puntoMedicion').get(0).options[0] = new Option("--SELECCIONE--", "");
            $.each(aData, function (i, item) {
                $('#puntoMedicion').get(0).options[$('#puntoMedicion').get(0).options.length] = new Option(item.Equinomb, item.Equicodi);
            });

            if (equicodi > 0) {
                $('#puntoMedicion').val(equicodi);
            }
        },
        error: function () {
            alert("Error al cargar Puntos de Medición.");
        }
    });
}

function obtenerFechaHora(tipo, valor) {

    var str, year, month, day, hour, minutes, d, finalDate;

    str = valor.replace(/\D/g, "");
    d = new Date(parseInt(str));

    year = d.getFullYear();
    month = pad(d.getMonth() + 1);
    day = pad(d.getDate());
    hour = pad(d.getHours());
    minutes = pad(d.getMinutes());

    if (tipo == "datetime") {
        finalDate = day + "/" + month + "/" + year + " " + hour + ":" + minutes;
    }
    if (tipo == "date") {
        finalDate = day + "/" + month + "/" + year;
    }
    if (tipo == "time") {
        finalDate = hour + ":" + minutes;
    }

    return finalDate;
}
function pad(num) {
    num = "0" + num;
    return num.slice(-2);
}

function soloNumeros(e) {
    var key = window.Event ? e.which : e.keyCode;
    return (key <= 13 || (key >= 48 && key <= 57) || key == 46);
}

function exportarExcel() {

    $.ajax({
        type: 'POST',
        dataType: 'json',
        url: controlador + 'GenerarReporte',
        data: {
            empresa: $("#empresa").val(),
            codigoSuministro: $("#codigoSuministro").val(),
            fecIni: $("#fechaIni").val(),
            fecFin: $("#fechaFin").val(),
            maxDemComprometidaIni: $("#maxDemComprometidaIni").val(),
            maxDemComprometidaFin: $("#maxDemComprometidaFin").val()
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
            mostrarAlerta("Error al generar el archivo");
        }
    });
}

pintarPaginado = function () {

    var registros = $('#cbRegistros').val();

    if (registros == undefined || registros == null) {
        registros = 50;
    }

    if ($('#cbPeriodoIni').val() != "") {

        $.ajax({
            type: 'POST',
            url: controlador + "paginado",
            data: {
                empresa: $("#empresa").val(),
                codigoSuministro: $("#codigoSuministro").val(),
                fecIni: $("#fechaIni").val(),
                fecFin: $("#fechaFin").val(),
                maxDemComprometidaIni: $("#maxDemComprometidaIni").val(),
                maxDemComprometidaFin: $("#maxDemComprometidaFin").val(),
                nroRegistrosPag: registros
            },
            success: function (evt) {
                $('#paginado').html(evt);
                mostrarPaginado();
            },
            error: function () {
                alert("Ha ocurrido un error");
            }
        });

    }
    else {
        mostrarAlerta("Seleccione periodo.");
    }
}

function mostrarPaginado() {
    $.ajax({
        type: 'POST',
        url: controlador + 'paginado',
        success: function (evt) {
            $('#paginado').html(evt);
            mostrarPaginado();
        },
        error: function () {
            alert("Error al cargar el paginado");
        }
    });
}