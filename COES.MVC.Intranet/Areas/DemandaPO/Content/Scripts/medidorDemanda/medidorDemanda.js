var name = "MedidorDemanda/";
var controller = siteRoot + "DemandaPO/" + name;
var dt;

$(document).ready(function () {

    $('.f-fecha').Zebra_DatePicker({
        onSelect: function () {
            var a = $('#fechaIni').val();
            var b = $('#fechaFin').val();
            if (ValidateDates(a, b) == false) {
                b = AddDays(a);
                $('#fechaFin').val(b);
            }
        }
    });

    //$('#idBarra').multipleSelect({
    //    single: false,
    //});

    //$('#idBaseDatos').multipleSelect({
    //    single: true,
    //});

    //$('#idVersion').multipleSelect({
    //    single: true,
    //    placeholder: '--Seleccione--'
    //});

    //$('#idParametro').multipleSelect({
    //    single: true,
    //});

    $('#idVersion').on('change', function () {
        let version = $('#idVersion').val();
        listarBarrasxVersion(version);
    });

    $('#btnConsultar').click(function () {
        consultarData();
    });

    $('#btnExportar').click(function () {
        exportarData();
    });

    listarBarrasxVersion();
});


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

listarBarrasxVersion = function () {
    $('option', '#idBarra').remove();
    let version = $('#idVersion').val();
    console.log(version, 'version');
    $.ajax({
        type: 'POST',
        url: controller + 'ListaBarrasxVersion',
        contentType: 'application/json; charset=utf-8',
        data: JSON.stringify({
            version: version
        }),
        cache: false,
        success: function (result) {
            console.log(result, 'result');
            $('#idBarra').get(0).options.length = 0;
            $('#idBarra').get(0).options[0] = new Option("--Todos--", "-1");
            $.each(result.ListaBarras, function (i, item) {
                $('#idBarra').get(0).options[$('#idBarra').get(0).options.length] = new Option(item.Gruponomb, item.Barsplcodi);
                //$('#idBarra').append(new Option(item.Gruponomb, item.Barsplcodi));
            });
        },
        error: function () {
            mostrarError();
        }
    });
}

mostrarError = function () {
    $('#mensaje').removeClass();
    $('#mensaje').addClass('action-error');
    $('#mensaje').text("Ha ocurrido un error.");
}

function consultarData() {
    const inicio = $('#fechaIni').val();
    const fin = $('#fechaFin').val();
    const fuente = $('#idBaseDatos').val();
    const version = $('#idVersion').val();
    const barra = ($('#idBarra').val()) ? $('#idBarra').val() : -1;
    const parametro = $('#idParametro').val();

    var columnas = [
        { title: 'Fecha', data: 'Fecha' },
        { title: 'Punto Medicion', data: 'NombrePunto' },
        { title: 'Transformador', data: 'NombreTransformador' },
        { title: 'Barra', data: 'NombreBarra' },
        { title: 'Total Energia Activa (MW)', data: 'Total' }
    ];

    let c = 1;
    for (var hora = 0; hora < 24; hora++) {
        for (var minuto = 0; minuto < 60; minuto += 15) {
            var h = ('0' + hora).slice(-2) + ':' + ('0' + minuto).slice(-2);
            columnas.push({ title: h, data: 'H' + c });
            c++;
        }
    }

    $.ajax({
        type: 'POST',
        url: controller + 'ConsultarData',
        contentType: 'application/json; charset=utf-8',
        data: JSON.stringify({
            inicio: inicio,
            fin: fin,
            fuente: fuente,
            version: version,
            barra: barra,
            parametro: parametro
        }),
        datatype: 'json',
        traditional: true,
        success: function (result) {
            console.log(result, 'result');
            dt = $('#dtDatos').DataTable({
                data: result,
                columns: columnas,
                initComplete: function () {
                    $('#dt').css('width', '100%');
                },
                drawCallback: function () {
                    $('#dt').css('width', '100%');
                },
                searching: false,
                bLengthChange: false,
                bSort: false,
                destroy: true,
                pageLength: 10,
                info: false,
            });
        },
        error: function () {
            alert("Ha ocurrido un problema...");
        }
    });
}

function exportarData() {
    
    let cabecera = $('#dtDatos thead th').map(function () {
        return $(this).text();
    }).get();

    let dato = dt.data().toArray();

    $.ajax({
        type: 'POST',
        url: controller + 'ExportarData',
        contentType: 'application/json',
        data: JSON.stringify({
            cabecera: cabecera,
            dato: dato
        }),
        datatype: 'json',
        traditional: true,
        success: function (result) {
            if (result != -1) {
                window.location = controller + 'abrirarchivo?formato=' + 1 + '&file=' + result;
                //mostrarMensaje('mensaje', 'exito', "Felicidades, el archivo se descargo correctamente...!");
                SetMessage('#message', 'El archivo se descargo correctamente...', 'success', false);
            }
            else {
                //mostrarMensaje('mensaje', 'error', "Lo sentimos, ha ocurrido un error inesperado");
                SetMessage('#message', 'Lo sentimos, ha ocurrido un error inesperado...', 'warning', false);
            }
        },
        error: function () {
            alert("Ha ocurrido un problema...");
        }
    });
}

function RefillDropDownList(element, data, data_id, data_name) {
    //Vacia el elemento
    element.empty();
    //Carga el elemento
    $.each(data, function (i, item) {
        var n_value = i, n_html = item;
        if (data_id) n_value = item[data_id];
        if (data_name) n_html = item[data_name];
        element.append($('<option></option>').val(n_value).html(n_html));
    });
    //Actualiza la lib.multipleselect
    element.multipleSelect('refresh');
}

function SetMessage(container, msg, tpo, del) {//{Contenedor, mensaje(string), tipoMensaje(string), delay(bool)}
    var new_class = "msg-" + tpo;//Identifica la nueva clase css
    $(container).removeClass($(container).attr('class'));//Quita la clase css previa
    $(container).addClass(new_class);
    $(container).html(msg);//Carga el mensaje
    //$(container).show();

    //Focus to message
    $('html, body').animate({ scrollTop: 0 }, 5);

    //Mensaje con delay o no
    if (del) $(container).show(0).delay(5000).hide(0);//5 Segundos
    else $(container).show();
}