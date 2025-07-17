var controlador = siteRoot + 'Monitoreo/Parametro/';
var ancho = 1200;
$(function () {
    $('#btnNuevo').click(function () {
        mostrarView();
    });

    $('#txtFecha').Zebra_DatePicker({
        format: 'm Y '
    });

    $('#btnSave').click(function () {

        saveParametro();

    });

    consultarTendencias();
});

function valida(e) {
    tecla = (document.all) ? e.keyCode : e.which;

    //Tecla de retroceso para borrar, siempre la permite
    if (tecla == 8) {
        return true;
    }

    // Patron de entrada, en este caso solo acepta numeros y punto
    patron = /[0-9-.]/;
    tecla_final = String.fromCharCode(tecla);
    return patron.test(tecla_final);
}

function mostrarView() {
    setTimeout(function () {
        $('#idPopupNew').bPopup({
            easing: 'easeOutBack',
            speed: 450,
            transition: 'slideDown',
            modalClose: false
        });
    }, 250);
}

function cerrarPopupNew() {
    $('#idPopupNew').bPopup().close();
}

function validarParametroHHI(aCero, aUno) {
    var msj = '';
    if (aCero == null || aCero.trim() == "")
        msj += "El valor de la Tendencia a Cero no es válido\n";
    if (aUno == null || aUno.trim() == "")
        msj += "El valor de la Tendencia a Uno no es válido";

    if (msj != '') {
        alert(msj);
        return false;
    }
    return true;
}

function saveParametro() {
    var fecha = $('#txtFecha').val();
    var aCero = $('#txtCero').val();
    var aUno = $('#txtUno').val();

    if (validarParametroHHI(aCero, aUno) && confirm('¿Está seguro que desea guardar la configuración de la Tendencia del HHI?')) {
        $.ajax({
            type: 'POST',
            async: true,
            url: controlador + 'SaveParametroHHI',
            data: {
                strfecha: fecha,
                aCero: aCero,
                aUno: aUno
            },
            success: function (result) {
                if (result.Resultado != "-1") {
                    alert("La configuración se registró correctamente ");
                    cerrarPopupNew();
                    $('#txtCero').val('');
                    $('#txtUno').val('');

                    consultarTendencias();

                } else {
                    alert("Ha ocurrido un error: " + result.Mensaje);
                }
            },
            error: function (err) {
                alert("Ha ocurrido un error");
            }
        });
    }
}

function consultarTendencias() {

    $.ajax({
        type: 'POST',
        async: true,
        url: controlador + 'ListarParametroHHI',
        success: function (data) {
            $('#parametroHHI').html(data);

            $('#reporteHHI').dataTable({
                "scrollY": 400,
                "scrollX": true,
                "sDom": 't',
                "ordering": false,
                "bPaginate": false,
                "iDisplayLength": -1
            });
        },
        error: function () {
            alert("Ha ocurrido un error");
        }
    });
}

function editarTendenciaHHI(idCero, idUno) {
    $.ajax({
        type: 'POST',
        url: controlador + "EditarParametroHHI",
        data: {
            idCero: idCero,
            idUno: idUno
        },
        success: function (evt) {
            $('#editarParametroHHI').html(evt);

            $('#mensaje').css("display", "none");
            setTimeout(function () {
                $('#popupEditarParametroHHI').bPopup({
                    easing: 'easeOutBack',
                    speed: 450,
                    transition: 'slideDown'
                });
            }, 50);

        },
        error: function (err) {
            alert('Ha ocurrido un error');
        }
    });
}

function actualizarParametroHHI() {
    var idCero = $("#hfHHITendenciaCeroCodi").val();
    var idUno = $("#hfHHITendenciaUnoCodi").val();
    var aCero = $("#HHITendenciaCeroParametroHHIEdit").val();
    var aUno = $("#HHITendenciaUnoParametroHHIEdit").val();
    var estado = $("#cbEstadoParametroHHI").val();

    if (validarParametroHHI(aCero, aUno) && confirm('¿Está seguro que desea actualizar la configuración de la Tendencia del HHI?')) {
        $.ajax({
            type: 'POST',
            url: controlador + 'ActualizarParametroHHI',
            data: {
                idCero: idCero,
                idUno: idUno,
                aCero: aCero,
                aUno: aUno,
                estado: estado
            },
            success: function (result) {
                if (result.Resultado != "-1") {
                    alert("La configuración se registró correctamente ");
                    $('#popupEditarParametroHHI').bPopup().close();
                    consultarTendencias();
                } else {
                    alert("Ha ocurrido un error: " + result.Mensaje);
                }
            },
            error: function (err) {
                alert('Ha ocurrido un error');
            }
        });
    }
}

function verTendenciaHHI(idCero, idUno) {
    $("#mensaje").hide();

    $.ajax({
        type: 'POST',
        url: controlador + "VerParametroHHI",
        data: {
            idCero: idCero,
            idUno: idUno
        },
        success: function (evt) {
            $('#verParametroHHI').html(evt);
            $('#mensaje').css("display", "none");

            setTimeout(function () {
                $('#popupVerParametroHHI').bPopup({
                    easing: 'easeOutBack',
                    speed: 450,
                    transition: 'slideDown'
                });
            }, 50);
        },
        error: function (err) {
            alert('Ha ocurrido un error');
        }
    });
}