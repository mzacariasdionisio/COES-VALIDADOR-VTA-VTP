var controlador = siteRoot + 'equipamiento/zona/';
var tabla, tablaedit,i;
var listaAreas = new Array();

$(function () {
    consultar2();
    $('#btnConsultar').on('click', function () {
        consultar();
    });

    $('#btnNuevaZona').on('click', function () {
        nuevaZona();
    });

    $('#cbNivel').change(function () {
        consultar2();
    });
 
});

consultar = function () {
    $.ajax({
        type: 'POST',
        url: controlador + 'ListaZonas',
        success: function (evt) {
            $('#listadoZonas').html(evt);
            $('#tablaListado').dataTable({
                "iDisplayLength": 25
            });
        },
        error: function () {
            mostrarMensaje('mensaje', 'error', 'Ha ocurrido un error.');
        }
    });
};

consultar2 = function () {
    $.ajax({
        type: 'POST',
        url: controlador + 'ListaZonasxNivel',
        data: {
            anivelcodi: $('#cbNivel').val()
        },
        success: function (evt) {
            $('#listadoZonas').html(evt);
            $('#tablaListado2').dataTable({
                "iDisplayLength": 25
            });
        },

        error: function () {
            mostrarMensaje('mensaje', 'error', 'Ha ocurrido un error.');
        }
    });
};

function detalleZona(id) {
    $.ajax({
        type: 'POST',
        url: controlador + "DetalleZona",
        data: {
            areacodi: id
        },
        success: function (evt) {
            $('#detZona').html(evt);
            setTimeout(function () {
                $('#popupDetalleZona').bPopup({
                    easing: 'easeOutBack',
                    speed: 450,
                    transition: 'slideDown'
                });
            }, 50);
            $('#tablaListadoAreas').dataTable({
                "searching": false,
                "paging": false,
                "ordering": false,
                "info": false
            });
        },
        error: function () {
            mostrarMensaje('mensaje', 'error', 'Se produjo un error.');
        }
    });
};
function buscarAreas(areacodi) {
    
    $.ajax({
        type: 'POST',
        url: controlador + "BuscarAreas",
        dataType: "json",
        data: {
            iCodigo: areacodi
        },
        success: function (resultado) {
            listaAreas = resultado.lista;
            editarZona(areacodi);
            
        },
        error: function () {
            mostrarMensaje('mensaje', 'error', 'Se produjo un error.');
        }
    });
};

function editarZona(id) {
    var f=1;
    $.ajax({
        type: 'POST',
        url: controlador + "EditarZona",        // Controller
        data: {
            areaCodi: id,
            idTarea: f
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

                        for (i = 0; i <listaAreas.length; i++) {
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
            $("#checkfiltroeditar").prop("checked", true);
        },
        error: function () {
            mostrarMensaje('mensaje', 'error', 'Se produjo un error.');
        }
    });
};

function nuevaZona() {
    var i = 1;
    $.ajax({
        type: 'POST',
        url: controlador + "NuevaZona",
        data: {
            idTarea: i,
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
            $("#checkfiltro").prop("checked", true);
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