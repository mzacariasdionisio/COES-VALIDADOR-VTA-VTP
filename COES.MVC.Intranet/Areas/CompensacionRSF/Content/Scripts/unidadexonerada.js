var sControlador = siteRoot + "compensacionrsf/unidadexonerada/";


$(document).ready(function () {
    buscar();

    $('#btnBuscar').click(function () {
        buscar();
    });

    $('#btnActualizar').click(function () {
        Actualizar();
    });

    $('#btnGrabar').click(function () {
        grabarLista();
    });
});

buscar = function () {
    var pericodi = document.getElementById('pericodi').value;
    var vcrecacodi = document.getElementById('vcrecacodi').value;
    $.ajax({
        type: 'POST',
        url: sControlador + "lista",
        data: { pericodi: pericodi, vcrecacodi: vcrecacodi },
        success: function (evt) {
            $('#listado').html(evt);
            viewEvent();
            //oTable = $('#tabla').dataTable({
            //    "sPaginationType": "full_numbers",
            //    "destroy": "true",
            //    "aaSorting": [[0, "asc"], [1, "asc"], [2, "asc"]]
            //});
        },
        error: function () {
            mostrarError();
        }
    });
};

//Funciones de vista detalle
viewEvent = function () {
    $('.exonerar').click(function () {
        vcruexcodi = $(this).attr("id").split("_")[1];
        abrirPopup(vcruexcodi);
    });
};

abrirPopup = function (vcruexcodi) {
    $.ajax({
        type: 'POST',
        url: sControlador + "Exonerar",
        data: { vcruexcodi: vcruexcodi },
        success: function (evt) {
            $('#popup').html(evt);
            setTimeout(function () {
                $('#popup').bPopup({
                    easing: 'easeOutBack',
                    speed: 450,
                    transition: 'slideDown'
                });
            }, 50);
        },
        error: function () {
            mostrarError("Lo sentimos, ha ocurrido un error inesperado");
        }
    });
}

//Funcion actualizar con popup

Actualizar = function () {
    var estado;
    var id = document.getElementById('EntidadUnidadExonerada_Vcruexcodi').value;
    //estado = document.getElementById('EntidadUnidadExonerada_Vcruexonerar').value;

    if (document.getElementById('EntidadUnidadExonerada_Vcruexonerar1').checked) {
        estado = document.getElementById('EntidadUnidadExonerada_Vcruexonerar1').value;
    }
    else if (document.getElementById('EntidadUnidadExonerada_Vcruexonerar2').checked) {
        estado = document.getElementById('EntidadUnidadExonerada_Vcruexonerar2').value;
    }

    var comentario = document.getElementById('EntidadUnidadExonerada_Vcruexobservacion').value;
    $.ajax({
        type: 'POST',
        url: sControlador + "actualizarcampos",
        data: { id: id, estado: estado, comentario: comentario },
        dataType: 'json',
        success: function (model) {
            if (model.sError == "") {
                mostrarExito("Felicidades, se grabo la exoneración de la unidad");
            }
            else {
                refrescar();
            }
        },
        error: function () {
            mostrarMensaje('Ha ocurrido un error');
        }
    });
    return;
}

grabarLista = function () {
    var iNumReg = document.getElementById('Count').value;
    if (iNumReg > 0) {
        var items = checkMark();
        var pericodi = document.getElementById('pericodi').value;
        var vcrecacodi = document.getElementById('vcrecacodi').value;
        $.ajax({
            type: 'POST',
            url: sControlador + 'grabarlistaexonerar',
            data: { pericodi: pericodi, vcrecacodi: vcrecacodi, items: items },
            dataType: 'json',
            success: function (model) {
                if (model.sError == "") {
                    mostrarExito("Felicidades, tenemos " + model.iNumReg + " unidades exoneradas");
                    refrescar();
                }
                else {
                    mostrarError("Lo sentimos, ha ocurrido el siguiente error: " + model.sError);
                }
            },
            error: function () {
                mostrarMensaje('Ha ocurrido un error');
            }
        });

    }
    else { return; }
}

nuevo = function () {
    //var Stpercodi = document.getElementById('Entidad_Stpercodi').value;
    window.location.href = sControlador + "new";
}

refrescar = function () {
    var cmbPericodi = document.getElementById('pericodi');
    var cmbRecacodi = 0;
    window.location.href = sControlador + "index?pericodi=" + cmbPericodi.value + "&vcrecacodi=" + cmbRecacodi.value;
}

refrescarrecalculo = function () {
    var cmbPericodi = document.getElementById('pericodi');
    var cmbRecacodi = document.getElementById('vcrecacodi');
    window.location.href = sControlador + "index?pericodi=" + cmbPericodi.value + "&vcrecacodi=" + cmbRecacodi.value;
}
