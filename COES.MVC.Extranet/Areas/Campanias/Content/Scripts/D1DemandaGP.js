var formatoAValor = 40;
var formatoBValor = 41;
var formatoCValor = 42;
var formatoDValor = 43;
var hot1 = null;
var hot2 = null;
var hot3 = null;
var hot4 = null;
var hot5 = null;
var hot6 = null;
var hot7 = null;


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
            if (typeof cargarDFormatoA !== 'undefined' &&
                typeof cargarDFormatoB !== 'undefined' &&
                typeof cargarDFormatoC !== 'undefined' &&
                typeof cargarFormatoD !== 'undefined') {
                clearInterval(interval);
                resolve();
            }
        }, 100); // Revisa cada 100ms
    });
}


function mostrarFichasFiltradas() {
    $("#tabFormatoD1A").hide();
    $("#tabFormatoD1B").hide();
    $("#tabFormatoD1C").hide();
    $("#tabFormatoD1D").hide();
    $("#tabfichad").hide();
    const fichasValores = {
        FormatoD1A: formatoAValor,
        FormatoD1B: formatoBValor,
        FormatoD1C: formatoCValor,
        FormatoD1D: formatoDValor,
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

    if (tipoFicha === "formatoA") {
        $("#DGPFormatoD1A").show();
        formularioSeleccionado = "DGPFormatoD1A";
        cargarDFormatoA();
        highlightTab(tabelement, 'tab-container-2');
    }
    else if (tipoFicha === "formatoB") {
        $("#DGPFormatoD1B").show();
        formularioSeleccionado = "DGPFormatoD1B";
        cargarDFormatoB();
        highlightTab(tabelement, 'tab-container-2');
    }
    else if (tipoFicha === "formatoC") {
        $("#DGPFormatoD1C").show();
        formularioSeleccionado = "DGPFormatoD1C";
        cargarDFormatoC();
        highlightTab(tabelement, 'tab-container-2');
    }
    else if (tipoFicha === "formatoD") {
        $("#DGPFormatoD1D").show();
        formularioSeleccionado = "DGPFormatoD1D";
        cargarFormatoD();
        highlightTab(tabelement, 'tab-container-2');
    }
    else if (tipoFicha === "fichaadic") {
        $("#FichaAdi").show();
        cargarAdicionalD1();
        highlightTab(tabelement, 'tab-container-2');
    }
}

// Validar los campos para aceptar solo n�meros positivos
document.querySelectorAll('.txtnumber').forEach(input => {
    input.addEventListener('input', function () {
        // Remover cualquier car�cter que no sea un n�mero o un punto decimal
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

        // Si el valor es negativo, establecerlo en vac�o
        if (parseFloat(this.value) < 0) {
            this.value = '';
        }
    });
});

document.querySelectorAll('.txtinteger').forEach(input => {
    input.addEventListener('input', function () {
        // Permitir solo d�gitos num�ricos
        this.value = this.value.replace(/[^0-9]/g, '');

        // Remover los ceros iniciales
        if (this.value.length > 1 && this.value[0] === '0') {
            this.value = this.value.substring(1);
        }

        // Si el valor es negativo, establecerlo en vac�o (si se desea evitar negativos)
        if (this.value < 0) {
            this.value = '';
        }
    });
});
