var rutaImagenes = siteRoot + "Content/Images/";
var controller = siteRoot + "CPPA/DescargaArchivos/";
var dtDetalle
var popDetalle

$(document).ready(function () {

    $('#btnRetornar').on('click', function () {
        const anio = $('#Anio').val();
        const ajuste = $('#Ajuste').val();
        const revision = $('#Revision').val();
        const emprcodi = $('#Emprcodi').val();

        var form = document.createElement("form");
        form.method = "POST";
        form.action = siteRoot + "CPPA/DescargaArchivos/Index";

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

        var hiddenEmprcodi = document.createElement("input");
        hiddenEmprcodi.type = "hidden";
        hiddenEmprcodi.name = "emprcodi";
        hiddenEmprcodi.value = emprcodi;
        form.appendChild(hiddenEmprcodi);

        document.body.appendChild(form);
        form.submit();
    });

    $('#btnDescargar').on('click', function () {
        let rutasSeleccionadas = [];
        $('.clsDescargar:checked').each(function () {
            let rowIndex = $(this).closest('tr').index();
            let rowData = $('#dtDetalle').DataTable().row(rowIndex).data();
            let ruta = rowData.Cpaddtruta;
            rutasSeleccionadas.push(ruta);
        });

        if (rutasSeleccionadas.length > 1) {
            descargarArchivos(rutasSeleccionadas); 
        } else if (rutasSeleccionadas.length === 1) {
            var filePath = rutasSeleccionadas[0];
            descargarArchivo(filePath);
        } else {
            SetMessage('#message',
                'No se marcó ninguna casilla',
                'warning', true);
        }
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
                        data: null,
                        render: function (data, type, row, meta) {
                            return '<input type="checkbox" class="clsDescargar" data-id="' + row.Cpaddtcodi + '" />';
                        }
                    },
                ],
                columnDefs: [
                    {
                        targets: [5], width: 70,
                        /*defaultContent: '<img class="clsDescargar" src="' + rutaImagenes + 'btn-open.png" style="cursor: pointer;" title="Descargar"/> '*/
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

function descargarArchivo(filePath) {

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

                SetMessage('#message', 'El archivo se descargo correctamente.', 'success', true);
            } else {
                alert(response.message || 'Error al descargar el archivo.');
            }
        },
        error: function () {
            alert('Error al descargar el archivo.');
        }
    });
}

function descargarArchivos(filePath) {

    const anio = $('#Anio').val();
    const ajuste = $('#Ajuste').val();
    const envio = document.querySelector('[data-envio]').getAttribute('data-envio');
    const revision = document.querySelector('[data-revision]').getAttribute('data-revision');
    const empresa = $('#Emprnomb').val();

    $.ajax({
        url: controller + 'DescargarDocumentos',
        type: 'GET',
        data: { rutasArchivo: filePath.join(','), anio: anio, ajuste: ajuste, revision: revision, envio: envio, empresa: empresa },
        success: function (response) {
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

                SetMessage('#message', 'El archivo se descargó correctamente.', 'success', true);
            } else {
                alert(response.message || 'Error al descargar el archivo.');
            }
        },
        error: function () {
            alert('Error al descargar los archivos.');
        }
    });
}

function formatBytes(bytes, decimals = 2) {
    if (bytes === 0) return '0 Bytes';
    const k = 1024;
    const dm = decimals < 0 ? 0 : decimals;
    const sizes = ['Bytes', 'KB', 'MB', 'GB', 'TB'];
    const i = Math.floor(Math.log(bytes) / Math.log(k));
    return parseFloat((bytes / Math.pow(k, i)).toFixed(dm)) + ' ' + sizes[i];
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