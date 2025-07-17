var controler = siteRoot + "transferencias/infodesbalance/";

//Funciones de busqueda
$(document).ready(function () {
    preload();
    $("#Version").prop("disabled", true);
    $("#PERICODI").change(function () {
        if ($("#PERICODI").val() != "") {
            var options = {};
            options.url = controler + "GetVersion";
            options.type = "POST";
            options.data = JSON.stringify({ pericodi: $("#PERICODI").val() });
            options.dataType = "json";
            options.contentType = "application/json";
            options.success = function (states) {
                //console.log(states);
                $("#Version").empty();
                $("#Version").removeAttr("disabled");
                $.each(states, function (k, v) {
                    var a = '<option value=' + v.RecaCodi + '>' + v.RecaNombre + '</option>';
                    $('#Version').append(a);
                    //console.log(a);
                });
            };
            options.error = function () { alert("Lo sentimos, no se encuentran registrada ninguna revisión"); };
            $.ajax(options);
        }
        else {
            $("#Version").empty();
            $("#Version").prop("disabled", true);
        }
    });

    $('#btnMostrar').click(function () {
        buscar();
    });

    $('#btnGenerarExcel').click(function () {
        generarExcel();
    });
});

function preload() {
    if ($("#PERICODI").val() != "") {
        var options = {};
        options.url = controler + "GetVersion";
        options.type = "POST";
        options.data = JSON.stringify({ pericodi: $("#PERICODI").val() });
        options.dataType = "json";
        options.contentType = "application/json";
        options.success = function (states) {
            //console.log(states);
            $("#Version").empty();
            $("#Version").removeAttr("disabled");
            $.each(states, function (k, v) {
                var a = '<option value=' + v.RecaCodi + '>' + v.RecaNombre + '</option>';
                $('#Version').append(a);
                //console.log(a);
            });
        };
        options.error = function () { alert("Lo sentimos, no se encuentran registrada ninguna revisión"); };
        $.ajax(options);
    }
    else {
        $("#Version").empty();
        $("#Version").prop("disabled", true);
    }
}

function buscar() {
    if ($("#PERICODI option:selected").val() == "") {
        alert("Seleccione un Periodo");
    }
    else {
      $.ajax({
        type: 'POST',
        url: controler + "lista",     
        data: { periodo: $("#PERICODI option:selected").val(), version: $("#Version").val(), desbalance: $("#txtDesbalance").val() },
        success: function (evt) {
            $('#listado').html(evt);
            oTable = $('#tabla').dataTable({
                "sPaginationType": "full_numbers",
                "destroy": "true"
            });
        },
        error: function () {
            alert("Lo sentimos, se ha producido un error");
        }
      });
    }

};

generarExcel = function () {
    if ($("#PERICODI").val() == "") {
        alert("Por favor, seleccione un periodo");
    }
    else {
        var iPericodi = $("#PERICODI").val();
        var iVersion = $("#Version").val();
        var desbalance = $("#txtDesbalance").val();
        $.ajax({
            type: 'POST',
            url: controler + 'generarexcel',
            data: { iPericodi: iPericodi, iVersion: iVersion, desbalance: desbalance},
            dataType: 'json',
            success: function (result) {
                if (result == 1) {
                    window.location = controler + 'abrirexcel';
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