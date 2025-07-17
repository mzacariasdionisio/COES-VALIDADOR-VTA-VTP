var controler = siteRoot + "PrimasRER/CentralRER/";
const imageRoot = siteRoot + "Content/Images/";

$(document).ready(function () {
    buscar(-2, -2, -2, null, null, null, null, -2);
    $('.txtFecha').Zebra_DatePicker({
    });
    $('#Rercenfechainicio').Zebra_DatePicker({
        onSelect: function () {
            cambiarFechaFin();
            limpiarCombo('centralPMPOSelect');
            limpiarCombo('barraPMPOSelect');
        }
    });
    $('#Rercenfechafin').Zebra_DatePicker({
        onSelect: function () {
            limpiarCombo('centralPMPOSelect');
            limpiarCombo('barraPMPOSelect');
        }
    });
    $('#btnBuscar').click(function () {
        buscarFiltro();
    });
    $('#btnNuevo').click(function () {
        nuevo();
    });
    $('#btnGenerarExcel').click(function () {
        generarExcel();
    });
    //ASSETEC - 18.10.2023
    $('#barraPMPOSelect').multipleSelect({
        single: true,
        filter: true
    });
    //ASSETEC - 18.10.2023
});

buscar = function (emprcodi, equicodi, ptomedicodi, fechaInicio, fechaFin, estado, codEntrega, barrcodi) {
    $.ajax({
        type: 'POST',
        url: controler + "lista",
        data: { emprcodi: emprcodi, equicodi: equicodi, ptomedicodi: ptomedicodi, fechaInicio: fechaInicio, fechaFin: fechaFin, estado: estado, codentrega: codEntrega, barrcodi: barrcodi},
        success: function (evt) {
            $('#listado').html(evt);
            addDeleteEvent();
            viewEvent();
            oTable = $('#tabla').dataTable({
                "sPaginationType": "full_numbers",
                "destroy": "true",
                "ordering": false
            });
        },
        error: function () {
            alert("Lo sentimos, ha ocurrido un error inesperado");
        }
    });
};

buscarFiltro = function () {
    if ($('#Activo').is(':checked')) {
        estado = 'A';
    }
    if ($('#Inactivo').is(':checked')) {
        estado = 'I';
    }
    if ($('#Todos').is(':checked')) {
        estado = 'TODOS';
    }
    var emprcodi = $("#empresaSelect").val();
    var equicodi = $("#centralSelect").val();
    var barrcodi = $("#barraSelect").val();
    var fechaInicio = $('#txtfechaIni').val();
    var fechaFin = $('#txtfechaFin').val();
    var codEntrega = $('#txtcodigoEntrega').val();
    buscar(emprcodi, equicodi, -2, fechaInicio, fechaFin, estado, codEntrega, barrcodi);
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

nuevo = function () {
    window.location.href = controler + "new";
}

addAntiForgeryToken = function (data) {
    data.__RequestVerificationToken = $('input[name=__RequestVerificationToken]').val();
    return data;
};

function viewEvent() {
   
    $('.view').click(function () {
        id = $(this).attr("id").split("_")[1];
        abrirPopup(id);
    });
};

abrirPopup = function (id) {

    $.ajax({
        type: 'POST',
        url: controler + "View/" + id,
        success: function (evt) {
            $('#popup').html(evt);

            setTimeout(function () {
                $('#popup').bPopup({
                    easing: 'easeOutBack',
                    speed: 450,
                    transition: 'slideDown'
                });
            }, 50);
        },
        error: function () {
            alert("Lo sentimos, ha ocurrido un error inesperado");
        }
    });
}

function Edit(rercencodi) {
    window.location.href = controler + "Edit?id=" + rercencodi;
}

generarExcel = function () {
    $.ajax({
        type: 'POST',
        url: controler + 'generarexcel',
        dataType: 'json',
        success: function (aData) {
            if (aData.Resultado == "1") {
                window.location = controler + 'abrirexcel';
            }
            else {
                alert(aData.Mensaje);
            }
        },
        error: function () {
            alert("Lo sentimos, se ha producido un error");
        }
    });
}

//Función que se activa cuando se cambia de empresa
function cargarEquipos() {
    limpiarCombo('centralSelect');

    // Limpiamos la información LVTEA
    limpiarCombo('codEntregaSelect');
    $('#barrTransferencia').val("");

    // Limpiamos la información LVTP
    limpiarCombo('centralUnidadSelect');
    limpiarCombo('cargoPrimaRERSelect');
    limpiarTabla("tablaBodyCentralUnidad");

    // Limpiamos la información PMPO
    limpiarCombo('centralPMPOSelect');
    limpiarCombo('barraPMPOSelect');
    limpiarTabla("tablaBodyCentralPMPO");

    var emprcodi = $('#empresaSelect').val();
    if (emprcodi != -1) {
        $.ajax({
            url: controler + 'ObtenerCentral?emprcodi=' + emprcodi,
            type: 'GET',
            success: function (response) {
                $('#Rercenfechainicio').val();
                $('#Rercenfechafin').val();
                var ListCentral = response.ListCentralGeneracion;

                // Listamos las centrales
                listarEnCombo(ListCentral, 'centralSelect', 'CentGeneCodi', 'CentGeneNombre')

            },
            error: function (error) {
                console.log(error);
            }
        });
    }

}

// Función que se activa cuando se cambia de central
function cargarCodEntrega() {
    // Limpiando información LVTEA
    limpiarCombo('codEntregaSelect');
    $('#barrTransferencia').val("");    //Limpiando lo que se muestra en barra transferencia

    var emprcodi = $('#empresaSelect').val();
    var equicodi = $('#centralSelect').val();
    if (equicodi != -1) {
        $.ajax({
            url: controler + 'ObtenerCodEntrega?emprcodi=' + emprcodi + '&equicodi=' + equicodi,
            type: 'GET',
            success: function (response) {
                var ListaCodigoEntrega = response.ListaCodigoEntrega;
                listarEnCombo(ListaCodigoEntrega, 'codEntregaSelect', 'CodiEntrCodi', 'CodiEntrCodigo')

                if (response.ListCentralRER.length == 0) {
                    //Caso que no exista un par Empresa-Central
                } else {
                    // Caso que ya exista un par Empresa-Central
                    $('#Rercenfechainicio').val(response.Rercenfechainicio);
                    cambiarFechaFin();
                    limpiarCombo('centralPMPOSelect');
                    limpiarCombo('barraPMPOSelect');
                }

            },
            error: function (error) {
                console.log(error);
            }
        });
    }
    
}

// Lista segun "listaDatos" en "comboSelect" con valores "valor" y textos "texto"
function listarEnCombo(listaDatos, comboSelect, valor, texto) {
    var listaDesplegable = document.getElementById(comboSelect);
    listaDesplegable.innerHTML = ''; // Limpiar opciones anteriores
    listaDesplegable.appendChild(crearOptionDefault()); // Crear la opción default
    if (listaDatos !== null) {
        listaDatos.forEach(function (codigoEntrega) {
            var option = document.createElement('option');
            option.text = codigoEntrega[texto];
            option.value = codigoEntrega[valor];
            listaDesplegable.appendChild(option);
        });
    }
}

// Lista segun "listaDatos" en "comboSelect" con valores "valor" y textos "texto"
function listarEnComboId(listaDatos, comboSelect, valor, texto) {
    var listaDesplegable = document.getElementById(comboSelect);
    listaDesplegable.innerHTML = ''; // Limpiar opciones anteriores
    listaDesplegable.appendChild(crearOptionDefault()); // Crear la opción default
    if (listaDatos !== null) {
        listaDatos.forEach(function (codigoEntrega) {
            var option = document.createElement('option');
            option.text = '[' + codigoEntrega[valor] + '] ' + codigoEntrega[texto];
            option.value = codigoEntrega[valor];
            listaDesplegable.appendChild(option);
        });
    }
}

function listarEnComboSinLimpiarDatos(listaDatos, comboSelect, valor, texto) {
    var listaDesplegable = document.getElementById(comboSelect);

    // Obtener el valor actual seleccionado antes de la actualización
    var valorSeleccionado = listaDesplegable.value;

    // Encontrar el índice del elemento con valor igual a valorSeleccionado
    var indexToRemove = listaDatos.findIndex(function (item) {
        return item[valor] === valorSeleccionado;
    });

    // Si se encuentra el elemento, eliminarlo de listaDatos
    if (indexToRemove !== -1) {
        listaDatos.splice(indexToRemove, 1);
    }

    // Limpiar solo las opciones que fueron añadidas dinámicamente, no la primera opción
    while (listaDesplegable.children.length > 1) {
        listaDesplegable.removeChild(listaDesplegable.lastChild);
    }

    if (listaDatos !== null) {
        listaDatos.forEach(function (codigoEntrega) {
            var option = document.createElement('option');
            option.text = codigoEntrega[texto];
            option.value = codigoEntrega[valor];
            listaDesplegable.appendChild(option);
        });
    }

    if (valorSeleccionado != '') {
        // Restaurar el valor seleccionado después de la actualización
        listaDesplegable.value = valorSeleccionado;
    }

}

function limpiarTabla(idTablaBody) {
    var tbody = document.getElementById(idTablaBody);
    while (tbody.firstChild) {
        tbody.removeChild(tbody.firstChild);
    }
}

function limpiarCombo(comboSelect) {
    var listaDesplegable = document.getElementById(comboSelect);
    listaDesplegable.innerHTML = ''; // Limpiar opciones anteriores
}

function cargarBarrTransferencia() {
    var emprcodi = $('#empresaSelect').val();
    var equicodi = $('#centralSelect').val();
    var codiEntrCodi = $('#codEntregaSelect').val();
    $.ajax({
        url: controler + 'ObtenerBarrTransferencia?emprcodi=' + emprcodi + '&equicodi=' + equicodi + '&codiEntrCodi=' + codiEntrCodi,
        type: 'GET',
        success: function (response) {
            var barrTransferencia = document.getElementById('barrTransferencia');
            //barrTransferencia.innerHTML = ''; // Limpiar opciones anteriores
            barrTransferencia.value = response.CodigoEntrega.BarrNombBarrTran;
        },
        error: function (error) {
            console.log(error);
        }
    });
}


mostrarExitoOperacion = function () {
    $('#mensaje').removeClass("action-error");
    $('#mensaje').addClass("action-exito");
    $('#mensaje').text("La operación se realizó con exito...!");
    $('#mensaje').css("display", "block");
};

mostrarErrorConMensaje = function (msg) {
    $('#mensaje').removeClass();
    $('#mensaje').html(msg);
    $('#mensaje').addClass("action-error");
};
mostrarError = function () {
    $('#mensaje').removeClass("action-exito");
    $('#mensaje').addClass("action-error");
    $('#mensaje').text("Ha ocurrido un error");
    $('#mensaje').css("display", "block");
};

function validarNumeroDecimales(numero) {
    var regex = /^\d+(\.\d{1,4})?$/;
    return regex.test(numero);
}

function cambiarFechaFin() {
    var fechaInicio = $('#Rercenfechainicio').val();
    var nuevaFecha = sumarAnios(fechaInicio, 20);
    $('#Rercenfechafin').val(nuevaFecha);
}

function sumarAnios(fecha, anios) {
    var partesFecha = fecha.split('/');
    var dia = parseInt(partesFecha[0]);
    var mes = parseInt(partesFecha[1]) - 1; // Restar 1 al mes, ya que en JavaScript los meses van de 0 a 11
    var anio = parseInt(partesFecha[2]);

    var fechaInicial = new Date(anio, mes, dia);
    fechaInicial.setFullYear(anio + anios);

    // Resta un día
    fechaInicial.setDate(fechaInicial.getDate() - 1);

    // Obtener la nueva fecha en formato dd/mm/yyyy
    var nuevaFecha = fechaInicial.getDate().toString().padStart(2, '0') + '/' +
        (fechaInicial.getMonth() + 1).toString().padStart(2, '0') + '/' +
        fechaInicial.getFullYear();

    return nuevaFecha;

}

nuevaCentral = function () {

    var mensajeError = validarDatos();
    if (mensajeError != "") {
        mostrarErrorConMensaje(mensajeError);
    }
    else {
        //Obtenemos los codigos de CentralUnidad y lo almacenamos en un hidden "codigosLVTEA"
        almacenarCodigosCentrales("tablaBodyCentralUnidad", 'codigosLVTEA', 'N');

        //Obtenemos los codigos de CentralPMPO y lo almacenamos en un hidden "codigosPMPO"
        almacenarCodigosCentrales("tablaBodyCentralPMPO", 'codigosPMPO', 'N');
        $.ajax({
            type: 'POST',
            url: controler + 'save',
            dataType: 'json',
            data: $('#frmNewCentralRER').serialize(),
            cache: false,
            success: function (aData) {
                if (aData.Resultado == "1") {
                    mostrarExitoOperacion();
                    window.location.href = controler + "index";
                } else if (aData.Resultado == "-2") {
                    mostrarErrorConMensaje(aData.MensajeError);
                } else
                    mostrarError("Lo sentimos, ha ocurrido un error");
            },
            error: function () {
                mostrarError("Lo sentimos, ha ocurrido un error");
            }
        });
    }
};

update = function () {
    if (confirm('¿Está seguro que desea actualizar la Central RER?')) {

        //Obtenemos los codigos de CentralUnidad y lo almacenamos en un hidden "codigosLVTEA"
        almacenarCodigosCentrales("tablaBodyCentralUnidad", 'codigosLVTEA', 'E');

        //Obtenemos los codigos de CentralPMPO y lo almacenamos en un hidden "codigosPMPO"
        almacenarCodigosCentrales("tablaBodyCentralPMPO", 'codigosPMPO', 'E');

        $.ajax({
            type: 'POST',
            url: controler + 'update',
            dataType: 'json',
            data: $('#frmEditCentralRER').serialize(),
            cache: false,
            success: function (aData) {
                if (aData.Resultado == "1") {
                    mostrarExitoOperacion();
                    window.location.href = controler + "index";
                } else if (aData.Resultado == "-2") {
                    mostrarErrorConMensaje(aData.MensajeError);
                } else
                    mostrarError("Lo sentimos, ha ocurrido un error");
            },
            error: function () {
                mostrarError("Lo sentimos, ha ocurrido un error");
            }
        });
        

    }
};

function crearOptionDefault() {
    var optionDefault = document.createElement('option');
    optionDefault.text = "--Seleccione--";
    optionDefault.value = -1;
    return optionDefault;
}

//Agregar CentralesUnidades a la tabla
function agregarCentralUnidad() {
    var select = document.getElementById("centralUnidadSelect");
    var valorSeleccionado = select.value;
    var textoSeleccionado = select.options[select.selectedIndex].text;

    var tablaBody = document.getElementById("tablaBodyCentralUnidad");

    // Verificar si el valor ya existe en la tabla
    var elementosTabla = capturarElementosTabla("tablaBodyCentralUnidad");
    var existeValor = elementosTabla.some(function (elemento) {
        return elemento[1] === valorSeleccionado;
    });

    if (existeValor) {
        alert("El elemento seleccionado ya existe en la tabla 'Central/Unidad'.");
    } else if (valorSeleccionado === "-1"){
        alert("No se puede agregar la opción '--Seleccione--' ");
    } else {

        var nuevaFila = tablaBody.insertRow();
        var celdaBotonEliminar = nuevaFila.insertCell(0);
        var celdaOculta = nuevaFila.insertCell(1);
        var celdaTexto = nuevaFila.insertCell(2);

        celdaOculta.innerHTML = valorSeleccionado;
        celdaTexto.innerHTML = textoSeleccionado;
        celdaOculta.style.display = "none";

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

//Agregar CentralesUnidades a la tabla
function agregarCentralPMPO() {
    var select = document.getElementById("centralPMPOSelect");
    var valorSeleccionado = select.value;
    var textoSeleccionado = select.options[select.selectedIndex].text;

    var tablaBody = document.getElementById("tablaBodyCentralPMPO");

    // Verificar si el valor ya existe en la tabla
    var elementosTabla = capturarElementosTabla("tablaBodyCentralPMPO");
    var existeValor = elementosTabla.some(function (elemento) {
        return elemento[1] === valorSeleccionado;
    });

    if (existeValor) {
        alert("El elemento seleccionado ya existe en la tabla 'Central'.");
    } else if (valorSeleccionado === "-1") {
        alert("No se puede agregar la opción '--Seleccione--' ");
    } else {

        var nuevaFila = tablaBody.insertRow();
        var celdaBotonEliminar = nuevaFila.insertCell(0);
        var celdaOculta = nuevaFila.insertCell(1);
        var celdaTexto = nuevaFila.insertCell(2);

        celdaOculta.innerHTML = valorSeleccionado;
        celdaTexto.innerHTML = textoSeleccionado;
        celdaOculta.style.display = "none";

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
        celdaBotonEliminar.style.width = "30px";
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

//N:Nuevo , E: Editar
function almacenarCodigosCentrales(tablaBody, nombreCodigosLVTEA, estado) {
    var valoresSegundaColumna = '0';
    var tablaCentralUnidad = capturarElementosTabla(tablaBody);
    for (var i = 0; i < tablaCentralUnidad.length; i++) {
        var codigo = tablaCentralUnidad[i][1]
        valoresSegundaColumna += "-" + codigo;
    }
    var inputCentralUnidad = document.createElement('input');
    inputCentralUnidad.type = 'hidden';
    inputCentralUnidad.name = nombreCodigosLVTEA;
    inputCentralUnidad.value = valoresSegundaColumna;
    if (estado == 'N') {
        document.getElementById('frmNewCentralRER').appendChild(inputCentralUnidad);
    } else if (estado == 'E') {
        document.getElementById('frmEditCentralRER').appendChild(inputCentralUnidad);
    }
}

function validarDatos() {
    var mensajeError = "";
    var campos = [
        { nombre: "Empresa generadora", valor: $('#empresaSelect').val() },
        { nombre: "Central", valor: $("#centralSelect").val() },
        { nombre: "Fecha de inicio", valor: $('#txtfechaInicio').val() },
        { nombre: "Fecha fin", valor: $('#txtfechaFin').val() },
        { nombre: "Energía Adjudicada", valor: $('#Rercenenergadj').val() },
        { nombre: "Precio Base", valor: $('#Rercenprecbase').val() },
        { nombre: "Inflación Base", valor: $('#Rerceninflabase').val() },
        { nombre: "Código de entrega", valor: $('#codEntregaSelect').val() }
    ];

    var camposConDecimales = [
        { nombre: "Energía Adjudicada", valor: $('#Rercenenergadj').val() },
        { nombre: "Precio Base", valor: $('#Rercenprecbase').val() },
        { nombre: "Inflación Base", valor: $('#Rerceninflabase').val() }
    ];

    for (var i = 0; i < campos.length; i++) {
        var campo = campos[i];
        if (campo.valor === "" || campo.valor === null || campo.valor == -1) {
            mensajeError += "El campo \"" + campo.nombre + "\" no fue seleccionado. <br>"
        }
    }

    var regex = /^\d+(\.\d{1,4})?$/;
    for (var i = 0; i < camposConDecimales.length; i++) {
        if (camposConDecimales[i].valor !== "" && !regex.test(camposConDecimales[i].valor)) {
            mensajeError += "El campo \"" + camposConDecimales[i].nombre + "\" posee más de 4 decimales. <br>"
        }
    }
    return mensajeError;
}

function CargarInformacionLVTP() {
    var emprcodi = $('#empresaSelect').val();

    if (emprcodi != -1) {
        $.ajax({
            url: controler + 'ObtenerInformacionLVTP?emprcodi=' + emprcodi,
            type: 'GET',
            success: function (aData) {
                if (aData.Resultado == "1") {
                    var ListCentralUnidad = aData.ListCentralUnidadLVTP;        // Central/Unidad
                    var ListCargoPrimaRER = aData.ListVtpPeajeIngreso;      // Cargo Prima RER

                    listarEnCombo(ListCentralUnidad, 'centralUnidadSelect', 'Cenequicodi', 'Ipefrdunidadnomb');
                    listarEnComboSinLimpiarDatos(ListCargoPrimaRER, 'cargoPrimaRERSelect', 'Pingnombre', 'Pingnombre');
                } else
                    alert("Lo sentimos, ha ocurrido un error");
            },
            error: function () {
                mostrarError("Lo sentimos, ha ocurrido un error");
            }
        });
    } else {
        alert("Primero debe seleccionar la \"Empresa\"");
    }

}

function CargarInformacionPMPO() {
    var emprcodi = $('#empresaSelect').val();
    var fechaInicial = $('#Rercenfechainicio').val();
    var fechaFinal = $('#Rercenfechafin').val();
    var ptomediCodi = $('#ptomediCodiSelect').val();

    $("#textoBarraNew").removeAttr('style');
    $("#textoBarraEdit").removeAttr('style');

    if (emprcodi != -1 && fechaInicial != "" && fechaFinal != "") {
        $.ajax({
            url: controler + 'ObtenerInformacionPMPO?emprcodi=' + emprcodi + '&fechaInicial=' + fechaInicial + ' &fechaFinal=' + fechaFinal + '&ptomediCodi=' + ptomediCodi,
            type: 'GET',
            success: function (aData) {
                if (aData.Resultado == "1") {
                    var ListCentralPMPO = aData.ListCentralMedicion;          // Central PMPO
                    var ListBarraPMPO = aData.ListBarraMedicion;            // Barra PMPO
                    if (ListBarraPMPO == null || ListBarraPMPO.length < 1) {
                        $("#textoBarraNew").attr('style', 'font-size: 12px; display: inline-block; color: red;');
                        $("#textoBarraNew").html("No existen barras para el rango de fecha de contrato ingresada");
                        $("#textoBarraEdit").attr('style', 'font-size: 12px; display: inline-block; color: red;');
                        $("#textoBarraEdit").html("No existen barras para el rango de fecha de contrato ingresada");
                    }
                    listarEnCombo(ListCentralPMPO, 'centralPMPOSelect', 'Ptomedicodi', 'Ptomedidesc')
                    //ASSETEC - 18.10.2023
                    //listarEnComboId(ListBarraPMPO, 'barraPMPOSelect', 'Ptomedicodi', 'Ptomedidesc')
                    RefillDropDowListConcat($('#barraPMPOSelect'), ListBarraPMPO, 'Ptomedicodi', 'Ptomedidesc');
                    //ASSETEC - 18.10.2023
                } else
                    alert("Lo sentimos, ha ocurrido un error");
            },
            error: function () {
                mostrarError("Lo sentimos, ha ocurrido un error");
            }
        });
    }
    else {
        alert("Primero debe seleccionar la \"Empresa\", \"Fecha de inicio\" y \"Fecha fin\"");
    }

}

//ASSETEC - 18.10.2023
function RefillDropDowListConcat(element, data, data_id, data_name) {
    //Vacia el elemento
    element.empty();
    //Carga el elemento
    $.each(data, function (i, item) {
        var n_value = i, n_html = item;
        if (data_id) n_value = item[data_id];
        if (data_name) n_html = item[data_name];
        element.append($('<option></option>').val(n_value).html("[" + n_value + "] " + n_html));

        // Guarda el valor del primer elemento
        if (i === 0) {
            primerElementoValor = n_value;
        }
    });

    // Restaura la opción actual si existía
    //element.val(currentValue);

    //Actualiza la lib.multipleselect
    element.multipleSelect('refresh');

    // Selecciona el primer elemento en $('#barraPMPOSelect').multipleSelect
    if (primerElementoValor !== null) {
        $('#barraPMPOSelect').multipleSelect('setSelects', [primerElementoValor]);
    }
}
//ASSETEC - 18.10.2023

////ASSETEC - 18.10.2023
//function RefillDropDowListConcat(element, data, data_id, data_name) {
//    //Vacia el elemento
//    element.empty();
//    //Carga el elemento
//    $.each(data, function (i, item) {
//        var n_value = i, n_html = item;
//        if (data_id) n_value = item[data_id];
//        if (data_name) n_html = item[data_name];
//        element.append($('<option></option>').val(n_value).html("[" + n_value + "] " + n_html));
//    });
//    //Actualiza la lib.multipleselect
//    element.multipleSelect('refresh');
//}
////ASSETEC - 18.10.2023

