var controlador = siteRoot + 'serviciorpfNuevo/analisisnormal/'
var plot = null;

$(function () {

    $('#tab-container').easytabs({
        animate:false
    });

    $('#tab-container-principal').easytabs({
        animate: false
    });
   
    $('#FechaProceso').Zebra_DatePicker({

    });

    $('#btnObtenerRango').click(function () {
        obtenerRango();
    });

    $('#btnNuevoProceso').click(function () {
        cancelarAnalisis();
    });

    cargarParametros();

    cargarPotencia();
});

cargarParametros = function ()
{
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

obtenerRango = function ()
{
    $.ajax({
        type: 'POST',
        url: controlador + 'verificararchivopotencia',
        dataType: 'json',
        cache: false,
        success: function (resultado) {
            if (resultado == 1) {                
                $.ajax({
                    type: "POST",
                    url: controlador + "seleccionrango",
                    data: { fecha: $('#FechaProceso').val() },
                    success: function (evt) {
                        $('#seleccionRango').html(evt);
                    },
                    error: function () {
                        mostrarError();
                    }
                });
            }
            if (resultado == -1) mostrarError();
            if (resultado == 0) mostrarMensaje("Cargue el archivo de potencias.");
        },
        error: function () {
            mostrarError();
        }
    });
}

evaluarMuestra = function ()
{
    $.ajax({
        type: "POST",
        url: controlador + "seleccionunidad",        
        success: function (evt) {
            $('#seleccionUnidad').html(evt);
            $('#tab-container').easytabs('select', '#paso2');
            cargarPotencia();
        },
        error: function () {
            mostrarError();
        }
    });    
}

mostrarExcluidos = function (id)
{
    $.ajax({
        type: "POST",
        url: controlador + "excluidos",
        data: { ptoMediCodi: id },
        success: function (evt) {
            $('#contentNoIncluidos').html(evt);

            setTimeout(function () {
                $('#popupUnidad').bPopup({
                    easing: 'easeOutBack',
                    speed: 450,
                    transition: 'slideDown'
                });
            }, 50);
            
        },
        error: function () {
            mostrarError();
        }
    });
}

cancelarAnalisis = function ()
{
    document.location.href = siteRoot + 'serviciorpf/analisisnormal/index';
}

cargarPotencia = function ()
{
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

procesarArchivo = function ()
{
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

cargarGrafico = function (id)
{   
    $.ajax({
        type: 'POST',
        url: controlador + 'grafico',
        dataType: 'json',
        data: {
            ptoMediCodi :id
        },
        cache: false,
        success: function (resultado) {
            if (resultado.Indicador == 1)
            {               
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
            if (resultado.Indicador == 2)
            {
                mostrarMensaje('No existen datos');
            }
            if (resultado.Indicador == -1)
            {
                mostrarError();
            }
        },
        error: function () {
            mostrarError();
        }
    });
}

plotear = function (resultado)
{
    var regresion = $.parseJSON(resultado.SerieRegresion);  

    if (plot) {
        plot.destroy();
    }

    plot = $.jqplot('cntGrafico', regresion, {
        title: 'Evaluación RPF en estado normal',
        seriesDefaults: {
            rendererOptions: {
                smooth: true
            },
        },
        series: [
            { showMarker: false },
            { linePattern: 'dashed', showMarker: false },
            { linePattern: 'dashed', showMarker: false },
            { showMarker: true, showLine: false, markerOptions: { style: "dimaond", size: 4 } }
        ],
        grid: {
            backgroundColor: '#ffffff',
            drawGridlines: true,
            gridLineColor: "#dddddd",
            gridLineWidth: 0.8,
            borderColor: "#dddddd",
            borderWidth: 0.8,
            shadow: false
        }
    });
}

mostrarReporte = function ()
{
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

exportar = function ()
{
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

generarWord = function ()
{
    $.ajax({
        type: 'POST',
        url: controlador + 'generarword',
        data: { fecha: $('#FechaProceso').val() },
        dataType: 'json',
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

grabarParametro = function ()
{
    if ($('#txtEstatismo').val() == "" ||
        $('#txtRPrimaria').val() == "" ||
        $('#txtFrecNominal').val() == "" ||
        $('#txtPorCumplimiento').val() == "" ||
        $('#txtVarPotencia').val() == "" ||
        $('#txtVarFrecuencia').val() == "" ||
        $('#txtNroIntentos').val() == "" ||
        $('#txtSinRango').val() == "" ||
        $('#txtBanda').val() == "" ||
        $('#txtNroDatos').val() == "")
    {
        mostrarMensaje("Debe ingresar todo los parámetros.");
    }
    else {
        $.ajax({
            type: 'POST',
            url: controlador + 'grabarparametro',
            data: {
                estatismo: $('#txtEstatismo').val(),
                rPrimaria: $('#txtRPrimaria').val(),
                frecNominal: $('#txtFrecNominal').val(),
                porCumplimiento: $('#txtPorCumplimiento').val(),
                varPotencia: $('#txtVarPotencia').val(),
                varFrecuencia: $('#txtVarFrecuencia').val(),
                intentos: $('#txtNroIntentos').val(),
                cantidad: $('#txtSinRango').val(),
                banda: $('#txtBanda').val(),
                nroDatos: $('#txtNroDatos').val(),
                rPrimariaNew: $('#txtRPrimariaNew').val(),
                vigencia: $('#txtFechaVigencia').val()
            },
            dataType: 'json',
            cache: false,
            success: function (result) {
                if (result == 1) {
                    mostrarExito("Los parámetros fueron modificados correctamente.");
                }
            },
            error: function () {
                mostrarError();
            }
        });
    }
}

mostrarHistorico = function()
{
    $.ajax({
        type: 'POST',
        url: controlador + 'historico',
        cache: false,
        success: function (evt) {
            $('#contenidoHistorico').html(evt);
            setTimeout(function () {
                $('#popupHistorico').bPopup({                    
                });
            }, 50);
        },
        error: function () {
            mostrarError();
        }
    });   
}

mostrarError = function ()
{
    alert("Ha ocurrido un error.");
}

mostrarMensaje = function (mensaje)
{
    alert(mensaje);
}

mostrarExito = function (mensaje)
{
    alert(mensaje);
}

validarNumero = function (item, evt)
{
    evt = (evt) ? evt : window.event;
    var charCode = (evt.which) ? evt.which : evt.keyCode;

    if (charCode == 46)
    {
        var regex = new RegExp(/\./g)
        var count = $(item).val().match(regex).length;
        if (count > 1)
        {
            return false;
        }
    }

    if (charCode > 31 && (charCode < 48 || charCode > 57)) {
        return false;
    }
    
    return true;
} 