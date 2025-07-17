var controlador = siteRoot + "transferencias/envioinformacion/";
var series = [];
var seriesB = [];
var labels = [];
var series4 = [];
var seriesB4 = [];
var labels4 = [];

$(document).ready(function () {

    $('#EMPRCODI').multipleSelect({
        width: '200px',
        filter: true,
        selectAll: false,
        single: true
    });

    $('#EMPRCODI2').multipleSelect({
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

    $('#EMPRCODI5').multipleSelect({
        width: '200px',
        filter: true,
        selectAll: false,
        single: true
    });

    document.getElementById('divOpcionCarga').style.display = "none";
    $("#VersionDes").prop("disabled", true);
    $("#PERIANIOMES").change(function () {
        if ($("#PERIANIOMES").val() != "") {
            var options = {};
            options.url = controlador + "GetVersion";
            options.type = "POST";
            options.data = JSON.stringify({ pericodi: $("#PERIANIOMES").val() });
            options.dataType = "json";
            options.contentType = "application/json";
            options.success = function (modelo) {
                $("#VersionDes").empty();
                $("#VersionDes").removeAttr("disabled");
                $.each(modelo.ListaRecalculo, function (k, v) {
                    var option = '<option value=' + v.RecaCodi + '>' + v.RecaNombre + '</option>';
                    $('#VersionDes').append(option);
                });
            };
            options.error = function () { alert("Lo sentimos, no se encuentran registrada ninguna revisión"); };
            $.ajax(options);
        }
        else {
            $("#VersionDes").empty();
            $("#VersionDes").prop("disabled", true);
        }
    });

    $("#Version").prop("disabled", true);
    $("#PERICODI2").change(function () {
        if ($("#PERICODI2").val() != "") {
            var options = {};
            options.url = controlador + "GetVersion";
            options.type = "POST";
            options.data = JSON.stringify({ pericodi: $("#PERICODI2").val() });
            options.dataType = "json";
            options.contentType = "application/json";
            options.success = function (modelo) {       
                $("#Version").empty();
                $("#Version").removeAttr("disabled");
                $.each(modelo.ListaRecalculo, function (k, v) {
                    var option = '<option value=' + v.RecaCodi + '>' + v.RecaNombre + '</option>';
                    $('#Version').append(option);                    
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

    $("#VersionG").prop("disabled", true);
    $("#PERICODI3").change(function () {
        listarVersion();
    });

    $("#VersionG").change(function () {
        listarEmpresa();
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
            options.success = function (modelo) {                
                $("#Version4").empty();
                $("#Version4").removeAttr("disabled");
                $.each(modelo.ListaRecalculo, function (k, v) {
                    var option = '<option value=' + v.RecaCodi + '>' + v.RecaNombre + '</option>';
                    $('#Version4').append(option);                    
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
            options.success = function (modelo) {
                ////console.log(modelo);
                $("#VersionC").empty();
                $("#VersionC").removeAttr("disabled");
                $.each(modelo.ListaRecalculo, function (k, v) {
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

    $("#PERIANIOMES").change(function () {
        if ($("#PERIANIOMES option:selected").val() != '') {
            $("#tipoinfocodi").change()
        }
    });

    $("#EMPRCODI").change(function () {        
        $("#hcbEMPRCODI").val($("#EMPRCODI option:selected").val());
        if ($("#EMPRCODI").multipleSelect('getSelects') != '') {
            $("#tipoinfocodi").change();
        }
        $("#hcbEMPRCODI").change();
        $("#hcbEMPRCODI2").change();
    });

    $("#hcbEMPRCODI").change(function () {        
        $("#EMPRCODI option[value='" + $("#hcbEMPRCODI").val() + "']").prop("selected", true);
    });
    
    $('#btnExportar').click(function () {
        debugger

        if ($("#PERIANIOMES option:selected").val() == '') {
            alert("Por favor, seleccione un Periodo");
            return;
        }
        
        if ($("#hcbEMPRCODI").val() == '') {
            alert("Por favor, seleccione una Empresa");
            return;
        }


        if ($("#tipoinfocodi").val() == '2' && $("#trnmodcodi").val() == '') {
            alert("Por favor, seleccione un Modelo");
            return;
        }

        switch ($("#tipoinfocodi").val()) {
            case "0":
                generarExcel();
                break;
            case "1":
                generarExcelBase()
                break;
            case "2":
                generarExcelModelo()
                break;                            
        }        
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

    $("#tipoinfocodi").change(function () {

        if ($("#tipoinfocodi").val() == "2") {
            if ($("#PERIANIOMES option:selected").val() == '') {                
                return;
            }

            if ($("#EMPRCODI").multipleSelect('getSelects') == '') {                
                return;
            }

            var options = {};
            options.url = controlador + "GetModelosGM";
            options.type = "POST";
            options.data = JSON.stringify({ emprcodi: $("#EMPRCODI option:selected").val() == "" ? 0 : $("#EMPRCODI option:selected").val() });
            options.dataType = "json";
            options.contentType = "application/json";
            options.success = function (modelo) {                
                $('#trnmodcodi').html('');
                $.each(modelo.ListaModelo, function (k, v) {
                    var option = '<option value=' + v.TrnModCodi + '>' + v.TrnModNombre + '</option>';
                    $('#trnmodcodi').append(option);
                });
                $("#tdModelo").css('display', 'inline-block');
                $("#trnmodcodi").css('display', 'inline-block');                
            };
            options.error = function () { alert("Lo sentimos, no se encuentran registrada ninguna revisión"); };
            $.ajax(options);
        } else {            
            $("#tdModelo").css('display', 'none');
            $("#trnmodcodi").css('display', 'none');
        }
    });
    /*
     * Subir
     */
    $("#PERICODI2").change(function () {        
        if ($("#PERICODI2 option:selected").val() != '') {
            $("#tipoinfocodi2").change();
        }
    });

    $("#EMPRCODI2").change(function () {        
        $("#hcbEMPRCODI2").val($("#EMPRCODI2 option:selected").val());
        if ($("#EMPRCODI2").multipleSelect('getSelects') != '') {
            $("#tipoinfocodi2").change();
        }
        $("#hcbEMPRCODI2").change();
        $("#hcbEMPRCODI").change();
    });

    $("#hcbEMPRCODI2").change(function () {        
        $("#EMPRCODI2 option[value='" + $("#hcbEMPRCODI2").val() + "']").prop("selected", true);
    });

    $("#tipoinfocodi2").change(function () {        
        if ($("#tipoinfocodi2").val() == "MD") {
            if ($("#PERICODI2 option:selected").val() == '') {
                return;
            }

            if ($("#EMPRCODI2").multipleSelect('getSelects') == '') {
                return;
            }

            var options = {};
            options.url = controlador + "GetModelosGM";
            options.type = "POST";
            options.data = JSON.stringify({ emprcodi: $("#EMPRCODI2 option:selected").val() == "" ? 0 : $("#EMPRCODI2 option:selected").val() });
            options.dataType = "json";
            options.contentType = "application/json";
            options.success = function (modelo) {                                
                $('#trnmodcodi2').html('');
                $.each(modelo.ListaModelo, function (k, v) {
                    var option = '<option value=' + v.TrnModCodi + '>' + v.TrnModNombre + '</option>';
                    $('#trnmodcodi2').append(option);                    
                });
                $("#tdModelo2").css('display', 'inline-block');
                $("#trnmodcodi2").css('display', 'inline-block');
            };
            options.error = function () { alert("Lo sentimos, no se encuentran registrada ninguna revisión"); };
            $.ajax(options);
        } else {            
            $("#tdModelo2").css('display', 'none');
            $("#trnmodcodi2").css('display', 'none');
        }
    });
});

listarVersion = function () {
    if ($("#PERICODI3").val() != "") {
        var options = {};
        options.url = controlador + "GetVersion";
        options.type = "POST";
        options.data = JSON.stringify({ pericodi: $("#PERICODI3").val() });
        options.dataType = "json";
        options.contentType = "application/json";
        options.success = function (modelo) {
            $("#VersionG").empty();
            $("#VersionG").removeAttr("disabled");
            $.each(modelo.ListaRecalculo, function (k, v) {
                var option = '<option value=' + v.RecaCodi + '>' + v.RecaNombre + '</option>';
                $('#VersionG').append(option);
            });
            listarEmpresa();
        };
        options.error = function () { alert("Lo sentimos, no se encuentran registrada ninguna revisión"); };
        $.ajax(options);
    }
    else {
        $("#VersionG").empty();
        $("#VersionG").prop("disabled", true);
    }
}

listarEmpresa = function () {
    if ($("#VersionG").val() != "") {
        var options = {};
        options.url = controlador + "GetEmpresasxPeriodo";
        options.type = "POST";
        options.data = JSON.stringify({ pericodi: $("#PERICODI3").val(), version: $("#VersionG").val() });
        options.dataType = "json";
        options.contentType = "application/json";
        options.success = function (empresas) {
            $("#EMPRCODI3").empty();
            $("#EMPRCODI3").removeAttr("disabled");
            var option2 = "<option value='0'>--Seleccione--</option>";
            $('#EMPRCODI3').append(option2);
            $.each(empresas.ListaEmpresas, function (k, v) {
                var option = '<option value=' + v.EmprCodi + '>' + v.EmprNombre + '</option>';
                $('#EMPRCODI3').append(option);
            });
        };
        options.error = function () { alert("Lo sentimos, no se encuentran registrada ninguna empresa."); };
        $.ajax(options);
    }
    else {
        $("#EMPRCODI3").empty();
        $("#EMPRCODI3").prop("disabled", true);
    }
}

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
        alert("Por favor, seleccione un Periodo");
    }
    else {        
        if ($("#hcbEMPRCODI").val() == '') {
            alert("Por favor, seleccione una Empresa");
        }
        else {
            $.ajax({
                type: 'POST',
                url: controlador + 'generarexcel',
                dataType: 'json',
                data: { iPeriodo: $("#PERIANIOMES option:selected").val(), iEmpresa: $("#EMPRCODI option:selected").val() },
                success: function (result) {
                    if (result == 1) {
                        window.location = controlador + 'abrirexcel';
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
}

generarExcelBase = function () {
    if ($("#PERIANIOMES option:selected").val() == "") {
        alert('Seleccione un periodo');
    }
    else {        
        if ($("#hcbEMPRCODI").val() == '') {
            alert("Por favor, seleccione una Empresa");
        }
        else {
            $.ajax({
                type: 'POST',
                url: controlador + 'generarexcelbase',
                dataType: 'json',
                data: { periodo: $("#PERIANIOMES option:selected").val(), empr: $("#EMPRCODI option:selected").val() },
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
}

generarExcelModelo = function () {
    if ($("#PERIANIOMES option:selected").val() == "") {
        alert('Seleccione un periodo');
    }
    else {
        
        if ($("#hcbEMPRCODI").val() == '') {
            alert("Por favor, seleccione una Empresa");
        }
        else {

            var trnenvcodi = 0;
            $.ajax({
                type: 'POST',
                url: controlador + 'generarExcelModelo',
                dataType: 'json',
                data: { periodo: $("#PERIANIOMES option:selected").val(), empr: $("#EMPRCODI option:selected").val(), trnmodcodi: $("#trnmodcodi").val(), recacodi: $("#VersionDes").val(), trnenvcodi: trnenvcodi },
                success: function (result) {
                    if (result == "1") {
                        window.location = controlador + 'abrirexcelmodelo';
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
}

listaEntregaRetiro = function () {
    var iBarrcodi;
    var iEmprCodi;

    if ($("#PERICODI3 option:selected").val() == "") {
        alert("Por favor, seleccione un Periodo...");
    }
    else {
        iPeriCodi = $("#PERICODI3 option:selected").val();
        iVersion = $("#VersionG").val();
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
                sListaEntrega += '<td style="text-align:left"><a  href=# id=view_' + v.CodiEntrCodigo + ' ' + 'class=Vergrafico' + '><img  src="../../../Areas/Transferencias/Content/Images/view.gif" title="Insertar linea en el grafico" alt="Insertar linea en el grafico" /></a></td>';
                sListaEntrega += '<td style="text-align:left">' + v.CodiEntrCodigo + '</td>';
                sListaEntrega += '<td style="text-align:left">' + v.EmprNombre + '</td>';
                sListaEntrega += '<td style="text-align:left">' + v.CentGeneNombre + '</td>';
                sListaEntrega += '<td style="text-align:left">' + v.BarrNombre + ' </td>';
                sListaEntrega += '<td style="text-align:left">' + v.TranEntrTipoInformacion + ' </td>';
                sListaEntrega += '<td style="text-align:left">' + v.Total + '</td>';
                sListaEntrega += '<td><a style="display:box;float:right" href=# id=view_' + v.CodiEntrCodigo + ' ' + 'class=Del ><img  src="../../../Areas/Transferencias/Content/Images/tachito.gif" title="Retirar linea del grafico" alt="Retirar linea del grafico" /></a></td>';
                sListaEntrega += '</tr>';
                $('#tabla').append(sListaEntrega);

            });
            //Lista de Retiros
            $.each(reti, function (k, v) {
                var sListaRetiro = '';
                sListaRetiro += '<tr>';
                sListaRetiro += '<td style="text-align:left"><a  href=# id=view_' + v.SoliCodiRetiCodigo + ' ' + 'class=Vergrafico' + '><img  src="../../../Areas/Transferencias/Content/Images/view.jpg" title="Insertar linea en el grafico" alt="Insertar linea en el grafico" /></a></a></td>';
                sListaRetiro += '<td style="text-align:left">' + v.SoliCodiRetiCodigo + '</td>';
                sListaRetiro += '<td style="text-align:left">' + v.EmprNombre + '</td>';
                sListaRetiro += '<td style="text-align:left">' + v.CliNombre + '</td>';
                sListaRetiro += '<td style="text-align:left">' + v.BarrNombre + ' </td>';
                sListaRetiro += '<td style="text-align:left">' + v.TranRetiTipoInformacion + ' </td>';
                sListaRetiro += '<td style="text-align:left">' + v.Total + '</td>';
                sListaRetiro += '<td style="text-align:left"><a   style="display:box;float:right" href=# id=view_' + v.SoliCodiRetiCodigo + ' ' + 'class=Del >' + '<img  src="../../../Areas/Transferencias/Content/Images/tachito.gif" title="Retirar linea del grafico" alt="Retirar linea del grafico" />' + '</a></td>';
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
                    sListaEntrega += '<td style="text-align:left"><a href=# id=view_' + v.TinfbCodigo + ' ' + 'class=Vergrafico4' + '><img  src="../../../Areas/Transferencias/Content/Images/view.gif" title="Insertar linea en el grafico" alt="Insertar linea en el grafico" /></a></td>';
                    sListaEntrega += '<td style="text-align:left">' + v.TinfbCodigo + '</td>';
                    sListaEntrega += '<td style="text-align:left">' + v.EmprNombre + '</td>';
                    sListaEntrega += '<td style="text-align:left">' + v.CentGeneNombre + '</td>';
                    sListaEntrega += '<td style="text-align:left">' + v.BarrNombre + ' </td>';
                    sListaEntrega += '<td style="text-align:left">' + v.TinfbTipoInformacion + ' </td>';
                    sListaEntrega += '<td style="text-align:left">' + v.Total + '</td>';
                    sListaEntrega += '<td style="text-align:left"><a style="display:box;float:right" href=# id=view_' + v.TinfbCodigo + ' ' + 'class=Del4 ><img  src="../../../Areas/Transferencias/Content/Images/tachito.gif" title="Retirar linea del grafico" alt="Retirar linea del grafico" /></a></td>';
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
                        //console.log(result.dataER);
                        //console.log(result.dataCodigo);
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
                    //var contDia = cont / 60   I;
                    if (tipo == "E") {
                        $.each(dataER, function (index, item) {
                            for (var prop in item) {

                                if (prop == 'TranEntrDetah1') {
                                    var contDia = cont / 1440; line1.push([contDia, 4 * item[prop]]);
                                    cont += 15;
                                }
                                if (prop == 'TranEntrDetah2') {

                                    var contDia = cont / 1440; line1.push([contDia, 4 * item[prop]]);
                                    cont += 15;

                                }
                                if (prop == 'TranEntrDetah3') {

                                    var contDia = cont / 1440; line1.push([contDia, 4 * item[prop]]);
                                    cont += 15;

                                }
                                if (prop == 'TranEntrDetah4') {
                                    var contDia = cont / 1440; line1.push([contDia, 4 * item[prop]]);
                                    cont += 15;
                                }
                                if (prop == 'TranEntrDetah5') {
                                    var contDia = cont / 1440; line1.push([contDia, 4 * item[prop]]);
                                    cont += 15;
                                }
                                if (prop == 'TranEntrDetah6') {
                                    var contDia = cont / 1440; line1.push([contDia, 4 * item[prop]]);
                                    cont += 15;
                                }
                                if (prop == 'TranEntrDetah7') {
                                    var contDia = cont / 1440; line1.push([contDia, 4 * item[prop]]);
                                    cont += 15;
                                }
                                if (prop == 'TranEntrDetah8') {
                                    var contDia = cont / 1440; line1.push([contDia, 4 * item[prop]]);
                                    cont += 15;
                                }
                                if (prop == 'TranEntrDetah9') {
                                    var contDia = cont / 1440; line1.push([contDia, 4 * item[prop]]);
                                    cont += 15;
                                }

                                if (prop == 'TranEntrDetah10') {
                                    var contDia = cont / 1440; line1.push([contDia, 4 * item[prop]]);
                                    cont += 15;
                                }
                                if (prop == 'TranEntrDetah11') {

                                    var contDia = cont / 1440; line1.push([contDia, 4 * item[prop]])
                                    cont += 15;
                                }
                                if (prop == 'TranEntrDetah12') {

                                    var contDia = cont / 1440; line1.push([contDia, 4 * item[prop]])
                                    cont += 15;
                                }
                                if (prop == 'TranEntrDetah13') {

                                    var contDia = cont / 1440; line1.push([contDia, 4 * item[prop]])
                                    cont += 15;
                                }
                                if (prop == 'TranEntrDetah14') {

                                    var contDia = cont / 1440; line1.push([contDia, 4 * item[prop]])
                                    cont += 15;
                                }
                                if (prop == 'TranEntrDetah15') {

                                    var contDia = cont / 1440; line1.push([contDia, 4 * item[prop]])
                                    cont += 15;
                                }
                                if (prop == 'TranEntrDetah16') {

                                    var contDia = cont / 1440; line1.push([contDia, 4 * item[prop]])
                                    cont += 15;
                                }
                                if (prop == 'TranEntrDetah17') {

                                    var contDia = cont / 1440; line1.push([contDia, 4 * item[prop]])
                                    cont += 15;
                                }
                                if (prop == 'TranEntrDetah18') {

                                    var contDia = cont / 1440; line1.push([contDia, 4 * item[prop]])
                                    cont += 15;
                                }
                                if (prop == 'TranEntrDetah19') {

                                    var contDia = cont / 1440; line1.push([contDia, 4 * item[prop]])
                                    cont += 15;
                                }
                                if (prop == 'TranEntrDetah20') {

                                    var contDia = cont / 1440; line1.push([contDia, 4 * item[prop]])
                                    cont += 15;
                                }
                                if (prop == 'TranEntrDetah21') {

                                    var contDia = cont / 1440; line1.push([contDia, 4 * item[prop]])
                                    cont += 15;
                                }
                                if (prop == 'TranEntrDetah22') {

                                    var contDia = cont / 1440; line1.push([contDia, 4 * item[prop]])
                                    cont += 15;
                                }
                                if (prop == 'TranEntrDetah23') {

                                    var contDia = cont / 1440; line1.push([contDia, 4 * item[prop]])
                                    cont += 15;
                                }
                                if (prop == 'TranEntrDetah24') {

                                    var contDia = cont / 1440; line1.push([contDia, 4 * item[prop]])
                                    cont += 15;
                                }
                                if (prop == 'TranEntrDetah25') {

                                    var contDia = cont / 1440; line1.push([contDia, 4 * item[prop]])
                                    cont += 15;
                                }
                                if (prop == 'TranEntrDetah26') {

                                    var contDia = cont / 1440; line1.push([contDia, 4 * item[prop]])
                                    cont += 15;
                                }
                                if (prop == 'TranEntrDetah27') {

                                    var contDia = cont / 1440; line1.push([contDia, 4 * item[prop]])
                                    cont += 15;
                                }
                                if (prop == 'TranEntrDetah28') {

                                    var contDia = cont / 1440; line1.push([contDia, 4 * item[prop]])
                                    cont += 15;
                                }
                                if (prop == 'TranEntrDetah29') {

                                    var contDia = cont / 1440; line1.push([contDia, 4 * item[prop]])
                                    cont += 15;
                                }
                                if (prop == 'TranEntrDetah30') {

                                    var contDia = cont / 1440; line1.push([contDia, 4 * item[prop]])
                                    cont += 15;
                                }
                                if (prop == 'TranEntrDetah31') {

                                    var contDia = cont / 1440; line1.push([contDia, 4 * item[prop]])
                                    cont += 15;
                                }
                                if (prop == 'TranEntrDetah32') {

                                    var contDia = cont / 1440; line1.push([contDia, 4 * item[prop]])
                                    cont += 15;
                                }
                                if (prop == 'TranEntrDetah33') {

                                    var contDia = cont / 1440; line1.push([contDia, 4 * item[prop]])
                                    cont += 15;
                                }
                                if (prop == 'TranEntrDetah34') {

                                    var contDia = cont / 1440; line1.push([contDia, 4 * item[prop]])
                                    cont += 15;
                                }
                                if (prop == 'TranEntrDetah35') {

                                    var contDia = cont / 1440; line1.push([contDia, 4 * item[prop]])
                                    cont += 15;
                                }
                                if (prop == 'TranEntrDetah36') {

                                    var contDia = cont / 1440; line1.push([contDia, 4 * item[prop]])
                                    cont += 15;
                                }
                                if (prop == 'TranEntrDetah37') {

                                    var contDia = cont / 1440; line1.push([contDia, 4 * item[prop]])
                                    cont += 15;
                                }
                                if (prop == 'TranEntrDetah38') {

                                    var contDia = cont / 1440; line1.push([contDia, 4 * item[prop]])
                                    cont += 15;
                                }
                                if (prop == 'TranEntrDetah39') {

                                    var contDia = cont / 1440; line1.push([contDia, 4 * item[prop]])
                                    cont += 15;
                                }
                                if (prop == 'TranEntrDetah40') {

                                    var contDia = cont / 1440; line1.push([contDia, 4 * item[prop]])
                                    cont += 15;
                                }
                                if (prop == 'TranEntrDetah41') {

                                    var contDia = cont / 1440; line1.push([contDia, 4 * item[prop]])
                                    cont += 15;
                                }
                                if (prop == 'TranEntrDetah42') {

                                    var contDia = cont / 1440; line1.push([contDia, 4 * item[prop]])
                                    cont += 15;
                                }
                                if (prop == 'TranEntrDetah43') {

                                    var contDia = cont / 1440; line1.push([contDia, 4 * item[prop]])
                                    cont += 15;
                                }
                                if (prop == 'TranEntrDetah44') {

                                    var contDia = cont / 1440; line1.push([contDia, 4 * item[prop]])
                                    cont += 15;
                                }
                                if (prop == 'TranEntrDetah45') {

                                    var contDia = cont / 1440; line1.push([contDia, 4 * item[prop]])
                                    cont += 15;
                                }
                                if (prop == 'TranEntrDetah46') {

                                    var contDia = cont / 1440; line1.push([contDia, 4 * item[prop]])
                                    cont += 15;
                                }
                                if (prop == 'TranEntrDetah47') {

                                    var contDia = cont / 1440; line1.push([contDia, 4 * item[prop]])
                                    cont += 15;
                                }
                                if (prop == 'TranEntrDetah48') {

                                    var contDia = cont / 1440; line1.push([contDia, 4 * item[prop]])
                                    cont += 15;
                                }
                                if (prop == 'TranEntrDetah49') {

                                    var contDia = cont / 1440; line1.push([contDia, 4 * item[prop]])
                                    cont += 15;
                                }
                                if (prop == 'TranEntrDetah50') {

                                    var contDia = cont / 1440; line1.push([contDia, 4 * item[prop]])
                                    cont += 15;
                                }
                                if (prop == 'TranEntrDetah51') {

                                    var contDia = cont / 1440; line1.push([contDia, 4 * item[prop]])
                                    cont += 15;
                                }
                                if (prop == 'TranEntrDetah52') {

                                    var contDia = cont / 1440; line1.push([contDia, 4 * item[prop]])
                                    cont += 15;
                                }
                                if (prop == 'TranEntrDetah53') {

                                    var contDia = cont / 1440; line1.push([contDia, 4 * item[prop]])
                                    cont += 15;
                                }
                                if (prop == 'TranEntrDetah54') {

                                    var contDia = cont / 1440; line1.push([contDia, 4 * item[prop]])
                                    cont += 15;
                                }
                                if (prop == 'TranEntrDetah55') {

                                    var contDia = cont / 1440; line1.push([contDia, 4 * item[prop]])
                                    cont += 15;
                                }
                                if (prop == 'TranEntrDetah56') {

                                    var contDia = cont / 1440; line1.push([contDia, 4 * item[prop]])
                                    cont += 15;
                                }
                                if (prop == 'TranEntrDetah57') {

                                    var contDia = cont / 1440; line1.push([contDia, 4 * item[prop]])
                                    cont += 15;
                                }
                                if (prop == 'TranEntrDetah58') {

                                    var contDia = cont / 1440; line1.push([contDia, 4 * item[prop]])
                                    cont += 15;
                                }
                                if (prop == 'TranEntrDetah59') {

                                    var contDia = cont / 1440; line1.push([contDia, 4 * item[prop]])
                                    cont += 15;
                                }
                                if (prop == 'TranEntrDetah60') {

                                    var contDia = cont / 1440; line1.push([contDia, 4 * item[prop]])
                                    cont += 15;
                                }
                                if (prop == 'TranEntrDetah61') {

                                    var contDia = cont / 1440; line1.push([contDia, 4 * item[prop]])
                                    cont += 15;
                                }
                                if (prop == 'TranEntrDetah62') {

                                    var contDia = cont / 1440; line1.push([contDia, 4 * item[prop]])
                                    cont += 15;
                                }
                                if (prop == 'TranEntrDetah63') {

                                    var contDia = cont / 1440; line1.push([contDia, 4 * item[prop]])
                                    cont += 15;
                                }
                                if (prop == 'TranEntrDetah64') {

                                    var contDia = cont / 1440; line1.push([contDia, 4 * item[prop]])
                                    cont += 15;
                                }
                                if (prop == 'TranEntrDetah65') {

                                    var contDia = cont / 1440; line1.push([contDia, 4 * item[prop]])
                                    cont += 15;
                                }
                                if (prop == 'TranEntrDetah66') {

                                    var contDia = cont / 1440; line1.push([contDia, 4 * item[prop]])
                                    cont += 15;
                                }
                                if (prop == 'TranEntrDetah67') {

                                    var contDia = cont / 1440; line1.push([contDia, 4 * item[prop]])
                                    cont += 15;
                                }
                                if (prop == 'TranEntrDetah68') {

                                    var contDia = cont / 1440; line1.push([contDia, 4 * item[prop]])
                                    cont += 15;
                                }
                                if (prop == 'TranEntrDetah69') {

                                    var contDia = cont / 1440; line1.push([contDia, 4 * item[prop]])
                                    cont += 15;
                                }
                                if (prop == 'TranEntrDetah70') {

                                    var contDia = cont / 1440; line1.push([contDia, 4 * item[prop]])
                                    cont += 15;
                                }
                                if (prop == 'TranEntrDetah71') {

                                    var contDia = cont / 1440; line1.push([contDia, 4 * item[prop]])
                                    cont += 15;
                                }
                                if (prop == 'TranEntrDetah72') {

                                    var contDia = cont / 1440; line1.push([contDia, 4 * item[prop]])
                                    cont += 15;
                                }
                                if (prop == 'TranEntrDetah73') {

                                    var contDia = cont / 1440; line1.push([contDia, 4 * item[prop]])
                                    cont += 15;
                                }
                                if (prop == 'TranEntrDetah74') {

                                    var contDia = cont / 1440; line1.push([contDia, 4 * item[prop]])
                                    cont += 15;
                                }
                                if (prop == 'TranEntrDetah75') {

                                    var contDia = cont / 1440; line1.push([contDia, 4 * item[prop]])
                                    cont += 15;
                                }
                                if (prop == 'TranEntrDetah76') {

                                    var contDia = cont / 1440; line1.push([contDia, 4 * item[prop]])
                                    cont += 15;
                                }
                                if (prop == 'TranEntrDetah77') {

                                    var contDia = cont / 1440; line1.push([contDia, 4 * item[prop]])
                                    cont += 15;
                                }
                                if (prop == 'TranEntrDetah78') {

                                    var contDia = cont / 1440; line1.push([contDia, 4 * item[prop]])
                                    cont += 15;
                                }
                                if (prop == 'TranEntrDetah79') {

                                    var contDia = cont / 1440; line1.push([contDia, 4 * item[prop]])
                                    cont += 15;
                                }
                                if (prop == 'TranEntrDetah80') {

                                    var contDia = cont / 1440; line1.push([contDia, 4 * item[prop]])
                                    cont += 15;
                                }
                                if (prop == 'TranEntrDetah81') {

                                    var contDia = cont / 1440; line1.push([contDia, 4 * item[prop]])
                                    cont += 15;
                                }
                                if (prop == 'TranEntrDetah82') {

                                    var contDia = cont / 1440; line1.push([contDia, 4 * item[prop]])
                                    cont += 15;
                                }
                                if (prop == 'TranEntrDetah83') {

                                    var contDia = cont / 1440; line1.push([contDia, 4 * item[prop]])
                                    cont += 15;
                                }
                                if (prop == 'TranEntrDetah84') {

                                    var contDia = cont / 1440; line1.push([contDia, 4 * item[prop]])
                                    cont += 15;
                                }
                                if (prop == 'TranEntrDetah85') {

                                    var contDia = cont / 1440; line1.push([contDia, 4 * item[prop]])
                                    cont += 15;
                                }
                                if (prop == 'TranEntrDetah86') {

                                    var contDia = cont / 1440; line1.push([contDia, 4 * item[prop]])
                                    cont += 15;
                                }
                                if (prop == 'TranEntrDetah87') {

                                    var contDia = cont / 1440; line1.push([contDia, 4 * item[prop]])
                                    cont += 15;
                                }
                                if (prop == 'TranEntrDetah88') {

                                    var contDia = cont / 1440; line1.push([contDia, 4 * item[prop]])
                                    cont += 15;
                                }
                                if (prop == 'TranEntrDetah89') {

                                    var contDia = cont / 1440; line1.push([contDia, 4 * item[prop]])
                                    cont += 15;
                                }
                                if (prop == 'TranEntrDetah90') {

                                    var contDia = cont / 1440; line1.push([contDia, 4 * item[prop]])
                                    cont += 15;
                                }
                                if (prop == 'TranEntrDetah91') {

                                    var contDia = cont / 1440; line1.push([contDia, 4 * item[prop]])
                                    cont += 15;
                                }
                                if (prop == 'TranEntrDetah92') {

                                    var contDia = cont / 1440; line1.push([contDia, 4 * item[prop]])
                                    cont += 15;
                                }
                                if (prop == 'TranEntrDetah93') {

                                    var contDia = cont / 1440; line1.push([contDia, 4 * item[prop]])
                                    cont += 15;
                                }
                                if (prop == 'TranEntrDetah94') {

                                    var contDia = cont / 1440; line1.push([contDia, 4 * item[prop]])
                                    cont += 15;
                                }
                                if (prop == 'TranEntrDetah95') {

                                    var contDia = cont / 1440; line1.push([contDia, 4 * item[prop]])
                                    cont += 15;
                                }
                                if (prop == 'TranEntrDetah96') {

                                    var contDia = cont / 1440; line1.push([contDia, 4 * item[prop]])
                                    cont += 15;
                                }
                                //console.log("o." + prop + " = " + 4*item[prop] + " " + prop[prop.length -1]);
                            }
                            //}
                            //line1.push([item.TranEntrDetadia, item.TranEntrDetaPromDia])
                        });
                    }
                    else {
                        $.each(dataER, function (index, item) {
                            for (var prop in item) {

                                if (prop == 'TranRetiDetah1') {


                                    var contDia = cont / 1440; line1.push([contDia, 4 * item[prop]]);
                                    cont += 15;


                                }
                                if (prop == 'TranRetiDetah2') {

                                    var contDia = cont / 1440; line1.push([contDia, 4 * item[prop]]);
                                    cont += 15;

                                }
                                if (prop == 'TranRetiDetah3') {

                                    var contDia = cont / 1440; line1.push([contDia, 4 * item[prop]]);
                                    cont += 15;

                                }
                                if (prop == 'TranRetiDetah4') {
                                    var contDia = cont / 1440; line1.push([contDia, 4 * item[prop]]);
                                    cont += 15;
                                }
                                if (prop == 'TranRetiDetah5') {
                                    var contDia = cont / 1440; line1.push([contDia, 4 * item[prop]]);
                                    cont += 15;
                                }
                                if (prop == 'TranRetiDetah6') {
                                    var contDia = cont / 1440; line1.push([contDia, 4 * item[prop]]);
                                    cont += 15;
                                }
                                if (prop == 'TranRetiDetah7') {
                                    var contDia = cont / 1440; line1.push([contDia, 4 * item[prop]]);
                                    cont += 15;
                                }
                                if (prop == 'TranRetiDetah8') {
                                    var contDia = cont / 1440; line1.push([contDia, 4 * item[prop]]);
                                    cont += 15;
                                }
                                if (prop == 'TranRetiDetah9') {
                                    var contDia = cont / 1440; line1.push([contDia, 4 * item[prop]]);
                                    cont += 15;
                                }

                                if (prop == 'TranRetiDetah10') {
                                    var contDia = cont / 1440; line1.push([contDia, 4 * item[prop]]);
                                    cont += 15;
                                }
                                if (prop == 'TranRetiDetah11') {

                                    var contDia = cont / 1440; line1.push([contDia, 4 * item[prop]])
                                    cont += 15;
                                }
                                if (prop == 'TranRetiDetah12') {

                                    var contDia = cont / 1440; line1.push([contDia, 4 * item[prop]])
                                    cont += 15;
                                }
                                if (prop == 'TranRetiDetah13') {

                                    var contDia = cont / 1440; line1.push([contDia, 4 * item[prop]])
                                    cont += 15;
                                }
                                if (prop == 'TranRetiDetah14') {

                                    var contDia = cont / 1440; line1.push([contDia, 4 * item[prop]])
                                    cont += 15;
                                }
                                if (prop == 'TranRetiDetah15') {

                                    var contDia = cont / 1440; line1.push([contDia, 4 * item[prop]])
                                    cont += 15;
                                }
                                if (prop == 'TranRetiDetah16') {

                                    var contDia = cont / 1440; line1.push([contDia, 4 * item[prop]])
                                    cont += 15;
                                }
                                if (prop == 'TranRetiDetah17') {

                                    var contDia = cont / 1440; line1.push([contDia, 4 * item[prop]])
                                    cont += 15;
                                }
                                if (prop == 'TranRetiDetah18') {

                                    var contDia = cont / 1440; line1.push([contDia, 4 * item[prop]])
                                    cont += 15;
                                }
                                if (prop == 'TranRetiDetah19') {

                                    var contDia = cont / 1440; line1.push([contDia, 4 * item[prop]])
                                    cont += 15;
                                }
                                if (prop == 'TranRetiDetah20') {

                                    var contDia = cont / 1440; line1.push([contDia, 4 * item[prop]])
                                    cont += 15;
                                }
                                if (prop == 'TranRetiDetah21') {

                                    var contDia = cont / 1440; line1.push([contDia, 4 * item[prop]])
                                    cont += 15;
                                }
                                if (prop == 'TranRetiDetah22') {

                                    var contDia = cont / 1440; line1.push([contDia, 4 * item[prop]])
                                    cont += 15;
                                }
                                if (prop == 'TranRetiDetah23') {

                                    var contDia = cont / 1440; line1.push([contDia, 4 * item[prop]])
                                    cont += 15;
                                }
                                if (prop == 'TranRetiDetah24') {

                                    var contDia = cont / 1440; line1.push([contDia, 4 * item[prop]])
                                    cont += 15;
                                }
                                if (prop == 'TranRetiDetah25') {

                                    var contDia = cont / 1440; line1.push([contDia, 4 * item[prop]])
                                    cont += 15;
                                }
                                if (prop == 'TranRetiDetah26') {

                                    var contDia = cont / 1440; line1.push([contDia, 4 * item[prop]])
                                    cont += 15;
                                }
                                if (prop == 'TranRetiDetah27') {

                                    var contDia = cont / 1440; line1.push([contDia, 4 * item[prop]])
                                    cont += 15;
                                }
                                if (prop == 'TranRetiDetah28') {

                                    var contDia = cont / 1440; line1.push([contDia, 4 * item[prop]])
                                    cont += 15;
                                }
                                if (prop == 'TranRetiDetah29') {

                                    var contDia = cont / 1440; line1.push([contDia, 4 * item[prop]])
                                    cont += 15;
                                }
                                if (prop == 'TranRetiDetah30') {

                                    var contDia = cont / 1440; line1.push([contDia, 4 * item[prop]])
                                    cont += 15;
                                }
                                if (prop == 'TranRetiDetah31') {

                                    var contDia = cont / 1440; line1.push([contDia, 4 * item[prop]])
                                    cont += 15;
                                }
                                if (prop == 'TranRetiDetah32') {

                                    var contDia = cont / 1440; line1.push([contDia, 4 * item[prop]])
                                    cont += 15;
                                }
                                if (prop == 'TranRetiDetah33') {

                                    var contDia = cont / 1440; line1.push([contDia, 4 * item[prop]])
                                    cont += 15;
                                }
                                if (prop == 'TranRetiDetah34') {

                                    var contDia = cont / 1440; line1.push([contDia, 4 * item[prop]])
                                    cont += 15;
                                }
                                if (prop == 'TranRetiDetah35') {

                                    var contDia = cont / 1440; line1.push([contDia, 4 * item[prop]])
                                    cont += 15;
                                }
                                if (prop == 'TranRetiDetah36') {

                                    var contDia = cont / 1440; line1.push([contDia, 4 * item[prop]])
                                    cont += 15;
                                }
                                if (prop == 'TranRetiDetah37') {

                                    var contDia = cont / 1440; line1.push([contDia, 4 * item[prop]])
                                    cont += 15;
                                }
                                if (prop == 'TranRetiDetah38') {

                                    var contDia = cont / 1440; line1.push([contDia, 4 * item[prop]])
                                    cont += 15;
                                }
                                if (prop == 'TranRetiDetah39') {

                                    var contDia = cont / 1440; line1.push([contDia, 4 * item[prop]])
                                    cont += 15;
                                }
                                if (prop == 'TranRetiDetah40') {

                                    var contDia = cont / 1440; line1.push([contDia, 4 * item[prop]])
                                    cont += 15;
                                }
                                if (prop == 'TranRetiDetah41') {

                                    var contDia = cont / 1440; line1.push([contDia, 4 * item[prop]])
                                    cont += 15;
                                }
                                if (prop == 'TranRetiDetah42') {

                                    var contDia = cont / 1440; line1.push([contDia, 4 * item[prop]])
                                    cont += 15;
                                }
                                if (prop == 'TranRetiDetah43') {

                                    var contDia = cont / 1440; line1.push([contDia, 4 * item[prop]])
                                    cont += 15;
                                }
                                if (prop == 'TranRetiDetah44') {

                                    var contDia = cont / 1440; line1.push([contDia, 4 * item[prop]])
                                    cont += 15;
                                }
                                if (prop == 'TranRetiDetah45') {

                                    var contDia = cont / 1440; line1.push([contDia, 4 * item[prop]])
                                    cont += 15;
                                }
                                if (prop == 'TranRetiDetah46') {

                                    var contDia = cont / 1440; line1.push([contDia, 4 * item[prop]])
                                    cont += 15;
                                }
                                if (prop == 'TranRetiDetah47') {

                                    var contDia = cont / 1440; line1.push([contDia, 4 * item[prop]])
                                    cont += 15;
                                }
                                if (prop == 'TranRetiDetah48') {

                                    var contDia = cont / 1440; line1.push([contDia, 4 * item[prop]])
                                    cont += 15;
                                }
                                if (prop == 'TranRetiDetah49') {

                                    var contDia = cont / 1440; line1.push([contDia, 4 * item[prop]])
                                    cont += 15;
                                }
                                if (prop == 'TranRetiDetah50') {

                                    var contDia = cont / 1440; line1.push([contDia, 4 * item[prop]])
                                    cont += 15;
                                }
                                if (prop == 'TranRetiDetah51') {

                                    var contDia = cont / 1440; line1.push([contDia, 4 * item[prop]])
                                    cont += 15;
                                }
                                if (prop == 'TranRetiDetah52') {

                                    var contDia = cont / 1440; line1.push([contDia, 4 * item[prop]])
                                    cont += 15;
                                }
                                if (prop == 'TranRetiDetah53') {

                                    var contDia = cont / 1440; line1.push([contDia, 4 * item[prop]])
                                    cont += 15;
                                }
                                if (prop == 'TranRetiDetah54') {

                                    var contDia = cont / 1440; line1.push([contDia, 4 * item[prop]])
                                    cont += 15;
                                }
                                if (prop == 'TranRetiDetah55') {

                                    var contDia = cont / 1440; line1.push([contDia, 4 * item[prop]])
                                    cont += 15;
                                }
                                if (prop == 'TranRetiDetah56') {

                                    var contDia = cont / 1440; line1.push([contDia, 4 * item[prop]])
                                    cont += 15;
                                }
                                if (prop == 'TranRetiDetah57') {

                                    var contDia = cont / 1440; line1.push([contDia, 4 * item[prop]])
                                    cont += 15;
                                }
                                if (prop == 'TranRetiDetah58') {

                                    var contDia = cont / 1440; line1.push([contDia, 4 * item[prop]])
                                    cont += 15;
                                }
                                if (prop == 'TranRetiDetah59') {

                                    var contDia = cont / 1440; line1.push([contDia, 4 * item[prop]])
                                    cont += 15;
                                }
                                if (prop == 'TranRetiDetah60') {

                                    var contDia = cont / 1440; line1.push([contDia, 4 * item[prop]])
                                    cont += 15;
                                }
                                if (prop == 'TranRetiDetah61') {

                                    var contDia = cont / 1440; line1.push([contDia, 4 * item[prop]])
                                    cont += 15;
                                }
                                if (prop == 'TranRetiDetah62') {

                                    var contDia = cont / 1440; line1.push([contDia, 4 * item[prop]])
                                    cont += 15;
                                }
                                if (prop == 'TranRetiDetah63') {

                                    var contDia = cont / 1440; line1.push([contDia, 4 * item[prop]])
                                    cont += 15;
                                }
                                if (prop == 'TranRetiDetah64') {

                                    var contDia = cont / 1440; line1.push([contDia, 4 * item[prop]])
                                    cont += 15;
                                }
                                if (prop == 'TranRetiDetah65') {

                                    var contDia = cont / 1440; line1.push([contDia, 4 * item[prop]])
                                    cont += 15;
                                }
                                if (prop == 'TranRetiDetah66') {

                                    var contDia = cont / 1440; line1.push([contDia, 4 * item[prop]])
                                    cont += 15;
                                }
                                if (prop == 'TranRetiDetah67') {

                                    var contDia = cont / 1440; line1.push([contDia, 4 * item[prop]])
                                    cont += 15;
                                }
                                if (prop == 'TranRetiDetah68') {

                                    var contDia = cont / 1440; line1.push([contDia, 4 * item[prop]])
                                    cont += 15;
                                }
                                if (prop == 'TranRetiDetah69') {

                                    var contDia = cont / 1440; line1.push([contDia, 4 * item[prop]])
                                    cont += 15;
                                }
                                if (prop == 'TranRetiDetah70') {

                                    var contDia = cont / 1440; line1.push([contDia, 4 * item[prop]])
                                    cont += 15;
                                }
                                if (prop == 'TranRetiDetah71') {

                                    var contDia = cont / 1440; line1.push([contDia, 4 * item[prop]])
                                    cont += 15;
                                }
                                if (prop == 'TranRetiDetah72') {

                                    var contDia = cont / 1440; line1.push([contDia, 4 * item[prop]])
                                    cont += 15;
                                }
                                if (prop == 'TranRetiDetah73') {

                                    var contDia = cont / 1440; line1.push([contDia, 4 * item[prop]])
                                    cont += 15;
                                }
                                if (prop == 'TranRetiDetah74') {

                                    var contDia = cont / 1440; line1.push([contDia, 4 * item[prop]])
                                    cont += 15;
                                }
                                if (prop == 'TranRetiDetah75') {

                                    var contDia = cont / 1440; line1.push([contDia, 4 * item[prop]])
                                    cont += 15;
                                }
                                if (prop == 'TranRetiDetah76') {

                                    var contDia = cont / 1440; line1.push([contDia, 4 * item[prop]])
                                    cont += 15;
                                }
                                if (prop == 'TranRetiDetah77') {

                                    var contDia = cont / 1440; line1.push([contDia, 4 * item[prop]])
                                    cont += 15;
                                }
                                if (prop == 'TranRetiDetah78') {

                                    var contDia = cont / 1440; line1.push([contDia, 4 * item[prop]])
                                    cont += 15;
                                }
                                if (prop == 'TranRetiDetah79') {

                                    var contDia = cont / 1440; line1.push([contDia, 4 * item[prop]])
                                    cont += 15;
                                }
                                if (prop == 'TranRetiDetah80') {

                                    var contDia = cont / 1440; line1.push([contDia, 4 * item[prop]])
                                    cont += 15;
                                }
                                if (prop == 'TranRetiDetah81') {

                                    var contDia = cont / 1440; line1.push([contDia, 4 * item[prop]])
                                    cont += 15;
                                }
                                if (prop == 'TranRetiDetah82') {

                                    var contDia = cont / 1440; line1.push([contDia, 4 * item[prop]])
                                    cont += 15;
                                }
                                if (prop == 'TranRetiDetah83') {

                                    var contDia = cont / 1440; line1.push([contDia, 4 * item[prop]])
                                    cont += 15;
                                }
                                if (prop == 'TranRetiDetah84') {

                                    var contDia = cont / 1440; line1.push([contDia, 4 * item[prop]])
                                    cont += 15;
                                }
                                if (prop == 'TranRetiDetah85') {

                                    var contDia = cont / 1440; line1.push([contDia, 4 * item[prop]])
                                    cont += 15;
                                }
                                if (prop == 'TranRetiDetah86') {

                                    var contDia = cont / 1440; line1.push([contDia, 4 * item[prop]])
                                    cont += 15;
                                }
                                if (prop == 'TranRetiDetah87') {

                                    var contDia = cont / 1440; line1.push([contDia, 4 * item[prop]])
                                    cont += 15;
                                }
                                if (prop == 'TranRetiDetah88') {

                                    var contDia = cont / 1440; line1.push([contDia, 4 * item[prop]])
                                    cont += 15;
                                }
                                if (prop == 'TranRetiDetah89') {

                                    var contDia = cont / 1440; line1.push([contDia, 4 * item[prop]])
                                    cont += 15;
                                }
                                if (prop == 'TranRetiDetah90') {

                                    var contDia = cont / 1440; line1.push([contDia, 4 * item[prop]])
                                    cont += 15;
                                }
                                if (prop == 'TranRetiDetah91') {

                                    var contDia = cont / 1440; line1.push([contDia, 4 * item[prop]])
                                    cont += 15;
                                }
                                if (prop == 'TranRetiDetah92') {

                                    var contDia = cont / 1440; line1.push([contDia, 4 * item[prop]])
                                    cont += 15;
                                }
                                if (prop == 'TranRetiDetah93') {

                                    var contDia = cont / 1440; line1.push([contDia, 4 * item[prop]])
                                    cont += 15;
                                }
                                if (prop == 'TranRetiDetah94') {

                                    var contDia = cont / 1440; line1.push([contDia, 4 * item[prop]])
                                    cont += 15;
                                }
                                if (prop == 'TranRetiDetah95') {

                                    var contDia = cont / 1440; line1.push([contDia, 4 * item[prop]])
                                    cont += 15;
                                }
                                if (prop == 'TranRetiDetah96') {

                                    var contDia = cont / 1440; line1.push([contDia, 4 * item[prop]])
                                    cont += 15;
                                }

                                //console.log("o." + prop + " = " + 4*item[prop] + " " + prop[prop.length -1]);
                            }
                        });
                    }
                    var a = { label: dataCodigo, lineWidth: 0.5, markerOptions: { size: 0.5, style: 'dimaond' }, showMarker: false };
                    labels.push(a);
                    var cantidadTotal = line1.length - 1536;
                    line2 = line1.splice(1536, cantidadTotal);
                    series.push(line1);
                    seriesB.push(line2);
                    //console.log(cantidadTotal);
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
                        label: 'MW.H',
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
                        label: 'MW.H',
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
                            testJqPlotIB(result.dataIB, result.dataCodigo);
                        }
                    },
                    error: function () {
                        mostrarError();
                    }
                });

                function testJqPlotIB(dataIB, dataCodigo) {
                    //console.log(dataIB)
                    //console.log(dataCodigo)
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
                alert('Lo sentimos, No hay elemntos que borrar');
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
        iVersion = $("#VersionG").val();
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

        //if ($("#EMPRCODI3").multipleSelect('getSelects') == "") {
        //    alert("Por favor, seleccione una Empresa...");
        //}
        //else
        //{
        //    iVersion = $("#VersionG").val();
        //    iPeriCodi = $("#PERICODI3").val();
        //    var iEmprCodi = $("#EMPRCODI3 option:selected").val();
        //    $.ajax({
        //        type: 'POST',
        //        url: controlador + 'descargarentregaretiro',
        //        dataType: 'json',
        //        data: { PeriCodi: iPeriCodi, Version: iVersion, EmprCodi: iEmprCodi },
        //        success: function (result) {
        //            if (result == 1) {
        //                window.location = controlador + 'abrirentregaretiro';
        //            }
        //            if (result == -1) {
        //                alert("Lo sentimos, se ha producido un error");
        //            }
        //        },
        //        error: function () {
        //            alert("Lo sentimos, se ha producido un error");
        //        }
        //    });
        //}
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
    var sBarrcodi;
    var sEmprcodi;
    if ($("#PericodiC").val() == "") {
        alert("Seleccione un Periodo");
    }
    else {
        sPericodi = $("#PericodiC").val();
        sVersion = $("#VersionC").val();
        if ($("#BARRCODI").val() == "") {
            sBarrcodi = null;
        }
        else {
            sBarrcodi = $("#BARRCODI").val();
        }
        if ($("#EMPRCODI5 option:selected").val() == "") {
            sEmprcodi = null;
        }
        else {
            sEmprcodi = $("#EMPRCODI5 option:selected").val();
        }
        $.ajax({
            type: 'POST',
            url: controlador + 'descargarenergiamensual',
            dataType: 'json',
            data: { sPericodi: sPericodi, sVersion: sVersion, sBarrcodi: sBarrcodi, sEmprcodi: sEmprcodi },
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