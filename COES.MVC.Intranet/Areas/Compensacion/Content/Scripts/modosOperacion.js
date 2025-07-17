var controlador = siteRoot + "compensacion/general/";

$( /**
   * Llamadas iniciales
   * @returns {} 
   */
   $(document).ready(function () {
       mostrarMensaje("Selecionar el periodo y la versión");
             
       $('#btnConsultar').click(function () {
           if (ValidarVersion('Consultar', 1)) {
               pintarBusqueda();
           }
       });

       $('#btnValidar').click(function () {
           validarConcepto();
       });            
              

       $("#pericodi").change(function () { ObtenerPeriodoCalculo(this.value, ''); });


       //Inicializamos la pantalla
       ObtenerPeriodoCalculo($("#pericodi").val(), '');
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

function exportar() {

    var arr = "N";
    if ($('#arranque').is(":checked")) {
        arr = "S"
    }

    var par = "N";
    if ($('#parada').is(":checked")) {
        par = "S"
    }

    $.ajax({
        type: 'POST',
        url: controlador + 'exportarHorasOperacion',
        data: {
            pecacodi: $("#pecacodi").val(),
            empresa: $("#empresa").val(),
            central: $("#central").val(),
            grupo: $("#grupo").val(),
            modo: $("#modo").val(),
            tipo: $("#tipo").val(),
            fecIni: $("#fechaini").val(),
            fecFin: $("#fechafin").val(),
            arranque: arr,
            parada: par
        },
        dataType: 'json',
        success: function (result) {
            if (result != -1) {
                document.location.href = controlador + 'descargar?formato=' + 1 + '&file=' + result
                mostrarMensaje("Exportación realizada");
            }
            else {
                mostrarError('Opcion Exportar: Ha ocurrido un error');
            }
        },
        error: function () {
                mostrarError('Opcion Exportar: Ha ocurrido un error');
         }
    });  
}

var pintarBusqueda =
    /**
    * Pinta el listado de periodos según el año seleccionado
    * @returns {} 
    */
    function () {
                
        $.ajax({
            type: "POST",
            url: controlador + "listarModosOperacionCompensaciones",
            data: {
                pecacodi: $("#pecacodi").val()
            },
            success: function (evt) {

                $('#listado').css("width", $('#mainLayout').width() + "px");
                $('#listado').html(evt);
                $('#tabla').dataTable({
                    "ordering": false,
                    "paging": false,
                    "scrollY": 340,
                    "scrollX": true,
                    "bDestroy": true
                });
                mostrarMensaje("Consulta generada");
            },
            error: function () {
                    mostrarError('Opcion Consultar: Ha ocurrido un error');
            }
        });
    };

function accionModo(accion, grupocodi) {

    var pecacodi = $("#pecacodi").val();
    $.ajax({
        type: 'POST',
        dataType: 'json',
        url: controlador + "AccionModo",
        data: {
            accion: accion,
            grupocodi: grupocodi,
            pecacodi: pecacodi
        },
        success: function (result) {

            if (result.success) {
                //mostrarExito("Se ha eliminado el rango de hora")
                pintarBusqueda();
            }
            else {
                alert(result.message);
            }

        },
        error: function () {
            mostrarError('Ha ocurrrido un error.');
        }
    });
}

function validarConcepto () {
    window.location.href = controlador + "ValidarConceptos";
}

function modificarModoOperacion(grupocodi) {
    window.location.href = controlador + "AsignacionBarrasModosOperacion?grupocodi=" + grupocodi;
}

// DSH 10-08-2017 : Se agrego por Requerimiento
function modificarRangoHora(pecaCodi, hopCodi) {
    $('#esNuevo').val(0);

    $("#modoOperacion").prop("disabled", true);
    $("#tipoOperacion").prop("disabled", true);
    $("#codigoRango").prop("disabled", true);
    $("#fechaInicio").prop("disabled", true);
    $("#fechaFin").prop("disabled", true);
    $.ajax({
        type: 'POST',
        url: controlador + 'ObtenerHoraOperacion',
        data: {
            pecacodi: pecaCodi,
            hopcodi: hopCodi
        },
        dataType: 'json',
        success: function (data) {
            var jsonData = JSON.parse(data);
            var fechahora;

            $('#modoOperacion').val(jsonData.ModoOperacion);
            $('#tipoOperacion').val(jsonData.TipoOperacion);
            $('#codigoRango').val(jsonData.Hopcodi);
            fechahora = obtenerFechaHora('date',jsonData.Crhophorini);
            $('#fechaInicio').val(fechahora);
            fechahora = obtenerFechaHora('time', jsonData.Crhophorini);
            $('#horaInicio').val(fechahora);
            fechahora = obtenerFechaHora('date',jsonData.Crhophorfin);
            $('#fechaFin').val(fechahora);
            fechahora = obtenerFechaHora('time', jsonData.Crhophorfin);
            $('#horaFin').val(fechahora);
            $("#popupEdicion").bPopup({
                autoClose: false
            });
        },
        error: function () {
            mostrarMensaje('mensaje', 'Ha ocurrido un error', $tipoMensajeError, $modoMensajeCuadro);
        }
    });

}


/*
* Guarda la edición del incremento/reducción
*/
function guardarEdicion() {

    var pecacodi = $('#pecacodi').val();
    if (pecacodi == "" || pecacodi == null) {
        alert('No se ha podido identificar el periodo');
        return;
    }

    var grupocodi = $('#modoOperacion').val();
    if (grupocodi == "" || grupocodi == null) {
        alert('El modo de operación no es válido');
        return;
    }

    var tipocodi = $('#tipoOperacion').val();
    if (tipocodi == "" || tipocodi == null) {
        alert('El tipo de operación no es válido');
        return;
    }

    var hopcodi = $('#codigoRango').val();
    if (hopcodi == "" || hopcodi == null) {
        alert('No se ha podido identificar el codigo de rango');
        return;
    }

    var horaini = $('#horaInicio').val();
    var horafin = $('#horaFin').val();
    
    if (!validarHoraMinuto(horaini)) {
        alert('La hora de la fecha inicio no es válida');
        return;
    }

    if (!validarHoraMinuto(horafin)) {
        alert('La hora de la fecha fin no es válida');
        return;
    }

    var fechaini = $('#fechaInicio').val() + ' ' + horaini ;
    var fechafin = $('#fechaFin').val() + ' ' + horafin  ;

    var esNuevo = false;

    if ($('#esNuevo').val() == 1) {
        esNuevo = true;
    }

    $.ajax({
        type: 'POST',
        url: controlador + 'GuardarRangoHoraOperacion',
        data: {
            Pecacodi: pecacodi,
            Hopcodi: hopcodi,
            Crhophorini: fechaini,
            Crhophorfin: fechafin,
            EsNuevo: esNuevo
        },
        dataType: 'json',
        success: function (result) {

            if (result.success) {
                $('#popupEdicion').bPopup().close();
                pintarBusqueda();
                if (esNuevo) {
                    mostrarExito("Se ha creado el rango de hora");
                }
                else {
                    mostrarExito("Se ha modificado el rango de hora");
                }
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

function eliminarRangoHora(pecaCodi, hopCodi) {
    if (confirm("¿Desea eliminar el rango de hora, cuyo código es " + hopCodi)) {
        $.ajax({
            type: 'POST',
            dataType: 'json',
            url: controlador + "EliminarRangoHoraOperacion",
            data: {
                pecacodi: pecaCodi,
                hopcodi: hopCodi
            },
            success: function (result) {

                if (result.success) {
                    mostrarExito("Se ha eliminado el rango de hora")
                    pintarBusqueda();
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

function obtenerData() {
    $.ajax({
        type: 'POST',
        url: controlador + 'obtenerDataHorasOperacion',
        data: {
            pecacodi: $("#pecacodi").val()
        },
        dataType: 'json',
        success: function (result) {
            $("select").change();
        },
        error: function () {
            mostrarError('Ha ocurrido un error', $tipoMensajeError, $modoMensajeCuadro);
        }
    });
}

function ObtenerListaCentral(valor, selected) {

    if (valor != "") {
        $.ajax({
            type: 'POST',
            url: controlador + 'obtenerListaCentral',
            data: {
                emprcodi: valor
            },
            dataType: 'json',
            success: function (data) {
                var jsonData = JSON.parse(data);
                dwr.util.removeAllOptions("modo");
                dwr.util.removeAllOptions("grupo");
                dwr.util.removeAllOptions("central");
                dwr.util.addOptions("central", jsonData, 'id', 'name');
                dwr.util.setValue("central", selected);
            },
            error: function () {
                mostrarError('Ha ocurrido un error', $tipoMensajeError, $modoMensajeCuadro);
            }
        });
    }
    else {
        dwr.util.removeAllOptions("modo");
        dwr.util.removeAllOptions("grupo");
        dwr.util.removeAllOptions("central");

    }
}

function ObtenerListaGrupo(valor, selected) {
    var empresa;
    empresa = $("#empresa").val();

    if (valor != "") {
        $.ajax({
            type: 'POST',
            url: controlador + 'obtenerListaGrupo',
            data: {
                emprcodi: empresa, 
                grupopadre: valor
            },
            dataType: 'json',
            success: function (data) {
                var jsonData = JSON.parse(data);
                dwr.util.removeAllOptions("modo");
                dwr.util.removeAllOptions("grupo");
                dwr.util.addOptions("grupo", jsonData, 'id', 'name');
                dwr.util.setValue("grupo", selected);
            },
            error: function () {
                mostrarError('Ha ocurrido un error');
            }
        });
    }
    else {
        dwr.util.removeAllOptions("modo");
        dwr.util.removeAllOptions("grupo");
    }
}

function ObtenerListaModo(valor, selected) {
    var empresa;
    empresa = $("#empresa").val();

    if (valor != "") {
        $.ajax({
            type: 'POST',
            url: controlador + 'obtenerListaModo',
            data: {
                emprcodi: empresa,
                grupopadre: valor
            },
            dataType: 'json',
            success: function (data) {
                var jsonData = JSON.parse(data);
                dwr.util.removeAllOptions("modo");
                dwr.util.addOptions("modo", jsonData, 'id', 'name');
                dwr.util.setValue("modo", selected);
            },
            error: function () {
                mostrarError('Ha ocurrido un error', $tipoMensajeError, $modoMensajeCuadro);
            }
        });
    }
    else {
        dwr.util.removeAllOptions("modo");
    }
}

function ObtenerPeriodoCalculo(valor, selected) {

    if (valor != "") {
        $.ajax({
            type: 'POST',
            url: controlador + 'obtenerPeriodoCalculo',
            data: {
                pericodi: valor
            },
            dataType: 'json',
            success: function (data) {
                var jsonData = JSON.parse(data);
                dwr.util.removeAllOptions("pecacodi");
                dwr.util.addOptions("pecacodi", jsonData, 'id', 'name');
                dwr.util.setValue("pecacodi", selected);
            },
            error: function () {
                mostrarError('Ha ocurrido un error', $tipoMensajeError, $modoMensajeCuadro);
            }
        });
    }
    else {
        dwr.util.removeAllOptions("pecacodi");
    }
}

function ValidarVersion(titulo_opcion, limpiar_listado) {
    if ($("#pecacodi").val() == "" || $("#pecacodi").val() == null) {
              
        mostrarAlerta("Opcion " + titulo_opcion + ": Verificar la selección del periodo y la versión");

        return false;
    }
    else {
        return true;
    }
}



function obtenerFechaHora(tipo, valor) {
    
    var str, year, month, day, hour, minute, d, finalDate;

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
        finalDate =  hour + ":" + minutes;
    }

    return finalDate;
}

function pad(num) {
    num = "0" + num;
    return num.slice(-2);
}

function soloHorasMinutos(e) {
    var key = window.Event ? e.which : e.keyCode
    return (key <= 13 || (key >= 48 && key <= 57) ||  key ==58 || key == 46);
}


function validarHoraMinuto(valor) {
    
    var isValid = /^([0-1]?[0-9]|2[0-3]):([0-5][0-9])(:[0-5][0-9])?$/.test(valor);
    return isValid;
}