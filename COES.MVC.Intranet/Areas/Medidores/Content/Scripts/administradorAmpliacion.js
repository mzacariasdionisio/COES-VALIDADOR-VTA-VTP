var controlador = siteRoot + 'Medidores/Ampliacion/';
var listFormatCodi = [];
var listFormatPeriodo = [];
$(function () {

    $('#cbFormato').change(function () {
        cargarEmpresas();
    });
    $('#mesIni').Zebra_DatePicker({
        format: 'm Y',
        onSelect: function () {

        }
    });

    $('#mesFin').Zebra_DatePicker({
        format: 'm Y',
        onSelect: function () {

        }
    });

    $('#btnAmpliar').click(function () {
        agregarAmpliacion();
    });

    $('#btnBuscar').click(function () {
        if ($('#cbFormato').val() > 0) {
            mostrarListado();
        }
        else {
            alert("Error: Seleccionar Formato");
        }
    });

    $('#btnExportar').click(function () {
        exportarExcel();
    });

    cargarEmpresas();
});

function mostrarListado() {
    var empresa = $('#cbEmpresa').multipleSelect('getSelects');
    if (empresa == "[object Object]") empresa = "-1";
    if (empresa == "") empresa = "-1";
    var formato = $('#cbFormato').val();
    $('#hfEmpresa').val(empresa);
    $.ajax({
        type: 'POST',
        url: controlador + "lista",
        data: {
            sEmpresas: $('#hfEmpresa').val(),
            idformato: formato,
            mesIni: $('#mesIni').val(),
            mesFin: $('#mesFin').val()
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
        url: controlador + "AgregarAmpliacion",
        data: {
            periodoIni: 0
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
    var formato = $('#cbFormato').val();
    $.ajax({
        type: 'POST',
        url: controlador + 'CargarEmpresas',

        data: { idFormato: formato },

        success: function (aData) {
            $('#empresas').html(aData);
        },
        error: function () {
            alert("Ha ocurrido un error");
        }
    });
}

function cargarEmpresasPopUp() {
    var formato = $('#cbFormato2').val();
    $.ajax({
        type: 'POST',
        url: controlador + 'CargarEmpresasPopUp',

        data: { idFormato: formato },

        success: function (aData) {
            $('#empresasPopUp').html(aData);
        },
        error: function () {
            alert("Ha ocurrido un error");
        }
    });
}

function grabarAmpliacion()
{
    var error = false;
    var hora = $('#cbHora').val();
    formato = $("#cbFormato2").val();
    var empresa = $('#empresasPopUp select').val();
    if (empresa == 0) {
        alert("Seleccionar Empresa");
    }
    else {
        $('#validaciones').bPopup().close();
        $.ajax({
            type: 'POST',
            url: controlador + "GrabarAmpliacion",
            dataType: 'json',
            data: {
                fecha: $('#idFechaEnvio').val(), hora: hora, empresa: empresa,
                idformato: formato
            },
            success: function (evt) {
                if (evt == 1) {
                    mostrarListado();
                }
                else {
                    alert("Error en Grabar Ampliación");
                }
            },
            error: function () {
                alert("Error en Grabar Ampliación");
            }
        });
    }
}