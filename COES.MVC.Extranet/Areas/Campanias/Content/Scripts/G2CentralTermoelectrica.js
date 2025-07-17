var catPropietario = 2;
var catConcesionTemporal = 3;
var catConcesionActual = 4;
var catCombustible = 34;

var catSubEstacion = 15;
var catPerfil = 16;
var catPreFacltibilidad= 17;
var catFacltibilidad = 18;
var catEstudDef = 19;
var catEia = 20;


var fichaAValor = 5;
var fichaBValor = 6;
var fichaCValor = 7;

$(document).ready(function () {
    // Esperar que los scripts adicionales se carguen
    verificarCargaCompleta().then(() => {
        console.log("Todas las dependencias están listas");
        mostrarFichasFiltradasTermo(); // Lógica de mostrar las pestañas
        $(".ficha").hide(); // Oculta todas las fichas inicialmente

        // Simula un clic en la primera ficha para mostrarla
        $('#tabs_container li:first-child a').click();
    });
});

function verificarCargaCompleta() {
    return new Promise((resolve) => {
        const interval = setInterval(() => {
            // Verificar que todas las funciones necesarias estén disponibles
            if (typeof cargarDatosA !== 'undefined' &&
                typeof cargarDatosB !== 'undefined' &&
                typeof cargarDatosC !== 'undefined' ) {
                clearInterval(interval);
                resolve();
            }
        }, 100); // Revisa cada 100ms
    });
}

function mostrarFichasFiltradasTermo() {
    $("#tabfichaa").hide();
    $("#tabfichab").hide();
    $("#tabfichac").hide();
    const fichasValores = {
        fichaa: fichaAValor,
        fichab: fichaBValor,
        fichac: fichaCValor
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
    if (tipoFicha === "fichaa") {
        $("#TFichaA").show();
        formularioSeleccionado = "TFichaA";
        cargarDatosA();
        highlightTab(tabelement, 'tab-container-2');
    } else if (tipoFicha === "fichab") {
        $("#TFichaB").show();
        formularioSeleccionado = "TFichaB";
        cargarDatosB();
        highlightTab(tabelement, 'tab-container-2');
    } else if (tipoFicha === "fichac") {
        $("#TFichaC").show();
        formularioSeleccionado = "TFichaC";
        cargarDatosC();
        highlightTab(tabelement, 'tab-container-2');
    } else if (tipoFicha === "fichaadic") {
        $("#FichaAdi").show();
        highlightTab(tabelement, 'tab-container-2');
        cargarAdicionalCCTT(); 
    }
}



