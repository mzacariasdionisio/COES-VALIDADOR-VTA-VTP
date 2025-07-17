var controlador = siteRoot + 'migraciones/parametro/';
var OPCION_VER = 1;
var OPCION_EDITAR = 2;
var OPCION_NUEVO = 3;
var ancho = 900;
var DATA_USUARIO = null;
var DATA_AREA = null;
var DATA_AREA_SELECCIONADO = null;
var DATA_USUARIO_SELECCIONADO = null;

$(function () {
    $('#cbCategoria').change(function () {
        cargarTableConcepto();
    });

    $('#btnConsultar').on('click', function () {
        cargarTableConcepto();
    });

    ancho = $('#mainLayout').width() - 50;

    cargarTableConcepto();

    var data = $("#hfListaUsuario").val();
    DATA_USUARIO = JSON.parse(data);

    var data2 = $("#hfListaArea").val();
    var areaTodos = {
        Areacode: -1, Areaname: 'TODOS', Areaabrev: 'TODOS'
    };
    DATA_AREA = [];
    DATA_AREA.push(areaTodos);
    DATA_AREA = DATA_AREA.concat(JSON.parse(data2));
});

function cargarTableConcepto() {
    $('#listado').html('');

    $.ajax({
        type: "POST",
        url: controlador + "VisualizacionConcepto",
        data: {
            catecodi: $("#cbCategoria").val()
        },
        global: false,
        success: function (result) {
            $('#listado').css("width", ancho + "px");
            $('#listado').html(result);

            $('#tabla_pr_concepto').dataTable({
                "sPaginationType": "full_numbers",
                "destroy": "true",
                "bInfo": true,
                //"bLengthChange": false,
                //"sDom": 'fpt',
                "ordering": true,
                "iDisplayLength": 15
            });
        },
        error: function (req, status, error) {
            alert("Ha ocurrido un error.");
        }
    });
}

function seleccionarPrConcepto(Concepcodi, Catenomb, Concepabrev, Concepdesc, Concepunid, Conceptipo) {
    $("#formconcepto").html(convertirStringToHtml(Concepdesc));
    $("#hfCodigoConcepto").val(Concepcodi);
}

///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
/// Usuarios y Areas
///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

function verAreaUsuario(Concepcodi, Catenomb, Concepabrev, Concepdesc, Concepunid, Conceptipo) {
    seleccionarPrConcepto(Concepcodi, Catenomb, Concepabrev, Concepdesc, Concepunid, Conceptipo);
    formularioAreaUsuario(Concepcodi, OPCION_VER);
}

function editarAreaUsuario(Concepcodi, Catenomb, Concepabrev, Concepdesc, Concepunid, Conceptipo) {
    seleccionarPrConcepto(Concepcodi, Catenomb, Concepabrev, Concepdesc, Concepunid, Conceptipo);
    formularioAreaUsuario(Concepcodi, OPCION_EDITAR);
}

function formularioAreaUsuario(id, opcion) {
    DATA_AREA_SELECCIONADO = null;
    DATA_USUARIO_SELECCIONADO = null;

    $.ajax({
        type: 'POST',
        dataType: 'json',
        traditional: true,
        url: controlador + "ListaConfiguracionXConcepto",
        data: {
            concepcodi: id
        },
        success: function (obj) {
            $('#mensaje').css("display", "none");

            inicializarFormulario(opcion, id, obj);

            setTimeout(function () {
                $('#popupUsuario').bPopup({
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

function inicializarFormulario(opcion, id, obj) {
    DATA_AREA_SELECCIONADO = obj.ListaAreaConcepto;
    DATA_USUARIO_SELECCIONADO = obj.ListaUsuariocodi;

    generarTablaUsuario(opcion);
    generarTablaArea(opcion);

    switch (opcion) {
        case OPCION_VER:
            $("#btnUsuarioGuardar").hide();
            $("#btnUsuarioCancelar").show();
            $("#btnUsuarioCancelar").val("Cerrar");
            break;
        case OPCION_EDITAR:
            $("#btnUsuarioCancelar").val("Cancelar");
            $("#btnUsuarioGuardar").show();
            $("#btnUsuarioCancelar").show();
            break;
    }

    $("#btnUsuarioGuardar").unbind();
    $('#btnUsuarioGuardar').click(function () {
        guardarConfiguracion();
    });

    $("#btnUsuarioCancelar").unbind();
    $('#btnUsuarioCancelar').click(function () {
        cerrarPopupUsuario();
    });
}

function cerrarPopupUsuario() {
    $('#popupUsuario').bPopup().close();
}

function guardarConfiguracion() {
    var entity = getObjetoJsonUsuarioConcepto();
    if (confirm('¿Desea guardar la configuración del concepto?')) {
        var msj = validarUsuarioConcepto(entity);

        if (msj == "") {
            $.ajax({
                type: 'POST',
                dataType: 'json',
                traditional: true,
                url: controlador + 'VisualizacionConceptoGuardar',
                data: {
                    concepcodi: entity.concepto,
                    listaUsuarios: entity.listaUsuario
                },
                cache: false,
                success: function (result) {
                    if (result.Resultado == '-1') {
                        alert('Ha ocurrido un error: ' + result.Mensaje);
                    } else {
                        alert("Se guardó correctamente la configuración");
                        cerrarPopupUsuario();
                        cargarTableConcepto();
                    }
                },
                error: function (err) {
                    alert('Ha ocurrido un error.');
                }
            });
        } else {
            alert(msj);
        }
    }
}

function getObjetoJsonUsuarioConcepto() {
    var obj = {};
    var listaUsuario = [];
    $("input[name=check_usuario_cnp]:checked").each(function () {
        var codigoUsuario = parseInt($(this).val()) || 0;
        listaUsuario.push(codigoUsuario);
    });
    DATA_USUARIO_SELECCIONADO = listaUsuario;

    obj.concepto = parseInt($("#hfCodigoConcepto").val()) || 0;
    obj.listaUsuario = listaUsuario;

    return obj;
}

function validarUsuarioConcepto(obj) {
    var msj = "";

    if (obj.concepto <= 0) {
        msj += "Debe seleccionar un concepto";
    }

    return msj;
}

function buscarUsuario(usercode) {
    if (DATA_USUARIO_SELECCIONADO != null) {
        for (var i = 0; i < DATA_USUARIO_SELECCIONADO.length; i++) {
            if (DATA_USUARIO_SELECCIONADO[i] == usercode) {
                return DATA_USUARIO_SELECCIONADO[i];
            }
        }
    }

    return null;
}

function generarTablaUsuario(opcion) {
    $("#div_tabla_usuario").html("");
    var strHtml = '';

    if (DATA_USUARIO == null || DATA_USUARIO.length == 0) {
        DATA_USUARIO = [];
        $("#div_tabla_usuario").hide();
    } else {
        $("#div_tabla_usuario").show();
    }
    var ancho = opcion != OPCION_VER ? 234 : 293;
    strHtml += '<table id="tabla_usuario" class="pretty tabla-icono" style="display: block; margin-top: 15px;width: auto">';
    strHtml += '<thead>';
    strHtml += '<tr>';
    if (opcion != OPCION_VER) {
        strHtml += '<th style="width: 50px" class="tbform-control"></th>';
    }
    strHtml += '<th style="width: 200px" class="tbform-control">Usuario</th>';
    strHtml += '<th style="width: 50px" class="tbform-control">Área abrev</th>';
    strHtml += '<th style="width: ' + ancho + 'px" class="tbform-control">Área</th>';
    strHtml += '</tr>';
    strHtml += '</thead>';

    strHtml += '<tbody>';
    for (var i = 0; i < DATA_USUARIO.length; i++) {
        if (opcion == OPCION_VER) {
            if (buscarUsuario(DATA_USUARIO[i].UserCode) != null) {
                strHtml += '<tr>';
                strHtml += '<td class="tbform-control usuario">' + DATA_USUARIO[i].UsernName + '</td>';
                strHtml += '<td class="tbform-control area_abrev">' + DATA_USUARIO[i].AreaAbrev + '</td>';
                strHtml += '<td class="tbform-control area_name">' + DATA_USUARIO[i].AreaName + '</td>';
                strHtml += '</tr>';
            }
        } else {
            strHtml += '<tr>';
            var check = buscarUsuario(DATA_USUARIO[i].UserCode) != null ? "checked" : "";
            strHtml += '<td class="tbform-control">';
            strHtml += '<input type="checkbox" name="check_usuario_cnp" id="check_usuario' + DATA_USUARIO[i].UserCode + '" value="' + DATA_USUARIO[i].UserCode + '" ' + check + ' />';
            strHtml += '<input type="hidden" id="fila_usuario' + DATA_USUARIO[i].UserCode + '" value="' + DATA_USUARIO[i].UserCode + '" />';
            strHtml += '<input type="hidden" name="check_usuario_area_cnp" id="fila_usuario_area' + DATA_USUARIO[i].UserCode + '" value="' + DATA_USUARIO[i].AreaCode + '" />';
            strHtml += '</td>';
            strHtml += '<td class="tbform-control usuario">' + DATA_USUARIO[i].UsernName + '</td>';
            strHtml += '<td class="tbform-control area_abrev">' + DATA_USUARIO[i].AreaAbrev + '</td>';
            strHtml += '<td class="tbform-control area_name">' + DATA_USUARIO[i].AreaName + '</td>';
            strHtml += '</tr>';
        }
    }
    strHtml += '</tbody>';
    strHtml += '</table>';

    $("#div_tabla_usuario").html(strHtml);

    $('#tabla_usuario').dataTable({
        "sPaginationType": "full_numbers",
        "destroy": "true",
        "bInfo": false,
        "bLengthChange": false,
        "sDom": 'ft',
        "ordering": false,
        "iDisplayLength": -1
    });
}

function buscarArea(areacode) {
    if (areacode == -1) {
        if (DATA_AREA != null && DATA_USUARIO != null && DATA_USUARIO_SELECCIONADO != null
            && DATA_USUARIO.length > 0 && DATA_USUARIO.length == DATA_USUARIO_SELECCIONADO.length) {
            return "TODOS";
        }
    }

    if (DATA_AREA_SELECCIONADO != null) {
        for (var i = 0; i < DATA_AREA_SELECCIONADO.length; i++) {
            if (DATA_AREA_SELECCIONADO[i].Areacode == areacode) {
                return DATA_AREA_SELECCIONADO[i];
            }
        }
    }

    return null;
}

function generarTablaArea(opcion) {
    $("#tabla_area").html("");
    var strHtml = '';

    if (DATA_AREA == null || DATA_AREA.length == 0) {
        DATA_AREA = [];
        $("#tabla_area").hide();
    } else {
        $("#tabla_area").show();
    }

    strHtml += '<tbody>';
    var m = 0;
    for (var i = 0; i < DATA_AREA.length; i++) {
        if (opcion == OPCION_VER) {
            if (buscarArea(DATA_AREA[i].Areacode) != null) {
                strHtml += '<tr>';
                if (m == 0) {
                    strHtml += '<td class="tbform-label" style="vertical-align: top;">Área:<td>';
                } else {
                    strHtml += '<td class=""><td>';
                }
                strHtml += '<td class="">' + DATA_AREA[i].Areaabrev + '</td>';
                strHtml += '</tr>';
                m++;
            }
        } else {
            strHtml += '<tr>';
            if (i == 0) {
                strHtml += '<td class="tbform-label" style="vertical-align: top;">Área:<td>';
            } else {
                strHtml += '<td class=""><td>';
            }

            var check = buscarArea(DATA_AREA[i].Areacode) != null ? "checked" : "";
            strHtml += '<td class="">';
            strHtml += '<input type="checkbox" name="check_area_cnp" id="check_area' + DATA_AREA[i].Areacode + '" value="' + DATA_AREA[i].Areacode + '" ' + check + ' />';
            strHtml += '<input type="hidden" id="fila_area' + DATA_AREA[i].Areacode + '" value="' + DATA_AREA[i].Areacode + '" />';
            strHtml += '</td>';
            strHtml += '<td class="">' + DATA_AREA[i].Areaabrev + '</td>';
            strHtml += '</tr>';
        }
    }
    strHtml += '</tbody>';

    $("#tabla_area").html(strHtml);

    $('input[name=check_area_cnp]:checkbox').unbind();
    $('input[name=check_area_cnp]:checkbox').change(function () {
        actualizarTablaXArea($(this).val(), $(this).prop("checked"));
    });
}

function actualizarTablaXArea(inarea, check) {
    if (inarea == -1) {
        $('input[name=check_area_cnp]:checkbox').each(function () {
            if (check) {
                $(this).prop('checked', true).attr('checked', 'checked');
            } else {
                $(this).prop('checked', false).removeAttr('checked');
            }
        });
    }

    $("input[name=check_usuario_cnp]").each(function () {
        var codigoUsuario = parseInt($(this).val()) || 0;

        if (inarea == -1) {
            if (check) {
                $(this).prop('checked', true).attr('checked', 'checked');
            } else {
                $(this).prop('checked', false).removeAttr('checked');
            }
        } else {
            var area = $("#fila_usuario_area" + codigoUsuario).val();
            if (area == inarea) {
                if (check) {
                    $(this).prop('checked', true).attr('checked', 'checked');
                } else {
                    $(this).prop('checked', false).removeAttr('checked');
                }
            }
        }

    });
}

///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
/// Util
///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

function mostrarError() {
    $('#textoMensaje').css("display", "block");
    $('#textoMensaje').removeClass();
    $('#textoMensaje').addClass('action-alert');
    $('#textoMensaje').text("Ha ocurrido un error");
}

function getCodigoEmpresa() {
    return parseInt($("#cbEmpresa").val()) || 0;
}

function getCodigoCategoria() {
    return parseInt($("#cbCategoria").val()) || 0;
}

function getEstado() {
    return "S";
}

function convertirStringToHtml(str) {
    if (str != undefined && str != null) {
        //return str.replace(/&/g, "&amp;").replace(/>/g, "&gt;").replace(/</g, "&lt;").replace(/"/g, "&quot;");
        return str.replace(/>/g, "&gt;").replace(/</g, "&lt;").replace(/"/g, "&quot;");
    }

    return "";
}