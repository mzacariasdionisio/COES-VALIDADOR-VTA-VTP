var controlador = siteRoot + "Siosein/RemisionesOsinergmin/";

$(function () {

    $("#btnNuevo").click(function () {
        CargarFormulario();
    });
    $("#btnCancelar").click(function () {
        cancelar();
    });

    $('#txtFecha').Zebra_DatePicker({
        format: 'Y',
        direction: false,
        onSelect: function () {
            listarPeriodo();
        }
    });

    $("#fechaMesPer").Zebra_DatePicker({
        format: "m Y",
        direction: false,
        onSelect: function (periodo) {
            validarPeriodo(periodo);
        }
    });
    $("#fechaUltEnvio").Zebra_DatePicker();
   
    $("#btnGrabarNuevo").click(function () {
        CrearPeriodo();
    });

    listarPeriodo();
});

function CrearPeriodo() {
    $.ajax({
        type: 'POST',
        url: controlador + 'CrearPeriodo',
        dataType: 'json',
        data: {
            periodo: $("#fechaMesPer").val(),
            ultimaFecha: $("#fechaUltEnvio").val()
        },
        success: function (result) {
            if (result) {
                cancelar();
                alert("Éxito");
                window.location.href = controlador + "Index";
            } else {
                alert("Verifique los datos");
            }
        },
        error: function () {
            alert("Error");
        }
    });
}

function CargarFormulario() {
    $.ajax({
        type: 'POST',
        url: controlador + 'CargarViewNuevoPeriodo',
        data: {},
        success: function (result) {
            $("#contenidopopupTablaNuevo").html(result);

            setTimeout(function () {
                $('#popupTablaNuevo').bPopup({
                    easing: 'easeOutBack',
                    speed: 450,
                    transition: 'slideDown',
                    modalClose: false
                });
            }, 50);
        },
        error: function () {
            alert("error");
        }
    });

}



function validarPeriodo(periodo) {
    if ($("#fechaMesPer").val().length === 7) {
        $.ajax({
            type: "POST",
            url: controlador + "ValidarPeriodo",
            data: {
                periodo: periodo
            },
            dataType: "json",
            success: function (result) {
                if (!result) {
                    $("#fechaMesPer").val("");
                    alert("El periodo ya existe");
                }
            },
            error: function () {
                alert("Error");
            }
        });
    }
};

function cancelar() {
    $("#popupTablaNuevo").bPopup().close();
}


function listarPeriodo() {
    if ($("#txtFecha").val() === null) return;

    $.ajax({
        type: "POST",
        url: controlador + "ListadoPeriodo",
        data: {
            anio: $("#txtFecha").val()
        },
        success: function (data) {

            $("#listado").html(data);

            $("#tablaSioCabecera").dataTable({
                "scrollY": 314,
                "scrollX": false,
                "sDom": "t",
                "ordering": false,
                "bDestroy": true,
                "bPaginate": false,
                "iDisplayLength": 50
            });
        },
        error: function () {
            alert("Error");
        }
    });
}

function CargarDetalleCabecera(Cabpriperiodo) {
    window.location.href = controlador + ListaTablasPried;
}