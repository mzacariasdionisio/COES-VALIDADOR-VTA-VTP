var controlador = siteRoot + "transferencias/envioinformacion/";
var series = [];
var seriesB = [];
var labels = [];
var series4 = [];
var seriesB4 = [];
var labels4 = [];
var error = [];
$(document).ready(function () {
    $('#EMPRCODI3').multipleSelect({
        width: '200px',
        filter: true,
        selectAll: false,
        single: true
    });

    $('#EMPRCODI4').multipleSelect({
        width: '200px',
        filter: true,
        selectAll: false,
        single: true
    });

    document.getElementById('divOpcionCarga').style.display = "none";
    document.getElementById('listado').style.display = "none";
    document.getElementById('divOpcionMensaje').style.display = "none";
    $('#btnSelecionarExcel').css('display', 'none');
    $('#btnGrabarExcel').css('display', 'none');
    $('#btnValidarGrillaExcel').css('display', 'none');

    $("#PERICODI").change(function () {
        if ($("#PERICODI").val() != "") {
            var options = {};
            options.url = controlador + "GetPermiso";
            options.type = "POST";
            options.data = JSON.stringify({ pericodi: $("#PERICODI").val() });
            options.dataType = "json";
            options.contentType = "application/json";
            options.success = function (modelo) {
                if (modelo.bEjecutar == true) {
                    buscarEnvios($("#PERICODI").val());
                    document.getElementById('divOpcionCarga').style.display = "block";
                    document.getElementById('listado').style.display = "block";
                    document.getElementById('divOpcionMensaje').style.display = "none";
                }
                else {
                    document.getElementById('divOpcionCarga').style.display = "none";
                    document.getElementById('listado').style.display = "none";
                    document.getElementById('divOpcionMensaje').style.display = "block";
                }
            };
            options.error = function () { alert("Lo sentimos, no se encuentran registrada ninguna revisión"); };
            $.ajax(options);
        }
        else {
            document.getElementById('divOpcionCarga').style.display = "none";
            document.getElementById('listado').style.display = "none";
            document.getElementById('divOpcionMensaje').style.display = "none";
        }
    });

    $("#Version").prop("disabled", true);
    $("#PERICODI3").change(function () {
        if ($("#PERICODI3").val() != "") {
            var options = {};
            options.url = controlador + "GetVersion";
            options.type = "POST";
            options.data = JSON.stringify({ pericodi: $("#PERICODI3").val() });
            options.dataType = "json";
            options.contentType = "application/json";
            options.success = function (ListaRecalculo) {
                //console.log(ListaRecalculo);
                $("#Version").empty();
                $("#Version").removeAttr("disabled");
                $.each(ListaRecalculo, function (k, v) {
                    var option = '<option value=' + v.RecaCodi + '>' + v.RecaNombre + '</option>';
                    $('#Version').append(option);
                    //console.log(option);
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

    $("#Version4").prop("disabled", true);
    $("#PERICODI4").change(function () {
        if ($("#PERICODI4").val() != "") {
            var options = {};
            options.url = controlador + "GetVersion";
            options.type = "POST";
            options.data = JSON.stringify({ pericodi: $("#PERICODI4").val() });
            options.dataType = "json";
            options.contentType = "application/json";
            options.success = function (ListaRecalculo) {
                //console.log(ListaRecalculo);
                $("#Version4").empty();
                $("#Version4").removeAttr("disabled");
                $.each(ListaRecalculo, function (k, v) {
                    var option = '<option value=' + v.RecaCodi + '>' + v.RecaNombre + '</option>';
                    $('#Version4').append(option);
                    //console.log(option);
                });
            };
            options.error = function () { alert("Lo sentimos, no se encuentran registrada ninguna revisión"); };
            $.ajax(options);
        }
        else {
            $("#Version4").empty();
            $("#Version4").prop("disabled", true);
        }
    });

    $("#VersionC").prop("disabled", true);
    $("#PericodiC").change(function () {
        if ($("#PericodiC").val() != "") {
            var options = {};
            options.url = controlador + "GetVersion";
            options.type = "POST";
            options.data = JSON.stringify({ pericodi: $("#PericodiC").val() });
            options.dataType = "json";
            options.contentType = "application/json";
            options.success = function (ListaRecalculo) {
                ////console.log(ListaRecalculo);
                $("#VersionC").empty();
                $("#VersionC").removeAttr("disabled");
                $.each(ListaRecalculo, function (k, v) {
                    var option = '<option value=' + v.RecaCodi + '>' + v.RecaNombre + '</option>';
                    $('#VersionC').append(option);
                    ////console.log(option);
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

    $('#tab-container').easytabs({
        animate: false
    });

    $('#btnGenerarExcel').click(function () {
        generarExcel();
    });

    $('#btnGenerarExcelBase').click(function () {
        generarExcelBase();
    });

    $('#btnProcesarFile').click(function () {
        procesarArchivo();
    });

    $('#btnVer').click(function () {
        document.getElementById("divResultado").style.display = "none";
        LimpiarGrafico();
        listaEntregaRetiro();
    });

    $('#Clean').click(function () {
        LimpiarGrafico();
    });

    $('#btnVer4').click(function () {
        document.getElementById("divResultado4").style.display = "none";
        LimpiarGrafico4();
        listaEntregaRetiro4();
    });

    $('#Clean4').click(function () {
        LimpiarGrafico4();
    });

    $('#btnDescargarEntregaRetiro').click(function () {
        descargarEntregaRetiro();
    });

    $('#btnDescargarInfoBase').click(function () {
        descargarInfoBase();
    });

    $('#btnDescargarEnergiaMensual').click(function () {
        descargarEnergiaMensual();
    });

    /*ASSETEC 202001*/
    $('#btnConsultar').click(function () {
        mostrarAlerta("Espere un momento, se esta procesando su solicitud");
        mostrarGrillaExcel(0);
        //if ($('#trnenvcodi').val() == 0) { mostrarMensaje('Por favor, registrar la información de Egresos y Peajes'); }
    });

    $('#btnDescargarExcel').click(function () {
        mostrarAlerta("Espere un momento, se esta procesando su solicitud");
        descargarArchivo(1);
    });

    $('#btnEliminarDatos').click(function () {
        if (confirm("¿Desea eliminar la información de la hoja de cálculo?")) {
            mostrarAlerta("Espere un momento, se esta procesando su solicitud");
            eliminarDatos();
        }
    });

    $('#btnValidarGrillaExcel').click(function () {
        mostrarAlerta("Espere un momento, se esta procesando su solicitud");
        validarCeldasInvisibles();
        if (parseInt(error.length) > 0) {
            abrirPopupErrores();
        } else {
            mostrarExito("La hoja de cálculo no tiene errores...!");
        }
    });

    $('#btnGrabarExcel').click(function () {
        mostrarAlerta("Espere un momento, se esta validando la información");
        if (parseInt(error.length) > 0) {
            abrirPopupErrores();
        } else {
            grabarExcel();
        }
    });

    $('#btnVerEnvios').click(function () {
        verEnviosAnteriores();
    });

    if (($('#pericodi').val() > 0) && ($('#recacodi').val() > 0)) {
        mostrarAlerta("Espere un momento, se esta procesando la última información registrada");
        mostrarGrillaExcel($('#trnenvcodi').val());
    }

    UploadExcel();
});

loadInfoFile = function (fileName, fileSize) {
    $('#fileInfo').html(fileName + " (" + fileSize + ")");
}

loadValidacionFile = function (mensaje) {
    $('#fileInfo').html(mensaje);
}

mostrarProgreso = function (porcentaje) {
    $('#progreso').text(porcentaje + "%");
}

mostrarError = function () {
    alert("Ha ocurrido un error.");
}

mostrarMensaje = function (mensaje) {
    alert(mensaje);

}

generarExcel = function () {
    if ($("#PERIANIOMES option:selected").val() == "") {
        alert('Seleccione un periodo');
    }
    else {
        $.ajax({
            type: 'POST',
            url: controlador + 'generarexcel',
            dataType: 'json',
            data: { periodo: $("#PERIANIOMES option:selected").val() },
            success: function (result) {
                if (result == "1") {
                    window.location = controlador + 'abrirexcel';
                }
                else if (result == "-1") {
                    mostrarMensaje("Lo sentimos, se ha producido un error en su solicitud");
                }
                else {
                    mostrarMensaje(result);
                }
            },
            error: function () {
                mostrarError();
            }
        });

    }
}

generarExcelBase = function () {
    if ($("#PERIANIOMES option:selected").val() == "") {
        alert('Seleccione un periodo');
    }
    else {
        $.ajax({
            type: 'POST',
            url: controlador + 'generarexcelbase',
            dataType: 'json',
            data: { periodo: $("#PERIANIOMES option:selected").val() },
            success: function (result) {
                if (result == "1") {
                    window.location = controlador + 'abrirexcelbase';
                }
                else if (result == "-1") {
                    mostrarMensaje("Lo sentimos, se ha producido un error en su solicitud");
                }
                else {
                    mostrarMensaje(result);
                }
            },
            error: function () {
                mostrarError();
            }
        });

    }
}

listaEntregaRetiro = function () {
    var iBarrcodi;
    var iEmprCodi;

    if ($("#PERICODI3 option:selected").val() == "") {
        alert("Por favor, seleccione un Periodo...");
    }
    else {
        iPeriCodi = $("#PERICODI3 option:selected").val();
        iVersion = $("#Version option:selected").val();
        document.getElementById("divResultado").style.display = "";
        if ($("#BARRCODI3").val() == "") {
            iBarrcodi = 0;
        }
        else {
            iBarrcodi = $("#BARRCODI3").val();
        }
        if ($("#EMPRCODI3 option:selected").val() === undefined) {
            iEmprCodi = 0;
        }
        else if ($("#EMPRCODI3 option:selected").val() == "") {
            iEmprCodi = 0;
        }
        else {
            iEmprCodi = $("#EMPRCODI3 option:selected").val();
        }
        $.ajax({
            url: controlador + 'getListRetiroEntrega',
            type: 'GET',
            data: { periodo: iPeriCodi, emprcodi: iEmprCodi, version: iVersion, barrcodi: iBarrcodi },
            success: function (result) {
                lista(result.entr, result.reti);
            },
            error: function () {
                alert('error a listar los resultados')
            }
        });
        $("#tabla").find("tr:gt(0)").remove();
        function lista(entr, reti) {
            //Lista de Entregas
            $.each(entr, function (k, v) {
                var sListaEntrega = "";
                sListaEntrega += "<tr>";
                sListaEntrega += '<td style="text-align:left"><a href=# id=view_' + v.CodiEntrCodigo + ' ' + 'class=Vergrafico' + '><img  src="~/Areas/Transferencias/Content/Images/view.gif" title="Insertar linea en el grafico" alt="Insertar linea en el grafico" /></a></td>';
                sListaEntrega += '<td style="text-align:left">' + v.CodiEntrCodigo + '</td>';
                sListaEntrega += '<td style="text-align:left">' + v.EmprNombre + '</td>';
                sListaEntrega += '<td style="text-align:left">' + v.CentGeneNombre + '</td>';
                sListaEntrega += '<td style="text-align:left">' + v.BarrNombre + ' </td>';
                sListaEntrega += '<td style="text-align:left">' + v.TranEntrTipoInformacion + ' </td>';
                sListaEntrega += '<td style="text-align:left">' + v.Total + '</td>';
                sListaEntrega += '<td style="text-align:left"><a style="display:box;float:right" href=# id=view_' + v.CodiEntrCodigo + ' ' + 'class=Del ><img  src="~/Areas/Transferencias/Content/Images/tachito.gif" title="Retirar linea del grafico" alt="Retirar linea del grafico" /></a></td>';
                sListaEntrega += '</tr>';
                $('#tabla').append(sListaEntrega);
            });
            //Lista de Retiros
            $.each(reti, function (k, v) {
                var sListaRetiro = '';
                sListaRetiro += '<tr>';
                sListaRetiro += '<td style="text-align:left"><a href=# id=view_' + v.SoliCodiRetiCodigo + ' ' + 'class=Vergrafico' + '><img  src="~/Areas/Transferencias/Content/Images/view.gif" title="Insertar linea en el grafico" alt="Insertar linea en el grafico" /></a></td>';
                sListaRetiro += '<td style="text-align:left">' + v.SoliCodiRetiCodigo + '</td>';
                sListaRetiro += '<td style="text-align:left">' + v.EmprNombre + '</td>';
                sListaRetiro += '<td style="text-align:left">' + v.CliNombre + '</td>';
                sListaRetiro += '<td style="text-align:left">' + v.BarrNombre + ' </td>';
                sListaRetiro += '<td style="text-align:left">' + v.TranRetiTipoInformacion + ' </td>';
                sListaRetiro += '<td style="text-align:left">' + v.Total + '</td>';
                sListaRetiro += '<td style="text-align:left"><a style="display:box;float:right" href=# id=view_' + v.SoliCodiRetiCodigo + ' ' + 'class=Del >' + '<img  src="~/Areas/Transferencias/Content/Images/tachito.gif" title="Retirar linea del grafico" alt="Retirar linea del grafico" />' + '</a></td>';
                sListaRetiro += '</tr>';
                $('#tabla').append(sListaRetiro);
            });
            add_Ver();
            Del();
        }
    }
}

listaEntregaRetiro4 = function () {
    var cbo = $("#PERICODI4 option:selected").val();

    if (cbo == "") {
        alert('Seleccione Periodo');
    }
    else {
        if ($("#EMPRCODI4").multipleSelect('getSelects') == "") {
            alert('Seleccione Empresa');
        }
        else {
            document.getElementById("divResultado4").style.display = "";
            peri = $("#PERICODI4 option:selected").val();
            ver = $("#Version4 option:selected").val();
            var empr = $("#EMPRCODI4 option:selected").val();
            $.ajax({
                url: controlador + 'getListInfoBase',
                type: 'GET',
                data: { periodo: peri, emprcodi: empr, version: ver },
                success: function (result) {
                    lista(result.infobase);
                },
                error: function () {
                    alert('error a listar los resultados')
                }
            });

            $("#tabla4").find("tr:gt(0)").remove();
            function lista(infobase) {
                //Lista de Entregas
                $.each(infobase, function (k, v) {

                    var sListaEntrega = "";
                    sListaEntrega += "<tr>";
                    sListaEntrega += '<td style="text-align:left"><a href=# id=view_' + v.TinfbCodigo + ' ' + 'class=Vergrafico4' + '><img  src="~/Areas/Transferencias/Content/Images/view.gif" title="Insertar linea en el grafico" alt="Insertar linea en el grafico" /></a></td>';
                    sListaEntrega += '<td style="text-align:left">' + v.TinfbCodigo + '</td>';
                    sListaEntrega += '<td style="text-align:left">' + v.EmprNombre + '</td>';
                    sListaEntrega += '<td style="text-align:left">' + v.CentGeneNombre + '</td>';
                    sListaEntrega += '<td style="text-align:left">' + v.BarrNombre + ' </td>';
                    sListaEntrega += '<td style="text-align:left">' + v.TinfbTipoInformacion + ' </td>';
                    sListaEntrega += '<td style="text-align:left">' + v.Total + '</td>';
                    sListaEntrega += '<td style="text-align:left"><a style="display:box;float:right" href=# id=view_' + v.TinfbCodigo + ' ' + 'class=Del4 ><img  src="~/Areas/Transferencias/Content/Images/tachito.gif" title="Retirar linea del grafico" alt="Retirar linea del grafico" /></a></td>';
                    sListaEntrega += '</tr>';
                    $('#tabla4').append(sListaEntrega);
                });
                add_Ver4();
                Del4();
            }
        }
    }
}

//Funciones llamar ver grafico
function add_Ver() {
    var iEmprCodi;
    if ($("#PERICODI3 option:selected").val() == "") {
        alert("Por favor, seleccione un Periodo");
    }
    else {
        if ($("#EMPRCODI3 option:selected").val() === undefined) {
            iEmprCodi = 0;
        }
        else if ($("#EMPRCODI3 option:selected").val() == "") {
            iEmprCodi = 0;
        }
        else {
            iEmprCodi = $("#EMPRCODI3 option:selected").val();
        }

        $(".Vergrafico").click(function (e) {
            document.getElementById('btnBotones').style.display = "block";
            e.preventDefault();
            id = $(this).attr("id").split("_")[1];
            peri = $("#PERICODI3 option:selected").val();
            empresa = iEmprCodi;
            $.ajax({
                type: "GET",
                dataType: "json",
                url: controlador + "FetchGraphData/",
                data:
                {
                    periodo: peri,
                    codigoER: id,
                    empr: empresa
                },
                success: function (result) {
                    if ($.isEmptyObject(result.dataER)) {
                        alert("No hay información");
                    } else {
                        testJqPlot(result.dataER, result.dataCodigo, result.tipo);
                    }
                },
                error: function () {
                    mostrarError();
                }
            });

            function testJqPlot(dataER, dataCodigo, tipo) {
                //console.log(dataER)
                var line1 = [];
                var existe = 0;
                var cont = 1440;
                var line2 = [];
                //var contDia = cont / 60;

                for (var key in labels) {
                    var a = labels[key];
                    if (a.label == (dataCodigo))
                        existe = 1;
                }
                if (existe == 1) {
                    alert('El registro ya fue previamente seleccionado');
                }
                else {
                    if (tipo == "E") {
                        $.each(dataER, function (index, item) {

                            for (var prop in item) {

                                if (prop == 'TranEntrDetah1') {
                                    var contDia = cont / 1440; line1.push([contDia, item[prop]]);
                                    cont += 15;
                                }
                                if (prop == 'TranEntrDetah2') {

                                    var contDia = cont / 1440; line1.push([contDia, item[prop]]);
                                    cont += 15;

                                }
                                if (prop == 'TranEntrDetah3') {

                                    var contDia = cont / 1440; line1.push([contDia, item[prop]]);
                                    cont += 15;

                                }
                                if (prop == 'TranEntrDetah4') {
                                    var contDia = cont / 1440; line1.push([contDia, item[prop]]);
                                    cont += 15;
                                }
                                if (prop == 'TranEntrDetah5') {
                                    var contDia = cont / 1440; line1.push([contDia, item[prop]]);
                                    cont += 15;
                                }
                                if (prop == 'TranEntrDetah6') {
                                    var contDia = cont / 1440; line1.push([contDia, item[prop]]);
                                    cont += 15;
                                }
                                if (prop == 'TranEntrDetah7') {
                                    var contDia = cont / 1440; line1.push([contDia, item[prop]]);
                                    cont += 15;
                                }
                                if (prop == 'TranEntrDetah8') {
                                    var contDia = cont / 1440; line1.push([contDia, item[prop]]);
                                    cont += 15;
                                }
                                if (prop == 'TranEntrDetah9') {
                                    var contDia = cont / 1440; line1.push([contDia, item[prop]]);
                                    cont += 15;
                                }

                                if (prop == 'TranEntrDetah10') {
                                    var contDia = cont / 1440; line1.push([contDia, item[prop]]);
                                    cont += 15;
                                }
                                if (prop == 'TranEntrDetah11') {

                                    var contDia = cont / 1440; line1.push([contDia, item[prop]])
                                    cont += 15;
                                }
                                if (prop == 'TranEntrDetah12') {

                                    var contDia = cont / 1440; line1.push([contDia, item[prop]])
                                    cont += 15;
                                }
                                if (prop == 'TranEntrDetah13') {

                                    var contDia = cont / 1440; line1.push([contDia, item[prop]])
                                    cont += 15;
                                }
                                if (prop == 'TranEntrDetah14') {

                                    var contDia = cont / 1440; line1.push([contDia, item[prop]])
                                    cont += 15;
                                }
                                if (prop == 'TranEntrDetah15') {

                                    var contDia = cont / 1440; line1.push([contDia, item[prop]])
                                    cont += 15;
                                }
                                if (prop == 'TranEntrDetah16') {

                                    var contDia = cont / 1440; line1.push([contDia, item[prop]])
                                    cont += 15;
                                }
                                if (prop == 'TranEntrDetah17') {

                                    var contDia = cont / 1440; line1.push([contDia, item[prop]])
                                    cont += 15;
                                }
                                if (prop == 'TranEntrDetah18') {

                                    var contDia = cont / 1440; line1.push([contDia, item[prop]])
                                    cont += 15;
                                }
                                if (prop == 'TranEntrDetah19') {

                                    var contDia = cont / 1440; line1.push([contDia, item[prop]])
                                    cont += 15;
                                }
                                if (prop == 'TranEntrDetah20') {

                                    var contDia = cont / 1440; line1.push([contDia, item[prop]])
                                    cont += 15;
                                }
                                if (prop == 'TranEntrDetah21') {

                                    var contDia = cont / 1440; line1.push([contDia, item[prop]])
                                    cont += 15;
                                }
                                if (prop == 'TranEntrDetah22') {

                                    var contDia = cont / 1440; line1.push([contDia, item[prop]])
                                    cont += 15;
                                }
                                if (prop == 'TranEntrDetah23') {

                                    var contDia = cont / 1440; line1.push([contDia, item[prop]])
                                    cont += 15;
                                }
                                if (prop == 'TranEntrDetah24') {

                                    var contDia = cont / 1440; line1.push([contDia, item[prop]])
                                    cont += 15;
                                }
                                if (prop == 'TranEntrDetah25') {

                                    var contDia = cont / 1440; line1.push([contDia, item[prop]])
                                    cont += 15;
                                }
                                if (prop == 'TranEntrDetah26') {

                                    var contDia = cont / 1440; line1.push([contDia, item[prop]])
                                    cont += 15;
                                }
                                if (prop == 'TranEntrDetah27') {

                                    var contDia = cont / 1440; line1.push([contDia, item[prop]])
                                    cont += 15;
                                }
                                if (prop == 'TranEntrDetah28') {

                                    var contDia = cont / 1440; line1.push([contDia, item[prop]])
                                    cont += 15;
                                }
                                if (prop == 'TranEntrDetah29') {

                                    var contDia = cont / 1440; line1.push([contDia, item[prop]])
                                    cont += 15;
                                }
                                if (prop == 'TranEntrDetah30') {

                                    var contDia = cont / 1440; line1.push([contDia, item[prop]])
                                    cont += 15;
                                }
                                if (prop == 'TranEntrDetah31') {

                                    var contDia = cont / 1440; line1.push([contDia, item[prop]])
                                    cont += 15;
                                }
                                if (prop == 'TranEntrDetah32') {

                                    var contDia = cont / 1440; line1.push([contDia, item[prop]])
                                    cont += 15;
                                }
                                if (prop == 'TranEntrDetah33') {

                                    var contDia = cont / 1440; line1.push([contDia, item[prop]])
                                    cont += 15;
                                }
                                if (prop == 'TranEntrDetah34') {

                                    var contDia = cont / 1440; line1.push([contDia, item[prop]])
                                    cont += 15;
                                }
                                if (prop == 'TranEntrDetah35') {

                                    var contDia = cont / 1440; line1.push([contDia, item[prop]])
                                    cont += 15;
                                }
                                if (prop == 'TranEntrDetah36') {

                                    var contDia = cont / 1440; line1.push([contDia, item[prop]])
                                    cont += 15;
                                }
                                if (prop == 'TranEntrDetah37') {

                                    var contDia = cont / 1440; line1.push([contDia, item[prop]])
                                    cont += 15;
                                }
                                if (prop == 'TranEntrDetah38') {

                                    var contDia = cont / 1440; line1.push([contDia, item[prop]])
                                    cont += 15;
                                }
                                if (prop == 'TranEntrDetah39') {

                                    var contDia = cont / 1440; line1.push([contDia, item[prop]])
                                    cont += 15;
                                }
                                if (prop == 'TranEntrDetah40') {

                                    var contDia = cont / 1440; line1.push([contDia, item[prop]])
                                    cont += 15;
                                }
                                if (prop == 'TranEntrDetah41') {

                                    var contDia = cont / 1440; line1.push([contDia, item[prop]])
                                    cont += 15;
                                }
                                if (prop == 'TranEntrDetah42') {

                                    var contDia = cont / 1440; line1.push([contDia, item[prop]])
                                    cont += 15;
                                }
                                if (prop == 'TranEntrDetah43') {

                                    var contDia = cont / 1440; line1.push([contDia, item[prop]])
                                    cont += 15;
                                }
                                if (prop == 'TranEntrDetah44') {

                                    var contDia = cont / 1440; line1.push([contDia, item[prop]])
                                    cont += 15;
                                }
                                if (prop == 'TranEntrDetah45') {

                                    var contDia = cont / 1440; line1.push([contDia, item[prop]])
                                    cont += 15;
                                }
                                if (prop == 'TranEntrDetah46') {

                                    var contDia = cont / 1440; line1.push([contDia, item[prop]])
                                    cont += 15;
                                }
                                if (prop == 'TranEntrDetah47') {

                                    var contDia = cont / 1440; line1.push([contDia, item[prop]])
                                    cont += 15;
                                }
                                if (prop == 'TranEntrDetah48') {

                                    var contDia = cont / 1440; line1.push([contDia, item[prop]])
                                    cont += 15;
                                }
                                if (prop == 'TranEntrDetah49') {

                                    var contDia = cont / 1440; line1.push([contDia, item[prop]])
                                    cont += 15;
                                }
                                if (prop == 'TranEntrDetah50') {

                                    var contDia = cont / 1440; line1.push([contDia, item[prop]])
                                    cont += 15;
                                }
                                if (prop == 'TranEntrDetah51') {

                                    var contDia = cont / 1440; line1.push([contDia, item[prop]])
                                    cont += 15;
                                }
                                if (prop == 'TranEntrDetah52') {

                                    var contDia = cont / 1440; line1.push([contDia, item[prop]])
                                    cont += 15;
                                }
                                if (prop == 'TranEntrDetah53') {

                                    var contDia = cont / 1440; line1.push([contDia, item[prop]])
                                    cont += 15;
                                }
                                if (prop == 'TranEntrDetah54') {

                                    var contDia = cont / 1440; line1.push([contDia, item[prop]])
                                    cont += 15;
                                }
                                if (prop == 'TranEntrDetah55') {

                                    var contDia = cont / 1440; line1.push([contDia, item[prop]])
                                    cont += 15;
                                }
                                if (prop == 'TranEntrDetah56') {

                                    var contDia = cont / 1440; line1.push([contDia, item[prop]])
                                    cont += 15;
                                }
                                if (prop == 'TranEntrDetah57') {

                                    var contDia = cont / 1440; line1.push([contDia, item[prop]])
                                    cont += 15;
                                }
                                if (prop == 'TranEntrDetah58') {

                                    var contDia = cont / 1440; line1.push([contDia, item[prop]])
                                    cont += 15;
                                }
                                if (prop == 'TranEntrDetah59') {

                                    var contDia = cont / 1440; line1.push([contDia, item[prop]])
                                    cont += 15;
                                }
                                if (prop == 'TranEntrDetah60') {

                                    var contDia = cont / 1440; line1.push([contDia, item[prop]])
                                    cont += 15;
                                }
                                if (prop == 'TranEntrDetah61') {

                                    var contDia = cont / 1440; line1.push([contDia, item[prop]])
                                    cont += 15;
                                }
                                if (prop == 'TranEntrDetah62') {

                                    var contDia = cont / 1440; line1.push([contDia, item[prop]])
                                    cont += 15;
                                }
                                if (prop == 'TranEntrDetah63') {

                                    var contDia = cont / 1440; line1.push([contDia, item[prop]])
                                    cont += 15;
                                }
                                if (prop == 'TranEntrDetah64') {

                                    var contDia = cont / 1440; line1.push([contDia, item[prop]])
                                    cont += 15;
                                }
                                if (prop == 'TranEntrDetah65') {

                                    var contDia = cont / 1440; line1.push([contDia, item[prop]])
                                    cont += 15;
                                }
                                if (prop == 'TranEntrDetah66') {

                                    var contDia = cont / 1440; line1.push([contDia, item[prop]])
                                    cont += 15;
                                }
                                if (prop == 'TranEntrDetah67') {

                                    var contDia = cont / 1440; line1.push([contDia, item[prop]])
                                    cont += 15;
                                }
                                if (prop == 'TranEntrDetah68') {

                                    var contDia = cont / 1440; line1.push([contDia, item[prop]])
                                    cont += 15;
                                }
                                if (prop == 'TranEntrDetah69') {

                                    var contDia = cont / 1440; line1.push([contDia, item[prop]])
                                    cont += 15;
                                }
                                if (prop == 'TranEntrDetah70') {

                                    var contDia = cont / 1440; line1.push([contDia, item[prop]])
                                    cont += 15;
                                }
                                if (prop == 'TranEntrDetah71') {

                                    var contDia = cont / 1440; line1.push([contDia, item[prop]])
                                    cont += 15;
                                }
                                if (prop == 'TranEntrDetah72') {

                                    var contDia = cont / 1440; line1.push([contDia, item[prop]])
                                    cont += 15;
                                }
                                if (prop == 'TranEntrDetah73') {

                                    var contDia = cont / 1440; line1.push([contDia, item[prop]])
                                    cont += 15;
                                }
                                if (prop == 'TranEntrDetah74') {

                                    var contDia = cont / 1440; line1.push([contDia, item[prop]])
                                    cont += 15;
                                }
                                if (prop == 'TranEntrDetah75') {

                                    var contDia = cont / 1440; line1.push([contDia, item[prop]])
                                    cont += 15;
                                }
                                if (prop == 'TranEntrDetah76') {

                                    var contDia = cont / 1440; line1.push([contDia, item[prop]])
                                    cont += 15;
                                }
                                if (prop == 'TranEntrDetah77') {

                                    var contDia = cont / 1440; line1.push([contDia, item[prop]])
                                    cont += 15;
                                }
                                if (prop == 'TranEntrDetah78') {

                                    var contDia = cont / 1440; line1.push([contDia, item[prop]])
                                    cont += 15;
                                }
                                if (prop == 'TranEntrDetah79') {

                                    var contDia = cont / 1440; line1.push([contDia, item[prop]])
                                    cont += 15;
                                }
                                if (prop == 'TranEntrDetah80') {

                                    var contDia = cont / 1440; line1.push([contDia, item[prop]])
                                    cont += 15;
                                }
                                if (prop == 'TranEntrDetah81') {

                                    var contDia = cont / 1440; line1.push([contDia, item[prop]])
                                    cont += 15;
                                }
                                if (prop == 'TranEntrDetah82') {

                                    var contDia = cont / 1440; line1.push([contDia, item[prop]])
                                    cont += 15;
                                }
                                if (prop == 'TranEntrDetah83') {

                                    var contDia = cont / 1440; line1.push([contDia, item[prop]])
                                    cont += 15;
                                }
                                if (prop == 'TranEntrDetah84') {

                                    var contDia = cont / 1440; line1.push([contDia, item[prop]])
                                    cont += 15;
                                }
                                if (prop == 'TranEntrDetah85') {

                                    var contDia = cont / 1440; line1.push([contDia, item[prop]])
                                    cont += 15;
                                }
                                if (prop == 'TranEntrDetah86') {

                                    var contDia = cont / 1440; line1.push([contDia, item[prop]])
                                    cont += 15;
                                }
                                if (prop == 'TranEntrDetah87') {

                                    var contDia = cont / 1440; line1.push([contDia, item[prop]])
                                    cont += 15;
                                }
                                if (prop == 'TranEntrDetah88') {

                                    var contDia = cont / 1440; line1.push([contDia, item[prop]])
                                    cont += 15;
                                }
                                if (prop == 'TranEntrDetah89') {

                                    var contDia = cont / 1440; line1.push([contDia, item[prop]])
                                    cont += 15;
                                }
                                if (prop == 'TranEntrDetah90') {

                                    var contDia = cont / 1440; line1.push([contDia, item[prop]])
                                    cont += 15;
                                }
                                if (prop == 'TranEntrDetah91') {

                                    var contDia = cont / 1440; line1.push([contDia, item[prop]])
                                    cont += 15;
                                }
                                if (prop == 'TranEntrDetah92') {

                                    var contDia = cont / 1440; line1.push([contDia, item[prop]])
                                    cont += 15;
                                }
                                if (prop == 'TranEntrDetah93') {

                                    var contDia = cont / 1440; line1.push([contDia, item[prop]])
                                    cont += 15;
                                }
                                if (prop == 'TranEntrDetah94') {

                                    var contDia = cont / 1440; line1.push([contDia, item[prop]])
                                    cont += 15;
                                }
                                if (prop == 'TranEntrDetah95') {

                                    var contDia = cont / 1440; line1.push([contDia, item[prop]])
                                    cont += 15;
                                }
                                if (prop == 'TranEntrDetah96') {

                                    var contDia = cont / 1440; line1.push([contDia, item[prop]])
                                    cont += 15;
                                }
                                //console.log("o." + prop + " = " + item[prop] + " " + prop[prop.length -1]);
                            }
                            //}
                            //line1.push([item.TranEntrDetadia, item.TranEntrDetaPromDia])
                        });
                    }
                    else {
                        $.each(dataER, function (index, item) {
                            for (var prop in item) {

                                if (prop == 'TranRetiDetah1') {
                                    var contDia = cont / 1440; line1.push([contDia, item[prop]]);
                                    cont += 15;
                                }
                                if (prop == 'TranRetiDetah2') {
                                    var contDia = cont / 1440; line1.push([contDia, item[prop]]);
                                    cont += 15;
                                }
                                if (prop == 'TranRetiDetah3') {

                                    var contDia = cont / 1440; line1.push([contDia, item[prop]]);
                                    cont += 15;

                                }
                                if (prop == 'TranRetiDetah4') {
                                    var contDia = cont / 1440; line1.push([contDia, item[prop]]);
                                    cont += 15;
                                }
                                if (prop == 'TranRetiDetah5') {
                                    var contDia = cont / 1440; line1.push([contDia, item[prop]]);
                                    cont += 15;
                                }
                                if (prop == 'TranRetiDetah6') {
                                    var contDia = cont / 1440; line1.push([contDia, item[prop]]);
                                    cont += 15;
                                }
                                if (prop == 'TranRetiDetah7') {
                                    var contDia = cont / 1440; line1.push([contDia, item[prop]]);
                                    cont += 15;
                                }
                                if (prop == 'TranRetiDetah8') {
                                    var contDia = cont / 1440; line1.push([contDia, item[prop]]);
                                    cont += 15;
                                }
                                if (prop == 'TranRetiDetah9') {
                                    var contDia = cont / 1440; line1.push([contDia, item[prop]]);
                                    cont += 15;
                                }

                                if (prop == 'TranRetiDetah10') {
                                    var contDia = cont / 1440; line1.push([contDia, item[prop]]);
                                    cont += 15;
                                }
                                if (prop == 'TranRetiDetah11') {

                                    var contDia = cont / 1440; line1.push([contDia, item[prop]])
                                    cont += 15;
                                }
                                if (prop == 'TranRetiDetah12') {

                                    var contDia = cont / 1440; line1.push([contDia, item[prop]])
                                    cont += 15;
                                }
                                if (prop == 'TranRetiDetah13') {

                                    var contDia = cont / 1440; line1.push([contDia, item[prop]])
                                    cont += 15;
                                }
                                if (prop == 'TranRetiDetah14') {

                                    var contDia = cont / 1440; line1.push([contDia, item[prop]])
                                    cont += 15;
                                }
                                if (prop == 'TranRetiDetah15') {

                                    var contDia = cont / 1440; line1.push([contDia, item[prop]])
                                    cont += 15;
                                }
                                if (prop == 'TranRetiDetah16') {

                                    var contDia = cont / 1440; line1.push([contDia, item[prop]])
                                    cont += 15;
                                }
                                if (prop == 'TranRetiDetah17') {

                                    var contDia = cont / 1440; line1.push([contDia, item[prop]])
                                    cont += 15;
                                }
                                if (prop == 'TranRetiDetah18') {

                                    var contDia = cont / 1440; line1.push([contDia, item[prop]])
                                    cont += 15;
                                }
                                if (prop == 'TranRetiDetah19') {

                                    var contDia = cont / 1440; line1.push([contDia, item[prop]])
                                    cont += 15;
                                }
                                if (prop == 'TranRetiDetah20') {

                                    var contDia = cont / 1440; line1.push([contDia, item[prop]])
                                    cont += 15;
                                }
                                if (prop == 'TranRetiDetah21') {

                                    var contDia = cont / 1440; line1.push([contDia, item[prop]])
                                    cont += 15;
                                }
                                if (prop == 'TranRetiDetah22') {

                                    var contDia = cont / 1440; line1.push([contDia, item[prop]])
                                    cont += 15;
                                }
                                if (prop == 'TranRetiDetah23') {

                                    var contDia = cont / 1440; line1.push([contDia, item[prop]])
                                    cont += 15;
                                }
                                if (prop == 'TranRetiDetah24') {

                                    var contDia = cont / 1440; line1.push([contDia, item[prop]])
                                    cont += 15;
                                }
                                if (prop == 'TranRetiDetah25') {

                                    var contDia = cont / 1440; line1.push([contDia, item[prop]])
                                    cont += 15;
                                }
                                if (prop == 'TranRetiDetah26') {

                                    var contDia = cont / 1440; line1.push([contDia, item[prop]])
                                    cont += 15;
                                }
                                if (prop == 'TranRetiDetah27') {

                                    var contDia = cont / 1440; line1.push([contDia, item[prop]])
                                    cont += 15;
                                }
                                if (prop == 'TranRetiDetah28') {

                                    var contDia = cont / 1440; line1.push([contDia, item[prop]])
                                    cont += 15;
                                }
                                if (prop == 'TranRetiDetah29') {

                                    var contDia = cont / 1440; line1.push([contDia, item[prop]])
                                    cont += 15;
                                }
                                if (prop == 'TranRetiDetah30') {

                                    var contDia = cont / 1440; line1.push([contDia, item[prop]])
                                    cont += 15;
                                }
                                if (prop == 'TranRetiDetah31') {

                                    var contDia = cont / 1440; line1.push([contDia, item[prop]])
                                    cont += 15;
                                }
                                if (prop == 'TranRetiDetah32') {

                                    var contDia = cont / 1440; line1.push([contDia, item[prop]])
                                    cont += 15;
                                }
                                if (prop == 'TranRetiDetah33') {

                                    var contDia = cont / 1440; line1.push([contDia, item[prop]])
                                    cont += 15;
                                }
                                if (prop == 'TranRetiDetah34') {

                                    var contDia = cont / 1440; line1.push([contDia, item[prop]])
                                    cont += 15;
                                }
                                if (prop == 'TranRetiDetah35') {

                                    var contDia = cont / 1440; line1.push([contDia, item[prop]])
                                    cont += 15;
                                }
                                if (prop == 'TranRetiDetah36') {

                                    var contDia = cont / 1440; line1.push([contDia, item[prop]])
                                    cont += 15;
                                }
                                if (prop == 'TranRetiDetah37') {

                                    var contDia = cont / 1440; line1.push([contDia, item[prop]])
                                    cont += 15;
                                }
                                if (prop == 'TranRetiDetah38') {

                                    var contDia = cont / 1440; line1.push([contDia, item[prop]])
                                    cont += 15;
                                }
                                if (prop == 'TranRetiDetah39') {

                                    var contDia = cont / 1440; line1.push([contDia, item[prop]])
                                    cont += 15;
                                }
                                if (prop == 'TranRetiDetah40') {

                                    var contDia = cont / 1440; line1.push([contDia, item[prop]])
                                    cont += 15;
                                }
                                if (prop == 'TranRetiDetah41') {

                                    var contDia = cont / 1440; line1.push([contDia, item[prop]])
                                    cont += 15;
                                }
                                if (prop == 'TranRetiDetah42') {

                                    var contDia = cont / 1440; line1.push([contDia, item[prop]])
                                    cont += 15;
                                }
                                if (prop == 'TranRetiDetah43') {

                                    var contDia = cont / 1440; line1.push([contDia, item[prop]])
                                    cont += 15;
                                }
                                if (prop == 'TranRetiDetah44') {

                                    var contDia = cont / 1440; line1.push([contDia, item[prop]])
                                    cont += 15;
                                }
                                if (prop == 'TranRetiDetah45') {

                                    var contDia = cont / 1440; line1.push([contDia, item[prop]])
                                    cont += 15;
                                }
                                if (prop == 'TranRetiDetah46') {

                                    var contDia = cont / 1440; line1.push([contDia, item[prop]])
                                    cont += 15;
                                }
                                if (prop == 'TranRetiDetah47') {

                                    var contDia = cont / 1440; line1.push([contDia, item[prop]])
                                    cont += 15;
                                }
                                if (prop == 'TranRetiDetah48') {

                                    var contDia = cont / 1440; line1.push([contDia, item[prop]])
                                    cont += 15;
                                }
                                if (prop == 'TranRetiDetah49') {

                                    var contDia = cont / 1440; line1.push([contDia, item[prop]])
                                    cont += 15;
                                }
                                if (prop == 'TranRetiDetah50') {

                                    var contDia = cont / 1440; line1.push([contDia, item[prop]])
                                    cont += 15;
                                }
                                if (prop == 'TranRetiDetah51') {

                                    var contDia = cont / 1440; line1.push([contDia, item[prop]])
                                    cont += 15;
                                }
                                if (prop == 'TranRetiDetah52') {

                                    var contDia = cont / 1440; line1.push([contDia, item[prop]])
                                    cont += 15;
                                }
                                if (prop == 'TranRetiDetah53') {

                                    var contDia = cont / 1440; line1.push([contDia, item[prop]])
                                    cont += 15;
                                }
                                if (prop == 'TranRetiDetah54') {

                                    var contDia = cont / 1440; line1.push([contDia, item[prop]])
                                    cont += 15;
                                }
                                if (prop == 'TranRetiDetah55') {

                                    var contDia = cont / 1440; line1.push([contDia, item[prop]])
                                    cont += 15;
                                }
                                if (prop == 'TranRetiDetah56') {

                                    var contDia = cont / 1440; line1.push([contDia, item[prop]])
                                    cont += 15;
                                }
                                if (prop == 'TranRetiDetah57') {

                                    var contDia = cont / 1440; line1.push([contDia, item[prop]])
                                    cont += 15;
                                }
                                if (prop == 'TranRetiDetah58') {

                                    var contDia = cont / 1440; line1.push([contDia, item[prop]])
                                    cont += 15;
                                }
                                if (prop == 'TranRetiDetah59') {

                                    var contDia = cont / 1440; line1.push([contDia, item[prop]])
                                    cont += 15;
                                }
                                if (prop == 'TranRetiDetah60') {

                                    var contDia = cont / 1440; line1.push([contDia, item[prop]])
                                    cont += 15;
                                }
                                if (prop == 'TranRetiDetah61') {

                                    var contDia = cont / 1440; line1.push([contDia, item[prop]])
                                    cont += 15;
                                }
                                if (prop == 'TranRetiDetah62') {

                                    var contDia = cont / 1440; line1.push([contDia, item[prop]])
                                    cont += 15;
                                }
                                if (prop == 'TranRetiDetah63') {

                                    var contDia = cont / 1440; line1.push([contDia, item[prop]])
                                    cont += 15;
                                }
                                if (prop == 'TranRetiDetah64') {

                                    var contDia = cont / 1440; line1.push([contDia, item[prop]])
                                    cont += 15;
                                }
                                if (prop == 'TranRetiDetah65') {

                                    var contDia = cont / 1440; line1.push([contDia, item[prop]])
                                    cont += 15;
                                }
                                if (prop == 'TranRetiDetah66') {

                                    var contDia = cont / 1440; line1.push([contDia, item[prop]])
                                    cont += 15;
                                }
                                if (prop == 'TranRetiDetah67') {

                                    var contDia = cont / 1440; line1.push([contDia, item[prop]])
                                    cont += 15;
                                }
                                if (prop == 'TranRetiDetah68') {

                                    var contDia = cont / 1440; line1.push([contDia, item[prop]])
                                    cont += 15;
                                }
                                if (prop == 'TranRetiDetah69') {

                                    var contDia = cont / 1440; line1.push([contDia, item[prop]])
                                    cont += 15;
                                }
                                if (prop == 'TranRetiDetah70') {

                                    var contDia = cont / 1440; line1.push([contDia, item[prop]])
                                    cont += 15;
                                }
                                if (prop == 'TranRetiDetah71') {

                                    var contDia = cont / 1440; line1.push([contDia, item[prop]])
                                    cont += 15;
                                }
                                if (prop == 'TranRetiDetah72') {

                                    var contDia = cont / 1440; line1.push([contDia, item[prop]])
                                    cont += 15;
                                }
                                if (prop == 'TranRetiDetah73') {

                                    var contDia = cont / 1440; line1.push([contDia, item[prop]])
                                    cont += 15;
                                }
                                if (prop == 'TranRetiDetah74') {

                                    var contDia = cont / 1440; line1.push([contDia, item[prop]])
                                    cont += 15;
                                }
                                if (prop == 'TranRetiDetah75') {

                                    var contDia = cont / 1440; line1.push([contDia, item[prop]])
                                    cont += 15;
                                }
                                if (prop == 'TranRetiDetah76') {

                                    var contDia = cont / 1440; line1.push([contDia, item[prop]])
                                    cont += 15;
                                }
                                if (prop == 'TranRetiDetah77') {

                                    var contDia = cont / 1440; line1.push([contDia, item[prop]])
                                    cont += 15;
                                }
                                if (prop == 'TranRetiDetah78') {

                                    var contDia = cont / 1440; line1.push([contDia, item[prop]])
                                    cont += 15;
                                }
                                if (prop == 'TranRetiDetah79') {

                                    var contDia = cont / 1440; line1.push([contDia, item[prop]])
                                    cont += 15;
                                }
                                if (prop == 'TranRetiDetah80') {

                                    var contDia = cont / 1440; line1.push([contDia, item[prop]])
                                    cont += 15;
                                }
                                if (prop == 'TranRetiDetah81') {

                                    var contDia = cont / 1440; line1.push([contDia, item[prop]])
                                    cont += 15;
                                }
                                if (prop == 'TranRetiDetah82') {

                                    var contDia = cont / 1440; line1.push([contDia, item[prop]])
                                    cont += 15;
                                }
                                if (prop == 'TranRetiDetah83') {

                                    var contDia = cont / 1440; line1.push([contDia, item[prop]])
                                    cont += 15;
                                }
                                if (prop == 'TranRetiDetah84') {

                                    var contDia = cont / 1440; line1.push([contDia, item[prop]])
                                    cont += 15;
                                }
                                if (prop == 'TranRetiDetah85') {

                                    var contDia = cont / 1440; line1.push([contDia, item[prop]])
                                    cont += 15;
                                }
                                if (prop == 'TranRetiDetah86') {

                                    var contDia = cont / 1440; line1.push([contDia, item[prop]])
                                    cont += 15;
                                }
                                if (prop == 'TranRetiDetah87') {

                                    var contDia = cont / 1440; line1.push([contDia, item[prop]])
                                    cont += 15;
                                }
                                if (prop == 'TranRetiDetah88') {

                                    var contDia = cont / 1440; line1.push([contDia, item[prop]])
                                    cont += 15;
                                }
                                if (prop == 'TranRetiDetah89') {

                                    var contDia = cont / 1440; line1.push([contDia, item[prop]])
                                    cont += 15;
                                }
                                if (prop == 'TranRetiDetah90') {

                                    var contDia = cont / 1440; line1.push([contDia, item[prop]])
                                    cont += 15;
                                }
                                if (prop == 'TranRetiDetah91') {

                                    var contDia = cont / 1440; line1.push([contDia, item[prop]])
                                    cont += 15;
                                }
                                if (prop == 'TranRetiDetah92') {

                                    var contDia = cont / 1440; line1.push([contDia, item[prop]])
                                    cont += 15;
                                }
                                if (prop == 'TranRetiDetah93') {

                                    var contDia = cont / 1440; line1.push([contDia, item[prop]])
                                    cont += 15;
                                }
                                if (prop == 'TranRetiDetah94') {

                                    var contDia = cont / 1440; line1.push([contDia, item[prop]])
                                    cont += 15;
                                }
                                if (prop == 'TranRetiDetah95') {

                                    var contDia = cont / 1440; line1.push([contDia, item[prop]])
                                    cont += 15;
                                }
                                if (prop == 'TranRetiDetah96') {

                                    var contDia = cont / 1440; line1.push([contDia, item[prop]])
                                    cont += 15;
                                }

                                //console.log("o." + prop + " = " + item[prop] + " " + prop[prop.length -1]);
                            }
                        });
                    }
                    var a = { label: dataCodigo, lineWidth: 0.5, markerOptions: { size: 0.5, style: 'dimaond' }, showMarker: false };
                    labels.push(a);
                    var cantidadTotal = line1.length - 1536;
                    line2 = line1.splice(1536, cantidadTotal);
                    series.push(line1);
                    seriesB.push(line2);
                    //console.log(line1);
                    //console.log(line2);
                    grafico();
                }
            }
        });
    }
};

grafico = function () {
    if (series.length > 0) {
        $("#grafico").empty();
        var plot1 = $.jqplot('grafico', series,
            {
                series: labels,
                legend: {
                    show: true,
                    location: 'ne',  // compass direction, nw, n, ne, e, se, s, sw, w.
                    placement: 'inside'
                },
                axes: {
                    xaxis: {
                        tickOptions: { formatString: "%#.2f" },
                        pad: 1,
                        min: 1,
                        max: 17,
                        numberTicks: 9
                    },
                    yaxis: {
                        //label: 'Energia',
                        tickOptions: { formatString: "%#.5f" },
                        pad: 0
                    }
                },
                highlighter: {
                    show: true,
                    sizeAdjust: 13.5
                },
                cursor: {
                    show: true,
                    zoom: true
                }
            }
        );
        ///////GRAFICO 2
        $("#grafico2").empty();
        var plot2 = $.jqplot('grafico2', seriesB,
            {
                series: labels,
                legend: {
                    show: true,
                    location: 'ne',  // compass direction, nw, n, ne, e, se, s, sw, w.
                    placement: 'inside'
                },
                axes: {
                    xaxis: {
                        label: 'Dia',
                        pad: 17,
                        min: 17,
                        max: 33,
                        numberTicks: 9,
                        tickOptions: { formatString: "%#.2f" },
                    },
                    yaxis: {
                        //label: 'Energia',
                        tickOptions: { formatString: "%#.5f" },
                        pad: 0
                    }
                },
                highlighter: {
                    show: true,
                    sizeAdjust: 13.5
                },
                cursor: {
                    show: true,
                    zoom: true
                }
            }
        );
    }
    else {
        $("#grafico").empty();
        $("#grafico2").empty();
    }
    $('#reset').click(function () { plot1.resetZoom(), plot2.resetZoom() });
}

function Del() {
    $(".Del").click(function (e) {
        e.preventDefault();
        if (confirm("Desea Quitar Grafico ?")) {
            id = $(this).attr("id").split("_")[1];
            var index = 0;
            var cont = 0;
            if (series.length > 0) {
                for (var key in labels) {
                    var a = labels[key];
                    if (a.label == (id)) {
                        cont = 1;
                        index = key;
                    }
                }
                if (cont == 1) {
                    series.splice(index, 1);
                    labels.splice(index, 1);
                    seriesB.splice(index, 1);
                    //seriesB.splice(index, 1);
                    //console.log(seriesB + ' :numero array serie2  y' + series + 'numero array serie 1');
                    //grafico2();
                    grafico();
                    cont = 0;
                }
                else {
                    alert('No existe');
                }
            }
            else {
                alert('No hay elemntos que borrar');
            }
        };
    });
}

LimpiarGrafico = function () {
    $("#grafico2").empty();
    $("#grafico").empty();
    document.getElementById('btnBotones').style.display = "none";
    series.length = 0;
    seriesB.length = 0;
    labels.length = 0;
}

function add_Ver4() {
    if ($("#PERICODI4 option:selected").val() == "") {
        alert("Por favor, seleccione un Periodo");
    }
    else {
        if ($("#EMPRCODI4").multipleSelect('getSelects') == "") {
            alert("Por favor, seleccione una Empresa");
        }
        else {
            $(".Vergrafico4").click(function (e) {
                document.getElementById('btnBotonesIB').style.display = "block";
                e.preventDefault();
                id = $(this).attr("id").split("_")[1];
                peri = $("#PERICODI4 option:selected").val();
                empresa = $("#EMPRCODI4 option:selected").val();
                $.ajax({
                    type: "GET",
                    dataType: "json",
                    url: controlador + "FetchGraphDataInfoBase/",
                    data:
                    {
                        periodo: peri,
                        codigoER: id,
                        empr: empresa
                    },
                    success: function (result) {
                        if ($.isEmptyObject(result.dataIB)) {
                            alert("Lo sentimos, no hay información asociada");
                        } else {
                            testJqPlot(result.dataIB, result.dataCodigo);
                        }
                    },
                    error: function () {
                        mostrarError();
                    }
                });

                function testJqPlot(dataIB, dataCodigo) {
                    //console.log(dataIB)
                    var line1 = [];
                    var existe = 0;
                    var cont = 1440;
                    var line2 = [];
                    //var contDia = cont / 60;

                    for (var key in labels4) {
                        var a = labels4[key];
                        if (a.label == (dataCodigo))
                            existe = 1;
                    }
                    if (existe == 1) {
                        alert('El registro ya fue previamente seleccionado');
                    }
                    else {
                        $.each(dataIB, function (index, item) {

                            for (var prop in item) {

                                if (prop == 'TinfbDe1') {
                                    var contDia = cont / 1440; line1.push([contDia, item[prop]]);
                                    cont += 15;
                                }
                                if (prop == 'TinfbDe2') {

                                    var contDia = cont / 1440; line1.push([contDia, item[prop]]);
                                    cont += 15;

                                }
                                if (prop == 'TinfbDe3') {

                                    var contDia = cont / 1440; line1.push([contDia, item[prop]]);
                                    cont += 15;

                                }
                                if (prop == 'TinfbDe4') {
                                    var contDia = cont / 1440; line1.push([contDia, item[prop]]);
                                    cont += 15;
                                }
                                if (prop == 'TinfbDe5') {
                                    var contDia = cont / 1440; line1.push([contDia, item[prop]]);
                                    cont += 15;
                                }
                                if (prop == 'TinfbDe6') {
                                    var contDia = cont / 1440; line1.push([contDia, item[prop]]);
                                    cont += 15;
                                }
                                if (prop == 'TinfbDe7') {
                                    var contDia = cont / 1440; line1.push([contDia, item[prop]]);
                                    cont += 15;
                                }
                                if (prop == 'TinfbDe8') {
                                    var contDia = cont / 1440; line1.push([contDia, item[prop]]);
                                    cont += 15;
                                }
                                if (prop == 'TinfbDe9') {
                                    var contDia = cont / 1440; line1.push([contDia, item[prop]]);
                                    cont += 15;
                                }

                                if (prop == 'TinfbDe10') {
                                    var contDia = cont / 1440; line1.push([contDia, item[prop]]);
                                    cont += 15;
                                }
                                if (prop == 'TinfbDe11') {

                                    var contDia = cont / 1440; line1.push([contDia, item[prop]])
                                    cont += 15;
                                }
                                if (prop == 'TinfbDe12') {

                                    var contDia = cont / 1440; line1.push([contDia, item[prop]])
                                    cont += 15;
                                }
                                if (prop == 'TinfbDe13') {

                                    var contDia = cont / 1440; line1.push([contDia, item[prop]])
                                    cont += 15;
                                }
                                if (prop == 'TinfbDe14') {

                                    var contDia = cont / 1440; line1.push([contDia, item[prop]])
                                    cont += 15;
                                }
                                if (prop == 'TinfbDe15') {

                                    var contDia = cont / 1440; line1.push([contDia, item[prop]])
                                    cont += 15;
                                }
                                if (prop == 'TinfbDe16') {

                                    var contDia = cont / 1440; line1.push([contDia, item[prop]])
                                    cont += 15;
                                }
                                if (prop == 'TinfbDe17') {

                                    var contDia = cont / 1440; line1.push([contDia, item[prop]])
                                    cont += 15;
                                }
                                if (prop == 'TinfbDe18') {

                                    var contDia = cont / 1440; line1.push([contDia, item[prop]])
                                    cont += 15;
                                }
                                if (prop == 'TinfbDe19') {

                                    var contDia = cont / 1440; line1.push([contDia, item[prop]])
                                    cont += 15;
                                }
                                if (prop == 'TinfbDe20') {

                                    var contDia = cont / 1440; line1.push([contDia, item[prop]])
                                    cont += 15;
                                }
                                if (prop == 'TinfbDe21') {

                                    var contDia = cont / 1440; line1.push([contDia, item[prop]])
                                    cont += 15;
                                }
                                if (prop == 'TinfbDe22') {

                                    var contDia = cont / 1440; line1.push([contDia, item[prop]])
                                    cont += 15;
                                }
                                if (prop == 'TinfbDe23') {

                                    var contDia = cont / 1440; line1.push([contDia, item[prop]])
                                    cont += 15;
                                }
                                if (prop == 'TinfbDe24') {

                                    var contDia = cont / 1440; line1.push([contDia, item[prop]])
                                    cont += 15;
                                }
                                if (prop == 'TinfbDe25') {

                                    var contDia = cont / 1440; line1.push([contDia, item[prop]])
                                    cont += 15;
                                }
                                if (prop == 'TinfbDe26') {

                                    var contDia = cont / 1440; line1.push([contDia, item[prop]])
                                    cont += 15;
                                }
                                if (prop == 'TinfbDe27') {

                                    var contDia = cont / 1440; line1.push([contDia, item[prop]])
                                    cont += 15;
                                }
                                if (prop == 'TinfbDe28') {

                                    var contDia = cont / 1440; line1.push([contDia, item[prop]])
                                    cont += 15;
                                }
                                if (prop == 'TinfbDe29') {

                                    var contDia = cont / 1440; line1.push([contDia, item[prop]])
                                    cont += 15;
                                }
                                if (prop == 'TinfbDe30') {

                                    var contDia = cont / 1440; line1.push([contDia, item[prop]])
                                    cont += 15;
                                }
                                if (prop == 'TinfbDe31') {

                                    var contDia = cont / 1440; line1.push([contDia, item[prop]])
                                    cont += 15;
                                }
                                if (prop == 'TinfbDe32') {

                                    var contDia = cont / 1440; line1.push([contDia, item[prop]])
                                    cont += 15;
                                }
                                if (prop == 'TinfbDe33') {

                                    var contDia = cont / 1440; line1.push([contDia, item[prop]])
                                    cont += 15;
                                }
                                if (prop == 'TinfbDe34') {

                                    var contDia = cont / 1440; line1.push([contDia, item[prop]])
                                    cont += 15;
                                }
                                if (prop == 'TinfbDe35') {

                                    var contDia = cont / 1440; line1.push([contDia, item[prop]])
                                    cont += 15;
                                }
                                if (prop == 'TinfbDe36') {

                                    var contDia = cont / 1440; line1.push([contDia, item[prop]])
                                    cont += 15;
                                }
                                if (prop == 'TinfbDe37') {

                                    var contDia = cont / 1440; line1.push([contDia, item[prop]])
                                    cont += 15;
                                }
                                if (prop == 'TinfbDe38') {

                                    var contDia = cont / 1440; line1.push([contDia, item[prop]])
                                    cont += 15;
                                }
                                if (prop == 'TinfbDe39') {

                                    var contDia = cont / 1440; line1.push([contDia, item[prop]])
                                    cont += 15;
                                }
                                if (prop == 'TinfbDe40') {

                                    var contDia = cont / 1440; line1.push([contDia, item[prop]])
                                    cont += 15;
                                }
                                if (prop == 'TinfbDe41') {

                                    var contDia = cont / 1440; line1.push([contDia, item[prop]])
                                    cont += 15;
                                }
                                if (prop == 'TinfbDe42') {

                                    var contDia = cont / 1440; line1.push([contDia, item[prop]])
                                    cont += 15;
                                }
                                if (prop == 'TinfbDe43') {

                                    var contDia = cont / 1440; line1.push([contDia, item[prop]])
                                    cont += 15;
                                }
                                if (prop == 'TinfbDe44') {

                                    var contDia = cont / 1440; line1.push([contDia, item[prop]])
                                    cont += 15;
                                }
                                if (prop == 'TinfbDe45') {

                                    var contDia = cont / 1440; line1.push([contDia, item[prop]])
                                    cont += 15;
                                }
                                if (prop == 'TinfbDe46') {

                                    var contDia = cont / 1440; line1.push([contDia, item[prop]])
                                    cont += 15;
                                }
                                if (prop == 'TinfbDe47') {

                                    var contDia = cont / 1440; line1.push([contDia, item[prop]])
                                    cont += 15;
                                }
                                if (prop == 'TinfbDe48') {

                                    var contDia = cont / 1440; line1.push([contDia, item[prop]])
                                    cont += 15;
                                }
                                if (prop == 'TinfbDe49') {

                                    var contDia = cont / 1440; line1.push([contDia, item[prop]])
                                    cont += 15;
                                }
                                if (prop == 'TinfbDe50') {

                                    var contDia = cont / 1440; line1.push([contDia, item[prop]])
                                    cont += 15;
                                }
                                if (prop == 'TinfbDe51') {

                                    var contDia = cont / 1440; line1.push([contDia, item[prop]])
                                    cont += 15;
                                }
                                if (prop == 'TinfbDe52') {

                                    var contDia = cont / 1440; line1.push([contDia, item[prop]])
                                    cont += 15;
                                }
                                if (prop == 'TinfbDe53') {

                                    var contDia = cont / 1440; line1.push([contDia, item[prop]])
                                    cont += 15;
                                }
                                if (prop == 'TinfbDe54') {

                                    var contDia = cont / 1440; line1.push([contDia, item[prop]])
                                    cont += 15;
                                }
                                if (prop == 'TinfbDe55') {

                                    var contDia = cont / 1440; line1.push([contDia, item[prop]])
                                    cont += 15;
                                }
                                if (prop == 'TinfbDe56') {

                                    var contDia = cont / 1440; line1.push([contDia, item[prop]])
                                    cont += 15;
                                }
                                if (prop == 'TinfbDe57') {

                                    var contDia = cont / 1440; line1.push([contDia, item[prop]])
                                    cont += 15;
                                }
                                if (prop == 'TinfbDe58') {

                                    var contDia = cont / 1440; line1.push([contDia, item[prop]])
                                    cont += 15;
                                }
                                if (prop == 'TinfbDe59') {

                                    var contDia = cont / 1440; line1.push([contDia, item[prop]])
                                    cont += 15;
                                }
                                if (prop == 'TinfbDe60') {

                                    var contDia = cont / 1440; line1.push([contDia, item[prop]])
                                    cont += 15;
                                }
                                if (prop == 'TinfbDe61') {

                                    var contDia = cont / 1440; line1.push([contDia, item[prop]])
                                    cont += 15;
                                }
                                if (prop == 'TinfbDe62') {

                                    var contDia = cont / 1440; line1.push([contDia, item[prop]])
                                    cont += 15;
                                }
                                if (prop == 'TinfbDe63') {

                                    var contDia = cont / 1440; line1.push([contDia, item[prop]])
                                    cont += 15;
                                }
                                if (prop == 'TinfbDe64') {

                                    var contDia = cont / 1440; line1.push([contDia, item[prop]])
                                    cont += 15;
                                }
                                if (prop == 'TinfbDe65') {

                                    var contDia = cont / 1440; line1.push([contDia, item[prop]])
                                    cont += 15;
                                }
                                if (prop == 'TinfbDe66') {

                                    var contDia = cont / 1440; line1.push([contDia, item[prop]])
                                    cont += 15;
                                }
                                if (prop == 'TinfbDe67') {

                                    var contDia = cont / 1440; line1.push([contDia, item[prop]])
                                    cont += 15;
                                }
                                if (prop == 'TinfbDe68') {

                                    var contDia = cont / 1440; line1.push([contDia, item[prop]])
                                    cont += 15;
                                }
                                if (prop == 'TinfbDe69') {

                                    var contDia = cont / 1440; line1.push([contDia, item[prop]])
                                    cont += 15;
                                }
                                if (prop == 'TinfbDe70') {

                                    var contDia = cont / 1440; line1.push([contDia, item[prop]])
                                    cont += 15;
                                }
                                if (prop == 'TinfbDe71') {

                                    var contDia = cont / 1440; line1.push([contDia, item[prop]])
                                    cont += 15;
                                }
                                if (prop == 'TinfbDe72') {

                                    var contDia = cont / 1440; line1.push([contDia, item[prop]])
                                    cont += 15;
                                }
                                if (prop == 'TinfbDe73') {

                                    var contDia = cont / 1440; line1.push([contDia, item[prop]])
                                    cont += 15;
                                }
                                if (prop == 'TinfbDe74') {

                                    var contDia = cont / 1440; line1.push([contDia, item[prop]])
                                    cont += 15;
                                }
                                if (prop == 'TinfbDe75') {

                                    var contDia = cont / 1440; line1.push([contDia, item[prop]])
                                    cont += 15;
                                }
                                if (prop == 'TinfbDe76') {

                                    var contDia = cont / 1440; line1.push([contDia, item[prop]])
                                    cont += 15;
                                }
                                if (prop == 'TinfbDe77') {

                                    var contDia = cont / 1440; line1.push([contDia, item[prop]])
                                    cont += 15;
                                }
                                if (prop == 'TinfbDe78') {

                                    var contDia = cont / 1440; line1.push([contDia, item[prop]])
                                    cont += 15;
                                }
                                if (prop == 'TinfbDe79') {

                                    var contDia = cont / 1440; line1.push([contDia, item[prop]])
                                    cont += 15;
                                }
                                if (prop == 'TinfbDe80') {

                                    var contDia = cont / 1440; line1.push([contDia, item[prop]])
                                    cont += 15;
                                }
                                if (prop == 'TinfbDe81') {

                                    var contDia = cont / 1440; line1.push([contDia, item[prop]])
                                    cont += 15;
                                }
                                if (prop == 'TinfbDe82') {

                                    var contDia = cont / 1440; line1.push([contDia, item[prop]])
                                    cont += 15;
                                }
                                if (prop == 'TinfbDe83') {

                                    var contDia = cont / 1440; line1.push([contDia, item[prop]])
                                    cont += 15;
                                }
                                if (prop == 'TinfbDe84') {

                                    var contDia = cont / 1440; line1.push([contDia, item[prop]])
                                    cont += 15;
                                }
                                if (prop == 'TinfbDe85') {

                                    var contDia = cont / 1440; line1.push([contDia, item[prop]])
                                    cont += 15;
                                }
                                if (prop == 'TinfbDe86') {

                                    var contDia = cont / 1440; line1.push([contDia, item[prop]])
                                    cont += 15;
                                }
                                if (prop == 'TinfbDe87') {

                                    var contDia = cont / 1440; line1.push([contDia, item[prop]])
                                    cont += 15;
                                }
                                if (prop == 'TinfbDe88') {

                                    var contDia = cont / 1440; line1.push([contDia, item[prop]])
                                    cont += 15;
                                }
                                if (prop == 'TinfbDe89') {

                                    var contDia = cont / 1440; line1.push([contDia, item[prop]])
                                    cont += 15;
                                }
                                if (prop == 'TinfbDe90') {

                                    var contDia = cont / 1440; line1.push([contDia, item[prop]])
                                    cont += 15;
                                }
                                if (prop == 'TinfbDe91') {

                                    var contDia = cont / 1440; line1.push([contDia, item[prop]])
                                    cont += 15;
                                }
                                if (prop == 'TinfbDe92') {

                                    var contDia = cont / 1440; line1.push([contDia, item[prop]])
                                    cont += 15;
                                }
                                if (prop == 'TinfbDe93') {

                                    var contDia = cont / 1440; line1.push([contDia, item[prop]])
                                    cont += 15;
                                }
                                if (prop == 'TinfbDe94') {

                                    var contDia = cont / 1440; line1.push([contDia, item[prop]])
                                    cont += 15;
                                }
                                if (prop == 'TinfbDe95') {

                                    var contDia = cont / 1440; line1.push([contDia, item[prop]])
                                    cont += 15;
                                }
                                if (prop == 'TinfbDe96') {

                                    var contDia = cont / 1440; line1.push([contDia, item[prop]])
                                    cont += 15;
                                }
                                //console.log("o." + prop + " = " + item[prop] + " " + prop[prop.length -1]);
                            }
                            //}
                            //line1.push([item.TranEntrDetadia, item.TranEntrDetaPromDia])
                        });
                        var a = { label: dataCodigo, lineWidth: 0.5, markerOptions: { size: 0.5, style: 'dimaond' }, showMarker: false };
                        labels4.push(a);
                        var cantidadTotal = line1.length - 1536;
                        line2 = line1.splice(1536, cantidadTotal);
                        series4.push(line1);
                        seriesB4.push(line2);
                        //console.log(line1);
                        //console.log(line2);
                        grafico4();
                    }
                }
            });
        }
    }
};

grafico4 = function () {
    if (series4.length > 0) {
        $("#grafico4").empty();
        var plot1 = $.jqplot('grafico4', series4,
            {
                series: labels4,
                legend: {
                    show: true,
                    location: 'ne',  // compass direction, nw, n, ne, e, se, s, sw, w.
                    placement: 'inside'
                },
                axes: {
                    xaxis: {
                        tickOptions: { formatString: "%#.2f" },
                        pad: 1,
                        min: 1,
                        max: 17,
                        numberTicks: 9
                    },
                    yaxis: {
                        //label: 'Energia',
                        tickOptions: { formatString: "%#.5f" },
                        pad: 0
                    }
                },
                highlighter: {
                    show: true,
                    sizeAdjust: 13.5
                },
                cursor: {
                    show: true,
                    zoom: true
                }
            }
        );
        ///////GRAFICO 2
        $("#grafico24").empty();
        var plot2 = $.jqplot('grafico24', seriesB4,
            {
                series: labels4,
                legend: {
                    show: true,
                    location: 'ne',  // compass direction, nw, n, ne, e, se, s, sw, w.
                    placement: 'inside'
                },
                axes: {
                    xaxis: {
                        label: 'Dia',
                        pad: 17,
                        min: 17,
                        max: 33,
                        numberTicks: 9,
                        tickOptions: { formatString: "%#.2f" },
                    },
                    yaxis: {
                        //label: 'Energia',
                        tickOptions: { formatString: "%#.5f" },
                        pad: 0
                    }
                },
                highlighter: {
                    show: true,
                    sizeAdjust: 13.5
                },
                cursor: {
                    show: true,
                    zoom: true
                }
            }
        );
    }
    else {
        $("#grafico4").empty();
        $("#grafico24").empty();
    }
    $('#reset4').click(function () { plot1.resetZoom(), plot2.resetZoom() });
}

function Del4() {
    $(".Del4").click(function (e) {
        e.preventDefault();
        if (confirm("Desea eliminar el código del gráfico ?")) {
            id = $(this).attr("id").split("_")[1];
            var index = 0;
            var cont = 0;
            if (series4.length > 0) {
                for (var key in labels4) {
                    var a = labels4[key];
                    if (a.label == (id)) {
                        cont = 1;
                        index = key;
                    }
                }
                if (cont == 1) {
                    series4.splice(index, 1);
                    labels4.splice(index, 1);
                    seriesB4.splice(index, 1);
                    //seriesB.splice(index, 1);
                    //console.log(seriesB4 + ' :numero array serie2  y' + series4 + 'numero array serie 1');
                    //grafico2();
                    grafico4();
                    cont = 0;
                }
                else {
                    alert('No existe');
                }
            }
            else {
                alert('Lo sentimos, no hay elemntos que borrar');
            }
        };
    });
}

LimpiarGrafico4 = function () {
    $("#grafico24").empty();
    $("#grafico4").empty();
    document.getElementById('btnBotonesIB').style.display = "none";
    series4.length = 0;
    seriesB4.length = 0;
    labels4.length = 0;
}

descargarEntregaRetiro = function () {
    var iBarrcodi;
    var iEmprCodi;

    if ($("#PERICODI3 option:selected").val() == "") {
        alert("Por favor, seleccione un Periodo...");
    }
    else {
        if ($("#EMPRCODI3 option:selected").val() === undefined && $("#BARRCODI3").val() == "") {
            alert("Por favor, seleccione una Barra de transferencia o Empresa");
            return;
        }
        iPeriCodi = $("#PERICODI3").val();
        iVersion = $("#Version").val();
        if ($("#BARRCODI3").val() == "") {
            iBarrcodi = 0;
        }
        else {
            iBarrcodi = $("#BARRCODI3").val();
        }
        if ($("#EMPRCODI3 option:selected").val() === undefined) {
            iEmprCodi = 0;
        }
        else if ($("#EMPRCODI3 option:selected").val() == "") {
            iEmprCodi = 0;
        }
        else {
            iEmprCodi = $("#EMPRCODI3 option:selected").val();
        }
        $.ajax({
            type: 'POST',
            url: controlador + 'descargarentregaretiro',
            dataType: 'json',
            data: { PeriCodi: iPeriCodi, Version: iVersion, EmprCodi: iEmprCodi, BarrCodi: iBarrcodi },
            success: function (result) {
                if (result == 1) {
                    window.location = controlador + 'abrirentregaretiro';
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

descargarInfoBase = function () {
    if ($("#PERICODI4 option:selected").val() == "") {
        alert("Por favor, seleccione un Periodo...");
    }
    else {
        if ($("#EMPRCODI4").multipleSelect('getSelects') == "") {
            alert("Por favor, seleccione una Empresa");
        }
        else {
            iVersion = $("#Version4").val();
            iPeriCodi = $("#PERICODI4").val();
            var iEmprCodi = $("#EMPRCODI4 option:selected").val();
            $.ajax({
                type: 'POST',
                url: controlador + 'descargarinfobase',
                dataType: 'json',
                data: { PeriCodi: iPeriCodi, Version: iVersion, EmprCodi: iEmprCodi },
                success: function (result) {
                    if (result == 1) {
                        window.location = controlador + 'abririnfobase';
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
}

descargarEnergiaMensual = function () {

    if ($("#PericodiC").val() == "") {
        alert("Seleccione un Periodo");
    }
    else {
        sVersion = $("#VersionC").val();
        sPericodi = $("#PericodiC").val();
        var sBarrcodi = null;
        var sEmprcodi = null;
        $.ajax({
            type: 'POST',
            url: controlador + 'descargarenergiamensual',
            dataType: 'json',
            data: { sPericodi: sPericodi, version: sVersion, barrcodi: sBarrcodi, emprcodi: sEmprcodi },
            success: function (result) {
                if (result == 1) {
                    window.location = controlador + 'abrirenergiamensual';
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

seleccionarEmpresa = function () {

    $.ajax({
        type: 'POST',
        url: controlador + "EscogerEmpresa",
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

abrirpopupresult = function (resultado) {

    $.ajax({
        type: 'POST',
        url: controlador + "ResultadoArchivo",
        data: { sResultado: resultado },
        success: function (evt) {
            $('#popup2').html(evt);
            setTimeout(function () {
                $('#popup2').bPopup({
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

//Funciones de generar pdf
function generarpdf() {

    $.ajax({
        type: 'POST',
        url: controlador + "createPdf",
        success: function (result) {
            if (result == 1) {
                window.location = controlador + 'abrirpdf';
            }
            if (result == -1) {
                alert("Lo sentimos, se ha producido un error");
            }
        },
        error: function () {
            alert("Lo sentimos, se ha producido un error");
        }

    });
};

function buscarEnvios(pericodi) {
    $.ajax({
        type: 'POST',
        url: controlador + "lista",
        data: { pericodi: pericodi },
        success: function (evt) {
            $('#listado').html(evt);
            //oTable = $('#tabla').dataTable({
            //    "sPaginationType": "full_numbers",
            //    "destroy": "true"
            //});
        },
        error: function () {
            alert("Lo sentimos, se ha producido un error");
        }
    });
};

//---------------------------------------------------------------------------------
recargar = function () {
    var cmbPericodi = document.getElementById('pericodi');
    var cmbTipoinfocodi = document.getElementById('tipoinfocodi');
    window.location.href = controlador + "index?pericodi=" + cmbPericodi.value + "&recacodi=0&tipoinfocodi=" + cmbTipoinfocodi.value;
}

recargarrecalculo = function () {
    var cmbPericodi = document.getElementById('pericodi');
    var cmbRecacodi = document.getElementById('recacodi');
    var cmbTipoinfocodi = document.getElementById('tipoinfocodi');
    window.location.href = controlador + "index?pericodi=" + cmbPericodi.value + "&recacodi=" + cmbRecacodi.value + "&tipoinfocodi=" + cmbTipoinfocodi.value;
}

recargarmodelo = function () {
    var cmbPericodi = document.getElementById('pericodi');
    var cmbRecacodi = document.getElementById('recacodi');
    var cmbTipoinfocodi = document.getElementById('tipoinfocodi');
    //var cmbTrnenvcodi = document.getElementById('trnenvcodi');
    var cmbTrnmodcodi = document.getElementById('trnmodcodi');
    window.location.href = controlador + "index?pericodi=" + cmbPericodi.value + "&recacodi=" + cmbRecacodi.value + "&tipoinfocodi=" + cmbTipoinfocodi.value + "&trnenvcodi=0&trnmodcodi=" + cmbTrnmodcodi.value;
}

mostrarExito = function (mensaje) {
    $('#mensaje').removeClass();
    $('#mensaje').html(mensaje);
    $('#mensaje').addClass('action-exito');
}

mostrarError = function (mensaje) {
    $('#mensaje').removeClass();
    $('#mensaje').html(mensaje);
    $('#mensaje').addClass('action-error');
}

mostrarAlerta = function (mensaje) {
    $('#mensaje').removeClass();
    $('#mensaje').html(mensaje);
    $('#mensaje').addClass('action-alert');
}

mostrarMensaje = function (mensaje) {
    $('#mensaje').removeClass();
    $('#mensaje').html(mensaje);
    $('#mensaje').addClass('action-message');
}

mostrarGrillaExcel = function (iTrnenvcodi) {
    var trnenvcodi = 0;
    if (iTrnenvcodi > 0) {
        trnenvcodi = $('#trnenvcodi').val();
    }
    //console.log($('#pericodi').val());
    //console.log($('#recacodi').val());
    //console.log($('#emprcodi').val());
    //console.log($('#trnenvcodi').val());
    //console.log($('#tipoinfocodi').val());
    $.ajax({
        type: 'POST',
        url: controlador + "grillaexcel",
        data: { pericodi: $('#pericodi').val(), recacodi: $('#recacodi').val(), emprcodi: $('#emprcodi').val(), tipoinfocodi: $('#tipoinfocodi').val(), trnenvcodi: trnenvcodi, trnmodcodi: $('#trnmodcodi').val() },
        dataType: 'json',
        success: function (result) {
            //console.log(result);
            var NroColumnas = result.NroColumnas;
            if (NroColumnas > 0) {
                configurarExcelWeb(result);
                //console.log(result.NroColumnas);
                if ($('#trnenvcodi').val() > 0)
                    mostrarExito('Código del envío: ' + $('#trnenvcodi').val() + ", Fecha de envío: " + $('#trnenvfecins').val() + ", Nro. Códigos: " + result.NroColumnas);
                else
                    mostrarExito('La información ya puede ser consultada');
            }
            else {
                if (typeof hot != 'undefined') {
                    hot.destroy();
                }
                var container = document.getElementById('grillaExcel');
                hot = new Handsontable(container, {
                    width: 1,
                    height: 1
                });
                $('#btnDescargarExcel').css('display', 'none');
                $('#btnSelecionarExcel').css('display', 'none');
                $('#btnGrabarExcel').css('display', 'none');
                $('#btnValidarGrillaExcel').css('display', 'none');
                $('#btnVerEnvios').css('display', 'none');
                mostrarMensaje('Lo sentimos, no encontramos información para este tipo de información');
            }
        },
        error: function () {
            if (typeof hot != 'undefined') {
                hot.destroy();
            }
            var container = document.getElementById('grillaExcel');
            hot = new Handsontable(container, {
                width: 1,
                height: 1
            });
            $('#btnDescargarExcel').css('display', 'none');
            $('#btnSelecionarExcel').css('display', 'none');
            $('#btnGrabarExcel').css('display', 'none');
            $('#btnValidarGrillaExcel').css('display', 'none');
            $('#btnVerEnvios').css('display', 'none');
            mostrarError('Lo sentimos no se puede mostrar la hoja de cálculo')
        }
    });
}

firstRowRendererCabecera = function (instance, td, row, col, prop, value, cellProperties) {
    Handsontable.renderers.TextRenderer.apply(this, arguments);
    td.style.color = 'white';
    td.style.background = '#3D8AB8';
    td.style.fontWeight = 'bold';
    td.style.fontFamily = 'sans - serif';
    td.style.fontSize = '12px';
    cellProperties.className = "htCenter",
        cellProperties.readOnly = true;
    //console.log(cellProperties);
}

firstRowRendererCeleste = function (instance, td, row, col, prop, value, cellProperties) {
    Handsontable.renderers.TextRenderer.apply(this, arguments);
    td.style.color = 'black';
    td.style.background = '#E8F6FF';
    td.style.fontFamily = 'sans - serif';
    td.style.fontSize = '12px';
}

firstRowRendererAmarillo = function (instance, td, row, col, prop, value, cellProperties) {
    Handsontable.renderers.TextRenderer.apply(this, arguments);
    td.style.color = 'black';
    td.style.background = '#FFFFD7';
}

negativeValueRenderer = function (instance, td, row, col, prop, value, cellProperties) {
    Handsontable.renderers.TextRenderer.apply(this, arguments);
    var sHeader = instance.getDataAtCell(2, col);
    var sBarra = instance.getDataAtCell(1, col);
    var sMensaje;

    if (row == 4 && col == 1) {
        //Limpiamos la lista de errores
        error = [];
    }
    if (value !== null && value !== undefined && value.toString().trim() !== "") {
        if (isNaN(parseFloat(value))) {
            //console.log('Basura ' + value); //NO ES NUMERO
            td.style.backgroundColor = '#F02211';
            td.style.color = '#FFFFFF';
            td.style.fontWeight = 'bold';
            sMensaje = "[1]Valor " + value + " en el Código " + sHeader + " en la barra de transferencia " + sBarra + " no es válido";
        }
        //else if (parseInt(value, 10) < 0) {
        //    // add class "negativ0o" pinta  Naranja
        //    td.style.backgroundColor = '#FFCC33';
        //    //console.log('Negativo ' + value);
        //}
        else if (parseFloat(value) > 350 || parseFloat(value) < -350) {
            //console.log('>350 ' + value); //"Maximo" pinta amarillo
            td.style.background = '#F3F554';
            sMensaje = "[2]El valor " + value.toString() + " en el Código " + sHeader + " en la barra de transferencia " + sBarra + " supera el Limite de Energía permitido: " + 350;
        }
        //Valores no Negativos para codigos de Retiro de la LVTEA y LVTP
        else if (sHeader && (sHeader.startsWith("CB") || sHeader.startsWith("CL"))) {
            if (parseFloat(value) < 0) {
                td.style.background = '#F3F554';
                sMensaje = "[2]El valor " + value.toString() + " en el Código " + sHeader + " en la barra de transferencia " + sBarra + " no puede ser negativo";
            }
        }
    }
    else if (value != "0") {
        console.log('Vacio/Nulo ' + value);
        td.style.background = '#ECAFF0'; //lila
        sMensaje = "[3]El Código " + sHeader + " en la barra de transferencia " + sBarra + " tiene un valor vacio en la Fila: " + row + ", Columna: " + col;
    }
    if (sMensaje) {
        //console.log(value);
        if (!isNaN(value)) value = "";
        error.push(value.toString().concat("_-_" + row + "_-_" + sHeader + "_-_" + sMensaje));
        //console.log(error);
    }
}

getCustomRenderer = function (instance, td, row, col, prop, value, cellProperties) {
    Handsontable.renderers.TextRenderer.apply(this, arguments);
    td.style.color = '#2980B9';
    td.style.background = '#2980B9';
    //console.log(value);
}

UploadExcel = function () {
    uploader = new plupload.Uploader({
        runtimes: 'html5,flash,silverlight,html4',
        browse_button: 'btnSelecionarExcel',
        url: controlador + 'uploadexcel',
        flash_swf_url: 'Scripts/Moxie.swf',
        silverlight_xap_url: 'Scripts/Moxie.xap',
        multi_selection: false,
        filters: {
            max_file_size: '100mb',
            mime_types: [
                { title: "Archivos Excel .xlsx", extensions: "xlsx,xls" }
            ]
        },
        init: {
            FilesAdded: function (up, files) {
                if (uploader.files.length == 2) {
                    uploader.removeFile(uploader.files[0]);
                }
                uploader.start();
                up.refresh();
            },
            UploadProgress: function (up, file) {
                mostrarAlerta("Por favor espere ...(<strong>" + file.percent + "%</strong>)");
            },
            UploadComplete: function (up, file) {
                mostrarAlerta("Subida completada <strong>Por favor espere ...</strong>");
                mostrarExcelWeb(file[0].name);

                //validarExcelWeb();
            },
            Error: function (up, err) {
                mostrarError(err.code + "-" + err.message);
            }
        }
    });
    uploader.init();
}

mostrarExcelWeb = function (sFile) {
    //console.log($('#pericodi').val());
    //console.log($('#recacodi').val());
    //console.log($('#emprcodi').val());
    //console.log($('#trnenvcodi').val());
    //console.log($('#tipoinfocodi').val());
    $.ajax({
        type: 'POST',
        url: controlador + 'mostrarexcelweb',
        data: { sarchivo: sFile, pericodi: $('#pericodi').val(), recacodi: $('#recacodi').val(), emprcodi: $('#emprcodi').val(), tipoinfocodi: $('#tipoinfocodi').val(), trnmodcodi: $('#trnmodcodi').val() },
        dataType: 'json',
        success: function (result) {
            configurarExcelWeb(result);
            //console.log(result.NroColumnas);
            if (result.MensajeError) {
                mostrarError("Lo sentimos, se ha producido un error: " + result.MensajeError);
            }
            else {
                var sMensaje = "Numero de códigos cargados: <b>" + result.NroColumnas + "</b>, por favor verifique los datos y luego proceda a <b>Grabar</b> en el icono del menú <b>Enviar datos</b>";
                if (result.sCodigoErroneo) {
                    sMensaje = sMensaje + " <br> <i><b>Códigos erroneos: </b> Los siguientes códigos no han sido considerados por estar fuera de fecha o ser inexistentes: " + result.sCodigoErroneo + "</i>";
                }
                if (result.sCodigoFaltante) {
                    sMensaje = sMensaje + " <br> <i><b>Codigos faltantes: </b> Los siguientes códigos no estan presentes en el archivo excel: " + result.sCodigoFaltante + "</i>";
                }
                mostrarAlerta(sMensaje);
            }
        },
        error: function () {
            mostrarError('Lo sentimos no se puede mostrar la hoja de cálculo')
        }
    });
}

configurarExcelWeb = function (result) {
    if (typeof hot != 'undefined') {
        hot.destroy();
    }
    var container = document.getElementById('grillaExcel');
    calculateSizeHandsontable(container);
    var limiteMaxEnergia = result.LimiteMaxEnergia; //Maximo Limite de energia
    //console.log(limiteMaxEnergia);
    var bGrabar = result.bGrabar;
    //console.log(bGrabar);
    //var numregistros = result.NumRegistros;
    $('#trnenvcodi').val(result.Trnenvcodi);
    //console.log($('#trnenvcodi').val());
    $('#testado').val(result.sEstado);
    //console.log($('#testado').val());
    $('#trnenvfecins').val(result.TrnEnvFecIns);
    //console.log($('#trnenvfecins').val());
    //console.log($('#trnenvcodi').val());
    var columns = result.Columnas;
    var widths = result.Widths;
    var NroColumnas = result.NroColumnas;
    var data = result.Data;
    var sRegExVal = new RegExp(/^-{0,1}\d*\.{0,1}\d+$/); //^\d+(?:[\.\,]\d+)?$
    hot = new Handsontable(container, {
        data: data,
        //maxCols: result.Columnas.length,
        colHeaders: false,
        rowHeaders: true,
        colWidths: widths,
        contextMenu: false,
        minSpareRows: 0,
        columns: columns,
        fixedRowsTop: result.FixedRowsTop,
        fixedColumnsLeft: result.FixedColumnsLeft,
        currentRowClassName: 'currentRow',
        cells: function (row, col, prop) {
            //console.log("col:" + col + " row:" + row + " prop" + prop);
            var cellProperties = {};
            if (row == 0 || row == 1 || row == 2 || row == 3) {
                cellProperties.renderer = firstRowRendererCabecera;
            }
            else if (col == 0) {
                cellProperties.renderer = firstRowRendererCeleste;
            }
            else if (col > 0) {
                cellProperties.renderer = negativeValueRenderer;
            }
            //console.log(this.instance.getData()[row][col]);
            return cellProperties;
        },
    });
    hot.render();
    validarCeldasInvisibles();
    if (bGrabar) {
        $('#btnSelecionarExcel').css('display', 'block');
        $('#btnGrabarExcel').css('display', 'block');
        $('#btnValidarGrillaExcel').css('display', 'block');
    }
    else {
        $('#btnSelecionarExcel').css('display', 'none');
        $('#btnGrabarExcel').css('display', 'none');
        $('#btnValidarGrillaExcel').css('display', 'none');
    }
    $('#btnDescargarExcel').css('display', 'block');
    $('#btnVerEnvios').css('display', 'block');
}

function calculateSizeHandsontable(container) {
    var offset = Handsontable.Dom.offset(container);
    var iAltura = $(window).height() - offset.top - 30;
    //console.log($(window).height());
    //console.log(offset.top);
    //console.log(iAltura);
    container.style.height = iAltura + 'px';
    container.style.overflow = 'hidden';
    container.style.width = '1020px';
}

abrirPopupErrores = function () {
    var html = '<span class="button b-close"><span>X</span></span>';
    html += '<p><b>Corregir los siguientes errores</b><p>';
    html += '<table border="0" class="pretty tabla-icono" id="tabla">'
    html += '<thead>'
    html += '<tr>'
    html += '<th>Fila</th>'
    html += '<th>Código</th>'
    html += '<th>Valor</th>'
    html += '<th>Comentario</th>'
    html += '</tr>'
    html += '</thead>'
    html += '<tbody>'
    for (var i = error.length - 1; i >= 0; i--) {
        var sStyle = "background : #ffffff;";
        var sBackground = "";
        if (i % 2)
            var sStyle = "background : #fbf4bf;";
        var SplitError = error[i].split("_-_");
        var sTipError = SplitError[3].substring(0, 3);
        if (sTipError === "[1]") {
            sBackground = " background-color: #F02211;";
        }
        else if (sTipError === "[2]") {
            sBackground = " background-color: #F3F554;";
        }
        else if (sTipError === "[3]") {
            sBackground = " background-color: #ECAFF0;";
        }
        var sMsgError = SplitError[3].substring(3);
        html += '<tr id="Fila_' + i + '">'
        html += '<td style="text-align:right;' + sBackground + '">' + (parseInt(SplitError[1]) + 1) + '</td>'
        html += '<td style="text-align:left;' + sStyle + '">' + SplitError[2] + '</td>'
        html += '<td style="text-align:left;' + sStyle + '">' + SplitError[0] + '</td>'
        html += '<td style="text-align:left;' + sStyle + '">' + sMsgError + '</td>'
        html += '</tr>'
    }
    html += '</tbody>'
    html += '</table>'

    $('#popupErrores').html(html);
    $('#popupErrores').bPopup({
        easing: 'easeOutBack',
        speed: 450,
        transition: 'slideDown'
    });
    mostrarError("Lo sentimos, la hoja del cálculo tiene errores");
}

grabarExcel = function () {
    //Parametros para la separción de la hoja de calculo web en tramos:
    var filas = hot.countRows();
    var nrogrupos = $('#iNroGrupos').val() - 1; //ESTE NUMERO PUEDE CRECER COES.MVC.Extranet.Areas.Transferencias.Helper.iNroGrupos
    console.log(nrogrupos);
    var cantidadPorGrupo = Math.trunc(filas / nrogrupos);
    var array = hot.getData();
    //Iniciamos el envio
    $('#trnenvcodi').val(0); //Listo para uno nuevo
    var iGrupos = -1;
    for (var i = 0; i <= nrogrupos; i++) {
        //DEFINICIÓN DE INTERVALOS
        var indiceInicio = i * cantidadPorGrupo;
        var indiceFinal = indiceInicio + cantidadPorGrupo - 1;
        //----------------------------------------------------
        if (i == nrogrupos) {
            //Para el ultimo tramo completamos el residuo
            indiceFinal = indiceInicio + (filas - i * cantidadPorGrupo) - 1;
        }
        var arraynuevo = array.slice(indiceInicio, indiceFinal + 1);
        //console.log("Indice Inicio:" + indiceInicio);
        //console.log("Indice Final:" + indiceFinal);        
        //console.log(arraynuevo);
        $.ajax({
            type: 'POST',
            url: controlador + 'cargarprevio',
            dataType: "json",
            contentType: 'application/json',
            traditional: true,
            //async: false,
            data: JSON.stringify({ datos: arraynuevo, nroelementos: filas, cont: i, indiceInicio: indiceInicio, indiceFinal: indiceFinal }),
            success: function (result) {
                //Tramo de hoja de calculo correctamente transferido al servidor
                var indicador = result;
                if (indicador == 1) {
                    //Numero de filas es igual a lo enviado al servidor
                    mostrarAlerta("La información se esta registrando en los servidores del COES...");
                    grabarDatos();
                }
            },
            error: function () {
                mostrarError(response.status + " " + response.statusText);
            }
        });
    }
}

grabarDatos = function () {
    $.ajax({
        type: "POST",
        url: controlador + 'grabargrillaexcel',
        dataType: "json",
        contentType: 'application/json',
        traditional: true,
        data: JSON.stringify({ pericodi: $('#pericodi').val(), recacodi: $('#recacodi').val(), emprcodi: $('#emprcodi').val(), tipoinfocodi: $('#tipoinfocodi').val(), trnmodcodi: $('#trnmodcodi').val(), testado: $('#testado').val() }),
        success: function (result) {
            if (result.sError == "") {
                var iTrnenvcodi = result.Trnenvcodi;
                $('#trnenvcodi').val(iTrnenvcodi); //Recogemos el ID de ENVIO 
                console.log($('#trnenvcodi').val());
                var sFecha = result.sFecha;
                var sPlazo = result.sPlazo;
                if (sPlazo == "S") {
                    mostrarExito('Operación Exitosa - ' + "Código de envío: <b>" + iTrnenvcodi + "</b>, Fecha de envío: <b>" + sFecha + "</b><br><span  style = 'margin-left: 30px;'><b>Resumen: </b></span><br><div style = 'margin-left: 30px;'>" + result.sMensaje + "</div>");
                }
                else {
                    //sPlazo == "N"
                    var sAlerta = "<br><span  style = 'margin-left: 30px;'>La información remitida será considerado como fuera de plazo y quedará a consideración del COES su inclusión en el proceso de valorización</span>";
                    mostrarAlerta('Operación completada - ' + "Código de envío: <b>" + iTrnenvcodi + "</b>, Fecha de envío: <b>" + sFecha + "</b>" + sAlerta + "<br><span  style = 'margin-left: 30px;'><b>Resumen: </b></span><br><div style = 'margin-left: 30px;'>" + result.sMensaje + "</div>");
                }
                $('#btnGrabarExcel').css('display', 'block');
            }
            else {
                mostrarError('Lo sentimos, ha ocurrido un error: ' + result.sError);
            }
        },
        error: function (response) {
            mostrarError(response.status + " " + response.statusText);
        }
    });
}

descargarArchivo = function (formato) {
    var trnenvcodi = 0;
    if ($('#trnenvcodi').val() > 0) {
        trnenvcodi = $('#trnenvcodi').val();
    }
    $.ajax({
        type: 'POST',
        url: controlador + 'exportardata',
        data: { pericodi: $('#pericodi').val(), recacodi: $('#recacodi').val(), emprcodi: $('#emprcodi').val(), trnenvcodi: trnenvcodi, trnmodcodi: $('#trnmodcodi').val(), tipoinfocodi: $('#tipoinfocodi').val(), formato: formato },
        dataType: 'json',
        success: function (result) {
            if (result != -1) {
                window.location = controlador + 'abrirarchivo?formato=' + formato + '&file=' + result;
                mostrarExito("Felicidades, el archivo se descargo correctamente...!");
            }
            else {
                mostrarError(result.error);
            }
        },
        error: function (response) {
            mostrarError(response.status + " " + response.statusText);
        }
    });
}

verEnviosAnteriores = function () {
    $.ajax({
        type: 'POST',
        url: controlador + "listaenvios",
        data: { pericodi: $('#pericodi').val(), recacodi: $('#recacodi').val(), emprcodi: $('#emprcodi').val(), tipoinfocodi: $('#tipoinfocodi').val(), trnmodcodi: $('#trnmodcodi').val() },
        success: function (evt) {
            //console.log(evt);
            $('#popupEnvios').html(evt);
            setTimeout(function () {
                $('#popupEnvios').bPopup({
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

function validarCeldasInvisibles() {
    const totalRows = hot.countRows();
    const totalCols = hot.countCols();

    for (let row = 4; row < totalRows; row++) {
        for (let col = 1; col < totalCols; col++) {
            const td = hot.getCell(row, col);
            const meta = hot.getCellMeta(row, col);
            const value = hot.getDataAtCell(row, col);
            const prop = hot.colToProp(col);
            const renderer = meta.renderer || Handsontable.renderers.TextRenderer;

            // Si td existe, se redibuja. Si no, creamos un td temporal solo para validar.
            if (td) {
                renderer(hot, td, row, col, prop, value, meta);
            } else {
                const tempTd = document.createElement("td");
                renderer(hot, tempTd, row, col, prop, value, meta);
            }
        }
    }
}