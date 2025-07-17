var controlador = siteRoot + 'Equipamiento/FTReporteHistorico/';

const ETAPA_MODIFICACION = 4;

$(function () {
    $.fn.dataTable.moment('DD/MM/YYYY');

    //ocultar barra lateral izquierda
    $("#btnOcultarMenu").click();

    $('#cbEtapa').change(function () {
        //visible fechas y estado
        $("#tr_fecha_consulta").hide();

        $("#cbEstado").empty();

        var idEtapa = getFiltro().idEtapa;
        if (idEtapa == ETAPA_MODIFICACION) {
            $("#tr_fecha_consulta").css("display", "inline-table");

            $('#cbEstado').append(`<option value="-1">--TODOS--</option>`);
            $('#cbEstado').append(`<option value="A">Activo</option>`);
            $('#cbEstado').append(`<option value="F">Fuera de COES</option>`);
            $('#cbEstado').append(`<option value="B">Baja</option>`);
        } else {
            $('#cbEstado').append(`<option value="-1">--TODOS--</option>`);
            $('#cbEstado').append(`<option value="A">Activo</option>`);
            $('#cbEstado').append(`<option value="P">En Proyecto</option>`);
            $('#cbEstado').append(`<option value="F">Fuera de COES</option>`);
            $('#cbEstado').append(`<option value="B">Baja</option>`);
        }

        filtroXEtapaYTipoempresa();
    });
    $("#tr_fecha_consulta").css("display", "inline-table");
    $('#cboTipoEmpresa').change(function () {
        filtroXEtapaYTipoempresa();
    });

    //empresa
    $('#cbEmpresa').multipleSelect({
        filter: true,
        onClose: function (view) {
        }
    });
    $('#cboFamilia').multipleSelect({
        filter: true,
        onClose: function (view) {
            filtroXAgrupacion();
        }
    });
    $('#cbCategoria').multipleSelect({
        filter: true,
        onClose: function (view) {
            filtroXAgrupacion();
        }
    });

    $('#cbTipoDato').multipleSelect({
        filter: true,
    });

    $('#FechaConsulta').Zebra_DatePicker({
        format: "d/m/Y",
    });
    $('#FechaDesde').Zebra_DatePicker({
        format: "d/m/Y",
        pair: $('#FechaHasta'),
        direction: false,
    });
    $('#FechaHasta').Zebra_DatePicker({
        format: "d/m/Y",
        pair: $('#FechaDesde'),
        direction: true,
    });

    //agrupacion
    $('#cbAgrupacion').change(function () {
        filtroXAgrupacion();
    });
    $('#cbParametroProp').multipleSelect({
        filter: true,
    });
    $('#cbParametroConcep').multipleSelect({
        filter: true,
    });

    $('#cboFamilia').multipleSelect('checkAll');
    $('#cbCategoria').multipleSelect('checkAll');

    $(".td_historico").hide();
    $("#chkHistorico").on("click", function () {
        var check = $('#chkHistorico').is(":checked");
        if (check) {
            $(".td_fechaconsulta").hide();
            $(".td_historico").show();
        }
        else {
            $(".td_fechaconsulta").show();
            $(".td_historico").hide();
        }
    });

    $('#cbEmpresa').multipleSelect('checkAll');
    $('#cbParametroProp').multipleSelect('checkAll');
    $('#cbParametroConcep').multipleSelect('checkAll');

    $('#btnConsultar').on('click', function () {
        buscarElementos();
    });

    $('#btnExportar').click(function () {
        exportarExcel();
    });

    ancho = $('#mainLayout').width() - 30;

    _inicializarFiltros();
});

async function _inicializarFiltros() {
    await filtroXEtapaYTipoempresa();
    await filtroXAgrupacion();
}

function getFiltro() {
    var obj = {};
    obj.idEtapa = parseInt($("#cbEtapa").val()) || 0;
    obj.idEmpresa = _getArrayStringMultiple("#cbEmpresa");
    obj.tipoEmpresa = parseInt($("#cboTipoEmpresa").val()) || 0;
    obj.famcodis = _getArrayStringMultiple("#cboFamilia");
    obj.catecodis = _getArrayStringMultiple("#cbCategoria");

    obj.idAgrupacion = parseInt($("#cbAgrupacion").val()) || 0;
    obj.propcodis = _getArrayStringMultiple("#cbParametroProp");
    obj.concepcodis = _getArrayStringMultiple("#cbParametroConcep");
    obj.tipocodis = $('#cbTipoDato').multipleSelect('getSelects').join(',');

    var check = $('#chkHistorico').is(":checked");
    obj.historico = check;
    if (ETAPA_MODIFICACION != obj.idEtapa) obj.historico = false;

    var checkFormula = $('#chkFormulaEnValor').is(":checked");
    obj.flagFormulaEnValor = checkFormula;

    obj.fecha = $("#FechaConsulta").val();
    obj.fechaDesde = $("#FechaDesde").val();
    obj.fechaHasta = $("#FechaHasta").val();
    obj.estado = $("#cbEstado").val();

    var fechaIni = "";
    var fechaFin = "";
    if (obj.historico) {
        fechaIni = obj.fechaDesde;
        fechaFin = obj.fechaHasta;
    }
    else {
        fechaIni = obj.fecha;
        fechaFin = obj.fecha;
    }

    obj.FechaIniConsulta = fechaIni;
    obj.FechaFinConsulta = fechaFin;

    return obj;
}

function _getArrayStringMultiple(id) {
    var arrayFiltro = $(id).multipleSelect('getSelects');
    if (arrayFiltro != "" && arrayFiltro != null && arrayFiltro != undefined) {
        return arrayFiltro.join(',')
    }

    return "";
}

async function buscarElementos() {
    $('#textoMensaje').hide();

    var objFiltro = getFiltro();

    return $.ajax({
        type: 'POST',
        url: controlador + "ListarDatoReporteHistoricoParametro",
        data: {
            tipoEmpresaFT: objFiltro.tipoEmpresa,
            idEtapa: objFiltro.idEtapa,
            idEmpresa: objFiltro.idEmpresa,
            famcodis: objFiltro.famcodis,
            catecodis: objFiltro.catecodis,
            propcodis: objFiltro.propcodis,
            concepcodis: objFiltro.concepcodis,
            tipocodis: objFiltro.tipocodis,
            historico: objFiltro.historico,
            flagFormulaEnValor: objFiltro.flagFormulaEnValor,
            fecha: objFiltro.FechaIniConsulta,
            fechafin: objFiltro.FechaFinConsulta,
            estado: objFiltro.estado,
        },
        success: function (evt) {
            if (evt.Resultado != "-1") {
                if (evt.Resultado == "1") {
                    $('#listado').css("width", ancho + "px");

                    $('#listado').html(evt);
                    var html = _dibujarTablaListado(evt.ListaDataRptHist);
                    $('#listado').html(html);

                    setTimeout(function () {
                        $('#tabla_rpt').dataTable({
                            "destroy": "true",
                            "scrollY": $('#listado').height() > 200 ? 500 + "px" : "100%",
                            "scrollX": true,
                            scrollCollapse: true,
                            "sDom": 'ft',
                            "aaSorting": [],
                            "iDisplayLength": -1
                        });
                    }, 250);

                    //$('#cbTipoDato').multipleSelect('setSelects', [2, 3]);
                } else {
                    alert("El listado web excede los 1000 registros. La consulta solo está disponible en reporte Excel.");
                }
            } else {
                alert(evt.Mensaje);
            }
        },
        error: function (err) {
            mostrarError();
        }
    });
};

function _dibujarTablaListado(lista) {
    var tipoEmpresa = getFiltro().tipoEmpresa;
    var textoCol2 = "";
    var textoCol3 = "";
    switch (tipoEmpresa) {
        case 10: //Generación
            textoCol2 = "Central";
            textoCol3 = "Unidad Generación / <br> Modo Operación";
            break;
        case 11: //Transmisión (Subestación)
            textoCol2 = "Subestación";
            textoCol3 = "Equipo";
            break;
        case 12: //Transmisión (Líneas)
            textoCol2 = "";
            textoCol3 = "Línea";
            break;
        case 13: //Transmisión (Equipos de compensación)
            textoCol2 = "Equipo de Compensación";
            textoCol3 = "Equipo";
            break;
    }

    var thHtml = textoCol2 != "" ? `<th>${textoCol2}</th>` : "";


    var fechavigHtml = "";
    var valorHtml = "";
    var valorCeroHtml = "";
    var comentarioHtml = "";
    var sustentoHtml = "";
    var usuariomodifHtml = "";
    var fechamodifHtml = "";
    var selecionados = $('#cbTipoDato').multipleSelect('getSelects').join(',');
    const myArray = selecionados.split(",");
    for (key in myArray) {
        var idCombo = myArray[key];
        var idcolumna = parseInt(idCombo);

        switch (idcolumna) {
            case 1: // columna fecha vigencia
                fechavigHtml = "<th>Fecha de vigencia</th>";
                break;
            case 2: // columna valor
                valorHtml = "<th>Valor</th>";
                break;
            case 3: // columna valor cero correcto
                valorCeroHtml = "<th>Valor cero (0) correcto</th>";
                break;
            case 4: // columna comentario
                comentarioHtml = "<th>Comentario</th>";
                break;
            case 5: // columna sustento
                sustentoHtml = "<th>Sustento</th>";
                break;
            case 6:// columna usuario modif
                usuariomodifHtml = "<th>Usuario modificación</th>";
                break;
            case 7:// columna fecha modif
                fechamodifHtml = "<th>Fecha modificación</th>";
                break;
        }
    }

    var cadena = '';
    cadena += `
    <table border="0" class="pretty tabla-icono" id="tabla_rpt" cellspacing="0" width="100%" >
        <thead>
            <tr>
            <th>Empresa</th>
            ${thHtml}
            <th>${textoCol3}</th>
            <th>Cod. Parámetro</th>
            <th>Nombre del parámetro</th>
            <th>Nombre Ficha Técnica</th>
            <th>Unidad de medida</th>
            ${fechavigHtml}
            ${valorHtml}
            ${valorCeroHtml}
            ${comentarioHtml}
            ${sustentoHtml}
            ${usuariomodifHtml}
            ${fechamodifHtml}
            </tr>
        </thead>
        <tbody>
    `;

    for (key in lista) {
        var item = lista[key];

        var tdHtml = textoCol2 != "" ? `<td class="ft_item">${item.Central}</td>` : "";

        var tdfechavigHtml = fechavigHtml != "" ? `<td class="ft_item">${item.FechaVigenciaDesc}</td>` : "";
        var tdvalorHtml = "";
        if (valorHtml != "") {

            var tdHtmlValor = item.EsArchivoValor ? getHtmlCeldaArchivo(item.Valor) : `${item.Valor}`;
            tdvalorHtml = `<td class="ft_item">${tdHtmlValor}</td>`;
        }

        var tdvalorCeroHtml = valorCeroHtml != "" ? `<td class="ft_item">${item.CheckCeroDesc}</td>` : "";
        var tdcomentarioHtml = comentarioHtml != "" ? `<td class="ft_item">${item.Comentario}</td>` : "";

        tdsustentoHtml = "";
        if (sustentoHtml != "") {
            var tdHtmlSustento = getHtmlCeldaArchivo(item.Sustento);
            tdsustentoHtml = `<td class="ft_item">${tdHtmlSustento}</td>`;
        }

        var tdusuariomodifHtml = usuariomodifHtml != "" ? `<td class="ft_item">${item.Usuariomodif}</td>` : "";
        var tdfechamodifHtml = fechamodifHtml != "" ? `<td class="ft_item">${item.Fechamodif}</td>` : "";

        var claseFila = "";

        cadena += `

            <tr id="fila_${item.Codigo}" class=${claseFila}>
                <td class="ft_item">${item.Empresa}</td>
                ${tdHtml}
                <td class="ft_item">${item.Elemento}</td>
                <td class="ft_item">${item.CodParam}</td>
                <td class="ft_item">${item.NombParam}</td>
                <td class="ft_item">${item.NombParamFT}</td>
                <td class="ft_item">${item.Unidad}</td>
                ${tdfechavigHtml}
                ${tdvalorHtml}
                ${tdvalorCeroHtml}
                ${tdcomentarioHtml}
                ${tdsustentoHtml}
                ${tdusuariomodifHtml}
                ${tdfechamodifHtml}
            </tr>
        `;
    }
    cadena += "</tbody></table>";

    return cadena;
}

function getHtmlCeldaArchivo(urlSustento) {
    var htmlDiv = '';

    //mostrar icono cuando exista url de descarga
    if (urlSustento != "" && urlSustento != null) {

        var arrayLink = getListaEnlaceXTexto(urlSustento);
        htmlDiv = '';
        for (var i = 0; i < arrayLink.length; i++) {
            var link = arrayLink[i];
            var esConfidencial = (link.toLocaleUpperCase()).includes('DescargarSustentoConfidencial?'.toLocaleUpperCase());
            var textoTitle = esConfidencial ? 'Descargar archivo de sustento (CONFIDENCIAL)' : 'Descargar archivo de sustento';

            htmlDiv += `
                <div class='estiloSustentoProp' title='${textoTitle} - ${link}' onclick='descargarArchivoSustento("${link}")'>
                    
                </div>
            `;
        }
    }

    return htmlDiv;
}

function getListaEnlaceXTexto(texto) {
    if (texto == null || texto == undefined) texto = "";
    texto = texto.trim();

    texto = texto.replace(/(?:\r\n|\r|\n)/g, ' ');
    texto = texto.replace(/(\n)+/g, ' ');

    var arrayLink = [];
    var arraySep = texto.split(' ');
    for (var i = 0; i < arraySep.length; i++) {
        var posibleLink = arraySep[i].trim();
        if (posibleLink.length > 0 && (
            posibleLink.toLowerCase().startsWith('http') || posibleLink.toLowerCase().startsWith('www'))) {
            arrayLink.push(posibleLink);
        }
    }

    return arrayLink;
}

function descargarArchivoSustento(urlSustento) {
    if (urlSustento != "" && urlSustento != null) {

        window.open(urlSustento, '_blank').focus();
    }
}

function mostrarError() {
    $('#textoMensaje').css("display", "block");
    $('#textoMensaje').removeClass();
    $('#textoMensaje').addClass('action-alert');
    $('#textoMensaje').text("Ha ocurrido un error");
}

///////////////////////////
/// Exportación masiva
///////////////////////////
function exportarExcel() {
    $('#textoMensaje').hide();

    var objFiltro = getFiltro();

    $.ajax({
        type: 'POST',
        url: controlador + 'ExportarReporteHistoricoParametro',
        data: {
            tipoEmpresaFT: objFiltro.tipoEmpresa,
            idEtapa: objFiltro.idEtapa,
            idEmpresa: objFiltro.idEmpresa,
            famcodis: objFiltro.famcodis,
            catecodis: objFiltro.catecodis,
            propcodis: objFiltro.propcodis,
            concepcodis: objFiltro.concepcodis,
            tipocodis: objFiltro.tipocodis,
            historico: objFiltro.historico,
            flagFormulaEnValor: objFiltro.flagFormulaEnValor,
            fecha: objFiltro.FechaIniConsulta,
            fechafin: objFiltro.FechaFinConsulta,
            estado: objFiltro.estado,
        },
        success: function (evt) {
            if (evt.Resultado != "-1") {
                window.location.href = controlador + 'DescargarExcel?archivo=' + evt.Resultado2 + '&nombre=' + evt.Resultado;
            }
            else {
                alert("Error:" + evt.Mensaje);
            }
        },
        error: function (result) {
            alert('Ha ocurrido un error al descargar el archivo excel. ' + result.status + ' - ' + result.statusText + '.');
        }
    });
}

///////////////////////////
/// filtros
///////////////////////////

async function filtroXAgrupacion() {
    var objFiltro = getFiltro();

    return $.ajax({
        type: 'POST',
        url: controlador + "ListarFiltroHistParamXAgrupacion",
        data: {
            agrupcodi: objFiltro.idAgrupacion,
            famcodis: objFiltro.famcodis,
            catecodis: objFiltro.catecodis,
        },
        success: function (model) {
            if (model.Resultado != "-1") {
                $('#cbParametroProp').unbind();
                $('#cbParametroConcep').unbind();

                $("#cbParametroProp").empty();
                $("#cbParametroConcep").empty();

                if (model.ListaPropiedad.length > 0) {
                    for (var i = 0; i < model.ListaPropiedad.length; i++) {
                        $('#cbParametroProp').append(`<option value="${model.ListaPropiedad[i].Propcodi}">${model.ListaPropiedad[i].Propnomb}</option>`);
                    }
                }

                if (model.ListaConcepto.length > 0) {
                    for (var i = 0; i < model.ListaConcepto.length; i++) {
                        $('#cbParametroConcep').append(`<option value="${model.ListaConcepto[i].Concepcodi}">${model.ListaConcepto[i].Concepdesc}</option>`);
                    }
                }

                $('#cbParametroProp').multipleSelect({
                    filter: true,
                });
                $('#cbParametroProp').multipleSelect('checkAll');
                $('#cbParametroConcep').multipleSelect({
                    filter: true,
                });
                $('#cbParametroConcep').multipleSelect('checkAll');
            } else {
                alert(model.Mensaje);
            }
        },
        error: function (err) {
            mostrarError();
        }
    });
};

async function filtroXEtapaYTipoempresa() {

    return $.ajax({
        type: 'POST',
        url: controlador + "ListarFiltroHistParamXEtapaYTipoempresa",
        data: {
            tipoEmpresaFT: getFiltro().tipoEmpresa,
            idEtapa: getFiltro().idEtapa,
        },
        success: function (model) {
            if (model.Resultado != "-1") {
                $('#cbEmpresa').unbind();

                $("#cbEmpresa").empty();

                $("#tr_emp").show();
                for (var i = 0; i < model.ListaEmpresa.length; i++) {
                    $('#cbEmpresa').append(`<option value="${model.ListaEmpresa[i].Emprcodi}">${model.ListaEmpresa[i].Emprnomb}</option>`);
                }

                //empresa
                $('#cbEmpresa').multipleSelect({
                    filter: true,
                    onClose: function (view) {
                    }
                });
                $('#cbEmpresa').multipleSelect('checkAll');
            } else {
                alert(model.Mensaje);
            }
        },
        error: function (err) {
            mostrarError();
        }
    });
};

