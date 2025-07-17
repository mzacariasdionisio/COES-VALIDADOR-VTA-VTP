var controlador = siteRoot + 'Equipamiento/ReportePotencia/';
$(function () {
    $('#btnConsultar').click(function () {
        listar();
    });

    $('#btnNuevo').click(function () {
        cargarModal();
    });

    $('#btnCargarModos').click(function () {
        cargarModos();
    });

    $('#btnGrabarModos').click(function () {
        grabarModos();
    });

    $('#btnCancelar').click(function () {
        document.location.href = siteRoot + 'Equipamiento/ReportePotencia/IndexConfiguracion';
    });

    listar();
});
mostrarAlertaPopup = function(mensaje) {
    $('#mensajePopup').removeClass();
    $('#mensajePopup').addClass('action-alert');
    $('#mensajePopup').html(mensaje);
};

mostrarErrorPopup = function() {
    $('#mensajePopup').removeClass();
    $('#mensajePopup').addClass('action-error');
    $('#mensajePopup').html("Ha ocurrido un error");
};

mostrarExito = function() {
    $('#mensaje').removeClass();
    $('#mensaje').addClass('action-exito');
    $('#mensaje').html("La operación se realizó correctamente");
};

mostrarError = function() {
    $('#mensaje').removeClass();
    $('#mensaje').addClass('action-error');
    $('#mensaje').html("Ha ocurrido un error");
};
listar = function() {
    $.ajax({
        type: 'POST',
        data: {
            iEmpresa: $('#cbEmpresa').val()
        },
        url: controlador + 'ListConfiguracion',
        global: false,
        success: function(evt) {

            $('#listado').css("width", $('#mainLayout').width() + "px");
            $('#listado').html(evt);
            $('#tablaRelaciones').DataTable({
                "scrollY": 430,
                "scrollX": false,
                "sDom": 't',
                "ordering": false,
                "iDisplayLength": 50,
                "bSortClasses": false
            });

        },
        error: function() {
            mostrarError();
        }
    });
};
cargarModal = function() {
    setTimeout(function() {
        $('#popupEdicion').bPopup({
            autoClose: false
        });

    }, 50);
};
cargarModos = function() {

    //if ($('#cbEmpresaEdit').val() !== "-2") {

        $.ajax({
            type: 'POST',
            data: {
                iEmpresa: $('#cbEmpresaEdit').val()
            },
            url: controlador + 'ModosConfiguracion',
            global: false,
            success: function(evt) {
                $('#contenidoEdicion').html(evt);
                $('#tbModos').DataTable({
                    "scrollY": 300,
                    "scrollX": true,
                    "sDom": 't',
                    "ordering": false,
                    "iDisplayLength": 50,
                    "bSortClasses": false
                });
            },
            error: function() {
                mostrarErrorPopup();
            }
        });
    //} else {
    //    mostrarAlertaPopup("Seleccione empresa");
    //}
};
seleccionarFila = function(evt) {
    if ($(evt).attr('name') === 'cbModo') {

        $('#tbModos tbody tr').each(function () {
            $(this).removeClass('filaseleccionada');
        });
    }
    if ($(evt).is(':checked')) {
        $(evt).closest('tr').addClass('filaseleccionada');
    }
};
grabarModos = function() {

    var lsCodigos = $("input[name=cbModo]:checked").serializeArray();
    console.log(lsCodigos);

    var stringArray = new Array();
    $.each(lsCodigos, function(key, value) {
        stringArray.push(value.value);
    });
    console.log(stringArray);

    if (confirm('¿Está seguro de realizar esta operación?')) {

        $.ajax({
            type: 'POST',
            url: controlador + 'Guardar',
            data: JSON.stringify({ modosConfigurados: stringArray }),
            datatype: 'json',
            contentType: 'application/json; charset=utf-8',
            success: function(result) {
                if (result == 1) {
                    listar();
                    $('#popupEdicion').bPopup().close();
                    mostrarExito();
                } else if (result == 2) {
                    mostrarAlertaPopup("La relación entre ambos puntos ya existe.");
                } else {
                    mostrarErrorPopup();
                }
            },
            error: function() {
                mostrarErrorPopup();
            }
        });
    }
};
quitarConfigurado = function(id) {
    alert('¿Está seguro de eliminar este registro?')
    {
        $.ajax({
            type: 'POST',
            data: {
                iGrupocodi: id
            },
            url: controlador + 'Eliminar',
            datatype: 'json',
            success: function(result) {
                if (result === 1) {
                    listar();
                    mostrarExito();
                } else {
                    mostrarError();
                }
            },
            error: function() {
                mostrarError();
            }
        });
    }
};