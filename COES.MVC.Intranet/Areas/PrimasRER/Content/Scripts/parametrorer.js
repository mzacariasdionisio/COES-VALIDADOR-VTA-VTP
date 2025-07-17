var controler = siteRoot + "PrimasRER/ParametroRER/";

$(document).ready(function () {
    buscar();
    $('#btnNuevo').click(function () {
        nuevo();
    });
    $("#popupNewParametroRER").addClass("general-popup");
    $("#popupEditParametroRER").addClass("general-popup");
    $('#tablaMeses').dataTable({
        "sPaginationType": "full_numbers",
        "destroy": "true",
        "bSort": false
    });
    $('#btnBuscar').click(function () {
        buscarVersion();
    });

    $('#version').change(function () {
        var version = $(this).val(); // Obtener la version seleccionada
        // Realizar la solicitud AJAX
        $.ajax({
            url: controler + "ListaMeses",
            type: 'GET',
            data: {
                anio: $('#anio').val(),
                descripcion: $('#Rerppraniotarifdesc').val(),
                version: version
            },
            success: function (result) {
                $('#tablaMeses tbody').html(result);
            },
            error: function () {
                alert('Ocurrió un error al cargar los datos.');
            }
        });
    });
});

buscar = function () {
    $.ajax({
        type: 'POST',
        url: controler + "lista",
        success: function (evt) {
            $('#listado').html(evt);
            addDeleteEvent();
            viewEvent();
            oTable = $('#tabla').dataTable({
                "sPaginationType": "full_numbers",
                "destroy": "true",
                "aaSorting": [[0, "desc"], [1, "desc"], [2, "asc"]]
            });
        },
        error: function () {
            mostrarError();
        }
    });
};

nuevo = function () {
    $.ajax({
        type: 'POST',
        url: controler + "new",
        success: function (evt) {
            $('#new').html(evt);
            $('#mensaje').css("display", "none");
            setTimeout(function () {
                $('#popupNewParametroRER').bPopup({
                    easing: 'easeOutBack',
                    speed: 450,
                    transition: 'slideDown'
                });
            }, 50);

        },
        error: function () {
            mostrarError();
        }
    });
};

save = function () {
    if (confirm('¿Está seguro que desea crear el Año Tarifario?')) {
        $.ajax({
            type: 'POST',
            url: controler + 'Save',
            dataType: 'json',
            data: $('#frmNewParametro').serialize(),
            cache: false,
            success: function (resultado) {
                if (resultado == 1) {
                    mostrarExitoOperacion();
                    $('#popupNewParametroRER').bPopup().close();
                    buscar();
                } else if (resultado == -2) {
                    alert("El Año Tarifario ya existe")
                    $('#popupNewParametroRER').bPopup().close();
                } else if (resultado == -3) {
                    alert("Cada versión debe tener un valor de inflación diferente de cero y debe tener como máximo 3 enteros y 3 decimales")
                } else {
                    mostrarError();
                }
            },
            error: function () {
                mostrarError();
            }
        });
    }
};


//Funciones de eliminado de registro
function addDeleteEvent() {
    $(".delete").click(function (e) {
        e.preventDefault();
        if (confirm("¿Desea eliminar la información seleccionada?")) {
            id = $(this).attr("id").split("_")[1];
            $.ajax({
                type: "post",
                dataType: "text",
                url: controler + "Delete/" + id,
                data: addAntiForgeryToken({ id: id }),
                success: function (resultado) {
                    if (resultado == "true")
                        $("#fila_" + id).remove();
                    else
                        alert("No se ha logrado eliminar el registro");
                }
            });
        }
    });
};

function viewEvent() {

    $('.view').click(function () {
        id = $(this).attr("id").split("_")[1];
        abrirPopup(id);
    });
};

editar = function (Reravcodi) {
    $.ajax({
        type: 'POST',
        url: controler + "Edit",
        data: {
            Reravcodi: Reravcodi
        },
        success: function (evt) {
            $('#edit').html(evt);
            $('#mensaje').css("display", "none");
            setTimeout(function () {
                $('#popupEditParametroRER').bPopup({
                    easing: 'easeOutBack',
                    speed: 450,
                    transition: 'slideDown'
                });
            }, 50);

        },
        error: function () {
            mostrarError();
        }
    });
};

update = function () {
    if (confirm('¿Está seguro que desea actualizar el Año Tarifario?')) {
        $.ajax({
            type: 'POST',
            url: controler + 'update',
            dataType: 'json',
            data: $('#frmEditParametroRER').serialize(),
            cache: false,
            success: function (resultado) {
                if (resultado == 1) {
                    mostrarExitoOperacion();
                    $('#popupEditParametroRER').bPopup().close();
                    buscar();
                } else if (resultado == -3) {
                    alert("Cada versión debe tener un valor de inflación diferente de cero y debe tener como máximo 3 enteros y 3 decimales")
                } else
                    mostrarError();
            },
            error: function () {
                mostrarError();
            }
        });
    }
};

mostrarExitoOperacion = function () {
    $('#mensaje').removeClass("action-error");
    $('#mensaje').addClass("action-exito");
    $('#mensaje').text("La operación se realizó con exito...!");
    $('#mensaje').css("display", "block");
};

mostrarError = function () {
    $('#mensaje').removeClass("action-exito");
    $('#mensaje').addClass("action-error");
    $('#mensaje').text("Ha ocurrido un error");
    $('#mensaje').css("display", "block");
};
