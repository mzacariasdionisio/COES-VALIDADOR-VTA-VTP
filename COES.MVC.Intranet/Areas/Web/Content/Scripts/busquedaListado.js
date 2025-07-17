/* global controlador, mostrarMensaje */

$(function () {
    inicializarEventos();
});

function inicializarEventos() {
    escucharRecomendacion();
    actualizarPalabrasClave();
    calificar();
    manejarSeleccionRelacionados();
    escucharRelacionados();
}

function escucharRecomendacion() {
    $(document).off("click", ".vote-star");
    $(document).on("click", ".vote-star", function () {
        let $this = $(this);
        //if ($(this).hasClass("voted")) return;

        $this.addClass("voted"); // Agrega clase para evitar más clics
        $this.removeClass("fa-star").addClass("fa-spinner fa-spin");

        // Crear un objeto con todos los datos de la fila
        const item = JSON.parse(this.getAttribute("data-item"));
        const idBusqueda = JSON.parse(this.getAttribute("data-extra"));

        // Enviar la votación al servidor con fetch
        fetch(controlador + "Recomendar?idBusqueda=" + idBusqueda, {
            method: "POST",
            headers: {
                "Content-Type": "application/json",
            },
            body: JSON.stringify(item),
        })
            .then((response) => {
                if (response.ok) {
                    return response.json();
                } else {
                    mostrarMensaje("mensaje", "error", "Error al procesar la recomendación.");
                }
            })
            .then((data) => {
                if (data.Success) {
                    if (data.Message.startsWith("Recomendación removida")) {
                        $this.removeClass("voted fa-spinner fa-spin")
                            .addClass("fa-star")
                            .css("pointer-events", "auto")
                            .css("opacity", "1");
                    } else {
                        // Cambiar el color de la estrella a dorado para indicar que fue votada
                        $this.removeClass("fa-spinner fa-spin")
                            .addClass("fa-star voted")
                            .css("opacity", "1");
                    }

                    mostrarMensaje("mensaje", "exito", data.Message);
                } else {
                    $this.removeClass("voted fa-spinner fa-spin")
                        .addClass("fa-star")
                        .css("pointer-events", "auto")
                        .css("opacity", "1");
                    mostrarMensaje("mensaje", "error", data.Message);
                }
            })
            .catch((err) => {
                console.error("Error en la calificación:", err);
                mostrarMensaje("mensaje", "error", "Error en la recomendación. Intenta nuevamente.");
                $this.removeClass("voted fa-spinner fa-spin")
                    .addClass("fa-star").css("pointer-events", "auto")
                    .css("opacity", "1");

            });
    });
}

function escucharRelacionados() {
    $(document).off("click", ".icono-verificacion");
    $(document).on("click", ".icono-verificacion", function () {
        let $this = $(this);

        $this.addClass("voted"); // Agrega clase para evitar más clics
        $this.removeClass("fa-solid fa-minus").addClass("fa-spinner fa-spin");

        // Crear un objeto con todos los datos de la fila
        const item = JSON.parse(this.getAttribute("data-item"));
        const idBusqueda = JSON.parse(this.getAttribute("data-extra"));

        // Enviar la votación al servidor con fetch
        fetch(controlador + "Relacionar?idBusqueda=" + idBusqueda, {
            method: "POST",
            headers: {
                "Content-Type": "application/json"
            },
            body: JSON.stringify(item),
        })
            .then((response) => {
                if (response.ok) {
                    return response.json();
                } else {
                    mostrarMensaje("mensaje", "error", "Error al procesar la relación.");
                }
            })
            .then((data) => {
                if (data.Success) {
                    console.log(data);
                    if (data.Message === "Relacionamiento eliminado") {
                        // remoción de relacionamiento
                        $this
                            .removeClass("voted fa-spinner fa-spin")
                            .addClass("fa-solid fa-minus")
                            .css("pointer-events", "auto")
                            .css("opacity", "1");
                    } else {
                        // Cambiar el color de la estrella a dorado para indicar que fue votada
                        $this.removeClass("fa-spinner fa-spin")
                            .addClass("fa-solid fa-check voted")
                            .css("opacity", "1");
                    }

                    mostrarMensaje("mensaje", "exito", data.Message);
                } else {
                    $this.removeClass("voted fa-spinner fa-spin")
                        .addClass("fa-solid fa-minus")
                        .css("pointer-events", "auto")
                        .css("opacity", "1");
                    mostrarMensaje("mensaje", "error", data.Message);
                }
            })
            .catch((err) => {
                console.error("Error en la calificación:", err);
                mostrarMensaje(
                    "mensaje",
                    "error",
                    "Error en la relación. Intenta nuevamente."
                );
                $this
                    .removeClass("voted fa-spinner fa-spin")
                    .addClass("fa-solid fa-minus")
                    .css("pointer-events", "auto")
                    .css("opacity", "1");

            });
    });
}

function actualizarPalabrasClave() {
    $(document).off("click", ".edit-icon");
    $(document).on("click", ".edit-icon", function () {
        // Cambiar a modo edición
        const $row = $(this).closest("tr");
        $row.find(".display-value").addClass("d-none"); // Ocultar texto
        $row.find(".edit-input").removeClass("d-none"); // Mostrar campo de entrada
        $(this).addClass("d-none"); // Ocultar ícono de edición
        $row.find(".save-icon").removeClass("d-none"); // Mostrar ícono de guardar
    });

    $(document).off("click", ".save-icon");
    $(document).on("click", ".save-icon", function () {
        // Guardar cambios
        const $row = $(this).closest("tr");
        const id = $row.find(".editable-cell").data("rowkey"); // ID del registro
        const newValue = $row.find(".edit-input").val(); // Nuevo valor

        // Enviar el cambio al servidor
        $.ajax({
            url: controlador + "ActualizarPalabrasClave", // Cambia por tu URL de controlador
            method: "POST",
            data: {
                id: id,
                palabrasClave: newValue
            },
            dataType: "json",
            success: function (response) {
                if (response.Success === true) {
                    // Actualizar la interfaz
                    $row.find(".display-value").text(newValue).removeClass("d-none");
                    $row.find(".edit-input").addClass("d-none");
                    $row.find(".save-icon").addClass("d-none");
                    $row.find(".edit-icon").removeClass("d-none");
                    mostrarMensaje("mensaje", "exito", response.Message);
                } else {
                    mostrarMensaje("mensaje", "error", response.Message);
                }
            },
            error: function () {
                mostrarMensaje("mensaje", "error", "Error al actualizar las palabras clave");
            },
        });
    });
}

function calificar() {
    $(document).off("click", ".btn-calificar");
    $(document).on("click", ".btn-calificar", function () {
        // Deshabilitar todos los botones para evitar múltiples clics
        $(".btn-calificar").prop("disabled", true);

        // Crear un objeto con todos los datos de la fila
        const calificacion = this.getAttribute("data-calificacion");
        const idBusqueda = this.getAttribute("data-extra");
        const clickedButton = $(this);
        const parentRow = clickedButton.closest("tr");
        parentRow.find(".btn-calificar").removeClass("voted");

        fetch(controlador + "Calificar?", {
            method: "POST",
            headers: {
                "Content-Type": "application/json"
            },
            body: JSON.stringify({
                idBusqueda: idBusqueda,
                calificacion: calificacion
            }),
        })
            .then((response) => {
                if (!response.ok) {
                    mostrarMensaje("mensaje", "error", "Error al procesar la calificación.");
                }
                return response.json();
            })
            .then((data) => {
                if (data.Success) {
                    // Cambiar el color
                    if (data.Message !== "Calificación removida correctamente") {
                        this.classList.add("voted");
                    }
                    mostrarMensaje("mensaje", "exito", data.Message);
                } else {
                    mostrarMensaje("mensaje", "error", data.Message);
                }
                $(".btn-calificar").prop("disabled", false);
            })
            .catch((err) => {
                console.error("Error en la calificación:", err);
                mostrarMensaje("mensaje", "error", "Error en la calificación. Intenta nuevamente.");
                $(".btn-calificar").prop("disabled", false);
            });
    });
}

function guardarRegistro(event, registro, idBusqueda) {
    fetch(controlador + "GuardarDocumentoAbierto", {
        method: "POST",
        headers: {
            "Content-Type": "application/json",
        },
        body: JSON.stringify({ registro: registro, idBusqueda: idBusqueda }),
    });
}

function manejarSeleccionRelacionados() {
    $(document).off("change", ".selectionRelacionados");
    $(document).on("change", ".selectionRelacionados", function () {
        let selectedValue = $(this).val() === "true";
        let idBusqueda = $(this).data("extra");

        $.ajax({
            url: controlador + "GuardarSeleccionRelacionados",
            type: "POST",
            data: { idBusqueda: idBusqueda, seleccion: selectedValue },
            dataType: "json",
            success: function (response) {
                let tabla = $("#tabla").DataTable();
                if (response.Success === true) {
                    if (response.Message === "Relacionamiento activo") {
                        tabla.column(7).visible(true);
                    } else {
                        tabla.column(7).visible(false);
                    }
                    mostrarMensaje("mensaje", "exito", response.Message);

                } else {
                    mostrarMensaje("mensaje", "error", response.Message);
                }
            },
            error: function () {
                mostrarMensaje("mensaje", "error", "Error en el relacionamiento. Intenta nuevamente.");
            },
        });
    });
}
