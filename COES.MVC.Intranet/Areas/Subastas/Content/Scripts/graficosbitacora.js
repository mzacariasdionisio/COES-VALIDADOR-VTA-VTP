var controlador = siteRoot + 'Subastas/GraficosYBitacora/';

const TG_URSSUBIR = 1;
const TG_URSBAJAR = 2;
const TG_TOTALSUBIR = 3;
const TG_TOTALBAJAR = 4;

var seAbrioBitacora = false;
var existenURSParaFiltro;

$(function () {

    /********************** GENERAL *********************/
    /***************************************************/
    $('#tab-container').easytabs({
        animate: false
    });
    $('#tab-container').easytabs('select', '#tabGraficos');


    /********************** Graficos *********************/
    /***************************************************/
    inicializarComboUrs();

    $('#txtFecOferta').Zebra_DatePicker({
        format: "d/m/Y",
        direction: ['01/01/1900', diaManiana()],
        onSelect: function (date) {
            ActualizarListadoUrs();
        }
    });

    $('#cbTipoOferta').change(function () {
        ActualizarListadoUrs();
    });

    $('#cbTipoGrafico').change(function () {
        visibilidadCampoUrs();
        ActualizarListadoUrs();
    });

    $('#btnConsultarGraficos').click(function () {
        mostrarGrafico();
    });

    visibilidadCampoUrs();
    ActualizarListadoUrs();
    setTimeout(function () {
        if (existenURSParaFiltro) {
            mostrarGrafico();
        }
    }, 2500);



    /********************** Bitacora *********************/
    /***************************************************/
    $('#tab-container').bind('easytabs:before', function (id, val, t) {
        if (t.selector == "#tabBitacora") {
            if (!seAbrioBitacora) {
                mostrarBitacora();
            }
            seAbrioBitacora = true;
        }
    });

    $('#FechaDesdeB').Zebra_DatePicker({
        format: "d/m/Y",
        pair: $('#FechaHastaB'),
        direction: false,
    });

    $('#FechaHastaB').Zebra_DatePicker({
        format: "d/m/Y",
        pair: $('#FechaDesdeB'),
        direction: [true, diaManiana()],
    });


    $('#btnConsultarBitacora').click(function () {
        mostrarBitacora();
    });

    $('#btnExpotarBitacora').click(function () {
        exportarBitacoraExcel();
    });

});

/******************INICIO GRAFICOS*****************/
function inicializarComboUrs() {
    $('#cbUrs').multipleSelect({
        width: '200px',
        filter: true,
    });
    $('#cbUrs').multipleSelect('checkAll');
}

function visibilidadCampoUrs() {
    var tipoGrafico = parseInt($("#cbTipoGrafico").val()) || 0;

    if (tipoGrafico == TG_URSSUBIR || tipoGrafico == TG_URSBAJAR) {
        $(".dataoUrs").css("display", "inline");
        $('#cbUrs').multipleSelect('checkAll');
    } else {
        $(".dataoUrs").css("display", "none");
    }

}

function ActualizarListadoUrs() {
    existenURSParaFiltro = false;
    limpiarBarraMensaje("mensaje_graficos");
    var tipoGrafico = parseInt($("#cbTipoGrafico").val()) || 0;

    if (tipoGrafico == TG_URSSUBIR || tipoGrafico == TG_URSBAJAR) {
        var filtro = datosFiltro();
        $("#cbUrs").empty();
        $("#cbUrs").multipleSelect("setSelects", [-1]);
        $.ajax({
            type: 'POST',
            url: controlador + 'ActualizarUrsSegunFiltro',
            data: {
                fechaOferta: filtro.fechaOferta,
                tipoOferta: filtro.tipoOferta,
                tipoGrafico: filtro.tipoGrafico,
            },

            success: function (evt) {
                if (evt.Resultado != -1) {
                    var listaUrsConOferta = evt.ListaUrsModoOperacion;

                    if (listaUrsConOferta.length > 0) {
                        for (var i = 0; i < listaUrsConOferta.length; i++) {
                            $('#cbUrs').append('<option value=' + listaUrsConOferta[i].Urscodi + '>' + listaUrsConOferta[i].Ursnomb + '</option>');
                        }
                        existenURSParaFiltro = true;
                    }
                    else {
                        mostrarMensaje('mensaje_graficos', 'alert', "No hay URS que ofertaron para el filtro seleccionado"); 
                        $("#grafico1").html("");
                    }

                    inicializarComboUrs();

                } else {
                    mostrarMensaje('mensaje_graficos', 'error', "Ha ocurrido un error: " + evt.Mensaje);
                }

            },
            error: function (err) {
                mostrarMensaje('mensaje_graficos', 'error', 'Se ha producido un error al cargar el listado de URS.');

            }
        });

    }


}

function mostrarGrafico() {
    limpiarBarraMensaje("mensaje_graficos");
    $("#grafico1").html("");
    var filtro = datosFiltro();
    var msg = validarDatosFiltroConsulta(filtro);
    var idUrs = filtro.todos ? "-1" : $('#hfIdUrs').val();

    if (msg == "") {

        $.ajax({
            type: 'POST',
            url: controlador + 'ConsultarGraficoOferta',
            data: {
                fechaOferta: filtro.fechaOferta,
                tipoOferta: filtro.tipoOferta,
                tipoGrafico: filtro.tipoGrafico,
                idsUrs: idUrs
            },
            success: function (evt) {

                if (evt.Resultado == 1) {

                    disenioGrafico(evt.Grafico, 'grafico1');

                    if (!evt.ExisteReserva)
                        mostrarMensaje('mensaje_graficos', 'alert', "No se encontró información de RSF para los filtros ingresados.");

                } else {
                    mostrarMensaje('mensaje_graficos', 'error', evt.Mensaje);
                }
            },
            error: function (err) {
                mostrarMensaje('mensaje_graficos', 'error', "Ha ocurrido un error");

            }
        });
    }
    else {
        mostrarMensaje('mensaje_graficos', 'error', msg);
    }
}


function datosFiltro() {
    var filtro = {};
    var Todos = false;
    var idUrss = "0";
    var tipoGrafico = parseInt($("#cbTipoGrafico").val()) || 0;

    if (tipoGrafico == TG_URSSUBIR || tipoGrafico == TG_URSBAJAR) {
        idUrss = $('#cbUrs').multipleSelect('getSelects');
        if (idUrss == "[object Object]") idUrss = "-1";

        //verifico si esta seleccionado TODOS        
        var lstSel = [];
        $("input:checkbox[name=selectAllIdUrs]:checked").each(function () {
            var textoFiltrado = $('.ms-search input').val();
            if (textoFiltrado == "")
                lstSel.push($(this));
        });
        if (lstSel.length > 0)
            Todos = true;

        $('#hfIdUrs').val(idUrss);
    }

    var fecOferta = $('#txtFecOferta').val();
    var tipoOferta = $('#cbTipoOferta').val();
    var tipoGrafico = $('#cbTipoGrafico').val();



    filtro.fechaOferta = fecOferta;
    filtro.tipoOferta = tipoOferta;
    filtro.tipoGrafico = tipoGrafico;
    filtro.idUrs = idUrss;
    filtro.todos = Todos;

    return filtro;
}

function validarDatosFiltroConsulta(datos) {
    var msj = "";

    var tipoGrafico = parseInt($("#cbTipoGrafico").val()) || 0;

    if (tipoGrafico == TG_URSSUBIR || tipoGrafico == TG_URSBAJAR) {
        if (datos.idUrs.length == 0) {
            //Si existen urs con ofertas pero no es escogio alguno
            if (existenURSParaFiltro) {
                msj += "<p>Debe escoger como mínimo una URS.</p>"; //
            } else { // no se escoge urs porque no hay urs con oferta para el filtro
                msj += "<p>No hay URS que ofertaron para el filtro seleccionado.</p>"; // 
                $("#grafico1").html("");
            }
            
        }
    }

    if (datos.fechaOferta == "") {
        msj += "<p>Debe escoger una fecha de Oferta correcta.</p>";
    } else {
        if (convertirFecha(datos.fechaOferta) > convertirFecha(diaManiana())) {
            msj += "<p>Debe escoger una fecha de oferta correcta, la fecha de oferta no debe superar al dia de mañana.</p>";
        }
    }

    return msj;
}

function disenioGrafico(grafico, idContenedorGrafico) {
    //generar series
    var series = [];
    for (var i = 0; i < grafico.Series.length; i++) {
        var serie = grafico.Series[i];

        if (serie.Type == "line") {
            var obj = {
                name: serie.Name,
                type: serie.Type,
                yAxis: serie.YAxis,
                data: grafico.SeriesData[i],
                dashStyle: 'LongDashDotDot',
                color: '#FF0000'
            };
        } else {
            var obj = {
                name: serie.Name,
                type: serie.Type,
                yAxis: serie.YAxis,
                data: grafico.SeriesData[i],
            };

        }

        series.push(obj);
    }
    var dataHora = grafico.XAxisCategories;
    var tituloGrafico = grafico.TitleText;
    var tituloExportacion = tituloGrafico.replace(/ /g, "");

    //Generar grafica
    Highcharts.chart(idContenedorGrafico, {
        chart: {
            //type: 'area',
            //inverted: true,
            zoomType: 'xy'
        },
        title: {
            text: tituloGrafico,
            style: {
                color: '#6D1D65',
                fontWeight: 'bold'
            }
        },
        subtitle: {
            text: ''
        },
        xAxis: [{
            categories: dataHora,
            crosshair: true,
            gridLineWidth: 1,
            labels: {
                rotation: 290
            }
        }],
        yAxis: {
            title: {
                text: 'Potencia [MW]',
                style: {
                    //color: 'red',
                    fontSize: 20
                }
            },
            labels: {
                format: '{value}',
            }
        },
        tooltip: {
            shared: true,
            useHTML: true,
            headerFormat: '<table><tr><th colspan="2" style="text-align: center;"><span id="tituloToltipGrafico" style="font-size: 15px">{point.key}</span></th></tr>',
            pointFormat: '<tr style="font-size: 15px"><td><span style="color:{series.color}">\u25CF</span> {series.name}: ' +

                '</td>' +
                '<td style="text-align: right"><b>{point.y} MW</b></td></tr>',
            footerFormat: '</table>',
        },
        legend: {
            align: 'center',
            verticalAlign: 'bottom',
            layout: 'horizontal'
        },
        //legend: {
        //    layout: 'vertical',
        //    align: 'right',
        //    verticalAlign: 'middle'
        //},
        plotOptions: {
            area: {
                stacking: 'normal',
                lineColor: '#666666',
                lineWidth: 1,
                marker: {
                    lineWidth: 1,
                    lineColor: '#666666'
                }
            }
        },
        exporting: {
            sourceWidth: 1400,
            sourceHeight: 600,
            scale: 2,
            filename: tituloExportacion,

            buttons: {
                customButton: {
                    /*menuItems: ["viewFullscreen", "printChart", "separator", "downloadPNG", "downloadJPEG", "downloadPDF", "downloadSVG"],*/
                    menuItems: ["downloadPNG", "downloadJPEG", "downloadPDF", "downloadSVG"],
                    className: 'myClass',
                    text: "Exportar",

                    theme: {
                        padding: 10,
                        'stroke-width': 2,
                        stroke: '#287AB0', //linea azul
                        style: {
                            color: 'white',
                            fontWeight: 'bold'
                        },
                        fill: '#2980B9',//fondo azul
                        r: 3,
                        states: {
                            hover: {
                                fill: '#CAE5FB',
                                style: {
                                    color: 'black'
                                },
                            },
                            //select: {
                            //    stroke: '#039',
                            //    fill: '#a4edba'
                            //}
                        }
                    }
                },
                contextButton: {
                    enabled: false
                },
            },
        },
        lang: {
            downloadPNG: "Exportar gráfico en archivo .PNG",
            downloadJPEG: "Exportar gráfico en archivo .JPEG",
            downloadPDF: "Exportar gráfico en archivo .PDF",
            downloadSVG: "Exportar gráfico en archivo .SVG",
        },
        series: series
    });
}
/******************FIN GRAFICOS*****************/




/******************INICIO BITACORA*****************/
function mostrarBitacora() {
    limpiarBarraMensaje("mensaje_bitacora");
    $("#contenedorBitacora").html('');

    var objData = getObjetoFiltro();
    var msj = validarConsulta(objData);

    if (msj == "") {
        $.ajax({
            type: 'POST',
            url: controlador + 'ListarBitacora',
            data: {
                fechaInicial: objData.FechaInicio,
                fechaFinal: objData.FechaFin
            },
            dataType: 'json',
            success: function (evt) {

                if (evt.Resultado != -1) {
                                       
                    if (evt.Resultado == 2) {
                        mostrarMensaje('mensaje_bitacora', 'error', "El rango ingresado para consulta sobrepasa el máximo permitido: 367 días");
                    } else {
                        var htmlBitacora = _dibujarTablaBitacora(evt.ListaBitacora);
                        $("#contenedorBitacora").html(htmlBitacora);

                        $('#tblBitacora').dataTable({
                            "scrollY": "400px",
                            "scrollX": true,
                            "scrollCollapse": false,
                            "sDom": 't',
                            "ordering": false,
                            paging: false,
                            "bAutoWidth": false,
                            "destroy": "true",
                            "iDisplayLength": -1
                        });
                    }

                } else {
                    mostrarMensaje('mensaje_bitacora', 'error', "Ha ocurrido un error: " + evt.Mensaje);
                }
            },
            error: function (err) {

                mostrarMensaje('mensaje_bitacora', 'error', "Ha ocurrido un error");
            }
        });
    } else {
        mostrarMensaje('mensaje_bitacora', 'error', msj);
    }
}

function _dibujarTablaBitacora(lista) {

    var cadena = '';
    cadena += `
    <table border="0" class="pretty tabla-icono" cellspacing="0" width="100%" id="tblBitacora">
        <thead>
            <tr>
                <th style='width: 100px'>Fecha de oferta </th>
                <th style='width: 100px'>Déficit</th>
                <th style='width: 100px'>Horario</th>
                <th style='width: 100px'>Promedio</th>
                <th style='width: 100px'>Mínimo</th>
                <th style='width: 100px'>Máximo</th>
                <th style='width: 300px'>Motivo de activación oferta por defecto</th>
                <th style='width: 100px'>Reducción de <br/> banda a (MW)</th>
            </tr>
        </thead>
        <tbody>
    `;
    var numF = 0;
    for (var key in lista) {
        let item = lista[key];
        let colorBlanco = "background: #FFFFFF";
        let colorGris = "background: #F9F8F8";
        let resultado = numF % 2;
        let estilo = resultado == 0 ? colorBlanco : colorGris;

        let prom = item.Promedio != null ? item.Promedio.toFixed(2) : "";
        let min = item.Minimo != null ? item.Minimo.toFixed(2) : "";
        let max = item.Maximo != null ? item.Maximo.toFixed(2) : "";
        let banda = item.Banda != null ? item.Banda.toFixed(2) : "";

        cadena += `
                <tr>
                    <td style="${estilo}">${item.FechaOferta}</td>
                    <td style="${estilo}">${item.Deficit}</td>
                    <td style="${estilo}">${item.Horario}</td>
                    <td style="${estilo}">${prom}</td>
                    <td style="${estilo}">${min}</td>
                    <td style="${estilo}">${max}</td>
                    <td style="${estilo}">${item.Motivos}</td>
                    <td style="${estilo}">${banda}</td>
                </tr>
                 `;

        numF++;
    }
    cadena += "</tbody></table>";

    return cadena;
}

function getObjetoFiltro() {

    var fechaInicio = $('#FechaDesdeB').val();
    var fechaFin = $('#FechaHastaB').val();

    var obj = {
        FechaInicio: fechaInicio,
        FechaFin: fechaFin
    };

    return obj;
}

function validarConsulta(objFiltro) {
    var listaMsj = [];

    // Valida consistencia del rango de fechas
    var fechaInicio = objFiltro.FechaInicio;
    var fechaFin = objFiltro.FechaFin;
    if (fechaInicio != "" && fechaFin != "") {
        if (CompararFechas(fechaInicio, fechaFin) == false) {
            listaMsj.push("La fecha de inicio no puede ser mayor que la fecha de fin.");
        }
    }

    var msj = listaMsj.join('\n');
    return msj;
}

function exportarBitacoraExcel() {
    limpiarBarraMensaje("mensaje_bitacora");

    var objData = getObjetoFiltro();
    var msj = validarConsulta(objData);

    if (msj == "") {
        $.ajax({
            type: 'POST',
            url: controlador + 'GenerarExcelBitacora',
            data: {
                fechaInicial: objData.FechaInicio,
                fechaFinal: objData.FechaFin
            },
            dataType: 'json',
            success: function (evt) {
                if (evt.Resultado != -1) {
                    if (evt.Resultado == 2) {
                        mostrarMensaje('mensaje_bitacora', 'error', "El rango ingresado para exportación sobrepasa el máximo permitido: 367 días"); 
                        $("#contenedorBitacora").html('');
                    } else {
                        window.location = controlador + "AbrirArchivo?file=" + evt.NombreArchivo;
                    }

                } else {
                    mostrarMensaje('mensaje_bitacora', 'error', "Ha ocurrido un error: " + evt.Mensaje);
                }
            },
            error: function (err) {

                mostrarMensaje('mensaje_bitacora', 'error', "Ha ocurrido un error");
            }
        });
    } else {
        mostrarMensaje('mensaje_bitacora', 'error', msj);
    }
}
/******************FIN BITACORA*****************/




/**********************************************/
/************ Funciones Generales  ************/
function CompararFechas(fecha1, fecha2) {

    //Split de las fechas recibidas para separarlas
    var x = fecha1.split('/');
    var z = fecha2.split('/');

    //Cambiamos el orden al formato americano, de esto dd/mm/yyyy a esto mm/dd/yyyy
    fecha1 = x[1] + '/' + x[0] + '/' + x[2];
    fecha2 = z[1] + '/' + z[0] + '/' + z[2];

    //Comparamos las fechas
    if (Date.parse(fecha1) > Date.parse(fecha2)) {
        return false;
    } else {
        return true;
    }
}


function convertirFecha(fecha) {
    var arrayFecha = fecha.split('/');
    var dia = arrayFecha[0];
    var mes = arrayFecha[1];
    var anio = arrayFecha[2];

    var salida = anio + mes + dia;
    return salida;
}

function AddZero(num) {
    return (num >= 0 && num < 10) ? "0" + num : num + "";
}

function diaManiana() { //devuelve strFecha en formato dd/mm/yyyy
    var hoy = new Date();
    let DIA_EN_MILISEGUNDOS = 24 * 60 * 60 * 1000;
    //let DIA_EN_MILISEGUNDOS = 48 * 60 * 60 * 1000;
    let manana = new Date(hoy.getTime() + DIA_EN_MILISEGUNDOS);
    var strDateTime = [[AddZero(manana.getDate()), AddZero(manana.getMonth() + 1), manana.getFullYear()].join("/")].join(" ");

    return strDateTime;
}


function cerrarPopup(id) {
    $("#" + id).bPopup().close()
}

function mostrarMensaje(id, tipo, mensaje) {
    $("#" + id).css("display", "block");
    $('#' + id).removeClass();
    $('#' + id).addClass('action-' + tipo);
    $('#' + id).html(mensaje);

}

function limpiarBarraMensaje(id) {
    $('#' + id).css("display", "none");
    $('#' + id).removeClass();
    $('#' + id).addClass('action-message');
    $('#' + id).html('');
}
