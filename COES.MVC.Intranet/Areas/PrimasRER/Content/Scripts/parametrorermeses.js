var controler = siteRoot + "PrimasRER/ParametroRER/";
var controlerRelaciones = siteRoot + "PrimasRER/ParametroRERRelaciones/";

$(document).ready(function () {
    buscarVersion();

    $("#popupEditParametroRERMeses").addClass("general-popup");
    $("#popupindexRevisiones").addClass("general-popup");
    $("#popupRelacionCentralRER").addClass("general-popup");
    $("#popupRelacionCodigoRetiroEdit").addClass("general-popup");
    $("#popupRelacionCodigoRetiroNew").addClass("general-popup");

    const equipoSelect = $("#equipoSelectNew");

    // Agregar un event listener para el evento change
    equipoSelect.on("change", function () {
        limpiarTabla("tablaBodyRelacionesNew");
    });

});

addAntiForgeryToken = function (data) {
    data.__RequestVerificationToken = $('input[name=__RequestVerificationToken]').val();
    return data;
};

function buscarVersion() {
    mostrarListado();
};

mostrarListado = function () {
    $.ajax({
        type: 'POST',
        url: controler + "listaMeses",
        data: {
            Reravcodi: $('#comboReravcodi').val()
        },
        success: function (evt) {
            $('#listado').html(evt);
            oTable = $('#tablameses').dataTable({
                "sPaginationType": "full_numbers",
                "destroy": "true",
                "ordering": false
            });
        },
        error: function () {
            mostrarError();
        }
    });
};

indexRevisiones = function (Rerpprcodi) {
    $.ajax({
        type: 'GET',
        url: controler + "indexRevisiones",
        data: {
            Rerpprcodi: Rerpprcodi
        },
        success: function (evt) {
            $('#indexRevisiones').html(evt);
            addDeleteEvento();
            $('#mensaje').css("display", "none");
            setTimeout(function () {
                $('#popupindexRevisiones').bPopup({
                    easing: 'easeOutBack',
                    speed: 450,
                    transition: 'slideDown'
                });
            }, 50);
        },
        error: function () {
            mostrarError();
        }
    });
};

function recargarRevisiones() {
    $.ajax({
        type: 'POST',
        url: controler + "RecargarRevisiones",
        data: {
            Rerpprcodi: $('#indexRevisionesRerpprcodi').val()
        },
        success: function (evt) {
            mostrarExitoOperacion();
            $('#popupindexRevisiones').bPopup().close();
        },
        error: function () {
            mostrarError();
        }
    });
}

//Funciones para eliminar una relacion de revision con un mes de un Anio Tarifario
function addDeleteEvento() {
    console.log("Eliminado una revision")
    $(".delete").click(function (e) {
        e.preventDefault();
        if (confirm("¿Desea eliminar la información seleccionada?")) {
            console.log("valor: ", $(this).attr("id").split("_"));
            id = $(this).attr("id").split("_")[1];
            console.log("id: ", id);
            $.ajax({
                type: "post",
                dataType: "text",
                url: controler + "Delete/" + id,
                data: addAntiForgeryToken({ id: id }),
                success: function (resultado) {
                    if (resultado == "true") {
                        mostrarExitoOperacion();
                        $("#fila_" + id).remove();
                    } else {
                        mostrarErrorConMensaje("Lo sentimos, ha ocurrido un error: " + resultado);
                    }
                }
            });
        }
    });
};

copiarVersionAnterior = function () {
    if (confirm("¿Desea copiar de una versión anterior?")) {
        $.ajax({
            type: 'POST',
            url: controler + "copiarVersionAnterior",
            data: {
                Reravcodi: $('#comboReravcodi').val()
            },
            success: function (resultado) {
                if (resultado == 1) {
                    mostrarExitoOperacion();
                    buscarVersion();
                } else if (resultado == -2) {
                    alert("No existe una versión anterior cuando la versión es anual");
                } else
                    mostrarError();
            },
            error: function () {
                mostrarError();
            }
        });
    }
};

editarMes = function (Rerpprcodi) {
    $.ajax({
        type: 'POST',
        url: controler + "EditMeses",
        data: {
            Rerpprcodi: Rerpprcodi
        },
        success: function (evt) {
            $('#editMeses').html(evt);
            $('#mensaje').css("display", "none");
            setTimeout(function () {
                $('#popupEditParametroRERMeses').bPopup({
                    easing: 'easeOutBack',
                    speed: 450,
                    transition: 'slideDown'
                });
            }, 50);

        },
        error: function () {
            mostrarError();
        }
    });
};

//Funciones de eliminado de registro
function addDeleteEvent() {
    $(".delete").click(function (e) {
        e.preventDefault();
        if (confirm("¿Desea eliminar la información seleccionada?")) {
            id = $(this).attr("id").split("_")[1];
            $.ajax({
                type: "post",
                dataType: "text",
                url: controler + "Delete/" + id,
                data: addAntiForgeryToken({ id: id }),
                success: function (resultado) {
                    if (resultado == "true")
                        $("#fila_" + id).remove();
                    else
                        alert("No se ha logrado eliminar el registro");
                }
            });
        }
    });
};

function viewEvent() {

    $('.view').click(function () {
        id = $(this).attr("id").split("_")[1];
        abrirPopup(id);
    });
};

updateMeses = function () {
    if (confirm('¿Está seguro que desea actualizar el mes del Año Tarifario?')) {
        if (validarTipoCambio()) {
            // Obtener el valor seleccionado del dropdown
            var selectedRecaPeriCodi = $('#ListaRecalculo').val();

            $.ajax({
                type: 'POST',
                url: controler + 'updateMeses',
                dataType: 'json',
                data: $('#frmEditParametroRERMeses').serialize() + '&recaPeriCodi=' + selectedRecaPeriCodi,
                cache: false,
                success: function (resultado) {
                    if (resultado == 1) {
                        mostrarExitoOperacion();
                        $('#popupEditParametroRERMeses').bPopup().close();
                        buscarVersion();
                    } else
                        mostrarError();
                },
                error: function () {
                    mostrarError();
                }
            });
        }

    }
};

function validarTipoCambio() {
    var tipoCambioInput = document.getElementById("Rerpprtipocambio");
    var tipoCambioValue = tipoCambioInput.value;

    // Expresión regular para verificar el formato del tipo de cambio con un máximo de 3 decimales
    var regex = /^\d{1,3}(\.\d{1,3})?$/; ///^\d+(\.\d{1,3})?$/;

    if (tipoCambioValue == 0) {
        // El valor no cumple con el formato requerido
        alert('El "Tipo de cambio" no puede ser cero');
        tipoCambioInput.focus(); // Darle el foco al input para corregir el valor
        return false
    }

    if (tipoCambioValue < 0) {
        // El valor no cumple con el formato requerido
        alert('El "Tipo de cambio" no puede ser negativo');
        tipoCambioInput.focus(); // Darle el foco al input para corregir el valor
        return false
    }

    if (!regex.test(tipoCambioValue)) {
        // El valor no cumple con el formato requerido
        alert('El "Tipo de cambio" debe tener un máximo de 3 números enteros y 3 decimales.');
        tipoCambioInput.focus(); // Darle el foco al input para corregir el valor
        return false
    }
    return true
}

mostrarExitoOperacion = function () {
    $('#mensaje').removeClass("action-error");
    $('#mensaje').addClass("action-exito");
    $('#mensaje').text("La operación se realizó con exito...!");
    $('#mensaje').css("display", "block");
};

mostrarError = function () {
    $('#mensaje').removeClass("action-exito");
    $('#mensaje').addClass("action-error");
    $('#mensaje').text("Ha ocurrido un error");
    $('#mensaje').css("display", "block");
};

function habilitarCombo() {
    var radioPMPO = document.getElementById("PMPO");
    var comboRecalculo = document.getElementById("ListaRecalculo");

    if (radioPMPO.checked) {
        comboRecalculo.disabled = true;
        comboRecalculo.selectedIndex = -1; // Deseleccionar cualquier opción seleccionada previamente
    } else {
        comboRecalculo.disabled = false;
        comboRecalculo.selectedIndex = 0;
    }
}

function verificarCombo() {
    var radioPMPO = document.getElementById("PMPO");
    var comboRecalculo = document.getElementById("ListaRecalculo");

    if (radioPMPO.checked) {
        comboRecalculo.disabled = true;
        comboRecalculo.selectedIndex = -1; // Deseleccionar cualquier opción seleccionada previamente
    }
}

listaRevisiones = function (Rerpprcodi) {
    $.ajax({
        type: 'POST',
        url: controler + "ListaRevisiones",
        data: {
            Rerpprcodi: Rerpprcodi
        },
        success: function (evt) {
            $('#listaRevisiones').html(evt);
            $('#mensaje').css("display", "none");
            setTimeout(function () {
                $('#popupListaRevisiones').bPopup({
                    easing: 'easeOutBack',
                    speed: 450,
                    transition: 'slideDown'
                });
            }, 50);

        },
        error: function () {
            mostrarError();
        }
    });
};

relacionCentralRER = function (Rerpprcodi) {
    $.ajax({
        type: 'POST',
        url: controlerRelaciones + "Index",
        data: {
            Rerpprcodi: Rerpprcodi
        },
        success: function (evt) {
            $('#relacionCentralRER').html(evt);
            $('#mensaje').css("display", "none");
            setTimeout(function () {
                $('#popupRelacionCentralRER').bPopup({
                    easing: 'easeOutBack',
                    speed: 450,
                    transition: 'slideDown'
                });
            }, 50);

        },
        error: function () {
            mostrarError();
        }
    });
};

nuevaRelacion = function () {
    $.ajax({
        type: 'POST',
        url: controlerRelaciones + "New",
        data: {
            Rerpprcodi: $('#Rerpprcodi').val()
        },
        success: function (evt) {
            $('#popupRelacionCodigoRetiroEdit').bPopup().close();
            $('#newRelacionCodigoRetiro').html(evt);
            $('#mensaje').css("display", "none");
            setTimeout(function () {
                $('#popupRelacionCodigoRetiroNew').bPopup({
                    easing: 'easeOutBack',
                    speed: 450,
                    transition: 'slideDown'
                });
            }, 50);

        },
        error: function () {
            mostrarError();
        }
    });
};

function cargarCentralRERCodRetiro() {
    var empresaSelect = document.getElementById('empresaSelectNew');
    var selectedEmpCod = empresaSelect.value;
    var Reravaniotarif = $('#Reravaniotarif').val();
    var Rerpprmes = $('#Rerpprmes').val();
    var Rerpprcodi = $('#Rerpprcodi').val();

    $.ajax({
        url: controlerRelaciones + 'ObtenerCentralRERCodRetiro?emprcodi=' + selectedEmpCod + '&Rerpprcodi=' + Rerpprcodi,
        type: 'GET',
        success: function (response) {
            
            var ListCentralRER = response.ListCentralRER;
            var ListaCodigoRetiro = response.ListaCodigoRetiro;

            listarEnCombo(ListCentralRER, 'equipoSelectNew', 'Rercencodi', 'Equinomb');

            var codRetiroSelect = document.getElementById('codRetiroSelectNew');
            codRetiroSelect.innerHTML = ''; // Limpiar opciones anteriores
            codRetiroSelect.appendChild(crearOptionDefault()); // Crear la opción default

            ListaCodigoRetiro.forEach(function (CodigoRetiro) {
                var option = document.createElement('option');
                option.text = CodigoRetiro.SoliCodiRetiCodigo;
                option.value = CodigoRetiro.SoliCodiRetiCodi + '-' + CodigoRetiro.BarrCodi + '-' + CodigoRetiro.BarrNombBarrTran;
                codRetiroSelect.appendChild(option);
            });

            limpiarTabla("tablaBodyRelacionesNew");
        },
        error: function (error) {
            console.log(error);
        }
    });
}

function grabarRelacionBarraTransferencia() {

    var mensajeError = validarDatos(1);
    if (mensajeError != "") {
        alert(mensajeError);
    } else {
        almacenarRelaciones("tablaBodyRelacionesNew", 'codigosRetiroRelacionesNew', 'campoHiddenNew');
        $.ajax({
            type: 'POST',
            url: controlerRelaciones + 'save',
            dataType: 'json',
            data:
            {
                rercencodi: $('#equipoSelectNew').val(),
                codigosRetiroRelaciones: $('#codigosRetiroRelacionesNew').val(),
                rerpprcodi: $('#IdRerCentralCodRetiroNew').val()
            },

            cache: false,
            success: function (aData) {
                if (aData.Resultado == "1") {
                    $('#popupRelacionCodigoRetiroNew').bPopup().close();
                    $('#popupRelacionCodigoRetiroEdit').bPopup().close();
                    $('#popupRelacionCentralRER').bPopup().close();
                    mostrarExitoOperacion();
                } else if (aData.Resultado == "-2") {
                    alert("Para la central RER seleccionada, ya existe códigos de retiro relacionados");
                } else
                    alert("Lo sentimos, ha ocurrido un error");
            },
            error: function () {
                mostrarError("Lo sentimos, ha ocurrido un error");
            }
        });
    }
}

function actualizarRelacionBarraTransferencia() {

    var mensajeError = validarDatos(0);
    if (mensajeError != "") {
        alert(mensajeError);
    } else {
        almacenarRelaciones("tablaBodyRelacionesEdit", 'codigosRetiroRelacionesEdit', 'campoHiddenEdit');
        $.ajax({
            type: 'POST',
            url: controlerRelaciones + 'update',
            dataType: 'json',
            data:
            {
                rercencodi: $('#equipoSelectEdit').val(),
                codigosRetiroRelaciones: $('#codigosRetiroRelacionesEdit').val(),
                rerpprcodi: $('#IdRerCentralCodRetiroEdit').val()
            },

            cache: false,
            success: function (aData) {
                if (aData.Resultado == "1") {
                    $('#popupRelacionCodigoRetiroNew').bPopup().close();
                    $('#popupRelacionCodigoRetiroEdit').bPopup().close();
                    $('#popupRelacionCentralRER').bPopup().close();
                    mostrarExitoOperacion();
                    //window.location.reload(true);
                } else
                    alert("Lo sentimos, ha ocurrido un error");
            },
            error: function () {
                mostrarError("Lo sentimos, ha ocurrido un error");
            }
        });
    }
}

//Agregar Relaciones a la tabla
function agregarRelacionBarraTransferencia(valor) {
    
    if (valor == 1) {
        agregarRelacionEnTabla("codRetiroSelectNew", "tablaBodyRelacionesNew");
    } else {
        agregarRelacionEnTabla("codRetiroSelectEdit", "tablaBodyRelacionesEdit");
    }
}

function agregarRelacionEnTabla(codRetiroSelect, tablaBodyRelaciones) {

    var codRetiroselect = document.getElementById(codRetiroSelect);
    var codigos = codRetiroselect.value.split("-");
    var codRetiroId = codigos[0];
    var barrCodi = codigos[1];
    var barrNombBarrTran = codigos[2];
    var codRetiroNomb = codRetiroselect.options[codRetiroselect.selectedIndex].innerText;

    var tablaBody = document.getElementById(tablaBodyRelaciones);
    // |X|codRetiroId|barrTransferenciaId|codRetiroText|barrTransferenciaText|
    // Verificar si el valor ya existe en la tabla
    var elementosTabla = capturarElementosTabla(tablaBodyRelaciones);
    var existeValor = elementosTabla.some(function (elemento) {
        return elemento[1] === codRetiroId;
    });

    if (existeValor) {
        alert("El elemento seleccionado ya existe en la tabla 'Código Retiro - Barra Transferencia'.");
    } else if (codRetiroselect.value === "-1") {
        alert("No se puede agregar la opción '--Seleccione--' ");
    } else {

        var nuevaFila = tablaBody.insertRow();
        var celdaBotonEliminar = nuevaFila.insertCell(0);
        var celdacodRetiroId = nuevaFila.insertCell(1);                  // id del codigo de retiro (coresocodi)
        var barrTransferenciaId = nuevaFila.insertCell(2);
        var codRetiroText = nuevaFila.insertCell(3);
        var barrTransferenciaText = nuevaFila.insertCell(4);

        celdacodRetiroId.innerHTML = codRetiroId;
        celdacodRetiroId.style.display = "none";
        barrTransferenciaId.innerHTML = barrCodi;
        barrTransferenciaId.style.display = "none";

        codRetiroText.innerHTML = codRetiroNomb;
        barrTransferenciaText.innerHTML = barrNombBarrTran;

        var imagenEliminar = document.createElement("img");
        imagenEliminar.src = "../../Content/Images/btn-cancel.png";
        imagenEliminar.title = "Eliminar el registro";
        imagenEliminar.alt = "Eliminar el registro";
        imagenEliminar.onclick = function () {
            if (confirm('¿Está seguro que desea eliminar la relación?')) {
                var fila = this.parentNode.parentNode;
                fila.parentNode.removeChild(fila);
            }
        };
        celdaBotonEliminar.appendChild(imagenEliminar);
    }
}

function agregarTodasRelacionBarraTransferencia(valor) {

    if (valor == 1) {
        agregarTodasRelacionesEnTabla("codRetiroSelectNew", "tablaBodyRelacionesNew");
    } else {
        agregarTodasRelacionesEnTabla("codRetiroSelectEdit", "tablaBodyRelacionesEdit");
    }
}

function agregarTodasRelacionesEnTabla(codRetiroSelect, tablaBodyRelaciones) {
    limpiarTabla(tablaBodyRelaciones);
    var codRetiroselect = document.getElementById(codRetiroSelect);
    var opciones = codRetiroselect.options;

    for (var i = 1; i < opciones.length; i++) {
        var codigos = opciones[i].value.split("-");
        var codRetiroId = codigos[0];
        var barrCodi = codigos[1];
        var barrNombBarrTran = codigos[2];

        var codRetiroNomb = opciones[i].text;

        var tablaBody = document.getElementById(tablaBodyRelaciones);
        var nuevaFila = tablaBody.insertRow();
        var celdaBotonEliminar = nuevaFila.insertCell(0);
        var celdacodRetiroId = nuevaFila.insertCell(1);                  // id del codigo de retiro (coresocodi)
        var barrTransferenciaId = nuevaFila.insertCell(2);
        var codRetiroText = nuevaFila.insertCell(3);
        var barrTransferenciaText = nuevaFila.insertCell(4);

        celdacodRetiroId.innerHTML = codRetiroId;
        celdacodRetiroId.style.display = "none";
        barrTransferenciaId.innerHTML = barrCodi;
        barrTransferenciaId.style.display = "none";

        codRetiroText.innerHTML = codRetiroNomb;
        barrTransferenciaText.innerHTML = barrNombBarrTran;

        var imagenEliminar = document.createElement("img");
        imagenEliminar.src = "../../Content/Images/btn-cancel.png";
        imagenEliminar.title = "Eliminar el registro";
        imagenEliminar.alt = "Eliminar el registro";
        imagenEliminar.onclick = function () {
            if (confirm('¿Está seguro que desea eliminar la Central/Unidad?')) {
                var fila = this.parentNode.parentNode;
                fila.parentNode.removeChild(fila);
            }
        };
        celdaBotonEliminar.appendChild(imagenEliminar);
    }
}

function capturarElementosTabla(tablaBody) {
    var tabla = document.getElementById(tablaBody);
    var filas = tabla.getElementsByTagName("tr");

    var elementosTabla = [];

    // Comenzamos en i = 0 del body
    for (var i = 0; i < filas.length; i++) {
        var celdas = filas[i].getElementsByTagName("td");
        var filaElementos = [];
        for (var j = 0; j < celdas.length; j++) {
            filaElementos.push(celdas[j].innerHTML);
        }
        elementosTabla.push(filaElementos);
    }
    return elementosTabla;
}

// Lista segun "listaDatos" en "comboSelect" con valores "valor" y textos "texto"
function listarEnCombo(listaDatos, comboSelect, valor, texto) {
    var listaDesplegable = document.getElementById(comboSelect);
    listaDesplegable.innerHTML = ''; // Limpiar opciones anteriores
    listaDesplegable.appendChild(crearOptionDefault()); // Crear la opción default

    listaDatos.forEach(function (codigoEntrega) {
        var option = document.createElement('option');
        option.text = codigoEntrega[texto];
        option.value = codigoEntrega[valor];
        listaDesplegable.appendChild(option);
    });
}

function crearOptionDefault() {
    var optionDefault = document.createElement('option');
    optionDefault.text = "--Seleccione--";
    optionDefault.value = -1;
    return optionDefault;
}

function limpiarTabla(idTablaBody) {
    var tbody = document.getElementById(idTablaBody);
    while (tbody.firstChild) {
        tbody.removeChild(tbody.firstChild);
    }
}

function almacenarRelaciones(tablaBody, nombreCodigos, campoHidden) {
    var valoresSegundaColumna = '0';
    var tablaCentralUnidad = capturarElementosTabla(tablaBody);
    for (var i = 0; i < tablaCentralUnidad.length; i++) {
        var codigo = tablaCentralUnidad[i][1]
        valoresSegundaColumna += "-" + codigo;
    }
    var inputRelaciones = document.createElement('input');
    inputRelaciones.type = 'hidden';
    inputRelaciones.id = nombreCodigos;
    inputRelaciones.value = valoresSegundaColumna;
    document.getElementById(campoHidden).appendChild(inputRelaciones);
}

function validarDatos(valor) {
    if (valor == 1) {
        tablaBodyRelaciones = "tablaBodyRelacionesNew";
    } else {
        tablaBodyRelaciones = "tablaBodyRelacionesEdit";
    }
    var mensajeError = "";
    var tablaRelaciones = capturarElementosTabla(tablaBodyRelaciones);
    var tablaTieneRelaciones = tablaRelaciones.length <= 0 ? "-1" : "1";

    var campos = [
        { nombre: "Empresa", valor: $('#empresaSelect').val() },
        { nombre: "Central", valor: $("#equipoSelect").val() },
        { nombre: "Tabla de relaciones", valor: tablaRelaciones }
    ];
    for (var i = 0; i < campos.length -1; i++) {
        var campo = campos[i];
        if (campo.valor === "" || campo.valor === null || campo.valor === -1 || campo.valor == "-1" || campo.valor == 0) {
            mensajeError += "El campo \"" + campo.nombre + "\" no fue seleccionado.\n"
        }
    }
    if (campos[2].valor == 0) {
        mensajeError += "La \"" + campos[2].nombre + "\" no posee elementos.\n"
    }
    return mensajeError;
}

mostrarErrorConMensaje = function (msg) {
    $('#mensaje').removeClass();
    $('#mensaje').html(msg);
    $('#mensaje').addClass("action-error");
};

function editarRelacionBarraTransferencia(Rerpprcodi, Rercencodi) {

    $.ajax({
        type: 'POST',
        url: controlerRelaciones + "Edit",
        data:
        {
            Rerpprcodi: Rerpprcodi,
            Rercencodi: Rercencodi
        },
        success: function (evt) {
            $('#popupRelacionCodigoRetiroNew').bPopup().close();
            $('#editRelacionCodigoRetiro').html(evt);
            $('#mensaje').css("display", "none");
            setTimeout(function () {
                $('#popupRelacionCodigoRetiroEdit').bPopup({
                    easing: 'easeOutBack',
                    speed: 450,
                    transition: 'slideDown'
                });
            }, 50);

        },
        error: function () {
            mostrarError();
        }
    });
}

function eliminarRelacionBarraTransferencia(Rerpprcodi, Rercencodi) {

    if (confirm("¿Desea eliminar la información seleccionada?")) {
        $.ajax({
            type: 'POST',
            url: controlerRelaciones + "Delete",
            dataType: 'json',
            data:
            {
                Rerpprcodi: Rerpprcodi,
                Rercencodi: Rercencodi
            },
            cache: false,
            success: function (aData) {
                if (aData.Resultado == "1") {
                    mostrarExitoOperacion();
                    $('#popupRelacionCentralRER').bPopup().close();
                } else
                    alert("Lo sentimos, ha ocurrido un error");
            },
            error: function () {
                mostrarError("Lo sentimos, ha ocurrido un error");
            }
        });
    }
}

function copiarRelacionMesAnterior() {

    if (confirm("¿Desea copiar las relaciones del mes anterior?")) {
        $.ajax({
            type: 'POST',
            url: controlerRelaciones + "copiarRelacionesMesAnterior",
            dataType: 'json',
            data:
            {
                Rerpprcodi: $('#Rerpprcodi').val()
            },
            cache: false,
            success: function (aData) {
                if (aData.Resultado == "1") {
                    mostrarExitoOperacion();
                    $('#popupRelacionCentralRER').bPopup().close();
                } else
                    alert("Lo sentimos, ha ocurrido un error");
            },
            error: function () {
                mostrarError("Lo sentimos, ha ocurrido un error");
            }
        });
    }
}