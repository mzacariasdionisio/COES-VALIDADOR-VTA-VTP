var controlador = siteRoot + 'PMPO/VolumenEmbalse/';

var contenedorHt, tblHandsonVolumenCaudal, listaErrores = [], tblErroresdatos;
var HEIGHT_MINIMO = 650;

$(function () {
    //Guardar
    $("#btnGuardar").click(function () {
        guardarHandsonVolumenCaudalCmgCP($('#txtFecha').val());
    });

    //Mostrar Errores
    $("#btnMostrarErrores").click(function () {
        mostrarErrores();
    });
    tblErroresdatos = $('#tblListaerrores').DataTable({
        "data": [],
        "columns": [
            { data: "className", visible: false },
            { data: "address", className: "texto_centrado" },
            { data: "valor", className: "texto_centrado" },
            { data: "message" }
        ],
        "filter": false,
        "scrollCollapse": true,
        "paging": false,
        "scrollY": 430,
        "createdRow": function (row, data, dataIndex) {
            $(row).find('td').eq(1).addClass(data.className);
        },

    });

    //Boton Autocompletar
    $("#btnImportarF").click(function () {
        obtenerDatosParaHandsonYMostrar(true);
    });

    obtenerDatosParaHandsonYMostrar(false);

});

//////////////////////////////////////////////////////////////////////////
function obtenerDatosParaHandsonYMostrar(mostrarVolExtranet) {
    var mtopcodi = parseInt($('#hdMtopcodi').val()) || 0;

    $("#div_leyenda_cargarbd").hide();

    $.ajax({
        type: 'POST',
        url: controlador + "ObtenerHandsonVolumenEmbalse",
        data: {
            mtopcodi: mtopcodi,
            mostrarVolExtranet: mostrarVolExtranet
        },
        success: function (evt) {
            if (evt.Resultado != "-1") {
                cabecerasHandson = evt.LstCabeceras;

                //muestro handson
                mostrarHandson(evt.Handson, 48 + 4);

                //valida datos (genera lista Error) despues de mostrar tabla
                tblHandsonVolumenCaudal.validateCells();

                mostrarMensajeCargaBD(mostrarVolExtranet);
                //updateDimensionHandson(tblHandsonVolumenCaudal, document.getElementById('htTablaWeb'));
            } else {
                mostrarMensaje_('mensaje', 'error', "Ha ocurrido un error: " + evt.Mensaje);
            }
        },
        error: function (err) {
            mostrarMensaje_('mensaje', 'error', 'Ha ocurrido un error.');
        }
    });
}

function mostrarMensajeCargaBD(mostrarVolExtranet) {
    var msj = '';
    if (mostrarVolExtranet) {
        var msj = `
            Se actualizó los valores de BD. Las columnas "Volumen Total Inicial Hm3" y "Volumen Total Inicial p.u." serán calculados luego de presionar el botón "Guardar".
        `;


        $("#div_leyenda_cargarbd").html(msj);

        $("#div_leyenda_cargarbd").show();
    }

}

function mostrarHandson(hot, numFilas) {
    $("#htTablaWeb").html('');

    listaErrores = [];
    contenedorHt = document.getElementById('htTablaWeb');

    var nestedHeader = obtenerCabeceraAgrupada(hot.NestedHeader.ListCellNestedHeaders);

    tblHandsonVolumenCaudal = new Handsontable(contenedorHt, {
        data: JSON.parse(hot.ListaExcelData2),
        //colWidths: hot.ListaColWidth,
        nestedHeaders: nestedHeader,
        columns: hot.Columnas,
        mergeCells: hot.ListaMerge,
        hiddenColumns: {
            // specify columns hidden by default
            columns: [0, 1]
        },
        width: '100%',
        height: 500,
        autoWrapRow: true,
        rowHeaders: true,  //quita numeración en filas
        cell: JSON.parse(hot.Esquema),
        minSpareRows: 0,
        minSpareCols: 0,
    });

    tblHandsonVolumenCaudal.updateSettings({
        beforeKeyDown(e) {
            //si se presiona ciertos caracteres
            if (e.keyCode === 106 || e.keyCode === 111 || e.keyCode === 190) { //char: '*', '/', ':'
                //no los muestra
                e.stopImmediatePropagation();
                e.preventDefault();
            }

        }
    });

    tblHandsonVolumenCaudal.addHook('afterRender', function () {
        //Valida (genera lista Error) cada vez que modifico una celda
        tblHandsonVolumenCaudal.validateCells();
    });

    tblHandsonVolumenCaudal.addHook('beforePaste', function () {
        //Valida (genera lista Error) cada vez que modifico una celda
        tblHandsonVolumenCaudal.validateCells();
    });

    tblHandsonVolumenCaudal.addHook('beforeValidate', function (value, row, prop, source) {
        listaErrores = [];
    });

    //Usado solo para obtener el listado Completo de Errores, para pintar las celdas se usa Renderer (colorCeldaValorRenderer)
    tblHandsonVolumenCaudal.addHook('afterValidate', function (isValid, value, row, prop, source) {

        var fila = "Fila "+ row;
        var numSddp = 'Volumen inicial por Sub-Embalse';

        if (prop == "VolIniXEmb") {
            if (!value || value === '') {
                if (parseFloat(value) != 0) {

                    var error1 = {
                        address: `<b>${fila}</b>&nbsp;&nbsp;&nbsp; / &nbsp;&nbsp;&nbsp;<b>${numSddp}</b>`,
                        valor: value,
                        message: "Campo vacío",
                        className: "errorNoNumerico"
                    };

                    listaErrores.push(error1);
                }
            } else {
                if (!$.isNumeric(value)) {
                    var error2 = {
                        address: `<b>${fila}</b>&nbsp;&nbsp;&nbsp; / &nbsp;&nbsp;&nbsp;<b>${numSddp}</b>`,
                        valor: value,
                        message: "El dato no es numérico",
                        className: "errorNoNumerico"
                    };

                    listaErrores.push(error2);
                }
            }
        }
    });

    tblHandsonVolumenCaudal.addHook('afterRenderer', function (TD, row, column, prop, value, cellProperties) {
        //pinta celda cada vez q modifico celda
        cellProperties.renderer = "miRenderCeldas"; //colorear celdas
    });

    tblHandsonVolumenCaudal.addHook('beforeRenderer', function (TD, row, column, prop, value, cellProperties) {
        //pinta celda al mostrar tabla
        cellProperties.renderer = "miRenderCeldas"; //colorear celdas
    });

    //pinta celda cada vez q modifico celda  
    tblHandsonVolumenCaudal.addHook('afterChange', function (source) {
        var dataHandson = tblHandsonVolumenCaudal.getSourceData();

        for (var i = 0; i < source.length; i++) {
            var filaCambiada = (source[i])[0];
            var propiedadCambiada = ((source[i])[1]).split('.')[0];
            var valorAntiguo = (source[i])[2];
            var valorNuevo = (source[i])[3];
            var miprop = (source[i])[1];
            var columna = tblHandsonVolumenCaudal.propToCol(miprop);
            var data = dataHandson[filaCambiada];
            var datosCelda = data[propiedadCambiada];


        }
    });
}

(function (Handsontable) {
    // Register an alias    
    Handsontable.renderers.registerRenderer('miRenderCeldas', colorCeldaValorRenderer);

})(Handsontable);

//Cambia de color las celdas segun se vayan mostrando (respecto al scroll) en la tabla web 
function colorCeldaValorRenderer(instance, td, row, col, prop, value, cellProperties) {
    Handsontable.renderers.NumericRenderer.apply(this, arguments);

    if (prop == "VolIniXEmb") { //no valida 1ra columna (ya que son no numericos)
        if (tblHandsonVolumenCaudal != undefined) {

            if (parseFloat(value) < 0) {
                td.className = 'errorLimitInferior';
            }

            if (!value || value === '') {
                if (parseFloat(value) != 0) {
                    td.className = 'errorNoNumerico';
                }
            } else {
                if (!$.isNumeric(value)) {
                    td.className = 'errorNoNumerico';
                }
            }
        }
    }

}


//////////////////////////////////////////////////////////////////////////

function guardarHandsonVolumenCaudalCmgCP() {
    if (listaErrores.length > 0) {
        alert('Existen errores en las celdas, favor de corregir y vuelva a presionar "Guardar". A continuación mostramos la lista de errores...');
        $("#btnMostrarErrores").trigger("click");

        return;
    }

    var mtopcodi = parseInt($('#hdMtopcodi').val()) || 0;
    var dataArray = tblHandsonVolumenCaudal.getSourceData();

    $.ajax({
        type: 'POST',
        url: controlador + 'GrabarHandsonVolumenEmbalse',
        data: {
            mtopcodi: mtopcodi,
            stringJson: JSON.stringify(dataArray)
        },
        datatype: 'json',
        success: function (evt) {
            if (evt.Resultado != "-1") {
                mostrarMensaje_('mensaje', 'exito', 'Los datos se guardaron correctamente.');
                obtenerDatosParaHandsonYMostrar(false);
            } else {
                mostrarMensaje_('mensaje', 'error', "Ha ocurrido un error: " + evt.Mensaje);
            }
        },
        error: function (err) {
            mostrarMensaje_('mensaje', 'error', "Ha ocurrido un error: ");
        }
    });

}


//////////////////////////////////////////////////////////////////////////
function mostrarVersiones() {
    var sFecha = getFechaConsulta();
    var periodoH = getPeriodoXFecha();

    $('#listadoHSH').html('');

    $.ajax({
        type: 'POST',
        url: controlador + "ListadoHistorial",
        data: {
            sFecha: sFecha,
            periodoH: periodoH,
        },
        success: function (evt) {
            if (evt.Resultado != "-1") {

                var html = dibujarTablaHistorial(evt.ListaEnvio);
                $('#listadoHistorial').html(html);
                $('#tabla_historial').dataTable({
                    "scrollY": 330,
                    "scrollX": true,
                    "sDom": 't',
                    "ordering": false,
                    "bPaginate": false,
                    "iDisplayLength": -1
                });

                abrirPopup("div_historial");
            } else {

                mostrarMensaje_('mensaje', 'error', 'Ha ocurrido un error al listar las versiones :' + evt.Mensaje);
            }
        },
        error: function (err) {
            mostrarMensaje_('mensaje', 'error', 'Ha ocurrido un error.');
        }
    });
}

function dibujarTablaHistorial(lista) {

    var cadena = '';
    cadena += `
    <table border="0" class="pretty tabla-icono dataTable" cellspacing="0" width="100%" id="tabla_historial">
        <thead>
            <tr>
                <th style='width: 70px'>Código de envío</th>
                <th style='width: 110px'>Fecha y hora del envío</th>
                <th style='width: 110px'>Usuario que realizó el envío</th>
            </tr>
        </thead>
        <tbody>
    `;

    for (key in lista) {
        var item = lista[key];

        cadena += `
            <tr>
                <td style="height: 24px">${item.Volcalcodi}</td>
                <td style="height: 24px">${item.VolcalfeccreacionDesc}</td>
                <td style="height: 24px">${item.Volcalusucreacion}</td>
            </tr>
        `;
    }
    cadena += "</tbody></table>";

    return cadena;
}

//////////////////////////////////////////////////////////////////////////
function actualizarBloqueDelHandsonSegunOrigen(anio, mes, rangoIni, rangoFin, tipo, accion, qnbenvcodi, numBotonOrigen) {
    var nombBoton = "";

    switch (numBotonOrigen) {
        case ORIGEN_HISTORICO: nombBoton = "Cargar Histórico"; break;
        case ORIGEN_PRONOSTICADO: nombBoton = "Cargar Pronóstico"; break;
        case ORIGEN_AUTOCOMPLETADO: nombBoton = "Autocompletar"; break;
    }

    if (listaErroresSinVacios.length > 0) {
        alert('Existen errores en las celdas, favor de corregir y vuelva a presionar "' + nombBoton + '". A continuación mostramos la lista de errores...');
        mostrarErroresSinVacios();

        return;
    }

    var accion_ = accion || 0;
    var anioIni_ = parseInt(rangoIni);
    var anioFin_ = parseInt(rangoFin);

    var dataArray = obtenerLstDatosDelHandson();

    $.ajax({
        type: 'POST',
        url: controlador + 'ActualizarBloqueHandsonSegunOrigen',
        data: {
            anioSerie: anio,
            mesSerie: mes,
            anioIni: anioIni_,
            anioFin: anioFin_,
            tipo: tipo,
            accion: accion_,
            origen: numBotonOrigen,
            codigoBase: codigoBaseEnDetalle,
            stringJson: JSON.stringify(dataArray),
            qnbenvcodi: qnbenvcodi,

        },
        datatype: 'json',
        success: function (evt) {
            if (evt.Resultado != "-1") {
                mostrarHandson(evt.DataHandsonSeriesHidrologica, evt.NumFilas);

                //valida datos (genera lista Error) despues de mostrar tabla
                tblHandsonVolumenCaudal.validateCells();
            } else {
                mostrarMensaje_('mensaje', 'error', "Ha ocurrido un error: " + evt.Mensaje);
            }
        },
        error: function (err) {
            mostrarMensaje_('mensaje', 'error', "Ha ocurrido un error: ");
        }
    });
}

function mostrarErrores() {

    tblHandsonVolumenCaudal.validateCells(); //para que valide al presionar Errores await

    setTimeout(function () {
        var listaErrorUnic = [];
        abrirPopup("erroresDatos");

        $.each(listaErrores, function (key, value) {
            var result = listaErrorUnic.filter(x => x.address === value.address);
            if (result.length === 0) {
                listaErrorUnic.push(value);
            }
        });

        tblErroresdatos.clear().draw();
        tblErroresdatos.rows.add(listaErrorUnic).draw();

    }, 2000);
}

function exportarInformacion(tipo, qnbenvcodi, anio, mes, accion) {

    $.ajax({
        type: 'POST',
        url: controlador + "DescargarSerieHidroCompleta",
        data: {
            codEnvio: qnbenvcodi,
            anio: anio,
            mes: mes,
            tipo: tipo,
            codigoBase: codigoBaseEnDetalle,
            accion: accion
        },
        success: function (evt) {
            if (evt.Resultado != "-1") {
                if (evt.Resultado == "0")
                    mostrarMensaje_('mensaje', 'message', evt.Mensaje);
                else
                    window.location = controlador + "Exportar?file_name=" + evt.Resultado;
            } else {
                mostrarMensaje_('mensaje', 'error', "Ha ocurrido un error: " + evt.Mensaje);
            }
        },
        error: function (err) {
            mostrarMensaje_('mensaje', 'error', 'Ha ocurrido un error.');
        }
    });
}

function obtenerCabeceraAgrupada(lstNestedHeaders) {

    var nestedHeaders = [];

    lstNestedHeaders.forEach(function (currentValue, index, array) {
        var nestedHeader = [];
        currentValue.forEach(function (item) {
            if (item.Colspan == 0) {
                nestedHeader.push("<span class='" + item.Class + "'>" + item.Label + " </span>");
                //nestedHeader.push(item.Label);
            } else {
                //nestedHeader.push({ label: "<div class='prueba'>" + item.Label + " </div>", colspan: item.Colspan });
                nestedHeader.push({ label: item.Label, colspan: item.Colspan });
            }
        });

        nestedHeaders.push(nestedHeader);
    });

    return nestedHeaders;
}

function actualizarAnioCrearSerie(opcion) {
    var mesAnio = "";
    if (opcion == 0) { //seleccione tipo
        $("#anioCrear").css("display", "none");
    } else {
        $("#anioCrear").css("display", "block");

        if (opcion == 3) { //tipo : semanal
            mesAnio = $("#hfNuevoMesAnioSemanal").val();

            $("#txtMesAnio").val(($("#hfNuevoMesAnioSemanal").val()).replace('*', ''));
        }
        if (opcion == 4) { //tipo : mensual
            mesAnio = $("#hfNuevoMesAnioMensual").val();

            $("#txtMesAnio").val(($("#hfNuevoMesAnioMensual").val()).replace('*', ''));
        }
    }

    if (mesAnio.includes("*")) {// cuando no existe registros anteriores
        $('#txtMesAnio').Zebra_DatePicker({
            format: 'm Y',
        });
    } else {// cuando  ya existe registrados
        $('#txtMesAnio').Zebra_DatePicker({
            format: 'm Y',
            direction: [mesAnio.replace('*', ''), "12 2100"],
        });
    }

}

//////////////////////////////////////////////////////////////////////////
function getFechaConsulta() {
    var sFecha = $('#txtFecha').val();

    return sFecha;
}

function getPeriodoXFecha() {
    var periodoH = parseInt($('#cbPeriodo').val()) || 0;

    var etapa = $("#cbEtapa").val();
    if (etapa == "P") {
        periodoH = 48;
    }

    return periodoH;
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

async function mostrarMensaje_(id, tipo, mensaje) {
    $("#" + id).css("display", "block");
    $('#' + id).removeClass();
    $('#' + id).addClass('action-' + tipo);
    $('#' + id).html(mensaje);

    //limpiarBarraMensaje(id);
}

function limpiarBarraMensaje(id) {
    $('#' + id).css("display", "none");
    $('#' + id).removeClass();
    $('#' + id).addClass('action-message');
    $('#' + id).html('');
}
