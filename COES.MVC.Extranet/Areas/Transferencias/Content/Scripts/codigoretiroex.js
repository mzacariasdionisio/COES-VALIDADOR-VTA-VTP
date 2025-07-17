var controler = siteRoot + "transferencias/codigoretiro/";

$(document).ready(function () {
    buscar();

    $('#CLICODI2').multipleSelect({
        width: '200px',
        filter: true,
        selectAll: false,
        single: true
    });

    $('#BARRCODI2').multipleSelect({
        width: '200px',
        filter: true,
        selectAll: false,
        single: true
    });

    $('.txtFecha').Zebra_DatePicker({
    });

    $('#btnBuscar').click(function () {
        buscar();
    });

    $('#btnGenerarExcel').click(function () {
        generarExcel();
    });
});

//Funciones de eliminado de registro
function add_deleteEvent() {
    $(".delete").click(function (e) {
        e.preventDefault();
        if (confirm("Desea solicitar dar de baja este contrato ?")) {
            id = $(this).attr("id").split("_")[1];
            $.ajax({
                type: "post",
                dataType: "text",
                url: controler + "Delete/" + id,
                data: AddAntiForgeryToken({ id: id }),
                success: function (resultado) {
                    if (resultado == "true") {
                        alert("Operación realizada correctamente.");
                        buscar();
                    }
                    else
                        alert("No se ha logrado eliminar el registro");
                }
            });
        }
    });
};

AddAntiForgeryToken = function (data) {
    data.__RequestVerificationToken = $('input[name=__RequestVerificationToken]').val();
    return data;
};

function buscar() {
    $.ajax({
        type: 'POST',
        url: controler + "lista",  
        data: { nombre: $('#txtNombre').val(), tipousu: $("#TIPOUSUACODI option:selected").text(), tipocont: $("#TIPOCONTCODI option:selected").text(), bartran: $("#BARRTRANCODI option:selected").text(), clinomb: $("#CLICODI option:selected").text(), fechaInicio: $('#txtfechaIni').val(), fechaFin: $('#txtfechaFin').val() },
        success: function (evt) {
            $('#listado').html(evt);
            add_deleteEvent();
            ViewEvent();
            oTable = $('#tabla').dataTable({
                "sPaginationType": "full_numbers",
                "destroy": "true",
                "aaSorting": [[1, "asc"]]
            });
        },
        error: function () {
            mostrarError();
        }
    });
}

function ViewEvent() {
    $('.view').click(function () {
        id = $(this).attr("id").split("_")[1];
        abrirPopup(id);
    });
};

abrirPopup = function (id) {
    $.ajax({
        type: 'POST',
        url: controler + "View/" + id,
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
            alert("Lo sentimos, ha ocurrido un error inesperado");
        }
    });
}

mostrarError = function () {
    alert("Ha ocurrido un error.");
}

mostrarMensaje = function (mensaje) {
    alert(mensaje);
}

seleccionarEmpresa = function () {
    $.ajax({
        type: 'POST',
        url: controler + "EscogerEmpresa",
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
            alert("Lo sentimos, ha ocurrido un error inesperado");
        }
    });
}

generarExcel = function () {
    $.ajax({
        type: 'POST',
        url: controler + 'generarexcel',
        data: { nombre: $('#txtNombre').val(), tipousu: $("#TIPOUSUACODI option:selected").text(), tipocont: $("#TIPOCONTCODI option:selected").text(), bartran: $("#BARRTRANCODI option:selected").text(), clinomb: $("#CLICODI option:selected").text(), fechaInicio: $('#txtfechaIni').val(), fechaFin: $('#txtfechaFin').val() },
        dataType: 'json',
        success: function (result) {
            if (result == 1) {
                window.location = controler + 'abrirexcel';
            }
            if (result == -1) {
                mostrarError();
            }
        },
        error: function () {
            mostrarError();
        }
    });
}