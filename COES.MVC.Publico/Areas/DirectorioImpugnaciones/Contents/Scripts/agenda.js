var controlador = siteRoot + 'DirectorioImpugnaciones/Agenda/';
var fecCal = new Date();
var nombreMes = ["Enero", "Febrero", "Marzo", "Abril", "Mayo", "Junio", "Julio", "Agosto", "Setiembre", "Octubre", "Noviembre", "Diciembre"];
var f1;

$(function () {
    $('#tab-container').easytabs({
        animate: false
    });

    $('#btnAdelante').click(function () {
        modificarMes(1);
    });

    $('#btnAtras').click(function () {
        modificarMes(-1);
    });
    
    $('select#fecMes').on('change', function () {
        fecCal.setMonth(parseInt($(this).val())-1);
        mostrarListaEventosMes();
    });

    $('select#fecAnio').on('change', function () {
        fecCal.setFullYear(parseInt($(this).val()));
        mostrarListaEventosMes();
    });

    //mostraCalendario();
    //mostrarListaEventos();
    mostrarListaEventosMes();

    $('#btnBuscar').click(function () {
        mostrarListaEventosMes();
    });

    //$('a[href=#vistaLista]').click(function () {
    //    mostrarListaEventos();
    //});

   
});

modificarMes = function (diferencia) {
    fecCal.setMonth(fecCal.getMonth() + diferencia);
    var mes = ((fecCal.getMonth() + 101) + "").substring(1);
    var anio = fecCal.getFullYear();
    //mostraCalendario();
    /* $('#fecAnio option[value=' + fecCal.getFullYear() + ']').attr('selected', 'selected');*/
    /* $('#fecMes option[value=' + ((fecCal.getMonth() + 101) + "").substring(1) + ']').attr('selected', 'selected');*/
    $('#fecAnio').val(anio);
    $('#fecMes').val(mes);
    mostrarListaEventosMes();
}


mostraCalendario = function () {
    var mes = ((fecCal.getMonth() + 101) + "").substring(1);
    var anio = fecCal.getFullYear();
    $('#fecMes').val(mes);
    $('#fecAnio').val(anio);
   /*$('#fecAnio option[value=' + fecCal.getFullYear() + ']').attr('selected', 'selected');*/
   /* $('#fecMes option[value=' + ((fecCal.getMonth() + 101) + "").substring(1) + ']').attr('selected', 'selected');*/
    var fechTemp = new Date(fecCal.getFullYear(), fecCal.getMonth(), 1);
    var fueraDeMes = false;
    for (var i = 0; i < 6; i++) {
        for (var j = 0; j < 7; j++) {
            if (fechTemp.getDay() == j && !fueraDeMes) {
                $('#f' + (i + 1) + (j + 1)).html('<div class="daynumber">' + fechTemp.getDate() + "</div>");
                if (fechTemp.toString() == (new Date((new Date()).getFullYear(), (new Date()).getMonth(), (new Date()).getDate())).toString()) {
                    $('#f' + (i + 1) + (j + 1)).attr('bgcolor', '#FFFFD2')
                } else {
                    $('#f' + (i + 1) + (j + 1)).attr('bgcolor', 'white')
                }
                fechTemp.setDate(fechTemp.getDate() + 1);
                if (fechTemp.getDate() == 1)
                    fueraDeMes = true;
            } else {
                $('#f' + (i + 1) + (j + 1)).html("");
                $('#f' + (i + 1) + (j + 1)).attr('bgcolor', '#EFF3F5');
            }
        }
    }
    //mostrarListaEventosMes();
}

mostrarListaEventos = function () {
    $.ajax({
        type: 'POST',
        url: controlador + 'ListaEventos',
        data: {
            tipo: tipoEventoAgenda,
            anio: $('#fecAnio').val()
        },
        success: function (result) {
            $('#listaEventos').html(result);
            $('#tablaListaEventos').dataTable({
                paging: false,
                scrollY: 100,
                "info": false,
                "scrollY": false
            });            
        },
        error: function () {

        }
    });
}

mostrarListaEventosMes = function () {
    console.log('Ingresa a evento mes');
    $.ajax({
        type: 'POST',
        url: controlador + 'ListaEventosMes',
        data: {
            mes: $('#fecMes').val(),
            anio: $('#fecAnio').val(),
            tipo: tipoEventoAgenda
        },
        success: function (result) {
            console.log(result);
            if (result.length > 0) {
                $('#ListaEventosMes').css("width", $('.form-main').width() + "px");
                $('#ListaEventosMes').html(result);
                //f1 = result[0]["FechaInicio"];
                //var fechTemp = new Date(fecCal.getFullYear(), fecCal.getMonth(), 1);
                //var fueraDeMes = false;
                //for (var i = 0; i < 6; i++) {
                //    for (var j = 0; j < 7; j++) {
                //        if (fechTemp.getDay() == j && !fueraDeMes) {
                //            for (var k = 0; k < result.length; k++) {
                //                var fechaEvento = new Date(parseInt(result[k]["FechaInicio"].replace('/Date(', "").replace(')/', '')));
                //                var f1 = "" + fechaEvento.getDate() + fechaEvento.getMonth() + fechaEvento.getFullYear();
                //                var f2 = "" + fechTemp.getDate() + fechTemp.getMonth() + fechTemp.getFullYear();
                //                if (f1 == f2) {
                //                    var evento = '<a href="JavaScript:mostrarEvento(' + result[k]["Codigo"] +
                //                        ')" title="' +
                //                        result[k]["Titulo"] +
                //                        '"><div class="evento">' + result[k]["Titulo"] + '</div></a>';
                //                    $('#f' + (i + 1) + (j + 1)).append(evento);
                //                }
                //            }
                //            fechTemp.setDate(fechTemp.getDate() + 1);
                //            if (fechTemp.getDate() == 1)
                //                fueraDeMes = true;
                //        }
                //    }
                //}
            }
        },
        error: function () {

        }
    });
}

mostrarEvento = function (id) {
    $.ajax({
        type: 'POST',
        url: controlador + 'ObtenerEvento',
        data: {
            id: id
        },
        success: function (result) {
            $('#contenidoEdicion').html(result);
            setTimeout(function () {
                $('#popupEdicion').bPopup({
                    autoClose: false
                });

            }, 50);
        },
        error: function () {

        }
    });
}

download = function (id) {
    document.location.href = controlador + "Download?id=" + id;
}