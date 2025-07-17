// Constantes generales para el flujo de datos del modulo
const controller = siteRoot + "PrimasRER/InsumosRER/";
const imageRoot = siteRoot + "Content/Images/";

// Variables para las grillas
var dtInsumos, dtCargaManual, dtExportar

$(document).ready(function () {

    $('#tab-container').easytabs({
        animate: false
    });

    listarPeriodos();
    listarVersiones();

    // ---------------------------------------------------------------------------------------

    // Llamada al metodo que inicializa la grilla de Insumos
    obtenerDtInsumos();

    // Llamada al metodo que lista y llena la grilla con los Insumos
    listarInsumos();

    $('#cbPeriodo').on('change', function () {
        listarInsumos();
    });
    
    $('#cbVersion').on('change', function () {
        listarInsumos();
    });


    $('#btnProcesar').click(function () {
        let data = validateAnioTarifarioAndVersion('#cbPeriodo', '#cbVersion');
        if (data.Error) {
            alert(data.ErrorMessage);
            return false;
        }

        let ruta = $('#txtDireccion').val();

        if (!ruta) {
            alert('No coloco la ruta donde se ubica los archivos: sddp.dat / gergnd.csv / gerhid.csv');
            return false;
        }

        ProcesarArchivosSddp();
    });

    $('#btnDescargar').click(function () {
        let data = validateAnioTarifarioAndVersion('#cbPeriodo', '#cbVersion');
        if (data.Error) {
            alert(data.ErrorMessage);
            return false;
        }

        DescargarArchivoSDDP();
    });

    // ---------------------------------------------------------------------------------------
    // Botones Aceptar y Cancelar del popup de Cargas Manuales
    $('#pop-aceptar-cm').click(function () {
        mesTarifario = $('#cbPeriodo').val();
        version = $('#cbVersion').val();

        if (!mesTarifario) {
            alert('No selecciono el año tarifario');
            return false;
        }

        if (!version) {
            alert('No selecciono la versión');
            return false;
        }
    });
    $('#pop-cancelar-cm').click(function () {
        $('#pop-carga-manual').bPopup().close();
    });

    // Botones Aceptar y Cancelar del popup de Exportar
    $('#pop-aceptar-e').click(function () {
        ejecutarExportarInsumos();
    });

    $('#pop-cancelar-e').click(function () {
        $('#pop-exportar').bPopup().close();
    });

    $('#dtCargaManual').on('change', '.chkSeleccion', function () {
        // Encuentra la fila (tr) que contiene el checkbox
        let fila = $(this).closest('tr');

        // Encuentra el botón "btnCarga" en la misma fila
        let btnCarga = fila.find('.btnCarga');

        // Verifica si el checkbox está marcado
        if ($(this).is(':checked')) {
            // Mostrar el botón "btnCarga"
            btnCarga.show();
        } else {
            // Ocultar el botón "btnCarga"
            btnCarga.hide();
        }
    });

    $("#btnManualUsuario").click(function () {
        window.location = controller + 'DescargarManualUsuario';
    });

    obtenerDtCargaManual();
    obtenerDtExportar();
});

function listarPeriodos() {
    $.ajax({
        type: 'POST',
        url: controller + 'ListarPeriodos',
        contentType: 'application/json; charset=utf-8',
        data: {},
        datatype: 'json',
        traditional: true,
        success: function (model) {

            $("#cbPeriodo").empty();
            $('#cbPeriodo').append("<option value='-1'>-SELECCIONAR-</option>");
            $.each(model.ListaAniosTarifario, function (k, v) {
                let option = '<option value =' + v.Anio + '>' + v.NomAnio + '</option>';
                $('#cbPeriodo').append(option);
            })
        },
        error: function () {
            alert("Ha ocurrido un problema...");
        }
    });
}

function listarVersiones() {
    $.ajax({
        type: 'POST',
        url: controller + 'ListarVersiones',
        contentType: 'application/json; charset=utf-8',
        data: {},
        datatype: 'json',
        traditional: true,
        success: function (model) {

            $("#cbVersion").empty();
            $('#cbVersion').append("<option value='-1'>-SELECCIONAR-</option>");
            $.each(model.ListaVersiones, function (k, v) {
                let option = '<option value =' + v.Numero + '>' + v.Nombre + '</option>';
                $('#cbVersion').append(option);
            })
        },
        error: function () {
            alert("Ha ocurrido un problema...");
        }
    });
}

function obtenerDtInsumos() {
    dtInsumos = $('#dtInsumos').DataTable({
        data: [],
        columns: [
            { title: 'Insumo', data: 'NomInsumo' },
            { title: 'Usuario y fecha de Última Importación', data: 'FecUltImportacion' },
            { title: 'Log', data: null },
            { title: '', data: null },
            { title: '', data: null },
        ],
        columnDefs: [
            {
                width: "150px",
                targets: 0,
            },
            {
                width: "20px",
                targets: 1,
            },
            {
                width: "5px",
                targets: 2,
                defaultContent:
                    '<div class="dt-col-options">' +
                    `<img src="${imageRoot}btn-open.png" class="dt-ico-log" title="Log" /> ` +
                    '</div>',
            },
            {
                width: "5px",
                targets: 3,
                defaultContent:
                    '<div class="dt-col-options">' +
                    `<img src="${imageRoot}Copiado.png" class="dt-ico-importar" title="Importar" /> ` +
                    '</div>',
            },
            {
                width: "20px",
                targets: 4,
                createdCell: function (cell, cellData, rowData, rowIndex, colIndex) {
                    if (rowIndex === 5 || rowIndex === 6) { // Ocultar el quinto y sexto registro
                        $(cell).empty(); // Eliminar el contenido de las celdas
                    } else {
                        $(cell).html(
                            '<div class="dt-col-options">' +
                            `<input type="button" value="Carga Manual" style="width:120px; height: 20px;" class="dt-ico-carga-manual" id="insumo_${rowIndex + 1}"/> ` +
                            `<input type="button" value="Exportar" style="width:120px; height: 20px;" class="dt-ico-exportar"/>` +
                            '</div>'
                        );
                    }
                }
            },
        ],
        createdRow: function (row, data, index) {

            $(row).find('.dt-ico-log').on('click', function () {
                let data = validateAnioTarifarioAndVersion('#cbPeriodo', '#cbVersion');
                if (data.Error) {
                    alert(data.ErrorMessage);
                    return false;
                }
                let opcion = 0;
                switch (index) {
                    case 0: opcion = 1;
                        msg = "Inyección Neta 15 min";
                        break;
                    case 1: opcion = 2;
                        msg = "Costo Marginal c/15 minutos";
                        break;
                    case 2: opcion = 3; 
                        msg = "Ingresos por Potencia";
                        break;
                    case 3: opcion = 4; 
                        msg = "Ingresos por Cargo Prima RER";
                        break;
                    case 4: opcion = 5; 
                        msg = "Energía Dejada de Inyectar (EDI) c/15 minutos";
                        break;
                    case 5: opcion = 6; 
                        msg = "Saldos VTEA c/15 minutos";
                        break;
                    case 6: opcion = 7; 
                        msg = "Saldos VTP";
                        break;
                    default:
                        alert('Opción no disponible');
                        return false;
                }

                // Muestra la ventana emergente
                popCaso = $('#pop-log').bPopup({
                    easing: 'easeOutBack',
                    speed: 350,
                    transition: 'fadeIn',
                    modalClose: false,
                    onOpen: function () {
                        // Carga grilla de Cargas Manuales
                        obtenerLog(data.AnioTarifario, data.NumeroVersion, opcion);
                    }
                });
            });

            $(row).find('.dt-ico-importar').on('click', function () {
                let data = validateAnioTarifarioAndVersion('#cbPeriodo', '#cbVersion');
                if (data.Error) {
                    alert(data.ErrorMessage);
                    return false;
                }
                estadoAnioTarifarioVersion(function (isEstadoValid, mensaje) {
                    if (!isEstadoValid) {
                        alert(mensaje + 'automática');
                        return false;
                    } else {
                        estadoDelProcesarCalculo(function (isProcesado, mensajeProcesado) {
                            if (!isProcesado) {
                                alert(mensajeProcesado);
                                return false;
                            } else {
                                let opcion = 0;
                                let msg = "";
                                switch (index) {
                                    case 0: opcion = 1;
                                        msg = "Inyección Neta 15 min.";
                                        break;
                                    case 1: opcion = 2;
                                        msg = "Costo Marginal c/15 minutos";
                                        break;
                                    case 2: opcion = 3;
                                        msg = "Ingresos por Potencia";
                                        break;
                                    case 3: opcion = 4;
                                        msg = "Ingresos por Cargo Prima RER";
                                        break;
                                    case 4: opcion = 5;
                                        msg = "Energía Dejada de Inyectar (EDI) c/15 minutos";
                                        break;
                                    case 5: opcion = 6;
                                        msg = "Saldos VTEA c/15 minutos";
                                        break;
                                    case 6: opcion = 7;
                                        msg = "Saldos VTP";
                                        break;
                                    default:
                                        alert('Opción no disponible');
                                        return false;
                                }
                                if (opcion > 0) {
                                    if (confirm("¿Desea importar la información de " + msg + "?")) {
                                        importarTotalInsumos(data.AnioTarifario, data.NumeroVersion, opcion);
                                    }
                                    else {
                                        return false;
                                    }
                                }
                            }
                        })
                    }
                });
            });

            $(row).find('.dt-ico-carga-manual').on('click', function () {
                let data = validateAnioTarifarioAndVersion('#cbPeriodo', '#cbVersion');
                if (data.Error) {
                    alert(data.ErrorMessage);
                    return false;
                }
                estadoAnioTarifarioVersion(function (isEstadoValid, mensaje) {
                    if (!isEstadoValid) {
                        alert(mensaje + 'manual');
                        return false;
                    } else {
                        estadoDelProcesarCalculo(function (isProcesado, mensajeProcesado) {
                            if (!isProcesado) {
                                alert(mensajeProcesado);
                                return false;
                            } else {
                                opcion = 0;
                                switch (index) {
                                    case 0: opcion = 1; //Inyección Neta 15 min
                                        break;
                                    case 1: opcion = 2; //Costo Marginal c/15 minutos
                                        break;
                                    case 2: opcion = 3; //Ingresos por Potencia
                                        break;
                                    case 3: opcion = 4; //Ingresos por Cargo Prima RER
                                        break;
                                    case 4: opcion = 5; //Energía Dejada de Inyectar (EDI) c/15 minutos
                                        break;
                                    case 5: opcion = 6; //Saldos VTEA c/15 minutos
                                        break;
                                    case 6: opcion = 7; //Saldos VTP
                                        break;
                                    default:
                                        alert('Opción no disponible');
                                        return false;
                                }

                                // Muestra la ventana emergente
                                popCaso = $('#pop-carga-manual').bPopup({
                                    easing: 'easeOutBack',
                                    speed: 350,
                                    transition: 'fadeIn',
                                    modalClose: false,
                                    onOpen: function () {
                                        // Carga grilla de Cargas Manuales
                                        listarCargaManual(data.AnioTarifario, data.NumeroVersion, opcion);
                                    }
                                });

                            }

                        })
                        
                    }
                });
            });

            $(row).find('.dt-ico-exportar').on('click', function () {
                let data = validateAnioTarifarioAndVersion('#cbPeriodo', '#cbVersion');
                if (data.Error) {
                    alert(data.ErrorMessage);
                    return false;
                }

                opcion = 0;
                switch (index) {
                    case 0: opcion = 1; //Inyección Neta 15 min
                        break;
                    case 1: opcion = 2; //Costo Marginal c/15 minutos
                        break;
                    case 2: opcion = 3; //Ingresos por Potencia
                        break;
                    case 3: opcion = 4; //Ingresos por Cargo Prima RER
                        break;
                    case 4: opcion = 5; //Energía Dejada de Inyectar (EDI) c/15 minutos
                        break;
                    case 5: opcion = 6; //Saldos VTEA c/15 minutos
                        break;
                    case 6: opcion = 7; //Saldos VTP
                        break;
                    default:
                        alert('Opción no disponible');
                        return false;
                }

                // Muestra la ventana emergente
                popCaso = $('#pop-exportar').bPopup({
                    easing: 'easeOutBack',
                    speed: 350,
                    transition: 'fadeIn',
                    modalClose: false,
                    onOpen: function () {
                        // Carga grilla de Exportaciones
                        listarExportar(data.AnioTarifario, data.NumeroVersion, opcion);
                    }
                });
            });
          
        },
        searching: false,
        bLengthChange: false,
        bSort: false,
        destroy: true,
        paging: true,
        pageLength: 20,
        info: false,
    });
}

function estadoAnioTarifarioVersion(callback) {
    let iAnioTarifario = $('#cbPeriodo').val()
    let iVersion = $('#cbVersion').val();

    $.ajax({
        type: 'POST',
        url: controller + 'EstadoAnioTarifarioVersion',
        contentType: 'application/json; charset=utf-8',
        data: JSON.stringify({
            iAnioTarifario: iAnioTarifario,
            iVersion: iVersion
        }),
        success: function (data) {
            if (data.Detalle === "1") {
                callback(true, data.Mensaje); // Llamada al callback con true si el estado es válido
            } else {
                callback(false, data.Mensaje); // Llamada al callback con true si el estado no es válido
            }
        },
        error: function (xhr, status, error) {
            callback(false, 'Error en la solicitud: ' + error); // Llamada al callback con false en caso de error
        }
    });
}

function estadoDelProcesarCalculo(callback) {
    let iAnioTarifario = $('#cbPeriodo').val()
    let iVersion = $('#cbVersion').val();

    $.ajax({
        type: 'POST',
        url: controller + 'EstadoDelProcesarCalculo',
        contentType: 'application/json; charset=utf-8',
        data: JSON.stringify({
            iAnioTarifario: iAnioTarifario,
            iVersion: iVersion
        }),
        success: function (data) {
            if (data.Detalle === "1") {
                callback(true, data.Mensaje); // Llamada al callback con true si el estado es válido
            } else {
                callback(false, data.Mensaje); // Llamada al callback con true si el estado no es válido
            }
        },
        error: function (xhr, status, error) {
            callback(false, 'Error en la solicitud: ' + error); // Llamada al callback con false en caso de error
        }
    });
}

function listarInsumos() {
    let sAnioTarifario = $('#cbPeriodo').val()
    let sVersion = $('#cbVersion').val();

    $.ajax({
        type: 'POST',
        url: controller + 'ListarInsumos',
        contentType: 'application/json; charset=utf-8',
        data: JSON.stringify({
            sAnioTarifario: sAnioTarifario,
            sVersion: sVersion
        }),
        dataType: 'json',
        traditional: true,
        success: function (result) {
            if (result.Resultado === "-1") {
                alert("Ocurrio un error inesperado");
            } else {
                // Llena la grilla de Insumos
                dtInsumos.clear();
                dtInsumos.rows.add(result.ListaInsumos);
                dtInsumos.draw();
            }
        },
        error: function () {
            alert("Ha ocurrido un problema...");
        }
    });
}

function obtenerDtCargaManual() {
    dtCargaManual = $('#dtCargaManual').DataTable({
        data: [],
        columns: [
            { title: 'Mes', data: 'NomMesAnio' },
            { title: 'Cargar Excel <br><input type="checkbox" id="seleccionarTodo1">', data: null },
            { title: 'Archivo <br>Excel', data: null },
            { title: 'Lectura<br>correcta', data: 'NomArchivo' },
            { title: 'Plantilla <br>Excel', data: null },
        ],
        columnDefs: [
            {
                width: "100px",
                targets: 0,
            },
            {
                width: "60px",
                targets: 1,
                defaultContent: '<input type="checkbox" class="chkSeleccion" />'
            },
            {
                width: "60px",
                targets: 2,
                createdCell: function (row, data, index) {
                    let str = `<input type="button" value="..." class="btnCarga" style="display:none;" id="btnCargar_` + index.NomMes + `" data-file-id="${data.Id}"/>`;
                    $(row).html(str);
                }
            },
            {
                width: "60px",
                targets: 3
            },
            {
                width: "60px",
                targets: 4,
                createdCell: function (row, data, index) {
                    let str = '<img src="../../../Content/Images/ExportExcel.png" class="btnPlantilla" height="15" style="margin: 3px;" id="' + data.Rerpprcodi + '"/>';
                    $(row).html(str);
                }
            }
        ],
        createdRow: function (row, data, index) {
            $(row).css('height', '30px');

            $(row).find('.btnPlantilla').on('click', function () {
                let Rerpprcodi = $(this).attr('id');
                descargarPlantilla(Rerpprcodi);
            });
        },
        searching: false,
        bLengthChange: false,
        bSort: false,
        destroy: true,
        paging: true,
        pageLength: 20,
        info: false,
        initComplete: function () {
            $('#seleccionarTodo1').on('change', function () {
                let checked = $(this).prop('checked');
                $('#dtCargaManual').find('.chkSeleccion').prop('checked', checked);

                if (checked) {
                    $('.btnCarga').show();
                } else {
                    $('.btnCarga').hide();
                }
            });

            $('#dtCargaManual').on('change', '.chkSeleccion', function () {
                if ($('.chkSeleccion:checked').length === $('.chkSeleccion').length) {
                    $('#seleccionarTodo1').prop('checked', true);
                } else {
                    $('#seleccionarTodo1').prop('checked', false);
                }
            });
        }
    });
}

function descargarPlantilla(iRerPprCodi) {
    let sTipoInsumo = $('#tipoInsumo').val();
    let sVersion = $('#cbVersion').val();

    $.ajax({
        type: 'POST',
        url: controller + 'ExportarPlantillaCargaManualInsumos',
        contentType: 'application/json; charset=utf-8',
        data: JSON.stringify({
            iRerPprCodi: iRerPprCodi,
            sVersion: sVersion,
            sTipoInsumo: sTipoInsumo
        }),
        dataType: 'json',
        traditional: true,
        success: function (result) {
            if (result.Resultado === "-1") {
                alert("Ha ocurrio un error: " + result.Mensaje);
            } else {
                window.location = controller + 'abrirarchivo?tipo=' + 1 + '&nombreArchivo=' + result.Resultado;
            }

        },
        error: function () {
            alert("Lo sentimos, se ha producido un error");
        }
    });
}

function obtenerLog(periodo, version, opcion) {
    $.ajax({
        type: 'POST',
        url: controller + 'ObtenerLog',
        contentType: 'application/json; charset=utf-8',
        data: JSON.stringify({
            idPeriodo: periodo,
            idVersion: version,
            opcion: opcion
        }),
        datatype: 'json',
        traditional: true,
        success: function (result) {
            if (result.Resultado === "-1") {
                alert("Ocurrio un error inesperado");

            } else {
                $("#popMensajeLog").html(result.Mensaje);

                $('.popup-title > span').empty();
                $('.popup-title > span').text('Última transacción: ' + result.Version);
            }
        },
        error: function () {
            alert("Ha ocurrido un problema...");
        }
    });
}

function listarCargaManual(periodo, version, opcion) {
    $.ajax({
        type: 'POST',
        url: controller + 'ListarMesesAnioTarifario',
        contentType: 'application/json; charset=utf-8',
        data: JSON.stringify({
            idPeriodo: periodo,
            idVersion: version,
            opcion: opcion
        }),
        datatype: 'json',
        traditional: true,
        success: function (result) {
            if (result.Resultado === "-1") {
                alert("Ocurrio un error inesperado");

            } else if (result.Resultado === "-2") {
                // Mensaje de un Año tarifario y Version con su estado Cerrado
                alert(result.MensajeError);
            }
            else {
                // Cambiar el titulo del popup
                $('.popup-title > span').empty();
                $('.popup-title > span').text('Carga Manual: ' + result.Version);

                // Establecer el checkbox como desmarcado al cargar la página
                $('#seleccionarTodo1').prop('checked', false);

                // Llena la grilla de CargaManual
                dtCargaManual.clear();
                dtCargaManual.rows.add(result.ListarMesesAnioTarifario);
                dtCargaManual.draw();

                document.getElementById("tipoInsumo").value = result.ListarMesesAnioTarifario[0].TipoInsumo;

                eliminaraArchivosGuardadosPreviamente("-1");

                const meses = ['Enero', 'Febrero', 'Marzo', 'Abril', 'Mayo', 'Junio', 'Julio', 'Agosto', 'Septiembre', 'Octubre', 'Noviembre', 'Diciembre'];
                const ids = ['btnCargar_Enero', 'btnCargar_Febrero', 'btnCargar_Marzo', 'btnCargar_Abril','btnCargar_Mayo', 'btnCargar_Junio', 'btnCargar_Julio', 'btnCargar_Agosto', 'btnCargar_Septiembre', 'btnCargar_Octubre', 'btnCargar_Noviembre', 'btnCargar_Diciembre'];

                for (let i = 0; i < meses.length; i++) {
                    let uploadFunction = createUploader((i + 1).toString(), ids[i]);
                    window[`uploadExcel_${meses[i]}`] = uploadFunction;
                }
            }
        },
        error: function () {
            alert("Ha ocurrido un problema...");
        }
    });
}
function eliminaraArchivosGuardadosPreviamente(sMes) {
    let iAnioTarifario = $('#cbPeriodo').val()
    let sVersion = $('#cbVersion').val();
    let sTipoInsumo = $('#tipoInsumo').val();

    $.ajax({
        type: 'POST',
        url: controller + 'EliminaraArchivosGuardadosPreviamente',
        contentType: 'application/json; charset=utf-8',
        data: JSON.stringify({ iAnioTarifario: iAnioTarifario, sMes: sMes, sVersion: sVersion, sTipoInsumo: sTipoInsumo}),
        dataType: 'json',
        traditional: true,
        cache: false,
        success: function (result) {
        },
        error: function () {
            mostrarError('Lo sentimos no se puede mostrar la hoja de cálculo')
        }
    });
}

function createUploader(mes, btnId) {
    let iAnioTarifario = $('#cbPeriodo').val();
    let sVersion = $('#cbVersion').val();
    let sTipoInsumo = $('#tipoInsumo').val();

    let uploader = new plupload.Uploader({
        runtimes: 'html5,flash,silverlight,html4',
        browse_button: btnId,
        url: `${controller}UploadExcel?sMes=${mes.padStart(2, '0')}&iAnioTarifario=${iAnioTarifario}&sVersion=${sVersion}&sTipoInsumo=${sTipoInsumo}`,
        flash_swf_url: 'Scripts/Moxie.swf',
        silverlight_xap_url: 'Scripts/Moxie.xap',
        multi_selection: false,
        filters: {
            max_file_size: '100mb',
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
                mostrarAlerta(`Por favor espere ...(${file.percent}%)`);
            },
            UploadComplete: function (up, file) {
                mostrarAlerta("Subida completada, procesando el archivo...");
                procesarArchivo(mes.padStart(2, '0'), sTipoInsumo);
            },
            Error: function (up, err) {
                mostrarError(`${err.code}-${err.message}`);
            }
        }
    });
    uploader.init();

    return uploader;
}

function procesarArchivosInsumo() {
    let iAnioTarifario = $('#cbPeriodo').val()
    let sVersion = $('#cbVersion').val()
    let sTipoInsumo = $('#tipoInsumo').val();
    let iMeses = obtenerMesesSeleccionados(dtCargaManual);
    if (confirm("¿Desea procesar los archivos de los meses seleccionados?")) {
        $.ajax({
            type: 'POST',
            url: controller + 'ProcesarArchivosInsumo',
            contentType: 'application/json; charset=utf-8',
            data: JSON.stringify({ iAnioTarifario: iAnioTarifario, sVersion: sVersion, sTipoInsumo: sTipoInsumo, iMeses: iMeses }),
            dataType: 'json',
            traditional: true,
            cache: false,
            success: function (result) {
                if (result.Resultado > 0) {
                    alert(result.Mensaje);
                    $('#pop-carga-manual').bPopup().close();
                    listarInsumos();
                }
                else {
                    alert(result.Mensaje);
                }
            },
            error: function () {
                mostrarError('Lo sentimos no se puede mostrar la hoja de cálculo')
            }
        });
    }
}

function obtenerMesesSeleccionados(dt) {
    let mesesSeleccionados = [];
    let filas = dt.rows().nodes(); 

    for (i = 0; i < filas.length; i++) {
        let fila = filas[i];
        let checkbox = $(fila).find('.chkSeleccion'); 
        if (checkbox.is(':checked')) {
            mesesSeleccionados.push(((i+4)%12)+1); 
        }
    }

    return mesesSeleccionados;
}

mostrarAlerta = function (msg) {
    $('#mensaje').removeClass();
    $('#mensaje').html(msg);
    $('#mensaje').addClass("action-alert");
};

mostrarError = function (msg) {
    $('#mensaje').removeClass();
    $('#mensaje').html(msg);
    $('#mensaje').addClass("action-error");
};

mostrarExito = function (mensaje) {
    $('#mensaje').removeClass();
    $('#mensaje').html(mensaje);
    $('#mensaje').addClass('action-exito');
}

procesarArchivo = function (sNumMes, sTipoInsumo) {
    let sAnioTarifario = $('#cbPeriodo').val()
    let sVersion = $('#cbVersion').val()

    $.ajax({
        type: 'POST',
        url: controller + 'ProcesarArchivo',
        data: { sAnioTarifario: sAnioTarifario, sVersion: sVersion, sNumMes: sNumMes, sTipoInsumo: sTipoInsumo },
        dataType: 'json',
        cache: false,
        success: function (result) {
            if (result.Resultado > 0) {
                alert(result.Mensaje)
                agregarImagenExitosa(dtCargaManual, (result.MesActual+7)%12, 3);
            }
            else {
                alert(result.Mensaje);
                eliminarImagen(dtCargaManual, (result.MesActual + 7) % 12, 3);
                eliminaraArchivosGuardadosPreviamente(sNumMes);
            }
        },
        error: function () {
            mostrarError('Lo sentimos no se puede mostrar la hoja de cálculo')
        }
    });
}

function agregarImagenExitosa(dataTable, rowIndex, columnIndex) {
    // Obtener la fila correspondiente en el DataTable
    let row = dataTable.row(rowIndex).node();

    // Agregar la imagen de carga exitosa en la celda especificada
    let str = '<img src="../../../Content/Images/btn-ok.png" alt="Carga Exitosa" />';
    $(row).find('td:eq(' + columnIndex + ')').html(str);
}

function eliminarImagen(dataTable, rowIndex, columnIndex) {
    // Obtener la fila correspondiente en el DataTable
    let row = dataTable.row(rowIndex).node();

    // Eliminar el contenido de la celda especificada
    $(row).find('td:eq(' + columnIndex + ')').empty();
}

function obtenerDtExportar() {
    dtExportar = $('#dtExportar').DataTable({
        data: [],
        columns: [
            { title: 'Mes', data: 'NomMesAnio' },
            { title: 'Descargar <br><input type="checkbox" id="seleccionarTodo2">', data: null },
        ],
        columnDefs: [
            {
                targets: 1,
                defaultContent: '<input type="checkbox" class="chkSeleccion" />'
            },
        ],
        searching: false,
        bLengthChange: false,
        bSort: false,
        destroy: true,
        paging: true,
        pageLength: 20,
        info: false,
        initComplete: function () {
            $('#seleccionarTodo2').on('change', function () {
                let checked = $(this).prop('checked');
                $('#dtExportar').find('.chkSeleccion').prop('checked', checked);
            });

            $('#dtExportar').on('change', '.chkSeleccion', function () {
                if ($('.chkSeleccion:checked').length === $('.chkSeleccion').length) {
                    $('#seleccionarTodo2').prop('checked', true);
                } else {
                    $('#seleccionarTodo2').prop('checked', false);
                }
            });
        }

    });
}

function listarExportar(periodo, version, opcion) {
    $.ajax({
        type: 'POST',
        url: controller + 'ListarMesesAnioTarifario',
        contentType: 'application/json; charset=utf-8',
        data: JSON.stringify({
            idPeriodo: periodo,
            idVersion: version,
            opcion: opcion
        }),
        datatype: 'json',
        traditional: true,
        success: function (result) {
            if (result.Resultado === "-1") {
                alert("Ocurrio un error inesperado");
            } else {
                // Cambiar el titulo del popup
                $('.popup-title > span').empty();
                $('.popup-title > span').text('Exportar: ' + result.Version);

                // Establecer el checkbox como desmarcado al cargar la página
                $('#seleccionarTodo2').prop('checked', false);

                // Llena la grilla de Exportar
                dtExportar.clear();
                dtExportar.rows.add(result.ListarMesesAnioTarifario);
                dtExportar.draw();

                document.getElementById("tipoInsumo").value = result.ListarMesesAnioTarifario[0].TipoInsumo;
            }
        },
        error: function () {
            alert("Ha ocurrido un problema...");
        }
    });
}

function ProcesarArchivosSddp() {
    $.ajax({
        type: 'POST',
        url: controller + 'ProcesarArchivosSddp',
        contentType: 'application/json; charset=utf-8',
        data: JSON.stringify({
            idPeriodo: $('#cbPeriodo').val(),
            idVersion: $('#cbVersion').val(),
            pathDirectorio: $('#txtDireccion').val()
        }),
        datatype: 'json',
        traditional: true,
        success: function (result) {
            if (result.Resultado === "1") {
                alert("Se procesaron correctamente los archivos SDDP: gergnd.csv, gerhid.csv, gerter.csv, duraci.csv  y sddp.dat");
            } else {
                alert(result.Mensaje);
            }
        },
        error: function () {
            alert("Ha ocurrido un problema...");
        }
    });
}

function DescargarArchivoSDDP() {
    $.ajax({
        type: 'POST',
        url: controller + 'DescargarArchivoSDDP',
        contentType: 'application/json; charset=utf-8',
        data: JSON.stringify({
            sAnioTarifario: $('#cbPeriodo').val(),
            sVersion: $('#cbVersion').val(),
        }),
        datatype: 'json',
        traditional: true,
        success: function (result) {
            if (result.Resultado === "-1") {
                alert("Ha ocurrio un error: " + result.Mensaje);
            } else {
                window.location = controller + 'abrirarchivo?tipo=' + 1 + '&nombreArchivo=' + result.Resultado;
            }
        },
        error: function () {
            alert("Ha ocurrido un problema...");
        }
    });
}

//----------------------------------------------------------------------------------

function importarTotalInsumos(idPeriodo, idVersion, opcion) {
    $.ajax({
        type: 'POST',
        url: controller + 'ImportarTotalInsumos',
        contentType: 'application/json; charset=utf-8',
        data: JSON.stringify({
            idPeriodo: idPeriodo,
            idVersion: idVersion,
            opcion: opcion
        }),
        datatype: 'json',
        traditional: true,
        success: function (result) {
            if (result.Resultado === "1") {
                alert(result.Mensaje);
                listarInsumos();
            }
            else if (result.Resultado === "-2") {
                // Mensaje de un Año tarifario y Version con su estado Cerrado
                alert(result.MensajeError);
            }
            else {
                alert(result.Mensaje);
            }
        },
        error: function () {
            alert("Ha ocurrido un problema...");
        }
    });
}

function ejecutarExportarInsumos()
{
    let anio = $('#cbPeriodo').val()
    let version = $('#cbVersion').val()
    let tipoInsumo = $('#tipoInsumo').val();
    let meses = obtenerMesesSeleccionados(dtExportar);

    if (meses.length == 0) {
        alert('Debe seleccionar al menos un mes');
        return false;
    }

    exportarInsumos(anio, version, meses, tipoInsumo);
}

function exportarInsumos(anio, version,  meses, tipoInsumo) {
    $.ajax({
        type: 'POST',
        url: controller + 'ExportarTotalInsumos',
        contentType: 'application/json; charset=utf-8',
        data: JSON.stringify({
            anio: anio,
            version: version,
            meses: meses,
            tipoInsumo: tipoInsumo
        }),
        datatype: 'json',
        traditional: true,
        success: function (result) {
            if (result.Resultado === "-1") {
                alert("Ha ocurrio un error: " + result.Mensaje);
            } else {
                window.location = controller + 'abrirarchivo?tipo=' + 1 + '&nombreArchivo=' + result.Resultado;
                $('#pop-exportar').bPopup().close();
            }
        },
        error: function () {
            alert("Ha ocurrido un problema...");
        }
    });
}

function cargarPopup() {
    $.ajax({
        type: 'POST',
        url: controller + 'CargarTotalInsumos',
        contentType: 'application/json; charset=utf-8',
        data: JSON.stringify({
            idPeriodo: $('#cbPeriodo').val(),
            idVersion: $('#cbVersion').val()
        }),
        datatype: 'json',
        traditional: true,
        success: function (result) {
            if (result.Resultado === "-1") {
                alert("Ocurrio un error inesperado");
            } else {
                alert(result.Mensaje);
            }
        },
        error: function () {
            alert("Ha ocurrido un problema...");
        }
    });
}

function descargarPopup() {
    $.ajax({
        type: 'POST',
        url: controller + 'DescargarTotalInsumos',
        contentType: 'application/json; charset=utf-8',
        data: JSON.stringify({
            idPeriodo: $('#cbPeriodo').val(),
            idVersion: $('#cbVersion').val()
        }),
        datatype: 'json',
        traditional: true,
        success: function (result) {
            if (result.Resultado === "-1") {
                alert("Ocurrio un error inesperado");
            } else {
                alert(result.Mensaje);
            }
        },
        error: function () {
            alert("Ha ocurrido un problema...");
        }
    });
}

function validarDatos1(ruta) {
    let data = {
        'Ruta': $(ruta).val() || null,
        'Error': false,
        'ErrorMessage': ''
    }

    if (!data.Ruta) {
        data.Error = true;
        data.ErrorMessage = 'No ha colocado la ruta del directorio';
        return data;
    }

    return data;
}

function validarDatos2(idCbAnioTarifario, idCbVersion) {

    let data = {
        'AnioTarifario': parseInt($(idCbAnioTarifario).val()) || 0,
        'NumeroVersion': $(idCbVersion).val(),
        'Error': false,
        'ErrorMessage': ''
    }

    if (!data.AnioTarifario) {
        data.Error = true;
        data.ErrorMessage = 'Debe seleccionar un año tarifario';
        return data;
    }

    if (!data.NumeroVersion) {
        data.Error = true;
        data.ErrorMessage = 'Debe seleccionar una versión';
        return data;
    }


    return data;
}

function validarDatos3(idCbAnioTarifario, idCbVersion, ruta) {

    let data = {
        'AnioTarifario': parseInt($(idCbAnioTarifario).val()) || 0,
        'NumeroVersion': $(idCbVersion).val(),
        'Ruta': $(ruta).val() || null,
        'Error': false,
        'ErrorMessage': ''
    }

    if (!data.AnioTarifario) {
        data.Error = true;
        data.ErrorMessage = 'Debe seleccionar un año tarifario';
        return data;
    }

    if (!data.NumeroVersion) {
        data.Error = true;
        data.ErrorMessage = 'Debe seleccionar una versión';
        return data;
    }

    if (!data.Ruta) {
        data.Error = true;
        data.ErrorMessage = 'No ha colocado la ruta del directorio';
        return data;
    }

    return data;
}