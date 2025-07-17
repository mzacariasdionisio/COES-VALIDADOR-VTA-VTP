var hojaPrincipal = '';

$(function () {
    $('#cbTipoFormato').unbind('change');
    $('#cbTipoFormato').change(function () {
        CANTIDAD_CLICK_TIPO_FORMATO++;
        cargarFormato(hojaPrincipal);
    });
    cargarFormato(hojaPrincipal);
});

function inicializarFormatoView(numHojaPrincipal, numHoja, formato, id, model, listaHojaModel) {
    $.ajax({
        type: 'POST',
        data: {
            idHojaPadre: numHoja,
            idFormato: formato
        },
        async: false,
        url: controlador + "ViewHojaPadreCargaDatos",
        success: function (evt) {
            $('#' + id).html(evt);

            cargarListaEmpresa(numHoja, model.ListaEmpresas);

            setEstaCargado(true, numHoja);
            setNombreHoja(getHojaNombre(numHoja), numHoja);

            horizonte(numHojaPrincipal, model.Periodo, model);

            inicializarFormatoViewTab(listaHojaModel, numHoja);

           /* for (var nh = 0; nh < listaHojaModel.length; nh++) {
                var numHojaHijo = listaHojaModel[nh].Hojacodi;
                LISTA_OBJETO_HOJA[numHojaHijo + ''] = crearObjetoHoja();
            }*/
        },
        error: function (err) {
            mostrarErrorPrincipal('Ocurrió un error.');
        }
    });
}

function horizonte(numHoja, periodo, Model) {
    //setIdEnvio(numHoja, Model.IdEnvio);

    /*
    $("#hfIdEnvioMain").val(Model.IdEnvio);
    $("#hfFormatoMain").val(Model.IdFormato);
    $("#hfEmpresaMain").val(Model.IdEmpresa);
    $("#hfFechaMain").val(Model.Fecha);
    $("#hfSemanaMain").val(Model.NroSemana);
    $("#hfMesMain").val(Model.Mes);

    $("#txtFecha").val(Model.Dia);
    $("#Anho").val(Model.Anho);
    $("#txtMes").val(Model.Mes);
    */
    switch (parseInt(periodo)) {
        case 1: //dia
            $('.cntFecha').css("display", "table-cell");
            $('.cntSemana').css("display", "none");
            $('.cntMes').css("display", "none");
            break;
        case 2: //semanal
            $('.cntFecha').css("display", "none");
            $('.cntSemana').css("display", "table-cell");
            $('.cntMes').css("display", "none");

            cargarSemanaAnho(numHoja, periodo);

            break;
            //mensual

            //break;
        case 3: case 5:
            $('.cntFecha').css("display", "none");
            $('.cntSemana').css("display", "none");
            $('.cntMes').css("display", "inline");
            $('.cntMes.divmes').css("display", "block");
            break;
    }
}

function cargarListaEmpresa(numHoja, lista) {
    $(getIdElemento(numHoja, '#cbEmpresa')).empty();
    for (var i = 0; i < lista.length; i++) {
        $(getIdElemento(numHoja, '#cbEmpresa')).append('<option value=' + lista[i].Emprcodi + '>' + lista[i].Emprnomb + '</option>');
    }
}

function inicializarFormatoViewTab(listaHoja, hojaPrincipal) {
    var listaHojaView = [];

    if (listaHoja != null) {
        var prefijo = "view";
        cargarListaTabsFromHojas(listaHoja, hojaPrincipal, 'tab-container', prefijo);

        for (var i = 0; i < listaHoja.length; i++) {
            var hoja = listaHoja[i];
            var keyHoja = hoja.Hojacodi.toString();
            LISTA_OBJETO_HOJA[keyHoja] = crearObjetoHoja();

            getErrores(keyHoja)[ERROR_NO_NUMERO].validar = true;
            getErrores(keyHoja)[ERROR_LIM_INFERIOR].validar = true;
            getErrores(keyHoja)[ERROR_LIM_SUPERIOR].validar = true;

            listaHojaView.push(crearHojaView(keyHoja, 'view' + keyHoja, getHoja(keyHoja)));
        }
    }

    //configuración de la hoja
    setListaHoja(listaHojaView, hojaPrincipal);
    setTieneFiltro(true, hojaPrincipal);
    setValidacionDataCongelada(false, hojaPrincipal);

    var config = crearConfigHoja();
    config.tieneFiltroArea = true;
    config.tieneFiltroSubestacion = true;
    config.tieneFiltroFormato = true;
    config.tienePanelIEOD = false;
    config.verUltimoEnvio = false;

    setConfigHoja(config, hojaPrincipal);

    //crear vistas
    inicializarHojaView(hojaPrincipal);
}

//ASSETEC 20180718////////////////////////////////////////
//Verificar la información de los Puntos de Medición vs Patron
verificarPronostico = function (IdEnvio, IdHojaPadre, anio, semana, mes, IdEmpresa) {
    console.log("verificarPronostico -> " + IdEnvio);
    $.ajax({
        type: 'POST',
        url: siteRoot + 'IEOD/pronostico/VerificarPronostico',
        data: { IdEnvio: IdEnvio, IdHojaPadre: IdHojaPadre, anio: anio, semana: semana, mes: mes, IdEmpresa: IdEmpresa },
        dataType: 'json',
        success: function (numPuntosDepurar) {
            if (isNaN(numPuntosDepurar)) {
                //no es un numero, mostramos mensaje
                console.log("ERROR -> " + numPuntosDepurar);
            }
            else {
                var iNum = Number(numPuntosDepurar);
                if (iNum > 0) {
                    //abrimos un popup para mostrar los puntos a depurar
                    console.log("numPuntosDepurar -> " + iNum);
                    abrirPronostico(IdEmpresa, IdEnvio, IdHojaPadre, iNum);
                }
                else {
                    console.log("Nada por depurar: numPuntosDepurar -> " + iNum);
                }
            }
        },
        error: function () {
            alert('Ha ocurrido un error al verificar la información reportada.');
        }
    });
}

abrirPronostico = function (IdEmpresa, IdEnvio, IdHojaPadre, NumPtosDepurar) {
    console.log("abrirPronostico -> IdEmpresa:" + IdEmpresa + " / IdEnvio:" + IdEnvio + " / " + " / IdHojaPadre:" + IdHojaPadre + " / NumPtosDepurar:" + NumPtosDepurar);
    $.ajax({
        type: 'GET',
        url: siteRoot + 'IEOD/pronostico/index',
        data: { IdEmpresa: IdEmpresa, IdEnvio: IdEnvio, IdHojaPadre: IdHojaPadre, NumPtosDepurar: NumPtosDepurar },
        success: function (evt) {
            $('#pronostico').html(evt);
            //$('#loading').bPopup().close();
            setTimeout(function () {
                $('#pronostico').bPopup({
                    easing: 'easeOutBack',
                    speed: 450,
                    transition: 'slideDown',
                    content: 'ajax'
                });
            }, 2000);
        },
        error: function () {
            alert("Lo sentimos, ha ocurrido un error al mostrar el popup");
        }
    });
}