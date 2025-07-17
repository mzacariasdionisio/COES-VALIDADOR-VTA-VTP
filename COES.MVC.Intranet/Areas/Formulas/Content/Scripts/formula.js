var controlador = siteRoot + 'formulas/'
var oTable = null;
var flag = true;
var plot = null;

$(function () {

    $('#FechaFormula').Zebra_DatePicker({
        onSelect: function (view, elements) {
            flag = false;
            oTable.dataTable().fnDraw();
        },
        onClear: function (view, elements) {
            flag = true;
            oTable.dataTable().fnDraw();
        }
    });

    $('#FechaDesde').Zebra_DatePicker({        
    });

    $('#FechaHasta').Zebra_DatePicker({
    });

    $('#cbAreaOperativa').change(function () {
        cargarFormulas();
    });

    $('#cbUsuario').change(function () {
        cargarFormulas();
    });
    
    $('#btnProcesar').click(function () {
        $('#mensaje').removeClass();
        $('#mensaje').addClass("action-alert");
        $('#mensaje').text("Debe grabar luego de procesar y hacer el tunnig por cada fórmula seleccionada.");
        $('#txtMinima').val("");
        $('#txtMedia').val("");
        $('#txtMaxima').val("");
        $('#diatipico').text($("#cbAgrupacion option:selected").text());
        $('#btnImportar').css("display", "block");
        $('#btnQuitarImportado').css("display", "block");

        calcular();
    });

    $('.close-formula').click(function () {
       
        if ($('#divFormula').css("display") == "block") {
            $('#divFormula').css("display", "none");
            $('.close-formula').text("Mostrar");
        }
        else {
            $('#divFormula').css("display", "block");
            $('.close-formula').text("Ocultar");
        }        
    });

    $("#resizable").resizable({ delay: 20 });

    $('#resizable').bind('resize', function (event, ui) {      
        plot.replot();
    });
    
    $('#btnBanda').click(function () {
        ajustarBanda();
    });

    $('#btnGrabar').click(function () {
        grabar();
    });   

    $('#btnOkGrabar').click(function () {
        grabar();
    });

    $('#btnImportar').click(function () {
        prevImportar();
    });

    $('#btnQuitarImportado').click(function () {
        quitarImportado();
    });

    $('#btnCancelGrabar').click(function () {
        $('#confirmarSave').bPopup().close();
    });

    $('#txtMinima').keypress(function (e) {
        if (e.keyCode == '13') {
            tunningFormula(1, "S", this.value, '');
        }
    });

    $('#txtMedia').keypress(function (e) {
        if (e.keyCode == '13') {
            tunningFormula(2, "S", this.value, '');
        }
    });

    $('#txtMaxima').keypress(function (e) {
        if (e.keyCode == '13') {
            tunningFormula(3, "S", this.value, '');
        }
    });

    cargarFormulas();
});

function getDate(fecha) {
    datetime = fecha.replace(/(\d+)\/(\d+)\/(\d+)/, "$3/$2/$1");
    return new Date(datetime);
}

function setearFormula(idFormula, abreviatura, detalle){
    $('#hfIdFormula').val(idFormula);
    $('#formula').text(abreviatura + " - " + detalle);
    $('#hfFuente').val($('#cbFuente').val());
    $('#hfSubEstacion').val(abreviatura);
    $('#hfArea').val(detalle);
}

function cargarFormulas() {
    var areaOperativa = $('#cbAreaOperativa').val();
    var Usuario = $('#cbUsuario').val();
    $.ajax({
        type: 'POST',
        url: controlador + "formula/formula",
        data: {
            areaOperativa: areaOperativa,
            indUsuario: Usuario,
        },
        success: function (evt) {
            $('#content-formula').html(evt);
            $('#FechaFormula').val("");
        },
        error: function () {
            mostrarError();
        }
    });
}

function calcular() {
    var formulas = $('#hfIdFormula').val();  

    if (formulas != "") {
        $.ajax({
            type: 'POST',
            url: controlador + 'formula/calcular',
            dataType: 'json',
            data: {
                formulas: formulas, fechaInicio: $('#FechaDesde').val(), fechaFin: $('#FechaHasta').val(),
                agrupacion: $('#cbAgrupacion').val(), fuente: $('#hfFuente').val()
            },
            cache: false,
            success: function (resultado) {

                $('#divAjuste').css('display', 'none');
                if (resultado == 1 || resultado == 2) {
                    procesar($('#cbAgrupacion').val());
                    
                    if (resultado == 2) {
                        $('#divAjuste').css('display', 'block');
                    }
                }
                else
                    mostrarError();
            },
            error: function () {
                mostrarError();
            }
        });
    }
    else
        mostrarAlerta("Por favor seleccione fórmulas");
}

function procesar(grupo){
    mostrarDatos(grupo);   
}

function mostrarDatos(grupo) {
    $.ajax({
        type: 'POST',
        url: controlador + "formula/datos",
        data: { agrupacion: grupo },
        success: function (evt) {
            $('#cntDatos').html(evt);
            mostrarPromedio('');
            plotear();            
        },
        error: function () {
            mostrarError();
        }
    });
}

function mostrarPromedio(idFocus){
    $.ajax({
        type: 'POST',
        url: controlador + 'formula/valores',
        data: {agrupacion: $('#cbAgrupacion').val()},
        success: function (evt) {
            $('#cntValores').html(evt);
            if (idFocus != '') {
                $(idFocus).focus();
            }
        },
        error: function () {
            mostrarError();
        }
    });
}

function removeSerie(obj) {
    var fecha = obj.value;
    var indicador = (obj.checked == true) ? "S" : "N";

    $.ajax({
        type: 'POST',
        url: controlador + 'formula/addremoveitem',
        dataType: 'json',
        data: {
            fecha: fecha, indicador: indicador
        },
        cache: false,
        success: function (resultado) {
            if (resultado == 1) {
                mostrarPromedio('');
                plotear();                
            }
            else
                mostrarError();
        },
        error: function () {
            mostrarError();
        }
    });
}

function plotear(){
    $.ajax({
        type: 'POST',
        url: controlador + 'formula/grafico',
        dataType: 'json',
        data: { agrupacion: $('#cbAgrupacion').val(), formula: $('#hfIdFormula').val() },
        cache: false,
        success: function (resultado) {          
            graficar(resultado);
        },
        error: function () {
            mostrarError();
        }
    });  
}

function graficar(resultado) {
    var serie = $.parseJSON(resultado.Series);
    var categoria = $.parseJSON(resultado.Categoria);
    var colores = $.parseJSON(resultado.Colores);    

    var array = [];
    
    for (i = 0; i < resultado.Cantidad; i++)
    {
        array.push({});
    }

    array.push({ lineWidth: 5.0 });
    array.push({ linePattern: [4, 3, 1, 3, 1, 3], lineWidth: 3.0 })
    array.push({ linePattern: [1, 2, 1, 2, 1, 2], lineWidth: 2.0 })
    array.push({ linePattern: [1, 2, 1, 2, 1, 2], lineWidth: 2.0 })
    array.push({ linePattern: [1, 2, 1, 2, 1, 2], lineWidth: 2.0 })
  
    if (plot) {
        plot.destroy();
    }

    plot = $.jqplot('cntGrafico', serie, {
        title: 'PERFIL SCADA',
        axes: {
            xaxis: {
                min: 1,
                max: 48,
                tickOptions: {
                    formatString: "%d"
                },
                tickInterval: "2"
            }
        },        
        seriesColors: colores,
        series: array,
        seriesDefaults: {
            rendererOptions: {
                smooth: true
            },
            showMarker: false
        },
        cursor: {           
            zoom: true,
            show:true
        },
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

    $('#cntGrafico').bind('jqplotDataClick', function (ev, seriesIndex, pointIndex, data) {
        var indice = 0;
        var pos = 0;
        
        $('.td-filaformula').css('background-color','#ffffff');
        $('#tablaDatos tr:first td').each(function (i) {
            $checkbox = $(this).find('input:checked');
            if ($checkbox.is(':checked')) {
                if (indice == seriesIndex) {
                    var i = 0;
                    $('#tablaDatos tr').each(function () {                        
                        if (i > 0) {
                            $(this).find('td').eq(pos).css("background-color", "#FFFFCC");
                        }
                        i++;                        
                    });
                }
                indice++;
            }
            pos++;
        });
    });

    $('#cntGrafico').bind('jqplotDataMouseOver', function (ev, seriesIndex, pointIndex, data) {        
        var indice = 0;
        var pos = 0;
        var texto = "Promedio";
        $('#tablaDatos tr:first td').each(function (i) {
            $checkbox = $(this).find('input:checked');
            if ($checkbox.is(':checked')) {
                if (indice == seriesIndex) {
                    $span = $(this).find('span');
                    texto = $span.text();                  
                }
                indice++;
            }
            pos++;
        });

        var color = plot.series[seriesIndex].seriesColors[seriesIndex];
        $('#chartTooltip').html("<div style='width:10px; height:10px; background-color:"+ color +"'></div>" + texto + " - " + obtenerHora(data[0]) + ' - ' + data[1].toFixed(2));
    });

    $('#cntGrafico').bind('jqplotDataUnhighlight', function (ev) {
        $('#chartTooltip').html('');
    });
}

function obtenerHora(i) {
    var hora = "";
    var minuto = (i % 2 == 0) ? "00" : "30";
    if (i == 1 || i == 48) hora = "00";
    else
    {
        hora = Math.floor(i / 2);
    }
    return pad(hora,2) + ":" + minuto;
}

function pad(str, max) {
    str = str.toString();
    return str.length < max ? pad("0" + str, max) : str;
}

function tunningFormula(id, tipo, valor, idfocus){    
    $.ajax({
        type: 'POST',
        url: controlador + 'formula/aplicartunnig',
        data: { indicador: tipo, indice: id, valor: valor, agrupacion: $('#cbAgrupacion').val() },
        dataType: 'json',
        cache: false,
        success: function (resultado) {
            if (resultado == 1)
            {
                mostrarPromedio(idfocus);
                plotear();
            }
            else
            {
                mostrarError();
            }
        },
        error: function () {
            mostrarError();
        }
    });    
}

function grabar(){
    $.ajax({
        type: 'POST',
        url: controlador + 'formula/grabarperfil',
        data: {
            comentario: $('#txtComentario').val(), fecInicio: $('#FechaDesde').val(),
            fecFin: $('#FechaHasta').val(), idFormula: $('#hfIdFormula').val(), agrupacion: $('#cbAgrupacion').val(),
            fuente: $('#hfFuente').val()
        },
        dataType: 'json',
        cache: false,
        success: function (resultado) {
            if (resultado > 0) {            
                $('#mensaje').removeClass("action-alert");
                $('#mensaje').addClass("action-exito");
                $('#mensaje').text("Se grabaron los datos correctamente...!");
            }
            else if (resultado == 0) {
                mostrarError();
            }
            else {
                alert("No se ha grabado");
            }
        },
        error: function () {
            mostrarError();
        }
    });
}

function preSave() {
    $('#confirmarSave').bPopup({
        easing: 'easeOutBack',
        speed: 450,
        transition: 'slideDown'
    });
}

function descargarFormato()
{
    $.ajax({
        type: 'POST',
        url: controlador + 'formula/generarformato',
        data: {
            subEstacion: $('#hfSubEstacion').val(),
            area: $('#hfArea').val(),
            agrupacion: $('#cbAgrupacion :selected').text(),
            formula: $('#hfIdFormula').val()
        },
        dataType: 'json',
        success: function (result) {
            if (result == 1) {
                window.location = controlador + "formula/descargarformato";
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

function prevImportar()
{
    $('#divImportar').css('display', 'block');
}

function closeImportar()
{
    $('#divImportar').css('display', 'none');
}

function importar() {
    if ($('#hfIdFormula').val() != "") {
        $.ajax({
            type: 'POST',
            url: controlador + 'formula/importar',
            data: { clasificacion: $('#cbAgrupacion').val(), formula: $('#hfIdFormula').val() },
            dataType: 'json',
            cache: false,
            success: function (resultado) {
                if (resultado == 1) {
                    plotear();
                    closeImportar();
                    $('#progreso').html('');
                    $('#fileInfo').html('');
                    $('#fileInfo').removeClass('action-exito');
                    $('#fileInfo').removeClass('action-alert');
                } else if (resultado == 0)
                {
                    mostrarAlerta("El formato importado no corresponde a la fórmula seleccionada.");
                    closeImportar();
                    $('#progreso').html('');
                    $('#fileInfo').html('');
                    $('#fileInfo').removeClass('action-exito');
                    $('#fileInfo').removeClass('action-alert');
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
        mostrarAlerta("Aún no ha seleccionado fórmula.");
    }
}

function quitarImportado()
{
    $.ajax({
        type: 'POST',
        url: controlador + 'formula/quitarimportado',
        data: { clasificacion: $('#cbAgrupacion').val(), formula: $('#hfIdFormula').val() },
        dataType: 'json',
        cache: false,
        success: function (resultado) {
            if (resultado == 1) {
                plotear();
            }
            else if (resultado == 0) {
                mostrarAlerta('No existe perfil importado.');
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

function loadInfoFile(fileName, fileSize) {
    $('#fileInfo').html(fileName + " (" + fileSize + ")");
    $('#fileInfo').removeClass('action-alert');
    $('#fileInfo').addClass('action-exito');
    $('#fileInfo').css('margin-bottom', '10px');
}

function loadValidacionFile(mensaje) {
    $('#fileInfo').html(mensaje);
    $('#fileInfo').removeClass('action-exito');
    $('#fileInfo').addClass('action-alert');
    $('#fileInfo').css('margin-bottom', '10px');
}

function mostrarProgreso(porcentaje) {
    $('#progreso').text(porcentaje + "%");
}

function ajustarBanda()
{
    var banda = $('#txtBanda').val();
    if (banda != "") {

        if (parseFloat(banda) > 0) {
            $.ajax({
                type: 'POST',
                url: controlador + 'formula/ajustarbanda',
                data: { clasificacion: $('#cbAgrupacion').val(), porcentaje: banda },
                dataType: 'json',
                cache: false,
                success: function (resultado) {
                    if (resultado == 1) {
                        mostrarPromedio('');
                        plotear();
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
            mostrarAlerta('Ingrese valor mayor a cero.');
        }
    }
    else {
        mostrarAlerta('Ingrese el porcentaje.');
    }
}

function mostrarError(){
    alert("Ha ocurrido un error");
}

function mostrarAlerta(msg){
    alert(msg);
}

function mostrarExito(msg){
    alert(msg);
}

function validarNumero(item, evt) {
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