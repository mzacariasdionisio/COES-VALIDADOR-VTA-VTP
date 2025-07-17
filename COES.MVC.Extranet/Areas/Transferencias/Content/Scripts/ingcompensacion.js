var controlador = siteRoot + "transferencias/ingresocompensacion/";

$(document).ready(function () {
    $("#VersionB").prop("disabled", true);
    $("#Pericodi2").change(function () {
        if ($("#Pericodi2").val() != "") {
            var options = {};
            options.url = controlador + "GetVersion";
            options.type = "POST";
            options.data = JSON.stringify({ pericodi: $("#Pericodi2").val() });
            options.dataType = "json";
            options.contentType = "application/json";
            options.success = function (ListaRecalculos) {
                //console.log(ListaRecalculos);
                $("#VersionB").empty();
                $("#VersionB").removeAttr("disabled");
                $.each(ListaRecalculos, function (k, v) {
                    var option = '<option value=' + v.RecaCodi + '>' + v.RecaNombre + '</option>';
                    $('#VersionB').append(option);
                    ////console.log(option);
                });
            };
            options.error = function () { alert("Lo sentimos, no se encuentran registrada ninguna revisión"); };
            $.ajax(options);
        }
        else {
            $("#VersionB").empty();
            $("#VersionB").prop("disabled", true);
        }
    });

    $('#tab-container').easytabs({
        animate: false
    });

    $('#btnBuscar').click(function () {
        buscar();
    });

    $('#btnGenerarExcel').click(function () {
        generarExcel();
    });
});

function buscar() {
    var p;
    var cbo = $("#Pericodi2 option:selected").val();

    if (cbo == "") {
        alert('Por favor, seleccione el Periodo');
    }
    else {
        p = $("#Pericodi2 option:selected").val();
        vers =  $("#VersionB option:selected").val();
        $.ajax({
        type: 'POST',
        url: controlador + "lista",
        data: { pericodi: p, version: vers },
        success: function (evt) {
            $('#listado').html(evt);
            //add_deleteEvent();
            //ViewEvent();
            oTable = $('#tabla').dataTable({
                "sPaginationType": "full_numbers",
                "destroy": true,
                "scrollY": 430,
                "scrollX": true,
                "aaSorting": [[2, "asc"], [3, "asc"]]
            });
        },
        error: function () {
            mostrarError();
        }
        });
    }

}

generarExcel = function () {
    if ($("#Pericodi2").val() == "") {
        alert("Por favor, seleccione un periodo");
    }
    else {
        var iPericodi = $("#Pericodi2").val();
        var iVersion =$("#VersionB").val();
        $.ajax({
            type: 'POST',
            url: controlador + 'generarexcel',
            data: { iPericodi: iPericodi, iVersion: iVersion },
            dataType: 'json',
            success: function (result) {
                if (result == 1) {
                    window.location = controlador + 'abrirexcel';
                }
                if (result == -1) {
                    alert("Lo sentimos, se ha producido un error");
                }
            },
            error: function () {
                alert("Lo sentimos, se ha producido un error");
            }
        });
    }
}