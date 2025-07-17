var controlador = siteRoot + "rechazocarga/esquemaunifilar/";
var uploaderArchivoDatos;
$( /**
   * Llamadas iniciales
   * @returns {} 
   */
   $(document).ready(function () {
       //mostrarMensaje("Selecionar el periodo y la versión");

       $('#fechaIni').Zebra_DatePicker({
           format: 'd/m/Y'
       });

       $('#fechaFin').Zebra_DatePicker({
           format: 'd/m/Y'
       });

       $('#fechaRecepcion').Zebra_DatePicker({
           format: 'd/m/Y'
       });

       $('#btnConsultar').click(function () {
           pintarPaginado();
           pintarBusqueda(1);
       });

       $('#btnGuadarEdicion').click(function () {
           guardarEdicion();
       });

       $('#btnCancelarEdicion').click(function () {
           cancelarEdicion();
       });

       $('#btnAgregar').click(function () {
           nuevoRegistro();
       });

       $('#btnBuscarEmpresa').click(function () {
           muestraBuscarEmpresa();
       });

       $('#btnBuscar').click(function () {
           buscarEmpresa();
       });       

       $('#btnExportar').click(function () {
           exportarExcel();
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

/**
    * Pinta el listado de esquema unifilares
    * @returns {} 
    */
var pintarBusqueda =    
    function (id) {

        var registros = $('#cbRegistros').val();
        //alert(registros);
        if (registros == undefined || registros == null) {
            registros = 50;
        }

        $.ajax({
            type: "POST",
            url: controlador + "ListarEsquemaUnifilar",
            data: {                
                empresa: $("#empresa").val(),
                codigoSuministro: $("#codigoSuministro").val(),               
                fecIni: $("#fechaIni").val(),
                fecFin: $("#fechaFin").val(),
                nroPagina: id,
                nroRegistros: registros
            },
            success: function (evt) {

                $('#listado').css("width", $('#mainLayout').width() + "px");
                $('#listado').html(evt);
                $('#tabla').dataTable({
                    "ordering": false,
                    "paging": false,
                    "scrollY": 340,
                    "scrollX": true,
                    "sDom": 't',
                    "bPaginate": false,
                    "fixedColumns": true
                });

                if ($("#cbRegistros").length) {
                    $('#cbRegistros').val(registros);
                }
                mostrarMensaje("Consulta generada");
            },
            error: function () {
                mostrarError('Opcion Consultar: Ha ocurrido un error');
            }
        });
    };
var nuevoRegistro = function () {
    window.location.href = controlador + "EditEsquemaUnifilar";
}

function guardarEdicion() {

    var emprcodi = $('#empresaSeleccionada').val();
    if (emprcodi == "" || emprcodi == null) {
        alert('La empresa no ha sido seleccionada');
        return;
    }

    var equicodi = $('#puntoMedicion').val();
    if (equicodi == "" || equicodi == null) {
        alert('El punto de medición no ha sido seleccionado');
        return;
    }

    var documento = $('#documentoEdit').val();
    if (documento == "" || documento == null) {
        alert('No se ha ingresado un documento');
        return;
    }

    var fecrecepcion = $('#fechaRecepcion').val();
    if (fecrecepcion == "" || fecrecepcion == null) {
        alert('No se ha ingresado fecha recepcion');
        return;
    }

    var estadoEdit = $('#estadoEdit').val();
    if (estadoEdit == "" || estadoEdit == null) {
        alert('No se ha seleccionado un estado');
        return;
    }

    var rcesqunombarchivo = $('#archivo').val();
    if (rcesqunombarchivo == "" || rcesqunombarchivo == null) {
        alert('No se ha seleccionado un archivo');
        return;
    }

    var esNuevo = false;
      
    var codEsquemaUnifilar = $('#codigoEsquemaUnifilar').val();
    var archivoDatos = $('#archivoDatos').val();

    $.ajax({
        type: 'POST',
        url: controlador + 'GuardarEsquemaUnifilar',
        data: {
            rcesqucodi:codEsquemaUnifilar,
            emprcodi: emprcodi,
            equicodi: equicodi,
            documento: documento,
            rcesqufecharecepcion: fecrecepcion,
            estado: estadoEdit,
            archivo: rcesqunombarchivo,
            EsNuevo: esNuevo,
            archivoDatos: archivoDatos
        },
        dataType: 'json',
        success: function (result) {

            if (result.success) {
                $('#popupEdicion').bPopup().close();
                
                if (esNuevo) {
                    mostrarExito("Se ha creado el esquema unifilar");
                }
                else {
                    mostrarExito("Se ha modificado el esquema unifilar");
                }
                pintarBusqueda(1);
            }
            else {
                alert(result.message);
            }

        },
        error: function () {
            mostrarMensaje('mensaje', 'Ha ocurrido un error', $tipoMensajeError, $modoMensajeCuadro);
        }
    });
}

function cancelarEdicion() {

    $('#popupEdicion').bPopup().close();

}
function modificarEsquemaUnifilar(rccareCodi) {
    $('#esNuevo').val(0);
    $('#archivoDatos').val('');
    $.ajax({
        type: 'POST',
        url: controlador + 'ObtenerEsquemaUnifilar',
        data: {
            rccarecodi: rccareCodi
        },
        dataType: 'json',
        success: function (data) {
            var jsonData = JSON.parse(data);
            var fechahora;
            $('#empresaEdit').val(jsonData.Emprrazsocial);
            $('#empresaSeleccionada').val(jsonData.Emprcodi);
            //$('#puntoMedicion').val(jsonData.Equinomb);            
            $('#documentoEdit').val(jsonData.Rcesqudocumento);
            fechahora = obtenerFechaHora('date', jsonData.Rcesqufecharecepcion);
            $('#fechaRecepcion').val(fechahora);
            $('#archivo').val(jsonData.Rcesqunombarchivo);
            $('#codigoEsquemaUnifilar').val(jsonData.Rcesqucodi);
            $('#estadoEdit').val(jsonData.Rcesquestado);
            $('#archivodatos').val(jsonData.Rcesqunombarchivodatos);

            listarPuntoMedicion(jsonData.Emprcodi, jsonData.Equicodi);
            $("#popupEdicion").bPopup({
                autoClose: false
            });
        },
        error: function () {
            mostrarMensaje('mensaje', 'Ha ocurrido un error', $tipoMensajeError, $modoMensajeCuadro);
        }
    });

}

function eliminarEsquemaUnifilar(rccareCodi) {
    if (confirm("¿Desea eliminar el registro seleccionado?")) {
        $.ajax({
            type: 'POST',
            dataType: 'json',
            url: controlador + "EliminarEsquemaUnifilar",
            data: {
                rccarecodi: rccareCodi
            },
            success: function (result) {

                if (result.success) {
                    mostrarExito("Se ha eliminado el esquema unifilar");
                    pintarBusqueda(1);
                }
                else {
                    alert(result.message);
                }

            },
            error: function () {
                mostrarError();
            }
        });
    }
}

function consultarHistorial(emprCodi, equiCodi) {
    $.ajax({
        type: 'POST',
        url: controlador + "ListarEsquemaUnifilarHistorial",
        data: {
            emprcodi: emprCodi,
            equicodi: equiCodi
        },
        success: function (result) {
            $('#listadoHistorial').css("width", "450px");
            $('#listadoHistorial').html(result);
            $('#tablaHistorial').dataTable({
                "ordering": true,
                "paging": false,
                "scrollY": 150,
                "scrollX": true,
                "bDestroy": true,
                "autoWidth": false,
                "searching": false,
                "columnDefs": [
                      { "width": "160px", "targets": 0 },
                      { "width": "250px", "targets": 1 }
                ]
            });
            $("#popupEsquemaUnifilarHistorial").bPopup({
                autoClose: false
            });

        },
        error: function () {
            mostrarError();
        }
    });
}
function muestraBuscarEmpresa() {
    $('#empresaBuscar').val('');
    $('#listadoEmpresas').html("");
    $("#popupBuscarEmpresa").bPopup({
        autoClose: false
    });
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
function seleccionarEmpresa() {
    var empresa = $('input:radio[name=codEmpresa]:checked').val();
    if (empresa == "" || empresa == null) {
        alert('No se ha seleccionado una empresa');
        return false;
    }
    var datos = empresa.split('/');
    document.getElementById("empresaEdit").value = datos[1];
    document.getElementById("empresaSeleccionada").value = datos[0];

    $('#popupBuscarEmpresa').bPopup().close();

    //luego de seleccionar la empresa se debe de cargar la lista de punto de medicion segun el tipo de empresa
    listarPuntoMedicion(datos[0], 0);
}

function listarPuntoMedicion(empcodi, equicodi) {

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
            if (equicodi > 0) {
                $('#puntoMedicion').val(equicodi);
            }
            
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
            UploadComplete: function (up, files) {
                //procesarArchivo();
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

function obtenerFechaHora(tipo, valor) {

    var str, year, month, day, hour, minutes, d, finalDate;

    str = valor.replace(/\D/g, "");
    d = new Date(parseInt(str));

    year = d.getFullYear();
    month = pad(d.getMonth() + 1);
    day = pad(d.getDate());
    hour = pad(d.getHours());
    minutes = pad(d.getMinutes());

    if (tipo == "datetime") {
        finalDate = day + "/" + month + "/" + year + " " + hour + ":" + minutes;
    }
    if (tipo == "date") {
        finalDate = day + "/" + month + "/" + year;
    }
    if (tipo == "time") {
        finalDate = hour + ":" + minutes;
    }

    return finalDate;
}
function pad(num) {
    num = "0" + num;
    return num.slice(-2);
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

    var fecha = new Date();
    fecha = new Date(fecha.getTime() + 1000);    
    var formatofecha = fecha.yyyymmddhhmmss();
    uploaderArchivoDatos = new plupload.Uploader({
        runtimes: 'html5,flash,silverlight,html4',
        browse_button: 'btnSelectFileDatos',
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
                    document.getElementById('archivoDatos').value = ext[0] + '_' + formatofecha + '.' + ext[1];
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

function exportarExcel() {

    $.ajax({
        type: 'POST',
        dataType: 'json',
        url: controlador + 'GenerarReporte',
        data: {
            empresa: $("#empresa").val(),
            codigoSuministro: $("#codigoSuministro").val(),
            fecIni: $("#fechaIni").val(),
            fecFin: $("#fechaFin").val()
        },
        success: function (result) {
            if (result != "-1") {
                window.location.href = controlador + 'DescargarFormato?file=' + result;
                //mostrarExito("Se ha eliminado el registro correctamente.");
                //pintarBusqueda();
            }
            else {
                alert("Error al generar el archivo.");
            }
        },
        error: function () {
            mostrarAlerta("Error al generar el archivo");
        }
    });
}

pintarPaginado = function () {

    var registros = $('#cbRegistros').val();

    if (registros == undefined || registros == null) {
        registros = 50;
    }

    $.ajax({
        type: 'POST',
        url: controlador + "paginado",
        data: {
            empresa: $("#empresa").val(),
            codigoSuministro: $("#codigoSuministro").val(),
            fecIni: $("#fechaIni").val(),
            fecFin: $("#fechaFin").val(),
            nroRegistrosPag: registros
        },
        success: function (evt) {
            $('#paginado').html(evt);
            mostrarPaginado();
        },
        error: function () {
            alert("Ha ocurrido un error");
        }
    });
}

function mostrarPaginado() {
    $.ajax({
        type: 'POST',
        url: controlador + 'paginado',
        success: function (evt) {
            $('#paginado').html(evt);
            mostrarPaginado();
        },
        error: function () {
            alert("Error al cargar el paginado");
        }
    });
}