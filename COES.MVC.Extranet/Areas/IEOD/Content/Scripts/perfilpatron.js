//ASSETEC 201909
var controlador = siteRoot + "IEOD/" + "PerfilPatron/";

//temporal
var test;
//variables comunes
var tipoPatron, datoObservacion, numIntervalos, puntoMedicion, nDias;
var flgActivo = false, dataRow;
//variables inputs bloques
var arrayBloques;
//variables highchart
var hcModelo, hcGrid, hcSerieIndex, hcAjuste;
//variables handsontable
var htModelo, htGrid, htBaseIndex, htAjusteIndex, htDestinoIndex, htRowIndex, htRowData, htSuma;
//variables datatable
var dtGrid, dtWidth;


$(document).ready(function () {

    dtWidth = $('#togglepanel').width();

    $("#togglebutton").click(function () {
        if ($("#togglepanel").is(":hidden")) {
            $("#togglepanel").slideDown();
            dtGrid.draw();
        } else {
            $("#togglepanel").slideUp();
            dtGrid.draw();
        }
    });

    $('#dpFecha').Zebra_DatePicker({
        onSelect: function (value) {
            Iniciar();//Carga el panel de puntos de medición
            if (flgActivo == true) {
                LimpiarBloques();
                $('#htgrid').html('');
                Medicion(dataRow.IdPunto, numIntervalos, value, dataRow.NomPunto);
            }
        }
    });

    $('.clsDm').Zebra_DatePicker({
        onSelect: function (a, b, c, d) {
            var i = d.context.id;//Id del Elemento Zdp mañana
            var ai = i.split('d');
            var idx = parseInt(ai[1]);//Indice para la serie del highchart
            var j = document.getElementById('dt' + idx);//Elemento Zdp tarde
            var t = document.getElementById(j.id).closest('div');//Elemento Td tarde
            var dia = GetDia(a);
            if (dia == 0) {
                t.style.visibility = 'visible';
                j.value = a;//Setea el valor de la tarde
            }
            else {
                t.style.visibility = 'collapse';
                j.value = a;//Setea el valor de la tarde
            }

            EditarHistorico(a, j.value, false, idx);
        }
    });
    $('.clsDt').Zebra_DatePicker({
        onSelect: function (a, b, c, d) {
            var i = d.context.id;//Id del Elemento Zdp tarde
            var ai = i.split('dt');
            var idx = parseInt(ai[1]);
            var j = document.getElementById('d' + idx);//Elemento Zdp mañana

            EditarHistorico(j.value, a, true, idx);
        }
    });

    $('.clsFiltro').on('change', function () {
        var id = this.id;
        if (id == 'cboUbicacion') {
            $('#cboEmpresa').val(0);
            ActualizarListas();
            setTimeout(Iniciar(), 100);
        }
        else {
            Iniciar();
        }
    });

    $('.clsFiltro').multipleSelect({
        filter: true,
        single: true
    });

    $('.clsBloque').on('change', function () {
        var id = this.id;
        var valor = parseFloat(this.value);
        if (!(isNaN(valor))) {
            EditarPorBloque(id, valor);
        }
    });

    $('#btnObservacion').on('click', function () {
        $('#obsgrid').html(datoObservacion);
        $('#popObservacion').bPopup({
            easing: 'easeOutBack',
            speed: 450,
            transition: 'slideDown',
            onClose: function () {
                $('#obsgrid').empty();
            }
        });
    });

    $('#cboFuente').change(function () {
        $('#cboEmpresa').val('0');
        $('#cboPtomedicion').val('0');
        ActualizarListas(1);

    });

    $('#cboEmpresa').change(function () {
        $('#cboPtomedicion').val('0');
        ActualizarListas(2);
    });

    $('#btnOcultarMenu').on('click', function () {
        ResizeCharts();
    });

    $('#btnConsultar').on('click', function () {
        Medicion();
    });
    //Inicia el modulo
    Iniciar();
});

function Iniciar() {
    $("#togglepanel").slideDown();
    //$('#modulo').hide();

    var selFuente = $('#cboFuente').val();
    var selEmpresa = $('#cboEmpresa').val();
    var selPunto = $('#cboPtomedicion').val();
    var selFecha = $('#dpFecha').val();

}


function Medicion() {
    if (typeof htGrid != 'undefined') {
        htGrid.destroy();
    }
    $.ajax({
        type: 'POST',
        url: controlador + "Medicion",
        data: {
            fecha: $('#dpFecha').val(),
            idPunto: $('#cboPtomedicion').val()
            //numIntervalos: selFuente
        },
        success: function (result) {
            console.log(result.Patron);
            //Flag modulo activo
            flgActivo = true;
            //Modelos
            htModelo = GetHandson(result.Datos.Handson);
            hcModelo = GetHighchart(result.Datos.Highchart);
            //Inicia los bloques
            (arrayBloques = []).length = htModelo.tabla.length;
            arrayBloques.fill(0);
            //Handsontable
            var container = document.getElementById('htgrid');
            htGrid = new Handsontable(container, {
                data: htModelo.tabla,
                fillHandle: true,
                stretchH: 'all',
                maxCols: htModelo.configuracion.length,
                maxRows: htModelo.tabla.length,
                minSpareCols: 0,
                minSpareRows: 0,
                columns: htModelo.configuracion,

            });
            //Highchart
           Grafico(hcModelo);
        },
        error: function () {
            alert("Ha ocurrido un problema...");
        }
    });
}

function Grafico(modelo) {
    hcGrid = Highcharts.chart('hcgrid', {
        chart: {
            type: 'spline',
            backgroundColor: 'transparent',
            plotShadow: false,
            zoomType: 'xy',
        },
        title: {
            text: ""
        },
        credits: {
            enabled: false
        },
        line: {
            cursor: 'ns-resize'
        },
        legend: {
            itemDistance: 15,
            verticalAlign: 'top',
            symbolWidth: 10,
            itemStyle: {
                fontWeight: 'normal'
            },
            y: 0
        },
        tooltip: { enabled: true },
        plotOptions: {
            series: {
                stickyTracking: false,
                marker: { enabled: true, radius: 3 },
                point: {
                    events: {
                        drop: function () {//Actualiza la columna Ajuste del Handsontable                            
                            var a = htGrid.getDataAtRowProp(this.x, 'ajuste');
                            hcAjuste = Highcharts.numberFormat(this.y - this.dragStart.y + a, 2);
                            htGrid.setDataAtRowProp(this.x, 'ajuste', hcAjuste, 'grafico');
                        }
                    }
                }
            }
        },
        xAxis: {
            tickInterval: 1,
            categories: modelo.categoria,
            labels: { rotation: 90 }
        },
        yAxis: {
            title: ''
        },
        series: modelo.series
    });
}

function GetFechas(datos) {
    for (var i = 0; i < datos.length; i++) {
        //Calendario - Mañana
        document.getElementById('d' + (i + 1).toString()).value = datos[i];
        //Calendario - Tarde
        document.getElementById('dt' + (i + 1).toString()).value = datos[i];
    }
}

function GetHandson(datos) {
    var modelo;
    var tabla = [];
    for (var i = 0; i < datos[0].data.length; i++) {
        var fila = {};
        for (var j = 0; j < datos.length; j++) {
            fila[datos[j].id] = datos[j].data[i];
        }
        tabla.push(fila);
    }

    var config = [];
    for (var i = 0; i < datos.length; i++) {
        var columna;
        switch (datos[i].render) {
            case 'hora':
                columna = { data: datos[i].id, title: datos[i].nombre, type: 'text', className: "htCenter", readOnly: true, renderer: HoraColumnRenderer };
                break;
            case 'normal':
                columna = { data: datos[i].id, title: datos[i].nombre, type: 'numeric', format: '0.00', readOnly: true };
                break;
            case 'edit':
                columna = { data: datos[i].id, title: datos[i].nombre, type: 'numeric', format: '0.00', allowInvalid: false, allowEmpty: false };
                break;
            case 'final':
                columna = { data: datos[i].id, title: datos[i].nombre, type: 'numeric', format: '0.00', readOnly: true, renderer: FinalColumnRenderer };
                break;
        }
        config.push(columna);
    }

    modelo = { tabla: tabla, configuracion: config };
    return modelo;
}

function GetHighchart(datos) {
    var modelo;
    var config = [];
    var categ = [];

    for (var i = 0; i < datos.length; i++) {
        var serie;
        switch (datos[i].render) {
            case 'categoria':
                categ = datos[i].data;
                break;
            case 'normal':
                serie = { id: datos[i].id, name: datos[i].nombre, data: datos[i].data };
                break;
            case 'patron':
                serie = { id: datos[i].id, name: datos[i].nombre, data: datos[i].data, lineWidth: 6, color: '#000000' };
                break;
            case 'plus':
                serie = { id: datos[i].id, name: datos[i].nombre, data: datos[i].data, lineWidth: 3 };
                break;
            case 'LongDash':
                serie = { id: datos[i].id, name: datos[i].nombre, data: datos[i].data, dashStyle: 'LongDash' };
                break;
            case 'final':
                serie = { id: datos[i].id, name: datos[i].nombre, data: datos[i].data, lineWidth: 3, color: '#59DA00', draggableY: true };
                break;
        }

        if (datos[i].render != 'categoria') {
            config.push(serie);
        }
    }

    modelo = { categoria: categ, series: config };
    return modelo;
}

function HoraColumnRenderer(instance, td, row, col, prop, value, cellProperties) {
    Handsontable.renderers.TextRenderer.apply(this, arguments);
    td.style.fontWeight = 'bold';
    td.style.background = '#F2F2F2';
}

function FinalColumnRenderer(instance, td, row, col, prop, value, cellProperties) {
    Handsontable.renderers.NumericRenderer.apply(this, arguments);
    td.style.background = '#E6F8E0';
}

function ValidarEventosHandsontable(evento) {
    var valid = false;
    var ListEventos = ['edit', 'autofill', 'paste', 'grafico'];

    for (var i = 0; i < ListEventos.length; i++) {
        if (evento == ListEventos[i]) {
            valid = true;
        }
    }

    return valid;
}

function ActualizarListas(opcion) {
    $.ajax({
        type: 'POST',
        url: controlador + "ActualizarListas",
        data: {
            idFuente: $('#cboFuente').val(),
            idEmpresa: $('#cboEmpresa').val()
        },
        success: function (result) {
            console.log(result);
            if (opcion == 1) {
                $('#cboEmpresa').empty();
                var selector = document.getElementById('cboEmpresa');
                selector.options[selector.options.length] = new Option('-Seleccione-', 0);
                selector.options[0].setAttribute('selected', 'selected');

                if (result.ListaEmpresa.length != 0) {

                    for (var i = 0; i < result.ListaEmpresa.length; ++i) {
                        selector.options[selector.options.length] = new Option(result.ListaEmpresa[i].Emprnomb, result.ListaEmpresa[i].Emprcodi);
                    }
                }
                $('#cboEmpresa').multipleSelect('refresh');
            }
            if (opcion == 1 || opcion == 2) {
                $('#cboPtomedicion').empty();
                var selector = document.getElementById('cboPtomedicion');
                selector.options[selector.options.length] = new Option('-Seleccione-', 0);
                selector.options[0].setAttribute('selected', 'selected');

                if (result.ListPtoMedicion.length != 0) {

                    for (var i = 0; i < result.ListPtoMedicion.length; ++i) {
                        selector.options[selector.options.length] = new Option(
                            result.ListPtoMedicion[i].Famnomb + ' - ' +
                            result.ListPtoMedicion[i].Equinomb, result.ListPtoMedicion[i].Ptomedicodi);
                    }
                }
                $('#cboPtomedicion').multipleSelect('refresh');
            }
        },
        error: function () {
            alert("Ha ocurrido un problema...");
        }
    });
}