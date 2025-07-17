var controlador = siteRoot + 'campanias/plantransmision/';
var controladorFichas = siteRoot + 'campanias/fichasproyecto/';

var fPrm1Valor = 33;
var fPrm2Valor = 34;
var fRed1Valor = 35;
var fRed2Valor = 36;
var fRed3Valor = 27;
var fRed4Valor = 38;
var fRed5Valor = 39;


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
            if (typeof cargarDatosPrm1 !== 'undefined' &&
                typeof cargarDatosPrm2 !== 'undefined' &&
                typeof cargarDatosRed1 !== 'undefined' &&
                typeof cargarDatosRed2 !== 'undefined' &&
                typeof cargarDatosRed3 !== 'undefined' &&
                typeof cargarDatosRed4 !== 'undefined' &&
                typeof cargarDatosRed5 !== 'undefined') {
                clearInterval(interval);
                resolve();
            }
        }, 100); // Revisa cada 100ms
    });
}



function mostrarFichasFiltradasSolar() {
    $("#tabfprm1").hide();
    $("#tabfprm2").hide();
    $("#tabfred1").hide();
    $("#tabfred2").hide();
    $("#tabfred3").hide();
    $("#tabfred4").hide();
    $("#tabfred5").hide();
    $("#FichaAdi").hide();

    const fichasValores = {
        fprm1: fPrm1Valor,
        fprm2: fPrm2Valor,
        fred1: fRed1Valor,
        fred2: fRed2Valor,
        fred3: fRed3Valor,
        fred4: fRed4Valor,
        fred5: fRed5Valor
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

    if (tipoFicha === "fprm1") {
        $("#Fichaprm1").show();
        formularioSeleccionado = "fprm1";
        cargarDatosPrm1();
    } else if (tipoFicha === "fprm2") {
        $("#Fichaprm2").show();
        formularioSeleccionado = "fprm2";
        cargarDatosPrm2();
    } else if (tipoFicha === "fred1") {
        $("#Fichared1").show();
        formularioSeleccionado = "fred1";
        cargarDatosRed1();
    } else if (tipoFicha === "fred2") {
        $("#Fichared2").show();
        formularioSeleccionado = "fred2";
        cargarDatosRed2();
    } else if (tipoFicha === "fred3") {
        $("#Fichared3").show();
        formularioSeleccionado = "fred3";
        cargarDatosRed3();
    } else if (tipoFicha === "fred4") {
        $("#Fichared4").show();
        formularioSeleccionado = "fred4";
        cargarDatosRed4();
    } else if (tipoFicha === "fred5") {
        $("#Fichared5").show();
        formularioSeleccionado = "fred5";
        cargarDatosRed5();
    } else if (tipoFicha === "fichaadic") {
        $("#FichaAdi").show();
        highlightTab(tabelement, 'tab-container-2');
        cargarAdicionalLin();
    }

    highlightTab(tabelement, 'tab-container-2');
}

