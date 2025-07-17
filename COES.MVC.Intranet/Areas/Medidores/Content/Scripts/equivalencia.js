var controlador = siteRoot + 'medidores/equivalencia/';

$(function () {
    $('#btnConsultar').click(function () {
        listar();
    });
    
    $('#btnNuevo').click(function () {
        cargarModal();
    });

    $('#btnCargarPuntos').click(function () {
        cargarPuntos();
    });

    $('#btnGrabarRelacion').click(function () {
        grabarRelacion();
    });

    $('#btnCancelar').click(function () {
        document.location.href = siteRoot + 'medidores/validacionregistro/index';
    });

    listar();
});

listar = function ()
{
    $.ajax({
        type: 'POST',
        data: {
            idEmpresa: $('#cbEmpresa').val()
        },
        url: controlador + 'relaciones',
        global: false,
        success: function (evt) {
            
            $('#listado').css("width", $('#mainLayout').width() + "px");
            $('#listado').html(evt);
            $('#tablaRelaciones').DataTable({
                "scrollY": 430,
                "scrollX": false,
                "sDom": 't',
                "ordering": false,
                "iDisplayLength": -1,
                "bSortClasses": false
            });
           
        },
        error: function () {
            mostrarError();
        }
    });
}

cargarModal = function ()
{    
    setTimeout(function () {
        $('#popupEdicion').bPopup({
            autoClose: false
        });

    }, 50);
}

quitarRelacion = function (id) {
    alert('¿Está seguro de eliminar este registro?')
    {
        $.ajax({
            type: 'POST',
            data: {
                id: id
            },
            url: controlador + 'eliminar',
            datatype: 'json',            
            success: function (result) {                
                if (result == 1) {
                    listar();
                    mostrarExito();
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

cargarPuntos = function () {

    if ($('#cbEmpresaEdit').val() != "") {

        $.ajax({
            type: 'POST',
            data: { 
                idEmpresa: $('#cbEmpresaEdit').val()
            },
            url: controlador + 'listado',
            global:false,
            success: function (evt) {
                $('#contenidoEdicion').html(evt);
                $('#tbMedidores').DataTable({
                    "scrollY": 300,
                    "scrollX": true,
                    "sDom": 't',
                    "ordering": false,
                    "iDisplayLength": 50,
                    "bSortClasses": false
                });
                $('#tbDespacho').DataTable({
                    "scrollY": 300,
                    "scrollX": true,
                    "sDom": 't',
                    "ordering": false,
                    "iDisplayLength": 50,
                    "bSortClasses": false
                });
            },
            error: function () {
                mostrarErrorPopup();
            }
        });
    }
    else
    {
        mostrarAlertaPopup("Seleccione empresa");
    }
}

grabarRelacion = function () {

    var idMedicion = $("input[name=rbMedidor]:checked").val();
    var idDespacho = $("input[name=rbDespacho]:checked").val();

    if (confirm('¿Está seguro de realizar esta operación?')) {

        $.ajax({
            type: 'POST',
            url: controlador + 'grabar',
            data: {
                idMedicion: idMedicion,
                idDespacho: idDespacho
            },
            datatype: 'json',
            success: function (result) {
                if (result == 1) {
                    listar();
                    $('#popupEdicion').bPopup().close();
                    mostrarExito();
                }
                else if (result == 2) {
                    mostrarAlertaPopup("La relación entre ambos puntos ya existe.");
                }
                else {
                    mostrarErrorPopup();
                }
            },
            error: function () {
                mostrarErrorPopup();
            }
        });
    }
}

seleccionarFila = function (evt)
{
    if ($(evt).attr('name') == 'rbMedidor') {

        $('#tbMedidores tbody tr').each(function () {
            $(this).removeClass('filaseleccionada');
        });
    }

    if ($(evt).attr('name') == 'rbDespacho') {

        $('#tbDespacho tbody tr').each(function () {
            $(this).removeClass('filaseleccionada');
        });
    }    

    if ($(evt).is(':checked')) {
        $(evt).closest('tr').addClass('filaseleccionada');
    }
}

mostrarAlertaPopup = function (mensaje)
{
    $('#mensajePopup').removeClass();
    $('#mensajePopup').addClass('action-alert');
    $('#mensajePopup').html(mensaje);
}

mostrarErrorPopup = function () {
    $('#mensajePopup').removeClass();
    $('#mensajePopup').addClass('action-error');
    $('#mensajePopup').html("Ha ocurrido un error");
}

mostrarExito = function () {
    $('#mensaje').removeClass();
    $('#mensaje').addClass('action-exito');
    $('#mensaje').html("La operación se realizó correctamente");
}

mostrarError = function () {
    $('#mensaje').removeClass();
    $('#mensaje').addClass('action-error');
    $('#mensaje').html("Ha ocurrido un error");
}