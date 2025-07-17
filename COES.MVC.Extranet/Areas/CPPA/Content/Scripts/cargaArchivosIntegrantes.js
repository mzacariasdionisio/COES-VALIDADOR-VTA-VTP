var rutaImagenes = siteRoot + "Content/Images/";
var controller = siteRoot + "CPPA/Enviar/";
var dtEnvios, dtDetalle, dtNuevo
var popEmpresas, popNuevo, popDetalle

$(document).ready(function () {

    //Aplica la lib. Zebra_Datepicker
    $('.filterFecha').Zebra_DatePicker({
    });

    document.getElementById('btnSeleccionar').addEventListener('click', openDialog);

    $('#btnCancelar').on('click', function () {
        $("#popCambiarEmpresa").bPopup().close();
    });

    $('#btnRetornar').on('click', function () {
        $("#popDetalle").bPopup().close();
    });

    $('#btnNuevo').on('click', function () {
        cargaNuevosArchivos();
    });

    $('#btnPopCancelar').on('click', function () {
        $("#popNuevo").bPopup().close();
    });

    $('#cbAnio').change(function () {
        cargarAjustes($(this).val());
    });

    $('#cbAjuste').change(function () {
        cargarRevisiones($('#cbAnio').val(), $(this).val());
    })

    $('#cbPopAnio').change(function () {
        cargarPopAjustes($(this).val());
    });

    $('#cbPopAjuste').change(function () {
        cargarPopRevisiones($('#cbPopAnio').val(), $(this).val());
    })

    $('#file').on("change", function () {
        importarArchivo();
    });

    $('#btnPopEnviar').on('click', function () {
        grabarDocumentos();
    });

    $('#btnConsultar').on('click', function () {
        listaEnviosPresupuestoAnual();
    });

    cargaParametros();
});

function cargaParametros() {
    const anio = $('#Anio').val() || null;
    const ajuste = $('#Ajuste').val();
    const revision = $('#Revision').val();

    if (anio !== null) {
        document.getElementById('cbAnio').value = anio;
        cargarAjustes(anio);
        if (ajuste !== null) {
            cargarRevisiones(anio, ajuste);
            document.getElementById('cbAjuste').value = ajuste;
            document.getElementById('cbRevision').value = revision;
            listaEnviosPresupuestoAnual();
        }
    }
}

function cargarAjustes(year) {

    if (year) {
        $('#cbAjuste').prop('disabled', false).empty().append('<option value="">Seleccione</option>');
        $('#cbRevision').prop('disabled', true).empty().append('<option value="">Seleccione</option>');

        listRevision
            .filter(x => x.Cpaapanio === parseInt(year))
            .map(x => x.Cpaapajuste)
            .filter((value, index, self) => self.indexOf(value) === index) // Eliminar duplicados
            .forEach((cpaapajuste) => { $('#cbAjuste').append('<option value="' + cpaapajuste + '">' + cpaapajuste + '</option>'); });
    }
    else {
        $('#cbAjuste').prop('disabled', true).empty().append('<option value="">Seleccione</option>');
        $('#cbRevision').prop('disabled', true).empty().append('<option value="">Seleccione</option>');
    }
}

function cargarRevisiones(year, fit) {
    if (year && fit) {
        $('#cbRevision').prop('disabled', false).empty().append('<option value="">Seleccione</option>');

        listRevision
            .filter(x => x.Cpaapanio === parseInt(year) && x.Cpaapajuste === fit)
            .forEach((revision) => {
                let estado = revision.Cparestado == 'A' ? '' : ' [' + revision.Cparestado + ']';
                $('#cbRevision').append('<option value="' + revision.Cparcodi + '">' + revision.Cparrevision + estado + '</option>');
            });
    }
    else {
        $('#cbRevision').prop('disabled', true).empty().append('<option value="">Seleccione</option>');
    }
}

function cargarPopAjustes(year) {

    if (year) {
        $('#cbPopAjuste').prop('disabled', false).empty().append('<option value="">Seleccione</option>');
        $('#cbPopRevision').prop('disabled', true).empty().append('<option value="">Seleccione</option>');

        listRevision
            .filter(x => x.Cpaapanio === parseInt(year))
            .map(x => x.Cpaapajuste)
            .filter((value, index, self) => self.indexOf(value) === index) // Eliminar duplicados
            .forEach((cpaapajuste) => { $('#cbPopAjuste').append('<option value="' + cpaapajuste + '">' + cpaapajuste + '</option>'); });
    }
    else {
        $('#cbPopAjuste').prop('disabled', true).empty().append('<option value="">Seleccione</option>');
        $('#cbPopRevision').prop('disabled', true).empty().append('<option value="">Seleccione</option>');
    }
}

function cargarPopRevisiones(year, fit) {
    if (year && fit) {
        $('#cbPopRevision').prop('disabled', false).empty().append('<option value="">Seleccione</option>');

        listRevision
            .filter(x => x.Cpaapanio === parseInt(year) && x.Cpaapajuste === fit)
            .forEach((revision) => {
                let estado = revision.Cparestado == 'A' ? '' : ' [' + revision.Cparestado + ']';
                $('#cbPopRevision').append('<option value="' + revision.Cparcodi + '">' + revision.Cparrevision + estado + '</option>');
            });
    }
    else {
        $('#cbPopRevision').prop('disabled', true).empty().append('<option value="">Seleccione</option>');
    }
}

function openDialog() {
    var e = document.getElementById('file');
    e.click();
}

function importarArchivo() {
    var archivos = document.getElementById('file').files;
    var table = $('#dtNuevo').DataTable();
    var extensionesPermitidas = ['xls', 'xlsx', 'doc', 'docx', 'pdf', 'csv'];

    if ((table.rows().data().length + archivos.length) > 5) {
        SetMessage('#mpop_nuevo', 'No se permiten cargar más de 05 archivos', 'warning', true);
        return;
    }

    for (var i = 0; i < archivos.length; i++) {
        var archivo = archivos[i];

        if (archivo.name.length > 40) {
            SetMessage('#mpop_nuevo', 'El nombre de un archivo excede los 40 caracteres. No se cargarán los archivos seleccionados.', 'warning', true);
            continue;
        }

        var maxSize = 15 * 1024 * 1024; // 15MB en bytes
        if (archivo.size > maxSize) {
            SetMessage('#mpop_nuevo', 'No se permite cargar un archivo cuyo tamaño exceda los 15MB', 'warning', true);
            continue;
        }

        // Obtener la extensión del archivo
        var extension = archivo.name.split('.').pop().toLowerCase();
        if (extensionesPermitidas.indexOf(extension) === -1) {
            SetMessage('#mpop_nuevo', 'Solo se pueden cargar archivos con las extensiones .xls, .xlsx, .doc, .docx, .pdf o .csv. No se cargarán los archivos seleccionados.', 'warning', true);
            continue;
        }


        var archivoData = {
            nombre: archivo.name,
            tamanio: formatBytes(archivo.size),
            file: archivo,
            index: Date.now() + i
        };
        table.row.add(archivoData);
    }
    table.draw();
}

function cargaNuevosArchivos() {

    popNuevo = $('#popNuevo').bPopup({
        easing: 'easeOutBack',
        speed: 350,
        transition: 'fadeIn',
        modalClose: false
    },)

    const anio = $('#cbAnio').val() || null;
    const ajuste = $('#cbAjuste').val() || null;
    const revision = $('#cbRevision').val() || null;
    if (anio !== null) {
        document.getElementById('cbPopAnio').value = $('#cbAnio').val();
        cargarPopAjustes(anio);
        if (ajuste !== null) {
            cargarPopRevisiones(anio, ajuste);
            document.getElementById('cbPopAjuste').value = $('#cbAjuste').val();
            document.getElementById('cbPopRevision').value = $('#cbRevision').val();
        }
    }

    if (!$.fn.DataTable.isDataTable('#dtNuevo')) {
        dtNuevo = $('#dtNuevo').DataTable({
            data: [],
            columns: [
                { title: 'Nombre del archivo', data: 'nombre', visible: true },
                { title: 'File', data: 'file', visible: false },
                { title: 'Tamaño', data: 'tamanio', visible: true },
                {
                    title: 'Quitar',
                    data: null
                },
            ],
            columnDefs: [
                {
                    targets: [3], width: 70,
                    render: function (data, type, row, meta) {
                        return '<img class="clsQuitar" src="' + rutaImagenes + 'btn-cancel.png" style="cursor: pointer;" title="Quitar" data-index="' + row.index + '"/>';
                    }
                }
            ],
            createdRow: function (row, data, index) {

            },
            searching: false,
            bLengthChange: false,
            bSort: false,
            destroy: true,
            paging: false,
            info: false
        });
    }


    $('#dtNuevo').off('click', '.clsQuitar').on('click', '.clsQuitar', function () {
        var row = $(this).closest('tr');
        dtNuevo.row(row).remove().draw();
    });

    //$('#dtNuevo').on('click', '.clsQuitar', function () {
    //    var index = $(this).data('index');
    //    eliminarArchivo(index);
    //});
}

function listaEnviosPresupuestoAnual() {

    const cparcodi = $('#cbRevision').val();
    const emprcodi = $('#Emprcodi').val();

    if (!cparcodi) {
        SetMessage('#message', 'No se ha seleccionado un Año presupuesto, Ajuste o Revisión. No se realizó la consulta.', 'warning', true);
        return; // Detener la ejecución si falta alguno de los filtros
    }

    $.ajax({
        type: 'POST',
        url: controller + 'ListaDocumentosGrilla',
        contentType: 'application/json; charset=utf-8',
        data: JSON.stringify({
            cparcodi: cparcodi,
            emprcodi: emprcodi
        }),
        datatype: 'json',
        traditional: true,
        success: function (result) {

            if (result.Resultado == -1) {
                if ($.fn.DataTable.isDataTable('#dtEnviosPresupuesto')) {
                    $('#dtEnviosPresupuesto').DataTable().clear().draw();
                }
                SetMessage('#message', 'El servicio no esta disponible en este momento...!', 'error', true);
                return;
            }

            if (result.sStatus == -1) {
                SetMessage('#message', result.sMensaje, 'warning', true);
            }

            if (result.sStatus == 0 && result.sMensaje !== null) {
                SetMessage('#message', result.sMensaje, 'success', true);
            }
            dtEnvios = $('#dtEnviosPresupuesto').DataTable({
                data: result.ListaDocumentosGrilla,
                columns: [
                    {
                        title: 'Acciones',
                        data: null
                    },
                    { title: 'Codigo', data: 'Cpadoccodi', visible: false },
                    {
                        title: 'Fecha envío',
                        data: 'Cpadocfeccreacion',
                        visible: true,
                        render: function (data) {
                            if (data) {
                                return formatDate(data);
                            } else {
                                return data
                            }
                        }
                    },
                    { title: 'Usuario', data: 'Cpadocusucreacion', visible: true },
                    { title: 'Año ppto.', data: 'Cpaapanio', visible: true },
                    { title: 'Ajuste', data: 'Cpaapajuste', visible: true },
                    { title: 'idRevision', data: 'Cparcodi', visible: false },
                    { title: 'Revisión', data: 'Cparrevision', visible: true },
                    { title: 'Código envío', data: 'Cpadoccodenvio', visible: true }
                ],
                columnDefs: [
                    {
                        targets: [0], width: 70,
                        defaultContent: '<img class="clsVer" src="' + rutaImagenes + 'btn-open1.png" style="cursor: pointer;" title="Ver"/> ' + '<img class="clsDescarga" src="' + rutaImagenes + 'btn-download.png" style="cursor: pointer;" title="Descargar"/>'
                    }
                ],
                createdRow: function (row, data, index) {

                },
                searching: true,
                bLengthChange: false,
                bSort: false,
                destroy: true,
                paging: false,
                info: false,
                scrollY: '273px',
                pageLength: 10,
            });
        },
        error: function () {
            alert("Ha ocurrido un problema...");
        }
    });
}


$(document).on('click', '#dtEnviosPresupuesto tr td .clsDescarga', function (e) {
    var row = $(this).closest('tr');
    var r = dtEnvios.row(row).data();

    descargaDocumentosZipeados(r.Cpadoccodi, r.Cpadoccodenvio);
});

function descargaDocumentosZipeados(cpadoccodi, cpadoccodenvio) {

    const anio = $('#cbAnio').val();
    const ajuste = $('#cbAjuste').val();
    const revision = $('#cbRevision option:selected').text();
    const emprnomb = $('#Emprnomb').val();

    $.ajax({
        url: controller + 'DescargaDocumentosZipeados',
        type: 'GET',
        data: { documento: cpadoccodi, envio: cpadoccodenvio, anio: anio, ajuste: ajuste, revision: revision, empresa: emprnomb },
        success: function (response) {

            if (response.resultado == -1) {
                SetMessage('#message', 'El servicio no esta disponible en este momento...!', 'error', true);
                return;
            }

            if (response.success) {
                var byteCharacters = atob(response.fileBytes);
                var byteArray = new Uint8Array(byteCharacters.length);

                for (var i = 0; i < byteCharacters.length; i++) {
                    byteArray[i] = byteCharacters.charCodeAt(i);
                }

                var blob = new Blob([byteArray], { type: 'application/zip' });
                var url = window.URL.createObjectURL(blob);
                var a = document.createElement('a');
                a.href = url;
                a.download = response.fileName;
                a.click();

                window.URL.revokeObjectURL(url);

                SetMessage('#mpop-detalle', 'El archivo se descargó correctamente.', 'success', true);
            } else {
                alert(response.message || 'Error al descargar el archivo.');
            }
        },
        error: function () {
            alert('Error al descargar los archivos.');
        }
    });
}


$(document).on('click', '#dtEnviosPresupuesto tr td .clsVer', function (e) {
    //$('#dtDetalle').html("");
    var row = $(this).closest('tr');
    var r = dtEnvios.row(row).data();

    listaDocumentosDetalle(r.Cpadoccodi, r.Cpadoccodenvio)
});

function listaDocumentosDetalle(cpadoccodi, cpadoccodenvio) {
    const anio = $('#cbAnio').val();
    const ajuste = $('#cbAjuste').val();
    const revision = $('#cbRevision').val();
    const revisionName = $('#cbRevision option:selected').text();
    const emprcodi = $('#Emprcodi').val();
    const emprnomb = $('#Emprnomb').val();

    var form = document.createElement("form");
    form.method = "POST";
    form.action = siteRoot + "CPPA/Enviar/ListaDetalleGrilla";

    var hiddenDocumento = document.createElement("input");
    hiddenDocumento.type = "hidden";
    hiddenDocumento.name = "documento";
    hiddenDocumento.value = cpadoccodi;
    form.appendChild(hiddenDocumento);

    var hiddenAnio = document.createElement("input");
    hiddenAnio.type = "hidden";
    hiddenAnio.name = "anio";
    hiddenAnio.value = anio;
    form.appendChild(hiddenAnio);

    var hiddenAjuste = document.createElement("input");
    hiddenAjuste.type = "hidden";
    hiddenAjuste.name = "ajuste";
    hiddenAjuste.value = ajuste;
    form.appendChild(hiddenAjuste);

    var hiddenRevision = document.createElement("input");
    hiddenRevision.type = "hidden";
    hiddenRevision.name = "revision";
    hiddenRevision.value = revision;
    form.appendChild(hiddenRevision);

    var hiddenRevisionName = document.createElement("input");
    hiddenRevisionName.type = "hidden";
    hiddenRevisionName.name = "revisionName";
    hiddenRevisionName.value = revisionName;
    form.appendChild(hiddenRevisionName);

    var hiddenEnvio = document.createElement("input");
    hiddenEnvio.type = "hidden";
    hiddenEnvio.name = "envio";
    hiddenEnvio.value = cpadoccodenvio;
    form.appendChild(hiddenEnvio);

    var hiddenEmprcodi = document.createElement("input");
    hiddenEmprcodi.type = "hidden";
    hiddenEmprcodi.name = "emprcodi";
    hiddenEmprcodi.value = emprcodi;
    form.appendChild(hiddenEmprcodi);

    var hiddenEmprnomb = document.createElement("input");
    hiddenEmprnomb.type = "hidden";
    hiddenEmprnomb.name = "emprnomb";
    hiddenEmprnomb.value = emprnomb;
    form.appendChild(hiddenEmprnomb);

    document.body.appendChild(form);
    form.submit();
}

function listaDetalle(cparcodi) {

    popDetalle = $('#popDetalle').bPopup({
        easing: 'easeOutBack',
        speed: 350,
        transition: 'fadeIn',
        modalClose: false
    },
        function () {
            $.ajax({
                type: 'POST',
                url: controller + 'ListaDocumentosDetalleGrilla',
                contentType: 'application/json; charset=utf-8',
                data: JSON.stringify({
                    cparcodi: cparcodi
                }),
                datatype: 'json',
                traditional: true,
                success: function (result) {

                    if (result.Resultado == -1) {
                        if ($.fn.DataTable.isDataTable('#dtDetalle')) {
                            $('#dtDetalle').DataTable().clear().draw();
                        }
                        SetMessage('#message', 'El servicio no esta disponible en este momento...!', 'error', true);
                        return;
                    }

                    dtDetalle = $('#dtDetalle').DataTable({
                        data: result.ListaDocumentosDetalleGrilla,
                        columns: [
                            { title: 'Detalle', data: 'Cpaddtcodi', visible: false },
                            { title: 'Documento', data: 'Cpadoccodi', visible: false },
                            { title: 'Ruta', data: 'Cpaddtruta', visible: true },
                            { title: 'Nombre del archivo', data: 'Cpaddtnombre', visible: true },
                            { title: 'Tamaño', data: 'Cpaddttamano', visible: true },
                            {
                                title: 'Descargar',
                                data: null
                            },
                        ],
                        columnDefs: [
                            {
                                targets: [5], width: 70,
                                defaultContent: '<img class="clsDescargar" src="' + rutaImagenes + 'btn-open.png" style="cursor: pointer;" title="Descargar"/> '
                            }
                        ],
                        createdRow: function (row, data, index) {

                        },
                        searching: false,
                        bLengthChange: false,
                        bSort: false,
                        destroy: true,
                        paging: false,
                        info: false,
                        scrollY: '180px',
                    });
                },
                error: function () {
                    alert("Ha ocurrido un problema...");
                }
            });
        }
    )
}

$(document).on('click', '#dtDetalle tr td .clsDescargar', function (e) {
    var row = $(this).closest('tr');
    var r = dtDetalle.row(row).data();
    descargarDocumentos(r.Cpaddtruta);
});

function descargarDocumentos(filePath) {

    $.ajax({
        url: controller + 'DescargarDocumento',
        type: 'GET',
        data: { rutaArchivo: filePath },
        success: function (response) {

            if (response.success) {
                var byteCharacters = atob(response.fileBytes);
                var byteArray = new Uint8Array(byteCharacters.length);

                for (var i = 0; i < byteCharacters.length; i++) {
                    byteArray[i] = byteCharacters.charCodeAt(i);
                }

                var blob = new Blob([byteArray], { type: 'application/octet-stream' });
                var url = window.URL.createObjectURL(blob);
                var a = document.createElement('a');
                a.href = url;
                a.download = response.fileName;
                a.click();

                window.URL.revokeObjectURL(url);

                SetMessage('#mpop-detalle', 'El archivo se descargo correctamente.', 'success', true);
            } else {
                alert(response.message || 'Error al descargar el archivo.');
            }
            isDownloading = false;
        },
        error: function () {
            isDownloading = false;
            alert('Error al descargar el archivo.');
        }
    });
}

function eliminarArchivo(index) {
    var table = $('#dtNuevo').DataTable();
    var rowData = table.row(index).data();
    table.row(index).remove().draw();
}

function formatBytes(bytes, decimals = 2) {
    if (bytes === 0) return '0 Bytes';
    const k = 1024;
    const dm = decimals < 0 ? 0 : decimals;
    const sizes = ['Bytes', 'KB', 'MB', 'GB', 'TB'];
    const i = Math.floor(Math.log(bytes) / Math.log(k));
    return parseFloat((bytes / Math.pow(k, i)).toFixed(dm)) + ' ' + sizes[i];
}

function grabarDocumentos() {

    var tabla = $('#dtNuevo').DataTable();
    var documentos = tabla.rows().data();

    if (documentos.length === 0) {
        SetMessage('#mpop_nuevo', 'No se han cargado archivos. No se realizó el envío.', 'warning', true);
        return; // Detener la ejecución si no hay documentos
    }

    let files = new FormData();

    documentos.each(function (data, index) {
        let archivo = data.file;
        
        if (archivo) {        
            files.append('files[' + index + ']', archivo);
        }
    });

    const cparcodi = $('#cbPopRevision').val();
    const revision = $('#cbPopRevision option:selected').text();
    const anio = $('#cbPopAnio').val();
    const ajuste = $('#cbPopAjuste').val();
    const emprcodi = $('#Emprcodi').val();

    if (!cparcodi || !anio || !ajuste) {
        SetMessage('#mpop_nuevo', 'No se ha seleccionado un Año, Ajuste o Revisión. No se realizó el envío.', 'warning', true);
        return; // Detener la ejecución si falta alguno de los filtros
    }

    files.append('anio', anio);
    files.append('ajuste', ajuste);
    files.append('revision', revision);
    files.append('cparcodi', cparcodi);
    files.append('emprcodi', emprcodi);

    $.ajax({
        type: 'POST',
        url: controller + 'GrabarDocumentos',
        contentType: false,
        processData: false,
        data: files,
        success: function (result) {

            if (result.Resultado == -1) {
                SetMessage('#mpop_nuevo', 'El servicio no esta disponible en este momento...!', 'error', true);
                return;
            }

            if (result.sStatus == 1) {
                SetMessage('#mpop_nuevo', result.sMensaje, 'warning', true);
            }

            if (result.sStatus == 0) {
                $("#popNuevo").bPopup().close();
                SetMessage('#message', result.sMensaje, 'success', true);

                document.getElementById('cbAnio').value = anio;
                cargarAjustes(anio);
                cargarRevisiones(anio, ajuste);
                document.getElementById('cbAjuste').value = ajuste;
                document.getElementById('cbRevision').value = cparcodi;
                listaEnviosPresupuestoAnual();
            }

            if (result.sStatus == -1) {
                SetMessage('#mpop_nuevo', result.sMensaje, 'error', true);
            }
            
        },
        error: function () {
            alert("Ha ocurrido un problema...");
        }
    });
}

seleccionarEmpresa = function () {

    $.ajax({
        type: 'POST',
        url: controller + "EscogerEmpresa",
        success: function (evt) {
            $('#popup').html(evt);
            setTimeout(function () {
                $('#popup').bPopup({
                    easing: 'easeOutBack',
                    speed: 450,
                    transition: 'slideDown',
                    modalClose: false,
                });
            }, 50);
        },
        error: function () {
            alert("Lo sentimos, ha ocurrido un error inesperado");
        }
    });
}

function SetMessage(container, msg, tpo, del) {//{Contenedor, mensaje(string), tipoMensaje(string), delay(bool)}
    var new_class = "msg-" + tpo;//Identifica la nueva clase css
    $(container).removeClass($(container).attr('class'));//Quita la clase css previa
    $(container).addClass(new_class);
    $(container).html(msg);//Carga el mensaje
    //$(container).show();

    //Focus to message
    $('html, body').animate({ scrollTop: 0 }, 5);

    //Mensaje con delay o no
    if (del) $(container).show(0).delay(5000).hide(0);//5 Segundos
    else $(container).show();
}

function formatDate(dateString) {
    // Extrae el número de milisegundos
    const milliseconds = parseInt(dateString.replace(/\/Date\((\d+)\)\//, '$1'));
    const date = new Date(milliseconds);

    // Formato dd/mm/yyyy
    const day = String(date.getDate()).padStart(2, '0');
    const month = String(date.getMonth() + 1).padStart(2, '0'); // Meses son 0-indexados
    const year = date.getFullYear();

    // Formato hh:mm
    const hours = String(date.getHours()).padStart(2, '0');
    const minutes = String(date.getMinutes()).padStart(2, '0');

    //return `${day}/${month}/${year}`;
    return `${day}/${month}/${year} ${hours}:${minutes}`;
}