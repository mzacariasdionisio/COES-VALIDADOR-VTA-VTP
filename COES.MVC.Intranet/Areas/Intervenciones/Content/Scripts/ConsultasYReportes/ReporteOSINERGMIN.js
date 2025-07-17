var controlador = siteRoot + "intervenciones/consultasyreportes/";
var pagActualGlobal;

$(document).ready(function () {
    $('#btnConsultarProc257d').click(function () {        
        cargarListaHtml();
    });

    $('#btnGenerarExcelProc257d').click(function () {
        generarExcelOSINERGMINProc257d();
    });   
});

function cargarLista() {

    $.ajax({
        type: 'POST',
        url: controlador + "RptOSINERGMINProc257dListado",
        data: {},
        success: function (evt) {
            $('#listado7d').html(evt);
            $('#tabla7d').dataTable({               
                "ordering": false,
                "searching": false,
                "iDisplayLength": 25
            });
        },
        error: function (err) {
            alert("Lo sentimos, ha ocurrido un error inesperado");
        }
    });
}

function cargarListaHtml() {    

    $.ajax({
        url: controlador + "RptOSINERGMINProc257dListadoHtml",
        type: 'POST',
        data: { },
        success: function (data) {
            if (data.Error == undefined) {
                if (data[0] == "1") {
                    $('#listado7d').css("width", $('#mainLayout').width() + "px");
                    $('#listado7d').html(data[1]);
                    $('#tabla').dataTable({
                        "ordering": false,
                        "searching": false,
                        "iDisplayLength": 25
                    });
                } else if (data[0] == "-1") {
                    $('#listado').css("width", $('#mainLayout').width() + "px");
                    $('#listado').html(data[1]);
                    alert("¡No existen registros para consultar!");
                } else if (data[0] == "0") {
                    $('#listado').css("width", $('#mainLayout').width() + "px");
                    $('#listado').html(data[1]);
                }
            } else {
                alert("Ha ocurrido un error");
            }
        },
        error: function (err) {
            alert("Lo sentimos, ha ocurrido el siguiente error");
        }
    });
}

function generarExcelOSINERGMINProc257d() {
    
    $.ajax({
        type: 'POST',
        url: controlador + 'RptOSINERGMINProc257dExportarExcel',
        data: {},
        dataType: 'json',
        success: function (result) {
            if (result == -1) {
                alert("No se encuentra datos a exportar!")
            }
            else if (result != -1) {
                document.location.href = controlador + 'Descargar?file=' + result;                
            }
            else {
                alert("Ha ocurrido un error");
            }
        },
        error: function (err) {
            alert("Ha ocurrido un error");
        }
    });
}


