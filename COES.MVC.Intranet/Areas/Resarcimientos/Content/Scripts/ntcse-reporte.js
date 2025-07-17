$.message = "";
var strControlador = siteRoot + "resarcimientos/reportehistorico/";

$(function () {
    var intFilaInicio = 3,
        hstGrillaPE = null,
        hstGrillaRC = null,
        arrCliente = null,
        arrBarra = null,
        arrNivelTension = null,
        arrTipo = null,
        arrBool = null,
        arrCodigoEventoCOES = null,
        arrEmpresa = null,
        container = document.getElementById('hst-grilla-pe');

    $(document).ready(function () {

        // Botones Inhabilitados por Defecto
        $('#btn-excel-descargar').prop("disabled", true);
        $('#btn-excel-descargar').css("color", 'darkgray');
        $('#btn-excel-descargar').css("background-image", 'url("")');

        $('#btn-consultar').click(function () {
            $('#btn-excel-descargar').prop("disabled", true);
            $('#btn-excel-descargar').css("color", 'darkgray');

            consultarReporteHistorico();
        });

        $("#btn-tabla_envios").on('click', function () {
            $('#panel-01').toggle();
        });

        $('#btn-excel-descargar').click(function () {
            descargarExcel(hstGrillaPE, hstGrillaRC);
        });

        $('#btn-plantilla-descargar').click(function () {
            window.location.href = strControlador + 'DescargarPlantillaExcel';
        })

        $('.codenvio').click(function () {
            var arrGrillas = leerGrillaBD($(this).attr('data-ofercodi'));
            $('#btn-excel-descargar').prop("disabled", false);
            $('#btn-excel-descargar').css("color", '');
            $('#btn-excel-descargar').css("background-image", '');
            calculateSize();
            //mostrarGrillaPE(arrGrillas[0]);
            //mostrarGrillaRC(arrGrillas[1]);
        });

        $('#tab-container').easytabs({ animate: false });
        $('#tab-container').css("display", 'none');

        traerParametros();
        mostrarGrillaPE(null);
        mostrarGrillaRC(null);
    })

    function traerParametros() {
        $.ajax({
            type: 'POST',
            url: strControlador + 'TraerParametros',
            async: false,
            data: { empresa: 0 },
            success: function (response) {
                arrCliente = response[0];
                arrBarra = response[1];
                arrNivelTension = response[2];
                arrTipo = response[3];
                arrBool = response[4];
                arrCodigoEventoCOES = response[5];
                arrEmpresa = response[6];
            },
            error: function (response) {
                $('#mensaje').html('No se han podido descargar los parámetros. ' + response.status + ' - ' + response.statusText + '.');
            }
        });
    }

    function consultarReporteHistorico() {

        $('#tblTramites >tbody>tr').remove();

        $.ajax({
            type: 'POST',
            url: strControlador + 'consultar',
            async: false,
            data: {
                empresa: $('#CboEmpresasGeneradoras').val(),
                periodo: $('#CboPeriodo').val(),
                codigoEnvio: $('#CodEnvio').val(),
                puntoEntrega: 0
            },
            success: function (resultado) {
                var arrenvios = resultado[0];

                if (arrenvios != null) {
                    for (var i = 0, j = arrenvios.length; i < j ; i++) {

                        var fecha = arrenvios[i].RpeFechaCreacion;
                        var parsedDate = new Date(parseInt(fecha.substr(6)));
                        var jsDate = new Date(parsedDate);

                        var day = jsDate.getDate(),
                            month = jsDate.getMonth() + 1,
                            year = jsDate.getFullYear(),
                            hours = jsDate.getHours()
                        minutes = jsDate.getMinutes();
                        seconds = jsDate.getSeconds();

                        (day < 10) ? day = "0" + day : day;
                        (month < 10) ? month = "0" + month : month;
                        (seconds < 10) ? seconds = "0" + seconds : seconds;

                        var date = day + "/" + month + "/" + year + " " + hours + ":" + minutes + ":" + seconds;

                        var tdCodigo = $('<td>').html(arrenvios[i].EnvioCodi).css("text-align", "center")
                             .css("color", "#3366CC"),
                             tdEmpGeneradora = $('<td>').html(arrenvios[i].RpeEmpresaGeneradoraNombre).css("text-align", "center"),
                             tdPeriodoDesc = $('<td>').html(arrenvios[i].PeriodoDesc).css("text-align", "center"),
                             tdUsuarioCreacion = $('<td>').html(arrenvios[i].RpeUsuarioCreacion).css("text-align", "center"),
                             tdFechaCreacion = $('<td>').html(date).css("text-align", "center"),
                             tr = $('<tr>')
                                 .data('codigoEnvio', arrenvios[i].EnvioCodi)
                                 .data('empresaGeneradora', arrenvios[i].RpeEmpresaGeneradoraNombre)
                                 .data('periodoDesc', arrenvios[i].PeriodoDesc)
                                 .data('usuarioCreacion', arrenvios[i].RpeUsuarioCreacion)
                                 .data('fechaCreacion', date)
                                 .append(tdCodigo)
                                 .append(tdEmpGeneradora)
                                 .append(tdPeriodoDesc)
                                 .append(tdUsuarioCreacion)
                                 .append(tdFechaCreacion)
                                 .click(function () {
                                     var arrGrillas = leerGrillaBD($(this).data('codigoEnvio'));


                                     $('#btn-excel-descargar').prop("disabled", false);
                                     $('#btn-excel-descargar').css("color", '');
                                     $('#btn-excel-descargar').css("background-image", '');

                                     $('#tab-container').css("display", '');

                                     mostrarGrillaPE(arrGrillas[0]);
                                     mostrarGrillaRC(arrGrillas[1]);

                                 });
                        $('#tblTramites >tbody').append(tr);
                    }
                }
                else {

                    $('#resultado').html('No hay registros...')
                    addClass('action-mesagge');
                }
            },
            error: function (req, status, error) {
            }
        });
    }

    function calculateSize() {
        var offset;
        offset = Handsontable.Dom.offset(container);

        availableHeight = 500;
        console.log("availableHeight " + availableHeight);

        availableWidth = $(window).width() - offset.left;
        console.log("availableWidth " + availableWidth);

        container.style.height = availableHeight + 'px';
        container.style.width = availableWidth + 'px';

        hstGrillaRC.render();
    }

    function mostrarGrillaPE(filas) {
        if (!!hstGrillaPE) {
            hstGrillaPE.destroy();
        }

        filas = (filas || []);

        filas.unshift(['Punto de\nEntrega', 'CLIENTE', 'Barra', 'Nivel de\nTensión', 'Energía\nSemestral\n(KW.h)', 'Incremento de\ntolerancias\nSector Distribución\ntipico2\n(Mercado regulado)', 'Tipo', 'Exonerado o\nFuerza\nMayor', 'Ni', 'Ki', 'Tiempo Ejecutado', '', 'Tiempo Programado', '', 'Responsable 1', '', 'Responsable 2', '', 'Responsable 3', '', 'Responsable 4', '', 'Responsable 5', '', 'Causa Resumida de\nInterrupción', 'Ei / E', 'Resarcimiento\n(US$)'],
            ['', '', '', '', '', '', '', '', '', '', 'Fecha Hora\nInicio', 'Fecha Hora\nFin', 'Fecha Hora\nInicio', 'Fecha Hora\nFin', 'Empresa', '%', 'Empresa', '%', 'Empresa', '%', 'Empresa', '%', 'Empresa', '%', '', '', '']);

        hstGrillaPE = new Handsontable($('#hst-grilla-pe')[0], {
            data: filas,
            colHeaders: true,
            rowHeaders: true,
            contextMenu: false,
            minSpareRows: 1,
            fixedRowsTop: 2,
            cells: function (row, col, prop) {
                var cellProperties = {};

                if (row === 0 || row === 1) {
                    cellProperties.renderer = headerRenderer;
                }

                if (row >= 2) {

                    switch (col) {
                        case 0:
                            cellProperties.renderer = puntoEntregaRenderer;
                            cellProperties.type = 'numeric';
                            break;
                        case 1:
                            cellProperties.type = 'autocomplete';
                            cellProperties.source = arrCliente[0];
                            break;
                        case 2:
                            cellProperties.type = 'autocomplete';
                            cellProperties.source = arrBarra[0];
                            break;
                        case 3:
                            cellProperties.type = 'autocomplete';
                            cellProperties.source = arrNivelTension;
                            break;
                        case 4:
                            cellProperties.renderer = energiaSemestralRenderer;
                            cellProperties.type = 'numeric';
                        case 5:
                            cellProperties.renderer = exoneradoFuerzaMayorRenderer;
                            cellProperties.type = 'autocomplete';
                            cellProperties.source = arrBool;
                            break;
                        case 6:
                            cellProperties.type = 'autocomplete';
                            cellProperties.source = arrTipo;
                            break;
                        case 7:
                            cellProperties.renderer = exoneradoFuerzaMayorRenderer;
                            cellProperties.type = 'autocomplete';
                            cellProperties.source = arrBool;
                            break;
                        case 8:
                        case 9:
                            cellProperties.renderer = NiKiRenderer;
                            cellProperties.type = 'numeric';
                            break;
                        case 10:
                        case 11:
                        case 12:
                        case 13:
                            cellProperties.renderer = tiempoEjecutadoRenderer;
                            break;
                        case 14:
                        case 16:
                        case 18:
                        case 20:
                        case 22:
                            cellProperties.type = 'autocomplete';
                            cellProperties.source = arrEmpresa;
                            break;
                        case 15:
                        case 17:
                        case 19:
                        case 21:
                        case 23:
                            cellProperties.renderer = responsablePercentageRenderer;
                            cellProperties.type = 'numeric';
                            break;
                        case 25:
                            cellProperties.renderer = eiERenderer;
                            cellProperties.type = 'numeric';
                            break;
                        case 26:
                            cellProperties.renderer = resarcimientoRenderer;
                            cellProperties.type = 'numeric';
                            break;
                        case 27: // ID de cliente
                            if (row == 2)
                                console.log('Ingreso ID Cliente...');
                            cellProperties.type = 'autocomplete';
                            cellProperties.source = arrCliente[1];
                            break;

                    }
                }
                else {
                    cellProperties.editor = false;
                }

                return cellProperties;
            },
            mergeCells: [
                { row: 0, col: 0, rowspan: 2, colspan: 1 },
                { row: 0, col: 1, rowspan: 2, colspan: 1 },
                { row: 0, col: 2, rowspan: 2, colspan: 1 },
                { row: 0, col: 3, rowspan: 2, colspan: 1 },
                { row: 0, col: 4, rowspan: 2, colspan: 1 },
                { row: 0, col: 5, rowspan: 2, colspan: 1 },
                { row: 0, col: 6, rowspan: 2, colspan: 1 },
                { row: 0, col: 7, rowspan: 2, colspan: 1 },
                { row: 0, col: 8, rowspan: 2, colspan: 1 },
                { row: 0, col: 9, rowspan: 2, colspan: 1 },
                { row: 0, col: 10, rowspan: 1, colspan: 2 },
                { row: 0, col: 12, rowspan: 1, colspan: 2 },
                { row: 0, col: 14, rowspan: 1, colspan: 2 },
                { row: 0, col: 16, rowspan: 1, colspan: 2 },
                { row: 0, col: 18, rowspan: 1, colspan: 2 },
                { row: 0, col: 20, rowspan: 1, colspan: 2 },
                { row: 0, col: 22, rowspan: 1, colspan: 2 },
                { row: 0, col: 24, rowspan: 2, colspan: 1 },
                { row: 0, col: 25, rowspan: 2, colspan: 1 },
                { row: 0, col: 26, rowspan: 2, colspan: 1 }
            ]
        });
    }

    function mostrarGrillaRC(filas) {
        if (!!hstGrillaRC) {
            hstGrillaRC.destroy();
        }

        filas = (filas || []);

        filas.unshift(['Punto de\nEntrega', 'CLIENTE', 'Barra', 'Código\nAlimentador', 'SED', '', 'ENS f', 'Código COES\ndel Evento', 'Interrupción', '', 'Pk\n(kW)', 'Compensable', 'ENS f, k\n(kWh)', 'Resarcimiento\n(US$)'],
            ['', '', '', '', 'Nombre', 'kV', '', '', 'Inicio', 'Fin', '', '', '', '']);

        hstGrillaRC = new Handsontable($('#hst-grilla-rc')[0], {
            data: filas,
            colHeaders: true,
            rowHeaders: true,
            contextMenu: false,
            minSpareRows: 1,
            fixedRowsTop: 2,
            cells: function (row, col, prop) {
                var cellProperties = {};

                if (row === 0 || row === 1) {
                    cellProperties.renderer = headerRenderer;
                }

                if (row >= 2) {
                    switch (col) {
                        case 0:
                            cellProperties.type = 'numeric';
                            break;
                        case 1:
                            cellProperties.type = 'autocomplete';
                            cellProperties.source = arrCliente;
                            break;
                        case 2:
                            cellProperties.type = 'autocomplete';
                            cellProperties.source = arrBarra;
                            break;
                        case 3:
                            break;
                        case 4:
                            break;
                        case 5:
                            cellProperties.renderer = sEDkVERenderer;
                            cellProperties.type = 'numeric';
                            break;
                        case 6:
                            cellProperties.renderer = eNSfRenderer;
                            cellProperties.type = 'numeric';
                            break;
                        case 7:
                            cellProperties.type = 'autocomplete';
                            cellProperties.source = arrCodigoEventoCOES;
                            break;
                        case 8:
                        case 9:
                            cellProperties.renderer = interrupcionRenderer;
                            break;
                        case 10:
                            cellProperties.renderer = pkRenderer;
                            cellProperties.type = 'numeric';
                            break;
                        case 11:
                            break;
                        case 12:
                            cellProperties.renderer = eNSfkRenderer;
                            cellProperties.type = 'numeric';
                            break;
                        case 13:
                            cellProperties.renderer = resarcimientoRenderer;
                            cellProperties.type = 'numeric';
                            break;
                    }
                }
                else {
                    cellProperties.editor = false;
                }

                return cellProperties;
            },
            mergeCells: [
                { row: 0, col: 0, rowspan: 2, colspan: 1 },
                { row: 0, col: 1, rowspan: 2, colspan: 1 },
                { row: 0, col: 2, rowspan: 2, colspan: 1 },
                { row: 0, col: 3, rowspan: 2, colspan: 1 },
                { row: 0, col: 4, rowspan: 1, colspan: 2 },
                { row: 0, col: 6, rowspan: 2, colspan: 1 },
                { row: 0, col: 7, rowspan: 2, colspan: 1 },
                { row: 0, col: 8, rowspan: 1, colspan: 2 },
                { row: 0, col: 10, rowspan: 2, colspan: 1 },
                { row: 0, col: 11, rowspan: 2, colspan: 1 },
                { row: 0, col: 12, rowspan: 2, colspan: 1 },
                { row: 0, col: 13, rowspan: 2, colspan: 1 }
            ]
        });

        hstGrillaRC.render();
    }

    function descargarExcel(grillaPE, grillaRC) {
        $('#mensaje').html('');
        var arrDataPE = grillaPE.getData(),
            arrDataRC = grillaRC.getData(),
            arrPE = [],
            arrRC = [];

        for (var i = 2, j = arrDataPE.length - 1; i < j; i++) {
            arrPE.push(arrDataPE[i]);
        }

        for (var i = 2, j = arrDataRC.length - 1; i < j; i++) {
            arrRC.push(arrDataRC[i]);
        }

        $.ajax({
            type: 'POST',
            url: strControlador + 'GenerarExcel',
            async: false,
            data: { filas: JSON.stringify([arrPE, arrRC]) },
            success: function (response) {
                if (response.length > 0) {
                    window.location.href = strControlador + 'DescargarExcel?archivo=' + response;

                    $('#mensaje').html('El archivo se ha descargado satisfactoriamente.');
                } else {
                    $('#mensaje').html('El no se ha generado.');
                }
            },
            error: function (response) {
                $('#mensaje').html('ha ocurrido un error al descargar el archivo excel. ' + response.status + ' - ' + response.statusText + '.');
            }
        });
    }

    function leerGrillaBD(codEnvio) {
        $.ajax({
            type: 'POST',
            url: strControlador + 'LeerGrillaBD',
            async: false,
            data: {
                codigoenvio: codEnvio
            },
            success: function (info) {

                $('#tab-container').css("display", '');
                mostrarGrillaPE(info[0]);
                mostrarGrillaRC(info[1]);

                $('#mensaje').html('La información se ha obtenido satisfactoriamente.');
            },
            error: function (info) {
                $('#mensaje').html('ha ocurrido un error al leer registro ! ');
            }
        });
    }


    function headerRenderer(instance, td, row, col, prop, value, cellProperties) {
        Handsontable.renderers.TextRenderer.apply(this, arguments);

        $(td).addClass('hst-cus-header');
    }

    function grupoRenderer(instance, td, row, col, prop, value, cellProperties) {
        var arrDataSeleccionada = instance.getData(),
            strIdAnterior = (row >= intFilaInicio ? arrDataSeleccionada[row - 1][0] : null),
            strIdAactual = (row >= intFilaInicio ? arrDataSeleccionada[row][0] : null);;

        if (row === intFilaInicio || (strIdAnterior !== null)) {
            if (strIdAnterior != strIdAactual) {
                $(td).parent().addClass('hst-group-separator');
            }
        }
    }

    function puntoEntregaRenderer(instance, td, row, col, prop, value, cellProperties) {
        Handsontable.renderers.TextRenderer.apply(this, arguments);

        grupoRenderer(instance, td, row, col, prop, value, cellProperties);

        $(td).addClass('hst-align-center');
    }

    function nivelTensionRenderer(instance, td, row, col, prop, value, cellProperties) {
        Handsontable.renderers.AutocompleteRenderer.apply(this, arguments);

        $(td).addClass('hst-align-center');
    }

    function energiaSemestralRenderer(instance, td, row, col, prop, value, cellProperties) {
        Handsontable.renderers.TextRenderer.apply(this, arguments);

        if ($.isNumeric(value)) {
            value = parseInt(value);
        }

        $(td)
            .addClass('hst-align-center')
            .html(value);
    }

    function exoneradoFuerzaMayorRenderer(instance, td, row, col, prop, value, cellProperties) {
        Handsontable.renderers.AutocompleteRenderer.apply(this, arguments);

        $(td).addClass('hst-align-center');
    }

    function NiKiRenderer(instance, td, row, col, prop, value, cellProperties) {
        Handsontable.renderers.TextRenderer.apply(this, arguments);

        if ($.isNumeric(value)) {
            value = parseFloat(value);
        }

        $(td)
            .addClass('hst-align-center')
            .html(value);
    }

    function tiempoEjecutadoRenderer(instance, td, row, col, prop, value, cellProperties) {
        Handsontable.renderers.TextRenderer.apply(this, arguments);

        if (col === 11) {
            grupoRenderer(instance, td, row, col, prop, value, cellProperties);
        }

        $(td)
            .addClass('hst-align-center')
            .html(value);
    }

    function responsablePercentageRenderer(instance, td, row, col, prop, value, cellProperties) {
        Handsontable.renderers.TextRenderer.apply(this, arguments);

        if ($.isNumeric(value)) {
            value = (parseFloat(value) * 100) + '%';
        }

        $(td)
            .addClass('hst-align-right')
            .html(value);
    }

    function eiERenderer(instance, td, row, col, prop, value, cellProperties) {
        Handsontable.renderers.TextRenderer.apply(this, arguments);

        if ($.isNumeric(value)) {
            value = (parseFloat(value) * 100).toFixed(2) + '%';
        }

        $(td)
            .addClass('hst-cus-highlight hst-align-right')
            .html(value);
    }

    function resarcimientoRenderer(instance, td, row, col, prop, value, cellProperties) {
        Handsontable.renderers.TextRenderer.apply(this, arguments);

        grupoRenderer(instance, td, row, col, prop, value, cellProperties);

        if ($.isNumeric(value)) {
            value = parseFloat(value).toFixed(2);
        }

        $(td)
            .addClass('hst-cus-highlight hst-align-right')
            .html(value);
    }

    function sEDkVERenderer(instance, td, row, col, prop, value, cellProperties) {
        Handsontable.renderers.TextRenderer.apply(this, arguments);

        if ($.isNumeric(value)) {
            value = parseInt(value);
        }

        $(td)
            .addClass('hst-align-right')
            .html(value);
    }

    function eNSfRenderer(instance, td, row, col, prop, value, cellProperties) {
        Handsontable.renderers.TextRenderer.apply(this, arguments);

        if ($.isNumeric(value)) {
            value = parseInt(value);
        }

        $(td)
            .addClass('hst-align-right')
            .html(value);
    }
    function pkRenderer(instance, td, row, col, prop, value, cellProperties) {
        Handsontable.renderers.TextRenderer.apply(this, arguments);

        if ($.isNumeric(value)) {
            value = parseInt(value);
        }

        $(td)
            .addClass('hst-align-right')
            .html(value);
    }

    function eNSfkRenderer(instance, td, row, col, prop, value, cellProperties) {
        Handsontable.renderers.TextRenderer.apply(this, arguments);

        if ($.isNumeric(value)) {
            value = parseFloat(value).toFixed(2);
        }

        $(td)
            .addClass('hst-cus-highlight hst-align-right')
            .html(value);
    }

    function interrupcionRenderer(instance, td, row, col, prop, value, cellProperties) {
        Handsontable.renderers.TextRenderer.apply(this, arguments);

        var date = (value && value.length > 0 ? new Date(value) : value);

        if (value && value.length > 0 && !isNaN(date.valueOf())) {
            var intDia = date.getDate(),
                intMes = date.getMonth() + 1,
                intHoras = date.getHours(),
                intMinutos = date.getMinutes();

            intDia = (intDia < 10 ? '0' : '') + intDia;
            intMes = (intMes < 10 ? '0' : '') + intMes;
            intHoras = (intHoras < 10 ? '0' : '') + intHoras;
            intMinutos = (intMinutos < 10 ? '0' : '') + intMinutos;

            value = intDia + '/' + intMes + '/' + date.getFullYear() + ' ' + intHoras + ":" + intMinutos;
        }

        $(td)
            .addClass('hst-align-center')
            .html(value);
    }

    function porcentajeRenderer(instance, td, row, col, prop, value, cellProperties) {
        Handsontable.renderers.NumericRenderer.apply(this, arguments);

        value = (value == null || value === "" ? null : value + '%');

        $(td).html(value);
    }
});