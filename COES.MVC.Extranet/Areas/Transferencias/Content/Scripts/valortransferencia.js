var controler = siteRoot +  "transferencias/valortransferencia/";

$(document).ready(function () {
    /*$('#EMPRCODI').multipleSelect({
        width: '200px',
        filter: true,
        selectAll: false,
        single: true
    });*/

    $("#VersionC").prop("disabled", true);
    $("#PERICODI").change(function () {
        listVersion();
    });

    $("#VersionC").change(function () {
        listEmpresa();
    });

    $("#VersionA").prop("disabled", true);
    $("#PERICODI2").change(function () {
        if ($("#PERICODI2").val() != "") {
            var options = {};
            options.url = controler + "GetVersion";
            options.type = "POST";
            options.data = JSON.stringify({ pericodi: $("#PERICODI2").val() });
            options.dataType = "json";
            options.contentType = "application/json";
            options.success = function (modeloLista) {
                //console.log(modeloLista);
                $("#VersionA").empty();
                $("#VersionA").removeAttr("disabled");
                $.each(modeloLista, function (k, v) {
                    var option = '<option value=' + v.RecaCodi + '>' + v.RecaNombre + '</option>';
                    $('#VersionA').append(option);
                    //console.log(option);
                });
            };
            options.error = function () { alert("Lo sentimos, no se encuentran registrada ninguna revisión"); };
            $.ajax(options);
        }
        else {
            $("#VersionA").empty();
            $("#VersionA").prop("disabled", true);
        }
    });


    $("#VersionD").prop("disabled", true);
    $("#Pericodi5").change(function () {
        if ($("#Pericodi5").val() != "") {
            var options = {};
            options.url = controler + "GetVersion";
            options.type = "POST";
            options.data = JSON.stringify({ pericodi: $("#Pericodi5").val() });
            options.dataType = "json";
            options.contentType = "application/json";
            options.success = function (modeloLista) {
                //console.log(modeloLista);
                $("#VersionD").empty();
                $("#VersionD").removeAttr("disabled");
                $.each(modeloLista, function (k, v) {
                    var option = '<option value=' + v.RecaCodi + '>' + v.RecaNombre + '</option>';
                    $('#VersionD').append(option);
                    //console.log(option);
                });
            };
            options.error = function () { alert("Lo sentimos, no se encuentran registrada ninguna revisión"); };
            $.ajax(options);
        }
        else {
            $("#VersionD").empty();
            $("#VersionD").prop("disabled", true);
        }
    });

    $("#VersionB").prop("disabled", true);

    $('#btnBuscar').click(function () {
        buscar();
    });

    $('#tab-container').easytabs({
        animate: false
    });

    $('#btnBuscar2').click(function () {
        buscar2();
    });
    
    $('#btnExportarEntregasRetirosEnergiaValorizados').click(function () {
        exportarEntregasRetirosEnergiaValorizados(); //Paso 2 - Exportar las Entregas y Retiros de energía valorizados
    });

    $('#btnExportarEntregasRetirosEnergiaValorizados15min').click(function () {
        exportarEntregasRetirosEnergiaValorizados15min(); //Paso 2 - Exportar las Entregas y Retiros de energía valorizados cada 15 mins
    });

    $('#btnExportarDeterminacionSaldosTransferencias').click(function () {
        exportarDeterminacionSaldosTransferencias(); //Paso 3 - Exportar la Determinación de saldos de transferencias
    });

    $('#btnExportarPagosTransferenciasEnergíaActiva').click(function () {
        exportarPagosTransferenciasEnergíaActiva(); //Paso 4 - Exportar los Pagos por transferencias de energía activa
    });

    $('#btnExportarEntregasRetirosEnergia').click(function () {
        exportarEntregasRetirosEnergia(); //Paso 5 - Exportar Reporte Mensual
    });

});

AddAntiForgeryToken = function (data) {
    data.__RequestVerificationToken = $('input[name=__RequestVerificationToken]').val();
    return data;
};

function listEmpresa(){
    if ($("#VersionC").val() != "") {
        var options = {};
        options.url = controler + "GetEmpresasxPeriodo";
        options.type = "POST";
        options.data = JSON.stringify({ pericodi: $("#PERICODI").val(), version: $("#VersionC").val() });
        options.dataType = "json";
        options.contentType = "application/json";
        options.success = function (empresas) {
            $("#EMPRCODI").empty();
            $("#EMPRCODI").removeAttr("disabled");
            var option2 = "<option value='0'>--Seleccione--</option>";
            $('#EMPRCODI').append(option2);
            $.each(empresas.ListaEmpresas, function (k, v) {
                var option = '<option value=' + v.EmprCodi + '>' + v.EmprNombre + '</option>';
                $('#EMPRCODI').append(option);
            });
        };
        options.error = function () { alert("Lo sentimos, no se encuentran registrada ninguna empresa"); };
        $.ajax(options);
    }
    else {
        $("#EMPRCODI").empty();
        $("#EMPRCODI").prop("disabled", true);
    }
}

function listVersion() {
    if ($("#PERICODI").val() != "") {
        var options = {};
        options.url = controler + "GetVersion";
        options.type = "POST";
        options.data = JSON.stringify({ pericodi: $("#PERICODI").val() });
        options.dataType = "json";
        options.contentType = "application/json";
        options.success = function (modeloLista) {
            console.log(modeloLista);
            $("#VersionC").empty();
            $("#VersionC").removeAttr("disabled");
            $.each(modeloLista, function (k, v) {
                var option = '<option value=' + v.RecaCodi + '>' + v.RecaNombre + '</option>';
                $('#VersionC').append(option);
            });
            listEmpresa();
        };
        options.error = function () { alert("Lo sentimos, no se encuentran registrada ninguna revisión"); };
        $.ajax(options);
    }
    else {
        $("#VersionC").empty();
        $("#VersionC").prop("disabled", true);
    }
}

function buscar() {  
    var pericodi = $("#PERICODI option:selected").val();
    var version = $("#VersionC option:selected").val();
    if (pericodi == "" || version == "") {
        alert('Por favor, seleccione el Periodo y la versión');
    }
    else
    {
        var empcodi = $("#EMPRCODI option:selected").val();
        var tipoempr = $("#TIPOEMPRCODI option:selected").val();
        var barrcodi = $("#BARRCODI option:selected").val();
        var flag = $('[name="FLAG"]:radio:checked').val();
        if (empcodi == '' || empcodi == 0)
            empcodi = null;
        if (barrcodi == '')
            barrcodi = null;
        if (pericodi == '')
            pericodi = null;
        if (tipoempr == '')
            tipoempr = null;
        if (flag == 'T')
            flag = null;
        $.ajax({
            type: 'POST',
            url: controler + "lista",
            data: { empcodi: empcodi, barrcodi: barrcodi, pericodi: pericodi, tipoemprcodi: tipoempr, vers: version, flagEntrReti: flag },
            success: function (evt) {
                $('#listado').html(evt);
                //add_deleteEvent();
                //ViewEvent();
                oTable = $('#tabla').dataTable({
                    "sPaginationType": "full_numbers",
                    "destroy": true,
                    "scrollY": 430,
                    "scrollX": true,
                    "aaSorting": [[0, "asc"], [1, "asc"], [2, "asc"]]
                });
            },
            error: function () {
                mostrarError();
            }
        });
    }
}

exportarEntregasRetirosEnergiaValorizados = function ()
{   //Paso 2 - Exportar las Entregas y Retiros de energía valorizados
    var pericodi = $("#PERICODI option:selected").val();
    var version = $("#VersionC option:selected").val();
    if (pericodi == "" || version == "") {
        alert('Por favor, seleccione el Periodo y la versión');
    }
    else {
        var empcodi = $("#EMPRCODI option:selected").val();
        var tipoempr = $("#TIPOEMPRCODI option:selected").val();
        var barrcodi = $("#BARRCODI option:selected").val();
        var flag = $('[name="FLAG"]:radio:checked').val();
        if (empcodi == '' || empcodi == 0)
            empcodi = null;
        if (barrcodi == '')
            barrcodi = null;
        if (pericodi == '')
            pericodi = null;
        if (tipoempr == '')
            tipoempr = null;
        if (flag == 'T')
            flag = null;
        $.ajax({
            type: 'POST',
            url: controler + 'ExportarEntregasRetirosEnergiaValorizados',
            data: { empcodi: empcodi, barrcodi: barrcodi, pericodi: pericodi, tipoemprcodi: tipoempr, vers: version, flagEntrReti: flag },
            dataType: 'json',
            success: function (result) {
                if (result == 1) {
                    window.location = controler + 'AbrirExcelEntregasRetirosEnergiaValorizados';
                }
                if (result == -1) {
                    mostrarError();
                }
            },
            error: function () {
                mostrarError();
            }
        });
    }
}

function buscar2() {

    var pericodi = $("#PERICODI2 option:selected").val();
    var version = $("#VersionA option:selected").val();
    if (pericodi == "" || version == "") {
        alert('Por favor, seleccione el Periodo y la versión');
    }
    else
    {
        $.ajax({
            type: 'POST',
            url: controler + "ListaInfo",
            data: { pericodi: pericodi, vers: version },
            success: function (evt) {
                $('#listado2').html(evt);
                //add_deleteEvent();
                //ViewEvent();
                oTable = $('#tabla2').dataTable({
                    "sPaginationType": "full_numbers",
                    "destroy": true,
                    "scrollY": 430,
                    "scrollX": true,
                    "aaSorting": [[0, "asc"]]
                });
            },
            error: function () {
                mostrarError();
            }
        });
    }
}

exportarDeterminacionSaldosTransferencias = function () {
    var pericodi = $("#PERICODI2 option:selected").val();
    var version = $("#VersionA option:selected").val();
    if (pericodi == "" || version == "") {
        alert('Por favor, seleccione el Periodo y la versión');
    }
    else {
        $.ajax({
            type: 'POST',
            url: controler + 'ExportarDeterminacionSaldosTransferencias',
            data: { pericodi: pericodi, vers: version },
            dataType: 'json',
            success: function (result) {
                if (result == 1) {
                    window.location = controler + 'AbrirDeterminacionSaldosTransferencias';
                }
                if (result == -1) {
                    mostrarError();
                }
            },
            error: function () {
                mostrarError();
            }
        });
    }
}

exportarPagosTransferenciasEnergíaActiva = function () {
    var pericodi = $("#PERICODI4 option:selected").val();
    var version = $("#VersionB option:selected").val();
    if (pericodi == "" || version == "") {
        alert('Por favor, seleccione el Periodo y la versión');
    }
    else {
        $.ajax({
            type: 'POST',
            url: controler + 'ExportarPagosTransferenciasEnergíaActiva',
            dataType: 'json',
            data: { iPeriCodi: pericodi, iVersion: version },
            success: function (result) {
                if (result == 1) {
                    window.location = controler + 'AbrirPagosTransferenciasEnergíaActiva';
                }
                if (result == -1) {
                    mostrarError();
                }
            },
            error: function () {
                mostrarError();
            }
        });
    }
}

exportarEntregasRetirosEnergia = function () {
    var pericodi = $("#Pericodi5 option:selected").val();
    var version = $("#VersionD option:selected").val();
    if (pericodi == "" || version == "") {
        alert('Por favor, seleccione el Periodo y la versión');
    }
    else {
        $.ajax({
            type: 'POST',
            url: controler + 'exportarEntregasRetirosEnergia',
            dataType: 'json',
            data: { iPeriCodi: pericodi, iVersion: version },
            success: function (result) {
                if (result == 1) {
                    window.location = controler + 'AbrirBalanceEnergia';
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


exportarEntregasRetirosEnergiaValorizados15min = function () {   //Paso 2 - Exportar las Entregas y Retiros de energía valorizadoscada 15 min
    var pericodi = $("#PERICODI option:selected").val();
    var version = $("#VersionC option:selected").val();
    var empcodi = $("#EMPRCODI option:selected").val();
    var barrcodi = $("#BARRCODI option:selected").val();

    if (pericodi == "" || version == "") {
        alert('Por favor seleccione el periodo y versión');
    }
    else if ((empcodi == "" || empcodi == undefined) && barrcodi == "") {
        alert('Por favor seleccione una Barra de Transferencia o una Empresa');
    }
    else {
        var tipoempr = $("#TIPOEMPRCODI option:selected").val();
        var flag = $('[name="FLAG"]:radio:checked').val();
        if (empcodi == '' || empcodi == undefined)
            empcodi = 0;
        if (barrcodi == '')
            barrcodi = 0;
        if (tipoempr == '')
            tipoempr = null;
        $.ajax({
            type: 'POST',
            url: controler + 'ExportarEntregasRetirosEnergiaValorizados15min',
            data: { pericodi: pericodi, vers: version, empcodi: empcodi, barrcodi: barrcodi, flagEntrReti: flag },
            dataType: 'json',
            success: function (result) {
                if (result == "1") {
                    window.location = controler + 'AbrirExcelEntregasRetirosEnergiaValorizados15min';
                }
                else if (result == "-1") {
                    mostrarError();
                }
                else {
                    alert(result);
                }
            },
            error: function () {
                mostrarError();
            }
        });
    }
}


function mostrarError() {
    alert('Lo sentimos, se ha producido un error al procesar la información');
}
