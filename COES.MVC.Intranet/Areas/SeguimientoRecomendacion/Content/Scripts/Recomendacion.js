var controladorBase = siteRoot + 'seguimientorecomendacion/recomendacion/';
var controladorComentario = siteRoot + 'seguimientorecomendacion/comentario/';
var controladorRecGestion = siteRoot + 'seguimientorecomendacion/gestion';

var uploader;
var uploaderCom;

$(function () {

    $('#txtFechaDesde').Zebra_DatePicker({
    });

    $('#txtFechaHasta').Zebra_DatePicker({
    });
    
    $('#btnRecNuevo').click(function () {
        editar(0,1);
    });



    $('#btnBuscar').click(function () {
        buscar();
    });

    $('#btnRecRecuperar').click(function () {
        buscar();
    });



    $(document).ready(function () {
        
        
        $('#rbRecActivo').prop('checked', true);
        $('#rbComActivo').prop('checked', true);

        buscar();

       
        deshabilitaComentario();

        $('#btnComNuevo').click(function () {
            if (($('#hfSrmreccodiEquipo').val() + "*") == "*") {
                $('#tab-container').easytabs('select', '#Recomendacion');
            }
            else {
                editarCom(0, 1, $('#hfSrmreccodiEquipo').val());
            }
        });


        crearUpload();
        crearUploadCom();

        $('#btnCancelarPag').click(function () {            
            document.location.href = controladorRecGestion;
        });
        
    });



    $('#tab-container').easytabs({
        animate: false
    });



    var tabs = $('#tab-container');
    tabs.easytabs({ animate: false });
    tabs.bind("easytabs:before", function (e, clicked) {
        if (clicked.parent().hasClass('disabled')) {
            return false;
        }
    });
});

buscar = function () {
    pintarPaginado1();
    mostrarListado(1);
}

buscarCom = function () {
    
    
    var id = $('#hfSrmreccodiEquipo').val();
    if ((id + '*') != '*') {
        
        ver(id, 1, -1, -1, -1)
    }
    else {
        alert('Previamente debe seleccionar una Recomendación...')
    }
}

pintarPaginadoCom = function (id) {
    
    var activo = obtenerComentarioActivo();
    
    $.ajax({
        type: 'POST',
        url: controladorComentario + "paginadoCom",
        data: {
            srmreccodi: id,
            activo: activo
        },
        success: function (evt) {
            $('#ComPaginado').html(evt);
            mostrarPaginado();
        },
        error: function () {
            mostrarError();
        }
    });
}

pintarPaginado1 = function () {

    var activo = obtenerRecomendacionActiva();
    var evencodi = obtenerId(window.location);


    $.ajax({
        type: 'POST',
        url: controladorBase + "paginado",
        data: {
            evenCodi: evencodi,
            activo: activo
        },
        success: function (evt) {            
            $('#RecPaginado').html(evt);
            mostrarPaginado();
        },
        error: function () {
            mostrarError();
        }
    });
}

pintarBusqueda = function (nroPagina) {
    mostrarListado(nroPagina);
}

obtenerRecomendacionActiva = function () {
    var activo = "N";

    if ($('#rbRecActivo').is(':checked')) {
        activo = 'S';
    }
    else {
        activo = 'N';
    }

    return activo;
}

obtenerId = function (url) {
    var str = "aaa"+url+"";
    var n = str.search("id=");
    var cad1 = str.substring(n + 3, 999)

    var n2 = cad1.search("#");
    if (n2 > 0)
    {
        cad1 = cad1.substr(0,n2)
    }

    return(cad1)
}



mostrarListado = function (nroPagina) {
    var activo = obtenerRecomendacionActiva();

    var evencodi = obtenerId(window.location);
    $('#hfNroPagina').val(nroPagina);
    $.ajax({
        type: 'POST',
        url: controladorBase + "lista",
        data: {            
            evenCodi: evencodi,
            activo: activo,            
            nroPage: nroPagina
        },
        success: function (evt) {
            $('#RecomendDetalle').html(evt);


        },
        error: function () {
            mostrarError();
        }
    });
}

activarRec= function (id, activo)
{
    var activar = activo == 'S' ? 'activar' : 'eliminar';


    if (confirm('¿Está seguro de ' + activar + ' este registro?')) {

        $.ajax({
            type: 'POST',
            url: controladorBase + "activar",
            dataType: 'json',
            cache: false,
            data: {
                id: id,
                activo:activo
            },
            success: function (resultado) {
                if (resultado == 1) {
                    buscar();
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

activarCom = function (id, activo) {
    var activar = activo == 'S' ? 'activar' : 'eliminar';


    if (confirm('¿Está seguro de ' + activar + ' este registro?')) {

        $.ajax({
            type: 'POST',
            url: controladorComentario + "activar",
            dataType: 'json',
            cache: false,
            data: {
                id: id,
                activo: activo
            },
            success: function (resultado) {
                if (resultado == 1) {
                    buscarCom();
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

//recomendacion
editar = function (id, accion) {

    var evencodi = obtenerId(window.location);
    
    $.ajax({
        type: 'POST',
        url: controladorBase + "editar",
        cache: false,
        data: {
            id: id,
            accion: accion,
            evencodi: evencodi
        },
        global: false,
        success: function (evt) {
            $('#contenidoEdicion').html(evt);
            setTimeout(function () {
                $('#popupEdicion').bPopup({
                    easing: 'easeOutBack',
                    speed: 450,
                    transition: 'slideDown'
                });
            }, 50);

            configurarRecomendacion();
        }
    });

}


obtenerComentarioActivo = function () {
    var activo = "N";

    if ($('#rbComActivo').is(':checked')) {
        activo = 'S';
    }
    else {
        activo = 'N';
    }

    return activo;
}


//ver listado de comentarios
ver = function (id, nroPagina, empr, ssee, equipo) {
    //pintarPaginadoCom(id);
    if (ssee != -1 && empr != -1) {
        $('#txtEmpresaEquipo').val('');
        $('#txtSubestacionEquipo').val('');
        $('#txtEquipo').val('');
        $('#hfSrmreccodiEquipo').val('');
    }

    var activo = obtenerComentarioActivo();
    $('#hfNroPaginaCom').val(nroPagina);
    $.ajax({
        type: 'POST',
        url: controladorComentario + "lista",
        cache: false,
        data: {
            srmreccodi: id,
            activo: activo,
            nroPage: nroPagina
        },
        success: function (evt) {
            $('#ComentDetalle').html(evt);

            var tabs = $('#tab-container');
            disable_easytabs(tabs, []);
            pintarPaginadoCom(id);
            //ir a la lista
            $('#tab-container').easytabs('select', '#Comentario');
            
            if (ssee != -1 && empr != -1) {
                $('#txtEmpresaEquipo').val(empr);
                $('#txtSubestacionEquipo').val(ssee);
                $('#txtEquipo').val(equipo);
                $('#hfSrmreccodiEquipo').val(id);
            }

        },
        error: function () {
            mostrarError();
        }
    });

}

configurarRecomendacion = function () {


    $(document).ready(function () {


        $('#cbSrmstdcodi').val($('#hfSrmstdcodi').val());
        $('#cbSrmcrtcodi').val($('#hfSrmcrtcodi').val());
        $('#cbUsercode').val($('#hfUsercode').val());

        if ($('#hfAccion').val() == 0) {
            $('#btnGrabar').hide();
        }
        
    });

    cargarBusquedaEquipo();

    $('#btnGrabar').click(function () {
        grabar();
    });

    $('#btnCancelar').click(function () {
        $('#popupEdicion').bPopup().close();
        buscar();
    });


    $('#btnBuscarEquipo').click(function () {
        openBusquedaEquipo();
    });

   

}




eliminar = function (id) {

    if (confirm('¿Está seguro de eliminar este registro?')) {

        $.ajax({
            type: 'POST',
            url: controladorBase + "eliminar",
            dataType: 'json',
            cache: false,
            data: { id: id },
            success: function (resultado) {
                if (resultado == 1) {
                    buscar();
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

    alert('Ha ocurrido un error.');
}

// editar
grabar = function () {

    var mensaje = validarRegistro();

    if (mensaje == "") {

        $.ajax({
            type: 'POST',
            url: controladorBase + "grabar",
            dataType: 'json',
            data: $('#form').serialize(),
            success: function (result) {
                if (result != "-1") {

                    //id
                    $('#hfSrmrecCodi').val(result);
 
                    document.getElementById('btnCargarArchivo').click();

                    //finalizando
                    $('#popupEdicion').bPopup().close();
                    buscar();
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
    else {
        mostrarAlerta(mensaje);
    }
}

mostrarAlerta = function (mensaje) {

    $('#mensaje').removeClass();
    $('#mensaje').addClass('action-alert');
    $('#mensaje').html(mensaje);
}

validarRegistro = function () {
    $('#mensaje').removeClass();
    $('#mensaje').html("");
    var mensaje = "<ul>";
    var flag = true;


    $('#hfSrmcrtcodi').val($('#cbSrmcrtcodi').val());
    $('#hfSrmstdcodi').val($('#cbrmstdcodi').val());
    $('#hfUsercode').val($('#cbUsercode').val());

    if ($('#hfEquiCodi').val() == "-1" || $('#hfEquiCodi').val() == null || $('#hfEquiCodi').val() == undefined) {
        mensaje += "<li>Debe seleccionar equipo</li>";//\n";
        flag = false;
    }

    if ($('#cbSrmcrtcodi').val()==0 || $('#cbSrmcrtcodi').val()==null)
    {
        mensaje += "<li>Debe seleccionar criticidad</li>";//\n";
        flag = false;
    }

    if ($('#cbSrmstdcodi').val() == 0 || $('#cbSrmstdcodi').val() == null) {
        mensaje += "<li>Debe seleccionar estado</li>";//\n";
        flag = false;
    }
    
    if ($('#cbUsercode').val() == 0 || $('#cbUsercode').val() == null) {
        mensaje += "<li>Debe seleccionar responsable</li>";//\n";
        flag = false;
    }

    //completar fecha
    completarFecha("txtSrmrecFecharecomend");
    completarFecha("txtSrmrecFechavencim");    
    
    var flagFecha = true;
    if ((($('#txtSrmrecFecharecomend').val() + "*") == "*")) {
        mensaje += "<li>Debe ingresar fecha de recomendación</li>";
        flag = false;
        flagFecha = false;
    }

    if ((($('#txtSrmrecFechavencim').val() + "*") == "*")) {
        mensaje += "<li>Debe ingresar fecha de vencimiento</li>";
        flag = false;
        flagFecha = false;
    }


    // fecha
    var fhIni = $('#txtSrmrecFecharecomend').val();
    var fhFin = $('#txtSrmrecFechavencim').val();

    var ldtIini = convertDateTime(fhIni);
    var ldtFin = convertDateTime(fhFin);

    if (ldtFin <= ldtIini) {
        mensaje = mensaje + "<li>Corregir rango de fechas</li>";
        flag = false;
    }

    //---


    if (isNaN($('#txtSrmrecDianotifplazo').val()) || (($('#txtSrmrecDianotifplazo').val() + "*") == "*")) {
        mensaje += "<li>Debe ingresar día notificación</li>";
        flag = false;
    }
    else
    {
        if ($('#txtSrmrecDianotifplazo').val()<=0)
        {
            mensaje += "<li>Día notificación debe ser mayor que cero</li>";
            flag = false;
        }
    }

    if ((($('#txtSrmrecTitulo').val() + "*") == "*")) {
        mensaje += "<li>Debe ingresar título</li>";
        flag = false;
    }

    if ((($('#txtSrmrecRecomendacion').val() + "*") == "*")) {
        mensaje += "<li>Debe ingresar recomendación</li>";
        flag = false;
    }

    
    if (flag) mensaje = "";
    return mensaje;
}

convertDateTime = function (fechahora) {


    var fecha = fechahora.toString().split("/");
    var dd = fecha[0];
    var mm = fecha[1] - 1;
    var yyyy = fecha[2];

    return new Date(yyyy, mm, dd,0,0, 0);
}

completarFecha = function (id) {

    var valor = $('#' + id).val() + " ";

    if (valor.trim() == "")
        return;

    var date = valor.split(' ')[0].split('/');


    var date0 = (validarNumero(date[0])) ? date[0] : "00";
    var date1 = (validarNumero(date[1])) ? date[1] : "00";
    var date2 = (validarNumero(date[2])) ? date[2] : "00";

    $('#' + id).val(date0 + "/" + date1 + "/" + date2);

}

//busqueda de equipo
openBusquedaEquipo = function () {

    $('#busquedaEquipo').bPopup({
        easing: 'easeOutBack',
        speed: 450,
        transition: 'slideDown'
    });
    $('#txtFiltro').focus();
}

cargarBusquedaEquipo = function () {

    $.ajax({
        type: "POST",
        url: siteRoot + "eventos/busqueda/index",
        data: {},
        global: false,
        success: function (evt) {
            $('#busquedaEquipo').html(evt);
            if ($('#hfEvenCodi').val() == "0") {
                openBusquedaEquipo();
            }
        },
        error: function (req, status, error) {
            mostrarError();
        }
    });
}

seleccionarEquipo = function (idEquipo, substacion, equipo, empresa, idEmpresa) {
    
    $('#spanEquipo').text(equipo);
    $('#busquedaEquipo').bPopup().close();
    $('#hfEquiCodi').val(idEquipo);
    $('#busquedaEquipo').bPopup().close();
    $('#txtEmpresa').val(empresa);
    $('#txtArea').val(substacion);
}


//tratamiento de archivos Recomendacion
obtenerRecomendacion = function()
{
    return $('#hfSrmrecCodi').val();    
}

crearUpload = function () {

    var srmreccodi = obtenerRecomendacion();

    uploader = new plupload.Uploader({
        runtimes: 'html5,flash,silverlight,html4',
        browse_button: 'btnSelectFile',
        container: document.getElementById('container'),
        url: controladorBase + 'upload',
        flash_swf_url: 'Scripts/Moxie.swf',
        silverlight_xap_url: 'Scripts/Moxie.xap',
        drop_element: 'container',
        chunk_size: '20mb',        
        filters: {
            max_file_size: '100mb',
            mime_types: [
                { title: "Image files", extensions: "jpg,jpeg,gif,png" },
                { title: "Zip files", extensions: "zip,rar" },
                { title: "Document files", extensions: "pdf,doc,docx,xls,xlsx,ppt,pptx,csv,msg" }
            ]
        },
        init: {
            PostInit: function () {                
                document.getElementById('filelist').innerHTML = ''; 
                $('#container').css('display', 'none');
                document.getElementById('btnCargarArchivo').onclick = function () {
                    if (uploader.files.length > 0) {
                        $('#loadingcarga').css('display', 'block');
                        uploader.start();
                        return false;
                    }
                    else {
                        document.getElementById('filelist').innerHTML = '<div class="action-alert">Seleccione archivos</div>';
                    }
                };
            },
            FilesAdded: function (up, files) {
                $('#btnCancelarCarga').show();
                document.getElementById('filelist').innerHTML = '';
                $('#container').css('display', 'block');

                for (i = 0; i < uploader.files.length; i++) {
                    var file = uploader.files[i];
                    document.getElementById('filelist').innerHTML += '<div class="file-name" id="' + file.id + '">' + file.name + ' (' + plupload.formatSize(file.size) + ') <a class="remove-item" href="JavaScript:eliminarFile(\'' + file.id + '\');">X</a> <b></b></div>';
                }

                srmreccodi = obtenerRecomendacion();

            },
            UploadProgress: function (up, file) {
                document.getElementById(file.id).getElementsByTagName('b')[0].innerHTML = '<span>' + file.percent + "%</span>";
                $('#' + file.id).css('background-color', '#FFFFB7');
            },
            UploadComplete: function (up, file) {
                $('#btnCancelarCarga').hide();
                $('#container').css('display', 'none');
                document.getElementById('filelist').innerHTML = '';
                $('#loadingcarga').css('display', 'none');
                procesarArchivos();
            },
            Error: function (up, err) {
                $('#container').css('display', 'none');
                document.getElementById('filelist').innerHTML = '<div class="action-alert">' + err.message + '</div>';
            }
        }
    });
    uploader.init();
}

cancelarCarga = function () {
    $('#container').css('display', 'none');
    document.getElementById('filelist').innerHTML = '';
    $('#loadingcarga').css('display', 'none');
}

procesarArchivos = function () {
    

    var reccodi = obtenerRecomendacion();

    if (reccodi == 0) {
        return;
    }

    $.ajax({
        type: 'POST',
        url: controladorBase + 'archivos',
        data: {
            tipo: "R",
            srmreccodi: reccodi
        },
        success: function (evt) {
            $('#listaArchivos').html(evt);
        },
        error: function () {

        }
    });
}

descargarArchivo = function (url) {
    document.location.href = controladorBase + "download?url=" +  url;
}

eliminarArchivo = function (url,tipo,ext,abrev) {
    //document.location.href = controladorBase + "delete?url=" + url;
    //alert(abrev);
    //return;
    //grabar

    if (abrev == 'R')
    {
        var mensaje = validarRegistro();

        if (mensaje == "") {

            $.ajax({
                type: 'POST',
                url: controladorBase + "grabar",
                dataType: 'json',
                data: $('#form').serialize(),
                success: function (result) {
                    if (result != "-1") {

                        //id
                        $('#hfSrmrecCodi').val(result);

                        //eliminar archivo
                        $.ajax({
                            type: 'POST',
                            url: controladorBase + 'delete',
                            data: {
                                url: url
                            },
                            success: function (evt) {

                                $.ajax({
                                    type: 'POST',
                                    url: controladorBase + 'archivos',
                                    data: {
                                        tipo: "R",
                                        srmreccodi: $('#hfSrmrecCodi').val()
                                    },
                                    success: function (evt) {
                                        $('#listaArchivos').html(evt);
                                    },
                                    error: function () {

                                    }
                                });
                            },
                            error: function () {

                            }
                        });

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
        else {
            mostrarAlerta(mensaje);
        }
    }
    else {
        //comentario
        if (abrev == 'C') {
            var mensaje = validarRegistroCom();
            if (mensaje == "") {
                
                $.ajax({
                    type: 'POST',
                    url: controladorComentario + "grabar",
                    dataType: 'json',
                    data: $('#formCom').serialize(),
                    success: function (result) {
                        if (result != "-1") {
                            //id1                            
                            $('#hfSrmcomCodi').val(result);
                            //id2
                            //$('#hfSrmrecCodi').val(result);
                            
                            //eliminar archivo
                            $.ajax({
                                type: 'POST',
                                url: controladorBase + 'delete',
                                data: {
                                    url: url
                                },
                                success: function (evt) {
                                    
                                    $.ajax({
                                        type: 'POST',
                                        url: controladorBase + 'archivos',
                                        data: {
                                            tipo: "C",
                                            srmreccodi: $('#hfSrmreccodiEquipo').val() + '\\' + $('#hfSrmcomCodi').val()
                                        },
                                        success: function (evt) {
                                            $('#listaArchivosCom').html(evt);
                                        },
                                        error: function () {

                                        }
                                    });
                                },
                                error: function () {

                                }
                            });

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
            else {
                mostrarAlertaCom(mensaje);
            }





        }
        else {


        }



    }


    

}

eliminarFile = function (id) {
    uploader.removeFile(id)
    $('#' + id).remove();
}

//easytabs
disable_easytabs = function (tabs, indexes) {
    var lis = tabs.children('ul').children();
    var index = 0;
    lis.each(function () {
        var li = $(this);
        var a = li.children('a');
        var disabled = $.inArray(index, indexes) != -1;
        if (disabled) {
            li.addClass('disabled');
        }
        else {
            li.removeClass('disabled');
        }
        index++;
    });
}

deshabilitaComentario = function () {
    var tabs = $('#tab-container');
    disable_easytabs(tabs, [1]);
    return false;
}

//editar comentario
editarCom = function (id, accion, srmreccodi) {

    $.ajax({
        type: 'POST',
        url: controladorComentario + "editar",
        cache: false,
        data: {
            id: id,
            accion: accion,
            srmreccodi: srmreccodi
        },
        success: function (evt) {

            $('#contenidoEdicionCom').html(evt);
            setTimeout(function () {
                $('#popupEdicionCom').bPopup({
                    easing: 'easeOutBack',
                    speed: 450,
                    transition: 'slideDown'
                });
            }, 50);

            configurarComentario();
        }
    });

}

configurarComentario = function () {


    $(document).ready(function () {

        $('#cbSrmstdcodi').val($('#hfSrmstdcodi').val());
        $('#cbSrmcrtcodi').val($('#hfSrmcrtcodi').val());
        $('#cbUsercodeCom').val($('#hfUsercodeCom').val());

    });
    
    $('#btnGrabarCom').click(function () {
        grabarCom();
    });

    $('#btnCancelarCom').click(function () {
        $('#popupEdicionCom').bPopup().close();
        buscarCom();
    });
        
}

mostrarExito = function () {
    $('#mensaje').removeClass();
    $('#mensaje').addClass('action-exito');
    $('#mensaje').html('La operación se realizó con éxito.');
}

grabarCom = function () {
    var mensaje = validarRegistroCom();   
    
    if (mensaje == "") {
        //console.log("Fecha comentario: " + $('#txtSrmcomFechacoment').val());
        $.ajax({
            type: 'POST',
            url: controladorComentario + "grabar",
            dataType: 'json',
            data: $('#formCom').serialize(),
            success: function (result) {
                if (result != "-1") {

                    //mostrarExito();
                    $('#hfSrmcomCodi').val(result);

                    //id
                    //$('#hfSrmrecCodi').val(result);
                    document.getElementById('btnCargarArchivoCom').click();

                    //finalizando
                    $('#popupEdicionCom').bPopup().close();
                    buscarCom();

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
    else {
        mostrarAlertaCom(mensaje);
    }
}

validarRegistroCom = function () {

    $('#mensajeCom').removeClass();
    $('#mensajeCom').html("");
    var mensaje = "<ul>";
    var flag = true;



    $('#hfUsercodeCom').val($('#cbUsercodeCom').val());
    $('#hfEmprCodi').val($('#cbEmprCodi').val());

    if ($('#rbSrmcomGruporesponsS').is(':checked')) {
        $('#hfSrmcomGruporespons').val('C');
    }

    if ($('#rbSrmcomGruporesponsN').is(':checked')) {
        $('#hfSrmcomGruporespons').val('A');
    }



    //completar fecha
    completarFecha("txtSrmcomFechacoment");

    if ((($('#txtSrmcomFechacoment').val() + "*") == "*")) {
        mensaje += "<li>Debe ingresar fecha de comentario</li>";
        flag = false;
    }

    if ((($('#txtSrmcomComentario').val() + "*") == "*")) {
        mensaje += "<li>Debe ingresar comentario</li>";
        flag = false;
    }

    
    if ($('#hfSrmcomGruporespons').val() == 'C')
    {
        //coes seleccionado
        if (($('#hfUsercodeCom').val() + "*") == "*") {
            mensaje += "<li>Debe seleccionar usuario COES</li>";
            flag = false;

        }
    }
    else
    {
        //agente seleccionado
        if (($('#hfEmprCodi').val() + "*") == "*") {
            mensaje += "<li>Debe seleccionar Agente</li>";
            flag = false;

        }
    }



    if (flag) mensaje = "";
    return mensaje;
}


//tratamiento de archivos Comentario
crearUploadCom = function () {

    var srmreccodi = $('#hfSrmreccodi').val();
    var srmcomcodi = $('#hfSrmcomCodi').val();
    
    uploaderCom = new plupload.Uploader({
        runtimes: 'html5,flash,silverlight,html4',
        browse_button: 'btnSelectFileCom',
        container: document.getElementById('containerCom'),
        url: controladorComentario + 'upload',
        flash_swf_url: 'Scripts/Moxie.swf',
        silverlight_xap_url: 'Scripts/Moxie.xap',
        drop_element: 'containerCom',
        chunk_size: '20mb',
        filters: {
            max_file_size: '100mb',
            mime_types: [
                { title: "Image files", extensions: "jpg,jpeg,gif,png" },
                { title: "Zip files", extensions: "zip,rar" },
                { title: "Document files", extensions: "pdf,doc,docx,xls,xlsx,ppt,pptx,csv,msg" }
            ]
        },
        init: {
            PostInit: function () {
                document.getElementById('filelistCom').innerHTML = '';
                $('#containerCom').css('display', 'none');
                document.getElementById('btnCargarArchivoCom').onclick = function () {
                    if (uploaderCom.files.length > 0) {
                        $('#loadingcargaCom').css('display', 'block');
                        uploaderCom.start();
                        return false;
                    }
                    else {
                        document.getElementById('filelistCom').innerHTML = '<div class="action-alert">Seleccione archivos</div>';
                    }
                };
            },
            FilesAdded: function (up, files) {
                $('#btnCancelarCargaCom').show();
                document.getElementById('filelistCom').innerHTML = '';
                $('#containerCom').css('display', 'block');

                for (i = 0; i < uploaderCom.files.length; i++) {
                    var file = uploaderCom.files[i];
                    document.getElementById('filelistCom').innerHTML += '<div class="file-name" id="' + file.id + '">' + file.name + ' (' + plupload.formatSize(file.size) + ') <a class="remove-item" href="JavaScript:eliminarFileCom(\'' + file.id + '\');">X</a> <b></b></div>';
                }
            },
            UploadProgress: function (up, file) {
                document.getElementById(file.id).getElementsByTagName('b')[0].innerHTML = '<span>' + file.percent + "%</span>";
                $('#' + file.id).css('background-color', '#FFFFB7');
            },
            UploadComplete: function (up, file) {
                $('#btnCancelarCargaCom').hide();
                $('#containerCom').css('display', 'none');
                document.getElementById('filelistCom').innerHTML = '';
                $('#loadingcargaCom').css('display', 'none');
                procesarArchivosCom();
            },
            Error: function (up, err) {
                $('#containerCom').css('display', 'none');
                document.getElementById('filelistCom').innerHTML = '<div class="action-alert">' + err.message + '</div>';
            }
        }
    });
    uploaderCom.init();
}

cancelarCargaCom = function () {
    $('#containerCom').css('display', 'none');
    document.getElementById('filelistCom').innerHTML = '';
    $('#loadingcargaCom').css('display', 'none');
}

procesarArchivosCom = function () {

    var reccodi = $('#hfSrmreccodiEquipo').val();
    var comcodi = $('#hfSrmcomCodi').val();

    if (comcodi == 0)
    {
        return;
    }

    $.ajax({
        type: 'POST',
        url: controladorBase + 'archivos',
        data: {
            tipo:"C",
            srmreccodi: reccodi + '\\' + comcodi
        },
        success: function (evt) {
            $('#listaArchivosCom').html(evt);
        },
        error: function () {

        }
    });
}

eliminarFileCom = function (id) {
    uploaderCom.removeFile(id)
    $('#' + id).remove();
}


//funciones de paginado de comentario
function cargarPrimeraPaginaCom() {
    var id = 1;
    $('#hfPaginaActualCom').val(id);
    mostrarPaginadoCom();
    pintarBusquedaCom(id);
}

function cargarAnteriorPaginaCom() {
    var id = $('#hfPaginaActualCom').val();
    if (id > 1) {
        id = id - 1;
        $('#hfPaginaActualCom').val(id);
        mostrarPaginadoCom();
        pintarBusquedaCom(id);
    }
}

function cargarPaginaCom(id) {
    $('#hfPaginaActualCom').val(id);
    mostrarPaginadoCom();
    pintarBusquedaCom(id);
}

function cargarSiguientePaginaCom() {
    var id = parseInt($('#hfPaginaActualCom').val()) + 1;
    $('#hfPaginaActualCom').val(id);
    mostrarPaginadoCom();
    pintarBusquedaCom(id);
}

function cargarUltimaPaginaCom() {
    var id = $('#hfNroPaginasCom').val();
    $('#hfPaginaActualCom').val(id);
    mostrarPaginadoCom();
    pintarBusquedaCom(id);
}

pintarBusquedaCom = function (nroPagina)
{
    var id = $('#hfSrmreccodiEquipo').val();
    var activo = obtenerComentarioActivo();

    $('#hfNroPaginaCom').val(nroPagina);
    $.ajax({
        type: 'POST',
        url: controladorComentario + "lista",
        cache: false,
        data: {
            srmreccodi: id,
            activo: activo,
            nroPage: nroPagina
        },
        success: function (evt) {
            $('#ComentDetalle').html(evt);

            var tabs = $('#tab-container');
            disable_easytabs(tabs, []);

            //ir a la lista
            $('#tab-container').easytabs('select', '#Comentario');

        },
        error: function () {
            mostrarError();
        }
    });

}

function mostrarPaginadoCom() {
    var nroToShow = parseInt($('#hfNroMostrarCom').val());
    var nroPaginas = parseInt($('#hfNroPaginasCom').val());
    var nroActual = parseInt($('#hfPaginaActualCom').val());

    $('.pag-ini').css('display', 'none');
    $('.pag-item').css('display', 'none');
    $('.pag-fin').css('display', 'none');
    $('.pag-item').removeClass('paginado-activo');

    $('#pagCom' + nroActual).addClass('paginado-activo');

    if (nroToShow - nroPaginas >= 0) {
        $('.pag-item').css('display', 'block');
        $('.pag-ini').css('display', 'none');
        $('.pag-fin').css('display', 'none');
    }
    else {
        $('.pag-fin').css('display', 'block');
        if (nroActual > 1) {
            $('.pag-ini').css('display', 'block');
        }
        var anterior = 0;
        var siguiente = 0;

        if (nroActual == 1) {

            anterior = 1;
            siguiente = nroToShow;
        }
        else {
            if (nroActual + nroToShow - 1 - nroPaginas > 0) {
                siguiente = nroPaginas;
                anterior = nroPaginas - nroToShow + 1;
            }
            else {
                anterior = nroActual;
                siguiente = nroActual + nroToShow - 1;
            }
        }

        for (j = anterior; j <= siguiente; j++) {
            $('#pagCom' + j).css('display', 'block')
        }
    }
}