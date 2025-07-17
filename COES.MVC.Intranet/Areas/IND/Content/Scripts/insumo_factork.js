var controlador = siteRoot + 'IND/FactorK/';
var LISTA_UNIDAD = [];
var ancho = 1000;
var ALTURA_HANDSON = 200;
var LISTA_HoT = [null];
var error = [];
var headerht = [];

$(function () {
    $('#cntMenu').css("display", "none");

    $('#tab-container').easytabs({
        animate: false
    });
    $('#tab-container').bind('easytabs:after', function () {
        //
    });
    $('#tab-container').easytabs('select', '#vistaConsulta');

    $('#cbAnio').change(function () {
        listarPeriodos("#cbAnio", "#cbPeriodo", "#vistaConsulta");
    });
    $('#cbPeriodo').change(function () {
        //
    });
    $('#cbTipoEmpresa').multipleSelect({
        width: '125px',
        filter: true,
        single: false,
        onClose: function () {
            listarEmpresas('#cbTipoEmpresa');
        }
    });
    $('#cbTipoEmpresa').multipleSelect('checkAll');
    $('#cbEmpresa').change(function () {
        listarCentrales('#cbEmpresa');
    });
    $('#cbCentral').change(function () {
        listarUnidadNombres('#cbEmpresa', '#cbCentral');
    });
    $('#btnBuscar').click(function () {
        mostrarListado();
    });

    $('#cbAnio2').change(function () {
        listarPeriodos("#cbAnio2", "#cbPeriodo2", "#vistaImportacion");
    });
    $('#cbPeriodo2').change(function () {
        $("#dtFechaInicio2").val('');
        $("#dtFechaFin2").val('');
    });
    $('#dtFechaInicio2').Zebra_DatePicker();
    $('#dtFechaFin2').Zebra_DatePicker();
    $('#cbTipoEmpresa2').multipleSelect({
        width: '125px',
        filter: true,
        single: false,
        onClose: function () {
            listarEmpresas('#cbTipoEmpresa2');
        }
    });
    $('#cbTipoEmpresa2').multipleSelect('checkAll');
    $('#cbEmpresa2').change(function () {
        listarCentrales('#cbEmpresa2');
    });
    $('#cbCentral2').change(function () {
        listarUnidadNombres('#cbEmpresa2','#cbCentral2');
    });
    $('#btnImportar').click(function () {
        importarInsumos();
    });
    $('#btnGuardar').click(function () {
        guardarInsumos();
    });

    verificarPV();
});

function listarPeriodos(idCbAnio, idCbPeriodo, idVista)
{
    var anio = parseInt($(idCbAnio).val()) || 0;
    $(idCbPeriodo).empty();

    $.ajax({
        type: 'POST',
        url: controlador + "PeriodoListado",
        data: {
            anio: anio,
        },
        success: function (evt) {
            if (evt.Resultado != "-1") {
                if (evt.ListaPeriodo.length > 0) {
                    $.each(evt.ListaPeriodo, function (i, item) {
                        $(idCbPeriodo).get(0).options[$(idCbPeriodo).get(0).options.length] = new Option(item.Iperinombre, item.Ipericodi);
                    });
                } else {
                    $(idCbPeriodo).get(0).options[0] = new Option("--", "0");
                }
            } else {
                alert("Ha ocurrido un error: " + evt.Mensaje);
            }
        },
        error: function (err) {
            alert('Ha ocurrido un error.');
        }
    });
}

function getTipoEmpresa(idCbTipoEmpresa)
{
    var tipoempresa = $(idCbTipoEmpresa).multipleSelect('getSelects');
    if (tipoempresa == "[object Object]" || tipoempresa.length == 0) tipoempresa = "0";//-1

    if (idCbTipoEmpresa == "#cbTipoEmpresa") {
        $('#hfTipoEmpresa').val(tipoempresa);
        var idTipoEmpresa = $('#hfTipoEmpresa').val();
    }
    else if (idCbTipoEmpresa == "#cbTipoEmpresa2") {
        $('#hfTipoEmpresa2').val(tipoempresa);
        var idTipoEmpresa = $('#hfTipoEmpresa2').val();
    }

    return idTipoEmpresa;
}

function listarEmpresas(idCbTipoEmpresa)
{
    if (idCbTipoEmpresa == '#cbTipoEmpresa')
    {
        $('#tab-container').easytabs('select', '#vistaConsulta');
        $('#cbEmpresa').empty();
        $('#cbCentral').empty();
        $('#cbUnidadNombre').empty();
    }
    else if (idCbTipoEmpresa == '#cbTipoEmpresa2')
    {
        $('#tab-container').easytabs('select', '#vistaImportacion');
        $('#cbEmpresa2').empty();
        $('#cbCentral2').empty();
        $('#cbUnidadNombre2').empty();
    }
    var tipoemprcodi = getTipoEmpresa(idCbTipoEmpresa);

    $.ajax({
        type: 'POST',
        url: controlador + "ListarEmpresasDeIdentificacionEmpresas",
        data: {
            tipoemprcodi: tipoemprcodi,
        },
        success: function (evt) {
            if (evt.Resultado == "1") {
                if (evt.ListaEmpresa.length > 0) {
                    if (idCbTipoEmpresa == '#cbTipoEmpresa') {
                        $('#cbEmpresa').get(0).options[0] = new Option("TODOS", "0");
                        $.each(evt.ListaEmpresa, function (i, item) {
                            $('#cbEmpresa').get(0).options[$('#cbEmpresa').get(0).options.length] = new Option(item.Emprnomb, item.Emprcodi);
                        });
                        $('#cbCentral').get(0).options[0] = new Option("TODOS", "0");
                        $('#cbUnidadNombre').get(0).options[0] = new Option("TODOS", "0");
                    }
                    else if (idCbTipoEmpresa == '#cbTipoEmpresa2') {
                        $('#cbEmpresa2').get(0).options[0] = new Option("TODOS", "0");
                        $.each(evt.ListaEmpresa, function (i, item) {
                            $('#cbEmpresa2').get(0).options[$('#cbEmpresa2').get(0).options.length] = new Option(item.Emprnomb, item.Emprcodi);
                        });
                        $('#cbCentral2').get(0).options[0] = new Option("TODOS", "0");
                        $('#cbUnidadNombre2').get(0).options[0] = new Option("TODOS", "0");
                    }
                }
            } else {
                alert("Hubo un error. " + evt.Mensaje);
            }
        },
        error: function (err) {
            alert('Ha ocurrido un error.');
        }
    });
}

function listarCentrales(idCbEmpresa)
{
    if (idCbEmpresa == '#cbEmpresa') {
        $('#tab-container').easytabs('select', '#vistaConsulta');
        $('#cbCentral').empty();
        $('#cbUnidadNombre').empty();
    }
    else if (idCbEmpresa == '#cbEmpresa2') {
        $('#tab-container').easytabs('select', '#vistaImportacion');
        $('#cbCentral2').empty();
        $('#cbUnidadNombre2').empty();
    }
    var emprcodi = parseInt($(idCbEmpresa).val()) || 0;

    if (emprcodi == 0)
    {
        if (idCbEmpresa == '#cbEmpresa') {
            $('#cbCentral').get(0).options[0] = new Option("TODOS", "0");
            $('#cbUnidadNombre').get(0).options[0] = new Option("TODOS", "0");
        }
        else if (idCbEmpresa == '#cbEmpresa2') {
            $('#cbCentral2').get(0).options[0] = new Option("TODOS", "0");
            $('#cbUnidadNombre2').get(0).options[0] = new Option("TODOS", "0");
        }
        return;
    }

    $.ajax({
        type: 'POST',
        url: controlador + "ListarCentralesDeIdentificacionEmpresas",
        data: {
            emprcodi: emprcodi,
        },
        success: function (evt) {
            if (evt.Resultado == "1") {
                if (evt.ListaCentral.length > 0) {
                    if (idCbEmpresa == '#cbEmpresa') {
                        $('#cbCentral').get(0).options[0] = new Option("TODOS", "0");
                        $.each(evt.ListaCentral, function (i, item) {
                            $('#cbCentral').get(0).options[$('#cbCentral').get(0).options.length] = new Option(item.Equinomb, item.Equicodi);
                        });
                        $('#cbUnidadNombre').get(0).options[0] = new Option("TODOS", "0");
                    }
                    else if (idCbEmpresa == '#cbEmpresa2') {
                        $('#cbCentral2').get(0).options[0] = new Option("TODOS", "0");
                        $.each(evt.ListaCentral, function (i, item) {
                            $('#cbCentral2').get(0).options[$('#cbCentral2').get(0).options.length] = new Option(item.Equinomb, item.Equicodi);
                        });
                        $('#cbUnidadNombre2').get(0).options[0] = new Option("TODOS", "0");
                    }
                }
            } else {
                alert("Hubo un error. " + evt.Mensaje);
            }
        },
        error: function (err) {
            alert('Ha ocurrido un error.');
        }
    });
}

function listarUnidadNombres(idCbEmpresa, idCbCentral)
{
    if (idCbEmpresa == '#cbEmpresa') {
        $('#tab-container').easytabs('select', '#vistaConsulta');
        $('#cbUnidadNombre').empty();
    }
    else if (idCbEmpresa == '#cbEmpresa2') {
        $('#tab-container').easytabs('select', '#vistaImportacion');
        $('#cbUnidadNombre2').empty();
    }
    var emprcodi = parseInt($(idCbEmpresa).val()) || 0;
    var equicodicentral = parseInt($(idCbCentral).val()) || 0;

    if (equicodicentral == 0) {
        if (idCbEmpresa == '#cbEmpresa') {
            $('#cbUnidadNombre').get(0).options[0] = new Option("TODOS", "0");
        }
        else if (idCbEmpresa == '#cbEmpresa2') {
            $('#cbUnidadNombre2').get(0).options[0] = new Option("TODOS", "0");
        }
        return;
    }

    $.ajax({
        type: 'POST',
        url: controlador + "ListarUnidadNombresDeIdentificacionEmpresas",
        data: {
            emprcodi: emprcodi,
            equicodicentral: equicodicentral
        },
        success: function (evt) {
            if (evt.Resultado == "1") {
                if (evt.ListaUnidadNombre.length > 0) {
                    if (idCbEmpresa == '#cbEmpresa') {
                        $('#cbUnidadNombre').get(0).options[0] = new Option("TODOS", "0");
                        $.each(evt.ListaUnidadNombre, function (i, item) {
                            $('#cbUnidadNombre').get(0).options[$('#cbUnidadNombre').get(0).options.length] = new Option(item.Relempunidadnomb, item.Relempcodi);
                        });
                    }
                    else if (idCbEmpresa == '#cbEmpresa2') {
                        $('#cbUnidadNombre2').get(0).options[0] = new Option("TODOS", "0");
                        $.each(evt.ListaUnidadNombre, function (i, item) {
                            $('#cbUnidadNombre2').get(0).options[$('#cbUnidadNombre2').get(0).options.length] = new Option(item.Relempunidadnomb, item.Relempcodi);
                        });
                    }
                }
            } else {
                alert("Hubo un error. " + evt.Mensaje);
            }
        },
        error: function (err) {
            alert('Ha ocurrido un error.');
        }
    });
}

function mostrarListado()
{
    ALTURA_HANDSON = parseInt($(".listado1").height());
    ancho = $("#mainLayout").width() > 1100 ? $("#mainLayout").width() - 30 : 800;

    $('#tab-container').easytabs('select', '#vistaConsulta');
    $("#mensajeListado1").html('');
    $("#cabeceraListado1").attr("style", "margin-bottom: 5px; margin-top: 0px; display: none;");
    $("#listado1").html('');

    var container1 = document.getElementById('listado1');

    var ipericodi = parseInt($("#cbPeriodo").val()) || 0;
    var relempcodi = parseInt($("#cbUnidadNombre").val()) || 0;

    $.ajax({
        type: 'POST',
        url: controlador + "ConsultarInsumos",
        data: {
            ipericodi: ipericodi,
            relempcodi: relempcodi
        },
        success: function (evt) {
            if (evt.Resultado == "1") {
                $("#mensajeListado1").html("Última importación realizada el '" + evt.InsumosFactorK.InsfckfecultimpDesc + "' por el usuario '" + evt.InsumosFactorK.Insfckusuultimp + "'.");
                $("#cabeceraListado1").attr("style", "margin-bottom: 10px; margin-top: 0px;");
                $("#txtInsfckcodi").val(evt.InsumosFactorK.Insfckcodi);
                $("#txtIperinombre").val(evt.InsumosFactorK.Iperinombre);
                $("#txtEmprnomb").val(evt.InsumosFactorK.Emprnomb);
                $("#txtEquinombcentral").val(evt.InsumosFactorK.Equinombcentral);
                $("#txtRelempunidadnomb").val(evt.RelacionEmpresa.Relempunidadnomb);
                $("#txtInsfckfrc").val(evt.InsumosFactorK.Insfckfrc);
                crearGrillaExcelInfkdt(0, container1, evt.HandsonInfkdt, ALTURA_HANDSON);

            } else if (evt.Resultado == "-2") {
                $("#mensajeListado1").html(evt.Mensaje);
                $("#listado1").html('');
            } else {
                alert(evt.Mensaje);
            }
        },
        error: function (err) {
            alert('Ha ocurrido un error.');
        }
    });
}

function crearGrillaExcelInfkdt(tab, container, handson, heightHansonTab)
{
    if (typeof LISTA_HoT[tab] != 'undefined' && LISTA_HoT[tab] !== null) {
        LISTA_HoT[tab].destroy();
    }

    var LateralIzq = function (instance, td, row, col, prop, value, cellProperties) {
        Handsontable.renderers.TextRenderer.apply(this, arguments);
        td.style.backgroundColor = '#4F81BD';
        td.style.fontSize = '11px';
        td.style.color = 'white'
    };

    var maxCols = handson.MaxCols;
    var maxRows = handson.MaxRows;
    var columns = handson.Columnas;
    var headers = handson.Headers;
    headerht = handson.Headers;
    var widths = handson.ListaColWidth;
    var excelData = handson.ListaExcelData;
    var dataDescrip = handson.ListaExcelDescripcion;
    var dataFormato = handson.ListaExcelFormatoHtml;
    var arrMergeCells = handson.ListaMerge;
    var listaCambios = handson.ListaCambios;

    LISTA_HoT[tab] = new Handsontable(container, {
        data: excelData,
        stretchH: "all",
        observeChanges: true,
        colHeaders: headers,
        colWidths: widths,
        rowHeaders: true,
        columnSorting: false,
        minSpareCols: 0,
        minSpareRows: 0,
        maxCols: maxCols,
        maxRows: maxRows,
        readOnly: true,
        columns: columns,
        height: heightHansonTab,
        mergeCells: arrMergeCells,
        fixedColumnsLeft: 0,
        cells: function (row, col, prop) {
            var cellProperties = {
            };

            if (row >= 0 && col == 0) {
                cellProperties.readOnly = true;
            }

            if (row >= 0 && col == 1) {
                cellProperties.readOnly = true;
                cellProperties.renderer = LateralIzq;
            }

            if (row >= 0) {
                switch (true) {
                    case (col <= 1):
                        break;
                    case (col > 1):
                        cellProperties.readOnly = false;
                        cellProperties.className = "htRight htMiddle";
                        cellProperties.type = 'numeric';
                        cellProperties.format = '0,0.00000';
                        cellProperties.renderer = valueRenderer;
                        break;
                }
            }
            return cellProperties;
        }
    });

}

function importarInsumos()
{
    if (!confirm("¿Está seguro que desea ejecutar la importación para el filtro de datos especificado?")) {
        return;
    }

    var ipericodi = parseInt($("#cbPeriodo2").val()) || 0;
    var fechainicio = $("#dtFechaInicio2").val() || '';
    var fechafin = $("#dtFechaFin2").val() || '';
    var emprcodi = $("#cbEmpresa2").val() || '0';
    var equicodicentral = $("#cbCentral2").val() || '0';
    var relempcodi = $("#cbUnidadNombre2").val() || '0';

    $.ajax({
        type: 'POST',
        url: controlador + "ImportarInsumos",
        data: {
            ipericodi: ipericodi,
            fechainicio: fechainicio,
            fechafin: fechafin,
            emprcodi: emprcodi,
            equicodicentral: equicodicentral,
            relempcodi: relempcodi
        },
        success: function (evt) {
            if (evt.Resultado != "-1") {
                $("#mensajeListado1").html('');
                $("#cabeceraListado1").attr("style", "margin-bottom: 5px; margin-top: 0px; display: none;");
                $("#listado1").html('');
                alert(evt.Mensaje);
            } else {
                alert("Ha ocurrido un error: " + evt.Mensaje);
            }
        },
        error: function (err) {
            alert("Ha ocurrido un error");
        }
    });
}

function guardarInsumos()
{
    if (parseInt(error.length) > 0)
    {
        abrirPopupErrores();
        return;
    } 

    var insfckcodi = parseInt($("#txtInsfckcodi").val()) || 0;
    var insfckfrc = $("#txtInsfckfrc").val();
    if (insfckfrc == null || insfckfrc.trim() == "") { alert("El FRC debe tener un valor numérico"); return; }
    if (!validarNumero(insfckfrc)) { alert("El FRC debe ser un valor numérico"); return; }
    var dataht = LISTA_HoT[0].getData();

    $.ajax({
        type: 'POST',
        url: controlador + "ActualizarInsumos",
        dataType: "json",
        contentType: 'application/json',
        traditional: true,
        data: JSON.stringify({
            insfckcodi: insfckcodi,
            insfckfrc: insfckfrc,
            dataht: dataht
        }),
        success: function (evt) {
            if (evt.Resultado != "-1") {
                alert(evt.Mensaje);
            } else {
                alert("Ha ocurrido un error: " + evt.Mensaje);
            }
        },
        error: function (err) {
            alert("Ha ocurrido un error");
        }
    });
}

function validarNumero(item, evt)
{
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

function verificarPV()
{
    const urlParams = new URLSearchParams(window.location.search);
    const pericodi = urlParams.get('pericodi');
    const ipericodi = urlParams.get('ipericodi');
    const emprcodi = urlParams.get('emprcodi');
    const equicodicentral = urlParams.get('equicodicentral');
    const equicodiunidad = urlParams.get('equicodiunidad');
    const grupocodi = urlParams.get('grupocodi');
    const famcodi = urlParams.get('famcodi');

    if (!(ipericodi != null && ipericodi > 0 &&
        emprcodi != null && emprcodi > 0 &&
        equicodicentral != null && equicodicentral > 0 &&
        equicodiunidad != null && equicodiunidad > 0 &&
        grupocodi != null && grupocodi > 0 &&
        famcodi != null && famcodi > 0))
    {
        return;
    }

    $('td div.suboptions', parent.document).removeClass('selected');
    $('td div.IndexFactorK', parent.document).addClass('selected');

    ALTURA_HANDSON = parseInt($(".listado1").height());
    ancho = $("#mainLayout").width() > 1100 ? $("#mainLayout").width() - 30 : 800;

    $('#tab-container').easytabs('select', '#vistaConsulta');
    $("#mensajeListado1").html('');
    $("#cabeceraListado1").attr("style", "margin-bottom: 5px; margin-top: 0px; display: none;");
    $("#listado1").html('');

    var container1 = document.getElementById('listado1');

    $.ajax({
        type: 'POST',
        url: controlador + "ConsultarInsumosPV",
        data: {
            ipericodi: ipericodi,
            emprcodi: emprcodi,
            equicodicentral: equicodicentral,
            equicodiunidad: equicodiunidad,
            grupocodi: grupocodi,
            famcodi: famcodi
        },
        success: function (evt) {
            if (evt.Resultado == "1") {
                $("#mensajeListado1").html("Última importación realizada el '" + evt.InsumosFactorK.InsfckfecultimpDesc + "' por el usuario '" + evt.InsumosFactorK.Insfckusuultimp + "'.");
                $("#cabeceraListado1").attr("style", "margin-bottom: 10px; margin-top: 0px;");
                $("#txtInsfckcodi").val(evt.InsumosFactorK.Insfckcodi);
                $("#txtIperinombre").val(evt.InsumosFactorK.Iperinombre);
                $("#txtEmprnomb").val(evt.InsumosFactorK.Emprnomb);
                $("#txtEquinombcentral").val(evt.InsumosFactorK.Equinombcentral);
                $("#txtRelempunidadnomb").val(evt.RelacionEmpresa.Relempunidadnomb);
                $("#txtInsfckfrc").val(evt.InsumosFactorK.Insfckfrc);
                crearGrillaExcelInfkdt(0, container1, evt.HandsonInfkdt, ALTURA_HANDSON);

            } else if (evt.Resultado == "-2") {
                $("#mensajeListado1").html(evt.Mensaje);
                $("#listado1").html('');
            } else {
                alert(evt.Mensaje);
            }
        },
        error: function (err) {
            alert('Ha ocurrido un error.');
        }
    });
}

valueRenderer = function (instance, td, row, col, prop, value, cellProperties)
{
    Handsontable.renderers.TextRenderer.apply(this, arguments);
    var sHeader = headerht[col]; //$(instance.getCell(0, col)).html(); //.getColHeader(col)
    var sColumn = $(instance.getCell(row, 1)).html();
    var valueMaximum = 999999999;
    var valueMinimum = 0;
    var sMensaje;

    if (row == 0 && col == 2)
    {
        //Limpiamos la lista de errores, esto sólo se hace una sóla vez
        error = [];
    }

    if (value)
    {
        if (isNaN(parseInt(value, 10)))
        {
            td.style.backgroundColor = '#F02211';
            td.style.color = '#FFFFFF';
            td.style.fontWeight = 'bold';
            sMensaje = "[1]El valor " + value + " en el día " + sHeader + " para el tipo " + sColumn + " no es válido.";
        }
        else if (parseInt(value, 10) > valueMaximum || parseInt(value, 10) < valueMinimum)
        {
            td.style.background = '#F3F554';
            sMensaje = "[2]El valor " + value.toString() + " en el día " + sHeader + " para el tipo " + sColumn + " no está en el rango permitido: máximo = " + valueMaximum + " , mínimo = " + valueMinimum + ".";
        }
    }
    //else if (value != "0") 
    //{
    //    td.style.background = '#ECAFF0'; //lila
    //    sMensaje = "[3]El día " + sHeader + " para el tipo " + sColumn + " tiene un valor vacio (fila: " + (row + 1) + ", columna: " + col + ").";
    //}
    if (sMensaje)
    {
        if (!isNaN(value)) value = "";
        error.push(value.toString().concat("_-_" + row + "_-_" + sHeader + "_-_" + sMensaje));
    }
}

abrirPopupErrores = function ()
{
    var html = '<span class="button b-close"><span>X</span></span>';
    html += '<p><b>Corregir los siguientes errores</b><p>';
    html += '<table border="0" class="pretty tabla-icono" id="tabla">'
    html += '<thead>'
    html += '<tr>'
    html += '<th>Fila</th>'
    html += '<th>Día</th>'
    html += '<th>Valor</th>'
    html += '<th>Comentario</th>'
    html += '</tr>'
    html += '</thead>'
    html += '<tbody>'
    for (var i = error.length - 1; i >= 0; i--)
    {
        var sStyle = "background : #ffffff;";
        var sBackground = "";
        if (i % 2) {
            var sStyle = "background : #fbf4bf;";
        }
        var SplitError = error[i].split("_-_");
        var sTipError = SplitError[3].substring(0, 3);
        if (sTipError === "[1]") {
            sBackground = " background-color: #F02211;";
        }
        else if (sTipError === "[2]") {
            sBackground = " background-color: #F3F554;";
        }
        else if (sTipError === "[3]") {
            sBackground = " background-color: #ECAFF0;";
        }
        var sMsgError = SplitError[3].substring(3);
        html += '<tr id="Fila_' + i + '">'
        html += '<td style="text-align:right;' + sBackground + '">' + (parseInt(SplitError[1]) + 1) + '</td>'
        html += '<td style="text-align:left;' + sStyle + '">' + SplitError[2] + '</td>'
        html += '<td style="text-align:left;' + sStyle + '">' + SplitError[0] + '</td>'
        html += '<td style="text-align:left;' + sStyle + '">' + sMsgError + '</td>'
        html += '</tr>'
    }
    html += '</tbody>'
    html += '</table>'

    $('#popupErrores').html(html);
    $('#popupErrores').bPopup({
        easing: 'easeOutBack',
        speed: 450,
        transition: 'slideDown'
    });
    alert("La hoja del cálculo tiene errores.");
}
