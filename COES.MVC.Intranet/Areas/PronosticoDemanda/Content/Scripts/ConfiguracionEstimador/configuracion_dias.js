var ht;
const mascara = { S: "Si", N: "No" };
$(document).ready(function () {
    $('.f-fecha').Zebra_DatePicker({
        onSelect: function () {//Evento onChange
            var a = $('#id-desde').val();
            var b = $('#id-hasta').val();
            if (ValidateDates(a, b) == false) {
                b = AddDays(a);
                $('#id-hasta').val(b);
            }  
            ActualizarDatos();
        }
    });

    $('#btn-guardar').on('click', function () {
        const res = ObtenerParametros();
        if (res == -1) {
            SetMessage('#message',
                'Debe activar al menos un parámetro para realizar el registro!',
                'warning', true);
            return false;
        }
        if (res == -2) {
            SetMessage('#message',
                'Existen parámetros activos a los que no se le han asignado valores!',
                'warning', true);
            return false;
        }
        RegistrarDatos(res);
    });

    $('.chk-single input[type="checkbox"]').on('click', function () {
        ParameterSingleOnOff(this, '.chk-single', true);
    })

    ObtenerDatos();
});

function ObtenerDatos() {
    $.ajax({
        type: 'POST',
        url: controller + 'ConfiguracionDiasDatos',
        contentType: 'application/json; charset=utf-8',
        data: JSON.stringify({
            fechaIni: $('#id-desde').val(),
            fechaFin: $('#id-hasta').val(),
        }),
        datatype: 'json',
        traditional: true,
        success: function (result) {
            IniciarHandson(result);
        },
        error: function () {
            alert("Ha ocurrido un problema...");
        }
    });
}

function ActualizarDatos() {
    $.ajax({
        type: 'POST',
        url: controller + 'ConfiguracionDiasDatos',
        contentType: 'application/json; charset=utf-8',
        data: JSON.stringify({
            fechaIni: $('#id-desde').val(),
            fechaFin: $('#id-hasta').val(),
        }),
        datatype: 'json',
        traditional: true,
        success: function (result) {
            ht.loadData(result);
        },
        error: function () {
            alert("Ha ocurrido un problema...");
        }
    });
}

function RegistrarDatos(parametros) {
    $.ajax({
        type: 'POST',
        url: controller + 'ConfiguracionDiasRegistrar',
        contentType: 'application/json; charset=utf-8',
        data: JSON.stringify({
            parametros: parametros,
            fechaIni: $('#id-desde').val(),
            fechaFin: $('#id-hasta').val(),
        }),
        datatype: 'json',
        traditional: true,
        success: function (result) {
            SetMessage('#message', result.dataMsg, result.typeMsg, true);
            ActualizarDatos();
        },
        error: function () {
            alert("Ha ocurrido un problema...");
        }
    });
}

function IniciarHandson(modelo) {
    const container = document.getElementById("ht");
    ht = new Handsontable(container, {
        data: modelo,
        fillHandle: false,
        readOnly: true,
        minSpareCols: 0,
        minSpareRows: 0,
        stretchH: "all",        
        className: "htCenter",
        colHeaders: ["Fecha", "Feriado", "Atípico", "Veda"],        
        columns: [
            {
                data: 0,                
                renderer: FechaColumnRenderer,
            },
            {
                data: 1,
                renderer: CustomColumnRenderer,
            },
            {
                data: 2,
                renderer: CustomColumnRenderer,
            },
            {
                data: 3,
                renderer: CustomColumnRenderer,
            },
        ],
    });
}

//Activa o desactiva un solo item
function ParameterSingleOnOff(element, elem_class, clear) {
    var target = $(element).closest(elem_class).next();
    var is_num = $(target).find('input[type="number"]');
    var is_rdo = $(target).find('input[type="radio"]');

    if (element.checked) {
        is_num.prop('disabled', false);
        is_rdo.prop('disabled', false);
    }
    else {
        is_num.prop('disabled', true);
        is_rdo.prop('disabled', true);

        if (clear) {
            is_num.prop('value', '');
            is_rdo.prop('checked', false);
        }
    }
}

function ObtenerParametros() {
    let res_object = {};
    const all_checked = $('.chk-single input[type="checkbox"]:checked');//Obtiene todos los elementos individuales seleccionados
    $.each(all_checked, function (j, s_item) {
        let field_value;
        const field_name = s_item.id;//Obtiene el nombre del parametro a registrar
        const s_target = $(s_item).closest('.chk-single').next();
        //Obtiene las posibles entradas
        const s_item_num = $(s_target).find('input[type="number"]');
        const s_item_rdo = $(s_target).find('input[type="radio"]:checked');
        //Evalua el tipo de entrada
        if (s_item_num.length != 0) {
            //obtener el valor
            field_value = s_item_num.val();
        }
        if (s_item_rdo.length != 0) {
            //obtener el valor
            field_value = s_item_rdo.val();
        }
        //Carga el campo del objeto
        res_object[field_name] = field_value;
    });

    //Validaciones
    //Objeto vacio
    if (jQuery.isEmptyObject(res_object)) {
        res_object = -1;
    }
    //Campos null or empty
    $.each(res_object, function (i, value) {
        if (!value) {
            res_object = -2;
            return false;
        }
    });

    return res_object;
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

//Estilo para la columna fechas del Handsontable
function FechaColumnRenderer(instance, td, row, col, prop, value, cellProperties) {
    Handsontable.renderers.TextRenderer.apply(this, arguments);
    td.style.fontWeight = 'bold';
    td.style.background = '#F2F2F2';
}
//Estilo para la mascara de los valores a mostrar
function CustomColumnRenderer(instance, td, row, col, prop, value, cellProperties) {
    Handsontable.renderers.TextRenderer.apply(this, arguments);
    td.innerHTML = mascara[value];
}
