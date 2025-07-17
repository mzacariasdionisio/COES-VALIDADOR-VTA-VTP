var controlador = siteRoot + 'TiempoReal/CargaArchxmlSp7/';
var ANCHO_LISTADO_EMS = 1200;

$(document).ready(function () {
    $('#txtFechaInicial').Zebra_DatePicker({
    });

    $('#txtFechaFinal').Zebra_DatePicker({
    });

    $('#btnExportarExcel').click(function () {
        exportarExcelCargaXML();
    });

    buscarSubidaArchivo();
});

$(function () {
    $('#btnConsultar').click(function () {
        buscarSubidaArchivo();
    });

    ANCHO_LISTADO_EMS = ANCHO_LISTADO_EMS > 1200 ? ANCHO_LISTADO_EMS : 1200;
    $(window).resize(function () {
        $('#listado').css("width", ANCHO_LISTADO_EMS + "px");
    });
});

buscarSubidaArchivo = function () {
    $('#mensaje').css("display", "none");
    mostrarListado();
};
mostrarExitoOperacion = function () {
    $('#mensaje').removeClass("action-error");
    $('#mensaje').addClass("action-exito");
    $('#mensaje').text("La operación se realizó con exito...!");
    $('#mensaje').css("display", "block");
};
mostrarError = function () {
    $('#mensaje').removeClass("action-exito");
    $('#mensaje').addClass("action-error");
    $('#mensaje').text("Ha ocurrido un error");
    $('#mensaje').css("display", "block");
};
ocultarMensaje = function () {
    $('#mensaje').css("display", "none");
};
mostrarListado = function (nroPagina) {
    $.ajax({
        type: 'POST',
        url: controlador + "ListadoCargaArchivo",
        data: $('#frmBusquedaXML').serialize(),
        success: function (evt) {
            //$('#listado').css("width", $('#mainLayout').width() + "px");

            $('#listado').html(evt);
            $('#tabla').dataTable({
                "scrollY": 430,
                "scrollX": false,
                "sDom": 'ft',
                "ordering": false,
                "iDisplayLength": -1
            });
        },
        error: function () {
            mostrarError();
        }
    });
};

function exportarExcelCargaXML() {
        $.ajax({
            type: 'POST',
            url: controlador + 'ExportarCargaArchivo',
            dataType: 'json',
            data: $('#frmBusquedaXML').serialize(),
            success: function (result) {
                if (result.Resultado != "-1") {
                    var paramList = [
                        { tipo: 'input', nombre: 'file1', value: result.NombreArchivoTmp },
                        { tipo: 'input', nombre: 'file2', value: result.NombreArchivo }
                    ];
                    var form = CreateForm(controlador + 'AbrirArchivoReporte', paramList);
                    document.body.appendChild(form);
                    form.submit();
                } else {
                    alert(result.Mensaje);
                }
            },
            error: function (err) {
                alert('Lo sentimos no se puede mostrar la consulta . *Revise que el rango de fechas no debe de sobrepasar el año')
            }
        });
}

function CreateForm(path, params, method = 'post') {
    var form = document.createElement('form');
    form.method = method;
    form.action = path;

    $.each(params, function (index, obj) {
        var hiddenInput = document.createElement(obj.tipo);
        hiddenInput.type = 'hidden';
        hiddenInput.name = obj.nombre;
        hiddenInput.value = obj.value;
        form.appendChild(hiddenInput);
    });
    return form;
}