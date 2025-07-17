var controlador = siteRoot + 'YupanaContinuo/CargaDatos/';

var cabecerasHandson = [];
var ESTA_RECALCULADO = false;
var ORIGEN_EDITADA_USUARIO = 5;

var contenedorHt, tblHandsonVolumenCaudal, listaErrores = [], tblErroresdatos;

$(function () {
    $('#tab-container').easytabs({
        animate: false
    });
    $('#tab-container').bind('easytabs:after', function () {
        refrehDatatable();
    });

    $('#txtFecha').Zebra_DatePicker({
        format: 'd/m/Y',
        onSelect: function () {
            iniciarTabsReporteYDiario(0);
        }
    });
    $('#cbHora').change(function () {
        iniciarTabsReporteYDiario(0);
    });

    $("#btnConsultar").click(function () {
        iniciarTabsReporteYDiario(0);
    });

    $("#btnActualizar").click(function () {
        actualizarConfiguracionYReporteAutomatico();
    });

    ///////////////////////////Barra de herramientas
    //Boton Actualizar
    $("#btnImportarExtranet").click(function () {
        actualizarDeExtranet();
    });

    //Exportar Informacion 
    $('#btnDescargarF').click(function () {
        exportarInformacion();
    });

    //Guardar
    $("#btnGuardar").click(function () {
        guardarHandsonDetalle48();
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

    //Mostrar Versiones
    $("#btnVerHistorial").click(function () {
        mostrarVersiones();
    });

    importarFormato();

    ///////////////////////////Barra de herramientas
    $("#btnActualizarSoloReporte").click(function () {
        actualizarReporteAutomatico();
    });

    iniciarTabsReporteYDiario(0);
});

function iniciarTabsReporteYDiario(cyupcodi) {
    var tipoConfiguracion = $("#tipoConfiguracion").val();
    var tyupcodi = parseInt($("#tyupcodi").val()) || 0;
    var fecha = $("#txtFecha").val();
    var hora = parseInt($("#cbHora").val()) || 0;

    $("#cyupcodi").val(cyupcodi);

    $("#div_reporte").html('');
    $("#barra_configuracion").hide();
    $("#barraHerramientaYC").hide();

    limpiarBarraMensaje('mensaje');
    limpiarBarraMensaje('mensaje2');

    $('#tab-container').easytabs('select', '#vistaReporte');

    $.ajax({
        type: 'POST',
        url: controlador + 'CargarFiltroYHandsonXDiaHora',
        dataType: 'json',
        cache: false,
        data: {
            fecha: fecha,
            hora: hora,
            tyupcodi: tyupcodi,
            cyupcodi: cyupcodi
        },
        success: function (data) {
            if (data.Resultado != "-1") {
                //tab Reporte
                if (data.Resultado == "1") {
                    ESTA_RECALCULADO = false;

                    //muestro handson
                    mostrarHandson(data.Handson, 48 + 4);

                    //valida datos (genera lista Error) despues de mostrar tabla
                    tblHandsonVolumenCaudal.validateCells();

                    //mostrar datos de envio
                    _mostrarDivInformacionEnvio(data.CabCalculo);
                } else {
                    $("#htTablaWeb").html('<div style="margin-top: 100px;">No existen datos para los filtros seleccionados.</div>');
                }

                //si la configuracion es diaria y existente para la hora seleccionada
                $("#yupcfgcodi").val(data.Yupcfgcodi)

                if (tipoConfiguracion == "D" && data.Yupcfgcodi > 0) {
                    $("#barra_configuracion").show();
                    $("#barraHerramientaYC").show();

                    cargarListadoConfiguracion();
                }
            } else {
                alert("Ha ocurrido un error: " + data.Mensaje);
            }
        },
        error: function (err) {
            alert('Ha ocurrido un error.');
        }
    });
}

function actualizarDeExtranet() {
    var tyupcodi = parseInt($("#tyupcodi").val()) || 0;
    var fecha = $("#txtFecha").val();
    var hora = parseInt($("#cbHora").val()) || 0;

    var dataArray = obtenerLstDatosDelHandson();

    limpiarBarraMensaje('mensaje');
    limpiarBarraMensaje('mensaje2');

    $.ajax({
        type: 'POST',
        url: controlador + 'CargarHandsonFromExtranetXDiaHora',
        dataType: 'json',
        cache: false,
        data: {
            fecha: fecha,
            hora: hora,
            tyupcodi: tyupcodi,
            stringJson: JSON.stringify(dataArray)
        },
        success: function (data) {
            if (data.Resultado != "-1") {
                ESTA_RECALCULADO = false;

                //muestro handson
                mostrarHandson(data.Handson, 48 + 4);

                //valida datos (genera lista Error) despues de mostrar tabla
                tblHandsonVolumenCaudal.validateCells();

                //mostrar datos de envio
                _mostrarDivInformacionEnvio(data.CabCalculo);
            } else {
                alert("Ha ocurrido un error: " + data.Mensaje);
            }
        },
        error: function (err) {
            alert('Ha ocurrido un error.');
        }
    });
}

function actualizarConfiguracionYReporteAutomatico() {
    var tyupcodi = parseInt($("#tyupcodi").val()) || 0;
    var fecha = $("#txtFecha").val();
    var hora = parseInt($("#cbHora").val()) || 0;

    $.ajax({
        type: 'POST',
        url: controlador + 'ConfiguracionYReporteAutomatizado',
        dataType: 'json',
        cache: false,
        data: {
            fecha: fecha,
            hora: hora,
            tyupcodi: tyupcodi,
        },
        success: function (data) {
            if (data.Resultado == "1") {
                alert('El proceso se ejecutó correctamente');
                iniciarTabsReporteYDiario(0);
            } else {
                alert("Ha ocurrido un error: " + data.Mensaje);
            }
        },
        error: function (err) {
            alert('Ha ocurrido un error.');
        }
    });
}

function actualizarReporteAutomatico() {
    var tyupcodi = parseInt($("#tyupcodi").val()) || 0;
    var fecha = $("#txtFecha").val();
    var hora = parseInt($("#cbHora").val()) || 0;

    $.ajax({
        type: 'POST',
        url: controlador + 'DataReporteAutomatizado',
        dataType: 'json',
        cache: false,
        data: {
            fecha: fecha,
            hora: hora,
            tyupcodi: tyupcodi,
        },
        success: function (data) {
            if (data.Resultado == "1") {
                alert('Los datos del Reporte se actualizaron correctamente');
                iniciarTabsReporteYDiario(0);
            } else {
                alert("Ha ocurrido un error: " + data.Mensaje);
            }
        },
        error: function (err) {
            alert('Ha ocurrido un error.');
        }
    });
}

function refrehDatatable() {
    $('#reporteConfiguracion').dataTable({
        "destroy": "true",
        "scrollY": 550,
        "scrollX": true,
        "sDom": 'ft',
        "ordering": false,
        "bPaginate": false,
        "iDisplayLength": -1
    });
}

//////////////////////////////////////////////////////////////////////////
function mostrarHandson(hot, numFilas) {
    $("#htTablaWeb").html('');

    listaErrores = [];
    contenedorHt = document.getElementById('htTablaWeb');

    var nestedHeader = obtenerCabeceraAgrupada(hot.NestedHeader.ListCellNestedHeaders);

    tblHandsonVolumenCaudal = new Handsontable(contenedorHt, {
        data: JSON.parse(hot.ListaExcelData2),
        colWidths: hot.ListaColWidth,
        nestedHeaders: nestedHeader,
        //stretchH: 'all', // 'none' is default

        columns: hot.Columnas,
        width: '100%',
        height: 550,
        //rowHeights: 23,
        rowHeaders: false,  //quita numeración en filas
        fixedColumnsLeft: 1,
        cell: JSON.parse(hot.Esquema),

        //maxFilas
        minSpareRows: 0,
        minSpareCols: 0,
        maxRows: numFilas,
        autoRowSize: { syncLimit: 30000 },
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
        var fila = tblHandsonVolumenCaudal.getDataAtCell(row, 0);
        var recurnombre = "";
        if (prop.includes("E")) {
            recurcodi = parseInt(prop.split('.')[0].split("E")[1]) || 0;

            var objRec = obtenerObjRecurso(recurcodi, LISTA_RECURSO) //del otro.js
            if (objRec != null) recurnombre = objRec.Recurnombre;
        }

        if (!value || value === '') {
            if (parseFloat(value) != 0) {

                var error1 = {
                    address: `<b>${fila}</b>&nbsp;&nbsp;&nbsp; / &nbsp;&nbsp;&nbsp;<b>${recurnombre}</b>`,
                    valor: value,
                    message: "Campo vacío",
                    className: "errorVacio"
                };

                listaErrores.push(error1);
            }
        } else {
            if (!$.isNumeric(value)) {
                var error2 = {
                    address: `<b>${fila}</b>&nbsp;&nbsp;&nbsp; / &nbsp;&nbsp;&nbsp;<b>${recurnombre}</b>`,
                    valor: value,
                    message: "El dato no es numérico",
                    className: "errorNoNumerico"
                };

                listaErrores.push(error2);
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

            //si el valor es editado cambia su origen
            if (valorAntiguo != valorNuevo) {

                var nombClase = "sinFormato";
                if (datosCelda.ValDefecto != datosCelda.Valor) {
                    datosCelda.Origen = ORIGEN_EDITADA_USUARIO;
                    nombClase = 'celdaInfoPorUsuario';
                    this.render()
                } else {
                    datosCelda.Origen = datosCelda.OrigenDefecto;
                    switch (datosCelda.OrigenDefecto) {

                        case ORIGEN_BASE: nombClase = 'celdaInfoBase'; break;
                        case ORIGEN_HISTORICO: nombClase = 'celdaInfoHistorico'; break;
                        case ORIGEN_PRONOSTICADO: nombClase = 'celdaInfoPronosticado'; break;
                        case ORIGEN_AUTOCOMPLETADO: nombClase = 'celdaInfoAutoCompletado'; break;
                    }
                }
                this.setCellMeta(filaCambiada, columna, 'className', nombClase);
                this.render();
            }
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

    if (col != 0) { //no valida 1ra columna (ya que son no numericos)
        if (tblHandsonVolumenCaudal != undefined) {

            if (parseFloat(value) < 0) {
                td.className += ' errorLimitInferior';
            }

            if (!value || value === '') {
                if (parseFloat(value) != 0)
                    td.className = 'errorVacio';
            } else {
                if (!$.isNumeric(value)) {
                    td.className = 'errorNoNumerico';
                }
            }
        }
    }

}

function obtenerCabeceraAgrupada(lstNestedHeaders) {

    var nestedHeaders = [];

    lstNestedHeaders.forEach(function (currentValue, index, array) {
        var nestedHeader = [];
        currentValue.forEach(function (item) {
            if (item.Colspan == 0) {
                var title = item.Title != undefined && item.Title != null ? item.Title : '';
                nestedHeader.push(`<span class='${item.Class}' title='${title}'> ${item.Label} </span>`);
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

function _mostrarDivInformacionEnvio(objEnvio) {

    if (objEnvio != null) {
        var cadena = '';
        cadena += `
        <strong>Código de cálculo</strong>: ${objEnvio.Volcalcodi}, 
        <strong>Fecha de cálculo</strong>: ${objEnvio.VolcalfeccreacionDesc},
        <strong>Tipo</strong>: ${objEnvio.VolcaltipoDesc}
    `;

        mostrarMensaje_('mensaje', 'message', cadena);
    }
}

//////////////////////////////////////////////////////////////////////////

//Devuelve los datos de las filas del handson
function obtenerLstDatosDelHandson() {

    var data = tblHandsonVolumenCaudal.getSourceData();
    var lstdata = [];
    var n = 0;

    data.forEach(function (items) {

        var strFila = "";

        var arrayDatos = Object.values(data[n]);
        var arrayCampos = Object.keys(data[n]);
        var numColumnas = arrayCampos.length - 1;

        if (arrayDatos[numColumnas] != "B") { //TODO menos Info Base

            var separador = "";
            for (let index = 0; index < numColumnas; index++) {
                if (index == 0) separador = ""; else separador = "|";
                if (index == 0)
                    strFila = strFila + separador + arrayCampos[index] + ":" + arrayDatos[index];
                else
                    strFila = strFila + separador + arrayCampos[index] + ":" + arrayDatos[index].Valor + "*" + arrayDatos[index].Origen;
            }

            lstdata.push(strFila);
        }
        n++;
    });
    return lstdata;
}

function guardarHandsonDetalle48() {
    if (listaErrores.length > 0) {
        alert('Existen errores en las celdas, favor de corregir y vuelva a presionar "Guardar". A continuación mostramos la lista de errores...');
        $("#btnMostrarErrores").trigger("click");

        return;
    }

    var tyupcodi = parseInt($("#tyupcodi").val()) || 0;
    var fecha = $("#txtFecha").val();
    var hora = parseInt($("#cbHora").val()) || 0;

    var dataArray = obtenerLstDatosDelHandson();

    $.ajax({
        type: 'POST',
        url: controlador + 'GrabarHandsonDetalleCargaConfiguracion',
        data: {
            fecha: fecha,
            hora: hora,
            tyupcodi: tyupcodi,
            stringJson: JSON.stringify(dataArray)
        },
        datatype: 'json',
        success: function (evt) {
            if (evt.Resultado != "-1") {
                mostrarMensaje_('mensaje2', 'exito', 'Los datos se guardaron correctamente.');
                iniciarTabsReporteYDiario(evt.CodigoEnvio);
            } else {
                mostrarMensaje_('mensaje2', 'error', "Ha ocurrido un error: " + evt.Mensaje);
            }
        },
        error: function (err) {
            mostrarMensaje_('mensaje2', 'error', "Ha ocurrido un error: ");
        }
    });

}

//////////////////////////////////////////////////////////////////////////
function recalcularHandsonVolumenCaudalCmgCP() {
    //if (listaErrores.length > 0) {
    //    alert('Existen errores en las celdas, favor de corregir y vuelva a presionar "Guardar". A continuación mostramos la lista de errores...');
    //    $("#btnMostrarErrores").trigger("click");

    //    return;
    //}

    $("#hfVersionCalculo").val(0);
    limpiarBarraMensaje('mensaje');
    limpiarBarraMensaje('mensaje2');

    var sFecha = getFechaConsulta();
    var periodoH = getPeriodoXFecha();
    var dataArray = obtenerLstDatosDelHandson();

    $.ajax({
        type: 'POST',
        url: controlador + 'RecalcularVolumenCaudalCmgCP',
        data: {
            sFecha: sFecha,
            periodoH: periodoH,
            volcalcodi: 0,
            stringJson: JSON.stringify(dataArray)
        },
        datatype: 'json',
        success: function (evt) {
            if (evt.Resultado != "-1") {
                if (evt.Resultado == "1") {
                    ESTA_RECALCULADO = true;
                    cabecerasHandson = evt.LstCabeceras;

                    //muestro handson
                    mostrarHandson(evt.Handson, 48 + 4);

                    //valida datos (genera lista Error) despues de mostrar tabla
                    tblHandsonVolumenCaudal.validateCells();

                    //mostrar datos de envio
                    _mostrarDivInformacionEnvio(evt.CabCalculo);
                } else {
                    $("#htTablaWeb").html('<div style="margin-top: 100px;">No existen datos para los filtros seleccionados.</div>');
                }
            } else {
                mostrarMensaje_('mensaje2', 'error', "Ha ocurrido un error: " + evt.Mensaje);
            }
        },
        error: function (err) {
            mostrarMensaje_('mensaje2', 'error', "Ha ocurrido un error: ");
        }
    });

}

//////////////////////////////////////////////////////////////////////////
function mostrarVersiones() {
    var tyupcodi = parseInt($("#tyupcodi").val()) || 0;
    var fecha = $("#txtFecha").val();
    var hora = parseInt($("#cbHora").val()) || 0;

    $('#listadoHistorial').html('');
    $("#mensaje2").hide();

    $.ajax({
        type: 'POST',
        url: controlador + "ListadoHistorial",
        data: {
            fecha: fecha,
            hora: hora,
            tyupcodi: tyupcodi,
        },
        success: function (evt) {
            if (evt.Resultado != "-1") {

                var html = dibujarTablaHistorial(evt.ListaEnvio);
                $('#listadoHistorial').html(html);

                abrirPopup("div_historial");

                setTimeout(function () {
                    $('#tabla_historial').dataTable({
                        "scrollY": 330,
                        "scrollX": true,
                        "sDom": 't',
                        "ordering": false,
                        "bPaginate": false,
                        "iDisplayLength": -1
                    });
                }, 1000);
            } else {

                mostrarMensaje_('mensaje2', 'error', 'Ha ocurrido un error al listar las versiones :' + evt.Mensaje);
            }
        },
        error: function (err) {
            mostrarMensaje_('mensaje2', 'error', 'Ha ocurrido un error.');
        }
    });
}

function dibujarTablaHistorial(lista) {

    var cadena = '';
    cadena += `
    <table border="0" class="pretty tabla-icono dataTable" cellspacing="0" width="100%" id="tabla_historial">
        <thead>
            <tr>
                <th style='width: 110px'>Fecha proceso</th>
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
            <tr onclick="mostrarExcelWebFromHistorial(${item.Cyupcodi});" style="cursor:pointer" >
                <td style="height: 24px">${item.CyupfechaDesc} ${item.CyupbloquehorarioDesc}</td>
                <td style="height: 24px">${item.Cyupcodi}</td>
                <td style="height: 24px">${item.CyupfecregistroDesc}</td>
                <td style="height: 24px">${item.Cyupusuregistro}</td>
            </tr>
        `;
    }
    cadena += "</tbody></table>";

    return cadena;
}

function mostrarExcelWebFromHistorial(codigo) {
    $('#div_historial').bPopup().close();
    iniciarTabsReporteYDiario(codigo);
}

//////////////////////////////////////////////////////////////////////////
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

    }, 1000);
}

//////////////////////////////////////////////////////////////////////////
function importarFormato() {

    var uploader = new plupload.Uploader({
        runtimes: 'html5,flash,silverlight,html4',
        browse_button: "btnImportarF",
        url: controlador + "Upload",
        flash_swf_url: 'Scripts/Moxie.swf',
        silverlight_xap_url: 'Scripts/Moxie.xap',
        multi_selection: false,
        filters: {
            max_file_size: '5mb',
            mime_types: [
                { title: "Archivos Excel .xlsx", extensions: "xlsx,xls" }
            ]
        },
        init: {
            FilesAdded: function (up, files) {
                if (uploader.files.length == 2) {
                    uploader.removeFile(uploader.files[0]);
                }
                uploader.start();
                up.refresh();
            },
            UploadProgress: function (up, file) {
            },
            UploadComplete: function (up, file) {
                leerFileUpExcel();
            },
            Error: function (up, err) {
                mostrarMensaje_('mensaje2', 'error', 'Se ha producido un error:' + err.message);
            }
        }
    });

    uploader.init();
}

function leerFileUpExcel() {
    limpiarBarraMensaje('mensaje');

    $.ajax({
        type: 'POST',
        url: controlador + 'LeerArchivoExcelVolumenCaudalCmgCP',
        dataType: 'json',
        async: true,
        data: {
        },
        success: function (resultado) {
            if (resultado.Resultado == "1") {
                mostrarExcelWebXEnvio(-1);
            } else {
                mostrarMensaje_('mensaje2', 'error', resultado.Mensaje);
            }
        },
        error: function (err) {
            mostrarMensaje_('mensaje2', 'error', 'Ha ocurrido un error en leer archivo importado.');
        }
    });
}

//////////////////////////////////////////////////////////////////////////
function exportarInformacion() {
    var tyupcodi = parseInt($("#tyupcodi").val()) || 0;
    var cyupcodi = parseInt($("#cyupcodi").val()) || 0;
    var fecha = $("#txtFecha").val();
    var hora = parseInt($("#cbHora").val()) || 0;

    $.ajax({
        type: 'POST',
        url: controlador + "DescargarVersionCalculo",
        data: {
            fecha: fecha,
            hora: hora,
            tyupcodi: tyupcodi,
            cyupcodi: cyupcodi
        },
        success: function (evt) {
            if (evt.Resultado != "-1") {
                if (evt.Resultado == "0")
                    mostrarMensaje_('mensaje2', 'message', evt.Mensaje);
                else
                    window.location = controlador + "Exportar?file_name=" + evt.Resultado;
            } else {
                mostrarMensaje_('mensaje2', 'error', "Ha ocurrido un error: " + evt.Mensaje);
            }
        },
        error: function (err) {
            mostrarMensaje_('mensaje2', 'error', 'Ha ocurrido un error.');
        }
    });
}

//////////////////////////////////////////////////////////////////////////
async function mostrarMensaje_(id, tipo, mensaje) {
    limpiarBarraMensaje(id);

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