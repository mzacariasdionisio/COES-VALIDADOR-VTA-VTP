var controler = siteRoot + 'eventos/registro/';
var fileBitacora = [];

$(function () {

    $('#txtHoraInicial').inputmask({
        mask: "1/2/y h:s:s",
        placeholder: "dd/mm/yyyy hh:mm:ss",
        alias: "datetime",
        hourFormat: "24"
    });

    $('#txtHoraInicial').Zebra_DatePicker({        
        readonly_element: false,        
        onSelect: function (date) {            
            $('#txtHoraInicial').val(date + " 00:00:00");
        }
    });

    $('#txtHoraFinal').inputmask({
        mask: "1/2/y h:s:s",
        placeholder: "dd/mm/yyyy hh:mm:ss",
        alias: "datetime",
        hourFormat: "24"
    });

    $('#txtHoraFinal').Zebra_DatePicker({
        readonly_element: false,
        onSelect: function (date) {
            $('#txtHoraFinal').val(date + " 00:00:00");
        }
    });

    $('#cbTipoEvento').change(function () {
        cargarSubCausaEvento();
    });

    $('#btnBuscarEquipo').click(function () {
        openBusquedaEquipo();
    });

    $('#btnGrabar').click(function () {
        grabar();
    });

    $('#btnCancelar').click(function () {
        document.location.href = siteRoot + 'eventos/evento/index';
    });

    $('#btnConvertir').click(function () {
        convertir();
    });

    cargarPrevio();       
    cargarBusquedaEquipo();
    cargarContenido();
});


cargarPrevio = function ()
{
    $('#cbTipoEvento').val($('#hfTipoEvento').val());
    $('#cbSubCausaEvento').val($('#hfSubCausaEvento').val());
    $('#cbEmpresaEvento').val($('#hfIdEmpresaEvento').val());
    $('#cbTipoOperacion').val($('#hfTipoOperacion').val());
        
    if ($('#hfCodigoEvento').val() != '0') {
        $('#btnConvertir').css('display', 'block');
    }
    else {
        $('#btnConvertir').css('display', 'none');
    }
}

cargarSubCausaEvento = function () {

    if ($('#cbTipoEvento').val() != "") {
        $.ajax({
            type: 'POST',
            url: controler + 'cargarsubcausaevento',
            dataType: 'json',
            data: { idTipoEvento: $('#cbTipoEvento').val() },
            cache: false,
            success: function (aData) {
                $('#cbSubCausaEvento').get(0).options.length = 0;
                $('#cbSubCausaEvento').get(0).options[0] = new Option("-SELECCIONE-", "");
                $.each(aData, function (i, item) {
                    $('#cbSubCausaEvento').get(0).options[$('#cbSubCausaEvento').get(0).options.length] =
                        new Option(item.Subcausadesc, item.Subcausacodi);
                });
            },
            error: function () {
                mostrarError();
            }
        });
    }
    else {
        $('#cbSubCausaEvento').get(0).options.length = 0;
    }
}

cargarBusquedaEquipo = function ()
{
    $.ajax({
        type: "POST",
        url: siteRoot + "eventos/busqueda/index",
        data: {},
        global:false,
        success: function (evt) {
            $('#busquedaEquipo').html(evt);
            if ($('#hfCodigoEvento').val() == "0") {
                openBusquedaEquipo();
            }
        },
        error: function (req, status, error) {
            mostrarError();
        }
    });
}

openBusquedaEquipo = function ()
{
    $('#busquedaEquipo').bPopup({
        easing: 'easeOutBack',
        speed: 450,
        transition: 'slideDown'
    });
    $('#txtFiltro').focus();
}

seleccionarEquipo = function (idEquipo, substacion, equipo, empresa, idEmpresa) {

    $('#cbEmpresaEvento').val(idEmpresa);
    $('#spanEquipo').text(substacion + ' ' + equipo);
    $('#busquedaEquipo').bPopup().close();
    $('#hfIdEquipo').val(idEquipo);

    $('#busquedaEquipo').bPopup().close();
}

grabar = function () {
    //$("#hdArchivoAdicional").val($("#flArchivoAdicional")[0].files[0]);
    var mensaje = validarRegistro();
    if (mensaje == "") {

        $.ajax({
            type: 'POST',
            url: controler + "grabarbitacora",
            dataType: 'json',
            data: $('#frmRegistro').serialize(),
            success: function (result) {
                if (result > 1) {
                    mostrarExito();
                    $('#hfCodigoEvento').val(result);
                    $('#btnConvertir').css('display', 'block');

                }
                if (result == -1) {
                    mostrarError();
                }
                if (result == -2) {
                    mostrarAlerta("La fecha inicial debe ser menor a la final.");
                }
            },
            error: function () {
                mostrarError();
            }
        });
    }
    else {
        mostrarAlerta(mensaje);
    }
}

validarRegistro = function () {

    var mensaje = "<ul>";
    var flag = true;
    if ($('#cbTipoEvento').val() == '') {
        mensaje = mensaje + "<li>Seleccione el tipo de evento.</li>";
        flag = false;
    }

    if ($('#txtHoraInicial').val() == '') {
        mensaje = mensaje + "<li>Ingrese la hora inicial.</li>";
        flag = false;
    }
    else {
        if (!$('#txtHoraInicial').inputmask("isComplete")) {
            mensaje = mensaje + "<li>Ingrese hora inicial.</li>";
            flag = false;
        }
    }

    if ($('#txtHoraFinal').val() == '') {
        mensaje = mensaje + "<li>Ingrese la hora final.</li>";
        flag = false;
    }
    else {
        if (!$('#txtHoraFinal').inputmask("isComplete")) {
            mensaje = mensaje + "<li>Ingrese hora final.</li>";
            flag = false;
        }
    }

    if ($('#txtDescripcion').val() == '') {
        mensaje = mensaje + "<li>Ingrese la descripción del evento.</li>";
        flag = false;
    }

    if ($('#cbSubCausaEvento').val() == '') {
        mensaje = mensaje + "<li>Seleccione la causa del evento.";
        flag = false;
    }
   
    mensaje = mensaje + "</ul>";

    if (flag) mensaje = "";      

    return mensaje;
}

convertir = function () {
    if (confirm('¿Está seguro de realizar esta operación?')) {
        $.ajax({
            type: 'POST',
            url: controler + "cambiarversion",
            dataType: 'json',
            data: {
                idEvento: $('#hfCodigoEvento').val()
            },
            success: function (result) {
                if (result == 1) {
                    document.location.href = controler + 'final?id=' + $('#hfCodigoEvento').val();
                }
                else if (result == 2) {
                    mostrarAlerta("Solo se pueden convertir a finales las bitácoras con tipo EVENTO o FALLA.")
                }
                else {
                    mostrarError();
                }
            },
            error: function () {
                mostrarError();
            }
        });
    }
}

mostrarError = function () {
    alert("Ha ocurrido un error");
}

mostrarAlerta = function (mensaje) {
    $('#mensaje').removeClass();
    $('#mensaje').addClass('action-alert');
    $('#mensaje').html(mensaje);
}

mostrarExito = function () {
    $('#mensaje').removeClass();
    $('#mensaje').addClass('action-exito');
    $('#mensaje').html('La operación se realizó con éxito.');
}

cargarContenido = function () {

    uploader = new plupload.Uploader({
        runtimes: 'html5,flash,silverlight,html4',
        browse_button: 'btnSelectFile',
        container: document.getElementById('container'),
        url: controler + 'Upload',
        flash_swf_url: '~/Content/Scripts/Moxie.swf',
        silverlight_xap_url: '~/Content/Scripts/Moxie.xap',
        multi_selection: false,
        chunk_size: '100mb',
        filters: {
            max_file_size: '100mb',
            mime_types: [
                { title: "Imagenes", extensions: "jpg,jpeg,gif,png" },
                { title: "Archivos comprimidos", extensions: "zip,rar" },
                { title: "Documentos", extensions: "pdf,doc,docx,xls,xlsx,ppt,pptx,csv" }
            ]
        },
        init: {
            PostInit: function () {
                if ($('#hfArchivoAdjunto').val() != "") {
                    $('#container').css('display', 'contents');
                    var files = $('#hfArchivoAdjunto').val().split(";");
                    for (i = 0; i < $('#hfArchivoAdjunto').val().split(";").length; i++) {
                        var cod = $('#hfCodigoEvento').val() + "-" + i;
                        fileBitacora.push({ id: cod, name: files[i] });
                    }
                } else $('#container').css('display', 'none');                
            },
            FilesAdded: function (up, files) {
                $('#container').css('display', 'contents');

                if (uploader.files.length > 0) {
                    
                    $('#filelist').html($('#filelist').html() + '<div class="file-name" id="' + files[0].id + '"><a href="JavaScript:downloadFileB(\'' + files[0].name + '\');">' + files[0].name +
                            '</a> <a class="remove-item" href="JavaScript:eliminarFile_(' +
                        '\'' + files[0].id + '\');">X</a> <b></b></div>');

                    $('#hfArchivoAdjunto').val($('#hfArchivoAdjunto').val() + ($('#hfArchivoAdjunto').val() != "" ? ";" : "") + files[0].name);
                }

                uploader.start();
            },
            UploadProgress: function (up, file) {
                document.getElementById(file.id).getElementsByTagName('b')[0].innerHTML = '<span>' + file.percent + "%</span>";
                $('#' + file.id).css('background-color', '#FFFFB7');
            },
            UploadComplete: function (up, file) {
                $('#loadingcarga').css('display', 'none');
            },
            Error: function (up, err) {
                $('#container').css('display', 'none');
                $('#filelist').html('<div class="action-alert">' + err.message + '</div>');
                $('#btnSelectFile').css('display', 'none');
            }
        }



    });
    uploader.init();

    eliminarFile = function (id) {
        $('#' + id).remove();
        fileBitacora.splice(fileBitacora.findIndex(v => v.id === id), 1);
        $('#hfArchivoAdjunto').val("");
        for (i = 0; i < fileBitacora.length; i++) {
            var file = fileBitacora[i];
            $('#hfArchivoAdjunto').val($('#hfArchivoAdjunto').val() + ($('#hfArchivoAdjunto').val() != "" ? ";" : "") + file.name);
        }
        for (i = 0; i < uploader.files.length; i++) {
            var file = uploader.files[i];
            $('#hfArchivoAdjunto').val($('#hfArchivoAdjunto').val() + ($('#hfArchivoAdjunto').val() != "" ? ";" : "") + file.name);
        }
    }

    eliminarFile_ = function (id) {
        $('#' + id).remove();
        uploader.removeFile(id);
        $('#hfArchivoAdjunto').val("");
        for (i = 0; i < fileBitacora.length; i++) {
            var file = fileBitacora[i];
            $('#hfArchivoAdjunto').val($('#hfArchivoAdjunto').val() + ($('#hfArchivoAdjunto').val() != "" ? ";" : "") + file.name);
        }
        for (i = 0; i < uploader.files.length; i++) {
            var file = uploader.files[i];
            $('#hfArchivoAdjunto').val($('#hfArchivoAdjunto').val() + ($('#hfArchivoAdjunto').val() != "" ? ";" : "") + file.name);
        }
    }

    downloadFileB = function (name) {
        window.location.href = controler + "DownloadFileBitacora?name=" + name;
    }

    //return uploader;
}