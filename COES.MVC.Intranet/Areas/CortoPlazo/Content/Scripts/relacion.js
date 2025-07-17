var controlador = siteRoot + 'cortoplazo/configuracion/';

$(function () {

    $('#btnConsultar').on('click', function () {
        consultar();
    });

    $('#btnNuevo').on('click', function () {
        editarRelacion(0);
    });

    $('#btnListado').on('click', function () {
        verListado();
    });

    consultar();

});

consultar = function () {

    $.ajax({
        type: 'POST',
        url: controlador + 'relacionlist',
        data: {
            idEmpresa: $('#cbEmpresa').val(),
            estado: $('#cbEstado').val()
        },
        success: function (evt) {
            $('#listado').html(evt);
            $('#tablaRelacion').dataTable({
                "iDisplayLength": 25
            });
        },
        error: function () {
            mostrarMensaje('mensaje', 'error', 'Ha ocurrido un error.');
        }
    });
}

verListado = function () {

    document.location.href = controlador + "RelacionListGlobal";
}

cargarEquipos = function () {

    $('option', '#cbEquipoEdit').remove();

    $.ajax({
        type: 'POST',
        url: controlador + 'cargarequipos',
        dataType: 'json',
        data: {
            emprcodi: $('#cbEmpresaEdit').val()
        },
        cache: false,
        global: false,
        success: function (aData) {
            $('#cbEquipoEdit').get(0).options.length = 0;
            $('#cbEquipoEdit').get(0).options[0] = new Option("--SELECCIONE--", "-1");
            $.each(aData, function (i, item) {
                $('#cbEquipoEdit').get(0).options[$('#cbEquipoEdit').get(0).options.length] = new Option(item.Text, item.Value);
            });
        },
        error: function () {
            mostrarMensaje('mensajeRelacion', 'error', 'Se ha producido un error.');
        }
    });
}

editarRelacion = function (id) {

    $.ajax({
        type: 'POST',
        url: controlador + 'relacionedit',
        data: {
            idRelacion: id
        },
        global: false,
        success: function (evt) {
            $('#contenidoRelacion').html(evt);
            setTimeout(function () {
                $('#popupRelacion').bPopup({
                    autoClose: false
                });
            }, 50);

            $('#cbEmpresaEdit').val($('#hfEmpresa').val());
            $('#cbEquipoEdit').val($('#hfEquipo').val());
            $('#cbEstadoEdit').val($('#hfEstado').val());
            //- Linea agregada movisoft 25.02.2021
            $('#cbIndRer').val($('#hfIndRer').val());

            $('#btnGrabarRelacion').on("click", function () {
                grabarRelacion();
            });

            $('#btnCancelarRelacion').on("click", function () {
                $('#popupRelacion').bPopup().close();
            });

            $('#cbEmpresaEdit').on("change", function () {
                cargarEquipos();
            });

            if (id != 0) {
                $('#cbEmpresaEdit').css('pointer-events', 'none');
                $('#cbEquipoEdit').css('pointer-events', 'none');
            }

            //- Cambios para ticket 2022-004245
            if ($('#hfIndNoModelada').val() == "S") {
                $('#cbIndModelada').prop('checked', true);
            }

            $('#trEquiposTNA').hide();
            if ($('#hfMasEquiposTNA').val() == "S") {
                $('#cbMasEquiposTNA').prop('checked', true);
                $('#trEquiposTNA').show();
            }            

            $('#cbMasEquiposTNA').click(function () {
                if ($('#cbMasEquiposTNA').is(':checked')) {
                    $('#trEquiposTNA').show();
                }
                else {
                    $('#trEquiposTNA').hide();
                }
            });

            $('#btnAgregarEquipo').on('click', function () {
                var nuevoEquipo = $('#txtEquipoTNA').val();
                if (nuevoEquipo != "") {

                    if ($('#txtNombreTna').val() != $('#txtEquipoTNA').val()) {
                        agregarEquipoAdicional(nuevoEquipo);
                        $('#txtEquipoTNA').val("");
                    }
                    else {
                        mostrarMensaje('mensajeRelacion', 'alert', 'El nombre de equipo TNA adicional debe ser distinto al principal.');
                    }
                }
                else {
                    mostrarMensaje('mensajeRelacion', 'alert', 'Ingrese el nombre del equipo TNA');
                }
            });
        },
        error: function () {
            mostrarMensaje('mensaje', 'error', 'Se ha producido un error.');
        }
    });
}

agregarEquipoAdicional = function (result) {
    //agregamos registros al final de la tabla
    var count = 0;
    var flag = true;
    $('#tablaAdicional>tbody tr').each(function (i) {
        $punto = $(this).find('#txtItemEquipo');
        if ($punto.val() == result) {
            flag = false;
        }
    });

    if (flag) {
        $('#tablaAdicional> tbody').append(
            '<tr>' +
            '   <td><input type="text" id="txtItemEquipo" value="' + result + '"/></td>' +          
            '   <td style="text-align:center">' +
            '       <input type="hidden" id="hfEquipoItem" value="' + 0 + '" /> ' +
            '       <img src="' + siteRoot + 'Content/Images/btn-cancel.png" alt="" onclick="$(this).parent().parent().remove();" style="cursor:pointer" />' +
            '   </td>' +
            '</tr>'
        );
    }
    else {
        mostrarMensaje('mensajeRelacion', 'alert', 'El equipo ya ha sido agregado.');
    }
}

eliminarRelacion = function (id) {
    if (confirm('¿Está seguro de realizar esta operación?')) {

        $.ajax({
            type: 'POST',
            url: controlador + 'relaciondelete',
            data: {
                idRelacion: id
            },
            dataType: 'json',
            success: function (result) {
                if (result == 1) {
                    mostrarMensaje('mensaje', 'exito', 'El registro se eliminó correctamente.');
                    consultar();
                }
                else {
                    mostrarMensaje('mensaje', 'error', 'Se ha producido un error.');
                }
            },
            error: function () {
                mostrarMensaje('mensaje', 'error', 'Se ha producido un error.');
            }
        });
    }
}

grabarRelacion = function () {
    var validacion = validarRegistro();

    if (validacion == "") {
        $.ajax({
            type: 'POST',
            url: controlador + 'relacionsave',
            data: $('#frmRegistro').serialize(),
            dataType: 'json',
            global: false,
            success: function (result) {
                if (result == 1) {
                    mostrarMensaje('mensaje', 'exito', 'Los datos se grabaron correctamente.');
                    $('#popupRelacion').bPopup().close();
                    consultar();

                }
                else if (result == 2) {
                    mostrarMensaje('mensajeRelacion', 'alert', 'La equivalencia del equipo ya se encuentra registrada.');
                }
                else {
                    mostrarMensaje('mensajeRelacion', 'error', 'Se ha producido un error.');
                }
            },
            error: function () {
                mostrarMensaje('mensajeRelacion', 'error', 'Se ha producido un error.');
            }
        });
    }
    else {
        mostrarMensaje('mensajeRelacion', 'alert', validacion);
    }
}

validarRegistro = function () {
    var mensaje = "<ul>";
    var flag = true;

    if ($('#cbEquipoEdit').val() == "") {
        mensaje = mensaje + "<li>Seleccione el equipo SGOCOES.</li>";
        flag = false;
    }


    // Validamos si los campos están completados correctamente

    if ($('#txtNomBarra').val() == "" && $('#txtIdGenerador').val() == "") {
        mensaje = mensaje + "<li>Ingrese las equivalencia con PSSE y/o NCP</li>";
        flag = false;
    }
    else {
        if (($('#txtNomBarra').val() == "" && $('#txtIdGenerador').val() != "") || ($('#txtNomBarra').val() != "" && $('#txtIdGenerador').val() == "")) {
            mensaje = mensaje + "<li>Debe completar Nombre Barra e ID Generador</li>";
            flag = false;
        }
    }

    if ($('#txtNombreTna').val() == "") {
        mensaje = mensaje + "<li>Ingrese el identificador del TNA</li>";
        flag = false;
    }


    if ($('#cbEstadoEdit').val() == "") {
        mensaje = mensaje + "<li>Seleccione el estado.</li>";
        flag = false;
    }

    var count = 0;
    var items = "";
    $('#tablaAdicional>tbody tr').each(function (i) {
        $punto = $(this).find('#txtItemEquipo');
        var constante = (count > 0) ? "@" : "";
        items = items + constante + $punto.val();
        count++;
    });

    $('#hfListaEquipoAdicional').val(items);
    $('#hfMasEquiposTNA').val("N");
    if ($('#cbMasEquiposTNA').is(':checked')) {
        if (count == 0) {
            mensaje = mensaje + "<li>Agregue al menos un equipo adicional.</li>";
            flag = false;
        }
        $('#hfMasEquiposTNA').val("S");
    }

    mensaje = mensaje + "</ul>";

    if (flag) {
        mensaje = "";
    }

    //- Cambios para ticket 2022-004245
    $('#hfIndNoModelada').val('N')
    if ($('#cbIndModelada').is(':checked')) $('#hfIndNoModelada').val('S');


    return mensaje;
}

mostrarMensaje = function (id, tipo, mensaje) {
    $('#' + id).removeClass();
    $('#' + id).addClass('action-' + tipo);
    $('#' + id).html(mensaje);
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

//-- Cambios para ticket 2022-004245

verConfiguracionTNA = function (id) {

    $.ajax({
        type: 'POST',
        url: controlador + 'relacionconfiguracion',
        data: {
            relacionCodi: id
        },
        global: false,
        success: function (evt) {
            $('#contenidoConfiguracion').html(evt);
            setTimeout(function () {
                $('#popupConfiguracion').bPopup({
                    autoClose: false
                });
            }, 50);

            $('#btnGrabarConfiguracion').on('click', function (){
                grabarConfiguracion();
            });

            $('#btnCancelarConfiguracion').on('click', function () {
                $('#popupConfiguracion').bPopup().close();
            });

            $('#btnAgregar').on('click', function () {                
                agregarBarraEMS($('#cbBarraEMS').val(), $("#cbBarraEMS option:selected").text());
            });            
           
        },
        error: function () {
            mostrarMensaje('mensaje', 'error', 'Se ha producido un error.');
        }
    });
};

agregarBarraEMS = function (barraCodi, barraNomb) {
    var count = 0;
    var flag = true;
    $('#tbBarras>tbody tr').each(function (i) {
        $punto = $(this).find('#hfBarraEMS');
        if ($punto.val() == barraCodi) {
            flag = false;
        }
    });

    if (flag) {

        $('#tbBarras> tbody').append(
            '<tr>' +
            '   <td style="text-align:center">' +
            '       <input type="hidden" id="hfBarraEMS" value="' + barraCodi + '" /> ' +
            '       <a><img src="' + siteRoot + 'Content/Images/btn-cancel.png" alt="" onclick="$(this).parent().parent().parent().remove();" style="cursor:pointer" /></a>' +
            '   </td>' +
            '   <td>' + barraNomb + '</td>' +
            '</tr>'
        );       
    }
    else {
        mostrarMensaje('mensajeEditBarraEMS', 'alert', 'La barra ya se encuentra agregada');
    }
};

grabarConfiguracion = function () {
    var validacion = validarConfiguracion();

    if (validacion == "") {

        var count = 0;
        var items = "";
        $('#tbBarras>tbody tr').each(function (i) {
            $punto = $(this).find('#hfBarraEMS');
            var constante = (count > 0) ? "," : "";
            items = items + constante + $punto.val();
            count++;
        });

        $('#hfListaBarras').val(items);

        var countPotencia = 0;
        var itemsPotencia = "";
        $('#tbPotencias>tbody tr').each(function (i) {
            $punto = $(this).find('#hfGrupoCodi');
            $txt = $(this).find('#txtPotencia');
            var constante = (countPotencia > 0) ? "," : "";
            itemsPotencia = itemsPotencia + constante + $punto.val() + '#' + $txt.val();
            countPotencia++;
        });

        $('#hfListaPotencia').val(itemsPotencia);
       
        $.ajax({
            type: 'POST',
            url: controlador + 'grabarconfiguracionunidad',
            data: $('#frmConfiguracion').serialize(),
            dataType: 'json',
            success: function (result) {
                if (result == 1) {
                    mostrarMensaje('mensaje', 'exito', 'La operación se realizó correctamente');
                    $('#popupConfiguracion').bPopup().close();
                }
                else {
                    mostrarMensaje('mensajeEditBarraEMS', 'error', 'Ha ocurrido un error.');
                }
            },
            error: function () {
                mostrarMensaje('mensajeEditBarraEMS', 'error', 'Ha ocurrido un error.');
            }
        });
       
    }
    else {
        mostrarMensaje('mensajeEditBarraEMS', 'alert', validacion);
    }
};

validarConfiguracion = function () {

    var html = "<ul>";
    var flag = true;

    var count = $('#tbBarras >tbody >tr').length;

    if (count == 0) {
        html = html + "<li>Seleccione al menos una barra EMS.</li>";
        flag = false;
    }


    var flagVacio = true;
    var flagFormato = true;
    $('#tbPotencias>tbody tr').each(function (i) {
        $punto = $(this).find('#txtPotencia');
        if ($punto.val() == "") {
            flagVacio = false;
        }
        else if (!validarDecimal($punto.val())) {
            flagFormato = false;
        }
    });

    if (!flagVacio) {
        html = html + "<li>Debe ingresar las potencias de todos los modos de operación.</li>";
        flag = false;
    }
    if (!flagFormato) {
        html = html + "<li>Las potencias deben tener formato numérico.</li>";
        flag = false;
    }

    html = html + "</ul>";


    if (flag) {
        html = "";
    }

    return html;
};

function validarDecimal(texto) {
    return /^-?[\d.]+(?:e-?\d+)?$/.test(texto);
};