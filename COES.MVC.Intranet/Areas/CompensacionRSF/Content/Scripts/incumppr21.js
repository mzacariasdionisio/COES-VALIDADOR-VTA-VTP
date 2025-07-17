var sControlador = siteRoot + "CompensacionRSF/IncumpPR21/";

$(document).ready(function () {
    buscar();
});

buscar = function () {
    $.ajax({
        type: 'POST',
        url: sControlador + "lista",
        success: function (evt) {
            $('#listado').html(evt);
            addDeleteEvent();
            viewEvent();
            oTable = $('#tabla').dataTable({
                "sPaginationType": "full_numbers",
                "destroy": "true",
                "aaSorting": [[0, "desc"]]
            });
        },
        error: function () {
            mostrarError();
        }
    });
};

nuevo = function () {
    window.location.href = sControlador + "new";
}

//Funciones de eliminado de registro
addDeleteEvent = function () {
    $(".delete").click(function (e) {
        e.preventDefault();
        if (confirm("¿Desea eliminar la información seleccionada?")) {
            pericodi = $(this).attr("id").split("_")[1];
            vcrinccodi = $(this).attr("id").split("_")[2];
            $.ajax({
                type: "post",
                dataType: "text",
                url: sControlador + "Delete",
                data: addAntiForgeryToken({ pericodi: pericodi, vcrinccodi: vcrinccodi }),
                success: function (resultado) {
                    console.log(resultado);
                    if (resultado == "true") {
                        $("#fila_" + pericodi + "_" + vcrinccodi).remove();
                        mostrarExito("Se ha eliminado correctamente el registro");
                    }
                    else {
                        mostrarError("No se ha logrado eliminar el registro: ");
                    }
                }
            });
        }
    });
};

//Funciones de vista detalle
viewEvent = function () {
    $('.view').click(function () {
        pericodi = $(this).attr("id").split("_")[1];
        vcrinccodi = $(this).attr("id").split("_")[2];
        abrirPopup(pericodi, vcrinccodi);
    });
};

abrirPopup = function (pericodi, vcrinccodi) {
    $.ajax({
        type: 'POST',
        url: sControlador + "View",
        data: { pericodi: pericodi, vcrinccodi: vcrinccodi },
        success: function (evt) {
            $('#popup').html(evt);
            setTimeout(function () {
                $('#popup').bPopup({
                    easing: 'easeOutBack',
                    speed: 450,
                    transition: 'slideDown'
                });
            }, 50);
        },
        error: function () {
            mostrarError("Lo sentimos, ha ocurrido un error inesperado");
        }
    });
}

mostrarGrillaExcelPR21 = function () {
    error = [];
    if (typeof hot != 'undefined') {
        hot.destroy();
    }
    var container = document.getElementById('grillaExcelPR21');
    console.log(container);
    $.ajax({
        type: 'POST',
        url: sControlador + "grillaExcelPR21",
        dataType: 'json',
        success: function (result) {
            var columns = result.Columnas;
            var headers = result.Headers;
            var widths = result.Widths;
            var data = result.Data;
            hot = new Handsontable(container, {
                data: data,
                colHeaders: headers,
                rowHeaders: true,
                colWidths: widths,
                contextMenu: true,
                minSpareRows: 1,
                fixedRowsTop: 1,
                fixedColumnsLeft: 2,
                columns: columns
            });
            hot.render();
            //$('#divAcciones').css('display', 'block');
        },
        error: function () {
            alert('Lo sentimos no se puede mostrar la hoja de cálculo')
        }
    });
};