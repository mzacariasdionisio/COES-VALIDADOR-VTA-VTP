var controlador = siteRoot + 'despacho/BarraModelada/';
var tabla,tablaedit;
$(function () {

    $('#btnNuevo').on('click', function () {
        nuevaBarraModelada();
    });
    consultar();
    $('#txtGrupotension').keypress(function (event) {
        return isNumber(event, this);
    });
});

consultar = function() {

   $.ajax({
        type: 'POST',
            url: controlador + 'ListaBarrasModeladas',
            success: function(evt) {
                $('#listado').html(evt);
                $('#tablaListado').dataTable({
                    "iDisplayLength": 25
                });
            },
            error: function() {
                mostrarMensaje('mensaje', 'error', 'Ha ocurrido un error.');
            }
        });
};
function detalleBarraModelada(id) {
    $.ajax({
        type: 'POST',
        url: controlador + "DetalleBarra?grupoCodi=" + id,
        success: function (evt) {
            $('#detBarraModelada').html(evt);
            setTimeout(function () {
                $('#popupDetalle').bPopup({
                    easing: 'easeOutBack',
                    speed: 450,
                    transition: 'slideDown'
                });
            }, 50);
            $('#tablaListadoBarras').dataTable({
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
function nuevaBarraModelada() {
    $.ajax({
        type: 'POST',
        url: controlador + "NuevaBarra",
        success: function (evt) {
            $('#nuevaBarraModelada').html(evt);
            setTimeout(function () {
                $('#popupNuevo').bPopup({
                    easing: 'easeOutBack',
                    speed: 450,
                    transition: 'slideDown'
                });
            }, 50);

            tabla = $('#tablaEquiposDisponibles').dataTable({
                ordering: false,
                info: false,
                columnDefs: [ {
                    orderable: false,
                    //className: 'select-checkbox',
                    targets: 0,
                    render: function (data, type, full, meta){
                    return '<input type="checkbox" name="id" value="' + $('<div/>').text(data).html() + '">';
        }
                } ],
                select: {
                    style: 'multi'
                },
                order: [[1, 'asc']]
            });
        },
        error: function () {
            mostrarMensaje('mensaje', 'error', 'Se produjo un error.');
        }
    });
};
function editarBarraModelada(id) {
    $.ajax({
        type: 'POST',
        url: controlador + "EditarBarra?grupoCodi="+id,
        success: function (evt) {
            $('#editarBarraModelada').html(evt);
            setTimeout(function () {
                $('#popupEditar').bPopup({
                    easing: 'easeOutBack',
                    speed: 450,
                    transition: 'slideDown'
                });
            }, 50);

            tablaedit = $('#tablaEquiposTotales').dataTable({
                columnDefs: [
                    {
                    orderable: false,
                    targets: 0,
                    render: function (data, type, full, meta) {
                        var checkbox = $("<input/>", {
                            "type": "checkbox",
                            'name': 'id',
                            'value': $('<div/>').text(data).html()
                        });
                        
                        if (full[5] !== "") {
                            checkbox.attr("checked", "checked"); 
                            checkbox.addClass("checkbox_checked");
                        } else {
                            checkbox.addClass("checkbox_unchecked");
                        }
                        return checkbox.prop("outerHTML");

                    }
                    },
                    {
                        targets: 5,
                        visible: false,
                        searchable: false
                    }
                ],
                select: {
                    style: 'multi'
                },
                order: [[1, 'asc']]
            });
        },
        error: function () {
            mostrarMensaje('mensaje', 'error', 'Se produjo un error.');
        }
    });
};
function isNumber(evt, element) {

    var charCode = (evt.which) ? evt.which : event.keyCode;
    if (
        (charCode != 45 || $(element).val().indexOf('-') != -1) &&      // “-” CHECK MINUS, AND ONLY ONE.
            (charCode != 46 || $(element).val().indexOf('.') != -1) &&      // “.” CHECK DOT, AND ONLY ONE.
            (charCode < 48 || charCode > 57))
        return false;

    return true;
};
mostrarMensaje = function (id, tipo, mensaje) {
    $('#' + id).removeClass();
    $('#' + id).addClass('action-' + tipo);
    $('#' + id).html(mensaje);
}