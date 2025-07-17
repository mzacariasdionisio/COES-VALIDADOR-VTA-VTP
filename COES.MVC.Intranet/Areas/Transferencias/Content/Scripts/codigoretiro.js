var controler = siteRoot + "transferencias/codigoretiro/";

$(document).ready(function () {
    buscar();

    $('.txtFecha').Zebra_DatePicker({
    });
   
    $('#btnBuscar').click(function () {
        buscar();
    });

    $('#btnGenerarExcel').click(function () {
        generarExcel();
    });
});

//Funciones de eliminado de registro
function addDeleteEvent() {
    $(".delete").click(function (e) {
        e.preventDefault();
        if (confirm("¿Desea dar de baja el código?")) {
            id = $(this).attr("id").split("_")[1];
            $.ajax({
                type: "post",
                dataType: "text",
                url: controler + "Delete/" + id,
                data: addAntiForgeryToken({ id: id }),
                success: function (resultado) {
                    if (resultado == "true")
                        alert("El registro se dio de baja");
                    else
                        alert("No se ha logrado eliminar el registro");
                }
            });
        }
    });
};

addAntiForgeryToken = function (data) {
    data.__RequestVerificationToken = $('input[name=__RequestVerificationToken]').val();
    return data;
};

function buscar() {
    pintarPaginado();
    mostrarListado(1);
}

function pintarPaginado() {
    $.ajax({
        type: 'POST',
        url: controler + "paginado",
        data: { nombreEmp: $("#EMPRCODI option:selected").text(), tipousu: $("#TIPOUSUACODI option:selected").text(), tipocont: $("#TIPOCONTCODI option:selected").text(), barr: $("#BARRCODI option:selected").text(), clinomb: $("#CLICODI option:selected").text(), fechaInicio: $('#txtfechaIni').val(), fechaFin: $('#txtfechaFin').val(), Solicodiretiobservacion: $('[name="SOLICODIRETIOBSERVACION"]:radio:checked').val(), radiobtn: $('[name="ESTADOLIST"]:radio:checked').val(), codretiro: $('#txtCodigoRetiro').val() },
        success: function (evt) {
            $('#paginado').html(evt);
            mostrarPaginado();
        },
        error: function () {
            alert("Lo sentimos, ha ocurrido un error inesperado");
        }
    });
}

function mostrarListado(nroPagina) {
    $('#hfNroPagina').val(nroPagina);
    $.ajax({
        type: 'POST',
        url: controler + "lista",
        data: { nombreEmp: $("#EMPRCODI option:selected").text(), tipousu: $("#TIPOUSUACODI option:selected").text(), tipocont: $("#TIPOCONTCODI option:selected").text(), barr: $("#BARRCODI option:selected").text(), clinomb: $("#CLICODI option:selected").text(), fechaInicio: $('#txtfechaIni').val(), fechaFin: $('#txtfechaFin').val(), Solicodiretiobservacion: $('[name="SOLICODIRETIOBSERVACION"]:radio:checked').val(), radiobtn: $('[name="ESTADOLIST"]:radio:checked').val(), codretiro: $('#txtCodigoRetiro').val(), NroPagina: $('#hfNroPagina').val() },
        success: function (evt) {
            $('#listado').css("width", $('#mainLayout').width() + "px");
            $('#listado').html(evt);
            addDeleteEvent();
            viewEvent();
            $('#tabla').dataTable({
                "scrollY": 430,
                "scrollX": true,
                "sDom": 't',
                "ordering": false,
                "iDisplayLength": 50
            });
        },
        error: function () {
            alert("Lo sentimos, ha ocurrido un error inesperado");
        }
    });
}

function pintarBusqueda(nroPagina) {
    mostrarListado(nroPagina);
}

generarExcel = function () {
    $.ajax({
        type: 'POST',
        url: controler + 'generarexcel',
        dataType: 'json',
        success: function (result) {
            if (result == 1) {
                window.location = controler + 'abrirexcel';
            }
            if (result == -1) {
                mostrarError();
            }
        },
        error: function () {
            mostrarError();
        }
    });
}

function viewEvent() {

    $('.view').click(function () {
        id = $(this).attr("id").split("_")[1];
        abrirPopup(id);
    });
};

abrirPopup = function (id) {

    $.ajax({
        type: 'POST',
        url: controler + "View/" + id,
        success: function (evt) {
            $('#popup').html(evt);

            setTimeout(function () {
                $('#popup').bPopup({
                    easing: 'easeOutBack',
                    speed: 450,
                    transition: 'slideDown'
                });
            }, 50);
        },
        error: function () {
            alert("Lo sentimos, ha ocurrido un error inesperado");
        }
    });
}

mostrarError = function () {
    alert("Ha ocurrido un error.");
}