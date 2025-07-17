var controlador = siteRoot + "Intervenciones/Registro/";

function mostrarAreas() {
    var idEmpresa = "0";
    if ($('#cbEmpresaLinea').val() != "" && $('#cbEmpresaLinea').val() != "0") {

        if (typeof famcodi == 'undefined' || famcodi == null || famcodi == undefined || famcodi == "0") {
            idFamilia = $('#hfIdFamiliaEquipo').val();
            if (typeof famcodi == 'undefined') idFamilia = 0;
        } else {
            idFamilia = famcodi;
        }

        idEmpresa = $('#cbEmpresaLinea').val();
        $.ajax({
            type: "POST",
            url: controlador + "BusquedaEquipoArea",
            data: { idEmpresa: idEmpresa, idFamilia: idFamilia, filtroFamilia: $("#hfFiltroFamilia").val() },
            global: false,
            success: function (evt) {
                $('#cntArea').html(evt);
                oTable = $('#tabla').dataTable({
                    "sPaginationType": "full_numbers",
                    "destroy": "true",
                    "bInfo": false,
                    "bLengthChange": false
                });
            },
            error: function (req, status, error) {
                alert("Ha ocurrido un error.");
            }
        });
    }
    else if ($('#cbEmpresaLinea').val() == "") {
        $('#cntArea').html("");
        $('#hfIdAreaEquipo').val("");
        $('#cntEquipo').html("");
    }
}

function mostrarEquipos(idArea) {
    var idEmpresa = "0";
    $('#hfIdAreaEquipo').val(idArea);

    if ($('#cbEmpresaLinea').val() != "" && $('#cbEmpresaLinea').val() != "0") {
        idEmpresa = $('#cbEmpresaLinea').val();
        $('#hfIdEmpresaLinea').val(idEmpresa);

        idFamilia = $('#cbFamiliaEquipo').val();
        $('#hfIdFamiliaEquipo').val(idFamilia);

        pintarPaginado(idEmpresa, idFamilia, idArea);
        pintarBusqueda(1);
    }
    else if ($('#cbEmpresaLinea').val() == "") {
        $('#cntEquipo').html("");
        $('#hfIdAreaEquipo').val("");
        $('#hfIdEmpresaLinea').val("");
    }
}

function buscarEquipo() {
    var idEmpresa = $('#hfIdEmpresaLinea').val();
    var idFamilia = $('#hfIdFamiliaEquipo').val();
    var idArea = $('#hfIdAreaEquipo').val();

    if (typeof famcodi == 'undefined' || famcodi == null || famcodi == undefined || famcodi == "0") {
        idFamilia = $('#hfIdFamiliaEquipo').val();
    } else {
        idFamilia = famcodi;
    }

    if ($('#txtFiltro').val() == '') {
        alert("Ingrese el filtro");
    }
    else {
        if (idEmpresa == "") { idEmpresa = "0"; $('#hfIdEmpresaLinea').val("0"); }
        if (idFamilia == "") { idFamilia = "0"; $('#hfIdFamiliaEquipo').val("0"); }
        if (idArea == "") { idArea = "0"; $('#hfIdAreaEquipo').val("0"); }

        pintarPaginado(idEmpresa, idFamilia, idArea);
        pintarBusqueda(1);
    }
}

function pintarPaginado(idEmpresa, idFamilia, idArea) {

    $.ajax({
        type: "POST",
        url: controlador + "BusquedaEquipoPaginado",
        data: { idEmpresa: idEmpresa, idFamilia: idFamilia, idArea: idArea, filtro: $('#txtFiltro').val(), filtroFamilia: $("#hfFiltroFamilia").val() },
        global: false,
        success: function (evt) {
            $('#cntPaginado').html(evt);
            mostrarPaginado();
        },
        error: function (req, status, error) {
            alert("Ha ocurrido un error.");
        }
    });
}

function pintarBusqueda(nroPagina) {

    $.ajax({
        type: "POST",
        url: controlador + "BusquedaEquipoResultado",
        global: false,
        data: {
            idEmpresa: $('#hfIdEmpresaLinea').val(), idFamilia: $('#hfIdFamiliaEquipo').val(),
            idArea: $('#hfIdAreaEquipo').val(), filtro: $('#txtFiltro').val(), nroPagina: nroPagina
            , filtroFamilia: $("#hfFiltroFamilia").val()
        },
        success: function (evt) {
            $('#cntEquipo').html(evt);
        },
        error: function (req, status, error) {
            alert("Ha ocurrido un error.");
        }
    });
}