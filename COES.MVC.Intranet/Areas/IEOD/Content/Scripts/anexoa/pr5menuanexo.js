var controlador = siteRoot + 'IEOD/AnexoA/';

var TMREPCODI_EXCEL = 4;
var TMREPCODI_WORD = 11;
var IMG_DESCARGA_EXCEL = '<img src="' + siteRoot + 'Content/Images/ExportExcel.png" alt="" width="19" height="19" style="">';
var IMG_DESCARGA_WORD = '<img src="' + siteRoot + 'Content/Images/doc.png" alt="" width="19" height="19" style="">';

$(function () {
    cargarMenuAnexo();

    //acciones
    $('#btnPopupNuevaVersionExcel').click(function () {
        $("#txtFechaNuevoExcel").val($("#txtFecha").val());
        $('#txtMotivoNuevoExcel').val('');
        setTimeout(function () {
            $('#idPopupVersionExcel').bPopup({
                easing: 'easeOutBack',
                speed: 450,
                transition: 'slideDown',
                modalClose: false
            });
        }, 50);
    });
    $('#txtFechaNuevoExcel').Zebra_DatePicker({
        onSelect: function () {
        }
    });
    $('#btnGuardarVersionExcel').click(function () {
        versionGuardarExcel();
    });

    $('#btnPopupNuevaVersionWord').click(function () {
        ListarSelectGpsFrecuencia();
        $("#txtFechaNuevoWord").val($("#txtFecha").val());
        $('#txtMotivoNuevoWord').val('');
        setTimeout(function () {
            $('#idPopupVersionWord').bPopup({
                easing: 'easeOutBack',
                speed: 450,
                transition: 'slideDown',
                modalClose: false
            });
        }, 50);
    });
    $('#txtFechaNuevoWord').Zebra_DatePicker({
        onSelect: function () {
        }
    });
    $('#btnGuardarVersionWord').click(function () {
        versionGuardarWord();
    });

    $('#btnPopupHojaAnexoA').click(function () {
        hojaListado();
    });

    $('#btnGuardarVisibleHoja').click(function () {
        guardarVisibleHoja();
    });

    $('#btnConfigurarGPS').on('click', function () {
        configurarGPS();
    });

    $("#btnManualUsuario").click(function () {
        window.location = controlador + 'DescargarManualUsuario';
    });

    //filtros
    $('#txtFecha').Zebra_DatePicker({
        onSelect: function () {
            versionListado();
        }
    });

    $('#btnExportarA0').click(function () {
        exportarExcelAnexoA(0);
    });
    $('#btnExportarA1').click(function () {
        exportarExcelAnexoA(1);
    });
    $('#btnExportarA2').click(function () {
        exportarExcelAnexoA(2);
    });
    $('#btnExportarA3').click(function () {
        exportarExcelAnexoA(3);
    });
    $('#btnExportarA4').click(function () {
        exportarExcelAnexoA(4);
    });
    $('#btnExportarA5').click(function () {
        exportarExcelAnexoA(5);
    });
    $('#btnExportarA6').click(function () {
        exportarExcelAnexoA(6);
    });

    $('#btnIrAlListado').click(function () {
        document.location.href = siteRoot + 'IEOD/AnexoA/GenerarIEOD';
    });

    versionListado();
});


function ListarSelectGpsFrecuencia() {
    $.ajax({
        type: 'POST',
        url: controlador + 'ListarGpsFrecuencia',
        data: {            
        },
        dataType: 'json',
        success: function (evt) {
            $('#ListaGpsFrecuencia').html(HtmResultListaGps(evt.ListaGPS));
            $('#cbGps').val(1);
        },
        error: function (err) {
            alert("Error en reporte");;
        }
    });
}

function HtmResultListaGps(lista) {
    var strHtml = '';
    strHtml += `
        <select class="form-select" id="cbGps" name="cbGps">
    `;
    
    if (lista.length > 0)
    {
        for (key in lista) {
            var item = lista[key];
            strHtml += `<option value="${item.Gpscodi}">${item.NombreYEstado}</option>
            `;
        }
    }
    else
    {
        strHtml += `
            <option value="0">No existen registros..</option>
        `;
    }
    strHtml += ` </select>
    `;
    return strHtml;
}

function exportarExcelAnexoA(num) {
    fec1 = $("#txtFecha").val();
    $.ajax({
        type: 'POST',
        url: controlador + 'GenerarReporteAnexoAByNumero',
        data: {
            numReporte: num,
            fecha: fec1,
        },
        dataType: 'json',
        success: function (result) {
            switch (result.Total) {
                case 1: window.location = controlador + "ExportarReporteXls?nameFile=" + result.Resultado; break;// si hay elementos
                case 2: alert("No existen registros !"); break;// sino hay elementos
                case -1: alert("Error en reporte result"); break;// Error en C#
            }
        },
        error: function (err) {
            alert("Error en reporte");;
        }
    });
};

/**
 * Menu de numerales
 * */
function cargarMenuAnexo() {
    $.ajax({
        type: 'POST',
        url: controlador + 'CargarMenu',
        data: {},
        dataType: 'json',
        success: function (e) {
            $('#MenuID').html(e.Menu);
            $('#myTable').DataTable({
                "paging": false,
                "lengthChange": false,
                "pagingType": false,
                "ordering": false,
                "info": false
            });

            $("#contenedorMenuPR5").show();
            $('#MenuID').show();
        },
        error: function (err) {
            alert("Ha ocurrido un error");
        }
    });
}

function fnClick(x) {
    var fecha = $("#txtFecha").val().replaceAll("/", '-');
    document.location.href = controlador + x + "?fecha=" + fecha;
}

/**
 * Versiones
 * */
function versionGuardarExcel() {
    $('#txtMotivoNuevoExcel').val($('#txtMotivoNuevoExcel').val().trim());

    //datos
    var fechaPeriodo = $('#txtFechaNuevoExcel').val();
    var motivo = $('#txtMotivoNuevoExcel').val();
    var msj = validaTexto(motivo, "Motivo");

    if (msj == "") {
        $.ajax({
            type: 'POST',
            async: true,
            url: controlador + 'GuardarNuevaVersion',
            data: {
                motivo: motivo,
                fechaPeriodo: fechaPeriodo,
                tmrepcodi: TMREPCODI_EXCEL
            },
            success: function (result) {
                if (result.Resultado == "-1") {
                    alert("Ocurrio un error en la generación de la versión");
                } else {
                    $("#txtFecha").val($("#txtFechaNuevoExcel").val());

                    versionListado();
                    $('#idPopupVersionExcel').bPopup().close();
                }
            },
            error: function (err) {
                alert("Ha ocurrido un error");
            }
        });
    } else {
        alert(msj);
    }
}

function versionGuardarWord() {
    $('#txtMotivoNuevoWord').val($('#txtMotivoNuevoWord').val().trim());

    //datos
    var fechaPeriodo = $('#txtFechaNuevoWord').val();
    var motivo = $('#txtMotivoNuevoWord').val();
    var gpsFrec = $('#cbGps').val();
    var leyenda = $('#cbLeyenda').is(':checked') ? true : false;

    var msj = validaTexto(motivo, "Motivo");

    if (msj == "") {
        $.ajax({
            type: 'POST',
            async: true,
            url: controlador + 'GuardarNuevaVersion',
            data: {
                motivo: motivo,
                fechaPeriodo: fechaPeriodo,
                tmrepcodi: TMREPCODI_WORD,
                gpscodi: gpsFrec,
                incluirLeyendaEcuador: leyenda,

            },
            success: function (result) {
                if (result.Resultado == "-1") {
                    alert("Ocurrio un error en la generación de la versión");
                } else {
                    $("#txtFecha").val($("#txtFechaNuevoWord").val());
                    versionListado();

                    $('#idPopupVersionWord').bPopup().close();
                }
            },
            error: function (err) {
                alert("Ha ocurrido un error");
            }
        });
    } else {
        alert(msj);
    }
}

function versionGuardarWord2() {


    //datos
    var fechaPeriodo = $('#txtFechaNuevoWord').val();



    $.ajax({
        type: 'POST',
        async: true,
        url: controlador + 'GeneraWordAnexoA',
        data: {
            fechaPeriodo: fechaPeriodo
        },
        success: function (result) {
            if (result.Resultado == "-1") {
                alert("Ocurrio un error en la generación de la versión");
            } else {
                //versionListado();

                $('#idPopupVersionWord').bPopup().close();
            }
        },
        error: function (err) {
            alert("Ha ocurrido un error");
        }
    });

}

function versionListado() {
    $("#reporte_excel_anexoa").html('');
    $("#mensaje").hide();

    var fechaPeriodo = $('#txtFecha').val();

    $.ajax({
        type: 'POST',
        url: controlador + 'ListadoVersion',
        dataType: 'json',
        data: {
            fechaPeriodo: fechaPeriodo
        },
        cache: false,
        success: function (data) {
            if (data.Resultado != "-1") {
                var htmlExcel = dibujarTablaListadoVersion(data.ListaVersion, true);
                $("#reporte_excel_anexoa").html(htmlExcel);

                var htmlWord = dibujarTablaListadoVersion(data.ListaVersion2, false);
                $("#reporte_word_anexoa").html(htmlWord);

                $('#tblListadoExcel').dataTable({
                    "destroy": "true",
                    "scrollX": true,
                    "sDom": 't',
                    "ordering": false,
                    "bPaginate": false,
                    "iDisplayLength": -1,
                    "language": {
                        "emptyTable": "¡No existen versiones del Excel!"
                    },
                });

                $('#tblListadoWord').dataTable({
                    "destroy": "true",
                    "scrollX": true,
                    "sDom": 't',
                    "ordering": false,
                    "bPaginate": false,
                    "iDisplayLength": -1,
                    "language": {
                        "emptyTable": "¡No existen versiones del Word!"
                    },
                });
            } else {
                alert("Ha ocurrido un error: " + data.Mensaje);
            }
        },
        error: function (err) {
            alert('Ha ocurrido un error.');
        }
    });
}

function dibujarTablaListadoVersion(lista, esExcel) {
    var idTabla = esExcel ? 'tblListadoExcel' : 'tblListadoWord';

    var cadena = '';
    cadena += `
    <table border="0" class="pretty tabla-icono dataTable" cellspacing="0" width="100%" id="${idTabla}">
        <thead>
            <tr>
                <th style='width: 70px'>Descargar</th>
                <th style='width: 40px'>N° Versión</th>
                <th style='width: 200px'>Fecha</th>
                <th style='width: 400px'>Motivo</th>
                <th style='width: 100px'>Usuario creación</th>
                <th style='width: 100px'>Fecha creación</th>
            </tr>
        </thead>
        <tbody>
    `;

    for (key in lista) {
        var item = lista[key];

        var htmlDescarga = '';
        if (esExcel) {
            htmlDescarga = `
                    <a title="Descargar Excel" href="JavaScript:descargarExcel(${item.Verscodi});">${IMG_DESCARGA_EXCEL} </a>
                    <a title="Vista previa Excel" href="JavaScript:vistaPreviaExcel(${item.Verscodi});" style='font-size: 28px;' > 👁 </a>
            `;
        } else {
            htmlDescarga = `
                    <a title="Descargar Word" href="JavaScript:descargarWord(${item.Verscodi});">${IMG_DESCARGA_WORD} </a>
                    <a title="Vista previa Excel" href="JavaScript:vistaPreviaWord(${item.Verscodi});" style='font-size: 28px;' > 👁 </a>
            `;
        }

        cadena += `
            <tr>
                <td style="height: 24px">
                    ${htmlDescarga}
                </td>
                <td style="height: 24px;text-align: center; ">${item.VerscorrelativoDesc}</td>
                <td style="height: 24px;text-align: center; ">${item.VersfechaperiodoDesc}</td>
                <td style="height: 24px;text-align: center; ">${item.Versmotivo}</td>
                <td style="height: 24px">${item.Versusucreacion}</td>
                <td style="height: 24px">${item.VersfeccreacionDesc}</td>
            </tr>
        `;
    }
    cadena += "</tbody></table>";

    return cadena;
}

/**
 * Hojas de excel
 * */

function hojaListado() {
    $("#div_hoja_excel").html('');

    $.ajax({
        type: 'POST',
        url: controlador + 'ListarHojaExcel',
        dataType: 'json',
        data: {
        },
        cache: false,
        success: function (data) {
            if (data.Resultado != "-1") {
                var htmlExcel = dibujarTablaListadoHoja(data.ListaHojaExcel);
                $("#div_hoja_excel").html(htmlExcel);

                //primero generar datatable
                setTimeout(function () {
                    $('#tbl_hoja').dataTable({
                        "destroy": "true",
                        "scrollX": true,
                        scrollY: 450,
                        "sDom": 'ft',
                        "ordering": false,
                        "bPaginate": false,
                        "iDisplayLength": -1,
                        "language": {
                            "emptyTable": "¡No existen hojas!"
                        },
                    });
                }, 150);

                //luego abrir popup
                setTimeout(function () {
                    $('#idPopupHojaExcel').bPopup({
                        easing: 'easeOutBack',
                        speed: 450,
                        transition: 'slideDown',
                        modalClose: false
                    });
                }, 50);

            } else {
                alert("Ha ocurrido un error: " + data.Mensaje);
            }
        },
        error: function (err) {
            alert('Ha ocurrido un error.');
        }
    });
}

function dibujarTablaListadoHoja(lista) {

    var cadena = '';
    cadena += `
    <table border="0" class="pretty tabla-icono dataTable" cellspacing="0" width="100%" id="tbl_hoja">
        <thead>
            <tr>
                <th style='width: 40px'>Mostrar</th>
                <th style='width: 200px'>Hoja</th>
                <th style='width: 900px'>Numeral</th>
            </tr>
        </thead>
        <tbody>
    `;

    for (key in lista) {
        var item = lista[key];
        var schecked = item.Mrephvisible == 1 ? " checked " : "";

        cadena += `
            <tr>
                <td style="height: 24px;text-align: center; ">
                    <input type='checkbox' id='hoja_${item.Mrephcodi}' name='hoja_excel' ${schecked} value='${item.Mrephcodi}' />
                </td>
                <td style="height: 24px;text-align: center; ">${item.Mrephnombre}</td>
                <td style="height: 24px;text-align: left; ">${item.Repdescripcion}</td>
            </tr>
        `;
    }
    cadena += "</tbody></table>";

    return cadena;
}

function guardarVisibleHoja() {
    $.ajax({
        type: "POST",
        url: controlador + "GuardarVisibleHojaExcel",
        traditional: true,
        data: {
            mrephcodisVisible: _listarSeleccion(),
        },
        success: function (model) {
            if (model.Resultado != "-1") {
                alert("Se ha actualizado correctamente.");
                $('#idPopupHojaExcel').bPopup().close();
            } else {
                alert(model.Mensaje);
            }
        },
        error: function (err) {
            alert("Lo sentimos, ha ocurrido un error inesperado");
        }
    });

}

function _listarSeleccion() {
    var selected = [];
    $('input[type=checkbox][name=hoja_excel]').each(function () {
        if ($(this).is(":checked")) {
            selected.push($(this).attr('value'));
        }
    });

    return selected.join(",");
}

/**
 * Descarga de archivo y vista previa
 * */
function descargarExcel(verscodi) {
    var paramList = [
        { tipo: 'input', nombre: 'verscodi', value: verscodi },
    ];
    var form = createFormArchivo(controlador + 'DescargarArchivoExcelXVersion', paramList);
    document.body.appendChild(form);
    form.submit();

    return true;
}

function descargarWord(verscodi) {
    var paramList = [
        { tipo: 'input', nombre: 'verscodi', value: verscodi },
    ];
    var form = createFormArchivo(controlador + 'DescargarArchivoWordXVersion', paramList);
    document.body.appendChild(form);
    form.submit();

    return true;
}

function vistaPreviaExcel(verscodi) {
    $("#vistaprevia").hide();

    $.ajax({
        type: 'POST',
        url: controlador + 'VistaPreviaArchivoExcel',
        data: {
            verscodi: verscodi
        },
        success: function (model) {
            if (model.Resultado != "") {
                var rutaCompleta = window.location.href;
                var rutaInicial = rutaCompleta.split("IEOD"); //Obtener url de intranet
                var urlPrincipal = rutaInicial[0];

                var urlFrame = urlPrincipal + model.Resultado;
                var urlFinal = "https://view.officeapps.live.com/op/embed.aspx?src=" + urlFrame;

                $('#idPopupVistaPrevia').bPopup({
                    easing: 'easeOutBack',
                    speed: 450,
                    transition: 'slideDown',
                    modalClose: false,
                    follow: [false, false] //x, y
                });

                $("#vistaprevia").show();
                $('#vistaprevia').html('');
                $('#vistaprevia').attr("src", urlFinal);

            } else {
                alert("la vista previa solo permite archivos word, excel y pdf.");
            }
        },
        error: function (err) {
            document.getElementById('filelist').innerHTML = `<div class="action-alert">Ha ocurrido un error.</div>`;
        }
    });

    return true;
}

function vistaPreviaWord(verscodi) {
    $("#vistaprevia").hide();

    $.ajax({
        type: 'POST',
        url: controlador + 'VistaPreviaArchivoWord',
        data: {
            verscodi: verscodi
        },
        success: function (model) {
            if (model.Resultado != "") {
                var rutaCompleta = window.location.href;
                var rutaInicial = rutaCompleta.split("IEOD"); //Obtener url de intranet
                var urlPrincipal = rutaInicial[0];

                var urlFrame = urlPrincipal + model.Resultado;
                var urlFinal = "https://view.officeapps.live.com/op/embed.aspx?src=" + urlFrame;

                $('#idPopupVistaPrevia').bPopup({
                    easing: 'easeOutBack',
                    speed: 450,
                    transition: 'slideDown',
                    modalClose: false
                });

                $("#vistaprevia").show();
                $('#vistaprevia').html('');
                $('#vistaprevia').attr("src", urlFinal);

            } else {
                alert("la vista previa solo permite archivos word, excel y pdf.");
            }
        },
        error: function (err) {
            document.getElementById('filelist').innerHTML = `<div class="action-alert">Ha ocurrido un error.</div>`;
        }
    });

    return true;
}

/**GPS */
function configurarGPS() {
    $.ajax({
        type: 'POST',
        url: controlador + 'ConfigurarGPS',
        success: function (evt) {
            $('#contenidoGPS').html(evt);

            //primero generar datatable
            setTimeout(function () {
                $('#tablaGPS').dataTable({
                    "destroy": "true",
                    "scrollX": true,
                    scrollY: 450,
                    "sDom": 't',
                    "ordering": false,
                    "bPaginate": false,
                    "iDisplayLength": -1,
                    "language": {
                        "emptyTable": "¡No existen gps!"
                    },
                });
            }, 150);

            //luego abrir popup
            setTimeout(function () {
                $('#popupGPS').bPopup({
                    autoClose: false
                });
            }, 50);

            $('#cbSelectAll').click(function (e) {
                var table = $(e.target).closest('table');
                $('td input:checkbox', table).prop('checked', this.checked);
            });

            $('#btnGrabarGPS').on('click', function () {
                grabarConfiguracionGPS();
            });

            $('#btnCancelarGPS').on('click', function () {
                $('#popupGPS').bPopup().close();
            });
        },
        error: function (err) {
            alert("Ha ocurrido un error");
        }
    });
}

function grabarConfiguracionGPS() {
    var gps = "";
    countgps = 0;
    $('#tablaGPS tbody input').each(function () {
        var ind = $(this).is(':checked') ? "S" : "N";
        var item = $(this).val() + "@" + ind;
        gps = gps + item + "#";
        if ($(this).is(':checked')) {
            countgps++;
        }
    });

    if (countgps > 0) {
        $.ajax({
            type: 'POST',
            url: controlador + 'GrabarConfiguracionGPS',
            data: {
                id: gps
            },
            dataType: 'json',
            success: function (result) {
                if (result == 1) {
                    $('#popupGPS').bPopup().close();
                }
                else {
                    alert("Ha ocurrido un error");
                }
            },
            error: function () {
                alert("Ha ocurrido un error");
            }
        });
    }
    else {
        alert("Seleccione al menos una GPS");
    }
}

/**
 * Util
 */
function createFormArchivo(path, params, method = 'post') {
    var form = document.createElement('form');
    form.method = method;
    form.action = path;

    $.each(params, function (index, obj) {
        var hiddenInput = document.createElement(obj.tipo);
        hiddenInput.type = 'hidden';
        hiddenInput.name = obj.nombre;
        hiddenInput.value = obj.value;
        form.appendChild(hiddenInput);
    });
    return form;
}

function validaTexto(texto, propiedad) {
    var msj = "";

    if (texto.trim() == "") {
        msj += "Debe escribir un motivo para poder hacer la generación";
    } else {
        if (texto.length > 250) {
            msj += "El campo '" + propiedad + "' no debe exceder los 250 caracteres.";
        }
    }

    return msj;
}