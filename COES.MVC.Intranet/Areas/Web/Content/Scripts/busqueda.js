/* global siteRoot */

const controlador = siteRoot + "web/busqueda/";

$(function () {
    // Inicializar elementos
    inicializarDatePickers();
    inicializarEventos();
    iniciarAutocomplete();
    escucharValorDeTop();
});

function inicializarDatePickers() {
    // Configurar Zebra_DatePicker en formato Año/Mes/Día
    $("#txtFechaInicio, #txtFechaFin").Zebra_DatePicker({
        format: "Y/m/d",
    });
}

function inicializarEventos() {
    // Botón de consulta
    $("#btnConsultar").on("click", consultar);
    $("#clearHistoryButton").on("click", limpiarChat);

    // Cambio en el formulario de selección
    const form = document.getElementById("selection-form");
    form.addEventListener("change", manejarCambioBusqueda);

    // Inicializar vista según la selección por defecto
    manejarCambioBusqueda();

    // Configuración de campos dinámicos
    inicializarCamposDinamicos(
        "fields-container",
        "add-field-btn",
        "dynamic-field"
    );

    // Palabras clave para excluir
    inicializarCamposDinamicos(
        "exclude-fields-container",
        "exclude-field-btn",
        "exclude-dynamic-field"
    );
}

function escucharValorDeTop() {
    document.getElementById("Result_number").addEventListener("input", function () {
        const max = parseInt(this.getAttribute("max"));
        const min = parseInt(this.getAttribute("min"));
        const value = parseInt(this.value);

        // Si el valor es mayor que el máximo, ajustarlo al máximo
        if (value > max) {
            this.value = max;
        }

        // Si el valor es menor que el mínimo, ajustarlo al mínimo
        if (value < min) {
            this.value = min;
        }
    });
}

function updateResultString() {
    // Obtener todos los campos de inclusión
    const includeFields = document.querySelectorAll(".dynamic-field");
    const includeValues = Array.from(includeFields)
        .map((input) => input.value.trim())
        .filter(value => value !== "");

    // Concatenar para result-string
    const includeConcatenacion = "+";
    const includeResult = includeValues.length > 0
        ? includeValues.map((value) => `${includeConcatenacion}"${value}"`).join(" ")
        : "";

    // Asignar a result-string
    document.getElementById("result-string").textContent = includeResult;

    // Obtener todos los campos de exclusión
    const excludeFields = document.querySelectorAll(".exclude-dynamic-field");
    const excludeValues = Array.from(excludeFields)
        .map(input => input.value.trim())
        .filter(value => value !== "");

    // Concatenar para exclude-result-string
    const excludeConcatenacion = "";
    const excludeResult =
        excludeValues.length > 0
            ? excludeValues.map((value) => `${excludeConcatenacion}"${value}"`).join(" ")
            : "";

    // Asignar a exclude-result-string
    document.getElementById("exclude-result-string").textContent = excludeResult;
}

function iniciarAutocomplete() {
    let selectedOption = document.querySelector('input[name="selection"]:checked').value;
    if (selectedOption === "Búsqueda Columnar") {
        if ($("#Search_text").data("ui-autocomplete")) {
            $("#Search_text").autocomplete("enable");
        } else {
            console.warn("Autocomplete no está inicializado.");
        }
        $("#Search_text").autocomplete({
            source: controlador + "Sugerir?highlights=false&fuzzy=true&searchField=NombreArchivoConExtension&",
            minLength: 3,
            position: {
                my: "left top",
                at: "left-23 bottom+10",
            },
            messages: {
                noResults: "No se encontraron resultados",
                results: function (amount) {
                    return "Quizá quiso decir o sugerencias encontradas: " + amount + " resultados disponibles, usa las teclas arriba y abajo para navegar.";
                }
            },
        });
    } else { $("#Search_text").autocomplete("disable"); }

    $("#Key_concepts").autocomplete({
        source: controlador + "Sugerir?highlights=false&fuzzy=true&searchField=Keyconcepts&",
        minLength: 3,
        position: {
            my: "left top",
            at: "left-23 bottom+10",
        },
    });
    $("#Key_word").autocomplete({
        source: controlador + "Sugerir?highlights=false&fuzzy=true&searchField=PalabrasClave&",
        minLength: 3,
        position: {
            my: "left top",
            at: "left-23 bottom+10",
        },
        select: function (event, ui) {
            this.value = ui.item.value;
            updateResultString();
        }
    });
    $("#Key_word_excluded").autocomplete({
        source: controlador + "Sugerir?highlights=false&fuzzy=true&searchField=PalabrasClave&",
        minLength: 3,
        position: {
            my: "left top",
            at: "left-23 bottom+10",
        },
        select: function (event, ui) {
            this.value = ui.item.value;
            updateResultString();
        }
    });
}

function manejarCambioBusqueda() {
    const selectedOption = document.querySelector('input[name="selection"]:checked').value;
    const tableContainer = document.getElementById("table-container");
    const ayudaContainer = document.getElementById("ayuda-container");
    const containerBC = document.getElementById("container-busqueda-conversacional");
    const listadoContainer = document.getElementById("listado");

    // Mostrar u ocultar la tabla según la opción seleccionada
    tableContainer.style.display = selectedOption === "Búsqueda Columnar" ? "block" : "none";
    ayudaContainer.style.display = selectedOption === "Búsqueda Columnar" ? "block" : "none";
    containerBC.style.display = selectedOption === "Búsqueda Columnar" ? "none" : "flex";
    listadoContainer.style.display = selectedOption === "Búsqueda Columnar" ? "block" : "none";

    iniciarAutocomplete();
}

function consultar() {
    // Obtener valores del formulario
    const selectedOption = document.querySelector('input[name="selection"]:checked').value;

    if (selectedOption === "Búsqueda Columnar") {
        realizarBusquedaColumnar();
    } else {
        realizarBusquedaConversacional();
    }
}

function limpiarChat() {
    $.ajax({
        type: "POST",
        url: controlador + "LimpiarChat",
        success: function (response) {
            if (response.Success === true) {
                mostrarMensaje("mensaje", "exito", response.Message);
                $("#chat-container").html("<strong>COES Search:</strong> Bienvenido, ¿en qué puedo ayudarte?");
            } else {
                mostrarMensaje("mensaje", "error", response.Message);
            }
        },
        error: function () {
            mostrarMensaje("mensaje", "error", "Ocurrió un error al realizar la limpieza. Intenta nuevamente.");
        },
    });
}

function realizarBusquedaColumnar() {
    const searchText = $("#Search_text").val() || "";
    const keyWords = document.getElementById("result-string").textContent.trim();
    const KeyWordsExcluded = document.getElementById("exclude-result-string").textContent.trim();
    const hora = "00:00";

    $.ajax({
        type: "POST",
        url: controlador + "Listar",
        data: {
            Search_text: searchText,
            Result_number: parseInt($("#Result_number").val()),
            Key_concepts: $("#Key_concepts").val(),
            Key_words: keyWords,
            Exclude_words: KeyWordsExcluded,
            Search_start_date: `${$("#txtFechaInicio").val()} ${hora}`,
            Search_end_date: `${$("#txtFechaFin").val()} ${hora}`,
            Tipo_documento: $("#TipoDocumento").val(),
            Search_type: true,
        },
        success: manejarRespuestaBusqueda,
        error: manejarErrorBusqueda
    });
}

function realizarBusquedaConversacional() {
    const searchText = $("#Search_text").val() || "";
    if (!searchText.trim()) return;

    $.ajax({
        type: "POST",
        url: controlador + "BusquedaConversacional",
        data: {
            busqueda: searchText,
            Search_type: false,
        },
        success: function (data) {
            let chatContainer = $("#chat-container");
            chatContainer.append("<p><strong>Tú:</strong> " + searchText + "</p>");
            chatContainer.append("<p><strong>COES Search:</strong> " + data.response + "</p>");

            // Limpiar la lista de referencias anteriores
            let citationsList = document.getElementById("citationsList");
            citationsList.innerHTML = "";

            // Generar las nuevas referencias como enlaces
            if (Array.isArray(data.references) && data.references.length > 0) {
                data.references.forEach(function (item, index) {
                    let listItem = document.createElement("li");
                    let link = document.createElement("a");
                    link.href = "#";
                    link.textContent = `[doc${index + 1}] ${item.Titulo}`;
                    link.onclick = function () {
                        showCitationDetail(item);
                    };

                    listItem.appendChild(link);
                    citationsList.appendChild(listItem);
                });
            }
            chatContainer.scrollTop($("#chat-container")[0].scrollHeight);
            $("#Search_text").val("");
        },
        error: manejarErrorBusqueda,
    });
}

function showCitationDetail(item) {
    // Muestra el título y contenido de la referencia seleccionada
    document.getElementById("referenceTitle").innerText = item.Titulo;
    document.getElementById("referenceContent").innerText = item.Content;
    document.getElementById("referenceUrl").innerText = item.Url;
}

function manejarRespuestaBusqueda(response) {
    $("#listado").html(response);
    $("#tabla").dataTable({
        columns: [null, null, null, null, null, null, null, { visible: false }],
        iDisplayLength: 25,
        order: [[0, "desc"]],
    });
}

function manejarErrorBusqueda() {
    mostrarMensaje("mensaje", "error", "Ocurrió un error al realizar la búsqueda. Intenta nuevamente.");
}

function inicializarCamposDinamicos(containerId, buttonId, fieldClass) {
    const fieldsContainer = document.getElementById(containerId);
    const addFieldBtn = document.getElementById(buttonId);

    addFieldBtn.addEventListener("click", (event) => {
        event.preventDefault();

        const fieldContainer = document.createElement("div");
        fieldContainer.className = "field-container";

        const inputField = document.createElement("input");
        inputField.type = "text";
        inputField.className = fieldClass;
        inputField.placeholder = `${fieldClass.includes("exclude") ? "Excluir palabra clave" : "Agregar palabra clave"}`;
        inputField.style = "width: 80%; height: auto;";
        inputField.addEventListener("input", updateResultString);
        $(inputField).autocomplete({
            source: controlador + "Sugerir?highlights=false&fuzzy=true&searchField=PalabrasClave&",
            minLength: 3,
            position: {
                my: "left top",
                at: "left-23 bottom+10",
            },
            select: function (event, ui) {
                // Actualiza el valor del input con el valor seleccionado
                this.value = ui.item.value;
                // Actualiza el resultString
                updateResultString();
            },
        });

        const removeBtn = document.createElement("button");
        removeBtn.textContent = "🗑️";
        removeBtn.className = "remove-btn";
        removeBtn.style = "width: 16%; height: auto;";
        removeBtn.addEventListener("click", (event) => {
            event.preventDefault();
            fieldContainer.remove();
            updateResultString();
        });

        fieldContainer.appendChild(inputField);
        fieldContainer.appendChild(removeBtn);
        fieldsContainer.appendChild(fieldContainer);
    });

    // Inicializar el primer campo para que actualice la cadena
    const firstField = fieldsContainer.querySelector(`.${fieldClass}`);
    if (firstField) {
        firstField.addEventListener("input", updateResultString);
    }
}

function mostrarMensaje(id, tipo, mensaje) {
    let mensajeElemento = $("#" + id);

    mensajeElemento.removeClass("action-success action-error action-warning");
    mensajeElemento.attr("class", "");
    mensajeElemento.addClass("action-" + tipo);
    mensajeElemento.html(mensaje);

    mensajeElemento.show();

    setTimeout(() => {
        mensajeElemento.fadeOut("slow", () => {
            mensajeElemento.html("");
            mensajeElemento.removeClass("action-exito action-error action-warning");
        });
    }, 5000);
}

function toggleAcordeon() {
    const contenido = document.getElementById("contenidoAcordeon");
    contenido.classList.toggle("mostrar");
}
