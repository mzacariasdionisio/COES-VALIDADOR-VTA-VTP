//variables globales
var controlador = siteRoot + 'Migraciones/AnexoA/'

$(function () {
    $('#cbEmpresa').multipleSelect({
        width: '150px',
        filter: true,
        onClose: function (view) {
            cargarSubEstacion();
        }
    });

    cargarValoresIniciales();
});

function cargarValoresIniciales() {
    $('#cbEmpresa').multipleSelect('checkAll');
    cargarSubEstacion();
}

function cargarSubEstacion() {
    $.ajax({
        type: 'POST',
        url: controlador + 'CargarSubEstacionFlujoPotencia',
        data: { idEmpresa: getEmpresa() },
        success: function (aData) {
            $('#subestacion').html(aData);
            $('#cbSubEstacion').multipleSelect({
                width: '150px',
                filter: true
            });
            cargarLista();
        },
        error: function () {
            alert("Ha ocurrido un error");
        }
    });
}

function cargarLista() {
    $.ajax({
        type: 'POST',
        url: controlador + 'CargarListaPALineasTransmision',
        data: {
            idEmpresa: getEmpresa(),
            idSubEstacion: getSubestacion(),
            fechaIni: getFechaInicio(),
            fechaFin: getFechaFin()
        },
        success: function (aData) {
            //
            //if (getSubestacion().split(',').length > 10) {
            //    $('#listado').css("width", $('#mainLayout').width() + "px");
            //} else { $('#listado').css("width", (getSubestacion().split(',').length) + "90px"); }
            //$('#listado').html(aData.Resultado);
            //$("#tabla_wrapper").css("width", "100%");
            $('#listado').css("width", $('#mainLayout').width() + "px");
            $('#listado').html(aData.Resultado);
            $('#listado').css("overflow-x", "auto");
        },
        error: function () {
            alert("Ha ocurrido un error");
        }
    });
}