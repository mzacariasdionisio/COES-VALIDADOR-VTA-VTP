var controlador = siteRoot + 'Combustibles/ConfiguracionGas/';

$(function () {  
    $('#tablaCentrales').dataTable({
        "scrollY": 530,
        "scrollX": false,
        "sDom": 't',
        "ordering": false,
        "iDisplayLength": -1
    });

    $('#GuardarCentral').click(function () {
        guardarCentral();
    });

    $('#btnActualizar').click(function () {
        ejecutarNotificaciones(4);
    });
});

function ejecutarNotificaciones(tipo) {
    $.ajax({
        type: 'POST',
        url: controlador + 'EjecutarProcesoAutomatico',
        data: {
            tipo: tipo
        },
        success: function (evt) {
            if (evt.Resultado != "-1") {
                alert("El proceso se ejecutó correctamente");
                window.location.reload();
            } else {
                alert("Ha ocurrido un error: " + evt.Mensaje);
            }
        },
        error: function (err) {
            alert("Ha ocurrido un error...");
        }
    });
}

function editarCentral(idCentral) {
    limpiarBarraMensaje("mensaje_popupEditar");
    limpiarBarraMensaje("mensaje_centrales");
    $.ajax({
        type: 'POST',
        url: controlador + "CargarCentral",
        data: {
            cbcxfecodi: idCentral,
        },
        success: function (evt) {
            if (evt.Resultado != "-1") {
                actualizarDatosCentralPopup(evt.CentralTermica);

                abrirPopup("popupEditarCentral");
            } else {
                mostrarMensaje('mensaje', 'error', evt.Mensaje);
            }                        
        },
        error: function (err) {            
            mostrarMensaje('mensaje', 'error', 'Ha ocurrido un error.' + evt.Mensaje);
        }
    });
}

function actualizarDatosCentralPopup(central) {
    
    $("#hfIdCentral").val(central.Cbcxfecodi);
    $("#campoEmpresa").html(central.Emprnomb);
    $("#campoCentral").html(central.Equinomb);
    if (central.Cbcxfeexistente == 1)
        document.getElementById("chbxExistente").checked = true;
    else {
        if (central.Cbcxfeexistente == 0)
            document.getElementById("chbxExistente").checked = false;
    }
    if (central.Cbcxfenuevo == 1)
        document.getElementById("chbxNueva").checked = true;
    else {
        if (central.Cbcxfenuevo == 0)
            document.getElementById("chbxNueva").checked = false;
    }
    if (central.Cbcxfevisibleapp == 1)
        document.getElementById("chbxEnExtranet").checked = true;
    else {
        if (central.Cbcxfevisibleapp == 0)
            document.getElementById("chbxEnExtranet").checked = false;
    }

    //valores
    $("#orden").val(central.Cbcxfeorden); 
    $("#minPUSum").val(central.MinPrecioUnitSuministro);
    $("#maxPUSum").val(central.MaxPrecioUnitSuministro);
    $("#minPUTransp").val(central.MinPrecioUnitTransporte);
    $("#maxPUTransp").val(central.MaxPrecioUnitTransporte);
    $("#minPUDistrib").val(central.MinPrecioUnitDistribucion);
    $("#maxPUDistrib").val(central.MaxPrecioUnitDistribucion);
    $("#minCostoGN").val(central.MinCostoGasNatural);
    $("#maxCostoGN").val(central.MaxCostoGasNatural);
}

function guardarCentral() {
    limpiarBarraMensaje("mensaje_popupEditar");
    limpiarBarraMensaje("mensaje_centrales");
    var centralT = {};
    centralT = getCentralTermica();

    var msg = validarCamposCentralAGuardar(centralT);

    if (msg == "") {
        $.ajax({            
            type: 'POST',
            url: controlador + 'GuardarCentral',
            contentType: "application/json",
            dataType: 'json',
            data: JSON.stringify({
                central: centralT,
            }
            ),
            cache: false,
            success: function (evt) {
                if (evt.Resultado != "-1") {

                    cerrarPopup("popupEditarCentral");
                    window.location.reload();                    
                } else {
                    mostrarMensaje('mensaje_popupEditar', 'error', evt.Mensaje);
                }
            },
            error: function (err) {
                mostrarMensaje('mensaje_popupEditar', 'error', 'Ha ocurrido un error.' + evt.Mensaje);
            }
        });
    } else {
        mostrarMensaje('mensaje_popupEditar', 'error', msg);
    }
}



function getCentralTermica() {
    var obj = {};

    obj.Cbcxfecodi = $("#hfIdCentral").val();
    obj.Cbcxfeexistente = document.getElementById("chbxExistente").checked == true ? 1 : 0;
    obj.Cbcxfenuevo = document.getElementById("chbxNueva").checked == true ? 1 : 0;
    obj.Cbcxfevisibleapp = document.getElementById("chbxEnExtranet").checked == true ? 1 : 0;

    obj.Cbcxfeorden = $("#orden").val(); 

    obj.MinPrecioUnitSuministro = $("#minPUSum").val();
    obj.MaxPrecioUnitSuministro = $("#maxPUSum").val();
    obj.MinPrecioUnitTransporte = $("#minPUTransp").val();
    obj.MaxPrecioUnitTransporte = $("#maxPUTransp").val();
    obj.MinPrecioUnitDistribucion = $("#minPUDistrib").val();
    obj.MaxPrecioUnitDistribucion = $("#maxPUDistrib").val();
    obj.MinCostoGasNatural = $("#minCostoGN").val();
    obj.MaxCostoGasNatural = $("#maxCostoGN").val();
    
    return obj;
}

function validarCamposCentralAGuardar(central) {
    var msj = "";
    var valMaxPermitido = 999999999;
    if (central.Cbcxfevisibleapp == 1) {  //centrales visibles en extranet

        // valido el orden 
        var esTexto = isNaN(central.Cbcxfeorden); 
        if (!esTexto) { 
            var val = Number(central.Cbcxfeorden); 
            if (val > 0) { 
                if (Number.isInteger(val)) { 
                    
                } else { 
                    msj += "<p>Debe ingresar un número entero.</p>"; 
                } 
            } else { 
                msj += "<p>Debe ingresar un número entero positivo.</p>"; 
            } 
        } else { 
            msj += "<p>Debe ingresar un número entero positivo correcto.</p>"; 
        } 

        //suministro
        if (central.MinPrecioUnitSuministro != "" && central.MaxPrecioUnitSuministro != "") {
            if (parseFloat(central.MinPrecioUnitSuministro) >= 0 ) {
                if (parseFloat(central.MinPrecioUnitSuministro) <= parseFloat(central.MaxPrecioUnitSuministro)) {

                } else {
                    msj += "<p>Precio Unitario por Suministro, el valor mínimo no debe superar al valor máximo.</p>";
                }
            } else {
                msj += "<p>Debe ingresar un rango válido (valores >= 0) para el precio unitario por suministro.</p>";
            }
           
        } else {
            if (central.MinPrecioUnitSuministro == "") {
                if (central.MaxPrecioUnitSuministro == "") {
                    msj += "<p>Debe ingresar un valor mínimo y máximo válidos para el precio unitario por suministro.</p>";
                } else {
                    msj += "<p>Debe ingresar un valor mínimo válido para el precio unitario por suministro.</p>";
                }
            } else {
                if (central.MaxPrecioUnitSuministro == "") {
                    msj += "<p>Debe ingresar un valor máximo válido para el precio unitario por suministro.</p>";
                }
            }
        }
        if (central.MaxPrecioUnitSuministro != "") {
            if (parseFloat(central.MaxPrecioUnitSuministro) > valMaxPermitido) {
                msj += "<p>El valor ingresado para el máximo precio unitario por suministro supera la capacidad máxima permitida por la base de datos: (##########.#####)</p>";
            }
        }
        if (central.MinPrecioUnitSuministro != "") {
            if (parseFloat(central.MinPrecioUnitSuministro) > valMaxPermitido) {
                msj += "<p>El valor ingresado para el mínimo precio unitario por suministro supera la capacidad máxima permitida por la base de datos: (##########.#####)</p>";
            }
        }

        //transporte
        if (central.MinPrecioUnitTransporte != "" && central.MaxPrecioUnitTransporte != "") {
            if (parseFloat(central.MinPrecioUnitTransporte) >= 0 ) {
                if (parseFloat(central.MinPrecioUnitTransporte) <= parseFloat(central.MaxPrecioUnitTransporte)) {

                } else {
                    msj += "<p>Precio Unitario por Transporte, el valor mínimo no debe superar al valor máximo.</p>";
                }
            } else {
                msj += "<p>Debe ingresar un rango válido (valores >= 0) para el precio unitario por transporte.</p>";
            }            
        } else {
            if (central.MinPrecioUnitTransporte == "") {
                if (central.MaxPrecioUnitTransporte == "") {
                    msj += "<p>Debe ingresar un valor mínimo y máximo válidos para el precio unitario por transporte.</p>";
                } else {
                    msj += "<p>Debe ingresar un valor mínimo válido para el precio unitario por transporte.</p>";
                }
            } else {
                if (central.MaxPrecioUnitTransporte == "") {
                    msj += "<p>Debe ingresar un valor máximo válido para el precio unitario por transporte.</p>";
                }
            }
        }
        if (central.MaxPrecioUnitTransporte != "") {
            if (parseFloat(central.MaxPrecioUnitTransporte) > valMaxPermitido) {
                msj += "<p>El valor ingresado para el máximo precio unitario por transporte supera la capacidad máxima permitida por la base de datos: (##########.#####)</p>";
            }
        }
        if (central.MinPrecioUnitTransporte != "") {
            if (parseFloat(central.MinPrecioUnitTransporte) > valMaxPermitido) {
                msj += "<p>El valor ingresado para el mínimo precio unitario por transporte supera la capacidad máxima permitida por la base de datos: (##########.#####)</p>";
            }
        }

        //transporte
        if (central.MinPrecioUnitDistribucion != "" && central.MaxPrecioUnitDistribucion != "") {
            if (parseFloat(central.MinPrecioUnitDistribucion) >= 0) {
                if (parseFloat(central.MinPrecioUnitDistribucion) <= parseFloat(central.MaxPrecioUnitDistribucion)) {

                } else {
                    msj += "<p>Precio Unitario por Distribución, el valor mínimo no debe superar al valor máximo.</p>";
                }
            } else {
                msj += "<p>Debe ingresar un rango válido (valores >= 0) para el precio unitario por distribución.</p>";
            }            
        } else {
            if (central.MinPrecioUnitDistribucion == "") {
                if (central.MaxPrecioUnitDistribucion == "") {
                    msj += "<p>Debe ingresar un valor mínimo y máximo válidos para el precio unitario por distribucion.</p>";
                } else {
                    msj += "<p>Debe ingresar un valor mínimo válido para el precio unitario por distribucion.</p>";
                }
            } else {
                if (central.MaxPrecioUnitDistribucion == "") {
                    msj += "<p>Debe ingresar un valor máximo válido para el precio unitario por distribucion.</p>";
                }
            }
        }
        if (central.MaxPrecioUnitDistribucion != "") {
            if (parseFloat(central.MaxPrecioUnitDistribucion) > valMaxPermitido) {
                msj += "<p>El valor ingresado para el máximo precio unitario por distribución supera la capacidad máxima permitida por la base de datos: (##########.#####)</p>";
            }
        }
        if (central.MinPrecioUnitDistribucion != "") {
            if (parseFloat(central.MinPrecioUnitDistribucion) > valMaxPermitido) {
                msj += "<p>El valor ingresado para el mínimo precio unitario por distribución supera la capacidad máxima permitida por la base de datos: (##########.#####)</p>";
            }
        }
    } else {
        //suministro
        if (central.MinPrecioUnitSuministro != "") {
            if (parseFloat(central.MinPrecioUnitSuministro) < 0 ) {
                msj += "<p>Debe ingresar un valor mínimo correcto (valor >= 0) para el precio unitario por suministro.</p>";
            }
            if (parseFloat(central.MinPrecioUnitSuministro) > valMaxPermitido) {
                msj += "<p>El valor ingresado para el mínimo precio unitario por suministro supera la capacidad máxima permitida por la base de datos: (##########.#####)</p>";
            }
        }
        if (central.MaxPrecioUnitSuministro != "") {
            if (parseFloat(central.MaxPrecioUnitSuministro) < 0 ) {
                msj += "<p>Debe ingresar un valor máximo correcto (valor >= 0) para el precio unitario por suministro.</p>";
            }
            if (parseFloat(central.MaxPrecioUnitSuministro) > valMaxPermitido) {
                msj += "<p>El valor ingresado para el máximo precio unitario por suministro supera la capacidad máxima permitida por la base de datos: (##########.#####)</p>";
            }
        }
        if (central.MinPrecioUnitSuministro != "" && central.MaxPrecioUnitSuministro != "") {
            if (parseFloat(central.MinPrecioUnitSuministro) > parseFloat(central.MaxPrecioUnitSuministro)) {
                msj += "<p>Precio Unitario por Suministro, el valor mínimo no debe superar al valor máximo.</p>";
            }
        }

        //transporte
        if (central.MinPrecioUnitTransporte != "") {
            if (parseFloat(central.MinPrecioUnitTransporte) < 0 ) {
                msj += "<p>Debe ingresar un valor mínimo correcto (valor >= 0) para el precio unitario por transporte.</p>";
            }
            if (parseFloat(central.MinPrecioUnitTransporte) > valMaxPermitido) {
                msj += "<p>El valor ingresado para el mínimo precio unitario por transporte supera la capacidad máxima permitida por la base de datos: (##########.#####)</p>";
            }
        }
        if (central.MaxPrecioUnitTransporte != "") {
            if (parseFloat(central.MaxPrecioUnitTransporte) < 0 ) {
                msj += "<p>Debe ingresar un valor máximo correcto (valor >= 0) para el precio unitario por transporte.</p>";
            }
            if (parseFloat(central.MaxPrecioUnitTransporte) > valMaxPermitido) {
                msj += "<p>El valor ingresado para el máximo precio unitario por transporte supera la capacidad máxima permitida por la base de datos: (##########.#####)</p>";
            }
        }
        if (central.MinPrecioUnitTransporte != "" && central.MaxPrecioUnitTransporte != "") {
            if (parseFloat(central.MinPrecioUnitTransporte) > parseFloat(central.MaxPrecioUnitTransporte)) {
                msj += "<p>Precio Unitario por Transporte, el valor mínimo no debe superar al valor máximo.</p>";
            }
        }

        //distribucion
        if (central.MinPrecioUnitDistribucion != "") {
            if (parseFloat(central.MinPrecioUnitDistribucion) < 0 ) {
                msj += "<p>Debe ingresar un valor mínimo correcto (valor >= 0) para el precio unitario por distribucion.</p>";
            }
            if (parseFloat(central.MinPrecioUnitDistribucion) > valMaxPermitido) {
                msj += "<p>El valor ingresado para el mínimo precio unitario por distribución supera la capacidad máxima permitida por la base de datos: (##########.#####)</p>";
            }
        }
        if (central.MaxPrecioUnitDistribucion != "") {
            if (parseFloat(central.MaxPrecioUnitDistribucion) < 0 ) {
                msj += "<p>Debe ingresar un valor máximo correcto (valor >= 0) para el precio unitario por distribucion.</p>";
            }
            if (parseFloat(central.MaxPrecioUnitDistribucion) > valMaxPermitido) {
                msj += "<p>El valor ingresado para el máximo precio unitario por distribución supera la capacidad máxima permitida por la base de datos: (##########.#####)</p>";
            }
        }
        if (central.MinPrecioUnitDistribucion != "" && central.MaxPrecioUnitDistribucion != "") {
            if (parseFloat(central.MinPrecioUnitDistribucion) > parseFloat(central.MaxPrecioUnitDistribucion)) {
                msj += "<p>Precio Unitario por distribucion, el valor mínimo no debe superar al valor máximo.</p>";
            }
        }
    }

    //costo gas natural
    if (central.MinCostoGasNatural != "" && central.MaxCostoGasNatural != "") {
        if (parseFloat(central.MinCostoGasNatural) >= 0 ) {
            if (parseFloat(central.MinCostoGasNatural) <= parseFloat(central.MaxCostoGasNatural)) {

            } else {
                msj += "<p>Costo de Gas Natural, el valor mínimo no debe superar al valor máximo.</p>";
            }
        } else {
            msj += "<p>Debe ingresar un rango válido (valores >=0) para el costo de gas natural.</p>";
        }                        
    } else {
        if (central.MinCostoGasNatural == "") {
            if (central.MaxCostoGasNatural == "") {
                msj += "<p>Debe ingresar un valor mínimo y máximo válidos para el costo de gas natural.</p>";
            } else {
                msj += "<p>Debe ingresar un valor mínimo válido para el costo de gas natural.</p>";
            }
        } else {
            if (central.MaxCostoGasNatural == "") {
                msj += "<p>Debe ingresar un valor máximo válido para el costo de gas natural.</p>";
            }
        }
    }

    if (central.MaxCostoGasNatural != "" ) {
        if (parseFloat(central.MaxCostoGasNatural) > valMaxPermitido) {
            msj += "<p>El valor ingresado para el máximo costo de gas natural supera la capacidad máxima permitida por la base de datos: (##########.#####)</p>";
        }
    }

    if (central.MinCostoGasNatural != "") {
        if (parseFloat(central.MinCostoGasNatural) > valMaxPermitido) {
            msj += "<p>El valor ingresado para el mínimo costo de gas natural supera la capacidad máxima permitida por la base de datos: (##########.#####)</p>";
        }
    }

    return msj;
}

function cerrarPopup(id) {
    $("#" + id).bPopup().close()
}

function abrirPopup(contentPopup) {
    setTimeout(function () {
        $("#" + contentPopup).bPopup({
            easing: 'easeOutBack',
            speed: 450,
            transition: 'slideDown',
            modalClose: false
        });
    }, 50);
}


function mostrarMensaje(id, tipo, mensaje) {
    $("#" + id).css("display", "block");
    $('#' + id).removeClass();
    $('#' + id).addClass('action-' + tipo);
    $('#' + id).html(mensaje);

}

function limpiarBarraMensaje(id) {
    $('#' + id).css("display", "none");
    $('#' + id).removeClass();
    $('#' + id).addClass('action-message');
    $('#' + id).html('');
}