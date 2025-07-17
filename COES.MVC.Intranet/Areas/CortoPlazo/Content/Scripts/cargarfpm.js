var controlador = siteRoot + 'cortoplazo/reportefinal/';
var hot = null;
$(function () {

    $('#txtFecha').Zebra_DatePicker({        
    });

    $('#btnConsultar').on('click', function () {
        consultar('CargarFactoresPerdida', 0);
    });

    $('#btnDescargarFormato').on('click', function () {
        descargarFormato();
    });

    $('#btnEnviarDatos').on('click', function () {
        enviarDatos();
    });

    $('#btnMostrarErrores').on('click', function () {
        mostrarErrores();
    });

    $('#btnVerEnvios').on('click', function () {
        verHistorico();
    });

    $('#divAcciones').hide();

    crearPupload();

});

consultar = function (action, tipo) {
    $.ajax({
        type: 'POST',
        url: controlador + action,
        data: {
            fecha: $('#txtFecha').val()
        },
        dataType: 'json',
        success: function (result) {
            if (result.Resultado == 1) {                
                cargarGrilla(result.Datos);
                $('#mensaje').removeClass();
                $('#mensaje').html('');
                $('#notaDatos').text(result.FechaDatos);

                if (tipo == 1) {
                    mostrarMensaje('mensaje', 'exito', 'Los datos se enviaron correctamente.');
                }

                if (action == 'CargarFactoresPerdida') {
                    cargarHistorico(result.ListaHistorico);
                }

                $('#divAcciones').show();
                $('#contenedor').show();
                $('#notaDatos').show();
            }
            else if (result.Resultado == 2) {
                mostrarMensaje('mensaje', 'alert', result.Mensaje);
                $('#divAcciones').hide();
                $('#contenedor').hide();
                $('#notaDatos').hide();
            }
            else {
                mostrarMensaje('mensaje', 'error', 'Ha ocurrido un error.');
            }
        },
        error: function () {
            mostrarMensaje('mensaje', 'error', 'Ha ocurrido un error.');
        }
    });
};

cargarGrilla = function (result) {
    if (hot != null) {
        hot.destroy();
    }
    var container = document.getElementById('contenedor');
    var data = result;

    var tituloRenderer = function (instance, td, row, col, prop, value, cellProperties) {
        Handsontable.renderers.TextRenderer.apply(this, arguments);
        td.style.fontWeight = 'bold';
        td.style.fontSize = '11px';
        td.style.textAlign = 'center';
        td.style.color = '#fff';
        td.style.backgroundColor = '#4C97C3';
    };

    var disbledRenderer = function (instance, td, row, col, prop, value, cellProperties) {
        Handsontable.renderers.TextRenderer.apply(this, arguments);
        td.style.backgroundColor = '#F2F2F2';
        td.style.fontSize = '11px';
        td.style.verticalAlign = 'middle';
    };

    var calculoRenderer = function (instance, td, row, col, prop, value, cellProperties) {
        Handsontable.renderers.TextRenderer.apply(this, arguments);
        
    };

    var merge = [
        {
            row: 0, col: 0, rowspan: 2, colspan: 1
        },
        {
            row: 0, col: 1, rowspan: 2, colspan: 1
        },
        {
            row: 0, col: 2, rowspan: 1, colspan: 3
        }
    ];

    hot = new Handsontable(container, {
        data: data,
        fixedRowsTop: 0,
        width: '100%',
        colWidths: [1, 200, 150, 150, 150],        
        maxRows: result.length,
        cells: function (row, col, prop) {
            var cellProperties = {};

            if (row == 0 || row == 1) {
                cellProperties.renderer = tituloRenderer;
                cellProperties.readOnly = true;
            }

            if (row> 1 && col < 2) {
                cellProperties.renderer = disbledRenderer;
                cellProperties.readOnly = true;
            }

            if (row>1 && col >= 2) {
                cellProperties.renderer = calculoRenderer;
                cellProperties.format = '0,0.00000';
                cellProperties.type = 'numeric';
            }

            return cellProperties;
        },
        mergeCells: merge
    });

};

descargarFormato = function () {
    var datos = hot.getData(0, 0, hot.countRows() - 1, hot.countCols() - 1);
    console.log(datos);

    $.ajax({
        type: 'POST',
        url: controlador + 'GenerarFormatoFPM',
        contentType: 'application/json',
        data: JSON.stringify({
            
            data: datos,
            fecha: $('#txtFecha').val()
        }),
        dataType: 'json',
        success: function (result) {
            if (result.Resultado == 1) {
                document.location.href = controlador + 'DescargarFormatoFPM?fecha=' + result.Fecha;
            }
            else if (result.Resultado == -1) {
                mostrarMensaje('mensaje', 'error', 'Ha ocurrido un error.');
            }
        },
        error: function () {
            mostrarMensaje('mensaje', 'error', 'Ha ocurrido un error.');
        }
    });
}

enviarDatos = function () {
    var datos = hot.getData(0, 0, hot.countRows() - 1, hot.countCols() - 1);

    if (datos.length > 2) {        

        var validacion = validarDatos();

        if (validacion.length == 0) {

            $.ajax({
                type: "POST",
                url: controlador + 'GrabarFactoresPerdida',
                dataType: "json",
                contentType: 'application/json',
                traditional: true,
                data: JSON.stringify({
                    fecha: $('#txtFecha').val(),
                    data: datos
                }),
                success: function (result) {
                    if (result.Resultado == 1) {
                        mostrarMensaje('mensaje', 'exito', 'Los datos se enviaron correctamente.');
                        consultar('CargarFactoresPerdida', 1);
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
        else {
            mostrarErrores();
        }            
    }
    else {
        mostrarMensaje('mensaje', 'alert', 'No hay filas de datos.');
    }
};

validarDatos = function () {
    var arr = [];
    var datos = hot.getData(0, 0, hot.countRows() - 1, hot.countCols() - 1);
  
    for (var i = 2; i < datos.length; i++) {

        for (var j = 2; j <= 4; j++) {
            var dato = datos[i][j];

            if (dato == "") {
                arr.push("La celda " + datos[i][1] + " - " + datos[1][j] + " debe tener valor.");
            }
            else {
                if (!validarNumero(dato)) {
                    arr.push("La celda " + datos[i][1] + " - " + datos[1][j] + " debe ser un número.");
                }
            }
        }       
    }

    return arr;
};

function crearPupload() {
    uploader = new plupload.Uploader({
        runtimes: 'html5,flash,silverlight,html4',
        browse_button: 'btnSelectExcel',
        url: siteRoot + 'cortoplazo/reportefinal/UploadFOM',
        flash_swf_url: 'Scripts/Moxie.swf',
        silverlight_xap_url: 'Scripts/Moxie.xap',
        multi_selection: false,
        filters: {
            max_file_size: '5mb',
            mime_types: [
                { title: "Archivos Excel .xlsx", extensions: "xlsx" }
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
                mostrarMensaje("mensaje","alert", "Por favor espere ...(<strong>" + file.percent + "%</strong>)");
            },
            UploadComplete: function (up, file) {
                mostrarMensaje("mensaje", "alert", "Subida completada <strong>Por favor espere</strong>");
                consultar('CargarFPMFromFile', 0);
            },
            Error: function (up, err) {
                mostrarMensaje('mensaje', 'error', 'Ha ocurrido un error.');
            }
        }
    });
    uploader.init();
}

mostrarErrores = function () {
    var validacion = validarDatos();

    var html = '<table class="pretty tabla-adicional" id="tablaErrores">';
    html = html + ' <thead>';
    html = html + '     <tr>';
    html = html + '         <th>Errores</th>';   
    html = html + '     </tr>';
    html = html + ' </thead>';
    html = html + ' <tbody>';

    for (var i in validacion) {
        html = html + '     <tr>';;
        html = html + '         <td>' + validacion[i] + '</td>';     
        html = html + '     </tr>';
    }

    html = html + ' </tbody>';
    html = html + '</table>';

    $('#contenidoErrores').html(html);

    setTimeout(function () {
        $('#popupErrores').bPopup({
            autoClose: false
        });
    }, 50);
};

cargarHistorico = function (result) {
    var html = '<table class="pretty tabla-adicional" id="tablaHistorico">';
    html = html + ' <thead>';
    html = html + '     <tr>';
    html = html + '         <th>Fecha</th>';
    html = html + '         <th>Usuario</th>';
    html = html + '         <th>Fecha de envío</th>';
    html = html + '     </tr>';
    html = html + ' </thead>';
    html = html + ' <tbody>';

    for (var i in result) {
        html = html + '     <tr>';;
        html = html + '         <td>' + result[i].FechaDatos + '</td>';
        html = html + '         <td>' + result[i].Cmfpmusumodificacion + '</td>';
        html = html + '         <td>' + result[i].FechaModificacion + '</td>';
        html = html + '     </tr>';
    }

    html = html + ' </tbody>';
    html = html + '</table>';

    $('#contenidoHistorico').html(html);    
};

verHistorico = function () {
    setTimeout(function () {
        $('#popupHistorico').bPopup({
            autoClose: false
        });
    }, 50);
};

mostrarMensaje = function (id, tipo, mensaje) {
    $('#' + id).removeClass();
    $('#' + id).addClass('action-' + tipo);
    $('#' + id).html(mensaje);
}

getFecha = function (date) {
    var parts = date.split("/");
    var date = new Date(parts[1] + "/" + parts[0] + "/" + parts[2]);
    return date.getTime();
};

function validarNumero(texto) {
    return /^-?[\d.]+(?:e-?\d+)?$/.test(texto);
};