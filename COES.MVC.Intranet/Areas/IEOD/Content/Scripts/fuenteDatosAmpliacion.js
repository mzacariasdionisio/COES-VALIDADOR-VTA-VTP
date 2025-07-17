var controlador = siteRoot + 'IEOD/Configuracion/';

$(function () {
    $('#cbFuenteDatos').change(function () {
        cargarEmpresas();
    });
    $('#FechaDesde').Zebra_DatePicker({
        onSelect: function () {
            $('#listado').html("");
        }
    });
    $('#FechaHasta').Zebra_DatePicker({
        onSelect: function () {
            $('#listado').html("");
        }
    });

    $('#btnAmpliar').click(function () {
        agregarAmpliacion();
    });
    $('#btnConsultar').click(function () {
        if ($('#cbFuenteDatos').val() > 0) {
            mostrarListado();
        }
    });

    cargarEmpresas();
    mostrarListado();
});

function mostrarListado() {
    $('#listado').html("");

    var empresa = $('#cbEmpresa').multipleSelect('getSelects');
    if (empresa == "[object Object]") empresa = "-1";
    if (empresa == "") empresa = "-1";
    var fuendatos = $('#cbFuenteDatos').val();
    $('#hfEmpresa').val(empresa);

    $.ajax({
        type: 'POST',
        url: controlador + "FuenteDatosAmpliacionLista",
        data: {
            sEmpresa: $('#hfEmpresa').val(),
            fuendatos: fuendatos,
            sfechaIni: $('#FechaDesde').val(),
            sfechaFin: $('#FechaHasta').val()
        },
        success: function (evt) {
            $('#listado').css("width", $('#mainLayout').width() + "px");
            $('#listado').html(evt);
            $('#tabla').dataTable({
                scrollY: 600,
                scrollX: true,
                scrollCollapse: true,
                paging: false,
                fixedColumns: true
            });
            $("#tabla_wrapper").css("width", "100%");
        },
        error: function () {
            alert("Ha ocurrido un error");
        }
    });
}

function agregarAmpliacion() {

    $.ajax({
        type: 'POST',
        url: controlador + "FuenteDatosAmpliacionNuevo",
        data: {
        },
        success: function (evt) {

            $('#detalleAmpliacion').html(evt);

            setTimeout(function () {
                $('#validaciones').bPopup({
                    easing: 'easeOutBack',
                    speed: 450,
                    transition: 'slideDown',
                    modalClose: false
                });
            }, 50);
        },
        error: function () {
            alert("Error en mostrar periodo");
        }
    });
}

function cargarEmpresas() {
    var fuendatos = $('#cbFuenteDatos').val();
    $.ajax({
        type: 'POST',
        url: controlador + 'FuenteDatosAmpliacionCargarEmpresas',

        data: { fuendatos: fuendatos },

        success: function (aData) {
            $('#empresas').html(aData);

            $('#cbEmpresa').multipleSelect({
                width: '250px',
                filter: true
            });

            $('#cbEmpresa').multipleSelect('checkAll');

            mostrarListado();
        },
        error: function () {
            alert("Ha ocurrido un error");
        }
    });
}

function cargarEmpresasPopUp() {
    var fuendatos = $('#cbFuenteDatos2').val();
    $.ajax({
        type: 'POST',
        url: controlador + 'FuenteDatosAmpliacionCargarEmpresasPopup',

        data: { fuendatos: fuendatos },

        success: function (aData) {
            $('#empresasPopUp').html(aData);
        },
        error: function (err) {
            alert("Ha ocurrido un error");
        }
    });
}

function grabarAmpliacion() {
    var hora = $('#cbHora').val();
    var fdatcodi = $("#cbFuenteDatos2").val();
    var empresa = $('#cbEmpresa2').val();
    if (empresa == 0) {
        alert("Seleccionar Empresa");
    }
    else {
        $.ajax({
            type: 'POST',
            url: controlador + "FuenteDatosAmpliacionGrabar",
            dataType: 'json',
            data: {
                sfechaPeriodo: $('#idFechaEnvio').val(),
                hora: hora,
                empresa: empresa,
                fdatcodi: fdatcodi,
                sfechaAmpl: $('#idFechaAmp').val(),
            },
            success: function (evt) {
                if (evt.Resultado == "1") {
                    $('#validaciones').bPopup().close();
                    mostrarListado();
                }
                else {
                    alert("Error en Grabar Ampliación: " + evt.Mensaje);
                }
            },
            error: function (err) {
                alert("Error en Grabar Ampliación");
            }
        });
    }
}