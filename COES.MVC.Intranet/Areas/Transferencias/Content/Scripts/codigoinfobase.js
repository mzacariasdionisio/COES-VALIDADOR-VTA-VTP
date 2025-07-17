var controler = siteRoot + "transferencias/codigoinfobase/";

$(document).ready(function () {
    buscar();

    $('#CENTGENECODI2').multipleSelect({
        width: '200px',
        filter: true,
        selectAll: false,
        single: true


    });

    $('#BARRCODI2').multipleSelect({
        width: '200px',
        filter: true,
        selectAll: false,
        single: true


    });

    $('#EMPRCODI2').multipleSelect({
        width: '200px',
        filter: true,
        selectAll: false,
        single: true

    });

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
        if (confirm("¿Desea eliminar la información seleccionada?")) {
            id = $(this).attr("id").split("_")[1];
            $.ajax({
                type: "post",
                dataType: "text",
                url: controler + "Delete/" + id,
                data: addAntiForgeryToken({ id: id }),
                success: function (resultado) {
                    if (resultado == "true")
                        $("#fila_" + id).remove();
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
    var estado;
    if ($('#Activo').is(':checked')) {
        estado = 'ACT'; //alert('Activo');
    }
    if ($('#Inactivo').is(':checked')) {
        estado = 'INA'; //alert('Inactivo');
    }
    if ($('#Todos').is(':checked')) {
        estado = 'TODOS'; //alert('Inactivo');
    }
    $.ajax({
        type: 'POST',
        url: controler + "paginado",
        data: { nombreEmp: $("#EMPRCODI option:selected").text(), centralGene: $("#CENTGENECODI option:selected").text(), nombreBarra: $("#BARRCODI option:selected").text(), fechaInicio: $('#txtfechaIni').val(), fechaFin: $('#txtfechaFin').val(), estado: estado, codinfobase: $('#txtCodInfoBase').val() },
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
    var estado;
    if ($('#Activo').is(':checked')) {
        estado = 'ACT'; //alert('Activo');
    }
    if ($('#Inactivo').is(':checked')) {
        estado = 'INA'; //alert('Inactivo');
    }
    if ($('#Todos').is(':checked')) {
        estado = 'TODOS'; //alert('Inactivo');
    }
    $.ajax({
        type: 'POST',
        url: controler + "lista",
        data: { nombreEmp: $("#EMPRCODI option:selected").text(), centralGene: $("#CENTGENECODI option:selected").text(), nombreBarra: $("#BARRCODI option:selected").text(), fechaInicio: $('#txtfechaIni').val(), fechaFin: $('#txtfechaFin').val(), estado: estado, codinfobase: $('#txtCodInfoBase').val(), NroPagina: $('#hfNroPagina').val() },
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
                alert("Lo sentimos, se ha producido un error");
            }
        },
        error: function () {
            alert("Lo sentimos, se ha producido un error");
        }
    });
}