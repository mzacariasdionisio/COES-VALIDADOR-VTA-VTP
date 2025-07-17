var controlador = siteRoot + 'cortoplazo/congestion/';
var hot = null;
var hotProgramado = null;
var arregloGrupoLinea = [];
var arregloGrupoLineaMinimo = [];
var arregloLinea = [];
var arregloTrafo2d = [];
var arregloTrafo3d = [];
var arregloGeneral = [];
var objPosSelecTmp = _inicializarObjetoPosicion();
$(function () {

    $('#FechaDesde').Zebra_DatePicker({
        onSelect: function () {
            consultar();
        }
    });

    $('#FechaHasta').Zebra_DatePicker({
    });

    $('#btnVer').on('click', function () {
        consultar();
    });

    $('#btnCancelar').on('click', function () {
        cancelar();
    });

    $('#btnCancelar2').on('click', function () {
        cancelar2();
    });

    $('#btnGrabar').on('click', function () {
        grabar();
    });

    $(document).ready(function () {
        consultar();
    });

    $('#btnAgregarGrupo').on('click', function () {
        agregarGrupoDespacho();
    });

    $('#btnAceptarGrupo').on('click', function () {
        grabarGrupoDespacho();
    });
    
});

cancelar = function () {
    document.location.href = controlador + "listamanual";
};

cancelar2 = function () {
    document.location = document.location;
};

consultar = function () {

    var fechaini = $('#FechaDesde').val();

    if (hot != null) {
        hot.destroy();
    }
    var container = document.getElementById('contenedor');

    //congestion ejecutada
    cargarGrilla(fechaini);

    //congestion programada (PDO/RDO) que pueden pasar a ejecutado
    cargarGrillaProgramado();
};

var dropdownTipoRenderer = function (instance, td, row, col, prop, value, cellProperties) {
    var selectedId;

    for (var index = 0; index < arregloFuente.length; index++) {
        if (parseInt(value) === arregloFuente[index].id) {
            selectedId = arregloFuente[index].id;
            value = arregloFuente[index].text;
        }
    }

    $(td).addClass("estilocombo");
    Handsontable.TextCell.renderer.apply(this, arguments);
    $('#selectedId').text(selectedId);
};

var dropdownEquipoRenderer = function (instance, td, row, col, prop, value, cellProperties) {
    var selectedId;
    var tipo = "Línea";
    for (var index = 0; index < arregloGeneral.length; index++) {
        if (parseInt(value) === arregloGeneral[index].id) {
            selectedId = arregloGeneral[index].id;
            value = arregloGeneral[index].text;
        }
    }

    $(td).addClass("estilocombo");
    Handsontable.TextCell.renderer.apply(this, arguments);
    $('#selectedId').text(selectedId);

};

var fechahoraRenderer = function (instance, td, row, col, prop, value, cellProperties) {
    var selectedId;
    var r = /^(((0[1-9]|[12]\d|3[01])[\/\.-](0[13578]|1[02])[\/\.-]((19|[2-9]\d)\d{2})\s(0[0-9]|1[0-9]|2[0-3]):([0-5][0-9]):([0-5][0-9]))|((0[1-9]|[12]\d|30)[\/\.-](0[13456789]|1[012])[\/\.-]((19|[2-9]\d)\d{2})\s(0[0-9]|1[0-9]|2[0-3]):([0-5][0-9]):([0-5][0-9]))|((0[1-9]|1\d|2[0-8])[\/\.-](02)[\/\.-]((19|[2-9]\d)\d{2})\s(0[0-9]|1[0-9]|2[0-3]):([0-5][0-9]):([0-5][0-9]))|((29)[\/\.-](02)[\/\.-]((1[6-9]|[2-9]\d)(0[48]|[2468][048]|[13579][26])|((16|[2468][048]|[3579][26])00))\s(0[0-9]|1[0-9]|2[0-3]):([0-5][0-9]):([0-5][0-9])))$/g;

    var value = instance.getDataAtCell(row, col);
    var td = instance.getCell(row, col);

    var esfechahora = r.test(value);

    if (!esfechahora) {
        td.style.backgroundColor = 'red';
    }
    else {
        td.style.background = '';
    }  
};

var validarFechaHora = function (fecha) {
    var r = /^(((0[1-9]|[12]\d|3[01])[\/\.-](0[13578]|1[02])[\/\.-]((19|[2-9]\d)\d{2})\s(0[0-9]|1[0-9]|2[0-3]):([0-5][0-9]):([0-5][0-9]))|((0[1-9]|[12]\d|30)[\/\.-](0[13456789]|1[012])[\/\.-]((19|[2-9]\d)\d{2})\s(0[0-9]|1[0-9]|2[0-3]):([0-5][0-9]):([0-5][0-9]))|((0[1-9]|1\d|2[0-8])[\/\.-](02)[\/\.-]((19|[2-9]\d)\d{2})\s(0[0-9]|1[0-9]|2[0-3]):([0-5][0-9]):([0-5][0-9]))|((29)[\/\.-](02)[\/\.-]((1[6-9]|[2-9]\d)(0[48]|[2468][048]|[13579][26])|((16|[2468][048]|[3579][26])00))\s(0[0-9]|1[0-9]|2[0-3]):([0-5][0-9]):([0-5][0-9])))$/g;        
    var esfechahora = r.test(fecha);
    return esfechahora;
};

var eliminarCongestion = function (id) {

    $.ajax({
        type: 'POST',
        url: controlador + 'ejecutadodelete',
        datatype: 'json',
        data: {
            idCongestion: id
        },
        success: function (result) {

        },
        error: function (jqXHR, textStatus, errorThrown) {
            mostrarError();
            console.log('Error:', jqXHR);
        }
    });
};

cargarGrilla = function (fechaini) {

    $.ajax({
        type: 'POST',
        url: controlador + 'obtenerlineatrafogrupoconfigurado',
        datatype: 'json',
        data: {
            fechaini: fechaini
        },
        success: function (result) {

            var dataGrupoLinea = result.ListGrupoLinea;
            var dataLinea = result.ListLineaEquipo;
            var dataTrafo2d = result.ListTrafo2dEquipo;
            var dataTrafo3d = result.ListTrafo3dEquipo;
            var dataRegionSeguridad = result.ListaRegionSeguridad;
            var dataGrupoLineaMinimo = result.ListaGrupoLineaMinimo;

            arregloGrupoLinea = [];
            arregloLinea = [];
            arregloTrafo2d = [];
            arregloTrafo3d = [];
            arregloRegionSeguridad = [];
            arregloGeneral = [];
            arregloGrupoLineaMinimo = [];

            for (var j in dataGrupoLinea) {
                arregloGrupoLinea.push({ id: dataGrupoLinea[j].Grulincodi, text: dataGrupoLinea[j].Grulinnombre });
                arregloGeneral.push({ id: dataGrupoLinea[j].Grulincodi, text: "Grupo Línea : " + dataGrupoLinea[j].Grulinnombre });
            }

            for (var j in dataRegionSeguridad) {
                //console.log(dataRegionSeguridad[j]);
                arregloRegionSeguridad.push({ id: dataRegionSeguridad[j].Regsegcodi, text: dataRegionSeguridad[j].Regsegnombre });
                arregloGeneral.push({ id: dataRegionSeguridad[j].Regsegcodi, text: "Región Seguridad     : " + dataRegionSeguridad[j].Regsegnombre });
            }

            for (var j in dataLinea) {
                arregloLinea.push({ id: dataLinea[j].Configcodi, text: dataLinea[j].Equinomb });
                arregloGeneral.push({ id: dataLinea[j].Configcodi, text: "Línea       : " + dataLinea[j].Equinomb });
            }

            for (var j in dataTrafo2d) {
                arregloTrafo2d.push({ id: dataTrafo2d[j].Configcodi, text: dataTrafo2d[j].Equinomb });
                arregloGeneral.push({ id: dataTrafo2d[j].Configcodi, text: "Trafo2D     : " + dataTrafo2d[j].Equinomb });
            }

            for (var j in dataTrafo3d) {
                arregloTrafo3d.push({ id: dataTrafo3d[j].Configcodi, text: dataTrafo3d[j].Equinomb });
                arregloGeneral.push({ id: dataTrafo3d[j].Configcodi, text: "Trafo3D     : " + dataTrafo3d[j].Equinomb });
            }

            for (var j in dataGrupoLineaMinimo) {
                arregloGrupoLineaMinimo.push({ id: dataGrupoLineaMinimo[j].Grulincodi, text: dataGrupoLineaMinimo[j].Grulinnombre });
                arregloGeneral.push({ id: dataGrupoLineaMinimo[j].Grulincodi, text: "Grupo Línea Flujo Min: " + dataGrupoLineaMinimo[j].Grulinnombre });
            }

            var container = document.getElementById('contenedor');

            //datos del listado procesado
            var data = result.Datos;

            var hotOptions = {
                data: data,                
                colHeaders: ['N°', 'Tipo: Equipo', 'Desde', 'Hasta', 'Observación', '', ''],
                columns: [
                    { type: 'numeric', readOnly: true },
                    {
                    },
                    {
                        type: 'time',
                        timeFormat: 'HH:mm',
                        correctFormat: true
                    },
                    {
                        type: 'time',
                        timeFormat: 'HH:mm',
                        correctFormat: true
                    },
                    {},
                    {
                        readOnly: true
                    },
                    {
                    }
                ],
                rowHeaders: true,
                comments: true,
                height: 400,
                colWidths: [40, 400, 70, 70, 310, 1, 1],
                cells: function (row, col, prop) {

                    var cellProperties = {};

                    if (col == 1) {
                        cellProperties.editor = 'select2';
                        cellProperties.renderer = dropdownEquipoRenderer;
                        cellProperties.width = "400px";
                        cellProperties.select2Options = {
                            data: arregloGeneral,//arregloLinea,//arregloGrupoLinea,
                            dropdownAutoWidth: true,
                            width: 'resolve',
                            allowClear: false,
                        };
                    }

                    return cellProperties;
                },
                afterLoadData: function () {
                    this.render();
                },
                beforeRemoveRow: function (index, amount) {
                    console.log('beforeRemove: index: %d, amount: %d', index, amount);
                    //eliminando filas

                    var codigos = "";
                    for (var i = amount; i > 0; i--) {

                        var valorCelda = hot.getDataAtCell(index + i - 1, 0);

                        if (valorCelda != "" && valorCelda != null) {
                            eliminarCongestion(valorCelda);
                            console.log('id eliminado: ' + valorCelda);

                        }
                    }
                },
                stretchH: 'all'
            };

            hot = new Handsontable(container, hotOptions);


            hot.updateSettings({
                contextMenu: {
                    callback: function (key, options) {
                        if (key === 'about') {
                            setTimeout(function () {
                                alert("This is a context menu with default and custom options mixed");
                            }, 100);
                        }
                        if (key === "properties_row") {

                            var fila = hot.getSelected()[0];
                            var valor = hot.getDataAtCell(hot.getSelected()[0], 6);                            

                            $('#hfFila').val(fila);
                            $('#hfCodigosDespacho').val(valor);
                            $('#popupGrupo').bPopup({

                            });

                            var array = valor.split('#');

                            $("#tablaGrupo > tbody").html("");

                            for (var j in array) {
                                $("#cbGrupo > option").each(function () {

                                    if (this.value != "") {
                                        if (this.value === array[j]) {
                                            $('#tablaGrupo> tbody').append(
                                                '<tr>' +
                                                '   <td>' + this.value + '</td>' +
                                                '   <td>' + this.text + '</td>' +
                                                '   <td style="text-align:center">' +
                                                '       <img src="' + siteRoot + 'Content/Images/btn-cancel.png" alt="" onclick="$(this).parent().parent().remove();" style="cursor:pointer" />' +
                                                '   </td>' +
                                                '</tr>'
                                            );
                                        }
                                    }   
                                });
                            }                            
                        }
                    },
                    items: {
                        "row_below": {
                            name: function () {
                                return " <div class= 'icon-agregar' >Agregar fila</div> ";
                            }
                        },
                        "remove_row": {
                            name: function () {
                                return " <div class= 'icon-eliminar' >Eliminar fila</div> ";
                            }
                        },
                        "properties_row": {
                            name: function () {
                                return " <div class= 'icon-properties' >Grupos de Despacho</div> ";
                            },
                            disabled: function () {
                                return (hot.getDataAtCell(hot.getSelected()[0], 1) === "");
                            }
                        }
                    }
                }
            });

        },
        error: function (jqXHR, textStatus, errorThrown) {
            mostrarError();
            console.log('Error:', jqXHR);
        }
    });

};

agregarGrupoDespacho = function () {

    var id = $('#cbGrupo').val();
    var text = $('#cbGrupo :selected').text();
    var flag = true;

    $('#tablaGrupo>tbody tr').each(function (i) {
        $punto = $(this).children('td:first').text();
        if (id === $punto) {
            flag = false;
        }
    });

    if (flag) {

        $('#tablaGrupo> tbody').append(
            '<tr>' +
            '   <td>' + id + '</td>' +
            '   <td>' + text + '</td>' +
            '   <td style="text-align:center">' +
            '       <img src="' + siteRoot + 'Content/Images/btn-cancel.png" alt="" onclick="$(this).parent().parent().remove();" style="cursor:pointer" />' +
            '   </td>' +
            '</tr>'
        );
    }
};

grabarGrupoDespacho = function () {
    var count = 0;
    var items = "";
    $('#tablaGrupo>tbody tr').each(function (i) {
        $punto = $(this).children('td:first').text();
        var constante = (count > 0) ? "#" : "";
        items = items + constante + $punto;
        count++;
    });
    var fila = $('#hfFila').val();
    hot.setDataAtCell(fila, 6, items);
    $('#popupGrupo').bPopup().close();
};

cargarGrillaProgramado = function ()
{
    var fecha = $('#FechaDesde').val();
    var flagHoraTR = $('#flagHoraTR').val();
    var horaTR = $('#horaTR').val();

    $.ajax({
        type: 'POST',
        url: controlador + 'obtenercongestionprogramada',
        datatype: 'json',
        data: {
            fecha: fecha,
            flagHoraTR: flagHoraTR,
            horaTR: horaTR
        },
        success: function (result) {

            var container = document.getElementById('contenedorProgramado');
            var data = result;

            var hotOptions = {
                data: data,                
                colHeaders: ['N°', 'Tipo: Equipo', 'Desde', 'Hasta'],
                columns: [
                 { readOnly: true },
                 { readOnly: false },
                 { readOnly: true },
                 { readOnly: true }
                ],
                rowHeaders: true,
                comments: true,
                height: 400,
                colWidths: [1, 300, 70, 70],
                cells: function (row, col, prop) {

                    var cellProperties = {};

                    if (col == 1) {
                        cellProperties.editor = 'select2';
                        cellProperties.renderer = dropdownEquipoRenderer;
                        cellProperties.width = "350px";
                        cellProperties.select2Options = {
                            data: arregloGeneral,//arregloLinea,//arregloGrupoLinea,
                            dropdownAutoWidth: true,
                            width: 'resolve',
                            allowClear: false,
                        };
                    }

                    return cellProperties;
                },
                afterSelection: function (actualFila, actualColumna, finalFila, finalColumna) {
                    var vFila = actualFila > finalFila ? finalFila : actualFila;
                    var vFilaFinal = finalFila > actualFila ? finalFila : actualFila;
                    var vColumna = actualColumna > finalColumna ? finalColumna : actualColumna;
                    var vColumnaFinal = finalColumna > actualColumna ? finalColumna : actualColumna;

                    //generar array temporal
                    objPosSelecTmp = {};
                    objPosSelecTmp.rowIni = vFila;
                    objPosSelecTmp.colIni = vColumna;
                    objPosSelecTmp.rowFin = vFilaFinal;
                    objPosSelecTmp.colFin = vColumnaFinal;

                },
                stretchH: 'all'
            };

            hotProgramado = new Handsontable(container, hotOptions);
        },
        error: function (jqXHR, textStatus, errorThrown) {
            mostrarError();
            console.log('Error:', jqXHR);
        }
    });
}

function _inicializarObjetoPosicion() {
    var objPos = { rowIni: -1, colIni: 0, rowFin: -1, colFin: 0};
    return objPos;
}

copiarContenido = function () {
    var colIni_ = objPosSelecTmp.colIni;
    var colFin_ = objPosSelecTmp.colFin;
    var rowIni_ = objPosSelecTmp.rowIni;
    var rowFin_ = objPosSelecTmp.rowFin;

    if (rowIni_ < 0 || rowFin_ > rowIni_) {
        alert("Debe seleccionar una fila para realizar la acción")
    }
    else {

        var index = -1;
        var flag = false;
        for (var k = hot.countRows() - 1; k >= 0; k--) {
            if (hot.getDataAtCell(k, 1) != "") {

                if (flag == false) {
                    index = k;
                    flag = true;
                }
            }
        }

        if (index == hot.countRows() - 1) {
            hot.alter('insert_row');
        }

        var equipo = hotProgramado.getDataAtCell(rowIni_, 1);
        var inicio = hotProgramado.getDataAtCell(rowIni_, 2);
        var fin = hotProgramado.getDataAtCell(rowIni_, 3);

        hot.setDataAtCell(index + 1, 1, equipo);
        hot.setDataAtCell(index + 1, 2, inicio);
        hot.setDataAtCell(index + 1, 3, fin);

        //Inicializar nuevamente
        objPosSelecTmp = _inicializarObjetoPosicion();
    }
};

convertDateTime = function (fechahora) {

    var fh = fechahora.toString().split(" ");
    var fecha = fh[0].split("/");
    var hora = fh[1].split(":");
    var dd = fecha[0];
    var mm = fecha[1] - 1;
    var yyyy = fecha[2];
    var h = hora[0];
    var m = hora[1];
    var s = parseInt(hora[2]);

    return new Date(yyyy, mm, dd, h, m, s);
};

obtenerFecha = function (fechahora) {
    var l1 = fechahora.getDate() + "/" + (fechahora.getMonth() + 1) + "/" + fechahora.getFullYear();
    return l1;
};

obtenerHora = function (fechahora) {
    var l1 = (fechahora.getHours() < 10 ? '0' : '') + fechahora.getHours() + ":" + (fechahora.getMinutes() < 10 ? '0' : '') + fechahora.getMinutes() + ":" + (fechahora.getSeconds() < 10 ? '0' : '') + fechahora.getSeconds();
    l1 = l1.toString();
    return l1;
};

validarDetalle = function (datosDetalle) {
    var mensaje = "<ul>";
    var flag = true;

    var datos = datosDetalle;

    var cad1 = "";
    for (i = 0; i < datos.length; i++) {

        if (datos[i][1] == '') {
            break;
        }
        else {

            //revision de fecha
            var horaini = datos[i][2];
            var horafin = datos[i][3];
            if (!validarHoraMinuto(horaini) || !validarHoraMinuto(horafin) || horaini == "" || horafin == "") {
                mensaje = "Ingrese horas";
                flag = false;
            }
            else {
                //Date.parse('01/01/2011 10:20:45') > Date.parse('01/01/2011 5:10:10')
                var fechaini = "01/01/2016 " + horaini + ":00";
                var fechafin = "01/01/2016 " + horafin + ":00";
                console.log(fechaini + " " + fechafin);
                if (horafin.localeCompare("00:00") != 0) {
                    if (Date.parse(fechaini) > Date.parse(fechafin)) {
                        mensaje = "Hora inicial supera la final";
                        flag = false;
                    }
                }

            }


        }
    }

    $('#hfDatos').val(datos);

    if (flag) mensaje = "";
    return mensaje;

};

validarHoraMinuto = function (hhmm) {
    var isValid = /^([0-1]?[0-9]|2[0-4]):([0-5][0-9])(:[0-5][0-9])?$/.test(hhmm);
    return isValid;
};

grabar = function () {

    // GRABAR CABECERA    
    var datos = hot.getData();
    var mensajeDetalle = validarDetalle(datos);

    eliminarAlerta();

    if (mensajeDetalle == "") {

        //graba detalle
        var error = "";
        var mensaje = "<ul>";
        var flag = true;

        $.ajax({
            type: 'POST',
            url: controlador + "grabarcongestionmanual",
            dataType: 'json',
            data: {
                dataExcel: $('#hfDatos').val(),
                fecha: $('#FechaDesde').val()
            },
            success: function (result) {
                if (result != "-1") {
                    mostrarExito();
                    consultar();
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
        mostrarAlerta(mensajeDetalle);
    }
};

eliminarAlerta = function () {
    $('#mensaje').removeClass();
    $('#mensaje').html("");
};

mostrarError = function () {
    alert('Ha ocurrido un error.');
};

mostrarAlerta = function (mensaje) {
    $('#mensaje').removeClass();
    $('#mensaje').addClass('action-alert');
    $('#mensaje').html(mensaje);
};

mostrarExito = function () {
    $('#mensaje').removeClass();
    $('#mensaje').addClass('action-exito');
    $('#mensaje').html('La operación se realizó con éxito.');
};

