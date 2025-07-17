var controlador = siteRoot + "GMM/agentes/";

$( /**
   * Llamadas iniciales
   * @returns {} 
   */
    $(document).ready(function () {

        $('#tab-container').easytabs({
            animate: false
        });

        $('#habilitado').prop('checked', true);
        $('#habilitadoEdit').prop('checked', true);


        $('#fechaIni').Zebra_DatePicker({
            format: 'd/m/Y'
        });

        $('#fechaFin').Zebra_DatePicker({
            format: 'd/m/Y'
        });

        $('#fechaIngresoEdit').Zebra_DatePicker({
            format: 'd/m/Y'
        });

        $('#fechaInicioEdit').Zebra_DatePicker({
            format: 'd/m/Y'
        });

        $('#fechaFinEdit').Zebra_DatePicker({
            format: 'd/m/Y'
        });


        $('#btnAgregar').click(function () {
            nuevoRegistro();
        });

        $('#btnConsultar').click(function () {
            pintarBusqueda();
        });

        $('#btnLimpiar').click(function () {
            limpiarBusqueda();
        });

        $('#btnGuadarEdicion').click(function () {
            guardarEdicion();
        });

        $('#btnCancelarEdicion').click(function () {
            cancelarEdicion();
        });

        $('#btnGuadarGarantia').click(function () {
            guardarGarantia();
        });
        $('#btnNuevaGarantia').click(function () {
            limpiarModalidad();
        });

        $('#btnBuscarEmpresa').click(function () {
            muestraBuscarEmpresa();
        });

        $('#btnBuscar').click(function () {
            buscarEmpresa();
        });

        $('#tipoModalidadEdit').change(function () {
            var valorCambiado = $(this).val();
            if (valorCambiado == 'CR') {
                $('#divMontoLab').css('display', 'none');
                $('#divMontoDet').css('display', 'none');
                $('#divCertificadoLab').css('display', 'block');
                $('#divCertificadoDet').css('display', 'block');
            }
            if (valorCambiado == 'GA') {
                $('#divMontoLab').css('display', 'block');
                $('#divMontoDet').css('display', 'block');
                $('#divCertificadoLab').css('display', 'none');
                $('#divCertificadoDet').css('display', 'none');
            }
            if (valorCambiado == '0') {
                $('#divMontoLab').css('display', 'none');
                $('#divMontoDet').css('display', 'none');
                $('#divCertificadoLab').css('display', 'none');
                $('#divCertificadoDet').css('display', 'none');
            }


        });

        $('#razonSocial').on('change', function () {
            if ($(this).val() != "0") $("#documento").val($(this).val());
            else $("#documento").val("");
        });

        crearPupload();
        pintarBusqueda();
        $("#btnSelectFile").on('change', function () {
            $("#spanAlertaPopup").hide();
        })
        $("#btnSelectFile").click(function () {
            $("#spanAlertaPopup").hide();
        })
    }));


/**
* Pinta el listado de registros según el filtro
* @returns {} 
*/
function pintarBusqueda(mensaje) {

    var _razonSocial = $("#razonSocial option:selected").text();
    if (_razonSocial == "Todas") {
        _razonSocial = "";
    }

    var _valEstado = $('#cboEstado').val();
    var _valDosMasIncumplimientos = $('#dosMasIncumplimientos').is(':checked');

    $.ajax({
        type: "POST",
        url: controlador + "listarAgentes",
        data: {
            razonSocial: _razonSocial,
            documento: $("#documento").val(),
            tipoParticipante: $("#tipoParticipante").val(),
            tipoModalidad: $("#tipoModalidad").val(),
            fecIni: $("#fechaIni").val(),
            fecFin: $("#fechaFin").val(),
            estado: _valEstado,
            dosMasIncumplimientos: _valDosMasIncumplimientos
        },
        success: function (evt) {

            $('#listadoAgentes').css("width", $('#mainLayout').width() + "px");
            $('#listadoAgentes').html(evt);
            $('#tablaAgentes').dataTable({
                "ordering": true,
                "paging": false,
                "scrollY": 340,
                "scrollX": true,
                "scrollCollapse": true,
                "fixedColumns": true
            });
            if (mensaje == "" || mensaje == undefined) {
                mostrarMensaje("Búsqueda ejecutada correctamente.");
            } else {
                mostrarMensaje(mensaje);
            }
        },
        error: function () {
            mostrarError('No se puedo ejecutar la búsqueda.');
        }
    });
};

var nuevoRegistro = function () {
    //window.location.href = controlador + "EditCargaEsencial";
    limpiarMensajepe();

    document.getElementById('divBuscarEmpresa').style.display = 'block';
    $('#divMontoLab').css('display', 'none');
    $('#divMontoDet').css('display', 'none');
    $('#divCertificadoLab').css('display', 'none');
    $('#divCertificadoDet').css('display', 'none');

    listarModalidades(0);
    listarEstados(0);
    listarIncumplimientos(0);

    $('#empresaEdit').val('');
    $('#empresaSeleccionada').val('');
    $('#documentoEdit').val('');
    $('#tipoParticipanteEdit').val();
    $('#fechaIngresoEdit').val('');
    $('#esNuevo').val(1);
    $('#esNuevoModalidad').val(1)
    $("#enRevisionEdit").removeAttr("checked");
    $("#habilitadoEdit").removeAttr("checked");
    $("#suspendidoEdit").removeAttr("checked");
    $("#bajaEdit").removeAttr("checked");
    // limpiar
    $("#popupEdicion").bPopup({
        autoClose: false,
        modalClose: false,
        positionStyle: 'fixed'
    });
}

function limpiarBusqueda() {
    $('input[name=rbtEstado]:checked').prop("checked", false);
    $('#dosMasIncumplimientos').prop("checked", false);
    $("#razonSocial").val("");
    $("#documento").val("");
    $("#tipoParticipante").val("0");
    $("#tipoModalidad").val("0");
    $("#fechaIni").val("");
    $("#fechaFin").val("");
}

function modificarAgentes(pEmpgcodi) {
    $('#esNuevo').val(0);
    DesbloquearPopup();
    document.getElementById('divBuscarEmpresa').style.display = 'none';
    // Limpiar garantías:
    limpiarGarantia();
    limpiarMensajepe();

    $.ajax({
        type: 'POST',
        url: controlador + 'ObtenerAgente',
        data: {
            empgcodi: pEmpgcodi
        },
        dataType: 'json',
        success: function (data) {
            var jsonData = JSON.parse(data);

            var fechahora;
            $('#empresaEdit').val(jsonData.EmpNombreEdit);
            $('#empresaSeleccionada').val(jsonData.EmpCodiEdit);
            $('#documentoEdit').val(jsonData.EmpRucEdit);
            $('#tipoParticipanteEdit').val(jsonData.EmptpartEdit);
            fechahora = obtenerFechaHora('date', jsonData.EmpFecIngEdit);
            $('#fechaIngresoEdit').val(fechahora);
            $('#estadoEmpresaIni').val(jsonData.EmpestadoEdit);

            $("#enRevisionEdit").prop("checked", false);
            $("#habilitadoEdit").prop("checked", false);
            $("#suspendidoEdit").prop("checked", false);
            $("#bajaEdit").removeAttr("checked");

            switch (jsonData.EmpestadoEdit) {
                case "R": $("#enRevisionEdit").prop("checked", true); break;
                case "H": $("#habilitadoEdit").prop("checked", true); break;
                case "S": $("#suspendidoEdit").prop("checked", true); break;
                case "B": $("#bajaEdit").attr("checked", "checked"); break;
                default: console.log('estado inválido');
            }
            $('#comentarioEdit').val(jsonData.EmpComentarioEdit);

            listarModalidades(jsonData.EmpCodiEdit);
            listarEstados(jsonData.EmpCodiEdit);
            listarIncumplimientos(jsonData.EmpCodiEdit);

            $('#esNuevo').val(0);
            $('#codigoAgente').val(jsonData.EmpCodiEdit);
            $('#esNuevoModalidad').val("1")
            $("#popupEdicion").bPopup({
                autoClose: false
            });
        },
        error: function () {
            mostrarMensaje('mensaje', 'Ha ocurrido un error', $tipoMensajeError, $modoMensajeCuadro);
        }
    });


}

function eliminarAgentes(pEmpgcodi) {

    if (confirm('¿Está seguro de eliminar el participante seleccionado?')) {

        $.ajax({
            type: 'POST',
            url: controlador + 'EliminarAgente',
            data: {
                empgcodi: pEmpgcodi
            },
            dataType: 'json',
            success: function (result) {
                if (result.success) {
                    pintarBusqueda("Se eliminó al agente correctamente");
                } else {
                    mostrarMensaje("No se pudo eliminar al agente");
                }
               
            },
            error: function () {
                mostrarMensaje('mensaje', 'Ha ocurrido un error', $tipoMensajeError, $modoMensajeCuadro);
            }
        });
    };

}

function listarModalidades(empgcodi) {

    $.ajax({
        type: 'POST',
        url: controlador + "ObtenerListaModalidades",
        async: false,
        dataType: 'html',
        data: {
            empgcodi: empgcodi
        },
        success: function (aData) {
            $('#listadoAgenteModalidades').html(aData);
            $('#tablaAgenteModalidades').dataTable({
                "ordering": true,
                "paging": false,
                "scrollY": 340,
                "scrollX": true,
                "scrollCollapse": true,
                "fixedColumns": true
            });
        },
        error: function (XMLHttpRequest, textStatus, errorThrown) {
            mostrarError("Ocurrió un error al obtener la Lista de Modalidades.");
        }
    });
}
function listarEstados(empgcodi) {

    $.ajax({
        type: 'POST',
        url: controlador + "ObtenerListaEstados",
        async: false,
        dataType: 'html',
        data: {
            empgcodi: empgcodi
        },
        success: function (aData) {
            $('#listadoAgenteEstados').html(aData);
            $('#tablaAgenteEstados').dataTable({
                "ordering": true,
                "paging": false,
                "scrollY": 340,
                "scrollX": true,
                "scrollCollapse": true,
                "fixedColumns": true
            });
        },
        error: function (XMLHttpRequest, textStatus, errorThrown) {
            mostrarError("Ocurrió un error al obtener la Lista de Estados.");
        }
    });
}
function listarIncumplimientos(empgcodi) {

    $.ajax({
        type: 'POST',
        url: controlador + "ObtenerListaIncumplimientos",
        async: false,
        dataType: 'html',
        data: {
            empgcodi: empgcodi
        },
        success: function (aData) {
            $('#listadoAgenteIncumplimientos').html(aData);
            $('#tablaAgenteIncumplimientos').dataTable({
                "ordering": true,
                "paging": false,
                "scrollY": 340,
                "scrollX": true,
                "scrollCollapse": true,
                "fixedColumns": true
            });
        },
        error: function (XMLHttpRequest, textStatus, errorThrown) {
            mostrarError("Ocurrió un error al obtener Lista de Incumplimientos.");
        }
    });
}

function guardarEdicion() {

    var empcodi = $('#empresaSeleccionada').val();
    var empresaEdit = document.getElementById('empresaEdit').value;
    if (empcodi == "" || empcodi == null || empresaEdit == "") {
        $('#empresaEdit').focus();
        mostrarAlertape('Debe seleccionar una empresa.');
        return;
    }

    var documento = $('#documentoEdit').val();
    if (documento == "" || documento == null) {
        $('#documentoEdit').focus();
        mostrarAlertape('Debe seleccionar una empresa.');
        return;
    }

    var fechaingreso = $('#fechaIngresoEdit').val();
    if (fechaingreso == "" || fechaingreso == null) {
        $('#fechaIngresoEdit').focus();
        mostrarAlertape('Debe seleccionar la fecha de ingreso.');
        return;
    }

    var tipoparticipante = $("#tipoParticipanteEdit").val();
    if (tipoparticipante == "0" || tipoparticipante == null) {
        $("#tipoParticipanteEdit").focus();
        mostrarAlertape('Debe seleccionar un tipo de participante.');
        return;
    }

    var _valEstado = $('input[name=rbtEstadoEdit]:checked').val();

    var esNuevo = $('#esNuevo').val() == 1 ? true : false;
    var codAgente = $('#codigoAgente').val();
    var estadoEmpresaIni = $('#estadoEmpresaIni').val();
    /*
    int pEmpgcodi, string pEmpgfecingreso, string pEmpgtipopart,
    string pEmpgestado, int pEmprcodi, string pEmpgComentario, bool pEsNuevo
    */
    $.ajax({
        type: 'POST',
        url: controlador + 'GuardarAgente',
        data: {
            pEmpgcodi: codAgente === "" ? 0 : codAgente,
            pEmpgfecingreso: fechaingreso,
            pEmpgtipopart: tipoparticipante,
            pEmpgestado: _valEstado,
            pEmprcodi: empcodi,
            pEmpgComentario: $('#comentarioEdit').val(),
            pEsNuevo: esNuevo,
            estadoEmpresaIni: estadoEmpresaIni
        },
        dataType: 'json',
        success: function (result) {

            if (result.success) {
                //$('#popupEdicion').bPopup().close();

                $("#codigoAgente").val(result.gnmEmpresa.EMPGCODI);
                if (esNuevo) {
                    BloquearPopup();
                } else {


                    DesbloquearPopup();
                }
                pintarBusqueda("Se guardo el registro correctamente.");

            }
            else {
                mostrarMensaje(result.message);
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

function limpiarGarantia() {

    document.getElementById("tipoModalidadEdit").selectedIndex = 0;
    $('#fechaInicioEdit').val("");
    $('#fechaFinEdit').val("");
    document.getElementById("tipoCertificacionEdit").selectedIndex = 0;
    $('#montoGarantiaEdit').val("");
    $('#archivo').val("");
}


function guardarGarantia() {

    var fechainicio = $('#fechaInicioEdit').val();
    if (fechainicio == "" || fechainicio == null) {
        $('#fechaInicioEdit').focus();
        mostrarAlertape('Debe seleccionar la fecha de inicio.');
        return;
    }

    var fechafin = $('#fechaFinEdit').val();
    if (fechafin == "" || fechafin == null) {
        $('#fechaFinEdit').focus();
        mostrarAlertape('Debe seleccionar la fecha de fin.');
        return;
    }

    var modalidad = $("#tipoModalidadEdit").val();
    if (modalidad == "0" || modalidad == null) {
        $("#tipoModalidadEdit").focus();
        mostrarAlertape('Debe seleccionar una modalidad.');
        return;
    }

    var tipocertificacion = $("#tipoCertificacionEdit").val();
    if (modalidad == "CR" && tipocertificacion == "0") {
        $("#tipoCertificacionEdit").focus();
        mostrarAlertape('Debe seleccionar un Tipo Certificado.');
        return;
    }

    var nombarchivo = $('#archivo').val();
    if (nombarchivo == "" || nombarchivo == null) {
        $('#archivo').focus();
        mostrarAlertape('Debe seleccionar subir un archivo.');
        return;
    }

    var montoGarantiaEdit = $('#montoGarantiaEdit').val();
    if (modalidad == "GA" && montoGarantiaEdit == null) {
        $("#montoGarantiaEdit").focus();
        mostrarAlertape('Debe ingresar el valor del monto de la Garantía.');
        return;
    }
    var codAgente = $('#codigoAgente').val();

    $.ajax({
        type: 'POST',
        url: controlador + 'GuardarGarantia',
        data: {
            pGaraCodi: $("#codigoGarantia").val(),
            pEmpgcodi: codAgente === "" ? 0 : codAgente,
            pFecini: fechainicio,
            pFecfin: fechafin,
            pMontoGarantia: montoGarantiaEdit === "" ? 0 : montoGarantiaEdit,
            pArchivo: nombarchivo,
            pTipoCertificado: tipocertificacion,
            pTipoModalidad: modalidad,
            nuevo: $('#esNuevoModalidad').val()
        },
        dataType: 'json',
        success: function (result) {

            if (result.success) {
                $('#popupEdicion').bPopup().close();
                pintarBusqueda("Se guardo el registro correctamente.");
            }
            else {
                mostrarMensaje(result.message);
            }

        },
        error: function () {
            mostrarAlertape('mensaje', 'Ha ocurrido un error', $tipoMensajeError, $modoMensajeCuadro);
        }
    });

    //limpiarGarantia();
}
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
function mostrarMensajepe(mensaje) {
    $('#mensajepe').removeClass();
    $('#mensajepe').html(mensaje);
    $('#mensajepe').addClass('action-message');
}
function mostrarAlertape(mensaje) {
    $('#mensajepe').removeClass();
    $('#mensajepe').html(mensaje);
    $('#mensajepe').addClass('action-alert');
}
function mostrarError(mensaje) {
    $('#mensajepe').removeClass();
    $('#mensajepe').html(mensaje);
    $('#mensajepe').addClass('action-error');
}
function limpiarMensajepe() {
    $('#mensajepe').removeClass();
    $('#mensajepe').html("");
}
function limpiarMensaje() {
    $('#mensaje').removeClass();
    $('#mensaje').html("");
}
function muestraBuscarEmpresa() {
    $("#empresaBuscar").val('');
    $("#listadoEmpresas").html("");
    $("#popupBuscarEmpresa").bPopup({
        autoClose: false
    });
}
function buscarEmpresa() {

    var empresaIngresada = document.getElementById("empresaBuscar").value;
    var tipoEmpresaId = $('input:radio[name=tipoEmpresa]:checked').val(); // $('#tipoParticipanteEdit').val(); //

    $.ajax({
        type: 'POST',
        datatype: 'json',
        url: controlador + "ListarMaestroEmpresas",
        data: {
            empresa: empresaIngresada
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
            mostrarError("Ocurrió un error al obtener la Lista de Empresas.");
        }
    });
}
function seleccionarEmpresa() {
    var empresa = $('input:radio[name=codEmpresa]:checked').val();
    if (empresa == "" || empresa == null) {
        mostrarAlertape('No ha seleccionado una empresa');
        return false;
    }
    var datos = empresa.split('/');
    document.getElementById("empresaEdit").value = datos[1];
    document.getElementById("empresaSeleccionada").value = datos[0];
    document.getElementById("documentoEdit").value = datos[2];

    $('#popupBuscarEmpresa').bPopup().close();
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
        url: siteRoot + 'gmm/agentes/Upload',
        flash_swf_url: '~/Content/Scripts/Moxie.swf',
        silverlight_xap_url: '~/Content/Scripts/Moxie.xap',
        multi_selection: false,
        filters: {
            max_file_size: '20mb',
            mime_types: [
                { title: "Archivos Pdf .pdf", extensions: "pdf" }
            ]
        },

        init: {
            preload: function () {
                $("#spanAlertaPopup").hide();
            },
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
                    if (ext[0].length > 35) {
                        $("#spanAlertaPopup").show();
                        $("#spanAlertaPopup").html("* El nombre del archivo supera el limite 35 letras");
                        uploader.removeFile(uploader.files[0]);
                        $('#archivo').val("");
                    }
                    //alert(file.name + formatofecha);
                });
            },
            Error: function (up, err) {
                console.log(err);
                if (err.message == 'File size error.') {
                    $("#spanAlertaPopup").show();
                    $("#spanAlertaPopup").html("* El archivo adjuntado supera el límite permitido 2mb");
                } else {
                    mostrarAlertape(err.code + "-" + err.message);
                }
            }
        }
    });

    uploader.init();
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
loadInfoFile = function (fileName, fileSize) {

    mostrarAlerta(fileName + " (" + fileSize + ")");
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

function descargarFile(fileName) {
    controler = siteRoot + "gmm/agentes/descarga";
    var paramList = [
        { tipo: 'input', nombre: 'fileName', value: fileName }
    ];
    var form = CreateForm(controler, paramList);
    document.body.appendChild(form);
    form.submit();
    return true;
}

function CreateForm(path, params, method = 'post') {
    var form = document.createElement('form');
    form.method = method;
    form.action = path;

    $.each(params, function (index, obj) {
        var hiddenInput = document.createElement(obj.tipo);
        hiddenInput.type = 'hidden';
        hiddenInput.name = obj.nombre;
        hiddenInput.value = obj.value;
        form.appendChild(hiddenInput);
    });
    return form;
}

/*
  MANTENIMIENTO MODALIDAD
 
 */
function limpiarGarantia() {

    document.getElementById("tipoModalidadEdit").selectedIndex = 0;
    $('#fechaInicioEdit').val("");
    $('#fechaFinEdit').val("");
    document.getElementById("tipoCertificacionEdit").selectedIndex = 0;
    $('#montoGarantiaEdit').val("");
    $('#archivo').val("");
}

function guardarGarantia() {

    var fechainicio = $('#fechaInicioEdit').val();
    if (fechainicio == "" || fechainicio == null) {
        $('#fechaInicioEdit').focus();
        mostrarAlertape('Debe seleccionar la fecha de inicio.');
        return;
    }

    var fechafin = $('#fechaFinEdit').val();
    if (fechafin == "" || fechafin == null) {
        $('#fechaFinEdit').focus();
        mostrarAlertape('Debe seleccionar la fecha de fin.');
        return;
    }

    var modalidad = $("#tipoModalidadEdit").val();
    if (modalidad == "0" || modalidad == null) {
        $("#tipoModalidadEdit").focus();
        mostrarAlertape('Debe seleccionar una modalidad.');
        return;
    }

    var tipocertificacion = $("#tipoCertificacionEdit").val();
    if (modalidad == "CR" && tipocertificacion == "0") {
        $("#tipoCertificacionEdit").focus();
        mostrarAlertape('Debe seleccionar un Tipo Certificado.');
        return;
    }

    var nombarchivo = $('#archivo').val();
    if (nombarchivo == "" || nombarchivo == null) {
        $('#archivo').focus();
        mostrarAlertape('Debe seleccionar subir un archivo.');
        return;
    }

    var montoGarantiaEdit = $('#montoGarantiaEdit').val();
    if (modalidad == "GA" && montoGarantiaEdit == null) {
        $("#montoGarantiaEdit").focus();
        mostrarAlertape('Debe ingresar el valor del monto de la Garantía.');
        return;
    }
    var codAgente = $('#codigoAgente').val();

    $.ajax({
        type: 'POST',
        url: controlador + 'GuardarGarantia',
        data: {
            pGaraCodi: $("#codigoGarantia").val(),
            pEmpgcodi: codAgente === "" ? 0 : codAgente,
            pFecini: fechainicio,
            pFecfin: fechafin,
            pMontoGarantia: montoGarantiaEdit === "" ? 0 : montoGarantiaEdit,
            pArchivo: nombarchivo,
            pTipoCertificado: tipocertificacion,
            pTipoModalidad: modalidad,
            nuevo: $('#esNuevoModalidad').val()
        },
        dataType: 'json',
        success: function (result) {

            if (result.success) {
                $('#popupEdicion').bPopup().close();
                pintarBusqueda();
                mostrarMensaje("Se guardo el registro correctamente.");
            }
            else {
                mostrarMensaje(result.message);
            }

        },
        error: function () {
            mostrarAlertape('mensaje', 'Ha ocurrido un error', $tipoMensajeError, $modoMensajeCuadro);
        }
    });

    //limpiarGarantia();
}

function limpiarModalidad() {

    document.getElementById("tipoModalidadEdit").selectedIndex = 0;
    $('#fechaInicioEdit').val("");
    $('#fechaFinEdit').val("");
    document.getElementById("tipoCertificacionEdit").selectedIndex = 0;
    $('#montoGarantiaEdit').val("");
    $('#archivo').val("");
    $("#codigoGarantia").val(0);
    $('#esNuevoModalidad').val("1");
}

function modificarModalidad(pGarantiaCodi) {
    $('#esNuevoModalidad').val(0);

    // Limpiar garantías:
    limpiarModalidad();
    limpiarMensajepe();

    $.ajax({
        type: 'POST',
        url: controlador + 'ObtenerGarantia',
        data: {
            garacodi: pGarantiaCodi
        },
        dataType: 'json',
        success: function (data) {
            var jsonData = JSON.parse(data);
            console.log(jsonData);
            var fechahora;
            $('#tipoModalidadEdit').val(jsonData.TMODCODI);
            $('#tipoCertificacionEdit').val(jsonData.TCERCODI);
            fechahora = obtenerFechaHora('date', jsonData.GARAFECINICIO);
            $('#fechaInicioEdit').val(fechahora);
            fechahora = obtenerFechaHora('date', jsonData.GARAFECFIN);
            $('#fechaFinEdit').val(fechahora);
            $('#montoGarantiaEdit').val(jsonData.GARAMONTOGARANTIA);
            $('#archivo').val(jsonData.GARAARCHIVO);

            if (jsonData.TMODCODI == 'CR') {
                $('#divMontoLab').css('display', 'none');
                $('#divMontoDet').css('display', 'none');
                $('#divCertificadoLab').css('display', 'block');
                $('#divCertificadoDet').css('display', 'block');
            }
            if (jsonData.TMODCODI == 'GA') {
                $('#divMontoLab').css('display', 'block');
                $('#divMontoDet').css('display', 'block');
                $('#divCertificadoLab').css('display', 'none');
                $('#divCertificadoDet').css('display', 'none');
            }
            if (jsonData.TMODCODI == '0') {
                $('#divMontoLab').css('display', 'none');
                $('#divMontoDet').css('display', 'none');
                $('#divCertificadoLab').css('display', 'none');
                $('#divCertificadoDet').css('display', 'none');
            }

            $("#codigoGarantia").val(pGarantiaCodi);


            $('#esNuevoModalidad').val(0);

        },
        error: function () {
            mostrarMensaje('mensaje', 'Ha ocurrido un error', $tipoMensajeError, $modoMensajeCuadro);
        }
    });


}

function eliminarModalidad(pGarantiaCodi) {

    if (confirm('¿Está seguro de realizar esta acción?')) {
        $.ajax({
            type: 'POST',
            url: controlador + 'EliminarGarantia',
            data: {
                pGaraCodi: pGarantiaCodi
            },
            dataType: 'json',
            success: function (result) {

                if (result.success) {
                    $('#popupEdicion').bPopup().close();
                    pintarBusqueda("Se eliminó el registro correctamente.");
                }
                else {
                    mostrarMensaje(result.message);
                }

            },
            error: function () {
                mostrarAlertape('mensaje', 'Ha ocurrido un error', $tipoMensajeError, $modoMensajeCuadro);
            }
        });
    }
    //mensajeOperacion("Este seguro de eliminar la garantía seleccionado?", null
    //    , {
    //        showCancel: true,
    //        onOk: $.ajax({
    //            type: 'POST',
    //            url: controlador + 'EliminarGarantia',
    //            data: {
    //                pGaraCodi: pGarantiaCodi
    //            },
    //            dataType: 'json',
    //            success: function (result) {

    //                if (result.success) {
    //                    $('#popupEdicion').bPopup().close();
    //                    pintarBusqueda();
    //                    mostrarMensaje("Se eliminó el registro correctamente.");
    //                }
    //                else {
    //                    mostrarMensaje(result.message);
    //                }

    //            },
    //            error: function () {
    //                mostrarAlertape('mensaje', 'Ha ocurrido un error', $tipoMensajeError, $modoMensajeCuadro);
    //            }
    //        }),
    //        onCancel: function () {

    //        }
    //    });
    limpiarGarantia();

}

function BloquearPopup() {
    $("#btnCancelarEdicion").attr("disabled", "disabled");
    $(".b-close").css("display", "none");
    $("#spanAlertaPopup").show();
    $("#spanAlertaPopup").html("* Debe registrar una modalidad antes de salir de la pantalla");
}
function DesbloquearPopup() {
    $("#btnCancelarEdicion").removeAttr("disabled");
    $(".b-close").css("display", "");

}

/*

 MANTENIMIENTO MODALIDAD

 */