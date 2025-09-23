const controlador = siteRoot + 'ValidadorVteaAnalisis/';
let graficoBarrasRol = null;
let diaSeleccionado = 0;
let periodoSeleccionado = '';
let versionSeleccionado = '';

$(function () {
  

    $('#btnDescargaEnergiaDia').on('click', function () {
        descargarReporteEnergiaDia();
    });  

    $('#btnRegresar').on('click', function () {
        regresar();
    });  
  
});

function tituloDia(dia) {
    document.getElementById('divTituloDia').textContent = `Energia & CMg - Dia ${dia}`;
}
function mostrarMensaje(id, tipo, mensaje) {
    $("#" + id).css("display", "block");
    $('#' + id).removeClass();
    $('#' + id).addClass('action-' + tipo);
    $('#' + id).html(mensaje);

}

function limpiarMensaje(id) {
    $('#' + id).css("display", "none");
    $('#' + id).removeClass();
    $('#' + id).addClass('action-message');
    $('#' + id).html('');
}


function verGraficoEmpresa(codigo, empresa, cliente, barra, dia, periodo, version) {
   
    $.ajax({
        type: 'POST',
        url: controlador + 'ObtenerEmpresaEnergiaDia',
        data: {
            codigo: codigo,
            empresa: empresa,
            cliente: cliente,
            barra: barra,
            dia: dia,
            periodo: periodo,
            version: version
        },
        dataType: 'json',
        global: false,
        success: function (result) {

            diaSeleccionado = 0;

            if (result.StrMensajeError == '') {
                diaSeleccionado = dia;
                periodoSeleccionado = periodo;
                versionSeleccionado = version;

                document.getElementById('divTituloDia').textContent = `Energía & CMg - Día ${diaSeleccionado}`;

                generarGraficoBarras(result);
            }
            else {
                alert(result.StrMensajeError);
            }
        },
        error: function () {
            alert("Ha ocurrido un error interno no previsto en el sistema. Por favor comunique al Administrador del sistema.");
        }
    });
}

function generarGraficoBarras(model) {
    // Obtener los datos de la tabla 
    let datosEnergia = model.DetalleEmpresaEnergiaDia.tmp;
       
    if (graficoBarrasRol) {
        graficoBarrasRol.destroy();
    }
    debugger;   

    // Crear datos para el gráfico
    var dataCMg = datosEnergia.map(d => [convertirHoraAHoraCompleta(d.hora), parseFloat(d.cmg)]);
    var dataEnergia = datosEnergia.map(d => [convertirHoraAHoraCompleta(d.hora), parseFloat(d.mwh)]);

    // Crear el gráfico con navegador de tiempo
    graficoBarrasRol = Highcharts.stockChart('AreaGrafico', {
        chart: {
            zoomType: 'x'
        },
        title: {
            text: null
        },
        subtitle: {
            useHTML: true,
            text: '<span style="color:#FF0000;font-weight:bold;">CMg ($/MWh)</span> &nbsp;&nbsp;&nbsp; <span style="color:#0077FF;font-weight:bold;">Energía (MWh)</span>',
            align: 'center'
        },
        rangeSelector: {
            inputEnabled: false, 
            buttons: [
                {
                    type: 'all',
                    text: 'Todos'
                }
            ],
            selected: 0,
            buttonTheme: {
                width: 60
            }
        },
        navigator: {
            enabled: true
        },
        scrollbar: {
            enabled: true
        },
        xAxis: {
            type: 'datetime',
            title: {
                text: 'Hora del Día'
            },
            labels: {
                format: '{value:%H:%M}', // Mostrar solo hora:minuto
                rotation: -45,
                formatter: function () {
                    const hora = Highcharts.dateFormat('%H:%M', this.value);
                    const esMedianocheSiguienteDia =
                        this.value === Date.UTC(2020, 0, 2, 0, 0, 0);
                    return esMedianocheSiguienteDia ? '24:00' : hora;
                }
            },
            tickInterval: 1000 * 60 * 45          
        },
        yAxis: [{
            title: {
                text: 'CMg ($/MWh)'
            },
            labels: {
                format: '{value:.2f}'
            },
            opposite: false 
        }, {
            title: {
                text: 'Energía (MWh)'
            },
            opposite: true,
            labels: {
                format: '{value:.2f}'
            }
        }],
        tooltip: {
            shared: true,
            xDateFormat: '%H:%M',
            formatter: function () {
                const esMedianoche = this.x === Date.UTC(2020, 0, 2, 0, 0, 0);
                const horaMostrada = esMedianoche
                    ? '24:00'
                    : Highcharts.dateFormat('%H:%M', this.x);

                let s = `<b>Hora: ${horaMostrada}</b><br/>`;
                this.points.forEach(point => {
                    s += `<span style="color:${point.color}">\u25CF</span> ${point.series.name}: <b>${point.y}</b><br/>`;
                });
                return s;
            }
        },
        legend: {           
            enabled: false
        },
        series: [{
            name: 'CMg ($/MWh)',
            type: 'area',
            yAxis: 0,
            data: dataCMg,
            marker: {
                enabled: true,
                radius: 3
            },
            color: '#FF0000',
            fillOpacity: 0.1
        }, {
            name: 'Energía (MWh)',
            type: 'line',
            yAxis: 1,
            data: dataEnergia,
            marker: {
                enabled: true,
                radius: 3
            },
            color: '#0077FF'
        }]
    });
}

function convertirHoraAHoraCompleta(horaStr) {

    if (horaStr === "24:00") {
        // Simula "24:00" como el inicio del día siguiente en UTC
        return Date.UTC(2020, 0, 2, 0, 0, 0); // 2020-01-02T00:00:00Z
    }

    const [horas, minutos] = horaStr.split(':').map(Number);
    return Date.UTC(2020, 0, 1, horas, minutos, 0); // 2020-01-01THH:mm:00Z
}
function descargarReporteEnergiaDia() {
  
    let mensajeError = 'Debe seleccionar un dia';      

    if (diaSeleccionado == '0' || diaSeleccionado == 0) {
        //mostrarMensaje('mensaje', 'error', mensajeError);
        alert(mensajeError)
        return;
    }

    $.ajax({
        type: 'POST',
        url: controlador + 'GenerarReporteEnergiaDia',
        data: {
            periodo: periodoSeleccionado,
            version: versionSeleccionado,
            dia: diaSeleccionado
        },
        dataType: 'json',
        success: function (result) {
            if (result != "-1") {
                window.location.href = controlador + 'DescargarArchivo?file=' + result;

            }
            else {
                alert("Ha ocurrido un error interno no previsto en el sistema. Por favor comunique al Administrador del sistema.");
            }
        },
        error: function (err) {
            alert("Ha ocurrido un error interno no previsto en el sistema.Por favor comunique al Administrador del sistema.");
        }
    });
}

function regresar() {
    if (window.history.length > 1) {
        window.history.back();
    } else {
        //window.location.href = '/Home/Index';
    }
}

