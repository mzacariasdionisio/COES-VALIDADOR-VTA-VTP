var controlador = siteRoot + 'ConsumoCombustible/Configuracion/';
$(function () {

    listadoTipoCombustible();

    $('#btnActualizar').click(function () {
        actualizarCombustible();
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
});

///////////////////////////
/// Formulario 
///////////////////////////

function listadoTipoCombustible() {
    $('#listado').html('');

    ancho = $("#mainLayout").width() > 1100 ? $("#mainLayout").width() - 30 : 1100;

    $.ajax({
        type: 'POST',
        url: controlador + "TipoCombustibleListado",
        data: {
        },
        success: function (evt) {
            if (evt.Resultado != "-1") {
                $('#listado').html(evt.Resultado);

                $("#listado").css("width", (ancho) + "px");
                $('#tabla_combustible').dataTable({
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

function editarCombustible(id) {
    $("#hfFenergcodi").val(0);
    $("#txtNombre").val('');
    $("#txtCodigoOsi").val('');

    $.ajax({
        type: 'POST',
        url: controlador + 'TipoCombustibleDatos',
        data: {
            fenergcodi: id
        },
        cache: false,
        success: function (result) {
            if (result.Resultado == '-1') {
                alert('Ha ocurrido un error:' + result.Mensaje);
            } else {
                var reg = result.FuenteEnergia;

                $("#hfFenergcodi").val(reg.Fenergcodi);
                $("#txtNombre").val(reg.Fenergnomb);
                $("#txtCodigoOsi").val(reg.Osinergcodi);

                var array1 = [];
                array1.push(reg.Tinfcoes);

                var array2 = [];
                array2.push(reg.Tinfosi);

                $("#cbMedidaOrigen").multipleSelect('setSelects', array1)
                $("#cbMedidaDestino").multipleSelect('setSelects', array2)

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

function actualizarCombustible() {
    var fenergcodi = parseInt($("#hfFenergcodi").val()) || 0;
    var osinergcodi = $("#txtCodigoOsi").val();
    var tinfcoes = parseInt($("#cbMedidaOrigen").val()) || 0;
    var tinfosi = parseInt($("#cbMedidaDestino").val()) || 0;

    $.ajax({
        type: 'POST',
        url: controlador + 'TipoCombustibleActualizar',
        data: {
            fenergcodi: fenergcodi,
            osinergcodi: osinergcodi,
            tinfcoes: tinfcoes,
            tinfosi: tinfosi
        },
        cache: false,
        success: function (result) {
            if (result.Resultado == '-1') {
                alert('Ha ocurrido un error:' + result.Mensaje);
            } else {
                alert('El registro se actualizó correctamente.');

                $('#popupEditar').bPopup().close();
                listadoTipoCombustible();
            }
        },
        error: function (err) {
            alert('Ha ocurrido un error.');
        }
    });
}
