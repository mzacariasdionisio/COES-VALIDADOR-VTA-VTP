var controlador = siteRoot + 'eventos/evento/'

$(function () {

    $('#txtFechaInicio').Zebra_DatePicker({               
    });

    $('#txtFechaHasta').Zebra_DatePicker({      
    });   
    
    $('#btnConsultar').click(function () {
        buscarEvento();
    });

    $('#btnAceptarEmpresa').click(function () {
        verInforme();
    });

    buscarEvento();
});

buscarEvento = function()
{
    pintarPaginado();
    mostrarListado(1);
}

pintarPaginado = function () {
    $.ajax({
        type: 'POST',
        url: controlador + "paginado",
        data: {
            fechaInicio: $('#txtFechaInicio').val(),
            fechaFin: $('#txtFechaHasta').val(),
            idTipoEvento: $('#cbTipoEvento').val()
        },
        success: function (evt) {
            $('#paginado').html(evt);
            mostrarPaginado();
        },
        error: function () {
            mostrarError();
        }
    });
}

mostrarListado = function (nroPagina) {
    $('#hfNroPagina').val(nroPagina);
    $.ajax({
        type: 'POST',
        url: controlador + "listado",
        data: {
            fechaInicio: $('#txtFechaInicio').val(),
            fechaFin: $('#txtFechaHasta').val(),
            idTipoEvento: $('#cbTipoEvento').val(),
            nroPagina: nroPagina
        },
        success: function (evt) {
            $('#listado').css("width", $('#mainLayout').width() + "px");

            $('#listado').html(evt);
            $('#tabla').dataTable({
                "scrollY": 430,
                "scrollX": true,
                "sDom": 't',
                "ordering": false,
                "iDisplayLength": 50
            });
        },
        error: function () {
            mostrarError();
        }
    });
}

pintarBusqueda = function (nroPagina) {
    mostrarListado(nroPagina);
}

cargarInforme = function (idEvento) {
    
    var indicador = $('#hfIndEmpresa').val();
    if (indicador != -1) {

        if (indicador == 2) {
            var empresa = $('#hfIdEmpresa').val();
            document.location.href = siteRoot + 'eventos/informe?evento=' + idEvento + '&empresa=' + empresa;
        }
        else
        {
            $('#hfIdEvento').val(idEvento);
            $('#popupEmpresa').bPopup({
                easing: 'easeOutBack',
                speed: 450,
                transition: 'slideDown'
            });
        }
    }
    else {
        mostrarAlerta("Usuario no tiene empresa asignada");
    }
}

cargarOtrosInformes = function (idEvento){
    cargarEstadoInforme(idEvento, 'S');
}

verEstadoInformes = function (idEvento) {
    cargarEstadoInforme(idEvento, 'N');
}

cargarEstadoInforme = function(idEvento, indicador){
    
    $.ajax({
        type: 'POST',
        url: controlador + "informe",
        data: {
            idEvento: idEvento,
            indicador: indicador
        },
        success: function (evt) {
            $('#contenedorInforme').html(evt);

            setTimeout(function () {
                $('#popupInforme').bPopup({
                    easing: 'easeOutBack',
                    speed: 450,
                    transition: 'slideDown'
                });

                $('#tablaReporte').dataTable({
                    "sPaginationType": "full_numbers",
                    "aaSorting": [[0, "desc"]],
                    "destroy": "true",
                    "sDom": 'ftp',
                });

            }, 50);
        },
        error: function () {
            mostrarError();
        }
    });
}

verInforme = function () {    
    var empresa = $('#cbEmpresaUsuario').val();
    var idEvento = $('#hfIdEvento').val();

    if (empresa != "") {
        document.location.href = siteRoot + 'eventos/informe?evento=' + idEvento + '&empresa=' + empresa;
    }
    else {
        mostrarAlerta("Seleccione empresa");
    }
}

showInforme = function (idEmpresa, idEvento)
{
    document.location.href = siteRoot + 'eventos/informe?evento=' + idEvento + '&empresa=' + idEmpresa;
}


mostrarError = function () {
    $('#mensaje').removeClass();
    $('#mensaje').addClass('action-error');
    $('#mensaje').html("Ha ocurrido un error.");    
}

mostrarAlerta = function (mensaje)
{
    alert(mensaje);
}