
$(document).ready(function () {
    $('#txtFecha').Zebra_DatePicker({
        direction: -1,
        onSelect: function () {
            buscar()
        }
    });

    $('#cboArea').on('change', function () {
        buscar();
    });

    $('#btnEjecutar').on('click', function () {
        ejecutar();
    });

    $('#btnModificarDE').on('click', function () {
        modificarDE();
    });

    $('#btnPegarRSF').on('click', function () {
        pegarRSF();
    });

    $('#btnExportar').on('click', function () {
        exportar(1);
    });

    buscar();

});

function buscar() {
    mostrarMensaje('mensaje', 'info', 'Espere un momento, se esta procesando su solicitud.');
    var sFecha = document.getElementById('txtFecha').value;
    var Areacodi = document.getElementById('cboArea').value;
    $.ajax({
        type: 'POST',
        url: controller + "Lista",
        data: { Areacodi: Areacodi, sFecha: sFecha },
        success: function (evt) {
            $('#listaCentrales').html(evt);
            viewEvent();
            oTable = $('#tabla').dataTable({
                "sPaginationType": "full_numbers",
                "pageLength": 20,
                "destroy": "true",
                "aaSorting": [[2, "desc"], [3, "desc"]]
            });
            $('#contentGrafico').css("display", "none");
            $('#btnPegarRSF').css("display", "none");
            $('#btnModificarDE').css("display", "none");
            $('#contentTabla').html("");
            $('#contentGrafico').html("");
        },
        error: function () {
            mostrarMensaje('mensaje', 'error', "Lo sentimos, ha ocurrido un error inesperado");
        }
    });
}

////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
//Calcula el despacho ejecutado en una fecha
function ejecutar() {
    mostrarMensaje('mensaje', 'info', 'Espere un momento, se esta procesando su solicitud.');
    var sFecha = document.getElementById('txtFecha').value;
    $.ajax({
        type: 'POST',
        url: controller + 'ObtenerDespachoEjecutado',
        data: { fecha: sFecha },
        dataType: 'json',
        success: function (result) {
            if (result == 1)
                buscar();
            else
                mostrarMensaje('mensaje', 'error', "Lo sentimos, ha ocurrido un error inesperado");
        },
        error: function () {
            mostrarMensaje('mensaje', 'error', "Lo sentimos, ha ocurrido un error inesperado");
        }
    });
}

//Exportar el despacho ejecutado
function exportar(formato) {
    mostrarMensaje('mensaje', 'info', 'Espere un momento, se esta procesando su solicitud.');
    var sFecha = document.getElementById('txtFecha').value;
    var Areacodi = document.getElementById('cboArea').value;
    $.ajax({
        type: 'POST',
        url: controller + 'exportardata',
        data: { Areacodi: Areacodi, sFecha: sFecha },
        dataType: 'json',
        success: function (result) {
            if (result != -1) {
                window.location = controller + 'abrirarchivo?formato=' + formato + '&file=' + result;
                mostrarMensaje('mensaje', 'exito', "Felicidades, el archivo se descargo correctamente...!");
            }
            else {
                mostrarMensaje('mensaje', 'error', "Lo sentimos, ha ocurrido un error inesperado");
            }
        },
        error: function () {
            mostrarMensaje('mensaje', 'error', "Lo sentimos, ha ocurrido un error inesperado");
        }
    });
}

function pintar(equicodi, fecha) {
    $.ajax({
        type: 'POST',
        url: controller + 'comparativo',
        data: {
            idEmpresa: -1,
            idCentral: equicodi,
            fecha: fecha
        },
        dataType: 'json',
        success: function (result) {
            pintarTabla(result.ListComparativo);
            pintarGrafico(result.ListComparativo);
            SetMessage('#message', result.Mensaje, result.TipoMensaje, true);
            $('#contentGrafico').css("display", "block");
            $('#btnPegarRSF').css("display", "block");
            $('#btnModificarDE').css("display", "block");
        },
        error: function () {
            mostrarMensaje('mensaje', 'error', 'Ha ocurrido un error.');
        }
    });
}

function pintarTabla(result) {
    var html = '<form name="frmBrowse">';
    html = html + ' <table class="pretty tabla-adicional">';
    html = html + ' <thead>';
    html = html + '     <tr>';
    html = html + '         <th><input name="chkAll" type="checkbox" onclick="SelectAll()" checked="" title="Selecciona a todos los RSF que se copia a Pronóstico">Todo</th>';
    html = html + '         <th>HORA</th>';
    html = html + '         <th>RPF</th>';
    html = html + '         <th>DE</th>';
    html = html + '         <th>DESV.%</th>';
    html = html + '         <th>PRONÓST</th>';
    html = html + '     </tr>';
    html = html + ' </thead>';
    html = html + ' <tbody>';
    var i = 0;
    var indice = 1;
    var sumRPF = 0;
    var sumDespacho = 0;
    var sumPronostico = 0;
    for (var i in result) {
        var style = '';
        if (i % 2 == 0) style = 'background-color: #f2f5f7';
        if (result[i].Desviacion * 100 > 5 || result[i].Desviacion * 100 < -5) {
            style = 'background-color: #ffb4b4';
        }
        html = html + '     <tr>';
        html = html + '         <td style="text-align:center;' + style + '"><input name="chkItem" type="checkbox" value="H' + indice + '"></td>';
        html = html + '         <td style="text-align:center;' + style + '">' + result[i].Hora + '</td>';
        html = html + '         <td style="text-align:right;' + style + '">' + $.number(result[i].ValorRPF, 2, '.', '') + '</td>';
        html = html + '         <td style="text-align:right;' + style + '">' + $.number(result[i].ValorDespacho, 2, '.', '') + '</td>';
        html = html + '         <td style="text-align:right;' + style + '">' + $.number(result[i].Desviacion * 100, 2, '.', '') + '% </td>';
        html = html + '         <td style="text-align:right;' + style + '"><input name="hItem" type="text" value="' + $.number(result[i].Pronostico, 2, '.', '') + '" style="width: 50px; height: 10px;"></td>';
        html = html + '     </tr>';

        sumRPF = sumRPF + result[i].ValorRPF;
        sumDespacho = sumDespacho + result[i].ValorDespacho;
        sumPronostico = sumPronostico + result[i].Pronostico;
        indice++;
        i++;
    }

    var desviacion = (sumRPF != 0) ? (sumDespacho - sumRPF) / sumRPF : 0;
    html = html + ' </tbody>';
    html = html + ' <thead>';
    html = html + '     <tr>';
    html = html + '         <th colspan="2">TOTAL</th>';
    html = html + '         <th style="text-align:right;">' + $.number(sumRPF, 2, '.', '') + '</th>';
    html = html + '         <th style="text-align:right;">' + $.number(sumDespacho, 2, '.', '') + '</th>';
    html = html + '         <th style="text-align:right;">' + $.number(desviacion * 100, 2, '.', '') + '% </th>';
    html = html + '         <th style="text-align:right;">' + $.number(sumPronostico, 2, '.', '') + '</th>';
    html = html + '     </tr>';
    html = html + ' </thead>';
    html = html + '</table>';
    html = html + ' <input type="hidden" id="Count" name="Count" value="' + i + '">';
    html = html + '</form>';
    $('#contentTabla').html(html);
}

function pintarGrafico(result) {
    var categorias = [];
    var series = [];
    var dataRPF = [];
    var dataDespacho = [];
    var dataPronostico = [];

    for (var i in result) {
        categorias.push(result[i].Hora);
        dataRPF.push(result[i].ValorRPF);
        dataDespacho.push(result[i].ValorDespacho);
        dataPronostico.push(result[i].Pronostico);
    }

    series.push({ name: 'RPF', data: dataRPF, color: '#7CB5EC' });
    series.push({ name: 'Despacho Ejecutado', data: dataDespacho, color: '#90ED7D' });
    series.push({ name: 'InfoPronostico', data: dataPronostico, color: '#F5A9A9' });

    $('#contentGrafico').highcharts({
        title: {
            text: 'Comparativo RPF VS Despacho Ejecutado: ' + result[0].Central,
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

function mostrarMensaje(id, tipo, mensaje) {
    $('#' + id).removeClass();
    $('#' + id).addClass('action-' + tipo);
    $('#' + id).html(mensaje);
}

function viewEvent() {
    var sFecha = document.getElementById('txtFecha').value;
    $('.view').click(function () {
        mostrarMensaje('mensaje', 'info', 'Espere un momento, se esta procesando su solicitud.');
        equicodi = $(this).attr("id").split("_")[1];
        document.getElementById('equicodi').value = equicodi;
        pintar(equicodi, sFecha);
    });
};

function SetMessage(container, msg, tpo, del) {//{Contenedor, mensaje(string), tipoMensaje(string), delay(bool)}
    var new_class = "msg-" + tpo;//Identifica la nueva clase css
    $(container).removeClass($(container).attr('class'));//Quita la clase css previa
    $(container).addClass(new_class);
    $(container).html(msg);//Carga el mensaje
    //Mensaje con delay o no
    if (del) $(container).show(0).delay(5000).hide(0);//5 Segundos
    else $(container).show();
}

function SelectAll() {
    var cont = frmBrowse.Count.value;
    var xSel;
    var i;
    xSel = frmBrowse.chkAll.checked;
    if (cont > 1) {
        for (i = 0; i < cont; i++) {
            eval('frmBrowse.chkItem[' + i + '].checked=' + xSel + ';');
        }
    }
    else {
        eval('frmBrowse.chkItem.checked=' + xSel + ';');
    }
}

function checkMark() {
    var cont = frmBrowse.Count.value;
    var xSel = "0";
    var i;
    if (cont > 1) {
        for (i = 0; i < cont; i++) {
            if (eval('frmBrowse.chkItem[' + i + '].checked')) {
                //console.log("A:" + eval('frmBrowse.chkItem[' + i + '].value'));
                xSel = xSel + "," + eval('frmBrowse.chkItem[' + i + '].value');
            }
        }
    }
    else if (cont == 1) {
        if (eval('frmBrowse.chkItem.checked')) {
            //console.log("B:" + eval('frmBrowse.chkItem[' + i + '].value'));
            xSel = eval('frmBrowse.chkItem.value');
        }
    }
    return xSel;
}

////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
//Modificar el despacho ejecutado en una fecha
function modificarDE() {
    mostrarMensaje('mensaje', 'info', 'Espere un momento, se esta procesando su solicitud.');
    var equicodi = document.getElementById('equicodi').value;
    if (equicodi == "")
        alert('Lo sentimos, aun no ha seleccionado ninguna central.');
    else {
        var sFecha = document.getElementById('txtFecha').value;
        var slistaH = "";
        for (i = 0; i < 48; i++) {
            if (slistaH != "") slistaH = slistaH + ",";
            slistaH = slistaH + eval('document.frmBrowse.hItem[' + i + '].value');
        }
        $.ajax({
            type: 'POST',
            url: controller + 'GrabarPronostico',
            data: { equicodi: equicodi, fecha: sFecha, listaH: slistaH },
            dataType: 'json',
            success: function (result) {
                if (result == 1) {
                    buscar();
                    pintar(equicodi, sFecha);
                }
                else
                    mostrarMensaje('mensaje', 'error', "Lo sentimos, ha ocurrido un error inesperado");
            },
            error: function () {
                mostrarMensaje('mensaje', 'error', "Lo sentimos, ha ocurrido un error inesperado");
            }
        });
    }
}

//Copiar algunos de los valores del RSF al Despacho Ejecutado para el pronostico
function pegarRSF() {
    mostrarMensaje('mensaje', 'info', 'Espere un momento, se esta procesando su solicitud.');
    var iNumReg = document.getElementById('Count').value;
    if (iNumReg > 0) {
        var slistaH = checkMark();
        console.log("slistaH:" + slistaH);
        var equicodi = document.getElementById('equicodi').value;
        var sFecha = document.getElementById('txtFecha').value;
        $.ajax({
            type: 'POST',
            url: controller + 'GrabarRSF',
            data: { equicodi: equicodi, fecha: sFecha, listaH: slistaH },
            dataType: 'json',
            success: function (result) {
                if (result == 1) {
                    buscar();
                    pintar(equicodi, sFecha);
                }
                else if (result == 0)
                    mostrarMensaje('mensaje', 'error', 'Debe seleccionar al menos un registro RPF.');
                else
                    mostrarMensaje('mensaje', 'error', "Lo sentimos, ha ocurrido un error inesperado");
            },
            error: function () {
                mostrarMensaje('mensaje', 'error', "Lo sentimos, ha ocurrido un error inesperado");
            }
        });

    }
}
////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
