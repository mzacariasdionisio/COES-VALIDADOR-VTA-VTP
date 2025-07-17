var controlador = siteRoot + 'Hidrologia/CargaDatosCmgCP/';
var FILTRO_FECHA_INICIO = '01/01/2000';
var FILTRO_FECHA_HOY = '';
var FILTRO_FECHA_ANTERIOR = '';

var cabecerasHandson = [];
var ESTA_RECALCULADO = false;
var ORIGEN_EDITADA_USUARIO = 10;

var contenedorHt, tblHandsonVolumenCaudal, listaErrores = [], tblErroresdatos;

$(function () {
    FILTRO_FECHA_HOY = $("#hfFechaActual").val();
    FILTRO_FECHA_ANTERIOR = $("#hfFechaAnterior").val();

    //$("#txtFecha").val("06/04/2022"); //quitar para pruebas

    _mostrarFiltroEtapa();
    $('#cbEtapa').change(function () {
        _mostrarFiltroEtapa();
    });

    //botón consultar
    $('#btnConsultar').click(function () {

        limpiarBarraMensaje('mensaje');
        mostrarExcelWebXEnvio(0);
    });

    //Guardar
    $("#btnGuardar").click(function () {
        guardarHandsonVolumenCaudalCmgCP();
    });

    //Exportar Informacion 
    $('#btnDescargarF').click(function () {
        exportarInformacion();
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

    //Boton ActualizarDeBD
    $("#btnActualizarDeBD").click(function () {
        actualizarDeBD();
    });

    //Boton Recalcular
    $("#btnRecalcular").click(function () {
        recalcularHandsonVolumenCaudalCmgCP();
    });

    importarFormato();

    mostrarExcelWebXEnvio(0);

});

function _mostrarFiltroEtapa() {
    var etapa = $("#cbEtapa").val();

    $('#txtFecha').unbind();
    $("#div_hora").hide();
    $(".item_operativa").css("display", "none");
    $(".item_postoperativa").css("display", "none");

    if (etapa == "P") { //Postoperativa
        $('#txtFecha').val(FILTRO_FECHA_ANTERIOR);
        $('#txtFecha').Zebra_DatePicker({
            direction: [FILTRO_FECHA_INICIO, FILTRO_FECHA_ANTERIOR],
            onSelect: function () {
            }
        });

        $(".item_postoperativa").css("display", "inline-block");
    } else { //Operativa
        $('#txtFecha').val(FILTRO_FECHA_HOY);
        $('#txtFecha').Zebra_DatePicker({
            direction: [FILTRO_FECHA_INICIO, FILTRO_FECHA_HOY],
            onSelect: function () {
            }
        });

        //posicion final
        var hh2 = (new Date()).getHours();
        var mi2 = (new Date()).getMinutes();
        var pp2 = 0;

        if ((0 <= mi2) && (mi2 <= 30)) {
            pp2 = 0;
        }
        if ((30 < mi2) && (mi2 < 60)) {
            pp2 = 1;
        }

        var pos2 = hh2 * 2 + pp2;
        if (hh2 == 0 && mi2 == 0) {
            pos2 = hh2 * 2 + 48;
        }
        $('#cbPeriodo').val(pos2);

        $("#div_hora").show();
        $(".item_operativa").css("display", "inline-block");
    }
}

//////////////////////////////////////////////////////////////////////////
function mostrarExcelWebXEnvio(volcalcodi) {
    $("#hfVersionCalculo").val(volcalcodi);
    limpiarBarraMensaje('mensaje');

    var sFecha = getFechaConsulta();
    var periodoH = getPeriodoXFecha();


    $.ajax({
        type: 'POST',
        url: controlador + "ObtenerHandsonVolumenCaudalCmgCP",
        data: {
            sFecha: sFecha,
            periodoH: periodoH,
            volcalcodi: volcalcodi
        },
        success: function (evt) {
            if (evt.Resultado != "-1") {
                if (evt.Resultado == "1") {
                    ESTA_RECALCULADO = false;
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
                mostrarMensaje_('mensaje', 'error', "Ha ocurrido un error: " + evt.Mensaje);
            }
        },
        error: function (err) {
            mostrarMensaje_('mensaje', 'error', 'Ha ocurrido un error.');
        }
    });
}

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
        var numSddp = prop.split('.')[0];

        if (!value || value === '') {
            if (parseFloat(value) != 0) {

                var error1 = {
                    address: `<b>${fila}</b>&nbsp;&nbsp;&nbsp; / &nbsp;&nbsp;&nbsp;<b>${numSddp}</b>`,
                    valor: value,
                    message: "Campo vacío",
                    className: "errorVacio"
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

//Devuelve los datos de las filas del handson (Todo menos "Informacion Base", es decir, Historico, Pronosticado y autocompletado)
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

function guardarHandsonVolumenCaudalCmgCP() {
    //if (listaErrores.length > 0) {
    //    alert('Existen errores en las celdas, favor de corregir y vuelva a presionar "Guardar". A continuación mostramos la lista de errores...');
    //    $("#btnMostrarErrores").trigger("click");

    //    return;
    //}

    var sFecha = getFechaConsulta();
    var periodoH = getPeriodoXFecha();
    var dataArray = obtenerLstDatosDelHandson();

    if (ESTA_RECALCULADO || confirm("Se ha modificado los datos, se requiere recalcular los volumenes. ¿Desea guardar sin realizar el recálculo?"))
        $.ajax({
            type: 'POST',
            url: controlador + 'GrabarHandsonVolumenCaudalCmgCP',
            data: {
                sFecha: sFecha,
                periodoH: periodoH,
                volcalcodi: 0,
                stringJson: JSON.stringify(dataArray)
            },
            datatype: 'json',
            success: function (evt) {
                if (evt.Resultado != "-1") {
                    mostrarMensaje_('mensaje', 'exito', 'Los datos se guardaron correctamente.');
                    mostrarExcelWebXEnvio(evt.CodigoEnvio);
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
function actualizarDeBD() {
    var sFecha = getFechaConsulta();
    var periodoH = getPeriodoXFecha();

    $.ajax({
        type: 'POST',
        url: controlador + "ActualizarDeBD",
        data: {
            sFecha: sFecha,
            periodoH: periodoH,
        },
        success: function (evt) {
            if (evt.Resultado != "-1") {
                mostrarExcelWebXEnvio(0);
            } else {
                mostrarMensaje_('mensaje', 'error', "Ha ocurrido un error: " + evt.Mensaje);
            }
        },
        error: function (err) {
            mostrarMensaje_('mensaje', 'error', 'Ha ocurrido un error.');
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
                <th style='width: 110px'>Fecha proceso</th>
                <th style='width: 110px'>Tipo</th>
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
            <tr onclick="mostrarExcelWebFromHistorial(${item.Volcalcodi});" style="cursor:pointer" >
                <td style="height: 24px">${item.VolcalfechaDesc} ${item.VolcalperiodoDesc}</td>
                <td style="height: 24px">${item.VolcaltipoDesc}</td>
                <td style="height: 24px">${item.Volcalcodi}</td>
                <td style="height: 24px">${item.VolcalfeccreacionDesc}</td>
                <td style="height: 24px">${item.Volcalusucreacion}</td>
            </tr>
        `;
    }
    cadena += "</tbody></table>";

    return cadena;
}

function mostrarExcelWebFromHistorial(codigo) {
    $('#div_historial').bPopup().close();
    mostrarExcelWebXEnvio(codigo);
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
                mostrarMensaje_('mensaje', 'error', 'Se ha producido un error:' + err.message);
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
                mostrarMensaje_('mensaje', 'error', resultado.Mensaje);
            }
        },
        error: function (err) {
            mostrarMensaje_('mensaje', 'error', 'Ha ocurrido un error en leer archivo importado.');
        }
    });
}

//////////////////////////////////////////////////////////////////////////
function exportarInformacion() {
    limpiarBarraMensaje('mensaje');

    var sFecha = getFechaConsulta();
    var periodoH = getPeriodoXFecha();
    var volcalcodi = getVersion();
    if (volcalcodi <= 0) volcalcodi = 0;

    $.ajax({
        type: 'POST',
        url: controlador + "DescargarVersionCalculo",
        data: {
            sFecha: sFecha,
            periodoH: periodoH,
            volcalcodi: volcalcodi
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

function getVersion() {
    var valor = parseInt($('#hfVersionCalculo').val()) || 0;
    return valor;
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
