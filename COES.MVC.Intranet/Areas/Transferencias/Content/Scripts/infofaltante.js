
var ce = "infofaltante/";
var controler2 = "transferencias/" + ce;
var controler = siteRoot + controler2;

$(document).ready(function () {

    $("#Version").prop("disabled", true);
    $("#Periodo2").change(function () {        
        if ($("#Periodo2").val() != "") {
            var options = {};
            options.url = controler + "GetVersion";
            options.type = "POST";
            options.data = JSON.stringify({ pericodi: $("#Periodo2").val() });
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

    $('#btnBuscar').click(function () {
        buscar();
    });

    $('#btnGenerarExcel').click(function () {
        generarExcel();
    });

    

    $('#tab-container').easytabs({
        animate: false
    });

    $('#btnDescargarEntregaRetiro').click(function () {
        descargarEntregaRetiro();
    });
});

function buscar() {
    if ($("#Periodo option:selected").val() == "") {
        alert('Por favor, seleccione el Periodo');
    }
    else { 
        $.ajax({
            type: 'POST',
            url: controler + "Lista",
            data: { PeriCodi: $("#Periodo option:selected").val() },
            success: function (evt) {
                $('#listado').html(evt);
                oTable = $('#tabla').dataTable({
                    "sPaginationType": "full_numbers",
                    "destroy": "true"
                });
            },
            error: function () {
                alert("error en Buscar");
            }
        });
    }
}

generarExcel = function () {
    if ($("#Periodo option:selected").val() == "") {
        alert('Por favor, seleccione el Periodo');
    }
    else { 
        $.ajax({
            type: 'POST',
            url: controler + 'generarexcel',
            data: { PeriCodi: $("#Periodo option:selected").val() },
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

descargarEntregaRetiro = function () {
    if ($("#Periodo2 option:selected").val() == "") {
        alert("Por favor, seleccione un Periodo...");
    }
    else {
       
            iVersion = $("#Version").val();
            iPeriCodi = $("#Periodo2").val();
      
            $.ajax({
                type: 'POST',
                url: controler + 'DescargarInfoEntregadaFueraDeFecha',
                dataType: 'json',
                data: { PeriCodi: iPeriCodi, Version: iVersion },
                success: function (result) {
                    if (result == 1) {
                        window.location = controler + 'AbrirInfoEntregadaFueraDeFecha';
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
