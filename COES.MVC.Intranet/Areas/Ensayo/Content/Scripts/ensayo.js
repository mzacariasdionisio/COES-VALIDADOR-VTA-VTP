var controlador = siteRoot + 'Ensayo/';

$(function () {
    $('#FechaDesde').Zebra_DatePicker({
    });
    $('#FechaHasta').Zebra_DatePicker({
    });

    $('#cbEmpresa').multipleSelect({
        width: '250px',
        filter: true,
        onClose: function (view) {
            cargarCentrales();
        }
    });
    $('#cbCentral').multipleSelect({
        width: '250px',
        filter: true
    });
    $('#cbEstado').multipleSelect({
        width: '250px',
        filter: true
    });
    $('#btnExcel').click(function () {
        exportarExcel(0);
    });

    $('#btnBuscar').click(function () {
        buscarEnsayo(1);
    });

    $(window).resize(function () {
        $('#listado').css("width", $('#mainLayout').width() + "px");
    });
    $('#btnNuevo').click(function () {
        formatoNuevoEnsayo();
    });
    buscarEnsayo(1); // muestra el listado de todos los ensayos de todas las empresas GEN
    valoresIniciales()
    cargarCentrales();
});

function exportarExcel(nroPagina)  ///para exportar arhivos excel PENDIENTE
{
    var empresa = $('#cbEmpresa').multipleSelect('getSelects');
    var estado = $('#cbEstado').multipleSelect('getSelects');
    var central = $('#cbCentral').multipleSelect('getSelects');
    var finicio = $('#FechaDesde').val();
    var ffin = $('#FechaHasta').val();
    if (empresa == "[object Object]") empresa = "-1";
    $('#hfEmpresa').val(empresa);
    $('#hfEstado').val(estado);
    $('#hfCentral').val(central);
    $.ajax({
        type: 'POST',
        url: controlador + 'genera/GenerarArchivoReporteXls',
        data: {
            empresas: $('#hfEmpresa').val(), estados: $('#hfEstado').val(),
            centrales: $('#hfCentral').val(), finicios: finicio,
            ffins: ffin
        },
        dataType: 'json',
        success: function (result) {
            if (result == 1) {
                window.location = controlador + "genera/ExportarReporte";
            }
            if (result == -1) {
                alert("Ha ocurrido un error al generar reporte excel");
            }
        },
        error: function () {
            alert("Ha ocurrido un error al generar arhivo excel");
        }
    });
}

function valoresIniciales() {
    $('#cbEmpresa').multipleSelect('checkAll');
    $('#cbCentral').multipleSelect('checkAll');
    $('#cbEstado').multipleSelect('checkAll');


}

function cargarCentrales() {
    var empresa = $('#cbEmpresa').multipleSelect('getSelects');

    if (empresa == "[object Object]") empresa = "-1";
    $('#hfEmpresa').val(empresa);
    $.ajax({
        type: 'POST',
        url: controlador + 'genera/CargarCentrales',

        data: { idEmpresa: $('#hfEmpresa').val() },

        success: function (aData) {

            $('#centrales').html(aData);
        },
        error: function () {
            alert("Ha ocurrido un error");
        }
    });
}

function buscarEnsayo(idEstado) {
    $('#hfEstado').val(idEstado);
    pintarPaginado()

    mostrarListado(1);
    //mostrarListado(1, idEstado);
}

function pintarPaginado() {
    var empresa = $('#cbEmpresa').multipleSelect('getSelects');
    var estado = "1";
    var central = $('#cbCentral').multipleSelect('getSelects');
    var finicio = $('#FechaDesde').val();
    var ffin = $('#FechaHasta').val();

    if (empresa == "[object Object]") empresa = "-1";
    if (empresa == "") empresa = "-1";
    if (central == "[object Object]") central = "-1";
    $('#hfEmpresa').val(empresa);
    //$('#hfEstado').val(estado);
    $('#hfCentral').val(central);
    $.ajax({
        type: 'POST',
        url: controlador + "genera/paginado",
        data: {
            empresas: $('#hfEmpresa').val(), estados: "1",
            centrales: $('#hfCentral').val(), finicios: finicio,
            ffins: ffin
        },
        success: function (evt) {
            $('#paginado').html(evt);
            mostrarPaginado();
        },
        error: function () {
            alert("Ha ocurrido un error paginado");
        }
    });
}

function pintarBusqueda(nroPagina) {
    mostrarListado(nroPagina);
}

function mostrarListado(nroPagina) {
    //function mostrarListado(nroPagina, idEstado) {
    $('#hfNroPagina').val(nroPagina);
    var empresa = $('#cbEmpresa').multipleSelect('getSelects');
    var central = $('#cbCentral').multipleSelect('getSelects');
    var finicio = $('#FechaDesde').val();
    var ffin = $('#FechaHasta').val();
    if (empresa == "[object Object]") empresa = "-1";
    if (empresa == "") empresa = "-1";
    if (central == "[object Object]") central = "-1";
    if (central == "") central = "-1";
    $('#hfEmpresa').val(empresa);
    $('#hfCentral').val(central);
    $.ajax({
        type: 'POST',
        url: controlador + "genera/ListaEnsayo",
        data: {
            empresas: $('#hfEmpresa').val(), centrales: $('#hfCentral').val(), nroPaginas: nroPagina,
            estado: parseInt($('#hfEstado').val()) || 0,
            //estado: idEstado,
            finicios: finicio, ffins: ffin
        },
        success: function (evt) {

            $('#listado').css("width", $('#mainLayout').width() + "px");
            $('#listado').html(evt);
        },
        error: function () {
            alert("Ha ocurrido un error");
        }
    });
}

function formatoNuevoEnsayo() {
    window.location.href = controlador + "genera/Nuevo?id=0";
}

function mostrarDetalleEnsayo(id) {
    location.href = controlador + "genera/Nuevo?id=" + id;
}

function mostrarEnsayoFormato(id) {
    // id: "Código del ensayo que pasa como parametro"
    location.href = controlador + "genera/EnvioFormato?id=" + id;
}

function mostrarEnsayoUnidades(id) {
    // id: "Código del ensayo que pasa como parametro"
    location.href = controlador + "genera/Nuevo?id=" + id;
}

function editarFormatoEnsayo(id) {
    location.href = controlador + "genera/EditarFormato?id=" + id;
}


function autorizarEnsayo(idEnsayo) {
    $.ajax({
        type: 'POST',
        url: controlador + "genera/GrabarAutorizaEnsayo",
        dataType: 'json',

        data: { iensayocodi: idEnsayo },

        success: function (evt) {
            $('#popupAutorizarEnsayo').bPopup().close();
            if (evt.Ensayoaprob == 1) {
                alert("El ensayo ha sido Aprobado!");
            }
            mostrarListado(1, 1);
        },
        error: function () {
            alert("Ha ocurrido un error en Autorizar ensayo");
        }
    });

}
function popupAutorizarEnsayo(idEnsayo) {

    $('#hEnsayoCodi').val(idEnsayo);
    $.ajax({
        type: 'POST',
        url: controlador + "genera/AutorizaEnsayo",
        data: { iensayocodi: idEnsayo },
        success: function (evt) {
            $('#AutorizarEnsayo').html(evt);
            setTimeout(function () {
                $('#popupAutorizarEnsayo').bPopup({
                    easing: 'easeOutBack',
                    speed: 450,
                    transition: 'slideDown'
                });
            }, 50);
        },
        error: function () {
            alert("Ha ocurrido un error en Autorizar Ensayo");
        }
    });
}

function cancelarAutorizarEnsayo() {
    $('#popupAutorizarEnsayo').bPopup().close();
}
