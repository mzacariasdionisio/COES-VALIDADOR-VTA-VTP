var TAMANIO_RESTAURAR = $("#divGeneral").width();
var ocultar;

$(function () {
    $('#btnExpandirRestaurar').click(function () {
        expandirRestaurar();
    });
});

function expandirRestaurar() {
    if ($('#hfExpandirContraer').val() == "E") {
        expandirExcel();
        updateDimension(1);
        ocultar = 1;
        //calculateSize2(1);
        $('#hfExpandirContraer').val("C");
        $('#spanExpandirContraer').text('Restaurar');

        var img = $('#imgExpandirContraer').attr('src');
        var newImg = img.replace('expandir.png', 'contraer.png');
        $('#imgExpandirContraer').attr('src', newImg);

    }
    else {
        restaurarExcel();
        updateDimension(2);
        ocultar = 2;
        //calculateSize2(2);
        $('#hfExpandirContraer').val("E");
        $('#spanExpandirContraer').text('Expandir');

        var img = $('#imgExpandirContraer').attr('src');
        var newImg = img.replace('contraer.png', 'expandir.png');
        $('#imgExpandirContraer').attr('src', newImg);

    }
    updateDimensionHandson();
}

function expandirExcel() {
    $('#mainLayout').addClass("divexcel");
    if (typeof (hot) != "undefined" && hot !== undefined && hot != null) {
        hot.render();
    }
}

function restaurarExcel() {

    $('#tophead').css("display", "none");
    $('#detExcel').css("display", "block");
    $('#mainLayout').removeClass("divexcel");
    $('#itemExpandir').css("display", "block");
    $('#itemRestaurar').css("display", "none");
}

function renderizarTabla(idTabla) {
    if ($('#hfExpandirContraer').val() == "E") {
        $(idTabla).css("width", TAMANIO_RESTAURAR + "px");
    }
    else {
    }
}

function updateDimensionHandson() {
    if (typeof (hot) != "undefined" && hot !== undefined && hot != null) {
        var offset = Handsontable.Dom.offset(document.getElementById('grillaExcel'));
        var widthHT;
        var heightHT;

        if (offset.top == 222) {
            heightHT = $(window).height() - offset.top - 90;
        }
        else {
            heightHT = $(window).height() - offset.top - 20;
        }

        widthHT = $(window).width() - 2 * offset.left;

        hot.updateSettings({
            width: widthHT
        });
    }
}

function updateDimension(val) {

    $('#listado').css("width", $('#mainLayout').width() + "px");
    $("#listado").hide();
    var nuevoTamanioTabla = val == 1 ? obtenerMedida() : getHeightTablaListado();
    $(".dataTables_scrollBody").css('height', nuevoTamanioTabla + 'px');
    $("#listado").show();
}

function obtenerMedida() {
    return $(window).height()
        - $("#Contenido").parent().height() //Filtros
        - 14 //<br>
        - $(".dataTables_filter").height()
        - $(".dataTables_scrollHead").height()
        - $("#contentHeader").height()
        - $("#divGeneral .content-titulo ").height()
        - $("#contentMenu").height()
        ;
}