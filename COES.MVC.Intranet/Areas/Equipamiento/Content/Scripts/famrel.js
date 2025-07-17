var controlador = siteRoot + 'Equipamiento/';
$(function () {

    buscarFamRel();

    $('#btnBuscar').click(function () {
        buscarFamRel();
    });
    $('#btnNuevo').click(function () {
        nuevoFamRel();
    });
    
    $("#popupFamrel").addClass("general-popup");
    $("#popUpDetalleFamrel").addClass("general-popup");
    $("#popUpEditarFamrel").addClass("general-popup");

    //$('#btnNuevo').click(function () {
    //    NuevoTipoRel();
    //});
    

});
function buscarFamRel() {
    ocultarMensaje();
    mostrarListado();
}
mostrarError = function () {
    $('#mensaje').removeClass("action-exito");
    $('#mensaje').addClass("action-error");
    $('#mensaje').text("Ha ocurrido un error");
    $('#mensaje').css("display", "block");
};
ocultarMensaje = function () {
    $('#mensaje').css("display", "none");
};
mostrarExitoOperacion = function () {
    $('#mensaje').removeClass("action-error");
    $('#mensaje').addClass("action-exito");
    $('#mensaje').text("Se realizó la operación..!");
    $('#mensaje').css("display", "block");
};
function mostrarListado() {
    
    $.ajax({
        type: 'POST',
        url: controlador + "Relacion/ListaFamRelTipoRel",
        data: {
            idTipoRel: $("#hdnTiporelcodi").val(),
            sEstado: $("#cbEstado").val()
        },
        success: function (evt) {
            $('#listado').css("width", $('#mainLayout').width() + "px");

            $('#listado').html(evt);
            $('#tabla').dataTable({
                "scrollY": 430,
                "scrollX": true,
                "sDom": 't',
                "ordering": false,
                "iDisplayLength": 50
            });
        },
        error: function () {
            alert("Ha ocurrido un error");
        }
    });
};
function mostrarDetalleFamRel(tiporel, fam1, fam2) {
    $.ajax({
        type: 'POST',
        url: controlador + "Relacion/DetalleFamRel",
        data: {
            iTipoRel: $("#hdnTiporelcodi").val(),
            iFamcodi1: fam1,
            iFamcodi2: fam2
        },
        success: function (evt) {
            $('#detallefamrel').html(evt);
            $('#mensaje').css("display", "none");
            setTimeout(function () {
                $('#popUpDetalleFamrel').bPopup({
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
function editarFamRel(tiporel, fam1, fam2) {
    $.ajax({
        type: 'POST',
        url: controlador + "Relacion/EditarFamRel",
        data: {
            iTipoRel: $("#hdnTiporelcodi").val(),
            iFamcodi1: fam1,
            iFamcodi2: fam2
        },
        success: function (evt) {
            $('#editarfamrel').html(evt);
            $('#mensaje').css("display", "none");
            setTimeout(function () {
                $('#popUpEditarFamrel').bPopup({
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
function nuevoFamRel() {
    $.ajax({
        type: 'POST',
        url: controlador + "Relacion/NuevoFamRel",
        data: {
            iTipoRel: $("#hdnTiporelcodi").val()
        },
        success: function (evt) {
            $('#nuevofamrel').html(evt);
            $('#mensaje').css("display", "none");
            setTimeout(function () {
                $('#popupFamrel').bPopup({
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
guardarFamRel = function () {
    if (confirm('¿Está seguro de agregar la nueva relación?')) {
        $.ajax({
            type: 'POST',
            url: controlador + 'Relacion/GuardarNuevoFamRel',
            dataType: 'json',
            data: {
                iTipoRel: $("#hdnTiporelcodi").val(),
                iFamcodi1: $("#cbFamilia1").val(),
                iFamcodi2: $("#cbFamilia2").val(),
                iNumCon: $("#Famnumconec").val(),
                sTension: $("#cbFamreltension").val()
            },
            cache: false,
            success: function (resultado) {
                if (resultado === 1) {
                    mostrarExitoOperacion();
                    $('#popupFamrel').bPopup().close();
                    mostrarListado();
                } else
                    mostrarError();
            },
            error: function () {
                mostrarError();
            }
        });
    }
};
actualizarFamRel = function () {
    if (confirm('¿Está seguro de editar la relación?')) {
        $.ajax({
            type: 'POST',
            url: controlador + 'Relacion/ActualizarFamRel',
            dataType: 'json',
            data: {
                iTipoRel: $("#hdnTiporelcodi").val(),
                iFamcodi1old: fc1,
                iFamcodi2old: fc2,
                iFamcodi1: $("#cbFamilia1").val(),
                iFamcodi2: $("#cbFamilia2").val(),
                iNumCon: $("#Famnumconec").val(),
                sTension: $("#cbFamreltension").val(),
                sEstado: $("#cbEstadoEdit").val()
            },
            cache: false,
            success: function (resultado) {
                if (resultado === 1) {
                    mostrarExitoOperacion();
                    $('#popUpEditarFamrel').bPopup().close();
                    mostrarListado();
                } else
                    mostrarError();
            },
            error: function () {
                mostrarError();
            }
        });
    }
};