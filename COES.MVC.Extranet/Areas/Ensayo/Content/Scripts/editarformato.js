var controlador = siteRoot + 'Ensayo/';

$(function () {
    $('#tablaformatos').contextMenu({
        selector: '.context-menu-one',
        callback: function (key, options) {
            var strformato = $(this).attr('id');
            var nombreFormato = $(this).parent('tr').find("td:first").html();
            var idFormato = strformato.substring(3, strformato.length);
            
            if (key == "observado") {
                observarFormato(idFormato,nombreFormato);
                
            }
            if (key == "aprobado") {
                aprobarFormato(idFormato);
            }
        },
        items: {
            "observado": { name: "OBSERVAR"},
            "aprobado": { name: "APROBAR"},           
        }
    });

    $('.context-menu-one').on('click', function (e) {
        console.log('clicked', this);
        
    })

    $('#btnAceptarObservarFormat').click(function () {
        guardarObservacionFormato();             
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
        alert("El ensayo ha sido Aprobado!")
    }
    location.href = controlador + "genera/EditarFormato?id=" + id;
}

function guardarAprobacionFormato() { //funcion que guarda el estado de la aprobacion de un formato para un ensayo
    var formatocodi = $('#hFormatoCodi').val();
    var ensayocodi = $('#hEnsayoCodi').val();
    $.ajax({
        type: 'POST',
        url: controlador + "genera/GuardarAprobacionFormato",
        data: {
            iEnsayocodi: ensayocodi, iFormatoCodi: formatocodi
        },
        success: function (evt) {
            refresh(ensayocodi, evt.Ensayoaprob);
        },
        error: function () {
            alert("Ha ocurrido un error Guardar Observación");
        }
    });
}

function guardarObservacionFormato() {  //funcion que guarda la observacion del estado del formato
    var formatocodi = $('#hFormatoCodi').val();
    var ensayocodi = $('#hEnsayoCodi').val();
    var strObservacion = $('#txtObserv').val();
    $.ajax({
        type: 'POST',
        url: controlador + "genera/GuardarObservacionFormato",
        data: {
            iEnsayocodi: ensayocodi, iFormatoCodi: formatocodi, sObservacion: strObservacion
        },
        success: function (evt) {
            refresh(ensayocodi);
        },
        error: function () {
            alert("Ha ocurrido un error Guardar Observación");
        }
    });
}

function observarFormato(idFormato, nombreFormato) {
    $('#stFormato').html('<H3>' + nombreFormato + '</H3>');
    $('#hFormatoCodi').val(idFormato);
    $('#popupObservarFortmato').bPopup({
        easing: 'easeOutBack',
        speed: 450,
        transition: 'slideDown'
    });  
}

function aprobarFormato(idFormato) {
    $('#hFormatoCodi').val(idFormato);
    $('#popupAprobFormato').bPopup({
        easing: 'easeOutBack',
        speed: 450,
        transition: 'slideDown'
    });

}

function abrirArchivo(archivo) {
    //window.location = siteRoot + 'Areas/Ensayo/Repositorio/' + archivo;
   window.location = controlador + 'genera/DescargarArchivoEnvio?archivo=' + archivo;
}

function popupHistorialFormato(idCodi, idFormato) {
    
    $.ajax({
        type: 'POST',
        url: controlador + "genera/HistorialFormato",
        data: { iensayocodi: idCodi, iformatocodi: idFormato},
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