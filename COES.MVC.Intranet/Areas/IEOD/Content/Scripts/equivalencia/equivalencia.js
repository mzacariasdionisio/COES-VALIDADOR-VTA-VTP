var controlador = siteRoot + 'IEOD/Configuracion/';
var ancho;
var empresas = [];
$(function () {
    ancho = $("#mainLayout").width() > 800 ? $("#mainLayout").width() : 800;

    $('#btnConsultar').on('click', function () {
        listarEquivalencia();
    });

    $('#btnExpotar').on('click', function () {
        exportarEquivalencia();
    });

    $("#btnManualUsuario").click(function () {
        window.location = controlador + 'DescargarManualUsuario';
    });

    $('#btnNuevaRelacion').on('click', function () {
        nuevaEquivalencia();
    });

    $('#cbEmpresa').on('change', function () {
        listarEquivalencia();
    });

    $('#cbCentral').on('change', function () {
        cargarEmpresasPrincipal();
        listarEquivalencia();
    });

    $('#cbMedida').on('change', function () {
        listarEquivalencia();
    });

    cargarEmpresasPrincipal();
    listarEquivalencia();
});

///////////////////////////
/// Consulta
///////////////////////////
function listarEquivalencia() {
    var origen = $('#cbCentral').val();

    var empresa = $('#cbEmpresa').val();
    if (empresa == "") empresa = "-1";

    var central = $('#cbCentral').val();
    if (central == "") central = "-1";

    var medida = $("#cbMedida").val();
    if (medida == "") medida = "-1";
    if (origen != 0) {
        $('#listado').html('');
        $.ajax({
            type: 'POST',
            url: controlador + 'ListarReporteEquivalencia',
            data: {
                idEmpresa: empresa,
                idCentral: central,
                medida: medida
            },
            dataType: 'json',
            success: function (result) {
                var ancho = $("#mainLayout").width() > 900 ? $("#mainLayout").width() - 30 : 900;

                $('#listado').html(result);
                var anchoReporte = $('#reporte').width();
                var scrollX = anchoReporte > ancho;
                $("#resultado").css("width", (scrollX ? ancho : anchoReporte) + 10 + "px");
                $('#reporte').dataTable({
                    "autoWidth": false,
                    "scrollX": scrollX,
                    "scrollY": "560px",
                    "scrollCollapse": false,
                    "sDom": 'ft',
                    "ordering": false,
                    paging: false,
                });

            },
            error: function (err) {
                alert('Ha ocurrido un error.');
            }
        });
    }
}

///////////////////////////
/// Nuevo
///////////////////////////
function nuevaEquivalencia() {
    var empresa = $('#cbEmpresa').val();
    if (empresa == "") empresa = "-1";
    var medida = $("#cbMedida").val();
    if (medida == "") medida = "-1";

    //    if (empresa != "-1") {
    $.ajax({
        type: 'POST',
        url: controlador + "NuevoEquivalencia",
        data: {
            idEmpresa: empresa,
            medida: medida
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
    //} else {
    //    alert("Seleccione una empresa y/o medida");
    //}
}


function registrarEquivalencia() {

    if (validarNuevoListaEquivalencia() && confirm('¿Está seguro que desea guardar la equivalencia?')) {
        $.ajax({
            type: 'POST',
            url: controlador + 'RegistrarEquivalencia',
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

function validarNuevoListaEquivalencia() {
    var msj = "";

    msj += LISTA_RELACION.length <= 0 ? "Debe crear 1 0 más Equivalencias" + "\n" : "";

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
        obj.Ptomedicodi = LISTA_RELACION[i].Ptomedicodi;
        obj.Canalcodi = LISTA_RELACION[i].Canalcodi;
        obj.Tipoinfocodi = LISTA_RELACION[i].Tipoinfocodi;
        obj.Pcanfactor = LISTA_RELACION[i].Pcanfactor;
        lista.push(obj);
    }

    return lista;
}

function validarNuevoEquivalencia() {
    var ptomedicion = $("#cbPtomedicion").val();
    var empresa = $('#cbTrEmpresa').val();
    var zona = $("#cbTrZona").val();
    var unidad = $("#cbTrUnidad").val();
    var canal = $("#cbTrCanal").val();
    var msj = "";

    msj += ptomedicion == "0" ? "Seleccionar Punto de medición" + "\n" : "";
    msj += empresa == "0" ? "Seleccionar Empresa" + "\n" : "";
    msj += zona == "0" ? "Seleccionar Zona" + "\n" : "";
    msj += unidad == "0" ? "Seleccionar Unidad de medida" + "\n" : "";
    msj += canal == "0" ? "Seleccionar Canal Scada" + "\n" : "";

    if (msj != "") {
        alert(msj);
        return false;
    }
    return true;
}

///////////////////////////
/// Ver
///////////////////////////
function verEquivalencia(canal, ptomedicion, tipoinfocodi) {
    $.ajax({
        type: 'POST',
        url: controlador + "VerEquivalencia",
        data: {
            canalcodi: canal,
            ptomedicodi: ptomedicion,
            tipoinfocodi: tipoinfocodi
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
function editarEquivalencia(canal, ptomedicion, tipoinfocodi) {
    $.ajax({
        type: 'POST',
        url: controlador + "EditarEquivalencia",
        data: {
            canalcodi: canal,
            ptomedicodi: ptomedicion,
            tipoinfocodi: tipoinfocodi
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
    var ptomedicodi = $("#ptomedicodiEdit").val();
    var tipoinfocodi = $("#tipoinfocodiEdit").val();
    var estado = $("#cbEstadoEdit").val();
    var check = $('#chkValorInversoEdit').is(':checked') ? -1 : 1;

    if (confirm('¿Está seguro que desea actualizar la equivalencia?')) {
        $.ajax({
            type: 'POST',
            url: controlador + 'ActualizarEquivalencia',
            dataType: 'json',
            data: {
                canalcodi: canalcodi,
                ptomedicodi: ptomedicodi,
                tipoinfocodi: tipoinfocodi,
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
function eliminarEquivalencia(canal, ptomedicion, tipoinfocodi) {

    if (confirm('¿Está seguro que desea eliminar la equivalencia?')) {
        $.ajax({
            type: 'POST',
            url: controlador + 'EliminarEquivalencia',
            dataType: 'json',
            data: {
                canalcodi: canal,
                ptomedicodi: ptomedicion,
                tipoinfocodi: tipoinfocodi
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
/// Exportar
///////////////////////////
function exportarEquivalencia() {
    var empresa = $('#cbEmpresa').val();
    if (empresa == "") empresa = "-1";

    var central = $('#cbCentral').val();
    if (central == "") central = "-1";

    var medida = $("#cbMedida").val();

    $.ajax({
        type: 'POST',
        url: controlador + 'GeneraReporteExcelEquivalencia',
        data: {
            idEmpresa: empresa,
            idCentral: central,
            medida: medida
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
/// Util
///////////////////////////
function cancelarEquivalencia() {
    $('#popupEditarEquivalencia').bPopup().close();
    $('#popupNuevaEquivalencia').bPopup().close();
    $('#nuevaEquivalencia').html("");
}

function cargarTipoPuntoMedicion() {
    var empresa = $('#cbEmpresa2').val();
    if (empresa == "") empresa = "-1";
    var famcodi = $('#cbTipoEquipoNuevo').val();
    var origen = $('#cbOrigen').val();


    $("#cbTipopto").empty();
    $('#cbTipopto').append($('<option></option>').val("0").html("-SELECCIONE-"));

    if (famcodi != "0") {
        $.ajax({
            type: 'POST',
            url: controlador + 'ListarTipoPtomedicion',
            data: {
                idEmpresa: empresa,
                origlectcodi: origen
            },
            dataType: 'json',
            success: function (result) {
                for (var item in result) {
                    $('#cbTipopto').append($('<option></option>').val(result[item].Tipoptomedicodi).html(result[item].Tipoptomedinomb));
                }

            },
            error: function (err) {
                alert('Ha ocurrido un error.');
            }
        });
    }
}

function cargarPuntoMedicion() {
    var empresa = $('#cbEmpresa2').val();
    if (empresa == "") empresa = "-1";
    var famcodi = $('#cbTipoEquipoNuevo').val();
    var origen = $('#cbOrigen').val();

    $("#cbPtomedicion").empty();
    $('#cbPtomedicion').append($('<option></option>').val("0").html("-SELECCIONE-"));

    if (famcodi != "0") {
        $.ajax({
            type: 'POST',
            url: controlador + 'ListarPtomedicion',
            data: {
                idEmpresa: empresa,
                famcodi: famcodi,
                origlectcodi: origen
            },
            dataType: 'json',
            success: function (result) {
                for (var item in result) {
                    $('#cbPtomedicion').append($('<option></option>').val(result[item].Ptomedicodi).html(result[item].Ptomedicodi + " " + result[item].Ptomedielenomb));
                }
                cargarCanal();
            },
            error: function (err) {
                alert('Ha ocurrido un error.');
            }
        });
    }
}

function cargarEmpresas() {
    var origen = $('#cbOrigen').val();
    if (origen == "") origen = "-1";

    $("#cbEmpresa2").empty();
    $('#cbEmpresa2').append($('<option></option>').val("0").html("-SELECCIONE-"));
    // $("#cbTrEmpresa").empty();
    $("#cbTipoEquipoNuevo").empty();
    $('#cbTipoEquipoNuevo').append($('<option></option>').val("").html("-SELECCIONE-"));
    $("#cbPtomedicion").empty();
    $('#cbPtomedicion').append($('<option></option>').val("0").html("-SELECCIONE-"));

    $.ajax({
        type: 'POST',
        url: controlador + 'ListarEmpresaXOrigenLectura',
        data: {
            origlectcodi: origen
        },
        dataType: 'json',
        success: function (result) {
            empresas.length = 0;

            for (var item in result) {
                $('#cbEmpresa2').append($('<option></option>').val(result[item].Emprcodi).html(result[item].Emprnomb));
                empresas.push({
                    sic: result[item].Emprcodi,
                    tr: result[item].Scadacodi
                });
            }

        },
        error: function (err) {
            alert('Ha ocurrido un error.:');
        }
    });
}

function cargarEmpresasPrincipal() {
    var origen = $('#cbCentral').val();
    if (origen == "") origen = "-1";

    $("#cbEmpresa").empty();
    $('#cbEmpresa').append($('<option></option>').val("-1").html("-TODOS-"));


    $.ajax({
        type: 'POST',
        url: controlador + 'ListarEmpresaXOrigenLectura',
        data: {
            origlectcodi: origen
        },
        dataType: 'json',
        success: function (result) {
            empresas.length = 0;

            for (var item in result) {
                $('#cbEmpresa').append($('<option></option>').val(result[item].Emprcodi).html(result[item].Emprnomb));
            }

        },
        error: function (err) {
            alert('Ha ocurrido un error.:');
        }
    });
}

function cargarTipoEquipo() {
    var origen = $('#cbOrigen').val();
    if (origen == "") origen = "-1";
    var empresa = $('#cbEmpresa2').val();
    if (empresa == "") empresa = "-1";
    $('#cbTrEmpresa').val();
    $("#cbTipoEquipoNuevo").empty();
    $('#cbTipoEquipoNuevo').append($('<option></option>').val("").html("-SELECCIONE-"));
    $.ajax({
        type: 'POST',
        url: controlador + 'ListarFamiliaXOrigenLecturaEmpresa',
        data: {
            origlectcodi: origen,
            emprcodi: empresa
        },
        dataType: 'json',
        success: function (result) {
            for (var item in result) {
                $('#cbTipoEquipoNuevo').append($('<option></option>').val(result[item].Famcodi).html(result[item].Famnomb));
            }

        },
        error: function (err) {
            alert('Ha ocurrido un error.:');
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
    var empresa = $('#cbTrEmpresa').val();

    $("#cbTrZona").empty();
    $('#cbTrZona').append($('<option></option>').val("0").html("-SELECCIONE-"));

    $("#cbTrCanal").empty();
    $('#cbTrCanal').append($('<option></option>').val("0").html("-SELECCIONE-"));
    console.log(empresa);
    if (empresa != null) {

        if (empresa != "0") {
            $.ajax({
                type: 'POST',
                url: controlador + 'ListarZona',
                data: {
                    idTrEmpresa: empresa
                },
                dataType: 'json',
                success: function (result) {
                    for (var item in result) {
                        $('#cbTrZona').append($('<option></option>').val(result[item].Zonacodi).html(result[item].Zonanomb));
                    }
                    cargarCanal();
                },
                error: function (err) {
                    alert('Ha ocurrido un error.');
                }
            });
        }
    }
}

function cargarUnidad() {
    var zona = $("#cbTrZona").val();

    $("#cbTrCanal").empty();
    $('#cbTrCanal').append($('<option></option>').val("0").html("-SELECCIONE-"));
    $("#cbMedidaNuevo").empty();
    $('#cbMedidaNuevo').append($('<option></option>').val("0").html("-SELECCIONE-"));
    if (zona != "0") {
        $.ajax({
            type: 'POST',
            url: controlador + 'ListarUnidadScada',
            data: {
                zonacodi: zona
            },
            dataType: 'json',
            success: function (result) {
                for (var item in result) {
                    $('#cbMedidaNuevo').append($('<option></option>').val(result[item].Tipoinfocodi).html(result[item].Canalunidad));
                }
                cargarCanal();
            },
            error: function (err) {
                alert('Ha ocurrido un error.');
            }
        });
    }
}

function cargarCanal() {
    empresa = $('#cbTrEmpresa').val();
    zona = $("#cbTrZona").val();
    idunidad = $("#cbMedidaNuevo").val();
    idunidad = $("#cbMedidaNuevo").val();
    unidad = $("#cbMedidaNuevo option[value=" + idunidad + "]").text();

    var arrayTxt = unidad.split('(');
    if (arrayTxt.length > 1) unidad = arrayTxt[0].trim();

    $("#cbTrCanal").empty();
    $('#cbTrCanal').append($('<option></option>').val("0").html("-SELECCIONE-"));

    if (empresa != "0" && zona != "0" && unidad != "") {
        $.ajax({
            type: 'POST',
            url: controlador + 'ListarCanal',
            data: {
                idTrEmpresa: empresa,
                idTrZona: zona,
                unidad: unidad
            },
            dataType: 'json',
            success: function (result) {
                for (var item in result) {
                    $('#cbTrCanal').append($('<option></option>').val(result[item].Canalcodi).html(result[item].Canalnomb));
                }
            },
            error: function (err) {
                alert('Ha ocurrido un error.');
            }
        });
    }
}

function buscarEmpresaTr(sic) {

    for (var i = 0; i < empresas.length; i++) {
        if (empresas[i].sic == sic) {
            return i;
        }
    }
    return 0;
}

function inicializarPopupNuevaEquivalencia() {
    APP_MOSTRAR_MODAL_EQUIPO_YA_CARGADO = 0;
    $('#busquedaEquipo').html('');

    $('#cbOrigen').on('change', function () {
        //cargarPuntoMedicion();
        cargarEmpresas();
    });
    $('#cbEmpresa2').on('change', function () {
        cargarTipoEquipo();
        idTr = buscarEmpresaTr($('#cbEmpresa2').val());
        $('#cbTrEmpresa').val(empresas[idTr].tr);
        cargarZona();
        $("#cbPtomedicion").empty();
        $('#cbPtomedicion').append($('<option></option>').val("0").html("-SELECCIONE-"));
    });
    $('#cbTipoEquipoNuevo').on('change', function () {
        cargarPuntoMedicion();
    });

    $('#cbTrEmpresa').on('change', function () {
        cargarZona();
    });
    $('#cbTrZona').on('change', function () {
        cargarUnidad();
    });

    $('#btnGrabarEquivalencia').click(function () {
        registrarEquivalencia();
    });

    $("#btnCancelarEquivalencia").click(function () {
        cancelarEquivalencia();
    });
    $('#cbMedidaNuevo').on('change', function () {
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

    $("#btnFiltroPto").unbind();
    $("#btnFiltroPto").click(function () {
        actualizaFiltrosPto();
    });
    $("#btnFiltroCanalSca").unbind();
    $("#btnFiltroCanalSca").click(function () {
        actualizaFiltrosCanalSca();
    });
}

/// Tabla Vista previa
function agregarEquivalencia() {
    origen = parseInt($("#cbOrigen").val()) || 0;
    pto = parseInt($("#cbPtomedicion").val()) || 0;
    unidad = parseInt($("#cbMedidaNuevo").val()) || 0;
    canal = parseInt($("#cbTrCanal").val()) || 0;
    tipoequipo = parseInt($("#cbTipoEquipoNuevo").val()) || 0;
    empresa = parseInt($("#cbEmpresa2").val()) || 0;
    var check = $('#chkValorInverso').is(':checked') ? -1 : 1;

    if (validarNuevoEquivalencia()) {
        var obj = {};
        obj.Origen = $("#cbOrigen option[value=" + origen + "]").text();
        obj.Unidad = $("#cbMedidaNuevo option[value=" + unidad + "]").text();
        obj.Empresa = $("#cbEmpresa2 option[value=" + empresa + "]").text();
        obj.TipoEquipo = $("#cbTipoEquipoNuevo option[value=" + tipoequipo + "]").text();
        obj.Ptomedicodi = pto;
        obj.Canalcodi = canal;
        obj.Ptonomb = $("#cbPtomedicion option[value=" + pto + "]").text();
        obj.Canalnomb = $("#cbTrCanal option[value=" + canal + "]").text();
        obj.Tipoinfocodi = unidad;
        obj.Pcanfactor = check;
        if (validarNoDuplicadoNuevoEquivalencia(obj)) {
            LISTA_RELACION.push(obj);

            //$("#iEquipoNuevo").val('');
            //$("#cbEquipoNuevo").val(-1);
            //$("#icbTrCanalNuevo").val('');
            //$("#cbTrCanalNuevo").val(-1);
        }
    }

    cargarTablaListaRelacion(LISTA_RELACION, 'tb_relacion', 'divTablaRelacion');
}

function cargarTablaListaRelacion(listaRel, idElementoTabla, idElementDiv) {
    $("#" + idElementDiv + "").html('');
    var strHtml = '<table class="pretty tabla-search" id="' + idElementoTabla + '" style="width: 1150px;">';

    strHtml += '<thead>';
    strHtml += '<tr>';
    strHtml += '<th class="tbform-control">Origen</th>';
    strHtml += '<th class="tbform-control">Empresa</th>';
    strHtml += '<th class="tbform-control">Tipo de equipo</th>';
    strHtml += '<th class="tbform-control">Punto de Medicion</th>';
    strHtml += '<th class="tbform-control">Canal</th>';
    strHtml += '<th class="tbform-control">Unidad</th>';
    strHtml += '<th class="tbform-control">Valor Inverso</th>';
    strHtml += '<th class="tbform-control">Acción</th>';
    strHtml += '</tr>';
    strHtml += '</thead>';

    strHtml += '<tbody>';
    for (var i = 0; i < listaRel.length; i++) {
        var txtValorInverso = listaRel[i].Pcanfactor == -1 ? "SÍ" : "NO";

        strHtml += '<tr>';
        strHtml += '<td class="tbform-control">' + listaRel[i].Origen + '</td>';
        strHtml += '<td class="tbform-control">' + listaRel[i].Empresa + '</td>';
        strHtml += '<td class="tbform-control">' + listaRel[i].TipoEquipo + '</td>';
        strHtml += '<td class="tbform-control">' + listaRel[i].Ptonomb + '</td>';
        strHtml += '<td class="tbform-control">' + listaRel[i].Canalnomb + '</td>';
        strHtml += '<td class="tbform-control">' + listaRel[i].Unidad + '</td>';
        strHtml += '<td class="tbform-control">' + txtValorInverso + '</td>';
        strHtml += '<td class="tbform-control">';
        strHtml += '<input type="button" value="-" title="Eliminar Relación" onclick="quitarRelacion(' + listaRel[i].Ptomedicodi + ',' + listaRel[i].Canalcodi + ')" >';
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

function validarNuevoEquivalencia() {
    var pto = parseInt($("#cbPtomedicion").val()) || 0;
    var canal = parseInt($("#cbTrCanal").val()) || 0;
    var msj = "";

    msj += pto <= 0 ? "Debe seleccionar Punto Medición" + "\n" : "";
    msj += canal <= 0 ? "Debe seleccionar Canal Scada" + "\n" : "";

    if (msj != "") {
        alert(msj);
        return false;
    }
    return true;
}

function validarNoDuplicadoNuevoEquivalencia(obj) {
    var msj = "";

    msj += buscarRelacion(obj.Ptomedicodi, obj.Canalcodi, LISTA_RELACION) ? "Ya existe el elemento en la lista" + "\n" : "";

    if (msj != "") {
        alert(msj);
        return false;
    }
    return true;
}

function buscarRelacion(codigoPto, codigoCanal, listaRel) {
    for (var i = 0; i < listaRel.length; i++) {
        if (listaRel[i].Ptomedicodi == codigoPto && listaRel[i].Canalcodi == codigoCanal) {
            return listaRel[i];
        }
    }

    return null;
}

function quitarRelacion(codigoPto, codigoCanal) {
    //generar nueva lista sin el elemento
    var listaNueva = [];
    var objElimi = {};
    for (var i = 0; i < LISTA_RELACION.length; i++) {
        if (LISTA_RELACION[i].Ptomedicodi == codigoPto && LISTA_RELACION[i].Canalcodi == codigoCanal) {
            objElimi = LISTA_RELACION[i];
        } else {
            listaNueva.push(LISTA_RELACION[i]);
        }
    }

    LISTA_RELACION = listaNueva;

    cargarTablaListaRelacion(LISTA_RELACION, 'tb_relacion', 'divTablaRelacion');
}

function actualizaFiltrosPto() {

    var ptomed = parseInt($('#txtFiltroPo').val()) || 0;
    if (ptomed > 0) {
        $.ajax({
            type: 'POST',
            url: controlador + 'GetPtoMedicion',
            data: {
                ptomedicodi: ptomed
            },
            dataType: 'json',
            success: function (evt) {
                if (evt.Resultado != "-1") {

                    $('#cbOrigen').val(evt.PtoMedicion.Origlectcodi);

                    $("#cbEmpresa2").empty();
                    $('#cbEmpresa2').append($('<option></option>').val("0").html("-SELECCIONE-"));
                    $("#cbTipoEquipoNuevo").empty();
                    $('#cbTipoEquipoNuevo').append($('<option></option>').val("").html("-SELECCIONE-"));
                    $("#cbPtomedicion").empty();
                    $('#cbPtomedicion').append($('<option></option>').val("0").html("-SELECCIONE-"));

                    for (var item in evt.ListaEmpresas) {
                        $('#cbEmpresa2').append($('<option></option>').val(evt.ListaEmpresas[item].Emprcodi).html(evt.ListaEmpresas[item].Emprnomb));
                    }
                    $('#cbEmpresa2').val(evt.PtoMedicion.Emprcodi);
                    for (var itemfam in evt.ListaFamilia) {
                        $('#cbTipoEquipoNuevo').append($('<option></option>').val(evt.ListaFamilia[itemfam].Famcodi).html(evt.ListaFamilia[itemfam].Famnomb));
                    }
                    $('#cbTipoEquipoNuevo').val(evt.PtoMedicion.Famcodi);

                    for (var itempto in evt.ListaPtomedicion) {
                        $('#cbPtomedicion').append($('<option></option>').val(evt.ListaPtomedicion[itempto].Ptomedicodi).html(evt.ListaPtomedicion[itempto].Ptomedicodi + " " + evt.ListaPtomedicion[itempto].Ptomedielenomb));
                    }
                    $('#cbPtomedicion').val(evt.PtoMedicion.Ptomedicodi);

                }
                else {
                    alert("Pto de medición ingresado no existe!");
                }

            },
            error: function (err) {
                alert('Ha ocurrido un error.:');
            }
        });
    } else {
        alert("Debe ingresar un código de punto de medición válido.");
    }
}

function actualizaFiltrosCanalSca() {
    $("#chkValorInverso").prop("checked", false);

    var canalcodi = parseInt($('#txtFiltroSca').val()) || 0;
    if (canalcodi > 0) {
        $.ajax({
            type: 'POST',
            url: controlador + 'GetTrCanalSp7',
            data: {
                canalcodi: canalcodi
            },
            dataType: 'json',
            success: function (evt) {
                if (evt.Resultado != "-1" && evt.TrCanal != null) {

                    $('#cbTrEmpresa').val(evt.TrCanal.Emprcodi);

                    $("#cbTrZona").empty();
                    $('#cbTrZona').append($('<option></option>').val("0").html("-SELECCIONE-"));

                    $("#cbTrCanal").empty();
                    $('#cbTrCanal').append($('<option></option>').val("0").html("-SELECCIONE-"));

                    $("#cbMedidaNuevo").empty();
                    $('#cbMedidaNuevo').append($('<option></option>').val("0").html("-SELECCIONE-"));



                    for (var item in evt.ListaTrZona) {
                        $('#cbTrZona').append($('<option></option>').val(evt.ListaTrZona[item].Zonacodi).html(evt.ListaTrZona[item].Zonanomb));
                    }
                    $('#cbTrZona').val(evt.TrCanal.Zonacodi);
                    for (var item in evt.ListarUnidadPorZona) {
                        $('#cbMedidaNuevo').append($('<option></option>').val(evt.ListarUnidadPorZona[item].Tipoinfocodi).html(evt.ListarUnidadPorZona[item].Canalunidad));
                    }
                    $('#cbMedidaNuevo').val(evt.TrCanal.Tipoinfocodi);
                    for (var item in evt.ListTrCanal) {
                        $('#cbTrCanal').append($('<option></option>').val(evt.ListTrCanal[item].Canalcodi).html(evt.ListTrCanal[item].Canalnomb));
                    }
                    $('#cbTrCanal').val(evt.TrCanal.Canalcodi);
                }
                else {
                    alert("¡Canal Scada ingresado no existe!");
                }

            },
            error: function (err) {
                alert('Ha ocurrido un error.:');
            }
        });
    } else {
        alert("Debe ingresar un código de canal válido.");
    }
}