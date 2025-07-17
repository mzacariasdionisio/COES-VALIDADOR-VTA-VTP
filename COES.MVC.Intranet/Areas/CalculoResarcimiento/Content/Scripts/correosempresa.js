var controlador = siteRoot + 'CalculoResarcimiento/Correo/';

var accion;

const NUEVO = 1;
const EDITAR = 2;
const IMG_EDITAR = '<img src="' + siteRoot + 'Content/Images/btn-edit.png" alt="Editar Envío" width="19" height="19" style="">';
const IMG_VER = '<img src="' + siteRoot + 'Content/Images/btn-open.png" alt="Ver Envío" width="19" height="19" style="">';
const IMG_ELIMINAR = '<img src="' + siteRoot + 'Content/Images/btn-cancel.png" alt="Ver Envío" width="19" height="19" style="">';


$(function () {
    $('#btnNuevo').click(function () {        
        limpiarPopupCorreos();
        document.getElementById("cbEmpresa").disabled = false;       
        accion = NUEVO;
        abrirPopup("popupEdicion");
    });

    $('#btnGrabarIngreso').click(function () {
        guardarEmpresaCorreo();
    });

    $('#btnExportar').click(function () {
        exportarListado();
    });

    $("#btnImportar").click(function () {
        limpiarBarraMensaje("mensaje");
        limpiarBarraMensaje("mensaje_popupImportar");
        $("#txtNombreArchivo").hide();
        $("#txtNombreArchivo").html('');
        $('#listadoErroresExcel').html('');
        $('#btnGrabarArchivoEmpresasCorreo').css('display', 'none');
        abrirPopup("popupCargarDesdeArchivo");
    });

    $('#btnGrabarArchivoEmpresasCorreo').click(function () {
        uploader.start();
    });

    AdjuntarArchivo();

    mostrarListadoEmpresaCorreos();
});

function mostrarListadoEmpresaCorreos() {
    limpiarBarraMensaje("mensaje");

    $.ajax({
        type: 'POST',
        url: controlador + "listarEmpresaCorreos",
        data: {},
        success: function (evt) {
            if (evt.Resultado != "-1") {
                var htmlEC = dibujarTablaEmpresaCorreo(evt.ListaEmpresasCorreo, evt.Grabar);
                $("#listado").html(htmlEC);

                $('#tablaEmpresaC').dataTable({
                    "scrollY": 480,
                    "scrollX": false,
                    "sDom": 't',
                    "ordering": false,
                    "iDisplayLength": -1
                });

            } else {
                mostrarMensaje('mensaje', 'error', evt.Mensaje);
            }
        },
        error: function (err) {
            mostrarMensaje('mensaje', 'error', 'Ha ocurrido un error.');
        }
    });
}


function dibujarTablaEmpresaCorreo(listaEmpresas, grabar) {

    var cadena = '';
    cadena += `
    <table border="0" class="pretty tabla-icono" id="tablaEmpresaC" >
       <thead>
           <tr style="height: 22px;">
               <th style='width:90px;'>Acciones</th>
               <th>Empresa</th>
               <th>Correos</th>
               <th>Usuario Modificación</th>
               <th>Fecha de Modificación</th>
            </tr>
        </thead>
        <tbody>
    `;

    for (key in listaEmpresas) {
        var item = listaEmpresas[key];

        if (grabar) {

            cadena += `
            <tr>
                <td style='width:90px;'>        
                    <a href="JavaScript:editarEmpresa(${item.Emprcodi});">${IMG_EDITAR}</a>
                    <a href="JavaScript:eliminarEmpresa(${item.Emprcodi});">${IMG_ELIMINAR}</a>
                </td>
                <td>${item.Emprnomb}</td>
                <td>${item.LstCorreos}</td>
                <td>${item.UsuarioModificacion}</td>
                <td>${item.FechaModificacionDesc}</td>
           </tr>           
        `;
        }
        else {
            cadena += `
            <tr>
                <td style='width:90px;'>        
                    <a href="JavaScript:editarEmpresa(${item.Emprcodi});">${IMG_EDITAR}</a>                   
                </td>
                <td>${item.Emprnomb}</td>
                <td>${item.LstCorreos}</td>
                <td>${item.UsuarioModificacion}</td>
                <td>${item.FechaModificacionDesc}</td>
           </tr>           
        `;
        }

    }

    cadena += `                   
        </tbody >
    </table >
    `;

    return cadena;
}

function limpiarPopupCorreos() {
    limpiarBarraMensaje("mensaje_popupCorreo");    
    $("#cbEmpresa").val("-1");

    let data = [];

    //Quitamos los que se crearon
    $('.all-mail').remove(); 
    $('.enter-mail-id').remove();
   
    $("#txtEmail").email_multiple({
        //data: data,
        reset: true
    });
    $("#txtEmail").val("");
}


function cargarCorreosPorEmpresa() {
    limpiarBarraMensaje("mensaje_popupCorreos");

    var empresaId = parseInt($("#cbEmpresa").val());
    $.ajax({
        type: 'POST',
        url: controlador + 'ObtenerCorreosPorEmpresa',
        data: {
            emprcodi: empresaId
        },
        dataType: 'json',
        //global: false,
        success: function (evt) {
            if (evt.Resultado != "-1") {                
                let lstCorreos = evt.ListaCorreosPorEmpresa;

                if (lstCorreos != "") {
                    var data = lstCorreos.split(',');
                    $("#txtEmail").email_multiple({
                        data: data
                        // reset: true
                    });
                }

                if (accion == EDITAR)
                    abrirPopup("popupEdicion");
                
            }
            else {
                mostrarMensaje('mensaje_popupCorreo', 'error', 'Se ha producido un error.');
            }
        },
        error: function () {
            mostrarMensaje('mensaje_popupCorreo', 'error', 'Se ha producido un error.');
        }
    });

    
}


function editarEmpresa(emprcodi) {
    limpiarBarraMensaje("mensaje");
    limpiarBarraMensaje("mensaje_popupCorreo");

    accion = EDITAR;
   
    //Quitamos los que se crearon
    $('.all-mail').remove();
    $('.enter-mail-id').remove();

    $('#cbEmpresa').val(emprcodi);    
    document.getElementById("cbEmpresa").disabled = true;
    
    cargarCorreosPorEmpresa();    
}

function eliminarEmpresa(emprcodi) {
    if (confirm('¿Desea eliminar el registro seleccionado?')) {

        $.ajax({
            type: 'POST',
            url: controlador + 'EliminarEmpresaCorreos',
            data: {
                emprcodi: emprcodi
            },
            dataType: 'json',
            success: function (evt) {
                if (evt.Resultado == "1") {
                    mostrarListadoEmpresaCorreos();
                    mostrarMensaje('mensaje', 'exito', 'El registro se eliminó correctamente.');
                }
                else {
                    mostrarMensaje('mensaje', 'error', 'Error: ' + evt.Mensaje);
                }
            },
            error: function () {
                mostrarMensaje('mensaje', 'error', 'Se ha producido un error.');
            }
        });
    }
}

function guardarEmpresaCorreo() {
    limpiarBarraMensaje("mensaje_popupCorreo");

    var data = {};
    data = getDataGuardar();

    var msg = validarCamposDatosAGuardar(data);

    if (msg == "") {
        $.ajax({
            type: 'POST',
            url: controlador + 'GuardarEmpresaCorreos',            
            dataType: 'json',
            
            data: {
                accion: accion,
                empresaId: data.Empresa,
                correos: data.ListaCorreos
            },
            cache: false,
            success: function (evt) {
                if (evt.Resultado != "-1") {
                    cerrarPopup("popupEdicion");

                    mostrarListadoEmpresaCorreos();

                } else {
                    mostrarMensaje('mensaje_popupCorreo', 'error', evt.Mensaje);
                }
            },
            error: function (err) {
                mostrarMensaje('mensaje_popupCorreo', 'error', 'Ha ocurrido un error.');
            }
        });
    } else {
        mostrarMensaje('mensaje_popupCorreo', 'error', msg);
    }
}

function getDataGuardar() {
    var obj = {};

    //otros
    obj.Empresa = parseInt($("#cbEmpresa").val());
    obj.ListaCorreos = $("#txtEmail").val();
          

    return obj;
}


function validarCamposDatosAGuardar(data) {
    var msj = "";

    if (data.Empresa == -1) {
        msj += "<p>Seleccione una empresa correcta.</p>";
    }

    if (data.ListaCorreos == "") {
        msj += "<p>Ingrese como mínimo un correo con formato válido y presiona Enter para añadirlo.</p>";
    }    


    return msj;
}


function exportarListado() {
    limpiarBarraMensaje("mensaje");

    $.ajax({
        type: 'POST',
        url: controlador + 'ExportarListado',
        data: {},
        dataType: 'json',
        success: function (evt) {
            if (evt.Resultado != "-1") {
                window.location = controlador + "Exportar?file_name=" + evt.Resultado;
            } else {
                mostrarMensaje('mensaje', 'error', 'Error: ' + evt.Mensaje);
            }
        },
        error: function (err) {
            mostrarMensaje('mensaje', 'error', 'Ha ocurrido un error.');
        }
    });
}

function AdjuntarArchivo() {

    uploader = new plupload.Uploader({
        runtimes: 'html5,flash,silverlight,html4',
        browse_button: "btnSeleccionarArchivo",

        url: controlador + "GuardarCorreosDesdeArchivo",
        multi_selection: false,
        filters: {
            max_file_size: '2mb',
            mime_types: [
                { title: "Archivos xls", extensions: "xls,xlsx" },
            ]
        },
        multipart_params: {},
        init: {
            FilesAdded: function (up, files) {
                if (uploader.files.length === 2) {
                    uploader.removeFile(uploader.files[0]);
                }

                $("#txtNombreArchivo").css('display', 'inline-block');
                for (i = 0; i < files.length; i++) {
                    var file = files[i];
                    $("#txtNombreArchivo").html(file.name);
                }

                $('#btnGrabarArchivoEmpresasCorreo').css('display', 'block');
                $('#listadoErroresExcel').html('');
                limpiarBarraMensaje("mensaje");
                limpiarBarraMensaje("mensaje_popupImportar");
            },
            UploadProgress: function (up, file) {
                mostrarMensaje('mensaje_popupImportar', 'alert', "Procesando Archivo, por favor espere ...(<strong>" + (file.percent - 1) + "%</strong>)");
                $('#listadoErroresExcel').html('');

                $('#btnGrabarArchivoEmpresasCorreo').css('display', 'none');
            },

            FileUploaded: function (up, file, info) {
                var aData = JSON.parse(info.response);
                if (aData.Resultado != '-1') {
                    mostrarListaErrores(aData);

                    if (aData.Resultado != "1") {
                        mostrarMensaje('mensaje_popupImportar', 'alert', "El archivo seleccionado tuvo los siguientes errores.");
                    } else {
                        
                        mostrarListadoEmpresaCorreos();
                        cerrarPopup('popupCargarDesdeArchivo');
                        mostrarMensaje('mensaje', 'exito', "El grabado de correo(s) fue éxitoso.");
                    }

                } else {
                    mostrarMensaje('mensaje_popupImportar', 'alert', aData.Mensaje);
                }

                $('#btnGrabarArchivoEmpresasCorreo').css('display', 'none');
            },
            UploadComplete: function (up, file) { 
                var sdfs = 0;
            },
            Error: function (up, err) {
                if (err.message == "File extension error.") {
                    mostrarMensaje('mensaje_popupImportar', 'alert', "El archivo seleccionado no tiene extension .xls o .xlsx.");
                } else {
                    mostrarMensaje('mensaje_popupImportar', 'alert', err.message);
                }
                $('#btnGrabarArchivoEmpresasCorreo').css('display', 'none');

            }
        }
    });

    uploader.init();
}

function mostrarListaErrores(aData) {
    $('#listadoErroresExcel').html('');

    //Tabla de errores
    if (aData.Resultado != '1') {
        $('#listadoErroresExcel').html(aData.Resultado);
    }
}


function cerrarPopup(id) {
    $("#" + id).bPopup().close()
}

function abrirPopup(contentPopup) {
    setTimeout(function () {
        $("#" + contentPopup).bPopup({
            easing: 'easeOutBack',
            speed: 450,
            transition: 'slideDown',
            modalClose: false
        });
    }, 50);
}


function mostrarMensaje(id, tipo, mensaje) {
    $("#" + id).css("display", "block");
    $('#' + id).removeClass();
    $('#' + id).addClass('action-' + tipo);
    $('#' + id).html(mensaje);

}

function limpiarBarraMensaje(id) {
    $('#' + id).css("display", "none");
    $('#' + id).removeClass();
    $('#' + id).addClass('action-message');
    $('#' + id).html('');
}

