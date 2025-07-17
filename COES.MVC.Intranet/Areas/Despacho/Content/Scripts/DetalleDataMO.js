var controlador = siteRoot + 'Despacho/DatosModoOperacion/'
$(function () {
    buscarEvento();
    $('#btnBuscar').click(function () {
        buscarEvento();
    });
    $('#btnCancelar').click(function () {
        //document.location.href = controlador + "index";
        history.back(1);
    });
    $(window).resize(function () {
        $('#listado').css("width", $('#mainLayout').width() + "px");
    });
    $("#popupParametro").addClass("general-popup");
    $("#popupHistorico").addClass("general-popup");
    $("#popupNuevo").addClass("general-popup");
    
    $('#btnNuevo').click(function () {
        nuevoParametro();
    });
});
function buscarEvento() {
    ocultarMensaje();
    mostrarListado();
}
function mostrarListado() {
    
    $.ajax({
        type: 'POST',
        url: controlador + "ListaDatosModoOperacion",
        data: $('#frmBusqueda').serialize(),
        success: function (evt) {
            $('#listado').css("width", $('#mainLayout').width() + "px");

            $('#listado').html(evt);
            $('#tabla').dataTable({
                "scrollY": 430,
                "scrollX": true,
                "sDom": 't',
                "ordering": false,
                "iDisplayLength": 1000
            });
        },
        error: function () {
            alert("Ha ocurrido un error");
        }
    });
}

editarParametro = function(idGrupo, idConcepto) {
    $.ajax({
        type: 'POST',
        url: controlador + "EdicionParametro",
        data: {
            idGrupo: idGrupo,
            idConcepto: idConcepto,
            sModo: 'EDITAR'
        },
        success: function(evt) {
            $('#edicionParametro').html(evt);
            $('#mensaje').css("display", "none");
            setTimeout(function() {
                $('#popupParametro').bPopup({
                    easing: 'easeOutBack',
                    speed: 450,
                    transition: 'slideDown'
                });
            }, 50);

        },
        error: function() {
            mostrarError();
        }
    });
};
nuevoParametro = function() {
    $.ajax({
        type: 'POST',
        url: controlador + "EdicionParametro",
        data: {
            idGrupo: $("#iGrupoCodi").val(),
            idConcepto: -1,
            sModo: 'NUEVO'
        },
        success: function(evt) {
            $('#nuevoParametro').html(evt);
            $('#mensaje').css("display", "none");
            setTimeout(function() {
                $('#popupNuevo').bPopup({
                    easing: 'easeOutBack',
                    speed: 450,
                    transition: 'slideDown'
                });
            }, 50);

        },
        error: function() {
            mostrarError();
        }
    });
};
mostrarHistorial=function(idGrupo, idConcepto) {
    $.ajax({
        type: 'POST',
        url: controlador + "HistorialParametro",
        data: {
            idGrupo: idGrupo,
            idConcepto: idConcepto
        },
        success: function(evt) {
            $('#historicoParametro').html(evt);
            $('#mensaje').css("display", "none");
            setTimeout(function() {
                $('#popupHistorico').bPopup({
                    easing: 'easeOutBack',
                    speed: 450,
                    transition: 'slideDown'
                });
            }, 50);

        },
        error: function() {
            mostrarError();
        }
    });
};
actualizar = function() {
    if (confirm('¿Está seguro de agregar el valor al parámetro?')) {
        $.ajax({
            type: 'POST',
            url: controlador + 'ActualizarValorParametro',
            dataType: 'json',
            data: {
                sModo: $('#hfModoEdicion').val(),
                iGrupoCodi: $('#hfGrupoCodi').val(),
                iConcepCodi: $('#hfConcepCodi').val(),
                sValor: $('#Formula').val(),
                sFechaDat: $("#FechaDat").val()
            },
            cache: false,
            success: function(resultado) {
                if (resultado == 1) {
                    mostrarExitoOperacion();
                    var modo = $('#hfModoEdicion').val();
                    if (modo == 'EDITAR') {
                        $('#popupParametro').bPopup().close();
                    } else {
                        $('#popupNuevo').bPopup().close();
                    }
                    
                    mostrarListado();
                } else
                    mostrarError();
            },
            error: function() {
                mostrarError();
            }
        });
    }
};
mostrarExitoOperacion = function() {
    $('#mensaje').removeClass("action-error");
    $('#mensaje').addClass("action-exito");
    $('#mensaje').text("El modo de operación se actualizó...!");
    $('#mensaje').css("display", "block");
};

mostrarError = function() {
    $('#mensaje').removeClass("action-exito");
    $('#mensaje').addClass("action-error");
    $('#mensaje').text("Ha ocurrido un error");
    $('#mensaje').css("display", "block");
};
ocultarMensaje= function() {
    $('#mensaje').css("display", "none");
}