var controlador = siteRoot + 'Ensayo/';

$(function () {
    $('#tablaformatos').contextMenu({
        selector: '.context-menu-one',
        callback: function (key, options) {
            var strformato = $(this).attr('id');
            var nombreFormato = $(this).parent('tr').find("td:first").html();
            var idFormato = strformato.substring(3, strformato.length);


            if (key == "observado") {
                observarFormato(idFormato, nombreFormato);

            }
            if (key == "aprobado") {
                aprobarFormato(idFormato);      // MODIFICAR ESTO A DOS PARAMETROS : SOS2610
            }
        },
        items: {
            "observado": { name: "OBSERVAR" },
            "aprobado": { name: "APROBAR" },
        }
    });

    $('.context-menu-one').on('click', function (e) {
        console.log('clicked', this);

    })

    $('#btnAceptarObservarFormat').click(function () {
        var elemento = document.getElementById("txtObserv");
        var num = elemento.value.length;
        if (num <= 200) {
            guardarObservacionFormato();
        } else {
            alert("Error : La observación solo debe tener 200 caracteres");
        }
    });

    $('#btnAceptarAprob').click(function () {
        guardarAprobacionFormato();
    });
    $('#btnCancelar').click(function () {
        cancelar();
    });
    $('#btnCancelarAprob').click(function () {
        cancelarAprob();
    });
    $('#btnCancelarObsFormat').click(function () {
        cancelarObs();
    });
})

cancelar = function () {
    document.location.href = controlador + "genera/index/";
}

cancelarAprob = function () {
    $('#popupAprobFormato').bPopup().close();
}

cancelarObs = function () {
    $('#txtObserv').val("");
    $('#popupObservarFortmato').bPopup().close();
}

salir = function () {
    $('#popupEnsayoHistorialFormato').bPopup().close();
}

refresh = function (id, ensayoaprob) {
    cancelarObs();
    alert("Los cambios se guardaron exitosamente");
    if (ensayoaprob == 1) // ensayo aprobado
    {
        alert("¡El ensayo ha sido Aprobado!")
    }
    location.href = controlador + "genera/EditarFormato?id=" + id;
}

function guardarAprobacionFormato() { //funcion que guarda el estado de la aprobacion de un formato para un ensayo
    var formatocodi = $('#hFormatoCodi').val();
    var ensayocodi = $('#hEnsayoCodi').val();
    var enunidadcodi = $('#hEnUnidadCodi').val();
    $.ajax({
        type: 'POST',
        url: controlador + "genera/GuardarAprobacionFormato",
        data: {
            iEnUnidadCodi: enunidadcodi, iFormatoCodi: formatocodi
        },
        success: function (evt) {
            refresh(ensayocodi, evt.Ensayoaprob);  //Actualice en el mismo ensayo
        },
        error: function () {
            alert("Ha ocurrido un error Guardar Observación");
        }
    });
}

function guardarObservacionFormato() {  //funcion que guarda la observacion del estado del formato
    var formatocodi = $('#hFormatoCodi').val();
    var ensayocodi = $('#hEnsayoCodi').val();
    var enunidadcodi = $('#hEnUnidadCodi').val();
    var strObservacion = $('#txtObserv').val();
    $.ajax({
        type: 'POST',
        url: controlador + "genera/GuardarObservacionFormato",
        data: {
            iEnunidcodi: enunidadcodi, iFormatoCodi: formatocodi, sObservacion: strObservacion
        },
        success: function (evt) {
            refresh(ensayocodi);
        },
        error: function () {
            alert("Ha ocurrido un error Guardar Observación");
        }
    });
}

function observarFormato(idFormato, nombreUnidad, idEnunidadcodi, nombreFormato) {
    $('#txtObserv').val("");
    document.getElementById("info").innerHTML = "Máximo 200 caracteres";
    $('#stUnidad').html('<H3>Unidad : ' + nombreUnidad + '</H3>');
    $('#stFormato').html('<H2>' + nombreFormato + '</H2>');
    $('#hEnUnidadCodi').val(idEnunidadcodi);
    $('#hFormatoCodi').val(idFormato);
    $('#popupObservarFortmato').bPopup({
        easing: 'easeOutBack',
        speed: 450,
        transition: 'slideDown'
    });
}

function aprobarFormato(idFormato, idEnunidadcodi) {
    $('#hFormatoCodi').val(idFormato);
    $('#hEnUnidadCodi').val(idEnunidadcodi);
    $('#popupAprobFormato').bPopup({
        easing: 'easeOutBack',
        speed: 450,
        transition: 'slideDown'
    });

}

function abrirArchivo(archivo) {
    //window.location = controlador + 'genera/ExportarReporteXls?nameFile=' + archivo;
    window.location = controlador + 'genera/DescargarArchivoEnvio?archivo=' + archivo;
}

function popupHistorialFormato(idEnsayoUnidad, idFormato) {

    $.ajax({
        type: 'POST',
        url: controlador + "genera/HistorialFormato",
        data: { ienunidadcodi: idEnsayoUnidad, iformatocodi: idFormato },
        success: function (evt) {
            $('#HistorialFormato').html(evt);
            setTimeout(function () {
                $('#popupEnsayoHistorialFormato').bPopup({
                    easing: 'easeOutBack',
                    speed: 450,
                    transition: 'slideDown'
                });
            }, 50);

        },
        error: function () {
            alert("Ha ocurrido un error en ingresar Historial Formato");
        }
    });
}