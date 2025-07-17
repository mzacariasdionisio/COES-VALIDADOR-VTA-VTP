var controlador = siteRoot + 'Eventos/AnalisisFallas/';

$(function () {

    ////$('#dtEditEveIni').inputmask({
    ////    mask: "1/2/y h:s:s",
    ////    placeholder: "dd/mm/yyyy hh:mm:ss",
    ////    alias: "datetime",
    ////    hourFormat: "24"
    ////});

    ////$('#dtEditEveIni').Zebra_DatePicker({
    ////    readonly_element: false,
    ////    format: 'd/m/Y',
    ////});

    ////$('#dtEditHoraInt').inputmask({
    ////    mask: "1/2/y h:s:s",
    ////    placeholder: "dd/mm/yyyy hh:mm:ss",
    ////    alias: "datetime",
    ////    hourFormat: "24"
    ////});

    ////$('#dtEditHoraInt').Zebra_DatePicker({
    ////    readonly_element: false,
    ////    direction: [$('#dtEditEveIni').val().split(" ", 1)[0], false],
    ////    format: 'd/m/Y',
    ////    onSelect: function (date) {
    ////        $('#dtEditHoraInt').val(date + " 00:00:00");
    ////    }
    ////});

    //$('#dtEditFecVen').inputmask({
    //    mask: "1/2/y h:s:s",
    //    placeholder: "dd/mm/yyyy hh:mm:ss",
    //    alias: "datetime",
    //    hourFormat: "24",
    //});
    //$('#dtEditFecVen').Zebra_DatePicker({
    //    readonly_element: false,
    //    //direction: [$('#dtEditEveIni').val().split(" ", 1)[0], false],
    //    format: 'd/m/Y',
    //    onSelect: function (date) {
    //        $('#dtEditFecVen').val(date + " 00:00:00");
    //    }
    //});

        $('#dtEditFecVen').inputmask({
        mask: "1/2/y h:s:s",
        placeholder: "dd/mm/yyyy HH:mm:ss",
        alias: "datetime",
        hourFormat: "24",
    });
    $('#dtEditFecVen').Zebra_DatePicker({
        readonly_element: false,
        //direction: [$('#dtEditEveIni').val().split(" ", 1)[0], false],
        format: 'd/m/Y',
        onSelect: function (date) {
            $('#dtEditFecVen').val(date + " 00:00:00");
        }
    });




    ancho = $("#mainLayout").width() > 900 ? $("#mainLayout").width() - 30 : 900;

    mostrarTabla();

    $("#btnNuevo").click(function () {
        NuevaConfiguracion();
    });

    $('#btnlistado').click(function () {
        window.location.href = controlador + 'InterrupcionSuministros';
    });

    $('#cbTipoInfo').change(function () {
        CargarReportes();
    });

    $('#btngrabargeneral').click(function () {
        ActualizarEvento();
    });

    console.log($("#Afecodi").val());
    ObtenerListaInterrupcionPorEventoSCO();
    console.log("entrí")
});

function mostrarTabla() {
    $('#listado').hide();
    ancho = $("#mainLayout").width() > 900 ? $("#mainLayout").width() - 30 : 900;
    $('#listado').css("width", $('#mainLayout').width() + "px");

    $('#listado').show();

    var anchoReporte = $('#tabla').width();
    $("#listado").css("width", (anchoReporte > ancho ? ancho : anchoReporte) + "px");

    $('#tabla').dataTable({
        "language": {
            "emptyTable": ""
        },
        "ordering": false,
        "destroy": "true",
        "info": false,
        "searching": true,
        "paging": false,
        "scrollX": true,
        "scrollY": $('#listado').height() > 200 ? 500 + "px" : "100%"
    });
}

function NuevaConfiguracion() {

    $.ajax({
        type: 'POST',
        url: controlador + 'NuevaConfiguracion',
        success: function (evt) {
            $('#contenidoDetalle').html(evt);

            setTimeout(function () {
                $('#popupDetalle').bPopup({
                    autoClose: false
                });
            }, 500);
        },
        error: function () {
            mostrarError('mensaje');
        }
    });
}

function ActualizarEvento() {
    var afecodi = $("#Afecodi").val();
    //var dtFechEve = $("#dtEditEveIni").val();
    //var dtHoraInt = $("#dtEditHoraInt").val();
    var dtFechVen = $("#dtEditFecVen").val();


    var nfilas = $("#table").find("tr");
    var lista = new Array();
    for (var i = 1; i < nfilas.length; i++) {
        var celdas = $(nfilas[i]).find("td");
        linea = i;
        var entidad = new Object();

        entidad.Evenini = $(celdas[0]).text().toString();
        entidad.Afefechainterr = $(celdas[1]).find(".dp-row").val().toString();
        entidad.Afecodi = $(celdas[3]).text().toString();
        entidad.Evencodi = $(celdas[4]).text().toString();

        lista.push(entidad);
    }

    var msjValidacionHo = ValidarFechas();
    if (msjValidacionHo != "")
        alert(msjValidacionHo);
    else {
        var oEventoEdit = {
            Afecodi: afecodi,
            //FechEvento: dtFechEve,
            //DI: dtHoraInt,
            DF: dtFechVen,
            LstEventos: lista
        }
        //console.log("lista", lista)
        //console.log("oEventoEdit", oEventoEdit)
        //console.log("oEventoEdit", JSON.stringify(oEventoEdit))
        $.ajax({
            type: 'POST',
            url: controlador + 'EditarEvento',
            contentType: "application/json; charset=utf-8",

            data: JSON.stringify(oEventoEdit),

            success: function (eData) {
                if (eData.Resultado == '-1') {
                    //alert(eData.StrMensaje);                    
                    mostrarMensaje('mensaje', 'alert', eData.StrMensaje);
                }
                else {
                    if (eData.Resultado != "") {
                        $("#mensaje2").hide();
                        //alert(eData.StrMensaje);
                        if (eData.Resultado == "1")
                            mostrarMensaje('mensaje', 'exito', eData.StrMensaje);
                        else
                            mostrarMensaje('mensaje', 'alert', eData.StrMensaje);

                    }
                }
            },
            error: function (err) {
                console.log(err);
                alert("Ha ocurrido un error");
            }
        });
    }
}

function ValidarFechas() {

    var fechaEvent = moment($("#dtEditEveIni").val(), 'DD/MM/YYYY HH:mm:ss');
    var fechaInterrup = moment($("#dtEditHoraInt").val(), 'DD/MM/YYYY HH:mm:ss');
    var fechaAmpli = moment($("#dtEditFecVen").val(), 'DD/MM/YYYY HH:mm:ss');

    var dif1 = fechaInterrup.diff(fechaEvent);
    var dif3 = fechaAmpli.diff(fechaInterrup);

    var mens = '';

    // Verifica que la hora de interrupción sea mayor o igual a la del evento
    if (dif1 < 0) {
        //return "La fecha de Interrupción debe ser mayor a " + $('#dtEditEveIni').val();
        mens = "La fecha de Interrupción debe ser mayor a o igual a " + $('#dtEditEveIni').val();
        mostrarMensaje('mensaje', 'alert', mens);
    }
    // Verifica que la hora de vencimiento sea mayor o igual a la de interrupción
    if (dif3 < 0) {
        //return "La fecha de vencimiento debe ser mayor o igual a " + $('#dtEditHoraInt').val();
        mens = "La fecha de vencimiento debe ser mayor o igual a " + $('#dtEditHoraInt').val();
        mostrarMensaje('mensaje', 'alert', mens);
    }
    return mens;
}


function mostrarMensaje(id, tipo, mensaje) {
    $('#' + id).removeClass();
    $('#' + id).addClass('action-' + tipo);
    $('#' + id).html(mensaje);
};


function ObtenerListaInterrupcionPorEventoSCO() {


    var miDataM = {
        Afeanio: $("#Afeanio").val(),
        Afecorr: $("#Afecorr").val()

    };



    $("#listado").html('');

    $.ajax({
        type: 'POST',
        url: controlador + 'ListarInterrupcionPorEventoSCO',
        contentType: "application/json; charset=utf-8",
        data: JSON.stringify(miDataM),

        success: function (eData) {
            if (eData.Resultado == '-1' && eData.StrMensaje != "") {
                alert(eData.StrMensaje);
            }
            else {
                if (eData.Resultado != "") {
                    $('#listado').css("width", $('#mainLayout').width() - 10 + "px");
                    $("#listado").html(eData.Resultado);

                    $('.dp-row').inputmask({
                        mask: "1/2/y h:s:s",
                        placeholder: "dd/mm/yyyy 00:00:00",
                        alias: "datetime",
                        hourFormat: "24",
                    });
                    $('.dp-row').Zebra_DatePicker({
                        readonly_element: false,
                        format: 'd/m/Y',
                        onSelect: function (date) {
                           /* $('.dp-row').val(date + " 00:00:00");*/
                        }
                    });

                    //$('#dtEditFecVen').inputmask({
                    //    mask: "1/2/y h:s:s",
                    //    placeholder: "dd/mm/yyyy HH:mm:ss",
                    //    alias: "datetime",
                    //    hourFormat: "24",
                    //});
                    //$('#dtEditFecVen').Zebra_DatePicker({
                    //    readonly_element: false,
                    //    //direction: [$('#dtEditEveIni').val().split(" ", 1)[0], false],
                    //    format: 'd/m/Y',
                    //    onSelect: function (date) {
                    //        $('#dtEditFecVen').val(date + " 00:00:00");
                    //    }
                    //});
                }
            }
        },
        error: function (err) {
            alert("Ha ocurrido un error");
        }
    });
}

