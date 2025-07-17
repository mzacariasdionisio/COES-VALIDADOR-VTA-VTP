var controlador = siteRoot + 'despacho/grupodespacho/';

//Inicio - Curva de Consumo
var puntosgrabar = "";
var esPersonalizado = false;
//Fin - Curva de Consumo

$(function () {

    //Inicio - Curva de Consumo

    var esVisible = false;
    $('#btnConfigurar').click(function () {
        if (esVisible) {
            $('.contenedor-edicion').css('visibility', 'hidden');
            esVisible = false;
        } else {
            $('.contenedor-edicion').css('visibility', 'visible');
            esVisible = true;
        }
    });

    $('.contenedor-edicion').css('visibility', 'hidden');
    
    //Evento del boton exportar NCP
    $('#btnExportarNCP').click(function () {
        ExportarTexto();
    });

    // Evento del boton cancelar
    $('#btnCancelarNuevo').click(function () {
        $('#popupEditarCurva').bPopup().close();
    });

    // Evento del boton de SugiereOptimo
    $('#btnSugiereOptimo').click(function () {
        sugiereOptimo();
    });

    // Evento del boton generar tramos
    $('#btnGeneraTramos').click(function () {
        generaTramos();
    });

    // Evento del boton generar tramos personalizados
    $('#btnGeneraTramosCustomizados').click(function () {
        generaTramosCustomizados();
    });

    $('#chkCustomizado').change(function () {
        generarElementosParaPersonalizar();
    });

    $('#cboTramoCustomizado').change(function () {
        //console.log("-->");
        generarElementosParaCombosSeleccion();
        
        $('#cboTramo').val($('#cboTramoCustomizado').val());
    });

    //Fin - Curva de Consumo

    $('#cbEmpresas').multipleSelect({
        width: '200px',
        filter: true,
        onClick: function (view) {
            cargarCentrales();
        },
        onClose: function (view) {
            cargarCentrales();
        }
    });

    $('#cbEmpresas').multipleSelect('checkAll');

    $('#btnCargar').click(function () {
        cargarArbol();
        mostrarCurva("");
    });

    $('#txtFechaDato').Zebra_DatePicker({
        onSelect: function () {
            mostrarCurva("");
        }
    });

    $('#btnCurvaConsumo').click(function () {
        mostrarCurva("");
    });

    $('#btnCurvaCM').click(function () {
        habilitarCurvaCMgN();
    });

    $('#cbTipoCentral').change(function () {
        cargarCentrales();
    });

    $('#btnGrabarIdentificadorNCP').click(function () {
        grabarIdentificadorNCP();
    });

    $('#btnCancelarIdentificadorNCP').click(function () {
        $('#popupIdentificadorNCP').bPopup().close();
    });

    cargarCentrales();
    cargarArbol();
    mostrarCurva("");
});

cargarCentrales = function () {

    var empresa = $('#cbEmpresas').multipleSelect('getSelects');
    if (empresa == "[object Object]") empresa = "";
    $('#hfEmpresa').val(empresa);

    $.ajax({
        type: 'POST',
        url: controlador + 'central',
        data: {
            tipoCentral: $('#cbTipoCentral').val(),
            empresas: $('#hfEmpresa').val()
        },
        success: function (evt) {
            $('#central').html(evt);
        },
        error: function () {
            mostrarError();
        }
    });
}

habilitarCurvaCMgN = function () {
    $('.contenedor-curva').toggle();
}

cargarArbol = function () {

    var empresa = $('#cbEmpresas').multipleSelect('getSelects');
    var central = $('#cbCentral').multipleSelect('getSelects');
    if (empresa == "[object Object]") empresa = "";
    if (central == "[object Object]") central = "";

    $('#hfEmpresa').val(empresa);
    $('#hfCentral').val(central);

    $.ajax({
        type: 'POST',
        url: controlador + 'arbol',
        data: {
            empresas: $('#hfEmpresa').val(),
            tipoCentral: $('#cbTipoCentral').val(),
            central: $('#hfCentral').val()
        },
        success: function (evt) {
            $('#tree').html(evt);
        },
        error: function () {
            mostrarError();
        }
    });
}

mostrarCurva = function (grupo) {

    if ($('#txtFechaDato').val() != "") {
        var empresa = $('#cbEmpresas').multipleSelect('getSelects');
        var central = $('#cbCentral').multipleSelect('getSelects');

        if (empresa == "[object Object]") empresa = "";
        if (central == "[object Object]") central = "";

        $('#hfEmpresa').val(empresa);
        $('#hfCentral').val(central);

        $.ajax({
            type: 'POST',
            url: controlador + 'curvaconsumo',
            data: {
                empresas: $('#hfEmpresa').val(),
                grupo: grupo,
                fecha: $('#txtFechaDato').val(),
                tipoCentral: $('#cbTipoCentral').val(),
                central: $('#hfCentral').val()
            },
            dataType: 'json',
            success: function (result) {
                if (result != -1) {
                    pintarGraficos(result);
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
        mostrarMensaje('Seleccione fecha de datos');
    }
}

//Inicio: Ajustar Curva Consumo

ExportarTexto = function () {
    if ($('#txtFechaDato').val() != "") {

        $.ajax({
            type: 'POST',
            url: controlador + "GenerarArchivoTextoReporteNCP",
            data: {
                fecha: $('#txtFechaDato').val()
            },
            dataType: 'json',
            success: function (result) {
                if (result == 1) {
                    window.location = controlador + "ExportarTexto";
                }
                if (result == -1) {
                    alert("Error al imprimir.");
                }
            },
            error: function () {
                mostrarError();
            }
        });
    }
    else {
    mostrarMensaje('Seleccione fecha de datos');
    }
}

var consumoHistorial = [];

dibujarHistorial = function (pos, grupocodi) {
 
    //Se borran todos las filas de la tabla de puntos
    $("#tablaCoordenadas tbody tr").remove();

    for (var i in consumoHistorial[pos]) {
        $("#tablaCoordenadas").append('<tr><td style="text-align:center">' + consumoHistorial[pos][i][0] + '</td><td style="text-align:left">' + consumoHistorial[pos][i][1] + '</td></tr>');
    }

    $.ajax({
        type: 'POST',
        url: controlador + 'CurvaConsumoGrupoId',
        data: {
            empresas: $('#hfEmpresa').val(),
            grupo: '',
            fecha: $('#txtFechaDato').val(),
            tipoCentral: $('#cbTipoCentral').val(),
            central: $('#hfCentral').val(),
            grupoId: grupocodi
        },
        global:false,
        dataType: 'json',
        success: function (resultado) {
            if (resultado != -1) {

                var result = resultado.datos;
                var resultAux = [];
                dataEnsayoAux = [];

                if (result.length == 1) {
                    for (var j in result[0].ListaSerie.SerieEnsayo) {
                        var corrd = result[0].ListaSerie.SerieEnsayo[j];
                        resultAux.push([corrd.PuntoX, corrd.PuntoY]);
                    }
                } else {
                    for (var i in result) {
                        for (var j in result[i].ListaSerie.SerieEnsayo) {
                            var corrd = result[i].ListaSerie.SerieEnsayo[j];
                            resultAux.push([corrd.PuntoX, corrd.PuntoY]);
                        }
                    }
                    //Ordenamiento de forma ascendente                  
                    resultAux = ordenamientoAscendente(resultAux);
                }

                
               //Se selecciona los puntos sin los repetidos
               for (var i = 0; i < resultAux.length; i++) {
                    var corrd = resultAux[i];
                    if ($.inArray(parseFloat(corrd.PuntoX), data_x) == -1) {
                          dataEnsayoAux.push([parseFloat(corrd[0]), parseFloat(corrd[1])]);
                     }
                }

                $('#contenidoHistoricoChart').html('');
                var contenedor = $("<div class='contenedor-main'></div>");
                var chartContent = $("<div class='contenedor-grafico'></div>");
                contenedor.append(chartContent);
                $('#contenidoHistoricoChart').append(contenedor);

                // Muestra la curva simple
                mostrarCurvaDoble(chartContent, dataEnsayoAux, consumoHistorial[pos]);

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

mostrarCurvaporGrupo = function (grupocodi) {
    if ($('#txtFechaDato').val() != "") {
        $.ajax({
            type: 'POST',
            url: controlador + 'CurvaConsumoPorGrupo',
            data: {
                grupocodi: grupocodi,
                fecha: $('#txtFechaDato').val()
            },
            global: false,
            dataType: 'json',
            success: function (result) {
                if (result != -1) {
                    listarHistorial(result, grupocodi);
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
        mostrarMensaje('Seleccione fecha de datos');
    }
}

listarHistorial = function (result, grupocodi) {
    //Se borran todos las filas de la tabla de puntos
    $("#tablaHistorial tbody tr").remove();

    consumoHistorial = [];

    //Se genera las filas de la tabla
    for (var i in result) {
        $("#tablaHistorial").append('<tr><td style="text-align:center">' + result[i].Fechadat + '</td><td style="text-align:left">' + result[i].Lastuser + '</td><td style="text-align:center">' + result[i].Fechaact + '</td><td><a href="JavaScript:dibujarHistorial(' + i + ',' + grupocodi+');"><img src="' + siteRoot +  '/Content/Images/ContextMenu/grafico.png" /></a></td></tr>');

        var consumo = []

        for (var j in result[i].ListaSerie.SerieConsumo) {
            var corrd = result[i].ListaSerie.SerieConsumo[j];
            consumo.push([corrd.PuntoX, corrd.PuntoY]);
        }
        consumoHistorial.push(consumo);
    }
}

//Fin: Ajustar Curva Consumo
abrirCurvaConsumo = function (nodo) {
    mostrarCurva(nodo);
}

cambiarCurva = function (idGrupo, tipo) {
    $.ajax({
        type: 'POST',
        url: controlador + 'actualizarcurva',
        data: {
            id: idGrupo,
            tipo: tipo
        },
        global: false,
        dataType: 'json',
        success: function (result) {
            if (result == 1) {
                var texto = (tipo == 0) ? "Ensayo de Pe" : "Ajustada SPR";
                $('#curva' + idGrupo).find("strong").text(texto);
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

pintarGraficos = function (result) {
    $('#contenidoGrafico').html('');

    for (var i in result) {

        var consumo = [];
        var ensayos = [];

        for (var j in result[i].ListaSerie.SerieConsumo) {
            var corrd = result[i].ListaSerie.SerieConsumo[j];
            consumo.push([corrd.PuntoX, corrd.PuntoY]);
        }

        for (var j in result[i].ListaSerie.SerieEnsayo) {
            var corrd = result[i].ListaSerie.SerieEnsayo[j];
            ensayos.push([corrd.PuntoX, corrd.PuntoY]);
        }

        var tipo = (result[i].Indcurva == "0") ? "Ensayo de Pe" : "Ajustada SPR";
        var cambio = (result[i].Indcurva == "0") ? 1 : 0;

        var contenedor = $("<div class='contenedor-main'></div>");
        var chartContent = $("<div class='contenedor-grafico'></div>");
        var chartCurva = $("<div class='contenedor-curva' id='curva" + result[i].Grupocodi + "'>Curva utilizada en CMgN: <strong>" + tipo +  "</strong><a style='float:right' href='JavaScript:cambiarCurva(" + result[i].Grupocodi + "," + cambio + ")'>Cambiar</a></div>");
        
        contenedor.append(chartContent);
        contenedor.append(chartCurva);

        // Inicio - Curva Consumo     
    
        var edicion = $("<div class=\"contenedor-edicion\"> <a style=\"float:left\" href=\"JavaScript:editarCurva('" + result[i].Curvcodi + "','" + result[i].Grupocodi + "','" + result[i].Gruponomb + "')\" > Editar Curva SPR</a>&nbsp;&nbsp;|&nbsp;&nbsp;<a href=\"JavaScript:editarCodigoNCP('" + result[i].Grupocodi + "','" + result[i].Gruponomb + "')\">Editar Código NCP</a>  <a style=\"float:right\" href=\"JavaScript:historicoCurva('" + result[i].Grupocodi + "','" + result[i].Gruponomb + "')\"> Histórico de Cambios </a> </div>");

        contenedor.append(edicion);
        // fin - Curva Consumo

        $('#contenidoGrafico').append(contenedor);

        $(chartContent).highcharts({
            title: {
                text: result[i].Gruponomb
            },
            yAxis: {
                title: {
                    text: 'Consumo[gal/h,m3/h,kg/h]',
                    style: {
                        color: '#009900'
                    }
                },
            },
            xAxis: {
                title: {
                    text: 'Potencia (MW)',
                    style: {
                        color: '#009900'
                    }
                },
            },
            legend: {
                layout: 'horizontal',
                align: 'center',
                verticalAlign: 'top',
                itemMarginTop: 25,
                borderWidth: 0
            },
            series: [{
                name: 'Curva ajustada SPR',
                data: consumo
            }, {
                name: 'Curva Ensayo de Pe',
                data: ensayos
            }],
            colors: ['#33B8FF', '#FF7A33'],
            chart: {
                backgroundColor: {
                    linearGradient: [0, 0, 0, 400],
                    stops: [
                        [0, '#FFFFFF'],
                        [1, '#E6E6E6']
                    ]
                },
                borderColor: '#E6E6E6',
                borderWidth: 2,
                className: 'dark-container',
                plotBackgroundColor: '#FFFFFF',
                plotBorderColor: '#E6E6E6',
                plotBorderWidth: 1
            }
        });

        //var curvaconsumo = "<span>Curva consumo utilizada en CMgN:</span><a JavaScript:modificarCurva()></a>";
        //$(chartCurva).html(curvaconsumo)
    }
}

mostrarError = function () {
    alert('Error');
}

mostrarMensaje = function (mensaje) {
    alert(mensaje);
}

// Inicio - Curva Consumo

var data_x = [];
var data_y = [];
var dataAuxDos = [];
var dataAuxTres = [];
var dataEnsayoAux = [];
var contenedor;
var chartContent;
var global_data_x = [];
var global_data_y = [];

//AGREGADO
var global_gruposCodi;

editarCodigoNCP = function (grupocodi, gruponomb) {
    
    $.ajax({
        type: 'POST',
        url: controlador + 'obtenerNCP',
        data: {
            id: grupocodi
        },
        dataType: 'json',
        success: function (result) {
            $('#txtCodigoNCP').val(result);
            $('#hfIdentificadorNCP').val(grupocodi);
            $('#lblModoOperacion').text(gruponomb)

            setTimeout(function () {
                $('#popupIdentificadorNCP').bPopup({
                    autoClose: false
                });
            }, 50);
        },
        error: function () {
            mostrarError();
        }
    });
}

grabarIdentificadorNCP = function () {
    if (confirm('¿Está seguro de realizar esta acción?')) {

        if (IsOnlyDigit($('#txtCodigoNCP').val()) == true) {
            $.ajax({
                type: 'POST',
                url: controlador + 'grabaridentificadorncp',
                data: {
                    id: $('#hfIdentificadorNCP').val(),
                    codigoncp: $('#txtCodigoNCP').val()
                },
                dataType: 'json',
                success: function (result) {
                    if (result == 1) {
                        $('#popupIdentificadorNCP').bPopup().close();
                        alert("La actualización se realizó correctamente.");
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
            alert("Ingrese un valor numérico para el identificador NCP");
        }
    }
}

editarCurva = function (curvcodi, grupocodi, gruponomb) {

    //obtenerNCP(grupocodi);

    setTimeout(function () {
        $('#popupEditarCurva').bPopup({
            autoClose: false,
        });
    }, 50);

    data_x = [];
    data_y = [];
    dataAuxDos = [];
    dataAuxTres = [];
    dataEnsayoAux = [];
    global_data_x = [];
    global_data_y = [];
    contenedor = null;
    chartContent = null;
    var titulo = "INDIVIDUAL : " + gruponomb;

    //Seleccione el combo en el inicio
    $("#cboTramo").val("-1");

    //Ocultar el mensaje de errores
    ocultarMensajeEditar();

    //limpiar las cajas de texto donde se muestra el resultado
    limpiarCajaResultado();

    //Se borran todos las filas de la tabla de puntos
    $("#tablaPuntos tbody tr").remove();

    //Se obtiene los datos para la grafica y se muestran los puntos en la tabla 
    $.ajax({
        type: 'POST',
        url: controlador + 'CurvaConsumoGrupoId',
        data: {
            empresas: $('#hfEmpresa').val(),
            grupo: '',
            fecha: $('#txtFechaDato').val(),
            tipoCentral: $('#cbTipoCentral').val(),
            central: $('#hfCentral').val(),
            grupoId: grupocodi
        },
        global: false,
        dataType: 'json',
        global:false,
        success: function (resultado) {
            var result = resultado.datos;
            var fecha = resultado.fechaDatos;

            if (result != -1) {
                var resultAux = [];
                 global_gruposCodi = grupocodi;

                if (result.length == 1) {
                        for (var j in result[0].ListaSerie.SerieEnsayo) {
                            var corrd = result[0].ListaSerie.SerieEnsayo[j];
                            resultAux.push([corrd.PuntoX, corrd.PuntoY]);
                        }
                } else {
                    titulo = "AGRUPADO : ";
                    for (var i in result) {
                        titulo += result[i].Gruponomb;
                        if (i != result.length - 1) { titulo += ","}
                        
                        for (var j in result[i].ListaSerie.SerieEnsayo) {
                            var corrd = result[i].ListaSerie.SerieEnsayo[j];
                            resultAux.push([corrd.PuntoX, corrd.PuntoY]);
                        }
                    }
                    //Ordenamiento de forma ascendente                  
                    resultAux = ordenamientoAscendente(resultAux);
                }

                //Se selecciona los puntos sin los repetidos
                for (var i = 0; i < resultAux.length; i++) {
                    var corrd = resultAux[i];
                    if ($.inArray(parseFloat(corrd[0]), data_x) == -1) {
                        dataEnsayoAux.push([parseFloat(corrd[0]), parseFloat(corrd[1])]);
                        data_x.unshift(parseFloat(corrd[0]));
                        data_y.unshift(parseFloat(corrd[1]));
                        global_data_x.unshift(parseFloat(corrd[0]));
                        global_data_y.unshift(parseFloat(corrd[1]));
                    }
                }

                var seleccionado = '';
                //Se muestra los puntos en la tabla
                for (var i = 0; i < data_x.length; i++) {
                    if (i != 0)
                    {                     
                        seleccionado = 'checked="checked"';
                    }
                    else
                    {
                        if ((parseFloat(data_x[i]).toFixed(3) == 0.000) && (parseFloat(data_y[i]).toFixed(3) == 0.000)) {
                            seleccionado = '';
                        }
                        else
                        {
                            seleccionado = 'checked="checked"';
                        }
                        
                    }

                    $("#tablaPuntos").append('<tr><td>P' + (i + 1) + '</td><td id="id_x_' + (i + 1) + '">' + parseFloat(data_x[i]).toFixed(3) + '</td><td id="id_y_' + (i + 1) + '">' + parseFloat(data_y[i]).toFixed(3) + '</td><td><input type="checkbox" ' + seleccionado + ' value="' + parseFloat(data_x[i]).toFixed(3) + '"  name="puntos" onchange="excluyePunto('+i+',this.checked);" ></td><td style="display:none"><select name="cboTramoPunto" style="width:50px"><option value="1">1</option><option value="2">2</option></select></td></tr>');
                }

                //Se coloca el titulo en el poppus
                $("#idTitulo").text(titulo);

                //Se muestra la grafica
                $('#contenidoChart').html('');
                contenedor = $("<div class='contenedor-main'></div>");
                chartContent = $("<div class='contenedor-grafico'></div>");
                contenedor.append(chartContent);
                $('#contenidoChart').append(contenedor);
                mostrarCurvaEnsayoSimple(chartContent, dataEnsayoAux);

                
                $('#txtFechaVigencia').Zebra_DatePicker({                    
                    direction: [fecha, false]
                });
                
            }
            else {
                mostrarError();
            }
        },
        error: function () {
            mostrarError();
        }
    });

    //Des-asocia el clik y lo asocia para que no se anide.
    $('#btnGrabarNuevo').unbind('click');

    $('#btnGrabarNuevo').click(function () {
        registrarCurva();
    });

    //Oculta la tabla de personalización de puntos de la curva
    ocultarElementosParaPersonalizarCurva();
}

ordenamientoAscendente = function (resultAux) {
    for (var i = 0; i < resultAux.length - 1; i++) {
        for (var j = 0; j < resultAux.length - 1; j++) {
            if (resultAux[j][0] < resultAux[j + 1][0]) {
                var tmp = resultAux[j + 1];
                resultAux[j + 1] = resultAux[j];
                resultAux[j] = tmp;

            }
        }
    }
    return resultAux;
}

historicoCurva = function (grupocodi, gruponomb) {
    setTimeout(function () {
        $('#popupHistoricoCurva').bPopup({
            autoClose: false,
        });
    }, 50);

    //mostrar las curvas del historial
    mostrarCurvaporGrupo(grupocodi);

    //Dibujar el primer grafico
    dibujarHistorial(0, grupocodi);

    //Se coloca el t´tulo en el poppus
    $("#idTituloHistorico").text("Histórico de Curva: " + gruponomb);

    // Evento del boton cancelar
    $('#btnCancelarHistorico').click(function () {
        $('#popupHistoricoCurva').bPopup().close();
    });

}

verificaCantidadPuntosSeleccionados = function () {
    var c = 0;
    $("input[name ='puntos']").each(function () {
        if (this.checked) { c++; }
    });
    return c;
}

verificaPuntosSeleccionados = function () {
    var selected = '';
    var aux_x = [];
    var aux_y = [];
    dataEnsayoAux = [];

    $("input[name ='puntos']").each(function () {
        if (this.checked) {
            var check_x = parseFloat($(this).val()).toFixed(3);
            for (var i = 0; i < global_data_x.length; i++) {
                var var_x = parseFloat(global_data_x[i]).toFixed(3);
                if (var_x == check_x) {
                    aux_x.push(global_data_x[i]);
                    aux_y.push(global_data_y[i]);
                    dataEnsayoAux.unshift([parseFloat(global_data_x[i]), parseFloat(global_data_y[i])]);//CAMBIADO
                }
            }
        }
    });

    data_x = aux_x;
    data_y = aux_y;
}

sugiereOptimo = function () {
    esPersonalizado = false;
    limpiarCajaResultado();
    ocultarMensajeEditar();

    var puntos = verificaCantidadPuntosSeleccionados();
    if (puntos < 2) {
        mostrarMensajeEditar("Para encontra el óptimo debe tener como mínimo 2 puntos");
    } else if (puntos < 3) {
        mostrarMensajeEditar("Se decidió por el ajuste con un tramo, dado que solo existe dos puntos");
        generaTramoUno();
    } else if (puntos < 4) {
        verificaPuntosSeleccionados();
        var e_1 = 0, e_2 = 0;
        var existe_2 = true;

        var puntosDos = interacionDosTramosGenerico(dataAuxDos, data_x, data_y);
        e_1 = coeficienteCorrelacion(data_x, data_y);

        if (puntosDos == null) { existe_2 = false; }
        else { e_2 = (dataAuxDos[2] + dataAuxDos[5]) / 2.0; }
        if (e_1 >= 0.992) {
            mostrarMensajeEditar("Se decidió el ajuste a 1 tramo, dado que: el R1 a un tramo es superior a 0.992");
            generaTramoUno();
        } else if (existe_2 && e_2 >= 0.992) {
            mostrarMensajeEditar("Se decidió el ajuste a 2 tramos, dado que: el R2 a 2 tramos es superior a 0.992 y el R1 a un tramo no supera a 0.992");
            generaTramoDos();
        } else if (existe_2 && (e_1 >= e_2)) {
            mostrarMensajeEditar("Se decidió el ajuste a 1 tramo, dado que: el R1 a 1 tramo y el R2 a 2 tramos son menores 0.992 y R1 >= R2");
            generaTramoUno();
        } else if (existe_2 && (e_1 < e_2)) {
            mostrarMensajeEditar("Se decidió el ajuste a 2 tramos, dado que: el R1 a 1 tramo y el R2 a 2 tramos son menores 0.992 y R2 > R1");
            generaTramoDos();
        } else if (!existe_2) {
            mostrarMensajeEditar("Se decidió el ajuste a 1 tramo, dado que: el R1 a 1 tramo es menor 0.992 y no existe ajuste a 2 tramos");
            generaTramoUno();
        }
    } else {
        verificaPuntosSeleccionados();
        var e_1 = 0, e_2 = 0, e_3 = 0;
        var existe_2 = true, existe_3 = true;

        var dosTramos = interacionDosTramosGenerico(dataAuxDos, data_x, data_y);
        var tresTramos = interacionTresTramosGenerico(dataAuxTres, data_x, data_y);

        e_1 = coeficienteCorrelacion(data_x, data_y);

        if (dosTramos == null) { existe_2 = false; }
        else { e_2 = (dataAuxDos[2] + dataAuxDos[5]) / 2.0; }

        if (tresTramos == null) { existe_3 = false; }
        else { e_3 = (dataAuxTres[2] + dataAuxTres[5] + dataAuxTres[8]) / 3.0; }

        if (e_1 >= 0.992) {
            mostrarMensajeEditar("Se decidió el ajuste a 1 tramo, dado que: el R1 a 1 tramo es superior a 0.992");
            generaTramoUno();
        } else if (existe_2 && e_2 >= 0.992) {
            mostrarMensajeEditar("Se decidió el ajuste a 2 tramos, dado que: el R2 a 2 tramos es superior a 0.992 y el R1 a 1 tramo no supera a 0.992");
            generaTramoDos();
        } else if (existe_3 && e_3 >= 0.992) {
            mostrarMensajeEditar("Se decidió el ajuste a 3 tramos, dado que: el R3 a 3 tramos es superior a 0.992 y el R1 a 1 tramo no supera a 0.992 y R2 a 2 tramos no superan a 0.992");
            generaTramoTres();
        } else if (!existe_2 && existe_3 && (e_1 >= e_3)) {
            mostrarMensajeEditar("Se decidió el ajuste a 1 tramo, dado que: el R1 a 1 tramo y el R3 a 3 tramos son menores 0.992, no existe ajuste a 2 tramos y R1 >= R3");
            generaTramoUno();
        } else if (!existe_2 && existe_3 && (e_1 < e_3)) {
            mostrarMensajeEditar("Se decidió el ajuste a 3 tramos, dado que: el R1 a 1 tramo y el R3 a 3 tramos son menores 0.992, no existe ajuste a 2 tramos y R3 > R1");
            generaTramoTres();
        } else if (existe_2 && !existe_3 && (e_1 >= e_2)) {
            mostrarMensajeEditar("Se decidió el ajuste a 1 tramo, dado que: el R1 a 1 tramo y el R2 a 2 tramos son menores 0.992, no existe ajuste a 3 tramos y R1 >= R2");
            generaTramoUno(); 
        } else if (existe_2 && !existe_3 && (e_1 < e_2)) {
            mostrarMensajeEditar("Se decidió el ajuste a 2 tramos, dado que: el R1 a 1 tramo y el R2 a 2 tramos son menores 0.992, no existe ajuste a 3 tramos y R2 > R1");
            generaTramoDos();   
        } else if (existe_2 && existe_3 && (e_1 >= e_2) && (e_1 >= e_3)) {
            mostrarMensajeEditar("Se decidió el ajuste a 1 tramo, dado que: el R1, R2 y R3 son menores 0.992, R1 >= R2 y R1 >= R3");
            generaTramoUno();
        } else if (existe_2 && existe_3 && (e_2 >= e_1) && (e_2 >= e_3)) {
            mostrarMensajeEditar("Se decidió el ajuste a 2 tramos, dado que: el R1, R2 y R3 son menores 0.992, R2 >= R1 y R2 >= R3");
            generaTramoDos();
        } else if (existe_2 && existe_3 && (e_3 >= e_1) && (e_3 >= e_2)) {
            mostrarMensajeEditar("Se decidió el ajuste a 3 tramos, dado que: el R1, R2 y R3 son menores 0.992, R3 >= R1 y R3 >= R2");
            generaTramoTres();
        } else if (!existe_2 && !existe_3) {
            mostrarMensajeEditar("Se decidió el ajuste a 1 tramo, dado que: el R1 a 1 tramo es menor 0.992, no existe ajuste a 2 y 3 tramos");
            generaTramoUno();
        }
    }
}

generaTramos = function () {
    esPersonalizado = false;
    limpiarCajaResultado();
    ocultarMensajeEditar();
    var tramo = $("#cboTramo").val();

    if (tramo == '-1') {
        mostrarMensajeEditar("Para generar un resultado debe seleccionar un tramo.");
    } else if (tramo == '1') {
        generaTramoUno();
    } else if (tramo == '2') {
        generaTramoDos();
    } else if (tramo == '3') {
        generaTramoTres();
    }
}

generaTramosCustomizados = function () {
    esPersonalizado = true;
    limpiarCajaResultado();
    ocultarMensajeEditar();
    var tramo = $("#cboTramoCustomizado").val();

    if (tramo == '2') {
        if (esValidoDosTramosElegidosCantidad() == false) {
            mostrarMensajeEditar("Para dos tramos: debe escoger como mínimo 2 en tramo uno y 1 en tramo dos");
        } else if (esValidoDosTramosElegidosAscendente() == false) {
            mostrarMensajeEditar("Los puntos deben pertener a los tramos de forma ascendente");
        } else {
            generaTramoDos();
        }
    } else if (tramo == '3') {
        if (esValidoTresTramosElegidosCantidad() == false) {
            mostrarMensajeEditar("Para tres tramos: debe escoger como mínimo 2 en tramo uno, 1 en tramo dos y 1 en tramo tres");
        } else if (esValidoTresTramosElegidosAscendente() == false) {
            mostrarMensajeEditar("Los puntos deben pertener a los tramos de forma ascendente");
        } else {
            generaTramoTres();
        }
    }
}

esValidoDosTramosElegidosCantidad = function () {
    var salida = true;
    var numTramosDeUno = 0, numTramosDeDos = 0;

    $("select[name ='cboTramoPunto']").each(function () {
        if ($(this).is(":visible")) {
            var tramo = parseInt($(this).val());
            if (tramo == 1) { numTramosDeUno++; }
            else if (tramo == 2) { numTramosDeDos++; }
        }
    });

    if (numTramosDeUno < 2 || numTramosDeDos < 1) {
        salida = false;
    }
    return salida;
}

esValidoDosTramosElegidosAscendente = function () {
    var salida = true;
    var tramoActual = 1;

    $("select[name ='cboTramoPunto']").each(function () {
        if ($(this).is(":visible")) {
            var tramo = parseInt($(this).val());
            if (tramoActual == 1 && tramo == 2) {
                tramoActual = 2;
            }
            if (tramoActual == 2 && tramo == 1) {
                salida = false;
            }
        }
    });

    return salida;
}

esValidoTresTramosElegidosCantidad = function () {
    var salida = true;
    var numTramosDeUno = 0, numTramosDeDos = 0, numTramosDeTres = 0;

    $("select[name ='cboTramoPunto']").each(function () {
        if ($(this).is(":visible")) {
            var tramo = parseInt($(this).val());
            if (tramo == 1) { numTramosDeUno++; }
            else if (tramo == 2) { numTramosDeDos++; }
            else if (tramo == 3) { numTramosDeTres++; }
        }
    });

    if (numTramosDeUno < 2 || numTramosDeDos < 1 || numTramosDeTres < 1) {
        salida = false;
    }
    return salida;
}

esValidoTresTramosElegidosAscendente = function () {
    var salida = true;
    var tramoActual = 1;

    $("select[name ='cboTramoPunto']").each(function () {
        if ($(this).is(":visible")) {
            var tramo = parseInt($(this).val());
            if (tramoActual == 1 && tramo == 2) {
                tramoActual = 2;
            }
            if (tramoActual == 2 && tramo == 3) {
                tramoActual = 3;
            }
            if (tramoActual == 2 && tramo == 1) {
                salida = false;
            }
            if (tramoActual == 3 && (tramo == 1 || tramo == 2)) {
                salida = false;
            }
        }
    });

    return salida;
}

generarElementosParaCombosSeleccion = function () {
    var tramo = $("#cboTramoCustomizado").val();

    $("select[name ='cboTramoPunto']").each(function () {
        $(this).empty();
    });

    $("select[name ='cboTramoPunto']").each(function () {
        if (tramo == 2) {
            $(this).append("<option value='1'>1</option><option value='2'>2</option>");
        } else if (tramo == 3) {
            $(this).append("<option value='1'>1</option><option value='2'>2</option><option value='3'>3</option>");
        }
    });
}

generarElementosParaPersonalizar = function () {
    if ($("#chkCustomizado").is(':checked')) {

        $('#cboTramo').val($('#cboTramoCustomizado').val());

        mostrarElementosParaPersonalizarCurva();
    } else {
        ocultarElementosParaPersonalizarCurva();
    }
}

mostrarElementosParaPersonalizarCurva = function () {
    $("#lblTextTramo").show();
    $("#cboTramoCustomizado").show();
    $("#btnGeneraTramosCustomizados").show();
    $("#tablaPuntos td:nth-child(5),th:nth-child(5)").show();
    $("#cboTramoCustomizado").val(2);

    $("input[name ='puntos']").each(function (index) {
        excluyePunto(index, this.checked);
    });
}

ocultarElementosParaPersonalizarCurva = function () {
    $("#chkCustomizado").prop("checked", false);
    $("#lblTextTramo").hide();
    $("#cboTramoCustomizado").hide();
    $("#btnGeneraTramosCustomizados").hide();
    $("#tablaPuntos td:nth-child(5),th:nth-child(5)").hide();
}

excluyePunto = function (pos, estado) {
    //console.log(pos + " - " + estado);

    $("select[name ='cboTramoPunto']").each(function (index) {
        if (index == pos) {
            if (estado) {
                $(this).show();
            } else {
                $(this).hide();
            }
        }
    });

}

generaTramoUno = function () {
    var puntos = verificaCantidadPuntosSeleccionados();
    if (puntos < 2) {
        mostrarMensajeEditar("Se debe seleccionar como mínimo dos puntos.");
    } else {
        //Se cargan los puntos seleccionados
        verificaPuntosSeleccionados();

        var puntosUno = interacionUnTramoGenerico(data_x, data_y);

        var x_0 = parseFloat(puntosUno[0][0]).toFixed(3);
        var y_0 = parseFloat(puntosUno[0][1]).toFixed(3);

        var x_1 = parseFloat(puntosUno[1][0]).toFixed(3);
        var y_1 = parseFloat(puntosUno[1][1]).toFixed(3);

        $("#p0_x").text(x_0);
        $("#p0_y").text(y_0);

        $("#p1_x").text(x_1);
        $("#p1_y").text(y_1);

        $("#e0").text(toStringCorrelacionLineal(data_x, data_y));
        $("#r0").text(toStringFormulaLineal(data_x, data_y));

        puntosgrabar = x_0 + '%' + y_0 + '#' + x_1 + '%' + y_1;
       
        var dataAjustada = [];
        dataAjustada.push([puntosUno[0][0], puntosUno[0][1]]);
        dataAjustada.push([puntosUno[1][0], puntosUno[1][1]]);

        mostrarCurvaDoble(chartContent, dataEnsayoAux, dataAjustada);
    }
}

generaTramoDos = function () {
    var puntos = verificaCantidadPuntosSeleccionados();
    if (puntos < 3) {
        mostrarMensajeEditar("Se debe seleccionar como mínimo tres puntos.");
    } else {
        //Se cargan los puntos seleccionados
        verificaPuntosSeleccionados();

        var dataAux = [];
        var puntosDos = interacionDosTramosGenerico(dataAux, data_x, data_y);

        if (puntosDos == null) {
            mostrarMensajeEditar("No se puede representar en 2 tramos, debido a que la dispersión de datos generan una curva no convexa.");
        } else {
            $("#p0_x").text(parseFloat(puntosDos[0][0]).toFixed(3));
            $("#p0_y").text(parseFloat(puntosDos[0][1]).toFixed(3));

            $("#p1_x").text(parseFloat(puntosDos[1][0]).toFixed(3));
            $("#p1_y").text(parseFloat(puntosDos[1][1]).toFixed(3));

            $("#p2_x").text(parseFloat(puntosDos[2][0]).toFixed(3));
            $("#p2_y").text(parseFloat(puntosDos[2][1]).toFixed(3));

            $("#r0").text(toStringFormula(dataAux[0], dataAux[1]));
            $("#e0").text(toStringCorrelacion(dataAux[2]));

            $("#r1").text(toStringFormula(dataAux[3], dataAux[4]));
            $("#e1").text(toStringCorrelacion(dataAux[5]));

            puntosgrabar = $("#p0_x").text() + '%' + $("#p0_y").text() + '#' + $("#p1_x").text() + '%' + $("#p1_y").text() + '#' + $("#p2_x").text() + '%' + $("#p2_y").text();

            var dataAjustada = [];
            dataAjustada.push([puntosDos[0][0], puntosDos[0][1]]);
            dataAjustada.push([puntosDos[1][0], puntosDos[1][1]]);
            dataAjustada.push([puntosDos[2][0], puntosDos[2][1]]);

            mostrarCurvaDoble(chartContent, dataEnsayoAux, dataAjustada);
        }
    }
}

generaTramoTres = function () {
    var puntos = verificaCantidadPuntosSeleccionados();
    if (puntos < 4) {
        mostrarMensajeEditar("Se debe seleccionar como mínimo cuatro puntos.");
    } else {
        //Se cargan los puntos seleccionados
        verificaPuntosSeleccionados();

        var dataAux = [];
        var puntosTres = interacionTresTramosGenerico(dataAux, data_x, data_y);

        if (puntosTres == null) {
            mostrarMensajeEditar("No se puede representar en 3 tramos, debido a que la dispersión de datos generan una curva no convexa.");
        } else {
            $("#p0_x").text(parseFloat(puntosTres[0][0]).toFixed(3));
            $("#p0_y").text(parseFloat(puntosTres[0][1]).toFixed(3));

            $("#p1_x").text(parseFloat(puntosTres[1][0]).toFixed(3));
            $("#p1_y").text(parseFloat(puntosTres[1][1]).toFixed(3));

            $("#p2_x").text(parseFloat(puntosTres[2][0]).toFixed(3));
            $("#p2_y").text(parseFloat(puntosTres[2][1]).toFixed(3));

            $("#p3_x").text(parseFloat(puntosTres[3][0]).toFixed(3));
            $("#p3_y").text(parseFloat(puntosTres[3][1]).toFixed(3));

            $("#r0").text(toStringFormula(dataAux[0], dataAux[1]));
            $("#e0").text(toStringCorrelacion(dataAux[2]));

            $("#r1").text(toStringFormula(dataAux[3], dataAux[4]));
            $("#e1").text(toStringCorrelacion(dataAux[5]));

            $("#r2").text(toStringFormula(dataAux[6], dataAux[7]));
            $("#e2").text(toStringCorrelacion(dataAux[8]));

            puntosgrabar = $("#p0_x").text() + '%' + $("#p0_y").text() + '#' + $("#p1_x").text() + '%' + $("#p1_y").text() + '#' + $("#p2_x").text() + '%' + $("#p2_y").text() + '#' + $("#p3_x").text() + '%' + $("#p3_y").text();

            var dataAjustada = [];
            dataAjustada.push([puntosTres[0][0], puntosTres[0][1]]);
            dataAjustada.push([puntosTres[1][0], puntosTres[1][1]]);
            dataAjustada.push([puntosTres[2][0], puntosTres[2][1]]);
            dataAjustada.push([puntosTres[3][0], puntosTres[3][1]]);

            mostrarCurvaDoble(chartContent, dataEnsayoAux, dataAjustada);
        }
    }
}

mostrarMensajeEditar = function (mensaje) {
    $("#mensajeAgregar").html(mensaje);
    $("#mensajeAgregar").show();
}

ocultarMensajeEditar = function () {
    $("#mensajeAgregar").html("");
    $("#mensajeAgregar").hide();
}

limpiarCajaResultado = function () {
    $("#p0_x").text("");
    $("#p0_y").text("");
    $("#p1_x").text("");
    $("#p1_y").text("");
    $("#p2_x").text("");
    $("#p2_y").text("");
    $("#p3_x").text("");
    $("#p3_y").text("");
    $("#e0").text("");
    $("#r0").text("");
    $("#e1").text("");
    $("#r1").text("");
    $("#e2").text("");
    $("#r2").text("");
}

sumaX = function (datos_x) {
    var suma = 0;
    for (var i = 0; i < datos_x.length; i++) {
        suma += parseFloat(datos_x[i]);
    }
    return suma;
}

sumaY = function (datos_y) {
    var salida = 0;
    for (var i = 0; i < datos_y.length; i++) {
        salida += parseFloat(datos_y[i]);
    }
    return salida;
}

sumaCuadradosX = function (datos_x) {
    var salida = 0;
    for (var i = 0; i < datos_x.length; i++) {
        salida += Math.pow(parseFloat(datos_x[i]), 2);
    }
    return salida;
}

sumaCuadradosY = function (datos_y) {
    var salida = 0;
    for (var i = 0; i < datos_y.length; i++) {
        salida += Math.pow(parseFloat(datos_y[i]), 2);
    }
    return salida;
}

posMay = function (aux) {
    var max = aux[0];
    var posMay = 0;
    for (var i = 0; i < aux.length; i++) {
        if (aux[i] > max) {
            max = aux[i];
            posMay = i;
        }
    }
    return posMay;
}

sumaXY = function (datos_x, datos_y) {
    var salida = 0;
    for (var i = 0; i < datos_y.length; i++) {
        salida += parseFloat(datos_x[i]) * parseFloat(datos_y[i]);
    }
    return salida;
}

valorLinealM = function (datos_x, datos_y) {
    var salida = 0;
    salida = (datos_x.length * sumaXY(datos_x, datos_y) - sumaX(datos_x) * sumaY(datos_y)) / (datos_x.length * sumaCuadradosX(datos_x) - sumaX(datos_x) * sumaX(datos_x));
    return salida;
}

valorLinealB = function (datos_x, datos_y) {
    var salida = 0;
    salida = (sumaY(datos_y) / datos_x.length) - (valorLinealM(datos_x, datos_y) * (sumaX(datos_x) / datos_x.length));
    return salida;
}

coeficienteCorrelacion = function (datos_x, datos_y) {
    var numerador = datos_x.length * sumaXY(datos_x, datos_y) - sumaX(datos_x) * sumaY(datos_y);
    var denominador = Math.sqrt((datos_x.length * sumaCuadradosX(datos_x) - sumaX(datos_x) * sumaX(datos_x)) * (datos_x.length * sumaCuadradosY(datos_y) - sumaY(datos_y) * sumaY(datos_y)));
    if (denominador == 0) {
        return 1;
    } else {
        return Math.pow(numerador / denominador, 2);
    }
}

toStringFormulaLineal = function (datos_x, datos_y) {
    var salida = "";
    salida = "Y = " + valorLinealM(datos_x, datos_y).toFixed(3) + "X + " + valorLinealB(datos_x, datos_y).toFixed(3);
    return salida;
}

toStringCorrelacionLineal = function (datos_x, datos_y) {
    var salida = "";
    salida = "R = " + coeficienteCorrelacion(datos_x, datos_y).toFixed(4);
    return salida;
}

toStringCorrelacion = function (datos_e) {
    var salida = "";
    salida = "R = " + datos_e.toFixed(4);
    return salida;
}

toStringFormula = function (datos_m, datos_b) {
    var salida = "";
    salida = "Y = " + datos_m.toFixed(3) + "X + " + datos_b.toFixed(3);
    return salida;
}

interacionUnTramoGenerico = function (datos_x, datos_y) {
    var salida = [];
    var x0 = 0;
    var y0 = valorLinealM(datos_x, datos_y) * x0 + valorLinealB(datos_x, datos_y);
    var xn = datos_x[datos_x.length - 1];
    var yn = valorLinealM(datos_x, datos_y) * xn + valorLinealB(datos_x, datos_y);
    salida.push([x0, y0]);
    salida.push([xn, yn]);
    return salida;
}

generaDosPuntos = function (size) {
    var data_a = [];
    var data_b = [];
    var a = 2;
    var b = 0;
    while (true) {
        b = size + 1 - a;
        data_a.push(a);
        data_b.push(b);
        a += 1;
        if (b == 2) {
            break;
        }
    }
    var salida = [];
    var size = data_a.length;
    for (var i = 0; i < size; i++) {
        salida.push([data_a[i], data_b[i]]);
        //console.log(data_a[i] + "-" + data_b[i]);
    }

    return salida;
}

generaTresPuntos = function (size) {
    var data_a = [];
    var data_b = [];
    var data_c = [];
    var a = 2;
    var b = 2;
    var c = 0;
    var parcial;
    externo: while (true) {
        parcial = size + 2 - a;
        interno: while (true) {
            c = parcial - b;
            data_a.push(a);
            data_b.push(b);
            data_c.push(c);
            if (b == 2 && c == 2) {
                break externo;
            }
            b++;
            if (c <= 2) {
                b = 2;
                c = 0;
                break interno;
            }
        }
        a++;
    }
    var salida = [];
    var size = data_a.length;
    for (var i = 0; i < size; i++) {
        salida.push([data_a[i], data_b[i], data_c[i]]);
        //console.log(data_a[i] + "-" + data_b[i] + "-" + data_c[i]);
    }
    return salida;
}

generaDosPuntosCustomizados = function () {
    var salida = [];
    var numTramosDeUno = 0, numTramosDeDos = 0;

    $("select[name ='cboTramoPunto']").each(function () {
        if ($(this).is(":visible")) {
            var tramo = parseInt($(this).val());
            if (tramo == 1) { numTramosDeUno++; }
            else if (tramo == 2) { numTramosDeDos++; }
        } 
    });
    numTramosDeDos++;
    salida.push([numTramosDeUno, numTramosDeDos]);
    //console.log(numTramosDeUno + "-" + numTramosDeDos);
    return salida;
}

generaTresPuntosCustomizados = function () {
    var salida = [];
    var numTramosDeUno = 0, numTramosDeDos = 0, numTramosDeTres = 0;
    $("select[name ='cboTramoPunto']").each(function () {
        if ($(this).is(":visible")) {
            var tramo = parseInt($(this).val());
            if (tramo == 1) { numTramosDeUno++; }
            else if (tramo == 2) { numTramosDeDos++; }
            else if (tramo == 3) { numTramosDeTres++; }
        }
    });
    numTramosDeDos++;
    numTramosDeTres++;
    salida.push([numTramosDeUno, numTramosDeDos, numTramosDeTres]);
    //console.log(numTramosDeUno + "-" + numTramosDeDos + "-" + numTramosDeTres);
    return salida;
}

interacionDosTramosGenerico = function (data, datos_x, datos_y) {
    var tamano = datos_x.length;
    var salida = [];

    var numPosibilidades;
    if (esPersonalizado) {
        numPosibilidades = generaDosPuntosCustomizados();
    } else {
        numPosibilidades = generaDosPuntos(datos_x.length);
    }

    for (var i = 0; i < numPosibilidades.length; i++) {
        var aux = numPosibilidades[i];
    }

    //los datos de la primera recta
    var array_x_a = [];
    var array_y_a = [];
    var array_e_a = [];

    //los datos de la segunda recta
    var array_x_b = [];
    var array_y_b = [];
    var array_e_b = [];

    //los datos de la suma de correlaciones
    var array_suma = [];

    for (var i = 0; i < numPosibilidades.length; i++) {
        var aux_x_a = [];
        var aux_y_a = [];

        //datos del primer tramo
        for (var m = 0; m < numPosibilidades[i][0]; m++) {
            aux_x_a.push(datos_x[m]);
            aux_y_a.push(datos_y[m]);
        }

        var e1 = coeficienteCorrelacion(aux_x_a, aux_y_a);
        var m1 = valorLinealM(aux_x_a, aux_y_a);
        var b1 = valorLinealB(aux_x_a, aux_y_a);

        //datos de la segundo tramo
        var aux_x_b = [];
        var aux_y_b = [];

        for (var m = 0; m < numPosibilidades[i][1]; m++) {
            aux_x_b.push(datos_x[numPosibilidades[i][0] - 1 + m]);
            aux_y_b.push(datos_y[numPosibilidades[i][0] - 1 + m]);
        }

        var e2 = coeficienteCorrelacion(aux_x_b, aux_y_b);
        var m2 = valorLinealM(aux_x_b, aux_y_b);
        var b2 = valorLinealB(aux_x_b, aux_y_b);

        //if (esPersonalizado || (m1 < m2)) {
        if (m1 < m2) {
            array_x_a.push(aux_x_a);
            array_y_a.push(aux_y_a);
            array_e_a.push(e1);

            array_x_b.push(aux_x_b);
            array_y_b.push(aux_y_b);
            array_e_b.push(e2);

            var suma = e1 + e2;
            array_suma.push(suma);
        }
    }

    if (array_suma.length == 0) {
        return null;
    }

    var posMax = posMay(array_suma);
    var suma = array_suma[posMax];

    var m1 = valorLinealM(array_x_a[posMax], array_y_a[posMax]);
    var b1 = valorLinealB(array_x_a[posMax], array_y_a[posMax]);
    var e1 = coeficienteCorrelacion(array_x_a[posMax], array_y_a[posMax]);

    var m2 = valorLinealM(array_x_b[posMax], array_y_b[posMax]);
    var b2 = valorLinealB(array_x_b[posMax], array_y_b[posMax]);
    var e2 = coeficienteCorrelacion(array_x_b[posMax], array_y_b[posMax]);

    data.push(m1);
    data.push(b1);
    data.push(e1);

    data.push(m2);
    data.push(b2);
    data.push(e2);

    var x0 = 0;
    var y0 = valorLinealM(array_x_a[posMax], array_y_a[posMax]) * x0 + valorLinealB(array_x_a[posMax], array_y_a[posMax]);

    var x1 = (b2 - b1) / (m1 - m2);
    var y1 = (m1 * b2 - m2 * b1) / (m1 - m2);

    var xn = datos_x[datos_x.length - 1];
    var yn = valorLinealM(array_x_b[posMax], array_y_b[posMax]) * xn + valorLinealB(array_x_b[posMax], array_y_b[posMax]);

    salida.push([x0, y0]);
    salida.push([x1, y1]);
    salida.push([xn, yn]);

    return salida;
}

interacionTresTramosGenerico = function (data, datos_x, datos_y) {
    var salida = [];
    var numPosibilidades;
    if (esPersonalizado) {
        numPosibilidades = generaTresPuntosCustomizados();
    } else {
        numPosibilidades = generaTresPuntos(datos_x.length);
    }

    //los datos de la primera recta
    var array_x_a = [];
    var array_y_a = [];
    var array_e_a = [];

    //los datos de la segunda recta
    var array_x_b = [];
    var array_y_b = [];
    var array_e_b = [];

    //los datos de la tercera recta
    var array_x_c = [];
    var array_y_c = [];
    var array_e_c = [];

    //los datos de la suma de correlaciones
    var array_suma = [];

    for (var i = 0; i < numPosibilidades.length; i++) {
        var aux_x_a = [];
        var aux_y_a = [];
        var size1 = numPosibilidades[i][0];

        //datos del primer tramo
        for (var m = 0; m < size1; m++) {
            aux_x_a.push(datos_x[m]);
            aux_y_a.push(datos_y[m]);
        }

        var e1 = coeficienteCorrelacion(aux_x_a, aux_y_a);
        var m1 = valorLinealM(aux_x_a, aux_y_a);
        var b1 = valorLinealB(aux_x_a, aux_y_a);

        //datos de la segundo tramo
        var aux_x_b = [];
        var aux_y_b = [];
        var size2 = numPosibilidades[i][1];

        for (var m = 0; m < size2; m++) {
            aux_x_b.push(datos_x[numPosibilidades[i][0] - 1 + m]);
            aux_y_b.push(datos_y[numPosibilidades[i][0] - 1 + m]);
        }

        var e2 = coeficienteCorrelacion(aux_x_b, aux_y_b);
        var m2 = valorLinealM(aux_x_b, aux_y_b);
        var b2 = valorLinealB(aux_x_b, aux_y_b);

        //datos del tercer tramo
        var aux_x_c = [];
        var aux_y_c = [];
        var size3 = numPosibilidades[i][2];

        for (var m = 0; m < size3; m++) {
            aux_x_c.push(datos_x[numPosibilidades[i][0] + numPosibilidades[i][1] - 2 + m]);
            aux_y_c.push(datos_y[numPosibilidades[i][0] + numPosibilidades[i][1] - 2 + m]);
        }

        var e3 = coeficienteCorrelacion(aux_x_c, aux_y_c);
        var m3 = valorLinealM(aux_x_c, aux_y_c);
        var b3 = valorLinealB(aux_x_c, aux_y_c);

        //if (esPersonalizado || (m1 < m2 && m2 < m3)) {
        if  (m1 < m2 && m2 < m3) {
            array_x_a.push(aux_x_a);
            array_y_a.push(aux_y_a);
            array_e_a.push(e1);

            array_x_b.push(aux_x_b);
            array_y_b.push(aux_y_b);
            array_e_b.push(e2);

            array_x_c.push(aux_x_c);
            array_y_c.push(aux_y_c);
            array_e_c.push(e3);

            var suma = e1 + e2 + e3;
            array_suma.push(suma);
        }
    }

    if (array_suma.length == 0) {
        return null;
    }

    var posMax = posMay(array_suma);

    var suma = array_suma[posMax];
    var m1 = valorLinealM(array_x_a[posMax], array_y_a[posMax]);
    var b1 = valorLinealB(array_x_a[posMax], array_y_a[posMax]);
    var e1 = coeficienteCorrelacion(array_x_a[posMax], array_y_a[posMax]);

    var m2 = valorLinealM(array_x_b[posMax], array_y_b[posMax]);
    var b2 = valorLinealB(array_x_b[posMax], array_y_b[posMax]);
    var e2 = coeficienteCorrelacion(array_x_b[posMax], array_y_b[posMax]);

    var m3 = valorLinealM(array_x_c[posMax], array_y_c[posMax]);
    var b3 = valorLinealB(array_x_c[posMax], array_y_c[posMax]);
    var e3 = coeficienteCorrelacion(array_x_c[posMax], array_y_c[posMax]);

    data.push(m1);
    data.push(b1);
    data.push(e1);

    data.push(m2);
    data.push(b2);
    data.push(e2);

    data.push(m3);
    data.push(b3);
    data.push(e3);

    var x0 = 0;
    var y0 = valorLinealM(array_x_a[posMax], array_y_a[posMax]) * x0 + valorLinealB(array_x_a[posMax], array_y_a[posMax]);

    var x1 = (b2 - b1) / (m1 - m2);
    var y1 = (m1 * b2 - m2 * b1) / (m1 - m2);

    var x2 = (b3 - b2) / (m2 - m3);
    var y2 = (m2 * b3 - m3 * b2) / (m2 - m3);

    var xn = datos_x[datos_x.length - 1];
    var yn = valorLinealM(array_x_c[posMax], array_y_c[posMax]) * xn + valorLinealB(array_x_c[posMax], array_y_c[posMax]);

    salida.push([x0, y0]);
    salida.push([x1, y1]);
    salida.push([x2, y2]);
    salida.push([xn, yn]);

    return salida;
}

mostrarCurvaEnsayoSimple = function (chartContent, dataEnsayo) {

    $(chartContent).highcharts({
        title: {
            text: ""
        },
        yAxis: {
            title: {
                text: 'Consumo[gal/h,m3/h,kg/h]',
                style: {
                    color: '#009900'
                }
            },
        },
        xAxis: {
            title: {
                text: 'Potencia (MW)',
                style: {
                    color: '#009900'
                }
            },
        },
        legend: {
            layout: 'horizontal',
            align: 'center',
            verticalAlign: 'top',
            itemMarginTop: 25,
            borderWidth: 0
        },
        series: [{
            name: 'Curva Ensayo de Pe',
            data: dataEnsayo
        }],
        colors: ['#FF7A33'],
        chart: {
            backgroundColor: {
                linearGradient: [0, 0, 0, 400],
                stops: [
                    [0, '#FFFFFF'],
                    [1, '#E6E6E6']
                ]
            },
            borderColor: '#E6E6E6',
            borderWidth: 2,
            className: 'dark-container',
            plotBackgroundColor: '#FFFFFF',
            plotBorderColor: '#E6E6E6',
            plotBorderWidth: 1
        }
    });
}

mostrarCurvaDoble = function (chartContent, dataEnsayo, dataAjustada) {
    $(chartContent).highcharts({
        title: {
            text: ""
        },
        yAxis: {
            title: {
                text: 'Consumo[gal/h,m3/h,kg/h]',
                style: {
                    color: '#009900'
                }
            },
        },
        xAxis: {
            title: {
                text: 'Potencia (MW)',
                style: {
                    color: '#009900'
                }
            },
        },
        legend: {
            layout: 'horizontal',
            align: 'center',
            verticalAlign: 'top',
            itemMarginTop: 25,
            borderWidth: 0
        },
        series: [{
            name: 'Curva ajustada SPR',
            data: dataAjustada
        }, {
            name: 'Curva Ensayo de Pe',
            data: dataEnsayo
        }],
        colors: ['#33B8FF', '#FF7A33'],
        chart: {
            backgroundColor: {
                linearGradient: [0, 0, 0, 400],
                stops: [
                    [0, '#FFFFFF'],
                    [1, '#E6E6E6']
                ]
            },
            borderColor: '#E6E6E6',
            borderWidth: 2,
            className: 'dark-container',
            plotBackgroundColor: '#FFFFFF',
            plotBorderColor: '#E6E6E6',
            plotBorderWidth: 1
        }
    });
}

registrarCurva = function () {
    //if ($('#txtNCP').val() == "") {
    //    mostrarMensajeEditar("Ingrese un valor en la casilla NCP.");
    //    return;
    //}

    //if (IsOnlyDigit($('#txtNCP').val()) == false) {
    //    mostrarMensajeEditar("Ingrese un valor numerico valido en la casilla NCP.");
    //    return;
    //}

    $.ajax({
        type: 'POST',
        url: controlador + 'SaveGrupodat',
        data: {
            iGrupoCodi: global_gruposCodi,
            sValor: puntosgrabar,
            sNcp: $('#txtNCP').val(),
            sFechaDat: $('#txtFechaVigencia').val()
        },
        dataType: 'json',
        success: function (result) {
            if (result == 1) {
                mostrarMensajeEditar("Datos Guardados correctamente.");

                setTimeout(function () {
                    $('#popupEditarCurva').bPopup().close();

                }, 50);

            } else if (result == 2) {
                mostrarMensajeEditar("Ya existe un elemento con los parametros indicados.");
            } else {
                mostrarError();
            }
        },
        error: function () {
            mostrarError();
        }
    });
}



function IsOnlyDigit(texto) {
    for (var i = 0; i < texto.length; i++)
        if (!(IsDigito(texto[i]) || texto[i] == ' ' || texto[i] == '+' || texto[i] == '(' || texto[i] == ')' || texto[i] == '-'))
            return false;
    return true;
}

function IsDigito(caracter) {
    if (caracter == '0' || caracter == '1' || caracter == '2' || caracter == '3' || caracter == '4' || caracter == '5' || caracter == '6' || caracter == '7' || caracter == '8' || caracter == '9')
        return true;
    return false;
}

// Fin - Curva Consumo