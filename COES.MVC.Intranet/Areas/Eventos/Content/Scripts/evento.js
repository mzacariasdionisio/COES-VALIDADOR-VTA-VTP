var controlador = siteRoot + 'eventos/'

var ordenamiento = [
    { Campo: "TIPOEVENABREV", Orden: "desc" },
    { Campo: "EMPRNOMB", Orden: "asc" },
    { Campo: "AREADESC", Orden: "asc" },
    { Campo: "FAMABREV", Orden: "asc" },
    { Campo: "EQUIABREV", Orden: "asc" },
    { Campo: "EVENINI", Orden: "asc" },
    { Campo: "CAUSAEVENABREV", Orden: "asc" },
    { Campo: "SUBCAUSAABREV", Orden: "asc" },
    { Campo: "EVENINTERRUP", Orden: "asc" },
    { Campo: "EVENRELEVANTE", Orden: "asc" },
    { Campo: "EVENASUNTO", Orden: "asc" },
    { Campo: "EVENFIN", Orden: "asc" },
    { Campo: "LASTUSER", Orden: "asc" },
    { Campo: "LASTDATE", Orden: "asc" }
];


$(function () {

    $('#FechaDesde').Zebra_DatePicker({
        onSelect: function () {
            buscarEvento();
        }
    });

    $('#cbVersion').val($('#hfVersion').val());
    $('#cbTurno').val($('#hfTurno').val());
    $('#cbFamilia').val($('#hfFamilia').val());
    $('#cbTipoEmpresa').val($('#hfTipoEmpresa').val());
    $('#cbEmpresa').val($('#hfEmpresa').val());
    $('#cbInterrupcion').val($('#hfInterrupcion').val());
    $('#cbTipoEvento').val($('#hfTipoEvento').val());
    //$('#cbFamilia').val("0");
    //$('#cbEmpresa').val("0");
    //$('#cbTipoEvento').val("0");
    

    $('#FechaHasta').Zebra_DatePicker({
        onSelect: function () {
            buscarEvento();
        }
    });

    $('#cbVersion').on("change", function () {
        buscarEvento();
    });
    $('#cbTurno').on("change", function () {
        buscarEvento();
    });
    $('#cbFamilia').on("change", function () {
        buscarEvento();
    });    
    $('#cbEmpresa').on("change", function () {
        buscarEvento();
    });
    $('#cbInterrupcion').on("change", function () {
        buscarEvento();
    });
    $('#cbTipoEvento').on("change", function () {
        buscarEvento();
    });

    buscarEvento();
    
    $('#btnBuscar').click(function () {
        buscarEvento();
    });

    $('#cbTipoEmpresa').change(function () {
        cargarEmpresas();
        buscarEvento();
    });

    $(window).resize(function () {
        $('#listado').css("width", $('#mainLayout').width() + "px");
    });

    $('#btnExportar').click(function () {
        exportar(0);
    });

    $('#btnExportarDetalle').click(function () {
        exportar(1);
    });
 
    $('#btnNuevo').click(function () {
        document.location.href = siteRoot + 'eventos/registro/final';
    });

    $('#btnBitacora').click(function () {
        document.location.href = siteRoot + 'eventos/registro/bitacora';
    });

    $('#btnAseguramiento').click(function () {
        document.location.href = siteRoot + 'eventos/registro/aseguramiento';
    });
     
    $('#btnLeyenda').click(function () {
        $('#divLeyenda').css('display', 'block');
    });

    $('#btnEventoCtaf').click(function () {
        generarEventoCtaf();
    });
});

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
    $.ajax({
        type: 'POST',
        url: controlador + "evento/paginado",
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
    $.ajax({
        type: 'POST',
        url: controlador + 'evento/cargarempresas',
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


function ordenar(elemento) {
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
    $('#hfNroPagina').val(nroPagina);
    $.ajax({
        type: 'POST',
        url: controlador + "evento/lista",
        data:  $('#frmBusqueda').serialize(),
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

mostrarLeyenda = function()
{   
    if ($('#leyenda').css("display") == "none") {
        $('#leyenda').css("display", "block");
        $('#spanLeyenda').text("Ocultar leyenda CIER");
    }
    else if ($('#leyenda').css("display") == "block") {
        $('#leyenda').css("display", "none");
        $('#spanLeyenda').text("Mostrar leyenda CIER");
    }
}

exportar = function(indicador)
{
    $('#hfIndicador').val(indicador);

    $.ajax({
        type: 'POST',
        url: controlador + "evento/exportarevento",
        dataType: 'json',
        cache: false,
        data: $('#frmBusqueda').serialize(),
        success: function (resultado) {
            if (resultado == 1) {
                location.href = controlador + "evento/descargarevento";
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

cargarEstadoInforme = function (idEvento)
{
    $.ajax({
        type: 'POST',
        url: controlador + "evento/informe",
        data: {
            idEvento: idEvento
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

pintarBusqueda = function(nroPagina)
{
    mostrarListado(nroPagina);
}

verEvento = function (id, preliminar) {
    if (preliminar == 'S') {
        document.location.href = siteRoot + "eventos/registro/bitacora?id=" + id;
    }
    else {
        //Inicio aplicativo Seg. Recomendaciones
        if (preliminar == 'A') {
            document.location.href = siteRoot + "eventos/registro/aseguramiento?id=" + id;
        }
        else {
            document.location.href = siteRoot + "eventos/registro/final?id=" + id;
        }
        //Fin aplicativo Seg. Recomendaciones
    }
}

copiarEvento = function (id, preliminar)
{
    if (preliminar == 'S') {
        document.location.href = siteRoot + "eventos/registro/bitacora?idCopia=" + id;
    }
    else {
        document.location.href = siteRoot + "eventos/registro/final?idCopia=" + id;
    }
}

consultarInforme = function (id)
{
    cargarEstadoInforme(id);
}

eliminarEvento = function (id)
{
    if (confirm('¿Está seguro de realizar esta acción?'))
    {
        $.ajax({
            type: 'POST',
            url: controlador + "evento/eliminar",
            dataType: 'json',
            cache: false,
            data: { idEvento: id},
            success: function (resultado) {
                if (resultado == 1) {
                    buscarEvento();
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
}

showInforme = function (idEmpresa, idEvento)
{
    document.location.href = siteRoot + 'eventos/informe?evento=' + idEvento + '&empresa=' + idEmpresa;
}

informeConsolidado = function (idEvento, indicador)
{
    document.location.href = siteRoot + 'eventos/informe?evento=' + idEvento + '&empresa=0&indicador=' + indicador;
}

mostrarError = function ()
{
    alert('Ha ocurrido un error.');
}

generarEventoCtaf = function () {
    var correcto = 0;
    var mensaje;
    var _cont = 0;
    var objeto = new Array();
    var inputs = document.querySelectorAll('.chCtaf');
    for (var k = 0; k < inputs.length; k++) {
        if (inputs[k].checked == true) {
            objeto.push(inputs[k].value);
            _cont++;
        }
    }
    if (_cont > 0) {
        $.ajax({
            type: 'POST',
            url: controlador + "evento/ValidarEventoCtaf",
            dataType: 'json',
            cache: false,
            traditional: true,
            data: { objEvento: objeto },
            success: function (resultado) {
                if (resultado != "") {
                    if (expresionRegular(resultado) == 1) {
                        mensaje = "<li>No puedes generar Evento Ctaf con caracteres especiales.</li>";
                        mostrarAlerta(mensaje);
                        correcto++;
                    }
                }
                if (correcto == 0) {
                    if (confirm('¿Está seguro de realizar esta acción?')) {
                        $.ajax({
                            type: 'POST',
                            url: controlador + "evento/generarEventoCtaf",
                            dataType: 'json',
                            cache: false,
                            traditional: true,
                            data: { objEvento: objeto },
                            success: function (resultado) {
                                if (resultado == 1) {
                                    buscarEvento();
                                }
                                else if (resultado == -2) {
                                    mensaje = "<li>No puedes generar Evento Ctaf con nombres tan largos.</li>";
                                    mostrarAlerta(mensaje);
                                }
                                else {
                                    mostrarError("Ocurrió un error.");
                                }
                            },
                            error: function () {
                                mostrarError("Ocurrió un error.");
                            }
                        });
                    }                   
                }
            },
            error: function () {
                mostrarError("Ocurrió un error.");
            }
        });
    }
    else {
        mensaje = "<li>Seleccione al menos un evento.</li>";
        mostrarAlerta(mensaje);
    }

}

const formato = /[`!@#$%^&*+={};'"\\|<>\?~\n]/;
const expresionRegular = (texto) => {
    if (formato.test(texto)) {
        return 1;
    }
    else
        return 0;
}