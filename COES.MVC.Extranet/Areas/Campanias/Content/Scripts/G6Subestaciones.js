var controlador = siteRoot + 'campanias/plantransmision/';
var controladorFichas = siteRoot + 'campanias/fichasproyecto/';
var sistBarras = 37;

var fichaAValor = 17;

$(function () {
    mostrarFichasFiltradaSubestaciones();
    $(".ficha").hide();
    cargarCatalogo(sistBarras, "#sistemaBarrasSelect");
});

function mostrarFichasFiltradaSubestaciones() {
    $("#tabfichaa").hide();
    const fichasValores = {
        fichaa: fichaAValor,
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
    if (tipoFicha === "fichaa") {
        $("#GSFichaA").show();
        formularioSeleccionado = "GSFichaA";
        cargarDatosSubA();
        highlightTab(tabelement, 'tab-container-2');
    } else if (tipoFicha === "fichaadic") {
        $("#FichaAdi").show();
        cargarAdicionalSub();
        highlightTab(tabelement, 'tab-container-2');
    }
}