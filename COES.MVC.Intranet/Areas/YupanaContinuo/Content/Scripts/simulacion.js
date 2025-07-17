var controlador = siteRoot + 'YupanaContinuo/Simulacion/';

var COLOR_ACTUALIZADO = '#350301'; //rojo oscuro
var COLOR_SIN_ACTUALIZAR = '#DE0A02'; //rojo 

var COLOR_EN_SIMULACION = '#F1E5E5';
var COLOR_EJEC_CONVERGE = '#20A705';
var COLOR_EJEC_DIVERGE = '#430AF4';
var COLOR_EJEC_CON_ERROR = '#F00B0B';
var COLOR_NO_SIMULADO = '#7B7878';
var COLOR_NO_SIMULADO_TIEMPO = '#DFDC00';

var fechaGrafico = "";
var identificadorGrafico = "";
var tagGrafico = "";

var numNodoClickeado = "";

var porcentajeSimulacion = -1;
var flagEsUltimoArbolCreado = "";  //SI: "S", NO: "N"
var flagActivarRefresco = "";

var gifcargando = '<img src="' + siteRoot + 'Areas/YupanaContinuo/Content/Images/cargando.gif" alt="" width="17" height="17" style="padding-left: 17px;">';

var SEG_REFRESCA_ARBOL = 15;

$(function () {
    $('#txtFecha').Zebra_DatePicker({
        onSelect: function () {
            cargarIdentificadoresSegunFecha();
        }
    });

    $('#cbIdentificador').change(function () {
        cargarTagsSegunIdentificador();
    });

    $('#btnConsultar').click(function () {
        mostrarGraficoArbol();
    });

    $('#btnRecalcular').click(function () {
        iniciarSimulacion();
    });

    $('#btnFinalizarEjec').click(function () {
        finalizarEjecucionGams();
    });

    $('#btnDescargarTag').click(function () {
        descargarArchivosDeSalidaTag();
    });

    setInterval(function () {
        if (flagActivarRefresco == "S") {
            if (flagEsUltimoArbolCreado == "S") {

                if (porcentajeSimulacion >= 0 && porcentajeSimulacion < 100) {
                    mostrarGraficoArbol();
                }
                if (porcentajeSimulacion == 100) {
                    flagActivarRefresco = 'N'
                    mostrarMensaje("mensaje_simulacion", "exito", "Mostrando último árbol. Simulación exitosa.");
                }
            }
        }
    }, SEG_REFRESCA_ARBOL * 1000);

    mostrarGraficoArbol();
});

function cargarIdentificadoresSegunFecha() {
    limpiarSeccionGrafico();

    var fechaConsulta_ = $("#txtFecha").val() || "";
    $("#cbIdentificador").empty();
    $('#cbIdentificador').get(0).options[0] = new Option("--  Seleccione  --", "0");

    $.ajax({
        type: 'POST',
        url: controlador + '/CargarListaIdentificadores',
        data: { fechaConsulta: fechaConsulta_ },

        success: function (evt) {
            if (evt.Resultado != "-1") {
                var listaIdentificadores = evt.ListaTopologia;

                //Listamos identificadores
                if (listaIdentificadores.length > 0) {
                    $("#cbIdentificador").empty();

                    $('#cbIdentificador').get(0).options[0] = new Option("--  Seleccione  --", "0"); //obliga seleccionar para buscar tag
                    for (var i = 0; i < listaIdentificadores.length; i++) {
                        $('#cbIdentificador').append('<option value=' + listaIdentificadores[i].Topcodi + '>' + listaIdentificadores[i].Identificador + '</option>');
                    }
                }

                cargarTagsSegunIdentificador();
            } else {
                alert("Ha ocurrido un error: " + evt.Mensaje);
            }

        },
        error: function (err) {
            mostrarMensaje_('mensaje', 'error', 'Se ha producido un error al cargar el listado de identificadores para la fecha escogida.');

        }
    });
}

function cargarTagsSegunIdentificador() {
    limpiarSeccionGrafico();

    var fechaConsulta_ = $("#txtFecha").val() || "";
    var topcodi = parseInt($("#cbIdentificador").val()) || 0;
    $("#cbTag").empty();

    $.ajax({
        type: 'POST',
        url: controlador + '/CargarListaTags',
        data: {
            fechaConsulta: fechaConsulta_,
            topcodi: topcodi
        },
        success: function (evt) {
            if (evt.Resultado != "-1") {
                if (evt.ListaTag.length > 0) {

                    $.each(evt.ListaTag, function (i, item) {
                        $('#cbTag').get(0).options[$('#cbTag').get(0).options.length] = new Option(item.Cparbtag, item.Cparbcodi);
                    });
                } else {
                    $('#cbTag').get(0).options[0] = new Option("--  Seleccione Tag  --", "0");
                }
            } else {
                alert("Ha ocurrido un error: " + evt.Mensaje);
            }
        },
        error: function (err) {
            mostrarMensaje_('mensaje', 'error', 'Se ha producido un error al cargar el listado de tags para el identificador escogido.');

        }
    });
}

function mostrarGraficoArbol() {
    limpiarSeccionGrafico();

    var fechaConsulta_ = $("#txtFecha").val();
    var cparbcodi = parseInt($("#cbTag").val()) || 0;

    $.ajax({
        url: controlador + "MostrarArbol",
        data: {
            fechaConsulta: fechaConsulta_,
            cparbcodi: cparbcodi,
        },
        type: 'POST',
        async: false,
        success: function (result) {
            if (result.Resultado === "-1") {
                mostrarMensaje_('mensaje', 'error', 'Se ha producido un error al mostrar arbol. Error: ' + result.Mensaje);

            } else {
                if (result.Resultado === "1") {
                    $("#mainLayout").css("height", "4650px");
                    /*verifico si el arbol consultado es el último de todos, osea el de ultimo de hoy. Para ello verifico si es el ultimo tag,
                      ya que si es el ultimo, se activa la actualizacion automatica de la vista del arbol*/

                    porcentajeSimulacion = result.PorcentajeNodosSimulados;
                    flagActivarRefresco = porcentajeSimulacion != -1 ? "S" : "N";

                    //mostrar mensaje de simulacion
                    if (porcentajeSimulacion >= 0 && porcentajeSimulacion < 100) {
                        flagEsUltimoArbolCreado = "S";

                        $("#mensaje_simulacion").css("display", "block");
                        mostrarMensaje("mensaje_simulacion", "alert", "Árbol en simulación. Porcentaje de avance: " + porcentajeSimulacion + "%." + gifcargando);
                    }

                    if (porcentajeSimulacion == -1) {
                        $("#mensaje_simulacion").css("display", "block");
                        mostrarMensaje("mensaje_simulacion", "alert", result.MensajeProceso);
                    }

                    if (porcentajeSimulacion == 100) {
                        $("#mensaje_simulacion").css("display", "block");
                        mostrarMensaje("mensaje_simulacion", "exito", "Simulación exitosa.");
                    }

                    //el boton GUARDAR solo se muestra en el último árbol creado
                    $("#hfFlagUltimoTag").val(result.EsUltimoTag);

                    //Seteo el valor del árbol a mostrar, usado para los botones Guardar y Descargar
                    $("#hfCodigoArbolMostrado").val(result.CodigoArbolMostrado);

                    //Mandamos datos al grafico (para titulo del grafico)
                    fechaGrafico = fechaConsulta_;
                    identificadorGrafico = result.IdentificadorArbolCreado;
                    tagGrafico = result.TagArbolCreado;

                    $('#mensaje_nota').css("display", "block");
                    if (result.EsUltimoTag == "S") {
                        $('#nota_guardar').css("display", "block");
                    } else
                        $('#nota_guardar').css("display", "none");


                    //por último se muestra el gráfico
                    var listaObj = [];
                    for (var i = 0; i < result.LstDatosNodos.length; i++) {
                        var regNodo = result.LstDatosNodos[i];

                        var regObj = {};
                        regObj.id = regNodo.Id;
                        regObj.numeroNodo = regNodo.NumeroNodo;
                        regObj.height = regNodo.Height;
                        regObj.borderColor = regNodo.BorderColor;
                        regObj.estadoNodo = regNodo.Estado;
                        regObj.description = regNodo.HtmlResultado;
                        regObj.descripSaveBD = regNodo.HtmlGuardarEscenario;
                        regObj.actualizacion = regNodo.MensajeActualizacion;
                        regObj.colorActualizacion = regNodo.ColorActualizacion;
                        regObj.info = regNodo.Info;
                        regObj.offset = regNodo.Offset;

                        listaObj.push(regObj);
                    }

                    mostrarGrafico(listaObj);
                }

                //Si no existe arbol
                if (result.Resultado === "2") {
                    $("#mainLayout").css("height", "620px");
                    limpiarSeccionGrafico();
                    //mostrarMensaje_('mensaje', 'error', 'No existe ningun árbol con los datos ingresados.');
                }
            }
        },
        error: function (xhr, status) {
            flagActivarRefresco = 'N';
            mostrarMensaje_('mensaje', 'error', 'Se ha producido un error al generar gráfico de árbol.');
        }
    });

}

function iniciarSimulacion() {
    if (confirm('¿Desea generar una simulación de la Fecha y hora actual?')) {
        flagActivarRefresco = "N";
        $.ajax({
            url: controlador + "IniciarSimulacion",
            data: {

            },
            type: 'POST',
            success: function (result) {
                if (result.Resultado === "-1") {
                    mostrarMensaje_('mensaje', 'error', 'Se ha producido un error al iniciar simulación. Error: ' + result.Mensaje);
                }
                else {
                    $('#btnRecalcular').hide();

                    mostrarMensaje("mensaje_simulacion", "alert", "Se iniciará simulación...");
                    setTimeout(function () {
                        location.reload(true)
                    }, 5000);
                }
            },
            error: function (xhr, status) {
                mostrarMensaje_('mensaje', 'error', 'Se ha producido un error al simular árbol.');
            }
        });
    }
}

function finalizarEjecucionGams() {
    porcentajeSimulacion = 0;

    if (confirm('¿Desea finalizar ejecución del árbol y procesos Gams?')) {
        flagActivarRefresco = "N";
        $.ajax({
            url: controlador + "FinalizarEjecucionGams",
            data: {

            },
            type: 'POST',
            success: function (result) {
                if (result.Resultado === "-1") {
                    mostrarMensaje_('mensaje', 'error', 'Se ha producido un error: ' + result.Mensaje);
                }
                else {
                    alert("Terminó de la ejecución del árbol y procesos Gams.");
                    mostrarGraficoArbol();
                }
            },
            error: function (xhr, status) {
                mostrarMensaje_('mensaje', 'error', 'Se ha producido un error al Finalizar ejecución de los procesos Gams del servidor de Yupana Continuo.');
            }
        });
    }
}

function mostrarArbolCreado(evt) {
    //mostramos la fecha del arbol creado (HOY)
    $("#txtFecha").val(evt.FechaArbolCreado);

    //actualizar combo Identificadores
    $("#cbIdentificador").empty();
    var listaIdentificadores = evt.ListaIdentificador;

    //Listamos identificadores
    if (listaIdentificadores.length > 0) {
        $('#cbIdentificador').get(0).options[0] = new Option("--  Seleccione  --", "0");
        for (var i = 0; i < listaIdentificadores.length; i++) {
            if (listaIdentificadores[i] == evt.IdentificadorArbolCreado) {
                $('#cbIdentificador').append('<option value=' + listaIdentificadores[i] + ' selected>' + listaIdentificadores[i] + '</option>');
            }
            else {
                $('#cbIdentificador').append('<option value=' + listaIdentificadores[i] + '>' + listaIdentificadores[i] + '</option>');
            }
        }

    } else {
        $('#cbIdentificador').get(0).options[0] = new Option("--  Seleccione  --", "0");
    }

    //actualizar combo tags   
    $("#cbTag").empty();
    var listaTags = evt.ListaTag;
    if (listaTags.length > 0) {
        $('#cbTag').get(0).options[0] = new Option("--  Seleccione Tag  --", "0");
        for (var i_ = 0; i_ < listaTags.length; i_++) {
            if (listaTags[i_] == evt.TagArbolCreado) {
                $('#cbTag').append('<option value=' + listaTags[i_] + ' selected>' + listaTags[i_] + '</option>');
            } else {
                $('#cbTag').append('<option value=' + listaTags[i_] + '>' + listaTags[i_] + '</option>');
            }
        }

    } else {
        $('#cbTag').get(0).options[0] = new Option("--  Seleccione Tag  --", "0");
    }


    //por ultimo muestra el arbol creado
    mostrarGraficoArbol();

    //Muestra mensaje que inicio la simulación
    $("#mensaje_simulacion").css("display", "block");
    mostrarMensaje("mensaje_simulacion", "alert", "Árbol en simulación ha iniciado.");
}


/**
 * Muestra mensajes de notificación
 * tipo: exito, error, alert, message
 */
function mostrarMensaje(id, tipo, mensaje) {
    $('#' + id).show();
    $('#' + id).removeClass();
    $('#' + id).addClass('action-' + tipo);
    $('#' + id).html(mensaje);

};

function mostrarMensaje_(id, tipo, mensaje) {
    alert(mensaje);
};


/************* Grafico  ******************* */
function mostrarGrafico(listaNodo) {
    Highcharts.chart('container_arbol', {

        chart: {
            height: 4400,
            inverted: false,
            events: {
                load: function () {
                    var ren = this.renderer;

                    //Cabeceras
                    ren.label('Condiciones Iniciales Térmicas', 310, 46)
                        .css({
                            fontWeight: 'bold',
                            color: "#6D6D6D"
                        })
                        .add();

                    ren.label('Act. H. Sin Compromiso', 620, 46)
                        .css({
                            fontWeight: 'bold',
                            color: "#6D6D6D"
                        })
                        .add();

                    ren.label('Act. H. Con Compromiso', 910, 46)
                        .css({
                            fontWeight: 'bold',
                            color: "#6D6D6D"
                        })
                        .add();

                    ren.label('Proyecciones RER', 1230, 46)
                        .css({
                            fontWeight: 'bold',
                            color: "#6D6D6D"
                        })
                        .add();

                    //Leyenda
                    var posX = 15;
                    var posX2 = posX + 25;
                    var posY = 180;
                    var posY2 = posY + 42;

                    ren.rect(posX - 10, posY - 10, 260, 170, 4).attr({
                        fill: 'white',
                        stroke: '#DDDDDD',
                        'stroke-width': 1
                    }).add();
                    ren.label('Leyenda:', posX, posY)
                        .css({
                            fontWeight: 'bold',
                            color: "#000000"
                        })
                        .add();

                    ren.rect(posX, posY + 30, 15, 15, 3).attr({
                        fill: 'white',
                        stroke: COLOR_EN_SIMULACION,
                        'stroke-width': 2
                    }).add();
                    ren.text('En Simulación', posX2, posY2).attr({
                    }).css({
                        fontSize: '9pt',
                        color: 'black'
                    }).add();

                    ren.rect(posX, posY + 50, 15, 15, 3).attr({
                        fill: 'white',
                        stroke: COLOR_EJEC_CON_ERROR,
                        'stroke-width': 2
                    }).add();
                    ren.text('Simulado con error', posX2, posY2 + 20).attr({
                    }).css({
                        fontSize: '9pt',
                        color: 'black'
                    }).add();

                    ren.rect(posX, posY + 70, 15, 15, 3).attr({
                        fill: 'white',
                        stroke: COLOR_EJEC_CONVERGE,
                        'stroke-width': 2
                    }).add();
                    ren.text('Simulado y Converge', posX2, posY2 + 40).attr({
                    }).css({
                        fontSize: '9pt',
                        color: 'black'
                    }).add();

                    ren.rect(posX, posY + 90, 15, 15, 3).attr({
                        fill: 'white',
                        stroke: COLOR_EJEC_DIVERGE,
                        'stroke-width': 2
                    }).add();
                    ren.text('Simulado y Diverge', posX2, posY2 + 60).attr({
                    }).css({
                        fontSize: '9pt',
                        color: 'black'
                    }).add();

                    ren.rect(posX, posY + 110, 15, 15, 3).attr({
                        fill: 'white',
                        stroke: COLOR_NO_SIMULADO,
                        'stroke-width': 2
                    }).add();
                    ren.text('No Simulado', posX2, posY2 + 80).attr({
                    }).css({
                        fontSize: '9pt',
                        color: 'black'
                    }).add();

                    ren.rect(posX, posY + 130, 15, 15, 3).attr({
                        fill: 'white',
                        stroke: COLOR_NO_SIMULADO_TIEMPO,
                        'stroke-width': 2
                    }).add();
                    ren.text('No Simulado por tiempo prolongado', posX2, posY2 + 100).attr({
                    }).css({
                        fontSize: '9pt',
                        color: 'black'
                    }).add();
                }
            }
        },

        title: {
            text: identificadorGrafico + ' / ' + tagGrafico,
            style: {
                color: '#000000',
                fontWeight: 'bold',
                fontSize: '16px'
            }
        },
        subtitle: {
            text: '.',
            style: {
                color: '#FFFFFF',
                fontSize: '16px'
            }
        },
        series: [
            {
                type: 'organization',
                name: 'COES',
                keys: ['from', 'to'],
                data: [
                    ['A', 'B'],
                    ['A', 'C'],
                    ['B', 'B1'],
                    ['B', 'B2'],
                    ['C', 'C1'],
                    ['C', 'C2'],
                    ['B1', 'B11'],
                    ['B1', 'B12'],
                    ['B2', 'B21'],
                    ['B2', 'B22'],
                    ['C1', 'C11'],
                    ['C1', 'C12'],
                    ['C2', 'C21'],
                    ['C2', 'C22'],
                    ['B11', 'B111'],
                    ['B11', 'B112'],
                    ['B12', 'B121'],
                    ['B12', 'B122'],
                    ['B21', 'B211'],
                    ['B21', 'B212'],
                    ['B22', 'B221'],
                    ['B22', 'B222'],
                    ['C11', 'C111'],
                    ['C11', 'C112'],
                    ['C12', 'C121'],
                    ['C12', 'C122'],
                    ['C21', 'C211'],
                    ['C21', 'C212'],
                    ['C22', 'C221'],
                    ['C22', 'C222']
                ],
                point: {
                    events: {
                        click: function () {
                            if (this.estadoNodo != 'C') {
                                popupBotones(this.id, this.estadoNodo);
                                numNodoClickeado = this.numeroNodo;
                            }

                            $('#btnDescargar').unbind();
                            $('#btnDescargar').click(function () {
                                $('#botonera').bPopup().close();
                                descargarArchivosDeSalidaNodo();
                            });

                            $('#btnGuardar').unbind();
                            $('#btnGuardar').click(function () {
                                $('#botonera').bPopup().close();
                                guardarDataInsumosNodo();
                            });

                        }

                    }
                },

                levels: [
                    {
                        level: 0,
                        color: 'silver',
                        dataLabels: {
                            color: 'black'
                        },
                        height: 25
                    }, {
                        level: 1,
                        color: '#FFBFBF', //rojo claro                
                        height: 25
                    },
                    {
                        level: 2,
                        color: '#C4CEFF' //violeta claro
                    },
                    {
                        level: 3,
                        color: '#80DFFF'  //celeste claro
                    }, {
                        level: 4,
                        color: '#BFFFBF'  //verde claro
                    }
                ],
                nodes: listaNodo,
                colorByPoint: false,
                color: '#007ad0',
                dataLabels: {
                    color: 'black',
                    nodeFormat: '<div style="text-align: center;font-size: 10px;font-weight:bold;margin-left: 90px;width: 16px;height: 16px;-moz-border-radius: 50%;-webkit-border-radius: 50%;border-radius: 50%;border: 1px solid #555;">{point.numeroNodo}</div> <div style="text-align: center; font-size: 14px; font-weight:bold; color: {point.colorActualizacion}; padding-bottom: 6px;">{point.actualizacion}</div>{point.description} <div style="text-align: center; font-size: 14px; padding-top: 5px; padding-bottom: 1px;">{point.descripSaveBD}</div>',

                },
                borderColor: 'white',
                nodeWidth: 240,
                borderWidth: 8,
                cursor: 'pointer'

            }
        ],
        tooltip: {
            outside: true,
            formatter: function () {
                return this.point.info;
            }
        },
        exporting: {
            allowHTML: true,
            sourceWidth: 1400,
            sourceHeight: 4400,

            buttons: {
                contextButton: {
                    //Desactivo FullScreen, PrintChart y svg
                    menuItems: [
                        //'printChart',
                        //'separator',
                        'downloadPNG',
                        'downloadJPEG',
                        'downloadPDF',
                        //'downloadSVG'
                    ]
                }
            }
        },
        lang: {
            printChart: 'Imprimir Gráfico',
            downloadPNG: 'Descargar Gráfico de la simulación en archivo .PNG',
            downloadJPEG: 'Descargar Gráfico de la simulación en archivo .JPEG',
            downloadPDF: 'Descargar Gráfico de la simulación en archivo  .PDF',
            downloadSVG: 'Descargar Gráfico de la simulación en archivo  .SVG',
            contextButtonTitle: 'Opciones de Descarga del Gráfico'
        },

    });
}

function descargarArchivosDeSalidaNodo() {
    var numeroNodoClikeado_ = numNodoClickeado;
    var codigoArbol_ = $("#hfCodigoArbolMostrado").val();;

    if (confirm('¿Desea descargar los datos del escenario?')) {

        $.ajax({
            type: 'POST',
            url: controlador + 'DescargarArchSalida',
            data: {
                numeroNodoClikeado: numeroNodoClikeado_,
                arbolcodi: codigoArbol_
            },
            success: function (evt) {

                if (evt.Resultado != "-1") {
                    if (evt.Resultado == "0")
                        alert(evt.Mensaje);
                    else
                        window.location = controlador + "Exportar?file_name=" + evt.Resultado;
                } else {
                    alert("Ha ocurrido un error: " + evt.Mensaje);
                }
            },
            error: function (err) {
                alert('Ha ocurrido un error al descargar archivo.');
            }
        });

    }
}

function descargarArchivosDeSalidaTag() {

    var codigoArbol_ = $("#hfCodigoArbolMostrado").val();;

    if (confirm('¿Desea descargar los datos del Tag mostrado?')) {

        $.ajax({
            type: 'POST',
            url: controlador + 'DescargarArchivosTag',
            data: {

                arbolcodi: codigoArbol_
            },
            success: function (evt) {

                if (evt.Resultado != "-1") {
                    if (evt.Resultado == "0")
                        alert(evt.Mensaje);
                    else
                        window.location = controlador + "Exportar?file_name=" + evt.Resultado;
                } else {
                    alert("Ha ocurrido un error: " + evt.Mensaje);
                }
            },
            error: function (err) {
                alert('Ha ocurrido un error al descargar archivo.');
            }
        });

    }
}

function guardarDataInsumosNodo() {
    var numeroNodoClikeado_ = numNodoClickeado;
    var codigoArbol_ = $("#hfCodigoArbolMostrado").val();;

    if (confirm('¿Desea guardar la información del escenario?')) {

        $.ajax({
            type: 'POST',
            url: controlador + 'GuardarDataNodo',
            data: {
                numeroNodoClikeado: numeroNodoClikeado_,
                arbolcodi: codigoArbol_
            },
            success: function (evt) {
                if (evt.Resultado != "-1") {
                    alert("La información del escenario se guardo correctamente.")
                    mostrarGraficoArbol();
                } else {
                    alert("Ha ocurrido un error: " + evt.Mensaje);
                }
            },
            error: function (err) {
                alert('Ha ocurrido un error al guardar información del escenario.');
            }
        });

    }
}

function popupBotones(id, estadoNodo) {
    var htmlMostrar = "";
    var htmlDescargar = '<div style="text-align: center;margin: 25px;">  <input type="button" id="btnDescargar" value="Descargar" >  </div >';
    var htmlDescargarYGuardar = '<div style="text-align: center;margin: 25px;">  <input type="button" id="btnDescargar" value="Descargar" ><input type="button" id="btnGuardar" value="Guardar" >  </div >';

    if (estadoNodo == "E") {
        htmlMostrar = htmlDescargarYGuardar;
    } else {
        htmlMostrar = htmlDescargar;
    }


    $('#idBotonera').html(htmlMostrar);
    setTimeout(function () {
        $('#botonera').bPopup({
            easing: 'easeOutBack',
            speed: 450,
            transition: 'slideDown',
            modalClose: false
        });

    }, 50);
}


function limpiarSeccionGrafico() {
    $('#container_arbol').html("");
    $('#mensaje_nota').css("display", "none");
    $("#mensaje_simulacion").hide();
}

