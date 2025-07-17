var controlador = siteRoot + 'ReportesMedicion/formatoreporte/';

$(function () {
    $("#cbArea").change(function () {
        buscarReporte();
    });
    $("#cbModulo").change(function () {
        buscarReporte();
    });

    $('#btnBuscar').click(function () {
        buscarReporte();
    });

    $("#btnManualUsuario").click(function () {
        window.location = controlador + 'DescargarManualUsuario';
    });

    $('#btnNuevo').click(function () {
        nuevoReporte(0);
    });

    $("#btnPtoCal").click(function () { window.location.href = siteRoot + 'ReportesMedicion/formatoreporte/IndexPtoCal'; })

    $('#listado').css("width", $('#mainLayout').width() - 30 + "px");

    buscarReporte();
});

function buscarReporte() {
    mostrarListado();
}

function mostrarListado() {
    $.ajax({
        type: 'POST',
        url: controlador + "ListaReporte",
        data: {
            idarea: $('#cbArea').val(),
            idmodulo: $("#cbModulo").val()
        },
        success: function (evt) {
            //$('#listado').css("width", $('#mainLayout').width() + "px");

            $('#listado').html(evt);
            $('#tabla').dataTable({
                bJQueryUI: true,
                "scrollY": 550,
                "scrollX": false,
                "sDom": 'ft',
                "ordering": true,
                "iDisplayLength": -1
            });
        },
        error: function () {
            alert("Ha ocurrido un error");
        }
    });
}

function nuevoReporte(id) {
    $.ajax({
        type: 'POST',
        url: controlador + "AgregarReporte?id=" + id,
        success: function (evt) {
            $('#edicionGrupo').html(evt);
            setTimeout(function () {
                $('#popupFormato').bPopup({
                    easing: 'easeOutBack',
                    speed: 450,
                    transition: 'slideDown'
                });
            }, 50);
            $('#alerta').css("display", "none");
            $('#mensaje').css("display", "none");
        },
        error: function () {
            alert("Error al mostrar nuevo reporte");
        }
    });
}

function mostrarDetalle(reporcodi) {
    location.href = controlador + "IndexDetalle?id=" + reporcodi;
}

function mostrarDetalleConsulta(reporcodi) {
    location.href = controlador + "IndexDetalle?id=" + reporcodi + "&esConsulta=" + 1;
}

function reporcodiValido(reporcodi) {
    $.ajax({
        type: 'POST',
        url: controlador + "ValidadReporcodi",
        dataType: 'json',

        data: { ireporcodi: reporcodi },

        success: function (evt) {

            if (evt.ReporcodiValido == "1") {
                location.href = controlador + "IndexVisualizacion?id=" + reporcodi;
            } else {
                if (evt.ReporcodiValido == "-1")
                    alert("Error : Tipo de Lectura No Definida");
                if (evt.ReporcodiValido == "0")
                    alert("Error : Tipo de Lectura No Válida");
            }

        },
        error: function () {
            alert("Error al verificar datos del reporte");
        }
    });

}

function visualizacionReporte(reporcodi) {
    reporcodiValido(reporcodi);
}