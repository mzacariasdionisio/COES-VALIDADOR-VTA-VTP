//var siteRoot = "http://localhost:60037/";
var sControlador = siteRoot + 'IEOD/pronostico/';

$(function () {
    //action-exito //action-alert //action-message
    buscar = function () {
        var IdEnvio = document.getElementById('PrnIdEnvio').value;
        var NumPtosDepurar = document.getElementById('PrnNumPtosDepurar').value;
        console.log("IdEnvio ->" + IdEnvio);
        console.log("NumPtosDepurar ->" + NumPtosDepurar);
        $.ajax({
            type: 'POST',
            url: sControlador + "lista",
            data: { IdEnvio: IdEnvio, NumPtosDepurar: NumPtosDepurar },
            success: function (evt) {
                $('#listaPuntos').html(evt);
                viewEvent();
                oTable = $('#tabla').dataTable({
                    "sPaginationType": "full_numbers",
                    "pageLength": 30,
                    "destroy": "true",
                    "aaSorting": [[3, "desc"], [0, "asc"], [1, "asc"], [2, "asc"]]
                });
                //mostrarMensaje('mensajeGrafico', 'message', 'Se han encontrado desviaciones en las mediciones durante la comparación de la información ingresada vs la información histórica.');
            },
            error: function () {
                mostrarMensaje('mensajeGrafico', 'error', 'Lo sentimos, ha ocurrido un error inesperado');
            }
        });
    };

    $('#btnModificar').on('click', function () {
        modificar();
    });
    $('#btnCerrar').on('click', function () {
        cerrar();
    });
    $('#contentGrafico').css("display", "none");
    $('#btnModificar').css("display", "none");
    $('#tableMotivo').css("display", "none");
    $('#btnCerrar').css("display", "none");

    buscar();
});

////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
//Funciones de vista detalle
viewEvent = function () {
    $('.viewdetalle').click(function () {
        Ptomedicodi = $(this).attr("id").split("_")[1];
        Lectcodi = $(this).attr("id").split("_")[2];
        Medifecha = $(this).attr("id").split("_")[3];
        console.log("Ptomedicodi ->" + Ptomedicodi);
        console.log("Lectcodi ->" + Lectcodi);
        console.log("Medifecha ->" + Medifecha);
        document.getElementById('Ptomedicodi').value = Ptomedicodi;
        document.getElementById('Lectcodi').value = Lectcodi;
        document.getElementById('Medifecha').value = Medifecha;
        pintar(Ptomedicodi, Lectcodi, Medifecha);
        ocultarListaMediciones();
        $('#contentGrafico').css("display", "block");
        $('#btnModificar').css("display", "block");
        $('#tableMotivo').css("display", "block");
        $('#btnCerrar').css("display", "block");
        mostrarMensaje('mensajeGrafico', 'alert', 'Por favor, corrija las desviaciones encontradas');
    });
};

pintar = function (Ptomedicodi, Lectcodi, Medifecha) {
    var IdEnvio = document.getElementById('PrnIdEnvio').value;
    $.ajax({
        type: 'POST',
        url: sControlador + 'comparativo',
        data: {
            IdEnvio: IdEnvio,
            Ptomedicodi: Ptomedicodi,
            Lectcodi: Lectcodi,
            Medifecha: Medifecha
        },
        dataType: 'json',
        success: function (result) {
            pintarTabla(result);
            pintarGrafico(result);
        },
        error: function () {
            mostrarMensaje('mensajeGrafico', 'error', 'Ha ocurrido un error.');
        }
    });
}

pintarTabla = function (result) {
    //console.log(result);
    var ListaJustificacion = result[0].ListaJustificacion;
    var ListaMeJustificacion = result[0].ListaMeJustificacion;
    var nroFilas = 0;
    var nroBloques = 0;
    //console.log(ListaJustificacion);
    console.log(ListaMeJustificacion);
    var html = '<form name="frmBrowse">';
    html = html + ' <table class="pretty tabla-adicional">';
    html = html + ' <thead>';
    html = html + '     <tr><th colspan="5">' + result[0].PtomediDesc + ' [' + result[0].Medifecha + '] - Desviación: ' + result[0].PorcDesviacion + '%</th></tr>';
    html = html + '     <tr>';
    html = html + '         <th>Hora</th>';
    html = html + '         <th>Histórico</th>';
    html = html + '         <th>Reportado</th>';
    html = html + '         <th>Desviación</th>';
    html = html + '         <th>Final</th>';
    html = html + '         <th>Justificación</th>';
    html = html + '     </tr>';
    html = html + ' </thead>';
    html = html + ' <tbody>';
    var i = 0;
    var sumPatron = 0;
    var sumPronostico = 0;
    var sunMedicion = 0;
    for (var i in result) {
        
        var style = '';
        if (i % 2 == 0) style = 'background-color: #f2f5f7';

        if (result[i].Desviacion * 100 > result[0].PorcDesviacion || result[i].Desviacion * 100 < (-1 * result[0].PorcDesviacion)) {
            style = 'background-color: #ffb4b4';
        }

        html = html + '     <tr>';
        html = html + '         <td style="text-align:center;' + style + '">' + result[i].Hora + '</td>';
        html = html + '         <td style="text-align:right;' + style + '">' + $.number(result[i].ValorPatron, 4, '.', '') + '</td>';
        html = html + '         <td style="text-align:right;' + style + '">' + $.number(result[i].Pronostico, 4, '.', '') + '</td>';
        html = html + '         <td style="text-align:right;' + style + '">' + $.number(result[i].Desviacion * 100, 2, '.', '') + '% </td>';
        html = html + '         <td style="text-align:right;' + style + '"><input name="hItem" type="text" value="' + $.number(result[i].ValorMedicion, 4, '.', '') + '" style="width: 50px; height: 12px; font-size: 11px;"></td>';
        if (style == 'background-color: #ffb4b4') {
            if (nroFilas == 0)
            {
                nroFilas = ListaMeJustificacion[nroBloques].nroFilas;
                //console.log(nroFilas);
                var Justcodi = ListaMeJustificacion[nroBloques].Justcodi;
                //console.log(Justcodi);
                html = html + '<td rowspan="' + nroFilas + '" style="text-align:right;' + style + '"><select id="hSelect" name="hSelect"  style="width: 150px; height: 25px; font-size: 12px;">';
                for (var j in ListaJustificacion) {
                    var selected = "";
                    //console.log(ListaJustificacion[j].Subcausacodi);
                    if (ListaJustificacion[j].Subcausacodi == ListaMeJustificacion[nroBloques].Subcausacodi)
                        selected = "selected";
                    html = html + '<option value="' + Justcodi + "_" + ListaJustificacion[j].Subcausacodi + '" ' + selected + '>' + ListaJustificacion[j].Subcausadesc + '</option>';
                }
                html = html + '</select></td>';
                nroBloques = nroBloques + 1;
            }
        }
        else {
            nroFilas = 0;
            html = html + '    <td style="text-align:center;' + style + '"> </td>';
        }
        html = html + '     </tr>';

        sumPatron = sumPatron + result[i].ValorPatron;
        sumPronostico = sumPronostico + result[i].Pronostico;
        sunMedicion = sunMedicion + result[i].ValorMedicion;
        i++;
    }

    var desviacion = (sumPatron != 0) ? (sumPronostico - sumPatron) / sumPatron : 0;
    html = html + ' </tbody>';
    html = html + ' <thead>';
    html = html + '     <tr>';
    html = html + '         <th>TOTAL</th>';
    html = html + '         <th style="text-align:right;">' + $.number(sumPatron, 4, '.', '') + '</th>';
    html = html + '         <th style="text-align:right;">' + $.number(sumPronostico, 4, '.', '') + '</th>';
    html = html + '         <th style="text-align:right;">' + $.number(desviacion * 100, 2, '.', '') + '% </th>';
    html = html + '         <th style="text-align:right;">' + $.number(sunMedicion, 4, '.', '') + '</th>';
    html = html + '         <th> </th>';
    html = html + '     </tr>';
    html = html + ' </thead>';
    html = html + '</table>';
    html = html + '</form>';
    html = html + '<input type="hidden" name="nroBloques" id="nroBloques" value="' + nroBloques + '" />';
    $('#contentTabla').html(html);
}

pintarGrafico = function (result) {
    $('#contentGrafico').empty();
    var categorias = [];
    var series = [];
    var dataPatron = [];
    var dataMedicion = [];
    var dataPrevisto = [];

    for (var i in result) {
        categorias.push(result[i].Hora);
        dataPatron.push(result[i].ValorPatron);
        dataPrevisto.push(result[i].ValorPrevisto);
        dataMedicion.push(result[i].ValorMedicion);
    }

    series.push({ name: 'Histórico', data: dataPatron, color: '#7CB5EC' });
    if (result[0].Lectura != "")
    {
        series.push({ name: result[0].Lectura, data: dataPrevisto, color: '#90ED7D' });
    }
    series.push({ name: 'Info. Reportada', data: dataMedicion, color: '#333333' });//, dashStyle: 'dot' 

    $('#contentGrafico').highcharts({
        chart:{zoomType: 'xy'},
        title: {
            text: 'Histórico vs Reportado: ' + result[0].PtomediDesc,
            x: -20
        },
        xAxis: {
            categories: categorias,
            labels: {
                rotation: -90
            }
        },
        yAxis: {
            title: {
                text: 'Potencia (MW)'
            },
            plotLines: [{
                value: 0,
                width: 1,
                color: '#808080'
            }]
        },
        tooltip: {
            valueSuffix: 'MW'
        },
        legend: {
            borderWidth: 0
        },
        series: series
    });
}

mostrarMensaje = function (id, tipo, mensaje) {
    $('#' + id).removeClass();
    $('#' + id).addClass('action-' + tipo);
    $('#' + id).html(mensaje);
}

////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
//Modificar el despacho ejecutado en una fecha
modificar = function () {
    var Ptomedicodi = document.getElementById('Ptomedicodi').value;
    if (Ptomedicodi == "")
    {
        alert('Lo sentimos, aun no ha seleccionado ningún punto de medición.');
    }
    else {
        var Lectcodi = document.getElementById('Lectcodi').value;
        var Medifecha = document.getElementById('Medifecha').value;
        var IdEnvio = document.getElementById('PrnIdEnvio').value;
        var IdEmpresa = document.getElementById('PrnIdEmpresa').value;
        var iNumH = 48;
        if (Lectcodi == 51) iNumH = 96;
        var slistaH = "";
        for (i = 0; i < iNumH; i++) {
            if (slistaH != "") slistaH = slistaH + ",";
            slistaH = slistaH + eval('document.frmBrowse.hItem[' + i + '].value');
        }
        var nroBloques = document.getElementById('nroBloques').value;
        console.log('nroBloques ->' + nroBloques);
        var slistaMeJust = "";
        if (nroBloques == 1) {
            slistaMeJust = eval('document.frmBrowse.hSelect.value');
        }
        else {
            for (i = 0; i < nroBloques; i++) {
                if (slistaMeJust != "") slistaMeJust = slistaMeJust + ",";
                slistaMeJust = slistaMeJust + eval('document.frmBrowse.hSelect[' + i + '].value');
                console.log(eval('document.frmBrowse.hSelect[' + i + '].value'));
            }
        }
        console.log("modificar -------------------------------------------->");
        console.log("Ptomedicodi ->" + Ptomedicodi);
        console.log("Lectcodi ->" + Lectcodi);
        console.log("Medifecha ->" + Medifecha);
        console.log("IdEnvio ->" + IdEnvio);
        console.log("IdEmpresa ->" + IdEmpresa);
        console.log("iNumH ->" + iNumH);
        console.log("slistaMeJust ->" + slistaMeJust);
        console.log("slistaH ->" + slistaH);
        $.ajax({
            type: 'POST',
            url: sControlador + 'GrabarMedicion',
            data: {
                IdEmpresa: IdEmpresa,
                IdEnvio: IdEnvio,
                Ptomedicodi: Ptomedicodi,
                Lectcodi: Lectcodi,
                Medifecha: Medifecha,
                listaMeJust: slistaMeJust,
                listaH: slistaH
            },
            dataType: 'json',
            success: function (result) {
                if (result == 1) {
                    buscar();
                    pintar(Ptomedicodi, Lectcodi, Medifecha);
                    mostrarMensaje('mensajeGrafico', 'exito', 'La información se actualizo correctamente');
                }
                else
                    mostrarMensaje('mensajeGrafico', 'error', 'Lo sentimos, se ha encontrado un error');
            },
            error: function () {
                alert('Ha ocurrido un error.');
            }
        });
    }
}

mostrarListaMediciones = function () {
    var divListado = document.getElementById('divListado');
    divListado.style.display = '';
}

ocultarListaMediciones = function () {
    var divListado = document.getElementById('divListado');
    divListado.style.display = 'none';
}

cerrar = function () {
    $('#pronostico').bPopup().close();
    $('#pronostico').empty();
}