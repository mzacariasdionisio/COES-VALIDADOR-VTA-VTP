var controlador = siteRoot + "mediciones/formato/";

$(function () {

    $('#btnAgregar').click(function () {
        addPunto();
    });

    $('#btnGrabarConfiguracion').click(function () {
        grabarConfiguracion();
    });

    $('#btnCancelarConfiguracion').click(function () {
        $('#popupEdicion').bPopup().close();
    });

    cargar();
});

cargar = function ()
{
    $.ajax({
        type: 'POST',
        url: controlador + "lista",       
        success: function (evt) {
            $('#listado').html(evt);
            $('#tabla').dataTable({
                "iDisplayLength": 50
            });
        },
        error: function () {
            mostrarError();
        }
    });
}

cargarCentrales = function () {

    $('#cbUnidad').get(0).options.length = 0;
    $('#cbUnidad').get(0).options[0] = new Option("--SELECCIONE--", "");

    $.ajax({
        global: false,
        type: 'POST',
        url: controlador + 'cargarcentrales',
        dataType: 'json',
        data: { idEmpresa: $('#cbEmpresa').val() },
        cache: false,
        success: function (aData) {
            $('#cbCentral').get(0).options.length = 0;
            $('#cbCentral').get(0).options[0] = new Option("--SELECCIONE--", "");
            $.each(aData, function (i, item) {
                $('#cbCentral').get(0).options[$('#cbCentral').get(0).options.length] = new Option(item.Text, item.Value);
            });
        },
        error: function () {
            mostrarError();
        }
    });
};

cargarUnidades = function () {
    $.ajax({
        global: false,
        type: 'POST',
        url: controlador + 'CargarUnidades',
        dataType: 'json',
        data: { ptoCentral: $('#cbCentral').val() },
        cache: false,
        success: function (aData) {
            $('#cbUnidad').get(0).options.length = 0;
            $('#cbUnidad').get(0).options[0] = new Option("--SELECCIONE--", "");
            $.each(aData, function (i, item) {
                $('#cbUnidad').get(0).options[$('#cbUnidad').get(0).options.length] = new Option(item.Text, item.Value);
            });
        },
        error: function () {
            mostrarError();
        }
    });
};

configurarPorTipo = function () {
    if ($('#cbTipo').val() == "1") {
        $('#cbUnidad').css("display", "block");
    }
    else {
        $('#cbUnidad').css("display", "none");
    }
}

addPunto = function ()
{
    $.ajax({
        type: 'POST',
        url: controlador + "agregar",       
        success: function (evt) {
            $('#edicionGrupo').html(evt);            
            setTimeout(function () {
                $('#popupUnidad').bPopup({
                    easing: 'easeOutBack',
                    speed: 450,
                    transition: 'slideDown'
                });
            }, 50);
            $('#alerta').css("display", "none");
            $('#mensaje').css("display", "none");
        },
        error: function () {
            mostrarError();
        }
    });
}

agregarPunto = function ()
{
    if (confirm('¿Está seguro de agregar el punto?'))
    {
        var ptoCentral = "";
        var ptoUnidad = "";
        var validacion = "";
        var indicador = 0;

        if ($('#cbTipo').val() == "1")
        {
            if ($('#cbUnidad').val() != "") {
                ptoCentral = $('#cbCentral').val();
                ptoUnidad = $('#cbUnidad').val();
            }
            else { validacion = "Seleccione una unidad."; }

            indicador = 1;
        }
        if ($('#cbTipo').val() == "2")
        {
            if ($('#cbCentral').val() != "") {
                ptoCentral = $('#cbCentral').val();
            }
            else { validacion = "Seleccione una central."; }

            indicador = 2;
        }

        if (validacion == "") {
            $.ajax({               
                type: 'POST',
                url: controlador + 'agregarpunto',
                dataType: 'json',
                data: { indicador: indicador, ptoCentral: ptoCentral, ptoUnidad: ptoUnidad },
                cache: false,
                success: function (resultado) {
                    if (resultado == 1) {
                        mostrarExitoOperacion("El punto se ha grabado correctamente...!");
                        $('#popupUnidad').bPopup().close();
                        cargar();
                    }
                    if (resultado == -1) {
                        mostrarErrorGrabar();
                    }
                    if (resultado == 2)
                    {
                        mostrarAlerta("Existe agregado el punto de la central correspondiente.");
                    }
                    if (resultado == 3) {
                        mostrarAlerta("Ya existe el registro.");
                    }
                    if (resultado == 4) {
                        mostrarAlerta("Existen unidades de la central seleccionada.");
                    }
                    if (resultado == 5) {
                        mostrarAlerta("Ya existe el registro");
                    }
                },
                error: function () {
                    mostrarErrorGrabar();
                }
            });
        }
        else
        {
            mostrarAlerta(validacion);
        }
    }
};

eliminarPunto = function (ptoMediCodi)
{
    if (confirm('¿Está seguro de eliminar este registro?')) {
        $.ajax({
            type: 'POST',
            url: controlador + 'eliminarpunto',
            dataType: 'json',
            data: { ptoMediCodi: ptoMediCodi },
            cache: false,
            success: function (resultado) {
                if (resultado == 1) {
                    mostrarExitoOperacion("El registro se elimino correctamente...!");
                    cargar();
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
}

editarPunto = function (empresa, ptomedicion, indicador, central, equipo, minimo, maximo) {
    $('#lblEmpresa').text(empresa);
    $('#lblPtomedicion').text(ptomedicion);
    $('#lblIndicador').text(indicador);
    $('#lblCentral').text(central);
    $('#lblUnidad').text(equipo);
    $('#txtValorMinimo').val(minimo);
    $('#txtValorMaximo').val(maximo);
    $('#hfCodigoConfiguracion').val(ptomedicion);

    setTimeout(function () {
        $('#popupEdicion').bPopup({
            easing: 'easeOutBack',
            speed: 450,
            transition: 'slideDown'
        });
    }, 50);
}

grabarConfiguracion = function () {

    $.ajax({
        type: 'POST',
        url: controlador + 'grabarconfiguracion',
        dataType: 'json',
        data: {
            ptomedicodi: $('#hfCodigoConfiguracion').val(),
            minimo: $('#txtValorMinimo').val(),
            maximo: $('#txtValorMaximo').val()
        },
        cache: false,
        success: function (resultado) {
            if (resultado == 1) {
                mostrarExitoOperacion("Operación correcta...!");
                $('#popupEdicion').bPopup().close();
                cargar();

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
    $('#mensaje').removeClass("action-exito");
    $('#mensaje').addClass("action-error");
    $('#mensaje').text("Ha ocurrido un error");
    $('#mensaje').css("display", "block");
}

mostrarAlerta = function (mensaje)
{
    $('#alerta').removeClass("action-error");
    $('#alerta').addClass("action-alert");
    $('#alerta').text(mensaje);
    $('#alerta').css("display", "block");
}

mostrarErrorGrabar = function ()
{
    $('#alerta').removeClass("action-alert");
    $('#alerta').addClass("action-error");
    $('#alerta').text("Ha ocurrido un error.");
    $('#alerta').css("display", "block");
}

mostrarExitoOperacion = function (mensaje)
{
    $('#mensaje').removeClass("action-error");
    $('#mensaje').addClass("action-exito");
    $('#mensaje').text(mensaje);
    $('#mensaje').css("display", "block");
}

validarNumero = function (item, evt) {
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