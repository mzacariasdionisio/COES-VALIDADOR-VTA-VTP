var controlador = siteRoot + "Intervenciones/Registro/";
var controler = siteRoot + "Intervenciones/Registro/";
var ANCHO_LISTADO = 900;

$(document).ready(function ($) {
    ANCHO_LISTADO = $("#mainLayout").width() > 900 ? $("#mainLayout").width() - 5 : 900;

    $('#btnEnviar').click(function () {
        enviarMensaje();
    });

    $('#btnRegresar').click(function () {
        regresarPantallaRegistro();
    });

    listarIntervencionesSeleccionadas();

    //muestro editor de contenido
    iniciarControlTexto('Contenido', []);

    //habilitar upload de archivos
    msj_InicializarCargaArchivo();
});

function msj_InicializarCargaArchivo() {
    LISTA_SECCION_ARCHIVO_X_MENSAJE = [];

    var seccion = {
        Inpstidesc: 'Archivos adjuntos',
        EsEditable: true,
        ListaArchivo: [],
        Modulo: TIPO_MODULO_MENSAJE,
        Progrcodi: getPlantillaCorreo().Progrcodi,
        Carpetafiles: getPlantillaCorreo().Carpetafiles,
        Subcarpetafiles: 0,
        TipoArchivo: TIPO_ARCHIVO_MENSAJE,
        IdDiv: `html_archivos_mensaje`,
        IdDivVistaPrevia: `vistaprevia_mensaje`,
        IdPrefijo: arch_getIdPrefijo(0)
    };

    LISTA_SECCION_ARCHIVO_X_MENSAJE.push(seccion);
    arch_cargarHtmlArchivoEnPrograma(seccion.IdDiv, seccion);
}

function regresarPantallaRegistro() {
    var progrcodi = $("#hfProgrcodi").val();

    //Observación parametros en la URL - Modificado 15/05/2019
    var paramList = [
        { tipo: 'input', nombre: 'progCodi', value: progrcodi },
    ];
    var form = CreateForm(controler + 'IntervencionesRegistro', paramList);
    document.body.appendChild(form);
    form.submit();

}

function enviarMensaje() {
    var correo = getPlantillaCorreo();
    var msg = validarCamposCorreoAGuardar(correo);

    if (msg == "") {
        $("#mensaje_validacion").hide();
        $.ajax({
            type: 'POST',
            url: controler + 'SaveIntervencionesMensajeRegistro',
            contentType: "application/json",
            dataType: 'json',
            data: JSON.stringify({
                plantillaCorreo: correo,
            }),
            success: function (result) {
                if (result.Resultado != "-1") {
                    alert("Se ha enviado correctamente el mensaje.");
                    regresarPantallaRegistro();
                }
                else {
                    alert("Ocurrió un error: " + result.StrMensaje);
                }
            },
            error: function (request, status, error) {
                alert("Error inesperado -" + request.responseText);
            }
        });
    } else {
        $("#mensaje_validacion").show();
        mostrarMensaje('mensaje_validacion', 'error', msg);
    }
}

function getPlantillaCorreo() {
    var obj = {};
    obj.Correlativos = $("#hfIntercodis").val();
    obj.Progrcodi = $("#hfProgrcodi").val();
    obj.Evenclasecodi = $("#hfIdTipoProgramacion").val();
    obj.Carpetafiles = parseInt($("#hfMsjCarpetaFiles").val());

    obj.PlanticorreoFrom = $("#From").val();
    obj.Planticorreos = $("#To").val();
    obj.PlanticorreosCc = $("#CC").val();
    obj.PlanticorreosBcc = $("#BCC").val();
    obj.Plantasunto = $("#Asuntos").val();
    obj.Plantcontenido = $("#Contenido").val();

    //archivos
    if (LISTA_SECCION_ARCHIVO_X_MENSAJE.length > 0)
        obj.ListaArchivo = LISTA_SECCION_ARCHIVO_X_MENSAJE[0].ListaArchivo;

    return obj;
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

//Intervenciones seleccionadas
function listarIntervencionesSeleccionadas() {
    var intercodis = $("#hfIntercodis").val();

    $.ajax({
        type: 'POST',
        url: controler + "MostrarListadoIntervencionesXIds",
        data: {
            intercodis: intercodis,
        },
        dataType: 'json',
        success: function (evt) {
            if (evt.Resultado != "-1") {
                $("#Asuntos").val(evt.Asunto);
                $("#CC").val(evt.CC);

                $("#listado").hide();
                $('#listado').css("width", ANCHO_LISTADO + "px");

                var html = _dibujarTablaListado(evt);
                $('#listado').html(html);

                $("#listado").show();

                var targetsnew = [0, 1];
                var Ordenacion = [[4, 'asc']];

                $('#TablaConsultaIntervencion').dataTable({
                    "ordering": true,
                    "columnDefs": [{
                        "targets": targetsnew,
                    }],
                    order: Ordenacion,
                    "info": false,
                    "searching": true,
                    "paging": false,
                    "scrollX": true,
                    "scrollY": $('#listado').height() > 200 ? nuevoTamanioTabla + "px" : "100%"
                });

            } else {
                alert(evt.Mensaje);
            }
        },
        error: function (err) {
            alert("Lo sentimos, ha ocurrido un error inesperado");
        }
    });
}

function _dibujarTablaListado(model) {
    var lista = model.ListaFilaWeb;

    var htmlThAlerta = '';
    if (model.PintarAlerta > 0) {
        var anchoThAlerta = model.PintarAlerta * 20;
        htmlThAlerta = `<th style="text-align:center;width: ${anchoThAlerta}px">Alertas<br> detectadas</th>`;
    }

    var cadena = '';
    cadena += `
    <table border="0" class="pretty tabla-icono" id="TablaConsultaIntervencion" style="overflow: auto; height:auto; width: 2000px !important; white-space: nowrap">
        <thead>
            <tr>
                <th style="text-align:left;">Fec. Mod.</th>
                <th style="text-align:left;">Usuario Mod.</th>
                <th style="text-align:left">Justificación</th>
                <th style="text-align:left">Clase</th>
                <th style="text-align:left">Estado</th>
                <th style="text-align:left">Tip.Interv.</th>
                <th style="text-align:left">Empresa</th>
                <th style="text-align:left">Ubicación</th>
                <th style="text-align:left">Tipo</th>
                <th style="text-align:left">Equipo</th>
                <th style="text-align:left">Fec.Inicio</th>
                <th style="text-align:left">Fec.Fin</th>
                <th style="text-align:left">MWI</th>
                <th style="text-align:left">Ind</th>
                <th style="text-align:left">Int</th>
                <th style="text-align:left">Descripción</th>
                <th style="text-align:left">Cod. Seguimiento</th>
            </tr>
        </thead>
        <tbody>
    `;

    for (key in lista) {
        var item = lista[key];
        var sStyle = '';
        cadena += `
            
            <tr id="fila_${item.Intercodi}">
                <td style="text-align:left; ${sStyle}">${item.UltimaModificacionFechaDesc}</td>
                <td style="text-align:left; ${sStyle}">${item.UltimaModificacionUsuarioDesc}</td>
                <td style="text-align:left; ${sStyle}">${item.Interjustifaprobrechaz}</td>

                <td style="text-align:left; ${sStyle}">${item.ClaseProgramacion}</td>
                <td style="text-align:left; ${sStyle}">${item.EstadoRegistro}</td>
                <td style="text-align:left; ${sStyle}">${item.Tipoevenabrev}</td>
                <td style="text-align:left; ${sStyle}">${item.EmprNomb}</td>
                <td style="text-align:left; ${sStyle}">${item.AreaNomb}</td>
                <td style="text-align:left; ${sStyle}">${item.Famabrev}</td>
                <td style="text-align:left; ${sStyle}">${item.Equiabrev}</td>
                <td style="text-align:left; ${sStyle}">${item.InterfechainiDesc}</td>
                <td style="text-align:left; ${sStyle}">${item.InterfechafinDesc}</td>
                <td style="text-align:left; ${sStyle}">${item.Intermwindispo}</td>
                <td style="text-align:left; ${sStyle}">${item.InterindispoDesc}</td>
                <td style="text-align:left; ${sStyle}">${item.InterinterrupDesc}</td>
                <td style="text-align:left; ${sStyle}">${item.Interdescrip}</td>
                <td style="text-align:left; ${sStyle}">${item.Intercodsegempr}</td>
            </tr>
        `;
    }
    cadena += "</tbody></table>";

    return cadena;
}

//Eventos 
function iniciarControlTexto(id, listaVariables) {

    tinymce.remove();

    tinymce.init({
        selector: '#' + id,
        plugins: [
            //'paste textcolor colorpicker textpattern link table image imagetools preview'
            'wordcount advlist anchor autolink codesample colorpicker contextmenu fullscreen image imagetools lists link media noneditable preview  searchreplace table template textcolor visualblocks wordcount'
        ],
        toolbar1:
            //'insertfile undo redo | fontsizeselect bold italic underline strikethrough | forecolor backcolor | alignleft aligncenter alignright alignjustify | bullist numlist outdent indent | link | table | image | mybutton | preview',
            'insertfile undo redo | styleselect fontsizeselect | forecolor backcolor | bullist numlist outdent indent | link | table | image | mybutton | preview',

        menubar: false,
        readonly: 0,
        language: 'es',
        statusbar: false,
        convert_urls: false,
        plugin_preview_width: 1000,
        setup: function (editor) {
            editor.on('change',
                function () {
                    editor.save();
                });
        }
    });

}

function llenarMenus(e, lista) {
    var listaObj = [];
    var numVariables = lista.length;

    if (numVariables >= 1) {
        var regObj = {};
        var regVariable = lista[0];
        regObj.text = regVariable.Nombre;
        regObj.onclick = function () { e.insertContent(regVariable.ContenidoHtml); };
        listaObj.push(regObj);

    }

    return listaObj;
}

function validarCamposCorreoAGuardar(correo) {
    var msj = "";

    /*********************************************** validacion del campo CC ***********************************************/
    var validaTo = 0;

    if ($('#To').val().trim() == "") {
        msj += "<p>Error encontrado en el campo Para. Debe ingresar remitente.</p>";
    }

    validaTo = validarCorreo(correo.Planticorreos.trim(), 0, -1, "To");
    if (validaTo < 0) {
        msj += "<p>Error encontrado en el campo Para. Revisar que el/los correo(s) sea(n) válido(s) y separados por punto y coma (;).</p>";
    }

    /*********************************************** validacion del campo CC ***********************************************/
    var validaCc = validarCorreo(correo.PlanticorreosCc.trim(), 0, -1, "Cc");
    if (validaCc < 0) {
        msj += "<p>Error encontrado en el campo CC. Revisar que el/los correo(s) sea(n) válido(s) y separados por punto y coma (;).</p>";
    }

    /*********************************************** validacion del campo BCC ***********************************************/
    var validaBcc = validarCorreo(correo.PlanticorreosBcc.trim(), 0, -1, "Bcc");
    if (validaBcc < 0) {
        msj += "<p>Error encontrado en el campo BCC. Revisar que el/los correo(s) sea(n) válido(s) y separados por punto y coma (;).</p>";
    }

    /********************************************** validacion del campo Asunto ***********************************************/
    var asunto_ = correo.Plantasunto.trim();
    if (asunto_ == "") {
        msj += "<p>Error encontrado en el campo Asunto. Debe ingresar Asunto.</p>";
    }

    /*********************************************** validacion del campo Contenido ***********************************************/
    var contenido_ = correo.Plantcontenido.trim();
    if (contenido_ == "") {
        msj += "<p>Ingrese Contenido.</p>";
    }

    return msj;
}

function validarCorreo(valCadena, minimo, maximo, campo) {
    var cadena = valCadena;

    var arreglo = cadena.split(';');
    var nroCorreo = 0;
    var longitud = arreglo.length;

    if (longitud == 0) {
        arreglo = cadena;
        longitud = 1;
    }

    for (var i = 0; i < longitud; i++) {

        var email = arreglo[i].trim();
        var validacion = validarDirecccionCorreo(email);

        if (validacion) {
            nroCorreo++;
        } else {
            if (email != "")
                return -1;
        }
    }

    if (minimo > nroCorreo)
        return -1;

    if (maximo > 0 && nroCorreo > maximo)
        return -1;

    return 1;
}

function validarDirecccionCorreo(email) {
    var re = /^(([^<>()\[\]\\.,;:\s@"]+(\.[^<>()\[\]\\.,;:\s@"]+)*)|(".+"))@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}])|(([a-zA-Z\-0-9]+\.)+[a-zA-Z]{2,}))$/;
    return re.test(email);
}

function mostrarMensaje(id, tipo, mensaje) {
    $("#" + id).css("display", "block");
    $('#' + id).removeClass();
    $('#' + id).addClass('action-' + tipo);
    $('#' + id).html(mensaje);

}