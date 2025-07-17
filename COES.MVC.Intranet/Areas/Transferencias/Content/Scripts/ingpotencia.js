var controlador = siteRoot + "transferencias/ingresopotencia/";

$(document).ready(function () {
    //buscar();
    document.getElementById('divOpcionCarga').style.display = "none";
    $("#Version").prop("disabled", true);
    $("#Pericodi").change(function () {
        if ($("#Pericodi").val() != "") {
            var options = {};
            options.url = controlador + "GetVersion";
            options.type = "POST";
            options.data = JSON.stringify({ pericodi: $("#Pericodi").val() });
            options.dataType = "json";
            options.contentType = "application/json";
            options.success = function (modelo) {
                //console.log(modelo);
                $("#Version").empty();
                $("#Version").removeAttr("disabled");
                $.each(modelo.ListaRecalculo, function (k, v) {
                    var option = '<option value=' + v.RecaCodi + '>' + v.RecaNombre + '</option>';
                    $('#Version').append(option);
                    //console.log(option);
                });
                if (modelo.bEjecutar == true)
                { document.getElementById('divOpcionCarga').style.display = "block"; }
                else
                { document.getElementById('divOpcionCarga').style.display = "none"; }
            };
            options.error = function () { alert("Lo sentimos, no se encuentran registrada ninguna revisión"); };
            $.ajax(options);
        }
        else {
            $("#Version").empty();
            $("#Version").prop("disabled", true);
            document.getElementById('divOpcionCarga').style.display = "none";
        }
    });

    $("#VersionB").prop("disabled", true);
    $("#Pericodi2").change(function () {
        if ($("#Pericodi2").val() != "") {
            var options = {};
            options.url = controlador + "GetVersion";
            options.type = "POST";
            options.data = JSON.stringify({ pericodi: $("#Pericodi2").val() });
            options.dataType = "json";
            options.contentType = "application/json";
            options.success = function (modelo) {
                //console.log(modelo);
                $("#VersionB").empty();
                $("#VersionB").removeAttr("disabled");
                $.each(modelo.ListaRecalculo, function (k, v) {
                    var option = '<option value=' + v.RecaCodi + '>' + v.RecaNombre + '</option>';
                    $('#VersionB').append(option);
                    //console.log(option);
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

    $('#btnCopiarVTP').click(function () {
        copiarVTP();
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
        vers = $("#VersionB option:selected").val();
        $.ajax({
            type: 'POST',
            url: controlador + "lista",
            data: { pericodi: p, version: vers },
            success: function (evt) {
                $('#listado').html(evt);
                //addDeleteEvent();
                //viewEvent();
                oTable = $('#tabla').dataTable({
                    "sPaginationType": "full_numbers",
                    "destroy": "true",
                    "aaSorting": [[2, "asc"]]
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
        var iVersion = $("#VersionB").val();
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

//ASSETEC 20181224
//Funciones de vista popup
copiarVTP = function () {
    var pericodi = $("#Pericodi").val();
    var version = $("#Version").val();
    if (pericodi == "" && version == "")
    {
        alert("Por favor, seleccionar un periodo y revisión correcto");
    }
    else {
        $.ajax({
            type: 'POST',
            url: controlador + "CopiarVTP",
            data: { pericodi: pericodi },
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
                alert("Lo sentimos, ha ocurrido un error inesperado");
            }
        });
    }
};

RecargarPopUp = function () {
    var pericodi = $("#pericodiVTP").val();
    $.ajax({
        type: 'POST',
        url: controlador + "CopiarVTP",
        data: { pericodi: pericodi },
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
            alert("Lo sentimos, ha ocurrido un error inesperado");
        }
    });
}

importarVTP = function () {
    var pericodi = $("#Pericodi").val();
    var version = $("#Version").val();
    var pericodiVTP = $("#pericodiVTP").val();
    var recpotcodi = $("#recpotcodi").val();
    //console.log(pericodi);
    if (pericodiVTP == "" && recpotcodi == "") {
        alert("Por favor, seleccionar un periodo y revisión correcto");
    }
    else {
        $.ajax({
            type: 'POST',
            url: controlador + "ImportarVTP",
            data: { pericodi: pericodi, version: version, pericodiVTP: pericodiVTP, recpotcodi: recpotcodi },
            dataType: 'json',
            success: function (result) {
                console.log(result);
                if (result == "1") {
                    mostrarExito("Felicidades, la información se reporto correctamente...!");
                }
                else {
                    mostrarError("Lo sentimos, se ha producido un error: " + result);
                }
            },
            error: function () {
                mostrarError("Lo sentimos, ha ocurrido un error inesperado");
            }
        });
    }
};

mostrarError = function (mensaje) {
    $('#mensaje').removeClass();
    $('#mensaje').html(mensaje);
    $('#mensaje').addClass('action-error');
}

mostrarExito = function (mensaje) {
    $('#mensaje').removeClass();
    $('#mensaje').html(mensaje);
    $('#mensaje').addClass('action-exito');
}