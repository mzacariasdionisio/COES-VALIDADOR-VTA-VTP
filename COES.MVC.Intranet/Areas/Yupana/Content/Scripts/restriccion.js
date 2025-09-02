var controlador = siteRoot + 'Yupana/Restriccion/';

var tblErrorImportacion;
var RESTRICCIONES;
var listaDataGeneral;
var listaRelaciones;
var tblHndRestricciones;
var listErrores = [];
const colItem = 1;
const MOD = 1;
var fecRI = "";
var fecRF = "";
var fecRICopia = "";
var fecRFCopia = "";

$(function () {

    fecRI = $("#hfFecIniSem").val();
    fecRF = $("#hfFecIniSem").val();

    $('#Anho').Zebra_DatePicker({
        format: 'Y',
        onSelect: function () {
            cargarSemanaAnho();
        }
    });
    $('#txtFecha').Zebra_DatePicker({
        onSelect: function () {
            fecRI = $("#txtFecha").val();
            fecRF = $("#txtFecha").val();
        }
    });

    $('#cbHorizonte').change(function () {
        horizonte();
        //pintaDescripcion($('#cbFormato').val())
        cargarRestricciones();
    });

    $('#btnConsultar').click(function () {
        cargarRestricciones();
    });

    $('#btnConfigurar').on('click', function () {
        document.location.href = siteRoot + 'Yupana/Restriccion/ConfiguracionIndex';
    });

    $('#btnEnviarDatos').click(function () {
        enviarDatos();
    });

    $('#btnMostrarErrores').click(function () {
        mostrarListadoErrores();
    });

    $('#btnCopiar').click(function () {
        copiarDatos();
    });

    $('#btnDescargar').click(function () {
        reporteRestriccion();
    });

    tblErrorImportacion = $('#tblErroresImportacion').DataTable({
        "columns": [
            { "data": "NumFila" },
            { "data": "Campo" },
            { "data": "ErrorDescripcion" }
        ]
    });

    //Explorar archivos
    importarFormato();

    horizonte();
    cargarSemanaAnho();

    cargarRestricciones();
});

function importarFormato() {

    var uploader = new plupload.Uploader({
        runtimes: 'html5,flash,silverlight,html4',
        browse_button: "btnImportar",
        url: controlador + "Upload",
        flash_swf_url: 'Scripts/Moxie.swf',
        silverlight_xap_url: 'Scripts/Moxie.xap',
        multi_selection: false,
        filters: {
            max_file_size: '5mb',
            mime_types: [
                { title: "Archivos Excel .xlsx", extensions: "xlsx,xls" }
            ]
        },
        init: {
            FilesAdded: function (up, files) {
                if (uploader.files.length == 2) {
                    uploader.removeFile(uploader.files[0]);
                }
                uploader.start();
                up.refresh();
            },
            UploadProgress: function (up, file) {
            },
            UploadComplete: function (up, file) {
                leerFileUpExcel();
            },
            Error: function (up, err) {
                mostrarMensaje_('mensaje', 'error', 'Se ha producido un error:' + err.message);
            }
        }
    });

    uploader.init();
}

function leerFileUpExcel() {
    limpiarBarraMensaje('mensaje');

    $.ajax({
        type: 'POST',
        url: controlador + 'ImportarRestriccionesExcel',
        dataType: 'json',
        async: true,
        data: {
        },
        success: function (evt) {
            if (evt.Resultado == "1") {
                var data = evt.DataRestricciones;

                if (data.Errores && data.Errores.length > 0) {
                    cargarListaErroresImportacion(data.Errores);
                    setTimeout(function () {
                        $("#erroresImportacion").bPopup({
                            easing: 'easeOutBack',
                            speed: 450,
                            transition: 'slideDown',
                            modalClose: false
                        });
                    }, 50);

                    mostrarMensaje('mensajeErrorI', 'alert', 'El archivo importado contiene errores generales en su información. para corregirlo tome en cuenta las notas de cada celda en la plantilla.');
                }
                else {
                    //RESTRICCIONES = data.DatosRestricTotal;
                    inicializarRestricciones(evt);
                    iniciarHandsontables(data.NombresGeneral);
                    cargarHansonTablas(data);
                    //tblHndRestricciones.render();
                }

            } else {
                mostrarMensaje('mensaje', 'error', evt.Mensaje);
            }
        },
        error: function (err) {
            mostrarMensaje('mensaje', 'error', 'Ha ocurrido un error en leer archivo importado.');
        }
    });
}

function cargarListaErroresImportacion(lstErrores) {
    tblErrorImportacion.clear();
    tblErrorImportacion.rows.add(lstErrores).draw();
}

function cargarRestricciones() {

  var  idHorizonte = $("#cbHorizonte").val();
  var  idEnvio = 0;
  var  fecha = $("#txtFecha").val();
  var  semana = $("#cbSemana").val();
  var  anho = $("#Anho").val();
  semana = anho.toString() + semana;

    limpiarBarraMensaje("mensaje");

    $.ajax({
        type: 'POST',
        url: controlador + "CargarRestricciones",
        data: {
            formato: idHorizonte,
            fecha: fecha,
            semana: semana
        },
        success: function (evt) {

            if (evt.Resultado == "1") {
                var data = evt.DataRestricciones;
                RESTRICCIONES = data.DatosRestricTotal;
                inicializarRestricciones(evt);
                iniciarHandsontables(data.NombresGeneral);
                cargarHansonTablas(data);
                if (evt.IdEnvio > 0) {
                    var mensaje = "<strong>Código de envío</strong> : " + evt.IdEnvio + ", <strong>Fecha de envío: </strong>" + evt.FechaProceso;
                    mostrarMensaje('mensaje', 'exito', mensaje);
                }
                else
                    mostrarMensaje('mensaje', 'message', "Por favor complete la información");

            } else {
                mostrarMensaje('mensaje', 'error', evt.Mensaje);
            }
        },
        error: function (xhr, status) {
            mostrarMensaje('mensaje', 'error', 'Se ha producido un error.');
        }
    });

}

function copiarDatos(rescfgcodi) {

    limpiarBarraMensaje('mensaje');
    $.ajax({
        type: 'POST',
        url: controlador + "CopiarIndex",
        dataType: 'json',
        data: {
        },
        success: function (evt) {
            if (evt.Resultado != "-1") {

                $('#popupCopiar').html('');
                //Inicializar Formulario
                $("#popupCopiar").html(generarHtmlPoputCopiar(evt));

                setTimeout(function () {
                    $('#popupCopiar').bPopup({
                        easing: 'easeOutBack',
                        speed: 450,
                        transition: 'slideDown',
                        modalClose: false,
                    });
                }, 50);

                $('#AnhoCopia').Zebra_DatePicker({
                    format: 'Y',
                    onSelect: function () {
                        cargarSemanaAnhoCopia();
                    }
                });
                $('#txtFechaCopia').Zebra_DatePicker({
                });

                $('#cbHorizonteCopia').change(function () {
                    horizonteCopia();
                });

                horizonteCopia();
                cargarSemanaAnhoCopia();

                $('#txtFechaDestinoIni').unbind();
                $('#txtFechaDestinoIni').Zebra_DatePicker({
                    readonly_element: false,
                    direction: [fecRI, fecRF],
                });
                $('#txtFechaDestinoFin').unbind();
                $('#txtFechaDestinoFin').Zebra_DatePicker({
                    readonly_element: false,
                    direction: [fecRI, fecRF],
                });

                $('#btnCopiarRestriccion').on("click", function () {
                    grabarCopia();
                });

                $('#btnCancelarRestriccion').on("click", function () {
                    $('#popupCopiar').bPopup().close();
                });

            } else {
                alert("Ha ocurrido un error: " + evt.Mensaje);
            }
        },
        error: function (err) {
            alert("Error al cargar Concepto");
        }
    });
}

function grabarCopia() {

   var idHorizonte = $("#cbHorizonte").val();

   var idHorizonteCopia = $("#cbHorizonteCopia").val();
   var fechaCopia = $("#txtFechaCopia").val();
   var semanaCopia = $('#cbSemanaCopia').val();
   var anhoCopia = $('#AnhoCopia').val();
   semanaCopia = anhoCopia.toString() + semanaCopia;

    limpiarBarraMensaje("mensaje");


    if (idHorizonte == 3) { // destino semanal

        if (idHorizonteCopia != 3) // origen diario o reprograma (caso3)
        {
            const fechaIniciovalid = moment($("#txtFechaDestinoIni").val(), "DD/MM/YYYY").toDate();
            const fechaFinvalid = moment($("#txtFechaDestinoFin").val(), "DD/MM/YYYY").toDate();

            if (idHorizonte == 3 && (fechaIniciovalid > fechaFinvalid)) {
                mostrarMensaje('mensaje2', 'alert', "La fecha fin debe ser mayor a la fecha inicio");
                return;
            }
        }
        else {// origen semanal(caso4)
            const fechaIniciovalid = moment($("#txtFechaOrigenIni").val(), "DD/MM/YYYY").toDate();
            const fechaFinvalid = moment($("#txtFechaOrigenFin").val(), "DD/MM/YYYY").toDate();

            if (idHorizonte == 3 && (fechaIniciovalid > fechaFinvalid)) {
                mostrarMensaje('mensaje2', 'alert', "La fecha fin debe ser mayor a la fecha inicio");
                return;
            }
        }
    }


    $.ajax({
        url: controlador + "CargarRestricciones",
        type: 'POST',
        data: {
            formato: idHorizonteCopia,
            fecha: fechaCopia,
            semana: semanaCopia
        },
        success: function (evt) {
            if (evt.Resultado == "1") {
                var dataCopiar = evt.DataRestricciones.DatosRestricciones;
                var dataCopiaFormateada = procesoCopia(dataCopiar);

                if (dataCopiaFormateada.length > 0) {
                    $('#popupCopiar').bPopup().close();
                    // Obtener los datos actuales
                    var dataGuardar = obtenerDataGuardar();
                    let datosExistentes = dataGuardar.DatosRestricciones;
                    let datosActualizados = datosExistentes.concat(dataCopiaFormateada);
                    tblHndRestricciones.loadData(datosActualizados);
                    tblHndRestricciones.render();
                    actualizarValoresItems(tblHndRestricciones);

                    mostrarMensaje('mensaje', 'exito', 'La información fue copiada correctamente.');
                }
                else
                    alert("No se encontró informacion a copiar para la fecha seleccionada");
            } else {
                mostrarMensaje('mensaje', 'error', evt.Mensaje);
            }
        },
        error: function () {
            mostrarMensaje('mensaje', 'error', "No se ha podido realizar la acción");
        }
    });
}

function procesoCopia(dataCopiar) {

    var idHorizonte = $("#cbHorizonte").val();
    idHorizonteCopia = $("#cbHorizonteCopia").val();
    var dataCopiaFormateada = [];
    if (dataCopiar.length > 0) {

        var fechaInicio = "";
        var fechaFin = "";

        if (idHorizonte != 3) { // diario y reprograma toma el mismo dia inicio y fin
            fechaInicio = $("#txtFecha").val();
            fechaFin = $("#txtFecha").val();
        }
        else {
            fechaInicio = $("#txtFechaDestinoIni").val();
            fechaFin = $("#txtFechaDestinoFin").val();
        }

        if (idHorizonte == 3 && idHorizonteCopia != 3) { // si estamos en el semanal y queremos copiar de diario o reprograma (caso 3) 
            const fechaMin = moment(fechaInicio, "DD/MM/YYYY").toDate();
            const fechaMax = moment(fechaFin, "DD/MM/YYYY").toDate();

            for (let i = 0; i < dataCopiar.length; i++) {
                const item = dataCopiar[i];

                for (let f = moment(fechaMin); f.isSameOrBefore(fechaMax); f.add(1, 'days')) {
                    // Crear copia del objeto y asigna fecha actualizada
                    const nuevoItem = { ...item };
                    nuevoItem.Fecha = f.format("DD/MM/YYYY");
                    dataCopiaFormateada.push(nuevoItem);
                }
            }
        }
        else {

            if (idHorizonte != 3 && idHorizonteCopia != 3) // SI ES DE DIARIO/REPROGRAMA A DIARO/REPROGRANA SOLO CAMBIAMOS LA FECHA (caso 1)
            {
                dataCopiar.forEach(item => {
                    const nuevoItem = { ...item };
                    nuevoItem.Fecha = fechaInicio
                    dataCopiaFormateada.push(nuevoItem);
                });
            }
            else { //caso 2 y caso 4 funciona la misma lógica
                //primero filtramos el rango de fecha escogido del usuario
                const fechaOrigenIni = $("#txtFechaOrigenIni").val();
                const fechaOrigenFin = idHorizonte != 3 ? $("#txtFechaOrigenIni").val() : $("#txtFechaOrigenFin").val();
                const fechaOriIni = moment(fechaOrigenIni, "DD/MM/YYYY");
                const fechaOriFin = moment(fechaOrigenFin, "DD/MM/YYYY");

                const listaFiltrada = dataCopiar.filter(obj => {
                    const fechaObj = moment(obj.Fecha, "DD/MM/YYYY");
                    return fechaObj.isSameOrAfter(fechaOriIni) && fechaObj.isSameOrBefore(fechaOriFin);
                });

                const fechaInicio_ = moment(fechaInicio, "DD/MM/YYYY");
                const fechaFin_ = moment(fechaFin, "DD/MM/YYYY");
                const diasRango = fechaFin_.diff(fechaInicio_, 'days') + 1;

                const agrupado = {};
                listaFiltrada.forEach(obj => {
                    const nombre = obj.Rescfgnombre;
                    if (!agrupado[nombre]) agrupado[nombre] = [];
                    agrupado[nombre].push(obj);
                });

                Object.keys(agrupado).forEach(nombre => {
                    // 3. Ordenar por fecha ascendente
                    const grupoOrdenado = agrupado[nombre].sort((a, b) =>
                        moment(a.Fecha, "DD/MM/YYYY").diff(moment(b.Fecha, "DD/MM/YYYY"))
                    );

                    //Truncamos al tamaño del rango
                    const truncado = grupoOrdenado.slice(0, diasRango);

                    //Generamos objetos con nuevas fechas
                    let fechaActual = moment(fechaInicio_);
                    truncado.forEach(item => {
                        const nuevoItem = { ...item };
                        nuevoItem.Fecha = fechaActual.format("DD/MM/YYYY");
                        dataCopiaFormateada.push(nuevoItem);
                        fechaActual.add(1, 'days');
                    });
                });
            }
        }
    }
    else
        dataCopiaFormateada = dataCopiar;

    return dataCopiaFormateada;
}



//Generar html
function generarHtmlPoputCopiar(model) {

    var horizonte = $("#cbHorizonte").val();
    var htmlDestino = ``;
    var htmlOrigen = ``;

    if (horizonte == 3) {
        htmlOrigen = `
                     Fin:
                     <input type="text" name="txtFechaOrigenFin" id="txtFechaOrigenFin" style="display:inline-block" value="${fecRFCopia}" />
    `;
    }

    if (horizonte == 3) {
        htmlDestino = `
                  <b>Destino</b>
                    <div class="search-content" style="padding-top: 5px; padding-bottom: 1px;">
                     <table class="content-tabla-search" style="width:auto">
                        <tr>
                            <td>
                                    Inicio:
                                    <input type="text" name="FechaIni" id="txtFechaDestinoIni" value="${fecRI}" />
                            </td>
                            <td id="tdFechaDestinoFin" >
                                    Fin:
                                    <input type="text" name="FechaFin" id="txtFechaDestinoFin" value="${fecRF}" />
                            </td>
                        </tr>
                     </table>
                    </div>
    `;
    }


    var html = `
        <div><span class="button b-close"><span>X</span></span></div>
        <div class="popup-title">
            <span>Copiar Restricciones</span>
        </div>

        <div class="content-registro popup-text" style="height: 200px">

            <div class="content-hijo" id="mainLayoutEdit" style="padding-top:0px; padding-left: 0px; padding-right: 0px;">
            <div id="mensaje2" class="action-message">Escoge un horizonte del cual quieres obtener una copia</div>
            <b>Origen</b>
                <div class="search-content" style="padding-top: 5px; padding-bottom: 1px;">

                    <table class="content-tabla-search" style="width:auto">
                        <tr>
                            <td>Horizonte:</td>
                            <td>
                                <select id="cbHorizonteCopia" name="IdHorizonte" style="width:110px;">
                                    <option value="1">Diario</option>
                                    <option value="2">Reprograma</option>
                                    <option value="3">Semanal</option>
                                </select>
                                <input type="hidden" id="hfHorizonteCopia" />
                            </td>
                            <td>
                                <div id="dDiaCopia">
                                    Fecha:
                                    <input type="text" name="Fecha" id="txtFechaCopia" value="${model.Dia}" />
                                </div>
                                <div id="dSemanaCopia" style="display:none">
                                    Año:
                                    <input type="text" id="AnhoCopia" name="Año" style="width:70px;" value="${model.Anho}"/>
                                    <input type="hidden" id="hfAnhoCopia" value="${model.Anho}" />
                                    Semana:
                                    <div id="SemanaIniCopia" style="display:inline-block; margin-right: 20px;"> </div>
                                    <input type="hidden" id="hfSemanaCopia" value="${model.NroSemana}" />

                                    Inicio:
                                    <input type="text" name="txtFechaOrigenIni" id="txtFechaOrigenIni" style="" value="${fecRICopia}" />
                                     ${htmlOrigen}

                                </div>
                                <input type="hidden" id="hfFechaCopia" />

                            </td>
                        </tr>
                    </table>
          </div>
          ${htmlDestino}
                <div style="clear:both; width:200px; margin:auto; text-align:center; margin-top:20px">
                    <input type="button" id="btnCopiarRestriccion" value="Copiar" />
                    <input type="button" id="btnCancelarRestriccion" value="Cancelar" />
                </div>
           </div>
        </div>
    `;

    return html;
}

function inicializarRestricciones(evt) {
    listErrores = [];
    $('.leyenda-actividad-titulo').css("display", "block");
}

function iniciarHandsontables(nombres) {

    var dtpConfig = {
        firstDay: 1,
        showWeekNumber: false,
        showTimepicker: false,
        i18n: {
            previousMonth: 'Mes anterior',
            nextMonth: 'Mes siguiente',
            months: ['Enero', 'Febrero', 'Marzo', 'Abril', 'Mayo', 'Junio', 'Julio', 'Agosto', 'Septiembre', 'Octubre', 'Noviembre', 'Diciembre'],
            weekdays: ['Domingo', 'Lunes', 'Martes', 'Miercoles', 'Jueves', 'Viernes', 'Sabado'],
            weekdaysShort: ['Dom', 'Lun', 'Mar', 'Mier', 'Jue', 'Vie', 'Sab']
        },
        disableDayFn(date) {

            const fechaMin = moment(fecRI, "DD/MM/YYYY").toDate();
            const fechaMax = moment(fecRF, "DD/MM/YYYY").toDate();

            return date < fechaMin || date > fechaMax;
        }
    };

    /************** RESTRICCIONES *********************/
    $('#tblRestricciones').html("");
    // Inicio datos
    Handsontable.renderers.registerRenderer('datosRestriccionRenderer', datosRestriccionRenderer);
    Handsontable.renderers.registerRenderer('dateTimeRenderer', dateTimeRenderer);
    Handsontable.renderers.registerRenderer('datosCombosRestriccionRenderer', datosCombosRestriccionRenderer);

    //containerTablaModoOperacion = document.getElementById('tblRestricciones');
    tblHndRestricciones = new Handsontable(document.getElementById('tblRestricciones'), {
        dataSchema: {
            Item: null,
            Rescfgnombre: null,
            Fecha: null,
            H1: null, H2: null, H3: null, H4: null, H5: null, H6: null, H7: null, H8: null, H9: null, H10: null, H11: null, H12: null,
            H13: null, H14: null, H15: null, H16: null, H17: null, H18: null, H19: null, H20: null, H21: null, H22: null, H23: null, H24: null,
            H25: null, H26: null, H27: null, H28: null, H29: null, H30: null, H31: null, H32: null, H33: null, H34: null, H35: null, H36: null,
            H37: null, H38: null, H39: null, H40: null, H41: null, H42: null, H43: null, H44: null, H45: null, H46: null, H47: null, H48: null,
            Rescfgcodi: null
        },
        colHeaders: ['', 'ITEM', 'NOMBRE', 'FECHA', '00:30', '01:00', '01:30', '02:00', '02:30', '03:00', '03:30', '04:00', '04:30', '05:00', '05:30', '06:00', '06:30', '07:00', '07:30', '08:00', '08:30', '09:00', '09:30', '10:00', '10:30', '11:00', '11:30', '12:00', '12:30', '13:00', '13:30', '14:00', '14:30', '15:00', '15:30', '16:00', '16:30', '17:00', '17:30', '18:00', '18:30', '19:00', '19:30', '20:00', '20:30', '21:00', '21:30', '22:00', '22:30', '23:00', '23:30', '00:00'],
        columns: [
            { data: 'Eliminar', renderer: botonEliminarRenderer, readOnly: true, className: 'htCenter htMiddle' },
            { data: 'Item', type: 'numeric', className: 'htCenter htMiddle', readOnly: true },
            { data: 'Rescfgnombre', type: 'autocomplete', className: 'htCenter', allowInvalid: false, filter: true, strict: true, source: nombres },
            { data: 'Fecha', type: 'date', dateFormat: 'DD/MM/YYYY', correctFormat: true, defaultDate: fecRI, className: ' htCenter', datePickerConfig: dtpConfig },
            { data: 'H1', type: 'numeric', className: 'htCenter' }, { data: 'H2', type: 'numeric', className: 'htCenter' },
            { data: 'H3', type: 'numeric', className: 'htCenter' }, { data: 'H4', type: 'numeric', className: 'htCenter' },
            { data: 'H5', type: 'numeric', className: 'htCenter' }, { data: 'H6', type: 'numeric', className: 'htCenter' },
            { data: 'H7', type: 'numeric', className: 'htCenter' }, { data: 'H8', type: 'numeric', className: 'htCenter' },
            { data: 'H9', type: 'numeric', className: 'htCenter' }, { data: 'H10', type: 'numeric', className: 'htCenter' },
            { data: 'H11', type: 'numeric', className: 'htCenter' }, { data: 'H12', type: 'numeric', className: 'htCenter' },
            { data: 'H13', type: 'numeric', className: 'htCenter' }, { data: 'H14', type: 'numeric', className: 'htCenter' },
            { data: 'H15', type: 'numeric', className: 'htCenter' }, { data: 'H16', type: 'numeric', className: 'htCenter' },
            { data: 'H17', type: 'numeric', className: 'htCenter' }, { data: 'H18', type: 'numeric', className: 'htCenter' },
            { data: 'H19', type: 'numeric', className: 'htCenter' }, { data: 'H20', type: 'numeric', className: 'htCenter' },
            { data: 'H21', type: 'numeric', className: 'htCenter' }, { data: 'H22', type: 'numeric', className: 'htCenter' },
            { data: 'H23', type: 'numeric', className: 'htCenter' }, { data: 'H24', type: 'numeric', className: 'htCenter' },
            { data: 'H25', type: 'numeric', className: 'htCenter' }, { data: 'H26', type: 'numeric', className: 'htCenter' },
            { data: 'H27', type: 'numeric', className: 'htCenter' }, { data: 'H28', type: 'numeric', className: 'htCenter' },
            { data: 'H29', type: 'numeric', className: 'htCenter' }, { data: 'H30', type: 'numeric', className: 'htCenter' },
            { data: 'H31', type: 'numeric', className: 'htCenter' }, { data: 'H32', type: 'numeric', className: 'htCenter' },
            { data: 'H33', type: 'numeric', className: 'htCenter' }, { data: 'H34', type: 'numeric', className: 'htCenter' },
            { data: 'H35', type: 'numeric', className: 'htCenter' }, { data: 'H36', type: 'numeric', className: 'htCenter' },
            { data: 'H37', type: 'numeric', className: 'htCenter' }, { data: 'H38', type: 'numeric', className: 'htCenter' },
            { data: 'H39', type: 'numeric', className: 'htCenter' }, { data: 'H40', type: 'numeric', className: 'htCenter' },
            { data: 'H41', type: 'numeric', className: 'htCenter' }, { data: 'H42', type: 'numeric', className: 'htCenter' },
            { data: 'H43', type: 'numeric', className: 'htCenter' }, { data: 'H44', type: 'numeric', className: 'htCenter' },
            { data: 'H45', type: 'numeric', className: 'htCenter' }, { data: 'H46', type: 'numeric', className: 'htCenter' },
            { data: 'H47', type: 'numeric', className: 'htCenter' }, { data: 'H48', type: 'numeric', className: 'htCenter' },
            { data: 'Rescfgcodi' }
        ],
        data: [],
        hiddenColumns: {
            columns: [52],
            indicators: false,
        },
        height: "430px",
        width: '100%',
        minSpareRows: 0,
        fixedColumnsLeft: 4,
        afterOnCellMouseDown: (event, coords, td) => {
            const colIndex = coords.col;

            // Solo permitir que se abra el datepicker en la columna 2 (Fecha)
            if (colIndex < 3) {
                event.stopImmediatePropagation();
            }
        },
        //Tamaño maximo de columna
        modifyColWidth: function (width, col) {
            if (col == 0) {
                return 30
            }
            if (col == 2) {
                return 360
            }
            if (col == 3) {
                return 150
            }
            if (col > 3) {
                return 60
            }
        },
        afterChange: function (changes, source) {
            var colNombre = 2;
            var colCodigo = 52;
            if (source === 'loadData' || source === 'internal') {
                var allData = this.getSourceData();

                if (allData.length > 0) {
                    var lastRowIndex = this.countRows() - 1;
                    var lastRow = allData[lastRowIndex];

                    // Verificamos campos obligatorios
                    var shouldAddRow = lastRow.Rescfgnombre && lastRow.Fecha;

                    if (shouldAddRow) {
                        // Agregamos nueva fila con alter('insert_row')
                        this.alter('insert_row', lastRowIndex + 1);

                        // Configuramos los valores por defecto
                        this.setDataAtCell(lastRowIndex + 1, colItem, lastRowIndex + 2); // Item

                        // Enfocamos el campo Nombre
                        setTimeout(() => {
                            this.selectCell(lastRowIndex + 1, 1);
                        }, 0);
                    }
                    // Actualizar números después de eliminar fila
                    //actualizarValoresItems(this);
                }
            }

            if (source === 'edit') {

                var allData = this.getSourceData();
                var lastRowIndex = this.countRows() - 1;
                var lastRow = allData[lastRowIndex];

                // Verificamos campos obligatorios
                var shouldAddRow = lastRow.Rescfgnombre && lastRow.Fecha;

                if (shouldAddRow) {
                    // Agregamos nueva fila con alter('insert_row')
                    this.alter('insert_row', lastRowIndex + 1);

                    // Configuramos los valores por defecto
                    this.setDataAtCell(lastRowIndex + 1, colItem, lastRowIndex + 2); // Item

                    // Enfocamos el campo Nombre
                    setTimeout(() => {
                        this.selectCell(lastRowIndex + 1, 1);
                    }, 0);
                }

                const [[row, prop, oldVal, newVal]] = changes;

                if (prop === 'Rescfgnombre') {
                    var valorNombre = this.getDataAtCell(row, colNombre);
                    var elemento = RESTRICCIONES.find(obj => obj.Rescfgnombre === valorNombre);

                    if (elemento != null) {
                        this.setDataAtCell(row, colCodigo, elemento.Rescfgcodi, 'internal');
                    }
                }

            } else if (source === 'remove_row') {
                // Actualizar números después de eliminar fila
                actualizarValoresItems(this);
            }
        }
    });

    precargarDetallesHnd();

    tblHndRestricciones.addHook('beforeChange', function (changes, [source]) {
        listErrores = [];
    });

    tblHndRestricciones.addHook('afterChange', function (changes, [source]) {
        listErrores = [];
        tblHndRestricciones.render();

        if (!changes || source === 'loadData') return;

        changes.forEach(([row, prop, oldVal, newVal]) => {
            if (prop === 'Rescfgnombre') {
                // Buscar en los datos originales el codi correspondiente
                const match = RESTRICCIONES.find(d => d.Rescfgnombre === newVal);
                if (match) {
                    // Actualizar el código si se encontró
                    this.setDataAtRowProp(row, 'Rescfgcodi', match.Rescfgcodi, 'sync');
                } else {
                    // Limpiar si no hay coincidencia
                    this.setDataAtRowProp(row, 'Rescfgcodi', null, 'sync');
                }
            }
        });

    });

}

function botonEliminarRenderer(instance, td, row, col, prop, value, cellProperties) {
    Handsontable.renderers.TextRenderer.apply(this, arguments);

    // Obtener el número total de filas
    const totalFilas = instance.countRows();

    // Solo mostrar el botón si hay más de una fila
    if (totalFilas > 1) {
        // Crear botón HTML
        const botonEliminar = document.createElement('button');
        botonEliminar.textContent = 'x';
        botonEliminar.className = 'btn-eliminar';
        botonEliminar.style.padding = '1px 5px';
        botonEliminar.style.cursor = 'pointer';
        botonEliminar.style.backgroundColor = '#de0202';
        botonEliminar.style.color = 'white';
        botonEliminar.style.border = 'none';
        botonEliminar.style.borderRadius = '3px';
        botonEliminar.style.weight = 'bold';

        // Asignar evento click
        botonEliminar.addEventListener('click', (e) => {
            e.stopPropagation();

            // Eliminar la fila
            instance.alter('remove_row', row);

            // Actualizar números de ITEM
            actualizarValoresItems(instance);
        });

        // Limpiar celda y agregar botón
        td.innerHTML = '';
        td.appendChild(botonEliminar);
    } else {
        // Si solo hay una fila, dejar la celda vacía
        td.innerHTML = '';
    }

    return td;
}

function actualizarValoresItems(hotInstance) {
    const data = hotInstance.getData();
    const cambios = [];

    data.forEach((rowData, index) => {
        cambios.push([index, colItem, index + 1]);
    });

    hotInstance.setDataAtCell(cambios);
}

function datosRestriccionRenderer(instance, td, row, col, prop, value, cellProperties) {
    Handsontable.renderers.TextRenderer.apply(this, arguments);

    var colNombre = 2;
    var colFecha = 3;

    var valItem = tblHndRestricciones.getDataAtRowProp(row, 'Item');
    var numEscenario = parseInt(prop.substring(1));
    var celda = convertirAmediaHora(numEscenario);


    //validaciones campo 48 horas
    if (col > colFecha && col <= 51) {
        if (value != null && value != "") {
            //Valido si es NO numerico    
            if (isNaN(value)) {
                td.className = 'celda_error_roja htCenter';
                agregarError(valItem, celda, value, "Solo se permiten valores numéricos (con punto como separador decimal).");
            } else {
                //Obtengo si el valor real ingresado tiene decimales
                var pE = Math.trunc(value);
                var pD;
                var arrN = (value + "").split(".");
                if (arrN.length > 1) {
                    pD = (value + "").replace((pE + ""), "0");
                }
                var pdecR = pD != undefined ? (pD + "").substring(2) : "";
                var numPDNumReal = pdecR.length;

                //Antes de validar lo convierto
                //var valo1 = parseFloat(td.innerHTML);
                var valo2 = parseFloat(value);
                if (numPDNumReal > 0) {
                    td.innerHTML = valo2;
                }

                //Uso el nuevo valor (max 3 decimales) para la s validaciones
                value = valo2;

                //valido si son positivos pero con mas de 3 cifras enteras o mas de 1 cifra decimal
                var parteEntera = Math.trunc(value);
                var parteDecimal;
                var arrNum = (value + "").split(".");
                if (arrNum.length > 1) {
                    parteDecimal = (value + "").replace((parteEntera + ""), "0");
                }

                if (parteEntera < 0)
                    parteEntera = parteEntera * (-1);

                var pe = parteEntera + "";
                var pd = parteDecimal != undefined ? (parteDecimal + "").substring(2) : "";
                var numPE = pe.length;
                var numPD = pd.length;

                //Solo es aceptado si tiene hasta 3 cifras enteras y 2 cifras decimales
                if (numPE > 4 || numPD > 2) {
                    td.className = 'celda_error_roja htCenter';
                    agregarError(valItem, celda, value, "Solo está permitido el ingreso de valores con máximo 4 cifras enteras y 2 cifras decimales.");
                }
            }
        }
    }
}

function detectarFilasDuplicadasPorNombreYFecha() {

    var data = tblHndRestricciones.getSourceData();

    const mapa = new Map();
    const duplicadas = new Set();

    data.forEach((row, index) => {

        if (row.Rescfgnombre != null) {
            const clave = `${row.Rescfgnombre || ''}-${row.Fecha || ''}`;
            if (mapa.has(clave)) {
                duplicadas.add(index);
                duplicadas.add(mapa.get(clave)); // Marca también la primera ocurrencia
            } else {
                mapa.set(clave, index);
            }
        }
    });

    return Array.from(duplicadas); // Devuelve los índices de las filas duplicadas
}

function datosCombosRestriccionRenderer(instance, td, row, col, prop, value, cellProperties) {

    Handsontable.renderers.AutocompleteRenderer.apply(this, arguments);

    var valItem = tblHndRestricciones.getDataAtRowProp(row, 'Item');

    const duplicadas = detectarFilasDuplicadasPorNombreYFecha();
    if (duplicadas.includes(row)) {
        td.className = 'celda_error_roja htCenter';
        agregarError(valItem, "Registro", "", "Registro duplicado");
    }

    const data = tblHndRestricciones.getData();
    const esUltimaFila = (row === data.length - 1);
    const esPrimeraFila = (row === 0);
    const esSoloUnaFila = (data.length === 1);

    if (esSoloUnaFila) {
        if (value == null || value == "") {
            //td.className = 'celda_error_roja htCenter';
            agregarError(valItem, "Nombre", "", "Campo vacío");
        }
    }
    else {
        if (esUltimaFila) {

            const data = tblHndRestricciones.getSourceData();
            const lastRow = data[data.length - 1];

            // ignoramos la columna item y fecha
            const columnasIgnoradas = [0, 1, 3];
            // Filtrar solo las columnas que quieres verificar
            const valoresFiltrados = Object.values(lastRow).filter((_, index) => !columnasIgnoradas.includes(index));
            // Verificar si alguna de las columnas relevantes tiene datos
            const tieneDatos = valoresFiltrados.some(valor => valor !== null && valor !== undefined && valor !== '');

            if (tieneDatos) {
                if (value == null || value == "") {
                    td.className = 'celda_error_roja htCenter';
                    agregarError(valItem, "Nombre", "", "Campo vacío");
                }
            }
        }
        else {
            if (value == null || value == "") {
                td.className = 'celda_error_roja htCenter';
                agregarError(valItem, "Nombre", "", "Campo vacío");
            }
        }

    }
}

// enderer para fechas
function dateTimeRenderer(instance, td, row, col, prop, value, cellProperties) {
    Handsontable.renderers.TextRenderer.apply(this, arguments);

    var valItem = tblHndRestricciones.getDataAtRowProp(row, 'Item');

    const data = tblHndRestricciones.getData();
    const esUltimaFila = (row === data.length - 1);
    const esPrimeraFila = (row === 0);
    const esSoloUnaFila = (data.length === 1);

    if (esSoloUnaFila) {
        if (value == null || value == "") {
            //td.className = 'celda_error_roja htCenter';
            agregarError(valItem, "Fecha", "", "Campo vacío");
        }
    }
    else {
        if (esUltimaFila) {

            const data = tblHndRestricciones.getSourceData();
            const lastRow = data[data.length - 1];

            // Ignoramos la columa item y nombre
            const columnasIgnoradas = [0, 1, 2];
            // Filtrar solo las columnas que quieres verificar
            const valoresFiltrados = Object.values(lastRow).filter((_, index) => !columnasIgnoradas.includes(index));
            // Verificar si alguna de las columnas relevantes tiene datos
            const tieneDatos = valoresFiltrados.some(valor => valor !== null && valor !== undefined && valor !== '');

            if (tieneDatos) {
                if (value == null || value == "") {
                    td.className = 'celda_error_roja htCenter';
                    agregarError(valItem, "Fecha", "", "Campo vacío");
                }
            }
        }
        else {
            if (value == null || value == "") {
                td.className = 'celda_error_roja htCenter';
                agregarError(valItem, "Fecha", "", "Campo vacío");
            }
        }
    }

    //var valItem = tblHndRestricciones.getDataAtRowProp(row, 'Item');
    if (value != null && value != "") {
        const formatoValido = /^\d{1,2}\/\d{1,2}\/\d{2,4}$/.test(value);
        if (!formatoValido) {
            agregarError(row + 1, prop, value, "Formato de fecha inválido (debe ser dd/mm/yyyy)");
        }
    }

    // Obtener datos de la fila
    const dataFila = instance.getSourceDataAtRow(row);
    const fechaIni = dataFila.Fecha;

    // Validación de rango permitido (02/05/2025 - 29/05/2025)
    if (value && prop === 'Fecha') {
        const [dia, mes, anio] = value.split('/');
        const fechaObj = new Date(anio, mes - 1, dia);
        const fechaMin = moment(fecRI, "DD/MM/YYYY").toDate();
        const fechaMax = moment(fecRF, "DD/MM/YYYY").toDate();

        if (fechaObj < fechaMin || fechaObj > fechaMax) {
            const rangoFechas = fecRI + " - " + fecRF;
            td.className = 'celda_error_roja htCenter';
            agregarError(row + 1, prop, value, `Dato fuera de rango [${rangoFechas}]`);
        }
    }

    return td;
}

function precargarDetallesHnd() {

    if (tblHndRestricciones != null) {
        tblHndRestricciones.updateSettings({
            cells(row, col, prop) {
                const cellProperties = {};
                var colNombre = 2;
                var colFecha = 3;

                if (col == colFecha)
                    cellProperties.renderer = "dateTimeRenderer";

                if (col == colNombre)
                    cellProperties.renderer = "datosCombosRestriccionRenderer";

                if (col > colFecha)
                    cellProperties.renderer = "datosRestriccionRenderer"; // uses lookup map//

                return cellProperties;
            },
        });
    }
}

function mostrarListadoErrores() {
    //limpiarBarraMensaje("mensaje");
    validarHorasPorFila();
    setTimeout(function () {
        $('#tablaErrores').html(dibujarTablaErrores());

        $('#contenedorErrores').bPopup({
            easing: 'easeOutBack',
            speed: 450,
            transition: 'slideDown',
            modalClose: false
        });
        $('#tablaErrorInd').dataTable({
            "scrollY": 330,
            "scrollX": true,
            "sDom": 't',
            "ordering": false,
            "bPaginate": false,
            "iDisplayLength": -1
        });

    }, 200);
}

function validarHorasPorFila() {
    const todasFilas = tblHndRestricciones.getSourceData();
    let data = [];
    if (todasFilas.length === 1) {
    // Si hay una sola fila, devolvemos toda la fila
        data = todasFilas;
    } else {
        // Si hay más de una fila, devolvemos las filas que al menos tenga nombre o fecha ingresados
        //Si no tiene nombre o fecha
        data = todasFilas.filter(fila => {
            const nombreValido = fila.Rescfgnombre && String(fila.Rescfgnombre).trim() !== '';
            const fechaValida = fila.Fecha && String(fila.Fecha).trim() !== '';
            const algunaHora = Array.from({ length: 48 }, (_, i) => 'H' + (i + 1)).some(key =>
                fila[key] !== null && fila[key] !== undefined && String(fila[key]).trim() !== ''
            );

            // Solo retorna si tiene algún campo útil con data
            return nombreValido || fechaValida || algunaHora;
        });
    }

    for (let row = 0; row < data.length; row++) {
        let fila = data[row];
        let filaIncompleta = false;

        for (let i = 1; i <= 48; i++) {
            let key = 'H' + i;
            let valor = fila[key];

            if (valor === null || valor === "" || valor === undefined) {
                filaIncompleta = true;
                break;
            }
        }

        if (filaIncompleta) {
            agregarError(row + 1, "", "", "Se debe ingresar valor numérico para las medias horas");
        }
    }
}

function cargarHansonTablas(datosEnBD) {

    listaDataGeneral = datosEnBD.DatosRestricciones.length > 0 ? datosEnBD.DatosRestricciones : [];
    var datosHnd = datosEnBD.DatosRestricciones.length > 0 ? datosEnBD.DatosRestricciones : [];

    var lstData = [];
    //tblHndRestricciones.loadData(lstData);

    for (var index in datosHnd) {

        var item = datosHnd[index];

        var data = {
            Rescfgcodi: item.Rescfgcodi,
            Rescfgnombre: item.Rescfgnombre,
            Fecha: item.Fecha,
            H1: item.H1, H2: item.H2, H3: item.H3, H4: item.H4, H5: item.H5, H6: item.H6, H7: item.H7, H8: item.H8, H9: item.H9, H10: item.H10, H11: item.H11, H12: item.H12,
            H13: item.H13, H14: item.H14, H15: item.H15, H16: item.H16, H17: item.H17, H18: item.H18, H19: item.H19, H20: item.H20, H21: item.H21, H22: item.H22, H23: item.H23, H24: item.H24,
            H25: item.H25, H26: item.H26, H27: item.H27, H28: item.H28, H29: item.H29, H30: item.H30, H31: item.H31, H32: item.H32, H33: item.H33, H34: item.H34, H35: item.H35, H36: item.H36,
            H37: item.H37, H38: item.H38, H39: item.H39, H40: item.H40, H41: item.H41, H42: item.H42, H43: item.H43, H44: item.H44, H45: item.H45, H46: item.H46, H47: item.H47, H48: item.H48
        };

        lstData.push(data);
    }
    tblHndRestricciones.loadData(lstData);
    actualizarValoresItems(tblHndRestricciones);

    if (datosHnd.length === 0) {
        tblHndRestricciones.alter('insert_row');
        tblHndRestricciones.setDataAtCell(0, colItem, 1);
    }
}

function limpiarBarraMensaje(id) {
    $('#' + id).css("display", "none");
    $('#' + id).removeClass();
    $('#' + id).addClass('action-message');
    $('#' + id).html('');
}

function formatFloat(num, casasDec, sepDecimal, sepMilhar) {

    if (num < 0) {
        num = -num;
        sinal = -1;
    } else
        sinal = 1;
    var resposta = "";
    var part = "";
    if (num != Math.floor(num)) // decimal values present
    {
        part = Math.round((num - Math.floor(num)) * Math.pow(10, casasDec)).toString(); // transforms decimal part into integer (rounded)
        while (part.length < casasDec)
            part = '0' + part;
        if (casasDec > 0) {
            resposta = sepDecimal + part;
            num = Math.floor(num);
        } else
            num = Math.round(num);
    } // end of decimal part
    else {
        while (part.length < casasDec)
            part = '0' + part;
        if (casasDec > 0) {
            resposta = sepDecimal + part;
        }
    }
    while (num > 0) // integer part
    {
        part = (num - Math.floor(num / 1000) * 1000).toString(); // part = three less significant digits
        num = Math.floor(num / 1000);
        if (num > 0)
            while (part.length < 3) // 123.023.123  if sepMilhar = '.'
                part = '0' + part; // 023
        resposta = part + resposta;
        if (num > 0)
            resposta = sepMilhar + resposta;
    }
    if (sinal < 0)
        resposta = '-' + resposta;
    return resposta;
}

function dibujarTablaErrores() {
    var cadena = `
        <div style='clear:both; height:5px'></div>
            <table id='tablaErrorInd' border='1' class='pretty tabla-adicional' cellspacing='0' style='width: 850px;'>
                <thead>
                    <tr>
                        <th style='width: 80px;'>Item</th>
                        <th style='width: 120px;'>Celda</th>
                        <th style='width: 250px;'>Valor</th>
                        <th style='width: 250px;'>Error</th>
                    </tr>
                </thead>
                <tbody>
    `;

    //ordeno el listado
    listErrores.sort((a, b) => String(a.Item).localeCompare(String(b.Item)) || String(a.Celda).localeCompare(String(b.Celda)));
    //listErrores.sort((a, b) => a.Item - b.Item);

    for (var i = 0; i < listErrores.length; i++) {
        var item = listErrores[i];
        cadena += `
                    <tr>
                        <td style='width:  80px; text-align: center; white-space: break-spaces;'>${item.Item}</td>
                        <td style='width: 120px; text-align: left; white-space: break-spaces;'>${item.Celda}</td>
                        <td style='width: 250px; text-align: left; white-space: break-spaces;'>${item.Valor}</td>
                        <td style='width: 250px; text-align: left; white-space: break-spaces;'>${item.Mensaje}</td>
                    </tr>
        `;
    }

    cadena += `
                </tbody>
            </table>
        </div>
    `;

    return cadena;
}

function agregarError(item, celda, valor, mensajeError) {
    //Agrega al array de tipo de informacion
    if (validarError(item, celda, valor, mensajeError)) {
        var regError = {
            Item: item,
            Celda: celda,
            Valor: valor,
            Mensaje: mensajeError
        };

        listErrores.push(regError);
    }
}

function validarError(item, celda, valor, mensajeError) {
    var arrayData = [];
    arrayData = listErrores.slice();

    for (var j in arrayData) {
        if (arrayData[j]['Item'] == item && arrayData[j]['Celda'] == celda && arrayData[j]['Valor'] == valor && arrayData[j]['Mensaje'] == mensajeError) {
            return false;
        }
    }
    return true;
}

function enviarDatos() {
    $('#hfHorizonte').val($('#cbHorizonte').val());
    $('#hfFecha').val($('#txtFecha').val());
    $('#hfSemana').val($('#cbSemana').val());
    $('#hfAnho').val($('#Anho').val());
    semana = $("#hfSemana").val();
    anho = $("#hfAnho").val();
    semana = anho.toString() + semana;
    idHorizonte = $("#hfHorizonte").val();


    limpiarBarraMensaje("mensaje");
    validarHorasPorFila();
    if (listErrores.length == 0) {
        var dataGuardar = obtenerDataGuardar();
        var dataJson = {
            formato: idHorizonte,
            fecha: $('#hfFecha').val(),
            semana: semana,
            datosAGuardar: dataGuardar
        };

        if (confirm("¿Desea enviar información a COES?")) {
            $.ajax({
                url: controlador + "GuardarRestricciones",
                type: 'POST',
                contentType: 'application/json; charset=UTF-8',
                dataType: 'json',
                data: JSON.stringify(dataJson),
                success: function (evt) {
                    if (evt.Resultado == "1") {
                        alert("Los datos se enviaron correctamente");
                        //mostrarMensaje('mensaje', 'exito', 'La información fue registrada correctamente.');
                        cargarRestricciones();
                    } else {
                        mostrarMensaje('mensaje', 'error', evt.Mensaje);
                    }
                },
                error: function () {
                    mostrarMensaje('mensaje', 'error', "No se ha podido realizar la acción");
                }
            });
        }
    }
    else
        mostrarListadoErrores();
}

function obtenerDataGuardar(dataHandson) {

    var dataHandsonRestrict = [];

    //dataHandsonMod = tblHndRestricciones.getSourceData();
    dataHandsonRestrict = tblHndRestricciones.getSourceData().filter(fila => {
        // Verificamos que los primeros 3 campos estén completos
        const campos = [fila.Item, fila.Rescfgnombre, fila.Fecha];

        return campos.every(val => val !== null && val !== undefined && String(val).trim() !== '');
    });


    var dataGuardar = {
        DatosRestricciones: dataHandsonRestrict
    };

    return dataGuardar;
}
function mostrarMensaje(id, tipo, mensaje) {
    $("#" + id).css("display", "block");
    $('#' + id).removeClass();
    $('#' + id).addClass('action-' + tipo);
    $('#' + id).html(mensaje);

}

function obtenerRangoFechasPorSemana() {
    var numSemana = parseInt($("#cbSemana").val()) || 0;
    var anio = parseInt($("#Anho").val()) || 0;

    $.ajax({
        type: 'POST',
        url: controlador + "ObtenerRangoFechasSemana",
        data: {
            numSemana: numSemana,
            anio: anio
        },
        success: function (evt) {

            if (evt.Resultado != "-1") {
                fecRI = evt.FechaIniSem;
                fecRF = evt.FechaFinSem;

                if (tblHndRestricciones) {
                    tblHndRestricciones.render();
                }
            } else {
                alert("Ha ocurrido un error: " + evt.Mensaje);
            }

        },
        error: function (xhr, status) {
            mostrarMensaje('mensaje', 'error', 'Se ha producido un error.');
        }
    });
}

function obtenerRangoFechasPorSemanaCopia() {
    var numSemana = parseInt($("#cbSemanaCopia").val()) || 0;
    var anio = parseInt($("#AnhoCopia").val()) || 0;

    $.ajax({
        type: 'POST',
        url: controlador + "ObtenerRangoFechasSemana",
        data: {
            numSemana: numSemana,
            anio: anio
        },
        success: function (evt) {

            if (evt.Resultado != "-1") {
                fecRICopia = evt.FechaIniSem;
                fecRFCopia = evt.FechaFinSem;

                $('#txtFechaOrigenIni').val(fecRICopia);
                $('#txtFechaOrigenFin').val(fecRFCopia);

                $('#txtFechaOrigenIni').unbind();
                $('#txtFechaOrigenIni').Zebra_DatePicker({
                    readonly_element: false,
                    direction: [fecRICopia, fecRFCopia],
                });
                $('#txtFechaOrigenFin').unbind();
                $('#txtFechaOrigenFin').Zebra_DatePicker({
                    readonly_element: false,
                    direction: [fecRICopia, fecRFCopia],
                });

            } else {
                alert("Ha ocurrido un error: " + evt.Mensaje);
            }

        },
        error: function (xhr, status) {
            mostrarMensaje('mensaje', 'error', 'Se ha producido un error.');
        }
    });
}


function horizonte() {

    var opcion = $('#cbHorizonte').val();
    switch (parseInt(opcion)) {
        case 1: //dia
        case 2: //dia
            $('#dDia').css("display", "block");
            $('#dSemana').css("display", "none");
            fecRI = $("#txtFecha").val();
            fecRF = $("#txtFecha").val();

            if (tblHndRestricciones) {
                tblHndRestricciones.render();
            }

            break;
        case 3: //semanal
            $('#dDia').css("display", "none");
            $('#dSemana').css("display", "block");
            obtenerRangoFechasPorSemana();
            break;

    }
}

function cargarSemanaAnho() {
    var anho = $('#Anho').val();
    $('#hfAnho').val(anho);
    $.ajax({
        type: 'POST',
        url: controlador + 'CargarSemanas',

        data: { idAnho: $('#hfAnho').val() },

        success: function (aData) {
            $('#SemanaIni').html(aData);
        },
        error: function () {
            alert("Ha ocurrido un error");
        }
    });
}

function horizonteCopia() {

    var idHorizonte = $("#cbHorizonte").val();
    var opcion = $('#cbHorizonteCopia').val();
    switch (parseInt(opcion)) {
        case 1: //dia
        case 2: //dia
            $('#dDiaCopia').css("display", "block");
            $('#dSemanaCopia').css("display", "none");
            break;
        case 3: //semanal
            $('#dDiaCopia').css("display", "none");
            $('#dSemanaCopia').css("display", "block");
            obtenerRangoFechasPorSemanaCopia();

            if (idHorizonte == 3)
                $('#tdFechaDestinoFin').css("display", "none");
            else
                $('#tdFechaDestinoFin').css("display", "block");

            break;

    }
}
function cargarSemanaAnhoCopia() {
    var anho = $('#AnhoCopia').val();
    $('#hfAnhoCopia').val(anho);
    $.ajax({
        type: 'POST',
        url: controlador + 'CargarSemanasPopup',

        data: { idAnho: $('#hfAnhoCopia').val() },

        success: function (aData) {
            $('#SemanaIniCopia').html(aData);
        },
        error: function () {
            alert("Ha ocurrido un error");
        }
    });

}

function convertirAmediaHora(valor) {
    // Calcular las horas y los minutos
    var horas = (valor % 2 === 1) ? Math.floor((valor - 1) / 2) : valor / 2; // Restamos 1 porque empieza en 00:30
    horas = (horas == 24) ? 0 : horas;
    const minutos = (valor % 2 === 1) ? 30 : 0; // Si es impar, serán 30 minutos

    // Formatear las horas y los minutos con 2 dígitos
    const horaFormateada = String(horas).padStart(2, '0');
    const minutosFormateados = String(minutos).padStart(2, '0');

    return `${horaFormateada}:${minutosFormateados}`;
}

//EXPORTACIÓN
function reporteRestriccion() {

    $.ajax({
        type: 'POST',
        url: controlador + 'ExportarRestriccion',
        dataType: 'json',
        data: {
        },
        success: function (evt) {
            if (evt.Resultado != "-1") {
                window.location = controlador + "AbrirArchivo?file=" + evt.NombreArchivo;
            }
            else {
                alert(evt.StrMensaje);
            }
        },
        error: function (err) {
            alert('Ha ocurrido un error de exportación');
        }
    });
}