var controlador = siteRoot + 'hidrologia/'

$(function () {

    $('#trCentralGrupo').hide();
    $('#cbEmpresaGrupo').val($('#hfEmpresaGrupo').val());
    $('#cbCategoria').val($('#hfCategoriaGrupo').val());
    $('#cbFEnergía').val($('#hfFuenteEnegía').val());
    $('#cbTipoGrupo').val($('#hfTipoGrupo').val());
    $('#cbGrupoEstado').val($('#hfGrupoEstado').val());

    if ($('#hfTipoGenerRer').val() == "S") {
        document.getElementById('chkTipoGenerRer').checked = true;
    } else document.getElementById('chkTipoGenerRer').checked = false;


    if ($('#cbCategoria').val() == 3 || $('#cbCategoria').val() == 5 || $('#cbCategoria').val() == 15 || $('#cbCategoria').val() == 17) {
        cargarGrupos();
    } else {
        $('#trCentralGrupo').hide();
    }


    $('#btnCancelarPtoDespacho').click(function () {
        cancelarPtoDespacho();
    });

    $('#btnGrabarPtoDespacho').click(function () {
        grabarPtoDespacho();
    });

    $('#cbCategoria').on('change', function () {
        if ($('#cbCategoria').val() == 3 || $('#cbCategoria').val() == 5 || $('#cbCategoria').val() == 15 || $('#cbCategoria').val() == 17) {
            cargarGrupos();
        } else {
            $('#trCentralGrupo').hide();
        }
    });

    $('#cbEmpresaGrupo').on('change', function () {
        cargarGrupos();
    });

});

function cancelarPtoDespacho() {
    $('#popupUnidad').bPopup().close();
}

function grabarPtoDespacho() {

    var codigogrupo = $('#hfcodigrupo').val();
    var nombre = $('#txtNombreGrupo').val();
    var abreviatura = $('#txtAbreviaturaGrupo').val();
    var osicodi = $('#txtosicodi').val();
    var empresa = $('#cbEmpresaGrupo').val();
    var tipo = $('#cbTipoGrupo').val();
    var grupoPadre = $('#cbCentralGrupo').val();
    var categoria = $('#cbCategoria').val();
    var grupotipo = "";
    if (categoria == 4 || categoria == 3) {
        grupotipo = "T";
    }
    if (categoria == 5 || categoria == 6) {
        grupotipo = "H";
    }
    if (categoria == 15 || categoria == 16) {
        grupotipo = "S";
    }
    if (categoria == 17 || categoria == 18) {
        grupotipo = "E";
    }
    var grupoactivo = 'S';
    var tipogrupo = $('#cbTipoGrupo').val();
    var tipofuente = $('#cbFEnergía').val();
    var tipogenerrer = (document.getElementById('chkTipoGenerRer').checked ? 'S' : 'N');
    var grupointegrante = (tipogrupo == 10 ? 'N' : 'S');
    var grupoestado = $('#cbGrupoEstado').val();


    $.ajax({
        type: 'POST',
        dataType: 'json',
        url: controlador + 'ptodespacho/grabarPtoDespacho',
        global: false,
        data: {
            empresa: empresa,
            codigrupo: codigogrupo,
            descripcion: nombre,
            abrev: abreviatura,
            grupotipo: grupotipo,
            categoria: categoria,
            grupopadre: (grupoPadre > 0 ? grupoPadre : -1),
            grupoactivo: grupoactivo,
            osicodi: osicodi,
            tipogrupo: tipogrupo,
            tipofuente: tipofuente,
            tipogenerrer: tipogenerrer,
            grupointegrante: grupointegrante,
            estado: grupoestado
        },
        success: function (evt) {
            if (evt == 1) {
                alert("Se grabo correctamente");
                $('#popupUnidad').bPopup().close();
                buscarDespacho();
            } else {
                alert("Error al registrar punto de despacho.");
            }

        },
        error: function () {
            alert("Ha ocurrido un error en el grabado");
        }
    });
}

function cargarGrupos() {

    var empr = $('#cbEmpresaGrupo').val();
    var cat = $('#cbCategoria').val();
    if (empr != "-1" && cat != "-1") {
        $('#trCentralGrupo').show();
        $('option', '#cbCentralGrupo').remove();
        $.ajax({
            type: 'POST',
            url: controlador + 'ptodespacho/cargasCentralByEmpCat',
            dataType: 'json',
            global: false,
            data: {
                empresa: empr,
                categoria: cat
            },
            cache: false,
            success: function (aData) {
                $('#cbCentralGrupo').get(0).options.length = 0;
                $('#cbCentralGrupo').get(0).options[0] = new Option("--SELECCIONE--", "");
                $.each(aData, function (i, item) {
                    $('#cbCentralGrupo').get(0).options[$('#cbCentralGrupo').get(0).options.length] = new Option(item.Text, item.Value);
                });

                $('#cbCentralGrupo').val($('#hfCentralGrupo').val());
                //$('#cbCentralGrupo').val($('#hfBarra').val());
            },
            error: function () {
                alert("Ha ocurrido un error.");
            }
        });
    } else {
        $('option', '#cbCentralGrupo').remove();
        $('#trCentralGrupo').hide();
    }
}