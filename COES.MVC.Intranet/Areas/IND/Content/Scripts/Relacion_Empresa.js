var controler = siteRoot + "IND/RelacionEmpresa/";

//Funciones de busqueda
$(document).ready(function () {
    buscar();
    $('#btnBuscar').click(function () {
        buscar();
    });

    $('#btnActualizarLista').click(function () {
        actualizarLista();
    });
});

function buscar() {
    $.ajax({
        type: 'POST',
        url: controler + "ListadoRelacionEmpresa",
        data: { nombre: $('#txtNombre').val() },
        success: function (evt) {
            $('#listado').html(evt);
            addDeleteEvent();
            oTable = $('#tabla').dataTable({
                "sPaginationType": "full_numbers",
                "destroy": "true"
            });
        },
        error: function () {
            alert("Lo sentimos, se ha producido un error");
        }
    });
};


//Funciones de eliminado de registro

function addDeleteEvent() {
    $(".delete").click(function (e) {
        e.preventDefault();
        if (confirm("¿Desea eliminar la información seleccionada?")) {
            id = $(this).attr("id").split("_")[1];
            alert(id);
            $.ajax({
                type: "POST",
                url: controler + "Delete",
                data: { id: id },
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

addAntiForgeryToken = function (data) {
    data.__RequestVerificationToken = $('input[name=__RequestVerificationToken]').val();
    return data;
};

function actualizarLista() {
    $.ajax({
        type: 'POST',
        url: controler + 'actualizarlista',
        dataType: 'json',
        success: function (result) {
            if (result == "1") {
                alert("La lista se actualizo correctamente");
            }
            else {
                alert(result);
            }
        },
        error: function () {
            alert("Error");
        }
    });
}
