var controlador = siteRoot + 'Combustibles/reporteGas/';

var IMG_DESCARGAR_EXCEL = '<img src="' + siteRoot + 'Content/Images/downloadExcel.png" alt="Descargar Detalle" width="19" height="19" style="">';

var LISTAREPORTEXDIA = [{ codigo: 1, valor: "Reporte Costos Variables (S//kWh)" }, { codigo: 2, valor: "Reporte Costos Variables (USD/kWh)" }];
var LISTAREPORTEXMES = [{ codigo: 3, valor: "Reporte Costo de C.G. - PCI (USD/GJ)" }, { codigo: 4, valor: "Reporte P.U. de C.G. por concepto - PCI (USD/GJ)" },
{ codigo: 5, valor: "Reporte P.U. de C.G. por concepto – Formato 3 (USD/GJ)" }, { codigo: 6, valor: "Reporte de Poder calorífico" }];

var LISTAITEMS = [{ codigo: 1, valor: "Suministro" }, { codigo: 2, valor: "Transporte" }, { codigo: 3, valor: "Distribución" }, { codigo: 4, valor: "Sum., trans. y Distribución" }];
var NOTA = `Nota: Se recomienda realizar la consulta por una central. En caso se elijan más centrales, este proceso podría tardar varios minutos.`;
var NOTA_MENSUAL = "";
$(function () {

    $('#cbEmpresa').multipleSelect({
        width: '250px',
        filter: true,
    });

    $('#cbCentral').multipleSelect({
        width: '250px',
        filter: true,
    });

    $('#cbEmpresa').multipleSelect('checkAll');
    $('#cbCentral').multipleSelect('checkAll');

    $('#FechaDesde').Zebra_DatePicker({
        format: 'd/m/Y',
    });

    $('#FechaHasta').Zebra_DatePicker({
        format: 'd/m/Y',
    });

    $('#btnBuscar').click(function () {
        buscarEnvio();
    });

    $('#btnExportar').click(function () {
        exportar();
    });

    $('#btnGrafico').click(function () {
        opcion = $('#cbTipoReporte').val() > 3 ? 1 : 0;
        generarGrafico(opcion);
    });

    $('#cboTipoMedida').change(function (e) {
        if ($(this).val() == -1)
            alert("Seleccione una opción de gráfico");
        else
            generarGrafico($(this).val());
    });

    $('#cbEmpresa').on("change", function () {
        listarCentralesFiltro();
    });

    $('input[name=rbidTipo][value=1]').attr('checked', true);
    cambiarFormatoFecha("1");
    //buscarEnvio();
});


function generarGrafico(opcion) {
    var centrales = $('#cbCentral').multipleSelect('getSelects');
    if (centrales == "[object Object]") empresa = "-1";
    if (centrales == "") centrales = "-1";
    $('#hfCentral').val(centrales);

    var msj = validarDatos();

    if (msj == "") {
        $.ajax({
            type: 'POST',
            url: controlador + 'GenerarGrafico',
            data: {
                centrales: $('#hfCentral').val(),
                tipoReporte: $('#cbTipoReporte').val(),
                finicio: $('#FechaDesde').val(),
                ffin: $('#FechaHasta').val(),
                opcion: opcion
            },
            dataType: 'json',
            success: function (evt) {
                if (evt.Resultado != "-1") {
                    limpiarBarraMensaje("mensaje");
                    if ($('#cbTipoReporte').val() >= 3) {
                        $('#idGrafico1').hide();
                        $('#idGrafico2').show();

                        if ($('#cbTipoReporte').val() == 3)
                            $('#IdCombo').hide();
                        else
                            $('#IdCombo').show();

                        graficoBarraXTipo(evt.Grafico2, "idGrafico2");
                    }
                    else {
                        $('#idGrafico1').show();
                        $('#idGrafico2').show();
                        $('#IdCombo').hide();

                        GraficoLinea(evt.Grafico1, "idGrafico1");
                        GraficoColumnas(evt.Grafico2, "idGrafico2");

                        //document.getElementById("myiframe").style.height = "1800px";
                    }

                } else {
                    mostrarMensaje('mensaje', 'error', 'Error: ' + evt.Mensaje);
                }
            },
            error: function (err) {
                alert('Ha ocurrido un error.');
            }
        });
    }
    else {
        mostrarMensaje('mensaje', 'alert', msj);
    }
}

function buscarEnvio() {
    var centrales = $('#cbCentral').multipleSelect('getSelects');
    if (centrales == "[object Object]") empresa = "-1";
    if (centrales == "") centrales = "-1";
    $('#hfCentral').val(centrales);

    var msj = validarDatos();
    if (msj == "") {
        $.ajax({
            type: "POST",
            url: controlador + "ListarReporteHistorico",
            dataType: 'json',
            data: {
                centrales: $('#hfCentral').val(),
                tipoReporte: $('#cbTipoReporte').val(),
                finicio: $('#FechaDesde').val(),
                ffin: $('#FechaHasta').val()
            },
            success: function (evt) {
                if (evt.Resultado != "-1") {
                    limpiarBarraMensaje("mensaje");

                    if ($('#cbTipoReporte').val() >= 3) {
                        switch ($('#cbTipoReporte').val()) {
                            case "4":
                                NOTA_MENSUAL = "Referido al PCI";
                                break;
                            case "5":
                                NOTA_MENSUAL = "Referido al PCS";
                                break;
                            default:
                                NOTA_MENSUAL = "";
                        }
                    }
                    else {
                        NOTA_MENSUAL = "";
                    }

                    $("#listadoEnvios").html(evt.Resultado);

                    $('#listadoEnvios').css("width", $('#mainLayout').width() + "px");
                    $('#tabla_data').dataTable({
                        filter: false,
                        info: "true",
                        scrollY: "400px",
                        scrollCollapse: true,
                        paging: false,
                        scrollX: true,
                        "ordering": false,
                        "language": {
                            "sInfo": NOTA_MENSUAL
                        }
                    });

                    if ($('#cbTipoReporte').val() > 3) {
                        var lista = $('#cbTipoReporte').val() != 6 ? LISTAITEMS.filter(x => x.codigo < 4) : LISTAITEMS;

                        $('#cboTipoMedida').empty();
                        var option = '<option value="-1" >-- SELECCIONE-- </option>';
                        $.each(lista, function (k, v) {
                            option += '<option value =' + v.codigo + '>' + v.valor + '</option>';
                        })
                        $('#cboTipoMedida').append(option);
                    }

                    $('#idGrafico1').hide();
                    $('#idGrafico2').hide();
                    $('#IdCombo').hide();

                } else {
                    mostrarMensaje('mensaje', 'error', evt.Mensaje);
                }
            },
            error: function (err) {
                alert('Ha ocurrido un error.');
            }
        });
    }
    else {
        mostrarMensaje('mensaje', 'alert', msj);
    }
}

function exportar() {
    //limpiarBarraMensaje("mensaje");

    var empresa = $('#cbEmpresa').multipleSelect('getSelects');
    var centrales = $('#cbCentral').multipleSelect('getSelects');
    if (empresa == "[object Object]") empresa = "-1";
    if (centrales == "[object Object]") empresa = "-1";
    if (empresa == "") empresa = "-1";
    if (centrales == "") centrales = "-1";
    $('#hfEmpresa').val(empresa);
    $('#hfCentral').val(centrales);

    var msj = validarDatos();

    if (msj == "") {
        $.ajax({
            type: 'POST',
            url: controlador + 'GenerarReporteExcelHistorico',
            data: {
                centrales: $('#hfCentral').val(),
                tipoReporte: $('#cbTipoReporte').val(),
                finicio: $('#FechaDesde').val(),
                ffin: $('#FechaHasta').val()
            },
            dataType: 'json',
            success: function (evt) {
                if (evt.Resultado != "-1") {
                    limpiarBarraMensaje("mensaje");
                    window.location = controlador + "Exportar?file_name=" + evt.Resultado;
                } else {
                    mostrarMensaje('mensaje', 'error', 'Error: ' + evt.Mensaje);
                }
            },
            error: function (err) {
                alert('Ha ocurrido un error.');
            }
        });
    }
    else {
        mostrarMensaje('mensaje', 'alert', msj);
    }
}

function cambiarFormatoFecha(tipo) {

    limpiarBarraMensaje("mensaje");
    $("#idnota").html('');
    switch (tipo) {
        case '1': //dia
            $("#idnota").html(NOTA);
            $('#hfidTipo').val(1);
            $("#listadoEnvios").html("");

            $('#FechaDesde').val($('#hfFechaDesde').val());
            $('#FechaHasta').val($('#hfFechaHasta').val());

            var fecha = new Date();
            fecha = new Date(fecha.setMonth(fecha.getMonth() + 1));
            fecha = new Date(fecha.getFullYear(), fecha.getMonth() + 1, 0);
            var fechadia = diaActual(fecha);

            $('#FechaDesde').Zebra_DatePicker({
                format: 'd/m/Y',
                //pair: $('#FechaHasta'),
                //direction: [false, stFechaFin]
                direction: ["01/04/2000", fechadia]
            });
            $('#FechaHasta').Zebra_DatePicker({
                format: 'd/m/Y',
                //pair: $('#FechaDesde'),
                direction: ["01/04/2000", fechadia]
            });

            $('#cbTipoReporte').empty();
            var option = '<option value="0" >-- SELECCIONE-- </option>';
            $.each(LISTAREPORTEXDIA, function (k, v) {
                option += '<option value =' + v.codigo + '>' + v.valor + '</option>';
            })
            $('#cbTipoReporte').append(option);

            break;
        case '2':// mes
            $('#hfidTipo').val(2);
            $("#listadoEnvios").html("");

            var fecha = new Date();
            var fechafin = new Date();
            var fechaLim = new Date();
            //var mes = "0" + (fecha.getMonth() + 1).toString();
            //mes = mes.substr(mes.length - 2, mes.length);
            var stFecha = obtenerFechaMes(fecha, 0);
            var stFechaFin = obtenerFechaMes(fechafin, 1);
            $('#FechaDesde').val(stFecha);
            $('#FechaHasta').val(stFechaFin);

            $('#FechaDesde').Zebra_DatePicker({
                format: 'm Y',
                pair: $('#FechaHasta'),
                direction: [false, stFechaFin]
            });
            $('#FechaHasta').Zebra_DatePicker({
                format: 'm Y',
                pair: $('#FechaDesde'),
                direction: [true, stFechaFin]
            });

            $('#cbTipoReporte').empty();
            var option = '<option value="0" >-- SELECCIONE-- </option>';
            $.each(LISTAREPORTEXMES, function (k, v) {
                option += '<option value =' + v.codigo + '>' + v.valor + '</option>';
            })
            $('#cbTipoReporte').append(option);

            break;
    }

}

function obtenerFechaMes(fecha , numero ) {
    fecha = new Date(fecha.setMonth(fecha.getMonth() + numero));
    var mesfin = "0" + (fecha.getMonth() + 1).toString();
    mesfin = mesfin.substr(mesfin.length - 2, mesfin.length);
    var stFecha = mesfin + " " + fecha.getFullYear();

    return stFecha;
}

function AddZero(num) {
    return (num >= 0 && num < 10) ? "0" + num : num + "";
}

function diaActual(now) { //devuelve strFecha en formato dd/mm/yyyy
    var strDateTime = [[AddZero(now.getDate()), AddZero(now.getMonth() + 1), now.getFullYear()].join("/")].join(" ");

    return strDateTime;
}

function handleClick(myRadio) {
    var valorRadio = myRadio.value;
    cambiarFormatoFecha(valorRadio);
}

//validar consulta
function validarDatos() {
    var validacion = "<ul>";
    var flag = true;

    // solo para resolución diaria
    if ($('#hfidTipo').val() == 1) {
        if (convertirFecha($('#FechaHasta').val()) < convertirFecha($('#FechaDesde').val())) {
            validacion = validacion + "<li>Fecha Hasta: debe ser mayor o igual que la fecha inicio .</li>";//Campo Requerido
            flag = false;
        }
    }

    // solo para resolución mensual
    if ($('#hfidTipo').val() == 2) {
        if ($('#FechaHasta').val() <= $('#FechaDesde').val()) {
            validacion = validacion + "<li>Fecha Hasta: debe ser mayor al mes fecha inicio .</li>";//Campo Requerido
            flag = false;
        }
    }
    if ($('#cbTipoReporte').val() == 0) {
        validacion = validacion + "<li>Tipo de reporte: debe seleccionar una opción válida.</li>";//Campo Requerido
        flag = false;
    }

    validacion = validacion + "</ul>";

    if (flag == true) validacion = "";

    return validacion;
}

function convertirFecha(fecha) {
    var arrayFecha = fecha.split('/');
    var dia = arrayFecha[0];
    var mes = arrayFecha[1];
    var anio = arrayFecha[2];

    var salida = anio + mes + dia;
    return salida;
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

function listarCentralesFiltro() {
    $.ajax({
        type: 'POST',
        url: controlador + "ListarCbCentralXEmpresa",
        datatype: 'json',
        data: JSON.stringify({
            idEmpresa: $('#cbEmpresa').val()
        }),
        contentType: "application/json",
        success: function (modelo) {
            $('#cbCentral').empty();
            $.each(modelo.ListadoCentrales, function (k, v) {
                var option = '<option value =' + v.Equicodi + '>' + v.Equinomb + '</option>';
                $('#cbCentral').append(option);
            })
            $('#cbCentral').multipleSelect({
                filter: true,
                //placeholder: "-- Seleccione Ubicación y/o Familia --",
                //selectAll: false,
                //allSelected: onoffline
            });
            $("#cbCentral").multipleSelect("refresh");
            $('#cbCentral').multipleSelect('checkAll');
        }
    });
}

function graficoBarraXTipo(result, idGrafico) {
    var categoria = [];

    for (var d in result.Categorias) {
        var item = result.Categorias[d];
        if (item == null) {
            continue;
        }
        categoria.push({
            name: item.Name,
            categories: item.Categories,
        });
    }

    var opcion = {
        chart: {
            zoomType: 'xy',
            shadow: true,
            spacingTop: 50,
            spacingBottom: 50
        },
        title: {
            text: result.TitleText
        },
        subtitle: {
            text: result.Subtitle,
            align: 'left',
            //x: -10,
            verticalAlign: 'bottom',
            floating: true,
            x: 10,
            y: 15
        },
        legend: {
            align: 'center',
            verticalAlign: 'top',
            layout: 'horizontal'
        },
        xAxis: {
            categories: categoria,
            labels: {
                rotation: -90
            },
            crosshair: true
        },
        yAxis: [{ //Primary Axes
            title: {
                text: result.Series[0].YAxisTitle,
                align: 'high',
                textAlign: 'center',
                rotation: 0,
                offset: 0,
                y: -25,
                x: -10,
            },
            labels: {
                format: '{value:,.2f}'
            }
        }],
        tooltip: {
            pointFormat: '<span style="color:{series.color}">{series.name}</span>: <b>{point.y}</b> <br/>',
            shared: true
        },
        plotOptions: {
            spline: { // PORCENTAJES
                marker: {
                    fillColor: '#FFFFFF',
                    lineWidth: 4,
                    lineColor: null // inherit from series
                }
            }
        },
        series: []
    };

    for (var i in result.Series) {
        opcion.series.push({
            name: result.Series[i].Name,
            type: result.Series[i].Type,
            color: result.Series[i].Color,
            yAxis: result.Series[i].YAxis,
            data: result.SeriesData[i]
        });
    }

    $('#' + idGrafico).highcharts(opcion);
}