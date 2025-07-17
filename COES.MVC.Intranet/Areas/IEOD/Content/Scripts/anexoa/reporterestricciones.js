var ancho = 900;

$(function () {

    $('#tab-container').easytabs({
        animate: false
    });

    $('#cbEmpresa').multipleSelect({
        width: '150px',
        filter: true,
        onClose: function (view) {
            cargarUbicacion();
        }
    });

    ancho = $("#mainLayout").width() > 900 ? $("#mainLayout").width() - 30 : 900;

    cargarValoresIniciales();
});

function cargarValoresIniciales() {
    $('#cbEmpresa').multipleSelect('checkAll');
    cargarUbicacion();
}

function mostrarReporteByFiltros() {
    cargarLista(1);
}

function cargarUbicacion() {
    var idEmpresa = getEmpresa();

    $.ajax({
        type: 'POST',
        async: false,
        url: controlador + 'CargarUbicacion',
        data: { idEmpresa: idEmpresa },
        success: function (aData) {
            $('#cbTipoRecursos').html(aData);
            $('#cbUbicacion').multipleSelect({
                width: '150px',
                filter: true,
                onClose: function (view) {
                    mostrarReporteByFiltros();
                }
            });
            $('#cbUbicacion').multipleSelect('checkAll');
            mostrarReporteByFiltros();
        },
        error: function (err) {
            alert("Ha ocurrido un error");
        }
    });
}

function cargarEquipo() {
    var idEmpresa = getEmpresa();
    var idUbicacion = getUbicacion();

    validacionesxFiltro(1);

    if (resultFiltro) {
        $.ajax({
            type: 'POST',
            url: controlador + 'CargarEquipos',
            data: { idEmpresa: idEmpresa, idUbicacion: idUbicacion },
            success: function (aData) {
                $('#equipo').html(aData);
                $('#cbEquipo').multipleSelect({
                    width: '150px',
                    filter: true,
                    onClose: function (view) {
                        mostrarReporteByFiltros();
                    }
                });

                $('#cbEquipo').multipleSelect('checkAll');

                mostrarReporteByFiltros(1);
            },
            error: function (err) {
                alert("Ha ocurrido un error");
            }
        });
    }
}

function cargarLista() {
    var idEmpresa = getEmpresa();
    var idUbicacion = getUbicacion();
    var fechaInicio = getFechaInicio();
    var fechaFin = getFechaFin();

    $.ajax({
        type: 'POST',
        url: controlador + 'CargarListaRestriccionesYMantto',
        data: { idEmpresa: idEmpresa, idUbicacion: idUbicacion, fechaInicio: fechaInicio, fechaFin: fechaFin },
        success: function (aData) {
            $('.filtro_fecha_desc').html(aData[0].FiltroFechaDesc);

            $('#listado1').html(aData[0].Resultado);
            var anchoReporte = $('#tablaRestric').width();
            $("#listado1").css("width", (anchoReporte > ancho ? ancho - 20 : anchoReporte - 50) + "px");
            $('#tablaRestric').dataTable({
                scrollX: 900,
                "sPaginationType": "full_numbers",
                "destroy": "true",
                "bInfo": true,
                "ordering": false,
                "iDisplayLength": 25,
            });

            $('#listado2').html(aData[1].Resultado);
            var anchoReporte = $('#tablaMantto').width();
            $("#listado2").css("width", (anchoReporte > ancho ? ancho - 20 : anchoReporte - 50) + "px");
            $('#tablaMantto').dataTable({
                scrollX: 900,
                "sPaginationType": "full_numbers",
                "destroy": "true",
                "bInfo": true,
                "ordering": false,
                "iDisplayLength": 25,
            });
            $('#tablaMantto2').dataTable();
        },
        error: function (err) {
            alert("Ha ocurrido un error en cargar lista");
        }
    });
}

//validaciones de los filtros de busqueda
function validacionesxFiltro(valor) {
    idEmpresa = getEmpresa();
    idUbicacion = getUbicacion();

    var equipo = "-1";
    $('#hfEquipo').val(equipo);
    idEquipo = $('#hfEquipo').val();

    fechaInicio = getFechaInicio();
    fechaFin = getFechaFin();

    var arrayFiltro = arrayFiltro || [];

    if (valor == 1) {
        arrayFiltro.push({ id: idEmpresa, mensaje: "Seleccione la opcion Empresa" }, { id: idUbicacion, mensaje: "Seleccione la opcion Ubicación" });
    }
    else {
        arrayFiltro.push({ id: idEmpresa, mensaje: "Seleccione la opcion Empresa" }, { id: idUbicacion, mensaje: "Seleccione la opcion Ubicación" }/*, { id: idEquipo, mensaje: "Seleccione la opcion Equipo" }*/);
    }
    validarFiltros(arrayFiltro);
}