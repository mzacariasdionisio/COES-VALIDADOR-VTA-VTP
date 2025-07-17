var controlador = siteRoot + 'Monitoreo/BandaTolerancia/';
var LISTA_JUSTIF = [];
var ancho = 900;
var MSJ_NO_DATA = 'No existen registros';

$(function () {
    $('#tab-container').easytabs({
        animate: false
    });

    ancho = $("#mainLayout").width() > 900 ? $("#mainLayout").width() - 30 : 900;
    var diaMesPicker = $('#hfDiaMesPicker').val();

    $('#txtMesIMM-1').Zebra_DatePicker({
        format: 'm Y',
        direction: diaMesPicker,
        onSelect: function () {
            filtroEmpresas(1);
        }
    });

    $('#txtMesIMM-2').Zebra_DatePicker({
        format: 'm Y',
        direction: diaMesPicker,
        onSelect: function () {
            filtroEmpresas(2);
        }
    });

    $('#txtMesIMM-3').Zebra_DatePicker({
        format: 'm Y',
        direction: diaMesPicker,
        onSelect: function () {
            filtroEmpresas(3);
        }
    });

    $('#txtMesIMM-4').Zebra_DatePicker({
        format: 'm Y',
        direction: diaMesPicker,
        onSelect: function () {
            filtroEmpresas(4);
        }
    });

    $('#txtMesIMM-5').Zebra_DatePicker({
        format: 'm Y',
        direction: diaMesPicker,
        onSelect: function () {
            filtroEmpresas(5);
        }
    });

    $('#txtMesIMM-6').Zebra_DatePicker({
        format: 'm Y',
        direction: diaMesPicker,
        onSelect: function () {
            filtroEmpresas(6);
        }
    });

    $('#txtMesIMM-7').Zebra_DatePicker({
        format: 'm Y',
        direction: diaMesPicker,
        onSelect: function () {
            filtroEmpresas(7);
        }
    });

    $('#cbEmpresaIMM-1').multipleSelect({
        width: '200px',
        filter: true,
        onClose: function (view) {
            pintarPaginado(1);
            pintarBusqueda(1);
        }
    });

    $('#cbEmpresaIMM-2').multipleSelect({
        width: '200px',
        filter: true,
        onClose: function (view) {
            pintarPaginado(2);
            pintarBusqueda2(1);
        }
    });


    $('#cbEmpresaIMM-3').multipleSelect({
        width: '200px',
        filter: true,
        onClose: function (view) {
            pintarPaginado(3);
            pintarBusqueda3(1);
        }
    });

    $('#cbEmpresaIMM-4').multipleSelect({
        width: '200px',
        filter: true,
        onClose: function (view) {
            pintarPaginado(4);
            pintarBusqueda4(1);
        }
    });

    $('#cbEmpresaIMM-5').multipleSelect({
        width: '200px',
        filter: true,
        onClose: function (view) {
            pintarPaginado(5);
            pintarBusqueda5(1);
        }
    });

    $('#cbEmpresaIMM-6').multipleSelect({
        width: '200px',
        filter: true,
        onClose: function (view) {
            pintarPaginado(6);
            pintarBusqueda6(1);
        }
    });

    $('#cbEmpresaIMM-7').multipleSelect({
        width: '200px',
        filter: true,
        onClose: function (view) {
            pintarPaginado(7);
            pintarBusqueda7(1);
        }
    });

    $('#cbEmpresaIMM-1').multipleSelect('checkAll');
    $('#cbEmpresaIMM-2').multipleSelect('checkAll');
    $('#cbEmpresaIMM-3').multipleSelect('checkAll');
    $('#cbEmpresaIMM-4').multipleSelect('checkAll');
    $('#cbEmpresaIMM-5').multipleSelect('checkAll');
    $('#cbEmpresaIMM-6').multipleSelect('checkAll');
    $('#cbEmpresaIMM-7').multipleSelect('checkAll');

    cargarVersionPrimeraPagina();

    $('#btnExportExcel1').click(function () {
        exportarExcelReporte(1);
    });
    $('#btnExportExcel2').click(function () {
        exportarExcelReporte(2);
    });
    $('#btnExportExcel3').click(function () {
        exportarExcelReporte(3);
    });
    $('#btnExportExcel4').click(function () {
        exportarExcelReporte(4);
    });
    $('#btnExportExcel5').click(function () {
        exportarExcelReporte(5);
    });
    $('#btnExportExcel6').click(function () {
        exportarExcelReporte(6);
    });
    $('#btnExportExcel7').click(function () {
        exportarExcelReporte(7);
    });

    $('#btnGrabarJustif1').click(function () {
        grabarJustif(1);
    });
    $('#btnGrabarJustif2').click(function () {
        grabarJustif(2);
    });
    $('#btnGrabarJustif3').click(function () {
        grabarJustif(3);
    });
    $('#btnGrabarJustif4').click(function () {
        grabarJustif(4);
    });
    $('#btnGrabarJustif5').click(function () {
        grabarJustif(5);
    });
    $('#btnGrabarJustif6').click(function () {
        grabarJustif(6);
    });
    $('#btnGrabarJustif7').click(function () {
        grabarJustif(7);
    });
});

function cargarVersionPrimeraPagina() {
    //Paginado de los reportes
    pintarPaginado(1);
    pintarPaginado(2);
    pintarPaginado(3);
    pintarPaginado(4);
    pintarPaginado(5);
    pintarPaginado(6);
    pintarPaginado(7);

    //Reporte
    var id = $("#idVersion").val();
    var empresa = "-1";
    var fechaPeriodo = $("#hfFechaPeriodo").val();

    $.ajax({
        type: 'POST',
        async: true,
        url: controlador + 'CargarIndicadorByTipo',
        data: {
            tipoIndicador: -1,
            idEmpresa: -1,
            nroPagina: 1,
            fechaInicial: fechaPeriodo
        },
        success: function (aData) {
            if (aData != null && aData.Resultado == "1") {
                mostrarDataByIndicador(aData.ListaResultado[0], 1, 2, 0);
                mostrarDataByIndicador(aData.ListaResultado[1], 2, 1, 2);
                mostrarDataByIndicador(aData.ListaResultado[2], 3, 3, 0);
                mostrarDataByIndicador(aData.ListaResultado[3], 4, 3, 0);
                mostrarDataByIndicador(aData.ListaResultado[4], 5, 1, 0);
                mostrarDataByIndicador(aData.ListaResultado[5], 6, 1, 0);
                mostrarDataByIndicador(aData.ListaResultado[6], 7, 1, 0);

                LISTA_JUSTIF = aData.ListaJustif;
            } else {
                alert("Ha ocurrido un error: " + aData.Resultado);
            }
        },
        error: function (err) {
            alert("Ha ocurrido un error");
        }
    });
}

function mostrarDataByIndicador(resultado, numero, colFijasIzq, colFijasDer) {
    if (resultado == null || resultado == '') {
        $('#listado' + numero).html(MSJ_NO_DATA);
    } else {
        $('#listado' + numero).html(resultado);
        var anchoReporte = $('#reporte' + numero).width();
        $("#resultado" + numero).css("width", (anchoReporte > ancho ? ancho : anchoReporte) + "px");
        $('#reporte' + numero).dataTable({
            "scrollX": true,
            "scrollY": "780px",
            "scrollCollapse": true,
            "sDom": 't',
            "ordering": false,
            paging: false,
            fixedColumns: {
                leftColumns: colFijasIzq,
                rightColumns: colFijasDer
            }
        });
    }
}

function cargarListaByIndicador(pagina, indicador, colFijasIzq, colFijasDer) {
    var empresa = $('#cbEmpresaIMM-' + indicador).multipleSelect('getSelects');
    if (empresa == "[object Object]" || empresa.length == 0) empresa = "-1";
    var fechaPeriodo = $("#txtMesIMM-" + indicador).val();

    if (empresa != "") {
        $.ajax({
            type: 'POST',
            async: true,
            url: controlador + 'CargarIndicadorByTipo',
            data: {
                tipoIndicador: indicador,
                idEmpresa: empresa.join(','),
                nroPagina: pagina,
                fechaInicial: fechaPeriodo
            },
            success: function (aData) {
                mostrarDataByIndicador(aData.ListaResultado[0], indicador, colFijasIzq, colFijasDer);
                actualizarListaJustif(aData.ListaJustif, indicador);
            },
            error: function (err) {
                alert("Ha ocurrido un error");
            }
        });
    } else {
        alert("Debe seleccionar al menos una empresa");
        $('#cbEmpresaIMM-' + indicador).multipleSelect('checkAll');
    }
}

function pintarBusqueda(nPagina1) {
    cargarListaByIndicador(nPagina1, 1, 2, 0);
}

function pintarBusqueda2(nPagina2) {
    cargarListaByIndicador(nPagina2, 2, 1, 2);
}

function pintarBusqueda3(nPagina3) {
    cargarListaByIndicador(nPagina3, 3, 3, 0);
}

function pintarBusqueda4(nPagina3) {
    cargarListaByIndicador(nPagina3, 4, 3, 0);
}

function pintarBusqueda5(nPagina5) {
    cargarListaByIndicador(nPagina5, 5, 1, 0);
}

function pintarBusqueda6(nPagina6) {
    cargarListaByIndicador(nPagina6, 6, 1, 0);
}

function pintarBusqueda7(nPagina7) {
    cargarListaByIndicador(nPagina7, 7, 1, 0);
}

function pintarPaginado(indicador) {
    var numIndicador = indicador == 1 ? "" : +indicador;
    $("#paginado" + numIndicador).html('');
    var empresa = $('#cbEmpresaIMM-' + indicador).multipleSelect('getSelects');
    var fecha = $('#txtMesIMM-' + indicador).val();
    $.ajax({
        type: 'POST',
        url: controlador + "/PaginadoIndicador" + indicador,
        data: {
            idEmpresa: empresa.join(','),
            fechaInicial: fecha
        },
        success: function (evt) {
            $('#paginado' + indicador).html(evt);
            eval("mostrarPaginado" + numIndicador)();
        },
        error: function (err) {
        }
    });
}

function filtroEmpresas(indicador) {
    var numIndicador = indicador == 1 ? "" : +indicador;
    var fechaDia = $('#txtMesIMM-' + indicador).val();
    $.ajax({
        type: 'POST',
        url: controlador + 'ComboEmpresa',
        data: {
            fechaDia: fechaDia,
            indicador: indicador
        },
        success: function (aData) {
            $('#divEmpresaIndicador' + indicador).html(aData);
            $('#cbEmpresaIMM-' + indicador).multipleSelect({
                width: '150px',
                filter: true,
                onClose: function (view) {
                    pintarPaginado(indicador);
                    eval("pintarBusqueda" + numIndicador)(1);
                }
            });
            $('#cbEmpresaIMM-' + indicador).multipleSelect('checkAll');
            pintarPaginado(indicador);
            eval("pintarBusqueda" + numIndicador)(1);
        },
        error: function (err) {
            alert('Ha ocurrido un error');
        }
    });
}

function exportarExcelReporte(indicador) {
    var numIndicador = indicador == 1 ? "" : +indicador;
    var empresa = $('#cbEmpresaIMM-' + indicador).multipleSelect('getSelects');
    var fechaDia = $('#txtMesIMM-' + indicador).val();
    var pagina = $('#hfPaginaActual' + numIndicador).val();

    $.ajax({
        type: 'POST',
        url: controlador + 'GenerarReporteExcelByTipo',
        data: {
            tipoIndicador: indicador,
            idEmpresa: empresa.join(','),
            nroPagina: pagina,
            fechaInicial: fechaDia
        },
        dataType: 'json',
        success: function (result) {
            switch (result.Total) {
                case 1: window.location = controlador + "ExportarReporteXls?nameFile=" + result.Resultado; break;// si hay elementos
                case 2: alert("No existen registros !"); break;// sino hay elementos
                case -1: alert(result.Mensaje); break;// Error en C#
            }
        },
        error: function (err) {
            alert("Error en reporte");;
        }
    });
}

////////////////////////////////////////////////////////////////////////
/// Justificaciones
////////////////////////////////////////////////////////////////////////

function actualizarListaJustif(listaNueva, indicador) {
    if (LISTA_JUSTIF != null && LISTA_JUSTIF.length > 0) {
        for (var i = 0; i < LISTA_JUSTIF.length; i++) {
            if (LISTA_JUSTIF[i].Immecodi == indicador) {
                LISTA_JUSTIF.splice(i, 1);
            }
        }
    } else {
        LISTA_JUSTIF = [];
    }
    LISTA_JUSTIF = LISTA_JUSTIF.concat(listaNueva);
}

function addJustif(element) {
    var emprcodi = parseInt($(element).parent().find("input[name='hfEmprcodi']").val()) || -1;
    var fechaHora = $(element).parent().find("input[name='hfFechaJustif']").val()
    var indicador = parseInt($(element).parent().find("input[name='hfIndicador']").val()) || -1;
    var barrcodi = parseInt($(element).parent().find("input[name='hfBarrcodi']").val()) || -1;
    var grupocodi = parseInt($(element).parent().find("input[name='hfGrupocodi']").val()) || -1;
    indicador = indicador > 0 ? indicador : null;
    emprcodi = emprcodi > 0 ? emprcodi : null;
    barrcodi = barrcodi > 0 ? barrcodi : null;
    grupocodi = grupocodi > 0 ? grupocodi : null;

    var obj = {
        Emprcodi: emprcodi,
        Immecodi: indicador,
        MjustfechaDesc: fechaHora,
        Barrcodi: barrcodi,
        Grupocodi: grupocodi,
        Mjustdescripcion: ''
    };

    mostrarPopupJustif(obj, element);
}

function editJustif(element) {
    var emprcodi = parseInt($(element).parent().find("input[name='hfEmprcodi']").val()) || -1;
    var fechaHora = $(element).parent().find("input[name='hfFechaJustif']").val()
    var indicador = parseInt($(element).parent().find("input[name='hfIndicador']").val()) || -1;
    var barrcodi = parseInt($(element).parent().find("input[name='hfBarrcodi']").val()) || -1;
    var grupocodi = parseInt($(element).parent().find("input[name='hfGrupocodi']").val()) || -1;
    indicador = indicador > 0 ? indicador : null;
    emprcodi = emprcodi > 0 ? emprcodi : null;
    barrcodi = barrcodi > 0 ? barrcodi : null;
    grupocodi = grupocodi > 0 ? grupocodi : null;

    var objJustif = buscarElementoJustif(indicador, emprcodi, fechaHora, barrcodi, grupocodi);
    txtjustif = objJustif != null && objJustif.Mjustdescripcion != null ? objJustif.Mjustdescripcion.trim() : '';

    var obj = {
        Emprcodi: emprcodi,
        Immecodi: indicador,
        MjustfechaDesc: fechaHora,
        Barrcodi: barrcodi,
        Grupocodi: grupocodi,
        Mjustdescripcion: txtjustif
    };

    mostrarPopupJustif(obj, element);
}

function buscarElementoJustif(indicador, emprcodi, fechaHora, barrcodi, grupocodi) {
    switch (indicador) {
        case 1:
        case 2:
        case 3:
        case 4:
            for (var i = 0; i < LISTA_JUSTIF.length; i++) {
                if (LISTA_JUSTIF[i].Immecodi == indicador && LISTA_JUSTIF[i].Emprcodi == emprcodi
                    && LISTA_JUSTIF[i].MjustfechaDesc == fechaHora) {
                    return LISTA_JUSTIF[i];
                }
            }
            break;
        case 5:
        case 6:
            for (var i = 0; i < LISTA_JUSTIF.length; i++) {
                if (LISTA_JUSTIF[i].Immecodi == indicador && LISTA_JUSTIF[i].Emprcodi == emprcodi && LISTA_JUSTIF[i].Barrcodi == barrcodi
                    && LISTA_JUSTIF[i].MjustfechaDesc == fechaHora) {
                    return LISTA_JUSTIF[i];
                }
            }
            break;
        case 7:
            for (var i = 0; i < LISTA_JUSTIF.length; i++) {
                if (LISTA_JUSTIF[i].Immecodi == indicador && LISTA_JUSTIF[i].Emprcodi == emprcodi && LISTA_JUSTIF[i].Grupocodi == grupocodi
                    && LISTA_JUSTIF[i].MjustfechaDesc == fechaHora) {
                    return LISTA_JUSTIF[i];
                }
            }
            break;
    }

    return null;
}

function getPosicionElementoJustif(indicador, emprcodi, fechaHora, barrcodi, grupocodi) {
    switch (indicador) {
        case 1:
        case 2:
        case 3:
        case 4:
            for (var i = 0; i < LISTA_JUSTIF.length; i++) {
                if (LISTA_JUSTIF[i].Immecodi == indicador && LISTA_JUSTIF[i].Emprcodi == emprcodi
                    && LISTA_JUSTIF[i].MjustfechaDesc == fechaHora) {
                    return i;
                }
            }
            break;
        case 5:
        case 6:
            for (var i = 0; i < LISTA_JUSTIF.length; i++) {
                if (LISTA_JUSTIF[i].Immecodi == indicador && LISTA_JUSTIF[i].Emprcodi == emprcodi && LISTA_JUSTIF[i].Barrcodi == barrcodi
                    && LISTA_JUSTIF[i].MjustfechaDesc == fechaHora) {
                    return i;
                }
            }
            break;
        case 7:
            for (var i = 0; i < LISTA_JUSTIF.length; i++) {
                if (LISTA_JUSTIF[i].Immecodi == indicador && LISTA_JUSTIF[i].Emprcodi == emprcodi && LISTA_JUSTIF[i].Grupocodi == grupocodi
                    && LISTA_JUSTIF[i].MjustfechaDesc == fechaHora) {
                    return i;
                }
            }
            break;
    }

    return -1;
}

function mostrarPopupJustif(obj, element) {
    $("#hfNuevoEmprcodi").val(obj.Emprcodi);
    $("#hfNuevoFechaJustif").val(obj.MjustfechaDesc);
    $("#hfNuevoIndicador").val(obj.Immecodi);
    $("#hfNuevoBarrcodi").val(obj.Barrcodi);
    $("#hfNuevoGrupocodi").val(obj.Grupocodi);
    $("#txtJustificacion").val(obj.Mjustdescripcion);
    $("#cbImme").val(obj.Immecodi);
    $("#cbEmpresaNuevo").val(obj.Emprcodi);
    $("#nuevaJustificacion .fecha_hora").html(obj.MjustfechaDesc);

    $("#btnGuardarJustif").unbind();
    setTimeout(function () {
        $('#popupJustificacion').bPopup({
            easing: 'easeOutBack',
            speed: 450,
            transition: 'slideDown'
        });
    }, 50);

    $('#btnGuardarJustif').click(function () {
        guardarJustif(element);
    });
}

function guardarJustif(element) {
    var emprcodi = parseInt($("#hfNuevoEmprcodi").val()) || -1;
    var fechaHora = $("#hfNuevoFechaJustif").val();
    var indicador = parseInt($("#hfNuevoIndicador").val()) || -1;
    var txtjustif = $("#txtJustificacion").val();
    var barrcodi = parseInt($("#hfNuevoBarrcodi").val()) || -1;
    var grupocodi = parseInt($("#hfNuevoGrupocodi").val()) || -1;
    txtjustif = txtjustif != null ? txtjustif.trim() : '';

    indicador = indicador > 0 ? indicador : null;
    emprcodi = emprcodi > 0 ? emprcodi : null;
    barrcodi = barrcodi > 0 ? barrcodi : null;
    grupocodi = grupocodi > 0 ? grupocodi : null;

    var obj = {
        Emprcodi: emprcodi,
        Immecodi: indicador,
        MjustfechaDesc: fechaHora,
        Barrcodi: barrcodi,
        Grupocodi: grupocodi,
        Mjustdescripcion: txtjustif
    };

    var posicionLista = getPosicionElementoJustif(indicador, emprcodi, fechaHora, barrcodi, grupocodi);
    if (posicionLista == -1) { //nuevo
        LISTA_JUSTIF.push(obj);
    } else {//edicion
        LISTA_JUSTIF[posicionLista].Mjustdescripcion = txtjustif;
    }

    if (txtjustif.length > 0) {
        $(element).parent().find("img[class='edit']").show();
        $(element).parent().find("img[class='add']").hide();
    } else {
        $(element).parent().find("img[class='edit']").hide();
        $(element).parent().find("img[class='add']").show();
    }

    $('#popupJustificacion').bPopup().close();
}

/// Grabar
function grabarJustif(indicador) {
    if (confirm('¿Desea grabar las justificaciones?')) {
        var obj = getObjetoJsonJustif();

        var numIndicador = indicador == 1 ? "" : +indicador;
        var nroPagina = $("#hfPaginaActual" + numIndicador).val();
        var fechaDia = $('#txtMesIMM-' + indicador).val();

        $.ajax({
            type: 'POST',
            dataType: 'json',
            traditional: true,
            url: controlador + 'GuardarJustificacionXIndicadorYDia',
            data: {
                dataJsonJustif: obj,
                tipoIndicador: indicador,
                nroPagina: nroPagina,
                fechaInicial: fechaDia
            },
            cache: false,
            success: function (result) {
                if (result.Resultado == '-1') {
                    alert('Ha ocurrido un error:' + result.Mensaje);
                } else {
                    alert("Se guardó correctamente las justificaciones");
                }
            },
            error: function (err) {
                alert('Ha ocurrido un error.');
            }
        });
    }
}

function getObjetoJsonJustif() {
    if (LISTA_JUSTIF != null && LISTA_JUSTIF.length > 0) {
        for (var i = 0; i < LISTA_JUSTIF.length; i++) {
            if (LISTA_JUSTIF[i].Mjustfecha != undefined) {
                LISTA_JUSTIF[i].Mjustfecha = null;
                LISTA_JUSTIF[i].Mjustfeccreacion = null;
                LISTA_JUSTIF[i].Mjustfecmodificacion = null;
            }
            if (LISTA_JUSTIF[i].Mjustfeccreacion != undefined) {
                LISTA_JUSTIF[i].Mjustfeccreacion = null;
            }
            if (LISTA_JUSTIF[i].Mjustfecmodificacion != undefined) {
                LISTA_JUSTIF[i].Mjustfecmodificacion = null;
            }
        }
    } else {
        LISTA_JUSTIF = [];
    }
    return JSON.stringify(LISTA_JUSTIF);
}