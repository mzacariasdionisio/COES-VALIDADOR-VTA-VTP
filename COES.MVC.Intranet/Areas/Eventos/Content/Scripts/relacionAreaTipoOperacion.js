var controlador = siteRoot + 'eventos/operacionesvarias/';
var ancho = 900;
var OPCION_VER = 1;
var OPCION_EDITAR = 2;
var OPCION_NUEVO = 3;
var DATA_AREA = null;
var DATA_AREA_SELECCIONADO = null;

$(function () {
    ancho = $('#mainLayout').width() - 50;

    cargarTableSubcausacodi();

    var data2 = $("#hfListaArea").val();
    var areaTodos = {
        Areacode: -1, Areaname: 'TODOS', Areaabrev: 'TODOS'
    };
    DATA_AREA = [];
    DATA_AREA.push(areaTodos);
    DATA_AREA = DATA_AREA.concat(JSON.parse(data2));

});

function cargarTableSubcausacodi() {
    //$('#listado').html('');

    $.ajax({
        type: "POST",
        url: controlador + "ListadoSubcausa",
        data: {
        },
        global: false,
        success: function (result) {
            //$('#listado').css("width", ancho + "px");
            $('#listado').html(result);

            $('#tabla_subcausa').dataTable({
                "sPaginationType": "full_numbers",
                "destroy": "true",
                "bInfo": true,
                //"bLengthChange": false,
                //"sDom": 'fpt',
                "ordering": true,
                "iDisplayLength": 15,
                "columnDefs": [{
                    "targets": [0],
                    "bSortable": false
                }],
                "order": [[3, "asc"]]
            });

        },
        error: function (req, status, error) {
            alert("Ha ocurrido un error.");
        }
    });
}

///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
/// Usuarios y Areas
///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

function seleccionarSubcausa(Subcausacodi, Subcausadesc, Configurado) {
    $("#formTipoOperacion").html(convertirStringToHtml(Subcausadesc));
    $("#hfCodigoSubcausa").val(Subcausacodi);
}

function verAreaUsuario(Subcausacodi, Subcausadesc, Configurado) {
    seleccionarSubcausa(Subcausacodi, Subcausadesc, Configurado);
    formularioAreaUsuario(Subcausacodi, OPCION_VER, Configurado);
}

function editarAreaUsuario(Subcausacodi, Subcausadesc, Configurado) {
    seleccionarSubcausa(Subcausacodi, Subcausadesc, Configurado);
    formularioAreaUsuario(Subcausacodi, OPCION_EDITAR, Configurado);
}

function formularioAreaUsuario(id, opcion, tieneConfiguracion) {
    DATA_AREA_SELECCIONADO = null;

    $.ajax({
        type: 'POST',
        dataType: 'json',
        traditional: true,
        url: controlador + "ListaConfiguracionXSubcausa",
        data: {
            subcausacodi: id
        },
        success: function (obj) {
            $('#mensaje').css("display", "none");

            inicializarFormulario(opcion, id, obj, tieneConfiguracion != "NO");

            setTimeout(function () {
                $('#popupRelacion').bPopup({
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

function inicializarFormulario(opcion, id, obj, tieneConfiguracion) {
    DATA_AREA_SELECCIONADO = obj.ListaRelacionAreaSubcausa;

    switch (opcion) {
        case OPCION_VER:
            $("#btnRelacionGuardar").hide();
            $("#btnRelacionCancelar").show();
            $("#btnRelacionCancelar").val("Cerrar");
            break;
        case OPCION_EDITAR:
            $("#btnRelacionCancelar").val("Cancelar");
            $("#btnRelacionGuardar").show();
            $("#btnRelacionCancelar").show();

            if ((DATA_AREA_SELECCIONADO == null || DATA_AREA_SELECCIONADO.length == 0) && !tieneConfiguracion) {
                DATA_AREA_SELECCIONADO = DATA_AREA;
            }

            break;
    }

    generarTablaArea(opcion);

    $("#btnRelacionGuardar").unbind();
    $('#btnRelacionGuardar').click(function () {
        guardarConfiguracion();
    });

    $("#btnRelacionCancelar").unbind();
    $('#btnRelacionCancelar').click(function () {
        cerrarPopupUsuario();
    });
}

function cerrarPopupUsuario() {
    $('#popupRelacion').bPopup().close();
}

function guardarConfiguracion() {
    var entity = getObjetoJsonUsuarioConcepto();
    if (confirm('¿Desea guardar la Relación entre Área usuario y Tipo de Operación?')) {
        var msj = validarUsuarioConcepto(entity);

        if (msj == "") {
            $.ajax({
                type: 'POST',
                dataType: 'json',
                traditional: true,
                url: controlador + 'ConfiguracionAreaSubcausaGuardar',
                data: {
                    subcausacodi: entity.subcausacodi,
                    listaArea: entity.listaArea
                },
                cache: false,
                success: function (result) {
                    if (result.Resultado == '-1') {
                        alert('Ha ocurrido un error: ' + result.Mensaje);
                    } else {
                        alert("Se guardó correctamente la configuración");
                        cargarTableSubcausacodi();
                        cerrarPopupUsuario();
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
    var listaArea = [];
    $("input[name=check_area_cnp]:checked").each(function () {
        var codigoArea = parseInt($(this).val()) || 0;
        listaArea.push(codigoArea);
    });
    DATA_AREA_SELECCIONADO = listaArea;

    obj.subcausacodi = parseInt($("#hfCodigoSubcausa").val()) || 0;
    obj.listaArea = listaArea;

    return obj;
}

function validarUsuarioConcepto(obj) {
    var msj = "";

    if (obj.concepto <= 0) {
        msj += "Debe seleccionar un concepto";
    }

    return msj;
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
            if (buscarArea(DATA_AREA[i].Areacode) != null && DATA_AREA[i].Areacode != -1) {
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

function buscarArea(areacode) {
    if (areacode == -1) {
        if (DATA_AREA != null && DATA_AREA_SELECCIONADO != null
            && DATA_AREA_SELECCIONADO.length > 0 && DATA_AREA.length == DATA_AREA_SELECCIONADO.length) {
            return "TODOS";
        }
    } else {
        if (DATA_AREA_SELECCIONADO != null) {
            for (var i = 0; i < DATA_AREA_SELECCIONADO.length; i++) {
                if (DATA_AREA_SELECCIONADO[i].Areacode == areacode) {
                    return DATA_AREA_SELECCIONADO[i];
                }
            }
        }
    }

    return null;
}

function actualizarTablaXArea(inarea, check) {
    $("input[name=check_area_cnp]").each(function () {
        var codigoArea = parseInt($(this).val()) || 0;

        if (inarea == -1) {
            if (check) {
                $(this).prop('checked', true).attr('checked', 'checked');
            } else {
                $(this).prop('checked', false).removeAttr('checked');
            }
        } else {
            var area = $("#fila_area" + codigoArea).val();
            if (area == inarea) {
                if (check) {
                    $(this).prop('checked', true).attr('checked', 'checked');
                } else {
                    $(this).prop('checked', false).removeAttr('checked');
                }
            }

            //Verificar
            var listaArea = [];
            var areaTodos = $("#check_area" + -1);
            $("input[name=check_area_cnp]:checked").each(function () {
                var codigoArea = parseInt($(this).val()) || 0;
                if (codigoArea != -1) {
                    listaArea.push(codigoArea);
                }
            });

            if (listaArea.length == DATA_AREA.length - 1) {
                areaTodos.prop('checked', true).attr('checked', 'checked');
            } else {
                areaTodos.prop('checked', false).removeAttr('checked');
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

function convertirStringToHtml(str) {
    if (str != undefined && str != null) {
        //return str.replace(/&/g, "&amp;").replace(/>/g, "&gt;").replace(/</g, "&lt;").replace(/"/g, "&quot;");
        return str.replace(/>/g, "&gt;").replace(/</g, "&lt;").replace(/"/g, "&quot;");
    }

    return "";
}