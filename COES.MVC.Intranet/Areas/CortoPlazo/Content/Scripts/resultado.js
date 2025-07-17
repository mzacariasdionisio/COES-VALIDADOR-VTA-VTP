var controladorResultado = siteRoot + 'cortoplazo/resultado/';

$(function () {

    $('#btnConsultar').on('click', function () {
        mostrarMensajeDefecto();
        consultar();
    });
    
    $('#txtFecha').Zebra_DatePicker({
    });

    $('#txtFechaDAT').Zebra_DatePicker({

    });

    $('#txtFechaInicioReproceso').Zebra_DatePicker({

        pair: $('#txtFechaFinReproceso'),
        onSelect: function (date) {            
            var date1 = getFecha(date);
            var date2 = getFecha($('#txtFechaFinReproceso').val());

            if (date1 > date2) {
                $('#txtFechaFinReproceso').val(date);
            }           
        }
    });

    $('#txtFechaFinReproceso').Zebra_DatePicker({
        direction: true      
    });

    $('#btnExportar').on('click', function () {
        exportar();
    });
    
    $('#btnReprocesar').on('click', function () {
        openReprocesar();
    });

    $('#btnReprocesarMasivo').on('click', function () {
        openReprocesarMasivo();
    });

    $('#btnAceptarReproceso').on('click', function () {
        reprocesar(0);
    });

    $('#btnOkReprocesarMasivo').on('click', function () {
        reprocesarMasivo();
    });

    $('#btnExportarMasivo').on('click', function () {
        openExportar();
    });

    $('#btnOkExportarMasivo').on('click', function () {
        exportarMasivo();
    });

    $('#cbIndicadorRaw').on('click', function () {

        $('#btnAceptarReproceso').show();
        $('#btnReprocesarConFile').hide();
        $('#trArchivoRaw').hide();
        if ($('#cbIndicadorRaw').is(':checked')) {
            $('#btnAceptarReproceso').hide();
            $('#btnReprocesarConFile').show();
            $('#trArchivoRaw').show();
        }
    });

    $('#cbIndicadorNCP').on('click', function () {
        $('#trRutaNCP').hide();

        if ($('#cbIndicadorNCP').is(':checked')) {          
            $('#trRutaNCP').show();
        }

    });

    $('#tableHorasReproceso').dataTable({
        "scrollY": 300,
        "scrollX": false,
        "sDom": 't',        
        "iDisplayLength": 60,
        "columns": [
            { "width": "30%" },
            { "width": "70%" }
        ]
    });


    $('#txtExportarDesde').Zebra_DatePicker({
        pair: $('#txtExportarHasta'),
        onSelect: function (date) {
            var date1 = getFecha(date);
            var date2 = getFecha($('#txtExportarHasta').val());

            if (date1 > date2) {
                $('#txtExportarHasta').val(date);
            }
        }
    });

    $('#txtExportarHasta').Zebra_DatePicker({
        direction: true
    });
    
    $('#cbSelectAll').click(function (e) {
        var table = $('#tableHorasReproceso');
        $('td input:checkbox', table).prop('checked', this.checked);
    });

    $('#btnExportarDAT').on('click', function () {
        exportarDAT();
    });

    $('#cbFuentePDO').on('change', function () {
        if ($('#cbFuentePDO').val() == "N") {
            $('#trOpcionNCP').show();
            $('#trEscenario').hide();
        }
        else if ($('#cbFuentePDO').val() == "Y") {
            $('#trOpcionNCP').hide();
            $('#trEscenario').show();
            $('#trRutaNCP').hide();
            $('#cbIndicadorNCP').prop('checked', false);
        }

    });

    /**Cambios CMgCP**/

    $('#btnOpenTipoReproceso').on('click', function () {
        prepararReproceso();
    });

    crearPupload();

    $('#btnOkReprocesarCambio').on('click', function () {
        confirmarReprocesoCambio();
    });

    /**Fin Cambios CMgCP**/


    $('#cbModelo').on('change', function (){
        validarFechaModelo();
    });

    $('#cbModeloReproceso').on('change', function () {
        validarFechaModeloReproceso();
    });

    consultar();
});

function consultar() {
        
    if ($('#txtFecha').val() != "") {
        $.ajax({
            type: 'POST',
            url: controladorResultado + 'listado',
            data: {
                fecha: $('#txtFecha').val(),
                tipoProceso: $('#cbTipoProceso').val()
            },
            success: function (evt) {
                $('#listado').html(evt);
                $('#tablaListado').dataTable({
                    "iDisplayLength": 25
                });

                
            },
            error: function () {
                mostrarMensaje('mensaje', 'error', 'Ha ocurrido un error.');
            }
        });
    }
    else
    {
        mostrarMensaje('mensaje', 'alert', 'Seleccione fecha.');
    }
}

function verResultado(correlativo, fecha, tipoProceso, usucreacion) {

    $.ajax({
        type: 'POST',
        url: controladorResultado + 'resultado',
        data: {
            correlativo: correlativo, 
            fecha: fecha,
            usuario: usucreacion,
            tipo: tipoProceso
        },
        success: function (evt) {
            $('#resultado').html(evt);
            $('#tablaResultado').dataTable({
                "iDisplayLength": 25
            });
            $('#tablaGeneracion').dataTable({
                "iDisplayLength": 25
            });
            $('#tablaRestriccion').dataTable({
                "iDisplayLength": 25
            });
            $('#hfCorrelativo').val(correlativo);
            $('#resultado').show();
            $('#folder').hide();
        },
        error: function () {
            mostrarMensaje('mensaje', 'error', 'Ha ocurrido un error.');
        }
    });
}

eliminarCorrida = function (correlativo) {
    
    if (confirm('¿Está seguro de realizar esta operación?')) {
        $.ajax({
            type: 'POST',
            url: controladorResultado + 'eliminarcorrida',
            data: {
                correlativo: correlativo
            },
            dataType: 'json',
            success: function (json) {
                if (json == 1) {
                    consultar();
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
}

coordenadas = function (correlativo) {

    $('#popupMapa').bPopup({
        content: 'iframe',
        contentContainer: '#contenidoMapa',
        modalClose: false,
        loadUrl: controladorResultado + 'mapa?correlativo=' + correlativo,
        onClose: function () {
            consultar();
        }
    });
}

openReprocesar = function () {

    $.ajax({
        type: 'POST',
        url: controladorResultado + 'obtenerescenarios',
        data: {
            fecha: $('#txtFecha').val()
        },
        dataType: 'json',
        global: false,
        success: function (result) {
            if (result != -1) {

                $('#cbTopologia').get(0).options.length = 0;
                $('#cbTopologia').get(0).options[0] = new Option("--SELECCIONE--", "0");
                $.each(result, function (i, item) {
                    $('#cbTopologia').get(0).options[$('#cbTopologia').get(0).options.length] = new Option(item.Topcodi + "-" + item.Topnombre, item.Topcodi);
                });

                $('#popupReprocesar').bPopup({
                    autoClose: false
                });

                $('#cbFuentePDO').val("Y");
                $('#trOpcionNCP').hide();
                $('#trRutaNCP').hide();
                $('#trEscenario').show();

                $('#trArchivoRaw').hide();
                $('#trRutaNCP').hide();
                $('#cbIndicadorRaw').prop('checked', false);
                $('#cbIndicadorNCP').prop('checked', false);
                $('#cbMostrarEnWeb').prop('checked', false);
                $('#progreso').html("");
                $('#fileInfo').html("");

                $('#cbEMS').val("P");

                $('#cbModelo').val("2");
                validarFechaModelo();
            }
            else {
                mostrarMensaje('mensaje', 'error', 'Se ha producido un error.');
            }
        },
        error: function () {
            mostrarMensaje('mensaje', 'error', 'Se ha producido un error.');
        }
    });
};

openReprocesarMasivo = function () {
    $('#cntNuevoReproceso').hide();
    $('#cntReprocesoActual').hide();
    $('#popupReprocesoMasivo').bPopup({
        autoClose: false
    });
}

reprocesar = function (indicador) {
    if ($('#cbHora').val() != "") {
        
        if (confirm('¿Está seguro de realizar esta acción?'))
        {
            var indicadorNCP = 0;
            if ($('#cbIndicadorNCP').is(':checked')) {
                indicadorNCP = 1;
            }

            var indicadorMostrarWeb = 0;
            if ($('#cbMostrarEnWeb').is(':checked')) {
                indicadorMostrarWeb = 1;
            }

            var fuenteRD = $('#cbFuentePDO').val();
            var idEscenario = "0";
            if ($('#cbFuentePDO').val() == "Y") {
                idEscenario = $('#cbTopologia').val();
            }


            $.ajax({
                type: 'POST',
                url: controladorResultado + 'reprocesar',
                data: {
                    fecha: $('#txtFecha').val(),
                    hora: $('#cbHora').val(),
                    indicador: indicador,
                    indicadorNCP: indicadorNCP,                    
                    indicadorMostrarWeb: indicadorMostrarWeb,
                    rutaNCP: $('#txtRutaNCP').val(),
                    version: $('#cbModelo').val(),
                    fuenteRD: fuenteRD,
                    idEscenario: idEscenario,
                    tipoEMS: $('#cbEMS').val()                    
                },
                dataType: 'json',
                global:false,
                success: function (result) {
                    if (result == 1) {
                        mostrarMensaje('mensaje', 'exito', 'El proceso se ejecutó correctamente.');
                        consultar();
                        $('#popupReprocesar').bPopup().close();
                    }
                    else {
                        mostrarMensaje('mensajeProceso', 'error', 'Se produjo un error.');
                    }
                },
                error: function () {
                    mostrarMensaje('mensajeProceso', 'error', 'Se produjo un error.');
                },
                beforeSend: function () {
                    $('#loading').bPopup({
                        fadeSpeed: 'fast',
                        opacity: 0.4,
                        followSpeed: 500,
                        modalColor: '#000000',
                        modalClose: false,
                        position: ['auto', 100] //x, y
                    });
                },
                complete: function () {
                    $('#loading').bPopup().close();
                }

            })
        }
    }
    else {
        mostrarMensaje('mensajeProceso', 'alert', 'Seleccione hora de reproceso');
    }        
}

reprocesarMasivo = function () {

    var horas = "";
    var newhoras = "";
    countHoras = 0;
    $('#tableHorasReproceso tbody input:checked').each(function () {
        horas = horas + $(this).val() + ",";
        countHoras++;
    });

    if (countHoras > 0)
    {
        newhoras = horas.substring(0, horas.length - 1);
    }

    $('#hfListadoHoras').val("");
    if (countHoras > 0) {
        $('#hfListadoHoras').val(newhoras);
    }

    if ($('#txtFechaInicioReproceso').val() != "" && $('#txtFechaFinReproceso').val() != "" && countHoras > 0) {

        if (confirm('¿Está seguro de realizar esta acción?')) {

            $.ajax({
                type: 'POST',
                url: controladorResultado + 'reprocesarmasivo',
                data: {
                    fechaInicio: $('#txtFechaInicioReproceso').val(),
                    fechaFin: $('#txtFechaFinReproceso').val(),
                    horas: $('#hfListadoHoras').val(),
                    version: $('#cbModeloReproceso').val()
                },
                dataType: 'json',
                global:false,
                success: function (result) {
                    if (result == 1) {
                        mostrarMensaje('mensaje', 'alert', 'Se ha iniciado el reproceso, cuando termine le llegará un correo indicando el resultado.');
                        consultar();
                        $('#popupReprocesoMasivo').bPopup().close();
                    }
                    else {
                        mostrarMensaje('mensajeProceso', 'error', 'Se produjo un error.');
                    }
                },
                error: function () {
                    mostrarMensaje('mensajeProceso', 'error', 'Se produjo un error.');
                },
                beforeSend: function () {
                    $('#loading').bPopup({
                        fadeSpeed: 'fast',
                        opacity: 0.4,
                        followSpeed: 500,
                        modalColor: '#000000',
                        modalClose: false,
                        position: ['auto', 100] //x, y
                    });
                },
                complete: function () {
                    $('#loading').bPopup().close();
                }

            })

        }
    }
}

function loadInfoFile(fileName, fileSize) {
    $('#fileInfo').html(fileName + " (" + fileSize + ")");
}

function loadValidacionFile(mensaje) {
    $('#fileInfo').html(mensaje);
}

function mostrarProgreso(porcentaje) {
    $('#progreso').text(porcentaje + "%");
}

procesarArchivo = function () {
    reprocesar(1);
}

function setMarkers(map) {

    // Data for the markers consisting of a name, a LatLng and a zIndex for the
    // order in which these markers should display on top of each other.
    var beaches = [
      ['Bondi Beach', -33.890542, 151.274856, 4],
      ['Coogee Beach', -33.923036, 151.259052, 5],
      ['Cronulla Beach', -34.028249, 151.157507, 3],
      ['Manly Beach', -33.80010128657071, 151.28747820854187, 2],
      ['Maroubra Beach', -33.950198, 151.259302, 1]
    ];
    // Adds markers to the map.

    // Marker sizes are expressed as a Size of X,Y where the origin of the image
    // (0,0) is located in the top left of the image.

    // Origins, anchor positions and coordinates of the marker increase in the X
    // direction to the right and in the Y direction down.
    var image = {
        url: 'https://developers.google.com/maps/documentation/javascript/examples/full/images/beachflag.png',
        // This marker is 20 pixels wide by 32 pixels high.
        size: new google.maps.Size(20, 32),
        // The origin for this image is (0, 0).
        origin: new google.maps.Point(0, 0),
        // The anchor for this image is the base of the flagpole at (0, 32).
        anchor: new google.maps.Point(0, 32)
    };
    // Shapes define the clickable region of the icon. The type defines an HTML
    // <area> element 'poly' which traces out a polygon as a series of X,Y points.
    // The final coordinate closes the poly by connecting to the first coordinate.
    var shape = {
        coords: [1, 1, 1, 20, 18, 20, 18, 1],
        type: 'poly'
    };
    for (var i = 0; i < beaches.length; i++) {
        var beach = beaches[i];
        var marker = new google.maps.Marker({
            position: { lat: beach[1], lng: beach[2] },
            map: map,
            icon: image,
            shape: shape,
            title: beach[0],
            zIndex: beach[3]
        });
    }
}

verArchivo = function (correlativo)
{
    if ($('#txtFecha').val() != "") {
        $.ajax({
            type: 'POST',
            url: controladorResultado + 'folder',
            data: {
                fecha: $('#txtFecha').val(),
                correlativo: correlativo
            },
            dataType:'json',
            success: function (result) {
                $('#hfBaseDirectory').val("");
                $('#hfRelativeDirectory').val(result.PathResultado);
                browser();                
                $('.bread-crumb').css("display", 'none');
                $('#resultado').hide();
                $('#folder').show();
            },
            error: function () {
                mostrarMensaje('mensaje', 'error', 'Ha ocurrido un error.');
            }
        });
    }
    else {
        mostrarMensaje('mensaje', 'alert', 'Seleccione fecha.');
    }

}

exportar = function (){

    if ($('#hfCorrelativo').val() != "") {
        $.ajax({
            type: 'POST',
            url: controladorResultado + "exportar",
            data: {
                correlativo: $('#hfCorrelativo').val()
            },
            dataType: 'json',
            cache: false,
            success: function (resultado) {
                if (resultado == 1) {
                    location.href = controladorResultado + "descargar";
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
        mostrarMensaje('mensaje', 'alert', 'Seleccione hora de ejecución.');
    }
}

exportarMasivo = function () {

    if ($('#txtExportarDesde').val() != "" && $('#txtExportarHasta').val() != "") {

        var accionexportar = 'exportarmasivo';
        var acciondescargar = 'descargarmasivo';
        if ($('#cbExportarWeb').is(':checked')) {
            accionexportar = 'exportarmasivoweb';
            acciondescargar = 'descargarmasivoweb'
        }
        
        var fechaInicio = getFecha($('#txtExportarDesde').val());
        var fechaFin = getFecha($('#txtExportarHasta').val());
        var days = (fechaFin - fechaInicio) / (1000 * 60 * 60 * 24);
        if (days <= 31) {

            $.ajax({
                type: 'POST',
                url: controladorResultado + accionexportar,
                data: {
                    fechaInicio: $('#txtExportarDesde').val(),
                    fechaFin: $('#txtExportarHasta').val(),
                    estimador: $('#cbFuenteEstimador').val(),
                    fuentepd: $('#cbFuentePrograma').val(),
                    version: $('#cbModeloExportar').val()
                },
                dataType: 'json',
                cache: false,
                success: function (resultado) {
                    if (resultado == 1) {
                        location.href = controladorResultado + acciondescargar;
                    }
                    else {
                        mostrarMensaje('mensajeExportar', 'error', 'Ha ocurrido un error.');
                    }
                },
                error: function () {
                    mostrarMensaje('mensajeExportar', 'error', 'Ha ocurrido un error.');
                }
            });
        }
        else
        {
            mostrarMensaje('mensajeExportar', 'alert', 'Solo se permite seleccionar como máximo 30 días.');
        }
    }
    else {
        mostrarMensaje('mensajeExportar', 'alert', 'Seleccione el rango de fechas..');
    }
}

exportarDAT = function () {
    if ($('#txtFechaDAT').val() != "") {

        if ($('#cbTipoArchivo').val() != "0") {
            exportarDatos($('#cbTipoArchivo').val());
        }
        else {
            exportarDatos("1");
            exportarDatos("2");
        }                   
    }
    else {
        mostrarMensaje('mensajeExportar', 'alert', 'Seleccione una fecha..');
    }
}

exportarDatos = function (tipo) {
    $.ajax({
        type: 'POST',
        url: controladorResultado + 'exportardat',
        data: {
            fecha: $('#txtFechaDAT').val(),
            opcion: $('#cbTipoDAT').val(),
            tipo: tipo,
            estimador: $('#cbFuenteEstimador').val(),
            fuentepd: $('#cbFuentePrograma').val(),
            version: $('#cbModeloFile').val()

        },
        dataType: 'json',
        cache: false,
        success: function (resultado) {
            if (resultado != 1) {
                location.href = controladorResultado + 'descargardat?file=' + resultado;
            }
            else {
                mostrarMensaje('mensajeExportar', 'error', 'Ha ocurrido un error.');
            }
        },
        error: function () {
            mostrarMensaje('mensajeExportar', 'error', 'Ha ocurrido un error.');
        }
    });
};

openExportar = function () {
    $('#cbExportarWeb').prop('checked', false);
    $('#divExportar').css('display', 'block');
}

closeExportar = function () {
    $('#divExportar').css('display', 'none');
}

mostrarMensaje = function (id, tipo, mensaje) {
    $('#' + id).removeClass();
    $('#' + id).addClass('action-' + tipo);
    $('#' + id).html(mensaje);
}

getFecha = function (date) {
    var parts = date.split("/");
    var date = new Date(parts[1] + "/" + parts[0] + "/" + parts[2]);
    return date.getTime();
}

function mostrarMensajeDefecto() {
    mostrarMensaje('mensaje', 'message', 'Seleccione fecha de consulta y un tipo de proceso.');
}

/****/
prepararReproceso = function () {

    if ($('#txtFechaInicioReproceso').val() != "" && $('#txtFechaFinReproceso').val() != "") {
        var tipoEjecucion = $("input:radio[name='TipoEjecucion']:checked").val();
        $('#cntNuevoReproceso').hide();
        $('#cntReprocesoActual').hide();

        validarFechaModeloReproceso();

        if (tipoEjecucion == "2") {
            $('#cntReprocesoActual').show();
        }
        else if (tipoEjecucion == "1") {
            $.ajax({
                type: 'POST',
                url: controladorResultado + 'reprocesamiento',
                data: {
                    fechaInicio: $('#txtFechaInicioReproceso').val(),
                    fechaFin: $('#txtFechaFinReproceso').val()
                },
                success: function (evt) {
                    $('#cntResultadoReproceso').html(evt);

                    $('#tablaReprocesamiento').dataTable({
                        "scrollY": 300,
                        "paging": false,
                        "scrollX": false,
                        "sDom": 't'
                    });

                    $('#cntNuevoReproceso').show();

                    $('#cbSelectAllReproceso').click(function (e) {
                        var table = $('#tablaReprocesamiento');
                        $('td.select-reproceso input:checkbox', table).prop('checked', this.checked);
                    });

                    $('.cb-seleccionar-file').click(function (e) {
                        var btn = $(this).closest('tr').find('.button-raw');
                        var spn = $(this).closest('tr').find('.span-file-raw');
                        $(btn).hide();
                        if ($(this).is(':checked')) {
                            $(btn).show();
                        }
                        else {
                            $(spn).text('');
                        }
                    });

                    $('.button-raw').click(function (e) {
                        $('#hfNombreArchivoReproceso').val($(this).closest('tr').attr('id'));
                        $('#hfIdReproceso').val($(this).closest('tr').attr('id'));
                        $('#btnSelectRawReproceso').click();
                    });

                },
                error: function () {
                    mostrarMensaje('mensajeProcesoMasivo', 'error', 'Ha ocurrido un error.');
                }
            });
        }
    }
    else {
        mostrarMensaje('mensajeProcesoMasivo', 'alert', 'Seleccione un rango de fechas correcto.');
    }
};

function crearPupload() {
    uploaderreproceso = new plupload.Uploader({
        runtimes: 'html5,flash,silverlight,html4',
        browse_button: 'btnSelectRawReproceso',
        url: siteRoot + 'cortoplazo/resultado/uploadreproceso',
        flash_swf_url: 'Scripts/Moxie.swf',
        silverlight_xap_url: 'Scripts/Moxie.xap',
        multi_selection: false,
        filters: {
            max_file_size: '2mb',
            mime_types: [
                { title: "Archivos PSSE .raw", extensions: "raw" }
            ]
        },
        init: {
            FilesAdded: function (up, files) {
                if (uploaderreproceso.files.length == 2) {
                    uploaderreproceso.removeFile(uploaderreproceso.files[0]);
                }
                uploaderreproceso.settings.multipart_params = {
                    "id": $('#hfIdReproceso').val()
                }
                uploaderreproceso.start();
                up.refresh();                
            },
            UploadProgress: function (up, file) {
                mostrarMensaje("mensajeProcesoMasivo", "alert", "Por favor espere ...(<strong>" + file.percent + "%</strong>)");
            },
            UploadComplete: function (up, file) {
                mostrarMensaje("mensajeProcesoMasivo", "message", "Complete los datos antes de reprocesar");
                var fila = $('#hfNombreArchivoReproceso').val();
                $('#' + fila).find('.span-file-raw').text(uploaderreproceso.files[0].name + "(" + plupload.formatSize(uploaderreproceso.files[0].size) + ")");
                //$('#' + fila).find('.span-file-raw').text(uploaderreproceso.files[0].name);
            },
            Error: function (up, err) {
                mostrarMensaje('mensajeProcesoMasivo', 'error', 'Ha ocurrido un error.');
            }
        }
    });
    uploaderreproceso.init();
}

confirmarReprocesoCambio = function () {
    var count = 0;
    var flagArchivo = true;
    var arreglo = [];
    $('#tablaReprocesamiento > tbody  > tr').each(function (index, tr) {
        var cbSelect = $(tr).find('.cb-select-process');
        var cbEscenario = $(tr).find('.select-escenario');
        var cbFile = $(tr).find('.cb-seleccionar-file');
        var spanFile = $(tr).find('.span-file-raw');
        var id = $(tr).attr('id');

        if ($(cbSelect).is(':checked')) {
            count++;
            var flagFile = 0;
            if ($(cbFile).is(':checked')) {
                if ($(spanFile).text() == "") {
                    flagArchivo = false;
                }
                else {
                    flagFile = 1;
                }
            }

            var itemArreglo = [];
            itemArreglo.push(id);
            itemArreglo.push($(tr).find('td:eq(1)').text());
            itemArreglo.push($(tr).find('td:eq(2)').text());
            itemArreglo.push($(cbEscenario).val());
            itemArreglo.push(flagFile);
            arreglo.push(itemArreglo);
         }
    });

    if (count > 0) {
        if (flagArchivo) {

            $.ajax({
                type: 'POST',
                url: controladorResultado + 'reprocesarmodificado',
                contentType: 'application/json',
                data: JSON.stringify({
                    data: arreglo,
                    version: $('#cbModeloReproceso').val()
                }),
                dataType: 'json',
                success: function (result) {
                    if (result == 1) {
                        mostrarMensaje('mensaje', 'alert', 'Se ha iniciado el reproceso, cuando finalice le llegará un correo indicando el resultado.');
                        consultar();
                        $('#popupReprocesoMasivo').bPopup().close();
                    }
                    else {
                        mostrarMensaje('mensajeProcesoMasivo', 'error', 'Ha ocurrido un error.');
                    }
                },
                error: function () {
                    mostrarMensaje('mensajeProcesoMasivo', 'error', 'Ha ocurrido un error.');
                }
            });

        }
        else {
            mostrarMensaje("mensajeProcesoMasivo", "alert", "Debe subir el archivo .raw para los periodos en los que indicó que se modificará el archivo.");
        }
    }
    else {
        mostrarMensaje("mensajeProcesoMasivo", "alert", "Seleccione al menos un periodo a reprocesar.");
    }
};

validarFechaModelo = function(){
    
    mostrarMensaje("mensajeProceso", "message", "Selecione hora de reproceso.");
    var mensaje = validarVersionModelo(1);

    if (mensaje != "") {
        mostrarMensaje("mensajeProceso", "alert", mensaje);
    }
};

validarFechaModeloReproceso = function () {
    
    mostrarMensaje("mensajeProcesoMasivo", "message", "Seleccione el rango de fechas y las horas a reprocesar.");
    var mensaje = validarVersionModelo(2);
    if (mensaje != "") {
        mostrarMensaje("mensajeProcesoMasivo", "alert", mensaje);
    }
};

validarVersionModelo = function (tipo) {
    var mensaje = "";    
    var fechaProceso = getFecha($('#txtFecha').val());
    var fechaModelo = getFecha($('#hfFechaVigenciaPR07').val());

    if (tipo == 1) {
        var version = $('#cbModelo').val();
        if (fechaProceso < fechaModelo) {
            if (version == 2) {
                mensaje = "Se está utilizando una nueva versión del modelo para una fecha anterior a su publicación.";
            }
        }
        else {
            if (version == 1) {
                mensaje = "Está utilizando una versión antigua del modelo.";
            }
        }
    }
    else if (tipo == 2) {
        var version = $('#cbModeloReproceso').val();
        var fechaInicio = getFecha($('#txtFechaInicioReproceso').val());
        var fechaFin = getFecha($('#txtFechaFinReproceso').val());

        if (version == 1 && fechaFin >= fechaModelo) {
            mensaje = "Está utilizando una versión antigua del modelo.";           
        }
        if (version == 2 && fechaInicio < fechaModelo) {
            mensaje = "Se está utilizando una nueva versión del modelo para una fecha anterior a su publicación.";
        }
    }

    return mensaje;
};