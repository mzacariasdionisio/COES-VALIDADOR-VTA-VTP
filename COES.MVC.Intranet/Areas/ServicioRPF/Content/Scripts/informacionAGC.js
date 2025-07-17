var controlador = siteRoot + 'Serviciorpf/InformacionAGC/';

const reporteExtranet = 1;
const reporteKumpliy = 2;
var datosFiltro;

$(function () {

    $('#txtFecha').Zebra_DatePicker({
        direction: -1,
        
        onSelect: function () {
            cargarEmpresas();

        },
    });

    $('#txtFecExpExtranet').Zebra_DatePicker({
        direction: -1,        
    });

    $('#txtFecExpK').Zebra_DatePicker({
        direction: -1,        
    });

    $('#cbEmpresa').on('change', function () {
        cargarListadoUrs();
    });

    $('#cbUrs').on('change', function () {
        cargarEquipos();
    });
    

    $('#btnConfiguracion').on('click', function () {
        document.location.href = controlador + 'equivalencia';
    });

    $('#btnConsultar').on('click', function () {
        compararInfoES();
    });

    $('#btnExportar').on('click', function () {
        descargarTabla();
    });

    $("#btnExportarExtranet").click(function () {
        resetearPopup(reporteExtranet);
        abrirPopup("popupExportarExtranet");
    });

    $("#btnDescargarExtranet").click(function () {
        descargarExtranet();
    });

    $("#btnExportarKumpliy").click(function () {
        resetearPopup(reporteKumpliy);
        abrirPopup("popupExportarKumpliy");
    });

    $("#btnDescargarKumpliy").click(function () {        
        descargarKumpliy();
    });
    
});


function compararInfoES() {
    limpiarBarraMensaje("mensaje");
    var datos = obtenerDatosConsulta();
    datosFiltro = datos;

    var msg = validarDatosConsulta(datos);

    if (msg == "") {
        $.ajax({
            type: 'POST',
            url: controlador + 'CompararInformacion',
            data: {
                fecha: datos.strFecha,
                idEmpresa: datos.idEmpresa,
                idUrs: datos.idUrs,
                idEquipo: datos.idEquipo,
                senial: datos.idSenial,
                resolucion: datos.idResolucion
            },
            dataType: 'json',
            success: function (evt) {
                if (evt.Resultado == "1") {
                    
                    $("#listado").html(evt.HtmlListado);
                    refrehDatatable();

                    GraficoLineaH(evt.Grafico, "grafico");

                    $("#btnExportar").css("display", "block");
                    
                } else {
                    mostrarMensaje('mensaje', 'error', 'Ha ocurrido un error.' + evt.Mensaje);

                }
            },
            error: function () {
                mostrarMensaje('mensaje', 'error', 'Ha ocurrido un error.' + evt.Mensaje);
            }
        });
    } else {
        mostrarMensaje('mensaje', 'alert', msg);
        $("#listado").html($("#hfListadoDefecto").val());
        refrehDatatable();
        $("#grafico").html("");
        $("#btnExportar").css("display", "none");
    }
}

function refrehDatatable() {
    $('#tablaComparativa').dataTable({
        "filter": false,
        "ordering": false,
        "scrollCollapse": true,
        "paging": false,
        "scrollY": 470,
    });
}

function cargarEmpresas() {
    var fechaConsulta = $("#txtFecha").val();

    if (fechaConsulta == "") {
        setearEmpresaPorDefecto();
        setearURSPorDefecto();
        setearEquipoPorDefecto();
    } else {
        $.ajax({
            type: 'POST',
            url: controlador + 'ObtenerListadoEmpresas',
            data: {
                fecha: fechaConsulta
            },
            dataType: 'json',
            success: function (result) {
                $("#cbEmpresa").empty();
                $('#cbEmpresa').append($('<option></option>').val("-1").html("-- Seleccione Empresa --"));
                for (var item in result) {
                    $('#cbEmpresa').append($('<option></option>').val(result[item].Emprcodi).html(result[item].Emprnomb));
                }
                setearURSPorDefecto();
                setearEquipoPorDefecto();
            },
            error: function () {
                mostrarMensaje('mensaje', 'error', 'Ha ocurrido un error.');
            }
        });
    }
}

function cargarListadoUrs() {    
    var idEmpresa = $("#cbEmpresa").val();
    var fechaConsulta = $("#txtFecha").val();

    if (idEmpresa == "-1") {
        setearURSPorDefecto();
        setearEquipoPorDefecto();
    } else {
        $.ajax({
            type: 'POST',
            url: controlador + 'ObtenerListadoUrs',
            data: {
                emprcodi: idEmpresa,
                fecha: fechaConsulta
            },
            dataType: 'json',
            success: function (result) {
                $("#cbUrs").empty();
                $('#cbUrs').append($('<option></option>').val("-1").html("-- Seleccione URS --"));
                for (var item in result) {
                    $('#cbUrs').append($('<option></option>').val(result[item].Grupocodi).html(result[item].Ursnomb));
                }
            },
            error: function () {
                mostrarMensaje('mensaje', 'error', 'Ha ocurrido un error.' + result.Mensaje);
            }
        });
    }    
}

function cargarEquipos() {
    var fechaConsulta = $("#txtFecha").val();
    var idUrs = $("#cbUrs").val();

    if (idUrs == "-1") {        
        setearEquipoPorDefecto();
    } else {
        $.ajax({
            type: 'POST',
            url: controlador + 'ObtenerListadoEquipos',
            data: {
                idUrs: idUrs,
                fecha: fechaConsulta
            },
            dataType: 'json',
            success: function (result) {
                $("#cbEquipo").empty();
                $('#cbEquipo').append($('<option></option>').val("-1").html("-- Seleccione Equipo --"));
                for (var item in result) {
                    $('#cbEquipo').append($('<option></option>').val(result[item].Equicodi).html(result[item].Ursnomb));
                }
            },
            error: function () {
                mostrarMensaje('mensaje', 'error', 'Ha ocurrido un error.' + result.Mensaje);
            }
        });
    }
}

function setearEmpresaPorDefecto() {
    $("#cbEmpresa").empty();
    $('#cbEmpresa').append($('<option></option>').val("-1").html("-- Seleccione Empresa --"));
}

function setearURSPorDefecto() {
    $("#cbUrs").empty();
    $('#cbUrs').append($('<option></option>').val("-1").html("-- Seleccione URS --"));
}

function setearEquipoPorDefecto() {
    $("#cbEquipo").empty();
    $('#cbEquipo').append($('<option></option>').val("-1").html("-- Seleccione Equipo --"));
}

function descargarTabla() {
    limpiarBarraMensaje("mensaje");
        
    $.ajax({
        type: 'POST',
        url: controlador + 'exportarTabla',
        data: {            
            fecha: datosFiltro.strFecha,
            idEmpresa: datosFiltro.idEmpresa,
            idUrs: datosFiltro.idUrs,
            idEquipo: datosFiltro.idEquipo,
            senial: datosFiltro.idSenial,
            resolucion: datosFiltro.idResolucion
            
        },
        dataType: 'json',
        success: function (evt) {
            
            if (evt.Resultado != "-1") {

                window.location = controlador + "Exportar?file_name=" + evt.Resultado;
            } else {
                mostrarMensaje('mensaje', 'error', 'Ha ocurrido un error.' + evt.Mensaje);
            }
        },
        error: function (err) {
           
            mostrarMensaje('mensaje', 'error', 'Ha ocurrido un error.');
        }
    });
    
}


function descargarExtranet() {
    limpiarBarraMensaje("mensaje_popupEE");
    limpiarBarraMensaje("mensaje");

    var datos = obtenerDatosPopup(reporteExtranet);
    var msg = validarExportacion(datos);

    if (msg == "") {
        var lstUrs = datos.lstSeleccionados.join(',');
        $.ajax({
            type: 'POST',
            url: controlador + 'exportarExtranet',
            data: {
                fecha: datos.fecha,
                strIdUrs: lstUrs
            },
            dataType: 'json',
            success: function (evt) {
                
                if (evt.Resultado != "-1") {
                    cerrarPopup('#popupExportarExtranet');
                    window.location = controlador + "Exportar?file_name=" + evt.Resultado;
                    if (evt.LstUrsSinPtoMedicion != "")
                        mostrarMensaje('mensaje', 'alert', 'Las siguientes urs no tienen puntos de medición: ' + evt.LstUrsSinPtoMedicion);
                    
                } else {
                    mostrarMensaje('mensaje_popupEE', 'error', 'Ha ocurrido un error.' + evt.Mensaje);
                }
            },
            error: function (err) {
                
                mostrarMensaje('mensaje_popupEE', 'error', 'Ha ocurrido un error.');
            }
        });
    } else {
        
        mostrarMensaje('mensaje_popupEE', 'error', msg);
    }
}


function validarExportacion(datos) {
    var msj = "";      

    if (datos.fecha == "") {
        msj += "<p>Debe escoger una fecha correcta.</p>";
    }

    if (datos.lstSeleccionados.length == 0) {
        msj += "<p>Debe escoger almenos una URS.</p>";
    }
    
    return msj;
}

function descargarKumpliy() {
    limpiarBarraMensaje("mensaje_popupEK");
    limpiarBarraMensaje("mensaje");

    var datos = obtenerDatosPopup(reporteKumpliy);
    var msg = validarExportacion(datos);

    if (msg == "") {
        var lstUrs = datos.lstSeleccionados.join(',');
        $.ajax({
            type: 'POST',
            url: controlador + 'exportarKumpliy',
            data: {
                fecha: datos.fecha,
                strIdUrs: lstUrs
            },
            dataType: 'json',
            success: function (evt) {
                
                if (evt.Resultado != "-1") {
                    cerrarPopup('#popupExportarKumpliy');
                    window.location = controlador + "Exportar?file_name=" + evt.Resultado;
                    if (evt.LstUrsSinPtoMedicion != "")
                        mostrarMensaje('mensaje', 'alert', 'Las siguientes urs no tienen puntos de medición: ' + evt.LstUrsSinPtoMedicion);
                } else {
                    mostrarMensaje('mensaje_popupEK', 'error', 'Error: ' + evt.Mensaje);
                }
            },
            error: function (err) {
                
                mostrarMensaje('mensaje_popupEK', 'error', 'Ha ocurrido un error.');
            }
        });
    } else {
        
        mostrarMensaje('mensaje_popupEK', 'error', msg);
    }
}

function obtenerDatosPopup(tipoReporte) {
    var filtro = {};
    let valoresCheck = [];

    if (tipoReporte == reporteExtranet) {

        checkboxes = document.getElementsByName("grupoursee");
        for (var i = 0, n = checkboxes.length; i < n; i++) {
            if(checkboxes[i].checked) 
                valoresCheck.push(checkboxes[i].value);
        }

        filtro.fecha = $("#txtFecExpExtranet").val();
        filtro.lstSeleccionados = valoresCheck;
    }

    if (tipoReporte == reporteKumpliy) {

        checkboxes = document.getElementsByName("grupoursek");
        for (var i = 0, n = checkboxes.length; i < n; i++) {
            if (checkboxes[i].checked)
                valoresCheck.push(checkboxes[i].value);
        }

        filtro.fecha = $("#txtFecExpK").val();
        filtro.lstSeleccionados = valoresCheck;
    }

    return filtro;
}


function seleccionarTodos(source, name) {
    checkboxes = document.getElementsByName(name);
    for (var i = 0, n = checkboxes.length; i < n; i++) {
        checkboxes[i].checked = source.checked;
    }
}

function verificarChecks(source, nameTodo, nameGrupo) {
    checkboxes = document.getElementsByName(nameGrupo);
    var total = checkboxes.length;
    var suma = 0;
    for (var i = 0, n = total; i < n; i++) {
        if (checkboxes[i].checked) {
            suma++;
        }
    }

    checkboxes = document.getElementsByName(nameTodo);
    if (suma == total) {
        
        checkboxes[0].checked = source.checked;
    } else {
        checkboxes[0].checked = false;
    }
}

function resetearPopup(tipoReporte) {

    if (tipoReporte == reporteExtranet) {
        limpiarBarraMensaje("mensaje_popupEE");
        $("#txtFecExpExtranet").val($("#hfFecExpExtranet").val());
        checkboxesTodo = document.getElementsByName("allursee");
        checkboxesTodo[0].checked = true;
        checkboxesGrupo = document.getElementsByName("grupoursee");
        for (var i = 0, n = checkboxesGrupo.length; i < n; i++) {
            checkboxesGrupo[i].checked = true;
        }
    }

    if (tipoReporte == reporteKumpliy) {
        limpiarBarraMensaje("mensaje_popupEK");
        $("#txtFecExpK").val($("#hfFecExpK").val());
        checkboxesTodo = document.getElementsByName("allursek");
        checkboxesTodo[0].checked = true;
        checkboxesGrupo = document.getElementsByName("grupoursek");
        for (var i = 0, n = checkboxesGrupo.length; i < n; i++) {
            checkboxesGrupo[i].checked = true;
        }
    }    
}


function cerrarPopup(id) {
    $(id).bPopup().close()
}

function abrirPopup(contentPopup) {
    setTimeout(function () {
        $("#" + contentPopup).bPopup({
            easing: 'easeOutBack',
            speed: 450,
            transition: 'slideDown',
            modalClose: false
        });
    }, 50);
}

function obtenerDatosConsulta() {
    var filtro = {};

    filtro.strFecha = $("#txtFecha").val();
    filtro.idEmpresa = $("#cbEmpresa").val();
    filtro.idUrs = $("#cbUrs").val();
    filtro.idEquipo = $("#cbEquipo").val();
    filtro.idSenial = $("#cbSenial").val();
    filtro.idResolucion = $("#cbResolucion").val();

    return filtro;
}

function validarDatosConsulta(datos) {
    var msj = "";  

    if (datos.strFecha == "") {
        msj += "<p>Debe escoger una fecha correcta.</p>";
    }

    if (datos.idEmpresa == "-1") {
        msj += "<p>Debe escoger una empresa correcta.</p>";
    }

    if (datos.idUrs == "-1") {
        msj += "<p>Debe escoger una URS correcta.</p>";
    }

    if (datos.idEquipo == "-1") {
        msj += "<p>Debe escoger un equipo correcto.</p>";
    }

    if (datos.idSenial == "") {
        msj += "<p>Debe escoger una senial correcta.</p>";
    }

    if (datos.idResolucion == "") {
        msj += "<p>Debe escoger una resolución correcta.</p>";
    }

    return msj;
}

function GraficoLineaH(data, content) {

    var series = [];

    for (var d in data.SerieData) {
        var item = data.SerieData[d];
        if (item === null) {
            continue;
        }
        series.push({ name: item.Name, data: item.Data, type: item.Type, color: item.Color });
    }

    Highcharts.chart(content, {
        chart: {
            type: 'line',
            shadow: true
        },
        title: {
            text: data.TitleText
        },
        tooltip: {
            valueDecimals: 2,
            shared: true,
            valueSuffix: ' ' + data.YaxixLabelsFormat
        },
        xAxis: {
            categories: data.XAxisCategories,
            crosshair: true,
            title: {
                text: data.XAxisTitle
            }
        },
        yAxis: {
            title: {
                text: data.YAxixTitle
            },
            labels: {
                format: '{value} ' + data.YaxixLabelsFormat
            },
            max: data.YaxixMax
        },
        plotOptions: {
            line: {
                dataLabels: {
                    enabled: false
                },
                marker: {
                    enabled: true
                },
                enableMouseTracking: true
            }
        },
        subtitle: {
            text: data.Subtitle,
            floating: false,            
        },
        legend: {
            layout: data.LegendLayout,
            align: data.LegendAlign,
            verticalAlign: data.LegendVerticalAlign
        },
        series: series,
        responsive: {
            rules: [{
                condition: {
                    maxWidth: 500
                },
                chartOptions: {
                    legend: {
                        layout: 'horizontal',
                        align: 'center',
                        verticalAlign: 'bottom'
                    }
                }
            }]
        }
    });
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