var controlador = siteRoot + 'calculoresarcimiento/medicion/';
var uploader;
var hot = null;
$(function () {        

    $('#btnConsultar').on('click', function () {
        consultar();
    });

    $('#btnRegresar').on('click', function () {
        document.location.href = siteRoot + 'calculoresarcimiento/calidadproducto';
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
        verEnvios();
    });

    $('#btnEnviarComentario').on('click', function () {
        grabarMediciones();
    });

    habilitarEnvio(false);
    crearPupload();    
});

function consultar() {
    mostrarMensajeDefecto();
    if ($('#cbEmpresa').val() != "") {        
        $.ajax({
            type: 'POST',
            url: controlador + 'consultar',
            data: {
                empresa: $('#cbEmpresa').val(),
                evento: $('#hfIdEvento').val()
            },
            dataType: 'json',
            success: function (result) {
                if (result.Result == 1) {
                    cargarGrilla(result);
                    habilitarEnvio(true);
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
        mostrarMensaje('mensaje', 'alert', 'Seleccione suministrador.');
    }
}

function habilitarEnvio(flag) {

    if (!flag) {
        $('#divAcciones').hide();
        $('#ctnBusqueda').hide();
        $('#detalleFormato').html('');
    }
    else {
        $('#divAcciones').show();
        $('#ctnBusqueda').show();
    }
}

function descargarFormato() {
    $.ajax({
        type: 'POST',
        url: controlador + "GenerarFormato",
        data: {
            empresa: $('#cbEmpresa').val(),
            evento: $('#hfIdEvento').val()
        },
        dataType: 'json',
        cache: false,
        success: function (resultado) {
            if (resultado == 1) {
                location.href = controlador + "DescargarFormato";
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

function enviarDatos() {
    $('#txtComentario').val("");
    var validacion = validarDatos();

    if (validacion.length > 0) {
        pintarError(validacion);
    }
    else {
        $('#popupEnvio').bPopup({});
    }
}

function pintarError(validaciones) {
    $('#contenidoError').html(obtenerErroresMediciones(validaciones));
    $('#popupErrores').bPopup({});
}

function obtenerErroresMediciones(data) {

    var headersAlterno = ["Nª", "Fecha Hora", "Tensión R-S", "Tensión S-T", "Tensión R-S", "∆%Vp", "a", "Ap", "Ep (kWh)", "a*Ap*Ep", "Compensación calculada por el Suministrador en US$"];

    var html = "<table class='pretty tabla-adicional' id='tablaErrores'>";
    html = html + " <thead>";
    html = html + "     <tr>";
    html = html + "         <th>Fila</th>";
    html = html + "         <th>Columna</th>";
    html = html + "         <th>Error</th>";
    html = html + "     </tr>";
    html = html + " </thead>";
    html = html + " <tbody>";
    for (var k in data) {
        html = html + "     <tr>";
        html = html + "         <td>" + (data[k].row + 1) + "</td>";
        if (data[k].col != -1) {
            html = html + "         <td>" + headersAlterno[data[k].col] + "</td>";
        }
        else {
            html = html + "         <td></td>";
        }
        html = html + "         <td>" + data[k].validation + "</td>";
        html = html + "     </tr>";
    }
    html = html + " </tbody>";
    html = html + "</table>";

    return html;
}

function grabarMediciones() {
    $.ajax({
        type: 'POST',
        url: controlador + 'GrabarMedicion',
        contentType: 'application/json',
        data: JSON.stringify({
            data: hot.getData(),
            empresa: $('#cbEmpresa').val(),
            evento: $('#hfIdEvento').val(),
            comentario: $('#txtComentario').val()
        }),
        dataType: 'json',
        success: function (result) {
            if (result == 1) {
                $('#popupEnvio').bPopup().close();
                consultar();
                mostrarMensaje('mensaje', 'exito', 'Los datos fueron grabados correctamente.');
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

function validarDatos() {
    var data = hot.getData();
    var validaciones = [];
    var flag = true;
    var camposTabla = [
        { id: 0},                       //- N
        { id: 1},                       //- Eliminar
        { id: 2, countDecimals: 3 },    //- Tensión R-S
        { id: 3, countDecimals: 3 },    //- Tensión S-T
        { id: 4, countDecimals: 3 },    //- Tensión T-R
        { id: 5, countDecimals: 3 },    //- ∆%Vp
        { id: 6, countDecimals: 2 },    //- a
        { id: 7, countDecimals: 2 },    //- Ap
        { id: 8, countDecimals: 4 },    //- Ep (kWh)
        { id: 9, countDecimals: 4 },    //- a*Ap*Ep
    ];

    if (data[0][9] == null || data[0][9] == "") {
        validaciones.push({ row: 0, col: 9, validation: Validacion.DatoObligatorio });
        flag = false;
    }
    else {
        if (!validarNumero(data[0][9])) {
            validaciones.push({ row: 0, col: 9, validation: Validacion.FormatoNumero });
            flag = false;
        }
        else {

            if (!validarNegativo(data[0][9])) {
                validaciones.push({ row: 0, col: 9, validation: Validacion.FormatoDecimalNegativo });
                flag = false;
            }

            if (!validarCantidadDecimales(data[0][9], 4)) {
                validaciones.push({ row: 0, col: 9, validation: Validacion.FormatoDecimal + 4});
                flag = false;
            }
        }
    }

    for (var i = 4; i < data.length; i++) {
        for (var j = 2; j < 10; j++) {

            if (data[i][j] == null || data[i][j] == "") {
                validaciones.push({ row: i - 4, col: j, validation: Validacion.DatoObligatorio });
                flag = false;
            }
            else {
                if (!validarNumero(data[i][j])) {
                    validaciones.push({ row: i - 4, col: j, validation: Validacion.FormatoNumero });
                    flag = false;
                }
                else {

                    if (!validarNegativo(data[i][j])) {
                        validaciones.push({ row: i - 4, col: j, validation: Validacion.FormatoDecimalNegativo });
                        flag = false;
                    }

                    if (!validarCantidadDecimales(data[i][j], camposTabla[j].countDecimals)) {
                        validaciones.push({ row: i - 4, col: j, validation: Validacion.FormatoDecimal + camposTabla[j].countDecimals });
                        flag = false;
                    }
                }
            }
        }

    }

    if (flag) {
        var tension = parseFloat($('#hfTensionEvento').val());
        var factor = parseFloat($('#hfFactorCompensacionUnitaria').val());
        var sum = 0;

        for (var i = 4; i < data.length; i++) {
            var vp = Math.max(Math.abs(parseFloat(data[i][2]) - tension), Math.abs(parseFloat(data[i][3]) - tension), Math.abs(parseFloat(data[i][4]) - tension)) * 100 / tension;

            if (truncateDecimals(vp, 3) != parseFloat(data[i][5])) {
                //- Valor  ∆%Vp no corresponde
                validaciones.push({ row: i - 4, col: j, validation: "El valor ∆%Vp no corresponde, debe ser: " + truncateDecimals(vp, 3) });
            }

            if (truncateDecimals(factor, 2) != parseFloat(data[i][6])) {
                //- Valor a no corresponde
                validaciones.push({ row: i - 4, col: j, validation: "El valor a no corresponde, debe ser: " + truncateDecimals(factor, 2)});
            }

            var Ap = 0;
            if (vp > 5.0 && vp <= 7.5) {
                Ap = 1;
            }
            if (vp > 7.5) Ap = 2 + vp - 7.5;

            if (truncateDecimals(Ap, 2) != parseFloat(data[i][7])) {
                //- Valor Ap no corresponde
                validaciones.push({ row: i - 4, col: j, validation: "El valor Ap no corresponde, debe ser: " + truncateDecimals(Ap, 2) });
            }

            var aApEp = factor * Ap * parseFloat(data[i][8]);

            if (truncateDecimals(aApEp, 4) != parseFloat(data[i][9])) {
                //- Valor a*Ap*Ep no corresponde
                validaciones.push({ row: i - 4, col: j, validation: "El valor a*Ap*Ep no corresponde, debe ser: " + truncateDecimals(aApEp, 4) });
            }
            sum = sum + aApEp;
        }

        var resarcimiento = parseFloat(data[0][9]);

        if (Math.abs(sum - resarcimiento) > 0.001) {
            validaciones.push({ row: 0, col: 10, validation: "El cálculo de resarcimiento es incorrecto, debe ser: " + truncateDecimals(sum, 4) });
        }
    }

    return validaciones;
}

function mostrarErrores() {
    mostrarMensajeDefecto();
    var validacion = validarDatos();

    if (validacion.length > 0) {
        mostrarMensaje('mensaje', 'alert', 'Se encontraron errores en los datos ingresados');
        pintarError(validacion);
    }
    else {
        mostrarMensaje('mensaje', 'exito', 'No se encontraron errores en los datos ingresados');
    }
}

function truncateDecimals(num, decimals) {
    /*var multiplier = Math.pow(10, digits),
        adjustedNum = number * multiplier,
        truncatedNum = Math[adjustedNum < 0 ? 'ceil' : 'floor'](adjustedNum);

    return truncatedNum / multiplier;*/
    var t = Math.pow(10, decimals);
    return (Math.round((num * t) + (decimals > 0 ? 1 : 0) * (Math.sign(num) * (10 / Math.pow(100, decimals)))) / t).toFixed(decimals);
};

function verEnvios() {
    mostrarMensajeDefecto();
    $.ajax({
        type: 'POST',
        url: controlador + 'Envios',
        data: {
            empresa: $('#cbEmpresa').val(),
            evento: $('#hfIdEvento').val()
        },
        dataType: 'json',
        global: false,
        success: function (result) {
            var html = obtenerHtmlEnvios(result);
            $('#contenidoEnvios').html(html);
            $('#popupEnvios').bPopup({});
        },
        error: function () {
            mostrarMensaje('mensaje', 'error', 'Se ha producido un error.');
        }
    });
}

function crearPupload() {
    uploader = new plupload.Uploader({
        runtimes: 'html5,flash,silverlight,html4',
        browse_button: 'btnSubirFormato',
        url: controlador + 'Upload',
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
                mostrarMensaje('mensaje', 'alert', "Por favor espere ...(<strong>" + file.percent + "%</strong>)");
            },
            UploadComplete: function (up, file) {
                mostrarMensaje('mensaje', 'alert', "Subida completada. <strong>Por favor espere...</strong>");
                procesarArchivo();

            },
            Error: function (up, err) {
                mostrarMensaje('mensaje', 'error', err.code + "-" + err.message);
            }
        }
    });
    uploader.init();
}

function procesarArchivo() {
    $.ajax({
        type: 'POST',
        url: controlador + 'ImportarMedicion',
        dataType: 'json',
        data: {
            empresa: $('#cbEmpresa').val(),
            evento: $('#hfIdEvento').val()
        },
        success: function (result) {
            if (result.Result == 1) {
                mostrarMensaje('mensaje', 'exito', 'Los datos se cargaron correctamente en el Excel web, presione el botón <strong>Enviar</strong> para grabar.');
                cargarGrilla(result);
            }
            else if (result.Result == -2) {
                var html = "El formato Excel que está intentando importar no tiene la estructura correcta. Es posible que haya eliminado filas y/o columnas.";              
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

function cargarGrilla(result) {

    $('#hfFactorCompensacionUnitaria').val(result.FactorCompensacionUnitaria);
    $('#hfTensionEvento').val(result.Tension);

    var data = [
        ["Compensación calculada por el Suministrador en US$", "", "", "", "", "", "", "", "", result.Resarcimiento],
        ["", "", "", "", "", "", "", "", "", ""],
        ["Nª", "Fecha Hora", "Tensión R-S", "Tensión S-T", "Tensión R-T", "∆%Vp", "a", "Ap", "Ep (kWh)", "a*Ap*Ep"],
        ["", "(dd/mm/aaaa hh:mm)", "(V)", "(V)", "(V)", "", "", "", "", ""]       
    ];

    for (var i in result.Data) {
        data.push(result.Data[i]);
    }

    var merge = [
        { row: 0, col: 0, rowspan: 1, colspan: 9 },
        { row: 1, col: 0, rowspan: 1, colspan: 10 },
        { row: 2, col: 0, rowspan: 2, colspan: 1 },
        { row: 2, col: 1, rowspan: 2, colspan: 1 },
        { row: 2, col: 5, rowspan: 2, colspan: 1 },
        { row: 2, col: 6, rowspan: 2, colspan: 1 },
        { row: 2, col: 7, rowspan: 2, colspan: 1 },
        { row: 2, col: 8, rowspan: 2, colspan: 1 },
        { row: 2, col: 9, rowspan: 2, colspan: 1 },
        { row: 2, col: 10, rowspan: 2, colspan: 1 },
    ]
    var grilla = document.getElementById('detalleFormato');
    var colwidths = [80, 160, 100, 100, 100, 100, 100, 100, 100, 100];

    if (hot != null) hot.destroy();

    hot = new Handsontable(grilla, {
        data: data,
        mergeCells: merge,
        height: 1000,
        colWidths: colwidths,
        cells: function (row, col, prop) {
            var cellProperties = {};

            if (row == 0 && col < 9) {
                cellProperties.renderer = tituloRenderer;
                cellProperties.readOnly = true;
            }
            if (row == 2 || row == 3) {
                cellProperties.renderer = tituloRenderer;
                cellProperties.readOnly = true;
            }

            if (row >= 4 && col < 2) {
                cellProperties.renderer = disabledRenderer;
                cellProperties.readOnly = true;
            }
            if (row >= 4 && col >= 2) {
                cellProperties.renderer = normalRenderer;
            }


            return cellProperties;
        },
        afterLoadData: function () {
            this.render();
        }
    });
    hot.render();

}

var tituloRenderer = function (instance, td, row, col, prop, value, cellProperties) {
    Handsontable.renderers.TextRenderer.apply(this, arguments);
    td.style.fontWeight = 'bold';
    td.style.fontSize = '11px';
    td.style.textAlign = 'center';
    td.style.verticalAlign = 'middle';
    td.style.color = '#fff';
    td.style.backgroundColor = '#4C97C3';
};

var disabledRenderer = function (instance, td, row, col, prop, value, cellProperties) {
    Handsontable.renderers.TextRenderer.apply(this, arguments);
    td.style.backgroundColor = '#F2F2F2';
    td.style.fontSize = '11px';
    td.style.verticalAlign = 'middle';
    td.style.textAlign = 'center';
};

var normalRenderer = function (instance, td, row, col, prop, value, cellProperties) {
    Handsontable.renderers.TextRenderer.apply(this, arguments);
   
    td.style.fontSize = '11px';
    td.style.verticalAlign = 'middle';
    td.style.textAlign = 'right';
};

function mostrarMensajeDefecto() {
    mostrarMensaje('mensaje', 'message', 'Complete los datos solicitados.');
}

function obtenerHtmlEnvios(result) {
    var html = "<table class='pretty tabla-adicional' id='tablaEnvios'>";
    html = html + " <thead>";
    html = html + "     <tr>";
    html = html + "         <th>ID Envío</th>";
    html = html + "         <th>Usuario</th>";
    html = html + "         <th>Fecha</th>";    
    html = html + "         <th>Comentario</th>";
    html = html + "     </tr>";
    html = html + " </thead>";
    html = html + " <tbody>";
    for (var k in result) {
        var comentario = "";       
        if (result[k].Reevlomotivocarga != null) comentario = result[k].Reevlomotivocarga;
        html = html + "     <tr>";
        html = html + "         <td>" + result[k].Reevlocodi + "</td>";
        html = html + "         <td>" + result[k].Reevlousucreacion + "</td>";
        html = html + "         <td>" + result[k].Fecha + "</td>";     
        html = html + "         <td>" + comentario + "</td>";
        html = html + "     </tr>";
    }
    html = html + " </tbody>";
    html = html + "</table>";

    return html;
}

function mostrarMensaje(id, tipo, mensaje) {
    $('#' + id).removeClass();
    $('#' + id).addClass('action-' + tipo);
    $('#' + id).html(mensaje);
}

function limpiarMensaje(id) {
    $('#' + id).removeClass();
    $('#' + id).html('');
}

var Validacion = {
    DatoObligatorio: "El dato es obligatorio",
    FormatoNumero: "El dato debe ser numérico",
    FormatoDecimal: "La cantidad máxima de decimales es ",
    FormatoDecimalNegativo: "No se permiten valores negativos"    
};

function validarNumero(texto) {
    return /^-?[\d.]+(?:e-?\d+)?$/.test(texto);
}

function validarCantidadDecimales(texto, decimales) {
    if (contarDecimales(texto) <= decimales) {
        return true;
    }
    return false;
}

function contarDecimales(value) {
    if (Math.floor(value) === value) return 0;
    if (value.toString().split(".").length == 1) return 0;
    return value.toString().split(".")[1].length || 0;
}

function validarNegativo(texto) {
    if (validarNumero(texto)) {
        if (parseFloat(texto) < 0) return false;
        return true;
    }
    return false;
}