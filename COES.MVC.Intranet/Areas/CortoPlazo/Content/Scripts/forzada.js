var controlador = siteRoot + 'cortoplazo/forzada/';
var hot = null;
var arregloGrupo = [];
var arregloSubcausa = [];


$(function () {

    $('#tab-container').easytabs({
        animate: false
    });

    $('#FechaDesde').Zebra_DatePicker({
    });

    $('#FechaHasta').Zebra_DatePicker({
    });

    $('#btnVer').on('click', function () {
        consultar();
    });

    $('#btnCancelar').on('click', function () {
        cancelar();
    });

    $('#btnGrabar').on('click', function () {
        grabar();
    });


    consultar();
    consultarMaestro();

    $('#btnNuevoMaestro').on('click', function () {
        editarMaestro(0);
    });

});

cancelar = function () {
    consultar();
}

consultar = function () {

    var fechaini = $('#FechaDesde').val();

    if (hot != null) {
        hot.destroy();
    }

    var container = document.getElementById('contenedor');
    cargarGrilla(fechaini);
}

var dropdownSubcausaRenderer = function (instance, td, row, col, prop, value, cellProperties) {
    var selectedId;

    for (var index = 0; index < arregloSubcausa.length; index++) {
        if (parseInt(value) === arregloSubcausa[index].id) {
            selectedId = arregloSubcausa[index].id;
            value = arregloSubcausa[index].text;
        }
    }

    $(td).addClass("estilocombo");
    Handsontable.TextCell.renderer.apply(this, arguments);
    $('#selectedId').text(selectedId);

}

var dropdownEquipoRenderer = function (instance, td, row, col, prop, value, cellProperties) {
    var selectedId;

    for (var index = 0; index < arregloGrupo.length; index++) {
        if (parseInt(value) === arregloGrupo[index].id) {
            selectedId = arregloGrupo[index].id;
            value = arregloGrupo[index].text;
        }
    }

    $(td).addClass("estilocombo");
    Handsontable.TextCell.renderer.apply(this, arguments);
    $('#selectedId').text(selectedId);

}

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

var eliminar = function (id) {

    $.ajax({
        type: 'POST',
        url: controlador + 'genforzadadelete',
        datatype: 'json',
        data: {
            idGenforcodi: id
        },
        success: function (result) {

        },
        error: function (jqXHR, textStatus, errorThrown) {
            mostrarError();
            console.log('Error:', jqXHR);
        }
    });

}

cargarGrilla = function (fechaini) {

    $.ajax({
        type: 'POST',
        url: controlador + 'obtenergeneracionforzada',
        datatype: 'json',
        data: {
            fechaini: fechaini
        },
        success: function (result) {

            var dataGrupo = result.ListEquipo;
            var dataSubcausa = result.ListaSubCausa;

            arregloGrupo = [];
            arregloSubcausa = [];

            for (var j in dataGrupo) {
                arregloGrupo.push({ id: dataGrupo[j].Relacioncodi, text: dataGrupo[j].Equinomb });
                //console.log("Grupo: " + j + "***" + dataGrupo[j].Relacioncodi + "***" + dataGrupo[j].Equinomb);
            }

            for (var j in dataSubcausa) {
                arregloSubcausa.push({ id: dataSubcausa[j].Subcausacodi, text: dataSubcausa[j].Subcausadesc });
                //console.log("Subcausa: " + j + "***" + dataSubcausa[j].Subcausacodi + "***" + dataSubcausa[j].Subcausadesc);
            }

            var container = document.getElementById('contenedor');

            //datos del listado procesado
            var data = result.Datos;

            var hotOptions = {
                data: data,
                colHeaders: true,
                colHeaders: ['N°', 'Símbolo', 'Generador', 'Calificación', 'Desde', 'Hasta'],
                columns: [
                 { type: 'numeric', readOnly: true },
                 {
                     type: 'dropdown',
                     source: ['>', '<', '=']
                 },
                 {
                 },
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
                 }
                ],
                rowHeaders: true,
                comments: true,
                height: 860,
                colWidths: [50, 50, 350, 400, 80, 80],
                cells: function (row, col, prop) {

                    var cellProperties = {};

                    if (col == 2) {
                        cellProperties.editor = 'select2';
                        cellProperties.renderer = dropdownEquipoRenderer;
                        cellProperties.width = "350px";
                        cellProperties.select2Options = {
                            data: arregloGrupo,
                            dropdownAutoWidth: true,
                            width: 'resolve',
                            allowClear: false,
                        };
                    }
                    if (col == 3) {
                        cellProperties.editor = 'select2';
                        cellProperties.renderer = dropdownSubcausaRenderer;
                        cellProperties.width = "400px";
                        cellProperties.select2Options = {
                            data: arregloSubcausa,
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
                            eliminar(valorCelda);
                            console.log('id eliminado: ' + valorCelda);
                        }
                    }
                }
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
}

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
}

obtenerFecha = function (fechahora) {
    var l1 = fechahora.getDate() + "/" + (fechahora.getMonth() + 1) + "/" + fechahora.getFullYear();
    return l1;
}

obtenerHora = function (fechahora) {
    var l1 = (fechahora.getHours() < 10 ? '0' : '') + fechahora.getHours() + ":" + (fechahora.getMinutes() < 10 ? '0' : '') + fechahora.getMinutes() + ":" + (fechahora.getSeconds() < 10 ? '0' : '') + fechahora.getSeconds();
    l1 = l1.toString();
    return l1;
}

validarDetalle = function (datosDetalle) {
    var mensaje = "<ul>";
    var flag = true;

    var simbolo = ">,<,=";

    var datos = datosDetalle;

    var cad1 = "";
    console.log(datos.length);
    for (i = 0; i < datos.length; i++) {

        //revisión de simbolo

        if (datos[i][2] == '') {
            break;
        }
        else {

            var simboloReg = datos[i][1];
            console.log("*" + simboloReg + "*");

            if ((simbolo.indexOf(simboloReg) < 0) || simboloReg == "") {
                mensaje = "Ingrese símbolo";
                flag = false;
            }
            else {
                var calif = datos[i][3];
                if (calif == '') {
                    mensaje = "Ingrese calificación";
                    flag = false;
                } else {
                    //revision de fecha
                    var horaini = datos[i][4];
                    var horafin = datos[i][5];
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
        }
    }

    $('#hfDatos').val(datos);

    if (flag) mensaje = "";
    return mensaje;

}

validarHoraMinuto = function (hhmm) {
    var isValid = /^([0-1]?[0-9]|2[0-4]):([0-5][0-9])(:[0-5][0-9])?$/.test(hhmm);
    return isValid;
}

grabar = function () {

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
            url: controlador + "grabarforzada",
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
}



/**==== Funciones para la tabla maestro ====*/

consultarMaestro = function () {
    $.ajax({
        type: 'POST',
        url: controlador + 'maestrolist',
        success: function (evt) {
            $('#listadoMaestro').html(evt);
            $('#tablaMaestro').dataTable({
            });
        },
        error: function () {
            mostrarMensaje('mensajeListMaestro', 'error', 'Se produjo un error.');
        }
    });
}

editarMaestro = function (id) {
    $('#mensajeListMaestro').removeClass();
    $('#mensajeListMaestro').text("");

    $.ajax({
        type: 'POST',
        url: controlador + 'maestroedit',
        data: {
            idMaestro: id
        },
        global: false,
        success: function (evt) {
            $('#contenidoMaestro').html(evt);
            setTimeout(function () {
                $('#popupMaestro').bPopup({
                    autoClose: false
                });
            }, 50);

            $('#cbGeneradorMaestro').val($('#hfGeneradorMaestro').val());
            $('#cbSimboloMaestro').val($('#hfSimboloMaestro').val());
            $('#cbTipoMaestro').val($('#hfTipoMaestro').val());
            $('#cbEstadoMaestro').val($('#hfEstadoMaestro').val());
            $('#cbSubcausaevento').val($('#hfSubcausaevento').val());

            $('#btnGrabarMaestro').on("click", function () {
                grabarMaestro();
            });

            $('#btnCancelarMaestro').on("click", function () {
                $('#popupMaestro').bPopup().close();
            });

        },
        error: function () {
            mostrarMensaje('mensajeListMaestro', 'error', 'Se ha producido un error.');
        }
    });
}

eliminarMaestro = function (id) {
    if (confirm('¿Está seguro de realizar esta operación?')) {
        $.ajax({
            type: 'POST',
            url: controlador + 'maestrodelete',
            data: {
                idMaestro: id
            },
            dataType: 'json',
            success: function (result) {
                if (result == 1) {
                    mostrarMensaje('mensajeListMaestro', 'exito', 'La operación se realizó correctamente.');
                    consultarMaestro();
                }
                else {
                    mostrarMensaje('mensajeListMaestro', 'error', 'Se ha producido un error.');
                }
            },
            error: function () {
                mostrarMensaje('mensajeListMaestro', 'error', 'Se ha producido un error.');
            }
        });
    }
}

grabarMaestro = function () {
    var validacion = validarMaestro();

    if (validacion == "") {
        $.ajax({
            type: 'POST',
            url: controlador + 'maestrosave',
            data: $('#frmRegistroMaestro').serialize(),
            dataType: 'json',
            global: false,
            success: function (result) {
                if (result == 1) {
                    mostrarMensaje('mensajeListMaestro', 'exito', 'Los datos se grabaron correctamente.');
                    $('#popupMaestro').bPopup().close();
                    consultarMaestro();
                }
                else if (result == 2) {
                    mostrarMensaje('mensajeEditMaestro', 'alert', 'Ya existe un registro con el mismo generador.');
                }
                else {
                    mostrarMensaje('mensajeEditMaestro', 'error', 'Se ha producido un error.');
                }
            },
            error: function () {
                mostrarMensaje('mensajeEditMaestro', 'error', 'Se ha producido un error.');
            }
        });
    }
    else {
        mostrarMensaje('mensajeEditMaestro', 'alert', validacion);
    }
}

validarMaestro = function () {
    var mensaje = "<ul>";
    var flag = true;

    if ($('#cbGeneradorMaestro').val() == '-1') {
        mensaje = mensaje + "<li>Por favor seleccione el generador asociado.</li>";
        flag = false;
    }

    if ($('#cbSimboloMaestro').val() == '') {
        mensaje = mensaje + "<li>Por favor seleccione el símbolo a utilizar.</li>";
        flag = false;
    }

    if ($('#cbTipoMaestro').val() == '') {
        mensaje = mensaje + "<li>Por favor seleccione un tipo.</li>";
        flag = false;
    }

    if ($('#cbEstadoMaestro').val() == '') {
        mensaje = mensaje + "<li>Por favor seleccione el estado.</li>";
        flag = false;
    }

    if ($('#cbSubcausaevento').val() == '') {
        mensaje = mensaje + "<li>Por favor seleccione la calificación.</li>";
        flag = false;
    }

    mensaje = mensaje + "</ul>";

    if (flag) {
        mensaje = "";
    }

    return mensaje;
}

eliminarAlerta = function () {
    $('#mensaje').removeClass();
    $('#mensaje').html("");
}

mostrarError = function () {
    alert('Ha ocurrido un error.');
}


mostrarAlerta = function (mensaje) {
    $('#mensaje').removeClass();
    $('#mensaje').addClass('action-alert');
    $('#mensaje').html(mensaje);
}

mostrarExito = function () {
    $('#mensaje').removeClass();
    $('#mensaje').addClass('action-exito');
    $('#mensaje').html('La operación se realizó con éxito.');
}

