var controlador = siteRoot + 'seguimientorecomendacion/';

var ordenamiento = [
    { Campo: "TIPO", Orden: "asc" },
    { Campo: "EMPRNOMB", Orden: "asc" },
    { Campo: "AREANOMB", Orden: "asc" },
    { Campo: "FAMABREV", Orden: "asc" },
    { Campo: "EQUIABREV", Orden: "asc" },
    { Campo: "EVENINI", Orden: "asc" },
    { Campo: "EVENFIN", Orden: "asc" },
    { Campo: "EVENASUNTO", Orden: "asc" },
    { Campo: "SRMRECFECHARECOMEND", Orden: "asc" },
    { Campo: "SRMRECFECHAVENCIM", Orden: "asc" },
    { Campo: "EQUINOMB", Orden: "asc" },
    { Campo: "SRMSTDDESRIP", Orden: "asc" },
    
    { Campo: "SRMCRTDESRIP", Orden: "asc" },
    { Campo: "SRMRECDIANOTIFPLAZO", Orden: "asc" }
];


$(function () {

    $('#cbFamilia').val($('#hfFamilia').val());
    $('#cbTipoEmpresa').val($('#hfTipoEmpresa').val());
    $('#cbEmpresa').val($('#hfEmpresa').val());

    $('#cbEstado').val($('#hfEstado').val());
    $('#cbCriticidad').val($('#hfCriticidad').val());
    
    $('#FechaDesde').Zebra_DatePicker({
        onSelect: function () {
            buscarEvento();
        }
    });

    $('#FechaHasta').Zebra_DatePicker({
        onSelect: function () {
            buscarEvento();
        }
    });

    $('#cbFamilia').on("change", function () {
        buscarEvento();
    });
    $('#cbTipoEmpresa').change(function () {
        cargarEmpresas();
        buscarEvento();
    });
    $('#cbEmpresa').on("change", function () {
        buscarEvento();
    });
      
    
    $('#btnBuscar').click(function () {
        buscarEvento();
    });

    $(window).resize(function () {
        $('#listado').css("width", $('#mainLayout').width() + "px");
    });

    $('#btnExportar').click(function () {
        exportar(0);
    });
    

    $('#btnAlarma').click(function () {
        alarma();
    });


    $('#cmbConRecomendacion').val($('#hfConRecomendacion').val());  

    
    verRecomendacion();
    verDetalle();
    buscarEvento();
});


alarma = function () {
    $.ajax({
        type: 'POST',
        url: controlador + "recomendacion/envioalarma",        
        success: function (evt) {
           
           
        },
        error: function () {
            
        }
    });
}

verRecomendacion = function()
{
    $('#leydFecha').hide();
    var cambio = 'T';//$('#cmbConRecomendacion').val();
    $('#hfConRecomendacion').val(cambio);
    //$('#cbDetalle').prop('checked', false)
    $('#cbEstado').val(0);
    $('#cbCriticidad').val(0);

    
    if (cambio == "N") {
        mostrarRecomendacion("hidden");
        mostrarEstadoCritic("hidden");
        $('#leydFecha').val("Evento");

        $('#hfConRecomendacion').val('N');
    }
    else {

        mostrarRecomendacion("visible")
        $('#leydFecha').val("Recomendación");

        if ($('#cbDetalle').prop('checked')) {
            mostrarEstadoCritic("visible")
            $('#hfDetRecomendacion').val('S');
        }
        else {
            mostrarEstadoCritic("hidden")
            $('#hfDetRecomendacion').val('N');
        }
    }

}

verDetalle = function()
{
    if ($('#cbDetalle').prop('checked'))
    {
        $('#tdLabelFechaDesde').hide();
        $('#tdFechaDesde').hide();
        $('#tdLabelFechaHasta').hide();
        $('#tdFechaHasta').hide();      
    }
    else
    {
        $('#tdLabelFechaDesde').show();
        $('#tdFechaDesde').show();
        $('#tdLabelFechaHasta').show();
        $('#tdFechaHasta').show();
    }    
}

mostrarRecomendacion = function(ver)
{    
    document.getElementById("grupoNivel").style.visibility = ver;    
}

mostrarEstadoCritic = function (ver) {
    return;
    document.getElementById("txtEstado").style.visibility = ver;
    document.getElementById("CmbEstado").style.visibility = ver;
    document.getElementById("txtCriticidad").style.visibility = ver;
    document.getElementById("CmbCriticidad").style.visibility = ver;
}

buscarEvento = function()
{
    pintarPaginado();
    $('#hfCampo').val('EVENINI');
    $('#hfOrden').val('desc');
    mostrarListado(1);
}

closeImportar = function ()
{
    $('#divLeyenda').css('display', 'none');
}

pintarPaginado = function()
{  
    var nivelrec = obtenerNivelRecomendacion();
    $('#hfDetRecomendacion').val(nivelrec);

    $.ajax({
        type: 'POST',
        url: controlador + "gestion/paginado",
        data: $('#frmBusqueda').serialize(),
        success: function (evt) {
            $('#paginado').html(evt);
            mostrarPaginado();
        },
        error: function () {
            mostrarError();
        }
    });
}

cargarEmpresas = function()
{
    var controlador2 = siteRoot + 'eventos/'
    $.ajax({
        type: 'POST',
        url: controlador2 + 'evento/cargarempresas',
        dataType: 'json',
        data: { idTipoEmpresa: $('#cbTipoEmpresa').val() },
        cache: false,
        success: function (aData) {
            $('#cbEmpresa').get(0).options.length = 0;
            $('#cbEmpresa').get(0).options[0] = new Option("--SELECCIONE--", "");
            $.each(aData, function (i, item) {
                $('#cbEmpresa').get(0).options[$('#cbEmpresa').get(0).options.length] = new Option(item.Text, item.Value);
            });
        },
        error: function () {
            mostrarError();
        }
    });
}


ordenar = function (elemento) {
    order = $.grep(ordenamiento, function (e) { return e.Campo == elemento; })[0].Orden;
    $('#hfCampo').val(elemento);
    $('#hfOrden').val(order);
    $.each(ordenamiento, function () {
        if (this.Campo == elemento) {
            this.Orden = (order == "asc") ? "desc" : "asc";
        }
    });
    mostrarListado(1);
}

mostrarListado = function(nroPagina)
{

    var rec = obtenerRecomendacion();    
    var nivelrec = obtenerNivelRecomendacion();
    $('#hfDetRecomendacion').val(nivelrec);

    $('#hfNroPagina').val(nroPagina);
    $.ajax({
        type: 'POST',
        url: controlador + "gestion/lista",
        data:  $('#frmBusqueda').serialize(),
        success: function (evt) {
            $('#listado').css("width", $('#mainLayout').width() + "px");

            $('#listado').html(evt);
            $('#tabla').dataTable({
                "scrollY": 430,
                "scrollX": true,
                "sDom": 't',
                "ordering": false,
                "iDisplayLength": 1000
            });
        },
        error: function () {
            mostrarError();
        }
    });
}

exportar = function(indicador)
{
    var nivelrec = obtenerNivelRecomendacion();
    $('#hfDetRecomendacion').val(nivelrec);
    $('#hfIndicador').val(indicador);

    $.ajax({
        type: 'POST',
        url: controlador + "gestion/exportarseguimiento",
        dataType: 'json',
        cache: false,
        data: $('#frmBusqueda').serialize(),
        success: function (resultado) {
            if (resultado == 1) {
                location.href = controlador + "gestion/descargarseguimiento";
            }
            else {
                mostrarError();
            }
        },
        error: function () {
            mostrarError();
        }
    });
}


pintarBusqueda = function(nroPagina)
{
    mostrarListado(nroPagina);
}


verSeguimiento = function(id)
{
    
    document.location.href = siteRoot +"seguimientorecomendacion/recomendacion/index?id=" + id;

}

eliminarEvento = function (id) {
    if (confirm('¿Está seguro de realizar esta operación?')) {
        $.ajax({
            type: 'POST',
            url: controlador + "gestion/eliminarevento",
            data: {
                id: id
            },
            dataType: 'json',
            success: function (result) {
                if (result == 1) {
                    buscarEvento();
                }
                else {
                    mostrarError();
                }
            },
            error: function () {

            }
        });
    }
}

mostrarError = function ()
{
    alert('Ha ocurrido un error.');
}

obtenerRecomendacion = function () {
    
    return $('#cmbConRecomendacion').val();
   
}

obtenerNivelRecomendacion = function () {

    if ($('#cbDetalle').prop('checked')) {
        return 'S';
    }
    else
    {
        return 'N';
    }
}
