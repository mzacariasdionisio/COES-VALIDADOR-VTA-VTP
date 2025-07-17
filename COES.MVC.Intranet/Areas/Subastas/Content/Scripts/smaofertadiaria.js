var controlador = siteRoot + 'Subastas/Oferta/';
var TIPO_OFERTA_DEFECTO = 0;
var TIPO_OFERTA_DIARIO = 1;

var HTML_ENVIO = '';
var HEIGHT_MINIMO = 500;

$(document).ready(function () {
    var formatoFecha = TIPO_OFERTA_DIARIO == parseInt($("#modulo").val()) ? 'd/m/Y' : 'm Y';

    $('#dte-subasta-fecha').Zebra_DatePicker({
        format: formatoFecha,
        onSelect: function (date) {
            $('#dte-subasta-fechaFin').val(date);
        }
    });
    $('#dte-subasta-fechaFin').Zebra_DatePicker({
        format: formatoFecha,
    });

    $('#urs').multipleSelect({
        selected: true,
        width: '150px',
        onClick: function (view) {
        },
        onClose: function (view) {
        }
    });
    $('#urs').multipleSelect('checkAll');

    // Boton envios
    $('#btnEnvios').click(function () {
        verEnvios();
    });

    // Filtro para listar el combo USUARIO al escoger EMPRESA
    $('#empresa').change(function () {
        listarUsuarios($('#empresa').val());
    });

    $('#username').change(function () {
        listarUrs($('#username').val());
    });

    $('#btn-consultar').click(function () {
        formExcel(0);
    });

    $('#btn-exportar').click(function () {
        exportarReporte();
    });

    $('#tab-container').easytabs({
        animate: false
    });

    formExcel(0);
});

function formExcel(idEnvio) {
    $("#hdOfercodi").val(idEnvio);
    $("#mensaje_envio").hide();

    var opcion = idEnvio > 0 ? 2 : 1;

    $("#hst-subasta-ingreso-subir").html('');
    $("#hst-subasta-ingreso-bajar").html('');
    var container1 = document.getElementById('hst-subasta-ingreso-subir');
    var container2 = document.getElementById('hst-subasta-ingreso-bajar');
    var hot1 = {};
    var hot2 = {};

    var resultado = validarFechas();
    if (resultado === "") {
        $.ajax({
            type: "POST",
            url: controlador + "grillaexcel",
            data: {
                empresacodi: $("#empresa").val(),
                tipooferta: $("#modulo").val(),
                oferfechaenvio: $('#dte-subasta-fecha').val(),
                oferfechaenviofin: $('#dte-subasta-fechaFin').val(),
                usercode: $('#username').val(),
                opcion: opcion,
                urs: $('#urs').val(),
                ofercodi: $("#hdOfercodi").val()
            },
            success: function (model) {
                if (model.Resultado != -1) {
                    //mensaje envio
                    if (model.OferCodi > 0) {
                        $("#mensaje_envio").show();
                        var estadoHtml = "<strong style='" + (model.OferEstado == "H" ? 'color:gray' : 'color:green') + "'>" + (model.OferEstado == "H" ? 'Histórico' : 'Activo') + "</strong>";
                        $("#mensaje_envio").html("<strong>Código de envío</strong>: " + model.OferCodenvio
                            + ", <strong>Fecha de envío: </strong>" + model.OferfechaenvioDesc
                            + ", <strong>Estado de envío: </strong>" + estadoHtml);
                    }

                    //handson
                    for (var tab = 0; tab < 2; tab++) {
                        var result = model.ListaTab[tab];
                        var container = tab == 0 ? container1 : container2;
                        var hot = tab == 0 ? hot1 : hot2;

                        $('#mensaje-valor-' + (tab + 1)).html("")
                        var respuesta = result.resultado;
                        if (respuesta != null) {
                            $('#mensaje-valor-' + (tab + 1)).html('<b>Mensaje:  </b>' + respuesta);
                        }

                        var columns = result.Columnas;
                        var headers = result.Headers;
                        var widths = result.ListaColWidth;
                        var data = result.ListaExcelData;
                        var arrMergeCells = result.ListaMerge;

                        hot = new Handsontable(container, {
                            data: data,
                            stretchH: "all",
                            observeChanges: true,
                            colHeaders: headers,
                            colWidths: widths,
                            rowHeaders: true,
                            columnSorting: false,
                            contextMenu: false,
                            minSpareRows: 0,
                            readOnly: true,
                            columns: columns,
                            height: HEIGHT_MINIMO,
                            mergeCells: arrMergeCells,
                            cells: function (row, col, prop) {
                                var cellProperties = {};

                                for (var i in result.ListaCambios) {
                                    if ((row == result.ListaCambios[i].Row) && (col == result.ListaCambios[i].Col)) {
                                        cellProperties.renderer = cambiosCellRenderer;
                                    }
                                }

                                return cellProperties;
                            }
                        });
                    }

                    //envios
                    if (idEnvio == 0) {
                        HTML_ENVIO = model.HtmlListaEnvio;
                    }

                } else {
                    alert("Ha ocurrido un error: " + model.Mensaje);
                }
            },
            error: function (req, status, error) {
                mensajeOperacion(error);
                validaErrorOperation(req.status);
            }
        });
    } else {
        alert("Error:" + resultado);
    }
}

function exportarReporte() {
    var resultado = validarFechas();
    if (resultado === "") {
        var opcion = parseInt($("#hdOfercodi").val()) > 0 ? 2 : 1;
        $.ajax({
            type: 'POST',
            url: controlador + "ExportarReporte",
            data: {
                empresacodi: $("#empresa").val(),
                tipooferta: $("#modulo").val(),
                oferfechaenvio: $('#dte-subasta-fecha').val(),
                oferfechaenviofin: $('#dte-subasta-fechaFin').val(),
                usercode: $('#username').val(),
                opcion: opcion,
                urs: $('#urs').val(),
                ofercodi: $("#hdOfercodi").val()
            },
            dataType: 'json',
            cache: false,
            success: function (model) {
                if (model.Resultado != -1) {
                    location.href = controlador + "DescargarReporte";
                } else {
                    alert("Ha ocurrido un error: " + model.Mensaje);
                }
            },
            error: function (err) {
                alert("Ha ocurrido un error.");
            }
        });
    } else {
        alert("Error:" + resultado);
    }
}

function verEnvios() {
    $('#ele-popup-content').html('');

    var htmlE = '<div class="title_tabla_pop_up">Listado de Envíos</div>';
    htmlE += '<div class="content-hijo content-frame"><div class="field-group"><div class="field-wrap" id="listado_envio">' + HTML_ENVIO + '</div></div></div>';
    $('#ele-popup-content').html(htmlE);

    $('.codenvio').click(function () {
        var ofercodi = $(this).attr('data-ofercodi');
        formExcel(ofercodi);
        $('#ele-popup').bPopup().close({
            autoClose: false
        });
    });

    $('#ele-popup').bPopup({ modalClose: false, escClose: false });

    setTimeout(function () {
        $('#tabla_envios').dataTable({
            "scrollY": 330,
            "scrollX": true,
            "sDom": 't',
            "ordering": false,
            "bPaginate": false,
            "iDisplayLength": -1
        });
    }, 200);

}

function listarUsuarios(idempresa) {
    $('#username > option').remove();
    $.ajax({
        type: "POST",
        url: controlador + 'ListarUsuarios',
        data: { tipo: idempresa },
        success: function (resultado) {
            $('#username').append($('<option>', { value: -1, text: "(TODOS)" }));
            for (i = 0, l = resultado.length; i < l; i++) {
                $('#username').append($('<option>', { value: resultado[i].Usercode, text: resultado[i].Username }));
            }

            listarUrs(-1);
        },
        error: function (req, status, error) {
            mensajeOperacion(error);
        }
    });
}

function listarUrs(usuariocodi) {
    $('#urs > option').remove();
    $.ajax({
        type: "POST",
        url: controlador + 'ListarUrs',
        data: { tipo: usuariocodi },
        success: function (resultado) {

            for (i = 0, l = resultado.length; i < l; i++) {

                $('#urs').append($('<option>', { value: resultado[i].Urscodi, text: resultado[i].Ursnomb }));
            }
            $('#urs').append($('#urs').multipleSelect({ width: '150px', }));
            $('#urs').multipleSelect('checkAll');
        },
        error: function (req, status, error) {
            mensajeOperacion(error);
        }
    });
}

function validarFechas() {
    var resultado = "";
    if ($('#modulo').val() == 1) {
        var fini = $('#dte-subasta-fecha').val();
        var ffin = $('#dte-subasta-fechaFin').val();

        var arrFechaIni = fini.split("/");
        var arrFechaFin = ffin.split("/");

        var fecha = new Date(arrFechaIni[2], arrFechaIni[1] - 1, arrFechaIni[0]);
        var fechaFin = new Date(arrFechaFin[2], arrFechaFin[1] - 1, arrFechaFin[0]);
        if (fecha > fechaFin) {
            resultado = "La fecha inicial no puede ser mayor que la fecha final’";
        }
    }
    return resultado;
};

function cambiosCellRenderer(instance, td, row, col, prop, value, cellProperties) {
    Handsontable.renderers.TextRenderer.apply(this, arguments);
    td.style.fontSize = '12px';

    var textAlign = 'center';
    if (col == 4 || col == 5 || col == 8)
        textAlign = 'right';
    if (col == 1 || col == 6 || col == 7)
        textAlign = 'left';
    td.style.textAlign = textAlign;
    td.style.color = 'black';
    td.style.background = '#FFFFD7';

    $(td).html(value);
}