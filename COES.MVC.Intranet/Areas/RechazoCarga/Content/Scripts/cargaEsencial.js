var controlador = siteRoot + "rechazocarga/cargaesencial/";

$( /**
   * Llamadas iniciales
   * @returns {} 
   */
   $(document).ready(function () {
       //mostrarMensaje("Selecionar el periodo y la versión");
       $('input[type=radio][name=tipoEmpresa]').change(function () {
           //$('empresaEdit').val(''); alert('1');
           document.getElementById('empresaEdit').value = '';
           $('empresaSeleccionada').val('');
           $('#puntoMedicion').empty();
           $('#puntoMedicion').get(0).options.length = 0;
           $('#puntoMedicion').get(0).options[0] = new Option("--SELECCIONE--", "");
       });

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

function muestraBuscarEmpresa() {
    $("#empresaBuscar").val('');
    $("#listadoEmpresas").html("");
    $("#popupBuscarEmpresa").bPopup({
        autoClose: false
    });
}
var nuevoRegistro = function () {
    window.location.href = controlador + "EditCargaEsencial";
}

var pintarBusqueda =
    /**
    * Pinta el listado de periodos según el año seleccionado
    * @returns {} 
    */
    function (id) {

        var registros = $('#cbRegistros').val();
        //alert(registros);
        if (registros == undefined || registros == null) {
            registros = 50;
        }


        $.ajax({
            type: "POST",
            url: controlador + "listarCargaEsencial",
            data: {
                estado: $("#estado").val(),
                razonSocial: $("#razonSocial").val(),
                documento: $("#documento").val(),
                cargaIni: $("#cargaIni").val(),
                cargaFin: $("#cargaFin").val(),
                fecIni: $("#fechaIni").val(),
                fecFin: $("#fechaFin").val(),
                nroPagina: id,                
                nroRegistros: registros
            },
            success: function (evt) {

                $('#listadoCargaEsencial').css("width", $('#mainLayout').width() + "px");
                $('#listadoCargaEsencial').html(evt);
                $('#tablaCargaEsencial').dataTable({
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

                mostrarMensaje("Consulta generada.");
            },
            error: function () {
                mostrarError('Opción Consultar: Ha ocurrido un error');
            }
        });
    };


function modificarCargaEsencial(rccareCodi) {
    $('#esNuevo').val(0);

    $.ajax({
        type: 'POST',
        url: controlador + 'ObtenerCargaEsencial',
        data: {
            rccarecodi: rccareCodi
        },
        dataType: 'json',
        success: function (data) {
            var jsonData = JSON.parse(data);
            var fechahora;
            $('#empresaEdit').val(jsonData.Emprrazsocial);
            //$('#puntoMedicion').val(jsonData.Equinomb);
            $('#carga').val(jsonData.Rccarecarga);
            $('#documentoEdit').val(jsonData.Rccaredocumento);
            fechahora = obtenerFechaHora('date', jsonData.Rccarefecharecepcion);
            $('#fechaRecepcion').val(fechahora);
            $('#estadoEdit').val(jsonData.Rccareestado);
            $('#archivo').val(jsonData.Rccarenombarchivo);
            $('#codigoCargaEsencial').val(jsonData.Rccarecodi);
            $('#empresaSeleccionada').val(jsonData.Emprcodi);

            if (jsonData.EsUsuarioLibre) {
                $("#tipoEmpresaUsuarioLibre").attr('checked', 'checked');
            } else {
                $("#tipoEmpresaDistribuidor").attr('checked', 'checked');
            }            

            if (jsonData.Rccaretipocarga == 1) {                
                document.getElementById("cargaParcialUsuarioLibre").checked = true;
                document.getElementById("cargaTotalUsuarioLibre").checked = false;
            } else {                
                document.getElementById("cargaParcialUsuarioLibre").checked = false;
                document.getElementById("cargaTotalUsuarioLibre").checked = true;
            }

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

function eliminarCargaEsencial(rccareCodi) {
    if (confirm("¿Desea eliminar el registro seleccionado?")) {
        $.ajax({
            type: 'POST',
            dataType: 'json',
            url: controlador + "EliminarCargaEsencial",
            data: {
                rccarecodi: rccareCodi
            },
            success: function (result) {

                if (result.success) {
                    mostrarExito("Se ha eliminado el registro correctamente.");
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

    if (equiCodi == '' || equiCodi == null) {
        equiCodi = 0;
    }
    $.ajax({
        type: 'POST',
        url: controlador + "ListarCargaEsencialHistorial",
        data: {
            emprcodi: emprCodi,
            equicodi: equiCodi
        },
        success: function (result) {            
            $('#listadoHistorial').css("width", "450px");
            $('#listadoHistorial').html(result);
            $('#tablaHistorial').dataTable({
                "filter": false,
                "ordering": false,
                "paging": false,
                "scrollY": 150,
                "scrollX": true,
                "bDestroy": true,
                "autoWidth": false,                
                "columnDefs": [
                      { "width": "150px", "targets": 0 },
                      { "width": "150px", "targets": 1 },
                      { "width": "100px", "targets": 2 }
                ]
            });
            $("#popupCargaEsencialHistorial").bPopup({
                autoClose: false
            });

        },
        error: function () {
            mostrarError();
        }
    });
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

/*
* Guarda la edición del incremento/reducción
*/
function guardarEdicion() {

    var empcodi = $('#empresaSeleccionada').val();
    var empresaEdit = document.getElementById('empresaEdit').value;
    if (empcodi == "" || empcodi == null || empresaEdit == "") {
        alert('No se ha seleccionado una empresa.');
        return;
    }

    var equicod = $('#puntoMedicion').val();
    if ($("#tipoEmpresaUsuarioLibre").is(":checked")) {
        if (equicod == "" || equicod == null) {
            alert('El punto de medición no ha sido seleccionado.');
            return;
        }
    } else {
        equicod = 0;
    }

    var rccarecarga = $('#carga').val();
    if (rccarecarga == "" || rccarecarga == null) {
        alert('No se ha ingresado la carga.');
        return;
    }

    var rccaredocumento = $('#documentoEdit').val();
    if (rccaredocumento == "" || rccaredocumento == null) {
        alert('No se ha ingresado el documento.');
        return;
    }

    var rccarefecharecepcion = $('#fechaRecepcion').val();
    if (rccarefecharecepcion == "" || rccarefecharecepcion == null) {
        alert('No se ha ingresado la fecha de recepción.');
        return;
    }

    var rccareestado = $('#estadoEdit').val();
    if (rccareestado == "" || rccareestado == null) {
        alert('No se ha seleccionado el estado.');
        return;
    }
    var rccarenombarchivo = $('#archivo').val();
    if (rccarenombarchivo == "" || rccarenombarchivo == null) {
        alert('No se ha subido el archivo.');
        return;
    }

    var esNuevo = false;
    var codCargaEsencial = $('#codigoCargaEsencial').val();
    var tipoCarga = $('input:radio[name=tipoCarga]:checked').val();    

    $.ajax({
        type: 'POST',
        url: controlador + 'GuardarCargaEsencial',
        data: {
            codigoCargaEsencial: codCargaEsencial,
            empresa: empcodi,
            puntoMedicion: equicod,
            carga: rccarecarga,
            documento: rccaredocumento,
            fechaRecepcion: rccarefecharecepcion,
            estado: rccareestado,
            archivo: rccarenombarchivo,
            EsNuevo: esNuevo,
            tipoCarga: tipoCarga
        },
        dataType: 'json',
        success: function (result) {

            if (result.success) {
                $('#popupEdicion').bPopup().close();                
                alert("Se ha modificado la carga esencial.");
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

function buscarEmpresa() {
    var empresaIngresada = document.getElementById("empresaBuscar").value;
    var tipoEmpresaId = $('input:radio[name=tipoEmpresa]:checked').val();

    $.ajax({
        type: 'POST',
        datatype: 'json',
        url: controlador + "ListarEmpresas",
        data: {
            empresa: empresaIngresada,
            tipoEmpresa: tipoEmpresaId
        },
        success: function (result) {
            $('#listadoEmpresas').css("width", "90%");
            $('#listadoEmpresas').html(result);
            $('#tablaListaEmpresas').dataTable({
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
function soloNumeros(e) {
    var key = window.Event ? e.which : e.keyCode;
    return (key <= 13 || (key >= 48 && key <= 57) || key == 46);
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
        url: siteRoot + 'rechazocarga/cargaesencial/upload',
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

Date.prototype.yyyymmddhhmmss = function () {
    var yyyy = this.getFullYear();
    var mm = this.getMonth() < 9 ? "0" + (this.getMonth() + 1) : (this.getMonth() + 1); // getMonth() is zero-based
    var dd = this.getDate() < 10 ? "0" + this.getDate() : this.getDate();
    var hh = this.getHours() < 10 ? "0" + this.getHours() : this.getHours();
    var min = this.getMinutes() < 10 ? "0" + this.getMinutes() : this.getMinutes();
    var ss = this.getSeconds() < 10 ? "0" + this.getSeconds() : this.getSeconds();
    return "".concat(yyyy).concat(mm).concat(dd).concat(hh).concat(min).concat(ss);
};

function exportarExcel() {
       
    $.ajax({
        type: 'POST',
        dataType: 'json',
        url: controlador + 'GenerarReporte',
        data: {
            estado: $("#estado").val(),
            razonSocial: $("#razonSocial").val(),
            documento: $("#documento").val(),
            cargaIni: $("#cargaIni").val(),
            cargaFin: $("#cargaFin").val(),
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

    if ($('#cbPeriodoIni').val() != "") {

        $.ajax({
            type: 'POST',
            url: controlador + "paginado",
            data: {
                estado: $("#estado").val(),
                razonSocial: $("#razonSocial").val(),
                documento: $("#documento").val(),
                cargaIni: $("#cargaIni").val(),
                cargaFin: $("#cargaFin").val(),
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
    else {
        mostrarAlerta("Seleccione periodo.");
    }
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
