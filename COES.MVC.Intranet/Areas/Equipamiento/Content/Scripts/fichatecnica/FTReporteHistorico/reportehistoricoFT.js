var controlador = siteRoot + 'Equipamiento/FTReporteHistorico/';

const ETAPA_MODIFICACION = 4;
var TIPO_SUBESTACION = -2;

$(function () {
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

        listarEmpresas();
    });
    $("#tr_fecha_consulta").css("display", "inline-table");

    $('#cbFichaMaestra').change(function () {
        listarFichaTecnica();
    });

    $('#cbFichaTecnica').change(function () {
        listarEmpresas();
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

    $('#cbEmpresa').multipleSelect({
        filter: true,
        onClose: function (view) {
        }
    });
    $('#cbEmpresa').multipleSelect('checkAll');

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
    await buscarElementos();
}

function getFiltro() {
    var obj = {};
    obj.idEtapa = parseInt($("#cbEtapa").val()) || 0;
    obj.idFicha = parseInt($("#cbFichaTecnica").val()) || 0;
    obj.idEmpresa = _getArrayStringMultiple("#cbEmpresa");

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

// Lista de Equipos
async function buscarElementos() {
    $('#textoMensaje').css("display", "none");
    $("#listado").html('');

    var objFiltro = getFiltro();
    var msj = "";
    if (objFiltro.idFicha <= 0) {
        msj += "Debe seleccionar una Ficha Técnica";
    }

    if (msj == "") {
        return $.ajax({
            type: 'POST',
            url: controlador + "ElementosListado",
            data: {
                idEtapa: objFiltro.idEtapa,
                idFicha: objFiltro.idFicha,
                iEmpresa: objFiltro.idEmpresa,
                tipocodis: objFiltro.tipocodis,
                historico: objFiltro.historico,
                flagFormulaEnValor: objFiltro.flagFormulaEnValor,
                fecha: objFiltro.FechaIniConsulta,
                fechafin: objFiltro.FechaFinConsulta,
                iEstado: objFiltro.estado
            },
            success: function (evt) {
                if (evt.Resultado != "-1") {
                    $('#listado').css("width", ancho + "px");

                    $('#listado').html(evt);
                    var html = _dibujarTablaListado(evt);
                    $('#listado').html(html);

                    setTimeout(function () {
                        $('#tabla').dataTable({
                            "destroy": "true",
                            "scrollY": $('#listado').height() > 200 ? 500 + "px" : "100%",
                            "scrollX": true,
                            scrollCollapse: true,
                            "sDom": 'ft',
                            "iDisplayLength": -1
                        });
                    }, 250);

                } else {
                    alert(evt.Mensaje);
                }
            },
            error: function (err) {
                mostrarError();
            }
        });
    } else {
        $('#textoMensaje').css("display", "block");
        $('#textoMensaje').removeClass();
        $('#textoMensaje').addClass('action-alert');
        $('#textoMensaje').text(msj);
    }
};

function _dibujarTablaListado(model) {
    var lista = model.ListaElemento;

    var htmlThEmpresa = "";
    if (TIPO_SUBESTACION != 1) {
        htmlThEmpresa = `<th>Empresa</th>`;
    }

    var cadena = '';
    cadena += `
    <table border="0" class="pretty tabla-icono" id="tabla" cellspacing="0" width="100%" >
        <thead>
            <tr>
            <th>Código</th>
            <th>Nombre </th>
            <th>Abreviatura</th>
            ${htmlThEmpresa}
            <th>Ubicación</th>
            </tr>
        </thead>
        <tbody>
    `;

    for (key in lista) {
        var item = lista[key];

        var claseFila = "";
        var htmlTdEmpresa = "";
        if (TIPO_SUBESTACION != 1) {
            htmlTdEmpresa = `<td class="ft_item">${item.Empresa}</td>`;
        }

        //var tdOpciones = _tdAcciones(model, item);
        cadena += `

            <tr id="fila_${item.Codigo}" class=${claseFila}>
                <td class="ft_item" style='text-align: center !important;'>${item.Codigo}</td>
                <td class="ft_item">${item.Nombre}</td>
                <td class="ft_item" style='text-align: center !important;'>${item.Abreviatura}</td>
                ${htmlTdEmpresa}
                <td class="ft_item">${item.Ubicacion}</td>
            </tr>
        `;
    }
    cadena += "</tbody></table>";

    return cadena;
}

///////////////////////////
/// Exportación masiva
///////////////////////////
function exportarExcel() {
    var objFiltro = getFiltro();
    var msj = "";
    if (objFiltro.idFicha <= 0) {
        msj += "Debe seleccionar una Ficha Técnica";
    }

    if (msj == "") {
        $.ajax({
            type: 'POST',
            url: controlador + 'ExportarMasivoFichaTecnica',
            data: {
                idEtapa: objFiltro.idEtapa,
                idFicha: objFiltro.idFicha,
                iEmpresa: objFiltro.idEmpresa,
                tipocodis: objFiltro.tipocodis,
                historico: objFiltro.historico,
                flagFormulaEnValor: objFiltro.flagFormulaEnValor,
                fecha: objFiltro.FechaIniConsulta,
                fechafin: objFiltro.FechaFinConsulta,
                iEstado: objFiltro.estado
            },
            success: function (evt) {
                if (evt.Error == undefined) {
                    if (evt[0] == null || evt[0] == "" || evt[1] == null || evt[1] == "") {
                        alert("No existen elementos a exportar.");
                    } else {
                        window.location.href = controlador + 'DescargarExcel?archivo=' + evt[0] + '&nombre=' + evt[1];
                    }
                }
                else {
                    alert("Error:" + evt.Descripcion);
                }
            },
            error: function (result) {
                alert('Ha ocurrido un error al descargar el archivo excel. ' + result.status + ' - ' + result.statusText + '.');
            }
        });
    } else {
        alert(msj);
    }
}

function mostrarError() {
    $('#textoMensaje').css("display", "block");
    $('#textoMensaje').removeClass();
    $('#textoMensaje').addClass('action-alert');
    $('#textoMensaje').text("Ha ocurrido un error");
}

///////////////////////////
/// filtros
///////////////////////////

// Lista de Ficha Tecnica
function listarFichaTecnica() {

    $('#cbFichaTecnica').unbind();
    $('#cbFichaTecnica').empty();

    $('#cbEmpresa').empty();
    var idFTmaestra = parseInt($("#cbFichaMaestra").val()) || 0;

    if (idFTmaestra > 0) {
        $.ajax({
            type: 'POST',
            url: controlador + 'ListaFichaTecnicaXMaestra',
            dataType: 'json',
            async: true,
            data: {
                idFTmaestra: idFTmaestra
            },
            success: function (result) {
                if (result.Resultado == "1") {
                    var listaFT = result.ListaFichaTecnicaSelec;
                    for (var i = 0; i < listaFT.length; i++) {
                        $('#cbFichaTecnica').append('<option value=' + listaFT[i].Fteqcodi + '>' + listaFT[i].Fteqnombre + '</option>');
                    }

                    $('#cbFichaTecnica').change(function () {
                        listarEmpresas();
                    });

                    listarEmpresas();
                } else {
                    mostrarError();
                }
            },
            error: function () {
                mostrarError();
            }
        });
    }
}

// Lista de Empresas
function listarEmpresas() {

    var idFT = parseInt($("#cbFichaTecnica").val()) || 0;

    if (idFT > 0) {
        $.ajax({
            type: 'POST',
            url: controlador + 'ListaEmpresaXFichaTecnica',
            dataType: 'json',
            async: true,
            data: {
                idFT: idFT
            },
            success: function (result) {
                if (result.Resultado == "1") {
                    $('#cbEmpresa').unbind();

                    $('#cbEmpresa').empty();

                    TIPO_SUBESTACION = result.TipoElementoId;

                    if (TIPO_SUBESTACION != 1) {
                        // mostrar combo
                        $("#tr_emp").show();

                        var listaEmp = result.ListaEmpresa;
                        for (var i = 0; i < listaEmp.length; i++) {
                            $('#cbEmpresa').append('<option value=' + listaEmp[i].Emprcodi + '>' + listaEmp[i].Emprnomb + '</option>');
                        }
                    }
                    else {
                        //ocultar combo
                        $("#tr_emp").hide();
                    }

                    $('#cbEmpresa').multipleSelect({
                        filter: true,
                    });
                    $('#cbEmpresa').multipleSelect('checkAll');
                } else {
                    mostrarError();
                }
            },
            error: function () {
                mostrarError();
            }
        });
    }
}
