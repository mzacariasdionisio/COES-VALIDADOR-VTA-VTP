var controlador = siteRoot + 'equipamiento/zona/';
var tabla;
var cambio;
var validar;
$(function () {

    $('#btnCancelarNuevo').on('click', function () {
        $('#popupNuevo').bPopup().close();
    });

    $('#btnGrabarNuevo').on('click', function () {
        validar = true;
        
        validacion();
        
        if (validar == true) { guardarZona(); }
        else { alert("nivel, nombre o abreviatura no validos");};

        
    });
    $('#checkfiltro').on('click', function () {
        var checkeado = document.getElementById('checkfiltro').checked;

        if (checkeado == false) {
            cambio = 0;
            checkeado = false;
            
        } else {
            
            cambio = 1;
            checkeado = true;
            
        };
        repliegue();
       
       
    });
    

});
function validacion()
{
    
    if (($('#cbNiveles').val() == -1) || ($("#txtZonanomb").val() == "") ||
        ($("#txtZonanomb").val() == null) || ($("#txtZonaabrev").val()== "") ||
        ($("#txtZonaabrev").val() == null)) {
        
        validar = false;
    }
    
    

    
}
function repliegue() {
   
    $.ajax({
        type: 'POST',
        url: controlador + "NuevaZona",
        data: {
            idTarea: cambio,
        },
        success: function (evt) {
            
            $('#nuevaZona').html(evt);
           
            setTimeout(function () {
                $('#popupNuevo').bPopup({
                    easing: 'easeOutBack',
                    speed: 450,
                    transition: 'slideDown'
                });
            }, 50);
            tabla = $('#tablaAreasDisponibles').dataTable({
                ordering: false,
                info: false,
                columnDefs: [{
                    orderable: false,
                    targets: 0,
                    render: function (data, type, full, meta) {
                        return '<input type="checkbox" name="id" value="' + $('<div/>').text(data).html() + '">';
                    }
                }],
                select: {
                    style: 'multi'
                },
                order: [[1, 'asc']]
            });
            

            if (cambio == 0) {

              $("#checkfiltro").prop("checked", false);
            }
            else { $("#checkfiltro").prop("checked", true);}


                

            
        },
        error: function () {
            mostrarMensaje('mensaje', 'error', 'Se produjo un error.');
        }
    });
    $.ajax({
        type: 'POST',
        url: controlador + "caja",
        data: {
            parametro: cambio,
        }
    });
    
}

function guardarZona() {

    var idsAreas = tabla.$('input[type="checkbox"]').serialize();
    idsAreas = idsAreas.replace(/id=/g, "");
    var nombreZona = $("#txtZonanomb").val();
    var abreviaturaZona = $("#txtZonaabrev").val();
    $.ajax({
        type: 'POST',
        url: controlador + "GrabarZona",
        dataType: "json",
        data: {
            anivelcodi: $('#cbNiveles').val(),
            sNombre: nombreZona,
            sAbrevitura: abreviaturaZona,
            areas: idsAreas
        },
        success: function (resultado) {

            if (resultado == 1) {
                mostrarMensaje('mensaje', 'exito', 'El proceso se ejecutó correctamente.');
                $('#popupNuevo').bPopup().close();
                consultar2();
            } else
                mostrarMensaje('mensaje', 'error', 'Se produjo un error.');
        },
        error: function () {
            mostrarMensaje('mensaje', 'error', 'Se produjo un error.');
        }
    });

};

mostrarMensaje = function (id, tipo, mensaje) {
    $('#' + id).removeClass();
    $('#' + id).addClass('action-' + tipo);
    $('#' + id).html(mensaje);
}