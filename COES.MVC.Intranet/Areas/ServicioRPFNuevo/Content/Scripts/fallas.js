var controlador = siteRoot + 'serviciorpfNuevo/analisisfalla/'
var plot = null;

$(function () {

    $('#tab-container').easytabs({
        animate: false
    });

    $('#txtFechaProceso').Zebra_DatePicker({

    });    

    $('#btnCondiciones').click(function () {
        verificarCondiciones();
    });

    $('#cbUnidad').multipleSelect({
        width: '350px',
        filter: true
    });

    cargarParametros();
});


verificarCondiciones = function ()
{
    var unidades = $('#cbUnidad').multipleSelect('getSelects');
    var hora = $('#txtHoraFalla').val();
    var fecha = $('#txtFechaProceso').val();
    $('#hfUnidad').val(unidades);    
    var unidad = $('#hfUnidad').val();
    var reserva = $('#txtReservaPrimaria').val();

    if (fecha != "" && hora != "" && unidad != "" && reserva!="") {
        
        if (validarHora(hora)) {

            $.ajax({
                type: "POST",
                url: controlador + "validacion",
                data: {
                    fecha: fecha, hora: hora, unidades: unidad, reserva: reserva
                },
                success: function (evt) {
                    $('#cntResultadoValidacion').html(evt);
                },
                error: function () {
                    mostrarError();
                }
            });
        }
        else {
            mostrarMensaje("Ingrese hora válida.");
        }
    }
    else
    {
        mostrarMensaje("Ingrese fecha y hora, seleccione la unidad o unidades e ingrese la reserva primaria.");
    }
}

reValidar = function ()
{
    var potencia = $('#txtPotenciaDesconectada').val();
    var reserva = $('#txtReservaPrimaria').val();

    if (potencia != "" && reserva != "") {        
        $.ajax({
            type: "POST",
            url: controlador + "revalidacion",
            data: {
                potencia: potencia, reserva:reserva
            },
            success: function (evt) {
                $('#cntVerificacion').html(evt);
            },
            error: function () {
                mostrarError();
            }
        });
    }
    else
    {
        if (potencia == "" && reserva == "") {
            mostrarMensaje("Ingrese la potencia desconectada y la reserva primaria.");
        }
        else
        {
            if (potencia == "") { mostrarMensaje("Ingrese la potencia desconectada."); }
            if (reserva == "") { mostrarMensaje("Ingrese la reserva primaria."); }
        }
    }
}

evaluar = function () {
    var segundos = "0";
    var flag = "S";
    if ($('#txtSegundos').length > 0) {
        segundos = $('#txtSegundos').val();
        if (segundos == "") flag = "N";
    }

    if (flag == "S") {
        $.ajax({
            type: "POST",
            url: controlador + "evaluacion",
            data: {
                segundos: segundos
            },
            success: function (evt) {
                $('#evaluacionUnidad').html(evt);
                $('#tab-container').easytabs('select', '#paso2');
                cargarPotencia();
            },
            error: function () {
                mostrarError();
            }
        });
    }
    else {
        mostrarMensaje("Ingrese la cantidad de segundos a evaluar.");
    }
}

cargarPotencia = function() {
   $.ajax({
    type: "POST",
        url: controlador + "cargapotencia",
                success: function (evt) {
                    $('#vistaPotencia').html(evt);
        },
            error: function () {
            mostrarError();
        }
    });
}

procesarArchivo = function () {
    $.ajax({
        type: 'POST',
        url: controlador + 'datospotencia',
        dataType: 'json',

        cache: false,
        success: function (resultado) {
            if (resultado == 1) {
                cargarPotencia();
            }
            else
                mostrarError();
        },
        error: function () {
            mostrarError();
        }
    });
}


cargarGrafico = function (id) {
    $.ajax({
        type: 'POST',
        url: controlador + 'grafico',
        dataType: 'json',
        data: {
            ptoMediCodi: id
        },
        cache: false,
        success: function (resultado) {
            $('#cntGrafico').html("");
            if (resultado.Indicador == 1) {
                plotear(resultado);

                $('#resultado').removeClass("action-exito");
                $('#resultado').removeClass("action-error");

                if (resultado.IndCumplimiento == "S") {
                    $('#resultado').html("Cumplió satisfactoriamente la evaluación con " + resultado.PorcentajeCumplimiento + "%");
                    $('#resultado').addClass("action-exito");
                }
                else {
                    $('#resultado').html("No cumplió satisfactoriamente la evaluación con " + resultado.PorcentajeCumplimiento + "%");
                    $('#resultado').addClass("action-error");
                }
            }
            if (resultado.Indicador == 2) {
                mostrarMensaje('No existen datos');                
            }
            if (resultado.Indicador == -1) {
                mostrarError();
            }
            if (resultado.Indicador == -2) {
                mostrarMensaje("No existen datos de la unidad seleccionada.");
            }
            if (resultado.Indicador == -3) {
                mostrarMensaje("No existen datos.");
            }
            if (resultado.Indicador == -4) {
                mostrarMensaje("Existen menos de 30 datos.");
            }
            if (resultado.Indicador == -5) {
                mostrarMensaje("No se han cargado las potencias máximas.");
            }
            if (resultado.Indicador == -6) {
                mostrarMensaje("No existe potencia máxima de la unidad seleccionada.");
            }
            if (resultado.Indicador == -7)            {
                mostrarMensaje("La frecuencia del segundo inicial es cero.");
            }
        },
        error: function () {
            mostrarError();
        }
    });
}


plotear = function (resultado) {
    var regresion = $.parseJSON(resultado.SerieRegresion);

    if (plot) {
        plot.destroy();
    }

    plot = $.jqplot('cntGrafico', regresion, {
        title: 'Evaluación RPF ante Fallas',
        seriesDefaults: {
            rendererOptions: {
                smooth: true
            },
        },
        series: [
            {
                showMarker: false,
                color: '#C6D9F1',
                negativeColor: '#C6D9F1',
                showMarker: false,
                showLine: true,
                fill: true,                
                lineWidth:1
            },
            { showMarker: false, color: '#FF4040', lineWidth: 1 },
            { showMarker: false, yaxis: 'y2axis', color: '#905EB5', lineWidth: 1 },
            { showMarker: false, yaxis: 'y2axis', color: '#33CC00', lineWidth: 1 }
        ],
        axes: {
            xaxis:
            {
                label: 'Segundos'
            },
            yaxis: {
                tickOptions: {
                    formatString: '%.3f'
                },  
                label: 'Potencia (MW)',
                labelRenderer: $.jqplot.CanvasAxisLabelRenderer,
                min: 0 
            },
            y2axis: {
                rendererOptions: {
                    alignTicks: true
                },
                tickOptions: {
                    formatString: '%.3f'
                },
                label: 'Frecuencia (HZ)',
                labelRenderer: $.jqplot.CanvasAxisLabelRenderer
            }
        },
        grid: {
            backgroundColor: '#ffffff',
            drawGridlines: true,
            gridLineColor: "#dddddd",
            gridLineWidth: 0.8,
            borderColor: "#dddddd",
            borderWidth: 0.8,
            shadow: false
        },
        cursor: {
            zoom: true,
            show: true
        }
    });
}


mostrarReporte = function () {
    $.ajax({
        type: "POST",
        url: controlador + "reporte",
        success: function (evt) {
            $('#reporte').html(evt);
            $('#tab-container').easytabs('select', '#paso3');
        },
        error: function () {
            mostrarError();
        }
    });
}

exportar = function () {
    $.ajax({
        type: 'POST',
        url: controlador + 'generarreporte',
        dataType: 'json',
        success: function (result) {
            if (result == 1) {
                window.location = controlador + 'exportar';
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

generarWord = function () {
    $.ajax({
        type: 'POST',
        url: controlador + 'generarword',
        dataType: 'json',
        data: {
            fecha: $('#txtFechaProceso').val()
        },
        success: function (result) {
            if (result == 1) {
                window.location = controlador + 'exportarword';
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


cargarParametros = function () {
    $.ajax({
        type: "POST",
        url: controlador + "configuracion",
        success: function (evt) {
            $('#parametros').html(evt);
        },
        error: function () {
            mostrarError();
        }
    });
}

grabarParametro = function () {

    if ($('#txtEstatismo').val() == "" || $('#txtRPrimaria').val() == "" ||
        $('#txtFrecNominal').val() == "" || $('#txtPorCumplimiento').val() == "" ||
        $('#txtFrecUmbral').val() == "" || $('#txtSegundos').val() == "" || $('#txtSegFrecFalla').val() == "")
    {
        mostrarMensaje("Debe ingresar todo los parámetros.");
    }
    else
    {
        if (parseInt($('#txtSegFrecFalla').val()) <= 10) {

            $.ajax({
                type: 'POST',
                url: controlador + 'grabarparametro',
                data: {
                    estatismo: $('#txtEstatismo').val(), rPrimaria: $('#txtRPrimaria').val(),
                    frecNominal: $('#txtFrecNominal').val(), porCumplimiento: $('#txtPorCumplimiento').val(),
                    frecUmbral: $('#txtFrecUmbral').val(), segundos: $('#txtSegundos').val(), segFalla: $('#txtSegFrecFalla').val()
                },
                dataType: 'json',
                cache: false,
                success: function (result) {
                    if (result == 1) {
                        mostrarExito("Los parámetros fueron modificados correctamente.");
                    }
                    else {
                        mostrarError();
                    }
                },
                error: function () {
                    mostrarError();
                }
            });
        }
        else {
            mostrarMensaje("Los segundos para la frecuencia de falla no debe superar a 10");
        }
    }
}

mostrarError = function () {
    alert("Ha ocurrido un error.");
}

mostrarMensaje = function (mensaje) {
    alert(mensaje);
}

mostrarExito = function (mensaje) {
    alert(mensaje);
}

validarNumero = function (item, evt) {
    evt = (evt) ? evt : window.event;
    var charCode = (evt.which) ? evt.which : evt.keyCode;

    if (charCode == 46) {
        var regex = new RegExp(/\./g)
        var count = $(item).val().match(regex).length;
        if (count > 1) {
            return false;
        }
    }

    if (charCode > 31 && (charCode < 48 || charCode > 57)) {
        return false;
    }

    return true;
}


validarHora = function (inputStr) {

    if (!inputStr || inputStr.length < 1) { return false; }
    var time = inputStr.split(':');
    return (time.length === 2
           && parseInt(time[0], 10) >= 0
           && parseInt(time[0], 10) <= 23
           && parseInt(time[1], 10) >= 0
           && parseInt(time[1], 10) <= 59) ||
           (time.length === 3
           && parseInt(time[0], 10) >= 0
           && parseInt(time[0], 10) <= 23
           && parseInt(time[1], 10) >= 0
           && parseInt(time[1], 10) <= 59
           && parseInt(time[2], 10) >= 0
           && parseInt(time[2], 10) <= 59)
}