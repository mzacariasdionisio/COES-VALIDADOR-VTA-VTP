var controlador = siteRoot + 'campanias/plantransmision/';
var controladorFichas = siteRoot + 'campanias/fichasproyecto/';
var catPropietario = 2;
var catConcesionTemporal = 3;
var catConcesionActual = 4;

var ficha1Valor = 56;

$(document).ready(function () {
    // Esperar que los scripts adicionales se carguen
    verificarCargaCompleta().then(() => {
        console.log("Todas las dependencias están listas");
        mostrarFichasFiltradasCronograma(); // Lógica de mostrar las pestañas
        $(".ficha").hide(); // Oculta todas las fichas inicialmente

        // Simula un clic en la primera ficha para mostrarla
        $('#tabs_container li:first-child a').click();
    });
});

function verificarCargaCompleta() {
    return new Promise((resolve) => {
        const interval = setInterval(() => {
            // Verificar que todas las funciones necesarias estén disponibles
            if (typeof cargarCroFicha1 !== 'undefined') {
                clearInterval(interval);
                resolve();
            }
        }, 100); // Revisa cada 100ms
    });
}


function mostrarFichasFiltradasCronograma() {
    $("#tabficha1").hide();
    const fichasValores = {
        ficha1: ficha1Valor,
    };

    Object.keys(fichasValores).forEach((ficha) => {
        const fichaDisponible =
            hojasRegistrar.find((valor) => valor === fichasValores[ficha]) ?? null;
        if (fichaDisponible !== null) {
            $(`#tab${ficha}`).show();
        } else {
            $(`#tab${ficha}`).hide();
        }
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
    if (tipoFicha === "ficha1") {
        $("#FichaT3A").show();
        formularioSeleccionado = "FichaT3A";
        cargarCroFicha1(); 
        highlightTab(tabelement, 'tab-container-2');
    } else if (tipoFicha === "fichaadic") {
        $("#FichaAdi").show();
        cargarAdicionalCro();
        highlightTab(tabelement, 'tab-container-2');
    }
}

