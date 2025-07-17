var sControlador = siteRoot + 'AporteIntegrantes/IngresoTransmision/';

$(document).ready(function () {

    $('#btnConsultar').click(function () {
        buscar();
    });
    $('#btnRegistrarEjecutado').click(function () {
        cargarEjecutadoPoyectado('E'); //Ejecutado
    });
    $('#btnRegistrarProyectado').click(function () {
        cargarEjecutadoPoyectado('P'); //Proyectado
    });
    $('#btnImportarEjecutadoPoyectado').click(function () {
        importarEjecutadoPoyectado();
    });
});

buscar = function () {
    var caiajcodi = document.getElementById("caiajcodi").value;
    var emprcodi = document.getElementById("emprcodi").value;
    console.log(emprcodi);
    if (emprcodi == null || emprcodi == "")
    {
        alert("Por favor, seleccionar una empresa");
    }
    else {
        $.ajax({
            type: 'POST',
            url: sControlador + "Lista",
            data: { caiajcodi: caiajcodi, emprcodi: emprcodi },
            success: function (evt) {
                $('#listado').html(evt);
                oTable = $('#tabla').dataTable({
                    "sPaginationType": "full_numbers",
                    "destroy": "true",
                    "aaSorting": [[0, "asc"], [1, "asc"]]
                });
            },
            error: function () {
                alert("Lo sentimos, se ha producido un error");
            }
        });
    }
};

cargarEjecutadoPoyectado = function (sIntervalo) {
    var caiprscodi = document.getElementById('caiprscodi').value;
    var caiajcodi = document.getElementById('caiajcodi').value;
    var emprcodi = document.getElementById('emprcodi').value;
    if (emprcodi == null || emprcodi == "") {
        alert("Por favor, seleccionar una empresa");
    }
    else{
        window.location.href = sControlador + "envio?caiprscodi=" + caiprscodi + "&caiajcodi=" + caiajcodi + "&emprcodi=" + emprcodi + "&intervalo=" + sIntervalo;
    }
}

importarEjecutadoPoyectado = function () {
    var caiprscodi = document.getElementById("caiprscodi").value;
    var caiajcodi = document.getElementById("caiajcodi").value;
    if (caiajcodi == "") {
        mostrarError("Por favor, seleccionar una versión de recalculo");
    }
    else {
        $.ajax({
            type: 'POST',
            url: sControlador + 'importarEjecutadoPoyectado',
            data: { caiprscodi: caiprscodi, caiajcodi: caiajcodi },
            dataType: 'json',
            success: function (model) {
                if (model.sError == "") {
                    mostrarExito("Felicidades, se copio correctamente toda la información Ejecutada y Proyectada.");
                }
                else {
                    mostrarError("Lo sentimos, ha ocurrido el siguiente error: " + model.sError);
                }
            },
            error: function () {
                mostrarError('Lo sentimos ha ocurrido un error inesperado');
            }
        });
    }
};

refrescar = function () {
    var caiprscodi = document.getElementById('caiprscodi').value;
    var caiajcodi = document.getElementById('caiajcodi').value;
    var emprcodi = document.getElementById('emprcodi').value;
    if (emprcodi == null || emprcodi == "") {
        alert("Por favor, seleccionar una empresa");
    }
    else {
        window.location.href = sControlador + "index?caiprscodi=" + caiprscodi + "&caiajcodi=" +caiajcodi + "&emprcodi=" + emprcodi;
    }
}