var name = "Bitacora/";
var controller = siteRoot + "PronosticoDemanda/" + name;

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

    $('.f-select-m').each(function () {
        var element = this;
        $(element).multipleSelect({
            name: element.id,
            filter: true,
            single: false,
            placeholder: 'Seleccione',
            selectAll: false,            
        });
    });
    
    $('#btnConsultar').on('click', function () {               
        Consultar();
    });

    $('#btnExportar').on('click', function () {
        var myTable = $("#dtDetalle").DataTable();
        var form_data = myTable.rows().data();
        var datos = [];
        for (var i = 0; i < form_data.length; i++) {
            var fila = [];
            fila.push(form_data[i].area);
            fila.push(form_data[i].empresa);
            fila.push(form_data[i].fechaInicio);
            fila.push(form_data[i].horaInicio);
            fila.push(form_data[i].fechaFin);
            fila.push(form_data[i].horaFin);
            fila.push(form_data[i].consumoTipico);
            fila.push(form_data[i].consumoPrevisto);
            fila.push(form_data[i].potenciaDiferencial);
            fila.push(form_data[i].barra);
            fila.push(form_data[i].punto);
            fila.push(form_data[i].ocurrencia);

            datos.push(fila);
        }
        Exportar(datos);
    });

});

function List(fechaIni, fechaFin, lect, empr, tipreg) {
    $('#dtDetalle').html("");
    dt = $('#dtDetalle').DataTable({
        serverSide: true,
        ajax: {
            type: "POST",
            url: controller + 'BitacoraList',
            contentType: 'application/json; charset=utf-8',
            data: function (d) {
                d.fechaIni = fechaIni;
                d.fechaFin = fechaFin;
                d.lect = lect;
                d.empr = empr;
                d.tipreg = tipreg;
                return JSON.stringify(d);
            },
            datatype: 'json',
            traditional: true
        },
        columns: [
            { data: "area", title: "Area" },
            { data: "empresa", title: "Empresa" },
            { data: "fechaInicio", title: "F. Inicio" },
            { data: "horaInicio", title: "H. Inicio" },
            { data: "fechaFin", title: "F. Fin" },
            { data: "horaFin", title: "H. Fin" },
            { data: "consumoTipico", title: "C. Tipico" },
            { data: "consumoPrevisto", title: "C. Previsto" },
            { data: "potenciaDiferencial", title: "P. Diferencial" },
            { data: "barra", title: "Barra" },
            { data: "punto", title: "Punto" },
            { data: "ocurrencia", title: "Ocurrencia" },
        ],
        initComplete: function () {
            $('#dtDetalle').css('width', '100%');
            $('.dataTables_scrollHeadInner').css('width', '100%');
            $('.dataTables_scrollHeadInner table').css('width', '98.84%');
        },
        searching: false,
        bLengthChange: false,
        bSort: true,
        destroy: true,
        paging: true,
        pageLength: 40,
        info: false
    });
}

function Exportar(datos) {
    $.ajax({
        type: 'POST',
        url: controller + 'Exportar',
        contentType: 'application/json; charset=utf-8',
        data: JSON.stringify({
            form: datos
        }),
        datatype: 'json',
        traditional: true,
        success: function (result) {
            if (result != -1) {
                window.location = controller + 'abrirarchivo?formato=' + 1 + '&file=' + result;
                mostrarMensaje('mensaje', 'exito', "Felicidades, el archivo se descargo correctamente...!");
            }
            else {
                mostrarMensaje('mensaje', 'error', "Lo sentimos, ha ocurrido un error inesperado");
            }
        },
        error: function () {
            alert("Ha ocurrido un problema...");
        }
    });
}

function Consultar() {
    const fechaIni = $('#fechaIni').val();
    const fechaFin = $('#fechaFin').val();    
    const indicador = $("input:radio[name='rblTipoRegistro']:checked").val();

    if (indicador == "0") {
        List(fechaIni,
            fechaFin,
            $('#id-tpdemanda').val(),
            $('#id-tpempresa').val(),
            '0');
    }
    else if (indicador == "1") {
        List(fechaIni,
            fechaFin,
            $('#id-tpdemanda').val(),
            $('#id-tpempresa').val(),
            '1');
    }
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