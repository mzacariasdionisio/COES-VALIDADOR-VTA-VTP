var controlador = siteRoot + 'campanias/plantransmision/';
var controladorFichas = siteRoot + 'campanias/fichasproyecto/';
var catPropietario = 2;
var catConcesionTemporal = 3;
var catConcesionActual = 4;
var fichaAValor = 14;
var fichaBValor = 15;
var fichaCValor = 16;



$(document).ready(function () {
    // Esperar que los scripts adicionales se carguen
    verificarCargaCompleta().then(() => {
        console.log("Todas las dependencias están listas");
        mostrarFichasFiltradasBiomasa(); // Lógica de mostrar las pestañas
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


function mostrarFichasFiltradasBiomasa() {
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
    if (tipoFicha === "fichaa") {
        $("#BFichaA").show();
        formularioSeleccionado = "BFichaA";
        cargarDatosA();
        highlightTab(tabelement, 'tab-container-2');
    } else if (tipoFicha === "fichab") {
        $("#BFichaB").show();
        formularioSeleccionado = "BFichaB";
        cargarDatosB();
        highlightTab(tabelement, 'tab-container-2');
    } else if (tipoFicha === "fichac") {
        $("#BFichaC").show();
        formularioSeleccionado = "BFichaC";
        cargarDatosC();
        highlightTab(tabelement, 'tab-container-2');
    } else if (tipoFicha === "fichaadic") {
        $("#FichaAdi").show();
        cargarAdicionalBio();
        highlightTab(tabelement, 'tab-container-2');
    }

}

// Validar los campos para aceptar solo números positivos
document.querySelectorAll('.txtnumber').forEach(input => {
    input.addEventListener('input', function () {
        // Remover cualquier carácter que no sea un número o un punto decimal
        this.value = this.value.replace(/[^0-9.]/g, '');

        // Permitir solo un punto decimal
        const parts = this.value.split('.');
        if (parts.length > 2) {
            this.value = parts[0] + '.' + parts.slice(1).join('');
        }

        // Remover los ceros iniciales
        if (this.value.length > 1 && this.value[0] === '0' && this.value[1] !== '.') {
            this.value = this.value.substring(1);
        }

        // Si el valor es negativo, establecerlo en vacío
        if (parseFloat(this.value) < 0) {
            this.value = '';
        }
    });
});

document.querySelectorAll('.txtinteger').forEach(input => {
    input.addEventListener('input', function () {
        // Permitir solo dígitos numéricos
        this.value = this.value.replace(/[^0-9]/g, '');

        // Remover los ceros iniciales
        if (this.value.length > 1 && this.value[0] === '0') {
            this.value = this.value.substring(1);
        }

        // Si el valor es negativo, establecerlo en vacío (si se desea evitar negativos)
        if (this.value < 0) {
            this.value = '';
        }
    });
});

