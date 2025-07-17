var controlador = siteRoot + 'hidrologia/'
var table = null;

var ordenamiento = [
    { Campo: "ptomedicodi", Orden: "desc" },
    { Campo: "emprnomb", Orden: "asc" },
    { Campo: "ptomedielenomb", Orden: "asc" },
    { Campo: "tptomedinomb", Orden: "asc" },
    { Campo: "desubicacion", Orden: "asc" },
    { Campo: "famnomb", Orden: "asc" },
    { Campo: "equinomb", Orden: "asc" },
    { Campo: "Areaoperativa", Orden: "asc" },
    { Campo: "Niveltension", Orden: "asc" },
    { Campo: "origlectnombre", Orden: "asc" },
    { Campo: "ptomediestado", Orden: "asc" },
    { Campo: "lastuser", Orden: "asc" },
    { Campo: "lastdate", Orden: "asc" }
];

$(function () {

    $('#cbOrigLectura').val("0");

    $('#cbEmpresa').multipleSelect({
        width: '200px',
        filter: true
    });

    $('#cbOrigLectura').multipleSelect({
        width: '200px',
        filter: true
    });

    $('#cbTipoPuntoMedicion').multipleSelect({
        width: '200px',
        filter: true
    });

    $('#cbFamilia').multipleSelect({
        width: '200px',
        filter: true
    });

    $('#cbTipoGrupoFiltro').multipleSelect({
        width: '200px',
        filter: true
    });

    $('#cbUbicacion').multipleSelect({
        width: '200px',
        filter: true
    });

    $('#btnBuscar').click(function () {
        buscarPtoMedicion();
    });

    $('#btnNuevo').click(function () {
        nuevoPtoMedicion(0);
    });

    $('#btnExportar').click(function () {
        exportarExcel();
    });

    $('#cbOrigLectura').change(function () {
        cargarTipoPto();
    });

    $('#cbTipoPuntoFiltro').change(function () {
        mostrarXTipoPuntoFiltro();
    });

    $('.trGrupo').hide();
    mostrarXTipoPuntoFiltro();

    cargarPrevio()
    buscarPtoMedicion();

    $("#btnPtoCal").click(function () { window.location.href = siteRoot + 'ReportesMedicion/formatoreporte/IndexPtoCal'; })

    $(window).resize(function () {
        $('#listado').css("width", $('#mainLayout').width() + "px");
    });
});

function mostrarXTipoPuntoFiltro() {

    if ($('#cbTipoPuntoFiltro').val() == 1) {
        $('.trEquipo').show();
        $('.trGrupo').hide();
        // FIT - VALORIZACION DIARIA
        $('.trLectura').show();
        $('.trUbicacion').show();
        $('.trBarra').hide();
        $('.trCliente').hide();
        // FIT - VALORIZACION DIARIA
    }
    if ($('#cbTipoPuntoFiltro').val() == 2) {
        $('.trEquipo').hide();
        $('.trBarra').hide();
        // FIT - VALORIZACION DIARIA
        $('.trLectura').show();
        $('.trUbicacion').show();
        $('.trCliente').hide();
        $('.trGrupo').show();
        // FIT - VALORIZACION DIARIA
    }
    // FIT - VALORIZACION DIARIA
    if ($('#cbTipoPuntoFiltro').val() == 3) {
        $('.trEquipo').hide();
        $('.trGrupo').hide();
        $('.trGrupo').hide();
        $('.trGrupo').hide();
        $('.trLectura').hide();
        $('.trUbicacion').hide();
        $('.trBarra').show();
        $('.trCliente').show();
        $('.trTipoPunto').hide();
    }
        // FIT - VALORIZACION DIARIA
}

function cargarPrevio() {
    $('#cbEmpresa').multipleSelect('checkAll');
    $('#cbOrigLectura').multipleSelect('checkAll');
    $('#cbTipoPuntoMedicion').multipleSelect('checkAll');
    $('#cbFamilia').multipleSelect('checkAll');
    $('#cbUbicacion').multipleSelect('checkAll');
    $('#cbTipoGrupoFiltro').multipleSelect('checkAll');
}

function buscarPtoMedicion() {

    if ($('#cbEmpresa').multipleSelect('rowCountSelected') <= 100 || $('#cbEmpresa').multipleSelect('isAllSelected') == "S") {

        if ($('#cbUbicacion').multipleSelect('rowCountSelected') <= 100 || $('#cbUbicacion').multipleSelect('isAllSelected') == "S") {

            pintarPaginado();
            mostrarListado(1, 'ptomedicodi', 'asc');
            $('#hfCampo').val('ptomedicodi');
            $('#hfOrden').val('asc');
        }
        else {
            alert("No puede seleccionar más de 100 ubicaciones.");
        }
    }
    else {
        alert("No puede seleccionar más de 100 empresas.");
    }
}

function nuevoPtoMedicion(id) {
    //window.location.href = controlador + 'ptomedicion/editar';

    $.ajax({
        type: 'POST',
        url: controlador + "ptomedicion/editar?id=" + id,
        success: function (evt) {
            $('#agregarPto').html(evt);
            setTimeout(function () {
                $('#popupUnidad').bPopup({
                    easing: 'easeOutBack',
                    speed: 450,
                    transition: 'slideDown'
                });
            }, 50);
        },
        error: function () {
            mostrarError();
        }
    });
}

function pintarBusqueda(nroPagina) {
    mostrarListado(nroPagina, $('#hfCampo').val(), $('#hfOrden').val());
}

function pintarPaginado() {

    var empresa = $('#cbEmpresa').multipleSelect('getSelects');
    var origen = $('#cbOrigLectura').multipleSelect('getSelects');
    var tipopto = $('#cbTipoPuntoMedicion').multipleSelect('getSelects');
    var familia = $('#cbFamilia').multipleSelect('getSelects');
    var categoria = $('#cbTipoGrupoFiltro').multipleSelect('getSelects');
    var ubicacion = $('#cbUbicacion').multipleSelect('getSelects');

    if (tipopto == "[object Object]") tipopto = "-1";
    $('#hfTipoPuntoMedicion').val(tipopto);
    $('#hfEmpresa').val($('#cbEmpresa').multipleSelect('isAllSelected') == "S" ? "-1" : empresa);

    $('#hfOrigLectura').val(origen);
    $('#hfFamilia').val(familia);
    $('#hfCategoriaFiltro').val(categoria);
    //$('#hfUbicacion').val(ubicacion);
    $('#hfUbicacion').val($('#cbUbicacion').multipleSelect('isAllSelected') == "S" ? "-1" : ubicacion);

    $.ajax({
        type: 'POST',
        url: controlador + "ptomedicion/paginado",
        data: {
            empresas: $('#hfEmpresa').val(),
            tipoOrigenLectura: $('#hfOrigLectura').val(),
            tipoPtomedicodi: $('#hfTipoPuntoMedicion').val(),
            idsFamilia: $('#hfFamilia').val(),
            ubicacion: $('#hfUbicacion').val(),
            categoria: $('#hfCategoriaFiltro').val(),
            tipoPunto: $('#cbTipoPuntoFiltro').val(),
            codigo: $('#txtCodigoPunto').val(),
            cliente: $('#cbTipoCliente').val(),
            barra: $('#cbTipoBarra').val(),
            nroPagina: 1
        },
        success: function (evt) {
            $('#paginado').html(evt);
            mostrarPaginado();
        },
        error: function () {
            alert("Ha ocurrido un error paginado");
        }
    });
}

function mostrarListado(nroPaginas, campo, orden) {
    var empresa = $('#cbEmpresa').multipleSelect('getSelects');
    var origen = $('#cbOrigLectura').multipleSelect('getSelects');
    var tipopto = $('#cbTipoPuntoMedicion').multipleSelect('getSelects');
    var familia = $('#cbFamilia').multipleSelect('getSelects');
    var ubicacion = $('#cbUbicacion').multipleSelect('getSelects');
    var categoria = $('#cbTipoGrupoFiltro').multipleSelect('getSelects');

    if (tipopto == "[object Object]") tipopto = "-1";
    $('#hfTipoPuntoMedicion').val(tipopto);
    //$('#hfEmpresa').val(empresa);
    $('#hfEmpresa').val($('#cbEmpresa').multipleSelect('isAllSelected') == "S" ? "-1" : empresa);
    $('#hfOrigLectura').val(origen);
    $('#hfFamilia').val(familia);
    $('#hfCategoriaFiltro').val(categoria);
    //$('#hfUbicacion').val(ubicacion);
    $('#hfUbicacion').val($('#cbUbicacion').multipleSelect('isAllSelected') == "S" ? "-1" : ubicacion);
    $.ajax({
        type: 'POST',
        url: controlador + "ptomedicion/lista",
        data: {
            empresas: $('#hfEmpresa').val(),
            tipoOrigenLectura: $('#hfOrigLectura').val(),
            tipoPtomedicodi: $('#hfTipoPuntoMedicion').val(),
            nroPagina: nroPaginas,
            idsFamilia: $('#hfFamilia').val(),
            ubicacion: $('#hfUbicacion').val(),
            categoria: $('#hfCategoriaFiltro').val(),
            tipoPunto: $('#cbTipoPuntoFiltro').val(),
            codigo: $('#txtCodigoPunto').val(),
            cliente: $('#cbTipoCliente').val(),
            barra: $('#cbTipoBarra').val(),
            campo: campo,
            orden: orden
        },
        success: function (evt) {
            $('#listado').css("width", $('#mainLayout').width() + "px");

            $('#listado').html(evt);
            table = $('#tabla').dataTable({
                "scrollY": 500,
                "scrollX": true,
                "sDom": 't',
                "ordering": false,
                "iDisplayLength": 40
            });

        },
        error: function () {
            alert("Ha ocurrido un error");
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
    mostrarListado(1, elemento, order);


}

function exportarExcel() {

    if ($('#cbEmpresa').multipleSelect('rowCountSelected') <= 100 || $('#cbEmpresa').multipleSelect('isAllSelected') == "S") {

        var empresa = $('#cbEmpresa').multipleSelect('getSelects');
        var origen = $('#cbOrigLectura').multipleSelect('getSelects');
        var tipopto = $('#cbTipoPuntoMedicion').multipleSelect('getSelects');
        var familia = $('#cbFamilia').multipleSelect('getSelects');
        var ubicacion = $('#cbUbicacion').multipleSelect('getSelects');
        var categoria = $('#cbTipoGrupoFiltro').multipleSelect('getSelects');

        if (tipopto == "[object Object]") tipopto = "-1";
        $('#hfTipoPuntoMedicion').val(tipopto);
        //$('#hfEmpresa').val(empresa);
        $('#hfEmpresa').val($('#cbEmpresa').multipleSelect('isAllSelected') == "S" ? "-1" : empresa);
        $('#hfOrigLectura').val(origen);
        $('#hfFamilia').val(familia);
        $('#hfCategoriaFiltro').val(categoria);
        //$('#hfUbicacion').val(ubicacion);
        $('#hfUbicacion').val($('#cbUbicacion').multipleSelect('isAllSelected') == "S" ? "-1" : ubicacion);

        $.ajax({
            type: 'POST',
            url: controlador + "ptomedicion/exportar",
            data: {
                empresas: $('#hfEmpresa').val(),
                tipoOrigenLectura: $('#hfOrigLectura').val(),
                tipoPtomedicodi: $('#hfTipoPuntoMedicion').val(),
                idsFamilia: $('#hfFamilia').val(),
                ubicacion: $('#hfUbicacion').val(),
                categoria: $('#hfCategoriaFiltro').val(),
                tipoPunto: $('#cbTipoPuntoFiltro').val(),
                codigo: $('#txtCodigoPunto').val(),
                cliente: $('#cbTipoCliente').val(),
                barra: $('#cbTipoBarra').val()
            },
            dataType: 'json',
            success: function (result) {
                if (result == 1) {
                    window.location = controlador + "ptomedicion/descargar";
                }
                if (result == -1) {
                    alert("Error en reporte result")
                }
            },
            error: function () {
                alert("Ha ocurrido un error");
            }
        });
    }
    else {
        alert("No puede seleccionar más de 100 empresas.");
    }
}

function cargarTipoPto() {
    var origen = $('#cbOrigLectura').multipleSelect('getSelects');
    $('#hfOrigLectura').val(origen);
    $.ajax({
        type: 'POST',
        url: controlador + 'ptomedicion/TipoPtoMedicion',
        data: { tipoOrigenLectura: $('#hfOrigLectura').val() },
        success: function (aData) {
            $('#tipoptomed').html(aData);
        },
        error: function () {
            alert("Ha ocurrido un error");
        }
    });
}

function ocultarColumnas() {

    var oTable = $('#tabla').dataTable();

    var bVis9 = oTable.fnSettings().aoColumns[9].bVisible;
    oTable.fnSetColumnVis(9, bVis9 ? false : true);

    var bVis10 = oTable.fnSettings().aoColumns[10].bVisible;
    oTable.fnSetColumnVis(10, bVis10 ? false : true);

    var bVis11 = oTable.fnSettings().aoColumns[11].bVisible;
    oTable.fnSetColumnVis(11, bVis11 ? false : true);
}

eliminar = function (id) {
    if (confirm('¿Está seguro de realizar esta acción?')) {

        $.ajax({
            type: 'POST',
            url: controlador + 'ptomedicion/deletepunto',
            data: {
                ptoMedicion: id
            },
            dataType: 'json',
            success: function (result) {
                if (result == 1) {
                    buscarPtoMedicion();
                    mostrarMensaje('mensajePunto', 'exito', 'La operación se realizó correctamente.');
                }
                else if (result == 2) {
                    mostrarMensaje('mensajePunto', 'alert', 'No se puede eliminar el punto ya que existen registros asociados.');
                }
                else {
                    mostrarMensaje('mensajePunto', 'error', 'Ha ocurrido un error.');
                }
            },
            error: function () {
                mostrarMensaje('mensajePunto', 'error', 'Ha ocurrido un error.');
            }
        });
    }
}

function mostrarDetalle(id) {
    //location.href = controlador + "ptomedicion/editar?id=" + id;
    nuevoPtoMedicion(id)
}

function cancelar() {
    document.location.href = controlador + "index";
}

mostrarMensaje = function (id, tipo, mensaje) {
    $('#' + id).removeClass();
    $('#' + id).addClass('action-' + tipo);
    $('#' + id).html(mensaje);
}

mostrarError = function () {
    $('#mensaje').removeClass("action-exito");
    $('#mensaje').addClass("action-error");
    $('#mensaje').text("Ha ocurrido un error");
    $('#mensaje').css("display", "block");
};

validarNumero = function (item, evt) {
    evt = (evt) ? evt : window.event;
    var charCode = (evt.which) ? evt.which : evt.keyCode;

    if (charCode == 46) {
        var regex = new RegExp(/\./g)
        var count = $(item).val().match(regex).length;
        if (count > 1) {
            return false;
        }
    }

    if (charCode > 31 && (charCode < 48 || charCode > 57)) {
        return false;
    }

    return true;
}