var controlador = siteRoot + 'Admin/Estadistica/'
var opcion = 0;

$(function () {

    $('#txtFechaInicio').Zebra_DatePicker({
    });

    $('#txtFechaFin').Zebra_DatePicker({
    });

    $('#btnMostrar').click(function () {
        mostrarEstadisticaGeneral();
    });

    $('#btnExportar').click(function () {
        exportar(opcion);
    });

    cargarMenu();
    mostrarEstadisticaGeneral();
    
});
function mostrar() {
    var idEnvio = $("#ab").val();
    exportar(idEnvio);
}


exportar = function (id) {
    opcion = id;
    $.ajax({
        type: 'POST',
        url: controlador + "ExportarEstadistica",
        dataType: 'json',
        cache: false,
        data: {
            idOpcion: id,
            fechaIni: $('#txtFechaInicio').val(),
            fechaFin: $('#txtFechaFin').val()
        },
        success: function (resultado) {
            if (resultado == 1) {
                location.href = controlador + "DescargarEstadistica";
            }
            else {
                alert("NO");
            }
        },
        error: function () {
            mostrarError();
            ALERT("SI");
        }
    });
}

cargarMenu = function ()
{
    $.ajax({
        type: "POST",
        url: controlador + "tree",
        success: function (evt) {
            $('#tree').html(evt);
        },
        error: function (req, status, error) {
            alert("Ha ocurrido un error.");
        }
    });
}

cargarEstadisticaMenu = function (id)
{
    opcion = id;
    $.ajax({
        type: 'POST',
        url: controlador + "opcion",
        data: {
            idOpcion: opcion, fechaInicio: $('#txtFechaInicio').val(), fechaFin: $('#txtFechaFin').val()
        },
        success: function (evt) {
            $('#listado').html(evt);
            $('#tabla').dataTable({
                "iDisplayLength": 50
            });
        },
        error: function () {
            alert("Ha ocurrido un error");
        }
    });
}

mostrarEstadisticaGeneral = function ()
{   
    $.ajax({
        type: 'POST',
        url: controlador + "aplicacion",
        data: {
            fechaInicio: $('#txtFechaInicio').val(), fechaFin: $('#txtFechaFin').val()
        },
        success: function (evt) {      
            $('#listado').html(evt);
            $('#tabla').dataTable({               
                "iDisplayLength": 50
            });
        },
        error: function () {
            alert("Ha ocurrido un error");
        }
    });
}