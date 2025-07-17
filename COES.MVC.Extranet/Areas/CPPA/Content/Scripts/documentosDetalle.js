var rutaImagenes = siteRoot + "Content/Images/";
var controller = siteRoot + "CPPA/Enviar/";
var dtDetalle
var popDetalle

$(document).ready(function () {

    $('#btnRetornar').on('click', function () {
        const anio = $('#Anio').val();
        const ajuste = $('#Ajuste').val();
        const revision = $('#Revision').val();
        //const desde = $('#Desde').val();
        //const hasta = $('#Hasta').val();

        var form = document.createElement("form");
        form.method = "POST"; 
        form.action = siteRoot + "CPPA/Enviar/Index";

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


        document.body.appendChild(form);
        form.submit();
    });

    listaDetalle();
});

function listaDetalle() {
    const documento = $('#Documento').val();
    $.ajax({
        type: 'POST',
        url: controller + 'ListaDocumentosDetalleGrilla',
        contentType: 'application/json; charset=utf-8',
        data: JSON.stringify({
            documento: documento
        }),
        datatype: 'json',
        traditional: true,
        success: function (result) {

            if (result.Resultado == -1) {
                if ($.fn.DataTable.isDataTable('#dtDetalle')) {
                    $('#dtDetalle').DataTable().clear().draw();
                }
                SetMessage('#messageDetalle', 'El servicio no esta disponible en este momento...!', 'error', true);
                return;
            }

            dtDetalle = $('#dtDetalle').DataTable({
                data: result.ListaDocumentosDetalleGrilla,
                columns: [
                    { title: 'Detalle', data: 'Cpaddtcodi', visible: false },
                    { title: 'Documento', data: 'Cpadoccodi', visible: false },
                    { title: 'Ruta', data: 'Cpaddtruta', visible: false },
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
                        defaultContent: '<img class="clsDescargar" src="' + rutaImagenes + 'btn-download.png" style="cursor: pointer;" title="Descargar"/>'
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

            if (response.resultado == -1) {
                SetMessage('#messageDetalle', 'El servicio no esta disponible en este momento...!', 'error', true);
                return;
            }

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
    //Variables para la cabecera de documentos
    const cparcodi = $('#cbPopRevision').val();
    const revision = $('#cbPopRevision option:selected').text();
    const anio = $('#cbPopAnio').val();
    const ajuste = $('#cbPopAjuste').val();
    const emprcodi = 1234;//REEMPLZAR ESTo

    let files = new FormData();

    var tabla = $('#dtNuevo').DataTable();
    var documentos = tabla.rows().data(); 
    documentos.each(function (data, index) {
        let archivo = data.file;
        
        if (archivo) {        
            files.append('files[' + index + ']', archivo);
        }
    });
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
            $("#popNuevo").bPopup().close();
        },
        error: function () {
            alert("Ha ocurrido un problema...");
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