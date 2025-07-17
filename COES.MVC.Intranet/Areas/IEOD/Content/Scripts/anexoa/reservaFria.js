//variables globales
var controlador = siteRoot + 'IEOD/AnexoA/'

$(function () {

    $('#tab-container').easytabs({
        animate: false
    });

    $('#cbEmpresa').multipleSelect({
        width: '250px',
        filter: true,
        onClose: function (view) {
            cargarListaReporteReservaFría();
        }
    });

    $('#cbTipoCombustible').multipleSelect({
        width: '150px',
        filter: true,
        onClose: function (view) {
            cargarListaReporteReservaFría();
        }
    });

    parametro1 = getValorFiltroRF();
    $('input[name=filtro_RF]').change(function () {
        parametro1 = getValorFiltroRF();
    });

    $('#btnBuscar').click(function () {
        cargarListaReporteReservaFría();
        cargarUnidadesRFria();

    });

    $('#btnAgregarRFria').on('click', function () {
        editarRFria(0);
    });

    $('#btnExportarRFria').on('click', function () {
        exportarRFria();
    });

    ancho = $("#mainLayout").width() > 900 ? $("#mainLayout").width() - 30 : 900;

    cargarValoresIniciales();

    cargarUnidadesRFria();
});

function cargarValoresIniciales() {
    $('#cbEmpresa').multipleSelect('checkAll');
    $('#cbTipoCombustible').multipleSelect('checkAll');
    cargarListaReporteReservaFría();
}

function mostrarReporteByFiltros() {
    cargarListaReporteReservaFría();
}

function cargarListaReporteReservaFría() {
    var idEmpresa = getEmpresa();
    var tipoCombustible = getTipoCombustible();
    var fechaInicio = getFechaInicio();
    var fechaFin = getFechaFin();

    $("#listado").html('');
    $("#idVistaGrafica").html('');

    validacionesxFiltro(1);
    if (resultFiltro) {
        $.ajax({
            type: 'POST',
            url: controlador + 'CargarReporteReservaFriaSistema',
            data: {
                idEmpresa: idEmpresa,
                tipoCombustible: tipoCombustible,
                fechaInicio: fechaInicio,
                fechaFin: fechaFin,
                filtroRF: getValorFiltroRF()
            },
            success: function (data) {

                if (data.Resultado != "-1") {
                    $('.filtro_fecha_desc').html(data.FiltroFechaDesc);
                    $('#listado').html(data.Resultado);

                    var anchoReporte = $('#reporte').width();
                    $("#resultado").css("width", (anchoReporte > ancho ? ancho : anchoReporte) + "px");

                    $('#reporte').dataTable({
                        "scrollX": true,
                        "scrollY": "780px",
                        "scrollCollapse": true,
                        "sDom": 't',
                        "ordering": false,
                        paging: false,
                        fixedColumns: {
                            leftColumns: 1
                        }
                    });

                    var htmlGrafico = "";
                    for (var i = 0; i < data.Graficos.length; i++) {
                        htmlGrafico += `
                            <div id="grafico_${i}" style="float: left; width: 500px; height: 500px;"></div>
                        `;
                    }

                    $("#idVistaGrafica").html(htmlGrafico);

                    for (var i = 0; i < data.Graficos.length; i++) {
                        graficoAreaRfria(`grafico_${i}`, data.Graficos[i]);
                    }
                } else {
                    alert("Ha ocurrido un error: " + data.Mensaje);
                }
            },
            error: function (err) {
                alert("Ha ocurrido un error");
                console.log(err.responseText);
            }
        });
    }
    else {
        $("#listado").html('');
        $('#idGraficoContainer').html('');
    }
}

function graficoAreaRfria(idVista, grafico) {
    var Varserie = [[]];
    var serieName = grafico.SeriesName;

    var opcion = {
        chart: {
            type: 'area',
            shadow: true
        },
        title: {
            text: grafico.TitleText
        },
        subtitle: {
            title: {
                enabled: true,
                text: ''
            }
        },
        xAxis: {
            categories: serieName,
            labels: {
                rotation: -90
            }
        },
        yAxis: {
            title: {
                text: 'MW'
            },
            labels: {
                formatter: function () {
                    return this.value;
                }
            },
            max: grafico.YaxixMax,
            min: grafico.YaxixMin
        },
        tooltip: {
            pointFormat: '<span style="color:{series.color}">{series.name}</span>: ({point.y:.1f})<br/>',
            split: true
        },
        plotOptions: {
            area: {
                stacking: 'normal',
                lineColor: '#ffffff',
                lineWidth: 0.1,
                marker: {
                    enabled: false
                }
            }
        },
        series: []
    };

    for (i = 0; i < grafico.Series.length; i++) {
        Varserie[i] = [];
        $.each(grafico.Series[i].Data, function (key, item) {
            var seriePoint = [];
            seriePoint.push(item.Y);
            Varserie[i].push(seriePoint);
        });
        opcion.series.push({
            name: grafico.Series[i].Name,
            data: Varserie[i],
            color: grafico.Series[i].Color,
        });
    }

    Highcharts.chart(idVista, opcion);
}

//validaciones de los filtros de busqueda
function validacionesxFiltro(valor) {
    var idEmpresa = getEmpresa();

    var arrayFiltro = arrayFiltro || [];

    if (valor == 1)
        arrayFiltro.push({ id: idEmpresa, mensaje: "Seleccione la opcion Empresa" });

    validarFiltros(arrayFiltro);
}

function getValorFiltroRF() {
    var listCheck = [];
    $("input[name='filtro_RF']:checked").each(function () {
        listCheck.push($(this).val());
    });

    return listCheck.toString();
}

/////////////////////////////////////////////////////
function cargarUnidadesRFria() {
    $.ajax({
        type: 'POST',
        url: controlador + 'ListarUnidadesRFria',
        data: {
            fecha: getFechaInicio()
        },
        success: function (evt) {
            
            $('#listaRFria').html(evt);
            $('#tablaRFria').dataTable({
                "iDisplayLength": 25
            });
            $('#hfFechaRFria').val(getFechaInicio());
            
        },
        error: function () {
            alert("Ha ocurrido un error");
        }
    });
}

function exportarRFria() {
    $.ajax({
        type: 'POST',
        url: controlador + "ExportarUnidadesRFria",
        data: {
            fecha: getFechaInicio()
        },
        dataType: 'json',
        cache: false,
        success: function (resultado) {
            if (resultado == 1) {
                location.href = controlador + "DescargarUnidadesRFria";
            }
            else {
                alert("Ha ocurrido un error");
            }
        },
        error: function () {
            alert("Ha ocurrido un error");
        }
    });
}

function editarRFria(id) {
    $.ajax({
        type: 'POST',
        url: controlador + 'EditarUnidadRFria',
        data: {
            id: id,
            fecha: (id == 0) ? getFechaInicio() : $('#hfFechaRFria').val()
        },
        success: function (evt) {
            $('#contenidoRFria').html(evt);
            setTimeout(function () {
                $('#popupRFria').bPopup({
                    autoClose: false,
                    modalClose: false,
                    escClose: false,
                    follow: [false, false]
                });
            }, 50);

            $('#btnGrabarRFria').on("click", function () {
                grabarRFria();
            });


            $('#btnCancelarRFria').on("click", function () {
                $('#popupRFria').bPopup().close();
            });

            $('#cbEmpresaRFria').val($('#hfEmpresaRFria').val());
            $('#cbCentralRFria').val($('#hfCentralRFria').val());
            $('#cbUnidadRFria').val($('#hfUnidadRFria').val());

            $('#cbEmpresaRFria').on('change', function () {
                cargarCentralesRFria();
            });

            $('#cbCentralRFria').on('change', function () {
                cargarGruposRFria();
            });

            $("#cbUnidadRFria").on('change', function () {
                $('#hfUnidadRFria').val($("#cbUnidadRFria").val());
            });

            $("#txtFechaRFria").attr("disabled", true);

            if (id != 0) {
                $("#cbEmpresaRFria").attr("disabled", true);
                $("#cbCentralRFria").attr("disabled", true);
                $("#cbUnidadRFria").attr("disabled", true);
            }

        },
        error: function () {
            alert("Ha ocurrido un error");
        }
    });
}

function cargarCentralesRFria() {
    $('#cbCentralRFria').get(0).options.length = 0;
    $('#cbCentralRFria').get(0).options[0] = new Option("-SELECCIONE-", "-1");
    $('#cbUnidadRFria').get(0).options.length = 0;
    $('#cbUnidadRFria').get(0).options[0] = new Option("-SELECCIONE-", "-1");

    if ($('#cbEmpresaRFria').val() != "-1") {
        $.ajax({
            type: 'POST',
            url: controlador + 'ObtenerCentralesRFria',
            data: {
                idEmpresa: $('#cbEmpresaRFria').val()
            },
            dataType: 'json',
            global: false,
            success: function (result) {

                $('#cbCentralRFria').get(0).options.length = 0;
                $('#cbCentralRFria').get(0).options[0] = new Option("-SELECCIONE-", "-1");
                $.each(result, function (i, item) {
                    $('#cbCentralRFria').get(0).options[$('#cbCentralRFria').get(0).options.length] = new Option(item.Gruponomb, item.Grupocodi);
                });
            },
            error: function () {
                alert("Ha ocurrido un error");
            }
        });
    }
    else {
        alert("Seleccione empresa.");
    }
}

function cargarGruposRFria() {
    $('#cbUnidadRFria').get(0).options.length = 0;
    $('#cbUnidadRFria').get(0).options[0] = new Option("-SELECCIONE-", "-1");

    if ($('#cbEmpresaRFria').val() != "-1" && $('#cbCentralRFria').val() != "-1") {
        $.ajax({
            type: 'POST',
            url: controlador + 'ObtenerUnidadesRFria',
            data: {
                idEmpresa: $('#cbEmpresaRFria').val(),
                central: $('#cbCentralRFria').val()
            },
            dataType: 'json',
            global: false,
            success: function (result) {
                $('#cbUnidadRFria').get(0).options.length = 0;
                $('#cbUnidadRFria').get(0).options[0] = new Option("-SELECCIONE-", "-1");
                $.each(result, function (i, item) {
                    $('#cbUnidadRFria').get(0).options[$('#cbUnidadRFria').get(0).options.length] = new Option(item.Gruponomb, item.Grupocodi);
                });
            },
            error: function () {
                alert("Ha ocurrido un error");
            }
        });
    }
    else {
        alert("Seleccione central.");
    }
}

function grabarRFria() {
    var validacion = validarRFria();

    if (validacion == "") {
        $.ajax({
            type: 'POST',
            url: controlador + 'GrabarUnidaRFria',
            data: $('#frmRegistroRFria').serialize(),
            dataType: 'json',
            global: false,
            success: function (result) {
                if (result == 1) {
                    $('#popupRFria').bPopup().close();
                    alert("Operación exitosa...!");
                    cargarUnidadesRFria();
                }
                else {
                    mostrarMensaje('mensajeReservaFria', 'error', 'Se ha producido un error.');
                }
            },
            error: function () {
                mostrarMensaje('mensajeReservaFria', 'error', 'Se ha producido un error.');
            }
        });
    }
    else {
        mostrarMensaje('mensajeReservaFria', 'alert', validacion);
    }
}

function validarRFria() {
    var html = "<ul>";
    var flag = true;

    if ($('#txtFechaRFria').val() == "") {
        html = html + "<li>Seleccione fecha</li>";
        flag = false;
    }

    if ($('#txtHoraInicioRFria').val() == "") {
        html = html + "<li>Ingrese hora inicio</li>";
        flag = false;
    }
    else {
        if (!validarHoraMinuto($('#txtHoraInicioRFria').val())) {
            html = html + "<li>La hora de inicio debe tener formato hh:mm</li>";
            flag = false;
        }
    }

    if ($('#txtHoraFinRFria').val() == "") {
        html = html + "<li>Ingrese hora fin</li>";
        flag = false;
    }
    else {
        if (!validarHoraMinuto($('#txtHoraFinRFria').val())) {
            html = html + "<li>La hora de inicio debe tener formato hh:mm</li>";
            flag = false;
        }
    }

    if ($('#txtHoraInicioRFria').val() != "" && $('#txtHoraFinRFria').val() != "") {
        if (validarHoraMinuto($('#txtHoraInicioRFria').val()) && validarHoraMinuto($('#txtHoraFinRFria').val())) {
            var timeIni = $('#txtHoraInicioRFria').val().split(':');
            var timeFin = $('#txtHoraFinRFria').val().split(':');

            if (parseInt(timeIni[0], 10) > parseInt(timeFin[0], 10)) {
                html = html + "<li>La hora de inicio debe ser menor a la hora final.</li>";
                flag = false;
            }
            else if (parseInt(timeIni[0], 10) == parseInt(timeFin[0], 10)) {
                if (parseInt(timeIni[1], 10) >= parseInt(timeFin[1], 10)) {
                    html = html + "<li>La hora de inicio debe ser menor a la hora final.</li>";
                    flag = false;
                }
            }
        }
    }


    if ($('#hfUnidadRFria').val() == "-1" && $('#hfUnidadRFria').val() != "") {
        html = html + "<li>Seleccione unidad</li>";
        flag = false;
    }

    html = html + "</ul>";

    if (flag) html = "";
    return html;
}

function eliminarRFria(id) {
    if (confirm('¿Está seguro de realizar esta operación?')) {

        $.ajax({
            type: 'POST',
            url: controlador + 'EliminarRFria',
            data: {
                id: id
            },
            dataType: 'json',
            success: function (result) {
                if (result == 1) {
                    alert("Operación exitosa...!");
                    cargarUnidadesRFria();
                }
                else {
                    alert("Ha ocurrido un error");
                }
            },
            error: function () {
                alert("Ha ocurrido un error");
            }
        });
    }
}

function mostrarMensaje(id, tipo, mensaje) {
    $('#' + id).removeClass();
    $('#' + id).addClass('action-' + tipo);
    $('#' + id).html(mensaje);
}

function validarHoraMinuto(hora) {
    if (!hora || hora.length < 1) { return false; }
    var time = hora.split(':');
    return (time.length === 2
        && parseInt(time[0], 10) >= 0
        && parseInt(time[0], 10) <= 23
        && parseInt(time[1], 10) >= 0
        && parseInt(time[1], 10) <= 59)
}