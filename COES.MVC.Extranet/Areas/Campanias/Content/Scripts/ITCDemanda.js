var controlador = siteRoot + 'campanias/plantransmision/';
var controladorFichas = siteRoot + 'campanias/fichasproyecto/';
var catPropietario = 2;
var catConcesionTemporal = 3;
var catConcesionActual = 4;

var f104Valor = 23;
var f108Valor = 24;
var fp101Valor = 25;
var fp102Valor = 26;
var fp103Valor = 27;
var f110Valor = 28;
var f116Valor = 29;
var f121Valor = 30;
var f123Valor = 31;
var fe01Valor = 32;


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
            if (typeof cargarDatos104 !== 'undefined' &&
                typeof cargarDatos108 !== 'undefined' &&
                typeof cargarDatosFP011 !== 'undefined' &&
                typeof cargarDatos012 !== 'undefined' &&
                typeof cargarDatos013 !== 'undefined' &&
                typeof cargarDatos110 !== 'undefined' &&
                typeof cargarDatos116 !== 'undefined' &&
                typeof cargarDatos121 !== 'undefined' &&
                typeof cargarDatos123 !== 'undefined' &&
                typeof cargarFe01 !== 'undefined') {
                clearInterval(interval);
                resolve();
            }
        }, 100); // Revisa cada 100ms
    });
}


function mostrarFichasFiltradasSolar() {
    $("#f104tab").hide();
    $("#f108tab").hide();
    $("#fp101tab").hide();
    $("#fp102tab").hide();
    $("#fp103tab").hide();
    $("#f110tab").hide();
    $("#f116tab").hide();
    $("#f121tab").hide();
    $("#f123tab").hide();
    $("#fe01tab").hide();


    const fichasValores = {
        f104: f104Valor,
        f108: f108Valor,
        fp101: fp101Valor,
        fp102: fp102Valor,
        fp103: fp103Valor,
        f110: f110Valor,
        f116: f116Valor,
        f121: f121Valor,
        f123: f123Valor,
        fe01: fe01Valor
    };

    Object.keys(fichasValores).forEach((ficha) => {
        const fichaDisponible =
            hojasRegistrar.find((valor) => valor === fichasValores[ficha]) ?? null;
        $(`#${ficha}tab`).toggle(fichaDisponible !== null);
    });
}

function openFicha(tipoFicha, tabelement) {
    limpiarMensaje('mensajeFicha');
    if (cambiosRealizados==true) {
        popupGuardadoAutomatico();
        cambiosRealizados = false;
        return;
    }
    $(".ficha").hide();
    // Mostrar la ficha correspondiente
    if (tipoFicha === "f104") {
        $("#f104").show();
        formularioSeleccionado = "f104";
        cargarDatos104();
        highlightTab(tabelement, 'tab-container-2');
    } else if (tipoFicha === "f108") {
        console.log('container f108',$("#f108"));
        $("#f108").show();
        formularioSeleccionado = "f108";
        cargarDatos108();
        highlightTab(tabelement, 'tab-container-2');
    } else if (tipoFicha === "fp101") {
        $("#fp101").show();
        formularioSeleccionado = "fp101";
        cargarDatosFP011();
        highlightTab(tabelement, 'tab-container-2');
    } else if (tipoFicha === "fp102") {
        $("#fp102").show();
        formularioSeleccionado = "fp102";
        cargarDatos012();
        highlightTab(tabelement, 'tab-container-2');
    } else if (tipoFicha === "fp103") {
        $("#fp103").show();
        formularioSeleccionado = "fp103";
        cargarDatos013();
        highlightTab(tabelement, 'tab-container-2');
    } else if (tipoFicha === "f110") {
        $("#f110").show();
        formularioSeleccionado = "f110";
        cargarDatos110();
        highlightTab(tabelement, 'tab-container-2');
    } else if (tipoFicha === "f116") {
        $("#f116").show();
        formularioSeleccionado = "f116";
        cargarDatos116();
        highlightTab(tabelement, 'tab-container-2');
    } else if (tipoFicha === "f121") {
        $("#f121").show();
        formularioSeleccionado = "f121";
        cargarDatos121();
        highlightTab(tabelement, 'tab-container-2');
    } else if (tipoFicha === "f123") {
        $("#f123").show();
        formularioSeleccionado = "f123";
        cargarDatos123();
        highlightTab(tabelement, 'tab-container-2');
    } else if (tipoFicha === "fe01") {
        $("#fe01").show();
        formularioSeleccionado = "fe01";
        cargarFe01();
        highlightTab(tabelement, 'tab-container-2');
    } else if (tipoFicha === "fichaadic") {
        $("#FichaAdi").show();
        cargarAdicionalLin();
        highlightTab(tabelement, 'tab-container-2');
    }
}

