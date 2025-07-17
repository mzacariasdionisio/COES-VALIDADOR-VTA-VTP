var controlador = siteRoot + 'Ensayo/';
var DATA_FORMATO = null;
var DATA_EQUIPO = null;
var ENSAYOCODI = 0;

$(function () {
    $('#btnCancelar').click(function () {
        regresar();
    });

    var dataFormato = $("#hfListaFormato").val();
    DATA_FORMATO = JSON.parse(dataFormato);

    var dataEq = $("#hfListaEquipo").val();
    DATA_EQUIPO = JSON.parse(dataEq);

    ENSAYOCODI = parseInt($("#hfCodigo").val());

    listarUnidades();
})

//////////////////////////////////////////////////////////////////////////////////////////
/// Lista de envio de cada formato (DERECHA)
//////////////////////////////////////////////////////////////////////////////////////////

function listarUnidades() {
    var strTabLi = '';
    var strTabDiv = '';
    var prefijo = 'unidad';

    //Generar el lado Izquierdo
    for (var i = 0; i < DATA_EQUIPO.length; i++) {
        var equipo = DATA_EQUIPO[i];
        var codigo = equipo.Equicodi;
        var nombre = equipo.Equinomb;
        var idTab = prefijo + codigo;
        var sufijoEQ = '_eq_' + codigo;
        strTabLi += '<li class="tab"><a href="#' + idTab + '" id="idView' + codigo + '" >' + nombre + '</a></li>';
        strTabDiv += '<div id="' + idTab + '">';
        strTabDiv += '<div style="float:left;padding-top: 10px;">';
        strTabDiv += generarHtmlFormatoXUnidad(idTab, codigo);
        strTabDiv += '</div>';
        strTabDiv += '<div class="table-list" id="listado' + sufijoEQ + '" style="display:inline-block;width:400px; height:300px;  float:right;margin-right:20px"></div>'
        strTabDiv += '</div>';
    }

    $("#tab-container ul.etabs").html(strTabLi);
    $("#tab-container div.panel-container").html(strTabDiv);

    $("#tab-container").show();
    $("#tab-container").easytabs({
        animate: false
    });

    $("#tab-container").show();

    //Generar el lado derecho
    for (var i = 0; i < DATA_EQUIPO.length; i++) {
        mostrarListado(ENSAYOCODI, DATA_EQUIPO[i].Equicodi, 0);
    }
}

function generarHtmlFormatoXUnidad(divId, codigo) {
    var html = '';
    for (var i = 0; i < DATA_FORMATO.length; i++) {
        var formatoPadre = DATA_FORMATO[i];
        html += '<div>' + formatoPadre.Formatodesc + ' </div>';

        var listaFHijo = formatoPadre.ListaFormato;
        if (listaFHijo != null) {
            for (var j = 0; j < listaFHijo.length; j++) {
                html += generarHtmlFormatoHijo(ENSAYOCODI, codigo, listaFHijo[j].Formatocodi, listaFHijo[j].Formatodesc);
            }
        }
    }

    return html;
}

function generarHtmlFormatoHijo(ensayocodi, equicodi, formatocodi, formatodesc) {
    var sufijoFormatoHijo = '_eq_' + equicodi + '_fmt_' + formatocodi;
    var idArchivo = 'archivo' + sufijoFormatoHijo;
    var idAdjuntar = 'btnSelectFileA' + sufijoFormatoHijo;
    var idEnviar = 'btnSelectFileE' + sufijoFormatoHijo;
    var idEnsayo = 'hCodiEnsayo' + sufijoFormatoHijo;
    var varUploader = 'uploaderP' + sufijoFormatoHijo;

    var disenioDiv = `
    <div>
        <div class="formulario-item" style="width:550px">
            <div class="formulario-label" style="width:350px">
                ${formatodesc} <input type="text" id="${idArchivo}" value="" style="width:250px" readonly="readonly" />
            </div>
            <div class="formulario-control" style="width:180px">
                <input type="button" id="${idAdjuntar}" value="Adjuntar" />
                <input type="button" id="${idEnviar}" value="Enviar" />
            </div>
        </div>

        <input type="hidden" id="${idEnsayo}" value="${ensayocodi}" />
        <script type="text/javascript">
            var fullDate = new Date();
            var twoDigitMonth = ((fullDate.getMonth().length) == 1) ? '0' + (fullDate.getMonth() + 1) : (fullDate.getMonth() + 1);
            var sFecha = fullDate.getFullYear().toString() + twoDigitMonth.toString() + fullDate.getDate().toString() + fullDate.getHours().toString() + fullDate.getMinutes().toString() + fullDate.getSeconds().toString();
            var ${varUploader} = new plupload.Uploader({
                runtimes: 'html5,flash,silverlight,html4',
                browse_button: '${idAdjuntar}',
                container: document.getElementById('tablaEmpresaUnidad'),
                multipart_params: {
                    "formatocodi": ${formatocodi},
                    "ensayocodi": ${ensayocodi},
                    "equicodi": ${equicodi}
                },
                url: siteRoot + 'ensayo/genera/upload',
                flash_swf_url: '~/Content/Scripts/Moxie.swf',
                silverlight_xap_url: '~/Content/Scripts/Moxie.xap',
                multi_selection: false,
                filters: {
                    max_file_size: '30mb',
                    mime_types: [
                        { title: "Archivos Zip .zip", extensions: "zip" },
                        { title: "Archivos Pdf .pdf", extensions: "pdf" },
                        { title: "Archivos Rar .rar", extensions: "rar" },
                        { title: "Archivos Excel .xlsx", extensions: "xlsx" },
                        { title: "Archivos Excel .xls", extensions: "xls" },
                        { title: "Archivos Word .docx", extensions: "docx" },
                        { title: "Archivos Word .doc", extensions: "doc" },
                        { title: "Archivos Imagen .jpg", extensions: "jpg" },
                        { title: "Archivos Imagen .gif", extensions: "gif" }
                    ]
                },

                init: {
                    PostInit: function () {
                        document.getElementById('${idEnviar}').onclick = function () {
                            if (${varUploader}.files.length > 0) {
                                ${varUploader}.start();
                            }
                            else
                                loadValidacionFile("Seleccione archivo.");
                            return false;
                        };
                    },
                    FilesAdded: function (up, files) {
                        if (${varUploader}.files.length == 2) {
                            ${varUploader}.removeFile(${varUploader}.files[0]);
                        }
                        plupload.each(files, function (file) {
                            $('#${idArchivo}').val(file.name);
                        });

                        up.refresh();
                    },
                    UploadProgress: function (up, file) {
                    },
                    FileUploaded: function(up, file, info) {                        
                        if(info.response == "1"){
                            mostrarListado(${ensayocodi}, ${equicodi}, ${formatocodi}); 
                            mensajeExito();
                        }else{
                            loadValidacionFile("Ocurrió un error al Enviar archivo");
                        }
                        $("#${idArchivo}").val("");
                    },
                    
                    Error: function (up, err) {
                        loadValidacionFile("Ocurrió un error al Enviar archivo");
                    }
                }
            });
            ${varUploader}.init();
        </script>
    </div>
    `;

    return disenioDiv;
}

//////////////////////////////////////////////////////////////////////////////////////////
/// Lista de envio de cada formato (IZQUIERDA)
//////////////////////////////////////////////////////////////////////////////////////////

function mostrarListado(ensayocodi, equicodi, formatocodi) {
    var sufijoEQ = '_eq_' + equicodi;
    $.ajax({
        type: 'POST',
        url: controlador + "genera/ListaFormato",
        data: {
            ensayocodi: ensayocodi,
            equicodi, equicodi,
            formatocodi: formatocodi
        },
        success: function (evt) {
            $('#listado' + sufijoEQ).html(evt);            
        },
        error: function (err) {
            alert("Ha ocurrido un error mostrar listado");
        }
    });
}

function popupHistorialFormato(enunidadcodi, equicodi, formatocodi) {
    var sufijoEQ = '_eq_' + equicodi;
    $.ajax({
        type: 'POST',
        url: controlador + "genera/HistorialFormato",
        data: {
            enunidadcodi: enunidadcodi,
            equicodi: equicodi,
            formatocodi: formatocodi
        },
        success: function (evt) {
            $('#HistorialFormato' + sufijoEQ).html(evt);
            setTimeout(function () {
                $('#popupEnsayoHistorialFormato' + sufijoEQ).show();
                $('#popupEnsayoHistorialFormato' + sufijoEQ).bPopup({
                    easing: 'easeOutBack',
                    speed: 450,
                    transition: 'slideDown'
                });
            }, 50); 
            //$('#popupEnsayoHistorialFormato_eq_'+sufijoEQ).bPopup().close();

        },
        error: function (err) {
            alert("Ha ocurrido un error en ingresar Historial Formato");
        }
    });
}


regresar = function () {
    document.location.href = controlador + "genera/Index/";
}

function abrirArchivo(archivo) {
    window.location = controlador + 'genera/DescargarArchivoEnvio?archivo=' + archivo;
}

loadInfoFile = function (fileName) {
    $('#fileInfo').html(fileName);
}

loadValidacionFile = function (mensaje) {
    alert(mensaje);
}

mostrarProgreso = function (porcentaje) {
    $('#progreso').text(porcentaje + "%");
}

mostrarError = function () {
    alert("Ha ocurrido un error.");
}

mostrarMensaje = function (mensaje) {
    alert(mensaje);
}

limpiarMensaje = function () {
    $('#mensaje').removeClass("action-alert");
    $('#mensaje').removeClass("action-exito");
    $('#mensaje').addClass("action-message");
    $('#mensaje').html("Complete los datos, seleccione y procese archivo.");
}

mensajeExito = function () {
    $('#popupFormatoEnviadoOk').bPopup({
        easing: 'easeOutBack',
        speed: 450,
        transition: 'slideDown'
    });
    $("#btnAceptarOK").click(function () {
        $('#popupFormatoEnviadoOk').bPopup().close();
    });
}

cerrarpopupFormatoEnviadoOk = function () {
    $('#popupFormatoEnviadoOk').bPopup().close();
}

function salir2 (equicodi) {
    $('#popupEnsayoHistorialFormato_eq_'+equicodi).bPopup().close();
}