var controlador = siteRoot + "campanias/plantransmision/";
var controladorFichas = siteRoot + "campanias/fichasproyecto/";
var catPropietario = 2;
var catConcesionTemporal = 3;
var catConcesionActual = 4;
var catTipoPPL = 5;
var catSE = 6;

var catPerfil = 16;
var catPreFacltibilidad = 17;
var catFacltibilidad = 18;
var catEstudDef = 19;
var catEia = 20;

var fichaAValor = 1;
var fichaBValor = 2;
var fichaCValor = 3;
var fichaDValor = 4;

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
            if (typeof cargarDatosA !== 'undefined' &&
                typeof cargarDatosB !== 'undefined' &&
                typeof cargarDatosC !== 'undefined' &&
                typeof cargarDatosD !== 'undefined') {
                clearInterval(interval);
                resolve();
            }
        }, 100); // Revisa cada 100ms
    });
}



function mostrarFichasFiltradas() {
    $("#tabfichaa").hide();
    $("#tabfichab").hide();
    $("#tabfichac").hide();
    $("#tabfichad").hide();
    const fichasValores = {
        fichaa: fichaAValor,
        fichab: fichaBValor,
        fichac: fichaCValor,
        fichad: fichaDValor,
    };

    Object.keys(fichasValores).forEach((ficha) => {
        const fichaDisponible =
            hojasRegistrar.find((valor) => valor === fichasValores[ficha]) ?? null;
        $(`#tab${ficha}`).toggle(fichaDisponible !== null);
    });
}

function openFicha(tipoFicha, tabelement) {

    if (tipoFicha != "fichad") {
        if (entre) {
            if (cambiosRealizados == true) {
                entre = true;
                const resultado = guardarDetalleTabla();
                const resultadg = validarTablaAntesDeGuardar(hotFichaD);
              
                if (!resultado) { // Si guardarDetalleTabla retorna false o undefined
                    cambiosRealizados = false;
                    return; // Detener la ejecución
                }
                else {
                    if (!resultadg) { 
                    popupGuardadoAutomatico();
                }
                }
            }
        }
        else {
            if (cambiosRealizados == true) {
                popupGuardadoAutomatico();
            }
        }
    }
    limpiarMensaje('mensajeFicha');
    if (cambiosRealizados === true) {
        popupGuardadoAutomatico();
        cambiosRealizados = false;
        return;
    }
    $(".ficha").hide();
    if (tipoFicha === "fichaa") {
        $("#HFichaA").show();
        formularioSeleccionado = "HFichaA";
        cargarDatosA();
        highlightTab(tabelement, 'tab-container-2');
    } else if (tipoFicha === "fichab") {
        $("#HFichaB").show();
        cargarDatosB();
        highlightTab(tabelement, 'tab-container-2');
        formularioSeleccionado = "HFichaB";
    } else if (tipoFicha === "fichac") {
        $("#HFichaC").show();
        cargarDatosC();
        highlightTab(tabelement, 'tab-container-2');
        formularioSeleccionado = "HFichaC";
    } else if (tipoFicha === "fichad") {
        $("#HFichaD").show();
        $("#formFinTablaD").hide();
        $("#formInicioTablaD").show();
        formularioSeleccionado = "HFichaD";

        cargarDatosD();
        highlightTab(tabelement, 'tab-container-2');
    } else if (tipoFicha === "fichaadic") {
        $("#FichaAdi").show();
        cargarAdicionalLin();
        highlightTab(tabelement, 'tab-container-2');
    }

    
}

function eliminarFile(id) {
    document.getElementById("contenidoPopup").innerHTML = '¿Está seguro de realizar esta operación?';
    $('#popupProyectoGeneral').bPopup({
        easing: 'easeOutBack',
        speed: 450,
        transition: 'slideDown'
    });
    $('#btnConfirmarPopup').off('click').on('click', function() {
        $.ajax({
            type: "POST",
            url: controladorFichas + "EliminarFile",
            data: {
                id: id,
            },
            dataType: "json",
            success: function (result) {
                if (result == 1) {
                    $("#fila" + id).remove();
                    mostrarMensaje(
                        "mensajeFicha",
                        "exito",
                        "El archivo se eliminó correctamente."
                    );
                } else {
                    mostrarMensaje("mensajeFicha", "error", "Se ha producido un error.");
                }
            },
            error: function () {
                mostrarMensaje("mensajeFicha", "error", "Se ha producido un error.");
            },
        });
        popupClose('popupProyectoGeneral');
    });
}