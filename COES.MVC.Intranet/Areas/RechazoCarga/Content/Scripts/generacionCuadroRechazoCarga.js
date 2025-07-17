var controlador = siteRoot + "rechazocarga/generacioncuadrosrechazocarga/";

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
       
       $('#btnConsultar').click(function () {
           pintarBusqueda();
       });
             
       $('#btnAgregar').click(function () {
           agregarCuadroPrograma();
       });

       $('#fechaMensual').Zebra_DatePicker({
           direction: 30,
           format: 'Y-m',
           onSelect: function (date) {
               $('#fechaMensual').val(date);

           }
       });
       $('#fechaDiaria').Zebra_DatePicker({
           direction: true,
           format: 'd/m/Y',
           onSelect: function (date) {
               $('#fechaDiaria').val(date);

           }
       });

       $('#fechaNuevo').Zebra_DatePicker({
           format: 'd/m/Y'
       });

       $("#horizonteDuplicar").change(function () {
           habilitarHorizonte();
       });

       $('#btnGenerarCopia').click(function () {
           generarCopia();
       });

       $('#btnCerrarCopia').click(function () {
           $("#popupDuplicarCuadro").bPopup().close();
       });

       $("#btnExportarExcel").click(function () {
           
           exportarExcel();
       });  

       $('#horaInicioNuevo').inputmask({
           mask: "h:s",
           placeholder: "hh:mm",
           alias: "datetime",
           hourFormat: "24"
       });

       $('#horaFinNuevo').inputmask({
           mask: "h:s",
           placeholder: "hh:mm",
           alias: "datetime",
           hourFormat: "24"
       });

       $("#btnBuscar").click(function () {

           buscarEvento();
       });  
      
   }));

var agregarCuadroPrograma = function () {
    window.location.href = siteRoot + "rechazocarga/ProgramaRechazoCarga/Inicio?codigoPrograma=0&codigoCuadroPrograma=0";
}
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

var pintarBusqueda =
    /**
    * Pinta el listado de cuadro de programas
    * @returns {} 
    */
    function () {

        $.ajax({
            type: "POST",
            url: controlador + "ListarGeneracionCuadroPrograma",
            data: {
                horizonte: $("#horizonte").val(),
                configuracion: $("#configuracion").val(),
                estado: $("#estado").val(),
                fecIni: $("#fechaIni").val(),
                fecFin: $("#fechaFin").val(),
                energiaRechazadaInicio: $("#energiaIni").val(),
                energiaRechazadaFin: $("#energiaFin").val(),
                perfil: $("#hdnPerfil").val(),
                sinPrograma: document.getElementById('chkSinPrograma').checked ? 1 : 0
            },
            success: function (evt) {

                $('#listado').css("width", $('#mainLayout').width() + "px");
                $('#listado').html(evt);
                $('#tabla').dataTable({
                    "destroy": "true",
                    "sPaginationType": "full_numbers",
                    "ordering": false,
                    "searching": true,
                    "paging": true,
                    "lengthMenu": [
                        [25, 50, -1],
                        [25, 50, "Todos"]
                    ],
                    "info": true,
                    "iDisplayLength": 25
                    , "autoWidth": false
                    , "columnDefs": [
                        {
                           width:"80px", targets: [1]
                        },
                        {
                        render: function (data, type, full, meta) {
                            return "<div class='text-wrap width-200'>" + data + "</div>";
                        },
                            targets: 2
                        },
                        {
                            width: "120px", targets: [3,4]
                        }
                    ]
                });

                

                mostrarMensaje("Consulta generada.");
            },
            error: function () {
                mostrarError('Opción Consultar: Ha ocurrido un error');
            }
        });
    };


function modificarCuadroProgramacion(codigoPrograma,codigoCuadroPrograma) {
    window.location.href = siteRoot + "rechazocarga/ProgramaRechazoCarga/Inicio?codigoPrograma=" + codigoPrograma + "&codigoCuadroPrograma=" + codigoCuadroPrograma;
}

function reprogramarCuadroProgramacion(codigoPrograma, codigoCuadroPrograma) {
    if (confirm("¿Desea ver el cuadro antes de reprogramar?")) {
        window.location.href = siteRoot + "rechazocarga/ProgramaRechazoCarga/Inicio?codigoPrograma=" + codigoPrograma + "&codigoCuadroPrograma=" + codigoCuadroPrograma;
    } else {
        
    }    
}

function eliminarCuadroProgramacion(rccuadcodi) {
    if (confirm("¿Desea eliminar el registro seleccionado?")) {
        $.ajax({
            type: 'POST',
            dataType: 'json',
            url: controlador + "EliminarCuadroProgramacion",
            data: {
                rccuadCodi: rccuadcodi
            },
            success: function (result) {

                if (result.success) {
                    alert("Se ha eliminado el registro correctamente.");
                    pintarBusqueda();
                }
                else {
                    alert(result.message);
                }

            },
            error: function () {
                mostrarError('Opción Eliminar: Ha ocurrido un error.');
            }
        });
    }
}

function noEjecutarCuadroProgramacion(rccuadcodi) {
    if (confirm("¿Desea NO EJECUTAR el cuadro seleccionado?")) {
        $.ajax({
            type: 'POST',
            dataType: 'json',
            url: controlador + "NoEjecutarCuadroProgramacion",
            data: {
                rccuadCodi: rccuadcodi
            },
            success: function (result) {

                if (result.success) {
                    alert("Se ha cambiado el estado correctamente.");
                    pintarBusqueda();
                }
                else {
                    alert(result.message);
                }

            },
            error: function () {
                mostrarError('Opción No Ejecutar: Ha ocurrido un error.');
            }
        });
    }
}

function habilitarHorizonte() {

    var horizonte = $('#horizonteDuplicar').val();
    var horizonteSemanal = $('#hdnHorizonteSemanal').val();
    var horizonteMensual = $('#hdnHorizonteMensual').val();
    //alert(horizonte);
    document.getElementById('tdDiaria').style.display = 'none';
    document.getElementById('tdMensual').style.display = 'none';
    document.getElementById('tdSemanal').style.display = 'none';
    
    if (horizonte > 0) {
        switch (horizonte) {
            case horizonteSemanal: document.getElementById('tdSemanal').style.display = ''; break;
            case horizonteMensual: document.getElementById('tdMensual').style.display = ''; break;
            default: document.getElementById('tdDiaria').style.display = ''; break;
        }
    }

}

function mostrarCuadroDuplicar(codigoPrograma, codigoCuadroPrograma) {
    $('#hdnCodigoCuadroDuplicar').val(codigoCuadroPrograma);
    limpiarDatosCuadroDuplicar();
    $("#popupDuplicarCuadro").bPopup({
        autoClose: false
    });
}

function mostrarDatosPrograma(tipoPrograma) {
    
    switch (parseInt(tipoPrograma.value)) {
        case 1: {
            document.getElementById('trProgramaExistente').style.display = '';
            document.getElementById('trNuevoPrograma').style.display = 'none'; break;
        };
        case 2: {
            document.getElementById('trProgramaExistente').style.display = 'none';
            document.getElementById('trNuevoPrograma').style.display = ''; break;
        };
        default: {
            document.getElementById('trProgramaExistente').style.display = 'none';
            document.getElementById('trNuevoPrograma').style.display = 'none'; break;
        }
    }
}

function generarCopia() {
    
    var tipoPrograma = $('input:radio[name=tipoPrograma]:checked').val();
    var programaDuplicar = $("#programaduplicar").val();
    var horizonteDuplicar = $("#horizonteDuplicar").val();
    var fechaMensual = $("#fechaMensual").val();
    var semanaAnio = $("#cbSemanaAnio").val();
    var fechaDiaria = $("#fechaDiaria").val();

    var fechaNuevo = $("#fechaNuevo").val();
    var horaInicioNuevo = $("#horaInicioNuevo").val();
    var horaFinNuevo = $("#horaFinNuevo").val();

    if (tipoPrograma == 1) {
        if (programaDuplicar == 0) {
            alert('No se ha seleccionado un programa.');
            return;
        }
    } else if (tipoPrograma == 2) {
        if (horizonteDuplicar == 0) {
            alert('No se ha seleccionado un horizonte.');
            return;
        }

        if (horizonteDuplicar == $("#hdnHorizonteDiario").val() && fechaDiaria == '') {
            alert('Ingrese una fecha diaria.');
            return;
        }

        if (horizonteDuplicar == $("#hdnHorizonteMensual").val() && fechaMensual == '') {
            alert('Ingrese un mes.');
            return;
        }

        if (horizonteDuplicar == $("#hdnHorizonteSemanal").val() && semanaAnio == '0') {
            alert('Seleccione una semana.');
            return;
        }
    } else if (tipoPrograma == 3) {
        fechaDiaria = '';
        fechaMensual = '';
        semanaAnio = 0;
    }

    if (fechaNuevo == '') {
        alert('Ingrese una fecha.');
        return;
    }

    if (horaInicioNuevo == '') {
        alert('Ingrese una hora inicio.');
        return;
    }

    if (horaFinNuevo == '') {
        alert('Ingrese una hora fin.');
        return;
    }

    const timeReg = /^([0-9]|0[0-9]|1[0-9]|2[0-3]):[0-5][0-9]$/;

    if (!horaInicioNuevo.match(timeReg)) {

        alert('Hora inicio no tiene formato correcto.');
        return;
    }

    if (!horaFinNuevo.match(timeReg)) {

        alert('Hora fin no tiene formato correcto.');
        return;
    }

    var fechahoraIni = fechaNuevo + ' ' + horaInicioNuevo;
    var fechahoraFin = fechaNuevo + ' ' + horaFinNuevo;

    var fechahoraInicioFormateada = formatearFecha(fechahoraIni);
    var fechahoraFinFormateada = formatearFecha(fechahoraFin);
    if (fechahoraFinFormateada < fechahoraInicioFormateada) {
        mostrarAlerta('Hora fin debe ser mayor a Hora inicio');
        return false;
    }

    var codigoCuadroDuplicar = $("#hdnCodigoCuadroDuplicar").val();

    $.ajax({
        type: "POST",
        url: controlador + "GenerarCopiaCuadroPrograma",
        data: {
            codigoCuadroPrograma: codigoCuadroDuplicar,
            tipoPrograma: tipoPrograma,
            codigoProgramaDuplicar: programaDuplicar,
            horizonte: horizonteDuplicar,
            fechaMensual: fechaMensual,
            fechaDiaria: fechaDiaria,
            semana: semanaAnio,
            fechaHoraInicio: fechahoraIni,
            fechaHoraFin: fechahoraFin
        },
        success: function (result) {

            if (result.success) {
                $('#popupDuplicarCuadro').bPopup().close();
                pintarBusqueda();
                alert("Se ha duplicado el cuadro de programa.");
            }
            else {
                alert(result.message);
            }
        },
        error: function () {
            mostrarError('Opción Duplicar: Ha ocurrido un error');
        }
    });
}

function formatearFecha(cadena) {
    var partes = cadena.split("/");
    var partesAnioHoraMin = partes[2].split(" ");
    var partesHoraMin = partesAnioHoraMin[1].split(":");
    var dia = partes[0];
    var mes = parseInt(partes[1]) - 1;
    var anio = partesAnioHoraMin[0];
    var hora = partesHoraMin[0];
    var minuto = partesHoraMin[1];
    var fecha = new Date(anio, mes, dia, hora, minuto, 0, 0);
    return fecha;
};

function exportarExcel() {
    
    $.ajax({
        type: "POST",
        url: controlador + "GenerarReporte",
        data: {
            horizonte: $("#horizonte").val(),
            configuracion: $("#configuracion").val(),
            estado: $("#estado").val(),
            fecIni: $("#fechaIni").val(),
            fecFin: $("#fechaFin").val(),
            energiaRechazadaInicio: $("#energiaIni").val(),
            energiaRechazadaFin: $("#energiaFin").val(),
            perfilUsuario: $("#hdnPerfil").val(),
            sinPrograma: document.getElementById('chkSinPrograma').checked ? 1 : 0
        },
        success: function (result) {

            if (result != "-1") {
                window.location.href = controlador + 'DescargarFormato?file=' + result;                
            }
            else {
                alert("Error al generar el archivo.");
            }
        },
        error: function () {
            mostrarError('Opción Exportar Excel: Error al generar el archivo.');
        }
    });
};

function mostrarAsignarEvento(codigoCuadroPrograma) {
    $('#hdnCodigoCuadroEvento').val(codigoCuadroPrograma);
    $("#codigoEvento").val('');
    $("#nombreEvento").val('');
    $("#listadoEventos").html("");
    $("#popupEvento").bPopup({
        autoClose: false
    });
}

function buscarEvento() {
    var codigoEvento = document.getElementById("codigoEvento").value;
    var nombreEvento = document.getElementById("nombreEvento").value;
    var codigoCuadroPrograma = $('#hdnCodigoCuadroEvento').val();

    $.ajax({
        type: 'POST',
        datatype: 'json',
        url: controlador + "ListarEventos",
        data: {
            codigoEvento: codigoEvento,
            nombreEvento: nombreEvento,
            codigoCuadroPrograma: codigoCuadroPrograma
        },
        success: function (result) {
            //$('#listadoEmpresas').css("width", "400px");
            $('#listadoEventos').css("width", "90%");
            $('#listadoEventos').html(result);
            $('#tablaListaEventos').dataTable({
                "filter": false,
                "ordering": true,
                "paging": false,
                "scrollY": 200,
                "scrollX": true,
                "bDestroy": true,
                "autoWidth": false,
                "columnDefs": [
                    { "width": "10%", "targets": 0 },
                    { "width": "20%", "targets": 1 },
                    { "width": "50%", "targets": 2 },
                    { "width": "20%", "targets": 3 }
                ]
            });
            $('#btnSeleccionarEvento').click(function () {
                seleccionarEvento();
            });

            $('#btnCerrarEvento').click(function () {
                $('#popupEvento').bPopup().close();
            });
        },
        error: function () {
            alert("Error al cargar la Lista de Eventos.");
        }
    });
}

function seleccionarEvento() {
    var evento = $('input:radio[name=codEvento]:checked').val();
    var codigoCuadro = $('#hdnCodigoCuadroEvento').val();
    

    if (evento == "" || evento == null) {
        if (confirm('No ha seleccionado un evento. ¿Desea continuar sin asignar evento?')) {
            evento = '';
        } else {
            return false;
        }        
    }
    
    $.ajax({
        type: "POST",
        url: controlador + "ActualizarCuadroProgramaEvento",
        data: {
            codigoCuadroPrograma: codigoCuadro,
            evento: evento
        },
        success: function (result) {

            if (result.success) {
                $('#popupEvento').bPopup().close();
                pintarBusqueda();
                if (evento == '') {
                    alert("Se ha actualizado el cuadro de programa.");
                }
                else {
                    alert("Se ha asociado el evento al cuadro de programa.");
                }

                
            }
            else {
                alert(result.message);
            }
        },
        error: function () {
            mostrarError('Opción Evento: Ha ocurrido un error');
        }
    });    
    
}

function limpiarDatosCuadroDuplicar() {
    var tipoProgramaExistente = document.getElementById('tipoProgramaExistente');
    var tipoProgramaNuevo = document.getElementById('tipoProgramaNuevo');
    var tipoSinPrograma = document.getElementById('tipoSinPrograma');
    tipoProgramaExistente.checked = true;
    tipoProgramaNuevo.checked = false;
    tipoSinPrograma.checked = false;    

    $('#programaduplicar').val('0');
    $('#cbSemanaAnio').val('0');
    $('#fechaNuevo').val('');
    $('#horaInicioNuevo').val('');
    $('#horaFinNuevo').val('');

    document.getElementById('trProgramaExistente').style.display = '';
    document.getElementById('trNuevoPrograma').style.display = 'none'; 
}