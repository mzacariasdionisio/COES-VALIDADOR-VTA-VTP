var CCGDAValor = 44;
var CCGDBValor = 45;
var CCGDCValor = 46;
var CCGDDValor = 47;
var CCGDEValor = 48;
var CCGDFValor = 49;
var hot1 = null;
var hot2 = null;
var hot3 = null;
var hot4 = null;

$(document).ready(function () {
    // Esperar que los scripts adicionales se carguen
    verificarCargaCompleta().then(() => {
        console.log("Todas las dependencias están listas");
        mostrarFichasFiltradas(); // Lógica de mostrar las pestañas
        $(".ficha").hide(); // Oculta todas las fichas inicialmente

        // Simula un clic en la primera ficha para mostrarla
        $('#tabs_container li:first-child a').click();
    });
});

function verificarCargaCompleta() {
    return new Promise((resolve) => {
        const interval = setInterval(() => {
            // Verificar que todas las funciones necesarias estén disponibles
            if (typeof cargarCCGDA !== 'undefined' && typeof cargarCCGDB !== 'undefined' &&
                typeof cargarCCGDC !== 'undefined' && typeof cargarCCGDD !== 'undefined' &&
                typeof cargarCCGDE !== 'undefined' && typeof cargarCCGDF !== 'undefined') {
                clearInterval(interval);
                resolve();
            }
        }, 100); // Revisa cada 100ms
    });
}


function mostrarFichasFiltradas() {
    // Ocultar todas las pestañas al inicio
    $("#tabCCGDA, #tabCCGDB, #tabCCGDC, #tabCCGDD, #tabCCGDE, #tabCCGDF").hide();

    const fichasValores = {
        CCGDA: CCGDAValor,
        CCGDB: CCGDBValor,
        CCGDC: CCGDCValor,
        CCGDD: CCGDDValor,
        CCGDE: CCGDEValor,
        CCGDF: CCGDFValor,
    };

    Object.keys(fichasValores).forEach((ficha) => {
        const fichaDisponible =
            hojasRegistrar.find((valor) => valor === fichasValores[ficha]) ?? null;
        $(`#tab${ficha}`).toggle(fichaDisponible !== null);
    });
}

function openFicha(tipoFicha, tabelement) {
    limpiarMensaje('mensajeFicha');
    if (cambiosRealizados) {
        popupGuardadoAutomatico();
        cambiosRealizados = false;
        return;
    }
    $(".ficha").hide();

    // Mostrar la ficha correspondiente
    if (tipoFicha === "CCGDA") {
        $("#CCGDA").show();
        formularioSeleccionado = "CCGDA";
        cargarCCGDA();
        highlightTab(tabelement, 'tab-container-2');
    } else if (tipoFicha === "CCGDB") {
        $("#CCGDB").show();
        formularioSeleccionado = "CCGDB";
        cargarCCGDB();
        highlightTab(tabelement, 'tab-container-2');
    }
    else if (tipoFicha === "CCGDC") {
        $("#CCGDC").show();
        formularioSeleccionado = "CCGDC";
        cargarCCGDC();
        highlightTab(tabelement, 'tab-container-2');
    }
    else if (tipoFicha === "CCGDD") {
        $("#CCGDD").show();
        formularioSeleccionado = "CCGDD";
        cargarCCGDD();
        highlightTab(tabelement, 'tab-container-2');
    }
    else if (tipoFicha === "CCGDE") {
        $("#CCGDE").show();
        formularioSeleccionado = "CCGDE";
        cargarCCGDE();
        highlightTab(tabelement, 'tab-container-2');
    }
    else if (tipoFicha === "CCGDF") {
        $("#CCGDF").show();
        formularioSeleccionado = "CCGDF";
        cargarCCGDF();
        highlightTab(tabelement, 'tab-container-2');
    }
    else if (tipoFicha === "fichaadic") {
        $("#FichaAdi").show();
        cargarAdicionalCCGD();
        highlightTab(tabelement, 'tab-container-2');
    }
}
