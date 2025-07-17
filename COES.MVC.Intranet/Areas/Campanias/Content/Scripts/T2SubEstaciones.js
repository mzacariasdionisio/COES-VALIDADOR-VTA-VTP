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
    $(".ficha").hide();
    if (tipoFicha === "fichaa") {        
        $("#FichaT2A").show();
        formularioSeleccionado = "FichaT2A";
        cargarDatosT2SubA();
        highlightTab(tabelement, 'tab-container-2');
    } else if (tipoFicha === "fichaadic") {
        $("#FichaAdi").show();
        cargarAdicionalTSub();
        highlightTab(tabelement, 'tab-container-2');
    }
}