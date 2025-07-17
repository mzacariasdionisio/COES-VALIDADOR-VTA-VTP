var pop, objHt;

$(document).ready(function () {

    document.getElementById('btnImportar').addEventListener('click', openDialog);

    $('#fileDireccion').on("change", function () {
        importarArchivo();
    });

});

function openDialog() {
    var e = document.getElementById('fileDireccion');
    e.click();
}

function importarArchivo() {
    let ListaArchivosEncontrados = $('#ListaArchivosEncontrados')
    ListaArchivosEncontrados.hide();
    const archivoSeleccionado = ($('#fileDireccion'))[0].files[0];
    document.getElementById('txtDireccion').value = archivoSeleccionado.name; 
    let archivo = new FormData();
    archivo.append('archivo', archivoSeleccionado);

    $.ajax({
        type: 'POST',
        url: controller + 'MaestrasImportar',
        contentType: false,
        processData: false,
        data: archivo,
        success: function (result) {
            console.log(result, 'rr');
            SetMessage('#message', result.dataMsg, result.typeMsg, true);
            if (result.detailMsg.length > 0) {

                let fileList = $('#fileList');
                fileList.empty();

                $.each(result.detailMsg, function (i, item) {
                    fileList.append('<li>' + item + '</li>');
                });
                ListaArchivosEncontrados.show();
            }
        },
        error: function () {
            alert("Ha ocurrido un problema...");
        }
    });
}

function btnImportarCarpeta() {
    let folderPath = $('#txtFolderPath').val();
    let ListaArchivosEncontrados = $('#ListaArchivosEncontrados')
    ListaArchivosEncontrados.hide();
    $.ajax({
        url: controller + 'GetFiles',
        type: 'POST',
        data: { folderPath: folderPath },
        success: function (data) {
            if (data != "-1") {
                let fileList = $('#fileList');
                fileList.empty();
                data.forEach(function (fileName) {
                    fileList.append('<li>' + fileName + '</li>');
                });
                ListaArchivosEncontrados.show();
            }
            else
                alert('La carpeta no existe.');
        },
        error: function (xhr) {
            alert('Error al obtener los archivos: ' + xhr.responseText);
        }
    });
}