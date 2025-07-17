$(function () {
    $('#cbEmpresa').multipleSelect({
        width: '150px',
        filter: true,
        onClose: function (view) {
            cargarCentralxEmpresa();
            cargarListaCombustibleConsumidoUnidadTermoelectrica();
        }
    });
    $('#cbCentral').multipleSelect({
        width: '150px',
        filter: true,
        onClose: function (view) {
            cargarTipoCombustible();
            cargarListaCombustibleConsumidoUnidadTermoelectrica();
        }
    });

    ancho = $("#mainLayout").width() > 900 ? $("#mainLayout").width() - 30 : 900;

    cargarValoresIniciales();
});

function cargarValoresIniciales() {
    $('#cbCentral').multipleSelect('checkAll');
    $('#cbEmpresa').multipleSelect('checkAll');
    $('#cbTipoCombustibleXcentral').multipleSelect('checkAll');

    cargarCentralxEmpresa();
    cargarTipoCombustible();
}

function mostrarReporteByFiltros() {
    cargarListaCombustibleConsumidoUnidadTermoelectrica();
}

function cargarCentralxEmpresa() {
    var idEmpresa = getEmpresa();
    validacionesxFiltro(1);
    if (resultFiltro) {
        $.ajax({
            type: 'POST',
            url: controlador + 'CargarCentralxEmpresa',
            data: { idEmpresa: idEmpresa },
            success: function (aData) {
                $('#centrales').html(aData);
            },
            error: function (err) {
                alert("Ha ocurrido un error");
            }
        });
    }
}

function cargarTipoCombustible() {
    var idCentral = getCentral();

    validacionesxFiltro(1);
    if (resultFiltro) {
        $.ajax({
            type: 'POST',
            url: controlador + 'CargarTipoCombustibleXCentral',
            data: { idCentral: idCentral },
            success: function (aData) {
                $('#cbTipoCombustible').html(aData);
                $('#cbTipoCombustibleXcentral').multipleSelect({
                    width: '150px',
                    filter: true,
                    onClose: function (view) {
                        cargarListaCombustibleConsumidoUnidadTermoelectrica();
                    }
                });
                $('#cbTipoCombustibleXcentral').multipleSelect('checkAll');
                cargarListaCombustibleConsumidoUnidadTermoelectrica();
            },
            error: function (err) {
                alert("Ha ocurrido un error");
            }
        });
    }
}

function cargarListaCombustibleConsumidoUnidadTermoelectrica() {
    $("#listado").html('');

    var idEmpresa = getEmpresa();
    var fechaInicio = getFechaInicio();
    var fechaFin = getFechaFin();
    var idCentral = getCentral();
    var idTCombustibleXcentral = getTipoCombustibleXcentral();

    validacionesxFiltro(2);
    if (resultFiltro) {
        $.ajax({
            type: 'POST',
            url: controlador + 'ListarCombustiblesConsumidoUnidTermo',
            data: { idEmpresa: idEmpresa, idCentral: idCentral, fechaInicio: fechaInicio, fechaFin: fechaFin, tipoCombustible: idTCombustibleXcentral },
            success: function (aData) {
                $('.filtro_fecha_desc').html(aData.FiltroFechaDesc);
                $('#listado').html(aData.Resultado);

                var anchoReporte = $('#reporte').width();
                var scrollX = anchoReporte > ancho;
                $("#resultado").css("width", (scrollX ? ancho : anchoReporte) + 10 + "px");
                $('#reporte').dataTable({
                    "autoWidth": false,
                    "scrollX": scrollX,
                    "scrollCollapse": false,
                    "sDom": 't',
                    "ordering": false,
                    paging: false,
                    fixedColumns: {
                        leftColumns: 5
                    }
                });
            },
            error: function (err) {
                alert("Ha ocurrido un error");
            }
        });
    }
}

//validaciones de los filtros de busqueda
function validacionesxFiltro(valor) {
    var idEmpresa = getEmpresa();
    var idCentral = getCentral();
    var idTCombustibleXcentral = getTipoCombustibleXcentral();

    var arrayUbicacion = arrayUbicacion || [];

    if (valor == 1)
        arrayUbicacion.push({ id: idEmpresa, mensaje: "Seleccione la opcion Empresa" }); //, { id: idCentral, mensaje: "Seleccione la opcion Central" });
    else
        //if ((valor == 2)) {
        //    arrayUbicacion.push({ id: idEmpresa, mensaje: "Seleccione la opcion Empresa" }, { id: idCentral, mensaje: "Seleccione la opcion Central" });
        //}
        //else {
        arrayUbicacion.push({ id: idEmpresa, mensaje: "Seleccione la opcion Empresa" }, { id: idCentral, mensaje: "Seleccione la opcion Central" }, { id: idTCombustibleXcentral, mensaje: "Seleccione la opcion Tipo Combustible" });
    //}

    validarFiltros(arrayUbicacion);
}