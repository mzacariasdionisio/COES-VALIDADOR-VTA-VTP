var controlador = siteRoot + 'informesosinergmin/indicadoresrpf/';
var fechaSistema;
$(function () {    
    $('#btnConsultar').click(function () {
        listar();
    });

    $('#txtFechaInic').Zebra_DatePicker({
        format: 'M Y',
        pair: $('#txtFechaFi'),
        direction: -1,
        onSelect: function(fechaFormat, fecha){
            $('#txtFechaInicio').val(fecha.substring(5, 7) + " " + fecha.substring(0, 4));
        }
    });

    $('#txtFechaFi').Zebra_DatePicker({
        format: 'M Y',
        onSelect: function (fechaFormat, fecha) {
            $('#txtFechaFin').val(fecha.substring(5, 7) + " " + fecha.substring(0, 4));
        }
    });

    $('#btnNuevo').click(function () {
        editarRegistro('');
    });

    $('#btnExportar').click(function () {
        exportar();
    });

    listar();
});

listar = function ()
{
    $.ajax({
        type: 'POST',
        url: controlador + 'listar',
        data: {
            fechaInicio: $('#txtFechaInicio').val(),
            fechaFin: $('#txtFechaFin').val()
        },
        success: function (evt) {
            $('#listado').html(evt);
            $('#tablaRpf').dataTable({
            });
        },
        error: function () {
            alert("Ha ocurrido un error");
        }
    });
}

editarRegistro = function (id)
{
    $.ajax({
        type: 'POST',
        data: { fecha: id },
        url: controlador + 'editar',
        success: function (evt){
            
            $('#contenidoEdicion').html(evt);
            setTimeout(function () {
                $('#popupEdicion').bPopup({
                    autoClose:false
                });
                
            }, 50);
            $('#btnGrabar').click(function () {
                grabarRegistro();
            });

            $('#btnCancelar').click(function () {
                $('#popupEdicion').bPopup().close();
            });

            $('#txtFecha').Zebra_DatePicker({
                format: 'm Y',
                onSelect: function (date) {
                    $('#hfFecha').val($('#txtFecha').val());
                }
            });
            formatoDecimal();
        },
        error: function () {
            alert("Ha ocurrido un error");
        }
    });
}
formatoDecimal = function () {
    $('#txtRpfenetotal').inputmask({
        placeholder: "",
        mask: "9[9][9][9][9].[9][9][9][9][9]",
        greedy: false
    });
    $('#txtRpfpotmedia').inputmask({
        placeholder: "",
        mask: "9[9][9][9][9].[9][9][9][9][9]",
        greedy: false
    });
    $('#txtEneindhidra').inputmask({
        placeholder: "",
        mask: "9[9][9][9][9].[9][9][9][9][9]",
        greedy: false
    });
    $('#txtPotindhidra').inputmask({
        placeholder: "",
        mask: "9[9][9][9][9].[9][9][9][9][9]",
        greedy: false
    });
}

grabarRegistro = function ()
{

    var mensaje = validarIngreso();

    if (mensaje == "") {
        $.ajax({
            type: 'POST',
            url: controlador + 'save',
            data: $('#frmRegistro').serialize(),
            dataType: 'json',
            success: function (result) {
                if (result == 1) {
                    listar();
                    mostrarExito();
                    $('#popupEdicion').bPopup().close();
                }
                else {
                    if (result == -2) {
                        mostrarAlerta("Ya existe un registro con esa fecha");
                    } else {
                        if (result == -3) {
                            mostrarAlerta("Los datos no pueden ser negativos");
                        }
                        else {
                            if (result == -4) {
                                mostrarAlerta("Solo es válido el 1 de cualquier mes");
                            }
                            mostrarError();
                        }
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

validarIngreso = function () {
    
    var mensaje = "";

    if ($('#txtFecha').val() == "") {
        mensaje = mensaje + "<li>Seleccione fecha</li>";
    }

    if ($('#txtRpfenetotal').val() == "")
    {
        mensaje = mensaje + "<li>Ingrese RPF Energia Total</li>";        
    }

    if ($('#txtRpfpotmedia').val() == "")
    {
        mensaje = mensaje + "<li>Ingrese RPF Potencia Media</li>";
    }

    return mensaje;
}

eliminarRegistro = function (id, rowId)
{
    if (confirm('¿Está seguro de eliminar registro?'))
    {
        $.ajax({
            type: 'POST',
            data: { fecha: id },
            url: controlador + 'eliminar',
            dataType: 'json',
            success: function (result) {              
                if (result == 1) {
                    $('#row' + rowId).remove();
                }
                else
                {
                    alert("Ha ocurrido en el servidor");
                }
            },
            error: function () {
                alert("Ha ocurrido un error");
            }
        });
    }
}

exportar = function () {
    $.ajax({
        type: 'POST',
        url: controlador + "exportarIndicadorRPF",
        dataType: 'json',
        cache: false,
        data: {
            fechaInicio: $('#txtFechaInicio').val(),
            fechaFin: $('#txtFechaFin').val()
        },
        success: function (resultado) {
            if (resultado == 1) {
                location.href = controlador + "descargarIndicadorRPF";
            }
            else {
                mostrarError();
            }
        },
        error: function () {
            mostrarError();
        }
    });
}

mostrarError = function ()
{
    $('#mensajeRegistro').removeClass();
    $('#mensajeRegistro').text("Ha ocurrido un error");
    $('#mensajeRegistro').addClass("action-error");
}

mostrarAlerta = function(mensaje)
{
    $('#mensajeRegistro').removeClass();
    $('#mensajeRegistro').html(mensaje);
    $('#mensajeRegistro').addClass("action-alert");
}

mostrarExito = function () {
    $('#mensaje').removeClass();
    $('#mensaje').text("Proceso ejecutado correctamente");
    $('#mensaje').addClass("action-exito");
}

validarNumero = function(item, evt) {
    evt = (evt) ? evt : window.event;
    var charCode = (evt.which) ? evt.which : evt.keyCode;

    if (charCode == 46) {
        var regex = new RegExp(/\./g)
        var count = $(item).val().match(regex).length;
        if (count > 1) {
            return false;
        }
    }
    if (charCode > 31 && (charCode < 48 || charCode > 57)) {
        return false;
    }
    return true;
}