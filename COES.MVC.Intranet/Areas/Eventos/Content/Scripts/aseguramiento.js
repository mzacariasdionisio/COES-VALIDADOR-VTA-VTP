var controler = siteRoot + 'eventos/registro/'

$(function () {

    $('#txtHoraInicial').inputmask({
        mask: "1/2/y h:s:s",
        placeholder: "dd/mm/yyyy hh:mm:ss",
        alias: "datetime",
        hourFormat: "24"
    });

    $('#txtHoraInicial').Zebra_DatePicker({
        readonly_element: false,
        onSelect: function (date) {
            $('#txtHoraInicial').val(date + " 00:00:00");
        }
    });

    $('#txtHoraFinal').inputmask({
        mask: "1/2/y h:s:s",
        placeholder: "dd/mm/yyyy hh:mm:ss",
        alias: "datetime",
        hourFormat: "24"
    });

    $('#txtHoraFinal').Zebra_DatePicker({
        readonly_element: false,
        onSelect: function (date) {
            $('#txtHoraFinal').val(date + " 00:00:00");
        }
    });

    $('#cbTipoEvento').change(function () {
        cargarSubCausaEvento();
    });

    $('#btnBuscarEquipo').click(function () {
        openBusquedaEquipo();
    });

    $('#btnGrabar').click(function () {
        grabar();
    });

    $('#btnCancelar').click(function () {
        document.location.href = siteRoot + 'eventos/evento/index';
    });

    $('#btnConvertir').click(function () {
        convertir();
    });

    cargarPrevio();
    cargarBusquedaEquipo();
    /*
    $(document).ready(function () {        
        
        $('#cbTipoOperacion').val($('#hfTipoOperacion').val());
    }
    )*/
});


cargarPrevio = function () {
    $('#cbTipoEvento').val($('#hfTipoEvento').val());
    $('#cbSubCausaEvento').val($('#hfSubCausaEvento').val());
    $('#cbEmpresaEvento').val($('#hfIdEmpresaEvento').val());
    $('#cbTipoOperacion').val($('#hfTipoOperacion').val());
    //$('#cbTipoOperacion').val(401);
    console.log($('#hfTipoOperacion').val());

    if ($('#hfCodigoEvento').val() != '0') {
        $('#btnConvertir').css('display', 'block');
    }
    else {
        $('#btnConvertir').css('display', 'none');
    }
}

cargarSubCausaEvento = function () {

    if ($('#cbTipoEvento').val() != "") {
        $.ajax({
            type: 'POST',
            url: controler + 'cargarsubcausaevento',
            dataType: 'json',
            data: { idTipoEvento: $('#cbTipoEvento').val() },
            cache: false,
            success: function (aData) {
                $('#cbSubCausaEvento').get(0).options.length = 0;
                $('#cbSubCausaEvento').get(0).options[0] = new Option("-SELECCIONE-", "");
                $.each(aData, function (i, item) {
                    $('#cbSubCausaEvento').get(0).options[$('#cbSubCausaEvento').get(0).options.length] =
                        new Option(item.Subcausadesc, item.Subcausacodi);
                });
            },
            error: function () {
                mostrarError();
            }
        });
    }
    else {
        $('#cbSubCausaEvento').get(0).options.length = 0;
    }
}

cargarBusquedaEquipo = function () {
    $.ajax({
        type: "POST",
        url: siteRoot + "eventos/busqueda/index",
        data: {},
        global: false,
        success: function (evt) {
            $('#busquedaEquipo').html(evt);
            if ($('#hfCodigoEvento').val() == "0") {
                openBusquedaEquipo();
            }
        },
        error: function (req, status, error) {
            mostrarError();
        }
    });
}

openBusquedaEquipo = function () {
    $('#busquedaEquipo').bPopup({
        easing: 'easeOutBack',
        speed: 450,
        transition: 'slideDown'
    });
    $('#txtFiltro').focus();
}

seleccionarEquipo = function (idEquipo, substacion, equipo, empresa, idEmpresa) {

    $('#cbEmpresaEvento').val(idEmpresa);
    $('#spanEquipo').text(substacion + ' ' + equipo);
    $('#busquedaEquipo').bPopup().close();
    $('#hfIdEquipo').val(idEquipo);

    $('#busquedaEquipo').bPopup().close();
}

grabar = function () {
    var mensaje = validarRegistro();
    if (mensaje == "") {
        $.ajax({
            type: 'POST',
            url: controler + "grabaraseguramiento",
            dataType: 'json',
            data: $('#frmRegistro').serialize(),
            success: function (result) {
                if (result > 1) {
                    mostrarExito();
                    $('#hfCodigoEvento').val(result);
                    $('#btnConvertir').css('display', 'block');
                }
                if (result == -1) {
                    mostrarError();
                }
                if (result == -2) {
                    mostrarAlerta("La fecha inicial debe ser menor a la final.");
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

validarRegistro = function () {

    var mensaje = "<ul>";
    var flag = true;
    if ($('#cbTipoEvento').val() == '') {
        mensaje = mensaje + "<li>Seleccione el tipo de evento.</li>";
        flag = false;
    }

    if ($('#txtHoraInicial').val() == '') {
        mensaje = mensaje + "<li>Ingrese la hora inicial.</li>";
        flag = false;
    }
    else {
        if (!$('#txtHoraInicial').inputmask("isComplete")) {
            mensaje = mensaje + "<li>Ingrese hora inicial.</li>";
            flag = false;
        }
    }

    if ($('#txtHoraFinal').val() == '') {
        //mensaje = mensaje + "<li>Ingrese la hora final.</li>";
        //flag = false;
    }
    else {
        if (!$('#txtHoraFinal').inputmask("isComplete")) {
            mensaje = mensaje + "<li>Ingrese hora final.</li>";
            flag = false;
        }
    }

    if ($('#txtDescripcion').val() == '') {
        mensaje = mensaje + "<li>Ingrese la descripción del evento.</li>";
        flag = false;
    }

    if ($('#cbSubCausaEvento').val() == '') {
        mensaje = mensaje + "<li>Seleccione la causa del evento.";
        flag = false;
    }

    mensaje = mensaje + "</ul>";

    if (flag) mensaje = "";

    return mensaje;
}

convertir = function () {
    if (confirm('¿Está seguro de realizar esta operación?')) {
        $.ajax({
            type: 'POST',
            url: controler + "cambiarversion",
            dataType: 'json',
            data: {
                idEvento: $('#hfCodigoEvento').val()
            },
            success: function (result) {
                if (result == 1) {
                    document.location.href = controler + 'final?id=' + $('#hfCodigoEvento').val();
                }
                else if (result == 2) {
                    mostrarAlerta("Solo se pueden convertir a finales las bitácoras con tipo EVENTO o FALLA.")
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

mostrarError = function () {
    alert("Ha ocurrido un error");
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