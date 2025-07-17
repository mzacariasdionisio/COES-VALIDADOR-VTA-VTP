var objHt, pop;
$(document).ready(function () {
    //Aplica multiselect
    $('.f-select').multipleSelect({
        filter: true,
        single: false,
        placeholder: 'Seleccione',
        onClose: function () {           
            cargarDatos(e);
        }
    });
    $('#id-ciudad').multipleSelect('checkAll');

    //Aplica la lib. Zebra_Datepicker
    $('.f-fecha').Zebra_DatePicker({
        onSelect: function () {//Evento onChange
            var a = $('#id-desde').val();
            var b = $('#id-hasta').val();
            if (ValidateDates(a, b) == false) {
                b = AddDays(a);
                $('#id-hasta').val(b);
            }
            cargarDatos();
        }
    });
    $('#btn-api').on('click', function () {
        pop = $('#popup').bPopup({
            easing: 'easeOutBack',
            speed: 350,
            transition: 'fadeIn',
            modalClose: false,
            onOpen: function () {
                SetMessage('#pop-mensaje',
                    '• Se obtendran los datos de las variables exógenas directamente desde el API openweathermap.org .<br>' +
                    '• Los datos obtenidos sera información pronosticada de hasta 4 días a futuro incluyendo hoy.<br>' +
                    '• Pueden existir información faltante dependiendo de la hora(hoy) que se ejecute el proceso.',
                    'warning');
            }
        });
    });

    $('#pop-api').on('click', function () {        
        obtenerDatosApi();
    });

    iniciarHandson();    
});

function iniciarHandson() {
    var container = document.getElementById('ht');
    objHt = new Handsontable(container, {
        data: [],
        fillHandle: true,
        minSpareCols: 0,
        minSpareRows: 0,
        className: "htCenter",
        stretchH: "all",
        columns: [
            {
                data: 0,
                title: 'Fecha y Hora',
                readOnly: true,
                renderer: HoraColumnRenderer
            },
            {
                data: 1,
                title: 'Ciudad',
                readOnly: true
            },
            {
                data: 2,
                title: 'Temperatura (°C)',
                readOnly: true
            },
            {
                data: 3,
                title: 'Sensación Térmica (°C)',
                readOnly: true
            },
            {
                data: 4,
                title: 'Nubosidad (%)',
                readOnly: true
            },
            {
                data: 5,
                title: 'Humedad (%)',
                readOnly: true,
            }]        
    });
    //carga los datos al iniciar el módulo (solo una vez)
    cargarDatos();
}

//Recoge todos los filtros seleccionados, actualiza las listas dependientes y muestra el detalle
function cargarDatos() {
    $.ajax({
        type: 'POST',
        url: controller + 'ExogenaList',
        contentType: 'application/json; charset=utf-8',
        data: JSON.stringify({
            fecini: $('#id-desde').val(),
            fecfin: $('#id-hasta').val(),
            ciud: $('#id-ciudad').val()
        }),
        datatype: 'json',
        success: function (modelo) {        
            const filas = modelo.length;
            const columnas = modelo[0].length;
            objHt.updateSettings({
                maxCols: columnas,
                maxRows: filas,
                data: modelo
            });
        },
        error: function myfunction() {
            SetMessage('#message',
                'Ha ocurrido un error al intentar mostrar los registros...',
                'error');
        }
    });
}

//Agrega una dia a una fecha(resultado en string)
function AddDays(value) {
    var ndias = 1;
    var dt = ConvertStringToDate(value);
    dt.setDate(dt.getDate() + ndias);

    var dd = (dt.getDate() < 10 ? '0' : '') + dt.getDate();
    var MM = ((dt.getMonth() + 1) < 10 ? '0' : '') + (dt.getMonth() + 1);
    var yyyy = dt.getFullYear();

    var dtf = dd + '/' + MM + '/' + yyyy;

    return dtf;
}

//Valida las fechas ingresadas
function ValidateDates(fecini, fecfin) {
    var valid = false;
    var fini = ConvertStringToDate(fecini);
    var ffin = ConvertStringToDate(fecfin);

    if (ffin.getTime() >= fini.getTime()) {
        valid = true;
    }

    return valid;
}

//Convierte un tipo string a tipo date
function ConvertStringToDate(value) {
    var pattern = /^(\d{1,2})\/(\d{1,2})\/(\d{4})$/;
    var arrayDate = value.match(pattern);
    var dt = new Date(arrayDate[3], arrayDate[2] - 1, arrayDate[1]);

    return dt;
}

//Cambio de color de las columnas del Handsontable
function HoraColumnRenderer(instance, td, row, col, prop, value, cellProperties) {
    Handsontable.renderers.TextRenderer.apply(this, arguments);
    td.style.fontWeight = 'bold';
    td.style.background = '#F2F2F2';
}

function obtenerDatosApi() {
    $.ajax({
        type: 'POST',
        url: controller + 'EjecutarExogena',
        data: {
        },
        datatype: 'json',
        success: function (result) {            
            SetMessage('#message', result.Mensaje, result.TipoMensaje);            
        },
        error: function () {
            SetMessage('#message',
                'Ha ocurrido un error al intentar obtener los datos del API...',
                'error');
        },
        complete: function () {
            pop.close();
            cargarDatos();
        }
    });
}