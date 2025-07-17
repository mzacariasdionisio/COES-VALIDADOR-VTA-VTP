var controlador = siteRoot + "rechazocarga/esquemaunifilar/";
var uploaderArchivoDatos;
var fecha;
$( /**
   * Llamadas iniciales
   * @returns {} 
   */
   $(document).ready(function () {
       //mostrarMensaje("Selecionar el periodo y la versión");

       $('#fechaRecepcion').Zebra_DatePicker({
           format: 'd/m/Y'
       });

       $('#btnBuscar').click(function () {
           buscarEmpresa();
       });

       $('#btnBuscarEmpresa').click(function () {
           muestraBuscarEmpresa();
       });

       $('#btnSeleccionarEmpresa').click(function () {
           seleccionarEmpresa();
       });

       $('#btnGuadarEdicion').click(function () {
           guardarEsquemaUnifilar();
       });

       $('#btnCancelarEdicion').click(function () {
           cancelar();
       });
       
       crearPupload();
       crearPuploadDatos();
   }));

function mostrarExito(mensaje) {
    $('#mensaje').removeClass();
    $('#mensaje').html(mensaje);
    $('#mensaje').addClass('action-exito');
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

function mostrarMensaje(mensaje) {
    $('#mensaje').removeClass();
    $('#mensaje').html(mensaje);
    $('#mensaje').addClass('action-message');
}
function buscarEmpresa() {
    var empresaIngresada = document.getElementById("empresaBuscar").value;   

    $.ajax({
        type: 'POST',
        datatype: 'json',
        url: controlador + "ListarEmpresas",
        data: {
            empresa: empresaIngresada
        },
        success: function (result) {
            $('#listadoEmpresas').css("width", "90%");
            $('#listadoEmpresas').html(result);
            $('#tablaEmpresas').dataTable({
                "filter": false,
                "ordering": true,
                "paging": false,
                "scrollY": 150,
                "scrollX": true,
                "bDestroy": true,
                "autoWidth": false,
                "columnDefs": [
                      { "width": "20%", "targets": 0 },
                      { "width": "80%", "targets": 1 }
                ]
            });
            $('#btnSeleccionarEmpresa').click(function () {
                seleccionarEmpresa();
            });
        },
        error: function () {
            alert("Error al cargar la Lista de Empresas.");
        }
    });
}

function muestraBuscarEmpresa() {

    $('#empresaBuscar').val('');
    $('#listadoEmpresas').html('');

    $("#popupEdicion").bPopup({
        autoClose: false
    });
}

function seleccionarEmpresa() {
    var empresa = $('input:radio[name=codEmpresa]:checked').val();
    if (empresa == "" || empresa == null) {
        alert('No se ha seleccionado una empresa');
        return false;
    }
    var datos = empresa.split('/');
    document.getElementById("empresaEdit").value = datos[1];
    document.getElementById("empresaSeleccionada").value = datos[0];

    $('#popupEdicion').bPopup().close();

    //luego de seleccionar la empresa se debe de cargar la lista de punto de medicion segun el tipo de empresa
    listarPuntoMedicion(datos[0]);
}

function guardarEsquemaUnifilar() {
        
    var empresaseleccionada = document.getElementById("empresaSeleccionada").value;
    if (empresaseleccionada == "" || empresaseleccionada == null) {
        alert('La empresa no ha sido seleccionada.');
        return;
    }

    var puntomedicion = $('#puntoMedicion').val();
    if (puntomedicion == "" || puntomedicion == "0") {
        alert('El punto de medición no ha sido seleccionado.');
        return;
    }

    var documento = $('#documentoEdit').val();
    if (documento == "" || documento == null) {
        alert('No se ha ingresado un documento.');
        return;
    }
        
    var fecharecepcion = $('#fechaRecepcion').val();
    if (fecharecepcion == "" || fecharecepcion == null) {
        alert('No se ha ingresado fecha recepción.');
        return;
    }

    var estado = $('#estadoEdit').val();
    if (estado == "" || estado == null) {
        alert('No se ha seleccionado un estado.');
        return;
    }
    var archivo = $('#archivo').val();
    if (archivo == "" || archivo == null) {
        alert('No se ha seleccionado un archivo Diagrama Unifilar.');
        return;
    }
    var esNuevo = true;
    var archivoDatos = $('#archivoDatos').val();

    $.ajax({
        type: 'POST',
        url: controlador + 'GuardarEsquemaUnifilar',
        data: {
            rcesqucodi: 0,
            emprcodi: empresaseleccionada,
            equicodi: puntomedicion,            
            documento: documento,
            rcesqufecharecepcion: fecharecepcion,
            estado: estado,
            archivo: archivo,
            esNuevo: true,
            archivoDatos: archivoDatos
        },
        dataType: 'json',
        success: function (result) {

            if (result.success) {
                limpiarDatosEsquemaUnifilar();
                mostrarExito('Registro ingresado satisfactoriamente');
            }
            else {
                mostrarError(result.message);
            }

        },
        error: function () {
            mostrarMensaje('mensaje', 'Ha ocurrido un error', $tipoMensajeError, $modoMensajeCuadro);
        }
    });
}
function cancelar() {
    window.location.href = controlador + "Index";
}

function listarPuntoMedicion(empcodi) {

    $('option', '#puntoMedicion').remove();
    $.ajax({
        type: 'POST',
        url: controlador + "ObtenerListaPuntoMedicion",
        dataType: 'json',
        data: {
            codigoEmpresa: empcodi
        },
        success: function (aData) {
            $('#puntoMedicion').get(0).options.length = 0;
            $('#puntoMedicion').get(0).options[0] = new Option("--SELECCIONE--", "");
            $.each(aData, function (i, item) {
                $('#puntoMedicion').get(0).options[$('#puntoMedicion').get(0).options.length] = new Option(item.Equinomb, item.Equicodi);
            });
        },
        error: function () {
            alert("Error al cargar la Lista de Empresas.");
        }
    });
}

function crearPupload() {

    var fecha = new Date();
    var formatofecha = fecha.yyyymmddhhmmss();
    uploader = new plupload.Uploader({
        runtimes: 'html5,flash,silverlight,html4',
        browse_button: 'btnSelectFile',
        multipart_params: {
            //"tipoCentral": tipoCentral,            
            "fecha": formatofecha
        },
        container: document.getElementById('container'),
        url: siteRoot + 'rechazocarga/esquemaunifilar/upload',
        flash_swf_url: '~/Content/Scripts/Moxie.swf',
        silverlight_xap_url: '~/Content/Scripts/Moxie.xap',
        multi_selection: false,
        filters: {
            max_file_size: '10mb',
            mime_types: [
                { title: "Archivos Pdf .pdf", extensions: "pdf" }
            ]
        },

        init: {
            PostInit: function () {
            },
            FilesAdded: function (up, files) {
                if (uploader.files.length == 2) {
                    uploader.removeFile(uploader.files[0]);
                }
                up.refresh();
                up.start();
            },
            UploadProgress: function (up, file) {
                mostrarAlerta("Por favor espere ...(<strong>" + file.percent + "%</strong>)");
            },
            UploadComplete: function (up, files) {
                mostrarMensaje("Subida completada.");
                plupload.each(files, function (file) {
                    var ext = file.name.split(".");
                    document.getElementById('archivo').value = ext[0] + '_' + formatofecha + '.' + ext[1];
                    //alert(file.name + formatofecha);
                });
            },
            Error: function (up, err) {
                alert(err.code + "-" + err.message);
            }
        }
    });

    uploader.init();
}
loadInfoFile = function (fileName, fileSize) {

    alert(fileName + " (" + fileSize + ")");
}

loadValidacionFile = function (mensaje) {
    mostrarAlerta(mensaje);
}
function soloNumeros(e) {
    var key = window.Event ? e.which : e.keyCode;
    return (key <= 13 || (key >= 48 && key <= 57) || key == 46);
}

function limpiarDatosEsquemaUnifilar() {
    document.getElementById("empresaSeleccionada").value = '';
    document.getElementById("empresaEdit").value = '';

    $('#puntoMedicion').val('');
    $('#documentoEdit').val('');
    $('#fechaRecepcion').val('');
    $('#estadoEdit').val('');
    $('#archivo').val('');
    $('#archivoDatos').val('');
   
}
Date.prototype.yyyymmddhhmmss = function () {
    var yyyy = this.getFullYear();
    var mm = this.getMonth() < 9 ? "0" + (this.getMonth() + 1) : (this.getMonth() + 1); // getMonth() is zero-based
    var dd = this.getDate() < 10 ? "0" + this.getDate() : this.getDate();
    var hh = this.getHours() < 10 ? "0" + this.getHours() : this.getHours();
    var min = this.getMinutes() < 10 ? "0" + this.getMinutes() : this.getMinutes();
    var ss = this.getSeconds() < 10 ? "0" + this.getSeconds() : this.getSeconds();
    return "".concat(yyyy).concat(mm).concat(dd).concat(hh).concat(min).concat(ss);
};

function crearPuploadDatos() {

    var fechaDatos = new Date();
    fechaDatos = new Date(fechaDatos.getTime() + 1000);    
    var formatofecha2 = fechaDatos.yyyymmddhhmmss();
    uploaderArchivoDatos = new plupload.Uploader({
        runtimes: 'html5,flash,silverlight,html4',
        browse_button: 'btnSelectFileDatos',
        multipart_params: {
            //"tipoCentral": tipoCentral,            
            "fecha": formatofecha2
        },
        container: document.getElementById('container'),
        url: siteRoot + 'rechazocarga/esquemaunifilar/upload',
        flash_swf_url: '~/Content/Scripts/Moxie.swf',
        silverlight_xap_url: '~/Content/Scripts/Moxie.xap',
        multi_selection: false,
        filters: {
            max_file_size: '10mb'
        },

        init: {
            PostInit: function () {
            },
            FilesAdded: function (up, files) {
                if (uploaderArchivoDatos.files.length == 2) {
                    uploaderArchivoDatos.removeFile(uploaderArchivoDatos.files[0]);
                }
                up.refresh();
                up.start();
            },
            UploadProgress: function (up, file) {
                mostrarAlerta("Por favor espere ...(<strong>" + file.percent + "%</strong>)");
            },
            UploadComplete: function (up, files) {
                mostrarMensaje("Subida completada.");
                plupload.each(files, function (file) {
                    var ext = file.name.split(".");
                    document.getElementById('archivoDatos').value = ext[0] + '_' + formatofecha2 + '.' + ext[1];
                    //alert(file.name + formatofecha);
                });
            },
            Error: function (up, err) {
                alert(err.code + "-" + err.message);
            }
        }
    });

    uploaderArchivoDatos.init();
}