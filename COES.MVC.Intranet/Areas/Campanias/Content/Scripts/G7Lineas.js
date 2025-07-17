var controlador = siteRoot + 'campanias/plantransmision/';
var controladorFichas = siteRoot + 'campanias/fichasproyecto/';


var fichaAValor = 18;
var fichaBValor = 19;

$(function () {
    mostrarFichasFiltradasLineasA();
    $(".ficha").hide();
});

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
        $("#GLFichaA").show();
        formularioSeleccionado = "GLFichaA";
        cargarFichaA();
        highlightTab(tabelement, 'tab-container-2');
    } else if (tipoFicha === "fichab") {
        $("#GLFichaB").show();
        formularioSeleccionado = "GLFichaB";
        cargarFichaB();
        highlightTab(tabelement, 'tab-container-2');
    } else if (tipoFicha === "fichaadic") {
        $("#FichaAdi").show();
        cargarAdicionalLin();
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