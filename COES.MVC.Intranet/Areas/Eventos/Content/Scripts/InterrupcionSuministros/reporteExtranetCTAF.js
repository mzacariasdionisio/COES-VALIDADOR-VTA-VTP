var controlador = siteRoot + 'Eventos/AnalisisFallas/';
var ANCHO_LISTADO = 900;
var TIPO_REPORTE_INTERRUPCION_ERACMF = 13;
var TIPO_REPORTE_INTERRUPCION = 14;
var TIPO_REPORTE_REDUCCION = 15;
var DATA_REPORTE = [];

var hansonCoordinacionEraqcmf;
var hansonCoordinacionInterrup;
var hansonCoordinacionEraqcmfSuministradora;
var hansonFuncionesEtapas;
var hansonAgenteDemoras;
var isValidEramf = true;
var isValidInterru = true;
var evt;

$(function () {
    $("#btnOcultarMenu").click();

    //Inicializar con Reporte interrupcion por defecto
    var tieneEracmf = $("#hfTieneCheckEracmf").val();
    $("#cbTipoInfo").val(tieneEracmf == "S" ? TIPO_REPORTE_INTERRUPCION_ERACMF : TIPO_REPORTE_INTERRUPCION);
    
    $("#btnRegresar").click(function () {
        location.href = controlador + "InterrupcionSuministros";
    });

    $("#cbEmpresa").change(function () {
        generarReporte();
    });
    $("#cbTipoInfo").change(function () {
        if ($("#cbTipoInfo").val() == TIPO_REPORTE_INTERRUPCION_ERACMF) {
            $('.trReporte').show();
            generarReporte();
        }
        else {
            $('.trReporte').hide();
            generarReporte();
        }
    });

    $("#cbTipoReporte").change(function () {
        var tipo = parseInt($("#cbTipoInfo").val()) || 0;
        var tipoReporte = parseInt($("#cbTipoReporte").val()) || 0;

        inicializarInterfaz(tipo);
        mostrarReporteTablaHtml(tipo, tipoReporte);
    });

    $("#btnConsultar").click(function () {
        generarReporte();
    });
    $("#btnExportar").click(function () {
        exportarReporte();
    });

    $("#btnExportarWord").click(function () {
        exportarReporteWord();
    });

    $("#btnGrabarCoordinacion").click(function (e) {
        guardarHorasCoordinacion(isValidEramf, hansonCoordinacionEraqcmf, hansonCoordinacionEraqcmfSuministradora);
    });

    $("#btnCopiarCoordinacion").click(function (e) {
        copiarHorasCoordinacion(hansonCoordinacionEraqcmf, hansonCoordinacionEraqcmfSuministradora);
    });

    $("#btnGrabarCoordinacion2").click(function (e) {
        guardarHorasCoordinacion(isValidInterru, hansonCoordinacionInterrup, hansonCoordinacionEraqcmfSuministradora);
    });

    $("#btnGrabarFuncionesEstapas").click(function (e) {
        var dataHandson = hansonFuncionesEtapas.getSourceData();

        var data = obtenerDataCondidicionesValido(dataHandson);
        if (data.length <= 0) {
            alert('No existe como mínino un registro completo.');
            return;
        }

        var dataJson = JSON.stringify(data);
        $.ajax({
            url: controlador + "GuardarCondiciones",
            type: 'POST',
            contentType: 'application/json; charset=UTF-8',
            dataType: 'json',
            data: dataJson,
            success: function (result) {
                alert(result.Resultado);
                $("#btnConsultar").trigger("click");
            },
            error: function (xhr, status) {
                alert('Se ha producido un error.');
            }
        });
    });

    $("#btnGrabarAgenteDemoras").click(function (e) {
        var dataHandson = hansonAgenteDemoras.getSourceData();

        var data = obtenerDataObserValido(dataHandson);
        if (data.length <= 0) {
            alert('No se ha registrado observación.');
            return;
        }
        var dataJson = JSON.stringify(data);
        $.ajax({
            url: controlador + "GuardarAgenteDemoras",
            type: 'POST',
            contentType: 'application/json; charset=UTF-8',
            dataType: 'json',
            data: dataJson,
            success: function (result) {
                alert(result.Resultado);
                $("#btnConsultar").trigger("click");
            },
            error: function (xhr, status) {
                alert('Se ha producido un error.');
            }
        });
    });

    // #region Config Handsontable

    var dtpConfig = {
        firstDay: 1,
        showWeekNumber: true,
        i18n: {
            previousMonth: 'Mes anterior',
            nextMonth: 'Mes siguiente',
            months: ['Enero', 'Febrero', 'Marzo', 'Abril', 'Mayo', 'Junio', 'Julio', 'Agosto', 'Septiembre', 'Octubre', 'Noviembre', 'Diciembre'],
            weekdays: ['Domingo', 'Lunes', 'Martes', 'Miercoles', 'Jueves', 'Viernes', 'Sabado'],
            weekdaysShort: ['Dom', 'Lun', 'Mar', 'Mier', 'Jue', 'Vie', 'Sab']
        }
    };


    // #endregion

    // #region Handsontable Coordinación 1

    hansonCoordinacionEraqcmf = new Handsontable(document.getElementById('tblHorasCoordinacion'), {
        dataSchema: {
            Emprcodi: null,
            Emprnomb: null,
            IntSubEdtacion: null,
            Afhofecha: null,
            EmpresaSuministradora:null
        },
        colHeaders: ['Empresa Cliente', 'Fecha y hora de Coordinación de Normalización','fd'],
        columns: [
            { data: 'Codigoosinergmin', editor: false, readOnly: true, className: "htCenter" },
            //{ data: 'Emprnombr', editor: false, readOnly: true, className: "htCenter" },
            //{ data: 'Intsumsubestacion', editor: false, readOnly: true, className: "htCenter" },
            { data: 'Afhofechadescripcion', type: 'date', dateFormat: 'DD/MM/YYYY HH:mm:ss', correctFormat: true, datePickerConfig: dtpConfig, className: "htCenter" }



            
        ],
        colWidths: [120, 200,100],
        columnSorting: true,
        minSpareRows: 0,
        rowHeaders: false,
        autoWrapRow: true,
        startRows: 0,
        stretchH: 'last',
        manualColumnResize: true,
        licenseKey: 'non-commercial-and-evaluation'
    });

    hansonCoordinacionEraqcmf.addHook('afterRender', function () {
        hansonCoordinacionEraqcmf.validateCells();
    });

    hansonCoordinacionEraqcmf.addHook('beforeValidate', function (value, row, prop, source) {
        isValidEramf = false;
    });

    hansonCoordinacionEraqcmf.addHook('afterValidate', function (isValid, value, row, prop, source) {

        if (prop === "Afhofechadescripcion") {
            isValidEramf = fechaInicioValidator(this, isValid, value, row, prop);
            return isValidEramf;
        }
    });

    // #endregion


    // #region Handsontable Coordinación Suministradora

    hansonCoordinacionEraqcmfSuministradora = new Handsontable(document.getElementById('tblHorasCoordinacionSuministradora'), {
        dataSchema: {
            Emprcodi: null,
            Emprnomb: null,
            IntSubEdtacion: null,
            Afhofecha: null
        },
        colHeaders: ['Empresa Suministradora', 'Fecha y hora de Coordinación de Normalización' + '<br>'+ '(dd/mm/yyyy hh:mm:ss)'],
        columns: [
            { data: 'EmpresaSuministradora', editor: false, readOnly: true, className: "htCenter" },
            //{ data: 'Emprnombr', editor: false, readOnly: true, className: "htCenter" },
            //{ data: 'Intsumsubestacion', editor: false, readOnly: true, className: "htCenter" },
            { data: 'Afhofechadescripcion', type: 'date', dateFormat: 'DD/MM/YYYY HH:mm:ss', correctFormat: true, datePickerConfig: dtpConfig, className: "htCenter" }
        ],
        colWidths: [250, 200],
        columnSorting: true,
        minSpareRows: 0,
        rowHeaders: false,
        autoWrapRow: true,
        startRows: 0,
        stretchH: 'last',
        manualColumnResize: true,
        licenseKey: 'non-commercial-and-evaluation'
    });

    hansonCoordinacionEraqcmfSuministradora.addHook('afterRender', function () {
        hansonCoordinacionEraqcmfSuministradora.validateCells();
    });

    hansonCoordinacionEraqcmfSuministradora.addHook('beforeValidate', function (value, row, prop, source) {
        isValidEramf = false;
    });

    hansonCoordinacionEraqcmfSuministradora.addHook('afterValidate', function (isValid, value, row, prop, source) {

        if (prop === "Afhofechadescripcion") {
            isValidEramf = fechaInicioValidator(this, isValid, value, row, prop);
            return isValidEramf;
        }
    });

    // #endregion
 

    // #region Handsontable Coordinación 2

    hansonCoordinacionInterrup = new Handsontable(document.getElementById('tblHorasCoordinacion2'), {
        dataSchema: {
            Emprcodi: null,
            Emprnomb: null,
            IntSubEdtacion: null,
            Afhofecha: null
        },
        colHeaders: ['Empresa','Nombre de Empresa','S.E', 'Fecha y hora de Coordinación de Normalización'],
        columns: [
            { data: 'Codigoosinergmin', editor: false, readOnly: true, className: "htCenter" },
            { data: 'Emprnombr', editor: false, readOnly: true, className: "htCenter" },
            { data: 'Intsumsubestacion', editor: false, readOnly: true, className: "htCenter" },
            { data: 'Afhofechadescripcion', type: 'date', dateFormat: 'DD/MM/YYYY HH:mm:ss', correctFormat: true, datePickerConfig: dtpConfig, className: "htCenter" }
        ],
        colWidths: [120,400, 80,300],
        columnSorting: true,
        minSpareRows: 0,
        rowHeaders: false,
        autoWrapRow: true,
        startRows: 0,
        stretchH: 'last',
        manualColumnResize: true,
        licenseKey: 'non-commercial-and-evaluation',
    });

    hansonCoordinacionInterrup.addHook('afterRender', function () {
        hansonCoordinacionEraqcmf.validateCells();
    });

    hansonCoordinacionInterrup.addHook('beforeValidate', function (value, row, prop, source) {
        isValidInterru = false;
    });

    hansonCoordinacionInterrup.addHook('afterValidate', function (isValid, value, row, prop, source) {

        if (prop === "Afhofechadescripcion") {
            isValidInterru = fechaInicioValidator(this, isValid, value, row, prop);
            return isValidInterru;
        }
    });

    setTimeout(function () { hansonCoordinacionInterrup.render(); }, 100);



    // #endregion



    // #region Handsontable funciones estapas

    hansonFuncionesEtapas = new Handsontable(document.getElementById('tblFuncionesEtapasActivas'), {
        dataSchema: {
            Afcondfuncion: null,
            Afcondnumetapadescrip: null,
            Afcondzona: null
        },
        colHeaders: ['Función', 'Etapa', 'Zona'],
        columns: [
            { data: 'Afcondfuncion', editor: 'select', selectOptions: ['f', 'Df'], className: "htCenter" },
            { data: 'Afcondnumetapadescrip', editor: 'select', className: "htCenter" },
            { data: 'Afcondzona', editor: 'select', className: "htCenter", selectOptions: ['Zona A', 'Zona B'] }
        ],
        colWidths: [200, 125, 125],
        columnSorting: true,
        minSpareRows: 0,
        rowHeaders: false,
        autoWrapRow: true,
        startRows: 0,
        manualColumnResize: true,
        stretchH: 'last',
        licenseKey: 'non-commercial-and-evaluation'
    });

    hansonFuncionesEtapas.addHook('afterRenderer', function (TD, row, column, prop, value, cellProperties) {
        if (prop === "Afcondnumetapadescrip") {
            var valueFunc = hansonFuncionesEtapas.getDataAtCell(row, column - 1);
            if (valueFunc === "f") {
                hansonFuncionesEtapas.setCellMeta(row, column, 'selectOptions', ['Etapa 1', 'Etapa 2', 'Etapa 3', 'Etapa 4', 'Etapa 5', 'Etapa 6', 'Etapa 7']);
            } else if (valueFunc === "Df") {
                hansonFuncionesEtapas.setCellMeta(row, column, 'selectOptions', ['Etapa 1', 'Etapa 2', 'Etapa 3']);
            }
        }
    });

    //setTimeout(function () { hansonFuncionesEtapas.render(); }, 100);

    // #endregion

    hansonAgenteDemoras = new Handsontable(document.getElementById('tblAgenteDemoras'), {
        dataSchema: {
            AfEmprenomb: null,
            Afhofechadescripcion: null,
            Afhmotivo: null
        },
        colHeaders: ['Empresa', 'Hora de Coordinación', 'Motivo'],
        columns: [
            { data: 'AfEmprenomb', editor: false, readOnly: true, className: "htCenter"  },
            { data: 'Afhofechadescripcion',editor: false, type: 'date', dateFormat: 'DD/MM/YYYY HH:mm:ss', correctFormat: true, datePickerConfig: dtpConfig, className: "htCenter" },
            {
                data: 'Afhmotivo',
                validator: function (value, callback) {
                    if (value != null) {
                        if (value.length > 140) {
                            callback(false);
                        }
                    }
                    callback(true);
                }
            }
        ],
        colWidths: [350, 250, 125],
        columnSorting: true,
        minSpareRows: 0,
        rowHeaders: false,
        autoWrapRow: true,
        startRows: 0,
        manualColumnResize: true,
        stretchH: 'last',
        licenseKey: 'non-commercial-and-evaluation'
    });

    hansonAgenteDemoras.addHook('afterRender', function () {
        hansonAgenteDemoras.validateCells();
    });

    if ($("#cbTipoInfo").val() == TIPO_REPORTE_INTERRUPCION_ERACMF) {
        $('.trReporte').show();
        generarReporte();
    }
    else {
        $('.trReporte').hide();
        generarReporte();
    }


    $('#btnDescargarFormato').click(function () {
      

        var data = {
            listaHorasCoordSuministradora: hansonCoordinacionEraqcmfSuministradora.getSourceData()
        };

        $.ajax({
            url: controlador + "DescargarFormatoInterrupcionSuministradora",
            type: 'POST',
            contentType: 'application/json; charset=UTF-8',
            dataType: 'json',
            data: JSON.stringify(data),
            success: function (result) {
                window.location = controlador + "Exportar?fi=" + result.Resultado; // si hay elementos
                //switch (result.NRegistros) {
                //    case 1: window.location = controlador + "Exportar?fi=" + result.Resultado; break;// si hay elementos
                //    case 2: alert("No existen registros !"); break;// sino hay elementos
                //    case -1: alert("Error en reporte result"); break;// Error en C#
                //}
            },
            error: function (xhr, status) {
                mostrarMensaje('mensajeInterr', 'error', 'Se ha producido un error.');
            }
        });
    });

    crearPupload();
});

function crearPupload() {

    var uploader = new plupload.Uploader({
        runtimes: 'html5,flash,silverlight,html4',
        browse_button: "btnImportar",
        url: controlador + "UploadInterrupcionSuministro",
        multi_selection: false,
        filters: {
            max_file_size: 0,
            mime_types: [
                { title: "Archivos xls", extensions: "xls,xlsx" }
            ]
        },
        init: {
            FilesAdded: function (up, files) {
                if (uploader.files.length === 2) {
                    uploader.removeFile(uploader.files[0]);
                }
                uploader.start();
                up.refresh();
            },
            UploadProgress: function (up, file) {
                mostrarMensaje('mensajeInterr', 'alert', "Por favor espere ...(<strong>" + file.percent + "%</strong>)");
            },

            FileUploaded: function (up, file, info) {
                var result = JSON.parse(info.response);
                if (result.NRegistros != -1) {
                    mostrarMensaje('mensajeInterr', 'exito', "<strong>Los datos se cargaron correctamente en el excel web, presione el botón enviar para grabar.</strong>");
                    hansonCoordinacionEraqcmfSuministradora.render();
                    cargarHandsonHorasCoordinacion(hansonCoordinacionEraqcmfSuministradora, result);
                } else {
                    mostrarMensaje('mensajeInterr', 'error', 'Se ha producido un error.');
                }
            },
            UploadComplete: function (up, file, info) {
            },
            Error: function (up, err) {
                mostrarError(err.code + "-" + err.message);
                if (err.code === -600) {
                    alert("Ha ocurrido un error"); return;
                }
            }
        }
    });

    uploader.bind('BeforeUpload', function (up, file) {
        //up.settings.multipart_params = {
        //    emprcodi: $("#cbEmpresa").val(),
        //    afecodi: $("#cbFechaHoraInicio").val(),
        //    idtipoinformacion: $('#CboReportar').val()
        //};
    });

    uploader.init();
}



function guardarHorasCoordinacion(isValid, hanson, hansonSuministro) {
    if (isValid) {
        var dataHandson = hanson.getSourceData();
        var dataHandsonSuministradora = hansonSuministro.getSourceData();
        var _afecodiSco = $("#hfAfecodiSco").val();

        var _dataHandson = obtenerDataValido(dataHandson);
        if (_dataHandson.length <= 0) {
            alert('No existe como mínino un registro completo.');
            return;
        }

        var dataSuministradora = obtenerDataValido(dataHandsonSuministradora);
       
        var datos = {
            listaHansonHoraCoord: _dataHandson,
            listaA: dataSuministradora,
            afecodiSco: _afecodiSco,
        }

        $.ajax({
            url: controlador + "GuardarHorasCoordinacion",
            type: 'POST',
            contentType: 'application/json; charset=UTF-8',
            dataType: 'json',
            data: JSON.stringify(datos),
            success: function (result) {
                if (evt.Resultado != "-1")
                    $("#btnConsultar").trigger("click");

                else
                    alert(evt.StrMensaje);


            },
            error: function (xhr, status) {
                alert('Se ha producido un error.');
            }
        });
    }
    else {
        alert('Celdas no validas marcadas en rojo en la columna "Fecha y hora de Coordinación de Normalización"');
    }
}

function copiarHorasCoordinacion(hanson, hansonSuministro) {
    var dataHandson = hanson.getSourceData();
    var dataHandsonSuministro = hansonSuministro.getSourceData();

    /*var lstHanson = obtenerDataValido(dataHandson);*/
    var lstHansonSuministro = obtenerDataValido(dataHandsonSuministro);

    if (lstHansonSuministro.length <= 0) {
        alert('No existe como mínino un registro completo para el copiado');
        return;
    }

    var data = {
        listansonHoraCoord: dataHandson,
        suministrolistaHansonHoraCoord: lstHansonSuministro,

    }

    var dataJson = JSON.stringify(data);
    $.ajax({
        url: controlador + "CopiarHorasCoordinacion",
        type: 'POST',
        contentType: 'application/json; charset=UTF-8',
        dataType: 'json',
        data: dataJson,
        success: function (result) {
            hansonCoordinacionEraqcmf.render();
            cargarHandsonHorasCoordinacion(hansonCoordinacionEraqcmf, result);
        },
        error: function (xhr, status) {
            alert('Se ha producido un error.');
        }
    });
    //if (isValid) {
        
    //}
    //else {
    //    alert('Celdas no validas marcadas en rojo en la columna "Fecha y hora de Coordinación de Normalización"');
    //}
}
function generarReporte() {
    var idEventoCtaf = $("#hfAfecodi").val();
    var tipo = parseInt($("#cbTipoInfo").val()) || 0;
    var empresa = parseInt($("#cbEmpresa").val()) || 0;
    var tipoReporte = parseInt($("#cbTipoReporte").val()) || 0;

    $(".tab-container").hide();
    $("#tab-containerTipo1").hide();
    $("#tab-containerTipo2").hide();
    $("#tab-containerTipo3").hide();

    cargarReporte(idEventoCtaf, tipo, empresa, tipoReporte);
}

function cargarReporte(idEventoCtaf, tipo, empresa, tipoReporte) {
    //var afeanio = parseInt($("#hAfeanio").val()) || 0;
    //var afecorr = parseInt($("#hAfecorr").val()) || 0;
    var afeanio = "";
    var afecorr = "";
    var _afecodi = 0;
    var inputs = document.querySelectorAll('.chCtaf');
    for (var k = 0; k < inputs.length; k++) {
        if (inputs[k].checked == true)
            _afecodi = inputs[k].value;
    }

    if (_afecodi > 0) {

        $.ajax({
            type: 'POST',
            url: controlador + 'ListarReporteHtmlByTipo',
            data: {
                afecodi: _afecodi,
                tipoReporte: tipo,
                emprcodi: empresa,
                anio: afeanio,
                correlativo: afecorr
            },
            success: function (evtResult) {
                evt = evtResult;
                inicializarInterfaz(tipo);
                ANCHO_LISTADO = $('#mainLayout').width() - 30;
                if (evt.Resultado != "-1")
                    mostrarReporteTablaHtml(tipo, tipoReporte);
                else
                    alert(evt.StrMensaje);
            },
            error: function (err) {
                alert("Ha ocurrido un error");
            }
        });
    }
    
}

function inicializarInterfaz(tipo) {
    $('#listado1Tipo1_pos0').html('');
    $('#listado1Tipo1_pos1').html('');
    $('#listado1Tipo1_pos2').html('');
    $('#listado1Tipo1_pos3').html('');
    $('#listado1Tipo1_pos4').html('');
    $('#listado1Tipo1_pos5').html('');
    $('#listado1Tipo1_pos6').html('');
    $('#listado1Tipo1_pos7').html('');
    $('#listado1Tipo1_pos8').html('');

    $('#listado7Tipo2_pos0').html('');
    $('#listado6Tipo2_pos3').html('');
    $('#listado8Tipo2_pos1').html('');
    $('#listado9Tipo2_pos2').html('');
    $('#listado10Tipo2_pos4').html('');

    $('#listado0Tipo3_pos0').html('');


    $(".tab-container").hide();
    $("#tab-containerTipo2").hide();
    $("#tab-containerTipo3").hide();

    switch (tipo) {
        case TIPO_REPORTE_INTERRUPCION_ERACMF:
            $(".tab-container").hide();
            break;
        case TIPO_REPORTE_INTERRUPCION:
            $("#tab-containerTipo2").show();
            $('#tab-containerTipo2').easytabs({
                animate: false
            });
            $('#tab-containerTipo2').bind('easytabs:after', function () {
                var id = $("#tab-containerTipo2 li.tab.active").attr("id");
                crearTablaHtml(id);
            });
            break;
        case TIPO_REPORTE_REDUCCION:
            $("#tab-containerTipo3").show();
            $('#tab-containerTipo3').easytabs({
                animate: false
            });
            $('#tab-containerTipo3').bind('easytabs:after', function () {
                var id = $("#tab-containerTipo3 li.tab.active").attr("id");
                crearTablaHtml(id);
            });
            break;
    }
}

function mostrarReporteTablaHtml(tipo, tipoReporte) {
    switch (tipo) {
        case TIPO_REPORTE_INTERRUPCION_ERACMF:
            DATA_REPORTE = evt.ListaReporte1Html;
            if (tipoReporte !== 8) {
                crearTablaHtml('listado1Tipo1_pos' + tipoReporte, tipo);
            }
            if (tipoReporte == 0) {
                hansonCoordinacionEraqcmf.render();
                hansonCoordinacionEraqcmfSuministradora.render();
                cargarHandsonHorasCoordinacion(hansonCoordinacionEraqcmf, evt.ListaHandsonHorasCoord);
                cargarHandsonHorasCoordinacion(hansonCoordinacionEraqcmfSuministradora, evt.ListaHandsonHorasSuministradora);
            }
            if (tipoReporte == 2) {
                hansonFuncionesEtapas.render();
                cargarHandsonCondiciones(evt.ListaHandsonEtapasFunc);
            }
            if (tipoReporte == 8) {
                $("#listado1Tipo1_pos8").parent().parent().parent().parent().show();
                hansonAgenteDemoras.render();
                cargarHandsonAgentesDemoras(evt.ListaHandsonAgentesDemoras);
            }
            break;
        case TIPO_REPORTE_INTERRUPCION:
            DATA_REPORTE = evt.ListaReporte2Html;
            crearTablaHtml('tablistado7Tipo2_pos0');
            crearTablaHtml('tablistado8Tipo2_pos1');
            crearTablaHtml('tablistado9Tipo2_pos2');
            crearTablaHtml('tablistado6Tipo2_pos3');
            crearTablaHtml('tablistado10Tipo2_pos4');
            hansonCoordinacionInterrup.render();
            cargarHandsonHorasCoordinacion(hansonCoordinacionInterrup, evt.ListaHandsonHorasCoord);
            break;
        case TIPO_REPORTE_REDUCCION:
            DATA_REPORTE = evt.ListaReporte3Html;
            crearTablaHtml('tablistado0Tipo3_pos0');
            break;
    }
}

function crearTablaHtml(idTab, tipo) {
    if (tipo == TIPO_REPORTE_INTERRUPCION_ERACMF) {
        var posArray = parseInt(idTab.substring(17, 18)) || 0;
        var idDivListado = '#' + idTab;
        var idTabla = "#" + "tablaRpt" + (posArray + 1);

        var dataHtml = DATA_REPORTE[posArray];

        $(idDivListado).parent().parent().parent().parent().show();

        $(idDivListado).html('');
        $(idDivListado).css("width", ANCHO_LISTADO + "px");
        $(idDivListado).html(dataHtml);
    } else {
        if (idTab.length > 21) {
            var posArray = parseInt(idTab.substring(21, 22)) || 0;
            var posTabla = parseInt(idTab.substring(10, 12)) || 0;
            var idDivListado = "#" + idTab.substring(3, 22);
            var idTabla = "#" + "tablaRpt" + posTabla;
        } else {
            var posArray = parseInt(idTab.substring(20, 21)) || 0;
            var posTabla = parseInt(idTab.substring(10, 11)) || 0;
            var idDivListado = "#" + idTab.substring(3, 21);
            var idTabla = "#" + "tablaRpt" + posTabla;
        }
       

        var dataHtml = DATA_REPORTE[posArray];
        $(idDivListado).html('');
        $(idDivListado).css("width", ANCHO_LISTADO + "px");
        $(idDivListado).html(dataHtml);
    }

    $(idTabla).dataTable({
        "scrollY": $(idDivListado).height() > 400 ? 400 + "px" : "100%",
        "scrollX": false,
        "sDom": 'ft',
        "ordering": true,
        "iDisplayLength": -1
    });
}

////////////////////////////////////////////////////////////////////////////////////////////////////////
/// Exportación
function exportarReporte() {
    var idEventoCtaf = $("#hfAfecodi").val() ;
    var tipo = parseInt($("#cbTipoInfo").val()) || 0;
    var empresa = parseInt($("#cbEmpresa").val()) || 0;
    var afeanio = parseInt($("#hAfeanio").val()) || 0;
    var afecorr = parseInt($("#hAfecorr").val()) || 0;

    $.ajax({
        type: 'POST',
        url: controlador + 'ExportarReporteExtranetCTAF',
        data: {
            afecodi: idEventoCtaf,
            tipoReporte: tipo,
            emprcodi: empresa,
            anio: afeanio,
            correlativo: afecorr
        },
        dataType: 'json',
        success: function (result) {
            if (result.Resultado !== "-1") {
                var paramList = [
                    { tipo: 'input', nombre: 'file1', value: result.NombreArchivo },
                ];
                var form = CreateForm(controlador + 'Exportar', paramList);
                document.body.appendChild(form);
                form.submit();
            }
            else {
                alert('Ha ocurrido un error');
            }
        },
        error: function (err) {
            alert('Ha ocurrido un error');
        }
    });
}

function exportarReporteWord() {
    var idEventoCtaf = $("#hfAfecodi").val();
    var tipo = parseInt($("#cbTipoInfo").val()) || 0;
    var empresa = parseInt($("#cbEmpresa").val()) || 0;
    var afeanio = parseInt($("#hAfeanio").val()) || 0;
    var afecorr = parseInt($("#hAfecorr").val()) || 0;

    $.ajax({
        type: 'POST',
        url: controlador + 'ExportarReporteWordCTAF',
        data: {
            afecodi: idEventoCtaf,
            tipoReporte: tipo,
            emprcodi: empresa,
            anio: afeanio,
            correlativo: afecorr
        },
        dataType: 'json',
        success: function (result) {
            if (result.Resultado !== "-1") {
                var paramList = [
                    { tipo: 'input', nombre: 'file1', value: result.NombreArchivo },
                ];
                var form = CreateForm(controlador + 'Exportar', paramList);
                document.body.appendChild(form);
                form.submit();
            }
            else {
                alert('Ha ocurrido un error');
            }
        },
        error: function (err) {
            alert('Ha ocurrido un error');
        }
    });
}

function CreateForm(path, params, method = 'post') {
    var form = document.createElement('form');
    form.method = method;
    form.action = path;

    $.each(params, function (index, obj) {
        var hiddenInput = document.createElement(obj.tipo);
        hiddenInput.type = 'hidden';
        hiddenInput.name = obj.nombre;
        hiddenInput.value = obj.value;
        form.appendChild(hiddenInput);
    });
    return form;
}

////////////////////////////////////////////////////////////////////////////////////////////////////////
function cargarHandsonHorasCoordinacion(handson, ListaHandsonHorasCoord) {
    if (ListaHandsonHorasCoord) {
        handson.loadData(ListaHandsonHorasCoord);
    }
}

function cargarHandsonCondiciones(ListaHandson) {
    if (ListaHandson) {
        hansonFuncionesEtapas.loadData(ListaHandson);
    }
}

function cargarHandsonAgentesDemoras(ListaHandson) {
    if (ListaHandson) {
        hansonAgenteDemoras.loadData(ListaHandson);        
    }
}

function obtenerDataValido(dataHandson) {
    var lstData = [];
    for (var index in dataHandson) {
        var item = dataHandson[index];

        if (item.Afhofechadescripcion) {
            lstData.push(item);
        }
    }
    return lstData;
}

function obtenerDataCondidicionesValido(dataHandson) {
    var lstData = [];
    for (var index in dataHandson) {
        var item = dataHandson[index];

        if (item.Afcondfuncion && item.Afcondnumetapadescrip && item.Afcondzona) {
            lstData.push(item);
        }
    }
    return lstData;
}

function obtenerDataObserValido(dataHandson) {
    var lstData = [];
    for (var index in dataHandson) {
        var item = dataHandson[index];

        if (item.Afhmotivo) {
            lstData.push(item);
        }
    }
    return lstData;
}

function fechaInicioValidator(instance, isValid, value, row, prop) {

    var error = [];

    if (value) {
        var fechaIni = moment(value, 'DD/MM/YYYY HH:mm:ss');
        if (fechaIni.isValid()) {
            var fechaEvento = moment($("#hfFechaInterrupcion").val(), 'DD/MM/YYYY HH:mm:ss');
            if (fechaEvento.isValid()) {
                isValid = fechaIni > fechaEvento;
                //var columnName = instance.getColHeader(instance.propToCol(prop));
                error = {
                    address: `[<b>Fecha inicio</b>] [<b>${row + 1}</b>]`,
                    valor: value,
                    className: "htInvalid",
                    message: "El dato es anterior a la fecha y hora de inicio de interrupción"
                };
            }
        } else {
            isValid = false;
            error = {
                address: `[<b>Fecha inicio</b>] [<b>${row + 1}</b>]`,
                valor: value,
                className: "htInvalid",
                message: "El dato no es de tipo fecha"
            };
        }
    }
    return isValid;
}
function Check(checkbox) {
    var afecodi = 0;
    var inputs = document.querySelectorAll('.chCtaf');
    for (var k = 0; k < inputs.length; k++) {
        if (inputs[k].id != checkbox.id)
            inputs[k].checked = false;

        if (inputs[k].checked == true) {
            afecodi = inputs[k].value;
            $("#hfAfecodiSco").val(afecodi);
        }
            
    }
    console.log($("#hfAfecodiSco").val());
    if (afecodi > 0) {
        var select = document.getElementById("cbTipoInfo"); //Seleccionamos el select
        var valorcbtipo;
        $.ajax({
            type: 'POST',
            url: controlador + 'ListarTipoInterrupcion',
            data: {
                afecodi: afecodi
            },
            success: function (evtResult) {
                $("#cbTipoInfo").empty();
                evt = evtResult;
                for (var x = 0; x < evt.ListaTipoInformacion.length; x++) {
                    var option = document.createElement("option"); //Creamos la opcion
                    option.value = evt.ListaTipoInformacion[x].Fdatcodi;
                    option.text = evt.ListaTipoInformacion[x].Fdatnombre
                    select.appendChild(option); //Metemos la opción en el select
                }
                if ($("#cbTipoInfo").val() == TIPO_REPORTE_INTERRUPCION_ERACMF)
                    $('.trReporte').show();
                else
                    $('.trReporte').hide();

                inicializarInterfaz(0);
            },
            error: function (err) {
                alert("Ha ocurrido un error");
            }
        });
    }
    //generarReporte();
};
