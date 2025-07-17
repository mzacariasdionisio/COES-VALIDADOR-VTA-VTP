var controller = siteRoot + "CPPA/Insumos/";
const imageRoot = siteRoot + "Content/Images/";

// Variables para las grillas
var dtInsumos, dtLogs, dtCargaManual, dtExportar

$(document).ready(function () {
    $('#tab-container').easytabs({
        animate: false
    });
    // Llamada al metodo que inicializa la grilla de Insumos
    obtenerDtInsumos();

    var $anioDropdown = $('#anioDropdown');
    var $ajusteDropdown = $('#ajusteDropdown');
    var $revisionDropdown = $('#revisionDropdown');

    // Desactivar los selects al inicio
    $ajusteDropdown.prop('disabled', true);
    $revisionDropdown.prop('disabled', true);

    $('#anioDropdown').change(function () {
        var selectedYear = $(this).val();

        if (selectedYear) {
            // Cargar ajustes para el año seleccionado
            cargarAjustes(selectedYear);
        } else {
            // Si no hay un año seleccionado, deshabilitar los otros select
            $('#ajusteDropdown').prop('disabled', true).empty().append('<option value="">--Seleccione--</option>');
            $('#revisionDropdown').prop('disabled', true).empty().append('<option value="">--Seleccione--</option>');
        }
        resetearValores();
    });

    $('#ajusteDropdown').change(function () {
        var selectedYear = $('#anioDropdown').val();
        var selectedAdjustment = $(this).val();

        if (selectedYear && selectedAdjustment) {
            // Cargar revisiones para el ajuste seleccionado
            cargarRevisiones(selectedYear, selectedAdjustment);
        } else {
            $('#revisionDropdown').prop('disabled', true).empty().append('<option value="">--Seleccione--</option>');
        }
        resetearValores();
    });

    $('#revisionDropdown').change(function () {
        if ($('#revisionDropdown').val()){
            listarInsumos();
        } else {
            resetearValores();
        }
    });

    // --------------------------------------------------
    $('#btnProcesar').click(function () {
        let ruta = $('#txtDireccion').val();
        if (!ruta) {
            alert('No coloco la ruta donde se ubica los archivos: sddp.dat / gergnd.csv / gerhid.csv');
            return false;
        }
        if ($('#revisionDropdown').val()) {
            ProcesarArchivosSddp();
        } else {
            alert('Por favor, primero seleccionar una revisión.');
            return false;
        }

    });

    $('#btnDescargar').click(function () {
        if ($('#revisionDropdown').val()) {
            DescargarArchivoSDDP();
        } else {
            alert('Por favor, primero seleccionar una revisión.');
            return false;
        }
    });

    // ---------------------------------------------------------------------------------------
    // Botones Aceptar y Cancelar del popup del log
    $('#pop-cancelar-l').click(function () {
        $('#pop-log').bPopup().close();
    });

    // Botones Aceptar y Cancelar del popup de Cargas Manuales
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

    obtenerDtLogs();
    obtenerDtCargaManual();
    obtenerDtExportar();

    if (revisionData.length === 0) {
        mostrarErrorConMensaje('No se encontraron revisiones en la base de datos');
    }
});

function resetearValores() {
    document.getElementById('btnProcesar').style.display = 'none';
    document.getElementById('btnDescargar').style.display = 'none';
    document.getElementById('miTextArea').value = '';
    dtInsumos.clear();
    dtInsumos.draw();
}

function cargarAjustes(year) {
    $('#ajusteDropdown').prop('disabled', false).empty().append('<option value="">--Seleccione--</option>');
    $('#revisionDropdown').prop('disabled', true).empty().append('<option value="">--Seleccione--</option>');

    // Filtrar y obtener los ajustes disponibles para el año
    var ajustes = revisionData
        .filter(x => x.Cpaapanio === parseInt(year))
        .map(x => x.Cpaapajuste)
        .filter((value, index, self) => self.indexOf(value) === index); // Eliminar duplicados

    // Llenar el dropdown con los ajustes
    ajustes.forEach(function (ajuste) {
        $('#ajusteDropdown').append('<option value="' + ajuste + '">' + ajuste + '</option>');
    });
}

function cargarRevisiones(year, ajuste) {
    $('#revisionDropdown').prop('disabled', false).empty().append('<option value="">--Seleccione--</option>');

    // Filtrar y obtener las revisiones disponibles para el ajuste
    var revisiones = revisionData
            .filter(x => x.Cpaapanio === parseInt(year) && x.Cpaapajuste === ajuste);
    revisiones.forEach(function (revision) {
        $('#revisionDropdown').append('<option value="' + revision.Cparcodi + '">' + revision.Cparrevision + '</option>');
    });
}

//----------------------------------------------------------------------------------
function ProcesarArchivosSddp() {
    let sAnioPresupuestal = $('#anioDropdown').val();
    let sAjuste = $('#ajusteDropdown').val();
    let sRevision = $('#revisionDropdown').val();
    let pathDirectorio = $('#txtDireccion').val();
    if (confirm("¿Desea procesar los archivos SDDP para la revisión seleccionada?")) {
        $.ajax({
            type: 'POST',
            url: controller + 'ProcesarArchivosSddp',
            contentType: 'application/json; charset=utf-8',
            data: JSON.stringify({
                sAnioPresupuestal: sAnioPresupuestal,
                sAjuste: sAjuste,
                sRevision: sRevision,
                pathDirectorio: pathDirectorio
            }),
            datatype: 'json',
            traditional: true,
            success: function (result) {
                if (result.sResultado === "1") {
                    alert(result.sMensaje);
                    listarInsumos();
                    //document.getElementById('miTextArea').value = result.sLog;      // Muestro el ultimo log que se encontró
                } else {
                    alert(result.sMensaje);
                    listarInsumos();
                }
            },
            error: function () {
                alert("Ha ocurrido un problema...");
            }
        });
    }
}

function DescargarArchivoSDDP() {
    let sAnioPresupuestal = $('#anioDropdown').val();
    let sAjuste = $('#ajusteDropdown').val();
    let sRevision = $('#revisionDropdown').val();

    $.ajax({
        type: 'POST',
        url: controller + 'DescargarArchivoSDDP',
        contentType: 'application/json; charset=utf-8',
        data: JSON.stringify({
            sAnioPresupuestal: sAnioPresupuestal,
            sAjuste: sAjuste,
            sRevision: sRevision
        }),
        datatype: 'json',
        traditional: true,
        success: function (result) {
            if (result.sResultado === "-1") {
                alert("Ha ocurrido un error: " + result.sMensaje);
            } else {
                window.location = controller + 'abrirarchivo?nombreArchivo=' + result.sResultado;
            }
        },
        error: function () {
            alert("Ha ocurrido un problema...");
        }
    });
}

//----------------------------------------------------------------------------------
function obtenerDtInsumos() {
    dtInsumos = $('#dtInsumos').DataTable({
        data: [],
        columns: [
            { title: 'Insumo', data: 'Cpainsdescinsumo' },
            { title: 'Fecha y usuario de última importación', data: 'Cpainsfecusuario' },
            {
                title: 'Procesamiento',
                data: 'Cpainstipproceso',
                render: function (data, type, row) {
                    return data === "M" ? "Manual" : data === "A" ? "Automático" : data;
                }
            },
            { title: 'Log', data: null },
            { title: 'Importar', data: null },
            { title: 'Carga Manual', data: null },
            { title: 'Descargar', data: null },
        ],
        columnDefs: [
            {
                width: "160px",
                targets: 0,
            },
            {
                width: "50px",
                targets: 1,
            },
            {
                width: "60px",
                targets: 2,
            },
            {
                width: "10px",
                targets: 3,
                createdCell: function (cell, cellData, rowData, rowIndex, colIndex) {
                    if (rowIndex === 3) { // Ocultar el botón log del insumo Generación programada PMPO 15 min.
                        $(cell).empty(); // Eliminar el contenido de las celdas
                    } else {
                        $(cell).html(
                            '<div class="dt-col-options">' +
                            `<img src="${imageRoot}btn-open.png" class="dt-ico-log" title="Log" /> ` +
                            '</div>',
                        );
                    }
                }
            },
            {
                width: "30px",
                targets: 4,
                createdCell: function (cell, cellData, rowData, rowIndex, colIndex) {
                    var estadoRevision = $('#estadoRevision').val();
                    var exiteCentrales = $('#exiteCentrales').val();
                    var seProcesoCalculo = $('#seProcesoCalculo').val();

                    if (rowIndex === 3) { // Ocultar botón importar automaticamente del insumo Generación programada PMPO 15 min.
                        $(cell).empty(); // Eliminar el contenido siempre
                    } else if (estadoRevision === "C" || estadoRevision === "X") {
                        $(cell).empty(); // Eliminar el contenido cuando la revisión este cerrada o anulada, no existan centrales registradas o si ya se procesó cálculo
                    } else if (exiteCentrales == "N") {
                        $(cell).empty(); // Eliminar el contenido cuando no existan centrales registradas.
                    } else if (seProcesoCalculo == "S") {
                        $(cell).empty(); // Eliminar el contenido cuando ya se procesó cálculo
                    }
                    else {
                        $(cell).html(
                            '<div class="dt-col-options">' +
                            `<img src="${imageRoot}Copiado.png" class="dt-ico-importar" title="Importar" /> ` +
                            '</div>',
                        );
                    }
                }
            },
            {
                width: "140px",
                targets: 5,
                createdCell: function (cell, cellData, rowData, rowIndex, colIndex) {
                    var estadoRevision = $('#estadoRevision').val();
                    var exiteCentrales = $('#exiteCentrales').val();
                    var seProcesoCalculo = $('#seProcesoCalculo').val();

                    if (estadoRevision === "C" || estadoRevision === "X") {
                        $(cell).empty(); // Eliminar el contenido cuando la revisión este cerrada o anulada, no existan centrales registradas o si ya se procesó cálculo
                    } else if (exiteCentrales == "N") {
                        $(cell).empty(); // Eliminar el contenido cuando no existan centrales registradas.
                    } else if (seProcesoCalculo == "S") {
                        $(cell).empty(); // Eliminar el contenido cuando ya se procesó cálculo
                    } else {
                        $(cell).html(
                            '<div class="dt-col-options">' +
                            `<input type="button" value="Carga Manual" style="width:120px; height: 20px;" class="dt-ico-carga-manual" id="insumo_${rowIndex + 1}"/> ` +
                            '</div>'
                        );
                    }
                }
            },
            {
                width: "140px",
                targets: 6,
                createdCell: function (cell, cellData, rowData, rowIndex, colIndex) {
                    var exiteCentrales = $('#exiteCentrales').val();

                    if (exiteCentrales == "N") {
                        $(cell).empty();
                    } else {
                        $(cell).html(
                            '<div class="dt-col-options">' +
                            `<input type="button" value="Descargar" style="width:120px; height: 20px;" class="dt-ico-exportar"/>` +
                            '</div>'
                        );
                    }
                    
                }
            },
        ],
        createdRow: function (row, data, index) {
            $(row).find('.dt-ico-log').on('click', function () {
                let opcion = 0;
                switch (index) {
                    case 0: opcion = 1; //Medidores generacion 15 min.
                        break;
                    case 1: opcion = 2; //Costo Marginal LVTEA 15 min.
                        break;
                    case 2: opcion = 3; //Costo Marginal PMPO 15 min.
                        break;
                    case 3: opcion = 4; //Generacion programada PMPO 15 min.
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
                        // Carga grilla de Logs
                        listarLogs(opcion);
                    }
                });
            });

            $(row).find('.dt-ico-importar').on('click', function () {

                let opcion = 0;
                let msg = "";
                switch (index) {
                    case 0: opcion = 1;
                        msg = "Medidores generacion 15 min.";
                        break;
                    case 1: opcion = 2;
                        msg = "Costo Marginal LVTEA 15 min.";
                        break;
                    case 2: opcion = 3;
                        msg = "Costo Marginal PMPO 15 min.";
                        break;
                    case 3: opcion = 4;
                        msg = "Generacion programada PMPO 15 min.";
                        break;

                    default:
                        alert('Opción no disponible');
                        return false;
                }
                if (opcion > 0) {
                    if (confirm("¿Desea importar la información de " + msg + "?")) {
                        importarTotalInsumos(opcion);
                    }
                    else {
                        return false;
                    }
                }

            });

            $(row).find('.dt-ico-carga-manual').on('click', function () {
                opcion = 0;
                switch (index) {
                    case 0: opcion = 1; //Medidores generacion 15 min.
                        break;
                    case 1: opcion = 2; //Costo Marginal LVTEA 15 min.
                        break;
                    case 2: opcion = 3; //Costo Marginal PMPO 15 min.
                        break;
                    case 3: opcion = 4; //Generacion programada PMPO 15 min.
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
                        listarCargaManual(opcion);
                    }
                });
            });

            $(row).find('.dt-ico-exportar').on('click', function () {

                opcion = 0;
                switch (index) {
                    case 0: opcion = 1; //Medidores generacion 15 min.
                        break;
                    case 1: opcion = 2; //Costo Marginal LVTEA 15 min.
                        break;
                    case 2: opcion = 3; //Costo Marginal PMPO 15 min.
                        break;
                    case 3: opcion = 4; //Generacion programada PMPO 15 min.
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
                        listarExportar(opcion);
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


function listarInsumos() {
    let sAnioPresupuestal = $('#anioDropdown').val();
    let sAjuste = $('#ajusteDropdown').val();
    let sRevision = $('#revisionDropdown').val();

    $.ajax({
        type: 'POST',
        url: controller + 'ListInsumos',
        contentType: 'application/json; charset=utf-8',
        data: JSON.stringify({
            sAnioPresupuestal: sAnioPresupuestal,
            sAjuste: sAjuste,
            sRevision: sRevision
        }),
        dataType: 'json',
        traditional: true,
        success: function (result) {
            if (result.sResultado === "-1") {
                mostrarErrorConMensaje(result.sMensaje);
            } else {
                document.getElementById("estadoRevision").value = result.sEstadoRevision;
                document.getElementById("exiteCentrales").value = result.sExiteCentrales;
                document.getElementById("seProcesoCalculo").value = result.sSeProcesoCalculo;

                // Llena la grilla de Insumos, el insumo 1 al 4 se muestran en la vista, 
                // El insumo 5 es el resultado de la última importación en la pestaña SDDP (con estado automático)

                dtInsumos.clear();
                dtInsumos.rows.add(result.ListInsumo.slice(0, 4));
                dtInsumos.draw();
                removerTodoAlerta();
                //mostrarExitoOperacion();

                if (result.ListInsumo.length > 4 && result.ListInsumo[4].Cpainslog) {
                    let cpainslog = result.ListInsumo[4].Cpainslog.replace(/<br\s*\/?>/gi, '\n');
                    document.getElementById('miTextArea').value = cpainslog;
                } else {
                    document.getElementById('miTextArea').value = "No hay información de log disponible.";
                }

                var estadoRevision = $('#estadoRevision').val();
                if (result.sEstadoRevision == 'C' || result.sEstadoRevision == 'X') {
                    document.getElementById('btnProcesar').style.display = 'none';
                    //    document.getElementById('btnDescargar').style.display = 'none';
                    document.getElementById('btnDescargar').style.display = 'inline';
                } else {
                    document.getElementById('btnProcesar').style.display = 'inline';
                    document.getElementById('btnDescargar').style.display = 'inline';
                }
                if (result.sEstadoRevision == 'C') {
                    mostrarAlertaConMensaje('La revisión del ajuste seleccionada está en estado "Cerrada", por lo que no es posible importar ni cargar insumos.');
                }
                if (result.sExiteCentrales == "N") {
                    document.getElementById('btnProcesar').style.display = 'none';
                    document.getElementById('btnDescargar').style.display = 'none';
                    mostrarAlertaConMensaje('No es posible importar, cargar ni descargar insumos debido a que no existen centrales de generación registradas para la revisión seleccionada.');
                }
                if (result.sSeProcesoCalculo === "S") {
                    document.getElementById('btnProcesar').style.display = 'none';
                    //mostrarAlertaConMensaje('No es posible importar, cargar ni descargar insumos debido a que no existen centrales de generación registradas para la revisión seleccionada.');
                }
            }
        },
        error: function () {
            alert("Ha ocurrido un problema...");
        }
    });
}

function listarLogs(sOpcion) {
    let sAnioPresupuestal = $('#anioDropdown').val();
    let sAjuste = $('#ajusteDropdown').val();
    let sRevision = $('#revisionDropdown').val();

    $.ajax({
        type: 'POST',
        url: controller + 'ListarLogs',
        contentType: 'application/json; charset=utf-8',
        data: JSON.stringify({
            sAnioPresupuestal: sAnioPresupuestal,
            sAjuste: sAjuste,
            sRevision: sRevision,
            sOpcion: sOpcion
        }),
        datatype: 'json',
        traditional: true,
        success: function (result) {
            if (result.Resultado === "-1") {
                mostrarErrorConMensaje(result.sMensaje);
            } else {
                $("#popMensajeLog").html(result.Mensaje);

                $('.popup-title > span').empty();
                $('.popup-title > span').text('Lista de logs: ' + result.NombInsumo);

                // Llena la grilla de Logs
                dtLogs.clear();
                dtLogs.rows.add(result.ListInsumo);
                dtLogs.draw();
            }
        },
        error: function () {
            alert("Ha ocurrido un problema...");
        }
    });
}

function obtenerDtLogs() {
    dtLogs = $('#dtLogs').DataTable({
        data: [],
        columns: [
            {
                title: 'Nro',
                data: null, // No necesitas un campo de datos porque los números se generan dinámicamente
                render: function (data, type, row, meta) {
                    return meta.row + 1; // meta.row es el índice de la fila, comenzando desde 0
                }
            },
            { title: 'Usuario', data: 'Cpainsusucreacion' },
            { title: 'Fecha / Hora', data: 'Cpainsfecusuario' },
            {
                title: 'Procesamiento',
                data: 'Cpainstipproceso',
                render: function (data, type, row) {
                    return data === "M" ? "Manual" : data === "A" ? "Automático" : data;
                }
            },
            { title: 'Resultado de la importación', data: 'Cpainslog' },
        ],
        columnDefs: [
            {
                width: "5px",
                targets: 0,
            },
            {
                width: "60px",
                targets: 1,
            },
            {
                width: "120px",
                targets: 2
            },
            {
                width: "60px",
                targets: 3
            },
            {
                width: "200px",
                targets: 4,
                createdCell: function (td, cellData, rowData, row, col) {
                    $(td).css('text-align', 'left'); // Alinea el contenido de la celda a la izquierda
                }
            }
        ],
        createdRow: function (row, data, index) {
            $(row).css('height', '30px');
        },
        searching: false,
        bLengthChange: false,
        bSort: false,
        destroy: true,
        paging: true,
        pageLength: 2,
        info: false
    });
}

function importarTotalInsumos(sOpcion) {
    let sAnioPresupuestal = $('#anioDropdown').val();
    let sAjuste = $('#ajusteDropdown').val();
    let sRevision = $('#revisionDropdown').val();

    $.ajax({
        type: 'POST',
        url: controller + 'ImportarTotalInsumos',
        contentType: 'application/json; charset=utf-8',
        data: JSON.stringify({
            sAnioPresupuestal: sAnioPresupuestal,
            sAjuste: sAjuste,
            sRevision: sRevision,
            sOpcion: sOpcion
        }),
        datatype: 'json',
        traditional: true,
        success: function (result) {
            if (result.sResultado === "1") {
                alert(result.sMensaje);
                listarInsumos();
            }
            else if (result.sResultado === "-2") {
                // Mensaje de un Año tarifario y Version con su estado Cerrado
                alert(result.sMensaje);
            }
            else {
                alert(result.sMensaje);
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
                    let str = '<img src="../../../Content/Images/ExportExcel.png" class="btnPlantilla" height="15" style="margin: 3px;" id="' + data.Id + '"/>';
                    $(row).html(str);
                }
            }
        ],
        createdRow: function (row, data, index) {
            $(row).css('height', '30px');

            $(row).find('.btnPlantilla').on('click', function () {
                let sMes = $(this).attr('id');
                descargarPlantilla(sMes);
            });
        },
        searching: false,
        bLengthChange: false,
        bSort: false,
        destroy: true,
        paging: false,
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

function listarCargaManual(sOpcion) {
    let sAnioPresupuestal = $('#anioDropdown').val();
    let sAjuste = $('#ajusteDropdown').val();
    let sRevision = $('#revisionDropdown').val();

    $.ajax({
        type: 'POST',
        url: controller + 'ListarMeses',
        contentType: 'application/json; charset=utf-8',
        data: JSON.stringify({
            sAnioPresupuestal: sAnioPresupuestal,
            sAjuste: sAjuste,
            sRevision: sRevision,
            sOpcion: sOpcion
        }),
        datatype: 'json',
        traditional: true,
        success: function (result) {
            if (result.sResultado === "-1") {
                $('#pop-carga-manual').bPopup().close();
                mostrarErrorConMensaje(result.sMensaje);
            }
            else {
                // Cambiar el titulo del popup
                $('.popup-title > span').empty();
                $('.popup-title > span').text('Carga Manual: ' + result.NombInsumo);

                // Establecer el checkbox como desmarcado al cargar la página
                $('#seleccionarTodo1').prop('checked', false);

                // Llena la grilla de CargaManual
                dtCargaManual.clear();
                dtCargaManual.rows.add(result.ListarMesesAnioPresupuestal);
                dtCargaManual.draw();

                document.getElementById("tipoInsumo").value = result.ListarMesesAnioPresupuestal[0].TipoInsumo;

                eliminaraArchivosGuardadosPreviamente("-1");

                const meses = ['Enero', 'Febrero', 'Marzo', 'Abril', 'Mayo', 'Junio', 'Julio', 'Agosto', 'Setiembre', 'Octubre', 'Noviembre', 'Diciembre'];
                const ids = ['btnCargar_Enero', 'btnCargar_Febrero', 'btnCargar_Marzo', 'btnCargar_Abril', 'btnCargar_Mayo', 'btnCargar_Junio', 'btnCargar_Julio', 'btnCargar_Agosto', 'btnCargar_Setiembre', 'btnCargar_Octubre', 'btnCargar_Noviembre', 'btnCargar_Diciembre'];

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

function descargarPlantilla(sMes) {
    let sAnioPresupuestal = $('#anioDropdown').val();
    let sAjuste = $('#ajusteDropdown').val();
    let sRevision = $('#revisionDropdown').val();
    let sOpcion = $('#tipoInsumo').val(); 
    $.ajax({
        type: 'POST',
        url: controller + 'ExportarPlantillaCargaManualInsumos',
        contentType: 'application/json; charset=utf-8',
        data: JSON.stringify({
            sMes: sMes,
            sAnioPresupuestal: sAnioPresupuestal,
            sAjuste: sAjuste,
            sRevision: sRevision,
            sOpcion: sOpcion
        }),
        dataType: 'json',
        traditional: true,
        success: function (result) {
            if (result.sResultado === "-1") {
                alert("Ha ocurrido un error: " + result.sMensaje);
            } else {
                window.location = controller + 'abrirarchivo?nombreArchivo=' + result.sResultado;
            }

        },
        error: function () {
            alert("Lo sentimos, se ha producido un error");
        }
    });
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
        paging: false,
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

function listarExportar(sOpcion) {
    let sAnioPresupuestal = $('#anioDropdown').val();
    let sAjuste = $('#ajusteDropdown').val();
    let sRevision = $('#revisionDropdown').val();

    $.ajax({
        type: 'POST',
        url: controller + 'ListarMeses',
        contentType: 'application/json; charset=utf-8',
        data: JSON.stringify({
            sAnioPresupuestal: sAnioPresupuestal,
            sAjuste: sAjuste,
            sRevision: sRevision,
            sOpcion: sOpcion
        }),
        datatype: 'json',
        traditional: true,
        success: function (result) {
            if (result.sResultado === "-1") {
                $('#pop-exportar').bPopup().close();
                mostrarErrorConMensaje(result.sMensaje);
            } else {
                // Cambiar el titulo del popup
                $('.popup-title > span').empty();
                $('.popup-title > span').text('Descargar: ' + result.NombInsumo);

                // Establecer el checkbox como desmarcado al cargar la página
                $('#seleccionarTodo2').prop('checked', false);

                // Llena la grilla de Exportar
                dtExportar.clear();
                dtExportar.rows.add(result.ListarMesesAnioPresupuestal);
                dtExportar.draw();

                document.getElementById("tipoInsumo").value = result.ListarMesesAnioPresupuestal[0].TipoInsumo;
            }
        },
        error: function () {
            alert("Ha ocurrido un problema...");
        }
    });
}

function ejecutarExportarInsumos() {
    let idRevision = $('#revisionDropdown').val();
    let sTipoInsumo = $('#tipoInsumo').val();
    let iMeses = obtenerMesesSeleccionados(dtExportar);

    if (iMeses.length == 0) {
        alert('Debe seleccionar al menos un mes');
        return false;
    }

    exportarInsumos(idRevision, sTipoInsumo, iMeses);
}

function exportarInsumos(idRevision, sTipoInsumo, iMeses) {
    $.ajax({
        type: 'POST',
        url: controller + 'ExportarTotalInsumos',
        contentType: 'application/json; charset=utf-8',
        data: JSON.stringify({
            idRevision: idRevision,
            sTipoInsumo: sTipoInsumo,
            iMeses: iMeses
        }),
        datatype: 'json',
        traditional: true,
        success: function (result) {
            if (result.sResultado === "-1") {
                alert("Ha ocurrido un error: " + result.sMensaje);
            } else {
                window.location = controller + 'abrirarchivo?nombreArchivo=' + result.sResultado;
                $('#pop-exportar').bPopup().close();
            }
        },
        error: function () {
            alert("Ha ocurrido un problema...");
        }
    });
}

function eliminaraArchivosGuardadosPreviamente(sMes) {

    let sAnioPresupuestal = $('#anioDropdown').val();
    let sAjuste = $('#ajusteDropdown').val();
    let sRevision = $('#revisionDropdown').val();
    let sTipoInsumo = $('#tipoInsumo').val();

    $.ajax({
        type: 'POST',
        url: controller + 'EliminaraArchivosGuardadosPreviamente',
        contentType: 'application/json; charset=utf-8',
        data: JSON.stringify({ sAnioPresupuestal: sAnioPresupuestal, sMes: sMes, sAjuste: sAjuste, sRevision: sRevision, sTipoInsumo: sTipoInsumo }),
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

    let sAnioPresupuestal = $('#anioDropdown').val();
    let sAjuste = $('#ajusteDropdown').val();
    let sRevision = $('#revisionDropdown').val();
    let sTipoInsumo = $('#tipoInsumo').val();

    let uploader = new plupload.Uploader({
        runtimes: 'html5,flash,silverlight,html4',
        browse_button: btnId,
        url: `${controller}UploadExcel?sMes=${mes.padStart(2, '0')}&sAnioPresupuestal=${sAnioPresupuestal}&sAjuste=${sAjuste}&sRevision=${sRevision}&sTipoInsumo=${sTipoInsumo}`,
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

procesarArchivo = function (sNumMes, sTipoInsumo) {
    let sAnioPresupuestal = $('#anioDropdown').val();
    let sAjuste = $('#ajusteDropdown').val();
    let sRevision = $('#revisionDropdown').val();

    $.ajax({
        type: 'POST',
        url: controller + 'ProcesarArchivo',
        data: { sAnioPresupuestal: sAnioPresupuestal, sAjuste: sAjuste, sRevision: sRevision, sNumMes: sNumMes, sTipoInsumo: sTipoInsumo },
        dataType: 'json',
        cache: false,
        success: function (result) {
            if (result.sResultado > 0) {
                removerAlerta();
                alert(result.sMensaje);
                agregarImagenExitosa(dtCargaManual, (result.iNumMes - 1), 3);
            }
            else {
                removerAlerta();
                alert(result.sMensaje);
                eliminarImagen(dtCargaManual, (result.iNumMes-1), 3);
                eliminaraArchivosGuardadosPreviamente(sNumMes);
            }
        },
        error: function () {
            mostrarError('Lo sentimos no se puede mostrar la hoja de cálculo')
        }
    });
}

function procesarArchivosInsumo() {

    let sAnioPresupuestal = $('#anioDropdown').val();
    let sAjuste = $('#ajusteDropdown').val();
    let sRevision = $('#revisionDropdown').val();
    let sTipoInsumo = $('#tipoInsumo').val();
    let iMeses = obtenerMesesSeleccionados(dtCargaManual);

    if (iMeses.length == 0) {
        alert('Debe seleccionar al menos un mes');
        return false;
    }

    if (confirm("¿Desea importar a la base de datos los archivos Excel de los meses seleccionados?")) {
        $.ajax({
            type: 'POST',
            url: controller + 'ProcesarArchivosInsumo',
            contentType: 'application/json; charset=utf-8',
            data: JSON.stringify({ sAnioPresupuestal: sAnioPresupuestal, sAjuste: sAjuste, sRevision: sRevision, sTipoInsumo: sTipoInsumo, iMeses: iMeses }),
            dataType: 'json',
            traditional: true,
            cache: false,
            success: function (result) {
                if (result.sResultado > 0) {
                    alert(result.sMensaje);
                    $('#pop-carga-manual').bPopup().close();
                    listarInsumos();
                }
                else {
                    alert(result.sMensaje);
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
            mesesSeleccionados.push(i + 1);
        }
    }

    return mesesSeleccionados;
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

mostrarExitoOperacion = function () {
    $('#mensaje').removeClass();
    $('#mensaje').addClass("action-exito");
    $('#mensaje').text("La operación se realizó con exito...!");
    $('#mensaje').css("display", "block");
};

mostrarErrorConMensaje = function (msg) {
    $('#mensaje').removeClass();
    $('#mensaje').html(msg);
    $('#mensaje').addClass("action-error");
};
mostrarAlertaConMensaje = function (msg) {
    $('#mensaje').removeClass();
    $('#mensaje').html(msg);
    $('#mensaje').addClass("action-alert");
};
mostrarAlerta = function (msg) {
    $('#mensaje').removeClass();
    $('#mensaje').html(msg);
    $('#mensaje').addClass("action-alert");
};
removerAlerta = function () {
    $('#mensaje').removeClass("action-alert");
    $('#mensaje').addClass("action-exito");
    $('#mensaje').text("La operación se realizó con exito...!");
    $('#mensaje').css("display", "block");
};

removerTodoAlerta = function () {
    $('#mensaje').removeClass();
    $('#mensaje').text("");
};