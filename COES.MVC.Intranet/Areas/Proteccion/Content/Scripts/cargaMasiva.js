let controlador = siteRoot + 'Proteccion/actualizacionmasiva/';
let uploader;
let totCampoVacio = 0;
let totNoNumero = 0;
let totValorNegatico = 0;

let validaInicial = true;
let hot;
let hotOptions;
let evtHot;

let hot1;
let hotOptions1;
let evtHot1;
let cabecerasUsoGeneral = [12, 14, 17, 22, 27, 29];

$(
$(document).ready(function () {
  
    $('#btnConsultar').click(function () {
    });

    $("#btnDescargarFormato").click(function () {
        plantillaEquipos();
    });
    $('#btnMostrarErrores').click(function () {
        validarDatos();
    });

    $('#btnEnviarDatos').click(function () {
        guardarDatos();
    });

    $("#cbTipoUso").change(function () {
        cargarPlantillaVacia();       
    });      

    crearPupload();
    cargarPlantillaVacia();

    $('#btnRegresar').click(function () {
        const url = siteRoot + 'Proteccion/actualizacionmasiva/Index';
        window.open(url, '_self');
    });


}));

let regresarPaginaLog = function () {
    window.location.href = controlador + "index";
}
function plantillaEquipos() {

    $.ajax({
        type: 'POST',
        url: controlador + 'ExportarRele',
        dataType: 'json',
        data: {
            tipoUsoId: $('#cbTipoUso').val()
        },       
        success: function (evt) {
            if (evt.Resultado != "-1") {
                window.location = controlador + "AbrirArchivo?file=" + evt.NombreArchivo;
            }
            else {
                alert(evt.StrMensaje);
            }
        },
        error: function (err) {
            alert('Ha ocurrido un error de exportación');
        }
    });
}

function crearPupload() {
    uploader = new plupload.Uploader({
        runtimes: 'html5,flash,silverlight,html4',
        browse_button: 'btnSelectExcel',
        url: siteRoot + 'Proteccion/actualizacionmasiva/Subir',
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
                mostrarAlerta("Por favor espere ...(<strong>" + file.percent + "%</strong>)");
            },
            UploadComplete: function (up, file) {
                mostrarAlerta("Subida completada <strong>Por favor espere</strong>");
                leerExcelSubido();
            },
            Error: function (up, err) {
                mostrarError(err.code + "-" + err.message);
            }
        }
    });
    uploader.init();
}

function leerExcelSubido() {
    $.ajax({
        type: 'POST',
        url: controlador + 'LeerExcelSubido',
        dataType: 'json',
        data: {
            tipoUsoId: $('#cbTipoUso').val()
        },      
        async: false,        
        success: function (respuesta) {
            if (respuesta.Exito == false) {
                mostrarError(respuesta.Mensaje);
            } else {
                if (typeof hot != 'undefined') {
                    hot.destroy();
                }
                crearHandsonTable(respuesta.Datos, true);
                evtHot = respuesta.Datos;
                mostrarExito("Archivo importado");              
            }
        },
        error: function () {
            mostrarError("Ha ocurrido un error");
        }
    });
}

function cargarPlantillaVacia() {
    $.ajax({
        type: 'POST',
        url: controlador + 'CargarPlantillaVacia',
        dataType: 'json',
        data: {
            tipoUsoId: $('#cbTipoUso').val()
        },
        async: false,
        success: function (respuesta) {
            if (!respuesta.Exito) {
                mostrarError(respuesta.Mensaje);
            } else {
                limpiarMensaje();
                if (typeof hot != 'undefined') {
                    hot.destroy();
                }
                crearHandsonTable(respuesta.Datos, false);
                evtHot = respuesta.Datos;               
            }
        },
        error: function () {
            mostrarError("Ha ocurrido un error");
        }
    });
}

function validarDatos() {    
	setTimeout(function () {
        $.ajax({
            type: 'POST',
            dataType: 'json',
            url: controlador + "ValidarDatosArchivo",
            data: {
                datos: JSON.stringify(hot.getData()),
                tipoUsoId: $('#cbTipoUso').val()
            },
            beforeSend: function () {
                mostrarAlerta("Validando Información ..");
            },
            success: function (respuesta) {
                if (!respuesta.Exito) {
                    mostrarError(respuesta.Mensaje);
                } else {

                    if (typeof hot != 'undefined') {
                        hot.destroy();
                    }
                    crearHandsonTable(respuesta.Datos, false);
                    evtHot = respuesta.Datos;
                    mostrarExito("Archivo validado.");
                  
                }
            },
            error: function () {
                mostrarError("Ocurrió un error");
            }
        });
    }, 100);
}
function guardarDatos() {            
    
    setTimeout(function () {
        if (confirm("¿Desea enviar información a COES?")) {
            $.ajax({
                type: 'POST',
                dataType: 'json',
                url: controlador + "GrabarDatosArchivo",
                data: {
                    datos: JSON.stringify(hot.getData()),
                    tipoUsoId: $('#cbTipoUso').val()
                },
                beforeSend: function () {
                    mostrarAlerta("Enviando Información ..");
                },
                success: function (respuesta) {              
                    if (respuesta.Exito) {
                        mostrarMensaje("Información grabada correctamente.");
                        regresarPaginaLog();
                    } else {
                        if (typeof hot != 'undefined') {
                            hot.destroy();
                        }
                        crearHandsonTable(respuesta.Datos, false);
                        evtHot = respuesta.Datos;

                        if (respuesta.Mensaje != '') {
                            mostrarError(respuesta.Mensaje);
                        } else {
                            mostrarError("Registros con errores, revisar.");    
                        }                                           
                    }
                },
                error: function () {
                    mostrarError("Ocurrió un error");
                }
            });
        }
    }, 100);
}


let container;
function crearHandsonTable(evtHot, validar) {

    function tituloAzul(instance, td, row, col, prop, value, cellProperties) {
        Handsontable.renderers.TextRenderer.apply(this, arguments);
        td.style.fontSize = '12px';
        td.style.color = 'white';
        td.style.background = 'darkblue';
    }

    function negativeValueRenderer(instance, td, row, col, prop, value, cellProperties) {
        Handsontable.renderers.TextRenderer.apply(this, arguments);
        if (parseInt(value, 10) < 0) {
            td.style.color = 'orange';
            td.style.fontStyle = 'italic';
        }
    }

    function calculateSize() {
        let offset;
        offset = Handsontable.Dom.offset(container);
        let availableHeight = 0;
        if (offset.top == 222) {
            availableHeight = $(window).height() - offset.top - 90;
        }
        else {
            availableHeight = $(window).height() - offset.top - 20;
        }

        availableHeight = $(window).width() - 2 * offset.left;
        container.style.height = availableHeight + 'px';
        hot.render();
    }

    container = document.getElementById('detalleFormato');
    Handsontable.renderers.registerRenderer('negativeValueRenderer', negativeValueRenderer);
    Handsontable.Dom.addEvent(window, 'resize', calculateSize);
    Handsontable.Dom.addEvent(container, 'click', function () {
        validaInicial = false;
    });

    hotOptions = {
        data: evtHot.Handson.ListaExcelData,
        height: 0,
        maxRows: evtHot.Handson.ListaExcelData.length,
        maxCols: evtHot.Handson.ListaExcelData[0].length,
        colHeaders: true,
        rowHeaders: true,
        fillHandle: true,
        columnSorting: false,
        className: "htCenter",
        readOnly: evtHot.Handson.ReadOnly,
        fixedRowsTop: evtHot.FilasCabecera,
        fixedColumnsLeft: evtHot.ColumnasCabecera,
        mergeCells: evtHot.Handson.ListaMerge,
        colWidths: evtHot.Handson.ListaColWidth,
        autoRowSize: true,
        afterLoadData: function () {
            this.render();
        },
        hiddenColumns: {
        },
        beforeChange: function (changes, source) {
        },

        cells: function (row, col, prop) {
            let cellProperties = {};
            let formato = "";
            let render;
            let readOnly = false;
            let tipo;
            if (row == 0) {
                if ($('#cbTipoUso').val() == $('#hdnCodigoTipoUsoGeneral').val()) {
                    if (cabecerasUsoGeneral.includes(col)) {
                        render = tituloAzul
                    }
                } else if (col == 12) {
                    render = tituloAzul
                }
                
                readOnly = true;
            }
            else if (row == 1) {
                render = tituloAzul;
                readOnly = true;
            }
            else if (row > 1) {

                if (col < 1) {
                    readOnly = true;
                }
                if (col == 0) {
                    let lastColumnValue = this.instance.getDataAtCell(row, 0);  
                    
                    if (lastColumnValue && lastColumnValue.trim() !== '') {
                       
                        cellProperties.renderer = function (instance, td, row, col, prop, value, cellProperties) {
                            td.innerHTML = `<img src="../Content/Images/error.png" title="${lastColumnValue}" style="width:20px; height:20px;display:block; margin: 0 auto;"/>`; // Imagen con tooltip
                        };

                        return cellProperties;

                    } else {
                       
                        cellProperties.renderer = function (instance, td, row, col, prop, value, cellProperties) {
                            td.innerHTML = value;
                            td.style.backgroundColor = '';  // Restaurar fondo por defecto
                        };
                    }
                }
            }

            cellProperties = {
                renderer: render,
                format: formato,
                type: tipo,
                readOnly: readOnly
            }

            return cellProperties;
        }
    };

    hot = new Handsontable(container, hotOptions);
    //le pasaban parametro 1 pero no recibe parametros

    calculateSize();       
}



function validarRegistros(data) {
    for (let row = 0; row < data.Handson.ListaExcelData.length; row++) {
        for (let col = 0; col < data.Handson.ListaExcelData[0].length; col++) {
            if ((row > data.FilasCabecera) && (col <= data.ColumnasCabecera) && (!data.Handson.ReadOnly)) {
                if (!data.Handson.ListaFilaReadOnly[row]) {
                    let value = data.Handson.ListaExcelData[row][col];
                    let celda = getExcelColumnName(parseInt(col) + 1) + (parseInt(row) + 1).toString();

                    if (col == 0) {
                        if (value == null || value == "") {
                            agregarError(celda, value, 0);
                        }
                    }
                    if (col == 5 || col == 6) {
                        if (!validarDecimal(value)) {
                            agregarError(celda, value, 1);
                        } else if (Number(value) < 0) {
                                agregarError(celda, value, 2);
                        }
                    }
                    if (col == 7 || col == 8) {
                        if (!validarFecha(value)) {
                            agregarError(celda, value, 3);
                        }
                    }
                }
            }
        }
    }
}

function validarError(celda) {
    for (let j in listErrores) {
        if (listErrores[j]['Celda'] == celda) {
            return false;
        }
    }
    return true;
}

function getExcelColumnName(pi_columnNumber) {
    let li_dividend = pi_columnNumber;
    let ls_columnName = "";
    let li_modulo;

    while (li_dividend > 0) {
        li_modulo = (li_dividend - 1) % 26;
        ls_columnName = String.fromCharCode(65 + li_modulo) + ls_columnName;
        li_dividend = Math.floor((li_dividend - li_modulo) / 26);
    }

    return ls_columnName;
}

let validarDecimal = function (n) {
    if (n == "")
        return false;

    let count = 0;
    let strCheck = "0123456789.-E";
    let i;

    for (i in n) {
        if (strCheck.indexOf(n[i]) == -1)
            return false;
        else if (n[i] == '.') {
                count = count + 1;
        }
        
    }
    
    return count <= 1;
}

let validarFecha = function (valor) {
    if (valor == null || valor.length <= 0) return false;
    let expresionFecha = /^(0?\d|[12]\d|3[01])\/(0?\d|1[0-2])\/(19[5-9]\d|20[0-4]\d|2050)\s([01]?\d|2[0-3]):[0-5]\d$/;
    return expresionFecha.test(valor);
};

function agregarError(celda, valor, tipo) {
    if (validarError(celda)) {
        let regError = {
            Celda: celda,
            Valor: valor,
            Tipo: tipo
        };

        listErrores.push(regError);
        switch (tipo) {
            case 0:
                totCampoVacio++;
                break;
            case 1:
                totNoNumero++;
                break;
            case 2:
                totValorNegatico++;
                break;
        }
    }
}

function mostrarError(mensaje) {
    $('#mensaje').removeClass();
    $('#mensaje').html(mensaje);
    $('#mensaje').addClass('action-error');
}
function mostrarAlerta(mensaje) {
    $('#mensaje').removeClass();
    $('#mensaje').html(mensaje);
    $('#mensaje').addClass('action-alert');
}
function mostrarExito(mensaje) {
    $('#mensaje').removeClass();
    $('#mensaje').html(mensaje);
    $('#mensaje').addClass('action-exito');
}

function limpiarMensaje() {
    $('#mensaje').removeClass();
    $('#mensaje').html("");
}