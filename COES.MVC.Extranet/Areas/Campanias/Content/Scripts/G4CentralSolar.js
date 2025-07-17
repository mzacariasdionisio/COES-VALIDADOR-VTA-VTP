var controlador = siteRoot + 'campanias/plantransmision/';
var controladorFichas = siteRoot + 'campanias/fichasproyecto/';
var catPropietario = 2;
var catConcesionTemporal = 3;
var catConcesionActual = 4;

var fichaAValor = 11;
var fichaBValor = 12;
var fichaCValor = 13;

$(document).ready(function () {
    // Esperar que los scripts adicionales se carguen
    verificarCargaCompleta().then(() => {
        console.log("Todas las dependencias están listas");
        mostrarFichasFiltradasSolar(); // Lógica de mostrar las pestañas
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
                typeof cargarDatosC !== 'undefined') {
                clearInterval(interval);
                resolve();
            }
        }, 100); // Revisa cada 100ms
    });
}

function mostrarFichasFiltradasSolar() {
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
   
    if (cambiosRealizados===true) {
        popupGuardadoAutomatico();
        cambiosRealizados = false;
        return;
    }
    $(".ficha").hide();
    // Mostrar la ficha correspondiente
    if (tipoFicha === "fichaa") {
        $("#FichaA").show(); 
        cargarDatosA();
        highlightTab(tabelement, 'tab-container-2');
        formularioSeleccionado = "G4FichaA";
        
    } else if (tipoFicha === "fichab") {
        $("#FichaB").show();
        cargarDatosB();
        highlightTab(tabelement, 'tab-container-2');
        formularioSeleccionado = "G4FichaB";

    } else if (tipoFicha === "fichac") {
        $("#FichaC").show();
        cargarDatosC();
        highlightTab(tabelement, 'tab-container-2');
        formularioSeleccionado = "G4FichaC";

    } else if (tipoFicha === "fichaadic") {
        $("#FichaAdi").show();
        highlightTab(tabelement, 'tab-container-2');
        cargarAdicionalSol();
       
   
    }
}

