
var controler = siteRoot + "transferencias/valortransferencia/";

$(document).ready(function () {

    $('#EMPRCODI').multipleSelect({
        width: '200px',
        filter: true,
        selectAll: false,
        single: true
    });

    document.getElementById('divOpcionCarga').style.display = "none";
    $("#Version").prop("disabled", true);
    $("#PERICODI3").change(function () {
        if ($("#PERICODI3").val() != "") {
            var options = {};
            options.url = controler + "GetVersion";
            options.type = "POST";
            options.data = JSON.stringify({ pericodi: $("#PERICODI3").val() });
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
                if (modelo.bEjecutar == true) { document.getElementById('divOpcionCarga').style.display = "block"; }
                else { document.getElementById('divOpcionCarga').style.display = "none"; }
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

    $("#VersionC").prop("disabled", true);
    $("#PERICODI").change(function () {
        if ($("#PERICODI").val() != "") {
            var options = {};
            options.url = controler + "GetVersion";
            options.type = "POST";
            options.data = JSON.stringify({ pericodi: $("#PERICODI").val() });
            options.dataType = "json";
            options.contentType = "application/json";
            options.success = function (modelo) {
                //console.log(modelo);
                $("#VersionC").empty();
                $("#VersionC").removeAttr("disabled");
                $.each(modelo.ListaRecalculo, function (k, v) {
                    var option = '<option value=' + v.RecaCodi + '>' + v.RecaNombre + '</option>';
                    $('#VersionC').append(option);
                    //console.log(option);
                });
            };
            options.error = function () { alert("Lo sentimos, no se encuentran registrada ninguna revisión"); };
            $.ajax(options);
        }
        else {
            $("#VersionC").empty();
            $("#VersionC").prop("disabled", true);
        }
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
            options.success = function (modelo) {
                //console.log(modelo);
                $("#VersionA").empty();
                $("#VersionA").removeAttr("disabled");
                $.each(modelo.ListaRecalculo, function (k, v) {
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

    $("#VersionB").prop("disabled", true);
    $("#PERICODI4").change(function () {
        if ($("#PERICODI4").val() != "") {
            var options = {};
            options.url = controler + "GetVersion";
            options.type = "POST";
            options.data = JSON.stringify({ pericodi: $("#PERICODI4").val() });
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

    $("#recpotcodiSaldo").prop("disabled", true);
    $("#pericodiSaldo").change(function () {
        if ($("#pericodiSaldo").val() != "") {
            var options = {};
            options.url = controler + "GetVersion";
            options.type = "POST";
            options.data = JSON.stringify({ pericodi: $("#pericodiSaldo").val() });
            options.dataType = "json";
            options.contentType = "application/json";
            options.success = function (modelo) {
                //console.log(modelo);
                $("#recpotcodiSaldo").empty();
                $("#recpotcodiSaldo").removeAttr("disabled");
                $.each(modelo.ListaRecalculo, function (k, v) {
                    var option = '<option value=' + v.RecaCodi + '>' + v.RecaNombre + '</option>';
                    $('#recpotcodiSaldo').append(option);
                    //console.log(option);
                });
            };
            options.error = function () { alert("Lo sentimos, no se encuentran registrada ninguna revisión"); };
            $.ajax(options);
        }
        else {
            $("#recpotcodiSaldo").empty();
            $("#recpotcodiSaldo").prop("disabled", true);
        }
    });

    $('#btnBuscar').click(function () {
        buscar();
    });

    $('#tab-container').easytabs({
        animate: false
    });

    $('#btnBuscar2').click(function () {
        buscar2();
    });

    $('#Procesar').click(function () {
        ProcesarValorizacion(); //Paso 1 - Entregas y Retiros de energía valorizados
    });

    $('#Borrar').click(function () {
        Borrar();
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

    //Paso 4 - Exportar los Pagos por transferencias de energía activa
    $('#btnExportarPagosTransferenciasEnergíaActiva').click(function () {
        exportarPagosTransferenciasEnergíaActiva();
    });

    $('#btnExportarPagosTransferenciasEnergíaActivaPrint').click(function () {
        exportarPagosTransferenciasEnergíaActivaPrint();
    });

    $('#btnExportarEntregasRetirosEnergia').click(function () {
        exportarEntregasRetirosEnergia();
    });

    $('#btnExportarDesviacionRetiros').click(function () {
        exportarDesviacionRetiros();
    });
    //ASSETEC 20190104
    $('#btnExportarDesviacionEntregas').click(function () {
        exportarDesviacionEntregas();
    });

    //Paso 5 - Exportar Reporte Mensual
    $('#btnExportarHistoricoEntregasRetiros').click(function () {
        exportarHistoricoEntregasRetiros();
    });

    $('#btnExportarHistorico15minCodigoEntregaRetiro').click(function () {
        exportarHistorico15minCodigoEntregaRetiro();
    });

    $('#btnMigrarSaldo').click(function () {
        MigrarSaldo();
    });

    $('#btnMigrarVTEA').click(function () {
        MigrarVTEA();
    });
});

addAntiForgeryToken = function (data) {
    data.__RequestVerificationToken = $('input[name=__RequestVerificationToken]').val();
    return data;
};

mostrarError = function () {
    alert('Lo sentimos, se ha producido un error al procesar la información');
}

Borrar = function () {
    var periodo = $("#PERICODI3 option:selected").val();

    if (periodo == '')
        alert('Seleccione un Periodo');
    else {
        version = $("#Version").val();
        $.ajax({
            type: 'POST',
            url: controler + 'Borrar',
            data: { pericodi: periodo, vers: version },
            dataType: 'json',
            success: function (resultado) {
                if (resultado == "1") {
                    alert('La información ha sido eliminada correctamente')
                    //buscar();
                }
                else {
                    mostrarMensaje(resultado);
                }
            },
            error: function () {
                mostrarError();
            }
        });
    }
}

ProcesarValorizacion = function () {   ////Paso 1 - Entregas y Retiros de energía valorizados
    var periodo = $("#PERICODI3 option:selected").val();
    if (periodo == '')
        alert('Seleccione un Periodo');
    else {
        version = $("#Version").val();
        $.ajax({
            type: 'POST',
            url: controler + 'procesarvalorizacion',
            data: { pericodi: periodo, vers: version },
            dataType: 'json',
            success: function (resultado) {
                if (resultado == "1") {
                    alert('La información ha sido procesada satisfactoriamente')
                    //buscar();
                }
                else {
                    mostrarMensaje(resultado);
                }
            },
            error: function () {
                mostrarError();
            }
        });
    }
}

buscar = function () {
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
        if (empcodi == '')
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
                //addDeleteEvent();
                //viewEvent();
                oTable = $('#tabla').dataTable({
                    "sPaginationType": "full_numbers",
                    "destroy": "true",
                    "aaSorting": [[0, "asc"], [1, "asc"], [4, "asc"]]
                });
            },
            error: function () {
                mostrarError();
            }
        });
    }
}

exportarEntregasRetirosEnergiaValorizados = function () {   //Paso 2 - Exportar las Entregas y Retiros de energía valorizados
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
        if (empcodi == '')
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
                if (result == "1") {
                    window.location = controler + 'AbrirExcelEntregasRetirosEnergiaValorizados';
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

exportarEntregasRetirosEnergiaValorizados15min = function () {   //Paso 2 - Exportar las Entregas y Retiros de energía valorizadoscada 15 min
    var pericodi = $("#PERICODI option:selected").val();
    var version = $("#VersionC option:selected").val();
    var empcodi = $("#EMPRCODI option:selected").val(); //if  || empcodi == undefined
    var barrcodi = $("#BARRCODI option:selected").val();

    if (pericodi == "" || version == "") {
        alert('Por favor seleccione el periodo y la versión');
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

buscar2 = function () {

    var pericodi = $("#PERICODI2 option:selected").val();
    var version = $("#VersionA option:selected").val();
    if (pericodi == "" || version == "") {
        alert('Por favor, seleccione el Periodo y la versión');
    }
    else {
        $.ajax({
            type: 'POST',
            url: controler + "ListaInfo",
            data: { pericodi: pericodi, vers: version },
            success: function (evt) {
                $('#listado2').html(evt);
                //addDeleteEvent();
                //viewEvent();
                oTable = $('#tabla2').dataTable({
                    "sPaginationType": "full_numbers",
                    "destroy": "true",
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
                if (result == "1") {
                    window.location = controler + 'AbrirDeterminacionSaldosTransferencias';
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
//Paso 4 - Exportar los Pagos por transferencias de energía activa
exportarPagosTransferenciasEnergíaActiva = function () {
    var pericodi = $("#PERICODI4 option:selected").val();
    var version = 1; //Siempre es solo el mensual
    if (pericodi == "") {
        alert('Por favor, seleccione el Periodo');
    }
    else {
        $.ajax({
            type: 'POST',
            url: controler + 'ExportarPagosTransferenciasEnergíaActiva',
            dataType: 'json',
            data: { iPeriCodi: pericodi, iVersion: version },
            success: function (result) {
                if (result == "1") {
                    window.location = controler + 'AbrirPagosTransferenciasEnergíaActiva';
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

exportarPagosTransferenciasEnergíaActivaPrint = function () {
    var pericodi = $("#PERICODI4 option:selected").val();
    var version = 1; //Siempre es solo el mensual
    if (pericodi == "") {
        alert('Por favor, seleccione el Periodo');
    }
    else {
        $.ajax({
            type: 'POST',
            url: controler + 'ExportarPagosTransferenciasEnergíaActivaPrint',
            dataType: 'json',
            data: { iPeriCodi: pericodi, iVersion: version },
            success: function (result) {
                if (result == "1") {
                    window.location = controler + 'AbrirPagosTransferenciasEnergíaActiva';
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

exportarEntregasRetirosEnergia = function () {
    var pericodi = $("#PERICODI4 option:selected").val();
    var version = $("#VersionB option:selected").val();
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
                if (result == "1") {
                    window.location = controler + 'AbrirBalanceEnergia';
                }
                else if (result == "-1") {
                    mostrarError();
                }
                else {
                    alert(result);
                }
            },
            error: function () {
                alert("Lo sentimos, se ha producido un error");
            }
        });
    }
}

exportarDesviacionRetiros = function () {
    var pericodi = $("#PERICODI4 option:selected").val();
    if (pericodi == "") {
        alert('Por favor, seleccione el Periodo');
    }
    else {
        $.ajax({
            type: 'POST',
            url: controler + 'exportarDesviacionRetiros',
            dataType: 'json',
            data: { iPeriCodi: pericodi },
            success: function (result) {
                if (result == "1") {
                    window.location = controler + 'AbrirDesviacionRetiros';
                }
                else if (result == "-1") {
                    mostrarError();
                }
                else {
                    alert(result);
                }
            },
            error: function () {
                alert("Lo sentimos, se ha producido un error");
            }
        });
    }
}
//ASSETEC 20190104
exportarDesviacionEntregas = function () {
    var pericodi = $("#PERICODI4 option:selected").val();
    var version = $("#VersionB option:selected").val();
    if (pericodi == "" || version == "") {
        alert('Por favor, seleccione el Periodo y la versión');
    }
    else {
        $.ajax({
            type: 'POST',
            url: controler + 'exportarDesviacionEntregas',
            dataType: 'json',
            data: { iPeriCodi: pericodi, iVersion: version },
            success: function (result) {
                if (result == "1") {
                    window.location = controler + 'AbrirDesviacionEntregas';
                }
                else if (result == "-1") {
                    mostrarError();
                }
                else {
                    alert(result);
                }
            },
            error: function () {
                alert("Lo sentimos, se ha producido un error");
            }
        });
    }
}

//Paso 5 - Exportar Reporte Mensual
exportarHistoricoEntregasRetiros = function () {
    var pericodiini = $("#PERICODI5 option:selected").val();
    var pericodifin = $("#PERICODI6 option:selected").val();
    var tipo = $("input[name=rdbtipo]:checked").val();
    var codigo = $('#txtcodigo').val();
    if (tipo == 'T')
        tipo = null;
    if (pericodiini == "" || pericodifin == "") {
        alert('Por favor, seleccione el Periodo de Inicio y Fin');
    }
    else if (pericodiini > pericodifin) {
        alert('Por favor, el Periodo de Inicio debe ser menor o igual al Periodo Final');
    }
    else {
        $.ajax({
            type: 'POST',
            url: controler + 'exportarHistoricoEntregasRetiros',
            dataType: 'json',
            data: { pericodiini: pericodiini, pericodifin: pericodifin, tipo: tipo, codigo: codigo },
            success: function (result) {
                if (result == "1") {
                    window.location = controler + 'AbrirHistoricoEntregasRetiros';
                }
                else if (result == "-1") {
                    mostrarError();
                }
                else {
                    alert(result);
                }
            },
            error: function () {
                alert("Lo sentimos, se ha producido un error");
            }
        });
    }
}

exportarHistorico15minCodigoEntregaRetiro = function () {
    var pericodiini = $("#PERICODI5 option:selected").val();
    var pericodifin = $("#PERICODI6 option:selected").val();
    var tipo = $("input[name=rdbtipo]:checked").val();
    var codigo = $('#txtcodigo').val();
    if (pericodiini == "" || pericodifin == "") {
        alert('Por favor, seleccione el Periodo de Inicio y Fin');
    }
    else if (pericodiini > pericodifin) {
        alert('Por favor, el Periodo de Inicio debe ser menor o igual al Periodo Final');
    }
    else if (tipo == 'T') {
        alert('Por favor, seleccione el Tipo Entrea o Retiro');
    }
    else if (codigo == "") {
        alert('Por favor, registrar el código');
    }
    else {
        $.ajax({
            type: 'POST',
            url: controler + 'exportarHistorico15minCodigoEntregaRetiro',
            dataType: 'json',
            data: { pericodiini: pericodiini, pericodifin: pericodifin, tipo: tipo, codigo: codigo },
            success: function (result) {
                if (result == "1") {
                    window.location = controler + 'AbrirHistorico15minCodigoEntregaRetiro';
                }
                else if (result == "-1") {
                    mostrarError();
                }
                else {
                    alert(result);
                }
            },
            error: function () {
                alert("Lo sentimos, se ha producido un error");
            }
        });
    }
}

//Paso 6 - Migracion TIEE
MigrarSaldo = function () {
    $.ajax({
        type: 'POST',
        url: controler + 'migrarsaldo',
        data: { pericodi: $('#pericodiSaldo').val(), recpotcodi: $('#recpotcodiSaldo').val(), migracodi: $('#migracodi').val() },
        dataType: 'json',
        success: function (result) {
            if (result == "1") {
                alert('Listo...! Usted puede volver a ejecutar el proceso de valorización');
            }
            else {
                alert(result);
            }
        },
        error: function () {
            mostrarError("Lo sentimos, se ha producido un error");
        }
    });
}

MigrarVTEA = function () {
    $.ajax({
        type: 'POST',
        url: controler + 'migrarvtea',
        data: { pericodi: $('#pericodiSaldo').val(), recpotcodi: $('#recpotcodiSaldo').val(), migracodi: $('#migracodi').val() },
        dataType: 'json',
        success: function (result) {
            if (result == "1") {
                alert('Listo...! Por favor, verificar la información en INGRESO DE DATOS [Ingreso por potencia, Factor de proporción, etc.] y luego puede volver a ejecutar el proceso de valorización');
            }
            else {
                alert(result);
            }
        },
        error: function () {
            mostrarError("Lo sentimos, se ha producido un error");
        }
    });
}