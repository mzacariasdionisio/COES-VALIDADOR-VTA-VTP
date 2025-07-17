var controlador = siteRoot + 'Migraciones/AnexoA/'
$(function () {

    $('#cbEmpresa').multipleSelect({
        width: '150px',
        filter: true,
        onClose: function (view) {
            CargarCentral();
        }
    });

    cargarValoresIniciales();
});


function cargarValoresIniciales() {
    $('#cbEmpresa').multipleSelect('checkAll');
    CargarCentral();
}

function CargarCentral() {
    if (getEmpresa() != undefined) {
        $.ajax({
            type: 'POST',
            async: false,
            url: controlador + 'CargarCentralxEmpresa',
            data: {
                idEmpresa: getEmpresa()
            },
            success: function (aData) {
                $('#central').html(aData);
                $('#cbCentral').multipleSelect({
                    width: '150px',
                    filter: true,
                    onClose: function (view) {
                        CargarEquipos();
                    }
                });
                $('#cbCentral').multipleSelect('checkAll');
                CargarEquipos();
            },
            error: function () {
                alert("Ha ocurrido un error");
            }
        });
    } else {
        alert("Debe seleccionar al menos una empresa");
        $('#cbCentral').multipleSelect('checkAll');
    }
}

function CargarEquipos() {
    if (getEmpresa() != undefined) {
        $.ajax({
            type: 'POST',
            async: false,
            url: controlador + 'CargarEquiposXCentral',
            data: {
                idEmpresa: getEmpresa(),
                centrales: getCentral()
            },
            success: function (aData) {
                $('#equipos').html(aData);
                $('#cbEquipo').multipleSelect({
                    width: '150px',
                    filter: true,
                    onClose: function (view) {
                        cargarLista();
                    }
                });
                $('#cbEquipo').multipleSelect('checkAll');
                cargarLista();
            },
            error: function () {
                alert("Ha ocurrido un error");
            }
        });
    } else {
        alert("Debe seleccionar al menos un equipo");
        $('#cbEquipo').multipleSelect('checkAll');
    }
}


function cargarLista() {

    var fechaI = $('#txtFechaInicio').val();
    var fechaF = $('#txtFechaFin').val();

    $.ajax({
        type: 'POST',
        url: controlador + 'CargarHorasIndiponibilidad',
        data: { fechaInicio: fechaI, fechaFin: fechaF, empresas: getEmpresa(), equicodis: getEquipo() },
        success: function (aData) {
            $('#listado').html(aData.Resultado);
            $('#idCabeceraHorasIndiponibilidad').dataTable({
                "ordering": true,
                "paging": false,
                "dom": "ft",
                "scrollY": 352,
                "bDestroy": true,
                "info": false
            });
        },
        error: function () {
            alert("Ha ocurrido un error");
        }
    });

}


