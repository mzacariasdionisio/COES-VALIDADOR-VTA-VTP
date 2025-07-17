var controler = siteRoot + "transferencias/factorperdida/";
var imageRoot = siteRoot + "Content/Images/";
var series = [];
var seriesB = [];
var labels = [];
var popAjusteIntervalo;
var dtAjusteIntervalo;
$(document).ready(function () {
    
    $('#BarrcodiE').multipleSelect({
        width: '200px',
        filter: true,
        selectAll: false,
        single: true
    });

    $('#popDia').multipleSelect({
        width: '200px',
        selectAll: false,
        single: true,
        placeholder: 'Día',
    });

    $('#popIntervalo').multipleSelect({
        width: '200px',
        selectAll: false,
        single: true,
        placeholder: 'Intervalo',
    });

    document.getElementById('divOpcionCarga').style.display = "none";
    $("#Version").prop("disabled", true);
    $("#Pericodi").change(function () {
        if ($("#Pericodi").val() != "") {
            var options = {};
            options.url = controler + "GetVersion";
            options.type = "POST";
            options.data = JSON.stringify({ pericodi: $("#Pericodi").val() });
            options.dataType = "json";
            options.contentType = "application/json";
            options.success = function (modelo) {
                //console.log(modelo.ListaRecalculo);
                //console.log(modelo.bEjecutar);
                $("#Version").empty();
                $("#Version").removeAttr("disabled");
                $.each(modelo.ListaRecalculo, function (k, v) {
                    var option = '<option value=' + v.RecaCodi + '>' + v.RecaNombre + '</option>';
                    $('#Version').append(option);
                    //console.log(option);
                }             
                );
                if (modelo.bEjecutar == true)
                { document.getElementById('divOpcionCarga').style.display = "block"; }
                else
                { document.getElementById('divOpcionCarga').style.display = "none"; }
                $('#contentTabla').css('display', 'none');
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

    $("#VersionA").prop("disabled", true);
    $("#PericodiE").change(function () {
        if ($("#PericodiE").val() != "") {
            var options = {};
            options.url = controler + "GetVersion";
            options.type = "POST";
            options.data = JSON.stringify({ pericodi: $("#PericodiE").val() });
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
            options.error = function () { alert("No se logro cargar las versiones de recalculo!"); };
            $.ajax(options);
        }
        else {
            $("#VersionA").empty();
            $("#VersionA").prop("disabled", true);
        }
    });

    $("#VersionB").prop("disabled", true);
    $("#PericodiG").change(function () {
        if ($("#PericodiG").val() != "") {
            var options = {};
            options.url = controler + "GetVersion";
            options.type = "POST";
            options.data = JSON.stringify({ pericodi: $("#PericodiG").val() });
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

    $('#btnVer').click(function () {
        document.getElementById("divResultado").style.display = "";
        LimpiarGrafico();
        listaBarrCostoMarginal();
    });

    $('#Clean').click(function () {
        LimpiarGrafico();
    });

    $('#btnGenerarCostoMarginal').click(function () {
        generarCostoMarginal();
    });

    $('#btnGenerarCMBarra').click(function () {
        generarCMBarra();

    });

    $('#btnGenerarporBarra').click(function () {
        generarPorBarra();
    });

    $('#btnImportarSGOCOES').click(function () {
        importarSGOCOES();
    });

    $('#btnAjustarIntervalos').click(function () {        
        popAjusteIntervalo = $('#popAjusteIntervalo').bPopup({
            easing: 'easeOutBack',
            speed: 350,
            transition: 'fadeIn',
            modalClose: false,
        }, function () {
            datosPopupAjusteIntervalos();
        });
    });

    $('#popAgregar').click(function () {
        const dia = $("#popDia").val();
        const intervalo = $("#popIntervalo").val();
        if (!dia || !intervalo) return false; 

        const str = `${dia} ${intervalo}`;        
        registrarAjusteIntervalo(str);
    })

    dtAjusteIntervalo = $('#tbAjusteIntervalo').DataTable({
        data: [],
        columns: [
            { data: "TrncmafechaStr", title: "Día" },
            { data: "TrncmafechaStr", title: "Intervalo" },
            { data: "TrncmafeccreacionStr", title: "Fecha / Hora remplazar" },
            { data: "Trncmausumodificacion", title: "Usuario que importo" },
            { data: "TrncmafecmodificacionStr", title: "Fecha / Hora que importo" },
            { title: '', data: null },
        ],
        columnDefs: [
            {
                targets: -1,
                defaultContent: `<img src="${imageRoot}btn-cancel.png" class="popEliminar" title="Eliminar registro"/>`,
            },
            {
                targets: [0],
                createdCell: function (td, cellData, rowData, row, col) {
                    
                    const [dia, hora] = rowData.TrncmafechaStr.split(" ");
                    $(td).html(dia);
                },
            },
            {
                targets: [1],
                createdCell: function (td, cellData, rowData, row, col) {
                    const [dia, hora] = rowData.TrncmafechaStr.split(" ");
                    $(td).html(hora);
                },
            }
        ],        
        searching: false,
        bLengthChange: false,
        bSort: false,
        destroy: true,
        info: false
    });

});

listaBarrCostoMarginal = function () {
    var cbo = $("#PericodiG option:selected").val();

    if (cbo == "") {
        alert('Por favor, seleccione el Periodo');
    }
    else {
        peri = $("#PericodiG option:selected").val();
        var version = $("#VersionB option:selected").val();
 
        $.ajax({
            url: controler + 'getListCostoMarginal',
            type: 'GET',
            data: { periodo: peri, vers: version },
            success: function (result) {

                lista(result.dataBar);
            },
            error: function () {
                alert('error a listar')
            }
        });
        $('#tabla').empty();
        function lista(data) {
            $.each(data, function (k, v) {
                var sListaEntrega = "";
                sListaEntrega += "<tr>";
                sListaEntrega += '<td style="text-align:center"><a href=# id=view_' + v.BarrCodi + ' ' + 'class=Vergrafico' + '><img  src="../../../Areas/Transferencias/Content/Images/view.gif" title="Insertar linea en el grafico" alt="Insertar linea en el grafico" /></a></td>';
                sListaEntrega += '<td style="text-align:left">' + v.CosMarBarraTransferencia + '</td>';
                sListaEntrega += '<td style="text-align:center"><a style="display:box;float:right" href=# id=view_' + v.CosMarBarraTransferencia + ' ' + 'class=Del ><img  src="../../../Areas/Transferencias/Content/Images/tachito.gif" title="Retirar linea del grafico" alt="Retirar linea del grafico" /></a></td>';
                sListaEntrega += '</tr>';
                $('#tabla').append(sListaEntrega);
            });
            add_Ver();
            Del();
        }
    }

}

function add_Ver() {
    $(".Vergrafico").click(function (e) {
        document.getElementById('btnBotones').style.display = "block";
        e.preventDefault();
        id = $(this).attr("id").split("_")[1];
        peri = $("#PericodiG option:selected").val();

        $.ajax({
            type: "GET",
            dataType: "json",
            url: controler + "FetchGraphData/",
            data:
                {
                    periodo: peri,
                    barCodi: id
                },
            success: function (result) {

                if ($.isEmptyObject(result.dataProm)) {
                    alert("Lo sentimos, no hay resultados");
                } else {
                    testJqPlot(result.dataProm, result.dataNomb);
                }
            },
            error: function () {
                mostrarError();
            }
        });

        function testJqPlot(dataBar, dataCodigoNom) {
            var line1 = [];
            var existe = 0;
            var cont = 1440;

            for (var key in labels)
            {
                var a = labels[key];
                if (a.label == (dataCodigoNom))  existe = 1;
            }
            if (existe == 1) {
                alert('existe');
            }
            else {
                $.each(dataBar, function (index, item) {
                    for (var prop in item) {

                        if (prop == 'CosMar1') {
                            var contDia = cont / 1440; line1.push([contDia, item[prop]]);
                            cont += 15;
                        }
                        if (prop == 'CosMar2') {
                            var contDia = cont / 1440; line1.push([contDia, item[prop]]);
                            cont += 15;
                        }
                        if (prop == 'CosMar3') {
                            var contDia = cont / 1440; line1.push([contDia, item[prop]]);
                            cont += 15;
                        }
                        if (prop == 'CosMar4') {
                            var contDia = cont / 1440; line1.push([contDia, item[prop]]);
                            cont += 15;
                        }
                        if (prop == 'CosMar5') {
                            var contDia = cont / 1440; line1.push([contDia, item[prop]]);
                            cont += 15;
                        }
                        if (prop == 'CosMar6') {
                            var contDia = cont / 1440; line1.push([contDia, item[prop]]);
                            cont += 15;
                        }
                        if (prop == 'CosMar7') {
                            var contDia = cont / 1440; line1.push([contDia, item[prop]]);
                            cont += 15;
                        }
                        if (prop == 'CosMar8') {
                            var contDia = cont / 1440; line1.push([contDia, item[prop]]);
                            cont += 15;
                        }
                        if (prop == 'CosMar9') {
                            var contDia = cont / 1440; line1.push([contDia, item[prop]]);
                            cont += 15;
                        }
                        if (prop == 'CosMar10') {
                            var contDia = cont / 1440; line1.push([contDia, item[prop]]);
                            cont += 15;
                        }
                        if (prop == 'CosMar11') {
                            var contDia = cont / 1440; line1.push([contDia, item[prop]])
                            cont += 15;
                        }
                        if (prop == 'CosMar12') {
                            var contDia = cont / 1440; line1.push([contDia, item[prop]])
                            cont += 15;
                        }
                        if (prop == 'CosMar13') {
                            var contDia = cont / 1440; line1.push([contDia, item[prop]])
                            cont += 15;
                        }
                        if (prop == 'CosMar14') {
                            var contDia = cont / 1440; line1.push([contDia, item[prop]])
                            cont += 15;
                        }
                        if (prop == 'CosMar15') {
                            var contDia = cont / 1440; line1.push([contDia, item[prop]])
                            cont += 15;
                        }
                        if (prop == 'CosMar16') {
                            var contDia = cont / 1440; line1.push([contDia, item[prop]])
                            cont += 15;
                        }
                        if (prop == 'CosMar17') {
                            var contDia = cont / 1440; line1.push([contDia, item[prop]])
                            cont += 15;
                        }
                        if (prop == 'CosMar18') {
                            var contDia = cont / 1440; line1.push([contDia, item[prop]])
                            cont += 15;
                        }
                        if (prop == 'CosMar19') {

                            var contDia = cont / 1440; line1.push([contDia, item[prop]])
                            cont += 15;
                        }
                        if (prop == 'CosMar20') {

                            var contDia = cont / 1440; line1.push([contDia, item[prop]])
                            cont += 15;
                        }
                        if (prop == 'CosMar21') {

                            var contDia = cont / 1440; line1.push([contDia, item[prop]])
                            cont += 15;
                        }
                        if (prop == 'CosMar22') {

                            var contDia = cont / 1440; line1.push([contDia, item[prop]])
                            cont += 15;
                        }
                        if (prop == 'CosMar23') {

                            var contDia = cont / 1440; line1.push([contDia, item[prop]])
                            cont += 15;
                        }
                        if (prop == 'CosMar24') {

                            var contDia = cont / 1440; line1.push([contDia, item[prop]])
                            cont += 15;
                        }
                        if (prop == 'CosMar25') {

                            var contDia = cont / 1440; line1.push([contDia, item[prop]])
                            cont += 15;
                        }
                        if (prop == 'CosMar26') {

                            var contDia = cont / 1440; line1.push([contDia, item[prop]])
                            cont += 15;
                        }
                        if (prop == 'CosMar27') {

                            var contDia = cont / 1440; line1.push([contDia, item[prop]])
                            cont += 15;
                        }
                        if (prop == 'CosMar28') {

                            var contDia = cont / 1440; line1.push([contDia, item[prop]])
                            cont += 15;
                        }
                        if (prop == 'CosMar29') {

                            var contDia = cont / 1440; line1.push([contDia, item[prop]])
                            cont += 15;
                        }
                        if (prop == 'CosMar30') {

                            var contDia = cont / 1440; line1.push([contDia, item[prop]])
                            cont += 15;
                        }
                        if (prop == 'CosMar31') {

                            var contDia = cont / 1440; line1.push([contDia, item[prop]])
                            cont += 15;
                        }
                        if (prop == 'CosMar32') {

                            var contDia = cont / 1440; line1.push([contDia, item[prop]])
                            cont += 15;
                        }
                        if (prop == 'CosMar33') {

                            var contDia = cont / 1440; line1.push([contDia, item[prop]])
                            cont += 15;
                        }
                        if (prop == 'CosMar34') {

                            var contDia = cont / 1440; line1.push([contDia, item[prop]])
                            cont += 15;
                        }
                        if (prop == 'CosMar35') {

                            var contDia = cont / 1440; line1.push([contDia, item[prop]])
                            cont += 15;
                        }
                        if (prop == 'CosMar36') {

                            var contDia = cont / 1440; line1.push([contDia, item[prop]])
                            cont += 15;
                        }
                        if (prop == 'CosMar37') {

                            var contDia = cont / 1440; line1.push([contDia, item[prop]])
                            cont += 15;
                        }
                        if (prop == 'CosMar38') {

                            var contDia = cont / 1440; line1.push([contDia, item[prop]])
                            cont += 15;
                        }
                        if (prop == 'CosMar39') {

                            var contDia = cont / 1440; line1.push([contDia, item[prop]])
                            cont += 15;
                        }
                        if (prop == 'CosMar40') {

                            var contDia = cont / 1440; line1.push([contDia, item[prop]])
                            cont += 15;
                        }
                        if (prop == 'CosMar41') {

                            var contDia = cont / 1440; line1.push([contDia, item[prop]])
                            cont += 15;
                        }
                        if (prop == 'CosMar42') {

                            var contDia = cont / 1440; line1.push([contDia, item[prop]])
                            cont += 15;
                        }
                        if (prop == 'CosMar43') {

                            var contDia = cont / 1440; line1.push([contDia, item[prop]])
                            cont += 15;
                        }
                        if (prop == 'CosMar44') {

                            var contDia = cont / 1440; line1.push([contDia, item[prop]])
                            cont += 15;
                        }
                        if (prop == 'CosMar45') {

                            var contDia = cont / 1440; line1.push([contDia, item[prop]])
                            cont += 15;
                        }
                        if (prop == 'CosMar46') {

                            var contDia = cont / 1440; line1.push([contDia, item[prop]])
                            cont += 15;
                        }
                        if (prop == 'CosMar47') {

                            var contDia = cont / 1440; line1.push([contDia, item[prop]])
                            cont += 15;
                        }
                        if (prop == 'CosMar48') {

                            var contDia = cont / 1440; line1.push([contDia, item[prop]])
                            cont += 15;
                        }
                        if (prop == 'CosMar49') {

                            var contDia = cont / 1440; line1.push([contDia, item[prop]])
                            cont += 15;
                        }
                        if (prop == 'CosMar50') {

                            var contDia = cont / 1440; line1.push([contDia, item[prop]])
                            cont += 15;
                        }
                        if (prop == 'CosMar51') {

                            var contDia = cont / 1440; line1.push([contDia, item[prop]])
                            cont += 15;
                        }
                        if (prop == 'CosMar52') {

                            var contDia = cont / 1440; line1.push([contDia, item[prop]])
                            cont += 15;
                        }
                        if (prop == 'CosMar53') {

                            var contDia = cont / 1440; line1.push([contDia, item[prop]])
                            cont += 15;
                        }
                        if (prop == 'CosMar54') {

                            var contDia = cont / 1440; line1.push([contDia, item[prop]])
                            cont += 15;
                        }
                        if (prop == 'CosMar55') {

                            var contDia = cont / 1440; line1.push([contDia, item[prop]])
                            cont += 15;
                        }
                        if (prop == 'CosMar56') {

                            var contDia = cont / 1440; line1.push([contDia, item[prop]])
                            cont += 15;
                        }
                        if (prop == 'CosMar57') {

                            var contDia = cont / 1440; line1.push([contDia, item[prop]])
                            cont += 15;
                        }
                        if (prop == 'CosMar58') {

                            var contDia = cont / 1440; line1.push([contDia, item[prop]])
                            cont += 15;
                        }
                        if (prop == 'CosMar59') {

                            var contDia = cont / 1440; line1.push([contDia, item[prop]])
                            cont += 15;
                        }
                        if (prop == 'CosMar60') {

                            var contDia = cont / 1440; line1.push([contDia, item[prop]])
                            cont += 15;
                        }
                        if (prop == 'CosMar61') {

                            var contDia = cont / 1440; line1.push([contDia, item[prop]])
                            cont += 15;
                        }
                        if (prop == 'CosMar62') {

                            var contDia = cont / 1440; line1.push([contDia, item[prop]])
                            cont += 15;
                        }
                        if (prop == 'CosMar63') {

                            var contDia = cont / 1440; line1.push([contDia, item[prop]])
                            cont += 15;
                        }
                        if (prop == 'CosMar64') {

                            var contDia = cont / 1440; line1.push([contDia, item[prop]])
                            cont += 15;
                        }
                        if (prop == 'CosMar65') {

                            var contDia = cont / 1440; line1.push([contDia, item[prop]])
                            cont += 15;
                        }
                        if (prop == 'CosMar66') {

                            var contDia = cont / 1440; line1.push([contDia, item[prop]])
                            cont += 15;
                        }
                        if (prop == 'CosMar67') {

                            var contDia = cont / 1440; line1.push([contDia, item[prop]])
                            cont += 15;
                        }
                        if (prop == 'CosMar68') {

                            var contDia = cont / 1440; line1.push([contDia, item[prop]])
                            cont += 15;
                        }
                        if (prop == 'CosMar69') {

                            var contDia = cont / 1440; line1.push([contDia, item[prop]])
                            cont += 15;
                        }
                        if (prop == 'CosMar70') {

                            var contDia = cont / 1440; line1.push([contDia, item[prop]])
                            cont += 15;
                        }
                        if (prop == 'CosMar71') {

                            var contDia = cont / 1440; line1.push([contDia, item[prop]])
                            cont += 15;
                        }
                        if (prop == 'CosMar72') {

                            var contDia = cont / 1440; line1.push([contDia, item[prop]])
                            cont += 15;
                        }
                        if (prop == 'CosMar73') {

                            var contDia = cont / 1440; line1.push([contDia, item[prop]])
                            cont += 15;
                        }
                        if (prop == 'CosMar74') {

                            var contDia = cont / 1440; line1.push([contDia, item[prop]])
                            cont += 15;
                        }
                        if (prop == 'CosMar75') {

                            var contDia = cont / 1440; line1.push([contDia, item[prop]])
                            cont += 15;
                        }
                        if (prop == 'CosMar76') {

                            var contDia = cont / 1440; line1.push([contDia, item[prop]])
                            cont += 15;
                        }
                        if (prop == 'CosMar77') {

                            var contDia = cont / 1440; line1.push([contDia, item[prop]])
                            cont += 15;
                        }
                        if (prop == 'CosMar78') {

                            var contDia = cont / 1440; line1.push([contDia, item[prop]])
                            cont += 15;
                        }
                        if (prop == 'CosMar79') {

                            var contDia = cont / 1440; line1.push([contDia, item[prop]])
                            cont += 15;
                        }
                        if (prop == 'CosMar80') {

                            var contDia = cont / 1440; line1.push([contDia, item[prop]])
                            cont += 15;
                        }
                        if (prop == 'CosMar81') {

                            var contDia = cont / 1440; line1.push([contDia, item[prop]])
                            cont += 15;
                        }
                        if (prop == 'CosMar82') {

                            var contDia = cont / 1440; line1.push([contDia, item[prop]])
                            cont += 15;
                        }
                        if (prop == 'CosMar83') {

                            var contDia = cont / 1440; line1.push([contDia, item[prop]])
                            cont += 15;
                        }
                        if (prop == 'CosMar84') {

                            var contDia = cont / 1440; line1.push([contDia, item[prop]])
                            cont += 15;
                        }
                        if (prop == 'CosMar85') {

                            var contDia = cont / 1440; line1.push([contDia, item[prop]])
                            cont += 15;
                        }
                        if (prop == 'CosMar86') {

                            var contDia = cont / 1440; line1.push([contDia, item[prop]])
                            cont += 15;
                        }
                        if (prop == 'CosMar87') {

                            var contDia = cont / 1440; line1.push([contDia, item[prop]])
                            cont += 15;
                        }
                        if (prop == 'CosMar88') {

                            var contDia = cont / 1440; line1.push([contDia, item[prop]])
                            cont += 15;
                        }
                        if (prop == 'CosMar89') {

                            var contDia = cont / 1440; line1.push([contDia, item[prop]])
                            cont += 15;
                        }
                        if (prop == 'CosMar90') {

                            var contDia = cont / 1440; line1.push([contDia, item[prop]])
                            cont += 15;
                        }
                        if (prop == 'CosMar91') {

                            var contDia = cont / 1440; line1.push([contDia, item[prop]])
                            cont += 15;
                        }
                        if (prop == 'CosMar92') {

                            var contDia = cont / 1440; line1.push([contDia, item[prop]])
                            cont += 15;
                        }
                        if (prop == 'CosMar93') {

                            var contDia = cont / 1440; line1.push([contDia, item[prop]])
                            cont += 15;
                        }
                        if (prop == 'CosMar94') {

                            var contDia = cont / 1440; line1.push([contDia, item[prop]])
                            cont += 15;
                        }
                        if (prop == 'CosMar95') {

                            var contDia = cont / 1440; line1.push([contDia, item[prop]])
                            cont += 15;
                        }
                        if (prop == 'CosMar96') {

                            var contDia = cont / 1440; line1.push([contDia, item[prop]])
                            cont += 15;
                        }

                    }

                    //line1.push([item.CosMarDia, (item.CosMarPromedioDia)*4])
                });

                var a = { label: dataCodigoNom, lineWidth: 0.5, markerOptions: { size: 0.5, style: 'dimaond' }, showMarker: false };
                labels.push(a);

                var cantidadTotal = line1.length - 1536;
                line2 = line1.splice(1536, cantidadTotal);
                series.push(line1);
                seriesB.push(line2);
                grafico();
            }
        }

    });
};

grafico = function () {
    if (series.length > 0) {
        $("#grafico").empty();
        plot1 = $.jqplot('grafico', series,
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
                        min: 1,
                        max: 17,
                        numberTicks: 9
                    },
                    yaxis: {
                        //label: 'KW.H',
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
                        pad: 0,
                        min: 17,
                        max: 32,
                        tickOptions: { formatString: "%#.2f" },
                    },
                    yaxis: {
                        //label: 'Energia',
                        tickOptions: { formatString: "%#.6f" },
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
        //Tamaño de la grafica autoajustable -> Solo funciona bien si hay una sola grafica
        //$('#grafico').width($('#divResultado').width() * 0.75);
        ////$('#grafico').height($('#divResultado').height() * 0.4);
        //$('#grafico2').width($('#divResultado').width() * 0.75);
    }
    else {
        $("#grafico").empty();
        $("#grafico2").empty();
    }

    $('#reset').click(function () { plot1.resetZoom(), plot2.resetZoom() }); // 
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

generarCostoMarginal = function () {
    if ($("#PericodiE").val() == "") {
        mostrarMensaje("Por favor, seleccione un periodo");
    }

    else {

      sPericodi = $("#PericodiE").val();
      var  version = $("#VersionA").val();
        $.ajax({
            type: 'POST',
            url: controler + 'generarcostomarginal',
            data: { sPericodi: sPericodi, vers: version },
            dataType: 'json',
            success: function (result) {
                if (result == 1) {
                    window.location = controler + 'abrircostomarginal';
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

generarCMBarra = function () {
    if ($("#PericodiE").val() == "") {
        mostrarMensaje("Por favor, seleccione un periodo");
    }

else if ($("#BarrcodiE").multipleSelect('getSelects')  == "") {
        mostrarMensaje("Por favor, seleccione una barra de transferencia");
    }
    else {
        sPericodi = $("#PericodiE").val();
        sBarrcodi = $("#BarrcodiE option:selected").val();
        var version = $("#VersionA").val();
        $.ajax({
            type: 'POST',
            url: controler + 'generarcmbarra',
            data: { sPericodi: sPericodi, sBarrcodi: sBarrcodi, vers: version },
            dataType: 'json',
            success: function (result) {
                if (result == 1) {
                    window.location = controler + 'abrircmbarra';
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

generarPorBarra = function () {
    if ($("#PericodiE").val() == "") {
        mostrarMensaje("Por favor, seleccione un periodo");
    }
    else if ($("#BarrcodiE").multipleSelect('getSelects') == "") {
        mostrarMensaje("Por favor, seleccione una barra de transferencia");
    }
    else {
        sPericodi = $("#PericodiE").val();
        sBarrcodi = $("#BarrcodiE option:selected").val();
        var version = $("#VersionA").val();
        $.ajax({
            type: 'POST',
            url: controler + 'generarporbarra',
            data: { sPericodi: sPericodi, sBarrcodi: sBarrcodi,vers:version },
            dataType: 'json',
            success: function (result) {
                if (result == 1) {
                    window.location = controler + 'abrirporbarra';
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

function ConfigurarCostoMarginal()
{   var fullDate = new Date();
    var twoDigitMonth = ((fullDate.getMonth().length) == 1) ? '0' + (fullDate.getMonth() + 1) : (fullDate.getMonth() + 1);
    var sFecha = fullDate.getFullYear().toString() + twoDigitMonth.toString() + fullDate.getDate().toString() + fullDate.getHours().toString() + fullDate.getMinutes().toString() + fullDate.getSeconds().toString();
    var uploader = new plupload.Uploader({
        runtimes: 'html5,flash,silverlight,html4',
        browse_button: 'btnSelectFile',
        container: document.getElementById('container'),
        url: controler + 'upload?sFecha=' + sFecha,
        flash_swf_url: '~/Content/Scripts/Moxie.swf',
        silverlight_xap_url: '~/Content/Scripts/Moxie.xap',
        multi_selection: false,
        filters: {
            max_file_size: '100mb',
            mime_types: [
                { title: "Archivos Excel .xlsx", extensions: "xlsx" }
            ]
        },
        init: {
            PostInit: function () {
                document.getElementById('btnProcesarFile').onclick = function () {
                    if ($("#Pericodi").val() == "") {
                        mostrarMensaje("Por favor, seleccione un periodo");
                    }
                    else {
                        if (uploader.files.length > 0) {
                            uploader.start();
                        }
                        else
                            loadValidacionFile("Seleccione archivo.");
                    }
                    return false;
                };
            },
            FilesAdded: function (up, files) {
                if (uploader.files.length == 2) {
                    uploader.removeFile(uploader.files[0]);
                }
                plupload.each(files, function (file) {
                    loadInfoFile(file.name, plupload.formatSize(file.size));
                });
                up.refresh();
            },
            UploadProgress: function (up, file) {
                mostrarProgreso(file.percent);
            },
            UploadComplete: function (up, file) {
                procesarArchivo(sFecha + "_" + file[0].name, $("#Pericodi").val());
            },
            Error: function (up, err) {
                loadValidacionFile(err.code + "-" + err.message);
            }
        }
    });
    uploader.init();
}

procesarArchivo = function (sFile, sPericodi) {
    var ValorMax = $('#txtValorMaximo').val();
    var version = $("#Version").val();
    $.ajax({
        type: 'POST',
        url: controler + 'procesararchivo',
        data: { sNombreArchivo: sFile, sPericodi: sPericodi, dValorMax: ValorMax, vers: version },
        dataType: 'json',
        cache: false,
        success: function (resultado) {
            if (resultado == "1") {
                mostrarMensaje("Archivo procesado");
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
//ASSETEC 202002
importarSGOCOES = function () {
    if ($("#Pericodi").val() == "") {
        mostrarMensaje("Por favor, seleccione un periodo");
    }
    else {
        pericodi = $("#Pericodi").val();
        var version = $("#Version").val();
        $.ajax({
            type: 'POST',
            url: controler + 'copiarsgocoes',
            data: { pericodi: pericodi, version: version },
            dataType: 'json',
            //async: false,
            success: function (result) {
                //console.log(result);
                pintarLogFaltante(result);
                $('#contentTabla').css('display', 'block');
                alert("El procedimiento se ejecuto satisfactoriamente");
            },
            error: function () {
                alert("Lo sentimos, se ha producido un error");
            }
        });
    }
}

pintarLogFaltante = function (result) {
    var html = '';
    html = html + ' <table class="pretty tabla-adicional">';
    html = html + ' <thead>';
    html = html + '     <tr>';
    html = html + '         <th>Barra</th>';
    html = html + '         <th>Días en que se completaron datos</th>';
    html = html + '     </tr>';
    html = html + ' </thead>';
    html = html + ' <tbody>';
    for (var i in result) {
        var ListaFechasFaltantes = result[i].ListaFecFaltantes;
        var style = '';
        if (i % 2 == 0) style = 'background-color: #f2f5f7';
        html = html + '     <tr>';
        html = html + '         <td style="text-align:left;' + style + '">' + result[i].NombreBarra + '</td>';
        html = html + '         <td style="text-align:left;' + style + '">';
        for (var k in ListaFechasFaltantes) {
            //console.log(ListaFechasFaltantes[k]);
            html = html + ListaFechasFaltantes[k] + ' / ';
        }
        html = html + '         </td>';
        html = html + '     </tr>';
        i++;
    }
    html = html + ' </tbody>';
    html = html + '</table>';
    html = html + '';
    $('#contentTabla').html(html);
}

//ASSETEC 20220908
datosPopupAjusteIntervalos = function () {
    $.ajax({
        type: "POST",
        dataType: "json",
        url: controler + "AjustarIntervalos/",
        data: {
            idPeriodo: $("#Pericodi").val(),
            idVersion: $("#Version").val(),
        },
        success: function (result) {
            const nomPeriodo = $("#Pericodi option:selected")
                .text();
            const nomVersion = $("#Version option:selected")
                .text();
            $("#nomPeriodo").text(nomPeriodo);
            $("#nomVersion").text(nomVersion);

            const dias = result.dias;
            const intervalos = result.intervalos;
            recargarListaDesplegable($('#popDia'), dias);
            recargarListaDesplegable($('#popIntervalo'), intervalos);
            listarAjusteIntervalos();
        },
        error: function () {
            mostrarError();
        }
    });

}

registrarAjusteIntervalo = function (str) {    
    $.ajax({
        type: "POST",
        dataType: "json",
        url: controler + "RegistrarAjusteIntervalo/",
        data: {
            idPeriodo: $("#Pericodi").val(),
            idVersion: $("#Version").val(),
            ajuste: str,
        },
        success: function (result) {
            if (Number.isInteger(result))
                listarAjusteIntervalos();
            else
                mostrarMensaje(result);
        },
        error: function () {
            mostrarError();
        }
    });
}

eliminarAjusteIntervalo = function (id) {
    $.ajax({
        type: "POST",
        dataType: "json",
        url: controler + "EliminarAjusteIntervalo/",
        data: {
            idRegistro: id,
        },
        success: function (result) {
            listarAjusteIntervalos();
        },
        error: function () {
            mostrarError();
        }
    });
}

listarAjusteIntervalos = function () {
    $.ajax({
        type: "POST",
        dataType: "json",
        url: controler + "ListarAjusteIntervalo/",
        data: {
            idPeriodo: $("#Pericodi").val(),
            idVersion: $("#Version").val(),
        },
        success: function (result) {
            dtAjusteIntervalo.clear();
            dtAjusteIntervalo.rows
                .add(result)
                .draw();
            console.log(result, "_res");
        },
        error: function () {
            mostrarError();
        }
    });
}

$(document).on('click', '#tbAjusteIntervalo tr td img.popEliminar', function () {
    var tr = $(this).closest('tr');
    var row = dtAjusteIntervalo.row(tr);
    var rowData = row.data();
    eliminarAjusteIntervalo(rowData.Trncmacodi);
});

function recargarListaDesplegable(element, data) {
    element.empty();
    $.each(data, function (i, item) {          
        element.append($('<option></option>')
            .val(item)
            .html(item));
    });
    element.multipleSelect('refresh');
}
