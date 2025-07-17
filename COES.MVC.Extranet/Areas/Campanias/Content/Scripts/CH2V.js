var H2VAValor = 50;
var H2VBValor = 51;
var H2VCValor = 52;
var H2VGValor = 53;
var H2VEValor = 54;
var H2VFValor = 55;
var HVADValor = 41;

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
            if (typeof cargarDatosHojaA !== 'undefined' &&
                typeof cargarDatosHojaB !== 'undefined' &&
                typeof cargarDatosC !== 'undefined' &&
                typeof cargarCH2VG !== 'undefined' &&
                typeof cargarCH2VE !== 'undefined' &&
                typeof cargarCH2VF !== 'undefined') {
                clearInterval(interval);
                resolve();
            }
        }, 100); // Revisa cada 100ms
    });
}

function mostrarFichasFiltradas() {
    $("#tabH2VA").hide();
    $("#tabH2VB").hide();
    $("#tabH2VC").hide();
    $("#tabH2VG").hide();
    $("#tabH2VE").hide();
    $("#tabH2VF").hide();
    $("#tabHVAD").hide();
    const fichasValores = {
        H2VA: H2VAValor,
        H2VB: H2VBValor,
        H2VC: H2VCValor,
        H2VG: H2VGValor,
        H2VE: H2VEValor,
        H2VF: H2VFValor,
        HVAD: HVADValor,
    };

    Object.keys(fichasValores).forEach((ficha) => {
        const fichaDisponible =
            hojasRegistrar.find((valor) => valor === fichasValores[ficha]) ?? null;
        $(`#tab${ficha}`).toggle(fichaDisponible !== null);
    });
}

function openFicha(tipoFicha, tabelement) {
    limpiarMensaje('mensajeFicha');
    if (cambiosRealizados === true) {
        popupGuardadoAutomatico();
        cambiosRealizados = false;
        return;
    }
    $(".ficha").hide();
    if (tipoFicha === "H2VA") {
        $("#H2VA").show();
        formularioSeleccionado = "H2VA";
        cargarDatosHojaA();
        highlightTab(tabelement, 'tab-container-2');
    } else if (tipoFicha === "H2VB") {
        $("#H2VB").show();
        formularioSeleccionado = "H2VB";
        cargarDatosHojaB();
        highlightTab(tabelement, 'tab-container-2');
    }
    else if (tipoFicha === "H2VC") {
        $("#H2VC").show();
        formularioSeleccionado = "H2VC";
        cargarDatosC();
        highlightTab(tabelement, 'tab-container-2');
    }
    else if (tipoFicha === "H2VG") {
        $("#H2VG").show();
        formularioSeleccionado = "H2VG";
        cargarCH2VG();
        highlightTab(tabelement, 'tab-container-2');
    }
    else if (tipoFicha === "H2VE") {
        $("#H2VE").show();
        formularioSeleccionado = "H2VE";
        cargarCH2VE();
        highlightTab(tabelement, 'tab-container-2');
    }
    else if (tipoFicha === "H2VF") {
        $("#H2VF").show();
        formularioSeleccionado = "H2VF";
        cargarCH2VF();
        highlightTab(tabelement, 'tab-container-2');

    } else if (tipoFicha === "HVAD") {
        $("#HVAD").show();
        formularioSeleccionado = "HVAD";
        cargarAdicionalCHV();
        highlightTab(tabelement, 'tab-container-2');
    }
    
}




// txtnumber Full Validar los campos para aceptar solo n�meros positivos con m�ximo 4 decimales
document.querySelectorAll('.txtnumber').forEach(input => {
    input.addEventListener('input', function () {
        // Remover cualquier car�cter que no sea un n�mero o un punto decimal
        this.value = this.value.replace(/[^0-9.]/g, '');

        // Permitir solo un punto decimal
        const parts = this.value.split('.');
        if (parts.length > 2) {
            this.value = parts[0] + '.' + parts.slice(1).join('');
        }

        // Limitar a un m�ximo de 4 decimales
        if (parts.length === 2 && parts[1].length > 4) {
            this.value = parts[0] + '.' + parts[1].substring(0, 4);
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

    input.addEventListener('paste', function (event) {
        // Obtener el contenido del portapapeles
        let pasteData = (event.clipboardData || window.clipboardData).getData('text');

        // Remover cualquier car�cter que no sea un n�mero o un punto decimal
        pasteData = pasteData.replace(/[^0-9.]/g, '');

        // Permitir solo un punto decimal
        const parts = pasteData.split('.');
        if (parts.length > 2) {
            pasteData = parts[0] + '.' + parts.slice(1).join('');
        }

        // Limitar a un m�ximo de 4 decimales
        if (parts.length === 2 && parts[1].length > 4) {
            pasteData = parts[0] + '.' + parts[1].substring(0, 4);
        }

        // Asignar el valor filtrado
        this.value = pasteData;

        // Prevenir el comportamiento por defecto de pegado
        event.preventDefault();
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

document.querySelectorAll('.txtanio').forEach(input => {
    input.addEventListener('input', function () {
        // Permitir solo dígitos numéricos
        this.value = this.value.replace(/[^0-9]/g, '');

        // Remover los ceros iniciales
        if (this.value.length > 1 && this.value[0] === '0') {
            this.value = this.value.substring(1);
        }

        // Limitar a un máximo de 4 dígitos
        if (this.value.length > 4) {
            this.value = this.value.substring(0, 4);
        }
    });
});
