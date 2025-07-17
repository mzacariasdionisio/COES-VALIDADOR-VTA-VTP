var controlador = siteRoot + 'yupana/'
$(function () {
    $('#cbVariable').on('change', function () {
        $('#hfOpcion').val(1);
        mostrarReporte();
    });
    $('#cbCosto').on('change', function () {
        $('#hfOpcion').val(0);
        mostrarReporte();
    });

    $('#cbFecha').on('change', function () {
        mostrarReporte();
    });

    $('#chbOrientacion').on('change', function () {
        mostrarReporte();
    });

    $('#btnEscoger').click(function () {
        openEscenario();
    });

    $('#cbTipoEscenario').on('change', function () {
        
    });

    $('#txtFechaInicio').Zebra_DatePicker({
        //direction: -1
    });

    $('#txtFechaFin').Zebra_DatePicker({
        //direction: -1
    });
  });


function mostrarMenu() {
    $.ajax({
        type: 'POST',
        url: controlador + "resultado/cargarmenu",

        success: function (evt) {

            $('#menuVariable').html(evt);

        },
        error: function () {
            alert("Ha ocurrido un error");
        }
    });
}

function mostrarReporte() {
    var topcodi = $("#hfTopologia").val();
    var escenario = $("#hfEscenario").val();
    var orientacion = 0;
    var parametro = "";
    var opcion = $('#hfOpcion').val();
    if ($('#chbOrientacion').prop('checked')) {
        orientacion = 1;
    }
    if (opcion == 1) {
        srestcodi = $("#cbVariable").val();
        parametro = $('select[name="IdVariable"] option:selected').text();
    }
    else {
        srestcodi = $("#cbCosto").val();
        parametro = $('select[name="IdCosto"] option:selected').text();
    }
   
    var titulo = "Escenario:" + topcodi + " " + escenario + " , Tipo:" + $('select[name="cbnTipoEscenario"] option:selected').text() + " , " + parametro; 
    $('#divTitulo').html(titulo);
    $.ajax({
        type: 'POST',
        url: controlador + "resultado/Lista",
        data: {
            topcodi: topcodi,
            srestcodi: srestcodi,
            fecha: $('#cbFecha').val(),
            orientacion: orientacion,
            costo: 0
        },
        success: function (evt) {
            $('#listado').css("width", $('#mainLayout').width() + "px");
            $('#listado').html(evt);
            $(document).ready(function () {
                var table = $('#tabla').dataTable({
                    scrollY: "600px",
                    scrollX: true,
                    scrollCollapse: true,
                    paging: false,
                    "sDom": 'ft',
                    ordering: false,
                    fixedColumns: {
                        leftColumns: 1
                    }
                });

            });
        },
        error: function () {
            alert("Ha ocurrido un error");
        }
    });
}

function mostrarCostoOperacion() {
    var topcodi = $("#hfTopologia").val();
    var escenario = $("#hfEscenario").val();
    var titulo = "Escenario:" + topcodi + " " + escenario + " , Tipo:" + $('select[name="cbnTipoEscenario"] option:selected').text() + " , Costos de la Operación" ;
    $('#divTitulo').html(titulo);
    $.ajax({
        type: 'POST',
        url: controlador + "resultado/Lista",
        data: {
            topcodi: topcodi,
            srestcodi: 0,
            fecha: $('#cbFecha').val(),
            orientacion: 0,
            costo: 1
        },
        success: function (evt) {
//            $('#listado').css("width", $('#mainLayout').width() + "px");
            $('#listado').html(evt);
        },
        error: function () {
            alert("Ha ocurrido un error");
        }
    });

}

openEscenario = function () {

    $('#divExportar').css('display', 'block'); 
}

closeExportar = function () {
    $('#divExportar').css('display', 'none');
}

function listarEscenario(busqueda) {
    var tipo = $('#cbTipoEscenario').val();
    var codigo = ($('#txtCodigo').val() == "") ? 0 : $('#txtCodigo').val();
    switch (busqueda) {
        case 0:

            break;
        case 1:
            if ($('#txtDescripcion').val() == "") {
                alert("Error:Ingresar descripción");
                return;
            }
            break;
        case 2:
            if (isNaN($('#txtCodigo').val())) {
                alert("Error:Ingresar código numérico");
                return;
            }
            break;
    }

    $.ajax({
        type: 'POST',
        url: controlador + "resultado/ListaEscenario",
        data: {
            fechaini: $('#txtFechaInicio').val(),
            fechafin: $('#txtFechaFin').val(),
            idTipo: tipo,
            nombre: $('#txtDescripcion').val(),
            topcodi: codigo,
            busqueda:busqueda
        },
        
        success: function (evt) {
            $('#idEscenario').html(evt);
            var table = $('#idtablatop').dataTable({
                scrollY: "200px",
                scrollX: true,
                scrollCollapse: true,
                paging: false,
                "sDom": 'ft',
                ordering: false
            });           
        },
        error: function () {
            alert("Ha ocurrido un error");
        }
    });
}

function seleccionarEscenario(topcodi, escenario, listafecha) {

    $('#idDivSearch').css('display', 'block');
    $('#divExportar').css('display', 'none');
    $('#hfTopologia').val(topcodi);
    $('#hfEscenario').val(escenario);
    $('#hfOpcion').val(1);
    var arrfechas = listafecha.split("#");
    $('select[id="cbFecha"]').empty();
    $.each(arrfechas, function (i, item) {
        $('#cbFecha').append($('<option>', {
            value: item,
            text: item
        }));
    });
    mostrarReporte($("#cbVariable").val(),0);
}

function exportarRsf() {
    var topcodi = $('#hfTopologia').val();
    $.ajax({
        type: 'POST',
        url: controlador + 'resultado/GeneraReporteRsfXls',
        data: {
            topcodi: topcodi
        },
        dataType: 'json',
        success: function (result) {
            if (result == 1) {//
                window.location = controlador + "resultado/ExportarReporteRsf";
            }
            if (result == -1) {
                alert("Error en reporte result")
            }
            if (result == 2) { // No existen registros
                alert("No existen registros !");
            }
        },
        error: function () {
            alert("Error en reporte");;
        }
    });
}

function exportarResultado() {
    var topcodi = $('#hfTopologia').val();
    var opcion = $('#hfOpcion').val();
    var srestcodi = (opcion == 1)?  $('#cbVariable').val() : $('#cbCosto').val() ;

    $.ajax({
        type: 'POST',
        url: controlador + 'resultado/GeneraReporteResultadoXls',
        data: {
            topcodi: topcodi,
            srestcodi: srestcodi,
            fecha: $('#cbFecha').val()
        },
        dataType: 'json',
        success: function (result) {
            if (result == 1) {//
                window.location = controlador + "resultado/ExportarReporteSalida";
            }
            if (result == -1) {
                alert("Error en reporte result")
            }
            if (result == 2) { // No existen registros
                alert("No existen registros !");
            }
        },
        error: function () {
            alert("Error en reporte");;
        }
    });
}