var controler = siteRoot + "Intervenciones/Registro/";
var ANCHO_LISTADO = 900;
var subFolder;
var urlAprobacion;

var ESTADO_APROBADO = 3;
var ESTADO_CERRADO = 5;
var EVENCLASECODI_ANUAL = 5;
var EVENCLASECODI_SEMANAL = 3;
var EVENCLASECODI_DIARIO = 2;
var EVENCLASECODI_EJECUTADO = 1;

var IMG_VER = `<img src="${siteRoot}Content/Images/btn-open.png" title="Ver informacion del registro"/>`;
var IMG_EDITAR = `<img src="${siteRoot}Content/Images/btn-edit.png" title="Editar informacion del registro"/>`;
var IMG_COMUNICACION_NINGUNA = `<img src="${siteRoot}Content/Images/message_ninguno.png" title="No tiene mensajes" />`;
var IMG_COMUNICACION_EXISTE_MENSAJE = `<img src="${siteRoot}Content/Images/message_existe_mensaje.png" title="Existe(n) mensaje(s)" />`;
var IMG_COMUNICACION_TODO_LEIDO = `<img src="${siteRoot}Content/Images/message_todo_leido.png" title="Todos los mensajes están leídos" />`;
var IMG_COMUNICACION_PENDIENTE_LEER = `<img src="${siteRoot}Content/Images/message_pendiente_leer.png" title="Existe(n) mensaje(s) sin leer" />`;
var IMG_MODIFICACION = `<img src="${siteRoot}Content/Images/historial.png" title="Ver informacion de Modificaciones" />`;
var IMG_TRAZABILIDAD = `<img src="${siteRoot}Content/Images/eslabon.png" title="Ver Trazabilidad" />`;
var IMG_FILE = `<img src="${siteRoot}Content/Images/adjuntos.png" title="Tiene archivos adjuntos"/>`;
var IMG_SUSTENTO = `<img src="${siteRoot}Content/Images/pdf.png" title="Descargar sustento en formato .pdf"/>`;

var ES_OBIGLATORIO_TRANSFERIR = false;

$(document).ready(function ($) {
    $.fn.dataTable.moment('DD/MM/YYYY HH:mm');
    //titulo
    var estadoProgDesc = $("#estadoProgDesc").val();
    var color = '';

    var htmlTitulo = $("#tituloIndex").html();
    var backColor = `<span style="color:${color} ;font-weight: bold;">[${estadoProgDesc}]</span>`;

    $("#tituloIndex").html(htmlTitulo + backColor);

    $('.txtFecha').inputmask({
        mask: "1/2/y",
        placeholder: "dd/mm/yyyy",
        alias: "datetime"
    });

    $('.txtFecha').Zebra_DatePicker({
        readonly_element: false
    });

    var fechaIniProg = $("#InterfechainiD").val();
    var fechaFinProg = $("#InterfechafinD").val();
    $('#InterfechainiD').Zebra_DatePicker({
        direction: [fechaIniProg, fechaFinProg],
    });
    $('#InterfechafinD').Zebra_DatePicker({
        direction: [fechaIniProg, fechaFinProg],
    });

    $('#cboTipoIntervencion, #InterDispo, #estadocodi').multipleSelect({
        selectAll: true,
        filter: true,
        placeholder: "SELECCIONE",
    });

    $('#cboFamilia, #cboUbicacion').multipleSelect({
        selectAll: true,
        filter: true,
        placeholder: "SELECCIONE",
    });

    $('#Empresa').multipleSelect({
        selectAll: true,
        filter: true,
        placeholder: "SELECCIONE",
    });
    var idsEmpresas = $("#hfEmpresa").val();
    var arrEmp = idsEmpresas.split(',');
    $("#Empresa").multipleSelect('setSelects', arrEmp);

    $('#cboEquipo').multipleSelect({
        selectAll: true,
        filter: true,
        placeholder: "SELECCIONE",
    });

    $('#cboTipoIntervencion, #InterDispo, #estadocodi').multipleSelect('checkAll');
    $('#cboFamilia, #cboUbicacion ').multipleSelect('checkAll');
    $('#Empresa').multipleSelect('checkAll');
    $('#cboEquipo').multipleSelect('checkAll');

    $('#cboFamilia').on("change", function () {
        listarEquipoFiltro();
    });

    $('#cboUbicacion').on("change", function () {
        listarEquipoFiltro();
    });

    $('#btnConsultar').click(function () {
        mostrarLista();
    });

    //check Ocultar Eliminados
    $(".check_eliminado").prop('checked', true);
    $(".check_eliminado").on("click", function () {
        mostrarLista();
    });

    //check Con archivos
    $(".check_mostrarAdjuntos").prop('checked', false);
    $(".check_mostrarAdjuntos").on("click", function () {
        mostrarLista();
    });

    //check Con Mensajes
    $(".check_mostrarMensajes").prop('checked', false);
    $(".check_mostrarMensajes").on("click", function () {
        mostrarLista();
    });

    $('#ReporteIntervenciones').click(function () {
        reporteIntervencionesXls();
    });

    $('#btnManttoConsulta').click(function () {
        generarManttoConsultaRegistro();
    });

    $('#IntervencionesNuevo').click(function () {
        if (ES_OBIGLATORIO_TRANSFERIR) {
            alert("¡Debe realizar la transferencia de intervenciones antes de hacer la acción!");
        } else {
            abrirPopupNuevo();
        }
    });

    $('#IntervencionesEliminar').click(function () {
        eliminarMasivo();
    });

    $('#IntervencionesCopiar').click(function () {
        copiarIntervenciones();
    });

    $('#IntervencionesImportar').click(function () {
        if (ES_OBIGLATORIO_TRANSFERIR) {
            alert("¡Debe realizar la transferencia de intervenciones antes de hacer la acción!");
        } else {
            var dProgrcodi = document.getElementById('Progrcodi').value;
            var dTipoProgramacion = document.getElementById('idTipoProgramacion').value;
            var dEmprcodi = document.getElementById('idEmprcodi').value;

            var paramList = [
                { tipo: 'input', nombre: 'progrCodi', value: dProgrcodi },
                { tipo: 'input', nombre: 'tipoProgramacion', value: dTipoProgramacion },
                { tipo: 'input', nombre: 'emprcodi', value: dEmprcodi }
            ];
            var form = CreateForm(controler + 'IntervencionesImportacion', paramList);
            document.body.appendChild(form);
            form.submit();
        }
    });

    $('#IntervencionesEnviarMensaje').click(function () {
        mensaje();
    });

    $('#btnEnviarRptaComunicacion').click(function () {
        enviarRespuestaComunicacion();
    });
    $('#btnDescargaPdfComunicacion').click(function () {
        descargarPdfComunicacion();
    });
    $('#btnDescargaZipComunicacion').click(function () {
        descargarZipComunicacion();
    });
    $('#IntervencionesRptComunicaciones').click(function () {
        generarIntervencionesRptComunicaciones();
    });

    $('#btnExportarExcel').click(function () {
        generarExcelXls();
    });

    $('#btnManttoPlantilla').click(function () {
        descargarManttoPlantilla();
    });

    $("#chkSeleccionar").prop('checked', false);

    $("#chkSeleccionar").on("click", function () {
        var check = $('#chkSeleccionar').is(":checked");
        $(".ChkSeleccion").prop("checked", check);
    });

    $('#btnRegresar2').click(function () {
        var idTipoprog = $("#idTipoProgramacion").val();
        var idempresas = $('#Empresa').multipleSelect('getSelects').join(',');

        var paramList = [
            { tipo: 'input', nombre: 'tipoProgramacion', value: idTipoprog },
            { tipo: 'input', nombre: 'emprcodis', value: idempresas },
        ];
        var form = CreateFormRegresar(controler + 'Programaciones', paramList);
        document.body.appendChild(form);
        form.submit();

    });

    $('#btnContraer').click(
        function (e) {
            $('#Contenido').toggle();
            $(this).css("display", "none");
            $('#btnDescontraer').css("display", "block");
            ocultar = 1;

            $("#listado").hide();
            var nuevoTamanioTabla = getHeightTablaListado();
            $(".dataTables_scrollBody").css('height', nuevoTamanioTabla + "px");
            $("#listado").show();
        }
    );

    $('#btnDescontraer').click(
        function (e) {
            $('#Contenido').slideDown();
            $(this).css("display", "none");
            $('#btnContraer').css("display", "block");
            ocultar = 0;

            $("#listado").hide();
            var nuevoTamanioTabla = getHeightTablaListado();
            $(".dataTables_scrollBody").css('height', nuevoTamanioTabla + 'px');
            $("#listado").show();
        }
    );

    $('#DescargarArchivos').click(function () {
        descargarArchivosAdjuntos();
    });

    $("#btnCerrarNotif").on("click", function () {
        CerrarNotificacion();
    });

    //Sustento
    var famcodisSustentoObligatorio = $("#hfFamcodiSustentoObligatorio").val();
    if (famcodisSustentoObligatorio != null && famcodisSustentoObligatorio != "") {
        LISTA_FAMCODI_SUSTENTO_OBLIGATORIO = famcodisSustentoObligatorio.split(',');
    }
    var famcodisSustentoOpcional = $("#hfFamcodiSustentoOpcional").val();
    if (famcodisSustentoOpcional != null && famcodisSustentoOpcional != "") {
        LISTA_FAMCODI_SUSTENTO_OPCIONAL = famcodisSustentoOpcional.split(',');
    }

    //Cargar filtros
    listarEquipoFiltro();

    //tabla
    inicializarListado();
});

async function inicializarListado() {
    //verificar obligatoriedad
    await st_VerificarObligatoriedadTransferir();

    //mostrar lista de intervenciones
    mostrarLista();
}

function CreateFormRegresar(path, params, method = 'post') {
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

//Descargar Archivos Adjuntos en .zip
function descargarArchivosAdjuntos() {
    var tipo = listarIntercodiChecked();

    _descargarArchivosAdjuntosXId(tipo);    
}

function _descargarArchivosAdjuntosXId(tipo) {
    if (tipo != null && tipo != "") {
        if (confirm("¿Desea descargar los archivos adjuntos de la información seleccionada?")) {
            $.ajax({
                type: 'POST',
                url: controler + 'DescargarArchivosSeleccionados',
                data: { tipo: tipo },
                success: function (evt) {
                    if (evt.Resultado != "-1") {
                        $('input[type=checkbox].ChkSeleccion').prop('checked', false);
                        $("#chkSeleccionar").prop('checked', false); // Deschekea el check seleccionar todos
                        window.location = controler + "ExportarZip?file_name=" + evt.Resultado;

                    } else {
                        alert("Ha ocurrido un error: " + evt.Mensaje);
                    }
                },
                error: function (err) {
                    alert("Error al descargar archivos");
                }
            });
        }
    } else {
        alert("Seleccione registros para descargar archivos.");
    }
}

//funcion que calcula el ancho disponible para la tabla reporte
function getHeightTablaListado() {
    var alto = $(window).height()
        - $(".header").height()
        - $(".form-title_intranet").height()
        - 15
        - $("#Contenido").parent().height() //Filtros
        - $(".dataTables_filter").height()
        - $(".dataTables_scrollHead").height()
        - $("#contentMenu").height()
        - $("#contentFooter").height()
        - 80
        ;

    return alto > 220 ? alto : 220;
}

function ubicacion() {
    $.ajax({
        type: 'POST',
        url: controler + "ListarCboUbicacion",
        datatype: 'json',

        data: JSON.stringify({ idEmpresa: $('#Emprcodi').val() }),

        contentType: "application/json",
        success: function (modelo) {
            $("#cboUbicacion").empty();
            $('#cboEquipo').empty();
            $.each(modelo.ListaCboUbicacion, function (k, v) {
                var option = '<option value =' + v.Areacodi + '>' + v.Areanomb + '</option>';
                $('#cboUbicacion').append(option);
            })
        },
        error: function () {
            alert("Error inesperado", $('#cboEmpresa').val());
        }
    });
}

function listarEquipoFiltro() {

    var idTipoPrograma = parseInt(document.getElementById('idTipoProgramacion').value) || 0;

    $.ajax({
        type: 'POST',
        url: controler + "ListarEquiposXprograma",
        datatype: 'json',
        data: JSON.stringify({ idUbicacion: $('#cboUbicacion').val(), idFamilia: $('#cboFamilia').val(), evenclasecodi: idTipoPrograma }),
        contentType: "application/json",
        success: function (modelo) {
            $('#cboEquipo').empty();
            $.each(modelo.ListaCboEquipo, function (k, v) {
                var option = '<option value =' + v.Entero1 + '>' + v.String1 + '</option>';
                $('#cboEquipo').append(option);
            })
            $('#cboEquipo').multipleSelect({
                filter: true,
                placeholder: "SELECCIONE"
            });
            $("#cboEquipo").multipleSelect("refresh");
            $('#cboEquipo').multipleSelect('checkAll');
        }
    });
}

function mostrarLista() {
    $("#chkSeleccionar").prop('checked', false);

    $('#listado').html('');
    ANCHO_LISTADO = $("#mainLayout").width() > 900 ? $("#mainLayout").width() - 25 : 900;

    var objData = getObjetoFiltro();
    var msj = validarConsulta(objData);

    if (msj == "") {
        LISTA_PLANTILLA_FORMULARIO_EXCLUSION = [];
        LISTA_PLANTILLA_FORMULARIO_INCLUSION = [];
        LISTA_PLANTILLA_FORMULARIO_INTERCODIS = '';
        $("#div_lista_int_sustento").html('');
        $("#div_lista_int_sustento").hide();

        $.ajax({
            type: 'POST',
            url: controler + "MostrarListadoRegistro",
            data: objData,
            dataType: 'json',
            success: function (evt) {
                if (evt.Resultado != "-1") {
                    //verificar obligatoriedad de transferir
                    if (evt.ListaFilaWeb.length > 0) ES_OBIGLATORIO_TRANSFERIR = false;

                    //html
                    $("#listado").hide();
                    $('#listado').css("width", ANCHO_LISTADO + "px");

                    var html = _dibujarTablaListado(evt);
                    $('#listado').html(html);
                    var nuevoTamanioTabla = getHeightTablaListado();
                    if (evt.IdTipoProgramacion == 1 || evt.IdTipoProgramacion == 2) nuevoTamanioTabla -= 25;

                    //Alerta creacion de interv por coordinador 
                    $("#leyenda_modificado_agente").hide();

                    var alertaMsj = evt.ListaIntervCount;
                    if (document.getElementById('idTipoProgramacion').value == "1" && alertaMsj > 0) {
                        $("#leyenda_modificado_agente").show();
                    }

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

                    viewEvent();
                    if (evt.ListaNotificaciones.length > 0)
                        mostrarPopupNotificacion(evt);

                } else {
                    alert(evt.Mensaje);
                }
            },
            error: function (err) {
                alert("Lo sentimos, ha ocurrido un error inesperado");
            }
        });
    } else {
        alert(msj);
    }
}

function _dibujarTablaListado(model) {
    var lista = model.ListaFilaWeb;

    var cadena = '';
    cadena += `
    <table border="0" class="pretty tabla-icono" id="TablaConsultaIntervencion" style="overflow: auto; height:auto; width: 2000px !important; white-space: nowrap">
        <thead>
            <tr>
                <th style="text-align:center;width:2%">Sel.</th>
                <th style="text-align:center;width:4%">Acción</th>
                <th style="text-align:center;width:3%">Arch.</th>
                <th style="text-align:center;width:6%">Tip.<br> Interv.</th>
                <th style="text-align:center;width:6%">Estado</th>
                <th style="text-align:center;width:6%">Empresa</th>
                <th style="text-align:center;width:6%">Ubicacion</th>
                <th style="text-align:center;width:6%">Tipo</th>
                <th style="text-align:center;width:6%">Equipo</th>
                <th style="text-align:center;width:5%">Fecha<br> inicio</th>
                <th style="text-align:center;width:5%">Fecha<br> fin</th>
                <th style="text-align:center;width:5%">MW <br>Ind.</th>
                <th style="text-align:center;width:5%">Disp.</th>
                <th style="text-align:center;width:5%">Interrup.</th>
                <th style="text-align:center;width:5%">Sist. Aisl.</th>
                <th style="text-align:center;width:5%">Inst. Prov.</th>
                <th style="text-align:center;width:10%">Descripción</th>
                <th style="text-align:center;width:5%">Mensajes <br>enviados</th>
                <th style="text-align:center;width:5%">Usuario Mod.</th>
                <th style="text-align:center;width:5%">Fec. Mod.</th>
            </tr>
        </thead>
        <tbody>
    `;

    for (key in lista) {
        var item = lista[key];

        //la fila está seleccionada
        var checkedSelec = "";
        if (item.ChkAprobacion) checkedSelec = ' checked ';

        //la fila tiene mensaje
        var checkedMsj = "";
        if (item.ChkMensaje) checkedMsj = ' checked ';

        //intervención mayor a 1 día
        var styleFraccionado = item.EstaFraccionado ? "; font-weight: bold;color: blue;" : "";

        //intervención con rago de hora
        var styleConnsecutivoXHora = item.EsConsecutivoRangoHora ? "; font-weight: bold;color: green;" : "";

        //
        var sdisabled = "";
        var sStyle = "";
        if (item.Interfuentestado <= 4 && model.IdTipoProgramacion == 1) {
            //creado por el Coordinador
            sStyle = "background-color:#ffe5ff;";
        }
        if (item.Estadocodi == 3) {   // Rechazado
            sStyle = "background-color:#FF2C2C; text-decoration:line-through";
            sdisabled = "disabled";
        }
        else if (item.Interdeleted == 1) {   // Eliminado
            sStyle = "background-color:#E0DADA; text-decoration:line-through";
            sdisabled = "disabled";
        }

        //
        var tdOpciones = _tdOpcionesXInter(model, item, sdisabled, sStyle);
        var tdFile = _tdFile(model, item, sdisabled, sStyle);

        //habilitar  check cuando no sea eliminado ni rechazado
        var tdSelec = `<input type="checkbox" ${checkedSelec} class="ChkSeleccion" value="${item.Intercodi}" name="ChkNameSeleccion" id="${item.Intercodi}" ${sdisabled} />`;
        if (item.Estadocodi == 3 || item.Interdeleted == 1) tdSelec = "";

        cadena += `
            
            <tr id="fila_${item.Intercodi}">
                <td style="${sStyle}">
                    ${tdSelec}
                </td>
                ${tdOpciones}
                ${tdFile}
                <td style="text-align:left; ${sStyle}">${item.Tipoevenabrev}</td>
                <td style="text-align:left; ${sStyle}">${item.EstadoRegistro}</td>
                <td style="text-align:left; ${sStyle}">${item.EmprNomb}</td>
                <td style="text-align:left; ${sStyle}">${item.AreaNomb}</td>
                <td style="text-align:left; ${sStyle}">${item.Famabrev}</td>
                <td style="text-align:left; ${sStyle}">${item.Equiabrev}</td>
                <td style="text-align:center; ${sStyle} ${styleFraccionado} ${styleConnsecutivoXHora}">${item.InterfechainiDesc}</td>
                <td style="text-align:center; ${sStyle} ${styleFraccionado} ${styleConnsecutivoXHora}">${item.InterfechafinDesc}</td>
                <td style="text-align:right; ${sStyle}">${item.Intermwindispo}</td>
                <td style="text-align:center; ${sStyle}">${item.InterindispoDesc}</td>
                <td style="text-align:center; ${sStyle}">${item.InterinterrupDesc}</td>
                <td style="text-align:center; ${sStyle}">${item.IntersistemaaisladoDesc}</td>
                <td style="text-align:center; ${sStyle}">${item.InterconexionprovDesc}</td>
                <td style="text-align:left; ${sStyle}">${item.Interdescrip}</td>
                <td style="${sStyle}">
                    <input type="checkbox" ${checkedMsj} class="ChkSeleccion1" disabled="disabled"  />
                </td>
                <td style="text-align:left; ${sStyle}">${item.UltimaModificacionUsuarioDesc}</td>
                <td style="text-align:left; ${sStyle}">${item.UltimaModificacionFechaDesc}</td>
            </tr>
        `;
    }
    cadena += "</tbody></table>";

    return cadena;
}

function _tdOpcionesXInter(Model, item, sdisabled, sStyle) {
    var html = '';

    var tagA = `
            <a href="#" id="view, ${item.Intercodi}" class="viewLogTrazabilidad">
                ${IMG_TRAZABILIDAD}
            </a>`;
    if (item.Intercodsegempr == null || item.Intercodsegempr == "") tagA = "";

    var htmlImgComunicacion = '';
    switch (item.TipoComunicacion) {
        case 1: htmlImgComunicacion = IMG_COMUNICACION_NINGUNA;
            break;
        case 2: htmlImgComunicacion = IMG_COMUNICACION_TODO_LEIDO;
            break;
        case 3: htmlImgComunicacion = IMG_COMUNICACION_PENDIENTE_LEER;
            break;
        case 4: htmlImgComunicacion = IMG_COMUNICACION_EXISTE_MENSAJE;
            break;
    }

    if (sdisabled == "") {
        html += `<td style="text-align: left; ${sStyle}">`;

        if (Model.EsCerrado) {
            html += `<a href="#" id="view, ${item.Intercodi}" class="viewEdicion">
                        ${IMG_VER}
                    </a>
                `;
        }
        else {
            html += `   
                <a href="#" id="view, ${item.Intercodi}" class="viewEdicion">
                    ${IMG_EDITAR}
                </a>
            `;
        }
        html += `   
            <a href="#" id="view, ${item.Intercodi}" class="viewComunicaciones">
                ${htmlImgComunicacion}
            </a>
            <a href="#" id="view, ${item.Intercodi}" class="viewLog">
                ${IMG_MODIFICACION}
            </a>
            ${tagA}
        </td>`;
    }
    else {
        html += `<td style="text-align:right; ${sStyle}">
            <a href="#" id="view, ${item.Intercodi}" class="viewEdicion">
                        ${IMG_VER}
            </a>
            <a href="#" id="view, ${item.Intercodi}" class="viewComunicaciones">
                ${htmlImgComunicacion}
            </a>
            <a href="#" id="view, ${item.Intercodi}" class="viewLog">
                ${IMG_MODIFICACION}
            </a>
            ${tagA}
        </td>`;
    }

    return html;
}

function _tdFile(Model, item, sdisabled, sStyle) {
    var html = `
        <td style="text-align:center; ${sStyle}">
    `;

    if (item.Interisfiles == 'S') {
        html += `
            <a href="#" id="view, ${item.Intercodi}" class="viewDescargaAdjunto">
                ${IMG_FILE}
                </a>
        `;
    }

    if (item.Interflagsustento == 1) {
        html += `
            <a href="#" id="view, ${item.Intercodi}" class="viewSustentoPdf">
                ${IMG_SUSTENTO}
                </a>
        `;
    }

    html += `
        </td>`;

    return html;
}

async function eliminarMasivo() {
    var intercodis = listarIntercodiChecked();
    var dataJson = null;

    if (intercodis != null && intercodis != "") {

        await st_VerificarListaIntervencionPuedeTenerSustento();

        var flagContinuar = true;
        if (!st_FlagTieneListaFormularioCompleto()) {
            flagContinuar = false;
        } else {
            flagContinuar = true;
            dataJson = JSON.stringify(st_ObtenerJsonLista(TIPO_EXCLUSION));
        }

        //guardar datos del formulario
        if (flagContinuar && confirm("¿Desea eliminar la información seleccionada?")) {
            eliminarMasivoBD(intercodis, dataJson);
        }

    } else {
        alert("Seleccione registro por eliminar.");
    }
}

function eliminarMasivoBD(intercodis, dataJson) {

    $.ajax({
        type: 'POST',
        url: controler + 'EliminarSeleccionados',
        data: {
            intercodis: intercodis,
            dataJson: dataJson
        },
        success: function (evt) {
            if (evt.Resultado != "-1") {
                $('input[type=checkbox].ChkSeleccion').prop('checked', false);
                $("#chkSeleccionar").prop('checked', false); // Deschekea el check seleccionar todos

                mostrarLista();
                document.getElementById('alerta').innerHTML = "<div class='action-exito '>" + evt.Mensaje + "</div>";
                $("#alerta").fadeOut(1600).fadeIn(1600).fadeOut(1000).fadeIn(1000).fadeOut(1000);
            } else {
                document.getElementById('alerta').innerHTML = "<div class='action-error '>" + evt.Mensaje + "</div>";
                $("#alerta").fadeOut(1600).fadeIn(1600).fadeOut(1000).fadeIn(1000).fadeOut(1000);
            }
        },
        error: function () {
            document.getElementById('alerta').innerHTML = "<div class='action-error mensajes'>Ocurrió un error al eliminar registro(s)</div>";
        }
    });
}

function reporteIntervencionesXls() {
    var objData = getObjetoFiltro();
    var msj = validarConsulta(objData);

    if (msj == "") {
        $.ajax({
            type: 'POST',
            url: controler + 'ExportarIntervenciones',
            data: objData,
            dataType: 'json',
            success: function (evt) {
                if (evt.Resultado != "-1") {
                    var paramList = [
                        { tipo: 'input', nombre: 'file', value: evt.NombreArchivo }
                    ];
                    var form = CreateForm(controler + 'AbrirArchivo', paramList);
                    document.body.appendChild(form);
                    form.submit();
                }
                else {
                    alert('Ha ocurrido un error');
                }
            },
            error: function (err) {
                alert('Ha ocurrido un error');
            }
        });
    } else {
        alert(msj);
    }
}

function mensaje() {
    var progrcodi = document.getElementById('Progrcodi').value;
    var tipoProgramacion = document.getElementById('idTipoProgramacion').value;
    var intercodis = listarIntercodiChecked();

    if (intercodis == "") {
        alert("Debe seleccionar una o varias intervenciones.");
    } else {
        //Modificación 15/05/2019
        var paramList = [
            { tipo: 'input', nombre: 'intercodis', value: intercodis },
            { tipo: 'input', nombre: 'progrcodi', value: progrcodi },
            { tipo: 'input', nombre: 'evenclasecodi', value: tipoProgramacion },
        ];
        var form = CreateForm(controler + 'IntervencionesMensajeRegistro', paramList);
        document.body.appendChild(form);
        form.submit();
    }

    return false;
}

function listarIntercodiChecked() {
    var selected = [];
    $('input[type=checkbox].ChkSeleccion').each(function () {
        if ($(this).is(":checked")) {
            selected.push($(this).attr('id'));
        }
    });

    return selected.join(";");
}

// VALIDAR PARA transferir a la BD las intervenciones de un horizonte superior
function copiarIntervenciones() {
    if (confirm("¿Desea transferir la información de intervenciones?")) {

        var idTipoprog = parseInt($("#idTipoProgramacion").val()) || 0;

        //se mostrará las validaciones si se pasa la fecha actual y con horas de operación
        if (EVENCLASECODI_EJECUTADO == idTipoprog) {
            validarCopiarProgramados();
        } else {
            transferirIntervenciones();
        }
    }
}

// Copiar o transferir a la BD las intervenciones de un horizonte superior
function transferirIntervenciones() {
    var progrcodi = document.getElementById('Progrcodi').value;
    var tipoProgramacion = document.getElementById('idTipoProgramacion').value;

    $.ajax({
        type: 'POST',
        url: controler + 'CopiarIntervenciones',
        data: {
            idProgramacion: progrcodi,
            idTipoProgramacion: tipoProgramacion
        },
        dataType: 'json',
        success: function (result) {
            if (result.Resultado != "-1") {
                mostrarLista(1);
                alert("¡Se ha realizado correctamente la transferencia de registros de intervenciones!")
                $('#popupListaProgramados').bPopup().close();
            } else {
                alert(result.Mensaje);
            }
        },
        error: function (err) {
            alert("Ha ocurrido un error");
        }
    });
}

function generarExcelXls() {
    var progrcodi = document.getElementById('Progrcodi').value;

    $.ajax({
        type: 'POST',
        url: controler + 'ExportarIntervencionesXLS',
        data: {
            idProgramacion: progrcodi
        },
        dataType: 'json',
        success: function (result) {

            alert(result.Mensaje);

            if (result.Resultado != "-1") {
                var paramList = [
                    { tipo: 'input', nombre: 'file', value: result.NombreArchivo }
                ];
                var form = CreateForm(controler + 'abrirarchivo', paramList);
                document.body.appendChild(form);
                form.submit();
            }
        },
        error: function () {
            alert('Ha ocurrido un error');
        }
    });
}

function generarManttoConsultaRegistro() {
    var objData = getObjetoFiltro();
    var msj = validarConsulta(objData);

    if (msj == "") {
        $.ajax({
            type: 'POST',
            url: controler + 'GenerarManttoConsultaRegistro',
            data: objData,
            dataType: 'json',
            success: function (evt) {
                if (evt.Resultado != "-1") {
                    var paramList = [
                        { tipo: 'input', nombre: 'file', value: evt.NombreArchivo }
                    ];
                    var form = CreateForm(controler + 'AbrirArchivo', paramList);
                    document.body.appendChild(form);
                    form.submit();
                }
                else {
                    alert('Ha ocurrido un error');
                }
            },
            error: function (err) {
                alert('Ha ocurrido un error');
            }
        });
    } else {
        alert(msj);
    }
}

//Descargar plantilla
function descargarManttoPlantilla() {

    var objData = getObjetoFiltro();
    var msj = validarConsulta(objData);

    if (msj == "") {
        $.ajax({
            type: 'POST',
            url: controler + 'DescargarManttoPlantillaActualizada',
            data: objData,
            dataType: 'json',
            success: function (evt) {
                if (evt.Resultado != "-1") {
                    var paramList = [
                        { tipo: 'input', nombre: 'file', value: evt.NombreArchivo }
                    ];
                    var form = CreateForm(controler + 'AbrirArchivo', paramList);
                    document.body.appendChild(form);
                    form.submit();
                }
                else {
                    alert('Ha ocurrido un error');
                }
            },
            error: function (err) {
                alert('Ha ocurrido un error');
            }
        });
    } else {
        alert(msj);
    }
}

//
function abrirPopupTrazabilidad(interCodi, tipoProgramacion) {
    $.ajax({
        type: 'POST',
        url: controler + "ListadoTrazabilidad",
        data: {
            interCodi: interCodi, tipoProgramacion: tipoProgramacion
        },
        success: function (evt) {
            $('#popup').html(evt);
            $('#popup').bPopup({
                modalClose: false,
                easing: 'easeOutBack',
                speed: 450,
                transition: 'slideDown',
                position: [10, 40]
            });
        },
        error: function () {
            alert("Lo sentimos, ha ocurrido un error inesperado");
        }
    });
}

function descargarFileTrazabilidad(fullPath, filename) {

    //Observación parametros en la URL - Modificado 15/05/2019
    var paramList = [
        { tipo: 'input', nombre: 'fullPath', value: fullPath },
        { tipo: 'input', nombre: 'filename', value: filename }
    ];
    var form = CreateForm(controler + 'DescargarArchivoDesdeCualquierDirectorio', paramList);
    document.body.appendChild(form);
    form.submit();
    //:fin

    //window.location = controler + 'DescargarArchivoDesdeCualquierDirectorio?fullPath=' + fullPath + '&&filename=' + filename;
}

function descargarFileSustento(intercodi) {
    window.location = controler + 'DescargarPDFSustento?intercodi=' + intercodi;
}

function viewEvent() {
    $('.viewEdicion').click(function (event) {
        event.preventDefault();
        interCodi = $(this).attr("id").split(",")[1];
        abrirPopupEdicion(interCodi);
    });

    $('.viewComunicaciones').click(function (event) {
        event.preventDefault();
        interCodi = $(this).attr("id").split(",")[1];
        abrirPopupComunicaciones(interCodi);
    });

    $('.viewLog').click(function (event) {
        event.preventDefault();
        interCodi = $(this).attr("id").split(",")[1];
        abrirPopupLog(interCodi);
    });

    $('.viewLogTrazabilidad').click(function (event) {
        event.preventDefault();
        interCodi = $(this).attr("id").split(",")[1];
        abrirPopupLogTrazabilidad(interCodi);
    });

    $('.viewSustentoPdf').click(function (event) {
        event.preventDefault();
        interCodi = $(this).attr("id").split(",")[1];
        descargarFileSustento(interCodi);
    });

    $('.viewDescargaAdjunto').click(function (event) {
        event.preventDefault();
        interCodi = $(this).attr("id").split(",")[1];
        _descargarArchivosAdjuntosXId(interCodi);
    });
};

function abrirPopupEdicion(interCodi) {
    $('#popupFormIntervencion').html('');

    var objParam = {
        interCodi: interCodi,
        progrCodi: 0,
        tipoProgramacion: 0,
        escruzadas: false,
    };

    //IntervencionesFormulario.js
    mostrarIntervencion(objParam);
}

function abrirPopupNuevo() {
    APP_MOSTRAR_MODAL_EQUIPO_YA_CARGADO = 0;

    $('#popupFormIntervencion').html('');

    var objParam = {
        interCodi: 0,
        progrCodi: parseInt($("#Progrcodi").val()) || 0,
        tipoProgramacion: parseInt($("#idTipoProgramacion").val()) || 0,
        escruzadas: false,
    };

    //IntervencionesFormulario.js
    mostrarIntervencion(objParam);
}

function abrirPopupLog(interCodi) {
    $.ajax({
        type: 'POST',
        url: controler + "ListadoModificaciones",
        data: {
            interCodi: interCodi
        },
        success: function (evt) {
            $('#popup').html(evt);

            $('#popup').bPopup({
                modalClose: false,
                easing: 'easeOutBack',
                speed: 50,
                transition: 'slideDown'
            });

            setTimeout(function () {
                $('#TablaConsultaMensajes').dataTable({
                    "ordering": true,
                    "info": false,
                    "sDom": 'ft',
                    "searching": false,
                    "paging": false,
                    "scrollX": true,
                    scrollCollapse: false,
                    "destroy": "true",
                    "bAutoWidth": false,
                    "scrollY": 500 + "px"
                });

            }, 150);
        },
        error: function (err) {
            alert("Lo sentimos, ha ocurrido un error inesperado");
        }
    });
}

function abrirPopupLogTrazabilidad(interCodi) {
    $.ajax({
        type: 'POST',
        url: controler + "ListadoTrazabilidad",
        data: {
            interCodi: interCodi
        },
        success: function (evt) {
            $('#popup').html(evt);

            setTimeout(function () {
                $('#popup').bPopup({
                    modalClose: false,
                    easing: 'easeOutBack',
                    speed: 450,
                    transition: 'slideDown'
                });
            }, 50);
        },
        error: function () {
            alert("Lo sentimos, ha ocurrido un error inesperado");
        }
    });
}

///////////////////////////////////////////////////////////////////////////////////////
// Utilitario
///////////////////////////////////////////////////////////////////////////////////////
function validarConsulta(objFiltro) {
    var listaMsj = [];

    //evenclase
    if (objFiltro.tipoProgramacion <= 0)
        listaMsj.push("Seleccione Tipo de programación");

    //Tipo intervención
    if (objFiltro.TipoEvenCodi == "")
        listaMsj.push("Tipo de intervención: Seleccione una opción.");
    //Estado
    if (objFiltro.EstadoCodi == "")
        listaMsj.push("Estado: Seleccione una opción.");
    //Ubicación
    if (objFiltro.AreaCodi == "")
        listaMsj.push("Ubicación: Seleccione una opción.");
    //Tipo equipo
    if (objFiltro.FamCodi == "")
        listaMsj.push("Tipo de equipo: Seleccione una opción.");
    //Equipo
    //

    //Empresa
    if (objFiltro.Emprcodi == "")
        listaMsj.push("Empresa: Seleccione una opción.");
    //Disponibilidad
    if (objFiltro.InterIndispo == "")
        listaMsj.push("Disponibilidad: Seleccione una opción.");

    // Valida filtros de fecha cuando falta seleccionar cualquiera de ellas
    if (objFiltro.InterFechaIni == "") {
        listaMsj.push("Seleccione una fecha de inicio");
    }
    if (objFiltro.InterFechaFin == "") {
        listaMsj.push("Seleccione una fecha de fin");
    }

    // Valida consistencia del rango de fechas
    var interfechaini = toDate(objFiltro.InterFechaIni).toISOString();
    var interfechafin = toDate(objFiltro.InterFechaFin).toISOString();
    if (interfechaini != "" && interfechafin != "") {
        if (CompararFechas(interfechaini, interfechafin) == false) {
            listaMsj.push("La fecha de inicio no puede ser mayor que la fecha de fin");
        }
    }

    var msj = listaMsj.join('\n');
    return msj;
}

function getObjetoFiltro() {
    var progrCodi = parseInt($('#Progrcodi').val()) || 0;
    var tipoProgramacion = parseInt($('#idTipoProgramacion').val()) || 0;

    var interFechaIni = $('#InterfechainiD').val();
    var interFechaFin = $('#InterfechafinD').val();

    var emprCodi = $('#Empresa').multipleSelect('getSelects').join(',');
    var tipoEvenCodi = $('#cboTipoIntervencion').multipleSelect('getSelects').join(',');
    var estadoCodi = $('#estadocodi').multipleSelect('getSelects').join(',');
    var areaCodi = $('#cboUbicacion').multipleSelect('getSelects').join(',');
    //var famCodi = $('#cboFamilia').multipleSelect('getSelects').join(',');
    var famCodi = $('#cboFamilia').val() != null && $('#cboFamilia')[0].length == $('#cboFamilia').val().length ? "0" : $('#cboFamilia').multipleSelect('getSelects').join(',');
    var equicodi = $('#cboEquipo').val() != null && $('#cboEquipo')[0].length == $('#cboEquipo').val().length ? "0" : $('#cboEquipo').multipleSelect('getSelects').join(',');
    //var equicodi = $('#cboEquipo').multipleSelect('getSelects').join(',');

    var interIndispo = $('#InterDispo').multipleSelect('getSelects').join(',');

    var descripcion = $('#txtNombreFiltro').val();
    var estadoEliminado = getEstado();

    var estadoFiles = getEstadoFiles();
    var estadoMensaje = getCheckMensaje();
    var aprobacion = listarIntercodiChecked();

    var tipoGrupoEquipo = $('#cboConjuntoEq').val();

    var obj = {
        Progrcodi: progrCodi,
        TipoProgramacion: tipoProgramacion,

        InterFechaIni: interFechaIni,
        InterFechaFin: interFechaFin,

        Emprcodi: emprCodi,
        TipoEvenCodi: tipoEvenCodi,
        EstadoCodi: estadoCodi,
        AreaCodi: areaCodi,
        FamCodi: famCodi,
        Equicodi: equicodi,

        InterIndispo: interIndispo,

        Descripcion: descripcion,
        EstadoEliminado: estadoEliminado,
        EstadoFiles: estadoFiles,
        EstadoMensaje: estadoMensaje,

        CheckIntercodi: aprobacion,
        TipoGrupoEquipo: tipoGrupoEquipo,
    };

    return obj;
}

function toDate(dateStr) {
    var parts = dateStr.split("/");
    return new Date(parts[2], parts[1] - 1, parts[0]);
}

//check eliminados
function getEstado() {
    var estado = "0";
    if ($('#check_eliminado').is(':checked')) {
        estado = '-1';
    }
    return estado;
}

//check Mostrar Archivos
function getEstadoFiles() {
    var estado = "0";
    if ($('#check_mostrarAdjuntos').is(':checked')) {
        estado = '1';
    }
    return estado;
}

//check mensajes
function getCheckMensaje() {
    var estado = "0";
    if ($('#check_mostrarMensajes').is(':checked')) {
        estado = '1';
    }
    return estado;
}

function CompararFechas(fecha1, fecha2) {

    //Split de las fechas recibidas para separarlas
    var x = fecha1.split('/');
    var z = fecha2.split('/');

    //Cambiamos el orden al formato americano, de esto dd/mm/yyyy a esto mm/dd/yyyy
    fecha1 = x[1] + '/' + x[0] + '/' + x[2];
    fecha2 = z[1] + '/' + z[0] + '/' + z[2];

    //Comparamos las fechas
    if (Date.parse(fecha1) > Date.parse(fecha2)) {
        return false;
    } else {
        return true;
    }
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

/////////////////////////////////////////////////////////////////////////

//popup programados con horarios iguales al ejecutado
function validarCopiarProgramados() {
    $('#formProgramados').html('');
    $.ajax({
        type: 'POST',
        url: controler + "ValidarCopiarProgramados",
        data: {
            idProgramacion: document.getElementById('Progrcodi').value,
            idTipoProgramacion: document.getElementById('idTipoProgramacion').value
        },
        success: function (dataHtml) {
            mostrarPopupProgramados(dataHtml);
        },
        error: function (err) {
            alert("Ha ocurrido un error");
        }
    });
}

function mostrarPopupProgramados(dataHtml) {
    $('#formProgramados').html(dataHtml);

    var excep_resultado = parseInt($("#hdResultado").val()) || 0;
    var excep_mensaje = $("#hdMensaje").val();
    var excep_detalle = $("#hdDetalle").val();

    if (excep_resultado == -1) {
        alert(excep_mensaje);
    } else {
        if (excep_resultado == -3) {
            alert("¡Ya se ha realizado la copia de registros!")
        } else {
            if (excep_resultado == -2) {
                $('#popupListaProgramados').bPopup().close();
                transferirIntervenciones();
            }
            else {
                setTimeout(function () {
                    $('#popupListaProgramados').bPopup({
                        easing: 'easeOutBack',
                        speed: 450,
                        transition: 'slideDown',
                        modalClose: true
                    });
                }, 50);
            }
        }
    }
}

/**
 * Comunicaciones
 * */
function abrirPopupComunicaciones(interCodi) {
    $("#hfIntercodiMsj").val(interCodi);

    listarMensajes(true);
}

var LISTA_MENSAJE_X_INT = [];

function listarMensajes(abrirPopup) {
    var interCodi = $("#hfIntercodiMsj").val();
    var tipoRemitente = $("#ddl-sender").val();
    var estadoMensaje = $("#ddl-state").val();

    $("#div-comments").show();
    $("#div-msg-contenido").hide();
    $("#lst-comments").html('');

    ARRAY_FILES_MENSAJE = [];

    $.ajax({
        type: 'POST',
        url: controlador + "ListadoMensaje",
        dataType: 'json',
        data: {
            interCodi: interCodi,
            tipoRemitente: tipoRemitente,
            estadoMensaje: estadoMensaje
        },
        success: function (evt) {
            if (evt.Resultado != "-1") {
                LISTA_MENSAJE_X_INT = evt.ListaMensajes;

                $('#lst-comments').html(dibujarTablaMensajes(LISTA_MENSAJE_X_INT));

                if (abrirPopup) {
                    setTimeout(function () {
                        $('#popupMensajes').bPopup({
                            easing: 'easeOutBack',
                            speed: 450,
                            transition: 'slideDown',
                            modalClose: false
                        });
                    }, 50);
                }

            } else {
                alert(evt.StrMensaje);
            }
        },
        error: function (err) {
            alert("Ha ocurrido un error");
        }
    });
}


function dibujarTablaMensajes(lista) {
    var htmlListado = '';

    if (lista != null && lista.length > 0) {
        for (key in lista) {
            var reg = lista[key];

            var htmlCC = reg.Msgcc != null && reg.Msgcc != '' ? `CC: ${reg.Msgcc}` : "";

            var estiloLeido = '';
            if (reg.EsLeido) estiloLeido = `background-color: #EDF5FC;`;

            var estiloArchivo = '';
            if (reg.Msgflagadj == 1) {
                estiloArchivo = `background-image: url(${siteRoot}Content/Images/attachments.png); `;
            }

            var htmlLectura = '';
            if (reg.EsLeido) {
                var textoLeido = '';
                if (reg.FechaDescLectura != null && reg.FechaDescLectura != "") textoLeido = `Leído por ${reg.UsuarioLectura} en ${reg.FechaDescLectura}`;

                htmlLectura = `
                    <div style="font-style: italic;text-align: right; color: green;height: 24px; background-image: url(${siteRoot}Content/Images/comment-seen.png); 
                                background-position: right center; background-repeat: no-repeat;padding-right: 34px;">
                        ${textoLeido}
                    </div>
                `;
            } else {
                htmlLectura = `
                    <div style="font-style: italic;text-align: right; color: red;">
                        [No leído]
                    </div>
                `;
            }

            htmlListado += `
                <section onclick="verContenidoMensaje(${reg.Msgcodi});" style='cursor: pointer; ${estiloLeido}'>

                    <div style='display: inline-block;'>
                        <div style='width:895px; float: left;font-weight: bold; margin-bottom: 5px; font-size: 14px;' >
                            ${reg.Remitente} (${reg.Msgfrom})
                        </div>
                        <div style='width:120px; float: right; text-align: right' >
                            ${reg.MsgfeccreacionDesc}
                        </div>
                        <div style='width:30px; float: right; height: 24px; ${estiloArchivo}; background-position: right center; background-repeat: no-repeat;' >                            
                        </div>
                    </div>
                    <div>
                        <div>Para: ${reg.Msgto}</div>
                        <div style='word-break: break-word;'>${htmlCC}</div>
                    </div>

                    <div style='display: inline-block;padding-top: 8px;'>
                        <div style='width:1050px; float: left;' >
                            Asunto: ${reg.Msgasunto}
                        </div>
                    </div>
                    <div style="height: 24px;">
                        ${htmlLectura}
                    </div>
                </section>
        `;
        }
    }

    return htmlListado;
}

async function verContenidoMensaje(msgcodi) {

    $("#div-comments").hide();
    $("#div-msg-contenido").show();
    $("#div-detalle-msg-contenido").html('');

    var reg = buscarMensaje(msgcodi, LISTA_MENSAJE_X_INT);
    if (reg != null) {
        var interCodi = ($("#hfIntercodiMsj").val()).trim();

        //si el mensaje es del agente y no está leído
        if (reg.Remitente == "COES" && reg.Msgestado == "N") {
            await marcarComoLeido(interCodi, reg.Msgcodi);
        }

        var htmlCC = reg.Msgcc != null && reg.Msgcc != '' ? ` CC: ${reg.Msgcc}` : "";

        var html = `
            <section>
                <div style='display: inline-block;'>
                    <div style='width:1050px; float: left;FONT-SIZE: 18px;' >
                        ${reg.Msgasunto}
                    </div>
                    <div style='width:50px; float: right; text-align: right' >
                        <img src="${siteRoot}Content/Images/btn-regresar.png" style="width:20px; height:20px;cursor: pointer;" title="Regresar a listado de comunicaciones" onclick="volverAListadoMensaje();" />
                    </div>
                </div>

                <div class='linea_division_mensaje'></div>
                
                <div style='display: inline-block;'>
                    <div style='width:850px; float: left;font-weight: bold; margin-bottom: 5px; font-size: 14px;' >
                        ${reg.Remitente} (${reg.Msgfrom})
                    </div>
                    <div style='width:150px; float: right; text-align: right' >
                        ${reg.MsgfeccreacionDesc}
                    </div>
                </div>
                <div>
                    <div>Para: ${reg.Msgto}</div>
                    <div style='word-break: break-word;'>${htmlCC}</div>
                </div>

                <div class='linea_division_mensaje'></div>

                <div style='padding: 10px; border: 1px solid #dddddd; min-height: 150px;'>
                    ${reg.Msgcontenido}
                </div>

                <div>
                    <div id="html_listaArchivosMensaje">
                    </div>

                    <div id="listaArchivos2">
                        <iframe id="vistaprevia_contenido_mensaje" style="width: 100%; height:500px;" frameborder="0" hidden></iframe>
                    </div>
                </div>
            </section>
        `;

        $("#div-detalle-msg-contenido").html(html);

        //si tiene archivos, entonces mostrarlos con opción de vista previa
        LISTA_SECCION_ARCHIVO_X_MENSAJE = [];
        if (reg.Msgflagadj == 1) {

            var seccion = {
                Inpstidesc: 'Archivos adjuntos',
                EsEditable: false,
                ListaArchivo: reg.ListaArchivo,
                Modulo: TIPO_MODULO_MENSAJE,
                Progrcodi: reg.Progrcodi,
                Carpetafiles: reg.Msgcodi,
                Subcarpetafiles: 0,
                TipoArchivo: TIPO_ARCHIVO_MENSAJE,
                IdDiv: `html_listaArchivosMensaje`,
                IdDivVistaPrevia: 'vistaprevia_contenido_mensaje',
                IdPrefijo: arch_getIdPrefijo(0)
            };

            LISTA_SECCION_ARCHIVO_X_MENSAJE.push(seccion);

            arch_cargarHtmlArchivoEnPrograma(seccion.IdDiv, seccion);
        }
    }

}

function volverAListadoMensaje() {
    listarMensajes(false);
}

function buscarMensaje(codigo, lista) {
    if (lista.length > 0) {
        for (var i = 0; i < lista.length; i++) {
            if (lista[i].Msgcodi == codigo) {
                return lista[i];
            }
        }
    }

    return null;
}

async function marcarComoLeido(intercodi, msgcodi) {

    return $.ajax({
        type: 'POST',
        url: controlador + "MarcarMensajeLeido",
        dataType: 'json',
        data: {
            interCodi: intercodi,
            msgcodi: msgcodi
        },
        success: function (evt) {
            if (evt.Resultado != "-1") {

            } else {
                alert("Ha ocurrido un error: " + evt.StrMensaje);
            }
        },
        error: function (err) {
            alert("Ha ocurrido un error");
        }
    });
}

/**
 * Reporte comunicaciones
 * */

function generarIntervencionesRptComunicaciones() {
    var intercodis = listarIntercodiChecked();

    if (intercodis == "") {
        alert("Debe seleccionar una o varias intervenciones.");
    } else {
        $.ajax({
            type: 'POST',
            url: controler + 'DescargarReporteComunicacionSeleccionados',
            data: {
                intercodis: intercodis
            },
            dataType: 'json',
            success: function (evt) {
                if (evt.Resultado != "-1") {
                    var paramList = [
                        { tipo: 'input', nombre: 'file', value: evt.NombreArchivo }
                    ];
                    var form = CreateForm(controler + 'abrirarchivo', paramList);
                    document.body.appendChild(form);
                    form.submit();
                } else {
                    alert("Ha ocurrido un error: " + evt.Mensaje);
                }
            },
            error: function (err) {
                alert('Ha ocurrido un error');
            }
        });
    }
}

/**
 * Opciones de comunicaciones
 * */
function enviarRespuestaComunicacion() {
    var progrcodi = document.getElementById('Progrcodi').value;
    var tipoProgramacion = document.getElementById('idTipoProgramacion').value;
    var intercodis = ($("#hfIntercodiMsj").val()).trim();

    //Modificación 15/05/2019
    var paramList = [
        { tipo: 'input', nombre: 'intercodis', value: intercodis },
        { tipo: 'input', nombre: 'progrcodi', value: progrcodi },
        { tipo: 'input', nombre: 'evenclasecodi', value: tipoProgramacion },
    ];
    var form = CreateForm(controler + 'IntervencionesMensajeRegistro', paramList);
    document.body.appendChild(form);
    form.submit();

    return false;
}

function descargarPdfComunicacion() {
    var interCodi = ($("#hfIntercodiMsj").val()).trim();

    if (LISTA_MENSAJE_X_INT.length > 0) {
        window.location.href = controlador + `DownloadFilePdfListadoMensaje?interCodi=${interCodi}`;
    } else {
        alert("No existe mensajes de la intervención seleccionada.");
    }
}

function descargarZipComunicacion() {
    var interCodi = ($("#hfIntercodiMsj").val()).trim();

    $.ajax({
        type: 'POST',
        url: controler + 'DescargarZipXMensaje',
        data: { interCodi: interCodi },
        success: function (evt) {
            if (evt.Resultado != "-1") {

                window.location = controler + "ExportarZip?file_name=" + evt.Resultado;

            } else {
                alert("Ha ocurrido un error: " + evt.Mensaje);
            }
        },
        error: function (err) {
            alert("Error al descargar archivos");
        }
    });
}

function mostrarPopupNotificacion(model) {
    var intercodis = "";
    var lista = model.ListaNotificaciones;
    var sStyle = "";
    var styleFraccionado = "";
    var styleConnsecutivoXHora = "";
    var cadena = '';
    cadena += `
    <table border="0" class="pretty tabla-icono" id="TablaConsultaIntervencionNoLeido" style="white-space: nowrap">
        <thead>
            <tr>
                <th style="text-align:center;width:20%">Empresa</th>
                <th style="text-align:center;width:12%">Ubicacion</th>
                <th style="text-align:center;width:8%">Tipo</th>
                <th style="text-align:center;width:6%">Equipo</th>
                <th style="text-align:center;width:8%">Fecha<br> inicio</th>
                <th style="text-align:center;width:8%">Fecha<br> fin</th>
                <th style="text-align:center;width:22%">Descripción</th>
                <th style="text-align:center;width:8%">Usuario Mod.</th>
                <th style="text-align:center;width:8%">Fec. Mod.</th>
                <th style="text-align:center;width:8%">Estado Antiguo.</th>
                <th style="text-align:center;width:8%">Estado Nuevo.</th>
            </tr>
        </thead>
        <tbody>
    `;

    for (key in lista) {
        var item = lista[key];
        cadena += `
            
            <tr id="fila_${item.Intercodi}">
                <td style="text-align:left; ${sStyle}">${item.EmprNomb}</td>
                <td style="text-align:left; ${sStyle}">${item.AreaNomb}</td>
                <td style="text-align:left; ${sStyle}">${item.Famabrev}</td>
                <td style="text-align:left; ${sStyle}">${item.Equiabrev}</td>
                <td style="text-align:center; ${sStyle} ${styleFraccionado} ${styleConnsecutivoXHora}">${item.InterfechainiDesc}</td>
                <td style="text-align:center; ${sStyle} ${styleFraccionado} ${styleConnsecutivoXHora}">${item.InterfechafinDesc}</td>
                <td style="text-align:left; ${sStyle}">${item.Interdescrip}</td>
                <td style="text-align:left; ${sStyle}">${item.UltimaModificacionUsuarioDesc}</td>
                <td style="text-align:left; ${sStyle}">${item.UltimaModificacionFechaDesc}</td>
                <td style="text-align:left; ${sStyle}">${item.EstadoRegistroPadre}</td>
                <td style="text-align:left; ${sStyle}">${item.EstadoRegistro}</td>
            </tr>
        `;
        if (intercodis == "")
            intercodis = item.Intercodi;
        else
            intercodis += "," + item.Intercodi;
    }
    $('#hfIntercodisNotif').val(intercodis);
    cadena += "</tbody></table>";
    $('#idtitulo').text("");
    //alert(idtitulo);
    $('#tablaNotificacion').html(cadena);
    switch (model.IdTipoProgramacion) {
        case 2: $('#idtitulo').text("Cambios de Estado - Programado Diario");
            break;
        case 3: $('#idtitulo').text("Cambios de Estado - Programado Semanal");
            break;
        case 4: $('#idtitulo').text("Cambios de Estado - Programado Mensual");
            break;
        case 5: $('#idtitulo').text("Cambios de Estado - Programado Anaul");
            break;
    }

    $('#popupFormNotificacion').bPopup({
        modalClose: false,
        easing: 'easeOutBack',
        speed: 50,
        transition: 'slideDown'
    });

    setTimeout(function () {
        $('#TablaConsultaIntervencionNoLeido').dataTable({
            "ordering": true,
            "info": false,
            "sDom": 'ft',
            "searching": false,
            "paging": false,
            "scrollX": true,
            scrollCollapse: false,
            "destroy": "true",
            "bAutoWidth": false,
            "scrollY": 500 + "px"
        });

    }, 150);

    $('#btnCerrarNotif').prop('disabled', true);
    $('#btnCerrarNotif').css({ opacity: 0.3 });
    setTimeout(function () {
        $('#btnCerrarNotif').prop('disabled', false);
        $('#btnCerrarNotif').css({ opacity: 1 });
    }, 10000);

}

function CerrarNotificacion() {
    var codigos = $("#hfIntercodisNotif").val();
    $.ajax({
        type: "POST",
        url: controler + "ActualizarLeidoAgente",
        traditional: true,
        data: {
            intercodis: codigos
        },
        success: function (evt) {
            if (evt.Resultado != "-1") {
                $('#popupFormNotificacion').bPopup().close();
                //_mostrarMensajeAlertaTemporal(true, evt.StrMensaje);

            } else {
                alert(evt.StrMensaje);
            }
        },
        error: function (err) {
            alert("Lo sentimos, ha ocurrido un error inesperado");
        }
    });
}