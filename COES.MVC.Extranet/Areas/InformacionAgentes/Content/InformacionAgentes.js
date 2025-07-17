var controlador = siteRoot + 'InformacionAgentes/';
$(function () {
    $('#progresoCarga').hide();
    document.getElementById('file').onchange = function () {

        var fileNameIndex = this.value.lastIndexOf("\\") + 1;
        var filename = this.value.substr(fileNameIndex);
        $('#hdnKeyFile').val(filename);
    };

    $('#cbEmpresa').on('change', function () {
        $('#hdnEmpresa').val(this.value);
        buscarArchivos();
    });
    $('#btnCargar').click(function (event) {
        event.preventDefault();
        var nombreArchivo = $('#hdnKeyFile').val();
        
        if (nombreArchivo) {

            var iExistente = cantidadArchivos(nombreArchivo);

            if (iExistente == '0') {
                cargarArchivo();
            } else {

                alert('El archivo seleccionado ya existe');
            }

        } else {
            alert('Seleccione un archivo primero');
        }

    });
    buscarArchivos();
});
var cargarArchivo = function () {
    var file = document.getElementById('file').files[0];
    var fd = new FormData();

    var key = $('#hdnKeyFile').val();
    var accessKey = $('#hdnAccessKEy').val();
    var policy = $('#hdnPolicy').val();
    var signature = $('#hdnSignature').val();
    var bucket = $('#hdnBucket').val();

    fd.append('key', key);
    fd.append('acl', 'public-read');
    fd.append('Content-Type', file.type);
    fd.append('AWSAccessKeyId', accessKey);
    fd.append('policy', policy);
    fd.append('signature', signature);
    fd.append("file", file);

    var xhr = new XMLHttpRequest(); //getXMLHTTPObject();

    xhr.upload.addEventListener("progress", uploadProgress, false);
    xhr.addEventListener("load", uploadComplete, false);
    xhr.addEventListener("error", uploadFailed, false);
    xhr.addEventListener("abort", uploadCanceled, false);

    $('#progresoCarga').show();
    $('#progresoCarga').val(0);
    $('#loading').bPopup({
        fadeSpeed: 'fast',
        opacity: 0.4,
        followSpeed: 500,
        modalColor: '#000000'
    });
    xhr.open('POST', 'https://' + bucket + '.s3.amazonaws.com/', true); //MUST BE LAST LINE BEFORE YOU SEND 

    xhr.send(fd);
};
function uploadProgress(evt) {
    if (evt.lengthComputable) {
        var percentComplete = Math.round(evt.loaded * 100 / evt.total);
        //document.getElementById('progressNumber').innerHTML = percentComplete.toString() + '%';
        $('#progresoCarga').val(percentComplete);
    }
    else {
        document.getElementById('progressNumber').innerHTML = 'no se puede calcular';
    }
}
function uploadCanceled(evt) {
    alert("La carga del archivo ha sido cancelada o se reinició el browser.");
    $('#loading').bPopup().close();
}
function uploadFailed(evt) {
    alert("Hubo un error al intentar cargar el archivo." + evt);
    $('#loading').bPopup().close();
}
function uploadComplete(evt) {
    guardarDatosArchivo();
}

function buscarArchivos() {
    var iEmpresa = $('#cbEmpresa').val();
    $('#hdnEmpresa').val(iEmpresa);
    pintarPaginado();
    mostrarListado(1);
}
function pintarPaginado() {

    $.ajax({
        type: 'POST',
        url: controlador + "Bandeja/Paginado",
        data: $('#formCarga').serialize(),
        success: function (evt) {
            $('#paginado').html(evt);
            mostrarPaginado();
        },
        error: function () {
            alert("Ha ocurrido un error");
        }
    });
}

function pintarBusqueda(nroPagina) {
    mostrarListado(nroPagina);
}

function mostrarListado(nroPagina) {
    $('#hfNroPagina').val(nroPagina);
    $.ajax({
        type: 'POST',
        url: controlador + "Bandeja/ListadoArchivos",
        data: $('#formCarga').serialize(),
        success: function (evt) {
            $('#listado').css("width", $('#mainLayout').width() + "px");

            $('#listado').html(evt);
            $('#tabla').dataTable({
                "scrollY": 430,
                "scrollX": true,
                "sDom": 't',
                "ordering": true,
                "iDisplayLength": 50
            });
        },
        error: function () {
            alert("Ha ocurrido un error");
        }
    });
}

var guardarDatosArchivo = function () {
    $.ajax({
        type: 'POST',
        url: controlador + "Bandeja/GuardarDatosArchivo",
        data: {
            iEmpresa: $('#hdnEmpresa').val(),
            sNombreArchivo: $('#hdnKeyFile').val()
        },
        success: function (evt) {
            alert('Se grabaron los datos');
            $('#loading').bPopup().close();
            window.location.replace(controlador + "Bandeja/Index");
        },
        error: function () {
            alert("Ha ocurrido un error");
            $('#loading').bPopup().close();
        }
    });
};
function cantidadArchivos(nombre) {
    var retval;
    $.ajax({
        type: 'POST',
        url: controlador + "Bandeja/ExisteArchivo",
        data: {
            sNombreArchivo: nombre
        },
        async: false,
        success: function (evt) {
            retval= evt;
        },
        error: function () {
            alert("Ha ocurrido un error");
        }
    });
    return retval;
}