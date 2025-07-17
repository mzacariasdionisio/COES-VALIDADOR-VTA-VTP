var controlador = siteRoot + 'IEOD/'
$(function () {

    $('#cbFamiliaPot').val($('#hfFamilia').val());
    $('#cbEquipo').val($('#hfEquipo').val());


    $('#cbEmpresa2').change(function () {
        CargarFamilias();
        cargarEquipos();
        CargarAreas();

    });

    $('#cbFamilia2').on('change', function () {
        empresa = $('#cbEmpresa2').val();
        cargarEquipos();

    });

    $('#idAgregar').click(function () {
        grabarNuevoEquipo();/**/
    });


    $('#btnCancelar').click(function () {
        cancelar();
    });



});
function cancelar() {
    $('#popupUnidad').bPopup().close();
}
function CargarFamilias() {

    empresa = $('#cbEmpresa2').val();
    $.ajax({
        type: 'POST',
        url: controlador + 'EquiposSEIN/CargarFamilias',
        dataType: 'json',
        data: { idEmpresa: empresa },
        cache: false,
        global: false,
        success: function (aData) {
            $('#cbFamilia2').get(0).options.length = 0;
            $('#cbFamilia2').get(0).options[0] = new Option("--SELECCIONE--", "");
            $.each(aData, function (i, item) {
                $('#cbFamilia2').get(0).options[$('#cbFamilia2').get(0).options.length] = new Option(item.Text, item.Value);
            });
        },
        error: function () {
            alert("Ha ocurrido un error");
        }
    });
}
function cargarEquipos() {

    var familia = $('#cbFamilia2').val();

    empresa = $('#cbEmpresa2').val();


    $.ajax({
        type: 'POST',
        url: controlador + 'EquiposSEIN/CargarEquipos',
        dataType: 'json',
        data: { idEmpresa: empresa, idFamilia: familia },
        cache: false,
        global: false,
        success: function (aData) {
            $('#cbEquipo').get(0).options.length = 0;
            $('#cbEquipo').get(0).options[0] = new Option("--SELECCIONE--", "");
            $.each(aData, function (i, item) {
                $('#cbEquipo').get(0).options[$('#cbEquipo').get(0).options.length] = new Option(item.Text, item.Value);
            });
        },
        error: function () {
            alert("Ha ocurrido un error");
        }
    });
}

function CargarAreas() {

    var familia = $('#cbFamilia2').val();

    empresa = $('#cbEmpresa2').val();



    $.ajax({
        type: 'POST',
        url: controlador + 'EquiposSEIN/CargarAreas',
        dataType: 'json',
        data: { idEmpresa: empresa },
        cache: false,
        global: false,
        success: function (aData) {
            $('#cbUbicacion').get(0).options.length = 0;
            $('#cbUbicacion').get(0).options[0] = new Option("--SELECCIONE--", "");
            $.each(aData, function (i, item) {
                $('#cbUbicacion').get(0).options[$('#cbUbicacion').get(0).options.length] = new Option(item.Text, item.Value);
            });
        },
        error: function () {
            alert("Ha ocurrido un error");
        }
    });
}

function grabarNuevoEquipo() {

    var empresa = $('#cbEmpresa2').val();
    var familia = $("#cbFamilia2").val();
    var equipo = $("#cbEquipo").val();

    var motivo = $("#cbMotivo").val();
    var fecha = $('#idFechaEnvio').val();

    var ubicacion = $("#cbUbicacion").val();;
    if (ubicacion == "") { ubicacion = 0; }

    if (empresa == 0) {
        alert("Seleccionar la empresa");
        return;
    }
    if (familia == 0) {
        alert("Seleccionar Tipo de Empresa");
        return;
    }
    if (equipo == 0) {
        alert("Seleccionar Equipo");
        return;
    }
    if (motivo == 0) {
        alert("Seleccionar el motivo");
        return;
    }
    if (fecha == "") {
        alert("Ingresar una fecha");
        return;
    }

    $.ajax({
        type: 'POST',
        url: controlador + 'EquiposSEIN/GrabarEquipo',
        dataType: 'json',
        data: {
            idempresa: empresa, idfamilia: familia, idequipo: equipo,
            idmotivo: motivo, ifecha: fecha, idUbicacion: ubicacion
        },
        cache: false,
        success: function (resultado) {
            if (resultado == 1) {
                document.location.href = controlador + "EquiposSEIN/index";
            }
            else {
                alert("Error al grabar equipo");
            }

        },
        error: function () {
            alert("Error al grabar equipo");
        }
    });
}