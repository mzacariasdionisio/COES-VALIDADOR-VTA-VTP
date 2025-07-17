var controlador = siteRoot + 'IEOD/EquiposSEIN/';

$(function () {
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
    $('#cbEmpresa').multipleSelect('checkAll');
    $('#cbFamilia').multipleSelect('checkAll');
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
        error: function (err) {
            alert("Ha ocurrido un error");
        }
    });
}

function agregarEquipo() {
    $.ajax({
        type: 'POST',
        url: controlador + "IngresoEquipoSEIN",
        data: {
        },
        success: function (evt) {
            $('#detalleSEIN').html(evt);

            cargarPopup();

            setTimeout(function () {
                $('#validaciones').bPopup({
                    easing: 'easeOutBack',
                    speed: 450,
                    transition: 'slideDown',
                    modalClose: false
                });
            }, 50);
        },
        error: function (err) {
            alert("Error en mostrar");
        }
    });
}

function cargarPopup() {
    $('#idFechaEnvio').unbind();
    $('#idFechaEnvio').inputmask({
        mask: "1/2/y h:s:s",
        placeholder: "dd/mm/yyyy hh:mm:ss",
        alias: "datetime",
        hourFormat: "24"
    });

    $('#idFechaEnvio').Zebra_DatePicker({
        readonly_element: false,
        onSelect: function (date) {
            $('#txtHoraInicial').val(date + " 00:00:00");
        }
    });

    $('#cbEmpresa2').unbind();
    $('#cbEmpresa2').change(function () {
        CargarFamilias();
        cargarEquipos();
        CargarAreas();
    });

    $('#cbFamilia2').unbind();
    $('#cbFamilia2').on('change', function () {
        empresa = $('#cbEmpresa2').val();
        cargarEquipos();

    });

    $("#trNeoUbicacion").hide();
    $('#cbMotivo').unbind();
    $('#cbMotivo').change(function () {
        var motiv = parseInt($('#cbMotivo').val()) || 0;
        if (motiv == 350) {
            $("#trNeoUbicacion").show();
        }
    });

    $('#idAgregar').unbind();
    $('#idAgregar').click(function () {
        grabarNuevoEquipo();/**/
    });

    $('#idCancelar').unbind();
    $('#idCancelar').click(function () {
        cancelar();
    });
}

function cancelar() {
    $('#validaciones').bPopup().close();
}

function CargarFamilias() {

    empresa = $('#cbEmpresa2').val();
    $.ajax({
        type: 'POST',
        url: controlador + 'CargarFamilias',
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
        error: function (err) {
            alert("Ha ocurrido un error");
        }
    });
}

function cargarEquipos() {
    var familia = $('#cbFamilia2').val();
    empresa = $('#cbEmpresa2').val();

    $.ajax({
        type: 'POST',
        url: controlador + 'CargarEquipos',
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
        error: function (err) {
            alert("Ha ocurrido un error");
        }
    });
}

function CargarAreas() {
    empresa = $('#cbEmpresa2').val();

    $.ajax({
        type: 'POST',
        url: controlador + 'CargarAreas',
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
        error: function (err) {
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
    else {
        if (!$('#idFechaEnvio').inputmask("isComplete")) {
            alert("Ingrese hora inicial");
            return;
        }
    }

    $.ajax({

        type: 'POST',
        url: controlador + 'GrabarEquipo',
        dataType: 'json',
        data: {
            idempresa: empresa, idfamilia: familia, idequipo: equipo,
            idmotivo: motivo, ifecha: fecha
        },
        cache: false,
        success: function (model) {
            if (model.Resultado != "1") {
                $("#idFecha").val(model.Resultado);
                mostrarListado();
                cancelar();
            }
            else {
                alert("Error al grabar equipo");
            }

        },
        error: function (err) {
            alert("Error al grabar equipo");
        }

    });

}