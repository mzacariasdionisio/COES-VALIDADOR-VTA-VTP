$.message = "";
var strControlador = siteRoot + "resarcimientos/reporte/";

$(function () {
    var intFilaInicio = 3,
        hstGrillaPE = null,
        hstGrillaRC = null,
        arrPeriodo = null,
        arrCliente = null,
        arrBarra = null,
        arrNivelTension = null,
        arrTipo = null,
        arrBool = null,
        arrCodigoEventoCOES = null,
        arrEmpresa = null,
        $btnConsultar = $('#btn-consultar'),
        $btnSave = $('#btn-save'),
        $btnDescargarExcel = $('#btn-excel-descargar'),
        $btnValidarConsistencia = $('#btn-validar-consistencia'),
        $btnDescargarPlantilla = $('#btn-plantilla-descargar'),
        $btnImportarExcel = $('#btn-excel-cargar');


    $(document)
        .ready(function () {

            // Botones Inhabilitados por Defecto
            $btnDescargarExcel.prop("disabled", true);
            $btnDescargarExcel.css("color", 'darkgray');
            $btnValidarConsistencia.prop("disabled", true);
            $btnValidarConsistencia.css("color", 'darkgray');
            $btnSave.prop("disabled", true);
            $btnSave.css("color", 'darkgray');

            // -- >

            $('#dte-subasta-fecha').Zebra_DatePicker();

            $btnConsultar.click(function () {
                $btnDescargarExcel.prop("disabled", true);
                $btnDescargarExcel.css("color", 'darkgray');
                consultarReporteHistorico();
            });

            $("#btn-tabla_envios")
                .on('click',
                    function () {
                        $('#panel').toggle();
                    });

            $btnValidarConsistencia.click(function () {
                validarErrores(hstGrillaPE, hstGrillaRC);
            });

            $btnDescargarExcel.click(function () {
                descargarExcel(hstGrillaPE, hstGrillaRC);
            });

            $btnSave.click(function () {
                grabarReporte(hstGrillaPE, hstGrillaRC)
            });

            $btnDescargarPlantilla.click(function () {
                window.location.href = strControlador + 'DescargarPlantillaExcel';
            })

            $('.codenvio')
                .click(function () {
                    var arrGrillas = leerGrillaBD($(this).attr('data-ofercodi'));
                    $btnDescargarExcel.prop("disabled", false);
                    $btnDescargarExcel.css("color", '#3D90CB');
                });

            // Botones deshabilitados
            $('#CboEmpresasGeneradoras')
                .change(function () {
                    //validarEmpresasGeneradoras();
                });

            $('#tab-container').easytabs({ animate: false });
            $('#tab-container').css("display", 'none');

            cargarExcel();

            traerParametros();


            mostrarGrillaPE(null);
            mostrarGrillaRC(null);
        });

    function cargarExcel() {
        var uploader = new plupload.Uploader({
            runtimes: 'html5,flash,silverlight,html4',
            browse_button: 'btn-excel-cargar',
            url: strControlador + 'CargarExcel',
            flash_swf_url: '../js/Moxie.swf',
            silverlight_xap_url: '../js/Moxie.xap',
            filters: {
                max_file_size: '10mb',
                mime_types: [
                    { title: "Archivos de Excel", extensions: "xls,xlsm,xlsx" }
                ]
            },
            init: {
                FilesAdded: function (up, files) {
                    plupload.each(files, function (file) {
                        uploader.start();
                    });
                },
                FileUploaded: function (up, file, info) {

                    var arrGrillas = JSON.parse(info.response);
                    $('#tab-container').css("display", '');

                    mostrarGrillaPE(arrGrillas[0]);
                    mostrarGrillaRC(arrGrillas[1]);

                    //
                    arrPeriodo = obtenerPeriodo(arrGrillas[2]);
                    $('#mensaje')
                        .html('El archivo ha subido satisfactoriamente.')
                        .addClass('action-message');

                    //Activación del Botón Validar Consistencia y Enviar

                    $btnValidarConsistencia.prop("disabled", false);
                    $btnValidarConsistencia.css("color", '#3D90CB');
                    $btnSave.prop("disabled", false);
                    $btnSave.css("color", '#3D90CB');

                },
                Error: function (up, err) {
                    $('#mensaje').html('El archivo \'<b>' + up.files[0].name + '</b>\' no tiene el formato correcto.')
                            .addClass('action-error');
                }
            }
        });
        uploader.init();
    }

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
                $('#mensaje').html('No se han podido descargar los parámetros. ' + response.status + ' - ' + response.statusText + '.')
                        .addClass('action-error');
            }
        });
    }

    function validarEmpresasGeneradoras() {
        $.ajax({
            type: 'POST',
            global: false,
            url: strControlador + 'ValidarEmpresasGeneradoras',
            async: false,
            data: { empresa: $('#CboEmpresasGeneradoras').val() },
            success: function (response) {
                if (response == 0) {

                    $btnDescargarPlantilla.prop("disabled", true);
                    $btnImportarExcel.prop("disabled", true);
                    $btnDescargarExcel.prop("disabled", true);
                    $btnValidarConsistencia.prop("disabled", true);
                    $btnSave.prop("disabled", true);
                    $btnConsultar.prop("disabled", true);

                    $btnDescargarPlantilla.css("color", 'darkgray');
                    $btnImportarExcel.css("color", 'darkgray');
                    $btnDescargarExcel.css("color", 'darkgray');
                    $btnValidarConsistencia.css("color", 'darkgray');
                    $btnSave.css("color", 'darkgray');
                    $btnConsultar.css("color", 'darkgray');

                    $('#resultado').html('La empresa escogida no pertenece al usuario, por favor escoga nuevamente...').css("color", "red");

                } else if (response == 1) {

                    $btnDescargarPlantilla.prop("disabled", false);
                    $btnImportarExcel.prop("disabled", false);
                    $btnDescargarExcel.prop("disabled", false);
                    $btnValidarConsistencia.prop("disabled", false);
                    $btnSave.prop("disabled", false);
                    $btnConsultar.prop("disabled", false);

                    $btnDescargarPlantilla.css("color", '3D90CB');
                    $btnImportarExcel.css("color", '3D90CB');
                    $btnDescargarExcel.css("color", '3D90CB');
                    $btnValidarConsistencia.css("color", '3D90CB');
                    $btnSave.css("color", '3D90CB');
                    $btnConsultar.css("color", '3D90CB');

                    $('#resultado').html('')
                }
            },
            error: function (response) {
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
                puntoEntrega: 0,
                codigoEnvio: 0
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
                                    $('#btn-excel-descargar').css("color", '#3D90CB');

                                    mostrarGrillaPE(arrGrillas[0]);
                                    mostrarGrillaRC(arrGrillas[1]);

                                });
                        $('#tblTramites >tbody').append(tr);
                    }
                }
                else {

                    $('#resultado').html('No hay registros...').css("color", "red");
                }
            },
            error: function (req, status, error) {
            }
        });
    }

    function obtenerPeriodo(filas) {
        var n = parseInt(filas[0][0]);

        var periodoExcel = filas[n + 1][11];

        return periodoExcel;
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
            filedColumnsLeft: 2,
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
                            //case 26:
                            //  cellProperties.renderer = resarcimientoRenderer;
                            //  cellProperties.type = 'numeric';
                            // break;
                        case 27: // ID de cliente
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
            filedColumnsLeft: 2,
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
                            cellProperties.source = arrCliente[0];
                            break;
                        case 2:
                            cellProperties.type = 'autocomplete';
                            cellProperties.source = arrBarra[0];
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
                            cellProperties.renderer = compensableRenderer;
                            cellProperties.type = 'autocomplete';
                            cellProperties.source = arrBool;
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

                    $('#mensaje').html('El archivo se ha descargado satisfactoriamente.')
                        .addClass('action-message');
                } else {
                    $('#mensaje').html('El archivo no se ha generado.')
                        .addClass('action-error');
                }
            },
            error: function (response) {
                $('#mensaje').html('ha ocurrido un error al descargar el archivo excel. ' + response.status + ' - ' + response.statusText + '.')
                    .addClass('action-error');
            }
        });
    }

    function grabarReporte(grillaPE, grillaRC) {
        $('#mensaje').html('');
        var blnExito = validarErrores(grillaPE, grillaRC);
        var arrDataPE = grillaPE.getData(),
            arrDataRC = grillaRC.getData(),
            arrPE = [],
            arrRC = [];

        if (blnExito == "errorPeriodo") {
            var periodoExcel = arrPeriodo.substr(0, 5) + "S" + arrPeriodo.substr(14);
            $('#mensaje')
                .html('El periodo seleccionado no corresponde al archivo cargado, referencia: ' + periodoExcel)
                .addClass('action-error');
        }

        if (blnExito == true) {
            for (var i = 2, j = arrDataPE.length - 1; i < j; i++) {

                //Get IDCliente
                for (var n = 0, m = arrCliente[0].length; n < m; n++) {

                    if (arrCliente[0][n].trim() == arrDataPE[i][1]) {
                        arrDataPE[i][1] = arrCliente[1][n];
                        break;
                    }
                }
                //Get IDBarra
                for (var nb = 0, mb = arrBarra[0].length; nb < mb; nb++) {
                    if (arrBarra[0][nb].trim() == arrDataPE[i][2]) {
                        arrDataPE[i][2] = arrBarra[1][nb];
                        break;
                    }
                }

                if (arrDataPE[i][0] != null) {
                    //console.log('Grabar--- Pondra en push --- arrDataPE[i][0] = ' + arrDataPE[i][0]);
                    arrPE.push(arrDataPE[i]);
                }

            }

            for (var i = 2, j = arrDataRC.length - 1; i < j; i++) {
                //Get IDCliente
                for (var n = 0, m = arrCliente[0].length; n < m; n++) {
                    if (arrCliente[0][n].trim() == arrDataRC[i][1]) {
                        arrDataRC[i][1] = arrCliente[1][n];
                        break;
                    }
                }
                //Get IDBarra
                for (var nb = 0, mb = arrBarra[0].length; nb < mb; nb++) {
                    if (arrBarra[0][nb].trim() == arrDataRC[i][2]) {
                        arrDataRC[i][2] = arrBarra[1][nb];
                        break;
                    }
                }


                arrRC.push(arrDataRC[i]);
            }

            $.ajax({
                type: 'POST',
                url: strControlador + 'grabarReporte',
                async: false,
                data: {
                    filas: JSON.stringify([arrPE, arrRC]),
                    emprGeneradora: $('#CboEmpresasGeneradoras').val(),
                    periodo: $('#CboPeriodo').val(),
                    puntoEntrega: 0
                },
                success: function (response) {
                    $('#mensaje').html(response)
                        .addClass('action-exito');
                    if (response.substr(0, 22) == "Registro satisfactorio") {
                        $('#btn-excel-descargar').prop("disabled", false);
                        $('#btn-excel-descargar').css("color", '#3D90CB');
                    }

                },
                error: function (response) {
                    $('#mensaje').html('ha ocurrido un error al grabar registros! ' + response.status + ' - ' + response.statusText + '.')
                        .addClass('action-error');
                }

            });

        } else if (blnExito == false) {
            $('#mensaje').html('Error al grabar; aun existen errores, por favor Valide consistencia...')
                .addClass('action-error');
        }

        return blnExito;
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

                $('#mensaje').html('La información se ha obtenido satisfactoriamente.')
                    .addClass('action-message');
            },
            error: function (info) {
                //console.log('error al traer datos del controlador');
                $('#mensaje').html('ha ocurrido un error al leer registro ! ')
                    .addClass('action-error');
            }
        });
    }

    function validarErrores(grillaPE, grillaRC) {
        $('#mensaje').html('');
        var periodo = $('#CboPeriodo option:selected').html();
        var periodoExcel = arrPeriodo.substr(0, 5) + "S" + arrPeriodo.substr(14);

        if (periodo !== periodoExcel) {
            $('#mensaje')
                .html('El periodo seleccionado no corresponde al archivo cargado, referencia: ' + periodoExcel)
                .addClass('action-error');
            return "errorPeriodo";
        } else {

            var arrDataPE = grillaPE.getData(),
                arrDataRC = grillaRC.getData(),
                arrPE = [],
                arrRC = [];


            for (var i = 2, j = arrDataPE.length - 1; i < j; i++) {

                //Get IDCliente
                for (var n = 0, m = arrCliente[0].length; n < m; n++) {

                    if (arrCliente[0][n].trim() == arrDataPE[i][1]) {
                        arrDataPE[i][1] = arrCliente[1][n];
                        break;
                    }
                }
                //Get IDBarra
                for (var nb = 0, mb = arrBarra[0].length; nb < mb; nb++) {
                    if (arrBarra[0][nb].trim() == arrDataPE[i][2]) {

                        arrDataPE[i][2] = arrBarra[1][nb];
                        break;
                    }
                }

                if (arrDataPE[i][0] != null) {
                    //console.log('Validar--- Pondra en push --- arrDataPE[i][0] = ' + arrDataPE[i][0]);
                    arrPE.push(arrDataPE[i]);
                }

            }


            for (var i = 2, j = arrDataRC.length - 1; i < j; i++) {

                //Get IDCliente
                for (var n = 0, m = arrCliente[0].length; n < m; n++) {
                    if (arrCliente[0][n].trim() == arrDataRC[i][1]) {
                        arrDataRC[i][1] = arrCliente[1][n];
                        break;
                    }
                }
                //Get IDBarra
                for (var nb = 0, mb = arrBarra[0].length; nb < mb; nb++) {
                    if (arrBarra[0][nb].trim() == arrDataRC[i][2]) {
                        arrDataRC[i][2] = arrBarra[1][nb];
                        break;
                    }
                }


                arrRC.push(arrDataRC[i]);
            }

            if (arrPE.length == 0 || arrRC.length == 0) {
                blnExito = false;
                $('#mensaje')
                    .html('Las grillas no se han cargado correctamente...')
                    .addClass('action-error');
            } else {

                var blnExito = true;

                $.ajax({
                    type: 'POST',
                    url: strControlador + 'ValidarGrillas',
                    async: false,
                    data: {
                        filas: JSON.stringify([arrPE, arrRC]),
                        emprGeneradora: $('#CboEmpresasGeneradoras').val(),
                        periodo: $('#CboPeriodo').val(),
                        puntoEntrega: 0
                    },
                    success: function (response) {
                        blnExito = response[0];
                        var arrEsValido = response[1];
                        var listError = response[2];

                        for (var i = 0, j = arrEsValido[0].length; i < j; i++) {
                            for (var m = 0, n = arrEsValido[0][i].length; m < n; m++) {
                                grillaPE.setCellMeta(i + 2, m, 'valid', arrEsValido[0][i][m]);
                            }
                        }
                        //devolver valores textos

                        arrDataPE = grillaPE.getData()
                        for (var i = 2, j = arrDataPE.length - 1; i < j; i++) {
                            // Get NameCliente
                            for (var n = 0, m = arrCliente[0].length; n < m; n++) {
                                if (arrCliente[1][n] == arrDataPE[i][1]) {
                                    arrDataPE[i][1] = arrCliente[0][n];
                                    break;
                                }
                            }
                            // Get NameBarra
                            for (var nb = 0, mb = arrBarra[0].length; nb < mb; nb++) {
                                if (arrBarra[1][nb] == arrDataPE[i][2]) {
                                    arrDataPE[i][2] = arrBarra[0][nb];
                                    break;
                                }
                            }

                            if (arrDataPE[i][0] != null) {
                                //console.log('Validar(Reponer)--- Pondra en push --- arrDataPE[i][0] = ' + arrDataPE[i][0]);
                                arrPE.push(arrDataPE[i]);
                            }
                        }

                        grillaPE.render();

                        for (var i = 0, j = arrEsValido[1].length; i < j; i++) {
                            for (var m = 0, n = arrEsValido[1][i].length; m < n; m++) {
                                grillaRC.setCellMeta(i + 2, m, 'valid', arrEsValido[1][i][m]);
                            }
                        }

                        //Devolver valores 
                        arrDataRC = grillaRC.getData()
                        for (var i = 2, j = arrDataRC.length - 1; i < j; i++) {
                            // Get NameCliente
                            for (var n = 0, m = arrCliente[0].length; n < m; n++) {
                                if (arrCliente[1][n] == arrDataRC[i][1]) {
                                    arrDataRC[i][1] = arrCliente[0][n];
                                    break;
                                }
                            }
                            // Get NameBarra
                            for (var nb = 0, mb = arrBarra[0].length; nb < mb; nb++) {
                                if (arrBarra[1][nb] == arrDataRC[i][2]) {
                                    arrDataRC[i][2] = arrBarra[0][nb];
                                    break;
                                }
                            }
                            arrRC.push(arrDataRC[i]);
                        }

                        grillaRC.render();

                        $('#div-error-popup > div> table > tbody > tr').remove();

                        if (listError != null && listError.length > 0) {

                            blnExito = false;

                            $('#mensaje')
                                .html('Las grillas presentan errores.')
                                .addClass('action-error');

                            for (var i = 0, j = listError.length; i < j; i++) {

                                var objError = listError[j];

                                var tdTipoNtcse = $('<td>').html(listError[i].tipontcse),
                                    tdCelda = $('<td>').html(listError[i].celda),
                                    tdValor = $('<td>').html(listError[i].valor),
                                    tdTipo = $('<td>').html(listError[i].tipo),
                                    tr = $('<tr>')
                                        .data('celda', listError[i].tipontcse)
                                        .data('celda', listError[i].celda)
                                        .data('valor', listError[i].valor)
                                        .data('tipo', listError[i].tipo)
                                        .append(tdTipoNtcse)
                                        .append(tdCelda)
                                        .append(tdValor)
                                        .append(tdTipo)
                                $('#div-error-popup > div> table> tbody').append(tr);

                            }
                            var t = setTimeout(function () {
                                popup = $('#div-error-popup').bPopup({ modalclose: false, escclose: false });
                                clearTimeout(t);
                            },
                                1000);
                        } else {
                            $('#mensaje')
                                .html('No se presentan errores!')
                                .addClass('action-message');
                        }

                    },
                    error: function (response) {
                        blnExito = false;
                        $('#mensaje')
                            .html('ha ocurrido un error en la validación de las grillas. ' +
                                response.status +
                                ' - ' +
                                response.statusText +
                                '.')
                            .addClass('action-error');
                    }
                });

            }
        }
        return blnExito;
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
        //console.log(value);
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
            value = (parseFloat(value) * 100).toFixed(2) + '%';
        }

        $(td)
            .addClass('hst-align-right')
            .html(value);
    }

    function eiERenderer(instance, td, row, col, prop, value, cellProperties) {
        Handsontable.renderers.TextRenderer.apply(this, arguments);

        // console.log('EiRender value inicial= ' + value);

        if ($.isNumeric(value)) {
            value = (parseFloat(value) * 100).toFixed(2) + '%';
        }

        //console.log('EiRender value posterior = ' + value);

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

    function arrIDClienteRenderer(instance, td, row, col, prop, value, cellProperties) {
        Handsontable.renderers.TextRenderer.apply(this, arguments);

        grupoRenderer(instance, td, row, col, prop, value, cellProperties);

        if ($.isNumeric(value)) {
            value = parseInt(value);
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

        //if ($.isNumeric(value)) {
        //    value = parseInt(value);
        //}

        $(td)
            .addClass('hst-cus-highlight hst-align-right')
            .html(value);
    }

    function compensableRenderer(instance, td, row, col, prop, value, cellProperties) {
        Handsontable.renderers.AutocompleteRenderer.apply(this, arguments);

        $(td).addClass('hst-align-center');
    }

    function interrupcionRenderer(instance, td, row, col, prop, value, cellProperties) {
        Handsontable.renderers.TextRenderer.apply(this, arguments);

        //var date = (value && value.length > 0 ? new Date(value) : value);

        //if (value && value.length > 0 && !isNaN(date.valueOf())) {
        //    var intDia = date.getDate(),
        //        intMes = date.getMonth() + 1,
        //        intHoras = date.getHours(),
        //        intMinutos = date.getMinutes();

        //    intDia = (intDia < 10 ? '0' : '') + intDia;
        //    intMes = (intMes < 10 ? '0' : '') + intMes;
        //    intHoras = (intHoras < 10 ? '0' : '') + intHoras;
        //    intMinutos = (intMinutos < 10 ? '0' : '') + intMinutos;

        //    value = intDia + '/' + intMes + '/' + date.getFullYear() + ' ' + intHoras + ":" + intMinutos;
        //}

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