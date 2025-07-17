var controlador = siteRoot + 'Equipamiento/equipo/';
$(function () {
    $('#btnRegresar').click(function () {
        history.back(1);
    });
    BuscarDatosPropiedades();
    $("#popupHistorico").addClass("general-popup");
    $("#popupValorPropiedad").addClass("general-popup");
    $('#btnBuscarPropiedad').click(function () {
        BuscarDatosPropiedades();
    });
});
BuscarDatosPropiedades = function () {
    pintarPaginado();
    mostrarListado(1);
};
pintarPaginado = function () {
    $.ajax({
        type: 'POST',
        url: controlador + "PaginadoEquipoPropiedades",
        data: $('#frmEquipoPropiedad').serialize(),
        success: function (evt) {
            $('#paginado').html(evt);
            mostrarPaginado();
        },
        error: function () {
            mostrarError();
        }
    });
};
mostrarListado = function (nroPagina) {
    $('#hfNroPagina').val(nroPagina);
    $.ajax({
        type: 'POST',
        url: controlador + "ListadoEquipoPropiedades",
        data: $('#frmEquipoPropiedad').serialize(),
        success: function (evt) {
            $('#listado').css("width", $('#mainLayout').width() + "px");

            $('#listado').html(evt);
            $('#tabla').dataTable({
                "scrollY": 430,
                "scrollX": true,
                "sDom": 't',
                "ordering": false,
                "iDisplayLength": 100
            });
        },
        error: function () {
            mostrarError();
        }
    });
};
pintarBusqueda = function (nroPagina) {
    mostrarListado(nroPagina);
};
mostrarHistoricoPropiedad = function (equicodi, propcodi) {
    $.ajax({
        type: 'POST',
        url: controlador + "HistoricoPropiedadEquipo",
        data: {
            iEquipo: equicodi,
            iPropiedad: propcodi
        },
        success: function (evt) {
            $('#historicoPropiedad').html(evt);
            $('#mensaje').css("display", "none");
            setTimeout(function () {
                $('#popupHistorico').bPopup({
                    easing: 'easeOutBack',
                    speed: 450,
                    transition: 'slideDown'
                });
            }, 50);

        },
        error: function () {
            mostrarError();
        }
    });
};
agregarValorPropiedad = function (equicodi, propcodi) {
    $.ajax({
        type: 'POST',
        url: controlador + "AgregarDatoPropiedadEquipo",
        data: {
            iEquipo: equicodi,
            iPropiedad: propcodi
        },
        success: function (evt) {
            $('#valorPropiedad').html(evt);
            $('#mensaje').css("display", "none");
            setTimeout(function () {
                $('#popupValorPropiedad').bPopup({
                    easing: 'easeOutBack',
                    speed: 450,
                    transition: 'slideDown'
                });
            }, 50);

        },
        error: function () {
            mostrarError();
        }
    });
};
function loadInfoFile(fileName, fileSize) {
    $('#fileInfo').html(fileName + " (" + fileSize + ")");
    $('#fileInfo').removeClass();
    $('#fileInfo').addClass('action-exito');
    $('#fileInfo').css('margin', '0px');
    $('#fileInfo').css('padding', '5px');
}

function loadValidacionFile(mensaje) {
    $('#fileInfo').html(mensaje);
    $('#fileInfo').removeClass();
    $('#fileInfo').addClass('action-alert');
    $('#fileInfo').css('margin', '0px');
    $('#fileInfo').css('padding', '5px');
}

function mostrarProgreso(porcentaje) {
    $('#fileInfo').text(porcentaje + "%");
    $('#fileInfo').removeClass();
    $('#fileInfo').addClass('action-exito');
    $('#fileInfo').css('margin', '0px');
    $('#fileInfo').css('padding', '5px');
}

function mostrarErrorFile(mensaje) {
    $('#fileInfo').text(mensaje);
    $('#fileInfo').removeClass();
    $('#fileInfo').addClass('action-error');
    $('#fileInfo').css('margin', '0px');
    $('#fileInfo').css('padding', '5px');
}
mostrarExitoOperacion = function () {
    $('#mensaje').removeClass("action-error");
    $('#mensaje').addClass("action-exito");
    $('#mensaje').text("La operación se realizó con exito...!");
    $('#mensaje').css("display", "block");
};

mostrarError = function () {
    $('#mensaje').removeClass("action-exito");
    $('#mensaje').addClass("action-error");
    $('#mensaje').text("Ha ocurrido un error");
    $('#mensaje').css("display", "block");
};
ocultarMensaje = function () {
    $('#mensaje').css("display", "none");
};
guardarValorPropiedad = function () {
    $.ajax({
        type: 'POST',
        url: controlador + 'AgregarValorPropiedad?iEquipo=' + $('#hdnEquipo').val() + '&iPropiedad=' + $('#hdnPropiedad').val()+'&sValor='+ $('#txtValor').val(),
        dataType: 'json',
        cache: false,
        success: function (resultado) {
            if (resultado == 1) {
                mostrarExitoOperacion();
                $('#popupValorPropiedad').bPopup().close();
                BuscarDatosPropiedades();
            } else
                mostrarError();
        },
        error: function () {
            mostrarError();
        }
    });

};