var controlador = siteRoot + 'PostOperacion/EvaluacionFallas/';
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
        fecCal.setMonth(parseInt($(this).val()) - 1);
        mostrarListaEventosMes(); 
    });

    $('select#fecAnio').on('change', function () {
        fecCal.setFullYear(parseInt($(this).val()));
        mostrarListaEventosMes();
    });

    mostrarListaEventosMes();

    //$('a[href=#vistaLista]').click(function () {
    //    mostrarListaEventos();
    //});

});

modificarMes = function (diferencia) {   
    fecCal.setMonth(fecCal.getMonth() + diferencia);
    var mes = ((fecCal.getMonth() + 101) + "").substring(1);
    var anio = fecCal.getFullYear();
    $('#fecAnio').val(anio);
    $('#fecMes').val(mes);
/*    fecCal.setMonth(fecCal.getMonth() + diferencia);*/
    mostrarListaEventosMes();
}

mostraCalendario = function () {            
    $('#fecMes').val(((fecCal.getMonth() + 101) + "").substring(1)); 
    $('#fecAnio').val(fecCal.getFullYear());    
    var fechTemp = new Date(fecCal.getFullYear(), fecCal.getMonth(), 1);
    var fueraDeMes = false;
    for (var i = 0; i < 6; i++) {
        for (var j = 0; j < 7; j++) {
            if (j == 0 || j == 6) {
                $('#f' + (i) + (j + 1)).attr('bgcolor', '#EFF3F5');
            }            
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
    mostrarListaEventosMes();
}

mostrarListaEventosMes = function () {
    //$.ajax({
    //    type: 'POST',
    //    url: controlador + 'ListaEventosMes',
    //    data: {
    //        mes: $('#fecMes').val(),
    //        anio: $('#fecAnio').val()

    //    },
    //    dataType: 'json',
    //    success: function (result) {
    //        if (result.length > 0) {                          
    //            var tipo;
    //            f1 = result[0]["AFEREUFECHAPROG"];
    //            var fechTemp = new Date(fecCal.getFullYear(), fecCal.getMonth(), 1);
    //            var fueraDeMes = false;
    //            for (var i = 0; i < 6; i++) {
    //                for (var j = 0; j < 7; j++) {
    //                    if (fechTemp.getDay() == j && !fueraDeMes) {
    //                        for (var k = 0; k < result.length; k++) {
    //                            tipo = (result[k]["AFECONVTIPOREUNION"] == "N" ? "No presencial" : "Presencial")        
    //                            var fechaevento = new Date(parseInt(result[k]["AFEREUFECHAPROG"].replace('/Date(', "").replace(')/', '')));
    //                            var f1 = "" + fechaevento.getDate() + fechaevento.getMonth() + fechaevento.getFullYear();
    //                            var f2 = "" + fechTemp.getDate() + fechTemp.getMonth() + fechTemp.getFullYear();
                                                                
    //                            if (f1 == f2) {                                
    //                                if (tipo == "No presencial") {
    //                                 var evento = '<a href="JavaScript:mostrarEvento(' + result[k]["AFECODI"] + ')" title="' +
    //                                     result[k]["CODIGO"] +
    //                                     '"><div class="evento" style = "background-color:#F59025">' + result[k]["CODIGO"] + "<br>" + tipo  + '</div></a>';                                    
    //                                    $('#f' + (i + 1) + (j + 1)).append(evento);
    //                                }
    //                                else {
    //                                    var evento = '<a href="JavaScript:mostrarEvento(' + result[k]["AFECODI"] + ')" title="' +
    //                                        result[k]["CODIGO"] +
    //                                        '"><div class="evento" style = "background-color:#7EBB48">' + result[k]["CODIGO"] + "<br>" + tipo  + '</div></a>';
    //                                    $('#f' + (i + 1) + (j + 1)).append(evento);
    //                                }
    //                            }
    //                        }
    //                        fechTemp.setDate(fechTemp.getDate() + 1);
    //                        if (fechTemp.getDate() == 1)
    //                            fueraDeMes = true;
    //                    }
    //                }
    //            }
    //        }
    //    },
    //    error: function () {

    //    }
    //});
    $.ajax({
        type: 'POST',
        url: controlador + 'ListaEventosMesNew',
        data: {
            mes: $('#fecMes').val(),
            anio: $('#fecAnio').val()
        },
        success: function (result) {
            console.log(result);
            if (result.length > 0) {
                $('#ListaEventosMes').css("width", $('.form-main').width() + "px");
                $('#ListaEventosMes').html(result);
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

String.prototype.removeCharAt = function (i) {
    var tmp = this.split(''); // convert to an array    
    tmp.splice(i - 1, 10); // remove 10 element from the array (adjusting for non-zero-indexed counts)
    return tmp.join(''); // reconstruct the string 
}
