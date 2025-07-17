// scrips relacionados => "globales.js"
var controlador = siteRoot + 'IEOD/EquiposSEIN/';
var ruta = siteRoot + 'IEOD/EquiposSEIN/';

var listFormatCodi = [];
var listFormatPeriodo = [];

$(function () {
    $('#cbTipoEmpresa').change(function () {
        cargarEmpresas();
    });

    $('#cbEmpresa').multipleSelect({
        width: '200px',
        filter: true
    });
    $('#cbFamilia').multipleSelect({
        width: '200px',
        filter: true
    });

    $('#idFecha').Zebra_DatePicker({
    });
    $('#btnBuscar').click(function () {
        mostrarListado();
    });
    $('#btnAgregar').click(function () {
        agregarEquipo();
    });

    valoresIniciales();
    mostrarListado();/**/

    $(window).resize(function () {
        $('#listadoSEIN').css("width", $('#mainLayout').width() + "px");
    });
});

function valoresIniciales() {
    /* var strFormatCodi = $('#hfFormatCodi').val();
     var strFormatPeriodo = $('#hfFormatPeriodo').val();
     listFormatCodi = strFormatCodi.split(',');
     listFormatPeriodo = strFormatPeriodo.split(',');*/
    cargarEmpresas();
    $('#cbEmpresa').multipleSelect('checkAll');
    $('#cbFamilia').multipleSelect('checkAll');
    /* listarFormato();*/

}

function mostrarListado() {


    var empresa = $('#cbEmpresa').multipleSelect('getSelects');
    var Familia = $('#cbFamilia').multipleSelect('getSelects');
    if (empresa == "[object Object]") empresa = "-1";
    if (empresa == "") empresa = "-1";
    if (Familia == "[object Object]") formato = "-1";
    if (Familia == "") Familia = "-1";
    $('#hfEmpresa').val(empresa);
    $('#hfFamilia').val(Familia);
    $.ajax({
        type: 'POST',
        url: controlador + "Lista",
        data: {
            sEmpresa: $('#hfEmpresa').val(), fecha: $('#idFecha').val(), sFamilia: $('#hfFamilia').val(),
            nroPagina: '1', orden: '1'
        },

        success: function (evt) {
            $('#listadoSEIN').css("width", $('#mainLayout').width() + "px");
            $('#listadoSEIN').html(evt);
            $('#tablaSEIN').dataTable({
                "scrollY": 430,
                "scrollX": true,
                "sDom": 't',
                "ordering": true,
                "iDisplayLength": 50
            });
        },
        error: function () {
            alert("Ha ocurrido un error");
        }
    });
}

function agregarEquipo() {


    $.ajax({
        type: 'POST',
        url: ruta + "IngresoEquipoSEIN",
        data: {
            idModulo: $('#hfIdModulo').val()
        },
        success: function (evt) {

            $('#detalleSEIN').html(evt);

            setTimeout(function () {
                $('#validaciones').bPopup({
                    easing: 'easeOutBack',
                    speed: 450,
                    transition: 'slideDown',
                    modalClose: false
                });
            }, 50);
        },
        error: function () {
            alert("Error en mostrar");
        }
    });
}

/*
function ocultarColumnas() {

    var oTable = $('#tabla').dataTable();

    var bVis9 = oTable.fnSettings().aoColumns[9].bVisible;
    oTable.fnSetColumnVis(9, bVis9 ? false : true);

    var bVis10 = oTable.fnSettings().aoColumns[10].bVisible;
    oTable.fnSetColumnVis(10, bVis10 ? false : true);

    var bVis11 = oTable.fnSettings().aoColumns[11].bVisible;
    oTable.fnSetColumnVis(11, bVis11 ? false : true);
}
*/

function cargarEmpresas() {
    var tipoempresa = $('#cbTipoEmpresa').val();
    $.ajax({
        type: 'POST',
        url: controlador + 'CargarEmpresas',
        data: { idTipoempresa: tipoempresa },

        success: function (aData) {
            $('#empresas').html(aData);
        },
        error: function () {
            alert("Ha ocurrido un error");
        }
    });
}