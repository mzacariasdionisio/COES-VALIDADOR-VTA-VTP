var controlador = siteRoot + 'calculoresarcimiento/insumos/';
var hotIndicador = null;
var hotTolerancia = null;
var uploaderInterrupcion;

$(function () {

    $('#tab-container').easytabs({
        animate: false
    });

    $('#tab-container').bind('easytabs:before', function (id, val, t) {
        limpiarMensaje('mensaje');
    });

    $('#btnConsultar').on('click', function () {
        consultar();
    });

    $('#txtAnio').Zebra_DatePicker({
        format: 'Y',
        onSelect: function (date) {
            cargarPeriodos(date)
        }
    });

    $('#cbPeriodo').on('change', function () {
        $('#tab-container').hide();
    });

    $('#btnNuevoPtoEntrega').on('click', function () {
        editarPtoEntrega(0);
    });

    $('#btnExcelPtoEntrega').on('click', function () {
        habilitarCargaPtoEntrega();
    });

    $('#btnImportarSuministradores').on('click', function () {
        habilitarCargaSuministrador();
    });

    $('#btnExportarPtoEntrega').on('click', function () {
        exportarPuntoEntrega();
    });

    $('#btnExportarSuministrador').on('click', function () {
        exportarSuministradores();
    });

    $('#btnGrabarIndicador').on('click', function () {
        grabarIndicador();
    });

    $('#btnGrabarTolerancia').on('click', function () {
        grabarTolerancia();
    });

    $('#btnNuevoIngreso').on('click', function () {
        editarIngreso(0);
    });

    $('#btnExportarIngreso').on('click', function () {
        exportarIngresos();
    });

    $('#btnNuevoEvento').on('click', function () {
        editarEvento(0);
    });

    $('#btnExportarEvento').on('click', function () {
        exportarEventos();
    });

    $('#btnCargarEvento').on('click', function () {
        habilitarCargaEvento();
    });

    $('#btnDescargarFormatoInterrupcion').on('click', function () {
        descargarFormatoInterrupcion();
    });

    $('#btnSubirFormatoInterrupcion').on('click', function () {
        subirFormatoInterrupcion();
    });

    $('#btnEnviarDatosInterrupcion').on('click', function () {
        enviarDatosInterrupcion();
    });

    $('#btnMostrarErroresInterrupcion').on('click', function () {
        mostrarErroresInterrupcion();
    });

    $('#tab-container').hide();

    cargarUploaderPtoEntrega();
    cargarUploaderSuministrador();
    cargarUploaderEvento();
    cargarUploaderInterrupcion();
});

function cargarPeriodos(anio) {
    $.ajax({
        type: 'POST',
        url: controlador + 'ObtenerPeriodos',
        data: {
            anio: anio
        },
        dataType: 'json',
        global: false,
        success: function (result) {
            if (result != -1) {
                $('#cbPeriodo').get(0).options.length = 0;
                $('#cbPeriodo').get(0).options[0] = new Option("--SELECCIONE--", "");
                $.each(result, function (i, item) {
                    $('#cbPeriodo').get(0).options[$('#cbPeriodo').get(0).options.length] = new Option(item.Repernombre, item.Repercodi);
                });
                $('#tab-container').hide();
            }
            else {
                mostrarMensaje('mensaje', 'error', 'Se ha producido un error.');
            }
        },
        error: function () {
            mostrarMensaje('mensaje', 'error', 'Se ha producido un error.');
        }
    });
}

function consultar() {
    if ($('#cbPeriodo').val() != "") {
        $('#tab-container').show();
        limpiarMensaje('mensaje');
        cargarPtoEntrega();
        cargarIndicadores();
        cargarTolerancias();
        cargarIngresos();
        cargarEventos();
        cargarInterrupciones();
    }
    else {
        mostrarMensaje('mensaje', 'alert', 'Por favor seleccione un periodo.');
    }
}

function cargarPtoEntrega() {
    $.ajax({
        type: 'POST',
        url: controlador + 'ListadoPtoEntrega',
        data: {
            periodo: $('#cbPeriodo').val()
        },
        success: function (evt) {
            $('#listadoPtoEntrega').html(evt);
            $('#tablaPtoEntrega').dataTable({
                "iDisplayLength": 25
            });
        },
        error: function () {
            mostrarMensaje('mensaje', 'error', 'Ha ocurrido un error.');
        }
    });
}

function editarPtoEntrega(id) {
    $.ajax({
        type: 'POST',
        url: controlador + 'editarPtoEntrega',
        data: {
            id: id
        },
        global: false,
        success: function (evt) {
            $('#contenidoPtoEntrega').html(evt);
            setTimeout(function () {
                $('#popupPtoEntrega').bPopup({
                    autoClose: false,
                    modalClose: false,
                    escClose: false,
                    follow: [false, false]
                });
            }, 50);

            $('#btnGrabarPtoEntrega').on("click", function () {
                grabarPtoEntrega();
            });

            $('#cbPtoEntrega').multipleSelect({
                width: '200px',
                filter: true,
                selectAll: false,
                single: true
            });

            $('#btnCancelarPtoEntrega').on("click", function () {
                $('#popupPtoEntrega').bPopup().close();
            });          
        },
        error: function () {
            mostrarMensaje('mensaje', 'error', 'Se ha producido un error.');
        }
    });
}

function grabarPtoEntrega() {
    var ptoEntrega = $('#cbPtoEntrega').multipleSelect('getSelects');
    
    if (ptoEntrega.length > 0) {
        $.ajax({
            type: 'POST',
            url: controlador + 'grabarptoentrega',
            data: {
                ptoEntrega: ptoEntrega[0],
                periodo: $('#cbPeriodo').val()
            },
            dataType: 'json',           
            success: function (result) {
                if (result == 1) {
                    $('#popupPtoEntrega').bPopup().close();
                    mostrarMensaje('mensaje', 'exito', 'El punto de entrega se agregó correctamente.');
                    cargarPtoEntrega();
                }
                else if (result == 2) {
                    mostrarMensaje('mensajeEdicion', 'alert', "El punto de entrega ya se encuentra registrado.");
                }
                else {
                    mostrarMensaje('mensajeEdicion', 'error', 'Se ha producido un error.');
                }
            },
            error: function () {
                mostrarMensaje('mensajeEdicion', 'error', 'Se ha producido un error.');
            }
        });
    }
    else {
        mostrarMensaje('mensajeEdicion', 'alert', 'Seleccione punto de entrega.');
    }
}

function eliminarPtoEntrega(id) {
    if (confirm('¿Está seguro de realizar esta operación?')) {

        $.ajax({
            type: 'POST',
            url: controlador + 'eliminarptoentrega',
            data: {
                id: id,
                periodo: $('#cbPeriodo').val()
            },
            dataType: 'json',
            success: function (result) {
                if (result == 1) {
                    mostrarMensaje('mensaje', 'exito', 'El registro se eliminó correctamente.');
                    cargarPtoEntrega();
                }
                else {
                    mostrarMensaje('mensaje', 'error', 'Se ha producido un error.');
                }
            },
            error: function () {
                mostrarMensaje('mensaje', 'error', 'Se ha producido un error.');
            }
        });
    }
}

function cargarUploaderPtoEntrega() {
    var uploaderPtoEntrega = new plupload.Uploader({
        runtimes: 'html5,flash,silverlight,html4',
        browse_button: 'btnSelectFilePtoEntrega',
        container: document.getElementById('containerPtoEntrega'),
        url: controlador + 'uploadptoentrega',
        flash_swf_url: '~/Content/Scripts/Moxie.swf',
        silverlight_xap_url: '~/Content/Scripts/Moxie.xap',
        multi_selection: false,
        filters: {
            max_file_size: '2mb',
            mime_types: [
                { title: "Archivos Excel .xlsx", extensions: "xlsx" }
            ]
        },
        init: {
            PostInit: function () {
                document.getElementById('btnProcesarFilePtoEntrega').onclick = function () {
                    if (uploaderPtoEntrega.files.length > 0) {
                        uploaderPtoEntrega.start();
                    }
                    else                      
                        loadValidacionFile('fileInfoPtoEntrega', 'Seleccione archivo');
                    return false;
                };
            },
            FilesAdded: function (up, files) {
                if (uploaderPtoEntrega.files.length == 2) {
                    uploaderPtoEntrega.removeFile(uploaderPtoEntrega.files[0]);
                }
                plupload.each(files, function (file) {
                    loadInfoFile('fileInfoPtoEntrega', file.name, plupload.formatSize(file.size));
                });
                uploaderPtoEntrega.refresh();
            },
            UploadProgress: function (up, file) {
                mostrarProgreso('progresoPtoEntrega', file.percent);
            },
            UploadComplete: function (up, file) {
                importarPtoEntrega();
            },
            Error: function (up, err) {
                loadValidacionFile('fileInfoPtoEntrega', err.code + "-" + err.message);
            }
        }
    });

    uploaderPtoEntrega.init();
}


function cargarUploaderSuministrador() {
    var uploaderSuministrador = new plupload.Uploader({
        runtimes: 'html5,flash,silverlight,html4',
        browse_button: 'btnSelectFileSuministrador',
        container: document.getElementById('containerSuministrador'),
        url: controlador + 'uploadsuministrador',
        flash_swf_url: '~/Content/Scripts/Moxie.swf',
        silverlight_xap_url: '~/Content/Scripts/Moxie.xap',
        multi_selection: false,
        filters: {
            max_file_size: '2mb',
            mime_types: [
                { title: "Archivos Excel .xlsx", extensions: "xlsx" }
            ]
        },
        init: {
            PostInit: function () {
                document.getElementById('btnProcesarFileSuministrador').onclick = function () {
                    if (uploaderSuministrador.files.length > 0) {
                        uploaderSuministrador.start();
                    }
                    else
                        loadValidacionFile('fileInfoSuministrador', 'Seleccione archivo');
                    return false;
                };
            },
            FilesAdded: function (up, files) {
                if (uploaderSuministrador.files.length == 2) {
                    uploaderSuministrador.removeFile(uploaderSuministrador.files[0]);
                }
                plupload.each(files, function (file) {
                    loadInfoFile('fileInfoSuministrador', file.name, plupload.formatSize(file.size));
                });
                uploaderSuministrador.refresh();
            },
            UploadProgress: function (up, file) {
                mostrarProgreso('progresoSuministrador', file.percent);
            },
            UploadComplete: function (up, file) {
                importarSuministradores();
            },
            Error: function (up, err) {
                loadValidacionFile('fileInfoSuministrador', err.code + "-" + err.message);
            }
        }
    });

    uploaderSuministrador.init();
}

function habilitarCargaPtoEntrega() {
    $('#divImportarPtoEntrega').toggle();
    $('#progresoPtoEntrega').removeClass();
    $('#fileInfoPtoEntrega').removeClass();
    $('#progresoPtoEntrega').html('');
    $('#fileInfoPtoEntrega').html('');
}


function habilitarCargaSuministrador() {
    $('#divImportarSuministrador').toggle();
    $('#progresoSuministrador').removeClass();
    $('#fileInfoSuministrador').removeClass();
    $('#progresoSuministrador').html('');
    $('#fileInfoSuministrador').html('');
}


function importarPtoEntrega() {
    $.ajax({
        type: 'POST',
        url: controlador + 'ImportarPtoEntrega',
        data: {            
            periodo: $('#cbPeriodo').val()
        },
        dataType: 'json',
        success: function (result) {
            if (result.Result == 1) {
                mostrarMensaje('mensaje', 'exito', 'La carga de puntos de entrega se realizó correctamente.');
                cargarPtoEntrega();
                habilitarCargaPtoEntrega();
            }
            else if (result.Result == 2) {
                var html = "No se realizó la carga por que se encontraron los siguientes errores: <ul>";

                for (var i in result.Errores) {
                    html = html + "<li>" + result.Errores[i] + "</li>";
                }
                html = html + "</ul>";
                mostrarMensaje('mensaje', 'alert', html);
                habilitarCargaPtoEntrega();
            }
            else if (result.Result == -1) {
                mostrarMensaje('mensaje', 'error', 'Se ha producido un error.');
            }
        },
        error: function () {
            mostrarMensaje('mensaje', 'error', 'Se ha producido un error.');
        }
    });
}


function importarSuministradores() {
    $.ajax({
        type: 'POST',
        url: controlador + 'ImportarSuministradores',
        data: {
            periodo: $('#cbPeriodo').val()
        },
        dataType: 'json',
        success: function (result) {
            if (result.Result == 1) {
                mostrarMensaje('mensaje', 'exito', 'La carga de suministradores por punto de entrega se realizó correctamente.');                
                habilitarCargaSuministrador();
            }
            else if (result.Result == 2) {
                var html = "No se realizó la carga por que se encontraron los siguientes errores: <ul>";

                for (var i in result.Errores) {
                    html = html + "<li>" + result.Errores[i] + "</li>";
                }
                html = html + "</ul>";
                mostrarMensaje('mensaje', 'alert', html);
                habilitarCargaSuministrador();
            }
            else if (result.Result == -1) {
                mostrarMensaje('mensaje', 'error', 'Se ha producido un error.');
            }
        },
        error: function () {
            mostrarMensaje('mensaje', 'error', 'Se ha producido un error.');
        }
    });
}


function exportarPuntoEntrega() {
    $.ajax({
        type: 'POST',
        url: controlador + "ExportarPuntoEntrega",
        data: {
            periodo: $('#cbPeriodo').val()
        },
        dataType: 'json',
        cache: false,
        success: function (resultado) {
            if (resultado == 1) {
                location.href = controlador + "DescargarPuntoEntrega";
            }
            else {
                mostrarMensaje('mensaje', 'error', 'Ha ocurrido un error.');
            }
        },
        error: function () {
            mostrarMensaje('mensaje', 'error', 'Ha ocurrido un error.');
        }
    });
}


function exportarSuministradores() {
    $.ajax({
        type: 'POST',
        url: controlador + "ExportarSuministradores",
        data: {
            periodo: $('#cbPeriodo').val()
        },
        dataType: 'json',
        cache: false,
        success: function (resultado) {
            if (resultado == 1) {
                location.href = controlador + "DescargarSuministradores";
            }
            else {
                mostrarMensaje('mensaje', 'error', 'Ha ocurrido un error.');
            }
        },
        error: function () {
            mostrarMensaje('mensaje', 'error', 'Ha ocurrido un error.');
        }
    });
}

function configurarSuministrador(idPtoEntrega) {
    limpiarMensaje('mensaje');
    $.ajax({
        type: 'POST',
        url: controlador + 'editarSuministrador',
        data: {
            idPtoEntrega: idPtoEntrega,
            periodo: $('#cbPeriodo').val()
        },
        global: false,
        success: function (evt) {
            $('#contenidoSuministrador').html(evt);
            setTimeout(function () {
                $('#popupSuministrador').bPopup({
                    autoClose: false,
                    modalClose: false,
                    escClose: false,
                    follow: [false, false]
                });
            }, 50);

            $('#btnAgregarSuministrador').on('click', function () {
                agregarSuministrador();
            });

            $('#btnGrabarSuministrador').on("click", function () {
                grabarPtoEntregaSuministrador();
            });

            $('#btnCancelarSuministrador').on("click", function () {
                $('#popupSuministrador').bPopup().close();
            });

            $('#hfIdPuntoEntrega').val(idPtoEntrega);
        },
        error: function () {
            mostrarMensaje('mensaje', 'error', 'Se ha producido un error.');
        }
    });
}

function agregarSuministrador() {
    if ($('#cbSuministrador').val() != "") {
        var emprCodi = $('#cbSuministrador').val();

        var count = 0;
        var flag = true;
        $('#tablaSuministrador>tbody tr').each(function (i) {
            $punto = $(this).find('#hfIdSuministrador');
            if ($punto.val() == emprCodi) {
                flag = false;
            }
        });

        if (flag) {
            $('#tablaSuministrador> tbody').append(
                '<tr>' +
                '   <td style="text-align:center">' +
                '       <img src="' + siteRoot + 'Content/Images/btn-cancel.png" alt="" onclick="$(this).parent().parent().remove();" style="cursor:pointer" />' +
                '       <input type="hidden" id="hfIdSuministrador" value="' + emprCodi + '" /> ' +
                '   </td>' +
                '   <td>' + $("#cbSuministrador option:selected").text() + '</td>' +
                '</tr>'
            );
            limpiarMensaje("mensajeSuministrador");
        }
        else {
            mostrarMensaje('mensajeSuministrador', 'alert', 'El suministrador seleccionado ya está relacionado al punto de entrega.en el periodo elegido.');
        }
    }
    else {
        mostrarMensaje('mensajeSuministrador', 'alert', 'Seleccione un suministrador.');
    }
}

function grabarPtoEntregaSuministrador() {
    var count = 0;
    var items = "";
    $('#tablaSuministrador>tbody tr').each(function (i) {
        $punto = $(this).find('#hfIdSuministrador');
        var constante = (count > 0) ? "," : "";
        items = items + constante + $punto.val();
        count++;
    });

    if (true) {
        $.ajax({
            type: 'POST',
            url: controlador + 'GrabarSuministrador',
            data: {
                idPtoEntrega: $('#hfIdPuntoEntrega').val(),
                periodo: $('#cbPeriodo').val(),
                suministradores: items
            },
            dataType: 'json',
            success: function (result) {
                if (result == 1) {                    
                    mostrarMensaje('mensaje', 'exito', 'La operación se realizó correctamente.');
                    $('#popupSuministrador').bPopup().close();
                }                
                else {
                    mostrarMensaje('mensajeSuministrador', 'error', 'Se ha producido un error.');
                }
            },
            error: function () {
                mostrarMensaje('mensajeSuministrador', 'error', 'Se ha producido un error.');
            }
        });
    }
    else {
        mostrarMensaje('mensajeSuministrador', 'alert', 'Se debe agregar uno o más suministradores.');
    }
}

function cargarIndicadores() {
    $.ajax({
        type: 'POST',
        url: controlador + 'ObtenerIndicadores',
        data: {
            periodo: $('#cbPeriodo').val()
        },
        dataType: 'json',
        success: function (result) {
            cargarGrillaIndicador(result.Data, result.RowSpans);
        },
        error: function () {
            mostrarMensaje('mensaje', 'error', 'Se ha producido un error.');
        }
    });
}

function cargarGrillaIndicador(result, rowspan) {
    if (hotIndicador != null) {
        hotIndicador.destroy();
    }
    var container = document.getElementById('excelIndicador');

    var disabledRenderer = function (instance, td, row, col, prop, value, cellProperties) {
        Handsontable.renderers.TextRenderer.apply(this, arguments);
        td.style.backgroundColor = '#F2F2F2';
        td.style.fontSize = '11px';
        td.style.verticalAlign = 'middle';
    };

    var merge = [];
   
    var row = 0;
    for (var j in rowspan) {
        merge.push({ row: row, col: 1, rowspan: rowspan[j], colspan: 1 });
        row = row + rowspan[j];
    }
  

    hotIndicador = new Handsontable(container, {
        data: result,
        mergeCells: merge,
        colHeaders: ['Id', 'Tipo de Interrupción', 'Causa de Interrupción', 'Ni', 'Ki'],
        columns: [
            {},
            {},
            {},
            {                
            },
            {               
            }
        ],

        comments: true,
        colWidths: [1, 200, 320, 80, 80],
        maxRows: result.length,
        cells: function (row, col, prop) {
            var cellProperties = {};

            if (col < 3) {
                cellProperties.renderer = disabledRenderer;
                cellProperties.readOnly = true;
            }
            
            return cellProperties;
        }
    });
}

function grabarIndicador() {
    
    var data = hotIndicador.getData();
   
    var html = "<ul>";
    var flag = true;

    for (i = 0; i < data.length; i++) {
        if (data[i][3] == "") {
            html = html + "<li>Ingrese el indicador Ni para la causa " + data[i][2] + "</li>";
            flag = false;
        }
        else {
            if (!validarNumero(data[i][3])) {
                html = html + "<li>El valor del indicador Ni de la causa " + data[i][2] + " debe ser numérico.</li>";
                flag = false;
            }
            else {
                if (!(parseFloat(data[i][3]) >= 0 && parseFloat(data[i][3]) <= 1)) {
                    html = html + "<li>El valor del indicador Ni de la causa " + data[i][2] + " debe estar entre 0 y 1.</li>";
                    flag = false;
                }
            }
        }

        if (data[i][4] == "") {
            html = html + "<li>Ingrese el indicador Ki para la causa " + data[i][2] + "</li>";
            flag = false;
        }
        else {
            if (!validarNumero(data[i][4])) {
                html = html + "<li>El valor del indicador Ki de la causa " + data[i][2] + " debe ser numérico.</li>";
                flag = false;
            }
            else {
                if (!(parseFloat(data[i][4]) >= 0 && parseFloat(data[i][4]) <= 1)) {
                    html = html + "<li>El valor del indicador Ki de la causa " + data[i][2] + " debe estar entre 0 y 1.</li>";
                    flag = false;
                }
            }
        }
    }

    html = html + "</ul>";

    if (flag) {
        $.ajax({
            type: 'POST',
            url: controlador + 'GrabarIndicadores',
            contentType: 'application/json',
            data: JSON.stringify({
                data: data,
                periodo: $('#cbPeriodo').val()
            }),
            dataType: 'json',
            success: function (result) {
                if (result == 1) 
                    mostrarMensaje('mensaje', 'exito', 'Los indicadores fueron grabados correctamente.');                
                else 
                    mostrarMensaje('mensaje', 'error', 'Se ha producido un error.');                
            },
            error: function () {
                mostrarMensaje('mensaje', 'error', 'Se ha producido un error.');
            }
        });
    }
    else {
        mostrarMensaje('mensaje', 'alert', html);
    }
}

function cargarTolerancias() {
    $.ajax({
        type: 'POST',
        url: controlador + 'ObtenerTolerancias',
        data: {
            periodo: $('#cbPeriodo').val()
        },
        dataType: 'json',
        success: function (result) {
            cargarGrillaTolerancia(result.Data);
        },
        error: function () {
            mostrarMensaje('mensaje', 'error', 'Se ha producido un error.');
        }
    });
}

function cargarGrillaTolerancia(result) {
    if (hotTolerancia != null) {
        hotTolerancia.destroy();
    }
    var container = document.getElementById('excelTolerancia');


    var tituloRenderer = function (instance, td, row, col, prop, value, cellProperties) {
        Handsontable.renderers.TextRenderer.apply(this, arguments);
        td.style.fontWeight = 'bold';
        td.style.fontSize = '11px';
        td.style.textAlign = 'center';
        td.style.color = '#fff';
        td.style.backgroundColor = '#4C97C3';
    };

    var disabledRenderer = function (instance, td, row, col, prop, value, cellProperties) {
        Handsontable.renderers.TextRenderer.apply(this, arguments);
        td.style.backgroundColor = '#F2F2F2';
        td.style.fontSize = '11px';
        td.style.verticalAlign = 'middle';
    };

    var merge = [
        {
            row: 0, col: 1, rowspan: 2, colspan: 1
        },
        {
            row: 0, col: 2, rowspan: 1, colspan: 2
        },
        {
            row: 0, col: 4, rowspan: 1, colspan: 2
        }
    ];
    var data = [
        ["", "Nivel de Tensión", 'Tolerancias', "", 'Incremento de tolerancias “Si”', ""],
        ["", "", "Número de Interrupciones \n por Cliente(N’)", "Duración Total Ponderada de \n Interrupciones por Cliente(D’)", "Número de Interrupciones \n por Cliente(N’)", "Duración Total Ponderada de \n Interrupciones por Cliente(D’)"]       
    ];

    for (var k in result) {
        data.push(result[k])
    }

    hotTolerancia = new Handsontable(container, {
        data: data,
        maxRows: data.length,
        colWidths: [1, 120, 170, 170, 170, 170],
        cells: function (row, col, prop) {
            var cellProperties = {};

            if (row == 0 || row == 1) {
                cellProperties.renderer = tituloRenderer;
                cellProperties.readOnly = true;
            }

            if (row > 1 && (col == 1 || col == 0)) {

                cellProperties.renderer = disabledRenderer;
                cellProperties.readOnly = true;
            }
            
            return cellProperties;
        },
        mergeCells: merge
    });
}

function grabarTolerancia() {

    var data = hotTolerancia.getData();
    var html = "<ul>";
    var flag = true;

    for (i = 2; i < data.length; i++) {
        if (data[i][2] == "" || data[i][3] == "" || data[i][4] == "" || data[i][5] == "") {
            html = html + "<li>Ingrese todos los valores para el nivel te tensión " + data[i][1] + "</li>";
            flag = false;
        }
        else {
            if (!validarEntero(data[i][2]) || !validarEntero(data[i][3]) || !validarEntero(data[i][4]) || !validarEntero(data[i][5])) {
                html = html + "<li>Debe ingreses valores enteros para el nivel te tensión " + data[i][1] + "</li>";
                flag = false;
            }
            else {
                if (parseInt(data[i][2]) > parseInt(data[i][4]) || parseInt(data[i][3]) > parseInt(data[i][5])) {
                    html = html + "<li>Los valores del campo Incremento de Tolerancia “SI”, deben ser mayores a los valores del campo “Tolerancias” para el nivel de tensión " + data[i][1] + "</li>";
                    flag = false;
                }
            }
        }       
    }

    html = html + "</ul>";

    if (flag) {
        $.ajax({
            type: 'POST',
            url: controlador + 'GrabarTolerancias',
            contentType: 'application/json',
            data: JSON.stringify({
                data: data,
                periodo: $('#cbPeriodo').val()
            }),
            dataType: 'json',
            success: function (result) {
                if (result == 1)
                    mostrarMensaje('mensaje', 'exito', 'Los datos fueron grabados correctamente.');
                else
                    mostrarMensaje('mensaje', 'error', 'Se ha producido un error.');
            },
            error: function () {
                mostrarMensaje('mensaje', 'error', 'Se ha producido un error.');
            }
        });
    }
    else {
        mostrarMensaje('mensaje', 'alert', html);
    }
}

function cargarIngresos() {
    $.ajax({
        type: 'POST',
        url: controlador + 'ListadoIngreso',
        data: {
            periodo: $('#cbPeriodo').val()
        },
        success: function (evt) {
            $('#listadoIngreso').html(evt);
            $('#tablaIngreso').dataTable({
                "iDisplayLength": 25
            });
        },
        error: function () {
            mostrarMensaje('mensaje', 'error', 'Ha ocurrido un error.');
        }
    });
}

function editarIngreso(id) {
    $.ajax({
        type: 'POST',
        url: controlador + 'EditarIngreso',
        data: {
            id: id
        },
        global: false,
        success: function (evt) {
            $('#contenidoIngreso').html(evt);
            setTimeout(function () {
                $('#popupIngreso').bPopup({
                    autoClose: false,
                    modalClose: false,
                    escClose: false,
                    follow: [false, false]
                });
            }, 50);

            $('#btnGrabarIngreso').on("click", function () {
                grabarIngreso();
            });

            $('#cbEmpresa').multipleSelect({
                width: '200px',
                filter: true,
                selectAll: false,
                single: true
            });
           
            var empresa = [];
            empresa.push($('#hfEmpresa').val())
            $('#cbEmpresa').multipleSelect('setSelects', empresa );

            $('#cbMoneda').val($('#hfMoneda').val());
            cargarMoneda($('#hfMoneda').val());

            $('#cbMoneda').on('change', function () {
                cargarMoneda($('#cbMoneda').val());
            });

            $('#btnCancelarIngreso').on("click", function () {
                $('#popupIngreso').bPopup().close();
            });
        },
        error: function () {
            mostrarMensaje('mensaje', 'error', 'Se ha producido un error.');
        }
    });
}

function cargarMoneda(moneda) {
    if (moneda == "S") {
        $('#spanMoneda').text("S/.");
    }
    else {
        $('#spanMoneda').text("$");
    }
}

function grabarIngreso() {
    var empresa = $('#cbEmpresa').multipleSelect('getSelects');
    var validacion = validarIngreso();
       
    if (validacion == "") {
        $.ajax({
            type: 'POST',
            url: controlador + 'GrabarIngreso',
            data: {
                codigo: $('#hfCodigoIngreso').val(),
                empresa: empresa[0],
                moneda: $('#cbMoneda').val(),
                ingreso: $('#txtIngreso').val(),
                periodo: $('#cbPeriodo').val()
            },
            dataType: 'json',
            success: function (result) {
                if (result == 1) {
                    $('#popupIngreso').bPopup().close();
                    mostrarMensaje('mensaje', 'exito', 'El registro se agregó correctamente.');
                    cargarIngresos();
                }                
                else {
                    mostrarMensaje('mensajeEdicion', 'error', 'Se ha producido un error.');
                }
            },
            error: function () {
                mostrarMensaje('mensajeEdicion', 'error', 'Se ha producido un error.');
            }
        });
    }
    else {
        mostrarMensaje('mensajeEdicion', 'alert', validacion);
    }
}

function validarIngreso() {
    var empresa = $('#cbEmpresa').multipleSelect('getSelects');

    var html = "<ul>";
    var flag = true;

    if (empresa.length == 0) {
        html = html + "<li>Seleccione empresa.</li>";
        flag = false;
    }
    if ($('#txtIngreso').val() == "") {
        html = html + "<li>Ingrese el ingreso por transmisión semestral.</li>";
        flag = false;
    }
    else {
        if (!validarNumero($('#txtIngreso').val())) {
            html = html + "<li>El ingreso por transmisión debe ser un valor numérico.</li>";
            flag = false;
        }
    }
    html = html + "</ul>";

    if (flag) html = "";
    return html;
}

function eliminarIngreso(id) {
    if (confirm('¿Está seguro de realizar esta operación?')) {

        $.ajax({
            type: 'POST',
            url: controlador + 'EliminarIngreso',
            data: {
                codigo: id
            },
            dataType: 'json',
            success: function (result) {
                if (result == 1) {
                    mostrarMensaje('mensaje', 'exito', 'El registro se eliminó correctamente.');
                    cargarIngresos();
                }
                else {
                    mostrarMensaje('mensaje', 'error', 'Se ha producido un error.');
                }
            },
            error: function () {
                mostrarMensaje('mensaje', 'error', 'Se ha producido un error.');
            }
        });
    }
}

function exportarIngresos() {
    $.ajax({
        type: 'POST',
        url: controlador + "ExportarIngresos",
        data: {
            periodo: $('#cbPeriodo').val()
        },
        dataType: 'json',
        cache: false,
        success: function (resultado) {
            if (resultado == 1) {
                location.href = controlador + "DescargarIngresos";
            }
            else {
                mostrarMensaje('mensaje', 'error', 'Ha ocurrido un error.');
            }
        },
        error: function () {
            mostrarMensaje('mensaje', 'error', 'Ha ocurrido un error.');
        }
    });
}

function descargarFileIngreso(id, extension) {
    location.href = controlador + 'DescargarArchivoIngreso?id=' + id + "&extension=" + extension;
}

function cargarEventos() {
    $.ajax({
        type: 'POST',
        url: controlador + 'ListadoEvento',
        data: {
            periodo: $('#cbPeriodo').val()
        },
        success: function (evt) {
            $('#listadoEvento').html(evt);
            $('#tablaEvento').dataTable({
                "iDisplayLength": 25
            });
        },
        error: function () {
            mostrarMensaje('mensaje', 'error', 'Ha ocurrido un error.');
        }
    });
}

function editarEvento(id) {
    $.ajax({
        type: 'POST',
        url: controlador + 'EditarEvento',
        data: {
            id: id,
            idPeriodo: $('#cbPeriodo').val()
        },
        global: false,
        success: function (evt) {
            $('#contenidoEvento').html(evt);
            setTimeout(function () {
                $('#popupEvento').bPopup({
                    autoClose: false,
                    modalClose: false,
                    escClose: false,
                    follow: [false, false]
                });
            }, 50);

            $('#txtFechaEvento').Zebra_DatePicker({
            });

            $('#btnGrabarEvento').on("click", function () {
                grabarEvento();
            });

            /*$('#cbEmpresa1').multipleSelect({
                width: '200px',
                filter: true,
                selectAll: false,
                single: true
            });

            var empresa1 = [];
            empresa1.push($('#hfEmpresa1').val())
            $('#cbEmpresa1').multipleSelect('setSelects', empresa1);

            $('#cbEmpresa2').multipleSelect({
                width: '200px',
                filter: true,
                selectAll: false,
                single: true
            });

            var empresa2 = [];
            empresa2.push($('#hfEmpresa2').val())
            $('#cbEmpresa2').multipleSelect('setSelects', empresa2);

            $('#cbEmpresa3').multipleSelect({
                width: '200px',
                filter: true,
                selectAll: false,
                single: true
            });

            var empresa3 = [];
            empresa3.push($('#hfEmpresa3').val())
            $('#cbEmpresa3').multipleSelect('setSelects', empresa3);

            $('#cbEmpresa4').multipleSelect({
                width: '200px',
                filter: true,
                selectAll: false,
                single: true
            });

            var empresa4 = [];
            empresa4.push($('#hfEmpresa4').val())
            $('#cbEmpresa4').multipleSelect('setSelects', empresa4);

            $('#cbEmpresa5').multipleSelect({
                width: '200px',
                filter: true,
                selectAll: false,
                single: true
            });

            var empresa5 = [];
            empresa5.push($('#hfEmpresa5').val())
            $('#cbEmpresa5').multipleSelect('setSelects', empresa5);*/

            $('#cbEmpresa1').val($('#hfEmpresa1').val());
            $('#cbEmpresa2').val($('#hfEmpresa2').val());
            $('#cbEmpresa3').val($('#hfEmpresa3').val());
            $('#cbEmpresa4').val($('#hfEmpresa4').val());
            $('#cbEmpresa5').val($('#hfEmpresa5').val());
                       
            $('#btnCancelarEvento').on("click", function () {
                $('#popupEvento').bPopup().close();
            });
        },
        error: function () {
            mostrarMensaje('mensaje', 'error', 'Se ha producido un error.');
        }
    });
}

function grabarEvento() {
    var validacion = validarEvento();

    if (validacion == "") {
        $.ajax({
            type: 'POST',
            url: controlador + 'GrabarEvento',
            data: $('#frmRegistroEvento').serialize(),
            dataType: 'json',
            global: false,
            success: function (result) {
                if (result == 1) {
                    mostrarMensaje('mensaje', 'exito', 'Los datos se grabaron correctamente.');
                    $('#popupEvento').bPopup().close();
                    cargarEventos();
                }
                else if (result == 2) {
                    mostrarMensaje('mensajeEvento', 'alert', 'La fecha debe estar dentro de las fechas del periodo.');
                }
                else {
                    mostrarMensaje('mensajeEvento', 'error', 'Se ha producido un error.');
                }
            },
            error: function () {
                mostrarMensaje('mensajeEvento', 'error', 'Se ha producido un error.');
            }
        });
    }
    else {
        mostrarMensaje('mensajeEvento', 'alert', validacion);
    }
}

function validarEvento() {
    var html = "<ul>";
    var flag = true;
    var porcentaje = 0;
    html = html + "</ul>";

    if ($('#txtEvento').val() == "") {
        html = html + "<li>Ingrese la descripción del evento.</li>";
        flag = false;
    }

    if ($('#txtFechaEvento').val() == "") {
        html = html + "<li>Ingrese la fecha del evento.</li>";
        flag = false;
    }

    if ($('#cbEmpresa1').val() == "") {
        html = html + "<li>Seleccione responsable 1.</li>";
        flag = false;
    }

    if ($('#txtPorcentaje1').val() == "") {
        html = html + "<li>Ingrese el Porcentaje del Responsable 1</li>";
        flag = false;
    }
    else {
        if (!validarNumero($('#txtPorcentaje1').val())) {
            html = html + "<li>El Porcentaje de Responsable 1 debe ser numérico.</li>";
            flag = false;
        }
        else {
            porcentaje = porcentaje + parseFloat($('#txtPorcentaje1').val());
            
        }
    }

    if (($('#cbEmpresa2').val() != "" && $('#txtPorcentaje2').val() == "") || ($('#cbEmpresa2').val() == "" && $('#txtPorcentaje2').val() != "")) {
        html = html + "<li>Para el Responsable 2, debe seleccionar empresa e ingresar porcentaje.</li>";
        flag = false;
    }
    else {
        if ($('#cbEmpresa2').val() != "" && $('#txtPorcentaje2').val() != "") {
            if (!validarNumero($('#txtPorcentaje2').val())) {
                html = html + "<li>El Porcentaje de Responsable 2 debe ser numérico.</li>";
                flag = false;
            }
            else {
                porcentaje = porcentaje + parseFloat($('#txtPorcentaje2').val());        
            }
        }
    }
    if (($('#cbEmpresa3').val() != "" && $('#txtPorcentaje3').val() == "") || ($('#cbEmpresa3').val() == "" && $('#txtPorcentaje3').val() != "")) {
        html = html + "<li>Para el Responsable 3, debe seleccionar empresa e ingresar porcentaje.</li>";
        flag = false;
    }
    else {
        if ($('#cbEmpresa3').val() != "" && $('#txtPorcentaje3').val() != "") {
            if (!validarNumero($('#txtPorcentaje3').val())) {
                html = html + "<li>El Porcentaje de Responsable 3 debe ser numérico.</li>";
                flag = false;
            }
            else {                
                porcentaje = porcentaje + parseFloat($('#txtPorcentaje3').val());                
            }
        }
    }

    if (($('#cbEmpresa4').val() != "" && $('#txtPorcentaje4').val() == "") || ($('#cbEmpresa4').val() == "" && $('#txtPorcentaje4').val() != "")) {
        html = html + "<li>Para el Responsable 4, debe seleccionar empresa e ingresar porcentaje.</li>";
        flag = false;
    }
    else {
        if ($('#cbEmpresa4').val() != "" && $('#txtPorcentaje4').val() != "") {
            if (!validarNumero($('#txtPorcentaje4').val())) {
                html = html + "<li>El Porcentaje de Responsable 4 debe ser numérico.</li>";
                flag = false;
            }
            else {
                porcentaje = porcentaje + parseFloat($('#txtPorcentaje4').val());
               
            }
        }
    }
    if (($('#cbEmpresa5').val() != "" && $('#txtPorcentaje5').val() == "") || ($('#cbEmpresa5').val() == "" && $('#txtPorcentaje5').val() != "")) {
        html = html + "<li>Para el Responsable 5, debe seleccionar empresa e ingresar porcentaje.</li>";
        flag = false;
    }
    else {
        if ($('#cbEmpresa5').val() != "" && $('#txtPorcentaje5').val() != "") {
            if (!validarNumero($('#txtPorcentaje5').val())) {
                html = html + "<li>El Porcentaje de Responsable 5 debe ser numérico.</li>";
                flag = false;
            }
            else {
                porcentaje = porcentaje + parseFloat($('#txtPorcentaje5').val());
            }
        }
    }

    /*var empresa1 = $('#cbEmpresa1').multipleSelect('getSelects');
    var empresa2 = $('#cbEmpresa2').multipleSelect('getSelects');
    var empresa3 = $('#cbEmpresa3').multipleSelect('getSelects');
    var empresa4 = $('#cbEmpresa4').multipleSelect('getSelects');
    var empresa5 = $('#cbEmpresa5').multipleSelect('getSelects');
    var porcentaje = 0;

    if (empresa1.length == 0) {
        html = html + "<li>Seleccione responsable 1.</li>";
        flag = false;
    }

    if ($('#txtPorcentaje1').val() == "") {
        html = html + "<li>Ingrese el Porcentaje del Responsable 1</li>";
        flag = false;
    }
    else {
        if (!validarNumero($('#txtPorcentaje1').val())) {
            html = html + "<li>El Porcentaje de Responsable 1 debe ser numérico.</li>";
            flag = false;
        }
        else {
            porcentaje = porcentaje + parseFloat($('#txtPorcentaje1').val());
        }
    }
    
    if ((empresa2.length > 0 && $('#txtPorcentaje2').val() == "") || (empresa2.length == 0 && $('#txtPorcentaje2').val() != "")) {
        html = html + "<li>Para el Responsable 2, debe seleccionar empresa e ingresar porcentaje.</li>";
        flag = false;
    }
    else {
        if (empresa2.length > 0 && $('#txtPorcentaje2').val() != "") {
            if (!validarNumero($('#txtPorcentaje2').val())) {
                html = html + "<li>El Porcentaje de Responsable 2 debe ser numérico.</li>";
                flag = false;
            }
            else {
                porcentaje = porcentaje + parseFloat($('#txtPorcentaje2').val());
            }
        }
    }
    if ((empresa3.length > 0 && $('#txtPorcentaje3').val() == "") || (empresa3.length == 0 && $('#txtPorcentaje3').val() != "")) {
        html = html + "<li>Para el Responsable 3, debe seleccionar empresa e ingresar porcentaje.</li>";
        flag = false;
    }
    else {
        if (empresa3.length > 0 && $('#txtPorcentaje3').val() != "") {
            if (!validarNumero($('#txtPorcentaje3').val())) {
                html = html + "<li>El Porcentaje de Responsable 3 debe ser numérico.</li>";
                flag = false;
            }
            else {
                porcentaje = porcentaje + parseFloat($('#txtPorcentaje3').val());
            }
        }
    }

    if ((empresa4.length > 0 && $('#txtPorcentaje4').val() == "") || (empresa4.length == 0 && $('#txtPorcentaje4').val() != "")) {
        html = html + "<li>Para el Responsable 4, debe seleccionar empresa e ingresar porcentaje.</li>";
        flag = false;
    }
    else {
        if (empresa4.length > 0 && $('#txtPorcentaje4').val() != "") {
            if (!validarNumero($('#txtPorcentaje4').val())) {
                html = html + "<li>El Porcentaje de Responsable 4 debe ser numérico.</li>";
                flag = false;
            }
            else {
                porcentaje = porcentaje + parseFloat($('#txtPorcentaje4').val());
            }
        }
    }
    if ((empresa5.length > 0 && $('#txtPorcentaje5').val() == "") || (empresa5.length == 0 && $('#txtPorcentaje5').val() != "")) {
        html = html + "<li>Para el Responsable 5, debe seleccionar empresa e ingresar porcentaje.</li>";
        flag = false;
    }
    else {
        if (empresa5.length > 0 && $('#txtPorcentaje5').val() != "") {
            if (!validarNumero($('#txtPorcentaje5').val())) {
                html = html + "<li>El Porcentaje de Responsable 5 debe ser numérico.</li>";
                flag = false;
            }
            else {
                porcentaje = porcentaje + parseFloat($('#txtPorcentaje5').val());
            }
        }
    }*/

    if ($('#txtComentario').val() == "") {
        html = html + "<li>Debe ingresar el comentario del Evento.</li>";
        flag = false;
    }

    if (redondear(porcentaje, 2) != 100 && flag) {
        html = html + "<li>Los porcentajes deben sumar 100%.</li>";
        flag = false;
    }
    
    if (flag) {
        html = "";   
    }
    return html;
}

function redondear(num, decimales) {
    return Math.round(num * Math.pow(10, decimales)) / Math.pow(10, decimales);
}

function eliminarEvento(id) {
    if (confirm('¿Está seguro de realizar esta operación?')) {

        $.ajax({
            type: 'POST',
            url: controlador + 'EliminarEvento',
            data: {
                id: id
            },
            dataType: 'json',
            success: function (result) {
                if (result == 1) {
                    mostrarMensaje('mensaje', 'exito', 'El registro se eliminó correctamente.');
                    cargarEventos();
                }
                else {
                    mostrarMensaje('mensaje', 'error', 'Se ha producido un error.');
                }
            },
            error: function () {
                mostrarMensaje('mensaje', 'error', 'Se ha producido un error.');
            }
        });
    }
}

function exportarEventos() {
    $.ajax({
        type: 'POST',
        url: controlador + "ExportarEventos",
        data: {
            periodo: $('#cbPeriodo').val()
        },
        dataType: 'json',
        cache: false,
        success: function (resultado) {
            if (resultado == 1) {
                location.href = controlador + "DescargarEventos";
            }
            else {
                mostrarMensaje('mensaje', 'error', 'Ha ocurrido un error.');
            }
        },
        error: function () {
            mostrarMensaje('mensaje', 'error', 'Ha ocurrido un error.');
        }
    });
}

function cargarUploaderEvento() {
    var uploaderEvento = new plupload.Uploader({
        runtimes: 'html5,flash,silverlight,html4',
        browse_button: 'btnSelectFileEvento',
        container: document.getElementById('containerEvento'),
        url: controlador + 'uploadevento',
        flash_swf_url: '~/Content/Scripts/Moxie.swf',
        silverlight_xap_url: '~/Content/Scripts/Moxie.xap',
        multi_selection: false,
        filters: {
            max_file_size: '2mb',
            mime_types: [
                { title: "Archivos Excel .xlsx", extensions: "xlsx" }
            ]
        },
        init: {
            PostInit: function () {
                document.getElementById('btnProcesarFileEvento').onclick = function () {
                    if (uploaderEvento.files.length > 0) {
                        uploaderEvento.start();
                    }
                    else
                        loadValidacionFile('fileInfoEvento', 'Seleccione archivo');
                    return false;
                };
            },
            FilesAdded: function (up, files) {
                if (uploaderEvento.files.length == 2) {
                    uploaderEvento.removeFile(uploaderEvento.files[0]);
                }
                plupload.each(files, function (file) {
                    loadInfoFile('fileInfoEvento', file.name, plupload.formatSize(file.size));
                });
                uploaderEvento.refresh();
            },
            UploadProgress: function (up, file) {
                mostrarProgreso('progresoEvento', file.percent);
            },
            UploadComplete: function (up, file) {
                importarEvento();
            },
            Error: function (up, err) {
                loadValidacionFile('fileInfoProgreso', err.code + "-" + err.message);
            }
        }
    });

    uploaderEvento.init();
}

function importarEvento() {
    $.ajax({
        type: 'POST',
        url: controlador + 'ImportarEvento',
        data: {
            periodo: $('#cbPeriodo').val()
        },
        dataType: 'json',
        success: function (result) {
            if (result.Result == 1) {
                mostrarMensaje('mensaje', 'exito', 'La carga de eventos se realizó correctamente.');
                cargarEventos();
                habilitarCargaEvento();
            }
            else if (result.Result == 2) {
                var html = "No se realizó la carga por que se encontraron los siguientes errores: <ul>";

                for (var i in result.Errores) {
                    html = html + "<li>" + result.Errores[i] + "</li>";
                }
                html = html + "</ul>";
                mostrarMensaje('mensaje', 'alert', html);
                habilitarCargaEvento();
            }
            else if (result.Result == -1) {
                mostrarMensaje('mensaje', 'error', 'Se ha producido un error.');
            }
        },
        error: function () {
            mostrarMensaje('mensaje', 'error', 'Se ha producido un error.');
        }
    });
}

function habilitarCargaEvento() {
    $('#divImportarEvento').toggle();
    $('#progresoEvento').removeClass();
    $('#fileInfoEvento').removeClass();
    $('#progresoEvento').html('');
    $('#fileInfoEvento').html('');
}



function cargarInterrupciones() {
    mostrarMensajeDefecto();
    if ($('#cbPeriodo').val() != "") {
        $.ajax({
            type: 'POST',
            url: controlador + 'consultarinterrupciones',
            data: {                
                periodo: $('#cbPeriodo').val()
            },
            dataType: 'json',
            success: function (result) {
                if (result.Result == 1) {
                    cargarGrillaInterrupcion(result);                 
                }
                else {
                    mostrarMensaje('mensaje', 'error', 'Se ha producido un error.');
                }
            },
            error: function () {
                mostrarMensaje('mensaje', 'error', 'Se ha producido un error.');
            }
        });
    }
    else {
        mostrarMensaje('mensaje', 'alert', 'Debe seleccionar periodo.');
    }
}


function descargarFormatoInterrupcion() {
    $.ajax({
        type: 'POST',
        url: controlador + "GenerarFormatoInterrupcion",
        data: {            
            periodo: $('#cbPeriodo').val()
        },
        dataType: 'json',
        cache: false,
        success: function (resultado) {
            if (resultado == 1) {
                location.href = controlador + "DescargarFormatoInterrupcion";
            }
            else {
                mostrarMensaje('mensaje', 'error', 'Ha ocurrido un error.');
            }
        },
        error: function () {
            mostrarMensaje('mensaje', 'error', 'Ha ocurrido un error.');
        }
    });
}


function subirFormatoInterrupcion() {

}

function enviarDatosInterrupcion() {   
    var validacion = validarDatosInterrupcion();
    if (validacion.length > 0) {
        pintarError(validacion);
    }
    else {
        grabarInterrupciones();
    }
       
}

function grabarInterrupciones() {

    $.ajax({
        type: 'POST',
        url: controlador + 'GrabarInterrupciones',
        contentType: 'application/json',
        data: JSON.stringify({
            data: getDataInterrupciones(),
            periodo: $('#cbPeriodo').val()           
        }),
        dataType: 'json',
        success: function (result) {
            
            if (result.Result == 1) {
                consultar();
                mostrarMensaje('mensaje', 'exito', 'Los datos fueron grabados correctamente.');
            }           
            else
                mostrarMensaje('mensaje', 'error', 'Se ha producido un error.');
        },
        error: function () {
            mostrarMensaje('mensaje', 'error', 'Se ha producido un error.');
        }
    });
}

function mostrarErroresInterrupcion() {
    mostrarMensajeDefecto();
    var validacion = validarDatosInterrupcion();

    if (validacion.length > 0) {
        mostrarMensaje('mensaje', 'alert', 'Se encontraron errores en los datos ingresados');
        pintarError(validacion);
    }
    else {
        mostrarMensaje('mensaje', 'exito', 'No se encontraron errores en los datos ingresados');
    }
}

function pintarError(validaciones) {
    $('#contenidoError').html(obtenerErroresInterrupciones(validaciones));
    $('#popupErrores').bPopup({});
}

function cargarUploaderInterrupcion() {
    uploaderInterrupcion = new plupload.Uploader({
        runtimes: 'html5,flash,silverlight,html4',
        browse_button: 'btnSubirFormatoInterrupcion',
        url: controlador + 'UploadInterrupcion',
        flash_swf_url: 'Scripts/Moxie.swf',
        silverlight_xap_url: 'Scripts/Moxie.xap',
        multi_selection: false,
        filters: {
            max_file_size: '5mb',
            mime_types: [
                { title: "Archivos Macros Excel .xlsm", extensions: "xlsm" }
            ]
        },
        init: {
            FilesAdded: function (up, files) {
                if (uploaderInterrupcion.files.length == 2) {
                    uploaderInterrupcion.removeFile(uploaderInterrupcion.files[0]);
                }
                uploaderInterrupcion.start();
                up.refresh();
            },
            UploadProgress: function (up, file) {
                mostrarMensaje('mensaje', 'alert', "Por favor espere ...(<strong>" + file.percent + "%</strong>)");
            },
            UploadComplete: function (up, file) {
                mostrarMensaje('mensaje', 'alert', "Subida completada. <strong>Por favor espere...</strong>");
                procesarArchivoInterrupcion();
            },
            Error: function (up, err) {
                mostrarMensaje('mensaje', 'error', err.code + "-" + err.message);
            }
        }
    });
    uploaderInterrupcion.init();
}


function procesarArchivoInterrupcion() {
    $.ajax({
        type: 'POST',
        url: controlador + 'ImportarInterrupciones',
        dataType: 'json',
        data: {           
            periodo: $('#cbPeriodo').val()
        },
        success: function (result) {
            if (result.Result == 1) {
                mostrarMensaje('mensaje', 'exito', 'Los datos se cargaron correctamente en el Excel web, presione el botón <strong>Enviar</strong> para grabar.');
                actualizarDataGrilla(result.Data);
            }
            else if (result.Result == 2) {
                var html = "No se realizó la carga por que se encontraron los siguientes errores: <ul>";
                for (var i in result.Errores) {
                    html = html + "<li>" + result.Errores[i] + "</li>";
                }
                html = html + "</ul>";
                mostrarMensaje('mensaje', 'alert', html);
            }
            else if (result.Result == -1) {
                mostrarMensaje('mensaje', 'error', 'Se ha producido un error.');
            }
        },
        error: function () {
            mostrarMensaje('mensaje', 'error', 'Se ha producido un error.');
        }
    });
}