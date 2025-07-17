var controlador = siteRoot + "GMM/Incumplimientos/";

$( /**
   * Llamadas iniciales
   * @returns {} 
   */
   $(document).ready(function () {
  

       $('#fechaRecepcionEdit').Zebra_DatePicker({
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

       $('#btnAgregarMontoEdicion').click(function () {
           nuevoMontoEdit();
       });

       $('#btnGuardarArchivo').click(function () {
           guardarArchivo();
       });       

       $('#btnGuadarEdicion').click(function () {
           guardarEdicion();
       });

       $('#btnCancelarEdicion').click(function () {
           cancelarEdicion();
       });

       $('#btnBuscarEmpresaAgente').click(function () {
           muestraBuscarEmpresaAgente();
       });

       $('#btnAgenteBuscar').click(function () {
           buscarEmpresaAgente();
       });

       $('#btnBuscarEmpresa').click(function () {
           muestraBuscarEmpresa();
       });

       $('#btnBuscar').click(function () {
           buscarEmpresa();
       });

       crearPupload();
       listaTipoInforme();
       setAnio("anhoEdit");
        setAnio("anho");
        pintarBusqueda();

   }));

var pintarBusqueda =
    /**
    * Pinta el listado de registros según el filtro
    * @returns {} 
    */
    function () {
     
        $.ajax({
            type: "POST",
            url: controlador + 'ListarIncumplimientos',
            data: {
                anho: $("#anho").val(),
                mes: $("#mes").val(),
                situacionEmpresa: $("#situacionEmpresa").val(),
                ruc: $("#documento").val(),
                razonSocial: $("#razonSocial").val()
            },
            success: function (evt) {

                $('#listadoIncumplimientos').css("width", $('#mainLayout').width() + "px");
                $('#listadoIncumplimientos').html(evt);
                $('#tablaIncumplimientos').dataTable({
                    "ordering": true,
                    "paging": false,
                    "scrollY": 340,
                    "scrollX": true,
                    "scrollCollapse": true,
                    "fixedColumns": true
                });

                mostrarMensaje("Búsqueda ejecutada correctamente.");
            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {
                mostrarError('No se puedo ejecutar la búsqueda.');
            }
        });
    };


function limpiarBusqueda() {
    $("#anho").val("");
    $("#mes").val("");
    $("#situacionEmpresa").val("1");
    $("#documento").val("");
    $("#razonSocial").val("");
}

var nuevoRegistro =
    function () {

        limpiarMensajepe();
       
        var div = document.getElementById('divEmpresaInc').style.display = 'block';
        document.getElementById('divEmpresaAfe').style.display = 'block';

        listarArchivos(0);

        $('#empresaAgenteEdit').val('');
        $('#empresaAgenteSeleccionada').val('');
        $('#tipoemprcodi').val('');
        $('#empresaSeleccionada').val('');
        $('#empresaEdit').val('');
        document.getElementById("anhoEdit").selectedIndex = 0;
        document.getElementById("mesEdit").selectedIndex = 0;
        $('#montoAfectadoEdit').val('');

        $('#codigoIncumplimiento').val('');
        $('#fechaRecepcionEdit').val('');
        document.getElementById("tipoInformeEdit").selectedIndex = 0;
        $('#archivo').val('');
        
        $('#esNuevo').val(1);
        // limpiar
        $("#popupEdicion").bPopup({
            autoClose: false
        });
    }

function modificarIncumplimientos(pIncucodi) {
    $('#esNuevo').val(0);
    document.getElementById('divEmpresaInc').style.display = 'none';
    document.getElementById('divEmpresaAfe').style.display = 'none';

    $('#fechaRecepcionEdit').val('');
    document.getElementById("tipoInformeEdit").selectedIndex = 0;
    $('#archivo').val('');

    listarArchivos(0);

    limpiarMensajepe();

    $.ajax({
        type: 'POST',
        url: controlador + 'ObtenerIncumplimiento',
        data: {
            incucodi: pIncucodi
        },
        dataType: 'json',
        success: function (data) {
            var jsonData = JSON.parse(data);
            $('#empresaSeleccionada').val(jsonData.EmprcodiEdit);
            $('#empresaEdit').val(jsonData.AfectadaEdit);
            $('#empresaAgenteSeleccionada').val(jsonData.EmpgcodiEdit);
            $('#empresaAgenteEdit').val(jsonData.IncumplidoraEdit);
            $('#tipoemprcodi').val(jsonData.TipoEmpresa);

            $('#anhoEdit').val(jsonData.IncuanioEdit);
            $('#mesEdit').val(jsonData.IncumesEdit);
            $('#montoAfectadoEdit').val(jsonData.IncumMontoEdit);

            listarArchivos(jsonData.IncucodiEdit);

            $('#esNuevo').val(0);
            $('#codigoIncumplimiento').val(jsonData.IncucodiEdit);

            $("#popupEdicion").bPopup({
                autoClose: false
            });
        },
        error: function () {
            mostrarMensaje('mensaje', 'Ha ocurrido un error', $tipoMensajeError, $modoMensajeCuadro);
        }
    });

}

function setAnio(aselect) {

    var d = new Date();
    var n = d.getFullYear();
    var select = document.getElementById(aselect);
    for (var i = 2018; i <= n + 1; i++) {
        var opc = document.createElement("option");
        opc.text = i;
        opc.value = i;
        select.add(opc)
    }
}

function listarArchivos(incucodi) {
    $.ajax({
        type: 'POST',
        url: controlador + "ListarArchivos",
        dataType: 'html',
        data: {
            incucodi: incucodi
        },
        success: function (aData) {
            $('#listadoIncumplimientoArchivos').html(aData);
            $('#tablaIncumplimientoArchivos').dataTable({
                "ordering": true,
                "paging": false,
                "scrollY": 340,
                "scrollX": true,
                "scrollCollapse": true,
                "fixedColumns": true
            });
        },
        error: function (XMLHttpRequest, textStatus, errorThrown) {
            mostrarAlertape("No es posible cargar la lista de Informes.");
        }
    });
}
function listaTipoInforme() {
    $.ajax({
        type: 'POST',
        url: controlador + "ListarTipoInforme",
        dataType: 'json',
        data: {},
        success: function (aData) {
           
            var _arr= aData.data;
            for(var idx in _arr){
                //console.log(_arr[idx]);
                //var opt = '<option value="' + item.TINFCODI + '">' + item.TINFINFORME + '</option>';
                
                var sel = document.getElementById('tipoInformeEdit');
                var opt = document.createElement('option');
                opt.appendChild(document.createTextNode(_arr[idx].TINFINFORME));
                opt.value = _arr[idx].TINFCODI;
                sel.appendChild(opt);
            }
        },
        error: function () {
            mostrarAlertape("No es posible obtener la lista de Tipos Informe.");
        }
    });
}


function guardarArchivo() {

    var codigoIncumplimiento = $('#codigoIncumplimiento').val();
    if (codigoIncumplimiento == "" || codigoIncumplimiento == null) {
        mostrarAlertape('Antes de ingresar un informe, debe registrar los datos del Incumplimiento.');
        return;
    }

    var tipoinforme = $("#tipoInformeEdit").val();
    if (tipoinforme == "0" || tipoinforme == null) {
        $("#tipoInformeEdit").focus();
        mostrarAlertape('Debe seleccionar un tipo de informe.');
        return;
    }

    var fecharecepcion = $('#fechaRecepcionEdit').val();
    if (fecharecepcion == "" || fecharecepcion == null) {
        $('#fechaRecepcionEdit').focus();
        mostrarAlertape('Debe ingresar la fecha de recepción.');
        return;
    }
    
    var nombarchivo = $('#archivo').val();
    if (nombarchivo == "" || nombarchivo == null) {
        $('#archivo').focus();
        mostrarAlertape('Debe seleccionar un archivo.');
        return;
    }

   
    var codigoIncumplimiento = $('#codigoIncumplimiento').val();

    $.ajax({
        type: 'POST',
        url: controlador + 'GuardarArchivo',
        data: {
            pIncucodigo: codigoIncumplimiento === "" ? 0 : codigoIncumplimiento,
            pTipoinforme: tipoinforme,
            pFecharecepcion: fecharecepcion,
            pArchivo: nombarchivo
        },
        dataType: 'json',
        success: function (result) {

            if (result.success) {
                //$('#popupEdicion').bPopup().close();
                pintarBusqueda();
                mostrarMensajepe('Se guardo el registro correctamente.');
            }
            else {
                mostrarMensajepe(result.message);
            }

            listarArchivos(codigoIncumplimiento === "" ? 0 : codigoIncumplimiento);

        },
        error: function () {
            mostrarAlertape('mensaje', 'Ha ocurrido un error', $tipoMensajeError, $modoMensajeCuadro);
        }
    });

    //limpiarGarantia();
}

function eliminarArchivo(detdinccodi) {

    $.ajax({
        type: 'POST',
        url: controlador + 'EliminarArchivo',
        data: {
            pDetdinccodi: detdinccodi
        },
        dataType: 'json',
        success: function (result) {

            if (result.success) {
                mostrarMensajepe("Se guardo el registro correctamente.");
            }
            else {
                mostrarMensajepe(result.message);
            }
            var codigoIncumplimiento = $('#codigoIncumplimiento').val();
            listarArchivos(codigoIncumplimiento === "" ? 0 : codigoIncumplimiento);
        },
        error: function () {
            mostrarAlertape('mensaje', 'Ha ocurrido un error', $tipoMensajeError, $modoMensajeCuadro);
        }
    });
}

function guardarEdicion() {
    var empagentecodi = $('#empresaAgenteSeleccionada').val();
    var empresaIncumplidora = $('#empresaAgenteEdit').val();
    var tipoemprcodi = $('#tipoemprcodi').val();

    if (empagentecodi == "" || empagentecodi == null || empresaIncumplidora == "") {
        mostrarAlertape('Debe seleccionar una empresa incumplidora.');
        return;
    }

    var empcodi = $('#empresaSeleccionada').val();
    var empresa = $('#empresaEdit').val();
    if (empcodi == "" || empcodi == null || empresa == "") {
        mostrarAlertape('Debe seleccionar una empresa afectada.');
        return;
    }

    var anhoEdit = $("#anhoEdit").val();
    if (anhoEdit == "" || anhoEdit == null) {
        mostrarAlertape('Debe seleccionar el año del periodo.');
        return;
    }

    var mesEdit = $("#mesEdit").val();
    if (mesEdit == "" || mesEdit == null) {
        mostrarAlertape('Debe seleccionar el mes del periodo.');
        return;
    }

    var esNuevo = $('#esNuevo').val() == 1 ? true : false;
    var codigoIncumplimiento = $('#codigoIncumplimiento').val();

    $.ajax({
        type: 'POST',
        url: controlador + 'GuardarIncumplimiento',
        data: {
            codigoIncumplimiento: codigoIncumplimiento === "" ? 0 : codigoIncumplimiento,
            empresaAgente: empagentecodi,
            tipoemprcodi: tipoemprcodi,
            empresa: empcodi,
            anho: anhoEdit,
            mes: mesEdit,
            montoAfectado: $('#montoAfectadoEdit').val(),
            EsNuevo: esNuevo
        },
        dataType: 'json',
        success: function (result) {

            if (result.success) {
                $('#popupEdicion').bPopup().close();
                mostrarMensajepe("Se guardo el registro correctamente.");
                pintarBusqueda();
            }
            else {
                mostrarMensajepe(result.message);
            }

        },
        error: function () {
            mostrarAlertape('mensaje', 'Ha ocurrido un error', $tipoMensajeError, $modoMensajeCuadro);
        }
    });
}
function cancelarEdicion() {
    $('#popupEdicion').bPopup().close();
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

function mostrarErrorpe(mensaje) {
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

function muestraBuscarEmpresaAgente() {
    $("#empresaAgenteBuscar").val('');
    $("#listadoEmpresasAgente").html("");
    $("#popupBuscarEmpresaAgente").bPopup({
        autoClose: false
    });
}
function buscarEmpresaAgente() {
    var empresaIngresada = document.getElementById("empresaAgenteBuscar").value;

    $.ajax({
        type: 'POST',
        datatype: 'json',
        url: controlador + "ListarEmpresasAgentes",
        data: {
            razonsocial: empresaIngresada
        },
        success: function (result) {
            $('#listadoEmpresasAgente').css("width", "90%");
            $('#listadoEmpresasAgente').html(result);
            $('#tablaListaEmpresasAgente').dataTable({
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
            $('#btnSeleccionarEmpresaAgente').click(function () {
                seleccionarEmpresaAgente();
            });
        },
        error: function () {
            mostrarAlertape("Error al cargar la Lista de Empresas Agente.");
        }
    });
}
function seleccionarEmpresaAgente() {
    var empresa = $('input:radio[name=codEmpresaAgente]:checked').val();
    if (empresa == "" || empresa == null) {
        mostrarAlertape('No se ha seleccionado una empresa Agente');
        return false;
    }
    var datos = empresa.split('/');
    document.getElementById("empresaAgenteEdit").value = datos[1];
    document.getElementById("empresaAgenteSeleccionada").value = datos[0];
    document.getElementById("tipoemprcodi").value = datos[2];
    
    $('#popupBuscarEmpresaAgente').bPopup().close();

    //luego de seleccionar la empresa se debe de cargar la lista de punto de medicion segun el tipo de empresa
    //listarPuntoMedicion(datos[0], 0);
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
            mostrarErrorpe("Ocurrió un error al obtener la Lista de Empresas.");
        }
    });
}
function seleccionarEmpresa() {
    var empresa = $('input:radio[name=codEmpresa]:checked').val();
    if (empresa == "" || empresa == null) {
        mostrarAlertape('No se ha seleccionado una empresa');
        return false;
    }
    var datos = empresa.split('/');
    document.getElementById("empresaEdit").value = datos[1];
    document.getElementById("empresaSeleccionada").value = datos[0];

    $('#popupBuscarEmpresa').bPopup().close();

}

function descargarFile(fileName) {
    controler = siteRoot + "GMM/Incumplimientos/descarga";
    var paramList = [
        { tipo: 'input', nombre: 'fileName', value: fileName }
    ];
    var form = CreateForm(controler, paramList);
    document.body.appendChild(form);
    form.submit();
    return true;
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
        url: siteRoot + 'GMM/Incumplimientos/upload',
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
                mostrarAlerta(err.code + "-" + err.message);
            }
        }
    });

    uploader.init();
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

Date.prototype.yyyymmddhhmmss =
    function () {
        var yyyy = this.getFullYear();
        var mm = this.getMonth() < 9 ? "0" + (this.getMonth() + 1) : (this.getMonth() + 1); // getMonth() is zero-based
        var dd = this.getDate() < 10 ? "0" + this.getDate() : this.getDate();
        var hh = this.getHours() < 10 ? "0" + this.getHours() : this.getHours();
        var min = this.getMinutes() < 10 ? "0" + this.getMinutes() : this.getMinutes();
        var ss = this.getSeconds() < 10 ? "0" + this.getSeconds() : this.getSeconds();
        return "".concat(yyyy).concat(mm).concat(dd).concat(hh).concat(min).concat(ss);
    };
loadInfoFile =
    function (fileName, fileSize) {
        mostrarAlerta(fileName + " (" + fileSize + ")");
    }
loadValidacionFile =
    function (mensaje) {
        mostrarAlerta(mensaje);
    }