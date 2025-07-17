var controlador = siteRoot + 'demandaMaxima/AmpliacionPlazo/';

$(function () {

    $('#btnConsultar').click(function () {
        pintarPaginado();
        pintarBusqueda(1);
    });

    $('#periodo').Zebra_DatePicker({
        format: 'm/Y'
    });

    $('#fechaPeriodo').Zebra_DatePicker({
        format: 'm/Y',
        onSelect: function (date) {
            $('#fechaPeriodo').val(date);
            inicializarFechaAmpliacion(date);
        }
    });

    $('#fechaAmpliacion').Zebra_DatePicker({
        format: 'd/m/Y'
    });

    $('#btnAmpliarPlazo').click(function () {
        //pintarPaginado();
        mostrarPopupAmpliacion();
    });

    $('#btnCancelar').click(function () {
        //pintarPaginado();
        cancelar();
    });

    $('#btnGuardar').click(function () {
        //pintarPaginado();
        guardar();
    });

    $('#empresa').multipleSelect({
        width: '300px',
        filter: true,
        onClick: function (view) {
        },
        onClose: function (view) {
        }
    });
    listarEmpresasMultiple();
    //changeZona();
    //crearPupload();
});

function mostrarPopupAmpliacion() {
    $('#empresa').multipleSelect('refresh');
    var empresaSeleccionada = $('#cbEmpresa').val();
    if (empresaSeleccionada != '0') {
        $("#empresa").multipleSelect('setSelects', [empresaSeleccionada]);
    } else {
        $("#empresa").multipleSelect('checkAll');
    }
    var periodoSeleccionado = $('#periodo').val();
    $('#hdnEmpresa').val('');
    if (periodoSeleccionado != '') {
        $('#fechaPeriodo').val(periodoSeleccionado);
        var fechaInicio = '01/' + periodoSeleccionado;
        $('#fechaAmpliacion').Zebra_DatePicker({
            format: 'd/m/Y',
            start_date: fechaInicio
        });
    } else {
        $('#fechaAmpliacion').val('');
    }

    $("#popupEdicion").bPopup({
        autoClose: false
    });
}
function inicializarFechaAmpliacion(date) {
    var fechaInicio = '01/' + date;
    $('#fechaAmpliacion').Zebra_DatePicker({
        format: 'd/m/Y',
        start_date: fechaInicio
    });
}
function cancelar() {
    $('#popupEdicion').bPopup().close();
}
pintarPaginado = function () {

    $.ajax({
        type: 'POST',
        url: controlador + "paginado",
        data: {
            empresa: $('#cbEmpresa').val(),
            periodo: $('#periodo').val()
        },
        success: function (evt) {
            $('#paginado').html(evt);
            mostrarPaginado();
        },
        error: function () {
            alert("Ha ocurrido un error");
        }
    });
}


function mostrarExito(mensaje) {
    $('#mensaje').removeClass();
    $('#mensaje').html(mensaje);
    $('#mensaje').addClass('action-exito');
}

function mostrarError(mensaje) {
    $('#mensaje').removeClass();
    $('#mensaje').html(mensaje);
    $('#mensaje').addClass('action-error');
}

function mostrarAlerta(mensaje) {
    $('#mensaje').removeClass();
    $('#mensaje').html(mensaje);
    $('#mensaje').addClass('action-alert');
}

function mostrarMensaje(mensaje) {
    $('#mensaje').removeClass();
    $('#mensaje').html(mensaje);
    $('#mensaje').addClass('action-message');
}

function mostrarPaginado() {
    $.ajax({
        type: 'POST',
        url: controlador + 'paginado',
        success: function (evt) {
            $('#paginado').html(evt);
            mostrarPaginado();
        },
        error: function () {
            alert("Error al cargar el paginado");
        }
    });
}

function pintarBusqueda(id) {
    //var inp1 = document.getElementById('cbEmpresa').value;

    //var suministrador = $('#cbSuministrador').multipleSelect('getSelects');
    //if (suministrador == "[object Object]") suministrador = "";
    //$('#hfSuministrador').val(suministrador);


    $.ajax({
        type: 'POST',
        url: controlador + 'ListarEmpresasAmpliacionPlazo',
        data: {
            empresa: $('#cbEmpresa').val(),
            periodo: $('#periodo').val(),
            nroPagina: id
        },
        success: function (evt) {
            $('#listado').css("width", $('#mainLayout').width() + "px");
            $('#listado').html(evt);
            $('#tabla').dataTable({
                "scrollY": 480,
                "scrollX": true,
                "sDom": 't',
                "ordering": false,
                "bDestroy": true,
                "bPaginate": false,
                "iDisplayLength": 50
            });
            //mostrarMensaje("Consulta generada.")
        },
        error: function () {
            mostrarAlerta("Error al obtener la consulta");
        }
    });
}

function guardar() {

    var empresa = $('#empresa').multipleSelect('getSelects');
    if (empresa == "[object Object]") empresa = "";
    $('#hdnEmpresa').val(empresa);

    var fechaPeriodo = $('#fechaPeriodo').val();
    if (fechaPeriodo == "" || fechaPeriodo == null) {
        alert('No se ha ingresado el periodo.');
        return;
    }

    var fechaAmpliacion = $('#fechaAmpliacion').val();
    if (fechaAmpliacion == "" || fechaAmpliacion == null) {
        alert('No se ha ingresado la fecha de ampliación.');
        return;
    }

    $.ajax({
        type: 'POST',
        url: controlador + 'GuardarAmpliacionPlazo',
        data: {
            empresas: $('#hdnEmpresa').val(),
            periodo: fechaPeriodo,
            fechaAmpliacion: fechaAmpliacion
        },
        dataType: 'json',
        success: function (result) {

            if (result.success) {
                $('#popupEdicion').bPopup().close();
                pintarPaginado();
                pintarBusqueda(1);
                alert("Se ha ingresado la ampliación satisfactoriamente.");
            }
            else {
                alert(result.message);
            }

        },
        error: function () {
            mostrarMensaje('mensaje', 'Ha ocurrido un error', $tipoMensajeError, $modoMensajeCuadro);
        }
    });
}

function listarEmpresasMultiple() {
    $.ajax({
        type: 'POST',
        url: controlador + "ObtenerListaEmpresas",
        dataType: 'json',
        success: function (result) {
            $('#empresa').empty().multipleSelect('refresh');
            $(result).each(function (i, v) { // indice, valor
                var $option = $('<option></option>')
                    .attr('value', v.Emprcodi)
                    .text(v.Emprrazsocial)
                    .prop('selected', false);
                $('#empresa').append($option).change();
            })
            $('#empresa').multipleSelect('refresh');
        },
        error: function () {
            alert("Error al cargar la Lista de Empresas.");
        }
    });
}
