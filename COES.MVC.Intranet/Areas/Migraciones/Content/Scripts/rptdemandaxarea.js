var controlador = siteRoot + 'Migraciones/Reporte/'

$(function () {
    $('#txtFechaini').Zebra_DatePicker({
        pair: $('#txtFechafin'),
        onSelect: function (date) {
            $('#txtFechafin').val($('#txtFechaini').val());
        }
    });
    $('#txtFechafin').Zebra_DatePicker({
        direction: true
    });
    $('#txtMesanio').Zebra_DatePicker({
        format: 'm/Y'
    });
    $('#txtMesanioini').Zebra_DatePicker({
        format: 'm/Y',
        pair: $('#txtMesaniofin'),
        onSelect: function (date) {
            var date1 = getFecha("01/" + date);
            var date2 = getFecha("01/" + $('#txtMesaniofin').val());
            if (date1 > date2) {
                $('#txtMesaniofin').val(date);
            }
        }
    });
    $('#txtMesaniofin').Zebra_DatePicker({
        format: 'm/Y',
        direction: true
    });
    $('#txtAnio').Zebra_DatePicker({
        format: 'Y',
        onSelect: function () {
            cargarSemanaAnho();
        }
    });
    $('#txtAnioini').Zebra_DatePicker({
        format: 'Y',
        pair: $('#txtAniofin'),
        onSelect: function (date) {
            var date1 = getFecha("01/01/" + date);
            var date2 = getFecha("01/01/" + $('#txtAniofin').val());
            if (date1 > date2) {
                $('#txtAniofin').val(date);
            }
        }
    });
    $('#txtAniofin').Zebra_DatePicker({
        format: 'Y',
        direction: true
    });

    $('#cbTipo').change(function (e) {
        evtView(this.value);
    });

    $("#btnConsultar").click(function () {
        cargarInformacion(1, 0);
    });

    $("#btnExportar").click(function () {
        var chk = 2;
        if ($('#rd1').is(":checked")) { chk = 1; }
        cargarInformacion(2, chk);
    });

    $('#btnConfigurarReporte').on('click', function () {
        abrirVentanaConfigReporte();
    });

    for (i = 5; i <= 18; i++) { $('#td' + i).hide(); }
});

function mostrarBtnConfigReporte() {
    $("#tdConfigRpt").hide();
    var tipoSelec = parseInt($('#cbTipo').val());
    if (tipoSelec == 1) {
        $("#tdConfigRpt").show();
    }
}
function abrirVentanaConfigReporte() {
    var idReporte = $('#hfIdReporte').val();
    var url = siteRoot + 'ReportesMedicion/formatoreporte/IndexDetalle?id=' + idReporte;
    window.open(url, '_blank');
}

function cargarSemanaAnho() {
    $("#cbSemanaini").empty();
    $("#cbSemanafin").empty();
    $.ajax({
        type: 'POST',
        url: controlador + 'CargarSemanas',

        data: { anio: $('#txtAnio').val() },

        success: function (data) {
            if (data.ListaSemanas.length > 0) {
                $.each(data.ListaSemanas, function (i, item) {
                    $('#cbSemanaini').get(0).options[$('#cbSemanaini').get(0).options.length] = new Option(item.NombreTipoInfo, item.IdTipoInfo);
                });
                $.each(data.ListaSemanas, function (i, item) {
                    $('#cbSemanafin').get(0).options[$('#cbSemanafin').get(0).options.length] = new Option(item.NombreTipoInfo, item.IdTipoInfo);
                });
            }
        },
        error: function () {
            alert("Ha ocurrido un error");
        }
    });
}

function evtView(x) {
    $('#listado').hide();
    for (i = 1; i <= 18; i++) { $('#td' + i).hide(); }
    switch (x) {
        case "1":
        case "2": for (i = 1; i <= 4; i++) { $('#td' + i).show(); } break;
        case "3": for (i = 5; i <= 10; i++) { $('#td' + i).show(); cargarSemanaAnho(); } break;
        case "4": for (i = 11; i <= 14; i++) { $('#td' + i).show(); } break;
        case "5": for (i = 15; i <= 18; i++) { $('#td' + i).show(); } break;
    }

    mostrarBtnConfigReporte();
}

function cargarInformacion(x_, y_) {
    var fec1_, fec2_;
    var anio_, sem1_, sem2_;
    var mes1_, mes2_;
    var anio1_, anio2_;
    var controller_ = "";
    var fixedColumns = 1;
    switch ($('#cbTipo').val()) {
        case "1": fec1_ = $("#txtFechaini").val(); fec2_ = $("#txtFechafin").val(); break;
        case "2": fec1_ = $("#txtFechaini").val(); fec2_ = $("#txtFechafin").val(); fixedColumns = 6; break;
        case "3": anio_ = $("#txtAnio").val(); sem1_ = $("#cbSemanaini").val(); sem2_ = $("#cbSemanafin").val(); break;
        case "4": mes1_ = $("#txtMesanioini").val(); mes2_ = $("#txtMesaniofin").val(); break;
        case "5": anio1_ = $("#txtAnioini").val(); anio2_ = $("#txtAniofin").val(); break;
    }

    $.ajax({
        type: 'POST',
        url: controlador + "CargarInformacionDemanda",
        dataType: 'json',
        data: {
            tipo: $('#cbTipo').val(), fec1: fec1_, fec2: fec2_, anio: anio_, sem1: sem1_, sem2: sem2_, mes1: mes1_, mes2: mes2_, anio1: anio1_, anio2: anio2_, xx: x_, yy: y_
        },
        success: function (evt) {
            if (x_ == 1) {
                $('#listado').show();
                $('#listado').css("width", ($('#mainLayout').width() - 20) + "px");
                $('#listado').html(evt.Resultado);
                if (evt.nRegistros > 0) {

                    if ($('#cbTipo').val() == "1") {
                        setTimeout(() => {
                            $('#tb_info').dataTable({
                                "scrollX": true,
                                "scrollY": false,
                                "scrollCollapse": false,
                                "bAutoWidth": false,
                                "sDom": 't',
                                "ordering": false,
                                paging: false,
                                "destroy": "true",
                                fixedColumns: {
                                    leftColumns: fixedColumns
                                }
                            });
                        }, 500);
                    } else {
                        $('#tb_info').dataTable({
                            "scrollX": true,
                            "scrollY": "780px",
                            "scrollCollapse": true,
                            "sDom": 'ft',
                            "ordering": false,
                            paging: false,
                            fixedColumns: {
                                leftColumns: fixedColumns
                            }
                        });
                    }
                }
            } else {
                switch (evt.nRegistros) {
                    case 1: window.location = controlador + "Exportar?fi=" + evt.Resultado; break;// si hay elementos
                    case 2: alert("No existen registros !"); break;// sino hay elementos
                    case -1: alert("Error en reporte result"); break;// Error en C#
                }
            }
        },
        error: function (err) { alert("Error..!!"); }
    });
}
