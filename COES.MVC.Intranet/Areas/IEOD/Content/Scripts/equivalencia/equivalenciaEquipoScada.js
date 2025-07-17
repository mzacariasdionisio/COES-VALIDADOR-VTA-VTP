var controlador = siteRoot + 'IEOD/Configuracion/';
var ancho;
var DATA_EMPRESA = null;
var DATA_TIPO_FAMILIA = null;
var LISTA_RELACION = [];
var APP_MOSTRAR_MODAL_EQUIPO_YA_CARGADO = 0;

$(function () {
    ancho = $("#mainLayout").width() > 800 ? $("#mainLayout").width() : 800;

    $('#btnRegresar').click(function () {
        document.location.href = siteRoot + 'Migraciones/DigSilent/ProcesoDigsilent';
    });

    $('#btnConsultar').on('click', function () {
        listarEquivalencia();
    });

    $('#btnExpotar').on('click', function () {
        exportarEquivalencia();
    });

    $('#btnNuevaRelacion').on('click', function () {
        nuevaEquivalencia();
    });

    $('#cbEmpresa').multipleSelect({
        width: '220px',
        filter: true,
        single: true,
        onClose: function () {
        }
    });
    $('#cbFamilia').multipleSelect({
        width: '220px',
        filter: true,
        single: true,
        onClose: function () {
        }
    });

    $('#cbMedida').on('change', function () {
        listarEquivalencia();
    });

    $("#cbEmpresa").multipleSelect("setSelects", [-1]);
    $("#cbFamilia").multipleSelect("setSelects", [-1]);

    listarEquivalencia();
});

///////////////////////////
/// Consulta
///////////////////////////
function listarEquivalencia() {
    var areacode = $('#cbArea').val();
    if (areacode == "") areacode = "-1";

    var empresa = $('#cbEmpresa').val();
    if (empresa == "") empresa = "-1";

    var familia = $('#cbFamilia').val();
    if (familia == "") familia = "-1";

    var medida = $("#cbMedida").val();
    if (medida == "") medida = "-1";

    $.ajax({
        type: 'POST',
        url: controlador + 'ListarReporteEquivalenciaEquipoScada',
        data: {
            areacode: areacode,
            idEmpresa: empresa,
            idFamilia: familia,
            medida: medida
        },
        dataType: 'json',
        success: function (result) {
            $('#listado').html(result);
            var anchoReporte = $('#reporte').width();
            $("#listado").css("width", (anchoReporte > ancho ? ancho : anchoReporte) + "px");

            $('#reporte').dataTable({
                "scrollY": 430,
                "scrollX": true,
                "sDom": 'ft',
                "ordering": true,
                "iDisplayLength": -1
            });
        },
        error: function (err) {
            alert('Ha ocurrido un error.');
        }
    });
}

function exportarEquivalencia() {
    var areacode = $('#cbArea').val();
    if (areacode == "") areacode = "-1";

    var empresa = $('#cbEmpresa').val();
    if (empresa == "") empresa = "-1";

    var familia = $('#cbFamilia').val();
    if (familia == "") familia = "-1";

    var medida = $("#cbMedida").val();
    if (medida == "") medida = "-1";

    $.ajax({
        type: 'POST',
        url: controlador + 'GeneraReporteExcelEquivalenciaEquipoScada',
        data: {
            idEmpresa: empresa,
            idFamilia: familia,
            medida: medida,
            areacode: areacode
        },
        dataType: 'json',
        success: function (result) {
            if (result == 1) {//
                window.location = controlador + "ExportarReporteEquivalencia";
            }
            if (result == -1) {
                alert("Error en reporte result")
            }
            if (result == 2) {// Si no existen registros
                alert("No existen registros !");
            }
        },
        error: function (err) {
            alert("Error en reporte");;
        }
    });
}

///////////////////////////
/// Nuevo
///////////////////////////
function nuevaEquivalencia() {
    $.ajax({
        type: 'POST',
        url: controlador + "NuevoEquivalenciaEquipoScada",
        data: {
        },
        success: function (evt) {
            $('#nuevaEquivalencia').html(evt);

            setTimeout(function () {
                $('#popupNuevaEquivalencia').bPopup({
                    easing: 'easeOutBack',
                    speed: 450,
                    transition: 'slideDown'
                });
            }, 50);
        },
        error: function (err) {
            alert('Ha ocurrido un error.');
        }
    });
}

function registrarEquivalencia() {

    if (validarNuevoListaEquivalencia() && confirm('¿Está seguro que desea guardar la equivalencia?')) {
        $.ajax({
            type: 'POST',
            url: controlador + 'RegistrarEquivalenciaEquipoScada',
            dataType: 'json',
            data: {
                dataLista: JSON.stringify(generarListaData())
            },
            cache: false,
            success: function (resultado) {
                if (resultado == 1) {
                    $('#popupNuevaEquivalencia').bPopup().close();
                    alert("Se registró correctamente");
                    listarEquivalencia();
                } else {
                    if (resultado != -1) {
                        alert(resultado);
                    } else {
                        alert('Ha ocurrido un error.');
                    }
                }
            },
            error: function (err) {
                alert('Ha ocurrido un error.');
            }
        });
    }
}

function validarNuevoEquivalencia() {
    var equipo = parseInt($("#cbEquipoNuevo").val()) || 0;
    var unidad = parseInt($("#cbMedidaNuevo").val()) || 0;
    var canal = parseInt($("#cbTrCanalNuevo").val()) || 0;
    var msj = "";

    msj += unidad <= 0 ? "Debe seleccionar Unidad de medida" + "\n" : "";
    msj += equipo <= 0 ? "Debe seleccionar Equipo" + "\n" : "";
    msj += canal <= 0 ? "Debe seleccionar Canal Scada" + "\n" : "";

    if (msj != "") {
        alert(msj);
        return false;
    }
    return true;
}

function generarListaData() {
    var lista = [];

    for (var i = 0; i < LISTA_RELACION.length; i++) {
        var obj = {};
        obj.Equicodi = LISTA_RELACION[i].Equicodi;
        obj.Canalcodi = LISTA_RELACION[i].Canalcodi;
        obj.Tipoinfocodi = LISTA_RELACION[i].Tipoinfocodi;
        obj.Ecanfactor = LISTA_RELACION[i].Ecanfactor;
        obj.Areacode = LISTA_RELACION[i].Areacode;

        lista.push(obj);
    }

    return lista;
}

function validarNuevoListaEquivalencia() {
    var msj = "";

    msj += LISTA_RELACION.length <= 0 ? "Debe crear 1 0 más Equivalencias" + "\n" : "";

    if (msj != "") {
        alert(msj);
        return false;
    }
    return true;
}

function inicializarPopupNuevaEquivalencia() {
    APP_MOSTRAR_MODAL_EQUIPO_YA_CARGADO = 0;
    $('#busquedaEquipo').html('');

    /////////////////////    Equipos
    //
    $('#btnBuscarEquipoRelacion').unbind();
    $('#btnBuscarEquipoRelacion').click(function () {
        visualizarBuscarEquipo();
    });

    /////////////////////    Canal
    //empresa
    $("#cbTrEmpresaNuevo").multipleSelect({
        width: '220px',
        filter: true,
        single: true,
        onClose: function () {
            cargarZona();
        }
    });
    $("#cbTrZonaNuevo").multipleSelect({
        width: '220px',
        filter: true,
        single: true,
        onClose: function () {
            cargarCanal();
        }
    });
    $("#cbTrCanalNuevo").multipleSelect({
        width: '500px',
        filter: true,
        single: true,
        onClose: function () {
        }
    });

    //Medida
    $('#cbMedidaNuevo').on('change', function () {
        /*$("#iEquipoNuevo").val('');
        $("#cbEquipoNuevo").val(-1);*/
        $("#icbTrCanalNuevo").val('');
        $("#cbTrCanalNuevo").val(-1);
        cargarCanal();
    });

    //
    LISTA_RELACION = [];
    $('#btnAgregarEquivalencia').unbind();
    $('#btnAgregarEquivalencia').click(function () {
        agregarEquivalencia();
    });

    cargarTablaListaRelacion(LISTA_RELACION, 'tb_relacion', 'divTablaRelacion');

    //
    $('#btnGrabarEquivalencia').unbind();
    $('#btnGrabarEquivalencia').click(function () {
        registrarEquivalencia();
    });
    $('#btnCancelarEquivalencia').unbind();
    $("#btnCancelarEquivalencia").click(function () {
        cancelarEquivalencia();
    });


}

///////////////////////////
/// Búsqueda Equipo
///////////////////////////
function visualizarBuscarEquipo() {

    if (APP_MOSTRAR_MODAL_EQUIPO_YA_CARGADO == 0) {
        cargarBusquedaEquipo(APP_MOSTRAR_MODAL_EQUIPO_YA_CARGADO);
    } else {
        openBusquedaEquipo();
    }

    APP_MOSTRAR_MODAL_EQUIPO_YA_CARGADO++;
}

function cargarBusquedaEquipo(flag) {
    $.ajax({
        type: "POST",
        url: controlador + "BusquedaEquipo",
        data: {
            filtroFamilia: -1
        },
        global: false,
        success: function (evt) {
            $('#busquedaEquipo').html(evt);
            openBusquedaEquipo();
        },
        error: function (req, status, error) {
            mostrarError();
        }
    });
}

function openBusquedaEquipo() {
    $('#busquedaEquipo').bPopup({
        easing: 'easeOutBack',
        speed: 450,
        transition: 'slideDown'
    });
    $('#txtFiltro').focus();
}

function seleccionarEquipo(equicodi, equinomb, areanomb, Emprnomb, Famabrev, emprcodi) {
    $("#cbEquipoNuevo").val('');
    $("#cbEquipoNuevoNombre").val('');
    $("#cbEquipoNuevoUbicacion").val('');
    $("#cbEquipoNuevoEmpresa").val('');
    $("#cbEquipoNuevoFamilia").val('');

    $("#iEmpresaNuevo").val(Emprnomb);
    $("#iUbicacionNuevo").val(areanomb);
    $("#iFamiliaNuevo").val(Famabrev);
    $("#iEquipoNuevo").val(equinomb);

    $("#cbEquipoNuevo").val(equicodi);
    $("#cbEquipoNuevoNombre").val(equinomb);
    $("#cbEquipoNuevoUbicacion").val(areanomb);
    $("#cbEquipoNuevoEmpresa").val(Emprnomb);
    $("#cbEquipoNuevoFamilia").val(Famabrev);

    $('#busquedaEquipo').bPopup().close();
}

///////////////////////////
/// Ver
///////////////////////////
function verEquivalencia(canal, equicodi, tipoinfocodi, areacode) {
    $.ajax({
        type: 'POST',
        url: controlador + "VerEquivalenciaEquipoScada",
        data: {
            canalcodi: canal,
            equicodi: equicodi,
            tipoinfocodi: tipoinfocodi,
            areacode: areacode
        },
        success: function (evt) {
            $('#verEquivalencia').html(evt);

            setTimeout(function () {
                $('#popupVerEquivalencia').bPopup({
                    easing: 'easeOutBack',
                    speed: 450,
                    transition: 'slideDown'
                });
            }, 50);
        },
        error: function (err) {
            alert('Ha ocurrido un error.');
        }
    });
}

///////////////////////////
/// Editar
///////////////////////////
function editarEquivalencia(canal, equicodi, tipoinfocodi, areacode) {
    $.ajax({
        type: 'POST',
        url: controlador + "EditarEquivalenciaEquipoScada",
        data: {
            canalcodi: canal,
            equicodi: equicodi,
            tipoinfocodi: tipoinfocodi,
            areacode: areacode
        },
        success: function (evt) {
            $('#editarEquivalencia').html(evt);

            setTimeout(function () {
                $('#popupEditarEquivalencia').bPopup({
                    easing: 'easeOutBack',
                    speed: 450,
                    transition: 'slideDown'
                });
            }, 50);
        },
        error: function (err) {
            alert('Ha ocurrido un error.');
        }
    });
}

function actualizarEquivalencia() {
    var canalcodi = $("#canalcodiEdit").val();
    var equicodi = $("#equicodiEdit").val();
    var tipoinfocodi = $("#tipoinfocodiEdit").val();
    var estado = $("#cbEstadoEdit").val();
    var areacode = -1;
    var check = $('#chkValorInversoEdit').is(':checked') ? -1 : 1;

    if (confirm('¿Está seguro que desea actualizar la equivalencia?')) {
        $.ajax({
            type: 'POST',
            url: controlador + 'ActualizarEquivalenciaEquipoScada',
            dataType: 'json',
            data: {
                canalcodi: canalcodi,
                equicodi: equicodi,
                tipoinfocodi: tipoinfocodi,
                areacode: areacode,
                estado: estado,
                check: check
            },
            cache: false,
            success: function (resultado) {
                if (resultado == 1) {
                    $('#popupEditarEquivalencia').bPopup().close();
                    alert("Se actualizar correctamente");
                    listarEquivalencia();
                } else {
                    if (resultado != -1) {
                        alert(resultado);
                    } else {
                        alert('Ha ocurrido un error.');
                    }
                }
            },
            error: function (err) {
                alert('Ha ocurrido un error.');
            }
        });
    }
}

///////////////////////////
/// Eliminar
///////////////////////////
function eliminarEquivalencia(canal, equicodi, tipoinfocodi, areacode) {

    if (confirm('¿Está seguro que desea eliminar la equivalencia?')) {
        $.ajax({
            type: 'POST',
            url: controlador + 'EliminarEquivalenciaEquipoScada',
            dataType: 'json',
            data: {
                canalcodi: canal,
                equicodi: equicodi,
                tipoinfocodi: tipoinfocodi,
                areacode: areacode
            },
            cache: false,
            success: function (resultado) {
                if (resultado == 1) {
                    alert("Se eliminó correctamente");
                    listarEquivalencia();
                } else {
                    if (resultado != -1) {
                        alert(resultado);
                    } else {
                        alert('Ha ocurrido un error.');
                    }
                }
            },
            error: function (err) {
                alert('Ha ocurrido un error.');
            }
        });
    }
}

///////////////////////////
/// Util
///////////////////////////
function cancelarEquivalencia() {
    $('#popupEditarEquivalencia').bPopup().close();
    $('#popupNuevaEquivalencia').bPopup().close();
    $('#nuevaEquivalencia').html("");
}

function cargarEquipo() {
    var empresa = parseInt($('#cbEmpresaNuevo').val()) || 0;
    var famcodi = parseInt($('#cbFamiliaNuevo').val()) || 0;

    $.ajax({
        type: 'POST',
        url: controlador + 'ListarEquipo',
        data: {
            idEmpresa: empresa,
            famcodi: famcodi
        },
        dataType: 'json',
        success: function (result) {
            var dataJson3 = result;
            $('#iEquipoNuevo').autocomplete('destroy').removeClass('xdsoft_input');
            $('#iEquipoNuevo').autocomplete({
                source: [{
                    data: dataJson3,
                    getTitle: function (item) {
                        return item.Equinomb
                    },
                    getValue: function (item) {
                        return item.Equinomb
                    },
                }],
                valueKey: 'Equinomb',
            }).on('selected.xdsoft', function (e, obj) {
                $("#cbEquipoNuevo").val(obj.Equicodi);
                $("#cbEquipoNuevoNombre").val(obj.Equinomb);
                $("#cbEquipoNuevoEmpresa").val(obj.Emprnomb);
                $("#cbEquipoNuevoFamilia").val(obj.Famnomb);
            }).on('open.xdsoft', function (e) {
                $("#cbEquipoNuevo").val('-1');
            });
        },
        error: function (err) {
            alert('Ha ocurrido un error.');
        }
    });
}

function cargarCentrales() {
    var empresa = $('#cbEmpresa').val();
    if (empresa == "") empresa = "-1";

    $("#cbCentral").empty();
    $('#cbCentral').append($('<option></option>').val("").html("-TODOS-"));
    $.ajax({
        type: 'POST',
        url: controlador + 'ObtenerCentrales',
        data: {
            idEmpresa: empresa
        },
        dataType: 'json',
        success: function (result) {
            for (var item in result) {
                $('#cbCentral').append($('<option></option>').val(result[item].Equicodi).html(result[item].Equinomb));
            }
            listarEquivalencia();
        },
        error: function (err) {
            alert('Ha ocurrido un error.');
        }
    });
}

function cargarZona() {
    $("#icbTrZonaNuevo").val('');
    var empresa = parseInt($('#cbTrEmpresaNuevo').val()) || 0;


    $.ajax({
        type: 'POST',
        url: controlador + 'ListarZona',
        data: {
            idTrEmpresa: empresa
        },
        dataType: 'json',
        success: function (result) {
            var dataJson3 = result;

            var htmlOption = '';
            for (var i = 0; i < dataJson3.length; i++) {
                var item = dataJson3[i];
                htmlOption += `<option value="${item.Zonacodi}">${item.Zonanomb}</option>`;
            }

            var htmlZona = ` <select id="cbTrZonaNuevo">
                            ${htmlOption}
                        </select>`;

            $("#td_zona_nuevo").html(htmlZona);
            $("#cbTrZonaNuevo").multipleSelect({
                width: '220px',
                filter: true,
                single: true,
                onClose: function () {
                    cargarCanal();
                }
            });

            cargarCanal();
        },
        error: function (err) {
            alert('Ha ocurrido un error.');
        }
    });
}

function cargarCanal() {
    $("#icbTrCanalNuevo").val('');
    $("#cbTrCanalNuevo").val(-1);
    var empresa = parseInt($('#cbTrEmpresaNuevo').val()) || 0;
    var zona = parseInt($("#cbTrZonaNuevo").val()) || 0;
    var tipoinfocodi = parseInt($("#cbMedidaNuevo").val()) || 0;

    if (empresa != 0 && zona != 0 && tipoinfocodi != 0) {
        $.ajax({
            type: 'POST',
            url: controlador + 'ListarCanal',
            data: {
                idTrEmpresa: empresa,
                idTrZona: zona,
                tipoinfocodi: tipoinfocodi
            },
            dataType: 'json',
            success: function (result) {
                var dataJson3 = result;

                var htmlOption = '';
                for (var i = 0; i < dataJson3.length; i++) {
                    var item = dataJson3[i];
                    htmlOption += `<option value="${item.Canalcodi}">${item.Canalcodi} - ${item.Canalnomb}</option>`;
                }

                var htmlCanal = ` <select id="cbTrCanalNuevo">
                            ${htmlOption}
                        </select>`;

                $("#td_canal_nuevo").html(htmlCanal);
                $("#cbTrCanalNuevo").multipleSelect({
                    width: '500px',
                    filter: true,
                    single: true,
                    onClose: function () {
                    }
                });
            },
            error: function (err) {
                alert('Ha ocurrido un error.');
            }
        });
    }
}

/// Tabla Vista previa
function agregarEquivalencia() {
    var equipo = parseInt($("#cbEquipoNuevo").val()) || 0;
    var unidad = parseInt($("#cbMedidaNuevo").val()) || 0;
    var canal = parseInt($("#cbTrCanalNuevo").val()) || 0;
    var area = parseInt($("#cbAreaNuevo").val()) || 0;
    var check = $('#chkValorInverso').is(':checked') ? -1 : 1;

    if (validarNuevoEquivalencia()) {
        var obj = {};
        obj.Unidad = $("#cbMedidaNuevo option[value=" + unidad + "]").text();
        obj.Empresa = $("#cbEquipoNuevoEmpresa").val();
        obj.TipoEquipo = $("#cbEquipoNuevoFamilia").val();
        obj.Equicodi = equipo;
        obj.Canalcodi = canal;
        obj.Equinomb = $("#cbEquipoNuevoNombre").val();
        obj.Canalnomb = $("#cbTrCanalNuevo").multipleSelect('getSelects', 'text');
        obj.Tipoinfocodi = unidad;
        obj.Ecanfactor = check;
        obj.Areacode = area;
        obj.Areaabrev = $("#cbAreaNuevo option[value=" + area + "]").text();
        obj.Ubicacion = $("#iUbicacionNuevo").val();

        if (validarNoDuplicadoNuevoEquivalencia(obj)) {
            LISTA_RELACION.push(obj);

            $("#cbTrCanalNuevo").multipleSelect("setSelects", []);
        }
    }

    cargarTablaListaRelacion(LISTA_RELACION, 'tb_relacion', 'divTablaRelacion');
}

function validarNoDuplicadoNuevoEquivalencia(obj) {
    var msj = "";

    msj += buscarRelacion(obj.Equicodi, obj.Canalcodi, LISTA_RELACION) ? "Ya existe el elemento en la lista" + "\n" : "";

    if (msj != "") {
        alert(msj);
        return false;
    }
    return true;
}

function buscarRelacion(codigoEq, codigoCanal, listaRel) {
    for (var i = 0; i < listaRel.length; i++) {
        if (listaRel[i].Equicodi == codigoEq && listaRel[i].Canalcodi == codigoCanal) {
            return listaRel[i];
        }
    }

    return null;
}

function cargarTablaListaRelacion(listaRel, idElementoTabla, idElementDiv) {
    $("#" + idElementDiv + "").html('');
    var strHtml = '<table class="pretty tabla-search" id="' + idElementoTabla + '" style="width: 1000px;">';

    strHtml += '<thead>';
    strHtml += '<tr>';
    strHtml += '<th class="tbform-control">Unidad</th>';
    strHtml += '<th class="tbform-control">Empresa</th>';
    strHtml += '<th class="tbform-control">Ubicación</th>';
    strHtml += '<th class="tbform-control">Tipo de equipo</th>';
    strHtml += '<th class="tbform-control">Equipo</th>';
    strHtml += '<th class="tbform-control">Canal</th>';
    strHtml += '<th class="tbform-control">Valor Inverso</th>';
    strHtml += '<th class="tbform-control">Area</th>';
    strHtml += '<th class="tbform-control">Acción</th>';
    strHtml += '</tr>';
    strHtml += '</thead>';

    strHtml += '<tbody>';
    for (var i = 0; i < listaRel.length; i++) {
        strHtml += '<tr>';

        strHtml += '<td class="tbform-control">' + listaRel[i].Unidad + '</td>';
        strHtml += '<td class="tbform-control">' + listaRel[i].Empresa + '</td>';
        strHtml += '<td class="tbform-control">' + listaRel[i].Ubicacion + '</td>';
        strHtml += '<td class="tbform-control">' + listaRel[i].TipoEquipo + '</td>';
        strHtml += '<td class="tbform-control">' + listaRel[i].Equinomb + '</td>';
        strHtml += '<td class="tbform-control">' + listaRel[i].Canalnomb + '</td>';
        strHtml += '<td class="tbform-control">' + (listaRel[i].Ecanfactor > 0 ? "NO" : "SÍ") + '</td>';
        strHtml += '<td class="tbform-control">' + listaRel[i].Areaabrev + '</td>';

        strHtml += '<td class="tbform-control">';
        strHtml += '<input type="button" value="-" title="Agregar Relación" onclick="quitarRelacion(' + listaRel[i].Equicodi + ',' + listaRel[i].Canalcodi + ')" >';
        strHtml += '</td>';

        strHtml += '</tr>';
    }
    strHtml += '</tbody>';

    strHtml += '</table>';

    $("#" + idElementDiv + "").html(strHtml);
    $('#' + idElementoTabla).dataTable({
        "sPaginationType": "full_numbers",
        "destroy": "true",
        "bInfo": true,
        //"bLengthChange": false,
        "sDom": 'pt',
        "ordering": false,
        "iDisplayLength": 5
    });

    $("#" + idElementDiv + "").show();

    return strHtml;
}

function quitarRelacion(codigoEq, codigoCanal) {
    //generar nueva lista sin el elemento
    var listaNueva = [];
    var objElimi = {};
    for (var i = 0; i < LISTA_RELACION.length; i++) {
        if (LISTA_RELACION[i].Equicodi == codigoEq && LISTA_RELACION[i].Canalcodi == codigoCanal) {
            objElimi = LISTA_RELACION[i];
        } else {
            listaNueva.push(LISTA_RELACION[i]);
        }
    }

    LISTA_RELACION = listaNueva;

    cargarTablaListaRelacion(LISTA_RELACION, 'tb_relacion', 'divTablaRelacion');
}