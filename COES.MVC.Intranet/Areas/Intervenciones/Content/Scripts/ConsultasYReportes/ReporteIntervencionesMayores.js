var controlador = siteRoot + "intervenciones/consultasyreportes/";
var pagActualGlobal;

$(document).ready(function () {

    $('#cboEmpresa').multipleSelect({
        placeholder: "------------------------------ Todos ------------------------------",
        selectAll: false,
        allSelected: onoffline
    });

    $('#btnBuscar').click(function () {
        ListarMayores();
    });

    $('#btnGenerarExcel').click(function () {
        generarExcel();
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
        - $("#tablaTabular2").parent().height()
        - 14 //<br>
        - $(".dataTables_filter").height()
        - $(".dataTables_scrollHead").height()
        - 61 //- $(".footer").height() - 10
        -80
        ;
}

function ListarMayores(nroPagina) {
    var idProgramacionAnual = document.getElementById('cboProgramacion').value;
    var emprCodi = JSON.stringify($('#cboEmpresa').val());

    if (idProgramacionAnual == "0") {
        alert("No ha seleccionado la programación anual");
        return;
    }

    $("#listado").html('');
    $.ajax({
        type: 'POST',
        url: controlador + "RptIntervencionesMayoresListado",
        data: {
            idProgramacionAnual: idProgramacionAnual, emprCodi: emprCodi
        },
        success: function (evt) {
            $("#listado").hide();
            $('#listado').css("width", $('#mainLayout').width() + "px");

            var nuevoTamanioTabla = getHeightTablaListado();
            $("#listado").show();
            nuevoTamanioTabla = nuevoTamanioTabla > 250 ? nuevoTamanioTabla : 250;

            $('#listado').html(evt);
            $('#tabla').dataTable({
                "ordering": true,
                "info": false,
                "searching": true,
                "paging": false,
                "iDisplayLength": 25,
                "scrollX": true,
                "scrollY": $('#listado').height() > 250 ? nuevoTamanioTabla + "px" : "100%"
            });           
        },
        error: function (err) {
            alert("Lo sentimos, ha ocurrido el siguiente error");
        }
    });
}

function generarExcel() {
    var idProgramacionAnual = document.getElementById('cboProgramacion').value;
    var emprCodi = JSON.stringify($('#cboEmpresa').val());
    var anexo = $('#cboAnexos').val();

    if (idProgramacionAnual == "0") {
        alert("No ha seleccionado la programación anual");
        return;
    }

    if (anexo == "0") {
        alert("No ha seleccionado el anexo");
        return;
    }

    $.ajax({
        type: 'POST',
        url: controlador + 'GenerarExcelIntervencionesMayores',
        data: {
            idProgramacionAnual: idProgramacionAnual, emprCodi: emprCodi, anexo: anexo
        },
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
