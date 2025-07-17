var controlador = siteRoot + 'Sorteo/Sorteo/'

$(function () {
    $('#mensaje').hide();
    $('#btnJugarSorteo').hide();

    sorteoLoad();
    $(document).on("click", "#btnIniciarSorteo", function () {
        popupDatos();
    });
    //$("#btnJugarSorteo").unbind("click");
    $(document).on("click", "#btnJugarSorteo", function () {
        jugarSorteo();
    });

    $(document).on("click", "#btnAceptarSorteo", function () {
        AceptarSorteo();
    });

    $("#txtCoordinador").on('input', function (e) {

        if (($('#txtCoordinador').val()).length > 2) {
            $("#msg").hide();
        } else {
            $("#msg").show();
        }

        if ($("#chkMantto").is(':checked')
            && $("#chkHoras").is(':checked')
            && ($('#txtCoordinador').val()).length > 2
            && ($('#txtDocoes').val()).length > 2) {
            $('#btnJugarSorteo').show();
        } else {
            $('#btnJugarSorteo').hide();
        }
    });

    $("#txtDocoes").on('input', function (e) {

        if (($('#txtDocoes').val()).length > 2) {
            $("#msg").hide();
        } else {
            $("#msg").show();
        }

        if ($("#chkMantto").is(':checked')
            && $("#chkHoras").is(':checked')
            && ($('#txtCoordinador').val()).length > 2
            && ($('#txtDocoes').val()).length > 2) {
            $('#btnJugarSorteo').show();
        } else {
            $('#btnJugarSorteo').hide();
        }
    });


    $("#chkMantto").change(function () {
        if ($(this).is(':checked')) {
            $('#lblUpdteChkMantto').show();
        } else {
            $('#lblUpdteChkMantto').hide();
        }

        if ($("#chkMantto").is(':checked')
            && $("#chkHoras").is(':checked')
            && ($('#txtCoordinador').val()).length > 2
            && ($('#txtDocoes').val()).length > 2) {
            $('#btnJugarSorteo').show();
        } else {
            $('#btnJugarSorteo').hide();
        }

    })

    $("#chkHoras").change(function () {
        if ($(this).is(':checked')) {
            $('#lblUpdteChkHoras').show();
        } else {
            $('#lblUpdteChkHoras').hide();
        }

        if ($("#chkMantto").is(':checked')
            && $("#chkHoras").is(':checked')
            && ($('#txtCoordinador').val()).length > 2
            && ($('#txtDocoes').val()).length > 2) {
            $('#btnJugarSorteo').show();
        } else {
            $('#btnJugarSorteo').hide();
        }
    })

    $(document).on("click", "#btnCancelarSorteo", function () {
        LimpiarPopUp();
        $('#popupDatos').bPopup().close();
        $("#required").hide();
    });

    $('.close').click(function () {
        $("#txtCoordinador").val("");
        $("#txtDocoes").val("");
        $("#chkMantto").prop("checked", false);
        $("#chkHoras").prop("checked", false);
    });
})

$('.close').click(function () {
    $("#txtCoordinador").val("");
    $("#txtDocoes").val("");
    $("#chkMantto").prop("checked", false);
    $("#chkHoras").prop("checked", false);
});

var b_hay_pruebahoy = true;

sorteoLoad = function () {
    obtenerFecha();
    TotalConteoTipoXEQTotal();
    TotalConteoTipoXEQ();
    TotalConteoTipoXNO();
    diasFaltantes();
    cantidadPruebas();
    obtenerHistoricoSorteo();
    listarAreas();
}

//function validarInput() {
//    if (($('#txtCoordinador').val()).length > 2 && ($('#txtDocoes').val()).length > 2) {
//        $("#msg").hide();
//        return true;
//    } else {
//        $("#msg").show();
//        return false;
//    }
//}

/*function jugarSorteo() {

    console.log("entro jugarSorteo b_hay_pruebahoy -> " + b_hay_pruebahoy);

    if (!b_hay_pruebahoy) {
        $('#mensaje').show();
        $('#mensaje').removeClass();
        $('#mensaje').addClass('action-message');
        $('#mensaje').html("Ya hubo sorteo el dia de hoy");
        $("#btnIniciarSorteo").hide();

        setTimeout(function () {
            $('#popupDatos').bPopup().close();
            LimpiarPopUp();
        }, 50);

        return;

    } else {
        $("#btnIniciarSorteo").show();
    }

    var coordinador = $("#txtCoordinador").val();
    var docoes = $("#txtDocoes").val();
    wf_log("Ingreso al modulo Sorteo", "INI", coordinador, docoes);
    diaActual = $("#diaActual").val();
    ultimodia = $("#ultimoDia").val();
    diasSorteo = [];
    diasSeleccionado = [];
    var prueba = Number($("#pruebas").val());

    console.log("prueba => " + prueba);


    for (var i = diaActual; i <= ultimodia; i++) {
        diasSorteo.push(Number(i));
    }

    var length = Number($("#diasFaltantes").val());

    console.log("entro 1");

    if (length == 1) {
        switch (prueba) {

            case 1: {
                var randomItem1 = diasSorteo[Math.floor(Math.random() * diasSorteo.length)];
                diasSorteo = diasSorteo.filter(number => number != Number(randomItem1));

                diasSeleccionado.push(randomItem1);
            } break;
        }
    }
    else if (length == 2) {
        switch (prueba) {

            case 1: {
                var randomItem1 = diasSorteo[Math.floor(Math.random() * diasSorteo.length)];
                diasSorteo = diasSorteo.filter(number => number != Number(randomItem1));

                diasSeleccionado.push(randomItem1);
            } break;
            case 2: {
                var randomItem1 = diasSorteo[Math.floor(Math.random() * diasSorteo.length)];
                diasSorteo = diasSorteo.filter(number => number != Number(randomItem1));
                var randomItem2 = diasSorteo[Math.floor(Math.random() * diasSorteo.length)];
                diasSorteo = diasSorteo.filter(number => number != Number(randomItem2));

                diasSeleccionado.push(randomItem1);
                diasSeleccionado.push(randomItem2);
            } break;
        }

    }
    else if (length == 3) {
        switch (prueba) {

            case 1: {
                var randomItem1 = diasSorteo[Math.floor(Math.random() * diasSorteo.length)];
                diasSorteo = diasSorteo.filter(number => number != Number(randomItem1));

                diasSeleccionado.push(randomItem1);
            } break;
            case 2: {
                var randomItem1 = diasSorteo[Math.floor(Math.random() * diasSorteo.length)];
                diasSorteo = diasSorteo.filter(number => number != Number(randomItem1));
                var randomItem2 = diasSorteo[Math.floor(Math.random() * diasSorteo.length)];
                diasSorteo = diasSorteo.filter(number => number != Number(randomItem2));

                diasSeleccionado.push(randomItem1);
                diasSeleccionado.push(randomItem2);
            } break;
            case 3: {
                var randomItem1 = diasSorteo[Math.floor(Math.random() * diasSorteo.length)];
                diasSorteo = diasSorteo.filter(number => number != Number(randomItem1));
                var randomItem2 = diasSorteo[Math.floor(Math.random() * diasSorteo.length)];
                diasSorteo = diasSorteo.filter(number => number != Number(randomItem2));
                var randomItem3 = diasSorteo[Math.floor(Math.random() * diasSorteo.length)];
                diasSorteo = diasSorteo.filter(number => number != Number(randomItem3));

                diasSeleccionado.push(randomItem1);
                diasSeleccionado.push(randomItem2);
                diasSeleccionado.push(randomItem3);
            } break;
        }

    }
    else {

        console.log("entro else");

        switch (prueba) {

            case 1: {
                var randomItem1 = diasSorteo[Math.floor(Math.random() * diasSorteo.length)];
                diasSorteo = diasSorteo.filter(number => number != Number(randomItem1));

                diasSeleccionado.push(randomItem1);
            } break;
            case 2: {
                var randomItem1 = diasSorteo[Math.floor(Math.random() * diasSorteo.length)];
                diasSorteo = diasSorteo.filter(number => number != Number(randomItem1));
                var randomItem2 = diasSorteo[Math.floor(Math.random() * diasSorteo.length)];
                diasSorteo = diasSorteo.filter(number => number != Number(randomItem2));

                diasSeleccionado.push(randomItem1);
                diasSeleccionado.push(randomItem2);
            } break;
            case 3: {
                var randomItem1 = diasSorteo[Math.floor(Math.random() * diasSorteo.length)];
                diasSorteo = diasSorteo.filter(number => number != Number(randomItem1));
                var randomItem2 = diasSorteo[Math.floor(Math.random() * diasSorteo.length)];
                diasSorteo = diasSorteo.filter(number => number != Number(randomItem2));
                var randomItem3 = diasSorteo[Math.floor(Math.random() * diasSorteo.length)];
                diasSorteo = diasSorteo.filter(number => number != Number(randomItem3));

                diasSeleccionado.push(randomItem1);
                diasSeleccionado.push(randomItem2);
                diasSeleccionado.push(randomItem3);
            } break;
            case 4: {
                var randomItem1 = diasSorteo[Math.floor(Math.random() * diasSorteo.length)];
                diasSorteo = diasSorteo.filter(number => number != Number(randomItem1));
                var randomItem2 = diasSorteo[Math.floor(Math.random() * diasSorteo.length)];
                diasSorteo = diasSorteo.filter(number => number != Number(randomItem2));
                var randomItem3 = diasSorteo[Math.floor(Math.random() * diasSorteo.length)];
                diasSorteo = diasSorteo.filter(number => number != Number(randomItem3));
                var randomItem4 = diasSorteo[Math.floor(Math.random() * diasSorteo.length)];
                diasSorteo = diasSorteo.filter(number => number != Number(randomItem4));

                diasSeleccionado.push(randomItem1);
                diasSeleccionado.push(randomItem2);
                diasSeleccionado.push(randomItem3);
                diasSeleccionado.push(randomItem4);

            } break;
        }
    }

    console.log("ini");
    console.log("array dias seleccionado: " + diasSeleccionado)
    console.log("fin");

    var rpta = random(diaActual, ultimodia);

    console.log("Respuesta: " + rpta)
    var exist = false;
    if (diasSeleccionado != null) {
        for (var i = 0; i < diasSeleccionado.length; i++) {
            console.log("Array de diasSeleccionado: " + Number(rpta))
            var dia = Number(diasSeleccionado[i]);
            console.log("Dia: " + dia)
            if (Number(rpta) == dia)
                exist = true;
        }
    }

    if (exist === true) {
        console.log(Number(rpta))
        console.log(dia)

        $('#mensaje').show();
        $('#mensaje').removeClass();
        $('#mensaje').addClass('action-exito');
        $('#mensaje').html("El proceso ha terminado satisfactoriamente!  Puede salir.");
        //var prueba = $("#pruebas").val();
        prueba -= 1;
        $("#pruebas").val(prueba);
        $("#tipo").val("POK");
        wf_log("SORTEO: DIA DE PRUEBA", "POK", coordinador, docoes);
        //wf_log("PRUEBA", "XEQ", coordinador, docoes);
        sorteoUnidad();
        eliminarLogSorteo();
        insertarSorteo();
        //AceptarSorteo();
        obtenerHistoricoSorteo();
        listarAreas();
        setTimeout(function () {
            $('#popupDatos').bPopup().close();
            LimpiarPopUp();
        }, 50);
    }
    else {

        console.log("else de exist==true")

        $('#mensaje').show();
        $('#mensaje').removeClass();
        $('#mensaje').addClass('action-alert');
        $('#mensaje').html("No hay prueba");
        $("#btnIniciarSorteo").hide();
        $("#tipo").val("PNO");

        try {

            if (document.getElementById("tablaArea").tBodies[0].rows.length == 0) {
                wf_log("SORTEO: NO EXISTE LISTADO DE UNIDADES PARA LA PRUEBA ", "PNO", coordinador, docoes);
                wf_log("DÍA DE PRUEBA: NO EXISTE LISTADO DE UNIDADES PARA LA PRUEBA ", "XEQ", coordinador, docoes);
            } else {
                wf_log("SORTEO: NO HAY PRUEBA ", "PNO", coordinador, docoes);
                wf_log("NO HAY PRUEBA", "XNO", coordinador, docoes);
            }

        } catch (ex) {
            wf_log("SORTEO: NO HAY PRUEBA ", "PNO", coordinador, docoes);
            wf_log("NO HAY PRUEBA", "XNO", coordinador, docoes);
        }

        insertarSorteo();
        obtenerHistoricoSorteo();
        listarAreas();
        setTimeout(function () {
            $('#popupDatos').bPopup().close();
            LimpiarPopUp();
        }, 50);
    }

    $("#diasFaltantes").val(length - 1);
};*/

function jugarSorteo() {

    console.log("entro jugarSorteo b_hay_pruebahoy -> " + b_hay_pruebahoy);

    if (!b_hay_pruebahoy) {
        $('#mensaje').show();
        $('#mensaje').removeClass();
        $('#mensaje').addClass('action-message');
        $('#mensaje').html("Ya hubo sorteo el dia de hoy");
        $("#btnIniciarSorteo").hide();

        setTimeout(function () {
            $('#popupDatos').bPopup().close();
            LimpiarPopUp();
        }, 50);

        return;

    } else {
        $("#btnIniciarSorteo").show();

        var coordinador = $("#txtCoordinador").val();
        var docoes = $("#txtDocoes").val();
        wf_log("Ingreso al modulo Sorteo", "INI", coordinador, docoes);
        diaActual = $("#diaActual").val();
        ultimodia = $("#ultimoDia").val();
        diasSorteo = [];
        diasSeleccionado = [];
        var prueba = Number($("#pruebas").val());

        console.log("prueba => " + prueba);

        //SE AGOTARON LAS BALOTAS NEGRAS ---MAX 4
        if (prueba <= 0) {

            $('#mensaje').show();
            $('#mensaje').removeClass();
            $('#mensaje').addClass('action-alert');
            $('#mensaje').html("No hay prueba");
            $("#btnIniciarSorteo").hide();
            $("#tipo").val("PNO");

            wf_log("SORTEO: NO HAY PRUEBA ", "PNO", coordinador, docoes);
            wf_log("NO HAY PRUEBA", "XNO", coordinador, docoes);            

        } else {

            for (var i = diaActual; i <= ultimodia; i++) {
                diasSorteo.push(Number(i));
            }

            var length = Number($("#diasFaltantes").val());

            console.log("entro 1");

            if (length == 1) {
                switch (prueba) {

                    case 1: {
                        var randomItem1 = diasSorteo[Math.floor(Math.random() * diasSorteo.length)];
                        diasSorteo = diasSorteo.filter(number => number != Number(randomItem1));

                        diasSeleccionado.push(randomItem1);
                    } break;
                }
            }
            else if (length == 2) {
                switch (prueba) {

                    case 1: {
                        var randomItem1 = diasSorteo[Math.floor(Math.random() * diasSorteo.length)];
                        diasSorteo = diasSorteo.filter(number => number != Number(randomItem1));

                        diasSeleccionado.push(randomItem1);
                    } break;
                    case 2: {
                        var randomItem1 = diasSorteo[Math.floor(Math.random() * diasSorteo.length)];
                        diasSorteo = diasSorteo.filter(number => number != Number(randomItem1));
                        var randomItem2 = diasSorteo[Math.floor(Math.random() * diasSorteo.length)];
                        diasSorteo = diasSorteo.filter(number => number != Number(randomItem2));

                        diasSeleccionado.push(randomItem1);
                        diasSeleccionado.push(randomItem2);
                    } break;
                }

            }
            else if (length == 3) {
                switch (prueba) {

                    case 1: {
                        var randomItem1 = diasSorteo[Math.floor(Math.random() * diasSorteo.length)];
                        diasSorteo = diasSorteo.filter(number => number != Number(randomItem1));

                        diasSeleccionado.push(randomItem1);
                    } break;
                    case 2: {
                        var randomItem1 = diasSorteo[Math.floor(Math.random() * diasSorteo.length)];
                        diasSorteo = diasSorteo.filter(number => number != Number(randomItem1));
                        var randomItem2 = diasSorteo[Math.floor(Math.random() * diasSorteo.length)];
                        diasSorteo = diasSorteo.filter(number => number != Number(randomItem2));

                        diasSeleccionado.push(randomItem1);
                        diasSeleccionado.push(randomItem2);
                    } break;
                    case 3: {
                        var randomItem1 = diasSorteo[Math.floor(Math.random() * diasSorteo.length)];
                        diasSorteo = diasSorteo.filter(number => number != Number(randomItem1));
                        var randomItem2 = diasSorteo[Math.floor(Math.random() * diasSorteo.length)];
                        diasSorteo = diasSorteo.filter(number => number != Number(randomItem2));
                        var randomItem3 = diasSorteo[Math.floor(Math.random() * diasSorteo.length)];
                        diasSorteo = diasSorteo.filter(number => number != Number(randomItem3));

                        diasSeleccionado.push(randomItem1);
                        diasSeleccionado.push(randomItem2);
                        diasSeleccionado.push(randomItem3);
                    } break;
                }

            }
            else {

                console.log("entro else");

                switch (prueba) {

                    case 1: {
                        var randomItem1 = diasSorteo[Math.floor(Math.random() * diasSorteo.length)];
                        diasSorteo = diasSorteo.filter(number => number != Number(randomItem1));

                        diasSeleccionado.push(randomItem1);
                    } break;
                    case 2: {
                        var randomItem1 = diasSorteo[Math.floor(Math.random() * diasSorteo.length)];
                        diasSorteo = diasSorteo.filter(number => number != Number(randomItem1));
                        var randomItem2 = diasSorteo[Math.floor(Math.random() * diasSorteo.length)];
                        diasSorteo = diasSorteo.filter(number => number != Number(randomItem2));

                        diasSeleccionado.push(randomItem1);
                        diasSeleccionado.push(randomItem2);
                    } break;
                    case 3: {
                        var randomItem1 = diasSorteo[Math.floor(Math.random() * diasSorteo.length)];
                        diasSorteo = diasSorteo.filter(number => number != Number(randomItem1));
                        var randomItem2 = diasSorteo[Math.floor(Math.random() * diasSorteo.length)];
                        diasSorteo = diasSorteo.filter(number => number != Number(randomItem2));
                        var randomItem3 = diasSorteo[Math.floor(Math.random() * diasSorteo.length)];
                        diasSorteo = diasSorteo.filter(number => number != Number(randomItem3));

                        diasSeleccionado.push(randomItem1);
                        diasSeleccionado.push(randomItem2);
                        diasSeleccionado.push(randomItem3);
                    } break;
                    case 4: {
                        var randomItem1 = diasSorteo[Math.floor(Math.random() * diasSorteo.length)];
                        diasSorteo = diasSorteo.filter(number => number != Number(randomItem1));
                        var randomItem2 = diasSorteo[Math.floor(Math.random() * diasSorteo.length)];
                        diasSorteo = diasSorteo.filter(number => number != Number(randomItem2));
                        var randomItem3 = diasSorteo[Math.floor(Math.random() * diasSorteo.length)];
                        diasSorteo = diasSorteo.filter(number => number != Number(randomItem3));
                        var randomItem4 = diasSorteo[Math.floor(Math.random() * diasSorteo.length)];
                        diasSorteo = diasSorteo.filter(number => number != Number(randomItem4));

                        diasSeleccionado.push(randomItem1);
                        diasSeleccionado.push(randomItem2);
                        diasSeleccionado.push(randomItem3);
                        diasSeleccionado.push(randomItem4);

                    } break;
                }
            }

            console.log("ini");
            console.log("array dias seleccionado: " + diasSeleccionado)
            console.log("fin");

            var rpta = random(diaActual, ultimodia);

            console.log("Respuesta: " + rpta)
            var exist = false;
            if (diasSeleccionado != null) {
                for (var i = 0; i < diasSeleccionado.length; i++) {
                    console.log("Array de diasSeleccionado: " + Number(rpta))
                    var dia = Number(diasSeleccionado[i]);
                    console.log("Dia: " + dia)
                    if (Number(rpta) == dia)
                        exist = true;
                }
            }

            if (exist === true) {

                try {

                    if (document.getElementById("tablaArea").tBodies[0].rows.length == 0) {
                        $("#tipo").val("PNO");
                        wf_log("SORTEO: NO EXISTE LISTADO DE UNIDADES PARA LA PRUEBA ", "PNO", coordinador, docoes);
                        wf_log("DÍA DE PRUEBA: NO EXISTE LISTADO DE UNIDADES PARA LA PRUEBA ", "XEQ", coordinador, docoes);

                    } else {
                        $("#tipo").val("POK");
                        wf_log("SORTEO: DIA DE PRUEBA", "POK", coordinador, docoes);
                        //wf_log("PRUEBA", "XEQ", coordinador, docoes);
                        sorteoUnidad();
                        eliminarLogSorteo();
                        insertarSorteo();
                    }

                    $('#mensaje').show();
                    $('#mensaje').removeClass();
                    $('#mensaje').addClass('action-exito');
                    $("#btnIniciarSorteo").hide();
                    $('#mensaje').html("El proceso ha terminado satisfactoriamente!  Puede salir.");
                    //var prueba = $("#pruebas").val();
                    prueba -= 1;
                    $("#pruebas").val(prueba);


                } catch (ex) {
                    wf_log("SORTEO: NO HAY PRUEBA ", "PNO", coordinador, docoes);
                    wf_log("NO HAY PRUEBA", "XNO", coordinador, docoes);
                }
            }
            else {

                //BALOTA BLANCA

                $('#mensaje').show();
                $('#mensaje').removeClass();
                $('#mensaje').addClass('action-alert');
                $('#mensaje').html("No hay prueba");
                $("#btnIniciarSorteo").hide();
                $("#tipo").val("PNO");

                wf_log("SORTEO: NO HAY PRUEBA ", "PNO", coordinador, docoes);
                wf_log("NO HAY PRUEBA", "XNO", coordinador, docoes);


            }

            $("#diasFaltantes").val(length - 1);

        }

        $("#btnIniciarSorteo").hide();
        obtenerHistoricoSorteo();
        listarAreas();
        setTimeout(function () {
            $('#popupDatos').bPopup().close();
            LimpiarPopUp();
        }, 50);

        b_hay_pruebahoy = false;
        
    }
    
};

function random(min, max) {
    var rpta = 0;
    $.ajax({
        type: 'POST',
        url: controlador + "Random",
        async: false,
        dataType: "json",
        data: {
            min: Number(min),
            max: Number(max),
        },
        success: function (data) {
            rpta = data;
        },
        error: function () {
            //mostrarError();
        }
    });
    return rpta;
}

obtenerFecha = function () {
    var hoy = new Date();
    var dd = hoy.getDate();
    var mm = hoy.getMonth() + 1; //hoy es 0!
    var yyyy = hoy.getFullYear();

    if (dd < 10) {
        dd = '0' + dd
    }

    if (mm < 10) {
        mm = '0' + mm
    }

    hoy = dd + '/' + mm + '/' + yyyy;
    $("#fechaActual").val(hoy);
    $("#diaActual").val(dd);

    var date = new Date();
    var ultimoDia = (new Date(date.getFullYear(), date.getMonth() + 1, 0)).getDate();
    $("#ultimoDia").val(ultimoDia);

    return hoy;
}

obtenerFechaAyer = function () {
    var fecha = new Date();
    var ayer = new Date(fecha.getTime() - 24 * 60 * 60 * 1000);
    var dia = ayer.getDate();
    var mes = ayer.getMonth() - 3;
    var anio = fecha.getFullYear();
    if (dia < 10) {
        dia = '0' + dia
    }

    if (mes < 10) {
        mes = '0' + mes
    }
    ayer = anio + "-" + mes + "-" + dia;
    return ayer;
}

//obtenerFechaIniMes = function () {
//    var fecha = new Date();
//    var dia = fecha.getDate();
//    var mes = fecha.getMonth() + 1;
//    var anio = fecha.getFullYear();
//    if (mes < 10) {
//        mes = '0' + mes
//    }
//    ayer = anio + "-" + mes + "-" + "01";
//    return ayer;
//}

obtenerHistoricoSorteo = function () {
    //var fecha = obtenerFechaIniMes();
    //console.log(fecha)
    $.ajax({
        type: 'POST',
        url: controlador + "Historico",
        //data: { logfecha: fecha },
        success: function (evt) {
            $('#historicoSorteo').html(evt);
        },
        error: function () {
            mostrarError();
        }
    });
}

diasFaltantes = function () {
    $.ajax({
        type: 'POST',
        url: controlador + "DiasFaltantes",
        success: function (evt) {
            ultimodia = $("#ultimoDia").val();
            $("#diasFaltantes").val(ultimodia - evt);
        },
        error: function (ex) {
            mostrarError();
        }
    });
}

cantidadPruebas = function () {
    var dd = $("#pruebas").val();
    if (dd == 0) {
        $("#mensaje").html("Ya se cumplio con todas las pruebas aleatorias programadas para este mes.");
    }
}

listarAreas = function () {
    $.ajax({
        type: 'POST',
        url: controlador + "Listado",
        success: function (evt) {
            $('#listadoAreas').html(evt);
        },
        error: function () {
            mostrarError();
        }
    });
}

mostrarError = function () {
    $('#mensaje').show();
    $('#mensaje').removeClass();
    $('#mensaje').addClass('action-alert');
    $('#mensaje').html("Ha ocurrido un error.");
}

$(document).on("click", "#btnEliminarArea", function () {
    var parent = $(this).parent().parent();
    var fila = $(this).closest("tr").index();
    $(parent).remove();
    ActualizarNumeroItem();
});

ActualizarNumeroItem = function () {
    var i = 1;
    $("#tablaArea tbody tr").each(function (index) {
        $(this).find("td:eq(0)").html(i);
        i++;
    })
}

AceptarSorteo = function () {
    var hoy = new Date();
    var datos = {};
    datos["logtipo"] = $("#tipo").val();
    datos["logusuario"] = "ccontrol-SCO-1D65T";
    datos["logfecha"] = hoy;
    datos["logdescrip"] = $("#mensaje").text();
    datos["logcoordinador"] = $("#txtCoordinador").val();
    datos["logdocoes"] = $("#txtDocoes").val();

    var rpta = 0;
    $.ajax({
        type: 'POST',
        url: controlador + "GrabarNuevo",
        async: false,
        dataType: "json",
        data: datos,
        success: function (data) {
            rpta = data;
        },
        error: function () {
            mostrarError();
        }
    });
    console.log(rpta);
    return rpta;
}

wf_log = function (logdescrip, logtipo, coordinador, docoes) {
    var rpta;
    var datos = { logdescrip: logdescrip, logtipo: logtipo, coordinador: coordinador, docoes: docoes };
    $.ajax({
        type: 'POST',
        url: controlador + "wf_log",
        async: false,
        dataType: "json",
        data: datos,
        success: function (data) {
            rpta = data;
        },
        error: function () {
            //mostrarError();
        }
    });
    console.log(rpta);
    return rpta;
}

TotalConteoTipoXEQTotal = function () {
    $.ajax({
        type: 'POST',
        url: controlador + "TotalConteoTipoXEQ",
        success: function (evt) {

            if (evt >= 4) {
                b_hay_pruebahoy = true;

                $("#pruebas").val(0);

                $('#mensaje').show();
                $('#mensaje').removeClass();
                $('#mensaje').addClass('action-message');
                $('#mensaje').html("Ya se cumplió con todas las pruebas aleatorias programadas para este día");
                cantidadPruebas();


                var dd = $("#pruebas").val();
                if (dd == 0) {
                    $("#mensaje").html("Ya se cumplio con todas las pruebas aleatorias programadas para este mes.");
                }

            }
            else {
                $("#pruebas").val($("#pruebas").val() - evt);
                cantidadPruebas();
            }
        },
        error: function (ex) {
            mostrarError();
        }
    });
}

TotalConteoTipoXNO = function () {
    logfecha = obtenerFecha();
    var datos = { tipo: "XNO", logfecha: logfecha };
    $.ajax({
        type: 'POST',
        url: controlador + "TotalConteoTipo",
        data: datos,
        success: function (evt) {
            if (evt > 0) {
                b_hay_pruebahoy = false;
                $('#mensaje').show();
                $('#mensaje').removeClass();
                $('#mensaje').addClass('action-exito');
                $('#mensaje').html("Ya hubo sorteo el dia de hoy");
                $("#btnIniciarSorteo").hide();
            } else {
                $("#btnIniciarSorteo").show();
            }
        },
        error: function (ex) {
            mostrarError();
        }
    });
}

TotalConteoTipoXEQ = function () {
    logfecha = obtenerFecha();
    var datos = { tipo: "XEQ", logfecha: logfecha };
    $.ajax({
        type: 'POST',
        url: controlador + "TotalConteoTipo",
        data: datos,
        success: function (evt) {
            if (evt > 0) {
                b_hay_pruebahoy = false;
                $('#mensaje').show();
                $('#mensaje').removeClass();
                $('#mensaje').addClass('action-exito');
                $('#mensaje').html("Ya hubo sorteo el dia de hoy");
                $("#btnIniciarSorteo").hide();
            } else {
                $("#btnIniciarSorteo").show();
            }
        },
        error: function (ex) {
            mostrarError();
        }
    });
}

sorteoUnidad = function () {

    var coordinador = $("#txtCoordinador").val();
    var docoes = $("#txtDocoes").val();

    var contador = 0;
    $("#tablaArea tbody tr").each(function (index) {
        contador++;
    })

    $.ajax({
        type: 'POST',
        url: controlador + "sorteoUnidad",
        async: false,
        data: { contador: contador },
        dataType: "json",
        success: function (data) {

            var equicodi = 0;
            var emprnomb = "";
            var areanomb = "";
            var equiabrev = "";

            console.log(" indice sorteado => " + data.nroequiposorteado)

            $("#tablaArea tbody tr").each(function (index) {
                console.log(" indice area => " + index)

                if (index == (data.nroequiposorteado - 1)) {
                    equicodi = $(this).find("td").eq(1).html();
                    emprnomb = $(this).find("td").eq(2).html();
                    areanomb = $(this).find("td").eq(3).html();
                    equiabrev = $(this).find("td").eq(4).html();

                    console.log(" equicodi => " + equicodi)
                    console.log(" emprnomb => " + emprnomb)
                    console.log(" areanomb => " + areanomb)
                    console.log(" equiabrev => " + equiabrev)

                    $("#codigo").val(equicodi);
                    var descripcion = emprnomb + " - " + areanomb + " -> " + equiabrev;
                    $("#unidad").text(descripcion);

                    wf_log("PRUEBA : " + descripcion, "SEQ", coordinador, docoes);
                    wf_log("PRUEBA : " + descripcion, "XEQ", coordinador, docoes);

                }

            })

        },
        error: function () {
            //mostrarError();
        }
    });
}

eliminarLogSorteo = function () {
    $.ajax({
        type: 'POST',
        url: controlador + "eliminarLogSorteo",
        async: false,
        dataType: "json",
        success: function (data) {
            console.log(data);
            if (data == true) {
                $('#mensaje').show();
                $('#mensaje').removeClass();
                $('#mensaje').addClass('action-exito');
                $("#mensaje").val("Elemento eliminado correctamente..!");
            } else {
                $('#mensaje').show();
                $('#mensaje').removeClass();
                $('#mensaje').addClass('action-error');
                $("#mensaje").val("No puedo borrar equipos de hoy");
            }
        },
        error: function () {
            //mostrarError();
        }
    });
}

eliminarLogSorteo = function () {
    $.ajax({
        type: 'POST',
        url: controlador + "eliminarLogSorteo",
        async: false,
        dataType: "json",
        success: function (data) {
            console.log(data);
            if (data == true) {
                $('#mensaje').show();
                $('#mensaje').removeClass();
                $('#mensaje').addClass('action-exito');
                $("#mensaje").val("Elemento eliminado correctamente..!");
            } else {
                $('#mensaje').show();
                $('#mensaje').removeClass();
                $('#mensaje').addClass('action-error');
                $("#mensaje").val("No puedo borrar equipos de hoy");
            }
        },
        error: function () {
            //mostrarError();
        }
    });
}

insertarSorteo = function () {
    $("#tablaArea tbody tr").each(function (index) {
        var equicodi = $(this).find("td").eq(1).html();
        $.ajax({
            type: 'POST',
            url: controlador + "insertarSorteo",
            async: false,
            data: {
                equicodi: equicodi,
                codigo: $("#codigo").val()
            },
            dataType: "json",
            success: function (evt) {
            },
            error: function () {
                //mostrarError();
            }
        });
    })
}

popupDatos = function () {
    setTimeout(function () {
        $('#popupDatos').bPopup({
            autoClose: false,
        });
    }, 50);
}

LimpiarPopUp = function () {
    $("#txtCoordinador").val("");
    $("#txtDocoes").val("");
    $("#chkMantto").prop("checked", false);
    $("#chkHoras").prop("checked", false);
}