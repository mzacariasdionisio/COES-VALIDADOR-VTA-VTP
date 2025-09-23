var controlador = siteRoot + 'Yupana/DisponibilidadCombustible/';
var tblError;
var tblErrorImportacion;

var TIENE_PERMISO_NUEVO = false;
var TIENE_PERMISO_EDITAR_ADMIN = false;


var containerHandsonDC;
var tblHdnDispComb;
var filaMesSeleccionado = -1;
var fecRI = "";
var fecRF = "";
var fecRICopia = "";
var fecRFCopia = "";
var listaEnvios = [];
var caso = 0;
var agregarFechaDefault = false;
var mostrandoDesdeHistorialEnvios = false;

const colIdRs = 0;
const colItem = 1;
const colNomb = 2;
const colDisp = 3;
const colFIni = 4;
const colHIni = 5;
const colFFin = 6;
const colHFin = 7;
const colCons = 8;

/*const colConsidera = 8;*/
const colorError = '#fe4c42'; //MISMO COLOR QUE EL POR DEFECTO

const listaRestriccionesTotales = [];

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

            //Para validar nuevamente las celdas
            renderizarHandsonDC();
        }

    });

    $('#cbHorizonte').change(function () {
        mostrarVisibilidadHorizonte();

    });


    $('#btnConsultar').click(function () {
        cargarRestricciones();
    });

    $('#btnConfigurar').on('click', function () {
        document.location.href = siteRoot + 'Yupana/DisponibilidadCombustible/ConfiguracionIndex';
    });

    $('#btnEnviarDatos').click(function () {
        enviarDatos();
    });

    $('#btnMostrarErrores').click(function () {

        mostrarListadoErrores(true);
    });

    $('#btnCopiar').click(function () {
        copiarDatos();
    });

    $('#btnDescargar').click(function () {
        reporteRestriccion();
    });

    mostrarVisibilidadHorizonte();
    cargarSemanaAnho();
    cargarRestricciones();


    tblError = $('#tblErrores').DataTable({
        "lengthChange": false,  // Desactiva el control de cambio de tamaño de filas
        "columns": [
            { "data": "Item" },
            { "data": "Columna" },
            { "data": "Descripcion" }
        ]
    });

    tblErrorImportacion = $('#tblErroresImportacion').DataTable({
        "lengthChange": false,  // Desactiva el control de cambio de tamaño de filas
        "columns": [
            { "data": "NumFila" },
            { "data": "Campo" },
            { "data": "ErrorDescripcion" }
        ]
    });

    $('#btnVerEnvios').click(function () {
        mostrarVentanaListaEnvios();
    });

    //Explorar archivos
    importarFormato();
});


function cargarRestricciones() {
    mostrandoDesdeHistorialEnvios = false;
    var idHorizonte = $("#cbHorizonte").val();
    var idEnvio = 0;
    var fecha = $("#txtFecha").val();
    var semana = $("#cbSemana").val();
    var anho = $("#Anho").val();
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
                agregarFechaDefault = idHorizonte == "1" || idHorizonte == "2";
                listaEnvios = evt.ListaEnvios;
                var dataG = evt.DataRestricciones;

                inicioDetalles(dataG.ListadoTotalRestricciones, mostrandoDesdeHistorialEnvios);

                cargarHansonTablas(dataG, false, false);

                var numRestricciones = dataG.LstRestricciones.length;
                if (numRestricciones == 0) {
                    mostrarMensaje('mensaje', 'alert', 'No se encontró información para el filtro seleccionado.');
                }

                if (evt.IdEnvio > 0) {
                    var mensaje = "<strong>Código de envío</strong> : " + evt.IdEnvio + ", <strong>Fecha de envío: </strong>" + evt.FechaProceso;
                    mostrarMensaje('mensaje', 'exito', mensaje);
                }


            } else {
                mostrarMensaje('mensaje', 'error', evt.Mensaje);
            }


        },
        error: function (xhr, status) {
            mostrarMensaje('mensaje', 'error', 'Se ha producido un error.');
        }
    });

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

    var existeErrores = mostrarListadoErrores(false);

    if (!existeErrores) {
        var dataGuardar = obtenerDataGuardar(false);
        var dataJson = {
            horizonte: idHorizonte,
            fecha: $('#hfFecha').val(),
            semana: semana,
            datosAGuardar: dataGuardar
        };

        if (confirm("¿Desea enviar información al COES?")) {
            $.ajax({
                url: controlador + "guardarDatosDispCombustible",
                type: 'POST',
                contentType: 'application/json; charset=UTF-8',
                dataType: 'json',
                data: JSON.stringify(dataJson),
                success: function (evt) {
                    if (evt.Resultado == "1") {


                        cargarRestricciones();

                        setTimeout(() => {
                            mostrarMensaje('mensaje', 'exito', 'La información fue registrada correctamente.');
                        }, 1000); // 1000 milisegundos = 1 segundo


                    } else {
                        mostrarMensaje('mensaje', 'error', evt.Mensaje);
                    }
                },
                error: function () {
                    mostrarMensaje('mensaje', 'error', "No se ha podido guardar la información.");
                }
            });
        }
    }
}

///Obtiene data del handson, muestra todo para el caso de COPIAR pero filtra solo filas completas para GUARDAR
function obtenerDataGuardar(mostrarTodos) {

    // Obtener datos VISIBLES (ignora celdas null internas, osea ignorar el getsourcedata)
    var dataHandsonDC = tblHdnDispComb.getData();
    var datosFiltrados;

    if (mostrarTodos) { //considero a las fillas q les falten algunos fdatos, Para Copiar
        datosFiltrados = dataHandsonDC;
    } else { //Para guardar a BD
        // Filtrar filas vacías (considerando todos los campos requeridos), solo muestra filas con data completas
        datosFiltrados = dataHandsonDC.filter(function (fila) {
            
            return (
                // Verifica que cada campo no sea null/undefined y que, si es string, no esté vacío o con solo espacios
                (fila[colNomb] != null && String(fila[colNomb]).trim() !== "") &&
                (fila[colDisp] != null && String(fila[colDisp]).trim() !== "") &&
                (fila[colFIni] != null && String(fila[colFIni]).trim() !== "") &&
                (fila[colHIni] != null && String(fila[colHIni]).trim() !== "") &&
                (fila[colFFin] != null && String(fila[colFFin]).trim() !== "") &&
                (fila[colHFin] != null && String(fila[colHFin]).trim() !== "") 
            );
        });
    }
    // Formatear para el servidor (eliminar propiedades innecesarias)
    var datosParaGuardar = datosFiltrados.map(function (fila) {

        return {
            IdRestriccion: fila[colIdRs],
            Item: fila[colItem],
            Nombre: fila[colNomb],
            Disponibilidad: fila[colDisp],
            FechaInicialDesc: fila[colFIni],
            HoraInicial: fila[colHIni],
            FechaFinalDesc: fila[colFFin],
            HoraFinal: fila[colHFin],
            Considerar: fila[colCons]
        };

    });

    return { RegistrosHnd: datosParaGuardar };
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

                if (tblHdnDispComb) {
                    tblHdnDispComb.render();
                }
            } else {

                mostrarMensaje('mensaje', 'error', 'Error: ' + evt.Mensaje);
            }

        },
        error: function (xhr, status) {
            mostrarMensaje('mensaje', 'error', 'Se ha producido un error.');
        }
    });
}

function cargarSemanaAnho() {

    var anho = $('#Anho').val();
    $('#hfAnho').val(anho);
    $.ajax({
        type: 'POST',
        url: controlador + 'CargarSemanas',
        data: {
            idAnho: $('#hfAnho').val()
        },

        success: function (aData) {
            $('#SemanaIni').html(aData);


        },
        error: function () {
            alert("Ha ocurrido un error");
        }
    });

}

function otrfuncion() {
    var anho = $('#Anho').val();
    $('#hfAnho').val(anho);

    $.ajax({
        type: 'POST',
        url: controlador + "CargarSedfgdfmanas",
        data: {
            idAnho: $('#hfAnho').val()
        },
        success: function (evt) {

            if (evt.Resultado != "-1") {
                var listaSems = evt.ListaSemana;

                //Listamos identificadores
                if (listaSems.length > 0) {
                    //usando for
                    $('#cbSemana').get(0).options[0] = new Option("--  Seleccione Identificador  --", "0"); //obliga seleccionar para buscar tag
                    for (var i = 0; i < listaSems.length; i++) {
                        $('#cbSemana').append('<option value=' + listaSems[i].IdTipoInfo + '>' + listaSems[i].NombreTipoInfo + '</option>');  // listaSems es List<string>, si es objeto usar asi listaSems[i].Equicodi
                    }

                } else {
                    $('#cbSemana').get(0).options[0] = new Option("--  Seleccione Identificador  --", "0");
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
function mostrarVisibilidadHorizonte() {

    var opcion = $('#cbHorizonte').val();
    switch (parseInt(opcion)) {
        case 1: //diario
        case 2: //reprograma
            $('#dDia').css("display", "block");
            $('#dSemana').css("display", "none");
            fecRI = $("#txtFecha").val();
            fecRF = $("#txtFecha").val();

            //Para validar nuevamente las celdas
            renderizarHandsonDC();
            break;
        case 3: //semanal
            $('#dDia').css("display", "none");
            $('#dSemana').css("display", "block");
            obtenerRangoFechasPorSemana();
            break;

    }

}

function renderizarHandsonDC() {
    if (tblHdnDispComb) {
        tblHdnDispComb.render();
    }
}

function inicioDetalles(listaRestr, esSoloLectura) {
    limpiarBarraMensaje('mensaje');

    // IMPORTANTE Destruir la instancia existente de Handsontable si existe
    if (tblHdnDispComb) {
        tblHdnDispComb.destroy();
        containerHandsonDC.innerHTML = ''; // Limpiar el contenedor
    }

    //Llena el listado total de restricciones
    cargarListadoRestricciones(listaRestr);



    // Configuración del datepicker con intervalos de 30 minutos
    var dtpConfiguracion = {
        firstDay: 1,
        showWeekNumber: false,
        //showWeekNumber: true,
        showTimepicker: false,

        i18n: {
            previousMonth: 'Mes anterior',
            nextMonth: 'Mes siguiente',
            months: ['Enero', 'Febrero', 'Marzo', 'Abril', 'Mayo', 'Junio', 'Julio', 'Agosto', 'Septiembre', 'Octubre', 'Noviembre', 'Diciembre'],
            weekdays: ['Domingo', 'Lunes', 'Martes', 'Miércoles', 'Jueves', 'Viernes', 'Sábado'],
            weekdaysShort: ['Dom', 'Lun', 'Mar', 'Mié', 'Jue', 'Vie', 'Sáb']
        }
        ,
        disableDayFn(date) {

            const fechaMin = moment(fecRI, "DD/MM/YYYY").toDate();
            const fechaMax = moment(fecRF, "DD/MM/YYYY").toDate();

            return date < fechaMin || date > fechaMax;
        }

    };

    // #region Handsontable 
    containerHandsonDC = document.getElementById('seccionTabla');


    tblHdnDispComb = new Handsontable(containerHandsonDC, {
        dataSchema: {
            IdRestriccion: null,
            Item: null,
            Nombre: null,
            Disponibilidad: null,
            FechaInicialDesc: null,
            HoraInicial: null,
            FechaFinalDesc: null,
            HoraFinal: null,
            Considerar: true // Por defecto marcado
        },
        colHeaders: ['', 'Item', 'Nombre', 'Disponibilidad', 'Fecha Inicial', 'Hora Inicial', 'Fecha Final', 'Hora Final', '¿Considerar?', ''],
        columns: [
            {
                data: 'IdRestriccion',
                type: 'numeric',
                readOnly: true,
                //className: 'htCenter htMiddle', // alineación horizontal y vertical
                //renderer: textoCentradoRenderer // Para mayor ajuste personalizado
            },
            {
                data: 'Item',
                type: 'numeric',
                readOnly: true,
                className: 'htCenter htMiddle', // alineación horizontal y vertical
                renderer: textoCentradoRenderer // Para mayor ajuste personalizado
            },

            {
                data: 'Nombre',
                type: 'autocomplete',
                readOnly: esSoloLectura,
                source: listaRestriccionesTotales.map(p => p.nombre),
                strict: true,  // Solo permite valores de la lista
                filter: true,
                allowInvalid: false,  // No permite valores fuera de la lista
                className: 'htCenter',
                //renderer: nombreDuplicadoRenderer  // para duplic ados en nombre
            },
            {
                data: 'Disponibilidad',
                type: 'text',
                readOnly: esSoloLectura,
                validator: function (value, callback) {
                    // Validación estricta de decimales
                    if (/^\d{1,8}(\.\d{1,4})?$/.test(value)) {
                        callback(true);
                    } else {
                        callback(false);
                    }
                },
                className: 'htRight',
                //renderer: disponibilidadRenderer  // en pause
            }
            ,

            {
                data: 'FechaInicialDesc',
                type: 'date',
                readOnly: esSoloLectura,
                dateFormat: 'DD/MM/YYYY',
                correctFormat: true,
                datePickerConfig: dtpConfiguracion,
                renderer: rendererUnificadoFechaHora,
                //renderer: dateTimeRenderer,
                className: 'htCenter',
                validator: function (value, callback) {
                    // Validar si es vacio
                    if (value != "") {
                        callback(true); // Válido
                    } else {
                        callback(false); // Inválido
                    }
                }
            }
            ,
            {
                data: 'HoraInicial',
                type: 'dropdown',
                readOnly: esSoloLectura,
                source: horariosDisponiblesIni,
                strict: true,
                allowInvalid: false,  // No permite valores fuera de la lista
                className: 'htCenter',
                //renderer: horaRenderer, // Nuevo renderer específico para horas
                renderer: rendererUnificadoFechaHora,
                validator: function (value, callback) {
                    // Validar que el valor esté en la lista de horarios permitidos
                    if (horariosDisponiblesIni.includes(value)) {
                        callback(true); // Válido
                    } else {
                        callback(false); // Inválido
                    }
                }
            },
            {
                data: 'FechaFinalDesc',
                type: 'date',
                readOnly: esSoloLectura,
                dateFormat: 'DD/MM/YYYY',
                correctFormat: true,
                datePickerConfig: dtpConfiguracion,
                renderer: rendererUnificadoFechaHora,
                className: 'htCenter',
                validator: function (value, callback) {
                    // Validar si es vacio
                    if (value != "") {
                        callback(true); // Válido
                    } else {
                        callback(false); // Inválido
                    }
                }
            },
            {
                data: 'HoraFinal',
                type: 'dropdown',
                readOnly: esSoloLectura,
                source: horariosDisponiblesFin,
                strict: true,
                allowInvalid: false,  // No permite valores fuera de la lista
                className: 'htCenter',
                renderer: rendererUnificadoFechaHora,
                validator: function (value, callback) {
                    // Validar que el valor esté en la lista de horarios permitidos
                    if (horariosDisponiblesFin.includes(value)) {
                        callback(true); // Válido
                    } else {
                        callback(false); // Inválido
                    }
                }
            },
            {
                data: 'Considerar',
                type: 'checkbox',
                readOnly: esSoloLectura,
                strict: true, // Solo permite true/false
                className: 'htCenter htMiddle', // para alineación  horizonntal y vertical
                renderer: checkboxCentradoRenderer, // Para amyor ajuste
                // Asegurar que solo acepte booleanos
                validator: function (value, callback) {
                    if (typeof value === 'string') {
                        const lowerValue = value.toLowerCase();
                        callback(lowerValue === 'true' || lowerValue === 'false');
                    } else {
                        callback(value === true || value === false);
                    }

                }
            },
            // Agrega esta nueva columna para el botón de eliminar
            {
                data: 'Eliminar',
                //renderer: botonEliminarRenderer,
                renderer: esSoloLectura ? function () {
                    // Renderer vacío para modo solo lectura
                    return function (instance, td) {
                        Handsontable.renderers.TextRenderer.apply(this, arguments);
                        td.innerHTML = '';
                        return td;
                    };
                } : botonEliminarRenderer,
                readOnly: true,
                className: 'htCenter htMiddle'
            }
        ],

        //colWidths: [80,250, 120, 170, 170, 80],
        colWidths: [0, 80, 250, 120, 120, 100, 120, 100, 80, 30],
        columnSorting: false, // no puede ordenarse las columnas        
        minSpareRows: 0,
        showWeekNumber: false,
        //rowHeaders: false, //quita numeración
        autoWrapRow: true,
        startRows: 1,
        width: 1020, //ancho de la tabla
        height: 350, //alto de la tabla
        manualColumnResize: true,
        fillHandle: false,          // Evita arrastrar el cuadro de relleno    
        hiddenColumns: {
            columns: [0],  // Índice de la columna a ocultar
            indicators: false  // (Opcional) Muestra un indicador de columna oculta
        },

        //Eventos
        beforeKeyDown: function (e) {
            // Solo para la columna Disponibilidad (índice 3)
            var selected = this.getSelected();
            if (selected && selected[1] === colDisp) {
                var key = e.keyCode || e.which;

                // Permitir: Números (0-9) Y Punto decimal (códigos 46, 110 y 190 para compatibilidad)
                // Bloquear: backspace(8), tab(9), enter(13), delete(46), left(37), right(39)
                var teclasPermitidas = [8, 9, 13, 37, 39, 46, 110, 190];
                var esNumerico = (key >= 48 && key <= 57) || (key >= 96 && key <= 105); // Teclado normal y numérico
                var esPuntoDecimal = (key === 110 || key === 190 || key === 46); // Punto en teclado numérico(110) y normal(190/46)
                var esTeclaPermitida = teclasPermitidas.includes(key);

                // Permite Ctrl+V / Cmd+V   Ctrl (17), V (86), C (67)                
                if ((e.ctrlKey || e.metaKey) && [67, 86, 88].includes(key)) {
                    return; // No bloquees el pegado
                }

                if (!esNumerico && !esPuntoDecimal && !esTeclaPermitida && !(e.ctrlKey || e.metaKey)) {              
                    e.stopImmediatePropagation();
                    e.preventDefault();
                }

                // Si es punto decimal, verificar que no exista ya uno
                if (esPuntoDecimal) {
                    var currentValue = this.getActiveEditor().TEXTAREA.value;
                    if (currentValue.indexOf('.') !== -1) {
                        e.stopImmediatePropagation();
                        e.preventDefault();
                    }
                }
            }
        },



        afterChange: function (changes, source) {
            var esPegado = source === 'paste';

            if ((source === 'paste' || source === 'edit') && changes) {

                /******** Agrego una fila al final del listado ********/
                var dataHandson = this.getSourceData();
                var indiceUltimaFila = this.countRows() - 1;
                var ultimaFila = dataHandson[indiceUltimaFila];

                // Verificamos campos obligatorios
                var deboAgregarFila = ultimaFila.Nombre && ultimaFila.FechaInicialDesc && ultimaFila.FechaFinalDesc;

                if (deboAgregarFila) {
                    // Agregamos nueva fila con alter('insert_row')
                    this.alter('insert_row', indiceUltimaFila + 1);

                    // Configuramos los valores por defecto
                    this.setDataAtCell(indiceUltimaFila + 1, colItem, indiceUltimaFila + 2); // Actualiza Item (cuando no es pegado)
                    this.setDataAtCell(indiceUltimaFila + 1, colCons, true); // Considerar

                    // Enfocamos el campo Nombre
                    setTimeout(() => {
                        this.selectCell(indiceUltimaFila + 1, 1);
                    }, 0);

                    // Actualizamos las celdas de fecha y hora en la tabla, solo para diario y reprograma
                    if (tblHdnDispComb != null && tblHdnDispComb != undefined && agregarFechaDefault && !mostrandoDesdeHistorialEnvios) {
                        var fechaFiltro = $("#txtFecha").val(); // Obtenemos la fecha del filtro 
                        var data = tblHdnDispComb.getData();
                        data.forEach((row, rowIndex) => {

                            tblHdnDispComb.setDataAtCell(indiceUltimaFila + 1, colFIni, fechaFiltro);

                            tblHdnDispComb.setDataAtCell(indiceUltimaFila + 1, colFFin, fechaFiltro);
                        });
                    }
                }

                if (esPegado) {

                    // Actualizar números de Item cuando es pegado
                    actualizarValoresItems(this);
                }

            }
            else {
                //No hay cambio de hanson pero presiono ELIMINAR
                if (source === 'remove_row') {
                    // Actualizar números después de eliminar fila
                    actualizarValoresItems(this);
                }
            }
        },

        beforeChange: function (changes, source) {

            changes.forEach((change) => {
                const [row, prop, oldValue, newValue] = change;

                //si no tiene formato de fecha pone vacio
                if ((prop === 'FechaInicialDesc' || prop === 'FechaFinalDesc') && newValue) {

                    if (!/^\d{2}\/\d{2}\/\d{4}$/.test(newValue)) {
                        //return false; // Cancela el cambio si la fecha no es válida
                        change[3] = ""; // Sobrescribe cualquier valor nuevo con `false`
                    }
                }

                // Forzar que "Considerar" siempre sea false si no tiene valor correcto
                if (prop === 'Considerar') {
                    var valColocar = change[3];

                    if (valColocar == 'true')
                        valColocar = true;
                    if (valColocar == 'false')
                        valColocar = false;

                    if (valColocar !== true && valColocar !== false) {
                        change[3] = false; // Sobrescribe cualquier valor nuevo con `false`
                    }

                }

                /*************** Al seleccionar NOMBRE seteo  su id ***************/
                // Si el cambio es en la columna "Nombre" (prop === 'Nombre')
                if (prop === 'Nombre') {
                    const restriccion = listaRestriccionesTotales.find(r => r.nombre === newValue);
                    if (restriccion) {
                        this.setDataAtCell(row, 0, restriccion.id); // Actualiza IdRestriccion (columna 0)
                        console.log("ID asignado:", restriccion.id); // Para depuración
                    }
                }
            });
        },        

    });


    // Agregar primera fila con Item=1 y Considerar=true cuando no hay data
    //tblHdnDispComb.setDataAtCell(0, colItem, 1);
    //tblHdnDispComb.setDataAtCell(0, colCons, true); 


    tblHdnDispComb.addHook('afterChange', function (TD, row, column, prop, value, cellProperties) {

        //cada cambio de fecha en la columna Fecha debe actualizar su Semana
        if (TD != null) {
            var datos = TD[0];
            var columna = datos[1];
            var fechaAntigua = datos[2];
            var fechaNueva = datos[3];
            if (columna === "FechaIniDesc" && fechaAntigua != fechaNueva) {

                //var diaIniAnioSeleccionado = $('#txtFechaIniAnio').val();
                //var anioSeleccionado = $('#txtAnio').val();
                //actualizarListaSemanaMes(anioSeleccionado, diaIniAnioSeleccionado, diaIniAnioSeleccionado);


            }
        }
    });

}

function cargarHansonTablas(dataGeneral, esInfoCopiado, esInfoImportado) {

    var lstRegistrosBD = dataGeneral.LstRestricciones;

    var datosHnd = lstRegistrosBD.length > 0 ? lstRegistrosBD : [];

    //Formateo listado
    var lstData = [];
    var cont = 1;

    for (var index in datosHnd) {

        var reg = datosHnd[index];

        var data = {
            IdRestriccion: reg.Rescfgcodi,
            Item: cont,
            Nombre: reg.Rescfgnombre,
            Disponibilidad: parseFloat(reg.Resdatdato),
            FechaInicialDesc: reg.ResdatfechainiDesc,
            HoraInicial: reg.Resdathoraini,
            FechaFinalDesc: reg.ResdatfechafinDesc,
            HoraFinal: reg.Resdathorafin,
            Considerar: reg.Resdatflag == "S" ? true : false
        };

        lstData.push(data);
        cont++;
    }

    //adiciono a lo existente    
    if (esInfoCopiado) {
        
        var dataActual = obtenerDataGuardar(true);
        var lstActualHnd = dataActual.RegistrosHnd;

        //Para diario y preprograma limpio las horas para la ultima fila
        var numRegistros = lstActualHnd.length;
        if (agregarFechaDefault) {
            var regFinal = lstActualHnd[numRegistros - 1];
            var sdf = 0;
            lstActualHnd[numRegistros - 1].HoraInicial = null;
            lstActualHnd[numRegistros - 1].HoraFinal = null;
        }

        lstData = lstData.concat(lstActualHnd);
    }

    //Lleno listado
    tblHdnDispComb.loadData(lstData);
    actualizarValoresItems(tblHdnDispComb);

    //Agrego fila adicional
    if (datosHnd.length === 0) {
        tblHdnDispComb.alter('insert_row');
        tblHdnDispComb.setDataAtCell(0, colItem, 1);
    }

    // Actualizamos las celdas de fecha y hora en la tabla, solo para diario y reprograma, no se aplica al copiar o importar
    if (tblHdnDispComb != null && tblHdnDispComb != undefined && agregarFechaDefault && listaEnvios.length == 0 && !esInfoCopiado && !esInfoImportado) {
        var fechaFiltro = $("#txtFecha").val(); // Obtenemos la fecha del filtro 
        var data = tblHdnDispComb.getData();
        data.forEach((row, rowIndex) => {

            tblHdnDispComb.setDataAtCell(rowIndex, colFIni, fechaFiltro);
            tblHdnDispComb.setDataAtCell(rowIndex, colHIni, "00:30");

            tblHdnDispComb.setDataAtCell(rowIndex, colFFin, fechaFiltro);
            tblHdnDispComb.setDataAtCell(rowIndex, colHFin, "24:00");
        });
    }    
}

/**************************************************/
/************ RENDERIZADO DE COLUMNAS  ************/
function horaRenderer(instance, td, row, col, prop, value, cellProperties) {
    //Handsontable.renderers.TextRenderer.apply(this, arguments);

    // Primero llamamos al renderer de autocompletado para que dibuje la flecha
    Handsontable.renderers.AutocompleteRenderer.apply(this, arguments);

    // Obtener los datos de toda la fila
    const dataFila = instance.getSourceDataAtRow(row);
    const fechaIni = dataFila.FechaInicialDesc;
    const horaIni = dataFila.HoraInicial;
    const fechaFin = dataFila.FechaFinalDesc;
    const horaFin = dataFila.HoraFinal;

    // Solo validar si ambas fechas y horas están completas
    if (fechaIni && horaIni && fechaFin && horaFin) {
        try {
            // Convertir fechas y horas a objetos Date para comparación
            const [diaIni, mesIni, anioIni] = fechaIni.split('/');
            const [horaIniH, minIni] = horaIni.split(':');

            const [diaFin, mesFin, anioFin] = fechaFin.split('/');
            const [horaFinH, minFin] = horaFin.split(':');

            const fechaHoraIni = new Date(anioIni, mesIni - 1, diaIni, horaIniH, minIni);
            const fechaHoraFin = new Date(anioFin, mesFin - 1, diaFin, horaFinH, minFin);

            // Si las fechas son iguales, comparar horas
            if (fechaIni === fechaFin && fechaHoraIni >= fechaHoraFin) {
                td.style.backgroundColor = colorError;
            }
        } catch (error) {
            console.error("Error al comparar horas:", error);
        }
    }

    //3: si tiene valor y no esta en el listado permitido
    if (col == colHIni) {
        if (!horariosDisponiblesIni.includes(value) && value != "" && value != null && value != undefined) {
            td.style.backgroundColor = colorError;
        }
    }
    if (col == colHFin) {
        if (!horariosDisponiblesFin.includes(value) && value != "" && value != null && value != undefined) {
            td.style.backgroundColor = colorError;
        }
    }

    return td;
}

// enderer para fechas
function rendererUnificadoFechaHora(instance, td, row, col, prop, value, cellProperties) {
    // 1. Aplicar el renderer específico según el tipo de columna
    if (prop === 'FechaInicialDesc' || prop === 'FechaFinalDesc') {
        dateTimeRenderer.apply(this, arguments);
    } else if (prop === 'HoraInicial' || prop === 'HoraFinal') {
        horaRenderer.apply(this, arguments);
    }

    // 2. Validación de traslapes (solo para columnas relevantes)
    const columnasRelevantes = [colFIni, colHIni, colFFin, colHFin];
    if (columnasRelevantes.includes(col)) {
        const allData = instance.getSourceData();
        const rowData = allData[row];

        //if (rowData.Nombre) { // Solo si tiene nombre de restricción
        if (rowData.FechaInicialDesc || rowData.FechaFinalDesc) { // Solo si tiene nombre de restricción

            const { filasConTraslape } = validarTablaDatos();

            if (filasConTraslape.includes(row)) {
                td.style.backgroundColor = colorError;
                td.title = "Existe traslape de fechas/horas con otro registro";

                // Mantener el estilo de error original si ya lo tenía
                if (!td.style.backgroundColor || td.style.backgroundColor === colorError) {
                    td.style.backgroundColor = colorError;
                }
            }
        }
    }

    return td;
}

function dateTimeRenderer(instance, td, row, col, prop, value, cellProperties) {
    Handsontable.renderers.TextRenderer.apply(this, arguments);

    // 2. Agrega la flecha (simulando dropdown)
    const arrow = document.createElement('div');
    arrow.className = 'htDatepickerArrow'; // Usamos una clase específica para fechas
    td.appendChild(arrow);
    td.style.position = 'relative';

    // Obtener datos de la fila
    const dataFila = instance.getSourceDataAtRow(row);
    const fechaIni = dataFila.FechaInicialDesc;
    const horaIni = dataFila.HoraInicial;
    const fechaFin = dataFila.FechaFinalDesc;
    const horaFin = dataFila.HoraFinal;

    // Validación de rango permitido ()
    if (value && (prop === 'FechaInicialDesc' || prop === 'FechaFinalDesc')) {
        const [dia, mes, anio] = value.split('/');
        const fechaObj = new Date(anio, mes - 1, dia);

        const fechaMin = moment(fecRI, "DD/MM/YYYY").toDate();
        const fechaMax = moment(fecRF, "DD/MM/YYYY").toDate();

        if (fechaObj < fechaMin || fechaObj > fechaMax) {
            td.style.backgroundColor = colorError;
        }
    }

    // Validación FECHA INICIAL > FECHA FINAL (independiente de la hora)
    if (fechaIni && fechaFin) {
        try {
            const [diaIni, mesIni, anioIni] = fechaIni.split('/');
            const [diaFin, mesFin, anioFin] = fechaFin.split('/');

            const fechaInicial = new Date(anioIni, mesIni - 1, diaIni);
            const fechaFinal = new Date(anioFin, mesFin - 1, diaFin);

            // Si FECHA INICIAL > FECHA FINAL → PINTAR ROJO AMBAS CELDAS
            if (fechaInicial > fechaFinal) {
                const colFechaIni = instance.propToCol('FechaInicialDesc');
                const colFechaFin = instance.propToCol('FechaFinalDesc');

                if (col === colFechaIni || col === colFechaFin) {
                    td.style.backgroundColor = colorError;
                }
            }
        } catch (error) {
            console.error("Error al comparar fechas:", error);
        }
    }

    // Validación HORA INICIAL > HORA FINAL (solo si las fechas son iguales)
    if (fechaIni && horaIni && fechaFin && horaFin && fechaIni === fechaFin) {
        try {
            const [horaIH, minI] = horaIni.split(':');
            const [horaFH, minF] = horaFin.split(':');

            if (parseInt(horaIH) > parseInt(horaFH) ||
                (parseInt(horaIH) === parseInt(horaFH) && parseInt(minI) >= parseInt(minF))) {

                const colHoraIni = instance.propToCol('HoraInicial');
                const colHoraFin = instance.propToCol('HoraFinal');

                if (col === colHoraIni || col === colHoraFin) {
                    td.style.backgroundColor = colorError;
                }
            }
        } catch (error) {
            console.error("Error al comparar horas:", error);
        }
    }

    return td;
}

// Renderer para texto centrado (ITEM)
function textoCentradoRenderer(instance, td, row, col, prop, value, cellProperties) {
    Handsontable.renderers.TextRenderer.apply(this, arguments);
    td.style.textAlign = 'center';
    td.style.verticalAlign = 'middle';
    return td;
}
function disponibilidadRenderer(instance, td, row, col, prop, value, cellProperties) {
    // Primero aplicamos el renderer base de texto
    Handsontable.renderers.TextRenderer.apply(this, arguments);

    // Obtenemos todos los datos
    const allData = instance.getSourceData();
    const totalFilas = allData.length;
    const rowData = allData[row];

    // Verificamos si es la última fila
    const esUltimaFila = (row === totalFilas - 1);
    const esPrimeraFila = (row === 0);
    const esSoloUnaFila = (totalFilas === 1);

    // Para la última fila: solo validar si hay datos en Nombre, Fechas u Horas
    if (esUltimaFila) {
        const tieneDatosRelevantes = (
            rowData.Nombre ||
            rowData.FechaInicialDesc || rowData.HoraInicial ||
            rowData.FechaFinalDesc || rowData.HoraFinal
        );

        if (tieneDatosRelevantes && (!value || value.toString().trim() === '')) {
            td.style.backgroundColor = colorError;
        }
    }
    // Para otras filas (excepto cuando solo hay una fila)
    else if (!esSoloUnaFila) {
        if (!value || value.toString().trim() === '') {
            td.style.backgroundColor = colorError;
        }
    }

    return td;
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
        botonEliminar.style.backgroundColor = colorError;
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

function checkboxCentradoRenderer(instance, td, row, col, prop, value, cellProperties) {
    // Usar el renderer estándar de checkbox
    Handsontable.renderers.CheckboxRenderer.apply(this, arguments);

    // Centrado adicional
    td.style.textAlign = 'center';
    td.style.verticalAlign = 'middle';

    // Asegurar que el checkbox esté centrado
    const checkbox = td.querySelector('input[type="checkbox"]');
    if (checkbox) {
        checkbox.style.margin = '0 auto';
        checkbox.style.display = 'block';
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

function fileNoEditable(instance, td, row, col, prop, value, cellProperties) {
    Handsontable.renderers.TextRenderer.apply(this, arguments);
    //td.style.fontWeight = 'bold';
    td.style.color = 'dimgray';
    td.style.background = 'bisque';
}

/**********************************************/
/***************** VALIDACIONES ***************/
function validarTablaDatos() {
    const data = tblHdnDispComb.getData();
    const errores = [];
    const registrosVistos = new Map(); // Para control de duplicados
    const filasConTraslape = new Set(); // Para marcar filas con traslapes

    const fechaMin = moment(fecRI, "DD/MM/YYYY").toDate();
    const fechaMax = moment(fecRF, "DD/MM/YYYY").toDate();
    //const rangoFechas = "02/05/2025 - 29/05/2025";
    const rangoFechas = fecRI + " - " + fecRF;

    // Nuevo: Mapa para agrupar registros por nombre
    const registrosPorNombre = new Map();

    data.forEach((row, rowIndex) => {
        const item = row.Item || rowIndex + 1;
        let tieneErrores = false;
        const erroresFila = [];
        const registrosDuplicados = [];

        var nombre = row[colNomb];
        var disponib = row[colDisp];
        var fecI = row[colFIni];
        var horaI = row[colHIni];
        var fecF = row[colFFin];
        var horaF = row[colHFin];


        // Determinar si es la última fila y está vacía (excepto Item y Considerar)
        const esUltimaFila = (rowIndex === data.length - 1);
        const esPrimeraFila = (rowIndex === 0);
        const esSoloUnaFila = (data.length === 1);

        const camposNoValidar = [colItem, colCons]; //['Item', 'Considerar']
        /*const camposParaValidar = [colNomb, colDisp, colFIni, colHIni, colFFin, colHFin]; */
        const camposParaValidar = [colNomb, colDisp,  colHIni,  colHFin]; 

        // Verificar si la última fila tiene algún dato (excepto Item y Considerar)
        const ultimaFilaConDatos = esUltimaFila && camposParaValidar.some(posCampo => {
            const valor = row[posCampo];
            return valor !== null && valor !== undefined && valor.toString().trim() !== '';
        });

        // Si hay más de una fila y la última está vacía, no validar
        if (data.length > 1 && esUltimaFila && !ultimaFilaConDatos) {
            return; // Saltar validación para esta fila
        }


        // Agrupar registros por nombre (solo si tiene nombre)
        if (nombre) {
            if (!registrosPorNombre.has(nombre)) {
                registrosPorNombre.set(nombre, []);
            }
            registrosPorNombre.get(nombre).push({
                item: item,
                fecI: fecI,
                horaI: horaI,
                fecF: fecF,
                horaF: horaF,
                rowIndex: rowIndex
            });
        }


        // Si solo hay una fila, siempre validar (incluso si está vacía)
        // Si hay múltiples filas, validar todas excepto la última si está vacía
        const camposObligatorios = [
            { key: colNomb, nombre: 'Nombre' },
            { key: colDisp, nombre: 'Disponibilidad' },
            { key: colFIni, nombre: 'Fecha Inicial' },
            { key: colHIni, nombre: 'Hora Inicial' },
            { key: colFFin, nombre: 'Fecha Final' },
            { key: colHFin, nombre: 'Hora Final' }
        ];

        //validacion campo vacios
        camposObligatorios.forEach(campo => {
            registrosDuplicados.push(row[campo.key] || '');

            //if (!row[campo.key] || row[campo.key].toString().trim() === '') {
            if (row[campo.key] === undefined || row[campo.key] === null || (typeof row[campo.key] === 'string' && row[campo.key].trim() === '')) {

                erroresFila.push({
                    Item: item,
                    Columna: campo.nombre,
                    Descripcion: 'Campo vacío'
                });
                tieneErrores = true;
            }
        });

        // Validar disponibilidad solo si no está vacía
        if (disponib && disponib.toString().trim() !== '') {            

            const strValor = disponib.toString().trim();

            // Verificamos si es un número con coma decimal (ej: "5,2")
            const isNumeroConComa = /^-?\d+,\d+$/.test(strValor);

            if (isNumeroConComa) {
                erroresFila.push({
                    Item: item,
                    Columna: 'Disponibilidad',
                    Descripcion: 'Use punto (.) como separador decimal en lugar de coma (,)'
                });
                tieneErrores = true;
            }
            else {

                // Verificamos si el string tiene un formato numérico válido (ej: "-123.456")
                const esNumeroValido = /^-?\d+(\.\d+)?$/.test(strValor);

                if (!esNumeroValido) {
                    erroresFila.push({
                        Item: item,
                        Columna: 'Disponibilidad',
                        Descripcion: 'Dato no numérico'
                    });
                    tieneErrores = true;
                }
                else {
                    const numValue = parseFloat(strValor);

                    if (numValue < 0) {
                        erroresFila.push({
                            Item: item,
                            Columna: 'Disponibilidad',
                            Descripcion: 'Dato negativo'
                        });
                        tieneErrores = true;
                    }

                    const partes = disponib.toString().split('.');
                    //const partes = row.Disponibilidad.toString().split('.');
                    if (partes[0].length > 8 || (partes[1] && partes[1].length > 4)) {
                        erroresFila.push({
                            Item: item,
                            Columna: 'Disponibilidad',
                            Descripcion: 'Dato incorrecto. Solo se permite como máximo 8 cifras enteras y 4 cifras decimales'
                        });
                        tieneErrores = true;
                    }
                }
            }
        }

        const validarFecha = (fechaStr, horaStr, campoFecha, campoHora, nombreCampo) => {
            if (!fechaStr) return; // ya está validado este caso

            // Primero validar el formato con una expresión regular
            const formatoValidoFecha = /^\d{1,2}\/\d{1,2}\/\d{2,4}$/.test(fechaStr);
            if (!formatoValidoFecha) {
                erroresFila.push({
                    Item: item,
                    Columna: nombreCampo,
                    Descripcion: 'Formato de fecha inválido (debe ser dd/mm/yyyy)'
                });
                tieneErrores = true;
                return;
            }

            // Validar formato de hora si se proporciona
            if (horaStr && !/^\d{2}:\d{2}$/.test(horaStr)) {
                erroresFila.push({
                    Item: item,
                    Columna: nombreCampo,
                    Descripcion: 'Formato de hora inválido (debe ser hh:mm)'
                });
                tieneErrores = true;
                return;
            }

            const [day, month, year] = fechaStr.split('/');
            let hora = "00";
            let minuto = "00";

            if (horaStr) {
                [hora, minuto] = horaStr.split(':');
            }

            // Verificar que los componentes sean números válidos
            const diaNum = parseInt(day, 10);
            const mesNum = parseInt(month, 10);
            const anioNum = parseInt(year, 10);
            const horaNum = parseInt(hora, 10);
            const minutoNum = parseInt(minuto, 10);

            if (isNaN(diaNum) || isNaN(mesNum) || isNaN(anioNum) ||
                isNaN(horaNum) || isNaN(minutoNum)) {
                erroresFila.push({
                    Item: item,
                    Columna: nombreCampo,
                    Descripcion: 'Formato de fecha/hora inválido (componentes no numéricos)'
                });
                tieneErrores = true;
                return;
            }

            try {
                const fecha = new Date(anioNum, mesNum - 1, diaNum, horaNum, minutoNum);

                // Verificar si la fecha creada es válida
                if (fecha.getFullYear() !== anioNum ||
                    fecha.getMonth() !== mesNum - 1 ||
                    fecha.getDate() !== diaNum ||
                    fecha.getHours() !== horaNum ||
                    fecha.getMinutes() !== minutoNum) {
                    throw new Error('Fecha/hora inválida');
                }

                // Crear fecha mínima (00:30 del día fechaMin)
                const [minDia, minMes, minAnio] = fecRI.split('/');
                const fechaMin = new Date(minAnio, minMes - 1, minDia, 0, 30);

                // Crear fecha máxima (00:00 del día siguiente a fechaMax)
                const [maxDia, maxMes, maxAnio] = fecRF.split('/');
                const fechaMaxDiaSiguiente = new Date(maxAnio, maxMes - 1, parseInt(maxDia) + 1, 0, 0);

                // Validar rangos
                if (horaStr && (fecha < fechaMin || fecha > fechaMaxDiaSiguiente)) {
                    const rangoFormateado = `${fecRI} 00:30 - ${fecRF} 24:00`;
                    erroresFila.push({
                        Item: item,
                        Columna: nombreCampo,
                        Descripcion: `Dato fuera de rango permitido [${rangoFormateado}]`
                    });
                    tieneErrores = true;

                    // Pintar celda de fecha
                    var colFecha = tblHdnDispComb.propToCol(campoFecha);
                    const cellFecha = tblHdnDispComb.getCell(rowIndex, colFecha);
                    if (cellFecha) {
                        cellFecha.style.backgroundColor = colorError;
                    }

                    // Pintar celda de hora si existe
                    var colHora = tblHdnDispComb.propToCol(campoHora);
                    if (colHora !== null) {
                        const cellHora = tblHdnDispComb.getCell(rowIndex, colHora);
                        if (cellHora) {
                            cellHora.style.backgroundColor = colorError;
                        }
                    }
                }

            } catch (e) {

                //Evitamos falsos errores de fecha y hora final
                if (horaNum == 24 && minutoNum == 0) {

                } else {
                    erroresFila.push({
                        Item: item,
                        Columna: nombreCampo,
                        Descripcion: 'Fecha/hora inválida'
                    });
                    tieneErrores = true;
                }
            }
        };

        validarFecha(fecI, horaI, 'FechaInicialDesc', 'HoraInicial', 'Fecha Inicial');
        validarFecha(fecF, horaF, 'FechaFinalDesc', 'HoraFinal', 'Fecha Final');

        //valido fecha Ini menos a fecha Fin
        if (fecI && fecF) {
            try {
                const [diaI, mesI, anioI] = fecI.split('/');
                const [diaF, mesF, anioF] = fecF.split('/');


                const fechaIni = new Date(anioI, mesI - 1, diaI);
                const fechaFin = new Date(anioF, mesF - 1, diaF);

                if (fechaIni > fechaFin) {
                    erroresFila.push({
                        Item: item,
                        Columna: "Fecha Inicial - Fecha Final",
                        Descripcion: 'La Fecha Inicial es mayor a la Fecha Final'
                    });
                    tieneErrores = true;
                }
            } catch (error) {
                //erroresFila.push({
                //    Item: item,
                //    Columna: "Fecha Inicial - Fecha Final",
                //    Descripcion: 'Formato de fecha inválido (debe ser DD/MM/YYYY)'
                //});
                //tieneErrores = true;
            }
        }

        // Control de duplicados
        const registroKey = registrosDuplicados.join('|');
        if (registroKey && registroKey !== '|||||') { // Solo si no es completamente vacío
            if (registrosVistos.has(registroKey)) {
                // Marcar este registro como duplicado
                erroresFila.push({
                    Item: item,
                    Columna: 'Registro',
                    Descripcion: 'Registro Duplicado'
                });
                tieneErrores = true;

                // Marcar también el registro original si aún no está marcado
                const originalItem = registrosVistos.get(registroKey);
                if (!errores.some(e => e.Item === originalItem && e.Descripcion === 'Registro Duplicado')) {
                    errores.push({
                        Item: originalItem,
                        Columna: 'Registro',
                        Descripcion: 'Registro Duplicado'
                    });
                }
            } else {
                registrosVistos.set(registroKey, item);
            }
        }

        // Validar Hora Inicial y Hora Final
        if (horaI && !horariosDisponiblesIni.includes(horaI)) {
            erroresFila.push({
                Item: item,
                Columna: 'Hora Inicial',
                Descripcion: 'Elemento fuera del listado de horas o con formato inválido. Use hh:mm en intervalos de 30 minutos del listado (ej: 08:00, 14:30)'
            });
            tieneErrores = true;
        }

        if (horaF && !horariosDisponiblesFin.includes(horaF)) {
            erroresFila.push({
                Item: item,
                Columna: 'Hora Final',
                Descripcion: 'Elemento fuera del listado de horas o con formato inválido. Use hh:mm en intervalos de 30 minutos del listado (ej: 08:00, 14:30)'
            });
            tieneErrores = true;
        }

        if (fecI && horaI && fecF && horaF) {
            try {
                const [diaI, mesI, anioI] = fecI.split('/');
                const [horaIH, minI] = horaI.split(':');

                const [diaF, mesF, anioF] = fecF.split('/');
                const [horaFH, minF] = horaF.split(':');

                const fechaHoraIni = new Date(anioI, mesI - 1, diaI, horaIH, minI);
                const fechaHoraFin = new Date(anioF, mesF - 1, diaF, horaFH, minF);

                // Si las fechas son iguales y la hora inicial es mayor o igual a la final
                if (fecI === fecF && fechaHoraIni >= fechaHoraFin) {
                    erroresFila.push({
                        Item: item,
                        Columna: "Fecha y Hora Inicial - Fecha y Hora Final",
                        Descripcion: 'La Fecha y Hora Inicial no debe ser igual o mayor a la Fecha y Hora Final'
                    });
                    tieneErrores = true;
                }
            } catch (error) {
                console.error("Error al comparar horas:", error); //Para validacion interna
            }
        }

        if (tieneErrores) {
            errores.push(...erroresFila);
        }
    });

    // Traslapes: Verificar traslapes para registros con el mismo nombre
    registrosPorNombre.forEach((registros, nombre) => {
        if (registros.length > 1) {
            // Ordenar registros por fecha/hora inicial
            registros.sort((a, b) => {
                const fechaA = convertirAFecha(a.fecI, a.horaI);
                const fechaB = convertirAFecha(b.fecI, b.horaI);
                return fechaA - fechaB;
            });

            // Verificar traslapes entre registros consecutivos
            for (let i = 0; i < registros.length - 1; i++) {
                const actual = registros[i];
                const siguiente = registros[i + 1];

                const finActual = convertirAFecha(actual.fecF, actual.horaF);
                const inicioSiguiente = convertirAFecha(siguiente.fecI, siguiente.horaI);
                const finActual30MinMas = finActual != null ? new Date(finActual.getTime() + (30 * 60 * 1000)) : null;

                if (finActual30MinMas > inicioSiguiente) {
                    //if (finActual > inicioSiguiente) {
                    // Hay traslape

                    // Marcar ambas filas como con traslape
                    filasConTraslape.add(actual.rowIndex);
                    filasConTraslape.add(siguiente.rowIndex);

                    errores.push({
                        Item: actual.item,
                        Columna: 'Rango de fechas',
                        //Descripcion: `Existe Traslape de fechas con el registro del ítem ${siguiente.item} para el nombre "${nombre}"`
                        Descripcion: `Existe Traslape de fechas con el registro del ítem ${siguiente.item} para la restricción "${nombre}"`
                    });
                    errores.push({
                        Item: siguiente.item,
                        Columna: 'Rango de fechas',
                        Descripcion: `Existe Traslape de fechas con el registro del ítem  ${actual.item} para la restricción "${nombre}"`
                        //Descripcion: `Traslape detectado con el registro ${actual.item} para el nombre "${nombre}"`
                    });
                }
            }
        }
    });

    // Ordenar por Item (numérico ascendente)
    errores.sort((a, b) => a.Item - b.Item);

    //return errores;
    // Devuelve ambos: errores y filas con traslape
    return {
        errores: errores.sort((a, b) => a.Item - b.Item),
        filasConTraslape: Array.from(filasConTraslape)
    };
}

// Función auxiliar para convertir fecha y hora en string a objeto Date
function convertirAFecha(fechaStr, horaStr) {
    if (!fechaStr || !horaStr) return null;

    try {
        const [dia, mes, anio] = fechaStr.split('/');
        const [horas, minutos] = horaStr.split(':');

        return new Date(
            parseInt(anio),
            parseInt(mes) - 1,
            parseInt(dia),
            parseInt(horas),
            parseInt(minutos)
        );
    } catch (e) {
        console.error("Error al convertir fecha:", e);
        return null;
    }
}

function mostrarListadoErrores(siempreMostrarVentana) {
    var tieneErrores = false;
    tblError.clear();

    const dataValidada = validarTablaDatos();
    const erroresEncontrados = dataValidada.errores;

    if (siempreMostrarVentana) {
        if (erroresEncontrados && erroresEncontrados.length > 0) {
            tieneErrores = true;
        }
        cargarListaErrores(erroresEncontrados);
        abrirPopup("erroresDC");

    } else {
        if (erroresEncontrados && erroresEncontrados.length > 0) {
            tieneErrores = true;
            cargarListaErrores(erroresEncontrados);
            abrirPopup("erroresDC");
        }
    }
    return tieneErrores;
}

function cargarListaErrores(lstErrores) {
    tblError.clear();
    tblError.rows.add(lstErrores).draw();
}

function detectarNombresDuplicados(data) {
    const nombresMap = new Map();
    const filasDuplicadas = [];

    data.forEach((row, index) => {
        const nombre = row[colNomb];
        if (nombre) {
            if (!nombresMap.has(nombre)) {
                nombresMap.set(nombre, []);
            }
            nombresMap.get(nombre).push(index);
        }
    });

    nombresMap.forEach((filas, nombre) => {
        if (filas.length > 1) {
            filasDuplicadas.push(...filas);
        }
    });

    return filasDuplicadas;
}

function cargarSemanasParaPopup() {
    limpiarBarraMensaje("mensaje2");

    var anho = $('#AnhoCopiar').val();

    if (!anho) {
        $('#cbSemanaCopiar').html('');
        return;
    }

    $.ajax({
        type: 'POST',
        url: controlador + 'CargarSemanasPopup',
        data: {
            idAnho: anho
        },
        success: function (data) {
            // Limpiar el select
            var comboFechas = $('#cbSemanaCopiar').empty();

            // Verificar si la respuesta es htmml
            if (typeof data === 'string' && data.startsWith('<select')) {
                // Si es html, reemplazar directamente
                comboFechas.replaceWith(data);
            } else {
                mostrarMensaje('mensaje2', 'error', "Error al cargar las semanas.");
            }

            // Seleccionar la semana actual si está disponible
            var semanaActual = $('#hfSemana').val();
            if (semanaActual) {
                $('#cbSemanaCopiar').val(semanaActual);
            }
        },
        error: function () {
            mostrarMensaje('mensaje2', 'error', "Ha ocurrido un error al cargar las semanas.");
          
        }
    });
}

function copiarDatos() {

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

function horizonteCopia() {

    idHorizonte = $("#cbHorizonte").val();
    var opcionCopia = $('#cbHorizonteCopia').val();
    switch (parseInt(opcionCopia)) {
        case 1: //dia
        case 2: //Reprograma
            $('#dDiaCopia').css("display", "block");
            $('#dSemanaCopia').css("display", "none");
            if (idHorizonte == 3) {
                caso = 3;
                $('#tdFechaDestinoIni').css("display", "block");
                $('#tdFechaDestinoFin').css("display", "block");
            }
            else {
                caso = 1;
                $('#tdFechaDestinoIni').css("display", "none");
                $('#tdFechaDestinoFin').css("display", "none");
            }

            break;
        case 3: //semanal
            $('#dDiaCopia').css("display", "none");
            $('#dSemanaCopia').css("display", "block");
            obtenerRangoFechasPorSemanaCopia();

            if (idHorizonte == 3) {
                caso = 4;
                $('#tdFechaDestinoFin').css("display", "none");
            }

            else {
                caso = 2;
                $('#tdFechaDestinoFin').css("display", "block");
            }


            break;

    }
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


function grabarCopia() {

    var idHorizonte = $("#cbHorizonte").val();
    var fechaP = $("#txtFecha").val() || "";

    var varHorizonteC = parseInt($("#cbHorizonteCopia").val()) || 0; //siempre
    var varFechaC = $("#txtFechaCopia").val() || "";
    var varAnioC = $("#AnhoCopia").val() || "";
    var varSemanaC = $("#cbSemanaCopia").val() || "";
    var varFechaOriIniC = $("#txtFechaOrigenIni").val() || "";
    var varFechaOriFinC = $("#txtFechaOrigenFin").val() || "";
    var varFechaDestIniC = $("#txtFechaDestinoIni").val() || "";
    var varFechaDestFinC = $("#txtFechaDestinoFin").val() || "";


    //idHorizonteCopia = $("#cbHorizonteCopia").val();
    var idHorizonteCopia = varHorizonteC;
    var fechaCopia = $("#txtFechaCopia").val();
    //semanaCopia = $('#cbSemanaCopia').val();
    var semanaCopia = varSemanaC;
    //anhoCopia = $('#AnhoCopia').val();
    var anhoCopia = varAnioC;
    //semanaCopia = anhoCopia.toString() + semanaCopia;
    varSemanaC = varAnioC.toString() + varSemanaC;

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
        url: controlador + "CopiarDatosDC",
        type: 'POST',
        data: {
            fechaVentana: fechaP,
            caso: caso,
            horizonteC: varHorizonteC,
            fechaC: varFechaC,
            anioC: varAnioC,
            semanaC: varSemanaC,
            fechaOriIniC: varFechaOriIniC,
            fechaOriFinC: varFechaOriFinC,
            fechaDestIniC: varFechaDestIniC,
            fechaDestFinC: varFechaDestFinC
        },
        success: function (evt) {
            if (evt.Resultado == "1") {
                var asd = 0;

                var dataG = evt.DataRestricciones;
                var lstRegistrosEncontrados = dataG.LstRestricciones;

                if (lstRegistrosEncontrados.length > 0) {
                    cargarHansonTablas(dataG, true, false);
                    setTimeout(() => {
                        //mostrarMensaje('mensaje', 'exito', 'La información fue copiada correctamente.');
                        mostrarMensaje('mensaje', 'exito', 'La información fue copiada correctamente. Para guardar esta información debe presionar "Enviar Datos".');
                    }, 1000); // 1000 milisegundos = 1 segundo
                    cerrarPopup('popupCopiar');
                }
                else {
                    mostrarMensaje('mensaje2', 'alert', 'No se encontró informacion a copiar para la fecha seleccionada.');
                }




            } else {
                mostrarMensaje('mensaje2', 'error', 'Error: ' + evt.Mensaje);
            }
        },
        error: function () {
            mostrarMensaje('mensaje2', 'error', "No se ha podido copiar la información.");
        }
    });

}

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
    limpiarBarraMensaje('mensajeErrorI');

    $.ajax({
        type: 'POST',
        url: controlador + 'ImportarRestriccionesExcel',
        dataType: 'json',
        async: true,
        data: {
        },
        success: function (evt) {
            if (evt.Resultado == "1") {

                var dataG = evt.DataRestricciones;

                // Verificar si hay errores antes de mostrarlos
                if (dataG.Errores && dataG.Errores.length > 0) {
                    cargarListaErroresImportacion(dataG.Errores);
                    abrirPopup("erroresImportacionDC");
                    //mostrarMensaje('mensaje', 'alert', 'El archivo seleccionado contiene errores generales en su información. para corregirlo tome en cuenta las notas de cada celda en la plantilla.');
                    mostrarMensaje('mensajeErrorI', 'alert', 'El archivo importado contiene errores generales en su información. para corregirlo tome en cuenta las notas de cada celda en la plantilla.');
                } else {


                    inicioDetalles(dataG.ListadoTotalRestricciones, false);
                    cargarHansonTablas(dataG, false, true);
                    mostrarMensaje('mensaje', 'exito', 'Importación completada correctamente.');
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


function mostrarVentanaListaEnvios() {
    $('#idEnviosAnteriores').html(dibujarTablaEnvios());
    setTimeout(function () {
        $('#enviosanteriores').bPopup({
            easing: 'easeOutBack',
            speed: 450,
            transition: 'slideDown',
            modalClose: false
        });
        $('#tablalenvio').dataTable({
            "scrollY": 300,
            //"scrollX": true,
            "sDom": 't',
            "ordering": false,
            "bPaginate": false,
            "iDisplayLength": -1
        });
    }, 50);
}

function dibujarTablaEnvios() {

    lista = listaEnvios;

    var cadena = "<div style='clear:both; height:5px'></div> ";
    cadena += "<table id='tablalenvio' border='1' class='pretty tabla-adicional' cellspacing='0'>";
    cadena += "<thead><tr><th style='width: 60px'>Id Envío</th><th style='width: 90px'>Fecha Hora</th><th style='width: 120px'>Usuario</th></tr></thead>";
    cadena += "<tbody>";

    for (key in lista) {
        cadena += "<tr onclick='mostrarEnvioRestricciones(" + lista[key].IdEnvio + ");' style='cursor:pointer'><td>" + lista[key].IdEnvio + "</td>";
        cadena += "<td>" + lista[key].FechaEnviosDesc + "</td>";
        cadena += "<td>" + lista[key].Usuario + "</td></tr>";
    }
    cadena += "</tbody></table>";
    return cadena;

}

function mostrarEnvioRestricciones(idEnvio) {
    limpiarBarraMensaje("mensaje");
    mostrandoDesdeHistorialEnvios = true;
    $.ajax({
        type: 'POST',
        url: controlador + "MostrarEnvio",
        data: {
            idEnvio: idEnvio
        },
        success: function (evt) {
            if (evt.Resultado == "1") {

                cerrarPopup('enviosanteriores');

                var dataG = evt.DataRestricciones;

                inicioDetalles(dataG.ListadoTotalRestricciones, mostrandoDesdeHistorialEnvios);

                cargarHansonTablas(dataG, false, false);

                var numRestricciones = dataG.LstRestricciones.length;
                if (numRestricciones == 0) {
                    mostrarMensaje('mensaje', 'alert', 'No se encontró información para el filtro seleccionado.');
                }

                if (evt.IdEnvio > 0) {
                    var mensaje = "<strong>Código de envío</strong> : " + evt.IdEnvio + ", <strong>Fecha de envío: </strong>" + evt.FechaProceso;
                    mostrarMensaje('mensaje', 'exito', mensaje);
                }


            } else {
                mostrarMensaje('mensaje', 'error', evt.Mensaje);
            }


        },
        error: function (xhr, status) {
            mostrarMensaje('mensaje', 'error', 'Se ha producido un error.');
        }
    });
}

/**********************************************/
/********************* DATA *******************/

function cargarListadoRestricciones(listado) {
    listaRestriccionesTotales.length = 0; // Limpiar el array antes de llenarlo

    if (listado && Object.keys(listado).length > 0) {

        for (const [idStr, nombre] of Object.entries(listado)) {
            const id = parseInt(idStr, 10); // Base 10 para evitar problemas("020" -> 20)

            if (!isNaN(id)) { // (por si acaso)
                listaRestriccionesTotales.push({
                    id: id,     // Ahora es un número
                    nombre: nombre
                });
            }
        }

    }
}



// Lista de horarios desde 00:30 hasta 24:00 en intervalos de 30 minutos
var horariosDisponiblesIni = [
    "00:30", "01:00", "01:30", "02:00", "02:30", "03:00", "03:30", "04:00",
    "04:30", "05:00", "05:30", "06:00", "06:30", "07:00", "07:30", "08:00",
    "08:30", "09:00", "09:30", "10:00", "10:30", "11:00", "11:30", "12:00",
    "12:30", "13:00", "13:30", "14:00", "14:30", "15:00", "15:30", "16:00",
    "16:30", "17:00", "17:30", "18:00", "18:30", "19:00", "19:30", "20:00",
    "20:30", "21:00", "21:30", "22:00", "22:30", "23:00", "23:30", "00:00"
];

// Lista de horarios desde 00:30 hasta 24:00 en intervalos de 30 minutos
var horariosDisponiblesFin = [
    "00:30", "01:00", "01:30", "02:00", "02:30", "03:00", "03:30", "04:00",
    "04:30", "05:00", "05:30", "06:00", "06:30", "07:00", "07:30", "08:00",
    "08:30", "09:00", "09:30", "10:00", "10:30", "11:00", "11:30", "12:00",
    "12:30", "13:00", "13:30", "14:00", "14:30", "15:00", "15:30", "16:00",
    "16:30", "17:00", "17:30", "18:00", "18:30", "19:00", "19:30", "20:00",
    "20:30", "21:00", "21:30", "22:00", "22:30", "23:00", "23:30", "24:00"
];


/**********************************************/
/************ Funciones Generales  ************/
function abrirPopup(id) {
    setTimeout(function () {
        $("#" + id).bPopup({
            easing: 'easeOutBack',
            speed: 450,
            transition: 'slideDown',
            modalClose: false
        });
    }, 50);
}

function cerrarPopup(id) {
    $("#" + id).bPopup().close()
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


