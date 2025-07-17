var controlador = siteRoot + "intervenciones/consultasyreportes/";

var tabActualGlobal;
var sBusca = false;

$(document).ready(function () {   

    $('#tab-container').easytabs({
        animate: true
    });

    setActualTab();

    $('#btnBuscar').click(function () {
        sBusca = true;
        ListarProgramados();
        ListarEjecutados();       
    });   

    $('#btnGenerarExcelF1F2').click(function () {      
        generarExcelF1F2ProgramadosEjecutados();
    });

    $('#btnGenerarExcelIndicesF1F2').click(function () {
        generarExcelIndicesF1F2();
    });     

});

//funcion que calcula el ancho disponible para la tabla reporte
function getHeightTablaListado() {
    return $(window).height()
        - $("header").height()
        - $("#cntTitulo").height() - 2
        - $("#Reemplazable .form-title").height()
        - 15
        - $("#Contenido").parent().height() //Filtros
        - 14 //<br>
        - $(".dataTables_filter").height()
        - $(".dataTables_scrollHead").height()
        - 61 //- $(".footer").height() - 10
        - 32 // - $("#tab-container .etabs").height()
        - 100
        ;
}

////////////////////////////////////////////////////////////////////////
// Para el manejo del Tab
////////////////////////////////////////////////////////////////////////
function setActualTab() {
    var nameTab = $('.tab .active').attr('href');

    // Programada
    if (nameTab == "#paso1") { 
        tabActualGlobal = 0;
    } // Ejectuada
    else if (nameTab == "#paso2") { 
        tabActualGlobal = 1;
    } else {

    }
}
////////////////////////////////////////////////////////////////////////

////////////////////////////////////////////////////////////////////////
// Para Los Programados
////////////////////////////////////////////////////////////////////////
function ListarProgramados() {

    var anio = $('#anio').val();
    var mes = $('#mes').val();

    $("#listadoProgramados").html('');
    $.ajax({
        type: 'POST',
        url: controlador + "RptF1F2ListadoProgramados",
        data: { anio: anio, mes: mes },
        success: function (evt) {
            $("#listadoProgramados").hide();
            $('#listadoProgramados').css("width", ($('#mainLayout').width() - 30) + "px");
            $('#listadoProgramados').html(evt);
            var nuevoTamanioTabla = getHeightTablaListado();
            $("#listadoProgramados").show();
            nuevoTamanioTabla = nuevoTamanioTabla > 250 ? nuevoTamanioTabla : 250;

            $('#tablaProgramados').dataTable({
                "ordering": true,
                "info": false,
                "searching": true,
                "paging": false,
                "iDisplayLength": 25,
                "scrollX": true,
                "scrollY": $('#listadoProgramados').height() > 250 ? nuevoTamanioTabla + "px" : "100%"
            });
            $(".dataTable ").css("width", "1654px");
            var ajuste = getHeightTablaListado();
            ajuste = ajuste > 250 ? ajuste : 250;
            $(".dataTables_scrollBody").css("height", ajuste + "px");
        },
        error: function (err) {
            alert("Lo sentimos, ha ocurrido un error inesperado");
        }
    });
}
////////////////////////////////////////////////////////////////////////

////////////////////////////////////////////////////////////////////////
// Para Los Ejecutados
////////////////////////////////////////////////////////////////////////
function ListarEjecutados() {

    var anio = $('#anio').val();
    var mes = $('#mes').val();

    $("#listadoEjecutados").html('');
    $.ajax({
        type: 'POST',
        url: controlador + "RptF1F2ListadoEjecutados",
        data: { anio: anio, mes: mes },
        success: function (evt) {
            $("#listadoEjecutados").hide();
            $('#listadoEjecutados').css("width", ($('#mainLayout').width() - 30) + "px");
            $('#listadoEjecutados').html(evt);
            var nuevoTamanioTabla = getHeightTablaListado();
            $("#listadoEjecutados").show();
            nuevoTamanioTabla = nuevoTamanioTabla > 250 ? nuevoTamanioTabla : 250;

            $('#tablaEjecutados').dataTable({
                "ordering": true,
                "info": false,
                "searching": true,
                "paging": false,
                "iDisplayLength": 25,
                "scrollX": true,
                "scrollY": $('#listadoEjecutados').height() > 250 ? nuevoTamanioTabla + "px" : "100%"
            });
            $(".dataTable ").css("width", "1654px");
            var ajuste = getHeightTablaListado();
            ajuste = ajuste > 250 ? ajuste : 250;
            $(".dataTables_scrollBody").css("height", ajuste + "px");
        },
        error: function (err) {
            alert("Lo sentimos, ha ocurrido un error inesperado");
        }
    });
}
////////////////////////////////////////////////////////////////////////

////////////////////////////////////////////////////////////////////////
// Genera Listados Excel
////////////////////////////////////////////////////////////////////////
function generarExcelF1F2ProgramadosEjecutados() {

    var anio = $('#anio').val();
    var mes = $('#mes').val();

    $.ajax({
        type: 'POST',
        url: controlador + 'GenerarExcelF1F2ProgramadosEjecutados',
        data: { anio: anio, mes: mes },
        dataType: 'json',
        success: function (result) {
            if (result == -1) {
                alert("No se encuentra datos a exportar!")
            }
            else if (result != -1) {
                document.location.href = controlador + 'Descargar?file=' + result;
            }
            else {
                alert("Ha ocurrido un error");
            }
        },
        error: function (err) {
            alert("Ha ocurrido un error");
        }
    });
}
////////////////////////////////////////////////////////////////////////

////////////////////////////////////////////////////////////////////////
// Genera reporte de indices F1 y F2
////////////////////////////////////////////////////////////////////////
function generarExcelIndicesF1F2() {

    var anio = $('#anio').val();
    var mes = $('#mes').val();

    var flgCorrectivo = false;
    if ($('#flgCorrectivo').is(':checked')) {
        flgCorrectivo = true;
    }

    $.ajax({
        type: 'POST',
        url: controlador + 'GenerarExcelIndicesF1F2',
        data: { anio: anio, mes: mes, flgCorrectivo: flgCorrectivo },
        dataType: 'json',
        success: function (result) {
            if (result == -1) {
                alert("No se encuentra datos a exportar!")
            }
            else if (result != -1) {
                document.location.href = controlador + 'Descargar?file=' + result;                
            }
            else {
                alert("Ha ocurrido un error");
            }
        },
        error: function (err) {
            alert("Ha ocurrido un error");
        }
    });
}
////////////////////////////////////////////////////////////////////////

