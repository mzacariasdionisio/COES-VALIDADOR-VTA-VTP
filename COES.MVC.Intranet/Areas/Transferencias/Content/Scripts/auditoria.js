var controler =  siteRoot + 'transferencias/auditoria/'

$(function () {
   

    var date = new Date();
    let day = date.getDate();
    let month = date.getMonth() + 1;
    let year = date.getFullYear();
   
    if (month < 10) {
        var fecha = day + "/" + '0' + month + "/" + year;
    } else {
        var fecha = day + "/" + month + "/" + year;
    }
 

    $('#txtFechaInicial').Zebra_DatePicker({
        pair: $('#txtFechaFinal'),
        onSelect: function (date) {
            var date1 = getFecha(date);
            var date2 = getFecha($('#txtFechaFinal').val());

            if (date1 > date2) {
                $('#txtFechaFinal').val(date);
            }
        }
    });

    $('#txtFechaFinal').Zebra_DatePicker({
        direction: true
    });

    $('#txtFechaInicial').val(fecha);
    $('#txtFechaFinal').val(fecha);

    $('#cbTipoAplicacion').change(function () {
        $(this).find(":selected").each(function () {

            $.ajax({
                type: 'POST',
                url: controler + "TraerComboTipoProceso",
                data: {
                    tipoProceso: $('#cbTipoAplicacion').val()
                },
                success: function (evt) {
                    var newData = JSON.stringify(evt)
                    var jsonDev = JSON.parse(newData);
                    var arrayListaProceso = jsonDev.ListaTipoProceso;
                    $('#cbTipoProceso').empty();
                    var i;
                    for (i = 0; i < arrayListaProceso.length; i++) {
                        $("#cbTipoProceso").append("<option value='" + arrayListaProceso[i].Tipprocodi + "'>" + arrayListaProceso[i].Tipprodescripcion + "</option>");

                    }
                 
                },
                error: function (err) {
                    mostrarError();
                }
            });
            //alert($(this).val());
        });
    });

    $('#btnConsultar').click(function () {
  
        consultarBuscar();
        pintarPaginado();
        //mostrarExito("Ya puede consultar la información");
    });

    $('#btnExporarExcel').click(function () {
        generarExcel();
    });


    consultarBuscar = function () {
        pintarPaginado();
        consultarAuditoriaProceso(1);
    }

    consultarAuditoriaProceso = function (nroPagina) {
        $('#hfNroPagina').val(nroPagina);
        console.log('nro ', $('#hfNroPagina').val());
        var fechainicial = $('#txtFechaInicial').val();
        var fechafinal = $('#txtFechaFinal').val();
        var tipoaplicacion = $('#cbTipoAplicacion').val();
        var tipoproceso = $('#cbTipoProceso').val();
        var audprousucreacion= $('#txtUsuario').val();
        $('#imgHandsontable').css('display', 'block');
        $.ajax({
            type: 'POST',
            url: controler + "lista",
            data: { audprousucreacion: audprousucreacion,tipoproceso: tipoproceso, fechainicial: fechainicial, fechafinal: fechafinal, NroPagina: $('#hfNroPagina').val()},
            success: function (evt) {
                $('#listado').html(evt);
            },
            error: function () {
                alert("Lo sentimos, se ha producido un error");
            }
        });
    };

    getFecha = function (date) {
        var parts = date.split("/");
        var date = new Date(parts[1] + "/" + parts[0] + "/" + parts[2]);
        return date.getTime();
    }

    function pintarPaginado() {
        $.ajax({
            type: 'POST',
            url: controler + "paginado",
            data: {
                audprousucreacion: $('#txtUsuario').val(),
                tipoproceso: $('#cbTipoProceso').val(),
                fechainicial:$('#txtFechaInicial').val(),
                fechafinal:$('#txtFechaFinal').val(),
                tipoaplicacion:$('#cbTipoAplicacion').val(),
                NroPagina: $('#hfNroPagina').val(),
            }, success: function (evt) {
                $('#paginado').html(evt);
                mostrarPaginado();
            },
            error: function () {
                alert("Lo sentimos, ha ocurrido un error inesperado");
            }
        });
    }
});

function pintarBusqueda(nroPagina) {
    consultarAuditoriaProceso(nroPagina);
}


generarExcel = function () {


    if ($('#txtFechaInicial').val() != "" && $('#txtFechaFinal').val() != "") {
        $.ajax({
            type: 'POST',
            url: controler + "GenerarExcel",
            dataType: 'json',
            data: {
                audprousucreacion: $('#txtUsuario').val(),
                tipoproceso: $('#cbTipoProceso').val(),
                fechainicial: $('#txtFechaInicial').val(),
                fechafinal: $('#txtFechaFinal').val(),
                tipoaplicacion: $('#cbTipoAplicacion').val(),
                NroPagina: $('#hfNroPagina').val()
            },
            success: function (result) {
                if (result == 1) {
                    window.location = controler + 'abrirexcel';
                }
                if (result == -1) {
                    alert("Lo sentimos, se ha producido un error");
                }
            },
            error: function (err) {
                mostrarError()
            }
        });
    }
    else {
        mostrarAlerta("Seleccione rango de fechas.");
    }
}
