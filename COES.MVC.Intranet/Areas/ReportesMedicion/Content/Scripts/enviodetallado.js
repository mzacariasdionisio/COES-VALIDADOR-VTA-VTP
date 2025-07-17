var controlador = siteRoot + 'hidrologia/';
$(function () {

    $('#FechaDesde').Zebra_DatePicker({

    });

    $('#FechaHasta').Zebra_DatePicker({

    });

    $('#cbLectura').change(function () {
        listarFormato();
    });

    $('#btnBuscar').click(function () {
        buscarDatos();
    });
    cargarPrevio();
});
function cargarPrevio() {

}

function buscarDatos() {
    //pintarPaginado(1)  
    //mostrarListado(1);
}

function pintarPaginado(id) {
    var empresa = $('#cbEmpresa').multipleSelect('getSelects');
    var cuenca = $('#cbCuenca').multipleSelect('getSelects');

    if (empresa == "[object Object]") empresa = "-1";
    if (empresa == "") empresa = "-1";
    if (cuenca == "[object Object]") cuenca = "-1";
    if (cuenca == "") cuenca = "-1";

    $('#hfEmpresa').val(empresa);
    $('#hfCuenca').val(cuenca);

    var tipoInformacion = $('#cbTipoInformacion').val();

    $.ajax({
        type: 'POST',
        url: controlador + "reporte/paginado",
        data: {
            idsEmpresa: $('#hfEmpresa').val(), idsCuenca: $('#hfCuenca').val(), idTipoInformacion: tipoInformacion,
            fechaInicial: $('#FechaDesde').val(), fechaFinal: $('#FechaHasta').val()
        },
        success: function (evt) {
            $('#paginado').html(evt);
            mostrarPaginado(id);
        },
        error: function () {
            alert("Ha ocurrido un error paginado");
        }
    });
}

function mostrarListado(nroPagina) {
    var empresa = $('#cbEmpresa').multipleSelect('getSelects');
    var cuenca = $('#cbCuenca').multipleSelect('getSelects');

    if (empresa == "[object Object]") empresa = "-1";
    if (empresa == "") empresa = "-1";
    if (cuenca == "[object Object]") cuenca = "-1";
    if (cuenca == "") cuenca = "-1";

    $('#hfEmpresa').val(empresa);
    $('#hfCuenca').val(cuenca);
    var tipoInformacion = $('#cbTipoInformacion').val();

    $.ajax({
        type: 'POST',
        url: controlador + "reporte/lista",
        data: {
            idsEmpresa: $('#hfEmpresa').val(), idsCuenca: $('#hfCuenca').val(), idTipoInformacion: tipoInformacion,
            fechaInicial: $('#FechaDesde').val(), fechaFinal: $('#FechaHasta').val(),
            nroPagina: nroPagina
        },
        success: function (evt) {
            $('#listado').css("width", $('#mainLayout').width() + "px");

            $('#listado').html(evt);
            if ($('#tabla th').length > 1) {
                $('#tabla').dataTable({
                    "aoColumns": aoColumns(),
                    "bSort": false,
                    "scrollY": 430,
                    "scrollX": true,
                    "sDom": 't',
                    "iDisplayLength": 50
                });
            }
        },
        error: function () {
            alert("Ha ocurrido un error");
        }
    });
}

function listarFormato() {
    var empresa = $('#cbEmpresa').val();
    var lectura = $('#cbLectura').val();

    if (empresa == "" || empresa == null) empresa = -1;
    if (lectura == "" || lectura == null) lectura = -1;

    $('#hfEmpresa').val(empresa);
    $('#hfLectura').val(lectura);
    $.ajax({
        type: 'POST',
        url: controlador + "EnvioDetallado/ListarFormatosXLectura",

        data: {
            idLectura: $('#hfLectura').val(), idEmpresa: $('#hfEmpresa').val()
        },
        success: function (evt) {
            $('#listTipoInformacion').html(evt);
        },
        error: function () {
            alert("Ha ocurrido un error");
        }
    });

}