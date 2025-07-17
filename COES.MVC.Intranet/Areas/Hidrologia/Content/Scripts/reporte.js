var controlador = siteRoot + 'hidrologia/';
var tipoInformacion = 0;
var opc = 0;
var listLectPeriodo = [];
var listLectCodi = [];
var currentValue = 0;
var flag = 0;
var flag2 = 0;

$(function () {
    $('#cbEmpresa').multipleSelect({
        width: '222px',
        filter: true
    });
    $('#cbCuenca').multipleSelect({
        width: '222px',
        filter: true
    });
    $('#cbFamilia').multipleSelect({
        width: '222px',
        filter: true
    });
    $('#cbPtoMedicion').multipleSelect({
        width: '222px',
        filter: true
    });
    $('#cbTipoCentral').multipleSelect({
        width: '180px',
        filter: true
    });
    $('#cbCentral').multipleSelect({
        width: '222px',
        filter: true
    });
    $('#cbUnidades').change(function () {
        listarPuntoMedicion();
    });
    $('#FechaDesde').Zebra_DatePicker({

    });

    $('#FechaHasta').Zebra_DatePicker({

    });
    $('#Anho').Zebra_DatePicker({
        format: 'Y',
        onSelect: function () {
            cargarSemanaAnho()
        }
    });
    $('#AnhoInicio').Zebra_DatePicker({
        format: 'Y'
    });
    $('#AnhoFin').Zebra_DatePicker({
        format: 'Y'
    });
    $('#cbTipoInformacion').change(function () {
        if ($(this).val() == 67)//PROGRAMA DIARIO CP
            flag2 = 1;
        else
            flag2 = 0;
        cambiarFormatoFecha(buscarTipoInformacion($(this).val()));

        $('#listado').html("");
        $('#paginado').html("");
    });
    $('#btnBuscar').click(function () {
        $('#listado').html("");
        $('#paginado').html("");
        $('#btnExpotar').show();
        var resultado = validarFiltros();
        if (resultado == "") {
            buscarDatos();
            //activaBtn();
        }
        else {
            alert("Error:" + resultado);
        }
    });
    $('#btnGrafico').click(function () {
        $('#paginado').html("");
        var tipoInformacion = buscarTipoInformacion($('#cbTipoInformacion').val());
        var valor = $("input[name='rbidTipo']:checked").val();

        //if ((tipoInformacion == 1) && (valor == 0)) // Diario x horas
        //    pintarPaginado(0);
        generarGrafico(1);
        $('#btnExpotar').hide();
    });
    $('#btnExpotar').click(function () {
        exportarExcel();
    });
    cargarPrevio();
    buscarDatos();
    cargarSemanaAnho()
});

$(document).ready(function () {
    cambiarFormatoFecha(buscarTipoInformacion($(cbTipoInformacion).val()));

});
function cargarPrevio() {
    var strLectCodi = $('#hfLectCodi').val();
    var strLectPeriodo = $('#hfLectPeriodo').val();
    listLectCodi = strLectCodi.split(',');
    listLectPeriodo = strLectPeriodo.split(',');
    flag = 0;
    flag2 = 0;
    tipoInformacion = 0;
    currentValue = 0;
    $('#cbEmpresa').multipleSelect('checkAll');
    $('#cbCuenca').multipleSelect('checkAll');
    $('#cbPtoMedicion').multipleSelect('checkAll');
    $('#cbFamilia').multipleSelect('checkAll');
    $('#cbTipoInformacion').val($("#hfLectura").val());
    $('input[name=rbidTipo][value=0]').attr('checked', true);
    //$('input[name=rbidTipo][value=1]').attr('disabled', true);
    $('#cbUnidades').val(11);
    listarPuntoMedicion();
    var fecha = new Date();
    var stFecha = fecha.getFullYear();
    $('#Anho').val(stFecha);
    $('#AnhoInicio').val(stFecha);
    $('#AnhoFin').val(stFecha);
    $('#SemanaIni select').val(1);
    $('#SemanaFin select').val(1);
    mostrarOcultarFiltroSemanal(0)
    mostrarOcultarFiltroAnhos(0);

    //desactivaBtn();
    //mostrarOcultarRbTipoGrafico(0);
    //mostrarOcultarFiltrosFechas(0);

}

function validarFiltros() {
    var empresa = $('#cbEmpresa').multipleSelect('getSelects');
    var cuenca = $('#cbCuenca').multipleSelect('getSelects');
    var tipoInformacion = $('#cbTipoInformacion').val();
    var resultado = "";
    var fini = $('#FechaDesde').val();
    var ffin = $('#FechaHasta').val();
    var semanaIni = $('#SemanaIni select').val();
    var semanaFin = $('#SemanaFin select').val();

    if (empresa == "") {
        resultado = "Debe seleccionar una empresa."
        return resultado;
    }
    if (cuenca == "") {
        resultado = "Debe selecionar una cuenca";
        return resultado;
    }


    if (tipoInformacion == 2) { // rpte semanal        
        var com = Number(semanaIni) > Number(semanaFin);
        if (semanaIni == 0) {
            resultado = "Debe seleccionar una semana de inicio";
            return resultado;
        }
        if (semanaFin == 0) {
            resultado = "Debe seleccionar una semana fin";
            return resultado;
        }
        if (com) {
            resultado = "La semana inicio no puede ser mayor a la semana final";
            return resultado;
        }
    }
    if (tipoInformacion == 3) { // rpte mensual        
        if ((process(fini)) > (process(ffin))) {
            resultado = "El mes inicial no puede ser mayor que el mes final’";
            return resultado;
        }
    }
    else {
        if ((process(fini)) > (process(ffin))) {
            resultado = "La fecha inicial no puede ser mayor que la fecha final’";
            return resultado;
        }
    }
    return resultado;
}

function process(date) { //format YYYY/MM/DD
    var tipoInformacion = $('#cbTipoInformacion').val();

    if (tipoInformacion == 25 || tipoInformacion == 26) { //Mensual
        var parts = date.split(" ");
        return new Date(parts[1], parts[0] - 1, 1);
    }
    var parts = date.split("/");
    return new Date(parts[2], parts[1] - 1, parts[0]);
}

function mostrarOcultarFiltroSemanal(opc) {
    if (opc == 1) {
        $('#idTr td').show();
    }
    if (opc == 0) {
        $('#idTr td').hide();
    }
}

function mostrarOcultarRbTipoGrafico(opc) {
    if (opc == 1) {
        $('#idTr2 td').show();
    }
    if (opc == 0) {
        $('#idTr2 td').hide();
    }
}

function mostrarOcultarFiltrosFechas(opc) {
    if (opc == 1) {
        $('#idTr3 td').show();
    }
    if (opc == 0) {
        $('#idTr3 td').hide();
    }
}

function mostrarOcultarFiltroAnhos(opc) {
    if (opc == 1) {
        $('#idTr4 td').show();
    }
    if (opc == 0) {
        $('#idTr4 td').hide();
    }
}

function activaBtn() {
    $('#btnGrafico').show();
    $('#btnExpotar').show();
}

function desactivaBtn() {
    $('#btnGrafico').hide();
    $('#btnExpotar').hide();
}

function cargarSemanaAnho() {
    var anho = $('#Anho').val();
    $('#hfAnho').val(anho);
    $.ajax({
        type: 'POST',
        url: controlador + 'reporte/CargarSemanas',

        data: { idAnho: $('#hfAnho').val() },

        success: function (aData) {

            $('#SemanaIni').html(aData);
            $('#SemanaFin').html(aData);
        },
        error: function () {
            alert("Ha ocurrido un error");
        }
    });

}

function cambiarFormatoFecha(tipo) {
    switch (tipo) {
        case '3':// Reporte mensual
            $('#FechaDesde').Zebra_DatePicker({
                format: 'm Y'
            });
            $('#FechaHasta').Zebra_DatePicker({
                format: 'm Y'
            });

            var fecha = new Date();
            var mes = "0" + (fecha.getMonth() + 1).toString();
            mes = mes.substr(mes.length - 2, mes.length);
            var stFecha = mes + " " + fecha.getFullYear();
            $('#FechaDesde').val(stFecha);
            $('#FechaHasta').val(stFecha);
            //mostrarOcultarRbTipoGrafico(0)
            mostrarOcultarFiltroSemanal(0);
            mostrarOcultarFiltrosFechas(1);
            mostrarOcultarFiltroAnhos(0);
            $("#divHoras").css("display", "none");
            $("#divSemana1").css("display", "none");
            $("#divSemana2").css("display", "none");
            $("#divDia").css("display", "none");
            $('input[name=rbidTipo][value=4]').prop('checked', true);
            break;
        case '5': // Reporte Semanal
        case '2':
            mostrarOcultarFiltrosFechas(0)
            mostrarOcultarFiltroSemanal(1);
            mostrarOcultarFiltroAnhos(0);
            $("#divHoras").css("display", "none");
            $("#divDia").css("display", "none");
            $("#divSemana1").css("display", "inline-block");
            //$("#divSemana2").css("display", "inline-block");
            $("#divMes").css("display", "inline-block");
            $('input[name=rbidTipo][value=2]').prop('checked', true);
            break;
        default:
            mostrarOcultarFiltroSemanal(0);
            mostrarOcultarFiltrosFechas(1)
            mostrarOcultarFiltroAnhos(0);
            $('#FechaDesde').Zebra_DatePicker({
            });
            $('#FechaHasta').Zebra_DatePicker({
            });
            var fecha = new Date();
            var dia = "0" + fecha.getDate().toString();
            dia = dia.substr(dia.length - 2, dia.length);
            var mes = "0" + (fecha.getMonth() + 1).toString();
            mes = mes.substr(mes.length - 2, mes.length);
            var stFecha = dia + "/" + mes + "/" + fecha.getFullYear();
            $('#FechaDesde').val(stFecha);
            $('#FechaHasta').val(stFecha);
            if (flag2 == 1) {
                $('input[name=rbidTipo][value=1]').prop('checked', true);
                //$('input[name=rbidTipo][value=0]').attr('disabled', true);
                $("#divHoras").css("display", "none");
            }
            else {
                $('input[name=rbidTipo][value=0]').prop('checked', true);
                $("#divHoras").css("display", "inline-block");
            }
            $("#divSemana1").css("display", "inline-block");
            //$("#divSemana2").css("display", "inline-block");
            $("#divMes").css("display", "inline-block");
            $("#divDia").css("display", "inline-block");

    }

}

function activarOpcionesRbTipoGrafico(opc) {
    switch (opc) {
        case '3':// Reporte mensual            
            $('input[name=rbidTipo][value=0]').attr('disabled', true);
            $('input[name=rbidTipo][value=1]').attr('disabled', true);
            $('input[name=rbidTipo][value=2]').attr('disabled', true);
            $('input[name=rbidTipo][value=3]').attr('disabled', true);
            $('input[name=rbidTipo][value=4]').attr('disabled', false);
            $('input[name=rbidTipo][value=5]').attr('disabled', false);

            break;
        case '5': // Reporte Semanal
        case '2':
            $('input[name=rbidTipo][value=0]').attr('disabled', true);
            $('input[name=rbidTipo][value=1]').attr('disabled', true);
            $('input[name=rbidTipo][value=2]').attr('disabled', false);
            $('input[name=rbidTipo][value=3]').attr('disabled', false);
            $('input[name=rbidTipo][value=4]').attr('disabled', false);
            $('input[name=rbidTipo][value=5]').attr('disabled', false);


            //$('input[name=rbidTipo][value=2]').attr('checked', true);
            break;
        default:
            $('input[name=rbidTipo][value=0]').attr('disabled', false);
            $('input[name=rbidTipo][value=1]').attr('disabled', false);
            $('input[name=rbidTipo][value=2]').attr('disabled', false);
            $('input[name=rbidTipo][value=3]').attr('disabled', false);
            $('input[name=rbidTipo][value=4]').attr('disabled', false);
            $('input[name=rbidTipo][value=5]').attr('disabled', false);
            //$('input[name=rbidTipo][value=1]').attr('checked', true);           
    }

}

function cambiarFormatoFecha2(tipo) {
    switch (tipo) {
        case '0': case '1':// Horas / diario
            mostrarOcultarFiltroSemanal(0);
            mostrarOcultarFiltrosFechas(1)
            mostrarOcultarFiltroAnhos(0);
            $('#FechaDesde').Zebra_DatePicker({
            });
            $('#FechaHasta').Zebra_DatePicker({
            });
            if (flag == 1) {
                var fecha = new Date();
                var dia = "0" + fecha.getDate().toString();
                dia = dia.substr(dia.length - 2, dia.length);
                var mes = "0" + (fecha.getMonth() + 1).toString();
                mes = mes.substr(mes.length - 2, mes.length);
                var stFecha = dia + "/" + mes + "/" + fecha.getFullYear();
                $('#FechaDesde').val(stFecha);
                $('#FechaHasta').val(stFecha)
                flag = 0;
            }

            break;
        case '2': case '3': //Semanal programado/cronológico
            mostrarOcultarFiltrosFechas(0)
            mostrarOcultarFiltroSemanal(1);
            mostrarOcultarFiltroAnhos(0);
            break;
        case '4':// Reporte mensual
            $('#FechaDesde').Zebra_DatePicker({
                format: 'm Y'
            });
            $('#FechaHasta').Zebra_DatePicker({
                format: 'm Y'
            });

            var fecha = new Date();
            var mes = "0" + (fecha.getMonth() + 1).toString();
            mes = mes.substr(mes.length - 2, mes.length);
            var stFecha = mes + " " + fecha.getFullYear();
            $('#FechaDesde').val(stFecha);
            $('#FechaHasta').val(stFecha);
            flag = 1;
            //mostrarOcultarRbTipoGrafico(0)
            mostrarOcultarFiltroSemanal(0);
            mostrarOcultarFiltrosFechas(1);
            mostrarOcultarFiltroAnhos(0);
            break;
        case '5': // Reporte Anual
            mostrarOcultarFiltroSemanal(0);
            mostrarOcultarFiltrosFechas(0);
            mostrarOcultarFiltroAnhos(1);
            break;
            //default:
            //    mostrarOcultarFiltroSemanal(0);
            //    mostrarOcultarFiltrosFechas(1)
            //    $('#FechaDesde').Zebra_DatePicker({
            //    });
            //    $('#FechaHasta').Zebra_DatePicker({
            //    });
            //    var fecha = new Date();
            //    var dia = "0" + fecha.getDate().toString();
            //    dia = dia.substr(dia.length - 2, dia.length);
            //    var mes = "0" + (fecha.getMonth() + 1).toString();
            //    mes = mes.substr(mes.length - 2, mes.length);
            //    var stFecha = dia + "/" + mes + "/" + fecha.getFullYear();
            //    $('#FechaDesde').val(stFecha);
            //    $('#FechaHasta').val(stFecha)
    }

}

function buscarDatos() {
    var tipoInformacion = buscarTipoInformacion($('#cbTipoInformacion').val());
    var valor = $("input[name='rbidTipo']:checked").val();
    $('#reporte').css("display", "block");
    $('#graficos').css("display", "none");
    if ((tipoInformacion == 1) && (valor == 0)) {// Diario x horas
        pintarPaginado(1);
    }
    //if (($('#cbTipoInformacion').val() == 63) && (valor == 1))
    //    pintarPaginado(1);
    //if (($('#cbTipoInformacion').val() == 66) && (valor == 1))
    //    pintarPaginado(1);
    mostrarListado(1);
}

function mostrarListado(nroPagina) {
    var empresa = $('#cbEmpresa').multipleSelect('getSelects');
    var cuenca = $('#cbCuenca').multipleSelect('getSelects');
    var familia = $('#cbFamilia').multipleSelect('getSelects');
    var ptomedicion = $('#cbPtoMedicion').multipleSelect('getSelects');
    var anho = $('#Anho').val();
    var anhoIni = $('#AnhoInicio').val();
    var anhoFin = $('#AnhoFin').val();
    var semanaIni = $('#SemanaIni select').val();
    var semanaFin = $('#SemanaFin select').val();
    var unidades = $('#cbUnidades').val();
    if (semanaIni == undefined) semanaIni = 0;
    if (semanaFin == undefined) semanaFin = 1;
    if (empresa == "[object Object]") empresa = "-1";
    if (empresa == "") empresa = "-1";
    if (cuenca == "[object Object]") cuenca = "-1";
    if (cuenca == "") cuenca = "-1";
    if (familia == "[object Object]") cuenca = "-1";
    if (familia == "") cuenca = "-1";
    if (ptomedicion == "[object Object]") ptomedicion = "-1";
    if (ptomedicion == "") ptomedicion = "-1";

    if (cuenca != "-1" && $('input[type=checkbox][name=selectItemIdCuenca]').not(':checked').length == 0){
        cuenca = "-1";
    }

    $('#hfEmpresa').val(empresa);
    $('#hfCuenca').val(cuenca);
    $('#hfFamilia').val(familia);
    $('#hfPtoMedicion').val(ptomedicion);
    $('#hfAnho').val(anho);
    $('#hfAnhoInicio').val(anhoIni);
    $('#hfAnhoFin').val(anhoFin);
    $('#hfSemanaIni').val(semanaIni);
    $('#hfSemanaFin').val(semanaFin);
    var valor = $("input[name='rbidTipo']:checked").val();
    $('#hfidTipo').val(valor.toString());
    $('#hfUnidad').val(unidades);
    
    $.ajax({
        type: 'POST',
        url: controlador + "reporte/lista",
        data: {
            idsEmpresa: $('#hfEmpresa').val(), idsCuenca: $('#hfCuenca').val(),idsFamilia: $('#hfFamilia').val(),
            idLectura: $('#cbTipoInformacion').val(),
            idsPtoMedicion: $('#hfPtoMedicion').val(), fechaInicial: $('#FechaDesde').val(), fechaFinal: $('#FechaHasta').val(),
            nroPagina: nroPagina, anho: $('#hfAnho').val(), anhoInicial: $('#hfAnhoInicio').val(), anhoFinal: $('#hfAnhoFin').val(),
            semanaIni: $('#hfSemanaIni').val(), semanaFin: $('#hfSemanaFin').val(),
            opcion: opc, rbDetalleRpte: $('#hfidTipo').val(), unidad: $('#hfUnidad').val()
        },
        success: function (evt) {
            $('#listado').css("width", $('#mainLayout').width() + "px");

            $('#listado').html(evt);
            if ($('#tabla th').length > 1) {
                $('#tabla').dataTable({
                    "bAutoWidth": false,
                    "bSort": false,
                    "scrollY": 320,
                    "scrollX": true,
                    "sDom": 't',
                    "iDisplayLength": 50
                });
            }
        },
        error: function () {
            alert("Ha ocurrido un error en lista");
        }
    });
}

function aoColumns() {
    var ao = [];
    $("#tabla th").each(function (i, th) {
        switch (i) {
            case 0:
                ao.push({ "sWidth": "70px" });
                break;
            default:
                ao.push({ "sWidth": "100px" });
                break;
        }
    });
    return ao;
}

function generarGrafico(nroPagina) {

    $('#reporte').css("display", "none");
    //$('#paginado').css("display", "none");   
    tipoInformacion = $('#cbTipoInformacion').val();
    $('#excelGrafico').css("display", "block");
    $('#excelGrafico').html("<img onclick='exportarGrafico();' width='32px' style='cursor: pointer; display: inline;' src='" + siteRoot + "Content/Images/ExportExcel.png' />");
    $('#graficos').css("display", "block");
    var anho = $('#Anho').val();
    var semanaIni = $('#SemanaIni select').val();
    var semanaFin = $('#SemanaFin select').val();
    var anhoIni = $('#AnhoInicio').val();
    var anhoFin = $('#AnhoFin').val();

    if (semanaIni == undefined) semanaIni = 0;
    if (semanaFin == undefined) semanaFin = 1;
    var empresa = $('#cbEmpresa').multipleSelect('getSelects');
    var cuenca = $('#cbCuenca').multipleSelect('getSelects');
    var familia = $('#cbFamilia').multipleSelect('getSelects');
    var ptomedicion = $('#cbPtoMedicion').multipleSelect('getSelects');
    var unidades = $('#cbUnidades').val();
    var fechadesde = $('#FechaDesde').val();
    var fechahasta = $('#FechaHasta').val();
    if (empresa == "[object Object]") empresa = "-1";
    if (empresa == "") empresa = "-1";
    if (cuenca == "[object Object]") cuenca = "-1";
    if (cuenca == "") cuenca = "-1";
    if (familia == "[object Object]") familia = "-1";
    if (familia == "") familia = "-1";
    if (ptomedicion == "[object Object]") ptomedicion = "-1";
    if (ptomedicion == "") ptomedicion = "-1";
    var valor = $("input[name='rbidTipo']:checked").val();
    $('#hfidTipo').val(valor.toString());
    $('#hfEmpresa').val(empresa);
    $('#hfCuenca').val(cuenca);
    $('#hfFamilia').val(familia);
    $('#hfPtoMedicion').val(ptomedicion);
    $('#hfFechaDesde').val(fechadesde);
    $('#hfFechaHasta').val(fechahasta);
    $('#hfAnho').val(anho);
    $('#hfSemanaIni').val(semanaIni);
    $('#hfSemanaFin').val(semanaFin);
    $('#hfUnidad').val(unidades);
    $('#hfAnhoInicio').val(anhoIni);
    $('#hfAnhoFin').val(anhoFin);
    
    $.ajax({
        type: 'POST',
        url: controlador + "reporte/graficoreporte",
        data: {
            fechaInicial: $('#hfFechaDesde').val(), fechaFinal: $('#hfFechaHasta').val(),
            idsEmpresas: $('#hfEmpresa').val(), idLectura: tipoInformacion, idsCuencas: $('#hfCuenca').val(), idsFamilia: $('#hfFamilia').val(),
            nroPagina: nroPagina, anho: $('#hfAnho').val(), semanaIni: $('#hfSemanaIni').val(), semanaFin: $('#hfSemanaFin').val(),
            anhoInicial: $('#hfAnhoInicio').val(), anhoFinal: $('#hfAnhoFin').val(),
            idsPtoMedicion: $('#hfPtoMedicion').val(), rbDetalleRpte: $('#hfidTipo').val(), unidad: $('#hfUnidad').val()
        },
        dataType: 'json',
        success: function (result) {
            var tipo = result.TipoReporte;
            switch (tipo) {
                //case 3:                    
                //    graficoHidrologiaMes(result);                    
                //    break;
                case 1: //EJECUTADO HISTORICO  / PROGRAMA DIARIO -> TABLA ME_MEDICION24
                    graficoHSHidrologiaM24(result);
                    break;
                case 2: //
                    graficoHidrologiaM1(result);
                    break;
                default:
                    break;
            }
        },
        error: function () {
            alert("Ha ocurrido un error en generar grafico");
        }
    });
}

function graficoHSHidrologiaM24(result) {
    var series = [];
    var itemseries = [];
    for (z = 0; z < result.Grafico.Series.length; z++) {
        {
            for (k = 0; k < result.Grafico.Series[z].Data.length; k++) {
                var seriePoint = [];
                var now = parseJsonDate(result.Grafico.Series[z].Data[k].X);
                var nowUTC = Date.UTC(now.getFullYear(), now.getMonth(), now.getDate(), now.getHours(), now.getMinutes(), now.getSeconds());
                seriePoint.push(nowUTC);
                seriePoint.push(result.Grafico.Series[z].Data[k].Y);
                itemseries.push(seriePoint);
            }
            series.push(itemseries);
            itemseries = [];
        }
    }
   // alert(series.length);
    var opcion = {


        title: {
            text: result.Grafico.titleText,
            style: {
                fontSize: '8'
            }
        },
        yAxis: {
            title: {
                text: result.Grafico.YaxixTitle,
                style: {
                    color: 'blue'
                }
            },
            labels: {
                style: {
                    color: 'blue'
                }
            },
            opposite:false
        },
        legend: {
            borderColor: "#909090",
            layout: 'vertical',
            align: 'left',
            verticalAlign: 'top',
            borderWidth: 0,
            enabled: true,
            itemStyle: {
                fontSize: "9px"
            }
        },

        series: []
    }; 
    for (i = 0; i < series.length; i++){
        opcion.series.push({
            name: result.Grafico.Series[i].Name,
            //color: result.Grafico.Series[i].Color,
            data: series[i],
            type: result.Grafico.Series[i].Type
        });
    }
    $('#graficos').highcharts('StockChart', opcion);
}

function graficoHidrologiaM1(result) {
    var opcion = {
        chart: {
            type: 'spline'
        },
        title: {
            text: result.Grafico.TitleText
        },
        xAxis: {

            categories: result.Grafico.XAxisCategories,
            style: {

                fontSize: '5'
            }
        },
        yAxis: {
            title: {
                text: result.Grafico.YaxixTitle
            },
            min: 0
        },
        tooltip: {
            headerFormat: '<b>{series.name}</b><br>',
            pointFormat: '{point.x:%e. %b}: {point.y:.2f} m'
        },

        plotOptions: {
            spline: {
                marker: {
                    enabled: true
                }
            }
        },
        legend: {
            borderColor: "#909090",
            layout: 'vertical',
            align: 'left',
            verticalAlign: 'top',
            borderWidth: 0,
            enabled: true,
            itemStyle: {
                fontSize: "9px"
            }
        },
        series: []
    };
    for (var i in result.Grafico.SeriesName) {
        opcion.series.push({
            name: result.Grafico.SeriesName[i],
            data: result.Grafico.SeriesData[i]
        });
    }
    $('#graficos').highcharts(opcion);

}

function graficoHidrologiaDiarioHighStock(result) {
    var opcion = {
        chart: {
            type: 'spline'
        },
        title: {
            text: result.Grafico.titleText
        },
        subtitle: {
            text: result.Grafico.subtitleText
        },
        xAxis: {

            categories: result.Grafico.xAxisCategories,
            style: {

                fontSize: '5'
            },

            title: {
                text: result.Grafico.xAxisTitle
            },
        },
        yAxis: {
            title: {
                text: result.Grafico.yAxixTitle
            },
            min: 0
        },
        tooltip: {
            headerFormat: '<b>{series.name}</b><br>',
            pointFormat: '{point.x:%e. %b}: {point.y:.2f} m'
        },

        plotOptions: {
            spline: {
                marker: {
                    enabled: true
                }
            }
        },

        series: []
    };
    for (var i in result.Grafico.seriesName) {
        opcion.series.push({
            name: result.Grafico.seriesName[i],
            data: result.Grafico.seriesData[i]
        });
    }
    $('#graficos').highcharts(opcion);
}

function pintarPaginado(id) { //id : tipo de reporte a mostrar 0: reporte grafico, 1: reporte listado
    var empresa = $('#cbEmpresa').multipleSelect('getSelects');
    var cuenca = $('#cbCuenca').multipleSelect('getSelects');
    var familia = $('#cbFamilia').multipleSelect('getSelects');
    var ptomedicion = $('#cbPtoMedicion').multipleSelect('getSelects');

    if (empresa == "[object Object]") empresa = "-1";
    if (empresa == "") empresa = "-1";
    if (cuenca == "[object Object]") cuenca = "-1";
    if (cuenca == "") cuenca = "-1";
    if (familia == "[object Object]") familia = "-1";
    if (familia == "") familia = "-1";

    if (ptomedicion == "[object Object]") ptomedicion = "-1";
    if (ptomedicion == "") ptomedicion = "-1";

    $('#hfEmpresa').val(empresa);
    $('#hfCuenca').val(cuenca);
    $('#hfFamilia').val(familia);
    $('#hfPtoMedicion').val(ptomedicion);

    var idFormatPeriodo = $('#cbTipoInformacion').val();

    $.ajax({
        type: 'POST',
        url: controlador + "reporte/paginado",
        data: {
            idsEmpresa: $('#hfEmpresa').val(), idsCuenca: $('#hfCuenca').val(), idsFamilia: $('#hfFamilia').val(), idLectura: idFormatPeriodo,
            idsPtoMedicion: $('#hfPtoMedicion').val(), fechaInicial: $('#FechaDesde').val(), fechaFinal: $('#FechaHasta').val()
        },
        success: function (evt) {
            $('#paginado').html(evt);
            mostrarPaginado(id);
        },
        error: function () {
            alert("Ha ocurrido un error paginado");
        }
    });
}

function pintarBusqueda(nroPagina, itipo) {
    //itipo : tipo de reporte a mostrar 0: reporte grafico, 1: reporte listado
    if (itipo == 0) {
        generarGrafico(nroPagina);
    }
    else {
        mostrarListado(nroPagina);
    }
}

function exportarExcel() {
    var empresa = $('#cbEmpresa').multipleSelect('getSelects');
    var cuenca = $('#cbCuenca').multipleSelect('getSelects');
    var familia = $('#cbFamilia').multipleSelect('getSelects');
    var ptomedicion = $('#cbPtoMedicion').multipleSelect('getSelects');
    var anho = $('#Anho').val();
    var semanaIni = $('#SemanaIni select').val();
    var semanaFin = $('#SemanaFin select').val();
    if (semanaIni == undefined) semanaIni = 0;
    if (semanaFin == undefined) semanaFin = 1;
    if (empresa == "[object Object]") empresa = "-1";
    if (empresa == "") empresa = "-1";
    if (cuenca == "[object Object]") cuenca = "-1";
    if (cuenca == "") cuenca = "-1";
    if (ptomedicion == "[object Object]") ptomedicion = "-1";
    if (ptomedicion == "") ptomedicion = "-1";
    var unidades = $('#cbUnidades').val();
    var anhoIni = $('#AnhoInicio').val();
    var anhoFin = $('#AnhoFin').val();

    if (cuenca != "-1" && $('input[type=checkbox][name=selectItemIdCuenca]').not(':checked').length == 0) {
        cuenca = "-1";
    }

    $('#hfAnhoInicio').val(anhoIni);
    $('#hfAnhoFin').val(anhoFin);
    $('#hfUnidad').val(unidades);
    $('#hfEmpresa').val(empresa);
    $('#hfCuenca').val(cuenca);
    $('#hfFamilia').val(familia);
    $('#hfPtoMedicion').val(ptomedicion);
    $('#hfAnho').val(anho);
    $('#hfSemanaIni').val(semanaIni);
    $('#hfSemanaFin').val(semanaFin);

    var valor = $("input[name='rbidTipo']:checked").val();
    $('#hfidTipo').val(valor.toString());
    var tipoInformacion = $('#cbTipoInformacion').val();

    $.ajax({
        type: 'POST',
        url: controlador + 'reporte/GenerarArchivoReporteXLS',
        data: {
            idsEmpresa: $('#hfEmpresa').val(), idsCuenca: $('#hfCuenca').val(), idsFamilia: $('#hfFamilia').val(), idLectura: tipoInformacion,
            idsPtoMedicion: $('#hfPtoMedicion').val(), fechaInicial: $('#FechaDesde').val(), fechaFinal: $('#FechaHasta').val(),
            annho: $('#hfAnho').val(), semanaIni: $('#hfSemanaIni').val(), semanaFin: $('#hfSemanaFin').val(),
            anhoInicial: $('#hfAnhoInicio').val(), anhoFinal: $('#hfAnhoFin').val(),
            rbDetalleRpte: $('#hfidTipo').val(), unidad: $('#hfUnidad').val()
        },
        dataType: 'json',
        success: function (result) {

            if (result == 1) {//Horas
                window.location = controlador + "reporte/ExportarReporte?tipoReporte=10";
            }
            if (result == 2) {//PROGRAMADO SEMANAL
                window.location = controlador + "reporte/ExportarReporte?tipoReporte=4";
            }
            if (result == 3) {//Mensual, Semanal-diario
                window.location = controlador + "reporte/ExportarReporte?tipoReporte=0";
            }
            if (result == 4) {//Semanal - Dia.
                window.location = controlador + "reporte/ExportarReporte?tipoReporte=6";
            }
            if (result == -1) {
                alert("Error en reporte result")
            }
        },
        error: function () {
            mostrarError();
        }
    });
}

function mostrarError() {
    alert("Ha ocurrido un error em reprte2 ");
}

function exportarGrafico() {
    var tipoinf = buscarTipoInformacion(tipoInformacion);
    switch (tipoinf) {
        case '1': //Diario CP -> tabla me_medicion1
            if (tipoInformacion == 67)
                exportarGrafDiarioM1();
            else// Ejcecutado Historico/TR -> tabla me_medicion24
                exportarGrafDiarioM24();
            break;
        case '2': //Prog Semanal
        case '5': // Ejecutado / Programa Semanal
            exportarGrafDiarioM1();
            break;
        case '3': //Prog/Ejecutado Mensual
            exportarGrafDiarioM1();
            break;
    }
}

// Genera Reporte excel de grafico Programado Diario CP 
function exportarGrafDiarioM1() {
    $.ajax({
        type: 'POST',
        url: controlador + "reporte/GenerarArchivoGrafM1",
        data: {
            fechaInicial: $('#hfFechaDesde').val(), fechaFinal: $('#hfFechaHasta').val()
        },
        dataType: 'json',
        success: function (result) {
            if (result == 1) {
                window.location = controlador + "reporte/ExportarReporte?tipoReporte=8";
            }
            if (result == -1) {
                mostrarError();
            }
        },
        error: function () {
            alert("Error Grafico export a Excel");
        }
    });
}

// Genera Reporte excel de grafico Ejecutado TR/Ejecutado Historico
function exportarGrafDiarioM24() {
    $.ajax({
        type: 'POST',
        url: controlador + "reporte/GenerarArchivoGrafM24",
        data: {
            fechaInicial: $('#hfFechaDesde').val(), fechaFinal: $('#hfFechaHasta').val()
        },
        dataType: 'json',
        success: function (result) {
            if (result == 1) {
                window.location = controlador + "reporte/ExportarReporte?tipoReporte=3";
            }
            if (result == -1) {
                mostrarError();
            }
        },
        error: function () {
            alert("Error en Ajax Grafico export a Excel");
        }
    });
}

function buscarTipoInformacion(valor) {

    for (var i = 0 ; i < listLectCodi.length; i++)
        if (listLectCodi[i] == valor) return listLectPeriodo[i];
}

function handleClick(myRadio) {
    currentValue = myRadio.value;
    cambiarFormatoFecha2(currentValue);
}

function listarPuntoMedicion() {
    var unidad = $('#cbUnidades').val();
    $('#hfUnidad').val(unidad);
    $.ajax({
        type: 'POST',
        url: controlador + "reporte/ListarPuntosMedicion",
        data: {
            iUnidad: $('#hfUnidad').val()
        },
        success: function (evt) {
            $('#listPuntoMedicion').html(evt);
        },
        error: function () {
            alert("Ha ocurrido un error");
        }
    });

}

