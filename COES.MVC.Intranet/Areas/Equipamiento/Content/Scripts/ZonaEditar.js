var controlador = siteRoot + 'equipamiento/zona/';
var tabla;
var cambio;
var validar;
$(function () {

    $('#btnCancelarEdit').on('click', function () {
        $('#popupEditarZona').bPopup().close();
    });
    
    $('#btnGrabarEdit').on('click', function () {
        validar = true;

        validacion();

        if (validar == true) { editZona(); }
        else { alert("nivel, nombre o abreviatura no validos"); };
        
    });
    $('#checkfiltroeditar').on('click', function () {
        var checkeado = document.getElementById('checkfiltroeditar').checked;

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

function repliegue() {
    var uwu = -1;
    $.ajax({
        type: 'POST',
        url: controlador + "EditarZona",
        data: {
            idTarea: cambio,
            areacodi:uwu
        },
        success: function (evt) {
            $('#editarZona').html(evt);         //está en index
            setTimeout(function () {
                $('#popupEditarZona').bPopup({       //está en index
                    easing: 'easeOutBack',
                    speed: 450,
                    transition: 'slideDown'
                });
            }, 50);
            tablaedit = $('#tablaAreasTotales').dataTable({     //tabla de EditarZona.cshtml
                ordering: false,
                info: false,
                columnDefs: [{
                    orderable: false,
                    targets: 0,
                    render: function (data, type, full, meta) {

                        var checkbox = $("<input/>", {
                            "type": "checkbox",
                            'name': 'id',
                            'value': $('<div/>').text(data).html()
                        });

                        for (i = 0; i < listaAreas.length; i++) {
                            if (full[0] == listaAreas[i]) {
                                checkbox.attr("checked", "checked");
                                checkbox.addClass("checkbox_checked");
                            } else {
                                checkbox.addClass("checkbox_unchecked");
                            }
                        }
                        return checkbox.prop("outerHTML")
                    }
                }],
                select: {
                    style: 'multi'
                },
                order: [[1, 'asc']]
            });
            if (cambio == 0) {

                $("#checkfiltroeditar").prop("checked", false);
                
            }
            else {
                $("#checkfiltroeditar").prop("checked", true);
                
            }
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
function validacion() {

    if (($('#cbNivelEditar').val() == -1) || ($("#txtZonanombre").val() == "") ||
        ($("#txtZonanombre").val() == null) || ($("#txtZonaabreviacion").val() == "") ||
        ($("#txtZonaabreviacion").val() == null)) {

        validar = false;
    }

}
function editZona() {
    var idsAreasEditar = tablaedit.$('input[type="checkbox"]').serialize();
    idsAreasEditar = idsAreasEditar.replace(/id=/g, "");
    
    var codigo = $("#hdnAreaCodi").val();
    var nombre = $("#txtZonanombre").val();
    var abreviatura = $("#txtZonaabreviacion").val();
    $.ajax({
        type: 'POST',
        url: controlador + "ActualizarZona",
        dataType: "json",
        data: {           
            iCodigo: codigo,
            Nivelcodi: $('#cbNivelEditar').val(),
            sNombre: nombre,
            sAbrevitura: abreviatura,
            AreasEditar: idsAreasEditar
        },
        success: function (resultado) {

            if (resultado == 1) {
                mostrarMensaje('mensaje', 'exito', 'El proceso se ejecutó correctamente.');
                $('#popupEditarZona').bPopup().close();
                consultar2();
            } else
                mostrarMensaje('mensaje', 'error', 'Se produjo un error.');
        },
        error: function () {
            mostrarMensaje('mensaje', 'error', 'Se produjo un error.');
        }
    });

};