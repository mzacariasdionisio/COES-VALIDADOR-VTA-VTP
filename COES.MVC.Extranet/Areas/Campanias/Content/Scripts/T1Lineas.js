var controlador = siteRoot + "campanias/plantransmision/";
var controladorFichas = siteRoot + "campanias/fichasproyecto/";

var fichaAValor = 20;
var fichaBValor = 21;


$(document).ready(function () {
    // Esperar que los scripts adicionales se carguen
    verificarCargaCompleta().then(() => {
        console.log("Todas las dependencias están listas");
        mostrarFichasFiltradasLineasA(); // Lógica de mostrar las pestañas
        $(".ficha").hide(); // Oculta todas las fichas inicialmente

        // Simula un clic en la primera ficha para mostrarla
        $('#tabs_container li:first-child a').click();
    });
});

function verificarCargaCompleta() {
    return new Promise((resolve) => {
        const interval = setInterval(() => {
            // Verificar que todas las funciones necesarias estén disponibles
            if (typeof cargarFichaA !== 'undefined') {
                clearInterval(interval);
                resolve();
            }
        }, 100); // Revisa cada 100ms
    });
}


function mostrarFichasFiltradasLineasA() {
    $("#tabfichaa").hide();
    $("#tabfichab").hide();
    const fichasValores = {
        fichaa: fichaAValor,
        fichab: fichaBValor,
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
    if (tipoFicha === "fichaa") {
        $("#TLFichaA").show();
        formularioSeleccionado = "TLFichaA";
        cargarFichaA();
        highlightTab(tabelement, 'tab-container-2');
    }  else if (tipoFicha === "fichaadic") {
        $("#FichaAdi").show();
        cargarAdicionalTLin();
        highlightTab(tabelement, 'tab-container-2');
    }
}

$(document).ready(function () {
    // Validar txtnumber (decimales)
    $('.txtnumber').on('input', function () {
        let valor = $(this).val();
        // Permitir solo n�meros y un punto decimal
        valor = valor.replace(/[^0-9.]/g, '').replace(/(\..*?)\..*/g, '$1');

        // Limitar a dos decimales si existe un punto
        if (valor.includes('.')) {
            const partes = valor.split('.');
            if (partes[1] && partes[1].length > 2) {
                valor = `${partes[0]}.${partes[1].substring(0, 2)}`;
            }
        }
        $(this).val(valor);
    });

    // Validar txtinteger (enteros)
    $('.txtinteger').on('input', function () {
        // Permitir solo d�gitos num�ricos
        let valor = $(this).val();
        valor = valor.replace(/[^0-9]/g, '');
        $(this).val(valor);
    });
});
