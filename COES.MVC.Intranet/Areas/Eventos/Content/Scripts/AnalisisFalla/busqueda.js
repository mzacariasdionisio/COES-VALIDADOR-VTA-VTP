var controlador = siteRoot + 'Eventos/AnalisisFallas/'

function mostrarAreas() {
    var idEmpresa = "0";
    if ($('#cbEmpresa').val() != "" && $('#cbEmpresa').val() != "0") {

        if (typeof famcodi == 'undefined' || famcodi == null || famcodi == undefined || famcodi == "0") {
            idFamilia = $('#hfIdFamilia').val();
            if (typeof famcodi == 'undefined') idFamilia = 0;
        } else {
            idFamilia = famcodi;
        }


        idEmpresa = $('#cbEmpresa').val();
        $.ajax({
            type: "POST",
            url: controlador + "AreaEquipo",
            data: { idEmpresa: idEmpresa, idFamilia: idFamilia },
            global: false,
            success: function (evt) {
                $('#cntArea').html(evt);
            },
            error: function (req, status, error) {
                alert("Ha ocurrido un error.");
            }
        });
    }
    else if ($('#cbEmpresa').val() == "") {
        $('#cntArea').html("");
        $('#hfIdArea').val("");
        $('#cntEquipo').html("");
    }
}

function mostrarEquipos(idArea) {
    var idEmpresa = "0";
    var idFamilia = "0";
    $('#hfIdArea').val(idArea);

    if ($('#cbEmpresa').val() != "" && $('#cbEmpresa').val() != "0") {
        idEmpresa = $('#cbEmpresa').val();
        $('#hfIdEmpresa').val(idEmpresa);
        $('#hfIdEmpresaRec').val(idEmpresa);
        $('#hfIdEmpresaObservacionobs').val(idEmpresa);
        if ($('#cbFamilia').val() != "") {
            idFamilia = $('#cbFamilia').val();
        }
        $('#hfIdFamilia').val(idFamilia);

        pintarPaginado(idEmpresa, idFamilia, idArea);
        pintarBusqueda(1);
    }
    else if ($('#cbEmpresa').val() == "") {
        $('#cntEquipo').html("");
        $('#hfIdEmpresa').val("");
        $('#hfIdArea').val("");
    }
}

function buscarEquipo(tipo) {
    if (tipo == 1) {
        var idEmpresa = $('#hfIdEmpresa').val();
        var idFamilia = $('#hfIdFamilia').val();
        var idArea = $('#hfIdArea').val();

        if (typeof famcodi == 'undefined' || famcodi == null || famcodi == undefined || famcodi == "0") {
            idFamilia = $('#hfIdFamilia').val();
        } else {
            idFamilia = famcodi;
        }



        if ($('#txtFiltro').val() == '') {
            alert("Ingrese el filtro");
        }
        else {
            if (idEmpresa == "") { idEmpresa = "0"; $('#hfIdEmpresa').val("0"); }
            if (idFamilia == "") { idFamilia = "0"; $('#hfIdFamilia').val("0"); }
            if (idArea == "") { idArea = "0"; $('#hfIdArea').val("0"); }

            pintarPaginado(idEmpresa, idFamilia, idArea);
            pintarBusqueda(1);
        }
    }
    


}


function pintarPaginado(idEmpresa, idFamilia, idArea) {
    if (typeof famcodi == 'undefined' || famcodi == null || famcodi == undefined || famcodi == "0") {
        idFamilia = $('#hfIdFamilia').val();
    } else {
        var fcodi = $('#hfIdFamilia').val();

        if (typeof fcodi == 'undefined')
            idFamilia = famcodi;
        else
            idFamilia = fcodi;
    }

    if (idFamilia + "*" == "*")
        idFamilia = "0";

    $.ajax({
        type: "POST",
        url: controlador + "PaginadoEquipo",
        data: { idEmpresa: idEmpresa, idFamilia: idFamilia, idArea: idArea, filtro: $('#txtFiltro').val() },
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
    var idArea = "0";
    if ($('#hfIdArea').val() != "") {
        idArea = $('#hfIdArea').val();
    }

    if (typeof famcodi == 'undefined' || famcodi == null || famcodi == undefined || famcodi == "0") {
        idFamilia = $('#hfIdFamilia').val();
    } else {
        var fcodi = $('#hfIdFamilia').val();

        if (typeof fcodi == 'undefined')
            idFamilia = famcodi;
        else
            idFamilia = fcodi;
    }

    if (idFamilia + "*" == "*")
        idFamilia = "0";

    $.ajax({
        type: "POST",
        url: controlador + "ResultadoEquipo",
        global: false,
        data: { idEmpresa: $('#hfIdEmpresa').val(), idFamilia: idFamilia, idArea: idArea, filtro: $('#txtFiltro').val(), nroPagina: nroPagina },
        success: function (evt) {
            $('#cntEquipo').html(evt);
        },
        error: function (req, status, error) {
            alert("Ha ocurrido un error.");
        }
    });
}