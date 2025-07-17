var controlador = siteRoot + 'ConsumoCombustible/Configuracion/';
$(function () {

    $('#btnNuevo').click(function () {
        abrirNuevoPopup();
    });
    $('#btnGuardar').click(function () {
        guardarNuevoFactor();
    });
    $('#btnEditar').click(function () {
        actualizarFactor();
    });

    $('#cbMedidaOrigen').multipleSelect({
        width: '350px',
        filter: true,
        single: true,
        onClose: function () {
        }
    });
    $('#cbMedidaDestino').multipleSelect({
        width: '350px',
        filter: true,
        single: true,
        onClose: function () {
        }
    });

    listadoFactorConversion();
});

///////////////////////////
/// Formulario 
///////////////////////////

function listadoFactorConversion() {
    $('#listado').html('');

    ancho = $("#mainLayout").width() > 1100 ? $("#mainLayout").width() - 30 : 1100;

    $.ajax({
        type: 'POST',
        url: controlador + "FactorConversionListado",
        data: {
        },
        success: function (evt) {
            if (evt.Resultado != "-1") {
                $('#listado').html(evt.Resultado);

                $("#listado").css("width", (ancho) + "px");
                $('#tabla_factor').dataTable({
                    "sPaginationType": "full_numbers",
                    "destroy": "true",
                    "ordering": false,
                    "searching": false,
                    "iDisplayLength": -1,
                    "info": false,
                    "paging": false,
                    "scrollX": true,
                    "scrollY": "100%"
                });
            } else {
                alert("Ha ocurrido un error: " + evt.Mensaje);
            }
        },
        error: function (err) {
            alert('Ha ocurrido un error.');
        }
    });
}

function editarFactor(id) {
    $("#hfFactorcodi").val(0);
    $("#txtMedidaOrigen").val('');
    $("#txtMedidaDestino").val('');
    $("#txtFactorEdicion").val('');

    $.ajax({
        type: 'POST',
        url: controlador + 'FactorConversionDatos',
        data: {
            tconvcodi: id
        },
        cache: false,
        success: function (result) {
            if (result.Resultado == '-1') {
                alert('Ha ocurrido un error:' + result.Mensaje);
            } else {
                var reg = result.FactorConversion;

                $("#hfFactorcodi").val(reg.Tconvcodi);
                $("#txtMedidaOrigen").val(reg.Tinforigenabrev);
                $("#txtMedidaDestino").val(reg.Tinfdestinoabrev);
                $("#txtFactorEdicion").val(reg.Tconvfactor);

                setTimeout(function () {
                    $('#popupEditar').bPopup({
                        easing: 'easeOutBack',
                        speed: 450,
                        transition: 'slideDown',
                        modalClose: false
                    });
                }, 50);
            }
        },
        error: function (err) {
            alert('Ha ocurrido un error.');
        }
    });
}

function actualizarFactor() {
    var id = parseInt($("#hfFactorcodi").val()) || 0;
    var factor = $("#txtFactorEdicion").val();

    $.ajax({
        type: 'POST',
        url: controlador + 'FactorConversionActualizar',
        data: {
            tconvcodi: id,
            tconvfactor: factor,
        },
        cache: false,
        success: function (result) {
            if (result.Resultado == '-1') {
                alert('Ha ocurrido un error:' + result.Mensaje);
            } else {
                alert('El registro se actualizó correctamente.');

                $('#popupEditar').bPopup().close();
                listadoFactorConversion();
            }
        },
        error: function (err) {
            alert('Ha ocurrido un error.');
        }
    });
}

function guardarNuevoFactor() {
    var factor = $("#txtFactor").val();
    var tinforigen= parseInt($("#cbMedidaOrigen").val()) || 0;
    var tinfdestino  = parseInt($("#cbMedidaDestino").val()) || 0;

    $.ajax({
        type: 'POST',
        url: controlador + 'FactorConversionGuardar',
        data: {
            tinfdestino: tinfdestino,
            tinforigen: tinforigen,
            tconvfactor: factor,
        },
        cache: false,
        success: function (result) {
            if (result.Resultado == '-1') {
                alert('Ha ocurrido un error:' + result.Mensaje);
            } else {
                alert('El registro se registró correctamente.');

                $('#popupNuevo').bPopup().close();
                listadoFactorConversion();
            }
        },
        error: function (err) {
            alert('Ha ocurrido un error.');
        }
    });
}


function abrirNuevoPopup() {
    var array1 = [];

    var array2 = [];

    $("#cbMedidaOrigen").multipleSelect('setSelects', array1);
    $("#cbMedidaDestino").multipleSelect('setSelects', array2);
    $("#txtFactor").val('');

    setTimeout(function () {
        $('#popupNuevo').bPopup({
            easing: 'easeOutBack',
            speed: 450,
            transition: 'slideDown',
            modalClose: false
        });
    }, 50);
}