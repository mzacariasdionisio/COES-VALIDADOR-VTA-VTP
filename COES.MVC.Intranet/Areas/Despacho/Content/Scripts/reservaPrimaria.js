var controlador = siteRoot + 'Despacho/Reserva/';

var OPCION_NUEVO = 1;
var OPCION_EDITAR = 2;
var OPCION_ACTUAL = 0;

var anchoTabla;

var listaErroresGeneral = [];
var listaErroresSubir = [];
var datosGeneral = [];

var containerTabla;
var tblHndSubir;

var IMG_EDITAR = `<img src="${siteRoot}Content/Images/btn-edit.png" title="Editar RPF"/>`;
var IMG_ELIMINAR = `<img src="${siteRoot}Content/Images/Trash.png" title="Eliminar RPF"/>`;

$(function () {

    $('#tab-container').easytabs({
        animate: false
    });

    $('#tab-container').easytabs('select', '#tabListado');

    $('#txtFechaConsulta').Zebra_DatePicker({
        onSelect: function () {
        }
    });

    $('#txtFechaVigencia').Zebra_DatePicker({
        onSelect: function () {
        }
    });

    $('#btnBuscar').click(function () {
        mostrarListadoRPF();
    });

    $('#btnNuevo').click(function () {
        mantenerRPF();
    });

    //tab2
    $('#btnErrores').click(function () {
        mostrarListadoErrores();
    });

    $('#btnGuardar').click(function () {
        guardarRPF();
    });

    mostrarListadoRPF();
});

function mostrarListadoRPF() {

    var fecha = $("#txtFechaConsulta").val();

    limpiarBarraMensaje('mensaje_Listado');
    $.ajax({
        type: 'POST',
        url: controlador + "ListarRPF",
        dataType: 'json',
        data: {
            fechaConsulta: fecha,
        },
        success: function (evt) {
            if (evt.Resultado != "-1") {

                //ocultar detalles
                $("#idFiltrosDet").hide();
                $("#tblRPF").hide();
                $('#btnGuardar').css("display", "none");

                $('#tab-container').easytabs('select', '#tabListado');
                $('#listado').html('');
                $('#listado').css("width", ($('#mainLayout').width() - 40) + "px");
                var html = _dibujarTablaListado(evt);
                $('#listado').html(html);

                $('#TablaConsultaRPF').dataTable({
                    "scrollY": 430,
                    "scrollX": true,
                    "scrollCollapse": false,
                    "sDom": 't',
                    "ordering": false,
                    "paging": false
                });

                viewEvent();

            } else {
                alert(evt.Mensaje);
            }
        },
        error: function (err) {
            alert('Error: Se ha producido un error inesperado.');
        }
    });
}

function _dibujarTablaListado(model) {
    var lista = model.ListaReserva;

    var cadena = '';
    cadena += `
    <table class="pretty tabla-icono" id="TablaConsultaRPF" style='table-layout: fixed;' >
        <thead>
            <tr>
                <th>Acciones</th>
                <th style="width: 80px">Estado</th>
                <th style="width: 100px">Fecha de vigencia</th>
                <th style="width: 120px">Usuario modificación</th>
                <th style="width: 120px">Fecha modificación</th>`;

    for (let i = 0; i < 24; i++) {
        const hora = i < 10 ? '0' + i : i;
        var cont = i + 1;
        if (cont == 24) cont = 0;
        const hora2 = cont < 10 ? '0' + cont : cont;
        cadena += ` <th style="width: 40px">${hora}:30</th>`;
        cadena += ` <th style="width: 40px">${hora2}:00</th>`;
    }
    cadena += `
            </tr>
        </thead>
        <tbody>
    `;


    var contador = 0;
    for (key in lista) {
        var item = lista[key];

        //
        var sStyle = "";
        if (item.Prsvactivo == 0) sStyle = "background-color:#FFDDDD;"; // eliminado
        var tdOpciones = _tdAcciones(model, item, sStyle, contador);

        cadena += `
            <tr>
                ${tdOpciones}
                <td style="text-align:left; ${sStyle}">${item.EstadoReserva}</td>
                <td style="text-align:left; ${sStyle}">${item.PrsvfechavigenciaDesc}</td>
                <td style="text-align:left; ${sStyle}">${item.Prsvusumodificacion}</td>
                <td style="text-align:left; ${sStyle}">${item.PrsvfecmodificacionDesc}</td>
                `;

        var lstDatos = item.Prsvdato.split(';');
        for (var key in lstDatos) {
            var item2 = lstDatos[key];
            cadena += `
            <td style="text-align:left; ${sStyle}">${item2}</td>
            `;
        }

        cadena += `</tr>`;
        contador++;
    }
    cadena += "</tbody></table>";

    return cadena;
}

function _tdAcciones(Model, item, sStyle, contador) {
    var html = '';

    html += `<td style="text-align: left; ${sStyle}">`;

    if (item.Prsvactivo != 0) {
        html += `   
                <a href="#" id="view,${item.PrsvfechavigenciaDesc}" class="viewEdicion">
                    ${IMG_EDITAR}
                </a>
                <a href="#" id="view,${item.PrsvfechavigenciaDesc}" class="viewEliminacion">
                    ${IMG_ELIMINAR}
                </a>
                </td>
        `;
    }
    else {
        html += `  </td>
        `;
    }

    return html;
}

function viewEvent() {
    $('.viewEdicion').click(function (event) {
        event.preventDefault();
        var fechavig = $(this).attr("id").split(",")[1];
        mantenerRPF(fechavig, true);
    });
    $('.viewEliminacion').click(function (event) {
        event.preventDefault();
        fechavig = $(this).attr("id").split(",")[1];
        eliminarRPF(fechavig);
    });
};

function mantenerRPF(fechavig, editable) {

    limpiarBarraMensaje("mensaje_rpf");
    $('#tab-container').easytabs('select', '#tabDetalle');


    if (editable) {

        OPCION_ACTUAL = OPCION_EDITAR;
        //cargo handson
        cargarDatosReserva(fechavig);
    }
    else {
        OPCION_ACTUAL = OPCION_NUEVO;
        nuevoRegistro();
    }
}

function nuevoRegistro() {
    var data = null;

    inicializarReserva();
    iniciarHandsontables();

}
function cargarDatosReserva(fechavig) {

    limpiarBarraMensaje("mensaje_rpf");
    var fecha = $("#txtFechaConsulta").val();

    $.ajax({
        type: 'POST',
        url: controlador + "CargarDatosRPF",
        data: {
            fechaVigencia: fechavig,
            fechaConsulta: fecha
        },
        success: function (evt) {

            if (evt.Resultado == "1") {

                var data = evt.Entidad;

                inicializarReserva(evt.Entidad);
                iniciarHandsontables();

                cargarHansonTablas(data);

            } else {
                mostrarMensaje('mensaje_rpf', 'error', evt.Mensaje);
            }
        },
        error: function (xhr, status) {
            mostrarMensaje('mensaje_rpf', 'error', 'Se ha producido un error.');
        }
    });

}

function cargarHansonTablas(datosEnBD) {
    tblHndSubir.loadData([]);

    cargarDataAHanson(datosEnBD);
    listaErroresGeneral = [];
}

function cargarDataAHanson(datosEnBd) {
    var tablaHandson;
    var datoshandson = [];
    tablaHandson = tblHndSubir;

    datoshandson = datosEnBd;

    var lstData;

    lstData = [
        { H1: '' },
        { H1: '00:30', H2: '01:00', H3: '01:30', H4: '02:00', H5: '02:30', H6: '03:00', H7: '03:30', H8: '04:00', H9: '04:30', H10: '05:00', H11: '05:30', H12: '06:00', H13: '06:30', H14: '07:00', H15: '07:30', H16: '08:00', H17: '08:30', H18: '09:00', H19: '09:30', H20: '10:00', H21: '10:30', H22: '11:00', H23: '11:30', H24: '12:00', H25: '12:30', H26: '13:00', H27: '13:30', H28: '14:00', H29: '14:30', H30: '15:00', H31: '15:30', H32: '16:00', H33: '16:30', H34: '17:00', H35: '17:30', H36: '18:00', H37: '18:30', H38: '19:00', H39: '19:30', H40: '20:00', H41: '20:30', H42: '21:00', H43: '21:30', H44: '22:00', H45: '22:30', H46: '23:00', H47: '23:30', H48: '00:00' },
        {
            H1: datoshandson.H1, H2: datoshandson.H2, H3: datoshandson.H3, H4: datoshandson.H4, H5: datoshandson.H5, H6: datoshandson.H6, H7: datoshandson.H7, H8: datoshandson.H8,
            H9: datoshandson.H9, H10: datoshandson.H10, H11: datoshandson.H11, H12: datoshandson.H12, H13: datoshandson.H13, H14: datoshandson.H14, H15: datoshandson.H15, H16: datoshandson.H16,
            H17: datoshandson.H17, H18: datoshandson.H18, H19: datoshandson.H19, H20: datoshandson.H20, H21: datoshandson.H21, H22: datoshandson.H22, H23: datoshandson.H23, H24: datoshandson.H24,
            H25: datoshandson.H25, H26: datoshandson.H26, H27: datoshandson.H27, H28: datoshandson.H28, H29: datoshandson.H29, H30: datoshandson.H30, H31: datoshandson.H31, H32: datoshandson.H32,
            H33: datoshandson.H33, H34: datoshandson.H34, H35: datoshandson.H35, H36: datoshandson.H36, H37: datoshandson.H37, H38: datoshandson.H38, H39: datoshandson.H39, H40: datoshandson.H40,
            H41: datoshandson.H41, H42: datoshandson.H42, H43: datoshandson.H43, H44: datoshandson.H44, H45: datoshandson.H45, H46: datoshandson.H46, H47: datoshandson.H47, H48: datoshandson.H48
        }
    ];

    tablaHandson.loadData(lstData);
}

function inicializarReserva(data) {

    listaErroresGeneral = [];
    listaErroresSubir = [];
    datosGeneral = [];
    //datosRpfSubir = [];

    $("#idFiltrosDet").show();
    $("#tblRPF").show();

    if (OPCION_ACTUAL == OPCION_NUEVO) {
        $(".label_filtro").hide();

        var fechaActual = $("#hdFechaActual").val();
        $("#txtFechaVigencia").val(fechaActual);

        $("#txtFechaVigencia").removeAttr('disabled')
        $("#txtFechaVigencia").parent().find("button").prop("class", "Zebra_DatePicker_Icon Zebra_DatePicker_Icon_Inside");
        //$("#btnNuevo").hide();
    }

    if (OPCION_ACTUAL == OPCION_EDITAR) {
        $(".label_filtro").show();

        var result = data.PrsvfechavigenciaDesc.slice(0, -9);

        $("#txtFechaVigencia").val(result);
        $("#txtEstado").html(data.EstadoReserva);
        $("#txtUsumodif").html(data.Prsvusumodificacion);
        $("#txtFechmodif").html(data.PrsvfecmodificacionDesc);

        $("#txtFechaVigencia").prop('disabled', 'disabled');
        $("#txtFechaVigencia").parent().find("button").prop("class", "Zebra_DatePicker_Icon Zebra_DatePicker_Icon_Disabled Zebra_DatePicker_Icon_Inside");
    }

    $('#btnGuardar').css("display", "block");
}

function iniciarHandsontables() {

    // Inicio datos
    Handsontable.renderers.registerRenderer('datosIngresadosRpfRenderer', datosIngresadosRpfRenderer);

    // #region Handsontable
    containerTabla = document.getElementById('tblRPF');

    tblHndSubir = new Handsontable(containerTabla, {
        data: [
            { H1: '' },
            { H1: '00:30', H2: '01:00', H3: '01:30', H4: '02:00', H5: '02:30', H6: '03:00', H7: '03:30', H8: '04:00', H9: '04:30', H10: '05:00', H11: '05:30', H12: '06:00', H13: '06:30', H14: '07:00', H15: '07:30', H16: '08:00', H17: '08:30', H18: '09:00', H19: '09:30', H20: '10:00', H21: '10:30', H22: '11:00', H23: '11:30', H24: '12:00', H25: '12:30', H26: '13:00', H27: '13:30', H28: '14:00', H29: '14:30', H30: '15:00', H31: '15:30', H32: '16:00', H33: '16:30', H34: '17:00', H35: '17:30', H36: '18:00', H37: '18:30', H38: '19:00', H39: '19:30', H40: '20:00', H41: '20:30', H42: '21:00', H43: '21:30', H44: '22:00', H45: '22:30', H46: '23:00', H47: '23:30', H48: '00:00' },
            { H1: '', H2: '', H3: '', H4: '', H5: '', H6: '', H7: '', H8: '', H9: '', H10: '', H11: '', H12: '', H13: '', H14: '', H15: '', H16: '', H17: '', H18: '', H19: '', H20: '', H21: '', H22: '', H23: '', H24: '', H25: '', H26: '', H27: '', H28: '', H29: '', H30: '', H31: '', H32: '', H33: '', H34: '', H35: '', H36: '', H37: '', H38: '', H39: '', H40: '', H41: '', H42: '', H43: '', H44: '', H45: '', H46: '', H47: '', H48: '' }
        ],
        dataSchema: {
            H1: null, H2: null, H3: null, H4: null, H5: null, H6: null, H7: null, H8: null, H9: null, H10: null,
            H11: null, H12: null, H13: null, H14: null, H15: null, H16: null, H17: null, H18: null, H19: null, H20: null,
            H21: null, H22: null, H23: null, H24: null, H25: null, H26: null, H27: null, H28: null, H29: null, H30: null,
            H31: null, H32: null, H33: null, H34: null, H35: null, H36: null, H37: null, H38: null, H39: null, H40: null,
            H41: null, H42: null, H43: null, H44: null, H45: null, H46: null, H47: null, H48: null
        },
        mergeCells: [
            { row: 0, col: 0, rowspan: 1, colspan: 48 }
        ],
        hiddenRows: {
            // specify rows hidden by default
            rows: [0]
        },
        colWidths: 55,
        height: '160px',
        width: '100%',

    });

    document.body.addEventListener('dblclick', function (e) {
        var edit = tblHndSubir.getActiveEditor();
        var item = tblHndSubir.getSelected()
        var startRow = edit.cellProperties.row;
        var startCol = edit.cellProperties.col;
        tblHndSubir.selectCell(startRow, startCol);
        tblHndSubir.getActiveEditor().enableFullEditMode();
        tblHndSubir.getActiveEditor().beginEditing(tblHndSubir.getDataAtCell(startRow, startCol));
    })

    tblHndSubir.addHook('beforeChange', function (changes, [source]) {
        listaErroresSubir = [];
        datosGeneral = [];
        //datosRpfSubir = [];
    });

    tblHndSubir.addHook('afterInit', validaRangosRsf);
    tblHndSubir.init();

    tblHndSubir.addHook('afterChange', function (changes, [source]) {
        listaErroresGeneral = [];

        validaRangosRsf();

        //Obtengo lista Errores General
        listaErroresGeneral = listaErroresGeneral.concat(listaErroresSubir);
    });

    //valido existe de dato en RPF
    function validaRangosRsf() {
        // datosGeneral = datosRpfSubir;
        agregarValErrorSinDatos();
    }

    function agregarValErrorSinDatos() {
        if (datosGeneral.length != 48) {
            agregarError("", "", "Se debe ingresar valor numérico para todas las celdas.");//
        }
    }

    //
    tblHndSubir.updateSettings({
        cells(row, col, prop) {
            const cellProperties = {};

            switch (row) {
                case 1:
                    cellProperties.readOnly = true;
                    cellProperties.className = 'htCenter fondo_activacion_escenarios ';
                    break;

                case 2:
                    cellProperties.readOnly = false;
                    cellProperties.renderer = "datosIngresadosRpfRenderer"; // uses lookup map
                    break;
                default:
            }

            return cellProperties;
        },


    });

}

function datosIngresadosRpfRenderer(instance, td, row, col, prop, value, cellProperties) {
    Handsontable.renderers.TextRenderer.apply(this, arguments);

    datosIngresadosRenderer(instance, td, row, col, prop, value, cellProperties);
}

function datosIngresadosRenderer(instance, td, row, col, prop, value, cellProperties) {

    var numEscenario = parseInt(prop.substring(1));
    var celda = convertirAmediaHora(numEscenario);

    if (value != null) {

        //Valido si es NO numerico    
        if (isNaN(value)) {
            td.className = 'celda_error_roja htCenter';
            agregarError(celda, value, "Solo se permiten valores numéricos (con punto como separador decimal) mayores a cero.");
            if (row == 2) {
                datosGeneral.push(value);
            }
        } else {
            //valido si son negativos
            if (value < 0) {
                td.className = 'celda_error_roja htCenter';
                agregarError(celda, value, "Solo se permiten valores numéricos (con punto como separador decimal) mayores a cero.");
                if (row == 2) {
                    datosGeneral.push(value);
                }
            } else {

                if (value > 100) {
                    td.className = 'celda_error_roja htCenter';
                    agregarError(celda, value, "El valor supera al límite permitido de 100");
                    if (row == 2) {
                        datosGeneral.push(value);
                    }
                }
                else {
                    //Obtengo si el valor real ingresado tiene decimales
                    var pE = Math.trunc(value);
                    var pD;
                    var arrN = (value + "").split(".");
                    if (arrN.length > 1) {
                        pD = (value + "").replace((pE + ""), "0");
                    }
                    var pdecR = pD != undefined ? (pD + "").substring(2) : "";
                    var numPDNumReal = pdecR.length;

                    //Antes de validar lo convierto
                    //var valo1 = parseFloat(td.innerHTML);
                    var valo2 = parseFloat(value);
                    if (numPDNumReal > 0) {
                        td.innerHTML = valo2;
                    }

                    //Uso el nuevo valor (max 3 decimales) para la s validaciones
                    value = valo2;

                    //valido si son positivos pero con mas de 3 cifras enteras o mas de 1 cifra decimal
                    var parteEntera = Math.trunc(value);
                    var parteDecimal;
                    var arrNum = (value + "").split(".");
                    if (arrNum.length > 1) {
                        parteDecimal = (value + "").replace((parteEntera + ""), "0");
                    }

                    var pe = parteEntera + "";
                    var pd = parteDecimal != undefined ? (parteDecimal + "").substring(2) : "";
                    var numPE = pe.length;
                    var numPD = pd.length;

                    //Solo es aceptado si tiene hasta 3 cifras enteras y 3 cifras decimales
                    if (numPE > 3 || numPD > 3) {
                        td.className = 'celda_error_roja htCenter';
                        agregarError(celda, value, "Solo está permitido el ingreso de valores como máximo de 3 cifras decimales.");
                        if (row == 2) {
                            datosGeneral.push(value);
                        }
                    } else {
                        if (value >= 0) {
                            td.className = 'celda_azul_claro htCenter';
                            if (row == 2) {
                                datosGeneral.push(value);
                            }
                        }
                    }
                }
            }
        }
    }

}

function agregarError(celda, valor, mensajeError) {
    //Agrega al array de tipo de informacion
    if (validarError(celda, valor, mensajeError)) {
        var regError = {
            Celda: celda,
            Valor: valor,
            Mensaje: mensajeError
        };

        listaErroresSubir.push(regError);
    }
}

function validarError(celda, valor, mensajeError) {
    var arrayData = [];
    arrayData = listaErroresSubir.slice();

    for (var j in arrayData) {
        if (arrayData[j]['Celda'] == celda && arrayData[j]['Valor'] == valor && arrayData[j]['Mensaje'] == mensajeError) {
            return false;
        }
    }
    return true;
}

function mostrarListadoErrores() {
    limpiarBarraMensaje("mensaje_rpf");

    setTimeout(function () {
        $('#tablaErrores').html(dibujarTablaErrores());

        $('#contenedorErrores').bPopup({
            easing: 'easeOutBack',
            speed: 450,
            transition: 'slideDown',
            modalClose: false
        });
        $('#tablaError').dataTable({
            "scrollY": 330,
            "scrollX": true,
            "sDom": 't',
            "ordering": false,
            "bPaginate": false,
            "iDisplayLength": -1
        });

    }, 200);
}

function dibujarTablaErrores() {
    var cadena = `
        <div style='clear:both; height:5px'></div>
            <table id='tablaError' border='1' class='pretty tabla-adicional' cellspacing='0' style='width: 850px;'>
                <thead>
                    <tr>
                        <th style='width: 80px;'>Celda</th>
                        <th style='width: 140px;'>Valor</th>
                        <th style='width: 400px;'>Tipo Error</th>
                    </tr>
                </thead>
                <tbody>
    `;

    for (var i = 0; i < listaErroresGeneral.length; i++) {
        var item = listaErroresGeneral[i];
        cadena += `
                    <tr>
                        <td style='width: 120px; text-align: left; white-space: break-spaces;'>${item.Celda}</td>
                        <td style='width: 140px; text-align: left; white-space: break-spaces;'>${item.Valor}</td>
                        <td style='width: 400px; text-align: left; white-space: break-spaces;'>${item.Mensaje}</td>
                    </tr>
        `;
    }

    cadena += `
                </tbody>
            </table>
        </div>
    `;

    return cadena;
}

function guardarRPF() {
    limpiarBarraMensaje("mensaje_rpf");

    if (listaErroresGeneral.length == 0) {
        guardarReservas();
    } else {
        mostrarListadoErrores();
    }
}

function guardarReservas() {
    limpiarBarraMensaje("mensaje_rpf");

    //var dataHandsonSubir = [];
    //dataHandsonSubir = tblHndSubir.getSourceData();

    var dataGuardar = obtenerDataGuardar();
    var dataJson = {
        fechaVigencia: $("#txtFechaVigencia").val(),
        opcion: OPCION_ACTUAL,
        dataGuardar: dataGuardar
    };

    $.ajax({
        url: controlador + "GuardarRPF",
        type: 'POST',
        contentType: 'application/json; charset=UTF-8',
        dataType: 'json',
        data: JSON.stringify(dataJson),
        success: function (result) {

            if (result.Resultado == "1") {
                $("#idFiltrosDet").hide();
                $("#tblRPF").hide();
                $('#btnGuardar').css("display", "none");

                $('#tab-container').easytabs('select', '#tabListado');
                mostrarListadoRPF();
                mostrarMensaje('mensaje_Listado', 'exito', 'La información fue registrada correctamente.');

            } else {
                if (result.Resultado == "2")
                    alert("Ya existe registro con la misma fecha de vigencia.");
                else
                    mostrarMensaje('mensaje_rpf', 'error', result.Mensaje);
            }
        },
        error: function (xhr, status) {
            mostrarMensaje('mensaje_rpf', 'error', 'Se ha producido un error.');
        }
    });

}

function eliminarRPF(fechavig) {
    if (confirm("Desea eliminar el registro?")) {
        $.ajax({
            type: 'POST',
            dataType: 'json',
            traditional: true,
            url: controlador + 'EliminarRPF',
            data: {
                fechaVigencia: fechavig
            },
            cache: false,
            success: function (result) {
                if (result.Resultado == '-1') {
                    alert('Ha ocurrido un error:' + result.Mensaje);
                } else {
                    //alert("Se eliminó correctamente el registro");
                    mostrarListadoRPF();
                }
            },
            error: function (err) {
                alert('Ha ocurrido un error.');
            }
        });
    }
}

function obtenerDataGuardar() {

    var datosRpf = [];

    datosRpf = ObtenerInformacionDeHandson(2);

    var dataGuardar = datosRpf;

    //var dataGuardar = {
    //    DatosSubir: datosRpf
    //};

    return dataGuardar;
}

function ObtenerInformacionDeHandson(fila) {

    var dataHandson = [];
    dataHandson = tblHndSubir.getSourceData();

    var lstData = dataHandson[fila];

    lstData.H1 = formatearValor3Decimales(lstData.H1);
    lstData.H2 = formatearValor3Decimales(lstData.H2);
    lstData.H3 = formatearValor3Decimales(lstData.H3);
    lstData.H4 = formatearValor3Decimales(lstData.H4);
    lstData.H5 = formatearValor3Decimales(lstData.H5);
    lstData.H6 = formatearValor3Decimales(lstData.H6);
    lstData.H7 = formatearValor3Decimales(lstData.H7);
    lstData.H8 = formatearValor3Decimales(lstData.H8);
    lstData.H9 = formatearValor3Decimales(lstData.H9);

    lstData.H10 = formatearValor3Decimales(lstData.H10);
    lstData.H11 = formatearValor3Decimales(lstData.H11);
    lstData.H12 = formatearValor3Decimales(lstData.H12);
    lstData.H13 = formatearValor3Decimales(lstData.H13);
    lstData.H14 = formatearValor3Decimales(lstData.H14);
    lstData.H15 = formatearValor3Decimales(lstData.H15);
    lstData.H16 = formatearValor3Decimales(lstData.H16);
    lstData.H17 = formatearValor3Decimales(lstData.H17);
    lstData.H18 = formatearValor3Decimales(lstData.H18);
    lstData.H19 = formatearValor3Decimales(lstData.H19);

    lstData.H20 = formatearValor3Decimales(lstData.H20);
    lstData.H21 = formatearValor3Decimales(lstData.H21);
    lstData.H22 = formatearValor3Decimales(lstData.H22);
    lstData.H23 = formatearValor3Decimales(lstData.H23);
    lstData.H24 = formatearValor3Decimales(lstData.H24);
    lstData.H25 = formatearValor3Decimales(lstData.H25);
    lstData.H26 = formatearValor3Decimales(lstData.H26);
    lstData.H27 = formatearValor3Decimales(lstData.H27);
    lstData.H28 = formatearValor3Decimales(lstData.H28);
    lstData.H29 = formatearValor3Decimales(lstData.H29);

    lstData.H30 = formatearValor3Decimales(lstData.H30);
    lstData.H31 = formatearValor3Decimales(lstData.H31);
    lstData.H32 = formatearValor3Decimales(lstData.H32);
    lstData.H33 = formatearValor3Decimales(lstData.H33);
    lstData.H34 = formatearValor3Decimales(lstData.H34);
    lstData.H35 = formatearValor3Decimales(lstData.H35);
    lstData.H36 = formatearValor3Decimales(lstData.H36);
    lstData.H37 = formatearValor3Decimales(lstData.H37);
    lstData.H38 = formatearValor3Decimales(lstData.H38);
    lstData.H39 = formatearValor3Decimales(lstData.H39);

    lstData.H40 = formatearValor3Decimales(lstData.H40);
    lstData.H41 = formatearValor3Decimales(lstData.H41);
    lstData.H42 = formatearValor3Decimales(lstData.H42);
    lstData.H43 = formatearValor3Decimales(lstData.H43);
    lstData.H44 = formatearValor3Decimales(lstData.H44);
    lstData.H45 = formatearValor3Decimales(lstData.H45);
    lstData.H46 = formatearValor3Decimales(lstData.H46);
    lstData.H47 = formatearValor3Decimales(lstData.H47);
    lstData.H48 = formatearValor3Decimales(lstData.H48);

    return lstData;
}

function formatearValor3Decimales(valorFormatear) {
    var salida = "";

    if (valorFormatear != null) {
        if (isNaN(valorFormatear)) {
            if (valorFormatear.trim() != "") {
                salida = parseFloat(valorFormatear).toFixed(3).toString();
            } else {
                salida = "";
            }
        } else {
            salida = parseFloat(valorFormatear).toFixed(3).toString();
        }
    } else {
        salida = null;
    }

    return salida;
}

function convertirAmediaHora(valor) {
    // Calcular las horas y los minutos
    var horas = (valor % 2 === 1) ? Math.floor((valor - 1) / 2) : valor / 2; // Restamos 1 porque empieza en 00:30
    horas = (horas == 24) ? 0 : horas;
    const minutos = (valor % 2 === 1) ? 30 : 0; // Si es impar, serán 30 minutos

    // Formatear las horas y los minutos con 2 dígitos
    const horaFormateada = String(horas).padStart(2, '0');
    const minutosFormateados = String(minutos).padStart(2, '0');

    return `${horaFormateada}:${minutosFormateados}`;
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