var controlador = siteRoot + 'reservafrianodoenergetico/periodoresumen/';


$(function () {

    $('#txtNrperrFecCreacion').inputmask({
        mask: "1/2/y",
        placeholder: "dd/mm/yyyy",
        alias: "datetime"        
    });

    $('#txtNrperrFecCreacion').Zebra_DatePicker({
        readonly_element: false,
        onSelect: function (date) {
            $('#txtNrperrFecCreacion').val(date);
        }
    });

    $('#txtNrperrFecModificacion').inputmask({
        mask: "1/2/y",
        placeholder: "dd/mm/yyyy",
        alias: "datetime"        
    });


    $('#txtNrperrFecModificacion').Zebra_DatePicker({
        readonly_element: false,
        onSelect: function (date) {
            $('#txtNrperrFecModificacion').val(date);
        }
    });

    $('#btnCancelar').click(function () {
        document.location.href = controlador;
    });

    $('#btnCancelar2').click(function () {
        document.location.href = controlador;
    });

    $('#btnObservaciones').click(function () {
        verObservaciones($('#cbNrperCodi').val());
    });

    $(document).ready(function () {

        $('#rbNrperrEliminadoS').prop('checked', true);
        if ($('#hfNrperrEliminado').val() == 'S') { $('#rbNrperrEliminadoS').prop('checked', true); }
        if ($('#hfNrperrEliminado').val() == 'N') { $('#rbNrperrEliminadoN').prop('checked', true); }
        
       $('#cbNrperCodi').val($('#hfNrperCodi').val());
        $('#cbNrsmodCodi').val($('#hfNrsmodCodi').val());

        if ($('#hfAccion').val() == 0) {
            $('#btnGrabar').hide();
            $('#btnGrabar2').hide();
            $('#btnCancelar').hide();
            $('#btnCancelar2').hide();
        }

    });

    $('#btnGrabar').click(function () {
        grabar();
    });

    $('#btnGrabar2').click(function () {
        grabar();
    });
});


verObservaciones = function (nrperCodi) {

    $.ajax({
        type: 'POST',
        url: controlador + "verobservaciones",
        dataType: 'json',
        data: {            
            idNrperCodi: nrperCodi
        },
        success: function (result) {
            if (result == "-1") {
                mostrarAlerta("<li>Ha ocurrido un error en el proceso seleccionado</li>");
            } else {
                //retorna los periodo y procesos correctos (separados por coma). Ej: percodi,proccodi1,proccodi2,...,proccodin. 1,1,2
                //actualiza los periodo-proceso correcto(s)

                if (result == "") {
                    alert("No hay observaciones")
                }
                else
                {
                    alert(result);
                }
            }
        },
        error: function () {
            mostrarError();
        }
    });
}

mostrarAlerta = function (mensaje) {
    $('#mensaje').removeClass();
    $('#mensaje').addClass('action-alert');
    $('#mensaje').html(mensaje);
}

mostrarExito = function () {
    $('#mensaje').removeClass();
    $('#mensaje').addClass('action-exito');
    $('#mensaje').html('La operación se realizó con éxito.');
}

procesarConceptoPeriodo = function (nrcptcodi, nrperCodi,descripcion) {

    var periodoId = document.getElementById('cbNrperCodi');
    var periodo = periodoId.options[periodoId.selectedIndex].text;

    if (confirm('¿Está seguro de procesar "' + descripcion + '" para el periodo "' + periodo + '" ?')) {
        
        $.ajax({
            type: 'POST',
            url: controlador + "procesarconceptoperiodo",
            dataType: 'json',
            data: {
                nrcptcodi: nrcptcodi,
                nrperCodi: nrperCodi
            },
            success: function(result) {
                if (result == "-1") {
                    mostrarAlerta("<li>Ha ocurrido un error en el proceso seleccionado</li>");
                } else {
                    //retorna los periodo y procesos correctos (separados por coma). Ej: percodi,proccodi1,proccodi2,...,proccodin. 1,1,2
                    //actualiza los periodo-proceso correcto(s)
                    //---
                    $.ajax({
                        type: 'POST',
                        url: controlador + "actualizarproceso",
                        dataType: 'json',
                        data: {
                            nrperCodi: nrperCodi,
                            resultado: result
                        },
                        success: function(result) {
                            //refrescar ventana
                            mostrarExito();
                            document.location.href = controlador +
                                "/editar?idNrsmodCodi=" +
                                $('#hfNrsmodCodi').val() +
                                "&idNrperCodi=" +
                                $('#hfNrperCodi').val();

                        },
                        error: function() {
                            mostrarError();
                        }
                    });
                    //---
                }
            },
            error: function() {
                mostrarError();
            }
        });
    }
}

validarRegistro = function () {

    var mensaje = "<ul>";
    var flag = true;
    
    $('#hfNrperCodi').val($('#cbNrperCodi').val());
    $('#hfNrsmodCodi').val($('#cbNrsmodCodi').val());
    
    if ($('#hfNrsmodCodi').val() == 0) {
        mensaje += "<li>Debe elegir Módulo</li>";
        flag = false;
    }

    if ($('#hfNrperCodi').val() == 0)
    {
        mensaje += "<li>Debe elegir Periodo</li>";
        flag = false;
    }
    
    if (flag) mensaje = "";
    return mensaje;
}

grabar = function () {
    //validar modulo-periodo
    var mensaje = validarRegistro();
    
    if (mensaje == "") {
        mostrarExito();
       
        $.ajax({
            type: 'POST',
            url: controlador + "grabar",
            dataType: 'json',
            data: {
                idNrsmodCodi: $('#hfNrsmodCodi').val(),
                idNrperCodi: $('#hfNrperCodi').val()
            },            
            success: function (result) {

                
                if (result == "1") {
                    mostrarAlerta("<li>El Periodo para el Módulo seleccionado ya ha sido creado previamente</li>");
                }
                else {

                    if (result == "0") {
                        mostrarExito();
                        document.location.href = controlador + "/editar?idNrsmodCodi=" + $('#hfNrsmodCodi').val() + "&idNrperCodi=" + $('#hfNrperCodi').val();
                    }
                    else {
                        mostrarError();
                    }
                }
            },
            error: function () {
                mostrarError();
            }
        });
    }
    else {
        mostrarAlerta(mensaje);
    }
}

mostrarError = function () {
    alert("Ha ocurrido un error");
}

